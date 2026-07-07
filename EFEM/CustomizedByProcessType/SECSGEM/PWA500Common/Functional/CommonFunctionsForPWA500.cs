using System;
using System.Collections.Generic;
using System.Collections.Concurrent;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using TickCounter_;

using EFEM.CustomizedByProcessType.PWA500BIN;
using EFEM.Defines.Common;
using EFEM.Defines.LoadPort;
using EFEM.Defines.MapData;
using EFEM.MaterialTracking;
using EFEM.Modules;
using FrameOfSystem3.Recipe;
using FrameOfSystem3.SECSGEM.DefineSecsGem;

namespace EFEM.CustomizedByProcessType.PWA500Common
{
    public class CommonFunctionsForPWA500
    {
        #region <Constructors>
        public CommonFunctionsForPWA500(bool useTrackOutCore, bool useComparePartId)
        {
            _substrateManager = SubstrateManager.Instance;
            _carrierServer = CarrierManagementServer.Instance;
            _loadPortManager = LoadPortManager.Instance;
            _processGroup = ProcessModuleGroup.Instance;
            _recipe = FrameOfSystem3.Recipe.Recipe.GetInstance();

            _lotHistoryLog = LotHistoryLog.Instance;

            UseTrackOutCore = useTrackOutCore;
            UseComparePartId = useComparePartId;
        }
        #endregion </Constructors>

        #region <Fields>
        private static SubstrateManager _substrateManager = null;
        private static CarrierManagementServer _carrierServer = null;
        private static ProcessModuleGroup _processGroup = null;
        private static LoadPortManager _loadPortManager = null;
        private static LotHistoryLog _lotHistoryLog = null;

        private static FrameOfSystem3.Recipe.Recipe _recipe = null;
        private Func<string, string, string, string, string[], string[], EN_MESSAGE_RESULT, bool, bool> _funcToSendClientMessage = null;
        private Action<EN_SCENARIO, Dictionary<string, string>, Dictionary<string, string>> _actionToEnqueueScenarioAsync = null;
        private const int ProcessModuleIndex = 0;

        private Func<string, Dictionary<string, string>, bool> _funcToUpdateScenarioParam = null;
        private Func<EN_SCENARIO, EN_SCENARIO_RESULT> _funcToExecuteScenario = null;

        private readonly TickCounter TicksForCarrierLoad = new TickCounter();
        private QueuedScenarioInfo _dequeuedScenarioToCarrierLoad = null;
        private readonly ConcurrentQueue<QueuedScenarioInfo> CarrierLoadingReservation = new ConcurrentQueue<QueuedScenarioInfo>();

        private static BinDataToUploadFromPWA500 _binDataToUpload = null;
        private static ScrapCoreInfo _scrapInfoToUpload = null;
        private string _pathForPms = string.Empty;
        #endregion </Fields>

        #region <Properties>
        private bool UseCoreMapHandlingOnly
        {
            get
            {
                return false;// (false == _recipe.GetValue(EN_RECIPE_TYPE.COMMON, PARAM_COMMON.UseSecsGem.ToString(), false));
            }
        }
        private int HandlingRequestDelayEachLoadPorts
        {
            get
            {
                return _recipe.GetValue(EN_RECIPE_TYPE.EQUIPMENT, PARAM_EQUIPMENT.HandlingRequestDelayEachLoadPorts.ToString(), 5000);
            }
        }
        public string PmsFullPath
        {
            get
            {
                return _pathForPms;
            }
        }
        public bool HasScenarioError { get; set; }
        public EN_SCENARIO FailedScenarioTypes { get; set; }
        public string ScenarioErrorDescription { get; set; }

        protected bool UseTrackOutCore { get; private set; }
        protected bool UseComparePartId { get; private set; }
        protected static Recipe Recipe
        {
            get
            {
                return _recipe;
            }
        }
        protected static SubstrateManager SubstrateManager
        {
            get
            {
                return _substrateManager;
            }
        }
        protected static CarrierManagementServer CarrierServer
        {
            get
            {
                return _carrierServer;
            }
        }
        protected static LoadPortManager LoadPortManager
        {
            get
            {
                return _loadPortManager;

            }
        }
        #endregion </Properties>

        #region <Methods>

        #region <Assign Functions>
        public void AssignFunctionToSendClientMessage(Func<string, string, string, string, string[], string[], EN_MESSAGE_RESULT, bool, bool> func)
        {
            _funcToSendClientMessage = func;
        }
        public void AssignActionToEnqueueScenarioAsync(Action<EN_SCENARIO, Dictionary<string, string>, Dictionary<string, string>> action)
        {
            _actionToEnqueueScenarioAsync = action;
        }
        public void AssignFunctionToUpdateParam(Func<string, Dictionary<string, string>, bool> func)
        {
            _funcToUpdateScenarioParam = func;
        }
        public void AssignFunctionToExecuteScenario(Func<EN_SCENARIO, EN_SCENARIO_RESULT> func)
        {
            _funcToExecuteScenario = func;
        }
        #endregion </Assign Functions>

        #region <OHT Handling>
        // 
        public void EnqueueScenarioCarrierHandlingAsync(int portId, LoadPortLoadingMode loadingType, string lotId, EN_SCENARIO scenario)
        {
            var param = ScenarioParameterBuilder.MakeParamToOHTHandling(portId, loadingType, lotId, scenario);
            var queuedScenario = new QueuedScenarioInfo
            {
                Scenario = scenario,
                ScenarioParams = param
            };
            if (scenario.Equals(EN_SCENARIO.SCENARIO_PORT_STATUS_LOAD_1) ||
                scenario.Equals(EN_SCENARIO.SCENARIO_PORT_STATUS_LOAD_2) ||
                scenario.Equals(EN_SCENARIO.SCENARIO_PORT_STATUS_LOAD_3) ||
                scenario.Equals(EN_SCENARIO.SCENARIO_PORT_STATUS_LOAD_4) ||
                scenario.Equals(EN_SCENARIO.SCENARIO_PORT_STATUS_LOAD_5) ||
                scenario.Equals(EN_SCENARIO.SCENARIO_PORT_STATUS_LOAD_6))
            {
                CarrierLoadingReservation.Enqueue(queuedScenario);
            }
            else if (scenario.Equals(EN_SCENARIO.SCENARIO_PORT_STATUS_UNLOAD_1) ||
                    scenario.Equals(EN_SCENARIO.SCENARIO_PORT_STATUS_UNLOAD_2) ||
                    scenario.Equals(EN_SCENARIO.SCENARIO_PORT_STATUS_UNLOAD_3) ||
                    scenario.Equals(EN_SCENARIO.SCENARIO_PORT_STATUS_UNLOAD_4) ||
                    scenario.Equals(EN_SCENARIO.SCENARIO_PORT_STATUS_UNLOAD_5) ||
                    scenario.Equals(EN_SCENARIO.SCENARIO_PORT_STATUS_UNLOAD_6))
            {
                //CarrierUnloadingReservation.Enqueue(queuedScenario);
            }

            //string message = string.Format("[{0:d2}/{1:d2}-{2:d2}:{3:d2}:{4:d2}.{5:d3}] Scenario : {6} Enqueued !! ",
            //    DateTime.Now.Month,
            //    DateTime.Now.Day,
            //    DateTime.Now.Hour,
            //    DateTime.Now.Minute,
            //    DateTime.Now.Second,
            //    DateTime.Now.Millisecond,
            //    scenario.ToString());
            //Console.WriteLine(message);
        }

        public void ExecuteScanrioToCarrierLoadAsync()
        {
            if (_funcToUpdateScenarioParam == null || _funcToExecuteScenario == null)
                return;

            if (UseCoreMapHandlingOnly)
            {
                while (CarrierLoadingReservation.Count > 0)
                {
                    CarrierLoadingReservation.TryDequeue(out _);
                }

                return;
            }

            if (_dequeuedScenarioToCarrierLoad != null)
            {
                var result = _funcToExecuteScenario(_dequeuedScenarioToCarrierLoad.Scenario);
                switch (result)
                {
                    case EN_SCENARIO_RESULT.WAITING:
                    case EN_SCENARIO_RESULT.PROCEED:
                        return;

                    case EN_SCENARIO_RESULT.COMPLETED:
                    case EN_SCENARIO_RESULT.ERROR:
                    case EN_SCENARIO_RESULT.TIMEOUT_ERROR:
                        {
                            TicksForCarrierLoad.SetTickCount((uint)HandlingRequestDelayEachLoadPorts);
                            _dequeuedScenarioToCarrierLoad = null;

                            // 종료 중이면 비운다.
                            if (false == EquipmentState_.EquipmentState.GetInstance().GetState().Equals(EquipmentState_.EQUIPMENT_STATE.SETUP) &&
                                false == EquipmentState_.EquipmentState.GetInstance().GetState().Equals(EquipmentState_.EQUIPMENT_STATE.FINISHING) &&
                                false == EquipmentState_.EquipmentState.GetInstance().GetState().Equals(EquipmentState_.EQUIPMENT_STATE.EXECUTING))
                            {
                                while (CarrierLoadingReservation.Count > 0)
                                {
                                    CarrierLoadingReservation.TryDequeue(out _);
                                }
                            }
                        }
                        break;
                    default:
                        break;
                }
            }
            else
            {
                if (CarrierLoadingReservation.Count <= 0)
                    return;

                // 셋 된 상태에서 Tick이 넘어가지 않았으면 리턴
                if (false == TicksForCarrierLoad.IsTickOver(false) &&
                    TicksForCarrierLoad.IsSet())
                    return;

                CarrierLoadingReservation.TryDequeue(out _dequeuedScenarioToCarrierLoad);
                // 파라메터 갱신
                Enum scenario = _dequeuedScenarioToCarrierLoad.Scenario;
                var scenarioParams = _dequeuedScenarioToCarrierLoad.ScenarioParams;
                _funcToUpdateScenarioParam(scenario.ToString(), scenarioParams);

                //string message = string.Format("[{0:d2}/{1:d2}-{2:d2}:{3:d2}:{4:d2}.{5:d3}] Scenario : {6} Dequeued !! ",
                //                DateTime.Now.Month,
                //                DateTime.Now.Day,
                //                DateTime.Now.Hour,
                //                DateTime.Now.Minute,
                //                DateTime.Now.Second,
                //                DateTime.Now.Millisecond,
                //                _dequeuedScenarioToCarrierLoad.Scenario.ToString());
                //Console.WriteLine(message);
            }
        }
        #endregion </OHT Handling>

        public bool ExecuteScenarioAsyncToCarrierLoad(string lotId, string carrierId)
        {
            if (_actionToEnqueueScenarioAsync == null)
                return false;

            var scenarioParam = new Dictionary<string, string>
            {
                [CarrierLoadUnloadKeys.KeyParamCarrierId] = carrierId,
                [CarrierLoadUnloadKeys.KeyParamLotId] = lotId
            };

            _actionToEnqueueScenarioAsync(EN_SCENARIO.SCENARIO_CARRIER_LOAD, scenarioParam, null);

            return true;
        }
        public bool ExecuteScenarioAsyncToCarrierUnload(string lotId, string partId, string stepId, string lotType)
        {
            if (_actionToEnqueueScenarioAsync == null)
                return false;

            var scenarioParam = new Dictionary<string, string>
            {
                [CarrierLoadUnloadKeys.KeyParamLotId] = lotId,
                [CarrierLoadUnloadKeys.KeyParamPartId] = partId,
                [CarrierLoadUnloadKeys.KeyParamStepId] = stepId,
                [CarrierLoadUnloadKeys.KeyParamLotType] = lotType
            };

            _actionToEnqueueScenarioAsync(EN_SCENARIO.SCENARIO_CARRIER_UNLOAD, scenarioParam, null);

            return true;
        }
        public string GetModelName()
        {
            return _recipe.GetValue(FrameOfSystem3.Recipe.EN_RECIPE_TYPE.EQUIPMENT, FrameOfSystem3.Recipe.PARAM_EQUIPMENT.MachineName.ToString(), string.Empty);
        }
        public string GetPMSFileName(string lotId, string substrateId)
        {
            return string.Format("{0}_{1}_{2}_{3}", GetModelName(), lotId, substrateId, DateTime.Now.ToString("yyMMddHHmmss"));
        }
        // 2025.05.16 dwlim [ADD] BinMap Upload를 위한 저장된 PMS File의 Data 불러오기
        public string[] GetPMSDataFromPMSFile(string fullPath)
        {
            if (string.IsNullOrWhiteSpace(fullPath) ||
                false == File.Exists(fullPath))
            {
                return null;
            }

            try
            {
                string[] data = File.ReadAllLines(fullPath);
                return data;
            }
            catch (Exception)
            {
                return null;
            }
        }
        public bool MakePMSFile(string lotId, string substrateId, string fileName, string body, ref string fullPath)
        {
            fullPath = string.Format(@"{0}\PMS\{1}\{2,00:d2}\{3,00:d2}\{4}\{5}\{6}.PMS", Define.DefineConstant.FilePath.FILEPATH_LOG,
                DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day,
                lotId, substrateId,
                fileName);

            StreamWriter sw = null;

            try
            {
                string dir = Path.GetDirectoryName(fullPath);
                if (false == Directory.Exists(dir))
                    Directory.CreateDirectory(dir);

                if (File.Exists(fullPath))
                    File.Delete(fullPath);

                sw = new StreamWriter(fullPath);

                sw.Write(body);

                sw.Close();
            }
            catch (Exception)
            {
                if (sw != null)
                {
                    sw.Close();
                }

                return false;
            }

            return true;
        }
        public void CreateScrapInfoForBin(string info, string qty, string userId)
        {
            _scrapInfoToUpload = new ScrapCoreInfo(info, qty);
        }
        public bool GetScrapDataToUpload(out ScrapCoreInfo info)
        {
            info = _scrapInfoToUpload;

            return (info != null);
        }
        public void ClearScrapDataToUpload()
        {
            if (_scrapInfoToUpload != null)
                _scrapInfoToUpload = null;
        }
        public void CreateBinDataToUpload(string nameOfEq, string substrateId, string ringId,
            int chipQty, double angle, int countRow, int countCol, string nullBinCode, string mapData,
            string pmsFileBody, string userId, bool useEventHandling)
        {
            _binDataToUpload = new BinDataToUploadFromPWA500(nameOfEq, substrateId, ringId,
                chipQty, angle, countRow, countCol, nullBinCode, mapData,
                pmsFileBody, userId, useEventHandling);
        }
        public bool GetBinDataToUpload(ref BinDataToUploadFromPWA500 dataToUpload)
        {
            if (_binDataToUpload == null)
                return false;

            dataToUpload = _binDataToUpload;
            return true;
        }
        public void ClearBinDataToUpload()
        {
            if (_binDataToUpload != null)
                _binDataToUpload = null;
        }
        // [TODO] : 2025.05.16 dwlim [ADD] 로그 제출로인해 작성. 나중에 수정해야함
        public Dictionary<string, string> MakeScenarioParamToUploadBinMap
            (string substrateId, string ringId, int chipQty, double ringFrameAngle, double waferAngle, int countRow, int countCol, string nullBinCode, string mapData,
            string userId, bool useEventHandling, BinDataToUploadFromPWA500 bindata)
        {
            string recipeId = EquipmentInfo.GetRecipeId();
            string lotId = string.Empty;
            //string fileName = string.Empty;

            if (GetSubstrateByName(substrateId, out var substrate) ||
                GetSubstrateByName(ringId, out substrate))
            {
                lotId = substrate.LotId;
            }

            MapData e142Mapdata = new MapData();
            MapDataControl e142MapControl = new MapDataControl();
            PMSControl e142PMSControl = new PMSControl();
            Dictionary<string, List<string[]>> transferedDiesData = new Dictionary<string, List<string[]>>();
            (int refX, int refY) = FindReferencePosition(countCol, countRow, waferAngle, mapData, "D", nullBinCode);
            if (null != bindata.PmsFileBody)
            {
                transferedDiesData = e142PMSControl.GetTransferedData(bindata.PmsFileBody);
                e142Mapdata = e142MapControl.MakeBinMapObject(lotId, substrateId, recipeId, mapData, (int)waferAngle, countCol, countRow, chipQty, refX, refY, transferedDiesData);
            }

            string serializedMapdata = e142MapControl.SerializeMapData(e142Mapdata);

            Dictionary<string, string> scenarioParams = new Dictionary<string, string>
            {
                [UploadCoreOrBinFileKeys.KeyParamWaferId] = substrateId,
                [UploadMapKeys.KeyParamFilmFrameLocation] = ringFrameAngle.ToString(),
                [UploadMapKeys.KeyParamFlatNotchLocation] = waferAngle.ToString(),
                [UploadCoreOrBinFileKeys.KeyParamMapData] = serializedMapdata,
            };

            return scenarioParams;
        }
        public Dictionary<string, string> MakeScenarioParamToUploadBinData
            (string nameOfEq, string substrateId, string ringId,
            int chipQty, double angle, int countRow, int countCol, string nullBinCode, string mapData,
            string pmsFileBody, string userId, bool useEventHandling)
        {
            string recipeId = EquipmentInfo.GetRecipeId();
            Dictionary<string, string> scenarioParams = new Dictionary<string, string>
            {
                [UploadCoreOrBinFileKeys.KeyParamCarrierId] = string.Empty,
                [UploadCoreOrBinFileKeys.KeyParamPortId] = string.Empty,
                [UploadCoreOrBinFileKeys.KeyParamLotId] = string.Empty,
                [UploadCoreOrBinFileKeys.KeyParamPartId] = string.Empty,
                [UploadCoreOrBinFileKeys.KeyParamRecipeId] = recipeId,

                // 슬롯 번호가 없다??
                //[UploadCoreOrBinFileKeys.KeyParamSlotId] = (slot).ToString(),

                [UploadCoreOrBinFileKeys.KeyParamOperatorId] = userId,
                [UploadCoreOrBinFileKeys.KeyChipQty] = chipQty.ToString(),
                [UploadCoreOrBinFileKeys.KeyPMSFileName] = string.Empty,
                [UploadCoreOrBinFileKeys.KeyPMSFileBody] = string.Empty,

                [UploadCoreOrBinFileKeys.KeySubstrateName] = substrateId,

                [UploadCoreOrBinFileKeys.KeyWaferAngle] = angle.ToString(),
                [UploadCoreOrBinFileKeys.KeyCountRow] = countRow.ToString(),
                [UploadCoreOrBinFileKeys.KeyCountCol] = countCol.ToString(),
                [UploadCoreOrBinFileKeys.KeyReferenceX] = string.Empty,
                [UploadCoreOrBinFileKeys.KeyReferenceY] = string.Empty,
                [UploadCoreOrBinFileKeys.KeyStartingPosX] = string.Empty,
                [UploadCoreOrBinFileKeys.KeyStartingPosY] = string.Empty,
                [UploadCoreOrBinFileKeys.KeyNullBinCode] = nullBinCode,
                [UploadCoreOrBinFileKeys.KeyMapData] = mapData,

                [UploadCoreOrBinFileKeys.KeyUseEventHandling] = useEventHandling.ToString(),
            };

            if (GetSubstrateByName(substrateId, out var substrate) ||
                GetSubstrateByName(ringId, out substrate))
            {
                string lotId = substrate.LotId;
                int portId, slot;
                string partId = substrate.GetAttribute(PWA500SubstrateAttributes.PartId);


                //Dictionary<string, string> additionalParams = null;
                string fileName = string.Empty;
                portId = substrate.DestinationPortId;
                slot = substrate.DestinationSlot;
                fileName = GetPMSFileName(lotId, substrateId);
                if (false == MakePMSFile(lotId, substrateId, fileName, pmsFileBody, ref _pathForPms))
                    return scenarioParams;

                if (portId <= 0 || slot < 0)
                    return scenarioParams;

                string carrierId = _carrierServer.GetCarrierId(portId);

                scenarioParams[UploadCoreOrBinFileKeys.KeyParamCarrierId] = carrierId;
                scenarioParams[UploadCoreOrBinFileKeys.KeyParamPortId] = EquipmentInfo.GetPortName(portId);
                scenarioParams[UploadCoreOrBinFileKeys.KeyParamLotId] = lotId;
                scenarioParams[UploadCoreOrBinFileKeys.KeyParamPartId] = partId;
                scenarioParams[UploadCoreOrBinFileKeys.KeyParamRecipeId] = recipeId;

                // 슬롯 번호가 없다??
                //[UploadCoreOrBinFileKeys.KeyParamSlotId] = (slot).ToString();

                scenarioParams[UploadCoreOrBinFileKeys.KeyParamOperatorId] = userId;
                scenarioParams[UploadCoreOrBinFileKeys.KeyChipQty] = chipQty.ToString();
                scenarioParams[UploadCoreOrBinFileKeys.KeyPMSFileName] = fileName;
                scenarioParams[UploadCoreOrBinFileKeys.KeyPMSFileBody] = PmsFullPath;

                scenarioParams[UploadCoreOrBinFileKeys.KeySubstrateName] = substrateId;

                var (x, y) = FindReferencePosition(countCol, countRow, angle, mapData, "D", nullBinCode);

                scenarioParams[UploadCoreOrBinFileKeys.KeyWaferAngle] = angle.ToString();
                scenarioParams[UploadCoreOrBinFileKeys.KeyCountRow] = countRow.ToString();
                scenarioParams[UploadCoreOrBinFileKeys.KeyCountCol] = countCol.ToString();
                scenarioParams[UploadCoreOrBinFileKeys.KeyReferenceX] = x.ToString();
                scenarioParams[UploadCoreOrBinFileKeys.KeyReferenceY] = y.ToString();
                scenarioParams[UploadCoreOrBinFileKeys.KeyStartingPosX] = x.ToString();
                scenarioParams[UploadCoreOrBinFileKeys.KeyStartingPosY] = y.ToString();
                scenarioParams[UploadCoreOrBinFileKeys.KeyNullBinCode] = nullBinCode;
                scenarioParams[UploadCoreOrBinFileKeys.KeyMapData] = mapData;

                scenarioParams[UploadCoreOrBinFileKeys.KeyUseEventHandling] = useEventHandling.ToString();

                _substrateManager.SetAttributeByKey(substrate.UniqueKey, PWA500SubstrateAttributes.RefPositionX, x.ToString());
                _substrateManager.SetAttributeByKey(substrate.UniqueKey, PWA500SubstrateAttributes.RefPositionY, y.ToString());
                _substrateManager.SetAttributeByKey(substrate.UniqueKey, PWA500SubstrateAttributes.StartingPositionX, x.ToString());
                _substrateManager.SetAttributeByKey(substrate.UniqueKey, PWA500SubstrateAttributes.StartingPositionY, y.ToString());
                _substrateManager.SetAttributeByKey(substrate.UniqueKey, PWA500SubstrateAttributes.CountX, countCol.ToString());
                _substrateManager.SetAttributeByKey(substrate.UniqueKey, PWA500SubstrateAttributes.CountY, countRow.ToString());
                _substrateManager.SetAttributeByKey(substrate.UniqueKey, PWA500SubstrateAttributes.Angle, angle.ToString());
                _substrateManager.SetAttributeByKey(substrate.UniqueKey, PWA500SubstrateAttributes.MapData, mapData);
                _substrateManager.SetAttributeByKey(substrate.UniqueKey, PWA500SubstrateAttributes.ChipQty, chipQty.ToString());
                _substrateManager.SaveDataByKey(substrate.UniqueKey);

                //_substrateManager.SetAttributesByKey(substrate.UniqueKey, new Dictionary<string, string>
                //{
                //    [PWA500SubstrateAttributes.MapData] = mapData,
                //    [PWA500SubstrateAttributes.ChipQty] = chipQty.ToString(),
                //});

                // 2026.02.11. jhlim [ADD] 고객사 요청으로 생성된 PMS 파일을 특정 폴더에 모아서 백업한다.
                try
                {
                    var backupPath = Path.Combine(Define.DefineConstant.FilePath.FILEPATH_LOG, "PMSBackup");
                    var fileNameWithEx = Path.GetFileName(PmsFullPath);
                    var destFilePath = $@"{backupPath}\{fileNameWithEx}";

                    if (false == Directory.Exists(backupPath))
                        Directory.CreateDirectory(backupPath);

                    if (File.Exists(destFilePath))
                    {
                        File.Delete(destFilePath);
                    }

                    File.Copy(PmsFullPath, destFilePath);
                }
                catch (Exception)
                {
                }
                // 2026.02.11. jhlim [END]
                return scenarioParams;
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

            //        EnqueueScenarioAsync(scenario, scenarioParams, additionalParams);

            //        return true;

            //        // 2024.08.18 : [END]
            //    }
            //}

            return scenarioParams;
        }

        private static (int X, int Y) FindReferencePosition(int countX, int countY, double angle,
            string mapData, string notch, string nullBincode)
        {
            CcwRotation ccw;
            if (angle == 90)
                ccw = CcwRotation.Deg90;
            else if (angle == 180)
                ccw = CcwRotation.Deg180;
            else if (angle == 270)
                ccw = CcwRotation.Deg270;
            else
                ccw = CcwRotation.Deg0;

            return ReferenceFinder.GetPosition(countX, countY, ccw, mapData, notch, nullBincode);
        }

        public void SetScenarioError(EN_SCENARIO failedScenario, string description = "")
        {
            FailedScenarioTypes = failedScenario;
            HasScenarioError = true;
            ScenarioErrorDescription = description;
        }
        public void ExecuteAfterScenarioCompletion(EN_SCENARIO typeOfScenario,
            Dictionary<string, string> scenarioParams,
            Dictionary<string, string> resultOfScenario,
            Dictionary<string, string> additionalParams,
            EN_MESSAGE_RESULT result)
        {
            // 완료된 시나리오 타입에 따라 실행되어야할 액션을 여기서 선택한다.
            switch (typeOfScenario)
            {
                case EN_SCENARIO.SCENARIO_WORK_START:
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
                        if (additionalParams == null)
                        {
                            if (false == FindSubstrateByNameOrRingIdAtProcessModule(resultSubstrateId, resultSubstrateId, out substrate, out _) || substrate == null)
                                return;

                            SetSubstrateAttributes(substrate,
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

                            if (FindSubstrateByNameOrRingIdAtProcessModule(substrateId, substrateId, out substrate, out _))
                            {
                                SetSubstrateAttributes(substrate,
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

                                _funcToSendClientMessage(nameOfEq, MessagesToSend.ResponseDownloadMapFile.ToString(),
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
                                    _funcToSendClientMessage(nameOfEq, MessagesToSend.ResponseDownloadMapFile.ToString(),
                                        string.Empty, string.Empty,
                                        messageContentToSend.Keys.ToArray(), messageContentToSend.Values.ToArray(),
                                        EN_MESSAGE_RESULT.NG, true);

                                    // TODO : 알람 발생 필요
                                }
                                else
                                {
                                    // Gem이 꺼져있으면 다운받은 맵을 전달
                                    _funcToSendClientMessage(nameOfEq, MessagesToSend.ResponseDownloadMapFile.ToString(),
                                            string.Empty, string.Empty,
                                            messageContentToSend.Keys.ToArray(), messageContentToSend.Values.ToArray(),
                                            result, true);
                                }
                                // 2025.07.16. jhlim [END]

                                // 2024.12.31. jhlim [ADD] NG 시 리턴 누락
                                return;
                            }

                            #region
                            if (false == additionalParams.TryGetValue(AdditionalParamKeys.KeyRingId, out string ringId))
                                return;

                            if (false == additionalParams.TryGetValue(AdditionalParamKeys.KeyUserId, out string userId))
                                return;

                            // Work_start 이후 발생하도록 수정 필요 -> ResponseDownloadMapFile 후 WaferSplitEvent 발생하도록 수정 필요
                            //int portId = substrate.SourcePortId;
                            //if (false == _carrierServer.HasCarrier(portId))
                            //    return;

                            string isLastString = substrate.GetAttribute(PWA500SubstrateAttributes.IsLastSubstrate);
                            bool.TryParse(isLastString, out bool isLast);
                            //bool isLast = _substrateManager.IsLastSubstrateAtLoadPort(portId, substrateId);
                            ExecuteScenarioToSplitWafer(nameOfEq, substrate.Name, ringId, userId, isLast);
                            #endregion
                        }

                        #endregion
                    }
                    break;
                case EN_SCENARIO.SCENARIO_WORK_END:
                    {
                        if (additionalParams == null || additionalParams.Count == 0)
                            return;

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
                        #endregion

                        if (false == additionalParams.TryGetValue(AdditionalParamKeys.KeyNameOfEq, out string nameOfEq))
                        {
                            SetScenarioError(typeOfScenario);
                            return;
                        }

                        if (result.Equals(EN_MESSAGE_RESULT.NG))
                        {
                            SetScenarioError(typeOfScenario);

                            _funcToSendClientMessage(nameOfEq, MessagesToSend.ResponseUploadCoreFile.ToString(),
                                string.Empty, string.Empty,
                                messageContentToSend.Keys.ToArray(), messageContentToSend.Values.ToArray(),
                                result, true);

                            //FrameOfSystem3.Task.TaskOperator.GetInstance().SetOperation(RunningMain_.OPERATION_EQUIPMENT.STOP);
                            return;
                        }

                        // 2024.08.18 : [START] 코어맵 핸들링만 사용하는 경우 이후 시나리오를 무시한다.
                        if (UseCoreMapHandlingOnly)
                        {
                            _funcToSendClientMessage(nameOfEq, MessagesToSend.ResponseUploadCoreFile.ToString(),
                                string.Empty, string.Empty,
                                messageContentToSend.Keys.ToArray(), messageContentToSend.Values.ToArray(),
                                result, true);

                            return;
                        }
                        // [END]

                        #region
                        // Track Out
                        if (false == additionalParams.TryGetValue(AdditionalParamKeys.KeySubstrateId, out string substrateId))
                            return;
                        if (false == additionalParams.TryGetValue(AdditionalParamKeys.KeyChipQty, out string qty))
                            return;
                        if (false == int.TryParse(qty, out int chipQty))
                            return;
                        if (false == additionalParams.TryGetValue(AdditionalParamKeys.KeyUserId, out string userId))
                            return;
                        #region

                        // Process End
                        if (FindSubstrateByNameOrRingIdAtProcessModule(substrateId, substrateId, out var substrate, out _))
                        {
                            int portId = substrate.SourcePortId;
                            string isLastString = substrate.GetAttribute(PWA500SubstrateAttributes.IsLastSubstrate);
                            bool.TryParse(isLastString, out bool isLast);
                            //if (isLast)
                            //{
                            //    var scenarioParam = new Dictionary<string, string>
                            //    {
                            //        [EESKeys.KeyCarrierId] = _carrierServer.GetCarrierId(portId),
                            //        [EESKeys.KeyPortId] = GetPortName(portId),
                            //        [EESKeys.KeyLotId] = substrate.LotId,
                            //        [EESKeys.KeyPartId] = substrate.GetAttribute(PWA500SubstrateAttributes.PartId),
                            //        [EESKeys.KeyParamRecipeId] = GetRecipeId(),
                            //        [EESKeys.KeyOperatorId] = "AUTO"
                            //    };

                            //    _actionToEnqueueScenarioAsync(EN_SCENARIO.SCENARIO_PROCESS_END, scenarioParam, null);
                            //}

                            string carrierId = _carrierServer.GetCarrierId(portId);
                            _lotHistoryLog.WriteSubstrateHistoryForWorkEnd(portId, carrierId, substrateId, qty);

                            // 2025.02.04. jhlim [ADD] 트랙아웃 이미 진행되었는지 검사
                            string isTrackoutCompleted = substrate.GetAttribute(PWA500SubstrateAttributes.IsTrackOutCompleted);
                            if (isTrackoutCompleted.Equals(bool.TrueString))
                            {
                                // 문자열이 True면 트랙아웃 패스
                                return;
                            }
                            // 2025.02.04. jhlim [END]
                        }
                        #endregion

                        if (chipQty <= 0)
                            return;

                        bool executeTrackOutAtCurrentEvent = true;
                        executeTrackOutAtCurrentEvent &= UseTrackOutCore;
                        string scrapQtyString = string.Empty, scrapInfo = string.Empty;
                        if (additionalParams.TryGetValue(ScrapInfoKeys.KeyScrapQty, out scrapQtyString) &&
                            additionalParams.TryGetValue(ScrapInfoKeys.KeyScrapData, out scrapInfo))
                        {
                            int scrapQty = 0;
                            int.TryParse(scrapQtyString, out scrapQty);
                            if (scrapQty > 0)
                            {
                                // 스크랩 할 것이 있으면 트랙아웃을 진행하지 않는다.
                                executeTrackOutAtCurrentEvent = false;
                            }
                        }

                        if (false == string.Equals(substrate.GetAttribute(PWA500SubstrateAttributes.ChipQty), qty, StringComparison.OrdinalIgnoreCase))
                        {
                            _substrateManager.SetAttributeByKey(substrate.UniqueKey, PWA500SubstrateAttributes.ChipQty, qty);
                            _substrateManager.SaveDataByKey(substrate.UniqueKey);
                        }

                        if (executeTrackOutAtCurrentEvent)
                        {
                            // 스크랩을 사용하는 경우, 스크랩 이벤트 완료 후 응답한다.
                            _funcToSendClientMessage(nameOfEq, MessagesToSend.ResponseUploadCoreFile.ToString(),
                                string.Empty, string.Empty,
                                messageContentToSend.Keys.ToArray(), messageContentToSend.Values.ToArray(),
                                result, true);

                            if (false == ExecuteScenarioToTrackOut(substrate.UniqueKey, substrateId, chipQty, userId, true))
                            {
                                SetScenarioError(typeOfScenario);
                                return;
                            }
                        }
                        else
                        {
                            ExecuteScenarioToScrapCoreChips(substrate, nameOfEq, scrapQtyString, scrapInfo, userId);
                        }
                        #endregion
                    }
                    break;
                case EN_SCENARIO.SCENARIO_REQ_CORE_WAFER_TRACK_OUT:
                    {
                        if (additionalParams == null ||
                            false == additionalParams.TryGetValue(AdditionalParamKeys.KeySubstrateId, out string substrateId))
                            return;

                        if (FindSubstrateByNameOrRingIdAtProcessModule(substrateId, substrateId, out var substrate, out _))
                        {
                            int portId = substrate.SourcePortId;
                            string lotId = substrate.LotId;

                            string carrierId = substrate.SourceCarrierId;
                            string chipQty = substrate.GetAttribute(PWA500SubstrateAttributes.ChipQty);
                            var isLast = substrate.GetAttribute(PWA500SubstrateAttributes.IsLastSubstrate);
                            _lotHistoryLog.WriteSubstrateHistoryForTrackOut(portId, carrierId, substrateId, lotId, chipQty, isLast.Equals(bool.TrueString));

                            // 2025.02.04. jhlim [ADD] 트랙아웃 진행 했다고 속성을 설정한다.
                            _substrateManager.SetAttributeByKey(substrate.UniqueKey, PWA500SubstrateAttributes.IsTrackOutCompleted, bool.TrueString);
                            // 2025.02.04. jhlim [END]
                        }
                    }
                    break;

                case EN_SCENARIO.SCENARIO_REQ_CORE_WAFER_SPLIT:
                case EN_SCENARIO.SCENARIO_REQ_CORE_WAFER_SPLIT_LAST:
                    {
                        if (result.Equals(EN_MESSAGE_RESULT.NG))
                        {
                            SetScenarioError(typeOfScenario);
                            //FrameOfSystem3.Task.TaskOperator.GetInstance().SetOperation(RunningMain_.OPERATION_EQUIPMENT.STOP);
                            return;
                        }

                        if (false == scenarioParams.TryGetValue(AssignSubstrateLotIdKeys.KeyParamWaferId, out string substrateId))
                            return;

                        if (false == FindSubstrateByNameOrRingIdAtProcessModule(substrateId, substrateId, out var substrate, out _) || substrate == null)
                            return;

                        string targetLotId = string.Empty, receivedPartId = string.Empty;
                        int portId = substrate.SourcePortId;
                        bool partIdError = false;
                        if (typeOfScenario.Equals(EN_SCENARIO.SCENARIO_REQ_CORE_WAFER_SPLIT))
                        {
                            if (false == resultOfScenario.TryGetValue(AssignSubstrateLotIdKeys.KeyResultLotId, out targetLotId))
                                return;

                            if (false == resultOfScenario.TryGetValue(AssignSubstrateLotIdKeys.KeyResultPartId, out receivedPartId))
                            {
                                partIdError = true;
                                //SetScenarioError(typeOfScenario, "Does not have Part Id Info");
                                //return;
                            }
                        }
                        else
                        {
                            if (false == _carrierServer.HasCarrier(portId))
                                return;

                            targetLotId = _carrierServer.GetCarrierLotId(portId);
                            receivedPartId = substrate.GetAttribute(PWA500SubstrateAttributes.PartId);
                            if (string.IsNullOrEmpty(receivedPartId))
                            {
                                partIdError = true;
                            }
                        }

                        string oldLotId = substrate.LotId;
                        string carrierId = _carrierServer.GetCarrierId(portId);
                        _lotHistoryLog.WriteSubstrateHistoryForWaferSplit(portId, carrierId, substrateId, oldLotId, targetLotId, typeOfScenario.Equals(EN_SCENARIO.SCENARIO_REQ_CORE_WAFER_SPLIT_LAST));

                        //substrate.SetLotId(targetLotId);
                        _substrateManager.SetLotIdByKey(substrate.UniqueKey, targetLotId);
                        _substrateManager.SaveDataByKey(substrate.UniqueKey);

                        if (additionalParams != null)
                        {
                            if (additionalParams.TryGetValue(AdditionalParamKeys.KeyNameOfEq, out string nameOfEq))
                            {
                                if (UseComparePartId)
                                {
                                    var partId = substrate.GetAttribute(PWA500SubstrateAttributes.PartId);
                                    if (false == receivedPartId.Equals(partId))
                                    {
                                        _funcToSendClientMessage(nameOfEq, MessagesToSend.RequestStop.ToString(), string.Empty, string.Empty, new string[] { }, new string[] { }, EN_MESSAGE_RESULT.OK, false);
                                        SetScenarioError(typeOfScenario, string.Format("Different Part Info -> Prev:{0}, New:{1}", partId, receivedPartId));
                                        return;
                                    }
                                }

                                var messageContentToSend = new Dictionary<string, string>();
                                messageContentToSend[AssignSubstrateLotIdKeys.KeySubstrateName] = substrateId;
                                messageContentToSend[AssignSubstrateLotIdKeys.KeyLotId] = targetLotId;

                                _funcToSendClientMessage(nameOfEq, MessagesToSend.RequestAssignLotId.ToString(),
                                    string.Empty, string.Empty,
                                    messageContentToSend.Keys.ToArray(), messageContentToSend.Values.ToArray(),
                                    result, true);
                            }
                        }

                        if (partIdError && UseComparePartId)
                        {
                            SetScenarioError(typeOfScenario, "Does not have Part Id Info");
                            return;
                        }
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
                        #endregion
                    }
                    break;

                case EN_SCENARIO.SCENARIO_REQ_CORE_CHIP_SPLIT_FIRST:
                case EN_SCENARIO.SCENARIO_REQ_CORE_CHIP_SPLIT:
                case EN_SCENARIO.SCENARIO_REQ_CORE_CHIP_FULL_SPLIT_FIRST:
                case EN_SCENARIO.SCENARIO_REQ_CORE_CHIP_FULL_SPLIT:
                    {
                        if (result.Equals(EN_MESSAGE_RESULT.NG))
                        {
                            SetScenarioError(typeOfScenario);
                            //FrameOfSystem3.Task.TaskOperator.GetInstance().SetOperation(RunningMain_.OPERATION_EQUIPMENT.STOP);
                            return;
                        }


                        if (additionalParams == null || additionalParams.Count == 0)
                            return;

                        #region
                        // 스플릿 이벤트 전송 후 리스폰스 전송
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

                        if (additionalParams.TryGetValue(AdditionalParamKeys.KeyNameOfEq, out string nameOfEq))
                        {
                            ExecuteToSendSimpleResultToClient(result, MessagesToSend.ResponseSplitCoreChip.ToString(), nameOfEq);
                        }
                        else
                            return;

                        if (result.Equals(EN_MESSAGE_RESULT.NG))
                            return;
                        #endregion

                        #region

                        // LotId 할당된 것을 설정
                        if (false == resultOfScenario.TryGetValue(SplitCoreChipKeys.KeyResultLotId, out string lotId))
                        {
                            return;
                        }
                        if (false == scenarioParams.TryGetValue(SplitCoreChipKeys.KeyParamSplitWaferId, out string coreSubstrateId))
                        {
                            return;
                        }
                        if (false == scenarioParams.TryGetValue(SplitCoreChipKeys.KeyParamRingFrameId, out string substrateId))
                        {
                            return;
                        }
                        if (false == scenarioParams.TryGetValue(SplitCoreChipKeys.KeyParamBinType, out string binType))
                        {
                            return;
                        }

                        if (FindSubstrateByNameOrRingIdAtProcessModule(substrateId, substrateId, out var binSubstrate, out _))
                        {
                            string chipQtyToSplit;
                            if (false == scenarioParams.TryGetValue(SplitCoreChipKeys.KeyParamSplitChipQty, out chipQtyToSplit))
                                chipQtyToSplit = "0";

                            string historyForBin = $"{lotId}:{coreSubstrateId}:{chipQtyToSplit}";
                            if (typeOfScenario.Equals(EN_SCENARIO.SCENARIO_REQ_CORE_CHIP_SPLIT_FIRST) ||
                                typeOfScenario.Equals(EN_SCENARIO.SCENARIO_REQ_CORE_CHIP_FULL_SPLIT_FIRST))
                            {
                                //binSubstrate.SetLotId(lotId);
                                _substrateManager.SetLotIdByKey(binSubstrate.UniqueKey, lotId);
                                _substrateManager.SetAttributeByKey(binSubstrate.UniqueKey, PWA500SubstrateAttributes.ChipQty, chipQtyToSplit);
                                _substrateManager.SetAttributeByKey(binSubstrate.UniqueKey, PWA500SubstrateAttributes.SplittedHistory, historyForBin);

                                _substrateManager.SaveDataByKey(binSubstrate.UniqueKey);

                            }
                            else
                            {
                                _substrateManager.SetAttributeByKey(binSubstrate.UniqueKey, PWA500SubstrateAttributes.SplittedLotId, lotId);

                                var prevHistory = _substrateManager.GetAttributeByKey(binSubstrate.UniqueKey, PWA500SubstrateAttributes.SplittedHistory);
                                _substrateManager.SetAttributeByKey(binSubstrate.UniqueKey, PWA500SubstrateAttributes.SplittedHistory, $"{prevHistory},{historyForBin}");

                                _substrateManager.SaveDataByKey(binSubstrate.UniqueKey);

                                // 기존 값을 읽어와 받은 데이터를 더한다.
                                //string qtyByString = binSubstrate.GetAttribute(PWA500BINSubstrateAttributes.ChipQty);
                                //if (false == int.TryParse(qtyByString, out int chipQty))
                                //    chipQty = 0;
                                //if (false == int.TryParse(chipQtyToIncreaseByString, out int chipQtyToIncrease))
                                //    chipQtyToIncrease = 0;

                                //int totalQty = chipQty + chipQtyToIncrease;
                                //binSubstrate.SetAttribute(PWA500BINSubstrateAttributes.ChipQty, totalQty.ToString());

                                string lotIdForParent = binSubstrate.LotId;
                                // 토탈이 아닌 증가되는 양만 머지한다. 여기서 수량이 계속 증가되는듯..
                                ExecuteScenarioToChipMerge(lotIdForParent, lotId, coreSubstrateId, substrateId, binType, chipQtyToSplit/*totalQty.ToString()*/);
                            }

                            if (FindSubstrateByNameOrRingIdAtProcessModule(coreSubstrateId, coreSubstrateId, out var coreSubstrate, out _))
                            {
                                int corePortId = coreSubstrate.SourcePortId;
                                int binPortId = binSubstrate.SourcePortId;

                                bool splitFirst = typeOfScenario.Equals(EN_SCENARIO.SCENARIO_REQ_CORE_CHIP_SPLIT_FIRST) ||
                                    typeOfScenario.Equals(EN_SCENARIO.SCENARIO_REQ_CORE_CHIP_FULL_SPLIT_FIRST);

                                bool splitFully = typeOfScenario.Equals(EN_SCENARIO.SCENARIO_REQ_CORE_CHIP_FULL_SPLIT_FIRST) ||
                                    typeOfScenario.Equals(EN_SCENARIO.SCENARIO_REQ_CORE_CHIP_FULL_SPLIT);

                                string carrierId = _carrierServer.GetCarrierId(corePortId);

                                string historyForCore = $"{lotId}:{substrateId}:{chipQtyToSplit}";
                                var prevHistory = _substrateManager.GetAttributeByKey(coreSubstrate.UniqueKey, PWA500SubstrateAttributes.SplittedHistory);
                                if (string.IsNullOrWhiteSpace(prevHistory))
                                {
                                    _substrateManager.SetAttributeByKey(coreSubstrate.UniqueKey, PWA500SubstrateAttributes.SplittedHistory, historyForCore);
                                }
                                else
                                {
                                    _substrateManager.SetAttributeByKey(coreSubstrate.UniqueKey, PWA500SubstrateAttributes.SplittedHistory, $"{prevHistory},{historyForCore}");
                                }

                                _substrateManager.SaveDataByKey(coreSubstrate.UniqueKey);

                                _lotHistoryLog.WriteSubstrateHistoryForChipSplit(corePortId, carrierId, coreSubstrateId, binPortId, substrateId, chipQtyToSplit, binType, lotId, splitFirst, splitFully);
                            }
                        }
                        #endregion
                    }
                    break;

                case EN_SCENARIO.SCENARIO_REQ_CORE_CHIP_MERGE:
                    {
                        if (result.Equals(EN_MESSAGE_RESULT.NG))
                        {
                            SetScenarioError(typeOfScenario);
                            //FrameOfSystem3.Task.TaskOperator.GetInstance().SetOperation(RunningMain_.OPERATION_EQUIPMENT.STOP);
                        }
                    }
                    break;

                case EN_SCENARIO.SCENARIO_BIN_WAFER_ID_READ:
                    {
                        if (additionalParams == null || additionalParams.Count == 0)
                            return;

                        #region
                        if (false == additionalParams.TryGetValue(AdditionalParamKeys.KeyNameOfEq, out string nameOfEq))
                            return;

                        if (false == additionalParams.TryGetValue(AdditionalParamKeys.KeyMessageNameToSend, out string messageNameToSend))
                            return;

                        ExecuteToSendSimpleResultToClient(result, messageNameToSend, nameOfEq);
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
                            SetScenarioError(typeOfScenario);
                            ExecuteToSendSimpleResultToClient(EN_MESSAGE_RESULT.NG, messageNameToSend, nameOfEq, "Does not have ring id or substrate name");
                        }
                        else
                        {
                            if (false == resultOfScenario.TryGetValue(AssignSubstrateIdKeys.KeyResultSubstrateId, out string newSubstrateName))
                            {
                                SetScenarioError(typeOfScenario);
                                ExecuteToSendSimpleResultToClient(EN_MESSAGE_RESULT.NG, messageNameToSend, nameOfEq, "SECS/GEM Scenario Error!");
                            }
                            else
                            {
                                string pmName = _processGroup.GetProcessModuleName(ProcessModuleIndex);
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

                                            _funcToSendClientMessage(nameOfEq, messageNameToSend,
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

                                    _funcToSendClientMessage(nameOfEq, messageNameToSend,
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

                case EN_SCENARIO.SCENARIO_BIN_WORK_END:
                    {
                        #region
                        // Track Out
                        //if (false == additionalParams.TryGetValue(AdditionalParamKeys.KeySubstrateId, out string substrateId))
                        //    return;
                        //if (false == additionalParams.TryGetValue(AdditionalParamKeys.KeyChipQty, out string qty))
                        //    return;
                        //if (false == int.TryParse(qty, out int chipQty))
                        //    return;

                        //ExecuteScenarioToTrackOut(substrateId, chipQty, "AUTO", false);
                        #endregion
                    }
                    break;
                case EN_SCENARIO.SCENARIO_REQ_BIN_WAFER_ID_ASSIGN:
                    {
                        // Robot에서 발생시키도록 시나리오 변경됨
                        //if (false == isManual)
                        //    return;

                        //#region
                        //if (false == scenarioParams.TryGetValue(AssignSubstrateIdKeys.KeyParamRingFrameId, out string ringId))
                        //    return;

                        //Substrate binSubstrate = new Substrate();
                        //if (false == _substrateManager.GetSubstrateByName(ringId, ref binSubstrate))
                        //    return;

                        //if (false == resultOfScenario.TryGetValue(AssignSubstrateIdKeys.KeyResultSubstrateId, out string newSubstrateId))
                        //{
                        //    return;
                        //}
                        //binSubstrate.SetName(newSubstrateId);
                        //#endregion
                    }
                    break;
                case EN_SCENARIO.SCENARIO_BIN_SORTING_END_1:
                case EN_SCENARIO.SCENARIO_BIN_SORTING_END_2:
                case EN_SCENARIO.SCENARIO_BIN_SORTING_END_3:
                    {
                        if (additionalParams == null || additionalParams.Count == 0)
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

                        _funcToSendClientMessage(nameOfEq, MessagesToSend.ResponseFinishSorting.ToString(),
                            string.Empty, string.Empty,
                            messageContentToSend.Keys.ToArray(), messageContentToSend.Values.ToArray(),
                            result, true);
                        //if (result.Equals(EN_MESSAGE_RESULT.NG))
                        //{

                        //    Dictionary<string, string> messageContentToSend = new Dictionary<string, string>
                        //    {
                        //        [ResultKeys.KeyResult] = EN_MESSAGE_RESULT.NG.ToString(),
                        //        [ResultKeys.KeyDescription] = "Gem Error",
                        //    };

                        //    SendClientToClientMessage(nameOfEq, MessagesToSend.ResponseFinishSorting.ToString(),
                        //        string.Empty, string.Empty,
                        //        messageContentToSend.Keys.ToArray(), messageContentToSend.Values.ToArray(),
                        //        EN_MESSAGE_RESULT.NG, true);
                        //}
                        //else
                        //{
                        //    ExecuteScenarioToAssignSubstrateId(nameOfEq, ringId, substrateType);
                        //}
                        #endregion
                    }
                    break;
                case EN_SCENARIO.SCENARIO_SCRAP_CORE_CHIP:
                    {
                        if (additionalParams == null || additionalParams.Count == 0)
                            return;

                        string substrateKey = string.Empty;

                        if (false == additionalParams.TryGetValue(AdditionalParamKeys.KeyNameOfEq, out string nameOfEq) ||
                            false == additionalParams.TryGetValue(AdditionalParamKeys.KeySubstrateKey, out substrateKey))
                        {
                            result = EN_MESSAGE_RESULT.NG;
                        }

                        if (false == _substrateManager.GetSubstrateByKey(substrateKey, out var substrate))
                        {
                            result = EN_MESSAGE_RESULT.NG;
                        }

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
                        #endregion

                        _funcToSendClientMessage(nameOfEq, MessagesToSend.ResponseUploadCoreFile.ToString(),
                            string.Empty, string.Empty,
                            messageContentToSend.Keys.ToArray(), messageContentToSend.Values.ToArray(),
                            result, true);

                        if (result == EN_MESSAGE_RESULT.NG)
                        {
                            SetScenarioError(typeOfScenario);
                            return;
                        }

                        var chipQtyString = substrate.GetAttribute(PWA500SubstrateAttributes.ChipQty);
                        int.TryParse(chipQtyString, out var chipQty);
                        string userId = "AUTO";
                        additionalParams.TryGetValue(AdditionalParamKeys.KeyUserId, out userId);
                        additionalParams.TryGetValue(ScrapInfoKeys.KeyParamScrapInfo, out var scrapInfo);
                        _substrateManager.SetAttributeByKey(substrate.UniqueKey, PWA500SubstrateAttributes.ScrapInfo, scrapInfo);
                        _substrateManager.SaveDataByKey(substrate.UniqueKey);

                        if (false == ExecuteScenarioToTrackOut(
                            substrate.UniqueKey,
                            substrate.Name,
                            chipQty, userId, true))
                        {
                            SetScenarioError(typeOfScenario);
                            return;
                        }
                    }
                    break;

                default:
                    break;
            }
        }

        private void ExecuteScenarioToScrapCoreChips(
            Substrate substrate,
            string nameOfEq,
            string scrapQty,
            string scrapInfo,
            string userId)
        {
            // 2025.05.08. jhlim [ADD] 스크랩 정보 업데이트(공정설비 미 업데이트 시 발생하지 않도록함)

            // TODO : 진행 도중 에러나면 중단된 정보를 저장하는 부분이 나중에 필요할 수 있음
            string substrateId = substrate.Name;
            string lotId = substrate.LotId;
            if (false == CreateScenarioParamToScrapInfo(
                isCore: true,
                lotId: lotId,
                substrateId: substrateId,
                scrapQty: scrapQty,
                scrapData: scrapInfo,
                userId: string.Empty,
                scenarioParam: out var param))
            {
                // TODO : 로그 추가 필요

            }

            var additional = new Dictionary<string, string>
            {
                [AdditionalParamKeys.KeyNameOfEq] = nameOfEq,
                [AdditionalParamKeys.KeyLotId] = lotId,
                [AdditionalParamKeys.KeySubstrateKey] = substrate.UniqueKey,
                [AdditionalParamKeys.KeySubstrateId] = substrateId,
                [AdditionalParamKeys.KeyUserId] = userId,
                [ScrapInfoKeys.KeyParamScrapInfo] = scrapInfo,
            };

            _actionToEnqueueScenarioAsync(
                EN_SCENARIO.SCENARIO_SCRAP_CORE_CHIP,
                param,
                additional);
        }
        public bool CreateScenarioParamToScrapInfo(
            bool isCore,
            string lotId,
            string substrateId,
            string scrapQty,
            string scrapData,
            string userId,
            out Dictionary<string, string> scenarioParam)
        {
            scenarioParam = new Dictionary<string, string>();
            scenarioParam[ScrapInfoKeys.KeyParamLotId] = lotId;
            scenarioParam[ScrapInfoKeys.KeyParamWaferId] = substrateId;
            scenarioParam[ScrapInfoKeys.KeyParamScrapQty] = scrapQty;
            scenarioParam[ScrapInfoKeys.KeyParamScrapInfo] = scrapData;
            if (false == isCore)
            {
                scenarioParam[ScrapInfoKeys.KeyParamOperatorId] = userId;
            }
            else
            {
                scenarioParam[ScrapInfoKeys.KeyParamWaferQty] = "1";
            }

            return true;
        }
        private bool ExecuteToSendSimpleResultToClient(EN_MESSAGE_RESULT result, string messageNameToSend, string nameOfEq, string description = "")
        {
            if (_funcToSendClientMessage == null)
                return false;

            if (messageNameToSend == null || string.IsNullOrEmpty(messageNameToSend))
                return true;

            Dictionary<string, string> messageContentToSend = new Dictionary<string, string>
            {
                [ResultKeys.KeyResult] = result.ToString(),
                [ResultKeys.KeyDescription] = description
            };

            return _funcToSendClientMessage(nameOfEq, messageNameToSend.ToString(),
                        string.Empty, string.Empty,
                        messageContentToSend.Keys.ToArray(),
                        messageContentToSend.Values.ToArray(),
                        result, true);
        }
        private bool ExecuteScenarioToChipMerge(string lotId, string lotIdToMerge, string coreWaferId, string binRingId, string binType, string chipQty)
        {
            if (_actionToEnqueueScenarioAsync == null)
                return false;

            Dictionary<string, string> scenarioParam = new Dictionary<string, string>();
            scenarioParam[SplitCoreChipKeys.KeyParamLotId] = lotId;
            scenarioParam[SplitCoreChipKeys.KeyParamSplitLotId] = lotIdToMerge;
            scenarioParam[SplitCoreChipKeys.KeyParamSplitWaferId] = coreWaferId;
            scenarioParam[SplitCoreChipKeys.KeyParamRingFrameId] = binRingId;
            scenarioParam[SplitCoreChipKeys.KeyParamBinType] = binType;
            scenarioParam[SplitCoreChipKeys.KeyParamSplitChipQty] = chipQty;

            _actionToEnqueueScenarioAsync(EN_SCENARIO.SCENARIO_REQ_CORE_CHIP_MERGE, scenarioParam, null);

            return true;
        }
        private bool ExecuteScenarioToSplitWafer(string nameOfEq, string substrateId, string ringId, string userId, bool isLast)
        {
            if (_actionToEnqueueScenarioAsync == null)
                return false;

            if (false == FindSubstrateByNameOrRingIdAtProcessModule(substrateId, substrateId, out var substrate, out _) || substrate == null)
                return false;

            var scenarioParam = new Dictionary<string, string>
            {
                [AssignSubstrateLotIdKeys.KeyParamLotId] = substrate.LotId,
                [AssignSubstrateLotIdKeys.KeyParamWaferId] = substrateId,
                [AssignSubstrateLotIdKeys.KeyParamPartId] = substrate.GetAttribute(PWA500SubstrateAttributes.PartId),
                [AssignSubstrateLotIdKeys.KeyParamRecipeId] = EquipmentInfo.GetRecipeId(),
                [AssignSubstrateLotIdKeys.KeyParamSlotId] = (substrate.SourceSlot).ToString(),
                [AssignSubstrateLotIdKeys.KeyParamOperatorId] = userId
            };

            Dictionary<string, string> additionalParams = new Dictionary<string, string>
            {
                [AdditionalParamKeys.KeyNameOfEq] = nameOfEq,
                [AdditionalParamKeys.KeySubstrateId] = substrateId,
                [AdditionalParamKeys.KeyRingId] = ringId
            };

            if (false == _recipe.GetValue(EN_RECIPE_TYPE.COMMON, PARAM_COMMON.UseSecsGem.ToString(), true))
            {
                var messageContentToSend = new Dictionary<string, string>();
                messageContentToSend[AssignSubstrateLotIdKeys.KeySubstrateName] = substrateId;
                messageContentToSend[AssignSubstrateLotIdKeys.KeyLotId] = substrate.LotId;

                _funcToSendClientMessage(nameOfEq, MessagesToSend.RequestAssignLotId.ToString(),
                    string.Empty, string.Empty,
                    messageContentToSend.Keys.ToArray(), messageContentToSend.Values.ToArray(), EN_MESSAGE_RESULT.OK
                    , true);

                return true;
            }

            EN_SCENARIO scenario;
            if (false == isLast)
            {
                scenario = EN_SCENARIO.SCENARIO_REQ_CORE_WAFER_SPLIT;
            }
            else
            {
                scenario = EN_SCENARIO.SCENARIO_REQ_CORE_WAFER_SPLIT_LAST;
            }

            _actionToEnqueueScenarioAsync(scenario, scenarioParam, additionalParams);
            return true;
        }
        private bool ExecuteScenarioToTrackOut(string substrateKey, string substrateId, int chipQty, string userId, bool isCore)
        {
            if (_actionToEnqueueScenarioAsync == null)
                return false;

            EN_SCENARIO scenario;
            if (false == isCore)
            {
                scenario = EN_SCENARIO.SCENARIO_REQ_BIN_WAFER_TRACK_OUT;
            }
            else
            {
                scenario = EN_SCENARIO.SCENARIO_REQ_CORE_WAFER_TRACK_OUT;
            }
            var scenarioParams = ScenarioParameterBuilder.MakeScenarioParamToTrackOut(substrateKey, userId, isCore);
            if (scenarioParams == null)
                return false;

            Dictionary<string, string> additionalParams = new Dictionary<string, string>();
            additionalParams[AdditionalParamKeys.KeySubstrateId] = substrateId;

            _actionToEnqueueScenarioAsync(scenario, scenarioParams, additionalParams);

            return true;
        }
        public void SetSubstrateAttributes(Substrate substrate, string substrateId, string angle, string countRow, string countCol, string qty, string referenceX, string referenceY, string startingX, string startingY, string mapData)
        {
            //substrate.SetName(substrateId);
            _substrateManager.SetNameByKey(substrate.UniqueKey, substrateId);
            _substrateManager.SetAttributeByKey(substrate.UniqueKey, PWA500SubstrateAttributes.Angle, angle);
            _substrateManager.SetAttributeByKey(substrate.UniqueKey, PWA500SubstrateAttributes.CountX, countRow);
            _substrateManager.SetAttributeByKey(substrate.UniqueKey, PWA500SubstrateAttributes.CountY, countCol);
            _substrateManager.SetAttributeByKey(substrate.UniqueKey, PWA500SubstrateAttributes.ChipQty, qty);
            _substrateManager.SetAttributeByKey(substrate.UniqueKey, PWA500SubstrateAttributes.RefPositionX, referenceX);
            _substrateManager.SetAttributeByKey(substrate.UniqueKey, PWA500SubstrateAttributes.RefPositionY, referenceY);
            _substrateManager.SetAttributeByKey(substrate.UniqueKey, PWA500SubstrateAttributes.StartingPositionX, startingX);
            _substrateManager.SetAttributeByKey(substrate.UniqueKey, PWA500SubstrateAttributes.StartingPositionY, startingY);
            _substrateManager.SetAttributeByKey(substrate.UniqueKey, PWA500SubstrateAttributes.MapData, mapData);

            _substrateManager.SaveDataByKey(substrate.UniqueKey);
        }

        #region <Abstract>
        public virtual SubstrateType GetSubstrateTypeByLoadPortIndex(int lpIndex)
        {
            return SubstrateType.Core;
        }
        #endregion </Abstract>

        #region <Substrate>
        public void AssignSubstrateInfoByCarrierRFIDInfo(int portId, string lotId)
        {
            var substrates = _substrateManager.GetSubstratesAtLoadPort(portId);

            foreach (var item in substrates)
            {
                bool isChanged = false;
                string prevLotId = item.Value.LotId;
                if (string.IsNullOrEmpty(prevLotId)/* || false == item.Value.LotId.Equals(lotId)*/)
                {
                    //item.Value.SetLotId(lotId);
                    _substrateManager.SetLotIdByKey(item.Value.UniqueKey, lotId);
                    isChanged = true;
                }

                string prevParentLotId = item.Value.GetAttribute(PWA500SubstrateAttributes.ParentLotId);
                if (string.IsNullOrEmpty(prevParentLotId))
                {
                    _substrateManager.SetAttributeByKey(item.Value.UniqueKey, PWA500SubstrateAttributes.ParentLotId, lotId);
                    isChanged = true;
                }

                var ringId = item.Value.GetAttribute(PWA500SubstrateAttributes.RingId);
                if (string.IsNullOrEmpty(ringId))
                {
                    _substrateManager.SetAttributeByKey(item.Value.UniqueKey, PWA500SubstrateAttributes.RingId, item.Value.Name);
                    isChanged = true;
                }

                if (isChanged)
                {
                    _substrateManager.SaveDataByKey(item.Value.UniqueKey);
                }
            }
        }
        public bool GetSubstrateAtProcessModuleByName(string substrateName, out Substrate s)
        {
            s = null;
            if (string.IsNullOrWhiteSpace(substrateName))
                return false;

            var pm = _processGroup.GetProcessModuleName(ProcessModuleIndex);
            List<Substrate> substrates = new List<Substrate>();
            if (false == _substrateManager.GetSubstratesAtProcessModule(pm, ref substrates))
                return false;

            foreach (var item in substrates)
            {
                if (item == null)
                    continue;

                if (string.Equals(item.Name, substrateName, StringComparison.OrdinalIgnoreCase))
                {
                    s = item;
                    return true;
                }
            }

            return false;
        }
        // 공정설비에서 데이터를 주고받을 대 Key를 변경하지 않도록 협의했어야 했다.(Key는 원래 없었고, RingId가 그 역할이었으나, RingId는 공정 설비에서 변경될 수도 있다.)
        public bool FindSubstrateByNameOrRingIdAtProcessModule(string substrateName, string ringId, out Substrate substrate, out string description)
        {
            substrate = null;
            description = string.Empty;

            var pmName = _processGroup.GetProcessModuleName(ProcessModuleIndex);
            List<Substrate> substrates = new List<Substrate>();
            if (false == _substrateManager.GetSubstratesAtProcessModule(pmName, ref substrates))
            {
                description = "There is no substrates at process module";

                return false;
            }

            foreach (var item in substrates)
            {
                if (item == null)
                    continue;

                if (item.Name.Equals(substrateName) ||
                    item.GetAttribute(PWA500SubstrateAttributes.RingId).Equals(ringId) ||
                    item.Name.Equals(ringId) ||
                    item.GetAttribute(PWA500SubstrateAttributes.RingId).Equals(substrateName))
                {
                    substrate = item;
                    return true;
                }
            }

            return false;
        }
        public bool GetSubstrateByName(string targetName, out Substrate s)
        {
            s = null;

            List<Substrate> substrates = new List<Substrate>();
            if (false == _substrateManager.GetSubstratesAll(ref substrates))
                return false;

            foreach (var item in substrates)
            {
                if (string.Equals(targetName, item.Name, StringComparison.OrdinalIgnoreCase))
                {
                    s = item;
                    return true;
                }
            }

            return false;
        }
        public bool FindSubstrateByAttribute(string substrateName, string ringId, string portId, string slot, out Substrate substrate)
        {
            substrate = null;
            var pmName = _processGroup.GetProcessModuleName(ProcessModuleIndex);

            // 정보 핸들링이 Key 기반으로 변경했기 때문에 아래 구문은 제거함 -> 공정설비내 존재하는 자재를 순회해서 찾도록 수정
            // 1. 해당 Substrate의 정보가 공정 설비에 존재하는 경우(정상)
            //if (_substrateManager.GetSubstrateAtProcessModule(pmName, substrateName, out substrate))
            //    return true;

            // 2. 공정 설비에 있는 Substrate와 Source 정보들을 바탕으로 자재를 매칭(이름이 없고 포트번호, 슬롯번호가 존재하는 경우)
            #region <Find substrate by source info>
            List<Substrate> substrates = new List<Substrate>();
            if (false == _substrateManager.GetSubstratesAtProcessModule(pmName, ref substrates))
                return false;

            substrate = null;
            for (int i = 0; i < substrates.Count; ++i)
            {
                // 2025.06.22. jhlim [MOD] 위치 정보보다 이름/링이름을 우선하여 찾는다.
                if (substrates[i].Name.Equals(substrateName) ||
                    substrates[i].GetAttribute(PWA500SubstrateAttributes.RingId).Equals(substrateName) ||
                    substrates[i].Name.Equals(ringId) ||
                    substrates[i].GetAttribute(PWA500SubstrateAttributes.RingId).Equals(ringId))
                {
                    substrate = substrates[i];
                    break;
                }
                // 2025.06.22. jhlim [END]
            }

            if (substrate == null)
            {
                for (int i = 0; i < substrates.Count; ++i)
                {
                    if (substrates[i].SourcePortId.ToString().Equals(portId) && substrates[i].SourceSlot.ToString().Equals(slot))
                    {
                        substrate = substrates[i];
                        break;
                    }
                }
            }
            #endregion </Find substrate by source info>

            return substrate != null;
        }
        #endregion

        #endregion </Methods>
    }

    public static class ScenarioParameterBuilder
    {
        public static Dictionary<string, string> MakeParamToProcessing(int portId, Substrate substrate)
        {
            var recipe = EquipmentInfo.GetRecipeId();
            if (string.IsNullOrWhiteSpace(recipe))
            {
                recipe = substrate.RecipeId;
            }

            var scenarioParam = new Dictionary<string, string>();
            scenarioParam[EESKeys.KeyCarrierId] = CarrierManagementServer.Instance.GetCarrierId(portId);
            scenarioParam[EESKeys.KeyPortId] = EquipmentInfo.GetPortName(portId);
            scenarioParam[EESKeys.KeyLotId] = substrate.LotId;
            scenarioParam[EESKeys.KeyPartId] = substrate.GetAttribute(PWA500SubstrateAttributes.PartId);
            scenarioParam[EESKeys.KeyParamRecipeId] = recipe;
            scenarioParam[EESKeys.KeyOperatorId] = "AUTO";

            return scenarioParam;
        }
        public static Dictionary<string, string> MakeParamToEquipmentStatus(Func<int, SubstrateType> funcForGetSubstrateType)
        {
            if (funcForGetSubstrateType == null)
                return new Dictionary<string, string>();

            var scenarioParams = new Dictionary<string, string>();

            int currentPort = -1;
            List<int> portIdForCore = new List<int>();
            // Core 기준으로 전송한다.
            for (int i = 0; i < LoadPortManager.Instance.Count; ++i)
            {
                if (false == LoadPortManager.Instance.IsLoadPortEnabled(i))
                    continue;

                var substrateType = funcForGetSubstrateType(i);
                switch (substrateType)
                {
                    case SubstrateType.Core:
                        {
                            int portId = LoadPortManager.Instance.GetLoadPortPortId(i);
                            if (CarrierManagementServer.Instance.HasCarrier(portId))
                            {
                                portIdForCore.Add(portId);

                                var status = CarrierManagementServer.Instance.GetCarrierAccessingStatus(portId);
                                switch (status)
                                {
                                    case CarrierAccessStates.InAccessed:
                                        {
                                            currentPort = portId;
                                        }
                                        break;

                                    default:
                                        break;
                                }
                            }
                        }
                        break;

                    default:
                        break;
                }

                if (currentPort > 0)
                {
                    break;
                }
            }

            string lotId = string.Empty, partId = string.Empty, stepSeq = string.Empty;
            if (currentPort < 0)
            {
                if (portIdForCore.Count > 0)
                {
                    currentPort = portIdForCore[0];
                }
            }

            if (currentPort > 0)
            {
                lotId = CarrierManagementServer.Instance.GetCarrierLotId(currentPort);
                partId = CarrierManagementServer.Instance.GetAttribute(currentPort, PWA500CarrierAttributes.KeyPartId);
                stepSeq = CarrierManagementServer.Instance.GetAttribute(currentPort, PWA500CarrierAttributes.KeyStepSeq);
            }

            scenarioParams[ProcessModuleStatusChangedKeys.KeyParamLotId] = lotId;
            scenarioParams[ProcessModuleStatusChangedKeys.KeyParamPartId] = partId;
            scenarioParams[ProcessModuleStatusChangedKeys.KeyParamStepSeq] = stepSeq;

            return scenarioParams;
        }
        public static Dictionary<string, string> MakeParamToOHTHandling(int portId, LoadPortLoadingMode loadingType, string lotId, EN_SCENARIO scenario)
        {
            var scenarioParams = new Dictionary<string, string>();
            string carrierType = loadingType == LoadPortLoadingMode.Foup ?
                OHTHandlingCarrierType.MAC.ToString() :
                OHTHandlingCarrierType.CASSETTE.ToString();

            scenarioParams[AMHSHandlingKeys.KeyParamPortId] = EquipmentInfo.GetPortName(portId);
            // 2024.12.24. jhlim [MOD]
            scenarioParams[AMHSHandlingKeys.KeyParamLotId] = lotId;
            scenarioParams[AMHSHandlingKeys.KeyParamCarrierId] = CarrierManagementServer.Instance.GetCarrierId(portId);
            // 2024.12.24. jhlim [END]
            scenarioParams[AMHSHandlingKeys.KeyParamCarrierType] = carrierType;

            if (scenario.Equals(EN_SCENARIO.SCENARIO_PORT_STATUS_LOAD_1) ||
                scenario.Equals(EN_SCENARIO.SCENARIO_PORT_STATUS_LOAD_2) ||
                scenario.Equals(EN_SCENARIO.SCENARIO_PORT_STATUS_LOAD_3) ||
                scenario.Equals(EN_SCENARIO.SCENARIO_PORT_STATUS_LOAD_4) ||
                scenario.Equals(EN_SCENARIO.SCENARIO_PORT_STATUS_LOAD_5) ||
                scenario.Equals(EN_SCENARIO.SCENARIO_PORT_STATUS_LOAD_6))
            {
                scenarioParams[AMHSHandlingKeys.KeyParamStatus] = OHTHandlingStatus.UNLOAD.ToString();
            }
            else if (scenario.Equals(EN_SCENARIO.SCENARIO_PORT_STATUS_UNLOAD_1) ||
                scenario.Equals(EN_SCENARIO.SCENARIO_PORT_STATUS_UNLOAD_2) ||
                scenario.Equals(EN_SCENARIO.SCENARIO_PORT_STATUS_UNLOAD_3) ||
                scenario.Equals(EN_SCENARIO.SCENARIO_PORT_STATUS_UNLOAD_4) ||
                scenario.Equals(EN_SCENARIO.SCENARIO_PORT_STATUS_UNLOAD_5) ||
                scenario.Equals(EN_SCENARIO.SCENARIO_PORT_STATUS_UNLOAD_6))
            {
                scenarioParams[AMHSHandlingKeys.KeyParamStatus] = OHTHandlingStatus.LOAD.ToString();
            }

            scenarioParams[AMHSHandlingKeys.KeyParamOperId] = "AUTO";

            return scenarioParams;
        }
        public static Dictionary<string, string> MakeScenarioParamToRecipeDownload(Substrate substrate)
        {
            string lotId = substrate.LotId;
            string partId = substrate.GetAttribute(PWA500SubstrateAttributes.PartId);
            string stepSeq = substrate.GetAttribute(PWA500SubstrateAttributes.StepSeq);
            string recipeId = substrate.RecipeId;
            string lotType = substrate.GetAttribute(PWA500SubstrateAttributes.LotType);

            var scenarioParam = new Dictionary<string, string>
            {
                [RecipeHandlingKeys.KeyParamLotId] = lotId,
                [RecipeHandlingKeys.KeyParamRecipeId] = recipeId,
                [RecipeHandlingKeys.KeyParamPartId] = partId,
                [RecipeHandlingKeys.KeyParamStepSeq] = stepSeq,
                [RecipeHandlingKeys.KeyParamLotType] = lotType,
                [RecipeHandlingKeys.KeyUseCommunicationToPM] = bool.TrueString,
            };

            return scenarioParam;
        }
        public static Dictionary<string, string> MakeScenarioParamToSendingAssignId(string newSubstrateId, string ringId)
        {
            Dictionary<string, string> scenarioParam = new Dictionary<string, string>
            {
                [AssignSubstrateIdKeys.KeySubstrateName] = newSubstrateId,
                [AssignSubstrateIdKeys.KeyRingId] = ringId
            };

            return scenarioParam;
        }
        public static Dictionary<string, string> MakeScenarioParamToBinWorkEnd(string substrateKey, bool useEventHandling)
        {
            Dictionary<string, string> scenarioParams = new Dictionary<string, string>();
            string userId = "AUTO";
            if (SubstrateManager.Instance.GetSubstrateByKey(substrateKey, out var substrate))
            {
                int portId = substrate.DestinationPortId;
                int slot = substrate.DestinationSlot;
                if (portId <= 0 || slot < 0)
                    return null;

                string lotId = substrate.LotId;
                string partId = substrate.GetAttribute(PWA500SubstrateAttributes.PartId);
                string recipeId = EquipmentInfo.GetRecipeId();

                string chipQty = substrate.GetAttribute(PWA500SubstrateAttributes.ChipQty);
                string carrierId = CarrierManagementServer.Instance.GetCarrierId(portId);

                scenarioParams[UploadCoreOrBinFileKeys.KeyParamCarrierId] = carrierId;
                scenarioParams[UploadCoreOrBinFileKeys.KeyParamPortId] = EquipmentInfo.GetPortName(portId);
                scenarioParams[UploadCoreOrBinFileKeys.KeyParamLotId] = lotId;
                scenarioParams[UploadCoreOrBinFileKeys.KeyParamPartId] = partId;
                scenarioParams[UploadCoreOrBinFileKeys.KeyParamRecipeId] = recipeId;

                scenarioParams[UploadCoreOrBinFileKeys.KeyParamRecipeId] = recipeId;
                scenarioParams[UploadCoreOrBinFileKeys.KeyParamOperatorId] = userId;
                scenarioParams[UploadCoreOrBinFileKeys.KeyChipQty] = chipQty;
                scenarioParams[UploadCoreOrBinFileKeys.KeyUseEventHandling] = useEventHandling.ToString();

                return scenarioParams;
            }
            else
            {
                if (false == useEventHandling)
                {
                    scenarioParams[UploadCoreOrBinFileKeys.KeyParamCarrierId] = string.Empty;
                    scenarioParams[UploadCoreOrBinFileKeys.KeyParamPortId] = string.Empty;
                    scenarioParams[UploadCoreOrBinFileKeys.KeyParamLotId] = string.Empty;
                    scenarioParams[UploadCoreOrBinFileKeys.KeyParamPartId] = string.Empty;
                    scenarioParams[UploadCoreOrBinFileKeys.KeyParamRecipeId] = EquipmentInfo.GetRecipeId();
                    scenarioParams[UploadCoreOrBinFileKeys.KeyParamOperatorId] = userId;
                    scenarioParams[UploadCoreOrBinFileKeys.KeyChipQty] = string.Empty;
                    scenarioParams[UploadCoreOrBinFileKeys.KeyUseEventHandling] = useEventHandling.ToString();

                    return scenarioParams;
                }
            }

            return null;
        }
        public static Dictionary<string, string> MakeScenarioParamToCoreTrackIn(int portId, Substrate substrate)
        {
            if (false == CarrierManagementServer.Instance.HasCarrier(portId))
                return null;

            string carrierId = CarrierManagementServer.Instance.GetCarrierId(portId);
            string lotId = CarrierManagementServer.Instance.GetCarrierLotId(portId);
            string partId = substrate.GetAttribute(PWA500SubstrateAttributes.PartId);
            string stepSeq = substrate.GetAttribute(PWA500SubstrateAttributes.StepSeq);
            string recipeId = EquipmentInfo.GetRecipeId();
            string chipQty = CarrierManagementServer.Instance.GetAttribute(portId, PWA500CarrierAttributes.KeyLotQty);

            var scenarioParam = new Dictionary<string, string>
            {
                [TrackInOrOut.KeyParamCarrierId] = carrierId,
                [TrackInOrOut.KeyParamPortId] = EquipmentInfo.GetPortName(portId),
                [TrackInOrOut.KeyParamLotId] = lotId,
                [TrackInOrOut.KeyParamPartId] = partId,
                [TrackInOrOut.KeyParamStepSeq] = stepSeq,
                [TrackInOrOut.KeyParamRecipeId] = recipeId,
                [TrackInOrOut.KeyParamChipQty] = chipQty,
                [TrackInOrOut.KeyParamOperatorId] = "AUTO"
            };

            return scenarioParam;
        }
        public static Dictionary<string, string> MakeScenarioParamToLotMatch(int portId, string lotId, string carrierId)
        {
            if (false == CarrierManagementServer.Instance.HasCarrier(portId))
                return null;

            var scenarioParam = new Dictionary<string, string>
            {
                [TrackInOrOut.KeyParamLotId] = lotId,
                [TrackInOrOut.KeyParamCarrierId] = carrierId,

                // 2024.09.03. jhlim [MOD]
                // MATERIAL_TYPE : TM_TAPE
                // CHANGE_REASON : 전량 소진 후 교체-FINISH_CHAGNE, 품종교체 - PACKAGE_CHAGNE
                // 추후 품종 교체 기준이 생기면 구분이 필요할 수 있다. 현재는 HARDCODING
                [TrackInOrOut.KeyParamChangeReason] = Constants.EmptyWaferChangeReason,
                [TrackInOrOut.KeyParamMaterialType] = Constants.EmptyWaferMaterialType,
                [TrackInOrOut.KeyParamStepSeq] = EquipmentInfo.GetStepIdForBinWafer()
            };
            // 2024.09.03. jhlim [END]

            return scenarioParam;
        }
        public static Dictionary<string, string> MakeScenarioParamToTrackOut(string key, string userId, bool isCore)
        {
            if (false == SubstrateManager.Instance.GetSubstrateByKey(key, out var substrate) || substrate == null)
                return null;

            string lotId = substrate.LotId;
            string partId = substrate.GetAttribute(PWA500SubstrateAttributes.PartId);
            string stepSeq = substrate.GetAttribute(PWA500SubstrateAttributes.StepSeq);
            string recipeId = EquipmentInfo.GetRecipeId();
            string chipQty = substrate.GetAttribute(PWA500SubstrateAttributes.ChipQty);

            int portId;
            if (false == isCore)
            {
                portId = substrate.DestinationPortId;
            }
            else
            {
                portId = substrate.SourcePortId;
            }

            if (portId <= 0 || false == CarrierManagementServer.Instance.HasCarrier(portId))
                return null;

            string carrierId = CarrierManagementServer.Instance.GetCarrierId(portId);

            Dictionary<string, string> scenarioParams = new Dictionary<string, string>();
            scenarioParams[TrackInOrOut.KeyParamCarrierId] = carrierId;
            scenarioParams[TrackInOrOut.KeyParamPortId] = EquipmentInfo.GetPortName(portId);
            scenarioParams[TrackInOrOut.KeyParamLotId] = lotId;
            scenarioParams[TrackInOrOut.KeyParamPartId] = partId;
            scenarioParams[TrackInOrOut.KeyParamStepSeq] = stepSeq;
            scenarioParams[TrackInOrOut.KeyParamRecipeId] = recipeId;
            scenarioParams[TrackInOrOut.KeyParamChipQty] = chipQty;

            if (false == isCore)
            {
                scenarioParams[TrackInOrOut.KeyParamBinType] = substrate.GetAttribute(PWA500SubstrateAttributes.BinCode);
            }

            scenarioParams[TrackInOrOut.KeyParamOperatorId] = userId;

            return scenarioParams;
        }
        public static Dictionary<string, string> MakeScenarioParamToRequestBinPartId(string lotId, string carrierId)
        {
            Dictionary<string, string> scenarioParams = new Dictionary<string, string>
            {
                [LotInfoKeys.KeyParamLotId] = lotId,
                [LotInfoKeys.KeyParamCarrierId] = carrierId
            };

            return scenarioParams;
        }
        public static Dictionary<string, string> MakeScenarioParamToAssignSubstrateId(int portId, int slot, SubstrateType substrateType, Substrate substrate)
        {
            if (false == CarrierManagementServer.Instance.HasCarrier(portId))
                return null;

            string lotId = substrate.LotId;
            string substrateId = substrate.Name;
            string chipQty = substrate.GetAttribute(PWA500SubstrateAttributes.ChipQty);
            string binCode = substrate.GetAttribute(PWA500SubstrateAttributes.BinCode);

            var scenarioParam = new Dictionary<string, string>
            {
                [AssignSubstrateIdKeys.KeyParamLotId] = lotId,
                [AssignSubstrateIdKeys.KeyParamBinType] = binCode,
                [AssignSubstrateIdKeys.KeyParamRingFrameId] = substrateId,
                [AssignSubstrateIdKeys.KeyParamSlotId] = (slot).ToString(),
                [AssignSubstrateIdKeys.KeyChipQty] = chipQty
            };

            return scenarioParam;
        }
        public static Dictionary<string, string> MakeScenarioParamToScrapBinInfo(string substrateName, string ringId)
        {
            Dictionary<string, string> scenarioParam = new Dictionary<string, string>
            {
                [ScrapInfoKeys.KeySubstrateName] = substrateName,
                [ScrapInfoKeys.KeyRingId] = ringId
            };

            return scenarioParam; ;
        }
        public static Dictionary<string, string> MakeScenarioParamToUploadBinFile(int portId, int slot, string equipId, Substrate substrate)
        {
            if (false == CarrierManagementServer.Instance.HasCarrier(portId))
                return null;

            string substrateName = substrate.Name;
            string ringId = substrate.GetAttribute(PWA500SubstrateAttributes.RingId);
            string recipeId = substrate.RecipeId;
            string substrateType = substrate.GetAttribute(PWA500SubstrateAttributes.SubstrateType);
            string stepId = substrate.GetAttribute(PWA500SubstrateAttributes.StepSeq);

            // 2024.10.29. jhlim [MOD] StepSeq가 설정값과 다르면 값을 셋한다.
            string stepSeqFromParam = EquipmentInfo.GetStepIdForBinWafer();
            if (stepId.Equals(stepSeqFromParam))
            {
                SubstrateManager.Instance.SetAttributeByKey(substrate.UniqueKey, PWA500SubstrateAttributes.StepSeq, stepSeqFromParam);
            }

            stepId = stepSeqFromParam;
            // 2024.10.29. jhlim [END]

            string partId = substrate.GetAttribute(PWA500SubstrateAttributes.PartId);
            string lotId = substrate.LotId;

            var scenarioParam = new Dictionary<string, string>
            {
                [UploadCoreOrBinFileKeys.KeySubstrateName] = substrateName,
                [UploadCoreOrBinFileKeys.KeyRingId] = ringId,
                [UploadCoreOrBinFileKeys.KeyRecipeId] = recipeId,
                [UploadCoreOrBinFileKeys.KeySubstrateType] = substrateType,
                [UploadCoreOrBinFileKeys.KeyStepId] = stepId,
                [UploadCoreOrBinFileKeys.KeyEquipId] = equipId,
                [UploadCoreOrBinFileKeys.KeyPartId] = partId,
                [UploadCoreOrBinFileKeys.KeySlot] = (slot).ToString(),
                [UploadCoreOrBinFileKeys.KeyLotId] = lotId
            };

            return scenarioParam;
        }
    }

    public static class EquipmentInfo
    {
        private const int ProcessModuleIndex = 0;

        public static string GetRecipeId()
        {
            return ProcessModuleGroup.Instance.GetRecipeId(ProcessModuleIndex);
        }
        public static string GetPortName(int portId)
        {
            return string.Format("B{0}", portId);

            //return string.Format("{0}_B{1}", Work.AppConfigManager.Instance.MachineName, portId);
        }
        public static string GetStepIdForBinWafer()
        {
            return Recipe.GetInstance().GetValue(EN_RECIPE_TYPE.EQUIPMENT, PARAM_EQUIPMENT.BinWaferStepId.ToString(), "P420");
        }
    }
}
