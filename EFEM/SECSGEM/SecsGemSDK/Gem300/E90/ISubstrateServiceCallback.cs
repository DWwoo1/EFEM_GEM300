namespace EFEM.Defines.MaterialTracking
{
    public interface ISubstrateServiceCallback
    {
        void OnCreated(SubstrateCreatedEventArgs e);

        void OnDeleted(SubstrateDeletedEventArgs e);

        void OnTransportChanged(SubstrateTransportStateChangedEventArgs e);

        void OnProcessingChanged(SubstrateProcessingStateChangedEventArgs e);

        void OnReadingChanged(SubstrateReadingStateChangedEventArgs e);

        void OnCreateRequestedByHost(SubstrateCreateRequestedEventArgs e);

        void OnUpdateRequestedByHost(SubstrateUpdateRequestedEventArgs e);

        void OnDeleteRequestedByHost(SubstrateDeleteRequestedEventArgs e);

        void OnCancelRequestedByHost(SubstrateCancelRequestedEventArgs e);

        void OnConfirmationDisplayed(SubstrateConfirmEventArgs e);

        void OnConfirmationSucceeded(SubstrateConfirmEventArgs e);

        void OnConfirmationFailed(SubstrateConfirmFailedEventArgs e);
    }
}
