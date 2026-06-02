using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;

using EquipmentState_;

using FrameOfSystem3.Recipe;
using FrameOfSystem3.Component;
using FrameOfSystem3.Functional;
using FrameOfSystem3.SECSGEM;
using FrameOfSystem3.SECSGEM.DefineSecsGem;
using Define.DefineEnumProject;
using FrameOfSystem3.Views.Functional;
using FrameOfSystem3.SECSGEM.Scenario;

using EFEM.Modules;
using EFEM.MaterialTracking;
using EFEM.CustomizedByProcessType.PWA500W;
using EFEM.CustomizedByProcessType.PWA500Common;

namespace EFEM.CustomizedByProcessType.UserInterface.OperationSecsGem.PWA500W
{
    public partial class SubViewOperationSecsGemCustomized : UserControlForMainView.CustomView
    {
        #region <Constructors>
        public SubViewOperationSecsGemCustomized()
        {
            InitializeComponent();

            _selectionList = Form_SelectionList.GetInstance();
            _messageBox = Form_MessageBox.GetInstance();
            _postOffice = PostOffice.GetInstance();
            _keyboard = Form_Keyboard.GetInstance();
            _recipe = FrameOfSystem3.Recipe.Recipe.GetInstance();
            _equipmentState = EquipmentState.GetInstance();
            _scenarioOperator = ScenarioOperator.Instance;
            _loadPortManager = LoadPortManager.Instance;
            _substrateManager = SubstrateManager.Instance;
            _carrierServer = CarrierManagementServer.Instance;
            _functionsForPWA500 = FunctionsForPWA500W_NRD.Instance;

            DataToSend = new Dictionary<string, string>();
            CarrierScenario = new Dictionary<string, EN_SCENARIO>();
            SubstrateScenario = new Dictionary<string, EN_SCENARIO>();
            ETCScenario = new Dictionary<string, EN_SCENARIO>();
            RecipeHandlingScenario = new Dictionary<string, EN_SCENARIO>();

            CoreCarriers = new Dictionary<int, string>();
            BinCarriers = new Dictionary<int, string>();
            EmptyCarriers = new Dictionary<int, string>();

            CoreSubstrates = new Dictionary<int, Substrate>();
            BinSubstrates = new Dictionary<int, Substrate>();

            _substrates = new List<Substrate>();
            
            ClassifyScenarios();
            
            _selectedScenario = EN_SCENARIO.SCENARIO_REQ_LOT_INFO_CORE_1.ToString();
            _selectedPortId = 1;
        }
        #endregion </Constructors>

        #region <Fields>
        private static Form_MessageBox _messageBox = null;
        private static Form_SelectionList _selectionList = null;
        private static Form_Keyboard _keyboard = null;
        private static PostOffice _postOffice = null;
        private static FrameOfSystem3.Recipe.Recipe _recipe = null;
        private static ScenarioOperator _scenarioOperator = null;
        private static LoadPortManager _loadPortManager = null;
        private static EquipmentState _equipmentState = null;
        private static SubstrateManager _substrateManager = null;
        private static CarrierManagementServer _carrierServer = null;
        private static FunctionsForPWA500W_NRD _functionsForPWA500 = null;
        private string _selectedScenario;
        private int _selectedPortId;
        private readonly Dictionary<string, string> DataToSend = null;
        private readonly Dictionary<string, EN_SCENARIO> CarrierScenario = null;
        private readonly Dictionary<string, EN_SCENARIO> SubstrateScenario = null;
        private readonly Dictionary<string, EN_SCENARIO> ETCScenario = null;
        private readonly Dictionary<string, EN_SCENARIO> RecipeHandlingScenario = null;

        private readonly Dictionary<int, Substrate> CoreSubstrates = null;
        private readonly Dictionary<int, Substrate> BinSubstrates = null;
        private readonly Dictionary<int, string> CoreCarriers = null;
        private readonly Dictionary<int, string> BinCarriers = null;
        private readonly Dictionary<int, string> EmptyCarriers = null;
        private List<Substrate> _substrates = null;

        private Substrate _selectedCoreSubstrate = null;
        private Substrate _selectedBinOrEmptySubstrate = null;

        private bool _isScenarioExecuting = false;
        #endregion </Fields>

        #region <Properties>
        private bool EnableUIControl
        {
            get
            {
                if (false == _equipmentState.GetState().Equals(EQUIPMENT_STATE.IDLE) &&
                    false == _equipmentState.GetState().Equals(EQUIPMENT_STATE.PAUSE))
                    return false;

                if (false == _scenarioOperator.UseScenario)
                    return false;

                return true;
            }
        }
        #endregion </Properties>

        #region <Methods>

        #region <Overrides>
        protected override void ProcessWhenActivation()
        {
            ClassifyScenarios();
            UpdateCarrierInfo();
            UpdateSubstrateInfo();
            UpdateDataToSend();
        }
        protected override void ProcessWhenDeactivation()
        {
        }
        public override void CallFunctionByTimer()
        {
            Enabled = EnableUIControl && (false == _isScenarioExecuting);
        }
        #endregion </Overrides>

        #region <UI Events>
        private async void BtnExecuteScenarioClicked(object sender, EventArgs e)
        {
            if (false == EnableUIControl)
                return;

            //if (!(sender is Component.CustomActionButton button))
            //    return;

            // DataToSend.Clear();
            //for (int row = 0; row < gvMessageToSend.Rows.Count; ++row)
            //{
            //    string key = gvMessageToSend[0, row].Value.ToString();
            //    string value = string.Empty;
            //    if (gvMessageToSend[1, row].Value != null)
            //        value = gvMessageToSend[1, row].Value.ToString();
            //    DataToSend[key] = value;
            //}
            bool includeUpdateParam = true;
            EN_SCENARIO scenarioTypeToExecute;

            if (sender.Equals(btnDownloadRecipe) || sender.Equals(btnUploadRecipe))
            {
                includeUpdateParam = false;
                if (sender.Equals(btnDownloadRecipe))
                {
                    scenarioTypeToExecute = EN_SCENARIO.SCENARIO_REQ_RECIPE_DOWNLOAD;
                }
                else
                {
                    scenarioTypeToExecute = EN_SCENARIO.SCENARIO_REQ_RECIPE_UPLOAD;
                }
                var paramList = _scenarioOperator.GetScenarioParameterList(scenarioTypeToExecute);
                if (paramList == null)
                    return;

                Dictionary<string, string> paramsToUpdate = new Dictionary<string, string>();
                for (int i = 0; i < paramList.Count; ++i)
                {
                    string paramName = paramList[i];
                    string paramValue = string.Empty;
                    if (paramName.Equals(RecipeHandlingKeys.KeyParamRecipeId))
                    {
                        paramValue = lblSelectedRecipeName.Text;
                    }
                    else if (paramName.Equals(RecipeHandlingKeys.KeyUseCommunicationToPM))
                    {
                        paramValue = bool.TrueString;
                    }

                    paramsToUpdate[paramName] = paramValue;
                }
                _scenarioOperator.UpdateScenarioParam(ScenarioSenders.Manual.ToString(), scenarioTypeToExecute, paramsToUpdate);
            }
            else
            {
                if (false == Enum.TryParse(_selectedScenario, out scenarioTypeToExecute))
                    return;
            }

            _isScenarioExecuting = true;
            var waitResponse = System.Threading.Tasks.Task.Run(() => ExecuteScenarioAsync(scenarioTypeToExecute, includeUpdateParam));
            var result = await waitResponse;
            _isScenarioExecuting = false;

            string message = string.Format("Scenario : {0}\r\nResult : {1}", scenarioTypeToExecute, result.ToString());
            _messageBox.ShowMessage(message);
        }


        //private void BtnExecuteScenarioClicked(object sender, EventArgs e)
        //{
        //    if (false == _messageBox.ShowMessage(string.Format("{0} scenario run?", _selectedScenario)))
        //        return;

        //    DataToSend.Clear();
        //    for (int row = 0; row < gvMessageToSend.Rows.Count; ++row)
        //    {
        //        string key = gvMessageToSend[0, row].Value.ToString();
        //        string value = string.Empty;
        //        if (gvMessageToSend[1, row].Value != null)
        //            value = gvMessageToSend[1, row].Value.ToString();
        //        DataToSend[key] = value;
        //    }

        //    if (false == Enum.TryParse(_selectedScenario, out ScenarioListTypes convertedScenarioName))
        //        return;

        //    _postOffice.SendMail(Define.DefineEnumProject.Mail.EN_SUBSCRIBER.ScenarioCirculator
        //        , Define.DefineEnumProject.Mail.EN_MAIL.SendScenario
        //        , convertedScenarioName
        //        , DataToSend);
        //}
        private void BtnScenarioSelectionClicked(object sender, EventArgs e)
        {
            if (sender.Equals(btnScenarioSelectionForCarrier))
            {
                if (false == _selectionList.CreateForm("CARRIER SCENARIO", CarrierScenario.Keys.ToArray(), _selectedScenario))
                    return;
            }
            else if (sender.Equals(btnScenarioSelectionForSubstrate))
            {
                if (false == _selectionList.CreateForm("SUBSTRATE SCENARIO", SubstrateScenario.Keys.ToArray(), _selectedScenario))
                    return;
            }
            else
            {
                if (false == _selectionList.CreateForm("ETC SCENARIO", ETCScenario.Keys.ToArray(), _selectedScenario))
                    return;
            }

            _selectionList.GetResult(ref _selectedScenario);

            UpdateDataToSend();
            
            lblSelectedScenarioName.Text = _selectedScenario.ToString();
        }
        private void LblSubstrateInfoClicked(object sender, EventArgs e)
        {
            if (!(sender is Sys3Controls.Sys3Label label))
                return;

            List<string> substrateNames = new List<string>();
            if (label.Equals(lblCoreSubstrateInfo))
            {
                foreach (var item in CoreSubstrates)
                {
                    substrateNames.Add(item.Value.Name);
                }
            }
            else if (label.Equals(lblBinOrEmptySubstrateInfo))
            {
                foreach (var item in BinSubstrates)
                {
                    substrateNames.Add(item.Value.Name);
                }
            }

            if (false == _selectionList.CreateForm("Select Substrate", substrateNames.ToArray(), string.Empty))
                return;

            string substrateName = string.Empty;
            _selectionList.GetResult(ref substrateName);

            if (false == _functionsForPWA500.GetSubstrateByName(substrateName, out var substrate))
                return;

            if (label.Equals(lblCoreSubstrateInfo))
            {
                _selectedCoreSubstrate = substrate;
            }
            else if (label.Equals(lblBinOrEmptySubstrateInfo))
            {
                _selectedBinOrEmptySubstrate = substrate;
            }

            label.Text = substrateName;
        }
        private void LblCarrierInfoClicked(object sender, EventArgs e)
        {
            if (sender.Equals(lblCarrierInfo))
            {
                Dictionary<int, string> carriers = new Dictionary<int, string>();
                for (int i = 0; i < _loadPortManager.Count; ++i)
                {
                    if (false == _loadPortManager.IsLoadPortEnabled(i))
                        continue;

                    int portId = _loadPortManager.GetLoadPortPortId(i);
                    if (false == _carrierServer.HasCarrier(portId))
                        continue;

                    carriers[portId] = _carrierServer.GetCarrierId(portId);
                }

                if (false == _selectionList.CreateForm("Select Carrier", carriers.Values.ToArray(), carriers.Keys.ToArray(), _selectedPortId))
                    return;

                _selectionList.GetResult(ref _selectedPortId);

                DisplaySelectedCarrierInfo();
            }
        }
        private void BtnApplySubstrateInfoToScenarioClicked(object sender, EventArgs e)
        {
            if (false == Enum.TryParse(_selectedScenario, out EN_SCENARIO scenario))
                return;

            if (false == SubstrateScenario.ContainsKey(_selectedScenario))
                return;
            
            if (sender.Equals(btnApplyBinOrEmptySubstrateInfoToScenario))
            {
                UpdateSubstrateScenarioData(scenario, _selectedBinOrEmptySubstrate, null);
            }
            else if (sender.Equals(btnApplyCoreSubstrateInfoToScenario))
            {
                UpdateSubstrateScenarioData(scenario, _selectedCoreSubstrate, _selectedBinOrEmptySubstrate);
            }
        }

        private void BtnEditSubstrateInfoClicked(object sender, EventArgs e)
        {
            if (sender.Equals(btnEditCoreSubstrateInfo))
            {
                if (_selectedCoreSubstrate == null)
                    return;

                Dictionary<string, string> targetAttributes = MaterialTracking.SubstrateMapper.ExtractDataAll(_selectedCoreSubstrate);
                FormMaterialEdit materialEdit = new FormMaterialEdit();
                if (materialEdit.CreateEditForm(targetAttributes))
                {
                    if (false == _messageBox.ShowMessage("정말로 자재정보를 변경할까요?"))
                        return;

                    Dictionary<string, string> attributeResults = new Dictionary<string, string>();
                    materialEdit.GetResult(ref attributeResults);

                    var data = MaterialTracking.SubstrateMapper.GetSubstrateDataFromAttributes(attributeResults, out var extra);
                    var key = data.UniqueKey;
                    _substrateManager.SetNameByKey(key, data.Name);
                    //_substrateManager.SetLocationIdByKey(key, data.LocationId);
                    _substrateManager.SetSourcePortIdByKey(key, data.SourcePortId);
                    _substrateManager.SetSourceSlotByKey(key, data.SourceSlot);
                    _substrateManager.SetSourceCarrierIdByKey(key, data.SourceCarrierId);
                    _substrateManager.SetCurrentCarrierKeyByKey(key, data.CurrentCarrierKey);
                    _substrateManager.SetDestinationPortIdByKey(key, data.DestinationPortId);
                    _substrateManager.SetDestinationSlotByKey(key, data.DestinationSlot);
                    _substrateManager.SetLotIdByKey(key, data.LotId);
                    _substrateManager.SetRecipeIdByKey(key, data.RecipeId);
                    _substrateManager.SetProcessJobIdByKey(key, data.ProcessJobId);
                    _substrateManager.SetControlJobIdByKey(key, data.ControlJobId);
                    _substrateManager.SetTransferStatusByKey(key, (Defines.MaterialTracking.TransportStates)data.TransportStatus);
                    _substrateManager.SetProcessingStatusByKey(key,(Defines.MaterialTracking.ProcessingStates)data.ProcessingStatus);
                    _substrateManager.SetIdReadingStateByKey(key, (Defines.MaterialTracking.IdReadingStates)data.IdReadingStatus);
                    _substrateManager.SetDoNotProcessFlagByKey(key, data.DoNotProcessFlag);
                    _substrateManager.SetUsageByKey(key, data.Usage);

                    if (extra != null)
                    {
                        foreach (var item in extra)
                        {
                            _substrateManager.SetAttributeByKey(key, item.Key, item.Value);
                        }
                    }

                    _substrateManager.SaveDataByKey(key);

                    if (false == _selectedCoreSubstrate.Name.Equals(lblCoreSubstrateInfo.Text))
                    {
                        lblCoreSubstrateInfo.Text = _selectedCoreSubstrate.Name;
                    }
                }

                materialEdit.DisposeControls();
                materialEdit = null;
            }
            else if (sender.Equals(btnEditBinOrEmptySubstrateInfo))
            {
                if (_selectedBinOrEmptySubstrate == null)
                    return;

                Dictionary<string, string> targetAttributes = MaterialTracking.SubstrateMapper.ExtractDataAll(_selectedBinOrEmptySubstrate);
                FormMaterialEdit materialEdit = new FormMaterialEdit();
                if (materialEdit.CreateEditForm(targetAttributes))
                {
                    if (false == _messageBox.ShowMessage("정말로 자재정보를 변경할까요?"))
                        return;

                    Dictionary<string, string> attributeResults = new Dictionary<string, string>();
                    materialEdit.GetResult(ref attributeResults);

                    var data = MaterialTracking.SubstrateMapper.GetSubstrateDataFromAttributes(attributeResults, out var extra);
                    var key = data.UniqueKey;
                    _substrateManager.SetNameByKey(key, data.Name);
                    //_substrateManager.SetLocationIdByKey(key, data.LocationId);
                    _substrateManager.SetSourcePortIdByKey(key, data.SourcePortId);
                    _substrateManager.SetSourceSlotByKey(key, data.SourceSlot);
                    _substrateManager.SetSourceCarrierIdByKey(key, data.SourceCarrierId);
                    _substrateManager.SetCurrentCarrierKeyByKey(key, data.CurrentCarrierKey);
                    _substrateManager.SetDestinationPortIdByKey(key, data.DestinationPortId);
                    _substrateManager.SetDestinationSlotByKey(key, data.DestinationSlot);
                    _substrateManager.SetLotIdByKey(key, data.LotId);
                    _substrateManager.SetRecipeIdByKey(key, data.RecipeId);
                    _substrateManager.SetProcessJobIdByKey(key, data.ProcessJobId);
                    _substrateManager.SetControlJobIdByKey(key, data.ControlJobId);
                    _substrateManager.SetTransferStatusByKey(key, (Defines.MaterialTracking.TransportStates)data.TransportStatus);
                    _substrateManager.SetProcessingStatusByKey(key, (Defines.MaterialTracking.ProcessingStates)data.ProcessingStatus);
                    _substrateManager.SetIdReadingStateByKey(key, (Defines.MaterialTracking.IdReadingStates)data.IdReadingStatus);
                    _substrateManager.SetDoNotProcessFlagByKey(key, data.DoNotProcessFlag);
                    _substrateManager.SetUsageByKey(key, data.Usage);

                    if (extra != null)
                    {
                        foreach (var item in extra)
                        {
                            _substrateManager.SetAttributeByKey(key, item.Key, item.Value);
                        }
                    }

                    _substrateManager.SaveDataByKey(key);

                    //_selectedBinOrEmptySubstrate.SetAttributesAll(attributeResults);
                    if (false == _selectedBinOrEmptySubstrate.Name.Equals(lblBinOrEmptySubstrateInfo.Text))
                    {
                        lblBinOrEmptySubstrateInfo.Text = _selectedBinOrEmptySubstrate.Name;
                    }
                }

                materialEdit.DisposeControls();
                materialEdit = null;
            }
        }
        private void BtnApplyCarrierInfoClicked(object sender, EventArgs e)
        {
            if (sender.Equals(btnApplyCarrierInfo))
            {
                if (false == Enum.TryParse(_selectedScenario, out EN_SCENARIO scenario))
                    return;

                if (false == CarrierScenario.ContainsKey(_selectedScenario))
                    return;

                if (false == _carrierServer.HasCarrier(_selectedPortId))
                    return;

                UpdateCarrierDataToSend(scenario, _selectedPortId);
            }
        }

        private void LblSelectedRecipeNameClicked(object sender, EventArgs e)
        {
            if (false == EnableUIControl)
                return;

            if (_keyboard.CreateForm(lblSelectedRecipeName.Text, 200, false, "Recipe name to handling"))
            {
                string result = string.Empty;
                _keyboard.GetResult(ref result);                
                lblSelectedRecipeName.Text = result;
            }
        }
        #endregion </UI Events>

        #region <Internal>
        private EN_SCENARIO_RESULT ExecuteScenarioAsync(EN_SCENARIO scenario, bool includeUpdateScenario)
        {
            EN_SCENARIO_RESULT result;
            TickCounter_.TickCounter tick = new TickCounter_.TickCounter();

            _scenarioOperator.InitScenarioAll();
            if (includeUpdateScenario)
            {
                DataToSend.Clear();
                for (int row = 0; row < gvMessageToSend.Rows.Count; ++row)
                {
                    string key = gvMessageToSend[0, row].Value.ToString();
                    string value = string.Empty;
                    if (gvMessageToSend[1, row].Value != null)
                        value = gvMessageToSend[1, row].Value.ToString();
                    DataToSend[key] = value;
                }

                _scenarioOperator.UpdateScenarioParam(ScenarioSenders.Manual.ToString(), scenario, DataToSend);
            }
            tick.SetTickCount(30000);
            while (true)
            {
                System.Threading.Thread.Sleep(1);

                if (tick.IsTickOver(true))
                {
                    return EN_SCENARIO_RESULT.TIMEOUT_ERROR;                    
                }

                result = _scenarioOperator.ExecuteScenario(ScenarioSenders.Manual.ToString(), scenario);
                switch (result)
                {
                    case EN_SCENARIO_RESULT.COMPLETED:
                        {
                            var scenarioResult = _scenarioOperator.GetScenarioResultData(ScenarioSenders.Manual.ToString(), scenario);
                            _functionsForPWA500.ExecuteAfterScenarioCompletion(scenario,
                                DataToSend, 
                                scenarioResult,
                                null,
                                EN_MESSAGE_RESULT.OK,
                                true);
                        }
                        break;
                    default:
                        break;
                }

                if (result == EN_SCENARIO_RESULT.PROCEED ||
                    result == EN_SCENARIO_RESULT.WAITING)
                    continue;

                return result;
            }
        }

        private void ClassifyScenarioTypesByEnum(EN_SCENARIO scenario)
        {
            if (false == _scenarioOperator.IsScenarioRegistered(scenario))
                return;

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
                case EN_SCENARIO.SCENARIO_SLOT_WAFER_MAPPING_CORE_1:
                case EN_SCENARIO.SCENARIO_SLOT_WAFER_MAPPING_CORE_2:
                case EN_SCENARIO.SCENARIO_SLOT_WAFER_MAPPING_CORE_3:
                case EN_SCENARIO.SCENARIO_SLOT_WAFER_MAPPING_EMPTY_TAPE:
                case EN_SCENARIO.SCENARIO_SLOT_WAFER_MAPPING_BIN_1:
                case EN_SCENARIO.SCENARIO_SLOT_WAFER_MAPPING_BIN_2:
                case EN_SCENARIO.SCENARIO_SLOT_WAFER_MAPPING_BIN_3:
                case EN_SCENARIO.SCENARIO_REQ_LOT_MERGE_CORE_1:
                case EN_SCENARIO.SCENARIO_REQ_LOT_MERGE_CORE_2:
                case EN_SCENARIO.SCENARIO_REQ_LOT_MERGE_CORE_3:
                case EN_SCENARIO.SCENARIO_REQ_LOT_ID_MERGE_AND_CHANGE_BIN_1:
                case EN_SCENARIO.SCENARIO_REQ_LOT_ID_MERGE_AND_CHANGE_BIN_2:
                case EN_SCENARIO.SCENARIO_REQ_LOT_ID_MERGE_AND_CHANGE_BIN_3:
                    CarrierScenario[scenario.ToString()] = scenario;
                    break;
                case EN_SCENARIO.SCENARIO_REQ_RECIPE_DOWNLOAD:
                case EN_SCENARIO.SCENARIO_REQ_RECIPE_UPLOAD:
                    ETCScenario[scenario.ToString()] = scenario;
                    //RecipeHandlingScenario[scenario.ToString()] = scenario;
                    break;

                case EN_SCENARIO.SCENARIO_PROCESS_START:
                case EN_SCENARIO.SCENARIO_PROCESS_END:
                case EN_SCENARIO.SCENARIO_REQ_TRACK_IN:
                case EN_SCENARIO.SCENARIO_REQ_CORE_WAFER_TRACK_OUT:
                case EN_SCENARIO.SCENARIO_REQ_LOT_MATCH:
                case EN_SCENARIO.SCENARIO_REQ_BIN_WAFER_TRACK_OUT:
                case EN_SCENARIO.SCENARIO_WORK_START:
                case EN_SCENARIO.SCENARIO_WORK_END:
                case EN_SCENARIO.SCENARIO_REQ_CORE_WAFER_SPLIT:
                case EN_SCENARIO.SCENARIO_REQ_CORE_WAFER_SPLIT_LAST:
                case EN_SCENARIO.SCENARIO_CORE_WAFER_DETACH_START:
                case EN_SCENARIO.SCENARIO_CORE_WAFER_DETACH_END:
                case EN_SCENARIO.SCENARIO_REQ_CORE_CHIP_SPLIT_FIRST:
                case EN_SCENARIO.SCENARIO_REQ_CORE_CHIP_SPLIT:
                case EN_SCENARIO.SCENARIO_REQ_CORE_CHIP_FULL_SPLIT_FIRST:
                case EN_SCENARIO.SCENARIO_REQ_CORE_CHIP_FULL_SPLIT:
                case EN_SCENARIO.SCENARIO_REQ_CORE_CHIP_MERGE:
                case EN_SCENARIO.SCENARIO_BIN_WAFER_ID_READ:
                case EN_SCENARIO.SCENARIO_BIN_WORK_END:
                case EN_SCENARIO.SCENARIO_REQ_BIN_WAFER_ID_ASSIGN:
                case EN_SCENARIO.SCENARIO_BIN_SORTING_START_1:
                case EN_SCENARIO.SCENARIO_BIN_SORTING_END_1:
                case EN_SCENARIO.SCENARIO_BIN_SORTING_START_2:
                case EN_SCENARIO.SCENARIO_BIN_SORTING_END_2:
                case EN_SCENARIO.SCENARIO_BIN_SORTING_START_3:
                case EN_SCENARIO.SCENARIO_BIN_SORTING_END_3:
                case EN_SCENARIO.SCENARIO_REQ_UPLOAD_BINFILE:
                case EN_SCENARIO.SCENARIO_ASSIGN_SUBSTRATE_ID:
                case EN_SCENARIO.SCENARIO_BIN_DATA_UPLOAD:
                case EN_SCENARIO.SCENARIO_BIN_PART_ID_INFO_REQ:
                //case EN_SCENARIO.SCENARIO_CORE_MAP_DOWNLOAD:  // 2026.05.15 dwlim [ADD] GEM300 시나리오 추가    // 이건 이벤트가 아닌 S14F1이라 지움
                case EN_SCENARIO.SCENARIO_CORE_MAP_UPLOAD:      // 2026.05.15 dwlim [ADD] GEM300 시나리오 추가
                case EN_SCENARIO.SCENARIO_BIN_MAP_UPLOAD:       // 2026.05.15 dwlim [ADD] GEM300 시나리오 추가				
                    SubstrateScenario[scenario.ToString()] = scenario;
                    break;

                default:
                    ETCScenario[scenario.ToString()] = scenario;
                    break;
            }
        }
        private void UpdateGridViewByAppliedData()
        {
            for (int i = 0; i < gvMessageToSend.Rows.Count; ++i)
            {
                gvMessageToSend[1, i].Value = string.Empty;

                string nameOfKey = gvMessageToSend[0, i].Value.ToString();
                if (false == DataToSend.TryGetValue(nameOfKey, out string value))
                    continue;

                gvMessageToSend[1, i].Value = value;
            }
        }
        private void ClassifyScenarios()
        {
            CarrierScenario.Clear();
            SubstrateScenario.Clear();
            ETCScenario.Clear();
            RecipeHandlingScenario.Clear();

            var types = Enum.GetValues(typeof(EN_SCENARIO));
            foreach (var item in types)
            {
                ClassifyScenarioTypesByEnum((EN_SCENARIO)item);
            }

        }
        private void UpdateCarrierInfo()
        {
            //for (int i = 0; i < _loadPortManager.Count; ++i)
            //{
            //    if (false == _loadPortManager.IsLoadPortEnabled(i))
            //        continue;

            //    var substrateType = _functionsForPWA500.GetSubstrateTypeByLoadPortIndex(i);
            //    string lpName = _functionsForPWA500.GetSubstrateTypeForUILoadPortIndex(i);
                
            //    switch (substrateType)
            //    {
            //        case SubstrateType.Core:
            //            CoreCarriers[i] = lpName;
            //            break;
                    
            //        default:
            //            EmptyCarriers[i] = lpName;
            //            break;
            //    }
            //}            
        }
        private void UpdateSubstrateInfo()
        {
            BinSubstrates.Clear();
            CoreSubstrates.Clear();

            _substrates.Clear();
            if (false == _substrateManager.GetSubstratesAll(ref _substrates))
                return;

            for(int i = 0; i < _substrates.Count; ++i)
            {
                string substrateTypeString = _substrates[i].GetAttribute(PWA500SubstrateAttributes.SubstrateType);
                if (false == Enum.TryParse(substrateTypeString, out SubstrateType substrateType))
                    continue;

                switch (substrateType)
                {
                    case SubstrateType.Core:
                        CoreSubstrates[CoreSubstrates.Count] = _substrates[i];
                        break;
                    
                    default:
                        BinSubstrates[BinSubstrates.Count] = _substrates[i];
                        break;
                }
            }

            DisplaySelectedCarrierInfo();
        }
        private void DisplaySelectedCarrierInfo()
        {
            if (_selectedPortId <= 0 || false == _carrierServer.HasCarrier(_selectedPortId))
            {
                lblCarrierInfo.Text = string.Empty;
            }
            else
            {
                lblCarrierInfo.Text = _carrierServer.GetCarrierId(_selectedPortId);
            }
        }
        private void UpdateDataToSend()
        {
            if (_selectedScenario.Equals(lblSelectedScenarioName.Text))
                return;

            lblSelectedScenarioName.Text = _selectedScenario;
            if (false == Enum.TryParse(_selectedScenario, out EN_SCENARIO convertedScenarioName))
                return;

            var dataToSend = _scenarioOperator.GetScenarioParameterList(convertedScenarioName);
            DataToSend.Clear();
            gvMessageToSend.Rows.Clear();

            if (dataToSend == null)
                return;

            for (int i = 0; i < dataToSend.Count; ++i)
            {
                string dataKey = dataToSend[i];
                DataToSend[dataKey] = string.Empty;

                gvMessageToSend.Rows.Add();
                gvMessageToSend[0, i].Value = dataKey;
                gvMessageToSend[1, i].Value = string.Empty;
            }
        }
        private void UpdateCarrierDataToSend(EN_SCENARIO scenario, int portId)
        {
            string lotId = _carrierServer.GetCarrierLotId(_selectedPortId);
            string carrierId = _carrierServer.GetCarrierId(_selectedPortId);
            string portName = _functionsForPWA500.GetPortName(_selectedPortId);
            string operatorId = "AUTO";

            switch (scenario)
            {
                case EN_SCENARIO.SCENARIO_REQ_LOT_INFO_CORE_1:
                case EN_SCENARIO.SCENARIO_REQ_LOT_INFO_CORE_2:
                case EN_SCENARIO.SCENARIO_REQ_LOT_INFO_CORE_3:
                case EN_SCENARIO.SCENARIO_REQ_LOT_INFO_EMPTY_TAPE:
                case EN_SCENARIO.SCENARIO_REQ_SLOT_INFO_CORE_1:
                case EN_SCENARIO.SCENARIO_REQ_SLOT_INFO_CORE_2:
                case EN_SCENARIO.SCENARIO_REQ_SLOT_INFO_CORE_3:
                case EN_SCENARIO.SCENARIO_REQ_SLOT_INFO_EMPTY_TAPE:
                    {
                        DataToSend[LotInfoKeys.KeyParamLotId] = lotId;
                        DataToSend[LotInfoKeys.KeyParamCarrierId] = carrierId;
                    }
                    break;

                case EN_SCENARIO.SCENARIO_RFID_READ_CORE_1:
                case EN_SCENARIO.SCENARIO_RFID_READ_CORE_2:
                case EN_SCENARIO.SCENARIO_RFID_READ_CORE_3:
                case EN_SCENARIO.SCENARIO_RFID_READ_EMPTY_TAPE:
                case EN_SCENARIO.SCENARIO_RFID_READ_BIN_1:
                case EN_SCENARIO.SCENARIO_RFID_READ_BIN_2:
                case EN_SCENARIO.SCENARIO_RFID_READ_BIN_3:
                    {
                        DataToSend[RFIDReadKeys.KeyParamLotId] = lotId;
                        DataToSend[RFIDReadKeys.KeyParamCarrierId] = carrierId;
                        DataToSend[RFIDReadKeys.KeyParamPortId] = portName;
                        DataToSend[RFIDReadKeys.KeyParamOperatorId] = operatorId;
                    }
                    break;                

                case EN_SCENARIO.SCENARIO_SLOT_WAFER_MAPPING_CORE_1:
                case EN_SCENARIO.SCENARIO_SLOT_WAFER_MAPPING_CORE_2:
                case EN_SCENARIO.SCENARIO_SLOT_WAFER_MAPPING_CORE_3:
                case EN_SCENARIO.SCENARIO_SLOT_WAFER_MAPPING_EMPTY_TAPE:
                case EN_SCENARIO.SCENARIO_SLOT_WAFER_MAPPING_BIN_1:
                case EN_SCENARIO.SCENARIO_SLOT_WAFER_MAPPING_BIN_2:
                case EN_SCENARIO.SCENARIO_SLOT_WAFER_MAPPING_BIN_3:
                case EN_SCENARIO.SCENARIO_REQ_LOT_ID_MERGE_AND_CHANGE_BIN_1:
                case EN_SCENARIO.SCENARIO_REQ_LOT_ID_MERGE_AND_CHANGE_BIN_2:
                case EN_SCENARIO.SCENARIO_REQ_LOT_ID_MERGE_AND_CHANGE_BIN_3:
                    {
                        DataToSend[SlotMappingKeys.KeyParamLotId] = lotId;
                        DataToSend[SlotMappingKeys.KeyParamCarrierId] = carrierId;
                        var substrates = _substrateManager.GetSubstratesAtLoadPort(_selectedPortId);
                        for (int i = 0; i < 25; ++i)
                        {
                            string keyId = string.Format("{0}{1}_{2}", SlotMappingKeys.KeyParamSlotNamePre, i + 1, SlotMappingKeys.KeyParamSlotNamePost);
                            string keyQty = string.Format("{0}{1}_{2}", SlotMappingKeys.KeyParamSlotQtyPre, i + 1, SlotMappingKeys.KeyParamSlotQtyPost);
                            string valueId = string.Empty;
                            string valueQty = "";
                            if (substrates.TryGetValue(i, out Substrate substrate))
                            {
                                valueId = substrate.Name;
                                valueQty = substrate.GetAttribute(PWA500SubstrateAttributes.ChipQty);
                                if (string.IsNullOrEmpty(valueQty) || valueQty.Equals("0"))
                                    valueQty = "";
                            }
                            
                            DataToSend[keyId] = valueId;
                            DataToSend[keyQty] = valueQty;
                        }
                    }
                    break;
                case EN_SCENARIO.SCENARIO_REQ_LOT_MERGE_CORE_1:
                case EN_SCENARIO.SCENARIO_REQ_LOT_MERGE_CORE_2:
                case EN_SCENARIO.SCENARIO_REQ_LOT_MERGE_CORE_3:
                    {
                        bool isCoreScenario = false;
                        if (scenario.Equals(EN_SCENARIO.SCENARIO_REQ_LOT_MERGE_CORE_1) ||
                            scenario.Equals(EN_SCENARIO.SCENARIO_REQ_LOT_MERGE_CORE_2) ||
                            scenario.Equals(EN_SCENARIO.SCENARIO_REQ_LOT_MERGE_CORE_3))
                        {
                            isCoreScenario = true;
                        }

                        string partId = string.Empty;
                        string recipeId = _functionsForPWA500.GetRecipeId();
                        var substrates = _substrateManager.GetSubstratesAtLoadPort(_selectedPortId);

                        bool isFisrtSubstrate = false;
                        for (int i = 0; i < 25; ++i)
                        {
                            string keyId = string.Format("{0}{1}_{2}", LotMergeKeys.KeyParamSlotLotIdPre, i + 1, LotMergeKeys.KeyParamSlotLotIdPost);
                            string valueId = string.Empty;
                            if (substrates.TryGetValue(i, out Substrate substrate))
                            {
                                valueId = substrate.LotId;
                                partId = substrate.GetAttribute(PWA500SubstrateAttributes.PartId);
                                //recipeId = substrate.GetRecipeId();

                                if (false == isCoreScenario)
                                {
                                    if (false == isFisrtSubstrate)
                                    {
                                        isFisrtSubstrate = true;
                                        lotId = valueId;
                                    }
                                }
                            }

                            DataToSend[keyId] = valueId;
                        }

                        DataToSend[LotMergeKeys.KeyParamLotId] = lotId;
                        DataToSend[LotMergeKeys.KeyParamCarrierId] = carrierId;
                        DataToSend[LotMergeKeys.KeyParamPartId] = partId;
                        DataToSend[LotMergeKeys.KeyParamRecipeId] = recipeId;
                        DataToSend[LotMergeKeys.KeyOperatorId] = operatorId;
                    }
                    break;

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
                case EN_SCENARIO.SCENARIO_REQ_RECIPE_DOWNLOAD:
                    break;
            }

            UpdateGridViewByAppliedData();
        }
        private void UpdateSubstrateScenarioData(EN_SCENARIO scenario, Substrate substrate1, Substrate substrate2)
        {
            if (substrate1 == null)
                return;

            int portId = substrate1.SourcePortId;
            int slotId = substrate1.SourceSlot + 1;
            
            string substrateName = substrate1.Name;
            string ringId = substrate1.GetAttribute(PWA500SubstrateAttributes.RingId);
            string carrierId = _carrierServer.GetCarrierId(portId);
            string portName = _functionsForPWA500.GetPortName(portId);
            string lotId = substrate1.LotId;
            string partId = substrate1.GetAttribute(PWA500SubstrateAttributes.PartId);
            string stepSeq = substrate1.GetAttribute(PWA500SubstrateAttributes.StepSeq);
            string recipeId = _functionsForPWA500.GetRecipeId();
            string operatorId = "AUTO";
            string chipQty = substrate1.GetAttribute(PWA500SubstrateAttributes.ChipQty);
            
            switch (scenario)
            {
                #region <Core Only>
                case EN_SCENARIO.SCENARIO_PROCESS_START:
                case EN_SCENARIO.SCENARIO_PROCESS_END:
                    {                        
                        DataToSend[EESKeys.KeyCarrierId] = carrierId;
                        DataToSend[EESKeys.KeyPortId] = portName;
                        DataToSend[EESKeys.KeyLotId] = lotId;
                        DataToSend[EESKeys.KeyPartId] = partId;
                        DataToSend[EESKeys.KeyParamRecipeId] = recipeId;
                        DataToSend[EESKeys.KeyOperatorId] = operatorId;
                    }
                    break;
                case EN_SCENARIO.SCENARIO_REQ_TRACK_IN:
                case EN_SCENARIO.SCENARIO_REQ_CORE_WAFER_TRACK_OUT:
                    {                        
                        DataToSend[TrackInOrOut.KeyParamCarrierId] = carrierId;
                        DataToSend[TrackInOrOut.KeyParamPortId] = portName;
                        DataToSend[TrackInOrOut.KeyParamLotId] = lotId;
                        DataToSend[TrackInOrOut.KeyParamPartId] = partId;
                        DataToSend[TrackInOrOut.KeyParamStepSeq] = stepSeq;
                        DataToSend[TrackInOrOut.KeyParamRecipeId] = recipeId;
                        DataToSend[TrackInOrOut.KeyParamChipQty] = chipQty;
                        DataToSend[TrackInOrOut.KeyParamOperatorId] = operatorId;
                    }
                    break;
                case EN_SCENARIO.SCENARIO_WORK_START:
                    {
                        DataToSend[RequestDownloadMapFileKeys.KeyParamCarrierId] = carrierId;
                        DataToSend[RequestDownloadMapFileKeys.KeyParamPortId] = portName;
                        DataToSend[RequestDownloadMapFileKeys.KeyParamLotId] = lotId;
                        DataToSend[RequestDownloadMapFileKeys.KeyParamPartId] = partId;                        
                        DataToSend[RequestDownloadMapFileKeys.KeyParamRecipeId] = recipeId;
                        DataToSend[RequestDownloadMapFileKeys.KeyParamOperatorId] = operatorId;
                        DataToSend[RequestDownloadMapFileKeys.KeyParamWaferId] = substrateName;
                        DataToSend[RequestDownloadMapFileKeys.KeyParamAngle] = "0";
                        DataToSend[RequestDownloadMapFileKeys.KeyNullBinCode] = " ";
                        DataToSend[RequestDownloadMapFileKeys.KeyUseEventHandling] = bool.TrueString;
                    }
                    break;

                case EN_SCENARIO.SCENARIO_WORK_END:
                    {
                        DataToSend[UploadCoreOrBinFileKeys.KeyParamCarrierId] = carrierId;
                        DataToSend[UploadCoreOrBinFileKeys.KeyParamPortId] = portName;
                        DataToSend[UploadCoreOrBinFileKeys.KeyParamLotId] = lotId;
                        DataToSend[UploadCoreOrBinFileKeys.KeyParamPartId] = partId;
                        DataToSend[UploadCoreOrBinFileKeys.KeyParamRecipeId] = recipeId;
                        DataToSend[UploadCoreOrBinFileKeys.KeyParamChipQty] = chipQty;
                        DataToSend[UploadCoreOrBinFileKeys.KeyParamOperatorId] = operatorId;
                        DataToSend[UploadCoreOrBinFileKeys.KeyPMSFileName] = string.Empty;
                        DataToSend[UploadCoreOrBinFileKeys.KeyPMSFileBody] = string.Empty;
                        DataToSend[UploadCoreOrBinFileKeys.KeySubstrateName] = substrateName;
                        DataToSend[UploadCoreOrBinFileKeys.KeyWaferAngle] = "0";
                        DataToSend[UploadCoreOrBinFileKeys.KeyCountRow] = "0";
                        DataToSend[UploadCoreOrBinFileKeys.KeyCountCol] = "0";
                        DataToSend[UploadCoreOrBinFileKeys.KeyReferenceX] = "0";
                        DataToSend[UploadCoreOrBinFileKeys.KeyReferenceY] = "0";
                        DataToSend[UploadCoreOrBinFileKeys.KeyStartingPosX] = "0";
                        DataToSend[UploadCoreOrBinFileKeys.KeyStartingPosY] = "0";
                        DataToSend[UploadCoreOrBinFileKeys.KeyNullBinCode] = " ";
                        DataToSend[UploadCoreOrBinFileKeys.KeyUseEventHandling] = bool.TrueString;
                    }
                    break;

                case EN_SCENARIO.SCENARIO_REQ_CORE_WAFER_SPLIT:
                case EN_SCENARIO.SCENARIO_REQ_CORE_WAFER_SPLIT_LAST:
                    {
                        DataToSend[AssignSubstrateLotIdKeys.KeyParamLotId] = lotId;
                        DataToSend[AssignSubstrateLotIdKeys.KeyParamWaferId] = substrateName;                        
                        DataToSend[AssignSubstrateLotIdKeys.KeyParamPartId] = partId;                        
                        DataToSend[AssignSubstrateLotIdKeys.KeyParamRecipeId] = recipeId;
                        DataToSend[AssignSubstrateLotIdKeys.KeyParamSlotId] = slotId.ToString();
                        DataToSend[AssignSubstrateLotIdKeys.KeyParamOperatorId] = operatorId;
                    }
                    break;
                
                case EN_SCENARIO.SCENARIO_CORE_WAFER_DETACH_START:
                case EN_SCENARIO.SCENARIO_CORE_WAFER_DETACH_END:
                    {                        
                        DataToSend[DetachingKeys.KeyParamCarrierId] = carrierId;
                        DataToSend[DetachingKeys.KeyParamPortId] = portName;
                        DataToSend[DetachingKeys.KeyParamLotId] = lotId;
                        DataToSend[DetachingKeys.KeyParamPartId] = partId;
                        DataToSend[DetachingKeys.KeyParamRecipeId] = recipeId;
                        DataToSend[DetachingKeys.KeyParamWaferId] = substrateName;
                        DataToSend[DetachingKeys.KeyParamSlotId] = slotId.ToString();
                        DataToSend[DetachingKeys.KeyParamOperatorId] = operatorId;
                        
                        // FDC는 매뉴얼 입력이니 냅두자
                        if (scenario.Equals(EN_SCENARIO.SCENARIO_CORE_WAFER_DETACH_START))
                        {

                        }
                    }
                    break;
                #endregion </Core Only>

                #region <Core and Bin or Empty Only>
                case EN_SCENARIO.SCENARIO_REQ_CORE_CHIP_SPLIT_FIRST:
                case EN_SCENARIO.SCENARIO_REQ_CORE_CHIP_SPLIT:
                case EN_SCENARIO.SCENARIO_REQ_CORE_CHIP_FULL_SPLIT_FIRST:
                case EN_SCENARIO.SCENARIO_REQ_CORE_CHIP_FULL_SPLIT:
                    {
                        if (substrate2 == null)
                            return;

                        string binCode = substrate2.GetAttribute(PWA500SubstrateAttributes.BinCode);
                        string ringIdToSort = substrate2.GetAttribute(PWA500SubstrateAttributes.RingId);

                        DataToSend[SplitCoreChipKeys.KeyParamLotId] = lotId;
                        DataToSend[SplitCoreChipKeys.KeyParamSplitWaferId] = substrateName;
                        DataToSend[SplitCoreChipKeys.KeyParamRingFrameId] = ringIdToSort;
                        DataToSend[SplitCoreChipKeys.KeyParamBinType] = binCode;
                        DataToSend[SplitCoreChipKeys.KeyParamSplitChipQty] = chipQty;
                    }
                    break;

                case EN_SCENARIO.SCENARIO_REQ_CORE_CHIP_MERGE:
                    {
                        if (substrate2 == null)
                            return;

                        string binCode = substrate2.GetAttribute(PWA500SubstrateAttributes.BinCode);
                        string ringIdToSort = substrate2.GetAttribute(PWA500SubstrateAttributes.RingId);
                        string lotIdToSort = substrate2.LotId;
                        string splittedLotId = substrate2.GetAttribute(PWA500SubstrateAttributes.SplittedLotId);
                        string splittedChipQty = substrate2.GetAttribute(PWA500SubstrateAttributes.ChipQty);

                        DataToSend[SplitCoreChipKeys.KeyParamLotId] = lotIdToSort;
                        DataToSend[SplitCoreChipKeys.KeyParamSplitLotId] = splittedLotId;
                        DataToSend[SplitCoreChipKeys.KeyParamSplitWaferId] = substrateName;
                        DataToSend[SplitCoreChipKeys.KeyParamRingFrameId] = ringIdToSort;
                        DataToSend[SplitCoreChipKeys.KeyParamBinType] = binCode;
                        DataToSend[SplitCoreChipKeys.KeyParamSplitChipQty] = splittedChipQty;
                    }
                    break;
                #endregion </Core and Bin or Empty Only>

                #region <Bin or Empty Only>
                case EN_SCENARIO.SCENARIO_REQ_LOT_MATCH:
                    {
                        DataToSend[DetachingKeys.KeyParamLotId] = lotId;
                        DataToSend[DetachingKeys.KeyParamCarrierId] = carrierId;
                        DataToSend[TrackInOrOut.KeyParamChangeReason] = Constants.EmptyWaferChangeReason;
                        DataToSend[TrackInOrOut.KeyParamMaterialType] = Constants.EmptyWaferMaterialType;
                    }
                    break;

                case EN_SCENARIO.SCENARIO_REQ_BIN_WAFER_TRACK_OUT:
                    {
                        int destPortId = substrate1.DestinationPortId;
                        string binCode = substrate1.GetAttribute(PWA500SubstrateAttributes.BinCode);

                        carrierId = _carrierServer.GetCarrierId(destPortId);
                        portName = _functionsForPWA500.GetPortName(destPortId);                        

                        DataToSend[TrackInOrOut.KeyParamCarrierId] = carrierId;
                        DataToSend[TrackInOrOut.KeyParamPortId] = portName;
                        DataToSend[TrackInOrOut.KeyParamLotId] = lotId;
                        DataToSend[TrackInOrOut.KeyParamPartId] = partId;
                        DataToSend[TrackInOrOut.KeyParamStepSeq] = stepSeq;
                        DataToSend[TrackInOrOut.KeyParamRecipeId] = recipeId;
                        DataToSend[TrackInOrOut.KeyParamChipQty] = chipQty.ToString();
                        DataToSend[TrackInOrOut.KeyParamBinType] = binCode;
                        DataToSend[TrackInOrOut.KeyParamOperatorId] = operatorId;
                    }
                    break;
                
                case EN_SCENARIO.SCENARIO_BIN_WAFER_ID_READ:
                    {                        
                        DataToSend[AssignRingIdKeys.KeyParamLotId] = lotId;
                        DataToSend[AssignRingIdKeys.KeyParamWaferId] = ringId;
                    }
                    break;

                case EN_SCENARIO.SCENARIO_BIN_DATA_UPLOAD:
                    {
                        portId = substrate1.DestinationPortId;
                        carrierId = _carrierServer.GetCarrierId(portId);
                        portName = _functionsForPWA500.GetPortName(portId);

                        DataToSend[UploadCoreOrBinFileKeys.KeyParamCarrierId] = carrierId;
                        DataToSend[UploadCoreOrBinFileKeys.KeyParamPortId] = portName;
                        DataToSend[UploadCoreOrBinFileKeys.KeyParamLotId] = lotId;
                        DataToSend[UploadCoreOrBinFileKeys.KeyParamRecipeId] = recipeId;
                        DataToSend[UploadCoreOrBinFileKeys.KeyParamPartId] = partId;
                        DataToSend[UploadCoreOrBinFileKeys.KeyParamOperatorId] = operatorId;
                        DataToSend[UploadCoreOrBinFileKeys.KeyParamChipQty] = chipQty;
                        DataToSend[UploadCoreOrBinFileKeys.KeyMapData] = substrate1.GetAttribute(PWA500SubstrateAttributes.MapData);
                        DataToSend[UploadCoreOrBinFileKeys.KeyPMSFileName] = string.Empty;
                        DataToSend[UploadCoreOrBinFileKeys.KeyPMSFileBody] = string.Empty;
                        DataToSend[UploadCoreOrBinFileKeys.KeySubstrateName] = substrateName;
                        DataToSend[UploadCoreOrBinFileKeys.KeyWaferAngle] = "0";
                        DataToSend[UploadCoreOrBinFileKeys.KeyCountRow] = "0";
                        DataToSend[UploadCoreOrBinFileKeys.KeyCountCol] = "0";
                        DataToSend[UploadCoreOrBinFileKeys.KeyReferenceX] = "0";
                        DataToSend[UploadCoreOrBinFileKeys.KeyReferenceY] = "0";
                        DataToSend[UploadCoreOrBinFileKeys.KeyStartingPosX] = "0";
                        DataToSend[UploadCoreOrBinFileKeys.KeyStartingPosY] = "0";
                        DataToSend[UploadCoreOrBinFileKeys.KeyNullBinCode] = " ";
                        DataToSend[UploadCoreOrBinFileKeys.KeyUseEventHandling] = bool.TrueString;
                    }
                    break;

                case EN_SCENARIO.SCENARIO_BIN_WORK_END:
                    {
                        portId = substrate1.DestinationPortId;
                        carrierId = _carrierServer.GetCarrierId(portId);
                        portName = _functionsForPWA500.GetPortName(portId);

                        DataToSend[UploadCoreOrBinFileKeys.KeyParamCarrierId] = carrierId;
                        DataToSend[UploadCoreOrBinFileKeys.KeyParamPortId] = portName;
                        DataToSend[UploadCoreOrBinFileKeys.KeyParamLotId] = lotId;
                        DataToSend[UploadCoreOrBinFileKeys.KeyParamPartId] = partId;
                        DataToSend[UploadCoreOrBinFileKeys.KeyParamRecipeId] = recipeId;
                        DataToSend[UploadCoreOrBinFileKeys.KeyParamChipQty] = chipQty;
                        DataToSend[UploadCoreOrBinFileKeys.KeyParamOperatorId] = operatorId;
                        DataToSend[UploadCoreOrBinFileKeys.KeyPMSFileName] = string.Empty;
                        DataToSend[UploadCoreOrBinFileKeys.KeyPMSFileBody] = string.Empty;
                        DataToSend[UploadCoreOrBinFileKeys.KeySubstrateName] = substrateName;
                        DataToSend[UploadCoreOrBinFileKeys.KeyWaferAngle] = "0";
                        DataToSend[UploadCoreOrBinFileKeys.KeyCountRow] = "0";
                        DataToSend[UploadCoreOrBinFileKeys.KeyCountCol] = "0";
                        DataToSend[UploadCoreOrBinFileKeys.KeyReferenceX] = "0";
                        DataToSend[UploadCoreOrBinFileKeys.KeyReferenceY] = "0";
                        DataToSend[UploadCoreOrBinFileKeys.KeyStartingPosX] = "0";
                        DataToSend[UploadCoreOrBinFileKeys.KeyStartingPosY] = "0";
                        DataToSend[UploadCoreOrBinFileKeys.KeyNullBinCode] = " ";
                        DataToSend[UploadCoreOrBinFileKeys.KeyUseEventHandling] = bool.TrueString;
                    }
                    break;

                // LOTID
                // BIN_TYPE
                // RINGFRAME_ID
                // SLOTID
                // CHIP_QTY
                case EN_SCENARIO.SCENARIO_REQ_BIN_WAFER_ID_ASSIGN:
                    {
                        slotId = substrate1.DestinationSlot + 1;
                        string binCode = substrate1.GetAttribute(PWA500SubstrateAttributes.BinCode);

                        DataToSend[AssignSubstrateIdKeys.KeyParamLotId] = lotId;
                        DataToSend[AssignSubstrateIdKeys.KeyParamBinType] = binCode;
                        DataToSend[AssignSubstrateIdKeys.KeyParamRingFrameId] = ringId;
                        DataToSend[AssignSubstrateIdKeys.KeyParamSlotId] = slotId.ToString();
                        DataToSend[AssignSubstrateIdKeys.KeyParamChipQty] = chipQty;
                    }
                    break;

                // LOTID
                // BIN_TYPE
                // RING_FRAME_ID
                case EN_SCENARIO.SCENARIO_BIN_SORTING_START_1:
                case EN_SCENARIO.SCENARIO_BIN_SORTING_START_2:
                case EN_SCENARIO.SCENARIO_BIN_SORTING_START_3:
                    {
                        string binCode = substrate1.GetAttribute(PWA500SubstrateAttributes.BinCode);

                        DataToSend[SortingKeys.KeyParamLotId] = lotId;
                        DataToSend[SortingKeys.KeyParamBinType] = binCode;
                        DataToSend[SortingKeys.KeyParamRingFrameId] = ringId;
                    }
                    break;

                // LOTID
                // BIN_TYPE
                // RINGFRAME_ID
                // CHIP_QTY
                case EN_SCENARIO.SCENARIO_BIN_SORTING_END_1:
                case EN_SCENARIO.SCENARIO_BIN_SORTING_END_2:
                case EN_SCENARIO.SCENARIO_BIN_SORTING_END_3:
                    //case ScenarioListTypes.SCENARIO_ASSIGN_SUBSTRATE_ID:
                    {
                        string binCode = substrate1.GetAttribute(PWA500SubstrateAttributes.BinCode);

                        DataToSend[SortingKeys.KeyParamLotId] = lotId;
                        DataToSend[SortingKeys.KeyParamBinType] = binCode;
                        DataToSend[SortingKeys.KeyParamRingFrameId] = ringId;
                        DataToSend[SortingKeys.KeyParamChipQty] = chipQty;
                    }
                    break;

                case EN_SCENARIO.SCENARIO_REQ_UPLOAD_BINFILE:
                    {
                        string stepId = substrate1.GetAttribute(PWA500SubstrateAttributes.StepSeq);

                        // 2024.10.29. jhlim [MOD] StepSeq가 설정값과 다르면 값을 셋한다.
                        string stepSeqFromParam = _functionsForPWA500.GetStepIdForBinWafer();
                        if (stepId.Equals(stepSeqFromParam))
                        {
                            _substrateManager.SetAttributeByKey(substrate1.UniqueKey, PWA500SubstrateAttributes.StepSeq, stepSeqFromParam);
                        }

                        stepId = stepSeqFromParam;
                        // 2024.10.29. jhlim [END]

                        DataToSend[UploadCoreOrBinFileKeys.KeySubstrateName] = substrateName;
                        DataToSend[UploadCoreOrBinFileKeys.KeyRingId] = ringId;
                        DataToSend[UploadCoreOrBinFileKeys.KeyRecipeId] = recipeId;
                        DataToSend[UploadCoreOrBinFileKeys.KeySubstrateType] = substrate1.GetAttribute(PWA500SubstrateAttributes.BinCode);
                        DataToSend[UploadCoreOrBinFileKeys.KeyStepId] = stepId;
                        DataToSend[UploadCoreOrBinFileKeys.KeyEquipId] = _recipe.GetValue(FrameOfSystem3.Recipe.EN_RECIPE_TYPE.EQUIPMENT, FrameOfSystem3.Recipe.PARAM_EQUIPMENT.MachineName.ToString(), string.Empty);
                        DataToSend[UploadCoreOrBinFileKeys.KeyPartId] = partId;
                        DataToSend[UploadCoreOrBinFileKeys.KeySlot] = slotId.ToString();
                        DataToSend[UploadCoreOrBinFileKeys.KeyLotId] = lotId;
                    }
                    break;

                #endregion </Bin or Empty Only>

                default:
                    break;
            }

            UpdateGridViewByAppliedData();
        }
        #endregion </Internal>

        #endregion </Methods>
    }
}