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
using FrameOfSystem3.Recipe;

namespace FrameOfSystem3.Views.Functional.TimeSelector
{
	public partial class TimePanel_Dial : UserControl
	{
		#region Constructor
		public TimePanel_Dial(Form_DateTimeSelector parentsInstance, Form_DateTimeSelector.EDialType dialType, string unit)
		{
			InitializeComponent();

			this.Dock = DockStyle.Left;

			_parents = parentsInstance;
			_myType = dialType;

			Func<Control, Size> GrowSize = (c) => { return new Size(c.Size.Width + 10, c.Size.Height); };

			lbl_Type.Text = unit;

			Value = 0;

			timerClicked.Interval = 1;
			timerClicked.Tick += Execute;
		}
		#endregion /Constructor

		#region const
		const uint FIRST_INTERVAL_TIME = 500;
		const int STEP_INTERVAL = 2;	// 2번에 한번씩 속도 up
		#endregion /const

		#region Filed
		Form_DateTimeSelector _parents = null;
		Form_DateTimeSelector.EDialType _myType;

		DateTime _startTime = DateTime.Now;
		uint _reactInterval = FIRST_INTERVAL_TIME;
		TickCounter_.TickCounter _tickInterval = new TickCounter_.TickCounter();
		int _speedUpStep = 0;
		Action<Form_DateTimeSelector.EDialType> _funcUpDownAction = null;
		#endregion /Filed

		public int Value { get; set; }

		#region UI event
		private void Execute(object sender, EventArgs e)
		{
			if (false == _tickInterval.IsTickOver(true))
				return;

			_funcUpDownAction(_myType);

			if(_speedUpStep < STEP_INTERVAL)
			{
				++_speedUpStep;
			}
			else
			{
				_reactInterval = (uint)(_reactInterval * 0.5);
				_speedUpStep = 0;
			}

			_tickInterval.SetTickCount(_reactInterval);
		}

		private void btn_MouseDown(object sender, MouseEventArgs e)
		{
			if (sender == btn_Down)
			{
				_funcUpDownAction = _parents.Clicked_Down;
			}
			else if (sender == btn_Up)
			{
				_funcUpDownAction = _parents.Clicked_Up;
			}
			else return;

			_startTime = DateTime.Now;
			_reactInterval = FIRST_INTERVAL_TIME;
			_speedUpStep = 0;
			_tickInterval.SetTickCount(1);
			timerClicked.Start();
		}
		private void btn_MouseUp(object sender, MouseEventArgs e)
		{
			timerClicked.Stop();
		}
		private void Click_Value(object sender, EventArgs e)
		{
			_parents.Clicked_Value(_myType);
		}
		private void Click_DragModeStart(object sender, MouseEventArgs e)
		{
			_parents.Clicked_DragModeStart(_myType);
		}
		private void Click_DragModeEnd(object sender, MouseEventArgs e)
		{
			_parents.Clicked_DragModeEnd();
		}
		#endregion /UI event

		#region interface
		public void UpdateUi()
		{
			lbl_Value.Text = Value.ToString();
		}
		#endregion /interface

	}
}
