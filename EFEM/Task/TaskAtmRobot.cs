using System;
using System.Collections.Generic;
using System.Collections.Concurrent;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;

using RunningTask_;

using FrameOfSystem3.Functional;
using FrameOfSystem3.DynamicLink_;
using FrameOfSystem3.Log;
using FrameOfSystem3.Work;
using FrameOfSystem3.SubSequence;

using Define.DefineEnumBase.Common;
using Define.DefineEnumBase.Log;
using Define.DefineEnumProject.Map;
using Define.DefineEnumProject.Task;
using Define.DefineEnumProject.Task.AtmRobot;
using Define.DefineEnumProject.Mail;
using Define.DefineEnumProject.SubSequence;
using Define.DefineEnumProject.Tool;

using EFEM.Modules;
using EFEM.Defines.Common;
using EFEM.Defines.AtmRobot;
using EFEM.Modules.AtmRobot;
using EFEM.MaterialTracking;
using EFEM.ActionScheduler;
using EFEM.ActionScheduler.RobotActionSchedulers;

namespace FrameOfSystem3.Task
{
    abstract class TaskAtmRobot : RunningTaskWrapper
    {
        #region constructor
        public TaskAtmRobot(int nIndexOfTask, string strTaskName, RecoveryData recovery)
            : base(nIndexOfTask, strTaskName, typeof(PARAM_PROCESS))
        {
            _taskOperator = TaskOperator.GetInstance();
            
            _recoveryData = recovery;
            //_recovery = new TaskAtmRobotRecovery(strTaskName, 1);
            AddRecoveryData(strTaskName, _recoveryData);

            _robotManager = AtmRobotManager.Instance;

            if (Enum.TryParse(strTaskName, out EN_TASK_LIST taskType))
            {
                //switch (taskType)
                //{
                //    case EN_TASK_LIST.AtmRobot:
                //        RobotIndex = (int)taskType - (int)EN_TASK_LIST.AtmRobot;
                //        break;
                //    default:
                //        throw new Exception(string.Format("----- Invalid Task Name : {0} -----", taskType.ToString()));
                //}
                RobotIndex = (int)taskType - (int)EN_TASK_LIST.AtmRobot;
            }

            _robotSchedulerManager = RobotActionSchedulerManager.Instance;
            _substrateManager = SubstrateManager.Instance;
            _processGroup = ProcessModuleGroup.Instance;
            _carrierServer = CarrierManagementServer.Instance;

            RobotName = _robotManager.GetRobotName(RobotIndex);

            _loadPortManager = LoadPortManager.Instance;

            int busyStatusIndex = 0;
            if (GetBusySignalIndex(RobotIndex, ref busyStatusIndex))
            {
                _robotManager.AttachBusySignalByDigitalInput(RobotIndex, busyStatusIndex, DigitalIO_.DigitalIO.GetInstance().ReadInput);
            }

            int alarmStatusIndex = 0;
            if (GetAlarmSignalIndex(RobotIndex, ref alarmStatusIndex))
            {
                _robotManager.AttachAlarmSignalByDigitalInput(RobotIndex, alarmStatusIndex, DigitalIO_.DigitalIO.GetInstance().ReadInput);
            }

            int servoStatusIndex = 0;
            if (GetServoSignalIndex(RobotIndex, ref servoStatusIndex))
            {
                _robotManager.AttachServoSignalByDigitalInput(RobotIndex, servoStatusIndex, DigitalIO_.DigitalIO.GetInstance().ReadInput);
            }

            Logger = _robotManager.GetLogger(RobotIndex);
        }
        protected override void MakeMappingTableForAction()
        {
            foreach (TASK_ACTION enAction in Enum.GetValues(typeof(TASK_ACTION)))
            {
                m_mapppingForAction.Add(enAction.ToString(), enAction);
            }
        }
        #endregion constructor

        #region field

        #region default

        #region instance
        static TaskOperator _taskOperator = null;
        protected static RecoveryData _recoveryData = null;
        //static TaskAtmRobotRecovery _recovery = null;
        #endregion /instance

        protected TASK_ACTION m_enAction = TASK_ACTION.STOP;
        Dictionary<string, TASK_ACTION> m_mapppingForAction = new Dictionary<string, TASK_ACTION>();
        #endregion /default

        #region <Robot>
        protected readonly int RobotIndex;
        protected readonly string RobotName;
        //private readonly AtmRobotController _robotController = null;

        protected static AtmRobotManager _robotManager = null;
        protected static RobotActionSchedulerManager _robotSchedulerManager = null;
        protected static SubstrateManager _substrateManager = null;
        protected static CarrierManagementServer _carrierServer = null; 
        protected static ProcessModuleGroup _processGroup = null;

        protected RobotWorkingInfo _workingInfo = null;

        protected bool _prevInitializationState = false;
        protected readonly TickCounter_.TickCounter RobotTicks = new TickCounter_.TickCounter();

        protected static LoadPortManager _loadPortManager = null;
        #endregion </Robot>

        #endregion /field

        #region <Properties>
        protected AtmRobotLogger Logger { get; private set; }
        #endregion </Properties>

        #region <Enum>
        enum SubStepPick
        {
            Init,
            BeforeApproachUnloading,
            ActionApproachUnloading,
            AfterApproachUnloading,
            BeforeActionUnloading,
            ActionUnloading,
            AfterActionUnloading,
            End
        }

        enum SubStepPlace
        {
            Init,
            BeforeApproachLoading,
            ActionApproachLoading,
            AfterApproachLoading,
            BeforeActionLoading,
            ActionLoading,
            AfterActionLoading,
            End
        }
        #endregion </Enum>

        #region inherit

        #region sequence
        protected override bool DoInitializeSequence()
        {
            if (_taskOperator.IsFinishingMode())
                return true;

            switch (m_nSeqNum)
            {
                case (int)STEP_INITIALIZE.START:
                    Views.Functional.Form_ProgressBar.GetInstance().ShowForm(GetTaskName(), (uint)STEP_INITIALIZE.END);
                    InitTemporaryData();
                    InitializeDynamicLinkState();
                    _robotManager.InitAtmRobotAction(RobotIndex);
                    if (false == CheckControllerConnectionStatus())
                        return true;

                    Logger.WriteActionStartLog("Initialize", string.Empty);
                    m_nSeqNum = (int)STEP_INITIALIZE.CHECK_ALARM_STATUS;
                    break;

                case (int)STEP_INITIALIZE.CHECK_ALARM_STATUS:
                    if (_robotManager.IsRobotBusy(RobotIndex))
                        break;

                    // 알람 해제 시도 5초 제한
                    RobotTicks.SetTickCount(5000);
                    ++m_nSeqNum;
                    break;

                case (int)STEP_INITIALIZE.CHECK_ALARM_STATUS + 1:
                    if (_robotManager.IsRobotBusy(RobotIndex))
                        break;

                    _robotManager.InitAtmRobotAction(RobotIndex);
                    //if (_robotManager.IsRobotAlarm(RobotIndex))
                    //{
                    //    ++m_nSeqNum;
                    //}
                    //else
                    {
                        m_nSeqNum = (int)STEP_INITIALIZE.PREPARE;
                    }
                    break;

                case (int)STEP_INITIALIZE.CHECK_ALARM_STATUS + 2:
                    {
                        var result = _robotManager.Clear(RobotIndex);
                        switch (result.CommandResult)
                        {
                            //case EN_COMMAND_RESULT.PROCEED:
                            //    break;
                            case CommandResult.Completed:
                            case CommandResult.Timeout:
                            case CommandResult.Error:
                            case CommandResult.Invalid:
                                {
                                    if (RobotTicks.IsTickOver(true))
                                    {
                                        string[] _arAlarmSubInfo = { GetTaskName(), result.Description };
                                        GenerateSequenceAlarm((int)EN_ALARM.ATM_ROBOT_ALARM_CLEARING_FAILED, false, ref _arAlarmSubInfo);
                                        m_nSeqNum = (int)STEP_INITIALIZE.END;
                                    }
                                    else
                                    {
                                        if (false == result.CommandResult.Equals(CommandResult.Completed))
                                        {
                                            SetDelayForSequence(100);
                                        }

                                        --m_nSeqNum;
                                    }
                                }
                                break;

                            default:
                                ++m_nSeqNum;
                                break;
                        }
                    }
                    break;

                case (int)STEP_INITIALIZE.CHECK_ALARM_STATUS + 3:
                    --m_nSeqNum;
                    break;

                case (int)STEP_INITIALIZE.PREPARE:
                    {
                        var result = _robotManager.InitializeAtmRobot(RobotIndex);
                        switch (result.CommandResult)
                        {
                            //case EN_COMMAND_RESULT.PROCEED:
                            //    break;
                            case CommandResult.Completed:
                                {
                                    SetSignal();
                                    m_nSeqNum = (int)STEP_INITIALIZE.END;
                                }
                                break;

                            case CommandResult.Timeout:
                            case CommandResult.Error:
                            case CommandResult.Invalid:
                                {
                                    string[] _arAlarmSubInfo = { GetTaskName(), string.Format("{0} - {1}", result.ActionName, result.Description) };
                                    GenerateSequenceAlarm((int)EN_ALARM.ATM_ROBOT_INITIALIZING_FAILED, false, ref _arAlarmSubInfo);
                                    m_nSeqNum = (int)STEP_INITIALIZE.END;
                                }
                                break;

                            default:
                                ++m_nSeqNum;
                                break;
                        }
                    }
                    break;

                case (int)STEP_INITIALIZE.PREPARE + 1:
                    {
                        --m_nSeqNum;
                    }
                    break;

                case (int)STEP_INITIALIZE.END:
                    Logger.WriteActionEndLog("Initialize", string.Empty);
                    return true;
            }

            Views.Functional.Form_ProgressBar.GetInstance().UpdateStep(GetTaskName(), (uint)m_nSeqNum);
            return false;
        }
        protected override bool DoEntrySequence()
        {
            if (_taskOperator.IsFinishingMode())
                return true;

            switch (m_nSeqNum)
            {
                case (int)STEP_ENTRY.START:
                    InitTemporaryData();
                    InitializeDynamicLinkState();
                    Logger.WriteActionStartLog("Entry", string.Empty);
                    if (false == CheckControllerConnectionStatus())
                        return true;

                    ++m_nSeqNum;
                    break;

                case (int)STEP_ENTRY.START + 1:
                    {
                        string[] tasks = null;
                        string[][] manual = null;
                        bool isManualOperation = _taskOperator.GetManualOperation(ref tasks, ref manual);
                        bool hasInvalidSubstrate = false;
                        if (tasks != null || false == isManualOperation)
                        {
                            //bool needToCheck = false;
                            if (false == isManualOperation)
                            {
                                //needToCheck = true;
                                Dictionary<RobotArmTypes, Substrate> substrateAtArm = new Dictionary<RobotArmTypes, Substrate>();
                                if (_substrateManager.GetSubstratesAtRobotAll(RobotName, ref substrateAtArm))
                                {
                                    foreach (var item in substrateAtArm)
                                    {
                                        if (item.Value == null)
                                            continue;

                                        if (item.Value.SourcePortId <= 0)
                                        {
                                            hasInvalidSubstrate = true;
                                            break;
                                        }
                                    }
                                }
                            }
                        }

                        if (hasInvalidSubstrate)
                        {
                            GenerateAlarm((int)EN_ALARM.ATM_ROBOT_CANNOT_GET_WORKING_INFO);
                            m_nSeqNum = (int)STEP_ENTRY.END;
                        }
                        else
                        {
                            m_nSeqNum = (int)STEP_ENTRY.PREPARE;
                        }
                    }
                    break;

                case (int)STEP_ENTRY.PREPARE:
                    var result = _robotManager.Clear(RobotIndex);
                    switch (result.CommandResult)
                    {
                        //case EN_COMMAND_RESULT.PROCEED:
                        //    break;
                        case CommandResult.Completed:
                            {
                                m_nSeqNum = (int)STEP_ENTRY.END;
                            }
                            break;

                        case CommandResult.Timeout:
                        case CommandResult.Error:
                        case CommandResult.Invalid:
                            {
                                string[] _arAlarmSubInfo = { GetTaskName(), string.Format("{0} - {1}", result.ActionName, result.Description) };
                                GenerateSequenceAlarm((int)EN_ALARM.ATM_ROBOT_ALARM_CLEARING_FAILED, false, ref _arAlarmSubInfo);
                                m_nSeqNum = (int)STEP_ENTRY.END;
                            }
                            break;

                        default:
                            ++m_nSeqNum;
                            break;
                    }
                    break;

                case (int)STEP_ENTRY.PREPARE + 1:
                    {
                        if (_taskOperator.IsFinishingMode())
                        {
                            m_nSeqNum = (int)STEP_ENTRY.END;
                            break;
                        }
                    }
                    --m_nSeqNum;
                    break;

                case (int)STEP_ENTRY.END:
                    Logger.WriteActionEndLog("Entry", string.Empty);
                    SetRobotPortState(RobotScheduleType.Selection);
                    return true;
            }

            return false;
        }
        protected override bool DoSetupSequence()
        {
            base.DoSetupSequence();

            switch (m_enAction)
            {
                case TASK_ACTION.SCHEDULING:
                    return ActionScheduling();

                case TASK_ACTION.PICK:
                case TASK_ACTION.MANUAL_PICK:
                    return ActionPick(true);

                case TASK_ACTION.PLACE:
                case TASK_ACTION.MANUAL_PLACE:
                    return ActionPlace(true);

                //case TASK_ACTION.GEM_SIMUL:
                    //return ActionGemSimul();

                default:
                    return false;
            }
        }
        protected override bool DoExecutingSequence()
        {
            base.DoExecutingSequence();

            switch (m_enAction)
            {
                case TASK_ACTION.SCHEDULING:
                    return ActionScheduling();

                case TASK_ACTION.PICK:
                    return ActionPick();

                case TASK_ACTION.PLACE:
                    return ActionPlace();

                default:
                    return false;
            }
        }
        protected override void DoAlwaysSequence()
        {
            if (_robotManager != null)
            {
                if (_prevInitializationState != _robotManager.GetInitializationState(RobotIndex))
                {
                    _prevInitializationState = _robotManager.GetInitializationState(RobotIndex);

                    if (false == _prevInitializationState)
                    {
                        GenerateAlarm((int)EN_ALARM.ATM_ROBOT_IS_NOT_INITIALIZED);
                    }
                }
            }

        }

        /// <summary>
        /// 2020.06.02 by yjlee [ADD] Code the sequence for exit.
        /// - Before returning 'true', it will be called continuously.
        /// </summary>
        protected override bool DoExitSequence()
        {
            switch (m_nSeqNum)
            {
                case (int)STEP_EXIT.START:
                    _robotSchedulerManager.RemoveCurrentManualWorkingInfo(RobotIndex);
                    _processGroup.ResetSignalsAll();
                   
                    if (_taskOperator.IsDryRunMode())
                    {
                        _substrateManager.RemoveSubstratesAtRobot(RobotName);
                    }

                    m_nSeqNum = (int)STEP_EXIT.END;
                    break;

                case (int)STEP_EXIT.END:
                    return true;
            }

            return false;
        }
        #endregion /sequence

        #region dynamic link
        /// <summary>
        /// 2020.06.02 by yjlee [ADD] Check whether a sequence is existent or not.
        /// </summary>
        protected override bool UpdateActionName(string actionName)
        {
            if (false == m_mapppingForAction.ContainsKey(actionName))
            {
                m_enAction = TASK_ACTION.STOP;
                return false;
            }

            m_enAction = m_mapppingForAction[actionName];
            return true;
        }
        /// <summary>
        /// Action pre condition과 flow post condition을 설정함.
        /// </summary>
        public override void InitializeActionCondition()
        {
        }
        protected override void DoSetupPrecondition()
        {
            base.DoSetupPrecondition();
        }
        protected override void DoSetupPostcondition()
        {
            base.DoSetupPostcondition();
        }
        protected override void DoExecutingPrecondition()
        {
            base.DoExecutingPrecondition();
        }
        protected override void DoExecutingPostcondition()
        {
            base.DoExecutingPostcondition();
        }
        #endregion /dynamic link

        #region sub sequence
        #endregion /sub sequence

        protected override bool CheckExternalDeviceStateIsIdle()
        {
            if (_robotManager == null)
                return false;

            return _robotManager.GetInitializationState(RobotIndex);
        }
        #endregion /inherit

        #region <Abstract Methods>

        #region <Input/Output>
        protected abstract bool GetBusySignalIndex(int index, ref int indexOfDigital);
        protected abstract bool GetAlarmSignalIndex(int index, ref int indexOfDigital);
        protected abstract bool GetServoSignalIndex(int index, ref int indexOfDigital);
        #endregion </Input/Output>

        #region <Scenario>
        protected abstract void InitScenarioInfoPick();
        protected abstract CommandResults UpdateParamToBeforePick();
        protected abstract CommandResults ExecuteScenarioToBeforePick();
        protected abstract CommandResults UpdateParamToAfterPick();
        protected abstract CommandResults ExecuteScenarioToAfterPick();
        protected abstract void InitScenarioInfoPlace();
        protected abstract CommandResults UpdateParamToBeforePlace();
        protected abstract CommandResults ExecuteScenarioToBeforePlace();
        protected abstract CommandResults UpdateParamToAfterPlace();
        protected abstract CommandResults ExecuteScenarioToAfterPlace();        
        #endregion </Scenario>

        #region <Material Handling With Process Module>

        #region <Recovery Data>
        protected abstract void UpdateRecoveryDataBeforePick();
        protected abstract void UpdateRecoveryDataAfterPick();
        protected abstract void UpdateRecoveryDataBeforePlace();
        protected abstract void UpdateRecoveryDataAfterPlace();
        #endregion </Recovery Data>

        #region <Init>
        protected abstract void InitMaterialHandlingInterface();
        #endregion <Init>

        #region <Loading>
        protected abstract CommandResults IsApproachLoadingPrepared();
        protected abstract CommandResults IsApproachLoadingCompleted();
        protected abstract CommandResults IsLoadingPrepared();
        protected abstract CommandResults IsLoadingCompleted();
        #endregion </Loading>

        #region <Unloading>
        protected abstract CommandResults IsApproachUnloadingPrepared();
        protected abstract CommandResults IsApproachUnloadingCompleted();
        protected abstract CommandResults IsUnloadingPrepared();
        protected abstract CommandResults IsUnloadingCompleted();
        #endregion </Unloading>

        #region <Handling Location Info>
        protected abstract string GetProcessModuleLocationName(ProcessModuleLocation location);
        #endregion </Handling Location Info>

        #endregion </Material Handling With Process Module>

        #endregion </Abstract Methods>

        #region action

        #region auto
        protected override bool ProcessBeforeError()
        {
            if (_taskOperator.GetTaskAlarmData(GetTaskName(), out _, out int step))
            {
                var message = $"Error Occurred : {step}";
                Logger.WriteAlarmLog(message);
            }

            return base.ProcessBeforeError();
        }

        protected override bool ProcessBeforeWarning()
        {
            if (_taskOperator.GetTaskAlarmData(GetTaskName(), out _, out int step))
            {
                var message = $"Warning Occurred : {step}";
                Logger.WriteAlarmLog(message);
            }

            return base.ProcessBeforeWarning();
        }

        protected virtual bool ActionScheduling()
        {
            switch (m_nSeqNum)
            {
                case (int)STEP_SCHEDULING.START:
                    {
                        Logger.WriteActionStartLog(m_enAction.ToString(), string.Empty);
                        InitMaterialHandlingInterface();
                        _robotSchedulerManager.InitSchedulers(RobotIndex);
                    }
                    m_nSeqNum = (int)STEP_SCHEDULING.CHECK_READY;
                    break;
                case (int)STEP_SCHEDULING.CHECK_READY:
                    {
                        if (_taskOperator.IsFinishingMode())
                        {
                            Logger.WriteActionEndLog(m_enAction.ToString(), "Stopped");
                            m_nSeqNum = (int)STEP_SCHEDULING.END;
                            break;
                        }

                        var currentPort = _robotSchedulerManager.ExecuteSchedulers(RobotIndex);
                        switch (currentPort)
                        {
                            case RobotScheduleType.Selection:
                                {
                                    ++m_nSeqNum;
                                }
                                break;
                            case RobotScheduleType.Pick:
                            case RobotScheduleType.Place:
                                {
                                    if (false == IsPortNotChanged(currentPort))
                                    {
                                        Logger.WriteActionEndLog(m_enAction.ToString(), $"ResultPortStatus:{currentPort}");
                                    }

                                    SetRobotPortState(currentPort);

                                    m_nSeqNum = (int)STEP_SCHEDULING.END;
                                }
                                break;
                            default:
                                break;
                        }
                    }
                    break;
                case (int)STEP_SCHEDULING.CHECK_READY + 1:
                    {
                        --m_nSeqNum;
                    }
                    break;

                case (int)STEP_SCHEDULING.END:
                    return true;

                default:
                    return false;
            }

            return false;
        }
        protected virtual bool ActionPick(bool manualAction = false)
        {
            switch (m_nSeqNum)
            {
                case (int)STEP_PICKING.START:
                    {
                        InitScenarioInfoPick();
                        UpdateRecoveryDataBeforePick();
                        InitMaterialHandlingInterface();
                        Logger.WriteActionStartLog(m_enAction.ToString(), string.Empty);
                        m_nSeqNum = (int)STEP_PICKING.CHECK_READY;
                    }
                    break;

                case (int)STEP_PICKING.CHECK_READY:
                    {
                        // 암의 Sub 정보와 Presence의 동기화 필요?
                        List<RobotArmTypes> availableArms = new List<RobotArmTypes>();
                        if (false == _taskOperator.IsDryRunMode())
                        {
                            if (false == _robotManager.GetAvailableArm(RobotIndex, true, ref availableArms))
                            {
                                GenerateAlarm((int)EN_ALARM.ATM_ROBOT_HAS_NO_AVAILABLE_ARM);
                                m_nSeqNum = (int)STEP_PICKING.END;
                                break;
                            }
                        }
                        _workingInfo = new RobotWorkingInfo();
                        _robotManager.InitAtmRobotAction(RobotIndex);
                        ++m_nSeqNum;
                    }
                    break;

                case (int)STEP_PICKING.CHECK_READY + 1:
                    {
                        if (_taskOperator.IsFinishingMode())
                        {
                            m_nSeqNum = (int)STEP_PICKING.END;
                            break;
                        }

                        m_nSeqNum = (int)STEP_PICKING.EXECUTE_SCENARIO_BEFORE_PICK;
                    }
                    break;

                #region <Execute scenario before pick>
                case (int)STEP_PICKING.EXECUTE_SCENARIO_BEFORE_PICK:
                    {
                        var result = UpdateParamToBeforePick();
                        switch (result.CommandResult)
                        {
                            case CommandResult.Proceed:
                                break;
                            case CommandResult.Completed:
                                {
                                    ++m_nSeqNum;
                                }
                                break;
                            case CommandResult.Skipped:
                                {
                                    m_nSeqNum = (int)STEP_PICKING.PICK;
                                }
                                break;
                            case CommandResult.Timeout:
                            case CommandResult.Error:
                            case CommandResult.Invalid:
                                {
                                    GenerateAlarm((int)EN_ALARM.ATM_ROBOT_SECSGEM_ERROR_BEFORE_PICK, result.Description);
                                    m_nSeqNum = (int)STEP_PICKING.END;
                                }
                                break;
                            default:
                                break;
                        }
                    }
                    break;

                case (int)STEP_PICKING.EXECUTE_SCENARIO_BEFORE_PICK + 1:
                    {
                        var result = ExecuteScenarioToBeforePick();
                        switch (result.CommandResult)
                        {
                            case CommandResult.Proceed:
                                break;
                            case CommandResult.Completed:
                            case CommandResult.Skipped:
                                {
                                    --m_nSeqNum;
                                }
                                break;
                            case CommandResult.Timeout:
                            case CommandResult.Error:
                            case CommandResult.Invalid:
                                {
                                    GenerateAlarm((int)EN_ALARM.ATM_ROBOT_SECSGEM_ERROR_BEFORE_PICK, result.Description);
                                    m_nSeqNum = (int)STEP_PICKING.END;
                                }
                                break;
                            default:
                                break;
                        }
                    }
                    break;

                case (int)STEP_PICKING.EXECUTE_SCENARIO_BEFORE_PICK + 2:
                    --m_nSeqNum;
                    break;
                #endregion </Execute scenario before pick>

                case (int)STEP_PICKING.PICK:
                    {
                        string description = string.Empty;
                        if (false == GetWorkingInformation(manualAction, ref _workingInfo, ref description))
                        {
                            GenerateAlarm((int)EN_ALARM.ATM_ROBOT_CANNOT_GET_WORKING_INFO);
                            m_nSeqNum = (int)STEP_PICKING.END;
                            break;
                        }
                        else
                        {
                            if (_workingInfo.LocationType == ModuleType.ProcessModule)
                            {
                                m_nSeqNum = (int)STEP_PICKING.PREPARE_APPROACH_UNLOADING;
                                break;
                            }
                        }
                        
                        var result = _robotManager.Pick(RobotIndex, _workingInfo.ActionArm, _workingInfo.LocationId, false, _workingInfo.SubstrateKey);
                        switch (result.CommandResult)
                        {
                            case CommandResult.Completed:
                            case CommandResult.Skipped:
                                {
                                    if (result.CommandResult == CommandResult.Completed)
                                    {
                                        m_nSeqNum = (int)STEP_PICKING.EXECUTE_SCENARIO_AFTER_PICK;
                                    }
                                    else
                                    {
                                        m_nSeqNum = (int)STEP_PICKING.END;
                                    }
                                }
                                break;

                            case CommandResult.Timeout:
                            case CommandResult.Error:
                            case CommandResult.Invalid:
                                {
                                    // 2025.07.09. jhlim [ADD] 픽업 실패 시 장소 정보 추가
                                    GenerateAlarm((int)EN_ALARM.ATM_ROBOT_PICKING_ACTION_FAILED, GetLocationInfoAtMaterialHandling(_workingInfo.LocationId, result.ActionName, result.Description));
                                    // 2025.07.09. jhlim [END]
                                    m_nSeqNum = (int)STEP_PICKING.END;
                                }
                                break;

                            default:
                                ++m_nSeqNum;
                                break;
                        }
                    }
                    break;

                case (int)STEP_PICKING.PICK + 1:
                    --m_nSeqNum;
                    break;

                #region <Approach Unloading>
                case (int)STEP_PICKING.PREPARE_APPROACH_UNLOADING:
                    {
                        string description = string.Empty;
                        if (false == GetWorkingInformation(manualAction, ref _workingInfo, ref description))
                        {
                            GenerateAlarm((int)EN_ALARM.ATM_ROBOT_CANNOT_GET_WORKING_INFO);
                            m_nSeqNum = (int)STEP_PICKING.END;
                            break;
                        }

                        var result = IsApproachUnloadingPrepared();
                        switch (result.CommandResult)
                        {
                            case CommandResult.Proceed:
                                break;
                            case CommandResult.Completed:
                                {
                                    m_nSeqNum = (int)STEP_PICKING.ACTION_APPROACH_UNLOADING;
                                }
                                break;
                            case CommandResult.Skipped:
                                {
                                    m_nSeqNum = (int)STEP_PICKING.END;
                                }
                                break;
                            default:
                                {
                                    GenerateAlarm(result.AlarmCode, result.Description);
                                    m_nSeqNum = (int)STEP_PICKING.END;
                                }
                                break;
                        }
                    }
                    break;
                case (int)STEP_PICKING.ACTION_APPROACH_UNLOADING:
                    {
                        string description = string.Empty;
                        if (false == GetWorkingInformation(manualAction, ref _workingInfo, ref description))
                        {
                            GenerateAlarm((int)EN_ALARM.ATM_ROBOT_CANNOT_GET_WORKING_INFO);
                            m_nSeqNum = (int)STEP_PICKING.END;
                            break;
                        }
                        var result = _robotManager.ApproachForPick(RobotIndex, _workingInfo.ActionArm, _workingInfo.LocationId);
                        switch (result.CommandResult)
                        {
                            case CommandResult.Proceed:
                                ++m_nSeqNum;
                                break;
                            case CommandResult.Completed:
                                m_nSeqNum = (int)STEP_PICKING.APPROACH_UNLOADING_COMPLETED;
                                break;
                            case CommandResult.Skipped:
                                {
                                    m_nSeqNum = (int)STEP_PICKING.END;
                                }
                                break;
                            default:
                                {
                                    GenerateAlarm((int)EN_ALARM.ATM_ROBOT_APPROACH_UNLOADING_FAILED);
                                    m_nSeqNum = (int)STEP_PICKING.END;
                                }
                                break;
                        }
                    }
                    break;
                case (int)STEP_PICKING.ACTION_APPROACH_UNLOADING + 1:
                    --m_nSeqNum;
                    break;
                case (int)STEP_PICKING.APPROACH_UNLOADING_COMPLETED:
                    {
                        string description = string.Empty;
                        if (false == GetWorkingInformation(manualAction, ref _workingInfo, ref description))
                        {
                            GenerateAlarm((int)EN_ALARM.ATM_ROBOT_CANNOT_GET_WORKING_INFO);
                            m_nSeqNum = (int)STEP_PICKING.END;
                            break;
                        }

                        var result = IsApproachUnloadingCompleted();
                        switch (result.CommandResult)
                        {
                            case CommandResult.Proceed:
                                break;
                            case CommandResult.Completed:
                                m_nSeqNum = (int)STEP_PICKING.PREPARE_ACTION_UNLOADING;
                                break;
                            case CommandResult.Skipped:
                                {
                                    m_nSeqNum = (int)STEP_PICKING.END;
                                }
                                break;
                            default:
                                {
                                    GenerateAlarm(result.AlarmCode, result.Description);
                                    m_nSeqNum = (int)STEP_PICKING.END;
                                }
                                break;
                        }
                    }
                    break;
                #endregion </Approach Unloading>

                #region <Unloading>
                case (int)STEP_PICKING.PREPARE_ACTION_UNLOADING:
                    {
                        string description = string.Empty;
                        if (false == GetWorkingInformation(manualAction, ref _workingInfo, ref description))
                        {
                            GenerateAlarm((int)EN_ALARM.ATM_ROBOT_CANNOT_GET_WORKING_INFO);
                            m_nSeqNum = (int)STEP_PICKING.END;
                            break;
                        }

                        var result = IsUnloadingPrepared();
                        switch (result.CommandResult)
                        {
                            case CommandResult.Proceed:
                                break;
                            case CommandResult.Completed:
                                {
                                    m_nSeqNum = (int)STEP_PICKING.ACTION_UNLOADING;
                                }
                                break;
                            case CommandResult.Skipped:
                                {
                                    m_nSeqNum = (int)STEP_PICKING.END;
                                }
                                break;
                            default:
                                {
                                    GenerateAlarm(result.AlarmCode, result.Description);
                                    m_nSeqNum = (int)STEP_PICKING.END;
                                }
                                break;
                        }
                    }
                    break;
                case (int)STEP_PICKING.ACTION_UNLOADING:
                    {
                        string description = string.Empty;
                        if (false == GetWorkingInformation(manualAction, ref _workingInfo, ref description))
                        {
                            GenerateAlarm((int)EN_ALARM.ATM_ROBOT_CANNOT_GET_WORKING_INFO);
                            m_nSeqNum = (int)STEP_PICKING.END;
                            break;
                        }
                        var result = _robotManager.Pick(RobotIndex, _workingInfo.ActionArm, _workingInfo.LocationId, false, _workingInfo.SubstrateKey);
                        switch (result.CommandResult)
                        {
                            case CommandResult.Proceed:
                                ++m_nSeqNum;
                                break;
                            case CommandResult.Completed:
                                m_nSeqNum = (int)STEP_PICKING.ACTION_UNLOADING_COMPLETED;
                                break;
                            case CommandResult.Skipped:
                                {
                                    m_nSeqNum = (int)STEP_PICKING.END;
                                }
                                break;
                            default:
                                {
                                    // 2025.07.09. jhlim [ADD] 픽업 실패 시 장소 정보 추가
                                    GenerateAlarm((int)EN_ALARM.ATM_ROBOT_UNLOADING_FAILED, GetLocationInfoAtMaterialHandling(_workingInfo.LocationId, result.ActionName, result.Description));
                                    // 2025.07.09. jhlim [END]
                                    m_nSeqNum = (int)STEP_PICKING.END;
                                }
                                break;
                        }
                    }
                    break;
                case (int)STEP_PICKING.ACTION_UNLOADING + 1:
                    --m_nSeqNum;
                    break;
                case (int)STEP_PICKING.ACTION_UNLOADING_COMPLETED:
                    {
                        string description = string.Empty;
                        if (false == GetWorkingInformation(manualAction, ref _workingInfo, ref description))
                        {
                            GenerateAlarm((int)EN_ALARM.ATM_ROBOT_CANNOT_GET_WORKING_INFO);
                            m_nSeqNum = (int)STEP_PICKING.END;
                            break;
                        }
                        var result = IsUnloadingCompleted();
                        switch (result.CommandResult)
                        {
                            case CommandResult.Proceed:
                                break;
                            case CommandResult.Completed:
                                {
                                    RobotTicks.SetTickCount(5000);
                                    ++m_nSeqNum;
                                }
                                break;
                            case CommandResult.Skipped:
                                {
                                    m_nSeqNum = (int)STEP_PICKING.END;
                                }
                                break;
                            default:
                                {
                                    GenerateAlarm(result.AlarmCode, result.Description);
                                    m_nSeqNum = (int)STEP_PICKING.END;
                                }
                                break;
                        }
                    }
                    break;
                case (int)STEP_PICKING.ACTION_UNLOADING_COMPLETED + 1:
                    {
                        if (RobotTicks.IsTickOver(true))
                        {
                            GenerateAlarm((int)EN_ALARM.INTERFACE_AFTER_UNLOADING_SMEMA_TIMEOUT);
                            m_nSeqNum = (int)STEP_PICKING.END;
                            break;
                        }

                        string description = string.Empty;
                        if (false == GetWorkingInformation(manualAction, ref _workingInfo, ref description))
                        {
                            GenerateAlarm((int)EN_ALARM.ATM_ROBOT_CANNOT_GET_WORKING_INFO);
                            m_nSeqNum = (int)STEP_PICKING.END;
                            break;
                        }
                        int pmIndex = _processGroup.GetProcessModuleIndexByEntry(_workingInfo.LocationId);
                        if (false == _processGroup.IsUnloadingRequested(pmIndex, _workingInfo.LocationId) || _taskOperator.IsDryRunOrSimulationMode())
                        {
                            m_nSeqNum = (int)STEP_PICKING.EXECUTE_SCENARIO_AFTER_PICK;
                        }
                        else
                        {
                            ++m_nSeqNum;
                        }
                    }
                    break;
                case (int)STEP_PICKING.ACTION_UNLOADING_COMPLETED + 2:
                    {
                        --m_nSeqNum;
                    }
                    break;
                #endregion </Unloading>

                #region <Execute scenario after pick>
                case (int)STEP_PICKING.EXECUTE_SCENARIO_AFTER_PICK:
                    {
                        var result = UpdateParamToAfterPick();
                        switch (result.CommandResult)
                        {
                            case CommandResult.Proceed:
                                break;
                            case CommandResult.Completed:
                                {
                                    ++m_nSeqNum;
                                }
                                break;
                            case CommandResult.Skipped:
                                {
                                    m_nSeqNum = (int)STEP_PICKING.UPDATE_LINK;
                                }
                                break;
                            case CommandResult.Timeout:
                            case CommandResult.Error:
                            case CommandResult.Invalid:
                                {
                                    GenerateAlarm((int)EN_ALARM.ATM_ROBOT_SECSGEM_ERROR_BEFORE_PICK, result.Description);
                                    m_nSeqNum = (int)STEP_PICKING.END;
                                }
                                break;
                            default:
                                break;
                        }
                    }
                    break;

                case (int)STEP_PICKING.EXECUTE_SCENARIO_AFTER_PICK + 1:
                    {
                        var result = ExecuteScenarioToAfterPick();
                        switch (result.CommandResult)
                        {
                            case CommandResult.Proceed:
                                break;
                            case CommandResult.Completed:
                            case CommandResult.Skipped:
                                {
                                    --m_nSeqNum;
                                }
                                break;
                            case CommandResult.Timeout:
                            case CommandResult.Error:
                            case CommandResult.Invalid:
                                {
                                    GenerateAlarm((int)EN_ALARM.ATM_ROBOT_SECSGEM_ERROR_AFTER_PICK, result.Description);
                                    m_nSeqNum = (int)STEP_PICKING.END;
                                }
                                break;
                            default:
                                break;
                        }
                    }
                    break;

                case (int)STEP_PICKING.EXECUTE_SCENARIO_AFTER_PICK + 2:
                    --m_nSeqNum;
                    break;
                #endregion </Execute scenario after pick>

                case (int)STEP_PICKING.UPDATE_LINK:
                    {
                        string description = string.Empty;
                        if (false == GetWorkingInformation(manualAction, ref _workingInfo, ref description))
                        {
                            GenerateAlarm((int)EN_ALARM.ATM_ROBOT_CANNOT_GET_WORKING_INFO);
                            m_nSeqNum = (int)STEP_PICKING.END;
                            break;
                        }

                        if (false == _substrateManager.GetSubstrateAtRobot(RobotName, _workingInfo.ActionArm, out var substrate))
                        {
                            GenerateAlarm((int)EN_ALARM.ATM_ROBOT_CANNOT_GET_WORKING_INFO);
                            m_nSeqNum = (int)STEP_PICKING.END;
                            break;
                        }
                        else
                        {
                            if (substrate.SourcePortId <= 0 || substrate.SourceSlot < 0)
                            {
                                GenerateAlarm((int)EN_ALARM.ATM_ROBOT_CANNOT_GET_WORKING_INFO);
                                m_nSeqNum = (int)STEP_PICKING.END;
                                break;
                            }
                        }
                        UpdateRecoveryDataAfterPick();
                        //SetRobotPortState(RobotScheduleType.Selection);
                        m_nSeqNum = (int)STEP_PICKING.END;
                    }
                    break;

                case (int)STEP_PICKING.END:
                    {
                        Logger.WriteActionEndLog(m_enAction.ToString(), string.Empty);

                        // 2024.12.29. jhlim [ADD] 위치 변경 -> 스킵 시 상태를 변경하여 다시 스케쥴링을 하기 위함
                        SetRobotPortState(RobotScheduleType.Selection);
                        // 2024.12.29. jhlim [MOD]
                        string description = string.Empty;
                        if (GetWorkingInformation(manualAction, ref _workingInfo, ref description))
                        {
                            int pmIndex = _processGroup.GetProcessModuleIndexByEntry(_workingInfo.LocationId);
                            _processGroup.SetUnloadingSignal(pmIndex, _workingInfo.LocationId, false);
                        }                       

                        if (manualAction)
                        {
                            _robotSchedulerManager.RemoveCurrentManualWorkingInfo(RobotIndex);
                        }
                    }
                    return true;

                default:
                    break;
            }

            return false;
        }
        protected virtual bool ActionPlace(bool manualAction = false)
        {
            switch (m_nSeqNum)
            {
                case (int)STEP_PLACING.START:
                    {
                        InitScenarioInfoPlace();
                        UpdateRecoveryDataBeforePlace();
                        InitMaterialHandlingInterface();
                        Logger.WriteActionStartLog(m_enAction.ToString(), string.Empty);
                        m_nSeqNum = (int)STEP_PLACING.CHECK_READY;
                    }
                    break;

                case (int)STEP_PLACING.CHECK_READY:
                    {
                        // 암의 Sub 정보와 Presence의 동기화 필요?
                        List<RobotArmTypes> availableArms = new List<RobotArmTypes>();
                        if (false == _taskOperator.IsDryRunMode())
                        {
                            if (false == _robotManager.GetAvailableArm(RobotIndex, false, ref availableArms))
                            {
                                GenerateAlarm((int)EN_ALARM.ATM_ROBOT_HAS_NO_AVAILABLE_ARM);
                                m_nSeqNum = (int)STEP_PLACING.END;
                                break;
                            }
                        }
                        _workingInfo = new RobotWorkingInfo();
                        _robotManager.InitAtmRobotAction(RobotIndex);
                        ++m_nSeqNum;
                    }
                    break;

                case (int)STEP_PLACING.CHECK_READY + 1:
                    {
                        if (_taskOperator.IsFinishingMode())
                        {
                            m_nSeqNum = (int)STEP_PLACING.END;
                            break;
                        }

                        m_nSeqNum = (int)STEP_PLACING.EXECUTE_SCENARIO_BEFORE_PLACE;
                    }
                    break;

                #region <Execute scenario before place>
                case (int)STEP_PLACING.EXECUTE_SCENARIO_BEFORE_PLACE:
                    {
                        var result = UpdateParamToBeforePlace();
                        switch (result.CommandResult)
                        {
                            case CommandResult.Proceed:
                                break;
                            case CommandResult.Completed:
                                {
                                    ++m_nSeqNum;
                                }
                                break;
                            case CommandResult.Skipped:
                                {
                                    m_nSeqNum = (int)STEP_PLACING.PLACE;
                                }
                                break;
                            case CommandResult.Timeout:
                            case CommandResult.Error:
                            case CommandResult.Invalid:
                                {
                                    GenerateAlarm((int)EN_ALARM.ATM_ROBOT_SECSGEM_ERROR_BEFORE_PICK, result.Description);
                                    m_nSeqNum = (int)STEP_PICKING.END;
                                }
                                break;
                            default:
                                break;
                        }
                    }
                    break;

                case (int)STEP_PLACING.EXECUTE_SCENARIO_BEFORE_PLACE + 1:
                    {
                        var result = ExecuteScenarioToBeforePlace();
                        switch (result.CommandResult)
                        {
                            case CommandResult.Proceed:
                                break;
                            case CommandResult.Completed:
                            case CommandResult.Skipped:
                                {
                                    //if (_taskOperator.IsFinishingMode())
                                    //{
                                    //    m_nSeqNum = (int)STEP_PLACING.END;
                                    //    break;
                                    //}

                                    --m_nSeqNum;
                                }
                                break;
                            case CommandResult.Timeout:
                            case CommandResult.Error:
                            case CommandResult.Invalid:
                                {
                                    GenerateAlarm((int)EN_ALARM.ATM_ROBOT_SECSGEM_ERROR_BEFORE_PLACE, result.Description);
                                    m_nSeqNum = (int)STEP_PLACING.END;
                                }
                                break;
                            default:
                                break;
                        }
                    }
                    break;

                case (int)STEP_PLACING.EXECUTE_SCENARIO_BEFORE_PLACE + 2:
                    --m_nSeqNum;
                    break;
                #endregion </Execute scenario before place>

                case (int)STEP_PLACING.PLACE:
                    {
                        string description = string.Empty;
                        if (false == GetWorkingInformation(manualAction, ref _workingInfo, ref description))
                        {
                            GenerateAlarm((int)EN_ALARM.ATM_ROBOT_CANNOT_GET_WORKING_INFO);
                            m_nSeqNum = (int)STEP_PLACING.END;
                            break;
                        }
                        else
                        {
                            if (_workingInfo.LocationType == ModuleType.ProcessModule)
                            {
                                m_nSeqNum = (int)STEP_PLACING.PREPARE_APPROACH_LOADING;
                                break;
                            }
                        }

                        var result = _robotManager.Place(RobotIndex, _workingInfo.ActionArm, _workingInfo.LocationId, false, _workingInfo.SubstrateKey);
                        switch (result.CommandResult)
                        {
                            case CommandResult.Completed:
                            case CommandResult.Skipped:
                                {
                                    m_nSeqNum = (int)STEP_PLACING.EXECUTE_SCENARIO_AFTER_PLACE;
                                }
                                break;

                            case CommandResult.Timeout:
                            case CommandResult.Error:
                            case CommandResult.Invalid:
                                {
                                    // 2025.07.09. jhlim [ADD] 픽업 실패 시 장소 정보 추가
                                    GenerateAlarm((int)EN_ALARM.ATM_ROBOT_PLACING_ACTION_FAILED, GetLocationInfoAtMaterialHandling(_workingInfo.LocationId, result.ActionName, result.Description));
                                    // 2025.07.09. jhlim [END]
                                    m_nSeqNum = (int)STEP_PLACING.END;
                                }
                                break;

                            default:
                                ++m_nSeqNum;
                                break;
                        }
                    }
                    break;

                case (int)STEP_PLACING.PLACE + 1:
                    {
                        //if (_taskOperator.IsFinishingMode())
                        //{
                        //    m_nSeqNum = (int)STEP_INITIALIZE.END;
                        //    break;
                        //}
                    }
                    --m_nSeqNum;
                    break;

                #region <Approach Loading>
                case (int)STEP_PLACING.PREPARE_APPROACH_LOADING:
                    {
                        string description = string.Empty;
                        if (false == GetWorkingInformation(manualAction, ref _workingInfo, ref description))
                        {
                            GenerateAlarm((int)EN_ALARM.ATM_ROBOT_CANNOT_GET_WORKING_INFO);
                            m_nSeqNum = (int)STEP_PLACING.END;
                            break;
                        }

                        var result = IsApproachLoadingPrepared();
                        switch (result.CommandResult)
                        {
                            case CommandResult.Proceed:
                                break;
                            case CommandResult.Completed:
                                {
                                    m_nSeqNum = (int)STEP_PLACING.ACTION_APPROACH_LOADING;
                                }
                                break;
                            case CommandResult.Skipped:
                                {
                                    m_nSeqNum = (int)STEP_PLACING.END;
                                }
                                break;
                            default:
                                {
                                    GenerateAlarm(result.AlarmCode, result.Description);
                                    m_nSeqNum = (int)STEP_PLACING.END;
                                }
                                break;
                        }
                    }
                    break;

                case (int)STEP_PLACING.ACTION_APPROACH_LOADING:
                    {
                        string description = string.Empty;
                        if (false == GetWorkingInformation(manualAction, ref _workingInfo, ref description))
                        {
                            GenerateAlarm((int)EN_ALARM.ATM_ROBOT_CANNOT_GET_WORKING_INFO);
                            m_nSeqNum = (int)STEP_PLACING.END;
                            break;
                        }

                        var result = _robotManager.ApproachForPlace(RobotIndex, _workingInfo.ActionArm, _workingInfo.LocationId);
                        switch (result.CommandResult)
                        {
                            case CommandResult.Proceed:
                                ++m_nSeqNum;
                                break;
                            case CommandResult.Completed:
                                m_nSeqNum = (int)STEP_PLACING.APPROACH_LOADING_COMPLETED;
                                break;
                            case CommandResult.Skipped:
                                {
                                    m_nSeqNum = (int)STEP_PLACING.END;
                                }
                                break;
                            default:
                                {
                                    GenerateAlarm((int)EN_ALARM.ATM_ROBOT_APPROACH_LOADING_FAILED);
                                    m_nSeqNum = (int)STEP_PLACING.END;
                                }
                                break;
                        }
                    }
                    break;

                case (int)STEP_PLACING.ACTION_APPROACH_LOADING + 1:
                    {
                        --m_nSeqNum;
                    }
                    break;

                case (int)STEP_PLACING.APPROACH_LOADING_COMPLETED:
                    {
                        string description = string.Empty;
                        if (false == GetWorkingInformation(manualAction, ref _workingInfo, ref description))
                        {
                            GenerateAlarm((int)EN_ALARM.ATM_ROBOT_CANNOT_GET_WORKING_INFO);
                            m_nSeqNum = (int)STEP_PLACING.END;
                            break;
                        }
                        var result = IsApproachLoadingCompleted();
                        switch (result.CommandResult)
                        {
                            case CommandResult.Proceed:
                                break;
                            case CommandResult.Completed:
                                m_nSeqNum = (int)STEP_PLACING.PREPARE_ACTION_LOADING;
                                break;
                            case CommandResult.Skipped:
                                {
                                    m_nSeqNum = (int)STEP_PLACING.END;
                                }
                                break;
                            default:
                                {
                                    GenerateAlarm(result.AlarmCode, result.Description);
                                    m_nSeqNum = (int)STEP_PLACING.END;
                                }
                                break;
                        }
                    }
                    break;
                #endregion </Approach Loading>

                #region <Loading>
                case (int)STEP_PLACING.PREPARE_ACTION_LOADING:
                    {
                        string description = string.Empty;
                        if (false == GetWorkingInformation(manualAction, ref _workingInfo, ref description))
                        {
                            GenerateAlarm((int)EN_ALARM.ATM_ROBOT_CANNOT_GET_WORKING_INFO);
                            m_nSeqNum = (int)STEP_PLACING.END;
                            break;
                        }

                        var result = IsLoadingPrepared();
                        switch (result.CommandResult)
                        {
                            case CommandResult.Proceed:
                                break;
                            case CommandResult.Completed:
                                {
                                    m_nSeqNum = (int)STEP_PLACING.ACTION_LOADING;
                                }
                                break;
                            case CommandResult.Skipped:
                                {
                                    m_nSeqNum = (int)STEP_PLACING.END;
                                }
                                break;
                            default:
                                {
                                    GenerateAlarm(result.AlarmCode, result.Description);
                                    m_nSeqNum = (int)STEP_PLACING.END;
                                }
                                break;
                        }
                    }
                    break;

                case (int)STEP_PLACING.ACTION_LOADING:
                    {
                        string description = string.Empty;
                        if (false == GetWorkingInformation(manualAction, ref _workingInfo, ref description))
                        {
                            GenerateAlarm((int)EN_ALARM.ATM_ROBOT_CANNOT_GET_WORKING_INFO);
                            m_nSeqNum = (int)STEP_PLACING.END;
                            break;
                        }

                        var result = _robotManager.Place(RobotIndex, _workingInfo.ActionArm, _workingInfo.LocationId, false, _workingInfo.SubstrateKey);
                        switch (result.CommandResult)
                        {
                            case CommandResult.Proceed:
                                ++m_nSeqNum;
                                break;
                            case CommandResult.Completed:
                                m_nSeqNum = (int)STEP_PLACING.ACTION_LOADING_COMPLETED;
                                break;
                            case CommandResult.Skipped:
                                {
                                    m_nSeqNum = (int)STEP_PLACING.END;
                                }
                                break;
                            default:
                                {
                                    // 2025.07.09. jhlim [ADD] 픽업 실패 시 장소 정보 추가
                                    GenerateAlarm((int)EN_ALARM.ATM_ROBOT_LOADING_FAILED, GetLocationInfoAtMaterialHandling(_workingInfo.LocationId, result.ActionName, result.Description));
                                    // 2025.07.09. jhlim [END]
                                    m_nSeqNum = (int)STEP_PLACING.END;
                                }
                                break;
                        }
                    }
                    break;

                case (int)STEP_PLACING.ACTION_LOADING + 1:
                    {
                        --m_nSeqNum;
                    }
                    break;

                case (int)STEP_PLACING.ACTION_LOADING_COMPLETED:
                    {
                        string description = string.Empty;
                        if (false == GetWorkingInformation(manualAction, ref _workingInfo, ref description))
                        {
                            GenerateAlarm((int)EN_ALARM.ATM_ROBOT_CANNOT_GET_WORKING_INFO);
                            m_nSeqNum = (int)STEP_PLACING.END;
                            break;
                        }

                        var result = IsLoadingCompleted();
                        switch (result.CommandResult)
                        {
                            case CommandResult.Proceed:
                                break;
                            case CommandResult.Completed:
                                {
                                    RobotTicks.SetTickCount(5000);
                                    ++m_nSeqNum;
                                }
                                break;
                            case CommandResult.Skipped:
                                {
                                    if (IsSimulation())
                                    {
                                        RobotTicks.SetTickCount(5000);
                                        ++m_nSeqNum;
                                    }
                                    else
                                    {
                                        m_nSeqNum = (int)STEP_PLACING.END;
                                    }
                                }
                                break;
                            default:
                                {
                                    GenerateAlarm(result.AlarmCode, result.Description);
                                    m_nSeqNum = (int)STEP_PLACING.END;
                                }
                                break;
                        }
                    }
                    break;
                case (int)STEP_PLACING.ACTION_LOADING_COMPLETED + 1:
                    {
                        if (RobotTicks.IsTickOver(true))
                        {
                            GenerateAlarm((int)EN_ALARM.INTERFACE_AFTER_LOADING_SMEMA_TIMEOUT);
                            m_nSeqNum = (int)STEP_PLACING.END;
                            break;
                        }

                        string description = string.Empty;
                        if (false == GetWorkingInformation(manualAction, ref _workingInfo, ref description))
                        {
                            GenerateAlarm((int)EN_ALARM.ATM_ROBOT_CANNOT_GET_WORKING_INFO);
                            m_nSeqNum = (int)STEP_PLACING.END;
                            break;
                        }
                        int pmIndex = _processGroup.GetProcessModuleIndexByEntry(_workingInfo.LocationId);
                        if (false == _processGroup.IsLoadingRequested(pmIndex, _workingInfo.LocationId) || _taskOperator.IsDryRunOrSimulationMode())
                        {
                            m_nSeqNum = (int)STEP_PLACING.EXECUTE_SCENARIO_AFTER_PLACE;
                        }
                        else
                        {
                            ++m_nSeqNum;
                        }
                    }
                    break;
                case (int)STEP_PLACING.ACTION_LOADING_COMPLETED + 2:
                    {
                        --m_nSeqNum;
                    }
                    break;
                #endregion </Loading>

                #region <Execute scenario after place>
                case (int)STEP_PLACING.EXECUTE_SCENARIO_AFTER_PLACE:
                    {
                        var result = UpdateParamToAfterPlace();
                        switch (result.CommandResult)
                        {
                            case CommandResult.Proceed:
                                break;
                            case CommandResult.Completed:
                                {
                                    ++m_nSeqNum;
                                }
                                break;
                            case CommandResult.Skipped:
                                {
                                    m_nSeqNum = (int)STEP_PLACING.UPDATE_LINK;
                                }
                                break;
                            case CommandResult.Timeout:
                            case CommandResult.Error:
                            case CommandResult.Invalid:
                                {
                                    GenerateAlarm((int)EN_ALARM.ATM_ROBOT_SECSGEM_ERROR_BEFORE_PICK, result.Description);
                                    m_nSeqNum = (int)STEP_PICKING.END;
                                }
                                break;
                            default:
                                break;
                        }
                    }
                    break;

                case (int)STEP_PLACING.EXECUTE_SCENARIO_AFTER_PLACE + 1:
                    {
                        var result = ExecuteScenarioToAfterPlace();
                        switch (result.CommandResult)
                        {
                            case CommandResult.Proceed:
                                break;
                            case CommandResult.Completed:
                            case CommandResult.Skipped:
                                {
                                    --m_nSeqNum;
                                }
                                break;
                            case CommandResult.Timeout:
                            case CommandResult.Error:
                            case CommandResult.Invalid:
                                {
                                    GenerateAlarm((int)EN_ALARM.ATM_ROBOT_SECSGEM_ERROR_BEFORE_PLACE, result.Description);
                                    m_nSeqNum = (int)STEP_PLACING.END;
                                }
                                break;
                            default:
                                break;
                        }
                    }
                    break;

                case (int)STEP_PLACING.EXECUTE_SCENARIO_AFTER_PLACE + 2:
                    --m_nSeqNum;
                    break;
                #endregion </Execute scenario after place>

                case (int)STEP_PLACING.UPDATE_LINK:
                    {
                        UpdateRecoveryDataAfterPlace();
                        //SetRobotPortState(RobotScheduleType.Selection);
                        m_nSeqNum = (int)STEP_PLACING.END;
                    }
                    break;

                case (int)STEP_PLACING.END:
                    {
                        Logger.WriteActionEndLog(m_enAction.ToString(), string.Empty);
                        
                        // 2024.12.29. jhlim [ADD] 위치 변경 -> 스킵 시 상태를 변경하여 다시 스케쥴링을 하기 위함
                        SetRobotPortState(RobotScheduleType.Selection);
                        // 2024.12.29. jhlim [MOD]

                        string description = string.Empty;
                        if (GetWorkingInformation(manualAction, ref _workingInfo, ref description))
                        {
                            int pmIndex = _processGroup.GetProcessModuleIndexByEntry(_workingInfo.LocationId);
                            _processGroup.SetLoadingSignal(pmIndex, _workingInfo.LocationId, false);
                        }

                        if (manualAction)
                        {
                            _robotSchedulerManager.RemoveCurrentManualWorkingInfo(RobotIndex);
                        }
                    }
                    return true;

                default:
                    break;
            }

            return false;
        }
        protected bool GetWorkingInformation(bool manualAction, ref RobotWorkingInfo workingInfo, ref string description)
        {
            if (false == manualAction)
            {
                if (false == _robotSchedulerManager.GetWorkingInformation(RobotIndex, ref workingInfo))
                {
                    description = "Can't get working information";
                    return false;
                }
            }
            else
            {
                if (false == _robotSchedulerManager.GetManualWorkingInformation(RobotIndex, ref workingInfo))
                {
                    description = "Can't get working information";
                    return false;
                }
            }

            description = string.Empty;
            return true;
        }
        protected bool IsPortNotChanged(RobotScheduleType portType)
        {
            var currentPort = GetPortStatus(EN_PORT.ROBOT_STATE.ToString());
            var comparePort = _robotSchedulerManager.ConvertPortStatusFromRobotPortType(portType);

            return currentPort == comparePort;
        }
        protected void SetRobotPortState(RobotScheduleType portType)
        {
            DynamicLink_.EN_PORT_STATUS newPortStatus = _robotSchedulerManager.ConvertPortStatusFromRobotPortType(portType);
            if (false == IsPortNotChanged(portType))
            {
                SetPortStatus(EN_PORT.ROBOT_STATE.ToString(), newPortStatus);
            }
        }
        #endregion

        #region manual
        #endregion /manual

        #endregion /action

        #region <ETC>
        private bool CheckControllerConnectionStatus()
        {
            if (false == _robotManager.IsConnectedWithController(RobotIndex))
            {
                GenerateAlarm((int)EN_ALARM.ATM_ROBOT_CONTROLLER_NOT_CONNECTED);
                return false;
            }

            return true;
        }
        private string GetLocationInfoAtMaterialHandling(string locationId, string actionName, string description)
        {
            string detailedLocationInfo, resultMessage;
            if (false == LocationServer.FindLocationById(locationId, out var location) ||
                locationId == null)
            {
                detailedLocationInfo = "UnknownLocation";
            }
            else
            {
                switch (location)
                {
                    case LoadPortLocation lp:
                        {
                            detailedLocationInfo = $"Port {lp.PortId} / Slot {lp.Slot}";
                        }
                        break;
                    case ProcessModuleLocation pm:
                        {
                            detailedLocationInfo = GetProcessModuleLocationName(pm);
                        }
                        break;
                    case RobotLocation rb:
                        {
                            detailedLocationInfo = rb.Id;
                        }
                        break;

                    default:
                        {
                            detailedLocationInfo = string.Empty;
                        }
                        break;
                }
            }
            
            resultMessage = $"Location : {detailedLocationInfo}, Info : {actionName} - {description}";
            
            return resultMessage;
        }
        #endregion </ETC>

        #region pre/post condition
        #endregion /pre/post condition

        #region common method

        #endregion /common method

        #region enum

        #region action
        /// <summary>
        /// 2020.06.02 by yjlee [ADD] Enumerate the actions of the task.
        /// </summary>
        public enum TASK_ACTION
        {
            STOP = 0,

            // Auto
            SCHEDULING,
            PICK,
            PLACE,

            // Manual
            MANUAL_PICK,
            MANUAL_PLACE,

            // Gem Simul
            //GEM_SIMUL,
        }
        #endregion /action

        #region step
        private enum STEP_INITIALIZE
        {
            START = 0,
            CHECK_ALARM_STATUS = 100,
            PREPARE = 500,
            END = 10000,
        }
        private enum STEP_ENTRY
        {
            START = 0,
            PREPARE = 50,

            END = 10000,
        }
        private enum STEP_EXIT
        {
            START = 0,
            END = 10000,
        }
        private enum STEP_SCHEDULING
        {
            START = 0,

            CHECK_READY = 100,

            CHECK_PORT = 900,

            END = 10000,
        }
        private enum STEP_PICKING
        {
            START = 0,

            CHECK_READY = 50,

            EXECUTE_SCENARIO_BEFORE_PICK = 100,

            PICK = 200,

            PREPARE_APPROACH_UNLOADING = 300,
            ACTION_APPROACH_UNLOADING = 330,
            APPROACH_UNLOADING_COMPLETED = 350,

            PREPARE_ACTION_UNLOADING = 400,
            ACTION_UNLOADING = 430,
            ACTION_UNLOADING_COMPLETED = 440,

            EXECUTE_SCENARIO_AFTER_PICK = 800,

            UPDATE_LINK = 900,

            END = 10000,
        }
        private enum STEP_PLACING
        {
            START = 0,

            CHECK_READY = 50,

            EXECUTE_SCENARIO_BEFORE_PLACE = 100,

            PLACE = 200,

            PREPARE_APPROACH_LOADING = 300,
            ACTION_APPROACH_LOADING = 330,
            APPROACH_LOADING_COMPLETED = 350,

            PREPARE_ACTION_LOADING = 400,
            ACTION_LOADING = 430,
            ACTION_LOADING_COMPLETED = 440,

            EXECUTE_SCENARIO_AFTER_PLACE = 800,

            UPDATE_LINK = 900,

            END = 10000,
        }
        #endregion /step

        #endregion /enum
    }
}