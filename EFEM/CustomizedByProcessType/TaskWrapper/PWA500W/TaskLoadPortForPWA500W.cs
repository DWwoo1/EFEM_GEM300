using System;
using System.Collections.Generic;
using System.Collections.Concurrent;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using TickCounter_;

using FrameOfSystem3.SECSGEM.Scenario;
using FrameOfSystem3.SECSGEM;
using FrameOfSystem3.SECSGEM.DefineSecsGem;

using EFEM.Defines.Common;
using EFEM.Defines.LoadPort;
using EFEM.MaterialTracking;
using EFEM.CustomizedByProcessType.PWA500W;
using EFEM.CustomizedByProcessType.PWA500Common;
using EFEM.Modules.LoadPort.Scheduler;

// ConfigTask에서 이 namespace를 가지고 클래스 타입을 가져오기 때문에 변경 불가
namespace FrameOfSystem3.Task
{
    class TaskLoadPortForPWA500W : TaskLoadPort
    {
        #region <Constructors>
        public TaskLoadPortForPWA500W(int nIndexOfTask, string strTaskName)
            : base(nIndexOfTask, strTaskName, new TaskLoadPortRecovery500W(strTaskName, nIndexOfTask))
        {
            // 0번이 공테이프, 그 외에는 Core,

            int coreIndex = _loadPortManager.Count - PortId;
            ScenarioTypeToIdRead = EN_SCENARIO.SCENARIO_RFID_READ_CORE_1 + coreIndex;
            ScenarioTypeToRequestLotInfo = EN_SCENARIO.SCENARIO_REQ_LOT_INFO_CORE_1 + coreIndex;
            ScenarioTypeToSlotVerification = EN_SCENARIO.SCENARIO_REQ_SLOT_INFO_CORE_1 + coreIndex;

            ScenarioTypeToSlotMapping = EN_SCENARIO.SCENARIO_SLOT_WAFER_MAPPING_CORE_1 + coreIndex;
            ScenarioTypeToLotMerge = EN_SCENARIO.SCENARIO_REQ_LOT_MERGE_CORE_1 + coreIndex;

            ScenarioTypeToCarrierLoad = EN_SCENARIO.SCENARIO_PORT_STATUS_LOAD_1 + LoadPortIndex;
            ScenarioTypeToCarrierUnload = EN_SCENARIO.SCENARIO_PORT_STATUS_UNLOAD_1 + LoadPortIndex;

            _functionsForPWA500 = FunctionsForPWA500W_NRD.Instance;

            _recovery = _recoveryData as TaskLoadPortRecovery500W;
            _lotHistoryLog = LotHistoryLog.Instance;
            
            _lotHistoryLog.AddLogInfo(PortId, LoadPortName);
        }
        #endregion </Constructors>

        #region <Fields>
        private readonly EN_SCENARIO ScenarioTypeToIdRead;            // 1~6
        private readonly EN_SCENARIO ScenarioTypeToRequestLotInfo;    // 4~6
        private readonly EN_SCENARIO ScenarioTypeToSlotVerification;  // 4~6
        private readonly EN_SCENARIO ScenarioTypeToSlotMapping;       // 1~6
        private readonly EN_SCENARIO ScenarioTypeToLotMerge;          // 1~3, 5~6(1~3은 Change 포함)

        private readonly EN_SCENARIO ScenarioTypeToCarrierLoad;
        private readonly EN_SCENARIO ScenarioTypeToCarrierUnload;

        private const int CarrierMaxCapacity = 25;
        private const int DelayBeforeIdReadScenario = 3000;

        private CommandResults _commandResult = new CommandResults("", CommandResult.Invalid);
        private static TaskLoadPortRecovery500W _recovery;
        string _lotId = string.Empty;
        string _partId = string.Empty;
        string _stepSeq = string.Empty;
        string _lotType = string.Empty;
        string _lotQty = string.Empty;
        string _recipeId = string.Empty;

        private string _toWrite = string.Empty;
        //private StepsBeforeSendingCarrier _currentStepBeforeSendingCarrier;

        private static FunctionsForPWA500W_NRD _functionsForPWA500 = null;

        private static LotHistoryLog _lotHistoryLog = null;
        private const LoadPortLoadingMode LoadingMode = LoadPortLoadingMode.Foup;
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
            int relIndex = lpIndex * 4;
            indexOfDigital = (int)Define.DefineEnumProject.DigitalIO.PWA500W.EN_DIGITAL_IN.LP1_RUN + relIndex;

            return true;
        }
        protected override void ExecuteAtAlways()
        {
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
                [RFIDReadKeys.KeyParamPortId] = _functionsForPWA500.GetPortName(PortId),
                [RFIDReadKeys.KeyParamOperatorId] = "AUTO"
            };

            // 2024.12.29. jhlim [ADD] 고객사 요청으로 id read 딜레이 추가
            SetDelayForSequence(DelayBeforeIdReadScenario);

            return _scenarioOperator.UpdateScenarioParam(GetTaskName(), ScenarioTypeToIdRead, param);
        }
        protected override CommandResults ExecuteScenarioToCarrierIdRead()
        {
            return RunScenario(ScenarioTypeToIdRead);
        }
        protected override bool UpdateParamToIdVarification()
        {
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

            return _scenarioOperator.UpdateScenarioParam(GetTaskName(), ScenarioTypeToSlotVerification, param);
        }
        protected override CommandResults ExecuteToSlotMapVarification()
        {
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
            bool needExecuteMerge = IsFinalProcessNeeded();

            // 2025.03.17. jhlim [MOD] 고객사 요청으로 SlotMapping도 Merge와 동일하게 모든 웨이퍼가 터미네이트 되면 SlotMapping 진행하지 않도록 변경
            // 이로써 슬롯매핑은 머지 실행 이후에 하도록 하며, EmptyCarrier의 경우 SlotMapping 하지 않도록 한다.
            if (false == needExecuteMerge)
            {
                // 터미네이트 되는 경우 History Log가 넘어가지 않는다..
                var isCore = MySubstrateType.Equals(SubstrateType.Empty);
                var carrierId = _carrierServer.GetCarrierId(PortId);
                var lotId = _carrierServer.GetCarrierLotId(PortId);
                var subs = _substrateManager.GetSubstratesAtLoadPort(PortId);
                List<string> substrateNames = new List<string>();
                foreach (var item in subs)
                {
                    substrateNames.Add(item.Value.Name);
                }

                _lotHistoryLog.BackupCarrierHistory(PortId, carrierId, lotId, substrateNames, isCore);

                return false;
            }
            else
            {
                // TODO : 빈소터와의 운영상 차이점 -> 코어는 현재 운영상 랏 머지/슬롯 매핑이 없다. -> 따라서 코어가 아닌 경우만 체크한다.
                if (MySubstrateType.Equals(SubstrateType.Core))
                    return false;
                    
                if (IsAlreadyLotMerged())
                    return false;

                if (false == MakeScenarioParamForMergeLot(ref scenarioParam))
                    return false;

                scenarioInfo.Scenario = ScenarioTypeToLotMerge;
            }

            scenarioInfo.ScenarioParams = scenarioParam;

            return true;
        }
        protected override void ExecuteAfterScenarioCompletion(EN_SCENARIO scenario, EN_SCENARIO_RESULT result, Dictionary<string, string> scenarioParam, Dictionary<string, string> additionalParams)
        {
            switch (scenario)
            {
                case EN_SCENARIO.SCENARIO_REQ_LOT_MERGE_CORE_1:
                case EN_SCENARIO.SCENARIO_REQ_LOT_MERGE_CORE_2:
                case EN_SCENARIO.SCENARIO_REQ_LOT_MERGE_CORE_3:
                case EN_SCENARIO.SCENARIO_REQ_LOT_ID_MERGE_AND_CHANGE_BIN_1:
                case EN_SCENARIO.SCENARIO_REQ_LOT_ID_MERGE_AND_CHANGE_BIN_2:
                case EN_SCENARIO.SCENARIO_REQ_LOT_ID_MERGE_AND_CHANGE_BIN_3:
                    {
                        if (false == result.Equals(EN_SCENARIO_RESULT.COMPLETED))
                        {
                            return;
                        }

                        // TODO : 빈소터와의 운영상 차이점 -> 코어는 현재 운영상 랏 머지/슬롯 매핑이 없다.
                        if (false == MySubstrateType.Equals(SubstrateType.Core))
                        {
                            #region <머지할 랏을 받아온다.>
                            var scenarioResult = GetScenarioResultData(scenario);
                            if (false == ApplyResultOfMergingLot(scenarioResult))
                                return;
                            #endregion </머지할 랏을 받아온다.>

                            // 2025.03.17. jhlim [MOD] 고객사 요청으로 SlotMapping도 Merge와 동일하게 모든 웨이퍼가 터미네이트 되면 SlotMapping 진행하지 않도록 변경
                            #region <슬롯매핑 실행 조건 체크>
                            if (false == IsFinalProcessNeeded())
                            {
                                return;
                            }
                            #endregion </슬롯매핑 실행 조건 체크>
                            // 2025.03.17. jhlim [END]

                            // 2024.09.29. jhlim [MOD] 고객사 요청으로 순서 변경(랏 머지&체인지 후 매핑 진행)
                            Dictionary<string, string> param = new Dictionary<string, string>();
                            if (false == MakeScenarioParamForSlotMapping(ref param))
                                return;

                            _carrierServer.SetAttribute(PortId, PWA500CarrierAttributes.KeyProcessStepBeforeSendingCarrier, ((int)StepsBeforeSendingCarrier.MergeAndChangeCompleted).ToString());
                            _carrierServer.SaveCarrierData(PortId);

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
                case EN_SCENARIO.SCENARIO_SLOT_WAFER_MAPPING_CORE_3:
                case EN_SCENARIO.SCENARIO_SLOT_WAFER_MAPPING_EMPTY_TAPE:
                case EN_SCENARIO.SCENARIO_SLOT_WAFER_MAPPING_BIN_1:
                case EN_SCENARIO.SCENARIO_SLOT_WAFER_MAPPING_BIN_2:
                case EN_SCENARIO.SCENARIO_SLOT_WAFER_MAPPING_BIN_3:
                    {
                        if (false == result.Equals(EN_SCENARIO_RESULT.COMPLETED))
                            return;

                        // TODO : 빈소터와의 운영상 차이점 -> 코어는 현재 운영상 랏 머지/슬롯 매핑이 없다.
                        if (MySubstrateType.Equals(SubstrateType.Core))
                        {
                            return;
                        }
                        _recovery.LotCompleted = true;
                        string carrierId = _carrierServer.GetCarrierId(PortId);
                        List<string> substrates = null;
                        if (PortId != 4)
                        {
                            var temporarySubstrates = _substrateManager.GetSubstratesAtLoadPort(PortId);
                            if(temporarySubstrates != null)
                            {
                                substrates = new List<string>();
                                foreach (var item in temporarySubstrates)
                                {
                                    substrates.Add(item.Value.Name);
                                }
                            }
                        }

                        bool isCore = (false == scenario.Equals(EN_SCENARIO.SCENARIO_SLOT_WAFER_MAPPING_BIN_1));
                        //bool isCore = scenario.Equals(EN_SCENARIO.SCENARIO_SLOT_WAFER_MAPPING_CORE_1) ||
                        //    scenario.Equals(EN_SCENARIO.SCENARIO_SLOT_WAFER_MAPPING_CORE_2);

                        string lotId = _carrierServer.GetCarrierLotId(PortId);
                        if (false == isCore)
                        {
                            lotId = _toWrite;
                        }

                        _lotHistoryLog.BackupCarrierHistory(PortId, carrierId, lotId, substrates, isCore);
                        _carrierServer.SetAttribute(PortId, PWA500CarrierAttributes.KeyProcessStepBeforeSendingCarrier, ((int)StepsBeforeSendingCarrier.SlotMappingCompleted).ToString());

                        if (MySubstrateType.Equals(SubstrateType.Core))
                        {
                            // 귀찮으니 비동기 실행
                            string partId = _carrierServer.GetAttribute(PortId, PWA500CarrierAttributes.KeyPartId);
                            string stepId = _carrierServer.GetAttribute(PortId, PWA500CarrierAttributes.KeyStepSeq);
                            string lotType = _carrierServer.GetAttribute(PortId, PWA500CarrierAttributes.KeyLotType);

                            _functionsForPWA500.ExecuteScenarioAsyncToCarrierUnload(lotId, partId, stepId, lotType);
                        }

                        _carrierServer.SaveCarrierData(PortId);
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
                    {
                        _commandResult.ActionName = ScenarioTypeToCarrierLoad.ToString();
                        EN_SCENARIO typeOfScenario = ScenarioTypeToCarrierLoad;

                        _functionsForPWA500.EnqueueScenarioCarrierHandlingAsync(PortId, LoadingMode, string.Empty, typeOfScenario);
                        return true;
                    }

                default:
                    {
                        _commandResult.ActionName = ScenarioTypeToCarrierLoad.ToString();
                        EN_SCENARIO typeOfScenario = ScenarioTypeToCarrierLoad;

                        _functionsForPWA500.EnqueueScenarioCarrierHandlingAsync(PortId, LoadingMode, CarrierLotIdType.PEMAC.ToString(), typeOfScenario);
                        return true;
                    }
            }
        }
        protected override CommandResults ExecuteScenarioToLoadCarrier()
        {
            _commandResult.CommandResult = CommandResult.Completed;
            return _commandResult;
        }
        protected override bool UpdateParamToUnloadCarrier()
        {
            _commandResult.ActionName = ScenarioTypeToCarrierUnload.ToString();
            EN_SCENARIO typeOfScenario = ScenarioTypeToCarrierUnload;
            string carrierLotId = _carrierServer.GetCarrierLotId(PortId);

            var param = _functionsForPWA500.MakeParamToOHTHandling(PortId, LoadingMode, carrierLotId, typeOfScenario);

            return _scenarioOperator.UpdateScenarioParam(GetTaskName(), ScenarioTypeToCarrierUnload, param);
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
        protected override bool PrepareBeforeSendingCarrier()
        {
            // 아래 작업 전 각 스텝에서 할 일 메서드화가 선행돼야함
            // 스텝에 따라 아래 액션을 Enqueue
            // 머지
            // 슬롯매핑
            // 태깅

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
            //            var stepString = _carrierServer.GetAttribute(PortId, PWA500WCarrierAttributeKeys.KeyProcessStepBeforeSendingCarrier);
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
            //                                PWA500WCarrierAttributeKeys.KeyProcessStepBeforeSendingCarrier,
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
                var loadingMode = LoadingMode;
                switch (MySubstrateType)
                {
                    case SubstrateType.Core:
                        {
                            // TODO : 빈소터와의 운영상 차이점 -> 무조건 터미네이트 되기 때문에 지정된 이름으로 RFID Writting 한다.
                            //bool needToMerge = GetSubstrateToMerge(out Dictionary<int, Substrate> substrates);
                            //if (substrates.Count > 0 || needToMerge)
                            //{
                            //    return new CommandResults(string.Empty, CommandResult.Skipped);
                            //}
                            string lotId = string.Empty;
                            if (false == GetLotIdToWrite(LoadingMode, ref lotId))
                                return new CommandResults(string.Empty, CommandResult.Skipped);

                            _toWrite = lotId;

                            //switch (loadingMode)
                            //{
                            //    case LoadPortLoadingMode.Cassette:
                            //        //_carrierIdToWrite = CarrierLotIdType.RCASSETTE.ToString();
                            //        _carrierIdToWrite = GetParameter(Recipe.PARAM_EQUIPMENT.WrittingLotIdToCassetteWhenLotIsTerminated, "RCASSETTE");
                            //        break;
                            //    case LoadPortLoadingMode.Foup:
                            //        _carrierIdToWrite = GetParameter(Recipe.PARAM_EQUIPMENT.WrittingLotIdToMACWhenLotIsTerminated, "PRMAC");
                            //        // 2024.11.27. jhlim [MOD] 고객사 요청으로 명칭 변경
                            //        //_carrierIdToWrite = CarrierLotIdType.PRMAC.ToString();
                            //        // 2024.11.27. jhlim [END]
                            //        break;
                            //    default:
                            //        break;
                            //}
                        }
                        break;

                    // W는 공테이프 캐리어가 BIN 캐리어이므로 쓸 필요가 없다.
                    //case SubstrateType.Empty:
                    //    {
                    //        var substrates = _substrateManager.GetSubstratesAtLoadPort(PortId);
                    //        if (substrates.Count > 0)
                    //            return new CommandResults(string.Empty, CommandResult.Skipped);
                    //        else
                    //        {
                    //            switch (loadingMode)
                    //            {
                    //                case LoadPortLoadingMode.Cassette:
                    //                    _carrierIdToWrite = CarrierLotIdType.ECASSETTE.ToString();
                    //                    break;
                    //                case LoadPortLoadingMode.Foup:
                    //                    // 2024.11.27. jhlim [MOD] 고객사 요청으로 명칭 변경
                    //                    _carrierIdToWrite = CarrierLotIdType.PEMAC.ToString();
                    //                    // 2024.11.27. jhlim [END]
                    //                    break;
                    //                default:
                    //                    break;
                    //            }
                    //        }
                    //    }
                    //    break;

                    default:
                        break;
                }

                var result = _rfidManager.WriteLotId(LoadPortIndex, loadingMode, _toWrite);
                if (result.CommandResult == CommandResult.Completed)
                {
                    _carrierServer.SetCarrierLotId(PortId, _toWrite);
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
                        return new CommandResults(string.Empty, CommandResult.Skipped);

                    default:
                        break;
                }

                var loadingMode = LoadingMode;
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
        private bool IsFinalProcessNeeded()
        {
            switch (MySubstrateType)
            {
                case SubstrateType.Core:
                    // TODO : 빈소터와의 운영상 차이점 -> 랏 머지가 없다.
                    return false;
                    //return GetSubstrateToMerge(out _);
                   
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
        private bool MakeScenarioParamForSlotMapping(ref Dictionary<string, string> scenarioParam)
        {
            if (scenarioParam == null)
                scenarioParam = new Dictionary<string, string>();

            scenarioParam.Clear();

            // 1~6 포트 모두 진행
            var substrates = _substrateManager.GetSubstratesAtLoadPort(PortId);
            string lotId = string.Empty;
            // TODO : 빈소터와의 운영상 차이점 -> Core는 SlotMapping 하지 않는다.
            if (MySubstrateSize.Equals(SubstrateType.Core))
                return false;

            if (substrates.Count <= 0)
                return false;
            var substrateFirst = substrates.First();
            lotId = substrateFirst.Value.LotId;

            //switch (MySubstrateType)
            //{
            //    case SubstrateType.Sort_12:
            //        {
            //            if (substrates.Count <= 0)
            //                return false;
            //            var substrateFirst = substrates.First();
            //            lotId = substrateFirst.Value.LotId;
            //        }
            //        break;
            //    default:
            //        {
            //            lotId = _carrierServer.GetCarrierLotId(PortId);
            //        }
            //        break;
            //}

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
                    id = substrate.Name;
                    qty = substrate.GetAttribute(PWA500SubstrateAttributes.ChipQty);
                    if (qty.Equals("0"))
                    {
                        // 2025.03.17. jhlim [MOD] 슬롯매핑 또한 자재 수량이 0이면 올리지 않는다.
                        id = string.Empty;
                        // 2025.03.17. jhlim [END]
                        qty = string.Empty;
                    }
                    else
                    {
                        waferQty++;
                    }

                    substratesToMapping[i] = Tuple.Create(id, qty);
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

            // TODO : 빈소터와의 운영상 차이점 -> 아래 두개는 가치효율 데이터인데, W는 아직 없다..
            // 2025.05.08. jhlim [ADD] 고객사 요청으로 캐리어내 웨이퍼 수량 추가
            //scenarioParam[SlotMappingKeys.KeyParamWaferQty] = waferQty.ToString();
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

                default:
                    // Bin의 경우 첫 번째 Lot 이름을 대표이름으로 병합한다. -> LotId Change 이후 새로운 Lot Id 부여받음
                    mergedLotId = firstSubstrate.Value.LotId;
                    return true;
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
                
                default:
                    // Bin의 경우 첫 번째 Lot 이름을 대표이름으로 병합한다. -> LotId Change 이후 새로운 Lot Id 부여받음
                    lotId = firstSubstrate.Value.LotId;
                    break;
            }

            scenarioParam[LotMergeKeys.KeyParamLotId] = lotId;
            scenarioParam[LotMergeKeys.KeyParamCarrierId] = carrierId;


            string partId = firstSubstrate.Value.GetAttribute(PWA500SubstrateAttributes.PartId);
            string recipeId = _functionsForPWA500.GetRecipeId();

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
            if (false == MySubstrateType.Equals(SubstrateType.Core))
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
                    scenarioParam[keyForId] = substrateIds[i- 1];
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
            var useChange = MySubstrateType.Equals(SubstrateType.Bin1);
            useChange |= MySubstrateType.Equals(SubstrateType.Bin2);
            useChange |= MySubstrateType.Equals(SubstrateType.Bin3);
            useChange |= MySubstrateType.Equals(SubstrateType.Empty);
            _lotHistoryLog.WriteHistoryForMerge(PortId, carrierId, lotId, useChange, lotIdToMerge);

            return true;
        }
        private void AssignSubstrateInfoByCarrierRFIDInfo(string lotId)
        {
            var substrates = _substrateManager.GetSubstratesAtLoadPort(PortId);

            foreach (var item in substrates)
            {
                string prevLotId = item.Value.LotId;
                if (string.IsNullOrEmpty(prevLotId)/* || false == item.Value.LotId.Equals(lotId)*/)
                {
                    //item.Value.SetLotId(lotId);
                    _substrateManager.SetLotIdByKey(item.Value.UniqueKey, lotId);
                }

                string prevParentLotId = item.Value.GetAttribute(PWA500SubstrateAttributes.ParentLotId);
                if (string.IsNullOrEmpty(prevParentLotId))
                {
                    _substrateManager.SetAttributeByKey(item.Value.UniqueKey, PWA500SubstrateAttributes.ParentLotId, lotId);
                }

                var ringId = item.Value.GetAttribute(PWA500SubstrateAttributes.RingId);
                if (string.IsNullOrEmpty(ringId))
                {
                    _substrateManager.SetAttributeByKey(item.Value.UniqueKey, PWA500SubstrateAttributes.RingId, item.Value.UniqueKey);
                }
                _substrateManager.SaveDataByKey(item.Value.UniqueKey);
            }
        }
        private void ExecuteAfterScenarioCompletedForHistory(EN_SCENARIO scenario)
        {
            if (scenario.Equals(ScenarioTypeToIdRead))
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
                if (scenario.Equals(ScenarioTypeToRequestLotInfo))
                {
                    #region <Lot Info 갱신>
                    var scenarioResult = GetScenarioResultData(ScenarioTypeToRequestLotInfo);
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

                    _carrierServer.SetCarrierLotId(PortId, _lotId);
                    _carrierServer.SetAttribute(PortId, PWA500CarrierAttributes.KeyPartId, _partId);
                    _carrierServer.SetAttribute(PortId, PWA500CarrierAttributes.KeyStepSeq, _stepSeq);
                    _carrierServer.SetAttribute(PortId, PWA500CarrierAttributes.KeyLotType, _lotType);
                    _carrierServer.SetAttribute(PortId, PWA500CarrierAttributes.KeyLotQty, _lotQty);
                    _carrierServer.SaveCarrierData(PortId);

                    string carrierId = _carrierServer.GetCarrierId(PortId);
                    _lotHistoryLog.WriteHistoryForLotInfo(PortId, carrierId, _lotId, _partId, _stepSeq, _lotType, _lotQty);
                    #endregion </Lot Info 갱신>
                }
                else if (scenario.Equals(ScenarioTypeToSlotVerification))
                {
                    #region <Slot Info 갱신>
                    var scenarioResult = GetScenarioResultData(ScenarioTypeToSlotVerification);
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
                                            if (scenarioResult.ContainsKey(keyForLotId))
                                            {
                                                //substrates[index].SetLotId(scenarioResult[keyForLotId]);
                                                _substrateManager.SetLotIdByKey(substrates[index].UniqueKey, scenarioResult[keyForLotId]);
                                                if (string.IsNullOrEmpty(substrates[index].GetAttribute(PWA500SubstrateAttributes.ParentLotId)))
                                                {
                                                    _substrateManager.SetAttributeByKey(substrates[index].UniqueKey, PWA500SubstrateAttributes.ParentLotId, scenarioResult[keyForLotId]);
                                                }
                                            }
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

                                                //substrates[index].SetAttribute(PWA500WSubstrateAttributes.RingId, scenarioResult[keyForSubstrateId]);
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
                                            //    [PWA500SubstrateAttributes.LotType] = _lotType,
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
                                        //    [PWA500SubstrateAttributes.LotType] = _lotType,
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
        #endregion </Internal Interfaces>

        #endregion </Methods>
    }

    class TaskLoadPortRecovery500W : Work.RecoveryData
    {
        public TaskLoadPortRecovery500W(string taskName, int nPortCount)
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