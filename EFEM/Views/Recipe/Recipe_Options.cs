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
using FrameOfSystem3.Views.Recipe.Options;

namespace FrameOfSystem3.Views.Recipe
{
	public partial class Recipe_Options : UserControlForMainView.CustomView
	{
		#region <CONSTRUCTOR>
		public Recipe_Options()
		{
			InitializeComponent();

			InitializeSubPanels();
		}
		#endregion </CONSTRUCTOR>

		#region <FIELD>
		private Functional.Form_MessageBox _messageBox = Functional.Form_MessageBox.GetInstance();

		private PanelInterface _panelInstance = new PanelInterface();

		private RecipeOptionProcess _panelProcess = new RecipeOptionProcess();
		#endregion </FIELD>

		enum EN_PANEL_LIST
		{
			Process,
		}

		private void InitializeSubPanels()
		{
			var addPanelList = new Dictionary<string, List<ParameterGroupPanel>>();
			addPanelList.Add(EN_PANEL_LIST.Process.ToString(), new List<ParameterGroupPanel>());
			addPanelList[EN_PANEL_LIST.Process.ToString()].Add(new ParameterGroupPanel(_panelProcess));

			var tabButtonList = new Dictionary<Sys3Controls.Sys3button, string>();
			tabButtonList.Add(btn_Options, EN_PANEL_LIST.Process.ToString());

			_panelInstance.InitializeSubPanels(panelSubView, addPanelList, tabButtonList);
		}

		#region <OVERRIDE>
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
		#endregion </OVERRIDE>

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