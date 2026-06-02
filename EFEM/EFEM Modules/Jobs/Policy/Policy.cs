using EFEM.Defines.Job;
using EFEM.Jobs.Domain;

namespace EFEM.Jobs.Policy
{
    public interface IProcessJobRemovalPolicy
    {
        bool ShouldRemoveLocalOnSdkDeleted(
            ProcessJob processJob,
            string linkedControlJobId);

        bool ShouldRequestSdkRemoveOnControlJobRemoval(
            ProcessJob processJob);

        bool ShouldIgnoreSdkRemoveFailureOnControlJobRemoval(
            ProcessJob processJob,
            long sdkRemoveResult);
    }

    public sealed class ImmediateProcessJobRemovalPolicy
        : IProcessJobRemovalPolicy
    {
        public bool ShouldRemoveLocalOnSdkDeleted(
            ProcessJob processJob,
            string linkedControlJobId)
        {
            return true;
        }

        public bool ShouldRequestSdkRemoveOnControlJobRemoval(
            ProcessJob processJob)
        {
            return true;
        }

        public bool ShouldIgnoreSdkRemoveFailureOnControlJobRemoval(
            ProcessJob processJob,
            long sdkRemoveResult)
        {
            return false;
        }
    }

    public sealed class RetainLinkedProcessJobUntilControlJobRemovalPolicy
           : IProcessJobRemovalPolicy
    {
        public bool ShouldRemoveLocalOnSdkDeleted(
            ProcessJob processJob,
            string linkedControlJobId)
        {
            // ControlJob에 연결되지 않은 고아 ProcessJob은 기존처럼 즉시 삭제한다.
            if (string.IsNullOrWhiteSpace(linkedControlJobId))
                return true;

            // ControlJob에 연결된 ProcessJob은 SDK Delete 이벤트만으로 로컬 삭제하지 않는다.
            return false;
        }

        public bool ShouldRequestSdkRemoveOnControlJobRemoval(
            ProcessJob processJob)
        {
            if (processJob == null)
                return false;

            // terminal ProcessJob은 SDK에서 자동 제거되므로 Remove 재호출을 생략한다.
            return !IsTerminalProcessJobState(processJob.State);
        }

        public bool ShouldIgnoreSdkRemoveFailureOnControlJobRemoval(
            ProcessJob processJob,
            long sdkRemoveResult)
        {
            if (processJob == null)
                return false;

            // terminal ProcessJob은 이미 SDK에서 제거되었을 수 있으므로 실패를 무시한다.
            return IsTerminalProcessJobState(processJob.State);
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