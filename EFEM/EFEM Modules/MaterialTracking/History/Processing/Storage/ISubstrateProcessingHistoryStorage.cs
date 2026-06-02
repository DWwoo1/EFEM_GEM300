using System.Collections.Generic;

using EFEM.MaterialTracking;

namespace EFEM.MaterialTracking.ProcessingHistory.Storage
{
    public interface ISubstrateProcessingHistoryStorage : ISubstrateEventObserver
    {
        void Record(SubstrateProcessingHistoryItem item);
    }
}
