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
using FrameOfSystem3.Recipe;
using FrameOfSystem3.SECSGEM;
using FrameOfSystem3.SECSGEM.Scenario;
using Define.DefineEnumProject.Task;
using FrameOfSystem3.Views.Functional;
using FrameOfSystem3.Views.MapManager;
using FrameOfSystem3.Views.Operation.SubPanelSummary.LoadPortSummary;

using EFEM.Modules;
using EFEM.Defines.Common;
using EFEM.Defines.AtmRobot;
using EFEM.MaterialTracking;
using EFEM.ActionScheduler;
using EFEM.CustomizedByProcessType.PWA500W;
using EFEM.CustomizedByProcessType.PWA500Common;

namespace EFEM.CustomizedByProcessType.UserInterface.OperationMainManual.PWA500W
{
    public enum PanelMode
    {
        Load,
        Unload,
        LoadPort,
        Editor
    }
    public partial class MainDisplaySubPanelManualOperationControl500W : UserControlForMainView.CustomView
    {
        #region <Constructors>
        public MainDisplaySubPanelManualOperationControl500W()
        {
            InitializeComponent();

            //this.Tag = name;

            _equipmentState = EquipmentState.GetInstance();
            _taskOperator = TaskOperator.GetInstance();
            _loadPortManager = LoadPortManager.Instance;
            _carrierServer = CarrierManagementServer.Instance;
            _processGroup = ProcessModuleGroup.Instance;
            _robotManager = AtmRobotManager.Instance;
            _robotSchedulerManager = RobotActionSchedulerManager.Instance;
            _substrateManager = SubstrateManager.Instance;
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

            _temporaryList = new List<Substrate>();

            _robotStateInformation = new RobotStateInformation();

            RobotArmControls = new ConcurrentDictionary<RobotArmTypes, Sys3Controls.Sys3Label>();
            RobotArmControls.TryAdd(RobotArmTypes.UpperArm, lblUpperArmSubstrateInfo);
            RobotArmControls.TryAdd(RobotArmTypes.LowerArm, lblLowerArmSubstrateInfo);

            _substratesInArm = new Dictionary<RobotArmTypes, Substrate>();

            LoadPortNames = new ConcurrentDictionary<int, Sys3Controls.Sys3Label>();
            LoadPortNames[0] = lblLoadPort1;
            LoadPortNames[1] = lblLoadPort2;
            LoadPortNames[2] = lblLoadPort3;
            LoadPortNames[3] = lblLoadPort4;

            InitInfo();

            ProcessModuleName = _processGroup.GetProcessModuleName(ProcessModuleIndex);

            this.Dock = DockStyle.Fill;
        }
        #endregion </Constructors>

        #region <Fields>

        #region <Instances>
        private static TaskOperator _taskOperator = null;
        private static LoadPortManager _loadPortManager = null;
        private static AtmRobotManager _robotManager = null;
        private static CarrierManagementServer _carrierServer = null;
        private static SubstrateManager _substrateManager = null;
        private static ProcessModuleGroup _processGroup = null;
        private static EquipmentState _equipmentState;
        private static RobotActionSchedulerManager _robotSchedulerManager = null;
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
        private int _selectedLoadPortIndex = -1;
        private int _selectedLoadPortSlot = -1;
        private LoadPortLocation _selectedLocation = null;
        //private int _selectedCoreLoadPortIndex = -1;
        //private int _selectedCoreLoadPortSlot = -1;
        ////private Substrate _selectedCoreSubstrate = null;

        //private int _selectedEmptyTapeLoadPortIndex = -1;
        //private int _selectedEmptyTapeLoadPortSlot = -1;
        ////private Substrate _selectedEmptyTapeSubstrate = null;

        //private int _selectedBinLoadPortIndex = -1;
        //private int _selectedBinLoadPortSlot = -1;
        ////private Substrate _selectedBinSubstrate = null;

        private readonly Dictionary<int, Panel> LoadPortPanels = null;
        private readonly Dictionary<int, SummaryLoadPortState_Slot> LoadPortSlots = null;

        private PanelMode _panelMode;
        #endregion </Substrate from LoadPort>

        #region <Substrate in Process Module>
        private List<Substrate> _temporaryList = null;
        #endregion <Substrate in Process Module>

        #region <Substrates In Robot>
        private RobotStateInformation _robotStateInformation = null;
        private readonly ConcurrentDictionary<RobotArmTypes, Sys3Controls.Sys3Label> RobotArmControls = null;
        private Dictionary<RobotArmTypes, Substrate> _substratesInArm = null;
        #endregion </Substrates In Robot>

        #region <ETC>
        private bool _executing = false;

        private readonly ConcurrentDictionary<int, Sys3Controls.Sys3Label> LoadPortNames = null;
        private readonly string ProcessModuleName = string.Empty;
        #endregion </ETC>

        #endregion </Fields>

        #region <Methods>

        #region <Override Methods>
        protected override void ProcessWhenActivation()
        {
            foreach (var item in LoadPortSlots)
            {
                item.Value.ActivateView();
            }

            UpdateSelectedSubstrate();
            DisplayLoadPortNames();
            base.ProcessWhenActivation();
        }
        public override void CallFunctionByTimer()
        {
            if (FrameOfSystem3.Task.TaskOperator.GetInstance().IsExiting)
                return;

            btnStopAction.Enabled = _executing;

            SetControlEnabled();

            foreach (var item in LoadPortSlots)
            {
                item.Value.CallFunctionByTimer();
            }

            //UpdateSelectedSubstrate();
            DisplayModuleInfo();
            DisplayRobotInfo();

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

            UpdateSelectedSubstrate();
        }
        // 2024.09.03. jhlim [DEL] 아래 기능은 폐기한다.
        //private async void BtnActionClicked(object sender, EventArgs e)
        //{
        //    if (false == IsEquipmentIdle())
        //        return;

        //    SubstrateTypeForControl substrateType;

        //    switch (_panelMode)
        //    {
        //        case PanelMode.Load:
        //            {
        //                if (sender.Equals(btnLoadCoreWafer))
        //                {
        //                    substrateType = SubstrateTypeForControl.Core;
        //                }
        //                else if (sender.Equals(btnLoadEmptyWafer))
        //                {
        //                    substrateType = SubstrateTypeForControl.EmptyTape;
        //                }
        //                //else if (sender.Equals(btnLoadWaferAll))
        //                //{
        //                //    substrateType = SubstrateTypeForControl.All;
        //                //}
        //                else
        //                    return;
        //            }
        //            break;
        //        case PanelMode.Unload:
        //            {
        //                if (sender.Equals(btnUnloadCoreWafer))
        //                {
        //                    substrateType = SubstrateTypeForControl.Core;
        //                }
        //                else if (sender.Equals(btnUnloadBinWafer))
        //                {
        //                    substrateType = SubstrateTypeForControl.Bin;
        //                }
        //                //else if (sender.Equals(btnUnloadWaferAll))
        //                //{
        //                //    substrateType = SubstrateTypeForControl.All;
        //                //}
        //                else
        //                    return;
        //            }
        //            break;
        //        default:
        //            return;
        //    }

        //    EnableControlsDuringExecutingAction(false);

        //    var waitResponse = System.Threading.Tasks.Task.Run(() => ExecuteAction(substrateType));
        //    var result = await waitResponse;

        //    EnableControlsDuringExecutingAction(true);
        //}
        // 2024.09.03. jhlim [END]
        private async void BtnSingleActionClicked(object sender, EventArgs e)
        {
            if (false == IsEquipmentIdle())
                return;

            if (!(sender is Sys3Controls.Sys3button btn))
                return;

            bool pickAction;
            SubstrateType substrateType = SubstrateType.Core;
            SubstrateSize substrateSize = SubstrateSize.Inch_8;
            switch (_panelMode)
            {
                case PanelMode.Load:
                    {
                        if (sender.Equals(btnPickCoreWafer))
                        {
                            pickAction = true;
                            // TODO : 임시 -> Location으로 자재 분류 확인 필요
                            if (_selectedLoadPortIndex > 0 &&
                                _functionsForPWA500.GetSubstrateSpecByLoadPortIndex(_selectedLoadPortIndex,
                                    ref substrateType,
                                    ref substrateSize))
                            {
                            }
                        }
                        else if (sender.Equals(btnPickBinWafer))
                        {
                            pickAction = true;
                            substrateType = SubstrateType.Bin1;
                            substrateSize = SubstrateSize.Inch_12;
                        }
                        else if (sender.Equals(btnPlaceCoreWafer))
                        {
                            pickAction = false;
                            if (_selectedLoadPortIndex > 0 &&
                                _functionsForPWA500.GetSubstrateSpecByLoadPortIndex(_selectedLoadPortIndex,
                                    ref substrateType,
                                    ref substrateSize))
                            {
                            }
                        }
                        else if (sender.Equals(btnPlaceBinWafer))
                        {
                            pickAction = false;
                            substrateType = SubstrateType.Bin1;
                            substrateSize = SubstrateSize.Inch_12;
                        }
                        else
                            return;
                    }
                    break;
                case PanelMode.Unload:
                    {
                        if (sender.Equals(btnPickCoreWafer))
                        {
                            pickAction = true;
                            if (_selectedLoadPortIndex > 0 &&
                                _functionsForPWA500.GetSubstrateSpecByLoadPortIndex(_selectedLoadPortIndex,
                                    ref substrateType,
                                    ref substrateSize))
                            {
                            }
                        }
                        else if (sender.Equals(btnPickBinWafer))
                        {
                            pickAction = true;
                            substrateType = SubstrateType.Bin1;
                            substrateSize = SubstrateSize.Inch_12;
                        }
                        else if (sender.Equals(btnPlaceCoreWafer))
                        {
                            pickAction = false;
                            // TODO : 임시 -> Location으로 자재 분류 확인 필요
                            if (_selectedLoadPortIndex > 0 &&
                                _functionsForPWA500.GetSubstrateSpecByLoadPortIndex(_selectedLoadPortIndex,
                                    ref substrateType,
                                    ref substrateSize))
                            {
                            }
                        }
                        else if (sender.Equals(btnPlaceBinWafer))
                        {
                            pickAction = false;
                            substrateType = SubstrateType.Bin1;
                            substrateSize = SubstrateSize.Inch_12;
                        }
                        else
                            return;
                    }
                    break;
                default:
                    return;
            }

            if (false == ShowMessageBoxBeforeManualAction(btn.Text))
                return;

            EnableControlsDuringExecutingAction(false);

            var waitResponse = System.Threading.Tasks.Task.Run(() => ExecuteSingleAction(substrateType, substrateSize, pickAction));
            var result = await waitResponse;

            EnableControlsDuringExecutingAction(true);
        }
        private void BtnStopActionClicked(object sender, EventArgs e)
        {
            _executing = false;
            _taskOperator.SetOperation(RunningMain_.OPERATION_EQUIPMENT.STOP);
        }
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
        private void DisplayModuleInfo()
        {
            if (false == _substrateManager.GetSubstratesAtProcessModule(ProcessModuleName, ref _temporaryList))
                return;

            bool needRedraw = false;
            if (_temporaryList.Count != gvSubstrateList.Rows.Count)
            {
                gvSubstrateList.Rows.Clear();

                needRedraw = true;
            }

            for (int i = 0; i < _temporaryList.Count; ++i)
            {
                if (needRedraw || gvSubstrateList[0, i].Value.ToString() != _temporaryList[i].Name)
                {
                    if (needRedraw)
                    {
                        gvSubstrateList.Rows.Add();
                    }

                    gvSubstrateList[0, i].Value = _temporaryList[i].Name;
                }
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
                    RobotArmControls[item.Key].Text = "";
                }
            }
        }
        private void SetControlEnabled()
        {
            if (_executing)
                return;

            bool enabled = IsEquipmentIdle();

            // 2024.09.03. jhlim [DEL] 아래 기능은 폐기한다.
            //switch (_panelMode)
            //{
            //    case PanelMode.Load:
            //        if (enabled)
            //        {
            //            btnLoadCoreWafer.Enabled = IsSubstrateValid(SubstrateTypeForControl.Core);
            //            btnLoadEmptyWafer.Enabled = IsSubstrateValid(SubstrateTypeForControl.EmptyTape);
            //            //btnLoadWaferAll.Enabled = IsSubstrateValid(SubstrateTypeForControl.All);
            //        }
            //        else
            //        {
            //            btnLoadCoreWafer.Enabled = enabled;
            //            btnLoadEmptyWafer.Enabled = enabled;
            //            //btnLoadWaferAll.Enabled = enabled;
            //        }
            //        btnUnloadCoreWafer.Enabled = false;
            //        btnUnloadBinWafer.Enabled = false;

            //        btnUnloadWaferAll.Enabled = false;
            //        break;
            //    case PanelMode.Unload:
            //        if (enabled)
            //        {
            //            btnUnloadCoreWafer.Enabled = IsSubstrateValid(SubstrateTypeForControl.Core);
            //            btnUnloadBinWafer.Enabled = IsSubstrateValid(SubstrateTypeForControl.Bin);
            //            //btnUnloadWaferAll.Enabled = IsSubstrateValid(SubstrateTypeForControl.All);
            //        }
            //        else
            //        {
            //            btnUnloadCoreWafer.Enabled = enabled;
            //            btnUnloadBinWafer.Enabled = enabled;
            //            //btnUnloadWaferAll.Enabled = enabled;
            //        }
            //        btnLoadCoreWafer.Enabled = false;
            //        btnLoadEmptyWafer.Enabled = false;
            //        //btnLoadWaferAll.Enabled = false;
            //        break;
            //    default:
            //        break;
            //}
            // 2024.09.03. jhlim [END]

            if (enabled)
            {
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
                //btnPickCoreWafer.Enabled = enabled;
                //btnPickBinWafer.Enabled = enabled;

                //btnPlaceCoreWafer.Enabled = enabled;
                //btnPlaceBinWafer.Enabled = enabled;
            }

            if (enabled)
            {
                btnPickCoreWafer.Enabled = IsSubstrateValidToPick(_panelMode, SubstrateType.Core);
                btnPickBinWafer.Enabled = IsSubstrateValidToPick(_panelMode, SubstrateType.Bin1) || IsSubstrateValidToPick(_panelMode, SubstrateType.Empty);
                btnPlaceCoreWafer.Enabled = IsSubstrateValidToPlace(_panelMode, SubstrateType.Core);
                btnPlaceBinWafer.Enabled = IsSubstrateValidToPlace(_panelMode, SubstrateType.Bin1) || IsSubstrateValidToPlace(_panelMode, SubstrateType.Empty);
            }
            else
            {
                btnPickCoreWafer.Enabled = enabled;
                btnPickBinWafer.Enabled = enabled;
                btnPlaceCoreWafer.Enabled = enabled;
                btnPlaceBinWafer.Enabled = enabled;
            }
        }
        #endregion </Display>

        #region <ETC>
        public void ChangePanelMode(PanelMode panelMode)
        {
            _panelMode = panelMode;

            InitInfo();

            UpdateSelectedSubstrate();
            //SetControlEnabled(false);
        }
        private void InitInfo()
        {
            _selectedLoadPortIndex = -1;
            _selectedLoadPortSlot = -1;

            //_selectedCoreLoadPortIndex = -1;
            //_selectedEmptyTapeLoadPortIndex = -1;
            //_selectedBinLoadPortIndex = -1;

            //_selectedCoreLoadPortSlot = -1;
            //_selectedEmptyTapeLoadPortSlot = -1;
            //_selectedBinLoadPortSlot = -1;

            switch (_panelMode)
            {
                case PanelMode.Load:
                    btnPickCoreWafer.Text = "PICK CORE WAFER FROM LP";
                    btnPickBinWafer.Text = "PICK EMPTY WAFER FROM LP";
                    btnPlaceCoreWafer.Text = "PLACE CORE WAFER TO PM";
                    btnPlaceBinWafer.Text = "PLACE EMPTY WAFER TO PM";
                    break;
                case PanelMode.Unload:
                    btnPickCoreWafer.Text = "PICK CORE WAFER FROM PM";
                    btnPickBinWafer.Text = "PICK EMPTY WAFER FROM PM";
                    btnPlaceCoreWafer.Text = "PLACE CORE WAFER TO LP";
                    btnPlaceBinWafer.Text = "PLACE EMPTY WAFER TO LP";
                    break;
                default:
                    break;
            }
            btnPickCoreWafer.Invalidate();
            btnPickBinWafer.Invalidate();
            btnPlaceCoreWafer.Invalidate();
            btnPlaceBinWafer.Invalidate();
        }
        private bool IsSelectedLoadPortValid(SubstrateType substrateType)
        {
            if (_selectedLoadPortIndex < 0 || _selectedLoadPortSlot < 0)
                return false;

            switch (substrateType)
            {
                case SubstrateType.Core:
                    {
                        if (_selectedLoadPortIndex == (int)LoadPortType.Core_8_1 ||
                            _selectedLoadPortIndex == (int)LoadPortType.Core_8_2 ||
                            _selectedLoadPortIndex == (int)LoadPortType.Core_12)
                        {
                            return true;
                        }
                    }
                    break;
                case SubstrateType.Bin1:
                    {
                        if (_selectedLoadPortIndex == (int)LoadPortType.Sort_12)
                        {
                            return true;
                        }
                    }
                    break;
                default:
                    break;
            }

            return false;
        }
        private bool HasSubstrateAtLoadPort()
        {
            int portId = _loadPortManager.GetLoadPortPortId(_selectedLoadPortIndex);
            return _substrateManager.HasSubstrateAtLoadPort(portId, _selectedLoadPortSlot);
        }
        private bool HasSubstrateAtArms(SubstrateType substrateType)
        {
            //_robotStateInformation = _robotManager.GetStateInformation(RobotIndex);
            var robotName = _robotManager.GetRobotName(RobotIndex);
            _substrateManager.GetSubstratesAtRobotAll(robotName, ref _substratesInArm);
            bool hasSubstrateAtArm = false;
            foreach (var item in _substratesInArm)
            {
                if (item.Value == null)
                    continue;

                //Dictionary<string, string> pairs = item.Value.GetAttributesAll();
                string subType = item.Value.GetAttribute(PWA500SubstrateAttributes.SubstrateType);
                if (false == Enum.TryParse(subType, out SubstrateType typeofSubstrate))
                    continue;

                if (typeofSubstrate.Equals(substrateType))
                    hasSubstrateAtArm = true;
            }

            return hasSubstrateAtArm;
        }
        private bool IsSubstrateValidToPick(PanelMode panelMode, SubstrateType substrateType)
        {
            if (HasSubstrateAtArms(substrateType))
                return false;

            if (panelMode.Equals(PanelMode.Load))
            {
                if (false == IsSelectedLoadPortValid(substrateType))
                    return false;

                return HasSubstrateAtLoadPort();
            }
            else
            {
                return true;
            }
        }
        private bool IsSubstrateValidToPlace(PanelMode panelMode, SubstrateType substrateType)
        {
            if (panelMode.Equals(PanelMode.Load))
            {
                return HasSubstrateAtArms(substrateType);
            }
            else
            {
                if (false == IsSelectedLoadPortValid(substrateType))
                    return false;

                return (HasSubstrateAtArms(substrateType) && false == HasSubstrateAtLoadPort());
            }
        }
        private bool SetManualPickWorkingInfo(SubstrateType substrateType, SubstrateSize substrateSize, bool pickAction)
        {
            List<RobotArmTypes> arms = new List<RobotArmTypes>();

            if (false == _robotManager.GetAvailableArm(RobotIndex, pickAction, ref arms))
                return false;

            RobotArmTypes armToWork;
            switch (substrateSize)
            {
                case SubstrateSize.Inch_8:
                    {
                        if (false == arms.Contains(RobotArmTypes.LowerArm))
                            return false;

                        armToWork = RobotArmTypes.LowerArm;
                    }
                    break;

                case SubstrateSize.Inch_12:
                    {
                        if (false == arms.Contains(RobotArmTypes.UpperArm))
                            return false;

                        armToWork = RobotArmTypes.UpperArm;
                    }
                    break;

                default:
                    return false;
            }
            
            switch (_panelMode)
            {
                case PanelMode.Load:
                    {
                        if (substrateType.Equals(SubstrateType.Core))
                        {
                            if (pickAction)
                            {
                                
                                string locationName = _loadPortManager.GetLoadPortName(_selectedLoadPortIndex);

                                int portId = _loadPortManager.GetLoadPortPortId(_selectedLoadPortIndex);
                                string key = _substrateManager.GetSubstrateKeyAtLoadPort(portId, _selectedLoadPortSlot);
                                if (string.IsNullOrEmpty(key))
                                    return false;

                                if (false == LocationServer.GetLoadPortLocation(portId, _selectedLoadPortSlot, out var targetLocationLp))
                                    return false;

                                _robotSchedulerManager.SetManualWorkingInformation(RobotIndex, armToWork, key, targetLocationLp.Id, ModuleType.LoadPort, false);
                            }
                            else
                            {
                                string robotName = _robotManager.GetRobotName(RobotIndex);
                                _robotStateInformation = _robotManager.GetStateInformation(RobotIndex);
                                if (false == _robotManager.GetSubstrate(robotName, armToWork, out var substrate))
                                    return false;

                                string[] destinationName = _processGroup.GetEntrywayNames(ProcessModuleIndex);

                                _robotSchedulerManager.SetManualWorkingInformation(RobotIndex, armToWork, substrate.UniqueKey, destinationName[(int)ProcessModuleEntryWays.Core_8_In], ModuleType.ProcessModule, true);
                            }
                            return true;
                        }

                        //if (substrateType.Equals(SubstrateType.Core_12))
                        //{
                        //    if (pickAction)
                        //    {

                        //        string locationName = _loadPortManager.GetLoadPortName(_selectedLoadPortIndex);

                        //        int portId = _loadPortManager.GetLoadPortPortId(_selectedLoadPortIndex);
                        //        string substrateName = _substrateManager.GetSubstrateNameAtLoadPort(portId, _selectedLoadPortSlot);
                        //        if (string.IsNullOrEmpty(substrateName))
                        //            return false;

                        //        LoadPortLocation targetLocationLp = new LoadPortLocation(-1, -1, "");
                        //        if (false == _locationServer.GetLoadPortSlotLocation(portId, _selectedLoadPortSlot, ref targetLocationLp))
                        //            return false;

                        //        _robotSchedulerManager.SetManualWorkingInformation(RobotIndex, armToWork, substrateName, targetLocationLp, false);
                        //    }
                        //    else
                        //    {
                        //        string robotName = _robotManager.GetRobotName(RobotIndex);
                        //        _robotStateInformation = _robotManager.GetStateInformation(RobotIndex);
                        //        Substrate substrate = new Substrate();
                        //        if (false == _robotManager.GetSubstrate(robotName, armToWork, ref substrate))
                        //            return false;

                        //        string[] destinationName = _processGroup.GetProcessModuleLocations(ProcessModuleIndex);

                        //        ProcessModuleLocation targetLocationPm = new ProcessModuleLocation("", "");
                        //        string processModuleName = _processGroup.GetProcessModuleName(ProcessModuleIndex);
                        //        if (false == _locationServer.GetProcessModuleLocation(processModuleName, destinationName[(int)ProcessModuleEntryWays.Core_12_In], ref targetLocationPm))
                        //            return false;

                        //        _robotSchedulerManager.SetManualWorkingInformation(RobotIndex, armToWork, substrate.Name, targetLocationPm, true);
                        //    }
                        //    return true;
                        //}

                        if (false == substrateType.Equals(SubstrateType.Core))
                        {
                            if (pickAction)
                            {
                                string locationName = _loadPortManager.GetLoadPortName(_selectedLoadPortIndex);

                                int portId = _loadPortManager.GetLoadPortPortId(_selectedLoadPortIndex);
                                string key = _substrateManager.GetSubstrateKeyAtLoadPort(portId, _selectedLoadPortSlot);
                                if (string.IsNullOrEmpty(key))
                                    return false;


                                if (false == LocationServer.GetLoadPortLocation(portId, _selectedLoadPortSlot, out var targetLocationLp))
                                    return false;

                                _robotSchedulerManager.SetManualWorkingInformation(RobotIndex, armToWork, key, targetLocationLp.Id, ModuleType.LoadPort, false);
                            }
                            else
                            {
                                var robotName = _robotManager.GetRobotName(RobotIndex);
                                Dictionary<RobotArmTypes, Substrate> substrateInRobot = new Dictionary<RobotArmTypes, Substrate>();
                                if (false == _substrateManager.GetSubstratesAtRobotAll(robotName, ref substrateInRobot))
                                    return false;

                                Substrate substrate = null;
                                RobotArmTypes targetArm = RobotArmTypes.All;
                                foreach (var item in substrateInRobot)
                                {
                                    if (false == _robotManager.GetSubstrate(robotName, item.Key, out substrate))
                                        continue;

                                    targetArm = item.Key;
                                    break;
                                }
                                if (targetArm.Equals(RobotArmTypes.All) || substrate == null)
                                    return false;

                                string[] destinationName = _processGroup.GetEntrywayNames(ProcessModuleIndex);

                                _robotSchedulerManager.SetManualWorkingInformation(RobotIndex, targetArm, substrate.UniqueKey, destinationName[(int)ProcessModuleEntryWays.Sort_12_In], ModuleType.ProcessModule, true);
                            }
                            return true;
                        }
                    }
                    break;
                case PanelMode.Unload:
                    {
                        string substrateInProcessModule = string.Empty;
                        if (substrateType.Equals(SubstrateType.Core))
                        {
                            if (pickAction)
                            {
                                // substrateInProcessModule = string.Empty;// substrate.Name;
                                string[] destinationName = _processGroup.GetEntrywayNames(ProcessModuleIndex);

                                _robotSchedulerManager.SetManualWorkingInformation(RobotIndex, armToWork, string.Empty, destinationName[(int)ProcessModuleEntryWays.Core_8_Out], ModuleType.ProcessModule, false);
                            }
                            else
                            {
                                RobotArmTypes targetArm = RobotArmTypes.All;
                                var robotName = _robotManager.GetRobotName(RobotIndex);
                                _substrateManager.GetSubstratesAtRobotAll(robotName, ref _substratesInArm);
                                    
                                foreach (var item in _substratesInArm)
                                {
                                    if (item.Value == null)
                                        continue;

                                    //Dictionary<string, string> pairs = item.Value.GetAttributesAll();
                                    string subType = item.Value.GetAttribute(PWA500SubstrateAttributes.SubstrateType);
                                    if (subType.Equals(SubstrateType.Core.ToString()))
                                    {
                                        if (item.Key != armToWork)
                                            return false;

                                        targetArm = item.Key;
                                        substrateInProcessModule = item.Value.UniqueKey;
                                        break;
                                    }

                                    //int portId = item.Value.SourcePortId;
                                    //if (portId == (int)LoadPortType.Core_1 + 1 ||
                                    //    portId == (int)LoadPortType.Core_2 + 1)
                                    //{
                                    //    substrateInProcessModule = item.Value.Name;
                                    //}
                                }
                                if (string.IsNullOrEmpty(substrateInProcessModule))
                                    return false;

                                string destinationName = _loadPortManager.GetLoadPortName(_selectedLoadPortIndex);

                                int portId = _loadPortManager.GetLoadPortPortId(_selectedLoadPortIndex);
                                if (false == LocationServer.GetLoadPortLocation(portId, _selectedLoadPortSlot, out var targetLocationLp))
                                    return false;

                                _robotSchedulerManager.SetManualWorkingInformation(RobotIndex, targetArm, substrateInProcessModule, targetLocationLp.Id, ModuleType.LoadPort, false);
                            }
                            return true;
                        }
                        //if (substrateType.Equals(SubstrateType.Core_12))
                        //{
                        //    if (pickAction)
                        //    {
                        //        substrateInProcessModule = string.Empty;// substrate.Name;
                        //        string[] destinationName = _processGroup.GetProcessModuleLocations(ProcessModuleIndex);

                        //        ProcessModuleLocation targetLocationPm = new ProcessModuleLocation("", "");
                        //        string processModuleName = _processGroup.GetProcessModuleName(ProcessModuleIndex);
                        //        if (false == _locationServer.GetProcessModuleLocation(processModuleName, destinationName[(int)ProcessModuleEntryWays.Core_12_Out], ref targetLocationPm))
                        //            return false;

                        //        _robotSchedulerManager.SetManualWorkingInformation(RobotIndex, armToWork, substrateInProcessModule, targetLocationPm, false);
                        //    }
                        //    else
                        //    {
                        //        RobotArmTypes targetArm = RobotArmTypes.All;
                        //        _robotManager.GetSubstrates(RobotIndex, ref _substratesInArm);
                        //        foreach (var item in _substratesInArm)
                        //        {
                        //            if (item.Value == null)
                        //                continue;

                        //            //Dictionary<string, string> pairs = item.Value.GetAttributesAll();
                        //            string subType = item.Value.GetAttribute(PWA500WSubstrateAttributes.SubstrateType);
                        //            if (subType.Equals(SubstrateType.Core_12.ToString()))
                        //            {
                        //                if (item.Key != armToWork)
                        //                    return false;

                        //                targetArm = item.Key;
                        //                substrateInProcessModule = item.Value.Name;
                        //                break;
                        //            }

                        //            //int portId = item.Value.SourcePortId;
                        //            //if (portId == (int)LoadPortType.Core_1 + 1 ||
                        //            //    portId == (int)LoadPortType.Core_2 + 1)
                        //            //{
                        //            //    substrateInProcessModule = item.Value.Name;
                        //            //}
                        //        }
                        //        if (string.IsNullOrEmpty(substrateInProcessModule))
                        //            return false;

                        //        string destinationName = _loadPortManager.GetLoadPortName(_selectedLoadPortIndex);

                        //        int portId = _loadPortManager.GetLoadPortPortId(_selectedLoadPortIndex);
                        //        LoadPortLocation targetLocationLp = new LoadPortLocation(-1, -1, "");
                        //        if (false == _locationServer.GetLoadPortSlotLocation(portId, _selectedLoadPortSlot, ref targetLocationLp))
                        //            return false;

                        //        _robotSchedulerManager.SetManualWorkingInformation(RobotIndex, targetArm, substrateInProcessModule, targetLocationLp, false);
                        //    }
                        //    return true;
                        //}
                        if (false == substrateType.Equals(SubstrateType.Core))
                        {
                            if (pickAction)
                            {
                                // substrateInProcessModule = string.Empty;// substrate.Name;
                                string[] destinationName = _processGroup.GetEntrywayNames(ProcessModuleIndex);

                                _robotSchedulerManager.SetManualWorkingInformation(RobotIndex, armToWork, string.Empty, destinationName[(int)ProcessModuleEntryWays.Sort_12_Out], ModuleType.ProcessModule, false);
                            }
                            else
                            {
                                RobotArmTypes targetArm = RobotArmTypes.All;
                                var robotName = _robotManager.GetRobotName(RobotIndex);
                                _substrateManager.GetSubstratesAtRobotAll(robotName, ref _substratesInArm);
                                foreach (var item in _substratesInArm)
                                {
                                    if (item.Value == null)
                                        continue;

                                    //Dictionary<string, string> pairs = item.Value.GetAttributesAll();
                                    string subType = item.Value.GetAttribute(PWA500SubstrateAttributes.SubstrateType);                                   
                                    if (false == subType.Equals(SubstrateType.Core.ToString()))
                                    {
                                        if (item.Key != armToWork)
                                            return false;

                                        targetArm = item.Key;
                                        substrateInProcessModule = item.Value.UniqueKey;
                                    }
                                }
                                if (string.IsNullOrEmpty(substrateInProcessModule))
                                    return false;

                                string destinationName = _loadPortManager.GetLoadPortName(_selectedLoadPortIndex);
                                int portId = _loadPortManager.GetLoadPortPortId(_selectedLoadPortIndex);
                                if (false == LocationServer.GetLoadPortLocation(portId, _selectedLoadPortSlot, out var targetLocationLp))
                                    return false;

                                _robotSchedulerManager.SetManualWorkingInformation(RobotIndex, targetArm, substrateInProcessModule, targetLocationLp.Id, ModuleType.LoadPort, false);
                            }
                            return true;
                        }
                    }
                    break;
                default:
                    break;
            }          

            return false;
        }
        private bool IsEquipmentIdle()
        {
            return _equipmentState.GetState().Equals(EQUIPMENT_STATE.IDLE);
        }
        private void EnableControlsDuringExecutingAction(bool enabled)
        {
            foreach (var kvp in gvManualAction.Controls)
            {
                if (kvp.Equals(btnStopAction))
                    continue;

                if (!(kvp is Sys3Controls.Sys3button button))
                    continue;

                button.Enabled = enabled;
            }
        }
        private void UpdateSelectedSubstrate()
        {
            string coreLoadPortName = string.Empty;
            string coreSlot = string.Empty;


            if (_selectedLoadPortIndex >= 0 && _selectedLoadPortSlot >= 0)
            {
                int portId = _loadPortManager.GetLoadPortPortId(_selectedLoadPortIndex);
                if (LocationServer.GetLoadPortLocation(portId, _selectedLoadPortSlot, out _selectedLocation))
                {
                    coreLoadPortName = _selectedLocation.LoadPortName;
                    coreSlot = _substrateManager.GetSubstrateNameAtLoadPort(portId, _selectedLoadPortSlot);
                }
            }

            lblSelectedLoadPortInfo.SubText = coreLoadPortName;
            lblSelectedLoadPortInfo.Text = coreSlot;
        }
        private void UpdateLoadPortInfo(int lpIndex, int slot)
        {
            _selectedLoadPortIndex = lpIndex;
            _selectedLoadPortSlot = slot;           
        }
        private bool ExecuteSingleAction(SubstrateType substrateType, SubstrateSize substrateSize, bool pickAction)
        {
            _executing = true;
            List<string> requestedLocations = new List<string>();
            _robotSchedulerManager.InitSchedulers(RobotIndex);
            _robotSchedulerManager.InitManualWorkingInformation(RobotIndex);

            //if (false)
            {
                while (true)
                {
                    if (false == _executing)
                        return false;

                    System.Threading.Thread.Sleep(100);

                    requestedLocations.Clear();
                    if (_panelMode.Equals(PanelMode.Load))
                    {
                        if (false == pickAction)
                        {
                            if (false == _processGroup.IsLoadingRequested(ProcessModuleIndex, ref requestedLocations))
                                continue;

                            //if (false == requestedLocations.Contains(Constants.ProcessModuleCoreInputName) ||
                            //    false == requestedLocations.Contains(Constants.ProcessModuleSortInputName))
                            //    return false;
                        }

                    }
                    else
                    {
                        if (pickAction)
                        {
                            if (false == _processGroup.IsUnloadingRequested(ProcessModuleIndex, ref requestedLocations))
                                continue;

                            //if (false == requestedLocations.Contains(Constants.ProcessModuleCoreOutputName) ||
                            //    false == requestedLocations.Contains(Constants.ProcessModuleSortOutputName))
                            //    return false;
                        }
                    }
                    break;
                }
            }

            if (false == SetManualPickWorkingInfo(substrateType, substrateSize, pickAction))
                return false;

            //switch (substrateType)
            //{
            //    case SubstrateType.Core:
            //        {
            //            if (_panelMode.Equals(PanelMode.Load))
            //            {
            //                SetManualPickWorkingInfo(substrateType, pickAction);
            //                //if (false == IsSubstrateValid(substrateType))
            //                //    break;

            //                //if (false == SetLoadWorkingInfo(substrateType, needPick))
            //                //    break;
            //            }
            //            else
            //            {
            //                SetManualPickWorkingInfo(substrateType, pickAction);
            //                //if (false == IsSubstrateValid(substrateType))
            //                //    break;

            //                //if (false == SetUnloadWorkingInfo(substrateType, requestedLocation, needPick))
            //                //    break;
            //            }
            //        }
            //        break;

            //    case SubstrateType.Core_12:
            //        {
            //            if (_panelMode.Equals(PanelMode.Load))
            //            {
            //                if (false == SetManualPickWorkingInfo(substrateType, pickAction))
            //                    return false;
            //                //if (false == IsSubstrateValid(substrateType))
            //                //    break;

            //                //if (false == SetLoadWorkingInfo(substrateType, needPick))
            //                //    break;
            //            }
            //            else
            //            {
            //                if (false == SetManualPickWorkingInfo(substrateType, pickAction))
            //                    return false;
            //                //if (false == IsSubstrateValid(substrateType))
            //                //    break;

            //                //if (false == SetUnloadWorkingInfo(substrateType, requestedLocation, needPick))
            //                //    break;
            //            }
            //        }
            //        break;

            //    case SubstrateType.Sort_12:
            //        {
            //            if (_panelMode.Equals(PanelMode.Load))
            //            {
            //                if (false == SetManualPickWorkingInfo(substrateType, pickAction))
            //                    return false;
            //                //if (false == IsSubstrateValid(substrateType))
            //                //    break;

            //                //if (false == SetLoadWorkingInfo(substrateType, needPick))
            //                //    break;
            //            }
            //            else
            //            {
            //                if (false == SetManualPickWorkingInfo(substrateType, pickAction))
            //                    return false;
            //                //if (false == IsSubstrateValid(substrateType))
            //                //    break;

            //                //if (false == SetUnloadWorkingInfo(substrateType, requestedLocation, needPick))
            //                //    break;
            //            }
            //        }
            //        break;

            //    default:
            //        return false;
            //}

            List<string> taskName = new List<string>();
            List<string> actionName = new List<string>();
            if (pickAction)
            {
                taskName.Add(EN_TASK_LIST.AtmRobot.ToString());
                actionName.Add(TaskAtmRobot.TASK_ACTION.MANUAL_PICK.ToString());
            }
            else
            {
                taskName.Add(EN_TASK_LIST.AtmRobot.ToString());
                actionName.Add(TaskAtmRobot.TASK_ACTION.MANUAL_PLACE.ToString());
            }
            string[] tasks = taskName.ToArray();
            string[] actions = actionName.ToArray();
            TaskOperator.GetInstance().SetOperation(ref tasks, ref actions);

            _executing = false;
            return true;
        }
        private bool ShowMessageBoxBeforeManualAction(string message)
        {
            var messageBox = Form_MessageBox.GetInstance();
            return messageBox.ShowMessage(message);
        }
        #endregion </ETC>

        #endregion </Methods>
    }
}