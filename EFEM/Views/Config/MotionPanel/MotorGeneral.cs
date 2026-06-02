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
    public partial class MotorGeneral : UserControlForMainView.CustomView
    {
        public MotorGeneral()
        {
            InitializeComponent();

			m_InstanceOfMotion					= FrameOfSystem3.Config.ConfigMotion.GetInstance();
			m_InstanceOfSelectionList			= Functional.Form_SelectionList.GetInstance();
			m_InstanceOfCalculator				= Functional.Form_Calculator.GetInstance();

			#region Mappint
			m_DicForMotionType.Clear();
			m_DicForMotorType.Clear();
            m_DicForMotorInmode.Clear();
            m_DicForMotorOutmode.Clear();

			foreach(MOTION_TYPE en in Enum.GetValues(typeof(MOTION_TYPE)))
			{
				m_DicForMotionType.Add((int)en, en.ToString());
			}
			foreach(MOTOR_TYPE en in Enum.GetValues(typeof(MOTOR_TYPE)))
			{
				m_DicForMotorType.Add((int)en, en.ToString());
			}
            foreach (Motion_.MOTOR_INPUTMODE en in Enum.GetValues(typeof(Motion_.MOTOR_INPUTMODE)))
            {
                m_DicForMotorInmode.Add((int)en, en.ToString());
            }
            foreach (Motion_.MOTOR_OUTPUTMODE en in Enum.GetValues(typeof(Motion_.MOTOR_OUTPUTMODE)))
            {
                m_DicForMotorOutmode.Add((int)en, en.ToString());
            }
			#endregion
		}

		#region 열거형
		private enum MOTION_TYPE
		{
			LINEAR = 0,
			CIRCULAR = 1,
		}
		private enum MOTOR_TYPE
		{
			SERVO = 0,
			STEPPING = 1,
		}
		#endregion

		#region 상수
		private readonly string MIN_OF_PARAM					= "0";
		private readonly string MAX_OF_PARAM					= "999999";

		private readonly string DEFALUT_VALUE					= "--";
		private const int SELECT_NONE							= -1;
		#endregion

		#region 변수
		private int m_nIndexOfSelectedItem	= -1;

		Dictionary<int, string> m_DicForMotionType				= new Dictionary<int,string>();
		Dictionary<int, string> m_DicForMotorType				= new Dictionary<int,string>();
        Dictionary<int, string> m_DicForMotorInmode				= new Dictionary<int,string>();
		Dictionary<int, string> m_DicForMotorOutmode			= new Dictionary<int,string>();

		FrameOfSystem3.Config.ConfigMotion m_InstanceOfMotion	= null;
		Functional.Form_SelectionList m_InstanceOfSelectionList	= null;
		Functional.Form_Calculator m_InstanceOfCalculator		= null;
		#endregion

		#region 외부인터페이스
		/// <summary>
		/// 2020.06.24 by twkang [ADD] 컨트롤들의 활성 상태를 설정한다.
		/// </summary>
		public void SetActiveControls(bool bEnable)
		{
			m_labelInputScaleMPR.Enabled		= bEnable;
			m_labelInputScaleCPR.Enabled		= bEnable;
			m_labelOutputScaleMPR.Enabled		= bEnable;
            m_labelOutputScaleCPR.Enabled		= bEnable;
            m_labelMotorType.Enabled			= bEnable;

			m_labelMotionType.Enabled			= bEnable;

            m_labelCommandDirection.Enabled     = bEnable;
            m_labelEncoderDirection.Enabled     = bEnable;

			m_labelAlarmLogic.Enabled			= bEnable;
			m_labelAlarmStopMode.Enabled		= bEnable;
			m_labelZphaseLogic.Enabled			= bEnable;

            m_labelMotorInmode.Enabled          = bEnable;
            m_labelMotorOutmode.Enabled         = bEnable;
		}
		/// <summary>
		/// 2020.07.07 by twkang [ADD] 라벨 상태와, 값을 초기화한다.
		/// </summary>
		public void ResetSelectedItem()
		{
			m_nIndexOfSelectedItem				= SELECT_NONE;
			
			m_labelMotorType.Text				= DEFALUT_VALUE;
			m_labelMotionType.Text				= DEFALUT_VALUE;
			
			m_labelCommandDirection.Text		= DEFALUT_VALUE;
			m_labelEncoderDirection.Text		= DEFALUT_VALUE;

			m_labelAlarmLogic.Text				= DEFALUT_VALUE;
			m_labelAlarmStopMode.Text			= DEFALUT_VALUE;
			m_labelZphaseLogic.Text				= DEFALUT_VALUE;

			m_labelInputScaleMPR.Text			= DEFALUT_VALUE;
			m_labelInputScaleCPR.Text			= DEFALUT_VALUE;
			m_labelOutputScaleMPR.Text			= DEFALUT_VALUE;
			m_labelOutputScaleCPR.Text			= DEFALUT_VALUE;
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
			int nValue				= -1;
			bool bResult			= false;

			#region CONFIG
			if (m_InstanceOfMotion.GetConfigParameter(nIndexOfItem, FrameOfSystem3.Config.ConfigMotion.EN_PARAM_CONFIG.MOTOR_TYPE, ref nValue))
			{
				if(m_DicForMotorType.ContainsKey(nValue))
				{
					m_labelMotorType.Text	= m_DicForMotorType[nValue];
				}
			}			
			if(m_InstanceOfMotion.GetConfigParameter(nIndexOfItem, FrameOfSystem3.Config.ConfigMotion.EN_PARAM_CONFIG.MOTION_TYPE, ref nValue))
			{
				if(m_DicForMotionType.ContainsKey(nValue))
				{
					m_labelMotionType.Text	= m_DicForMotionType[nValue];
				}
			}
            if (m_InstanceOfMotion.GetConfigParameter(nIndexOfItem, FrameOfSystem3.Config.ConfigMotion.EN_PARAM_CONFIG.MOTOR_INPUT, ref nValue))
            {
                if (m_DicForMotorInmode.ContainsKey(nValue))
                {
                    m_labelMotorInmode.Text     = m_DicForMotorInmode[nValue];
                }
            }
            if (m_InstanceOfMotion.GetConfigParameter(nIndexOfItem, FrameOfSystem3.Config.ConfigMotion.EN_PARAM_CONFIG.MOTOR_OUTPUT, ref nValue))
            {
                if (m_DicForMotorOutmode.ContainsKey(nValue))
                {
                    m_labelMotorOutmode.Text    = m_DicForMotorOutmode[nValue];
                }
            }
			if (m_InstanceOfMotion.GetConfigParameter(nIndexOfItem, FrameOfSystem3.Config.ConfigMotion.EN_PARAM_CONFIG.DIR_COMMAND, ref bResult))
			{
				m_labelCommandDirection.Text = bResult ? Define.DefineConstant.Motion.INVERT_OFF : Define.DefineConstant.Motion.INVERT_ON;				
			}
			
			if (m_InstanceOfMotion.GetConfigParameter(nIndexOfItem, FrameOfSystem3.Config.ConfigMotion.EN_PARAM_CONFIG.DIR_ENCODER, ref bResult))
			{
				m_labelEncoderDirection.Text = bResult ? Define.DefineConstant.Motion.INVERT_OFF : Define.DefineConstant.Motion.INVERT_ON;
			}

			if (m_InstanceOfMotion.GetConfigParameter(nIndexOfItem, FrameOfSystem3.Config.ConfigMotion.EN_PARAM_CONFIG.ALARM_LOGIC, ref bResult))
			{
				m_labelAlarmLogic.Text = bResult ? Define.DefineConstant.Motion.LOGIC_ACTIVE_HIGH : Define.DefineConstant.Motion.LOGIC_ACTIVE_LOW;
			}

			if (m_InstanceOfMotion.GetConfigParameter(nIndexOfItem, FrameOfSystem3.Config.ConfigMotion.EN_PARAM_CONFIG.ALARM_STOPMODE, ref bResult))
			{
				m_labelAlarmStopMode.Text = bResult ? Define.DefineConstant.Motion.STOPMODE_EMG : Define.DefineConstant.Motion.STOPMODE_DECEL;
			}

			if (m_InstanceOfMotion.GetConfigParameter(nIndexOfItem, FrameOfSystem3.Config.ConfigMotion.EN_PARAM_CONFIG.ZPHASE_LOGIC, ref bResult))
			{
				m_labelZphaseLogic.Text = bResult ? Define.DefineConstant.Motion.LOGIC_ACTIVE_HIGH : Define.DefineConstant.Motion.LOGIC_ACTIVE_LOW;
			}
			#endregion

			#region Scale
			if(false == m_InstanceOfMotion.GetConfigParameter(nIndexOfItem, FrameOfSystem3.Config.ConfigMotion.EN_PARAM_CONFIG.INPUT_SCALE_CPR, ref strValue))
			{
				strValue = string.Empty;
			}
			m_labelInputScaleCPR.Text	= strValue;

			if (false == m_InstanceOfMotion.GetConfigParameter(nIndexOfItem, FrameOfSystem3.Config.ConfigMotion.EN_PARAM_CONFIG.INPUT_SCALE_MPR, ref strValue))
			{
				strValue = string.Empty;
			}
			m_labelInputScaleMPR.Text = strValue;

			if (false == m_InstanceOfMotion.GetConfigParameter(nIndexOfItem, FrameOfSystem3.Config.ConfigMotion.EN_PARAM_CONFIG.OUTPUT_SCALE_CPR, ref strValue))
			{
				strValue = string.Empty;
			}
			m_labelOutputScaleCPR.Text = strValue;

			if (false == m_InstanceOfMotion.GetConfigParameter(nIndexOfItem, FrameOfSystem3.Config.ConfigMotion.EN_PARAM_CONFIG.OUTPUT_SCALE_MPR, ref strValue))
			{
				strValue = string.Empty;
			}
			m_labelOutputScaleMPR.Text = strValue;
			#endregion
		}
		#endregion

		#region 내부인터페이스
		
		#endregion

		#region UI이벤트
		/// <summary>
		/// 2020.06.24 by twkang [ADD] CONFIG 라벨 클릭 이벤트
		/// </summary>
		private void Click_ConfigLabel(object sender, EventArgs e)
		{
			Control ctrl		= sender as Control;

			string strValue		= string.Empty;

			switch(ctrl.TabIndex)
			{
				case 0: // MOTOR TYPE
					if(m_InstanceOfSelectionList.CreateForm(m_lblMotorType.Text
						, Define.DefineEnumProject.SelectionList.EN_SELECTIONLIST.MOTION_MOTOR_TYPE
						, m_labelMotorType.Text))
					{
						m_InstanceOfSelectionList.GetResult(ref strValue);
						if(m_InstanceOfMotion.SetConfigParameter(m_nIndexOfSelectedItem, FrameOfSystem3.Config.ConfigMotion.EN_PARAM_CONFIG.MOTOR_TYPE, strValue))
						{
							m_labelMotorType.Text	= strValue;
						}
					}
					break;
				case 1: // MOTION TYPE
					if (m_InstanceOfSelectionList.CreateForm(m_lblMotorType.Text, Define.DefineEnumProject.SelectionList.EN_SELECTIONLIST.MOTION_MOTION_TYPE, m_labelMotionType.Text))
					{
						m_InstanceOfSelectionList.GetResult(ref strValue);
						if (m_InstanceOfMotion.SetConfigParameter(m_nIndexOfSelectedItem, FrameOfSystem3.Config.ConfigMotion.EN_PARAM_CONFIG.MOTION_TYPE, strValue))
						{
							m_labelMotionType.Text = strValue;
						}
					}
					break;
				case 2: // COMMAND DIRECTION
					if(m_InstanceOfSelectionList.CreateForm(m_lblCommandDirection.Text
						, Define.DefineEnumProject.SelectionList.EN_SELECTIONLIST.MOTION_MOTOR_DIRECTION
						, m_labelCommandDirection.Text))
					{
						m_InstanceOfSelectionList.GetResult(ref strValue);
						if(m_InstanceOfMotion.SetConfigParameter(m_nIndexOfSelectedItem, FrameOfSystem3.Config.ConfigMotion.EN_PARAM_CONFIG.DIR_COMMAND, strValue))
						{
							m_labelCommandDirection.Text	= strValue;
						}
					}
					break;
				case 3:	// ENCODER
					if(m_InstanceOfSelectionList.CreateForm(m_labelEncoderDirection.Text
						, Define.DefineEnumProject.SelectionList.EN_SELECTIONLIST.MOTION_MOTOR_DIRECTION
						, m_labelEncoderDirection.Text))
					{
						m_InstanceOfSelectionList.GetResult(ref strValue);
						if(m_InstanceOfMotion.SetConfigParameter(m_nIndexOfSelectedItem, FrameOfSystem3.Config.ConfigMotion.EN_PARAM_CONFIG.DIR_ENCODER, strValue))
						{
							m_labelEncoderDirection.Text	= strValue;
						}
					}
					break;
				case 4: // ALARM LOGIC
					if(m_InstanceOfSelectionList.CreateForm(m_labelAlarmLogic.Text
						, Define.DefineEnumProject.SelectionList.EN_SELECTIONLIST.MOTION_LOGIC
						, m_labelAlarmLogic.Text))
					{
						m_InstanceOfSelectionList.GetResult(ref strValue);
						if(m_InstanceOfMotion.SetConfigParameter(m_nIndexOfSelectedItem
							, FrameOfSystem3.Config.ConfigMotion.EN_PARAM_CONFIG.ALARM_LOGIC
							, strValue))
						{
							m_labelAlarmLogic.Text	= strValue;
						}
					}
					break;
				case 5: // ALARM STOP MODE
					if (m_InstanceOfSelectionList.CreateForm(m_labelAlarmStopMode.Text
						, Define.DefineEnumProject.SelectionList.EN_SELECTIONLIST.MOTION_LIMIT_STOP_MODE
						, m_labelAlarmStopMode.Text))
					{
						m_InstanceOfSelectionList.GetResult(ref strValue);
						if (m_InstanceOfMotion.SetConfigParameter(m_nIndexOfSelectedItem
							, FrameOfSystem3.Config.ConfigMotion.EN_PARAM_CONFIG.LIMIT_NEGATIVE_STOPMODE
							, strValue))
						{
							m_labelAlarmStopMode.Text = strValue;
						}
					}
					break;
				case 6: // ZPHASE LOGIC
					if (m_InstanceOfSelectionList.CreateForm(m_labelZphaseLogic.Text
						, Define.DefineEnumProject.SelectionList.EN_SELECTIONLIST.MOTION_LOGIC
						, m_labelZphaseLogic.Text))
					{
						m_InstanceOfSelectionList.GetResult(ref strValue);
						if (m_InstanceOfMotion.SetConfigParameter(m_nIndexOfSelectedItem
							, FrameOfSystem3.Config.ConfigMotion.EN_PARAM_CONFIG.LIMIT_POSITIVE_LOGIC
							, strValue))
						{
							m_labelZphaseLogic.Text = strValue;
						}
					}
                    break;

                case 7: // IN-MODE
                    if (m_InstanceOfSelectionList.CreateForm(m_lblMotorInmode.Text
						, Define.DefineEnumProject.SelectionList.EN_SELECTIONLIST.MOTION_INMODE
						, m_labelMotorInmode.Text))
					{
						m_InstanceOfSelectionList.GetResult(ref strValue);

						if (m_InstanceOfMotion.SetConfigParameter(m_nIndexOfSelectedItem
							, FrameOfSystem3.Config.ConfigMotion.EN_PARAM_CONFIG.MOTOR_INPUT
							, strValue))
						{
							m_labelMotorInmode.Text = strValue;
						}
					}
                    break;

                case 8: // OUT-MODE
                    if (m_InstanceOfSelectionList.CreateForm(m_lblMotorOutmode.Text
                        , Define.DefineEnumProject.SelectionList.EN_SELECTIONLIST.MOTION_OUTMODE
                        , m_labelMotorOutmode.Text))
                    {
                        m_InstanceOfSelectionList.GetResult(ref strValue);

                        if (m_InstanceOfMotion.SetConfigParameter(m_nIndexOfSelectedItem
                            , FrameOfSystem3.Config.ConfigMotion.EN_PARAM_CONFIG.MOTOR_OUTPUT
                            , strValue))
                        {
                            m_labelMotorOutmode.Text = strValue;
                        }
                    }
					break;
			}
		}
		/// <summary>
		/// 2021.01.05 by twkang [ADD] 스케일 라벨을 눌렀을 때
		/// </summary>
		private void Click_ScaleConfig(object sender, EventArgs e)
		{
			Control ctrl		= sender as Control;
			double dbValue		= 0;

			switch(ctrl.TabIndex)
			{
				case 0: // INPUT MPR (Scale)
					if (m_InstanceOfCalculator.CreateForm(m_labelInputScaleMPR.Text, MIN_OF_PARAM, MAX_OF_PARAM))
					{
						m_InstanceOfCalculator.GetResult(ref dbValue);
						if (m_InstanceOfMotion.SetConfigParameter(m_nIndexOfSelectedItem, FrameOfSystem3.Config.ConfigMotion.EN_PARAM_CONFIG.INPUT_SCALE_MPR, dbValue))
						{
							m_labelInputScaleMPR.Text = dbValue.ToString();
						}
					}
					break;
				case 1:	// INPUT CPR (Scale)
					if (m_InstanceOfCalculator.CreateForm(m_labelInputScaleCPR.Text, MIN_OF_PARAM, MAX_OF_PARAM))
					{
						m_InstanceOfCalculator.GetResult(ref dbValue);
						if (m_InstanceOfMotion.SetConfigParameter(m_nIndexOfSelectedItem, FrameOfSystem3.Config.ConfigMotion.EN_PARAM_CONFIG.INPUT_SCALE_CPR, dbValue))
						{
							m_labelInputScaleCPR.Text = dbValue.ToString();
						}
					}
					break;
				case 2:	// OUTPUT MPR (Scale)
					if (m_InstanceOfCalculator.CreateForm(m_labelOutputScaleMPR.Text, MIN_OF_PARAM, MAX_OF_PARAM))
					{
						m_InstanceOfCalculator.GetResult(ref dbValue);
						if (m_InstanceOfMotion.SetConfigParameter(m_nIndexOfSelectedItem, FrameOfSystem3.Config.ConfigMotion.EN_PARAM_CONFIG.OUTPUT_SCALE_MPR, dbValue))
						{
							m_labelOutputScaleMPR.Text = dbValue.ToString();
						}
					}
					break;
				case 3:	// OUTPUT CPR (Scale)
					if (m_InstanceOfCalculator.CreateForm(m_labelOutputScaleCPR.Text, MIN_OF_PARAM, MAX_OF_PARAM))
					{
						m_InstanceOfCalculator.GetResult(ref dbValue);
						if (m_InstanceOfMotion.SetConfigParameter(m_nIndexOfSelectedItem, FrameOfSystem3.Config.ConfigMotion.EN_PARAM_CONFIG.OUTPUT_SCALE_CPR, dbValue))
						{
							m_labelOutputScaleCPR.Text = dbValue.ToString();
						}
					}
					break;
			}
		}
		#endregion
	}
}
