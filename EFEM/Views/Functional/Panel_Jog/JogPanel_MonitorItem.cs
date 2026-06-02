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

namespace FrameOfSystem3.Views.Functional.Jog
{
	public partial class JogPanel_MonitorItem : UserControl
	{
		#region Constructor
		public JogPanel_MonitorItem(Form_Jog parentsInstance, EN_JOG_DIRECTION direction, JogPanel_Monitor.JogMonitorItem datas)
		{
			InitializeComponent();

			this.Dock = DockStyle.Top;

			_parents = parentsInstance;

			_isExtraAxis = datas.IsExtraAxis;
			_isParameterMode = datas.IsParameterMode;

			lbl_AxisName.Text = datas.AxisName;
			lbl_AxisName.SubText = direction.ToString();
			_direction = direction;

			if (false == _isExtraAxis)
			{
				if (_isParameterMode)
				{
					lbl_ParameterName.Text = datas.ParameterName;
				}
				else
				{
					lbl_ParameterName.Text = "ABSOLUTE DESTINATION";
				}

				lbl_ParameterValue.Text = _parameterValue.ToString("F3");
			}
			else
			{
				lbl_ParameterName.Visible = false;
				lbl_ParameterName.Enabled = false;
				lbl_ParameterValue.Visible = false;
				lbl_ParameterValue.Enabled= false;
				btn_Set.Visible = false;
				btn_Set.Enabled = false;
				btn_Go.Visible = false;
				btn_Go.Enabled = false;
			}
		}
		#endregion /Constructor

		#region Filed
		Form_Jog _parents = null;

		bool _isExtraAxis = false;
		bool _isParameterMode = false;
		double _parameterValue = 0.0;

		bool _isMeasureMode = false;
		double _measureModeOffset = 0.0;

		EN_JOG_DIRECTION _direction;
		#endregion /Filed

		#region UI event
		private void Click_Measure(object sender, EventArgs e)
		{
			if(false == _isMeasureMode)
			{
				double command;
				if (false == double.TryParse(lbl_Command.Text, out command))
				{
					_measureModeOffset = 0.0;
					return;
				}

				_measureModeOffset = command;
			}
			else
			{
				_measureModeOffset = 0.0;
			}

			_isMeasureMode = !_isMeasureMode;
			btn_Measure.ButtonClicked = _isMeasureMode;
		}
		private void Click_Set(object sender, EventArgs e)
		{
			if (_isExtraAxis) return;

			if(_isParameterMode)
			{
				_parents.SetParameter(_direction, true);
			}
			else
			{
				_parents.SetAbsDestination(_direction, true);
			}
		}
		private void Click_Go(object sender, EventArgs e)
		{
			if (_isExtraAxis) return;

			_parents.GoAbsDestination(_direction);
		}
		private void Click_ParameterValue(object sender, EventArgs e)
		{
			if (_isExtraAxis) return;

			if (_isParameterMode)
			{
				_parents.SetParameter(_direction, false);
			}
			else
			{
				_parents.SetAbsDestination(_direction, false);
			}
		}
		#endregion /UI event

		#region Property
		public double CommandPosition { set { lbl_Command.Text = value.ToString("F3"); } }
		public double ActualPosition
		{
			set
			{
				double result = value - _measureModeOffset;
				lbl_Actual.Text = result.ToString("F3");
			} 
		}
		public double ParameterValue 
		{
			get { return _parameterValue; }
			set 
			{
				_parameterValue = value;
				lbl_ParameterValue.Text = value.ToString("F3"); 
			} 
		}
		#endregion /Property
	}
}
