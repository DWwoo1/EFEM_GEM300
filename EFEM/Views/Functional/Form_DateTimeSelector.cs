using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using FrameOfSystem3.Views.Functional.TimeSelector;

namespace FrameOfSystem3.Views.Functional
{
	public partial class Form_DateTimeSelector : Form
	{
		#region singleton
		private static Form_DateTimeSelector _Instance = new Form_DateTimeSelector();
		public static Form_DateTimeSelector GetInstance()
		{
			return _Instance;
		}
		private Form_DateTimeSelector()
		{
			this.DoubleBuffered = true;

			InitializeComponent();

			this.Size = new Size(this.Size.Width, this.Size.Height + this.PreferredSize.Height);

			timer_DragMode.Stop();
			timer_DragMode.Interval = 10;
			timer_DragMode.Tick += Tick_DragMode;
		}
		#endregion /singleton

		#region const
		const int WIDTH_MARGIN = 20;
		#endregion /const

		#region field
		Dictionary<EDialType, TimePanel_Dial> _currentDials = new Dictionary<EDialType, TimePanel_Dial>();
		bool _disableKeyUpEvent = false;
		bool _disableClickValue = false;
		EDialType _typeForDragMode = EDialType.Day;
		int _dragModeLastY = 0;
		#endregion /field

		#region property
		public enum EShowType
		{ 
			Full,
			Day,
			Time_WithoutMs,
			Time_WithMs,
			Hour,
			Minute,
			Second,
			Millisecond,
		}
		public enum EDialType
		{
			Day,
			Hour,
			Minute,
			Second,
			Millisecond,
		}
		#endregion /property

		#region interface
		public bool CreateForm(EShowType type = EShowType.Full, string strTitle = "")
		{
			return CreateForm(TimeSpan.Zero, type, strTitle);
		}
		public bool CreateForm(TimeSpan preValue, EShowType type = EShowType.Full, string strTitle = "")
		{
			panel_Dials.Controls.Clear();
			_currentDials.Clear();

			Action<EDialType> AddType = (e) => { _currentDials.Add(e, new TimePanel_Dial(this, e, GetUnit(e))); };

			switch (type)
			{
				case EShowType.Full:
					AddType(EDialType.Day);
					AddType(EDialType.Hour);
					AddType(EDialType.Minute);
					AddType(EDialType.Second);
					AddType(EDialType.Millisecond);
					break;
				case EShowType.Day:
					AddType(EDialType.Day);
					break;
				case EShowType.Time_WithoutMs:
					AddType(EDialType.Hour);
					AddType(EDialType.Minute);
					AddType(EDialType.Second);
					break;
				case EShowType.Time_WithMs:
					AddType(EDialType.Hour);
					AddType(EDialType.Minute);
					AddType(EDialType.Second);
					AddType(EDialType.Millisecond);
					break;
				case EShowType.Hour:
					AddType(EDialType.Hour);
					break;
				case EShowType.Minute:
					AddType(EDialType.Minute);
					break;
				case EShowType.Second:
					AddType(EDialType.Second);
					break;
				case EShowType.Millisecond:
					AddType(EDialType.Millisecond);
					break;
				default:
					return false;
			}

			Action<EDialType, int> applyPreValue = (t, v) =>
			{
				if (v <= 0) return;
				if (false == _currentDials.ContainsKey(t))
					AddType(t);

				_currentDials[t].Value = v;
			};
			applyPreValue(EDialType.Day, preValue.Days);
			applyPreValue(EDialType.Hour, preValue.Hours);
			applyPreValue(EDialType.Minute, preValue.Minutes);
			applyPreValue(EDialType.Second, preValue.Seconds);
			applyPreValue(EDialType.Millisecond, preValue.Milliseconds);

			int minKey = (int)EDialType.Millisecond, maxKey = (int)EDialType.Day;
			foreach(int en in _currentDials.Keys.Select(k => (int)k))
			{
				if (minKey > en) minKey = en;
				if (maxKey < en) maxKey = en;
			}
			for (int i = minKey; i < maxKey; ++i)
			{
				var en = (EDialType)i;
				if (false == _currentDials.ContainsKey(en))
					AddType(en);
			}

			Size newSize = new Size(panel_Buttons.Size.Width + WIDTH_MARGIN, this.Size.Height);
			foreach(var kvp in _currentDials.OrderByDescending(p => p.Key))
			{
				panel_Dials.Controls.Add(kvp.Value);
				newSize.Width += kvp.Value.Size.Width;
			}

			this.Size = newSize;

			UpdateUi();
			this.ShowDialog();

			if (this.DialogResult == System.Windows.Forms.DialogResult.OK)
				return true;

			return false;
		}
		public void GetResult(ref TimeSpan result)
		{
			result = TimeSpan.Zero;
			foreach(var kvp in _currentDials)
			{
				switch (kvp.Key)
				{
					case EDialType.Day:			result += new TimeSpan(kvp.Value.Value, 0, 0, 0); break;
					case EDialType.Hour:		result += new TimeSpan(kvp.Value.Value, 0, 0); break;
					case EDialType.Minute:		result += new TimeSpan(0, kvp.Value.Value, 0); break;
					case EDialType.Second:		result += new TimeSpan(0, 0, kvp.Value.Value); break;
					case EDialType.Millisecond: result += new TimeSpan(0, 0, 0, 0, kvp.Value.Value); break;
					default:
						break;
				}
			}
		}
		public void GetResult(ref string result)
		{
			TimeSpan ts = TimeSpan.Zero;
			GetResult(ref ts);
			result = ts.ToString();
		}
		#endregion /interface

		#region click event
		public void Clicked_Value(EDialType type)
		{
			if (_disableClickValue)
				return;

			DragModeEnd();
			_disableKeyUpEvent = true;
			try
			{
				var calculator = Form_Calculator.GetInstance();
				int max = GetLimit(type);
				if (false == calculator.CreateForm("0", "0", max.ToString(), GetUnit(type), type.ToString()))
					return;

				int value = 0;
				calculator.GetResult(ref value);

				if (value > GetLimit(type))
					return;

				_currentDials[type].Value = value;
				UpdateUi();
			}
			finally
			{
				_disableKeyUpEvent = false;
			}
		}
		public void Clicked_Up(EDialType type)
		{
			CountUp(type);
			UpdateUi();
		}
		public void Clicked_Down(EDialType type)
		{
			CountDown(type);
			UpdateUi();
		}
		public void Clicked_DragModeStart(EDialType type)
		{
			DragModeStart(type);
		}
		public void Clicked_DragModeEnd()
		{
			DragModeEnd();
		}

		private void Click_Apply(object sender, EventArgs e)
		{
			this.DialogResult = System.Windows.Forms.DialogResult.OK;
		}
		private void Click_Cancel(object sender, EventArgs e)
		{
			this.DialogResult = System.Windows.Forms.DialogResult.Cancel;
		}
		private void Click_Zero(object sender, EventArgs e)
		{
			SetZero();
			UpdateUi();
		}
		private void Form_EditNumber_KeyDown(object sender, KeyEventArgs e)
		{
			if (_disableKeyUpEvent)
				return;

			int nKeyCode = (int)e.KeyCode;

			switch (e.KeyCode)
			{
				case Keys.Escape: // Esc 입력 시
				case Keys.Back: // 백스페이스 입력 시
					Click_Cancel(null, null);
					break;
				case Keys.Enter: // 엔터 입력 시
					Click_Apply(null, null);
					break;
				default:
					break;
			}
		}
		#endregion /click event

		#region method
		private void SetZero()
		{
			foreach (var kvp in _currentDials)
			{
				kvp.Value.Value = 0;
			}
		}

		private void CountUp(EDialType type)
		{
			if (false == _currentDials.ContainsKey(type))
				return;

			int current = _currentDials[type].Value;

			if (current >= GetLimit(type))
			{
				if (type == EDialType.Day)
					return;

				EDialType nextType = (EDialType)((int)type - 1);

				if (false == _currentDials.ContainsKey(nextType))
				{
					_currentDials.Add(nextType, new TimePanel_Dial(this, nextType, GetUnit(nextType)));
					panel_Dials.Controls.Add(_currentDials[nextType]);
					this.Size = new Size(this.Size.Width + _currentDials[nextType].Size.Width, this.Size.Height);
				}

				CountUp(nextType);
				_currentDials[type].Value = 0;
			}
			else
			{
				_currentDials[type].Value++;
			}
		}
		private bool CountDown(EDialType type)
		{
			if (false == _currentDials.ContainsKey(type))
				return false;

			int current = _currentDials[type].Value;
			if (current <= 0)
			{
				EDialType nextType = (EDialType)((int)type - 1);
				if (false == _currentDials.ContainsKey(nextType) || false == CountDown(nextType))
				{
					_currentDials[type].Value = 0;
					return false;
				}

				_currentDials[type].Value = GetLimit(type);
				return true;
			}

			--_currentDials[type].Value;
			return true;
		}

		private void UpdateUi()
		{
			foreach (var kvp in _currentDials)
			{
				kvp.Value.UpdateUi();
			}
		}
		private int GetLimit(EDialType type)
		{
			switch (type)
			{
				case EDialType.Day: return int.MaxValue;
				case EDialType.Hour: return 23;
				case EDialType.Minute: 
				case EDialType.Second: return 59;
				case EDialType.Millisecond: return 999;
				default: return -1;
			}
		}
		private string GetUnit(EDialType type)
		{
			switch (type)
			{
				case Form_DateTimeSelector.EDialType.Minute: return "Min";
				case Form_DateTimeSelector.EDialType.Second: return "Sec";
				case Form_DateTimeSelector.EDialType.Millisecond: return "Ms";
				default: return type.ToString();
			}
		}

		private void DragModeStart(EDialType type)
		{
			_typeForDragMode = type;
			_dragModeLastY = Cursor.Position.Y;
			timer_DragMode.Start();
		}
		private void DragModeEnd()
		{
			_disableClickValue = false;
			timer_DragMode.Stop();
		}
		private void Tick_DragMode(object sender, EventArgs e)
		{
			int currentY = Cursor.Position.Y;
			int triggerGap = 5;
			if(_dragModeLastY < currentY)
			{
				if (currentY - _dragModeLastY < triggerGap)
					return;

				_disableClickValue = true;
				CountDown(_typeForDragMode);
			}
			else
			{
				if (_dragModeLastY - currentY < triggerGap)
					return;

				_disableClickValue = true;
				CountUp(_typeForDragMode);
			}

			_dragModeLastY = currentY;
			UpdateUi();
		}
		private void Form_DateTimeSelector_FormClosing(object sender, FormClosingEventArgs e)
		{
			DragModeEnd();
		}
		#endregion /method
	}
}
