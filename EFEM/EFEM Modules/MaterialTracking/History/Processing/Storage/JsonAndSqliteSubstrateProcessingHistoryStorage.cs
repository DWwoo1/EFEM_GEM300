using System;
using System.IO;
using System.Text;
using System.Threading;
using System.Collections.Generic;


using EFEM.Database;

namespace EFEM.MaterialTracking.ProcessingHistory.Storage
{
    /// <summary>
    /// ProcessingState 변경 이력을 JSON Lines 파일로 기록하는 구현.
    /// 한 줄에 JSON 하나 (SubstrateProcessingHistoryEntry 1건)씩 Append.
    /// </summary>
    public sealed class JsonAndSqliteSubstrateProcessingHistoryStorage : ISubstrateProcessingHistoryStorage
    {
        #region <Constructors>
        public JsonAndSqliteSubstrateProcessingHistoryStorage(string filePath, int maxParallelIO, MaterialDbContext db)
        {
            _storages = new Dictionary<StorageType, ISubstrateProcessingHistoryStorage>
            {
                [StorageType.Json] = new JsonSubstrateProcessingHistoryStorage(filePath, maxParallelIO),
                [StorageType.Sqlite] = new SqliteSubstrateProcessingHistoryStorage(db)
            };
        }

        #endregion </Constructors>

        #region <Fields>
        private readonly Dictionary<StorageType, ISubstrateProcessingHistoryStorage> _storages;
        #endregion </Fields>

        #region <Types>
        private enum StorageType
        {
            Json,
            Sqlite,
        }
        #endregion </Types>

        #region <Methods>
        public void Record(SubstrateProcessingHistoryItem item)
        {
            foreach (var s in _storages)
            {
                s.Value.Record(item);
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
            if (_storages[StorageType.Json] is ISubstrateProcessingHistoryStorage json)
            {
                json.OnSubstrateArchived(substrateKey, destinationPath);
            }
        }

        public void OnSubstrateDeleted(string substrateKey)
        {
            if (_storages[StorageType.Json] is ISubstrateProcessingHistoryStorage json)
            {
                json.OnSubstrateDeleted(substrateKey);
            }
        }
        #endregion </Methods>
    }
}
