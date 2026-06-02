using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using FrameOfSystem3.Component;

namespace FrameOfSystem3.Views.Functional.Jog
{
	public partial class JogPanel_ModeSpeed : UserControl
	{
		#region <CONSTRUCTOR>
		public JogPanel_ModeSpeed(Form_Jog parentsInstance)
		{
			InitializeComponent();

			_parents = parentsInstance;
		}
		#endregion </CONSTRUCTOR>

		#region <FILED>
		Form_Jog _parents = null;
		#endregion </FILED>

		#region <UI EVENT>
		private void Click_ProgressBar(object sender, EventArgs e)
		{
			_parents.ChangeSpeedRatio((int)pb_speed.Tick);
		}
		private void Click_DefinedSpeed(object sender, EventArgs e)
		{
			int speedRatio;
			if (sender == btn_Define_20) speedRatio = 20;
			else if (sender == btn_Define_40) speedRatio = 40;
			else if (sender == btn_Define_60) speedRatio = 60;
			else if (sender == btn_Define_80) speedRatio = 80;
			else if (sender == btn_Define_100) speedRatio = 100;
			else return;

			_parents.ChangeSpeedRatio(speedRatio);
			pb_speed.Tick = (uint)speedRatio;
		}
		#endregion </UI EVENT>

		#region Property
		public int Tick 
		{
			get { return (int)pb_speed.Tick; }
			set
			{
				pb_speed.Tick = (uint)value;
				pb_speed.Invalidate();
			}
		}
		#endregion /Property

		#region Interface
		public void InitPrograssiveBar()
		{
			Tick = 50;
		}
		#endregion /Interface

	}
}
