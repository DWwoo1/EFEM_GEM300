using System;
using System.Text;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using System.Collections.Generic;

using EFEM.Database;

namespace EFEM.MaterialTracking.LocationHistory.Storage
{
    class JsonAndSqliteSubstrateLocationHistoryStorage : ISubstrateLocationHistoryStorage
    {
        private enum StorageType
        {
            Json,
            Sqlite,
        }
        private readonly Dictionary<StorageType, ISubstrateLocationHistoryStorage> _storages;
        public JsonAndSqliteSubstrateLocationHistoryStorage(string activeHistoryPath, int maxParallelIO, MaterialDbContext db)
        {
            _storages = new Dictionary<StorageType, ISubstrateLocationHistoryStorage>
            {
                [StorageType.Json] = new JsonSubstrateLocationHistoryStorage(activeHistoryPath, maxParallelIO),
                [StorageType.Sqlite] = new SqliteSubstrateLocationHistoryStorage(db)
            };
        }

        public void RecordChange(SubstrateLocationChangeItem entry)
        {
            foreach (var item in _storages)
            {
                item.Value.RecordChange(entry);
            }
        }

        public Task<IReadOnlyList<SubstrateLocationChangeItem>> ReadChangesAsync(string substrateKey)
        {
            return _storages[StorageType.Json].ReadChangesAsync(substrateKey);
        }
        // ISubstrateHistoryLifecycle
        public void OnSubstrateCreated(string substrateKey)
        {
        }

        public void OnSubstrateArchived(string substrateKey, string destinationPath)
        {
            if (_storages[StorageType.Json] is JsonSubstrateLocationHistoryStorage json)
            {
                json.OnSubstrateArchived(substrateKey, destinationPath);
            }
        }

        public void OnSubstrateDeleted(string substrateKey)
        {
            if (_storages[StorageType.Json] is JsonSubstrateLocationHistoryStorage json)
            {
                json.OnSubstrateDeleted(substrateKey);
            }
        }
    }
}
