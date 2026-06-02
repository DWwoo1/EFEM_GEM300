using System;

namespace EFEM.Jobs.Repository
{
    public sealed class RemovedBindingTarget
    {
        public string ProcessJobId { get; set; }
        public string CarrierId { get; set; }
        public int Slot { get; set; }

        public string Reason { get; set; }
        public DateTime RemovedTime { get; set; }

        public RemovedBindingTarget()
        {
            ProcessJobId = string.Empty;
            CarrierId = string.Empty;
            Reason = string.Empty;
            RemovedTime = DateTime.Now;
        }

        public string GetKey()
        {
            return CreateKey(ProcessJobId, CarrierId, Slot);
        }

        public static string CreateKey(
            string processJobId,
            string carrierId,
            int slot)
        {
            return (processJobId ?? string.Empty)
                + "|"
                + (carrierId ?? string.Empty)
                + "|"
                + slot.ToString();
        }
    }
}