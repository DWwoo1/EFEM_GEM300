using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using EFEM.Defines.LoadPort;
using TransferStateOnly;

namespace EFEM.Modules.LoadPort.State
{
    public class StateTransitionManager : ILoadPortStateTransitionManager
    {
        #region <Constructors>
        public StateTransitionManager(
            int portId,
            ref LoadPortStateInformation information,
            VerificationTransitionOptions options = null)
        {
            PortId = portId;
            StateInformation = information;
            _options = options ?? new VerificationTransitionOptions();

            TransferStateTransitioner = new TransferState(PortId, new OutOfService(PortId), StateInformation);
            CarrierIdStateTransitioner = new CarrierIdStateOnly.CarrierIdState(
                PortId,
                new CarrierIdStateOnly.IdNotRead(PortId),
                StateInformation);

            CarrierSlotMapTransitioner = new CarrierSlotMapStateOnly.CarrierSlotMapState(
                PortId,
                new CarrierSlotMapStateOnly.IdNotRead(PortId),
                StateInformation);
        }
        #endregion </Constructors>

        #region <Fields>
        private readonly int PortId;
        private readonly LoadPortStateInformation StateInformation = null;
        private readonly TransferState TransferStateTransitioner = null;
        private readonly CarrierIdStateOnly.CarrierIdState CarrierIdStateTransitioner = null;
        private readonly CarrierSlotMapStateOnly.CarrierSlotMapState CarrierSlotMapTransitioner = null;
        private readonly VerificationTransitionOptions _options = null;
        private readonly object _sync = new object();

        private HostVerificationResult _pendingCarrierIdResult = HostVerificationResult.None;
        private HostVerificationResult _pendingSlotMapResult = HostVerificationResult.None;
        #endregion </Fields>

        #region <Properties>
        public LoadPortTransferStates TransferState
        {
            get
            {
                return TransferStateTransitioner.CurrentTransferState;
            }
        }

        public CarrierIdVerificationStates CarrierIdVerificationState
        {
            get
            {
                return CarrierIdStateTransitioner.CurrentCarrierIdState;
            }
        }

        public CarrierSlotMapVerificationStates CarrierSlotMapVerificationState
        {
            get
            {
                return CarrierSlotMapTransitioner.CurrentCarrierSlotMapState;
            }
        }
        #endregion </Properties>

        #region <Events>
        public event EventHandler<TransferStateChangedEventArgs> TransferStateChanged;
        public event EventHandler<CarrierIdVerificationStateChangedEventArgs> CarrierIdVerificationStateChanged;
        public event EventHandler<CarrierSlotMapVerificationStateChangedEventArgs> CarrierSlotMapVerificationStateChanged;
        #endregion </Events>

        #region <Methods>
        public void InitTransferState()
        {
            lock (_sync)
            {
                TransferStateTransitioner.InitState();
                _pendingCarrierIdResult = HostVerificationResult.None;
                _pendingSlotMapResult = HostVerificationResult.None;
            }
        }

        public void PostCarrierIdVerificationResult(bool isSuccess)
        {
            lock (_sync)
            {
                _pendingCarrierIdResult = isSuccess
                    ? HostVerificationResult.Accepted
                    : HostVerificationResult.Rejected;
            }
        }

        public void PostCarrierSlotMapVerificationResult(bool isSuccess)
        {
            lock (_sync)
            {
                _pendingSlotMapResult = isSuccess
                    ? HostVerificationResult.Accepted
                    : HostVerificationResult.Rejected;
            }
        }

        public void ExecuteTransition()
        {
            TransferStateChangedEventArgs transferArgs = null;
            CarrierIdVerificationStateChangedEventArgs carrierIdArgs = null;
            CarrierSlotMapVerificationStateChangedEventArgs slotMapArgs = null;

            lock (_sync)
            {
                var prevTransfer = TransferState;
                var prevCarrierId = CarrierIdVerificationState;
                var prevSlotMap = CarrierSlotMapVerificationState;

                TransferStateTransitioner.TransitState(StateInformation);

                CarrierIdStateTransitioner.TransitState(
                    TransferState,
                    StateInformation,
                    _options.CarrierIdPolicy);

                if (CarrierIdStateTransitioner.CurrentCarrierIdState == CarrierIdVerificationStates.WaitingForHost
                    && _pendingCarrierIdResult != HostVerificationResult.None)
                {
                    CarrierIdStateTransitioner.CompleteByHost(
                        _pendingCarrierIdResult == HostVerificationResult.Accepted);

                    _pendingCarrierIdResult = HostVerificationResult.None;
                }

                CarrierSlotMapTransitioner.TransitState(
                    TransferState,
                    CarrierIdVerificationState,
                    StateInformation,
                    _options.SlotMapPolicy);

                if (CarrierSlotMapTransitioner.CurrentCarrierSlotMapState == CarrierSlotMapVerificationStates.WaitingForHost
                    && _pendingSlotMapResult != HostVerificationResult.None)
                {
                    CarrierSlotMapTransitioner.CompleteByHost(
                        _pendingSlotMapResult == HostVerificationResult.Accepted);

                    _pendingSlotMapResult = HostVerificationResult.None;
                }

                if (prevTransfer != TransferState)
                {
                    transferArgs = new TransferStateChangedEventArgs(PortId, prevTransfer, TransferState);
                }

                if (prevCarrierId != CarrierIdVerificationState)
                {
                    carrierIdArgs = new CarrierIdVerificationStateChangedEventArgs(
                        PortId,
                        prevCarrierId,
                        CarrierIdVerificationState);
                }

                if (prevSlotMap != CarrierSlotMapVerificationState)
                {
                    slotMapArgs = new CarrierSlotMapVerificationStateChangedEventArgs(
                        PortId,
                        prevSlotMap,
                        CarrierSlotMapVerificationState);
                }
            }

            if (transferArgs != null)
            {
                TransferStateChanged?.Invoke(this, transferArgs);
            }

            if (carrierIdArgs != null)
            {
                CarrierIdVerificationStateChanged?.Invoke(this, carrierIdArgs);
            }

            if (slotMapArgs != null)
            {
                CarrierSlotMapVerificationStateChanged?.Invoke(this, slotMapArgs);
            }
        }
        #endregion </Methods>
    }
}

namespace TransferStateOnly
{
    public class TransferState
    {
        public TransferState(int portId, BaseTransferState initialState, LoadPortStateInformation initInfo)
        {
            PortId = portId;
            _currentState = initialState;
            _currentInformation = new LoadPortStateInformation();

            initInfo.CopyTo(ref _currentInformation);
            CurrentTransferState = _currentState.StateName;
        }

        protected BaseTransferState _currentState;
        protected LoadPortStateInformation _currentInformation;

        protected readonly int PortId;

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
                _currentState = new OutOfService(PortId);
            }

            CurrentTransferState = _currentState.StateName;
        }

        public void TransitState(LoadPortStateInformation newInfo)
        {
            if (!newInfo.Enabled)
            {
                InitState();
            }
            else
            {
                _currentState.TransitState(this, newInfo);
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
        public abstract void TransitState(TransferState newState, LoadPortStateInformation newInfo);

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

        public override void TransitState(TransferState newState, LoadPortStateInformation newInfo)
        {
            if (newInfo.Initialized)
            {
                newState.SetState(new InService(PortId));
            }
        }
    }

    public class InService : BaseTransferState
    {
        public InService(int portId /*, LoadPortStateInformation initialInfo*/) : base(portId/*, initialInfo*/)
        {
            StateName = LoadPortTransferStates.InService;
        }

        public override void TransitState(TransferState newState, LoadPortStateInformation newInfo)
        {
            if (IsCarrierCorrectlyPlaced(newInfo))
            {
                newState.SetState(new TransferBlocked(PortId));
            }
            else
            {
                newState.SetState(new ReadyToLoad(PortId));
            }
        }
    }

    public class TransferBlocked : BaseTransferState
    {
        public TransferBlocked(int portId /*, LoadPortStateInformation initialInfo*/) : base(portId/*, initialInfo*/)
        {
            StateName = LoadPortTransferStates.TransferBlocked;
        }

        private int _seqNum;

        public override void TransitState(TransferState newState, LoadPortStateInformation newInfo)
        {
            // 2025.09.08. jhlim [MOD] 메서드로 변경 -> AccessingStatus가 Stopped/Completed이면서, 캐리어가 배출 준비(언로드) 완료된 상태
            // 기존에는 언로딩 되던 상태를 체크했었다.
            if (IsCarrierStoppedOrCompleted(newInfo))
            {
                newState.SetState(new ReadyToUnload(PortId));
            }
            // TransferBlocked에서 캐리어가 제거되는 경우(언로딩된 상태) ReadyToLoad로 전이한다.
            else if (IsCarrierRemoved(newInfo) &&
                (false == newInfo.DoorState &&
                 false == newInfo.DockState &&
                 false == newInfo.ClampState))       // 제거되었고, 문이 열리고, 언도킹되었고, 언클램핑 상태면 상태 변경
            {
                newState.SetState(new ReadyToLoad(PortId));
            }
            else
            {
                switch (_seqNum)
                {
                    case 0:
                        if (false == IsCurrentlyClampingStatus(newState, newInfo) &&
                            false == IsCurrentlyLoadingStatus(newState, newInfo))
                        {
                            break;
                        }

                        ++_seqNum; break;
                    case 1:
                        // TODO : Carrier Id Verification
                        // OK 될 때까지 태스크에서는 기다려야 한다.
                        // 태스크에서는 OK 되면 도킹, Failed 면 에러
                        break;
                    case 2:
                        // TODO : Carrier Slot Map Verification
                        // OK 될 때까지 태스크에서는 기다려야 한다.
                        // 태스크에서는 이후 작업 진행, 여기선 default or 다음 스텝에서 계속 리턴
                        break;
                    default:
                        break;
                }
            }
        }
    }

    public class ReadyToLoad : BaseTransferState
    {
        public ReadyToLoad(int portId /*, LoadPortStateInformation initialInfo*/) : base(portId/*, initialInfo*/)
        {
            StateName = LoadPortTransferStates.ReadyToLoad;
        }

        public override void TransitState(TransferState newState, LoadPortStateInformation newInfo)
        {
            if (IsCarrierCorrectlyPlaced(newInfo))
            {
                newState.SetState(new TransferBlocked(PortId));
            }
        }
    }

    public class ReadyToUnload : BaseTransferState
    {
        public ReadyToUnload(int portId /*, LoadPortStateInformation initialInfo*/) : base(portId/*, initialInfo*/)
        {
            StateName = LoadPortTransferStates.ReadyToUnload;
        }

        public override void TransitState(TransferState newState, LoadPortStateInformation newInfo)
        {

            // 2025.09.08. jhlim [MOD] CorrectlyPlaced는 Present와 Place가 모두 On 된 상태인데, 둘 중 하나만 Off인 비정상 상황에도 ReadyToLoad로 넘어가던 조건 개선
            //if (false == IsCarrierCorrectlyPlaced(newState) &&
            if (IsCarrierRemoved(newInfo) &&
                false == newInfo.DockState &&
                false == newInfo.DoorState)
            {
                newState.SetState(new ReadyToLoad(PortId));
            }
            // 2025.09.08. jhlim [END]
        }
    }
}

namespace CarrierIdStateOnly
{
    public class CarrierIdState
    {
        public CarrierIdState(int portId, BaseCarrierIdState initialState, LoadPortStateInformation initInfo)
        {
            PortId = portId;
            _currentState = initialState;
            _currentInformation = new LoadPortStateInformation();

            initInfo.CopyTo(ref _currentInformation);
            CurrentCarrierIdState = _currentState.StateName;
        }

        protected BaseCarrierIdState _currentState;
        protected LoadPortStateInformation _currentInformation;
        protected readonly int PortId;

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
            if (false == (_currentState is IdNotRead))
            {
                _currentState = new IdNotRead(PortId);
            }

            CurrentCarrierIdState = _currentState.StateName;
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

        public void CompleteByHost(bool isSuccess)
        {
            if (!(_currentState is WaitingForHost))
            {
                return;
            }

            SetState(isSuccess
                ? (BaseCarrierIdState)new VerificationOk(PortId)
                : new VerificationFailed(PortId));

            CurrentCarrierIdState = _currentState.StateName;
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
                    newState.SetState(new VerificationOk(PortId));
                }
                else
                {
                    newState.SetState(new WaitingForHost(PortId));
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

namespace CarrierSlotMapStateOnly
{
    public class CarrierSlotMapState
    {
        public CarrierSlotMapState(int portId, BaseCarrierSlotMapState initialState, LoadPortStateInformation initInfo)
        {
            PortId = portId;
            _currentState = initialState;
            _currentInformation = new LoadPortStateInformation();

            initInfo.CopyTo(ref _currentInformation);
            CurrentCarrierSlotMapState = _currentState.StateName;
        }

        protected BaseCarrierSlotMapState _currentState;
        protected LoadPortStateInformation _currentInformation;
        protected readonly int PortId;

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
            if (false == (_currentState is IdNotRead))
            {
                _currentState = new IdNotRead(PortId);
            }

            CurrentCarrierSlotMapState = _currentState.StateName;
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

        public void CompleteByHost(bool isSuccess)
        {
            if (!(_currentState is WaitingForHost))
            {
                return;
            }

            SetState(isSuccess
                ? (BaseCarrierSlotMapState)new VerificationOk(PortId)
                : new VerificationFailed(PortId));

            CurrentCarrierSlotMapState = _currentState.StateName;
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
                    newState.SetState(new VerificationOk(PortId));
                }
                else
                {
                    newState.SetState(new WaitingForHost(PortId));
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

#region <Old>
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;

//using EFEM.Defines.LoadPort;
//using TransferStateOnly;

//namespace EFEM.Modules.LoadPort
//{
//    public class StateTransitionManager
//    {
//        #region <Constructors>
//        public StateTransitionManager(int portId, ref LoadPortStateInformation information)
//        {
//            PortId = portId;
//            StateInformation = information;
//            //_transferState = LoadPortTransferStates.OutOfService;

//            TransferStateTransitioner = new TransferState(PortId, new OutOfService(PortId), StateInformation);
//            CarrierIdStateTransitioner = new CarrierIdStateOnly.CarrierIdState(PortId, new CarrierIdStateOnly.IdNotRead(PortId), StateInformation);
//            CarrierSlotMapTransitioner = new CarrierSlotMapStateOnly.CarrierSlotMapState(PortId, new CarrierSlotMapStateOnly.IdNotRead(PortId), StateInformation);
//        }
//        #endregion </Constructors>

//        #region <Fields>
//        private readonly int PortId;
//        private readonly LoadPortStateInformation StateInformation = null;
//        private readonly TransferState TransferStateTransitioner = null;
//        private readonly CarrierIdStateOnly.CarrierIdState CarrierIdStateTransitioner = null;
//        private readonly CarrierSlotMapStateOnly.CarrierSlotMapState CarrierSlotMapTransitioner = null;
//        #endregion </Fields>

//        #region <Properties>
//        public LoadPortTransferStates TransferState
//        {
//            get
//            {
//                return TransferStateTransitioner.CurrentTransferState;
//            }
//        }
//        public CarrierIdVerificationStates CarrierIdState
//        {
//            get
//            {
//                return CarrierIdStateTransitioner.CurrentCarrierIdState;
//            }
//        }
//        public CarrierSlotMapVerificationStates CarrierSlotMapState
//        {
//            get
//            {
//                return CarrierSlotMapTransitioner.CurrentCarrierSlotMapState;
//            }
//        }

//        #endregion </Properties>

//        #region <Methods>

//        #region <Execute>
//        public void InitTransferState()
//        {
//            TransferStateTransitioner.InitState();
//        }

//        public void ExecuteTransition()
//        {
//            TransferStateTransitioner.TransitState(StateInformation);
//            CarrierIdStateTransitioner.TransitState(TransferState, StateInformation);
//            CarrierSlotMapTransitioner.TransitState(TransferState, CarrierIdState, StateInformation);
//        }
//        #endregion </Execute>

//        #region <Internals>

//        #region <States>
//        #endregion </States>

//        #endregion </Internals>

//        #endregion </Methods>
//    }
//}

//namespace TransferStateOnly
//{
//    public class TransferState
//    {
//        #region <Constructors>
//        public TransferState(int portId, BaseTransferState initialState, LoadPortStateInformation initInfo)
//        {
//            PortId = portId;
//            _currentState = initialState;
//            _currentInformation = new LoadPortStateInformation();

//            // 현재 값은 참조가 아닌 별도의 객체로 저장
//            initInfo.CopyTo(ref _currentInformation);
//        }
//        #endregion </Constructors>

//        #region <Fields>
//        protected BaseTransferState _currentState;
//        protected LoadPortStateInformation _currentInformation;

//        protected readonly int PortId;
//        #endregion </Fields>

//        #region <Properties>
//        public LoadPortStateInformation CurrentStateInformation
//        {
//            get
//            {
//                return _currentInformation;
//            }
//        }
//        public LoadPortTransferStates CurrentTransferState { get; private set; }
//        #endregion </Properties>

//        #region <Events>
//        // 상태 변화 통지용 이벤트
//        public delegate void StateChangedHandler(BaseTransferState newState);
//        //public event StateChangedHandler OnStateChanged;
//        #endregion </Events>

//        #region <Methods>
//        public void InitState()
//        {
//            if (false == (_currentState is OutOfService))
//            {
//                _currentState = new OutOfService(PortId);
//                //_currentState.TransitState(this, _currentInformation);
//            }
//        }

//        public void TransitState(LoadPortStateInformation newInfo)
//        {
//            if (false == newInfo.Enabled)
//            {
//                InitState();
//            }
//            else
//            {
//                _currentState.TransitState(this, newInfo);
//            }

//            //OnStateChanged?.Invoke(_currentState);

//            // 상태 전이 이후 현재 상태를 동기화한다.
//            newInfo.CopyTo(ref _currentInformation);
//            CurrentTransferState = _currentState.StateName;
//        }

//        public void SetState(BaseTransferState newState)
//        {
//            if (_currentState.GetType() != newState.GetType())
//            {
//                System.Console.WriteLine(string.Format("Transit State : {0} -> {1}", _currentState.GetType().Name, newState.GetType().Name));
//                _currentState = newState;
//            }
//        }
//        #endregion </Methods>
//    }

//    public abstract class BaseTransferState
//    {
//        #region <Constructors>
//        public BaseTransferState(int portId /*, LoadPortStateInformation initialInfo*/)
//        {
//            PortId = portId;
//        }
//        #endregion </Constructors>

//        #region <Fields>
//        protected readonly int PortId;
//        #endregion </Fields>

//        #region <Properties>
//        public LoadPortTransferStates StateName { get; protected set; }
//        #endregion </Properties>

//        #region <Methods>
//        public abstract void TransitState(TransferState newState, LoadPortStateInformation newInfo);

//        #region <Check loadport status>
//        // 캐리어가 정확히 놓여있다.(Placed, Present)
//        protected bool IsCarrierCorrectlyPlaced(TransferState currentState)
//        {
//            return (currentState.CurrentStateInformation.Placed && currentState.CurrentStateInformation.Present);
//        }
//        // 캐리어가 완벽히 제거되었다.
//        protected bool IsCarrierRemoved(TransferState currentState)
//        {
//            return (false == currentState.CurrentStateInformation.Placed && false == currentState.CurrentStateInformation.Present);
//        }

//        // 클램핑 중인 상태이다.
//        protected bool IsCurrentlyClampingStatus(TransferState currentState, LoadPortStateInformation newInfo)
//        {
//            if (currentState.CurrentStateInformation.ClampState != newInfo.ClampState)
//            {
//                return newInfo.ClampState;
//            }

//            return false;
//        }

//        protected bool IsCarrierStoppedOrCompleted(TransferState newState, LoadPortStateInformation newInfo)
//        {
//            if ((newInfo.CarrierAccessingState.Equals(CarrierAccessStates.CarrierCompleted) ||
//                newInfo.CarrierAccessingState.Equals(CarrierAccessStates.CarrierStopped)) &&
//                false == newState.CurrentStateInformation.DoorState &&
//                false == newState.CurrentStateInformation.DockState &&
//                false == newState.CurrentStateInformation.ClampState)          // 전부 해제되고, 자재가 완료 되었으면 ReadyToUnload
//                return true;

//            return false;
//        }
//        // 문이 열린 상태이다.
//        protected bool IsCurrentlyOpeningStatus(TransferState currentState, LoadPortStateInformation newInfo)
//        {
//            if (currentState.CurrentStateInformation.DoorState != newInfo.DoorState)
//            {
//                return newInfo.DoorState;
//            }

//            return false;
//        }

//        // 캐리어가 로딩되는 중이다.
//        protected bool IsCurrentlyLoadingStatus(TransferState currentState, LoadPortStateInformation newInfo)
//        {
//            if (currentState.CurrentStateInformation.DockState != newInfo.DockState)
//            {
//                // 도킹이 해제되는 중이다.
//                return newInfo.DockState;
//            }

//            return false;
//        }

//        // 캐리어가 언로딩되는 중이다.
//        protected bool IsCurrentlyUnloadingStatus(TransferState currentState, LoadPortStateInformation newInfo)
//        {
//            // 2024.07.04. jhlim [MOD] 홈이 안 잡힌 상태에서는 체크하지 않는다.
//            if (currentState.CurrentStateInformation.DockState != newInfo.DockState &&
//                newInfo.Initialized &&
//                (newInfo.CarrierAccessingState.Equals(CarrierAccessStates.CarrierCompleted) ||
//                newInfo.CarrierAccessingState.Equals(CarrierAccessStates.CarrierStopped)))
//            {
//                // 도킹이 해제되는 중이다.
//                return (false == newInfo.DockState);
//            }

//            return false;
//        }
//        #endregion </Check loadport status>

//        #endregion </Methods>
//    }

//    public class OutOfService : BaseTransferState
//    {
//        public OutOfService(int portId /*, LoadPortStateInformation initialInfo*/) : base(portId/*, initialInfo*/)
//        {
//            StateName = LoadPortTransferStates.OutOfService;
//        }

//        public override void TransitState(TransferState newState, LoadPortStateInformation newInfo)
//        {
//            if (newState.CurrentStateInformation.Initialized)
//            {
//                newState.SetState(new InService(PortId));
//            }
//        }
//    }

//    public class InService : BaseTransferState
//    {
//        public InService(int portId /*, LoadPortStateInformation initialInfo*/) : base(portId/*, initialInfo*/)
//        {
//            StateName = LoadPortTransferStates.InService;
//        }

//        public override void TransitState(TransferState newState, LoadPortStateInformation newInfo)
//        {
//            if (IsCarrierCorrectlyPlaced(newState))
//            {
//                newState.SetState(new TransferBlocked(PortId));
//            }
//            else
//            {
//                newState.SetState(new ReadyToLoad(PortId));
//            }
//        }
//    }

//    public class TransferBlocked : BaseTransferState
//    {
//        public TransferBlocked(int portId /*, LoadPortStateInformation initialInfo*/) : base(portId/*, initialInfo*/)
//        {
//            StateName = LoadPortTransferStates.TransferBlocked;
//        }

//        private int _seqNum;

//        public override void TransitState(TransferState newState, LoadPortStateInformation newInfo)
//        {
//            // 2025.09.08. jhlim [MOD] 메서드로 변경 -> AccessingStatus가 Stopped/Completed이면서, 캐리어가 배출 준비(언로드) 완료된 상태
//            // 기존에는 언로딩 되던 상태를 체크했었다.
//            if (IsCarrierStoppedOrCompleted(newState, newInfo))
//            {                
//                newState.SetState(new ReadyToUnload(PortId));
//            }
//            // TransferBlocked에서 캐리어가 제거되는 경우(언로딩된 상태) ReadyToLoad로 전이한다.
//            else if (IsCarrierRemoved(newState) &&
//                (false == newState.CurrentStateInformation.DoorState &&
//                 false == newState.CurrentStateInformation.DockState &&
//                 false == newState.CurrentStateInformation.ClampState))       // 제거되었고, 문이 열리고, 언도킹되었고, 언클램핑 상태면 상태 변경
//            {
//                newState.SetState(new ReadyToLoad(PortId));
//            }
//            else
//            {
//                switch (_seqNum)
//                {
//                    case 0:
//                        if (false == IsCurrentlyClampingStatus(newState, newInfo) && false == IsCurrentlyLoadingStatus(newState, newInfo))
//                            break;
//                        ++_seqNum; break;
//                    case 1:
//                        // TODO : Carrier Id Verification
//                        // OK 될 때까지 태스크에서는 기다려야 한다.
//                        // 태스크에서는 OK 되면 도킹, Failed 면 에러
//                        break;
//                    case 2:
//                        // TODO : Carrier Slot Map Verification
//                        // OK 될 때까지 태스크에서는 기다려야 한다.
//                        // 태스크에서는 이후 작업 진행, 여기선 default or 다음 스텝에서 계속 리턴
//                        break;
//                    default:
//                        break;
//                }
//            }
//        }
//    }

//    public class ReadyToLoad : BaseTransferState
//    {
//        public ReadyToLoad(int portId /*, LoadPortStateInformation initialInfo*/) : base(portId/*, initialInfo*/)
//        {
//            StateName = LoadPortTransferStates.ReadyToLoad;
//        }

//        public override void TransitState(TransferState newState, LoadPortStateInformation newInfo)
//        {
//            if (IsCarrierCorrectlyPlaced(newState))
//            {
//                newState.SetState(new TransferBlocked(PortId));
//            }
//        }
//    }

//    public class ReadyToUnload : BaseTransferState
//    {
//        public ReadyToUnload(int portId /*, LoadPortStateInformation initialInfo*/) : base(portId/*, initialInfo*/)
//        {
//            StateName = LoadPortTransferStates.ReadyToUnload;
//        }

//        public override void TransitState(TransferState newState, LoadPortStateInformation newInfo)
//        {

//            // 2025.09.08. jhlim [MOD] CorrectlyPlaced는 Present와 Place가 모두 On 된 상태인데, 둘 중 하나만 Off인 비정상 상황에도 ReadyToLoad로 넘어가던 조건 개선
//            //if (false == IsCarrierCorrectlyPlaced(newState) &&
//            if (IsCarrierRemoved(newState) &&
//                false == newInfo.DockState &&
//                false == newInfo.DoorState)
//            {
//                newState.SetState(new ReadyToLoad(PortId));
//            }
//            // 2025.09.08. jhlim [END]
//        }
//    }
//}

//namespace CarrierIdStateOnly
//{
//    public class CarrierIdState
//    {
//        #region <Constructors>
//        public CarrierIdState(int portId, BaseCarrierIdState initialState, LoadPortStateInformation initInfo)
//        {
//            PortId = portId;
//            _currentState = initialState;
//            _currentInformation = new LoadPortStateInformation();

//            // 현재 값은 참조가 아닌 별도의 객체로 저장
//            initInfo.CopyTo(ref _currentInformation);
//        }
//        #endregion </Constructors>

//        #region <Fields>
//        protected BaseCarrierIdState _currentState;
//        protected LoadPortStateInformation _currentInformation;
//        protected readonly int PortId;
//        #endregion </Fields>

//        #region <Properties>
//        public LoadPortStateInformation CurrentStateInformation
//        {
//            get
//            {
//                return _currentInformation;
//            }
//        }
//        public CarrierIdVerificationStates CurrentCarrierIdState { get; private set; }
//        #endregion </Properties>

//        #region <Methods>
//        public void InitState()
//        {
//            if (false == (_currentState is IdNotRead))
//            {
//                _currentState = new IdNotRead(PortId);
//            }
//        }

//        public void TransitState(LoadPortTransferStates transferState, LoadPortStateInformation newInfo)
//        {
//            if (false == transferState.Equals(LoadPortTransferStates.TransferBlocked))
//            {
//                InitState();
//            }
//            else
//            {
//                _currentState.TransitState(this, newInfo);
//            }

//            // 상태 전이 이후 현재 상태를 동기화한다.
//            newInfo.CopyTo(ref _currentInformation);
//            CurrentCarrierIdState = _currentState.StateName;
//        }

//        public void SetState(BaseCarrierIdState newState)
//        {
//            if (_currentState.GetType() != newState.GetType())
//            {
//                System.Console.WriteLine(string.Format("Carrier Id State : {0} -> {1}", _currentState.GetType().Name, newState.GetType().Name));
//                _currentState = newState;
//            }
//        }
//        #endregion </Methods>
//    }

//    public abstract class BaseCarrierIdState
//    {
//        #region <Constructors>
//        public BaseCarrierIdState(int portId /*, LoadPortStateInformation initialInfo*/)
//        {
//            PortId = portId;
//        }
//        #endregion </Constructors>

//        #region <Fields>
//        protected readonly int PortId;
//        #endregion </Fields>

//        #region <Properties>
//        public CarrierIdVerificationStates StateName { get; protected set; }
//        #endregion </Properties>

//        #region <Methods>
//        public abstract void TransitState(CarrierIdState newState, LoadPortStateInformation newInfo);

//        #region <Check loadport status>
//        // 캐리어가 정확히 놓여있다.(Placed, Present)
//        protected bool IsCarrierCorrectlyPlaced(CarrierIdState currentState)
//        {
//            return (currentState.CurrentStateInformation.Placed && currentState.CurrentStateInformation.Present);
//        }

//        // 클램핑 중인 상태이다.
//        protected bool IsCurrentlyClampingStatus(CarrierIdState currentState, LoadPortStateInformation newInfo)
//        {
//            if (currentState.CurrentStateInformation.ClampState != newInfo.ClampState)
//            {
//                return newInfo.ClampState;
//            }

//            return false;
//        }

//        // 캐리어가 로딩되는 중이다.
//        protected bool IsCurrentlyLoadingStatus(CarrierIdState currentState, LoadPortStateInformation newInfo)
//        {
//            if (currentState.CurrentStateInformation.DockState != newInfo.DockState)
//            {
//                // 도킹이 해제되는 중이다.
//                return newInfo.DockState;
//            }

//            return false;
//        }

//        // 문이 열린 상태이다.
//        protected bool IsCurrentlyOpeningStatus(CarrierIdState currentState, LoadPortStateInformation newInfo)
//        {
//            if (currentState.CurrentStateInformation.DoorState != newInfo.DoorState)
//            {
//                return newInfo.DoorState;
//            }

//            return false;
//        }

//        // 캐리어가 언로딩되는 중이다.
//        protected bool IsCurrentlyUnloadingStatus(CarrierIdState currentState, LoadPortStateInformation newInfo)
//        {
//            if (currentState.CurrentStateInformation.DockState != newInfo.DockState)
//            {
//                // 도킹이 해제되는 중이다.
//                return (false == newInfo.DockState);
//            }

//            return false;
//        }
//        #endregion </Check loadport status>

//        #endregion </Methods>
//    }

//    public class IdNotRead : BaseCarrierIdState
//    {
//        public IdNotRead(int portId /*, LoadPortStateInformation initialInfo*/) : base(portId/*, initialInfo*/)
//        {
//            StateName = CarrierIdVerificationStates.NotRead;
//        }

//        public override void TransitState(CarrierIdState newState, LoadPortStateInformation newInfo)
//        {
//            if (IsCurrentlyClampingStatus(newState, newInfo) || IsCurrentlyLoadingStatus(newState, newInfo))
//            {
//                newState.SetState(new WaitingForHost(PortId));
//            }
//        }
//    }

//    public class WaitingForHost : BaseCarrierIdState
//    {
//        public WaitingForHost(int portId /*, LoadPortStateInformation initialInfo*/) : base(portId/*, initialInfo*/)
//        {
//            StateName = CarrierIdVerificationStates.WaitingForHost;
//        }

//        public override void TransitState(CarrierIdState newState, LoadPortStateInformation newInfo)
//        {
//            newState.SetState(new VerificationOk(PortId));
//        }
//    }

//    public class VerificationOk : BaseCarrierIdState
//    {
//        public VerificationOk(int portId /*, LoadPortStateInformation initialInfo*/) : base(portId/*, initialInfo*/)
//        {
//            StateName = CarrierIdVerificationStates.VerificationOk;
//        }

//        public override void TransitState(CarrierIdState newState, LoadPortStateInformation newInfo)
//        {
//        }
//    }

//    public class VerificationFailed : BaseCarrierIdState
//    {
//        public VerificationFailed(int portId /*, LoadPortStateInformation initialInfo*/) : base(portId/*, initialInfo*/)
//        {
//            StateName = CarrierIdVerificationStates.VerificationFailed;
//        }

//        public override void TransitState(CarrierIdState newState, LoadPortStateInformation newInfo)
//        {
//        }
//    }

//    //public class CarrierIdState
//    //{
//    //    private BaseCarrierIdState state;

//    //    public CarrierIdState(TransferStateOnly.TransferState transferState)
//    //    {
//    //        transferState.OnStateChanged += UpdateState;
//    //        state = new IdNotRead(this); // 초기 상태 설정
//    //    }

//    //    public void SetState(BaseCarrierIdState newState)
//    //    {
//    //        this.state = newState;
//    //    }

//    //    public void UpdateState(TransferStateOnly.BaseTransferState newState)
//    //    {
//    //        if (newState is TransferStateOnly.TransferBlocked)
//    //        {
//    //            state.TransitState();
//    //        }            
//    //    }
//    //}

//    //public abstract class BaseCarrierIdState
//    //{
//    //    protected CarrierIdState _currentCarrierIdState;

//    //    public BaseCarrierIdState(CarrierIdState carrierIdState)
//    //    {
//    //        _currentCarrierIdState = carrierIdState;
//    //    }

//    //    public abstract void TransitState();
//    //}

//    //public class IdNotRead : BaseCarrierIdState
//    //{
//    //    public IdNotRead(CarrierIdState state) : base(state) { }

//    //    public override void TransitState()
//    //    {
//    //        _currentCarrierIdState.SetState(new WaitingForHost(_currentCarrierIdState));
//    //    }
//    //}

//    //public class WaitingForHost : BaseCarrierIdState
//    //{
//    //    public WaitingForHost(CarrierIdState state) : base(state) { }

//    //    public override void TransitState()
//    //    {
//    //        _currentCarrierIdState.SetState(new VerificationOk(_currentCarrierIdState));
//    //    }
//    //}

//    //public class VerificationOk : BaseCarrierIdState
//    //{
//    //    public VerificationOk(CarrierIdState state) : base(state) { }

//    //    public override void TransitState()
//    //    {
//    //        _currentCarrierIdState.SetState(new VerificationFailed(_currentCarrierIdState));
//    //    }
//    //}

//    //public class VerificationFailed : BaseCarrierIdState
//    //{
//    //    public VerificationFailed(CarrierIdState state) : base(state) { }

//    //    public override void TransitState()
//    //    {
//    //    }
//    //}
//}

//namespace CarrierSlotMapStateOnly
//{
//    public class CarrierSlotMapState
//    {
//        #region <Constructors>
//        public CarrierSlotMapState(int portId, BaseCarrierSlotMapState initialState, LoadPortStateInformation initInfo)
//        {
//            PortId = portId;
//            _currentState = initialState;
//            _currentInformation = new LoadPortStateInformation();

//            // 현재 값은 참조가 아닌 별도의 객체로 저장
//            initInfo.CopyTo(ref _currentInformation);
//        }
//        #endregion </Constructors>

//        #region <Fields>
//        protected BaseCarrierSlotMapState _currentState;
//        protected LoadPortStateInformation _currentInformation;
//        protected readonly int PortId;
//        #endregion </Fields>

//        #region <Properties>
//        public LoadPortStateInformation CurrentStateInformation
//        {
//            get
//            {
//                return _currentInformation;
//            }
//        }
//        public CarrierSlotMapVerificationStates CurrentCarrierSlotMapState { get; private set; }
//        #endregion </Properties>

//        #region <Methods>
//        public void InitState()
//        {
//            if (false == (_currentState is IdNotRead))
//            {
//                _currentState = new IdNotRead(PortId);
//            }
//        }

//        public void TransitState(LoadPortTransferStates transferState, CarrierIdVerificationStates idState, LoadPortStateInformation newInfo)
//        {
//            if (false == transferState.Equals(LoadPortTransferStates.TransferBlocked)
//                || false == idState.Equals(CarrierIdVerificationStates.VerificationOk))
//            {
//                InitState();
//            }
//            else
//            {
//                _currentState.TransitState(this, newInfo);
//            }

//            // 상태 전이 이후 현재 상태를 동기화한다.
//            newInfo.CopyTo(ref _currentInformation);
//            CurrentCarrierSlotMapState = _currentState.StateName;
//        }

//        public void SetState(BaseCarrierSlotMapState newState)
//        {
//            if (_currentState.GetType() != newState.GetType())
//            {
//                System.Console.WriteLine(string.Format("SlotMap State : {0} -> {1}", _currentState.GetType().Name, newState.GetType().Name));
//                _currentState = newState;
//            }
//        }
//        #endregion </Methods>
//    }

//    public abstract class BaseCarrierSlotMapState
//    {
//        #region <Constructors>
//        public BaseCarrierSlotMapState(int portId /*, LoadPortStateInformation initialInfo*/)
//        {
//            PortId = portId;
//        }
//        #endregion </Constructors>

//        #region <Fields>
//        protected readonly int PortId;
//        #endregion </Fields>

//        #region <Properties>
//        public CarrierSlotMapVerificationStates StateName { get; protected set; }
//        #endregion </Properties>

//        #region <Methods>
//        public abstract void TransitState(CarrierSlotMapState newState, LoadPortStateInformation newInfo);

//        #region <Check loadport status>
//        // 캐리어가 정확히 놓여있다.(Placed, Present)
//        protected bool IsCarrierCorrectlyPlaced(CarrierSlotMapState currentState)
//        {
//            return (currentState.CurrentStateInformation.Placed && currentState.CurrentStateInformation.Present);
//        }

//        // 클램핑 중인 상태이다.
//        protected bool IsCurrentlyClampingStatus(CarrierSlotMapState currentState, LoadPortStateInformation newInfo)
//        {
//            if (currentState.CurrentStateInformation.ClampState != newInfo.ClampState)
//            {
//                return newInfo.ClampState;
//            }

//            return false;
//        }

//        // 문이 열린 상태이다.
//        protected bool IsCurrentlyOpeningStatus(CarrierSlotMapState currentState, LoadPortStateInformation newInfo)
//        {
//            if (currentState.CurrentStateInformation.DoorState != newInfo.DoorState)
//            {
//                return newInfo.DoorState;
//            }

//            return false;
//        }

//        // 캐리어가 언로딩되는 중이다.
//        protected bool IsCurrentlyUnloadingStatus(CarrierSlotMapState currentState, LoadPortStateInformation newInfo)
//        {
//            if (currentState.CurrentStateInformation.DockState != newInfo.DockState)
//            {
//                // 도킹이 해제되는 중이다.
//                return (false == newInfo.DockState);
//            }

//            return false;
//        }
//        #endregion </Check loadport status>
//        #endregion </Methods>
//    }

//    public class IdNotRead : BaseCarrierSlotMapState
//    {
//        public IdNotRead(int portId /*, LoadPortStateInformation initialInfo*/) : base(portId/*, initialInfo*/)
//        {
//            StateName = CarrierSlotMapVerificationStates.NotRead;
//        }

//        public override void TransitState(CarrierSlotMapState newState, LoadPortStateInformation newInfo)
//        {
//            //if (IsCurrentlyOpeningStatus(newState, newInfo))
//            {
//                newState.SetState(new WaitingForHost(PortId));
//            }
//        }
//    }

//    public class WaitingForHost : BaseCarrierSlotMapState
//    {
//        public WaitingForHost(int portId /*, LoadPortStateInformation initialInfo*/) : base(portId/*, initialInfo*/)
//        {
//            StateName = CarrierSlotMapVerificationStates.WaitingForHost;
//        }

//        public override void TransitState(CarrierSlotMapState newState, LoadPortStateInformation newInfo)
//        {
//            newState.SetState(new VerificationOk(PortId));
//        }
//    }

//    public class VerificationOk : BaseCarrierSlotMapState
//    {
//        public VerificationOk(int portId /*, LoadPortStateInformation initialInfo*/) : base(portId/*, initialInfo*/)
//        {
//            StateName = CarrierSlotMapVerificationStates.VerificationOk;
//        }

//        public override void TransitState(CarrierSlotMapState newState, LoadPortStateInformation newInfo)
//        {
//        }
//    }

//    public class VerificationFailed : BaseCarrierSlotMapState
//    {
//        public VerificationFailed(int portId /*, LoadPortStateInformation initialInfo*/) : base(portId/*, initialInfo*/)
//        {
//            StateName = CarrierSlotMapVerificationStates.VerificationFailed;
//        }

//        public override void TransitState(CarrierSlotMapState newState, LoadPortStateInformation newInfo)
//        {
//        }
//    }
//}
#endregion </Old>