namespace EFEM.Defines.Job
{
    public interface IControlJobServiceCallback
    {
        void OnCreated(ControlJobCreatedEventArgs e);

        void OnStateChanged(ControlJobStateChangedEventArgs e);

        void OnDeleted(ControlJobDeletedEventArgs e);

        void OnVerifyRequestedByHost(ControlJobVerifyRequestedEventArgs e);

        void OnCommandRequestedByHost(ControlJobCommandRequestedEventArgs e);

        void OnManualStartRequired(ControlJobManualStartEventArgs e);

        void OnHeadOfQueueChanged(ControlJobHoqChangedEventArgs e);
    }
}
