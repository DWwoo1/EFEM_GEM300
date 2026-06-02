using System;
using System.Collections.Generic;
using System.Linq;

using EFEM.Defines.Common;
using EFEM.Modules.LoadPort;
using EFEM.Defines.LoadPort;
using EFEM.Defines.CarrierManagement;
using EFEM.Modules.LoadPort.Scheduler;

namespace EFEM.Modules
{
    public class LoadPortManager
    {
        #region <Constructors>
        public LoadPortManager() { }
        #endregion </Constructors>

        #region <Fields>
        private static LoadPortManager _instance = null;

        private readonly Dictionary<int, LoadPortOperator> LoadPorts = new Dictionary<int, LoadPortOperator>();
        #endregion </Fields>

        #region <Properties>
        public static LoadPortManager Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new LoadPortManager();
                }

                return _instance;
            }
        }

        public int Count
        {
            get
            {
                return LoadPorts.Count;
            }
        }
        public int MaxCapacity { get { return 25; } }
        #endregion </Properties>

        #region <Methods>

        #region <Assign, Object>
        public void AssignLoadPorts(LoadPortOperator loadPort)
        {
            int index = LoadPorts.Count;
            LoadPorts.Add(index, loadPort);

            //_loadPortes[index].InitController();
        }
        public void ExitLoadPorts()
        {
            foreach (var item in LoadPorts)
            {
                item.Value.SaveRecoveryData();
            }
        }
        public LoadPortLogger GetLogger(int lpIndex)
        {
            if (false == LoadPorts.ContainsKey(lpIndex))
                return null;

            return LoadPorts[lpIndex].Logger;
        }
        public ICarrierService GetCarrierService(int lpIndex)
        {
            if (false == LoadPorts.ContainsKey(lpIndex))
                return null;

            return LoadPorts[lpIndex].CarrierService;
        }

        public int GetLoadPortIndexByPortId(int portId)
        {
            foreach (var item in LoadPorts)
            {
                if (item.Value.PortId == portId)
                    return item.Key;
            }

            return -1;
        }

        public int GetLoadPortPortId(int lpIndex)
        {
            if (false == LoadPorts.ContainsKey(lpIndex))
                return -1;

            return LoadPorts[lpIndex].PortId;
        }

        public int GetLoadPortPortId(string name)
        {
            foreach (var item in LoadPorts)
            {
                if (item.Value.Name.Equals(name))
                    return item.Value.PortId;
            }

            return -1;
        }

        public void SetLoadPortEnabled(int lpIndex, bool enabled)
        {
            if (false == LoadPorts.ContainsKey(lpIndex))
                return;

            LoadPorts[lpIndex].EnableLoadPort(enabled);
        }
        public bool IsLoadPortEnabled(int lpIndex)
        {
            if (false == LoadPorts.ContainsKey(lpIndex))
                return false;

            return LoadPorts[lpIndex].Enabled;
        }
        //public bool GetCarrier(int lpIndex, ref Carrier carrier)
        //{
        //    if (false == LoadPorts.ContainsKey(lpIndex))
        //        return false;

        //    return LoadPorts[lpIndex].GetCarrier(ref carrier);
        //}
        public LoadPortStateInformation GetLoadPortState(int lpIndex)
        {
            if (false == LoadPorts.ContainsKey(lpIndex))
                return null;

            return LoadPorts[lpIndex].GetLoadPortState();
        }
        public bool GetLoadPortTransferState(int lpIndex, ref LoadPortTransferStates transferState)
        {
            if (false == LoadPorts.ContainsKey(lpIndex))
                return false;

            transferState = LoadPorts[lpIndex].TransferState;
            return true;
        }
        public bool GetLoadPortCarrierIdState(int lpIndex, ref CarrierIdVerificationStates carrierIdState)
        {
            if (false == LoadPorts.ContainsKey(lpIndex))
                return false;

            carrierIdState = LoadPorts[lpIndex].CarrierIdVerificationState;
            return true;
        }
        public bool GetLoadPortCarrierSlotMapState(int lpIndex, ref CarrierSlotMapVerificationStates slotMapState)
        {
            if (false == LoadPorts.ContainsKey(lpIndex))
                return false;

            slotMapState = LoadPorts[lpIndex].CarrierSlotMapVerificationState;
            return true;
        }

        public void RecreateCarrierAtLoadPort(int lpIndex)
        {
            if (false == LoadPorts.ContainsKey(lpIndex))
                return;

            LoadPorts[lpIndex].RecreateCarrier();
        }
        #endregion </Assign, Object>

        #region <Event Handler>
        public void AttachModeChangerEventHandler(int lpIndex, LoadPortLoadingMode type, LoadPortModeEventHandler eventHandler)
        {
            if (false == LoadPorts.ContainsKey(lpIndex))
                return;

            LoadPorts[lpIndex].AttachModeChangerEventHandler(type, eventHandler);
        }
        public void AttachMechanicalButtonEventHandlers(int lpIndex, LoadPortButtonTypes type, ButtonPressedEventHandler eventHandler)
        {
            if (false == LoadPorts.ContainsKey(lpIndex))
                return;

            LoadPorts[lpIndex].AttachMechanicalButtonEventHandlers(type, eventHandler);
        }
        public void AttachBusySignalByDigitalInput(int lpIndex, int signalIndex, Func<int, bool> functionToReadInput)
        {
            if (false == LoadPorts.ContainsKey(lpIndex))
                return;

            if (signalIndex >= 0)
            {
                LoadPorts[lpIndex].AttachBusySignalByDigitalInput(() => functionToReadInput(signalIndex));
            }
        }
        #endregion </Event Handler>

        #region <Execute>
        public void Execute()
        {
            foreach (var item in LoadPorts)
            {
                item.Value.Execute();
            }
        }
        #endregion </Execute>

        #region <Scheduler>
        public void RegisterCompletionCondition(
            int lpIndex,
            ICarrierCompletionCondition condition,
            ICarrierCompletionHandlingPolicy policy)
        {
            if (false == LoadPorts.ContainsKey(lpIndex))
                return;

            LoadPorts[lpIndex].RegisterCompletionCondition(condition);
            LoadPorts[lpIndex].RegisterCompletionHandlingPolicy(policy);
        }
        public CARRIER_PORT_TYPE ExecuteSchedulers(int lpIndex)
        {
            if (false == LoadPorts.ContainsKey(lpIndex))
                return CARRIER_PORT_TYPE.SELECTION;

            return LoadPorts[lpIndex].ExecuteSchedulers();
        }
        public void ChangeSlotMapForDryRun(int lpIndex)
        {
            if (false == LoadPorts.ContainsKey(lpIndex))
                return;

            LoadPorts[lpIndex].ChangeSlotMapForDryRun();
        }
        #endregion </Scheduler>

        #region <Actions>
        public void InitLoadPortAction(int lpIndex)
        {
            if (false == LoadPorts.ContainsKey(lpIndex))
                return;

            LoadPorts[lpIndex].InitAction();
        }

        public CommandResults InitializeLoadPort(int lpIndex)
        {
            if (false == LoadPorts.ContainsKey(lpIndex))
                return new CommandResults(LoadPortCommands.Initialize.ToString(), CommandResult.Error);

            return LoadPorts[lpIndex].Initialize();
        }
        public CommandResults ClearAlarmLoadPort(int lpIndex)
        {
            if (false == LoadPorts.ContainsKey(lpIndex))
                return new CommandResults(LoadPortCommands.Reset.ToString(), CommandResult.Error);

            return LoadPorts[lpIndex].ClearAlarm();
        }
        public CommandResults LoadCarrierAtLoadPort(int lpIndex)
        {
            if (false == LoadPorts.ContainsKey(lpIndex))
                return new CommandResults(LoadPortCommands.Load.ToString(), CommandResult.Error);

            return LoadPorts[lpIndex].Load();
        }
        public CommandResults UnloadCarrierAtLoadPort(int lpIndex)
        {
            if (false == LoadPorts.ContainsKey(lpIndex))
                return new CommandResults(LoadPortCommands.Unload.ToString(), CommandResult.Error);

            return LoadPorts[lpIndex].Unload();
        }
        public CommandResults ClampCarrierAtLoadPort(int lpIndex)
        {
            if (false == LoadPorts.ContainsKey(lpIndex))
                return new CommandResults(LoadPortCommands.Clamp.ToString(), CommandResult.Error);

            return LoadPorts[lpIndex].Clamp();
        }
        public CommandResults ReleaseCarrierAtLoadPort(int lpIndex)
        {
            if (false == LoadPorts.ContainsKey(lpIndex))
                return new CommandResults(LoadPortCommands.Unclamp.ToString(), CommandResult.Error);

            return LoadPorts[lpIndex].UnClamp();
        }
        public CommandResults DockCarrierAtLoadPort(int lpIndex)
        {
            if (false == LoadPorts.ContainsKey(lpIndex))
                return new CommandResults(LoadPortCommands.Dock.ToString(), CommandResult.Error);

            return LoadPorts[lpIndex].Dock();
        }
        public CommandResults UnDockCarrierAtLoadPort(int lpIndex)
        {
            if (false == LoadPorts.ContainsKey(lpIndex))
                return new CommandResults(LoadPortCommands.Undock.ToString(), CommandResult.Error);

            return LoadPorts[lpIndex].UnDock();
        }
        public CommandResults OpenCarrierAtLoadPort(int lpIndex)
        {
            if (false == LoadPorts.ContainsKey(lpIndex))
                return new CommandResults(LoadPortCommands.DoorOpen.ToString(), CommandResult.Error);

            return LoadPorts[lpIndex].OpenDoor();
        }
        public CommandResults CloseCarrierAtLoadPort(int lpIndex)
        {
            if (false == LoadPorts.ContainsKey(lpIndex))
                return new CommandResults(LoadPortCommands.DoorClose.ToString(), CommandResult.Error);

            return LoadPorts[lpIndex].CloseDoor();
        }
        public CommandResults ScanCarrierAtLoadPort(int lpIndex)
        {
            if (false == LoadPorts.ContainsKey(lpIndex))
                return new CommandResults(LoadPortCommands.ScanDown.ToString(), CommandResult.Error);

            return LoadPorts[lpIndex].Scan();
        }
        public CommandResults GetMapCarrierAtLoadPort(int lpIndex)
        {
            if (false == LoadPorts.ContainsKey(lpIndex))
                return new CommandResults(LoadPortCommands.GetMap.ToString(), CommandResult.Error);

            return LoadPorts[lpIndex].GetSlotMap();
        }
        public CommandResults ChangeLoadPortMode(LoadPortLoadingMode mode, int lpIndex)
        {
            if (false == LoadPorts.ContainsKey(lpIndex))
            {
                return new CommandResults($"ChangeTo{mode}", CommandResult.Error);
            }

            return LoadPorts[lpIndex].ChangeCarrierMode(mode);
        }
        public CommandResults FindCurrentLoadingMode(int lpIndex)
        {
            if (false == LoadPorts.ContainsKey(lpIndex))
                return new CommandResults(LoadPortCommands.FindLoadingMode.ToString(), CommandResult.Error);

            return LoadPorts[lpIndex].FindCarrierMode();
        }
        public CommandResults ChangeLoadPortMode(int lpIndex, LoadPortLoadingMode type)
        {
            if (false == LoadPorts.ContainsKey(lpIndex))
                return new CommandResults(LoadPortCommands.GetMap.ToString(), CommandResult.Error);

            return LoadPorts[lpIndex].ChangeCarrierMode(type);
        }
        public CommandResults ChangeLoadPortAccessModeToAuto(int lpIndex)
        {
            if (false == LoadPorts.ContainsKey(lpIndex))
                return new CommandResults(LoadPortCommands.ChangeAccessModeToAuto.ToString(), CommandResult.Error);

            return LoadPorts[lpIndex].ChangeAccess(LoadPortAccessMode.Auto);
        }
        public CommandResults ChangeLoadPortAccessModeToManual(int lpIndex)
        {
            if (false == LoadPorts.ContainsKey(lpIndex))
                return new CommandResults(LoadPortCommands.ChangeAccessModeToManual.ToString(), CommandResult.Error);

            return LoadPorts[lpIndex].ChangeAccess(LoadPortAccessMode.Manual);
        }
        public CommandResults ChangeLoadPortAccessMode(int lpIndex, LoadPortAccessMode mode)
        {
            if (false == LoadPorts.ContainsKey(lpIndex))
                return new CommandResults(LoadPortCommands.GetMap.ToString(), CommandResult.Error);

            return LoadPorts[lpIndex].ChangeAccess(mode);
        }
        #endregion </Actions>

        #region <States>
        public bool IsConnectedWithController(int lpIndex)
        {
            if (false == LoadPorts.ContainsKey(lpIndex))
                return false;

            return LoadPorts[lpIndex].IsConnected;
        }
        public bool GetInitializationState(int lpIndex)
        {
            if (false == LoadPorts.ContainsKey(lpIndex))
                return false;

            return LoadPorts[lpIndex].Initialized;
        }
        public bool IsLoadPortBusy(int lpIndex)
        {
            if (false == LoadPorts.ContainsKey(lpIndex))
                return false;

            return LoadPorts[lpIndex].IsLoadPortBusy;
        }
        public bool GetPresentState(int lpIndex)
        {
            if (false == LoadPorts.ContainsKey(lpIndex))
                return false;

            return LoadPorts[lpIndex].Present;
        }
        public bool GetPlacedState(int lpIndex)
        {
            if (false == LoadPorts.ContainsKey(lpIndex))
                return false;

            return LoadPorts[lpIndex].Placed;
        }
        public bool IsPlacementMismatch(int lpIndex)
        {
            if (false == LoadPorts.ContainsKey(lpIndex))
                return false;

            return LoadPorts[lpIndex].IsPlacementMismatch;
        }
        public bool GetClampingState(int lpIndex)
        {
            if (false == LoadPorts.ContainsKey(lpIndex))
                return false;

            return LoadPorts[lpIndex].ClampState;
        }

        public bool GetDockingState(int lpIndex)
        {
            if (false == LoadPorts.ContainsKey(lpIndex))
                return false;

            return LoadPorts[lpIndex].DockState;
        }

        public bool GetDoorState(int lpIndex)
        {
            if (false == LoadPorts.ContainsKey(lpIndex))
                return false;

            return LoadPorts[lpIndex].DoorState;
        }

        public bool HasErrorStatusByPlacementError(int lpIndex)
        {
            if (false == LoadPorts.ContainsKey(lpIndex))
                return false;

            return LoadPorts[lpIndex].PlacementErrorState;
        }

        public bool HasErrorStatusByCarrierOut(int lpIndex)
        {
            if (false == LoadPorts.ContainsKey(lpIndex))
                return false;

            return LoadPorts[lpIndex].CarrierOutErrorState;
        }

        public bool HasTriggeredAlarm(int lpIndex, ref string alarmDescription)
        {
            if (false == LoadPorts.ContainsKey(lpIndex))
                return false;

            alarmDescription = LoadPorts[lpIndex].TriggeredControllerAlarm;
            return (false == string.IsNullOrEmpty(alarmDescription));
        }
        public LoadPortLoadingMode GetCarrierLoadingType(int lpIndex)
        {
            if (false == LoadPorts.ContainsKey(lpIndex))
                return LoadPortLoadingMode.Unknown;

            return LoadPorts[lpIndex].LoadingType;
        }

        public LoadPortAccessMode GetAccessMode(int lpIndex)
        {
            if (false == LoadPorts.ContainsKey(lpIndex))
                return LoadPortAccessMode.Manual;

            return LoadPorts[lpIndex].AccessMode;
        }

        public bool IsInAccessViolation(int lpIndex)
        {
            if (false == LoadPorts.ContainsKey(lpIndex))
                return false;

            return LoadPorts[lpIndex].IsInAccessViolation;
        }
        public bool IsPIOInterfaceWorking(int lpIndex)
        {
            if (false == LoadPorts.ContainsKey(lpIndex))
                return false;

            return LoadPorts[lpIndex].IsPIOInterfaceWorking;
        }
        public bool IsAnyPIOInputSignalOn(int lpIndex)
        {
            if (false == LoadPorts.ContainsKey(lpIndex))
                return false;

            return LoadPorts[lpIndex].IsAnyPIOInputSignalOn;
        }
        public bool IsAnyPIOOutputSignalOn(int lpIndex)
        {
            if (false == LoadPorts.ContainsKey(lpIndex))
                return false;

            return LoadPorts[lpIndex].IsAnyPIOOutputSignalOn;
        }
        public Dictionary<string, bool> HasActivePIOInputs(int lpIndex)
        {
            if (false == LoadPorts.ContainsKey(lpIndex))
                return null;

            return LoadPorts[lpIndex].HasActivePIOInputs;
        }
        public bool IsLoadPortSimulationMode(int lpIndex)
        {
            if (false == LoadPorts.ContainsKey(lpIndex))
                return false;

            return LoadPorts[lpIndex].IsLoadPortSimulationMode;
        }
        #endregion <States>

        #region <E87 Services>      
        public void PostCarrierIdVerificationResult(int lpIndex, bool isSuccess)
        {
            if (false == LoadPorts.ContainsKey(lpIndex))
                return;

            if (isSuccess)
                LoadPorts[lpIndex].ProceedWithCarrierForId(null, null);
            else
                LoadPorts[lpIndex].CancelCarrier(null);
        }

        public void PostCarrierSlotMapVerificationResult(int lpIndex, bool isSuccess)
        {
            if (false == LoadPorts.ContainsKey(lpIndex))
                return;

            if (isSuccess)
                LoadPorts[lpIndex].ProceedWithCarrierForSlot(null, null);
            else
                LoadPorts[lpIndex].CancelCarrier(null);
        }

        /// <summary>
        /// E87 Transfer State 전이 #8:
        /// TRANSFER BLOCKED -> READY TO LOAD
        /// 의미:
        /// 자동 언로드 전송이 E84/PIO COMPT로 정상 완료되었음을 상태모델에 전달한다.
        /// </summary>
        public void PostUnloadTransferCompletedByPioCompt(int lpIndex)
        {
            if (false == LoadPorts.ContainsKey(lpIndex))
                return;

            LoadPorts[lpIndex].NotifyUnloadTransferCompleted();
        }

        /// <summary>
        /// E87 Transfer State 전이 #10:
        /// TRANSFER BLOCKED -> TRANSFER READY
        /// 의미:
        /// 전송 실패를 상태모델에 전달한다.
        /// 현재 구현은 TRANSFER READY를 외부 상태로 두지 않으므로
        /// 내부적으로 ReadyToLoad 또는 ReadyToUnload로 평탄화해서 처리한다.
        /// </summary>
        public void PostTransferFailed(int lpIndex)
        {
            if (false == LoadPorts.ContainsKey(lpIndex))
                return;

            LoadPorts[lpIndex].NotifyTransferFailed();
        }

        /// <summary>
        /// E87 Transfer State 전이 #7의 원인 중 하나:
        /// READY TO UNLOAD -> TRANSFER BLOCKED
        /// 의미:
        /// CarrierReCreate 서비스가 발행되었음을 상태모델에 전달한다.
        /// 재전송/재구성 시작 경계로 사용한다.
        /// </summary>
        public void PostCarrierReCreateIssued(int lpIndex)
        {
            if (false == LoadPorts.ContainsKey(lpIndex))
                return;

            LoadPorts[lpIndex].CarrierReCreate();
        }

        public void AssociateCarrier(int lpIndex, string carrierId)
        {
            if (false == LoadPorts.ContainsKey(lpIndex))
                return;

            LoadPorts[lpIndex].Association(carrierId);
        }
        public void UnAssociateCarrier(int lpIndex)
        {
            if (false == LoadPorts.ContainsKey(lpIndex))
                return;

            LoadPorts[lpIndex].UnAssociation();
        }
        #endregion </E87 Services>

        #region <ETC>
        public string GetLoadPortName(int lpIndex)
        {
            if (false == LoadPorts.ContainsKey(lpIndex))
                return null;

            return LoadPorts[lpIndex].Name;
        }

        public string GetCurrentLocationId(int portId)
        {
            foreach (var item in LoadPorts)
            {
                if (item.Value.PortId.Equals(portId))
                {
                    return item.Value.GetCurrentLocationName();
                }
            }
            return string.Empty;
        }
        #endregion </ETC>

        #region <AMHS>
        public bool AssignAMHSSignalControlFunctions(int lpIndex,
            Func<int, bool> functionToReadInput,
            Func<int, bool> functionToReadOutput,
            Func<int, bool, DigitalIO_.DIO_RESULT> functionToWriteOutput)
        {
            if (false == LoadPorts.ContainsKey(lpIndex))
                return false;

            return LoadPorts[lpIndex].AssignAMHSSignalControlFunctions(functionToReadInput, functionToReadOutput, functionToWriteOutput);
        }

        public bool AssignActionBeforeCarrierLoads(int lpIndex, Func<int, CommandResults> action)
        {
            if (false == LoadPorts.ContainsKey(lpIndex))
                return false;

            return LoadPorts[lpIndex].AssignActionBeforeCarrierLoads(action);
        }
        public bool WriteAMHSEmergencyStop(int lpIndex, bool value)
        {
            if (false == LoadPorts.ContainsKey(lpIndex))
                return false;

            return LoadPorts[lpIndex].WriteAMHSEmergencyStop(value);
        }
        public bool WriteAMHSHandoffAvailable(int lpIndex, bool value)
        {
            if (false == LoadPorts.ContainsKey(lpIndex))
                return false;

            return LoadPorts[lpIndex].WriteAMHSHandoffAvailable(value);
        }
        public bool ReadPIOInput(int lpIndex, int inputIndex, bool defaultValue)
        {
            if (false == LoadPorts.ContainsKey(lpIndex))
                return false;

            return LoadPorts[lpIndex].ReadPIOInput(inputIndex, defaultValue);
        }
        public bool ReadPIOOutput(int lpIndex, int outputIndex)
        {
            if (false == LoadPorts.ContainsKey(lpIndex))
                return false;

            return LoadPorts[lpIndex].ReadPIOOutput(outputIndex);
        }
        public bool GetAMHSSaftyInterLockStatus(int lpIndex)
        {
            if (false == LoadPorts.ContainsKey(lpIndex))
                return false;

            return LoadPorts[lpIndex].GetAMHSSaftyInterLockStatus();
        }
        public bool GetAMHSSignalValues(int lpIndex, ref Dictionary<int, bool> inputs, ref Dictionary<int, bool> outputs)
        {
            if (false == LoadPorts.ContainsKey(lpIndex))
                return false;

            return LoadPorts[lpIndex].GetAMHSSignalValues(ref inputs, ref outputs);
        }
        public bool GetAMHSInformation(int lpIndex, ref AMHSInformation information)
        {
            if (false == LoadPorts.ContainsKey(lpIndex))
                return false;

            return LoadPorts[lpIndex].GetAMHSInformation(ref information);
        }

        public bool InitializeAMHSSignals(int lpIndex)
        {
            if (false == LoadPorts.ContainsKey(lpIndex))
                return false;

            return LoadPorts[lpIndex].InitializeSignals();
        }
        //public bool SetNormalStatus(int lpIndex)
        //{
        //    if (false == LoadPorts.ContainsKey(lpIndex))
        //        return false;

        //    return LoadPorts[lpIndex].SetNormalStatus();
        //}
        public CommandResults ExecuteAMHSHandlingToLoad(int lpIndex)
        {
            if (false == LoadPorts.ContainsKey(lpIndex))
                return new CommandResults(LoadPortCommands.AMHSLoading.ToString(), CommandResult.Error);

            return LoadPorts[lpIndex].ExecuteAMHSHandlingToLoad();
        }
        public CommandResults ExecuteAMHSHandlingToUnload(int lpIndex)
        {
            if (false == LoadPorts.ContainsKey(lpIndex))
                return new CommandResults(LoadPortCommands.AMHSUnloading.ToString(), CommandResult.Error);

            return LoadPorts[lpIndex].ExecuteAMHSHandlingToUnload();
        }
        public bool WriteAMHSOutput(int lpIndex, int index, bool newValue)
        {
            if (false == LoadPorts.ContainsKey(lpIndex))
                return false;

            return LoadPorts[lpIndex].WriteAMHSOutput(index, newValue);
        }
        public bool WriteAMHSStopSignal(int lpIndex, bool newValue)
        {
            if (false == LoadPorts.ContainsKey(lpIndex))
                return false;

            return LoadPorts[lpIndex].WriteAMHSStopSignal(newValue);
        }
        #endregion </AMHS>

        #endregion </Methods>
    }
}