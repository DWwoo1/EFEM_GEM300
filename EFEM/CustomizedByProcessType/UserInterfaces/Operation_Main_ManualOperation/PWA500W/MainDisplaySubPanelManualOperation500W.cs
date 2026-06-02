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
using EFEM.MaterialTracking;
using FrameOfSystem3.Views;
using FrameOfSystem3.Views.MapManager;
using FrameOfSystem3.Views.Operation.SubPanelSummary.LoadPortSummary;
using FrameOfSystem3.Views.Operation.SubPanelManualOperation.LoadPort;

namespace EFEM.CustomizedByProcessType.UserInterface.OperationMainManual.PWA500W
{
    public partial class MainDisplaySubPanelManualOperation500W : ParameterPanel
    {
        #region <Constructors>
        public MainDisplaySubPanelManualOperation500W(string name)
        {
            InitializeComponent();

            this.Tag = name;

            //PanelInstance = new PanelInterface();
            //MessageBox = Functional.Form_MessageBox.GetInstance();
            _selectedPanels = PanelMode.Load;
            ManualOperation = new MainDisplaySubPanelManualOperationControl500W
            {
                Dock = DockStyle.Fill
            };
            pnManualOperation.Controls.Add(ManualOperation);

            ManualOperationLoadPort = new MainDisplaySubPanelManualOperationLoadPort
            {
                Dock = DockStyle.Fill
            };
            ManualOperationLoadPort.Hide();
            pnManualOperation.Controls.Add(ManualOperationLoadPort);

            MaterialEditor = new MainDisplaySubPanelManualOperationEditor500W
            {
                Dock = DockStyle.Fill
            };
            MaterialEditor.Hide();
            pnManualOperation.Controls.Add(MaterialEditor);

            OperationButtons = new Dictionary<PanelMode, Sys3Controls.Sys3button>
            {
                { PanelMode.Load, btnManualOperationLoad },
                { PanelMode.Unload, btnManualOperationUnload },
                { PanelMode.LoadPort, btnManualOperationLoadPort },
                { PanelMode.Editor, btnManualOperationEditor }
            };

            InitializeSubPanels();

            this.Dock = DockStyle.Fill;
        }
        #endregion </Constructors>

        #region <Fields>
        //private readonly PanelInterface PanelInstance = null;
        //private readonly Functional.Form_MessageBox MessageBox = null;
        private readonly MainDisplaySubPanelManualOperationControl500W ManualOperation = null;
        private readonly MainDisplaySubPanelManualOperationEditor500W MaterialEditor = null;
        private readonly MainDisplaySubPanelManualOperationLoadPort ManualOperationLoadPort = null;
        private readonly Dictionary<PanelMode, Sys3Controls.Sys3button> OperationButtons = null;
        private PanelMode _selectedPanels;
        private UserControlForMainView.CustomView _selectedPanel = null;
        #endregion </Fields>

        #region <Properties>
        #endregion </Properties>

        #region <Methods>

        #region <SubPanel>
        private void InitializeSubPanels()
        {
            _selectedPanel = ManualOperation;
            //var addPanelList = new Dictionary<string, List<ParameterGroupPanel>>();

            //AddControlToPanelInterface(SubPanels.Load,
            //    new MainDisplaySubPanelManualOperationLoad500BIN(SubPanels.Load.ToString()),
            //    ref addPanelList);

            //AddControlToPanelInterface(SubPanels.Unload,
            //    new MainDisplaySubPanelManualOperationUnload500BIN(SubPanels.Unload.ToString()),
            //    ref addPanelList);


            // 버튼 할당
            //var tabButtonList = new Dictionary<Sys3Controls.Sys3button, string>
            //{
            //    { btnManualOperationLoad, SubPanels.Load.ToString() },
            //    { btnManualOperationUnload, SubPanels.Unload.ToString() }
            //};

            //PanelInstance.InitializeSubPanels(pnManualOperation, addPanelList, tabButtonList);
        }
        //private void AddControlToPanelInterface(SubPanels panelType, ParameterPanel panelControl, ref Dictionary<string, List<ParameterGroupPanel>> panelList)
        //{
        //    string panelSummaryName = panelType.ToString();
        //    var lpSummaryPanel = new ParameterGroupPanel(panelControl, false, true);

        //    lpSummaryPanel.DisableGroupBox();   // 그룹박스 미사용

        //    panelList.Add(panelSummaryName, new List<ParameterGroupPanel>());
        //    panelList[panelSummaryName].Add(lpSummaryPanel);
        //}
        #endregion </SubPanel>

        #region <Override Methods>
        protected override void ProcessWhenActivation()
        {
            _selectedPanel.ActivateView();

            base.ProcessWhenActivation();
        }
        public override void CallFunctionByTimer()
        {
            _selectedPanel.CallFunctionByTimer();

            base.CallFunctionByTimer();
        }
        protected override void ProcessWhenDeactivation()
        {
            _selectedPanel.DeactivateView();

            base.ProcessWhenDeactivation();
        }
        #endregion </Override Methods>

        #region <UI Events>
        private void BtnSubPanelClicked(object sender, EventArgs e)
        {
            if (!(sender is Sys3Controls.Sys3button btn)) return;

            foreach (var item in OperationButtons)
            {
                if (item.Value.Equals(btn))
                {
                    if (item.Value.ButtonClicked)
                        return;
                }
            }

            foreach (var item in OperationButtons)
            {
                if (item.Value.Equals(btn))
                {
                    SelectSubPanel(item.Key);
                    
                    item.Value.ButtonClicked = true;
                    item.Value.MainFontColor = Color.White;
                }
                else
                {
                    item.Value.ButtonClicked = false;
                    item.Value.MainFontColor = Color.DarkBlue;
                }
            }

            DisplaySubPanel();
        }
        #endregion </UI Events>

        #region <Internal>
        private void DisplaySubPanel()
        {
            _selectedPanel.Hide();
            _selectedPanel.DeactivateView();

            switch (_selectedPanels)
            {
                case PanelMode.Load:
                case PanelMode.Unload:
                    _selectedPanel = ManualOperation;
                    break;
                case PanelMode.LoadPort:
                    _selectedPanel = ManualOperationLoadPort;
                    break;
                case PanelMode.Editor:
                    _selectedPanel = MaterialEditor;
                    break;
                default:
                    break;
            }

            _selectedPanel.ActivateView();
            _selectedPanel.Show();
        }
        private void SelectSubPanel(PanelMode panelMode)
        {
            switch (panelMode)
            {
                case PanelMode.Load:
                case PanelMode.Unload:
                    {
                        ManualOperation.ChangePanelMode(panelMode);
                    }
                    break;
                case PanelMode.Editor:
                    break;
                default:
                    break;
            }
            _selectedPanels = panelMode;
        }
        #endregion </Internal>

        #endregion </Methods>
    }
}