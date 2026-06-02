
using System;
using System.Collections.Generic;

using FrameOfSystem3.SECSGEM.SecsGemSDK.Gem300;

using EFEM.Defines.Common;
using EFEM.Defines.Job;
using EFEM.Defines.LoadPort;
using EFEM.Defines.MaterialTracking;
using EFEM.Defines.CarrierManagement;

namespace FrameOfSystem3.SECSGEM.DefineSecsGem
{
    public enum VerificationResult
    {
        Suceeded = 0,
        Failed,
    }
    public enum VerificationType
    {
        Id = 0,
        Slot = 1
    }
    public interface ICarrierManagementDriver
    {
        event EventHandler<CarrierPortCarrierEventArgs> CarrierInStarted;
        event EventHandler<CarrierDeletedEventArgs> CarrierDeleted;
        event EventHandler<LoadPortStateChangedEventArgs> CarrierTransferStateChanged;
        event EventHandler<LoadPortStateChangedEventArgs> CarrierAccessModeChanged;
        event EventHandler<CarrierVerificationSucceededEventArgs> CarrierVerificationSucceeded;
        event EventHandler<CarrierVerificationResultWithoutRemoteArgs> CarrierVerificationResultWithoutRemote;
        event EventHandler<CarrierVerificationFailedEventArgs> CarrierVerificationFailed;
        event EventHandler<HostCarrierRequestEventArgs> CarrierInRequestedByHost;
        event EventHandler<HostCarrierRequestEventArgs> CarrierOutRequestedByHost;
        event EventHandler<HostCarrierRequestEventArgs> CarrierCancelRequestedByHost;
        event EventHandler<HostChangeAccessRequestEventArgs> AccessChangeRequestedByHost;
        event EventHandler<HostChangeServiceStatusRequestEventArgs> ServiceStatusChangeRequestedByHost;

        long NotifyCarrierDetection(string locationId, string carrierId, CarrierIdVerificationStates idVerificationResult, bool detectionStatus);
        long ReqBind(string locationId, string carrierId, string slotMap);
        long ReqCancelBind(string locationId, string carrierId);
        //long CmsReqCarrierIn(string locationId, string carrierId);
        //long CmsReqCarrierOut(string locationId, string carrierId);
        long ReqCarrierReCreate(string locationId, string carrierId);
        long ReqCancelCarrier(string locationId, string carrierId);
        long ReqProceedCarrier(
            string locationId,
            string carrierId,
            IReadOnlyDictionary<int, CarrierSlotMapStates> map,
            IReadOnlyDictionary<int, string> lots,
            IReadOnlyDictionary<int, string> substrateNames,
            string usage);

        long RspCarrierIn(long messageId, string locationId, string carrierId, long result, long[] errorCodes, string[] errorTexts);
        long RspCarrierOut(long messageId, string locationId, string carrierId, long result, long[] errorCodes, string[] errorTexts);
        long RspCancelCarrier(long messageId, string locationId, string carrierId, long result, long[] errorCodes, string[] errorTexts);
        long RspCarrierRelease(long messageId, string locationId, string carrierId, long result, long[] errorCodes, string[] errorTexts);
        long RspChangeAccess(long messageId, long mode, long result, string[] locationIds, long[] errorCodes, string[] errorTexts);
        long RspChangeServiceStatus(long messageId, string locationId, long state, long result, long[] errorCodes, string[] errorTexts);

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
        //long CmsSetTransferReady(string locationId, long state);
    }

    public interface IProcessJobDriver
    {
        event EventHandler<ProcessJobCreatedEventArgs> ProcessJobCreated;
        event EventHandler<ProcessJobStateChangedEventArgs> ProcessJobStateChanged;
        event EventHandler<ProcessJobDeletedEventArgs> ProcessJobDeleted;
        event EventHandler<ProcessJobVerifyRequestedEventArgs> ProcessJobVerifyRequestedByHost;
        event EventHandler<ProcessJobCommandRequestedEventArgs> ProcessJobCommandRequestedByHost;
        event EventHandler<ProcessJobRecipeVariableRequestedEventArgs> ProcessJobRecipeVariablesRequestedByHost;
        event EventHandler<ProcessJobStartMethodRequestedEventArgs> ProcessJobStartMethodRequestedByHost;
        event EventHandler<ProcessJobMaterialOrderRequestedEventArgs> ProcessJobMaterialOrderRequestedByHost;
        event EventHandler<ProcessJobManualStartEventArgs> ProcessJobManualStartRequired;
        event EventHandler<ProcessJobSettingUpEventArgs> ProcessJobSettingUpRequested;

        long Create(
            string processJobId,
            EFEM.Defines.Common.MaterialFormat materialFormat,
            ProcessStartMode startMode,
            MaterialOrderMode materialOrder,
            IReadOnlyDictionary<string, IReadOnlyList<int>> materialInfo,
            RecipeMethod recipeMethod,
            string recipeId,
            string[] recipeParameterNames,
            string[] recipeParameterValues);

        long CreateWithNumericRecipe(
            string processJobId,
            EFEM.Defines.Common.MaterialFormat materialFormat,
            ProcessStartMode startMode,
            MaterialOrderMode materialOrder,
            IReadOnlyDictionary<string, IReadOnlyList<int>> materialInfo,
            RecipeMethod recipeMethod,
            string recipeId,
            string[] recipeParameterNames,
            long[] recipeParameterValues);

        long RequestJob(string processJobId);
        long RequestAllJobIds();
        long RequestCommand(string processJobId, ProcessJobCommand command);

        long AcknowledgeVerify(long messageId, string[] processJobIds, long result, long[] errorCodes, string[] errorTexts);
        long AcknowledgeCommand(long messageId, ProcessJobCommand command, string processJobId, long result, long[] errorCodes, string[] errorTexts);
        long AcknowledgeRecipeVariables(long messageId, string processJobId, long result, long[] errorCodes, string[] errorTexts);
        long AcknowledgeStartMethod(long messageId, string[] processJobIds, long result, long[] errorCodes, string[] errorTexts);
        long AcknowledgeMaterialOrder(long messageId, long result);

        long SetJobInfo(
            string processJobId,
            EFEM.Defines.Common.MaterialFormat materialFormat,
            ProcessStartMode startMode,
            MaterialOrderMode materialOrder,
            IReadOnlyDictionary<string, IReadOnlyList<int>> materialInfo,
            RecipeMethod recipeMethod,
            string recipeId,
            string[] recipeParameterNames,
            string[] recipeParameterValues);

        long SetJobInfoWithNumericRecipe(
            string processJobId,
            EFEM.Defines.Common.MaterialFormat materialFormat,
            ProcessStartMode startMode,
            MaterialOrderMode materialOrder,
            IReadOnlyDictionary<string, IReadOnlyList<int>> materialInfo,
            RecipeMethod recipeMethod,
            string recipeId,
            string[] recipeParameterNames,
            long[] recipeParameterValues);

        long SetState(string processJobId, ProcessJobState state);
        long NotifySettingUpStarted(string processJobId);
        long NotifySettingUpCompleted(string processJobId);
        long Remove(string processJobId);
        long RemoveAll();
    }

    public interface IControlJobDriver
    {
        event EventHandler<ControlJobCreatedEventArgs> ControlJobCreated;
        event EventHandler<ControlJobStateChangedEventArgs> ControlJobStateChanged;
        event EventHandler<ControlJobDeletedEventArgs> ControlJobDeleted;
        event EventHandler<ControlJobVerifyRequestedEventArgs> ControlJobVerifyRequestedByHost;
        event EventHandler<ControlJobCommandRequestedEventArgs> ControlJobCommandRequestedByHost;
        event EventHandler<ControlJobManualStartEventArgs> ControlJobManualStartRequired;
        event EventHandler<ControlJobHoqChangedEventArgs> ControlJobHeadOfQueueChanged;

        long Create(string controlJobId, ControlJobStartMode startMode, string[] processJobIds);
        long RequestJob(string controlJobId);
        long RequestAllJobIds();
        long RequestSelect(string controlJobId);
        long RequestHeadOfQueue(string controlJobId);
        long RequestHeadOfQueueInfo();
        long RequestCommand(string controlJobId, ControlJobCommand command, string commandParameterName, string commandParameterValue);

        long AcknowledgeVerify(long messageId, string controlJobId, long result, long[] errorCodes, string[] errorTexts);
        long AcknowledgeCommand(long messageId, string controlJobId, ControlJobCommand command, long result, long[] errorCodes, string[] errorTexts);

        long SetJobInfo(string controlJobId, ControlJobState state, ControlJobStartMode startMode, string[] processJobIds);
        long Remove(string controlJobId);
        long RemoveAll();
    }

    public interface ISubstrateTrackingDriver
    {
        event EventHandler<SubstrateCreatedEventArgs> SubstrateCreated;
        event EventHandler<SubstrateDeletedEventArgs> SubstrateDeleted;
        event EventHandler<SubstrateTransportStateChangedEventArgs> SubstrateTransportChanged;
        event EventHandler<SubstrateProcessingStateChangedEventArgs> SubstrateProcessingChanged;
        event EventHandler<SubstrateReadingStateChangedEventArgs> SubstrateReadingChanged;
        event EventHandler<SubstrateCreateRequestedEventArgs> SubstrateCreateRequestedByHost;
        event EventHandler<SubstrateUpdateRequestedEventArgs> SubstrateUpdateRequestedByHost;
        event EventHandler<SubstrateDeleteRequestedEventArgs> SubstrateDeleteRequestedByHost;
        event EventHandler<SubstrateCancelRequestedEventArgs> SubstrateCancelRequestedByHost;
        event EventHandler<SubstrateConfirmEventArgs> SubstrateConfirmationDisplayed;
        event EventHandler<SubstrateConfirmEventArgs> SubstrateConfirmationSucceeded;
        event EventHandler<SubstrateConfirmFailedEventArgs> SubstrateConfirmationFailed;

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

    /// <summary>
    /// GEM 공통 드라이버(SecsGem) 위에 GEM300 capability를 조합하는 추상 베이스.
    /// 공개 서비스(Carrier/ProcessJob/ControlJob/Substrate)는
    /// 아래 capability driver를 통해 동작한다.
    /// </summary>
    public abstract class SecsGem300 : SecsGem
    {
        internal abstract ICarrierManagementDriver CmsDriver { get; }
        internal abstract IProcessJobDriver PjDriver { get; }
        internal abstract IControlJobDriver CjDriver { get; }
        internal abstract ISubstrateTrackingDriver StsDriver { get; }
    }
}