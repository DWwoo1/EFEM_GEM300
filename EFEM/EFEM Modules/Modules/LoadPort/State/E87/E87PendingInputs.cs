using System;

using EFEM.Defines.LoadPort;

namespace EFEM.Modules.LoadPort.State
{
    /// <summary>
    /// E87 Transfer/Verification 전이에 필요한 "이번 사이클까지 누적된 외부 입력"을 보관하는 mailbox.
    ///
    /// 설계 의도:
    /// - ApplyExternalInput()는 입력을 기록만 한다.
    /// - 실제 상태 전이는 Evaluate()에서만 수행한다.
    /// - bool 필드 여러 개를 상태모델에 흩뿌리지 않고, 한 객체에서 관리한다.
    /// </summary>
    public sealed class E87PendingInputs
    {
        public string AssociationCarrierId { get; private set; }

        public ServiceStatusChangeRequest ServiceStatusChange { get; private set; }
        public TransferTrigger TransferTriggers { get; private set; }
        public HostVerificationResult CarrierIdResult { get; private set; }
        public HostVerificationResult SlotMapResult { get; private set; }
        public ReservationRequest ReservationRequest { get; private set; }
        public AssociationRequest AssociationRequest { get; private set; }
        public AccessModeChangeRequest AccessModeChange { get; private set; }

        public void Clear()
        {
            ServiceStatusChange = ServiceStatusChangeRequest.None;
            TransferTriggers = TransferTrigger.None;
            CarrierIdResult = HostVerificationResult.None;
            SlotMapResult = HostVerificationResult.None;
            ReservationRequest = ReservationRequest.None;
            AssociationRequest = AssociationRequest.None;
            AccessModeChange = AccessModeChangeRequest.None;
            AssociationCarrierId = null;
        }

        public void RequestInService()
        {
            ServiceStatusChange = ServiceStatusChangeRequest.ToInService;
        }

        public void RequestOutOfService()
        {
            ServiceStatusChange = ServiceStatusChangeRequest.ToOutOfService;
        }

        public void AddTransferTrigger(TransferTrigger trigger)
        {
            TransferTriggers |= trigger;
        }

        public void SetCarrierIdResult(HostVerificationResult result)
        {
            CarrierIdResult = result;
        }

        public void SetSlotMapResult(HostVerificationResult result)
        {
            SlotMapResult = result;
        }

        public void RequestReserveAtPort()
        {
            ReservationRequest = ReservationRequest.ReserveAtPort;
        }

        public void RequestCancelReservationAtPort()
        {
            ReservationRequest = ReservationRequest.CancelReservationAtPort;
        }
        public void RequestBindAssociation(string carrierId)
        {
            AssociationRequest = AssociationRequest.BindAssociation;
            AssociationCarrierId = carrierId;
        }

        public void RequestUnbindAssociation()
        {
            AssociationRequest = AssociationRequest.UnbindAssociation;
            AssociationCarrierId = null;
        }
        public void RequestAccessModeToAuto()
        {
            AccessModeChange = AccessModeChangeRequest.ToAuto;
        }

        public void RequestAccessModeToManual()
        {
            AccessModeChange = AccessModeChangeRequest.ToManual;
        }
        /// <summary>
        /// 현재까지 누적된 입력을 snapshot으로 꺼내고 내부 상태를 초기화한다.
        /// Evaluate() 1회당 1번 호출하는 용도이다.
        /// </summary>
        public E87PendingInputsSnapshot Consume()
        {
            var snapshot = new E87PendingInputsSnapshot(
                ServiceStatusChange,
                TransferTriggers,
                CarrierIdResult,
                SlotMapResult,
                ReservationRequest,
                AssociationRequest,
                AssociationCarrierId,
                AccessModeChange);

            Clear();
            return snapshot;
        }
    }

    /// <summary>
    /// Evaluate() 한 사이클 동안만 사용하는 읽기 전용 입력 snapshot.
    /// </summary>
    public struct E87PendingInputsSnapshot
    {
        public E87PendingInputsSnapshot(
            ServiceStatusChangeRequest serviceStatusChange,
            TransferTrigger transferTriggers,
            HostVerificationResult carrierIdResult,
            HostVerificationResult slotMapResult,
            ReservationRequest reservationRequest,
            AssociationRequest associationRequest,
            string associationCarrierId,
            AccessModeChangeRequest accessModeChange)
        {
            ServiceStatusChange = serviceStatusChange;
            TransferTriggers = transferTriggers;
            CarrierIdResult = carrierIdResult;
            SlotMapResult = slotMapResult;
            ReservationRequest = reservationRequest;
            AssociationRequest = associationRequest;
            AssociationCarrierId = associationCarrierId;
            AccessModeChange = accessModeChange;
        }

        public ServiceStatusChangeRequest ServiceStatusChange { get; }
        public TransferTrigger TransferTriggers { get; }
        public HostVerificationResult CarrierIdResult { get; }
        public HostVerificationResult SlotMapResult { get; }
        public ReservationRequest ReservationRequest { get; }
        public AssociationRequest AssociationRequest { get; }
        public string AssociationCarrierId { get; }
        public AccessModeChangeRequest AccessModeChange { get; }
        public bool HasTransferTrigger(TransferTrigger trigger)
        {
            return (TransferTriggers & trigger) == trigger;
        }
    }
    /// <summary>
    /// E87 Transfer State 전이 #2, #3에 대응하는 서비스 상태 변경 요청.
    /// </summary>
    public enum ServiceStatusChangeRequest
    {
        None = 0,
        ToInService = 1,
        ToOutOfService = 2
    }

    /// <summary>
    /// E87 Transfer State 전이에 필요한 전송/복구 트리거 집합.
    /// Flags로 둔 이유:
    /// - 같은 cycle 안에 여러 입력이 누적될 수 있기 때문
    /// - 예: READY 후 실패 보고, 또는 완료 후 복귀 보고 등
    /// </summary>
    [Flags]
    public enum TransferTrigger
    {
        None = 0,

        // E87 Transfer State 전이 #6:
        // READY TO LOAD -> TRANSFER BLOCKED
        LoadStartedByPioReady = 1 << 0,

        // E87 Transfer State 전이 #7:
        // READY TO UNLOAD -> TRANSFER BLOCKED
        UnloadStartedByPioReady = 1 << 1,

        // E87 Transfer State 전이 #8:
        // TRANSFER BLOCKED -> READY TO LOAD
        UnloadCompletedByPioCompt = 1 << 2,

        // E87 Transfer State 전이 #10:
        // TRANSFER BLOCKED -> TRANSFER READY
        // 현재 구현은 외부 상태를 평탄화하므로
        // 내부적으로 ReadyToLoad/ReadyToUnload로 귀결시키기 위한 실패 신호
        TransferFailed = 1 << 3,

        // E87 Transfer State 전이 #7의 원인 중 하나
        CarrierReCreateIssued = 1 << 4,

        // E87 Transfer State 전이 #9의 구현용 축약 트리거
        CarrierReturnedToPort = 1 << 5
    }

    public enum ReservationRequest
    {
        None = 0,
        ReserveAtPort = 1,
        CancelReservationAtPort = 2
    }
    public enum AssociationRequest
    {
        None = 0,
        BindAssociation = 1,
        UnbindAssociation = 2
    }
    public enum AccessModeChangeRequest
    {
        None = 0,
        ToAuto = 1,
        ToManual = 2
    }
}