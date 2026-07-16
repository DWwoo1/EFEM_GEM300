using EFEM.CustomizedByProcessType.PWA500Common;
using EFEM.Defines.MaterialTracking;
using FrameOfSystem3.Views.Functional;
using System;
using System.Collections.Generic;

namespace EFEM.CustomizedByProcessType.UserInterface.OperationMainManual.PWA500BIN
{
    /// <summary>
    /// PWA500BIN(BIN_SORTER) 용 Substrate 편집 필드 구성.
    /// GetFields() 가 반환하는 "순서 그대로" FormSubstrateEdit 화면에 위→아래로 배치된다.
    /// 속성 중요도에 맞춰 순서/카테고리/편집기를 이곳에서 자유롭게 조정한다.
    /// </summary>
    public sealed class PWA500BINSubstrateFieldLayoutProvider : IMaterialFieldLayoutProvider
    {
        private const string CategorySteps = "12.Steps";
        private const string ParameterName = "Bin Unloading Step";
        // BinUnloadingStep 은 이름(enum.ToString())으로 저장/EnumPersistence 로 소비 → 문자열 저장 모드(useSelectionValue 불필요).
        private static readonly string[] UnloadingStepItems = Enum.GetNames(typeof(UnloadingStepTypesFor500BIN));

        public IReadOnlyList<MaterialFieldDescriptor> GetFields()
        {
            var fields = new List<MaterialFieldDescriptor>();
            fields.AddRange(SubstrateFieldLayoutCommon.CommonFields());
            fields.AddRange(SubstrateFieldLayoutCommon.Pwa500ExtraFields());
            fields.AddRange(new List<MaterialFieldDescriptor>
            {
                new MaterialFieldDescriptor(
                PWA500SubstrateAttributes.BinUnloadingStep,
                ParameterName,
                CategorySteps,
                MaterialFieldEditorKind.SelectionList,
                selectionItems: UnloadingStepItems)
            });
            return fields;
        }
    }
}
