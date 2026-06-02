using System;
using System.Collections.Generic;

using FrameOfSystem3.Views.Setup.AtmRobot;

namespace FrameOfSystem3.Views.Setup
{
	public partial class Setup_AtmRobot : UserControlForMainView.CustomView
	{
        #region <Constructors>
        public Setup_AtmRobot()
        {
            InitializeComponent();

            InitializeSubPanels();
        }
        #endregion </Constructors>

        #region <Types>
        enum EN_PANEL_LIST
        {
            ATM1 = 0,
        }
        #endregion </Types>

        #region <Fields>
        #region GUI
        private readonly Functional.Form_MessageBox _messageBox = Functional.Form_MessageBox.GetInstance();
        private readonly PanelInterface _panelInstance = new PanelInterface();
        #endregion
        #endregion </Fields>

        #region <Properties>
        #endregion </Properties>

        #region <Methods>

        private void InitializeSubPanels()
        {
            var arrays = Enum.GetValues(typeof(EN_PANEL_LIST));

            var addPanelList = new Dictionary<string, List<ParameterGroupPanel>>();
            var tabButtonList = new Dictionary<Sys3Controls.Sys3button, string>();
            
            foreach (var item in arrays)
            {
                addPanelList.Add(item.ToString(), new List<ParameterGroupPanel>());

                ParameterGroupPanel subviewPanel = new ParameterGroupPanel(new SetupSubViewAtmRobot((int)item));
                subviewPanel.Dock = System.Windows.Forms.DockStyle.Fill;
                subviewPanel.DisableGroupBox();
                addPanelList[item.ToString()].Add(subviewPanel);
            }

            tabButtonList.Add(m_tabATM1, EN_PANEL_LIST.ATM1.ToString());

            _panelInstance.InitializeSubPanels(panelSubView, addPanelList, tabButtonList);
        }

        #region <Override Methods>
        public override void CallFunctionByTimer()
        {
            _panelInstance.CallFunctionByTimer();
            base.CallFunctionByTimer();
        }
        protected override void ProcessWhenActivation()
        {
            _panelInstance.ProcessWhenActivation();
            base.ProcessWhenActivation();
        }
        protected override void ProcessWhenDeactivation()
        {
            _panelInstance.ProcessWhenDeactivation();
            base.ProcessWhenDeactivation();
        }
        #endregion </Override Methods>

        #region <UI EVENT>
        private void Click_TabButton(object sender, EventArgs e)
        {
            if (!(sender is Sys3Controls.Sys3button btn)) return;

            if (false == _panelInstance.Click_TabButton(btn))
            {
                _messageBox.ShowMessage("Not ready page...");
                return;
            }
        }

        private void Click_AllExtendReduce(object sender, EventArgs e)
        {
            bool isExtend;
            if (sender == btn_AllExtend)
            {
                isExtend = true;
            }
            else if (sender == btn_AllReduce)
            {
                isExtend = false;
            }
            else return;

            _panelInstance.Click_AllExtendReduce(isExtend);
        }
        private void ClickParameterSave(object sender, EventArgs e)
        {
            _panelInstance.ClickParameterSave();
        }
        private void ClickParameterUndo(object sender, EventArgs e)
        {
            _panelInstance.ClickParameterUndo();
        }
        #endregion </UI EVENT>
        
        #endregion </Methods>
    }
}
