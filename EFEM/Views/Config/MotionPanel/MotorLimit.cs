using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace FrameOfSystem3.Views.Config.MotionPanel
{
	public partial class MotorLimit : UserControlForMainView.CustomView
	{
		public MotorLimit()
		{
			InitializeComponent();

			#region Instance
			m_InstanceOfMotion					= FrameOfSystem3.Config.ConfigMotion.GetInstance();
			m_InstanceOfSelectionList			= Functional.Form_SelectionList.GetInstance();
			m_InstanceOfCalculator				= Functional.Form_Calculator.GetInstance();
			#endregion

		}

		#region 상수
		private readonly string MIN_OF_PARAM					= "0";
		private readonly string MAX_OF_PARAM					= "999999";
		private readonly string DEFALUT_VALUE					= "--";
		private const int SELECT_NONE							= -1;
		#endregion

		#region 변수
		private int m_nIndexOfSelectedItem		= -1;

		FrameOfSystem3.Config.ConfigMotion m_InstanceOfMotion		= null;
		Functional.Form_SelectionList m_InstanceOfSelectionList		= null;
		Functional.Form_Calculator m_InstanceOfCalculator			= null;
		#endregion

		#region 내부인터페이스
		/// <summary>
		/// 
		/// </summary>
		private void SetEnableHWLimitPositiveControls(bool bEnable)
        {
            m_labelHWLimitLogicPositive.Enabled     = bEnable;
            m_labelHWLimitStopModePositive.Enabled  = bEnable;
        }
		/// <summary>
		/// 
		/// </summary>
		private void SetEnableHWLimitNegativeControls(bool bEnable)
        {
            m_labelHWLimitLogicNegative.Enabled		= bEnable;
            m_labelHWLimitStopModeNegative.Enabled	= bEnable;
        }
		/// <summary>
		/// 
		/// </summary>
		private void SetEnableSWLimitPositiveControls(bool bEnable)
		{
			m_labelSWLimitStopModePositive.Enabled	= bEnable;
			m_labelSWLimitPositionPositive.Enabled	= bEnable;
		}
		/// <summary>
		/// 
		/// </summary>
		private void SetEnableSWLimitNegativeControls(bool bEnable)
		{
			m_labelSWLimitStopModeNegative.Enabled	= bEnable;
			m_labelSWLimitPositionNegative.Enabled	= bEnable;
		}
		/// <summary>
		/// 2020.07.07 by twkang [ADD] 라벨 상태와, 값을 초기화한다.
		/// </summary>
		private void ResetSelectedItem()
		{
			m_labelHWLimitLogicNegative.Text	= DEFALUT_VALUE;
			m_labelHWLimitLogicPositive.Text	= DEFALUT_VALUE;
			m_labelHWLimitStopModeNegative.Text	= DEFALUT_VALUE;
			m_labelHWLimitStopModePositive.Text	= DEFALUT_VALUE;

			m_labelSWLimitPositionNegative.Text	= DEFALUT_VALUE;
			m_labelSWLimitPositionPositive.Text = DEFALUT_VALUE;
			m_labelSWLimitStopModeNegative.Text	= DEFALUT_VALUE;
			m_labelSWLimitStopModePositive.Text	= DEFALUT_VALUE;
		}
		#endregion

		#region 외부인터페이스
		/// <summary>
		/// 2020.06.24 by twkang [ADD] 컨트롤들의 활성 상태를 설정한다.
		/// </summary>
		public void SetActiveControls(bool bEnable)
		{
			m_ToggleHWLimitPositive.Enabled		= bEnable;
			m_ToggleHWLimitNegative.Enabled		= bEnable;

			m_ToggleSWLimitPositive.Enabled		= bEnable;
			m_ToggleSWLimitNegative.Enabled		= bEnable;

			if (bEnable)
			{
				SetEnableHWLimitPositiveControls(m_ToggleHWLimitPositive.Active);
				SetEnableHWLimitNegativeControls(m_ToggleHWLimitNegative.Active);
				SetEnableSWLimitPositiveControls(m_ToggleSWLimitPositive.Active);
				SetEnableSWLimitNegativeControls(m_ToggleSWLimitNegative.Active);
			}
			else
			{
				SetEnableHWLimitPositiveControls(false);
				SetEnableHWLimitNegativeControls(false);
				SetEnableSWLimitPositiveControls(false);
				SetEnableSWLimitNegativeControls(false);
			}
		}
		/// <summary>
		/// 2020.06.24 by twkang [ADD] Config Label 값을 갱신한다.
		/// </summary>
		public void SetValuesToControls(int nIndexOfItem)
		{
			ResetSelectedItem();

			if (nIndexOfItem == SELECT_NONE)
			{
				return;
			}

			m_nIndexOfSelectedItem	= nIndexOfItem;

			string strValue			= string.Empty;
			bool bResult			= false;

			#region HARDWARE LIMIT CONFIG
			if (false == m_InstanceOfMotion.GetConfigParameter(nIndexOfItem, FrameOfSystem3.Config.ConfigMotion.EN_PARAM_CONFIG.LIMIT_POSITIVE_ENABLE, ref bResult))
			{
				bResult = false;
			}
			m_ToggleHWLimitPositive.Active = bResult;

			if (false == m_InstanceOfMotion.GetConfigParameter(nIndexOfItem, FrameOfSystem3.Config.ConfigMotion.EN_PARAM_CONFIG.LIMIT_NEGATIVE_ENABLE, ref bResult))
			{
				bResult = false;
			}
			m_ToggleHWLimitNegative.Active = bResult;

			if (m_InstanceOfMotion.GetConfigParameter(nIndexOfItem, FrameOfSystem3.Config.ConfigMotion.EN_PARAM_CONFIG.LIMIT_NEGATIVE_LOGIC, ref bResult))
			{
				m_labelHWLimitLogicNegative.Text = bResult ? Define.DefineConstant.Motion.LOGIC_ACTIVE_HIGH : Define.DefineConstant.Motion.LOGIC_ACTIVE_LOW;

			}

			if (m_InstanceOfMotion.GetConfigParameter(nIndexOfItem, FrameOfSystem3.Config.ConfigMotion.EN_PARAM_CONFIG.LIMIT_POSITIVE_LOGIC, ref bResult))
			{
				m_labelHWLimitLogicPositive.Text = bResult ? Define.DefineConstant.Motion.LOGIC_ACTIVE_HIGH : Define.DefineConstant.Motion.LOGIC_ACTIVE_LOW;
			}

			if (m_InstanceOfMotion.GetConfigParameter(nIndexOfItem, FrameOfSystem3.Config.ConfigMotion.EN_PARAM_CONFIG.LIMIT_POSITIVE_STOPMODE, ref bResult))
			{
				m_labelHWLimitStopModePositive.Text = bResult ? Define.DefineConstant.Motion.STOPMODE_EMG : Define.DefineConstant.Motion.STOPMODE_DECEL;
			}

			if (m_InstanceOfMotion.GetConfigParameter(nIndexOfItem, FrameOfSystem3.Config.ConfigMotion.EN_PARAM_CONFIG.LIMIT_NEGATIVE_STOPMODE, ref bResult))
			{
				m_labelHWLimitStopModeNegative.Text = bResult ? Define.DefineConstant.Motion.STOPMODE_EMG : Define.DefineConstant.Motion.STOPMODE_DECEL;
			}

			#endregion

			#region SOFTWARE LIMIT CONFIG
			if (false == m_InstanceOfMotion.GetConfigParameter(nIndexOfItem, FrameOfSystem3.Config.ConfigMotion.EN_PARAM_CONFIG.SWLIMIT_NEGATIVE_ENABLE, ref bResult))
			{
				bResult = false;
			}
			m_ToggleSWLimitNegative.Active = bResult;

			if (false == m_InstanceOfMotion.GetConfigParameter(nIndexOfItem, FrameOfSystem3.Config.ConfigMotion.EN_PARAM_CONFIG.SWLIMIT_POSITIVE_ENABLE, ref bResult))
			{
				bResult = false;
			}
			m_ToggleSWLimitPositive.Active = bResult;

			if (false == m_InstanceOfMotion.GetConfigParameter(nIndexOfItem, FrameOfSystem3.Config.ConfigMotion.EN_PARAM_CONFIG.SWLIMIT_NEGATIVE_POSITION, ref strValue))
			{
				strValue = string.Empty;
			}
			m_labelSWLimitPositionNegative.Text = strValue;

			if (false == m_InstanceOfMotion.GetConfigParameter(nIndexOfItem, FrameOfSystem3.Config.ConfigMotion.EN_PARAM_CONFIG.SWLIMIT_POSITIVE_POSITION, ref strValue))
			{
				strValue = string.Empty;
			}
			m_labelSWLimitPositionPositive.Text = strValue;

			if (m_InstanceOfMotion.GetConfigParameter(nIndexOfItem, FrameOfSystem3.Config.ConfigMotion.EN_PARAM_CONFIG.SWLIMIT_NEGATIVE_STOPMODE, ref bResult))
			{
				m_labelSWLimitStopModeNegative.Text = bResult ? Define.DefineConstant.Motion.STOPMODE_EMG : Define.DefineConstant.Motion.STOPMODE_DECEL;
			}

			if (m_InstanceOfMotion.GetConfigParameter(nIndexOfItem, FrameOfSystem3.Config.ConfigMotion.EN_PARAM_CONFIG.SWLIMIT_POSITIVE_STOPMODE, ref bResult))
			{
				m_labelSWLimitStopModePositive.Text = bResult ? Define.DefineConstant.Motion.STOPMODE_EMG : Define.DefineConstant.Motion.STOPMODE_DECEL;
			}

			#endregion
		}
		#endregion

		#region UI이벤트
		/// <summary>
		/// 2020.06.24 by twkang [ADD] Hardware Limit Config 라벨 클릭 이벤트
		/// </summary>
		private void Click_HWLimitLabels(object sender, EventArgs e)
		{
			Control ctrl		= sender as Control;
			string strValue		= string.Empty;

			switch(ctrl.TabIndex)
			{
				case 0: // NEGATIVE LIMIT
					if(m_InstanceOfMotion.SetConfigParameter(m_nIndexOfSelectedItem
						, FrameOfSystem3.Config.ConfigMotion.EN_PARAM_CONFIG.LIMIT_NEGATIVE_ENABLE
						, m_ToggleHWLimitNegative.Active))
					{
						SetEnableHWLimitNegativeControls(m_ToggleHWLimitNegative.Active);
					}
					break;
				case 1:	// NEGATIVE LOGIC
					if(m_InstanceOfSelectionList.CreateForm(m_lblHWLimitLogicNegative.Text
						, Define.DefineEnumProject.SelectionList.EN_SELECTIONLIST.MOTION_LOGIC
						, m_labelHWLimitLogicNegative.Text))
					{
						m_InstanceOfSelectionList.GetResult(ref strValue);
						if(m_InstanceOfMotion.SetConfigParameter(m_nIndexOfSelectedItem
							, FrameOfSystem3.Config.ConfigMotion.EN_PARAM_CONFIG.LIMIT_NEGATIVE_LOGIC
							, strValue))
						{
							m_labelHWLimitLogicNegative.Text	= strValue;
						}
					}
					break;
				case 2:	// NEGATIVE STOP MODE
					if(m_InstanceOfSelectionList.CreateForm(m_lblHWLimitStopModeNegative.Text
						, Define.DefineEnumProject.SelectionList.EN_SELECTIONLIST.MOTION_LIMIT_STOP_MODE
						, m_labelHWLimitStopModeNegative.Text))
					{
						m_InstanceOfSelectionList.GetResult(ref strValue);
						if (m_InstanceOfMotion.SetConfigParameter(m_nIndexOfSelectedItem
							, FrameOfSystem3.Config.ConfigMotion.EN_PARAM_CONFIG.LIMIT_NEGATIVE_STOPMODE
							, strValue))
						{
							m_labelHWLimitStopModeNegative.Text = strValue;
						}						
					}
					break;
				case 3:	// POSTIVE LIMIT
					if(m_InstanceOfMotion.SetConfigParameter(m_nIndexOfSelectedItem
						, FrameOfSystem3.Config.ConfigMotion.EN_PARAM_CONFIG.LIMIT_POSITIVE_ENABLE
						, m_ToggleHWLimitPositive.Active))
					{
						SetEnableHWLimitPositiveControls(m_ToggleHWLimitPositive.Active);
					}
					break;
				case 4:	// POSITIVE LOGIC
					if(m_InstanceOfSelectionList.CreateForm(m_lblHWLimitLogicPositive.Text
						, Define.DefineEnumProject.SelectionList.EN_SELECTIONLIST.MOTION_LOGIC
						, m_labelHWLimitLogicPositive.Text))
					{
						m_InstanceOfSelectionList.GetResult(ref strValue);
						if(m_InstanceOfMotion.SetConfigParameter(m_nIndexOfSelectedItem
							, FrameOfSystem3.Config.ConfigMotion.EN_PARAM_CONFIG.LIMIT_POSITIVE_LOGIC
							, strValue))
						{
							m_labelHWLimitLogicPositive.Text	= strValue;
						}
					}
					break;
				case 5:	// POSITIVE STOP MODE
					if(m_InstanceOfSelectionList.CreateForm(m_lblHWLimitStopModePositive.Text
						, Define.DefineEnumProject.SelectionList.EN_SELECTIONLIST.MOTION_LIMIT_STOP_MODE
						, m_labelHWLimitStopModePositive.Text))
					{
						m_InstanceOfSelectionList.GetResult(ref strValue);
						if(m_InstanceOfMotion.SetConfigParameter(m_nIndexOfSelectedItem
							, FrameOfSystem3.Config.ConfigMotion.EN_PARAM_CONFIG.LIMIT_POSITIVE_STOPMODE
							, strValue))
						{
							m_labelHWLimitStopModePositive.Text	= strValue;
						}
					}
					break;
			}
		}
		/// <summary>
		/// 2020.06.24 by twkang [ADD] Software Limit Config 라벨 클릭 이벤트
		/// </summary>
		private void Click_SWLimitLabels(object sender, EventArgs e)
		{
			Control ctrl		= sender as Control;

			string strValue		= string.Empty;
			double dbValue		= 0;

			switch (ctrl.TabIndex)
			{
				case 0: // NEGATIVE LIMIT
					if(m_InstanceOfMotion.SetConfigParameter(m_nIndexOfSelectedItem
						, FrameOfSystem3.Config.ConfigMotion.EN_PARAM_CONFIG.SWLIMIT_NEGATIVE_ENABLE
						, m_ToggleSWLimitNegative.Active))
					{
						SetEnableSWLimitNegativeControls(m_ToggleSWLimitNegative.Active);
					}
					break;
				case 1:	// NEGATIVE POSITION
					if(m_InstanceOfCalculator.CreateForm(m_labelSWLimitPositionNegative.Text
						, MIN_OF_PARAM
						, MAX_OF_PARAM))
					{
						m_InstanceOfCalculator.GetResult(ref dbValue);
						if(m_InstanceOfMotion.SetConfigParameter(m_nIndexOfSelectedItem
							, FrameOfSystem3.Config.ConfigMotion.EN_PARAM_CONFIG.SWLIMIT_NEGATIVE_POSITION
							, dbValue))
						{
							m_labelSWLimitPositionNegative.Text	= dbValue.ToString();
						}
					}
					break;
				case 2:	// STOP MODE
					if(m_InstanceOfSelectionList.CreateForm(m_lblSWLimitStopModeNegative.Text
						, Define.DefineEnumProject.SelectionList.EN_SELECTIONLIST.MOTION_LIMIT_STOP_MODE
						, m_labelSWLimitStopModeNegative.Text))
					{
						m_InstanceOfSelectionList.GetResult(ref strValue);
						if(m_InstanceOfMotion.SetConfigParameter(m_nIndexOfSelectedItem
							, FrameOfSystem3.Config.ConfigMotion.EN_PARAM_CONFIG.SWLIMIT_NEGATIVE_STOPMODE
							, strValue))
						{
							m_labelSWLimitStopModeNegative.Text	= strValue;
						}
					}
					break;
				case 3:	// POSTIVE LIMIT
					if(m_InstanceOfMotion.SetConfigParameter(m_nIndexOfSelectedItem
						, FrameOfSystem3.Config.ConfigMotion.EN_PARAM_CONFIG.SWLIMIT_POSITIVE_ENABLE
						, m_ToggleSWLimitPositive.Active))
					{
						SetEnableSWLimitPositiveControls(m_ToggleSWLimitPositive.Active);
					}
					break;
				case 4:	// POSITIVE POSITION
					if(m_InstanceOfCalculator.CreateForm(m_labelSWLimitPositionPositive.Text
						, MIN_OF_PARAM
						, MAX_OF_PARAM))
					{
						m_InstanceOfCalculator.GetResult(ref dbValue);
						if(m_InstanceOfMotion.SetConfigParameter(m_nIndexOfSelectedItem
							, FrameOfSystem3.Config.ConfigMotion.EN_PARAM_CONFIG.SWLIMIT_POSITIVE_POSITION
							, dbValue))
						{
							m_labelSWLimitPositionPositive.Text	= dbValue.ToString();
						}
					}
					break;
				case 5:	// STOP MODE
					if(m_InstanceOfSelectionList.CreateForm(m_lblSWLimitStopModePositive.Text
						, Define.DefineEnumProject.SelectionList.EN_SELECTIONLIST.MOTION_LIMIT_STOP_MODE
						, m_labelSWLimitStopModePositive.Text))
					{
						m_InstanceOfSelectionList.GetResult(ref strValue);
						if(m_InstanceOfMotion.SetConfigParameter(m_nIndexOfSelectedItem
							, FrameOfSystem3.Config.ConfigMotion.EN_PARAM_CONFIG.SWLIMIT_POSITIVE_STOPMODE
							, strValue))
						{
							m_labelSWLimitStopModePositive.Text	= strValue;
						}
					}
					break;
			}
		}
		#endregion
	}
}
