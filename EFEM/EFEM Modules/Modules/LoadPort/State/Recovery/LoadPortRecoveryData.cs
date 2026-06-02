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
        [JsonProperty("portId")]
        public int PortId { get; set; }

        // --- Logical State (핵심 복구 대상) ---

        [JsonProperty("reservationState")]
        public ReservationStates ReservationState { get; set; }

        [JsonProperty("associationState")]
        public AssociationStates AssociationState { get; set; }

        [JsonProperty("associatedCarrierId")]
        public string AssociatedCarrierId { get; set; }

        // --- Verification (정책에 따라 사용) ---

        [JsonProperty("carrierIdVerificationState")]
        public CarrierIdVerificationStates CarrierIdVerificationState { get; set; }

        [JsonProperty("slotMapVerificationState")]
        public CarrierSlotMapVerificationStates SlotMapVerificationState { get; set; }

        // --- 참고용 (디버깅 / 정책용) ---

        [JsonProperty("transferState")]
        public LoadPortTransferStates TransferState { get; set; }

        [JsonProperty("accessMode")]
        public LoadPortAccessMode AccessMode { get; set; }

        // --- Metadata ---

        [JsonProperty("savedAtUtc")]
        public DateTime SavedAtUtc { get; set; }

        [JsonProperty("version")]
        public int Version { get; set; } = 1;
    }
}