using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using FrameOfSystem3.Recipe;

using Define.DefineEnumBase.Common;
using Define.DefineEnumProject.Motion;

namespace FrameOfSystem3.Views.Functional.Jog
{
    extern alias MotionInstance;

	#region 열거형
	public enum EN_MOTION_TYPE
	{
		SPEED,
		RELATIVE,
	}
	public enum EN_JOG_DIRECTION
	{
		X,
		Y,
		ExtraX,
		ExtraY,
	}
	#endregion

	// 2025.05.23. by shkim. [MOD] Parameter or Axis Name에 맞춰 Form 크기 가변되도록 개선
    public partial class Form_Jog : Form
	{
		#region Constructor
		private Form_Jog()
		{
			InitializeComponent();

			_panel_DpadNormal = new JogPanel_DpadNormal(this);
			_panel_DpadExtend = new JogPanel_DpadExtend(this);
			_panel_ModeRelative = new JogPanel_ModeRelative(this);
			_panel_ModeSpeed = new JogPanel_ModeSpeed(this);
			_panel_ItemList = new JogPanel_ItemList(this);
			_panel_Monitor = new JogPanel_Monitor(this);

			_calculator = Form_Calculator.GetInstance();
			_messageBox = Form_MessageBox.GetInstance();
			_configJog = FrameOfSystem3.Config.ConfigJog.GetInstance();

            _interlock = Interlock_.Interlock.GetInstance();
            _configinterlock = FrameOfSystem3.Config.ConfigInterlock.GetInstance();
			_motion = FrameOfSystem3.Config.ConfigMotion.GetInstance();

			// 2021.08.02 by junho [ADD] Add user friendly function
			_recipe = FrameOfSystem3.Recipe.Recipe.GetInstance();

			_timerForUpdate = new Timer();
			_timerForUpdate.Interval = 300;
			_timerForUpdate.Tick += new EventHandler(Execute);

			InitializeSubPanels();

            DEFAULT_REDUCE_PANEL_WIDTH = _panelJogMain.Width;
        }
        private void InitializeSubPanels()
		{
			panel_Jog.Controls.Add(_panel_DpadNormal);
			panel_Jog.Controls.Add(_panel_DpadExtend);
			_panel_DpadExtend.Hide();

            _panel_DpadNormal.Anchor = AnchorStyles.Left | AnchorStyles.Right;
            _panel_DpadExtend.Anchor = AnchorStyles.Left | AnchorStyles.Right;

            panel_Mode.Controls.Add(_panel_ModeSpeed);
			panel_Mode.Controls.Add(_panel_ModeRelative);
			_panel_ModeRelative.Hide();

            _panel_ModeSpeed.Anchor = AnchorStyles.Left | AnchorStyles.Right;
            _panel_ModeRelative.Anchor = AnchorStyles.Left | AnchorStyles.Right;

            _flowLayoutPanel_Main.Controls.Add(_panel_ItemList);
            _flowLayoutPanel_Main.Controls.Add(_panel_Monitor);
            _panel_Monitor.Hide();
		}
        #endregion /Constructor

        readonly int DEFAULT_MONITOR_PANEL_WIDTH = 513;
        readonly int DEFAULT_REDUCE_PANEL_WIDTH = 0;
        readonly int MARGIN_FOR_TEXT = 10;

        #region <FILED>

        #region Instance
        FrameOfSystem3.Config.ConfigJog _configJog = null;
		FrameOfSystem3.Config.ConfigMotion _motion = null;
        FrameOfSystem3.Recipe.Recipe _recipe = null;
        FrameOfSystem3.Config.ConfigInterlock _configinterlock = null;

        Interlock_.Interlock _interlock = null;

		Form_Calculator _calculator = null;
		Form_MessageBox _messageBox = null;
		#endregion /Instance

		#region Panel
		private JogPanel_DpadNormal _panel_DpadNormal = null;
		private JogPanel_DpadExtend _panel_DpadExtend = null;

		private JogPanel_ModeRelative _panel_ModeRelative = null;
		private JogPanel_ModeSpeed _panel_ModeSpeed = null;

		private JogPanel_ItemList _panel_ItemList = null;
		private JogPanel_Monitor _panel_Monitor = null;
		#endregion /Panel

		#region Constant
		private const int DELAY_MOTION = 10;
		private const int RATIO_MOTION = 100;
		private const int SELECT_NONE = -1;

		private const int FORM_SIZE_REDUCE = 306;
		private const int FORM_SIZE_EXTEND_LIST = 707;
		private const int FORM_SIZE_EXTEND_MONITOR = 821;

		private readonly string MIN_OF_DISTANCE = "0";
		private readonly string MAX_OF_DISTANCE = "999999";

		private readonly string LEFT_ARROW = "◀";
		private readonly string RIGHT_ARROW = "▶";
		#endregion /Constant

		#region Common
		private int _selectedJogItem = -1;
		private string _selectedJogName = string.Empty;

		private bool _isParameterMode = false;

		private EN_EXTEND_PANEL _extendType = EN_EXTEND_PANEL.REDUCE;
		private EN_MOTION_TYPE _motionType = EN_MOTION_TYPE.SPEED;

		private bool _enableX = false;
		private bool _enableY = false;
		private bool _enableExtraX = false;
		private bool _enableExtraY = false;

		private int _axisNoX = -1;
		private int _axisNoY = -1;
		private int _axisNoExtraX = -1;
		private int _axisNoExtraY = -1;

		private bool _isReverseX = false;
		private bool _isReverseY = false;
		private bool _isReverseExtraX = false;
		private bool _isReverseExtraY = false;

		private string _recipeTypeX = string.Empty;
		private string _recipeTypeY = string.Empty;

		private string _parameterNameX = string.Empty;
		private string _parameterNameY = string.Empty;

		private int _motionSpeedRatio = 50;
		private double _relativeStep = 0.0;
		private double _selectedDefineStep = 0.1;

		private static Timer _timerForUpdate = null;
		Dictionary<int, IPointXY> _axisList = new Dictionary<int, IPointXY>();
		#endregion /Common

		#endregion </FILED>

		#region <PROPERTY>
		private int SelectedJogItem
		{
			get { return _selectedJogItem; }
			set
			{
				_selectedJogItem = value;

				string jogName = string.Empty;
				if (value == SELECT_NONE)
				{
					lbl_SelectedName.Text = string.Empty;
				}
				else if (_configJog != null)
				{
					if (false == _configJog.GetParameter(value, FrameOfSystem3.Config.ConfigJog.EN_PARAM_JOG.NAME, ref jogName))
						jogName = string.Empty;
				}
				SelectedJogName = jogName;
			}
		}
		private string SelectedJogName
		{
			get { return _selectedJogName; }
			set
			{
				_selectedJogName = value;
				lbl_SelectedName.Text = value;
			}
		}
		private int MotionSpeedRatio
		{
			get { return _motionSpeedRatio; }
			set
			{
				_motionSpeedRatio = value;
				_panel_ModeSpeed.Tick = value;
			}
		}
		private double RelativeStep
		{
			get { return _relativeStep; }
			set
			{
				_relativeStep = value;

				if (_panel_ModeRelative != null)
				{
					_panel_ModeRelative.DistanceLabelText = value.ToString("F3");
				}
			}
		}
		private EN_MOTION_TYPE MotionType
		{
			get { return _motionType; }
			set
			{
				_motionType = value;
				SetModePanel(value);
			}
		}
		#endregion </PROPERTY>

		#region 싱글톤
		private static Form_Jog _Instance	= null;
		public static Form_Jog GetInstance()
		{
            if (_Instance == null)
                _Instance = new Form_Jog();
			return _Instance;
		}
		#endregion

		#region <INTERFACE>
		public void CreateForm()
		{
			_isParameterMode = false;
			btn_Extend.Visible = false;

            ResetControlValue();
			UpdateJogItemOnGrid();
			SetExtendPanel(EN_EXTEND_PANEL.ITEM_LIST);

			_timerForUpdate.Start();

			this.Show();
		}
		public void CreateForm(
			string selectedJogName
			, string xRecipeType
			, string xParameterName
			, int xSelectedAxis

			, string yRecipeType
			, string yParameterName
			, int ySelectedAxis)
		{
			_isParameterMode = true;
			btn_Extend.Visible = true;

            ResetControlValue();

			SelectedJogName = selectedJogName;

			if (xSelectedAxis != SELECT_NONE)
			{
				_enableX = true;
				_axisNoX = xSelectedAxis;
				_recipeTypeX = xRecipeType;
				_parameterNameX = xParameterName;
			}

			if (ySelectedAxis != SELECT_NONE)
			{
				_enableY = true;
				_axisNoY = ySelectedAxis;
				_recipeTypeY = yRecipeType;
				_parameterNameY = yParameterName;
			}

			UpdatePanelsAtParameterMode();

            SetExtendPanel(EN_EXTEND_PANEL.MONITOR);

			_timerForUpdate.Start();

			this.Show();
		}
		private void UpdateJogItemOnGrid()
		{
			int[] arrOfItem = null;

			if (false == _configJog.GetListOfItem(ref arrOfItem))
				return;

			_axisList.Clear();
			var itemList = new Dictionary<int, string>();
			int nCountOfItem = arrOfItem.Length;
			for (int index = 0; index < nCountOfItem; ++index)
			{
				string strValue = string.Empty;
				if (false == _configJog.GetParameter(index, FrameOfSystem3.Config.ConfigJog.EN_PARAM_JOG.NAME, ref strValue))
					strValue = string.Empty;

				itemList.Add(index, strValue);

				int x = -1, y = -1;
				if (false == _configJog.GetParameter(index, FrameOfSystem3.Config.ConfigJog.EN_PARAM_JOG.INDEX_LEFT_RIGHT, ref x)) x = -1;
				if (false == _configJog.GetParameter(index, FrameOfSystem3.Config.ConfigJog.EN_PARAM_JOG.INDEX_UP_DOWN, ref y)) y = -1;
				_axisList.Add(index, new IPointXY(x, y));
			}

			_panel_ItemList.UpdateJogItemOnGrid(itemList);
		}

		private void UpdatePanelsAtParameterMode()
		{
			SetJogPanel(_enableX, _enableY, _enableExtraX, _enableExtraY);

            // 2025.05.23. by shkim. [DEL] 불필요코드 (아래 Action에서 처리중)
            //UpdateMonitorPanel(_axisNoX, _axisNoY, _axisNoExtraX, _axisNoExtraY);
            // 2025.05.23. by shkim. [END]

            SetModePanel(_motionType);

			var monitorItemList = new Dictionary<EN_JOG_DIRECTION, JogPanel_Monitor.JogMonitorItem>();

            Action<EN_JOG_DIRECTION, int, string> AddMonitorItem = (direction, axisNo, parameterName) =>
            {
                #region
                if (axisNo != SELECT_NONE)
                {
                    string axisName = _configJog.GetAxisNameByIndex(axisNo);
                    monitorItemList.Add(direction, new JogPanel_Monitor.JogMonitorItem(axisNo, axisName, parameterName));
                }
                #endregion
            };

            AddMonitorItem(EN_JOG_DIRECTION.X, _axisNoX, _parameterNameX);
            AddMonitorItem(EN_JOG_DIRECTION.Y, _axisNoY, _parameterNameY);

            _panel_Monitor.UpdateMonitorItem(monitorItemList);

            double currentValue;
			string strValue;
			if(_axisNoX != SELECT_NONE)
			{
				strValue = GetRecipeValue(_recipeTypeX, _parameterNameX);
				if (strValue != string.Empty)
				{
					currentValue = Math.Round(double.Parse(strValue), 3);
					_panel_Monitor.UpdateParameterValue(EN_JOG_DIRECTION.X, currentValue);
				}
			}
			if(_axisNoY != SELECT_NONE)
			{
				strValue = GetRecipeValue(_recipeTypeY, _parameterNameY);
				if (strValue != string.Empty)
				{
					currentValue = Math.Round(double.Parse(strValue), 3);
					_panel_Monitor.UpdateParameterValue(EN_JOG_DIRECTION.Y, currentValue);
				}
			}
		}

		#region from panels

		#region dpad
		public void Down_Mouse(bool clickLR, bool clickUD, bool directionLR, bool directionUD, bool extraAxis)
		{
			// 2023.04.06 by junho [ADD] interlock
			if (false == IsMachineIdleState()) return;

			if (extraAxis)
			{
				if (_isReverseExtraX) directionLR = !directionLR;
				if (_isReverseExtraY) directionUD = !directionUD;
			}
			else
			{
				if (_isReverseX) directionLR = !directionLR;
				if (_isReverseY) directionUD = !directionUD;
			}

			switch (_motionType)
			{
				case EN_MOTION_TYPE.SPEED:
					if (false == CheckInterlockCurrentPostion(true))
						return;

					if (clickLR)
					{
						_motion.MoveAtSpeed(extraAxis ? _axisNoExtraX : _axisNoX
							, directionLR, _motionSpeedRatio);
					}

					if (clickUD)
					{
						_motion.MoveAtSpeed(extraAxis ? _axisNoExtraY : _axisNoY
							, directionUD, _motionSpeedRatio);
					}
					break;
				case EN_MOTION_TYPE.RELATIVE:
					if (clickLR)
					{
						double destination = directionLR ? _relativeStep : (-1 * _relativeStep);
						_motion.MoveReleatively(extraAxis ? _axisNoExtraX : _axisNoX
                            , destination, Motion_.MOTION_SPEED_CONTENT.RUN, RATIO_MOTION, DELAY_MOTION);
					}

					if (clickUD)
					{
						double destination = directionUD ? _relativeStep : (-1 * _relativeStep);
						_motion.MoveReleatively(extraAxis ? _axisNoExtraY : _axisNoY
                            , destination, Motion_.MOTION_SPEED_CONTENT.RUN, RATIO_MOTION, DELAY_MOTION);
					}
					break;
			}
		}
		public void Up_Mouse()
		{
			if (EN_MOTION_TYPE.RELATIVE == _motionType)
				return;

			Click_Stop();
		}
		public void Click_Stop()
		{
			if (_axisNoX != SELECT_NONE)
			{
				_motion.StopMotion(_axisNoX, false);
			}

			if (_axisNoY != SELECT_NONE)
			{
				_motion.StopMotion(_axisNoY, false);
			}

			if(_axisNoExtraX != SELECT_NONE)
			{
				_motion.StopMotion(_axisNoExtraX, false);
			}

			if (_axisNoExtraY != SELECT_NONE)
			{
				_motion.StopMotion(_axisNoExtraY, false);
			}
		}
		#endregion /dpad

		#region mode
		public void Click_Distance(string oldValue)
		{
			if (_calculator.CreateForm(oldValue, MIN_OF_DISTANCE, MAX_OF_DISTANCE))
			{
				double result = 0;
				_calculator.GetResult(ref result);
				RelativeStep = result;
			}
		}
		public void Click_DefinedStep(double step)
		{
			_selectedDefineStep = step;
			RelativeStep = step;
		}
		public void Click_FindControl(EN_UP_DOWN category)
		{
			double distance = category == EN_UP_DOWN.UP ? _selectedDefineStep : -1 * _selectedDefineStep;
			RelativeStep = RelativeStep + distance;
		}

		public void ChangeSpeedRatio(int speedRatio)
		{
			MotionSpeedRatio = speedRatio;
		}
		#endregion /mode

		#region monitor
		public void SelectJogItem(int selectNo)
		{
			SelectedJogItem = selectNo;
			UpdateParameter(selectNo);

            // 2025.05.23. by shkim. [DEL] 불필요코드 (ItemList에서는 MonitorPanel을 업데이트 하지 않음)
            // UpdateMonitorPanel(_axisNoX, _axisNoY, _axisNoExtraX, _axisNoExtraY);
            // 2025.05.23. by shkim. [END]

            SetJogPanel(_enableX, _enableY, _enableExtraX, _enableExtraY);
			SetModePanel(_motionType);

			ResizeReducePanelWidth();
        }
		public void SetParameter(EN_JOG_DIRECTION direction, bool isButton)
		{
			if (false == _isParameterMode) return;

			int axisNo = -1;
			string recipeType = string.Empty;
			string parameterName = string.Empty;
			string oldValue = string.Empty;

			switch (direction)
			{
				case EN_JOG_DIRECTION.X:
					axisNo = _axisNoX;
					recipeType = _recipeTypeX;
					parameterName = _parameterNameX;
					break;
				case EN_JOG_DIRECTION.Y:
					axisNo = _axisNoY;
					recipeType = _recipeTypeY;
					parameterName = _parameterNameY;
					break;
				default: return;
			}

			oldValue = GetRecipeValue(recipeType, parameterName);
			if (oldValue == string.Empty) oldValue = "0";	// return 시키지 않고 0으로 설정

			double newValue = 0.0;
			double min = double.MaxValue;
			double max = double.MinValue;
			EN_RECIPE_TYPE enType;
			if (Enum.TryParse(recipeType, out enType))
			{
				min = _recipe.GetValue(enType, parameterName, 0, EN_RECIPE_PARAM_TYPE.MIN, min);
				max = _recipe.GetValue(enType, parameterName, 0, EN_RECIPE_PARAM_TYPE.MAX, max);
			}
			else
			{
				min = _recipe.GetValue(recipeType, parameterName, 0, EN_RECIPE_PARAM_TYPE.MIN, min);
				max = _recipe.GetValue(recipeType, parameterName, 0, EN_RECIPE_PARAM_TYPE.MAX, max);
			}

			if (isButton)
			{
				newValue = _motion.GetCommandPosition(axisNo);
			}
			else
			{
				if (false == _calculator.CreateForm(oldValue, min.ToString("F3"), max.ToString("F3")))
					return;

				_calculator.GetResult(ref newValue);
			}

			newValue = Math.Round(newValue, 3);
			if (false == _messageBox.ShowMessage(string.Format("Do you want change parameter [{0}] ?\n{1} -> {2}"
				, parameterName, oldValue, newValue.ToString())))
				return;

			// 2023.09.13 by junho [ADD] limit 확인하도록 변경
			if (min > newValue || max < newValue)
			{
				_messageBox.ShowMessage(string.Format("Can not change position (Limit over) [{0}] ?\n{1} -> {2}"
					, parameterName, oldValue, newValue.ToString()));
				return;
			}

			FrameOfSystem3.Recipe.EN_RECIPE_TYPE type;
			if (false == Enum.TryParse(recipeType, out type) || type == FrameOfSystem3.Recipe.EN_RECIPE_TYPE.PROCESS)
			{
				if (false == _recipe.SetValue(recipeType, parameterName, 0, FrameOfSystem3.Recipe.EN_RECIPE_PARAM_TYPE.VALUE, newValue.ToString()))
					return;

				if (_recipe.GetValue(recipeType, parameterName, 0, FrameOfSystem3.Recipe.EN_RECIPE_PARAM_TYPE.DATA_TYPE, string.Empty) == string.Empty)
				{
					_recipe.SetValue(recipeType, parameterName, 0, FrameOfSystem3.Recipe.EN_RECIPE_PARAM_TYPE.DATA_TYPE, FrameOfSystem3.Recipe.EN_DATA_TYPE.FLOAT8.ToString());
				}
			}
			else
			{
				if (false == _recipe.SetValue(type, parameterName, 0, FrameOfSystem3.Recipe.EN_RECIPE_PARAM_TYPE.VALUE, newValue.ToString()))
					return;

				if (_recipe.GetValue(type, parameterName, 0, FrameOfSystem3.Recipe.EN_RECIPE_PARAM_TYPE.DATA_TYPE, string.Empty) == string.Empty)
				{
					_recipe.SetValue(type, parameterName, 0, FrameOfSystem3.Recipe.EN_RECIPE_PARAM_TYPE.DATA_TYPE, FrameOfSystem3.Recipe.EN_DATA_TYPE.FLOAT8.ToString());
				}
			}

			_panel_Monitor.UpdateParameterValue(direction, newValue);
		}
		public void SetAbsDestination(EN_JOG_DIRECTION direction, bool isButton)
		{
			int axisNo = -1;
			string oldValue = string.Empty;

			switch (direction)
			{
				case EN_JOG_DIRECTION.X:
					axisNo = _axisNoX;
					break;
				case EN_JOG_DIRECTION.Y:
					axisNo = _axisNoY;
					break;
				default: return;
			}

			double newValue = 0.0;
			if (isButton)
			{
				newValue = _motion.GetCommandPosition(axisNo);
			}
			else
			{
				if (false == _calculator.CreateForm(oldValue, MIN_OF_DISTANCE, MAX_OF_DISTANCE))
					return;

				_calculator.GetResult(ref newValue);
			}
			_panel_Monitor.UpdateParameterValue(direction, newValue);
		}
		public void GoAbsDestination(EN_JOG_DIRECTION direction)
		{
			double destination;
			if (false == _panel_Monitor.TryGetParameterValue(direction, out destination))
				return;

			int axisNo = -1;
			switch (direction)
			{
				case EN_JOG_DIRECTION.X:
					axisNo = _axisNoX;
					break;
				case EN_JOG_DIRECTION.Y:
					axisNo = _axisNoY;
					break;
				default: return;
			}
			if (axisNo == -1) return;

			// 2023.04.06 by junho [ADD] interlock
			if (false == IsMachineIdleState()) return;

			if (false == ConfirmButtonClick("MOVE")) return;

			// 2024.04.11. by shkim [MOD] RUN Speed는 너무 빠름...
            // _motion.MoveAbsolutely(axisNo, destination, Motion_.MOTION_SPEED_CONTENT.RUN, RATIO_MOTION, DELAY_MOTION);
            _motion.MoveAbsolutely(axisNo, destination, Motion_.MOTION_SPEED_CONTENT.JOG_HIGH, RATIO_MOTION, DELAY_MOTION);

		}
		#endregion /monitor

		#endregion /from panels

		#endregion </INTERFACE>

		#region <METHOD>
		/// <summary>
		/// 2020.08.12 by twkang [ADD] 컨트롤들의 값을 초기화한다.
		/// </summary>
		private void ResetControlValue()
		{
			SelectedJogItem = SELECT_NONE;

			_axisNoX = SELECT_NONE;
			_axisNoY = SELECT_NONE;
			_axisNoExtraX = SELECT_NONE;
			_axisNoExtraY = SELECT_NONE;

			_enableX = false;
			_enableY = false;
			_enableExtraX = false;
			_enableExtraY = false;

			_isReverseX = false;
			_isReverseY = false;
			_isReverseExtraX = false;
			_isReverseExtraY = false;

			_parameterNameX = string.Empty;
			_parameterNameY = string.Empty;
			_recipeTypeX = string.Empty;
			_recipeTypeY = string.Empty;

			MotionType = EN_MOTION_TYPE.SPEED;

			UpdateMonitorPanel(SELECT_NONE, SELECT_NONE, SELECT_NONE, SELECT_NONE);
			SetJogPanel(false, false, false, false);
		}

		private void UpdateParameter(int itemIndex)
		{
			if (false == _configJog.GetParameter(itemIndex, FrameOfSystem3.Config.ConfigJog.EN_PARAM_JOG.ENABLE_LEFT_RIGHT, ref _enableX))
				_enableX = false;

			if (false == _configJog.GetParameter(itemIndex, FrameOfSystem3.Config.ConfigJog.EN_PARAM_JOG.ENABLE_UP_DOWN, ref _enableY))
				_enableY = false;

			if (false == _configJog.GetParameter(itemIndex, FrameOfSystem3.Config.ConfigJog.EN_PARAM_JOG.INDEX_LEFT_RIGHT, ref _axisNoX))
				_axisNoX = -1;

			if (false == _configJog.GetParameter(itemIndex, FrameOfSystem3.Config.ConfigJog.EN_PARAM_JOG.INDEX_UP_DOWN, ref _axisNoY))
				_axisNoY = -1;

			if (false == _configJog.GetParameter(itemIndex, FrameOfSystem3.Config.ConfigJog.EN_PARAM_JOG.ENABLE_EXTRA_LEFT_RIGHT, ref _enableExtraX))
				_enableExtraX = false;

			if (false == _configJog.GetParameter(itemIndex, FrameOfSystem3.Config.ConfigJog.EN_PARAM_JOG.ENABLE_EXTRA_UP_DOWN, ref _enableExtraY))
				_enableExtraY = false;

			if (false == _configJog.GetParameter(itemIndex, FrameOfSystem3.Config.ConfigJog.EN_PARAM_JOG.INDEX_EXTRA_LEFT_RIGHT, ref _axisNoExtraX))
				_axisNoExtraX = -1;

			if (false == _configJog.GetParameter(itemIndex, FrameOfSystem3.Config.ConfigJog.EN_PARAM_JOG.INDEX_EXTRA_UP_DOWN, ref _axisNoExtraY))
				_axisNoExtraY = -1;

			if(false == _enableX || _axisNoX == SELECT_NONE)
			{
				_enableX = false;
				_axisNoX = SELECT_NONE;
			}
			if (false == _enableY || _axisNoY == SELECT_NONE)
			{
				_enableY = false;
				_axisNoY = SELECT_NONE;
			}
			if (false == _enableExtraX || _axisNoExtraX == SELECT_NONE)
			{
				_enableExtraX = false;
				_axisNoExtraX = SELECT_NONE;
			}
			if (false == _enableExtraY || _axisNoExtraY == SELECT_NONE)
			{
				_enableExtraY = false;
				_axisNoExtraY = SELECT_NONE;
			}

			_isReverseX = false;
			_isReverseY = false;
			_isReverseExtraX = false;
			_isReverseExtraY = false;
			int[] arrOfItem = null;
			if (_configJog.GetListOfReverseItem(ref arrOfItem))
			{
				foreach (int i in arrOfItem)
				{
					string strValue = string.Empty;
					if (_configJog.GetParameterReverse(i, FrameOfSystem3.Config.ConfigJog.EN_PARAM_JOG_REVERSE.KEY, ref strValue))
					{
						int axisNo = int.Parse(strValue);
						_isReverseX = _axisNoX == axisNo;
						_isReverseY = _axisNoY == axisNo;
						_isReverseExtraX = _axisNoExtraX == axisNo;
						_isReverseExtraY = _axisNoExtraY == axisNo;
					}
				}
			}
		}

		#region execute
		private void Execute(object sender, EventArgs e)
		{
			UpdatePosition();
			if (_motionType == EN_MOTION_TYPE.SPEED)
				CheckInterlockCurrentPostion(false);
		}
		private void UpdatePosition()
		{
			if (_extendType == EN_EXTEND_PANEL.MONITOR)
			{
				var command = new Dictionary<EN_JOG_DIRECTION, double>();
				var actual = new Dictionary<EN_JOG_DIRECTION, double>();
				if (_enableX && SELECT_NONE != _axisNoX)
				{
					command.Add(EN_JOG_DIRECTION.X, _motion.GetCommandPosition(_axisNoX));
					actual.Add(EN_JOG_DIRECTION.X, _motion.GetActualPosition(_axisNoX));
				}
				if (_enableY && SELECT_NONE != _axisNoY)
				{
					command.Add(EN_JOG_DIRECTION.Y, _motion.GetCommandPosition(_axisNoY));
					actual.Add(EN_JOG_DIRECTION.Y, _motion.GetActualPosition(_axisNoY));
				}
				if (_enableExtraX && SELECT_NONE != _axisNoExtraX)
				{
					command.Add(EN_JOG_DIRECTION.ExtraX, _motion.GetCommandPosition(_axisNoExtraX));
					actual.Add(EN_JOG_DIRECTION.ExtraX, _motion.GetActualPosition(_axisNoExtraX));
				}
				if (_enableExtraY && SELECT_NONE != _axisNoExtraY)
				{
					command.Add(EN_JOG_DIRECTION.ExtraY, _motion.GetCommandPosition(_axisNoExtraY));
					actual.Add(EN_JOG_DIRECTION.ExtraY, _motion.GetActualPosition(_axisNoExtraY));
				}

				_panel_Monitor.UpdatePosition(command, actual);
			}
			else if(_extendType== EN_EXTEND_PANEL.ITEM_LIST)
			{
				var positions = new Dictionary<int, DPointXY>();
				foreach (var kvp in _axisList)
				{
					positions.Add(kvp.Key, new DPointXY(
						kvp.Value.x < 0 ? 0.0 : _motion.GetActualPosition(kvp.Value.x)
						,kvp.Value.y < 0 ? 0.0 : _motion.GetActualPosition(kvp.Value.y)));
				}
				_panel_ItemList.UpdatePosition(positions);
			}
		}
        #endregion /execute

        #region panel
        void ResizeReducePanelWidth()
        {
            using (Graphics g = this.CreateGraphics())
            {
                SizeF size = g.MeasureString(SelectedJogName, lbl_SelectedName.MainFont);
                size.Width += MARGIN_FOR_TEXT;

                if (size.Width > DEFAULT_REDUCE_PANEL_WIDTH)
                {
                    _panelJogMain.Width = (int)size.Width;
                }
                else
                {
                    _panelJogMain.Width = DEFAULT_REDUCE_PANEL_WIDTH;
                }
            }
            Invalidate(true);
        }

        public void ResizeMonitorPanelSize(int additionalWidth)
        {
            _panel_Monitor.Width = DEFAULT_MONITOR_PANEL_WIDTH + additionalWidth;
        }

        private void SetJogPanel(bool useX, bool useY, bool useExtraX, bool useExtraY)
		{
			if (false == useExtraX && false == useExtraY)
			{
				_panel_DpadExtend.Hide();
				_panel_DpadNormal.Show();
				_panel_DpadNormal.SetAcitveMoveButtons(useX, useY);
			}
			else
			{
				_panel_DpadNormal.Hide();
				_panel_DpadExtend.Show();
				_panel_DpadExtend.SetAcitveMoveButtons(useX, useY, useExtraX, useExtraY);
			}
		}
		private void SetModePanel(EN_MOTION_TYPE type)
		{
			switch (type)
			{
				case EN_MOTION_TYPE.RELATIVE:
					_panel_ModeSpeed.Hide();
					_panel_ModeRelative.Show();
					btn_ModeRelative.ButtonClicked = true;
					btn_ModeSpeed.ButtonClicked = false;
					break;
				case EN_MOTION_TYPE.SPEED:
					_panel_ModeRelative.Hide();
					_panel_ModeSpeed.Show();
					btn_ModeRelative.ButtonClicked = false;
					btn_ModeSpeed.ButtonClicked = true;
					break;
			}
		}
		private void SetExtendPanel(EN_EXTEND_PANEL type)
		{
			switch(type)
			{
				case EN_EXTEND_PANEL.REDUCE:
					_panel_ItemList.Hide();
					_panel_Monitor.Hide();
					btn_Extend.Text = RIGHT_ARROW;
					break;
				case EN_EXTEND_PANEL.ITEM_LIST:
					if (_isParameterMode) return;

					_panel_Monitor.Hide();
					_panel_ItemList.Show();
					btn_Extend.Text = RIGHT_ARROW;
					break;
				case EN_EXTEND_PANEL.MONITOR:
					_panel_ItemList.Hide();
					_panel_Monitor.Show();
					btn_Extend.Text = LEFT_ARROW;
					break;
			}

            _extendType = type;
            ResizeReducePanelWidth();
            // ChangeFormSize(type);
        }

		private void UpdateMonitorPanel(int axisX, int axisY, int axisExtraX, int axisExtraY)
		{
			var monitorItemList = new Dictionary<EN_JOG_DIRECTION, JogPanel_Monitor.JogMonitorItem>();

			Action<EN_JOG_DIRECTION, int> AddMonitorItem = (direction, axisNo) =>
				{
					#region
					if (axisNo != SELECT_NONE)
					{
						string axisName;
						switch (direction)
						{
							case EN_JOG_DIRECTION.X:
							case EN_JOG_DIRECTION.Y:
								axisName = _configJog.GetAxisNameByIndex(axisNo);
								monitorItemList.Add(direction, new JogPanel_Monitor.JogMonitorItem(axisNo, axisName, false));
								break;
							case EN_JOG_DIRECTION.ExtraX:
							case EN_JOG_DIRECTION.ExtraY:
								axisName = _configJog.GetAxisNameByIndex(axisNo);
								monitorItemList.Add(direction, new JogPanel_Monitor.JogMonitorItem(axisNo, axisName, true));
								break;
						}
					}
					#endregion
				};

			AddMonitorItem(EN_JOG_DIRECTION.X, axisX);
			AddMonitorItem(EN_JOG_DIRECTION.Y, axisY);
			AddMonitorItem(EN_JOG_DIRECTION.ExtraX, axisExtraX);
			AddMonitorItem(EN_JOG_DIRECTION.ExtraY, axisExtraY);

			_panel_Monitor.UpdateMonitorItem(monitorItemList);
		}
		#endregion /panel

		private bool CheckInterlockCurrentPostion(bool bBeforeMove)
		{
			if (EquipmentState_.EquipmentState.GetInstance().GetState() == EquipmentState_.EQUIPMENT_STATE.IDLE)
			{
				string strInterlockDiscription = "";
				if (_axisNoX >= 0 && !_interlock.CheckMotionInSafety(_axisNoX, _motion.GetActualPosition(_axisNoX)))
				{
					int nStatus = 0;
					_motion.GetMotorState(_axisNoX, ref nStatus);
					if (false == _motion.GetState(ref nStatus, MotionInstance::Motion_.MOTOR_STATE.MOTION_DONE) || bBeforeMove)
					{
						_motion.StopMotion(_axisNoX, false);
                        strInterlockDiscription = _configinterlock.GetMotionLastInterference(_axisNoX);
						Form_MessageBox.GetInstance().ShowMessage(strInterlockDiscription, "MOVE FAIL OCCURED INTERLOCK");
					}
					return false;
				}
				if (_axisNoY >= 0 && !_interlock.CheckMotionInSafety(_axisNoY, _motion.GetActualPosition(_axisNoY)))
				{
					int nStatus = 0;
					_motion.GetMotorState(_axisNoY, ref nStatus);
					if (false == _motion.GetState(ref nStatus, MotionInstance::Motion_.MOTOR_STATE.MOTION_DONE) || bBeforeMove)
					{
						_motion.StopMotion(_axisNoY, false);
                        strInterlockDiscription = _configinterlock.GetMotionLastInterference(_axisNoY);
                        Form_MessageBox.GetInstance().ShowMessage(strInterlockDiscription, "MOVE FAIL OCCURED INTERLOCK");
					}
					return false;
				}
			}
			return true;
		}
		/// <summary>
		/// 2021.07.28 by twkang [ADD] 버튼 클릭 확인
		/// </summary>
		private bool ConfirmButtonClick(string strActionName)
		{
			return _messageBox.ShowMessage(string.Format("Action : [{0}], This is a confirmation message", strActionName));
		}
		/// <summary>
		/// 2021.10.13 by twkang [ADD] Extend Mode 설정
		/// </summary>
		private void ChangeFormSize(EN_EXTEND_PANEL category)
		{
			if (_extendType == category)
				return;

			_extendType = category;

			int formSize;
			switch (category)
			{
				case EN_EXTEND_PANEL.REDUCE: formSize = FORM_SIZE_REDUCE; break;
				case EN_EXTEND_PANEL.ITEM_LIST: formSize = FORM_SIZE_EXTEND_LIST; break;
				case EN_EXTEND_PANEL.MONITOR: formSize = FORM_SIZE_EXTEND_MONITOR; break;
				default: return;
			}

			Size pSizeOfForm = this.Size;
			pSizeOfForm.Width = formSize;
			pSizeOfForm.Height = 516;
			this.Size = pSizeOfForm;

			Invalidate();
		}

		private string GetRecipeValue(string recipeType, string parameterName)
		{
			if (recipeType == string.Empty || parameterName == string.Empty)
				return string.Empty;

			string result = string.Empty;

			EN_RECIPE_TYPE enType;
			if (Enum.TryParse(recipeType, out enType))
				result = _recipe.GetValue(enType, parameterName, 0, EN_RECIPE_PARAM_TYPE.VALUE, string.Empty);
			else
				result = _recipe.GetValue(recipeType, parameterName, 0, EN_RECIPE_PARAM_TYPE.VALUE, string.Empty);

			return result;
		}

		private bool IsMachineIdleState()
		{
			EquipmentState_.EQUIPMENT_STATE current = EquipmentState_.EquipmentState.GetInstance().GetState();
			switch (current)
			{
				case EquipmentState_.EQUIPMENT_STATE.IDLE:
					return true;
				default:
					return false;
			}
		}
		#endregion </METHOD>

		#region <ENUM>
		private enum EN_EXTEND_PANEL
		{
			REDUCE,
			ITEM_LIST,
			MONITOR,
		}
		#endregion </ENUM>

		#region <UI EVENT>
		private void Click_Close(object sender, EventArgs e)
		{
			_timerForUpdate.Stop();
			this.Hide();
		}
		private void Click_MotionMode(object sender, EventArgs e)
		{
			EN_MOTION_TYPE motionType;
			if (sender == btn_ModeRelative) motionType = EN_MOTION_TYPE.RELATIVE;
			else if (sender == btn_ModeSpeed) motionType = EN_MOTION_TYPE.SPEED;
			else return;

			MotionType = motionType;
		}
		private void Form_Jog_KeyDown(object sender, KeyEventArgs e)
		{
			// 2023.04.06 by junho [ADD] interlock
			if (false == IsMachineIdleState()) return;

			int axisNo;
			bool direction;
			switch (e.KeyData)
			{
				case Keys.Up:
					if (false == _enableY) return;
					axisNo = _axisNoY;
					direction = true ^ _isReverseY;
					break;
				case Keys.Left:
					if (false == _enableX) return;
					axisNo = _axisNoX;
					direction = false ^ _isReverseX;
					break;
				case Keys.Right:
					if (false == _enableX) return;
					axisNo = _axisNoX;
					direction = true ^ _isReverseX;
					break;
				case Keys.Down:
					if (false == _enableY) return;
					axisNo = _axisNoY;
					direction = false ^ _isReverseY;
					break;
				default:
					return;
			}

			if (_motionType == EN_MOTION_TYPE.RELATIVE)
			{
				double destination = direction ? _relativeStep : (-1 * _relativeStep);
				_motion.MoveReleatively(axisNo, destination
                    , Motion_.MOTION_SPEED_CONTENT.RUN, RATIO_MOTION, DELAY_MOTION);
			}
			else if (_motionType == EN_MOTION_TYPE.SPEED)
			{
				_motion.MoveAtSpeed(axisNo, direction, _motionSpeedRatio);
			}
		}
		private void Form_Jog_KeyUp(object sender, KeyEventArgs e)
		{
			if (EN_MOTION_TYPE.RELATIVE == MotionType)
				return;

			switch (e.KeyData)
			{
				case Keys.Up:
				case Keys.Down:
					if (_enableY)
					{
						_motion.StopMotion(_axisNoY, false);
					}
					break;
				case Keys.Left:
				case Keys.Right:
					if (_enableX)
					{
						_motion.StopMotion(_axisNoX, false);
					}
					break;
				case Keys.Escape:
					// 2025.03.18. by shkim. [MOD] Null 예외 보완
					
                    if (_enableX)
                    {
                        _motion.StopMotion(_axisNoX, false);
                    }
                    if (_enableY)
                    {
                        _motion.StopMotion(_axisNoY, false);
                    }
     //               _motion.StopMotion(_axisNoY, false);
					//_motion.StopMotion(_axisNoX, false);
					
					// 2025.03.18. by shkim. [END]
					Click_Close(null, null);
					break;
				default:
					return;
			}
		}
		private void Click_ExtendMonitor(object sender, EventArgs e)
		{
			if (_extendType != EN_EXTEND_PANEL.MONITOR)
			{
				SetExtendPanel(EN_EXTEND_PANEL.MONITOR);
			}
			else
			{
				SetExtendPanel(EN_EXTEND_PANEL.REDUCE);
			}
		}
		private void Click_ExtendList(object sender, EventArgs e)
		{
			if (_extendType != EN_EXTEND_PANEL.ITEM_LIST)
			{
				SetExtendPanel(EN_EXTEND_PANEL.ITEM_LIST);
			}
			else
			{
				SetExtendPanel(EN_EXTEND_PANEL.REDUCE);
			}
		}
        #endregion </UI EVENT>

        private void Form_Jog_Load(object sender, EventArgs e)
        {

        }
    }
}
