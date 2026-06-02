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
	public partial class JogPanel_Monitor : UserControl
	{
		#region <CONSTRUCTOR>
		public JogPanel_Monitor(Form_Jog parentsInstance)
		{
			InitializeComponent();

			_parents = parentsInstance;
		}
		#endregion </CONSTRUCTOR>

		#region <FILED>
		Form_Jog _parents = null;

		Dictionary<EN_JOG_DIRECTION, JogPanel_MonitorItem> _itemPanels = new Dictionary<EN_JOG_DIRECTION, JogPanel_MonitorItem>();
		#endregion </FILED>

		#region <INTERFACE>
		public void UpdateMonitorItem(Dictionary<EN_JOG_DIRECTION, JogMonitorItem> itemList)
		{
			panel_StackItem.Controls.Clear();
			_itemPanels.Clear();
			foreach(var kvp in itemList.Reverse())
			{
				_itemPanels.Add(kvp.Key, new JogPanel_MonitorItem(_parents, kvp.Key, kvp.Value));
				panel_StackItem.Controls.Add(_itemPanels[kvp.Key]);
			}
		}
		public void UpdatePosition(Dictionary<EN_JOG_DIRECTION, double> command, Dictionary<EN_JOG_DIRECTION, double> actual)
		{
			foreach (EN_JOG_DIRECTION en in Enum.GetValues(typeof(EN_JOG_DIRECTION)))
			{
				if (false == _itemPanels.ContainsKey(en)) continue;
				if (false == command.ContainsKey(en) || false == actual.ContainsKey(en))
				{
					_itemPanels[en].CommandPosition = 0.0;
					_itemPanels[en].ActualPosition = 0.0;
				}
				else
				{
					_itemPanels[en].CommandPosition = command[en];
					_itemPanels[en].ActualPosition = actual[en];
				}
			}
		}

		public void UpdateParameterValue(EN_JOG_DIRECTION direction, double value)
		{
			if (false == _itemPanels.ContainsKey(direction)) return;

			_itemPanels[direction].ParameterValue = value;
		}
		public bool TryGetParameterValue(EN_JOG_DIRECTION direction, out double result)
		{
			if (false == _itemPanels.ContainsKey(direction)
				|| direction == EN_JOG_DIRECTION.ExtraX
				|| direction == EN_JOG_DIRECTION.ExtraY)
			{
				result = 0.0;
				return false;
			}

			result = _itemPanels[direction].ParameterValue;
			return true;
		}
		#endregion </INTERFACE>

		public class JogMonitorItem
		{
			#region <CONSTRUCTOR>
			public JogMonitorItem(int axisNo, string axisName, bool isExtraAxis = false)
			{
				AxisNo = axisNo;
				AxisName = axisName;

				IsExtraAxis = isExtraAxis;

				IsParameterMode = false;
				ParameterName = string.Empty;
			}
			public JogMonitorItem(
			int axisNo
			, string axisName
			, string parameterName)
			{
				AxisNo = axisNo;
				AxisName = axisName;

				IsExtraAxis = false;

				IsParameterMode = true;
				ParameterName = parameterName;
			}
			#endregion </CONSTRUCTOR>

			#region Property
			public int AxisNo { get; set; }
			public string AxisName { get; set; }
			public bool IsExtraAxis { get; set; }
			public bool IsParameterMode { get; set; }
			public string ParameterName { get; set; }
			#endregion /Property
		}
	}
}
