using System;
using System.Threading;

using FrameOfSystem3.Task;
using FrameOfSystem3.Work;

using EFEM.Modules;
using EFEM.Defines.LoadPort;
using EFEM.MaterialTracking;

namespace FrameOfSystem3.Views.EFEM_Simulator
{
    public partial class EFEMSimulator_LoarPort : ParameterPanel
    {
        #region <Constructors>
        public EFEMSimulator_LoarPort()
        {
            InitializeComponent();

            _taskOperator = Task.TaskOperator.GetInstance();
            _loadPortManager = LoadPortManager.Instance;

            SelectionListViewer = Functional.Form_SelectionList.GetInstance();

            int count = AppConfigManager.Instance.CountLoadPort;
            LoadPortNames = new string[count];
            for (int i = 0; i < LoadPortNames.Length; ++i)
            {
                LoadPortNames[i] = string.Format("LoadPort {0}", i + 1);
            }
        }
        #endregion </Constructors>

        #region <Fields>
        private static Task.TaskOperator _taskOperator = null;
        private static LoadPortManager _loadPortManager = null;
        private int _selectedIndex = 0;
        private readonly Functional.Form_SelectionList SelectionListViewer = null;
        private readonly string[] LoadPortNames = null;
        private readonly ReaderWriterLockSlim SlimLock = new ReaderWriterLockSlim();

        private const string SelectionListTitle = "Select LoadPort";

        private LoadPortTransferStates _transferState;
        private CarrierIdVerificationStates _carrierIdState;
        private CarrierSlotMapVerificationStates _carrierSlotMapState;
        #endregion </Fields>

        #region <Properties>
        #endregion </Properties>

        #region <Methods>

        #region <Override>
        protected override void ProcessWhenActivation()
        {
            base.ProcessWhenActivation();
        }

        public override void CallFunctionByTimer()
        {
            UpdateUI();
            DisplayStates();

            base.CallFunctionByTimer();
        }
        #endregion </Override>

        #region <Display>
        private void DisplayStates()
        {
            SlimLock.EnterWriteLock();

            if (_loadPortManager.GetLoadPortTransferState(_selectedIndex, ref _transferState))
            {
                lblTransferState.Text = _transferState.ToString();
            }

            if (_loadPortManager.GetLoadPortCarrierIdState(_selectedIndex, ref _carrierIdState))
            {
                lblIdState.Text = _carrierIdState.ToString();
            }

            if (_loadPortManager.GetLoadPortCarrierSlotMapState(_selectedIndex, ref _carrierSlotMapState))
            {
                lblSlotMapState.Text = _carrierSlotMapState.ToString();
            }

            SlimLock.ExitWriteLock();
        }
        private void UpdateUI()
        {
            lblLoadPortSelection.Text = LoadPortNames[_selectedIndex];
        }
        #endregion </Display>

        #region <UI Events>
        private void BtnCarrierPresenceClicked(object sender, EventArgs e)
        {
            if (sender.Equals(btnCarrierPlaced))
            {
                _taskOperator.TriggerLoadPortPlacedForSimul(_selectedIndex + 1);
            }
            else if (sender.Equals(btnCarrierRemoved))
            {
                _taskOperator.TriggerLoadPortRemovedForSimul(_selectedIndex + 1);
            }
            else if (sender.Equals(btnRemoveCarrierAll))
            {
                for (int i = 0; i < _loadPortManager.Count; ++i)
                {
                    int portId = _loadPortManager.GetLoadPortPortId(i);
                    _taskOperator.TriggerLoadPortRemovedForSimul(portId);
                }
            }

        }

        private void BtnMechanicalButtonClicked(object sender, EventArgs e)
        {
            if (sender.Equals(btnLoadButton))
            {
                if (false == CarrierManagementServer.Instance.HasCarrier(_selectedIndex + 1))
                    return;

                _taskOperator.TriggerLoadPortLoadButtonClickedForSimul(_selectedIndex + 1);
            }
            else if (sender.Equals(btnUnloadButton))
            {
                if (false == CarrierManagementServer.Instance.HasCarrier(_selectedIndex + 1))
                    return;

                _taskOperator.TriggerLoadPortUnloadButtonClickedForSimul(_selectedIndex + 1);
            }
            else if (sender.Equals(btnLoadCarrierAll))
            {
                string[] taskName = new string[_loadPortManager.Count];
                string[] actionNames = new string[taskName.Length];
                for (int i = 0; i < taskName.Length; ++i)
                {
                    taskName[i] = string.Format("LoadPort{0}", i + 1);
                    actionNames[i] = TaskLoadPort.TASK_ACTION.CARRIER_LOADING.ToString();
                }

                _taskOperator.SetOperation(ref taskName, ref actionNames);
            }
            else if (sender.Equals(btnUnloadCarrierAll))
            {
                string[] taskName = new string[_loadPortManager.Count];
                string[] actionNames = new string[taskName.Length];
                for (int i = 0; i < taskName.Length; ++i)
                {
                    taskName[i] = string.Format("LoadPort{0}", i + 1);
                    actionNames[i] = TaskLoadPort.TASK_ACTION.CARRIER_UNLOADING.ToString();
                }

                _taskOperator.SetOperation(ref taskName, ref actionNames);
            }
        }
        private void LblLoadPortSlectionClicked(object sender, EventArgs e)
        {
            if (SelectionListViewer.CreateForm(SelectionListTitle, LoadPortNames,
                LoadPortNames[_selectedIndex], false, LoadPortNames[0]))
            {
                int selected = 0;
                SelectionListViewer.GetResult(ref selected);
                if (selected >= 0 && selected < LoadPortNames.Length)
                {
                    if (_selectedIndex != selected)
                    {
                        SlimLock.EnterWriteLock();

                        _selectedIndex = selected;
                        UpdateUI();

                        SlimLock.ExitWriteLock();
                    }
                }
            }
        }

        #endregion </UI Events>

        #endregion </Methods>

        private void ClearInfos(object sender, EventArgs e)
        {
            if (sender.Equals(btnClearLoadPortInfo))
            {
                for (int i = 0; i < _loadPortManager.Count; ++i)
                {
                    _taskOperator.TriggerLoadPortRemovedForSimul(i + 1);
                }
            }
            else if (sender.Equals(btnClearRobotInfo))
            {
                for (int i = 0; i < AtmRobotManager.Instance.Count; ++i)
                {
                    var name = AtmRobotManager.Instance.GetRobotName(i);
                    SubstrateManager.Instance.RemoveSubstratesAtRobot(name);
                }
            }
            else if (sender.Equals(btnClearPMInfo))
            {
                for (int i = 0; i < ProcessModuleGroup.Instance.Count; ++i)
                {
                    var name = ProcessModuleGroup.Instance.GetProcessModuleName(i);
                    SubstrateManager.Instance.RemoveSubstratesAtProcessModule(name);
                }
            }
        }
    }
}
