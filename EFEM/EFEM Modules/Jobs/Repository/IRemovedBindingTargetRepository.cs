using System.Collections.Generic;

namespace EFEM.Jobs.Repository
{
    public interface IRemovedBindingTargetRepository
    {
        void AddOrUpdate(RemovedBindingTarget target);

        void Remove(
            string processJobId,
            string carrierId,
            int slot);

        void RemoveByProcessJob(string processJobId);

        IReadOnlyList<RemovedBindingTarget> GetAll();

        void Clear();
    }
}