using System;
using System.Collections.Generic;
using System.Windows.Forms;

using FrameOfSystem3.Recipe;

using EFEM.Modules;
using EFEM.Defines.LoadPort;
using EFEM.MaterialTracking;

namespace FrameOfSystem3.Views.Operation.SubPanelSummary.LoadPortSummary
{
    public partial class SummaryLoadPortState : UserControl
    {
        #region <Constructors>
        public SummaryLoadPortState(int lpIndex)
        {
            LoadPortIndex = lpIndex;
            MessageBox = Functional.Form_MessageBox.GetInstance();
            
            _loadPortManager = LoadPortManager.Instance;
            _carrierManager = CarrierManagementServer.Instance;

            _stateInformation = new LoadPortStateInformation();

            PortId = _loadPortManager.GetLoadPortPortId(lpIndex);
            _recipe = FrameOfSystem3.Recipe.Recipe.GetInstance();

            InitializeComponent();

            InitializeSubPanels();

            this.Dock = DockStyle.Fill;
        }
        #endregion </Constructors>

        #region <Enums>
        private enum TabSubPanels
        {
            Slot,
            State,
            PIO,
        }
        #endregion </Enums>

        #region <Fields>
        private readonly PanelInterface PanelInterface = new PanelInterface();
        private readonly int LoadPortIndex;
        private readonly int PortId;

        private static LoadPortManager _loadPortManager = null;
        private static CarrierManagementServer _carrierManager = null;
        private static FrameOfSystem3.Recipe.Recipe _recipe = null;
        private readonly Functional.Form_MessageBox MessageBox = null;
        private LoadPortStateInformation _stateInformation = null;
        #endregion </Fields>

        #region <Properties>
        #endregion </Properties>

        #region <Methods>

        #region <Display Methods>
        public void ProcessWhenActivation()
        {
            UpdateLoadPortUsableByParameter();

            PanelInterface.ProcessWhenActivation();
        }

        public void ProcessWhenDeactivation()
        {
            PanelInterface.ProcessWhenDeactivation();
        }

        public void CallFunctionByTimer()
        {
            if (DisplayLoadPortStates())
            {
                DisplayCarrierInfo();
            }

            PanelInterface.CallFunctionByTimer();
        }

        private void DisplayCarrierInfo()
        {
            if (false == _carrierManager.GetCarrierId(PortId).Equals(lblCarrierId.Text))
            {
                lblCarrierId.Text = _carrierManager.GetCarrierId(PortId);
            }

            if (false == _carrierManager.GetCarrierLotId(PortId).Equals(sys3Label2.Text))
            {
                sys3Label2.Text = _carrierManager.GetCarrierLotId(PortId);
            }
        }

        private bool DisplayLoadPortStates()
        {
            _stateInformation = _loadPortManager.GetLoadPortState(LoadPortIndex);
            if (_stateInformation == null)
                return false;

            if (ledInitialized.Active != _stateInformation.Initialized)
            {
                ledInitialized.Active = _stateInformation.Initialized;
            }

            Enabled = _stateInformation.Enabled;
            if (false == Enabled)
                return false;

            if (ledPlaced.Active != _stateInformation.Placed)
            {
                ledPlaced.Active = _stateInformation.Placed;
            }

            if (ledPresent.Active != _stateInformation.Present)
            {
                ledPresent.Active = _stateInformation.Present;
            }

            if (ledClamped.Active != _stateInformation.ClampState)
            {
                ledClamped.Active = _stateInformation.ClampState;
            }

            if (ledDocked.Active != _stateInformation.DockState)
            {
                ledDocked.Active = _stateInformation.DockState;
            }

            if (ledOpened.Active != _stateInformation.DoorState)
            {
                ledOpened.Active = _stateInformation.DoorState;
            }

            return true;
        }

        private void UpdateLoadPortUsableByParameter()
        {
        }
        #endregion </Display Methods>

        #region <Initialize>
        private void InitializeSubPanels()
        {
            var addPanelList = new Dictionary<string, List<ParameterGroupPanel>>();
            var tabButtonList = new Dictionary<Sys3Controls.Sys3button, string>();

            string namePanelSlot = TabSubPanels.Slot.ToString();
            addPanelList.Add(namePanelSlot, new List<ParameterGroupPanel>());

            // Slot
            var lpSlot = new ParameterGroupPanel(new SummaryLoadPortState_Slot(LoadPortIndex), false, true);
            lpSlot.Dock = DockStyle.Fill;
            lpSlot.DisableGroupBox();
            addPanelList[namePanelSlot].Add(lpSlot);
            tabButtonList.Add(btnLoadPortStateSlotInfo, namePanelSlot);

            // State
            namePanelSlot = TabSubPanels.State.ToString();
            addPanelList.Add(namePanelSlot, new List<ParameterGroupPanel>());
            var lpState = new ParameterGroupPanel(new SummaryLoadPortState_State(LoadPortIndex), false, true);
            lpState.Dock = DockStyle.Fill;
            lpState.DisableGroupBox();
            addPanelList[namePanelSlot].Add(lpState);
            tabButtonList.Add(btnLoadPortState, namePanelSlot);

            // PIO
            namePanelSlot = TabSubPanels.PIO.ToString();
            addPanelList.Add(namePanelSlot, new List<ParameterGroupPanel>());
            var lpPio = new ParameterGroupPanel(new SummaryLoadPortState_PIO(LoadPortIndex), false, true);
            lpPio.Dock = DockStyle.Fill;
            lpPio.DisableGroupBox();
            addPanelList[namePanelSlot].Add(lpPio);
            tabButtonList.Add(btnLoadPortStateParallelIO, namePanelSlot);

            PanelInterface.InitializeSubPanels(pnLoadPortState, addPanelList, tabButtonList);
        }
        #endregion </Initialize>

        #region <UI Events>
        private void BtnSubPanelButtonClicked(object sender, EventArgs e)
        {
            if (!(sender is Sys3Controls.Sys3button btn)) return;

            if (false == PanelInterface.Click_TabButton(btn))
            {
                MessageBox.ShowMessage("Not ready page...");
                return;
            }
        }
        #endregion </UI Events>

        #endregion </Methods>

        private void LedLoadPortStatesHovered(object sender, EventArgs e)
        {
            if (!(sender is Sys3Controls.Sys3LedLabel label)) return;

            tooltipLoadPortStates.ToolTipTitle = label.Tag.ToString();
            tooltipLoadPortStates.SetToolTip(label, string.Format(label.Active.ToString()));
        }
    }
}