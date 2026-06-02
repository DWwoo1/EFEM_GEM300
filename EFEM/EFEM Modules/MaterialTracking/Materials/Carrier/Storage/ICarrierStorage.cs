using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Threading;

namespace EFEM.MaterialTracking.CarrierStorage
{
    public interface ICarrierStorage
    {
        void RegisterListner(ICarrierEventObserver carrierEvent);

        // Read (부팅/복구 경로)
        bool LoadDataFromStorage(out List<CarrierItem> dataFromStroage);
        Task<CarrierItem> GetByKeyAsync(string key);
        bool IsExists(int portId, out string key);
        bool IsExists(string key);

        // Write
        Task UpsertAsync(CarrierItem dto);
        Task DeleteAsync(string key);

        // Archive
        Task ArchiveAsync(string key, int portId, string baseArchivePath);

        // Infra
        void InitializeStorage();
    }
}
