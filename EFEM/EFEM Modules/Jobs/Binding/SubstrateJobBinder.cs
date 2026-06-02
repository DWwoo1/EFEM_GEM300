using System;
using System.Collections.Generic;

using EFEM.Defines.Job;
using EFEM.Defines.MaterialTracking;
using EFEM.Jobs.Domain;
using EFEM.Jobs.Manager;
using EFEM.MaterialTracking;
using EFEM.Jobs.Repository;

namespace EFEM.Jobs.Binding
{
    /// <summary>
    /// 검증 완료된 Substrate에 Job 정보를 바인딩하는 객체.
    ///
    /// SRP 기준 책임:
    /// - Job 정보와 Substrate 정보의 연결만 담당한다.
    ///
    /// 의존 객체별 책임:
    /// - IJobManager:
    ///   ControlJob, ProcessJob, ControlJob-ProcessJob 관계 조회.
    ///
    /// - CarrierManagementServer:
    ///   CarrierId로 PortId를 찾거나, PortId 기준 CarrierId를 조회.
    ///
    /// - SubstrateManager:
    ///   Slot에 존재하는 Substrate 조회 및 Substrate 속성 기록.
    ///
    /// 이 객체가 직접 하지 않는 일:
    /// - ProcessJob 생성
    /// - ControlJob 생성
    /// - SlotMap 검증
    /// - Carrier 생성/삭제
    /// - Substrate 생성/삭제
    /// - Job 상태 변경
    /// </summary>
    public sealed class SubstrateJobBinder : ISubstrateJobBinder
    {
        private readonly object _sync = new object();

        private readonly IJobManager _jobManager;
        private readonly SubstrateManager _substrateManager;
        private readonly CarrierManagementServer _carrierManager;
        private readonly IRemovedBindingTargetRepository _removedBindingTargetRepository;
        private readonly IJobBindingTargetIndex _bindingTargetIndex;

        private readonly HashSet<string> _removedBindingTargetKeys =
            new HashSet<string>(StringComparer.OrdinalIgnoreCase);

        private sealed class ProcessJobMaterialReference
        {
            public string CarrierId { get; private set; }
            public int Slot { get; private set; }

            public ProcessJobMaterialReference(string carrierId, int slot)
            {
                CarrierId = carrierId;
                Slot = slot;
            }
        }

        public SubstrateJobBinder(
            IJobManager jobManager,
            SubstrateManager substrateManager,
            CarrierManagementServer carrierManager,
            IRemovedBindingTargetRepository removedBindingTargetRepository,
            IJobBindingTargetIndex bindingTargetIndex)
        {
            if (jobManager == null)
                throw new ArgumentNullException(nameof(jobManager));

            if (substrateManager == null)
                throw new ArgumentNullException(nameof(substrateManager));

            if (carrierManager == null)
                throw new ArgumentNullException(nameof(carrierManager));

            if (removedBindingTargetRepository == null)
                throw new ArgumentNullException(nameof(removedBindingTargetRepository));

            if (bindingTargetIndex == null)
                throw new ArgumentNullException(nameof(bindingTargetIndex));

            _jobManager = jobManager;
            _substrateManager = substrateManager;
            _carrierManager = carrierManager;
            _removedBindingTargetRepository = removedBindingTargetRepository;
            _bindingTargetIndex = bindingTargetIndex;

            ReloadRemovedBindingTargets();
        }

        public void BindByProcessJob(string processJobId)
        {
            if (string.IsNullOrWhiteSpace(processJobId))
                return;

            lock (_sync)
            {
                ProcessJob processJob =
                    _jobManager.GetProcessJobOrDefault(processJobId);

                if (processJob == null)
                {
                    _bindingTargetIndex.RemoveProcessJob(processJobId);
                    return;
                }

                RefreshBindingTargetIndexCore(processJob);

                string controlJobId =
                    _jobManager.GetControlJobIdOrDefault(processJobId);

                BindProcessJobToSubstrates(
                    controlJobId,
                    processJob);
            }
        }

        public void BindByControlJob(string controlJobId)
        {
            if (string.IsNullOrWhiteSpace(controlJobId))
                return;

            lock (_sync)
            {
                IReadOnlyList<ProcessJob> processJobs = _jobManager.GetLinkedProcessJobs(controlJobId);

                if (processJobs == null || processJobs.Count == 0)
                    return;

                foreach (ProcessJob processJob in processJobs)
                {
                    if (processJob == null)
                        continue;

                    RefreshBindingTargetIndexCore(processJob);

                    BindProcessJobToSubstrates(
                        controlJobId,
                        processJob);
                }
            }
        }

        public void BindByCarrierPort(int portId)
        {
            if (portId <= 0)
                return;

            lock (_sync)
            {
                IReadOnlyList<string> processJobIds =
                    GetProcessJobIdsByCarrierPortCore(portId);

                if (processJobIds == null || processJobIds.Count == 0)
                    return;

                foreach (string processJobId in processJobIds)
                {
                    ProcessJob processJob =
                        _jobManager.GetProcessJobOrDefault(processJobId);

                    if (processJob == null)
                    {
                        _bindingTargetIndex.RemoveProcessJob(processJobId);
                        continue;
                    }

                    string controlJobId =
                        _jobManager.GetControlJobIdOrDefault(processJobId);

                    BindProcessJobToSubstrates(
                        controlJobId,
                        processJob);
                }
            }
        }
        public IReadOnlyList<string> GetControlJobIdsByCarrierPort(int portId)
        {
            var result = new List<string>();

            if (portId <= 0)
                return result;

            lock (_sync)
            {
                IReadOnlyList<string> processJobIds =
                    GetProcessJobIdsByCarrierPortCore(portId);

                if (processJobIds == null || processJobIds.Count == 0)
                    return result;

                foreach (string processJobId in processJobIds)
                {
                    if (string.IsNullOrWhiteSpace(processJobId))
                        continue;

                    string controlJobId =
                        _jobManager.GetControlJobIdOrDefault(processJobId);

                    if (string.IsNullOrWhiteSpace(controlJobId))
                        continue;

                    if (!ContainsIgnoreCase(result, controlJobId))
                        result.Add(controlJobId);
                }
            }

            return result;
        }
        public IReadOnlyList<string> GetControlJobIdsByCarrier(
            string carrierId)
        {
            var result = new List<string>();

            if (string.IsNullOrWhiteSpace(carrierId))
                return result;

            lock (_sync)
            {
                IReadOnlyList<string> processJobIds =
                    GetProcessJobIdsByCarrierCore(carrierId);

                if (processJobIds == null || processJobIds.Count == 0)
                    return result;

                foreach (string processJobId in processJobIds)
                {
                    if (string.IsNullOrWhiteSpace(processJobId))
                        continue;

                    string controlJobId =
                        _jobManager.GetControlJobIdOrDefault(processJobId);

                    if (string.IsNullOrWhiteSpace(controlJobId))
                        continue;

                    if (!ContainsIgnoreCase(result, controlJobId))
                        result.Add(controlJobId);
                }
            }

            return result;
        }
        public IReadOnlyList<string> GetProcessJobIdsByCarrier(
            string carrierId)
        {
            lock (_sync)
            {
                return GetProcessJobIdsByCarrierCore(carrierId);
            }
        }

        public IReadOnlyList<string> GetProcessJobIdsByCarrierPort(int portId)
        {
            lock (_sync)
            {
                return GetProcessJobIdsByCarrierPortCore(portId);
            }
        }
        private IReadOnlyList<string> GetProcessJobIdsByCarrierCore(
            string carrierId)
        {
            var result = new List<string>();

            if (string.IsNullOrWhiteSpace(carrierId))
                return result;

            IReadOnlyList<string> processJobIds =
                _bindingTargetIndex.GetProcessJobIdsByCarrier(carrierId);

            if (processJobIds == null || processJobIds.Count == 0)
                return result;

            foreach (string processJobId in processJobIds)
            {
                if (string.IsNullOrWhiteSpace(processJobId))
                    continue;

                ProcessJob processJob =
                    _jobManager.GetProcessJobOrDefault(processJobId);

                if (processJob == null)
                {
                    _bindingTargetIndex.RemoveProcessJob(processJobId);
                    continue;
                }

                if (!ContainsIgnoreCase(result, processJobId))
                    result.Add(processJobId);
            }

            return result;
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
        private IReadOnlyList<string> GetProcessJobIdsByCarrierPortCore(int portId)
        {
            var result = new List<string>();

            if (portId <= 0)
                return result;

            string carrierId = _carrierManager.GetCarrierId(portId);

            if (string.IsNullOrWhiteSpace(carrierId))
                return result;

            _bindingTargetIndex.UpdateCarrierPort(
                portId,
                carrierId);

            IReadOnlyList<string> processJobIds =
                _bindingTargetIndex.GetProcessJobIdsByPortCarrier(
                    portId,
                    carrierId);

            if (processJobIds == null || processJobIds.Count == 0)
                return result;

            foreach (string processJobId in processJobIds)
            {
                if (string.IsNullOrWhiteSpace(processJobId))
                    continue;

                ProcessJob processJob =
                    _jobManager.GetProcessJobOrDefault(processJobId);

                if (processJob == null)
                {
                    _bindingTargetIndex.RemoveProcessJob(processJobId);
                    continue;
                }

                result.Add(processJobId);
            }

            return result;
        }

        public bool IsBoundForControlJob(string controlJobId)
        {
            return IsBoundForControlJob(
                controlJobId,
                JobBindingValidationMode.ProcessJobAndControlJob);
        }

        public bool IsBoundForControlJob(
            string controlJobId,
            JobBindingValidationMode mode)
        {
            if (string.IsNullOrWhiteSpace(controlJobId))
                return false;

            lock (_sync)
            {
                IReadOnlyList<ProcessJob> processJobs =
                    _jobManager.GetLinkedProcessJobs(controlJobId);

                if (processJobs == null || processJobs.Count == 0)
                    return false;

                foreach (ProcessJob processJob in processJobs)
                {
                    if (processJob == null)
                        continue;

                    if (!IsProcessJobBound(
                        controlJobId,
                        processJob,
                        mode))
                    {
                        return false;
                    }
                }

                return true;
            }
        }

        public bool IsBoundForProcessJob(
            string processJobId,
            JobBindingValidationMode mode)
        {
            if (string.IsNullOrWhiteSpace(processJobId))
                return false;

            lock (_sync)
            {
                ProcessJob processJob =
                    _jobManager.GetProcessJobOrDefault(processJobId);

                if (processJob == null)
                    return false;

                string controlJobId =
                    _jobManager.GetControlJobIdOrDefault(processJobId);

                return IsProcessJobBound(
                    controlJobId,
                    processJob,
                    mode);
            }
        }
        public void UnbindByProcessJob(string processJobId)
        {
            if (string.IsNullOrWhiteSpace(processJobId))
                return;

            lock (_sync)
            {
                ProcessJob processJob =
                    _jobManager.GetProcessJobOrDefault(processJobId);

                if (processJob == null)
                {
                    _bindingTargetIndex.RemoveProcessJob(processJobId);
                    return;
                }

                string controlJobId =
                    _jobManager.GetControlJobIdOrDefault(processJobId);

                UnbindProcessJobFromSubstrates(
                    controlJobId,
                    processJob);

                /*
                 * SetProcessJobInfo 흐름에서는 이후 BindByProcessJob에서 다시 등록된다.
                 * ProcessJob 제거 흐름에서는 그대로 제거 상태로 남는다.
                 */
                _bindingTargetIndex.RemoveProcessJob(processJobId);
            }
        }

        public void UnbindByControlJob(string controlJobId)
        {
            if (string.IsNullOrWhiteSpace(controlJobId))
                return;

            lock (_sync)
            {
                IReadOnlyList<ProcessJob> processJobs = _jobManager.GetLinkedProcessJobs(controlJobId);

                if (processJobs == null)
                    return;

                foreach (ProcessJob processJob in processJobs)
                {
                    if (processJob == null)
                        continue;

                    UnbindProcessJobFromSubstrates(
                        controlJobId,
                        processJob);
                }
            }
        }

        private void BindProcessJobToSubstrates(
            string controlJobId,
            ProcessJob processJob)
        {
            if (processJob == null)
                return;

            if (!HasActiveSubstrateBindingTarget(processJob))
            {
                BindCarrierOnlyProcessJobToSubstrates(
                    controlJobId,
                    processJob);

                return;
            }

            foreach (ProcessJobMaterialReference materialRef in EnumerateMaterialReferences(processJob))
            {
                if (IsRemovedBindingTarget(processJob.Id, materialRef))
                    continue;

                int portId =
                    _carrierManager.GetPortIdByCarrierId(materialRef.CarrierId);

                if (portId <= 0)
                    continue;

                string substrateKey =
                    _substrateManager.GetSubstrateKeyAtLoadPort(
                        portId,
                        materialRef.Slot);

                if (string.IsNullOrWhiteSpace(substrateKey))
                    continue;

                _substrateManager.SetJobBindingInfoByKey(
                    substrateKey,
                    controlJobId,
                    processJob.Id,
                    processJob.RecipeId).GetAwaiter().GetResult();
            }
        }

        public void ReloadRemovedBindingTargets()
        {
            lock (_sync)
            {
                _removedBindingTargetKeys.Clear();

                IReadOnlyList<RemovedBindingTarget> targets =
                    _removedBindingTargetRepository.GetAll();

                if (targets != null)
                {
                    foreach (RemovedBindingTarget target in targets)
                    {
                        if (target == null)
                            continue;

                        if (string.IsNullOrWhiteSpace(target.ProcessJobId))
                            continue;

                        if (string.IsNullOrWhiteSpace(target.CarrierId))
                            continue;

                        if (target.Slot <= 0)
                            continue;

                        _removedBindingTargetKeys.Add(target.GetKey());
                    }
                }

                RebuildBindingTargetIndexCore();
            }
        }
        public void RemoveBindingTarget(
            string processJobId,
            string carrierId,
            int slot,
            string reason)
        {
            if (string.IsNullOrWhiteSpace(processJobId))
                return;

            if (string.IsNullOrWhiteSpace(carrierId))
                return;

            if (slot <= 0)
                return;

            var target = new RemovedBindingTarget
            {
                ProcessJobId = processJobId,
                CarrierId = carrierId,
                Slot = slot,
                Reason = reason ?? string.Empty,
                RemovedTime = DateTime.Now
            };

            lock (_sync)
            {
                _removedBindingTargetKeys.Add(target.GetKey());
                _removedBindingTargetRepository.AddOrUpdate(target);

                ProcessJob processJob =
                    _jobManager.GetProcessJobOrDefault(processJobId);

                if (processJob == null)
                    _bindingTargetIndex.RemoveProcessJob(processJobId);
                else
                    RefreshBindingTargetIndexCore(processJob);
            }
        }
        public void ClearRemovedBindingTargets(string processJobId)
        {
            if (string.IsNullOrWhiteSpace(processJobId))
                return;

            lock (_sync)
            {
                string prefix = processJobId + "|";

                var removeKeys = new List<string>();

                foreach (string key in _removedBindingTargetKeys)
                {
                    if (key.StartsWith(prefix, StringComparison.OrdinalIgnoreCase))
                        removeKeys.Add(key);
                }

                foreach (string key in removeKeys)
                    _removedBindingTargetKeys.Remove(key);

                _removedBindingTargetRepository.RemoveByProcessJob(processJobId);

                ProcessJob processJob =
                    _jobManager.GetProcessJobOrDefault(processJobId);

                if (processJob == null)
                    _bindingTargetIndex.RemoveProcessJob(processJobId);
                else
                    RefreshBindingTargetIndexCore(processJob);
            }
        }
        private bool HasActiveSubstrateBindingTarget(ProcessJob processJob)
        {
            if (processJob == null)
                return false;

            foreach (ProcessJobMaterialReference materialRef in EnumerateMaterialReferences(processJob))
            {
                if (IsRemovedBindingTarget(processJob.Id, materialRef))
                    continue;

                return true;
            }

            return false;
        }
        private void BindCarrierOnlyProcessJobToSubstrates(
            string controlJobId,
            ProcessJob processJob)
        {
            IReadOnlyList<Substrate> substrates =
                GetEligibleCarrierOnlySubstrates(
                    controlJobId,
                    processJob);

            if (substrates == null || substrates.Count == 0)
                return;

            foreach (Substrate substrate in substrates)
            {
                if (substrate == null)
                    continue;

                if (string.IsNullOrWhiteSpace(substrate.UniqueKey))
                    continue;

                _substrateManager.SetJobBindingInfoByKey(
                    substrate.UniqueKey,
                    controlJobId,
                    processJob.Id,
                    processJob.RecipeId).GetAwaiter().GetResult();
            }
        }
        private IReadOnlyList<Substrate> GetEligibleCarrierOnlySubstrates(
            string controlJobId,
            ProcessJob processJob)
        {
            var result = new List<Substrate>();

            if (processJob == null)
                return result;

            IReadOnlyDictionary<string, IReadOnlyList<int>> materialInfo =
                processJob.MaterialInfo;

            if (materialInfo == null || materialInfo.Count == 0)
                return result;

            foreach (KeyValuePair<string, IReadOnlyList<int>> item in materialInfo)
            {
                string carrierId = item.Key;
                IReadOnlyList<int> slots = item.Value;

                if (string.IsNullOrWhiteSpace(carrierId))
                    continue;

                // Slot 목록이 있는 Carrier는 기존 slot 지정 경로에서 처리한다.
                if (slots != null && slots.Count > 0)
                    continue;

                int portId =
                    _carrierManager.GetPortIdByCarrierId(carrierId);

                if (portId <= 0)
                    continue;

                Dictionary<int, Substrate> substrates =
                    _substrateManager.GetSubstratesAtLoadPort(portId);

                if (substrates == null || substrates.Count == 0)
                    continue;

                foreach (KeyValuePair<int, Substrate> itemSubstrate in substrates)
                {
                    Substrate substrate = itemSubstrate.Value;

                    if (!IsEligibleCarrierOnlySubstrate(
                        controlJobId,
                        processJob,
                        carrierId,
                        substrate))
                    {
                        continue;
                    }

                    result.Add(substrate);
                }
            }

            return result;
        }
        private static bool IsEligibleCarrierOnlySubstrate(
            string controlJobId,
            ProcessJob processJob,
            string carrierId,
            Substrate substrate)
        {
            if (processJob == null || substrate == null)
                return false;

            if (!string.Equals(
                substrate.SourceCarrierId,
                carrierId,
                StringComparison.OrdinalIgnoreCase))
            {
                return false;
            }

            if (substrate.TransportStatus != TransportStates.AtSource)
                return false;

            if (substrate.ProcessingStatus != ProcessingStates.NeedsProcessing)
                return false;

            // 다른 ProcessJob에 이미 묶여 있으면 덮어쓰지 않는다.
            if (!string.IsNullOrWhiteSpace(substrate.ProcessJobId) &&
                !string.Equals(
                    substrate.ProcessJobId,
                    processJob.Id,
                    StringComparison.OrdinalIgnoreCase))
            {
                return false;
            }

            // ControlJob이 있는 검사에서는 다른 ControlJob 바인딩을 덮어쓰지 않는다.
            if (!string.IsNullOrWhiteSpace(controlJobId) &&
                !string.IsNullOrWhiteSpace(substrate.ControlJobId) &&
                !string.Equals(
                    substrate.ControlJobId,
                    controlJobId,
                    StringComparison.OrdinalIgnoreCase))
            {
                return false;
            }

            return true;
        }
        private IReadOnlyList<Substrate> GetBoundCarrierOnlySubstrates(
            ProcessJob processJob)
        {
            var result = new List<Substrate>();

            if (processJob == null)
                return result;

            IReadOnlyDictionary<string, IReadOnlyList<int>> materialInfo =
                processJob.MaterialInfo;

            if (materialInfo == null || materialInfo.Count == 0)
                return result;

            foreach (KeyValuePair<string, IReadOnlyList<int>> item in materialInfo)
            {
                string carrierId = item.Key;
                IReadOnlyList<int> slots = item.Value;

                if (string.IsNullOrWhiteSpace(carrierId))
                    continue;

                if (slots != null && slots.Count > 0)
                    continue;

                IReadOnlyList<Substrate> substrates =
                    _substrateManager.GetSubstratesByProcessJobAndCarrier(
                        processJob.Id,
                        carrierId);

                if (substrates == null || substrates.Count == 0)
                    continue;

                foreach (Substrate substrate in substrates)
                {
                    if (substrate == null)
                        continue;

                    result.Add(substrate);
                }
            }

            return result;
        }
        private static bool HasCarrierOnlyReference(ProcessJob processJob)
        {
            if (processJob == null)
                return false;

            if (processJob.MaterialInfo == null ||
                processJob.MaterialInfo.Count == 0)
            {
                return false;
            }

            foreach (KeyValuePair<string, IReadOnlyList<int>> item in processJob.MaterialInfo)
            {
                bool hasCarrier =
                    !string.IsNullOrWhiteSpace(item.Key);

                bool hasSlots =
                    item.Value != null && item.Value.Count > 0;

                if (hasCarrier && !hasSlots)
                    return true;
            }

            return false;
        }
        private bool IsRemovedBindingTarget(
            string processJobId,
            ProcessJobMaterialReference materialRef)
        {
            if (string.IsNullOrWhiteSpace(processJobId))
                return false;

            if (materialRef == null)
                return false;

            string key = RemovedBindingTarget.CreateKey(
                processJobId,
                materialRef.CarrierId,
                materialRef.Slot);

            return _removedBindingTargetKeys.Contains(key);
        }
        private bool IsProcessJobBound(
            string controlJobId,
            ProcessJob processJob,
            JobBindingValidationMode mode)
        {
            if (processJob == null)
                return false;

            if (!HasActiveSubstrateBindingTarget(processJob))
                return IsCarrierOnlyProcessJobBound(
                    controlJobId,
                    processJob,
                    mode);

            foreach (ProcessJobMaterialReference materialRef in EnumerateMaterialReferences(processJob))
            {
                if (IsRemovedBindingTarget(processJob.Id, materialRef))
                    continue;

                int portId =
                    _carrierManager.GetPortIdByCarrierId(materialRef.CarrierId);

                if (portId <= 0)
                {
                    if (IsTransferredProcessJobBound(
                        controlJobId,
                        processJob,
                        materialRef,
                        0,
                        mode))
                    {
                        continue;
                    }

                    return false;
                }

                string substrateKey =
                    _substrateManager.GetSubstrateKeyAtLoadPort(
                        portId,
                        materialRef.Slot);

                if (string.IsNullOrWhiteSpace(substrateKey))
                {
                    if (IsTransferredProcessJobBound(
                        controlJobId,
                        processJob,
                        materialRef,
                        portId,
                        mode))
                    {
                        continue;
                    }

                    return false;
                }

                Substrate substrate;

                if (!_substrateManager.GetSubstrateByKey(substrateKey, out substrate) ||
                    substrate == null)
                {
                    if (IsTransferredProcessJobBound(
                        controlJobId,
                        processJob,
                        materialRef,
                        portId,
                        mode))
                    {
                        continue;
                    }

                    return false;
                }

                if (!IsSubstrateBoundToJob(
                    controlJobId,
                    processJob,
                    substrate,
                    mode))
                {
                    return false;
                }
            }

            return true;
        }
        private bool IsCarrierOnlyProcessJobBound(
            string controlJobId,
            ProcessJob processJob,
            JobBindingValidationMode mode)
        {
            if (processJob == null)
                return false;

            if (!HasCarrierOnlyReference(processJob))
                return true;

            IReadOnlyList<Substrate> boundSubstrates =
                GetBoundCarrierOnlySubstrates(processJob);

            if (boundSubstrates != null && boundSubstrates.Count > 0)
            {
                foreach (Substrate substrate in boundSubstrates)
                {
                    if (!IsSubstrateBoundToJob(
                        controlJobId,
                        processJob,
                        substrate,
                        mode))
                    {
                        return false;
                    }
                }

                return true;
            }

            IReadOnlyList<Substrate> eligibleSubstrates =
                GetEligibleCarrierOnlySubstrates(
                    controlJobId,
                    processJob);

            // 조건에 맞는 기판이 없으면 NoTarget 정책 유지.
            if (eligibleSubstrates == null || eligibleSubstrates.Count == 0)
                return true;

            // 조건에 맞는 기판이 있는데 아직 바인딩되지 않았다면 Bound가 아니다.
            return false;
        }
        private static bool IsSubstrateBoundToJob(
            string controlJobId,
            ProcessJob processJob,
            Substrate substrate,
            JobBindingValidationMode mode)
        {
            if (processJob == null || substrate == null)
                return false;

            if (!string.Equals(
                substrate.ProcessJobId,
                processJob.Id,
                StringComparison.OrdinalIgnoreCase))
            {
                return false;
            }

            if (mode == JobBindingValidationMode.ProcessJobOnly)
                return true;

            if (string.IsNullOrWhiteSpace(controlJobId))
                return false;

            return string.Equals(
                substrate.ControlJobId,
                controlJobId,
                StringComparison.OrdinalIgnoreCase);
        }
        private bool IsTransferredProcessJobBound(
            string controlJobId,
            ProcessJob processJob,
            ProcessJobMaterialReference materialRef,
            int portId,
            JobBindingValidationMode mode)
        {
            if (processJob == null)
                return false;

            if (materialRef == null)
                return false;

            IReadOnlyList<Substrate> substrates =
                _substrateManager.GetSubstratesByJobInfo(
                    portId,
                    materialRef.Slot,
                    materialRef.CarrierId,
                    processJob.Id);

            if (substrates == null || substrates.Count == 0)
                return false;

            foreach (Substrate substrate in substrates)
            {
                if (IsSubstrateBoundToJob(
                    controlJobId,
                    processJob,
                    substrate,
                    mode))
                {
                    return true;
                }
            }

            return false;
        }
        private void UnbindProcessJobFromSubstrates(
            string controlJobId,
            ProcessJob processJob)
        {
            if (processJob == null)
                return;

            if (!HasActiveSubstrateBindingTarget(processJob))
            {
                UnbindCarrierOnlyProcessJobFromSubstrates(
                    controlJobId,
                    processJob);

                return;
            }

            foreach (ProcessJobMaterialReference materialRef in EnumerateMaterialReferences(processJob))
            {
                if (IsRemovedBindingTarget(processJob.Id, materialRef))
                    continue;

                int portId =
                    _carrierManager.GetPortIdByCarrierId(materialRef.CarrierId);

                if (portId <= 0)
                    continue;

                string substrateKey =
                    _substrateManager.GetSubstrateKeyAtLoadPort(
                        portId,
                        materialRef.Slot);

                if (string.IsNullOrWhiteSpace(substrateKey))
                    continue;

                _substrateManager.ClearJobBindingInfoByKey(
                    substrateKey,
                    controlJobId,
                    processJob.Id,
                    clearRecipeId: false).GetAwaiter().GetResult();
            }
        }
        private void UnbindCarrierOnlyProcessJobFromSubstrates(
            string controlJobId,
            ProcessJob processJob)
        {
            IReadOnlyList<Substrate> substrates =
                GetBoundCarrierOnlySubstrates(processJob);

            if (substrates == null || substrates.Count == 0)
                return;

            foreach (Substrate substrate in substrates)
            {
                if (substrate == null)
                    continue;

                if (string.IsNullOrWhiteSpace(substrate.UniqueKey))
                    continue;

                _substrateManager.ClearJobBindingInfoByKey(
                    substrate.UniqueKey,
                    controlJobId,
                    processJob.Id,
                    clearRecipeId: false).GetAwaiter().GetResult();
            }
        }
        private IEnumerable<ProcessJobMaterialReference> EnumerateMaterialReferences(
            ProcessJob processJob)
        {
            if (processJob == null)
                yield break;

            IReadOnlyDictionary<string, IReadOnlyList<int>> materialInfo =
                processJob.MaterialInfo;

            if (materialInfo == null || materialInfo.Count == 0)
                yield break;

            foreach (KeyValuePair<string, IReadOnlyList<int>> item in materialInfo)
            {
                string carrierId = item.Key;
                IReadOnlyList<int> slots = item.Value;

                /*
                 * 캐리어 없음 + 슬롯 있음은 JobManager 검증에서 막혀야 한다.
                 * Binder에서는 방어적으로 skip한다.
                 */
                if (string.IsNullOrWhiteSpace(carrierId))
                    continue;

                /*
                 * 캐리어 있음 + 슬롯 없음은 정상이다.
                 * 다만 특정 Substrate를 찾을 수 없으므로 Substrate 바인딩 대상은 아니다.
                 */
                if (slots == null || slots.Count == 0)
                    continue;

                foreach (int slot in slots)
                {
                    if (slot <= 0)
                        continue;

                    yield return new ProcessJobMaterialReference(
                        carrierId,
                        slot);
                }
            }
        }
        private static bool HasSubstrateBindingTarget(ProcessJob processJob)
        {
            if (processJob == null)
                return false;

            if (processJob.MaterialInfo == null || processJob.MaterialInfo.Count == 0)
                return false;

            foreach (KeyValuePair<string, IReadOnlyList<int>> item in processJob.MaterialInfo)
            {
                bool hasCarrier = !string.IsNullOrWhiteSpace(item.Key);
                bool hasSlots = item.Value != null && item.Value.Count > 0;

                if (hasCarrier && hasSlots)
                    return true;
            }

            return false;
        }
        public JobBindingSnapshot GetBindingSnapshot(
            string controlJobId,
            string processJobId)
        {
            var snapshot = new JobBindingSnapshot
            {
                ControlJobId = controlJobId ?? string.Empty,
                ProcessJobId = processJobId ?? string.Empty,
                Status = JobBindingStatus.NoTarget,
                Message = string.Empty
            };

            if (string.IsNullOrWhiteSpace(processJobId))
            {
                snapshot.Status = JobBindingStatus.Invalid;
                snapshot.Message = "ProcessJobId is empty.";
                return snapshot;
            }

            lock (_sync)
            {
                ProcessJob processJob =
                    _jobManager.GetProcessJobOrDefault(processJobId);

                if (processJob == null)
                {
                    snapshot.Status = JobBindingStatus.Invalid;
                    snapshot.Message = "ProcessJob does not exist.";
                    return snapshot;
                }

                if (!HasActiveSubstrateBindingTarget(processJob))
                {
                    ApplyCarrierOnlyBindingSnapshot(
                        snapshot,
                        controlJobId,
                        processJob);

                    return snapshot;
                }

                bool hasBound = false;
                bool hasTransferred = false;
                bool hasPending = false;
                bool hasInvalid = false;

                foreach (ProcessJobMaterialReference materialRef in EnumerateMaterialReferences(processJob))
                {
                    if (IsRemovedBindingTarget(processJob.Id, materialRef))
                        continue;

                    JobBindingSnapshot.Material materialSnapshot =
                        CreateMaterialBindingSnapshot(
                            controlJobId,
                            processJob,
                            materialRef);

                    snapshot.Materials.Add(materialSnapshot);

                    if (materialSnapshot.Status == JobBindingStatus.Invalid)
                        hasInvalid = true;
                    else if (materialSnapshot.Status == JobBindingStatus.Pending)
                        hasPending = true;
                    else if (materialSnapshot.Status == JobBindingStatus.Transferred)
                        hasTransferred = true;
                    else if (materialSnapshot.Status == JobBindingStatus.Bound)
                        hasBound = true;
                }

                if (hasInvalid)
                {
                    snapshot.Status = JobBindingStatus.Invalid;
                    snapshot.Message = "One or more material bindings are invalid.";
                }
                else if (hasPending)
                {
                    snapshot.Status = JobBindingStatus.Pending;
                    snapshot.Message = "One or more material bindings are pending.";
                }
                else if (hasTransferred)
                {
                    /*
                     * 하나라도 Transferred가 있으면 ProcessJob 전체를 Transferred로 표시한다.
                     * Binding 자체는 유효하지만 현재 LoadPort Slot에는 없는 상태다.
                     */
                    snapshot.Status = JobBindingStatus.Transferred;
                    snapshot.Message = "One or more bound substrates were transferred from load port.";
                }
                else if (hasBound)
                {
                    snapshot.Status = JobBindingStatus.Bound;
                    snapshot.Message = "All material bindings are bound at load port.";
                }
                else
                {
                    snapshot.Status = JobBindingStatus.NoTarget;
                    snapshot.Message = "No substrate binding target.";
                }

                return snapshot;
            }
        }
        private void ApplyCarrierOnlyBindingSnapshot(
            JobBindingSnapshot snapshot,
            string controlJobId,
            ProcessJob processJob)
        {
            if (snapshot == null || processJob == null)
                return;

            if (!HasCarrierOnlyReference(processJob))
            {
                snapshot.Status = JobBindingStatus.NoTarget;
                snapshot.Message = "No substrate binding target.";
                return;
            }

            IReadOnlyList<Substrate> substrates =
                GetBoundCarrierOnlySubstrates(processJob);

            if (substrates == null || substrates.Count == 0)
            {
                substrates =
                    GetEligibleCarrierOnlySubstrates(
                        controlJobId,
                        processJob);
            }

            if (substrates == null || substrates.Count == 0)
            {
                snapshot.Status = JobBindingStatus.NoTarget;
                snapshot.Message = "No eligible substrate for carrier-only binding.";
                return;
            }

            bool hasBound = false;
            bool hasPending = false;
            bool hasInvalid = false;

            foreach (Substrate substrate in substrates)
            {
                JobBindingSnapshot.Material material =
                    CreateCarrierOnlyMaterialBindingSnapshot(
                        controlJobId,
                        processJob,
                        substrate);

                snapshot.Materials.Add(material);

                if (material.Status == JobBindingStatus.Invalid)
                    hasInvalid = true;
                else if (material.Status == JobBindingStatus.Pending)
                    hasPending = true;
                else if (material.Status == JobBindingStatus.Bound ||
                         material.Status == JobBindingStatus.Transferred)
                    hasBound = true;
            }

            if (hasInvalid)
            {
                snapshot.Status = JobBindingStatus.Invalid;
                snapshot.Message = "One or more carrier-only bindings are invalid.";
            }
            else if (hasPending)
            {
                snapshot.Status = JobBindingStatus.Pending;
                snapshot.Message = "One or more carrier-only bindings are pending.";
            }
            else if (hasBound)
            {
                snapshot.Status = JobBindingStatus.Bound;
                snapshot.Message = "Carrier-only substrates are bound.";
            }
            else
            {
                snapshot.Status = JobBindingStatus.NoTarget;
                snapshot.Message = "No carrier-only binding target.";
            }
        }
        private JobBindingSnapshot.Material CreateCarrierOnlyMaterialBindingSnapshot(
            string controlJobId,
            ProcessJob processJob,
            Substrate substrate)
        {
            var snapshot = new JobBindingSnapshot.Material
            {
                CarrierId = substrate == null ? string.Empty : substrate.SourceCarrierId,
                Slot = substrate == null ? 0 : substrate.SourceSlot,
                PortId = substrate == null ? 0 : substrate.SourcePortId,
                Status = JobBindingStatus.Pending
            };

            if (substrate == null)
            {
                snapshot.Status = JobBindingStatus.Invalid;
                snapshot.Message = "Substrate is null.";
                return snapshot;
            }

            snapshot.SubstrateId = substrate.UniqueKey ?? string.Empty;
            snapshot.BoundControlJobId = substrate.ControlJobId;
            snapshot.BoundProcessJobId = substrate.ProcessJobId;
            snapshot.BoundRecipeId = substrate.RecipeId;

            if (string.IsNullOrWhiteSpace(substrate.ProcessJobId))
            {
                snapshot.Status = JobBindingStatus.Pending;
                snapshot.Message = "Eligible carrier-only substrate is not bound.";
                return snapshot;
            }

            if (!string.Equals(
                substrate.ProcessJobId,
                processJob.Id,
                StringComparison.OrdinalIgnoreCase))
            {
                snapshot.Status = JobBindingStatus.Invalid;
                snapshot.Message = "Carrier-only substrate is bound to another ProcessJob.";
                return snapshot;
            }

            if (!string.IsNullOrWhiteSpace(controlJobId) &&
                !string.Equals(
                substrate.ControlJobId,
                controlJobId,
                StringComparison.OrdinalIgnoreCase))
            {
                if (string.IsNullOrWhiteSpace(substrate.ControlJobId))
                {
                    snapshot.Status = JobBindingStatus.Pending;
                    snapshot.Message = "Carrier-only substrate is not bound to ControlJob.";
                }
                else
                {
                    snapshot.Status = JobBindingStatus.Invalid;
                    snapshot.Message = "Carrier-only substrate is bound to another ControlJob.";
                }

                return snapshot;
            }

            snapshot.Status =
                substrate.TransportStatus == TransportStates.AtSource
                    ? JobBindingStatus.Bound
                    : JobBindingStatus.Transferred;

            snapshot.Message =
                snapshot.Status == JobBindingStatus.Bound
                    ? "Carrier-only substrate is bound at source."
                    : "Carrier-only substrate was transferred from source.";

            return snapshot;
        }
        private JobBindingSnapshot.Material CreateMaterialBindingSnapshot(
            string controlJobId,
            ProcessJob processJob,
            ProcessJobMaterialReference materialRef)
        {
            var snapshot = new JobBindingSnapshot.Material
            {
                CarrierId = materialRef.CarrierId,
                Slot = materialRef.Slot,
                Status = JobBindingStatus.Pending
            };

            int portId = _carrierManager.GetPortIdByCarrierId(materialRef.CarrierId);
            snapshot.PortId = portId;

            if (portId <= 0)
            {
                if (TryApplyTransferredJobBinding(
                    snapshot,
                    0,
                    materialRef,
                    controlJobId,
                    processJob))
                {
                    return snapshot;
                }

                snapshot.Status = JobBindingStatus.Pending;
                snapshot.Message = "Carrier is not arrived or port is unknown.";
                return snapshot;
            }

            string substrateKey =
                _substrateManager.GetSubstrateKeyAtLoadPort(
                    portId,
                    materialRef.Slot);

            snapshot.SubstrateId = substrateKey ?? string.Empty;

            if (string.IsNullOrWhiteSpace(substrateKey))
            {
                if (TryApplyTransferredJobBinding(
                    snapshot,
                    portId,
                    materialRef,
                    controlJobId,
                    processJob))
                {
                    return snapshot;
                }

                snapshot.Status = JobBindingStatus.Pending;
                snapshot.Message = "Substrate does not exist at the load port slot.";
                return snapshot;
            }

            Substrate substrate;

            if (!_substrateManager.GetSubstrateByKey(substrateKey, out substrate) || substrate == null)
            {
                if (TryApplyTransferredJobBinding(
                    snapshot,
                    portId,
                    materialRef,
                    controlJobId,
                    processJob))
                {
                    return snapshot;
                }

                snapshot.Status = JobBindingStatus.Pending;
                snapshot.Message = "Substrate lookup failed.";
                return snapshot;
            }

            snapshot.BoundControlJobId = substrate.ControlJobId;
            snapshot.BoundProcessJobId = substrate.ProcessJobId;
            snapshot.BoundRecipeId = substrate.RecipeId;

            if (!string.Equals(substrate.ProcessJobId, processJob.Id, StringComparison.Ordinal))
            {
                if (string.IsNullOrWhiteSpace(substrate.ProcessJobId))
                {
                    snapshot.Status = JobBindingStatus.Pending;
                    snapshot.Message = "Substrate is not bound to ProcessJob.";
                }
                else
                {
                    snapshot.Status = JobBindingStatus.Invalid;
                    snapshot.Message = "Substrate is bound to another ProcessJob.";
                }

                return snapshot;
            }

            if (!string.IsNullOrWhiteSpace(controlJobId) &&
                !string.Equals(substrate.ControlJobId, controlJobId, StringComparison.Ordinal))
            {
                if (string.IsNullOrWhiteSpace(substrate.ControlJobId))
                {
                    snapshot.Status = JobBindingStatus.Pending;
                    snapshot.Message = "Substrate is not bound to ControlJob.";
                }
                else
                {
                    snapshot.Status = JobBindingStatus.Invalid;
                    snapshot.Message = "Substrate is bound to another ControlJob.";
                }

                return snapshot;
            }

            snapshot.Status = JobBindingStatus.Bound;
            snapshot.Message = "Bound at load port slot.";

            return snapshot;
        }
        private bool TryApplyTransferredJobBinding(
            JobBindingSnapshot.Material snapshot,
            int portId,
            ProcessJobMaterialReference materialRef,
            string controlJobId,
            ProcessJob processJob)
        {
            IReadOnlyList<Substrate> substrates =
                _substrateManager.GetSubstratesByJobInfo(
                    portId,
                    materialRef.Slot,
                    materialRef.CarrierId,
                    processJob.Id);

            if (substrates == null || substrates.Count == 0)
                return false;

            Substrate substrate = substrates[0];

            if (substrate == null)
                return false;

            snapshot.BoundControlJobId = substrate.ControlJobId;
            snapshot.BoundProcessJobId = substrate.ProcessJobId;
            snapshot.BoundRecipeId = substrate.RecipeId;

            if (!string.IsNullOrWhiteSpace(controlJobId) &&
                !string.Equals(substrate.ControlJobId, controlJobId, StringComparison.OrdinalIgnoreCase))
            {
                snapshot.Status = JobBindingStatus.Invalid;
                snapshot.Message = "Transferred substrate is bound to another ControlJob.";
                return true;
            }

            snapshot.Status = JobBindingStatus.Transferred;
            snapshot.Message = "Bound substrate was transferred from load port.";

            return true;
        }

        private void RebuildBindingTargetIndexCore()
        {
            _bindingTargetIndex.Clear();

            IReadOnlyList<ProcessJob> processJobs =
                _jobManager.GetAllProcessJobs();

            if (processJobs == null)
                return;

            foreach (ProcessJob processJob in processJobs)
            {
                if (processJob == null)
                    continue;

                RefreshBindingTargetIndexCore(processJob);
            }
        }

        private void RefreshBindingTargetIndexCore(ProcessJob processJob)
        {
            if (processJob == null)
                return;

            IReadOnlyList<JobBindingTarget> targets =
                CreateActiveBindingTargets(processJob);

            IReadOnlyList<string> carrierReferences =
                CreateCarrierReferences(processJob);

            _bindingTargetIndex.AddOrUpdateProcessJob(
                processJob.Id,
                targets);

            _bindingTargetIndex.AddOrUpdateProcessJobCarrierReferences(
                processJob.Id,
                carrierReferences);
        }
        private IReadOnlyList<string> CreateCarrierReferences(
            ProcessJob processJob)
        {
            var result = new List<string>();

            if (processJob == null)
                return result;

            IReadOnlyDictionary<string, IReadOnlyList<int>> materialInfo =
                processJob.MaterialInfo;

            if (materialInfo == null || materialInfo.Count == 0)
                return result;

            foreach (KeyValuePair<string, IReadOnlyList<int>> item in materialInfo)
            {
                string carrierId = item.Key;

                if (string.IsNullOrWhiteSpace(carrierId))
                    continue;

                if (!ContainsIgnoreCase(result, carrierId))
                    result.Add(carrierId);
            }

            return result;
        }
        private IReadOnlyList<JobBindingTarget> CreateActiveBindingTargets(
            ProcessJob processJob)
        {
            var result = new List<JobBindingTarget>();

            if (processJob == null)
                return result;

            foreach (ProcessJobMaterialReference materialRef in EnumerateMaterialReferences(processJob))
            {
                if (IsRemovedBindingTarget(processJob.Id, materialRef))
                    continue;

                int sourcePortId =
                    _carrierManager.GetPortIdByCarrierId(materialRef.CarrierId);

                result.Add(
                    new JobBindingTarget(
                        processJob.Id,
                        materialRef.CarrierId,
                        sourcePortId,
                        materialRef.Slot));
            }

            return result;
        }
    }
}