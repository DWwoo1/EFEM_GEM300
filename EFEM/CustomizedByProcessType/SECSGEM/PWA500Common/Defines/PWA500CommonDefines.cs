using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;

using EFEM.History;

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
        // 2026.07.08. jhlim [ADD] 배출 전 RFID 태그에 기입할 랏ID(머지 결과). 앱 재시작 후 재개 시 복원용으로 영속화.
        public const string KeyLotIdToWrite = "LotIdToWrite";
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

        public const string ScrapInfo = "ScrapInfo";
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
    // [영속화 enum] Carrier.Extra(KeyProcessStepBeforeSendingCarrier)에 저장됨. 저장은 이름으로.
    // 멤버 재배치/삭제 금지 — 끝에만 추가. (2026.06.25 커밋 9a22ef2에서 MovingAdsCompleted가
    // SlotMappingCompleted와 WriteTag 사이에 삽입된 전례가 있음 — 이 클래스의 사고를 재발시키지 말 것.)
    public enum StepsBeforeSendingCarrier
    {
        Init = 0,
        MergeAndChangeCompleted = 1,
        SlotMappingCompleted = 2,
        MovingAdsCompleted = 3,
        WriteTag = 4,
    }
    // [영속화 enum] Substrate.Extra(BinUnloadingStep)에 저장됨(PWA500BIN 전용). 저장은 이름으로.
    // 멤버 재배치/삭제 금지 — 끝에만 추가.
    public enum UnloadingStepTypesFor500BIN
    {
        Init = 0,
        AfterScrap = 1,
        AfterIdAssignment = 2,
        AfterBinTrackOut = 3,
        Finished = 4,
    }
    // [영속화 enum] Substrate.Extra(BinUnloadingStep)에 저장됨(PWA500W 전용). 저장은 이름으로.
    // 멤버 재배치/삭제 금지 — 끝에만 추가.
    public enum UnloadingStepTypesFor500W
    {
        Init = 0,
        AfterIdAssignment = 1,
        AfterBinTrackOut = 2,
        Finished = 3,
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
        RequestUploadPBIData,
        #endregion </Request>

        #region <Response>
        ResponseDownloadRecipe,
        ResponseUploadRecipe,
        ResponseDeleteRecipe,
        ResponseAssignSubstrateId,
        ResponseAssignLotId,
        ResponseUploadBinFile,
        ResponseAssignCoreSubstrateId,
        ResponseUploadBinScrapInfo,
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
        RequestUploadBinScrapInfo,
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
    public static class ScrapInfoKeys
    {
        public static readonly string KeySubstrateName = "SubstrateName";
        public static readonly string KeyRingId = "RingId";
        public static readonly string KeyBackUpRingId = "BackUpRingId";
        public static readonly string KeyScrapQty = "ScrapQty";
        public static readonly string KeyScrapData = "ScrapData";
        public static readonly string KeyUserId = "UserId";

        public static readonly string KeyParamLotId = "LOTID";
        public static readonly string KeyParamWaferId = "WAFERID";
        public static readonly string KeyParamScrapQty = "SCRAP_QTY";
        public static readonly string KeyParamScrapInfo = "SCRAP_INFO";
        public static readonly string KeyParamWaferQty = "WAFER_QTY";
        public static readonly string KeyParamOperatorId = "OPERID";
    }
    public static class PBIDataKeys
    {
        public static readonly string KeyPickUpWaferID = "PickUpWaferID";
        public static readonly string KeyPickIndexX = "PickIndexX";
        public static readonly string KeyPickIndexY = "PickIndexY";
        public static readonly string KeyWaferVisionX = "WaferVisionX";
        public static readonly string KeyWaferVisionY = "WaferVisionY";
        public static readonly string KeyWaferVisionT = "WaferVisionT";
        public static readonly string KeyToolNum = "ToolNum";
        public static readonly string KeyPickUpForce = "PickUpForce";
        public static readonly string KeyULCResultX = "ULCResultX";
        public static readonly string KeyULCResultY = "ULCResultY";
        public static readonly string KeyULCResultT = "ULCResultT";
        public static readonly string KeySortingWaferID = "SortingWaferID";
        public static readonly string KeySortingIndexX = "SortingIndexX";
        public static readonly string KeySortingIndexY = "SortingIndexY";
        public static readonly string KeyPlaceForce = "PlaceForce";
        public static readonly string KeyGap_LeftTop_X = "Gap_LeftTop_X";
        public static readonly string KeyGap_LeftBottom_X = "Gap_LeftBottom_X";
        public static readonly string KeyGap_RightTop_X = "Gap_RightTop_X";
        public static readonly string KeyGap_RightBottom_X = "Gap_RightBottom_X";
        public static readonly string KeyGap_LeftTop_Y = "Gap_LeftTop_Y";
        public static readonly string KeyGap_LeftBottom_Y = "Gap_LeftBottom_Y";
        public static readonly string KeyGap_RightTop_Y = "Gap_RightTop_Y";
        public static readonly string KeyGap_RightBottom_Y = "Gap_RightBottom_Y";
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
    public static class FormattedRecipeHandlingKeys
    {
        public static readonly string KeyRecipeId = "RecipeId";
        public static readonly string KeyRecipeBody = "RecipeBody";
        public static readonly string KeyUseCommunicationToPM = "UseCommunicationToPM";

        public static readonly string KeyProcessProgramId = "PPID";
        public static readonly string KeyCommandCode = "CCODE";
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
    public static class UploadMapKeys
    {
        public static readonly string KeyParamWaferId = "WAFERID";
        public static readonly string KeyParamMapData = "MAPDATA";
        public static readonly string KeyParamFilmFrameLocation = "FILM_FRAME_LOCATION";
        public static readonly string KeyParamFlatNotchLocation = "FLAT_NOTCH_LOCATION";
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
    public static class MovingAdsKeys
    {
        public static readonly string KeyParamLotId = "LOTID";
        public static readonly string KeyParamCarrierId = "CARRIERID";

        public static readonly string KeyParamWaferQty = "WAFER_QTY";
        public static readonly string KeyParamAdsMoveFlag = "ADS_MOVE_FLAG";
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
    public static class WaferEndKeys
    {
        public static readonly string KeyParamPortId = "PortID";
        public static readonly string KeyParamLotId = "LOTID";
        public static readonly string KeyParamSlotId = "SLOTID";
        public static readonly string KeyParamSortingInfo = "SORTING_INFO";
        public static readonly string KeyParamRingFrameId = "RINGFRAME_ID";
    }
    #endregion </For SECS/GEM>

    #region <Types>
    public sealed class ScrapCoreInfo
    {
        public ScrapCoreInfo(string info, string qty, string userId = "AUTO")
        {
            Info = info;
            Qty = qty;
            UserId = userId;
        }

        public string Info { get; }
        public string Qty { get; }
        public string UserId { get; }
    }
    #endregion </Types>

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
            var basePath = string.Format(@"{0}\History", Define.DefineConstant.FilePath.FILEPATH_LOG);

            // 2026.07.06. jhlim [MOD] 저장 메커니즘(큐잉/파일 조작/백업/병합/개명/고아 정리)을
            // 공용 엔진(EFEM.History.LotHistoryEngine)으로 분리.
            // 이 클래스는 PWA500 이벤트 어휘(Core/Bin, 이벤트 타입)와 메시지 문구만 정의하는 파사드로 유지한다.
            // 2단계: 영속화를 IHistoryStore로 추상화.
            // 3단계: 파일(주) + DB(병행, best-effort) 합성 - DB 저장소는 초기화 시 AttachDatabaseStore로 장착된다.
            _fileStore = new FileHistoryStore(basePath, Enum.GetNames(typeof(SubstrateType)), "LotHistoryLogError");
            _parallelStore = new ParallelHistoryStore(_fileStore);
            _engine = new LotHistoryEngine(_parallelStore);

            // 4단계: 조회는 자동 소스 선택 - 파일 우선, 없으면 DB 폴백. (랏 목록/집계는 합집합)
            _basePath = basePath;
            _fileQuery = new FileHistoryQuery(_fileStore);
            _query = new CompositeHistoryQuery(_fileQuery);
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
        private static LotHistoryLog _instance = null;
        private readonly LotHistoryEngine _engine = null;
        private readonly FileHistoryStore _fileStore = null;
        private readonly ParallelHistoryStore _parallelStore = null;
        private readonly CompositeHistoryQuery _query = null;
        private readonly FileHistoryQuery _fileQuery = null;
        private readonly string _basePath = null;

        // 병행 검증 기간 전용 : 파일↔DB 일일 자동 대조 (DB 조회 장착 시 활성화)
        private HistoryConsistencyChecker _consistencyChecker = null;
        private DateTime _lastVerifiedDate = DateTime.MinValue;
        private volatile bool _verifyRunning = false;

        // 불변 키 해석기 (초기화 시 주입) - 미주입/해석 실패 시 키는 빈 값으로 기록된다.
        private Func<int, string> _carrierKeyResolver = null;
        private Func<string, string> _substrateKeyResolver = null;
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
            _engine.RegisterCarrierDirectory(portId, name);
        }
        public void AttachDisplayLogAction(Action<int, string> action)
        {
            _engine.AttachDisplayLogAction(action);
        }
        /// <summary>
        /// 2026.07.06. jhlim [ADD] 불변 키 해석기 주입. (초기화 시 1회)
        /// 캐리어 방문 키(포트→CarrierKey)와 기판 생성 키(이름→SubstrateKey)를 이력 레코드에 함께 실어,
        /// DB 저장소(도입 예정)에서 개명/재작업과 무관한 키 컬럼으로 쓸 수 있게 한다.
        /// 미주입 시 키는 빈 값으로 기록되며 파일 이력에는 영향이 없다.
        /// </summary>
        public void AttachKeyResolvers(Func<int, string> carrierKeyByPort, Func<string, string> substrateKeyByName)
        {
            _carrierKeyResolver = carrierKeyByPort;
            _substrateKeyResolver = substrateKeyByName;
        }
        /// <summary>
        /// 2026.07.06. jhlim [ADD] 병행 기록용 DB 저장소 장착. (초기화 시 1회)
        /// 파일 저장소가 주(primary)이고 DB는 best-effort - DB 실패는 파일 기록을 막지 않는다.
        /// </summary>
        public void AttachDatabaseStore(IHistoryStore databaseStore)
        {
            _parallelStore.SetSecondary(databaseStore);
        }
        /// <summary>
        /// 2026.07.06. jhlim [ADD] DB 조회 장착. (초기화 시 1회)
        /// 조회는 항상 파일 우선이며, 파일에 없는 날짜/랏만 DB로 폴백한다. (설정 토글 없음 - 자동 선택)
        /// </summary>
        public void AttachDatabaseQuery(IHistoryQuery databaseQuery)
        {
            _query.SetDatabase(databaseQuery);

            // 병행 검증 기간 전용: 화면은 파일 우선이라 DB 내용을 볼 일이 없으므로
            // 전일자 파일↔DB 대조를 매일 자동 수행해 리포트로 남긴다. (파일 은퇴 시 함께 제거)
            _consistencyChecker = new HistoryConsistencyChecker(
                _fileQuery, databaseQuery, string.Format(@"{0}\Verify", _basePath), _fileStore.WriteDiagnostic);
        }
        /// <summary>조회 화면용 - 자동 소스 선택 조회기를 반환한다. (분류 문자열은 Core/Bin)</summary>
        public IHistoryQuery GetQuery()
        {
            return _query;
        }
        public string GetBackupHistoryPath(DateTime time, bool isCore)
        {
            return _fileStore.GetBackupPath(time, CategoryOf(isCore));
        }
        public string GetCarrierHistoryPath(int portId, string carrierId)
        {
            return _fileStore.GetCarrierHistoryPath(portId, carrierId);
        }
        public string GetSubstratePath(string substrateName, bool isCore)
        {
            return _fileStore.GetSubstratePath(substrateName, CategoryOf(isCore));
        }
        public void ClearPreviousHistory(int portId, string carrierId, string loadportName)
        {
            _engine.ClearPreviousHistory(portId, carrierId, loadportName);
        }
        public void UpdateSubstrateHistoryToCarrierHistory(int portId, string carrierId, string substrateName)
        {
            UpdateSubstrateHistoryToCarrierHistory(portId, carrierId, substrateName, ResolveSubstrateKey(substrateName));
        }
        /// <summary>substrate 객체를 쥔 호출부용 오버로드 - 이름 역해석 없이 불변 키(UniqueKey)를 그대로 사용한다.</summary>
        public void UpdateSubstrateHistoryToCarrierHistory(int portId, string carrierId, string substrateName, string substrateKey)
        {
            // 지연 바인딩 대상은 Bin 기판뿐이다. (Core는 기록 시점에 캐리어에 즉시 이중 기록)
            _engine.BindSubstrateToCarrier(portId, ResolveCarrierKey(portId), carrierId, substrateKey ?? string.Empty, substrateName, CategoryOf(false));
        }
        public void BackupCarrierHistory(int portId, string carrierId, string lotId, List<string> substrates, bool isCore)
        {
            _engine.CompleteCarrier(portId, ResolveCarrierKey(portId), carrierId, lotId, substrates, CategoryOf(isCore));
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
            // 이 시점에는 도메인 개명 전이므로 구 이름(링ID)으로 키를 해석한다.
            WriteSubstrateHistoryForAssignSubstrateId(portId, substrateName, assignedSubstrateName, ResolveSubstrateKey(substrateName));
        }
        /// <summary>substrate 객체를 쥔 호출부용 오버로드 - 새 이름으로는 키 해석이 불가능한 시점이라 명시 키가 특히 중요하다.</summary>
        public void WriteSubstrateHistoryForAssignSubstrateId(int portId, string substrateName, string assignedSubstrateName, string substrateKey)
        {
            _engine.RenameSubstrate(substrateKey ?? string.Empty, substrateName, assignedSubstrateName, CategoryOf(SubstrateType.Bin));

            WriteSubstrateLog(assignedSubstrateName, SubstrateBasedEventType.IdAssign, SubstrateType.Bin, string.Format("서버로부터 이름이 [{0}] 으로 할당됨 [링 이름:{1}]", assignedSubstrateName, substrateName), substrateKey);
        }
        public void WriteSubstrateHistoryForBinWorkEnd(int portId, string substrateName, string binCode, string remainingChips)
        {
            WriteSubstrateHistoryForBinWorkEnd(portId, substrateName, binCode, remainingChips, ResolveSubstrateKey(substrateName));
        }
        public void WriteSubstrateHistoryForBinWorkEnd(int portId, string substrateName, string binCode, string remainingChips, string substrateKey)
        {
            WriteSubstrateLog(substrateName, SubstrateBasedEventType.WorkEnd, SubstrateType.Bin, string.Format("작업 종료 이벤트 송신 -> [빈코드:{0}], [칩수량:{1}]", binCode, remainingChips), substrateKey);
        }
        public void WriteSubstrateHistoryForBinTrackOut(int portId, string substrateName, string lotId, string binCode, string remainingChips)
        {
            WriteSubstrateHistoryForBinTrackOut(portId, substrateName, lotId, binCode, remainingChips, ResolveSubstrateKey(substrateName));
        }
        public void WriteSubstrateHistoryForBinTrackOut(int portId, string substrateName, string lotId, string binCode, string remainingChips, string substrateKey)
        {
            WriteSubstrateLog(substrateName, SubstrateBasedEventType.TrackOut, SubstrateType.Bin, string.Format("랏 [{0}] 트랙 아웃 진행 [빈코드:{1}], [칩수량:{2}]", lotId, binCode, remainingChips), substrateKey);
        }
        public void WriteSubstrateHistoryForReqBinPartId(int portId, string substrateName, string binCode, string oldPartId, string newPartId)
        {
            WriteSubstrateHistoryForReqBinPartId(portId, substrateName, binCode, oldPartId, newPartId, ResolveSubstrateKey(substrateName));
        }
        public void WriteSubstrateHistoryForReqBinPartId(int portId, string substrateName, string binCode, string oldPartId, string newPartId, string substrateKey)
        {
            WriteSubstrateLog(substrateName, SubstrateBasedEventType.ReqPartId, SubstrateType.Bin, string.Format("파트 아이디를 부여받아 [{0}] 에서 [{1}] 로 변경 [빈코드:{2}]", oldPartId, newPartId, binCode), substrateKey);
        }
        public void WriteSubstrateHistoryForUploadBinMap(int portId, string substrateName, string serializedMapData)
        {
            WriteSubstrateLog(substrateName, SubstrateBasedEventType.UploadBinData, SubstrateType.Bin, string.Format("맵 정보 업로드 진행 [직렬화된 Map Data:{0}]", serializedMapData));
        }
        public void WriteSubstrateHistoryForUploadBinData(int portId, string substrateName, string pmsPath)
        {
            WriteSubstrateHistoryForUploadBinData(portId, substrateName, pmsPath, ResolveSubstrateKey(substrateName));
        }
        public void WriteSubstrateHistoryForUploadBinData(int portId, string substrateName, string pmsPath, string substrateKey)
        {
            var fullPath = Path.GetFullPath(pmsPath);
            WriteSubstrateLog(substrateName, SubstrateBasedEventType.UploadBinData, SubstrateType.Bin, string.Format("작업 정보 업로드 진행 [PMS파일 경로:{0}]", fullPath), substrateKey);
        }
        #endregion </SubstrateBasedEvents>

        #region <Executing>
        public void ExecuteWriteAsync()
        {
            _engine.ExecuteWriteAsync();

            RunDailyConsistencyCheckIfNeeded();
        }
        /// <summary>
        /// 기동 후 첫 펌핑과 날짜 변경 후 첫 펌핑에서 전일자 대조를 1회 수행한다.
        /// 대조는 파일/DB 읽기가 있어 백그라운드로 실행한다. (펌핑 루프는 장비 스캔과 엮여 있어 블로킹 금지)
        /// </summary>
        private void RunDailyConsistencyCheckIfNeeded()
        {
            if (_consistencyChecker == null || _verifyRunning)
                return;

            var targetDate = DateTime.Today.AddDays(-1);
            if (_lastVerifiedDate >= targetDate)
                return;

            _verifyRunning = true;
            System.Threading.Tasks.Task.Run(() =>
            {
                try
                {
                    _consistencyChecker.VerifyDate(targetDate, Enum.GetNames(typeof(SubstrateType)));
                }
                catch
                {
                }
                finally
                {
                    // 실패해도 같은 날 재시도 폭주를 막기 위해 완료로 표시한다. (다음 날짜에 다시 수행됨)
                    _lastVerifiedDate = targetDate;
                    _verifyRunning = false;
                }
            });
        }

        public void FlushAll()
        {
            _engine.FlushAll();
        }

        #endregion </Executing>

        #region <Internal>
        private static string CategoryOf(bool isCore)
        {
            return CategoryOf(isCore ? SubstrateType.Core : SubstrateType.Bin);
        }
        private static string CategoryOf(SubstrateType substrateType)
        {
            return substrateType.ToString();
        }
        // 키 해석 실패가 이력 기록 자체를 막으면 안 되므로 예외는 삼키고 빈 값으로 기록한다.
        private string ResolveCarrierKey(int portId)
        {
            try
            {
                return _carrierKeyResolver != null ? (_carrierKeyResolver(portId) ?? string.Empty) : string.Empty;
            }
            catch
            {
                return string.Empty;
            }
        }
        private string ResolveSubstrateKey(string substrateName)
        {
            try
            {
                return _substrateKeyResolver != null ? (_substrateKeyResolver(substrateName) ?? string.Empty) : string.Empty;
            }
            catch
            {
                return string.Empty;
            }
        }
        private void WriteSubstrateLog(int portId, string carrierId, string substrateName, SubstrateBasedEventType type, SubstrateType substrateType, string message)
        {
            // Substrate History와 Carrier History에 동시 기록
            _engine.AppendSubstrateEventWithCarrier(new HistoryRecord
            {
                Time = DateTime.Now,
                PortId = portId,
                Category = CategoryOf(substrateType),
                CarrierKey = ResolveCarrierKey(portId),
                CarrierId = carrierId,
                SubstrateKey = ResolveSubstrateKey(substrateName),
                SubstrateName = substrateName,
                SubstrateEventCode = type.ToString(),
                Message = message,
            });
        }
        private void WriteSubstrateLog(string substrateName, SubstrateBasedEventType type, SubstrateType substrateType, string message)
        {
            WriteSubstrateLog(substrateName, type, substrateType, message, ResolveSubstrateKey(substrateName));
        }
        private void WriteSubstrateLog(string substrateName, SubstrateBasedEventType type, SubstrateType substrateType, string message, string substrateKey)
        {
            _engine.AppendSubstrateEvent(new HistoryRecord
            {
                Time = DateTime.Now,
                PortId = -1,        // 소속 캐리어 미확정 단계라 포트 정보 없음
                Category = CategoryOf(substrateType),
                SubstrateKey = substrateKey ?? string.Empty,
                SubstrateName = substrateName,
                SubstrateEventCode = type.ToString(),
                Message = message,
            });
        }
        private void WriteCarrierLog(int portId, string carrierId, CarrierBasedEventType type, string message)
        {
            _engine.AppendCarrierEvent(new HistoryRecord
            {
                Time = DateTime.Now,
                PortId = portId,
                CarrierKey = ResolveCarrierKey(portId),
                CarrierId = carrierId,
                CarrierEventCode = type.ToString(),
                Message = message,
            });
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
}
