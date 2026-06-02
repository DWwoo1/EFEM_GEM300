using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using Define.DefineEnumBase.Common;
using FrameOfSystem3.Component;

namespace FrameOfSystem3.Views.Functional.Jog
{
	public partial class JogPanel_ModeRelative : UserControl
	{
		#region <CONSTRUCTOR>
		public JogPanel_ModeRelative(Form_Jog parentsInstance)
		{
			InitializeComponent();

			_parents = parentsInstance;
		}
		#endregion </CONSTRUCTOR>

		#region <FILED>
		Form_Jog _parents = null;
		#endregion </FILED>

		#region <UI EVENT>
		private void Click_Distance(object sender, EventArgs e)
		{
			_parents.Click_Distance(lbl_Distance.Text);
		}
		private void Click_DefinedStep(object sender, EventArgs e)
		{
			double step;
			if (sender == btn_Define_1) step = 0.001;
			else if (sender == btn_Define_10) step = 0.01;
			else if (sender == btn_Define_100) step = 0.1;
			else if (sender == btn_Define_500) step = 0.5;
			else if (sender == btn_Define_1000) step = 1;
			else return;

			_parents.Click_DefinedStep(step);
		}
		private void Click_FindControl(object sender, EventArgs e)
		{
			EN_UP_DOWN category;
			if (sender == btn_FineControlUp) category = EN_UP_DOWN.UP;
			else if (sender == btn_FineControlDown) category = EN_UP_DOWN.DOWN;
			else return;

			_parents.Click_FindControl(category);
		}
		#endregion </UI EVENT>

		#region <PROPERTY>
		public string DistanceLabelText { set { lbl_Distance.Text = value; Refresh(); } }
		#endregion </PROPERTY>
	}
}
