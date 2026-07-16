using System;
using System.Collections.Generic;
using System.Collections.Concurrent;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.IO;
using Newtonsoft.Json;

using EFEM.Defines.Common;

namespace EFEM.MaterialTracking.CarrierStorage
{
    // 파일을 임시 파일(Guid)에 기록 성공 시 -> 기록해야할 키.json 파일에 기록 및 bak 파일 남김(이전 데이터)
    public sealed class JsonCarrierStorage : ICarrierStorage, IDisposable
    {
        #region <Constructors>
        public JsonCarrierStorage(string activePath, int maxParallelIO = 6)
        {
            if (string.IsNullOrWhiteSpace(activePath))
                throw new ArgumentNullException(nameof(activePath));

            _activePath = activePath;

            if (false == Directory.Exists(_activePath))
            {
                Directory.CreateDirectory(_activePath);
            }
            _carrierEventListners = new List<ICarrierEventObserver>();

            _ioThrottle = new SemaphoreSlim(maxParallelIO, maxParallelIO);
        }
        #endregion </Constructors>

        #region <Fields>
        private readonly string _activePath;    // 파일 저장소 경로
        private readonly SemaphoreSlim _ioThrottle;     // 동시에 디스크 쓰기 제한
        private readonly MonitorKeyedLocker _keyedLocker = new MonitorKeyedLocker();    // 자재 키 개별 락
        private readonly List<ICarrierEventObserver> _carrierEventListners;
        private volatile bool _disposed;
        
        private static readonly JsonSerializerSettings _jsonSettings = new JsonSerializerSettings
        {
            Formatting = Formatting.Indented,
            NullValueHandling = NullValueHandling.Ignore,
            // 열거형은 이름으로 저장. AllowIntegerValues=false 로 정수 토큰(레거시 ordinal)을 조용히
            // 해석하지 않는다(정수 데이터는 1회 변환기가 이름으로 선변환해야 함).
            Converters = new List<JsonConverter>
            {
                new Newtonsoft.Json.Converters.StringEnumConverter { AllowIntegerValues = false }
            }
        };
        #endregion </Fields>

        #region <Methods>

        #region <Interface>
        public void RegisterListner(ICarrierEventObserver carrierEvent)
        {
            _carrierEventListners.Add(carrierEvent);
        }
        public void InitializeStorage()
        {
            if (_disposed) 
                throw new ObjectDisposedException(nameof(JsonCarrierStorage));

            Directory.CreateDirectory(_activePath);
        }
        public bool LoadDataFromStorage(out List<CarrierItem> dataFromStorage)
        {
            dataFromStorage = new List<CarrierItem>();

            string[] files = Directory.GetFiles(_activePath, "*.json");
            if (files.Length <= 0)
                return false;

            for (int i = 0; i < files.Length; ++i)
            {
                string name = Path.GetFileName(files[i]);

                // 포맷 스탬프/백업 파일은 자재가 아니므로 건너뛴다.
                if (string.Equals(name, EFEM.MaterialTracking.LegacyRecoveryConverter.StampFileName, StringComparison.OrdinalIgnoreCase))
                    continue;
                if (name.IndexOf(".bak", StringComparison.OrdinalIgnoreCase) >= 0)
                    continue;

                string fileName = Path.GetFileNameWithoutExtension(files[i]);

                try
                {
                    // 1) 저장소에서 전송용 Data(DTO) 형태로 읽어옴
                    var dto = GetByKeyAsync(fileName).GetAwaiter().GetResult();
                    if (dto == null)
                        continue;

                    dataFromStorage.Add(dto);
                }
                catch (Exception ex)
                {
                    // 손상/미변환(정수 잔존) 파일은 격리: 전체 복구 로드를 중단시키지 않는다.
                    AsyncLoggerForEfem.Instance.WriteDebugLog(
                        $"[JsonCarrierStorage] skip unreadable recovery file '{name}': {ex.Message}");
                }
            }

            return true;
        }
        public bool IsExists(int portId, out string findKey)
        {
            findKey = string.Empty;
            var files = Directory.EnumerateFiles(_activePath);
            foreach (var item in files)
            {
                var key = Path.GetFileNameWithoutExtension(item);
                if (CarrierMapper.TryGetPortIdByKey(key, out var p) && 
                    p.Equals(portId))
                {
                    // 혹시라도 찾았으면
                    findKey = key;
                    return true;
                }
            }

            return false;
        }
        public bool IsExists(string key)
        {
            return File.Exists(JsonPath(_activePath, key));
        }

        public async Task<CarrierItem> GetByKeyAsync(string key)
        {
            if (key.EndsWith(".bak"))
                return null;

            var path = JsonPath(_activePath, key);
            if (false == File.Exists(path))
                return null;

            //return LoadJson(path);
            return await LoadJsonAsync(path).ConfigureAwait(false);
        }
        public Task UpsertAsync(CarrierItem dto)
        {
            if (_disposed)
                throw new ObjectDisposedException(nameof(JsonCarrierStorage));

            UpsertSync(dto);

            return Task.CompletedTask;
        }
        public Task DeleteAsync(string key)
        {
            if (_disposed) 
                throw new ObjectDisposedException(nameof(JsonCarrierStorage));

            // 디스크 쓰기 점유
            _ioThrottle.Wait();

            try
            {
                using (_keyedLocker.Acquire(key))
                {
                    var path = JsonPath(_activePath, key);
                    var bak = BakPath(_activePath, key);

                    if (File.Exists(path)) 
                        File.Delete(path);
                    if (File.Exists(bak)) 
                        File.Delete(bak);
                }
            }
            finally
            {
                // 디스크 쓰기 제한 해제
                _ioThrottle.Release(); 
            }

            return Task.CompletedTask;
        }
        public Task ArchiveAsync(string key, int portId, string baseArchivePath)
        {
            if (_disposed) 
                throw new ObjectDisposedException(nameof(JsonCarrierStorage));

            if (string.IsNullOrWhiteSpace(baseArchivePath)) 
                throw new ArgumentNullException(nameof(baseArchivePath));
            
            Directory.CreateDirectory(baseArchivePath);
            var destPath = Path.Combine(baseArchivePath, "Carrier");
            Directory.CreateDirectory(destPath);

            // 디스크 쓰기 점유
            _ioThrottle.Wait();

            try
            {
                using (_keyedLocker.Acquire(key))
                {
                    var src = JsonPath(_activePath, key);
                    var dst = JsonPath(destPath, key);
                    if (false == File.Exists(src))
                        return Task.CompletedTask;

                    // 목표 위치 파일 존재 시 제거
                    if (File.Exists(dst))
                        File.Delete(dst);
                    
                    // 목표위치로 파일 이동
                    File.Move(src, dst);

                    // 백업파일 제거
                    var bak = BakPath(_activePath, key);
                    if (File.Exists(bak)) 
                        File.Delete(bak);
                }
            }
            finally
            {
                // 디스크 쓰기 제한 해제
                _ioThrottle.Release(); 
            }

            foreach (var item in _carrierEventListners)
            {
                item?.OnCarrierArchived(portId, baseArchivePath);
            }

            return Task.CompletedTask;
        }
        public void Dispose()
        {
            if (_disposed) 
                return;
            
            _disposed = true;
            
            _ioThrottle?.Dispose();
            _keyedLocker?.Dispose();
        }
        #endregion </Interface>

        #region <Internal>
        private static string JsonPath(string dir, string key) => Path.Combine(dir, key + ".json");
        private static string BakPath(string dir, string key) => Path.Combine(dir, key + ".json.bak");
        private static void SafeMoveReplace(string src, string dst)
        {
            try
            {
                if (File.Exists(dst))
                    File.Delete(dst);
                
                File.Move(src, dst);
            }
            catch
            {
                try { File.Copy(src, dst, true); File.Delete(src); } catch { }
            }
        }
        private static async Task<CarrierItem> LoadJsonAsync(string path)
        {
            using (var fs = new FileStream(
                       path, FileMode.Open, FileAccess.Read, FileShare.Read,
                       bufferSize: 4096,
                       useAsync: true))
            using (var reader = new StreamReader(fs, Encoding.UTF8))
            {
                var json = await reader.ReadToEndAsync().ConfigureAwait(false);

                return JsonConvert.DeserializeObject<CarrierItem>(json, _jsonSettings);
            }
        }
        private static CarrierItem LoadJson(string path)
        {
            string json = File.ReadAllText(path, Encoding.UTF8);

            return JsonConvert.DeserializeObject<CarrierItem>(json, _jsonSettings);
        }
        private void UpsertSync(CarrierItem dto)
        {
            if (_disposed)
                throw new ObjectDisposedException(nameof(JsonCarrierStorage));
            if (dto == null)
                throw new ArgumentNullException(nameof(dto));
            
            ValidateKey(dto.UniqueKey);

            // JSON 정규화
            if (dto.Extra == null) 
                dto.Extra = new Dictionary<string, string>(StringComparer.Ordinal);

            // 디스크 쓰기 점유
            _ioThrottle.Wait();
            try
            {
                var key = dto.UniqueKey;
                var dst = JsonPath(_activePath, key);
                var bak = BakPath(_activePath, key);
                var tmp = Path.Combine(_activePath, key + "." + Guid.NewGuid().ToString("N") + ".tmp");
                var tmpCreated = false;

                try
                {
                    // JSON 정규화
                    if (dto.Extra == null) dto.Extra = new Dictionary<string, string>(StringComparer.Ordinal);

                    // tmp 파일에 바로 직렬화
                    SerializeToTempJson(dto, tmp, out tmpCreated);

                    // 유닛별 락
                    using (_keyedLocker.Acquire(key))
                    {
                        if (File.Exists(dst))
                        {
                            // 파일이 있으면 기존파일 백업파일로 변경 후 덮어 씌움
                            File.Replace(tmp, dst, bak);
                        }
                        else
                        {
                            // 파일 이동
                            File.Move(tmp, dst);
                        }

                        tmpCreated = false;
                    }
                }
                finally
                {
                    if (tmpCreated) 
                    {
                        try
                        {
                            File.Delete(tmp); 
                        } 
                        catch { } 
                    }
                }
            }
            finally
            {
                // 디스크 쓰기 제한 해제
                _ioThrottle.Release();
            }
        }
        private static void ValidateKey(string key)
        {
            if (string.IsNullOrWhiteSpace(key)) 
                throw new ArgumentException("Key is required.", nameof(key));
            if (key.IndexOfAny(Path.GetInvalidFileNameChars()) >= 0)
                throw new ArgumentException($"Invalid key for filename: '{key}'", nameof(key));
        }
        private static void SerializeToTempJson(CarrierItem dto, string tmp, out bool tmpCreated)
        {
            tmpCreated = false;

            // 1) 문자열 직렬화 
            string json = JsonConvert.SerializeObject(dto, _jsonSettings);
            
            // 2) UTF-8(BOM 없음)으로 기록
            var utf8NoBom = new UTF8Encoding(false);
            using (var fs = new FileStream(tmp, FileMode.Create, FileAccess.Write, FileShare.None, 8192, false))
            using (var sw = new StreamWriter(fs, utf8NoBom))
            {
                sw.Write(json);
                sw.Flush();
                try 
                {
                    fs.Flush(true); 
                }
                catch
                {
                    fs.Flush(); 
                }

                tmpCreated = true;
            }
        }
        #endregion </Internal>
        
        #endregion </Methods>
    }
}