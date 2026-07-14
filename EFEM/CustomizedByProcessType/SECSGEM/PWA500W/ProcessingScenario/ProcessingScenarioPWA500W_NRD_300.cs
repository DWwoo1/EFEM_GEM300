using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Diagnostics;

using FrameOfSystem3.Recipe;
using FrameOfSystem3.SECSGEM.DefineSecsGem;
using FrameOfSystem3.SECSGEM.Scenario.Common.Auto;
using FrameOfSystem3.Functional;

using AnalogIO_;
using WCFManager_;

using Define.DefineEnumProject.Mail;
using Define.DefineConstant;
using Define.DefineEnumProject.AppConfig;
using EFEM.Modules;
using EFEM.MaterialTracking;
using EFEM.Defines.MaterialTracking;
using EFEM.CustomizedByProcessType.PWA500W;
using EFEM.CustomizedByProcessType.PWA500Common;

using FrameOfSystem3.ExternalDevice.Serial.FanFilterUnit;
using FrameOfSystem3.SECSGEM.Trace;

namespace FrameOfSystem3.SECSGEM.Scenario
{
    public class ProcessingScenarioPWA500W_NRD_300 : ProcessingScenario
    {
        public ProcessingScenarioPWA500W_NRD_300()
        {
            _postOffice = PostOffice.GetInstance();
            _postOffice.RequestSubscribe(EN_SUBSCRIBER.ProcessingScenario);

            _loadPortManager = LoadPortManager.Instance;
            _robotManager = AtmRobotManager.Instance;
            _processModuleGroup = ProcessModuleGroup.Instance;
            _carrierServer = CarrierManagementServer.Instance;
            _substrateManager = SubstrateManager.Instance;

            _wcfManager = WCFManager.GetInstance();
            _analogIO = AnalogIO.GetInstance();
            _ffuManager = FanFilterUnitManager.Instance;


            var commons = Enum.GetNames(typeof(PARAM_COMMON)).ToList();
            for(int i = 0; i < commons.Count; ++i)
            {
                var k = commons[i];
                if (k == PARAM_COMMON.PROCESS_FILE_PATH.ToString() ||
                    k == PARAM_COMMON.PROCESS_FILE_NAME.ToString())
                    continue;
                
                var v = _recipe.GetValue(EN_RECIPE_TYPE.COMMON, k, string.Empty);

                _myEcidListForCommon[k] = v;
            }

            var equipments = Enum.GetNames(typeof(PARAM_EQUIPMENT)).ToList();
            for (int i = 0; i < equipments.Count; ++i)
            {
                var k = equipments[i];
                if (k == PARAM_EQUIPMENT.MachineLanguage.ToString() ||
                    k == PARAM_EQUIPMENT.MachineName.ToString() ||
                    k == PARAM_EQUIPMENT.UnlockParameterChange.ToString() ||
                    k == PARAM_EQUIPMENT.RAM_METRICS_EXPORT_PATH.ToString())
                    continue;


                var v = _recipe.GetValue(EN_RECIPE_TYPE.EQUIPMENT, k, string.Empty);

                _myEcidListForEquipment[k] = v;
            }

            _functionsForPWA500 = FunctionsForPWA500W_NRD_300.Instance;
            //_functionsForPWA500.AssignFunctionToSendClientMessage(SendClientToClientMessage);
            //_functionsForPWA500.AssignActionToEnqueueScenarioAsync(EnqueueScenario);
            //_functionsForPWA500.AssignFunctionToUpdateParam(UpdateScenarioParams);
            //_functionsForPWA500.AssignFunctionToExecuteScenario(ExecuteScenario);

            _lotHistoryLog = LotHistoryLog.Instance;
        }

        WCFManager _wcfManager;
        PostOffice _postOffice = null;
        private const string IsResponseMessage = "Request";
        private const string NameOfClient = "MAIN";
        private const string RecipePathForPM = @"\\192.168.100.200\Recipe\RMS";
        //private const string RecipePathForPM = @"\\127.0.0.1\bp5000ld\RMS";
        private readonly int ProcessModuleIndex = 0;
        //private readonly int RobotIndex = 0;

        private static LoadPortManager _loadPortManager;
        private static AtmRobotManager _robotManager;
        private static SubstrateManager _substrateManager;
        private static CarrierManagementServer _carrierServer;
        private static ProcessModuleGroup _processModuleGroup;
        private static AnalogIO _analogIO = null;
        private static FanFilterUnitManager _ffuManager = null;

        private static FunctionsForPWA500W_NRD_300 _functionsForPWA500 = null;

        // TODO : AutoScenario
        //private readonly Dictionary<string, string> ClientInfos;
        //private QueuedScenarioInfo _executingScenarioInfo = null;
        //private readonly ConcurrentQueue<QueuedScenarioInfo> _queuedScenario = new ConcurrentQueue<QueuedScenarioInfo>();
        private const string NameOfPM = "PWA500W";
        
        //private const string _recipeBasePathToDownload = @"\\192.168.100.200\Shared\Download";
        //private const string _recipeBasePathToDownload = @"\\192.168.100.150\EFEM\RMS\PWA500W\Download";

        private string _recipePathToUploadForPM = string.Empty;
        //private readonly Dictionary<string, string> RecipePath = null;

        private PWA500WNRDTraceDataProvider _traceDataProvider = null;
        private ITraceRecoveryStore _traceRecoveryStore = null;

        //private SharedFolderAccess _sharedFolderForAccess = null;
        //private const string AccessAccount = "protec";
        //private const string AccessPassword = "1";
        private string _accessIpAddress = string.Empty;
        private const int AlarmOffset = 2000000;

        private readonly ConcurrentQueue<int> _queuedAlarmsFromPM = new ConcurrentQueue<int>();
        
        // TODO : 나중에 올려야함
        Dictionary<string, string> _ecidToUpdate = new Dictionary<string, string>();
        private static LotHistoryLog _lotHistoryLog = null;
        
        private const long StatusVariableIdForESDAtStage = 1803;

        private Dictionary<string, string> _finishPickingParams = new Dictionary<string, string>();
        private Dictionary<string, string> _finishPlacingParams = new Dictionary<string, string>();

        private ConcurrentDictionary<string, string> _myEcidListForCommon = new ConcurrentDictionary<string, string>();
        private ConcurrentDictionary<string, string> _myEcidListForEquipment = new ConcurrentDictionary<string, string>();

        // 2026.07.09 dwlim 통신로그 제출 위한 임시변수 (다 쓰고 지우자)
        private bool _isReceived = false;
        private string _recipeBody = string.Empty;
        
        #region interface

        #region <Init, Exit>
        public override bool Init(string recipePath, string configPath, Dictionary<string, StatusVariable> statusVariableList, Dictionary<long, List<StatusVariable>> reportList, Dictionary<string, CollectionEvent> collectionEventList)
        {
            var result = base.Init(recipePath, configPath, statusVariableList, reportList, collectionEventList);

            int clientIndex = 4;
            if (Work.AppConfigManager.Instance.ProcessModuleSimulation)
            {
                clientIndex = 9;
            }

            int[] indexOfClients = new int[1];
            if (_wcfManager.GetListofClientItems(ref indexOfClients))
            {
                for (int i = 0; i < indexOfClients.Length; ++i)
                {
                    if (false == i.Equals(clientIndex))
                        continue;

                    string deviceName = string.Empty;
                    int indexOfItem = indexOfClients[i];
                    if (false == _wcfManager.GetParameter(indexOfItem, ParameterTypeForClient.Name, ref deviceName))
                        continue;

                    string accessIp = string.Empty;
                    if (false == _wcfManager.GetParameter(indexOfItem, ParameterTypeForClient.TargetServiceIP, ref accessIp))
                        continue;

                    _accessIpAddress = string.Format(@"\\{0}", accessIp);
                }
            }

            if (_traceDataProvider != null)
            {
                Dictionary<long, string> snapshot;
                if (_traceDataProvider.TryGetSnapshot(out snapshot))
                {
                    UpdateVariable(snapshot.Keys.ToArray(), snapshot.Values.ToArray());
                }
            }

            return result;
        }
        public override void Exit()
        {
            //System.Threading.Tasks.Task.Run(() => _sharedFolderForAccess.DisconnectFromSharedFolder());
            base.Exit();
        }
        #endregion </Init, Exit>

        #region <Properties>
        private bool UseCoreMapHandlingOnly
        {
            get
            {
                return false;// (false == _recipe.GetValue(EN_RECIPE_TYPE.COMMON, PARAM_COMMON.UseSecsGem.ToString(), false));
            }
        }
        #endregion </Properties>

        #region config
        protected override void MakeCustomScenario()
        {
            foreach (EN_SCENARIO scenario in Enum.GetValues(typeof(EN_SCENARIO)))
            {
                if (ScenarioList.ContainsKey(scenario))
                    continue;

                switch (scenario)
                {
                    case EN_SCENARIO.SCENARIO_REQ_LOT_INFO_CORE_1:
                    case EN_SCENARIO.SCENARIO_REQ_LOT_INFO_CORE_2:
                    case EN_SCENARIO.SCENARIO_REQ_LOT_INFO_CORE_3:
                    case EN_SCENARIO.SCENARIO_REQ_LOT_INFO_EMPTY_TAPE:
                    case EN_SCENARIO.SCENARIO_PORT_STATUS_LOAD_1:
                    case EN_SCENARIO.SCENARIO_PORT_STATUS_LOAD_2:
                    case EN_SCENARIO.SCENARIO_PORT_STATUS_LOAD_3:
                    case EN_SCENARIO.SCENARIO_PORT_STATUS_LOAD_4:
                    case EN_SCENARIO.SCENARIO_PORT_STATUS_LOAD_5:
                    case EN_SCENARIO.SCENARIO_PORT_STATUS_LOAD_6:
                    case EN_SCENARIO.SCENARIO_PORT_STATUS_UNLOAD_1:
                    case EN_SCENARIO.SCENARIO_PORT_STATUS_UNLOAD_2:
                    case EN_SCENARIO.SCENARIO_PORT_STATUS_UNLOAD_3:
                    case EN_SCENARIO.SCENARIO_PORT_STATUS_UNLOAD_4:
                    case EN_SCENARIO.SCENARIO_PORT_STATUS_UNLOAD_5:
                    case EN_SCENARIO.SCENARIO_PORT_STATUS_UNLOAD_6:
                    case EN_SCENARIO.SCENARIO_CARRIER_LOAD:
                    case EN_SCENARIO.SCENARIO_CARRIER_UNLOAD:
                    case EN_SCENARIO.SCENARIO_RFID_READ_CORE_1:
                    case EN_SCENARIO.SCENARIO_RFID_READ_CORE_2:
                    case EN_SCENARIO.SCENARIO_RFID_READ_CORE_3:
                    case EN_SCENARIO.SCENARIO_RFID_READ_EMPTY_TAPE:
                    case EN_SCENARIO.SCENARIO_RFID_READ_BIN_1:
                    case EN_SCENARIO.SCENARIO_RFID_READ_BIN_2:
                    case EN_SCENARIO.SCENARIO_RFID_READ_BIN_3:
                    case EN_SCENARIO.SCENARIO_REQ_SLOT_INFO_CORE_1:
                    case EN_SCENARIO.SCENARIO_REQ_SLOT_INFO_CORE_2:
                    case EN_SCENARIO.SCENARIO_REQ_SLOT_INFO_CORE_3:
                    case EN_SCENARIO.SCENARIO_REQ_SLOT_INFO_EMPTY_TAPE:
                    //case EN_SCENARIO.SCENARIO_REQ_RECIPE_DOWNLOAD:
                    //case EN_SCENARIO.SCENARIO_REQ_RECIPE_UPLOAD:
                    case EN_SCENARIO.SCENARIO_REQ_TRACK_IN:
                    case EN_SCENARIO.SCENARIO_REQ_LOT_MATCH:
                    case EN_SCENARIO.SCENARIO_REQ_BIN_WAFER_TRACK_OUT:
                    case EN_SCENARIO.SCENARIO_SLOT_WAFER_MAPPING_CORE_1:
                    case EN_SCENARIO.SCENARIO_SLOT_WAFER_MAPPING_CORE_2:
                    case EN_SCENARIO.SCENARIO_SLOT_WAFER_MAPPING_CORE_3:
                    case EN_SCENARIO.SCENARIO_SLOT_WAFER_MAPPING_EMPTY_TAPE:
                    // TODO : 빈소터와의 운영상 차이점 -> W는 없다.
                    case EN_SCENARIO.SCENARIO_REQ_CORE_WAFER_TRACK_OUT:
                    case EN_SCENARIO.SCENARIO_SLOT_WAFER_MAPPING_BIN_1:
                    case EN_SCENARIO.SCENARIO_SLOT_WAFER_MAPPING_BIN_2:
                    case EN_SCENARIO.SCENARIO_SLOT_WAFER_MAPPING_BIN_3:
                    // TODO : 빈소터와의 운영상 차이점 -> W는 없다.
                    case EN_SCENARIO.SCENARIO_REQ_LOT_MERGE_CORE_1:
                    case EN_SCENARIO.SCENARIO_REQ_LOT_MERGE_CORE_2:
                    case EN_SCENARIO.SCENARIO_REQ_LOT_MERGE_CORE_3:
                    case EN_SCENARIO.SCENARIO_REQ_LOT_ID_MERGE_AND_CHANGE_BIN_1:
                    case EN_SCENARIO.SCENARIO_REQ_LOT_ID_MERGE_AND_CHANGE_BIN_2:
                    case EN_SCENARIO.SCENARIO_REQ_LOT_ID_MERGE_AND_CHANGE_BIN_3:
                    case EN_SCENARIO.SCENARIO_WORK_START:
                    case EN_SCENARIO.SCENARIO_WORK_END:
                    case EN_SCENARIO.SCENARIO_REQ_CORE_WAFER_SPLIT:
                    case EN_SCENARIO.SCENARIO_REQ_CORE_WAFER_SPLIT_LAST:
                    case EN_SCENARIO.SCENARIO_REQ_CORE_CHIP_SPLIT_FIRST:
                    case EN_SCENARIO.SCENARIO_REQ_CORE_CHIP_SPLIT:
                    case EN_SCENARIO.SCENARIO_REQ_CORE_CHIP_FULL_SPLIT_FIRST:
                    case EN_SCENARIO.SCENARIO_REQ_CORE_CHIP_FULL_SPLIT:
                    case EN_SCENARIO.SCENARIO_REQ_CORE_CHIP_MERGE:
                    case EN_SCENARIO.SCENARIO_ADS_MOVE_FLAG_1:
                    case EN_SCENARIO.SCENARIO_ADS_MOVE_FLAG_2:
                    case EN_SCENARIO.SCENARIO_SCRAP_BIN_CHIP:
                    case EN_SCENARIO.SCENARIO_SCRAP_CORE_CHIP:
                    case EN_SCENARIO.SCENARIO_UPLOAD_WORK_RESULT:
                    case EN_SCENARIO.SCENARIO_BIN_WORK_END:
                    case EN_SCENARIO.SCENARIO_BIN_PART_ID_INFO_REQ:
                    case EN_SCENARIO.SCENARIO_REQ_BIN_WAFER_ID_ASSIGN:
                    // TODO : 빈소터와의 운영상 차이점 -> 현재 ID가 없다고 한다. 나중에 사용한다고는 했지만, 서버에도 적용되어 있지 않다.
                    case EN_SCENARIO.SCENARIO_REQ_CORE_WAFER_ID:
                    case EN_SCENARIO.SCENARIO_BIN_SORTING_START_2:
                    case EN_SCENARIO.SCENARIO_BIN_SORTING_END_2:
                    case EN_SCENARIO.SCENARIO_BIN_SORTING_START_3:
                    case EN_SCENARIO.SCENARIO_BIN_SORTING_END_3:
                    // 아래는 미구현
                    case EN_SCENARIO.SCENARIO_REQ_COLLET_CHANGE_1:
                    case EN_SCENARIO.SCENARIO_REQ_COLLET_CHANGE_2:
                    case EN_SCENARIO.SCENARIO_REQ_HOOD_CHANGE:
                        break;

                    case EN_SCENARIO.SCENARIO_EQUIPMENT_START:
                        {
                            MakeScenario(scenario, new SendingEventScenario(scenario.ToString(),
                                CollectionEventList[EN_EVENT_LIST.EQUIPMENT_START.ToString()].Id,
                                CollectionEventList[EN_EVENT_LIST.EQUIPMENT_START.ToString()].VariableIds,
                                false, 10000, false));
                        }
                        break;
                    case EN_SCENARIO.SCENARIO_EQUIPMENT_END:
                        {
                            MakeScenario(scenario, new SendingEventScenario(scenario.ToString(),
                                CollectionEventList[EN_EVENT_LIST.EQUIPMENT_END.ToString()].Id,
                                CollectionEventList[EN_EVENT_LIST.EQUIPMENT_END.ToString()].VariableIds,
                                false, 10000, false));
                        }
                        break;
                    case EN_SCENARIO.SCENARIO_PROCESS_START:
                        {
                            MakeScenario(scenario, new SendingEventScenario(scenario.ToString(),
                                CollectionEventList[EN_EVENT_LIST.PROCESS_START.ToString()].Id,
                                CollectionEventList[EN_EVENT_LIST.PROCESS_START.ToString()].VariableIds,
                                false, 10000, false));
                        }
                        break;
                    case EN_SCENARIO.SCENARIO_PROCESS_END:
                        {
                            MakeScenario(scenario, new SendingEventScenario(scenario.ToString(),
                                CollectionEventList[EN_EVENT_LIST.PROCESS_END.ToString()].Id,
                                CollectionEventList[EN_EVENT_LIST.PROCESS_END.ToString()].VariableIds,
                                false, 10000, false));
                        }
                        break;
                    case EN_SCENARIO.SCENARIO_ERROR_START:
                        {
                            MakeScenario(scenario, new SendingEventScenario(scenario.ToString(),
                                CollectionEventList[EN_EVENT_LIST.ERROR_START.ToString()].Id,
                                CollectionEventList[EN_EVENT_LIST.ERROR_START.ToString()].VariableIds,
                                false, 10000, false));
                        }
                        break;
                    case EN_SCENARIO.SCENARIO_ERROR_STOP:
                        {
                            MakeScenario(scenario, new SendingEventScenario(scenario.ToString(),
                                CollectionEventList[EN_EVENT_LIST.ERROR_STOP.ToString()].Id,
                                CollectionEventList[EN_EVENT_LIST.ERROR_STOP.ToString()].VariableIds,
                                false, 10000, false));
                        }
                        break;
                    case EN_SCENARIO.SCENARIO_LOT_START:
                        {
                            MakeScenario(scenario, new SendingEventScenario(scenario.ToString(),
                                CollectionEventList[EN_EVENT_LIST.LOT_START.ToString()].Id,
                                CollectionEventList[EN_EVENT_LIST.LOT_START.ToString()].VariableIds,
                                false, 10000, false));
                        }
                        break;
                    case EN_SCENARIO.SCENARIO_LOT_END:
                        {
                            MakeScenario(scenario, new SendingEventScenario(scenario.ToString(),
                                CollectionEventList[EN_EVENT_LIST.LOT_END.ToString()].Id,
                                CollectionEventList[EN_EVENT_LIST.LOT_END.ToString()].VariableIds,
                                false, 10000, false));
                        }
                        break;
                    case EN_SCENARIO.SCENARIO_WAFER_START:
                        {
                            MakeScenario(scenario, new SendingEventScenario(scenario.ToString(),
                                CollectionEventList[EN_EVENT_LIST.WAFER_START.ToString()].Id,
                                CollectionEventList[EN_EVENT_LIST.WAFER_START.ToString()].VariableIds,
                                false, 10000, false));
                        }
                        break;
                    case EN_SCENARIO.SCENARIO_WAFER_END:
                        {
                            MakeScenario(scenario, new SendingEventScenario(scenario.ToString(),
                                CollectionEventList[EN_EVENT_LIST.WAFER_END.ToString()].Id,
                                CollectionEventList[EN_EVENT_LIST.WAFER_END.ToString()].VariableIds,
                                false, 10000, false));
                        }
                        break;
                    case EN_SCENARIO.SCENARIO_CHAMBER_START:
                        {
                            MakeScenario(scenario, new SendingEventScenario(scenario.ToString(),
                                CollectionEventList[EN_EVENT_LIST.CHAMBER_START.ToString()].Id,
                                CollectionEventList[EN_EVENT_LIST.CHAMBER_START.ToString()].VariableIds,
                                false, 10000, false));
                        }
                        break;
                    case EN_SCENARIO.SCENARIO_CHAMBER_END:
                        {
                            MakeScenario(scenario, new SendingEventScenario(scenario.ToString(),
                                CollectionEventList[EN_EVENT_LIST.CHAMBER_END.ToString()].Id,
                                CollectionEventList[EN_EVENT_LIST.CHAMBER_END.ToString()].VariableIds,
                                false, 10000, false));
                        }
                        break;
                    case EN_SCENARIO.SCENARIO_CORE_MAP_DOWNLOAD:
                        {
                            MakeScenario(scenario, new ScenarioReqWaferMapDownload(scenario.ToString(),
                                14,
                                1,
                                false));
                        }
                        break;
                    case EN_SCENARIO.SCENARIO_CORE_MAP_UPLOAD:
                        {
                            MakeScenario(scenario, new SendingEventScenario(
                                scenario.ToString(),
                                CollectionEventList[EN_EVENT_LIST.CORE_MAP_UPLOAD.ToString()].Id,
                                CollectionEventList[EN_EVENT_LIST.CORE_MAP_UPLOAD.ToString()].VariableIds,
                                false, 10000, false));
                        }
                        break;
                    case EN_SCENARIO.SCENARIO_STEP_START:
                        {
                            MakeScenario(scenario, new SendingEventScenario(scenario.ToString(),
                                CollectionEventList[EN_EVENT_LIST.STEP_START.ToString()].Id,
                                CollectionEventList[EN_EVENT_LIST.STEP_START.ToString()].VariableIds,
                                false, 10000, false));
                        }
                        break;
                    case EN_SCENARIO.SCENARIO_STEP_END:
                        {
                            MakeScenario(scenario, new SendingEventScenario(scenario.ToString(),
                                CollectionEventList[EN_EVENT_LIST.STEP_END.ToString()].Id,
                                CollectionEventList[EN_EVENT_LIST.STEP_END.ToString()].VariableIds,
                                false, 10000, false));
                        }
                        break;
                    case EN_SCENARIO.SCENARIO_CORE_WAFER_DETACH_START:
                        {
                            MakeScenario(scenario, new SendingEventScenario(scenario.ToString(),
                                CollectionEventList[EN_EVENT_LIST.CORE_WAFER_DETACH_START.ToString()].Id,
                                CollectionEventList[EN_EVENT_LIST.CORE_WAFER_DETACH_START.ToString()].VariableIds,
                                false, 10000, false));
                        }
                        break;
                    case EN_SCENARIO.SCENARIO_CORE_WAFER_DETACH_END:
                        {
                            MakeScenario(scenario, new SendingEventScenario(scenario.ToString(),
                                CollectionEventList[EN_EVENT_LIST.CORE_WAFER_DETACH_END.ToString()].Id,
                                CollectionEventList[EN_EVENT_LIST.CORE_WAFER_DETACH_END.ToString()].VariableIds,
                                false, 10000, false));
                        }
                        break;
                    case EN_SCENARIO.SCENARIO_BIN_WAFER_ID_READ:
                        {
                            MakeScenario(scenario, new SendingEventScenario(scenario.ToString(),
                                CollectionEventList[EN_EVENT_LIST.BIN_WAFER_RING_ID_READ.ToString()].Id,
                                CollectionEventList[EN_EVENT_LIST.BIN_WAFER_RING_ID_READ.ToString()].VariableIds,
                                false, 10000, false));
                        }
                        break;
                    case EN_SCENARIO.SCENARIO_BIN_MAP_UPLOAD:
                        {
                            // Map and PMS Upload
                            MakeScenario(scenario, new SendingEventScenario(
                                scenario.ToString(),
                                CollectionEventList[EN_EVENT_LIST.BIN_MAP_UPLOAD.ToString()].Id,
                                CollectionEventList[EN_EVENT_LIST.BIN_MAP_UPLOAD.ToString()].VariableIds,
                                false, 10000, false));
                        }
                        break;
                    case EN_SCENARIO.SCENARIO_BIN_SORTING_START_1:
                        {
                            MakeScenario(scenario, new SendingEventScenario(scenario.ToString(),
                                CollectionEventList[EN_EVENT_LIST.BIN_SORTING_START.ToString()].Id,
                                CollectionEventList[EN_EVENT_LIST.BIN_SORTING_START.ToString()].VariableIds,
                                false, 10000, false));
                        }
                        break;
                    case EN_SCENARIO.SCENARIO_BIN_SORTING_END_1:
                        {
                            MakeScenario(scenario, new SendingEventScenario(scenario.ToString(),
                                CollectionEventList[EN_EVENT_LIST.BIN_SORTING_END.ToString()].Id,
                                CollectionEventList[EN_EVENT_LIST.BIN_SORTING_END.ToString()].VariableIds,
                                false, 10000, false));
                        }
                        break;
                    case EN_SCENARIO.SCENARIO_REQ_UPLOAD_BINFILE:
                        {
                            MakeScenario(scenario, new ScenarioReqUploadBinFile(scenario.ToString()));
                        }
                        break;
                    case EN_SCENARIO.SCENARIO_BIN_DATA_UPLOAD:
                        {
                            MakeScenario(scenario, new ScenarioUploadBinDataGEM300(
                                scenario.ToString(),
                                CollectionEventList[EN_EVENT_LIST.BIN_DATA_UPLOAD.ToString()].Id,
                                CollectionEventList[EN_EVENT_LIST.BIN_DATA_UPLOAD.ToString()].VariableIds,
                                StatusVariableList[EN_SVID_LIST.PMS_FILEBODY.ToString()].Id,
                                10000));
                        }
                        break;
                    case EN_SCENARIO.SCENARIO_ASSIGN_SUBSTRATE_ID:
                        {
                            MakeScenario(scenario, new ClientToClientCommunicationScenario(scenario.ToString()));
                        }
                        break;
                    case EN_SCENARIO.SCENARIO_BIN_WAFER_END:
                        {
                            // 2026.06.24 dwlim [MOD] VID 추가 및 Sorting_Info의 Format 변경으로 수정
                            MakeScenario(scenario, new ScenarioBinWaferEndForGEM300(scenario.ToString(),
                                CollectionEventList[EN_EVENT_LIST.BIN_WAFER_END.ToString()].Id,
                                CollectionEventList[EN_EVENT_LIST.BIN_WAFER_END.ToString()].VariableIds,
                                StatusVariableList[EN_SVID_LIST.SORTING_INFO.ToString()].Id,
                                10000));
                        }
                        break;
                    case EN_SCENARIO.SCENARIO_RECIPE_DOWNLOAD_BY_HOST:
                    case EN_SCENARIO.SCENARIO_RECIPE_UPLOAD_BY_HOST:
                        {
                            bool isUpload = scenario == EN_SCENARIO.SCENARIO_RECIPE_UPLOAD_BY_HOST;
                            int functionToSend = isUpload ? 23 : 25 ;
                            MakeScenario(scenario, new ScenarioFormattedRecipeHandlingByHost(scenario.ToString(),
                                7,
                                functionToSend,
                                isUpload));
                        }
                        break;

                    default:
                        break;
                }
            }
        }

        protected override ITraceDataProvider CreateTraceDataProvider()
        {
            _traceDataProvider = new PWA500WNRDTraceDataProvider(
                _analogIO,
                _ffuManager);

            return _traceDataProvider;
        }

        protected override ITraceRecoveryStore CreateTraceRecoveryStore()
        {
            _traceRecoveryStore = new IniTraceRecoveryStore();
            return _traceRecoveryStore;
        }

        protected override void MakeScenarioByConfigFiles(string configPath)
        {
        }

        public override List<string> GetScenarioParameterList(EN_SCENARIO scenario)
        {
            if (false == ScenarioList.TryGetValue(scenario, out ScenarioBaseClass scen))
                return null;

            List<string> parameterList = null;
            if (scen is SendingEventScenario)
            {
                var convertedScen = scen as SendingEventScenario;

                foreach (var item in CollectionEventList)
                {
                    if (item.Value.Id.Equals(convertedScen.EventId))
                    {
                        parameterList = new List<string>();
                        foreach (var kvp in item.Value.Variables)
                        {
                            parameterList.Add(kvp.Value.Name);
                        }
                        break;
                    }
                }
            }
            else if (scen is ScenarioReqLotInfo)
            {
                var convertedScen = scen as ScenarioReqLotInfo;
                foreach (var item in CollectionEventList)
                {
                    if (item.Value.Id.Equals(convertedScen.EventId))
                    {
                        parameterList = new List<string>();
                        foreach (var kvp in item.Value.Variables)
                        {
                            parameterList.Add(kvp.Value.Name);
                        }
                        break;
                    }
                }
            }
            else if (scen is ScenarioReqLotMergeAndChange)
            {
                var convertedScen = scen as ScenarioReqLotMergeAndChange;
                parameterList = new List<string>();
                foreach (var item in CollectionEventList)
                {
                    if (item.Value.Id.Equals(convertedScen.EventIdMerge) ||
                        item.Value.Id.Equals(convertedScen.EventIdChange))
                    {
                        foreach (var kvp in item.Value.Variables)
                        {
                            if (false == parameterList.Contains(kvp.Value.Name))
                            {
                                parameterList.Add(kvp.Value.Name);
                            }
                        }
                    }
                }
            }
            else if (scen is ScenarioProceedWithCarrier)
            {
                var convertedScen = scen as ScenarioProceedWithCarrier;

                foreach (var item in CollectionEventList)
                {
                    if (item.Value.Id.Equals(convertedScen.EventId))
                    {
                        parameterList = new List<string>();
                        foreach (var kvp in item.Value.Variables)
                        {
                            parameterList.Add(kvp.Value.Name);
                        }
                        break;
                    }
                }
            }
            else if (scen is EFEM.CustomizedByProcessType.PWA500Common.ScenarioRecipeHandlingRequest)
            {
                parameterList = new List<string>();
                parameterList.Add(RecipeHandlingKeys.KeyParamRecipeId);
                parameterList.Add(RecipeHandlingKeys.KeyUseCommunicationToPM);
            }
            else if (scen is ScenarioReqWorkStart)
            {
                //var convertedScen = scen as ScenarioReqWorkStart;
                parameterList = new List<string>();
                parameterList.Add(RequestDownloadMapFileKeys.KeyParamCarrierId);
                parameterList.Add(RequestDownloadMapFileKeys.KeyParamPortId);
                parameterList.Add(RequestDownloadMapFileKeys.KeyParamLotId);
                parameterList.Add(RequestDownloadMapFileKeys.KeyParamPartId);
                parameterList.Add(RequestDownloadMapFileKeys.KeyParamRecipeId);
                parameterList.Add(RequestDownloadMapFileKeys.KeyParamOperatorId);
                parameterList.Add(RequestDownloadMapFileKeys.KeyParamWaferId);
                parameterList.Add(RequestDownloadMapFileKeys.KeyParamAngle);
                parameterList.Add(RequestDownloadMapFileKeys.KeyNullBinCode);
                parameterList.Add(RequestDownloadMapFileKeys.KeyUseEventHandling);
                //          foreach (var item in CollectionEventList)
                //          {
                //              if (item.Value.Id.Equals(convertedScen.EventId))
                //              {
                //                  parameterList = new List<string>();
                //                  foreach (var kvp in item.Value.Variables)
                //                  {
                //                      parameterList.Add(kvp.Value.Name);
                //                  }
                //var data = convertedScen.WaferData.GetDataAll();
                //if (data != null)
                //                  {
                //                      foreach (var kvp in data)
                //                      {
                //		parameterList.Add(kvp.Key);
                //	}
                //                  }
                //break;
                //              }
                //          }
            }
            else if (scen is ScenarioReqWaferMapDownload)
            {
                parameterList = new List<string>();
                parameterList.Add(RequestDownloadMapFileKeys.KeyParamLotId);
                parameterList.Add(RequestDownloadMapFileKeys.KeyParamRecipeId);
                parameterList.Add(RequestDownloadMapFileKeys.KeyParamOperatorId);
                parameterList.Add(RequestDownloadMapFileKeys.KeyParamWaferId);
            }
            else if (scen is ScenarioReqWaferMapUpload || scen is ScenarioReqBinMapUploadGEM300)
            {
                parameterList = new List<string>();
                parameterList.Add(UploadMapKeys.KeyParamWaferId);
                parameterList.Add(UploadMapKeys.KeyParamFilmFrameLocation);     // 2026.06.24 dwlim [ADD] VID 추가
                parameterList.Add(UploadMapKeys.KeyParamFlatNotchLocation);     // 2026.06.24 dwlim [ADD] VID 추가
                parameterList.Add(UploadMapKeys.KeyParamMapData);
            }
            else if (scen is ScenarioUploadBinDataGEM300)
            {
                parameterList = new List<string>();
                parameterList.Add(UploadCoreOrBinFileKeys.KeySubstrateName);
                parameterList.Add(UploadCoreOrBinFileKeys.KeyMapData);
                parameterList.Add(UploadCoreOrBinFileKeys.KeyPMSFileName);
                parameterList.Add(UploadCoreOrBinFileKeys.KeyPMSFileBody);
            }
            else if (scen is ScenarioReqWaferSplitFromLot)
            {
                var convertedScen = scen as ScenarioReqWaferSplitFromLot;
                foreach (var item in CollectionEventList)
                {
                    if (item.Value.Id.Equals(convertedScen.EventId))
                    {
                        parameterList = new List<string>();
                        foreach (var kvp in item.Value.Variables)
                        {
                            parameterList.Add(kvp.Value.Name);
                        }
                        break;
                    }
                }
            }
            else if (scen is ScenarioReqChipSplit)
            {
                var convertedScen = scen as ScenarioReqChipSplit;
                foreach (var item in CollectionEventList)
                {
                    if (item.Value.Id.Equals(convertedScen.EventId))
                    {
                        parameterList = new List<string>();
                        foreach (var kvp in item.Value.Variables)
                        {
                            parameterList.Add(kvp.Value.Name);
                        }
                        break;
                    }
                }
            }
            else if (scen is ScenarioReqUploadBinFile)
            {
                parameterList = new List<string>();
                parameterList.Add(UploadCoreOrBinFileKeys.KeySubstrateName);
                parameterList.Add(UploadCoreOrBinFileKeys.KeyRingId);
                parameterList.Add(UploadCoreOrBinFileKeys.KeyRecipeId);
                parameterList.Add(UploadCoreOrBinFileKeys.KeySubstrateType);
                parameterList.Add(UploadCoreOrBinFileKeys.KeyStepId);
                parameterList.Add(UploadCoreOrBinFileKeys.KeyEquipId);
                parameterList.Add(UploadCoreOrBinFileKeys.KeyPartId);
                parameterList.Add(UploadCoreOrBinFileKeys.KeySlot);
                parameterList.Add(UploadCoreOrBinFileKeys.KeyLotId);
            }
            else if (scen is ScenarioReqWaferIdAssign)
            {
                var convertedScen = scen as ScenarioReqWaferIdAssign;
                foreach (var item in CollectionEventList)
                {
                    if (item.Value.Id.Equals(convertedScen.EventId))
                    {
                        parameterList = new List<string>();
                        foreach (var kvp in item.Value.Variables)
                        {
                            parameterList.Add(kvp.Value.Name);
                        }
                        break;
                    }
                }
            }
            else if (scen is ScenarioFormattedRecipeHandlingByHost)
            {
                parameterList = new List<string>();
                parameterList.Add(UploadCoreOrBinFileKeys.KeySubstrateName);
                parameterList.Add(UploadCoreOrBinFileKeys.KeyRingId);
                parameterList.Add(UploadCoreOrBinFileKeys.KeyRecipeId);
                parameterList.Add(UploadCoreOrBinFileKeys.KeySubstrateType);
                parameterList.Add(UploadCoreOrBinFileKeys.KeyStepId);
                parameterList.Add(UploadCoreOrBinFileKeys.KeyEquipId);
                parameterList.Add(UploadCoreOrBinFileKeys.KeyPartId);
                parameterList.Add(UploadCoreOrBinFileKeys.KeySlot);
                parameterList.Add(UploadCoreOrBinFileKeys.KeyLotId);
            }
            return parameterList;
        }

        public override Dictionary<string, string> GetScenarioResultData(EN_SCENARIO scenario)
        {
            if (false == ScenarioList.TryGetValue(scenario, out ScenarioBaseClass scen))
                return null;

            return scen.GetResultData();
        }

        public override bool UpdateScenarioParams(string scenarioName, Dictionary<string, string> param)
        {
            if (false == Enum.TryParse(scenarioName, out EN_SCENARIO scenario))
                return false;

            if (false == ScenarioList.TryGetValue(scenario, out var s))
                return false;

            switch (s)
            {
                case SendingEventScenario _:
                    {
                        if (scenario == EN_SCENARIO.SCENARIO_CORE_WAFER_DETACH_START)
                        {
                            List<string> variablesToUpdate = new List<string>();
                            string vidName;

                            #region <EES>
                            vidName = EN_SVID_LIST.CARRIERID.ToString();
                            if (param.ContainsKey(vidName))
                            {
                                variablesToUpdate.Add(param[vidName]);
                            }

                            vidName = EN_SVID_LIST.PORTID.ToString();
                            if (param.ContainsKey(vidName))
                            {
                                variablesToUpdate.Add(param[vidName]);
                            }

                            vidName = EN_SVID_LIST.LOTID.ToString();
                            if (param.ContainsKey(vidName))
                            {
                                variablesToUpdate.Add(param[vidName]);
                            }

                            vidName = EN_SVID_LIST.PARTID.ToString();
                            if (param.ContainsKey(vidName))
                            {
                                variablesToUpdate.Add(param[vidName]);
                            }

                            vidName = EN_SVID_LIST.RECIPEID.ToString();
                            if (param.ContainsKey(vidName))
                            {
                                variablesToUpdate.Add(param[vidName]);
                            }

                            vidName = EN_SVID_LIST.WAFERID.ToString();
                            if (param.ContainsKey(vidName))
                            {
                                variablesToUpdate.Add(param[vidName]);
                            }

                            vidName = EN_SVID_LIST.SLOTID.ToString();
                            if (param.ContainsKey(vidName))
                            {
                                variablesToUpdate.Add(param[vidName]);
                            }

                            vidName = EN_SVID_LIST.OPERID.ToString();
                            if (param.ContainsKey(vidName))
                            {
                                variablesToUpdate.Add(param[vidName]);
                            }
                            #endregion </EES>

                            #region <ERD>
                            vidName = EN_SVID_LIST.NEEDLE_HEIGHT.ToString();
                            if (param.ContainsKey(vidName))
                            {
                                variablesToUpdate.Add(param[vidName]);
                            }

                            vidName = EN_SVID_LIST.EXPENSION_HEIGHT.ToString();
                            if (param.ContainsKey(vidName))
                            {
                                variablesToUpdate.Add(param[vidName]);
                            }

                            vidName = EN_SVID_LIST.PICK_SEARCH_LEVEL.ToString();
                            if (param.ContainsKey(vidName))
                            {
                                variablesToUpdate.Add(param[vidName]);
                            }

                            vidName = EN_SVID_LIST.PICK_SEARCH_SPEED.ToString();
                            if (param.ContainsKey(vidName))
                            {
                                variablesToUpdate.Add(param[vidName]);
                            }

                            vidName = EN_SVID_LIST.PICK_DELAY.ToString();
                            if (param.ContainsKey(vidName))
                            {
                                variablesToUpdate.Add(param[vidName]);
                            }

                            vidName = EN_SVID_LIST.PICK_FORCE.ToString();
                            if (param.ContainsKey(vidName))
                            {
                                variablesToUpdate.Add(param[vidName]);
                            }

                            vidName = EN_SVID_LIST.PICK_SLOWUP_LEVEL.ToString();
                            if (param.ContainsKey(vidName))
                            {
                                variablesToUpdate.Add(param[vidName]);
                            }

                            vidName = EN_SVID_LIST.PICK_SLOWUP_SPEED.ToString();
                            if (param.ContainsKey(vidName))
                            {
                                variablesToUpdate.Add(param[vidName]);
                            }

                            vidName = EN_SVID_LIST.PLACE_SEARCH_LEVEL.ToString();
                            if (param.ContainsKey(vidName))
                            {
                                variablesToUpdate.Add(param[vidName]);
                            }

                            vidName = EN_SVID_LIST.PLACE_SEARCH_SPEED.ToString();
                            if (param.ContainsKey(vidName))
                            {
                                variablesToUpdate.Add(param[vidName]);
                            }

                            vidName = EN_SVID_LIST.PLACE_DELAY.ToString();
                            if (param.ContainsKey(vidName))
                            {
                                variablesToUpdate.Add(param[vidName]);
                            }

                            vidName = EN_SVID_LIST.PLACE_FORCE.ToString();
                            if (param.ContainsKey(vidName))
                            {
                                variablesToUpdate.Add(param[vidName]);
                            }

                            vidName = EN_SVID_LIST.PLACE_SLOWUP_LEVEL.ToString();
                            if (param.ContainsKey(vidName))
                            {
                                variablesToUpdate.Add(param[vidName]);
                            }

                            vidName = EN_SVID_LIST.PLACE_SLOWUP_SPEED.ToString();
                            if (param.ContainsKey(vidName))
                            {
                                variablesToUpdate.Add(param[vidName]);
                            }
                            #endregion </ERD>

                            s.UpdateParamValues(new SendingEventParamValues(variablesToUpdate));
                        }
                        else
                        {
                            s.UpdateParamValues(new SendingEventParamValues(param.Values.ToList()));
                        }
                    }
                    break;
                case ScenarioReqWaferMapDownload _:
                    {
                        List<string> vids = new List<string>();

                        var waferMapData = new WaferMapData
                        {
                            WaferId = param[RequestDownloadMapFileKeys.KeyParamWaferId],
                        };
                        vids.Add(param[RequestDownloadMapFileKeys.KeyParamLotId]);
                        vids.Add(param[RequestDownloadMapFileKeys.KeyParamRecipeId]);
                        vids.Add(param[RequestDownloadMapFileKeys.KeyParamOperatorId]);
                        vids.Add(param[RequestDownloadMapFileKeys.KeyParamWaferId]);

                        s.UpdateParamValues(new ScenarioReqWaferMapDownloadParamValues(vids, waferMapData));
                    }
                    break;
                case ScenarioReqBinMapUploadGEM300 _:
                    {
                        List<string> vids = new List<string>();
                        vids.Add(param[UploadMapKeys.KeyParamWaferId]);
                        vids.Add(param[UploadMapKeys.KeyParamFilmFrameLocation]);   // 2026.06.24 dwlim [ADD] VID 추가
                        vids.Add(param[UploadMapKeys.KeyParamFlatNotchLocation]);   // 2026.06.24 dwlim [ADD] VID 추가
                        vids.Add(param[UploadMapKeys.KeyParamMapData]);

                        ScenarioList[scenario].UpdateParamValues(new ScenarioReqBinMapUploadGEM300ParamValues(vids, true, string.Empty));
                    }
                    break;
                case ScenarioReqWorkEnd _:
                    {
                        List<string> vids = new List<string>();
                        vids.Add(param[UploadCoreOrBinFileKeys.KeyParamCarrierId]);
                        vids.Add(param[UploadCoreOrBinFileKeys.KeyParamPortId]);
                        vids.Add(param[UploadCoreOrBinFileKeys.KeyParamLotId]);
                        vids.Add(param[UploadCoreOrBinFileKeys.KeyParamPartId]);
                        vids.Add(param[UploadCoreOrBinFileKeys.KeyParamRecipeId]);
                        vids.Add(param[UploadCoreOrBinFileKeys.KeyParamOperatorId]);
                        vids.Add(param[UploadCoreOrBinFileKeys.KeyChipQty]);

                        double.TryParse(param[UploadCoreOrBinFileKeys.KeyWaferAngle], out double angle);
                        int.TryParse(param[UploadCoreOrBinFileKeys.KeyCountRow], out int row);
                        int.TryParse(param[UploadCoreOrBinFileKeys.KeyCountCol], out int col);
                        int.TryParse(param[UploadCoreOrBinFileKeys.KeyReferenceX], out int refX);
                        int.TryParse(param[UploadCoreOrBinFileKeys.KeyReferenceY], out int refY);
                        int.TryParse(param[UploadCoreOrBinFileKeys.KeyStartingPosX], out int startingX);
                        int.TryParse(param[UploadCoreOrBinFileKeys.KeyStartingPosY], out int startingY);

                        bool useEventHandling = true;
                        if (param.ContainsKey(UploadCoreOrBinFileKeys.KeyUseEventHandling))
                        {
                            bool.TryParse(param[UploadCoreOrBinFileKeys.KeyUseEventHandling], out useEventHandling);
                        }
                        int.TryParse(param[UploadCoreOrBinFileKeys.KeyChipQty], out int chipQty);
                        var waferMapData = new WaferMapData
                        {
                            WaferId = param[UploadCoreOrBinFileKeys.KeySubstrateName],
                            Angle = angle,
                            CountOfRow = row,
                            CountOfCol = col,
                            IndexOfRefX = refX,
                            IndexOfRefY = refY,
                            IndexOfStartingX = startingX,
                            IndexOfStartingY = startingY,
                            CountOfProcessDies = chipQty,
                            MapData = param[UploadCoreOrBinFileKeys.KeyMapData]
                        };

                        s.UpdateParamValues(new ScenarioReqWorkEndParamValues(vids, useEventHandling, string.Empty, waferMapData));
                    }
                    break;
                case ScenarioUploadBinDataGEM300 _:
                    {   
                        List<string> vids = new List<string>();
                        vids.Add(param[UploadCoreOrBinFileKeys.KeySubstrateName]);
                        vids.Add(param[UploadCoreOrBinFileKeys.KeyPMSFileName]);
                        vids.Add(param[UploadCoreOrBinFileKeys.KeyPMSFileBody]);

                        s.UpdateParamValues(new ScenarioUploadBinDataGEM300ParamValues(vids, true, param[UploadCoreOrBinFileKeys.KeyPMSFileBody]));
                    }
                    break;
                // 2026.06.24 dwlim [ADD] VID 추가 및 수정으로 추가됨
                case ScenarioBinWaferEndForGEM300 _:
                    {
                        List<string> vids = new List<string>();
                        vids.Add(param[WaferEndKeys.KeyParamPortId]);
                        vids.Add(param[WaferEndKeys.KeyParamLotId]);
                        vids.Add(param[WaferEndKeys.KeyParamSlotId]);
                        vids.Add(param[WaferEndKeys.KeyParamRingFrameId]);
                        vids.Add(param[WaferEndKeys.KeyParamSortingInfo]);

                        s.UpdateParamValues(new ScenarioBinWaferEndForGEM300ParamValues(vids, true, param[WaferEndKeys.KeyParamSortingInfo]));
                    }
                    break;
                case ScenarioReqUploadBinFile _:
                    {
                        s.UpdateParamValues(new ScenarioReqUploadBinFileParamValues(
                            NameOfClient,
                            MessagesToSend.RequestUploadBinFile.ToString(),
                            param));
                    }
                    break;
                case ScenarioFormattedRecipeHandlingByHost _:
                    {
                        if (false == param.TryGetValue(RecipeHandlingKeys.KeyParamRecipeId, out string recipeId))
                            return false;
                        if (false == param.TryGetValue(RecipeHandlingKeys.KeyRecipeBody, out string recipeBody))
                            return false;
                        if (false == param.TryGetValue(RecipeHandlingKeys.KeyUseCommunicationToPM, out string useComm))
                            return false;
                        if (false == bool.TryParse(useComm, out bool useCommunication))
                            return false;

                        s.UpdateParamValues(new ScenarioFormattedRecipeHandlingByHostParamValues(useCommunication, recipeId, recipeBody));
                    }
                    break;
                case ClientToClientCommunicationScenario _:
                    {
                        s.UpdateParamValues(new ClientToClientCommunicationParamValues(
                           NameOfClient,
                           MessagesToSend.RequestAssignSubstrateId.ToString(),
                           param));
                    }
                    break;

                default:
                    break;
            }

            return true;
        }
        public override bool IsScenarioEnabled(EN_SCENARIO scenario)
        {
            return (false == (GetInstanceScenario(scenario) == null));
        }
        #endregion /config

        #region Delegate Functions
        public override bool RemoteCommandReceived(string rcmdName, string[] cpNames, string[] cpValues, ref long[] results)
        {
            WriteLog("Received RCMD : " + rcmdName);

            WriteLog("> success");

            return true;
        }
        public override bool ClientToClientMessageReceived(string deviceName, string messageName, string sendingType, string scenarioName, string[] contentNames, string[] messages, EN_MESSAGE_RESULT result, ref bool useLogging)
        {
            if (false == Enum.TryParse(messageName, out MessagesToReceive messageTypeToReceive))
                return false;

            bool requestOnlyMessage = false;
            requestOnlyMessage |= (messageTypeToReceive.Equals(MessagesToReceive.RequestUpdateEquipmentData));
            requestOnlyMessage |= (messageTypeToReceive.Equals(MessagesToReceive.RequestUpdateTraceData));
            requestOnlyMessage |= (messageTypeToReceive.Equals(MessagesToReceive.RequestUpdateEquipmentState));
            requestOnlyMessage |= (messageTypeToReceive.Equals(MessagesToReceive.RequestNotifyAlarmStatus));
            requestOnlyMessage |= (messageTypeToReceive.Equals(MessagesToReceive.RequestFinishPicking));
            requestOnlyMessage |= (messageTypeToReceive.Equals(MessagesToReceive.RequestFinishPlacing));
            //requestOnlyMessage |= (messageTypeToReceive.Equals(MessagesToReceive.RequestMoveMaterial));
            //requestOnlyMessage |= (messageTypeToReceive.Equals(MessagesToReceive.RequestMaterialMoved));

            Dictionary<string, string> messagePairs = new Dictionary<string, string>();
            for (int i = 0; i < contentNames.Length; ++i)
            {
                messagePairs[contentNames[i]] = messages[i];
            }

            if (messageName.StartsWith("Response") || requestOnlyMessage)
            {
                sendingType = DefinesForClientToClientMessage.VALUE_MESSAGE_TYPE_ACK;
                return ParseMessages(deviceName, messageTypeToReceive, scenarioName, messagePairs, result, ref useLogging);
            }
            else
            {
                sendingType = DefinesForClientToClientMessage.VALUE_MESSAGE_TYPE_SEND;
                return ParseMessagesAndAck(deviceName, messageTypeToReceive, scenarioName, messagePairs, result, ref useLogging);
            }
        }
        public override bool SecsMessageReceived(UserDefinedSecsMessage receivedSecsMessage, ref UserDefinedSecsMessage secsMessageToSend)
        {
            foreach (var kvp in ScenarioList)
            {
                if (false == IsScenarioRunning(kvp.Key))
                    continue;

                if (kvp.Value.ReceiveStream == receivedSecsMessage.Stream
                    && receivedSecsMessage.Stream == 12)
                {
                    if (kvp.Value is ScenarioReqWorkStart)
                    {
                        var scenario = kvp.Value as ScenarioReqWorkStart;
                        if (receivedSecsMessage.Function == scenario.FunctionToReceivedWaferMapSetup ||
                            receivedSecsMessage.Function == scenario.FunctionToReceivedWaferMapData)
                        {
                            if (scenario.UpdateReceivedSecsMessage(receivedSecsMessage.Function,
                                receivedSecsMessage.ListItemFormat))
                                break;
                        }
                    }
                    else if (kvp.Value is ScenarioReqWorkEnd)
                    {
                        var scenario = kvp.Value as ScenarioReqWorkEnd;
                        if (receivedSecsMessage.Function == scenario.FunctionToReceivedWaferMapDataSetup ||
                            receivedSecsMessage.Function == scenario.FunctionToReceivedWaferMapTransmitInquire ||
                            receivedSecsMessage.Function == scenario.FunctionToReceivedWaferMapData)
                        {
                            if (scenario.UpdateReceivedSecsMessage(receivedSecsMessage.Function,
                                receivedSecsMessage.ListItemFormat))
                                break;
                        }
                    }
                    else if (kvp.Value is ScenarioUploadBinData)
                    {
                        var scenario = kvp.Value as ScenarioUploadBinData;
                        if (receivedSecsMessage.Function == scenario.FunctionToReceivedWaferMapDataSetup ||
                            receivedSecsMessage.Function == scenario.FunctionToReceivedWaferMapTransmitInquire ||
                            receivedSecsMessage.Function == scenario.FunctionToReceivedWaferMapData)
                        {
                            if (scenario.UpdateReceivedSecsMessage(receivedSecsMessage.Function,
                                receivedSecsMessage.ListItemFormat))
                                break;
                        }
                    }
                }
                else
                {
                    if (kvp.Value.ReceiveStream == receivedSecsMessage.Stream
                        && kvp.Value.ReceiveFunction == receivedSecsMessage.Function
                        && kvp.Value.Receiving)
                    {
                        if (kvp.Value is ScenarioSendEventThenHandlingSecsMessage)
                        {
                            if (kvp.Value.UpdateReceiveMessage(receivedSecsMessage.ListItemFormat))
                            {
                                var targetScenario = kvp.Value as ScenarioSendEventThenHandlingSecsMessage;
                                secsMessageToSend = new UserDefinedSecsMessage(targetScenario.SendStream,
                                    targetScenario.SendFunction);

                                secsMessageToSend.SetStructure(targetScenario.MessageFormatToSend);
                                return true;
                            }
                        }
                        else if (kvp.Value is ScenarioReqLotMergeAndChange)
                        {
                            if (kvp.Value.UpdateReceiveMessage(receivedSecsMessage.ListItemFormat))
                            {
                                var targetScenario = kvp.Value as ScenarioReqLotMergeAndChange;
                                secsMessageToSend = new UserDefinedSecsMessage(targetScenario.StreamToSend,
                                    targetScenario.FunctionToSend);

                                secsMessageToSend.SetStructure(targetScenario.MessageFormatToSend);
                                return true;
                            }
                        }
                        else if (kvp.Value is ScenarioReqWaferMapDownload)
                        {
                            if (kvp.Value.UpdateReceiveMessage(receivedSecsMessage.ListItemFormat))
                            {
                                return true;
                            }
                              
                        }
                    }
                }
            }
            return false;
            //throw new NotImplementedException();
        }
        public override EN_PPGRANT CheckingRecipeControlGrant(string recipeName)
        {
            // 현재 자재 정보에 따라 레시피를 다운로드 할지 말지 여부를 결정해야하는데,
            // EFEM은 레시피를 사용하지 않으니 그냥 OK로 넘긴다.
            return EN_PPGRANT.OK;

            var state = EquipmentState_.EquipmentState.GetInstance().GetState();
            switch (state)
            {
                case EquipmentState_.EQUIPMENT_STATE.EXECUTING:
                case EquipmentState_.EQUIPMENT_STATE.SETUP:
                    return Task.TaskOperator.GetInstance().IsMachineWait()
                        ? EN_PPGRANT.OK
                        : EN_PPGRANT.BUSY;
                // 경우에 따라서 레시피 조작이 가능한지 여부 판단 후 코드 리턴
                //
                // return EN_PPGRANT.OK; or return EN_PPGRANT.BUSY;

                case EquipmentState_.EQUIPMENT_STATE.FINISHING:
                case EquipmentState_.EQUIPMENT_STATE.INITIALIZE:
                case EquipmentState_.EQUIPMENT_STATE.READY:
                    if (IsScenarioRunning(EN_SCENARIO.SCENARIO_REQ_RECIPE_DOWNLOAD))
                    {
                        UpdateScenarioPermission(EN_SCENARIO.SCENARIO_REQ_RECIPE_DOWNLOAD, false);
                    }

                    return EN_PPGRANT.BUSY;


                case EquipmentState_.EQUIPMENT_STATE.IDLE:
                case EquipmentState_.EQUIPMENT_STATE.PAUSE:
                    return EN_PPGRANT.OK;

                default:
                    if (IsScenarioRunning(EN_SCENARIO.SCENARIO_REQ_RECIPE_DOWNLOAD))
                    {
                        UpdateScenarioPermission(EN_SCENARIO.SCENARIO_REQ_RECIPE_DOWNLOAD, false);
                    }
                    return EN_PPGRANT.BUSY;
            }
        }
        protected override void OnTerminalMessageReceived(string message)
        {
            SendClientToClientMessage(NameOfClient, MessagesToSend.RequestCallOperator.ToString(),
                                         string.Empty, string.Empty,
                                         new string[] { "Message" }, new string[] { message },
                                         EN_MESSAGE_RESULT.OK, false);
        }
        #endregion /Delegate Functions

        #region Variable Get/Set
        public override void UpdateVariablesAll()
        {
            var baseStatusVariablesToUpdate = new Dictionary<long, string>
            {
                [14] = _functionsForPWA500.GetModelName(),
                [15] = GetSoftwareVersion()
            };
            UpdateVariable(baseStatusVariablesToUpdate.Keys.ToArray(), baseStatusVariablesToUpdate.Values.ToArray());

            var baseEquipmentConstantsToUpdate = new Dictionary<long, string>
            {
                [121] = "PROTEC"
            };
            UpdateEquipmentConstants(baseEquipmentConstantsToUpdate.Keys.ToArray(), baseEquipmentConstantsToUpdate.Values.ToArray());

            UpdateECVAll();
        }
        private void UpdateECVAll()
        {
            Dictionary<string, string> toUpdate = new Dictionary<string, string>();
            foreach (var item in _myEcidListForCommon)
            {
                var key = item.Key;
                var value = _recipe.GetValue(EN_RECIPE_TYPE.COMMON, item.Key, string.Empty);

                toUpdate[key] = value;
            }

            foreach (var item in _myEcidListForEquipment)
            {
                var key = item.Key;
                var value = _recipe.GetValue(EN_RECIPE_TYPE.EQUIPMENT, item.Key, string.Empty);

                toUpdate[key] = value;
            }

            UpdateEquipmentConstants(toUpdate.Keys.ToArray(), toUpdate.Values.ToArray());

            // COMMON
            //         List<long> listEcids = new List<long>();
            //         List<string> listValues = new List<string>();

            //         int index = 0;
            //         var paramRange = PARAM_RANGE.GetInstance();
            //         int indexOfItem;
            //         string value = string.Empty;
            //foreach (var en in Enum.GetValues(typeof(PARAM_COMMON)))
            //         {
            //             string parameter = en.ToString();
            //             string[] parameters = parameter.Split('_');

            //             if (false == int.TryParse(parameters[parameters.Length - 1], out indexOfItem))
            //             {
            //                 value = _recipe.GetValue(EN_RECIPE_TYPE.COMMON, en.ToString(),
            //                     0, EN_RECIPE_PARAM_TYPE.VALUE, String.Empty);

            //                 listEcids.Add(paramRange.ECID_COMMON_START + index);
            //                 listValues.Add(value);

            //                 ++index;
            //             }
            //             else
            //             {
            //                 for (int i = 0; i < indexOfItem; i++)
            //                 {
            //                     value = _recipe.GetValue(EN_RECIPE_TYPE.COMMON, en.ToString(),
            //                         0, EN_RECIPE_PARAM_TYPE.VALUE, String.Empty);

            //                     listEcids.Add(paramRange.ECID_COMMON_START + index);
            //                     listValues.Add(value);

            //                     ++index;
            //                 }
            //             }
            //         }
            //         foreach (PARAM_EQUIPMENT en in Enum.GetValues(typeof(PARAM_EQUIPMENT)))
            //         {
            //             string parameter = en.ToString();
            //             string[] parameters = parameter.Split('_');

            //             if (false == int.TryParse(parameters[parameters.Length - 1], out indexOfItem))
            //             {
            //                 value = _recipe.GetValue(EN_RECIPE_TYPE.EQUIPMENT, parameter,
            //                     0, EN_RECIPE_PARAM_TYPE.VALUE, String.Empty);

            //                 listEcids.Add(paramRange.ECID_EQUIP_START + index);
            //                 listValues.Add(value);

            //                 ++index;
            //             }
            //             else
            //             {
            //                 for (int i = 0; i < indexOfItem; i++)
            //                 {
            //                     value = _recipe.GetValue(EN_RECIPE_TYPE.EQUIPMENT, parameter,
            //                         i, EN_RECIPE_PARAM_TYPE.VALUE, String.Empty);

            //                     listEcids.Add(paramRange.ECID_EQUIP_START + index);
            //                     listValues.Add(value);

            //                     ++index;
            //                 }
            //             }
            //         }

            //         long[] ecids = listEcids.ToArray();
            //         string[] values = listValues.ToArray();

            //         UpdateEquipmentConstants(ecids, values);        
        }
        public override bool UpdateECVParameter(string strECVName, string strValue)
        {
            return true;
            //throw new NotImplementedException();
        }
        public override void EquipmentParameterChangeRequested(string[] ecNameList, string[] valueList)
        {
            // 공정설비에 변경 요청 메시지를 보낸다. -> 현재는 서버 to PM 업데이트 시나리오가 없음
            // TODO : 내거인지 아닌지 확인 후 값이 다르면 갱신해야한다.
            for (int i = 0; i < ecNameList.Length; ++i)
            {
                var k = ecNameList[i];
                var v = valueList[i];
                if (_myEcidListForCommon.TryGetValue(ecNameList[i], out var c))
                {
                    if (v != c)
                    {
                        _myEcidListForCommon[k] = v;
                        UpdateEcidFromHost(EN_RECIPE_TYPE.COMMON, k, v);
                    }
                }

                if (_myEcidListForEquipment.TryGetValue(ecNameList[i], out var e))
                {
                    if (v != e)
                    {
                        _myEcidListForEquipment[k] = v;
                        UpdateEcidFromHost(EN_RECIPE_TYPE.EQUIPMENT, k, v);
                    }
                }
            }

            UpdateEquipmentConstants(ecNameList, valueList);
        }
        private void UpdateEcidFromHost(EN_RECIPE_TYPE type, string name, string value)
        {
            var formatString = _recipe.GetValue(type, name, 0, EN_RECIPE_PARAM_TYPE.DATA_TYPE, string.Empty);
            if (false == Enum.TryParse(formatString, out EN_DATA_TYPE format))
                return;

            switch (format)
            {
                case EN_DATA_TYPE.BOOL:
                    {
                        bool valueBool = value == "1" ? true : false;
                        _recipe.SetValue(type, name, valueBool.ToString());
                    }
                    break;

                default:
                    {
                        _recipe.SetValue(type, name, value);
                    }
                    break;
            }
        }
        #endregion /Variable Get/Set

        #region state changed
        public override void ControlStateChanged(string state)
        {
            PushCurrentTraceSnapshotToHost();
        }
        public override void EquipmentstateChanged(string state)
        {

        }
        #endregion /state changed

        #region <Recipe Management>
        public override void RecipeFileIsDeleted(string[] deletedFileList)
        {
            // 레시피 파일 제거 후 이벤트 발생 필요 시 구현
        }

        #region <UnFormatted Recipe>
        public override void UploadingUnFormattedRecipeAckReceived(string recipeName, EN_ACK7 recipeUploadAck)
        {
            throw new NotImplementedException();
        }
        public override bool UploadingUnFormattedRecipeReceived(string recipeName, ref string recipeFullPath)
        {
            // recipeName = A or A.rcp;
            // recipeFullPath = D:\Work\Recipe\A.rcp;
            WriteLog("Upload recipe (unformatted)");

            // 1. 파일 있는지 체크, 있다면 복사
            if (false == CheckRecipeFiles(recipeName))
            {
                WriteLog("> Check failed");
                return false;
            }

            // 2. 파일 압축
            if (false == CompressFiles(_recipePath, recipeName, out recipeFullPath))
            {
                WriteLog("> Compression failed");
                return false;
            }

            if (System.Diagnostics.Debugger.IsAttached && File.Exists(recipeFullPath))
            {
                var bytes = File.ReadAllBytes(recipeFullPath);
                string temp = string.Empty;
                for (int i = 0; i < bytes.Length; ++i)
                {
                    if (i == 0)
                        temp = $"{bytes[i]}";
                    else
                        temp = $"{temp} {bytes[i]}";
                }

                Console.WriteLine($" ---- Uploading File Bytes ---- Length : {bytes.Length}");
                Console.WriteLine(temp);
            }

            //         string source = string.Format("{0}\\{1}{2}", _recipePath, recipeName, Define.DefineConstant.FileFormat.FILEFORMAT_RECIPE);
            //if (false == FunctionsETC.FileExistCheck(source))
            //	return false;

            //string destination = string.Format("{0}\\upload\\{1}{2}", _recipePath, recipeName, Define.DefineConstant.FileFormat.FILEFORMAT_RECIPE);
            //if (false == FunctionsETC.FileCopy(source, destination))
            //	return false;

            //recipeFullPath = destination;

            WriteLog("> Success");
            return true;
        }
        public override EN_ACK7 DownloadingUnFormattedRecipeReceived(string recipeName, string recipeFullPath)
        {
            // recipeName = A or A.rcp;
            // recipeFullPath = D:\Work\SecsGem\XWork\Recipe\A.rcp;
            WriteLog("Download recipe (unformatted)");

            try
            {
                string newFullPath = recipeFullPath;
                if (false == Path.HasExtension(recipeFullPath))
                {
                    newFullPath = string.Format("{0}.zip", recipeFullPath);

                    if (File.Exists(newFullPath))
                        File.Delete(newFullPath);

                    File.Move(recipeFullPath, newFullPath);
                }


                ExtractFile(newFullPath, _recipePath, recipeName);
            }
            catch (Exception)
            {
                throw;
            }
            // 1. 파일이 정상인지 검사
            // 2. 파일 압축 해제

            //if (false == FrameOfSystem3.Task.TaskOperator.GetInstance().IsMachineWait())
            //{
            //	WriteLog("> machine is running");
            //	return EN_ACK7.UNSUPPORTED;
            //}

            //if (false == FunctionsETC.FileExistCheck(recipeFullPath))
            //{
            //	WriteLog("> source file not found : " + recipeFullPath);
            //	return EN_ACK7.NOT_FOUND;
            //}

            //         // 3. 각 설비에 파일 복사
            //string destination = AddExtensionToFileName(string.Format("{0}\\{1}", _recipePath, recipeName));

            //if (false == FunctionsETC.FileCopy(recipeFullPath, destination))
            //{
            //	WriteLog("> file copy failed");
            //	return EN_ACK7.PERMISSION;
            //}
            string recipePath = _recipePath;
            string recipeId = recipeName;
            if (false == Path.GetExtension(recipeId).Equals(FileFormat.FILEFORMAT_RECIPE))
            {
                recipeId = string.Format("{0}{1}", recipeId, FileFormat.FILEFORMAT_RECIPE);
            }
            string errorMessage = string.Empty;
            if (false == Recipe.Recipe.GetInstance().LoadProcessRecipe(ref recipePath, ref recipeId, ref errorMessage))
            {
                return EN_ACK7.PERMISSION;
            }

            WriteLog("> Success");

            // 다운로드를 성공했으니 시나리오 실행 중이라면 Permission OK
            if (IsScenarioRunning(EN_SCENARIO.SCENARIO_REQ_RECIPE_DOWNLOAD))
            {
                UpdateScenarioPermission(EN_SCENARIO.SCENARIO_REQ_RECIPE_DOWNLOAD, true);
            }

            return EN_ACK7.OK;
        }
        #endregion </UnFormatted Recipe>

        #region <Formatted Recipe Control>
        public override bool UploadingFormattedRecipeReceived(string recipeName, out Dictionary<string, SemiObject[]> recipeBodies)
        {
            // Host에서 Recipe Download 요청
            //  - EFEM, PM 입장에선 Upload

            // out Dictionary<string, SemiObject[]> recipeBodies에서 Key는 CCODE, Value는 PPARM이다.

            // 1. 멤버변수 - FIeld (응답을 받았는지 Flag, Recipe Data)
            // 2. Flag와 Message Data 초기화 하고
            // 3. Request Upload Recipe Message 보내고
            // 4. While문으로 Flag 체크하고
            // 5. 받은 Data를 파싱해서 out으로 되어있는 Recipe Bodies로 채워서 보낸다

            _isReceived = false;
            _recipeBody = string.Empty;
            recipeBodies = new Dictionary<string, SemiObject[]>();

            EN_MESSAGE_RESULT result = EN_MESSAGE_RESULT.OK;

            Dictionary<string, string> messageContentToSend = new Dictionary<string, string>
            {
                [RecipeHandlingKeys.KeyRecipeId] = recipeName
            };

            if (false == SendClientToClientMessage(NameOfClient, MessagesToSend.RequestUploadRecipe.ToString(),
                string.Empty, string.Empty, messageContentToSend.Keys.ToArray(), messageContentToSend.Values.ToArray(), result, true))
                return false;

            while (!_isReceived)
            {
            }

            FormattedRecipeParser parser = FormattedRecipeParser.Instance;
            Dictionary<string, string> convertedRecipeData = parser.ConvertStringToDictionary(_recipeBody, EN_PROCESS_TYPE.DIE_TRANSFER_300);
            foreach (var item in convertedRecipeData)
            {
                recipeBodies[item.Key] = new SemiObjectAscii[] { new SemiObjectAscii(item.Key.ToString(), item.Value.ToString()) };
            }

            return true;
        }
        // Download한 레시피 저장하거나 PM에 보내는 부분 처리해야하는데.. 
        public override bool DownloadingFormattedRecipeReceived(string recipeName, Dictionary<string, string[]> recipeBodies)
        {
            // Host에서 EFEM으로 Recipe Upload
            //  - EFEM, PM 입장에선 Download

            _isReceived = false;
            Dictionary<string, string> recipeData = new Dictionary<string, string>();

            //  Dictionary<string, string[]> recipeBodies - Key: CCODE, Value: PPARM
            // PPARM은 Value만 주기로해서 CCODE당 1개씩이다. PM에 문제 발생 방지를 위해 파싱할 때 1개로 강제
            foreach (var item in recipeBodies)
            {
                recipeData[item.Key] = item.Value[0];
            }
            
            EN_MESSAGE_RESULT result = EN_MESSAGE_RESULT.OK;
            FormattedRecipeParser parser = FormattedRecipeParser.Instance;
            string recipeBody = parser.ConvertForSendToPM(recipeData, EN_PROCESS_TYPE.DIE_TRANSFER_300);

            if (string.IsNullOrWhiteSpace(recipeBody))
                return false;

            Dictionary<string, string> messageContentToSend = new Dictionary<string, string>
            {
                [RecipeHandlingKeys.KeyRecipeId] = recipeName,
                [RecipeHandlingKeys.KeyRecipeBody] = recipeBody
            };
            
            if (false == SendClientToClientMessage(NameOfClient, MessagesToSend.RequestDownloadRecipe.ToString(),
                DefinesForClientToClientMessage.VALUE_MESSAGE_TYPE_SEND, string.Empty, messageContentToSend.Keys.ToArray(),
                messageContentToSend.Values.ToArray(), result, true))
                return false;

            while (!_isReceived)
            {

            }

            return true;
        }
        #endregion </Formatted Recipe Control>

        #endregion </Recipe Management>

        #region Alarm
        public override void ExecuteReportAlarm(int alarmId, EN_GEM_ALARM_STATE state)
        {
            if (false == Recipe.Recipe.GetInstance().GetValue(EN_RECIPE_TYPE.COMMON, PARAM_COMMON.UseSecsGem.ToString(), false))
                return;

            alarmId += 1000000;

            base.ExecuteReportAlarm(alarmId, state);
        }
        #endregion /Alarm

        public override void Execute()
        {
            _lotHistoryLog.ExecuteWriteAsync();

            _functionsForPWA500.ExecuteScanrioToCarrierLoadAsync();
        }
        #endregion /interface

        #region method
        private bool CheckRecipeFiles(string recipeName)
        {
            try
            {
                if (false == Directory.Exists(_recipePath))
                    Directory.CreateDirectory(_recipePath);

                string recipeFullPath = string.Empty;
                if (false == HasRecipeFile(_recipePath, recipeName, out recipeFullPath))
                {
                    // 파일이 있는지 체크
                    string targetRecipeName = string.Empty;
                    string[] files = Directory.GetFiles(_recipePath);
                    for (int i = 0; i < files.Length; ++i)
                    {
                        string fileName = Path.GetFileNameWithoutExtension(files[i]);
                        if (recipeName.Equals(fileName))
                        {
                            targetRecipeName = fileName;
                            break;
                        }
                    }

                    // 파일이 없으면,
                    if (string.IsNullOrEmpty(targetRecipeName))
                    {
                        targetRecipeName = recipeName;

                        string path = string.Empty, currentRecipe = string.Empty;
                        _recipe.GetProcessFileInformation(ref path, ref currentRecipe);
                        string fileNameWithoutExtension = Path.GetFileNameWithoutExtension(recipeName);

                        string sourceFileName = Path.Combine(path, currentRecipe);
                        string destFileName = Path.Combine(path, "EFEM", "RMS", string.Format("{0}{1}", fileNameWithoutExtension, FileFormat.FILEFORMAT_RECIPE));

                        try
                        {
                            WriteLog(string.Format("Create a recipe file (Source : {0}, Destination : {1}", sourceFileName, destFileName));
                            File.Copy(sourceFileName, destFileName);
                            WriteLog("Create a recipe file has completed");
                        }
                        catch (Exception ex)
                        {
                            WriteLog(string.Format("Recipe File Copy has Failed => {0}, {1}", ex.Message, ex.StackTrace));
                            return false;
                        }

                        //WriteLog(string.Format("There is no recipe file : {0}", recipeName));
                        //return false;
                    }
                }
                CopyRecipeFileToBasePath(_recipePath, "EFEM", recipeName, recipeFullPath, false);

                //if (RecipePath == null || RecipePath.Count <= 0)
                //{
                //    WriteLog("Invalid client path");
                //    return false;
                //}

                //foreach (var item in RecipePath)
                {
                    //string recipePathForClient = item.Value;
                    // 사용자 이름과 비밀번호가 잘못되었다고 나옴
                    //string recipePathForClient = @"\\ADT02-500BIN\Recipe\RMS";
                    //if (false == HasRecipeFile(_recipePathForPWA500BIN/*recipePathForClient*/, recipeName, out recipeFullPath))
                    if (false == File.Exists(_recipePathToUploadForPM))
                    {
                        WriteLog(string.Format("There is no recipe file in client : {0}", _recipePathToUploadForPM));
                        return false;
                    }

                    //CopyRecipeFileToBasePath(_recipePath, NameOfPM, recipeName, _recipePathToUploadForPM, true);
                }

                #region MyRegion
                //recipeFullPath = string.Empty;

                ////string[] files = Directory.GetFiles(path);

                //string sourceFileName = recipeName;
                //if (recipeName.Contains(FileFormat.FILEFORMAT_RECIPE))
                //{
                //    sourceFileName = Path.GetFileNameWithoutExtension(recipeName);
                //}

                //for (int i = 0; i < 1; ++i)
                //{
                //    string fileName = string.Format(@"{0}\{1}{2}", RecipePathForPM, recipeName, FileFormat.FILEFORMAT_RECIPE);
                //    //if (fileName.Contains(FileFormat.FILEFORMAT_RECIPE))
                //    //{
                //    //    fileName = Path.GetFileNameWithoutExtension(fileName);
                //    //}
                //    //if (fileName.Equals(sourceFileName))
                //    {
                //        recipeFullPath = fileName;
                //        //return true;
                //    }
                //}

                ////return false;

                ////foreach (var item in RecipePath)
                //{
                //    //string recipePathForClient = item.Value;
                //    //if (false == HasRecipeFile(recipePathForClient, recipeName, out recipeFullPath))
                //    //{
                //    //    WriteLog(string.Format("There is no recipe file in client({0}) : {1}", item.Key, recipeName));
                //    //    return false;
                //    //}
                //    string temp = RecipePath.Keys.ToArray()[0];
                //    //CopyRecipeFileToBasePath(_recipePath, temp, recipeName, recipeFullPath);
                //}
                #endregion

                return true;
            }
            catch (Exception ex)
            {
                WriteLog(string.Format("{0} -> {1}", ex.Message, ex.StackTrace));
                return false;
            }
        }
        private bool ExtractFile(string targetFileFullPath, string basePath, string recipeName)
        {
            try
            {
                string outputPath = string.Format(@"{0}\Download\{1}", basePath, recipeName);
                if (Directory.Exists(outputPath))
                {
                    string[] filesToDelete = Directory.GetFiles(outputPath);
                    for (int i = 0; i < filesToDelete.Length; ++i)
                    {
                        File.Delete(filesToDelete[i]);
                    }
                }

                System.IO.Compression.ZipFile.ExtractToDirectory(targetFileFullPath, outputPath);

                string[] filesToMove = Directory.GetFiles(outputPath);
                for (int i = 0; i < filesToMove.Length; ++i)
                {
                    string targetPath = string.Empty;

                    string onlyFileName = Path.GetFileName(filesToMove[i]);
                    switch (onlyFileName)
                    {
                        case "EFEM.rcp":
                            targetPath = _recipePath;
                            break;

                        default:
                            //foreach (var item in RecipePath)
                            {
                                string fileName = string.Format("{0}.rcp", NameOfPM);
                                if (onlyFileName.Equals(fileName))
                                {
                                    //string fullPath = string.Format(@"\\192.168.100.150\EFEM\RMS\Download\{0}\{1}{2}", NameOfPM, recipeName, FileFormat.FILEFORMAT_RECIPE);

                                    // TODO : 임시                                   
                                    //targetPath = @"\\192.168.100.150\EFEM\RMS\Download";// outputPath.Replace("127.0.0.1", "192.168.100.150");                                  
                                    targetPath = string.Format(@"{0}\Download\{1}", _recipePath, recipeName);
                                    if (false == Directory.Exists(targetPath))
                                        Directory.CreateDirectory(targetPath);
                                    //targetPath = string.Format(@"{0}\Download", _recipePathForPWA500BIN);
                                }
                            }
                            break;
                    }

                    string targetFullPath = string.Format(@"{0}\{1}", targetPath, recipeName);
                    if (false == Path.HasExtension(targetFullPath))
                    {
                        targetFullPath = string.Format(@"{0}{1}", targetFullPath, FileFormat.FILEFORMAT_RECIPE);
                    }

                    if (File.Exists(targetFullPath))
                        File.Delete(targetFullPath);

                    File.Move(filesToMove[i], targetFullPath);
                }

                return true;
            }
            catch (Exception ex)
            {
                WriteLog(string.Format("{0} -> {1}", ex.Message, ex.StackTrace));

                return false;
            }

        }
        private bool CompressFiles(string basePath, string recipeName, out string fullPathToUpload)
        {
            try
            {
                string outputPath = string.Format(@"{0}\Upload", basePath);
                string pathToCompress = string.Format(@"{0}\{1}", outputPath, recipeName);
                string outputFile = string.Format(@"{0}\{1}.zip", outputPath, recipeName);
                if (File.Exists(outputFile))
                    File.Delete(outputFile);

                System.IO.Compression.ZipFile.CreateFromDirectory(pathToCompress, outputFile);

                string[] files = Directory.GetFiles(pathToCompress);
                for (int i = 0; i < files.Length; ++i)
                {
                    File.Delete(files[i]);
                }

                Directory.Delete(pathToCompress);

                fullPathToUpload = string.Format(@"{0}\{1}", outputPath, recipeName);
                if (File.Exists(fullPathToUpload))
                    File.Delete(fullPathToUpload);

                File.Move(outputFile, fullPathToUpload);

                return true;
            }
            catch (Exception ex)
            {
                WriteLog(string.Format("{0} -> {1}", ex.Message, ex.StackTrace));
                fullPathToUpload = string.Empty;
                return false;
            }

        }
        private bool HasRecipeFile(string path, string recipeName, out string recipeFullPath)
        {
            recipeFullPath = string.Empty;

            string[] files = Directory.GetFiles(path);

            string sourceFileName = recipeName;
            if (recipeName.Contains(FileFormat.FILEFORMAT_RECIPE))
            {
                sourceFileName = Path.GetFileNameWithoutExtension(recipeName);
            }

            for (int i = 0; i < files.Length; ++i)
            {
                string fileName = files[i];
                if (fileName.Contains(FileFormat.FILEFORMAT_RECIPE))
                {
                    fileName = Path.GetFileNameWithoutExtension(files[i]);
                }
                if (fileName.Equals(sourceFileName))
                {
                    recipeFullPath = files[i];
                    return true;
                }
            }

            return false;
        }
        private void CopyRecipeFileToBasePath(string basePath, string nameOfClent, string recipeName, string fullPathForTargetFile, bool moveFiles)
        {
            string pathToCopy = string.Format(@"{0}\Upload\{1}\{2}{3}", basePath, recipeName, nameOfClent, FileFormat.FILEFORMAT_RECIPE);
            string pathName = Path.GetDirectoryName(pathToCopy);

            if (File.Exists(pathToCopy))
                File.Delete(pathToCopy);

            if (File.Exists(pathName))
                File.Delete(pathName);

            if (false == Directory.Exists(pathName))
                Directory.CreateDirectory(pathName);

            if (false == moveFiles)
            {
                File.Copy(fullPathForTargetFile, pathToCopy, true);
            }
            else
            {
                File.Move(fullPathForTargetFile, pathToCopy);
            }
        }

        private string GetSoftwareVersion()
        {
            var fv = System.Diagnostics.FileVersionInfo.GetVersionInfo(System.Reflection.Assembly.GetExecutingAssembly().Location);
            var assemblyVersion = System.Reflection.Assembly.GetExecutingAssembly().GetName().Version;
            var fullVersion = string.Format("{0}.{1}.{2}", fv.FileVersion.ToString(), assemblyVersion.Build.ToString(), assemblyVersion.Revision.ToString());
            return fullVersion;
        }
        private bool CheckSendData(Dictionary<string, string> data, params string[] keys)
        {
            foreach (string key in keys)
            {
                if (false == data.ContainsKey(key))
                    return false;
            }
            return true;
        }
        private bool GetRecipeFileList(out List<string> result)
        {
            result = new List<string>();

            System.IO.DirectoryInfo dInfo = new System.IO.DirectoryInfo(_recipePath);
            try
            {
                foreach (var fInfo in dInfo.GetFiles())
                {
                    if (fInfo.Extension.ToLower().Equals(Define.DefineConstant.FileFormat.FILEFORMAT_RECIPE))
                    {
                        result.Add(System.IO.Path.GetFileNameWithoutExtension(fInfo.Name));
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return false;
            }

            return true;
        }
        private bool LoadPpid(string receivedPpid)
        {
            string ppid = AddExtensionToFileName(receivedPpid);
            WriteLog(string.Format("> target file {0}\\{1}", _recipePath, ppid));

            if (false == FunctionsETC.FileExistCheck(_recipePath, ppid))
            {
                WriteLog("> file not found");
                return false;
            }

            string path = _recipePath;
            string strErrorMsg = string.Empty;
            if (false == _recipe.LoadProcessRecipe(ref path, ref ppid, ref strErrorMsg))
            {
                WriteLog(string.Format("> recipe load fail : {0}" + strErrorMsg));
                return false;
            }
            return true;
        }

        //private void ExecuteQueuedScenario()
        //{
        //    if (_executingScenarioInfo != null)
        //    {
        //        var result = ExecuteScenario(_executingScenarioInfo.Scenario);
        //        switch (result)
        //        {
        //            case EN_SCENARIO_RESULT.WAITING:
        //            case EN_SCENARIO_RESULT.PROCEED:
        //                return;

        //            case EN_SCENARIO_RESULT.COMPLETED:
        //            case EN_SCENARIO_RESULT.ERROR:
        //            case EN_SCENARIO_RESULT.TIMEOUT_ERROR:
        //                {
        //                    var messageResult = EN_MESSAGE_RESULT.NG;
        //                    if (result.Equals(EN_SCENARIO_RESULT.COMPLETED))
        //                    {
        //                        messageResult = EN_MESSAGE_RESULT.OK;
        //                    }

        //                    var resultOfScenario = GetScenarioResultData(_executingScenarioInfo.Scenario);

        //                    // 검증 필요
        //                    _functionsForPWA500.ExecuteAfterScenarioCompletion(_executingScenarioInfo.Scenario,
        //                            _executingScenarioInfo.ScenarioParams,
        //                            resultOfScenario,
        //                            _executingScenarioInfo.AdditionalParams,
        //                            messageResult,
        //                            false);

        //                    _executingScenarioInfo = null;
        //                }
        //                break;
        //            default:
        //                break;
        //        }
        //    }
        //    else
        //    {
        //        if (false == _queuedScenario.TryDequeue(out var head))
        //            return;

        //        if (IsScenarioRunning(head.Scenario))
        //        {
        //            // 연속 Enqueue 된 경우?
        //            //QueuedScenario.TryDequeue(out head);
        //            _queuedScenario.Enqueue(head);
        //            return;
        //        }

        //        _executingScenarioInfo = head;

        //        // 파라메터 갱신
        //        EN_SCENARIO scenario = head.Scenario;
        //        var scenarioParams = head.ScenarioParams;
        //        UpdateScenarioParams(scenario.ToString(), scenarioParams);

        //        //if (QueuedScenario.Count <= 0)
        //        //    return;

        //        //if (QueuedScenario.TryPeek(out QueuedScenarioInfo _temporaryScenario))
        //        //{
        //        //    if (IsScenarioRunning(_temporaryScenario.Scenario))
        //        //    {
        //        //        QueuedScenario.TryDequeue(out _temporaryScenario);
        //        //        QueuedScenario.Enqueue(_temporaryScenario);
        //        //        return;
        //        //    }

        //        //    QueuedScenario.TryDequeue(out _executingScenarioInfo);

        //        //    // 파라메터 갱신
        //        //    EN_SCENARIO scenario = _executingScenarioInfo.Scenario;
        //        //    var scenarioParams = _executingScenarioInfo.ScenarioParams;
        //        //    UpdateScenarioParams(scenario.ToString(), scenarioParams);
        //        //}
        //    }
        //}

        // TODO : AutoScenario
        private void EnqueueScenario(
            EN_SCENARIO scenario,
            Dictionary<string, string> scenarioParams,
            Dictionary<string, string> additionalParams = null)
        {
            EnqueueAutoScenarioByUpdate(ScenarioSenders.Auto.ToString(), scenario, scenarioParams, additionalParams);
        }

        protected override void OnAutoScenarioCompleted(
            AutoScenarioRequest request,
            EN_SCENARIO_RESULT result,
            Dictionary<string, string> resultData)
        {
            if (request == null)
                return;

            EN_MESSAGE_RESULT messageResult = EN_MESSAGE_RESULT.NG;
            if (result == EN_SCENARIO_RESULT.COMPLETED)
            {
                messageResult = EN_MESSAGE_RESULT.OK;
            }

            ExecuteAfterScenarioCompletion(request.Scenario,
                request.ScenarioParams,
                resultData,
                request.AdditionalParams,
                messageResult,
                false);
        }

        private Dictionary<string, string> MakeScenarioParamForStepHandling(string substrateKey)
        {
            var pmName = ProcessModuleGroup.Instance.GetProcessModuleName(ProcessModuleIndex);
            List<Substrate> substrates = new List<Substrate>();
            if (_substrateManager.GetSubstratesAtProcessModule(pmName, ref substrates))
            {
                foreach (var item in substrates)
                {
                    if (string.Equals(item.UniqueKey, substrateKey, StringComparison.OrdinalIgnoreCase))
                    {
                        Dictionary<string, string> param = new Dictionary<string, string>
                        {
                            [EN_SVID_LIST.LOTID.ToString()] = item.LotId,
                            [EN_SVID_LIST.SLOTID.ToString()] = item.SourceSlot.ToString(),
                            [EN_SVID_LIST.RECIPEID.ToString()] = item.RecipeId,
                            [EN_SVID_LIST.RECIPE_STEP.ToString()] = "1",
                            [EN_SVID_LIST.UNITID.ToString()] = _functionsForPWA500.GetModelName()
                        };

                        return param;
                    }
                }
            }

            return null;
        }

        private void ExecuteAfterScenarioCompletion(
            EN_SCENARIO typeOfScenario,
            Dictionary<string, string> scenarioParams,
            Dictionary<string, string> resultOfScenario,
            Dictionary<string, string> additionalParams,
            EN_MESSAGE_RESULT result,
            bool isManual = false)
        {
            // 완료된 시나리오 타입에 따라 실행되어야할 액션을 여기서 선택한다.
            switch (typeOfScenario)
            {
                case EN_SCENARIO.SCENARIO_STEP_END:
                    {
                        #region
                        // TODO : 에러 로그 필요
                        if (false == additionalParams.TryGetValue(AdditionalParamKeys.KeySubstrateKey, out string substrateKey) ||
                            string.IsNullOrWhiteSpace(substrateKey) ||
                            false == _substrateManager.GetSubstrateByKey(substrateKey, out var s) ||
                            s == null)
                            return;

                        additionalParams.TryGetValue(AdditionalParamKeys.KeyLotId, out var lotId);
                        additionalParams.TryGetValue(AdditionalParamKeys.KeySubstrateId, out var substrateId);
                        additionalParams.TryGetValue(AdditionalParamKeys.KeySlotId, out var slot);
                        additionalParams.TryGetValue(AdditionalParamKeys.KeyUserId, out var userId);

                        Dictionary<string, string> scenarioParam = new Dictionary<string, string>
                        {
                            [DetachingKeys.KeyParamLotId] = lotId,
                            [DetachingKeys.KeyParamRecipeId] = s.RecipeId,
                            [DetachingKeys.KeyParamWaferId] = substrateId,
                            [DetachingKeys.KeyParamSlotId] = slot,
                            [DetachingKeys.KeyParamOperatorId] = userId
                        };

                        EnqueueScenario(
                                EN_SCENARIO.SCENARIO_CORE_WAFER_DETACH_END,
                                scenarioParam,
                                additionalParams);
                        #endregion
                    }
                    break;

                case EN_SCENARIO.SCENARIO_CHAMBER_START:
                    {
                        // TODO : 에러 로그 필요
                        if (false == additionalParams.TryGetValue(AdditionalParamKeys.KeySubstrateKey, out string substrateKey) ||
                            string.IsNullOrWhiteSpace(substrateKey) ||
                            false == _substrateManager.GetSubstrateByKey(substrateKey, out var s) ||
                            s == null)
                            return;

                        additionalParams.TryGetValue(AdditionalParamKeys.KeyLotId, out var lotId);
                        additionalParams.TryGetValue(AdditionalParamKeys.KeySubstrateId, out var substrateId);
                        additionalParams.TryGetValue(AdditionalParamKeys.KeySlotId, out var slot);
                        additionalParams.TryGetValue(AdditionalParamKeys.KeyUserId, out var userId);

                        // string lotId = substrate.LotId;
                        Dictionary<string, string> scenarioParam = new Dictionary<string, string>
                        {
                            [DetachingKeys.KeyParamLotId] = lotId,
                            [DetachingKeys.KeyParamRecipeId] = s.RecipeId,
                            [DetachingKeys.KeyParamWaferId] = substrateId,
                            [DetachingKeys.KeyParamSlotId] = slot.ToString(),
                            [DetachingKeys.KeyParamOperatorId] = userId
                        };

                        if (_traceDataProvider is IDetachingTraceParameterProvider detachingProvider)
                        {
                            detachingProvider.AppendDetachingTraceParameters(scenarioParam);
                        }

                        EnqueueScenario(EN_SCENARIO.SCENARIO_CORE_WAFER_DETACH_START,
                            scenarioParam,
                            additionalParams);
                    }
                    break;

                case EN_SCENARIO.SCENARIO_CORE_WAFER_DETACH_START:
                case EN_SCENARIO.SCENARIO_CORE_WAFER_DETACH_END:
                case EN_SCENARIO.SCENARIO_BIN_SORTING_START_1:
                case EN_SCENARIO.SCENARIO_BIN_SORTING_START_2:
                case EN_SCENARIO.SCENARIO_BIN_SORTING_START_3:
                    {
                        #region
                        if (false == additionalParams.TryGetValue(AdditionalParamKeys.KeyNameOfEq, out string nameOfEq))
                            return;

                        if (false == additionalParams.TryGetValue(AdditionalParamKeys.KeyMessageNameToSend, out string messageNameToSend))
                            return;

                        ExecuteToSendSimpleResultToClient(result, messageNameToSend, nameOfEq);

                        // DetachStart
                        if (typeOfScenario == EN_SCENARIO.SCENARIO_CORE_WAFER_DETACH_START)
                        {
                            if (false == additionalParams.TryGetValue(
                                AdditionalParamKeys.KeySubstrateKey,
                                out string substrateKey) ||
                                string.IsNullOrWhiteSpace(substrateKey))
                                return;

                            var param = MakeScenarioParamForStepHandling(substrateKey);
                            if (param != null)
                            {
                                EnqueueScenario(
                                    EN_SCENARIO.SCENARIO_STEP_START,
                                    param);
                            }
                        }
                        else if (typeOfScenario == EN_SCENARIO.SCENARIO_CORE_WAFER_DETACH_END)
                        {
                            if (false == additionalParams.TryGetValue(
                                AdditionalParamKeys.KeySubstrateKey,
                                out string substrateKey) ||
                                string.IsNullOrWhiteSpace(substrateKey))
                                return;

                            var scenarioParam = MakeScenarioParamToChamberHandling(substrateKey);
                            if (scenarioParam != null)
                            {
                                EnqueueScenario(EN_SCENARIO.SCENARIO_CHAMBER_END,
                                                                scenarioParam,
                                                                additionalParams);
                            }
                        }
                        #endregion
                    }
                    break;

                case EN_SCENARIO.SCENARIO_BIN_WAFER_ID_READ:
                    {
                        #region
                        if (false == isManual)
                        {
                            if (false == additionalParams.TryGetValue(AdditionalParamKeys.KeyNameOfEq, out string nameOfEq))
                                return;

                            if (false == additionalParams.TryGetValue(AdditionalParamKeys.KeyMessageNameToSend, out string messageNameToSend))
                                return;

                            ExecuteToSendSimpleResultToClient(result, messageNameToSend, nameOfEq);
                        }
                        #endregion
                    }
                    break;

                case EN_SCENARIO.SCENARIO_REQ_CORE_WAFER_ID:
                    {
                        #region
                        if (false == additionalParams.TryGetValue(AdditionalParamKeys.KeyNameOfEq, out string nameOfEq))
                            return;

                        if (false == additionalParams.TryGetValue(AdditionalParamKeys.KeyRingId, out string ringId))
                            return;

                        if (false == additionalParams.TryGetValue(AdditionalParamKeys.KeySubstrateId, out string substrateName))
                            return;

                        if (false == additionalParams.TryGetValue(AdditionalParamKeys.KeyMessageNameToSend, out string messageNameToSend))
                            return;

                        if (result.Equals(EN_MESSAGE_RESULT.NG))
                        {
                            _functionsForPWA500.SetScenarioError(typeOfScenario);
                            ExecuteToSendSimpleResultToClient(EN_MESSAGE_RESULT.NG, messageNameToSend, nameOfEq, "Does not have ring id or substrate name");
                        }
                        else
                        {
                            if (false == resultOfScenario.TryGetValue(AssignSubstrateIdKeys.KeyResultSubstrateId, out string newSubstrateName))
                            {
                                _functionsForPWA500.SetScenarioError(typeOfScenario);
                                ExecuteToSendSimpleResultToClient(EN_MESSAGE_RESULT.NG, messageNameToSend, nameOfEq, "SECS/GEM Scenario Error!");
                            }
                            else
                            {
                                string pmName = ProcessModuleGroup.Instance.GetProcessModuleName(ProcessModuleIndex);
                                List<Substrate> substrates = new List<Substrate>();
                                if (_substrateManager.GetSubstratesAtProcessModule(pmName, ref substrates))
                                {
                                    for (int i = 0; i < substrates.Count; ++i)
                                    {
                                        var name = substrates[i].Name;
                                        if (name.Equals(ringId) || name.Equals(substrateName))
                                        {
                                            _substrateManager.SetAttributeByKey(substrates[i].UniqueKey, PWA500SubstrateAttributes.RingId, substrateName);
                                            _substrateManager.SetNameByKey(substrates[i].UniqueKey, newSubstrateName);
                                            _substrateManager.SaveDataByKey(substrates[i].UniqueKey);

                                            Dictionary<string, string> messageContentToSend = new Dictionary<string, string>
                                            {
                                                [AssignSubstrateIdKeys.KeySubstrateName/*"SubstrateName"*/] = newSubstrateName,
                                                [AssignSubstrateIdKeys.KeyResultRingId] = substrateName
                                            };

                                            SendClientToClientMessage(nameOfEq, messageNameToSend,
                                                string.Empty, string.Empty,
                                                messageContentToSend.Keys.ToArray(), messageContentToSend.Values.ToArray(),
                                                result, true);

                                            return;
                                        }
                                    }
                                }

                                // 통신이 꺼져있고, 자재 정보를 못찾으면
                                var useSecsGem = _recipe.GetValue(EN_RECIPE_TYPE.COMMON, PARAM_COMMON.UseSecsGem.ToString(), true);
                                if (false == useSecsGem)
                                {
                                    Dictionary<string, string> messageContentToSend = new Dictionary<string, string>
                                    {
                                        [AssignSubstrateIdKeys.KeySubstrateName/*"SubstrateName"*/] = newSubstrateName,
                                        [AssignSubstrateIdKeys.KeyResultRingId] = substrateName
                                    };

                                    SendClientToClientMessage(nameOfEq, messageNameToSend,
                                        string.Empty, string.Empty,
                                        messageContentToSend.Keys.ToArray(), messageContentToSend.Values.ToArray(),
                                        result, true);

                                    return;
                                }
                            }
                        }

                        ExecuteToSendSimpleResultToClient(EN_MESSAGE_RESULT.NG, messageNameToSend, nameOfEq, "Does not have ring id or substrate name");
                        #endregion
                    }
                    break;

                case EN_SCENARIO.SCENARIO_CORE_MAP_UPLOAD:
                    {
                        if (additionalParams == null)
                            return;

                        if (false == additionalParams.TryGetValue(AdditionalParamKeys.KeyNameOfEq, out string nameOfEq))
                            return;

                        if (false == additionalParams.TryGetValue(AdditionalParamKeys.KeyMessageNameToSend, out string messageNameToSend))
                            return;

                        string desc = string.Empty;
                        if (result == EN_MESSAGE_RESULT.NG)
                            desc = "GEM Error";

                        ExecuteToSendSimpleResultToClient(
                            result,
                            messageNameToSend,
                            nameOfEq,
                            desc);
                    }
                    break;

                case EN_SCENARIO.SCENARIO_CORE_MAP_DOWNLOAD:
                    {
                        #region
                        Dictionary<string, string> messageContentToSend = new Dictionary<string, string>();
                        messageContentToSend[ResultKeys.KeyResult] = result.ToString();
                        if (result.Equals(EN_MESSAGE_RESULT.OK))
                        {
                            messageContentToSend[ResultKeys.KeyDescription] = string.Empty;
                        }
                        else
                        {
                            messageContentToSend[ResultKeys.KeyDescription] = "Gem Error";
                        }

                        if (false == resultOfScenario.TryGetValue(RequestDownloadMapFileKeys.KeyResultSubstrateId, out string resultSubstrateId))
                        {
                            result = EN_MESSAGE_RESULT.NG;
                            messageContentToSend[ResultKeys.KeyResult] = result.ToString();
                        }

                        if (false == resultOfScenario.TryGetValue(RequestDownloadMapFileKeys.KeyResultCountRow, out string resultCountRow))
                        {
                            result = EN_MESSAGE_RESULT.NG;
                            messageContentToSend[ResultKeys.KeyResult] = result.ToString();
                        }

                        if (false == resultOfScenario.TryGetValue(RequestDownloadMapFileKeys.KeyResultCountCol, out string resultCountCol))
                        {
                            result = EN_MESSAGE_RESULT.NG;
                            messageContentToSend[ResultKeys.KeyResult] = result.ToString();
                        }

                        if (false == resultOfScenario.TryGetValue(RequestDownloadMapFileKeys.KeyResultAngle, out string resultAngle))
                        {
                            result = EN_MESSAGE_RESULT.NG;
                            messageContentToSend[ResultKeys.KeyResult] = result.ToString();
                        }

                        if (false == resultOfScenario.TryGetValue(RequestDownloadMapFileKeys.KeyResultQty, out string resultQty))
                        {
                            result = EN_MESSAGE_RESULT.NG;
                            messageContentToSend[ResultKeys.KeyResult] = result.ToString();
                        }

                        if (false == resultOfScenario.TryGetValue(RequestDownloadMapFileKeys.KeyResultReferenceX, out string resultRefX))
                        {
                            result = EN_MESSAGE_RESULT.NG;
                            messageContentToSend[ResultKeys.KeyResult] = result.ToString();
                        }
                        if (false == resultOfScenario.TryGetValue(RequestDownloadMapFileKeys.KeyResultReferenceY, out string resultRefY))
                        {
                            result = EN_MESSAGE_RESULT.NG;
                            messageContentToSend[ResultKeys.KeyResult] = result.ToString();
                        }
                        if (false == resultOfScenario.TryGetValue(RequestDownloadMapFileKeys.KeyResultStartingX, out string resultStartX))
                        {
                            result = EN_MESSAGE_RESULT.NG;
                            messageContentToSend[ResultKeys.KeyResult] = result.ToString();
                        }
                        if (false == resultOfScenario.TryGetValue(RequestDownloadMapFileKeys.KeyResultStartingY, out string resultStartY))
                        {
                            result = EN_MESSAGE_RESULT.NG;
                            messageContentToSend[ResultKeys.KeyResult] = result.ToString();
                        }


                        if (false == resultOfScenario.TryGetValue(RequestDownloadMapFileKeys.KeyResultMapData, out string resultMapData))
                        {
                            result = EN_MESSAGE_RESULT.NG;
                            messageContentToSend[ResultKeys.KeyResult] = result.ToString();
                        }

                        Substrate substrate;
                        if (isManual)
                        {
                            if (false == _functionsForPWA500.FindSubstrateByNameOrRingIdAtProcessModule(resultSubstrateId, resultSubstrateId, out substrate, out _) || substrate == null)
                                return;

                            _functionsForPWA500.SetSubstrateAttributes(substrate,
                                resultSubstrateId,
                                resultAngle,
                                resultCountRow,
                                resultCountCol,
                                resultQty,
                                resultRefX,
                                resultRefY,
                                resultStartX,
                                resultStartY,
                                resultMapData);
                        }
                        else
                        {
                            if (additionalParams == null)
                                return;

                            if (false == additionalParams.TryGetValue(AdditionalParamKeys.KeySubstrateId, out string substrateId))
                            {
                                result = EN_MESSAGE_RESULT.NG;
                                messageContentToSend[ResultKeys.KeyResult] = result.ToString();
                            }

                            if (false == additionalParams.TryGetValue(AdditionalParamKeys.KeyNameOfEq, out string nameOfEq))
                            {
                                result = EN_MESSAGE_RESULT.NG;
                                messageContentToSend[ResultKeys.KeyResult] = result.ToString();
                            }

                            if (result.Equals(EN_MESSAGE_RESULT.NG))
                            {
                                resultCountRow = "0";
                                resultCountCol = "0";
                                resultAngle = "0";
                                resultQty = "0";
                                resultMapData = string.Empty;
                            }

                            messageContentToSend[RequestDownloadMapFileKeys.KeySubstrateName] = resultSubstrateId;
                            messageContentToSend[RequestDownloadMapFileKeys.KeyCountRow] = resultCountRow;
                            messageContentToSend[RequestDownloadMapFileKeys.KeyCountCol] = resultCountCol;
                            messageContentToSend[RequestDownloadMapFileKeys.KeyWaferAngle] = resultAngle;
                            messageContentToSend[RequestDownloadMapFileKeys.KeyChipQty] = resultQty;
                            messageContentToSend[RequestDownloadMapFileKeys.KeyMapData] = resultMapData;

                            if (_functionsForPWA500.FindSubstrateByNameOrRingIdAtProcessModule(substrateId, substrateId, out substrate, out _))
                            {
                                _functionsForPWA500.SetSubstrateAttributes(substrate,
                                    resultSubstrateId,
                                    resultAngle,
                                    resultCountRow,
                                    resultCountCol,
                                    resultQty,
                                    resultRefX,
                                    resultRefY,
                                    resultStartX,
                                    resultStartY,
                                    resultMapData);

                                SendClientToClientMessage(nameOfEq, MessagesToSend.ResponseDownloadMapFile.ToString(),
                                         string.Empty, string.Empty,
                                         messageContentToSend.Keys.ToArray(), messageContentToSend.Values.ToArray(),
                                         result, true);

                                if (UseCoreMapHandlingOnly || result == EN_MESSAGE_RESULT.NG)
                                    return;
                            }
                            else
                            {
                                // 2025.07.16. jhlim [MOD] 자재 정보가 없는 경우, GEM이 꺼져있으면 다운받은 맵을 넘긴다.
                                var useSecsGem = _recipe.GetValue(EN_RECIPE_TYPE.COMMON, PARAM_COMMON.UseSecsGem.ToString(), true);
                                if (useSecsGem)
                                //if (false == UseCoreMapHandlingOnly)
                                {
                                    // Gem이 켜져 있으면 알람
                                    SendClientToClientMessage(nameOfEq, MessagesToSend.ResponseDownloadMapFile.ToString(),
                                        string.Empty, string.Empty,
                                        messageContentToSend.Keys.ToArray(), messageContentToSend.Values.ToArray(),
                                        EN_MESSAGE_RESULT.NG, true);

                                    // TODO : 알람 발생 필요
                                }
                                else
                                {
                                    // Gem이 꺼져있으면 다운받은 맵을 전달
                                    SendClientToClientMessage(nameOfEq, MessagesToSend.ResponseDownloadMapFile.ToString(),
                                            string.Empty, string.Empty,
                                            messageContentToSend.Keys.ToArray(), messageContentToSend.Values.ToArray(),
                                            result, true);
                                }
                                // 2025.07.16. jhlim [END]

                                // 2024.12.31. jhlim [ADD] NG 시 리턴 누락
                                return;
                            }

                            #region
                            //if (false == additionalParams.TryGetValue(AdditionalParamKeys.KeyRingId, out string ringId))
                            //    return;

                            //if (false == additionalParams.TryGetValue(AdditionalParamKeys.KeyUserId, out string userId))
                            //    return;

                            // 원래 스플릿 이후 할당받은 랏을 공정설비에 넘겨줬으나, GEM300 시나리오에는 없으므로,
                            // 원래 랏을 보내준다.
                            messageContentToSend.Clear();
                            messageContentToSend[AssignSubstrateLotIdKeys.KeySubstrateName] = substrateId;
                            messageContentToSend[AssignSubstrateLotIdKeys.KeyLotId] = substrate.LotId;

                            SendClientToClientMessage(nameOfEq, MessagesToSend.RequestAssignLotId.ToString(),
                                string.Empty, string.Empty,
                                messageContentToSend.Keys.ToArray(), messageContentToSend.Values.ToArray(),
                                result, true);


                            // Work_start 이후 발생하도록 수정 필요 -> ResponseDownloadMapFile 후 WaferSplitEvent 발생하도록 수정 필요
                            //int portId = substrate.SourcePortId;
                            //if (false == _carrierServer.HasCarrier(portId))
                            //    return;

                            //string isLastString = substrate.GetAttribute(PWA500SubstrateAttributes.IsLastSubstrate);
                            //bool.TryParse(isLastString, out bool isLast);
                            //bool isLast = _substrateManager.IsLastSubstrateAtLoadPort(portId, substrateId);
                            //ExecuteScenarioToSplitWafer(nameOfEq, substrate.Name, ringId, userId, isLast);
                            #endregion
                        }

                        #endregion
                    }
                    break;

                case EN_SCENARIO.SCENARIO_BIN_SORTING_END_1:
                case EN_SCENARIO.SCENARIO_BIN_SORTING_END_2:
                case EN_SCENARIO.SCENARIO_BIN_SORTING_END_3:
                    {
                        if (additionalParams == null)
                            return;

                        #region
                        if (false == additionalParams.TryGetValue(AdditionalParamKeys.KeyRingId, out string ringId))
                        {
                            result = EN_MESSAGE_RESULT.NG;
                        }

                        if (false == additionalParams.TryGetValue(AdditionalParamKeys.KeySubstrateType, out string subType))
                        {
                            result = EN_MESSAGE_RESULT.NG;
                        }
                        if (false == Enum.TryParse(subType, out SubstrateType substrateType))
                        {
                            result = EN_MESSAGE_RESULT.NG;
                        }

                        if (false == additionalParams.TryGetValue(AdditionalParamKeys.KeyNameOfEq, out string nameOfEq))
                        {
                            result = EN_MESSAGE_RESULT.NG;
                        }

                        int chipQty = 0;
                        if (false == additionalParams.TryGetValue(AdditionalParamKeys.KeyChipQty, out string qty) ||
                            false == int.TryParse(qty, out chipQty))
                        {
                            result = EN_MESSAGE_RESULT.NG;
                        }

                        string description = string.Empty;
                        if (result == EN_MESSAGE_RESULT.NG)
                        {
                            description = "Gem Error";
                        }

                        Dictionary<string, string> messageContentToSend = new Dictionary<string, string>
                        {
                            [ResultKeys.KeyResult] = result.ToString(),
                            [ResultKeys.KeyDescription] = description,
                        };

                        SendClientToClientMessage(nameOfEq, MessagesToSend.ResponseFinishSorting.ToString(),
                            string.Empty, string.Empty,
                            messageContentToSend.Keys.ToArray(), messageContentToSend.Values.ToArray(),
                            result, true);
                        #endregion
                    }
                    break;

                default:
                    break;
            }
        }
        private Dictionary<string, string> MakeScenarioParamToChamberHandling(string substrateKey)
        {
            var pmName = ProcessModuleGroup.Instance.GetProcessModuleName(ProcessModuleIndex);
            List<Substrate> substrates = new List<Substrate>();
            if (_substrateManager.GetSubstratesAtProcessModule(pmName, ref substrates))
            {
                foreach (var item in substrates)
                {
                    if (string.Equals(item.UniqueKey, substrateKey, StringComparison.OrdinalIgnoreCase))
                    {
                        return MakeScenarioParamToChamberHandling(
                            item.LotId,
                            item.SourceSlot,
                            item.RecipeId);
                    }
                }
            }

            return null;
        }

        private Dictionary<string, string> MakeScenarioParamToChamberHandling(
            string lotId,
            int slot,
            string recipeId)
        {
            Dictionary<string, string> data = new Dictionary<string, string>
            {
                [EN_SVID_LIST.LOTID.ToString()] = lotId,
                [EN_SVID_LIST.SLOTID.ToString()] = slot.ToString(),
                [EN_SVID_LIST.RECIPEID.ToString()] = recipeId,
                [EN_SVID_LIST.UNITID.ToString()] = _functionsForPWA500.GetModelName(),
            };
            return data;
        }
        //private void EnqueueScenario(EN_SCENARIO scenario, Dictionary<string, string> scenarioParams, Dictionary<string, string> additionalParams = null)
        //{
        //    Dictionary<string, string> params2;
        //    if (additionalParams == null)
        //    {
        //        params2 = new Dictionary<string, string>();
        //    }
        //    else
        //    {
        //        params2 = new Dictionary<string, string>(additionalParams);
        //    }

        //    if (scenarioParams == null)
        //    {
        //        scenarioParams = new Dictionary<string, string>();
        //    }

        //    QueuedScenarioInfo scenarioInfo = new QueuedScenarioInfo
        //    {
        //        Scenario = scenario,
        //        ScenarioParams = new Dictionary<string, string>(scenarioParams),
        //        AdditionalParams = params2
        //    };

        //    _queuedScenario.Enqueue(scenarioInfo);
        //}
        private bool ParseMessagesAndAck(string nameOfEq, MessagesToReceive messageName, string scenarioName, Dictionary<string, string> messagePairs, EN_MESSAGE_RESULT result, ref bool useLogging)
        {
            switch (messageName)
            {
                case MessagesToReceive.RequestAssignRingId:
                    {
                        #region
                        if (false == messagePairs.TryGetValue(AssignRingIdKeys.KeyOldRingId, out string oldRingId))
                            return false;
                        if (false == messagePairs.TryGetValue(AssignRingIdKeys.KeyNewRingId, out string newRingId))
                            return false;

                        return ExecuteScenarioToAssignSubstrateRingId(nameOfEq, oldRingId, newRingId, false);
                        #endregion
                    }

                case MessagesToReceive.RequestAssignCoreRingId:
                    {
                        #region
                        if (false == messagePairs.TryGetValue(AssignRingIdKeys.KeyOldRingId, out string oldRingId))
                            return false;
                        if (false == messagePairs.TryGetValue(AssignRingIdKeys.KeyNewRingId, out string newRingId))
                            return false;

                        return ExecuteScenarioToAssignSubstrateRingId(nameOfEq, oldRingId, newRingId, true);
                        #endregion
                    }

                case MessagesToReceive.RequestDownloadMapFile:
                    {
                        #region
                        if (false == messagePairs.TryGetValue(RequestDownloadMapFileKeys.KeySubstrateName, out string substrateName))
                            return false;

                        if (false == messagePairs.TryGetValue(/*"RingId"*/RequestDownloadMapFileKeys.KeyRingId, out string ringId))
                            return false;

                        if (Task.TaskOperator.GetInstance().IsSimulationMode())
                        {
                            return ExecuteScenarioToDownloadMapFile(nameOfEq, substrateName, ringId, 0, " ", "AUTO", true);
                        }

                        if (false == messagePairs.TryGetValue(RequestDownloadMapFileKeys.KeyWaferAngle, out string angle))
                            return false;
                        if (false == double.TryParse(angle, out double waferAngle))
                            return false;
                        if (false == messagePairs.TryGetValue(RequestDownloadMapFileKeys.KeyUserId, out string userId))
                            return false;
                        if (false == messagePairs.TryGetValue(RequestDownloadMapFileKeys.KeyNullBinCode, out string nullBinCode))
                            return false;

                        // 이름의 유효성을 체크한다.
                        if (false == _substrateManager.IsValidSubstrateName(substrateName))
                            return false;

                        bool useEventHandling = !UseCoreMapHandlingOnly;
                        return ExecuteScenarioToDownloadMapFile(nameOfEq, substrateName, ringId, waferAngle, nullBinCode, userId, useEventHandling);
                        #endregion
                    }

                case MessagesToReceive.RequestUploadRecipe:
                    {
                        #region
                        if (false == messagePairs.TryGetValue(RecipeHandlingKeys.KeyRecipeId, out string recipeId))
                            return false;

                        #region <업로드 시나리오 비동기 실행>
                        var scenario = EN_SCENARIO.SCENARIO_RECIPE_DOWNLOAD_BY_HOST;
                        var paramList = GetScenarioParameterList(scenario);
                        if (paramList == null)
                            return false;

                        Dictionary<string, string> paramsToUpdate = new Dictionary<string, string>();
                        for (int i = 0; i < paramList.Count; ++i)
                        {
                            string paramName = paramList[i];
                            string paramValue = string.Empty;
                            if (paramName.Equals(RecipeHandlingKeys.KeyParamRecipeId))
                            {
                                paramValue = recipeId;
                            }
                            else if (paramName.Equals(RecipeHandlingKeys.KeyUseCommunicationToPM))
                            {
                                paramValue = bool.TrueString;
                            }

                            paramsToUpdate[paramName] = paramValue;
                        }

                        EnqueueScenario(scenario, paramsToUpdate);

                        return true;
                        #endregion </업로드 시나리오 비동기 실행>
                        #endregion
                    }

                case MessagesToReceive.RequestStartDetaching:
                    {
                        #region 
                        string substrateName = string.Empty, ringId = string.Empty, recipeId = string.Empty, userId = string.Empty;
                        if (false == messagePairs.TryGetValue(DetachingKeys.KeySubstarateName, out substrateName))
                            return false;

                        if (false == messagePairs.TryGetValue(DetachingKeys.KeyRingId, out ringId))
                            return false;

                        if (false == messagePairs.TryGetValue(DetachingKeys.KeyRecipeId, out recipeId))
                            return false;

                        if (false == messagePairs.TryGetValue(DetachingKeys.KeyUserId, out userId))
                            return false;

                        if (false == _functionsForPWA500.FindSubstrateByNameOrRingIdAtProcessModule(substrateName, ringId, out var substrate, out var description) && false == UseCoreMapHandlingOnly)
                        {
                            WriteLog(description);

                            Dictionary<string, string> messageContentToSend = new Dictionary<string, string>
                            {
                                [ResultKeys.KeyResult] = EN_MESSAGE_RESULT.NG.ToString(),
                                [ResultKeys.KeyDescription] = string.Format("Cannot find substrate by ring id : {0}", ringId)
                            };

                            return SendClientToClientMessage(nameOfEq, MessagesToSend.ResponseStartDetaching.ToString(),
                                        string.Empty, string.Empty,
                                        messageContentToSend.Keys.ToArray(), messageContentToSend.Values.ToArray(),
                                        result, true);
                        }

                        int portId = substrate.SourcePortId;
                        int slot = substrate.SourceSlot;

                        string carrierId = _carrierServer.GetCarrierId(portId);
                        _lotHistoryLog.WriteSubstrateHistoryForStartOrFinishDetaching(portId, carrierId, substrateName, true);
                        _substrateManager.SetProcessingStatusByKey(substrate.UniqueKey, ProcessingStates.InProcess);
                        _substrateManager.SaveDataByKey(substrate.UniqueKey);
                        if (UseCoreMapHandlingOnly)
                        {
                            ExecuteToSendSimpleResultToClient(EN_MESSAGE_RESULT.OK, MessagesToSend.ResponseStartDetaching.ToString(), nameOfEq);
                            return true;
                        }

                        // Chamber Start
                        var scenarioParam = MakeScenarioParamToChamberHandling(
                            substrate.LotId,
                            substrate.SourceSlot,
                            substrate.RecipeId);

                        string lotId = substrate.LotId;
                        Dictionary<string, string> additionalParams = new Dictionary<string, string>
                        {
                            [AdditionalParamKeys.KeyNameOfEq] = nameOfEq,
                            [AdditionalParamKeys.KeyMessageNameToSend] = MessagesToSend.ResponseStartDetaching.ToString(),
                            [AdditionalParamKeys.KeySubstrateKey] = substrate.UniqueKey,
                            [AdditionalParamKeys.KeyLotId] = lotId,
                            [AdditionalParamKeys.KeySubstrateId] = substrateName,
                            [AdditionalParamKeys.KeySlotId] = slot.ToString(),
                            [AdditionalParamKeys.KeyUserId] = userId
                        };

                        EnqueueScenario(EN_SCENARIO.SCENARIO_CHAMBER_START,
                            scenarioParam,
                            additionalParams);


                        //string lotId = substrate.LotId;
                        //Dictionary<string, string> scenarioParam = new Dictionary<string, string>
                        //{
                        //    [DetachingKeys.KeyParamLotId] = lotId,
                        //    [DetachingKeys.KeyParamRecipeId] = EquipmentInfo.GetRecipeId(),
                        //    [DetachingKeys.KeyParamWaferId] = substrateName,
                        //    [DetachingKeys.KeyParamSlotId] = slot.ToString(),
                        //    [DetachingKeys.KeyParamOperatorId] = userId
                        //};

                        //if (_traceDataProvider is IDetachingTraceParameterProvider detachingProvider)
                        //{
                        //    detachingProvider.AppendDetachingTraceParameters(scenarioParam);
                        //}

                        //Dictionary<string, string> additionalParams = new Dictionary<string, string>
                        //{
                        //    [AdditionalParamKeys.KeyNameOfEq] = nameOfEq,
                        //    [AdditionalParamKeys.KeyMessageNameToSend] = MessagesToSend.ResponseStartDetaching.ToString(),
                        //    [AdditionalParamKeys.KeySubstrateKey] = substrate.UniqueKey
                        //};

                        //EnqueueScenario(EN_SCENARIO.SCENARIO_CORE_WAFER_DETACH_START,
                        //    scenarioParam, 
                        //    additionalParams);
                        #endregion

                        return true;
                    }

                case MessagesToReceive.RequestFinishDetaching:
                    {
                        #region 
                        if (false == messagePairs.TryGetValue(DetachingKeys.KeySubstarateName, out string substrateName))
                            return false;

                        if (false == messagePairs.TryGetValue(DetachingKeys.KeyRingId, out string ringId))
                            return false;

                        if (false == messagePairs.TryGetValue(DetachingKeys.KeyRecipeId, out string recipeId))
                            return false;

                        if (false == messagePairs.TryGetValue(DetachingKeys.KeyUserId, out string userId))
                            return false;

                        if (false == _functionsForPWA500.FindSubstrateByNameOrRingIdAtProcessModule(substrateName, ringId, out var substrate, out var description))
                        {
                            WriteLog(description);
                            return false;
                        }

                        int portId = substrate.SourcePortId;
                        int slot = substrate.SourceSlot;
                        string carrierId = _carrierServer.GetCarrierId(portId);
                        _lotHistoryLog.WriteSubstrateHistoryForStartOrFinishDetaching(portId, carrierId, substrateName, false);

                        //substrate.SetProcessingStatus(EFEM.Defines.MaterialTracking.ProcessingStates.Processed);
                        _substrateManager.SetProcessingStatusByKey(substrate.UniqueKey, ProcessingStates.Processed);
                        _substrateManager.SaveDataByKey(substrate.UniqueKey);
                        if (UseCoreMapHandlingOnly)
                        {
                            ExecuteToSendSimpleResultToClient(EN_MESSAGE_RESULT.OK, MessagesToSend.ResponseFinishDetaching.ToString(), nameOfEq);
                            return true;
                        }

                        // StepEnd
                        var param = MakeScenarioParamForStepHandling(substrate.UniqueKey);
                        if (param != null)
                        {
                            Dictionary<string, string> additional = new Dictionary<string, string>();
                            additional[AdditionalParamKeys.KeyLotId] = substrate.LotId;
                            additional[AdditionalParamKeys.KeySlotId] = substrate.SourceSlot.ToString();
                            additional[AdditionalParamKeys.KeySubstrateId] = substrateName;
                            additional[AdditionalParamKeys.KeySubstrateKey] = substrate.UniqueKey;
                            additional[AdditionalParamKeys.KeyNameOfEq] = nameOfEq;
                            additional[AdditionalParamKeys.KeyUserId] = userId;
                            additional[AdditionalParamKeys.KeyMessageNameToSend] = MessagesToSend.ResponseFinishDetaching.ToString();

                            EnqueueScenario(
                                EN_SCENARIO.SCENARIO_STEP_END,
                                param,
                                additional);
                            return true;
                        }
                        
                        return false;
                        #endregion
                    }

                case MessagesToReceive.RequestStartSorting:
                    {
                        #region 
                        if (false == messagePairs.TryGetValue(SortingKeys.KeyRingId, out string ringId))
                            return false;

                        if (false == messagePairs.TryGetValue(SortingKeys.KeyRecipeId, out string recipeId))
                            return false;

                        if (false == messagePairs.TryGetValue(SortingKeys.KeySubstrateType, out string subType))
                            return false;
                        //if (false == Enum.TryParse(subType, out SubstrateType substrateType))
                        //    return false;

                        if (false == messagePairs.TryGetValue(SortingKeys.KeyBinCode, out string binCode))
                            return false;

                        if (false == _functionsForPWA500.FindSubstrateByNameOrRingIdAtProcessModule(ringId, ringId, out var substrate, out var description) && false == UseCoreMapHandlingOnly)
                        {
                            WriteLog(description);

                            Dictionary<string, string> messageContentToSend = new Dictionary<string, string>
                            {
                                [ResultKeys.KeyResult] = EN_MESSAGE_RESULT.NG.ToString(),
                                [ResultKeys.KeyDescription] = string.Format("Cannot find substrate by ring id : {0}", ringId)
                            };

                            return SendClientToClientMessage(nameOfEq, MessagesToSend.ResponseStartSorting.ToString(),
                                        string.Empty, string.Empty,
                                        messageContentToSend.Keys.ToArray(), messageContentToSend.Values.ToArray(),
                                        result, true);
                        }

                        int portId = substrate.SourcePortId;
                        _lotHistoryLog.WriteSubstrateHistoryForStartSorting(portId, ringId);

                        //substrate.SetProcessingStatus(EFEM.Defines.MaterialTracking.ProcessingStates.InProcess);
                        _substrateManager.SetProcessingStatusByKey(substrate.UniqueKey, ProcessingStates.InProcess);
                        if (UseCoreMapHandlingOnly)
                        {
                            ExecuteToSendSimpleResultToClient(EN_MESSAGE_RESULT.OK, MessagesToSend.ResponseStartSorting.ToString(), nameOfEq);
                            return true;
                        }

                        string carrierId = substrate.SourceCarrierId;
                        Dictionary<string, string> scenarioParam = new Dictionary<string, string>
                        {
                            [SortingKeys.KeyParamCarrierId] = carrierId,
                            [SortingKeys.KeyParamBinType] = binCode,
                            [SortingKeys.KeyParamRingFrameId] = ringId,
                            // TODO : 빈소터와의 운영상 차이점 -> 아래 두개는 가치효율 데이터인데, W는 아직 없다..
                            //[SortingKeys.KeyParamCoreLotId] = coreLotId,
                            //[SortingKeys.KeyParamCorePartId] = corePartId,
                        };
                        // 2025.05.08. jhlim [END]

                        EN_SCENARIO scenario = EN_SCENARIO.SCENARIO_BIN_SORTING_START_1;
                        //switch (substrateType)
                        //{
                        //    case SubstrateType.Bin1:
                        //        scenario = EN_SCENARIO.SCENARIO_BIN_SORTING_START_1;
                        //        break;
                        //    case SubstrateType.Bin2:
                        //        scenario = EN_SCENARIO.SCENARIO_BIN_SORTING_START_2;
                        //        break;
                        //    case SubstrateType.Bin3:
                        //        scenario = EN_SCENARIO.SCENARIO_BIN_SORTING_START_3;
                        //        break;
                        //    default:
                        //        return false;
                        //}

                        _substrateManager.SetAttributeByKey(substrate.UniqueKey, PWA500SubstrateAttributes.SubstrateType, SubstrateType.Bin1.ToString());
                        _substrateManager.SetAttributeByKey(substrate.UniqueKey, PWA500SubstrateAttributes.BinCode, binCode);
                        //_substrateManager.SetAttributesByKey(substrate.UniqueKey, new Dictionary<string, string>
                        //{
                        //    [PWA500SubstrateAttributes.SubstrateType] = SubstrateType.Bin1.ToString(),
                        //    [PWA500SubstrateAttributes.BinCode] = binCode,
                        //});

                        _substrateManager.SaveDataByKey(substrate.UniqueKey);

                        return ExecuteSimpleScenarioAndSendClientMessage(scenario, scenarioParam, nameOfEq, MessagesToSend.ResponseStartSorting.ToString());

                        //Dictionary<string, string> messageContentToSend = new Dictionary<string, string>
                        //{
                        //    [ResultKeys.KeyResult] = EN_MESSAGE_RESULT.OK.ToString(),
                        //    [ResultKeys.KeyDescription] = string.Empty,
                        //};

                        //return SendClientToClientMessage(nameOfEq, MessagesToSend.ResponseStartSorting.ToString(),
                        //            string.Empty, string.Empty,
                        //            messageContentToSend.Keys.ToArray(), messageContentToSend.Values.ToArray(),
                        //            result, true);
                        #endregion
                    }

                case MessagesToReceive.RequestFinishSorting:
                    {
                        #region 
                        if (false == messagePairs.TryGetValue(SortingKeys.KeyRingId, out string ringId))
                            return false;
                        if (false == messagePairs.TryGetValue(SortingKeys.KeyRecipeId, out string recipeId))
                            return false;
                        if (false == messagePairs.TryGetValue(SortingKeys.KeySubstrateType, out string subType))
                            return false;
                        if (false == Enum.TryParse(subType, out SubstrateType substrateType))
                            return false;
                        if (false == messagePairs.TryGetValue(SortingKeys.KeyBinCode, out string binCode))
                            return false;
                        if (false == messagePairs.TryGetValue(SortingKeys.KeyChipQty, out string qty))
                            return false;
                        if (false == int.TryParse(qty, out int chipQty))
                            return false;

                        if (false == _functionsForPWA500.FindSubstrateByNameOrRingIdAtProcessModule(ringId, ringId, out var substrate, out var description))
                        {
                            WriteLog(description);
                            return false;
                        }

                        // 2025.01.02. jhlim [DEL] 공테이프는 캐리어가 없을 수도 있다. -> 나간 시점
                        //int slot = substrate.SourceSlot;
                        //if (portId <= 0 || slot < 0)
                        //    return false;

                        //if (false == _carrierServer.HasCarrier(portId))
                        //    return false;

                        int portId = substrate.SourcePortId;
                        string lotId = substrate.LotId;
                        string parentLotId = substrate.GetAttribute(PWA500SubstrateAttributes.ParentLotId);
                        _lotHistoryLog.WriteSubstrateHistoryForFinishSorting(portId, ringId, lotId, parentLotId);

                        string coreLotId = substrate.GetAttribute(PWA500SubstrateAttributes.CoreLotId);
                        string corePartId = substrate.GetAttribute(PWA500SubstrateAttributes.CorePartId);
                        //substrate.SetProcessingStatus(EFEM.Defines.MaterialTracking.ProcessingStates.Processed);
                        _substrateManager.SetProcessingStatusByKey(substrate.UniqueKey, ProcessingStates.Processed);
                        if (UseCoreMapHandlingOnly)
                        {
                            ExecuteToSendSimpleResultToClient(EN_MESSAGE_RESULT.OK, MessagesToSend.ResponseFinishSorting.ToString(), nameOfEq);
                            return true;
                        }

                        // 2025.05.08. jhlim [MOD] 가치효율 관련 코어 랏, 파트 정보 적용
                        string carrierId = substrate.SourceCarrierId;
                        Dictionary<string, string> scenarioParam = new Dictionary<string, string>
                        {
                            [SortingKeys.KeyParamCarrierId] = carrierId,
                            [SortingKeys.KeyParamBinType] = binCode,
                            [SortingKeys.KeyParamRingFrameId] = ringId,
                            [SortingKeys.KeyParamChipQty] = qty,
                            //[SortingKeys.KeyParamParentLotId] = parentLotId,
                            // TODO : 빈소터와의 운영상 차이점 -> 아래 두개는 가치효율 데이터인데, W는 아직 없다..
                            //[SortingKeys.KeyParamCoreLotId] = coreLotId,
                            //[SortingKeys.KeyParamCorePartId] = corePartId,
                        };
                        // 2025.05.08. jhlim [MOD]

                        EN_SCENARIO scenario = EN_SCENARIO.SCENARIO_BIN_SORTING_END_1;
                        //switch (substrateType)
                        //{
                        //    case SubstrateType.Bin1:
                        //        scenario = EN_SCENARIO.SCENARIO_BIN_SORTING_END_1;
                        //        break;
                        //    case SubstrateType.Bin2:
                        //        scenario = EN_SCENARIO.SCENARIO_BIN_SORTING_END_2;
                        //        break;
                        //    case SubstrateType.Bin3:
                        //        scenario = EN_SCENARIO.SCENARIO_BIN_SORTING_END_3;
                        //        break;
                        //    default:
                        //        return false;
                        //}

                        Dictionary<string, string> additionalParams = new Dictionary<string, string>
                        {
                            [AdditionalParamKeys.KeyNameOfEq] = nameOfEq,
                            [AdditionalParamKeys.KeyRingId] = ringId,
                            [AdditionalParamKeys.KeySubstrateType] = subType,
                            [AdditionalParamKeys.KeyChipQty] = qty
                        };

                        _substrateManager.SetAttributeByKey(substrate.UniqueKey, PWA500SubstrateAttributes.SubstrateType, substrateType.ToString());
                        _substrateManager.SetAttributeByKey(substrate.UniqueKey, PWA500SubstrateAttributes.ChipQty, qty.ToString());
                        _substrateManager.SetAttributeByKey(substrate.UniqueKey, PWA500SubstrateAttributes.BinCode, binCode);

                        //_substrateManager.SetAttributesByKey(substrate.UniqueKey, new Dictionary<string, string>
                        //{
                        //    [PWA500SubstrateAttributes.SubstrateType] = substrateType.ToString(),
                        //    [PWA500SubstrateAttributes.ChipQty] = qty.ToString(),
                        //    [PWA500SubstrateAttributes.BinCode] = binCode,

                        //});

                        _substrateManager.SaveDataByKey(substrate.UniqueKey);

                        EnqueueScenario(scenario, scenarioParam, additionalParams);

                        return true;
                        #endregion
                    }

                case MessagesToReceive.RequestSplitCoreChip:
                    {
                        // 2024.08.18 : [START] 코어맵 핸들링만 사용하는 경우 이후 시나리오를 무시한다.
                        if (UseCoreMapHandlingOnly)
                        {
                            ExecuteToSendSimpleResultToClient(EN_MESSAGE_RESULT.OK, MessagesToSend.ResponseSplitCoreChip.ToString(), nameOfEq);
                            return true;
                        }
                        // [END]

                        #region 
                        if (false == messagePairs.TryGetValue(SplitCoreChipKeys.KeyCoreSubstrateName,
                            out string coreSubstrateName))
                            return false;
                        if (false == messagePairs.TryGetValue(/*"BinRingId"*/SplitCoreChipKeys.KeyBinRingId,
                            out string ringId))
                            return false;
                        if (false == messagePairs.TryGetValue(SplitCoreChipKeys.KeySubstrateType, out string subType))
                            return false;
                        if (false == Enum.TryParse(subType, out SubstrateType substrateType))
                            return false;
                        if (false == messagePairs.TryGetValue(SplitCoreChipKeys.KeyRecipeId,
                            out string recipeId))
                            return false;
                        if (false == messagePairs.TryGetValue(SplitCoreChipKeys.KeySplitQty, out string qty))
                            return false;
                        if (false == int.TryParse(qty, out int splitQty))
                            return false;

                        // 보내준 데이터에 잔여 수량이 있는 경우(없는 경우는 본설비 업데이트 이전) 잔여 수량이 0이면 Full Split 시나리오 실행,
                        // 데이터가 없거나, 잔여 수량이 0이 아니면(파싱 실패 포함) 기본 스플릿 시나리오 실행
                        bool isSplittedFully = false;
                        if (messagePairs.TryGetValue(SplitCoreChipKeys.KeyRemainingChips, out string remainingChipsString))
                        {
                            if (int.TryParse(remainingChipsString, out int remainingChips))
                            {
                                if (remainingChips <= 0)
                                {
                                    isSplittedFully = true;
                                }
                            }
                        }

                        if (false == messagePairs.TryGetValue(SplitCoreChipKeys.KeyIsFirstSorting, out string firstSortingFlag))
                            return false;
                        if (false == bool.TryParse(firstSortingFlag, out bool isFirstSorting))
                            return false;
                        if (false == messagePairs.TryGetValue(SplitCoreChipKeys.KeyUserId, out string userId))
                            return false;
                        if (false == messagePairs.TryGetValue(SplitCoreChipKeys.KeyBinCode, out string binCode))
                            return false;

                        if (
                            _functionsForPWA500.FindSubstrateByNameOrRingIdAtProcessModule(
                                coreSubstrateName, 
                                coreSubstrateName,
                                out var coreSubstrate,  out _) &&
                                coreSubstrate != null &&
                            _functionsForPWA500.FindSubstrateByNameOrRingIdAtProcessModule(
                                ringId,
                                ringId,
                                out var binSubstrate, out _) &&
                                binSubstrate != null)
                        {
                            int corePortId = coreSubstrate.SourcePortId;
                            int binPortId = binSubstrate.SourcePortId;

                            bool splitFirst = isFirstSorting;
                            bool splitFully = isSplittedFully;

                            string carrierId = _carrierServer.GetCarrierId(corePortId);
                            string lotId = coreSubstrate.LotId;
                            string historyForCore = $"{lotId}:{coreSubstrateName}:{qty}";
                            var prevHistory = _substrateManager.GetAttributeByKey(binSubstrate.UniqueKey, PWA500SubstrateAttributes.SplittedHistory);
                            if (string.IsNullOrWhiteSpace(prevHistory))
                            {
                                _substrateManager.SetAttributeByKey(binSubstrate.UniqueKey, PWA500SubstrateAttributes.SplittedHistory, historyForCore);
                            }
                            else
                            {
                                _substrateManager.SetAttributeByKey(binSubstrate.UniqueKey, PWA500SubstrateAttributes.SplittedHistory, $"{prevHistory},{historyForCore}");
                            }
                            binSubstrate.RecipeId = recipeId;
                            _substrateManager.SaveDataByKey(binSubstrate.UniqueKey);

                            _lotHistoryLog.WriteSubstrateHistoryForChipSplit(corePortId, carrierId, coreSubstrateName, binPortId, ringId, qty, binCode, lotId, splitFirst, splitFully);
                        }
                            
                        // TODO : 칩 정보를 자재에 반영 필요
                        return ExecuteToSendSimpleResultToClient(EN_MESSAGE_RESULT.OK, MessagesToSend.ResponseSplitCoreChip.ToString(), nameOfEq);
                        #endregion
                    }

                case MessagesToReceive.RequestUploadCoreFile:
                    {
                        #region
                        if (false == messagePairs.TryGetValue(UploadCoreOrBinFileKeys.KeySubstrateName, out string substrateName))
                            return false;
                        if (false == messagePairs.TryGetValue(UploadCoreOrBinFileKeys.KeyRingId, out string ringId))
                            return false;
                        if (false == messagePairs.TryGetValue(UploadCoreOrBinFileKeys.KeyRecipeId, out string recipeId))
                            return false;
                        //if (false == messagePairs.TryGetValue(UploadCoreOrBinFileKeys.KeyPMSBody, out string pmsBody))
                        //    return false;
                        if (false == messagePairs.TryGetValue(UploadCoreOrBinFileKeys.KeyChipQty, out string qty))
                            return false;
                        if (false == int.TryParse(qty, out int chipQty))
                            return false;
                        if (false == messagePairs.TryGetValue(UploadCoreOrBinFileKeys.KeyCountRow, out string row))
                            return false;
                        if (false == int.TryParse(row, out int countRow))
                            return false;
                        if (false == messagePairs.TryGetValue(UploadCoreOrBinFileKeys.KeyCountCol, out string col))
                            return false;
                        if (false == int.TryParse(col, out int countCol))
                            return false;
                        if (false == messagePairs.TryGetValue(UploadCoreOrBinFileKeys.KeyWaferAngle, out string angle))
                            return false;
                        if (false == double.TryParse(angle, out double waferAngle))
                            return false;
                        if (false == messagePairs.TryGetValue(UploadCoreOrBinFileKeys.KeyNullBinCode, out string nullBinCode))
                            return false;
                        if (false == messagePairs.TryGetValue(UploadCoreOrBinFileKeys.KeyMapData, out string mapData))
                            return false;
                        if (false == messagePairs.TryGetValue(UploadCoreOrBinFileKeys.KeyUserId, out string userId))
                            return false;

                        // 길이 비교
                        int count = countRow * countCol;
                        if (mapData.Length != count)
                        {
                            Dictionary<string, string> messageContentToSend = new Dictionary<string, string>
                            {
                                [ResultKeys.KeyResult] = EN_MESSAGE_RESULT.NG.ToString(),
                                [ResultKeys.KeyDescription] = string.Format("Invalid Length : Row:{0}, Col{1}, DataLength:{2}", countRow, countCol, mapData.Length)
                            };

                            return SendClientToClientMessage(nameOfEq, MessagesToSend.ResponseUploadCoreFile.ToString(),
                                        string.Empty, string.Empty,
                                        messageContentToSend.Keys.ToArray(), messageContentToSend.Values.ToArray(),
                                        result, true);
                        }
                        //

                        bool useEventHandling = !UseCoreMapHandlingOnly;
                        return ExecuteScenarioToWorkEnd(nameOfEq, substrateName, ringId, chipQty, waferAngle, countRow, countCol, nullBinCode, mapData, userId, true, useEventHandling);
                        #endregion
                    }

                case MessagesToReceive.RequestUploadScrapInfo:
                    {
                    }
                    return false;

                default:
                    return false;
            }

            //return false;
        }
        private bool ParseMessages(string nameOfEq, MessagesToReceive messageName, string scenarioName, Dictionary<string, string> messagePairs, EN_MESSAGE_RESULT result, ref bool useLogging)
        {
            switch (messageName)
            {
                case MessagesToReceive.RequestUpdateEquipmentData:
                    {
                        #region
                        //Dictionary<int, Dictionary<string, string>> ecidToUpdate = new Dictionary<int, Dictionary<string, string>>();

                        //int count = 0;
                        //int key = 0;
                        //ecidToUpdate[key] = new Dictionary<string, string>();
                        //foreach (var item in messagePairs)
                        //{
                        //    if (++count > 2)
                        //    {
                        //        count = 0;
                        //        ++key;
                        //        ecidToUpdate[key] = new Dictionary<string, string>();
                        //    }

                        //    if (EquipmentConstantList.TryGetValue(item.Key, out _))
                        //    {
                        //        ecidToUpdate[key][item.Key] = item.Value;
                        //    }
                        //}

                        //if (ecidToUpdate.Count > 0)
                        //{
                        //    foreach (var item in ecidToUpdate)
                        //    {
                        //        UpdateEquipmentConstants(item.Value.Keys.ToArray(), item.Value.Values.ToArray());
                        //    }
                        //    useLogging = false;
                        //}
                        //else
                        //{
                        //    useLogging = true;
                        //}

                        //return (ecidToUpdate.Count > 0);
                        _ecidToUpdate.Clear();
                        foreach (var item in messagePairs)
                        {
                            if (EquipmentConstantList.ContainsKey(item.Key))
                            {
                                if (false == EquipmentConstantList[item.Key].Value.Equals(item.Value))
                                {
                                    EquipmentConstantList[item.Key].Value = item.Value;
                                    _ecidToUpdate[item.Key] = item.Value;
                                }
                            }
                        }

                        useLogging = false;
                        if (_ecidToUpdate.Count > 0)
                        {
                            UpdateEquipmentConstants(_ecidToUpdate.Keys.ToArray(), _ecidToUpdate.Values.ToArray());
                        }

                        return true;
                        #endregion
                    }

                case MessagesToReceive.RequestUpdateTraceData:
                    {
                        useLogging = false;

                        if (_traceDataProvider != null)
                        {
                            _traceDataProvider.TryApplyExternalTraceData(messagePairs);
                        }
                    }
                    break;

                case MessagesToReceive.RequestUpdateEquipmentState:
                    {
                        #region
                        // Status
                        if (false == messagePairs.TryGetValue(MachineInfoKeys.KeyEquipmentState, out string status))
                            return false;
                        if (false == Enum.TryParse(status, out EquipmentState_.EQUIPMENT_STATE equipmentStatus))
                            return false;

                        var prevEquipmentStatus = _processModuleGroup.GetEquipmentState(ProcessModuleIndex);
                        switch (equipmentStatus)
                        {
                            case EquipmentState_.EQUIPMENT_STATE.IDLE:
                                {
                                    // Finishing to Idle, Executing(씹힌 경우) to Idle이면 정지 이벤트 발생
                                    if (prevEquipmentStatus.Equals(EquipmentState_.EQUIPMENT_STATE.FINISHING) ||
                                        prevEquipmentStatus.Equals(EquipmentState_.EQUIPMENT_STATE.EXECUTING))
                                    {
                                        var param = new Dictionary<string, string>();
                                        EnqueueScenario(EN_SCENARIO.SCENARIO_EQUIPMENT_END, param, null);
                                    }
                                }
                                break;
                            case EquipmentState_.EQUIPMENT_STATE.EXECUTING:
                                {
                                    // Ready to Executing, Idle to Executing(씹힌 경우) 시작 이벤트 발생
                                    if (prevEquipmentStatus.Equals(EquipmentState_.EQUIPMENT_STATE.IDLE) ||
                                        prevEquipmentStatus.Equals(EquipmentState_.EQUIPMENT_STATE.READY))
                                    {
                                        var param = new Dictionary<string, string>();
                                        EnqueueScenario(EN_SCENARIO.SCENARIO_EQUIPMENT_START, param, null);
                                    }
                                }
                                break;

                            default:
                                break;
                        }

                        _processModuleGroup.SetEquipmentState(ProcessModuleIndex, equipmentStatus);

                        // Recipe
                        if (false == messagePairs.TryGetValue(MachineInfoKeys.KeyRecipeId, out string recipeId))
                            return false;
                        _processModuleGroup.SetRecipeId(ProcessModuleIndex, recipeId);

                        useLogging = false;
                        return true;
                        #endregion
                    }

                case MessagesToReceive.RequestNotifyAlarmStatus:
                    {
                        if (false == Recipe.Recipe.GetInstance().GetValue(EN_RECIPE_TYPE.COMMON, PARAM_COMMON.UseSecsGem.ToString(), false))
                            return true;

                        #region
                        if (_queuedAlarmsFromPM.Count == 0)
                        {
                            var param = ScenarioParameterBuilder.MakeParamToEquipmentStatus(_functionsForPWA500.GetSubstrateTypeByLoadPortIndex);
                            EnqueueScenario(EN_SCENARIO.SCENARIO_ERROR_START, param, null);
                        }

                        foreach (var item in messagePairs)
                        {
                            string alidString = item.Key;
                            string statusString = item.Value;

                            if (false == int.TryParse(alidString, out int alid))
                                continue;

                            if (false == Enum.TryParse(statusString, out EN_GEM_ALARM_STATE status))
                                continue;

                            int alarmId = alid + AlarmOffset;
                            base.ExecuteReportAlarm(alid + AlarmOffset, status);

                            switch (status)
                            {
                                case EN_GEM_ALARM_STATE.CLEARED:
                                    _queuedAlarmsFromPM.Enqueue(alarmId);
                                    break;
                                case EN_GEM_ALARM_STATE.OCCURED:
                                    _queuedAlarmsFromPM.TryDequeue(out _);
                                    break;
                            }
                        }

                        if (_queuedAlarmsFromPM.Count == 0)
                        {
                            var param = ScenarioParameterBuilder.MakeParamToEquipmentStatus(_functionsForPWA500.GetSubstrateTypeByLoadPortIndex);
                            EnqueueScenario(EN_SCENARIO.SCENARIO_ERROR_STOP, param, null);
                        }

                        // Id
                        //if (false == messagePairs.TryGetValue(NotifyAlarmKeys.KeyAlarmId, out string id))
                        //    return false;
                        //if (false == int.TryParse(id, out int alarmId))
                        //    return false;

                        //// Status
                        //if (false == messagePairs.TryGetValue(NotifyAlarmKeys.KeyStatus, out string status))
                        //    return false;
                        //if (false == int.TryParse(status, out int alarmStatus))
                        //    return false;
                        //if (false == Enum.IsDefined(typeof(EN_GEM_ALARM_STATE), alarmStatus))
                        //    return false;


                        return true;
                        #endregion
                    }

                case MessagesToReceive.ResponseDownloadRecipe:
                    {
                        //bool scenarioPermission = true;
                        //if (false == messagePairs.TryGetValue(ResultKeys.KeyResult, out string resultFromClient) ||
                        //    false == messagePairs.TryGetValue(ResultKeys.KeyDescription, out _))
                        //    scenarioPermission = false;

                        //if (resultFromClient.Equals(EN_MESSAGE_RESULT.OK.ToString()))
                        //    scenarioPermission = true;
                        //else
                        //    scenarioPermission = false;

                        //if (IsScenarioRunning(EN_SCENARIO.SCENARIO_REQ_RECIPE_DOWNLOAD))
                        //{
                        //    UpdateScenarioPermission(EN_SCENARIO.SCENARIO_REQ_RECIPE_DOWNLOAD, scenarioPermission);
                        //}

                        //return scenarioPermission;

                       bool scenarioPermission = true;
                        if (false == messagePairs.TryGetValue(ResultKeys.KeyResult, out string resultFromClient) ||
                            false == messagePairs.TryGetValue(ResultKeys.KeyDescription, out _))
                            scenarioPermission = false;

                        if (resultFromClient.Equals(EN_MESSAGE_RESULT.OK.ToString()))
                            scenarioPermission = true;
                        else
                            scenarioPermission = false;

                        _isReceived = scenarioPermission;

                        return scenarioPermission;
                    }

                case MessagesToReceive.ResponseUploadRecipe:
                    {
                        _recipePathToUploadForPM = string.Empty;
                        messagePairs.TryGetValue(ResultKeys.KeyResult, out string resultMessage);
                        result = resultMessage.Equals(EN_MESSAGE_RESULT.OK.ToString()) ? EN_MESSAGE_RESULT.OK : EN_MESSAGE_RESULT.NG;

                        bool scenarioPermission = result.Equals(EN_MESSAGE_RESULT.OK) ? true : false;

                        if (false == messagePairs.TryGetValue(RecipeHandlingKeys.KeyRecipeId, out string recipeId))
                            scenarioPermission = false;

                        if (false == messagePairs.TryGetValue(RecipeHandlingKeys.KeyRecipeBody, out string recipeBody))
                            scenarioPermission = false;

                        _recipeBody = recipeBody;
                        _isReceived = scenarioPermission;

                        scenarioPermission = true;
                        //if (scenarioPermission)
                        //{
                        //    //string pathToWrite = string.Format(@"{0}\Upload\{1}\{2}{3}", _recipePath, recipeId, NameOfPM, FileFormat.FILEFORMAT_RECIPE);
                        //    //string pathName = Path.GetDirectoryName(pathToWrite);
                        //    //if (File.Exists(pathName))
                        //    //    File.Delete(pathName);
                        //    //if (false == Directory.Exists(pathName))
                        //    //    Directory.CreateDirectory(pathName);

                        //    //using (StreamWriter sw = new StreamWriter(pathToWrite))
                        //    //{
                        //    //    sw.Write(recipeBody);
                        //    //}

                        //    //_recipePathToUploadForPM = pathToWrite;// Path.GetDirectoryName(recipeFullPath);
                        //    ////RecipePath[NameOfPM] = pathToUpload;
                        //}

                        //messagePairs.TryGetValue(ResultKeys.KeyDescription, out string description);
                        //// PM이 먼저 요청한 경우에는 Result를 보내야 한다.
                        //// else에는 EFEM이 먼저 요청한 경우 만들어야한다.
                        //if (true) // true 자리에 PM이 요청한거에 대한 변수를 만들어 넣어야함 else는 Host가 먼저요청
                        //{
                        //    // 이 함수가 아니라 ClientToClient 뭐시기 함수 써야한다.
                        //    ExecuteToSendSimpleResultToClient(result, MessagesToReceive.RequestUploadRecipeResult.ToString(), nameOfEq, description);

                        //    if (IsScenarioRunning(EN_SCENARIO.SCENARIO_RECIPE_DOWNLOAD_BY_HOST))
                        //    {
                        //        UpdateScenarioPermission(EN_SCENARIO.SCENARIO_RECIPE_DOWNLOAD_BY_HOST, scenarioPermission);
                        //    }
                        //}
                        //else
                        //{
                        //    var scenario = EN_SCENARIO.SCENARIO_RECIPE_DOWNLOAD_BY_HOST;  // UpdateParam, MakeCustomScenario 만들어야함
                        //    var paramList = GetScenarioParameterList(scenario);
                        //    if (paramList == null)
                        //        return false;

                        //    Dictionary<string, string> paramsToUpdate = new Dictionary<string, string>();
                        //    for (int i = 0; i < paramList.Count; ++i)
                        //    {
                        //        string paramName = paramList[i];
                        //        string paramValue = string.Empty;
                        //        if (paramName.Equals(RecipeHandlingKeys.KeyParamRecipeId))
                        //        {
                        //            paramValue = recipeId;
                        //        }
                        //        else if (paramName.Equals(RecipeHandlingKeys.KeyUseCommunicationToPM))
                        //        {
                        //            paramValue = bool.TrueString;
                        //        }

                        //        paramsToUpdate[paramName] = paramValue;
                        //    }

                        //    EnqueueScenario(scenario, paramsToUpdate);
                        //}



                        return scenarioPermission;
                    }

                case MessagesToReceive.ResponseDeleteRecipe:
                    {
                        // TODO : RMS는 추후 구현
                    }
                    break;
                case MessagesToReceive.ResponseAssignSubstrateId:
                    {
                        bool scenarioPermission = true;
                        if (false == messagePairs.TryGetValue(ResultKeys.KeyResult, out string resultFromClient) ||
                            false == messagePairs.TryGetValue(ResultKeys.KeyDescription, out _))
                            scenarioPermission = false;

                        if (resultFromClient.Equals(EN_MESSAGE_RESULT.OK.ToString()))
                            scenarioPermission = true;
                        else
                            scenarioPermission = false;

                        if (IsScenarioRunning(EN_SCENARIO.SCENARIO_ASSIGN_SUBSTRATE_ID))
                        {
                            ScenarioList[EN_SCENARIO.SCENARIO_ASSIGN_SUBSTRATE_ID].UpdatePermission(scenarioPermission);
                            return true;
                        }

                        return false;
                    }

                case MessagesToReceive.ResponseAssignLotId:
                    {
                        return true;
                    }

                case MessagesToReceive.ResponseUploadBinFile:
                    {
                        #region
                        bool scenarioPermission = true;
                        if (false == messagePairs.TryGetValue(ResultKeys.KeyResult, out string resultFromClient) ||
                            false == messagePairs.TryGetValue(ResultKeys.KeyDescription, out _))
                            scenarioPermission = false;

                        if (resultFromClient != null &&
                            resultFromClient.Equals(EN_MESSAGE_RESULT.OK.ToString()))
                            scenarioPermission = true;
                        else
                            scenarioPermission = false;

                        if (false == scenarioPermission)
                            return scenarioPermission;

                        if (false == messagePairs.TryGetValue(UploadCoreOrBinFileKeys.KeySubstrateName, out string substrateName))
                            return false;
                        if (false == messagePairs.TryGetValue(UploadCoreOrBinFileKeys.KeyRingId, out string ringId))
                            return false;
                        if (false == messagePairs.TryGetValue(UploadCoreOrBinFileKeys.KeyRecipeId, out string recipeId))
                            return false;
                        if (false == messagePairs.TryGetValue(UploadCoreOrBinFileKeys.KeySubstrateType, out string subType))
                            return false;
                        if (false == Enum.TryParse(subType, out SubstrateType substrateType))
                            return false;
                        if (false == messagePairs.TryGetValue(UploadCoreOrBinFileKeys.KeyChipQty, out string qty))
                            return false;
                        if (false == int.TryParse(qty, out int chipQty))
                            return false;
                        if (false == messagePairs.TryGetValue(UploadCoreOrBinFileKeys.KeyPMSBody, out string pmsBody))
                            return false;
                        if (false == messagePairs.TryGetValue(UploadCoreOrBinFileKeys.KeyCountRow, out string row))
                            return false;
                        if (false == int.TryParse(row, out int countRow))
                            return false;
                        if (false == messagePairs.TryGetValue(UploadCoreOrBinFileKeys.KeyCountCol, out string col))
                            return false;
                        if (false == int.TryParse(col, out int countCol))
                            return false;
                        if (false == messagePairs.TryGetValue(UploadCoreOrBinFileKeys.KeyWaferAngle, out string angle))
                            return false;
                        if (false == double.TryParse(angle, out double waferAngle))
                            return false;
                        if (false == messagePairs.TryGetValue(UploadCoreOrBinFileKeys.KeyNullBinCode, out string nullBinCode))
                            return false;
                        if (false == messagePairs.TryGetValue(UploadCoreOrBinFileKeys.KeyBinCode, out string binCode))
                            return false;
                        if (false == messagePairs.TryGetValue(UploadCoreOrBinFileKeys.KeyMapData, out string mapData))
                            return false;

                        _functionsForPWA500.CreateBinDataToUpload(nameOfEq, substrateName, ringId, chipQty, waferAngle, countRow, countCol, nullBinCode, mapData, pmsBody, "AUTO", true);


                        if (IsScenarioRunning(EN_SCENARIO.SCENARIO_REQ_UPLOAD_BINFILE))
                        {
                            UpdateScenarioPermission(EN_SCENARIO.SCENARIO_REQ_UPLOAD_BINFILE, scenarioPermission);
                        }

                        return true;
                        //return ExecuteScenarioToUploadBinData(nameOfEq, substrateName, ringId, chipQty, waferAngle, countRow, countCol, nullBinCode, mapData, pmsBody, "AUTO", true);
                        #endregion
                    }

                case MessagesToReceive.RequestFinishPicking:
                    {
                        // W는 없으나 공정설비에서 발생시키므로 로그만 제거
                        useLogging = false;

                        #region
                        //// 이벤트 발생 필요
                        //// SubstrateName
                        //// RingId
                        //// BinCode
                        //// X
                        //// Y
                        //// Head
                        //// EES

                        //if (false == MakeScenarioParamToPickAndPlace(messagePairs, true, ref _finishPickingParams))
                        //    return false;

                        //EnqueueScenarioAsync(EN_SCENARIO.SCENARIO_PICK_UP_END, _finishPickingParams);
                        #endregion
                    }
                    break;
                case MessagesToReceive.RequestFinishPlacing:
                    {
                        // W는 없으나 공정설비에서 발생시키므로 로그만 제거
                        useLogging = false;
                        #region

                        //// 이벤트 발생 필요
                        //// SubstrateName
                        //// RingId
                        //// BinCode
                        //// X
                        //// Y
                        //// Head

                        //if (false == MakeScenarioParamToPickAndPlace(messagePairs, true, ref _finishPlacingParams))
                        //    return false;

                        //EnqueueScenarioAsync(EN_SCENARIO.SCENARIO_PLACE_END, _finishPlacingParams);
                        #endregion
                    }
                    break;

                default:
                    return false;
            }
            return false;
        }
        private bool ExecuteToSendSimpleResultToClient(EN_MESSAGE_RESULT result, string messageNameToSend, string nameOfEq, string description = "")
        {
            if (messageNameToSend == null || string.IsNullOrEmpty(messageNameToSend))
                return true;

            Dictionary<string, string> messageContentToSend = new Dictionary<string, string>
            {
                [ResultKeys.KeyResult] = result.ToString(),
                [ResultKeys.KeyDescription] = description,
            };

            return SendClientToClientMessage(nameOfEq, messageNameToSend.ToString(),
                        string.Empty, string.Empty,
                        messageContentToSend.Keys.ToArray(),
                        messageContentToSend.Values.ToArray(),
                        result, true);
        }
        private bool ExecuteSimpleScenarioAndSendClientMessage(EN_SCENARIO scenario, Dictionary<string, string> scenarioParam, string nameOfEq, string messageNameToSend)
        {
            if (nameOfEq == null)
                nameOfEq = string.Empty;

            if (messageNameToSend == null)
                messageNameToSend = string.Empty;

            Dictionary<string, string> additionalParams = new Dictionary<string, string>
            {
                [AdditionalParamKeys.KeyNameOfEq] = nameOfEq,
                [AdditionalParamKeys.KeyMessageNameToSend] = messageNameToSend,
            };

            EnqueueScenario(scenario, scenarioParam, additionalParams);
            return true;
        }
        private bool ExecuteScenarioToAssignSubstrateRingId(string nameOfEq, string oldSubstrateId, string newSubstrateId, bool isCore)
        {
            string pmName = _processModuleGroup.GetProcessModuleName(ProcessModuleIndex);
            List<Substrate> substrates = new List<Substrate>();
            bool result = _substrateManager.GetSubstratesAtProcessModule(pmName, ref substrates);
            if (result)
            {
                bool succeed = false;
                for (int i = 0; i < substrates.Count; ++i)
                {
                    var name = substrates[i].Name;
                    string ringId = substrates[i].GetAttribute(PWA500SubstrateAttributes.RingId);
                    if (name.Equals(oldSubstrateId) || name.Equals(newSubstrateId) || ringId.Equals(oldSubstrateId) || ringId.Equals(newSubstrateId))
                    {
                        succeed = true;

                        string oldName = name;
                        if (false == isCore)
                        {
                            if (false == oldName.Equals(newSubstrateId))
                            {
                                if (false == isCore)
                                {
                                    int portId = substrates[i].SourcePortId;
                                    _lotHistoryLog.WriteSubstrateHistoryForReadRingId(portId, oldSubstrateId, newSubstrateId);
                                }
                            }

                            // 읽은 1D를 이름으로 설정한다. -> 원래는 Ring Id 이며, 나중에 Id를 받는다.
                            _substrateManager.SetAttributeByKey(substrates[i].UniqueKey, PWA500SubstrateAttributes.RingId, newSubstrateId);
                            //substrates[i].SetName(newSubstrateId);
                            //_substrateManager.SetNameByKey(substrates[i].UniqueKey, newSubstrateId);
                            _substrateManager.SaveDataByKey(substrates[i].UniqueKey);

                            if (UseCoreMapHandlingOnly)
                            {
                                return ExecuteToSendSimpleResultToClient(EN_MESSAGE_RESULT.OK, MessagesToSend.ResponseAssignRingId.ToString(), nameOfEq);
                            }
                            else
                            {
                                Dictionary<string, string> scenarioParams = new Dictionary<string, string>
                                {
                                    [AssignRingIdKeys.KeyParamLotId] = substrates[i].LotId,
                                    [AssignRingIdKeys.KeyParamWaferId] = newSubstrateId,
                                };

                                Dictionary<string, string> additionalParams = new Dictionary<string, string>();
                                additionalParams[AdditionalParamKeys.KeyNameOfEq] = nameOfEq;
                                additionalParams[AdditionalParamKeys.KeySubstrateId] = newSubstrateId;
                                additionalParams[AdditionalParamKeys.KeyMessageNameToSend] = MessagesToSend.ResponseAssignRingId.ToString();
                                EnqueueScenario(EN_SCENARIO.SCENARIO_BIN_WAFER_ID_READ, scenarioParams, additionalParams);
                            }
                        }
                        else
                        {
                            if (UseCoreMapHandlingOnly)
                            {
                                return ExecuteToSendSimpleResultToClient(EN_MESSAGE_RESULT.OK, MessagesToSend.ResponseAssignCoreRingId.ToString(), nameOfEq);
                            }
                            else
                            {
                                // 1. 응답메시지를 보내고
                                ExecuteToSendSimpleResultToClient(EN_MESSAGE_RESULT.OK, MessagesToSend.ResponseAssignCoreRingId.ToString(), nameOfEq);

                                // 2. 이벤트 발생
                                ExecuteScenarioToCoreWaferIdRequest(nameOfEq, oldName, newSubstrateId);
                                //Dictionary<string, string> scenarioParams = new Dictionary<string, string>
                                //{
                                //    [AssignRingIdKeys.KeyParamRingFrameId] = newSubstrateId
                                //};

                                //Dictionary<string, string> additionalParams = new Dictionary<string, string>();
                                //additionalParams[AdditionalParamKeys.KeyNameOfEq] = nameOfEq;
                                //additionalParams[AdditionalParamKeys.KeyRingId] = oldName;
                                //additionalParams[AdditionalParamKeys.KeySubstrateId] = newSubstrateId;
                                //additionalParams[AdditionalParamKeys.KeyMessageNameToSend] = MessagesToSend.RequestAssignCoreSubstrateId.ToString();
                                //EnqueueScenarioAsync(EN_SCENARIO.SCENARIO_REQ_CORE_WAFER_ID, scenarioParams, additionalParams);
                            }
                        }
                    }
                }

                var useSecsGem = _recipe.GetValue(EN_RECIPE_TYPE.COMMON, PARAM_COMMON.UseSecsGem.ToString(), true);
                if (false == succeed && false == useSecsGem)
                {
                    // TODO : 통신이 꺼져있고 자재 정보를 못 찾으면

                    // 1. 응답메시지를 보내고
                    ExecuteToSendSimpleResultToClient(EN_MESSAGE_RESULT.OK, MessagesToSend.ResponseAssignCoreRingId.ToString(), nameOfEq);

                    // 2. 이벤트 발생
                    ExecuteScenarioToCoreWaferIdRequest(nameOfEq, oldSubstrateId, newSubstrateId);
                }

                result = succeed;
            }

            #region <Original Codes>
            //bool result = true;
            //if (false == result)
            //{
            //    Dictionary<string, string> messageContentToSend = new Dictionary<string, string>
            //    {
            //        [ResultKeys.KeyResult] = EN_MESSAGE_RESULT.NG.ToString(),
            //        [ResultKeys.KeyDescription] = "Does not have ring id",
            //        [AssignRingIdKeys.KeyOldRingId] = oldSubstrateId,
            //        [AssignRingIdKeys.KeyNewRingId] = newSubstrateId
            //    };

            //    MessagesToSend title = isCore ? MessagesToSend.ResponseAssignCoreRingId : MessagesToSend.ResponseAssignRingId;
            //    SendClientToClientMessage(nameOfEq, title.ToString(),
            //        string.Empty, string.Empty,
            //        messageContentToSend.Keys.ToArray(), messageContentToSend.Values.ToArray(),
            //        EN_MESSAGE_RESULT.NG,
            //        true);
            //}

            //Substrate substrate = new Substrate();
            //if (_substrateManager.GetSubstrateByName(oldSubstrateId, ref substrate) ||
            //    _substrateManager.GetSubstrateByName(newSubstrateId, ref substrate))
            //{
            //    string oldName = substrate.Name;
            //    if (false == oldName.Equals(newSubstrateId))
            //    {
            //        int portId = substrate.SourcePortId;
            //        _lotHistoryLog.WriteSubstrateHistoryForReadRingId(portId, oldSubstrateId, newSubstrateId);
            //    }

            //    // 읽은 1D를 이름으로 설정한다. -> 원래는 Ring Id 이며, 나중에 Id를 Assign 받는다.
            //    substrate.SetAttribute(PWA500WSubstrateAttributes.RingId, newSubstrateId);
            //    substrate.SetName(newSubstrateId);

            //    if (UseCoreMapHandlingOnly)
            //    {
            //        return ExecuteToSendSimpleResultToClient(EN_MESSAGE_RESULT.OK, MessagesToSend.ResponseAssignRingId.ToString(), nameOfEq);
            //    }
            //    else
            //    {
            //        Dictionary<string, string> scenarioParams = new Dictionary<string, string>
            //        {
            //            [AssignRingIdKeys.KeyParamLotId] = substrate.LotId,
            //            [AssignRingIdKeys.KeyParamWaferId] = newSubstrateId,
            //        };

            //        Dictionary<string, string> additionalParams = new Dictionary<string, string>();
            //        additionalParams[AdditionalParamKeys.KeyNameOfEq] = nameOfEq;
            //        additionalParams[AdditionalParamKeys.KeySubstrateId] = newSubstrateId;
            //        additionalParams[AdditionalParamKeys.KeyMessageNameToSend] = MessagesToSend.ResponseAssignRingId.ToString();
            //        EnqueueScenarioAsync(EN_SCENARIO.SCENARIO_BIN_WAFER_ID_READ, scenarioParams, additionalParams);
            //    }
            //}
            //else
            //{
            //    Dictionary<string, string> messageContentToSend = new Dictionary<string, string>
            //    {
            //        [ResultKeys.KeyResult] = EN_MESSAGE_RESULT.NG.ToString(),
            //        [ResultKeys.KeyDescription] = "Does not have ring id",
            //        [AssignRingIdKeys.KeyOldRingId] = oldSubstrateId,
            //        [AssignRingIdKeys.KeyNewRingId] = newSubstrateId
            //    };

            //    SendClientToClientMessage(nameOfEq, MessagesToSend.ResponseAssignRingId.ToString(),
            //        string.Empty, string.Empty,
            //        messageContentToSend.Keys.ToArray(), messageContentToSend.Values.ToArray(),
            //        EN_MESSAGE_RESULT.NG,
            //        true);
            //    result = false;
            //}
            #endregion </Original Codes>

            return result;
        }
        private void ExecuteScenarioToCoreWaferIdRequest(string nameOfEq, string oldName, string newName)
        {
            Dictionary<string, string> scenarioParams = new Dictionary<string, string>
            {
                [AssignRingIdKeys.KeyParamRingFrameId] = newName
            };

            Dictionary<string, string> additionalParams = new Dictionary<string, string>();
            additionalParams[AdditionalParamKeys.KeyNameOfEq] = nameOfEq;
            additionalParams[AdditionalParamKeys.KeyRingId] = oldName;
            additionalParams[AdditionalParamKeys.KeySubstrateId] = newName;
            additionalParams[AdditionalParamKeys.KeyMessageNameToSend] = MessagesToSend.RequestAssignCoreSubstrateId.ToString();

            EnqueueScenario(EN_SCENARIO.SCENARIO_REQ_CORE_WAFER_ID, scenarioParams, additionalParams);
        }
        private bool ExecuteScenarioToDownloadMapFile(string nameOfEq, string substrateId, string ringId, double waferAngle, string nullBinCode, string userId, bool useEventHandling)
        {
            // TODO : 여기서 자재 정보를 Set 할게 아니라, WorkStart 이후 정상일 때에만 Set 하도록 해야한다.
            // 2025.01.22. jhlim [MOD] RingId를 이용해 찾도록 추가
            // 2024.12.29. jhlim [MOD] RingId가 고유하므로, RingId 부터 찾는다.

            if (_functionsForPWA500.FindSubstrateByNameOrRingIdAtProcessModule(substrateId, ringId, out var substrate, out var description) ||
                _functionsForPWA500.GetSubstrateByName(substrateId, out substrate) ||
                _functionsForPWA500.GetSubstrateByName(ringId, out substrate))
            {
                int portId, slot;
                portId = substrate.SourcePortId;
                slot = substrate.SourceSlot;

                if (portId <= 0 || slot < 0)
                    return false;

                string carrierId = _carrierServer.GetCarrierId(portId);
                string lotId = substrate.LotId;
                string partId = substrate.GetAttribute(PWA500SubstrateAttributes.PartId);
                string recipeId = substrate.RecipeId;

                Dictionary<string, string> scenarioParams = new Dictionary<string, string>
                {
                    [RequestDownloadMapFileKeys.KeyParamCarrierId] = carrierId,
                    [RequestDownloadMapFileKeys.KeyParamPortId] = portId.ToString(),
                    [RequestDownloadMapFileKeys.KeyParamLotId] = lotId,
                    [RequestDownloadMapFileKeys.KeyParamPartId] = partId,
                    [RequestDownloadMapFileKeys.KeyParamRecipeId] = recipeId,
                    [RequestDownloadMapFileKeys.KeyParamOperatorId] = userId,
                    [RequestDownloadMapFileKeys.KeyParamWaferId] = substrateId,
                    [RequestDownloadMapFileKeys.KeyParamAngle] = waferAngle.ToString(),
                    [RequestDownloadMapFileKeys.KeyNullBinCode] = nullBinCode,
                    [RequestDownloadMapFileKeys.KeyUseEventHandling] = useEventHandling.ToString(),

                };

                // 공정설비에서 받은 이름을 이 웨이퍼의 이름으로 설정한다.
                //substrate.SetName(substrateId);
                //_substrateManager.SetNameByKey(substrate.UniqueKey, substrateId);
                _substrateManager.SaveDataByKey(substrate.UniqueKey);

                Dictionary<string, string> additionalParams = new Dictionary<string, string>();
                additionalParams[AdditionalParamKeys.KeyNameOfEq] = nameOfEq;
                additionalParams[AdditionalParamKeys.KeySubstrateId] = substrateId;
                additionalParams[AdditionalParamKeys.KeyRingId] = ringId;
                additionalParams[AdditionalParamKeys.KeyUserId] = userId;

                _lotHistoryLog.WriteSubstrateHistoryForDownloadMap(portId, carrierId, substrateId, ringId);

                EnqueueScenario(EN_SCENARIO.SCENARIO_CORE_MAP_DOWNLOAD, scenarioParams, additionalParams);
                return true;
            }
            else
            {
                if (false == string.IsNullOrEmpty(description))
                {
                    WriteLog(description);
                }

                // 2025.07.16. jhlim [MOD] 자재 정보가 없는 경우, GEM이 꺼져있으면 이벤트 핸들링 없이 시나리오를 진행한다.
                var useSecsGem = _recipe.GetValue(EN_RECIPE_TYPE.COMMON, PARAM_COMMON.UseSecsGem.ToString(), true);
                useEventHandling = false;
                //if (UseCoreMapHandlingOnly)
                if (false == useSecsGem)
                {
                    Dictionary<string, string> scenarioParams = new Dictionary<string, string>
                    {
                        [RequestDownloadMapFileKeys.KeyParamCarrierId] = string.Empty,
                        [RequestDownloadMapFileKeys.KeyParamPortId] = string.Empty,
                        [RequestDownloadMapFileKeys.KeyParamLotId] = string.Empty,
                        [RequestDownloadMapFileKeys.KeyParamPartId] = string.Empty,
                        [RequestDownloadMapFileKeys.KeyParamRecipeId] = EquipmentInfo.GetRecipeId(),
                        [RequestDownloadMapFileKeys.KeyParamOperatorId] = userId,
                        [RequestDownloadMapFileKeys.KeyParamWaferId] = substrateId,
                        [RequestDownloadMapFileKeys.KeyParamAngle] = waferAngle.ToString(),
                        [RequestDownloadMapFileKeys.KeyNullBinCode] = nullBinCode,
                        [RequestDownloadMapFileKeys.KeyUseEventHandling] = useEventHandling.ToString()
                    };

                    Dictionary<string, string> additionalParams = new Dictionary<string, string>();
                    additionalParams[AdditionalParamKeys.KeyNameOfEq] = nameOfEq;
                    additionalParams[AdditionalParamKeys.KeySubstrateId] = substrateId;
                    additionalParams[AdditionalParamKeys.KeyRingId] = ringId;
                    additionalParams[AdditionalParamKeys.KeyUserId] = userId;

                    EnqueueScenario(EN_SCENARIO.SCENARIO_CORE_MAP_DOWNLOAD, scenarioParams, additionalParams);
                    return true;
                }
                else
                {
                    Dictionary<string, string> messageContentToSend = new Dictionary<string, string>
                    {
                        [ResultKeys.KeyResult] = EN_MESSAGE_RESULT.NG.ToString(),
                        [ResultKeys.KeyDescription] = "Does not have ring id",
                        [RequestDownloadMapFileKeys.KeyParamWaferId] = substrateId,
                        [RequestDownloadMapFileKeys.KeyCountRow] = string.Empty,
                        [RequestDownloadMapFileKeys.KeyCountCol] = string.Empty,
                        [RequestDownloadMapFileKeys.KeyParamAngle] = string.Empty,
                        [RequestDownloadMapFileKeys.KeyMapData] = string.Empty,
                    };

                    SendClientToClientMessage(nameOfEq, MessagesToSend.ResponseDownloadMapFile.ToString(),
                        string.Empty, string.Empty,
                        messageContentToSend.Keys.ToArray(), messageContentToSend.Values.ToArray(),
                        EN_MESSAGE_RESULT.NG,
                        true);

                    return false;
                }
                // 2025.07.16. jhlim [END]
            }
        }

        private bool ExecuteScenarioToWorkEnd(string nameOfEq, string substrateId, string ringId,
            int chipQty, double angle, int countRow, int countCol, string nullBinCode, string mapData,
            string userId, bool isCore, bool useEventHandling)
        {
            if (_functionsForPWA500.FindSubstrateByNameOrRingIdAtProcessModule(substrateId, ringId, out var substrate, out _))
            {
                MapDataControl control = new MapDataControl();
                var map = control.MakeCoreMapObject(
                    substrate.LotId,
                    substrate.Name,
                    substrate.RecipeId,
                    mapData,
                    (int)angle,
                    countCol,
                    countRow,
                    chipQty,
                    0,
                    0
                    );

                var dataToUpload = control.SerializeMapData(map);

                _substrateManager.SetAttributeByKey(substrate.UniqueKey, PWA500SubstrateAttributes.MapData, mapData);
                _substrateManager.SetAttributeByKey(substrate.UniqueKey, PWA500SubstrateAttributes.ChipQty, chipQty.ToString());
                _substrateManager.SaveDataByKey(substrate.UniqueKey);

                double ffrot = 0;
                Dictionary<string, string> scenarioParams = new Dictionary<string, string>
                {
                    [UploadMapKeys.KeyParamWaferId] = substrateId,
                    [UploadMapKeys.KeyParamFilmFrameLocation] = ffrot.ToString(),   // 2026.06.24 dwlim [ADD] VID 추가
                    [UploadMapKeys.KeyParamFlatNotchLocation] = angle.ToString(),   // 2026.06.24 dwlim [ADD] VID 추가
                    [UploadMapKeys.KeyParamMapData] = dataToUpload
                };

                Dictionary<string, string> additionalParams = new Dictionary<string, string>
                {
                    [AdditionalParamKeys.KeyNameOfEq] = nameOfEq,
                    [AdditionalParamKeys.KeyMessageNameToSend] = MessagesToSend.ResponseUploadCoreFile.ToString(),
                };


                return EnqueueAutoScenarioByUpdate(
                    ScenarioSenders.Auto.ToString(),
                    EN_SCENARIO.SCENARIO_CORE_MAP_UPLOAD, 
                    scenarioParams, 
                    additionalParams);
                //UpdateScenarioParams(EN_SCENARIO.SCENARIO_CORE_MAP_UPLOAD.ToString(), scenarioParam);




                //string lotId = substrate.LotId;

                //EN_SCENARIO scenario;
                //int portId, slot;
                //string partId = substrate.GetAttribute(PWA500SubstrateAttributes.PartId);
                //string recipeId = EquipmentInfo.GetRecipeId();

                //Dictionary<string, string> additionalParams = null;
                //if (false == isCore)
                //{
                //    portId = substrate.DestinationPortId;
                //    slot = substrate.DestinationSlot;
                //    scenario = EN_SCENARIO.SCENARIO_BIN_WORK_END;

                //    additionalParams = new Dictionary<string, string>
                //    {
                //        [AdditionalParamKeys.KeySubstrateId] = substrateId,
                //        [AdditionalParamKeys.KeyChipQty] = chipQty.ToString(),
                //        [AdditionalParamKeys.KeyUserId] = userId,
                //    };
                //}
                //else
                //{
                //    portId = substrate.SourcePortId;
                //    slot = substrate.SourceSlot;
                //    scenario = EN_SCENARIO.SCENARIO_WORK_END;
                //    additionalParams = new Dictionary<string, string>
                //    {
                //        [AdditionalParamKeys.KeyNameOfEq] = nameOfEq,
                //        [AdditionalParamKeys.KeySubstrateId] = substrateId,
                //        [AdditionalParamKeys.KeyChipQty] = chipQty.ToString(),
                //        [AdditionalParamKeys.KeyUserId] = userId,
                //    };
                //}

                //if (portId <= 0 || slot < 0)
                //    return false;

                //string carrierId = _carrierServer.GetCarrierId(portId);
                //Dictionary<string, string> scenarioParams = new Dictionary<string, string>
                //{
                //    [UploadCoreOrBinFileKeys.KeyParamCarrierId] = carrierId,
                //    [UploadCoreOrBinFileKeys.KeyParamPortId] = EquipmentInfo.GetPortName(portId),
                //    [UploadCoreOrBinFileKeys.KeyParamLotId] = lotId,
                //    [UploadCoreOrBinFileKeys.KeyParamPartId] = partId,
                //    [UploadCoreOrBinFileKeys.KeyParamRecipeId] = recipeId,
                    
                //    //[UploadCoreOrBinFileKeys.KeyParamSlotId] = (slot).ToString(),

                //    [UploadCoreOrBinFileKeys.KeyParamOperatorId] = userId,
                //    [UploadCoreOrBinFileKeys.KeyChipQty] = chipQty.ToString(),

                //    [UploadCoreOrBinFileKeys.KeySubstrateName] = substrateId,

                //    [UploadCoreOrBinFileKeys.KeyWaferAngle] = angle.ToString(),
                //    [UploadCoreOrBinFileKeys.KeyCountRow] = countRow.ToString(),
                //    [UploadCoreOrBinFileKeys.KeyCountCol] = countCol.ToString(),
                //    [UploadCoreOrBinFileKeys.KeyReferenceX] = substrate.GetAttribute(PWA500SubstrateAttributes.RefPositionX),
                //    [UploadCoreOrBinFileKeys.KeyReferenceY] = substrate.GetAttribute(PWA500SubstrateAttributes.RefPositionY),
                //    [UploadCoreOrBinFileKeys.KeyStartingPosX] = substrate.GetAttribute(PWA500SubstrateAttributes.StartingPositionX),
                //    [UploadCoreOrBinFileKeys.KeyStartingPosY] = substrate.GetAttribute(PWA500SubstrateAttributes.StartingPositionY),
                //    [UploadCoreOrBinFileKeys.KeyNullBinCode] = nullBinCode,
                //    [UploadCoreOrBinFileKeys.KeyMapData] = mapData,

                //    [UploadCoreOrBinFileKeys.KeyUseEventHandling] = useEventHandling.ToString(),
                //};

                //_substrateManager.SetAttributeByKey(substrate.UniqueKey, PWA500SubstrateAttributes.MapData, mapData);
                //_substrateManager.SetAttributeByKey(substrate.UniqueKey, PWA500SubstrateAttributes.ChipQty, chipQty.ToString());
                //_substrateManager.SaveDataByKey(substrate.UniqueKey);

                ////_substrateManager.SetAttributesByKey(substrate.UniqueKey, new Dictionary<string, string>
                ////{
                ////    [PWA500SubstrateAttributes.MapData] = mapData,
                ////    [PWA500SubstrateAttributes.ChipQty] = chipQty.ToString()
                ////});

                //EnqueueScenario(scenario, scenarioParams, additionalParams);
                //return true;
            }
            //else
            //{
            //    if (false == useEventHandling)
            //    {
            //        Dictionary<string, string> scenarioParams = new Dictionary<string, string>
            //        {
            //            [UploadCoreOrBinFileKeys.KeyParamCarrierId] = string.Empty,
            //            [UploadCoreOrBinFileKeys.KeyParamPortId] = string.Empty,
            //            [UploadCoreOrBinFileKeys.KeyParamLotId] = string.Empty,
            //            [UploadCoreOrBinFileKeys.KeyParamPartId] = string.Empty,
            //            [UploadCoreOrBinFileKeys.KeyParamRecipeId] = EquipmentInfo.GetRecipeId(),
            //            [UploadCoreOrBinFileKeys.KeyParamOperatorId] = userId,
            //            [UploadCoreOrBinFileKeys.KeyChipQty] = chipQty.ToString(),

            //            [UploadCoreOrBinFileKeys.KeyPMSFileName] = string.Empty,
            //            [UploadCoreOrBinFileKeys.KeyPMSFileBody] = string.Empty,

            //            [UploadCoreOrBinFileKeys.KeySubstrateName] = substrateId,

            //            [UploadCoreOrBinFileKeys.KeyWaferAngle] = angle.ToString(),
            //            [UploadCoreOrBinFileKeys.KeyCountRow] = countRow.ToString(),
            //            [UploadCoreOrBinFileKeys.KeyCountCol] = countCol.ToString(),
            //            [UploadCoreOrBinFileKeys.KeyNullBinCode] = nullBinCode,
            //            [UploadCoreOrBinFileKeys.KeyMapData] = mapData,
            //            [UploadCoreOrBinFileKeys.KeyReferenceX] = "0",
            //            [UploadCoreOrBinFileKeys.KeyReferenceY] = "0",
            //            [UploadCoreOrBinFileKeys.KeyStartingPosX] = "0",
            //            [UploadCoreOrBinFileKeys.KeyStartingPosY] = "0",

            //            [UploadCoreOrBinFileKeys.KeyUseEventHandling] = useEventHandling.ToString(),
            //        };

            //        Dictionary<string, string> additionalParams = new Dictionary<string, string>
            //        {
            //            [AdditionalParamKeys.KeyNameOfEq] = nameOfEq,
            //            [AdditionalParamKeys.KeySubstrateId] = substrateId,
            //            [AdditionalParamKeys.KeyChipQty] = chipQty.ToString(),
            //            [AdditionalParamKeys.KeyUserId] = userId,
            //        };

            //        if (isCore)
            //        {
            //            EnqueueScenario(EN_SCENARIO.SCENARIO_WORK_END, scenarioParams, additionalParams);
            //        }
            //        else
            //        {
            //            EnqueueScenario(EN_SCENARIO.SCENARIO_BIN_WORK_END, scenarioParams, additionalParams);
            //        }
            //        return true;

            //        // 2024.08.18 : [END]
            //    }
            //}

            return false;
        }
        #endregion /method
    }
}