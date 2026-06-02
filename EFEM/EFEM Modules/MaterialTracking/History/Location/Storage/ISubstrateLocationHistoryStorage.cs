using System.Threading.Tasks;
using System.Collections.Generic;

namespace EFEM.MaterialTracking.LocationHistory.Storage
{
    /// <summary>
    /// Substrate 체류 히스토리를 실제 저장소(DB/파일 등)에 기록하는 인터페이스.
    /// </summary>
    public interface ISubstrateLocationHistoryStorage : ISubstrateEventObserver
    {
        void RecordChange(SubstrateLocationChangeItem entry);
        
        Task<IReadOnlyList<SubstrateLocationChangeItem>> ReadChangesAsync(string substrateKey);
    }
}
