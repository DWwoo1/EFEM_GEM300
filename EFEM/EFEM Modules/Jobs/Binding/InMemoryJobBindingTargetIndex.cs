using System;
using System.Collections.Generic;

namespace EFEM.Jobs.Binding
{
    public sealed class InMemoryJobBindingTargetIndex : IJobBindingTargetIndex
    {
        private readonly object _sync = new object();

        private readonly Dictionary<string, List<JobBindingTarget>> _targetsByProcessJobId =
            new Dictionary<string, List<JobBindingTarget>>(StringComparer.OrdinalIgnoreCase);

        // Carrier만 있고 Slot 목록이 없는 Job을 조회하기 위한 인덱스 원본.
        private readonly Dictionary<string, List<string>> _carrierReferencesByProcessJobId =
            new Dictionary<string, List<string>>(StringComparer.OrdinalIgnoreCase);

        private readonly Dictionary<string, HashSet<string>> _processJobIdsByCarrier =
            new Dictionary<string, HashSet<string>>(StringComparer.OrdinalIgnoreCase);

        private readonly Dictionary<string, HashSet<string>> _processJobIdsByPortCarrier =
            new Dictionary<string, HashSet<string>>(StringComparer.OrdinalIgnoreCase);

        public void Clear()
        {
            lock (_sync)
            {
                _targetsByProcessJobId.Clear();
                _carrierReferencesByProcessJobId.Clear();
                _processJobIdsByCarrier.Clear();
                _processJobIdsByPortCarrier.Clear();
            }
        }

        public void AddOrUpdateProcessJob(
            string processJobId,
            IReadOnlyList<JobBindingTarget> targets)
        {
            if (string.IsNullOrWhiteSpace(processJobId))
                return;

            lock (_sync)
            {
                if (targets == null || targets.Count == 0)
                {
                    _targetsByProcessJobId.Remove(processJobId);
                    RebuildIndexesUnderLock();
                    return;
                }

                var copiedTargets = new List<JobBindingTarget>();

                foreach (JobBindingTarget target in targets)
                {
                    if (!IsValidTarget(target))
                        continue;

                    copiedTargets.Add(target);
                }

                if (copiedTargets.Count == 0)
                    _targetsByProcessJobId.Remove(processJobId);
                else
                    _targetsByProcessJobId[processJobId] = copiedTargets;

                RebuildIndexesUnderLock();
            }
        }
        public void AddOrUpdateProcessJobCarrierReferences(
            string processJobId,
            IReadOnlyList<string> carrierIds)
        {
            if (string.IsNullOrWhiteSpace(processJobId))
                return;

            lock (_sync)
            {
                if (carrierIds == null || carrierIds.Count == 0)
                {
                    _carrierReferencesByProcessJobId.Remove(processJobId);
                    RebuildIndexesUnderLock();
                    return;
                }

                var copiedCarrierIds = new List<string>();

                foreach (string carrierId in carrierIds)
                {
                    if (string.IsNullOrWhiteSpace(carrierId))
                        continue;

                    if (!ContainsIgnoreCase(copiedCarrierIds, carrierId))
                        copiedCarrierIds.Add(carrierId);
                }

                if (copiedCarrierIds.Count == 0)
                    _carrierReferencesByProcessJobId.Remove(processJobId);
                else
                    _carrierReferencesByProcessJobId[processJobId] = copiedCarrierIds;

                RebuildIndexesUnderLock();
            }
        }
        public void RemoveProcessJob(string processJobId)
        {
            if (string.IsNullOrWhiteSpace(processJobId))
                return;

            lock (_sync)
            {
                _targetsByProcessJobId.Remove(processJobId);
                _carrierReferencesByProcessJobId.Remove(processJobId);
                RebuildIndexesUnderLock();
            }
        }

        public void UpdateCarrierPort(
            int sourcePortId,
            string carrierId)
        {
            if (sourcePortId <= 0)
                return;

            if (string.IsNullOrWhiteSpace(carrierId))
                return;

            lock (_sync)
            {
                var updated =
                    new Dictionary<string, List<JobBindingTarget>>(
                        StringComparer.OrdinalIgnoreCase);

                foreach (KeyValuePair<string, List<JobBindingTarget>> item in _targetsByProcessJobId)
                {
                    var targets = new List<JobBindingTarget>();

                    foreach (JobBindingTarget target in item.Value)
                    {
                        if (target == null)
                            continue;

                        if (string.Equals(
                            target.CarrierId,
                            carrierId,
                            StringComparison.OrdinalIgnoreCase))
                        {
                            targets.Add(
                                new JobBindingTarget(
                                    target.ProcessJobId,
                                    target.CarrierId,
                                    sourcePortId,
                                    target.Slot));
                        }
                        else
                        {
                            targets.Add(target);
                        }
                    }

                    updated[item.Key] = targets;
                }

                _targetsByProcessJobId.Clear();

                foreach (KeyValuePair<string, List<JobBindingTarget>> item in updated)
                    _targetsByProcessJobId[item.Key] = item.Value;

                RebuildIndexesUnderLock();
            }
        }

        public IReadOnlyList<string> GetProcessJobIdsByCarrier(
            string carrierId)
        {
            var result = new List<string>();

            if (string.IsNullOrWhiteSpace(carrierId))
                return result;

            lock (_sync)
            {
                HashSet<string> processJobIds;

                if (!_processJobIdsByCarrier.TryGetValue(carrierId, out processJobIds))
                    return result;

                foreach (string processJobId in processJobIds)
                {
                    if (!ContainsIgnoreCase(result, processJobId))
                        result.Add(processJobId);
                }
            }

            return result;
        }

        public IReadOnlyList<string> GetProcessJobIdsByPortCarrier(
            int sourcePortId,
            string carrierId)
        {
            var result = new List<string>();

            if (sourcePortId <= 0)
                return result;

            if (string.IsNullOrWhiteSpace(carrierId))
                return result;

            string key = CreatePortCarrierKey(sourcePortId, carrierId);

            lock (_sync)
            {
                HashSet<string> portCarrierProcessJobIds;

                if (_processJobIdsByPortCarrier.TryGetValue(key, out portCarrierProcessJobIds))
                {
                    foreach (string processJobId in portCarrierProcessJobIds)
                    {
                        if (!ContainsIgnoreCase(result, processJobId))
                            result.Add(processJobId);
                    }
                }

                // Carrier는 있지만 Slot 목록이 없는 Job도 포함한다.
                HashSet<string> carrierProcessJobIds;

                if (_processJobIdsByCarrier.TryGetValue(carrierId, out carrierProcessJobIds))
                {
                    foreach (string processJobId in carrierProcessJobIds)
                    {
                        if (!ContainsIgnoreCase(result, processJobId))
                            result.Add(processJobId);
                    }
                }
            }

            return result;
        }

        public IReadOnlyList<JobBindingTarget> GetTargetsByProcessJobId(
            string processJobId)
        {
            var result = new List<JobBindingTarget>();

            if (string.IsNullOrWhiteSpace(processJobId))
                return result;

            lock (_sync)
            {
                List<JobBindingTarget> targets;

                if (!_targetsByProcessJobId.TryGetValue(processJobId, out targets))
                    return result;

                result.AddRange(targets);
            }

            return result;
        }

        private void RebuildIndexesUnderLock()
        {
            _processJobIdsByCarrier.Clear();
            _processJobIdsByPortCarrier.Clear();

            // Carrier reference index.
            foreach (KeyValuePair<string, List<string>> item in _carrierReferencesByProcessJobId)
            {
                string processJobId = item.Key;
                List<string> carrierIds = item.Value;

                if (carrierIds == null)
                    continue;

                foreach (string carrierId in carrierIds)
                {
                    if (string.IsNullOrWhiteSpace(carrierId))
                        continue;

                    AddToIndex(
                        _processJobIdsByCarrier,
                        carrierId,
                        processJobId);
                }
            }

            // Carrier + Slot이 있는 실제 BindingTarget index.
            foreach (KeyValuePair<string, List<JobBindingTarget>> item in _targetsByProcessJobId)
            {
                foreach (JobBindingTarget target in item.Value)
                {
                    if (!IsValidTarget(target))
                        continue;

                    AddToIndex(
                        _processJobIdsByCarrier,
                        target.CarrierId,
                        target.ProcessJobId);

                    if (target.SourcePortId > 0)
                    {
                        AddToIndex(
                            _processJobIdsByPortCarrier,
                            CreatePortCarrierKey(target.SourcePortId, target.CarrierId),
                            target.ProcessJobId);
                    }
                }
            }
        }

        private static bool ContainsIgnoreCase(
            IReadOnlyList<string> values,
            string value)
        {
            if (values == null || values.Count == 0)
                return false;

            for (int i = 0; i < values.Count; ++i)
            {
                if (string.Equals(
                    values[i],
                    value,
                    StringComparison.OrdinalIgnoreCase))
                {
                    return true;
                }
            }

            return false;
        }

        private static void AddToIndex(
            Dictionary<string, HashSet<string>> index,
            string key,
            string processJobId)
        {
            if (string.IsNullOrWhiteSpace(key))
                return;

            if (string.IsNullOrWhiteSpace(processJobId))
                return;

            HashSet<string> processJobIds;

            if (!index.TryGetValue(key, out processJobIds))
            {
                processJobIds =
                    new HashSet<string>(StringComparer.OrdinalIgnoreCase);

                index[key] = processJobIds;
            }

            processJobIds.Add(processJobId);
        }

        private static string CreatePortCarrierKey(
            int sourcePortId,
            string carrierId)
        {
            return sourcePortId.ToString()
                + "|"
                + (carrierId ?? string.Empty);
        }

        private static bool IsValidTarget(JobBindingTarget target)
        {
            if (target == null)
                return false;

            if (string.IsNullOrWhiteSpace(target.ProcessJobId))
                return false;

            if (string.IsNullOrWhiteSpace(target.CarrierId))
                return false;

            if (target.Slot <= 0)
                return false;

            return true;
        }
    }
}