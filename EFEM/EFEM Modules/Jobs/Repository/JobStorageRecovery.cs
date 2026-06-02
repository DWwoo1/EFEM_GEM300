using System;
using System.Collections.Generic;

using EFEM.Jobs.Domain;

namespace EFEM.Jobs.Repository
{
    public static class JobStorageRecovery
    {
        public static void Repair(
            IOrderedRepository<ControlJob, string> controlJobRepository,
            IOrderedRepository<ProcessJob, string> processJobRepository,
            IJobRelationRepository relationRepository,
            IRemovedBindingTargetRepository removedBindingTargetRepository)
        {
            if (controlJobRepository == null)
                throw new ArgumentNullException(nameof(controlJobRepository));

            if (processJobRepository == null)
                throw new ArgumentNullException(nameof(processJobRepository));

            if (relationRepository == null)
                throw new ArgumentNullException(nameof(relationRepository));

            if (removedBindingTargetRepository == null)
                throw new ArgumentNullException(nameof(removedBindingTargetRepository));

            var validProcessJobIds = BuildProcessJobIdSet(processJobRepository);

            relationRepository.Clear();

            var controlJobs = controlJobRepository.GetAll();

            foreach (var controlJob in controlJobs)
            {
                if (controlJob == null)
                    continue;

                var linkedProcessJobIds = FilterValidProcessJobIds(
                    controlJob.ProcessJobIds,
                    validProcessJobIds);

                if (linkedProcessJobIds.Length == 0)
                {
                    controlJobRepository.Remove(controlJob.Id);
                    continue;
                }

                if (!AreSame(controlJob.ProcessJobIds, linkedProcessJobIds))
                {
                    controlJob.ChangeProcessJobIds(linkedProcessJobIds);
                    controlJobRepository.AddOrUpdate(controlJob);
                }

                relationRepository.Link(controlJob.Id, linkedProcessJobIds);
            }

            RepairRemovedBindingTargets(
                processJobRepository,
                removedBindingTargetRepository);
        }

        private static HashSet<string> BuildProcessJobIdSet(
            IOrderedRepository<ProcessJob, string> processJobRepository)
        {
            var result = new HashSet<string>(StringComparer.Ordinal);

            var processJobs = processJobRepository.GetAll();

            foreach (var processJob in processJobs)
            {
                if (processJob == null)
                    continue;

                if (string.IsNullOrWhiteSpace(processJob.Id))
                    continue;

                result.Add(processJob.Id);
            }

            return result;
        }

        private static string[] FilterValidProcessJobIds(
            string[] processJobIds,
            HashSet<string> validProcessJobIds)
        {
            if (processJobIds == null || processJobIds.Length == 0)
                return new string[0];

            var result = new List<string>();

            foreach (var processJobId in processJobIds)
            {
                if (string.IsNullOrWhiteSpace(processJobId))
                    continue;

                if (!validProcessJobIds.Contains(processJobId))
                    continue;

                if (!result.Contains(processJobId))
                    result.Add(processJobId);
            }

            return result.ToArray();
        }
        private static bool ContainsMaterialSlot(
            IReadOnlyDictionary<string, IReadOnlyList<int>> materialInfo,
            string carrierId,
            int slot)
        {
            if (materialInfo == null || materialInfo.Count == 0)
                return false;

            if (string.IsNullOrWhiteSpace(carrierId))
                return false;

            if (slot <= 0)
                return false;

            foreach (KeyValuePair<string, IReadOnlyList<int>> item in materialInfo)
            {
                if (!string.Equals(
                    item.Key,
                    carrierId,
                    StringComparison.OrdinalIgnoreCase))
                {
                    continue;
                }

                if (item.Value == null || item.Value.Count == 0)
                    return false;

                foreach (int currentSlot in item.Value)
                {
                    if (currentSlot == slot)
                        return true;
                }
            }

            return false;
        }
        private static void RepairRemovedBindingTargets(
            IOrderedRepository<ProcessJob, string> processJobRepository,
            IRemovedBindingTargetRepository removedBindingTargetRepository)
        {
            var removedTargets = removedBindingTargetRepository.GetAll();

            if (removedTargets == null || removedTargets.Count == 0)
                return;

            foreach (RemovedBindingTarget target in removedTargets)
            {
                if (target == null)
                    continue;

                if (string.IsNullOrWhiteSpace(target.ProcessJobId))
                    continue;

                ProcessJob processJob =
                    processJobRepository.GetOrDefault(target.ProcessJobId);

                /*
                 * ProcessJob이 사라졌으면 제거 대상 기록도 의미가 없다.
                 */
                if (processJob == null)
                {
                    removedBindingTargetRepository.Remove(
                        target.ProcessJobId,
                        target.CarrierId,
                        target.Slot);

                    continue;
                }

                /*
                 * ProcessJob.MaterialInfo 원본에 더 이상 해당 Carrier/Slot이 없으면
                 * removed target도 stale 상태다.
                 */
                if (!ContainsMaterialSlot(
                    processJob.MaterialInfo,
                    target.CarrierId,
                    target.Slot))
                {
                    removedBindingTargetRepository.Remove(
                        target.ProcessJobId,
                        target.CarrierId,
                        target.Slot);
                }
            }
        }
        private static bool AreSame(string[] left, string[] right)
        {
            if (left == null)
                left = new string[0];

            if (right == null)
                right = new string[0];

            if (left.Length != right.Length)
                return false;

            for (int i = 0; i < left.Length; ++i)
            {
                if (!string.Equals(left[i], right[i], StringComparison.Ordinal))
                    return false;
            }

            return true;
        }
    }
}