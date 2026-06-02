namespace EFEM.Defines.CarrierManagement
{
    public interface ICarrierServiceCallback
    {
        void OnCarrierInStarted(CarrierPortCarrierEventArgs e);
        void OnCarrierDeleted(CarrierDeletedEventArgs e);
        void OnTransferStateChanged(LoadPortStateChangedEventArgs e);
        void OnAccessModeChanged(LoadPortStateChangedEventArgs e);
        void OnVerificationSucceeded(CarrierVerificationSucceededEventArgs e);
        void OnVerificationResultWithoutRemote(CarrierVerificationResultWithoutRemoteArgs e);
        void OnVerificationFailed(CarrierVerificationFailedEventArgs e);
        void OnCarrierInRequestedByHost(HostCarrierRequestEventArgs e);
        void OnCarrierOutRequestedByHost(HostCarrierRequestEventArgs e);
        void OnCarrierCancelRequestedByHost(HostCarrierRequestEventArgs e);
        void OnAccessChangeRequestedByHost(HostChangeAccessRequestEventArgs e);
        void OnServiceStatusChangeRequestedByHost(HostChangeServiceStatusRequestEventArgs e);
    }
}