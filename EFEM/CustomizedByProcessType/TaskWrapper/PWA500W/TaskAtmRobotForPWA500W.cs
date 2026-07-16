using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using TickCounter_;

using EFEM.Defines.Common;
using EFEM.Defines.AtmRobot;
using EFEM.Defines.LoadPort;
using EFEM.MaterialTracking;
using EFEM.Defines.MaterialTracking;
using EFEM.CustomizedByProcessType.PWA500W;
using EFEM.CustomizedByProcessType.PWA500Common;

using FrameOfSystem3.Recipe;
using FrameOfSystem3.SECSGEM;
using FrameOfSystem3.SECSGEM.Scenario;

using Define.DefineEnumProject.Task.AtmRobot;
using FrameOfSystem3.SECSGEM.DefineSecsGem;

// ConfigTask에서 이 namespace를 가지고 클래스 타입을 가져오기 때문에 변경 불가
namespace FrameOfSystem3.Task
{
    class TaskAtmRobotForPWA500W : TaskAtmRobot
    {
        #region <Constructors>
        public TaskAtmRobotForPWA500W(int nIndexOfTask, string strTaskName)
            : base(nIndexOfTask, strTaskName, new TaskAtmRobotRecovery500W_300(strTaskName, nIndexOfTask))
        {
            Ticks = new TickCounter();
            _functionsForPWA500 = FunctionsForPWA500W_NRD.Instance;
            ProcessModuleName = _processGroup.GetProcessModuleName(ProcessModuleIndex);
            _lotHistoryLog = LotHistoryLog.Instance;
        }
        #endregion </Constructors>

        #region <Fields>
        private CommandResults _commandResult = new CommandResults("", CommandResult.Invalid);
        private const EN_SCENARIO ScenarioRecipeDownload = EN_SCENARIO.SCENARIO_REQ_RECIPE_DOWNLOAD;
        private const EN_SCENARIO ScenarioCoreTrackIn = EN_SCENARIO.SCENARIO_REQ_TRACK_IN;
        private const EN_SCENARIO ScenarioLotMatch = EN_SCENARIO.SCENARIO_REQ_LOT_MATCH;
        private const EN_SCENARIO ScenarioBinWaferIdAssign = EN_SCENARIO.SCENARIO_REQ_BIN_WAFER_ID_ASSIGN;
        private const EN_SCENARIO ScenarioSendClientToBinWaferIdAssign = EN_SCENARIO.SCENARIO_ASSIGN_SUBSTRATE_ID;
        private const EN_SCENARIO ScenarioBinWorkEnd = EN_SCENARIO.SCENARIO_BIN_WORK_END;
        private const EN_SCENARIO ScnearioBinTrackOut = EN_SCENARIO.SCENARIO_REQ_BIN_WAFER_TRACK_OUT;
        private const EN_SCENARIO ScenarioReqBinPartId = EN_SCENARIO.SCENARIO_BIN_PART_ID_INFO_REQ;
        private const EN_SCENARIO ScenarioSendClientUploadBinFile = EN_SCENARIO.SCENARIO_REQ_UPLOAD_BINFILE;
        private const EN_SCENARIO ScenarioUploadBinData = EN_SCENARIO.SCENARIO_BIN_DATA_UPLOAD;
        private const EN_SCENARIO ScenarioProcessStart = EN_SCENARIO.SCENARIO_PROCESS_START;
        private const EN_SCENARIO ScenarioProcessEnd = EN_SCENARIO.SCENARIO_PROCESS_END;
        private QueuedScenarioInfo _executingScenario = new QueuedScenarioInfo();
        private readonly Queue<EN_SCENARIO> QueuedScenarioForBinSubstrate = new Queue<EN_SCENARIO>();
        private readonly Queue<EN_SCENARIO> QueuedScenarioForCoreSubstrate = new Queue<EN_SCENARIO>();
        private string _temporaryDescription = string.Empty;

        private readonly TickCounter Ticks = null;
        private const int ProcessModuleIndex = 0;
        private readonly string ProcessModuleName = string.Empty;

        // 시간 파라메터화가 필요한가?
        private const uint TimeoutShort = 30000;
        private const uint TimeoutLong = 60000;
        private CommandResults _result = new CommandResults("", CommandResult.Error);
        private int _subStepInterface;

        //private const string ErrorDescriptionForInvalidSubstratePortInfo = "Invalid Substrate Port Info";
        //private const string ErrorDescriptionForDoesntHaveCarrier = "Does not have carrier at loadport";
        //private const string ErrorDescriptionForLoadPortNotEnabled = "Loadport is not enabled";
        //private const string ErrorDescriptionForDoorIsNotOpened = "Loadport door is not opened";
        //private const string ErrorDescriptionForSlotIsFull = "All of the slot is full";

        //private const string ErrorDescriptionForAssignSubstrateId = "Cannot getting a assigned substrate Id";
        //private const string ErrorDescriptionForRequestPartId = "Cannot getting a assigned part Id";
        private static FunctionsForPWA500W_NRD _functionsForPWA500 = null;
        private static LotHistoryLog _lotHistoryLog = null;

        private string _newSubstrateId;
        private string _newPartId;

        private BinDataToUploadFromPWA500 _binDataToUpload;

        private List<Substrate> _substratesAtProcessModule = new List<Substrate>();
        #endregion </Fields>

        #region <Properties>
        private string MachineName
        {
            get
            {
                return GetParameter(PARAM_EQUIPMENT.MachineName, string.Empty);
            }
        }
        #endregion </Properties>

        #region <Methods>

        #region <Overrids>

        #region <Input/Output>
        protected override bool GetBusySignalIndex(int index, ref int indexOfDigital)
        {
            indexOfDigital = (int)Define.DefineEnumProject.DigitalIO.PWA500W.EN_DIGITAL_IN.ROBOT_BUSY_STATUS;
            return true;
        }
        protected override bool GetAlarmSignalIndex(int index, ref int indexOfDigital)
        {
            indexOfDigital = (int)Define.DefineEnumProject.DigitalIO.PWA500W.EN_DIGITAL_IN.ROBOT_ALARM_STATUS;
            return true;
        }
        protected override bool GetServoSignalIndex(int index, ref int indexOfDigital)
        {
            indexOfDigital = (int)Define.DefineEnumProject.DigitalIO.PWA500W.EN_DIGITAL_IN.ROBOT_SERVO_ON_OFF_STATUS;
            return true;
        }
        #endregion </Input/Output>

        #region <Scenario>
        protected override void InitScenarioInfoPick()
        {
            QueuedScenarioForCoreSubstrate.Clear();
            QueuedScenarioForBinSubstrate.Clear();

            if (false == _scenarioOperator.UseScenario)
                return;

            // 2024.10.30. jhlim [MOD] 트랙인보다 레시피 다운로드가 선행되어야 해서 변경
            bool isManual = IsManual();
            if (false == GetWorkingInformation(isManual, ref _workingInfo, ref _temporaryDescription))
                return;

            if (_workingInfo.LocationType != ModuleType.LoadPort)
                return;

            if (false == LocationServer.FindLocationById(_workingInfo.LocationId, out var location))
                return;

            if (!(location is LoadPortLocation lpLocation))
                return;

            #region <Track in or lot match>
            int portId = lpLocation.PortId;
            if (false == _substrateManager.GetSubstrateByLocationAndKey(location, string.Empty, out var substrate))
                return;

            var key = substrate.UniqueKey;
            string subTypeString = substrate.GetAttribute(PWA500SubstrateAttributes.SubstrateType);
            if (false == Enum.TryParse(subTypeString, out SubstrateType substrateType))
                return;

            string sourceCarrierId = _carrierServer.GetCarrierId(portId);
            switch (substrateType)
            {
                case SubstrateType.Core:
                    {
                        if (IsFirstSubstrateAtLoadPort(sourceCarrierId, portId, key))
                        {
                            bool useDownloadingRecipe = GetParameter(PARAM_COMMON.UseDownloadingRecipe, true);
                            if (useDownloadingRecipe)
                            {
                                var downloadingStatusString = _carrierServer.GetAttribute(portId, PWA500CarrierAttributes.KeyDownloadingRecipeCompleted);
                                bool.TryParse(downloadingStatusString, out var downloadingCompleted);
                                if (false == downloadingCompleted)
                                {
                                    QueuedScenarioForCoreSubstrate.Enqueue(ScenarioRecipeDownload);
                                }
                            }

                            var trackInStatusString = _carrierServer.GetAttribute(portId, PWA500CarrierAttributes.KeyTrackInCompleted);
                            bool.TryParse(trackInStatusString, out var trackInCompleted);

                            if (false == trackInCompleted)
                            {
                                QueuedScenarioForCoreSubstrate.Enqueue(ScenarioCoreTrackIn);
                            }

                            SetDelayForSequence(1000);
                        }
                        
                        bool isLast = IsLastSubstrateAtLoadPortBeforePick(portId, key);
                        _substrateManager.SetAttributeByKey(substrate.UniqueKey, PWA500SubstrateAttributes.IsLastSubstrate, isLast.ToString());
                    }
                    break;
                case SubstrateType.Empty:
                case SubstrateType.Bin1:
                case SubstrateType.Bin2:
                case SubstrateType.Bin3:
                    //case SubstrateType.Sort_12:
                    {
                        if (IsFirstSubstrateAtLoadPort(sourceCarrierId, portId, key))
                        {
                            var trackInStatusString = _carrierServer.GetAttribute(portId, PWA500CarrierAttributes.KeyTrackInCompleted);
                            bool.TryParse(trackInStatusString, out var trackInCompleted);

                            if (false == trackInCompleted)
                            {
                                QueuedScenarioForBinSubstrate.Enqueue(ScenarioLotMatch);
                            }
                        }
                    }
                    break;
            }
            #endregion </Track in or lot match>
        }
        protected override CommandResults UpdateParamToBeforePick()
        {
            if (false == _scenarioOperator.UseScenario)
            {
                _commandResult.CommandResult = CommandResult.Skipped;
                return _commandResult;
            }

            // 2024.10.30. jhlim [MOD] 트랙인보다 레시피 다운로드가 선행되어야 해서 변경
            bool isManual = IsManual();
            if (false == GetWorkingInformation(isManual, ref _workingInfo, ref _temporaryDescription))
            {
                _commandResult.CommandResult = CommandResult.Error;
                _commandResult.Description = _temporaryDescription;
                return _commandResult;
            }

            if (_workingInfo.LocationType != ModuleType.LoadPort)
            {
                _commandResult.CommandResult = CommandResult.Skipped;
                return _commandResult;
            }

            if (false == LocationServer.FindLocationById(_workingInfo.LocationId, out var location))
            {
                _commandResult.CommandResult = CommandResult.Skipped;
                return _commandResult;
            }

            if (!(location is LoadPortLocation lpLocation))
            {
                _commandResult.CommandResult = CommandResult.Skipped;
                return _commandResult;
            }

            #region <Track in or lot match>
            int portId = lpLocation.PortId;
            if (false == _substrateManager.GetSubstrateByLocationAndKey(location, string.Empty, out var substrate))
            {
                _commandResult.CommandResult = CommandResult.Error;
                _commandResult.Description = "Cannot find info at loadport";
                return _commandResult;
            }

            //string substrateName = substrate.Name;
            string subTypeString = substrate.GetAttribute(PWA500SubstrateAttributes.SubstrateType);
            if (false == Enum.TryParse(subTypeString, out SubstrateType substrateType))
            {
                _commandResult.CommandResult = CommandResult.Error;
                _commandResult.Description = "Cannot find substrate info at loadport";
                return _commandResult;
            }

            //string sourceCarrierId = _carrierServer.GetCarrierId(portId);
            switch (substrateType)
            {
                case SubstrateType.Core:
                    {
                        if (QueuedScenarioForCoreSubstrate.Count <= 0)
                        {
                            _commandResult.CommandResult = CommandResult.Skipped;
                            return _commandResult;
                        }

                        var scenario = QueuedScenarioForCoreSubstrate.Dequeue();
                        InitResult(scenario);
                        switch (scenario)
                        {
                            case ScenarioRecipeDownload:
                                {
                                    Dictionary<string, string> scenarioParam = ScenarioParameterBuilder.MakeScenarioParamToRecipeDownload(substrate);
                                    if (scenarioParam == null)
                                    {
                                        _commandResult.CommandResult = CommandResult.Error;
                                        _commandResult.Description = "Invalid scenario param";
                                        return _commandResult;
                                    }

                                    _executingScenario = new QueuedScenarioInfo
                                    {
                                        Scenario = scenario,
                                        ScenarioParams = scenarioParam,
                                        AdditionalParams = new Dictionary<string, string>()
                                        {
                                            [AdditionalParamKeys.KeySubstrateType] = subTypeString
                                        }
                                    };

                                    if (false == _scenarioOperator.UpdateScenarioParam(GetTaskName(), scenario, scenarioParam))
                                    {
                                        _commandResult.CommandResult = CommandResult.Error;
                                        _commandResult.Description = "Failed to update for scenario param";
                                    }
                                    else
                                    {
                                        _commandResult.CommandResult = CommandResult.Completed;
                                        _commandResult.Description = string.Empty;
                                    }
                                    return _commandResult;
                                }

                            case ScenarioCoreTrackIn:
                                {
                                    string description = string.Empty;
                                    // TODO : 빈소터와의 운영상 차이점 -> BIN에서는 이종 파트 정보가 오는 것을 방지하기 위해 인터락을 했었음.
                                    //if (false == IsValidPartIdsForSubstrates(ref substrate, ref description))
                                    //{
                                    //    _commandResult.CommandResult = CommandResult.Error;
                                    //    _commandResult.Description = description;
                                    //    return _commandResult;
                                    //}

                                    Dictionary<string, string> scenarioParam = ScenarioParameterBuilder.MakeScenarioParamToCoreTrackIn(portId, substrate);
                                    if (scenarioParam == null)
                                    {
                                        _commandResult.CommandResult = CommandResult.Error;
                                        _commandResult.Description = "Invalid scenario param";
                                        return _commandResult;
                                    }

                                    _executingScenario = new QueuedScenarioInfo
                                    {
                                        Scenario = scenario,
                                        ScenarioParams = scenarioParam,
                                        AdditionalParams = new Dictionary<string, string>()
                                        {
                                            [AdditionalParamKeys.KeySubstrateType] = subTypeString
                                        }
                                    };

                                    if (false == _scenarioOperator.UpdateScenarioParam(GetTaskName(), scenario, scenarioParam))
                                    {
                                        _commandResult.CommandResult = CommandResult.Error;
                                        _commandResult.Description = "Failed to update for scenario param";
                                    }
                                    else
                                    {
                                        _commandResult.CommandResult = CommandResult.Completed;
                                        _commandResult.Description = string.Empty;
                                    }
                                    return _commandResult;
                                }
                            default:                                
                                {
                                    _commandResult.CommandResult = CommandResult.Skipped;
                                    _commandResult.Description = string.Empty;
                                    return _commandResult;
                                }
                        }
                    }
                case SubstrateType.Empty:
                case SubstrateType.Bin1:
                case SubstrateType.Bin2:
                case SubstrateType.Bin3:
                    {
                        if (QueuedScenarioForBinSubstrate.Count <= 0)
                        {
                            _commandResult.CommandResult = CommandResult.Skipped;
                            return _commandResult;
                        }

                        var scenario = QueuedScenarioForBinSubstrate.Dequeue();
                        InitResult(ScenarioLotMatch);

                        if (false == scenario.Equals(ScenarioLotMatch))
                        {
                            _commandResult.CommandResult = CommandResult.Skipped;
                            return _commandResult;
                        }

                        // 매 장 LotMatch 발생한다.
                        string lotId = _carrierServer.GetCarrierLotId(portId);
                        string carrierId = _carrierServer.GetCarrierId(portId);
                        Dictionary<string, string> scenarioParam = ScenarioParameterBuilder.MakeScenarioParamToLotMatch(portId, lotId, carrierId);
                        if (scenarioParam == null)
                        {
                            _commandResult.CommandResult = CommandResult.Error;
                            _commandResult.Description = "Invalid scenario param";
                            return _commandResult;
                        }

                        _executingScenario = new QueuedScenarioInfo
                        {
                            Scenario = scenario,
                            ScenarioParams = scenarioParam
                        };


                        if (false == _scenarioOperator.UpdateScenarioParam(GetTaskName(), scenario, scenarioParam))
                        {
                            _commandResult.CommandResult = CommandResult.Error;
                            _commandResult.Description = "Failed to update for scenario param";
                        }
                        else
                        {
                            _commandResult.CommandResult = CommandResult.Completed;
                            _commandResult.Description = string.Empty;
                        }
                        return _commandResult;
                    }

                default:
                    {
                        _commandResult.CommandResult = CommandResult.Skipped;
                        _commandResult.Description = string.Empty;
                        return _commandResult;
                    }
            }
            #endregion </Track in or lot match>

            // 2024.10.30. jhlim [END]
        }
        protected override CommandResults ExecuteScenarioToBeforePick()
        {
            return RunScenario(_executingScenario.Scenario);
        }
        protected override CommandResults UpdateParamToAfterPick()
        {
            _commandResult.CommandResult = CommandResult.Skipped;
            return _commandResult;
        }
        protected override CommandResults ExecuteScenarioToAfterPick()
        {
            _commandResult.CommandResult = CommandResult.Skipped;
            return _commandResult;
        }
        protected override void InitScenarioInfoPlace()
        {
            QueuedScenarioForCoreSubstrate.Clear();
            QueuedScenarioForBinSubstrate.Clear();

            if (false == _scenarioOperator.UseScenario)
                return;

            bool isManual = IsManual();
            if (false == GetWorkingInformation(isManual, ref _workingInfo, ref _temporaryDescription))
                return;

            if (false == _substrateManager.GetSubstrateAtRobot(RobotName, _workingInfo.ActionArm, out var substrate))
                return;

            string subTypeString = substrate.GetAttribute(PWA500SubstrateAttributes.SubstrateType);
            if (false == Enum.TryParse(subTypeString, out SubstrateType substrateType))
                return;

            //var location = _workingInfo.Location;
            switch (substrateType)
            {
                case SubstrateType.Core:
                    {
                        //if (location is ProcessModuleLocation)
                        if (_workingInfo.LocationType == ModuleType.ProcessModule)
                        {
                            //QueuedScenarioForCoreSubstrate.Enqueue(ScenarioProcessStart);
                            if (IsFirstSubstrateAtLoadPort(substrate.SourceCarrierId, substrate.SourcePortId, substrate.UniqueKey))
                            {
                                QueuedScenarioForCoreSubstrate.Enqueue(ScenarioProcessStart);
                            }
                        }
                        else if (_workingInfo.LocationType == ModuleType.LoadPort)
                        {
                            var isLast = _substrateManager.GetAttributeByKey(substrate.UniqueKey, PWA500SubstrateAttributes.IsLastSubstrate);
                            if (string.Equals(isLast, bool.TrueString, StringComparison.OrdinalIgnoreCase))
                            {
                                // 마지막 자재인 경우
                                QueuedScenarioForCoreSubstrate.Enqueue(ScenarioProcessEnd);
                            }
                        }
                    }
                    break;

                default:
                    //if (location is LoadPortLocation)
                    if (_workingInfo.LocationType == ModuleType.LoadPort)
                    {
                        // Initialize Flags
                        _newSubstrateId = string.Empty;
                        _newPartId = string.Empty;

                        // 스텝을 가져온다.
                        int unloadingStep = GetUnloadingStep(ref substrate);
                        switch (unloadingStep)
                        {
                            case (int)UnloadingStepTypesFor500W.Init:
                                {
                                    // 1. Id Assignment Event
                                    QueuedScenarioForBinSubstrate.Enqueue(ScenarioBinWaferIdAssign);
                                    // 2. Send to PM Assigned Id
                                    QueuedScenarioForBinSubstrate.Enqueue(ScenarioSendClientToBinWaferIdAssign);
                                    // 3. Bin Work End Event
                                    QueuedScenarioForBinSubstrate.Enqueue(ScenarioBinWorkEnd);
                                    // 4. Bin Track Out Event
                                    QueuedScenarioForBinSubstrate.Enqueue(ScnearioBinTrackOut);
                                    // 5. Request Part Id Info
                                    QueuedScenarioForBinSubstrate.Enqueue(ScenarioReqBinPartId);
                                    // 6. Request Bin data to PWA500BIN
                                    QueuedScenarioForBinSubstrate.Enqueue(ScenarioSendClientUploadBinFile);
                                    // 7. Upload Bin Data Event
                                    QueuedScenarioForBinSubstrate.Enqueue(ScenarioUploadBinData);
                                }
                                break;
                            case (int)UnloadingStepTypesFor500W.AfterIdAssignment:
                                {
                                    // AssignWaferId 이후
                                    // 2. Send to PM Assigned Id
                                    QueuedScenarioForBinSubstrate.Enqueue(ScenarioSendClientToBinWaferIdAssign);
                                    // 3. Bin Work End Event
                                    QueuedScenarioForBinSubstrate.Enqueue(ScenarioBinWorkEnd);
                                    // 4. Bin Track Out Event
                                    QueuedScenarioForBinSubstrate.Enqueue(ScnearioBinTrackOut);
                                    // 5. Request Part Id Info
                                    QueuedScenarioForBinSubstrate.Enqueue(ScenarioReqBinPartId);
                                    // 6. Request Bin data to PWA500BIN
                                    QueuedScenarioForBinSubstrate.Enqueue(ScenarioSendClientUploadBinFile);
                                    // 7. Upload Bin Data Event
                                    QueuedScenarioForBinSubstrate.Enqueue(ScenarioUploadBinData);
                                }
                                break;
                            case (int)UnloadingStepTypesFor500W.AfterBinTrackOut:
                                {
                                    // BinTrackout 이후
                                    // 5. Request Part Id Info
                                    QueuedScenarioForBinSubstrate.Enqueue(ScenarioReqBinPartId);
                                    // 6. Request Bin data to PWA500BIN
                                    QueuedScenarioForBinSubstrate.Enqueue(ScenarioSendClientUploadBinFile);
                                    // 7. Upload Bin Data Event
                                    QueuedScenarioForBinSubstrate.Enqueue(ScenarioUploadBinData);
                                }
                                break;

                            default:
                                break;
                        }
                    }
                    break;
            }
        }
        protected override CommandResults UpdateParamToBeforePlace()
        {
            if (false == _scenarioOperator.UseScenario)
            {
                _commandResult.CommandResult = CommandResult.Skipped;
                return _commandResult;
            }

            bool isManual = IsManual();
            if (false == GetWorkingInformation(isManual, ref _workingInfo, ref _temporaryDescription))
            {
                _commandResult.CommandResult = CommandResult.Error;
                _commandResult.Description = _temporaryDescription;
                return _commandResult;
            }

            if (false == LocationServer.FindLocationById(_workingInfo.LocationId, out var location))
            {
                _commandResult.CommandResult = CommandResult.Error;
                _commandResult.Description = $"Cannot find a location { _workingInfo.LocationId }";
                return _commandResult;
            }

            if (false == _substrateManager.GetSubstrateAtRobot(RobotName, _workingInfo.ActionArm, out var substrate))
            {
                _commandResult.CommandResult = CommandResult.Error;
                _commandResult.Description = "Cannot find info at robot";
                return _commandResult;
            }

            string subTypeString = substrate.GetAttribute(PWA500SubstrateAttributes.SubstrateType);
            if (false == Enum.TryParse(subTypeString, out SubstrateType substrateType))
            {
                _commandResult.CommandResult = CommandResult.Error;
                _commandResult.Description = "Cannot find substrate info at robot";
                return _commandResult;
            }

            if (location.LocationKind == ModuleType.LoadPort)
            {
                var lpLocation = location as LoadPortLocation;
                int portId = lpLocation.PortId;
                int slot = lpLocation.Slot;

                if (substrateType.Equals(SubstrateType.Core))
                {
                    if (QueuedScenarioForCoreSubstrate.Count <= 0)
                    {
                        _commandResult.CommandResult = CommandResult.Skipped;
                        _commandResult.Description = string.Empty;

                        return _commandResult;
                    }

                    var scenario = QueuedScenarioForCoreSubstrate.Dequeue();
                    switch (scenario)
                    {
                        case ScenarioProcessEnd:
                            {
                                var scenarioParam = ScenarioParameterBuilder.MakeParamToProcessing(portId, substrate);

                                InitResult(scenario);
                                _executingScenario = new QueuedScenarioInfo
                                {
                                    Scenario = scenario,
                                    ScenarioParams = scenarioParam
                                };

                                if (false == _scenarioOperator.UpdateScenarioParam(GetTaskName(), scenario, scenarioParam))
                                {
                                    _commandResult.CommandResult = CommandResult.Error;
                                    _commandResult.Description = "Failed to update for scenario param";
                                }
                                else
                                {
                                    _commandResult.CommandResult = CommandResult.Completed;
                                    _commandResult.Description = string.Empty;
                                }

                                return _commandResult;
                            }
                            break;

                        default:
                            _commandResult.CommandResult = CommandResult.Skipped;
                            _commandResult.Description = string.Empty;
                            return _commandResult;
                    }
                }
                else
                {
                    if (QueuedScenarioForBinSubstrate.Count <= 0)
                    {
                        _commandResult.CommandResult = CommandResult.Skipped;
                        return _commandResult;
                    }

                    // Id Assignment Event -> Send to PM Assigned Id -> Bin Work End Event -> Bin Track Out Event -> Request Part Id Info -> Uploading Bin File Event..
                    var scenario = QueuedScenarioForBinSubstrate.Dequeue();
                    InitResult(scenario);
                    switch (scenario)
                    {
                        case ScenarioBinWaferIdAssign:
                            {
                                #region <1. Id Assignment Event>
                                Dictionary<string, string> scenarioParam
                                    = ScenarioParameterBuilder.MakeScenarioParamToAssignSubstrateId(portId, slot, substrateType, substrate);
                                if (scenarioParam == null)
                                {
                                    _commandResult.CommandResult = CommandResult.Error;
                                    _commandResult.Description = "Invalid scenario param";
                                    return _commandResult;
                                }

                                _executingScenario = new QueuedScenarioInfo
                                {
                                    Scenario = ScenarioBinWaferIdAssign,
                                    ScenarioParams = scenarioParam
                                };


                                if (false == _scenarioOperator.UpdateScenarioParam(GetTaskName(), ScenarioBinWaferIdAssign, scenarioParam))
                                {
                                    _commandResult.CommandResult = CommandResult.Error;
                                    _commandResult.Description = "Failed to update for scenario param";
                                }
                                else
                                {
                                    _commandResult.CommandResult = CommandResult.Completed;
                                    _commandResult.Description = string.Empty;
                                }
                                return _commandResult;
                                #endregion </1. Id Assignment Event>        
                            }

                        case ScenarioSendClientToBinWaferIdAssign:
                            {
                                int currentStep = GetUnloadingStep(ref substrate);
                                if (currentStep == (int)UnloadingStepTypesFor500W.Init)
                                {
                                    // Step 증가
                                    UnloadingStepTypesFor500W nextStep = UnloadingStepTypesFor500W.AfterIdAssignment;
                                    _substrateManager.SetAttributeByKey(substrate.UniqueKey, PWA500SubstrateAttributes.BinUnloadingStep, nextStep.ToString());
                                }
                                else
                                {
                                    // 재시도일 것이다.
                                    _newSubstrateId = substrate.Name;
                                }

                                #region <2. Send to PM Assigned Id : 공정설비에 할당받은 결과를 전달한다.>
                                string ringId = substrate.GetAttribute(PWA500SubstrateAttributes.RingId);

                                _lotHistoryLog.WriteSubstrateHistoryForAssignSubstrateId(portId, ringId, _newSubstrateId, substrate.UniqueKey);

                                // 서버에서 받은 이름을 이 웨이퍼의 이름으로 설정한다.
                                // substrate.SetName(_newSubstrateId);
                                var key = substrate.UniqueKey;
                                _substrateManager.SetNameByKey(key, _newSubstrateId);
                                _substrateManager.SaveDataByKey(key);
                                //substrate.SetAttribute(PWA500WSubstrateAttributes.RingId, ringId);

                                var scenarioParam = ScenarioParameterBuilder.MakeScenarioParamToSendingAssignId(_newSubstrateId, ringId);
                                if (scenarioParam == null)
                                {
                                    _commandResult.CommandResult = CommandResult.Error;
                                    _commandResult.Description = "Invalid scenario param";
                                    return _commandResult;
                                }

                                _executingScenario = new QueuedScenarioInfo
                                {
                                    Scenario = ScenarioSendClientToBinWaferIdAssign,
                                    ScenarioParams = scenarioParam
                                };

                                if (false == _scenarioOperator.UpdateScenarioParam(GetTaskName(), _executingScenario.Scenario, scenarioParam))
                                {
                                    _commandResult.CommandResult = CommandResult.Error;
                                    _commandResult.Description = "Failed to update for scenario param";
                                }
                                else
                                {
                                    _commandResult.CommandResult = CommandResult.Completed;
                                    _commandResult.Description = string.Empty;
                                }
                                return _commandResult;
                                #endregion </2. Send to PM Assigned Id : 공정설비에 할당받은 결과를 전달한다.>                                            
                            }

                        case ScenarioBinWorkEnd:
                            {
                                #region <3. Bin Work End Event : Bin Work End Event를 발생시킨다.>
                                //string substrateId = substrate.Name;
                                //string ringId = substrate.GetAttribute(PWA500SubstrateAttributes.RingId);

                                var scenarioParam = ScenarioParameterBuilder.MakeScenarioParamToBinWorkEnd(substrate.UniqueKey, true);
                                if (scenarioParam == null)
                                {
                                    _commandResult.CommandResult = CommandResult.Error;
                                    _commandResult.Description = "Invalid scenario param";
                                    return _commandResult;
                                }

                                _executingScenario = new QueuedScenarioInfo
                                {
                                    Scenario = ScenarioBinWorkEnd,
                                    ScenarioParams = scenarioParam
                                };

                                if (false == _scenarioOperator.UpdateScenarioParam(GetTaskName(), _executingScenario.Scenario, scenarioParam))
                                {
                                    _commandResult.CommandResult = CommandResult.Error;
                                    _commandResult.Description = "Failed to update for scenario param";
                                }
                                else
                                {
                                    _commandResult.CommandResult = CommandResult.Completed;
                                    _commandResult.Description = string.Empty;
                                }
                                return _commandResult;
                                #endregion </3. Bin Work End Event : Bin Work End Event를 발생시킨다.>
                            }

                        case ScnearioBinTrackOut:
                            {
                                #region <4. Bin Track Out Event : Bin Track Out Event를 발생시킨다.>
                                //string substrateId = substrate.Name;
                                var scenarioParam = ScenarioParameterBuilder.MakeScenarioParamToTrackOut(substrate.UniqueKey, "AUTO", false);
                                if (scenarioParam == null)
                                {
                                    _commandResult.CommandResult = CommandResult.Error;
                                    _commandResult.Description = "Invalid scenario param";
                                    return _commandResult;
                                }

                                _executingScenario = new QueuedScenarioInfo
                                {
                                    Scenario = ScnearioBinTrackOut,
                                    ScenarioParams = scenarioParam
                                };

                                if (false == _scenarioOperator.UpdateScenarioParam(GetTaskName(), _executingScenario.Scenario, scenarioParam))
                                {
                                    _commandResult.CommandResult = CommandResult.Error;
                                    _commandResult.Description = "Failed to update for scenario param";
                                }
                                else
                                {
                                    _commandResult.CommandResult = CommandResult.Completed;
                                    _commandResult.Description = string.Empty;
                                }
                                return _commandResult;
                                #endregion </4. Bin Track Out Event : Bin Track Out Event를 발생시킨다.>
                            }

                        case ScenarioReqBinPartId:
                            {
                                #region <5. Request Part Id Info : Track Out 이후 변경된 PartId 를 받기위한 이벤트를 발생시킨다.>
                                string lotId = substrate.LotId;
                                if (false == _carrierServer.HasCarrier(portId))
                                {
                                    _commandResult.CommandResult = CommandResult.Error;
                                    _commandResult.Description = "Does not have carrier";
                                    return _commandResult;
                                }

                                string carrierId = _carrierServer.GetCarrierId(portId);

                                var scenarioParam = ScenarioParameterBuilder.MakeScenarioParamToRequestBinPartId(lotId, carrierId);
                                if (scenarioParam == null)
                                {
                                    _commandResult.CommandResult = CommandResult.Error;
                                    _commandResult.Description = "Invalid scenario param";
                                    return _commandResult;
                                }

                                _executingScenario = new QueuedScenarioInfo
                                {
                                    Scenario = ScenarioReqBinPartId,
                                    ScenarioParams = scenarioParam
                                };

                                if (false == _scenarioOperator.UpdateScenarioParam(GetTaskName(), _executingScenario.Scenario, scenarioParam))
                                {
                                    _commandResult.CommandResult = CommandResult.Error;
                                    _commandResult.Description = "Failed to update for scenario param";
                                }
                                else
                                {
                                    _commandResult.CommandResult = CommandResult.Completed;
                                    _commandResult.Description = string.Empty;
                                }
                                return _commandResult;
                                #endregion </5. Request Part Id Info : Track Out 이후 변경된 PartId 를 받기위한 이벤트를 발생시킨다.>
                            }

                        case ScenarioSendClientUploadBinFile:
                            {
                                int currentStep = GetUnloadingStep(ref substrate);
                                if (currentStep == (int)UnloadingStepTypesFor500W.AfterIdAssignment)
                                {
                                    // Step 증가
                                    UnloadingStepTypesFor500W nextStep = UnloadingStepTypesFor500W.AfterBinTrackOut;
                                    _substrateManager.SetAttributeByKey(substrate.UniqueKey, PWA500SubstrateAttributes.BinUnloadingStep, nextStep.ToString());
                                    _substrateManager.SetAttributeByKey(substrate.UniqueKey, PWA500SubstrateAttributes.PartId, _newPartId);
                                    _substrateManager.SaveDataByKey(substrate.UniqueKey);
                                    //_substrateManager.SetAttributesByKey(substrate.UniqueKey, new Dictionary<string, string>
                                    //{
                                    //    [PWA500SubstrateAttributes.BinUnloadingStep] = nextStep.ToString(),
                                    //    [PWA500SubstrateAttributes.PartId] = _newPartId,
                                    //});
                                }
                                #region <6. Uploading Bin File Event>

                                // 매 장 BinFile Upload 발생이 필요하다.
                                Dictionary<string, string> scenarioParam = ScenarioParameterBuilder.MakeScenarioParamToUploadBinFile(portId, slot, MachineName, substrate);
                                if (scenarioParam == null)
                                {
                                    _commandResult.CommandResult = CommandResult.Error;
                                    _commandResult.Description = "Invalid scenario param";
                                    return _commandResult;
                                }

                                _executingScenario = new QueuedScenarioInfo
                                {
                                    Scenario = ScenarioSendClientUploadBinFile,
                                    ScenarioParams = scenarioParam
                                };

                                // 공정설비에 맵 데이터 요청 메시지 전달
                                // TODO : 시나리오 결과를 받아 섭에 적용하기 구현 필요
                                if (false == _scenarioOperator.UpdateScenarioParam(GetTaskName(), _executingScenario.Scenario, scenarioParam))
                                {
                                    _commandResult.CommandResult = CommandResult.Error;
                                    _commandResult.Description = "Failed to update for scenario param";
                                }
                                else
                                {
                                    _commandResult.CommandResult = CommandResult.Completed;
                                    _commandResult.Description = string.Empty;
                                }
                                return _commandResult;
                                #endregion </6. Uploading Bin File Event>
                            }

                        case ScenarioUploadBinData:
                            {
                                if (_functionsForPWA500.GetBinDataToUpload(ref _binDataToUpload) || IsSimulation())
                                {
                                    if (IsSimulation())
                                    {
                                        string name = substrate.Name;
                                        string ringId = substrate.GetAttribute(PWA500SubstrateAttributes.RingId);
                                        string qty = substrate.GetAttribute(PWA500SubstrateAttributes.ChipQty);
                                        int.TryParse(qty, out int chipQty);
                                        string angle = substrate.GetAttribute(PWA500SubstrateAttributes.Angle);
                                        double.TryParse(angle, out double waferAngle);
                                        string row = substrate.GetAttribute(PWA500SubstrateAttributes.CountY);
                                        int.TryParse(row, out int countRow);
                                        string col = substrate.GetAttribute(PWA500SubstrateAttributes.CountX);
                                        int.TryParse(col, out int countCol);
                                        string nullBinCode = " ";
                                        string mapData = "12345";
                                        string pmsFileBody = "TEST_PMS";
                                        _binDataToUpload = new BinDataToUploadFromPWA500("MAIN",
                                            name,
                                            ringId,
                                            chipQty,
                                            waferAngle,
                                            countRow,
                                            countCol,
                                            nullBinCode,
                                            mapData,
                                            pmsFileBody,
                                            "AUTO",
                                            true);
                                    }

                                    Dictionary<string, string> scenarioParam = _functionsForPWA500.MakeScenarioParamToUploadBinData(
                                        _binDataToUpload.NameOfEq,
                                        _binDataToUpload.SubstrateId,
                                        _binDataToUpload.RingId,
                                        _binDataToUpload.ChipQty,
                                        /*_binDataToUpload.Angle*/270,
                                        _binDataToUpload.CountRow,
                                        _binDataToUpload.CountCol,
                                        _binDataToUpload.NullBinCode,
                                        _binDataToUpload.MapData,
                                        _binDataToUpload.PmsFileBody,
                                        _binDataToUpload.UserId,
                                        _binDataToUpload.UseEventHandling);

                                    if (scenarioParam == null)
                                    {
                                        _commandResult.CommandResult = CommandResult.Error;
                                        _commandResult.Description = "Invalid scenario param";
                                        return _commandResult;
                                    }

                                    _executingScenario = new QueuedScenarioInfo
                                    {
                                        Scenario = ScenarioUploadBinData,
                                        ScenarioParams = scenarioParam
                                    };

                                    // 공정설비에 맵 데이터 요청 메시지 전달 -> 콜백에서 UploadBinData Event 발생
                                    if (false == _scenarioOperator.UpdateScenarioParam(GetTaskName(), _executingScenario.Scenario, scenarioParam))
                                    {
                                        _commandResult.CommandResult = CommandResult.Error;
                                        _commandResult.Description = "Failed to update for scenario param";
                                    }
                                    else
                                    {
                                        _commandResult.CommandResult = CommandResult.Completed;
                                        _commandResult.Description = string.Empty;
                                    }
                                }
                                else
                                {
                                    _commandResult.CommandResult = CommandResult.Error;
                                    _commandResult.Description = "Does not have bin data";

                                }

                                return _commandResult;

                            }

                        default:
                            {
                                _commandResult.CommandResult = CommandResult.Skipped;
                                _commandResult.Description = string.Empty;
                                return _commandResult;
                            }
                    }
                }
            }
            else if (location.LocationKind == ModuleType.ProcessModule)
            {
                if (substrateType == SubstrateType.Core)
                {
                    if (QueuedScenarioForCoreSubstrate.Count <= 0)
                    {
                        _commandResult.CommandResult = CommandResult.Skipped;
                        _commandResult.Description = string.Empty;

                        return _commandResult;
                    }

                    var scenario = QueuedScenarioForCoreSubstrate.Dequeue();
                    switch (scenario)
                    {
                        case ScenarioProcessStart:
                            {
                                int portId = substrate.SourcePortId;
                                var scenarioParam = ScenarioParameterBuilder.MakeParamToProcessing(portId, substrate);

                                InitResult(scenario);
                                _executingScenario = new QueuedScenarioInfo
                                {
                                    Scenario = scenario,
                                    ScenarioParams = scenarioParam
                                };

                                if (false == _scenarioOperator.UpdateScenarioParam(GetTaskName(), scenario, scenarioParam))
                                {
                                    _commandResult.CommandResult = CommandResult.Error;
                                    _commandResult.Description = "Failed to update for scenario param";
                                }
                                else
                                {
                                    _commandResult.CommandResult = CommandResult.Completed;
                                    _commandResult.Description = string.Empty;
                                }

                                return _commandResult;
                            }
                            break;

                        default:
                            _commandResult.CommandResult = CommandResult.Skipped;
                            _commandResult.Description = string.Empty;
                            return _commandResult;
                    }
                }
                else
                {
                    _commandResult.CommandResult = CommandResult.Skipped;
                    return _commandResult;
                }
            }
            else
            {
                _commandResult.CommandResult = CommandResult.Skipped;
                return _commandResult;
            }
        }
        protected override CommandResults ExecuteScenarioToBeforePlace()
        {
            return RunScenario(_executingScenario.Scenario);
        }
        protected override CommandResults UpdateParamToAfterPlace()
        {
            bool isManual = IsManual();
            if (false == GetWorkingInformation(isManual, ref _workingInfo, ref _temporaryDescription))
            {
                _commandResult.CommandResult = CommandResult.Error;
                _commandResult.Description = _temporaryDescription;
                return _commandResult;
            }

            switch (_workingInfo.LocationType)
            {
                case ModuleType.ProcessModule:
                    {
                        if (false == _scenarioOperator.UseScenario)
                        {
                            _commandResult.CommandResult = CommandResult.Skipped;
                            _commandResult.Description = string.Empty;
                            return _commandResult;
                        }

                        if (QueuedScenarioForCoreSubstrate.Count <= 0)
                        {
                            _commandResult.CommandResult = CommandResult.Skipped;
                            _commandResult.Description = string.Empty;
                            return _commandResult;
                        }

                        var scenario = QueuedScenarioForCoreSubstrate.Dequeue();
                        var key = _workingInfo.SubstrateKey;
                        if (false == _substrateManager.GetSubstrateByKey(key, out var substrate) || substrate == null)
                        {
                            _commandResult.CommandResult = CommandResult.Error;
                            _commandResult.Description = "Cannot find substrate info at process module";
                            return _commandResult;
                        }

                        string subTypeString = substrate.GetAttribute(PWA500SubstrateAttributes.SubstrateType);
                        if (false == Enum.TryParse(subTypeString, out SubstrateType substrateType))
                        {
                            _commandResult.CommandResult = CommandResult.Error;
                            _commandResult.Description = "Cannot find substrate info at process module";
                            return _commandResult;
                        }

                        if (false == substrateType.Equals(SubstrateType.Core))
                        {
                            _commandResult.CommandResult = CommandResult.Skipped;
                            _commandResult.Description = string.Empty;
                            return _commandResult;
                        }

                        int portId = substrate.SourcePortId;
                        string sourceCarrierId = _carrierServer.GetCarrierId(portId);
                        if (IsFirstSubstrateAtLoadPort(sourceCarrierId, portId, substrate.UniqueKey))
                        {
                            #region <Process Start>
                            var scenarioParam = ScenarioParameterBuilder.MakeParamToProcessing(portId, substrate);

                            InitResult(scenario);
                            _executingScenario = new QueuedScenarioInfo
                            {
                                Scenario = scenario,
                                ScenarioParams = scenarioParam
                            };

                            if (false == _scenarioOperator.UpdateScenarioParam(GetTaskName(), scenario, scenarioParam))
                            {
                                _commandResult.CommandResult = CommandResult.Error;
                                _commandResult.Description = "Failed to update for scenario param";
                            }
                            else
                            {
                                _commandResult.CommandResult = CommandResult.Completed;
                                _commandResult.Description = string.Empty;
                            }
                            return _commandResult;
                            #endregion </Process Start>
                        }
                    }
                    break;
                case ModuleType.LoadPort:
                    {
                        if (LocationServer.FindLocationById(_workingInfo.LocationId, out var location))
                        {
                            var lpLocation = location as LoadPortLocation;
                            var carrierId = _carrierServer.GetCarrierId(lpLocation.PortId);

                            if (_substrateManager.GetSubstrateByLocationAndKey(lpLocation, string.Empty, out var substrate))
                            {
                                // 코어 기판 이력은 기록 시점에 캐리어 이력에 즉시 이중 기록되므로 병합 대상이 아니다.
                                // (병합 처리부가 Bin 폴더만 읽어 사실상 무시되지만, 이름 충돌 시 오기록을 막기 위해 타입으로도 차단.
                                //  타입 파싱 실패 시에는 Bin 이력 유실을 피하기 위해 기존처럼 병합을 시도한다.)
                                string subType = substrate.GetAttribute(PWA500SubstrateAttributes.SubstrateType);
                                SubstrateType substrateType = SubstrateType.Core;
                                bool isCore = GetSubstrateTypeByAttribute(subType, ref substrateType) &&
                                              substrateType.Equals(SubstrateType.Core);
                                if (false == isCore)
                                {
                                    // 2026.07.06. jhlim [MOD] 이력 파일은 Name(링ID→할당ID)으로 생성/개명되므로 Name으로 조회해야 한다.
                                    // UniqueKey({캐리어}_{위치}_{Ticks})로는 파일이 존재한 적이 없어 병합이 항상 스킵되고 있었다.
                                    // (Name은 파일 저장소용, UniqueKey는 DB 저장소의 키 컬럼용으로 함께 전달한다)
                                    _lotHistoryLog.UpdateSubstrateHistoryToCarrierHistory(lpLocation.PortId, carrierId, substrate.Name, substrate.UniqueKey);
                                }
                            }
                        }
                    }
                    break;

                default:
                    break;
            }

            _commandResult.CommandResult = CommandResult.Skipped;
            _commandResult.Description = string.Empty;
            return _commandResult;
        }
        protected override CommandResults ExecuteScenarioToAfterPlace()
        {
            if (false == _scenarioOperator.UseScenario)
            {
                _commandResult.ActionName = "Idle";
                _commandResult.Description = string.Empty;
                _commandResult.CommandResult = CommandResult.Skipped;
            }
            else
            {
                if (_executingScenario != null)
                {
                    return RunScenario(_executingScenario.Scenario);
                }
                else
                {
                    _commandResult.ActionName = "Idle";
                    _commandResult.Description = string.Empty;
                    _commandResult.CommandResult = CommandResult.Skipped;
                }
            }

            return _commandResult;
        }
        #endregion </Scenario>

        #region <Material Handling With Process Module>

        private bool IsTickOver()
        {
            //return false;

            return Ticks.IsTickOver(false);
        }

        #region <Loading>
        protected override void InitMaterialHandlingInterface()
        {            
            _subStepInterface = 0;
        }

        protected override CommandResults IsApproachLoadingPrepared()
        {
            const string MethodName = "IsApproachLoadingPrepared";

            if (false == IsLoadingSignalStillActive(_workingInfo.LocationId))
                return ReturnSkipped(MethodName);

            switch (_subStepInterface)
            {
                case 0:
                    {
                        //  1. Foup이 준비 되었는지 확인 후 준비 되지 않았으면 Skipped 리턴
                        //     준비되면 스메마 켜고 진행
                        bool prepared;
                        if (_workingInfo.LocationType == ModuleType.LoadPort)
                        {
                            if (LocationServer.FindLocationById(_workingInfo.LocationId, out var location))
                            {
                                var lpLocation = location as LoadPortLocation;
                                prepared = _carrierServer.HasCarrier(lpLocation.PortId);
                                if (prepared)
                                {
                                    // 스메마 켠다.
                                    _processGroup.SetLoadingSignal(ProcessModuleIndex, _workingInfo.LocationId, true);
                                    ++_subStepInterface;
                                }
                                else
                                {
                                    return ReturnSkipped(MethodName);
                                }
                            }
                        }
                        if (_workingInfo.LocationType == ModuleType.ProcessModule)
                        {
                            // 스메마 켠다.
                            _processGroup.SetLoadingSignal(ProcessModuleIndex, _workingInfo.LocationId, true);
                            ++_subStepInterface;
                        }
                        else
                        {
                            return ReturnToError(CommandResult.Error, EN_ALARM.INTERFACE_BEFORE_LOADING_DATA_INVALID, MethodName, _subStepInterface, "Location is invalid");
                        }
                    }
                    break;
                case 1:
                    {
                        if (false == GetSubstrateNameByKey(_workingInfo.SubstrateKey, out var name) || string.IsNullOrWhiteSpace(name))
                        {
                            return ReturnToError(CommandResult.Error, EN_ALARM.INTERFACE_BEFORE_LOADING_DATA_INVALID, MethodName, _subStepInterface, RequestMessages.RequestApproachLoading.ToString());
                        }

                        // 2. AppreachLoading을 전송
                        if (false == _processGroup.SendMessage(ProcessModuleIndex, _workingInfo.LocationId,
                            RequestMessages.RequestApproachLoading.ToString(), name))
                        {
                            return ReturnToError(CommandResult.Error, EN_ALARM.INTERFACE_BEFORE_LOADING_SENDING_FAILED, MethodName,
                                _subStepInterface, RequestMessages.RequestApproachLoading.ToString());
                        }

                        Ticks.SetTickCount(TimeoutShort);
                        ++_subStepInterface;
                    }
                    break;
                case 2:
                    {
                        if (IsTickOver())
                        {
                            return ReturnToError(CommandResult.Error, EN_ALARM.INTERFACE_BEFORE_LOADING_SENDING_COMPLETED_TIMEOUT_ACK, MethodName,
                                _subStepInterface, RequestMessages.RequestApproachLoading.ToString());
                        }

                        //  3. Ack 확인
                        var result = _processGroup.IsSendingCompleted(ProcessModuleIndex, _workingInfo.LocationId,
                            RequestMessages.RequestApproachLoading.ToString());
                        switch (result)
                        {
                            case CommunicationResult.Ack:
                                Ticks.SetTickCount(TimeoutLong);
                                ++_subStepInterface;
                                break;
                            case CommunicationResult.Nack:
                            case CommunicationResult.Error:
                                return ReturnToError(CommandResult.Error, EN_ALARM.INTERFACE_BEFORE_LOADING_SENDING_COMPLETED_BUT_NACK, MethodName,
                                    _subStepInterface, RequestMessages.RequestApproachLoading.ToString());

                            default:
                                break;
                        }

                    }
                    break;
                case 3:
                    {
                        if (IsTickOver())
                        {
                            return ReturnToError(CommandResult.Error, EN_ALARM.INTERFACE_BEFORE_LOADING_RECEIVING_RESPONSE_MESSAGE_TIMEOUT, MethodName,
                                _subStepInterface, ResponseMessages.ResponseApproachLoading.ToString());
                        }

                        // 4. Response 확인
                        var result = _processGroup.IsMessageReceived(ProcessModuleIndex, _workingInfo.LocationId,
                            ResponseMessages.ResponseApproachLoading.ToString());
                        switch (result)
                        {
                            case CommunicationResult.Ack:
                                Ticks.SetTickCount(TimeoutShort);
                                ++_subStepInterface;
                                break;

                            case CommunicationResult.Nack:
                            case CommunicationResult.Error:
                                {
                                    return ReturnToError(CommandResult.Error, EN_ALARM.INTERFACE_BEFORE_LOADING_RECEIVING_COMPLETED_BUT_ERROR, MethodName,
                                        _subStepInterface, ResponseMessages.ResponseApproachLoading.ToString());
                                }

                            default:
                                break;
                        }
                    }
                    break;

                case 4:
                    {
                        if (IsTickOver())
                        {
                            return ReturnToError(CommandResult.Timeout, EN_ALARM.INTERFACE_BEFORE_LOADING_RECEIVING_RESPONSE_DATA_TIMEOUT, MethodName,
                                _subStepInterface, ResponseMessages.ResponseApproachLoading.ToString());
                        }

                        // 5. 데이터 확인
                        if (false == _processGroup.GetReceivedData(ProcessModuleIndex, _workingInfo.LocationId,
                            ResponseMessages.ResponseApproachLoading.ToString(), out _))
                            break;

                        // 6. Ack 전송 : 콜백에서 자동 Ack 나가니 현재 미구현
                        //if (false == _processGroup.SetAckReceivedMessage(ProcessModuleIndex, _workingInfo.Location,
                        //    ResponseMessages.ResponseApproachLoading.ToString(), CommunicationResult.Ack, string.Empty))
                        //{

                        //}

                        return ReturnCompleted();
                    }

                default:
                    break;
            }

            return ReturnProceed();
        }
        protected override CommandResults IsApproachLoadingCompleted()
        {
            const string MethodName = "IsApproachLoadingCompleted";

            // 상황이 바뀌었을 수 있다..
            if (false == IsLoadingSignalStillActive(_workingInfo.LocationId))
                return ReturnSkipped(MethodName);

            return ReturnCompleted();
        }
        protected override CommandResults IsLoadingPrepared()
        {
            const string MethodName = "IsLoadingPrepared";

            // 상황이 바뀌었을 수 있다..
            if (false == IsLoadingSignalStillActive(_workingInfo.LocationId))
                return ReturnSkipped(MethodName);

            switch (_subStepInterface)
            {
                case 0:
                    {
                        if (false == GetSubstrateNameByKey(_workingInfo.SubstrateKey, out var name) || string.IsNullOrWhiteSpace(name))
                        {
                            return ReturnToError(CommandResult.Error, EN_ALARM.INTERFACE_ACTION_LOADING_DATA_INVALID, MethodName, _subStepInterface, RequestMessages.RequestApproachLoading.ToString());
                        }

                        // 1. ActionLoading을 전송
                        if (false == _processGroup.SendMessage(ProcessModuleIndex, _workingInfo.LocationId,
                            RequestMessages.RequestActionLoading.ToString(), name))
                        {
                            return ReturnToError(CommandResult.Error, EN_ALARM.INTERFACE_ACTION_LOADING_SENDING_FAILED, MethodName,
                                _subStepInterface, RequestMessages.RequestActionLoading.ToString());
                        }

                        Ticks.SetTickCount(TimeoutShort);
                        ++_subStepInterface;
                    }
                    break;
                case 1:
                    {
                        if (IsTickOver())
                        {
                            return ReturnToError(CommandResult.Timeout, EN_ALARM.INTERFACE_ACTION_LOADING_SENDING_COMPLETED_TIMEOUT_ACK, MethodName,
                                _subStepInterface, RequestMessages.RequestActionLoading.ToString());
                        }

                        //  2. Ack 확인
                        var result = _processGroup.IsSendingCompleted(ProcessModuleIndex, _workingInfo.LocationId,
                            RequestMessages.RequestActionLoading.ToString());
                        switch (result)
                        {
                            case CommunicationResult.Ack:
                                Ticks.SetTickCount(TimeoutLong);
                                ++_subStepInterface;
                                break;
                            case CommunicationResult.Nack:
                            case CommunicationResult.Error:
                                {
                                    return ReturnToError(CommandResult.Error, EN_ALARM.INTERFACE_ACTION_LOADING_SENDING_COMPLETED_BUT_NACK, MethodName,
                                        _subStepInterface, RequestMessages.RequestActionLoading.ToString());
                                }
                            default:
                                break;
                        }

                    }
                    break;
                case 2:
                    {
                        if (IsTickOver())
                        {
                            return ReturnToError(CommandResult.Timeout, EN_ALARM.INTERFACE_ACTION_LOADING_RECEIVING_RESPONSE_MESSAGE_TIMEOUT, MethodName,
                                _subStepInterface, ResponseMessages.ResponseActionLoading.ToString());
                        }

                        // 3. Response 확인
                        var result = _processGroup.IsMessageReceived(ProcessModuleIndex, _workingInfo.LocationId,
                            ResponseMessages.ResponseActionLoading.ToString());
                        switch (result)
                        {
                            case CommunicationResult.Ack:
                                Ticks.SetTickCount(TimeoutShort);
                                ++_subStepInterface;
                                break;

                            case CommunicationResult.Nack:
                            case CommunicationResult.Error:
                                {
                                    return ReturnToError(CommandResult.Error, EN_ALARM.INTERFACE_ACTION_LOADING_RECEIVING_COMPLETED_BUT_ERROR, MethodName,
                                        _subStepInterface, ResponseMessages.ResponseActionLoading.ToString());
                                }

                            default:
                                break;
                        }
                    }
                    break;

                case 3:
                    {
                        if (IsTickOver())
                        {
                            return ReturnToError(CommandResult.Timeout, EN_ALARM.INTERFACE_ACTION_LOADING_RECEIVING_RESPONSE_DATA_TIMEOUT, MethodName,
                                _subStepInterface, ResponseMessages.ResponseActionLoading.ToString());                            
                        }

                        // 4. 데이터 확인
                        if (false == _processGroup.GetReceivedData(ProcessModuleIndex, _workingInfo.LocationId,
                            ResponseMessages.ResponseActionLoading.ToString(), out _))
                            break;

                        // 6. Ack 전송 : 콜백에서 자동 Ack 나가니 현재 미구현
                        //if (false == _processGroup.SetAckReceivedMessage(ProcessModuleIndex, _workingInfo.Location,
                        //    ResponseMessages.ResponseActionLoading.ToString(), CommunicationResult.Ack, string.Empty))
                        //{

                        //}

                        return ReturnCompleted();
                    }

                default:
                    break;
            }

            return ReturnProceed();
        }
        protected override CommandResults IsLoadingCompleted()
        {
            const string MethodName = "IsLoadingCompleted";

            // 상황이 바뀌었을 수 있다..
            if (false == IsLoadingSignalStillActive(_workingInfo.LocationId))
            {
                return ReturnSkipped(MethodName);
            }

            switch (_subStepInterface)
            {
                case 0:
                    {
                        if (false == GetSubstrateNameByKey(_workingInfo.SubstrateKey, out var name) || string.IsNullOrWhiteSpace(name))
                        {
                            return ReturnToError(CommandResult.Error, EN_ALARM.INTERFACE_AFTER_LOADING_DATA_INVALID, MethodName, _subStepInterface, RequestMessages.RequestApproachLoading.ToString());
                        }

                        // 1. ConfirmLoading 전송
                        if (false == _processGroup.SendMessage(ProcessModuleIndex, _workingInfo.LocationId,
                            RequestMessages.RequestConfirmLoading.ToString(), name))
                        {
                            return ReturnToError(CommandResult.Error, EN_ALARM.INTERFACE_AFTER_LOADING_SENDING_FAILED, MethodName,
                                _subStepInterface, RequestMessages.RequestConfirmLoading.ToString());
                        }

                            Ticks.SetTickCount(TimeoutShort);
                        ++_subStepInterface;
                    }
                    break;
                case 1:
                    {
                        if (IsTickOver())
                        {
                            return ReturnToError(CommandResult.Timeout, EN_ALARM.INTERFACE_AFTER_LOADING_SENDING_COMPLETED_TIMEOUT_ACK, MethodName,
                                _subStepInterface, RequestMessages.RequestConfirmLoading.ToString());
                        }

                        //  2. Ack 확인
                        var result = _processGroup.IsSendingCompleted(ProcessModuleIndex, _workingInfo.LocationId,
                            RequestMessages.RequestConfirmLoading.ToString());
                        switch (result)
                        {
                            case CommunicationResult.Ack:
                                Ticks.SetTickCount(TimeoutLong);
                                ++_subStepInterface;
                                break;
                            case CommunicationResult.Nack:
                            case CommunicationResult.Error:
                                {
                                    return ReturnToError(CommandResult.Error, EN_ALARM.INTERFACE_AFTER_LOADING_SENDING_COMPLETED_BUT_NACK, MethodName,
                                        _subStepInterface, RequestMessages.RequestConfirmLoading.ToString());
                                }
                            default:
                                break;
                        }

                    }
                    break;
                case 2:
                    {
                        if (IsTickOver())
                        {
                            return ReturnToError(CommandResult.Timeout, EN_ALARM.INTERFACE_AFTER_LOADING_RECEIVING_RESPONSE_MESSAGE_TIMEOUT, MethodName,
                                _subStepInterface, ResponseMessages.ResponseConfirmLoading.ToString());
                        }

                        // 3. Response 확인
                        var result = _processGroup.IsMessageReceived(ProcessModuleIndex, _workingInfo.LocationId,
                            ResponseMessages.ResponseConfirmLoading.ToString());
                        switch (result)
                        {
                            case CommunicationResult.Ack:
                                Ticks.SetTickCount(TimeoutShort);
                                ++_subStepInterface;
                                break;

                            case CommunicationResult.Nack:
                            case CommunicationResult.Error:
                                return ReturnToError(CommandResult.Error, EN_ALARM.INTERFACE_AFTER_LOADING_RECEIVING_COMPLETED_BUT_ERROR, MethodName,
                                    _subStepInterface, ResponseMessages.ResponseConfirmLoading.ToString());

                            default:
                                break;
                        }
                    }
                    break;

                case 3:
                    {
                        if (IsTickOver())
                        {
                            return ReturnToError(CommandResult.Timeout, EN_ALARM.INTERFACE_AFTER_LOADING_RECEIVING_RESPONSE_DATA_TIMEOUT, MethodName
                                , _subStepInterface, ResponseMessages.ResponseConfirmLoading.ToString());
                        }

                        // 4. 데이터 확인
                        if (false == _processGroup.GetReceivedData(ProcessModuleIndex, _workingInfo.LocationId,
                            ResponseMessages.ResponseConfirmLoading.ToString(), out _))
                            break;

                        // 5. Ack 전송 : 콜백에서 자동 Ack 나가니 현재 미구현
                        //if (false == _processGroup.SetAckReceivedMessage(ProcessModuleIndex, _workingInfo.Location,
                        //    ResponseMessages.ResponseConfirmLoading.ToString(), CommunicationResult.Ack, string.Empty))
                        //    return ReturnError(CommandResult.Error, MethodName,
                        //        _subStepInterface, string.Format("Response has failed : {0}", ResponseMessages.ResponseConfirmLoading.ToString()));

                        Ticks.SetTickCount(100);
                        ++_subStepInterface;
                    }
                    break;

                case 4:
                    {
                        if (false == IsTickOver())
                            break;

                        // 6. SMEMA OFF
                        _processGroup.SetLoadingSignal(ProcessModuleIndex, _workingInfo.LocationId, false);

                        return ReturnCompleted();
                    }

                default:
                    break;
            }

            return ReturnProceed();
        }
        #endregion </Loading>

        #region <Unloading>
        private bool IsUnloadingSignalStillActive(string location)
        {
            // TODO : 시뮬용 리턴
            //if (Work.AppConfigManager.Instance.ControllerDigital == Define.DefineEnumProject.AppConfig.EN_DIGITAL_IO_CONTROLLER.NONE)
            //{
            //    return true;
            //}

            //  1) PM의 스메마 확인 후 Off면 Skipped 리턴
            List<string> requestedLocation = new List<string>();
            if (false == _processGroup.IsUnloadingRequested(ProcessModuleIndex, ref requestedLocation))
                return false;

            if (false == requestedLocation.Contains(location))
                return false;

            return true;
        }

        //private bool FindPortInfo(int sourcePortId, SubstrateType substrateType, ref int targetPortId)
        //{
        //    for (int i = 0; i < _loadPortManager.Count; ++i)
        //    {
        //        // 2024.09.03. jhlim [MOD] SubType을 UI에는 Center/Left/Right로 지정되도록 변경
        //        //FrameOfSystem3.Recipe.PARAM_EQUIPMENT paramName;
        //        //paramName = FrameOfSystem3.Recipe.PARAM_EQUIPMENT.LoadPortType1 + i;
        //        //string subTypeByRecipe = FrameOfSystem3.Recipe.Recipe.GetInstance().GetValue(FrameOfSystem3.Recipe.EN_RECIPE_TYPE.EQUIPMENT,
        //        //    paramName.ToString(),
        //        //    SubstrateType.Empty.ToString());

        //        //if (false == Enum.TryParse(subTypeByRecipe, out SubstrateType convertedSubType))
        //        //    continue;
        //        SubstrateType convertedSubType = _functionsForPWA500.GetSubstrateTypeByLoadPortIndex(i);
        //        // 2024.09.03. jhlim [END]

        //        if (false == substrateType.Equals(convertedSubType))
        //            continue;
        //        if (substrateType == SubstrateType.Sort_12)
        //        {
        //            if (FrameOfSystem3.Recipe.Recipe.GetInstance().GetValue(FrameOfSystem3.Recipe.EN_RECIPE_TYPE.COMMON, FrameOfSystem3.Recipe.PARAM_COMMON.UseCycleMode.ToString(), false))
        //                substrateType = SubstrateType.Core_8;
        //        }

        //        if (substrateType == SubstrateType.Core_8 ||
        //            substrateType == SubstrateType.Core_12 ||
        //            substrateType == SubstrateType.Sort_12)
        //        {
        //            if (sourcePortId > 0)
        //            {
        //                targetPortId = sourcePortId;
        //                return true;
        //            }
        //        }

        //        // 캐리어가 있고, Accessing 된 거만 찾는다.
        //        int temporaryPortId = _loadPortManager.GetLoadPortPortId(i);
        //        if (_carrierServer.HasCarrier(temporaryPortId))
        //        {
        //            targetPortId = temporaryPortId;
        //            return true;
        //        }
        //    }

        //    return false;
        //}

        //private bool CheckCarrierExistanceBySubstrateType(int sourcePortId, int sourceSlot, string substrateType, ref int targetPortId, ref int targetSlot, ref SubstrateType targetType)
        //{
        //    if (false == Enum.TryParse(substrateType, out targetType))
        //        return false;

        //    bool result = FindPortInfo(sourcePortId, targetType, ref targetPortId);
        //    if (result)
        //    {
        //        switch (targetType)
        //        {
        //            case SubstrateType.Core_8:
        //            case SubstrateType.Core_12:
        //                {
        //                    if (sourceSlot < 0)
        //                    {
        //                        targetSlot = -1;
        //                        int capacity = _carrierServer.GetCapacity(targetPortId);
        //                        for (int i = 0; i < capacity; ++i)
        //                        {
        //                            if (false == _substrateManager.HasSubstrateAtLoadPort(targetPortId, i))
        //                            {
        //                                targetSlot = i;
        //                                break;
        //                            }
        //                        }
        //                    }
        //                    else
        //                    {
        //                        targetSlot = sourceSlot;
        //                    }

        //                    if (targetSlot < 0)
        //                        return false;

        //                    return _carrierServer.HasCarrier(targetPortId);
        //                }
        //            case SubstrateType.Sort_12:
        //                {
        //                    if (Recipe.Recipe.GetInstance().GetValue(EN_RECIPE_TYPE.COMMON, PARAM_COMMON.UseCycleMode.ToString(),
        //                        false))
        //                    {
        //                        //targetPortId = 4;
        //                        targetType = SubstrateType.Core_8;
        //                    }

        //                    if (sourceSlot < 0)
        //                    {
        //                        targetSlot = -1;
        //                        int capacity = _carrierServer.GetCapacity(targetPortId);
        //                        for (int i = 0; i < capacity; ++i)
        //                        {
        //                            if (false == _substrateManager.HasSubstrateAtLoadPort(targetPortId, i))
        //                            {
        //                                targetSlot = i;
        //                                break;
        //                            }
        //                        }
        //                    }
        //                    else
        //                    {
        //                        targetSlot = sourceSlot;
        //                    }

        //                    if (targetSlot < 0)
        //                        return false;

        //                    return _carrierServer.HasCarrier(targetPortId);
        //                }

        //            default:
        //                return false;
        //        }
        //    }

        //    return false;
        //}
        //private bool CheckCarrierExistanceBySubstrateType2(int sourcePortId, string substrateType, ref int portId, ref SubstrateType targetType)
        //{
        //    if (false == Enum.TryParse(substrateType, out targetType))
        //        return false;

        //    switch (targetType)
        //    {
        //        case SubstrateType.Core_8:
        //        case SubstrateType.Core_12:
        //        case SubstrateType.Sort_12:
        //            portId = sourcePortId;
        //            return _carrierServer.HasCarrier(sourcePortId);

        //        //case SubstrateType.Bin:
        //        //    {
        //        //        if (Recipe.Recipe.GetInstance().GetValue(EN_RECIPE_TYPE.COMMON, PARAM_COMMON.UseCycleMode.ToString(),
        //        //            false))
        //        //        {
        //        //            portId = 4;
        //        //            targetType = SubstrateType.Core_8;
        //        //            return _carrierServer.HasCarrier(sourcePortId);
        //        //        }
        //        //        else
        //        //        {
        //        //            for (int i = 0; i < _loadPortManager.Count; ++i)
        //        //            {
        //        //                // 2024.09.03. jhlim [MOD] SubType을 UI에는 Center/Left/Right로 지정되도록 변경
        //        //                //FrameOfSystem3.Recipe.PARAM_EQUIPMENT paramName;
        //        //                //paramName = FrameOfSystem3.Recipe.PARAM_EQUIPMENT.LoadPortType1 + i;
        //        //                //string subTypeByRecipe = FrameOfSystem3.Recipe.Recipe.GetInstance().GetValue(FrameOfSystem3.Recipe.EN_RECIPE_TYPE.EQUIPMENT,
        //        //                //    paramName.ToString(),
        //        //                //    SubstrateType.Empty.ToString());

        //        //                //if (false == Enum.TryParse(subTypeByRecipe, out SubstrateType convertedSubType))
        //        //                //    continue;
        //        //                SubstrateType convertedSubType = _functionsForPWA500.GetSubstrateTypeByLoadPortIndex(i);
        //        //                // 2024.09.03. jhlim [END]

        //        //                if (false == targetType.Equals(convertedSubType))
        //        //                    continue;

        //        //                portId = _loadPortManager.GetLoadPortPortId(i);
        //        //                if (_carrierServer.HasCarrier(portId))
        //        //                    return true;
        //        //            }
        //        //        }

        //        //        return false;
        //        //    }

        //        default:
        //            return false;
        //    }
        //}
        //private int FindPortIdBySubstrateType(int sourcePortId, string substrateType)
        //{
        //    if (false == Enum.TryParse(substrateType, out SubstrateType targetType))
        //        return -1;

        //    switch (targetType)
        //    {
        //        case SubstrateType.Core_8:
        //        case SubstrateType.Core_12:
        //        case SubstrateType.Sort_12:
        //            return sourcePortId;

        //        //case SubstrateType.Bin:
        //        //    {
        //        //        for (int i = 0; i < _loadPortManager.Count; ++i)
        //        //        {
        //        //            // 2024.09.03. jhlim [MOD] SubType을 UI에는 Center/Left/Right로 지정되도록 변경
        //        //            //FrameOfSystem3.Recipe.PARAM_EQUIPMENT paramName;
        //        //            //paramName = FrameOfSystem3.Recipe.PARAM_EQUIPMENT.LoadPortType1 + i;
        //        //            //string subTypeByRecipe = FrameOfSystem3.Recipe.Recipe.GetInstance().GetValue(FrameOfSystem3.Recipe.EN_RECIPE_TYPE.EQUIPMENT,
        //        //            //    paramName.ToString(),
        //        //            //    SubstrateType.Empty.ToString());

        //        //            //if (false == Enum.TryParse(subTypeByRecipe, out SubstrateType convertedSubType))
        //        //            //    continue;
        //        //            SubstrateType convertedSubType = _functionsForPWA500.GetSubstrateTypeByLoadPortIndex(i);
        //        //            // 2024.09.03. jhlim [END]

        //        //            if (false == targetType.Equals(convertedSubType))
        //        //                continue;

        //        //            return _loadPortManager.GetLoadPortPortId(i);
        //        //        }

        //        //        return -1;
        //        //    }

        //        default:
        //            return -1;
        //    }
        //}
        private CheckingCarrierCodeToUnload FindWellknownProtInfoBySubstrateType(
            Substrate substrate,
            SubstrateType subType,
            ref int portId,
            ref int slot,
            ref string description,
            bool isSeperatedWithBinAndEmpty)
        {
            // 개조 전 : 받은 포트로 그대로 넘겨준다.
            if (false == isSeperatedWithBinAndEmpty)
            {
                portId = substrate.SourcePortId;
                slot = substrate.SourceSlot;
                var lpIndex = _loadPortManager.GetLoadPortIndexByPortId(portId);

                if (false == _loadPortManager.IsLoadPortEnabled(lpIndex))
                {
                    description = ErrorDescriptionsForMaterialHanding.ErrorDescriptionForLoadPortNotEnabled;
                    return CheckingCarrierCodeToUnload.PortNotEnabled;
                    //return false;
                }

                if (false == _carrierServer.HasCarrier(portId) ||
                    _carrierServer.GetCarrierAccessingStatus(portId).Equals(CarrierAccessStates.CarrierCompleted) ||
                    _carrierServer.GetCarrierAccessingStatus(portId).Equals(CarrierAccessStates.CarrierStopped))
                {
                    description = ErrorDescriptionsForMaterialHanding.ErrorDescriptionForDoesntHaveCarrier;

                    return CheckingCarrierCodeToUnload.DoesNotHaveToAccessCarrier;
                }

                if (false == _loadPortManager.GetDoorState(lpIndex))
                {
                    description = ErrorDescriptionsForMaterialHanding.ErrorDescriptionForDoorIsNotOpened;
                    return CheckingCarrierCodeToUnload.DoorIsNotOpened;
                    //return false;
                }

                return CheckingCarrierCodeToUnload.Ok;
            }

            // 개조 후 : 작업 완료된 자재는 Bin 전용 포트로 찾아 넣는다.
            description = string.Empty;
            portId = -1; slot = -1;
            switch (subType)
            {
                case SubstrateType.Bin1:
                case SubstrateType.Bin2:
                case SubstrateType.Bin3:
                    {
                        int lpIndex = -1;
                        for (int i = 0; i < _loadPortManager.Count; ++i)
                        {
                            SubstrateType convertedSubType = _functionsForPWA500.GetSubstrateTypeByLoadPortIndex(i);
                            if (false == subType.Equals(convertedSubType))
                                continue;

                            lpIndex = i;
                            break;
                        }

                        // 비정상 포트
                        portId = _loadPortManager.GetLoadPortPortId(lpIndex);
                        if (lpIndex < 0 || portId <= 0)
                        {
                            description = ErrorDescriptionsForMaterialHanding.ErrorDescriptionForInvalidSubstratePortInfo;
                            return CheckingCarrierCodeToUnload.InvalidPortInfo;
                        }

                        // 포트 미사용
                        if (false == _loadPortManager.IsLoadPortEnabled(lpIndex))
                        {
                            description = ErrorDescriptionsForMaterialHanding.ErrorDescriptionForLoadPortNotEnabled;
                            return CheckingCarrierCodeToUnload.PortNotEnabled;
                            //return false;
                        }

                        // 포트상태 비정상
                        if (false == _carrierServer.HasCarrier(portId) ||
                            _carrierServer.GetCarrierAccessingStatus(portId).Equals(CarrierAccessStates.CarrierCompleted) ||
                            _carrierServer.GetCarrierAccessingStatus(portId).Equals(CarrierAccessStates.CarrierStopped))
                        {
                            description = ErrorDescriptionsForMaterialHanding.ErrorDescriptionForDoesntHaveCarrier;

                            return CheckingCarrierCodeToUnload.DoesNotHaveToAccessCarrier;
                        }

                        // 문이 닫힘
                        if (false == _loadPortManager.GetDoorState(lpIndex))
                        {
                            description = ErrorDescriptionsForMaterialHanding.ErrorDescriptionForDoorIsNotOpened;
                            return CheckingCarrierCodeToUnload.DoorIsNotOpened;
                            //return false;
                        }

                        // 포트에서 순차탐색한다.
                        int capacity = _carrierServer.GetCapacity(portId);
                        var substrates = _substrateManager.GetSubstratesAtLoadPort(portId);
                        var loadingMode = _loadPortManager.GetCarrierLoadingType(lpIndex);
                        for (int i = 1; i <= capacity; ++i)
                        {
                            if (i == 1 &&
                                (loadingMode == LoadPortLoadingMode.Cassette ||
                                loadingMode == LoadPortLoadingMode.ClosedCassette))
                                continue;

                            if (false == substrates.ContainsKey(i))
                            {
                                slot = i;
                                return CheckingCarrierCodeToUnload.Ok;
                            }
                        }

                        description = ErrorDescriptionsForMaterialHanding.ErrorDescriptionForSlotIsFull;
                        return CheckingCarrierCodeToUnload.SlotsIsFull;
                        //return false;
                    }

                default:
                    {
                        portId = substrate.SourcePortId;
                        slot = substrate.SourceSlot;
                        var lpIndex = _loadPortManager.GetLoadPortIndexByPortId(portId);

                        if (false == _loadPortManager.IsLoadPortEnabled(lpIndex))
                        {
                            description = ErrorDescriptionsForMaterialHanding.ErrorDescriptionForLoadPortNotEnabled;
                            return CheckingCarrierCodeToUnload.PortNotEnabled;
                            //return false;
                        }

                        if (false == _carrierServer.HasCarrier(portId) ||
                            _carrierServer.GetCarrierAccessingStatus(portId).Equals(CarrierAccessStates.CarrierCompleted) ||
                            _carrierServer.GetCarrierAccessingStatus(portId).Equals(CarrierAccessStates.CarrierStopped))
                        {
                            description = ErrorDescriptionsForMaterialHanding.ErrorDescriptionForDoesntHaveCarrier;

                            return CheckingCarrierCodeToUnload.DoesNotHaveToAccessCarrier;
                        }

                        if (false == _loadPortManager.GetDoorState(lpIndex))
                        {
                            description = ErrorDescriptionsForMaterialHanding.ErrorDescriptionForDoorIsNotOpened;
                            return CheckingCarrierCodeToUnload.DoorIsNotOpened;
                            //return false;
                        }

                        return CheckingCarrierCodeToUnload.Ok;
                    }
                    //return true;
            }
        }

        private bool FindWellknownProtInfoBySubstrateTypeOld(Substrate substrate, SubstrateType subType, ref int portId, ref int slot, ref string description)
        {
            description = string.Empty;
            portId = -1; slot = -1;
            // TODO : 2025.02.19. dwlim [MOD] Sort_12 Substrate Source와 Destination 다른 문제 수정했음
            portId = substrate.SourcePortId;
            slot = substrate.SourceSlot;
            return true;
            //switch (subType)
            //{
            //    case SubstrateType.Bin_12:
            //        {
            //            int lpIndex = -1;
            //            for (int i = 0; i < _loadPortManager.Count; ++i)
            //            {
            //                // 2024.09.03. jhlim [MOD] SubType을 UI에는 Center/Left/Right로 지정되도록 변경
            //                //FrameOfSystem3.Recipe.PARAM_EQUIPMENT paramName;
            //                //paramName = FrameOfSystem3.Recipe.PARAM_EQUIPMENT.LoadPortType1 + i;
            //                //string subTypeByRecipe = FrameOfSystem3.Recipe.Recipe.GetInstance().GetValue(FrameOfSystem3.Recipe.EN_RECIPE_TYPE.EQUIPMENT,
            //                //    paramName.ToString(),
            //                //    SubstrateType.Empty.ToString());

            //                //if (false == Enum.TryParse(subTypeByRecipe, out SubstrateType convertedSubType))
            //                //    continue;
            //                SubstrateType convertedSubType = _functionsForPWA500.GetSubstrateTypeByLoadPortIndex(i);
            //                // 2024.09.03. jhlim [END]

            //                if (false == subType.Equals(convertedSubType))
            //                    continue;

            //                lpIndex = i;
            //                break;
            //            }

            //            if (lpIndex < 0)
            //            {
            //                description = ErrorDescriptionForInvalidSubstratePortInfo;
            //                return false;
            //            }

            //            portId = _loadPortManager.GetLoadPortPortId(lpIndex);
            //            if (portId <= 0)
            //            {
            //                description = ErrorDescriptionForInvalidSubstratePortInfo;
            //                return false;
            //            }

            //            if (false == _carrierServer.HasCarrier(portId))
            //            {
            //                description = ErrorDescriptionForDoesntHaveCarrier;
            //                return false;
            //            }

            //            if (false == _loadPortManager.IsLoadPortEnabled(lpIndex))
            //            {
            //                description = ErrorDescriptionForLoadPortNotEnabled;
            //                return false;
            //            }

            //            if (false == _loadPortManager.GetDoorState(lpIndex))
            //            {
            //                description = ErrorDescriptionForDoorIsNotOpened;
            //                return false;
            //            }

            //            int capacity = _carrierServer.GetCapacity(portId);
            //            var substrates = _substrateManager.GetSubstratesAtLoadPort(portId);
            //            var loadingMode = _loadPortManager.GetCarrierLoadingType(lpIndex);
            //            for (int i = 0; i < capacity; ++i)
            //            {
            //                if (i == 0 && loadingMode.Equals(LoadPortLoadingMode.Cassette))
            //                    continue;

            //                if (false == substrates.ContainsKey(i))
            //                {
            //                    slot = i;
            //                    return true;
            //                }
            //            }

            //            description = ErrorDescriptionForSlotIsFull;
            //            return false;
            //        }

            //    default:
            //        portId = substrate.SourcePortId;
            //        slot = substrate.SourceSlot;
            //        return true;
            //}
        }

        //private int FindDestinationPortBySubstrateType(Substrate substrate, SubstrateType subType)
        //{
        //    if (subType.Equals(SubstrateType.Core_8) ||
        //        subType.Equals(SubstrateType.Core_12) ||
        //        subType.Equals(SubstrateType.Sort_12))
        //    {
        //        return substrate.SourcePortId;
        //    }

        //    //int portId = -1;
        //    for (int i = 0; i < _loadPortManager.Count; ++i)
        //    {
        //        // 2024.09.03. jhlim [MOD] SubType을 UI에는 Center/Left/Right로 지정되도록 변경
        //        //FrameOfSystem3.Recipe.PARAM_EQUIPMENT paramName;
        //        //paramName = FrameOfSystem3.Recipe.PARAM_EQUIPMENT.LoadPortType1 + i;
        //        //string subTypeByRecipe = FrameOfSystem3.Recipe.Recipe.GetInstance().GetValue(FrameOfSystem3.Recipe.EN_RECIPE_TYPE.EQUIPMENT,
        //        //    paramName.ToString(),
        //        //    SubstrateType.Empty.ToString());

        //        //if (false == Enum.TryParse(subTypeByRecipe, out SubstrateType convertedSubType))
        //        //    continue;
        //        SubstrateType convertedSubType = _functionsForPWA500.GetSubstrateTypeByLoadPortIndex(i);
        //        // 2024.09.03. jhlim [END]

        //        if (false == subType.Equals(convertedSubType))
        //            continue;

        //        switch (convertedSubType)
        //        {
        //            case SubstrateType.Sort_12:
        //                return _loadPortManager.GetLoadPortPortId(i);

        //            default:
        //                break;
        //        }
        //    }

        //    return -1;
        //}

        protected override CommandResults IsApproachUnloadingPrepared()
        {
            const string MethodName = "IsApproachUnloadingPrepared";

            // 상황이 바뀌었을 수 있다..
            if (false == IsUnloadingSignalStillActive(_workingInfo.LocationId))
                return ReturnSkipped(MethodName);

            switch (_subStepInterface)
            {
                case 0:
                    {
                        var locName = _workingInfo.LocationId;

                        SubstrateType targetType = SubstrateType.Core;
                        SubstrateSize targetSize = SubstrateSize.Inch_8;
                        if (false == _functionsForPWA500.GetSubstrateSpecByRequestedLocation(locName, ref targetType, ref targetSize) ||
                            false == TryFindValidPortId(targetType, targetSize))
                        {
                            return ReturnToError(
                                CommandResult.Error,
                                EN_ALARM.INTERFACE_BEFORE_UNLOADING_SENDING_FAILED,
                                MethodName,
                                _subStepInterface,
                                $"Does not have [{targetType}] at loadport"
                            );
                        }

                        ++_subStepInterface;
                        _processGroup.SetUnloadingSignal(ProcessModuleIndex, _workingInfo.LocationId, true);
                        SetDelayForSequence(500);

                        #region <Original>
                        //if (locName.Contains("12"))
                        //{
                        //    int portId = 2;
                        //    if (locName.Contains("Sort"))
                        //    {
                        //        portId = 1;
                        //    }
                        //    hasCarrier = _carrierServer.HasCarrier(portId);
                        //    if (hasCarrier)
                        //    {
                        //        _processGroup.SetUnloadingSignal(ProcessModuleIndex, _workingInfo.Location.Name, true);
                        //    }

                        //    ++_subStepInterface;
                        //    SetDelayForSequence(500);
                        //}
                        // 2025.06.18. jhlim [END]
                        #endregion </Original>
                    }
                    break;
                case 1:
                    {
                        // 이 시점에는 어떤 자재인지 모르니 Key가 없다.
                        //if (false == GetSubstrateNameByKeyForUnloading(_workingInfo.SubstrateKey, out var name) || string.IsNullOrWhiteSpace(name))
                        //{
                        //    return ReturnToError(CommandResult.Error, EN_ALARM.INTERFACE_BEFORE_UNLOADING_DATA_INVALID, MethodName, _subStepInterface, RequestMessages.RequestApproachLoading.ToString());
                        //}

                        // 1. ApproachUnloading을 전송
                        if (false == _processGroup.SendMessage(ProcessModuleIndex, _workingInfo.LocationId,
                            RequestMessages.RequestApproachUnloading.ToString(), string.Empty))
                        {
                            return ReturnToError(CommandResult.Error, EN_ALARM.INTERFACE_BEFORE_UNLOADING_SENDING_FAILED, MethodName,
                                _subStepInterface, RequestMessages.RequestApproachUnloading.ToString());
                        }

                        Ticks.SetTickCount(TimeoutShort);
                        ++_subStepInterface;
                    }
                    break;
                case 2:
                    {
                        if (IsTickOver())
                        {
                            return ReturnToError(CommandResult.Timeout, EN_ALARM.INTERFACE_BEFORE_UNLOADING_SENDING_COMPLETED_TIMEOUT_ACK, MethodName,
                                _subStepInterface, RequestMessages.RequestApproachUnloading.ToString());
                        }

                        //  2. Ack 확인
                        var result = _processGroup.IsSendingCompleted(ProcessModuleIndex, _workingInfo.LocationId,
                            RequestMessages.RequestApproachUnloading.ToString());

                        switch (result)
                        {
                            case CommunicationResult.Ack:
                                Ticks.SetTickCount(TimeoutLong);
                                ++_subStepInterface;
                                break;
                            case CommunicationResult.Nack:
                            case CommunicationResult.Error:
                                {
                                    return ReturnToError(CommandResult.Error, EN_ALARM.INTERFACE_BEFORE_UNLOADING_SENDING_COMPLETED_BUT_NACK, MethodName,
                                        _subStepInterface, RequestMessages.RequestApproachUnloading.ToString());
                                }
                            default:
                                break;
                        }

                    }
                    break;
                case 3:
                    {
                        if (IsTickOver())
                        {
                            return ReturnToError(CommandResult.Timeout, EN_ALARM.INTERFACE_BEFORE_UNLOADING_RECEIVING_RESPONSE_MESSAGE_TIMEOUT, MethodName,
                                _subStepInterface, ResponseMessages.ResponseApproachUnloading.ToString());
                        }

                        // 3. Response 확인
                        var result = _processGroup.IsMessageReceived(ProcessModuleIndex, _workingInfo.LocationId,
                            ResponseMessages.ResponseApproachUnloading.ToString());
                        switch (result)
                        {
                            case CommunicationResult.Ack:
                                Ticks.SetTickCount(TimeoutShort);
                                ++_subStepInterface;
                                break;

                            case CommunicationResult.Nack:
                            case CommunicationResult.Error:
                                return ReturnToError(CommandResult.Error, EN_ALARM.INTERFACE_BEFORE_UNLOADING_RECEIVING_COMPLETED_BUT_ERROR, MethodName,
                                    _subStepInterface, ResponseMessages.ResponseApproachUnloading.ToString());

                            default:
                                break;
                        }
                    }
                    break;

                case 4:
                    {
                        if (IsTickOver())
                        {
                            return ReturnToError(CommandResult.Timeout, EN_ALARM.INTERFACE_BEFORE_UNLOADING_RECEIVING_RESPONSE_DATA_TIMEOUT, MethodName,
                                _subStepInterface, ResponseMessages.ResponseApproachUnloading.ToString());
                        }

                        // 4. 데이터 확인
                        if (false == _processGroup.GetReceivedData(ProcessModuleIndex, _workingInfo.LocationId,
                            ResponseMessages.ResponseApproachUnloading.ToString(), out var receivedData))
                            break;

                        #region <Data 확인>
                        Substrate substrate;
                        SubstrateType convertedType = SubstrateType.Core;
                        string substrateName, lotId, recipeId, ringId, portId, slot, subType;
                        if (false == _processGroup.IsSimulationMode(ProcessModuleIndex))
                        {
                            if (false == receivedData.TryGetValue(PWA500MaterialHandling.SubstrateName, out substrateName))
                            {
                                return ReturnToError(CommandResult.Error, EN_ALARM.INTERFACE_BEFORE_UNLOADING_RECEIVING_COMPLETED_BUT_DATA_INVALID, MethodName,
                                    _subStepInterface, string.Format("{0} : {1}",
                                    ResponseMessages.ResponseApproachUnloading.ToString(), PWA500MaterialHandling.SubstrateName));
                            }

                            if (false == receivedData.TryGetValue(PWA500MaterialHandling.LotId, out lotId))
                            {
                                return ReturnToError(CommandResult.Error, EN_ALARM.INTERFACE_BEFORE_UNLOADING_RECEIVING_COMPLETED_BUT_DATA_INVALID, MethodName,
                                    _subStepInterface, string.Format("{0} : {1}",
                                    ResponseMessages.ResponseApproachUnloading.ToString(), PWA500MaterialHandling.LotId));
                            }

                            if (false == receivedData.TryGetValue(PWA500MaterialHandling.RecipeId, out recipeId))
                            {
                                return ReturnToError(CommandResult.Error, EN_ALARM.INTERFACE_BEFORE_UNLOADING_RECEIVING_COMPLETED_BUT_DATA_INVALID, MethodName,
                                    _subStepInterface, string.Format("{0} : {1}",
                                    ResponseMessages.ResponseApproachUnloading.ToString(), PWA500MaterialHandling.RecipeId));
                            }

                            if (false == receivedData.TryGetValue(PWA500MaterialHandling.RingId, out ringId))
                            {
                                return ReturnToError(CommandResult.Error, EN_ALARM.INTERFACE_BEFORE_UNLOADING_RECEIVING_COMPLETED_BUT_DATA_INVALID, MethodName,
                                    _subStepInterface, string.Format("{0} : {1}",
                                    ResponseMessages.ResponseApproachUnloading.ToString(), PWA500MaterialHandling.RingId));
                            }

                            if (false == receivedData.TryGetValue(PWA500MaterialHandling.PortId, out portId))
                            {
                                return ReturnToError(CommandResult.Error, EN_ALARM.INTERFACE_BEFORE_UNLOADING_RECEIVING_COMPLETED_BUT_DATA_INVALID, MethodName,
                                    _subStepInterface, string.Format("{0} : {1}",
                                    ResponseMessages.ResponseApproachUnloading.ToString(), PWA500MaterialHandling.PortId));
                            }

                            if (false == receivedData.TryGetValue(PWA500MaterialHandling.SlotId, out slot))
                            {
                                return ReturnToError(CommandResult.Error, EN_ALARM.INTERFACE_BEFORE_UNLOADING_RECEIVING_COMPLETED_BUT_DATA_INVALID, MethodName,
                                    _subStepInterface, string.Format("{0} : {1}",
                                    ResponseMessages.ResponseApproachUnloading.ToString(), PWA500MaterialHandling.SlotId));
                            }

                            if (false == receivedData.TryGetValue(PWA500MaterialHandling.SubstrateType, out subType) ||
                                false == Enum.TryParse(subType, out convertedType))
                            {
                                return ReturnToError(CommandResult.Error, EN_ALARM.INTERFACE_BEFORE_UNLOADING_RECEIVING_COMPLETED_BUT_DATA_INVALID, MethodName,
                                    _subStepInterface, string.Format("{0} : {1}",
                                    ResponseMessages.ResponseApproachUnloading.ToString(), PWA500MaterialHandling.SubstrateType));
                            }

                            //if (subType == "Core")
                            //{
                            //    var locName = _workingInfo.Location.Name;
                            //    bool is300mm = locName.Contains("8");
                            //    bool is400mm = locName.Contains("12");
                            //    if (is300mm)
                            //    {
                            //        convertedType = SubstrateType.Core_8;
                            //    }
                            //    else if (is400mm)
                            //    {
                            //        convertedType = SubstrateType.Core_12;
                            //    }
                            //}
                            //else
                            //{
                            //    convertedType = SubstrateType.Sort_12;
                            //}

                            //if (false == receivedData.TryGetValue(PWA500WSubstrateAttributes.SubstrateType, out subType) ||
                            //    false == Enum.TryParse(subType, out convertedType))
                            //{
                            //    return ReturnToError(CommandResult.Error, EN_ALARM.INTERFACE_BEFORE_UNLOADING_RECEIVING_COMPLETED_BUT_DATA_INVALID, MethodName,
                            //        _subStepInterface, string.Format("{0} : {1}",
                            //        ResponseMessages.ResponseApproachUnloading.ToString(), PWA500WSubstrateAttributes.SubstrateType));
                            //}
                        }
                        else
                        {
                            #region <For Simulation>
                            substrate = null;

                            var processModuleName = _processGroup.GetProcessModuleName(ProcessModuleIndex);

                            List<string> requestedLocation = new List<string>();
                            _processGroup.IsUnloadingRequested(ProcessModuleIndex, ref requestedLocation);
                            // 2025.02.24. dwlim [MOD] Simulation Mode 수정 => Sort 1장 완료하려면 Core 2장 완료해야함
                            var substrates = new List<Substrate>();

                            var locId = _workingInfo.LocationId;
                            bool isCore = false;
                            if (locId.Contains("Core"))
                            {
                                isCore = true;
                            }
                            if (_substrateManager.GetSubstratesAtProcessModule(processModuleName, ref substrates))
                            {
                                foreach (var item in substrates)
                                {
                                    if (item.ProcessingStatus != ProcessingStates.Processed)
                                        continue;

                                    var subTypeString = item.GetAttribute(PWA500SubstrateAttributes.SubstrateType);
                                    if (false == Enum.TryParse(subTypeString, out SubstrateType substrateType))
                                        continue;

                                    var isCoreByType = substrateType == SubstrateType.Core;
                                    if (isCore == isCoreByType)
                                    {
                                        substrate = item;
                                        break;
                                    }
                                }
                            }

                            //var unloadingSubstrates = new List<Substrate>();
                            //_substrateManager.GetSubstratesAtProcessModule(processModuleName, ref substrates);

                            //bool existUnloadingCore = false;
                            //if (requestedLocation.Count != 0)
                            //{
                            //    for (int i = 0; i < requestedLocation.Count; i++)
                            //    {
                            //        foreach (var item in substrates)
                            //        {
                            //            var subTypeString = item.GetAttribute(PWA500SubstrateAttributes.SubstrateType);
                            //            if (false == Enum.TryParse(subTypeString, out SubstrateType substrateType))
                            //                continue;

                            //            if (substrateType == SubstrateType.Core)
                            //            {
                            //                if (false == item.ProcessingStatus.Equals(ProcessingStates.Processed))
                            //                    continue;

                            //                unloadingSubstrates.Add(item);
                            //            }
                            //        }
                            //        if (unloadingSubstrates.Count == 0)
                            //            break;

                            //        substrate = unloadingSubstrates.First();
                            //        existUnloadingCore = true;
                            //    }
                            //    if (false == existUnloadingCore)
                            //    {
                            //        foreach (var item in substrates)
                            //        {
                            //            var subTypeString = item.GetAttribute(PWA500SubstrateAttributes.SubstrateType);
                            //            if (false == Enum.TryParse(subTypeString, out SubstrateType substrateType))
                            //                continue;

                            //            if (false == substrateType.Equals(SubstrateType.Core))
                            //            {
                            //                if (false == item.ProcessingStatus.Equals(ProcessingStates.Processed))
                            //                    continue;

                            //                unloadingSubstrates.Add(item);
                            //            }
                            //        }
                            //        if (unloadingSubstrates.Count == 0)
                            //            break;

                            //        substrate = unloadingSubstrates.First();
                            //    }
                            //}
                            // 2025.02.24. dwlim [End]

                            substrateName = substrate.Name;
                            lotId = substrate.LotId;
                            recipeId = substrate.RecipeId;
                            portId = substrate.SourcePortId.ToString();
                            slot = substrate.SourceSlot.ToString();
                            subType = substrate.GetAttribute(PWA500SubstrateAttributes.SubstrateType);
                            Enum.TryParse(subType, out convertedType);
                            ringId = substrate.GetAttribute(PWA500SubstrateAttributes.RingId);
                            #endregion </For Simulation>
                        }

                        //bool hasCarrier = false;
                        if (false == _functionsForPWA500.FindSubstrateByAttribute(substrateName, ringId, portId, slot, out substrate) ||
                            substrate == null)
                        {
                            //#region <자재 정보를 못 찾은 경우>
                            //string substrateTemporaryName;
                            //if (string.IsNullOrEmpty(ringId) && string.IsNullOrEmpty(substrateName))
                            //    substrateTemporaryName = "Unknown";
                            //else if (string.IsNullOrEmpty(substrateName))
                            //    substrateTemporaryName = ringId;
                            //else
                            //    substrateTemporaryName = substrateName;

                            //// 받은 이름이 같을 수도 있으니, Key를 년월시_시분초밀리초를 붙인 값으로 하자
                            //_substrateManager.CreateSubstrate(string.Format("{0}_{1}", substrateTemporaryName, DateTime.Now.ToString("yyyyMMdd_HHmmssfff")), substrateTemporaryName, _workingInfo.Location);

                            //substrate = new Substrate();
                            //bool result = false;

                            //if (_workingInfo.Location is ProcessModuleLocation pmLocation)
                            //{
                            //    result = _substrateManager.GetSubstrateAtProcessModule(substrateTemporaryName, pmLocation, ref substrate);
                            //}

                            //if (result)
                            //{
                            //    var attr = substrate.GetAttributesAll();
                            //    string[] keys = attr.Keys.ToArray();
                            //    for (int i = 0; i < keys.Length; ++i)
                            //    {
                            //        if (attr[keys[i]] == null)
                            //        {
                            //            substrate.SetAttribute(keys[i], string.Empty);
                            //        }
                            //    }

                            //    substrate.SetAttribute(BaseSubstrateAttributeKeys.Name, substrateTemporaryName);
                            //    substrate.SetAttribute(PWA500SubstrateAttributes.RingId, substrateTemporaryName);
                            //    substrate.SetAttribute(BaseSubstrateAttributeKeys.ProcessingState, ProcessingStates.Processed.ToString());
                            //    substrate.SetAttribute(BaseSubstrateAttributeKeys.TransPortState, SubstrateTransferStates.AtWork.ToString());
                            //    substrate.SetAttribute(PWA500SubstrateAttributes.SubstrateType, subType);
                            //    substrate.SetAttribute(BaseSubstrateAttributeKeys.LotId, lotId);
                            //    substrate.SetAttribute(PWA500SubstrateAttributes.ParentLotId, lotId);
                            //    substrate.SetAttribute(BaseSubstrateAttributeKeys.RecipeId, recipeId);

                            //    int port = -1;// FindUnknownPortInfoBySubstrateType(substrate, convertedType);
                            //    int slotIndex = -1;// FindUnknownSlotInfoByPortId(port, convertedType);

                            //    var substrateSize = _functionsForPWA500.GetSubstrateSizeByLocationName(_workingInfo.Location.Name);
                            //    substrate.SetAttribute(PWA500SubstrateAttributes.SubstrateSize, substrateSize.ToString());

                            //    substrate.SetAttribute(BaseSubstrateAttributeKeys.SourcePortId, port.ToString());
                            //    substrate.SetAttribute(BaseSubstrateAttributeKeys.SourceSlot, slotIndex.ToString());
                            //    substrate.SetAttribute(BaseSubstrateAttributeKeys.DestinationPortId, port.ToString());
                            //    substrate.SetAttribute(BaseSubstrateAttributeKeys.DestinationSlot, slotIndex.ToString());
                            //    substrateName = substrateTemporaryName;
                            //}
                            //else
                            {
                                // 못찾으면 에러
                                return ReturnToError(CommandResult.Error, EN_ALARM.INTERFACE_BEFORE_UNLOADING_RECEIVING_COMPLETED_BUT_DATA_INVALID, MethodName,
                                    _subStepInterface, string.Format("Find substrate has failed by Receiving data : {0}, [{1},{2},{3}]", ResponseMessages.ResponseApproachUnloading.ToString(),
                                    ringId, portId, slot));
                            }
                            //#endregion </자재 정보를 못 찾은 경우>
                        }

                        // 찾았으면 Set
                        if (false == int.TryParse(portId, out int sourcePortId))
                            sourcePortId = -1;
                        if (false == int.TryParse(slot, out int sourceSlot))
                            sourceSlot = -1;

                        var key = substrate.UniqueKey;
                        _workingInfo.SubstrateKey = key;
                        _substrateManager.SetNameByKey(key, substrateName);
                        _substrateManager.SetRecipeIdByKey(key, recipeId);
                        _substrateManager.SetSourcePortIdByKey(key, sourcePortId);
                        _substrateManager.SetSourceSlotByKey(key, sourceSlot);
                        _substrateManager.SetProcessingStatusByKey(key, ProcessingStates.Processed);
                        //_substrateManager.SetLocationByKey(key, _workingInfo.Location);
                        //substrate.SetName(substrateName);
                        //substrate.SetRecipeId(recipeId);
                        //substrate.SetSourcePortId(sourcePortId);
                        //substrate.SetSourceSlot(sourceSlot);
                        //substrate.SetProcessingStatus(ProcessingStates.Processed);
                        _substrateManager.SetAttributeByKey(key, PWA500SubstrateAttributes.SubstrateType, convertedType.ToString());                        

                        //if (sourcePortId > 0)
                        //{
                        int targetPortId = 0, targetSlot = 0;
                        string description = string.Empty;
                        // 2024.08.22. jhlim [MOD] 공정설비에서 자재를 찾도록 수정
                        //hasCarrier = FindWellknownProtInfoBySubstrateType(substrate, convertedType, ref targetPortId, ref targetSlot, ref description);
                        var checkingResult = FindWellknownProtInfoBySubstrateType(substrate, convertedType, ref targetPortId, ref targetSlot, ref description, false);
                        if (checkingResult == CheckingCarrierCodeToUnload.Ok)
                        {
                            _substrateManager.SetDestinationPortIdByKey(key, targetPortId);
                            _substrateManager.SetDestinationSlotByKey(key, targetSlot);
                            //substrate.SetDestinationPortId(targetPortId);
                            //substrate.SetDestinationSlot(targetSlot);
                        }

                        // 2024.08.22. jhlim [END]
                        //hasCarrier = CheckCarrierExistanceBySubstrateType(_substrateToPick.SourcePortId, _substrateToPick.SourceSlot, subType, ref targetPortId, ref targetSlot, ref convertedType);
                        //if (targetPortId > 0)
                        //{
                        //    _substrateToPick.SetProcessingStatus(ProcessingStates.Processed);
                        //    _substrateToPick.SetDestinationPortId(targetPortId);
                        //    _substrateToPick.SetDestinationSlot(targetSlot);
                        //    switch (convertedType)
                        //    {
                        //        case SubstrateType.Bin1:
                        //        case SubstrateType.Bin2:
                        //        case SubstrateType.Bin3:
                        //            {
                        //                _substrateToPick.SetDestinationPortId(targetPortId);
                        //                _substrateToPick.SetDestinationSlot(targetSlot);
                        //                _substrateToPick.SetAttribute(PWA500WSubstrateAttributes.SubstrateType, convertedType.ToString());
                        //            }
                        //            break;

                        //        default:
                        //            break;
                        //    }
                        //}
                        //}

                        // TODO : 원래 이름을 여기서 다시 갱신해줬었는데(공정설비에서 바꼈을 수도 있으니), 이제는 Key 기반이니까 필요 없다.
                        //_workingInfo.SubstrateName = substrate.Name;

                        //if (ManualWorkingInfo.Count > 0)
                        //{
                        //    ManualWorkingInfo.First().Value.SubstrateName = substrate.Name;
                        //}
                        _substrateManager.SaveDataByKey(key);
                        #endregion </Data 확인>

                        // 5. Ack 전송 : 콜백에서 자동 Ack 나가니 현재 미구현
                        //if (false == _processGroup.SetAckReceivedMessage(ProcessModuleIndex, _workingInfo.Location,
                        //    ResponseMessages.ResponseApproachUnloading.ToString(), CommunicationResult.Ack, string.Empty))
                        //{

                        //}

                        string handlingResult = checkingResult == CheckingCarrierCodeToUnload.Ok ?
                            PWA500MaterialHandling.HandlingResultOk : PWA500MaterialHandling.HandlingResultNg;
                        receivedData[PWA500MaterialHandling.HandlingResult] = handlingResult;

                        if (false == _processGroup.SendMessage(ProcessModuleIndex, _workingInfo.LocationId,
                            RequestMessages.RequestStartUnloading.ToString(), receivedData))
                        {
                            return ReturnToError(CommandResult.Error, EN_ALARM.INTERFACE_BEFORE_UNLOADING_SENDING_FAILED, MethodName,
                                _subStepInterface, RequestMessages.RequestStartUnloading.ToString());
                        }

                        if (sourcePortId > 0 && checkingResult != CheckingCarrierCodeToUnload.Ok)
                        {
                            int lpIndex = _loadPortManager.GetLoadPortIndexByPortId(sourcePortId);
                            if (description.Equals(ErrorDescriptionsForMaterialHanding.ErrorDescriptionForDoesntHaveCarrier))
                            {
                                // Carrier가 없고, Auto면 올 때까지 대기해야 하므로 스킵
                                if (_loadPortManager.GetAccessMode(lpIndex).Equals(LoadPortAccessMode.Auto))
                                {
                                    return ReturnSkipped(MethodName);
                                }
                                else
                                {
                                    return ReturnToError(CommandResult.Error, EN_ALARM.ATM_ROBOT_DOES_NOT_HAVE_CARRIER, MethodName, _subStepInterface, description);
                                }
                            }
                            else
                            {
                                return ReturnToError(CommandResult.Error, EN_ALARM.INTERFACE_BEFORE_UNLOADING_RECEIVING_COMPLETED_BUT_DATA_INVALID, MethodName,
                                    _subStepInterface, description);
                            }
                            //return ReturnSkipped(MethodName, description);
                        }

                        Ticks.SetTickCount(TimeoutShort);
                        ++_subStepInterface;
                    }
                    break;

                case 5:
                    {
                        if (IsTickOver())
                        {
                            return ReturnToError(CommandResult.Timeout, EN_ALARM.INTERFACE_BEFORE_UNLOADING_SENDING_COMPLETED_TIMEOUT_ACK, MethodName,
                                _subStepInterface, RequestMessages.RequestStartUnloading.ToString());
                        }

                        //  2. Ack 확인
                        var result = _processGroup.IsSendingCompleted(ProcessModuleIndex, _workingInfo.LocationId,
                            RequestMessages.RequestStartUnloading.ToString());

                        switch (result)
                        {
                            case CommunicationResult.Ack:
                                Ticks.SetTickCount(TimeoutLong);
                                ++_subStepInterface;
                                break;
                            case CommunicationResult.Nack:
                            case CommunicationResult.Error:
                                {
                                    return ReturnToError(CommandResult.Error, EN_ALARM.INTERFACE_BEFORE_UNLOADING_SENDING_COMPLETED_BUT_NACK, MethodName,
                                        _subStepInterface, RequestMessages.RequestStartUnloading.ToString());
                                }
                            default:
                                break;
                        }
                    }
                    break;

                case 6:
                    {
                        if (IsTickOver())
                        {
                            return ReturnToError(CommandResult.Timeout, EN_ALARM.INTERFACE_BEFORE_UNLOADING_RECEIVING_RESPONSE_DATA_TIMEOUT, MethodName,
                                _subStepInterface, ResponseMessages.ResponseStartUnloading.ToString());
                        }

                        // 3. Response 확인 
                        //var result = _processGroup.IsMessageReceived(ProcessModuleIndex, _workingInfo.LocationId,
                        //    ResponseMessages.ResponseStartUnloading.ToString());
                        //switch (result)
                        //{
                        //    case CommunicationResult.Ack:
                        //        _processGroup.SetUnloadingSignal(ProcessModuleIndex, _workingInfo.Location, true);
                        //        return ReturnCompleted();

                        //    case CommunicationResult.Nack:
                        //    case CommunicationResult.Error:
                        //        return CommandResult.Error;

                        //    default:
                        //        break;
                        //}

                        if (false == _processGroup.GetReceivedData(ProcessModuleIndex, _workingInfo.LocationId, ResponseMessages.ResponseStartUnloading.ToString(), out _))
                            break;

                        _processGroup.SetUnloadingSignal(ProcessModuleIndex, _workingInfo.LocationId, true);

                        return ReturnCompleted();
                    }

                default:
                    break;
            }

            return ReturnProceed();
        }
        protected override CommandResults IsApproachUnloadingCompleted()
        {
            const string MethodName = "IsApproachUnloadingCompleted";

            // 상황이 바뀌었을 수 있다..
            if (false == IsUnloadingSignalStillActive(_workingInfo.LocationId))
                return ReturnSkipped(MethodName);

            return ReturnCompleted();
        }
        protected override CommandResults IsUnloadingPrepared()
        {
            const string MethodName = "IsUnloadingPrepared";

            // 상황이 바뀌었을 수 있다..
            if (false == IsUnloadingSignalStillActive(_workingInfo.LocationId))
                return ReturnSkipped(MethodName);

            switch (_subStepInterface)
            {
                case 0:
                    {
                        if (false == GetSubstrateNameByKeyForUnloading(_workingInfo.SubstrateKey, out var name) || string.IsNullOrWhiteSpace(name))
                        {
                            return ReturnToError(CommandResult.Error, EN_ALARM.INTERFACE_ACTION_UNLOADING_DATA_INVALID, MethodName, _subStepInterface, RequestMessages.RequestApproachLoading.ToString());
                        }
                        
                        // 1. ActionUnloading을 전송
                        if (false == _processGroup.SendMessage(ProcessModuleIndex, _workingInfo.LocationId,
                            RequestMessages.RequestActionUnloading.ToString(), name))
                        {
                            return ReturnToError(CommandResult.Error, EN_ALARM.INTERFACE_ACTION_UNLOADING_SENDING_FAILED, MethodName,
                                _subStepInterface, RequestMessages.RequestActionUnloading.ToString());
                        }

                        Ticks.SetTickCount(TimeoutShort);
                        ++_subStepInterface;
                    }
                    break;
                case 1:
                    {
                        if (IsTickOver())
                        {
                            return ReturnToError(CommandResult.Timeout, EN_ALARM.INTERFACE_ACTION_UNLOADING_SENDING_COMPLETED_TIMEOUT_ACK, MethodName,
                                _subStepInterface, RequestMessages.RequestActionUnloading.ToString());
                        }

                        //  2. Ack 확인
                        var result = _processGroup.IsSendingCompleted(ProcessModuleIndex, _workingInfo.LocationId,
                            RequestMessages.RequestActionUnloading.ToString());

                        switch (result)
                        {
                            case CommunicationResult.Ack:
                                Ticks.SetTickCount(TimeoutLong);
                                ++_subStepInterface;
                                break;
                            case CommunicationResult.Nack:
                            case CommunicationResult.Error:
                                {
                                    return ReturnToError(CommandResult.Error, EN_ALARM.INTERFACE_ACTION_UNLOADING_SENDING_COMPLETED_BUT_NACK, MethodName,
                                        _subStepInterface, RequestMessages.RequestActionUnloading.ToString());
                                }
                            default:
                                break;
                        }

                    }
                    break;
                case 2:
                    {
                        if (IsTickOver())
                        {
                            return ReturnToError(CommandResult.Timeout, EN_ALARM.INTERFACE_ACTION_UNLOADING_RECEIVING_RESPONSE_MESSAGE_TIMEOUT, MethodName,
                                _subStepInterface, ResponseMessages.ResponseActionUnloading.ToString());
                        }

                        // 3. Response 확인
                        var result = _processGroup.IsMessageReceived(ProcessModuleIndex, _workingInfo.LocationId,
                            ResponseMessages.ResponseActionUnloading.ToString());
                        switch (result)
                        {
                            case CommunicationResult.Ack:
                                Ticks.SetTickCount(TimeoutShort);
                                ++_subStepInterface;
                                break;

                            case CommunicationResult.Nack:
                            case CommunicationResult.Error:
                                {
                                    return ReturnToError(CommandResult.Timeout, EN_ALARM.INTERFACE_ACTION_UNLOADING_RECEIVING_COMPLETED_BUT_ERROR, MethodName,
                                        _subStepInterface, ResponseMessages.ResponseActionUnloading.ToString());
                                }

                            default:
                                break;
                        }
                    }
                    break;

                case 3:
                    {
                        if (IsTickOver())
                        {
                            return ReturnToError(CommandResult.Timeout, EN_ALARM.INTERFACE_ACTION_UNLOADING_RECEIVING_RESPONSE_DATA_TIMEOUT, MethodName,
                                _subStepInterface, ResponseMessages.ResponseActionUnloading.ToString());
                        }

                        // 4. 데이터 확인
                        if (false == _processGroup.GetReceivedData(ProcessModuleIndex, _workingInfo.LocationId,
                            ResponseMessages.ResponseActionUnloading.ToString(), out _))
                            break;

                        // 5. Ack 전송 : 콜백에서 자동 Ack 나가니 현재 미구현
                        //if (false == _processGroup.SetAckReceivedMessage(ProcessModuleIndex, _workingInfo.Location,
                        //    ResponseMessages.ResponseActionUnloading.ToString(), CommunicationResult.Ack, string.Empty))
                        //{

                        //}

                        return ReturnCompleted();
                    }

                default:
                    break;
            }

            return ReturnProceed();
        }
        protected override CommandResults IsUnloadingCompleted()
        {
            const string MethodName = "IsUnloadingCompleted";

            // 상황이 바뀌었을 수 있다..
            if (false == IsUnloadingSignalStillActive(_workingInfo.LocationId))
            {
                return ReturnSkipped(MethodName);
            }

            switch (_subStepInterface)
            {
                case 0:
                    {
                        if (false == _substrateManager.GetSubstrateByKey(_workingInfo.SubstrateKey, out var s) || s == null)
                        {
                            return ReturnToError(CommandResult.Error, EN_ALARM.INTERFACE_AFTER_UNLOADING_DATA_INVALID, MethodName, _subStepInterface, RequestMessages.RequestApproachLoading.ToString());
                        }

                        // 1. ConfirmUnloading 전송
                        if (false == _processGroup.SendMessage(ProcessModuleIndex, _workingInfo.LocationId,
                            RequestMessages.RequestConfirmUnloading.ToString(), s.Name))
                        {
                            return ReturnToError(CommandResult.Error, EN_ALARM.INTERFACE_AFTER_UNLOADING_SENDING_FAILED, MethodName,
                                _subStepInterface, RequestMessages.RequestConfirmUnloading.ToString());
                        }

                        Ticks.SetTickCount(TimeoutShort);
                        ++_subStepInterface;
                    }
                    break;
                case 1:
                    {
                        if (IsTickOver())
                        {
                            return ReturnToError(CommandResult.Timeout, EN_ALARM.INTERFACE_AFTER_UNLOADING_SENDING_COMPLETED_TIMEOUT_ACK, MethodName,
                                _subStepInterface, RequestMessages.RequestConfirmUnloading.ToString());
                        }

                        //  2. Ack 확인
                        var result = _processGroup.IsSendingCompleted(ProcessModuleIndex, _workingInfo.LocationId,
                            RequestMessages.RequestConfirmUnloading.ToString());

                        switch (result)
                        {
                            case CommunicationResult.Ack:
                                Ticks.SetTickCount(TimeoutLong);
                                ++_subStepInterface;
                                break;
                            case CommunicationResult.Nack:
                            case CommunicationResult.Error:
                                {
                                    return ReturnToError(CommandResult.Error, EN_ALARM.INTERFACE_AFTER_UNLOADING_SENDING_COMPLETED_BUT_NACK, MethodName,
                                        _subStepInterface, RequestMessages.RequestConfirmUnloading.ToString());
                                }
                            default:
                                break;
                        }

                    }
                    break;
                case 2:
                    {
                        if (IsTickOver())
                        {
                            return ReturnToError(CommandResult.Timeout, EN_ALARM.INTERFACE_AFTER_UNLOADING_RECEIVING_RESPONSE_MESSAGE_TIMEOUT, MethodName,
                                _subStepInterface, ResponseMessages.ResponseConfirmUnloading.ToString());
                        }

                        // 3. Response 확인
                        var result = _processGroup.IsMessageReceived(ProcessModuleIndex, _workingInfo.LocationId,
                            ResponseMessages.ResponseConfirmUnloading.ToString());
                        switch (result)
                        {
                            case CommunicationResult.Ack:
                                Ticks.SetTickCount(TimeoutShort);
                                ++_subStepInterface;
                                break;

                            case CommunicationResult.Nack:
                            case CommunicationResult.Error:
                                {
                                    return ReturnToError(CommandResult.Error, EN_ALARM.INTERFACE_AFTER_UNLOADING_RECEIVING_COMPLETED_BUT_ERROR, MethodName,
                                        _subStepInterface, ResponseMessages.ResponseConfirmUnloading.ToString());
                                }

                            default:
                                break;
                        }
                    }
                    break;

                case 3:
                    {
                        if (IsTickOver())
                        {
                            return ReturnToError(CommandResult.Timeout, EN_ALARM.INTERFACE_AFTER_UNLOADING_RECEIVING_RESPONSE_DATA_TIMEOUT, MethodName,
                                _subStepInterface, ResponseMessages.ResponseConfirmUnloading.ToString());
                        }

                        // 4. 데이터 확인
                        //Dictionary<string, string> receivedData = new Dictionary<string, string>();
                        //if (false == _processGroup.GetReceivedData(ProcessModuleIndex, _workingInfo.LocationId,
                        //    ResponseMessages.ResponseConfirmUnloading.ToString(), ref receivedData))
                        //    break;

                        //// 5. Ack 전송 : 콜백에서 자동 Ack 나가니 현재 미구현
                        //if (false == _processGroup.SetAckReceivedMessage(ProcessModuleIndex, _workingInfo.Location,
                        //    ResponseMessages.ResponseConfirmUnloading.ToString(), CommunicationResult.Ack))
                        //    return CommandResult.Error;

                        Ticks.SetTickCount(100);
                        ++_subStepInterface;
                    }
                    break;
                case 4:
                    {
                        if (false == IsTickOver())
                            break;

                        // 6. SMEMA OFF
                        _processGroup.SetUnloadingSignal(ProcessModuleIndex, _workingInfo.LocationId, false);

                        return ReturnCompleted();
                    }

                default:
                    break;
            }

            return ReturnProceed();
        }
        #endregion </Unloading>

        #endregion </Material Handling With Process Module>

        #region <Recovery Data>
        protected override void UpdateRecoveryDataBeforePick()
        {
        }

        protected override void UpdateRecoveryDataAfterPick()
        {
        }
        protected override void UpdateRecoveryDataBeforePlace()
        {
        }
        protected override void UpdateRecoveryDataAfterPlace()
        {
        }
        #endregion </Recovery Data>

        #region <Handling Location Info>
        protected override string GetProcessModuleLocationName(ProcessModuleLocation location)
        {
            string detailed;
            switch (location.Id)
            {
                case Constants.ProcessModuleCore_8_InputName:
                    detailed = "Supply InBuffer(#8)";
                    break;
                case Constants.ProcessModuleCore_8_OutputName:
                    detailed = "Supply OutBuffer(#8)";
                    break;
                case Constants.ProcessModuleCore_12_InputName:
                    detailed = "Supply InBuffer(#12)";
                    break;
                case Constants.ProcessModuleCore_12_OutputName:
                    detailed = "Supply OutBuffer(#12)";
                    break;
                case Constants.ProcessModuleSort_12_InputName:
                    detailed = "Sorting InBuffer";
                    break;
                case Constants.ProcessModuleSort_12_OutputName:
                    detailed = "Sorting OutBuffer";
                    break;
                default:
                    {
                        detailed = location.Id;
                    }
                    break;
            }
            
            return detailed;
        }
        #endregion </Handling Location Info>

        #endregion </Overrids>

        #region <Internals>

        #region <Scenario>
        private bool IsManual()
        {
            var currentStatus = EquipmentState_.EquipmentState.GetInstance().GetState();
            switch (currentStatus)
            {
                case EquipmentState_.EQUIPMENT_STATE.SETUP:
                    return true;

                case EquipmentState_.EQUIPMENT_STATE.EXECUTING:
                case EquipmentState_.EQUIPMENT_STATE.FINISHING:
                    return false;

                default:
                    return false;
            }
        }
        // W에서는 미사용
        //private bool IsValidPartIdsForSubstrates(ref Substrate substrate, ref string description)
        //{
        //    // 2025.07.15. jhlim [MOD] 자재의 파트이름이 아닌 캐리어의 파트 이름을 받아오도록 개선 -> 자재 파트 이름은 갱신 중일 수도 있음
        //    int portId = substrate.SourcePortId;
        //    if (false == _carrierServer.HasCarrier(portId))
        //    {
        //        description = "Does not have carrier at port";
        //    }

        //    string partIdToTrackIn = _carrierServer.GetAttribute(portId, PWA500BINCarrierAttributeKeys.KeyPartId);
        //    if (string.IsNullOrEmpty(partIdToTrackIn))
        //    {
        //        description = "Invalid Core PartId";
        //        return false;
        //    }
        //    // 2025.07.15. jhlim [END]

        //    ProcessingStates processingStatus;
        //    string partIdToCompare = string.Empty, substrateTypeToString = string.Empty, parentPartId = string.Empty, corePartId = string.Empty;
        //    var processModuleName = _processGroup.GetProcessModuleName(ProcessModuleIndex);
        //    if (_substrateManager.GetSubstratesAtProcessModule(processModuleName, ref _substratesAtProcessModule))
        //    {
        //        // 1.공정 설비내 자재 유무에 따른 조건
        //        //     1) Core Wafer 존재 시 파트 정보가 다를 경우 에러 발생
        //        //     2) 본설비내 부자재(공테이프) Wafer 존재 시
        //        //         - 작업 전(NeedsProcessing 상태) 상태는 파트 정보 상관없이 정상
        //        //         - 작업 중 / 완료(InProcessing / Processed) 상태의 경우 파트 정보 다를 시 에러
        //        //          -> 가.. 아니라 Core의 파트 정보 정합성을 비교한다. -> 없거나 다르면 에러(
        //        if (_substratesAtProcessModule.Count > 0)
        //        {
        //            foreach (var item in _substratesAtProcessModule)
        //            {
        //                substrateTypeToString = item.GetAttribute(PWA500BINSubstrateAttributes.SubstrateType);
        //                processingStatus = item.ProcessingStatus;
        //                parentPartId = item.GetAttribute(PWA500BINSubstrateAttributes.PartId);
        //                corePartId = item.GetAttribute(PWA500BINSubstrateAttributes.CorePartId);
        //                if (HasDifferentPartInfo(partIdToTrackIn, item.Name, substrateTypeToString, parentPartId, corePartId, processingStatus, ref description))
        //                {
        //                    return false;
        //                }
        //            }
        //        }

        //        // 2.포트 조건
        //        //     1) Core용 Carrier 존재 시
        //        //         - 작업 전 / 완료 상태인 경우 파트 정보 상관 없이 정상(NotAccessed / CarrierStopped / CarrierCompleted)
        //        //         - 작업 중인 상태인 경우 파트 정보가 다르면 에러(InAccessed)
        //        //     2) Bin용 Carrier 존재 시
        //        int sourcePortId = substrate.SourcePortId;
        //        for (int i = 0; i < _loadPortManager.Count; ++i)
        //        {
        //            if (false == _loadPortManager.IsLoadPortEnabled(i))
        //                continue;

        //            int portIdToCheck = _loadPortManager.GetLoadPortPortId(i);
        //            var substrateType = _functionsForPWA500.GetSubstrateTypeByLoadPortIndex(i);
        //            switch (substrateType)
        //            {
        //                case SubstrateType.Core:
        //                    {
        //                        if (sourcePortId != portIdToCheck)
        //                        {
        //                            var accessingStatus = _carrierServer.GetCarrierAccessingStatus(portIdToCheck);
        //                            switch (accessingStatus)
        //                            {
        //                                case CarrierAccessStates.InAccessed:
        //                                    {
        //                                        var substrates = _substrateManager.GetSubstratesAtLoadPort(portIdToCheck);
        //                                        if (substrates.Count > 0)
        //                                        {
        //                                            var substrateFirst = substrates.First().Value;
        //                                            if (substrateFirst != null)
        //                                            {
        //                                                substrateTypeToString = substrateFirst.GetAttribute(PWA500BINSubstrateAttributes.SubstrateType);
        //                                                processingStatus = substrateFirst.ProcessingStatus;
        //                                                parentPartId = substrateFirst.GetAttribute(PWA500BINSubstrateAttributes.PartId);
        //                                                corePartId = substrateFirst.GetAttribute(PWA500BINSubstrateAttributes.CorePartId);
        //                                                if (HasDifferentPartInfo(partIdToTrackIn, substrateFirst.Name, substrateTypeToString, parentPartId, corePartId, processingStatus, ref description))
        //                                                {
        //                                                    return false;
        //                                                }
        //                                            }
        //                                        }
        //                                    }
        //                                    break;
        //                                default:
        //                                    break;
        //                            }
        //                        }
        //                    }
        //                    break;
        //                case SubstrateType.Bin1:
        //                case SubstrateType.Bin2:
        //                case SubstrateType.Bin3:
        //                    {
        //                        var substrates = _substrateManager.GetSubstratesAtLoadPort(portIdToCheck);
        //                        foreach (var item in substrates)
        //                        {
        //                            substrateTypeToString = item.Value.GetAttribute(PWA500BINSubstrateAttributes.SubstrateType);
        //                            processingStatus = item.Value.ProcessingStatus;
        //                            parentPartId = item.Value.GetAttribute(PWA500BINSubstrateAttributes.PartId);
        //                            corePartId = item.Value.GetAttribute(PWA500BINSubstrateAttributes.CorePartId);
        //                            if (HasDifferentPartInfo(partIdToTrackIn, item.Value.Name, substrateTypeToString, parentPartId, corePartId, processingStatus, ref description))
        //                            {
        //                                return false;
        //                            }
        //                        }
        //                    }
        //                    break;
        //                default:
        //                    break;
        //            }
        //        }
        //    }


        //    return true;
        //}
        private bool GetSubstrateTypeByAttribute(string attributeValue, ref SubstrateType substrateType)
        {
            return Enum.TryParse(attributeValue, out substrateType);
        }
        private void InitResult(EN_SCENARIO scenario)
        {
            _commandResult.ActionName = scenario.ToString();
            _commandResult.CommandResult = CommandResult.Proceed;
            _commandResult.Description = string.Empty;
        }
        private CommandResults RunScenario(EN_SCENARIO scenario)
        {
            var result = _scenarioOperator.ExecuteScenario(GetTaskName(), scenario);
            _commandResult.ActionName = scenario.ToString();
            switch (result)
            {
                case EN_SCENARIO_RESULT.WAITING:
                case EN_SCENARIO_RESULT.PROCEED:
                    _commandResult.CommandResult = CommandResult.Proceed;
                    break;
                case EN_SCENARIO_RESULT.COMPLETED:
                    {
                        _commandResult.CommandResult = CommandResult.Completed;

                        EN_SCENARIO typeOfScenario = scenario;
                        switch (typeOfScenario)
                        {
                            case ScenarioRecipeDownload:
                                {
                                    bool isManual = IsManual();
                                    if (false == GetWorkingInformation(isManual, ref _workingInfo, ref _temporaryDescription))
                                        break;

                                    if (_workingInfo.LocationType != ModuleType.LoadPort)
                                        break;

                                    if (false == LocationServer.FindLocationById(_workingInfo.LocationId, out var location))
                                        break;

                                    if (!(location is LoadPortLocation lpLocation))
                                        break;

                                    int portId = lpLocation.PortId;
                                    var downloadingStatusString = _carrierServer.GetAttribute(portId, PWA500CarrierAttributes.KeyDownloadingRecipeCompleted);
                                    if (false == string.Equals(downloadingStatusString, bool.TrueString, StringComparison.OrdinalIgnoreCase))
                                    {
                                        _carrierServer.SetAttribute(portId, PWA500CarrierAttributes.KeyDownloadingRecipeCompleted, bool.TrueString);
                                        _carrierServer.SaveCarrierData(portId);
                                    }
                                }
                                break;

                            case ScenarioCoreTrackIn:
                            case ScenarioLotMatch:
                                {
                                    bool isManual = IsManual();
                                    if (false == GetWorkingInformation(isManual, ref _workingInfo, ref _temporaryDescription))
                                        break;

                                    if (_workingInfo.LocationType != ModuleType.LoadPort)
                                        break;

                                    if (false == LocationServer.FindLocationById(_workingInfo.LocationId, out var location))
                                        break;

                                    if (!(location is LoadPortLocation lpLocation))
                                        break;

                                    int portId = lpLocation.PortId;
                                    var trackInStatusString = _carrierServer.GetAttribute(portId, PWA500CarrierAttributes.KeyTrackInCompleted);
                                    if (false == string.Equals(trackInStatusString, bool.TrueString, StringComparison.OrdinalIgnoreCase))
                                    {
                                        _carrierServer.SetAttribute(portId, PWA500CarrierAttributes.KeyTrackInCompleted, bool.TrueString);
                                        _carrierServer.SaveCarrierData(portId);
                                    }

                                    #region <Track in or lot match>
                                    if (false == _substrateManager.GetSubstrateByLocationAndKey(location, string.Empty, out var substrate))
                                        break;

                                    string lotId = substrate.LotId;
                                    string sourceCarrierId = substrate.SourceCarrierId;
                                    _lotHistoryLog.WriteHistoryForTrackIn(portId, sourceCarrierId, typeOfScenario.ToString(), lotId);
                                    #endregion </Track in or lot match>
                                }
                                break;

                            case ScenarioBinWaferIdAssign:
                                {
                                    // Substrate Id 할당된 것을 전달
                                    var resultOfScenario = _scenarioOperator.GetScenarioResultData(GetTaskName(), ScenarioBinWaferIdAssign);
                                    if (false == resultOfScenario.TryGetValue(AssignSubstrateIdKeys.KeyResultSubstrateId,
                                        out _newSubstrateId))
                                    {
                                        _commandResult.CommandResult = CommandResult.Error;
                                        _commandResult.Description = ErrorDescriptionsForMaterialHanding.ErrorDescriptionForAssignSubstrateId;
                                    }
                                }
                                break;

                            case ScenarioBinWorkEnd:
                                {
                                    bool isManual = IsManual();
                                    if (false == GetWorkingInformation(isManual, ref _workingInfo, ref _temporaryDescription))
                                        break;

                                    if (false == _substrateManager.GetSubstrateAtRobot(RobotName, _workingInfo.ActionArm, out var substrate))
                                        break;

                                    int portId = substrate.DestinationPortId;
                                    string substrateName = substrate.Name;
                                    string remainingChips = substrate.GetAttribute(PWA500SubstrateAttributes.ChipQty);
                                    string binCode = substrate.GetAttribute(PWA500SubstrateAttributes.BinCode);

                                    _lotHistoryLog.WriteSubstrateHistoryForBinWorkEnd(portId, substrateName, binCode, remainingChips, substrate.UniqueKey);
                                }
                                break;

                            case ScnearioBinTrackOut:
                                {
                                    bool isManual = IsManual();
                                    if (false == GetWorkingInformation(isManual, ref _workingInfo, ref _temporaryDescription))
                                        break;

                                    if (false == _substrateManager.GetSubstrateAtRobot(RobotName, _workingInfo.ActionArm, out var substrate))
                                        break;

                                    int portId = substrate.DestinationPortId;
                                    string lotId = substrate.LotId;
                                    string substrateName = substrate.Name;
                                    string remainingChips = substrate.GetAttribute(PWA500SubstrateAttributes.ChipQty);
                                    string binCode = substrate.GetAttribute(PWA500SubstrateAttributes.BinCode);

                                    _lotHistoryLog.WriteSubstrateHistoryForBinTrackOut(portId, substrateName, lotId, binCode, remainingChips, substrate.UniqueKey);
                                }
                                break;

                            case ScenarioReqBinPartId:
                                {
                                    var scenarioResult = _scenarioOperator.GetScenarioResultData(GetTaskName(), ScenarioReqBinPartId);
                                    if (false == scenarioResult.TryGetValue(LotInfoKeys.KeyResultPartId,
                                        out _newPartId))
                                    {
                                        _commandResult.CommandResult = CommandResult.Error;
                                        _commandResult.Description = ErrorDescriptionsForMaterialHanding.ErrorDescriptionForRequestPartId;
                                    }

                                    bool isManual = IsManual();
                                    if (false == GetWorkingInformation(isManual, ref _workingInfo, ref _temporaryDescription))
                                        break;

                                    if (false == _substrateManager.GetSubstrateAtRobot(RobotName, _workingInfo.ActionArm, out var substrate))
                                        break;

                                    int portId = substrate.DestinationPortId;
                                    string substrateName = substrate.Name;
                                    string binCode = substrate.GetAttribute(PWA500SubstrateAttributes.BinCode);
                                    string partId = substrate.GetAttribute(PWA500SubstrateAttributes.PartId);
                                    _lotHistoryLog.WriteSubstrateHistoryForReqBinPartId(portId, substrateName, binCode, partId, _newPartId, substrate.UniqueKey);
                                }
                                break;
                            case ScenarioUploadBinData:
                                {
                                    _binDataToUpload = null;
                                    _functionsForPWA500.ClearBinDataToUpload();

                                    bool isManual = IsManual();
                                    if (false == GetWorkingInformation(isManual, ref _workingInfo, ref _temporaryDescription))
                                        break;

                                    if (false == _substrateManager.GetSubstrateAtRobot(RobotName, _workingInfo.ActionArm, out var substrate))
                                        break;

                                    int portId = substrate.DestinationPortId;
                                    string substrateName = substrate.Name;

                                    _lotHistoryLog.WriteSubstrateHistoryForUploadBinData(portId, substrateName, _functionsForPWA500.PmsFullPath, substrate.UniqueKey);
                                }
                                break;

                            default:
                                break;
                        }
                    }
                    break;
                case EN_SCENARIO_RESULT.ERROR:
                    {
                        if (IsSimulation())
                        {
                            EN_SCENARIO typeOfScenario = scenario;
                            switch (typeOfScenario)
                            {
                                case ScenarioSendClientToBinWaferIdAssign:
                                    {
                                        _commandResult.CommandResult = CommandResult.Completed;
                                    }
                                    return _commandResult;

                                case ScenarioSendClientUploadBinFile:
                                    {
                                        _newPartId = "TEST_PART_ID";
                                        _commandResult.CommandResult = CommandResult.Completed;
                                    }
                                    return _commandResult;
                            }
                        }
                        _commandResult.CommandResult = CommandResult.Error;
                        _commandResult.Description = _commandResult.ActionName;
                    }
                    break;
                case EN_SCENARIO_RESULT.TIMEOUT_ERROR:
                    _commandResult.CommandResult = CommandResult.Timeout;
                    _commandResult.Description = _commandResult.ActionName;
                    break;
                default:
                    break;
            }

            return _commandResult;
        }
        private int GetUnloadingStep(ref Substrate substrate)
        {
            var step = substrate.GetAttribute(PWA500SubstrateAttributes.BinUnloadingStep);
            if (false == EFEM.MaterialTracking.EnumPersistence.TryParseName<UnloadingStepTypesFor500W>(step, out var unloadingStep))
            {
                // 파싱 불가면 Init으로 고정
                unloadingStep = UnloadingStepTypesFor500W.Init;
                _substrateManager.SetAttributeByKey(substrate.UniqueKey, PWA500SubstrateAttributes.BinUnloadingStep, unloadingStep.ToString());
            }

            return (int)unloadingStep;
        }
        private bool HasSameSourceSubstrate(Substrate substrate, string sourceCarrierId, int portId, string key)
        {
            return string.Equals(substrate.SourceCarrierId, sourceCarrierId, StringComparison.OrdinalIgnoreCase) &&
                substrate.SourcePortId == portId && false == string.Equals(substrate.UniqueKey, key, StringComparison.OrdinalIgnoreCase);
        }

        private bool IsFirstSubstrateAtLoadPort(string sourceCarrierId, int portId, string key)
        {
            int count = _loadPortManager.Count;
            for (int i = 0; i < count; ++i)
            {
                if (false == _loadPortManager.IsLoadPortEnabled(i))
                    continue;

                int lpPort = _loadPortManager.GetLoadPortPortId(i);

                var subs = _substrateManager.GetSubstratesAtLoadPort(lpPort);
                foreach (var item in subs)
                {
                    if (lpPort == portId)
                    {
                        if (item.Value.TransportStatus == TransportStates.AtDestination)
                            return false;
                    }
                    else
                    {
                        if (HasSameSourceSubstrate(item.Value, sourceCarrierId, portId, key))
                            return false;
                    }
                }
            }

            var pmName = _processGroup.GetProcessModuleName(ProcessModuleIndex);
            List<Substrate> subsAtPm = new List<Substrate>();
            if (_substrateManager.GetSubstratesAtProcessModule(pmName, ref subsAtPm))
            {
                foreach (var item in subsAtPm)
                {
                    if (HasSameSourceSubstrate(item, sourceCarrierId, portId, key))
                        return false;
                }
            }

            var subsAtRb = new Dictionary<RobotArmTypes, Substrate>();
            if (_substrateManager.GetSubstratesAtRobotAll(RobotName, ref subsAtRb))
            {
                foreach (var item in subsAtRb)
                {
                    if (HasSameSourceSubstrate(item.Value, sourceCarrierId, portId, key))
                        return false;
                }
            }

            return true;
        }
        private bool IsLastSubstrateAtLoadPortBeforePick(int portId, string key)
        {
            var subs = _substrateManager.GetSubstratesAtLoadPort(portId);
            if (subs.Count == 1)
                return true;

            foreach (var item in subs)
            {
                if (string.Equals(item.Value.UniqueKey, key, StringComparison.OrdinalIgnoreCase))
                    continue;

                // 다른놈이 있는거다.
                if (item.Value.TransportStatus != TransportStates.AtDestination)
                    return false;
            }

            return true;
        }
        #endregion </Scenario>

        #region <Material Handling Interface>
        private bool GetSubstrateNameByKey(string key, out string name)
        {
            name = null;
            if (false == _substrateManager.GetSubstrateByKey(key, out var s) || s == null)
                return false;

            name = s.Name;
            return true;
        }
        private bool GetSubstrateNameByKeyForUnloading(string key, out string name)
        {
            name = null;

            var pm = _processGroup.GetProcessModuleName(ProcessModuleIndex);
            List<Substrate> substrates = new List<Substrate>();
            if (false == _substrateManager.GetSubstratesAtProcessModule(pm, ref substrates))
                return false;

            foreach (var item in substrates)
            {
                if (item.UniqueKey == key)
                {
                    name = item.Name;
                    return true;
                }
            }

            return false;
        }
        private bool TryFindValidPortId(SubstrateType targetType, SubstrateSize targetSize)
        {
            SubstrateType curType = SubstrateType.Core;
            SubstrateSize curSize = SubstrateSize.Inch_8;
            for (int i = 0; i < _loadPortManager.Count; ++i)
            {
                // 1. Enable 상태인지 검사
                if (false == _loadPortManager.IsLoadPortEnabled(i))
                    continue;

                // 2. 포트타입이 타겟포트와 같은지 검사
                if (false == _functionsForPWA500.GetSubstrateSpecByLoadPortIndex(i, ref curType, ref curSize))
                    continue;
                if (curType != targetType ||
                    curSize != targetSize)
                    continue;

                // 3. 캐리어가 있는지 여부 반환
                var port = _loadPortManager.GetLoadPortPortId(i);
                if (_carrierServer.HasCarrier(port))
                    return true;
            }

            return false;
        }
        private bool GetNextSlotInformationToPlace(int lpIndex, ref int slot)
        {
            int portId = _loadPortManager.GetLoadPortPortId(lpIndex);
            if (false == _carrierServer.HasCarrier(portId))
                return false;

            //if (false == _substrateManager.HasAnySubstrateInLoadPort(portId))
            //    return false;

            slot = -1;
            bool notAvailableSlotFirst = (_loadPortManager.GetCarrierLoadingType(lpIndex) == LoadPortLoadingMode.Cassette || _loadPortManager.GetCarrierLoadingType(lpIndex) == LoadPortLoadingMode.ClosedCassette);
            int capacity = _carrierServer.GetCapacity(portId);
            for (int i = 1; i <= capacity; ++i)
            {
                if (notAvailableSlotFirst && i == 1)
                    continue;

                if (false == _substrateManager.HasSubstrateAtLoadPort(portId, i))
                {
                    slot = i;
                    break;
                }
            }

            return (slot >= 0);
        }
        private void InitSubStepFlag()
        {
            _subStepInterface = 0;
        }
        private CommandResults ReturnToError(CommandResult result, EN_ALARM alarmCode, string methodName, int step, string description)
        {
            InitSubStepFlag();

            _result.ActionName = methodName;
            _result.AlarmCode = (int)alarmCode;
            _result.CommandResult = CommandResult.Error;
            _result.Description = description;

            return _result;
        }
        private CommandResults ReturnSkipped(string methodName)
        {
            InitSubStepFlag();

            _result.ActionName = methodName;
            _result.CommandResult = CommandResult.Skipped;
            _result.Description = string.Empty;

            return _result;
        }
        private CommandResults ReturnProceed()
        {
            //InitSubStepFlag();

            _result.ActionName = string.Empty;
            _result.CommandResult = CommandResult.Proceed;
            _result.Description = string.Empty;

            return _result;
        }
        private CommandResults ReturnCompleted()
        {
            InitSubStepFlag();

            _result.ActionName = string.Empty;
            _result.CommandResult = CommandResult.Completed;
            _result.Description = string.Empty;

            return _result;
        }
        private bool IsLoadingSignalStillActive(string location)
        {
            // TODO : 시뮬용 리턴
            //if (Work.AppConfigManager.Instance.ControllerDigital == Define.DefineEnumProject.AppConfig.EN_DIGITAL_IO_CONTROLLER.NONE)
            //{
            //    return true;
            //}

            //  1) PM의 스메마 확인 후 Off면 Skipped 리턴
            List<string> requestedLocation = new List<string>();
            if (false == _processGroup.IsLoadingRequested(ProcessModuleIndex, ref requestedLocation))
                return false;

            if (false == requestedLocation.Contains(location))
                return false;

            return true;
        }

        #endregion </Material Handling Interface>

        #endregion </Internals>

        #endregion </Methods>
    }

    class TaskAtmRobotRecovery500W_300 : Work.RecoveryData
    {
        public TaskAtmRobotRecovery500W_300(string taskName, int nPortCount)
            : base(taskName, nPortCount)
        {
        }

        protected override void LoadData(ref FileComposite_.FileComposite fComp, string sRootName)
        {
        }
        protected override void SaveData(ref FileComposite_.FileComposite fComp, string sRootName)
        {
        }
    }
}
