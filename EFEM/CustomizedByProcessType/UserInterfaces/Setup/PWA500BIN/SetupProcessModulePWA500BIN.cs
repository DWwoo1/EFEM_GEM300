using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using System.Collections.Concurrent;
using System.Threading;
using System.Linq;

using Sys3Controls;

using EFEM.Modules;
using EFEM.Defines.Common;
using EFEM.Defines.LoadPort;
using EFEM.MaterialTracking;
using EFEM.Defines.ProcessModule;
using EFEM.CustomizedByProcessType.PWA500BIN;

using FrameOfSystem3.Views;
using Define.DefineEnumProject.DigitalIO;

namespace EFEM.CustomizedByProcessType.UserInterface.Setup.PWA500BIN
{
    public partial class SetupProcessModulePWA500BIN : UserControlForMainView.CustomView
    {
        #region <Constructors>
        public SetupProcessModulePWA500BIN()
        {
            InitializeComponent();

            _processGroup = ProcessModuleGroup.Instance;
            _communicationInfo = new NetworkInformation();

            MappedServiceIndex = new Dictionary<int, int>();
            MappedClientIndex = new Dictionary<int, int>();

            LoadPortPanels = new Dictionary<int, Panel>
            {
                [0] = pnLoadPort1,
                [1] = pnLoadPort2,
                [2] = pnLoadPort3,
                [3] = pnLoadPort4,
                [4] = pnLoadPort5,
                [5] = pnLoadPort6
            };
            PanelInterfaces = new Dictionary<int, PanelInterface>();

            foreach (var item in LoadPortPanels)
            {
                string name = string.Format("LoadPort{0}", item.Key + 1);
                var panels = new Dictionary<string, List<ParameterGroupPanel>>
                {
                    [name] = new List<ParameterGroupPanel>()
                };

                ParameterGroupPanel groupPanel = new ParameterGroupPanel(new SetupPWA500BINLoadPortOptions(item.Key));
                groupPanel.Dock = DockStyle.Fill;
                groupPanel.DisableGroupBox();
                panels[name].Add(groupPanel);

                PanelInterfaces[item.Key] = new PanelInterface();
                PanelInterfaces[item.Key].InitializeSubPanels(item.Value, panels, name);

            }

            {
                string name = "ProcessModuleOptions";
                var panels = new Dictionary<string, List<ParameterGroupPanel>>
                {
                    [name] = new List<ParameterGroupPanel>()
                };

                ParameterGroupPanel groupPanel = new ParameterGroupPanel(new SetupPWA500BINOptions(name));
                groupPanel.Dock = DockStyle.Fill;
                groupPanel.DisableGroupBox();
                panels[name].Add(groupPanel);

                int index = PanelInterfaces.Count;
                PanelInterfaces[index] = new PanelInterface();
                PanelInterfaces[index].InitializeSubPanels(pnPWA500BINOptions, panels, name);
            }
            InitGridViews();

            this.Dock = DockStyle.Fill;
        }
        #endregion </Constructors>

        #region <Fields>
        private const int ProcessModuleIndex = 0;
        private static ProcessModuleGroup _processGroup = null;
        private NetworkInformation _communicationInfo;
        //private readonly WCFServiceIndex[] WcfServiceIndex = null;
        //private readonly WCFClientIndex[] WcfClientIndex = null;

        private const int ColumnStatus = 0;
        private const int ColumnName = 1;

        private readonly Color EnabledColor = Color.LimeGreen;
        private readonly Color DisabledColor = Color.White;

        private readonly Dictionary<int, int> MappedServiceIndex = null;
        private readonly Dictionary<int, int> MappedClientIndex = null;

        private readonly Dictionary<int, PanelInterface> PanelInterfaces = null;
        private readonly Dictionary<int, Panel> LoadPortPanels = null;
        #endregion </Fields>

        #region <Methods>

        #region <Initialize>
        private void InitializeSubViewLoadPort()
        {

        }
        #endregion </Initialize>

        #region <Override Methods>
        public override void CallFunctionByTimer()
        {
            UpdateServiceStatus();
            foreach (var item in PanelInterfaces)
            {
                item.Value.CallFunctionByTimer();
            }
        }
        protected override void ProcessWhenActivation()
        {
            base.ProcessWhenActivation();

            foreach (var item in PanelInterfaces)
            {
                item.Value.ProcessWhenActivation();
            }
        }
        protected override void ProcessWhenDeactivation()
        {
            foreach (var item in PanelInterfaces)
            {
                item.Value.ProcessWhenDeactivation();
            }

            base.ProcessWhenDeactivation();
        }
        #endregion </Override Methods>

        #region <Display>
        private void InitGridViews()
        {
            if (_processGroup.GetCommunicationInfo(ProcessModuleIndex, ref _communicationInfo))
            {
                // Service
                gvServiceStatus.Rows.Clear();
                int index = 0;
                foreach (var item in _communicationInfo.ServiceInfo)
                {
                    gvServiceStatus.Rows.Add();
                    gvServiceStatus[ColumnName, index].Value = item.Value.Name;
                    MappedServiceIndex[item.Key] = index;
                    ++index;
                }

                gvClientStatus.Rows.Clear();
                index = 0;
                foreach (var item in _communicationInfo.ClientInfo)
                {
                    gvClientStatus.Rows.Add();
                    gvClientStatus[ColumnName, index].Value = item.Value.Name;
                    MappedClientIndex[item.Key] = index;
                    ++index;
                }
            }

        }
        private void UpdateServiceStatus()
        {
            if (_processGroup.GetCommunicationInfo(ProcessModuleIndex, ref _communicationInfo))
            {
                foreach (var item in _communicationInfo.ServiceInfo)
                {
                    int row = MappedServiceIndex[item.Key];
                    if (row < gvClientStatus.Rows.Count)
                    {
                        if (item.Value.ConnectionStatus)
                        {
                            gvServiceStatus.Rows[row].Cells[ColumnStatus].Style.BackColor = EnabledColor;
                            gvServiceStatus.Rows[row].Cells[ColumnStatus].Style.SelectionBackColor = EnabledColor;
                        }
                        else
                        {
                            gvServiceStatus.Rows[row].Cells[ColumnStatus].Style.BackColor = DisabledColor;
                            gvServiceStatus.Rows[row].Cells[ColumnStatus].Style.SelectionBackColor = DisabledColor;
                        }
                    }
                }

                foreach (var item in _communicationInfo.ClientInfo)
                {
                    int row = MappedClientIndex[item.Key];
                    if (row < gvClientStatus.Rows.Count)
                    {
                        // gvServiceStatus[ColumnStatus, item.Key];

                        if (item.Value.ConnectionStatus)
                        {
                            gvClientStatus.Rows[row].Cells[ColumnStatus].Style.BackColor = EnabledColor;
                            gvClientStatus.Rows[row].Cells[ColumnStatus].Style.SelectionBackColor = EnabledColor;
                        }
                        else
                        {
                            gvClientStatus.Rows[row].Cells[ColumnStatus].Style.BackColor = DisabledColor;
                            gvClientStatus.Rows[row].Cells[ColumnStatus].Style.SelectionBackColor = DisabledColor;
                        }
                    }
                }
            }
        }
        #endregion </Display>

        #region <UI Events>
        private void BtnSaveParameterClicked(object sender, EventArgs e)
        {
            foreach (var item in PanelInterfaces)
            {
                item.Value.ClickParameterSave();
            }

            for (int i = 0; i < LoadPortManager.Instance.Count; ++i)
            {
                var param = FrameOfSystem3.Recipe.PARAM_EQUIPMENT.UseLoadPort1 + i;
                bool value = FrameOfSystem3.Recipe.Recipe.GetInstance().GetValue(FrameOfSystem3.Recipe.EN_RECIPE_TYPE.EQUIPMENT, param.ToString(), true);
                LoadPortManager.Instance.SetLoadPortEnabled(i, value);
            }
        }

        private void BtnUndoParameterClicked(object sender, EventArgs e)
        {
            foreach (var item in PanelInterfaces)
            {
                item.Value.ClickParameterUndo();
            }

            for (int i = 0; i < LoadPortManager.Instance.Count; ++i)
            {
                var param = FrameOfSystem3.Recipe.PARAM_EQUIPMENT.UseLoadPort1 + i;
                bool value = FrameOfSystem3.Recipe.Recipe.GetInstance().GetValue(FrameOfSystem3.Recipe.EN_RECIPE_TYPE.EQUIPMENT, param.ToString(), true);
                LoadPortManager.Instance.SetLoadPortEnabled(i, value);
            }
        }
        #endregion </UI Events>

        #endregion </Methods>


    }
}
