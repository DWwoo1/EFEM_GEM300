using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

using EFEM.Database;

namespace EFEM.MaterialTracking.SubstrateStorage
{
    // 파일을 임시 파일(Guid)에 기록 성공 시 -> 기록해야할 키.json 파일에 기록 및 bak 파일 남김(이전 데이터)
    public sealed class JsonAndSqliteSubstrateStorage : ISubstrateStorage
    {
        #region <Constructors>
        public JsonAndSqliteSubstrateStorage(string activePath, int maxParallelIO, MaterialDbContext db)
        {
            _storages = new Dictionary<StorageType, ISubstrateStorage>
            {
                [StorageType.Json] = new JsonSubstrateStorage(activePath, maxParallelIO),
                [StorageType.Sqlite] = new SqliteSubstrateStorage(db)
            };
        }
        #endregion </Constructors>

        #region <Fields>
        private readonly List<ISubstrateEventObserver> _listeners = new List<ISubstrateEventObserver>();
        private readonly Dictionary<StorageType, ISubstrateStorage> _storages;
        #endregion </Fields>

        #region <Type>
        private enum StorageType
        {
            Json,
            Sqlite,
        }
        #endregion </Type>

        #region <Methods>

        #region <Interface>
        public void RegisterCallbackListner(ISubstrateEventObserver listner)
        {
            _listeners.Add(listner);
        }
        public void InitializeStorage()
        {
            foreach (var item in _storages)
            {
                item.Value.InitializeStorage();
            }
        }
        public bool LoadDataFromStorage(out List<SubstrateItem> dataFromStorage)
        {
            dataFromStorage = null;
            if (_storages[StorageType.Json] is ISubstrateStorage json)
            {
                json.LoadDataFromStorage(out dataFromStorage);
            }

            return true;
        }
        public bool IsExists(string key)
        {
            if (_storages[StorageType.Json] is ISubstrateStorage json)
            {
                return json.IsExists(key);
            }

            return false;
        }

        public async Task<SubstrateItem> GetByKeyAsync(string key)
        {
            if (_storages[StorageType.Json] is ISubstrateStorage json)
            {
                return await json.GetByKeyAsync(key);
            }

            return null;
        }
        public async Task<IReadOnlyList<SubstrateItem>> ListByLocationAsync(string locationName)
        {
            var list = new List<SubstrateItem>();
            if (_storages[StorageType.Json] is ISubstrateStorage json)
            {
                return await json.ListByLocationAsync(locationName);
            }

            return null;
        }
        public Task UpsertsAsync(IEnumerable<SubstrateItem> dtos)
        {
            foreach (var item in _storages)
            {
                item.Value.UpsertsAsync(dtos);
            }

            return Task.CompletedTask;
        }
        public Task UpsertAsync(SubstrateItem dto)
        {
            foreach (var item in _storages)
            {
                item.Value.UpsertAsync(dto);
            }

            return Task.CompletedTask;
        }
        public Task DeleteAsync(string key)
        {
            foreach (var item in _storages)
            {
                item.Value.DeleteAsync(key);
            }

            foreach (var item in _listeners)
            {
                item.OnSubstrateDeleted(key);
            }

            return Task.CompletedTask;
        }

        public Task ArchiveAsync(string key, string destinationPath)
        {
            foreach (var item in _storages)
            {
                item.Value.ArchiveAsync(key, destinationPath);
            }

            foreach (var item in _listeners)
            {
                item.OnSubstrateArchived(key, destinationPath);
            }

            return Task.CompletedTask;
        }
        #endregion </Interface>

        #endregion </Methods>
    }
}