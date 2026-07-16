using System;
using System.Collections.Generic;

using EFEM.Defines.MaterialTracking;
using EFEM.CustomizedByProcessType.PWA500Common;

namespace FrameOfSystem3.Views.Functional
{
    /// <summary>
    /// Carrier 편집 화면의 기본 필드 구성(공통 Base + PWA500 확장).
    /// FormMaterialAttributeEdit 이 이 목록을 순서 그대로 접이식 카테고리로 렌더한다.
    /// </summary>
    public static class CarrierFieldLayoutCommon
    {
        public const string CategoryBasic = "1.Basic";
        public const string CategoryStatus = "2.Status";
        public const string CategoryTime = "3.Time";
        public const string CategoryLot = "4.Lot";
        public const string CategoryFlags = "5.Flags";

        // SelectionList 항목(저장될 값). EN_SELECTIONLIST 등록 대신 즉석 배열 사용.
        private static readonly string[] BoolItems = { bool.TrueString, bool.FalseString };
        private static readonly string[] AccessStatusItems = Enum.GetNames(typeof(EFEM.Defines.LoadPort.CarrierAccessStates));

        // ProcessStepBeforeSendingCarrier 는 이름(enum.ToString())으로 저장/EnumPersistence 로 소비된다.
        // 항목=저장값이 모두 enum 이름이라 useSelectionValue 모드가 필요 없다(문자열 저장 모드).
        private static readonly string[] ProcessStepBeforeSendingCarrierItems = Enum.GetNames(typeof(StepsBeforeSendingCarrier));

        /// <summary>공통(Base) 캐리어 필드.</summary>
        public static List<MaterialFieldDescriptor> CommonFields()
        {
            return new List<MaterialFieldDescriptor>
            {
                // 1.Basic
                new MaterialFieldDescriptor(BaseCarrierAttributeKeys.UniqueKey, "Unique Key", CategoryBasic, MaterialFieldEditorKind.ReadOnly),
                new MaterialFieldDescriptor(BaseCarrierAttributeKeys.CarrierId, "Carrier ID", CategoryBasic, MaterialFieldEditorKind.Keyboard),
                new MaterialFieldDescriptor(BaseCarrierAttributeKeys.LotId,     "Lot ID",     CategoryBasic, MaterialFieldEditorKind.Keyboard),

                // 2.Status
                new MaterialFieldDescriptor(BaseCarrierAttributeKeys.CarrierAccessStatus, "Access Status", CategoryStatus, MaterialFieldEditorKind.SelectionList, selectionItems: AccessStatusItems),

                // 3.Time (시스템 기록값 — 표시 전용)
                new MaterialFieldDescriptor(BaseCarrierAttributeKeys.LoadTime,   "Load Time",   CategoryTime, MaterialFieldEditorKind.ReadOnly),
                new MaterialFieldDescriptor(BaseCarrierAttributeKeys.UnloadTime, "Unload Time", CategoryTime, MaterialFieldEditorKind.ReadOnly),
            };
        }

        /// <summary>PWA500(BIN/W 공통) 캐리어 확장 필드.</summary>
        public static List<MaterialFieldDescriptor> Pwa500CarrierFields()
        {
            return new List<MaterialFieldDescriptor>
            {
                // 4.Lot
                new MaterialFieldDescriptor(PWA500CarrierAttributes.KeyPartId,     "Part ID",   CategoryLot, MaterialFieldEditorKind.Keyboard),
                new MaterialFieldDescriptor(PWA500CarrierAttributes.KeyStepSeq,    "Step Seq",  CategoryLot, MaterialFieldEditorKind.Keyboard),
                new MaterialFieldDescriptor(PWA500CarrierAttributes.KeyLotType,    "Lot Type",  CategoryLot, MaterialFieldEditorKind.Keyboard),
                new MaterialFieldDescriptor(PWA500CarrierAttributes.KeyLotQty,     "Lot Qty",   CategoryLot, MaterialFieldEditorKind.Keyboard),
                new MaterialFieldDescriptor(PWA500CarrierAttributes.KeyLotIdToWrite, "Lot ID To Write", CategoryLot, MaterialFieldEditorKind.Keyboard),

                // 5.Flags
                new MaterialFieldDescriptor(PWA500CarrierAttributes.KeyTrackInCompleted,           "Track In Completed",          CategoryFlags, MaterialFieldEditorKind.SelectionList, selectionItems: BoolItems),
                new MaterialFieldDescriptor(PWA500CarrierAttributes.KeyDownloadingRecipeCompleted, "Recipe Download Completed",   CategoryFlags, MaterialFieldEditorKind.SelectionList, selectionItems: BoolItems),
                new MaterialFieldDescriptor(PWA500CarrierAttributes.KeyProcessStepBeforeSendingCarrier, "Step Before Sending",  CategoryFlags, MaterialFieldEditorKind.SelectionList, selectionItems: ProcessStepBeforeSendingCarrierItems),
            };
        }
    }
}
