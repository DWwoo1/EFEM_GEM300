using System;
using System.Collections.Generic;
using System.Collections.Concurrent;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


using TickCounter_;

using FrameOfSystem3.Recipe;
using FrameOfSystem3.SECSGEM.DefineSecsGem;

using EFEM.Defines.Common;
using EFEM.MaterialTracking;
using EFEM.Defines.LoadPort;
using EFEM.Modules;
using EFEM.CustomizedByProcessType.PWA500BIN;

namespace EFEM.CustomizedByProcessType.PWA500Common
{
    #region <Constants>
    public static class PWA500CarrierAttributes
    {
        public const string KeyPartId = "PartId";
        public const string KeyStepSeq = "StepSeq";
        public const string KeyLotType = "LotType";
        public const string KeyLotQty = "LotQty";
        public const string KeyProcessStepBeforeSendingCarrier = "ProcessStepBeforeSendingCarrier";
        public const string KeyTrackInCompleted = "TrackInCompleted";
        public const string KeyDownloadingRecipeCompleted = "DownloadingRecipeCompleted";
    }
    public static class PWA500SubstrateAttributes
    {       
        public const string SubstrateSize = "SubstrateSize";
        public const string SubstrateType = "SubstrateType";
        public const string RingId = "RingId";
        public const string PartId = "PartId";
        public const string LotType = "LotType";
        public const string StepSeq = "StepSeq";
        public const string ChipQty = "ChipQty";
        public const string BinCode = "BinCode";
        public const string RefPositionX = "RefPositionX";
        public const string RefPositionY = "RefPositionY";
        public const string StartingPositionX = "StartingPositionX";
        public const string StartingPositionY = "StartingPositionY";
        public const string CountX = "CountX";
        public const string CountY = "CountY";
        public const string Angle = "Angle";
        public const string MapData = "MapData";
        public const string ParentLotId = "ParentLotId";
        public const string SplittedLotId = "SplittedLotId";
        public const string IsLastSubstrate = "IsLastSubstrate";
        public const string IsTrackOutCompleted = "IsTrackOutCompleted";
        public const string BinUnloadingStep = "BinUnloadingStep";
        public const string CoreLotId = "CoreLotId";
        public const string CorePartId = "CorePartId";
        public const string SplittedHistory = "SplittedHistory";
    }
    public static class PWA500MaterialHandling
    {
        public const string SubstrateName = "SubstrateName";
        public const string SubstrateType = "SubstrateType";
        public const string LotId = "LotId";
        public const string RecipeId = "RecipeId";
        public const string PortId = "PortId";
        public const string SlotId = "SlotId";
        public const string HandlingResult = "HandlingResult";
        public const string HandlingResultOk = "Ok";
        public const string HandlingResultNg = "Ng";
        public const string RingId = "RingId";
    }
    #endregion </Constants>

    #region <Enumerations>
    public enum SubstrateType
    {
        Core,
        Empty,
        Bin1,
        Bin2,
        Bin3,
    }
    public enum SubstrateSize
    {
        Inch_8,            // 8 inch
        Inch_12,            // 12 inch
    }
    public enum RequestMessages
    {
        RequestApproachLoading,
        RequestActionLoading,
        RequestConfirmLoading,

        RequestApproachUnloading,
        RequestActionUnloading,
        RequestConfirmUnloading,

        RequestStartUnloading,

        RequestLoadingSmemaOnAtSimul,
        RequestLoadingSmemaOffAtSimul,
        RequestUnloadingSmemaOnAtSimul,
        RequestUnloadingSmemaOffAtSimul,
    }
    public enum ResponseMessages
    {
        ResponseApproachLoading,
        ResponseActionLoading,
        ResponseConfirmLoading,

        ResponseApproachUnloading,
        ResponseActionUnloading,
        ResponseConfirmUnloading,

        ResponseStartUnloading,
    }
    public enum StepsBeforeSendingCarrier
    {
        Init = 0,
        MergeAndChangeCompleted,
        SlotMappingCompleted,
        WriteTag,
    }
    public enum CheckingCarrierCodeToUnload
    {
        Ok = 0,
        Skip,
        InvalidPortInfo,
        PortNotEnabled,
        DoesNotHaveToAccessCarrier,
        DoorIsNotOpened,
        SlotsIsFull,
    }
    #endregion </Enumerations>

    #region <For SECS/GEM>
    public enum RemoteCommandTypes
    {
        STOP = 0,
        NEXT_WORK_REQ,
        STOP_WORK_REQ,
    }
    public enum ObjectNames
    {
        LOTINFO,
        CHANGELOTINFO,
        ASSIGN_WAFER_LOT_ID,
        CORE_WAFER_ID_RESULT,
        ASSIGN_SPLIT_LOT_ID,
        ASSIGN_WAFER_ID,
        LOT_MERGE,
        LOT_ID_CHANGE,
        ProceedWithCarrier,
        BIN_PART_ID_INFO,
    }
    public enum AttributeNames
    {
        LOTID,
        SPLIT_LOTID,
    }
    public enum OHTHandlingStatus
    {
        LOAD,
        UNLOAD
    }
    public enum OHTHandlingCarrierType
    {
        MAC,
        CASSETTE
    }
    public enum CarrierLotIdType
    {
        // 빈용기 투입 요청 Lot Id
        PEMAC,
        ECASSETTE,

        // Terminated Wafer 포함된 Carrier Id(배출 시 Tag에 쓸 이름)        
        // 2025.07.25. jhlim [DEL] 해당 명칭은 설정 가능하도록 변경되었으므로, 아래 열거형은 제거.. -> 투입요청용 이름도 변경해야하나??
        // 2025/07/25 기준 MAC:PRMAC, CASSETTE:RCASSETTE 사용중임
        // 2024/11/27에 MAC:RCMAC -> PRMAC으로 변경됨
        //PRMAC,
        //RCASSETTE,

        // Core or Empty 요청 -> 공란
        // 완성된 Bin Carrier -> LotId
        // 2025.07.25. jhlim [END]
    }
    public enum ProceModuleHeadNum
    {
        Right = 0,
        Left
    }
    public enum MessagesToReceive
    {
        #region <Request>
        RequestUpdateEquipmentData,
        RequestUpdateTraceData,
        RequestUpdateEquipmentState,


        RequestNotifyAlarmStatus,
        RequestAssignRingId,
        RequestDownloadMapFile,
        RequestAssignCoreRingId,
        RequestStartDetaching,
        RequestFinishDetaching,
        RequestStartSorting,
        RequestFinishSorting,
        RequestSplitCoreChip,
        RequestUploadCoreFile,
        RequestUploadScrapInfo,

        RequestUploadRecipe,
        RequestUploadRecipeResult,

        // 2025.07.11. jhlim [ADD] 가치효율 이벤트 추가 요청
        RequestMoveMaterial,            // 자재 이송 시작 전 발생
        RequestMaterialMoved,           // 자재 이송 완료 후 발생
        RequestFinishPicking,           // 픽업 완료 시 발생
        RequestFinishPlacing,           // 본딩 완료 시 발생
        // 2025.07.11. jhlim [END]
        #endregion </Request>

        #region <Response>
        ResponseDownloadRecipe,
        ResponseUploadRecipe,
        ResponseDeleteRecipe,
        ResponseAssignSubstrateId,
        ResponseAssignLotId,
        ResponseUploadBinFile,
        ResponseAssignCoreSubstrateId,
        #endregion </Response>
    }

    public enum MessagesToSend
    {
        #region <Request>
        RequestStop,
        RequestCallOperator,
        RequestDownloadRecipe,
        RequestUploadRecipe,
        RequestDeleteRecipe,
        RequestAssignSubstrateId,
        RequestAssignLotId,
        RequestUploadBinFile,
        RequestAssignCoreSubstrateId,
        #endregion </Request>

        #region <Response>
        ResponseAssignRingId,
        ResponseAssignCoreRingId,
        ResponseDownloadMapFile,
        ResponseStartDetaching,
        ResponseFinishDetaching,
        ResponseStartSorting,
        ResponseFinishSorting,
        ResponseSplitCoreChip,
        ResponseUploadCoreFile,
        ResponseUploadScrapInfo,
        #endregion </Response>
    }
    public static class ErrorDescriptionsForMaterialHanding
    {
        public const string ErrorDescriptionForControlJobIsNotExecuted = "ControlJob is not executed";
        public const string ErrorDescriptionForInvalidSubstratePortInfo = "Invalid Substrate Port Info";
        public const string ErrorDescriptionForDoesntHaveCarrier = "Does not have carrier at loadport";
        public const string ErrorDescriptionForLoadPortNotEnabled = "Loadport is not enabled";
        public const string ErrorDescriptionForDoorIsNotOpened = "Loadport door is not opened";
        public const string ErrorDescriptionForSlotIsFull = "All of the slot is full";

        public const string ErrorDescriptionForAssignSubstrateId = "Cannot getting a assigned substrate Id";
        public const string ErrorDescriptionForRequestPartId = "Cannot getting a assigned part Id";
    }
    public static class CarrierLoadUnloadKeys
    {
        public static readonly string KeyParamCarrierId = "CARRIERID";
        public static readonly string KeyParamLotId = "LOTID";
        public static readonly string KeyParamPartId = "PARTID";
        public static readonly string KeyParamStepId = "STEPSEQ";
        public static readonly string KeyParamLotType = "LOTTYPE";
    }
    public static class AdditionalParamKeys
    {
        public static readonly string KeyNameOfEq = "NameOfEq";
        public static readonly string KeySubstrateId = "SubstrateId";
        public static readonly string KeyLotId = "LotId";
        public static readonly string KeySlotId = "SlotId";
        public static readonly string KeySubstrateKey = "SubstrateKey";
        public static readonly string KeyRingId = "RingId";
        public static readonly string KeyUserId = "UserId";
        public static readonly string KeyMessageNameToSend = "ScenarioNameToSend";
        public static readonly string KeySubstrateType = "SubstrateType";
        public static readonly string KeyChipQty = "ChipQty";
    }
    public static class ResultKeys
    {
        public static readonly string KeyResult = "Result";
        public static readonly string KeyDescription = "Description";
    }
    public static class NotifyAlarmKeys
    {
        public static readonly int BaseAlarmIndexOffset = 2000000;
        public static readonly string KeyAlarmId = "AlarmId";
        public static readonly string KeyStatus = "Status";
    }
    public static class MachineInfoKeys
    {
        public static readonly string KeyLotId = "LotId";
        public static readonly string KeyRecipeId = "RecipeId";
        public static readonly string KeyEquipmentState = "EquipmentState";
    }
    public static class EESKeys
    {
        public static readonly string KeyCarrierId = "CARRIERID";
        public static readonly string KeyPortId = "PORTID";
        public static readonly string KeyLotId = "LOTID";
        public static readonly string KeyPartId = "PARTID";
        public static readonly string KeyParamRecipeId = "RECIPEID";
        public static readonly string KeyOperatorId = "OPERID";

        public static readonly string KeyParamBinType = "BIN_TYPE";
        public static readonly string KeyParamWorkIndexX = "WORKING_INDEX_X";
        public static readonly string KeyParamWorkIndexY = "WORKING_INDEX_Y";
        public static readonly string KeyParamWorkingPickerNum = "WORKING_PICKER_NUM";
        public static readonly string KeyParamESDSensor04 = "ESD_SENSOR_04";

        // 자재 이송 관련 가치효율들
        // 자재 이송
        public const string KeyRingId = "RingId";
        public const string KeyLocation = "Location";

        // PNP
        public const string KeySubstrateName = "SubstrateName";
        public const string KeyBinCode = "BinCode";
        public const string KeyIndexX = "IndexX";
        public const string KeyIndexY = "IndexY";
        public const string KeyHead = "PickerIndex";
    }
    public static class RFIDReadKeys
    {
        public static readonly string KeyParamLotId = "LOTID";
        public static readonly string KeyParamCarrierId = "CARRIERID";
        public static readonly string KeyParamPortId = "PORTID";
        public static readonly string KeyParamOperatorId = "OPERID";
    }
    public static class LotInfoKeys
    {
        public static readonly string KeyParamLotId = "LOTID";
        public static readonly string KeyParamCarrierId = "CARRIERID";

        public static readonly string KeyResultLotId = "LotId";
        public static readonly string KeyResultPartId = "PartId";
        public static readonly string KeyResultStepSeq = "StepSeq";
        public static readonly string KeyResultLotType = "LotType";
        public static readonly string KeyResultLotQty = "LotQty";
        public static readonly string KeyResultRecipeId = "RecipeId";
    }
    public static class SlotMapVefiricationKeys
    {
        public static readonly string KeyResultLotId = "LotId";
        public static readonly string KeyResultName = "Name";
        public static readonly string KeyResultStatus = "Status";
        public static readonly string KeyIsCancelCarrier = "IsCancelCarrier";
    }
    public static class RecipeHandlingKeys
    {
        public static readonly string KeyRecipeId = "RecipeId";
        public static readonly string KeyRecipeBody = "RecipeBody";
        public static readonly string KeyUseCommunicationToPM = "UseCommunicationToPM";

        public static readonly string KeyParamLotId = "LOTID";
        public static readonly string KeyParamRecipeId = "RECIPEID";
        public static readonly string KeyParamPartId = "PARTID";
        public static readonly string KeyParamStepSeq = "STEPSEQ";
        public static readonly string KeyParamLotType = "LOTTYPE";
    }
    public static class AssignRingIdKeys
    {
        public static readonly string KeyOldRingId = "OldRingId";
        public static readonly string KeyNewRingId = "NewRingId";

        public static readonly string KeyParamLotId = "LOTID";
        public static readonly string KeyParamWaferId = "WAFERID";

        public static readonly string KeyParamRingFrameId = "RINGFRAME_ID";
    }
    public static class AssignSubstrateLotIdKeys
    {
        public static readonly string KeySubstrateName = "SubstrateName";
        public static readonly string KeyLotId = "LotId";

        public static readonly string KeyParamLotId = "LOTID";
        public static readonly string KeyParamWaferId = "WAFERID";
        public static readonly string KeyParamPartId = "PARTID";
        public static readonly string KeyParamRecipeId = "RECIPEID";
        public static readonly string KeyParamSlotId = "SLOTID";
        public static readonly string KeyParamOperatorId = "OPERID";

        // 시나리오 결과용
        public static readonly string KeyResultLotId = "LotId";
        public static readonly string KeyResultPartId = "PartId";
        public static readonly string KeyResultSubstrateId = "SubstrateId";
        public static readonly string KeyResultQty = "Qty";
    }
    public static class AssignSubstrateIdKeys
    {
        public static readonly string KeySubstrateName = "SubstrateName";
        public static readonly string KeyRingId = "RingId";
        public static readonly string KeyRecipeId = "RecipeId";
        public static readonly string KeySubstrateType = "SubstrateType";
        public static readonly string KeyChipQty = "ChipQty";

        // Param 전송용
        public static readonly string KeyParamLotId = "LOTID";
        public static readonly string KeyParamBinType = "BIN_TYPE";
        public static readonly string KeyParamRingFrameId = "RINGFRAME_ID";
        public static readonly string KeyParamSlotId = "SLOTID";
        public static readonly string KeyParamChipQty = "CHIP_QTY";

        // 시나리오 결과용
        public static readonly string KeyResultSubstrateId = "SubstrateId";
        public static readonly string KeyResultRingId = "RingId";
    }
    public static class AssignBinLotIdKeys
    {
        public static readonly string KeySubstarateName = "SubstrateName";
        public static readonly string KeyLotId = "LotId";
    }
    public static class SortingKeys
    {
        public static readonly string KeyRingId = "RingId";
        public static readonly string KeyRecipeId = "RecipeId";
        public static readonly string KeySubstrateType = "SubstrateType";
        public static readonly string KeyBinCode = "BinCode";
        public static readonly string KeyChipQty = "ChipQty";

        // Param용
        public static readonly string KeyParamLotId = "LOTID";
        public static readonly string KeyParamCarrierId = "CARRIERID";
        public static readonly string KeyParamBinType = "BIN_TYPE";
        public static readonly string KeyParamRingFrameId = "RINGFRAME_ID";
        public static readonly string KeyParamChipQty = "CHIP_QTY";
        public static readonly string KeyParamParentLotId = "MATERIAL_LOT_ID_TO_COMSUME";

        // 2025.05.08. jhlim [ADD] 고객사 요청으로 가치효율용 VID 추가
        public static readonly string KeyParamCorePartId = "CORE_LOTID";
        public static readonly string KeyParamCoreLotId = "PARTID";
        // 2025.05.08. jhlim [END]
    }
    public static class DetachingKeys
    {
        public static readonly string KeySubstarateName = "SubstrateName";
        public static readonly string KeyRingId = "RingId";
        public static readonly string KeyRecipeId = "RecipeId";
        public static readonly string KeyUserId = "UserId";
        public static readonly string KeySubstrateType = "SubstrateType";


        // Param용
        public static readonly string KeyParamCarrierId = "CARRIERID";
        public static readonly string KeyParamPortId = "PORTID";
        public static readonly string KeyParamLotId = "LOTID";
        public static readonly string KeyParamPartId = "PARTID";
        public static readonly string KeyParamRecipeId = "RECIPEID";
        public static readonly string KeyParamWaferId = "WAFERID";
        public static readonly string KeyParamSlotId = "SLOTID";
        public static readonly string KeyParamOperatorId = "OPERID";
    }
    public static class SplitCoreChipKeys
    {
        public static readonly string KeyCoreSubstrateName = "CoreSubstrateName";
        public static readonly string KeyBinRingId = "BinRingId";

        public static readonly string KeySubstrateType = "SubstrateType";
        public static readonly string KeyRecipeId = "RecipeId";
        public static readonly string KeySplitQty = "SplitQty";
        public static readonly string KeyRemainingChips = "RemainingChips";
        public static readonly string KeyIsFirstSorting = "IsFirstSorting";
        public static readonly string KeyUserId = "UserId";
        public static readonly string KeyBinCode = "BinCode";

        // Param 전송용
        public static readonly string KeyParamLotId = "LOTID";
        public static readonly string KeyParamSplitLotId = "SPLIT_LOTID";
        public static readonly string KeyParamSplitWaferId = "SPLIT_WAFERID";
        public static readonly string KeyParamRingFrameId = "RINGFRAME_ID";
        public static readonly string KeyParamBinType = "BIN_TYPE";
        public static readonly string KeyParamSplitChipQty = "CHIP_QTY";

        // 시나리오 결과용
        public static readonly string KeyResultLotId = "LotId";
        public static readonly string KeyResultSplittedLotId = "SplittedLotId";
        public static readonly string KeyResultQty = "Qty";
    }
    public static class RequestDownloadMapFileKeys
    {
        public static readonly string KeySubstrateName = "SubstrateName";
        public static readonly string KeyRingId = "RingId";
        public static readonly string KeyWaferAngle = "WaferAngle";
        public static readonly string KeyUserId = "UserId";

        public static readonly string KeyCountRow = "CountRow";
        public static readonly string KeyCountCol = "CountCol";
        public static readonly string KeyChipQty = "ChipQty";
        public static readonly string KeyMapData = "MapData";
        public static readonly string KeyNullBinCode = "NullBinCode";
        public static readonly string KeyUseEventHandling = "UseEventHandling";

        // Param 전송용
        public static readonly string KeyParamCarrierId = "CARRIERID";
        public static readonly string KeyParamPortId = "PORTID";
        public static readonly string KeyParamLotId = "LOTID";
        public static readonly string KeyParamPartId = "PARTID";
        public static readonly string KeyParamRecipeId = "RECIPEID";
        public static readonly string KeyParamOperatorId = "OPERID";
        public static readonly string KeyParamWaferId = "WAFERID";
        public static readonly string KeyParamAngle = "ANGLE";
        public static readonly string KeyParamMapData = "MAPDATA";

        // 시나리오 결과용
        public static readonly string KeyResultSubstrateId = "SubstrateId";
        public static readonly string KeyResultCountRow = "CountRow";
        public static readonly string KeyResultCountCol = "CountCol";
        public static readonly string KeyResultReferenceX = "ReferenceX";
        public static readonly string KeyResultReferenceY = "ReferenceY";
        public static readonly string KeyResultStartingX = "StartingX";
        public static readonly string KeyResultStartingY = "StartingY";
        public static readonly string KeyResultAngle = "Angle";
        public static readonly string KeyResultQty = "Qty";
        public static readonly string KeyResultMapData = "MapData";
    }
    public static class TrackInOrOut
    {
        public static readonly string KeyParamCarrierId = "CARRIERID";
        public static readonly string KeyParamPortId = "PORTID";
        public static readonly string KeyParamLotId = "LOTID";
        public static readonly string KeyParamPartId = "PARTID";
        public static readonly string KeyParamStepSeq = "STEPSEQ";
        public static readonly string KeyParamRecipeId = "RECIPEID";
        public static readonly string KeyParamChipQty = "CHIP_QTY";
        public static readonly string KeyParamBinType = "BIN_TYPE";
        public static readonly string KeyParamOperatorId = "OPERID";

        public static readonly string KeyParamChangeReason = "CHANGE_REASON";
        public static readonly string KeyParamMaterialType = "MATERIAL_TYPE";
    }
    public static class UploadCoreOrBinFileKeys
    {
        public static readonly string KeySubstrateName = "SubstrateName";
        public static readonly string KeyRingId = "RingId";
        public static readonly string KeyRecipeId = "RecipeId";
        public static readonly string KeySubstrateType = "SubstrateType";
        public static readonly string KeyPMSBody = "PMSFileBody";
        public static readonly string KeyCountRow = "CountRow";
        public static readonly string KeyCountCol = "CountCol";
        public static readonly string KeyWaferAngle = "WaferAngle";
        public static readonly string KeyMapData = "MapData";
        public static readonly string KeyChipQty = "ChipQty";
        public static readonly string KeyNullBinCode = "NullBinCode";
        public static readonly string KeyUserId = "UserId";
        public static readonly string KeyBinCode = "BinCode";
        public static readonly string KeyUseEventHandling = "UseEventHandling";

        //public static readonly string KeyXMLFileName = "XMLFileName";
        //public static readonly string KeyXMLFileBody = "XMLFileBody";
        public static readonly string KeyPMSFileName = "PMSFileName";
        public static readonly string KeyPMSFileBody = "PMSFileBody";

        public static readonly string KeyReferenceX = "ReferenceX";
        public static readonly string KeyReferenceY = "ReferenceY";
        public static readonly string KeyStartingPosX = "StartingPosX";
        public static readonly string KeyStartingPosY = "StartingPosY";

        public static readonly string KeyStepId = "StepId";
        public static readonly string KeyEquipId = "EquipId";
        public static readonly string KeyPartId = "PartId";
        public static readonly string KeySlot = "Slot";
        public static readonly string KeyLotId = "LotId";

        // Param 전송용
        public static readonly string KeyParamCarrierId = "CARRIERID";
        public static readonly string KeyParamPortId = "PORTID";
        public static readonly string KeyParamLotId = "LOTID";
        public static readonly string KeyParamRecipeId = "RECIPEID";
        public static readonly string KeyParamPartId = "PARTID";
        public static readonly string KeyParamChipQty = "CHIP_QTY";
        public static readonly string KeyParamWaferId = "WAFERID";
        public static readonly string KeyParamSlotId = "SLOTID";
        public static readonly string KeyParamOperatorId = "OPERID";
        public static readonly string KeyParamMapData = "MAPDATA";
    }
    public static class SlotMappingKeys
    {
        public static readonly string KeyParamLotId = "LOTID";
        public static readonly string KeyParamCarrierId = "CARRIERID";

        public static readonly string KeyParamSlotNamePre = "SLOT";
        public static readonly string KeyParamSlotNamePost = "WAFERID";

        public static readonly string KeyParamSlotQtyPre = "SLOT";
        public static readonly string KeyParamSlotQtyPost = "WAFER_CHIP_QTY";
        public static readonly string KeyParamWaferQty = "WAFER_QTY";
    }
    public static class LotMergeKeys
    {
        // PARAM
        public const string KeyParamLotId = "LOTID";
        public const string KeyParamCarrierId = "CARRIERID";
        public const string KeyParamPartId = "PARTID";
        public const string KeyParamRecipeId = "RECIPEID";
        public const string KeyOperatorId = "OPERID";

        public const string KeyParamSlotLotIdPre = "SLOT";
        public const string KeyParamSlotLotIdPost = "WAFER_LOT_ID";

        // Result
        public const string KeyResultLotId = "LotId";
    }
    public static class ChangeToLotIdKeys
    {
        // PARAM
        public static readonly string KeyParamLotId = "LOTID";
        public static readonly string KeyParamCarrierId = "CARRIERID";
    }
    public static class AMHSHandlingKeys
    {
        // PARAM
        public static readonly string KeyParamPortId = "PORTID";
        public static readonly string KeyParamLotId = "LOTID";
        public static readonly string KeyParamCarrierId = "CARRIERID";
        public static readonly string KeyParamCarrierType = "CARRIER_TYPE";
        public static readonly string KeyParamStatus = "STATUS";
        public static readonly string KeyParamOperId = "OPERID";
    }
    public static class ProcessModuleStatusChangedKeys
    {
        public static readonly string KeyParamLotId = "LOTID";
        public static readonly string KeyParamPartId = "PARTID";
        public static readonly string KeyParamStepSeq = "STEPSEQ";
    }
    #endregion </For SECS/GEM>

    public class BinDataToUploadFromPWA500
    {
        #region <Constructors>
        public BinDataToUploadFromPWA500(string nameOfEq, string substrateId, string ringId,
            int chipQty, double angle, int countRow, int countCol, string nullBinCode, string mapData,
            string pmsFileBody, string userId, bool useEventHandling)
        {
            NameOfEq = nameOfEq;
            SubstrateId = substrateId;
            RingId = ringId;
            ChipQty = chipQty;
            Angle = angle;
            CountRow = countRow;
            CountCol = countCol;
            NullBinCode = nullBinCode;
            MapData = mapData;
            PmsFileBody = pmsFileBody;
            UserId = userId;
            UseEventHandling = useEventHandling;
        }
        #endregion </Constructors>

        #region <Properties>
        public string NameOfEq { get; private set; }
        public string SubstrateId { get; private set; }
        public string RingId { get; private set; }
        public int ChipQty { get; private set; }
        public double Angle { get; private set; }
        public int CountRow { get; private set; }
        public int CountCol { get; private set; }
        public string NullBinCode { get; private set; }
        public string MapData { get; private set; }
        public string PmsFileBody { get; private set; }
        public string UserId { get; private set; }
        public bool UseEventHandling { get; private set; }
        #endregion </Properties>

        #region <Methods>
        #endregion </Methods>
    }

    public class LotHistoryLog
    {
        #region <Constructors>
        private LotHistoryLog()
        {
            BasePath = string.Format(@"{0}\History", Define.DefineConstant.FilePath.FILEPATH_LOG);
            CurrentWorkingPath = new Dictionary<int, string>();

            BasePathForSubstrate = string.Format(@"{0}\CurrentWorking", BasePath);
        }
        #endregion </Constructors>

        #region <Types>
        enum CarrierBasedEventType
        {
            IdRead,
            LotInfo,
            ReqSlotMap,
            TrackIn,
            LotMatch,
            TrackOut,
            LotMerge,
            LotMergeAndChange,
            SlotMapping,
        }

        enum SubstrateType
        {
            Core,
            Bin
        }

        enum SubstrateBasedEventType
        {
            WorkStart,
            WorkEnd,
            WaferSplit,
            StartDetaching,
            FinishDetaching,
            ChipSplit,
            ChipSplitAndMerge,
            TrackOut,
            RingIdRead,
            StartSorting,
            FinishSorting,
            IdAssign,
            ReqPartId,
            UploadBinData
        }
        #endregion </Types>

        #region <Fields>
        private const string LogFileExtension = ".log";

        private static LotHistoryLog _instance = null;
        private readonly string BasePath = null;
        private readonly string BasePathForSubstrate = null;
        private readonly Dictionary<int, string> CurrentWorkingPath = null;
        private readonly ConcurrentQueue<Tuple<string, string>> QueueToWrite = new ConcurrentQueue<Tuple<string, string>>();

        private Action<int, string> _logMessageToDisplay = null;
        #endregion </Fields>

        #region <Properties>
        public static LotHistoryLog Instance
        {
            get
            {
                if (_instance == null)
                    _instance = new LotHistoryLog();

                return _instance;
            }
        }
        #endregion </Properties>

        #region <Methods>

        #region <AssignPath>
        public void AddLogInfo(int portId, string name)
        {
            string dir = string.Format(@"{0}\CurrentWorking\{1}", BasePath, name);
            CurrentWorkingPath[portId] = dir;
            if (false == Directory.Exists(dir))
                Directory.CreateDirectory(dir);
        }
        public void AttachDisplayLogAction(Action<int, string> action)
        {
            _logMessageToDisplay = action;
        }
        public string GetBackupHistoryPath(DateTime time, bool isCore)
        {
            if (isCore)
            {
                return string.Format(@"{0}\Backup\{1:0000}\{2:00}\{3:00}\Core", BasePath, time.Year, time.Month, time.Day);
            }
            else
            {
                return string.Format(@"{0}\Backup\{1:0000}\{2:00}\{3:00}\Bin", BasePath, time.Year, time.Month, time.Day);
            }
        }
        public string GetCarrierHistoryPath(int portId, string carrierId)
        {
            if (false == CurrentWorkingPath.TryGetValue(portId, out string basePath))
                return string.Empty;

            return string.Format(@"{0}\{1}{2}", basePath, carrierId, LogFileExtension);
        }
        public string GetSubstratePath(string substrateName, bool isCore)
        {
            SubstrateType substrateType = isCore ? SubstrateType.Core : SubstrateType.Bin;

            return string.Format(@"{0}\{1}\{2}{3}", BasePathForSubstrate, substrateType.ToString(), substrateName, LogFileExtension);
        }
        public void ClearPreviousHistory(int portId, string carrierId, string loadportName)
        {
            if (false == CurrentWorkingPath.TryGetValue(portId, out string basePath))
                return;

            DateTime date = DateTime.Now;
            string backupPath = string.Format(@"{0}\Backup\{1:0000}\{2:00}\{3:00}\NotCompleted\{4}", BasePath, date.Year, date.Month, date.Day, loadportName);
            if (false == Directory.Exists(backupPath))
                Directory.CreateDirectory(backupPath);
            
            string[] files = Directory.GetFiles(basePath);
            string sourceFilePath = string.Format(@"{0}\{1}{2}", basePath, carrierId, LogFileExtension);
            for(int i = 0; files != null && i < files.Length; ++i)
            {
                var file = files[i];
                if (false == file.Equals(sourceFilePath))
                {
                    try
                    {
                        string fileNameToMove = Path.GetFileName(file);
                        string destinationPath = Path.Combine(backupPath, fileNameToMove);
                        if (File.Exists(destinationPath))
                            File.Delete(destinationPath);

                        File.Move(file, destinationPath);
                    }
                    catch
                    {

                    }
                }
            }
            
        }        
        public void UpdateSubstrateHistoryToCarrierHistory(int portId, string carrierId, string substrateName)
        {
            try
            {
                var substrateHistoryFullPath = GetSubstratePath(substrateName, false);
                var substrateHistoryPath = Path.GetDirectoryName(substrateHistoryFullPath);
                if (false == Directory.Exists(substrateHistoryPath) ||
                    false == File.Exists(substrateHistoryFullPath))
                    return;

                var carrierHistoryFullPath = GetCarrierHistoryPath(portId, carrierId);
                var carrierHistoryPath = Path.GetDirectoryName(carrierHistoryFullPath);
                if (false == Directory.Exists(carrierHistoryPath) ||
                    false == File.Exists(carrierHistoryFullPath))
                    return;

                string[] lines;
                using (FileStream fs = new FileStream(substrateHistoryFullPath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
                using (StreamReader sr = new StreamReader(fs))
                {
                    var tempList = new System.Collections.Generic.List<string>();
                    while (false == sr.EndOfStream)
                    {
                        tempList.Add(sr.ReadLine());
                    }
                    lines = tempList.ToArray();
                }

                if (lines == null || lines.Length <= 0)
                    return;

                UpdateRingIdToSubstrateId(substrateName, ref lines);

                using (FileStream fs = new FileStream(carrierHistoryFullPath, FileMode.Append, FileAccess.Write, FileShare.ReadWrite))
                using (StreamWriter sw = new StreamWriter(fs))
                {
                    for (int i = 0; lines != null && i < lines.Length; ++i)
                    {
                        sw.WriteLine(lines[i]);
                    }
                }
            }
            catch
            {

            }
        }
        private void UpdateRingIdToSubstrateId(string substrateName, ref string[] linesToChange)
        {
            for (int i = 0; i < linesToChange.Length; ++i)
            {
                var parts = linesToChange[i].Split(new char[] { '\t' }, StringSplitOptions.None);
                if (parts.Length < 2)
                    continue;

                parts[2] = substrateName;
                linesToChange[i] = string.Join("\t", parts);
            }
        }
        public void BackupCarrierHistory(int portId, string carrierId, string lotId, List<string> substrates, bool isCore)
        {
            if (false == CurrentWorkingPath.TryGetValue(portId, out string basePath))
                return;

            string sourceFilePath = string.Format(@"{0}\{1}{2}", basePath, carrierId, LogFileExtension);
            DateTime date = DateTime.Now;
            string backupPath;
            if (isCore)
            {
                backupPath = string.Format(@"{0}\Backup\{1:0000}\{2:00}\{3:00}\Core\{4}", BasePath, date.Year, date.Month, date.Day, lotId);
            }
            else
            {
                backupPath = string.Format(@"{0}\Backup\{1:0000}\{2:00}\{3:00}\Bin\{4}", BasePath, date.Year, date.Month, date.Day, lotId);
            }
            //var backupPath = string.Format(@"{0}\Backup\{1:0000}\{2:00}\{3:00}\{4}", BasePath, date.Year, date.Month, date.Day, lotId);

            var backupFullPath = string.Format(@"{0}\{1}{2}", backupPath, carrierId, LogFileExtension);

            try
            {
                if (false == Directory.Exists(backupPath))
                    Directory.CreateDirectory(backupPath);

                if (File.Exists(backupFullPath))
                {
                    File.Delete(backupFullPath);
                }

                // Substrate Lists
                if (substrates != null)
                {
                    Dictionary<string, string> filebodies = null;
                    string backupSubstratePath = string.Format(@"{0}\Wafers", backupPath);
                    if (false == isCore)
                    {
                        filebodies = new Dictionary<string, string>();
                    }
                    
                    for (int i = 0; i < substrates.Count; ++i)
                    {
                        SubstrateType type;
                        if (isCore)
                        {
                            type = SubstrateType.Core;
                        }
                        else
                        {
                            type = SubstrateType.Bin;
                            string body = string.Empty;
                            body = GetSubstrateHistoryFromFile(type, substrates[i]);
                            filebodies[substrates[i]] = body;
                        }

                        MoveSubstrateHistoryFile(type, substrates[i], backupSubstratePath);
                    }

                }

                // Carrier History
                File.Move(sourceFilePath, backupFullPath);                
            }
            catch
            {

            }
        }
        #endregion </AssignPath>

        #region <CarrierBasedEvents>
        public void WriteHistoryForIdRead(int portId, string carrierId, string lotId)
        {
            WriteCarrierLog(portId, carrierId, CarrierBasedEventType.IdRead, string.Format("아이디 읽음 : [랏:{0}], [캐리어:{1}]", lotId, carrierId));
        }
        public void WriteHistoryForLotInfo(int portId, string carrierId, string lotId, string partId, string stepSeq, string lotType, string lotQty)
        {
            WriteCarrierLog(portId, carrierId, CarrierBasedEventType.LotInfo, string.Format("랏 정보 요청 진행 : [랏:{0}], [파트:{1}], [스텝:{2}], [랏 타입:{3}], [랏 수량:{4}]", lotId, partId, stepSeq, lotType, lotQty));
        }
        public void WriteHistoryForSlotMap(int portId, string carrierId, Dictionary<int, string> status)
        {
            string logToWrite = string.Empty;
            foreach (var item in status)
            {
                if (false == string.IsNullOrEmpty(logToWrite))
                {
                    logToWrite = string.Format("{0}, [슬롯:{1}, 상태:{2}]", logToWrite, item.Key + 1, item.Value);
                }
                else
                {
                    logToWrite = string.Format("슬롯 정보 요청 진행 : [슬롯:{0}, 상태:{1}]", item.Key + 1, item.Value);
                }
            }

            WriteCarrierLog(portId, carrierId, CarrierBasedEventType.ReqSlotMap, logToWrite);
        }
        public void WriteHistoryForTrackIn(int portId, string carrierId, string scenario, string lotId)
        {
            if (scenario.Equals("SCENARIO_REQ_TRACK_IN"))
            {
                WriteCarrierLog(portId, carrierId, CarrierBasedEventType.TrackIn, string.Format("트랙인 진행 : [랏:{0}]", lotId));
            }
            else if (scenario.Equals("SCENARIO_REQ_LOT_MATCH"))
            {
                WriteCarrierLog(portId, carrierId, CarrierBasedEventType.LotMatch, string.Format("원부자재 교체 진행 [원부자재 랏:{0}]", lotId));
            }
        }
        public void WriteHistoryForTrackOut(int portId, string carrierId, string lotId)
        {
            WriteCarrierLog(portId, carrierId, CarrierBasedEventType.TrackOut, string.Format("트랙아웃 진행 : [랏:{0}]", lotId));
        }
        public void WriteHistoryForMerge(int portId, string carrierId, string newLotId, bool useChange, Dictionary<int, string> lotIdToMerge)
        {
            CarrierBasedEventType type;
            if (useChange)
            {
                type = CarrierBasedEventType.LotMergeAndChange;
            }
            else
            {
                type = CarrierBasedEventType.LotMerge;
            }

            string logToWrite = string.Empty;
            foreach (var item in lotIdToMerge)
            {
                if (false == string.IsNullOrEmpty(logToWrite))
                {
                    logToWrite = string.Format("{0}, [슬롯:{1}, 랏:{2}]", logToWrite, item.Key + 1, item.Value);
                }
                else
                {
                    logToWrite = string.Format("랏 [{0}] 으로 머지 진행 : [슬롯:{1}, 랏:{2}]", newLotId, item.Key + 1, item.Value);
                }
            }

            //logToWrite = string.Format("{0} -> {1}", logToWrite, newLotId);
            WriteCarrierLog(portId, carrierId, type, logToWrite);
        }
        public void WriteHistoryForSlotMapping(int portId, string carrierId, Dictionary<int, Tuple<string, string>> substratesToMapping, int count)
        {
            string logToWrite = string.Empty;
            if (substratesToMapping.Count > 0)
            {
                foreach (var item in substratesToMapping)
                {
                    if (false == string.IsNullOrEmpty(logToWrite))
                    {
                        logToWrite = string.Format("{0}, [슬롯:{1}, 이름:{2}, 수량:{3}]", logToWrite, item.Key + 1, item.Value.Item1, item.Value.Item2);
                    }
                    else
                    {
                        logToWrite = string.Format("슬롯 매핑 진행 : [슬롯:{0}, 이름:{1}, 수량:{2}]", item.Key + 1, item.Value.Item1, item.Value.Item2);
                    }
                }

                logToWrite = string.Format("{0} [슬롯내 웨이퍼 수량 : {1}]", logToWrite, count);
            }
            else
            {
                logToWrite = "슬롯 매핑 진행 : 비었음";
            }

            //logToWrite = string.Format("{0}", logToWrite);
            WriteCarrierLog(portId, carrierId, CarrierBasedEventType.SlotMapping, logToWrite);
        }
        #endregion </CarrierBasedEvents>

        #region <SubstrateBasedEvents>
        public void WriteSubstrateHistoryForDownloadMap(int portId, string carrierId, string substrateName, string ringId)
        {
            WriteSubstrateLog(portId, carrierId, substrateName, SubstrateBasedEventType.WorkStart, SubstrateType.Core, string.Format("바코드 인식하여 이름이 [{0}] 에서 [{1}] 으로 변경됨", ringId, substrateName));
        }
        public void WriteSubstrateHistoryForWaferSplit(int portId, string carrierId, string substrateName, string oldLotId, string newLotId, bool isLast)
        {
            string logToWrite = string.Format("랏이 스플릿되어 [{0}] 에서 [{1}] 으로 변경됨", oldLotId, newLotId);
            if (isLast)
            {
                logToWrite = string.Format("랏이 스플릿 되었으나 유지됨 [{0} -> {1}]", oldLotId, newLotId);
            }
            
            WriteSubstrateLog(portId, carrierId, substrateName, SubstrateBasedEventType.WaferSplit, SubstrateType.Core, logToWrite);
        }
        public void WriteSubstrateHistoryForStartOrFinishDetaching(int portId, string carrierId, string substrateName, bool isStarting)
        {
            SubstrateBasedEventType eventType;
            string logToWrite = string.Empty;
            if (isStarting)
            {
                eventType = SubstrateBasedEventType.StartDetaching;
                logToWrite = "작업 시작";
            }
            else
            {
                eventType = SubstrateBasedEventType.FinishDetaching;
                logToWrite = "작업 종료";
            }

            WriteSubstrateLog(portId, carrierId, substrateName, eventType, SubstrateType.Core, logToWrite);
        }
        public void WriteSubstrateHistoryForChipSplit(int corePortId, string coreCarrierId, string coreSubstrateName, int binPortId, string binSubstrateName, string splittedQty, string binCode, string assignedLotId, bool isFirst, bool isFully)
        {
            SubstrateBasedEventType eventType;
            string logToWriteForCore, logToWriteForBin;
            if (isFirst)
            {
                eventType = SubstrateBasedEventType.ChipSplit;
                logToWriteForCore = string.Format("[{0}] 수량만큼 칩 스플릿 되어 랏 [{1}] 생성, 공테이프 웨이퍼 [{2}] 에 부여될 예정 (빈코드:{3})", splittedQty, assignedLotId, binSubstrateName, binCode);
                logToWriteForBin = string.Format("공테이프 웨이퍼 [{0}] 에 코어 웨이퍼 [{1}] 로부터 스플릿된 랏 [{2}] 과 칩 수량 [{3}] 부여됨 (빈코드:{4})", binSubstrateName, coreSubstrateName, assignedLotId, splittedQty, binCode);
            }
            else
            {
                eventType = SubstrateBasedEventType.ChipSplitAndMerge;
                logToWriteForCore = string.Format("[{0}] 수량만큼 칩 스플릿 되어 임시 랏 [{1}] 생성, 빈 웨이퍼 [{2}] 에 병합될 예정 (빈코드:{3})", splittedQty, assignedLotId, binSubstrateName, binCode);
                logToWriteForBin = string.Format("빈 웨이퍼 [{0}] 에 코어 웨이퍼 [{1}] 로부터 스플릿된 랏 [{2}] 과 칩 수량 [{3}] 병합됨 (빈코드:{4})", binSubstrateName, coreSubstrateName, assignedLotId, splittedQty, binCode);
            }
            
            if (isFully)
            {
                logToWriteForCore = string.Format("{0}, (전량)", logToWriteForCore);
                logToWriteForBin = string.Format("{0}, (전량)", logToWriteForBin);
            }

            WriteSubstrateLog(corePortId, coreCarrierId, coreSubstrateName, eventType, SubstrateType.Core, logToWriteForCore);
            WriteSubstrateLog(binSubstrateName, eventType, SubstrateType.Bin, logToWriteForBin);
        }
        public void WriteSubstrateHistoryForWorkEnd(int portId, string carrierId, string substrateName, string remainingChips)
        {
            WriteSubstrateLog(portId, carrierId, substrateName, SubstrateBasedEventType.WorkEnd, SubstrateType.Core, string.Format("맵 업로드 및 작업 종료 이벤트 송신 [남은 칩:{0}]", remainingChips));
        }
        public void WriteSubstrateHistoryForTrackOut(int portId, string carrierId, string substrateName, string lotId, string remainingChips, bool isLast)
        {
            WriteSubstrateLog(portId, carrierId, substrateName, SubstrateBasedEventType.TrackOut, SubstrateType.Core, string.Format("랏 [{0}] 트랙 아웃 [남은 칩:{1}]", lotId, remainingChips));            
        }

        public void WriteSubstrateHistoryForReadRingId(int portId, string oldRingId, string newRingId)
        {            
            WriteSubstrateLog(newRingId, SubstrateBasedEventType.RingIdRead, SubstrateType.Bin, string.Format("바코드 인식하여 이름이 [{0}] 에서 [{1}] 으로 변경됨", oldRingId, newRingId));
        }
        public void WriteSubstrateHistoryForStartSorting(int portId, string substrateName)
        {
            WriteSubstrateLog(substrateName, SubstrateBasedEventType.StartSorting, SubstrateType.Bin, "작업 시작");
        }
        public void WriteSubstrateHistoryForFinishSorting(int portId, string substrateName, string assignedLotId, string materialLotId)
        {
            WriteSubstrateLog(substrateName, SubstrateBasedEventType.FinishSorting, SubstrateType.Bin, string.Format("작업 종료 (부여된 랏:{0}, 원부자재 랏:{1}]", assignedLotId, materialLotId));
        }
        public void WriteSubstrateHistoryForAssignSubstrateId(int portId, string substrateName, string assignedSubstrateName)
        {
            RenameBinSubstrateFile(substrateName, assignedSubstrateName);

            WriteSubstrateLog(assignedSubstrateName, SubstrateBasedEventType.IdAssign, SubstrateType.Bin, string.Format("서버로부터 이름이 [{0}] 으로 할당됨 [링 이름:{1}]", assignedSubstrateName, substrateName));
        }
        public void WriteSubstrateHistoryForBinWorkEnd(int portId, string substrateName, string binCode, string remainingChips)
        {
            WriteSubstrateLog(substrateName, SubstrateBasedEventType.WorkEnd, SubstrateType.Bin, string.Format("작업 종료 이벤트 송신 -> [빈코드:{0}], [칩수량:{1}]", binCode, remainingChips));
        }
        public void WriteSubstrateHistoryForBinTrackOut(int portId, string substrateName, string lotId, string binCode, string remainingChips)
        {
            WriteSubstrateLog(substrateName, SubstrateBasedEventType.TrackOut, SubstrateType.Bin, string.Format("랏 [{0}] 트랙 아웃 진행 [빈코드:{1}], [칩수량:{2}]", lotId, binCode, remainingChips));
        }
        public void WriteSubstrateHistoryForReqBinPartId(int portId, string substrateName, string binCode, string oldPartId, string newPartId)
        {
            WriteSubstrateLog(substrateName, SubstrateBasedEventType.ReqPartId, SubstrateType.Bin, string.Format("파트 아이디를 부여받아 [{0}] 에서 [{1}] 로 변경 [빈코드:{2}]", oldPartId, newPartId, binCode));
        }
        public void WriteSubstrateHistoryForUploadBinMap(int portId, string substrateName, string serializedMapData)
        {
            WriteSubstrateLog(substrateName, SubstrateBasedEventType.UploadBinData, SubstrateType.Bin, string.Format("맵 정보 업로드 진행 [직렬화된 Map Data:{0}]", serializedMapData));
        }
        public void WriteSubstrateHistoryForUploadBinData(int portId, string substrateName, string pmsPath)
        {
            var fullPath = Path.GetFullPath(pmsPath);
            WriteSubstrateLog(substrateName, SubstrateBasedEventType.UploadBinData, SubstrateType.Bin, string.Format("작업 정보 업로드 진행 [PMS파일 경로:{0}]", fullPath));
        }
        #endregion </SubstrateBasedEvents>

        #region <Executing>
        public void ExecuteWriteAsync()
        {
            if (QueueToWrite.Count <= 0)
                return;

            if (QueueToWrite.TryDequeue(out Tuple<string, string> logInfoToWrite))
            {
                WriteLog(logInfoToWrite.Item1, logInfoToWrite.Item2);
            }
        }
        #endregion </Executing>

        #region <Internal>
        private bool GetHistoryTimeFromLog(string message, ref DateTime time)
        {
            string[] splittedLine = message.Split('\t');
            if (splittedLine.Length <= 0)
                return false;

            return DateTime.TryParse(splittedLine[0], out time);            
        }
        private string GetSubstrateHistoryFromFile(SubstrateType type, string substrateName)
        {
            string sourceFilePath = string.Format(@"{0}\{1}\{2}{3}", BasePathForSubstrate, type.ToString(), substrateName, LogFileExtension);
            string fileBody = string.Empty;
            try
            {
                string sourcePath = Path.GetDirectoryName(sourceFilePath);
                if (false == Directory.Exists(sourcePath))
                    return fileBody;

                if (false == File.Exists(sourceFilePath))
                    return fileBody;

                using (StreamReader sr = new StreamReader(sourceFilePath))
                {
                    fileBody = sr.ReadToEnd();
                }

                return fileBody;
            }
            catch (Exception)
            {
                throw;
            }
        }
        private void MoveSubstrateHistoryFile(SubstrateType type, string substrateName, string newPath)
        {
            string sourceFilePath = string.Format(@"{0}\{1}\{2}{3}", BasePathForSubstrate, type.ToString(), substrateName, LogFileExtension);
            string destFilePath = string.Format(@"{0}\{1}{2}", newPath, substrateName, LogFileExtension);

            try
            {
                string sourcePath = Path.GetDirectoryName(sourceFilePath);
                if (false == Directory.Exists(sourcePath))
                    return;

                string destPath = Path.GetDirectoryName(destFilePath);
                if (false == Directory.Exists(destPath))
                    Directory.CreateDirectory(destPath);

                if (false == File.Exists(sourceFilePath))
                    return;

                if (File.Exists(destFilePath))
                    File.Delete(destFilePath);

                File.Move(sourceFilePath, destFilePath);
            }
            catch (Exception)
            {

                throw;
            }
        }
        private void RenameBinSubstrateFile(string oldName, string newName)
        {
            string sourceFilePath = string.Format(@"{0}\{1}\{2}{3}", BasePathForSubstrate, SubstrateType.Bin.ToString(), oldName, LogFileExtension);
            string destFilePath = string.Format(@"{0}\{1}\{2}{3}", BasePathForSubstrate, SubstrateType.Bin.ToString(), newName, LogFileExtension);

            try
            {
                string sourcePath = Path.GetDirectoryName(sourceFilePath);
                if (false == Directory.Exists(sourcePath))
                    return;

                string destPath = Path.GetDirectoryName(destFilePath);
                if (false == Directory.Exists(destPath))
                    Directory.CreateDirectory(destPath);

                if (false == File.Exists(sourceFilePath))
                    return;

                if (File.Exists(destFilePath))
                    File.Delete(destFilePath);

                File.Move(sourceFilePath, destFilePath);
            }
            catch (Exception)
            {

            }
        }        
        private void WriteSubstrateLog(int portId, string carrierId, string substrateName, SubstrateBasedEventType type, SubstrateType substrateType, string message)
        {
            // Substrate History 기록
            WriteSubstrateLog(substrateName, type, substrateType, message);

            // Carrier History 에도 기록
            WriteCarrierLog(portId, carrierId, substrateName, type, message);
        }
        private void WriteSubstrateLog(string substrateName, SubstrateBasedEventType type, SubstrateType substrateType, string message)
        {
            string filePath = string.Format(@"{0}\{1}\{2}{3}", BasePathForSubstrate, substrateType.ToString(), substrateName, LogFileExtension);
            DateTime time = DateTime.Now;
            var logEntry = string.Format("{0:d2}/{1:d2}-{2:d2}:{3:d2}:{4:d2}.{5:d3}\t{6}\t{7}\t{8}\t{9}",
                time.Month,
                time.Day,
                time.Hour,
                time.Minute,
                time.Second,
                time.Millisecond,
                string.Empty,       // Carrier Event Type
                substrateName,      // SubstrateName
                type.ToString(),    // Substrate Event Type
                message);

            EnqueueLogToWrite(filePath, logEntry);
        }
        private void WriteCarrierLog(int portId, string carrierId, string substrateName, SubstrateBasedEventType type, string message)
        {
            if (false == CurrentWorkingPath.TryGetValue(portId, out string basePath))
                return;

            string filePath = string.Format(@"{0}\{1}{2}", basePath, carrierId, LogFileExtension);
            DateTime time = DateTime.Now;
            var logEntry = string.Format("{0:d2}/{1:d2}-{2:d2}:{3:d2}:{4:d2}.{5:d3}\t{6}\t{7}\t{8}\t{9}",
                time.Month,
                time.Day,
                time.Hour,
                time.Minute,
                time.Second,
                time.Millisecond,
                string.Empty,       // Carrier Event Type
                substrateName,      // SubstrateName
                type.ToString(),    // Substrate Event Type
                message);

            if(_logMessageToDisplay != null)
            {
                _logMessageToDisplay(portId, logEntry);
            }

            EnqueueLogToWrite(filePath, logEntry);
        }
        private void WriteCarrierLog(int portId, string carrierId, CarrierBasedEventType type, string message)
        {
            if (false == CurrentWorkingPath.TryGetValue(portId, out string basePath))
                return;

            string filePath = string.Format(@"{0}\{1}{2}", basePath, carrierId, LogFileExtension);
            DateTime time = DateTime.Now;
            var logEntry = string.Format("{0:d2}/{1:d2}-{2:d2}:{3:d2}:{4:d2}.{5:d3}\t{6}\t{7}\t{8}\t{9}",
                time.Month,
                time.Day,
                time.Hour,
                time.Minute,
                time.Second,
                time.Millisecond,
                type.ToString(),        // Carrier Event Type
                string.Empty,           // SubstrateName
                string.Empty,           // Substrate Event Type
                message);

            EnqueueLogToWrite(filePath, logEntry);
        }

        private void EnqueueLogToWrite(string filePath, string logEntry)
        {
            QueueToWrite.Enqueue(Tuple.Create(filePath, logEntry));
        }
        private void WriteLog(string filePath, string logEntry)
        {
            try
            {
                string dirName = Path.GetDirectoryName(filePath);
                if (false == Directory.Exists(dirName))
                    Directory.CreateDirectory(dirName);

                using (FileStream fs = new FileStream(filePath, FileMode.Append, FileAccess.Write, FileShare.ReadWrite))
                using (StreamWriter sw = new StreamWriter(fs))
                {
                    sw.AutoFlush = true;

                    sw.WriteLine(logEntry);
                }
            }
            catch (Exception)
            {
            }
        }
        #endregion </Internal>

        #endregion </Methods>
    }

    public enum CcwRotation
    {
        Deg0 = 0,
        Deg90 = 90,
        Deg180 = 180,
        Deg270 = 270
    }

    public static class ReferenceFinder
    {
        public static (int X, int Y) GetPosition(
            int countX,
            int countY,
            CcwRotation angle,
            string mapData,
            string notchCode,
            string nullBincode)
        {
            if (mapData == null ||
                countX <= 0 ||
                countY <= 0 ||
                nullBincode == null ||
                notchCode == null)
                return (0, 0);
            
            if (nullBincode.Length != notchCode.Length)
                return (0, 0);

            var cellSize = nullBincode.Length;

            int expectedLen = checked(countX * countY * cellSize);
            if (mapData.Length != expectedLen)
                return (0, 0);

            int totalTokens = countX * countY;

            // 1) 노치코드 위치를 탐색
            int tokenIndex = FindNotchIndex(mapData, totalTokens, cellSize, notchCode);

            // 2) 노치가 없으면 첫 Bincode 위치를 탐색
            if (tokenIndex < 0)
            {
                tokenIndex = FindFirstBincodeIndex(mapData, totalTokens, cellSize, nullBincode);
                if (tokenIndex < 0)
                    return (0, 0);
            }

            // 3) 현재 맵 기준 좌표를 얻어옴
            var (xCur, yCur) = IndexToPosition(tokenIndex, countX);

            // 4) 입력 각도 기준으로 역회전보상하여 좌표를 변환
            return RotatePositionToZero(xCur, yCur, countX, countY, angle);
        }

        private static int FindNotchIndex(string s, int totalTokens, int tokenSize, string token)
        {
            for (int t = 0; t < totalTokens; t++)
            {
                int pos = t * tokenSize;
                if (IsTokenEqual(s, pos, token))
                    return t;
            }

            return -1;
        }

        private static int FindFirstBincodeIndex(string s, int totalTokens, int tokenSize, string blankToken)
        {
            for (int t = 0; t < totalTokens; t++)
            {
                int pos = t * tokenSize;
                if (!IsTokenEqual(s, pos, blankToken)) return t;
            }
            return -1;
        }

        private static bool IsTokenEqual(string s, int pos, string token)
        {
            if (pos < 0 || pos + token.Length > s.Length) 
                return false;

            for (int i = 0; i < token.Length; i++)
            {
                if (s[pos + i] != token[i]) 
                    return false;
            }

            return true;
        }

        private static (int X, int Y) IndexToPosition(int tokenIndex, int widthCells)
        {
            int x = (tokenIndex % widthCells) + 1;
            int y = (tokenIndex / widthCells) + 1;

            return (x, y);
        }

        private static (int X0, int Y0) RotatePositionToZero(
            int xCur, int yCur,
            int widthCells, int heightCells,
            CcwRotation r)
        {
            int w0 = (r == CcwRotation.Deg90 || r == CcwRotation.Deg270) ? heightCells : widthCells;
            int h0 = (r == CcwRotation.Deg90 || r == CcwRotation.Deg270) ? widthCells : heightCells;

            switch (r)
            {
                case CcwRotation.Deg0: 
                    return (xCur, yCur);
                case CcwRotation.Deg90:
                    return ((w0 + 1) - yCur, xCur);
                case CcwRotation.Deg180:
                    return ((w0 + 1) - xCur, (h0 + 1) - yCur);
                case CcwRotation.Deg270:
                    return (yCur, (h0 + 1) - xCur);
                default:
                    return (0, 0);
            }
        }
    }

    public class CommonFunctionsForPWA500
    {
        #region <Constructors>
        public CommonFunctionsForPWA500(bool useTrackOutCore, bool useComparePartId)
        {
            _substrateManager = SubstrateManager.Instance;
            _carrierServer = CarrierManagementServer.Instance;
            _loadPortManager = LoadPortManager.Instance;
            _processGroup = ProcessModuleGroup.Instance;
            _recipe = FrameOfSystem3.Recipe.Recipe.GetInstance();

            _lotHistoryLog = LotHistoryLog.Instance;

            UseTrackOutCore = useTrackOutCore;
            UseComparePartId = useComparePartId;
        }
        #endregion </Constructors>

        #region <Fields>
        private static SubstrateManager _substrateManager = null;
        private static CarrierManagementServer _carrierServer = null;
        private static ProcessModuleGroup _processGroup = null;
        private static LoadPortManager _loadPortManager = null;
        private static LotHistoryLog _lotHistoryLog = null;

        private static FrameOfSystem3.Recipe.Recipe _recipe = null;
        private Func<string, string, string, string, string[], string[], EN_MESSAGE_RESULT, bool, bool> _funcToSendClientMessage = null;
        private Action<EN_SCENARIO, Dictionary<string, string>, Dictionary<string, string>> _actionToEnqueueScenarioAsync = null;
        private const int ProcessModuleIndex = 0;

        private Func<string, Dictionary<string, string>, bool> _funcToUpdateScenarioParam = null;
        private Func<EN_SCENARIO, EN_SCENARIO_RESULT> _funcToExecuteScenario = null;

        private readonly TickCounter TicksForCarrierLoad = new TickCounter();
        private QueuedScenarioInfo _dequeuedScenarioToCarrierLoad = null;
        private readonly ConcurrentQueue<QueuedScenarioInfo> CarrierLoadingReservation = new ConcurrentQueue<QueuedScenarioInfo>();

        private static BinDataToUploadFromPWA500 _binDataToUpload = null;

        private string _pathForPms = string.Empty;
        #endregion </Fields>

        #region <Properties>
        private bool UseCoreMapHandlingOnly
        {
            get
            {
                return false;// (false == _recipe.GetValue(EN_RECIPE_TYPE.COMMON, PARAM_COMMON.UseSecsGem.ToString(), false));
            }
        }
        private int HandlingRequestDelayEachLoadPorts
        {
            get
            {
                return _recipe.GetValue(EN_RECIPE_TYPE.EQUIPMENT, PARAM_EQUIPMENT.HandlingRequestDelayEachLoadPorts.ToString(), 5000);
            }
        }
        public string PmsFullPath
        {
            get
            {
                return _pathForPms;
            }
        }
        public bool HasScenarioError { get; set; }
        public EN_SCENARIO FailedScenarioTypes { get; set; }
        public string ScenarioErrorDescription { get; set; }

        protected bool UseTrackOutCore { get; private set; }
        protected bool UseComparePartId { get; private set; }
        protected static Recipe Recipe
        {
            get
            {
                return _recipe;
            }
        }
        protected static SubstrateManager SubstrateManager
        {
            get
            {
                return _substrateManager;
            }
        }
        protected static CarrierManagementServer CarrierServer
        {
            get
            {
                return _carrierServer;
            }
        }
        protected static LoadPortManager LoadPortManager
        {
            get
            {
                return _loadPortManager;

            }
        }
        #endregion </Properties>

        #region <Methods>

        #region <Assign Functions>
        public void AssignFunctionToSendClientMessage(Func<string, string, string, string, string[], string[], EN_MESSAGE_RESULT, bool, bool> func)
        {
            _funcToSendClientMessage = func;
        }
        public void AssignActionToEnqueueScenarioAsync(Action<EN_SCENARIO, Dictionary<string, string>, Dictionary<string, string>> action)
        {
            _actionToEnqueueScenarioAsync = action;
        }
        public void AssignFunctionToUpdateParam(Func<string, Dictionary<string, string>, bool> func)
        {
            _funcToUpdateScenarioParam = func;
        }
        public void AssignFunctionToExecuteScenario(Func<EN_SCENARIO, EN_SCENARIO_RESULT> func)
        {
            _funcToExecuteScenario = func;
        }
        #endregion </Assign Functions>

        #region <OHT Handling>
        // 
        public void EnqueueScenarioCarrierHandlingAsync(int portId, LoadPortLoadingMode loadingType, string lotId, EN_SCENARIO scenario)
        {
            var param = MakeParamToOHTHandling(portId, loadingType, lotId, scenario);
            var queuedScenario = new QueuedScenarioInfo
            {
                Scenario = scenario,
                ScenarioParams = param
            };
            if (scenario.Equals(EN_SCENARIO.SCENARIO_PORT_STATUS_LOAD_1) ||
                scenario.Equals(EN_SCENARIO.SCENARIO_PORT_STATUS_LOAD_2) ||
                scenario.Equals(EN_SCENARIO.SCENARIO_PORT_STATUS_LOAD_3) ||
                scenario.Equals(EN_SCENARIO.SCENARIO_PORT_STATUS_LOAD_4) ||
                scenario.Equals(EN_SCENARIO.SCENARIO_PORT_STATUS_LOAD_5) ||
                scenario.Equals(EN_SCENARIO.SCENARIO_PORT_STATUS_LOAD_6))
            {
                CarrierLoadingReservation.Enqueue(queuedScenario);
            }
            else if (scenario.Equals(EN_SCENARIO.SCENARIO_PORT_STATUS_UNLOAD_1) ||
                    scenario.Equals(EN_SCENARIO.SCENARIO_PORT_STATUS_UNLOAD_2) ||
                    scenario.Equals(EN_SCENARIO.SCENARIO_PORT_STATUS_UNLOAD_3) ||
                    scenario.Equals(EN_SCENARIO.SCENARIO_PORT_STATUS_UNLOAD_4) ||
                    scenario.Equals(EN_SCENARIO.SCENARIO_PORT_STATUS_UNLOAD_5) ||
                    scenario.Equals(EN_SCENARIO.SCENARIO_PORT_STATUS_UNLOAD_6))
            {
                //CarrierUnloadingReservation.Enqueue(queuedScenario);
            }

            //string message = string.Format("[{0:d2}/{1:d2}-{2:d2}:{3:d2}:{4:d2}.{5:d3}] Scenario : {6} Enqueued !! ",
            //    DateTime.Now.Month,
            //    DateTime.Now.Day,
            //    DateTime.Now.Hour,
            //    DateTime.Now.Minute,
            //    DateTime.Now.Second,
            //    DateTime.Now.Millisecond,
            //    scenario.ToString());
            //Console.WriteLine(message);
        }

        public void ExecuteScanrioToCarrierLoadAsync()
        {
            if (_funcToUpdateScenarioParam == null || _funcToExecuteScenario == null)
                return;

            if (UseCoreMapHandlingOnly)
            {
                while (CarrierLoadingReservation.Count > 0)
                {
                    CarrierLoadingReservation.TryDequeue(out _);
                }

                return;
            }

            if (_dequeuedScenarioToCarrierLoad != null)
            {
                var result = _funcToExecuteScenario(_dequeuedScenarioToCarrierLoad.Scenario);
                switch (result)
                {
                    case EN_SCENARIO_RESULT.WAITING:
                    case EN_SCENARIO_RESULT.PROCEED:
                        return;

                    case EN_SCENARIO_RESULT.COMPLETED:
                    case EN_SCENARIO_RESULT.ERROR:
                    case EN_SCENARIO_RESULT.TIMEOUT_ERROR:
                        {
                            TicksForCarrierLoad.SetTickCount((uint)HandlingRequestDelayEachLoadPorts);
                            _dequeuedScenarioToCarrierLoad = null;

                            // 종료 중이면 비운다.
                            if (false == EquipmentState_.EquipmentState.GetInstance().GetState().Equals(EquipmentState_.EQUIPMENT_STATE.SETUP) &&
                                false == EquipmentState_.EquipmentState.GetInstance().GetState().Equals(EquipmentState_.EQUIPMENT_STATE.FINISHING) &&
                                false == EquipmentState_.EquipmentState.GetInstance().GetState().Equals(EquipmentState_.EQUIPMENT_STATE.EXECUTING))
                            {
                                while (CarrierLoadingReservation.Count > 0)
                                {
                                    CarrierLoadingReservation.TryDequeue(out _);
                                }
                            }
                        }
                        break;
                    default:
                        break;
                }
            }
            else
            {
                if (CarrierLoadingReservation.Count <= 0)
                    return;

                // 셋 된 상태에서 Tick이 넘어가지 않았으면 리턴
                if (false == TicksForCarrierLoad.IsTickOver(false) &&
                    TicksForCarrierLoad.IsSet())
                    return;

                CarrierLoadingReservation.TryDequeue(out _dequeuedScenarioToCarrierLoad);
                // 파라메터 갱신
                Enum scenario = _dequeuedScenarioToCarrierLoad.Scenario;
                var scenarioParams = _dequeuedScenarioToCarrierLoad.ScenarioParams;
                _funcToUpdateScenarioParam(scenario.ToString(), scenarioParams);

                string message = string.Format("[{0:d2}/{1:d2}-{2:d2}:{3:d2}:{4:d2}.{5:d3}] Scenario : {6} Dequeued !! ",
                                DateTime.Now.Month,
                                DateTime.Now.Day,
                                DateTime.Now.Hour,
                                DateTime.Now.Minute,
                                DateTime.Now.Second,
                                DateTime.Now.Millisecond,
                                _dequeuedScenarioToCarrierLoad.Scenario.ToString());
                Console.WriteLine(message);
            }
        }
        #endregion </OHT Handling>

        public bool ExecuteScenarioAsyncToCarrierLoad(string lotId, string carrierId)
        {
            if (_actionToEnqueueScenarioAsync == null)
                return false;

            var scenarioParam = new Dictionary<string, string>
            {
                [CarrierLoadUnloadKeys.KeyParamCarrierId] = carrierId,
                [CarrierLoadUnloadKeys.KeyParamLotId] = lotId
            };

            _actionToEnqueueScenarioAsync(EN_SCENARIO.SCENARIO_CARRIER_LOAD, scenarioParam, null);

            return true;
        }
        public bool ExecuteScenarioAsyncToCarrierUnload(string lotId, string partId, string stepId, string lotType)
        {
            if (_actionToEnqueueScenarioAsync == null)
                return false;

            var scenarioParam = new Dictionary<string, string>
            {
                [CarrierLoadUnloadKeys.KeyParamLotId] = lotId,
                [CarrierLoadUnloadKeys.KeyParamPartId] = partId,
                [CarrierLoadUnloadKeys.KeyParamStepId] = stepId,
                [CarrierLoadUnloadKeys.KeyParamLotType] = lotType
            };

            _actionToEnqueueScenarioAsync(EN_SCENARIO.SCENARIO_CARRIER_UNLOAD, scenarioParam, null);

            return true;
        }
        public string GetModelName()
        {
            return _recipe.GetValue(FrameOfSystem3.Recipe.EN_RECIPE_TYPE.EQUIPMENT, FrameOfSystem3.Recipe.PARAM_EQUIPMENT.MachineName.ToString(), string.Empty);
        }
        public string GetPMSFileName(string lotId, string substrateId)
        {
            return string.Format("{0}_{1}_{2}_{3}", GetModelName(), lotId, substrateId, DateTime.Now.ToString("yyMMddHHmmss"));
        }
        // 2025.05.16 dwlim [ADD] BinMap Upload를 위한 저장된 PMS File의 Data 불러오기
        public string[] GetPMSDataFromPMSFile(string fullPath)
        {
            if (string.IsNullOrWhiteSpace(fullPath) ||
                false == File.Exists(fullPath))
            {
                return null;
            }

            try
            {
                string[] data = File.ReadAllLines(fullPath);
                return data;
            }
            catch (Exception)
            {
                return null;
            }
        }
        public bool MakePMSFile(string lotId, string substrateId, string fileName, string body, ref string fullPath)
        {
            fullPath = string.Format(@"{0}\PMS\{1}\{2,00:d2}\{3,00:d2}\{4}\{5}\{6}.PMS", Define.DefineConstant.FilePath.FILEPATH_LOG,
                DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day,
                lotId, substrateId,
                fileName);

            StreamWriter sw = null;

            try
            {
                string dir = Path.GetDirectoryName(fullPath);
                if (false == Directory.Exists(dir))
                    Directory.CreateDirectory(dir);

                if (File.Exists(fullPath))
                    File.Delete(fullPath);

                sw = new StreamWriter(fullPath);

                sw.Write(body);

                sw.Close();
            }
            catch (Exception)
            {
                if (sw != null)
                {
                    sw.Close();
                }

                return false;
            }

            return true;
        }
        public void MakeBinDataToUpload(string nameOfEq, string substrateId, string ringId,
            int chipQty, double angle, int countRow, int countCol, string nullBinCode, string mapData,
            string pmsFileBody, string userId, bool useEventHandling)
        {
            _binDataToUpload = new BinDataToUploadFromPWA500(nameOfEq, substrateId, ringId,
                chipQty, angle, countRow, countCol, nullBinCode, mapData,
                pmsFileBody, userId, useEventHandling);
        }
        public bool GetBinDataToUpload(ref BinDataToUploadFromPWA500 dataToUpload)
        {
            if (_binDataToUpload == null)
                return false;

            dataToUpload = _binDataToUpload;
            return true;
        }
        public void ClearBinDataToUpload()
        {
            if (_binDataToUpload != null)
                _binDataToUpload = null;
        }
        // [TODO] : 2025.05.16 dwlim [ADD] 로그 제출로인해 작성. 나중에 수정해야함
        public Dictionary<string, string> MakeScenarioParamToUploadBinMap
            (string substrateId, string ringId, int chipQty, double angle, int countRow, int countCol, string nullBinCode, string mapData,
            string userId, bool useEventHandling, BinDataToUploadFromPWA500 bindata)
        {
            string recipeId = GetRecipeId();
            string lotId = string.Empty;
            //string fileName = string.Empty;

            if (GetSubstrateByName(substrateId, out var substrate) ||
                GetSubstrateByName(ringId, out substrate))
            {
                lotId = substrate.LotId;
            }

            MapData e142Mapdata = new MapData();
            MapDataControl e142MapControl = new MapDataControl();
            PMSControl e142PMSControl = new PMSControl();
            Dictionary<string, List<string[]>> transferedDiesData = new Dictionary<string, List<string[]>>();
            (int refX, int refY) = FindReferencePosition(countCol, countRow, angle, mapData, "D", nullBinCode);
            if (null != bindata.PmsFileBody)
            {
                transferedDiesData = e142PMSControl.GetTransferedData(bindata.PmsFileBody);
                e142Mapdata = e142MapControl.MakeBinMapObject(lotId, substrateId, recipeId, mapData, (int)angle, countCol, countRow, chipQty, refX, refY, transferedDiesData);
            }

            string serializedMapdata = e142MapControl.SerializeMapData(e142Mapdata);

            Dictionary<string, string> scenarioParams = new Dictionary<string, string>
            {
                [UploadCoreOrBinFileKeys.KeyParamWaferId] = substrateId,
                [UploadCoreOrBinFileKeys.KeyParamMapData] = serializedMapdata,
            };

            return scenarioParams;
        }
        public Dictionary<string, string> MakeScenarioParamToUploadBinData
            (string nameOfEq, string substrateId, string ringId,
            int chipQty, double angle, int countRow, int countCol, string nullBinCode, string mapData,
            string pmsFileBody, string userId, bool useEventHandling)
        {
            string recipeId = GetRecipeId();
            Dictionary<string, string> scenarioParams = new Dictionary<string, string>
            {
                [UploadCoreOrBinFileKeys.KeyParamCarrierId] = string.Empty,
                [UploadCoreOrBinFileKeys.KeyParamPortId] = string.Empty,
                [UploadCoreOrBinFileKeys.KeyParamLotId] = string.Empty,
                [UploadCoreOrBinFileKeys.KeyParamPartId] = string.Empty,
                [UploadCoreOrBinFileKeys.KeyParamRecipeId] = recipeId,

                // 슬롯 번호가 없다??
                //[UploadCoreOrBinFileKeys.KeyParamSlotId] = (slot).ToString(),

                [UploadCoreOrBinFileKeys.KeyParamOperatorId] = userId,
                [UploadCoreOrBinFileKeys.KeyChipQty] = chipQty.ToString(),
                [UploadCoreOrBinFileKeys.KeyPMSFileName] = string.Empty,
                [UploadCoreOrBinFileKeys.KeyPMSFileBody] = string.Empty,

                [UploadCoreOrBinFileKeys.KeySubstrateName] = substrateId,

                [UploadCoreOrBinFileKeys.KeyWaferAngle] = angle.ToString(),
                [UploadCoreOrBinFileKeys.KeyCountRow] = countRow.ToString(),
                [UploadCoreOrBinFileKeys.KeyCountCol] = countCol.ToString(),
                [UploadCoreOrBinFileKeys.KeyReferenceX] = string.Empty,
                [UploadCoreOrBinFileKeys.KeyReferenceY] = string.Empty,
                [UploadCoreOrBinFileKeys.KeyStartingPosX] = string.Empty,
                [UploadCoreOrBinFileKeys.KeyStartingPosY] = string.Empty,
                [UploadCoreOrBinFileKeys.KeyNullBinCode] = nullBinCode,
                [UploadCoreOrBinFileKeys.KeyMapData] = mapData,

                [UploadCoreOrBinFileKeys.KeyUseEventHandling] = useEventHandling.ToString(),
            };

            if (GetSubstrateByName(substrateId, out var substrate) ||
                GetSubstrateByName(ringId, out substrate))
            {
                string lotId = substrate.LotId;
                int portId, slot;
                string partId = substrate.GetAttribute(PWA500SubstrateAttributes.PartId);


                //Dictionary<string, string> additionalParams = null;
                string fileName = string.Empty;
                portId = substrate.DestinationPortId;
                slot = substrate.DestinationSlot;
                fileName = GetPMSFileName(lotId, substrateId);
                if (false == MakePMSFile(lotId, substrateId, fileName, pmsFileBody, ref _pathForPms))
                    return scenarioParams;

                if (portId <= 0 || slot < 0)
                    return scenarioParams;

                string carrierId = _carrierServer.GetCarrierId(portId);

                scenarioParams[UploadCoreOrBinFileKeys.KeyParamCarrierId] = carrierId;
                scenarioParams[UploadCoreOrBinFileKeys.KeyParamPortId] = GetPortName(portId);
                scenarioParams[UploadCoreOrBinFileKeys.KeyParamLotId] = lotId;
                scenarioParams[UploadCoreOrBinFileKeys.KeyParamPartId] = partId;
                scenarioParams[UploadCoreOrBinFileKeys.KeyParamRecipeId] = recipeId;

                // 슬롯 번호가 없다??
                //[UploadCoreOrBinFileKeys.KeyParamSlotId] = (slot).ToString();

                scenarioParams[UploadCoreOrBinFileKeys.KeyParamOperatorId] = userId;
                scenarioParams[UploadCoreOrBinFileKeys.KeyChipQty] = chipQty.ToString();
                scenarioParams[UploadCoreOrBinFileKeys.KeyPMSFileName] = fileName;
                scenarioParams[UploadCoreOrBinFileKeys.KeyPMSFileBody] = PmsFullPath;

                scenarioParams[UploadCoreOrBinFileKeys.KeySubstrateName] = substrateId;

                var (x, y) = FindReferencePosition(countCol, countRow, angle, mapData, "D", nullBinCode);
                
                scenarioParams[UploadCoreOrBinFileKeys.KeyWaferAngle] = angle.ToString();
                scenarioParams[UploadCoreOrBinFileKeys.KeyCountRow] = countRow.ToString();
                scenarioParams[UploadCoreOrBinFileKeys.KeyCountCol] = countCol.ToString();
                scenarioParams[UploadCoreOrBinFileKeys.KeyReferenceX] = x.ToString();
                scenarioParams[UploadCoreOrBinFileKeys.KeyReferenceY] = y.ToString();
                scenarioParams[UploadCoreOrBinFileKeys.KeyStartingPosX] = x.ToString();
                scenarioParams[UploadCoreOrBinFileKeys.KeyStartingPosY] = y.ToString();
                scenarioParams[UploadCoreOrBinFileKeys.KeyNullBinCode] = nullBinCode;
                scenarioParams[UploadCoreOrBinFileKeys.KeyMapData] = mapData;

                scenarioParams[UploadCoreOrBinFileKeys.KeyUseEventHandling] = useEventHandling.ToString();

                _substrateManager.SetAttributeByKey(substrate.UniqueKey, PWA500SubstrateAttributes.RefPositionX, x.ToString());
                _substrateManager.SetAttributeByKey(substrate.UniqueKey, PWA500SubstrateAttributes.RefPositionY, y.ToString());
                _substrateManager.SetAttributeByKey(substrate.UniqueKey, PWA500SubstrateAttributes.StartingPositionX, x.ToString());
                _substrateManager.SetAttributeByKey(substrate.UniqueKey, PWA500SubstrateAttributes.StartingPositionY, y.ToString());
                _substrateManager.SetAttributeByKey(substrate.UniqueKey, PWA500SubstrateAttributes.CountX, countCol.ToString());
                _substrateManager.SetAttributeByKey(substrate.UniqueKey, PWA500SubstrateAttributes.CountY, countRow.ToString());
                _substrateManager.SetAttributeByKey(substrate.UniqueKey, PWA500SubstrateAttributes.Angle, angle.ToString());
                _substrateManager.SetAttributeByKey(substrate.UniqueKey, PWA500SubstrateAttributes.MapData, mapData);
                _substrateManager.SetAttributeByKey(substrate.UniqueKey, PWA500SubstrateAttributes.ChipQty, chipQty.ToString());
                _substrateManager.SaveDataByKey(substrate.UniqueKey);

                //_substrateManager.SetAttributesByKey(substrate.UniqueKey, new Dictionary<string, string>
                //{
                //    [PWA500SubstrateAttributes.MapData] = mapData,
                //    [PWA500SubstrateAttributes.ChipQty] = chipQty.ToString(),
                //});

                // 2026.02.11. jhlim [ADD] 고객사 요청으로 생성된 PMS 파일을 특정 폴더에 모아서 백업한다.
                try
                {
                    var backupPath = Path.Combine(Define.DefineConstant.FilePath.FILEPATH_LOG, "PMSBackup");
                    var fileNameWithEx = Path.GetFileName(PmsFullPath);
                    var destFilePath = $@"{backupPath}\{fileNameWithEx}";

                    if (false == Directory.Exists(backupPath))
                        Directory.CreateDirectory(backupPath);

                    if (File.Exists(destFilePath))
                    {
                        File.Delete(destFilePath);
                    }

                    File.Copy(PmsFullPath, destFilePath);
                }
                catch (Exception)
                {
                }
                // 2026.02.11. jhlim [END]
                return scenarioParams;
            }
            //else
            //{
            //    if (false == useEventHandling)
            //    {
            //        Dictionary<string, string> scenarioParams = new Dictionary<string, string>
            //        {
            //            [UploadCoreOrBinFileKeys.KeyParamCarrierId] = string.Empty,
            //            [UploadCoreOrBinFileKeys.KeyParamPortId] = string.Empty,
            //            [UploadCoreOrBinFileKeys.KeyParamLotId] = string.Empty,
            //            [UploadCoreOrBinFileKeys.KeyParamPartId] = string.Empty,
            //            [UploadCoreOrBinFileKeys.KeyParamRecipeId] = _functionsForPWA500.GetRecipeId(),

            //            [UploadCoreOrBinFileKeys.KeyParamOperatorId] = userId,
            //            [UploadCoreOrBinFileKeys.KeyChipQty] = chipQty.ToString(),

            //            [UploadCoreOrBinFileKeys.KeyPMSFileName] = string.Empty,
            //            [UploadCoreOrBinFileKeys.KeyPMSFileBody] = string.Empty,

            //            [UploadCoreOrBinFileKeys.KeySubstrateName] = substrateId,

            //            [UploadCoreOrBinFileKeys.KeyWaferAngle] = angle.ToString(),
            //            [UploadCoreOrBinFileKeys.KeyCountRow] = countRow.ToString(),
            //            [UploadCoreOrBinFileKeys.KeyCountCol] = countCol.ToString(),
            //            [UploadCoreOrBinFileKeys.KeyNullBinCode] = nullBinCode,
            //            [UploadCoreOrBinFileKeys.KeyMapData] = mapData,
            //            [UploadCoreOrBinFileKeys.KeyReferenceX] = "0",
            //            [UploadCoreOrBinFileKeys.KeyReferenceY] = "0",
            //            [UploadCoreOrBinFileKeys.KeyStartingPosX] = "0",
            //            [UploadCoreOrBinFileKeys.KeyStartingPosY] = "0",

            //            [UploadCoreOrBinFileKeys.KeyUseEventHandling] = useEventHandling.ToString(),
            //        };

            //        Dictionary<string, string> additionalParams = new Dictionary<string, string>
            //        {
            //            [AdditionalParamKeys.KeyNameOfEq] = nameOfEq,
            //            [AdditionalParamKeys.KeySubstrateId] = substrateId,
            //            [AdditionalParamKeys.KeyChipQty] = chipQty.ToString(),
            //            [AdditionalParamKeys.KeyUserId] = userId,
            //        };

            //        EnqueueScenarioAsync(scenario, scenarioParams, additionalParams);

            //        return true;

            //        // 2024.08.18 : [END]
            //    }
            //}

            return scenarioParams;
        }

        private static (int X, int Y) FindReferencePosition(int countX, int countY, double angle,
            string mapData, string notch, string nullBincode)
        {
            CcwRotation ccw;
            if (angle == 90)
                ccw = CcwRotation.Deg90;
            else if (angle == 180)
                ccw = CcwRotation.Deg180;
            else if (angle == 270)
                ccw = CcwRotation.Deg270;
            else
                ccw = CcwRotation.Deg0;

            return ReferenceFinder.GetPosition(countX, countY, ccw, mapData, notch, nullBincode);
        }

        public void SetScenarioError(EN_SCENARIO failedScenario, string description = "")
        {
            FailedScenarioTypes = failedScenario;
            HasScenarioError = true;
            ScenarioErrorDescription = description;
        }
        public void ExecuteAfterScenarioCompletion(EN_SCENARIO typeOfScenario,
            Dictionary<string, string> scenarioParams,
            Dictionary<string, string> resultOfScenario,
            Dictionary<string, string> additionalParams,
            EN_MESSAGE_RESULT result,
            bool isManual = false)
        {
            // 완료된 시나리오 타입에 따라 실행되어야할 액션을 여기서 선택한다.
            switch (typeOfScenario)
            {
                case EN_SCENARIO.SCENARIO_WORK_START:
                    {
                        #region
                        Dictionary<string, string> messageContentToSend = new Dictionary<string, string>();
                        messageContentToSend[ResultKeys.KeyResult] = result.ToString();
                        if (result.Equals(EN_MESSAGE_RESULT.OK))
                        {
                            messageContentToSend[ResultKeys.KeyDescription] = string.Empty;
                        }
                        else
                        {
                            messageContentToSend[ResultKeys.KeyDescription] = "Gem Error";
                        }

                        if (false == resultOfScenario.TryGetValue(RequestDownloadMapFileKeys.KeyResultSubstrateId, out string resultSubstrateId))
                        {
                            result = EN_MESSAGE_RESULT.NG;
                            messageContentToSend[ResultKeys.KeyResult] = result.ToString();
                        }

                        if (false == resultOfScenario.TryGetValue(RequestDownloadMapFileKeys.KeyResultCountRow, out string resultCountRow))
                        {
                            result = EN_MESSAGE_RESULT.NG;
                            messageContentToSend[ResultKeys.KeyResult] = result.ToString();
                        }

                        if (false == resultOfScenario.TryGetValue(RequestDownloadMapFileKeys.KeyResultCountCol, out string resultCountCol))
                        {
                            result = EN_MESSAGE_RESULT.NG;
                            messageContentToSend[ResultKeys.KeyResult] = result.ToString();
                        }

                        if (false == resultOfScenario.TryGetValue(RequestDownloadMapFileKeys.KeyResultAngle, out string resultAngle))
                        {
                            result = EN_MESSAGE_RESULT.NG;
                            messageContentToSend[ResultKeys.KeyResult] = result.ToString();
                        }

                        if (false == resultOfScenario.TryGetValue(RequestDownloadMapFileKeys.KeyResultQty, out string resultQty))
                        {
                            result = EN_MESSAGE_RESULT.NG;
                            messageContentToSend[ResultKeys.KeyResult] = result.ToString();
                        }

                        if (false == resultOfScenario.TryGetValue(RequestDownloadMapFileKeys.KeyResultReferenceX, out string resultRefX))
                        {
                            result = EN_MESSAGE_RESULT.NG;
                            messageContentToSend[ResultKeys.KeyResult] = result.ToString();
                        }
                        if (false == resultOfScenario.TryGetValue(RequestDownloadMapFileKeys.KeyResultReferenceY, out string resultRefY))
                        {
                            result = EN_MESSAGE_RESULT.NG;
                            messageContentToSend[ResultKeys.KeyResult] = result.ToString();
                        }
                        if (false == resultOfScenario.TryGetValue(RequestDownloadMapFileKeys.KeyResultStartingX, out string resultStartX))
                        {
                            result = EN_MESSAGE_RESULT.NG;
                            messageContentToSend[ResultKeys.KeyResult] = result.ToString();
                        }
                        if (false == resultOfScenario.TryGetValue(RequestDownloadMapFileKeys.KeyResultStartingY, out string resultStartY))
                        {
                            result = EN_MESSAGE_RESULT.NG;
                            messageContentToSend[ResultKeys.KeyResult] = result.ToString();
                        }


                        if (false == resultOfScenario.TryGetValue(RequestDownloadMapFileKeys.KeyResultMapData, out string resultMapData))
                        {
                            result = EN_MESSAGE_RESULT.NG;
                            messageContentToSend[ResultKeys.KeyResult] = result.ToString();
                        }

                        Substrate substrate;
                        if (isManual)
                        {
                            if (false == FindSubstrateByNameOrRingIdAtProcessModule(resultSubstrateId, resultSubstrateId, out substrate, out _) || substrate == null)
                                return;

                            SetSubstrateAttributes(substrate,
                                resultSubstrateId,
                                resultAngle,
                                resultCountRow,
                                resultCountCol,
                                resultQty,
                                resultRefX,
                                resultRefY,
                                resultStartX,
                                resultStartY,
                                resultMapData);
                        }
                        else
                        {
                            if (additionalParams == null)
                                return;

                            if (false == additionalParams.TryGetValue(AdditionalParamKeys.KeySubstrateId, out string substrateId))
                            {
                                result = EN_MESSAGE_RESULT.NG;
                                messageContentToSend[ResultKeys.KeyResult] = result.ToString();
                            }

                            if (false == additionalParams.TryGetValue(AdditionalParamKeys.KeyNameOfEq, out string nameOfEq))
                            {
                                result = EN_MESSAGE_RESULT.NG;
                                messageContentToSend[ResultKeys.KeyResult] = result.ToString();
                            }

                            if (result.Equals(EN_MESSAGE_RESULT.NG))
                            {
                                resultCountRow = "0";
                                resultCountCol = "0";
                                resultAngle = "0";
                                resultQty = "0";
                                resultMapData = string.Empty;
                            }

                            messageContentToSend[RequestDownloadMapFileKeys.KeySubstrateName] = resultSubstrateId;
                            messageContentToSend[RequestDownloadMapFileKeys.KeyCountRow] = resultCountRow;
                            messageContentToSend[RequestDownloadMapFileKeys.KeyCountCol] = resultCountCol;
                            messageContentToSend[RequestDownloadMapFileKeys.KeyWaferAngle] = resultAngle;
                            messageContentToSend[RequestDownloadMapFileKeys.KeyChipQty] = resultQty;
                            messageContentToSend[RequestDownloadMapFileKeys.KeyMapData] = resultMapData;

                            if (FindSubstrateByNameOrRingIdAtProcessModule(substrateId, substrateId, out substrate, out _))
                            {
                                SetSubstrateAttributes(substrate,
                                    resultSubstrateId,
                                    resultAngle,
                                    resultCountRow,
                                    resultCountCol,
                                    resultQty,
                                    resultRefX,
                                    resultRefY,
                                    resultStartX,
                                    resultStartY,
                                    resultMapData);

                                _funcToSendClientMessage(nameOfEq, MessagesToSend.ResponseDownloadMapFile.ToString(),
                                         string.Empty, string.Empty,
                                         messageContentToSend.Keys.ToArray(), messageContentToSend.Values.ToArray(),
                                         result, true);

                                if (UseCoreMapHandlingOnly || result == EN_MESSAGE_RESULT.NG)
                                    return;
                            }
                            else
                            {
                                // 2025.07.16. jhlim [MOD] 자재 정보가 없는 경우, GEM이 꺼져있으면 다운받은 맵을 넘긴다.
                                var useSecsGem = _recipe.GetValue(EN_RECIPE_TYPE.COMMON, PARAM_COMMON.UseSecsGem.ToString(), true);
                                if (useSecsGem)
                                //if (false == UseCoreMapHandlingOnly)
                                {
                                    // Gem이 켜져 있으면 알람
                                    _funcToSendClientMessage(nameOfEq, MessagesToSend.ResponseDownloadMapFile.ToString(),
                                        string.Empty, string.Empty,
                                        messageContentToSend.Keys.ToArray(), messageContentToSend.Values.ToArray(),
                                        EN_MESSAGE_RESULT.NG, true);

                                    // TODO : 알람 발생 필요
                                }
                                else
                                {
                                    // Gem이 꺼져있으면 다운받은 맵을 전달
                                    _funcToSendClientMessage(nameOfEq, MessagesToSend.ResponseDownloadMapFile.ToString(),
                                            string.Empty, string.Empty,
                                            messageContentToSend.Keys.ToArray(), messageContentToSend.Values.ToArray(),
                                            result, true);
                                }
                                // 2025.07.16. jhlim [END]

                                // 2024.12.31. jhlim [ADD] NG 시 리턴 누락
                                return;
                            }

                            #region
                            if (false == additionalParams.TryGetValue(AdditionalParamKeys.KeyRingId, out string ringId))
                                return;

                            if (false == additionalParams.TryGetValue(AdditionalParamKeys.KeyUserId, out string userId))
                                return;

                            // Work_start 이후 발생하도록 수정 필요 -> ResponseDownloadMapFile 후 WaferSplitEvent 발생하도록 수정 필요
                            //int portId = substrate.SourcePortId;
                            //if (false == _carrierServer.HasCarrier(portId))
                            //    return;

                            string isLastString = substrate.GetAttribute(PWA500SubstrateAttributes.IsLastSubstrate);
                            bool.TryParse(isLastString, out bool isLast);
                            //bool isLast = _substrateManager.IsLastSubstrateAtLoadPort(portId, substrateId);
                            ExecuteScenarioToSplitWafer(nameOfEq, substrate.Name, ringId, userId, isLast);
                            #endregion
                        }

                        #endregion
                    }
                    break;
                case EN_SCENARIO.SCENARIO_WORK_END:
                    {
                        if (isManual)
                            return;

                        #region
                        Dictionary<string, string> messageContentToSend = new Dictionary<string, string>();
                        messageContentToSend[ResultKeys.KeyResult] = result.ToString();
                        if (result.Equals(EN_MESSAGE_RESULT.OK))
                        {
                            messageContentToSend[ResultKeys.KeyDescription] = string.Empty;
                        }
                        else
                        {
                            messageContentToSend[ResultKeys.KeyDescription] = "Gem Error";
                        }

                        if (additionalParams.TryGetValue(AdditionalParamKeys.KeyNameOfEq, out string nameOfEq))
                        {
                            _funcToSendClientMessage(nameOfEq, MessagesToSend.ResponseUploadCoreFile.ToString(),
                                string.Empty, string.Empty,
                                messageContentToSend.Keys.ToArray(), messageContentToSend.Values.ToArray(),
                                result, true);
                        }
                        #endregion

                        if (result.Equals(EN_MESSAGE_RESULT.NG))
                        {
                            SetScenarioError(typeOfScenario);
                            //FrameOfSystem3.Task.TaskOperator.GetInstance().SetOperation(RunningMain_.OPERATION_EQUIPMENT.STOP);
                            return;
                        }

                        // 2024.08.18 : [START] 코어맵 핸들링만 사용하는 경우 이후 시나리오를 무시한다.
                        if (UseCoreMapHandlingOnly)
                            return;
                        // [END]

                        #region
                        // Track Out
                        if (false == additionalParams.TryGetValue(AdditionalParamKeys.KeySubstrateId, out string substrateId))
                            return;
                        if (false == additionalParams.TryGetValue(AdditionalParamKeys.KeyChipQty, out string qty))
                            return;
                        if (false == int.TryParse(qty, out int chipQty))
                            return;
                        if (false == additionalParams.TryGetValue(AdditionalParamKeys.KeyUserId, out string userId))
                            return;
                        #region

                        // Process End
                        if (FindSubstrateByNameOrRingIdAtProcessModule(substrateId, substrateId, out var substrate, out _))
                        {
                            int portId = substrate.SourcePortId;
                            string isLastString = substrate.GetAttribute(PWA500SubstrateAttributes.IsLastSubstrate);
                            bool.TryParse(isLastString, out bool isLast);
                            //if (isLast)
                            //{
                            //    var scenarioParam = new Dictionary<string, string>
                            //    {
                            //        [EESKeys.KeyCarrierId] = _carrierServer.GetCarrierId(portId),
                            //        [EESKeys.KeyPortId] = GetPortName(portId),
                            //        [EESKeys.KeyLotId] = substrate.LotId,
                            //        [EESKeys.KeyPartId] = substrate.GetAttribute(PWA500SubstrateAttributes.PartId),
                            //        [EESKeys.KeyParamRecipeId] = GetRecipeId(),
                            //        [EESKeys.KeyOperatorId] = "AUTO"
                            //    };

                            //    _actionToEnqueueScenarioAsync(EN_SCENARIO.SCENARIO_PROCESS_END, scenarioParam, null);
                            //}

                            string carrierId = _carrierServer.GetCarrierId(portId);
                            _lotHistoryLog.WriteSubstrateHistoryForWorkEnd(portId, carrierId, substrateId, qty);

                            // 2025.02.04. jhlim [ADD] 트랙아웃 이미 진행되었는지 검사
                            string isTrackoutCompleted = substrate.GetAttribute(PWA500SubstrateAttributes.IsTrackOutCompleted);
                            if (isTrackoutCompleted.Equals(bool.TrueString))
                            {
                                // 문자열이 True면 트랙아웃 패스
                                return;
                            }
                            // 2025.02.04. jhlim [END]
                        }
                        else
                        {

                        }
                        #endregion

                        if (chipQty <= 0)
                            return;

                        if (UseTrackOutCore)
                        {
                            if (false == ExecuteScenarioToTrackOut(substrate.UniqueKey, substrateId, chipQty, userId, true))
                            {
                                SetScenarioError(typeOfScenario);
                                return;
                            }
                        }
                        #endregion
                    }
                    break;
                case EN_SCENARIO.SCENARIO_REQ_CORE_WAFER_TRACK_OUT:
                    {
                        if (additionalParams == null ||
                            false == additionalParams.TryGetValue(AdditionalParamKeys.KeySubstrateId, out string substrateId))
                            return;

                        if (FindSubstrateByNameOrRingIdAtProcessModule(substrateId, substrateId, out var substrate, out _))
                        {
                            int portId = substrate.SourcePortId;
                            string lotId = substrate.LotId;

                            string carrierId = substrate.SourceCarrierId;
                            string chipQty = substrate.GetAttribute(PWA500SubstrateAttributes.ChipQty);
                            var isLast = substrate.GetAttribute(PWA500SubstrateAttributes.IsLastSubstrate);
                            _lotHistoryLog.WriteSubstrateHistoryForTrackOut(portId, carrierId, substrateId, lotId, chipQty, isLast.Equals(bool.TrueString));

                            // 2025.02.04. jhlim [ADD] 트랙아웃 진행 했다고 속성을 설정한다.
                            _substrateManager.SetAttributeByKey(substrate.UniqueKey, PWA500SubstrateAttributes.IsTrackOutCompleted, bool.TrueString);
                            // 2025.02.04. jhlim [END]
                        }
                    }
                    break;

                case EN_SCENARIO.SCENARIO_REQ_CORE_WAFER_SPLIT:
                case EN_SCENARIO.SCENARIO_REQ_CORE_WAFER_SPLIT_LAST:
                    {
                        if (result.Equals(EN_MESSAGE_RESULT.NG))
                        {
                            SetScenarioError(typeOfScenario);
                            //FrameOfSystem3.Task.TaskOperator.GetInstance().SetOperation(RunningMain_.OPERATION_EQUIPMENT.STOP);
                            return;
                        }

                        if (false == scenarioParams.TryGetValue(AssignSubstrateLotIdKeys.KeyParamWaferId, out string substrateId))
                            return;

                        if (false == FindSubstrateByNameOrRingIdAtProcessModule(substrateId, substrateId, out var substrate, out _) || substrate == null)
                            return;

                        string targetLotId = string.Empty, receivedPartId = string.Empty;
                        int portId = substrate.SourcePortId;
                        bool partIdError = false;
                        if (typeOfScenario.Equals(EN_SCENARIO.SCENARIO_REQ_CORE_WAFER_SPLIT))
                        {
                            if (false == resultOfScenario.TryGetValue(AssignSubstrateLotIdKeys.KeyResultLotId, out targetLotId))
                                return;

                            if (false == resultOfScenario.TryGetValue(AssignSubstrateLotIdKeys.KeyResultPartId, out receivedPartId))
                            {
                                partIdError = true;
                                //SetScenarioError(typeOfScenario, "Does not have Part Id Info");
                                //return;
                            }
                        }
                        else
                        {
                            if (false == _carrierServer.HasCarrier(portId))
                                return;

                            targetLotId = _carrierServer.GetCarrierLotId(portId);
                            receivedPartId = substrate.GetAttribute(PWA500SubstrateAttributes.PartId);
                            if (string.IsNullOrEmpty(receivedPartId))
                            {
                                partIdError = true;
                            }
                        }

                        string oldLotId = substrate.LotId;
                        string carrierId = _carrierServer.GetCarrierId(portId);
                        _lotHistoryLog.WriteSubstrateHistoryForWaferSplit(portId, carrierId, substrateId, oldLotId, targetLotId, typeOfScenario.Equals(EN_SCENARIO.SCENARIO_REQ_CORE_WAFER_SPLIT_LAST));

                        //substrate.SetLotId(targetLotId);
                        _substrateManager.SetLotIdByKey(substrate.UniqueKey, targetLotId);
                        _substrateManager.SaveDataByKey(substrate.UniqueKey);

                        if (false == isManual && additionalParams != null)
                        {
                            if (additionalParams.TryGetValue(AdditionalParamKeys.KeyNameOfEq, out string nameOfEq))
                            {
                                if (UseComparePartId)
                                {
                                    var partId = substrate.GetAttribute(PWA500SubstrateAttributes.PartId);
                                    if (false == receivedPartId.Equals(partId))
                                    {
                                        _funcToSendClientMessage(nameOfEq, MessagesToSend.RequestStop.ToString(), string.Empty, string.Empty, new string[] { }, new string[] { }, EN_MESSAGE_RESULT.OK, false);
                                        SetScenarioError(typeOfScenario, string.Format("Different Part Info -> Prev:{0}, New:{1}", partId, receivedPartId));
                                        return;
                                    }
                                }

                                var messageContentToSend = new Dictionary<string, string>();
                                messageContentToSend[AssignSubstrateLotIdKeys.KeySubstrateName] = substrateId;
                                messageContentToSend[AssignSubstrateLotIdKeys.KeyLotId] = targetLotId;

                                _funcToSendClientMessage(nameOfEq, MessagesToSend.RequestAssignLotId.ToString(),
                                    string.Empty, string.Empty,
                                    messageContentToSend.Keys.ToArray(), messageContentToSend.Values.ToArray(),
                                    result, true);
                            }
                        }

                        if (partIdError && UseComparePartId)
                        {
                            SetScenarioError(typeOfScenario, "Does not have Part Id Info");
                            return;
                        }
                    }
                    break;

                case EN_SCENARIO.SCENARIO_CORE_WAFER_DETACH_START:
                case EN_SCENARIO.SCENARIO_CORE_WAFER_DETACH_END:
                case EN_SCENARIO.SCENARIO_BIN_SORTING_START_1:
                case EN_SCENARIO.SCENARIO_BIN_SORTING_START_2:
                case EN_SCENARIO.SCENARIO_BIN_SORTING_START_3:
                    {
                        #region
                        if (false == additionalParams.TryGetValue(AdditionalParamKeys.KeyNameOfEq, out string nameOfEq))
                            return;

                        if (false == additionalParams.TryGetValue(AdditionalParamKeys.KeyMessageNameToSend, out string messageNameToSend))
                            return;

                        ExecuteToSendSimpleResultToClient(result, messageNameToSend, nameOfEq);
                        #endregion
                    }
                    break;

                case EN_SCENARIO.SCENARIO_REQ_CORE_CHIP_SPLIT_FIRST:
                case EN_SCENARIO.SCENARIO_REQ_CORE_CHIP_SPLIT:
                case EN_SCENARIO.SCENARIO_REQ_CORE_CHIP_FULL_SPLIT_FIRST:
                case EN_SCENARIO.SCENARIO_REQ_CORE_CHIP_FULL_SPLIT:
                    {
                        if (result.Equals(EN_MESSAGE_RESULT.NG))
                        {
                            SetScenarioError(typeOfScenario);
                            //FrameOfSystem3.Task.TaskOperator.GetInstance().SetOperation(RunningMain_.OPERATION_EQUIPMENT.STOP);
                            return;
                        }

                        if (false == isManual)
                        {
                            #region
                            // 스플릿 이벤트 전송 후 리스폰스 전송
                            Dictionary<string, string> messageContentToSend = new Dictionary<string, string>();
                            messageContentToSend[ResultKeys.KeyResult] = result.ToString();
                            if (result.Equals(EN_MESSAGE_RESULT.OK))
                            {
                                messageContentToSend[ResultKeys.KeyDescription] = string.Empty;
                            }
                            else
                            {
                                messageContentToSend[ResultKeys.KeyDescription] = "Gem Error";
                            }

                            if (additionalParams.TryGetValue(AdditionalParamKeys.KeyNameOfEq, out string nameOfEq))
                            {
                                ExecuteToSendSimpleResultToClient(result, MessagesToSend.ResponseSplitCoreChip.ToString(), nameOfEq);
                            }
                            else
                                return;

                            if (result.Equals(EN_MESSAGE_RESULT.NG))
                                return;
                            #endregion
                        }
                        #region

                        // LotId 할당된 것을 설정
                        if (false == resultOfScenario.TryGetValue(SplitCoreChipKeys.KeyResultLotId, out string lotId))
                        {
                            return;
                        }
                        if (false == scenarioParams.TryGetValue(SplitCoreChipKeys.KeyParamSplitWaferId, out string coreSubstrateId))
                        {
                            return;
                        }
                        if (false == scenarioParams.TryGetValue(SplitCoreChipKeys.KeyParamRingFrameId, out string substrateId))
                        {
                            return;
                        }
                        if (false == scenarioParams.TryGetValue(SplitCoreChipKeys.KeyParamBinType, out string binType))
                        {
                            return;
                        }

                        if (FindSubstrateByNameOrRingIdAtProcessModule(substrateId, substrateId, out var binSubstrate, out _))
                        {
                            string chipQtyToSplit;
                            if (false == scenarioParams.TryGetValue(SplitCoreChipKeys.KeyParamSplitChipQty, out chipQtyToSplit))
                                chipQtyToSplit = "0";

                            string historyForBin = $"{lotId}:{coreSubstrateId}:{chipQtyToSplit}";
                            if (typeOfScenario.Equals(EN_SCENARIO.SCENARIO_REQ_CORE_CHIP_SPLIT_FIRST) ||
                                typeOfScenario.Equals(EN_SCENARIO.SCENARIO_REQ_CORE_CHIP_FULL_SPLIT_FIRST))
                            {
                                //binSubstrate.SetLotId(lotId);
                                _substrateManager.SetLotIdByKey(binSubstrate.UniqueKey, lotId);
                                _substrateManager.SetAttributeByKey(binSubstrate.UniqueKey, PWA500SubstrateAttributes.ChipQty, chipQtyToSplit);
                                _substrateManager.SetAttributeByKey(binSubstrate.UniqueKey, PWA500SubstrateAttributes.SplittedHistory, historyForBin);

                                _substrateManager.SaveDataByKey(binSubstrate.UniqueKey);

                            }
                            else
                            {
                                _substrateManager.SetAttributeByKey(binSubstrate.UniqueKey, PWA500SubstrateAttributes.SplittedLotId, lotId);

                                var prevHistory = _substrateManager.GetAttributeByKey(binSubstrate.UniqueKey, PWA500SubstrateAttributes.SplittedHistory);
                                _substrateManager.SetAttributeByKey(binSubstrate.UniqueKey, PWA500SubstrateAttributes.SplittedHistory, $"{prevHistory},{historyForBin}");

                                _substrateManager.SaveDataByKey(binSubstrate.UniqueKey);

                                // 기존 값을 읽어와 받은 데이터를 더한다.
                                //string qtyByString = binSubstrate.GetAttribute(PWA500BINSubstrateAttributes.ChipQty);
                                //if (false == int.TryParse(qtyByString, out int chipQty))
                                //    chipQty = 0;
                                //if (false == int.TryParse(chipQtyToIncreaseByString, out int chipQtyToIncrease))
                                //    chipQtyToIncrease = 0;

                                //int totalQty = chipQty + chipQtyToIncrease;
                                //binSubstrate.SetAttribute(PWA500BINSubstrateAttributes.ChipQty, totalQty.ToString());

                                string lotIdForParent = binSubstrate.LotId;
                                // 토탈이 아닌 증가되는 양만 머지한다. 여기서 수량이 계속 증가되는듯..
                                ExecuteScenarioToChipMerge(lotIdForParent, lotId, coreSubstrateId, substrateId, binType, chipQtyToSplit/*totalQty.ToString()*/);
                            }

                            if (FindSubstrateByNameOrRingIdAtProcessModule(coreSubstrateId, coreSubstrateId, out var coreSubstrate, out _))
                            {
                                int corePortId = coreSubstrate.SourcePortId;
                                int binPortId = binSubstrate.SourcePortId;

                                bool splitFirst = typeOfScenario.Equals(EN_SCENARIO.SCENARIO_REQ_CORE_CHIP_SPLIT_FIRST) ||
                                    typeOfScenario.Equals(EN_SCENARIO.SCENARIO_REQ_CORE_CHIP_FULL_SPLIT_FIRST);

                                bool splitFully = typeOfScenario.Equals(EN_SCENARIO.SCENARIO_REQ_CORE_CHIP_FULL_SPLIT_FIRST) ||
                                    typeOfScenario.Equals(EN_SCENARIO.SCENARIO_REQ_CORE_CHIP_FULL_SPLIT);

                                string carrierId = _carrierServer.GetCarrierId(corePortId);

                                string historyForCore = $"{lotId}:{substrateId}:{chipQtyToSplit}";
                                var prevHistory = _substrateManager.GetAttributeByKey(coreSubstrate.UniqueKey, PWA500SubstrateAttributes.SplittedHistory);
                                if (string.IsNullOrWhiteSpace(prevHistory))
                                {
                                    _substrateManager.SetAttributeByKey(coreSubstrate.UniqueKey, PWA500SubstrateAttributes.SplittedHistory, historyForCore);
                                }
                                else
                                {
                                    _substrateManager.SetAttributeByKey(coreSubstrate.UniqueKey, PWA500SubstrateAttributes.SplittedHistory, $"{prevHistory},{historyForCore}");
                                }

                                _substrateManager.SaveDataByKey(coreSubstrate.UniqueKey);

                                _lotHistoryLog.WriteSubstrateHistoryForChipSplit(corePortId, carrierId, coreSubstrateId, binPortId, substrateId, chipQtyToSplit, binType, lotId, splitFirst, splitFully);
                            }
                        }
                        #endregion
                    }
                    break;

                case EN_SCENARIO.SCENARIO_REQ_CORE_CHIP_MERGE:
                    {
                        if (result.Equals(EN_MESSAGE_RESULT.NG))
                        {
                            SetScenarioError(typeOfScenario);
                            //FrameOfSystem3.Task.TaskOperator.GetInstance().SetOperation(RunningMain_.OPERATION_EQUIPMENT.STOP);
                        }
                    }
                    break;

                case EN_SCENARIO.SCENARIO_BIN_WAFER_ID_READ:
                    {
                        #region
                        if (false == isManual)
                        {
                            if (false == additionalParams.TryGetValue(AdditionalParamKeys.KeyNameOfEq, out string nameOfEq))
                                return;

                            if (false == additionalParams.TryGetValue(AdditionalParamKeys.KeyMessageNameToSend, out string messageNameToSend))
                                return;

                            ExecuteToSendSimpleResultToClient(result, messageNameToSend, nameOfEq);
                        }
                        #endregion
                    }
                    break;

                case EN_SCENARIO.SCENARIO_REQ_CORE_WAFER_ID:
                    {
                        #region
                        if (false == additionalParams.TryGetValue(AdditionalParamKeys.KeyNameOfEq, out string nameOfEq))
                            return;

                        if (false == additionalParams.TryGetValue(AdditionalParamKeys.KeyRingId, out string ringId))
                            return;

                        if (false == additionalParams.TryGetValue(AdditionalParamKeys.KeySubstrateId, out string substrateName))
                            return;

                        if (false == additionalParams.TryGetValue(AdditionalParamKeys.KeyMessageNameToSend, out string messageNameToSend))
                            return;

                        if (result.Equals(EN_MESSAGE_RESULT.NG))
                        {
                            SetScenarioError(typeOfScenario);
                            ExecuteToSendSimpleResultToClient(EN_MESSAGE_RESULT.NG, messageNameToSend, nameOfEq, "Does not have ring id or substrate name");
                        }
                        else
                        {
                            if (false == resultOfScenario.TryGetValue(AssignSubstrateIdKeys.KeyResultSubstrateId, out string newSubstrateName))
                            {
                                SetScenarioError(typeOfScenario);
                                ExecuteToSendSimpleResultToClient(EN_MESSAGE_RESULT.NG, messageNameToSend, nameOfEq, "SECS/GEM Scenario Error!");
                            }
                            else
                            {
                                string pmName = _processGroup.GetProcessModuleName(ProcessModuleIndex);
                                List<Substrate> substrates = new List<Substrate>();
                                if (_substrateManager.GetSubstratesAtProcessModule(pmName, ref substrates))
                                {
                                    for (int i = 0; i < substrates.Count; ++i)
                                    {
                                        var name = substrates[i].Name;
                                        if (name.Equals(ringId) || name.Equals(substrateName))
                                        {
                                            _substrateManager.SetAttributeByKey(substrates[i].UniqueKey, PWA500SubstrateAttributes.RingId, substrateName);
                                            _substrateManager.SetNameByKey(substrates[i].UniqueKey, newSubstrateName);
                                            _substrateManager.SaveDataByKey(substrates[i].UniqueKey);

                                            Dictionary<string, string> messageContentToSend = new Dictionary<string, string>
                                            {
                                                [AssignSubstrateIdKeys.KeySubstrateName/*"SubstrateName"*/] = newSubstrateName,
                                                [AssignSubstrateIdKeys.KeyResultRingId] = substrateName
                                            };

                                            _funcToSendClientMessage(nameOfEq, messageNameToSend,
                                                string.Empty, string.Empty,
                                                messageContentToSend.Keys.ToArray(), messageContentToSend.Values.ToArray(),
                                                result, true);

                                            return;
                                        }
                                    }
                                }

                                // 통신이 꺼져있고, 자재 정보를 못찾으면
                                var useSecsGem = _recipe.GetValue(EN_RECIPE_TYPE.COMMON, PARAM_COMMON.UseSecsGem.ToString(), true);
                                if (false == useSecsGem)
                                {
                                    Dictionary<string, string> messageContentToSend = new Dictionary<string, string>
                                    {
                                        [AssignSubstrateIdKeys.KeySubstrateName/*"SubstrateName"*/] = newSubstrateName,
                                        [AssignSubstrateIdKeys.KeyResultRingId] = substrateName
                                    };

                                    _funcToSendClientMessage(nameOfEq, messageNameToSend,
                                        string.Empty, string.Empty,
                                        messageContentToSend.Keys.ToArray(), messageContentToSend.Values.ToArray(),
                                        result, true);
                                    
                                    return;
                                }
                            }
                        }

                        ExecuteToSendSimpleResultToClient(EN_MESSAGE_RESULT.NG, messageNameToSend, nameOfEq, "Does not have ring id or substrate name");
                        #endregion
                    }
                    break;

                case EN_SCENARIO.SCENARIO_BIN_WORK_END:
                    {
                        #region
                        // Track Out
                        //if (false == additionalParams.TryGetValue(AdditionalParamKeys.KeySubstrateId, out string substrateId))
                        //    return;
                        //if (false == additionalParams.TryGetValue(AdditionalParamKeys.KeyChipQty, out string qty))
                        //    return;
                        //if (false == int.TryParse(qty, out int chipQty))
                        //    return;

                        //ExecuteScenarioToTrackOut(substrateId, chipQty, "AUTO", false);
                        #endregion
                    }
                    break;
                case EN_SCENARIO.SCENARIO_REQ_BIN_WAFER_ID_ASSIGN:
                    {
                        // Robot에서 발생시키도록 시나리오 변경됨
                        //if (false == isManual)
                        //    return;

                        //#region
                        //if (false == scenarioParams.TryGetValue(AssignSubstrateIdKeys.KeyParamRingFrameId, out string ringId))
                        //    return;

                        //Substrate binSubstrate = new Substrate();
                        //if (false == _substrateManager.GetSubstrateByName(ringId, ref binSubstrate))
                        //    return;

                        //if (false == resultOfScenario.TryGetValue(AssignSubstrateIdKeys.KeyResultSubstrateId, out string newSubstrateId))
                        //{
                        //    return;
                        //}
                        //binSubstrate.SetName(newSubstrateId);
                        //#endregion
                    }
                    break;
                case EN_SCENARIO.SCENARIO_BIN_SORTING_END_1:
                case EN_SCENARIO.SCENARIO_BIN_SORTING_END_2:
                case EN_SCENARIO.SCENARIO_BIN_SORTING_END_3:
                    {
                        if (additionalParams == null)
                            return;
                        #region
                        if (false == additionalParams.TryGetValue(AdditionalParamKeys.KeyRingId, out string ringId))
                        {
                            result = EN_MESSAGE_RESULT.NG;
                        }

                        if (false == additionalParams.TryGetValue(AdditionalParamKeys.KeySubstrateType, out string subType))
                        {
                            result = EN_MESSAGE_RESULT.NG;
                        }
                        if (false == Enum.TryParse(subType, out SubstrateType substrateType))
                        {
                            result = EN_MESSAGE_RESULT.NG;
                        }

                        if (false == additionalParams.TryGetValue(AdditionalParamKeys.KeyNameOfEq, out string nameOfEq))
                        {
                            result = EN_MESSAGE_RESULT.NG;
                        }

                        int chipQty = 0;
                        if (false == additionalParams.TryGetValue(AdditionalParamKeys.KeyChipQty, out string qty) ||
                            false == int.TryParse(qty, out chipQty))
                        {
                            result = EN_MESSAGE_RESULT.NG;
                        }

                        string description = string.Empty;
                        if (result == EN_MESSAGE_RESULT.NG)
                        {
                            description = "Gem Error";
                        }

                        Dictionary<string, string> messageContentToSend = new Dictionary<string, string>
                        {
                            [ResultKeys.KeyResult] = result.ToString(),
                            [ResultKeys.KeyDescription] = description,
                        };

                        _funcToSendClientMessage(nameOfEq, MessagesToSend.ResponseFinishSorting.ToString(),
                            string.Empty, string.Empty,
                            messageContentToSend.Keys.ToArray(), messageContentToSend.Values.ToArray(),
                            result, true);
                        //if (result.Equals(EN_MESSAGE_RESULT.NG))
                        //{

                        //    Dictionary<string, string> messageContentToSend = new Dictionary<string, string>
                        //    {
                        //        [ResultKeys.KeyResult] = EN_MESSAGE_RESULT.NG.ToString(),
                        //        [ResultKeys.KeyDescription] = "Gem Error",
                        //    };

                        //    SendClientToClientMessage(nameOfEq, MessagesToSend.ResponseFinishSorting.ToString(),
                        //        string.Empty, string.Empty,
                        //        messageContentToSend.Keys.ToArray(), messageContentToSend.Values.ToArray(),
                        //        EN_MESSAGE_RESULT.NG, true);
                        //}
                        //else
                        //{
                        //    ExecuteScenarioToAssignSubstrateId(nameOfEq, ringId, substrateType);
                        //}
                        #endregion
                    }
                    break;

                default:
                    break;
            }
        }
        
        private bool ExecuteToSendSimpleResultToClient(EN_MESSAGE_RESULT result, string messageNameToSend, string nameOfEq, string description = "")
        {
            if (_funcToSendClientMessage == null)
                return false;

            if (messageNameToSend == null || string.IsNullOrEmpty(messageNameToSend))
                return true;

            Dictionary<string, string> messageContentToSend = new Dictionary<string, string>
            {
                [ResultKeys.KeyResult] = result.ToString(),
                [ResultKeys.KeyDescription] = description
            };

            return _funcToSendClientMessage(nameOfEq, messageNameToSend.ToString(),
                        string.Empty, string.Empty,
                        messageContentToSend.Keys.ToArray(),
                        messageContentToSend.Values.ToArray(),
                        result, true);
        }
        private bool ExecuteScenarioToChipMerge(string lotId, string lotIdToMerge, string coreWaferId, string binRingId, string binType, string chipQty)
        {
            if (_actionToEnqueueScenarioAsync == null)
                return false;

            Dictionary<string, string> scenarioParam = new Dictionary<string, string>();
            scenarioParam[SplitCoreChipKeys.KeyParamLotId] = lotId;
            scenarioParam[SplitCoreChipKeys.KeyParamSplitLotId] = lotIdToMerge;
            scenarioParam[SplitCoreChipKeys.KeyParamSplitWaferId] = coreWaferId;
            scenarioParam[SplitCoreChipKeys.KeyParamRingFrameId] = binRingId;
            scenarioParam[SplitCoreChipKeys.KeyParamBinType] = binType;
            scenarioParam[SplitCoreChipKeys.KeyParamSplitChipQty] = chipQty;

            _actionToEnqueueScenarioAsync(EN_SCENARIO.SCENARIO_REQ_CORE_CHIP_MERGE, scenarioParam, null);

            return true;
        }
        private bool ExecuteScenarioToSplitWafer(string nameOfEq, string substrateId, string ringId, string userId, bool isLast)
        {
            if (_actionToEnqueueScenarioAsync == null)
                return false;

            if (false == FindSubstrateByNameOrRingIdAtProcessModule(substrateId, substrateId, out var substrate, out _) || substrate == null)
                return false;

            var scenarioParam = new Dictionary<string, string>
            {
                [AssignSubstrateLotIdKeys.KeyParamLotId] = substrate.LotId,
                [AssignSubstrateLotIdKeys.KeyParamWaferId] = substrateId,
                [AssignSubstrateLotIdKeys.KeyParamPartId] = substrate.GetAttribute(PWA500SubstrateAttributes.PartId),
                [AssignSubstrateLotIdKeys.KeyParamRecipeId] = GetRecipeId(),
                [AssignSubstrateLotIdKeys.KeyParamSlotId] = (substrate.SourceSlot).ToString(),
                [AssignSubstrateLotIdKeys.KeyParamOperatorId] = userId
            };

            Dictionary<string, string> additionalParams = new Dictionary<string, string>
            {
                [AdditionalParamKeys.KeyNameOfEq] = nameOfEq,
                [AdditionalParamKeys.KeySubstrateId] = substrateId,
                [AdditionalParamKeys.KeyRingId] = ringId
            };

            if (false == _recipe.GetValue(EN_RECIPE_TYPE.COMMON, PARAM_COMMON.UseSecsGem.ToString(), true))
            {
                var messageContentToSend = new Dictionary<string, string>();
                messageContentToSend[AssignSubstrateLotIdKeys.KeySubstrateName] = substrateId;
                messageContentToSend[AssignSubstrateLotIdKeys.KeyLotId] = substrate.LotId;

                _funcToSendClientMessage(nameOfEq, MessagesToSend.RequestAssignLotId.ToString(),
                    string.Empty, string.Empty,
                    messageContentToSend.Keys.ToArray(), messageContentToSend.Values.ToArray(), EN_MESSAGE_RESULT.OK
                    , true);
                
                return true;
            }

            EN_SCENARIO scenario;
            if (false == isLast)
            {
                scenario = EN_SCENARIO.SCENARIO_REQ_CORE_WAFER_SPLIT;
            }
            else
            {
                scenario = EN_SCENARIO.SCENARIO_REQ_CORE_WAFER_SPLIT_LAST;
            }

            _actionToEnqueueScenarioAsync(scenario, scenarioParam, additionalParams);
            return true;
        }
        private bool ExecuteScenarioToTrackOut(string substrateKey, string substrateId, int chipQty, string userId, bool isCore)
        {
            if (_actionToEnqueueScenarioAsync == null)
                return false;

            EN_SCENARIO scenario;
            if (false == isCore)
            {
                scenario = EN_SCENARIO.SCENARIO_REQ_BIN_WAFER_TRACK_OUT;
            }
            else
            {
                scenario = EN_SCENARIO.SCENARIO_REQ_CORE_WAFER_TRACK_OUT;
            }
            var scenarioParams = MakeScenarioParamToTrackOut(substrateKey, userId, isCore);
            if (scenarioParams == null)
                return false;

            Dictionary<string, string> additionalParams = new Dictionary<string, string>();
            additionalParams[AdditionalParamKeys.KeySubstrateId] = substrateId;

            _actionToEnqueueScenarioAsync(scenario, scenarioParams, additionalParams);

            return true;
        }
        public void SetSubstrateAttributes(Substrate substrate, string substrateId, string angle, string countRow, string countCol, string qty, string referenceX, string referenceY, string startingX, string startingY, string mapData)
        {
            //substrate.SetName(substrateId);
            _substrateManager.SetNameByKey(substrate.UniqueKey, substrateId);
            _substrateManager.SetAttributeByKey(substrate.UniqueKey, PWA500SubstrateAttributes.Angle, angle);
            _substrateManager.SetAttributeByKey(substrate.UniqueKey, PWA500SubstrateAttributes.CountX,  countRow);
            _substrateManager.SetAttributeByKey(substrate.UniqueKey, PWA500SubstrateAttributes.CountY,  countCol);
            _substrateManager.SetAttributeByKey(substrate.UniqueKey, PWA500SubstrateAttributes.ChipQty,  qty);
            _substrateManager.SetAttributeByKey(substrate.UniqueKey, PWA500SubstrateAttributes.RefPositionX,  referenceX);
            _substrateManager.SetAttributeByKey(substrate.UniqueKey, PWA500SubstrateAttributes.RefPositionY,  referenceY);
            _substrateManager.SetAttributeByKey(substrate.UniqueKey, PWA500SubstrateAttributes.StartingPositionX,  startingX);
            _substrateManager.SetAttributeByKey(substrate.UniqueKey, PWA500SubstrateAttributes.StartingPositionY,  startingY);
            _substrateManager.SetAttributeByKey(substrate.UniqueKey, PWA500SubstrateAttributes.MapData, mapData);

            _substrateManager.SaveDataByKey(substrate.UniqueKey);
        }
        public string GetRecipeId()
        {
            return _processGroup.GetRecipeId(ProcessModuleIndex);
        }
        public string GetPortName(int portId)
        {
            return string.Format("B{0}", portId);

            //return string.Format("{0}_B{1}", Work.AppConfigManager.Instance.MachineName, portId);
        }
        public string GetStepIdForBinWafer()
        {
            return _recipe.GetValue(EN_RECIPE_TYPE.EQUIPMENT, PARAM_EQUIPMENT.BinWaferStepId.ToString(), "P420");
        }
        public Dictionary<string, string> MakeParamToProcessing(int portId, Substrate substrate)
        {
            var recipe = GetRecipeId();
            if (string.IsNullOrWhiteSpace(recipe))
            {
                recipe = substrate.RecipeId;
            }

            var scenarioParam = new Dictionary<string, string>();
            scenarioParam[EESKeys.KeyCarrierId] = _carrierServer.GetCarrierId(portId);
            scenarioParam[EESKeys.KeyPortId] = GetPortName(portId);
            scenarioParam[EESKeys.KeyLotId] = substrate.LotId;
            scenarioParam[EESKeys.KeyPartId] = substrate.GetAttribute(PWA500SubstrateAttributes.PartId);
            scenarioParam[EESKeys.KeyParamRecipeId] = recipe;
            scenarioParam[EESKeys.KeyOperatorId] = "AUTO";

            return scenarioParam;
        }
        public Dictionary<string, string> MakeParamToEquipmentStatus()
        {
            var scenarioParams = new Dictionary<string, string>();

            int currentPort = -1;
            List<int> portIdForCore = new List<int>();
            // Core 기준으로 전송한다.
            for (int i = 0; i < _loadPortManager.Count; ++i)
            {
                if (false == _loadPortManager.IsLoadPortEnabled(i))
                    continue;

                var substrateType = GetSubstrateTypeByLoadPortIndex(i);
                switch (substrateType)
                {
                    case SubstrateType.Core:
                        {
                            int portId = _loadPortManager.GetLoadPortPortId(i);
                            if (_carrierServer.HasCarrier(portId))
                            {
                                portIdForCore.Add(portId);

                                var status = _carrierServer.GetCarrierAccessingStatus(portId);
                                switch (status)
                                {
                                    case CarrierAccessStates.InAccessed:
                                        {
                                            currentPort = portId;
                                        }
                                        break;

                                    default:
                                        break;
                                }
                            }
                        }
                        break;

                    default:
                        break;
                }

                if (currentPort > 0)
                {
                    break;
                }
            }

            string lotId = string.Empty, partId = string.Empty, stepSeq = string.Empty;
            if (currentPort < 0)
            {
                if (portIdForCore.Count > 0)
                {
                    currentPort = portIdForCore[0];
                }
            }

            if (currentPort > 0)
            {
                lotId = _carrierServer.GetCarrierLotId(currentPort);
                partId = _carrierServer.GetAttribute(currentPort, PWA500CarrierAttributes.KeyPartId);
                stepSeq = _carrierServer.GetAttribute(currentPort, PWA500CarrierAttributes.KeyStepSeq);
            }

            scenarioParams[ProcessModuleStatusChangedKeys.KeyParamLotId] = lotId;
            scenarioParams[ProcessModuleStatusChangedKeys.KeyParamPartId] = partId;
            scenarioParams[ProcessModuleStatusChangedKeys.KeyParamStepSeq] = stepSeq;

            return scenarioParams;
        }
        public Dictionary<string, string> MakeParamToOHTHandling(int portId, LoadPortLoadingMode loadingType, string lotId, EN_SCENARIO scenario)
        {
            var scenarioParams = new Dictionary<string, string>();
            string carrierType = loadingType == LoadPortLoadingMode.Foup ?
                OHTHandlingCarrierType.MAC.ToString() :
                OHTHandlingCarrierType.CASSETTE.ToString();

            scenarioParams[AMHSHandlingKeys.KeyParamPortId] = GetPortName(portId);
            // 2024.12.24. jhlim [MOD]
            scenarioParams[AMHSHandlingKeys.KeyParamLotId] = lotId;
            scenarioParams[AMHSHandlingKeys.KeyParamCarrierId] = _carrierServer.GetCarrierId(portId);
            // 2024.12.24. jhlim [END]
            scenarioParams[AMHSHandlingKeys.KeyParamCarrierType] = carrierType;

            if (scenario.Equals(EN_SCENARIO.SCENARIO_PORT_STATUS_LOAD_1) ||
                scenario.Equals(EN_SCENARIO.SCENARIO_PORT_STATUS_LOAD_2) ||
                scenario.Equals(EN_SCENARIO.SCENARIO_PORT_STATUS_LOAD_3) ||
                scenario.Equals(EN_SCENARIO.SCENARIO_PORT_STATUS_LOAD_4) ||
                scenario.Equals(EN_SCENARIO.SCENARIO_PORT_STATUS_LOAD_5) ||
                scenario.Equals(EN_SCENARIO.SCENARIO_PORT_STATUS_LOAD_6))
            {
                scenarioParams[AMHSHandlingKeys.KeyParamStatus] = OHTHandlingStatus.UNLOAD.ToString();
            }
            else if (scenario.Equals(EN_SCENARIO.SCENARIO_PORT_STATUS_UNLOAD_1) ||
                scenario.Equals(EN_SCENARIO.SCENARIO_PORT_STATUS_UNLOAD_2) ||
                scenario.Equals(EN_SCENARIO.SCENARIO_PORT_STATUS_UNLOAD_3) ||
                scenario.Equals(EN_SCENARIO.SCENARIO_PORT_STATUS_UNLOAD_4) ||
                scenario.Equals(EN_SCENARIO.SCENARIO_PORT_STATUS_UNLOAD_5) ||
                scenario.Equals(EN_SCENARIO.SCENARIO_PORT_STATUS_UNLOAD_6))
            {
                scenarioParams[AMHSHandlingKeys.KeyParamStatus] = OHTHandlingStatus.LOAD.ToString();
            }

            scenarioParams[AMHSHandlingKeys.KeyParamOperId] = "AUTO";

            return scenarioParams;
        }
        public Dictionary<string, string> MakeScenarioParamToRecipeDownload(Substrate substrate)
        {
            string lotId = substrate.LotId;
            string partId = substrate.GetAttribute(PWA500SubstrateAttributes.PartId);
            string stepSeq = substrate.GetAttribute(PWA500SubstrateAttributes.StepSeq);
            string recipeId = substrate.RecipeId;
            string lotType = substrate.GetAttribute(PWA500SubstrateAttributes.LotType);

            var scenarioParam = new Dictionary<string, string>
            {
                [RecipeHandlingKeys.KeyParamLotId] = lotId,
                [RecipeHandlingKeys.KeyParamRecipeId] = recipeId,
                [RecipeHandlingKeys.KeyParamPartId] = partId,
                [RecipeHandlingKeys.KeyParamStepSeq] = stepSeq,
                [RecipeHandlingKeys.KeyParamLotType] = lotType,
                [RecipeHandlingKeys.KeyUseCommunicationToPM] = bool.TrueString,
            };

            return scenarioParam;
        }
        public Dictionary<string, string> MakeScenarioParamToSendingAssignId(string newSubstrateId, string ringId)
        {
            Dictionary<string, string> scenarioParam = new Dictionary<string, string>
            {
                [AssignSubstrateIdKeys.KeySubstrateName] = newSubstrateId,
                [AssignSubstrateIdKeys.KeyRingId] = ringId
            };

            return scenarioParam;
        }
        public Dictionary<string, string> MakeScenarioParamToBinWorkEnd(string substrateKey, bool useEventHandling)
        {
            Dictionary<string, string> scenarioParams = new Dictionary<string, string>();
            string userId = "AUTO";
            if (_substrateManager.GetSubstrateByKey(substrateKey, out var substrate))
            {
                int portId = substrate.DestinationPortId;
                int slot = substrate.DestinationSlot;
                if (portId <= 0 || slot < 0)
                    return null;

                string lotId = substrate.LotId;
                string partId = substrate.GetAttribute(PWA500SubstrateAttributes.PartId);
                string recipeId = GetRecipeId();

                string chipQty = substrate.GetAttribute(PWA500SubstrateAttributes.ChipQty);
                string carrierId = _carrierServer.GetCarrierId(portId);

                scenarioParams[UploadCoreOrBinFileKeys.KeyParamCarrierId] = carrierId;
                scenarioParams[UploadCoreOrBinFileKeys.KeyParamPortId] = GetPortName(portId);
                scenarioParams[UploadCoreOrBinFileKeys.KeyParamLotId] = lotId;
                scenarioParams[UploadCoreOrBinFileKeys.KeyParamPartId] = partId;
                scenarioParams[UploadCoreOrBinFileKeys.KeyParamRecipeId] = recipeId;

                scenarioParams[UploadCoreOrBinFileKeys.KeyParamRecipeId] = recipeId;
                scenarioParams[UploadCoreOrBinFileKeys.KeyParamOperatorId] = userId;
                scenarioParams[UploadCoreOrBinFileKeys.KeyChipQty] = chipQty;
                scenarioParams[UploadCoreOrBinFileKeys.KeyUseEventHandling] = useEventHandling.ToString();

                return scenarioParams;
            }
            else
            {
                if (false == useEventHandling)
                {
                    scenarioParams[UploadCoreOrBinFileKeys.KeyParamCarrierId] = string.Empty;
                    scenarioParams[UploadCoreOrBinFileKeys.KeyParamPortId] = string.Empty;
                    scenarioParams[UploadCoreOrBinFileKeys.KeyParamLotId] = string.Empty;
                    scenarioParams[UploadCoreOrBinFileKeys.KeyParamPartId] = string.Empty;
                    scenarioParams[UploadCoreOrBinFileKeys.KeyParamRecipeId] = GetRecipeId();
                    scenarioParams[UploadCoreOrBinFileKeys.KeyParamOperatorId] = userId;
                    scenarioParams[UploadCoreOrBinFileKeys.KeyChipQty] = string.Empty;
                    scenarioParams[UploadCoreOrBinFileKeys.KeyUseEventHandling] = useEventHandling.ToString();

                    return scenarioParams;
                }
            }

            return null;
        }
        public Dictionary<string, string> MakeScenarioParamToCoreTrackIn(int portId, Substrate substrate)
        {
            if (false == _carrierServer.HasCarrier(portId))
                return null;

            string carrierId = _carrierServer.GetCarrierId(portId);
            string lotId = _carrierServer.GetCarrierLotId(portId);
            string partId = substrate.GetAttribute(PWA500SubstrateAttributes.PartId);
            string stepSeq = substrate.GetAttribute(PWA500SubstrateAttributes.StepSeq);
            string recipeId = GetRecipeId();
            string chipQty = _carrierServer.GetAttribute(portId, PWA500CarrierAttributes.KeyLotQty);

            var scenarioParam = new Dictionary<string, string>
            {
                [TrackInOrOut.KeyParamCarrierId] = carrierId,
                [TrackInOrOut.KeyParamPortId] = GetPortName(portId),
                [TrackInOrOut.KeyParamLotId] = lotId,
                [TrackInOrOut.KeyParamPartId] = partId,
                [TrackInOrOut.KeyParamStepSeq] = stepSeq,
                [TrackInOrOut.KeyParamRecipeId] = recipeId,
                [TrackInOrOut.KeyParamChipQty] = chipQty,
                [TrackInOrOut.KeyParamOperatorId] = "AUTO"
            };

            return scenarioParam;
        }
        public Dictionary<string, string> MakeScenarioParamToLotMatch(int portId, string lotId, string carrierId)
        {
            if (false == _carrierServer.HasCarrier(portId))
                return null;

            var scenarioParam = new Dictionary<string, string>
            {
                [TrackInOrOut.KeyParamLotId] = lotId,
                [TrackInOrOut.KeyParamCarrierId] = carrierId,

                // 2024.09.03. jhlim [MOD]
                // MATERIAL_TYPE : TM_TAPE
                // CHANGE_REASON : 전량 소진 후 교체-FINISH_CHAGNE, 품종교체 - PACKAGE_CHAGNE
                // 추후 품종 교체 기준이 생기면 구분이 필요할 수 있다. 현재는 HARDCODING
                [TrackInOrOut.KeyParamChangeReason] = Constants.EmptyWaferChangeReason,
                [TrackInOrOut.KeyParamMaterialType] = Constants.EmptyWaferMaterialType,
                [TrackInOrOut.KeyParamStepSeq] = GetStepIdForBinWafer()
            };
            // 2024.09.03. jhlim [END]

            return scenarioParam;
        }
        public Dictionary<string, string> MakeScenarioParamToTrackOut(string key, string userId, bool isCore)
        {
            if (false == _substrateManager.GetSubstrateByKey(key, out var substrate) || substrate == null)
                return null;

            string lotId = substrate.LotId;
            string partId = substrate.GetAttribute(PWA500SubstrateAttributes.PartId);
            string stepSeq = substrate.GetAttribute(PWA500SubstrateAttributes.StepSeq);
            string recipeId = GetRecipeId();
            string chipQty = substrate.GetAttribute(PWA500SubstrateAttributes.ChipQty);

            int portId;
            if (false == isCore)
            {
                portId = substrate.DestinationPortId;
            }
            else
            {
                portId = substrate.SourcePortId;
            }

            if (portId <= 0 || false == _carrierServer.HasCarrier(portId))
                return null;

            string carrierId = _carrierServer.GetCarrierId(portId);

            Dictionary<string, string> scenarioParams = new Dictionary<string, string>();
            scenarioParams[TrackInOrOut.KeyParamCarrierId] = carrierId;
            scenarioParams[TrackInOrOut.KeyParamPortId] = GetPortName(portId);
            scenarioParams[TrackInOrOut.KeyParamLotId] = lotId;
            scenarioParams[TrackInOrOut.KeyParamPartId] = partId;
            scenarioParams[TrackInOrOut.KeyParamStepSeq] = stepSeq;
            scenarioParams[TrackInOrOut.KeyParamRecipeId] = recipeId;
            scenarioParams[TrackInOrOut.KeyParamChipQty] = chipQty;

            if (false == isCore)
            {
                scenarioParams[TrackInOrOut.KeyParamBinType] = substrate.GetAttribute(PWA500SubstrateAttributes.BinCode);
            }

            scenarioParams[TrackInOrOut.KeyParamOperatorId] = userId;

            return scenarioParams;
        }
        public Dictionary<string, string> MakeScenarioParamToRequestBinPartId(string lotId, string carrierId)
        {
            Dictionary<string, string> scenarioParams = new Dictionary<string, string>
            {
                [LotInfoKeys.KeyParamLotId] = lotId,
                [LotInfoKeys.KeyParamCarrierId] = carrierId
            };

            return scenarioParams;
        }
        public Dictionary<string, string> MakeScenarioParamToAssignSubstrateId(int portId, int slot, SubstrateType substrateType, Substrate substrate)
        {
            if (false == _carrierServer.HasCarrier(portId))
                return null;

            string lotId = substrate.LotId;
            string substrateId = substrate.Name;
            string chipQty = substrate.GetAttribute(PWA500SubstrateAttributes.ChipQty);
            string binCode = substrate.GetAttribute(PWA500SubstrateAttributes.BinCode);

            var scenarioParam = new Dictionary<string, string>
            {
                [AssignSubstrateIdKeys.KeyParamLotId] = lotId,
                [AssignSubstrateIdKeys.KeyParamBinType] = binCode,
                [AssignSubstrateIdKeys.KeyParamRingFrameId] = substrateId,
                [AssignSubstrateIdKeys.KeyParamSlotId] = (slot).ToString(),
                [AssignSubstrateIdKeys.KeyChipQty] = chipQty
            };

            return scenarioParam;
        }
        public Dictionary<string, string> MakeScenarioParamToUploadBinFile(int portId, int slot, string equipId, Substrate substrate)
        {
            if (false == _carrierServer.HasCarrier(portId))
                return null;

            string substrateName = substrate.Name;
            string ringId = substrate.GetAttribute(PWA500SubstrateAttributes.RingId);
            string recipeId = substrate.RecipeId;
            string substrateType = substrate.GetAttribute(PWA500SubstrateAttributes.SubstrateType);
            string stepId = substrate.GetAttribute(PWA500SubstrateAttributes.StepSeq);

            // 2024.10.29. jhlim [MOD] StepSeq가 설정값과 다르면 값을 셋한다.
            string stepSeqFromParam = GetStepIdForBinWafer();
            if (stepId.Equals(stepSeqFromParam))
            {
                _substrateManager.SetAttributeByKey(substrate.UniqueKey, PWA500SubstrateAttributes.StepSeq, stepSeqFromParam);
            }

            stepId = stepSeqFromParam;
            // 2024.10.29. jhlim [END]

            string partId = substrate.GetAttribute(PWA500SubstrateAttributes.PartId);
            string lotId = substrate.LotId;

            var scenarioParam = new Dictionary<string, string>
            {
                [UploadCoreOrBinFileKeys.KeySubstrateName] = substrateName,
                [UploadCoreOrBinFileKeys.KeyRingId] = ringId,
                [UploadCoreOrBinFileKeys.KeyRecipeId] = recipeId,
                [UploadCoreOrBinFileKeys.KeySubstrateType] = substrateType,
                [UploadCoreOrBinFileKeys.KeyStepId] = stepId,
                [UploadCoreOrBinFileKeys.KeyEquipId] = equipId,
                [UploadCoreOrBinFileKeys.KeyPartId] = partId,
                [UploadCoreOrBinFileKeys.KeySlot] = (slot).ToString(),
                [UploadCoreOrBinFileKeys.KeyLotId] = lotId
            };

            return scenarioParam;
        }

        #region <Abstract>
        public virtual SubstrateType GetSubstrateTypeByLoadPortIndex(int lpIndex)
        {
            return SubstrateType.Core;
        }
        #endregion </Abstract>

        #region <Substrate>
        public void AssignSubstrateInfoByCarrierRFIDInfo(int portId, string lotId)
        {
            var substrates = _substrateManager.GetSubstratesAtLoadPort(portId);

            foreach (var item in substrates)
            {
                bool isChanged = false;
                string prevLotId = item.Value.LotId;
                if (string.IsNullOrEmpty(prevLotId)/* || false == item.Value.LotId.Equals(lotId)*/)
                {
                    //item.Value.SetLotId(lotId);
                    _substrateManager.SetLotIdByKey(item.Value.UniqueKey, lotId);
                    isChanged = true;
                }

                string prevParentLotId = item.Value.GetAttribute(PWA500SubstrateAttributes.ParentLotId);
                if (string.IsNullOrEmpty(prevParentLotId))
                {
                    _substrateManager.SetAttributeByKey(item.Value.UniqueKey, PWA500SubstrateAttributes.ParentLotId, lotId);
                    isChanged = true;
                }

                var ringId = item.Value.GetAttribute(PWA500SubstrateAttributes.RingId);
                if (string.IsNullOrEmpty(ringId))
                {
                    _substrateManager.SetAttributeByKey(item.Value.UniqueKey, PWA500SubstrateAttributes.RingId, item.Value.UniqueKey);
                    isChanged = true;
                }

                if (isChanged)
                {
                    _substrateManager.SaveDataByKey(item.Value.UniqueKey);
                }
            }
        }
        public bool GetSubstrateAtProcessModuleByName(string substrateName, out Substrate s)
        {
            s = null;
            if (string.IsNullOrWhiteSpace(substrateName))
                return false;

            var pm = _processGroup.GetProcessModuleName(ProcessModuleIndex);
            List<Substrate> substrates = new List<Substrate>();
            if (false == _substrateManager.GetSubstratesAtProcessModule(pm, ref substrates))
                return false;

            foreach (var item in substrates)
            {
                if (item == null)
                    continue;

                if (string.Equals(item.Name, substrateName, StringComparison.OrdinalIgnoreCase))
                {
                    s = item;
                    return true;
                }
            }

            return false;
        }
        // 공정설비에서 데이터를 주고받을 대 Key를 변경하지 않도록 협의했어야 했다.(Key는 원래 없었고, RingId가 그 역할이었으나, RingId는 공정 설비에서 변경될 수도 있다.)
        public bool FindSubstrateByNameOrRingIdAtProcessModule(string substrateName, string ringId, out Substrate substrate, out string description)
        {
            substrate = null;
            description = string.Empty;

            var pmName = _processGroup.GetProcessModuleName(ProcessModuleIndex);
            List<Substrate> substrates = new List<Substrate>();
            if (false == _substrateManager.GetSubstratesAtProcessModule(pmName, ref substrates))
            {
                description = "There is no substrates at process module";

                return false;
            }

            foreach (var item in substrates)
            {
                if (item == null)
                    continue;

                if (item.Name.Equals(substrateName) ||
                    item.GetAttribute(PWA500SubstrateAttributes.RingId).Equals(ringId) ||
                    item.Name.Equals(ringId) ||
                    item.GetAttribute(PWA500SubstrateAttributes.RingId).Equals(substrateName))
                {
                    substrate = item;
                    return true;
                }
            }

            return false;
        }
        public bool GetSubstrateByName(string targetName, out Substrate s)
        {
            s = null;

            List<Substrate> substrates = new List<Substrate>();
            if (false == _substrateManager.GetSubstratesAll(ref substrates))
                return false;

            foreach (var item in substrates)
            {
                if (string.Equals(targetName, item.Name, StringComparison.OrdinalIgnoreCase))
                {
                    s = item;
                    return true;
                }
            }

            return false;
        }
        public bool FindSubstrateByAttribute(string substrateName, string ringId, string portId, string slot, out Substrate substrate)
        {
            substrate = null;
            var pmName = _processGroup.GetProcessModuleName(ProcessModuleIndex);

            // 정보 핸들링이 Key 기반으로 변경했기 때문에 아래 구문은 제거함 -> 공정설비내 존재하는 자재를 순회해서 찾도록 수정
            // 1. 해당 Substrate의 정보가 공정 설비에 존재하는 경우(정상)
            //if (_substrateManager.GetSubstrateAtProcessModule(pmName, substrateName, out substrate))
            //    return true;

            // 2. 공정 설비에 있는 Substrate와 Source 정보들을 바탕으로 자재를 매칭(이름이 없고 포트번호, 슬롯번호가 존재하는 경우)
            #region <Find substrate by source info>
            List<Substrate> substrates = new List<Substrate>();
            if (false == _substrateManager.GetSubstratesAtProcessModule(pmName, ref substrates))
                return false;

            substrate = null;
            for (int i = 0; i < substrates.Count; ++i)
            {
                // 2025.06.22. jhlim [MOD] 위치 정보보다 이름/링이름을 우선하여 찾는다.
                if (substrates[i].Name.Equals(substrateName) ||
                    substrates[i].GetAttribute(PWA500SubstrateAttributes.RingId).Equals(substrateName) ||
                    substrates[i].Name.Equals(ringId) ||
                    substrates[i].GetAttribute(PWA500SubstrateAttributes.RingId).Equals(ringId))
                {
                    substrate = substrates[i];
                    break;
                }
                // 2025.06.22. jhlim [END]
            }

            if (substrate == null)
            {
                for (int i = 0; i < substrates.Count; ++i)
                {
                    if (substrates[i].SourcePortId.ToString().Equals(portId) && substrates[i].SourceSlot.ToString().Equals(slot))
                    {
                        substrate = substrates[i];
                        break;
                    }
                }
            }
            #endregion </Find substrate by source info>

            return substrate != null;
        }
        #endregion

        #endregion </Methods>
    }
}
