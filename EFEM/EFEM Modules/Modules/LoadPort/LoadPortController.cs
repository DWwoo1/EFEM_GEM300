using System;
using System.Collections.Generic;
using System.Linq;
using System.Collections.Concurrent;

using TickCounter_;

using Define.DefineEnumBase.Common;
using EFEM.Defines.Common;
using EFEM.Defines.LoadPort;
using EFEM.Defines.Communicator;

namespace EFEM.Modules.LoadPort
{
    public abstract class LoadPortController// : LoadPort, ILoadPortStateHandler, ILoadPortHandler
    {
        #region <Constructors>
        public LoadPortController(int portId, string name, EN_CONNECTION_TYPE interfaceType, int commIndex)// : base(portId, name)
        {
            if (commIndex >= 0)
            {
                _comm = new Communicator(interfaceType, commIndex);
            }
            
            PortId = portId;
            Name = name;
            LoadPortModeChanger = new Dictionary<LoadPortLoadingMode, LoadPortModeEventHandler>();
            TicksForConnection.SetTickCount(RetryInterval);

            CassettePlacement = new ConcurrentDictionary<int, bool>();
            MACPlacement = new ConcurrentDictionary<int, bool>();
        }
        #endregion </Constructors>

        #region <Fields>
        protected readonly int PortId;
        protected readonly string Name;
        private readonly Communicator _comm = null;
        private string _receivedTemporaryMessage = string.Empty;
        private string _receivedMessage = string.Empty;

        protected int _actionStep = 0;
        protected LoadPortCommands _doingAction = LoadPortCommands.Idle;
        protected TickCounter _timeChecker = new TickCounter();

        protected CommandResults _result = new CommandResults(LoadPortCommands.Idle.ToString(), CommandResult.Error);
        protected readonly ConcurrentDictionary<LoadPortButtonTypes, ButtonPressedEventHandler> OnButtonPressedEventHandler
            = new ConcurrentDictionary<LoadPortButtonTypes, ButtonPressedEventHandler>();

        protected readonly Dictionary<LoadPortLoadingMode, LoadPortModeEventHandler> LoadPortModeChanger = null;
        protected SlotMapStateUpdatedEventHandler _slotMapStateUpdatedEventHandler = null;
        protected LoadPortLogger _logger = null;
        protected string _triggeredAlarm = string.Empty;

        private readonly TickCounter TicksForConnection = new TickCounter();
        private readonly uint RetryInterval = 5000;
        private const int RetryCountLimit = 3;
        private int _retryCount;

        private ConcurrentDictionary<int, CarrierSlotMapStates> _readSlotMaps = new ConcurrentDictionary<int, CarrierSlotMapStates>();
        #endregion </Fields>

        #region <Properties>
        public LoadPortActionStates State
        {
            get
            {
                if (false == _doingAction.Equals(LoadPortCommands.Idle))
                    return LoadPortActionStates.Busy;

                return LoadPortActionStates.Idle;
            }
        }
        public bool Initialized { get; private set; }
        public bool Present { get; private set; }
        public bool Placed { get; private set; }
        public ConcurrentDictionary<int, bool> CassettePlacement { get; protected set; }
        public ConcurrentDictionary<int, bool> MACPlacement { get; protected set; }
        public bool ClampState { get; private set; }
        public bool DockState { get; private set; }
        public bool DoorState { get; private set; }
        public LoadPortAccessMode AccessMode { get; private set; }
        public LoadPortLoadingMode LoadingType { get; private set; }
        public IDictionary<int, CarrierSlotMapStates> SlotState
        {
            get
            {
                return _readSlotMaps;
            }
        }
        public bool IsPlacementMismatch
        {
            get
            {
                var placement = (LoadingType == LoadPortLoadingMode.Foup) ? MACPlacement : CassettePlacement;

                // dict가 null이거나 Clear되어 키가 없을 수 있으니 안전하게 TryGet
                if (placement == null)
                    return false;

                if (!placement.TryGetValue(0, out var placement_1)) return false;
                if (!placement.TryGetValue(1, out var placement_2)) return false;
                if (!placement.TryGetValue(2, out var placement_3)) return false;

                return !(placement_1 == placement_2 && placement_2 == placement_3);
            }
        }
        //public ConcurrentDictionary<int, CarrierSlotMapStates> SlotState { get; private set; }
        #endregion </Properties>

        #region <Methods>

        #region <Init/Close>
        public abstract bool InitController();
        public abstract bool CloseController();
        #endregion </Init/Close>

        #region <Interfaces>
        public void AssignLogger(ref LoadPortLogger logger)
        {
            _logger = logger;
        }
        public void AttachModeChangerEventHandler(LoadPortLoadingMode type, LoadPortModeEventHandler eventHandler)
        {
            LoadPortModeChanger[type] = eventHandler;
        }

        public void AttachMechanicalButtonEventHandlers(LoadPortButtonTypes type,
            ButtonPressedEventHandler eventHandler)
        {
            OnButtonPressedEventHandler[type] = eventHandler;
        }

        public void AttachSlotMapStateUpdatedEventHandler(SlotMapStateUpdatedEventHandler eventHandler)
        {
            _slotMapStateUpdatedEventHandler = eventHandler;
        }

        public void InitializeFlags()
        {
            InitAction();
        }

        public CommandResults Initialize()
        {
            return DoInitialize();
        }
        public CommandResults Load()
        {
            return DoLoad();
        }
        public CommandResults Unload()
        {
            return DoUnload();
        }
        public CommandResults Clamp()
        {
            return DoClamp();
        }
        public CommandResults UnClamp()
        {
            return DoUnClamp();
        }
        public CommandResults Dock()
        {
            return DoDock();
        }
        public CommandResults UnDock()
        {
            return DoUnDock();
        }
        public CommandResults OpenDoor()
        {
            return DoOpenDoor();
        }
        public CommandResults CloseDoor()
        {
            return DoCloseDoor();
        }
        public CommandResults Scan()
        {
            return DoScan();
        }
        public CommandResults GetSlotMap()
        {
            return DoGetSlotMap();
        }
        public CommandResults ChangeLoadingMode(LoadPortLoadingMode mode)
        {
            return DoChangeLoadingMode(mode);
        }
        public CommandResults ClearAlarm()
        {
            return DoClearAlarm();
        }
        public CommandResults AmpControl(bool enabled)
        {
            return DoAmpControl(enabled);
        }
        #endregion </Interfaces>

        #region <Actions>
        public virtual void InitAction()
        {
            _actionStep = 0;
            _doingAction = LoadPortCommands.Idle;
            _triggeredAlarm = string.Empty;
        }
        public virtual void OnButtonPressed(LoadPortButtonTypes buttonType)
        {
            if (OnButtonPressedEventHandler != null && OnButtonPressedEventHandler.ContainsKey(buttonType))
            {
                if (false == OnButtonPressedEventHandler.TryGetValue(buttonType,
                    out ButtonPressedEventHandler CallbackButtonPressed))
                    return;

                CallbackButtonPressed();
            }
        }
        public abstract CommandResults DoInitialize();
        public abstract CommandResults DoLoad();
        public abstract CommandResults DoUnload();
        public abstract CommandResults DoClamp();
        public abstract CommandResults DoUnClamp();
        public abstract CommandResults DoDock();
        public abstract CommandResults DoUnDock();
        public abstract CommandResults DoOpenDoor();
        public abstract CommandResults DoCloseDoor();
        public abstract CommandResults DoScan();
        public abstract CommandResults DoGetSlotMap();
        public abstract CommandResults DoFindLoadingMode();
        public abstract CommandResults DoChangeLoadingMode(LoadPortLoadingMode mode);
        public abstract CommandResults DoChangeAccessMode(LoadPortAccessMode mode);
        public abstract CommandResults DoClearAlarm();
        public abstract CommandResults DoAmpControl(bool enabled);
        public abstract string GetTriggeredControllerAlarm();
        #endregion </Actions>

        #region <States>
        public abstract void OnIndicatorChanged(LoadPortIndicatorTypes indicator, LoadPortIndicatorStates state);
        public void RemoveCarrierMap()
        {
            _readSlotMaps = null;
        }
        protected void ChangeInitializationState(bool newState)
        {
            if (Initialized != newState)
            {
                if (this is LoadPortControllers.LoadPortControllerSimulator)
                {
                    _logger.WriteCommLog($"Initialized : {Initialized} -> {newState}", true);
                }
                    
                Initialized = newState;
            }
        }
        protected void ChangePresentState(bool newState)
        {
            if (Present != newState)
            {
                if (this is LoadPortControllers.LoadPortControllerSimulator)
                {
                    _logger.WriteCommLog($"Present : {Present} -> {newState}", true);
                }

                Present = newState;
            }
        }
        protected void ChangePlacedState(bool newState)
        {
            if (Placed != newState)
            {
                if (this is LoadPortControllers.LoadPortControllerSimulator)
                {
                    _logger.WriteCommLog($"Placed : {Placed} -> {newState}", true);
                }

                Placed = newState;
            }
        }
        protected void ChangeClampState(bool newState)
        {
            if (ClampState != newState)
            {
                if (this is LoadPortControllers.LoadPortControllerSimulator)
                {
                    _logger.WriteCommLog($"ClampState : {ClampState} -> {newState}", true);
                }

                ClampState = newState;
            }
        }
        protected void ChangeDockState(bool newState)
        {
            if (DockState != newState)
            {
                if (this is LoadPortControllers.LoadPortControllerSimulator)
                {
                    _logger.WriteCommLog($"DockState : {DockState} -> {newState}", true);
                }

                DockState = newState;
            }
        }
        protected void ChangeDoorState(bool newState)
        {
            if (DoorState != newState)
            {
                if (this is LoadPortControllers.LoadPortControllerSimulator)
                {
                    _logger.WriteCommLog($"DoorState : {DoorState} -> {newState}", true);
                }

                DoorState = newState;
            }
        }
        protected void ChangeAccessingState(LoadPortAccessMode newState)
        {
            if (AccessMode != newState)
            {
                if (this is LoadPortControllers.LoadPortControllerSimulator)
                {
                    _logger.WriteCommLog($"AccessMode : {AccessMode} -> {newState}", true);
                }

                AccessMode = newState;
            }
        }
        protected void ChangeLoadingTypeState(LoadPortLoadingMode carrierType)
        {
            if (false == LoadingType.Equals(carrierType))
            {
                if (this is LoadPortControllers.LoadPortControllerSimulator)
                {
                    _logger.WriteCommLog($"LoadingType : {LoadingType} -> {carrierType}", true);
                }

                LoadingType = carrierType;
            }
        }
        protected void ChangeSlotMap(IReadOnlyDictionary<int, CarrierSlotMapStates> mapState)
        {
            if (_readSlotMaps == null)
                _readSlotMaps = new ConcurrentDictionary<int, CarrierSlotMapStates>();
            _readSlotMaps.Clear();
            foreach (var item in mapState)
            {
                _readSlotMaps[item.Key] = item.Value;
            }

            _slotMapStateUpdatedEventHandler?.Invoke(PortId, _readSlotMaps);
        }
        #endregion </States>

        #region <Communication>
        public bool IsConnected()
        {
            if (this is LoadPortControllers.LoadPortControllerSimulator)
                return true;

            if (_comm == null)
                return false;

            return _comm.IsConnected;
        }
        protected bool DoAction(LoadPortCommands command, byte[] messageToSend)
        {
            if (false == IsConnected())
                return false;

            if (false == _doingAction.Equals(LoadPortCommands.Idle))
                return false;

            _doingAction = command;

            return _comm.WriteByteData(messageToSend);
        }

        protected bool DoAction(LoadPortCommands command, string messageToSend)
        {
            if (false == IsConnected())
                return false;

            if (false == _doingAction.Equals(LoadPortCommands.Idle))
                return false;

            _doingAction = command;
            
            _logger.WriteCommLog(messageToSend, false);

            return _comm.WriteStringData(messageToSend);
        }

        protected bool ReceiveByteData(ref byte[] receivedMessage)
        {
            if (false == IsConnected())
                return false;

            return _comm.ReadByteData(ref receivedMessage);
        }

        protected bool ReceiveStringData(ref string receivedMessage)
        {
            if (false == IsConnected())
                return false;

            return _comm.ReadStringData(ref receivedMessage);
        }
        // 2024.12.21. jhlim [ADD] 연결되지 않는 경우 무한으로 익셉션이 발생해 로그가 계속 쌓이게된다..
        private bool RetryConnectIfNeed()
        {
            if (false == IsConnected())
            {
                if (_retryCount < RetryCountLimit && TicksForConnection.IsTickOver(false))
                {
                    _comm.OpenPort();

                    TicksForConnection.SetTickCount(RetryInterval);
                    ++_retryCount;
                }

                return false;
            }
            else
            {
                // Flag Reset
                _retryCount = 0;
                TicksForConnection.SetTickCount(RetryInterval);

                return true;
            }
        }
        // 2024.12.21. jhlim [END]
        #endregion </Communication>

        #region <Thread>
        protected abstract void ParseMessages(string receivedMessage);
        protected abstract bool RemoveTokens(string receivedMessage, ref string newString);
        public void Monitoring()
        {
            if (false == (this is LoadPortControllers.LoadPortControllerSimulator))
            {
                if (_comm == null)
                    return;

                // 2024.12.21. jhlim [MOD] 내용이 길어져 메서드로 변경
                if (false == RetryConnectIfNeed())
                    return;

                //if (false == _comm.IsConnected)
                //{
                //    _comm.OpenPort();

                //    return;
                //}
                // 2024.12.21. jhlim [END]

                _receivedTemporaryMessage = string.Empty; _receivedMessage = string.Empty;
                if (false == _comm.ReadStringData(ref _receivedTemporaryMessage))
                    return;

                if (false == RemoveTokens(_receivedTemporaryMessage, ref _receivedMessage))
                    return;

                _logger.WriteCommLog(_receivedMessage, true);
            }

            ParseMessages(_receivedMessage);
        }
        #endregion </Thread>

        #endregion </Methods>
    }
}