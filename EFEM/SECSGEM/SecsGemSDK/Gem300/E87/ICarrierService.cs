using System;
using System.Collections.Generic;

using EFEM.Defines.LoadPort;
using FrameOfSystem3.SECSGEM.DefineSecsGem;

namespace EFEM.Defines.CarrierManagement
{
    public interface ICarrierService
    {
        bool IsDriverAttached { get; }
        void RegisterCallback(
            string locationName,
            ICarrierServiceCallback callback);
        void AttachDriver(ICarrierManagementDriver driver);
        void DetachDriver();
        void UnregisterCallback(string locationName);

        long NotifyCarrierDetection(string locationId, string carrierId, CarrierIdVerificationStates idVerificationResult, bool detectionStatus);
        long Bind(string locationId, string carrierId, string slotMap);
        long CancelBinding(string locationId, string carrierId);
        //long RequestCarrierIn(string locationId, string carrierId);
        //long RequestCarrierOut(string locationId, string carrierId);
        long RequestCarrierRecreate(string locationId, string carrierId);
        long RequestCancelCarrier(string locationId, string carrierId);
        long RequestProceedCarrier(
            string locationId,
            string carrierId,
            IReadOnlyDictionary<int, CarrierSlotMapStates> map,
            IReadOnlyDictionary<int, string> lots,
            IReadOnlyDictionary<int, string> substrateNames,
            string usage);

        long AcknowledgeCarrierIn(long messageId, string locationId, string carrierId, long result, long[] errorCodes, string[] errorTexts);
        long AcknowledgeCarrierOut(long messageId, string locationId, string carrierId, long result, long[] errorCodes, string[] errorTexts);
        long AcknowledgeCancelCarrier(long messageId, string locationId, string carrierId, long result, long[] errorCodes, string[] errorTexts);
        long AcknowledgeCarrierRelease(long messageId, string locationId, string carrierId, long result, long[] errorCodes, string[] errorTexts);
        long AcknowledgeChangeAccess(long messageId, long mode, long result, string[] locationIds, long[] errorCodes, string[] errorTexts);
        long AcknowledgeChangeServiceStatus(long messageId, string locationId, long state, long result, long[] errorCodes, string[] errorTexts);
        long SetLoadPortInfo(string locationId, LoadPortStateInformation state, string carrierId);
        long ChangeAccessMode(string locationId, LoadPortAccessMode mode);
        long SetCarrierLocation(string locationId, string carrierId);
        long SetCarrierMovement(string locationId, string carrierId);
        long SetCarrierAccessing(string locationId, CarrierAccessStates state, string carrierId);
        long SetCarrierIdentifier(string locationId, string carrierId, VerificationResult result);
        long SetCarrierIdStatus(string carrierId, CarrierIdVerificationStates state);
        long SetSlotMap(string locationId, IReadOnlyDictionary<int, CarrierSlotMapStates> map, string carrierId, VerificationResult result);
        long SetSlotMapStatus(string carrierId, CarrierSlotMapStates state);
        long SetCarrierInfo(string carrierId,
            string locationId,
            CarrierIdVerificationStates carrierIdStatus,
            CarrierSlotMapStates slotMapStatus,
            CarrierAccessStates accessingStatus,
            IReadOnlyDictionary<int, CarrierSlotMapStates> map,
            string[] lotIds, string[] substrateIds, string usage);

        long SetCarrierOutStart(string locationId, string carrierId);
        long SetSubstrateCount(string carrierId, long substrateCount);
        long SetUsage(string carrierId, string usage);
        long SetMaterialArrived(string materialId);
        long SetPioSignal(string locationId, long signal, long state);
        long SetReadyToLoad(string locationId);
        long SetReadyToUnload(string locationId);
        //long SetTransferReady(string locationId, long state);
    }

    public sealed class CarrierPortCarrierEventArgs : EventArgs
    {
        public CarrierPortCarrierEventArgs(string locationId, string carrierId) { LocationId = locationId; CarrierId = carrierId; }
        public string LocationId { get; }
        public string CarrierId { get; }
    }

    public sealed class CarrierDeletedEventArgs : EventArgs
    {
        public CarrierDeletedEventArgs(string carrierId) { CarrierId = carrierId; }
        public string CarrierId { get; }
    }

    public sealed class LoadPortStateChangedEventArgs : EventArgs
    {
        public LoadPortStateChangedEventArgs(string locationId, long state) { LocationId = locationId; State = state; }
        public string LocationId { get; }
        public long State { get; }
    }

    public sealed class CarrierVerificationSucceededEventArgs : EventArgs
    {
        public CarrierVerificationSucceededEventArgs(VerificationType verifyType, string locationId, string carrierId, string slotMap, string[] lotIds, string[] substrateIds, string usage)
        {
            VerifyType = verifyType;
            LocationId = locationId;
            CarrierId = carrierId;
            SlotMap = slotMap;
            LotIds = lotIds ?? Array.Empty<string>();
            SubstrateIds = substrateIds ?? Array.Empty<string>();
            Usage = usage;
        }
        public VerificationType VerifyType { get; }
        public string LocationId { get; }
        public string CarrierId { get; }
        public string SlotMap { get; }
        public string[] LotIds { get; }
        public string[] SubstrateIds { get; }
        public string Usage { get; }
    }

    public sealed class CarrierVerificationFailedEventArgs : EventArgs
    {
        public CarrierVerificationFailedEventArgs(VerificationType verifyType, string locationId, string carrierId, long failReason) 
        {
            VerifyType = verifyType; 
            LocationId = locationId; 
            CarrierId = carrierId; 
            FailReason = failReason; 
        }
        public VerificationType VerifyType { get; }
        public string LocationId { get; }
        public string CarrierId { get; }
        public long FailReason { get; }
    }
    public sealed class CarrierVerificationResultWithoutRemoteArgs : EventArgs
    {
        public CarrierVerificationResultWithoutRemoteArgs(
            string locationId,
            string carrierId,
            Dictionary<int, string> lotIds,
            Dictionary<int, string> substrateIds,
            string usage,
            VerificationResult result)
        {
            LocationId = locationId;
            CarrierId = carrierId;
            LotIds = new Dictionary<int, string>(lotIds);
            SubstrateIds = new Dictionary<int, string>(substrateIds);
            Usage = usage;
            Result = result;
        }
        public string LocationId { get; }
        public string CarrierId { get; }
        public string SlotMap { get; }
        public Dictionary<int, string> LotIds { get; }
        public Dictionary<int, string> SubstrateIds { get; }
        public string Usage { get; }
        public VerificationResult Result { get; }
    }
    public sealed class HostCarrierRequestEventArgs : EventArgs
    {
        public HostCarrierRequestEventArgs(long messageId, string locationId, string carrierId) 
        {
            MessageId = messageId; 
            LocationId = locationId; 
            CarrierId = carrierId; 
        }
        public long MessageId { get; }
        public string LocationId { get; }
        public string CarrierId { get; }
    }

    public sealed class HostChangeAccessRequestEventArgs : EventArgs
    {
        public HostChangeAccessRequestEventArgs(long messageId, long mode, string[] locationIds) { MessageId = messageId; Mode = mode; LocationIds = locationIds ?? Array.Empty<string>(); }
        public long MessageId { get; }
        public long Mode { get; }
        public string[] LocationIds { get; }
    }

    public sealed class HostChangeServiceStatusRequestEventArgs : EventArgs
    {
        public HostChangeServiceStatusRequestEventArgs(long messageId, string locationId, long state) { MessageId = messageId; LocationId = locationId; State = state; }
        public long MessageId { get; }
        public string LocationId { get; }
        public long State { get; }
    }
}
