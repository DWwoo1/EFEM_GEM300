using System;
using System.Collections.Generic;

using EFEM.Defines.Job;
using EFEM.Jobs.Binding;
using EFEM.Jobs.Domain;
using EFEM.Jobs.Manager;
using EFEM.MaterialTracking;
using EFEM.Defines.MaterialTracking;

namespace EFEM.Jobs.Completion
{
    public sealed class JobCompletionEvaluator : IJobCompletionEvaluator
    {
        private readonly IJobManager _jobManager;
        private readonly ISubstrateJobBinder _jobBinder;
        private readonly SubstrateManager _substrateManager;

        public JobCompletionEvaluator(
            IJobManager jobManager,
            ISubstrateJobBinder jobBinder,
            SubstrateManager substrateManager)
        {
            if (jobManager == null)
                throw new ArgumentNullException(nameof(jobManager));

            if (jobBinder == null)
                throw new ArgumentNullException(nameof(jobBinder));

            if (substrateManager == null)
                throw new ArgumentNullException(nameof(substrateManager));

            _jobManager = jobManager;
            _jobBinder = jobBinder;
            _substrateManager = substrateManager;
        }

        public bool AreAllMaterialsProcessed(string processJobId)
        {
            if (string.IsNullOrWhiteSpace(processJobId))
                return false;

            ProcessJob processJob =
                _jobManager.GetProcessJobOrDefault(processJobId);

            if (processJob == null)
                return false;

            string controlJobId =
                _jobManager.GetControlJobIdOrDefault(processJobId);

            JobBindingSnapshot snapshot =
                _jobBinder.GetBindingSnapshot(
                    controlJobId,
                    processJobId);

            if (snapshot == null)
                return false;

            /*
             * Carrier는 있지만 Slot 목록이 없는 Job은
             * 실제 Substrate 재료 완료로 판단할 수 없다.
             * 자동 완료가 아니라 수동 처리를 기다려야 하므로 false.
             */
            if (snapshot.Status == JobBindingStatus.NoTarget)
                return false;

            if (snapshot.Status == JobBindingStatus.Pending ||
                snapshot.Status == JobBindingStatus.Invalid)
            {
                return false;
            }

            if (snapshot.Materials == null || snapshot.Materials.Count == 0)
                return false;

            foreach (JobBindingSnapshot.Material material in snapshot.Materials)
            {
                if (material == null)
                    return false;

                if (!IsMaterialProcessed(
                    processJobId,
                    material))
                {
                    return false;
                }
            }

            return true;
        }

        public bool AreAllProcessJobsCompleted(string controlJobId)
        {
            if (string.IsNullOrWhiteSpace(controlJobId))
                return false;

            IReadOnlyList<ProcessJob> processJobs =
                _jobManager.GetLinkedProcessJobs(controlJobId);

            if (processJobs == null || processJobs.Count == 0)
                return false;

            foreach (ProcessJob processJob in processJobs)
            {
                if (processJob == null)
                    return false;

                if (HasSubstrateMaterialTarget(processJob))
                {
                    if (!AreAllMaterialsProcessed(processJob.Id))
                        return false;

                    continue;
                }

                /*
                 * 재료가 없는 Job은 자동 재료 완료로 보지 않는다.
                 * 수동 처리 결과로 ProcessJob 상태가 완료 계열이 되어야 한다.
                 */
                if (!IsTerminalProcessJobState(processJob.State))
                    return false;
            }

            return true;
        }

        private bool IsMaterialProcessed(
            string processJobId,
            JobBindingSnapshot.Material material)
        {
            Substrate substrate;

            if (!string.IsNullOrWhiteSpace(material.SubstrateId))
            {
                if (_substrateManager.GetSubstrateByKey(
                    material.SubstrateId,
                    out substrate) &&
                    substrate != null)
                {
                    return IsSubstrateProcessed(substrate);
                }
            }

            IReadOnlyList<Substrate> substrates =
                _substrateManager.GetSubstratesByJobInfo(
                    material.PortId,
                    material.Slot,
                    material.CarrierId,
                    processJobId);

            if (substrates == null || substrates.Count == 0)
                return false;

            foreach (Substrate item in substrates)
            {
                if (item == null)
                    continue;

                if (IsSubstrateProcessed(item))
                    return true;
            }

            return false;
        }

        private static bool IsSubstrateProcessed(Substrate substrate)
        {
            if (substrate == null)
                return false;

            return substrate.ProcessingStatus == ProcessingStates.Processed;
        }

        private static bool HasSubstrateMaterialTarget(ProcessJob processJob)
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

                if (hasCarrier && hasSlots)
                    return true;
            }

            return false;
        }

        private static bool IsTerminalProcessJobState(ProcessJobState state)
        {
            return state == ProcessJobState.ProcessComplete
                || state == ProcessJobState.Stopped
                || state == ProcessJobState.Aborted
                || state == ProcessJobState.JobCanceled
                || state == ProcessJobState.JobComplete;
        }
    }
}