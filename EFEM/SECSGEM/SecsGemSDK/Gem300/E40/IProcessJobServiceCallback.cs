namespace EFEM.Defines.Job
{
    public interface IProcessJobServiceCallback
    {
        void OnCreated(ProcessJobCreatedEventArgs e);
        void OnStateChanged(ProcessJobStateChangedEventArgs e);
        void OnDeleted(ProcessJobDeletedEventArgs e);
        void OnManualStartRequired(ProcessJobManualStartEventArgs e);
        void OnSettingUpRequested(ProcessJobSettingUpEventArgs e);

        // 필요시 추후 분리
        void OnVerifyRequestedByHost(ProcessJobVerifyRequestedEventArgs e);
        void OnCommandRequestedByHost(ProcessJobCommandRequestedEventArgs e);
        void OnRecipeVariablesRequestedByHost(ProcessJobRecipeVariableRequestedEventArgs e);
        void OnStartMethodRequestedByHost(ProcessJobStartMethodRequestedEventArgs e);
        void OnMaterialOrderRequestedByHost(ProcessJobMaterialOrderRequestedEventArgs e);
    }
}
