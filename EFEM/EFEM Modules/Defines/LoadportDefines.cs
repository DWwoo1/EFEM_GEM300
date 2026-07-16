using System;
using System.Diagnostics;
using System.Linq;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Concurrent;

using EFEM.Defines.Common;
using EFEM.MaterialTracking;
using EFEM.Modules.LoadPort.Recovery;

using FrameOfSystem3.Recipe;

namespace EFEM.Defines.LoadPort
{
    #region <Enumerations>

    #region <E87>
    public enum LoadPortTransferStates
    {
        Unknown = -1,
        OutOfService,
        InService,
        TransferBlocked,
        ReadyToLoad,
        ReadyToUnload
    }
    // TODO : UI상 초기값 설정 필요
    public enum CarrierIdVerificationStates
    {
        NotRead,
        WaitingForHost,
        VerificationOk,
        VerificationFailed
    }
    
    public enum CarrierSlotMapVerificationStates
    {
        NotRead,
        WaitingForHost,
        VerificationOk,
        VerificationFailed
    }

    // [영속화 enum] 저장은 멤버 "이름"으로만 한다(정수 저장 금지). 멤버 재배치/삭제 금지 — 필요시 끝에만 추가.
    // 주의(레거시 호환): 5.18 이하 버전은 맨 앞에 Unknown=0 이 있어 정수 저장 시 값이 1씩 컸다
    //   (5.18: Unknown=0, NotAccessed=1, InAccessed=2, CarrierCompleted=3, CarrierStopped=4).
    //   커밋 6c82adf(2026-04-23)에서 Unknown을 제거해 ordinal이 밀렸고, 이로 인해 5.18이 저장한
    //   InAccessed(=2)가 신버전에서 CarrierCompleted(=2)로 오독되어 미처리 캐리어가 조기 배출되는 사고가 있었다.
    //   레거시 정수 복구 데이터는 반드시 1회 변환기(scheme-aware)에서 이 5.18 매핑으로 해석해야 한다.
    public enum CarrierAccessStates
    {
        //Unknown,   // 5.18 이하에서 0 이었음(제거됨). 절대 다시 넣지 말 것 — ordinal이 다시 밀린다.
        NotAccessed = 0,
        InAccessed = 1,
        CarrierCompleted = 2,
        CarrierStopped = 3,
    }
    // [영속화 enum] 저장은 이름으로. 멤버 재배치/삭제 금지 — 끝에만 추가.
    public enum CarrierSlotMapStates
    {
        Undefined = 0,          // 초기 상태
        Empty = 1,              // 자재 없음
        NotEmpty = 2,           // 있으나 사용 불가
        CorrectlyOccupied = 3,  // 자재 있음(정상)
        DoubleSlotted = 4,      // 슬롯 중첩(이중 감지)
        CrossSlotted = 5,       //
    }

    public enum LoadPortAccessMode
    {
        Manual = 0,
        Auto,
    }
    #endregion </E87>

    #region <Customs>
    public enum LoadPortButtonTypes
    {
        Load = 0,
        Unload,
    }
    public enum LoadPortIndicatorTypes
    {
        Load = 0,
        Unload,
        Auto,
        Manual,
        Reserved1,
        Reserved2,
        Reserved3,
        Reserved4,
    }
    public enum LoadPortIndicatorStates
    {
        Off = 0,
        On,
        Blink
    }
    
    public enum LoadPortActionStates
    {
        Idle,
        Busy,
        Fault,
    }
    public enum LoadPortCommands
    {
        Idle,
        Load,
        Unload,
        Clamp,
        Unclamp,
        Dock,
        Undock,
        DoorOpen,
        DoorClose,
        Hello,
        Initialize,
        Scan,
        ScanDown,
        Reset,
        AmpOn,
        AmpOff,
        GetState,
        GetMap,
        GetCapacity,
        FindLoadingMode,
        ChangeToCassette,
        ChangeToClosedCassette,
        ChangeToFoup,
        ChangeAccessModeToAuto,
        ChangeAccessModeToManual,
        GetAcceessingMode,
        AMHSLoading,
        AMHSUnloading,

        LedOn,          // 2024.11.14. by dwlim [ADD] SELOP8 LED I/F 추가
        LedOff,         // 2024.11.14. by dwlim [ADD] SELOP8 LED I/F 추가
        LedBlink,       // 2024.11.14. by dwlim [ADD] SELOP8 LED I/F 추가
        LedStatus,      // 2024.11.18. by dwlim [ADD] SELOP8 LED Status 추가
    }
    public enum LoadPortLoadingMode
    {
        Unknown = -1,
        Foup,
        Cassette,
        ClosedCassette,
    }

    public enum VarificationResults
    {
        Proceed,
        Completed,
        Error,
    }
    public enum E23InputSignals
    {
        Valid = 0,
        CarrierStage_0,
        CarrierStage_1,
        CarrierStage_2,
        CarrierStage_3,
        TransferRequest,
        Busy,
        Complete,
    }
    public enum E23OutputSignals
    {
        LoadRequest = 0,
        UnloadRequest,
        Abort,
        Ready,
        Spare_1,
        Spare_2,
        Spare_3,
        Spare_4,
    }
    public enum E84InputSignals
    {
        Valid = 0,
        CarrierStage_0,
        CarrierStage_1,
        Spare_1,
        TransferRequest,
        Busy,
        Complete,
        ContinuousHandoff,

        //Valid = 0,
        //CarrierStage_0,
        //CarrierStage_1,
        //TransferRequest,
        //Busy,
        //Complete,
        //ContinuousHandoff,
        //Spare_1,
    }
    public enum E84OutputSignals
    {
        LoadRequest = 0,
        UnloadRequest,
        Spare_1,
        Ready,
        Spare_2,
        Spare_3,
        HandoffAvailable,
        EmergencyStop,

        //LoadRequest = 0,
        //UnloadRequest,
        //Ready,
        //HandoffAvailable,
        //EmergencyStop,
        //Spare_1,
        //Spare_2,
        //Spare_3,
    }
    public enum PIOProgressForStateModel
    {
        Ready,
        Complete,
    }
    #endregion </Customs>

    #region <Scheduler>
    public enum CARRIER_PORT_TYPE
    {
        SELECTION,
        READY_TO_LOAD,
        ACTION_LOAD,
        READY_TO_UNLOAD,
        ACTION_UNLOAD
    }
    #endregion </Scheduler>

    #endregion </Enumerations>

    #region <Class&Struct>
    public class LoadPortLogger : ModuleLogger
    {
        public LoadPortLogger(string logType, string name) : base(logType, name, true) { }

        public void WriteOperationStartLog(LoadPortCommands command)
        {
            WriteLog(LogTitleTypes.OPER, string.Format("----- {0} -----", command.ToString()));
        }
        public void WriteOperationEndLog(LoadPortCommands command, CommandResults result)
        {
            if (result.CommandResult == CommandResult.Proceed)
                return;

            WriteLog(LogTitleTypes.OPER, string.Format("----- {0}, Result : {1}, Description : {2}", command.ToString(), result.CommandResult.ToString(), result.Description));
        }
        public void WriteCommLog(string message, bool received)
        {
            if (false == received)
            {
                WriteLog(LogTitleTypes.SEND, message);
            }
            else
            {
                WriteLog(LogTitleTypes.RECV, message);
            }

        }        
        public void WriteSignalChangedLog(string signalName, bool changedValue, bool input)
        {
            string message = string.Format("Signal Changed : {0} -> {1}", signalName, changedValue.ToString());
            if (input)
            {
                WriteLog(LogTitleTypes.IN, message);
            }
            else
            {
                WriteLog(LogTitleTypes.OUT, message);
            }
        }
        public void WriteCarrierStatusChangedLog(string signalName, bool changedValue)
        {
            string message = string.Format("Signal Changed : {0} -> {1}", signalName, changedValue.ToString());
            WriteLog(LogTitleTypes.CARR, message);            
        }
        public void WriteCarrrierEvent(bool created)
        {
            if (created)
            {
                WriteLog(LogTitleTypes.CARR, "Carrier has been created");
            }
            else
            {
                WriteLog(LogTitleTypes.CARR, "Carrier has been removed");
            }
        }
        // 2026.07.08. jhlim [ADD] RFID 읽기/쓰기 기록 일반화. RFIDManager가 포트별로 이 로거를 통해 호출한다.
        public void WriteRfidLog(string message)
        {
            WriteLog(LogTitleTypes.RFID, message);
        }
    }
    public class LoadPortStateInformation
    {
        #region <Properties>
        public bool Enabled { get; set; }
        public bool Initialized { get; set; }
        public bool Present { get; set; }
        public bool Placed { get; set; }
        public bool IsPlacementMismatch { get; set; }
        public bool ClampState { get; set; }
        public bool DockState { get; set; }
        public bool DoorState { get; set; }
        public string AssociatedCarrierId { get; set; }
        public bool PlacementErrorState { get; set; }
        public bool CarrierOutErrorState { get; set; }
        public string TriggeredAlarm { get; set; }
        public LoadPortAccessMode AccessMode { get; set; }
        public LoadPortLoadingMode LoadingType { get; set; }
        public LoadPortTransferStates TransferState { get; set; }
        public CarrierAccessStates CarrierAccessingState { get; set; }
        public CarrierIdVerificationStates CarrierIdVerificationState { get; set; }
        public CarrierSlotMapVerificationStates CarrierSlotMapVerificationState { get; set; }
        public ReservationStates ReservationState { get; set; }
        public AssociationStates AssociationState { get; set; }
        #endregion </Properties>

        #region <Methods>
        public void CopyTo(ref LoadPortStateInformation instance)
        {
            if (instance == null)
            {
                instance = new LoadPortStateInformation();
            }
            instance.Enabled = Enabled;
            instance.Initialized = Initialized;
            instance.Present = Present;
            instance.Placed = Placed;
            instance.IsPlacementMismatch = IsPlacementMismatch;
            instance.ClampState = ClampState;
            instance.DockState = DockState;
            instance.DoorState = DoorState;
            instance.AccessMode = AccessMode;
            instance.LoadingType = LoadingType;
            instance.AssociationState = AssociationState;
            instance.TransferState = TransferState;
            instance.PlacementErrorState = PlacementErrorState;
            instance.CarrierOutErrorState = CarrierOutErrorState;
            instance.TriggeredAlarm = TriggeredAlarm;
            instance.CarrierAccessingState = CarrierAccessingState;
            instance.CarrierIdVerificationState = CarrierIdVerificationState;
            instance.CarrierSlotMapVerificationState = CarrierSlotMapVerificationState;
            instance.AssociationState = AssociationState;
            instance.ReservationState = ReservationState;
            instance.AssociatedCarrierId = AssociatedCarrierId;
        }
        #endregion </Methods>
    }
    public class AMHSInformation
    {
        public AMHSInformation(Define.DefineEnumProject.AppConfig.EN_PIO_INTERFACE_TYPE interfaceType, int interlockIndex,
            Dictionary<int, Tuple<int, string>> digitalInputs, Dictionary<int, Tuple<int, string>> digitalOutputs)
        {
            InterfaceType = interfaceType;

            SaftyInterLockIndex = interlockIndex;

            if (digitalInputs != null)
            {
                DigitalInputs = new ReadOnlyDictionary<int, Tuple<int, string>>(digitalInputs);
            }

            if (digitalOutputs != null)
            {
                DigitalOutputs = new ReadOnlyDictionary<int, Tuple<int, string>>(digitalOutputs);
            }
        }

        public readonly Define.DefineEnumProject.AppConfig.EN_PIO_INTERFACE_TYPE InterfaceType;
        
        public readonly int SaftyInterLockIndex;
        public readonly ReadOnlyDictionary<int, Tuple<int, string>> DigitalInputs = null;
        public readonly ReadOnlyDictionary<int, Tuple<int, string>> DigitalOutputs = null;
    }
    public abstract class AutomatedMaterialHandlingSystemController
    {
        public AutomatedMaterialHandlingSystemController(int lpIndex, AMHSInformation information)
        {
            Index = lpIndex;
            Information = information;

            InputSignalValues = new ConcurrentDictionary<int, bool>();
            InputSignalNames = new Dictionary<int, string>();
            if (Information.DigitalInputs != null)
            {
                foreach (var item in Information.DigitalInputs)
                {
                    int index = item.Value.Item1;
                    InputSignalValues[index] = false;
                    InputSignalNames[index] = item.Value.Item2;
                }
            }

            OutputSignalValues = new ConcurrentDictionary<int, bool>();
            OutputSignalNames = new Dictionary<int, string>();
            if (Information.DigitalOutputs != null)
            {
                foreach (var item in Information.DigitalOutputs)
                {
                    int index = item.Value.Item1;
                    OutputSignalValues[index] = false;
                    OutputSignalNames[index] = item.Value.Item2;
                }
            }

            SaftyInterLockIndex = Information.SaftyInterLockIndex;
            EmergencyStopIndex = GetEmergencyStopSignalIndex();
            
            _taskOperator = FrameOfSystem3.Task.TaskOperator.GetInstance();

            _status = new LoadPortStateInformation();

            InputSignalValuesForSimulation = new ConcurrentDictionary<int, bool>(InputSignalValues);
            OutputSignalValuesForSimulation = new ConcurrentDictionary<int, bool>(OutputSignalValues);
        }

        #region <Fields>
        protected Func<int, CommandResults> actionBeforeCarrierLoad = null;
        protected Func<int, CommandResults> actionBeforeCarrierUnload = null;
        protected Func<int, LoadPortLoadingMode, CommandResults> modeChangeBeforeCarrierLoad = null;

        private Func<int, bool> readInput = null;
        private Func<int, bool> readOutput = null;
        private Func<int, bool, DigitalIO_.DIO_RESULT> _writeOutput = null;
        private readonly int SaftyInterLockIndex;
        private readonly int EmergencyStopIndex;
        private static FrameOfSystem3.Task.TaskOperator _taskOperator = null;

        protected readonly int Index;
        protected int _seqNum = 0;
        protected CommandResults _commandResult;

        private readonly TickCounter_.TickCounter TimerOverTicks = new TickCounter_.TickCounter();
        private readonly TickCounter_.TickCounter TimerOverTicksForPresence = new TickCounter_.TickCounter();
        private readonly TickCounter_.TickCounter TimerOverTicksForDelay = new TickCounter_.TickCounter();

        protected TimeSpan _chatteringTimeLimit = new TimeSpan();

        protected readonly ConcurrentDictionary<int, bool> InputSignalValues = null;        // Key : SignalIndex, Value : Signal Value
        protected readonly ConcurrentDictionary<int, bool> OutputSignalValues = null;       // Key : SignalIndex, Value : Signal Value

        protected readonly AMHSInformation Information = null;

        protected LoadPortStateInformation _status;
        protected LoadPortLogger _logger;

        private bool _temporaryReadInputValue = false;
        private bool _temporaryReadOutputValue = false;
        private readonly Dictionary<int, string> InputSignalNames = null;
        private readonly Dictionary<int, string> OutputSignalNames = null;

        private readonly ConcurrentDictionary<int, bool> InputSignalValuesForSimulation = null;        // Key : SignalIndex, Value : Signal Value
        private readonly ConcurrentDictionary<int, bool> OutputSignalValuesForSimulation = null;       // Key : SignalIndex, Value : Signal Value
        private const string CarrierPresence = "CarrierPresence";
        private const string CarrierPlacement = "CarrierPlacement";
        private bool _carrierPresenceStatus;
        private bool _carrierPlacementStatus;

        private LoadPortCommands _currentCommand;

        private Action _notifyTransferReadyForLoad = null;
        private Action _notifyTransferCompleteForLoad = null;
        private Action _notifyTransferReadyForUnload = null;
        private Action _notifyTransferCompleteForUnload = null;

        public int? ReadySignalIndex { get; protected set; }
        public int? CompleteSignalIndex { get; protected set; }
        #endregion </Fields>

        #region <Properites>
        public Define.DefineEnumProject.AppConfig.EN_PIO_INTERFACE_TYPE InterfaceType
        {
            get
            {
                if (Information == null)
                    return Define.DefineEnumProject.AppConfig.EN_PIO_INTERFACE_TYPE.E84;

                return Information.InterfaceType;
            }
        }
        public int IndexOfEmergencyStopSignal
        {
            get
            {
                if (Information == null)
                    return -1;

                return EmergencyStopIndex;
            }
        }
        public int PortId { get; set; }
        #endregion </Properites>

        #region <Methods>

        #region <Assigns>
        public void AssignSignalControlFunctions(
            Func<int, bool> functionToReadInput,
            Func<int, bool> functionToReadOutput,
            Func<int, bool, DigitalIO_.DIO_RESULT> functionToWriteOutput,
            ref LoadPortLogger lpLogger)
        {
            readInput = functionToReadInput;
            readOutput = functionToReadOutput;
            _writeOutput = functionToWriteOutput;
            _logger = lpLogger;
        }
        public void AssignActionBeforeCarrierLoad(Func<int, CommandResults> action)
        {
            actionBeforeCarrierLoad = action;
        }
        public void AssignActionBeforeCarrierUnload(Func<int, CommandResults> action)
        {
            actionBeforeCarrierUnload = action;
        }
        public void AssignActionModeChangeBeforeCarrierLoad(Func<int, LoadPortLoadingMode, CommandResults> action)
        {
            modeChangeBeforeCarrierLoad = action;
        }
        public void RegisterTransferNotifications(
            Action readyForLoad,
            Action completeForLoad,
            Action readyForUnload,
            Action completeForUnload)
        {
            _notifyTransferReadyForLoad = readyForLoad;
            _notifyTransferCompleteForLoad = completeForLoad;
            _notifyTransferReadyForUnload = readyForUnload;
            _notifyTransferCompleteForUnload = completeForUnload;
        }
        #endregion </Assigns>

        #region <TaskOperator>
        protected bool IsFinishingMode()
        {
            return _taskOperator.IsFinishingMode();
        }
        protected bool IsSimulationMode()
        {
            return _taskOperator.IsSimulationMode();
        }
        #endregion </TaskOperator>

        #region <IO Control>        
        public void GetSignalInformation(ref AMHSInformation information)
        {
            information = Information;
        }
        public void GetSignalValues(ref Dictionary<int, bool> inputs, ref Dictionary<int, bool> outputs)
        {
            inputs = InputSignalValues.ToDictionary(item => item.Key, item => item.Value);
            outputs = OutputSignalValues.ToDictionary(item => item.Key, item => item.Value);
        }
        public bool IsInterLockDetected()
        {
            if (readInput == null)
                return true;

            return readInput(SaftyInterLockIndex);
        }
        public void ExecuteGatheringSignals(LoadPortStateInformation status)
        {
            _status = status;

            if (readInput != null)
            {
                foreach (var item in InputSignalValues)
                {
                    int index = item.Key;
                    if (false == IsSimulationMode())
                    {
                        _temporaryReadInputValue = readInput(index);
                    }
                    else
                    {
                        _temporaryReadInputValue = InputSignalValuesForSimulation[index];
                    }

                    if (InputSignalValues[index] != _temporaryReadInputValue)
                    {
                        InputSignalValues[index] = _temporaryReadInputValue;

                        if (CompleteSignalIndex.HasValue &&
                            CompleteSignalIndex.Value == index &&
                            _temporaryReadInputValue)
                        {
                            NotifyProgressToOperator(PIOProgressForStateModel.Complete);
                        }

                        _logger.WriteSignalChangedLog(InputSignalNames[index], _temporaryReadInputValue, true);
                    }
                    
                }
            }

            if (readOutput != null)
            {
                foreach (var item in OutputSignalValues)
                {
                    int index = item.Key;
                    if (false == IsSimulationMode())
                    {
                        _temporaryReadOutputValue = readOutput(index);
                    }
                    else
                    {
                        _temporaryReadOutputValue = OutputSignalValuesForSimulation[index];
                    }

                    if (OutputSignalValues[index] != _temporaryReadOutputValue)
                    {
                        OutputSignalValues[index] = _temporaryReadOutputValue;

                        if (ReadySignalIndex.HasValue &&
                            ReadySignalIndex.Value == index &&
                            _temporaryReadOutputValue)
                        {
                            NotifyProgressToOperator(PIOProgressForStateModel.Ready);
                        }

                        _logger.WriteSignalChangedLog(OutputSignalNames[index], _temporaryReadOutputValue, false);
                    }                    
                }
            }

            if (_status != null)
            {
                if (_carrierPresenceStatus != _status.Present)
                {
                    _carrierPresenceStatus = _status.Present;
                    _logger.WriteCarrierStatusChangedLog(CarrierPresence, _carrierPresenceStatus);
                }

                if (_carrierPlacementStatus != _status.Placed)
                {
                    _carrierPlacementStatus = _status.Placed;
                    _logger.WriteCarrierStatusChangedLog(CarrierPlacement, _carrierPlacementStatus);
                }

            }
        }
        protected bool ReadInput(int index, bool defaultSignal)
        {
            if (_taskOperator.IsSimulationMode())
            {
                InputSignalValuesForSimulation[index] = defaultSignal;
            }

            return InputSignalValues[index];
        }
        protected bool ReadOutput(int index)
        {
            if (_taskOperator.IsSimulationMode())
            {
                return OutputSignalValuesForSimulation[index];
            }

            return OutputSignalValues[index];
        }
        public bool WriteOutput(int index, bool newValue)
        {
            if (_taskOperator.IsSimulationMode())
            {
                OutputSignalValuesForSimulation[index] = newValue;
                return true;
            }

            //OutputSignalValues[index] = newValue;
            return _writeOutput(index, newValue).Equals(DigitalIO_.DIO_RESULT.OK);
        }
        #endregion </IO Control>

        #region <Timer>
        protected void SetTickCountForDelay(uint ticks)
        {
            TimerOverTicksForDelay.SetTickCount(ticks);
        }
        protected bool IsTickOverForDelay()
        {
            return TimerOverTicksForDelay.IsTickOver(true);
        }
        protected void SetTickCountForPresence(uint ticks)
        {
            TimerOverTicksForPresence.SetTickCount(ticks);
        }
        protected bool IsTickOverForPresence()
        {
            return TimerOverTicksForPresence.IsTickOver(true);
        }
        protected void SetTickCount(uint ticks)
        {
            TimerOverTicks.SetTickCount(ticks);
        }
        protected bool IsTickOver()
        {
            return TimerOverTicks.IsTickOver(true);
        }
        #endregion </Timer>

        #region <Seq>
        protected CommandResults ReturnResultGoodOrNg(LoadPortCommands command, CommandResult commandResult, string description)
        {
            if (false == Information.InterfaceType.Equals(Define.DefineEnumProject.AppConfig.EN_PIO_INTERFACE_TYPE.E84))
            {
                InitializeSignals();
            }

            _seqNum = 0;
            _commandResult.ActionName = command.ToString();
            _commandResult.CommandResult = commandResult;
            _commandResult.Description = description;

            if (IsSimulationMode())
            {
                foreach (var item in InputSignalValuesForSimulation)
                {
                    InputSignalValuesForSimulation[item.Key] = false;
                }
            }

            return _commandResult;
        }
        protected CommandResults ExecuteActionBeforeLoad(int lpIndex, LoadPortCommands command)
        {
            if (actionBeforeCarrierLoad == null)
                return new CommandResults(command.ToString(), CommandResult.Completed);

            return actionBeforeCarrierLoad(lpIndex);
        }
        protected CommandResults ExecuteActionBeforeUnload(int lpIndex, LoadPortCommands command)
        {
            if (actionBeforeCarrierUnload == null)
                return new CommandResults(command.ToString(), CommandResult.Completed);

            return actionBeforeCarrierUnload(lpIndex);
        }
        protected CommandResults ExecuteModeChangeAction(int lpIndex, LoadPortLoadingMode mode, LoadPortCommands command)
        {
            if (mode.Equals(LoadPortLoadingMode.Unknown))
                return new CommandResults(command.ToString(), CommandResult.Completed);

            if (modeChangeBeforeCarrierLoad == null)
                return new CommandResults(command.ToString(), CommandResult.Completed);

            return modeChangeBeforeCarrierLoad(lpIndex, mode);
        }
        #endregion </Seq>

        #region <Abstracts>
        public abstract void InitializeSignals();
        public abstract bool ReadAMHSPIOInput(int inputIndex, bool defaultSignal);
        public abstract bool ReadAMHSPIOOutput(int outputIndex);
        public CommandResults ExecuteToLoadWithAMHS(LoadPortCommands command)
        {
            _currentCommand = command;
            return ExecuteHandlingToLoad(command);
        }
        public CommandResults ExecuteToUnloadWithAMHS(LoadPortCommands command)
        {
            _currentCommand = command;
            return ExecuteHandlingToUnload(command);
        }
        public abstract int GetEmergencyStopSignalIndex();
        public virtual bool WriteHandoffAvailable(bool value) { return false; }
        public virtual bool WriteEmergencyStop(bool value) { return false; }
        public virtual bool CheckIsInAccessViolation() { return false; }
        public virtual bool IsPIOInterfaceWorking() { return false; }
        public virtual bool IsAnyPIOInputSignalOn() { return false; }
        public virtual bool IsAnyPIOOutputSignalOn() { return false; }
        //public virtual void SetNormalStatus() { }
        public virtual Dictionary<string, bool> HasActivePIOInputs() { return null; }
        public virtual LoadPortLoadingMode CheckTriggerLoadingMode()
        {
            return LoadPortLoadingMode.Unknown;
        }
        protected abstract CommandResults ExecuteHandlingToLoad(LoadPortCommands command);
        protected abstract CommandResults ExecuteHandlingToUnload(LoadPortCommands command);
        protected bool IsCarrierPlaced(LoadPortCommands command)
        {
            if (_status == null)
                return false;

            switch (command)
            {
                case LoadPortCommands.AMHSLoading:
                    {
                        if (IsSimulationMode())
                        {
                            _taskOperator.TriggerLoadPortPlacedForSimul(PortId);
                        }

                        return _carrierPlacementStatus;
                    }

                case LoadPortCommands.AMHSUnloading:
                    {
                        if (IsSimulationMode())
                        {
                            _taskOperator.TriggerLoadPortRemovedForSimul(PortId);
                        }

                        return !_carrierPlacementStatus;
                    }

                default:
                    return false;
            }
        }
        protected bool IsCarrierPresence(LoadPortCommands command)
        {
            if (_status == null)
                return false;

            switch (command)
            {
                case LoadPortCommands.AMHSLoading:
                    {
                        if (IsSimulationMode())
                        {
                            _taskOperator.TriggerLoadPortPlacedForSimul(PortId);
                        }

                        return _carrierPresenceStatus;
                    }

                case LoadPortCommands.AMHSUnloading:
                    {
                        if (IsSimulationMode())
                        {
                            _taskOperator.TriggerLoadPortRemovedForSimul(PortId);
                        }

                        return !_carrierPresenceStatus;
                    }

                default:
                    return false;
            }
        }
        protected bool GetTriggerCarrierPresence(LoadPortCommands command)
        {
            if (_status == null)
                return false;
            
            switch (command)
            {
                case LoadPortCommands.AMHSLoading:
                    {
                        if (IsSimulationMode())
                        {
                            _taskOperator.TriggerLoadPortPlacedForSimul(PortId);
                        }

                        return (_carrierPresenceStatus && _carrierPlacementStatus);
                    }
                    
                case LoadPortCommands.AMHSUnloading:
                    {
                        if (IsSimulationMode())
                        {
                            _taskOperator.TriggerLoadPortRemovedForSimul(PortId);
                        }

                        return (false == _carrierPresenceStatus && false == _carrierPlacementStatus);
                    }

                default:
                    return false;
            }
        }
        protected bool CheckCarrierReadyForCommand(LoadPortCommands command)
        {
            if (_status == null)
                return false;

            switch (command)
            {
                case LoadPortCommands.AMHSLoading:
                    {
                        return (false == _carrierPresenceStatus && false == _carrierPlacementStatus);
                    }

                case LoadPortCommands.AMHSUnloading:
                    {
                        return (_carrierPresenceStatus && _carrierPlacementStatus);
                    }

                default:
                    return false;
            }
        }
        protected bool IsPlacementMismatch()
        {
            if (_status == null)
                return true;

            if (IsSimulationMode())
                return false;

            return _status.IsPlacementMismatch;
        }
        protected void NotifyProgressToOperator(PIOProgressForStateModel progress)
        {
            switch (progress)
            {
                case PIOProgressForStateModel.Ready:
                    {
                        if (_currentCommand == LoadPortCommands.AMHSLoading)
                        {
                            _notifyTransferReadyForLoad?.Invoke();
                        }
                        else if (_currentCommand == LoadPortCommands.AMHSUnloading)
                        {
                            _notifyTransferReadyForUnload?.Invoke();
                        }
                    }
                    break;
                case PIOProgressForStateModel.Complete:
                    {
                        if (_currentCommand == LoadPortCommands.AMHSLoading)
                        {
                            _notifyTransferCompleteForLoad?.Invoke();
                        }
                        else if (_currentCommand == LoadPortCommands.AMHSUnloading)
                        {
                            _notifyTransferCompleteForUnload?.Invoke();
                        }
                    }
                    break;

                default:
                    break;
            }
        }
        #endregion </Abstracts>

        #region <Notification>

        #endregion </Notification>

        #endregion </Methods>
    }

    /*
     * - ABORT는 NORMAL OFF 이며, 에러 발생 시 ON
     * - ERROR 초기화 시 신호 RESET
     * - VALID 신호 OFF 시 신호 RESET
     * 
     * 1. OHT의 [VALID], [CS0~3] ON 신호를 보고 설비의 [L_REQ]/[U_REQ] ON
     * 2. OHT는 설비의 [L_REQ][U_REQ] ON 신호를 보고 [TR_REQ] ON (OHT [TR_REQ] 감시 timeout parameter 필요)
     * 3. OHT의 [TR_REQ] ON 신호를 보고 [READY] ON
     * 4. OHT는 설비의 [READY] ON 신호를 보고 [BUSY] ON (OHT [BUSY] 감시 timeout parameter 필요)
     * 5. 설비는 자재 감지/미감지되면 각각 [L_REQ]/[U_REQ] OFF (설비의 [READY] ON 이후 자재 감지까지의 timeout parameter 필요)
     * 6. OHT는 전송 완료 시 [BUSY] OFF
     * 7. OHT는 [BUSY] OFF 후 [COMPT] ON
     * 8. OHT는 [COMPT] ON 후 [TR_REQ] OFF
     * 9. 설비는 OHT [COMPT] ON, [TR_REQ] OFF 확인 후 [READY] OFF
     * 10. OHT는 설비의 [READY] OFF 확인 후 [COMPT], [CS0~3], [VALID] 전부 OFF (설비 timeout parameter 필요)
     *
     */
    public class E23Handler : AutomatedMaterialHandlingSystemController
    {
        #region <Constructors>
        public E23Handler(int lpIndex,
            int saftyInterLockIndex,
            Dictionary<int, Tuple<int, string>> inputs,
            Dictionary<int, Tuple<int, string>> outputs)
            : base(lpIndex,
                  new AMHSInformation(Define.DefineEnumProject.AppConfig.EN_PIO_INTERFACE_TYPE.E23,
                  saftyInterLockIndex, inputs, outputs))
        {
            var input = new Dictionary<E23InputSignals, int>();
            foreach (var item in inputs)
            {
                if (false == Enum.TryParse(item.Value.Item2, out E23InputSignals inputEnums))
                    continue;

                input[inputEnums] = item.Value.Item1;

                if (inputEnums == E23InputSignals.Complete)
                {
                    CompleteSignalIndex = input[inputEnums];
                }
            }

            var output = new Dictionary<E23OutputSignals, int>();
            foreach (var item in outputs)
            {
                if (false == Enum.TryParse(item.Value.Item2, out E23OutputSignals outputEnums))
                    continue;

                output[outputEnums] = item.Value.Item1;

                if (outputEnums == E23OutputSignals.Ready)
                {
                    ReadySignalIndex = output[outputEnums];
                }
            }

            InputSignals = new ReadOnlyDictionary<E23InputSignals, int>(input);
            OutputSignals = new ReadOnlyDictionary<E23OutputSignals, int>(output);
        }
        #endregion </Constructors>

        #region <Fields>
        private readonly ReadOnlyDictionary<E23InputSignals, int> InputSignals = null;
        private readonly ReadOnlyDictionary<E23OutputSignals, int> OutputSignals = null;
        #endregion </Fields>

        #region <Types>
        private enum Timers
        {
            Outputs,
            Long,
            T1,     // L,UL Req ~ TR_REQ ON 까지(3sec)
            T3,     // READY ON ~ BUSY ON 까지(3sec)
            T6      // READY OFF ~ COMP OFF 까지(3sec)
        }
        #endregion </Types>

        #region <Methods>

        #region <Overrides>
        public override void InitializeSignals()
        {
            _seqNum = 0;
            _commandResult = new CommandResults("", CommandResult.Proceed);
            foreach (var item in OutputSignals)
            {
                WriteOutput(item.Value, false);
            }
        }
        public override bool ReadAMHSPIOInput(int inputIndex, bool defaultSignal)
        {
            return false;
        }
        public override bool ReadAMHSPIOOutput(int outputIndex)
        {
            return false;
        }
        protected override CommandResults ExecuteHandlingToLoad(LoadPortCommands command)
        {
            return ExecuteHandling(command);
        }
        protected override CommandResults ExecuteHandlingToUnload(LoadPortCommands command)
        {
            return ExecuteHandling(command);
        }
        public override int GetEmergencyStopSignalIndex()
        {
            foreach (var item in Information.DigitalOutputs)
            {
                if (item.Value.Item2.Equals(E23OutputSignals.Abort.ToString()))
                {
                    return item.Value.Item1;
                }
            }

            return -1;
        }
        #endregion </Overrides>

        #region <Timers>
        private void SetTimer(Timers timer)
        {
            switch (timer)
            {
                case Timers.Outputs:
                    SetTickCount(1000);
                    break;
                case Timers.Long:
                    SetTickCount(10000);
                    break;
                case Timers.T1:
                case Timers.T3:
                case Timers.T6:
                    SetTickCount(3000);
                    break;
                default:
                    break;
            }
        }
        #endregion </Timers>

        #region <Wrapping Interfaces>
        private bool ReadPIOInput(E23InputSignals input, bool defaultSignal)
        {
            if (false == InputSignals.ContainsKey(input))
                return defaultSignal;

            return ReadInput(InputSignals[input], defaultSignal);
        }
        private bool WritePIOOutput(E23OutputSignals output, bool newSignal)
        {
            if (false == OutputSignals.ContainsKey(output))
                return false;

            return WriteOutput(OutputSignals[output], newSignal);
        }
        private CommandResults ExecuteHandling(LoadPortCommands command)
        {
            switch (_seqNum)
            {
                #region <Case 0~10:OHT의 [VALID], [CS0~3] ON 신호를 보고 설비의 [L_REQ]/[U_REQ] ON>
                case 0:
                    {
                        if (IsFinishingMode())
                        {
                            return ReturnResultGoodOrNg(command, CommandResult.Skipped, "Stopping Requested");
                        }

                        if (ReadPIOInput(E23InputSignals.Valid, true) && IsAnyCarrierStageSignalsOn())
                        {
                            SetTimer(Timers.Outputs);
                            ++_seqNum;
                        }
                    }
                    break;
                case 1:
                    {
                        if (IsTickOver())
                        {
                            return ReturnResultGoodOrNg(command, CommandResult.Timeout, "Request output signal on timeout");
                        }

                        E23OutputSignals output = command.Equals(LoadPortCommands.AMHSLoading) ?
                                E23OutputSignals.LoadRequest : E23OutputSignals.UnloadRequest;

                        if (false == WritePIOOutput(output, true))
                            break;

                        _seqNum = 10;
                    }
                    break;
                #endregion </Case 0~10:OHT의 [VALID], [CS0~3] ON 신호를 보고 설비의 [L_REQ]/[U_REQ] ON>

                #region <Case 10~11:OHT는 설비의[L_REQ][U_REQ] ON 신호를 보고[TR_REQ] ON(OHT[TR_REQ] 감시 timeout parameter 필요)>
                case 10:
                    {
                        SetTimer(Timers.T1);     // TR_REQ 감시
                        ++_seqNum;
                    }
                    break;
                case 11:
                    {
                        if (IsTickOver())
                        {
                            return ReturnResultGoodOrNg(command, CommandResult.Timeout, "Transfer Request signal timeout");
                        }

                        if (false == ReadPIOInput(E23InputSignals.TransferRequest, true))
                            break;

                        _seqNum = 20;
                    }
                    break;
                #endregion </Case 10~11:OHT는 설비의[L_REQ][U_REQ] ON 신호를 보고[TR_REQ] ON(OHT[TR_REQ] 감시 timeout parameter 필요)>

                #region <Case 20:Ready 전 액션 실행>
                case 20:
                    {
                        SetTimer(Timers.Long);
                        ++_seqNum;
                    }
                    break;

                case 21:
                    {
                        if (IsTickOver())
                        {
                            return ReturnResultGoodOrNg(command, CommandResult.Timeout, "Action timeout before ready signals on");
                        }

                        switch (command)
                        {
                            case LoadPortCommands.AMHSLoading:
                                {
                                    var result = ExecuteActionBeforeLoad(Index, command);
                                    switch (result.CommandResult)
                                    {
                                        case CommandResult.Proceed:
                                            break;
                                        case CommandResult.Completed:
                                        case CommandResult.Skipped:
                                            _seqNum = 30;
                                            break;

                                        default:
                                            return ReturnResultGoodOrNg(command, CommandResult.Error, "Action has error before ready signals on");
                                    }
                                }
                                break;
                            case LoadPortCommands.AMHSUnloading:
                                {
                                    var result = ExecuteActionBeforeUnload(Index, command);
                                    switch (result.CommandResult)
                                    {
                                        case CommandResult.Proceed:
                                            break;
                                        case CommandResult.Completed:
                                        case CommandResult.Skipped:
                                            _seqNum = 30;
                                            break;

                                        default:
                                            return ReturnResultGoodOrNg(command, CommandResult.Error, "Action has error before ready signals on");
                                    }
                                }
                                break;
                            default:
                                _seqNum = 30;
                                break;
                        }
                    }
                    break;
                #endregion </Case 20:Ready 전 액션 실행>

                #region <Case 30~31:OHT의[TR_REQ] ON 신호를 보고[READY] ON>
                case 30:
                    {
                        SetTimer(Timers.Outputs);
                        ++_seqNum;
                    }
                    break;
                case 31:
                    {
                        if (IsTickOver())
                        {
                            return ReturnResultGoodOrNg(command, CommandResult.Timeout, "Ready output signal timeout");
                        }

                        if (false == WritePIOOutput(E23OutputSignals.Ready, true))
                            break;

                        SetTickCountForPresence(30000);     // 자재 안착 감시
                        _seqNum = 40;
                    }
                    break;
                #endregion </Case 30~31:OHT의[TR_REQ] ON 신호를 보고[READY] ON>

                #region <Case 40~41:OHT는 설비의 [READY] ON 신호를 보고 [BUSY] ON (OHT [BUSY] 감시 timeout parameter 필요)>
                case 40:
                    {
                        SetTimer(Timers.T3);     // Busy 감시
                        ++_seqNum;
                    }
                    break;
                case 41:
                    {
                        if (IsTickOver())
                        {
                            return ReturnResultGoodOrNg(command, CommandResult.Timeout, "Busy signal timeout");
                        }

                        if (false == ReadPIOInput(E23InputSignals.Busy, true))
                            break;

                        _seqNum = 50;
                    }
                    break;
                #endregion </Case 40~41:OHT는 설비의 [READY] ON 신호를 보고 [BUSY] ON (OHT [BUSY] 감시 timeout parameter 필요)>

                #region <Case 50~52:설비는 자재 감지/미감지되면 각각 [L_REQ]/[U_REQ] OFF (설비의 [READY] ON 이후 자재 감지까지의 timeout parameter 필요)>
                case 50:
                    {
                        if (IsTickOverForPresence())
                        {
                            return ReturnResultGoodOrNg(command, CommandResult.Timeout, "Carrier presence timeout");
                        }

                        if (false == GetTriggerCarrierPresence(command))
                            break;

                        ++_seqNum;
                    }
                    break;
                case 51:
                    {
                        SetTimer(Timers.Outputs);
                        ++_seqNum;
                    }
                    break;
                case 52:
                    {
                        if (IsTickOver())
                        {
                            return ReturnResultGoodOrNg(command, CommandResult.Timeout, "Request output signal off timeout");
                        }

                        E23OutputSignals output = command.Equals(LoadPortCommands.AMHSLoading) ?
                                E23OutputSignals.LoadRequest : E23OutputSignals.UnloadRequest;

                        if (false == WritePIOOutput(output, false))
                            break;

                        _seqNum = 60;
                    }
                    break;
                #endregion </Case 50~52:설비는 자재 감지/미감지되면 각각 [L_REQ]/[U_REQ] OFF (설비의 [READY] ON 이후 자재 감지까지의 timeout parameter 필요)>

                #region <Case 60~61:OHT는 전송 완료 시 [BUSY] OFF, [COMPT] ON, [TR_REQ] OFF>
                case 60:
                    {
                        // 없는거지만 감시한다.
                        SetTickCount(10000);
                        ++_seqNum;
                    }
                    break;
                case 61:
                    {
                        if (IsTickOver())
                        {
                            return ReturnResultGoodOrNg(command, CommandResult.Timeout, "Handling Completion timeout");
                        }

                        if (false == IsHandlingCompleted())
                            break;

                        _seqNum = 70;
                    }
                    break;
                #endregion </Case 60~61:OHT는 전송 완료 시 [BUSY] OFF, [COMPT] ON, [TR_REQ] OFF>

                #region <Case 70:설비는 OHT [COMPT] ON, [TR_REQ] OFF 확인 후 [READY] OFF>
                case 70:
                    {
                        SetTimer(Timers.Outputs);
                        _seqNum = 80;
                    }
                    break;
                case 71:
                    {
                        if (IsTickOver())
                        {
                            return ReturnResultGoodOrNg(command, CommandResult.Timeout, "Ready output signal off timeout");
                        }

                        if (false == WritePIOOutput(E23OutputSignals.Ready, false))
                            break;

                        SetTimer(Timers.T6);     // COMP 감시
                        _seqNum = 80;
                    }
                    break;
                #endregion </Case 70:설비는 OHT [COMPT] ON, [TR_REQ] OFF 확인 후 [READY] OFF>

                #region <Case 80:OHT는 설비의 [READY] OFF 확인 후 [COMPT], [CS0~3], [VALID] 전부 OFF (설비 timeout parameter 필요)>
                case 80:
                    {
                        if (IsTickOver())
                        {
                            return ReturnResultGoodOrNg(command, CommandResult.Timeout, "Interface Completion timeout");
                        }

                        if (false == IsInterfaceCompleted())
                            break;

                        return ReturnResultGoodOrNg(command, CommandResult.Completed, string.Empty);
                    }
                #endregion </Case 80:OHT는 설비의 [READY] OFF 확인 후 [COMPT], [CS0~3], [VALID] 전부 OFF (설비 timeout parameter 필요)>

                default:
                    break;
            }

            _commandResult.ActionName = command.ToString();
            _commandResult.CommandResult = CommandResult.Proceed;
            return _commandResult;
        }
        #endregion </Wrapping Interfaces>

        #region <Signal Wrappers>
        private bool IsAnyCarrierStageSignalsOn()
        {
            return ReadPIOInput(E23InputSignals.CarrierStage_0, true)
                || ReadPIOInput(E23InputSignals.CarrierStage_1, true)
                || ReadPIOInput(E23InputSignals.CarrierStage_2, true)
                || ReadPIOInput(E23InputSignals.CarrierStage_3, true);
        }
        private bool IsHandlingCompleted()
        {
            // Busy Off -> Complete On -> TransferRequest Off면 완료
            return (false == ReadPIOInput(E23InputSignals.Busy, false) &&
                false == ReadPIOInput(E23InputSignals.TransferRequest, false) &&
                ReadPIOInput(E23InputSignals.Complete, true));
        }
        private bool IsInterfaceCompleted()
        {
            return (false == ReadPIOInput(E23InputSignals.Complete, false) &&
                false == ReadPIOInput(E23InputSignals.CarrierStage_0, false) &&
                false == ReadPIOInput(E23InputSignals.CarrierStage_1, false) &&
                false == ReadPIOInput(E23InputSignals.CarrierStage_2, false) &&
                false == ReadPIOInput(E23InputSignals.CarrierStage_3, false) &&
                false == ReadPIOInput(E23InputSignals.Valid, false));
        }
        #endregion </Signal Wrappers>

        #endregion </Methods>
    }
    public class CustomizedE23 : AutomatedMaterialHandlingSystemController
    {
        #region <Constructors>
        public CustomizedE23(int lpIndex,
            int saftyInterLockIndex,
            Dictionary<int, Tuple<int, string>> inputs,
            Dictionary<int, Tuple<int, string>> outputs)
            : base(lpIndex,
                  new AMHSInformation(Define.DefineEnumProject.AppConfig.EN_PIO_INTERFACE_TYPE.E23,
                  saftyInterLockIndex, inputs, outputs))
        {
            var input = new Dictionary<E23InputSignals, int>();
            foreach (var item in inputs)
            {
                if (false == Enum.TryParse(item.Value.Item2, out E23InputSignals inputEnums))
                    continue;

                input[inputEnums] = item.Value.Item1;

                if (inputEnums == E23InputSignals.Complete)
                {
                    CompleteSignalIndex = input[inputEnums];
                }
            }

            var output = new Dictionary<E23OutputSignals, int>();
            foreach (var item in outputs)
            {
                if (false == Enum.TryParse(item.Value.Item2, out E23OutputSignals outputEnums))
                    continue;

                output[outputEnums] = item.Value.Item1;

                if (outputEnums == E23OutputSignals.Ready)
                {
                    ReadySignalIndex = output[outputEnums];
                }
            }
            
            InputSignals = new ReadOnlyDictionary<E23InputSignals, int>(input);
            OutputSignals = new ReadOnlyDictionary<E23OutputSignals, int>(output);
        }
        #endregion </Constructors>

        #region <Fields>
        private readonly ReadOnlyDictionary<E23InputSignals, int> InputSignals = null;
        private readonly ReadOnlyDictionary<E23OutputSignals, int> OutputSignals = null;
        #endregion </Fields>

        #region <Types>
        private enum Timers
        {
            Outputs,
            Long,
            T1,     // L,UL Req ~ TR_REQ ON 까지(3sec)
            T3,     // READY ON ~ BUSY ON 까지(3sec)
            T6      // READY OFF ~ COMP OFF 까지(3sec)
        }
        #endregion </Types>

        #region <Methods>
        
        #region <Overrides>
        public override void InitializeSignals()
        {
            _seqNum = 0;
            _commandResult = new CommandResults("", CommandResult.Proceed);
            foreach (var item in OutputSignals)
            {
                WriteOutput(item.Value, false);
            }
        }
        public override bool ReadAMHSPIOInput(int inputIndex, bool defaultSignal)
        {
            return false;
        }
        public override bool ReadAMHSPIOOutput(int outputIndex)
        {
            return false;
        }
        protected override CommandResults ExecuteHandlingToLoad(LoadPortCommands command)
        {
            return ExecuteHandling(command);
        }
        protected override CommandResults ExecuteHandlingToUnload(LoadPortCommands command)
        {
            return ExecuteHandling(command);
        }
        public override int GetEmergencyStopSignalIndex()
        {
            foreach (var item in Information.DigitalOutputs)
            {
                if (item.Value.Item2.Equals(E23OutputSignals.Abort.ToString()))
                {
                    return item.Value.Item1;
                }
            }

            return -1;
        }
        public override LoadPortLoadingMode CheckTriggerLoadingMode()
        {
            bool cs0 = ReadPIOInput(E23InputSignals.CarrierStage_0, false);
            bool cs1 = ReadPIOInput(E23InputSignals.CarrierStage_1, true);
            if (cs0 && false == cs1)
                return LoadPortLoadingMode.Foup;
            else if (cs1 && false == cs0)
                return LoadPortLoadingMode.Cassette;
            else 
                return LoadPortLoadingMode.Unknown;

        }
        #endregion </Overrides>

        #region <Timers>
        private void SetTimer(Timers timer)
        {
            switch (timer)
            {
                case Timers.Outputs:
                    SetTickCount(1000);
                    break;
                case Timers.Long:
                    SetTickCount(10000);
                    break;
                case Timers.T1:
                case Timers.T3:
                case Timers.T6:
                    SetTickCount(10000);
                    break;
                default:
                    break;
            }
        }
        #endregion </Timers>

        #region <Wrapping Interfaces>
        private bool ReadPIOInput(E23InputSignals input, bool defaultSignal)
        {
            if (false == InputSignals.ContainsKey(input))
                return defaultSignal;

            return ReadInput(InputSignals[input], defaultSignal);
        }
        private bool WritePIOOutput(E23OutputSignals output, bool newSignal)
        {
            if (false == OutputSignals.ContainsKey(output))
                return false;

            return WriteOutput(OutputSignals[output], newSignal);
        }
        private CommandResults ExecuteHandling(LoadPortCommands command)
        {            
            switch (_seqNum)
            {
                #region <Case 0~10:OHT의 [VALID], [CS0~3] ON 신호를 보고 설비의 [L_REQ]/[U_REQ] ON>
                case 0:
                    {
                        if (IsFinishingMode())
                        {
                            return ReturnResultGoodOrNg(command, CommandResult.Skipped, "Stopping Requested");
                        }

                        if (ReadPIOInput(E23InputSignals.Valid, true) && IsAnyCarrierStageSignalsOn())
                        {
                            SetTimer(Timers.Outputs);
                            ++_seqNum;
                        }
                    }
                    break;
                case 1:
                    {
                        if (IsTickOver())
                        {
                            return ReturnResultGoodOrNg(command, CommandResult.Timeout, "Request output signal on timeout");
                        }

                        E23OutputSignals output = command.Equals(LoadPortCommands.AMHSLoading) ?
                                E23OutputSignals.LoadRequest : E23OutputSignals.UnloadRequest;

                        if (false == WritePIOOutput(output, true))
                            break;

                        _seqNum = 10;
                    }
                    break;
                #endregion </Case 0~10:OHT의 [VALID], [CS0~3] ON 신호를 보고 설비의 [L_REQ]/[U_REQ] ON>

                #region <Case 10~11:OHT는 설비의[L_REQ][U_REQ] ON 신호를 보고[TR_REQ] ON(OHT[TR_REQ] 감시 timeout parameter 필요)>
                case 10:
                    {
                        SetTimer(Timers.T1);     // TR_REQ 감시
                        ++_seqNum;
                    }
                    break;
                case 11:
                    {
                        if (IsTickOver())
                        {
                            return ReturnResultGoodOrNg(command, CommandResult.Timeout, "Transfer Request signal timeout");
                        }

                        if (false == ReadPIOInput(E23InputSignals.TransferRequest, true))
                            break;

                        _seqNum = 20;
                    }
                    break;
                #endregion </Case 10~11:OHT는 설비의[L_REQ][U_REQ] ON 신호를 보고[TR_REQ] ON(OHT[TR_REQ] 감시 timeout parameter 필요)>

                #region <Case 20:Ready 전 액션 실행>
                case 20:
                    {
                        SetTimer(Timers.Long);
                        ++_seqNum;
                    }
                    break;
                
                case 21:
                    { 
                        if (IsTickOver())
                        {
                            return ReturnResultGoodOrNg(command, CommandResult.Timeout, "Action timeout before ready signals on");
                        }

                        switch (command)
                        {
                            case LoadPortCommands.AMHSLoading:
                                {
                                    
                                    var result = ExecuteActionBeforeLoad(Index, command);
                                    switch (result.CommandResult)
                                    {
                                        case CommandResult.Proceed:
                                            break;
                                        case CommandResult.Completed:
                                        case CommandResult.Skipped:
                                            _seqNum = 25;
                                            break;
                                        case CommandResult.Timeout:
                                        case CommandResult.Error:
                                        case CommandResult.Invalid:
                                            return ReturnResultGoodOrNg(command, CommandResult.Error, "Action has error before ready signals on");

                                        default:
                                            return ReturnResultGoodOrNg(command, CommandResult.Error, "Action has error before ready signals on");
                                    }
                                }
                                break;
                            case LoadPortCommands.AMHSUnloading:
                                {
                                    var result = ExecuteActionBeforeUnload(Index, command);
                                    switch (result.CommandResult)
                                    {
                                        case CommandResult.Proceed:
                                            break;
                                        case CommandResult.Completed:
                                        case CommandResult.Skipped:
                                            _seqNum = 30;
                                            break;
                                        case CommandResult.Timeout:
                                        case CommandResult.Error:
                                        case CommandResult.Invalid:
                                            return ReturnResultGoodOrNg(command, CommandResult.Error, "Action has error before ready signals on");

                                        default:
                                            return ReturnResultGoodOrNg(command, CommandResult.Error, "Action has error before ready signals on");
                                    }
                                }
                                break;
                            default:
                                _seqNum = 25;
                                break;
                        }
                    }
                    break;

                case 25:
                    {
                        SetTimer(Timers.Long);
                        ++_seqNum;
                    }
                    break;
                case 26:
                    {
                        if (IsTickOver())
                        {
                            return ReturnResultGoodOrNg(command, CommandResult.Timeout, "Changing Mode Action timeout");
                        }

                        var mode = CheckTriggerLoadingMode();
                        var result = ExecuteModeChangeAction(Index, mode, command);
                        switch (result.CommandResult)
                        {
                            case CommandResult.Proceed:
                                break;
                            case CommandResult.Completed:
                            case CommandResult.Skipped:
                                _seqNum = 30;
                                break;
                            case CommandResult.Timeout:
                            case CommandResult.Error:
                            case CommandResult.Invalid:
                                return ReturnResultGoodOrNg(command, CommandResult.Error, "Changing Mode Action error");

                            default:
                                _seqNum = 30;
                                break;
                        }
                    }
                    break;
                #endregion </Case 20:Ready 전 액션 실행>

                #region <Case 30~31:OHT의[TR_REQ] ON 신호를 보고[READY] ON>
                case 30:
                    {
                        SetTimer(Timers.Outputs);
                        ++_seqNum;
                    }
                    break;
                case 31:
                    {
                        if (IsTickOver())
                        {
                            return ReturnResultGoodOrNg(command, CommandResult.Timeout, "Ready output signal timeout");
                        }

                        if (false == WritePIOOutput(E23OutputSignals.Ready, true))
                            break;

                        SetTickCountForPresence(30000);     // 자재 안착 감시
                        _seqNum = 40;
                    }
                    break;
                #endregion </Case 30~31:OHT의[TR_REQ] ON 신호를 보고[READY] ON>

                #region <Case 40~41:OHT는 설비의 [READY] ON 신호를 보고 [BUSY] ON (OHT [BUSY] 감시 timeout parameter 필요)>
                case 40:
                    {
                        SetTimer(Timers.T3);     // Busy 감시
                        ++_seqNum;
                    }
                    break;
                case 41:
                    {
                        if (IsTickOver())
                        {
                            return ReturnResultGoodOrNg(command, CommandResult.Timeout, "Busy signal timeout");
                        }

                        if (false == ReadPIOInput(E23InputSignals.Busy, true))
                            break;

                        _seqNum = 50;
                    }
                    break;
                #endregion </Case 40~41:OHT는 설비의 [READY] ON 신호를 보고 [BUSY] ON (OHT [BUSY] 감시 timeout parameter 필요)>

                #region <Case 50~52:설비는 자재 감지/미감지되면 각각 [L_REQ]/[U_REQ] OFF (설비의 [READY] ON 이후 자재 감지까지의 timeout parameter 필요)>
                case 50:
                    {
                        if (IsTickOverForPresence())
                        {
                            return ReturnResultGoodOrNg(command, CommandResult.Timeout, "Carrier presence timeout");
                        }

                        if (false == GetTriggerCarrierPresence(command))
                            break;

                        ++_seqNum;
                    }
                    break;
                case 51:
                    {
                        SetTimer(Timers.Outputs);
                        ++_seqNum;
                    }
                    break;
                case 52:
                    {
                        if (IsTickOver())
                        {
                            return ReturnResultGoodOrNg(command, CommandResult.Timeout, "Request output signal off timeout");
                        }

                        E23OutputSignals output = command.Equals(LoadPortCommands.AMHSLoading) ?
                                E23OutputSignals.LoadRequest : E23OutputSignals.UnloadRequest;

                        if (false == WritePIOOutput(output, false))
                            break;

                        _seqNum = 60;
                    }
                    break;
                #endregion </Case 50~52:설비는 자재 감지/미감지되면 각각 [L_REQ]/[U_REQ] OFF (설비의 [READY] ON 이후 자재 감지까지의 timeout parameter 필요)>

                #region <Case 60~61:OHT는 전송 완료 시 [BUSY] OFF, [COMPT] ON, [TR_REQ] OFF>
                case 60:
                    {
                        // 없는거지만 감시한다.
                        SetTickCount(10000);
                        ++_seqNum;
                    }
                    break;
                case 61:
                    {
                        if (IsTickOver())
                        {
                            return ReturnResultGoodOrNg(command, CommandResult.Timeout, "Handling Completion timeout");
                        }

                        if (false == IsHandlingCompleted())
                            break;

                        _seqNum = 70;
                    }
                    break;
                #endregion </Case 60~61:OHT는 전송 완료 시 [BUSY] OFF, [COMPT] ON, [TR_REQ] OFF>

                #region <Case 70:설비는 OHT [COMPT] ON, [TR_REQ] OFF 확인 후 [READY] OFF>
                case 70:
                    {
                        SetTimer(Timers.Outputs);
                        ++_seqNum;
                    }
                    break;
                case 71:
                    {
                        if (IsTickOver())
                        {
                            return ReturnResultGoodOrNg(command, CommandResult.Timeout, "Ready output signal off timeout");
                        }

                        if (false == WritePIOOutput(E23OutputSignals.Ready, false))
                            break;

                        SetTimer(Timers.T6);     // COMP 감시
                        _seqNum = 80;
                    }
                    break;
                #endregion </Case 70:설비는 OHT [COMPT] ON, [TR_REQ] OFF 확인 후 [READY] OFF>

                #region <Case 80:OHT는 설비의 [READY] OFF 확인 후 [COMPT], [CS0~3], [VALID] 전부 OFF (설비 timeout parameter 필요)>
                case 80:
                    {
                        if (IsTickOver())
                        {
                            return ReturnResultGoodOrNg(command, CommandResult.Timeout, "Interface Completion timeout");
                        }

                        if (false == IsInterfaceCompleted())
                            break;

                        return ReturnResultGoodOrNg(command, CommandResult.Completed, string.Empty);
                    }
                #endregion </Case 80:OHT는 설비의 [READY] OFF 확인 후 [COMPT], [CS0~3], [VALID] 전부 OFF (설비 timeout parameter 필요)>

                default:
                    break;
            }

            _commandResult.ActionName = command.ToString();
            _commandResult.CommandResult = CommandResult.Proceed;
            return _commandResult;
        }
        #endregion </Wrapping Interfaces>

        #region <Signal Wrappers>
        private bool IsAnyCarrierStageSignalsOn()
        {
            return ReadPIOInput(E23InputSignals.CarrierStage_0, true)
                || ReadPIOInput(E23InputSignals.CarrierStage_1, true)
                || ReadPIOInput(E23InputSignals.CarrierStage_2, true)
                || ReadPIOInput(E23InputSignals.CarrierStage_3, true);
        }
        private bool IsHandlingCompleted()
        {
            // Busy Off -> Complete On -> TransferRequest Off면 완료
            return (false == ReadPIOInput(E23InputSignals.Busy, false) &&
                false == ReadPIOInput(E23InputSignals.TransferRequest, false) &&
                ReadPIOInput(E23InputSignals.Complete, true));
        }
        private bool IsInterfaceCompleted()
        {
            return (false == ReadPIOInput(E23InputSignals.Complete, false) &&
                false == ReadPIOInput(E23InputSignals.CarrierStage_0, false) &&
                false == ReadPIOInput(E23InputSignals.CarrierStage_1, false) &&
                false == ReadPIOInput(E23InputSignals.CarrierStage_2, false) &&
                false == ReadPIOInput(E23InputSignals.CarrierStage_3, false) &&
                false == ReadPIOInput(E23InputSignals.Valid, false));
        }
        #endregion </Signal Wrappers>

        #endregion </Methods>
    }

    /*
     * - HO_AVBL은 NORMAL ON 이며, Handoff 불가능할 시 OFF
     * - ES는 NORMAL ON 이며, 에러 발생 시 OFF
     * - ERROR 초기화 시 신호 RESET
     * - VALID 신호 OFF 시 신호 RESET
     * 
     * 1. OHT의 [VALID], [CS0~1] ON 신호를 보고 설비의 [L_REQ]/[U_REQ] ON
     * 2. OHT는 설비의 [L_REQ][U_REQ] ON 신호를 보고 [TR_REQ] ON (OHT [TR_REQ] 감시 timeout parameter 필요)
     * 3. OHT의 [TR_REQ] ON 신호를 보고 [READY] ON
     * 4. OHT는 설비의 [READY] ON 신호를 보고 [BUSY] ON (OHT [BUSY] 감시 timeout parameter 필요)
     * 5. 설비는 자재 감지/미감지되면 각각 [L_REQ]/[U_REQ] OFF (설비의 [READY] ON 이후 자재 감지까지의 timeout parameter 필요)
     * 6. OHT는 설비의 [L_REQ] OFF 신호를 보고 [BUSY] OFF
     * 7. OHT는 [BUSY] OFF 후 [TR_REQ] Off, [COMPT] ON
     * 8. 설비는 OHT의 [COMPT] ON 신호를 보고 [READY] OFF
     * 9. OHT는 설비의 [READY] OFF 확인 후 [VALID], [COMPT], [CS0~1] 전부 OFF (설비 timeout parameter 필요)
     *
     */
    public class E84Handler : AutomatedMaterialHandlingSystemController
    {
        #region <Constructors>
        public E84Handler(int lpIndex,
            int saftyInterLockIndex,
            Dictionary<int, Tuple<int, string>> inputs,
            Dictionary<int, Tuple<int, string>> outputs)
            : base(lpIndex,
                  new AMHSInformation(Define.DefineEnumProject.AppConfig.EN_PIO_INTERFACE_TYPE.E84,
                  saftyInterLockIndex, inputs, outputs))
        {
            var input = new Dictionary<E84InputSignals, int>();
            foreach (var item in inputs)
            {
                if (false == Enum.TryParse(item.Value.Item2, out E84InputSignals inputEnums))
                    continue;

                input[inputEnums] = item.Value.Item1;

                if (inputEnums == E84InputSignals.Complete)
                {
                    CompleteSignalIndex = input[inputEnums];
                }
            }

            var output = new Dictionary<E84OutputSignals, int>();
            foreach (var item in outputs)
            {
                if (false == Enum.TryParse(item.Value.Item2, out E84OutputSignals outputEnums))
                    continue;

                output[outputEnums] = item.Value.Item1;

                if (outputEnums == E84OutputSignals.Ready)
                {
                    ReadySignalIndex = output[outputEnums];
                }
            }

            InputSignals = new ReadOnlyDictionary<E84InputSignals, int>(input);
            OutputSignals = new ReadOnlyDictionary<E84OutputSignals, int>(output);
        }
        #endregion </Constructors>

        #region <Fields>
        private readonly ReadOnlyDictionary<E84InputSignals, int> InputSignals = null;
        private readonly ReadOnlyDictionary<E84OutputSignals, int> OutputSignals = null;
        #endregion </Fields>

        #region <Types>
        private enum Timers
        {
            Outputs,
            Long,
            T1,     // L,UL Req ~ TR_REQ ON 까지(2sec)
            T2,     // READY ON ~ BUSY ON 까지(2sec)
            T5,      // READY OFF ~ COMP OFF 까지(2sec)

            // 2025.09.12 dwlim [ADD] E84에 명시된 Timeout 추가
            TP1,    // L/U REQ ON ~ TR_REQ ON 까지(2sec)
            TP2,    // READY ON ~ BUSY ON 까지(2sec)
            TP3,    // BUSY ON ~ CARRIER ON/OFF 까지(60sec)
            TP4,    // L/U REQ OFF ~ BUSY OFF 까지(60sec)
            TP5,    // READY OFF ~ VALID OFF 까지(2sec)
            TP6,    // VALID OFF ~ VALID ON 까지(2sec)

            TD0,    // CS ON ~ VALID ON 까지(0.1sec)
            TD1,    // VALID OFF ~ VALID ON 까지(1sec)
            // 2025.09.12 dwlim [END]
        }
        #endregion </Types>

        #region <Methods>

        #region <Overrides>
        public override void InitializeSignals()
        {
            _seqNum = 0;
            _commandResult = new CommandResults("", CommandResult.Proceed);
            foreach (var item in OutputSignals)
            {
                WriteOutput(item.Value, false);
            }
        }
        public override bool ReadAMHSPIOInput(int inputIndex, bool defaultSignal)
        {
            return false;
        }
        public override bool ReadAMHSPIOOutput(int outputIndex)
        {
            return false;
        }
        protected override CommandResults ExecuteHandlingToLoad(LoadPortCommands command)
        {
            return ExecuteHandling(command);
        }
        protected override CommandResults ExecuteHandlingToUnload(LoadPortCommands command)
        {
            return ExecuteHandling(command);
        }
        public override int GetEmergencyStopSignalIndex()
        {
            foreach (var item in Information.DigitalOutputs)
            {
                if (item.Value.Item2.Equals(E84OutputSignals.EmergencyStop.ToString()))
                {
                    return item.Value.Item1;
                }
            }

            return -1;
        }
        #endregion </Overrides>

        #region <Timers>
        private void SetTimer(Timers timer)
        {
            switch (timer)
            {
                case Timers.Outputs:
                    SetTickCount(2000);
                    break;
                case Timers.Long:
                    SetTickCount(10000);
                    break;
                case Timers.T1:
                case Timers.T2:
                case Timers.T5:
                    SetTickCount(2000);
                    break;

                // 2025.09.12 dwlim [ADD] E84에 명시된 Timeout 추가
                case Timers.TP1:
                case Timers.TP2:
                case Timers.TP5:
                case Timers.TP6:
                    SetTickCount(2000);
                    break;

                case Timers.TP3:
                case Timers.TP4:
                    SetTickCount(60000);
                    break;

                case Timers.TD0:
                    SetTickCount(100);
                    break;

                case Timers.TD1:
                    SetTickCount(1000);
                    break;
                // 2025.09.12 dwlim [END]

                default:
                    break;
            }
        }
        #endregion </Timers>

        #region <Wrapping Interfaces>
        private bool ReadPIOInput(E84InputSignals input, bool defaultSignal)
        {
            if (false == InputSignals.ContainsKey(input))
                return defaultSignal;

            return ReadInput(InputSignals[input], defaultSignal);
        }
        private bool WritePIOOutput(E84OutputSignals output, bool newSignal)
        {
            if (false == OutputSignals.ContainsKey(output))
                return false;

            return WriteOutput(OutputSignals[output], newSignal);
        }
        private CommandResults ExecuteHandling(LoadPortCommands command)
        {           
            switch (_seqNum)
            {
                #region <Case 0~10:OHT의 [VALID], [CS0~1] ON 신호를 보고 설비의 [L_REQ]/[U_REQ] ON>
                case 0:
                    {
                        if (IsFinishingMode())
                        {
                            return ReturnResultGoodOrNg(command, CommandResult.Skipped, "Stopping Requested");
                        }

                        if (ReadPIOInput(E84InputSignals.Valid, true) && IsAnyCarrierStageSignalsOn())
                        {
                            SetTimer(Timers.Outputs);
                            ++_seqNum;
                        }
                    }
                    break;
                case 1:
                    {
                        if (IsTickOver())
                        {
                            return ReturnResultGoodOrNg(command, CommandResult.Timeout, "Request output signal on timeout");
                        }

                        E84OutputSignals output = command.Equals(LoadPortCommands.AMHSLoading) ?
                                E84OutputSignals.LoadRequest : E84OutputSignals.UnloadRequest;

                        if (false == WritePIOOutput(output, true))
                            break;

                        _seqNum = 10;
                    }
                    break;
                #endregion </Case 0~10:OHT의 [VALID], [CS0~1] ON 신호를 보고 설비의 [L_REQ]/[U_REQ] ON>

                #region <Case 10~11:OHT는 설비의[L_REQ][U_REQ] ON 신호를 보고[TR_REQ] ON(OHT[TR_REQ] 감시 timeout parameter 필요)>
                case 10:
                    {
                        SetTimer(Timers.T1);     // TR_REQ 감시
                        ++_seqNum;
                    }
                    break;
                case 11:
                    {
                        if (IsTickOver())
                        {
                            return ReturnResultGoodOrNg(command, CommandResult.Timeout, "Transfer Request signal timeout");
                        }

                        if (false == ReadPIOInput(E84InputSignals.TransferRequest, true))
                            break;

                        _seqNum = 20;
                    }
                    break;
                #endregion </Case 10~11:OHT는 설비의[L_REQ][U_REQ] ON 신호를 보고[TR_REQ] ON(OHT[TR_REQ] 감시 timeout parameter 필요)>

                #region <Case 20:Ready 전 액션 실행>
                case 20:
                    {
                        SetTimer(Timers.Long);
                        ++_seqNum;
                    }
                    break;

                case 21:
                    {
                        if (IsTickOver())
                        {
                            return ReturnResultGoodOrNg(command, CommandResult.Timeout, "Action timeout before ready signals on");
                        }

                        switch (command)
                        {
                            case LoadPortCommands.AMHSLoading:
                                {
                                    var result = ExecuteActionBeforeLoad(Index, command);
                                    switch (result.CommandResult)
                                    {
                                        case CommandResult.Proceed:
                                            break;
                                        case CommandResult.Completed:
                                        case CommandResult.Skipped:
                                            _seqNum = 30;
                                            break;

                                        default:
                                            return ReturnResultGoodOrNg(command, CommandResult.Error, "Action has error before ready signals on");
                                    }
                                }
                                break;
                            case LoadPortCommands.AMHSUnloading:
                                {
                                    var result = ExecuteActionBeforeUnload(Index, command);
                                    switch (result.CommandResult)
                                    {
                                        case CommandResult.Proceed:
                                            break;
                                        case CommandResult.Completed:
                                        case CommandResult.Skipped:
                                            _seqNum = 30;
                                            break;

                                        default:
                                            return ReturnResultGoodOrNg(command, CommandResult.Error, "Action has error before ready signals on");
                                    }
                                }
                                break;
                            default:
                                _seqNum = 30;
                                break;
                        }
                    }
                    break;
                #endregion </Case 20:Ready 전 액션 실행>

                #region <Case 30~31:OHT의[TR_REQ] ON 신호를 보고[READY] ON>
                case 30:
                    {
                        SetTimer(Timers.Outputs);
                        ++_seqNum;
                    }
                    break;
                case 31:
                    {
                        if (IsTickOver())
                        {
                            return ReturnResultGoodOrNg(command, CommandResult.Timeout, "Ready output signal timeout");
                        }

                        if (false == WritePIOOutput(E84OutputSignals.Ready, true))
                            break;

                        SetTickCountForPresence(30000);     // 자재 안착 감시
                        _seqNum = 40;
                    }
                    break;
                #endregion </Case 30~31:OHT의[TR_REQ] ON 신호를 보고[READY] ON>

                #region <Case 40~41:OHT는 설비의 [READY] ON 신호를 보고 [BUSY] ON (OHT [BUSY] 감시 timeout parameter 필요)>
                case 40:
                    {
                        SetTimer(Timers.T2);     // Busy 감시
                        ++_seqNum;
                    }
                    break;
                case 41:
                    {
                        if (IsTickOver())
                        {
                            return ReturnResultGoodOrNg(command, CommandResult.Timeout, "Busy signal timeout");
                        }

                        if (false == ReadPIOInput(E84InputSignals.Busy, true))
                            break;

                        _seqNum = 50;
                    }
                    break;
                #endregion </Case 40~41:OHT는 설비의 [READY] ON 신호를 보고 [BUSY] ON (OHT [BUSY] 감시 timeout parameter 필요)>

                #region <Case 50~52:설비는 자재 감지/미감지되면 각각 [L_REQ]/[U_REQ] OFF (설비의 [READY] ON 이후 자재 감지까지의 timeout parameter 필요)>
                case 50:
                    {
                        if (IsTickOverForPresence())
                        {
                            return ReturnResultGoodOrNg(command, CommandResult.Timeout, "Carrier presence timeout");
                        }

                        if (false == GetTriggerCarrierPresence(command))
                            break;

                        ++_seqNum;
                    }
                    break;
                case 51:
                    {
                        SetTimer(Timers.Outputs);
                        ++_seqNum;
                    }
                    break;
                case 52:
                    {
                        if (IsTickOver())
                        {
                            return ReturnResultGoodOrNg(command, CommandResult.Timeout, "Request output signal off timeout");
                        }

                        E84OutputSignals output = command.Equals(LoadPortCommands.AMHSLoading) ?
                                E84OutputSignals.LoadRequest : E84OutputSignals.UnloadRequest;

                        if (false == WritePIOOutput(output, false))
                            break;

                        _seqNum = 60;
                    }
                    break;
                #endregion </Case 50~52:설비는 자재 감지/미감지되면 각각 [L_REQ]/[U_REQ] OFF (설비의 [READY] ON 이후 자재 감지까지의 timeout parameter 필요)>

                #region <Case 60~61:OHT는 전송 완료 시 [BUSY] OFF, [COMPT] ON, [TR_REQ] OFF>
                case 60:
                    {
                        // 없는거지만 감시한다.
                        SetTickCount(10000);
                        ++_seqNum;
                    }
                    break;
                case 61:
                    {
                        if (IsTickOver())
                        {
                            return ReturnResultGoodOrNg(command, CommandResult.Timeout, "Handling Completion timeout");
                        }

                        if (false == IsHandlingCompleted())
                            break;

                        _seqNum = 70;
                    }
                    break;
                #endregion </Case 60~61:OHT는 전송 완료 시 [BUSY] OFF, [COMPT] ON, [TR_REQ] OFF>

                #region <Case 70:설비는 OHT [COMPT] ON 확인 후 [READY] OFF>
                case 70:
                    {
                        SetTimer(Timers.Outputs);
                        _seqNum = 80;
                    }
                    break;
                case 71:
                    {
                        if (IsTickOver())
                        {
                            return ReturnResultGoodOrNg(command, CommandResult.Timeout, "Ready output signal off timeout");
                        }

                        if (false == WritePIOOutput(E84OutputSignals.Ready, false))
                            break;

                        SetTimer(Timers.T5);     // COMP 감시
                        _seqNum = 80;
                    }
                    break;
                #endregion </Case 70:설비는 OHT [COMPT] ON 확인 후 [READY] OFF>

                #region <Case 80:OHT는 설비의 [READY] OFF 확인 후 [COMPT], [CS0~1], [VALID] 전부 OFF (설비 timeout parameter 필요)>
                case 80:
                    {
                        if (IsTickOver())
                        {
                            return ReturnResultGoodOrNg(command, CommandResult.Timeout, "Interface Completion timeout");
                        }

                        if (false == IsInterfaceCompleted())
                            break;

                        return ReturnResultGoodOrNg(command, CommandResult.Completed, string.Empty);
                    }
                #endregion </Case 80:OHT는 설비의 [READY] OFF 확인 후 [COMPT], [CS0~1], [VALID] 전부 OFF (설비 timeout parameter 필요)>
                default:
                    break;
            }
            _commandResult.ActionName = command.ToString();
            _commandResult.CommandResult = CommandResult.Proceed;
            return _commandResult;
        }
        #endregion </Wrapping Interfaces>

        #region <Signal Wrappers>
        private bool IsAnyCarrierStageSignalsOn()
        {
            return ReadPIOInput(E84InputSignals.CarrierStage_0, true)
                || ReadPIOInput(E84InputSignals.CarrierStage_1, true);
        }
        private bool IsHandlingCompleted()
        {
            // Busy Off -> Complete On -> TransferRequest Off면 완료
            return (false == ReadPIOInput(E84InputSignals.Busy, false) &&
                false == ReadPIOInput(E84InputSignals.TransferRequest, false) &&
                ReadPIOInput(E84InputSignals.Complete, true));
        }
        private bool IsInterfaceCompleted()
        {
            return (false == ReadPIOInput(E84InputSignals.Complete, false) &&
                false == ReadPIOInput(E84InputSignals.CarrierStage_0, false) &&
                false == ReadPIOInput(E84InputSignals.CarrierStage_1, false) &&
                false == ReadPIOInput(E84InputSignals.Valid, false));
        }
        #endregion </Signal Wrappers>

        #endregion </Methods>
    }
    public class CustomizedE84 : AutomatedMaterialHandlingSystemController
    {
        #region <Constructors>
        public CustomizedE84(int lpIndex,
            int saftyInterLockIndex,
            Dictionary<int, Tuple<int, string>> inputs,
            Dictionary<int, Tuple<int, string>> outputs)
            : base(lpIndex,
                  new AMHSInformation(Define.DefineEnumProject.AppConfig.EN_PIO_INTERFACE_TYPE.E84,
                  saftyInterLockIndex, inputs, outputs))
        {
            var input = new Dictionary<E84InputSignals, int>();
            foreach (var item in inputs)
            {
                if (false == Enum.TryParse(item.Value.Item2, out E84InputSignals inputEnums))
                    continue;

                input[inputEnums] = item.Value.Item1;

                if (inputEnums == E84InputSignals.Complete)
                {
                    CompleteSignalIndex = input[inputEnums];
                }
            }

            var output = new Dictionary<E84OutputSignals, int>();
            foreach (var item in outputs)
            {
                if (false == Enum.TryParse(item.Value.Item2, out E84OutputSignals outputEnums))
                    continue;

                output[outputEnums] = item.Value.Item1;

                if (outputEnums == E84OutputSignals.Ready)
                {
                    ReadySignalIndex = output[outputEnums];
                }
            }

            InputSignals = new ReadOnlyDictionary<E84InputSignals, int>(input);
            OutputSignals = new ReadOnlyDictionary<E84OutputSignals, int>(output);
        }
        #endregion </Constructors>

        #region <Fields>
        private readonly ReadOnlyDictionary<E84InputSignals, int> InputSignals = null;
        private readonly ReadOnlyDictionary<E84OutputSignals, int> OutputSignals = null;

        private readonly Dictionary<E84InputSignals, ChatteringTime> _monitorChattering = new Dictionary<E84InputSignals, ChatteringTime>();

        private bool _isPIOInterfaceAlarm = false;
        private bool _enableTP = false;
        private bool _enableTD = false;

        private Stopwatch _stopWatch = new Stopwatch();
        #endregion </Fields>

        #region <Types>
        private enum Timers
        {
            Outputs,
            Long,
            T1,     // L,UL Req ~ TR_REQ ON 까지(3sec)
            T2,     // READY ON ~ BUSY ON 까지(3sec)
            T5,     // READY OFF ~ COMP OFF 까지(3sec)

            // 2025.09.12 dwlim [ADD] E84에 명시된 Timeout 추가
            TP1,    // L/U REQ ON ~ TR_REQ ON 까지(2sec)
            TP2,    // READY ON ~ BUSY ON 까지(2sec)
            TP3,    // BUSY ON ~ CARRIER ON/OFF 까지(60sec)
            TP4,    // L/U REQ OFF ~ BUSY OFF 까지(60sec)
            TP5,    // READY OFF ~ VALID OFF 까지(2sec)
            /*
            CARRIER SENSOR ON은 Placement Sensor On을 의미하는 것으로 보인다.
            CARRIER SENSOR OFF는 Placement Sensor Off를 의미하는 것으로 보인다.
            CARRIER DETECT는 Placement Sensor와 Presence Sensor 모두 On을 의미하는 것으로 보인다.
            CARRIER REMOVE는 Placement Sensor와 Presence Sensor 모두 Off를 의미하는 것으로 보인다.
            */
            TD3,    // CARRIER SENSOR ON ~ CARRIER DETECT, CARRIER SENSOR OFF ~ CARRIER REMOVE (10sec)

            TC1,    // VALID ON ~ BUSY ON, BUSY OFF ~ VALID OFF(200msec)
            TC2,    // BUSY ON ~ BUSY OFF (200msec)
            // 2025.09.12 dwlim [END]

            PlacementSensorStabilizeDelay, // 2025.12.29 dwlim [ADD]
        }
        private enum EN_TP_SECTION
        {
            None,
            START,
            TP1,
            TP2,
            TP3,
            TP4,
            TP5,
            END,
        }
        private enum EN_TP_CHATTERING_SECTION
        {
            NONE,
            TP1,
            TP2,
            TP3,
            TP4,
            TP5,
        }
        private enum STEP_PIO_HANDLING
        {
            START = 0,

            TP1 = 20,

            TP2 = 40,

            TP3 = 60,

            TP4 = 80,

            TP5 = 100,

            END = 120,
        }
        #endregion </Types>

        #region <Methods>

        #region <Overrides>
        public override void InitializeSignals()
        {
            _seqNum = 0;
            _commandResult = new CommandResults("", CommandResult.Proceed);
            _isPIOInterfaceAlarm = false;
            //foreach (var item in OutputSignals)
            //{
            //    WriteOutput(item.Value, false);
            //}

            // 기존의 모든 신호 Off가 아닌, HO_AVBL, ES를 제외한 모든 신호 Off
            foreach (var item in OutputSignals)
            {
                if (item.Key.Equals(E84OutputSignals.HandoffAvailable) || item.Key.Equals(E84OutputSignals.EmergencyStop))
                    continue;

                WriteOutput(item.Value, false);
            }
        }
        public override bool WriteEmergencyStop(bool value)
        {
            if (_isPIOInterfaceAlarm)
            {
                return WritePIOOutput(E84OutputSignals.EmergencyStop, false);
            }
            return WritePIOOutput(E84OutputSignals.EmergencyStop, value);
        }
        public override bool WriteHandoffAvailable(bool value)
        {
            if (_isPIOInterfaceAlarm)
            {
                return WritePIOOutput(E84OutputSignals.HandoffAvailable, false);
            }
            return WritePIOOutput(E84OutputSignals.HandoffAvailable, value);
        }
        public override bool ReadAMHSPIOInput(int inputIndex, bool defaultSignal)
        {
            if (false == InputSignals.ContainsKey((E84InputSignals)inputIndex))
                return defaultSignal;

            return ReadPIOInput((E84InputSignals)inputIndex, defaultSignal);
        }
        public override bool ReadAMHSPIOOutput(int outputIndex)
        {
            if (false == OutputSignals.ContainsKey((E84OutputSignals)outputIndex))
                return false;

            return ReadPIOOutput((E84OutputSignals)outputIndex);
        }
        //private bool SetAlarmStatus()
        //{
        //    return (WriteOutput(E84OutputSignals.EmergencyStop, false) &&
        //        WriteOutput(E84OutputSignals.HandoffAvailable, false));
        //}
        //private bool ResetAlarmStatus()
        //{
        //    return (WriteOutput(E84OutputSignals.EmergencyStop, false) &&
        //        WriteOutput(E84OutputSignals.HandoffAvailable, false) &&
        //        WriteOutput(E84OutputSignals.LoadRequest, false) &&
        //        WriteOutput(E84OutputSignals.UnloadRequest, false) &&
        //        WriteOutput(E84OutputSignals.Ready, false));
        //}
        protected override CommandResults ExecuteHandlingToLoad(LoadPortCommands command)
        {
            return ExecuteHandling(command);
        }
        protected override CommandResults ExecuteHandlingToUnload(LoadPortCommands command)
        {
            return ExecuteHandling(command);
        }
        public override LoadPortLoadingMode CheckTriggerLoadingMode()
        {
            bool cs0 = ReadPIOInput(E84InputSignals.CarrierStage_0, true);
            bool cs1 = ReadPIOInput(E84InputSignals.CarrierStage_1, false);
            if (cs0 && false == cs1)
                return LoadPortLoadingMode.Foup;
            else if (cs1 && false == cs0)
                return LoadPortLoadingMode.Cassette;
            else
                return LoadPortLoadingMode.Unknown;

        }
        public override int GetEmergencyStopSignalIndex()
        {
            foreach (var item in Information.DigitalOutputs)
            {
                if (item.Value.Item2.Equals(E84OutputSignals.EmergencyStop.ToString()))
                {
                    return item.Value.Item1;
                }
            }

            return -1;
        }
        public override bool CheckIsInAccessViolation()
        {
            return !(ReadPIOInput(E84InputSignals.Valid, false));
        }
        public override bool IsPIOInterfaceWorking()
        {
            return (_seqNum > 0);
        }
        public override bool IsAnyPIOInputSignalOn()
        {
            foreach (KeyValuePair< E84InputSignals, int> kv in InputSignals)
            {
                if (ReadPIOInput(kv.Key, false))
                {
                    return true;
                }
            }

            return false;
        }
        public override bool IsAnyPIOOutputSignalOn()
        {
            foreach (KeyValuePair<E84OutputSignals, int> kv in OutputSignals)
            {
                if (kv.Key.Equals(E84OutputSignals.HandoffAvailable) || kv.Key.Equals(E84OutputSignals.EmergencyStop))
                    continue;

                if (ReadPIOOutput(kv.Key))
                {
                    return true;
                }
            }

            return false; 
        }
        public override Dictionary<string, bool> HasActivePIOInputs()
        {
            Dictionary<string, bool>  ActivePIOInputs = new Dictionary<string, bool>();
            foreach (KeyValuePair<E84InputSignals, int> kv in InputSignals)
            {
                ActivePIOInputs.Add(kv.Key.ToString(), ReadPIOInput(kv.Key, false));
            }

            return ActivePIOInputs;
        }
        #endregion </Overrides>

        #region <Timers>
        private void SetTimer(Timers timer)
        {
            switch (timer)
            {
                case Timers.Outputs:
                    SetTickCount(2000);
                    break;
                case Timers.Long:
                    SetTickCount(10000);
                    break;
                case Timers.PlacementSensorStabilizeDelay:
                    SetTickCount(500);
                    break;
                //case Timers.T1:
                //case Timers.T2:
                //case Timers.T5:
                //    SetTickCount(2000);
                //    break;
                default:
                    break;
            }
        }
        private void SetTimerFromParameter(Timers timer)
        {
            // TP1 ~ TP5, TD3
            string timerName = string.Empty;
            uint tickCount;
            int ntickCount = 0;

            timerName = timer.ToString();

            if (false == Enum.TryParse(timerName, out PARAM_COMMON param))
                return;

            ntickCount = Recipe.GetInstance().GetValue(EN_RECIPE_TYPE.COMMON, param.ToString(), 0);

            if (ntickCount < 0)
                return;

            if (param == PARAM_COMMON.TP1 || param == PARAM_COMMON.TP2 || param == PARAM_COMMON.TP3 || param == PARAM_COMMON.TP4 || param == PARAM_COMMON.TP5)
            {
                _enableTP = false;
                _enableTP = 999 <= ntickCount ? false : true;
            }
            if (param == PARAM_COMMON.TD3)
            {
                _enableTD = false;
                _enableTD = 999 <= ntickCount ? false : true;
            }

            tickCount = (uint)ntickCount;

            switch (timer)
            {
                case Timers.TP1:
                case Timers.TP2:
                case Timers.TP3:
                case Timers.TP4:
                case Timers.TP5:
                case Timers.TD3:
                    tickCount *= 1000;
                    break;
                default:
                    break;
            }
            if (timer.Equals(Timers.TD3))
            {
                SetTickCountForDelay(tickCount);
            }
            else
            {
                SetTickCount(tickCount);
            }
        }
        #endregion </Timers>

        #region <Wrapping Interfaces>
        private bool ReadPIOInput(E84InputSignals input, bool defaultSignal)
        {
            if (false == InputSignals.ContainsKey(input))
                return defaultSignal;

            return ReadInput(InputSignals[input], defaultSignal);
        }
        private bool ReadPIOOutput(E84OutputSignals output)
        {
            if (false == OutputSignals.ContainsKey(output))
                return false;

            return ReadOutput(OutputSignals[output]);
        }
        private bool WritePIOOutput(E84OutputSignals output, bool newSignal)
        {
            if (false == OutputSignals.ContainsKey(output))
                return false;

            return WriteOutput(OutputSignals[output], newSignal);
        }
        private CommandResults ExecuteHandling(LoadPortCommands command)
        {
            EN_TP_SECTION section = GetTPSection(_seqNum);

            // TP3은 TP3 구간에서 따로 확인
            if (false == IsCarrierCorrectlyPlacedOnLoadPort(command, section))
            {
                if (command.Equals(LoadPortCommands.AMHSLoading))
                {
                    return ReturnWithErrorCheck(command, CommandResult.Timeout, string.Format("{0} Sensor Logic (Carrier is placed incorrectly. Remove this or load stable.)", section));
                }
                else
                {
                    return ReturnWithErrorCheck(command, CommandResult.Timeout, string.Format("{0} Sensor Logic (Carrier is removed incorrectly. Remove this or load stable.)", section));
                }
            }

            if (false == SetAndMonitoringChattering(_seqNum, command, out string description))
            {
                if (false == string.IsNullOrEmpty(description))
                    return ReturnWithErrorCheck(command, CommandResult.Timeout, description);

                else
                    return ReturnWithErrorCheck(command, CommandResult.Error, description);
            }

            switch (_seqNum)
            {
                #region <Case START>
                case (int)STEP_PIO_HANDLING.START:
                    {
                        ++_seqNum;
                    }
                    break;
                #endregion </Case START>

                #region <Case START: EFEM LoadPort의 물리적, 논리적 재하상태 확인 후, 설비의 [HO_AVBL] ON>
                case (int)STEP_PIO_HANDLING.START + 1:
                    {
                        if (false == (WritePIOOutput(E84OutputSignals.EmergencyStop, true) && (WritePIOOutput(E84OutputSignals.HandoffAvailable, true))))
                            break;

                        ++_seqNum;
                    }
                    break;
                #endregion </Case START: EFEM LoadPort의 물리적, 논리적 재하상태 확인 후, 설비의 [HO_AVBL] ON>

                #region <Case START :OHT의 [VALID], [CS0~1] ON 신호를 보고 설비의 [L_REQ]/[U_REQ] ON>
                case (int)STEP_PIO_HANDLING.START + 2:
                    {
                        if (IsFinishingMode())
                        {
                            return ReturnResultGoodOrNg(command, CommandResult.Skipped, "Stopping Requested");
                        }

                        if (false == (IsAnyCarrierStageSignalsOn() && IsValidSignalsOn()))
                            break;

                        ++_seqNum;
                    }
                    break;
                case (int)STEP_PIO_HANDLING.START + 3:
                    {
                        SetTimer(Timers.Outputs);
                        ++_seqNum;
                    }
                    break;
                case (int)STEP_PIO_HANDLING.START + 4:
                    {
                        E84OutputSignals output = command.Equals(LoadPortCommands.AMHSLoading) ?
                                E84OutputSignals.LoadRequest : E84OutputSignals.UnloadRequest;

                        if (IsTickOver())
                        {
                            return ReturnWithErrorCheck(command, CommandResult.Timeout, string.Format("{0} output signal on timeout", output));
                        }

                        if (false == WritePIOOutput(output, true))
                            break;

                        _seqNum = (int)STEP_PIO_HANDLING.TP1;
                    }
                    break;
                #endregion </Case START:OHT의 [VALID], [CS0~1] ON 신호를 보고 설비의 [L_REQ]/[U_REQ] ON>
                //TP1 구간시작: 20 ~ 49
                #region <Case TP1:OHT는 설비의[L_REQ][U_REQ] ON 신호를 보고[TR_REQ] ON(OHT[TR_REQ] 감시 timeout parameter 필요)>
                case (int)STEP_PIO_HANDLING.TP1:
                    {
                        SetTimerFromParameter(Timers.TP1);     // TR_REQ 감시
                        ++_seqNum;
                    }
                    break;
                case (int)STEP_PIO_HANDLING.TP1 + 1:
                    {
                        if (IsTickOver() && _enableTP)
                        {
                            return ReturnWithErrorCheck(command, CommandResult.Timeout, "TP1 Timeout (TR_REQ signal did not turn ON within specified time.)");
                        }

                        //TEST 끝나고 지워야함//
                        //if (ReadPIOInput(E84InputSignals.Valid, false))
                        //    break;
                        //if (false == ReadPIOInput(E84InputSignals.Valid, false))
                        //    break;
                        //TEST 끝나고 지워야함//

                        if (false == ReadPIOInput(E84InputSignals.TransferRequest, true))
                            break;
                        
                        ++_seqNum;
                    }
                    break;
                #endregion </Case TP1:OHT는 설비의[L_REQ][U_REQ] ON 신호를 보고[TR_REQ] ON(OHT[TR_REQ] 감시 timeout parameter 필요)>

                #region <Case TP1: TP1 감시구간, Ready 전 액션 실행>
                case (int)STEP_PIO_HANDLING.TP1 + 2:
                    {
                        SetTimer(Timers.Long);
                        ++_seqNum;
                    }
                    break;
                case (int)STEP_PIO_HANDLING.TP1 + 3:
                    {
                        if (IsTickOver())
                        {
                            return ReturnWithErrorCheck(command, CommandResult.Timeout, "Action timeout before READY signals on");
                        }

                        switch (command)
                        {
                            case LoadPortCommands.AMHSLoading:
                                {

                                    var result = ExecuteActionBeforeLoad(Index, command);
                                    switch (result.CommandResult)
                                    {
                                        case CommandResult.Proceed:
                                            break;
                                        case CommandResult.Completed:
                                        case CommandResult.Skipped:
                                            ++_seqNum;
                                            break;
                                        case CommandResult.Timeout:
                                        case CommandResult.Error:
                                        case CommandResult.Invalid:
                                            return ReturnWithErrorCheck(command, CommandResult.Error, "Action has error before ready signals on");

                                        default:
                                            return ReturnWithErrorCheck(command, CommandResult.Error, "Action has error before ready signals on");
                                    }
                                }
                                break;
                            case LoadPortCommands.AMHSUnloading:
                                {
                                    var result = ExecuteActionBeforeUnload(Index, command);
                                    switch (result.CommandResult)
                                    {
                                        case CommandResult.Proceed:
                                            break;
                                        case CommandResult.Completed:
                                        case CommandResult.Skipped:
                                            _seqNum = (int)STEP_PIO_HANDLING.TP1 + 5;
                                            break;
                                        case CommandResult.Timeout:
                                        case CommandResult.Error:
                                        case CommandResult.Invalid:
                                            return ReturnWithErrorCheck(command, CommandResult.Error, "Action has error before ready signals on");

                                        default:
                                            return ReturnWithErrorCheck(command, CommandResult.Error, "Action has error before ready signals on");
                                    }
                                }
                                break;
                            default:
                                ++_seqNum;
                                break;
                        }
                    }
                    break;

                case (int)STEP_PIO_HANDLING.TP1 + 4:
                    {
                        if (IsTickOver())
                        {
                            return ReturnWithErrorCheck(command, CommandResult.Timeout, "Changing Mode Action timeout before READY signals on");
                        }

                        var mode = CheckTriggerLoadingMode();
                        var result = ExecuteModeChangeAction(Index, mode, command);
                        switch (result.CommandResult)
                        {
                            case CommandResult.Proceed:
                                break;
                            case CommandResult.Completed:
                            case CommandResult.Skipped:
                                ++_seqNum;
                                break;
                            case CommandResult.Timeout:
                            case CommandResult.Error:
                            case CommandResult.Invalid:
                                return ReturnWithErrorCheck(command, CommandResult.Error, "Changing Mode Action error");

                            default:
                                ++_seqNum;
                                break;
                        }
                    }
                    break;
                #endregion </Case TP1: TP1 감시구간, Ready 전 액션 실행>

                #region <Case 40~41:OHT의[TR_REQ] ON 신호를 보고[READY] ON>
                case (int)STEP_PIO_HANDLING.TP1 + 5:
                    {
                        if (IsTickOver())
                        {
                            return ReturnWithErrorCheck(command, CommandResult.Timeout, "READY output signal timeout");
                        }

                        // TODO : LoadPort Transfer Status도 같이 변경되어야 함. ex) Ready To Load -> Transfer Blocked
                        if (false == WritePIOOutput(E84OutputSignals.Ready, true))
                            break;

                        _seqNum = (int)STEP_PIO_HANDLING.TP2;
                    }
                    break;
                #endregion </Case 40~41:OHT의[TR_REQ] ON 신호를 보고[READY] ON>
                //TP2 구간시작: 50 ~ 59
                #region <Case TP2: TP2 감시구간, OHT는 설비의 [READY] ON 신호를 보고 [BUSY] ON (OHT [BUSY] 감시 timeout parameter 필요)>
                case (int)STEP_PIO_HANDLING.TP2:
                    {
                        SetTimerFromParameter(Timers.TP2);     // Busy 감시
                        ++_seqNum;
                    }
                    break;
                case (int)STEP_PIO_HANDLING.TP2 + 1:
                    {
                        if (IsTickOver() && _enableTP)
                        {
                            return ReturnWithErrorCheck(command, CommandResult.Timeout, "TP2 Timeout (BUSY signal did not turn ON within specified time.)");
                        }

                        if (false == ReadPIOInput(E84InputSignals.Busy, true))
                            break;
                        
                        _seqNum = (int)STEP_PIO_HANDLING.TP3;
                    }
                    break;
                #endregion </Case TP2: TP2 감시구간, OHT는 설비의 [READY] ON 신호를 보고 [BUSY] ON (OHT [BUSY] 감시 timeout parameter 필요)>
                //TP3 구간시작: 60 ~ 79
                #region <Case TP3: TP3 감시구간, 설비는 CARRIER 안착 감시 (CARRIER 안착 timeout parameter 필요)>
                case (int)STEP_PIO_HANDLING.TP3:
                    {
                        SetTimerFromParameter(Timers.TP3);     // 자재 안착 감시
                        ++_seqNum;
                    }
                    break;
                case (int)STEP_PIO_HANDLING.TP3 + 1:
                    {
                        if (IsTickOver() && _enableTP)
                        {
                            if (command.Equals(LoadPortCommands.AMHSLoading))
                            {
                                return ReturnWithErrorCheck(command, CommandResult.Timeout, "TP3 Timeout (Carrier was not detected within specified time.)");
                            }
                            else
                            {
                                return ReturnWithErrorCheck(command, CommandResult.Timeout, "TP3 Timeout (Carrier was not removed within specified time.)");
                            }
                        }

                        if (false == IsPlacementMismatch() && false == IsCarrierPresence(command) && false == IsCarrierPlaced(command))
                            break;

                        SetTimerFromParameter(Timers.TD3);     // 자재 안착 후, L_REQ, U_REQ Off Delay
                        ++_seqNum;
                    }
                    break;
                case (int)STEP_PIO_HANDLING.TP3 + 2:
                    {
                        if (IsTickOver() && _enableTP)
                        {
                            if (command.Equals(LoadPortCommands.AMHSLoading))
                            {
                                return ReturnWithErrorCheck(command, CommandResult.Timeout, "TP3 Timeout (Carrier was not detected within specified time.)");
                            }
                            else
                            {
                                return ReturnWithErrorCheck(command, CommandResult.Timeout, "TP3 Timeout (Carrier was not removed within specified time.)");
                            }
                        }
                        if (IsTickOverForDelay() && _enableTD)
                        {
                            return ReturnWithErrorCheck(command, CommandResult.Timeout, "TP3 Sensor Logic (Carrier is placed incorrectly. Remove this or load stable.)");
                        }

                        // Load 작업 중 Carrier를 최초 감지한 후에 TD3시간 이내에 Placement/Presence 센서가 적어도 하나 이상 감지되지 않은 경우 에러 발생
                        // Unload 작업 중 Carrier를 최초 미감지한 후에 TD3시간 이내에 Placement/Presence 센서가 적어도 하나 이상 미감지되지 않은 경우 에러 발생
                        if (IsPlacementMismatch() || false == GetTriggerCarrierPresence(command))
                            break;

                        ++_seqNum;
                    }
                    break;
                #endregion </Case TP3: TP3 감시구간, 설비는 CARRIER 안착 감시 (CARRIER 안착 timeout parameter 필요)>

                #region <Case TP3:설비는 자재 감지/미감지되면 각각 [L_REQ]/[U_REQ] OFF (설비의 [READY] ON 이후 자재 감지까지의 timeout parameter 필요)>
                case (int)STEP_PIO_HANDLING.TP3 + 3:
                    {
                        SetTimer(Timers.Outputs);
                        ++_seqNum;
                    }
                    break;
                case (int)STEP_PIO_HANDLING.TP3 + 4:
                    {
                        E84OutputSignals output = command.Equals(LoadPortCommands.AMHSLoading) ? E84OutputSignals.LoadRequest : E84OutputSignals.UnloadRequest;
                        string strCommand = command.Equals(LoadPortCommands.AMHSLoading) ? "L_REQ" : "U_REQ";

                        if (IsTickOver())
                        {
                            return ReturnWithErrorCheck(command, CommandResult.Timeout, string.Format("{0} output signal off timeout", strCommand));
                        }

                        if (false == WritePIOOutput(output, false))
                            break;

                        //_seqNum = (int)STEP_PIO_HANDLING.TP4;
                        ++_seqNum;
                    }
                    break;
                case (int)STEP_PIO_HANDLING.TP3 + 5:
                    {
                        SetTimer(Timers.PlacementSensorStabilizeDelay);
                        ++_seqNum;
                    }
                    break;
                case (int)STEP_PIO_HANDLING.TP3 + 6:
                    {
                        if (false == IsTickOver())
                            break;

                        if (IsPlacementMismatch() || false == GetTriggerCarrierPresence(command))
                            break;

                        _seqNum = (int)STEP_PIO_HANDLING.TP4;
                    }
                    break;
                #endregion </Case TP3:설비는 자재 감지/미감지되면 각각 [L_REQ]/[U_REQ] OFF (설비의 [READY] ON 이후 자재 감지까지의 timeout parameter 필요)>
                //TP4 구간시작: 80 ~ 99
                #region <Case TP4: TP4 감시구간, OHT는 전송 완료 시 [BUSY] OFF, [COMPT] ON, [TR_REQ] OFF>
                case (int)STEP_PIO_HANDLING.TP4:
                    {
                        SetTimerFromParameter(Timers.TP4);
                        ++_seqNum;
                    }
                    break;
                case (int)STEP_PIO_HANDLING.TP4 + 1:
                    {
                        if (IsTickOver() && _enableTP)
                        {
                            if (ReadPIOInput(E84InputSignals.Busy, false))
                                return ReturnWithErrorCheck(command, CommandResult.Error, "TP4 Timeout (BUSY signal did not turn OFF within specified time.)");

                            if (ReadPIOInput(E84InputSignals.TransferRequest, false))
                                return ReturnWithErrorCheck(command, CommandResult.Error, "TP4 Timeout (TR_REQ signal did not turn OFF within specified time.)");
                             
                            if (false == ReadPIOInput(E84InputSignals.Complete, true))
                                return ReturnWithErrorCheck(command, CommandResult.Error, "TP4 Timeout (COMPT signal did not turn ON within specified time.)");
                        }

                        if (false == IsHandlingCompleted())
                            break;

                        ++_seqNum;
                    }
                    break;
                #endregion </Case TP4: TP4 감시구간, OHT는 전송 완료 시 [BUSY] OFF, [COMPT] ON, [TR_REQ] OFF>

                #region <Case TP4: READY 출력신호 OFF, 설비는 OHT [COMPT] ON 확인 후 [READY] OFF>
                case (int)STEP_PIO_HANDLING.TP4 + 2:
                    {
                        SetTimer(Timers.Outputs);
                        ++_seqNum;
                    }
                    break;
                case (int)STEP_PIO_HANDLING.TP4 + 3:
                    {
                        if (IsTickOver())
                        {
                            return ReturnWithErrorCheck(command, CommandResult.Timeout, "READY output signal off timeout");
                        }

                        if (false == WritePIOOutput(E84OutputSignals.Ready, false))
                            break;

                        _seqNum = (int)STEP_PIO_HANDLING.TP5;
                    }
                    break;
                #endregion </Case TP4:설비는 OHT [COMPT] ON 확인 후 [READY] OFF>
                //TP5 구간시작: 100 ~ 109
                #region <Case TP5: TP5 감시구간, OHT는 설비의 [READY] OFF 확인 후 [COMPT], [CS0~1], [VALID] 전부 OFF (설비 timeout parameter 필요)>
                case (int)STEP_PIO_HANDLING.TP5:
                    {
                        SetTimerFromParameter(Timers.TP5);     // VALID OFF 감시
                        ++_seqNum;
                    }
                    break;
                case (int)STEP_PIO_HANDLING.TP5 + 1:
                    {
                        if (IsTickOver() && _enableTP)
                        {
                            if (ReadPIOInput(E84InputSignals.Valid, false))
                                return ReturnWithErrorCheck(command, CommandResult.Error, "TP5 Timeout (VALID signal did not turn OFF within specified time.)");

                            if (ReadPIOInput(E84InputSignals.Complete, false))
                                return ReturnWithErrorCheck(command, CommandResult.Error, "TP5 Timeout (COMPT signal did not turn OFF within specified time.)");

                            if (ReadPIOInput(E84InputSignals.CarrierStage_0, false))
                                return ReturnWithErrorCheck(command, CommandResult.Error, "TP5 Timeout (CS_0 signal did not turn OFF within specified time.)");
                        }

                        if (false == IsInterfaceCompleted())
                            break;

                        _seqNum = (int)STEP_PIO_HANDLING.END;
                    }
                    break;
                #endregion </Case TP5: TP5 감시구간, OHT는 설비의 [READY] OFF 확인 후 [COMPT], [CS0~1], [VALID] 전부 OFF (설비 timeout parameter 필요)>

                #region <Case END:설비는 HO_AVBL 출력신호 OFF>
                case (int)STEP_PIO_HANDLING.END:
                    {
                        SetTimer(Timers.Outputs);
                        ++_seqNum;
                    }
                    break;
                case (int)STEP_PIO_HANDLING.END + 1:
                    {
                        if (IsTickOver())
                        {
                            return ReturnWithErrorCheck(command, CommandResult.Timeout, "HO_AVBL output signal off timeout");
                        }

                        if (false == WritePIOOutput(E84OutputSignals.HandoffAvailable, false))
                            break;
                        // TODO : 원래는 Complete 신호가 Off 되고 Transfer Blocked도 Ready To Load로 바뀌어야하는데,
                        //        HO_AVBL Off될 때 바꾸도록 한다. (Unloading만 해당된다. Load는 없다.)

                        ++_seqNum;
                    }
                    break;
                case (int)STEP_PIO_HANDLING.END + 2:
                    {
                        SetTimer(Timers.Outputs);
                        ++_seqNum;
                    }
                    break;
                case (int)STEP_PIO_HANDLING.END + 3:
                    {
                        if (false == IsTickOver())
                            break;

                        InitializeSignals();
                        return ReturnResultGoodOrNg(command, CommandResult.Completed, string.Empty);
                    }
                #endregion </Case END:설비는 HO_AVBL 출력신호 OFF>

                default:
                    break;
            }

            _commandResult.ActionName = command.ToString();
            _commandResult.CommandResult = CommandResult.Proceed;
            return _commandResult;
        }
        #endregion </Wrapping Interfaces>

        #region <Signal Wrappers>
        private bool IsValidSignalsOn()
        {
            return ReadPIOInput(E84InputSignals.Valid, true);
        }
        private bool IsAnyCarrierStageSignalsOn()
        {
            // 삼성전자 M1라인의 E84에서는 CS_1을 일체 사용하지 않는다고 함
            return ReadPIOInput(E84InputSignals.CarrierStage_0, true)
                || ReadPIOInput(E84InputSignals.CarrierStage_1, false);
        }
        private bool IsHandlingCompleted()
        {
            // Busy Off -> Complete On -> TransferRequest Off면 완료
            return (false == ReadPIOInput(E84InputSignals.Busy, false) &&
                false == ReadPIOInput(E84InputSignals.TransferRequest, false) &&
                ReadPIOInput(E84InputSignals.Complete, true));
        }
        private bool IsInterfaceCompleted()
        {
            return (false == ReadPIOInput(E84InputSignals.Complete, false) &&
                false == ReadPIOInput(E84InputSignals.CarrierStage_0, false) &&
                false == ReadPIOInput(E84InputSignals.CarrierStage_1, false) &&
                false == ReadPIOInput(E84InputSignals.Valid, false));
        }
        //public override void SetNormalStatus()
        //{
        //    _seqNum = 0;
        //    _isPIOInterfaceAlarm = false;
        //    //_commandResult = new CommandResults("", CommandResult.Proceed);
        //    foreach (var item in OutputSignals)
        //    {
        //        if (item.Key.Equals(E84OutputSignals.HandoffAvailable) || item.Key.Equals(E84OutputSignals.EmergencyStop))
        //            continue;

        //        WriteOutput(item.Value, false);
        //    }
        //}
        private CommandResults ReturnWithErrorCheck(LoadPortCommands command, CommandResult commandResult, string description)
        {
            _isPIOInterfaceAlarm = true;
            WritePIOOutput(E84OutputSignals.EmergencyStop, false);
            WritePIOOutput(E84OutputSignals.HandoffAvailable, false);

            return ReturnResultGoodOrNg(command, commandResult, description);
        }
        //
        private bool SetChatteringTime(E84InputSignals inputSignal, Timers settingTimer, bool monitoredSignal)
        {
            string timerName = string.Empty;
            uint tickCount = 0;
            int ntickCount = 0;

            if (false == (settingTimer == Timers.TC1 || settingTimer == Timers.TC2))
                return false;

            timerName = settingTimer.ToString();

            if (false == Enum.TryParse(timerName, out PARAM_COMMON param))
                return false;

            ntickCount = Recipe.GetInstance().GetValue(EN_RECIPE_TYPE.COMMON, param.ToString(), 0);

            if (ntickCount < 0)
                return false;

            tickCount = (uint)ntickCount;

            _monitorChattering.Add(inputSignal, new ChatteringTime());

            // 정상 신호가 ON이고, Off으로 Chattering되는 것을 감시할 경우 Reverse한다.
            if (false == monitoredSignal)
            {
                _monitorChattering[inputSignal].Reverse = true;
            }
            _monitorChattering[inputSignal].MonitoredSignal = monitoredSignal;
            _monitorChattering[inputSignal].ChatteringTimeLimit = TimeSpan.FromMilliseconds(tickCount);    // tickCount가 0이면 자동으로 TimeSpan.Zero으로 들어감

            return true;
        }
        private bool MonitorChatteringTime(out string alarmMessage, out string chatteringTimeSignalValue)
        {
            bool readInput;
            foreach (var item in _monitorChattering)
            {
                if (item.Value.MonitoredSignal)
                {
                    readInput = ReadPIOInput(item.Key, false);
                }
                else
                {
                    readInput = ReadPIOInput(item.Key, true);
                }

                if (item.Value.CheckChatteringTimeOver(readInput))
                {
                    // Chattering 시간 초과
                    chatteringTimeSignalValue = readInput ? "ON" : "OFF";
                    alarmMessage = item.Key.ToString();
                    return false;
                }
            }
            alarmMessage = string.Empty;
            chatteringTimeSignalValue = string.Empty;

            return true;
        }
        private void ResetChattering()
        {
            _monitorChattering.Clear();
        }
        private bool SetAndMonitoringChattering(int seqNum, LoadPortCommands command, out string despription)
        {
            despription = string.Empty;
            if (GetChatteringSection(seqNum, out EN_TP_CHATTERING_SECTION chatteringSection))
            {
                switch (seqNum)
                {
                    case (int)STEP_PIO_HANDLING.TP1:
                        ResetChattering();
                        if (false == (SetChatteringTime(E84InputSignals.CarrierStage_0, Timers.TC1, false)
                                && SetChatteringTime(E84InputSignals.Valid, Timers.TC1, false)
                                && SetChatteringTime(E84InputSignals.Busy, Timers.TC1, true)
                                && SetChatteringTime(E84InputSignals.Complete, Timers.TC1, true)))
                        {
                            despription = "Action has TC1 Setting error before [TransferRequest] signals on";
                            return false;
                        }
                        break;
                    case (int)STEP_PIO_HANDLING.TP2:
                        ResetChattering();
                        if (false == (SetChatteringTime(E84InputSignals.CarrierStage_0, Timers.TC1, false)
                                && SetChatteringTime(E84InputSignals.Valid, Timers.TC1, false)
                                && SetChatteringTime(E84InputSignals.TransferRequest, Timers.TC1, false)
                                && SetChatteringTime(E84InputSignals.Complete, Timers.TC1, true)))
                        {
                            despription = "Action has TC1 Setting error before [BUSY] signals on";
                            return false;
                        }
                        break;
                    case (int)STEP_PIO_HANDLING.TP3:
                        string strCommand = command.Equals(LoadPortCommands.AMHSLoading) ? "Load" : "Unload";
                        ResetChattering();
                        if (false == (SetChatteringTime(E84InputSignals.CarrierStage_0, Timers.TC2, false)
                                && SetChatteringTime(E84InputSignals.Valid, Timers.TC2, false)
                                && SetChatteringTime(E84InputSignals.TransferRequest, Timers.TC2, false)
                                && SetChatteringTime(E84InputSignals.Busy, Timers.TC2, false)
                                && SetChatteringTime(E84InputSignals.Complete, Timers.TC2, true)))
                        {
                            despription = string.Format("Action has TC2 Setting error before {0} Carrier", strCommand);
                            return false;
                        }
                        break;
                    case (int)STEP_PIO_HANDLING.TP4:
                        ResetChattering();
                        if (false == (SetChatteringTime(E84InputSignals.CarrierStage_0, Timers.TC2, false)
                                && SetChatteringTime(E84InputSignals.Valid, Timers.TC2, false)))
                        {
                            despription = "Action has TC2 Setting error before [Ready] signals Off";
                            return false;
                        }
                        break;
                    case (int)STEP_PIO_HANDLING.TP5:
                        ResetChattering();
                        if (false == (SetChatteringTime(E84InputSignals.TransferRequest, Timers.TC1, true)
                                && SetChatteringTime(E84InputSignals.Busy, Timers.TC1, true)))
                        {
                            despription = "Action has TC1 Setting error before [VALID] signals Off";
                            return false;
                        }
                        break;
                    default:
                        break;
                }
            }
            else
            {
                if (chatteringSection == EN_TP_CHATTERING_SECTION.NONE)
                    return true;

                string strChattetingTPName = string.Empty;
                strChattetingTPName = chatteringSection.ToString();
                if (false == MonitorChatteringTime(out string alarmMessage, out string value))
                {
                    despription = string.Format("{0} Illegal sequence ({1} signal was turned {2} improperly.)", strChattetingTPName, alarmMessage, value);
                    return false;
                }
            }

            return true;
        }
        #endregion </Signal Wrappers>

        #region <ETC>
        private bool IsCarrierCorrectlyPlacedOnLoadPort(LoadPortCommands command, EN_TP_SECTION section)
        {
            switch (section)
            {
                case EN_TP_SECTION.TP1:
                case EN_TP_SECTION.TP2:
                    {
                        if (IsPlacementMismatch())
                            return false;

                        return CheckCarrierReadyForCommand(command);
                    }
                case EN_TP_SECTION.TP3:
                    return true;
                case EN_TP_SECTION.TP4:
                case EN_TP_SECTION.TP5:
                    {
                        if (IsPlacementMismatch())
                            return false;

                        return GetTriggerCarrierPresence(command);
                    }
                case EN_TP_SECTION.START:
                case EN_TP_SECTION.END:
                    return true;
                default:
                    return false;
            }

            #region <수정 전>
            //switch (command)
            //{
            //    case LoadPortCommands.AMHSLoading:
            //        {
            //            switch (section)
            //            {
            //                case TPSection.None:
            //                    break;
            //                case TPSection.Before_TP1:
            //                    break;
            //                    // IsCarrierPlaced수정해서 쓰는게 좋을듯싶다
            //                    // 수정방향은 매개변수로 받는 command를 없애는 방식으로
            //                case TPSection.TP1:
            //                case TPSection.TP2:
            //                    CheckCarrierReadyForCommand(command);
            //                    break;
            //                case TPSection.TP3:
            //                    break;
            //                case TPSection.TP4:
            //                case TPSection.TP5:
            //                    GetTriggerCarrierPresence(command);
            //                    break;
            //                case TPSection.After_TP5:
            //                    break;
            //                default:
            //                    return false;
            //            }
            //        }
            //        break;
            //    case LoadPortCommands.AMHSUnloading:
            //        {
            //            switch (section)
            //            {
            //                case TPSection.None:
            //                    break;
            //                case TPSection.Before_TP1:
            //                    break;
            //                case TPSection.TP1:
            //                    break;
            //                case TPSection.TP2:
            //                    break;
            //                case TPSection.TP3:
            //                    break;
            //                case TPSection.TP4:
            //                    break;
            //                case TPSection.TP5:
            //                    break;
            //                case TPSection.After_TP5:
            //                    break;
            //                default:
            //                    return false;
            //            }
            //        }
            //        break;
            //    default:
            //        return false;
            //}
            #endregion </수정 전>
        }
        private EN_TP_SECTION GetTPSection(int seqNum)
        {
            if (seqNum == (int)STEP_PIO_HANDLING.START)
                return EN_TP_SECTION.START;

            else if ((int)STEP_PIO_HANDLING.START < seqNum && seqNum < (int)STEP_PIO_HANDLING.TP2)
                return EN_TP_SECTION.TP1;

            else if ((int)STEP_PIO_HANDLING.TP2 <= seqNum && seqNum < (int)STEP_PIO_HANDLING.TP3)
                return EN_TP_SECTION.TP2;

            else if ((int)STEP_PIO_HANDLING.TP3 <= seqNum && seqNum < (int)STEP_PIO_HANDLING.TP4)
                return EN_TP_SECTION.TP3;

            else if ((int)STEP_PIO_HANDLING.TP4 <= seqNum && seqNum < (int)STEP_PIO_HANDLING.TP5)
                return EN_TP_SECTION.TP4;

            else if ((int)STEP_PIO_HANDLING.TP5 <= seqNum && seqNum < (int)STEP_PIO_HANDLING.END)
                return EN_TP_SECTION.TP5;

            else if ((int)STEP_PIO_HANDLING.END <= seqNum)
                return EN_TP_SECTION.END;

            else
                return EN_TP_SECTION.None;
        }
        private bool GetChatteringSection(int seqNum, out EN_TP_CHATTERING_SECTION section)
        {
            //if ((int)STEP_PIO_HANDLING.TP1 < seqNum && seqNum < (int)STEP_PIO_HANDLING.TP2)
            if ((int)STEP_PIO_HANDLING.TP1 == seqNum || (int)STEP_PIO_HANDLING.TP2 == seqNum || (int)STEP_PIO_HANDLING.TP3 == seqNum
                || (int)STEP_PIO_HANDLING.TP4 == seqNum || (int)STEP_PIO_HANDLING.TP5 == seqNum)
            {
                section = EN_TP_CHATTERING_SECTION.NONE;
                return true;
            }

            if ((int)STEP_PIO_HANDLING.TP1 < seqNum && seqNum < (int)STEP_PIO_HANDLING.TP2)
            {
                section = EN_TP_CHATTERING_SECTION.TP1;
            }
            else if ((int)STEP_PIO_HANDLING.TP2 < seqNum && seqNum < (int)STEP_PIO_HANDLING.TP3)
            {
                section = EN_TP_CHATTERING_SECTION.TP2;
            }
            else if ((int)STEP_PIO_HANDLING.TP3 < seqNum && seqNum < (int)STEP_PIO_HANDLING.TP4)
            {
                section = EN_TP_CHATTERING_SECTION.TP3;
            }
            else if ((int)STEP_PIO_HANDLING.TP4 < seqNum && seqNum < (int)STEP_PIO_HANDLING.TP5)
            {
                section = EN_TP_CHATTERING_SECTION.TP4;
            }
            else if ((int)STEP_PIO_HANDLING.TP5 < seqNum && seqNum < (int)STEP_PIO_HANDLING.END)
            {
                section = EN_TP_CHATTERING_SECTION.TP5;
            }
            else
            {
                section = EN_TP_CHATTERING_SECTION.NONE;
            }
            return false;
        }
        #endregion </ETC>

        #endregion </Methods>
    }
    // 2025.09.30 dwlim [ADD] E84의 Chattering Time Monitoring을 위한 Class 추가
    public class ChatteringTime
    {
        #region <Constructors>
        #endregion </Constructors>

        #region <Fields>
        private Stopwatch _chatteringStopwatch = new Stopwatch();
        #endregion </Fields>      

        #region <Property>
        public bool MonitoredSignal { get; set; } = false;
        public bool Reverse { get; set; } = false;
        public bool _isActive { get; set; } = false;
        public TimeSpan ChatteringTimeLimit { get; set; } = new TimeSpan();
        #endregion </Property>   

        //정상 신호가 ON이고, Off으로 Chattering되는 것을 감시할 경우 Reverse한다.
        public bool CheckChatteringTimeOver(bool chatteringSignal)
        {
            bool signal = Reverse ? !chatteringSignal : chatteringSignal;

            if (ChatteringTimeLimit <= TimeSpan.Zero)
            {
                // 채터링 설정값이 0이면 Unuse상태이고, ON이면 바로 Error Message
                return signal;
            }

            if (!signal)
            {
                _isActive = false;
                _chatteringStopwatch.Reset();
                return false;
            }

            if (!_isActive)
            {
                _isActive = true;
                _chatteringStopwatch.Restart();
                return false;
            }

            return _chatteringStopwatch.Elapsed >= ChatteringTimeLimit;
        }

        //public bool CheckChatteringTimeOver(bool chatteringSignal)
        //{
        //    bool signal = Reverse ? !chatteringSignal : chatteringSignal;

        //    if (signal)
        //    {
        //        // 최초 On 전환 시점 기억
        //        if (!_isActive)
        //        {
        //            _isActive = true;
        //            Stopwatch.Restart();
        //        }

        //        // On 상태 지속 시간이 TimeLimit을 넘었는지 확인
        //        TimeSpan tresult = Stopwatch.Elapsed;
        //        //Console.WriteLine(tresult);
        //        if (tresult >= ChatteringTimeLimit)
        //        {
        //            //Console.WriteLine(tresult);
        //            return true;
        //        }
        //        else
        //        {
        //            //Console.WriteLine(tresult);
        //        }

        //        return false;
        //    }

        //    // Off 상태면 초기화
        //    if (_isActive)
        //    {
        //        _isActive = false;
        //        Stopwatch.Reset();
        //    }
        //    return false;
        //}

        //public bool CheckChatteringTimeOver(bool chatteringSignal)
        //{
        //    bool signal = Reverse ? !chatteringSignal : chatteringSignal;

        //    if (signal)
        //    {
        //        // 최초 On 전환 시점 기억
        //        if (_onSince == null)
        //            _onSince = DateTime.UtcNow;

        //        // On 상태 지속 시간이 TimeLimit을 넘었는지 확인
        //        //return DateTime.UtcNow - _onSince >= ChatteringTimeLimit;

        //        TimeSpan tresult = DateTime.UtcNow - _onSince.Value;
        //        //result = tresult >= ChatteringTimeLimit;
        //        if (tresult >= ChatteringTimeLimit)
        //        {
        //            Console.WriteLine(tresult);
        //            return true;
        //        }


        //        return false;
        //    }

        //    // Off 상태면 초기화
        //    _onSince = null;
        //    return false;
        //}

        //public bool CheckChatteringTimeOver(bool chatteringSignal)
        //{
        //    bool signal = Reverse ? !chatteringSignal : chatteringSignal;

        //    if (signal)
        //    {
        //        if (!_isActive)
        //        {
        //            _isActive = true;
        //            Stopwatch.Restart();
        //        }

        //        return Stopwatch.Elapsed > ChatteringTimeLimit;
        //    }

        //    if (_isActive)
        //    {
        //        _isActive = false;
        //        Stopwatch.Reset();  // 다음 구간 측정을 위해 정지/리셋
        //    }

        //    return false;
        //}
    }
    #endregion </Class&Struct>

    #region <Delegates>
    public delegate void ButtonPressedEventHandler();
    public delegate void LoadPortModeEventHandler(bool trigger);
    public delegate void SlotMapStateUpdatedEventHandler(int portId, IDictionary<int, CarrierSlotMapStates> slotMaps);
    #endregion </Delegates>

    #region <State Transition>
    public enum VerificationTransitionPolicy
    {
        Immediate = 0,
        WaitForHostResult = 1
    }

    public enum HostVerificationResult
    {
        None = 0,
        Accepted = 1,
        Rejected = 2
    }
    public enum CarrierDetectionStates
    {
        NoneDetected = 0,     // Present = false, Placed = false
        PartiallyDetected = 1,// Present ^ Placed
        FullyDetected = 2     // Present = true, Placed = true
    }

    public sealed class VerificationTransitionOptions
    {
        public VerificationTransitionPolicy CarrierIdPolicy { get; set; }
            = VerificationTransitionPolicy.WaitForHostResult;

        public VerificationTransitionPolicy SlotMapPolicy { get; set; }
            = VerificationTransitionPolicy.WaitForHostResult;
    }

    public struct TransferStateChangedEvent
    {
        public int PortId { get; set; }
        public LoadPortTransferStates PreviousState { get; set; }
        public LoadPortTransferStates CurrentState { get; set; }
    }

    public struct CarrierIdStateChangedEvent
    {
        public int PortId { get; set; }
        public CarrierIdVerificationStates PreviousState { get; set; }
        public CarrierIdVerificationStates CurrentState { get; set; }
    }

    public struct CarrierSlotMapStateChangedEvent
    {
        public int PortId { get; set; }
        public CarrierSlotMapVerificationStates PreviousState { get; set; }
        public CarrierSlotMapVerificationStates CurrentState { get; set; }
    }
    public struct ReservationStateChangedEvent
    {
        public int PortId { get; set; }
        public ReservationStates PreviousState { get; set; }
        public ReservationStates CurrentState { get; set; }
    }

    public struct AssociationStateChangedEvent
    {
        public int PortId { get; set; }
        public string PreviousCarrierId { get; set; }
        public string CurrentCarrierId { get; set; }
        public AssociationStates PreviousState { get; set; }
        public AssociationStates CurrentState { get; set; }
    }
    public struct AccessModeChangedEvent
    {
        public int PortId { get; set; }
        public LoadPortAccessMode PreviousMode { get; set; }
        public LoadPortAccessMode CurrentMode { get; set; }
    }
    public struct CarrierAccessingStateChangedEvent
    {
        public int PortId { get; set; }
        public CarrierAccessStates PreviousState { get; set; }
        public CarrierAccessStates CurrentState { get; set; }
    }
    public delegate void TransferStateChangedHandler(object sender, TransferStateChangedEvent e);
    public delegate void CarrierIdStateChangedHandler(object sender, CarrierIdStateChangedEvent e);
    public delegate void CarrierSlotMapStateChangedHandler(object sender, CarrierSlotMapStateChangedEvent e);
    public delegate void ReservationStateChangedHandler(object sender, ReservationStateChangedEvent e);
    public delegate void AssociationStateChangedHandler(object sender, AssociationStateChangedEvent e);
    public delegate void AccessModeChangedHandler(object sender, AccessModeChangedEvent e);
    public delegate void CarrierAccessingStateChangedHandler(object sender, CarrierAccessingStateChangedEvent e);
    public delegate void CarrierDetectionChangedHandler(object sender, CarrierDetectionChangedEvent e);

    public struct CarrierDetectionChangedEvent
    {
        public int PortId { get; set; }

        public bool PreviousPresent { get; set; }
        public bool PreviousPlaced { get; set; }

        public bool CurrentPresent { get; set; }
        public bool CurrentPlaced { get; set; }

        public CarrierDetectionStates PreviousState { get; set; }
        public CarrierDetectionStates CurrentState { get; set; }
    }

    public interface ILoadPortStateModel
    {
        int PortId { get; }
        bool SupportsReservationState { get; }
        bool SupportsAssociationState { get; }

        ReservationStates ReservationState { get; }
        AssociationStates AssociationState { get; }

        event TransferStateChangedHandler TransferStateChanged;
        event CarrierIdStateChangedHandler CarrierIdStateChanged;
        event CarrierSlotMapStateChangedHandler CarrierSlotMapStateChanged;
        event ReservationStateChangedHandler ReservationStateChanged;
        event AssociationStateChangedHandler AssociationStateChanged;
        event AccessModeChangedHandler AccessModeChanged;
        event CarrierAccessingStateChangedHandler CarrierAccessingStateChanged;

        void Initialize();
        void Reset();
        LoadPortRecoveryData CreateRecoveryData();
        void RecoverFromObservation(LoadPortRecoveryData recoveryData, in LoadPortObservation observation);
        void UpdateObservation(in LoadPortObservation observation);
        bool Evaluate();
        void ApplyExternalInput(in LoadPortExternalInput input);
        void CopyStateTo(LoadPortStateInformation state);
        bool CanChangeAccessMode(LoadPortAccessMode targetMode);
    }
    public struct LoadPortObservation
    {
        public int PortId;
        public bool Enabled;
        public bool Initialized;
        public bool Present;
        public bool Placed;
        public bool IsPlacementMismatch;
        public bool ClampState;
        public bool DockState;
        public bool DoorState;
        public bool ReadyForWork;
        public bool PlacementErrorState;
        public bool CarrierOutErrorState;
        public string TriggeredAlarm;
        public LoadPortAccessMode AccessMode;
        public LoadPortLoadingMode LoadingType;
        public CarrierAccessStates CarrierAccessingState;
    }
    public enum LoadPortExternalInputType
    {
        CarrierIdVerificationAccepted,
        CarrierIdVerificationRejected,
        CarrierSlotMapVerificationAccepted,
        CarrierSlotMapVerificationRejected,

        // E87 Transfer State 전이 #2:
        // OUT OF SERVICE -> IN SERVICE
        // 의미:
        // 장비/포트 서비스 상태를 사용 가능(In Service)으로 전환하라는 서비스 요청
        ChangeServiceStatusToInService,

        // E87 Transfer State 전이 #3:
        // IN SERVICE -> OUT OF SERVICE
        // 의미:
        // 장비/포트 서비스 상태를 사용 불가(Out Of Service)로 전환하라는 서비스 요청
        ChangeServiceStatusToOutOfService,

        // E87 Transfer State 전이 #6:
        // READY TO LOAD -> TRANSFER BLOCKED
        // 의미:
        // 자동 로드 전송이 PIO READY로 시작되었음을 알리는 입력
        LoadTransferStartedByPioReady,

        // E87 Transfer State 전이 #7:
        // READY TO UNLOAD -> TRANSFER BLOCKED
        // 의미:
        // 자동 언로드 전송이 PIO READY로 시작되었음을 알리는 입력
        UnloadTransferStartedByPioReady,

        // E87 Transfer State 전이 #8:
        // TRANSFER BLOCKED -> READY TO LOAD
        // 의미:
        // 자동 언로드 전송이 PIO COMPT로 정상 완료되었음을 알리는 입력
        UnloadTransferCompletedByPioCompt,

        // E87 Transfer State 전이 #10:
        // TRANSFER BLOCKED -> TRANSFER READY
        // 의미:
        // 전송이 실패했음을 알리는 입력
        // 현재 구현은 TRANSFER READY를 외부 상태로 두지 않으므로,
        // 내부적으로는 ReadyToLoad 또는 ReadyToUnload로 평탄화해서 처리한다.
        TransferFailed,

        // E87 Transfer State 전이 #7의 한 원인:
        // READY TO UNLOAD -> TRANSFER BLOCKED
        // 의미:
        // CarrierReCreate 서비스가 발행되어 다시 전송 준비/재전송 시작 상태로 들어가야 함
        CarrierReCreateIssued,

        // E87 Transfer State 전이 #9의 구현용 축약 입력:
        // TRANSFER BLOCKED -> READY TO UNLOAD
        // 의미:
        // 문서상 "캐리어가 다시 unload position으로 반환됨" 또는
        // 공정 완료 / CancelCarrier / 내부 버퍼 복귀 등의 결과로
        // 이제 unload 가능한 상태가 되었음을 알리는 구현용 입력
        // 참고: 이 이름 자체는 구현용 축약명이며, 문서 원문 용어를 그대로 옮긴 것은 아니다.
        CarrierReturnedToPort,

        // E87 ChangeAccess 서비스 축약 입력
        ChangeAccessModeToAuto,
        ChangeAccessModeToManual,

        ReserveAtPort,
        CancelReservationAtPort,
        BindAssociation,
        UnbindAssociation
    }
    public enum ReservationStates
    {
        Unknown = -1,
        NotReserved = 0,
        Reserved = 1
    }
    public enum AssociationStates
    {
        Unknown = -1,
        NotAssociated = 0,
        Associated = 1
    }
    public struct LoadPortExternalInput
    {
        public int PortId;
        public LoadPortExternalInputType InputType;
        public bool BooleanValue;
        public string CarrierId;
    }
    #endregion </State Transition>
}