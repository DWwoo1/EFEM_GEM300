using System;
using System.Collections.Generic;
using System.Collections.Concurrent;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Alarm_;

using TickCounter_;
using Define.DefineEnumProject.Task;
using Define.DefineEnumProject.Task.LoadPort;
using FrameOfSystem3.SECSGEM.Scenario;
using FrameOfSystem3.SECSGEM;
using FrameOfSystem3.SECSGEM.SecsGemSDK.Gem300;
using FrameOfSystem3.SECSGEM.DefineSecsGem;

using EFEM.Jobs.Binding;
using EFEM.Jobs.Manager;
using EFEM.Defines.Common;
using EFEM.Defines.LoadPort;
using EFEM.MaterialTracking;
using EFEM.Jobs.Domain;
using EFEM.Defines.Job;
using EFEM.CustomizedByProcessType.PWA500W;
using EFEM.Modules.LoadPort.Scheduler;
using EFEM.CustomizedByProcessType.PWA500Common;
using EFEM.Defines.CarrierManagement;

// ConfigTask에서 이 namespace를 가지고 클래스 타입을 가져오기 때문에 변경 불가
namespace FrameOfSystem3.Task
{
    class TaskLoadPortForPWA500W_300 : TaskLoadPort, ICarrierServiceCallback
    {
        #region <Constructors>
        public TaskLoadPortForPWA500W_300(int nIndexOfTask, string strTaskName)
            : base(nIndexOfTask, strTaskName, new TaskLoadPortRecovery500W(strTaskName, nIndexOfTask))
        {
            // 0번이 공테이프, 그 외에는 Core,

            int coreIndex = _loadPortManager.Count - PortId;

            _functionsForPWA500 = FunctionsForPWA500W_NRD_300.Instance;

            _recovery = _recoveryData as TaskLoadPortRecovery500W_300;
            _lotHistoryLog = LotHistoryLog.Instance;

            #region <AMHS - E84>
            _carrierMovementRelatedCommands = new Dictionary<string, TASK_ACTION>();
            _carrierMovementRelatedCommands.Add(TASK_ACTION.CARRIER_LOADING.ToString(), TASK_ACTION.CARRIER_LOADING);
            _carrierMovementRelatedCommands.Add(TASK_ACTION.CARRIER_UNLOADING.ToString(), TASK_ACTION.CARRIER_UNLOADING);
            //_carrierMovementRelatedCommands.Add(TASK_ACTION.CARRIER_UNLOADING_BEFORE_AMHS.ToString(), TASK_ACTION.CARRIER_UNLOADING_BEFORE_AMHS);
            _carrierMovementRelatedCommands.Add(TASK_ACTION.CHANGE_LOADPORT_LOADING_MODE.ToString(), TASK_ACTION.CHANGE_LOADPORT_LOADING_MODE);
            _carrierMovementRelatedCommands.Add(TASK_ACTION.CARRIER_CLAMPING.ToString(), TASK_ACTION.CARRIER_CLAMPING);
            _carrierMovementRelatedCommands.Add(TASK_ACTION.CARRIER_UNCLAMPING.ToString(), TASK_ACTION.CARRIER_UNCLAMPING);
            _carrierMovementRelatedCommands.Add(TASK_ACTION.CARRIER_DOCKING.ToString(), TASK_ACTION.CARRIER_DOCKING);
            _carrierMovementRelatedCommands.Add(TASK_ACTION.CARRIER_UNDOCKING.ToString(), TASK_ACTION.CARRIER_UNDOCKING);
            _carrierMovementRelatedCommands.Add(TASK_ACTION.CARRIER_OPENING.ToString(), TASK_ACTION.CARRIER_OPENING);
            _carrierMovementRelatedCommands.Add(TASK_ACTION.CARRIER_CLOSING.ToString(), TASK_ACTION.CARRIER_CLOSING);
            _carrierMovementRelatedCommands.Add(TASK_ACTION.INITIALIZE.ToString(), TASK_ACTION.INITIALIZE);
            #endregion </AMHS - E84>

            _lotHistoryLog.AddLogInfo(PortId, LoadPortName);

            var carrierService = _loadPortManager.GetCarrierService(LoadPortIndex);
            if (carrierService != null)
            {
                carrierService.RegisterCallback(LoadPortName, this);
            }
        }
        #endregion </Constructors>

        #region <Fields>

        private const int CarrierMaxCapacity = 25;

        private CommandResults _commandResult = new CommandResults("", CommandResult.Invalid);
        private static TaskLoadPortRecovery500W_300 _recovery;
        string _lotId = string.Empty;
        string _partId = string.Empty;
        string _stepSeq = string.Empty;
        string _lotType = string.Empty;
        string _lotQty = string.Empty;
        string _recipeId = string.Empty;

        private string _toWrite = string.Empty;
        //private StepsBeforeSendingCarrier _currentStepBeforeSendingCarrier;

        private static FunctionsForPWA500W_NRD_300 _functionsForPWA500 = null;

        private static LotHistoryLog _lotHistoryLog = null;
        private const LoadPortLoadingMode LoadingMode = LoadPortLoadingMode.Foup;

        private readonly Dictionary<string, TASK_ACTION> _carrierMovementRelatedCommands;
        private string _lastCompletionConditionKey;
        private EN_SCENARIO_RESULT _executedScenarioResult = EN_SCENARIO_RESULT.WAITING;
        #endregion </Fields>

        #region <Properties>
        bool NeedExecuteToScenarioForId
        {
            get
            {
                if (GetLoadPortState().CarrierAccessingState == CarrierAccessStates.NotAccessed ||
                    GetLoadPortState().CarrierIdVerificationState == CarrierIdVerificationStates.NotRead)
                    return true;

                return false;
            }
        }
        bool NeedExecuteToScenarioForSlot
        {
            get
            {
                if (GetLoadPortState().CarrierAccessingState == CarrierAccessStates.NotAccessed ||
                    GetLoadPortState().CarrierSlotMapVerificationState == CarrierSlotMapVerificationStates.NotRead)
                    return true;

                return false;
            }
        }
        SubstrateType MySubstrateType
        {
            get
            {
                return _functionsForPWA500.GetSubstrateTypeByLoadPortIndex(LoadPortIndex);
            }
        }
        SubstrateSize MySubstrateSize
        {
            get
            {
                return _functionsForPWA500.GetSubstrateSizeByLoadPortIndex(LoadPortIndex);
            }
        }
        #endregion </Properties>

        #region <Methods>

        #region <Overrides>
        protected override void GetCompletionCondition(
            out ICarrierCompletionHandlingPolicy policy,
            out ICarrierCompletionCondition condition)
        {

            _functionsForPWA500.UpdateConditionAndPolicy(
                PortId,
                MySubstrateType,
                ref _lastCompletionConditionKey,
                ref _completionPolicy,
                ref _completionCondition);

            policy = _completionPolicy;
            condition = _completionCondition;
        }

        protected override bool ActionCarrierLoading(bool manual = false)
        {
            switch (m_nSeqNum)
            {
                case (int)STEP_CARRIER_LOADING.START:
                    if (false == _loadPortManager.IsLoadPortEnabled(LoadPortIndex))
                    {
                        m_nSeqNum = (int)STEP_CARRIER_LOADING.END;
                        break;
                    }

                    Logger.WriteActionStartLog(m_enAction.ToString(), string.Empty);
                    m_nSeqNum = (int)STEP_CARRIER_LOADING.CHECK_READY;
                    break;

                case (int)STEP_CARRIER_LOADING.CHECK_READY:
                    {
                        if (_taskOperator.IsFinishingMode())
                        {
                            m_nSeqNum = (int)STEP_CARRIER_LOADING.END;
                            break;
                        }

                        _loadPortManager.InitLoadPortAction(LoadPortIndex);
                        if (manual)
                        {
                            SetDelayForSequence(500);   // 버튼 후딜레이가 있는듯.. 로드포트 비지 상태가 되어 바로 명령이 들어가면 Nack 된다.
                        }

                        if (false == _loadPortManager.GetCarrierLoadingType(LoadPortIndex).Equals(LoadPortLoadingMode.Foup))
                        {
                            m_nSeqNum = (int)STEP_CARRIER_LOADING.READ_CARRIER_ID;
                        }
                        else
                        {
                            m_nSeqNum = (int)STEP_CARRIER_LOADING.CLAMP_CARRIER;
                        }
                    }
                    break;

                case (int)STEP_CARRIER_LOADING.CLAMP_CARRIER:
                    {
                        var result = _loadPortManager.ClampCarrierAtLoadPort(LoadPortIndex);
                        switch (result.CommandResult)
                        {
                            //case EN_COMMAND_RESULT.PROCEED:
                            //    break;
                            case CommandResult.Completed:
                                {
                                    if (_taskOperator.IsFinishingMode())
                                    {
                                        m_nSeqNum = (int)STEP_CARRIER_LOADING.END;
                                        break;
                                    }

                                    m_nSeqNum = (int)STEP_CARRIER_LOADING.READ_CARRIER_ID;
                                }
                                break;

                            case CommandResult.Timeout:
                            case CommandResult.Error:
                            case CommandResult.Invalid:
                                {
                                    string[] arAlarmSubInfo = { GetTaskName(), string.Format("Clamp {0} - {1}", result.ActionName, result.Description) };
                                    GenerateSequenceAlarm((int)EN_ALARM.LOADPORT_FAILED_TO_ACTION, false, ref arAlarmSubInfo);
                                    m_nSeqNum = (int)STEP_CARRIER_LOADING.END;
                                }
                                break;

                            default:
                                break;
                        }
                    }
                    break;

                case (int)STEP_CARRIER_LOADING.READ_CARRIER_ID:
                    {
                        InitRFID(true);
                        ++m_nSeqNum;
                    }
                    break;

                case (int)STEP_CARRIER_LOADING.READ_CARRIER_ID + 1:
                    {
                        string carrierId = string.Empty;
                        // TODO : 로딩모드별 RFID 사용유무 재사용 하도록 개선 필요
                        var loadingMode = _loadPortManager.GetCarrierLoadingType(LoadPortIndex);
                        switch (loadingMode)
                        {
                            case LoadPortLoadingMode.ClosedCassette:
                                loadingMode = LoadPortLoadingMode.Cassette;
                                break;
                            default:
                                break;
                        }
                        var result = _rfidManager.ReadCarrierId(LoadPortIndex, loadingMode, ref carrierId);
                        //

                        switch (result.CommandResult)
                        {
                            case CommandResult.Completed:
                                {
                                    // 적용
                                    _carrierServer.SetCarrierId(PortId, carrierId);
                                    _carrierServer.SaveCarrierData(PortId);
                                    m_nSeqNum = (int)STEP_CARRIER_LOADING.READ_LOT_ID;
                                }
                                break;

                            case CommandResult.Timeout:
                            case CommandResult.Error:
                            case CommandResult.Invalid:
                                {
                                    if (++_retryCountForTagging <= MaxRetryCount)
                                    {
                                        InitRFID(false);
                                        SetDelayForSequence(DelayForRetryTagging);
                                    }
                                    else
                                    {
                                        string[] arAlarmSubInfo = { GetTaskName(), string.Format("RFID Reading Carrier Id {0} - {1}", result.ActionName, result.Description) };
                                        GenerateSequenceAlarm((int)EN_ALARM.RFID_READ_COMMAND_HAS_FAILED, false, ref arAlarmSubInfo);
                                        m_nSeqNum = (int)STEP_CARRIER_LOADING.END;
                                    }
                                }
                                break;

                            default:
                                break;
                        }
                    }
                    break;

                case (int)STEP_CARRIER_LOADING.READ_LOT_ID:
                    {
                        m_nSeqNum = (int)STEP_CARRIER_LOADING.EXECUTE_SCENARIO_TO_ID_READ;
                    }
                    break;

                case (int)STEP_CARRIER_LOADING.EXECUTE_SCENARIO_TO_ID_READ:
                    {
                        if (false == UpdateParamToCarrierIdRead())
                        {
                            m_nSeqNum = (int)STEP_CARRIER_LOADING.CHECK_ID_VERIFICATION_BY_HOST;
                        }
                        else
                        {
                            ++m_nSeqNum;
                        }
                    }
                    break;
                case (int)STEP_CARRIER_LOADING.EXECUTE_SCENARIO_TO_ID_READ + 1:
                    {
                        var result = ExecuteScenarioToCarrierIdRead();
                        switch (result.CommandResult)
                        {
                            case CommandResult.Completed:
                            case CommandResult.Skipped:
                                {
                                    if (_taskOperator.IsFinishingMode())
                                    {
                                        m_nSeqNum = (int)STEP_CARRIER_LOADING.END;
                                        break;
                                    }

                                    m_nSeqNum = (int)STEP_CARRIER_LOADING.CHECK_ID_VERIFICATION_BY_HOST;
                                }
                                break;
                            case CommandResult.Timeout:
                            case CommandResult.Error:
                            case CommandResult.Invalid:
                                {
                                    GenerateAlarm((int)EN_ALARM.LOADPORT_FAILED_TO_EXECUTE_SCENARIO_ID_READ, result.ActionName);
                                    m_nSeqNum = (int)STEP_CARRIER_LOADING.END;
                                }
                                break;
                            default:
                                break;
                        }
                    }
                    break;

                case (int)STEP_CARRIER_LOADING.CHECK_ID_VERIFICATION_BY_HOST:
                    {
                        if (false == UpdateParamToIdVarification())
                        {
                            m_nSeqNum = (int)STEP_CARRIER_LOADING.CHECK_ID_VERIFICATION_BY_HOST + 2;
                        }
                        else
                        {
                            ++m_nSeqNum;
                        }
                    }
                    break;
                case (int)STEP_CARRIER_LOADING.CHECK_ID_VERIFICATION_BY_HOST + 1:
                    {
                        var result = ExecuteScenarioToIdVarification();
                        switch (result.CommandResult)
                        {
                            case CommandResult.Completed:
                            case CommandResult.Skipped:
                                {
                                    _loadPortManager.PostCarrierIdVerificationResult(LoadPortIndex, true);
                                    ++m_nSeqNum;
                                }
                                break;

                            case CommandResult.Timeout:
                            case CommandResult.Error:
                            case CommandResult.Invalid:
                                {
                                    _loadPortManager.PostCarrierIdVerificationResult(LoadPortIndex, false);
                                    GenerateAlarm((int)EN_ALARM.LOADPORT_FAILED_TO_EXECUTING_SCENARIO_ID_VERIFICATION, result.ActionName);
                                    m_nSeqNum = (int)STEP_CARRIER_LOADING.END;
                                }
                                break;

                            default:
                                break;
                        }
                    }
                    break;

                case (int)STEP_CARRIER_LOADING.CHECK_ID_VERIFICATION_BY_HOST + 2:
                    {
                        if (_taskOperator.IsFinishingMode())
                        {
                            m_nSeqNum = (int)STEP_CARRIER_LOADING.END;
                            break;
                        }

                        m_nSeqNum = (int)STEP_CARRIER_LOADING.LOAD_CARRIER;
                    }
                    break;

                case (int)STEP_CARRIER_LOADING.LOAD_CARRIER:
                    {
                        var result = _loadPortManager.LoadCarrierAtLoadPort(LoadPortIndex);
                        switch (result.CommandResult)
                        {
                            case CommandResult.Completed:
                                {
                                    if (_taskOperator.IsDryRunMode())
                                    {
                                        _loadPortManager.ChangeSlotMapForDryRun(LoadPortIndex);
                                    }

                                    m_nSeqNum = (int)STEP_CARRIER_LOADING.CHECK_SLOT_VALIDITY;
                                }
                                break;

                            case CommandResult.Timeout:
                            case CommandResult.Error:
                            case CommandResult.Invalid:
                                {
                                    string[] arAlarmSubInfo = { GetTaskName(), string.Format("Load {0} - {1}", result.ActionName, result.Description) };
                                    GenerateSequenceAlarm((int)EN_ALARM.LOADPORT_FAILED_TO_ACTION, false, ref arAlarmSubInfo);
                                    m_nSeqNum = (int)STEP_CARRIER_LOADING.END;
                                }
                                break;

                            default:
                                break;
                        }
                    }
                    break;

                case (int)STEP_CARRIER_LOADING.CHECK_SLOT_VALIDITY:
                    {
                        if (false == CheckSlotValidation() || HasInvalidSlots())
                        {
                            m_nSeqNum = (int)STEP_CARRIER_LOADING.UNLOAD_CARRIER_BY_ERROR;
                            break;
                        }

                        m_nSeqNum = (int)STEP_CARRIER_LOADING.CHECK_SLOTMAP_VERIFICATION_BY_HOST;
                    }
                    break;

                case (int)STEP_CARRIER_LOADING.CHECK_SLOTMAP_VERIFICATION_BY_HOST:
                    {
                        _receivedCancelCarrier = false;
                        if (false == UpdateParamToSlotMapVarification())
                        {
                            m_nSeqNum = (int)STEP_CARRIER_LOADING.CHECK_SLOTMAP_VERIFICATION_BY_HOST + 2;
                        }
                        else
                        {
                            ++m_nSeqNum;
                        }
                    }
                    break;
                case (int)STEP_CARRIER_LOADING.CHECK_SLOTMAP_VERIFICATION_BY_HOST + 1:
                    {
                        var result = ExecuteToSlotMapVarification();
                        switch (result.CommandResult)
                        {
                            case CommandResult.Completed:
                            case CommandResult.Skipped:
                                {
                                    if (_receivedCancelCarrier)
                                    {
                                        _loadPortManager.PostCarrierSlotMapVerificationResult(LoadPortIndex, false);
                                        m_nSeqNum = (int)STEP_CARRIER_LOADING.UNLOAD_CARRIER_BY_ERROR;
                                        break;
                                    }
                                    else
                                    {
                                        if (UseSecsGem && UseSlotValidationResult)
                                        {
                                            if (false == IsScannedInfoValidWithHost())
                                            {
                                                // error
                                                _loadPortManager.PostCarrierSlotMapVerificationResult(LoadPortIndex, false);
                                                m_nSeqNum = (int)STEP_CARRIER_LOADING.UNLOAD_CARRIER_BY_ERROR;
                                            }
                                            else
                                            {
                                                ApplyScannedInfo();
                                                ++m_nSeqNum;
                                            }
                                        }
                                        else
                                        {
                                            ++m_nSeqNum;
                                        }
                                    }
                                }
                                break;
                            case CommandResult.Timeout:
                            case CommandResult.Error:
                            case CommandResult.Invalid:
                                {
                                    _loadPortManager.PostCarrierSlotMapVerificationResult(LoadPortIndex, false);
                                    GenerateAlarm((int)EN_ALARM.LOADPORT_FAILED_TO_EXECUTING_SCENARIO_SLOT_VERIFICATION, result.ActionName);
                                    m_nSeqNum = (int)STEP_CARRIER_LOADING.END;
                                }
                                break;
                            default:
                                break;
                        }
                    }
                    break;

                case (int)STEP_CARRIER_LOADING.CHECK_SLOTMAP_VERIFICATION_BY_HOST + 2:
                    {
                        if (_taskOperator.IsFinishingMode())
                        {
                            m_nSeqNum = (int)STEP_CARRIER_LOADING.END;
                            break;
                        }

                        _loadPortManager.PostCarrierSlotMapVerificationResult(LoadPortIndex, true);
                        m_nSeqNum = (int)STEP_CARRIER_LOADING.WAIT_FOR_JOB_BINDING;
                    }
                    break;

                case (int)STEP_CARRIER_LOADING.WAIT_FOR_JOB_BINDING:
                    {
                        if (TryPrepareJobsUntilReady())
                        {
                            m_nSeqNum = (int)STEP_CARRIER_LOADING.WAIT_FOR_JOB_BINDING + 10;
                        }
                        else
                        {
                            ++m_nSeqNum;
                        }
                    }
                    break;
                case (int)STEP_CARRIER_LOADING.WAIT_FOR_JOB_BINDING + 1:
                    {
                        if (_taskOperator.IsFinishingMode())
                        {
                            m_nSeqNum = (int)STEP_CARRIER_LOADING.END;
                            break;
                        }

                        --m_nSeqNum;
                    }
                    break;

                case (int)STEP_CARRIER_LOADING.WAIT_FOR_JOB_BINDING + 10:
                    {
                        if (IsLotStarted())
                        {
                            m_nSeqNum = (int)STEP_CARRIER_LOADING.UPDATE_LINK;
                            break;
                        }
                        ++m_nSeqNum;
                    }
                    break;

                case (int)STEP_CARRIER_LOADING.WAIT_FOR_JOB_BINDING + 11:
                    {
                        if (_taskOperator.IsFinishingMode())
                        {
                            m_nSeqNum = (int)STEP_CARRIER_LOADING.END;
                            break;
                        }

                        --m_nSeqNum;
                    }
                    break;

                case (int)STEP_CARRIER_LOADING.UNLOAD_CARRIER_BY_ERROR:
                    {
                        var result = _loadPortManager.UnloadCarrierAtLoadPort(LoadPortIndex);
                        switch (result.CommandResult)
                        {
                            case CommandResult.Completed:
                            case CommandResult.Timeout:
                            case CommandResult.Error:
                            case CommandResult.Invalid:
                                {
                                    if (_receivedCancelCarrier)
                                    {
                                        GenerateAlarm((int)EN_ALARM.LOADPORT_FAILED_TO_EXECUTING_SCENARIO_SLOT_VERIFICATION, "Canceled by host");
                                    }
                                    else
                                    {
                                        GenerateAlarm((int)EN_ALARM.LOADPORT_SLOT_STATUS_IS_WRONG);
                                    }

                                    m_nSeqNum = (int)STEP_CARRIER_LOADING.END;
                                }
                                break;

                            default:
                                break;
                        }
                    }
                    break;

                case (int)STEP_CARRIER_LOADING.UPDATE_LINK:
                    SetCarrierPortState(CARRIER_PORT_TYPE.SELECTION);
                    m_nSeqNum = (int)STEP_CARRIER_LOADING.END;
                    break;

                case (int)STEP_CARRIER_LOADING.END:
                    Logger.WriteActionEndLog(m_enAction.ToString(), string.Empty);
                    return true;

                default:
                    return false;
            }

            return false;
        }
        protected override bool ActionCarrierUnloading(bool manual, bool reportForcefully)
        {
            switch (m_nSeqNum)
            {
                case (int)STEP_CARRIER_UNLOADING.START:
                    InitQueuedScenario();
                    if (false == _loadPortManager.IsLoadPortEnabled(LoadPortIndex))
                    {
                        m_nSeqNum = (int)STEP_CARRIER_UNLOADING.END;
                        break;
                    }
                    Logger.WriteActionStartLog(m_enAction.ToString(), string.Empty);
                    m_nSeqNum = (int)STEP_CARRIER_UNLOADING.CHECK_READY;
                    break;

                case (int)STEP_CARRIER_UNLOADING.CHECK_READY:
                    {
                        _loadPortManager.InitLoadPortAction(LoadPortIndex);
                        if (manual)
                        {
                            SetDelayForSequence(500);   // 버튼 후딜레이가 있는듯.. 로드포트 비지 상태가 되어 바로 명령이 들어가면 Nack 된다.
                        }

                        var status = _loadPortManager.GetLoadPortState(LoadPortIndex);
                        if (false == reportForcefully &&
                            false == status.DoorState &&
                            false == status.DockState &&
                            false == status.ClampState)
                        {
                            m_nSeqNum = (int)STEP_CARRIER_UNLOADING.UPDATE_LINK;
                        }
                        else
                        {
                            m_nSeqNum = (int)STEP_CARRIER_UNLOADING.EXECUTE_QUEUED_SCENARIO_BEFORE_END;
                        }
                    }
                    break;

                case (int)STEP_CARRIER_UNLOADING.EXECUTE_QUEUED_SCENARIO_BEFORE_END:
                    {
                        InitQueuedScenario();

                        var accessStatus = _carrierServer.GetCarrierAccessingStatus(PortId);
                        if (accessStatus.Equals(CarrierAccessStates.CarrierCompleted) || reportForcefully)
                        {
                            // 강제 이벤트 전송이어도 작업하지 않았거나, 중단된 캐리어는 보고하지 않는다.
                            if (accessStatus.Equals(CarrierAccessStates.NotAccessed) ||
                                accessStatus.Equals(CarrierAccessStates.CarrierStopped))
                            {
                                m_nSeqNum = (int)STEP_CARRIER_UNLOADING.UNLOAD_CARRIER;
                                break;
                            }
                            else
                            {
                                if (EnqueueScenraioBeforeActionCompletion(out QueuedScenarioInfo scenarioListToEnque))
                                {
                                    EnqueueScenario(scenarioListToEnque.Scenario, scenarioListToEnque.ScenarioParams, scenarioListToEnque.AdditionalParams);
                                }
                            }
                        }
                        else
                        {
                            m_nSeqNum = (int)STEP_CARRIER_UNLOADING.UNLOAD_CARRIER;
                            break;
                        }

                        //if (false == _carrierServer.GetCarrierAccessingStatus(PortId).Equals(CarrierAccessStates.CarrierCompleted))
                        //{
                        //    m_nSeqNum = (int)STEP_CARRIER_UNLOADING.UNLOAD_CARRIER;
                        //    break;
                        //}
                        //else
                        //{
                        //    if (EnqueueScenraioBeforeActionCompletion(out QueuedScenarioInfo scenarioListToEnque))
                        //    {
                        //        EnqueueScenario(scenarioListToEnque.Scenario, scenarioListToEnque.ScenarioParams, scenarioListToEnque.AdditionalParams);
                        //    }
                        //}

                        ++m_nSeqNum;
                    }
                    break;

                case (int)STEP_CARRIER_UNLOADING.EXECUTE_QUEUED_SCENARIO_BEFORE_END + 1:
                    {
                        if (false == DequeueQueuedScenario())
                        {
                            m_nSeqNum = (int)STEP_CARRIER_UNLOADING.WRITE_CARRIER_ID_TAG;
                        }
                        else
                        {
                            ++m_nSeqNum;
                        }
                    }
                    break;

                case (int)STEP_CARRIER_UNLOADING.EXECUTE_QUEUED_SCENARIO_BEFORE_END + 2:
                    {
                        var result = ExecuteQueuedScenario();
                        switch (result.CommandResult)
                        {
                            case CommandResult.Skipped:
                            case CommandResult.Completed:
                                m_nSeqNum = (int)STEP_CARRIER_UNLOADING.EXECUTE_QUEUED_SCENARIO_BEFORE_END + 1;
                                break;
                            case CommandResult.Timeout:
                            case CommandResult.Error:
                            case CommandResult.Invalid:
                                {
                                    GenerateAlarm((int)EN_ALARM.LOADPORT_SECSGEM_ERROR_BEFORE_UNLOADING_CARRIER, result.ActionName);
                                    m_nSeqNum = (int)STEP_CARRIER_UNLOADING.END;
                                }
                                break;
                            default:
                                break;
                        }
                    }
                    break;

                case (int)STEP_CARRIER_UNLOADING.WRITE_CARRIER_ID_TAG:
                    {
                        InitRFID(true);
                        ++m_nSeqNum;
                    }
                    break;

                case (int)STEP_CARRIER_UNLOADING.WRITE_CARRIER_ID_TAG + 1:
                    {
                        var result = WriteCarrierId();
                        switch (result.CommandResult)
                        {
                            case CommandResult.Completed:
                            case CommandResult.Skipped:
                                {
                                    m_nSeqNum = (int)STEP_CARRIER_UNLOADING.WRITE_CARRIER_LOT_ID_TAG;
                                }
                                break;
                            case CommandResult.Timeout:
                            case CommandResult.Error:
                            case CommandResult.Invalid:
                                {
                                    if (++_retryCountForTagging <= MaxRetryCount)
                                    {
                                        InitRFID(false);
                                        SetDelayForSequence(DelayForRetryTagging);
                                    }
                                    else
                                    {
                                        GenerateAlarm((int)EN_ALARM.RFID_WRITE_COMMAND_HAS_FAILED);
                                        m_nSeqNum = (int)STEP_CARRIER_UNLOADING.END;
                                    }
                                }
                                break;
                            default:
                                break;
                        }
                    }
                    break;

                case (int)STEP_CARRIER_UNLOADING.WRITE_CARRIER_LOT_ID_TAG:
                    {
                        InitRFID(true);
                        ++m_nSeqNum;
                    }
                    break;

                case (int)STEP_CARRIER_UNLOADING.WRITE_CARRIER_LOT_ID_TAG + 1:
                    {
                        var result = WriteLotId();
                        switch (result.CommandResult)
                        {
                            case CommandResult.Completed:
                            case CommandResult.Skipped:
                                InitRFID(true);
                                m_nSeqNum = (int)STEP_CARRIER_UNLOADING.EXECUTE_AFTER_WRITING;
                                break;
                            case CommandResult.Timeout:
                            case CommandResult.Error:
                            case CommandResult.Invalid:
                                {
                                    if (++_retryCountForTagging <= MaxRetryCount)
                                    {
                                        InitRFID(false);
                                        SetDelayForSequence(DelayForRetryTagging);
                                    }
                                    else
                                    {
                                        GenerateAlarm((int)EN_ALARM.RFID_WRITE_COMMAND_HAS_FAILED);
                                        m_nSeqNum = (int)STEP_CARRIER_UNLOADING.END;
                                    }
                                }
                                break;
                            default:
                                break;
                        }
                    }
                    break;

                case (int)STEP_CARRIER_UNLOADING.EXECUTE_AFTER_WRITING:
                    {
                        var result = ExecuteAfterWriting();
                        switch (result.CommandResult)
                        {
                            case CommandResult.Completed:
                            case CommandResult.Skipped:
                                m_nSeqNum = (int)STEP_CARRIER_UNLOADING.UNLOAD_CARRIER;
                                break;
                            case CommandResult.Timeout:
                            case CommandResult.Error:
                            case CommandResult.Invalid:
                                {
                                    GenerateAlarm((int)EN_ALARM.RFID_WRITE_COMMAND_HAS_FAILED);
                                    m_nSeqNum = (int)STEP_CARRIER_UNLOADING.END;
                                }
                                break;
                            default:
                                break;
                        }
                    }
                    break;

                case (int)STEP_CARRIER_UNLOADING.UNLOAD_CARRIER:
                    {
                        var result = _loadPortManager.UnloadCarrierAtLoadPort(LoadPortIndex);
                        switch (result.CommandResult)
                        {
                            case CommandResult.Completed:
                                {
                                    m_nSeqNum = (int)STEP_CARRIER_UNLOADING.UPDATE_LINK;
                                }
                                break;
                            case CommandResult.Timeout:
                            case CommandResult.Error:
                            case CommandResult.Invalid:
                                {
                                    string[] arAlarmSubInfo = { GetTaskName(), string.Format("Unload {0} - {1}", result.ActionName, result.Description) };
                                    GenerateSequenceAlarm((int)EN_ALARM.LOADPORT_FAILED_TO_ACTION, false, ref arAlarmSubInfo);
                                    m_nSeqNum = (int)STEP_CARRIER_UNLOADING.END;
                                }
                                break;

                            default:
                                break;
                        }
                    }
                    break;

                case (int)STEP_CARRIER_UNLOADING.UPDATE_LINK:
                    SetCarrierPortState(CARRIER_PORT_TYPE.SELECTION);
                    m_nSeqNum = (int)STEP_CARRIER_UNLOADING.END;
                    break;

                case (int)STEP_CARRIER_UNLOADING.END:
                    Logger.WriteActionEndLog(m_enAction.ToString(), string.Empty);
                    return true;

                default:
                    return false;
            }

            return false;
        }
        protected override bool GetBusyIndex(int lpIndex, ref int indexOfDigital)
        {
            int relIndex = lpIndex * 4;
            indexOfDigital = (int)Define.DefineEnumProject.DigitalIO.PWA500W.EN_DIGITAL_IN.LP1_RUN + relIndex;

            return true;
        }
        protected override void ExecuteAtAlways()
        {
            // PIO Interface가 E84인 경우에만
            if (_pioInterfaceType == (Define.DefineEnumProject.AppConfig.EN_PIO_INTERFACE_TYPE.E84))
            {
                if (IsAlarmState())
                {
                    _loadPortManager.WriteAMHSEmergencyStop(LoadPortIndex, false);
                    _loadPortManager.WriteAMHSHandoffAvailable(LoadPortIndex, false);
                    return;
                }
                if (false == IsPIOEmergencyStopOn())
                {
                    _loadPortManager.WriteAMHSEmergencyStop(LoadPortIndex, false);
                }
                else
                {
                    _loadPortManager.WriteAMHSEmergencyStop(LoadPortIndex, true);
                }

                if (false == IsPIOHandOffAvailableOn())
                {
                    _loadPortManager.WriteAMHSHandoffAvailable(LoadPortIndex, false);
                }
                else
                {
                    if (false == _loadPortManager.IsPIOInterfaceWorking(LoadPortIndex))
                    {
                        _loadPortManager.WriteAMHSHandoffAvailable(LoadPortIndex, true);
                    }
                    //_loadPortManager.WriteAMHSHandoffAvailable(LoadPortIndex, true);
                }
            }
        }
        protected override void GetAtmRobotTaskName(out List<string> taskNames)
        {
            taskNames = new List<string>();
            taskNames.Add(Define.DefineEnumProject.Task.EN_TASK_LIST.AtmRobot.ToString());
        }
        protected override bool UpdateParamToCarrierIdRead()
        {
            if (false == _scenarioOperator.UseScenario)
                return false;

            if (false == _carrierServer.HasCarrier(PortId))
                return false;

            return true;
        }
        protected override CommandResults ExecuteScenarioToCarrierIdRead()
        {
            return new CommandResults(m_enAction.ToString(), CommandResult.Skipped);
        }
        protected override bool UpdateParamToIdVarification()
        {
            if (false == _carrierServer.HasCarrier(PortId))
                return false;

            if (false == NeedExecuteToScenarioForId)
                return false;

            var carrierService = _loadPortManager.GetCarrierService(LoadPortIndex);
            if (carrierService == null)
                return false;

            //InitResult(ScenarioTypeToIdRead);
            string carrierId = _carrierServer.GetCarrierId(PortId);
            if (_scenarioOperator.UseScenario)
            {
                _lotHistoryLog.ClearPreviousHistory(PortId, carrierId, LoadPortName);

                carrierService.SetCarrierIdentifier(
                     GetLoadPortName(),
                     _carrierServer.GetCarrierId(PortId),
                     (long)VerificationResult.Suceeded);
            }
            else
            {
                carrierService.RequestProceedCarrier(
                    GetLoadPortName(),
                    carrierId,
                    null,
                    null,
                    null,
                    "PRODUCT");
                //_loadPortManager.PostCarrierIdVerificationResult(LoadPortIndex, true);
            }

            return true;
        }
        private void UpdateCarrierInfo()
        {
            var id = _carrierServer.GetCarrierId(PortId);
            _loadPortManager.AssociateCarrier(LoadPortIndex, id);

            _carrierServer.UpdateSlotLocationNameToLocation(PortId, id);
        }
        protected override CommandResults ExecuteScenarioToIdVarification()
        {
            var state = GetLoadPortState();
            switch (state.CarrierIdVerificationState)
            {
                case CarrierIdVerificationStates.NotRead:
                case CarrierIdVerificationStates.WaitingForHost:
                    return new CommandResults(m_enAction.ToString(), CommandResult.Proceed);

                case CarrierIdVerificationStates.VerificationOk:
                    {
                        UpdateCarrierInfo();

                        return new CommandResults(m_enAction.ToString(), CommandResult.Completed);
                    }

                case CarrierIdVerificationStates.VerificationFailed:
                    return new CommandResults(m_enAction.ToString(), CommandResult.Error);

                default:
                    return new CommandResults(m_enAction.ToString(), CommandResult.Proceed);
            }
        }
        protected override bool UpdateParamToSlotMapVarification()
        {
            if (false == _carrierServer.HasCarrier(PortId))
                return false;

            if (false == NeedExecuteToScenarioForId)
                return false;

            var slots = _carrierServer.GetCarrierSlotMap(PortId);
            if (_loadPortManager.IsLoadPortSimulationMode(LoadPortIndex))
            {
                var newSlot = new Dictionary<int, CarrierSlotMapStates>();
                if (_functionsForPWA500.IsEmptyCarrierAtSimulation(MySubstrateType))
                {
                    _substrateManager.RemoveSubstrateAtLoadPortAll(PortId);
                    //var newSlot = new Dictionary<int, CarrierSlotMapStates>();
                    foreach (var item in slots)
                    {
                        newSlot[item.Key] = CarrierSlotMapStates.Empty;
                    }
                    _carrierServer.SetCarrierSlotMap(PortId, newSlot);
                    _carrierServer.SaveCarrierData(PortId);
                    slots = newSlot;
                }
                else
                {
                    List<string> targets = new List<string>();
                    foreach (var item in slots)
                    {
                        int capa = 5;
                        if (MySubstrateType == SubstrateType.Core)
                        {
                            capa = 7;
                        }
                        var isTargetSlot = (item.Key % 2 == 1);
                        if (isTargetSlot ||
                            item.Key > capa)
                        //if (isTargetSlot)
                        {
                            newSlot[item.Key] = CarrierSlotMapStates.Empty;
                            var key = _substrateManager.GetSubstrateKeyAtLoadPort(PortId, item.Key);
                            if (false == string.IsNullOrWhiteSpace(key))
                            {
                                targets.Add(key);
                            }
                        }
                        else
                        {
                            newSlot[item.Key] = CarrierSlotMapStates.CorrectlyOccupied;
                        }
                    }

                    foreach (var item in targets)
                    {
                        _substrateManager.RemoveSubstrateByKey(item);
                    }

                    _carrierServer.SetCarrierSlotMap(PortId, newSlot);
                    _carrierServer.SaveCarrierData(PortId);
                    slots = newSlot;
                }
            }

            var carrierService = _loadPortManager.GetCarrierService(LoadPortIndex);
            if (carrierService == null)
                return false;

            //InitResult(ScenarioTypeToIdRead);

            string carrierId = _carrierServer.GetCarrierId(PortId);

            if (_scenarioOperator.UseScenario)
            {
                carrierService.SetSlotMap(
                    GetLoadPortName(),
                    slots,
                    _carrierServer.GetCarrierId(PortId),
                    (long)VerificationResult.Suceeded);
            }
            else
            {
                Dictionary<int, string> lots = new Dictionary<int, string>();
                Dictionary<int, string> substrateNames = new Dictionary<int, string>();
                var substrates = _substrateManager.GetSubstratesAtLoadPort(PortId);
                foreach (var item in slots)
                {
                    var lotId = string.Empty;
                    var name = string.Empty;
                    if (substrates.TryGetValue(item.Key, out var s))
                    {
                        lotId = s.LotId;
                        name = s.Name;
                    }

                    lots[item.Key] = lotId;
                    substrateNames[item.Key] = name;
                }

                carrierService.RequestProceedCarrier(
                    GetLoadPortName(),
                    carrierId,
                    slots,
                    lots,
                    substrateNames,
                    "PRODUCT");
            }

            return true;
        }
        protected override CommandResults ExecuteToSlotMapVarification()
        {
            var state = GetLoadPortState();
            switch (state.CarrierSlotMapVerificationState)
            {
                case CarrierSlotMapVerificationStates.NotRead:
                case CarrierSlotMapVerificationStates.WaitingForHost:
                    return new CommandResults(m_enAction.ToString(), CommandResult.Proceed);

                case CarrierSlotMapVerificationStates.VerificationOk:
                    {
                        MakeSubstratesAtLocal();
                        return new CommandResults(m_enAction.ToString(), CommandResult.Completed);
                    }

                case CarrierSlotMapVerificationStates.VerificationFailed:
                    return new CommandResults(m_enAction.ToString(), CommandResult.Error);

                default:
                    return new CommandResults(m_enAction.ToString(), CommandResult.Proceed);
            }
        }
        protected override bool IsScannedInfoValidWithHost()
        {
            return true;
        }
        protected override void ApplyScannedInfo()
        {
        }
        protected override bool EnqueueScenraioBeforeActionCompletion(out QueuedScenarioInfo scenarioInfo)
        {
            scenarioInfo = null;
            return false;
        }
        protected override void ExecuteAfterScenarioCompletion(EN_SCENARIO scenario, EN_SCENARIO_RESULT result, Dictionary<string, string> scenarioParam, Dictionary<string, string> additionalParams)
        {
        }
        protected override bool UpdateParamToLoadCarrier()
        {
            return false;
        }
        protected override CommandResults ExecuteScenarioToLoadCarrier()
        {
            _commandResult.CommandResult = CommandResult.Completed;
            return _commandResult;
        }
        protected override bool UpdateParamToUnloadCarrier()
        {
            return false;
        }
        protected override CommandResults ExecuteScenarioToUnloadCarrier()
        {
            _commandResult.CommandResult = CommandResult.Completed;
            return _commandResult;
        }
        protected override bool UpdateParamAfterCarrierArrived()
        {
            return false;
        }
        protected override CommandResults ExecuteScenarioAfterCarrierArrived()
        {
            _commandResult.CommandResult = CommandResult.Completed;
            return _commandResult;
        }
        protected override bool PrepareBeforeSendingCarrier()
        {
            return false;
        }
        protected override CommandResults ExecuteBeforeSendingCarrier()
        {
            _commandResult.CommandResult = CommandResult.Completed;
            return _commandResult;
        }
        protected override CommandResults WriteCarrierId()
        {
            _commandResult.CommandResult = CommandResult.Completed;
            return _commandResult;
        }
        protected override CommandResults WriteLotId()
        {
            _commandResult.CommandResult = CommandResult.Completed;
            return _commandResult;
        }
        protected override CommandResults ExecuteAfterWriting()
        {
            _commandResult.CommandResult = CommandResult.Completed;
            return _commandResult;
        }

        protected override bool CheckSlotValidation()
        {
            return true;
        }
        #endregion </Overrides>

        #region <Internal Interfaces>
        private string GetLoadPortName()
        {
            return _loadPortManager.GetLoadPortName(LoadPortIndex);
        }
        private LoadPortStateInformation GetLoadPortState()
        {
            return _loadPortManager.GetLoadPortState(LoadPortIndex);
        }
        private void MakeSubstratesAtLocal()
        {
            if (_scenarioOperator.UseScenario ||
                MySubstrateType == SubstrateType.Empty)
            {
                //string carrierId = _carrierServer.GetCarrierId(PortId);
                var slots = _carrierServer.GetCarrierSlotMap(PortId);
                var substrates = _substrateManager.GetSubstratesAtLoadPort(PortId);
                foreach (var item in slots)
                {
                    if (substrates.TryGetValue(item.Key, out var s))
                    {
                        _substrateManager.CreateSubstrateAtDriver(s.UniqueKey);
                    }
                }
            }
        }

        private bool TryGetMyControlJobByCarrierId(
            IJobManager manager,
            string carrierId,
            out ControlJob controlJob)
        {
            controlJob = null;

            if (manager == null)
                return false;

            if (string.IsNullOrWhiteSpace(carrierId))
                return false;

            var controlJobs = manager.GetAllControlJobs();
            if (controlJobs == null || controlJobs.Count == 0)
                return false;

            foreach (var job in controlJobs)
            {
                if (job == null)
                    continue;

                if (false == IsMyControlJobByCarrierId(job, carrierId))
                    continue;

                controlJob = job;
                return true;
            }

            return false;
        }
        private bool IsMyControlJobByCarrierId(
            ControlJob controlJob,
            string carrierId)
        {
            if (controlJob == null)
                return false;

            if (string.IsNullOrWhiteSpace(carrierId))
                return false;

            if (MySubstrateType == SubstrateType.Core || MySubstrateType == SubstrateType.Empty)
            {
                return ContainsControlJobByBinderCarrier(
                    controlJob,
                    carrierId);
            }

            //if (_functionsForPWA500.IsBinType(MySubstrateType))
            //{
            //    // Bin 정책:
            //    // OutSpec이 있으면 OutSpec destination carrier로 간다.
            //    if (HasMaterialOutputSpecification(controlJob))
            //    {
            //        return ContainsMaterialOutputSpecificationValue(
            //            controlJob,
            //            carrierId);
            //    }

            //    // OutSpec이 없으면 source carrier로 간다.
            //    return ContainsControlJobByBinderCarrier(
            //        controlJob,
            //        carrierId);
            //}

            return false;
        }
        private bool ContainsControlJobByBinderCarrier(
            ControlJob controlJob,
            string carrierId)
        {
            if (controlJob == null)
                return false;

            if (string.IsNullOrWhiteSpace(carrierId))
                return false;

            var binder = SubstrateJobBindingService.Instance;
            if (binder == null)
                return false;

            var controlJobIds = binder.GetControlJobIdsByCarrier(carrierId);
            if (controlJobIds == null || controlJobIds.Count == 0)
                return false;

            foreach (var controlJobId in controlJobIds)
            {
                if (string.Equals(
                    controlJobId,
                    controlJob.Id,
                    StringComparison.OrdinalIgnoreCase))
                {
                    return true;
                }
            }

            return false;
        }
        private static bool HasMaterialOutputSpecification(ControlJob controlJob)
        {
            if (controlJob == null)
                return false;

            var outputSpecifications = controlJob.MaterialOutputSpecifications;
            if (outputSpecifications == null || outputSpecifications.Length == 0)
                return false;

            foreach (var outputSpec in outputSpecifications)
            {
                if (outputSpec == null)
                    continue;

                if (false == string.IsNullOrWhiteSpace(outputSpec.Value))
                    return true;
            }

            return false;
        }
        private static bool ContainsMaterialOutputSpecificationValue(
            ControlJob controlJob,
            string carrierId)
        {
            if (controlJob == null)
                return false;

            if (string.IsNullOrWhiteSpace(carrierId))
                return false;

            var outputSpecifications = controlJob.MaterialOutputSpecifications;
            if (outputSpecifications == null || outputSpecifications.Length == 0)
                return false;

            foreach (var outputSpec in outputSpecifications)
            {
                if (outputSpec == null)
                    continue;

                if (string.Equals(
                    outputSpec.Value,
                    carrierId,
                    StringComparison.OrdinalIgnoreCase))
                {
                    return true;
                }
            }

            return false;
        }
        private bool TryAdvanceJobUntilProcessJobProcessing(
            IJobManager manager,
            ControlJob controlJob,
            out string processJobId)
        {
            processJobId = string.Empty;

            if (manager == null || controlJob == null)
                return false;

            if (controlJob.State == ControlJobState.Queued)
            {
                if (false == manager.IsHeadOfQueueControlJob(controlJob.Id))
                {
                    manager.RequestControlJobHeadOfQueue(controlJob.Id);
                    return false;
                }

                manager.RequestControlJobSelect(controlJob.Id);
                return false;
            }

            if (controlJob.State == ControlJobState.Selected ||
                controlJob.State == ControlJobState.WaitingForStart)
            {
                return false;
            }

            if (controlJob.State != ControlJobState.Executing)
                return false;

            return TryAdvanceProcessJobUntilProcessing(
                manager,
                controlJob.Id,
                out processJobId);
        }
        private bool TryAdvanceProcessJobUntilProcessing(
            IJobManager manager,
            string controlJobId,
            out string processJobId)
        {
            processJobId = string.Empty;

            if (manager == null)
                return false;

            if (string.IsNullOrWhiteSpace(controlJobId))
                return false;

            var processJobs = manager.GetLinkedProcessJobs(controlJobId);
            if (processJobs == null || processJobs.Count == 0)
                return false;

            foreach (var processJob in processJobs)
            {
                if (processJob == null)
                    continue;

                if (processJob.State == ProcessJobState.JobQueued)
                {
                    manager.NotifyProcessJobSettingUpStarted(processJob.Id);
                    return false;
                }

                if (processJob.State == ProcessJobState.SettingUp)
                {
                    manager.NotifyProcessJobSettingUpCompleted(processJob.Id);
                    return false;
                }

                if (processJob.State == ProcessJobState.WaitingForStart)
                {
                    return false;
                }

                if (processJob.State == ProcessJobState.Processing)
                {
                    processJobId = processJob.Id;
                    return true;
                }
            }

            return false;
        }
        private static bool IsControlJobBound(ControlJob controlJob)
        {
            if (controlJob == null)
                return false;

            if (string.IsNullOrWhiteSpace(controlJob.Id))
                return false;

            return SubstrateJobBindingService.Instance == null ||
                   SubstrateJobBindingService.Instance.IsBoundForControlJob(controlJob.Id);
        }
        private bool TryPrepareJobsUntilReady()
        {
            if (MySubstrateType == SubstrateType.Bin1
                || MySubstrateType == SubstrateType.Bin2
                || MySubstrateType == SubstrateType.Bin3)
                return true;

            string carrierId = _carrierServer.GetCarrierId(PortId);
            if (string.IsNullOrWhiteSpace(carrierId))
                return false;

            IJobManager manager = JobManager.Instance;

            ControlJob controlJob;
            if (false == TryGetMyControlJobByCarrierId(
                manager,
                carrierId,
                out controlJob))
            {
                return false;
            }

            if (controlJob == null)
                return false;

            if (false == IsControlJobBound(controlJob))
                return false;

            if (MySubstrateType == SubstrateType.Core)
            {
                return true;
            }
            else
            {
                // TODO : 고객사 확인 필요
                // Bin1/2/3의 경우 JobBinding 이후 잡 스타트 진행
                // 2026.06.19 dwlim [MOD] 공테이프(Empty 또는 Bin에 Job 사용 안하기로함)에 잡 진행 
                if (/*_functionsForPWA500.IsBinType(MySubstrateType)*/MySubstrateType == SubstrateType.Empty)
                {
                    //string processJobId;
                    return TryAdvanceJobUntilProcessJobProcessing(
                        JobManager.Instance,
                        controlJob,
                        out _);
                }
            }
            return true;
        }
        private bool IsLotStarted()
        {
            if (MySubstrateType != SubstrateType.Core)
                return true;

            return true;
        }
        private Dictionary<string, string> MakeScenarioParamToLotHandling(string lotId, string carrierId)
        {
            Dictionary<string, string> data = new Dictionary<string, string>
            {
                [EN_SVID_LIST.LOTID.ToString()] = lotId,
                [EN_SVID_LIST.CARRIERID.ToString()] = carrierId
            };
            return data;
        }
        private void OnAutoScenarioCompleted(
            string sender,
            EN_SCENARIO scenario,
            Dictionary<string, string> scenarioParams,
            Dictionary<string, string> resultData,
            EN_SCENARIO_RESULT result)
        {
            if (false == string.Equals(
                sender, 
                GetTaskName(), 
                StringComparison.OrdinalIgnoreCase))
                return;

            _executedScenarioResult = result;
            //switch (scenario)
            //{
            //    case ScenarioCoreLotStart:
            //        break;
            //}
        }
        #region <E84>
        private bool IsPIOEmergencyStopOn()
        {
            // 1 EMO 버튼이 눌러졌을 경우 OFF
            if (DigitalIO_.DigitalIO.GetInstance().ReadInput((int)Define.DefineEnumProject.DigitalIO.PWA500BIN.EN_DIGITAL_IN.EFEM_EMS_STATUS))
                return false;

            // 2 Passive 측의 인터락 점검문 또는 커버가 열렸을 경우 OFF
            //   -> Door로 판단된다.
            if (false == DigitalIO_.DigitalIO.GetInstance().ReadInput((int)Define.DefineEnumProject.DigitalIO.PWA500BIN.EN_DIGITAL_IN.EFEM_DOOR_CLOSE))
                return false;

            // 3 Passive 측의 Carrier Handling Robot이 에러 상태인 경우(내부 버퍼 공간이 있는 tool 또는 stocker) OFF 
            //   -> Fixed Buffer라 해당되지 않음

            // 4 Light curtain 에러가 발생하였을 경우 OFF
            //   -> Light Curtain이 없으므로 해당되지 않음

            // 5 Loadport 와 연관되지 않는 error가 발생한 경우 ON
            //   -> Task가 분리되어 있으므로 중대한 Alarm 발생이 아니면 분리되어 있다고 보는게 맞다.

            // 6 Loadport 와 연관된 error가 발생한 경우 OFF
            //   -> 각 LoadPort의 Task에서 Alarm이 발생하면 정지하게 된다.
            if (false == (EquipmentState_.EquipmentState.GetInstance().GetState().Equals(EquipmentState_.EQUIPMENT_STATE.IDLE) ||
                EquipmentState_.EquipmentState.GetInstance().GetState().Equals(EquipmentState_.EQUIPMENT_STATE.READY) ||
                EquipmentState_.EquipmentState.GetInstance().GetState().Equals(EquipmentState_.EQUIPMENT_STATE.SETUP) ||
                EquipmentState_.EquipmentState.GetInstance().GetState().Equals(EquipmentState_.EQUIPMENT_STATE.EXECUTING) ||
                EquipmentState_.EquipmentState.GetInstance().GetState().Equals(EquipmentState_.EQUIPMENT_STATE.FINISHING)))
                return false;

            // 7 Protection bar 에러가 발생하였을 경우 OFF
            if (IsSaftyInterLockError())
                return false;

            // 8 Manual 모드인 경우 OFF
            // 9 Auto 모드인 경우 ON
            if (false == _loadPortManager.GetAccessMode(LoadPortIndex).Equals(LoadPortAccessMode.Auto))
                return false;

            return true;
        }
        private bool IsPIOHandOffAvailableOn()
        {
            // 3 Passive의 INTRA access mode가 manual일 경우 OFF
            if (false == _loadPortManager.GetAccessMode(LoadPortIndex).Equals(LoadPortAccessMode.Auto))
                return false;

            if (false == _loadPortManager.IsPIOInterfaceWorking(LoadPortIndex))
            {
                // 1 Presence 센서는 ON이고 Placement 센서는 OFF일 경우 OFF
                // 2 Presence 센서는 OFF이고 Placement 센서는 ON일 경우 OFF
                bool placed = _loadPortManager.GetPlacedState(LoadPortIndex);
                bool present = _loadPortManager.GetPresentState(LoadPortIndex);
                if (placed != present)
                {
                    return false;
                }
            }

            // 4 Carrier가 tool(FIMS 인터페이스)측으로 도킹되어 있거나 앞 또는 뒤로 이동 중일 경우 OFF
            //   -> E84 문서상 Transfer Blocked 상태에서는 Valid가 On이면 HO_AVBL On이다.
            //      하지만 삼성전자 사양에는 Valid보다 HO_AVBL이 먼저 On이다.
            LoadPortTransferStates transferState = LoadPortTransferStates.Unknown;
            if (_loadPortManager.GetLoadPortTransferState(LoadPortIndex, ref transferState))
            {
                if (false == (transferState.Equals(LoadPortTransferStates.ReadyToLoad)
                    || transferState.Equals(LoadPortTransferStates.ReadyToUnload)
                    || (transferState.Equals(LoadPortTransferStates.TransferBlocked) && _loadPortManager.IsPIOInterfaceWorking(LoadPortIndex))))
                    return false;
            }

            // 5 Input용 port에 carrier가 있을 경우 (내부 버퍼 공간이 있는 tool 또는 stocker) OFF
            //   -> Fixed Buffer라 해당되지 않음

            // 6 Load port의 Light curtain 에러가 발생하였을 경우 OFF
            //   -> Light Curtain이 없으므로 해당되지 않음

            // 7 Load port의 Protection Bar 에러가 발생하였을 경우 OFF
            if (IsSaftyInterLockError())
                return false;

            // 8 Active와 통신하는 load port가 아닌 다른 load port에서 Carrier Handling Robot에 에러가 발생할 경우 ON
            //   -> Massage창에서 Retry/Pass로 빠지면 문제없다.

            // 9 AUTO mode상태에서 인위적으로 carrier를 port위로 놓을 경우 OFF
            //   -> Access Violation인 경우이므로, Alarm 발생하여 Off됨

            // 10 ES 신호가 OFF되었을 경우 OFF
            if (false == _loadPortManager.ReadPIOOutput(LoadPortIndex, (int)E84OutputSignals.EmergencyStop))
                return false;

            // 11 Passive측 load port가 공정 진행 중일 경우 OFF
            if (_loadPortManager.GetDockingState(LoadPortIndex) || isCarrierMovingOnLoadPort())
                return false;

            // 12 Load port가 초기화 중인 경우 OFF
            //if (_manualActionToExecute == (Func<int, CommandResults>)_loadPortManager.InitializeLoadPort)
            //    return false;

            // 13 Tool의 메인 프로그램이 고장으로 멈췄을 경우 OFF
            if (EquipmentState_.EquipmentState.GetInstance().GetState().Equals(EquipmentState_.EQUIPMENT_STATE.UNDEFINED) ||
                EquipmentState_.EquipmentState.GetInstance().GetState().Equals(EquipmentState_.EQUIPMENT_STATE.PAUSE))
                return false;

            // 14 순서에 맞지 않는 PI/O 신호가 감지되었을 경우 OFF
            //    -> Chattering에서 감시하여 Alarm 발생함으로 Off됨

            // 15 통신할 수 없는 경우(Timeout Error, EQ Port Error, ES Signal Off, Access Violation Error, Signal Abnormal On / Off) OFF
            //    -> Alarm 발생함으로 Off됨

            // 16 Loadport 와 연관되지 않는 error가 발생한 경우 ON
            //    -> Task가 분리되어 있으므로 중대한 Alarm 발생이 아니면 분리되어 있다고 보는게 맞다.

            // 17 Load 통신이 완료되었을 경우 (COMP. 신호는 TP5구간 동안에 READY신호가 OFF되면 OFF된다) OFF
            //    - 구현되어 있다.

            return true;
        }
        private bool isCarrierMovingOnLoadPort()
        {
            string strActionName = GetRunningAction();
            if (string.IsNullOrEmpty(strActionName))
                return false;

            if (false == _carrierMovementRelatedCommands.ContainsKey(strActionName))
                return false;

            return true;
        }
        protected override bool GetPIOEmergencyStopOn()
        {
            return _loadPortManager.ReadPIOOutput(LoadPortIndex, (int)E84OutputSignals.EmergencyStop);
        }
        protected override bool GetPIOHandOffAvailableOn()
        {
            return _loadPortManager.ReadPIOOutput(LoadPortIndex, (int)E84OutputSignals.HandoffAvailable);
        }
        #endregion </E84>

        #endregion </Internal Interfaces>

        #region <ICarrierServiceCallbacks>
        public void OnCarrierInStarted(CarrierPortCarrierEventArgs e)
        {
            throw new NotImplementedException();
        }

        public void OnCarrierDeleted(CarrierDeletedEventArgs e)
        {
            throw new NotImplementedException();
        }

        public void OnTransferStateChanged(LoadPortStateChangedEventArgs e)
        {
            //throw new NotImplementedException();
        }

        public void OnAccessModeChanged(LoadPortStateChangedEventArgs e)
        {
            //throw new NotImplementedException();
        }

        public void OnVerificationSucceeded(CarrierVerificationSucceededEventArgs e)
        {
            switch (e.VerifyType)
            {
                case VerificationType.Id:
                    {
                        _loadPortManager.PostCarrierIdVerificationResult(LoadPortIndex, true);
                    }
                    break;
                case VerificationType.Slot:
                    {
                        var slotIds = e.SubstrateIds;
                        var substrates = _substrateManager.GetSubstratesAtLoadPort(PortId);
                        for (int i = 0; slotIds != null && i < slotIds.Length; ++i)
                        {
                            var id = slotIds[i];
                            var lotId = e.LotIds[i]; 
                            if (substrates.TryGetValue(i + 1, out var s))
                            {
                                s.Name = id;
                                s.SetAttribute(PWA500SubstrateAttributes.SubstrateType, MySubstrateType.ToString());
                                s.SetAttribute(PWA500SubstrateAttributes.SubstrateSize, MySubstrateSize.ToString());
                                s.LotId = lotId;
                                _substrateManager.SaveDataByKey(s.UniqueKey);
                            }
                        }

                        _loadPortManager.PostCarrierSlotMapVerificationResult(LoadPortIndex, true);
                    }
                    break;
                default:
                    break;
            }
        }

        public void OnVerificationFailed(CarrierVerificationFailedEventArgs e)
        {
            switch (e.VerifyType)
            {
                case VerificationType.Id:
                    {
                        _loadPortManager.PostCarrierIdVerificationResult(LoadPortIndex, false);
                    }
                    break;
                case VerificationType.Slot:
                    {
                        _loadPortManager.PostCarrierSlotMapVerificationResult(LoadPortIndex, false);
                    }
                    break;
                default:
                    break;
            }
        }

        public void OnCarrierInRequestedByHost(HostCarrierRequestEventArgs e)
        {
            throw new NotImplementedException();
        }

        public void OnCarrierOutRequestedByHost(HostCarrierRequestEventArgs e)
        {
            throw new NotImplementedException();
        }

        public void OnCarrierCancelRequestedByHost(HostCarrierRequestEventArgs e)
        {
            _loadPortManager.PostCarrierIdVerificationResult(LoadPortIndex, false);
            _loadPortManager.PostCarrierSlotMapVerificationResult(LoadPortIndex, false);
        }

        public void OnAccessChangeRequestedByHost(HostChangeAccessRequestEventArgs e)
        {
            throw new NotImplementedException();
        }

        public void OnServiceStatusChangeRequestedByHost(HostChangeServiceStatusRequestEventArgs e)
        {
            throw new NotImplementedException();
        }

        public void OnVerificationResultWithoutRemote(CarrierVerificationResultWithoutRemoteArgs e)
        {
            var lotIds = e.LotIds;
            var slotIds = e.SubstrateIds;
            var substrates = _substrateManager.GetSubstratesAtLoadPort(PortId);
            if (substrates != null)
            {
                foreach (var item in substrates)
                {
                    bool changed = false;
                    if (slotIds.TryGetValue(item.Key, out var name))
                    {
                        item.Value.Name = name;
                        changed = true;
                    }

                    if (lotIds.TryGetValue(item.Key, out var lot))
                    {
                        item.Value.LotId = lot;
                        changed = true;
                    }
                    if (changed)
                    {
                        _substrateManager.SaveDataByKey(item.Value.UniqueKey);
                    }
                }
            }

            _loadPortManager.PostCarrierSlotMapVerificationResult(LoadPortIndex, true);
        }
        #endregion </ICarrierServiceCallbacks>

        #endregion </Methods>
    }

    class TaskLoadPortRecovery500W_300 : Work.RecoveryData
    {
        public TaskLoadPortRecovery500W_300(string taskName, int nPortCount)
            : base(taskName, nPortCount)
        {
        }

        #region <Fields>
        private const string KeyAccessStatus = "AccessStatus";
        private CarrierAccessStates _accessStatus;

        private bool _lotCompletionFlag;
        #endregion </Fields>

        #region <Properties>
        public CarrierAccessStates AccessStatus
        {
            get
            {
                return _accessStatus;
            }
            set
            {
                if (false == _accessStatus.Equals(value))
                {
                    _accessStatus = value;
                    //Save();
                }
            }
        }
        public bool LotCompleted
        {
            get
            {
                return _lotCompletionFlag;
            }
            set
            {
                if (false == _lotCompletionFlag.Equals(value))
                {
                    _lotCompletionFlag = value;
                    //Save();
                }
            }
        }
        #endregion </Properties>

        protected override void LoadData(ref FileComposite_.FileComposite fComp, string sRootName)
        {
            string value = string.Empty;
            fComp.GetValue(sRootName, KeyAccessStatus, ref value);
            if (false == Enum.TryParse(value, out _accessStatus))
            {
                AccessStatus = CarrierAccessStates.NotAccessed;
            }
            else
            {
                AccessStatus = _accessStatus;
            }
        }
        protected override void SaveData(ref FileComposite_.FileComposite fComp, string sRootName)
        {
            fComp.AddItem(sRootName, KeyAccessStatus, AccessStatus.ToString());
        }
    }
}