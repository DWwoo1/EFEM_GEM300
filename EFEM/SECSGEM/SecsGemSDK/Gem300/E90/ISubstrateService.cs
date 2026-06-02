using System;

using EFEM.Defines.Common;

using FrameOfSystem3.SECSGEM.DefineSecsGem;

namespace EFEM.Defines.MaterialTracking
{
    public interface ISubstrateService
    {
        bool IsDriverAttached { get; }
        void AttachDriver(ISubstrateTrackingDriver driver);
        void DetachDriver();
        void RegisterCallback(ISubstrateServiceCallback callback);
        void UnregisterCallback(ISubstrateServiceCallback callback);
        long InitializeLocation(string locationId, string substrateId);
        long InitializeBatchLocation(string batchLocationId, string substrateId);
        long SetTransport(string locationId, string substrateId, TransportStates transportState);
        long SetBatchTransport(string[] locationIds, string[] substrateIds, TransportStates transportState);
        long SetProcessing(string locationId, string substrateId, ProcessingStates processingState);
        long SetBatchProcessing(string[] locationIds, string[] substrateIds, ProcessingStates processingState);
        long SetInfo(string locationId, string substrateId, TransportStates transportState, ProcessingStates processingState, IdReadingStates readingState);
        long SetReadResult(string locationId, string substrateId, string readSubstrateId, long result);
        long NotifyMaterialArrived(string materialId);
        long Create(string locationId, string substrateId);
        long Cancel(string locationId, string substrateId);
        long Proceed(string locationId, string substrateId, string readSubstrateId);
        long Delete(string locationId, string substrateId);
        long AcknowledgeCreate(long messageId, string locationId, string substrateId, long result, long[] errorCodes, string[] errorTexts);
        long AcknowledgeCancel(long messageId, string locationId, string substrateId, long result, long[] errorCodes, string[] errorTexts);
        long AcknowledgeUpdate(long messageId, string locationId, string substrateId, long result, long[] errorCodes, string[] errorTexts);
        long AcknowledgeDelete(long messageId, string locationId, string substrateId, long result);
        long Remove(string substrateId);
        long RemoveAll();
    }
    
    public sealed class SubstrateCreatedEventArgs : EventArgs
    {
        public SubstrateCreatedEventArgs(string locationId, string substrateId, MaterialFormat substrateType, TransportStates transportState, ProcessingStates processingState) { LocationId = locationId; SubstrateId = substrateId; SubstrateType = substrateType; TransportState = transportState; ProcessingState = processingState; }
        public string LocationId { get; }
        public string SubstrateId { get; }
        public MaterialFormat SubstrateType { get; }
        public TransportStates TransportState { get; }
        public ProcessingStates ProcessingState { get; }
    }

    public sealed class SubstrateDeletedEventArgs : EventArgs
    {
        public SubstrateDeletedEventArgs(string substrateId) { SubstrateId = substrateId; }
        public string SubstrateId { get; }
    }

    public sealed class SubstrateTransportStateChangedEventArgs : EventArgs
    {
        public SubstrateTransportStateChangedEventArgs(string locationId, string substrateId, TransportStates state)
        {
            LocationId = locationId;
            SubstrateId = substrateId;
            State = state;
        }
        public string LocationId { get; }
        public string SubstrateId { get; }
        public TransportStates State { get; }
    }

    public sealed class SubstrateProcessingStateChangedEventArgs : EventArgs
    {
        public SubstrateProcessingStateChangedEventArgs(string locationId, string substrateId, ProcessingStates state) 
        {
            LocationId = locationId; 
            SubstrateId = substrateId; 
            State = state; 
        }
        public string LocationId { get; }
        public string SubstrateId { get; }
        public ProcessingStates State { get; }
    }
    public sealed class SubstrateReadingStateChangedEventArgs : EventArgs
    {
        public SubstrateReadingStateChangedEventArgs(string locationId, string substrateId, IdReadingStates state)
        {
            LocationId = locationId;
            SubstrateId = substrateId;
            State = state;
        }
        public string LocationId { get; }
        public string SubstrateId { get; }
        public IdReadingStates State { get; }
    }
    public sealed class SubstrateCreateRequestedEventArgs : EventArgs
    {
        public SubstrateCreateRequestedEventArgs(long messageId, string locationId, string substrateId) { MessageId = messageId; LocationId = locationId; SubstrateId = substrateId; }
        public long MessageId { get; }
        public string LocationId { get; }
        public string SubstrateId { get; }
    }

    public sealed class SubstrateUpdateRequestedEventArgs : EventArgs
    {
        public SubstrateUpdateRequestedEventArgs(long messageId, string locationId, string substrateId, MaterialFormat substrateType, TransportStates transportState, ProcessingStates processingState)
        {
            MessageId = messageId;
            LocationId = locationId;
            SubstrateId = substrateId;
            SubstrateType = substrateType;
            TransportState = transportState;
            ProcessingState = processingState;
        }
        public long MessageId { get; }
        public string LocationId { get; }
        public string SubstrateId { get; }
        public MaterialFormat SubstrateType { get; }
        public TransportStates TransportState { get; }
        public ProcessingStates ProcessingState { get; }
    }

    public sealed class SubstrateDeleteRequestedEventArgs : EventArgs
    {
        public SubstrateDeleteRequestedEventArgs(long messageId, string locationId, string substrateId) { MessageId = messageId; LocationId = locationId; SubstrateId = substrateId; }
        public long MessageId { get; }
        public string LocationId { get; }
        public string SubstrateId { get; }
    }

    public sealed class SubstrateCancelRequestedEventArgs : EventArgs
    {
        public SubstrateCancelRequestedEventArgs(long messageId, string locationId, string substrateId) { MessageId = messageId; LocationId = locationId; SubstrateId = substrateId; }
        public long MessageId { get; }
        public string LocationId { get; }
        public string SubstrateId { get; }
    }

    public sealed class SubstrateConfirmEventArgs : EventArgs
    {
        public SubstrateConfirmEventArgs(string locationId, string substrateId, string readSubstrateId) { LocationId = locationId; SubstrateId = substrateId; ReadSubstrateId = readSubstrateId; }
        public string LocationId { get; }
        public string SubstrateId { get; }
        public string ReadSubstrateId { get; }
    }

    public sealed class SubstrateConfirmFailedEventArgs : EventArgs
    {
        public SubstrateConfirmFailedEventArgs(string locationId, string substrateId) { LocationId = locationId; SubstrateId = substrateId; }
        public string LocationId { get; }
        public string SubstrateId { get; }
    }
}
