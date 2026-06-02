using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using EFEM.Modules;
using EFEM.Defines.LoadPort;

namespace FrameOfSystem3.Views.EFEM_Simulator
{
    public partial class Form_EFEMSimulator : Form
    {
        #region <Constructors>
        private Form_EFEMSimulator()
        {
            InitializeComponent();

            Timer = new Timer
            {
                Interval = 300
            };
            Timer.Tick += new EventHandler(CallFunctionByTimer);

            //_loadPortManager = LoadPortManager.Instance;
            MessageBox = Functional.Form_MessageBox.GetInstance();

            InitializeSubPanels();

            if (Work.AppConfigManager.Instance.ProcessType != Define.DefineEnumProject.AppConfig.EN_PROCESS_TYPE.DIE_TRANSFER &&
                Work.AppConfigManager.Instance.ProcessType != Define.DefineEnumProject.AppConfig.EN_PROCESS_TYPE.DIE_TRANSFER_300)
            {
                lblSubSize.Visible = false;
                tglSubSize.Visible = false;
            }
            else
            {
                Task.TaskOperator.GetInstance().SetSubstrateSizeFor500(8);
            }
        }
        #endregion </Constructors>

        #region <Fields>
        private readonly Timer Timer = null;
        private static Form_EFEMSimulator _instance = null;
        //private static LoadPortManager _loadPortManager = null;

        private readonly PanelInterface PanelInterface = new PanelInterface();
        private readonly Functional.Form_MessageBox MessageBox = null;
        #endregion </Fields>

        #region <Properties>
        public static Form_EFEMSimulator Instance
        {
            get
            {
                if (_instance == null)
                    _instance = new Form_EFEMSimulator();
                return _instance;
            }
        }
        #endregion </Properties>

        #region <Methods>

        #region <Create Form>
        public void CreateForm()
        {
            PanelInterface.ProcessWhenActivation();

            Timer.Start();

            this.Show();
        }
        private void InitializeSubPanels()
        {
            var addPanelList = new Dictionary<string, List<ParameterGroupPanel>>();
            var tabButtonList = new Dictionary<Sys3Controls.Sys3button, string>();

            string namePanelSlot = "LOADPORT".ToString();
            addPanelList.Add(namePanelSlot, new List<ParameterGroupPanel>());

            var lpPanel = new ParameterGroupPanel(new EFEMSimulator_LoarPort(), false, true);
            lpPanel.DisableGroupBox();
            addPanelList[namePanelSlot].Add(lpPanel);
            tabButtonList.Add(btnLoadPortSimulator, namePanelSlot);

            string namePanelRobot = "ROBOT";
            addPanelList.Add(namePanelRobot, new List<ParameterGroupPanel>());
            
            var atmRobotPanel = new ParameterGroupPanel(new EFEMSimulator_AtmRobot(), false, true);
            atmRobotPanel.DisableGroupBox();
            addPanelList[namePanelRobot].Add(atmRobotPanel);
           
            tabButtonList.Add(btnAtmRobotSimulator, namePanelRobot);

            PanelInterface.InitializeSubPanels(pnLoadPortState, addPanelList, tabButtonList);
        }
        #endregion </Create Form>

        #region <Timer>
        private void CallFunctionByTimer(object sender, EventArgs e)
        {
            PanelInterface.CallFunctionByTimer();
        }
        #endregion </Timer>

        #region <UI EVENT>
        private void BtnCloseClicked(object sender, EventArgs e)
        {
            PanelInterface.ProcessWhenDeactivation();

            Timer.Stop();

            this.Hide();
        }
        #endregion </UI EVENT>

        #endregion </Methods>

        private void BtnSubPanelButtonClicked(object sender, EventArgs e)
        {
            if (!(sender is Sys3Controls.Sys3button btn)) return;

            if (false == PanelInterface.Click_TabButton(btn))
            {
                MessageBox.ShowMessage("Not ready page...");
                return;
            }
        }

        private void TglSubSizeToggled(object sender, EventArgs e)
        {
            if (tglSubSize.Active)
            {
                Task.TaskOperator.GetInstance().SetSubstrateSizeFor500(12);
            }
            else
            {
                Task.TaskOperator.GetInstance().SetSubstrateSizeFor500(8);
            }
        }
    }
}
