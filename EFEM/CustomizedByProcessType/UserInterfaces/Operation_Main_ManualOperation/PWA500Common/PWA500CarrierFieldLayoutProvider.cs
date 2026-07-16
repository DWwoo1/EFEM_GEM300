using System.Collections.Generic;

using FrameOfSystem3.Views.Functional;

namespace EFEM.CustomizedByProcessType.UserInterface.OperationMainManual.PWA500Common
{
    /// <summary>
    /// PWA500(BIN/W 공통) Carrier 편집 필드 구성.
    /// GetFields() 반환 순서 = 화면 순서. 순서/카테고리/편집기를 이곳에서 조정한다.
    /// </summary>
    public sealed class PWA500CarrierFieldLayoutProvider : IMaterialFieldLayoutProvider
    {
        public IReadOnlyList<MaterialFieldDescriptor> GetFields()
        {
            var fields = new List<MaterialFieldDescriptor>();
            fields.AddRange(CarrierFieldLayoutCommon.CommonFields());
            fields.AddRange(CarrierFieldLayoutCommon.Pwa500CarrierFields());
            return fields;
        }
    }
}
