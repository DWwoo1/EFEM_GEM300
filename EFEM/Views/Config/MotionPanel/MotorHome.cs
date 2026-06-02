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
    public partial class MotorHome : UserControlForMainView.CustomView
    {
        public MotorHome()
        {
            InitializeComponent();

			m_InstanceOfMotion					= FrameOfSystem3.Config.ConfigMotion.GetInstance();
			m_InstanceOfCalculator				= Functional.Form_Calculator.GetInstance();
			m_InstanceOfSelectionList			= Functional.Form_SelectionList.GetInstance();
			m_InstanceOfMessageBox				= Functional.Form_MessageBox.GetInstance();

			#region Mapping
			m_DicForHomeMode.Clear();
			foreach(HOME_MODE en in Enum.GetValues(typeof(HOME_MODE)))
			{
				m_DicForHomeMode.Add((int)en, en.ToString());
			}
			m_DicForSpeedPattern.Clear();
			foreach(MOTION_SPEED_PATTERN en in Enum.GetValues(typeof(MOTION_SPEED_PATTERN)))
			{
				m_DicForSpeedPattern.Add((int)en, en.ToString());
			}
			#endregion
		}

		#region 열거형
		private enum HOME_MODE
		{
			ORG_ONESTEP = 0,
			ORG_TWOSTEP = 1,
			ORG_EZ_ONESTEP_INSTANTSTOP = 2,
			ORG_EZ_ONESTEP_STOP = 3,
			ORG_EZ_TWOSTEP_INSTANTSTOP = 4,
			ORG_EZ_TWOSTEP_STOP = 5,
			EZ_ONESTEP = 6,
			EL_ONESTEP = 7,
			EL_TWOSTEP = 8,
			EL_EZ_TWOSTEP_INSTANTSTOP = 9,
			EL_EZ_TWOSTEP_STOP = 10,
			ABSOLUTE_ONLY = 11,
			FIXED = 12,
		}
		private enum MOTION_SPEED_PATTERN
		{
			CONSTANT		= 0,
			TRAPEZODIAL		= 1,
			S_CURVE			= 2,
			ANTI_VABRATION	= 3,
		}
		#endregion

		#region 상수
		private readonly string MIN_OF_PARAM			= "0";
		private readonly string MAX_OF_PARAM			= "999999";

		private readonly string DEFALUT_VALUE			= "--";
		private const int SELECT_NONE					= -1;
		#endregion

		#region 변수
		private int m_nIndexOfSelectedItem				= SELECT_NONE;

		Dictionary<int, string> m_DicForHomeMode				= new Dictionary<int,string>();
		Dictionary<int, string> m_DicForSpeedPattern			= new Dictionary<int,string>();

		Functional.Form_MessageBox m_InstanceOfMessageBox		= null;
		Functional.Form_SelectionList m_InstanceOfSelectionList	= null;
		Functional.Form_Calculator m_InstanceOfCalculator		= null;
		FrameOfSystem3.Config.ConfigMotion m_InstanceOfMotion			= null;
		#endregion

		#region 외부인터페이스
		/// <summary>
		/// 2020.06.24 by twkang [ADD] 컨트롤들의 활성 상태를 설정한다.
		/// </summary>
		public void SetActiveControls(bool bEnable)
		{
			m_ToggleAlwaysHomeEnd.Enabled   = bEnable;
			if(bEnable == false)
			{
				m_ToggleAlwaysHomeEnd.Active	= false;
			}

            m_labelHomeMode.Enabled				= bEnable;
            m_labelHomeLogic.Enabled			= bEnable;
            m_labelHomeDirection.Enabled		= bEnable;
            m_labelEscapeDistance.Enabled		= bEnable;
            m_labelHomeOffset.Enabled			= bEnable;
            m_labelEZCount.Enabled				= bEnable;
            m_labelBasePosition.Enabled			= bEnable;
			m_labelAbsolutePositionSet.Enabled	= bEnable;

            m_labelSpeedPattern.Enabled			= bEnable;
            m_labelVelocityStart.Enabled		= bEnable;
            m_labelVelocityEnd.Enabled			= bEnable;
            m_labelAccelTime.Enabled			= bEnable;
            m_labelDecelTime.Enabled			= bEnable;
            m_labelJerkAccel.Enabled			= bEnable;
            m_labelJerkDecel.Enabled			= bEnable;
            m_labelHomeTimeout.Enabled			= bEnable;
			m_labelAccelVelocity.Enabled		= bEnable;
			m_labelDeccelVelocity.Enabled		= bEnable;

			m_btnGet.Enabled					= bEnable;
			m_btnSet.Enabled					= bEnable;
		}
		
		/// <summary>
		/// 2020.06.24 by twkang [ADD] 컨트롤들의 값을 설정한다.
		/// </summary>
		public void SetValuesToControls(int nIndexOfItem)
		{
			ResetSelectedItem();
			if (nIndexOfItem == SELECT_NONE)
			{
				return;
			}

			m_nIndexOfSelectedItem	= nIndexOfItem;

			bool bResult			= false;
			string strValue			= string.Empty;
			int nResult				= -1;
			double dbValue			= -1;

			#region HOME CONFIGURATION
			m_InstanceOfMotion.GetHomeParameter(nIndexOfItem, FrameOfSystem3.Config.ConfigMotion.EN_PARAM_HOME.ENABLE_HOME_END, ref bResult);
			m_ToggleAlwaysHomeEnd.Active		= bResult;

			if(m_InstanceOfMotion.GetHomeParameter(nIndexOfItem, FrameOfSystem3.Config.ConfigMotion.EN_PARAM_HOME.HOME_MODE, ref nResult))
			{
				if(m_DicForHomeMode.ContainsKey(nResult))
				{
					m_labelHomeMode.Text		= m_DicForHomeMode[nResult];
				}				
			}
			
			if (m_InstanceOfMotion.GetHomeParameter(nIndexOfItem, FrameOfSystem3.Config.ConfigMotion.EN_PARAM_HOME.HOME_LOGIC, ref bResult))
			{
				m_labelHomeLogic.Text		= bResult ? Define.DefineConstant.Motion.LOGIC_ACTIVE_HIGH : Define.DefineConstant.Motion.LOGIC_ACTIVE_LOW;
			}

			if (m_InstanceOfMotion.GetHomeParameter(nIndexOfItem, FrameOfSystem3.Config.ConfigMotion.EN_PARAM_HOME.HOME_ESCAPE_DISTANCE, ref strValue))
			{
				m_labelEscapeDistance.Text	= strValue;
			}

			if (m_InstanceOfMotion.GetHomeParameter(nIndexOfItem, FrameOfSystem3.Config.ConfigMotion.EN_PARAM_HOME.HOME_COUNT_Z, ref strValue))
			{
				m_labelEZCount.Text			= strValue;
			}

			if (m_InstanceOfMotion.GetHomeParameter(nIndexOfItem, FrameOfSystem3.Config.ConfigMotion.EN_PARAM_HOME.HOME_BASE_POSITION, ref strValue))
			{
				m_labelBasePosition.Text	= strValue;
			}

			if (m_InstanceOfMotion.GetHomeParameter(nIndexOfItem, FrameOfSystem3.Config.ConfigMotion.EN_PARAM_HOME.HOME_DIRECTION, ref bResult))
			{
				m_labelHomeDirection.Text = bResult ? Define.DefineConstant.Motion.DIRECTION_POSITIVE : Define.DefineConstant.Motion.DIRECITON_NEGATIVE;				
			}

			if (m_InstanceOfMotion.GetHomeParameter(nIndexOfItem, FrameOfSystem3.Config.ConfigMotion.EN_PARAM_HOME.HOME_OFFSET, ref strValue))
			{
				m_labelHomeOffset.Text				= strValue;
			}
			if(m_InstanceOfMotion.GetHomeParameter(nIndexOfItem, FrameOfSystem3.Config.ConfigMotion.EN_PARAM_HOME.HOME_ABSOLUTE_POSITION, ref dbValue))
			{
				m_labelAbsolutePositionSet.Text		= dbValue.ToString();
			}
			#endregion

			#region HOME SPPED CONFIGURATION
			if (m_InstanceOfMotion.GetHomeParameter(nIndexOfItem, FrameOfSystem3.Config.ConfigMotion.EN_PARAM_HOME.HOME_SPEED_PATTERN, ref nResult))
			{
				if(m_DicForSpeedPattern.ContainsKey(nResult))
				{
					m_labelSpeedPattern.Text	= m_DicForSpeedPattern[nResult];
				}
			}

			if (m_InstanceOfMotion.GetHomeParameter(nIndexOfItem, FrameOfSystem3.Config.ConfigMotion.EN_PARAM_HOME.HOME_VELOCITY_START, ref dbValue))
			{
				m_labelVelocityStart.Text	= string.Format("{0:0.000}", dbValue);
			}

			if (m_InstanceOfMotion.GetHomeParameter(nIndexOfItem, FrameOfSystem3.Config.ConfigMotion.EN_PARAM_HOME.HOME_VELOCITY_END, ref dbValue))
			{
				m_labelVelocityEnd.Text		= string.Format("{0:0.000}", dbValue);
			}
			
			if (m_InstanceOfMotion.GetHomeParameter(nIndexOfItem, FrameOfSystem3.Config.ConfigMotion.EN_PARAM_HOME.HOME_TIME_ACCEL, ref dbValue))
			{
				m_labelAccelTime.Text		= string.Format("{0:0.000}", dbValue);
			}

			if (m_InstanceOfMotion.GetHomeParameter(nIndexOfItem, FrameOfSystem3.Config.ConfigMotion.EN_PARAM_HOME.HOME_TIME_DECEL, ref dbValue))
			{
				m_labelDecelTime.Text		= string.Format("{0:0.000}", dbValue);
			}
			
			if (m_InstanceOfMotion.GetHomeParameter(nIndexOfItem, FrameOfSystem3.Config.ConfigMotion.EN_PARAM_HOME.HOME_JERK_ACCEL, ref strValue))
			{
				m_labelJerkAccel.Text		= strValue;
			}

			if (m_InstanceOfMotion.GetHomeParameter(nIndexOfItem, FrameOfSystem3.Config.ConfigMotion.EN_PARAM_HOME.HOME_JERK_DECEL, ref strValue))
			{
				m_labelJerkDecel.Text		= strValue;
			}

			if (m_InstanceOfMotion.GetHomeParameter(nIndexOfItem, FrameOfSystem3.Config.ConfigMotion.EN_PARAM_HOME.TIMEOUT, ref strValue))
			{
				m_labelHomeTimeout.Text		= strValue;
			}
			if (m_InstanceOfMotion.GetHomeParameter(nIndexOfItem, FrameOfSystem3.Config.ConfigMotion.EN_PARAM_HOME.HOME_ACCEL, ref dbValue))
			{
				m_labelAccelVelocity.Text	= string.Format("{0:0.000}", dbValue);
			}
			if(m_InstanceOfMotion.GetHomeParameter(nIndexOfItem, FrameOfSystem3.Config.ConfigMotion.EN_PARAM_HOME.HOME_DECEL, ref dbValue))
			{
				m_labelDeccelVelocity.Text	= string.Format("{0:0.000}", dbValue);
			}
			#endregion

		}
		/// <summary>
		/// 
		/// </summary>
		public void UpdateState()
		{
			if(SELECT_NONE == m_nIndexOfSelectedItem)
			{
				return;
			}

			double dbAbsolutePosition	= m_InstanceOfMotion.GetAbsolutePosition(m_nIndexOfSelectedItem);

			m_labelAbsolutePosition.Text	= string.Format("{0:0.0000}", dbAbsolutePosition);
		}
		#endregion

		#region 내부인터페이스
		/// <summary>
		/// 2020.07.07 by twkang [ADD] 라벨 상태와, 값을 초기화한다.
		/// </summary>
		private void ResetSelectedItem()
		{
			m_nIndexOfSelectedItem			= SELECT_NONE;

			m_labelHomeMode.Text			= DEFALUT_VALUE;
			m_labelHomeLogic.Text			= DEFALUT_VALUE;
			m_labelHomeDirection.Text		= DEFALUT_VALUE;
			m_labelEscapeDistance.Text		= DEFALUT_VALUE;
			m_labelHomeOffset.Text			= DEFALUT_VALUE;
			m_labelEZCount.Text				= DEFALUT_VALUE;
			m_labelBasePosition.Text		= DEFALUT_VALUE;
			m_labelAbsolutePosition.Text	= DEFALUT_VALUE;
			m_labelAbsolutePositionSet.Text	= DEFALUT_VALUE;

			m_labelAccelVelocity.Text		= DEFALUT_VALUE;
			m_labelDeccelVelocity.Text		= DEFALUT_VALUE;
			m_labelSpeedPattern.Text		= DEFALUT_VALUE;
			m_labelVelocityStart.Text		= DEFALUT_VALUE;
			m_labelVelocityEnd.Text			= DEFALUT_VALUE;
			m_labelAccelTime.Text			= DEFALUT_VALUE;
			m_labelDecelTime.Text			= DEFALUT_VALUE;
			m_labelJerkAccel.Text			= DEFALUT_VALUE;
			m_labelJerkDecel.Text			= DEFALUT_VALUE;
			m_labelHomeTimeout.Text			= DEFALUT_VALUE;
		}
		#endregion

		#region UI 이벤트
		/// <summary>
		/// 2020.06.23 by twkang [ADD] Configuration Label 클릭 이벤트
		/// </summary>
		private void Click_Configuration(object sender, EventArgs e)
		{
			Control ctrl		= sender as Control;
			
			string strValue		= string.Empty;
			double dbValue		= 0;

			switch(ctrl.TabIndex)
			{
				case 0: // HOME END ENABLE
					bool bEnable		= m_ToggleAlwaysHomeEnd.Active;
					if(false == m_InstanceOfMotion.SetHomeParameter(m_nIndexOfSelectedItem, FrameOfSystem3.Config.ConfigMotion.EN_PARAM_HOME.ENABLE_HOME_END, bEnable))
					{
						m_ToggleAlwaysHomeEnd.Active		= !bEnable;
					}
					break;
				case 1:	// HOME MODE
					if(m_InstanceOfSelectionList.CreateForm(m_lblHomeMode.Text
						, Define.DefineEnumProject.SelectionList.EN_SELECTIONLIST.MOTION_HOME_MODE
						, m_labelHomeMode.Text))
					{
						m_InstanceOfSelectionList.GetResult(ref strValue);
						if(m_InstanceOfMotion.SetHomeParameter(m_nIndexOfSelectedItem, FrameOfSystem3.Config.ConfigMotion.EN_PARAM_HOME.HOME_MODE, strValue))
						{
							m_labelHomeMode.Text	= strValue;
						}
					}
					break;
				case 2:	// HOME LOGIC
					if (m_InstanceOfSelectionList.CreateForm(m_lblHomeLogic.Text
						, Define.DefineEnumProject.SelectionList.EN_SELECTIONLIST.MOTION_LOGIC
						, m_labelHomeLogic.Text))
					{
						m_InstanceOfSelectionList.GetResult(ref strValue);
						if (m_InstanceOfMotion.SetHomeParameter(m_nIndexOfSelectedItem, FrameOfSystem3.Config.ConfigMotion.EN_PARAM_HOME.HOME_LOGIC, strValue))
						{
							m_labelHomeLogic.Text = strValue;
						}
					}
					break;
				case 3:	// ESCAPE DISTANCE
					if(m_InstanceOfCalculator.CreateForm(m_labelEscapeDistance.Text, MIN_OF_PARAM, MAX_OF_PARAM))
					{
						m_InstanceOfCalculator.GetResult(ref dbValue);
						if(m_InstanceOfMotion.SetHomeParameter(m_nIndexOfSelectedItem, FrameOfSystem3.Config.ConfigMotion.EN_PARAM_HOME.HOME_ESCAPE_DISTANCE, dbValue))
						{
							m_labelEscapeDistance.Text	= dbValue.ToString();
						}
					}
					break;
				case 4:	// ENCODER Z COUNT
					if(m_InstanceOfCalculator.CreateForm(m_labelEZCount.Text, MIN_OF_PARAM, MAX_OF_PARAM))
					{
						m_InstanceOfCalculator.GetResult(ref dbValue);
						if(m_InstanceOfMotion.SetHomeParameter(m_nIndexOfSelectedItem, FrameOfSystem3.Config.ConfigMotion.EN_PARAM_HOME.HOME_COUNT_Z, dbValue))
						{
							m_labelEZCount.Text		= dbValue.ToString();
						}
					}
					break;
				case 5:	// BACE POSITION
					if (m_InstanceOfCalculator.CreateForm(m_labelBasePosition.Text, MIN_OF_PARAM, MAX_OF_PARAM))
					{
						m_InstanceOfCalculator.GetResult(ref dbValue);
						if (m_InstanceOfMotion.SetHomeParameter(m_nIndexOfSelectedItem, FrameOfSystem3.Config.ConfigMotion.EN_PARAM_HOME.HOME_BASE_POSITION, dbValue))
						{
							m_labelBasePosition.Text = dbValue.ToString();
						}
					}
					break;
				case 6:	// HOME DIRECTION
					if(m_InstanceOfSelectionList.CreateForm(m_lblHomeDirection.Text
						, Define.DefineEnumProject.SelectionList.EN_SELECTIONLIST.MOTION_HOME_DIRECTION
						, m_labelHomeDirection.Text))
					{
						m_InstanceOfSelectionList.GetResult(ref strValue);
						if(m_InstanceOfMotion.SetHomeParameter(m_nIndexOfSelectedItem, FrameOfSystem3.Config.ConfigMotion.EN_PARAM_HOME.HOME_DIRECTION, strValue))
						{
							m_labelHomeDirection.Text	= strValue;
						}
					}
					break;
				case 7:	// HOME OFFSET
					if(m_InstanceOfCalculator.CreateForm(m_labelHomeOffset.Text, MIN_OF_PARAM, MAX_OF_PARAM))
					{
						m_InstanceOfCalculator.GetResult(ref dbValue);
						if(m_InstanceOfMotion.SetHomeParameter(m_nIndexOfSelectedItem, FrameOfSystem3.Config.ConfigMotion.EN_PARAM_HOME.HOME_OFFSET, dbValue))
						{
							m_labelHomeOffset.Text		= dbValue.ToString();
						}
					}
					break;
				case 8: // Absolute Position
					if(m_InstanceOfCalculator.CreateForm(m_labelAbsolutePositionSet.Text, MIN_OF_PARAM, MAX_OF_PARAM))
					{
						m_InstanceOfCalculator.GetResult(ref dbValue);
						m_labelAbsolutePositionSet.Text		= dbValue.ToString();
					}
					break;
			}
		}
		/// <summary>
		/// 2020.06.23 by twkang [ADD] Speed Configuration Label 클릭 이벤ㄴ트
		/// </summary>
		private void Click_SpeedConfiguration(object sender, EventArgs e)
		{
			Control ctrl		= sender as Control;

			string strValue		= string.Empty;
			double dbValue		= 0;
			
			switch(ctrl.TabIndex)
			{
				case 0: // SPPED PATTERN
					if(m_InstanceOfSelectionList.CreateForm(m_lblSpeedPattern.Text
						, Define.DefineEnumProject.SelectionList.EN_SELECTIONLIST.MOTION_SPEED_PATTERN
						, m_labelSpeedPattern.Text))
					{
						m_InstanceOfSelectionList.GetResult(ref strValue);
						if(m_InstanceOfMotion.SetHomeParameter(m_nIndexOfSelectedItem, FrameOfSystem3.Config.ConfigMotion.EN_PARAM_HOME.HOME_SPEED_PATTERN, strValue))
						{
							m_labelSpeedPattern.Text	= strValue;
						}
					}
					break;
				case 1:	// VELOCITY START
					if(m_InstanceOfCalculator.CreateForm(m_labelVelocityStart.Text, MIN_OF_PARAM, MAX_OF_PARAM))
					{
						m_InstanceOfCalculator.GetResult(ref dbValue);
						if(m_InstanceOfMotion.SetHomeParameter(m_nIndexOfSelectedItem, FrameOfSystem3.Config.ConfigMotion.EN_PARAM_HOME.HOME_VELOCITY_START, dbValue))
						{
							m_labelVelocityStart.Text		= dbValue.ToString();
						}
					}
					break;
				case 2:	// VELOCITY END
					if (m_InstanceOfCalculator.CreateForm(m_labelVelocityEnd.Text, MIN_OF_PARAM, MAX_OF_PARAM))
					{
						m_InstanceOfCalculator.GetResult(ref dbValue);
						if (m_InstanceOfMotion.SetHomeParameter(m_nIndexOfSelectedItem, FrameOfSystem3.Config.ConfigMotion.EN_PARAM_HOME.HOME_VELOCITY_END, dbValue))
						{
							m_labelVelocityEnd.Text = dbValue.ToString();
						}
					}
					break;
				case 3:	// ACCELERATION TIME
					if (m_InstanceOfCalculator.CreateForm(m_labelAccelTime.Text, MIN_OF_PARAM, MAX_OF_PARAM))
					{
						m_InstanceOfCalculator.GetResult(ref dbValue);
						if (m_InstanceOfMotion.SetHomeParameter(m_nIndexOfSelectedItem, FrameOfSystem3.Config.ConfigMotion.EN_PARAM_HOME.HOME_TIME_ACCEL, dbValue))
						{
							m_labelAccelTime.Text = dbValue.ToString();
						}
					}
					break;
				case 4:	// DECELERATION TIME
					if (m_InstanceOfCalculator.CreateForm(m_labelDecelTime.Text, MIN_OF_PARAM, MAX_OF_PARAM))
					{
						m_InstanceOfCalculator.GetResult(ref dbValue);
						if (m_InstanceOfMotion.SetHomeParameter(m_nIndexOfSelectedItem, FrameOfSystem3.Config.ConfigMotion.EN_PARAM_HOME.HOME_TIME_DECEL, dbValue))
						{
							m_labelDecelTime.Text = dbValue.ToString();
						}
					}
					break;
				case 5:	// ACCELERATION JERK
					if (m_InstanceOfCalculator.CreateForm(m_labelJerkAccel.Text, MIN_OF_PARAM, MAX_OF_PARAM))
					{
						m_InstanceOfCalculator.GetResult(ref dbValue);
						if (m_InstanceOfMotion.SetHomeParameter(m_nIndexOfSelectedItem, FrameOfSystem3.Config.ConfigMotion.EN_PARAM_HOME.HOME_JERK_ACCEL, dbValue))
						{
							m_labelJerkAccel.Text = dbValue.ToString();
						}
					}
					break;
				case 6:	// DECELERATION JERK
					if (m_InstanceOfCalculator.CreateForm(m_labelJerkDecel.Text, MIN_OF_PARAM, MAX_OF_PARAM))
					{
						m_InstanceOfCalculator.GetResult(ref dbValue);
						if (m_InstanceOfMotion.SetHomeParameter(m_nIndexOfSelectedItem, FrameOfSystem3.Config.ConfigMotion.EN_PARAM_HOME.HOME_JERK_DECEL, dbValue))
						{
							m_labelJerkDecel.Text = dbValue.ToString();
						}
					}
					break;
				case 7: // HOME TIMEOUT
					if (m_InstanceOfCalculator.CreateForm(m_labelHomeTimeout.Text, MIN_OF_PARAM, MAX_OF_PARAM))
					{
						m_InstanceOfCalculator.GetResult(ref dbValue);
						if (m_InstanceOfMotion.SetHomeParameter(m_nIndexOfSelectedItem, FrameOfSystem3.Config.ConfigMotion.EN_PARAM_HOME.TIMEOUT, dbValue))
						{
							m_labelHomeTimeout.Text = dbValue.ToString();
						}
					}
					break;
				case 8: // HOME ACCEL
					if (m_InstanceOfCalculator.CreateForm(m_labelAccelVelocity.Text, MIN_OF_PARAM, MAX_OF_PARAM))
					{
						m_InstanceOfCalculator.GetResult(ref dbValue);
						if (m_InstanceOfMotion.SetHomeParameter(m_nIndexOfSelectedItem, FrameOfSystem3.Config.ConfigMotion.EN_PARAM_HOME.HOME_ACCEL, dbValue))
						{
							m_labelAccelVelocity.Text = dbValue.ToString();
						}
					}
					break;
				case 9: // HOME DECEL
					if (m_InstanceOfCalculator.CreateForm(m_labelDeccelVelocity.Text, MIN_OF_PARAM, MAX_OF_PARAM))
					{
						m_InstanceOfCalculator.GetResult(ref dbValue);
						if (m_InstanceOfMotion.SetHomeParameter(m_nIndexOfSelectedItem, FrameOfSystem3.Config.ConfigMotion.EN_PARAM_HOME.HOME_DECEL, dbValue))
						{
							m_labelDeccelVelocity.Text = dbValue.ToString();
						}
					}
					break;
			}

			SetValuesToControls(m_nIndexOfSelectedItem);
		}
		/// <summary>
		/// 2020.11.16 by twkang [ADD] AbsolutePosition 버튼을 눌렀을 때
		/// </summary>
		private void Click_AbsolutePositionButton(object sender, EventArgs e)
		{
			Control ctrl	= sender as Control;

			switch(ctrl.TabIndex)
			{
				case 0: // GET
                    m_labelAbsolutePositionSet.Text = string.Format("{0:0.0000}", m_InstanceOfMotion.GetAbsolutePosition(m_nIndexOfSelectedItem));
					break;
				case 1:	// SET
					double dbValue		= 0;
					if(double.TryParse(m_labelAbsolutePositionSet.Text, out dbValue))
					{
						// 2024.11.14. by shkim [ADD] Absolute 설정 전, 값이 맞는지 확인
						if(m_InstanceOfMessageBox.ShowMessage(string.Format("다음 값으로 Absolute Setting을 할까요?\n{0}", dbValue)))
						{
                            if (m_InstanceOfMotion.SetHomeParameter(m_nIndexOfSelectedItem, FrameOfSystem3.Config.ConfigMotion.EN_PARAM_HOME.HOME_ABSOLUTE_POSITION, dbValue))
                            {
                                m_InstanceOfMessageBox.ShowMessage(Define.DefineConstant.Common.SUCCESS);
                            }
                            else
                            {
                                m_InstanceOfMessageBox.ShowMessage(Define.DefineConstant.Common.FAIL);
                            }

                        }
						// 2024.11.14. by shkim [END]
					}
					break;
			}
		}
		#endregion
	}
}
