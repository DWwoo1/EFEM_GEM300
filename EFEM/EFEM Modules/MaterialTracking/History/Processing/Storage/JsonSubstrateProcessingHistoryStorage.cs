using System;
using System.IO;
using System.Text;
using System.Threading;
using System.Collections.Generic;
using Newtonsoft.Json;

using EFEM.Defines.Common;

namespace EFEM.MaterialTracking.ProcessingHistory.Storage
{
    /// <summary>
    /// ProcessingState 변경 이력을 JSON Lines 파일로 기록하는 구현.
    /// 한 줄에 JSON 하나 (SubstrateProcessingHistoryEntry 1건)씩 Append.
    /// </summary>
    public sealed class JsonSubstrateProcessingHistoryStorage : ISubstrateProcessingHistoryStorage, IDisposable
    {
        #region <Constructors>

        public JsonSubstrateProcessingHistoryStorage(string filePath, int maxParallelIO = 6)
        {
            if (string.IsNullOrWhiteSpace(filePath))
                throw new ArgumentNullException(nameof(filePath));

            _filePath = filePath;

            Directory.CreateDirectory(_filePath);

            _ioThrottle = new SemaphoreSlim(maxParallelIO, maxParallelIO);
        }

        #endregion </Constructors>

        #region <Fields>

        private readonly string _filePath;
        private const string ArchiveRelPath = "Processing";
        private readonly SemaphoreSlim _ioThrottle;
        private readonly MonitorKeyedLocker _keyedLocker = new MonitorKeyedLocker();
        private volatile bool _disposed;

        private static readonly JsonSerializerSettings _jsonSettings = new JsonSerializerSettings
        {
            Formatting = Formatting.None,             // JSONL 이라 한 줄에 한 객체
            NullValueHandling = NullValueHandling.Ignore
        };

        #endregion </Fields>

        #region <Methods>
        private static string ActiveHistoryPathForKey(string root, string key)
            => Path.Combine(root, $"{key}.proc.jsonl");

        private static string ArchivedHistoryPathForKey(string archiveRoot, string key)
            => Path.Combine(archiveRoot, $"{key}.proc.jsonl");
        
        public void Record(SubstrateProcessingHistoryItem item)
        {
            if (_disposed)
                throw new ObjectDisposedException(nameof(JsonSubstrateProcessingHistoryStorage));
            if (item == null)
                throw new ArgumentNullException(nameof(item));
            if (string.IsNullOrWhiteSpace(item.SubstrateKey))
                throw new ArgumentException("SubstrateKey is required.", nameof(item));

            _ioThrottle.Wait();

            try
            {
                var path = ActiveHistoryPathForKey(_filePath, item.SubstrateKey);

                var json = JsonConvert.SerializeObject(item, _jsonSettings);
                var line = json + Environment.NewLine;

                // 하나의 파일에 여러 키가 함께 기록되므로 파일 단위 lock
                using (_keyedLocker.Acquire(item.SubstrateKey))
                {
                    var utf8NoBom = new UTF8Encoding(false);

                    using (var fs = new FileStream(
                               path,
                               FileMode.Append,
                               FileAccess.Write,
                               FileShare.Read,
                               bufferSize: 4096,
                               useAsync: true))

                    using (var sw = new StreamWriter(fs, utf8NoBom))
                    {
                        sw.Write(line);
                        sw.Flush();
                        try
                        {
                            fs.Flush(true);
                        }
                        catch
                        {
                            fs.Flush();
                        }
                    }
                }
            }
            finally
            {
                _ioThrottle.Release();
            }
        }

        public void OnSubstrateCreated(string substrateKey)
        {
            //return;
            //if (_disposed)
            //    throw new ObjectDisposedException(nameof(JsonSubstrateProcessingHistoryStorage));
            //if (string.IsNullOrWhiteSpace(substrateKey))
            //    throw new ArgumentException("SubstrateKey is required.", nameof(substrateKey));

            //var path = ActiveHistoryPathForKey(_filePath, substrateKey);

            //_ioThrottle.Wait();
            //try
            //{
            //    using (_keyedLocker.Acquire(substrateKey))
            //    {
            //        if (false == File.Exists(path))
            //        {
            //            Directory.CreateDirectory(_filePath);
            //            using (var fs = new FileStream(path, FileMode.CreateNew, FileAccess.Write))
            //            {
            //                // 빈 파일 생성
            //            }
            //        }
            //    }
            //}
            //finally
            //{
            //    _ioThrottle.Release();
            //}
        }

        public void OnSubstrateArchived(string substrateKey, string destinationPath)
        {
            if (_disposed)
                throw new ObjectDisposedException(nameof(JsonSubstrateProcessingHistoryStorage));
            if (string.IsNullOrWhiteSpace(substrateKey))
                throw new ArgumentException("SubstrateKey is required.", nameof(substrateKey));
            if (string.IsNullOrWhiteSpace(destinationPath))
                throw new ArgumentNullException(nameof(destinationPath));

            var src = ActiveHistoryPathForKey(_filePath, substrateKey);
            var dstPath = Path.Combine(destinationPath, ArchiveRelPath);

            Directory.CreateDirectory(dstPath);
            var dst = ArchivedHistoryPathForKey(dstPath, substrateKey);

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
                throw new ObjectDisposedException(nameof(JsonSubstrateProcessingHistoryStorage));
            if (string.IsNullOrWhiteSpace(substrateKey))
                throw new ArgumentException("SubstrateKey is required.", nameof(substrateKey));

            var path = ActiveHistoryPathForKey(_filePath, substrateKey);
            
            _ioThrottle.Wait();
            try
            {
                using (_keyedLocker.Acquire(substrateKey))
                {
                    if (File.Exists(path))
                        File.Delete(path);
                }
            }
            finally
            {
                _ioThrottle.Release();
            }
        }
        public void Dispose()
        {
            if (_disposed)
                return;

            _disposed = true;
            _ioThrottle?.Dispose();
            _keyedLocker?.Dispose();
        }

        #endregion </Methods>
    }
}
