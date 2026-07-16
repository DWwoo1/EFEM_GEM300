using System.Collections.Generic;

using Define.DefineEnumProject.AppConfig;
using Define.DefineEnumProject.SelectionList;

namespace FrameOfSystem3.Views.Functional
{
    /// <summary>
    /// FormMaterialAttributeEdit 에서 한 속성(필드)을 어떤 편집기로 수정할지 구분한다.
    /// (Substrate/Carrier 공용 — material 중립)
    /// </summary>
    public enum MaterialFieldEditorKind
    {
        ReadOnly,           // 편집 불가 (표시만)
        Keyboard,           // Form_Keyboard
        CalculatorPort,     // Form_Calculator (1 ~ LoadPort 개수)
        CalculatorSlot,     // Form_Calculator (0 ~ 24)
        SelectionList,      // Form_SelectionList (SelectionListType 사용)
    }

    /// <summary>
    /// FormMaterialAttributeEdit 에 배치되는 한 개의 속성 필드를 서술한다.
    /// 도메인별 <see cref="IMaterialFieldLayoutProvider"/> 가 이 목록의 "순서 그대로" 화면에 나열한다.
    /// </summary>
    public sealed class MaterialFieldDescriptor
    {
        public MaterialFieldDescriptor(
            string key,
            string displayName,
            string category,
            MaterialFieldEditorKind editor,
            EN_SELECTIONLIST selectionListType = EN_SELECTIONLIST.NONE,
            int calcMin = 0,
            int calcMax = 0,
            string[] selectionItems = null,
            int[] selectionIndices = null,
            bool useSelectionValue = false)
        {
            Key = key;
            DisplayName = string.IsNullOrEmpty(displayName) ? key : displayName;
            Category = category ?? string.Empty;
            Editor = editor;
            SelectionListType = selectionListType;
            CalcMin = calcMin;
            CalcMax = calcMax;
            SelectionItems = selectionItems;
            SelectionIndices = selectionIndices;
            UseSelectionValue = useSelectionValue;
        }

        /// <summary>속성 키 (Base*AttributeKeys.* / PWA500*Attributes.*).</summary>
        public string Key { get; }

        /// <summary>화면에 표시할 이름 (Sys3Label). 비어 있으면 Key 사용.</summary>
        public string DisplayName { get; }

        /// <summary>그룹 헤더 문자열. 인접한 같은 문자열끼리 하나의 그룹으로 묶인다.</summary>
        public string Category { get; }

        public MaterialFieldEditorKind Editor { get; }

        /// <summary>
        /// <see cref="MaterialFieldEditorKind.SelectionList"/> 일 때 사용할 선택 목록 종류(전역 레지스트리 기반).
        /// <see cref="SelectionItems"/> 가 지정되면 그쪽이 우선한다.
        /// </summary>
        public EN_SELECTIONLIST SelectionListType { get; }

        /// <summary>
        /// SelectionList 를 EN_SELECTIONLIST 등록 없이 즉석 배열로 구성할 때 사용(저장될 값 자체를 넣는다).
        /// null 이면 <see cref="SelectionListType"/> 을 사용한다.
        /// </summary>
        public string[] SelectionItems { get; }

        /// <summary>SelectionItems 와 짝을 이루는 인덱스/값. null 이면 0..n 이 자동 생성된다.</summary>
        public int[] SelectionIndices { get; }

        /// <summary>
        /// true 이면 선택 결과를 "항목 텍스트"가 아니라 짝지어진 <see cref="SelectionIndices"/> 의 int 값으로 저장한다.
        /// (예: 저장 경로가 ((int)enum).ToString() 인 필드). 이 경우 항목은 표시용, 인덱스는 저장될 정수값이다.
        /// </summary>
        public bool UseSelectionValue { get; }

        /// <summary>Calculator 계열 편집기의 최소값.</summary>
        public int CalcMin { get; }

        /// <summary>Calculator 계열 편집기의 최대값. 0 이면 호출부에서 기본값을 사용한다.</summary>
        public int CalcMax { get; }
    }

    /// <summary>
    /// 도메인/제품별로 편집 화면의 필드 구성을 제공한다. (Substrate/Carrier 공용)
    /// </summary>
    public interface IMaterialFieldLayoutProvider
    {
        IReadOnlyList<MaterialFieldDescriptor> GetFields();
    }

    /// <summary>
    /// 프로세스 타입 → 제품별 Substrate 필드 레이아웃 Provider.
    /// MaterialExtraAttributeFactory(FrameOfSystem3.Functional) 와 동일한 분기 패턴을 따른다.
    /// </summary>
    public static class SubstrateFieldLayoutFactory
    {
        public static IMaterialFieldLayoutProvider Create(EN_PROCESS_TYPE processType)
        {
            switch (processType)
            {
                case EN_PROCESS_TYPE.BIN_SORTER:
                    return new EFEM.CustomizedByProcessType.UserInterface.OperationMainManual.PWA500BIN.PWA500BINSubstrateFieldLayoutProvider();

                case EN_PROCESS_TYPE.DIE_TRANSFER:
                case EN_PROCESS_TYPE.DIE_TRANSFER_300:
                    // 현재 W / W_300 은 동일 레이아웃. 300 전용 구성이 필요해지면 여기서 분리한다.
                    return new EFEM.CustomizedByProcessType.UserInterface.OperationMainManual.PWA500W.PWA500WSubstrateFieldLayoutProvider();

                case EN_PROCESS_TYPE.NONE:
                default:
                    return new NullMaterialFieldLayoutProvider();
            }
        }
    }

    /// <summary>
    /// 프로세스 타입 미지정 등에서 사용하는 빈 Provider. 공통 Substrate 필드만 노출한다.
    /// </summary>
    internal sealed class NullMaterialFieldLayoutProvider : IMaterialFieldLayoutProvider
    {
        public IReadOnlyList<MaterialFieldDescriptor> GetFields()
        {
            return SubstrateFieldLayoutCommon.CommonFields();
        }
    }
}
