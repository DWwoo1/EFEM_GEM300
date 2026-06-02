namespace EFEM.Jobs.Completion
{
    public interface IJobCompletionEvaluator
    {
        /// <summary>
        /// ProcessJob에 포함된 모든 실제 재료가 Processed 상태인지 확인한다.
        /// 재료 대상이 없는 Job은 자동 완료로 보지 않으므로 false를 반환한다.
        /// </summary>
        bool AreAllMaterialsProcessed(string processJobId);

        /// <summary>
        /// ControlJob에 포함된 모든 ProcessJob이 완료되었는지 확인한다.
        /// 재료가 있는 Job은 재료 완료 기준, 재료가 없는 Job은 ProcessJob 상태 기준으로 본다.
        /// </summary>
        bool AreAllProcessJobsCompleted(string controlJobId);
    }
}