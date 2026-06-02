using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using Define.DefineEnumProject.Task;

using FrameOfSystem3.Component;
using FrameOfSystem3.Views.Setup.RFID;

namespace FrameOfSystem3.Views.Setup
{
	public partial class Setup_RFID : UserControlForMainView.CustomView
	{
		#region <CONSTRUCTOR>
		public Setup_RFID()
		{
			InitializeComponent();

			InitializeSubPanels();
		}
		#endregion </CONSTRUCTOR>

		#region <FIELD>
		private Functional.Form_MessageBox _messageBox = Functional.Form_MessageBox.GetInstance();

		private PanelInterface _panelInstance = new PanelInterface();

		private SetupSubViewRfid _panelFoup = new SetupSubViewRfid(EFEM.Defines.LoadPort.LoadPortLoadingMode.Foup);
		private SetupSubViewRfid _panelCassette = new SetupSubViewRfid(EFEM.Defines.LoadPort.LoadPortLoadingMode.Cassette);
		#endregion </FIELD>

		enum EN_PANEL_LIST
		{
			FOUP,
			CASSETTE,
		}

		
		private void InitializeSubPanels()
		{
			var addPanelList = new Dictionary<string, List<ParameterGroupPanel>>();
			var tabButtonList = new Dictionary<Sys3Controls.Sys3button, string>();

			addPanelList.Add(EN_PANEL_LIST.FOUP.ToString(), new List<ParameterGroupPanel>());
			ParameterGroupPanel subviewPanel_Foup = new ParameterGroupPanel(_panelFoup);
			subviewPanel_Foup.Dock = DockStyle.Fill;
            subviewPanel_Foup.DisableGroupBox();
			addPanelList[EN_PANEL_LIST.FOUP.ToString()].Add(subviewPanel_Foup);

			addPanelList.Add(EN_PANEL_LIST.CASSETTE.ToString(), new List<ParameterGroupPanel>());
			ParameterGroupPanel subviewPanel_Cassette = new ParameterGroupPanel(_panelCassette);
			subviewPanel_Cassette.Dock = DockStyle.Fill;
			subviewPanel_Cassette.DisableGroupBox();
			addPanelList[EN_PANEL_LIST.CASSETTE.ToString()].Add(subviewPanel_Cassette);

			tabButtonList.Add(m_tabFoup, EN_PANEL_LIST.FOUP.ToString());
			tabButtonList.Add(m_tabCassette, EN_PANEL_LIST.CASSETTE.ToString());

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
			Sys3Controls.Sys3button btn = sender as Sys3Controls.Sys3button;
			if (btn == null) return;

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
	}
}