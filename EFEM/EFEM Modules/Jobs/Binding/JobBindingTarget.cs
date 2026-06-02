namespace EFEM.Jobs.Binding
{
    /// <summary>
    /// Binder가 실제로 바인딩 대상으로 보는 Carrier + Slot 정보.
    /// ProcessJob.MaterialInfo 원본에서 만들어지는 조회용 모델이다.
    /// </summary>
    public sealed class JobBindingTarget
    {
        public string ProcessJobId { get; private set; }
        public string CarrierId { get; private set; }
        public int SourcePortId { get; private set; }
        public int Slot { get; private set; }

        public JobBindingTarget(
            string processJobId,
            string carrierId,
            int sourcePortId,
            int slot)
        {
            ProcessJobId = processJobId ?? string.Empty;
            CarrierId = carrierId ?? string.Empty;
            SourcePortId = sourcePortId;
            Slot = slot;
        }
    }
}