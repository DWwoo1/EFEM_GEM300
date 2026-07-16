using System;
using System.Text;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using System.Collections.Generic;
using Newtonsoft.Json;

using EFEM.Defines.Common;

namespace EFEM.MaterialTracking.LocationHistory.Storage
{
    class JsonSubstrateLocationHistoryStorage : ISubstrateLocationHistoryStorage, IDisposable
    {
        private readonly string _filePath;
        private const string ArchiveRelPath = "Location";
        private readonly SemaphoreSlim _ioThrottle;
        private readonly MonitorKeyedLocker _keyedLocker = new MonitorKeyedLocker();
        private volatile bool _disposed;

        private static readonly JsonSerializerSettings _jsonSettings = CreateJsonSettings();
        private static JsonSerializerSettings CreateJsonSettings()
        {
            try
            {
                return new JsonSerializerSettings
                {
                    Formatting = Formatting.None,
                    NullValueHandling = NullValueHandling.Ignore,
                    // 열거형(ModuleType 등)은 이름으로 저장. 정수 토큰은 조용히 해석하지 않는다.
                    Converters = new List<JsonConverter>
                    {
                        new Newtonsoft.Json.Converters.StringEnumConverter { AllowIntegerValues = false }
                    }
                };
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException(
                    "JsonSubstrateLocationHistoryStorage.CreateJsonSettings failed.", ex);
            }
        }

        public JsonSubstrateLocationHistoryStorage(string activeHistoryPath, int maxParallelIO = 6)
        {
            if (string.IsNullOrWhiteSpace(activeHistoryPath))
                throw new ArgumentNullException(nameof(activeHistoryPath));

            _filePath = activeHistoryPath;

            Directory.CreateDirectory(_filePath);
            RemoveLegacyFiles();

            _ioThrottle = new SemaphoreSlim(maxParallelIO, maxParallelIO);
        }

        //private static string ActiveHistoryPathForKey(string root, string key)
        //    => Path.Combine(root, $"{key}.loc.jsonl");

        //private static string ArchivedHistoryPathForKey(string archiveRoot, string key)
        //    => Path.Combine(archiveRoot, $"{key}.loc.jsonl");

        private static string ActiveChangeHistoryPathForKey(string root, string key)
            => Path.Combine(root, $"{key}.chg.jsonl");

        private static string ArchivedChangeHistoryPathForKey(string archiveRoot, string key)
            => Path.Combine(archiveRoot, $"{key}.chg.jsonl");

        private void RemoveLegacyFiles()
        {
            var files = Directory.GetFiles(_filePath);
            foreach (var item in files)
            {
                try
                {
                    if (item.EndsWith(".loc.jsonl"))
                        File.Delete(item);
                }
                catch { }
            }
        }
        public void AddOrUpdateLocations(IEnumerable<LocationItem> items) { }
        //public void RecordStay(SubstrateStayHistoryItem entry)
        //{
        //    return;

        //    if (_disposed)
        //        throw new ObjectDisposedException(nameof(JsonSubstrateLocationHistoryStorage));
        //    if (entry == null)
        //        throw new ArgumentNullException(nameof(entry));

        //    var key = entry.SubstrateKey;

        //    _ioThrottle.Wait();
        //    try
        //    {
        //        using (_keyedLocker.Acquire(key))
        //        {
        //            var path = ActiveHistoryPathForKey(_filePath, key);
        //            var json = JsonConvert.SerializeObject(entry, _jsonSettings);
        //            var utf8NoBom = new UTF8Encoding(false);
                    
        //            using (var fs = new FileStream(path, FileMode.Append, FileAccess.Write, FileShare.Read, 4096, true))
        //            {
        //                using (var sw = new StreamWriter(fs, utf8NoBom))
        //                {
        //                    sw.WriteLine(json);
        //                    sw.Flush();
        //                    try { fs.Flush(true); } catch { fs.Flush(); }
        //                }
        //            }
        //        }
        //    }
        //    finally
        //    {
        //        _ioThrottle.Release();
        //    }
        //}
        public void RecordChange(SubstrateLocationChangeItem entry)
        {
            if (_disposed)
                throw new ObjectDisposedException(nameof(JsonSubstrateLocationHistoryStorage));
            if (entry == null)
                throw new ArgumentNullException(nameof(entry));

            var key = entry.SubstrateKey;

            _ioThrottle.Wait();
            try
            {
                using (_keyedLocker.Acquire(key))
                {
                    // Change 전용 파일 경로 (Stay와 분리)
                    var path = ActiveChangeHistoryPathForKey(_filePath, key);

                    var json = JsonConvert.SerializeObject(entry, _jsonSettings);
                    var utf8NoBom = new UTF8Encoding(false);

                    using (var fs = new FileStream(path, FileMode.Append, FileAccess.Write, FileShare.Read, 4096, true))
                    {
                        using (var sw = new StreamWriter(fs, utf8NoBom))
                        {
                            sw.WriteLine(json);
                            sw.Flush();
                            try { fs.Flush(true); } catch { fs.Flush(); }
                        }
                    }
                }
            }
            finally
            {
                _ioThrottle.Release();
            }
        }
        public Task<IReadOnlyList<SubstrateLocationChangeItem>> ReadChangesAsync(string substrateKey)
        {
            if (_disposed)
                throw new ObjectDisposedException(nameof(JsonSubstrateLocationHistoryStorage));
            if (string.IsNullOrWhiteSpace(substrateKey))
                throw new ArgumentException("SubstrateKey is required.", nameof(substrateKey));

            var key = substrateKey;

            _ioThrottle.Wait();
            try
            {
                using (_keyedLocker.Acquire(key))
                {
                    var path = ActiveChangeHistoryPathForKey(_filePath, key);

                    // 파일이 아직 없으면(기록이 없으면) 빈 리스트
                    if (false == File.Exists(path))
                    {
                        //return null;
                        return Task.FromResult<IReadOnlyList<SubstrateLocationChangeItem>>(Array.Empty<SubstrateLocationChangeItem>());
                    }

                    var result = new List<SubstrateLocationChangeItem>();

                    // RecordChange가 utf8NoBom으로 썼으니 동일하게 읽는다.
                    var utf8NoBom = new UTF8Encoding(false);

                    // FileShare.ReadWrite: 쓰는 중(append)에도 읽을 수 있게.
                    // (RecordChange는 FileShare.Read로 열기 때문에 ReadWrite로 열어도 충돌 안 남)
                    using (var fs = new FileStream(path, FileMode.Open, FileAccess.Read, FileShare.ReadWrite, 4096, true))
                    using (var sr = new StreamReader(fs, utf8NoBom))
                    {
                        string line;
                        while ((line = sr.ReadLine()) != null)
                        {
                            if (string.IsNullOrWhiteSpace(line))
                                continue;

                            try
                            {
                                var item = JsonConvert.DeserializeObject<SubstrateLocationChangeItem>(line, _jsonSettings);
                                if (item == null)
                                    continue;

                                // 방어: 다른 키가 섞였을 가능성은 낮지만, 혹시 몰라 필터
                                if (string.Equals(item.SubstrateKey, key, StringComparison.OrdinalIgnoreCase) == false)
                                    continue;

                                // "" -> null 정규화 (FK/복구 안정성)
                                var from = string.IsNullOrWhiteSpace(item.FromLocationName) ? null : item.FromLocationName;
                                var fromKind = item.FromLocationKind;
                                var to = string.IsNullOrWhiteSpace(item.ToLocationName) ? null : item.ToLocationName;
                                var toKind = item.ToLocationKind;
                                result.Add(new SubstrateLocationChangeItem(
                                    substrateKey: item.SubstrateKey,
                                    fromLocationName: from,
                                    fromLocationKind: fromKind,
                                    toLocationName: to,
                                    toLocationKind: toKind,
                                    changeTime: item.ChangeTime,
                                    reason: item.Reason));
                            }
                            catch (JsonException)
                            {
                                // 손상된 라인/부분 쓰기 등: 스킵
                                // 필요하면 로깅 훅을 넣어도 됨.
                                continue;
                            }
                        }
                    }

                    // 일반적으로 append라 이미 시간순이겠지만, 안전하게 정렬
                    result.Sort((a, b) => a.ChangeTime.CompareTo(b.ChangeTime));

                    return Task.FromResult((IReadOnlyList<SubstrateLocationChangeItem>)result);
                }
            }
            finally
            {
                _ioThrottle.Release();
            }
        }
        // ISubstrateHistoryLifecycle
        public void OnSubstrateCreated(string substrateKey)
        {
        }

        public void OnSubstrateArchived(string substrateKey, string destinationPath)
        {
            if (_disposed)
                throw new ObjectDisposedException(nameof(JsonSubstrateLocationHistoryStorage));
            if (string.IsNullOrWhiteSpace(substrateKey))
                throw new ArgumentException("SubstrateKey is required.", nameof(substrateKey));
            if (string.IsNullOrWhiteSpace(destinationPath))
                throw new ArgumentNullException(nameof(destinationPath));

            //var src = ActiveHistoryPathForKey(_filePath, substrateKey);
            //var dstPath = Path.Combine(destinationPath, ArchiveRelPath);
            
            //Directory.CreateDirectory(dstPath);
            //var dst = ArchivedHistoryPathForKey(dstPath, substrateKey);

            // Change History
            var src = ActiveChangeHistoryPathForKey(_filePath, substrateKey);
            var dstPath = Path.Combine(destinationPath, ArchiveRelPath);

            Directory.CreateDirectory(dstPath);
            var dst = ArchivedChangeHistoryPathForKey(dstPath, substrateKey);

            _ioThrottle.Wait();
            try
            {
                using (_keyedLocker.Acquire(substrateKey))
                {
                    if (false == File.Exists(src))
                        return;

                    if (File.Exists(dst))
                        File.Delete(dst);

                    File.Move(src, dst);
                }
            }
            finally
            {
                _ioThrottle.Release();
            }
        }

        public void OnSubstrateDeleted(string substrateKey)
        {
            if (_disposed)
                throw new ObjectDisposedException(nameof(JsonSubstrateLocationHistoryStorage));
            if (string.IsNullOrWhiteSpace(substrateKey))
                throw new ArgumentException("SubstrateKey is required.", nameof(substrateKey));

            //var path = ActiveHistoryPathForKey(_filePath, substrateKey);
            var path = ActiveChangeHistoryPathForKey(_filePath, substrateKey);

            _ioThrottle.Wait();
            try
            {
                using (_keyedLocker.Acquire(substrateKey))
                {
                    if (File.Exists(path))
                        File.Delete(path);

                    //if (File.Exists(path2))
                    //    File.Delete(path2);
                }
            }
            finally
            {
                _ioThrottle.Release();
            }
        }

        public void Dispose()
        {
            if (_disposed) return;
            _disposed = true;

            _ioThrottle?.Dispose();
            _keyedLocker?.Dispose();
        }
    }
}
