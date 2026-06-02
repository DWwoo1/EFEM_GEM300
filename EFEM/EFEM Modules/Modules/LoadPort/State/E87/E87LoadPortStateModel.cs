using System;
using EFEM.Defines.LoadPort;

using EFEM.Modules.LoadPort.Recovery;

namespace EFEM.Modules.LoadPort.State
{
    public sealed class E87LoadPortStateModel : ILoadPortStateModel
    {
        public E87LoadPortStateModel(int portId, VerificationTransitionOptions options = null)
        {
            _portId = portId;
            _options = options ?? new VerificationTransitionOptions();
            _stateInformation = new LoadPortStateInformation();
            _observation = default(LoadPortObservation);

            _carrierStateModel = new E87CarrierStateModel(portId);
        }

        #region <Fields>
        private readonly int _portId;
        private readonly VerificationTransitionOptions _options;
        private readonly object _sync = new object();
        private readonly LoadPortStateInformation _stateInformation;

        private LoadPortObservation _observation;

        // verification + transfer/service/PIO 입력을 모두 한 mailbox에서 관리한다.
        // 의미:
        // - Carrier ID 승인/거절
        // - SlotMap 승인/거절
        // - ChangeServiceStatus
        // - PIO READY / COMPT
        // - TransferFailed / CarrierReCreate / CarrierReturnedToPort
        // 를 ApplyExternalInput()에서 누적하고 Evaluate()에서 소비한다.
        private readonly E87PendingInputs _pendingInputs = new E87PendingInputs();

        private readonly ReservationModel _reservation = new ReservationModel();
        private readonly AssociationModel _association = new AssociationModel();
        private readonly E87CarrierStateModel _carrierStateModel;

        // TransferBlocked의 내부 원인.
        // 외부 공개 상태는 그대로 TransferBlocked를 유지하고,
        // 내부적으로만 PIO handoff 진행 중인지 구분한다.
        private TransferBlockedCause _blockedCause = TransferBlockedCause.None;

        private bool _isSynchronized;
        #endregion </Fields>

        #region <Types>
        private enum TransferBlockedCause
        {
            None = 0,
            LoadPioInProgress = 1,
            UnloadPioInProgress = 2
        }
        #endregion </Types>

        #region <Properties>
        public int PortId
        {
            get { return _portId; }
        }
        public bool SupportsReservationState
        {
            get { return true; }
        }

        public bool SupportsAssociationState
        {
            get { return true; }
        }

        public ReservationStates ReservationState
        {
            get { return _reservation.State; }
        }

        public AssociationStates AssociationState
        {
            get { return _association.State; }
        }
        #endregion </Properties>

        #region <Events>
        public event TransferStateChangedHandler TransferStateChanged;
        public event CarrierIdStateChangedHandler CarrierIdStateChanged;
        public event CarrierSlotMapStateChangedHandler CarrierSlotMapStateChanged;
        public event ReservationStateChangedHandler ReservationStateChanged;
        public event AssociationStateChangedHandler AssociationStateChanged;
        public event AccessModeChangedHandler AccessModeChanged;
        public event CarrierAccessingStateChangedHandler CarrierAccessingStateChanged;
        #endregion </Events>

        #region <Methods>
        public void Initialize()
        {
            lock (_sync)
            {
                // 모든 pending 입력을 초기화한다.
                // 의미:
                // 이전 cycle에 들어온 서비스 요청/PIO 경계/검증 결과가
                // 새 시작 상태에 영향을 주지 않도록 한다.
                _pendingInputs.Clear();
                _reservation.Initialize();
                _association.Initialize();
                _carrierStateModel.Initialize();

                _blockedCause = TransferBlockedCause.None;
                _stateInformation.TransferState = LoadPortTransferStates.OutOfService;
                _stateInformation.CarrierIdVerificationState = CarrierIdVerificationStates.NotRead;
                _stateInformation.CarrierSlotMapVerificationState = CarrierSlotMapVerificationStates.NotRead;

                _carrierStateModel.CopyTo(_stateInformation);
                _stateInformation.ReservationState = _reservation.State;
                _stateInformation.AssociationState = _association.State;
                _stateInformation.AssociatedCarrierId = _association.CarrierId;

                _isSynchronized = false;
            }
        }

        public void Reset()
        {
            lock (_sync)
            {
                // reset도 initialize와 동일하게 모든 pending 입력을 제거한다.
                _pendingInputs.Clear();
                _reservation.Reset();
                _association.Reset();
                _carrierStateModel.Reset();

                _blockedCause = TransferBlockedCause.None;
                _stateInformation.TransferState = LoadPortTransferStates.OutOfService;
                _stateInformation.CarrierIdVerificationState = CarrierIdVerificationStates.NotRead;
                _stateInformation.CarrierSlotMapVerificationState = CarrierSlotMapVerificationStates.NotRead;

                _carrierStateModel.CopyTo(_stateInformation);
                _stateInformation.ReservationState = _reservation.State;
                _stateInformation.AssociationState = _association.State;
                _stateInformation.AssociatedCarrierId = _association.CarrierId;
            }
        }
        public LoadPortRecoveryData CreateRecoveryData()
        {
            lock (_sync)
            {
                return new LoadPortRecoveryData
                {
                    PortId = _portId,

                    // Logical state
                    ReservationState = _reservation.State,
                    AssociationState = _association.State,
                    AssociatedCarrierId = _association.CarrierId,

                    // Verification (정책용/참고용)
                    CarrierIdVerificationState = _stateInformation.CarrierIdVerificationState,
                    SlotMapVerificationState = _stateInformation.CarrierSlotMapVerificationState,

                    // Debug / 참고용
                    TransferState = _stateInformation.TransferState,
                    AccessMode = _stateInformation.AccessMode
                };
            }
        }
        public void RecoverFromObservation(LoadPortRecoveryData recoveryData, in LoadPortObservation observation)
        {
            lock (_sync)
            {
                _pendingInputs.Clear();

                _observation = observation;
                ApplyObservationToStateInformation();

                _carrierStateModel.Synchronize(observation.CarrierAccessingState);
                _carrierStateModel.CopyTo(_stateInformation);

                _stateInformation.TransferState = InferTransferStateFromObservation();

                ApplyRecoveredReservation(recoveryData);
                ApplyRecoveredAssociation(recoveryData);

                if (recoveryData != null)
                {
                    _stateInformation.CarrierIdVerificationState = recoveryData.CarrierIdVerificationState;
                    _stateInformation.CarrierSlotMapVerificationState = recoveryData.SlotMapVerificationState;
                }

                _stateInformation.AccessMode = observation.AccessMode;

                _isSynchronized = true;
            }
        }
        public void UpdateObservation(in LoadPortObservation observation)
        {
            lock (_sync)
            {
                _observation = observation;
            }
        }

        public void ApplyExternalInput(in LoadPortExternalInput input)
        {
            lock (_sync)
            {
                switch (input.InputType)
                {
                    case LoadPortExternalInputType.CarrierIdVerificationAccepted:
                        // Carrier ID 전이 #8:
                        // WAITING FOR HOST -> ID VERIFICATION OK
                        // 의미:
                        // host가 ProceedWithCarrier에 해당하는 수락 결정을 내림
                        _pendingInputs.SetCarrierIdResult(HostVerificationResult.Accepted);
                        break;

                    case LoadPortExternalInputType.CarrierIdVerificationRejected:
                        // Carrier ID 전이 #9:
                        // WAITING FOR HOST -> ID VERIFICATION FAIL
                        // 의미:
                        // host가 CancelCarrier에 해당하는 거절 결정을 내림
                        _pendingInputs.SetCarrierIdResult(HostVerificationResult.Rejected);
                        break;

                    case LoadPortExternalInputType.CarrierSlotMapVerificationAccepted:
                        // Slot Map 전이 #15:
                        // WAITING FOR HOST -> SLOT MAP VERIFICATION OK
                        _pendingInputs.SetSlotMapResult(HostVerificationResult.Accepted);
                        break;

                    case LoadPortExternalInputType.CarrierSlotMapVerificationRejected:
                        // Slot Map 전이 #16:
                        // WAITING FOR HOST -> SLOT MAP VERIFICATION FAIL
                        _pendingInputs.SetSlotMapResult(HostVerificationResult.Rejected);
                        break;

                    case LoadPortExternalInputType.ChangeServiceStatusToInService:
                        // Transfer 전이 #2:
                        // OUT OF SERVICE -> IN SERVICE
                        _pendingInputs.RequestInService();
                        break;

                    case LoadPortExternalInputType.ChangeServiceStatusToOutOfService:
                        // Transfer 전이 #3:
                        // IN SERVICE -> OUT OF SERVICE
                        _pendingInputs.RequestOutOfService();
                        break;

                    case LoadPortExternalInputType.LoadTransferStartedByPioReady:
                        // Transfer 전이 #6:
                        // READY TO LOAD -> TRANSFER BLOCKED
                        // 의미:
                        // 자동 로드 전송 handoff가 PIO READY로 시작됨
                        _pendingInputs.AddTransferTrigger(TransferTrigger.LoadStartedByPioReady);
                        break;

                    case LoadPortExternalInputType.UnloadTransferStartedByPioReady:
                        // Transfer 전이 #7:
                        // READY TO UNLOAD -> TRANSFER BLOCKED
                        // 의미:
                        // 자동 언로드 전송 handoff가 PIO READY로 시작됨
                        _pendingInputs.AddTransferTrigger(TransferTrigger.UnloadStartedByPioReady);
                        break;

                    case LoadPortExternalInputType.UnloadTransferCompletedByPioCompt:
                        // Transfer 전이 #8:
                        // TRANSFER BLOCKED -> READY TO LOAD
                        // 의미:
                        // 자동 언로드 전송이 PIO COMPT와 함께 정상 완료됨
                        _pendingInputs.AddTransferTrigger(TransferTrigger.UnloadCompletedByPioCompt);
                        break;

                    case LoadPortExternalInputType.TransferFailed:
                        // Transfer 전이 #10:
                        // TRANSFER BLOCKED -> TRANSFER READY
                        // 의미:
                        // 전송이 실패했음.
                        // 주의:
                        // 이 입력은 별도의 전용 PIO FAIL 선을 직접 의미하지 않는다.
                        // AMHS timeout / error / abort 등 상위 failure event 를
                        // TransferFailed 로 매핑한 것이다.
                        // 현재 구현은 TRANSFER READY 를 외부 상태로 두지 않으므로
                        // ReadyToLoad / ReadyToUnload 로 평탄화한다.
                        _pendingInputs.AddTransferTrigger(TransferTrigger.TransferFailed);
                        break;

                    case LoadPortExternalInputType.CarrierReCreateIssued:
                        // Transfer 전이 #7의 원인 중 하나:
                        // READY TO UNLOAD -> TRANSFER BLOCKED
                        _pendingInputs.AddTransferTrigger(TransferTrigger.CarrierReCreateIssued);
                        break;

                    case LoadPortExternalInputType.CarrierReturnedToPort:
                        // Transfer 전이 #9의 구현용 축약:
                        // TRANSFER BLOCKED -> READY TO UNLOAD
                        // 의미:
                        // 공정 완료 / cancel / 포트 복귀 등으로
                        // 이제 unload 가능한 위치로 돌아왔음을 알림
                        _pendingInputs.AddTransferTrigger(TransferTrigger.CarrierReturnedToPort);
                        break;

                    case LoadPortExternalInputType.ReserveAtPort:
                        // E87 Reservation 전이:
                        // NOT RESERVED -> RESERVED
                        // 의미:
                        // ReserveAtPort 서비스가 발행되어
                        // 현재 포트를 특정 carrier / 작업을 위해 예약 상태로 전이시킨다.
                        _pendingInputs.RequestReserveAtPort();
                        break;

                    case LoadPortExternalInputType.CancelReservationAtPort:
                        // E87 Reservation 전이:
                        // RESERVED -> NOT RESERVED
                        // 의미:
                        // CancelReservationAtPort 서비스가 발행되어
                        // 현재 포트의 예약을 해제한다.
                        _pendingInputs.RequestCancelReservationAtPort();
                        break;

                    case LoadPortExternalInputType.BindAssociation:
                        // E87 Bind 서비스:
                        // - Port 와 CarrierID 를 연계(Association)
                        // - 동시에 Reservation 을 NOT RESERVED -> RESERVED 로 전이시킨다.
                        _pendingInputs.RequestBindAssociation(input.CarrierId);
                        break;

                    case LoadPortExternalInputType.UnbindAssociation:
                        // E87 CancelBind 서비스:
                        // - 현재 Port/Carrier Association 을 해제
                        // - Reservation 이 RESERVED 상태라면 NOT RESERVED 로 전이시킨다.
                        _pendingInputs.RequestUnbindAssociation();
                        break;

                    case LoadPortExternalInputType.ChangeAccessModeToAuto:
                        // E87 ChangeAccess 서비스:
                        // MANUAL -> AUTO 요청
                        _pendingInputs.RequestAccessModeToAuto();
                        break;

                    case LoadPortExternalInputType.ChangeAccessModeToManual:
                        // E87 ChangeAccess 서비스:
                        // AUTO -> MANUAL 요청
                        _pendingInputs.RequestAccessModeToManual();
                        break;
                }
            }
        }
        private void ApplyRecoveredReservation(LoadPortRecoveryData data)
        {
            if (data == null)
                return;

            if (data.ReservationState == ReservationStates.Reserved)
                _reservation.ReserveAtPort();
            else
                _reservation.CancelReservationAtPort();

            _stateInformation.ReservationState = _reservation.State;
        }

        private void ApplyRecoveredAssociation(LoadPortRecoveryData data)
        {
            if (data == null)
                return;

            if (data.AssociationState == AssociationStates.Associated)
                _association.Bind(data.AssociatedCarrierId);
            else
                _association.Unbind();

            _stateInformation.AssociationState = _association.State;
            _stateInformation.AssociatedCarrierId = _association.CarrierId;
        }
        public bool CanChangeAccessMode(LoadPortAccessMode targetMode)
        {
            lock (_sync)
            {
                // 같은 모드로의 요청은 허용
                if (_stateInformation.AccessMode == targetMode)
                {
                    return true;
                }

                // 문서상 RESERVED 상태에서는 AccessMode 변경 금지
                if (_reservation.State == ReservationStates.Reserved)
                {
                    return false;
                }

                // 문서상 carrier transfer 중에는 AccessMode 변경 금지
                // 현재 구현의 최소 해석은 TRANSFER BLOCKED 상태를 전송 중으로 본다.
                if (_stateInformation.TransferState == LoadPortTransferStates.TransferBlocked)
                {
                    return false;
                }

                return true;
            }
        }
        public void CopyStateTo(LoadPortStateInformation state)
        {
            lock (_sync)
            {
                _stateInformation.CopyTo(ref state);
            }
        }
        private LoadPortTransferStates InferTransferStateFromObservation()
        {
            if (!_stateInformation.Enabled || !_stateInformation.Initialized)
                return LoadPortTransferStates.OutOfService;

            if (!_stateInformation.Present &&
                !_stateInformation.Placed &&
                !_stateInformation.ClampState &&
                !_stateInformation.DockState &&
                !_stateInformation.DoorState)
            {
                return LoadPortTransferStates.ReadyToLoad;
            }

            if (_stateInformation.Present &&
                _stateInformation.Placed &&
                !_stateInformation.ClampState &&
                !_stateInformation.DockState &&
                !_stateInformation.DoorState &&
                (_stateInformation.CarrierAccessingState == CarrierAccessStates.CarrierCompleted ||
                 _stateInformation.CarrierAccessingState == CarrierAccessStates.CarrierStopped))
            {
                return LoadPortTransferStates.ReadyToUnload;
            }

            return LoadPortTransferStates.TransferBlocked;
        }
        public bool Evaluate()
        {
            TransferStateChangedEvent transferEvent = default(TransferStateChangedEvent);
            CarrierIdStateChangedEvent carrierIdEvent = default(CarrierIdStateChangedEvent);
            CarrierSlotMapStateChangedEvent slotMapEvent = default(CarrierSlotMapStateChangedEvent);
            ReservationStateChangedEvent reservationEvent = default(ReservationStateChangedEvent);
            AssociationStateChangedEvent associationEvent = default(AssociationStateChangedEvent);
            AccessModeChangedEvent accessModeEvent = default(AccessModeChangedEvent);
            CarrierAccessingStateChangedEvent carrierAccessingEvent = default(CarrierAccessingStateChangedEvent);

            bool raiseTransferEvent = false;
            bool raiseCarrierIdEvent = false;
            bool raiseSlotMapEvent = false;
            bool raiseReservationEvent = false;
            bool raiseAssociationEvent = false;
            bool raiseAccessModeEvent = false;
            bool raiseCarrierAccessingEvent = false;

            TransferStateChangedHandler transferHandler = TransferStateChanged;
            CarrierIdStateChangedHandler carrierIdHandler = CarrierIdStateChanged;
            CarrierSlotMapStateChangedHandler slotMapHandler = CarrierSlotMapStateChanged;
            ReservationStateChangedHandler reservationHandler = ReservationStateChanged;
            AssociationStateChangedHandler associationHandler = AssociationStateChanged;
            AccessModeChangedHandler accessModeHandler = AccessModeChanged;
            CarrierAccessingStateChangedHandler carrierAccessingHandler = CarrierAccessingStateChanged;

            bool changed = false;

            lock (_sync)
            {
                var prevTransfer = _stateInformation.TransferState;
                var prevCarrierId = _stateInformation.CarrierIdVerificationState;
                var prevSlotMap = _stateInformation.CarrierSlotMapVerificationState;
                var prevReservation = _stateInformation.ReservationState;
                var prevAssociation = _stateInformation.AssociationState;
                var prevAssociatedCarrierId = _stateInformation.AssociatedCarrierId;
                var prevAccessMode = _stateInformation.AccessMode;

                var prevClamp = _stateInformation.ClampState;
                var prevDock = _stateInformation.DockState;
                var prevDoor = _stateInformation.DoorState;

                // 복구 완료 전에는 observation 값만 반영한다.
                // pending input 소비, 상태전이 계산, 이벤트 발행은 하지 않는다.
                ApplyObservationToStateInformation();

                if (false == _isSynchronized)
                {
                    _carrierStateModel.Synchronize(_observation.CarrierAccessingState);
                    _carrierStateModel.CopyTo(_stateInformation);

                    return false;
                }

                // 이번 Evaluate()에서만 사용할 입력 snapshot을 꺼낸다.
                // 의미:
                // ApplyExternalInput()로 들어온 서비스/PIO/검증 결과를
                // 상태 전이 계산 시점에 원자적으로 소비한다.
                var pending = _pendingInputs.Consume();

                //ApplyObservationToStateInformation();

                // CarrierAccessingState는 raw observation을 직접 쓰지 않고,
                // E87CarrierStateModel이 compare / projection 을 담당한다.
                _carrierStateModel.UpdateObservation(_observation.CarrierAccessingState);

                bool carrierAccessingChanged = _carrierStateModel.Evaluate(out carrierAccessingEvent);
                changed |= carrierAccessingChanged;

                _carrierStateModel.CopyTo(_stateInformation);

                // 실제 CarrierAccessingStateChanged 이벤트 발행은
                // 다른 상태 이벤트와 동일하게 Evaluate() 마지막(lock 밖)에서 수행한다.
                if (carrierAccessingChanged && carrierAccessingHandler != null)
                {
                    raiseCarrierAccessingEvent = true;
                }

                // 서비스 입력만 보고 짧게 처리한다.
                switch (pending.ReservationRequest)
                {
                    case ReservationRequest.ReserveAtPort:
                        // E87 Reservation 전이:
                        // NOT RESERVED -> RESERVED
                        // ReserveAtPort 서비스 입력을 현재 cycle에서 반영한다.
                        _reservation.ReserveAtPort();
                        break;

                    case ReservationRequest.CancelReservationAtPort:
                        // E87 Reservation 전이:
                        // RESERVED -> NOT RESERVED
                        // CancelReservationAtPort 서비스 입력을 현재 cycle에서 반영한다.
                        _reservation.CancelReservationAtPort();
                        break;
                }

                switch (pending.AssociationRequest)
                {
                    case AssociationRequest.BindAssociation:
                        // E87 Bind 서비스 의미:
                        // - Load Port/Carrier Association 을 ASSOCIATED 로 전이
                        // - 동시에 Load Port Reservation 을 NOT RESERVED -> RESERVED 로 전이
                        //
                        // 구현 메모:
                        // - 현재 구현에서 Bind 는 logical association 서비스로만 해석한다.
                        // - 즉 Bind 입력만으로 physical carrier object 를 생성하지 않는다.
                        // - physical carrier 생성은 포트에 캐리어가 실제 안착되었음이 확인된 뒤
                        //   (TransferBlocked + Present + Placed) LoadPortOperator.AssignCarrierByTransferState()에서 수행한다.
                        _association.Bind(pending.AssociationCarrierId);
                        //_reservation.ReserveAtPort();
                        break;

                    case AssociationRequest.UnbindAssociation:
                        // E87 CancelBind 서비스 의미:
                        // - Association 을 NOT ASSOCIATED 로 전이
                        _association.Unbind();
                        //_reservation.CancelReservationAtPort();
                        break;
                }

                // E87 ChangeAccess 서비스 입력은 mailbox에서 소비하되,
                // 실제 AccessMode 상태는 Controller observation 값으로만 확정한다.
                // 즉 여기서는 직접 _stateInformation.AccessMode 를 변경하지 않는다.
                switch (pending.AccessModeChange)
                {
                    case AccessModeChangeRequest.ToAuto:
                    case AccessModeChangeRequest.ToManual:
                        break;
                }

                if (prevAccessMode != _stateInformation.AccessMode)
                {
                    changed = true;

                    if (accessModeHandler != null)
                    {
                        accessModeEvent = new AccessModeChangedEvent
                        {
                            PortId = _portId,
                            PreviousMode = prevAccessMode,
                            CurrentMode = _stateInformation.AccessMode
                        };
                        raiseAccessModeEvent = true;
                    }
                }

                var nextTransfer = EvaluateTransfer(prevTransfer, pending);

                var nextCarrierId = EvaluateCarrierId(prevCarrierId, nextTransfer, prevClamp, prevDock, pending);
                // 이번 Evaluate 안에서 WaitingForHost에 들어갔고 결과가 이미 있으면 즉시 완료 처리
                if (nextCarrierId == CarrierIdVerificationStates.WaitingForHost &&
                    pending.CarrierIdResult != HostVerificationResult.None)
                {
                    nextCarrierId = pending.CarrierIdResult == HostVerificationResult.Accepted
                        ? CarrierIdVerificationStates.VerificationOk
                        : CarrierIdVerificationStates.VerificationFailed;
                }

                var nextSlotMap = EvaluateSlotMap(prevSlotMap, nextTransfer, nextCarrierId, prevDoor, pending);
                // 이번 Evaluate 안에서 WaitingForHost에 들어갔고 결과가 이미 있으면 즉시 완료 처리
                if (nextSlotMap == CarrierSlotMapVerificationStates.WaitingForHost &&
                    pending.SlotMapResult != HostVerificationResult.None)
                {
                    nextSlotMap = pending.SlotMapResult == HostVerificationResult.Accepted
                        ? CarrierSlotMapVerificationStates.VerificationOk
                        : CarrierSlotMapVerificationStates.VerificationFailed;
                }

                _stateInformation.TransferState = nextTransfer;
                _stateInformation.CarrierIdVerificationState = nextCarrierId;
                _stateInformation.CarrierSlotMapVerificationState = nextSlotMap;
                _stateInformation.ReservationState = _reservation.State;
                _stateInformation.AssociationState = _association.State;
                _stateInformation.AssociatedCarrierId = _association.CarrierId;

                if (prevTransfer != nextTransfer)
                {
                    changed = true;

                    if (transferHandler != null)
                    {
                        transferEvent = new TransferStateChangedEvent
                        {
                            PortId = _portId,
                            PreviousState = prevTransfer,
                            CurrentState = nextTransfer
                        };
                        raiseTransferEvent = true;
                    }
                }

                if (prevCarrierId != nextCarrierId)
                {
                    changed = true;

                    if (carrierIdHandler != null)
                    {
                        carrierIdEvent = new CarrierIdStateChangedEvent
                        {
                            PortId = _portId,
                            PreviousState = prevCarrierId,
                            CurrentState = nextCarrierId
                        };
                        raiseCarrierIdEvent = true;
                    }
                }

                if (prevSlotMap != nextSlotMap)
                {
                    changed = true;

                    if (slotMapHandler != null)
                    {
                        slotMapEvent = new CarrierSlotMapStateChangedEvent
                        {
                            PortId = _portId,
                            PreviousState = prevSlotMap,
                            CurrentState = nextSlotMap
                        };
                        raiseSlotMapEvent = true;
                    }
                }

                if (prevReservation != _stateInformation.ReservationState)
                {
                    changed = true;

                    if (reservationHandler != null)
                    {
                        reservationEvent = new ReservationStateChangedEvent
                        {
                            PortId = _portId,
                            PreviousState = prevReservation,
                            CurrentState = _stateInformation.ReservationState
                        };
                        raiseReservationEvent = true;
                    }
                }

                if (prevAssociation != _stateInformation.AssociationState ||
                    !string.Equals(prevAssociatedCarrierId, _stateInformation.AssociatedCarrierId, StringComparison.Ordinal))
                {
                    changed = true;

                    if (associationHandler != null)
                    {
                        associationEvent = new AssociationStateChangedEvent
                        {
                            PortId = _portId,
                            PreviousState = prevAssociation,
                            CurrentState = _stateInformation.AssociationState,
                            PreviousCarrierId = prevAssociatedCarrierId,
                            CurrentCarrierId = _stateInformation.AssociatedCarrierId
                        };
                        raiseAssociationEvent = true;
                    }
                }
            }

            if (raiseTransferEvent)
            {
                transferHandler(this, transferEvent);
            }

            if (raiseCarrierIdEvent)
            {
                carrierIdHandler(this, carrierIdEvent);
            }

            if (raiseSlotMapEvent)
            {
                slotMapHandler(this, slotMapEvent);
            }

            if (raiseReservationEvent)
            {
                reservationHandler(this, reservationEvent);
            }

            if (raiseAssociationEvent)
            {
                associationHandler(this, associationEvent);
            }

            if (raiseAccessModeEvent)
            {
                accessModeHandler(this, accessModeEvent);
            }

            if (raiseCarrierAccessingEvent)
            {
                carrierAccessingHandler(this, carrierAccessingEvent);
            }

            return changed;
        }
        private void ApplyObservationToStateInformation()
        {
            _stateInformation.Enabled = _observation.Enabled;
            _stateInformation.Initialized = _observation.Initialized;
            _stateInformation.Present = _observation.Present;
            _stateInformation.Placed = _observation.Placed;
            _stateInformation.IsPlacementMismatch = _observation.IsPlacementMismatch;
            _stateInformation.ClampState = _observation.ClampState;
            _stateInformation.DockState = _observation.DockState;
            _stateInformation.DoorState = _observation.DoorState;
            _stateInformation.PlacementErrorState = _observation.PlacementErrorState;
            _stateInformation.CarrierOutErrorState = _observation.CarrierOutErrorState;
            _stateInformation.TriggeredAlarm = _observation.TriggeredAlarm;
            _stateInformation.AccessMode = _observation.AccessMode;
            _stateInformation.LoadingType = _observation.LoadingType;
        }
        private LoadPortTransferStates EvaluateTransfer(
            LoadPortTransferStates prevTransfer,
            E87PendingInputsSnapshot pending)
        {
            // Transfer 전이 #3:
            // IN SERVICE -> OUT OF SERVICE
            // 의미:
            // 서비스 상태 변경 요청이 들어오면 가장 우선해서 OutOfService로 보낸다.
            if (pending.ServiceStatusChange == ServiceStatusChangeRequest.ToOutOfService)
            {
                _blockedCause = TransferBlockedCause.None;
                return LoadPortTransferStates.OutOfService;
            }

            switch (prevTransfer)
            {
                case LoadPortTransferStates.OutOfService:
                    {
                        _blockedCause = TransferBlockedCause.None;

                        // Transfer 전이 #2:
                        // OUT OF SERVICE -> IN SERVICE
                        // 의미:
                        // ChangeServiceStatus 서비스로 사용 가능 상태에 들어간다.
                        if (pending.ServiceStatusChange == ServiceStatusChangeRequest.ToInService)
                        {
                            return LoadPortTransferStates.InService;
                        }

                        return LoadPortTransferStates.OutOfService;
                    }

                case LoadPortTransferStates.InService:
                    {
                        _blockedCause = TransferBlockedCause.None;

                        // E87 Transfer 전이 #4 구현:
                        // IN SERVICE 의 디폴트 입력은 TRANSFER READY 또는 TRANSFER BLOCKED 이다.
                        // 현재 구현은 TRANSFER READY 를 외부 enum 으로 두지 않으므로,
                        // 캐리어가 존재하지 않으면 READY TO LOAD,
                        // 캐리어가 이미 존재하면 TRANSFER BLOCKED 로 평탄화한다.
                        return (_stateInformation.Present && _stateInformation.Placed)
                            ? LoadPortTransferStates.TransferBlocked
                            : LoadPortTransferStates.ReadyToLoad;
                    }

                case LoadPortTransferStates.ReadyToLoad:
                    {
                        _blockedCause = TransferBlockedCause.None;

                        // E87 Transfer 전이 #6:
                        // READY TO LOAD -> TRANSFER BLOCKED
                        // - Manual: 장비가 수동 로드 전송 시작을 논리적으로 인지
                        // - Automated: PIO READY 활성화
                        // - Internal Buffer: CarrierOut 시작
                        // 현재 구현은
                        //   * Auto 모드에서는 PIO READY 를 시작 경계로 사용하고
                        //   * Manual 로드는 실제 안착 센서(Present && Placed)로 Blocked 진입을 판정한다.
                        if (_stateInformation.AccessMode == LoadPortAccessMode.Auto &&
                            pending.HasTransferTrigger(TransferTrigger.LoadStartedByPioReady))
                        {
                            _blockedCause = TransferBlockedCause.LoadPioInProgress;
                            return LoadPortTransferStates.TransferBlocked;
                        }

                        // Manual load 또는 PIO 없이도 실제 캐리어가 정상 안착되면 Blocked 로 본다.
                        if (_stateInformation.Present && _stateInformation.Placed)
                        {
                            return LoadPortTransferStates.TransferBlocked;
                        }

                        return LoadPortTransferStates.ReadyToLoad;
                    }

                case LoadPortTransferStates.ReadyToUnload:
                    {
                        _blockedCause = TransferBlockedCause.None;

                        // E87 Transfer 전이 #7:
                        // READY TO UNLOAD -> TRANSFER BLOCKED
                        // - Manual: 수동 언로드 시작에 대한 논리적 지시
                        // - Automated: PIO READY 활성화
                        // - Internal Buffer: CarrierIn 시작
                        // - By Service: CarrierReCreate
                        // 현재 구현은
                        //   * Auto 모드에서는 PIO READY 를 언로드 시작 경계로 사용하고
                        //   * CarrierReCreate 는 access mode 와 무관한 상위 서비스 입력으로 유지한다.
                        if (_stateInformation.AccessMode == LoadPortAccessMode.Auto &&
                            pending.HasTransferTrigger(TransferTrigger.UnloadStartedByPioReady))
                        {
                            _blockedCause = TransferBlockedCause.UnloadPioInProgress;
                            return LoadPortTransferStates.TransferBlocked;
                        }

                        if (pending.HasTransferTrigger(TransferTrigger.CarrierReCreateIssued))
                        {
                            _blockedCause = TransferBlockedCause.UnloadPioInProgress;
                            return LoadPortTransferStates.TransferBlocked;
                        }

                        // Empty-port fallback:
                        // 실제 캐리어 제거가 완료되고 clamp/dock/door 까지 모두 해제되었으면
                        // READY TO LOAD 로 복귀한다.
                        if (!_stateInformation.Present &&
                            !_stateInformation.Placed &&
                            !_stateInformation.DockState &&
                            !_stateInformation.DoorState &&
                            !_stateInformation.ClampState)
                        {
                            return LoadPortTransferStates.ReadyToLoad;
                        }

                        return LoadPortTransferStates.ReadyToUnload;
                    }

                case LoadPortTransferStates.TransferBlocked:
                    {
                        // E87 Transfer 전이 #8:
                        // TRANSFER BLOCKED -> READY TO LOAD
                        // - Manual: 캐리어 언로드 완료 + 현재 신호가 캐리어 없음 + 작업자가 완료를 논리적으로 지시
                        // - Automated: PIO COMPT
                        // - Internal Buffer: 로드 포트에서 내부 버퍼로 이동 완료
                        // 현재 구현은 Auto 모드에서는 COMPT 입력으로 완료를 인지한다.
                        if (_blockedCause == TransferBlockedCause.UnloadPioInProgress &&
                            pending.HasTransferTrigger(TransferTrigger.UnloadCompletedByPioCompt))
                        {
                            _blockedCause = TransferBlockedCause.None;
                            return LoadPortTransferStates.ReadyToLoad;
                        }

                        // Transfer 전이 #9의 구현용 축약:
                        // TRANSFER BLOCKED -> READY TO UNLOAD
                        // 의미:
                        // 공정 완료 / CancelCarrier / 포트 복귀 등으로
                        // 이제 unload 가능한 상태가 되었음을 나타낸다.
                        if (_blockedCause == TransferBlockedCause.UnloadPioInProgress &&
                            pending.HasTransferTrigger(TransferTrigger.CarrierReturnedToPort))
                        {
                            _blockedCause = TransferBlockedCause.None;
                            return LoadPortTransferStates.ReadyToUnload;
                        }

                        // Transfer 전이 #10:
                        // TRANSFER BLOCKED -> TRANSFER READY
                        // 의미:
                        // 전송 실패 후 문서상으로는 TRANSFER READY로 돌아가야 한다.
                        // 현재 구현은 TRANSFER READY를 외부 상태로 두지 않으므로,
                        // 현재 관측값을 기준으로 ReadyToLoad/ReadyToUnload로 평탄화한다.
                        if (pending.HasTransferTrigger(TransferTrigger.TransferFailed))
                        {
                            _blockedCause = TransferBlockedCause.None;

                            if (!_stateInformation.Present &&
                                !_stateInformation.Placed &&
                                !_stateInformation.DoorState &&
                                !_stateInformation.DockState &&
                                !_stateInformation.ClampState)
                            {
                                return LoadPortTransferStates.ReadyToLoad;
                            }

                            return LoadPortTransferStates.ReadyToUnload;
                        }

                        // 로드 PIO 진행 중이면, 캐리어가 아직 감지되지 않아도 Blocked를 유지한다.
                        // 캐리어가 정상 안착되면 내부 cause만 해제하고 상태는 계속 Blocked다.
                        if (_blockedCause == TransferBlockedCause.LoadPioInProgress)
                        {
                            if (_stateInformation.Present && _stateInformation.Placed)
                            {
                                _blockedCause = TransferBlockedCause.None;
                            }

                            return LoadPortTransferStates.TransferBlocked;
                        }

                        // 언로드 PIO 진행 중이면, COMPT/실패/복귀 입력 전까지 Blocked를 유지한다.
                        if (_blockedCause == TransferBlockedCause.UnloadPioInProgress)
                        {
                            // Manual 언로드 완료 조건:
                            // Present=false && Placed=false && Door/Dock/Clamp=false
                            // Auto는 COMPT / Fail / Returned 입력 전까지 Blocked 유지
                            if (_stateInformation.AccessMode == LoadPortAccessMode.Manual &&
                                !_stateInformation.Present &&
                                !_stateInformation.Placed &&
                                !_stateInformation.DoorState &&
                                !_stateInformation.DockState &&
                                !_stateInformation.ClampState)
                            {
                                _blockedCause = TransferBlockedCause.None;
                                return LoadPortTransferStates.ReadyToLoad;
                            }

                            return LoadPortTransferStates.TransferBlocked;
                        }

                        // E87 Transfer 전이 #9의 내부 판정 구현:
                        // 문서 정의상 READY TO UNLOAD 는 carrier 가 존재하고 unload 가능해야 한다.
                        // 또한 CarrierAccessingStatus 의 CARRIER COMPLETE / CARRIER STOPPED 는
                        // 모두 "carrier should be moved out" 의미이므로,
                        // 캐리어가 실제로 존재(Present && Placed)하고,
                        // 접근이 Complete 또는 Stopped 이며,
                        // door/dock/clamp 가 해제되면 READY TO UNLOAD 로 본다.
                        // 이 조건은 표준 상태 정의 + CAS 정의를 조합한 구현 해석이다.
                        if ((_stateInformation.CarrierAccessingState == CarrierAccessStates.CarrierCompleted ||
                             _stateInformation.CarrierAccessingState == CarrierAccessStates.CarrierStopped) &&
                            _stateInformation.Present &&
                            _stateInformation.Placed &&
                            !_stateInformation.DoorState &&
                            !_stateInformation.DockState &&
                            !_stateInformation.ClampState)
                        {
                            _blockedCause = TransferBlockedCause.None;
                            return LoadPortTransferStates.ReadyToUnload;
                        }

                        // 보조 조건:
                        // 캐리어가 제거되고 모든 해제가 끝났으면 빈 포트로 본다.
                        if (!_stateInformation.Present &&
                            !_stateInformation.Placed &&
                            !_stateInformation.DoorState &&
                            !_stateInformation.DockState &&
                            !_stateInformation.ClampState)
                        {
                            _blockedCause = TransferBlockedCause.None;
                            return LoadPortTransferStates.ReadyToLoad;
                        }

                        return LoadPortTransferStates.TransferBlocked;
                    }

                default:
                    return prevTransfer;
            }
        }

        private CarrierIdVerificationStates EvaluateCarrierId(
            CarrierIdVerificationStates prevCarrierId,
            LoadPortTransferStates nextTransfer,
            bool prevClamp,
            bool prevDock,
            E87PendingInputsSnapshot pending)
        {
            // Carrier context가 끝난 상태에서는 검증 상태를 초기화한다.
            if (ShouldResetCarrierVerification(nextTransfer))
            {
                return CarrierIdVerificationStates.NotRead;
            }

            // ReadyToUnload는 동일 carrier가 unload 대기 중인 상태이므로 검증 결과를 유지한다.
            if (nextTransfer == LoadPortTransferStates.ReadyToUnload)
            {
                return prevCarrierId;
            }

            if (nextTransfer != LoadPortTransferStates.TransferBlocked)
            {
                return CarrierIdVerificationStates.NotRead;
            }

            bool clampEdge = !prevClamp && _stateInformation.ClampState;
            bool dockEdge = !prevDock && _stateInformation.DockState;

            if (prevCarrierId == CarrierIdVerificationStates.NotRead)
            {
                if (clampEdge || dockEdge)
                {
                    // Carrier ID 전이 #6 / #7의 supplier-side shortcut:
                    // 표준은 "ID read / verification 시작" 이벤트를 요구하지만,
                    // 현재 구현은 clamp edge 또는 dock edge 를
                    // Carrier ID 상태 전이 시작 조건으로 치환한다.
                    // 정책이 Immediate 면 바로 VERIFICATION OK,
                    // WaitForHostResult 면 WAITING FOR HOST 로 진입한다.
                    return _options.CarrierIdPolicy == VerificationTransitionPolicy.Immediate
                        ? CarrierIdVerificationStates.VerificationOk
                        : CarrierIdVerificationStates.WaitingForHost;
                }
            }

            if (prevCarrierId == CarrierIdVerificationStates.WaitingForHost &&
                pending.CarrierIdResult != HostVerificationResult.None)
            {
                // Carrier ID 전이 #8 / #9:
                // WAITING FOR HOST -> ID VERIFICATION OK / FAIL
                return pending.CarrierIdResult == HostVerificationResult.Accepted
                    ? CarrierIdVerificationStates.VerificationOk
                    : CarrierIdVerificationStates.VerificationFailed;
            }

            return prevCarrierId;
        }

        private CarrierSlotMapVerificationStates EvaluateSlotMap(
            CarrierSlotMapVerificationStates prevSlotMap,
            LoadPortTransferStates nextTransfer,
            CarrierIdVerificationStates nextCarrierId,
            bool prevDoor,
            E87PendingInputsSnapshot pending)
        {
            // Carrier context가 끝난 상태에서는 검증 상태를 초기화한다.
            if (ShouldResetCarrierVerification(nextTransfer))
            {
                return CarrierSlotMapVerificationStates.NotRead;
            }

            // ReadyToUnload는 동일 carrier가 unload 대기 중인 상태이므로 검증 결과를 유지한다.
            if (nextTransfer == LoadPortTransferStates.ReadyToUnload)
            {
                return prevSlotMap;
            }

            if (nextTransfer != LoadPortTransferStates.TransferBlocked ||
                nextCarrierId != CarrierIdVerificationStates.VerificationOk)
            {
                return CarrierSlotMapVerificationStates.NotRead;
            }

            bool doorOpenEdge = !prevDoor && _stateInformation.DoorState;
            if (prevSlotMap == CarrierSlotMapVerificationStates.NotRead)
            {
                if (doorOpenEdge)
                {
                    // Slot Map 전이 #13 / #14의 supplier-side shortcut:
                    // 표준은 "slot map read / verification 시작" 이벤트를 요구하지만,
                    // 현재 구현은 door open edge 를
                    // Slot Map 상태 전이 시작 조건으로 치환한다.
                    // 정책이 Immediate 면 바로 VERIFICATION OK,
                    // 아니면 WAITING FOR HOST 로 진입한다.
                    return _options.SlotMapPolicy == VerificationTransitionPolicy.Immediate
                        ? CarrierSlotMapVerificationStates.VerificationOk
                        : CarrierSlotMapVerificationStates.WaitingForHost;
                }
            }

            if (prevSlotMap == CarrierSlotMapVerificationStates.WaitingForHost &&
                pending.SlotMapResult != HostVerificationResult.None)
            {
                // Slot Map 전이 #15 / #16:
                // WAITING FOR HOST -> SLOT MAP VERIFICATION OK / FAIL
                return pending.SlotMapResult == HostVerificationResult.Accepted
                    ? CarrierSlotMapVerificationStates.VerificationOk
                    : CarrierSlotMapVerificationStates.VerificationFailed;
            }

            return prevSlotMap;
        }
        private bool ShouldResetCarrierVerification(
            LoadPortTransferStates nextTransfer)
        {
            switch (nextTransfer)
            {
                case LoadPortTransferStates.OutOfService:
                case LoadPortTransferStates.InService:
                case LoadPortTransferStates.ReadyToLoad:
                    return true;

                default:
                    return false;
            }
        }
        #endregion </Methods>
    }

    /// <summary>
    /// hot path 상태머신이 아니라 서비스 입력으로만 바뀌는 느린 논리 상태.
    ///
    /// E87 Reservation 전이를 반영한다.
    /// - ReserveAtPort: NOT RESERVED -> RESERVED
    /// - CancelReservationAtPort: RESERVED -> NOT RESERVED
    /// - Bind: NOT RESERVED -> RESERVED
    /// - CancelBind: RESERVED -> NOT RESERVED
    /// </summary>
    public sealed class ReservationModel
    {
        public ReservationStates State { get; private set; }

        public void Initialize()
        {
            State = ReservationStates.NotReserved;
        }

        public void Reset()
        {
            State = ReservationStates.NotReserved;
        }

        public bool ReserveAtPort()
        {
            if (State == ReservationStates.Reserved)
                return false;

            State = ReservationStates.Reserved;
            return true;
        }

        public bool CancelReservationAtPort()
        {
            if (State == ReservationStates.NotReserved)
                return false;

            State = ReservationStates.NotReserved;
            return true;
        }
    }

    /// <summary>
    /// hot path 상태머신이 아니라 서비스 입력으로만 바뀌는 느린 논리 상태.
    ///
    /// 현재 구현은
    /// - Association 상태(NotAssociated / Associated)
    /// - 연계된 CarrierId
    /// 를 함께 관리한다.
    /// Bind / CancelBind 의미를 직접 반영하기 위한 최소 모델이다.
    /// </summary>
    public sealed class AssociationModel
    {
        public AssociationStates State { get; private set; }
        public string CarrierId { get; private set; }

        public void Initialize()
        {
            State = AssociationStates.NotAssociated;
            CarrierId = null;
        }

        public void Reset()
        {
            State = AssociationStates.NotAssociated;
            CarrierId = null;
        }

        public bool Bind(string carrierId)
        {
            if (State == AssociationStates.Associated &&
                string.Equals(CarrierId, carrierId, StringComparison.Ordinal))
            {
                return false;
            }

            State = AssociationStates.Associated;
            CarrierId = carrierId;
            return true;
        }

        public bool Unbind()
        {
            if (State == AssociationStates.NotAssociated &&
                string.IsNullOrEmpty(CarrierId))
            {
                return false;
            }

            State = AssociationStates.NotAssociated;
            CarrierId = null;
            return true;
        }
    }
}