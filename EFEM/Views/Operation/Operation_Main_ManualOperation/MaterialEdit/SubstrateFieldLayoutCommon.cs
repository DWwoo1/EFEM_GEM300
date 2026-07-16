using System;
using System.Collections.Generic;

using EFEM.Defines.MaterialTracking;
using EFEM.CustomizedByProcessType.PWA500Common;

namespace FrameOfSystem3.Views.Functional
{
    /// <summary>
    /// 제품 공통(Base) Substrate 속성의 기본 필드 구성.
    /// 제품별 Provider 가 이 목록을 재사용해 확장 속성과 조합한다.
    /// 편집기/카테고리 매핑은 기존 FormMaterialEdit(ClassifyItemTypeByName / GetCategory)에서 이관했다.
    /// </summary>
    public static class SubstrateFieldLayoutCommon
    {
        public const string CategoryBasic = "1.Basic";
        public const string CategorySlots = "2.Slots";
        public const string CategoryStatus = "3.Status";
        public const string CategoryEtc = "4.ETC";
        public const string CategoryJobs = "5.Jobs";
        public const string CategoryUsage = "6.Usage";

        // PWA500(BIN/W) 확장 속성 카테고리
        public const string CategorySubstrate = "7.Substrate";
        public const string CategoryLot = "8.Lot";
        public const string CategoryMap = "9.Map";
        public const string CategoryRoots = "10.Roots";
        public const string CategoryFlags = "11.Flags";

        // SelectionList 항목(저장될 값 = enum 이름 / bool 문자열). EN_SELECTIONLIST 등록 대신 즉석 배열 사용.
        private static readonly string[] BoolItems = { bool.TrueString, bool.FalseString };
        private static readonly string[] TransferStateItems = Enum.GetNames(typeof(TransportStates));
        private static readonly string[] ProcessingStateItems = Enum.GetNames(typeof(ProcessingStates));
        private static readonly string[] IdReadingStateItems = Enum.GetNames(typeof(IdReadingStates));
        private static readonly string[] SubstrateTypeItems = Enum.GetNames(typeof(SubstrateType));

        /// <summary>
        /// 공통 Base 필드를 표준 순서로 반환한다.
        /// </summary>
        public static List<MaterialFieldDescriptor> CommonFields()
        {
            return new List<MaterialFieldDescriptor>
            {
                // 1.Basic
                new MaterialFieldDescriptor(BaseSubstrateAttributeKeys.UniqueKey, "Unique Key", CategoryBasic, MaterialFieldEditorKind.ReadOnly),
                new MaterialFieldDescriptor(BaseSubstrateAttributeKeys.Name,      "Name",       CategoryBasic, MaterialFieldEditorKind.Keyboard),
                new MaterialFieldDescriptor(BaseSubstrateAttributeKeys.LotId,     "Lot ID",     CategoryBasic, MaterialFieldEditorKind.Keyboard),
                new MaterialFieldDescriptor(BaseSubstrateAttributeKeys.Location,  "Location",   CategoryBasic, MaterialFieldEditorKind.ReadOnly),

                // 2.Slots
                new MaterialFieldDescriptor(BaseSubstrateAttributeKeys.SourcePortId,      "Source Port",      CategorySlots, MaterialFieldEditorKind.CalculatorPort),
                new MaterialFieldDescriptor(BaseSubstrateAttributeKeys.SourceSlot,        "Source Slot",      CategorySlots, MaterialFieldEditorKind.CalculatorSlot),
                new MaterialFieldDescriptor(BaseSubstrateAttributeKeys.DestinationPortId, "Destination Port", CategorySlots, MaterialFieldEditorKind.CalculatorPort),
                new MaterialFieldDescriptor(BaseSubstrateAttributeKeys.DestinationSlot,   "Destination Slot", CategorySlots, MaterialFieldEditorKind.CalculatorSlot),

                // 3.Status
                new MaterialFieldDescriptor(BaseSubstrateAttributeKeys.TransPortState,  "Transfer State",   CategoryStatus, MaterialFieldEditorKind.SelectionList, selectionItems: TransferStateItems),
                new MaterialFieldDescriptor(BaseSubstrateAttributeKeys.ProcessingState, "Processing State", CategoryStatus, MaterialFieldEditorKind.SelectionList, selectionItems: ProcessingStateItems),
                new MaterialFieldDescriptor(BaseSubstrateAttributeKeys.IdReadingState,  "Id Reading State", CategoryStatus, MaterialFieldEditorKind.SelectionList, selectionItems: IdReadingStateItems),

                // 5.Jobs
                new MaterialFieldDescriptor(BaseSubstrateAttributeKeys.RecipeId,     "Recipe ID",      CategoryJobs, MaterialFieldEditorKind.Keyboard),
                new MaterialFieldDescriptor(BaseSubstrateAttributeKeys.ProcessJobId, "Process Job ID", CategoryJobs, MaterialFieldEditorKind.Keyboard),
                new MaterialFieldDescriptor(BaseSubstrateAttributeKeys.ControlJobId, "Control Job ID", CategoryJobs, MaterialFieldEditorKind.Keyboard),

                // 6.Usage
                new MaterialFieldDescriptor(BaseSubstrateAttributeKeys.Usage,            "Usage",             CategoryUsage, MaterialFieldEditorKind.SelectionList, selectionItems: BoolItems),
                new MaterialFieldDescriptor(BaseSubstrateAttributeKeys.DoNotProcessFlag, "Do Not Process",    CategoryUsage, MaterialFieldEditorKind.SelectionList, selectionItems: BoolItems),

                // 4.ETC (식별/캐리어 연계 값)
                new MaterialFieldDescriptor(BaseSubstrateAttributeKeys.OriginName,        "Origin Name",         CategoryEtc, MaterialFieldEditorKind.ReadOnly),
                new MaterialFieldDescriptor(BaseSubstrateAttributeKeys.SourceCarrierId,   "Source Carrier ID",   CategoryEtc, MaterialFieldEditorKind.Keyboard),
                new MaterialFieldDescriptor(BaseSubstrateAttributeKeys.CurrentCarrierKey, "Current Carrier Key", CategoryEtc, MaterialFieldEditorKind.Keyboard),
            };
        }

        /// <summary>
        /// PWA500 계열(BIN/W) 확장 속성 필드. 제품별 Provider 에서 재사용한다.
        /// 순서를 제품마다 다르게 하고 싶으면 이 목록을 그대로 쓰지 말고 Provider 에서 직접 구성하면 된다.
        /// </summary>
        public static List<MaterialFieldDescriptor> Pwa500ExtraFields()
        {
            return new List<MaterialFieldDescriptor>
            {
                // 7.Substrate
                new MaterialFieldDescriptor(PWA500SubstrateAttributes.SubstrateType, "Substrate Type", CategorySubstrate, MaterialFieldEditorKind.SelectionList, selectionItems: SubstrateTypeItems),
                new MaterialFieldDescriptor(PWA500SubstrateAttributes.SubstrateSize, "Substrate Size", CategorySubstrate, MaterialFieldEditorKind.Keyboard),
                new MaterialFieldDescriptor(PWA500SubstrateAttributes.RingId,        "Ring ID",        CategorySubstrate, MaterialFieldEditorKind.Keyboard),
                new MaterialFieldDescriptor(PWA500SubstrateAttributes.PartId,        "Part ID",        CategorySubstrate, MaterialFieldEditorKind.Keyboard),

                // 8.Lot
                new MaterialFieldDescriptor(PWA500SubstrateAttributes.LotType,         "Lot Type",          CategoryLot, MaterialFieldEditorKind.Keyboard),
                new MaterialFieldDescriptor(PWA500SubstrateAttributes.StepSeq,         "Step Seq",          CategoryLot, MaterialFieldEditorKind.Keyboard),
                new MaterialFieldDescriptor(PWA500SubstrateAttributes.ChipQty,         "Chip Qty",          CategoryLot, MaterialFieldEditorKind.Keyboard),
                new MaterialFieldDescriptor(PWA500SubstrateAttributes.BinCode,         "Bin Code",          CategoryLot, MaterialFieldEditorKind.Keyboard),
                //new MaterialFieldDescriptor(PWA500SubstrateAttributes.BinUnloadingStep,"Bin Unloading Step",CategoryLot, MaterialFieldEditorKind.Keyboard),

                // 9.Map
                new MaterialFieldDescriptor(PWA500SubstrateAttributes.RefPositionX,      "Ref Position X",      CategoryMap, MaterialFieldEditorKind.Keyboard),
                new MaterialFieldDescriptor(PWA500SubstrateAttributes.RefPositionY,      "Ref Position Y",      CategoryMap, MaterialFieldEditorKind.Keyboard),
                new MaterialFieldDescriptor(PWA500SubstrateAttributes.StartingPositionX, "Starting Position X", CategoryMap, MaterialFieldEditorKind.Keyboard),
                new MaterialFieldDescriptor(PWA500SubstrateAttributes.StartingPositionY, "Starting Position Y", CategoryMap, MaterialFieldEditorKind.Keyboard),
                new MaterialFieldDescriptor(PWA500SubstrateAttributes.CountX,            "Count X",             CategoryMap, MaterialFieldEditorKind.Keyboard),
                new MaterialFieldDescriptor(PWA500SubstrateAttributes.CountY,            "Count Y",             CategoryMap, MaterialFieldEditorKind.Keyboard),
                new MaterialFieldDescriptor(PWA500SubstrateAttributes.Angle,             "Angle",               CategoryMap, MaterialFieldEditorKind.Keyboard),
                new MaterialFieldDescriptor(PWA500SubstrateAttributes.MapData,           "Map Data",            CategoryMap, MaterialFieldEditorKind.Keyboard),

                // 10.Lineage
                new MaterialFieldDescriptor(PWA500SubstrateAttributes.ParentLotId,     "Parent Lot ID",   CategoryRoots, MaterialFieldEditorKind.Keyboard),
                new MaterialFieldDescriptor(PWA500SubstrateAttributes.SplittedLotId,   "Splitted Lot ID", CategoryRoots, MaterialFieldEditorKind.Keyboard),
                new MaterialFieldDescriptor(PWA500SubstrateAttributes.CoreLotId,       "Core Lot ID",     CategoryRoots, MaterialFieldEditorKind.Keyboard),
                new MaterialFieldDescriptor(PWA500SubstrateAttributes.CorePartId,      "Core Part ID",    CategoryRoots, MaterialFieldEditorKind.Keyboard),
                new MaterialFieldDescriptor(PWA500SubstrateAttributes.SplittedHistory, "Splitted History",CategoryRoots, MaterialFieldEditorKind.Keyboard),
                new MaterialFieldDescriptor(PWA500SubstrateAttributes.ScrapInfo,       "Scrap Info",      CategoryRoots, MaterialFieldEditorKind.Keyboard),

                // 11.Flags
                new MaterialFieldDescriptor(PWA500SubstrateAttributes.IsLastSubstrate,     "Is Last Substrate",     CategoryFlags, MaterialFieldEditorKind.SelectionList, selectionItems: BoolItems),
                new MaterialFieldDescriptor(PWA500SubstrateAttributes.IsTrackOutCompleted, "Is Track Out Complete", CategoryFlags, MaterialFieldEditorKind.SelectionList, selectionItems: BoolItems),
            };
        }
    }
}
