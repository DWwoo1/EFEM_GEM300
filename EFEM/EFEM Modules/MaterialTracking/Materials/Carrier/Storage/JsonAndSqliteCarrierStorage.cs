using System;
using System.Collections.Generic;
using System.Collections.Concurrent;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.IO;

using EFEM.Database;

namespace EFEM.MaterialTracking.CarrierStorage
{
    // 파일을 임시 파일(Guid)에 기록 성공 시 -> 기록해야할 키.json 파일에 기록 및 bak 파일 남김(이전 데이터)
    public sealed class JsonAndSqliteCarrierStorage : ICarrierStorage
    {
        #region <Constructors>
        public JsonAndSqliteCarrierStorage(string activePath, int maxParallelIO, MaterialDbContext db)
        {
            _storages = new Dictionary<StorageType, ICarrierStorage>
            {
                [StorageType.Json] = new JsonCarrierStorage(activePath, maxParallelIO),
                [StorageType.Sqlite] = new SqliteCarrierStorage(db)
            };
        }
        #endregion </Constructors>

        #region <Fields>
        private readonly Dictionary<StorageType, ICarrierStorage> _storages;
        private readonly List<ICarrierEventObserver> _carrierEventListners = new List<ICarrierEventObserver>();
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
        public void RegisterListner(ICarrierEventObserver carrierEvent)
        {
            _carrierEventListners.Add(carrierEvent);
        }
        public void InitializeStorage()
        {
            foreach (var item in _storages)
            {
                item.Value.InitializeStorage();
            }
        }
        public bool LoadDataFromStorage(out List<CarrierItem> dataFromStorage)
        {
            dataFromStorage = null;
            if (_storages[StorageType.Json] is ICarrierStorage json)
            {
                json.LoadDataFromStorage(out dataFromStorage);
            }

            return true;
        }
        public bool IsExists(int portId, out string findKey)
        {
            findKey = string.Empty;
            if (_storages[StorageType.Json] is ICarrierStorage json)
            {
                return json.IsExists(portId, out findKey);
            }

            return false;
        }
        public bool IsExists(string key)
        {
            if (_storages[StorageType.Json] is ICarrierStorage json)
            {
                return json.IsExists(key);
            }

            return false;
        }

        public async Task<CarrierItem> GetByKeyAsync(string key)
        {
            if (_storages[StorageType.Json] is ICarrierStorage json)
            {
                return await json.GetByKeyAsync(key);
            }

            return null;
        }
        public Task UpsertAsync(CarrierItem dto)
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

            return Task.CompletedTask;
        }
        public Task ArchiveAsync(string key, int portId, string baseArchivePath)
        {
            foreach (var item in _storages)
            {
                item.Value.ArchiveAsync(key, portId, baseArchivePath);
            }

            foreach (var item in _carrierEventListners)
            {
                item?.OnCarrierArchived(portId, baseArchivePath);
            }

            return Task.CompletedTask;
        }
        #endregion </Interface>
        
        #endregion </Methods>
    }
}