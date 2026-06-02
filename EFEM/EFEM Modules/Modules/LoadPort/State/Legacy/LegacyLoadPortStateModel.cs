using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using EFEM.Defines.LoadPort;
using EFEM.Modules.LoadPort.Recovery;

using Legacy.TransferStateOnly;
using Legacy.CarrierIdStateOnly;
using Legacy.CarrierSlotMapStateOnly;

namespace EFEM.Modules.LoadPort.State
{
    public sealed class LegacyLoadPortStateModel : ILoadPortStateModel
    {
        public LegacyLoadPortStateModel(int portId, VerificationTransitionOptions options = null)
        {
            _portId = portId;
            _options = options ?? new VerificationTransitionOptions();

            _stateInformation = new LoadPortStateInformation();

            _transferStateTransitioner = new TransferState(_portId, new OutOfService(_portId), _stateInformation);
            _carrierIdStateTransitioner = new CarrierIdState(
                _portId, new Legacy.CarrierIdStateOnly.IdNotRead(_portId),
                _stateInformation);
            _carrierSlotMapTransitioner = new CarrierSlotMapState(
                _portId, new Legacy.CarrierSlotMapStateOnly.IdNotRead(_portId),
                _stateInformation);
        }

        #region <Fields>
        private readonly int _portId;
        private readonly LoadPortStateInformation _stateInformation;
        private readonly TransferState _transferStateTransitioner;
        private readonly CarrierIdState _carrierIdStateTransitioner;
        private readonly CarrierSlotMapState _carrierSlotMapTransitioner;
        private readonly VerificationTransitionOptions _options;
        private readonly object _sync = new object();

        // verification + transfer/service/PIO 입력을 모두 하나의 mailbox에서 관리한다.
        // 의미:
        // - CarrierId / SlotMap 승인·거절
        // - ChangeServiceStatus
        // - PIO READY / COMPT
        // - TransferFailed / CarrierReCreate / CarrierReturnedToPort
        // 를 모두 ApplyExternalInput()로 기록하고 Evaluate()에서 한 번에 소비한다.
        private readonly E87PendingInputs _pendingInputs = new E87PendingInputs();
        #endregion </Fields>

        #region <Properties>
        public int PortId
        {
            get { return _portId; }
        }

        public LoadPortTransferStates TransferState
        {
            get { return _transferStateTransitioner.CurrentTransferState; }
        }

        public CarrierIdVerificationStates CarrierIdVerificationState
        {
            get { return _carrierIdStateTransitioner.CurrentCarrierIdState; }
        }

        public CarrierSlotMapVerificationStates CarrierSlotMapVerificationState
        {
            get { return _carrierSlotMapTransitioner.CurrentCarrierSlotMapState; }
        }
        public bool SupportsReservationState
        {
            get { return false; }
        }

        public bool SupportsAssociationState
        {
            get { return false; }
        }

        public ReservationStates ReservationState
        {
            get { return _stateInformation.ReservationState; }
        }

        public AssociationStates AssociationState
        {
            get { return _stateInformation.AssociationState; }
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
                _transferStateTransitioner.InitState();
                _carrierIdStateTransitioner.InitState();
                _carrierSlotMapTransitioner.InitState();

                // verification 결과 + transfer/service/PIO 입력을 한 번에 초기화한다.
                _pendingInputs.Clear();

                _stateInformation.TransferState = _transferStateTransitioner.CurrentTransferState;
                _stateInformation.CarrierIdVerificationState = _carrierIdStateTransitioner.CurrentCarrierIdState;
                _stateInformation.CarrierSlotMapVerificationState = _carrierSlotMapTransitioner.CurrentCarrierSlotMapState;
                _stateInformation.ReservationState = ReservationStates.Unknown;
                _stateInformation.AssociationState = AssociationStates.Unknown;
                _stateInformation.AssociatedCarrierId = null;
            }
        }

        public void Reset()
        {
            lock (_sync)
            {
                _transferStateTransitioner.InitState();
                _carrierIdStateTransitioner.InitState();
                _carrierSlotMapTransitioner.InitState();

                // reset 이후 이전 cycle 입력이 남아있지 않도록 모두 제거한다.
                _pendingInputs.Clear();

                _stateInformation.TransferState = _transferStateTransitioner.CurrentTransferState;
                _stateInformation.CarrierIdVerificationState = _carrierIdStateTransitioner.CurrentCarrierIdState;
                _stateInformation.CarrierSlotMapVerificationState = _carrierSlotMapTransitioner.CurrentCarrierSlotMapState;
                _stateInformation.ReservationState = ReservationStates.Unknown;
                _stateInformation.AssociationState = AssociationStates.Unknown;
                _stateInformation.ReservationState = ReservationStates.Unknown;
            }
        }
        public LoadPortRecoveryData CreateRecoveryData()
        {
            // 추후 필요시 구현
            return null;
        }
        public void RecoverFromObservation(LoadPortRecoveryData recoveryData, in LoadPortObservation observation)
        {
            // 추후 필요시 구현
        }
        public void UpdateObservation(in LoadPortObservation observation)
        {
            lock (_sync)
            {
                _stateInformation.Enabled = observation.Enabled;
                _stateInformation.Initialized = observation.Initialized;
                _stateInformation.Present = observation.Present;
                _stateInformation.Placed = observation.Placed;
                _stateInformation.IsPlacementMismatch = observation.IsPlacementMismatch;
                _stateInformation.ClampState = observation.ClampState;
                _stateInformation.DockState = observation.DockState;
                _stateInformation.DoorState = observation.DoorState;
                _stateInformation.PlacementErrorState = observation.PlacementErrorState;
                _stateInformation.CarrierOutErrorState = observation.CarrierOutErrorState;
                _stateInformation.TriggeredAlarm = observation.TriggeredAlarm;
                _stateInformation.AccessMode = observation.AccessMode;
                _stateInformation.LoadingType = observation.LoadingType;
                _stateInformation.CarrierAccessingState = observation.CarrierAccessingState;
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
                        _pendingInputs.AddTransferTrigger(TransferTrigger.LoadStartedByPioReady);
                        break;

                    case LoadPortExternalInputType.UnloadTransferStartedByPioReady:
                        // Transfer 전이 #7:
                        // READY TO UNLOAD -> TRANSFER BLOCKED
                        _pendingInputs.AddTransferTrigger(TransferTrigger.UnloadStartedByPioReady);
                        break;

                    case LoadPortExternalInputType.UnloadTransferCompletedByPioCompt:
                        // Transfer 전이 #8:
                        // TRANSFER BLOCKED -> READY TO LOAD
                        _pendingInputs.AddTransferTrigger(TransferTrigger.UnloadCompletedByPioCompt);
                        break;

                    case LoadPortExternalInputType.TransferFailed:
                        // Transfer 전이 #10:
                        // TRANSFER BLOCKED -> TRANSFER READY
                        // legacy/public 상태는 TransferReady를 직접 갖지 않으므로
                        // 내부적으로 ReadyToLoad 또는 ReadyToUnload로 평탄화 처리
                        _pendingInputs.AddTransferTrigger(TransferTrigger.TransferFailed);
                        break;

                    case LoadPortExternalInputType.CarrierReCreateIssued:
                        // Transfer 전이 #7의 원인 중 하나
                        _pendingInputs.AddTransferTrigger(TransferTrigger.CarrierReCreateIssued);
                        break;

                    case LoadPortExternalInputType.CarrierReturnedToPort:
                        // Transfer 전이 #9의 구현용 축약 입력
                        _pendingInputs.AddTransferTrigger(TransferTrigger.CarrierReturnedToPort);
                        break;
                }
            }
        }
        public bool CanChangeAccessMode(LoadPortAccessMode targetMode)
        {
            return true;
        }
        public void CopyStateTo(LoadPortStateInformation state)
        {
            lock (_sync)
            {
                _stateInformation.CopyTo(ref state);
            }
        }

        public bool Evaluate()
        {
            TransferStateChangedEvent transferEvent = default(TransferStateChangedEvent);
            CarrierIdStateChangedEvent carrierIdEvent = default(CarrierIdStateChangedEvent);
            CarrierSlotMapStateChangedEvent slotMapEvent = default(CarrierSlotMapStateChangedEvent);

            bool raiseTransferEvent = false;
            bool raiseCarrierIdEvent = false;
            bool raiseSlotMapEvent = false;

            TransferStateChangedHandler transferHandler = TransferStateChanged;
            CarrierIdStateChangedHandler carrierIdHandler = CarrierIdStateChanged;
            CarrierSlotMapStateChangedHandler slotMapHandler = CarrierSlotMapStateChanged;

            bool changed = false;

            lock (_sync)
            {
                var prevTransfer = TransferState;
                var prevCarrierId = CarrierIdVerificationState;
                var prevSlotMap = CarrierSlotMapVerificationState;

                // verification 결과 + transfer/service/PIO 입력을
                // 이번 Evaluate 한 사이클 동안만 사용할 snapshot으로 꺼낸다.
                var pending = _pendingInputs.Consume();

                _transferStateTransitioner.TransitState(_stateInformation, pending);

                _carrierIdStateTransitioner.TransitState(
                    TransferState,
                    _stateInformation,
                    _options.CarrierIdPolicy);

                if (_carrierIdStateTransitioner.CurrentCarrierIdState == CarrierIdVerificationStates.WaitingForHost
                    && pending.CarrierIdResult != HostVerificationResult.None)
                {
                    // Carrier ID 전이 #8 / #9:
                    // WAITING FOR HOST -> ID VERIFICATION OK / FAIL
                    // 의미:
                    // 같은 Evaluate 안에서 WaitingForHost 진입 후 결과가 이미 들어와 있으면 즉시 소비 가능
                    _carrierIdStateTransitioner.CompleteByHost(
                        pending.CarrierIdResult == HostVerificationResult.Accepted);
                }

                _carrierSlotMapTransitioner.TransitState(
                    TransferState,
                    CarrierIdVerificationState,
                    _stateInformation,
                    _options.SlotMapPolicy);

                if (_carrierSlotMapTransitioner.CurrentCarrierSlotMapState == CarrierSlotMapVerificationStates.WaitingForHost
                    && pending.SlotMapResult != HostVerificationResult.None)
                {
                    // Slot Map 전이 #15 / #16:
                    // WAITING FOR HOST -> SLOT MAP VERIFICATION OK / FAIL
                    _carrierSlotMapTransitioner.CompleteByHost(
                        pending.SlotMapResult == HostVerificationResult.Accepted);
                }

                _stateInformation.TransferState = TransferState;
                _stateInformation.CarrierIdVerificationState = CarrierIdVerificationState;
                _stateInformation.CarrierSlotMapVerificationState = CarrierSlotMapVerificationState;
                _stateInformation.ReservationState = ReservationStates.Unknown;
                _stateInformation.AssociationState = AssociationStates.Unknown;
                _stateInformation.ReservationState = ReservationStates.Unknown;

                if (prevTransfer != TransferState)
                {
                    changed = true;

                    if (transferHandler != null)
                    {
                        transferEvent = new TransferStateChangedEvent
                        {
                            PortId = _portId,
                            PreviousState = prevTransfer,
                            CurrentState = TransferState
                        };
                        raiseTransferEvent = true;
                    }
                }

                if (prevCarrierId != CarrierIdVerificationState)
                {
                    changed = true;

                    if (carrierIdHandler != null)
                    {
                        carrierIdEvent = new CarrierIdStateChangedEvent
                        {
                            PortId = _portId,
                            PreviousState = prevCarrierId,
                            CurrentState = CarrierIdVerificationState
                        };
                        raiseCarrierIdEvent = true;
                    }
                }

                if (prevSlotMap != CarrierSlotMapVerificationState)
                {
                    changed = true;

                    if (slotMapHandler != null)
                    {
                        slotMapEvent = new CarrierSlotMapStateChangedEvent
                        {
                            PortId = _portId,
                            PreviousState = prevSlotMap,
                            CurrentState = CarrierSlotMapVerificationState
                        };
                        raiseSlotMapEvent = true;
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

            return changed;
        }
        #endregion </Methods>
    }
}

namespace Legacy.TransferStateOnly
{
    using EFEM.Modules.LoadPort.State;

    public class TransferState
    {
        public TransferState(int portId, BaseTransferState initialState, LoadPortStateInformation initInfo)
        {
            PortId = portId;

            _outOfServiceState = new OutOfService(PortId);
            _inServiceState = new InService(PortId);
            _transferBlockedState = new TransferBlocked(PortId);
            _readyToLoadState = new ReadyToLoad(PortId);
            _readyToUnloadState = new ReadyToUnload(PortId);

            _currentState = initialState;
            _currentInformation = new LoadPortStateInformation();

            initInfo.CopyTo(ref _currentInformation);
            CurrentTransferState = _currentState.StateName;
        }

        protected BaseTransferState _currentState;
        protected LoadPortStateInformation _currentInformation;

        protected readonly int PortId;

        private readonly BaseTransferState _outOfServiceState;
        private readonly BaseTransferState _inServiceState;
        private readonly BaseTransferState _transferBlockedState;
        private readonly BaseTransferState _readyToLoadState;
        private readonly BaseTransferState _readyToUnloadState;

        public LoadPortStateInformation CurrentStateInformation
        {
            get
            {
                return _currentInformation;
            }
        }

        public LoadPortTransferStates CurrentTransferState { get; private set; }

        public void InitState()
        {
            if (!(_currentState is OutOfService))
            {
                MoveToOutOfService();
            }
            else
            {
                CurrentTransferState = _currentState.StateName;
            }
        }

        public void TransitState(
            LoadPortStateInformation newInfo,
            E87PendingInputsSnapshot pending)
        {
            // E87 Transfer #3:
            // 어떤 상태에서든 OutOfService 요청이 들어오면 최우선 처리
            if (pending.ServiceStatusChange == ServiceStatusChangeRequest.ToOutOfService)
            {
                MoveToOutOfService();
                newInfo.CopyTo(ref _currentInformation);
                CurrentTransferState = _currentState.StateName;
                return;
            }

            if (!newInfo.Enabled)
            {
                InitState();
            }
            else
            {
                _currentState.TransitState(this, newInfo, pending);
            }

            newInfo.CopyTo(ref _currentInformation);
            CurrentTransferState = _currentState.StateName;
        }

        public void SetState(BaseTransferState newState)
        {
            if (_currentState.GetType() != newState.GetType())
            {
                System.Console.WriteLine(string.Format("Transit State : {0} -> {1}", _currentState.GetType().Name, newState.GetType().Name));
                _currentState = newState;
            }
        }

        public void MoveToOutOfService()
        {
            SetState(_outOfServiceState);
            CurrentTransferState = _currentState.StateName;
        }

        public void MoveToInService()
        {
            SetState(_inServiceState);
            CurrentTransferState = _currentState.StateName;
        }

        public void MoveToTransferBlocked()
        {
            SetState(_transferBlockedState);
            CurrentTransferState = _currentState.StateName;
        }

        public void MoveToReadyToLoad()
        {
            SetState(_readyToLoadState);
            CurrentTransferState = _currentState.StateName;
        }

        public void MoveToReadyToUnload()
        {
            SetState(_readyToUnloadState);
            CurrentTransferState = _currentState.StateName;
        }
    }

    public abstract class BaseTransferState
    {
        #region <Constructors>
        public BaseTransferState(int portId /*, LoadPortStateInformation initialInfo*/)
        {
            PortId = portId;
        }
        #endregion </Constructors>

        #region <Fields>
        protected readonly int PortId;
        #endregion </Fields>

        #region <Properties>
        public LoadPortTransferStates StateName { get; protected set; }
        #endregion </Properties>

        #region <Methods>
        public abstract void TransitState(
            TransferState newState,
            LoadPortStateInformation newInfo,
            E87PendingInputsSnapshot pending);

        #region <Check loadport status>
        // 캐리어가 정확히 놓여있다.(Placed, Present)
        protected bool IsCarrierCorrectlyPlaced(LoadPortStateInformation info)
        {
            return info.Placed && info.Present;
        }
        // 캐리어가 완벽히 제거되었다.
        protected bool IsCarrierRemoved(LoadPortStateInformation info)
        {
            return !info.Placed && !info.Present;
        }

        // 클램핑 중인 상태이다.
        protected bool IsCurrentlyClampingStatus(TransferState currentState, LoadPortStateInformation newInfo)
        {
            if (currentState.CurrentStateInformation.ClampState != newInfo.ClampState)
            {
                return newInfo.ClampState;
            }

            return false;
        }

        protected bool IsCarrierStoppedOrCompleted(LoadPortStateInformation info)
        {
            if ((info.CarrierAccessingState.Equals(CarrierAccessStates.CarrierCompleted) ||
                info.CarrierAccessingState.Equals(CarrierAccessStates.CarrierStopped)) &&
                false == info.DoorState &&
                false == info.DockState &&
                false == info.ClampState)
            {
                return true;
            }

            return false;
        }

        // 문이 열린 상태이다.
        protected bool IsCurrentlyOpeningStatus(TransferState currentState, LoadPortStateInformation newInfo)
        {
            if (currentState.CurrentStateInformation.DoorState != newInfo.DoorState)
            {
                return newInfo.DoorState;
            }

            return false;
        }

        // 캐리어가 로딩되는 중이다.
        protected bool IsCurrentlyLoadingStatus(TransferState currentState, LoadPortStateInformation newInfo)
        {
            if (currentState.CurrentStateInformation.DockState != newInfo.DockState)
            {
                // 도킹이 해제되는 중이다.
                return newInfo.DockState;
            }

            return false;
        }

        // 캐리어가 언로딩되는 중이다.
        protected bool IsCurrentlyUnloadingStatus(TransferState currentState, LoadPortStateInformation newInfo)
        {
            // 2024.07.04. jhlim [MOD] 홈이 안 잡힌 상태에서는 체크하지 않는다.
            if (currentState.CurrentStateInformation.DockState != newInfo.DockState &&
                newInfo.Initialized &&
                (newInfo.CarrierAccessingState.Equals(CarrierAccessStates.CarrierCompleted) ||
                newInfo.CarrierAccessingState.Equals(CarrierAccessStates.CarrierStopped)))
            {
                // 도킹이 해제되는 중이다.
                return (false == newInfo.DockState);
            }

            return false;
        }
        #endregion </Check loadport status>

        #endregion </Methods>
    }

    public class OutOfService : BaseTransferState
    {
        public OutOfService(int portId /*, LoadPortStateInformation initialInfo*/) : base(portId/*, initialInfo*/)
        {
            StateName = LoadPortTransferStates.OutOfService;
        }

        public override void TransitState(
            TransferState newState,
            LoadPortStateInformation newInfo,
            E87PendingInputsSnapshot pending)
        {
            // E87 Transfer #2:
            // OUT OF SERVICE -> IN SERVICE
            if (pending.ServiceStatusChange == ServiceStatusChangeRequest.ToInService)
            {
                newState.MoveToInService();
                return;
            }

            // legacy 기존 의미 유지
            if (newInfo.Initialized)
            {
                newState.MoveToInService();
            }
        }
    }

    public class InService : BaseTransferState
    {
        public InService(int portId /*, LoadPortStateInformation initialInfo*/) : base(portId/*, initialInfo*/)
        {
            StateName = LoadPortTransferStates.InService;
        }

        public override void TransitState(
            TransferState newState,
            LoadPortStateInformation newInfo,
            E87PendingInputsSnapshot pending)
        {
            if (pending.ServiceStatusChange == ServiceStatusChangeRequest.ToOutOfService)
            {
                newState.MoveToOutOfService();
                return;
            }

            if (IsCarrierCorrectlyPlaced(newInfo))
            {
                newState.MoveToTransferBlocked();
            }
            else
            {
                newState.MoveToReadyToLoad();
            }
        }
    }

    public class TransferBlocked : BaseTransferState
    {
        public TransferBlocked(int portId /*, LoadPortStateInformation initialInfo*/) : base(portId/*, initialInfo*/)
        {
            StateName = LoadPortTransferStates.TransferBlocked;
        }

        public override void TransitState(
            TransferState newState,
            LoadPortStateInformation newInfo,
            E87PendingInputsSnapshot pending)
        {
            // E87 Transfer #8:
            // TRANSFER BLOCKED -> READY TO LOAD
            if (pending.HasTransferTrigger(TransferTrigger.UnloadCompletedByPioCompt))
            {
                newState.MoveToReadyToLoad();
                return;
            }

            // E87 Transfer #9 구현용 축약:
            // TRANSFER BLOCKED -> READY TO UNLOAD
            if (pending.HasTransferTrigger(TransferTrigger.CarrierReturnedToPort))
            {
                newState.MoveToReadyToUnload();
                return;
            }

            // E87 Transfer #10:
            // TRANSFER BLOCKED -> TRANSFER READY
            // legacy/public 상태는 TransferReady를 직접 갖지 않으므로 평탄화 처리
            if (pending.HasTransferTrigger(TransferTrigger.TransferFailed))
            {
                if (IsCarrierRemoved(newInfo) &&
                    false == newInfo.DoorState &&
                    false == newInfo.DockState &&
                    false == newInfo.ClampState)
                {
                    newState.MoveToReadyToLoad();
                }
                else
                {
                    newState.MoveToReadyToUnload();
                }
                return;
            }

            // legacy 기존 의미 유지
            if (IsCarrierStoppedOrCompleted(newInfo))
            {
                newState.MoveToReadyToUnload();
                return;
            }

            if (IsCarrierRemoved(newInfo) &&
                false == newInfo.DoorState &&
                false == newInfo.DockState &&
                false == newInfo.ClampState)
            {
                newState.MoveToReadyToLoad();
                return;
            }

            // 검증 진행 여부는 CarrierIdState / CarrierSlotMapState가 담당한다.
        }
    }

    public class ReadyToLoad : BaseTransferState
    {
        public ReadyToLoad(int portId /*, LoadPortStateInformation initialInfo*/) : base(portId/*, initialInfo*/)
        {
            StateName = LoadPortTransferStates.ReadyToLoad;
        }

        public override void TransitState(
            TransferState newState,
            LoadPortStateInformation newInfo,
            E87PendingInputsSnapshot pending)
        {
            // E87 Transfer #6:
            // READY TO LOAD -> TRANSFER BLOCKED
            if (pending.HasTransferTrigger(TransferTrigger.LoadStartedByPioReady))
            {
                newState.MoveToTransferBlocked();
                return;
            }

            if (IsCarrierCorrectlyPlaced(newInfo))
            {
                newState.MoveToTransferBlocked();
            }
        }
    }

    public class ReadyToUnload : BaseTransferState
    {
        public ReadyToUnload(int portId /*, LoadPortStateInformation initialInfo*/) : base(portId/*, initialInfo*/)
        {
            StateName = LoadPortTransferStates.ReadyToUnload;
        }

        public override void TransitState(
            TransferState newState,
            LoadPortStateInformation newInfo,
            E87PendingInputsSnapshot pending)
        {
            // E87 Transfer #7:
            // READY TO UNLOAD -> TRANSFER BLOCKED
            if (pending.HasTransferTrigger(TransferTrigger.UnloadStartedByPioReady))
            {
                newState.MoveToTransferBlocked();
                return;
            }

            if (pending.HasTransferTrigger(TransferTrigger.CarrierReCreateIssued))
            {
                newState.MoveToTransferBlocked();
                return;
            }

            if (IsCarrierRemoved(newInfo) &&
                false == newInfo.DockState &&
                false == newInfo.DoorState)
            {
                newState.MoveToReadyToLoad();
            }
        }
    }
}

namespace Legacy.CarrierIdStateOnly
{
    public class CarrierIdState
    {
        public CarrierIdState(int portId, BaseCarrierIdState initialState, LoadPortStateInformation initInfo)
        {
            PortId = portId;

            _idNotReadState = new IdNotRead(PortId);
            _waitingForHostState = new WaitingForHost(PortId);
            _verificationOkState = new VerificationOk(PortId);
            _verificationFailedState = new VerificationFailed(PortId);

            _currentState = initialState;
            _currentInformation = new LoadPortStateInformation();

            initInfo.CopyTo(ref _currentInformation);
            CurrentCarrierIdState = _currentState.StateName;
        }

        protected BaseCarrierIdState _currentState;
        protected LoadPortStateInformation _currentInformation;
        protected readonly int PortId;

        private readonly BaseCarrierIdState _idNotReadState;
        private readonly BaseCarrierIdState _waitingForHostState;
        private readonly BaseCarrierIdState _verificationOkState;
        private readonly BaseCarrierIdState _verificationFailedState;

        public LoadPortStateInformation CurrentStateInformation
        {
            get
            {
                return _currentInformation;
            }
        }

        public CarrierIdVerificationStates CurrentCarrierIdState { get; private set; }

        public void InitState()
        {
            MoveToIdNotRead();
        }

        public void TransitState(
            LoadPortTransferStates transferState,
            LoadPortStateInformation newInfo,
            VerificationTransitionPolicy policy)
        {
            if (transferState != LoadPortTransferStates.TransferBlocked)
            {
                InitState();
            }
            else
            {
                _currentState.TransitState(this, newInfo, policy);
            }

            newInfo.CopyTo(ref _currentInformation);
            CurrentCarrierIdState = _currentState.StateName;
        }

        public void SetState(BaseCarrierIdState newState)
        {
            if (_currentState.GetType() != newState.GetType())
            {
                Console.WriteLine(string.Format("Carrier Id State : {0} -> {1}", _currentState.GetType().Name, newState.GetType().Name));
                _currentState = newState;
            }
        }

        public void MoveToIdNotRead()
        {
            SetState(_idNotReadState);
            CurrentCarrierIdState = _currentState.StateName;
        }

        public void MoveToWaitingForHost()
        {
            SetState(_waitingForHostState);
            CurrentCarrierIdState = _currentState.StateName;
        }

        public void MoveToVerificationOk()
        {
            SetState(_verificationOkState);
            CurrentCarrierIdState = _currentState.StateName;
        }

        public void MoveToVerificationFailed()
        {
            SetState(_verificationFailedState);
            CurrentCarrierIdState = _currentState.StateName;
        }

        public void CompleteByHost(bool isSuccess)
        {
            if (!(_currentState is WaitingForHost))
            {
                return;
            }

            if (isSuccess)
            {
                MoveToVerificationOk();
            }
            else
            {
                MoveToVerificationFailed();
            }
        }
    }

    public abstract class BaseCarrierIdState
    {
        #region <Constructors>
        public BaseCarrierIdState(int portId /*, LoadPortStateInformation initialInfo*/)
        {
            PortId = portId;
        }
        #endregion </Constructors>

        #region <Fields>
        protected readonly int PortId;
        #endregion </Fields>

        #region <Properties>
        public CarrierIdVerificationStates StateName { get; protected set; }
        #endregion </Properties>

        #region <Methods>
        public abstract void TransitState(
            CarrierIdState newState,
            LoadPortStateInformation newInfo,
            VerificationTransitionPolicy policy);

        #region <Check loadport status>
        // 캐리어가 정확히 놓여있다.(Placed, Present)
        protected bool IsCarrierCorrectlyPlaced(CarrierIdState currentState)
        {
            return (currentState.CurrentStateInformation.Placed && currentState.CurrentStateInformation.Present);
        }

        // 클램핑 중인 상태이다.
        protected bool IsCurrentlyClampingStatus(CarrierIdState currentState, LoadPortStateInformation newInfo)
        {
            if (currentState.CurrentStateInformation.ClampState != newInfo.ClampState)
            {
                return newInfo.ClampState;
            }

            return false;
        }

        // 캐리어가 로딩되는 중이다.
        protected bool IsCurrentlyLoadingStatus(CarrierIdState currentState, LoadPortStateInformation newInfo)
        {
            if (currentState.CurrentStateInformation.DockState != newInfo.DockState)
            {
                // 도킹이 해제되는 중이다.
                return newInfo.DockState;
            }

            return false;
        }

        // 문이 열린 상태이다.
        protected bool IsCurrentlyOpeningStatus(CarrierIdState currentState, LoadPortStateInformation newInfo)
        {
            if (currentState.CurrentStateInformation.DoorState != newInfo.DoorState)
            {
                return newInfo.DoorState;
            }

            return false;
        }

        // 캐리어가 언로딩되는 중이다.
        protected bool IsCurrentlyUnloadingStatus(CarrierIdState currentState, LoadPortStateInformation newInfo)
        {
            if (currentState.CurrentStateInformation.DockState != newInfo.DockState)
            {
                // 도킹이 해제되는 중이다.
                return (false == newInfo.DockState);
            }

            return false;
        }
        #endregion </Check loadport status>

        #endregion </Methods>
    }

    public class IdNotRead : BaseCarrierIdState
    {
        public IdNotRead(int portId) : base(portId)
        {
            StateName = CarrierIdVerificationStates.NotRead;
        }
        public override void TransitState(
            CarrierIdState newState,
            LoadPortStateInformation newInfo,
            VerificationTransitionPolicy policy)
        {
            if (IsCurrentlyClampingStatus(newState, newInfo) ||
                IsCurrentlyLoadingStatus(newState, newInfo))
            {
                if (policy == VerificationTransitionPolicy.Immediate)
                {
                    newState.MoveToVerificationOk();
                }
                else
                {
                    newState.MoveToWaitingForHost();
                }
            }
        }
    }

    public class WaitingForHost : BaseCarrierIdState
    {
        public WaitingForHost(int portId) : base(portId)
        {
            StateName = CarrierIdVerificationStates.WaitingForHost;
        }

        public override void TransitState(
            CarrierIdState newState,
            LoadPortStateInformation newInfo,
            VerificationTransitionPolicy policy)
        {
            // 실제 대기. Host 결과는 StateTransitionManager.ExecuteTransition()에서 반영한다.
        }
    }

    public class VerificationOk : BaseCarrierIdState
    {
        public VerificationOk(int portId /*, LoadPortStateInformation initialInfo*/) : base(portId/*, initialInfo*/)
        {
            StateName = CarrierIdVerificationStates.VerificationOk;
        }

        public override void TransitState(
            CarrierIdState newState,
            LoadPortStateInformation newInfo,
            VerificationTransitionPolicy policy)
        {
        }
    }

    public class VerificationFailed : BaseCarrierIdState
    {
        public VerificationFailed(int portId /*, LoadPortStateInformation initialInfo*/) : base(portId/*, initialInfo*/)
        {
            StateName = CarrierIdVerificationStates.VerificationFailed;
        }

        public override void TransitState(
            CarrierIdState newState,
            LoadPortStateInformation newInfo,
            VerificationTransitionPolicy policy)
        {
        }
    }
}

namespace Legacy.CarrierSlotMapStateOnly
{
    public class CarrierSlotMapState
    {
        public CarrierSlotMapState(int portId, BaseCarrierSlotMapState initialState, LoadPortStateInformation initInfo)
        {
            PortId = portId;

            _idNotReadState = new IdNotRead(PortId);
            _waitingForHostState = new WaitingForHost(PortId);
            _verificationOkState = new VerificationOk(PortId);
            _verificationFailedState = new VerificationFailed(PortId);

            _currentState = initialState;
            _currentInformation = new LoadPortStateInformation();

            initInfo.CopyTo(ref _currentInformation);
            CurrentCarrierSlotMapState = _currentState.StateName;
        }

        protected BaseCarrierSlotMapState _currentState;
        protected LoadPortStateInformation _currentInformation;
        protected readonly int PortId;

        private readonly BaseCarrierSlotMapState _idNotReadState;
        private readonly BaseCarrierSlotMapState _waitingForHostState;
        private readonly BaseCarrierSlotMapState _verificationOkState;
        private readonly BaseCarrierSlotMapState _verificationFailedState;

        public LoadPortStateInformation CurrentStateInformation
        {
            get
            {
                return _currentInformation;
            }
        }

        public CarrierSlotMapVerificationStates CurrentCarrierSlotMapState { get; private set; }

        public void InitState()
        {
            if (!(_currentState is IdNotRead))
            {
                MoveToIdNotRead();
            }
            else
            {
                CurrentCarrierSlotMapState = _currentState.StateName;
            }
        }

        public void TransitState(
            LoadPortTransferStates transferState,
            CarrierIdVerificationStates idState,
            LoadPortStateInformation newInfo,
            VerificationTransitionPolicy policy)
        {
            if (transferState != LoadPortTransferStates.TransferBlocked
                || idState != CarrierIdVerificationStates.VerificationOk)
            {
                InitState();
            }
            else
            {
                _currentState.TransitState(this, newInfo, policy);
            }

            newInfo.CopyTo(ref _currentInformation);
            CurrentCarrierSlotMapState = _currentState.StateName;
        }

        public void SetState(BaseCarrierSlotMapState newState)
        {
            if (_currentState.GetType() != newState.GetType())
            {
                Console.WriteLine(string.Format("SlotMap State : {0} -> {1}", _currentState.GetType().Name, newState.GetType().Name));
                _currentState = newState;
            }
        }

        public void MoveToIdNotRead()
        {
            SetState(_idNotReadState);
            CurrentCarrierSlotMapState = _currentState.StateName;
        }

        public void MoveToWaitingForHost()
        {
            SetState(_waitingForHostState);
            CurrentCarrierSlotMapState = _currentState.StateName;
        }

        public void MoveToVerificationOk()
        {
            SetState(_verificationOkState);
            CurrentCarrierSlotMapState = _currentState.StateName;
        }

        public void MoveToVerificationFailed()
        {
            SetState(_verificationFailedState);
            CurrentCarrierSlotMapState = _currentState.StateName;
        }

        public void CompleteByHost(bool isSuccess)
        {
            if (!(_currentState is WaitingForHost))
            {
                return;
            }

            if (isSuccess)
            {
                MoveToVerificationOk();
            }
            else
            {
                MoveToVerificationFailed();
            }
        }
    }

    public abstract class BaseCarrierSlotMapState
    {
        #region <Constructors>
        public BaseCarrierSlotMapState(int portId /*, LoadPortStateInformation initialInfo*/)
        {
            PortId = portId;
        }
        #endregion </Constructors>

        #region <Fields>
        protected readonly int PortId;
        #endregion </Fields>

        #region <Properties>
        public CarrierSlotMapVerificationStates StateName { get; protected set; }
        #endregion </Properties>

        #region <Methods>
        public abstract void TransitState(
            CarrierSlotMapState newState,
            LoadPortStateInformation newInfo,
            VerificationTransitionPolicy policy);

        #region <Check loadport status>
        // 캐리어가 정확히 놓여있다.(Placed, Present)
        protected bool IsCarrierCorrectlyPlaced(CarrierSlotMapState currentState)
        {
            return (currentState.CurrentStateInformation.Placed && currentState.CurrentStateInformation.Present);
        }

        // 클램핑 중인 상태이다.
        protected bool IsCurrentlyClampingStatus(CarrierSlotMapState currentState, LoadPortStateInformation newInfo)
        {
            if (currentState.CurrentStateInformation.ClampState != newInfo.ClampState)
            {
                return newInfo.ClampState;
            }

            return false;
        }

        // 문이 열린 상태이다.
        protected bool IsCurrentlyOpeningStatus(CarrierSlotMapState currentState, LoadPortStateInformation newInfo)
        {
            if (currentState.CurrentStateInformation.DoorState != newInfo.DoorState)
            {
                return newInfo.DoorState;
            }

            return false;
        }

        // 캐리어가 언로딩되는 중이다.
        protected bool IsCurrentlyUnloadingStatus(CarrierSlotMapState currentState, LoadPortStateInformation newInfo)
        {
            if (currentState.CurrentStateInformation.DockState != newInfo.DockState)
            {
                // 도킹이 해제되는 중이다.
                return (false == newInfo.DockState);
            }

            return false;
        }
        #endregion </Check loadport status>
        #endregion </Methods>
    }

    public class IdNotRead : BaseCarrierSlotMapState
    {
        public IdNotRead(int portId) : base(portId)
        {
            StateName = CarrierSlotMapVerificationStates.NotRead;
        }

        public override void TransitState(
            CarrierSlotMapState newState,
            LoadPortStateInformation newInfo,
            VerificationTransitionPolicy policy)
        {
            if (IsCurrentlyOpeningStatus(newState, newInfo))
            {
                if (policy == VerificationTransitionPolicy.Immediate)
                {
                    newState.MoveToVerificationOk();
                }
                else
                {
                    newState.MoveToWaitingForHost();
                }
            }
        }
    }

    public class WaitingForHost : BaseCarrierSlotMapState
    {
        public WaitingForHost(int portId) : base(portId)
        {
            StateName = CarrierSlotMapVerificationStates.WaitingForHost;
        }

        public override void TransitState(
            CarrierSlotMapState newState,
            LoadPortStateInformation newInfo,
            VerificationTransitionPolicy policy)
        {
            // 실제 대기. Host 결과는 StateTransitionManager.ExecuteTransition()에서 반영한다.
        }
    }

    public class VerificationOk : BaseCarrierSlotMapState
    {
        public VerificationOk(int portId /*, LoadPortStateInformation initialInfo*/) : base(portId/*, initialInfo*/)
        {
            StateName = CarrierSlotMapVerificationStates.VerificationOk;
        }

        public override void TransitState(
            CarrierSlotMapState newState,
            LoadPortStateInformation newInfo,
            VerificationTransitionPolicy policy)
        {
        }
    }

    public class VerificationFailed : BaseCarrierSlotMapState
    {
        public VerificationFailed(int portId /*, LoadPortStateInformation initialInfo*/) : base(portId/*, initialInfo*/)
        {
            StateName = CarrierSlotMapVerificationStates.VerificationFailed;
        }

        public override void TransitState(
            CarrierSlotMapState newState,
            LoadPortStateInformation newInfo,
            VerificationTransitionPolicy policy)
        {
        }
    }
}