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
	public partial class JogPanel_DpadExtend : UserControl
	{
		#region <CONSTRUCTOR>
		public JogPanel_DpadExtend(Form_Jog parentsInstance)
		{
			InitializeComponent();

			_parents = parentsInstance;
		}
		#endregion </CONSTRUCTOR>

		#region <FILED>
		Form_Jog _parents = null;
		#endregion </FILED>

		#region <UI EVENT>
		private void Down_Mouse(object sender, MouseEventArgs e)
		{
			bool clickLR = true;
			bool clickUD = true;
			bool directionLR = true;
			bool directionUD = true;
			bool extraAxis = false;

			if (sender == btn_Left || sender == btn_Right || sender == btn_ExtraLeft || sender == btn_ExtraRight)
				clickUD = false;
			else if (sender == btn_Up || sender == btn_Down || sender == btn_ExtraUp || sender == btn_ExtraDown)
				clickLR = false;

			if (sender == btn_Left || sender == btn_UpLeft || sender == btn_DownLeft || sender == btn_ExtraLeft)
				directionLR = false;
			if (sender == btn_Down || sender == btn_DownLeft || sender == btn_DownRight || sender == btn_ExtraDown)
				directionUD = false;

			if(sender == btn_ExtraUp || sender == btn_ExtraRight
				|| sender == btn_ExtraDown || sender == btn_ExtraLeft)
				extraAxis = true;

			_parents.Down_Mouse(clickLR, clickUD, directionLR, directionUD, extraAxis);
		}
		private void Up_Mouse(object sender, MouseEventArgs e)
		{
			_parents.Up_Mouse();
		}
		private void Click_Stop(object sender, EventArgs e)
		{
			_parents.Click_Stop();
		}
		#endregion </UI EVENT>

		#region <INTERFACE>

		public void SetAcitveMoveButtons(bool useX, bool useY, bool useExtraX, bool useExtraY)
		{
			btn_Stop.Enabled = useX || useY || useExtraX || useExtraY;

			if (useX)
			{
				btn_Left.Show();
				btn_Right.Show();
			}
			else
			{
				btn_Left.Hide();
				btn_Right.Hide();
			}

			if (useY)
			{
				btn_Up.Show();
				btn_Down.Show();
			}
			else
			{
				btn_Up.Hide();
				btn_Down.Hide();
			}

			if (useX && useY)
			{
				btn_UpLeft.Show();
				btn_UpRight.Show();
				btn_DownLeft.Show();
				btn_DownRight.Show();
			}
			else
			{
				btn_UpLeft.Hide();
				btn_UpRight.Hide();
				btn_DownLeft.Hide();
				btn_DownRight.Hide();
			}

			if(useExtraX)
			{
				btn_ExtraLeft.Show();
				btn_ExtraRight.Show();
			}
			else
			{
				btn_ExtraLeft.Hide();
				btn_ExtraRight.Hide();
			}

			if(useExtraY)
			{
				btn_ExtraUp.Show();
				btn_ExtraDown.Show();
			}
			else
			{
				btn_ExtraUp.Hide();
				btn_ExtraDown.Hide();
			}
		}
		#endregion </INTERFACE>
	}
}
