using System;

using Newtonsoft.Json;

using EFEM.Defines.LoadPort;

namespace EFEM.Modules.LoadPort.Recovery
{
    /// <summary>
    /// LoadPort 상태 복구를 위한 최소 스냅샷 데이터
    /// </summary>
    public sealed class LoadPortRecoveryData
    {
        [JsonProperty("PortId")]
        public int PortId { get; set; }

        // --- Logical State (핵심 복구 대상) ---

        [JsonProperty("ReservationState")]
        public ReservationStates ReservationState { get; set; }

        [JsonProperty("AssociationState")]
        public AssociationStates AssociationState { get; set; }

        [JsonProperty("AssociatedCarrierId")]
        public string AssociatedCarrierId { get; set; }

        // --- Verification (정책에 따라 사용) ---

        [JsonProperty("CarrierIdVerificationState")]
        public CarrierIdVerificationStates CarrierIdVerificationState { get; set; }

        [JsonProperty("SlotMapVerificationState")]
        public CarrierSlotMapVerificationStates SlotMapVerificationState { get; set; }

        // --- 참고용 (디버깅 / 정책용) ---

        [JsonProperty("TransferState")]
        public LoadPortTransferStates TransferState { get; set; }

        [JsonProperty("AccessMode")]
        public LoadPortAccessMode AccessMode { get; set; }

        // --- Metadata ---

        [JsonProperty("SavedAtUtc")]
        public DateTime SavedAtUtc { get; set; }
    }
}