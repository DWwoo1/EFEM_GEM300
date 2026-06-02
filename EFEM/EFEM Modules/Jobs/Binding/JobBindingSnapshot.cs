using System.Collections.Generic;
using EFEM.Defines.Job;

namespace EFEM.Jobs.Binding
{
    /// <summary>
    /// ProcessJob 하나에 대한 현재 Substrate 바인딩 상태 스냅샷.
    /// UI 표시용으로 읽기만 한다.
    /// </summary>
    public sealed class JobBindingSnapshot
    {
        public string ControlJobId { get; set; }
        public string ProcessJobId { get; set; }

        public JobBindingStatus Status { get; set; }
        public string Message { get; set; }

        public List<Material> Materials { get; private set; }

        public JobBindingSnapshot()
        {
            ControlJobId = string.Empty;
            ProcessJobId = string.Empty;
            Message = string.Empty;
            Materials = new List<Material>();
        }

        /// <summary>
        /// Carrier + Slot 단위 바인딩 상세 정보.
        /// 독립 도메인 객체가 아니라 JobBindingSnapshot 내부 상세 행이다.
        /// </summary>
        public sealed class Material
        {
            public string CarrierId { get; set; }
            public int Slot { get; set; }
            public int PortId { get; set; }
            public string SubstrateId { get; set; }

            public string BoundControlJobId { get; set; }
            public string BoundProcessJobId { get; set; }
            public string BoundRecipeId { get; set; }

            public JobBindingStatus Status { get; set; }
            public string Message { get; set; }

            public Material()
            {
                CarrierId = string.Empty;
                SubstrateId = string.Empty;
                BoundControlJobId = string.Empty;
                BoundProcessJobId = string.Empty;
                BoundRecipeId = string.Empty;
                Message = string.Empty;
            }
        }
    }
}