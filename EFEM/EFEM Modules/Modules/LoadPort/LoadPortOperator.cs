using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

using Define.DefineEnumBase.Common;
using EFEM.Defines.Common;
using EFEM.Defines.LoadPort;
using EFEM.MaterialTracking;
using EFEM.Modules.LoadPort.Recovery;
using EFEM.Defines.CarrierManagement;
using EFEM.Modules.LoadPort.Scheduler;
using EFEM.Modules.LoadPort.LoadPortControllers;

namespace EFEM.Modules.LoadPort
{
    public class LoadPortOperator
    {
        #region <Constructors>
        public LoadPortOperator(
            int portId,
            string name,
            LoadPortController controller,
            ILoadPortStateModel stateModel,
            LoadPortActionScheduler scheduler,
            AutomatedMaterialHandlingSystemController amhsController,
            Dictionary<string, string> locationNames,
            FrameOfSystem3.SECSGEM.IGem300ScenarioService gem300Service)
        {
            PortId = portId;
            Name = name;

            _gem300Service = gem300Service;
            _actionScheduler = scheduler;

            string logType = string.Format("{0}{1}", BaseLogTypes.LogTypeLoadPort, portId);
            _logger = new LoadPortLogger(logType, Name);

            _state = new LoadPortStateInformation();
            _stateModel = stateModel;
            _stateModel.Initialize();
            _stateModel.CopyStateTo(_state);

            _recoveryFilePath = Path.Combine(RecoveryFileDefines.LoadPortRecoveryFilePath, $"{Name}.json");
            _pendingRecoveryData = LoadPortRecoveryStorage.Load(_recoveryFilePath);
            _recoveryRequired = true;

            SubscribeStateModelEvents();

            //StateTransitionManager = new StateTransitionManager(portId, ref State);

            Controller = controller;
            if (Controller != null)
            {
                Controller.AttachSlotMapStateUpdatedEventHandler(UpdateCarrierSlotMap);
                Controller.AssignLogger(ref _logger);
            }


            _carrierServer = CarrierManagementServer.Instance;
            LoadPortSlots = new Dictionary<int, LoadPortLocation>();

            LoadPortLocations = new Dictionary<LoadPortLoadingMode, string>();
            foreach (var item in locationNames)
            {
                if (Enum.TryParse(item.Key, out LoadPortLoadingMode mode))
                {
                    LoadPortLocations[mode] = item.Value;
                }
            }

            //for (int i = 0; i < MaxCapacity; ++i)
            //{
            //    LoadPortSlots[i] = new LoadPortLocation(PortId, i, Name);
            //}
            //_locationServer.AddLoadPortLocation(PortId, LoadPortSlots);

            _substrateManager = SubstrateManager.Instance;
            //_substrateManager.AddLoadPortBuffers(portId, MaxCapacity);

            AMHSController = amhsController;
            if (AMHSController != null)
            {
                AMHSController.PortId = PortId;
                AMHSController.RegisterTransferNotifications(
                    PostLoadTransferStartedByPioReady,
                    PostLoadTransferStartedByPioCompt,
                    PostUnloadTransferStartedByPioReady,
                    NotifyUnloadTransferCompleted);
            }
        }
        #endregion </Constructors>

        #region <Fields>
        private readonly ILoadPortStateModel _stateModel = null;
        private readonly LoadPortStateInformation _state = null;
        private readonly FrameOfSystem3.SECSGEM.IGem300ScenarioService _gem300Service;
        private readonly LoadPortActionScheduler _actionScheduler;
        // 매 사이클 재사용 버퍼
        private LoadPortObservation _observationBuffer;

        private readonly LoadPortController Controller = null;
        //private Carrier _carrier = null;
        //private LoadPortTransferStates _backupTransferState;

        //private Carrier _myCarrier = null;
        private static CarrierManagementServer _carrierServer = null;
        private LoadPortLogger _logger = null;
        private int _actionStep = 0;

        protected readonly Dictionary<int, LoadPortLocation> LoadPortSlots = null;
        private Dictionary<LoadPortLoadingMode, string> LoadPortLocations = null;

        private static SubstrateManager _substrateManager = null;

        private int _seqCheckingPlacementStatus = 0;
        private const uint PlacementTimeOver = 10000;
        private const string PlacementError = "Placement Error";
        private readonly TickCounter_.TickCounter _placementStatusChecker = new TickCounter_.TickCounter();

        private int _seqCheckingCarrierOutStatus = 0;
        private const uint CarrierOutTimeOver = 1000;
        private const string CarrierOutError = "Carrier Out Error";
        private readonly TickCounter_.TickCounter _carrierOutStatusChecker = new TickCounter_.TickCounter();

        private readonly AutomatedMaterialHandlingSystemController AMHSController = null;

        private Func<bool> _functionToReadInput = null;

        // Enabled 변화를 E87 ChangeServiceStatus 서비스로 자동 반영하기 위한 캐시
        private bool? _lastAutoInServiceCondition = null;
        private bool _enabled = false;

        private readonly string _recoveryFilePath;
        private LoadPortRecoveryData _pendingRecoveryData;
        private bool _recoveryRequired;
        #endregion </Fields>

        #region <Properties>
        public bool Enabled
        {
            get
            {
                return _enabled;
            }
        }
        public int PortId { get; private set; }
        public string Name { get; private set; }
        public LoadPortLogger Logger
        {
            get
            {
                return _logger;
            }
        }
        public bool IsConnected
        {
            get
            {
                if (Controller == null)
                    return false;

                return Controller.IsConnected();
            }
        }
        public ICarrierService CarrierService
        {
            get
            {
                return _gem300Service.Carrier;
            }
        }
        public bool Initialized
        {
            get
            {
                if (_state == null)
                    return false;

                return _state.Initialized;
            }
        }
        public bool IsLoadPortBusy
        {
            get
            {
                if (_functionToReadInput == null || Controller is LoadPortControllers.LoadPortControllerSimulator)
                {
                    return Controller.State.Equals(LoadPortActionStates.Busy);
                }

                return _functionToReadInput();
            }
        }
        public bool Present
        {
            get
            {
                if (_state == null)
                    return false;

                return _state.Present;
            }
        }
        public bool Placed
        {
            get
            {
                if (_state == null)
                    return false;

                return _state.Placed;
            }
        }
        public bool IsPlacementMismatch
        {
            get
            {
                if (_state == null)
                    return false;

                return _state.IsPlacementMismatch;
            }
        }
        public bool ClampState
        {
            get
            {
                if (_state == null)
                    return false;

                return _state.ClampState;
            }
        }
        public bool DockState
        {
            get
            {
                if (_state == null)
                    return false;

                return _state.DockState;
            }
        }
        public bool DoorState
        {
            get
            {
                if (_state == null)
                    return false;

                return _state.DoorState;
            }
        }
        public bool PlacementErrorState
        {
            get
            {
                if (_state == null)
                    return true;

                return _state.PlacementErrorState;
            }
        }
        public bool CarrierOutErrorState
        {
            get
            {
                if (_state == null)
                    return true;

                return _state.CarrierOutErrorState;
            }
        }
        public string TriggeredControllerAlarm
        {
            get
            {
                if (_state == null)
                    return string.Empty;

                return _state.TriggeredAlarm;
            }
        }
        public LoadPortTransferStates TransferState
        {
            get
            {
                return _state.TransferState;
            }
        }
        public CarrierIdVerificationStates CarrierIdVerificationState
        {
            get
            {
                return _state.CarrierIdVerificationState;
            }
        }
        public CarrierSlotMapVerificationStates CarrierSlotMapVerificationState
        {
            get
            {
                return _state.CarrierSlotMapVerificationState;
            }
        }
        public ReservationStates ReservationState
        {
            get
            {
                return _state.ReservationState;
            }
        }

        public AssociationStates AssociationState
        {
            get
            {
                return _state.AssociationState;
            }
        }
        public string AssociatedCarrierId
        {
            get
            {
                return _state.AssociatedCarrierId;
            }
        }
        public CarrierAccessStates CarrierAccessingState
        {
            get
            {
                if (_state == null)
                    return CarrierAccessStates.NotAccessed;

                return _state.CarrierAccessingState;
            }
        }
        //public CarrierSlotMapStates[] SlotState
        //{
        //    get
        //    {
        //        if (_myCarrier == null)
        //            return null;

        //        return _myCarrier.SlotState;
        //    }
        //}
        public LoadPortLoadingMode LoadingType
        {
            get
            {
                if (_state == null)
                    // TODO : 2025.02.17. dwlim [MOD] 기본을 Cassette에서 Foup으로 변경하였는데, 나중에 PM에 맞게 적용하게끔 수정 필요
                    return LoadPortLoadingMode.Foup;

                return _state.LoadingType;
            }
        }

        public LoadPortAccessMode AccessMode
        {
            get
            {
                if (_state == null)
                    return LoadPortAccessMode.Manual;

                return _state.AccessMode;
            }
        }

        public bool IsInAccessViolation
        {
            get
            {
                if (AMHSController == null)
                    return false;

                return AMHSController.CheckIsInAccessViolation();
            }
        }
        public bool IsPIOInterfaceWorking
        {
            get
            {
                if (AMHSController == null)
                    return false;

                return AMHSController.IsPIOInterfaceWorking();
            }
        }
        public bool IsAnyPIOInputSignalOn
        {
            get
            {
                if (AMHSController == null)
                    return false;

                return AMHSController.IsAnyPIOInputSignalOn();
            }
        }
        public bool IsAnyPIOOutputSignalOn
        {
            get
            {
                if (AMHSController == null)
                    return false;

                return AMHSController.IsAnyPIOOutputSignalOn();
            }
        }
        public Dictionary<string, bool> HasActivePIOInputs
        {
            get
            {
                if (AMHSController == null)
                    return null;

                return AMHSController.HasActivePIOInputs();
            }
        }
        public bool IsLoadPortSimulationMode
        {
            get
            {
                return Controller is LoadPortControllerSimulator;
            }
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
        public event CarrierDetectionChangedHandler CarrierDetectionChanged;
        #endregion </Events>

        #region <Methods>

        #region <Event Handler>
        public void AttachModeChangerEventHandler(LoadPortLoadingMode type, LoadPortModeEventHandler eventHandler)
        {
            if (Controller == null)
                return;

            Controller.AttachModeChangerEventHandler(type, eventHandler);
        }
        public void AttachMechanicalButtonEventHandlers(LoadPortButtonTypes type, ButtonPressedEventHandler eventHandler)
        {
            Controller.AttachMechanicalButtonEventHandlers(type, eventHandler);
        }
        public void AttachBusySignalByDigitalInput(Func<bool> functionToReadInput)
        {
            _functionToReadInput = functionToReadInput;
        }

        private void SubscribeStateModelEvents()
        {
            _stateModel.TransferStateChanged += OnTransferStateChanged;
            _stateModel.CarrierIdStateChanged += OnCarrierIdStateChanged;
            _stateModel.CarrierSlotMapStateChanged += OnCarrierSlotMapStateChanged;
            _stateModel.AccessModeChanged += OnAccessModeChanged;
            _stateModel.CarrierAccessingStateChanged += OnCarrierAccessingStateChanged;

            if (_stateModel.SupportsReservationState)
            {
                _stateModel.ReservationStateChanged += OnReservationStateChanged;
            }

            if (_stateModel.SupportsAssociationState)
            {
                _stateModel.AssociationStateChanged += OnAssociationStateChanged;
            }
        }
        private void NotifyCarrierAccessStateToService(CarrierAccessStates state)
        {
            _gem300Service.Carrier.SetCarrierAccessing(Name, state, _carrierServer.GetCarrierId(PortId));
        }
        private void NotifyAccessModeToService(LoadPortAccessMode acceeMode)
        {
            _gem300Service.Carrier.ChangeAccessMode(Name, acceeMode);
        }
        private void NotifyTransferStateToService(LoadPortTransferStates state)
        {
            long result;
            switch (state)
            {
                case LoadPortTransferStates.ReadyToLoad:
                    result = _gem300Service.Carrier.SetReadyToLoad(Name);
                    break;
                case LoadPortTransferStates.ReadyToUnload:
                    result = _gem300Service.Carrier.SetReadyToUnload(Name);
                    break;
                default:
                    result = 0;
                    break;
            }
            if (result != 0)
            {

            }
        }
        private void OnTransferStateChanged(object sender, TransferStateChangedEvent e)
        {
            _state.TransferState = e.CurrentState;
            TransferStateChanged?.Invoke(this, e);

            NotifyTransferStateToService(e.CurrentState);
        }
        private void OnCarrierDetectionChanged(
            LoadPortStateInformation previousState,
            LoadPortStateInformation currentState)
        {
            if (previousState == null || currentState == null)
                return;

            // 신호 변경 자체를 기준으로 발행
            if (previousState.Present == currentState.Present &&
                previousState.Placed == currentState.Placed)
            {
                return;
            }

            var e = new CarrierDetectionChangedEvent
            {
                PortId = PortId,

                PreviousPresent = previousState.Present,
                PreviousPlaced = previousState.Placed,

                CurrentPresent = currentState.Present,
                CurrentPlaced = currentState.Placed,

                PreviousState = GetCarrierDetectionState(previousState.Present, previousState.Placed),
                CurrentState = GetCarrierDetectionState(currentState.Present, currentState.Placed),
            };

            CarrierDetectionChanged?.Invoke(this, e);

            //switch (e.CurrentState)
            //{
            //    case CarrierDetectionStates.NoneDetected:
            //        {
            //            if (_carrierServer.HasCarrier(PortId))
            //            {
            //                DateTime date = DateTime.Now;
            //                var archivePath = $@"{Define.DefineConstant.FilePath.FILEPATH_LOG}\BackupRecoveryData\{date.Year:0000}\{date.Month:00}\{date.Day:00}";

            //                _carrierServer.RemoveOrArchiveCarrierByPort(PortId, archivePath);
            //                Controller.RemoveCarrierMap();
            //                _logger.WriteCarrrierEvent(false);

            //                _gem300Service.Carrier.NotifyCarrierDetection(
            //                    Name,
            //                    _carrierServer.HasCarrier(PortId));
            //            }
            //        }
            //        break;
            //    case CarrierDetectionStates.PartiallyDetected:
            //        break;
            //    case CarrierDetectionStates.FullyDetected:
            //        {
            //            if (false == _carrierServer.HasCarrier(PortId))
            //            {
            //                _carrierServer.CreateCarrier(PortId);
            //                _logger.WriteCarrrierEvent(true);

            //                var result = _gem300Service.Carrier.NotifyCarrierDetection(
            //                    Name,
            //                    _carrierServer.HasCarrier(PortId));
            //            }
            //        }
            //        break;
            //    default:
            //        break;
            //}
       
            //Temp();
        }
        private void OnCarrierIdStateChanged(object sender, CarrierIdStateChangedEvent e)
        {
            _state.CarrierIdVerificationState = e.CurrentState;
            CarrierIdStateChanged?.Invoke(this, e);
        }

        private void OnCarrierSlotMapStateChanged(object sender, CarrierSlotMapStateChangedEvent e)
        {
            _state.CarrierSlotMapVerificationState = e.CurrentState;
            CarrierSlotMapStateChanged?.Invoke(this, e);
        }

        private void OnReservationStateChanged(object sender, ReservationStateChangedEvent e)
        {
            _state.ReservationState = e.CurrentState;
            ReservationStateChanged?.Invoke(this, e);
        }

        private void OnAssociationStateChanged(object sender, AssociationStateChangedEvent e)
        {
            _state.AssociationState = e.CurrentState;
            _state.AssociatedCarrierId = e.CurrentCarrierId;
            AssociationStateChanged?.Invoke(this, e);
        }
        private void OnAccessModeChanged(object sender, AccessModeChangedEvent e)
        {
            _state.AccessMode = e.CurrentMode;
            AccessModeChanged?.Invoke(this, e);

            NotifyAccessModeToService(e.CurrentMode);
        }
        private void OnCarrierAccessingStateChanged(object sender, CarrierAccessingStateChangedEvent e)
        {
            _state.CarrierAccessingState = e.CurrentState;
            CarrierAccessingStateChanged?.Invoke(this, e);

            NotifyCarrierAccessStateToService(e.CurrentState);
        }
        #endregion </Event Handler>

        #region <Carrier>
        public void RecreateCarrier()
        {
            if (TransferState.Equals(LoadPortTransferStates.TransferBlocked) ||
                TransferState.Equals(LoadPortTransferStates.ReadyToUnload))
            {
                if (_carrierServer.HasCarrier(PortId))
                {
                    DateTime date = DateTime.Now;
                    var archivePath = $@"{Define.DefineConstant.FilePath.FILEPATH_LOG}\BackupRecoveryData\{date.Year:0000}\{date.Month:00}\{date.Day:00}";
                    _carrierServer.RemoveOrArchiveCarrierByPort(PortId, archivePath);

                    Controller.RemoveCarrierMap();
                    _stateModel.Reset();
                    _stateModel.CopyStateTo(_state);

                    _lastAutoInServiceCondition = null;
                }
            }
        }

        private void AssignCarrierByTransferState()
        {
            // 1. physical carrier 생성 기준
            //    - 현재 구현은 Bind 시점에 carrier object 를 만들지 않는다.
            //    - Bind 는 logical association / reservation 만 반영한다.
            //    - 실제 carrier object 생성은 포트에 캐리어가 정상 안착되었음이 확인된 뒤,
            //      즉 TransferBlocked + Present + Placed 조건을 만족할 때 수행한다.
            //
            // 문서와의 구현 차이:
            //    - 문서상 Bind 는 carrier context 와 더 밀접하게 연결될 수 있으나,
            //      현재 구현은 logical binding 과 physical carrier 존재를 분리한다.
            if (TransferState == LoadPortTransferStates.TransferBlocked &&
                Present &&
                Placed &&
                false == _carrierServer.HasCarrier(PortId))
            {
                _carrierServer.CreateCarrier(PortId);
                _logger.WriteCarrrierEvent(true);

                var result = _gem300Service.Carrier.NotifyCarrierDetection(
                    Name,
                    string.Empty,
                    _state.CarrierIdVerificationState,
                    _carrierServer.HasCarrier(PortId));
            }

            // 2. physical carrier 제거 기준
            //    - 현재 구현은 ReadyToLoad 복귀 시점에 carrier 를 제거/아카이브한다.
            if (TransferState == LoadPortTransferStates.ReadyToLoad &&
                _carrierServer.HasCarrier(PortId))
            {
                DateTime date = DateTime.Now;
                var archivePath = $@"{Define.DefineConstant.FilePath.FILEPATH_LOG}\BackupRecoveryData\{date.Year:0000}\{date.Month:00}\{date.Day:00}";

                UnAssociation();

                _carrierServer.RemoveOrArchiveCarrierByPort(PortId, archivePath);
                Controller.RemoveCarrierMap();
                _logger.WriteCarrrierEvent(false);

                _gem300Service.Carrier.NotifyCarrierDetection(
                    Name,
                    _carrierServer.GetCarrierId(PortId),
                    _state.CarrierIdVerificationState,
                    _carrierServer.HasCarrier(PortId));
            }
        }
        #endregion </Carrier>

        #region <Scheduler>
        public void RegisterCompletionCondition(ICarrierCompletionCondition condition)
        {
            if (_actionScheduler == null)
                return;

            _actionScheduler.RegisterCompletionCondition(condition);
        }
        public void RegisterCompletionHandlingPolicy(ICarrierCompletionHandlingPolicy policy)
        {
            if (_actionScheduler == null)
                return;

            _actionScheduler.RegisterCompletionHandlingPolicy(policy);
        }
        public CARRIER_PORT_TYPE ExecuteSchedulers()
        {
            if (_actionScheduler == null)
                return CARRIER_PORT_TYPE.SELECTION;

            return _actionScheduler.ExecuteSchedulers();
        }
        public void ChangeSlotMapForDryRun()
        {
            if (_actionScheduler == null)
                return;

            _actionScheduler.ChangeSlotMapForDryRun();
        }
        #endregion </Scheduler>

        #region <Actions>
        public void InitAction()
        {
            _actionStep = 0;
            Controller.InitAction();
        }
        public CommandResults Initialize()
        {
            switch (_actionStep)
            {
                case 0:
                    _logger.WriteOperationStartLog(LoadPortCommands.Initialize);
                    ++_actionStep;
                    break;
            }

            var result = Controller.DoInitialize();
            switch (result.CommandResult)
            {
                case CommandResult.Proceed:
                    break;
                default:
                    _actionStep = 0;
                    _logger.WriteOperationEndLog(LoadPortCommands.Initialize, result);
                    break;

            }

            return result;
        }
        public CommandResults Load()
        {
            switch (_actionStep)
            {
                case 0:
                    _logger.WriteOperationStartLog(LoadPortCommands.Load);
                    ++_actionStep;
                    break;
            }

            var result = Controller.DoLoad();
            switch (result.CommandResult)
            {
                case CommandResult.Proceed:
                    break;
                default:
                    _actionStep = 0;
                    _logger.WriteOperationEndLog(LoadPortCommands.Load, result);
                    break;

            }

            return result;
        }
        public CommandResults Unload()
        {
            switch (_actionStep)
            {
                case 0:
                    _logger.WriteOperationStartLog(LoadPortCommands.Unload);
                    ++_actionStep;
                    break;
            }

            var result = Controller.DoUnload();
            switch (result.CommandResult)
            {
                case CommandResult.Proceed:
                    break;
                default:
                    _actionStep = 0;
                    _logger.WriteOperationEndLog(LoadPortCommands.Unload, result);
                    break;

            }

            return result;
        }
        public CommandResults Clamp()
        {
            switch (_actionStep)
            {
                case 0:
                    _logger.WriteOperationStartLog(LoadPortCommands.Clamp);
                    ++_actionStep;
                    break;
            }

            var result = Controller.DoClamp();
            switch (result.CommandResult)
            {
                case CommandResult.Proceed:
                    break;
                default:
                    _actionStep = 0;
                    _logger.WriteOperationEndLog(LoadPortCommands.Clamp, result);
                    break;

            }

            return result;
        }
        public CommandResults UnClamp()
        {
            switch (_actionStep)
            {
                case 0:
                    _logger.WriteOperationStartLog(LoadPortCommands.Unclamp);
                    ++_actionStep;
                    break;
            }

            var result = Controller.DoUnClamp();
            switch (result.CommandResult)
            {
                case CommandResult.Proceed:
                    break;
                default:
                    _actionStep = 0;
                    _logger.WriteOperationEndLog(LoadPortCommands.Unclamp, result);
                    break;

            }

            return result;
        }
        public CommandResults Dock()
        {
            switch (_actionStep)
            {
                case 0:
                    _logger.WriteOperationStartLog(LoadPortCommands.Dock);
                    ++_actionStep;
                    break;
            }

            var result = Controller.DoDock();
            switch (result.CommandResult)
            {
                case CommandResult.Proceed:
                    break;
                default:
                    _actionStep = 0;
                    _logger.WriteOperationEndLog(LoadPortCommands.Dock, result);
                    break;

            }

            return result;
        }
        public CommandResults UnDock()
        {
            switch (_actionStep)
            {
                case 0:
                    _logger.WriteOperationStartLog(LoadPortCommands.Undock);
                    ++_actionStep;
                    break;
            }

            var result = Controller.DoUnDock();
            switch (result.CommandResult)
            {
                case CommandResult.Proceed:
                    break;
                default:
                    _actionStep = 0;
                    _logger.WriteOperationEndLog(LoadPortCommands.Undock, result);
                    break;

            }

            return result;
        }
        public CommandResults OpenDoor()
        {
            switch (_actionStep)
            {
                case 0:
                    _logger.WriteOperationStartLog(LoadPortCommands.DoorOpen);
                    ++_actionStep;
                    break;
            }

            var result = Controller.DoOpenDoor();
            switch (result.CommandResult)
            {
                case CommandResult.Proceed:
                    break;
                default:
                    _actionStep = 0;
                    _logger.WriteOperationEndLog(LoadPortCommands.DoorOpen, result);
                    break;

            }

            return result;
        }
        public CommandResults CloseDoor()
        {
            switch (_actionStep)
            {
                case 0:
                    _logger.WriteOperationStartLog(LoadPortCommands.DoorClose);
                    ++_actionStep;
                    break;
            }

            var result = Controller.DoCloseDoor();
            switch (result.CommandResult)
            {
                case CommandResult.Proceed:
                    break;
                default:
                    _actionStep = 0;
                    _logger.WriteOperationEndLog(LoadPortCommands.DoorClose, result);
                    break;

            }

            return result;
        }
        public CommandResults Scan()
        {
            switch (_actionStep)
            {
                case 0:
                    _logger.WriteOperationStartLog(LoadPortCommands.ScanDown);
                    ++_actionStep;
                    break;
            }

            var result = Controller.DoScan();
            switch (result.CommandResult)
            {
                case CommandResult.Proceed:
                    break;
                default:
                    _actionStep = 0;
                    _logger.WriteOperationEndLog(LoadPortCommands.ScanDown, result);
                    break;

            }

            return result;
        }
        public CommandResults GetSlotMap()
        {
            switch (_actionStep)
            {
                case 0:
                    _logger.WriteOperationStartLog(LoadPortCommands.GetMap);
                    ++_actionStep;
                    break;
            }

            var result = Controller.DoGetSlotMap();
            switch (result.CommandResult)
            {
                case CommandResult.Proceed:
                    break;
                default:
                    _actionStep = 0;
                    _logger.WriteOperationEndLog(LoadPortCommands.GetMap, result);
                    break;

            }

            return result;
        }
        public CommandResults FindCarrierMode()
        {
            LoadPortCommands action = LoadPortCommands.FindLoadingMode;

            switch (_actionStep)
            {
                case 0:
                    {
                        _logger.WriteOperationStartLog(action);
                        ++_actionStep;
                    }
                    break;
            }

            var result = Controller.DoFindLoadingMode();
            switch (result.CommandResult)
            {
                case CommandResult.Proceed:
                    break;
                default:
                    _actionStep = 0;
                    _logger.WriteOperationEndLog(action, result);
                    break;

            }

            return result;
        }
        public CommandResults ChangeCarrierMode(LoadPortLoadingMode mode)
        {
            LoadPortCommands action;
            switch (mode)
            {
                case LoadPortLoadingMode.Cassette:
                    action = LoadPortCommands.ChangeToCassette;
                    break;
                case LoadPortLoadingMode.ClosedCassette:
                    action = LoadPortCommands.ChangeToClosedCassette;
                    break;
                default:
                    action = LoadPortCommands.ChangeToFoup;
                    break;
            }

            switch (_actionStep)
            {
                case 0:
                    {
                        _logger.WriteOperationStartLog(action);
                        ++_actionStep;
                    }
                    break;
            }

            var result = Controller.DoChangeLoadingMode(mode);
            switch (result.CommandResult)
            {
                case CommandResult.Proceed:
                    break;
                default:
                    _actionStep = 0;
                    _logger.WriteOperationEndLog(action, result);
                    break;

            }

            return result;
        }
        public CommandResults ChangeAccessMode(LoadPortAccessMode mode)
        {
            LoadPortCommands action = mode == LoadPortAccessMode.Auto ?
                LoadPortCommands.ChangeAccessModeToAuto :
                LoadPortCommands.ChangeAccessModeToManual;

            switch (_actionStep)
            {
                case 0:
                    _logger.WriteOperationStartLog(action);

                    // E87 ChangeAccess 가드:
                    // - RESERVED 상태에서는 변경 금지
                    // - carrier transfer 중(현재 최소 해석: TransferBlocked)에는 변경 금지
                    if (!_stateModel.CanChangeAccessMode(mode))
                    {
                        var rejected = new CommandResults(action.ToString(), CommandResult.Invalid);
                        _logger.WriteOperationEndLog(action, rejected);
                        _actionStep = 0;
                        return rejected;
                    }

                    // ChangeAccess 서비스 입력을 상태모델에 전달한다.
                    if (mode == LoadPortAccessMode.Auto)
                    {
                        PostChangingAccessModeToAuto();
                    }
                    else
                    {
                        PostChangingAccessModeToManual();
                    }

                    ++_actionStep;
                    break;
            }

            var result = Controller.DoChangeAccessMode(mode);
            switch (result.CommandResult)
            {
                case CommandResult.Proceed:
                    break;
                default:
                    _actionStep = 0;
                    _logger.WriteOperationEndLog(action, result);
                    break;

            }

            return result;
        }
        public CommandResults ClearAlarm()
        {
            switch (_actionStep)
            {
                case 0:
                    _logger.WriteOperationStartLog(LoadPortCommands.Reset);
                    ++_actionStep;
                    break;
            }

            var result = Controller.DoClearAlarm();
            switch (result.CommandResult)
            {
                case CommandResult.Proceed:
                    break;
                default:
                    _actionStep = 0;
                    _logger.WriteOperationEndLog(LoadPortCommands.Reset, result);
                    break;

            }

            return result;
        }
        public CommandResults AmpControl(bool enabled)
        {
            LoadPortCommands action = enabled ?
                LoadPortCommands.AmpOn :
                LoadPortCommands.AmpOff;

            switch (_actionStep)
            {
                case 0:
                    _logger.WriteOperationStartLog(action);
                    ++_actionStep;
                    break;
            }

            var result = Controller.DoAmpControl(enabled);
            switch (result.CommandResult)
            {
                case CommandResult.Proceed:
                    break;
                default:
                    _actionStep = 0;
                    _logger.WriteOperationEndLog(action, result);
                    break;

            }

            return result;
        }
        #endregion </Actions>

        #region <States>
        public void SaveRecoveryData()
        {
            var recoveryData = _stateModel.CreateRecoveryData();
            LoadPortRecoveryStorage.Save(_recoveryFilePath, recoveryData);
        }
        public void EnableLoadPort(bool enabled)
        {
            _enabled = enabled;
        }
        public LoadPortStateInformation GetLoadPortState()
        {
            return _state;
        }
        private bool IsCurrectlyPlaced(bool placed)
        {
            return ((placed == _state.Present) && (placed == _state.Placed));
        }
        private bool CanCarrierOut()
        {
            return (false == _state.DockState
                && false == _state.ClampState
                && false == _state.DoorState);
        }
        private bool CheckPlaceStatus(bool enabled, bool present, bool placed)
        {
            if (!enabled)
                return true;

            switch (_seqCheckingPlacementStatus)
            {
                case 0:
                    {
                        if (present == placed)
                            break;

                        _placementStatusChecker.SetTickCount(15000);
                        ++_seqCheckingPlacementStatus;
                    }
                    break;

                case 1:
                    {
                        if (_placementStatusChecker.IsTickOver(true))
                        {
                            if (present == placed)
                            {
                                _seqCheckingPlacementStatus = 0;
                                break;
                            }

                            return false;
                        }

                        if (present != placed)
                            break;

                        --_seqCheckingPlacementStatus;
                    }
                    break;
            }

            return true;
        }

        private bool CheckCarrierOutStatus(
            bool enabled,
            bool present,
            bool placed,
            bool dockState,
            bool clampState,
            bool doorState)
        {
            if (!enabled)
                return true;

            bool canCarrierOut = !dockState && !clampState && !doorState;

            switch (_seqCheckingCarrierOutStatus)
            {
                case 0:
                    {
                        if (!canCarrierOut)
                        {
                            if (!present || !placed)
                            {
                                _carrierOutStatusChecker.SetTickCount(15000);
                                ++_seqCheckingCarrierOutStatus;
                            }
                        }
                    }
                    break;

                case 1:
                    {
                        if (_carrierOutStatusChecker.IsTickOver(true))
                        {
                            if (present && placed)
                            {
                                _seqCheckingCarrierOutStatus = 0;
                                break;
                            }

                            return false;
                        }

                        if (!present || !placed)
                            break;

                        --_seqCheckingCarrierOutStatus;
                    }
                    break;
            }

            return true;
        }

        private void SyncServiceStatusFromEnabledAndInitialized(bool enabled, bool initialized)
        {
            bool shouldBeInService = enabled && initialized;

            if (!_lastAutoInServiceCondition.HasValue ||
                _lastAutoInServiceCondition.Value != shouldBeInService)
            {
                if (shouldBeInService)
                {
                    PostChangeServiceStatusToInService();
                }
                else
                {
                    PostChangeServiceStatusToOutOfService();
                }

                _lastAutoInServiceCondition = shouldBeInService;
            }
        }

        private void FillObservation(ref LoadPortObservation observation)
        {
            observation.PortId = PortId;
            observation.Enabled = Enabled;
            observation.Initialized = Controller.Initialized;
            observation.Present = Controller.Present;
            observation.Placed = Controller.Placed;
            observation.IsPlacementMismatch = Controller.IsPlacementMismatch;
            observation.ClampState = Controller.ClampState;
            observation.DockState = Controller.DockState;
            observation.DoorState = Controller.DoorState;
            observation.AccessMode = Controller.AccessMode;
            observation.LoadingType = Controller.LoadingType;
            observation.CarrierAccessingState = _carrierServer.HasCarrier(PortId)
                ? _carrierServer.GetCarrierAccessingStatus(PortId)
                : CarrierAccessStates.NotAccessed;
            observation.TriggeredAlarm = Controller.GetTriggeredControllerAlarm();

            observation.PlacementErrorState = false == CheckPlaceStatus(
                observation.Enabled,
                observation.Present,
                observation.Placed);

            observation.CarrierOutErrorState = false == CheckCarrierOutStatus(
                observation.Enabled,
                observation.Present,
                observation.Placed,
                observation.DockState,
                observation.ClampState,
                observation.DoorState);
        }

        //private void UpdateLoadPortState()
        //{
        //    State.Enabled = Enabled;
        //    State.Initialized = Controller.Initialized;
        //    State.Placed = Controller.Placed;
        //    State.IsPlacementMismatch = Controller.IsPlacementMismatch;
        //    State.Present = Controller.Present;
        //    State.ClampState = Controller.ClampState;
        //    State.DockState = Controller.DockState;
        //    State.DoorState = Controller.DoorState;
        //    State.LoadingType = Controller.LoadingType;
        //    State.TransferState = TransferState;
        //    State.AccessMode = Controller.AccessMode;

        //    if (_carrierServer.HasCarrier(PortId))
        //    {
        //        State.CarrierAccessingState = _carrierServer.GetCarrierAccessingStatus(PortId);
        //    }
        //    else
        //    {
        //        State.CarrierAccessingState = CarrierAccessStates.Unknown;
        //    }

        //    State.TriggeredAlarm = Controller.GetTriggeredControllerAlarm();
        //    State.PlacementErrorState = (false == CheckPlaceStatus());
        //    State.CarrierOutErrorState = (false == CheckCarrierOutStatus());

        //    //if (State.Enabled != Enabled)
        //    //{
        //    //}

        //    //if (State.Initialized != Controller.Initialized)
        //    //{
        //    //}

        //    //if (State.Placed != Controller.Placed)
        //    //{
        //    //}

        //    //if (State.Present != Controller.Present)
        //    //{
        //    //}

        //    //if (State.ClampState != Controller.ClampState)
        //    //{
        //    //}

        //    //if (State.DockState != Controller.DockState)
        //    //{
        //    //}

        //    //if (State.DoorState != Controller.DoorState)
        //    //{
        //    //}

        //    //if (State.LoadingType != Controller.LoadingType)
        //    //{
        //    //}

        //    //if (State.TransferState != TransferState)
        //    //{
        //    //}
        //}
        private void UpdateCarrierSlotMap(int portId, IDictionary<int, CarrierSlotMapStates> slotMap)
        {
            if (Controller.SlotState == null)
                return;

            if (false == _carrierServer.HasCarrier(portId))
            {
                return;
            }

            _carrierServer.SetCarrierSlotMap(portId, slotMap);
        }
        #endregion </States>

        /*
         * 구현 메모
         *
         * - Bind 는 logical association / reservation 서비스로 해석한다.
         * - physical carrier 생성은 Bind 시점이 아니라 실제 안착 확인 후 수행한다.
         * - 즉 TransferBlocked + Present + Placed 조건에서 carrier 를 생성한다.
         */
        #region <Internal StateModel Inputs>
        private void PostCarrierIdVerificationResult(bool isSuccess)
        {
            var input = new LoadPortExternalInput
            {
                PortId = PortId,
                InputType = isSuccess
                    ? LoadPortExternalInputType.CarrierIdVerificationAccepted
                    : LoadPortExternalInputType.CarrierIdVerificationRejected,
                BooleanValue = isSuccess
            };

            _stateModel.ApplyExternalInput(in input);
        }

        private void PostCarrierSlotMapVerificationResult(bool isSuccess)
        {
            var input = new LoadPortExternalInput
            {
                PortId = PortId,
                InputType = isSuccess
                    ? LoadPortExternalInputType.CarrierSlotMapVerificationAccepted
                    : LoadPortExternalInputType.CarrierSlotMapVerificationRejected,
                BooleanValue = isSuccess
            };

            _stateModel.ApplyExternalInput(in input);
        }

        /// <summary>
        /// 전이 #2: OUT OF SERVICE -> IN SERVICE
        /// 의미: InService 입력을 상태모델에 전달한다.
        /// </summary>
        private void PostChangeServiceStatusToInService()
        {
            var input = new LoadPortExternalInput
            {
                PortId = PortId,
                InputType = LoadPortExternalInputType.ChangeServiceStatusToInService,
                BooleanValue = true
            };

            _stateModel.ApplyExternalInput(in input);
        }

        /// <summary>
        /// 전이 #3: IN SERVICE -> OUT OF SERVICE
        /// 의미: OutOfService 입력을 상태모델에 전달한다.
        /// </summary>
        private void PostChangeServiceStatusToOutOfService()
        {
            var input = new LoadPortExternalInput
            {
                PortId = PortId,
                InputType = LoadPortExternalInputType.ChangeServiceStatusToOutOfService,
                BooleanValue = true
            };

            _stateModel.ApplyExternalInput(in input);
        }

        /// <summary>
        /// 전이 #6: READY TO LOAD -> TRANSFER BLOCKED
        /// 의미: Load transfer start 입력을 상태모델에 전달한다.
        /// </summary>
        private void PostLoadTransferStartedByPioReady()
        {
            var input = new LoadPortExternalInput
            {
                PortId = PortId,
                InputType = LoadPortExternalInputType.LoadTransferStartedByPioReady,
                BooleanValue = true
            };

            _stateModel.ApplyExternalInput(in input);

            _gem300Service.Carrier.SetPioSignal(
                Name,
                7,
                1);
        }

        // 개념상 Loading은 Ready 신호만 꺼지면 되지만 쌍을 맞추기 위해서 추가한다.
        private void PostLoadTransferStartedByPioCompt()
        {
            //var input = new LoadPortExternalInput
            //{
            //    PortId = PortId,
            //    InputType = LoadPortExternalInputType.LoadTransferStartedByPioReady,
            //    BooleanValue = true
            //};

            //_stateModel.ApplyExternalInput(in input);

            _gem300Service.Carrier.SetPioSignal(
                Name,
                7,
                0);
        }

        /// <summary>
        /// 전이 #7: READY TO UNLOAD -> TRANSFER BLOCKED
        /// 의미: Unload transfer start 입력을 상태모델에 전달한다.
        /// </summary>
        private void PostUnloadTransferStartedByPioReady()
        {
            var input = new LoadPortExternalInput
            {
                PortId = PortId,
                InputType = LoadPortExternalInputType.UnloadTransferStartedByPioReady,
                BooleanValue = true
            };

            _stateModel.ApplyExternalInput(in input);

            _gem300Service.Carrier.SetPioSignal(
                Name,
                7,
                1);
        }

        /// <summary>
        /// 전이 #8: TRANSFER BLOCKED -> READY TO LOAD
        /// 의미: Unload transfer complete 입력을 상태모델에 전달한다.
        /// </summary>
        private void PostUnloadTransferCompletedByPioCompt()
        {
            var input = new LoadPortExternalInput
            {
                PortId = PortId,
                InputType = LoadPortExternalInputType.UnloadTransferCompletedByPioCompt,
                BooleanValue = true
            };

            _stateModel.ApplyExternalInput(in input);

            _gem300Service.Carrier.SetPioSignal(
                Name,
                7,
                0);
        }

        /// <summary>
        /// 전이 #10: TRANSFER BLOCKED -> TRANSFER READY
        /// 의미: TransferFailed 입력을 상태모델에 전달한다.
        /// </summary>
        private void PostTransferFailed()
        {
            var input = new LoadPortExternalInput
            {
                PortId = PortId,
                InputType = LoadPortExternalInputType.TransferFailed,
                BooleanValue = true
            };

            _stateModel.ApplyExternalInput(in input);
        }

        /// <summary>
        /// 전이 #7 관련 입력
        /// 의미: CarrierReCreate 입력을 상태모델에 전달한다.
        /// </summary>
        private void PostCarrierReCreateIssued()
        {
            var input = new LoadPortExternalInput
            {
                PortId = PortId,
                InputType = LoadPortExternalInputType.CarrierReCreateIssued,
                BooleanValue = true
            };

            _stateModel.ApplyExternalInput(in input);
        }

        /// <summary>
        /// 전이 #9: TRANSFER BLOCKED -> READY TO UNLOAD
        /// 의미: Carrier returned 입력을 상태모델에 전달한다.
        /// </summary>
        private void PostCarrierReturnedToPort()
        {
            var input = new LoadPortExternalInput
            {
                PortId = PortId,
                InputType = LoadPortExternalInputType.CarrierReturnedToPort,
                BooleanValue = true
            };

            _stateModel.ApplyExternalInput(in input);
        }

        /// <summary>
        /// Reservation 전이 입력
        /// 의미: ReserveAtPort 입력을 상태모델에 전달한다.
        /// </summary>
        private void PostReserveAtPort()
        {
            var input = new LoadPortExternalInput
            {
                PortId = PortId,
                InputType = LoadPortExternalInputType.ReserveAtPort,
                BooleanValue = true
            };

            _stateModel.ApplyExternalInput(in input);
        }

        /// <summary>
        /// Reservation 전이 입력
        /// 의미: CancelReservationAtPort 입력을 상태모델에 전달한다.
        /// </summary>
        private void PostCancelReservationAtPort()
        {
            var input = new LoadPortExternalInput
            {
                PortId = PortId,
                InputType = LoadPortExternalInputType.CancelReservationAtPort,
                BooleanValue = true
            };

            _stateModel.ApplyExternalInput(in input);
        }

        /// <summary>
        /// Association 전이 입력
        /// 의미: Bind 입력을 상태모델에 전달한다.
        /// 주의:
        /// - 현재 구현에서 physical carrier object 생성은 이 시점에 수행하지 않는다.
        /// - 실제 생성은 캐리어가 포트에 정상 안착된 뒤
        ///   LoadPortOperator.AssignCarrierByTransferState()에서 수행한다.
        /// </summary>
        private void PostAssociation(string carrierId)
        {
            var input = new LoadPortExternalInput
            {
                PortId = PortId,
                InputType = LoadPortExternalInputType.BindAssociation,
                BooleanValue = true,
                CarrierId = carrierId
            };

            _stateModel.ApplyExternalInput(in input);
        }

        /// <summary>
        /// Association 전이 입력
        /// 의미: CancelBind 입력을 상태모델에 전달한다.
        /// </summary>
        private void PostUnAssociation()
        {
            var input = new LoadPortExternalInput
            {
                PortId = PortId,
                InputType = LoadPortExternalInputType.UnbindAssociation,
                BooleanValue = true,
                CarrierId = null
            };

            _stateModel.ApplyExternalInput(in input);
        }

        /// <summary>
        /// AccessMode 전이 입력
        /// 의미: Manual 변경 입력을 상태모델에 전달한다.
        /// </summary>
        private void PostChangingAccessModeToManual()
        {
            var input = new LoadPortExternalInput
            {
                PortId = PortId,
                InputType = LoadPortExternalInputType.ChangeAccessModeToManual,
                BooleanValue = true
            };

            _stateModel.ApplyExternalInput(in input);
        }

        /// <summary>
        /// AccessMode 전이 입력
        /// 의미: Auto 변경 입력을 상태모델에 전달한다.
        /// </summary>
        private void PostChangingAccessModeToAuto()
        {
            var input = new LoadPortExternalInput
            {
                PortId = PortId,
                InputType = LoadPortExternalInputType.ChangeAccessModeToAuto,
                BooleanValue = true
            };

            _stateModel.ApplyExternalInput(in input);
        }
        #endregion </Internal StateModel Inputs>

        #region <CMS Facade>
        /// <summary>
        /// ChangeAccess 서비스.
        /// 포트의 AccessMode 변경을 요청한다.
        /// </summary>
        public CommandResults ChangeAccess(LoadPortAccessMode mode)
        {
            return ChangeAccessMode(mode);
        }

        /// <summary>
        /// ChangeServiceStatus 서비스.
        /// 포트를 InService 또는 OutOfService로 전환한다.
        ///
        /// 구현 메모:
        /// - 현재 서비스 상태는 Enabled / Initialized 조건으로 자동 동기화된다.
        /// - 따라서 이 서비스는 targetStatus에 따라 Enabled 값만 먼저 변경한다.
        /// - 실제 InService / OutOfService 반영은 다음 Execute() cycle에서
        ///   SyncServiceStatusFromEnabledAndInitialized()를 통해 수행된다.
        /// - InService는 Enabled=true 이면서 Initialized=true 일 때만 반영된다.
        /// </summary>
        public void ChangeServiceStatus(LoadPortTransferStates targetStatus)
        {
            switch (targetStatus)
            {
                case LoadPortTransferStates.InService:
                    EnableLoadPort(true);
                    break;

                case LoadPortTransferStates.OutOfService:
                    EnableLoadPort(false);
                    break;

                default:
                    throw new ArgumentOutOfRangeException(nameof(targetStatus), targetStatus,
                        "ChangeServiceStatus only supports InService or OutOfService.");
            }

            //bool initialized = Controller != null && Controller.Initialized;
            //SyncServiceStatusFromEnabledAndInitialized(Enabled, initialized);
        }

        /// <summary>
        /// ReserveAtPort 서비스.
        /// 포트를 Reserved 상태로 전이시킨다.
        /// </summary>
        public void ReserveAtPort()
        {
            PostReserveAtPort();
        }

        /// <summary>
        /// CancelReservationAtPort 서비스.
        /// 포트의 Reservation을 해제한다.
        /// </summary>
        public void CancelReservationAtPort()
        {
            PostCancelReservationAtPort();
        }

        /// <summary>
        /// 포트와 CarrierID를 연계한다.
        ///
        /// 구현 메모:
        /// - 현재 구현은 포트-캐리어 연계와 Reservation 반영 중심으로 처리한다.
        /// - 추후 carrier object를 포트와 독립된 모델로 분리할 때 재구현할 예정이다.
        /// - 문서 의미에 더 가깝게 가려면 Bind 시점에 포트와 무관한 carrier object/context가 생성되어야 한다.
        /// </summary>
        public void Association(string carrierId)
        {
            PostAssociation(carrierId);
        }

        /// <summary>
        /// 포트와 Carrier의 연계를 해제한다.
        ///
        /// 구현 메모:
        /// - 현재 구현은 포트-캐리어 연계 해제 중심으로 처리한다.
        /// - 추후 carrier object를 포트와 독립된 모델로 분리할 때 Bind와 함께 재구현할 예정이다.
        /// </summary>
        public void UnAssociation()
        {
            PostUnAssociation();
        }

        /// <summary>
        /// CarrierReCreate 서비스.
        /// Carrier 재구성을 요청한다.
        ///
        /// 구현 메모:
        /// - 현재는 상태모델 입력 중심으로 처리한다.
        /// - 실제 carrier 제거/복구 동작은 RecreateCarrier() 헬퍼와 별도 책임으로 남아 있다.
        /// </summary>
        public void CarrierReCreate()
        {
            PostCarrierReCreateIssued();
        }

        /// <summary>
        /// ProceedWithCarrier 서비스.
        /// 대기 중인 Carrier 검증 절차의 진행을 요청한다.
        ///
        /// 구현 메모:
        /// - 현재 구현은 WaitingForHost 상태의 Carrier ID / SlotMap 검증 승인으로 축약되어 있다.
        /// </summary>
        public void ProceedWithCarrierForId(string carrierId = null, IDictionary<string, string> properties = null)
        {
            if (CarrierIdVerificationState == CarrierIdVerificationStates.WaitingForHost)
            {
                PostCarrierIdVerificationResult(true);
            }
        }
        public void ProceedWithCarrierForSlot(string carrierId = null, IDictionary<string, string> properties = null)
        {
            if (CarrierSlotMapVerificationState == CarrierSlotMapVerificationStates.WaitingForHost)
            {
                PostCarrierSlotMapVerificationResult(true);
            }
        }

        /// <summary>
        /// CancelCarrier 서비스.
        /// 대기 중인 Carrier 검증 절차의 취소를 요청한다.
        /// </summary>
        public void CancelCarrier(string carrierId = null)
        {
            if (CarrierIdVerificationState == CarrierIdVerificationStates.WaitingForHost)
            {
                PostCarrierIdVerificationResult(false);
                return;
            }

            if (CarrierSlotMapVerificationState == CarrierSlotMapVerificationStates.WaitingForHost)
            {
                PostCarrierSlotMapVerificationResult(false);
                return;
            }
        }

        /// <summary>
        /// CancelCarrierAtPort 서비스.
        /// 포트의 Carrier를 다시 unload 가능한 상태로 되돌리도록 요청한다.
        ///
        /// 구현 메모:
        /// - 현재 구현은 상태모델에 Carrier returned 입력을 전달하는 수준까지만 반영한다.
        /// - 추후 문서 의미에 더 가깝게 가려면 실제 carrier를 되돌리는 동작까지 추가되어야 한다.
        /// </summary>
        public void CancelCarrierAtPort()
        {
            PostCarrierReturnedToPort();
        }

        #region <Not implemented>
        /// <summary>
        /// CarrierTagReadData 서비스.
        /// Carrier tag 읽기를 요청한다.
        /// </summary>
        public CommandResults TagRead(string dataSeg = null, int? dataSize = null)
        {
            return new CommandResults("TagRead", CommandResult.Invalid, "Carrier tag read is not implemented.");
        }

        /// <summary>
        /// CarrierTagWriteData 서비스.
        /// Carrier tag 쓰기를 요청한다.
        /// </summary>
        public CommandResults TagWrite(string dataSeg = null, string data = null)
        {
            return new CommandResults("TagWrite", CommandResult.Invalid, "Carrier tag write is not implemented.");
        }

        /// <summary>
        /// CarrierOut 서비스.
        /// 내부 버퍼의 Carrier를 로드 포트로 이동시키도록 요청한다.
        /// </summary>
        public void CarrierOut()
        {
            throw new NotImplementedException("CMS CarrierOut is not implemented yet.");
        }

        /// <summary>
        /// CarrierIn 서비스.
        /// 로드 포트의 Carrier를 내부 버퍼로 이동시키도록 요청한다.
        /// </summary>
        public void CarrierIn()
        {
            throw new NotImplementedException("CMS CarrierIn is not implemented yet.");
        }

        /// <summary>
        /// CarrierRelease 서비스.
        /// Carrier hold 해제를 요청한다.
        /// </summary>
        public void CarrierRelease()
        {
            throw new NotImplementedException("CMS CarrierRelease is not implemented yet.");
        }

        /// <summary>
        /// CancelCarrierOut 서비스.
        /// CarrierOut 요청의 취소를 요청한다.
        /// </summary>
        public void CancelCarrierOut(string carrierId)
        {
            throw new NotImplementedException("E87 CancelCarrierOut is not implemented yet.");
        }

        /// <summary>
        /// CancelAllCarrierOut 서비스.
        /// 대기 중인 모든 CarrierOut 요청의 취소를 요청한다.
        /// </summary>
        public void CancelAllCarrierOut()
        {
            throw new NotImplementedException("E87 CancelAllCarrierOut is not implemented yet.");
        }

        /// <summary>
        /// CarrierNotification 서비스.
        /// Carrier 정보를 통지한다.
        /// </summary>
        public void CarrierNotification(string carrierId, IDictionary<string, string> properties = null)
        {
            throw new NotImplementedException("E87 CarrierNotification is not implemented yet.");
            //if (!_carrierServer.HasCarrier(PortId))
            //    return;

            //if (!string.IsNullOrWhiteSpace(carrierId))
            //{
            //    _carrierServer.SetCarrierId(PortId, carrierId);
            //}

            //if (properties == null)
            //    return;

            //foreach (var item in properties)
            //{
            //    _carrierServer.SetAttribute(PortId, item.Key, item.Value);
            //}
        }

        /// <summary>
        /// CancelCarrierNotification 서비스.
        /// CarrierNotification 취소를 요청한다.
        /// </summary>
        public void CancelCarrierNotification(string carrierId)
        {
            throw new NotImplementedException("E87 CancelCarrierNotification is not implemented yet.");
        }
        #endregion </Not implemented>

        #endregion </CMS Facade>

        #region <Notify at transferring>
        /// <summary>
        /// 외부 언로드 완료 통지.
        /// 자동 언로드 완료를 상태모델에 전달한다.
        /// </summary>
        public void NotifyUnloadTransferCompleted()
        {
            PostUnloadTransferCompletedByPioCompt();
        }

        /// <summary>
        /// 외부 전송 실패 통지.
        /// 전송 실패를 상태모델에 전달한다.
        /// </summary>
        public void NotifyTransferFailed()
        {
            PostTransferFailed();
        }
        #endregion </Notify at transferring>

        #region <Gathering>
        private bool NeedRecoveryStateInformations(ref LoadPortStateInformation previousState)
        {
            if (_recoveryRequired && _observationBuffer.Initialized)
            {
                _stateModel.RecoverFromObservation(_pendingRecoveryData, _observationBuffer);
                _stateModel.CopyStateTo(_state);

                _recoveryRequired = false;
                _pendingRecoveryData = null;

                OnCarrierDetectionChanged(previousState, _state);
                //Temp(_state.TransferState);

                AssignCarrierByTransferState();
                UpdateAMHSValues();

                var carrierId = _carrierServer.GetCarrierId(PortId);
                _gem300Service.Carrier.SetLoadPortInfo(
                Name,
                _state,
                carrierId);

                return true;
            }

            return false;
        }
        private void TryFinalizePendingCarrierCompletion()
        {
            if (_actionScheduler == null)
                return;

            LoadPortStateInformation observedState = new LoadPortStateInformation();

            observedState.Enabled = _observationBuffer.Enabled;
            observedState.Initialized = _observationBuffer.Initialized;
            observedState.Present = _observationBuffer.Present;
            observedState.Placed = _observationBuffer.Placed;
            observedState.IsPlacementMismatch = _observationBuffer.IsPlacementMismatch;
            observedState.ClampState = _observationBuffer.ClampState;
            observedState.DockState = _observationBuffer.DockState;
            observedState.DoorState = _observationBuffer.DoorState;
            observedState.PlacementErrorState = _observationBuffer.PlacementErrorState;
            observedState.CarrierOutErrorState = _observationBuffer.CarrierOutErrorState;
            observedState.TriggeredAlarm = _observationBuffer.TriggeredAlarm;
            observedState.AccessMode = _observationBuffer.AccessMode;
            observedState.LoadingType = _observationBuffer.LoadingType;
            observedState.CarrierAccessingState = _observationBuffer.CarrierAccessingState;

            // Scheduler 전체 실행이 아니라 completion 확정만 위임한다.
            _actionScheduler.TryFinalizePendingCarrierCompletion(
                PortId,
                observedState);

            // CarrierCompleted가 저장됐을 수 있으므로 E87 평가 전에 다시 읽는다.
            _observationBuffer.CarrierAccessingState =
                _carrierServer.HasCarrier(PortId)
                    ? _carrierServer.GetCarrierAccessingStatus(PortId)
                    : CarrierAccessStates.NotAccessed;
        }
        public void Execute()
        {
            if (Controller == null || _state == null)
                return;

            // 이전값 백업
            LoadPortStateInformation previousState = new LoadPortStateInformation();
            _state.CopyTo(ref previousState);

            Controller.Monitoring();

            FillObservation(ref _observationBuffer);

            if (NeedRecoveryStateInformations(ref previousState))
                return;

            TryFinalizePendingCarrierCompletion();

            // E87 Transfer OOS/IS는 ChangeServiceStatus로만 움직이게 두고,
            // Operator가 Enabled && Initialized 조건을 서비스 요청으로 자동 변환한다.
            SyncServiceStatusFromEnabledAndInitialized(
                _observationBuffer.Enabled,
                _observationBuffer.Initialized);

            _stateModel.UpdateObservation(in _observationBuffer);
            _stateModel.Evaluate();
            _stateModel.CopyStateTo(_state);

            // 최신 _state 반영 직후 이벤트 발행
            OnCarrierDetectionChanged(previousState, _state);

            AssignCarrierByTransferState();
            UpdateAMHSValues();
        }
        #endregion </Gathering>

        #region <ETC>
        private static CarrierDetectionStates GetCarrierDetectionState(bool present, bool placed)
        {
            if (present && placed)
                return CarrierDetectionStates.FullyDetected;

            if (!present && !placed)
                return CarrierDetectionStates.NoneDetected;

            return CarrierDetectionStates.PartiallyDetected;
        }
        public string GetCurrentLocationName()
        {
            LoadPortLocations.TryGetValue(LoadingType, out var loc);
            return loc;
        }
        #endregion </ETC>

        #region <AMHS>
        public bool AssignAMHSSignalControlFunctions(
            Func<int, bool> functionToReadInput,
            Func<int, bool> functionToReadOutput,
            Func<int, bool, DigitalIO_.DIO_RESULT> functionToWriteOutput)
        {
            if (AMHSController == null)
                return false;

            AMHSController.AssignSignalControlFunctions(functionToReadInput, functionToReadOutput, functionToWriteOutput, ref _logger);
            return true;
        }

        public bool AssignActionBeforeCarrierLoads(Func<int, CommandResults> action)
        {
            if (AMHSController == null)
                return false;

            AMHSController.AssignActionBeforeCarrierLoad(action);
            return true;
        }
        public bool WriteAMHSEmergencyStop(bool value)
        {
            if (AMHSController == null)
                return false;

            return AMHSController.WriteEmergencyStop(value);
        }
        public bool WriteAMHSHandoffAvailable(bool value)
        {
            if (AMHSController == null)
                return false;

            return AMHSController.WriteHandoffAvailable(value);
        }
        public bool ReadPIOInput(int inputIndex, bool defaultValue)
        {
            return AMHSController.ReadAMHSPIOInput(inputIndex, defaultValue);
        }
        public bool ReadPIOOutput(int outputIndex)
        {
            return AMHSController.ReadAMHSPIOOutput(outputIndex);
        }
        public bool GetAMHSSaftyInterLockStatus()
        {
            if (AMHSController == null)
                return false;

            return (false == AMHSController.IsInterLockDetected());
        }
        public bool GetAMHSSignalValues(ref Dictionary<int, bool> inputs, ref Dictionary<int, bool> outputs)
        {
            if (AMHSController == null)
                return false;

            AMHSController.GetSignalValues(ref inputs, ref outputs);
            return true;
        }
        public bool GetAMHSInformation(ref AMHSInformation information)
        {
            if (AMHSController == null)
                return false;

            AMHSController.GetSignalInformation(ref information);
            return information != null;
        }

        public void UpdateAMHSValues()
        {
            if (AMHSController == null)
                return;

            AMHSController.ExecuteGatheringSignals(_state);
        }

        public bool InitializeSignals()
        {
            if (AMHSController == null)
                return false;

            AMHSController.InitializeSignals();
            return true;
        }
        //public bool SetNormalStatus()
        //{
        //    if (AMHSController == null)
        //        return false;

        //    AMHSController.SetNormalStatus();
        //    return true;
        //}
        public CommandResults ExecuteAMHSHandlingToLoad()
        {
            if (AMHSController == null)
                return new CommandResults(LoadPortCommands.AMHSLoading.ToString(), CommandResult.Error);

            switch (_actionStep)
            {
                case 0:
                    _logger.WriteOperationStartLog(LoadPortCommands.AMHSLoading);
                    ++_actionStep;
                    break;
            }

            var result = AMHSController.ExecuteToLoadWithAMHS(LoadPortCommands.AMHSLoading);
            switch (result.CommandResult)
            {
                case CommandResult.Proceed:
                    break;
                default:
                    _actionStep = 0;
                    _logger.WriteOperationEndLog(LoadPortCommands.AMHSLoading, result);
                    break;

            }

            return result;
        }
        public CommandResults ExecuteAMHSHandlingToUnload()
        {
            if (AMHSController == null)
                return new CommandResults(LoadPortCommands.AMHSUnloading.ToString(), CommandResult.Error);


            switch (_actionStep)
            {
                case 0:
                    _logger.WriteOperationStartLog(LoadPortCommands.AMHSUnloading);
                    ++_actionStep;
                    break;
            }

            var result = AMHSController.ExecuteToUnloadWithAMHS(LoadPortCommands.AMHSUnloading);
            switch (result.CommandResult)
            {
                case CommandResult.Proceed:
                    break;
                default:
                    _actionStep = 0;
                    _logger.WriteOperationEndLog(LoadPortCommands.AMHSUnloading, result);
                    break;

            }

            return result;
        }
        public bool WriteAMHSOutput(int index, bool newValue)
        {
            if (AMHSController == null)
                return false;

            return AMHSController.WriteOutput(index, newValue);
        }
        public bool WriteAMHSStopSignal(bool newValue)
        {
            if (AMHSController == null)
                return false;

            int index = AMHSController.IndexOfEmergencyStopSignal;
            bool value = newValue;
            switch (AMHSController.InterfaceType)
            {
                case Define.DefineEnumProject.AppConfig.EN_PIO_INTERFACE_TYPE.E84:
                    value = !value;
                    break;
                case Define.DefineEnumProject.AppConfig.EN_PIO_INTERFACE_TYPE.E23:
                    break;

                default:
                    return false;
            }
            return AMHSController.WriteOutput(index, value);
        }
        #endregion </AMHS>

        #endregion </Methods>
    }
}