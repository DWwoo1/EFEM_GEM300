using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Threading;

namespace EFEM.MaterialTracking.SubstrateStorage
{
    public interface ISubstrateStorage
    {
        void RegisterCallbackListner(ISubstrateEventObserver listner);
        bool LoadDataFromStorage(out List<SubstrateItem> dataFromStroage);

        // Read (부팅/복구 경로)
        Task<SubstrateItem> GetByKeyAsync(string key);
        Task<IReadOnlyList<SubstrateItem>> ListByLocationAsync(string locationName);
        bool IsExists(string key);

        // Write
        Task UpsertsAsync(IEnumerable<SubstrateItem> dtos);
        Task UpsertAsync(SubstrateItem dto);
        Task DeleteAsync(string key);

        // Archive
        Task ArchiveAsync(string key, string destinationPath);

        // Infra
        void InitializeStorage();
    }
}
