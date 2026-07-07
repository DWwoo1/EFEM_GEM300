using Alarm_;
using EFEM.CustomizedByProcessType.PWA500BIN;
using EFEM.CustomizedByProcessType.PWA500Common;
using EFEM.Defines.Common;
using EFEM.Defines.LoadPort;
using EFEM.MaterialTracking;
using EFEM.Modules.LoadPort.Scheduler;
using FrameOfSystem3.SECSGEM;
using FrameOfSystem3.SECSGEM.DefineSecsGem;
using FrameOfSystem3.SECSGEM.Scenario;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using TickCounter_;

// ConfigTask에서 이 namespace를 가지고 클래스 타입을 가져오기 때문에 변경 불가
namespace FrameOfSystem3.Task
{
    class TaskLoadPortForPWA500BIN : TaskLoadPort
    {
        #region <Constructors>
        public TaskLoadPortForPWA500BIN(int nIndexOfTask, string strTaskName)
            : base(nIndexOfTask, strTaskName, new TaskLoadPortRecovery500BIN(strTaskName, nIndexOfTask))
        {
            if (PortId > 4)
            {
                int coreIndex = _loadPortManager.Count - PortId;
                ScenarioTypeToIdRead = EN_SCENARIO.SCENARIO_RFID_READ_CORE_1 + coreIndex;
                ScenarioTypeToRequestLotInfo = EN_SCENARIO.SCENARIO_REQ_LOT_INFO_CORE_1 + coreIndex;
                ScenarioTypeToSlotVerification = EN_SCENARIO.SCENARIO_REQ_SLOT_INFO_CORE_1 + coreIndex;

                ScenarioTypeToSlotMapping = EN_SCENARIO.SCENARIO_SLOT_WAFER_MAPPING_CORE_1 + coreIndex;
                ScenarioTypeToLotMerge = EN_SCENARIO.SCENARIO_REQ_LOT_MERGE_CORE_1 + coreIndex;

                ScenarioTypeToAdsMoveFlag = EN_SCENARIO.SCENARIO_ADS_MOVE_FLAG_1 + coreIndex;
            }
            else if (PortId == 4)
            {
                ScenarioTypeToIdRead = EN_SCENARIO.SCENARIO_RFID_READ_EMPTY_TAPE;
                ScenarioTypeToRequestLotInfo = EN_SCENARIO.SCENARIO_REQ_LOT_INFO_EMPTY_TAPE;
                ScenarioTypeToSlotVerification = EN_SCENARIO.SCENARIO_REQ_SLOT_INFO_EMPTY_TAPE;

                ScenarioTypeToSlotMapping = EN_SCENARIO.SCENARIO_SLOT_WAFER_MAPPING_EMPTY_TAPE;
            }
            else
            {
                ScenarioTypeToIdRead = EN_SCENARIO.SCENARIO_RFID_READ_BIN_1 + LoadPortIndex;

                // Bin은 없음..
                //ScenarioTypeToRequestLotInfo = ScenarioListTypes.SCENARIO_REQ_LOT_INFO_CORE_1 + LoadPortIndex;
                //ScenarioTypeToSlotVerification = ScenarioListTypes.SCENARIO_REQ_SLOT_INFO_CORE_1 + LoadPortIndex;

                ScenarioTypeToSlotMapping = EN_SCENARIO.SCENARIO_SLOT_WAFER_MAPPING_BIN_1 + LoadPortIndex;
                ScenarioTypeToLotMerge = EN_SCENARIO.SCENARIO_REQ_LOT_ID_MERGE_AND_CHANGE_BIN_1 + LoadPortIndex;
            }

            ScenarioTypeToCarrierLoad = EN_SCENARIO.SCENARIO_PORT_STATUS_LOAD_1 + LoadPortIndex;
            ScenarioTypeToCarrierUnload = EN_SCENARIO.SCENARIO_PORT_STATUS_UNLOAD_1 + LoadPortIndex;

            _functionsForPWA500 = FunctionsForPWA500BIN_TP.Instance;

            _recovery = _recoveryData as TaskLoadPortRecovery500BIN;

            #region <Assign Digital IO>
            int relIndexOutput = LoadPortIndex * 2;
            int indexCassetteOutput = (int)Define.DefineEnumProject.DigitalIO.PWA500BIN.EN_DIGITAL_OUT.LP1_MANUAL_CASSETTE + relIndexOutput;
            int indexFoupOutput = (int)Define.DefineEnumProject.DigitalIO.PWA500BIN.EN_DIGITAL_OUT.LP1_MANUAL_MAC_FOUP + relIndexOutput;

            _loadPortManager.AttachModeChangerEventHandler(LoadPortIndex, LoadPortLoadingMode.Cassette,
                (trigger) => { DigitalIO_.DigitalIO.GetInstance().WriteOutput(indexCassetteOutput, trigger); }
                );

            _loadPortManager.AttachModeChangerEventHandler(LoadPortIndex, LoadPortLoadingMode.Foup,
                (trigger) => { DigitalIO_.DigitalIO.GetInstance().WriteOutput(indexFoupOutput, trigger); }
                );

            int relIndex = LoadPortIndex * 8;
            int indexInputCassetteChanger = (int)Define.DefineEnumProject.DigitalIO.PWA500BIN.EN_DIGITAL_IN.LP1_MANUAL_BUTTON_CASSETTE_STATUS + relIndex;
            int indexInputFoupChanger = (int)Define.DefineEnumProject.DigitalIO.PWA500BIN.EN_DIGITAL_IN.LP1_MANUAL_BUTTON_MAC_FOUP_STATUS + relIndex;

            TriggerChangingMode = new ConcurrentDictionary<LoadPortMode, bool>();
            TriggerChangingMode.TryAdd(new LoadPortMode(LoadPortLoadingMode.Cassette, indexInputCassetteChanger), false);
            TriggerChangingMode.TryAdd(new LoadPortMode(LoadPortLoadingMode.Foup, indexInputFoupChanger), false);

            int indexInputCassetteMode = (int)Define.DefineEnumProject.DigitalIO.PWA500BIN.EN_DIGITAL_IN.LP1_PLACEMENT_CASSETTE_STATUS + relIndex;
            int indexInputFoupMode = (int)Define.DefineEnumProject.DigitalIO.PWA500BIN.EN_DIGITAL_IN.LP1_PLACEMENT_MAC_FOUP_STATUS + relIndex;
            //LoadPortModeSignals = new Dictionary<LoadPortLoadingMode, int>
            //{
            //    { LoadPortLoadingMode.Cassette, indexInputCassetteMode },
            //    { LoadPortLoadingMode.Foup, indexInputFoupMode }
            //};

            //CarrierPresenceIndex = (int)Define.DefineEnumProject.DigitalIO.PWA500BIN.EN_DIGITAL_IN.LP1_PRESENT_STATUS + relIndexInput;

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

            #endregion </Assign Digital IO>

            _lotHistoryLog = LotHistoryLog.Instance;

            _lotHistoryLog.AddLogInfo(PortId, LoadPortName);

            IsOldEvents = NewVersionChecker.IsOldEvents();
        }
        #endregion </Constructors>

        #region <Fields>
        private readonly EN_SCENARIO ScenarioTypeToIdRead;            // 1~6
        private readonly EN_SCENARIO ScenarioTypeToRequestLotInfo;    // 4~6
        private readonly EN_SCENARIO ScenarioTypeToSlotVerification;  // 4~6
        private readonly EN_SCENARIO ScenarioTypeToSlotMapping;       // 1~6
        private readonly EN_SCENARIO ScenarioTypeToLotMerge;          // 1~3, 5~6(1~3은 Change 포함)
        private readonly EN_SCENARIO ScenarioTypeToAdsMoveFlag;
        private readonly EN_SCENARIO ScenarioTypeToCarrierLoad;
        private readonly EN_SCENARIO ScenarioTypeToCarrierUnload;

        private const int CarrierMaxCapacity = 25;
        private const int DelayBeforeIdReadScenario = 3000;

        private CommandResults _commandResult = new CommandResults("", CommandResult.Invalid);
        private static TaskLoadPortRecovery500BIN _recovery;
        string _lotId = string.Empty;
        string _partId = string.Empty;
        string _stepSeq = string.Empty;
        string _lotType = string.Empty;
        string _lotQty = string.Empty;
        string _recipeId = string.Empty;

        private string _toWrite = string.Empty;
        private StepsBeforeSendingCarrier _currentStepBeforeSendingCarrier;
        private EN_SCENARIO _queuedScenarioBeforeSendingCarrier;

        private static FunctionsForPWA500BIN_TP _functionsForPWA500 = null;
        private static LotHistoryLog _lotHistoryLog = null;
        private readonly ConcurrentDictionary<LoadPortMode, bool> TriggerChangingMode;
        private readonly Dictionary<string, TASK_ACTION> _carrierMovementRelatedCommands;

        private string _lastCompletionConditionKey;
        #endregion </Fields>

        #region <Properties>
        bool NeedExecuteToScenario
        {
            get
            {
                return _carrierServer.GetCarrierAccessingStatus(PortId) == CarrierAccessStates.NotAccessed;
            }
        }
        SubstrateType MySubstrateType
        {
            get
            {
                return _functionsForPWA500.GetSubstrateTypeByLoadPortIndex(LoadPortIndex);
            }
        }
        private bool IsOldEvents { get; }
        #endregion </Properties>

        #region <Methods>

        #region <Overrides>
        protected override void GetCompletionCondition(
            out ICarrierCompletionHandlingPolicy policy,
            out ICarrierCompletionCondition condition)
        {
            if (false == (_completionPolicy is ICarrierCompletionHandlingPolicy))
            {
                _completionPolicy = new DefaultCarrierCompletionHandlingPolicy();
            }

            string currentCompletionConditionKey = MakeCompletionConditionKey();

            bool isCompletionConditionChanged =
                _lastCompletionConditionKey != null &&
                false == string.Equals(
                    _lastCompletionConditionKey,
                    currentCompletionConditionKey,
                    StringComparison.Ordinal);

            if (isCompletionConditionChanged)
            {
                _completionPolicy.ResetCarrierCompletionRequest(PortId);
                _completionCondition = null;
            }

            switch (MySubstrateType)
            {
                case SubstrateType.Empty:
                    if (false == (_completionCondition is CarrierEmptiedCompletionCondition))
                    {
                        _completionCondition = new CarrierEmptiedCompletionCondition();
                    }
                    break;

                case SubstrateType.Bin1:
                case SubstrateType.Bin2:
                case SubstrateType.Bin3:
                    if (false == (_completionCondition is CapacityLimitCarrierCompletionCondition))
                    {
                        Recipe.PARAM_EQUIPMENT paramUseCapacity =
                            Recipe.PARAM_EQUIPMENT.UseCapacityLimitBin1 + LoadPortIndex;

                        Recipe.PARAM_EQUIPMENT paramCapacityLimit =
                            Recipe.PARAM_EQUIPMENT.AvailableCarrierCapacityBin1 + LoadPortIndex;

                        _completionCondition = new CapacityLimitCarrierCompletionCondition(
                            LoadPortIndex,
                            paramUseCapacity.ToString(),
                            paramCapacityLimit.ToString());
                    }
                    break;

                default:
                    if (false == (_completionCondition is DefaultCarrierCompletionCondition))
                    {
                        _completionCondition = new DefaultCarrierCompletionCondition();
                    }
                    break;
            }

            _lastCompletionConditionKey = currentCompletionConditionKey;

            policy = _completionPolicy;
            condition = _completionCondition;
        }

        protected override bool GetBusyIndex(int lpIndex, ref int indexOfDigital)
        {
            int relIndex = lpIndex * 8;
            indexOfDigital = (int)Define.DefineEnumProject.DigitalIO.PWA500BIN.EN_DIGITAL_IN.LP1_RUN + relIndex;

            return true;
        }
        protected override void ExecuteAtAlways()
        {
            if (EquipmentState_.EquipmentState.GetInstance().GetState().Equals(EquipmentState_.EQUIPMENT_STATE.IDLE) ||
                EquipmentState_.EquipmentState.GetInstance().GetState().Equals(EquipmentState_.EQUIPMENT_STATE.PAUSE))
            {
                foreach (var item in TriggerChangingMode)
                {
                    if (item.Value != DigitalIO_.DigitalIO.GetInstance().ReadInput(item.Key.DigitalInputIndex))
                    {
                        TriggerChangingMode[item.Key] = !item.Value;
                        if (TriggerChangingMode[item.Key])
                        {
                            ChangingModeButtonClicked(item.Key.LoadingType);
                        }
                        //_loadPortManager.ChangeLoadPortMode(MyLoadPortIndex, item.Key.LoadingType);
                    }
                }
            }
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

            if (false == NeedExecuteToScenario)
                return false;

            InitResult(ScenarioTypeToIdRead);

            string carrierId = _carrierServer.GetCarrierId(PortId);
            _lotHistoryLog.ClearPreviousHistory(PortId, carrierId, LoadPortName);

            var param = new Dictionary<string, string>
            {
                [RFIDReadKeys.KeyParamLotId] = _carrierServer.GetCarrierLotId(PortId),
                [RFIDReadKeys.KeyParamCarrierId] = carrierId,
                [RFIDReadKeys.KeyParamPortId] = EquipmentInfo.GetPortName(PortId),
                [RFIDReadKeys.KeyParamOperatorId] = "AUTO"
            };

            // 2024.12.29. jhlim [ADD] 고객사 요청으로 id read 딜레이 추가
            SetDelayForSequence(DelayBeforeIdReadScenario);

            return UpdateScenarioParam(ScenarioTypeToIdRead, param);
        }
        protected override CommandResults ExecuteScenarioToCarrierIdRead()
        {
            return RunScenario(ScenarioTypeToIdRead);
        }
        protected override bool UpdateParamToIdVarification()
        {
            //if (PortId < 4)
            if (IsBinType(MySubstrateType))
                return false;

            if (false == _scenarioOperator.UseScenario)
                return false;

            if (false == _carrierServer.HasCarrier(PortId))
                return false;

            if (false == NeedExecuteToScenario)
                return false;

            InitResult(ScenarioTypeToRequestLotInfo);

            var param = new Dictionary<string, string>
            {
                [LotInfoKeys.KeyParamLotId] = _carrierServer.GetCarrierLotId(PortId),
                [LotInfoKeys.KeyParamCarrierId] = _carrierServer.GetCarrierId(PortId),
            };
            return _scenarioOperator.UpdateScenarioParam(GetTaskName(), ScenarioTypeToRequestLotInfo, param);
        }
        protected override CommandResults ExecuteScenarioToIdVarification()
        {
            //if (PortId < 4)
            if (IsBinType(MySubstrateType))
            {
                _commandResult.ActionName = "Idle";
                _commandResult.CommandResult = CommandResult.Completed;
                _commandResult.Description = string.Empty;
                return _commandResult;
            }

            if (false == _carrierServer.HasCarrier(PortId))
            {
                _commandResult.CommandResult = CommandResult.Error;
                _commandResult.Description = "Does not have carrier";
                return _commandResult;
            }

            _commandResult = RunScenario(ScenarioTypeToRequestLotInfo);

            return _commandResult;
        }
        protected override bool UpdateParamToSlotMapVarification()
        {
            //if (PortId < 4 && _carrierServer.GetCarrierAccessingStatus(PortId).Equals(CarrierAccessStates.NotAccessed))
            //{
            //    // 1~3 포트의 경우, 작업하지 않았으면 비어있어야함
            //    if (_loadPortManager.IsLoadPortSimulationMode(LoadPortIndex))
            //    {
            //        _substrateManager.RemoveSubstrateAtLoadPortAll(PortId);
            //    }
            //}
            if (_loadPortManager.IsLoadPortSimulationMode(LoadPortIndex))
            {
                InitializeSlotInfoAtSimulationMode();
            }

            //if (PortId < 4)
            if (IsBinType(MySubstrateType))
            {
                return false;
            }

            if (false == _carrierServer.HasCarrier(PortId))
                return false;

            if (false == _scenarioOperator.UseScenario)
            {
                string lotId = _carrierServer.GetCarrierLotId(PortId);
                _functionsForPWA500.AssignSubstrateInfoByCarrierRFIDInfo(PortId, lotId);

                return false;
            }

            if (false == NeedExecuteToScenario)
                return false;

            InitResult(ScenarioTypeToSlotVerification);

            var param = new Dictionary<string, string>
            {
                [LotInfoKeys.KeyParamLotId] = _carrierServer.GetCarrierLotId(PortId),
                [LotInfoKeys.KeyParamCarrierId] = _carrierServer.GetCarrierId(PortId),
            };

            return UpdateScenarioParam(ScenarioTypeToSlotVerification, param);
        }
        protected override CommandResults ExecuteToSlotMapVarification()
        {
            //if (PortId < 4)
            if (IsBinType(MySubstrateType))
            {
                _commandResult.ActionName = "Idle";
                _commandResult.CommandResult = CommandResult.Completed;
                _commandResult.Description = string.Empty;

                return _commandResult;
            }

            if (false == _carrierServer.HasCarrier(PortId))
            {
                _commandResult.CommandResult = CommandResult.Error;
                _commandResult.Description = "Does not have carrier";
                return _commandResult;
            }

            _commandResult = RunScenario(ScenarioTypeToSlotVerification);
            return _commandResult;
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
            scenarioInfo = new QueuedScenarioInfo();

            if (false == _scenarioOperator.UseScenario)
                return false;

            if (false == _carrierServer.GetCarrierAccessingStatus(PortId).Equals(CarrierAccessStates.CarrierCompleted))
                return false;

            Dictionary<string, string> scenarioParam = new Dictionary<string, string>();

            // 2025.03.17. jhlim [MOD] 메서드로 변경
            bool needExecuteMerge = HasSubstrateToMergeOrSlotMapping();
            //switch (MySubstrateType)
            //{
            //    case SubstrateType.Core:
            //        {
            //            needExecuteMerge = GetSubstrateToMerge(out _);
            //        }
            //        break;
            //    case SubstrateType.Empty:
            //        needExecuteMerge = false;
            //        break;
            //    default:
            //        needExecuteMerge = true;
            //        break;
            //}
            // 2025.03.17. jhlim [END]

            // 2025.03.17. jhlim [MOD] 고객사 요청으로 SlotMapping도 Merge와 동일하게 모든 웨이퍼가 터미네이트 되면 SlotMapping 진행하지 않도록 변경
            // 이로써 슬롯매핑은 머지 실행 이후에 하도록 하며, EmptyCarrier의 경우 SlotMapping 하지 않도록 한다.
            if (false == needExecuteMerge)
            {
                // 터미네이트 되는 경우 History Log가 넘어가지 않는다..
                var isCore = MySubstrateType.Equals(SubstrateType.Core);
                var carrierId = _carrierServer.GetCarrierId(PortId);
                var lotId = _carrierServer.GetCarrierLotId(PortId);
                var subs = _substrateManager.GetSubstratesAtLoadPort(PortId);
                List<string> substrateNames = new List<string>();
                foreach (var item in subs)
                {
                    substrateNames.Add(item.Value.Name);
                }

                _lotHistoryLog.BackupCarrierHistory(PortId, carrierId, lotId, substrateNames, isCore);

                // 랏 머지를 진행하지 않으니 랏 머지 완료로 처리한다.
                _carrierServer.SetAttribute(PortId, PWA500CarrierAttributes.KeyProcessStepBeforeSendingCarrier, ((int)StepsBeforeSendingCarrier.MergeAndChangeCompleted).ToString());
                _carrierServer.SaveCarrierData(PortId);

                return false;
            }
            else
            {
                // 2025.06.11. jhlim [MOD] 코어만 -> 공테이프가 아니면으로 조건 확장
                // 코어의 경우 이미 머지됐는지 확인한다.. -> 확인 후 이미 머지되었으면 false 리턴하여 스킵
                if (false == MySubstrateType.Equals(SubstrateType.Empty))
                {
                    if (IsAlreadyLotMerged())
                        return false;
                }
                // 2025.06.11. jhlim [END]

                if (false == MakeScenarioParamForMergeLot(ref scenarioParam))
                    return false;

                scenarioInfo.Scenario = ScenarioTypeToLotMerge;
            }
            //if (false == needExecuteMerge)
            //{
            //    if (false == MakeScenarioParamForSlotMapping(ref scenarioParam))
            //        return false;

            //    scenarioInfo.Scenario = ScenarioTypeToSlotMapping;
            //}
            //else
            //{
            //    if (false == MakeScenarioParamForMergeLot(ref scenarioParam))
            //        return false;

            //    scenarioInfo.Scenario = ScenarioTypeToLotMerge;
            //}
            // 2025.03.17. jhlim [END]

            #region <Old>
            //// 2024.09.29. jhlim [MOD] 고객사 요청으로 순서 변경
            //// 코어는 머지 -> 슬롯매핑, 공테이프는 슬롯매핑, 빈은 머지&체인지 -> 슬롯매핑 순으로 진행한다.
            ////if (false == MakeScenarioParamForSlotMapping(ref scenarioParam))
            ////    return false;
            ////scenarioInfo.Scenario = ScenarioTypeToSlotMapping;
            //if (MySubstrateType.Equals(SubstrateType.Empty))
            //{
            //    if (false == MakeScenarioParamForSlotMapping(ref scenarioParam))
            //        return false;

            //    scenarioInfo.Scenario = ScenarioTypeToSlotMapping;
            //}
            //else
            //{
            //    if (false == MakeScenarioParamForMergeLot(ref scenarioParam))
            //        return false;

            //    scenarioInfo.Scenario = ScenarioTypeToLotMerge;
            //}
            //// 2024.09.29. jhlim [END]
            #endregion </Old>

            scenarioInfo.ScenarioParams = scenarioParam;

            return true;
        }
        protected override void ExecuteAfterScenarioCompletion(EN_SCENARIO scenario, EN_SCENARIO_RESULT result, Dictionary<string, string> scenarioParam, Dictionary<string, string> additionalParams)
        {
            switch (scenario)
            {
                case EN_SCENARIO.SCENARIO_REQ_LOT_MERGE_CORE_1:
                case EN_SCENARIO.SCENARIO_REQ_LOT_MERGE_CORE_2:
                case EN_SCENARIO.SCENARIO_REQ_LOT_ID_MERGE_AND_CHANGE_BIN_1:
                case EN_SCENARIO.SCENARIO_REQ_LOT_ID_MERGE_AND_CHANGE_BIN_2:
                case EN_SCENARIO.SCENARIO_REQ_LOT_ID_MERGE_AND_CHANGE_BIN_3:
                    {
                        if (false == result.Equals(EN_SCENARIO_RESULT.COMPLETED))
                        {
                            return;
                        }

                        if (false == MySubstrateType.Equals(SubstrateType.Empty))
                        {
                            #region <머지할 랏을 받아온다.>
                            var scenarioResult = _scenarioOperator.GetScenarioResultData(GetTaskName(), scenario);
                            if (false == ApplyResultOfMergingLot(scenarioResult))
                                return;
                            #endregion </머지할 랏을 받아온다.>

                            // 랏 머지 완료 처리
                            _carrierServer.SetAttribute(PortId, PWA500CarrierAttributes.KeyProcessStepBeforeSendingCarrier, ((int)StepsBeforeSendingCarrier.MergeAndChangeCompleted).ToString());
                            _carrierServer.SaveCarrierData(PortId);

                            // 2025.03.17. jhlim [MOD] 고객사 요청으로 SlotMapping도 Merge와 동일하게 모든 웨이퍼가 터미네이트 되면 SlotMapping 진행하지 않도록 변경
                            #region <슬롯매핑 실행 조건 체크>
                            if (false == HasSubstrateToMergeOrSlotMapping())
                            {
                                // 슬롯 매핑 완료 처리
                                _carrierServer.SetAttribute(PortId, PWA500CarrierAttributes.KeyProcessStepBeforeSendingCarrier, ((int)StepsBeforeSendingCarrier.SlotMappingCompleted).ToString());
                                _carrierServer.SaveCarrierData(PortId);

                                if (MySubstrateType == SubstrateType.Core)
                                {
                                    // ADS Move Flag 이벤트 발생
                                    ExecuteMovingAdsScenario();
                                }
                                else
                                {
                                    // ADS MOVE FLAG 이벤트 완료 처리
                                    _carrierServer.SetAttribute(PortId, PWA500CarrierAttributes.KeyProcessStepBeforeSendingCarrier, ((int)StepsBeforeSendingCarrier.MovingAdsCompleted).ToString());
                                    _carrierServer.SaveCarrierData(PortId);
                                }
                                    
                                return;
                            }
                            #endregion </슬롯매핑 실행 조건 체크>
                            // 2025.03.17. jhlim [END]

                            // 2024.09.29. jhlim [MOD] 고객사 요청으로 순서 변경(랏 머지&체인지 후 매핑 진행)
                            Dictionary<string, string> param = new Dictionary<string, string>();
                            if (false == MakeScenarioParamForSlotMapping(ref param))
                                return;

                            EnqueueScenario(ScenarioTypeToSlotMapping, param, null);
                            // 2024.09.29. jhlim [END]

                            // 2024.09.29. jhlim [DEL] LotId Change는 Merge에 병합됨
                            //Dictionary<string, string> param = new Dictionary<string, string>();
                            //if (MakeScenarioParamForChangeToLotId(ref param))
                            //{
                            //    EnqueueScenario(ScenarioTypeToLotIdChange, param, null);
                            //}
                        }
                    }
                    break;

                case EN_SCENARIO.SCENARIO_SLOT_WAFER_MAPPING_CORE_1:
                case EN_SCENARIO.SCENARIO_SLOT_WAFER_MAPPING_CORE_2:
                case EN_SCENARIO.SCENARIO_SLOT_WAFER_MAPPING_EMPTY_TAPE:
                case EN_SCENARIO.SCENARIO_SLOT_WAFER_MAPPING_BIN_1:
                case EN_SCENARIO.SCENARIO_SLOT_WAFER_MAPPING_BIN_2:
                case EN_SCENARIO.SCENARIO_SLOT_WAFER_MAPPING_BIN_3:
                    {
                        if (false == result.Equals(EN_SCENARIO_RESULT.COMPLETED))
                            return;

                        // 슬롯 매핑 완료 처리
                        _carrierServer.SetAttribute(PortId, PWA500CarrierAttributes.KeyProcessStepBeforeSendingCarrier, ((int)StepsBeforeSendingCarrier.SlotMappingCompleted).ToString());
                        _carrierServer.SaveCarrierData(PortId);

                        #region <히스토리 정리>
                        string carrierId = _carrierServer.GetCarrierId(PortId);
                        List<string> substrates = null;
                        if (MySubstrateType != SubstrateType.Empty)
                        {
                            var temporarySubstrates = _substrateManager.GetSubstratesAtLoadPort(PortId);
                            if (temporarySubstrates != null)
                            {
                                substrates = new List<string>();
                                foreach (var item in temporarySubstrates)
                                {
                                    substrates.Add(item.Value.Name);
                                }
                            }
                        }

                        //bool isCore = scenario.Equals(EN_SCENARIO.SCENARIO_SLOT_WAFER_MAPPING_CORE_1) ||
                        //    scenario.Equals(EN_SCENARIO.SCENARIO_SLOT_WAFER_MAPPING_CORE_2);
                        bool isCore = MySubstrateType == SubstrateType.Core;
                        string lotId = _carrierServer.GetCarrierLotId(PortId);
                        if (false == isCore)
                        {
                            lotId = _toWrite;
                        }
                        _lotHistoryLog.BackupCarrierHistory(PortId, carrierId, lotId, substrates, isCore);
                        #endregion </히스토리 정리>

                        if (MySubstrateType == SubstrateType.Core)
                        {
                            if (false == result.Equals(EN_SCENARIO_RESULT.COMPLETED))
                                return;

                            // ADS Move Flag 이벤트 발생
                            ExecuteMovingAdsScenario();
                        }
                    }
                    break;
                case EN_SCENARIO.SCENARIO_ADS_MOVE_FLAG_1:
                case EN_SCENARIO.SCENARIO_ADS_MOVE_FLAG_2:
                    {
                        if (false == result.Equals(EN_SCENARIO_RESULT.COMPLETED))
                            return;

                        // ADS MOVE FLAG 이벤트 완료 처리
                        _carrierServer.SetAttribute(PortId, PWA500CarrierAttributes.KeyProcessStepBeforeSendingCarrier, ((int)StepsBeforeSendingCarrier.MovingAdsCompleted).ToString());
                        _carrierServer.SaveCarrierData(PortId);

                        // 중요하지 않은 이벤트니 비동기 실행한다.
                        string lotId = _carrierServer.GetCarrierLotId(PortId);
                        string partId = _carrierServer.GetAttribute(PortId, PWA500CarrierAttributes.KeyPartId);
                        string stepId = _carrierServer.GetAttribute(PortId, PWA500CarrierAttributes.KeyStepSeq);
                        string lotType = _carrierServer.GetAttribute(PortId, PWA500CarrierAttributes.KeyLotType);

                        _functionsForPWA500.ExecuteScenarioAsyncToCarrierUnload(lotId, partId, stepId, lotType);
                    }
                    break;

                default:
                    break;
            }
        }
        protected override bool UpdateParamToLoadCarrier()
        {
            switch (MySubstrateType)
            {
                case SubstrateType.Core:
                case SubstrateType.Empty:
                    {
                        _commandResult.ActionName = ScenarioTypeToCarrierLoad.ToString();
                        var loadingMode = _loadPortManager.GetCarrierLoadingType(LoadPortIndex);
                        EN_SCENARIO typeOfScenario = ScenarioTypeToCarrierLoad;

                        _functionsForPWA500.EnqueueScenarioCarrierHandlingAsync(PortId, loadingMode, string.Empty, typeOfScenario);
                        return true;
                        //var param = ScenarioParameterBuilder.MakeParamToOHTHandling(PortId, _loadingMode, string.Empty, typeOfScenario);
                        //return _scenarioOperator.UpdateScenarioParam(GetTaskName(), ScenarioTypeToCarrierLoad, param);
                    }

                case SubstrateType.Bin1:
                case SubstrateType.Bin2:
                case SubstrateType.Bin3:
                    {
                        LoadPortLoadingMode loadingMode = LoadPortLoadingMode.Unknown;
                        for (int i = 0; i < _loadPortManager.Count; ++i)
                        {
                            var substrateType = _functionsForPWA500.GetSubstrateTypeByLoadPortIndex(i);
                            if (false == substrateType.Equals(SubstrateType.Core))
                                continue;

                            int portId = _loadPortManager.GetLoadPortPortId(i);
                            if (false == _carrierServer.HasCarrier(portId))
                                continue;

                            loadingMode = _loadPortManager.GetCarrierLoadingType(i);
                            break;
                        }

                        // 아직 Core 캐리어가 도착하지 않은 거다..
                        if (loadingMode.Equals(LoadPortLoadingMode.Unknown))
                            return false;

                        string binLotId = string.Empty;
                        //switch (loadingMode)
                        //{
                        //    case LoadPortLoadingMode.Foup:
                        //        binLotId = GetParameter(Recipe.PARAM_EQUIPMENT.WrittingLotIdToMACWhenCarrierIsEmpty, "PEMAC");
                        //        break;
                        //    case LoadPortLoadingMode.Cassette:
                        //        binLotId = GetParameter(Recipe.PARAM_EQUIPMENT.WrittingLotIdToCassetteWhenCarrierIsEmpty, "ECASSETTE");
                        //        break;
                        //    case LoadPortLoadingMode.ClosedCassette:
                        //        binLotId = GetParameter(Recipe.PARAM_EQUIPMENT.WrittingLotIdToClosedCassetteWhenCarrierIsEmpty, "IECASSETTE");
                        //        break;
                        //    default:
                        //        break;
                        //}
                        
                        if (loadingMode == LoadPortLoadingMode.Foup)
                        {
                            // 로딩/배출 이름이 달라야 하는것같다. 일단 맥은 PEMAC으로 요청
                            binLotId = CarrierLotIdType.PEMAC.ToString();//GetParameter(Recipe.PARAM_EQUIPMENT.WrittingLotIdToMACWhenCarrierIsEmpty, "PEMAC");
                        }
                        else
                        {
                            binLotId = GetParameter(Recipe.PARAM_EQUIPMENT.WrittingLotIdToCassetteWhenCarrierIsEmpty, "ECASSETTE");
                        }

                        //switch (loadingMode)
                        //{
                        //    case LoadPortLoadingMode.Cassette:
                        //    case LoadPortLoadingMode.ClosedCassette:
                        //        {
                        //            binLotId = CarrierLotIdType.ECASSETTE.ToString();
                        //        }
                        //        break;
                        //    case LoadPortLoadingMode.Foup:
                        //        {
                        //            binLotId = CarrierLotIdType.PEMAC.ToString();
                        //        }
                        //        break;
                        //}

                        _commandResult.ActionName = ScenarioTypeToCarrierLoad.ToString();
                        var loadingModeForBinCarrier = _loadPortManager.GetCarrierLoadingType(LoadPortIndex);
                        EN_SCENARIO typeOfScenario = ScenarioTypeToCarrierLoad;

                        _functionsForPWA500.EnqueueScenarioCarrierHandlingAsync(PortId, loadingModeForBinCarrier, binLotId, typeOfScenario);
                        return true;
                    }

                default:
                    break;
            }

            return false;
        }
        protected override CommandResults ExecuteScenarioToLoadCarrier()
        {
            _commandResult.CommandResult = CommandResult.Completed;
            return _commandResult;
        }
        protected override bool UpdateParamToUnloadCarrier()
        {
            _commandResult.ActionName = ScenarioTypeToCarrierUnload.ToString();
            var loadingMode = _loadPortManager.GetCarrierLoadingType(LoadPortIndex);
            EN_SCENARIO typeOfScenario = ScenarioTypeToCarrierUnload;
            string carrierLotId = _carrierServer.GetCarrierLotId(PortId);

            var param = ScenarioParameterBuilder.MakeParamToOHTHandling(PortId, loadingMode, carrierLotId, typeOfScenario);

            return UpdateScenarioParam(ScenarioTypeToCarrierUnload, param);
        }
        protected override CommandResults ExecuteScenarioToUnloadCarrier()
        {
            var result = _scenarioOperator.ExecuteScenario(GetTaskName(), ScenarioTypeToCarrierUnload);
            switch (result)
            {
                case EN_SCENARIO_RESULT.WAITING:
                case EN_SCENARIO_RESULT.PROCEED:
                    _commandResult.CommandResult = CommandResult.Proceed;
                    break;
                case EN_SCENARIO_RESULT.COMPLETED:
                    _commandResult.CommandResult = CommandResult.Completed;
                    break;
                case EN_SCENARIO_RESULT.ERROR:
                    _commandResult.CommandResult = CommandResult.Error;
                    _commandResult.Description = "Scenario Error";
                    break;
                case EN_SCENARIO_RESULT.TIMEOUT_ERROR:
                    _commandResult.CommandResult = CommandResult.Timeout;
                    _commandResult.Description = "Scenario Timeout";
                    break;
                default:
                    break;
            }

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
        private void InitScenarioBeforeSendingCarrier()
        {
            _queuedScenarioBeforeSendingCarrier = EN_SCENARIO.FdcUpdate;
            _currentStepBeforeSendingCarrier = (int)StepsBeforeSendingCarrier.Init;
        }

        private CommandResults ApplyResultAfterExecution(Dictionary<string, string> result)
        {
            if (result == null)
            {
                return new CommandResults(
                    _queuedScenarioBeforeSendingCarrier.ToString(),
                    CommandResult.Error,
                    "Invalid scenario result");
            }

            if (false == ApplyResultOfMergingLot(result))
            {
                return new CommandResults(
                    _queuedScenarioBeforeSendingCarrier.ToString(),
                    CommandResult.Error,
                    "Failed to apply scenario result");
            }

            return new CommandResults(string.Empty, CommandResult.Completed);
        }

        // Proceed : 현재 스텝에 머뭄(재귀호출)
        // Skipped : Execute 하지 않음(사실상 완료 조건)
        // Completed : Execute 실행
        // 기타 에러 : 알람 발생
        private CommandResults UpdateScenarioBeforeSendingCarrier()
        {
            #region <기본 스킵 조건 체크>
            // 시나리오 사용 유무 체크
            if (false == _scenarioOperator.UseScenario)
            {
                return new CommandResults(string.Empty, CommandResult.Skipped, "Scenario is unused");
            }

            // 캐리어 완료 상태 체크
            var accessingStatus = _carrierServer.GetCarrierAccessingStatus(PortId);
            if (false == accessingStatus.Equals(CarrierAccessStates.CarrierCompleted))
            {
                return new CommandResults(string.Empty, CommandResult.Skipped, "Carrier is not completed");
            }
            #endregion </기본 스킵 조건 체크>

            // 3. 스텝 상태 체크
            var stepString = _carrierServer.GetAttribute(PortId, PWA500CarrierAttributes.KeyProcessStepBeforeSendingCarrier);
            if (int.TryParse(stepString, out int stepInt) &&
                Enum.IsDefined(typeof(StepsBeforeSendingCarrier), stepInt))
            {
                _currentStepBeforeSendingCarrier = (StepsBeforeSendingCarrier)stepInt;
            }
            else
            {
                _currentStepBeforeSendingCarrier = default;
            }

            // 스텝별로 실행할 내용을 아래 switch-case에 넣는다.
            switch (_currentStepBeforeSendingCarrier)
            {
                case StepsBeforeSendingCarrier.Init:
                    {
                        // 1) EnqueueScenraioBeforeActionCompletion 에서 하던 내용
                        Dictionary<string, string> scenarioParam = new Dictionary<string, string>();

                        // 2025.03.17. jhlim [MOD] 메서드로 변경
                        bool needExecuteMerge = HasSubstrateToMergeOrSlotMapping();

                        // 2025.03.17. jhlim [MOD] 고객사 요청으로 SlotMapping도 Merge와 동일하게 모든 웨이퍼가 터미네이트 되면 SlotMapping 진행하지 않도록 변경
                        // 이로써 슬롯매핑은 머지 실행 이후에 하도록 하며, EmptyCarrier의 경우 SlotMapping 하지 않도록 한다.
                        if (false == needExecuteMerge)
                        {
                            #region <머지할 필요 없는 경우>
                            // 히스토리 정리 및 슬롯 매핑 패스하도록 스텝 증가

                            // 터미네이트 되는 경우 History Log가 넘어가지 않는다..
                            var isCore = MySubstrateType != SubstrateType.Empty;
                            var carrierId = _carrierServer.GetCarrierId(PortId);
                            var lotId = _carrierServer.GetCarrierLotId(PortId);
                            var subs = _substrateManager.GetSubstratesAtLoadPort(PortId);
                            List<string> substrateNames = new List<string>();
                            foreach (var item in subs)
                            {
                                substrateNames.Add(item.Value.Name);
                            }

                            _lotHistoryLog.BackupCarrierHistory(PortId, carrierId, lotId, substrateNames, isCore);

                            // 머지할게 없다 -> 슬롯매핑까지 진행하지 않는다.
                            _carrierServer.SetAttribute(PortId, PWA500CarrierAttributes.KeyProcessStepBeforeSendingCarrier, ((int)StepsBeforeSendingCarrier.SlotMappingCompleted).ToString());
                            _carrierServer.SaveCarrierData(PortId);
                            #endregion </머지할 필요 없는 경우>

                            // 스킵하여 해당 메서드를 재귀 호출
                            return new CommandResults(string.Empty, CommandResult.Proceed, "Lot Merge and SlotMapping Skipped");
                        }
                        else
                        {
                            #region <머지 진행하는 경우>
                            // 2026.03.24. jhlim [MOD] 아래 조건은 스텝에 의해 제어되므로 필요없을 것이지만, 혹시 몰라 추가해놓음
                            // 2025.06.11. jhlim [MOD] 코어만 -> 공테이프가 아니면으로 조건 확장
                            // 코어의 경우 이미 머지됐는지 확인한다.. -> 확인 후 이미 머지되었으면 false 리턴하여 스킵
                            if (MySubstrateType != SubstrateType.Empty)
                            {
                                if (IsAlreadyLotMerged())
                                {
                                    // 스킵하므로 
                                    _carrierServer.SetAttribute(PortId, PWA500CarrierAttributes.KeyProcessStepBeforeSendingCarrier, ((int)StepsBeforeSendingCarrier.MergeAndChangeCompleted).ToString());
                                    _carrierServer.SaveCarrierData(PortId);

                                    return new CommandResults(string.Empty, CommandResult.Proceed, "Lot Merge Skipped");
                                }
                            }
                            // 2025.06.11. jhlim [END]
                            // 2026.03.24. jhlim [END] 

                            if (false == MakeScenarioParamForMergeLot(ref scenarioParam))
                                return new CommandResults(string.Empty, CommandResult.Error, "MakeScenarioParam for Merge Error");
                            #endregion </머지 진행하는 경우>

                            _queuedScenarioBeforeSendingCarrier = ScenarioTypeToLotMerge;
                            UpdateScenarioParam(ScenarioTypeToLotMerge, scenarioParam);

                            return new CommandResults(string.Empty, CommandResult.Completed);
                        }
                    }
                    break;
                case StepsBeforeSendingCarrier.MergeAndChangeCompleted:
                    {
                        // 2) ExecuteAfterScenarioCompletion
                        //  - SlotMapping

                        // 2025.03.17. jhlim [MOD] 고객사 요청으로 SlotMapping도 Merge와 동일하게 모든 웨이퍼가 터미네이트 되면 SlotMapping 진행하지 않도록 변경
                        #region <슬롯매핑 실행 조건 체크>
                        if (false == HasSubstrateToMergeOrSlotMapping())
                        {
                            // 슬롯 매핑 완료 처리
                            _carrierServer.SetAttribute(PortId, PWA500CarrierAttributes.KeyProcessStepBeforeSendingCarrier, ((int)StepsBeforeSendingCarrier.SlotMappingCompleted).ToString());
                            _carrierServer.SaveCarrierData(PortId);

                            return new CommandResults(string.Empty, CommandResult.Proceed, "SlotMapping Skipped");
                        }
                        #endregion </슬롯매핑 실행 조건 체크>
                        // 2025.03.17. jhlim [END]

                        Dictionary<string, string> param = new Dictionary<string, string>();
                        if (false == MakeScenarioParamForSlotMapping(ref param))
                            return new CommandResults(string.Empty, CommandResult.Error, "MakeScenarioParam for SlotMapping Error");

                        _queuedScenarioBeforeSendingCarrier = ScenarioTypeToSlotMapping;
                        UpdateScenarioParam(ScenarioTypeToSlotMapping, param);
                        //_queuedScenarioBeforeSendingCarrier.Scenario = ScenarioTypeToSlotMapping;
                        //_queuedScenarioBeforeSendingCarrier.ScenarioParams = param;

                        return new CommandResults(string.Empty, CommandResult.Completed);
                    }
                    break;
                case StepsBeforeSendingCarrier.SlotMappingCompleted:
                    {
                        // 3) WriteTag
                        // 시나리오 실행부에서 모두 처리되었을 것이고, 태그쪽은 이벤트 기반이 아니므로 여기서 처리할 것은 없다.
                        return new CommandResults(string.Empty, CommandResult.Completed);
                    }
                    break;
                case StepsBeforeSendingCarrier.WriteTag:
                    {
                        // 태깅 완료 후 이제 넘어간다.
                        return new CommandResults(string.Empty, CommandResult.Skipped);
                    }
                    break;
                default:
                    _commandResult.CommandResult = CommandResult.Skipped;
                    return _commandResult;
            }

            //_commandResult.CommandResult = CommandResult.Completed;
            //return _commandResult;
        }

        // Proceed : 현재 스텝에 머뭄(재귀호출)
        // Skipped/Completed : 이전 스텝으로 돌아감
        // 기타 : 알람 발생
        private CommandResults ExecuteScenarioBeforeSendingCarrier()
        {
            if (_queuedScenarioBeforeSendingCarrier == null)
                return new CommandResults(string.Empty, CommandResult.Skipped);

            if (_currentStepBeforeSendingCarrier == StepsBeforeSendingCarrier.SlotMappingCompleted)
            {
                return new CommandResults(string.Empty, CommandResult.Completed);
            }
            else
            {
                var result = _scenarioOperator.ExecuteScenario(GetTaskName(), _queuedScenarioBeforeSendingCarrier);
                switch (result)
                {
                    case EN_SCENARIO_RESULT.WAITING:
                    case EN_SCENARIO_RESULT.PROCEED:
                        return new CommandResults(_queuedScenarioBeforeSendingCarrier.ToString(), CommandResult.Proceed);

                    case EN_SCENARIO_RESULT.COMPLETED:
                        {
                            // ExecuteAfterScenarioCompletion 에서 하던 행위들
                            switch (_queuedScenarioBeforeSendingCarrier)
                            {
                                case EN_SCENARIO.SCENARIO_REQ_LOT_MERGE_CORE_1:
                                case EN_SCENARIO.SCENARIO_REQ_LOT_MERGE_CORE_2:
                                case EN_SCENARIO.SCENARIO_REQ_LOT_ID_MERGE_AND_CHANGE_BIN_1:
                                case EN_SCENARIO.SCENARIO_REQ_LOT_ID_MERGE_AND_CHANGE_BIN_2:
                                case EN_SCENARIO.SCENARIO_REQ_LOT_ID_MERGE_AND_CHANGE_BIN_3:
                                    {
                                        // 랏 머지 완료 처리
                                        _carrierServer.SetAttribute(PortId, PWA500CarrierAttributes.KeyProcessStepBeforeSendingCarrier, ((int)StepsBeforeSendingCarrier.MergeAndChangeCompleted).ToString());
                                        _carrierServer.SaveCarrierData(PortId);

                                        if (MySubstrateType == SubstrateType.Empty)
                                            return new CommandResults(string.Empty, CommandResult.Completed);

                                        #region <머지할 랏을 받아온다.>
                                        var scenarioResult = GetScenarioResultData(_queuedScenarioBeforeSendingCarrier);
                                        
                                        return ApplyResultAfterExecution(scenarioResult);
                                        #endregion </머지할 랏을 받아온다.>
                                    }

                                case EN_SCENARIO.SCENARIO_SLOT_WAFER_MAPPING_CORE_1:
                                case EN_SCENARIO.SCENARIO_SLOT_WAFER_MAPPING_CORE_2:
                                case EN_SCENARIO.SCENARIO_SLOT_WAFER_MAPPING_EMPTY_TAPE:
                                case EN_SCENARIO.SCENARIO_SLOT_WAFER_MAPPING_BIN_1:
                                case EN_SCENARIO.SCENARIO_SLOT_WAFER_MAPPING_BIN_2:
                                case EN_SCENARIO.SCENARIO_SLOT_WAFER_MAPPING_BIN_3:
                                    {
                                        // 슬롯 매핑 완료 처리
                                        _carrierServer.SetAttribute(PortId, PWA500CarrierAttributes.KeyProcessStepBeforeSendingCarrier, ((int)StepsBeforeSendingCarrier.SlotMappingCompleted).ToString());
                                        _carrierServer.SaveCarrierData(PortId);

                                        #region <히스토리 정리>
                                        string carrierId = _carrierServer.GetCarrierId(PortId);
                                        List<string> substrates = null;
                                        if (MySubstrateType != SubstrateType.Empty)
                                        {
                                            var temporarySubstrates = _substrateManager.GetSubstratesAtLoadPort(PortId);
                                            if (temporarySubstrates != null)
                                            {
                                                substrates = new List<string>();
                                                foreach (var item in temporarySubstrates)
                                                {
                                                    substrates.Add(item.Value.Name);
                                                }
                                            }
                                        }

                                        bool isCore = MySubstrateType == SubstrateType.Core;

                                        string lotId = _carrierServer.GetCarrierLotId(PortId);
                                        if (false == isCore)
                                        {
                                            lotId = _toWrite;
                                        }
                                        _lotHistoryLog.BackupCarrierHistory(PortId, carrierId, lotId, substrates, isCore);
                                        #endregion </히스토리 정리>

                                        // 중요하지 않은 이벤트니 비동기 실행한다.
                                        if (MySubstrateType.Equals(SubstrateType.Core))
                                        {
                                            string partId = _carrierServer.GetAttribute(PortId, PWA500CarrierAttributes.KeyPartId);
                                            string stepId = _carrierServer.GetAttribute(PortId, PWA500CarrierAttributes.KeyStepSeq);
                                            string lotType = _carrierServer.GetAttribute(PortId, PWA500CarrierAttributes.KeyLotType);
                                            _functionsForPWA500.ExecuteScenarioAsyncToCarrierUnload(lotId, partId, stepId, lotType);
                                        }

                                        return new CommandResults(string.Empty, CommandResult.Completed);
                                    }

                                default:
                                    return new CommandResults(string.Empty, CommandResult.Completed);
                            }
                        }
                        
                    case EN_SCENARIO_RESULT.ERROR:
                    case EN_SCENARIO_RESULT.TIMEOUT_ERROR:
                        return new CommandResults(
                            _queuedScenarioBeforeSendingCarrier.ToString(),
                            CommandResult.Error, 
                            result.ToString());

                    default:
                        return new CommandResults(string.Empty, CommandResult.Completed);
                }
            }
        }
        protected override bool PrepareBeforeSendingCarrier()
        {
            return false;
            //// 1. 작업이 완료되었는지 먼저 확인
            //var accessingStatus = _carrierServer.GetCarrierAccessingStatus(PortId);
            //if (false == accessingStatus.Equals(CarrierAccessStates.CarrierCompleted))
            //    return false;

            //switch (MySubstrateType)
            //{
            //    case SubstrateType.Bin1:
            //    case SubstrateType.Bin2:
            //    case SubstrateType.Bin3:
            //        {
            //            var stepString = _carrierServer.GetAttribute(PortId, PWA500BINCarrierAttributeKeys.KeyProcessStepBeforeSendingCarrier);
            //            if (int.TryParse(stepString, out int stepInt) &&
            //                Enum.IsDefined(typeof(StepsBeforeSendingCarrier), stepInt))
            //            {
            //                _currentStepBeforeSendingCarrier = (StepsBeforeSendingCarrier)stepInt;
            //            }
            //            else
            //            {
            //                _currentStepBeforeSendingCarrier = default;
            //            }

            //            // 2. 진행단계에 따라..
            //            switch (_currentStepBeforeSendingCarrier)
            //            {
            //                case StepsBeforeSendingCarrier.Init:
            //                    {
            //                        // 2-1. 머지, 체인지 실행 
            //                        Dictionary<string, string> scenarioParam = new Dictionary<string, string>();
            //                        if (false == MakeScenarioParamForMergeLot(ref scenarioParam))
            //                            return false;

            //                        InitResult(ScenarioTypeToLotMerge);

            //                        return _scenarioOperator.UpdateScenarioParam(GetTaskName(), ScenarioTypeToLotMerge, scenarioParam);
            //                    }


            //                case StepsBeforeSendingCarrier.MergeAndChangeCompleted:
            //                    {
            //                        // 2-2. 슬롯 매핑
            //                        Dictionary<string, string> scenarioParam = new Dictionary<string, string>();
            //                        if (false == MakeScenarioParamForSlotMapping(ref scenarioParam))
            //                            return false;

            //                        InitResult(ScenarioTypeToSlotMapping);

            //                        return _scenarioOperator.UpdateScenarioParam(GetTaskName(), ScenarioTypeToSlotMapping, scenarioParam);

            //                    }

            //                case StepsBeforeSendingCarrier.SlotMappingCompleted:
            //                    {
            //                        // 2-3. 태그쓰기 실행
            //                        InitRFID(true);
            //                        return true;
            //                    }

            //                default:
            //                    return false;
            //            }
            //        }

            //    default:
            //        return false;
            //}
        }
        protected override CommandResults ExecuteBeforeSendingCarrier()
        {
            _commandResult.CommandResult = CommandResult.Completed;
            return _commandResult;
            //switch (MySubstrateType)
            //{
            //    case SubstrateType.Bin1:
            //    case SubstrateType.Bin2:
            //    case SubstrateType.Bin3:
            //        {
            //            switch (_currentStepBeforeSendingCarrier)
            //            {
            //                case StepsBeforeSendingCarrier.Init:
            //                case StepsBeforeSendingCarrier.MergeAndChangeCompleted:
            //                    {
            //                        // LotMerge 실행(멤버변수는 안에서 갱신됨)
            //                        EN_SCENARIO scenario;
            //                        if (_currentStepBeforeSendingCarrier == StepsBeforeSendingCarrier.Init)
            //                        {
            //                            scenario = ScenarioTypeToLotMerge;
            //                        }
            //                        else
            //                        {
            //                            scenario = ScenarioTypeToSlotMapping;
            //                        }
            //                        var result = RunScenario(scenario);
            //                        if (result.CommandResult == CommandResult.Completed ||
            //                            result.CommandResult == CommandResult.Skipped)
            //                        {
            //                            // 완료 후 다음 스텝
            //                            if (_currentStepBeforeSendingCarrier == StepsBeforeSendingCarrier.Init)
            //                            {
            //                                _currentStepBeforeSendingCarrier = StepsBeforeSendingCarrier.MergeAndChangeCompleted;
            //                            }
            //                            else
            //                            {
            //                                _currentStepBeforeSendingCarrier = StepsBeforeSendingCarrier.SlotMappingCompleted;
            //                            }

            //                            _carrierServer.SetAttribute(PortId,
            //                                PWA500BINCarrierAttributeKeys.KeyProcessStepBeforeSendingCarrier,
            //                                ((int)_currentStepBeforeSendingCarrier).ToString());
            //                        }

            //                        return result;
            //                    }

            //                case StepsBeforeSendingCarrier.SlotMappingCompleted:
            //                    {
            //                        var result = WriteLotId()
            //                    }
            //                    break;
            //                case StepsBeforeSendingCarrier.WriteTag:
            //                    break;

            //                default:
            //                    _commandResult.CommandResult = CommandResult.Completed;
            //                    return _commandResult;
            //            }

            //            return _commandResult;
            //        }

            //    default:
            //        _commandResult.CommandResult = CommandResult.Completed;
            //        return _commandResult;
            //}
        }
        protected override CommandResults WriteCarrierId()
        {
            // 2025.07.09. jhlim [DEL] 이전 스텝에서 했으니 안 해도 된다.
            //InitRFID(true);
            // 2025.07.09. jhlim [END]
            return new CommandResults(string.Empty, CommandResult.Completed);
        }
        protected override CommandResults WriteLotId()
        {
            if (false == _scenarioOperator.UseScenario)
            {
                return new CommandResults(string.Empty, CommandResult.Skipped);
            }
            else
            {
                string lotId = string.Empty;
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
                if (false == GetLotIdToWrite(loadingMode, ref lotId))
                {
                    // 랏 아이디 쓰기 완료 처리
                    _carrierServer.SetAttribute(PortId, PWA500CarrierAttributes.KeyProcessStepBeforeSendingCarrier, ((int)StepsBeforeSendingCarrier.WriteTag).ToString());
                    _carrierServer.SaveCarrierData(PortId);

                    return new CommandResults(string.Empty, CommandResult.Skipped);
                }

                _toWrite = lotId;
                var result = _rfidManager.WriteLotId(LoadPortIndex, loadingMode, _toWrite);
                if (result.CommandResult == CommandResult.Completed)
                {
                    _carrierServer.SetCarrierLotId(PortId, _toWrite);
                    // 랏 아이디 쓰기 완료 처리
                    _carrierServer.SetAttribute(PortId, PWA500CarrierAttributes.KeyProcessStepBeforeSendingCarrier, ((int)StepsBeforeSendingCarrier.WriteTag).ToString());
                    _carrierServer.SaveCarrierData(PortId);

                    // 2025.07.09. jhlim [DEL] 항상 동작 전 Init 하는 것이 Rule 이므로, 여기서는 안 해도 된다.
                    // InitRFID(true);
                    // 2025.07.09. jhlim [END]
                }

                return result;
            }
        }
        protected override CommandResults ExecuteAfterWriting()
        {
            if (false == _scenarioOperator.UseScenario)
            {
                return new CommandResults(string.Empty, CommandResult.Skipped);
            }
            else
            {
                switch (MySubstrateType)
                {
                    case SubstrateType.Core:
                    case SubstrateType.Empty:
                        return new CommandResults(string.Empty, CommandResult.Skipped);

                    default:
                        break;
                }

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

                string readLotId = string.Empty;
                _commandResult = _rfidManager.ReadLotId(LoadPortIndex, loadingMode, ref readLotId);
                switch (_commandResult.CommandResult)
                {
                    case CommandResult.Completed:
                    case CommandResult.Skipped:
                        {
                            if (false == readLotId.Equals(_toWrite))
                            {
                                _commandResult = new CommandResults(_commandResult.ActionName, CommandResult.Error, string.Format("Lot Id does not matched"));
                            }
                            else
                            {
                                _carrierServer.SetAttribute(PortId, PWA500CarrierAttributes.KeyProcessStepBeforeSendingCarrier, ((int)StepsBeforeSendingCarrier.WriteTag).ToString());
                                _carrierServer.SaveCarrierData(PortId);
                            }
                        }
                        break;

                    default:
                        break;
                }

                return _commandResult;
            }
        }

        protected override bool CheckSlotValidation()
        {
            if (_loadPortManager.GetCarrierLoadingType(LoadPortIndex) == LoadPortLoadingMode.Cassette || _loadPortManager.GetCarrierLoadingType(LoadPortIndex) == LoadPortLoadingMode.ClosedCassette)
            {
                var substrates = _substrateManager.GetSubstratesAtLoadPort(PortId);
                if (substrates == null || substrates.Count <= 0)
                    return true;

                return (false == substrates.ContainsKey(1));
            }

            return true;
        }
        #endregion </Overrides>

        #region <Internal Interfaces>
        private string MakeCompletionConditionKey()
        {
            switch (MySubstrateType)
            {
                case SubstrateType.Bin1:
                case SubstrateType.Bin2:
                case SubstrateType.Bin3:
                    {
                        Recipe.PARAM_EQUIPMENT paramUseCapacity =
                            Recipe.PARAM_EQUIPMENT.UseCapacityLimitBin1 + LoadPortIndex;

                        Recipe.PARAM_EQUIPMENT paramCapacityLimit =
                            Recipe.PARAM_EQUIPMENT.AvailableCarrierCapacityBin1 + LoadPortIndex;

                        bool useCapacityLimit =
                            _recipe.GetValue(
                                Recipe.EN_RECIPE_TYPE.EQUIPMENT,
                                paramUseCapacity.ToString(),
                                false);

                        int capacityLimit =
                            _recipe.GetValue(
                                Recipe.EN_RECIPE_TYPE.EQUIPMENT,
                                paramCapacityLimit.ToString(),
                                0);

                        return string.Format(
                            "{0}|{1}|{2}|{3}|{4}",
                            MySubstrateType,
                            paramUseCapacity,
                            useCapacityLimit,
                            paramCapacityLimit,
                            capacityLimit);
                    }

                default:
                    return MySubstrateType.ToString();
            }
        }
        private bool GetLotIdToWrite(LoadPortLoadingMode loadingMode, ref string lotId)
        {
            // 1. 캐리어내에 자재가 있는지 검사
            bool hasAnySubstrates = true;
            var tempSubs = _substrateManager.GetSubstratesAtLoadPort(PortId);
            if (tempSubs == null ||
                tempSubs.Count <= 0)
                hasAnySubstrates = false;

            switch (MySubstrateType)
            {
                case SubstrateType.Core:
                    {
                        // 2-1 : 머지할 자재가 있거나, 자재가 없으면
                        //  1) 머지할게 있는 경우 리턴시켜서 기입하지 않음
                        if (hasAnySubstrates)
                        {
                            bool needToMerge = GetSubstrateToMerge(out Dictionary<int, Substrate> substrates);
                            if (substrates.Count > 0 || needToMerge)
                            {
                                return false;
                            }
                        }
                    }
                    break;

                case SubstrateType.Empty:
                    {
                        // 2-2 : 자재가 있는데 완료되면(Completed or Stopped) 자재 폐기용일텐데, 이 경우 자재가 있을 수도 있음. 
                        // 검사할게 없다. 자재가 없으면 -> 아래서 Empty조건으로 기입, 있으면 -> Terminated 조건으로 기입
                        //var substrates = _substrateManager.GetSubstratesAtLoadPort(PortId);
                        //if (substrates.Count > 0)
                        //{
                        //    return false;
                        //}
                    }
                    break;

                default:
                    {
                        // 머지 & 체인지 된 값으로 적용
                        lotId = _toWrite;
                        return true;
                    }
            }

            switch (loadingMode)
            {
                case LoadPortLoadingMode.ClosedCassette:
                    //{
                    //    if (hasAnySubstrates)
                    //    {
                    //        lotId = GetParameter(Recipe.PARAM_EQUIPMENT.WrittingLotIdToClosedCassetteWhenLotIsTerminated, "IRCASSETTE");
                    //    }
                    //    else
                    //    {
                    //        lotId = GetParameter(Recipe.PARAM_EQUIPMENT.WrittingLotIdToClosedCassetteWhenCarrierIsEmpty, "IECASSETTE");
                    //    }
                    //}
                    //break;
                case LoadPortLoadingMode.Cassette:
                    //_carrierIdToWrite = CarrierLotIdType.RCASSETTE.ToString();
                    if (hasAnySubstrates)
                    {
                        lotId = GetParameter(Recipe.PARAM_EQUIPMENT.WrittingLotIdToCassetteWhenLotIsTerminated, "RCASSETTE");
                    }
                    else
                    {
                        lotId = GetParameter(Recipe.PARAM_EQUIPMENT.WrittingLotIdToCassetteWhenCarrierIsEmpty, "ECASSETTE");
                    }
                    break;
                case LoadPortLoadingMode.Foup:
                    // 2024.11.27. jhlim [MOD] 고객사 요청으로 명칭 변경
                    //_carrierIdToWrite = CarrierLotIdType.PRMAC.ToString();
                    // 2024.11.27. jhlim [END]
                    if (hasAnySubstrates)
                    {
                        lotId = GetParameter(Recipe.PARAM_EQUIPMENT.WrittingLotIdToMACWhenLotIsTerminated, "PHMAC");
                    }
                    else
                    {
                        lotId = GetParameter(Recipe.PARAM_EQUIPMENT.WrittingLotIdToMACWhenCarrierIsEmpty, "PRMAC");
                    }
                    break;

                default:
                    break;
            }

            return true;
        }

        // 2025.05.26. jhlim [ADD] 모든 자재의 랏 이름이 캐리어의 랏 이름과 같다면 머지가 완료된 것이므로 진행할 필요가 없다.
        private bool IsAlreadyLotMerged()
        {
            string parentLotId = _carrierServer.GetCarrierLotId(PortId);
            var temporarySubstrates = _substrateManager.GetSubstratesAtLoadPort(PortId);
            foreach (var item in temporarySubstrates)
            {
                // 1개라도 다른게 있다면 머지가 안 된 것이다..
                var substrateLot = item.Value.LotId;
                if (false == parentLotId.Equals(substrateLot))
                    return false;
            }

            return true;
        }
        // 2025.05.26. jhlim [END] 
        private bool HasSubstrateToMergeOrSlotMapping()
        {
            switch (MySubstrateType)
            {
                case SubstrateType.Core:
                    return GetSubstrateToMerge(out _);

                case SubstrateType.Empty:
                    return false;

                default:
                    return true;
            }
        }
        private void InitResult(EN_SCENARIO scenario)
        {
            _commandResult.ActionName = scenario.ToString();
            _commandResult.CommandResult = CommandResult.Proceed;
            _commandResult.Description = string.Empty;
        }
        private CommandResults RunScenario(EN_SCENARIO scenario)
        {
            if (false == _carrierServer.HasCarrier(PortId))
            {
                _commandResult.CommandResult = CommandResult.Error;
                _commandResult.Description = "Does not have carrier";
                return _commandResult;
            }

            var result = _scenarioOperator.ExecuteScenario(GetTaskName(), scenario);
            _commandResult.ActionName = scenario.ToString();
            switch (result)
            {
                case EN_SCENARIO_RESULT.WAITING:
                case EN_SCENARIO_RESULT.PROCEED:
                    _commandResult.CommandResult = CommandResult.Proceed;
                    break;
                case EN_SCENARIO_RESULT.COMPLETED:
                    _commandResult.CommandResult = CommandResult.Completed;
                    ExecuteAfterScenarioCompletedForHistory(scenario);
                    break;
                case EN_SCENARIO_RESULT.ERROR:
                    _commandResult.CommandResult = CommandResult.Error;
                    _commandResult.Description = "Scenario Error";
                    break;
                case EN_SCENARIO_RESULT.TIMEOUT_ERROR:
                    _commandResult.CommandResult = CommandResult.Timeout;
                    _commandResult.Description = "Scenario Timeout";
                    break;

                default:
                    break;
            }
            return _commandResult;
        }

        private void ExecuteMovingAdsScenario()
        {
            Dictionary<string, string> param = new Dictionary<string, string>();
            if (false == MakeScenarioParamForMovingAdsFlag(ref param))
                return;
            
            EnqueueScenario(ScenarioTypeToAdsMoveFlag, param, null);
        }
        private bool MakeScenarioParamForMovingAdsFlag(ref Dictionary<string, string> scenarioParam)
        {
            if (scenarioParam == null)
                scenarioParam = new Dictionary<string, string>();

            scenarioParam.Clear();

            // TODO : 전량소팅된 경우는..? 랏 아이디를 뭐로 올려야하지..?
            string lotId = _carrierServer.GetCarrierLotId(PortId);
            string carrierId = _carrierServer.GetCarrierId(PortId);
            scenarioParam[MovingAdsKeys.KeyParamLotId] = lotId;
            scenarioParam[MovingAdsKeys.KeyParamCarrierId] = carrierId;
            
            var substrates = _substrateManager.GetSubstratesAtLoadPort(PortId);

            int fullSortedSubstrateCount = 0;
            int waferQty = 0;
            string[] substrateIds = new string[CarrierMaxCapacity];
            for (int i = 1; i <= CarrierMaxCapacity; ++i)
            {
                string id = string.Empty;
                if (substrates.TryGetValue(i, out Substrate substrate))
                {
                    id = substrate.Name;
                    var qty = substrate.GetAttribute(PWA500SubstrateAttributes.ChipQty);
                    if (string.Equals(qty, "0", StringComparison.OrdinalIgnoreCase))
                    {
                        // 2025.03.17. jhlim [END]
                        qty = string.Empty;
                        
                        ++fullSortedSubstrateCount;
                    }
                    else
                    {
                        waferQty++;
                    }
                }

                substrateIds[i - 1] = id;
            }

            scenarioParam[MovingAdsKeys.KeyParamAdsMoveFlag] = IsFullSortedSubstrateAllOrNone(substrates.Count, fullSortedSubstrateCount) ? "N" : "Y";
            for (int i = 1; i <= CarrierMaxCapacity; ++i)
            {
                string keyForId = BuildSlotMappingWaferIdKey(i);
                scenarioParam[keyForId] = substrateIds[i - 1];
            }

            // 2025.05.08. jhlim [ADD] 고객사 요청으로 캐리어내 웨이퍼 수량 추가
            scenarioParam[MovingAdsKeys.KeyParamWaferQty] = waferQty.ToString();
            // 2025.05.08. jhlim [END]

            //_lotHistoryLog.WriteHistoryForSlotMapping(PortId, carrierId, substratesToMapping, waferQty);

            return true;
        }

        private void InitializeSlotInfoAtSimulationMode()
        {
            if (_carrierServer.GetCarrierAccessingStatus(PortId) != CarrierAccessStates.NotAccessed)
                return;

            var slots = _carrierServer.GetCarrierSlotMap(PortId);
            var newSlots = new Dictionary<int, CarrierSlotMapStates>();
            switch (MySubstrateType)
            {
                case SubstrateType.Core:
                case SubstrateType.Empty:
                    {
                        int capa = 7;
                        List<string> targets = new List<string>();
                        foreach (var item in slots)
                        {                           
                            var isTargetSlot = (item.Key % 2 == 1);
                            if (isTargetSlot || item.Key > capa)
                            {
                                newSlots[item.Key] = CarrierSlotMapStates.Empty;
                                var key = _substrateManager.GetSubstrateKeyAtLoadPort(PortId, item.Key);
                                if (false == string.IsNullOrWhiteSpace(key))
                                {
                                    targets.Add(key);
                                }
                            }
                            else
                            {
                                newSlots[item.Key] = CarrierSlotMapStates.CorrectlyOccupied;
                            }
                        }

                        foreach (var item in targets)
                        {
                            _substrateManager.RemoveSubstrateByKey(item);
                        }
                    }
                    break;
                case SubstrateType.Bin1:
                case SubstrateType.Bin2:
                case SubstrateType.Bin3:
                    {
                        _substrateManager.RemoveSubstrateAtLoadPortAll(PortId);
                        foreach (var item in slots)
                        {
                            newSlots[item.Key] = CarrierSlotMapStates.Empty;
                        }
                    }
                    break;
                default:
                    break;
            }

            _carrierServer.SetCarrierSlotMap(PortId, newSlots);
            _carrierServer.SaveCarrierData(PortId);
        }
        private static bool IsBinType(SubstrateType type)
        {
            return type == SubstrateType.Bin1 ||
                   type == SubstrateType.Bin2 ||
                   type == SubstrateType.Bin3;
        }

        private static string BuildSlotMappingWaferIdKey(int slotIndex)
        {
            return $"{SlotMappingKeys.KeyParamSlotNamePre}{slotIndex}_{SlotMappingKeys.KeyParamSlotNamePost}";
        }
        private static string BuildSlotMappingQtyKey(int slotIndex)
        {
            return $"{SlotMappingKeys.KeyParamSlotQtyPre}{slotIndex}_{SlotMappingKeys.KeyParamSlotQtyPost}";
        }
        // 전량 소진된 자재가 0개이거나, 모든 자재가 전량 소진된 경우 true : ADS_MOVE_FLAG = N
        // 전량 소진된 자재가 1개 이상인 경우 false : ADS_MOVE_FLAG = Y
        private bool IsFullSortedSubstrateAllOrNone(int substrateCount, int fullSortedSubstrateCount)
        {
            if (fullSortedSubstrateCount == 0 ||
                fullSortedSubstrateCount == substrateCount)
                return true;
            
            return false;
        }

        // 1. 모든 코어가 전량 소진된 경우 머지 진행하지 않음
        // 2. 전량 소진된 자재가 없는 경우 머지 진행
        // 3. 공존하는 경우
        //  3-1. 1개면 머지진행하지 않음
        //  3-2. 2개 이상이면 전량 소진되지 않은 것만 머지 진행
        private bool GetSubstrateToMerge(out Dictionary<int, Substrate> substrates)
        {
            substrates = new Dictionary<int, Substrate>();
            var temporarySubstrates = _substrateManager.GetSubstratesAtLoadPort(PortId);

            bool hasTerminatedSubstrate = false;
            foreach (var item in temporarySubstrates)
            {
                string qtyString = item.Value.GetAttribute(PWA500SubstrateAttributes.ChipQty);
                if (false == int.TryParse(qtyString, out int qty))
                    continue;

                if (qty > 0)
                {
                    substrates[item.Key] = item.Value;
                }
                else
                {
                    hasTerminatedSubstrate = true;
                }
            }

            if (false == hasTerminatedSubstrate)
            {
                return true;
            }
            else
            {
                return substrates.Count > 1;
            }
        }

        // SlotMapping 시 웨이퍼 총 수량과 웨이퍼 이름, 칩 수량을 얻어오기 위한 함수
        // Old : 전량소팅된 자재는 웨이퍼 수량과 이름을 포함시키지 않음
        // New : 전량소팅된 자재도 웨이퍼 수량과 이름을 포함시킨다.
        private void GetTotalQtyAndWaferInfoAtSlot(
            Substrate substrate,
            out string waferId, 
            out string qty,
            ref int totalQty)
        {
            waferId = substrate.Name;
            qty = substrate.GetAttribute(PWA500SubstrateAttributes.ChipQty);
            if (IsOldEvents)
            {
                if (qty.Equals("0"))
                {
                    // 2025.03.17. jhlim [MOD] 슬롯매핑 또한 자재 수량이 0이면 올리지 않는다.
                    waferId = string.Empty;                        
                    // 2025.03.17. jhlim [END]
                    qty = string.Empty;
                }
                else
                {
                    // 잔여수량이 있는 웨이퍼만 수량에 포함시킨다.
                    totalQty++;
                }
            }
            else
            {
                if (qty.Equals("0"))
                {                    
                    qty = string.Empty;
                }

                // 전량소팅된 자재도 수량에 포함시켜야 한다고 한다.
                totalQty++;
            }

        }
        private bool MakeScenarioParamForSlotMapping(ref Dictionary<string, string> scenarioParam)
        {
            if (scenarioParam == null)
                scenarioParam = new Dictionary<string, string>();

            scenarioParam.Clear();

            // 1~6 포트 모두 진행
            var substrates = _substrateManager.GetSubstratesAtLoadPort(PortId);
            string lotId = string.Empty;
            switch (MySubstrateType)
            {
                case SubstrateType.Bin1:
                case SubstrateType.Bin2:
                case SubstrateType.Bin3:
                    {
                        if (substrates.Count <= 0)
                            return false;

                        var substrateFirst = substrates.First();
                        lotId = substrateFirst.Value.LotId;
                    }
                    break;
                default:
                    {
                        lotId = _carrierServer.GetCarrierLotId(PortId);
                    }
                    break;
            }

            string carrierId = _carrierServer.GetCarrierId(PortId);
            scenarioParam[SlotMappingKeys.KeyParamLotId] = lotId;
            scenarioParam[SlotMappingKeys.KeyParamCarrierId] = carrierId;

            Dictionary<int, Tuple<string, string>> substratesToMapping = new Dictionary<int, Tuple<string, string>>();

            // TODO : Slot 이 1부터로 변경되면서 수정한 부분
            int waferQty = 0;
            string[] substrateIds = new string[CarrierMaxCapacity];
            string[] substrateQtys = new string[CarrierMaxCapacity];
            for (int i = 1; i <= CarrierMaxCapacity; ++i)
            {
                string id = string.Empty;
                string qty = "";
                if (substrates.TryGetValue(i, out Substrate substrate))
                {
                    GetTotalQtyAndWaferInfoAtSlot(substrate, out id, out qty, ref waferQty);
                    //id = substrate.Name;
                    //qty = substrate.GetAttribute(PWA500SubstrateAttributes.ChipQty);
                    //if (qty.Equals("0"))
                    //{
                    //    // 2026.06.25. jhlim [DEL] 자재 수량이 0이면 올리지 않았으나, ADS 이벤트 추가됨으로써 올리도록 변경되었다.
                    //    // 2025.03.17. jhlim [MOD] 슬롯매핑 또한 자재 수량이 0이면 올리지 않는다.
                    //    //id = string.Empty;                        
                    //    // 2025.03.17. jhlim [END]
                    //    // 2026.06.25. jhlim [END]
                    //    qty = string.Empty;
                    //}
                    //else
                    //{
                    //    waferQty++;
                    //}

                    substratesToMapping[i] = Tuple.Create(id, qty);
                }

                substrateIds[i - 1] = id;
                substrateQtys[i - 1] = qty;
            }

            for (int i = 1; i <= CarrierMaxCapacity; ++i)
            {
                string keyForId = BuildSlotMappingWaferIdKey(i);
                    //string.Format("{0}{1}_{2}", SlotMappingKeys.KeyParamSlotNamePre, i, SlotMappingKeys.KeyParamSlotNamePost);
                scenarioParam[keyForId] = substrateIds[i - 1];
            }

            for (int i = 1; i <= CarrierMaxCapacity; ++i)
            {
                string keyForQty = BuildSlotMappingQtyKey(i);
                    //string.Format("{0}{1}_{2}", SlotMappingKeys.KeyParamSlotQtyPre, i, SlotMappingKeys.KeyParamSlotQtyPost);
                scenarioParam[keyForQty] = substrateQtys[i - 1];
            }

            // 2025.05.08. jhlim [ADD] 고객사 요청으로 캐리어내 웨이퍼 수량 추가
            scenarioParam[SlotMappingKeys.KeyParamWaferQty] = waferQty.ToString();
            // 2025.05.08. jhlim [END]

            _lotHistoryLog.WriteHistoryForSlotMapping(PortId, carrierId, substratesToMapping, waferQty);

            return true;
        }
        private bool MakeScenarioParamForChangeToLotId(ref Dictionary<string, string> scenarioParam)
        {
            if (scenarioParam == null)
                scenarioParam = new Dictionary<string, string>();

            scenarioParam.Clear();

            // 1~6 포트 모두 진행
            string lotId = _carrierServer.GetCarrierLotId(PortId);
            string carrierId = _carrierServer.GetCarrierId(PortId);

            scenarioParam[ChangeToLotIdKeys.KeyParamLotId] = lotId;
            scenarioParam[ChangeToLotIdKeys.KeyParamCarrierId] = carrierId;

            return true;
        }
        private bool GetMergedLotId(ref string mergedLotId)
        {
            // 1~6 포트 모두 진행
            mergedLotId = _carrierServer.GetCarrierLotId(PortId);

            // 2024.10.23. jhlim [MOD] 코어는 머지할 자재를 모두 가져오는 것이 아닌, 자재 정보를 통해 선별된 것만 가져온다.
            Dictionary<int, Substrate> substrates;
            if (MySubstrateType.Equals(SubstrateType.Core))
            {
                GetSubstrateToMerge(out substrates);
            }
            else
            {
                substrates = _substrateManager.GetSubstratesAtLoadPort(PortId);
            }
            // 2024.10.23. jhlim [END]

            if (substrates.Count <= 0)
                return false;

            var firstSubstrate = substrates.First();
            switch (MySubstrateType)
            {
                case SubstrateType.Core:
                    {
                        bool hasParentLotId = false;
                        foreach (var item in substrates)
                        {
                            if (item.Value.LotId.Equals(mergedLotId))
                            {
                                hasParentLotId = true;
                                break;
                            }
                        }

                        if (false == hasParentLotId)
                        {
                            foreach (var item in substrates)
                            {
                                string chipQtyString = item.Value.GetAttribute(PWA500SubstrateAttributes.ChipQty);
                                if (false == string.IsNullOrEmpty(chipQtyString) &&
                                    false == chipQtyString.Equals("0"))
                                {
                                    mergedLotId = firstSubstrate.Value.LotId;
                                    return true;
                                }
                            }
                        }
                    }
                    return true;

                case SubstrateType.Bin1:
                case SubstrateType.Bin2:
                case SubstrateType.Bin3:
                    // Bin의 경우 첫 번째 Lot 이름을 대표이름으로 병합한다. -> LotId Change 이후 새로운 Lot Id 부여받음
                    mergedLotId = firstSubstrate.Value.LotId;
                    return true;

                default:
                    return false;
            }
        }
        private bool MakeScenarioParamForMergeLot(ref Dictionary<string, string> scenarioParam)
        {
            if (scenarioParam == null)
                scenarioParam = new Dictionary<string, string>();

            scenarioParam.Clear();

            // 1~6 포트 모두 진행
            string lotId = _carrierServer.GetCarrierLotId(PortId);
            string carrierId = _carrierServer.GetCarrierId(PortId);

            // 2024.10.23. jhlim [MOD] 코어는 머지할 자재를 모두 가져오는 것이 아닌, 자재 정보를 통해 선별된 것만 가져온다.
            Dictionary<int, Substrate> substrates;
            if (MySubstrateType.Equals(SubstrateType.Core))
            {
                GetSubstrateToMerge(out substrates);
            }
            else
            {
                substrates = _substrateManager.GetSubstratesAtLoadPort(PortId);
            }
            // 2024.10.23. jhlim [END]

            if (substrates.Count <= 0)
                return false;

            var firstSubstrate = substrates.First();
            switch (MySubstrateType)
            {
                case SubstrateType.Core:
                    {
                        // 2025.03.18. jhlim [MOD] 로직이 잘못 되어 있었다.
                        // 1. 모랏을 가진놈이 수량 0인지 체크
                        bool hasValidParentLotId = false;
                        string chipQtyString = string.Empty;
                        foreach (var item in substrates)
                        {
                            if (item.Value.LotId.Equals(lotId))
                            {
                                chipQtyString = item.Value.GetAttribute(PWA500SubstrateAttributes.ChipQty);
                                if (false == chipQtyString.Equals("0"))
                                {
                                    hasValidParentLotId = true;
                                    break;
                                }
                            }
                        }

                        // 2. 모랏을 가진 자재의 칩이 없으면, 수량이 0개가 아닌 자재 중 하나를 골라 머지한다.
                        if (false == hasValidParentLotId)
                        {
                            foreach (var item in substrates)
                            {
                                chipQtyString = item.Value.GetAttribute(PWA500SubstrateAttributes.ChipQty);
                                if (false == chipQtyString.Equals("0"))
                                {
                                    lotId = item.Value.LotId;
                                    break;
                                }
                            }
                        }
                        // 2025.03.18. jhlim [END]
                    }
                    break;
                case SubstrateType.Bin1:
                case SubstrateType.Bin2:
                case SubstrateType.Bin3:
                    // Bin의 경우 첫 번째 Lot 이름을 대표이름으로 병합한다. -> LotId Change 이후 새로운 Lot Id 부여받음
                    lotId = firstSubstrate.Value.LotId;
                    break;
                default:
                    break;
            }

            scenarioParam[LotMergeKeys.KeyParamLotId] = lotId;
            scenarioParam[LotMergeKeys.KeyParamCarrierId] = carrierId;

            string partId = firstSubstrate.Value.GetAttribute(PWA500SubstrateAttributes.PartId);
            string recipeId = EquipmentInfo.GetRecipeId();

            scenarioParam[LotMergeKeys.KeyParamPartId] = partId;
            scenarioParam[LotMergeKeys.KeyParamRecipeId] = recipeId;
            scenarioParam[LotMergeKeys.KeyOperatorId] = "AUTO";

            // TODO : Slot 이 1부터로 변경되면서 수정한 부분
            for (int i = 1; i <= CarrierMaxCapacity; ++i)
            {
                string substrateLotId = string.Empty;
                if (substrates.TryGetValue(i, out Substrate substrate))
                {
                    substrateLotId = substrate.LotId;
                }

                string keyForQty = string.Format("{0}{1}_{2}", LotMergeKeys.KeyParamSlotLotIdPre, i, LotMergeKeys.KeyParamSlotLotIdPost);
                scenarioParam[keyForQty] = substrateLotId;
            }

            // Change를 위함
            //if (PortId < 4)
            //if (MySubstrateType.Equals(SubstrateType.Bin1) ||
            //    MySubstrateType.Equals(SubstrateType.Bin2) ||
            //    MySubstrateType.Equals(SubstrateType.Bin3))
            if (IsBinType(MySubstrateType))
            {
                string[] substrateIds = new string[CarrierMaxCapacity];
                string[] substrateQtys = new string[CarrierMaxCapacity];

                for (int i = 1; i <= CarrierMaxCapacity; ++i)
                {
                    string id = string.Empty;
                    string qty = "0";
                    if (substrates.TryGetValue(i, out Substrate substrate))
                    {
                        id = substrate.Name;
                        qty = substrate.GetAttribute(PWA500SubstrateAttributes.ChipQty);
                    }
                    substrateIds[i - 1] = id;
                    substrateQtys[i - 1] = qty;
                }

                for (int i = 1; i <= CarrierMaxCapacity; ++i)
                {
                    string keyForId = string.Format("{0}{1}_{2}", SlotMappingKeys.KeyParamSlotNamePre, i, SlotMappingKeys.KeyParamSlotNamePost);
                    scenarioParam[keyForId] = substrateIds[i - 1];
                }

                for (int i = 1; i <= CarrierMaxCapacity; ++i)
                {
                    string keyForQty = string.Format("{0}{1}_{2}", SlotMappingKeys.KeyParamSlotQtyPre, i, SlotMappingKeys.KeyParamSlotQtyPost);
                    scenarioParam[keyForQty] = substrateQtys[i - 1];
                }

                scenarioParam[SlotMappingKeys.KeyParamWaferQty] = substrates.Count.ToString();
            }
            return true;
        }
        private bool ApplyResultOfMergingLot(Dictionary<string, string> resultOfMergingLot)
        {
            if (false == resultOfMergingLot.TryGetValue(LotMergeKeys.KeyResultLotId, out string lotId))
                return false;

            _toWrite = lotId;

            Dictionary<int, string> lotIdToMerge = new Dictionary<int, string>();

            // 새로 부여 받은 LotId로 모든 자재의 LotId를 갱신한다.
            var substrates = _substrateManager.GetSubstratesAtLoadPort(PortId);
            foreach (var item in substrates)
            {
                lotIdToMerge[item.Key] = item.Value.LotId;

                //item.Value.SetLotId(lotId);
                _substrateManager.SetLotIdByKey(item.Value.UniqueKey, lotId);
                _substrateManager.SaveDataByKey(item.Value.UniqueKey);
            }

            string carrierId = _carrierServer.GetCarrierId(PortId);
            var useChange = false == MySubstrateType.Equals(SubstrateType.Core);
            _lotHistoryLog.WriteHistoryForMerge(PortId, carrierId, lotId, useChange, lotIdToMerge);

            return true;
        }
        private void ExecuteAfterScenarioCompletedForHistory(EN_SCENARIO scenario)
        {
            if (scenario == ScenarioTypeToIdRead)
            {
                string lotId = _carrierServer.GetCarrierLotId(PortId);
                string carrierId = _carrierServer.GetCarrierId(PortId);

                _lotHistoryLog.WriteHistoryForIdRead(PortId, carrierId, lotId);

                if (MySubstrateType.Equals(SubstrateType.Core))
                {
                    // 귀찮으니 비동기 실행
                    _functionsForPWA500.ExecuteScenarioAsyncToCarrierLoad(lotId, carrierId);
                }
            }
            else
            {
                // Core or Empty
                if (MySubstrateType.Equals(SubstrateType.Core) ||
                    MySubstrateType.Equals(SubstrateType.Empty))
                {
                    if (scenario.Equals(ScenarioTypeToRequestLotInfo))
                    {
                        #region <Lot Info 갱신>
                        var scenarioResult = _scenarioOperator.GetScenarioResultData(GetTaskName(), ScenarioTypeToRequestLotInfo);
                        if (scenarioResult == null)
                        {
                            _commandResult.CommandResult = CommandResult.Error;
                            _commandResult.Description = "Scenario Result Error";
                            return;
                        }
                        _lotId = scenarioResult[LotInfoKeys.KeyResultLotId];
                        _partId = scenarioResult[LotInfoKeys.KeyResultPartId];
                        _stepSeq = scenarioResult[LotInfoKeys.KeyResultStepSeq];
                        _lotType = scenarioResult[LotInfoKeys.KeyResultLotType];
                        _lotQty = scenarioResult[LotInfoKeys.KeyResultLotQty];
                        _recipeId = scenarioResult[LotInfoKeys.KeyResultRecipeId];

                        if (string.IsNullOrEmpty(_lotId) ||
                            string.IsNullOrEmpty(_partId) ||
                            string.IsNullOrEmpty(_stepSeq) ||
                            string.IsNullOrEmpty(_lotType) ||
                            string.IsNullOrEmpty(_lotQty))
                        {
                            _commandResult.CommandResult = CommandResult.Error;
                            _commandResult.Description = "Scenario Result Error";
                            return;
                        }

                        if (MySubstrateType.Equals(SubstrateType.Core) ||
                            MySubstrateType.Equals(SubstrateType.Empty))
                        {
                            _carrierServer.SetCarrierLotId(PortId, _lotId);
                            _carrierServer.SetAttribute(PortId, PWA500CarrierAttributes.KeyPartId, _partId);
                            _carrierServer.SetAttribute(PortId, PWA500CarrierAttributes.KeyStepSeq, _stepSeq);
                            _carrierServer.SetAttribute(PortId, PWA500CarrierAttributes.KeyLotType, _lotType);
                            _carrierServer.SetAttribute(PortId, PWA500CarrierAttributes.KeyLotQty, _lotQty);
                            _carrierServer.SaveCarrierData(PortId);
                        }

                        string carrierId = _carrierServer.GetCarrierId(PortId);
                        _lotHistoryLog.WriteHistoryForLotInfo(PortId, carrierId, _lotId, _partId, _stepSeq, _lotType, _lotQty);
                        #endregion </Lot Info 갱신>
                    }
                    else if (scenario.Equals(ScenarioTypeToSlotVerification))
                    {
                        #region <Slot Info 갱신>
                        var scenarioResult = _scenarioOperator.GetScenarioResultData(GetTaskName(), ScenarioTypeToSlotVerification);
                        if (scenarioResult == null)
                        {
                            _commandResult.CommandResult = CommandResult.Error;
                            _commandResult.Description = "Scenario Result Error";
                            return;
                        }

                        if (false == scenarioResult.TryGetValue(SlotMapVefiricationKeys.KeyIsCancelCarrier, out string isCancelCarrier))
                            return;

                        bool.TryParse(isCancelCarrier, out _receivedCancelCarrier);
                        if (_receivedCancelCarrier)
                        {
                            // TODO : Cancel Carrier Logging 필요
                            _commandResult.CommandResult = CommandResult.Skipped;
                        }
                        else
                        {
                            Dictionary<int, string> scenarioResultForStatus = new Dictionary<int, string>();

                            var status = _carrierServer.GetCarrierSlotMap(PortId);
                            var carrierLotId = _carrierServer.GetCarrierLotId(PortId);
                            var substrates = _substrateManager.GetSubstratesAtLoadPort(PortId);
                            string statusKeyForSplit = string.Format("{0}_", SlotMapVefiricationKeys.KeyResultStatus);
                            foreach (var item in scenarioResult)
                            {
                                if (item.Key.Contains(statusKeyForSplit))
                                {
                                    string[] statusKey = item.Key.Split('_');
                                    if (statusKey.Length != 2)
                                        continue;

                                    if (false == int.TryParse(statusKey[1], out int index))
                                        continue;

                                    if (index <= 0 || index > _carrierServer.GetCapacity(PortId))
                                        continue;

                                    if (item.Value == "4")       // Exist -> SemiStandard에서는 3이 맞음(CorrectlyOccupied)
                                    {
                                        scenarioResultForStatus[index] = item.Value;

                                        // 전산상 있는데 스캔 정보상 없는거면 나가있는 자재를 검사해야하며, 없을 시에는 검증 에러임
                                        if (false == status[index].Equals(CarrierSlotMapStates.CorrectlyOccupied))
                                        {
                                            // 이미 나가있으니 랏 정보는 세팅 되어있을 것으로 예상
                                            // TODO : 나간 자재가 있는지 검색 후 없으면 검증에러처리 필요
                                            continue;
                                        }
                                        else
                                        {
                                            // 여기는 정상이다.(서버도 있고, 스캔 결과에도 있고)
                                            string keyForLotId = string.Format("{0}_{1}", SlotMapVefiricationKeys.KeyResultLotId, index);
                                            string keyForSubstrateId = string.Format("{0}_{1}", SlotMapVefiricationKeys.KeyResultName, index);
                                            if (substrates.ContainsKey(index))
                                            {
                                                var lotId = carrierLotId;
                                                if (UseSlotValidationResult)
                                                {
                                                    scenarioResult.TryGetValue(keyForLotId, out lotId);
                                                }
                                                _substrateManager.SetLotIdByKey(substrates[index].UniqueKey, lotId);
                                                if (string.IsNullOrEmpty(substrates[index].GetAttribute(PWA500SubstrateAttributes.ParentLotId)))
                                                {
                                                    _substrateManager.SetAttributeByKey(substrates[index].UniqueKey, PWA500SubstrateAttributes.ParentLotId, lotId);
                                                }

                                                //if (scenarioResult.ContainsKey(keyForLotId))
                                                //{
                                                //    //substrates[index].SetLotId(scenarioResult[keyForLotId]);
                                                //    _substrateManager.SetLotIdByKey(substrates[index].UniqueKey, scenarioResult[keyForLotId]);
                                                //    if (string.IsNullOrEmpty(substrates[index].GetAttribute(PWA500SubstrateAttributes.ParentLotId)))
                                                //    {
                                                //        _substrateManager.SetAttributeByKey(substrates[index].UniqueKey, PWA500SubstrateAttributes.ParentLotId, scenarioResult[keyForLotId]);
                                                //    }
                                                //}

                                                if (scenarioResult.ContainsKey(keyForSubstrateId))
                                                {
                                                    // 2024.12.29. jhlim [MOD] Ring Id를 고유하게 만들기 위함 : CarrierId_LP{포트번호}.{슬롯번호} 형식
                                                    var curRingId = substrates[index].GetAttribute(PWA500SubstrateAttributes.RingId);
                                                    if (string.IsNullOrEmpty(curRingId))
                                                    {
                                                        _substrateManager.SetAttributeByKey(substrates[index].UniqueKey, PWA500SubstrateAttributes.RingId, substrates[index].Name);
                                                    }

                                                    // 2025.07.25. jhlim [MOD] 옵션에따라 슬롯 정보 적용하도록 변경 -> 슬롯 불일치 시 에러 띄우도록 개선 필요
                                                    if (UseSlotValidationResult)
                                                    {
                                                        //substrates[index].SetName(scenarioResult[keyForSubstrateId]);
                                                        _substrateManager.SetNameByKey(substrates[index].UniqueKey, scenarioResult[keyForSubstrateId]);
                                                    }
                                                    // 2025.07.25. jhlim [END]

                                                    //substrates[index].SetAttribute(PWA500BINSubstrateAttributes.RingId, scenarioResult[keyForSubstrateId]);
                                                    // 2024.12.29. jhlim [END]
                                                }

                                                #region <Lot Info 갱신>
                                                //substrates[index].SetRecipeId(_recipeId);
                                                _substrateManager.SetRecipeIdByKey(substrates[index].UniqueKey, _recipeId);
                                                _substrateManager.SetAttributeByKey(substrates[index].UniqueKey, PWA500SubstrateAttributes.PartId, _partId);
                                                _substrateManager.SetAttributeByKey(substrates[index].UniqueKey, PWA500SubstrateAttributes.StepSeq, _stepSeq);
                                                _substrateManager.SetAttributeByKey(substrates[index].UniqueKey, PWA500SubstrateAttributes.LotType, _lotType);
                                                //_substrateManager.SetAttributesByKey(substrates[index].UniqueKey, new Dictionary<string, string>
                                                //{
                                                //    [PWA500SubstrateAttributes.PartId] = _partId,
                                                //    [PWA500SubstrateAttributes.StepSeq] = _stepSeq,
                                                //    [PWA500SubstrateAttributes.LotType] = _lotType
                                                //});

                                                //substrates[index].SetAttribute(PWA500SubstrateAttributes.PartId, _partId);
                                                //substrates[index].SetAttribute(PWA500SubstrateAttributes.StepSeq, _stepSeq);
                                                //substrates[index].SetAttribute(PWA500SubstrateAttributes.LotType, _lotType);
                                                _substrateManager.SaveDataByKey(substrates[index].UniqueKey);
                                                #endregion </Lot Info 갱신>
                                            }
                                        }
                                    }
                                    else
                                    {
                                        // TODO : 서버에는 없지만, 자재 정보는 있으므로 검증 실패 처리 필요
                                        // 서버에서는 없다고 하는데 실제로 있는 경우(이미 나가있는 자재 포함) -> Verification Alarm 사용 시 에러임
                                        if (status.ContainsKey(index) &&
                                            status[index] == CarrierSlotMapStates.CorrectlyOccupied)
                                        {
                                            Substrate temporarySubstrate;
                                            if (substrates.ContainsKey(index))
                                            {
                                                temporarySubstrate = substrates[index];
                                            }
                                            else
                                            {
                                                string carrierId = _carrierServer.GetCarrierId(PortId);
                                                if (false == _substrateManager.GetSubstrateBySourceCarrierInfo(PortId, index, carrierId, out temporarySubstrate))
                                                    continue;
                                            }

                                            #region <Lot Info 갱신>
                                            //temporarySubstrate.SetRecipeId(_recipeId);
                                            _substrateManager.SetRecipeIdByKey(temporarySubstrate.UniqueKey, _recipeId);

                                            // 2024.12.29. jhlim [DEL] Ring Id를 고유하게 만들기 위함 : CarrierId_LP{포트번호}.{슬롯번호} 형식
                                            if (string.IsNullOrEmpty(temporarySubstrate.GetAttribute(PWA500SubstrateAttributes.RingId)))
                                            {
                                                _substrateManager.SetAttributeByKey(temporarySubstrate.UniqueKey, PWA500SubstrateAttributes.RingId, temporarySubstrate.Name);
                                            }
                                            // 2024.12.29. jhlim [END]

                                            _substrateManager.SetAttributeByKey(temporarySubstrate.UniqueKey, PWA500SubstrateAttributes.PartId, _partId);
                                            _substrateManager.SetAttributeByKey(temporarySubstrate.UniqueKey, PWA500SubstrateAttributes.StepSeq, _stepSeq);
                                            _substrateManager.SetAttributeByKey(temporarySubstrate.UniqueKey, PWA500SubstrateAttributes.LotType, _lotType);

                                            //_substrateManager.SetAttributesByKey(temporarySubstrate.UniqueKey, new Dictionary<string, string>
                                            //{
                                            //    [PWA500SubstrateAttributes.PartId] = _partId,
                                            //    [PWA500SubstrateAttributes.StepSeq] = _stepSeq,
                                            //    [PWA500SubstrateAttributes.LotType] = _lotType
                                            //});

                                            //temporarySubstrate.SetAttribute(PWA500SubstrateAttributes.PartId, _partId);
                                            //temporarySubstrate.SetAttribute(PWA500SubstrateAttributes.StepSeq, _stepSeq);
                                            //temporarySubstrate.SetAttribute(PWA500SubstrateAttributes.LotType, _lotType);
                                            if (string.IsNullOrEmpty(temporarySubstrate.GetAttribute(PWA500SubstrateAttributes.ParentLotId)))
                                            {
                                                string parentLotId = _carrierServer.GetCarrierLotId(PortId);
                                                _substrateManager.SetAttributeByKey(temporarySubstrate.UniqueKey, PWA500SubstrateAttributes.ParentLotId, parentLotId);
                                            }
                                            _substrateManager.SaveDataByKey(temporarySubstrate.UniqueKey);
                                            #endregion </Lot Info 갱신>

                                            continue;
                                        }
                                        else
                                        {
                                            // 정상
                                        }
                                    }
                                }
                            }

                            string currentCarrierId = _carrierServer.GetCarrierId(PortId);
                            _lotHistoryLog.WriteHistoryForSlotMap(PortId, currentCarrierId, scenarioResultForStatus);
                        }
                        #endregion </Slot Info 갱신>
                    }
                }
            }
        }


        #endregion </Internal Interfaces>

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

        #endregion </Methods>
    }

    class TaskLoadPortRecovery500BIN : Work.RecoveryData
    {
        public TaskLoadPortRecovery500BIN(string taskName, int nPortCount)
            : base(taskName, nPortCount)
        {
        }

        #region <Fields>
        //private const string KeyAccessStatus = "AccessStatus";
        //private CarrierAccessStates _accessStatus;

        //private bool _lotCompletionFlag;
        #endregion </Fields>

        #region <Properties>
        //public CarrierAccessStates AccessStatus
        //{
        //    get
        //    {
        //        return _accessStatus;
        //    }
        //    set
        //    {
        //        if (false == _accessStatus.Equals(value))
        //        {
        //            _accessStatus = value;
        //            //Save();
        //        }
        //    }
        //}
        //public bool LotCompleted
        //{
        //    get
        //    {
        //        return _lotCompletionFlag;
        //    }
        //    set
        //    {
        //        if (false == _lotCompletionFlag.Equals(value))
        //        {
        //            _lotCompletionFlag = value;
        //            //Save();
        //        }
        //    }
        //}
        #endregion </Properties>

        protected override void LoadData(ref FileComposite_.FileComposite fComp, string sRootName)
        {
            //string value = string.Empty;
            //fComp.GetValue(sRootName, KeyAccessStatus, ref value);
            //if (false == Enum.TryParse(value, out _accessStatus))
            //{
            //    AccessStatus = CarrierAccessStates.NotAccessed;
            //}
            //else
            //{
            //    AccessStatus = _accessStatus;
            //}
        }
        protected override void SaveData(ref FileComposite_.FileComposite fComp, string sRootName)
        {
            //fComp.AddItem(sRootName, KeyAccessStatus, AccessStatus.ToString());
        }
    }
}