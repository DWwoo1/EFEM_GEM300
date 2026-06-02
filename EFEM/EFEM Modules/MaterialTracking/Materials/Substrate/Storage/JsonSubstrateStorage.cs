using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.IO;
using Newtonsoft.Json;

using SubstrateStorageOnly;
using EFEM.Defines.Common;
using EFEM.MaterialTracking;

namespace EFEM.MaterialTracking.SubstrateStorage
{
    // 파일을 임시 파일(Guid)에 기록 성공 시 -> 기록해야할 키.json 파일에 기록 및 bak 파일 남김(이전 데이터)
    public sealed class JsonSubstrateStorage : ISubstrateStorage, IDisposable
    {
        #region <Constructors>
        public JsonSubstrateStorage(string activePath, int maxParallelIO = 6)
        {
            if (string.IsNullOrWhiteSpace(activePath))
                throw new ArgumentNullException(nameof(activePath));

            _activePath = activePath;

            if (false == Directory.Exists(_activePath))
            {
                Directory.CreateDirectory(_activePath);
            }

            _ioThrottle = new SemaphoreSlim(maxParallelIO, maxParallelIO);
        }
        #endregion </Constructors>

        #region <Fields>
        private readonly string _activePath;    // 파일 저장소 경로
        private readonly SemaphoreSlim _ioThrottle;     // 동시에 디스크 쓰기 제한
        private readonly MonitorKeyedLocker _keyedLocker = new MonitorKeyedLocker();    // 자재 키 개별 락
        private readonly List<ISubstrateEventObserver> _listeners = new List<ISubstrateEventObserver>();
        private volatile bool _disposed;
        
        private static readonly JsonSerializerSettings _jsonSettings = new JsonSerializerSettings
        {
            Formatting = Formatting.Indented,
            NullValueHandling = NullValueHandling.Ignore
        };
        #endregion </Fields>

        #region <Properties>
        #endregion </Properties>

        #region <Methods>

        #region <Interface>
        public void RegisterCallbackListner(ISubstrateEventObserver listner)
        {
            _listeners.Add(listner);
        }
        public void InitializeStorage()
        {
            if (_disposed) 
                throw new ObjectDisposedException(nameof(JsonSubstrateStorage));

            Directory.CreateDirectory(_activePath);

            // 부팅 시 남은 tmp 정리 -> tmp 파일은 캐리어 기준 자재가 제거되어 arcive로 이동 시 지워지도록 변경
            //foreach (var tmp in Directory.EnumerateFiles(_activePath, "*.tmp"))
            //{
            //    try
            //    {
            //        File.Delete(tmp);
            //    }
            //    catch { }
            //}

            // 부팅 시 레거시 파일(기존 xml 파일) 있으면 -> 로드 후 json으로 대체. 추후 버전 횡전개 시 필요없어질 기능임
            MigrateLegacyFolder(_activePath);
        }
        public bool LoadDataFromStorage(out List<SubstrateItem> dataFromStorage)
        {
            dataFromStorage = new List<SubstrateItem>();

            string[] files = Directory.GetFiles(_activePath);
            if (files.Length <= 0)
                return false;

            for (int i = 0; i < files.Length; ++i)
            {
                string fileName = Path.GetFileNameWithoutExtension(files[i]);

                // 1) 저장소에서 전송용 Data(DTO) 형태로 읽어옴
                var dto = GetByKeyAsync(fileName).GetAwaiter().GetResult();
                if (dto == null)
                    continue;

                dataFromStorage.Add(dto);
            }

            return true;
        }
        public bool IsExists(string key)
        {
            return File.Exists(JsonPath(_activePath, key));
        }

        public async Task<SubstrateItem> GetByKeyAsync(string key)
        {
            if (key.EndsWith(".bak"))
                return null;

            var path = JsonPath(_activePath, key);
            if (false == File.Exists(path))
                return null;

            return await LoadJsonAsync(path).ConfigureAwait(false);
            //return LoadJson(path);
        }
        public async Task<IReadOnlyList<SubstrateItem>> ListByLocationAsync(string locationName)
        {
            var list = new List<SubstrateItem>();
            foreach (var file in Directory.EnumerateFiles(_activePath, "*.json"))
            {               
                var dto = await LoadJsonAsync(file).ConfigureAwait(false); //LoadJson(file);
                
                if (dto != null && string.Equals(dto.LocationId, locationName, StringComparison.Ordinal))
                    list.Add(dto);
            }

            return list;
        }
        public Task UpsertsAsync(IEnumerable<SubstrateItem> dtos)
        {
            if (_disposed)
                throw new ObjectDisposedException(nameof(JsonSubstrateStorage));

            foreach (var dto in dtos)
            {
                UpsertSync(dto);
            }

            return Task.CompletedTask;
        }
        public Task UpsertAsync(SubstrateItem dto)
        {
            if (_disposed)
                throw new ObjectDisposedException(nameof(JsonSubstrateStorage));

            UpsertSync(dto);

            return Task.CompletedTask;
        }
        public Task DeleteAsync(string key)
        {
            if (_disposed) 
                throw new ObjectDisposedException(nameof(JsonSubstrateStorage));

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

            foreach (var item in _listeners)
            {
                item.OnSubstrateDeleted(key);
            }

            return Task.CompletedTask;
        }

        public Task ArchiveAsync(string key, string destinationPath)
        {
            if (_disposed) throw new ObjectDisposedException(nameof(JsonSubstrateStorage));
            if (string.IsNullOrWhiteSpace(destinationPath)) throw new ArgumentNullException(nameof(destinationPath));
            
            if (false == Directory.Exists(destinationPath))
                Directory.CreateDirectory(destinationPath);

            // 디스크 쓰기 점유
            _ioThrottle.Wait();

            try
            {
                using (_keyedLocker.Acquire(key))
                {
                    var src = JsonPath(_activePath, key);
                    var dst = JsonPath(destinationPath, key);
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
            
            foreach (var item in _listeners)
            {
                item.OnSubstrateArchived(key, destinationPath);
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

        // --- 레거시 XML → JSON 마이그레이션 (InitializeAsync에서만 호출) ---
        private void MigrateLegacyFolder(string folder)
        {
            // *.xml 스캔 -> .json / .json.bak 제외
            foreach (var legacy in Directory.EnumerateFiles(folder, "*.xml"))
            {
                if (legacy.EndsWith(".json", StringComparison.OrdinalIgnoreCase) ||
                    legacy.EndsWith(".json.bak", StringComparison.OrdinalIgnoreCase))
                    continue;

                // 파일명 기반 키
                var keyHint = Path.GetFileNameWithoutExtension(legacy);

                try
                {
                    if (false == LegacySubstrateLoader.TryLoadLegacyDto(legacy, out var dto) || dto == null)
                    {
                        // 파싱 실패 -> 에러파일 이력 생성
                        var quarantine = Path.Combine(folder, keyHint + ".legacy.err.xml");
                        SafeMoveReplace(legacy, quarantine);
                        continue;
                    }

                    // JSON 정규화
                    if (dto.Extra == null)
                        dto.Extra = new Dictionary<string, string>(StringComparer.Ordinal);

                    ValidateKey(dto.UniqueKey);

                    var dst = JsonPath(folder, dto.UniqueKey);
                    var bak = BakPath(folder, dto.UniqueKey);
                    var tmp = Path.Combine(folder, dto.UniqueKey + "." + Guid.NewGuid().ToString("N") + ".tmp");

                    // 임시 파일로 직렬화
                    SerializeToTempJson(dto, tmp, out var tmpCreated);

                    // 유닛 별 락
                    using (_keyedLocker.Acquire(dto.UniqueKey))
                    {
                        // 파일이 있으면 -> 이전파일을 백업파일로(.bak) 보내고 덮어 씌움, 없으면 이동
                        if (File.Exists(dst)) 
                            File.Replace(tmp, dst, bak);
                        else 
                            File.Move(tmp, dst);
                        
                        tmpCreated = false;
                        try
                        {
                            // 기존 xml 파일 제거
                            File.Delete(legacy); 
                        } 
                        catch { }
                    }

                    if (tmpCreated)
                    {
                        try 
                        {
                            // tmp 파일 제거
                            File.Delete(tmp);
                        } 
                        catch { } 
                    }
                }
                catch { }
            }
        }
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

        private static SubstrateItem LoadJson(string path)
        {
            string json = File.ReadAllText(path, Encoding.UTF8);

            return JsonConvert.DeserializeObject<SubstrateItem>(json, _jsonSettings);
        }
        private static async Task<SubstrateItem> LoadJsonAsync(string path)
        {
            using (var fs = new FileStream(
                       path, FileMode.Open, FileAccess.Read, FileShare.Read,
                       bufferSize: 4096,
                       useAsync: true))
            using (var reader = new StreamReader(fs, Encoding.UTF8))
            {
                var json = await reader.ReadToEndAsync().ConfigureAwait(false);

                return JsonConvert.DeserializeObject<SubstrateItem>(json, _jsonSettings);
            }
        }
        private void UpsertSync(SubstrateItem dto)
        {
            if (_disposed)
                throw new ObjectDisposedException(nameof(JsonSubstrateStorage));
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
                var isNew = false;

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
                            isNew = true;

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

                if (isNew)
                {
                    foreach (var item in _listeners)
                    {
                        item.OnSubstrateCreated(key);
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

        private static void SerializeToTempJson(SubstrateItem dto, string tmp, out bool tmpCreated)
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

namespace SubstrateStorageOnly
{
    using System.Xml.Linq;
    using EFEM.MaterialTracking.SubstrateStorage;

    public static class LegacySubstrateLoader
    {
        private const string RootName = "SubstrateAttributes";
        private const string ElementName = "Item";
        private const string AttributeKey = "Key";
        private const string AttributeValue = "Value";

        public static bool TryLoadLegacyDto(string legacyPath, out SubstrateItem dto)
        {
            dto = null;
            if (string.IsNullOrWhiteSpace(legacyPath) || !File.Exists(legacyPath))
                return false;

            try
            {
                var xml = XElement.Load(legacyPath, LoadOptions.PreserveWhitespace);

                // 루트명 불일치해도 내부의 Item만 모으면 동작
                IEnumerable<XElement> items =
                    string.Equals(xml.Name.LocalName, RootName, StringComparison.Ordinal)
                    ? xml.Elements(ElementName)
                    : xml.Descendants(ElementName);

                // 마지막 항목 우선 정책
                var map = new Dictionary<string, string>(StringComparer.Ordinal);
                foreach (var item in items)
                {
                    var keyAttr = item.Attribute(AttributeKey);
                    if (keyAttr == null) continue;

                    var key = (keyAttr.Value ?? string.Empty).Trim();
                    if (key.Length == 0) continue;

                    var valAttr = item.Attribute(AttributeValue);
                    var val = valAttr != null ? (valAttr.Value ?? string.Empty).Trim() : string.Empty;
                    map[key] = val;
                }

                dto = SubstrateMapper.GetSubstrateDataFromAttributes(map, out var extra);
                dto.Extra = extra;
                if (extra != null)
                {
                    foreach (var item in extra)
                    {
                        dto.Extra[item.Key] = item.Value;
                    }
                }

                return true;
            }
            catch
            {
                return false;
            }
        }
    }
}