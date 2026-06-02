using System;
using System.Collections.Generic;
using System.Collections.Concurrent;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using EquipmentState_;

using FrameOfSystem3.Task;
using FrameOfSystem3.Views;
using Define.DefineConstant;
using Define.DefineEnumProject.Task;
using FrameOfSystem3.Recipe;

using EFEM.Modules;
using EFEM.MaterialTracking;
using EFEM.Defines.LoadPort;
using EFEM.Defines.MaterialTracking;

using FrameOfSystem3.Views.Operation.SubPanelSummary.LoadPortSummary;

namespace FrameOfSystem3.Views.Operation.SubPanelManualOperation.LoadPort
{
    public partial class MainDisplaySubPanelParallelIOE84 : ParameterPanel
    {
        #region <Constructors>
        public MainDisplaySubPanelParallelIOE84()
        {
            InitializeComponent();

            this.Dock = DockStyle.Fill;

            _equipmentState = EquipmentState.GetInstance();
            _taskOperator = TaskOperator.GetInstance();
            _loadPortManager = LoadPortManager.Instance;
            _carrierServer = CarrierManagementServer.Instance;
            _selectionList = Functional.Form_SelectionList.GetInstance();

            var loadingModes = new List<LoadPortLoadingMode>((LoadPortLoadingMode[])Enum.GetValues(typeof(LoadPortLoadingMode)));
            loadingModes.Remove(LoadPortLoadingMode.Unknown);
            LoadPortLoadingModes = new Dictionary<int, string>();

            for (int i = 0; i < loadingModes.Count; ++i)
            {
                if (loadingModes[i].Equals(LoadPortLoadingMode.Unknown))
                    continue;

                int index = LoadPortLoadingModes.Count;
                LoadPortLoadingModes[index] = loadingModes[i].ToString();
            }

            LoadPortAccessingModes = new Dictionary<int, string>();
            var accessingModes = Enum.GetNames(typeof(LoadPortAccessMode));
            for (int i = 0; i < accessingModes.Length; ++i)
            {
                LoadPortAccessingModes[i] = accessingModes[i];
            }

            CarrierAccessStatus = new Dictionary<int, string>();
            var accessStatus = Enum.GetNames(typeof(CarrierAccessStates));
            for (int i = 0; i < accessStatus.Length; ++i)
            {
                CarrierAccessStatus[i] = accessStatus[i];
            }

            _inputControls = new List<Sys3Controls.Sys3LedLabelWithText>
            {
                lblInput1,
                lblInput2,
                lblInput3,
                lblInput4,
                lblInput5,
                lblInput6,
                lblInput7,
                lblInput8,
            };
            _outputControls = new List<Sys3Controls.Sys3LedLabelWithText>
            {
                lblOutput1,
                lblOutput2,
                lblOutput3,
                lblOutput4,
                lblOutput5,
                lblOutput6,
                lblOutput7,
                lblOutput8,
            };

            _inputValues = new Dictionary<int, bool>();
            _outputValues = new Dictionary<int, bool>();
            _inputIndex = new Dictionary<int, int>();
            _outputIndex = new Dictionary<int, int>();
            LockObject = new object();

            UpdateLoadPortStatus(true);
        }
        #endregion </Constructors>

        #region <Fields>
        private readonly string _parentFilePath = string.Empty;
        private readonly string _targetPath = string.Empty;
        private readonly string _pathE84Load = string.Empty;

        #region <Instances>
        private static TaskOperator _taskOperator = null;
        private static LoadPortManager _loadPortManager = null;
        private static CarrierManagementServer _carrierServer = null;
        private static EquipmentState _equipmentState;
        private readonly PanelInterface _panelInstance = new PanelInterface();
        private static Functional.Form_SelectionList _selectionList = null;
        #endregion </Instances>

        #region <LoadPort Controls>
        private Define.DefineEnumProject.AppConfig.EN_PIO_INTERFACE_TYPE _pioInterfaceType;
        private AMHSInformation amhsInformation = null;

        private int _selectedLoadPortIndex = 0;
        private int _selectedLoadPortPortId = 1;

        private LoadPortTransferStates _transferState;
        private CarrierIdVerificationStates _carrierIdState;
        //private CarrierSlotMapVerificationStates _carrierSlotState;
        private CarrierAccessStates? _carrierAccessState;
        private LoadPortLoadingMode _loadingMode;
        private LoadPortAccessMode _accessMode;

        private const string Unknown = "Unknown";
        private const int ParallelIOCount = 8;

        private int _selectedLoadPortLoadingMode = 0;
        private int _selectedLoadPortAccessingMode = 0;
        private int _selectedCarrierAccessStatus = 1;
        private readonly Dictionary<int, string> LoadPortLoadingModes = null;
        private readonly Dictionary<int, string> LoadPortAccessingModes = null;
        private readonly Dictionary<int, string> CarrierAccessStatus = null;

        private AMHSInformation _amhsInformation = null;
        private List<Sys3Controls.Sys3LedLabelWithText> _inputControls = null;
        private List<Sys3Controls.Sys3LedLabelWithText> _outputControls = null;
        private Dictionary<int, int> _inputIndex = null;        // Key : Address    Value : 순번
        private Dictionary<int, bool> _inputValues = null;      // Key : Address    Value : Value
        private Dictionary<int, int> _outputIndex = null;       // Key : Address    Value : 순번
        private Dictionary<int, bool> _outputValues = null;     // Key : Address    Value : Value

        private readonly object LockObject = null;

        private const string ActionNameChangeLoadingMode = "CHANGE_LOADPORT_LOADING_MODE";
        private const string ActionNameChangeAccessMode = "CHANGE_LOADPORT_ACCESSE_MODE";
        private const string ActionNameChangeAccessStatus = "CHANGE_CARRIER_ACCESSE_STATUS";
        #endregion </LoadPort Controls>

        #endregion </Fields>

        #region <Methods>

        #region <Initialize>
        private void GetAMHSInterfaceType()
        {
            if (_loadPortManager.GetAMHSInformation(_selectedLoadPortIndex, ref amhsInformation))
            {
                _pioInterfaceType = amhsInformation.InterfaceType;
            }
        }
        private void InitStatus()
        {
            _transferState = LoadPortTransferStates.Unknown;
            _carrierIdState = CarrierIdVerificationStates.NotRead;
            //_carrierSlotState = CarrierSlotMapVerificationStates.Unknown;
            //_carrierAccessState = CarrierAccessStates.Unknown;
            _loadingMode = LoadPortLoadingMode.Unknown;
            _accessMode = LoadPortAccessMode.Manual;
        }
        private void RefreshParallelIOInformation()
        {
            lock (LockObject)
            {
                if (false == _loadPortManager.GetAMHSInformation(_selectedLoadPortIndex, ref _amhsInformation))
                    return;

                _inputValues.Clear();
                _outputValues.Clear();
                _inputIndex.Clear();
                _outputIndex.Clear();
                for (int i = 0; i < ParallelIOCount; ++i)
                {
                    int inputIndex = _amhsInformation.DigitalInputs[i].Item1;
                    int outputIndex = _amhsInformation.DigitalOutputs[i].Item1;

                    _inputIndex[inputIndex] = i;
                    _outputIndex[outputIndex] = i;

                    _inputValues[inputIndex] = false;
                    _outputValues[outputIndex] = false;

                    string inputName = _amhsInformation.DigitalInputs[i].Item2;
                    _inputControls[i].ActiveMessage = inputName;
                    _inputControls[i].Text = inputName;

                    string outputName = _amhsInformation.DigitalOutputs[i].Item2;
                    _outputControls[i].ActiveMessage = outputName;
                    _outputControls[i].Text = outputName;
                }
            }
        }
        #endregion </Initialize>

        #region <Override Methods>
        protected override void ProcessWhenActivation()
        {
            RefreshParallelIOInformation();

            GetAMHSInterfaceType();

            base.ProcessWhenActivation();
        }
        public override void CallFunctionByTimer()
        {
            UpdateLoadPortHardwareStatus();

            UpdateLoadPortStatus();

            lock (LockObject)
            {
                UpdateParallelSignalValues();

                DisplayParallelSignalControls();
            }

            base.CallFunctionByTimer();
        }
        protected override void ProcessWhenDeactivation()
        {
            base.ProcessWhenDeactivation();
        }
        #endregion </Override Methods>

        #region <UI Events>
        private void BtnSaveParameterClicked(object sender, EventArgs e)
        {
            _panelInstance.ClickParameterSave();
        }

        private void BtnUndoParameterClicked(object sender, EventArgs e)
        {
            _panelInstance.ClickParameterUndo();
        }
        private void LblSelectedLoadPortClicked(object sender, EventArgs e)
        {
            var names = GetLoadPortNames();
            if (names == null || names.Count == 0)
                return;

            if (_selectionList.CreateForm("Select LoadPort", names.Values.ToArray(), names.Keys.ToArray(), _selectedLoadPortIndex))
            {
                _selectionList.GetResult(ref _selectedLoadPortIndex);

                _selectedLoadPortPortId = _loadPortManager.GetLoadPortPortId(_selectedLoadPortIndex);

                DisplayLoadPortNames();
                InitStatus();
                //RefreshLoadPortSlotControl();
                RefreshParallelIOInformation();
            }

        }
        private void BtnPIOOutputClicked(object sender, EventArgs e)
        {
            if (false == IsEquipmentIdle())
                return;

            if (!(sender is Sys3Controls.Sys3LedLabelWithText button))
                return;

            for (int i = 0; i < _outputControls.Count; ++i)
            {
                if (button.Equals(_outputControls[i]))
                {
                    foreach (var item in _outputIndex)
                    {
                        if (item.Value.Equals(i))
                        {
                            _loadPortManager.WriteAMHSOutput(_selectedLoadPortIndex, item.Key, !button.Active);
                            return;
                        }
                    }

                }
            }
        }
        private void BtnAMHSActionClicked(object sender, EventArgs e)
        {
            if (false == IsEquipmentIdle())
                return;

            if (!(sender is Sys3Controls.Sys3button button))
                return;

            string actionName = button.Tag.ToString();
            if (string.IsNullOrEmpty(actionName))
                return;

            if (actionName.Equals("INITIALIZE_AMHS"))
            {
                _loadPortManager.InitializeAMHSSignals(_selectedLoadPortIndex);
            }
            else
            {
                if (_pioInterfaceType.Equals(Define.DefineEnumProject.AppConfig.EN_PIO_INTERFACE_TYPE.E84))
                {
                    var messageBox = Functional.Form_MessageBox.GetInstance();

                    if (false == _loadPortManager.ReadPIOOutput(_selectedLoadPortIndex, (int)E84OutputSignals.EmergencyStop)
                        && false == _loadPortManager.ReadPIOOutput(_selectedLoadPortIndex, (int)E84OutputSignals.HandoffAvailable))
                    {
                        messageBox.ShowMessage(string.Format("The PIO interface is not ready because both [{0}] and [{1}] are currently OFF", E84OutputSignals.EmergencyStop, E84OutputSignals.HandoffAvailable));
                        return;
                    }
                    if (false == _loadPortManager.ReadPIOOutput(_selectedLoadPortIndex, (int)E84OutputSignals.EmergencyStop))
                    {
                        messageBox.ShowMessage(string.Format("The PIO interface is not ready because [{0}] is currently OFF", E84OutputSignals.EmergencyStop));
                        return;
                    }
                    if (false == _loadPortManager.ReadPIOOutput(_selectedLoadPortIndex, (int)E84OutputSignals.HandoffAvailable))
                    {
                        messageBox.ShowMessage(string.Format("The PIO interface is not ready because [{0}] is currently OFF", E84OutputSignals.HandoffAvailable));
                        return;
                    }
                }
                if (Enum.TryParse(actionName, out TaskLoadPort.TASK_ACTION actionToEnum))
                {
                    EN_TASK_LIST taskName = EN_TASK_LIST.LoadPort1 + _selectedLoadPortIndex;
                    if (actionToEnum.Equals(TaskLoadPort.TASK_ACTION.WAIT_FOR_UNLOADING))
                    {
                        string[] taskToExecute = { taskName.ToString() };
                        string[][] actionsToExecute =
                            {
                            new string[] { TaskLoadPort.TASK_ACTION.CARRIER_UNLOADING_BEFORE_AMHS.ToString() },
                            new string[] { TaskLoadPort.TASK_ACTION.WAIT_FOR_UNLOADING.ToString() }
                            };

                        _taskOperator.SetOperation(ref taskToExecute, ref actionsToExecute, 1);
                    }
                    else
                    {
                        string[] taskToExecute = { taskName.ToString() };
                        string[] actionsToExecute = { actionName };
                        _taskOperator.SetOperation(ref taskToExecute, ref actionsToExecute);
                    }
                }
            }
        }
        private void BtnLoadPortCommandClicked(object sender, EventArgs e)
        {
            if (false == IsEquipmentIdle())
                return;

            if (!(sender is Sys3Controls.Sys3button button))
                return;

            string actionName = button.Tag.ToString();

            TaskLoadPort.TASK_ACTION taskAction = TaskLoadPort.TASK_ACTION.INITIALIZE;
            if (actionName.Equals(ActionNameChangeLoadingMode) ||
                actionName.Equals(ActionNameChangeAccessMode) ||
                actionName.Equals(ActionNameChangeAccessStatus))
            {
                if (actionName.Equals(ActionNameChangeLoadingMode))
                {
                    if (false == LoadPortLoadingModes.TryGetValue(_selectedLoadPortLoadingMode, out string modeToString))
                        return;

                    if (false == Enum.TryParse(modeToString, out LoadPortLoadingMode currentMode))
                        return;

                    switch (currentMode)
                    {
                        case LoadPortLoadingMode.Unknown:
                            return;

                    }

                    _taskOperator.TargetLoadingMode = currentMode;
                    taskAction = TaskLoadPort.TASK_ACTION.CHANGE_LOADPORT_LOADING_MODE;
                }
                else if (actionName.Equals(ActionNameChangeAccessMode))
                {
                    if (false == LoadPortAccessingModes.TryGetValue(_selectedLoadPortAccessingMode, out string modeToString))
                        return;

                    if (false == Enum.TryParse(modeToString, out LoadPortAccessMode currentMode))
                        return;

                    switch (currentMode)
                    {
                        case LoadPortAccessMode.Auto:
                            taskAction = TaskLoadPort.TASK_ACTION.CHANGE_LOADPORT_ACCESSE_MODE_TO_AUTO;
                            break;

                        case LoadPortAccessMode.Manual:
                            taskAction = TaskLoadPort.TASK_ACTION.CHANGE_LOADPORT_ACCESSE_MODE_TO_MANUAL;
                            break;

                        default:
                            return;
                    }
                }
                else if (actionName.Equals(ActionNameChangeAccessStatus))
                {
                    if (false == CarrierAccessStatus.TryGetValue(_selectedCarrierAccessStatus, out string modeToString))
                        return;

                    if (false == Enum.TryParse(modeToString, out CarrierAccessStates currentMode))
                        return;

                    switch (currentMode)
                    {
                        case CarrierAccessStates.NotAccessed:
                            taskAction = TaskLoadPort.TASK_ACTION.CHANGE_CARRIER_ACCESS_STATUS_TO_NOT_ACCESSED;
                            break;
                        case CarrierAccessStates.InAccessed:
                            taskAction = TaskLoadPort.TASK_ACTION.CHANGE_CARRIER_ACCESS_STATUS_TO_IN_ACCESSED;
                            break;
                        case CarrierAccessStates.CarrierCompleted:
                            taskAction = TaskLoadPort.TASK_ACTION.CHANGE_CARRIER_ACCESS_STATUS_TO_CARRIER_COMPLETED;
                            break;
                        case CarrierAccessStates.CarrierStopped:
                            taskAction = TaskLoadPort.TASK_ACTION.CHANGE_CARRIER_ACCESS_STATUS_TO_CARRIER_STOPPED;
                            break;
                        default:
                            break;
                    }
                }
            }
            else
            {
                if (false == Enum.TryParse(actionName, out taskAction))
                    return;
            }

            EN_TASK_LIST taskName = EN_TASK_LIST.LoadPort1 + _selectedLoadPortIndex;
            string[] taskToExecute = { taskName.ToString() };
            string[] action = { taskAction.ToString() };

            if (false == ShowMessageBoxBeforeManualAction(taskName.ToString(), button.Text))
                return;

            _taskOperator.SetOperation(ref taskToExecute, ref action);
        }
        #endregion </UI Events>

        #region <Display>
        //private void RefreshLoadPortSlotControl()
        //{
        //    foreach (var item in LoadPortSlots)
        //    {
        //        if (item.Key == _selectedLoadPortIndex)
        //        {
        //            item.Value.Show();
        //            item.Value.ActivateView();
        //            item.Value.Visible = true;
        //        }
        //        else
        //        {
        //            item.Value.DeactivateView();
        //            item.Value.Visible = false;
        //            item.Value.Hide();
        //        }
        //    }
        //}
        private void UpdateLoadPortHardwareStatus()
        {
            // Initialized
            lblInitializationStatus.Active = _loadPortManager.GetInitializationState(_selectedLoadPortIndex);
            lblPresenceStatus.Active = _loadPortManager.GetPresentState(_selectedLoadPortIndex);
            lblPlacedStatus.Active = _loadPortManager.GetPlacedState(_selectedLoadPortIndex);
            lblClampingStatus.Active = _loadPortManager.GetClampingState(_selectedLoadPortIndex);
            lblDockingStatus.Active = _loadPortManager.GetDockingState(_selectedLoadPortIndex);
            lblDoorStatus.Active = _loadPortManager.GetDoorState(_selectedLoadPortIndex);
        }
        private void UpdateLoadPortStatus(bool initialize = false)
        {
            // Transfer State
            if (_loadPortManager.GetLoadPortTransferState(_selectedLoadPortIndex, ref _transferState) || initialize)
            {
                lblTransferStatus.Text = _transferState.ToString();
            }
            else
            {
                lblTransferStatus.Text = Unknown;
            }

            // Id State
            if (_loadPortManager.GetLoadPortCarrierIdState(_selectedLoadPortIndex, ref _carrierIdState) || initialize)
            {
                lblCarrierIdStatus.Text = _carrierIdState.ToString();
            }
            else
            {
                lblCarrierIdStatus.Text = Unknown;
            }

            // Slot State
            //if (_loadPortManager.GetLoadPortCarrierSlotMapState(_selectedLoadPortIndex, ref _carrierSlotState) || initialize)
            //{
            //    lblCarrierSlotStatus.Text = _carrierSlotState.ToString();
            //}
            //else
            //{
            //    lblCarrierSlotStatus.Text = Unknown;
            //}

            // AccessMode State
            if (false == _carrierAccessState.HasValue ||
                _carrierServer.GetCarrierAccessingStatus(_selectedLoadPortPortId) != _carrierAccessState ||
                initialize)
            {
                _carrierAccessState = _carrierServer.GetCarrierAccessingStatus(_selectedLoadPortPortId);
                lblCarrierAccessStatus.Text = _carrierAccessState.ToString();
            }

            // LoadingType
            if (false == _loadPortManager.GetCarrierLoadingType(_selectedLoadPortIndex).Equals(_loadingMode) || initialize)
            {
                _loadingMode = _loadPortManager.GetCarrierLoadingType(_selectedLoadPortIndex);
                lblLoadPortLoadingMode.Text = _loadingMode.ToString();
            }

            // AHMS
            if (false == _loadPortManager.GetAccessMode(_selectedLoadPortIndex).ToString().Equals(lblLoadPortAccessMode.Text) || initialize)
            {
                lblLoadPortAccessMode.Text = _loadPortManager.GetAccessMode(_selectedLoadPortIndex).ToString();
            }

            //if (false == _loadPortManager.GetAccessMode(_selectedLoadPortIndex).Equals(_accessMode) || initialize)
            //{
            //    _accessMode = _loadPortManager.GetAccessMode(_selectedLoadPortIndex);
            //    lblLoadPortAccessMode.Text = _accessMode.ToString();
            //}
        }
        private void DisplayLoadPortNames()
        {
            lblTitleSelectedLoadPort.Text = string.Format("LOADPORT {0}", _selectedLoadPortIndex + 1);
            lblSelectedLoadPort.Text = GetLoadPortName(_selectedLoadPortIndex);
        }
        private void UpdateParallelSignalValues()
        {
            _loadPortManager.GetAMHSSignalValues(_selectedLoadPortIndex, ref _inputValues, ref _outputValues);
        }
        private void DisplayParallelSignalControls()
        {
            lblInputSaftyInterLock.Active = _loadPortManager.GetAMHSSaftyInterLockStatus(_selectedLoadPortIndex);

            if (_inputValues == null)
            {
                for (int i = 0; i < _inputControls.Count; ++i)
                {
                    _inputControls[i].Active = false;
                }
            }
            else
            {
                foreach (var item in _inputValues)
                {
                    if (false == _inputIndex.TryGetValue(item.Key, out int index))
                        continue;

                    if (index < 0 || index >= _inputControls.Count)
                        continue;

                    _inputControls[index].Active = item.Value;
                }
            }

            if (_outputValues == null)
            {
                for (int i = 0; i < _outputControls.Count; ++i)
                {
                    _outputControls[i].Active = false;
                }
            }
            else
            {
                foreach (var item in _outputValues)
                {
                    if (false == _outputIndex.TryGetValue(item.Key, out int index))
                        continue;

                    if (index < 0 || index >= _outputControls.Count)
                        continue;

                    _outputControls[index].Active = item.Value;
                }
            }
        }
        #endregion </Display>

        #region <ETC>
        private string GetLoadPortName(int index)
        {
            var names = GetLoadPortNames();
            if (index < 0 || index >= names.Count)
                return string.Empty;

            return names[index];
        }
        private Dictionary<int, string> GetLoadPortNames()
        {
            var names = new Dictionary<int, string>();
            for (int i = 0; i < _loadPortManager.Count; ++i)
            {
                names[i] = _loadPortManager.GetLoadPortName(i);
            }

            return names;
        }
        private bool IsEquipmentIdle()
        {
            return _equipmentState.GetState().Equals(EQUIPMENT_STATE.IDLE);
        }
        private bool IsEquipmentBusy()
        {
            bool result = false;

            result = (_equipmentState.GetState().Equals(EQUIPMENT_STATE.EXECUTING) || _equipmentState.GetState().Equals(EQUIPMENT_STATE.SETUP)
                || _equipmentState.GetState().Equals(EQUIPMENT_STATE.INITIALIZE) || _equipmentState.GetState().Equals(EQUIPMENT_STATE.INITIALIZE));

            return result;
        }
        private bool ShowMessageBoxBeforeManualAction(string taskName, string actionName)
        {
            var messageBox = Functional.Form_MessageBox.GetInstance();
            return messageBox.ShowMessage(string.Format("Do you really want to execute : {0}", actionName));
        }    
        #endregion </ETC>

        #endregion </Methods>

        
    }
}