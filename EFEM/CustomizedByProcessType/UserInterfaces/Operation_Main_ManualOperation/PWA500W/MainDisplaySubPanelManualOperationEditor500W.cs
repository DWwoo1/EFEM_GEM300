using System;

using System.Collections.Generic;
using System.Collections.Concurrent;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using EquipmentState_;

using FrameOfSystem3.Task;
using Define.DefineEnumProject.Task;
using FrameOfSystem3.Recipe;
using FrameOfSystem3.SECSGEM;
using FrameOfSystem3.SECSGEM.Scenario;
using FrameOfSystem3.Views;
using FrameOfSystem3.Views.Functional;
using FrameOfSystem3.Views.MapManager;
using FrameOfSystem3.Views.Operation.SubPanelSummary.LoadPortSummary;

using EFEM.Modules;
using EFEM.Defines.AtmRobot;
using EFEM.MaterialTracking;
using EFEM.Defines.MaterialTracking;
using EFEM.ActionScheduler;
using EFEM.CustomizedByProcessType.PWA500W;
using EFEM.CustomizedByProcessType.PWA500Common;

namespace EFEM.CustomizedByProcessType.UserInterface.OperationMainManual.PWA500W
{
    public partial class MainDisplaySubPanelManualOperationEditor500W : UserControlForMainView.CustomView
    {
        #region <Constructors>
        public MainDisplaySubPanelManualOperationEditor500W()
        {
            InitializeComponent();

            //this.Tag = name;

            _equipmentState = EquipmentState.GetInstance();
            _taskOperator = TaskOperator.GetInstance();
            _loadPortManager = LoadPortManager.Instance;
            _carrierServer = CarrierManagementServer.Instance;
            _processGroup = ProcessModuleGroup.Instance;
            _substrateManager = SubstrateManager.Instance;
            _robotManager = AtmRobotManager.Instance;
            _robotSchedulerManager = RobotActionSchedulerManager.Instance;
            _messageBox = Form_MessageBox.GetInstance();
            _recipe = FrameOfSystem3.Recipe.Recipe.GetInstance();
            _functionsForPWA500 = FunctionsForPWA500W_NRD.Instance;

            LoadPortPanels = new Dictionary<int, Panel>
            {
                { pnLoadPort1.TabIndex, pnLoadPort1 },
                { pnLoadPort2.TabIndex, pnLoadPort2 },
                { pnLoadPort3.TabIndex, pnLoadPort3 },
                { pnLoadPort4.TabIndex, pnLoadPort4 }
            };

            LoadPortSlots = new Dictionary<int, SummaryLoadPortState_Slot>();
            foreach (var item in LoadPortPanels)
            {
                var eventHandler = new DelegateCellClicked(LoadPortMapCellClicked);
                LoadPortSlots.Add(item.Key, new SummaryLoadPortState_Slot(item.Key, eventHandler));
                LoadPortSlots[item.Key].Dock = DockStyle.Fill;

                item.Value.Controls.Add(LoadPortSlots[item.Key]);
            }

            _robotStateInformation = new RobotStateInformation();
            RobotArmControls = new ConcurrentDictionary<RobotArmTypes, Sys3Controls.Sys3Label>();
            RobotArmControls.TryAdd(RobotArmTypes.UpperArm, lblUpperArmSubstrateInfo);
            RobotArmControls.TryAdd(RobotArmTypes.LowerArm, lblLowerArmSubstrateInfo);

            _substratesInArm = new Dictionary<RobotArmTypes, Substrate>();

            _substratesAtProcessModule = new List<Substrate>();
            _core_8_SubstratesAtProcessModule = new List<Substrate>();
            _core_12_SubstratesAtProcessModule = new List<Substrate>();
            _sortSubstratesAtProcessModule = new List<Substrate>();

            _selectionList = Form_SelectionList.GetInstance();

            LoadPortNames = new ConcurrentDictionary<int, Sys3Controls.Sys3Label>();
            LoadPortNames[0] = lblLoadPort1;
            LoadPortNames[1] = lblLoadPort2;
            LoadPortNames[2] = lblLoadPort3;
            LoadPortNames[3] = lblLoadPort4;

            // 2026.07.09. [ADD] lblLoadPort(헤더)와 슬롯맵 사이에 캐리어 이름 라벨을 생성.
            // 슬롯맵이 이미 Dock.Fill 로 추가된 뒤라, 여기서 Dock.Top 으로 추가하면 상단에 배치된다.
            CarrierLabels = new ConcurrentDictionary<int, Sys3Controls.Sys3Label>();
            foreach (var item in LoadPortPanels)
            {
                var carrierLabel = new Sys3Controls.Sys3Label
                {
                    Dock = DockStyle.Top,
                    Height = 24,
                    BackGroundColor = Color.Khaki,
                    MainFont = new Font("맑은 고딕", 9F, FontStyle.Bold),
                    MainFontColor = Color.Black,
                    TextAlignMain = Sys3Controls.EN_TEXTALIGN.MIDDLE_CENTER,
                    UseBorder = true,
                    BorderStroke = 1,
                    BorderStyle = ButtonBorderStyle.Solid,
                    Description = "",
                    UseImage = false,
                    UseSubFont = false,
                    Text = string.Empty,
                    Tag = item.Key,
                };
                carrierLabel.Click += CarrierLabelClicked;
                item.Value.Controls.Add(carrierLabel);
                CarrierLabels[item.Key] = carrierLabel;
            }

            InitInfo();

            ProcessModuleName = _processGroup.GetProcessModuleName(ProcessModuleIndex);
        }
        #endregion </Constructors>

        #region <Fields>

        #region <Instances>
        private static TaskOperator _taskOperator = null;
        private static LoadPortManager _loadPortManager = null;
        private static AtmRobotManager _robotManager = null;
        private static CarrierManagementServer _carrierServer = null;
        private static ProcessModuleGroup _processGroup = null;
        private static SubstrateManager _substrateManager = null;
        private static EquipmentState _equipmentState;
        private static RobotActionSchedulerManager _robotSchedulerManager = null;
        private static Form_MessageBox _messageBox = null;
        private static Form_SelectionList _selectionList = null;
        private static FrameOfSystem3.Recipe.Recipe _recipe = null;
        private static FunctionsForPWA500W_NRD _functionsForPWA500 = null;
        #endregion </Instances>

        #region <Constants>
        private const int RobotIndex = 0;
        private const int ProcessModuleIndex = 0;

        private const string TitleBinLoadPort = "BIN LOADPORT";
        private const string TitleEmptyLoadPort = "EMPTY LOADPORT";
        #endregion </Constants>

        #region <Substrate from LoadPort>
        private readonly Dictionary<int, Panel> LoadPortPanels = null;
        private readonly Dictionary<int, SummaryLoadPortState_Slot> LoadPortSlots = null;
        #endregion </Substrate from LoadPort>

        #region <Substrates in Process Module>
        private const int ColumnSubstrateName = 0;
        private const int ColumnRequestEnabled = 0;
        private const int ColumnRequestLocation = 1;

        private List<Substrate> _substratesAtProcessModule = null;
        private List<Substrate> _core_8_SubstratesAtProcessModule = null;   // 2025.02.13 dwlim [ADD] 500W에 맞게 수정
        private List<Substrate> _core_12_SubstratesAtProcessModule = null;  // 2025.02.13 dwlim [ADD] 500W에 맞게 수정
        private List<Substrate> _sortSubstratesAtProcessModule = null;
        #endregion </Substrates in Process Module>

        #region <Substrates In Robot>
        private RobotStateInformation _robotStateInformation = null;
        private readonly ConcurrentDictionary<RobotArmTypes, Sys3Controls.Sys3Label> RobotArmControls = null;
        private Dictionary<RobotArmTypes, Substrate> _substratesInArm = null;
        #endregion </Substrates In Robot>

        #region <ETC>
        private readonly ConcurrentDictionary<int, Sys3Controls.Sys3Label> LoadPortNames = null;
        // 2026.07.09. [ADD] 로드포트별 캐리어 이름 라벨(런타임 생성, pnLoadPort 상단 Dock)
        private readonly ConcurrentDictionary<int, Sys3Controls.Sys3Label> CarrierLabels = null;
        #endregion </ETC>

        private readonly string ProcessModuleName = string.Empty;
        private string _selectedLocationName = string.Empty;
        private Substrate _selectedSubstrate = null;

        // 2025.07.08. dwlim [ADD] Move Substrate Information 추가
        private bool _activateMoveSource = false;
        private bool _activateMoveDestination = false;

        private Substrate _selectedSourceSubstrate = null;
        private Substrate _selectedDestinationSubstrate = null;

        private Location _selectedSourceLocation = null;
        private Location _selectedDestinationLocation = null;

        private readonly Color LableDefaultColor = Color.Bisque;
        private readonly Color LableActivateColor = Color.Chocolate;

        private readonly Color MoveButtonFirstColor = Color.DeepSkyBlue;
        private readonly Color MoveButtonSecondColor = Color.SteelBlue;

        private readonly Color ApplyButtonFirstColor = Color.PaleGreen;
        private readonly Color ApplyButtonSecondColor = Color.ForestGreen;

        private bool _isSubstrateMovePrepared = false;
        private bool _isSubstrateMoveActivated = false;
        private bool _isSubstrateMoveInitialized = false;
        // 2025.07.08. dwlim [END]
        #endregion </Fields>

        #region <Methods>

        #region <Override Methods>
        protected override void ProcessWhenActivation()
        {
            foreach (var item in LoadPortSlots)
            {
                item.Value.ActivateView();
            }
            
            gvCoreSubstrateList.ClearSelection();
            gvSortSubstrateList.ClearSelection();
            _selectedLocationName = string.Empty;
            _selectedSubstrate = null;

            InitializeMove();           // 2025.07.08. dwlim [ADD] Move Substrate Information 수정
            UpdateSelectedSubstrateInfo();
            DisplayLoadPortNames();
            base.ProcessWhenActivation();
        }
        public override void CallFunctionByTimer()
        {
            if (FrameOfSystem3.Task.TaskOperator.GetInstance().IsExiting)
                return;

            foreach (var item in LoadPortSlots)
            {
                item.Value.CallFunctionByTimer();
            }

            //UpdateSelectedSubstrate();
            //DisplayModuleInfo();
            DisplayRobotInfo();
            RefreshSubstrateList();
            DisplayCarrierNames();                    // 2026.07.09. [ADD] 캐리어 라벨 갱신
            EnableEditButtons();
            UpdateSelectedSubstrateLableColor();      // 2025.07.08. dwlim [ADD] Move Substrate Information 추가

            base.CallFunctionByTimer();
        }
        protected override void ProcessWhenDeactivation()
        {
            foreach (var item in LoadPortSlots)
            {
                item.Value.DeactivateView();
            }

            base.ProcessWhenDeactivation();
        }
        #endregion </Override Methods>

        #region <UI Events>
        private void LoadPortMapCellClicked(int clickedMapIndex, Queue<int> polints)
        {
            UpdateLoadPortInfo(clickedMapIndex, polints.Last());

            DisableHighlight(clickedMapIndex);
            
            UpdateSelectedSubstrateInfo();

            UpdateMoveSubstrateInfo();      // 2025.07.08. dwlim [ADD] Move Substrate Information 추가

            //UpdateSelectedSubstrate();
        }
        private void BtnSelectArmClicked(object sender, EventArgs e)
        {
            RobotArmTypes arm;
            if (sender.Equals(lblUpperArmSubstrateInfo))
            {
                arm = RobotArmTypes.UpperArm;
            }
            else if (sender.Equals(lblLowerArmSubstrateInfo))
            {
                arm = RobotArmTypes.LowerArm;
            }
            else
                return;

            _robotStateInformation = _robotManager.GetStateInformation(RobotIndex);
            if (_robotStateInformation == null)
                return;

            string robotName = _robotManager.GetRobotName(RobotIndex);
            _robotManager.GetSubstrate(robotName, arm, out _selectedSubstrate);

            if (LocationServer.GetRobotLocation(robotName, arm, out var location))
            {
                _selectedLocationName = location.Id;
                UpdateSelectedSubstrateInfo();
                UpdateMoveSubstrateInfo();      // 2025.07.08. dwlim [ADD] Move Substrate Information 추가
            }
        }
        private void GvEditProcessModuleClicked(object sender, EventArgs e)
        {
            if (!(sender is Sys3Controls.Sys3DoubleBufferedDataGridView gv))
                return;

            string[] locations = _processGroup.GetEntrywayNames(ProcessModuleIndex);
            
            if (sender.Equals(gvCoreSubstrateList))
            {
                for(int i = 0; i < locations.Length; ++i)
                {
                    if (locations[i].Contains("Core"))
                    {
                        _selectedLocationName = locations[i];
                        break;
                    }
                }
            }
            else
            {
                for (int i = 0; i < locations.Length; ++i)
                {
                    if (locations[i].Contains("Sort"))
                    {
                        _selectedLocationName = locations[i];
                        break;
                    }
                }
            }
            _selectedSubstrate = null;
            UpdateSelectedSubstrateInfo();
        }
        private void GvEditProcessModuleCellClicked(object sender, DataGridViewCellEventArgs e)
        {
            if (!(sender is Sys3Controls.Sys3DoubleBufferedDataGridView gv))
                return;

            if (e.ColumnIndex < 0 || e.RowIndex < 0)
                return;
           
            if (sender.Equals(gvCoreSubstrateList))
            {
                gvSortSubstrateList.ClearSelection();
            }
            else
            {
                gvCoreSubstrateList.ClearSelection();
            }

            string selectedName = gv[ColumnSubstrateName, e.RowIndex].Value.ToString();
            if (false == GetSubstrateByName(selectedName, out _selectedSubstrate) || _selectedSubstrate == null)
                return;

            _selectedLocationName = _selectedSubstrate.LocationId;
            UpdateSelectedSubstrateInfo();
            UpdateMoveSubstrateInfo();      // 2025.07.08. dwlim [ADD] Move Substrate Information 추가
        }
        private void MainControlClicked(object sender, EventArgs e)
        {
            _selectedLocationName = string.Empty;
            _selectedSubstrate = null;

            UpdateSelectedSubstrateInfo();
        }
        private void BtnEditClicked(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(_selectedLocationName))
                return;

            if (sender.Equals(btnCreate))
            {
                if (false == _messageBox.ShowMessage("정말로 자재정보를 생성할까요?"))
                    return;

                if (LocationServer.GetLocationById(_selectedLocationName, out var location))
                {
                    var name = DateTime.Now.ToString("Unknown_HHmmss");
                    _substrateManager.CreateSubstrate(name, name, location);
                    if (false == _substrateManager.GetSubstrateByKey(name, out var temporarySubstrate) ||
                        temporarySubstrate == null)
                        return;

                    FormMaterialAttributeEdit materialEdit = new FormMaterialAttributeEdit();
                    Dictionary<string, string> targetAttributes = MaterialTracking.SubstrateMapper.ExtractDataAll(temporarySubstrate);
                    if (materialEdit.CreateEditForm(targetAttributes, SubstrateFieldLayoutFactory.Create(FrameOfSystem3.Work.AppConfigManager.Instance.ProcessType)))
                    {
                        Dictionary<string, string> attributeResults = new Dictionary<string, string>();
                        materialEdit.GetResult(ref attributeResults);

                        bool isInValidNewName = (false == attributeResults.TryGetValue("Name", out string resultName) || false == _substrateManager.IsValidSubstrateName(resultName));
                        if (isInValidNewName)
                        {
                            _messageBox.ShowMessage(string.Format("이름에 사용할 수 없는 문자열이 포함되었습니다. : {0}", resultName));
                        }
                        else
                        {
                            //temporarySubstrate.SetAttributesAll(attributeResults);

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

                            //if (location is LoadPortLocation)
                            //{
                            //    var loc = location as LoadPortLocation;
                            //    _substrateManager.AssignSubstrateAtLoadPort(loc.PortId, loc.Slot, temporarySubstrate);
                            //}
                            //else if (location is ProcessModuleLocation)
                            //{
                            //    var loc = location as ProcessModuleLocation;
                            //    _substrateManager.AssignSubstrateAtProcessModule(loc.ProcessModuleName, temporarySubstrate);
                            //    _processGroup.AssignSubstrate(loc.ProcessModuleName, temporarySubstrate);
                            //}
                            //else if (location is RobotLocation)
                            //{
                            //    var loc = location as RobotLocation;
                            //    _substrateManager.AssignSubstrateAtRobot(loc.RobotName, loc.Arm, temporarySubstrate);
                            //    //_robotManager.AssignSubstrate(RobotIndex, loc.Arm, temporarySubstrate);
                            //}

                            _selectedSubstrate = null;
                            _selectedLocationName = string.Empty;
                        }
                    }
                    else
                    {
                        if (temporarySubstrate != null)
                        {
                            _substrateManager.RemoveSubstrateByKey(temporarySubstrate.UniqueKey);
                        }
                    }
                    materialEdit.DisposeControls();
                    materialEdit = null;
                }
            }
            else if (sender.Equals(btnEdit))
            {
                if (_selectedSubstrate == null)
                    return;

                //string currentLocation = _selectedSubstrate.CurrentLocation;
                Dictionary<string, string> targetAttributes = MaterialTracking.SubstrateMapper.ExtractDataAll(_selectedSubstrate);
                FormMaterialAttributeEdit materialEdit = new FormMaterialAttributeEdit();
                if (materialEdit.CreateEditForm(targetAttributes, SubstrateFieldLayoutFactory.Create(FrameOfSystem3.Work.AppConfigManager.Instance.ProcessType)))
                {
                    if (_messageBox.ShowMessage("정말로 자재정보를 변경할까요?"))
                    {
                        Dictionary<string, string> attributeResults = new Dictionary<string, string>();
                        materialEdit.GetResult(ref attributeResults);

                        bool isInValidNewName = (false == attributeResults.TryGetValue("Name", out string resultName) || false == _substrateManager.IsValidSubstrateName(resultName));
                        if (isInValidNewName)
                        {
                            _messageBox.ShowMessage(string.Format("이름에 사용할 수 없는 문자열이 포함되었습니다. : {0}", resultName));

                        }
                        else
                        {
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
                            _substrateManager.SetTransferStatusByKey(key, (TransportStates)data.TransportStatus);
                            _substrateManager.SetProcessingStatusByKey(key, (ProcessingStates)data.ProcessingStatus);
                            _substrateManager.SetIdReadingStateByKey(key, (IdReadingStates)data.IdReadingStatus);
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
                        }
                    }
                }

                materialEdit.DisposeControls();
                materialEdit = null;
            }
            else if (sender.Equals(btnDisable))
            {
                if (LocationServer.GetLocationById(_selectedLocationName, out var location))
                {
                    //if (!(location is RobotLocation targetLocation))
                    //    return;

                    if (false == _substrateManager.GetSubstrateByLocationAndKey(location, string.Empty, out var substrate))
                        return;

                    //substrate.SetProcessingStatus(ProcessingStates.Skipped);
                    _substrateManager.SetProcessingStatusByKey(substrate.UniqueKey, ProcessingStates.Skipped);
                    _substrateManager.SaveDataByKey(substrate.UniqueKey);
                }
            }

            #region <기존_2025.07.08 Move Substrate Information 추가>
            //else if (sender.Equals(btnChangePortInfo))
            //{
            //    Location location = new Location(_selectedLocationName);
            //    if (_locationServer.GetLocationByName(_selectedLocationName, ref location))
            //    {
            //        //if (!(location is RobotLocation targetLocation))
            //        //    return;

            //        // 1. 목표 자재를 가져온다.
            //        Substrate substrate = new Substrate();
            //        if (false == _substrateManager.GetSubstrate(location, string.Empty, ref substrate))
            //            return;

            //        // 2. UI 선택을 위한 로드포트 이름을 가져온다.
            //        Dictionary<int, string> loadPortNames = new Dictionary<int, string>();
            //        foreach (var item in LoadPortNames)
            //        {
            //            int portId = _loadPortManager.GetLoadPortPortId(item.Key);
            //            if (false == _carrierServer.HasCarrier(portId))
            //                continue;

            //            loadPortNames[item.Key] = item.Value.Text;
            //        }

            //        // 3. 로드포트 선택창을 띄운다.
            //        if (_selectionList.CreateForm("Select LoadPort", loadPortNames.Values.ToArray(), loadPortNames.Keys.ToArray(), 0))
            //        {
            //            int selectedLoadPort = 0;
            //            _selectionList.GetResult(ref selectedLoadPort);
            //            int portId = _loadPortManager.GetLoadPortPortId(selectedLoadPort);
            //            if (portId <= 0)
            //                return;

            //            int capacity = _carrierServer.GetCapacity(portId);
            //            Dictionary<int, string> slotNames = new Dictionary<int, string>();

            //            // 4. Substrate 목록을 가져와 이름을 갱신한다.
            //            var substrates = _substrateManager.GetSubstratesAtLoadPort(portId);
            //            for (int i = 0; i < capacity; ++i)
            //            {
            //                if (substrates.ContainsKey(i))
            //                    continue;

            //                slotNames[i] = string.Format("Slot {0}", i);
            //            }

            //            // 5. 슬롯 번호를 띄운다.
            //            if (_selectionList.CreateForm("Select Slot", slotNames.Values.ToArray(), slotNames.Keys.ToArray(), 0))
            //            {
            //                int selectedSlot = 0;
            //                _selectionList.GetResult(ref selectedSlot);
            //                if (selectedSlot < 0)
            //                    return;

            //                if (false == _messageBox.ShowWarningMessage(string.Format("현재 선택된 정보가 맞습니까? [Port : {0}, Slot : {1}]\r\n※주의 : 자재 포트 정보가 변경 됩니다.", loadPortNames[selectedLoadPort], slotNames[selectedSlot])))
            //                    return;

            //                // 6. 선택한 슬롯으로 설정한다.
            //                substrate.SetSourcePortId(portId);
            //                substrate.SetSourceSlot(selectedSlot);
            //                substrate.SetDestinationPortId(portId);
            //                substrate.SetDestinationSlot(selectedSlot);
            //                LoadPortLocation targetLocation = new LoadPortLocation(portId, selectedSlot, string.Empty);
            //                _locationServer.GetLoadPortSlotLocation(portId, selectedSlot, ref targetLocation);
            //                _substrateManager.MoveMaterialToModule(targetLocation, substrate);
            //            }
            //        }
            //    }
            //}
            //else if (sender.Equals(btnReplaceFromPM) || sender.Equals(btnReplaceFromLP))
            //{
            //    Location location = new Location(_selectedLocationName);
            //    if (_locationServer.GetLocationByName(_selectedLocationName, ref location))
            //    {
            //        // 1. 원본 Substrate 백업(제거를 위함)
            //        if (!(location is RobotLocation targetLocation))
            //            return;

            //        Substrate substrateOriginal = new Substrate();
            //        string robotName = _robotManager.GetRobotName(RobotIndex);
            //        if (false == _robotManager.GetSubstrate(robotName, targetLocation.Arm, ref substrateOriginal))
            //            return;
            //        string originalName = substrateOriginal.Name;

            //        // 2. 공정설비로부터 Substrate List를 가져옴
            //        Dictionary<int, string> substrateNames = new Dictionary<int, string>();

            //        if (sender.Equals(btnReplaceFromPM))
            //        {
            //            List<Substrate> substrates = new List<Substrate>();
            //            if (false == _substrateManager.GetSubstratesAtProcessModule(ProcessModuleName, ref substrates))
            //                return;

            //            int index = 0;
            //            for (int i = 0; i < substrates.Count; ++i)
            //            {
            //                if (substrates[i].Name.Equals(originalName))
            //                    continue;

            //                substrateNames.Add(index, substrates[i].Name);
            //                index++;
            //            }
            //        }
            //        else
            //        {
            //            Dictionary<int, string> loadPortNames = new Dictionary<int, string>();
            //            foreach (var item in LoadPortNames)
            //            {
            //                int portId = _loadPortManager.GetLoadPortPortId(item.Key);
            //                if (false == _carrierServer.HasCarrier(portId))
            //                    continue;

            //                loadPortNames[item.Key] = item.Value.Text;
            //            }

            //            // 2-1. 로드포트 선택
            //            if (_selectionList.CreateForm("Select LoadPort", loadPortNames.Values.ToArray(), loadPortNames.Keys.ToArray(), 0))
            //            {
            //                int selectedLoadPort = 0;
            //                _selectionList.GetResult(ref selectedLoadPort);
            //                int portId = _loadPortManager.GetLoadPortPortId(selectedLoadPort);
            //                if (portId <= 0)
            //                    return;

            //                // 2-2. Substrate 목록을 가져와 이름을 갱신한다.
            //                var substrates = _substrateManager.GetSubstratesAtLoadPort(portId);
            //                foreach (var item in substrates)
            //                {
            //                    substrateNames[item.Key] = item.Value.Name;
            //                }
            //            }
            //        }

            //        // 3. 자재를 선택(UI)
            //        if (_selectionList.CreateForm("Select Substrate", substrateNames.Values.ToArray(), substrateNames.Keys.ToArray(), 0))
            //        {
            //            string selectedName = string.Empty;
            //            _selectionList.GetResult(ref selectedName);

            //            if (string.IsNullOrEmpty(selectedName))
            //                return;

            //            if (false == _messageBox.ShowWarningMessage(string.Format("현재 선택된 정보가 맞습니까? [{0} -> {1}]\r\n※주의 : 자재 정보가 변경됩니다.", originalName, selectedName)))
            //                return;

            //            // 4. 타겟 자재를 가져옴
            //            Substrate selectedSubstrate = new Substrate();
            //            if (false == _substrateManager.GetSubstrateByName(selectedName, ref selectedSubstrate))
            //                return;

            //            // 5. 원본 Substrate 제거
            //            _substrateManager.RemoveSubstrate(substrateOriginal.Name, location);

            //            // 6. 타겟 자재가 존재하는 위치에서 제거
            //            _substrateManager.MoveMaterialToRobot(selectedSubstrate.CurrentLocation, robotName, targetLocation.Arm, selectedSubstrate);

            //            // 7. 원본 위치에 Set
            //            _substrateManager.SaveRecoveryDataAll();
            //        }

            //    }

            //}
            #endregion </기존_2025.07.08 Move Substrate Information 추가>

            else if (sender.Equals(btnRemove))
            {
                if (_selectedSubstrate == null)
                    return;

                var locId = _selectedSubstrate.LocationId;
                if (false == LocationServer.FindLocationById(locId, out var location))
                    return;

                if (false == _messageBox.ShowMessage("정말로 자재정보를 제거할까요?"))
                    return;

                var key = _selectedSubstrate.UniqueKey;
                var name = _selectedSubstrate.Name;
                _substrateManager.RemoveSubstrateByKey(key);

                if (_selectedLocationName == "ProcessModule" ||
                    location is ProcessModuleLocation)
                {
                }
                else if (location is RobotLocation)
                {
                    var robotLoc = location as RobotLocation;
                    RobotArmControls[robotLoc.Arm].Text = string.Empty;
                }

                //_substrateManager.RemoveSubstrate(key, location);
                _selectedSubstrate = null;
            }

            UpdateSelectedSubstrateInfo();
        }
        // 2026.07.09. [ADD] 로드포트 캐리어 라벨 클릭 → 확인 후 캐리어 속성 편집
        private void CarrierLabelClicked(object sender, EventArgs e)
        {
            if (!(sender is Sys3Controls.Sys3Label label) || label.Tag == null)
                return;

            if (false == int.TryParse(label.Tag.ToString(), out int index))
                return;

            int portId = _loadPortManager.GetLoadPortPortId(index);
            if (false == _carrierServer.HasCarrier(portId))
                return;

            EditCarrierAtPort(portId);
        }
        private void EditCarrierAtPort(int portId)
        {
            if (false == IsEquipmentIdleOrPause())
                return;

            if (false == _carrierServer.HasCarrier(portId))
                return;

            // 편집 전 확인
            if (false == _messageBox.ShowMessage("캐리어 정보를 편집할까요?"))
                return;

            var carrier = _carrierServer.GetCarrierInfoById(_carrierServer.GetCarrierId(portId));
            if (carrier == null)
                return;

            Dictionary<string, string> targetAttributes = MaterialTracking.CarrierMapper.ExtractDataAll(carrier);

            FormMaterialAttributeEdit carrierEdit = new FormMaterialAttributeEdit();
            if (carrierEdit.CreateEditForm(targetAttributes,
                CarrierFieldLayoutFactory.Create(FrameOfSystem3.Work.AppConfigManager.Instance.ProcessType)))
            {
                Dictionary<string, string> attributeResults = new Dictionary<string, string>();
                carrierEdit.GetResult(ref attributeResults);

                var data = MaterialTracking.CarrierMapper.GetCarrierDataFromAttributes(attributeResults, out var extra);
                if (data != null)
                {
                    _carrierServer.SetCarrierId(portId, data.CarrierId);
                    _carrierServer.SetCarrierLotId(portId, data.LotId);
                    _carrierServer.SetCarrierAccessingStatus(portId, (EFEM.Defines.LoadPort.CarrierAccessStates)data.AccessStatus);

                    if (extra != null)
                    {
                        foreach (var kv in extra)
                            _carrierServer.SetAttribute(portId, kv.Key, kv.Value);
                    }

                    // 캐리어는 포트 기반 저장(setter 는 자동저장 안 함)
                    _carrierServer.SaveCarrierData(portId);
                }
            }

            carrierEdit.DisposeControls();
        }
        private void ProcessModuleClicked(object sender, MouseEventArgs e)
        {
            var hitGvSort = gvSortSubstrateList.HitTest(e.X, e.Y);
            var hitGvCore = gvCoreSubstrateList.HitTest(e.X, e.Y);
            if (false == (hitGvSort.Type == DataGridViewHitTestType.None) && false == (hitGvCore.Type == DataGridViewHitTestType.None))
                return;

            if (false == _activateMoveDestination)
                return;

            string[] locations = _processGroup.GetProcessModuleLocations(ProcessModuleIndex);
            List<Substrate> substrates = new List<Substrate>();

            _substrateManager.GetSubstratesAtProcessModule(ProcessModuleName, ref substrates);
            if (_selectionList.CreateForm("Select Process Module Location", locations, _selectedLocationName))
            {
                _selectionList.GetResult(ref _selectedLocationName);
                UpdateMoveSubstrateInfo();
            }

            //string[] locations = _processGroup.GetEntrywayNames(ProcessModuleIndex);
            //string selectedPMLocation = string.Empty;
            ////string substrateType = string.Empty;
            //List<Substrate> substrates = new List<Substrate>();

            //_substrateManager.GetSubstratesAtProcessModule(ProcessModuleName, ref substrates);
            ////substrateType = _functionsForPWA500.GetSubstrateTypeForUILoadPortIndex(_selectedSourceSubstrate.SourcePortId - 1);
            //int sourcePortId = _selectedSourceSubstrate.SourcePortId;
            //int sourceLoadPortIndex = _loadPortManager.GetLoadPortIndexByPortId(sourcePortId);

            //SubstrateType subType = SubstrateType.Core;
            //SubstrateSize subSize = SubstrateSize.Inch_8;
            //if (false == _functionsForPWA500.GetSubstrateSpecByLoadPortIndex(sourceLoadPortIndex, ref subType, ref subSize))
            //    return;

            //if (sender.Equals(gvSortSubstrateList))
            //{
            //    if (false == subType.Equals(SubstrateType.Core))
            //    {
            //        string[] sortLocation = null;
            //        for (int i = 0; i < locations.Length; ++i)
            //        {
            //            if (locations[i].Contains(Constants.Sort_12_Name))
            //            {
            //                sortLocation[i] = locations[i];
            //            }
            //        }

            //        if (_selectionList.CreateForm("Select Process Module Bin Location", sortLocation, _selectedLocationName))
            //        {
            //            _selectionList.GetResult(ref _selectedLocationName);

            //            UpdateMoveSubstrateInfo();
            //        }
            //    }
            //}
            //else if (sender.Equals(gvCoreSubstrateList))
            //{
            //    string[] coreLocation = null;

            //    if (subType.Equals(SubstrateType.Core))
            //    {
            //        if (subSize.Equals(SubstrateSize.Inch_8))
            //        {
            //            coreLocation = new string[2];
            //            for (int i = 0; i < locations.Length; ++i)
            //            {
            //                if (locations[i].Contains(Constants.Core_8_Name))
            //                {
            //                    coreLocation[i] = locations[i];
            //                }
            //            }
            //        }
            //        else if (subSize.Equals(SubstrateSize.Inch_12))
            //        {
            //            coreLocation = new string[2];
            //            for (int i = 0; i < locations.Length; ++i)
            //            {
            //                if (locations[i].Contains(Constants.Core_12_Name))
            //                {
            //                    coreLocation[i] = locations[i];
            //                }
            //            }
            //        }
            //        else
            //            return;

            //        if (_selectionList.CreateForm("Select Process Module Core Location", coreLocation, _selectedLocationName))
            //        {
            //            _selectionList.GetResult(ref _selectedLocationName);

            //            UpdateMoveSubstrateInfo();
            //        }
            //    }
            //}
        }
        // 2025.07.08. dwlim [ADD] Move Substrate Information 추가
        private void BtnSelectForMovingLocation(object sender, EventArgs e)
        {
            if (sender.Equals(lblSelectedSource))
            {
                if (false == _isSubstrateMovePrepared && false == _isSubstrateMoveActivated)
                    return;

                if (false == _activateMoveSource && false == _activateMoveDestination)
                {
                    _activateMoveSource = true;

                    _selectedLocationName = string.Empty;
                    _selectedSubstrate = null;

                    UpdateSelectedSubstrateInfo();
                }
                else
                {
                    _activateMoveSource = false;
                    return;
                }
            }
            else if (sender.Equals(lblSelectedDestination))
            {
                if (false == _isSubstrateMovePrepared && false == _isSubstrateMoveActivated)
                    return;

                if (false == _activateMoveDestination && false == _activateMoveSource)
                {
                    _activateMoveDestination = true;

                    _selectedLocationName = string.Empty;
                    _selectedSubstrate = null;
                    UpdateSelectedSubstrateInfo();
                }
                else
                {
                    _activateMoveDestination = false;
                    return;
                }
            }
            else if (sender.Equals(btnMove))
            {
                if (_isSubstrateMoveInitialized)
                {
                    // 이 때의 버튼은 Move 버튼이고, Substrate 정보 수정을 시작한다.
                    StartMove();

                    return;
                }
                if (_isSubstrateMovePrepared)
                {
                    // 이 때의 버튼은 Cancel 버튼이고, 선택했던 기판의 정보 모두 Clear한다.
                    UpdateMoveSubstrateInfo(true);
                    InitializeMove();
                    return;
                }

                if (null == _selectedSourceSubstrate || false == _isSubstrateMoveActivated)
                    return;

                if (null == _selectedDestinationSubstrate)
                {
                    if (_selectedDestinationLocation is LoadPortLocation)
                    {
                        var location = _selectedDestinationLocation as LoadPortLocation;

                        if (false == _selectedSourceSubstrate.SourcePortId.Equals(location.PortId))
                            return;

                        if (false == _messageBox.ShowMessage("정말로 자재 정보를 이동할까요?"))
                            return;

                        if (_substrateManager.MoveMaterialToLoadPort(location, location.PortId, location.Slot, _selectedSourceSubstrate))
                            InitializeMove();
                    }
                    else if (_selectedDestinationLocation is ProcessModuleLocation)
                    {
                        var location = _selectedDestinationLocation as ProcessModuleLocation;

                        if (false == _messageBox.ShowMessage("정말로 자재 정보를 이동할까요?"))
                            return;

                        if (_substrateManager.MoveMaterialToProcessModule(location, location.ProcessModuleName, location.Id, _selectedSourceSubstrate))
                            InitializeMove();
                    }
                    else if (_selectedDestinationLocation is RobotLocation)
                    {
                        var location = _selectedDestinationLocation as RobotLocation;

                        if (false == _messageBox.ShowMessage("정말로 자재 정보를 이동할까요?"))
                            return;

                        if (_substrateManager.MoveMaterialToATMRobot(location, _robotManager.GetRobotName(RobotIndex), location.Arm, _selectedSourceSubstrate))
                            InitializeMove();
                    }
                }
                else
                {
                    if (false == _messageBox.ShowMessage("정말로 자재 정보를 교환할까요?"))
                        return;
                    if (_substrateManager.SwapMaterialBetweenModules(_selectedSourceSubstrate.UniqueKey, _selectedSourceLocation, _selectedDestinationSubstrate.UniqueKey, _selectedDestinationLocation))
                        InitializeMove();
                }
            }
            else if (sender.Equals(btnCancel))
            {
                UpdateMoveSubstrateInfo(true);
                InitializeMove();
            }
        }
        // 2025.07.08. dwlim [END]
        #endregion </UI Events>

        #region <Display>
        private void DisplayLoadPortNames()
        {
            foreach (var item in LoadPortNames)
            {
                // 2024.09.03. jhlim [MOD] SubType을 UI에는 Center/Left/Right로 지정되도록 변경
                //var recipe = PARAM_EQUIPMENT.LoadPortType1 + item.Key;
                //string name = _recipe.GetValue(EN_RECIPE_TYPE.EQUIPMENT, recipe.ToString(), SubstrateType.Core.ToString());
                //item.Value.Text = name;
                item.Value.Text = _functionsForPWA500.GetLoadPortNameForUIUsingSizeAndType(item.Key);// GetSubstrateTypeForUILoadPortIndex(item.Key);
                // 2024.09.03. jhlim [END]
            }
        }
        // 2026.07.09. [ADD] 캐리어 존재 시 라벨에 Carrier ID 표시(없으면 빈칸)
        private void DisplayCarrierNames()
        {
            if (CarrierLabels == null)
                return;

            foreach (var item in CarrierLabels)
            {
                int portId = _loadPortManager.GetLoadPortPortId(item.Key);
                string text = _carrierServer.HasCarrier(portId) ? _carrierServer.GetCarrierId(portId) : string.Empty;
                if (false == item.Value.Text.Equals(text))
                    item.Value.Text = text;
            }
        }
        private void DisableHighlight(int index)
        {
            foreach (var item in LoadPortSlots)
            {
                if (index >= 0)
                {
                    if (item.Key.Equals(index))
                        continue;
                }

                item.Value.DisableHighlight();
            }
        }
        private void DisplayRobotInfo()
        {
            _robotStateInformation = _robotManager.GetStateInformation(RobotIndex);
            if (_robotStateInformation == null)
                return;
            
            string robotName = _robotManager.GetRobotName(RobotIndex);
            foreach (var item in RobotArmControls)
            {
                if (_robotManager.GetSubstrate(robotName, item.Key, out var s))
                {
                    if (false == RobotArmControls[item.Key].Text.Equals(s.Name))
                    {
                        RobotArmControls[item.Key].Text = s.Name;
                    }
                }
                else
                {
                    RobotArmControls[item.Key].Text = string.Empty;
                }
            }
        }
        private void UpdateSortGridView()
        {
            bool isCleared = false;
            if (gvSortSubstrateList.Rows.Count != _sortSubstratesAtProcessModule.Count)
            {
                gvSortSubstrateList.Rows.Clear();
                isCleared = true;
            }

            for (int i = 0; i < _sortSubstratesAtProcessModule.Count; ++i)
            {
                if (isCleared || gvSortSubstrateList[ColumnSubstrateName, i].Value.ToString() != _sortSubstratesAtProcessModule[i].Name)
                {
                    if (isCleared)
                    {
                        gvSortSubstrateList.Rows.Add();
                    }

                    gvSortSubstrateList[ColumnSubstrateName, i].Value = _sortSubstratesAtProcessModule[i].Name;
                }
            }
        }
        private void UpdateCoreGridView()
        {
            int coreSubstratesAtProcessModuleCount = _core_8_SubstratesAtProcessModule.Count + _core_12_SubstratesAtProcessModule.Count;
            int core_8_IndexCount = _core_8_SubstratesAtProcessModule.Count;
            int gvIndexCount = gvCoreSubstrateList.Rows.Count;
            bool isCleared = false;

            if (gvIndexCount != coreSubstratesAtProcessModuleCount)
            {
                gvCoreSubstrateList.Rows.Clear();
                isCleared = true;
                gvIndexCount = 0;
            }

            for (int i = 0; i < _core_8_SubstratesAtProcessModule.Count; ++i)
            {
                if (isCleared || gvCoreSubstrateList[ColumnSubstrateName, i].Value.ToString() != _core_8_SubstratesAtProcessModule[i].Name)
                {
                    if (isCleared)
                    {
                        gvCoreSubstrateList.Rows.Add();
                        gvIndexCount++;
                    }

                    gvCoreSubstrateList[ColumnSubstrateName, i].Value = _core_8_SubstratesAtProcessModule[i].Name;
                }
            }
            for (int i = 0; i < _core_12_SubstratesAtProcessModule.Count; ++i)
            {
                if (isCleared || gvCoreSubstrateList[ColumnSubstrateName, i + core_8_IndexCount].Value.ToString() != _core_12_SubstratesAtProcessModule[i].Name)
                {
                    if (isCleared)
                    {
                        gvCoreSubstrateList.Rows.Add();
                        gvIndexCount++;
                    }

                    gvCoreSubstrateList[ColumnSubstrateName, i + core_8_IndexCount].Value = _core_12_SubstratesAtProcessModule[i].Name;
                }
            }
        }
        public void RefreshSubstrateList()
        {
            if (_substrateManager.GetSubstratesAtProcessModule(ProcessModuleName, ref _substratesAtProcessModule))
            //if (_processGroup.GetSubstrates(ProcessModuleIndex, ref _temporaryList))
            {
                _core_8_SubstratesAtProcessModule.Clear();
                _core_12_SubstratesAtProcessModule.Clear();
                _sortSubstratesAtProcessModule.Clear();
                for (int i = 0; i < _substratesAtProcessModule.Count; ++i)
                {
                    string subType = _substratesAtProcessModule[i].GetAttribute(PWA500SubstrateAttributes.SubstrateType);
                    if (false == Enum.TryParse(subType, out SubstrateType convertedSubType))
                        continue;

                    string subSize = _substratesAtProcessModule[i].GetAttribute(PWA500SubstrateAttributes.SubstrateSize);
                    if (false == Enum.TryParse(subSize, out SubstrateSize convertedSubSize))
                        continue;

                    switch (convertedSubType)
                    {
                        case SubstrateType.Core:
                            {
                                if (convertedSubSize == SubstrateSize.Inch_8)
                                {
                                    _core_8_SubstratesAtProcessModule.Add(_substratesAtProcessModule[i]);
                                }
                                else
                                {
                                    _core_12_SubstratesAtProcessModule.Add(_substratesAtProcessModule[i]);
                                }
                            }
                            break;

                        default:
                            _sortSubstratesAtProcessModule.Add(_substratesAtProcessModule[i]);
                            break;
                    }
                }

                UpdateCoreGridView();
                UpdateSortGridView();
            }
        }
        private void UpdateSelectedSubstrateInfo()
        {
            lblSelectedSubstrate.SubText = _selectedSubstrate == null ? _selectedLocationName : _selectedSubstrate.LocationId;
            lblSelectedSubstrate.Text = _selectedSubstrate == null ? string.Empty : _selectedSubstrate.Name;
        }
        private void UpdateMoveSubstrateInfo(bool needClear = false)
        {
            if (false == needClear)
            {
                if (false == _isSubstrateMovePrepared && false == _isSubstrateMoveActivated)
                    return;

                if (_activateMoveSource)
                    UpdateSourceSubstrateInfo(needClear);

                if (_activateMoveDestination)
                    UpdateDestinationSubstrateInfo(needClear);
            }
            else
            {
                _activateMoveSource = false;
                _activateMoveDestination = false;
                UpdateSourceSubstrateInfo(needClear);
                UpdateDestinationSubstrateInfo(needClear);
            }
        }
        private void UpdateSourceSubstrateInfo(bool needClear = false)
        {
            if (needClear)
            {
                lblSelectedSource.Text = null;
                _selectedSourceLocation = null;
                _selectedSourceSubstrate = null;
                _activateMoveSource = false;
            }
            else
            {
                if (_selectedSubstrate == null)
                    return;

                if (false == LocationServer.GetLocationById(_selectedLocationName, out _selectedSourceLocation))
                    return;

                if (false == ShowMessagebySelectedSourceLocation(_selectedSourceLocation))
                {
                    _activateMoveSource = false;
                    return;
                }

                if (_substrateManager.GetSubstrateByLocationAndKey(_selectedSourceLocation, _selectedSubstrate.UniqueKey, out _selectedSourceSubstrate))
                {
                    lblSelectedSource.Text = _selectedSourceSubstrate.LocationId;
                    _activateMoveSource = false;
                    EnablelblSelectedDestination(true);
                }
            }
        }
        private void UpdateDestinationSubstrateInfo(bool needClear = false)
        {
            if (needClear)
            {
                lblSelectedDestination.Text = null;
                _selectedDestinationLocation = null;
                _selectedDestinationSubstrate = null;
                _activateMoveDestination = false;
            }
            else
            {
                if (_selectedSourceSubstrate == null)
                    return;

                if (_selectedSubstrate == null)
                {
                    if (false == LocationServer.GetLocationById(_selectedLocationName, out _selectedDestinationLocation))
                    {
                        _selectedDestinationLocation = null;
                        return;
                    }

                    if (_selectedDestinationLocation is LoadPortLocation)
                    {
                        var Location = _selectedDestinationLocation as LoadPortLocation;
                        if (false == _selectedSourceSubstrate.SourcePortId.Equals(Location.PortId))
                        {
                            _selectedDestinationLocation = null;
                            return;
                        }
                    }

                    if (_selectedDestinationLocation is RobotLocation)
                    {
                        // Arm To Arm은 사이즈가 달라서 불가능
                        if (_selectedSourceLocation is RobotLocation)
                        {
                            _selectedDestinationLocation = null;
                            return;
                        }

                        var Location = _selectedDestinationLocation as RobotLocation;
                        RobotArmTypes armType = RobotArmTypes.All;
                        armType = Location.Arm;
                        var subSizeString = _selectedSourceSubstrate.GetAttribute(PWA500SubstrateAttributes.SubstrateSize);
                        if (false == Enum.TryParse(subSizeString, out SubstrateSize substrateSize))
                            return;

                        if (false == CheckSubstrateTypeByRobotArmType(RobotIndex, substrateSize, armType))
                        {
                            _selectedDestinationLocation = null;
                            return;
                        }
                    }

                    if (false == ShowMessagebySelectedDestinationLocation(_selectedDestinationLocation))
                    {
                        _activateMoveDestination = false;
                        return;
                    }

                    lblSelectedDestination.Text = _selectedDestinationLocation.Id;
                    _activateMoveDestination = false;
                    EnableApplyForMove();
                }
                else
                {
                    if (_selectedSourceLocation.Id.Equals(_selectedLocationName))
                        return;

                    if (false == _selectedSourceSubstrate.SourcePortId.Equals(_selectedSubstrate.SourcePortId))
                        return;

                    if (LocationServer.GetLocationById(_selectedLocationName, out _selectedDestinationLocation))
                    {
                        if (false == ShowMessagebySelectedDestinationLocation(_selectedDestinationLocation))
                        {
                            _selectedDestinationLocation = null;
                            return;
                        }
                        if (_substrateManager.GetSubstrateByLocationAndKey(_selectedDestinationLocation, _selectedSubstrate.UniqueKey, out _selectedDestinationSubstrate))
                        {
                            lblSelectedDestination.Text = _selectedDestinationSubstrate.LocationId;
                            _activateMoveDestination = false;
                            EnableApplyForMove();
                        }
                        else
                        {
                            _selectedDestinationSubstrate = null;
                        }
                    }
                    else
                    {
                        _selectedDestinationLocation = null;
                    }
                }
            }
        }
        private bool ShowMessagebySelectedSourceLocation(Location selectedLocation)
        {
            //Location location = new Location("");
            if (selectedLocation is LoadPortLocation)
            {
                var location = selectedLocation as LoadPortLocation;
                if (false == _messageBox.ShowMessage(string.Format("이동 또는 교환하려고 하는 자재의 위치를 Port ID : {0}, Slot : {1}으로 선택할까요?", location.PortId, location.Slot)))
                    return false;
            }
            if (selectedLocation is RobotLocation)
            {
                var location = selectedLocation as RobotLocation;
                if (false == _messageBox.ShowMessage(string.Format("이동 또는 교환하려고 하는 자재의 위치를 {0}으로 선택할까요?", location.Arm)))
                    return false;
            }
            if (selectedLocation is ProcessModuleLocation)
            {
                var location = selectedLocation as ProcessModuleLocation;
                string pmLocationName = location.Id.StartsWith("PM1.") ? location.Id.Substring(4) : location.Id;
                if (false == _messageBox.ShowMessage(string.Format("이동 또는 교환하려고 하는 자재의 위치를 Process Module의 {0}으로 선택할까요?", pmLocationName)))
                    return false;
            }
            return true;
        }
        private bool ShowMessagebySelectedDestinationLocation(Location selectedLocation)
        {
            //Location location = new Location("");
            if (null == _selectedDestinationSubstrate)
            {
                if (selectedLocation is LoadPortLocation)
                {
                    var location = selectedLocation as LoadPortLocation;
                    if (false == _messageBox.ShowMessage(string.Format("선택한 자재를 이동할 자재의 위치를 Port ID : {0}, Slot : {1}으로 선택할까요?", location.PortId, location.Slot)))
                        return false;
                }
                if (selectedLocation is RobotLocation)
                {
                    var location = selectedLocation as RobotLocation;
                    if (false == _messageBox.ShowMessage(string.Format("선택한 자재를 이동할 자재의 위치를 {0}으로 선택할까요?", location.Arm)))
                        return false;
                }
                if (selectedLocation is ProcessModuleLocation)
                {
                    var location = selectedLocation as ProcessModuleLocation;
                    string pmLocationName = location.Id.StartsWith("PM1.") ? location.Id.Substring(4) : location.Id;
                    if (false == _messageBox.ShowMessage(string.Format("선택한 자재를 이동할 자재의 위치를 Process Module의 {0}으로 선택할까요?", pmLocationName)))
                        return false;
                }

                return true;
            }
            else
            {
                if (selectedLocation is LoadPortLocation)
                {
                    var location = selectedLocation as LoadPortLocation;
                    if (false == _messageBox.ShowMessage(string.Format("선택한 자재를 교환할 자재의 위치를 Port ID : {0}, Slot : {1}으로 선택할까요?", location.PortId, location.Slot)))
                        return false;
                }
                if (selectedLocation is RobotLocation)
                {
                    var location = selectedLocation as RobotLocation;
                    if (false == _messageBox.ShowMessage(string.Format("선택한 자재를 교환할 자재의 위치를 {0}으로 선택할까요?", location.Arm)))
                        return false;
                }
                if (selectedLocation is ProcessModuleLocation)
                {
                    var location = selectedLocation as ProcessModuleLocation;
                    string pmLocationName = location.Id.StartsWith("PM1.") ? location.Id.Substring(4) : location.Id;
                    if (false == _messageBox.ShowMessage(string.Format("선택한 자재를 교환할 자재의 위치를 Process Module의 {0}으로 선택할까요?", pmLocationName)))
                        return false;
                }
                return true;
            }
        }
        private void UpdateSelectedSubstrateLableColor()
        {
            lblSelectedSource.BackGroundColor = _activateMoveSource ? LableActivateColor : LableDefaultColor;

            lblSelectedDestination.BackGroundColor = _activateMoveDestination ? LableActivateColor : LableDefaultColor;
        }
        private void EnablelblSelectedSource(bool isEnabled)
        {
            lblSelectedSource.Enabled = isEnabled;
        }
        private void EnablelblSelectedDestination(bool isEnabled)
        {
            lblSelectedDestination.Enabled = isEnabled;
        }
        private void EnableEditButtons()
        {
            bool enabled = IsEquipmentIdleOrPause();
            if (false == enabled || string.IsNullOrEmpty(_selectedLocationName) || true == _isSubstrateMovePrepared || true == _isSubstrateMoveActivated)
            {
                btnCreate.Enabled = false;
                btnEdit.Enabled = false;
                //btnChangePortInfo.Enabled = false;
                btnDisable.Enabled = false;
                //btnReplaceFromPM.Enabled = false;
                //btnMove.Enabled = false;
                btnRemove.Enabled = false;
            }
            else
            {
                bool noSubstrate = _selectedSubstrate == null;
                btnCreate.Enabled = noSubstrate;
                btnEdit.Enabled = false == noSubstrate;
                //btnChangePortInfo.Enabled = false == noSubstrate;
                btnDisable.Enabled = false == noSubstrate;
                //btnReplaceFromPM.Enabled = false == noSubstrate;
                //btnMove.Enabled = false == noSubstrate;
                btnRemove.Enabled = false == noSubstrate;
            }
        }
        #endregion </Display>

        #region <ETC>
        private void InitInfo()
        {
        }
        private bool IsEquipmentIdleOrPause()
        {
            return _equipmentState.GetState().Equals(EQUIPMENT_STATE.IDLE) || _equipmentState.GetState().Equals(EQUIPMENT_STATE.PAUSE);
        }
        private void UpdateLoadPortInfo(int lpIndex, int slot)
        {
            int portId = _loadPortManager.GetLoadPortPortId(lpIndex);
            if (false == _carrierServer.HasCarrier(portId))
                return;

            if (LocationServer.GetLoadPortLocation(portId, slot, out var location))
            {
                _selectedLocationName = location.Id;
                _substrateManager.GetSubstrateByLocationAndKey(location, string.Empty, out _selectedSubstrate);
            }           
        }
        private void InitializeMove()
        {
            _isSubstrateMoveInitialized = true;
            _isSubstrateMovePrepared = false;
            _isSubstrateMoveActivated = false;

            UpdateMoveSubstrateInfo(true);
            EnablelblSelectedSource(false);
            EnablelblSelectedDestination(false);

            btnMove.Text = "MOVE";
            btnMove.GradientFirstColor = MoveButtonFirstColor;
            btnMove.GradientSecondColor = MoveButtonSecondColor;

            _activateMoveSource = false;
            _activateMoveDestination = false;
        }
        private void StartMove()
        {
            _isSubstrateMoveInitialized = false;
            _isSubstrateMovePrepared = true;
            _isSubstrateMoveActivated = false;

            EnablelblSelectedSource(true);
        }
        private void EnableApplyForMove()
        {
            _isSubstrateMoveInitialized = false;
            _isSubstrateMovePrepared = false;
            _isSubstrateMoveActivated = true;

            btnMove.Text = "APPLY";
            btnMove.GradientFirstColor = ApplyButtonFirstColor;
            btnMove.GradientSecondColor = ApplyButtonSecondColor;
        }
        //private bool GetSubstrateTypeByDestinationPortID(int portId, ref SubstrateType substrateType)
        //{
        //    switch (portId)
        //    {
        //        case 1:
        //            substrateType = SubstrateType.Sort_12;
        //            return true;
        //        case 2:
        //            substrateType = SubstrateType.Core_12;
        //            return true;
        //        case 3:
        //        case 4:
        //            substrateType = SubstrateType.Core_8;
        //            return true;
        //        default:
        //            return false;
        //    }
        //}
        private bool CheckSubstrateTypeByRobotArmType(int robotIndex, SubstrateSize substrateSize, RobotArmTypes availableArm)
        {
            switch (substrateSize)
            {
                case SubstrateSize.Inch_8:
                    {
                        if (availableArm == RobotArmTypes.LowerArm)
                            return true;
                    }
                    break;
                case SubstrateSize.Inch_12:
                    {
                        if (availableArm == RobotArmTypes.UpperArm)
                            return true;
                    }
                    break;
                default:
                    break;
            }
            
            return false;
            //switch (substrateType)
            //{
            //    case SubstrateType.Core_8:
            //        {
            //            switch (availableArm)
            //            {
            //                case RobotArmTypes.LowerArm:
            //                    return true;
            //                case RobotArmTypes.UpperArm:
            //                default:
            //                    return false;
            //            }
            //        }
            //    case SubstrateType.Core_12:
            //    case SubstrateType.Sort_12:
            //        {
            //            switch (availableArm)
            //            {
            //                case RobotArmTypes.UpperArm:
            //                    return true;
            //                case RobotArmTypes.LowerArm:
            //                default:
            //                    return false;
            //            }
            //        }
            //    default:
            //        return false;
            //}
        }

        private bool GetSubstrateByName(string name, out Substrate s)
        {
            s = null;

            var pm = _processGroup.GetProcessModuleName(ProcessModuleIndex);
            List<Substrate> substrates = new List<Substrate>();
            if (false == _substrateManager.GetSubstratesAtProcessModule(pm, ref substrates) || substrates.Count <= 0)
                return false;

            foreach (var item in substrates)
            {
                if (string.Equals(item.Name, name, StringComparison.OrdinalIgnoreCase))
                {
                    s = item;
                    return true;
                }
            }

            return false;
        }
        #endregion </ETC>

        #endregion </Methods>

    }
}