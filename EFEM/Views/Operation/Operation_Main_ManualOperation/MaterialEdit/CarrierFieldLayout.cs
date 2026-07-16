using System.Collections.Generic;

using Define.DefineEnumProject.AppConfig;

namespace FrameOfSystem3.Views.Functional
{
    /// <summary>
    /// 프로세스 타입 → Carrier 필드 레이아웃 Provider.
    /// 캐리어 속성은 BIN/W 공통(PWA500Common)이라 모든 PWA500 타입이 동일 Provider 를 사용한다.
    /// (Substrate 와 달리 제품별 분기는 현재 불필요 — 필요해지면 여기서 분리한다.)
    /// </summary>
    public static class CarrierFieldLayoutFactory
    {
        public static IMaterialFieldLayoutProvider Create(EN_PROCESS_TYPE processType)
        {
            switch (processType)
            {
                case EN_PROCESS_TYPE.BIN_SORTER:
                case EN_PROCESS_TYPE.DIE_TRANSFER:
                case EN_PROCESS_TYPE.DIE_TRANSFER_300:
                    return new EFEM.CustomizedByProcessType.UserInterface.OperationMainManual.PWA500Common.PWA500CarrierFieldLayoutProvider();

                case EN_PROCESS_TYPE.NONE:
                default:
                    return new NullCarrierFieldLayoutProvider();
            }
        }
    }

    /// <summary>프로세스 타입 미지정 시 공통 캐리어 필드만 노출한다.</summary>
    internal sealed class NullCarrierFieldLayoutProvider : IMaterialFieldLayoutProvider
    {
        public IReadOnlyList<MaterialFieldDescriptor> GetFields()
        {
            return CarrierFieldLayoutCommon.CommonFields();
        }
    }
}
