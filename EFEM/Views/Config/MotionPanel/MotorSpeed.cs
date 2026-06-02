using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using FrameOfSystem3.Config;

namespace FrameOfSystem3.Views.Config.MotionPanel
{
    public partial class MotorSpeed : UserControlForMainView.CustomView
    {
        public MotorSpeed()
        {
            InitializeComponent();
			
			m_InstanceOfMotionSpeed				= FrameOfSystem3.Config.ConfigMotionSpeed.GetInstance();
			m_InstanceOfSelectionList			= Functional.Form_SelectionList.GetInstance();
			m_InstanceOfCalculator				= Functional.Form_Calculator.GetInstance();
			m_InsatnceOfMessageBox				= Functional.Form_MessageBox.GetInstance();

			#region Mapping
			m_DicForSpeedContent.Clear();
			m_DicForSpeedContent.Add(m_labelRun.TabIndex, FrameOfSystem3.Config.ConfigMotion.EN_SPEED_CONTENT.RUN);
			m_DicForSpeedContent.Add(m_labelJogLow.TabIndex, FrameOfSystem3.Config.ConfigMotion.EN_SPEED_CONTENT.JOG_LOW);
			m_DicForSpeedContent.Add(m_labelJogHigh.TabIndex, FrameOfSystem3.Config.ConfigMotion.EN_SPEED_CONTENT.JOG_HIGH);
			m_DicForSpeedContent.Add(m_labelManual.TabIndex, FrameOfSystem3.Config.ConfigMotion.EN_SPEED_CONTENT.MANUAL);
			m_DicForSpeedContent.Add(m_labelCustom1.TabIndex, FrameOfSystem3.Config.ConfigMotion.EN_SPEED_CONTENT.CUSTOM_1);
			m_DicForSpeedContent.Add(m_labelCustom2.TabIndex, FrameOfSystem3.Config.ConfigMotion.EN_SPEED_CONTENT.SHORT_DISTANCE);

			m_DicForSpeedPattern.Clear();
			foreach(SPEED_PATTERN en in Enum.GetValues(typeof(SPEED_PATTERN)))
			{
				m_DicForSpeedPattern.Add((int)en, en.ToString());
			}
			#endregion
		}

		#region 열거형
		private enum SPEED_PATTERN
		{
			CONSTANT = 0,
			TRAPEZODIAL = 1,
			S_CURVE = 2,
			ANTI_VABRATION = 3,
		}
		#endregion

		#region 상수
		private readonly string DEFALUT_VALUE							= "--";
		private readonly string MIN_OF_PARAM							= "0";
		private readonly string MAX_OF_PARAM							= "999999";
		private const int SELECT_NONE									= -1;
		#endregion

		#region 변수
		private int m_nIndexOfSelectedItem								= SELECT_NONE;
		private int m_nTabIndexOfContent								= SELECT_NONE;
		
		private Sys3Controls.Sys3LedLabel m_ledSelected					= null;

		Dictionary<int, string> m_DicForSpeedPattern					= new Dictionary<int,string>();

		Dictionary<int, FrameOfSystem3.Config.ConfigMotion.EN_SPEED_CONTENT> m_DicForSpeedContent 
																		= new Dictionary<int,FrameOfSystem3.Config.ConfigMotion.EN_SPEED_CONTENT>();

		#region Instance
		FrameOfSystem3.Config.ConfigMotionSpeed m_InstanceOfMotionSpeed		= null;
		Functional.Form_MessageBox m_InsatnceOfMessageBox					= null;
		Functional.Form_SelectionList m_InstanceOfSelectionList				= null;
		Functional.Form_Calculator m_InstanceOfCalculator					= null;
		#endregion

		#endregion

		#region 외부인터페이스
		/// <summary>
		/// 2020.06.26 by twkang [ADD] 컨트롤들의 활성 상태를 설정한다.
		/// </summary>
		public void SetActiveControls(bool bEnable)
        {
            m_labelRun.Enabled          = bEnable;
            m_labelJogLow.Enabled       = bEnable;
            m_labelJogHigh.Enabled      = bEnable;
            m_labelManual.Enabled       = bEnable;
            m_labelCustom1.Enabled      = bEnable;
            m_labelCustom2.Enabled      = bEnable;

			m_nTabIndexOfContent		= -1;

			if(false == bEnable)
			{
				ResetControlValues();

				SetActiveParameterControls(false);
			}
				
        }
		/// <summary>
		/// 2021.07.26 by twkang [ADD] 
		/// </summary>
		public void SetActiveParameterControls(bool bEnable)
		{
			m_labelSpeedPattern.Enabled		= bEnable;
			m_labelVelocity.Enabled			= bEnable;
			m_labelMaxVelocity.Enabled		= bEnable;
			m_labelAccel.Enabled			= bEnable;
			m_labelDecel.Enabled			= bEnable;
			m_labelAccelTime.Enabled		= bEnable;
			m_labelDecelTime.Enabled		= bEnable;
			m_labelJerkAccel.Enabled		= bEnable;
			m_labelJerkDecel.Enabled		= bEnable;
			m_labelTimeout.Enabled			= bEnable;
			m_btnCopy.Enabled				= bEnable;

			if(bEnable == false)
			{
				m_labelShortDistance.Enabled	= false;
				m_labelShortDistanceAuto.Enabled	= false;
			}
		}
        /// <summary>
        /// 2020.12.23 by yjlee [ADD] Set the enable option of the controls.
        /// </summary>
        private void SetActiveShortDistanceControls(bool bEnable)
        {
            m_labelShortDistance.Enabled        = bEnable;
            m_labelShortDistanceAuto.Enabled    = bEnable;
        }
		/// <summary>
		/// 2020.06.26 by twkang [ADD] 컨트롤들의 값을 설정한다.
		/// </summary>
		public void SetValuesToControls(int nIndex)
		{
			ResetControlValues();

			ResetSelectedItem();

			SetActiveParameterControls(false);

			m_nIndexOfSelectedItem	= nIndex;
		}
		#endregion

		#region 내부인터페이스
		/// <summary>
		/// 2021.07.27 by twkang [ADD] 선택아이템을 초기화한다.
		/// </summary>
		private void ResetSelectedItem()
		{
			m_nTabIndexOfContent = -1;
			m_nIndexOfSelectedItem = -1;
		}
		/// <summary>
		/// 2020.06.26 by twkang [ADD] 컨트롤들의 값을 초기화한다.
		/// </summary>
		private void ResetControlValues()
		{
			if(m_ledSelected != null)
			{
				m_ledSelected.Active			= false;
				m_ledSelected					= null;
				m_labelName.Text				= "";
				m_labelSpeedPattern.Text		= DEFALUT_VALUE;

				m_labelShortDistance.Text		= DEFALUT_VALUE;
				m_labelShortDistanceAuto.Text	= DEFALUT_VALUE;
				
				m_labelVelocity.Text			= DEFALUT_VALUE;
				m_labelMaxVelocity.Text			= DEFALUT_VALUE;
				m_labelAccel.Text				= DEFALUT_VALUE;
				m_labelDecel.Text				= DEFALUT_VALUE;
				m_labelAccelTime.Text			= DEFALUT_VALUE;
				m_labelDecelTime.Text			= DEFALUT_VALUE;
				m_labelJerkAccel.Text			= DEFALUT_VALUE;
				m_labelJerkDecel.Text			= DEFALUT_VALUE;
				m_labelTimeout.Text				= DEFALUT_VALUE;
			}
		}
		/// <summary>
		/// 2020.06.26 by twkang [ADD] 라벨들의 값을 채운다.
		/// </summary>
		private void SetLabelValues(bool bUseShortDistanceControls)
		{
			if(false == m_DicForSpeedContent.ContainsKey(m_nTabIndexOfContent))
			{
				return;
			}

			string strValue			= string.Empty;
			double dbValue			= -1;
			int nValue				= -1;

			m_labelName.Text		= m_DicForSpeedContent[m_nTabIndexOfContent].ToString();

			if(false == m_InstanceOfMotionSpeed.GetSpeedParameter(m_nIndexOfSelectedItem
				, m_DicForSpeedContent[m_nTabIndexOfContent]
				, FrameOfSystem3.Config.ConfigMotionSpeed.EN_PARAM_SPEED.JERK_ACCEL
				, ref strValue))
			{
				strValue				= DEFALUT_VALUE;
			}
			m_labelJerkAccel.Text		= strValue;

			if (false == m_InstanceOfMotionSpeed.GetSpeedParameter(m_nIndexOfSelectedItem
				, m_DicForSpeedContent[m_nTabIndexOfContent]
				, FrameOfSystem3.Config.ConfigMotionSpeed.EN_PARAM_SPEED.JERK_DECEL
				, ref strValue))
			{
				strValue				= DEFALUT_VALUE;
			}
			m_labelJerkDecel.Text		= strValue;

			m_labelSpeedPattern.Text = DEFALUT_VALUE;
			if (m_InstanceOfMotionSpeed.GetSpeedParameter(m_nIndexOfSelectedItem
				, m_DicForSpeedContent[m_nTabIndexOfContent]
				, FrameOfSystem3.Config.ConfigMotionSpeed.EN_PARAM_SPEED.SPEED_PATTERN
				, ref nValue))
			{
				if(m_DicForSpeedPattern.ContainsKey(nValue))
				{
					m_labelSpeedPattern.Text = m_DicForSpeedPattern[nValue];
				}				
			}
			
			if(false == m_InstanceOfMotionSpeed.GetSpeedParameter(m_nIndexOfSelectedItem
				, m_DicForSpeedContent[m_nTabIndexOfContent]
				, FrameOfSystem3.Config.ConfigMotionSpeed.EN_PARAM_SPEED.ACCELERATION
				, ref dbValue))
			{
				dbValue				= 0;
			}
			m_labelAccel.Text			= string.Format("{0:0.000}", dbValue);

			if (false == m_InstanceOfMotionSpeed.GetSpeedParameter(m_nIndexOfSelectedItem
				, m_DicForSpeedContent[m_nTabIndexOfContent]
				, FrameOfSystem3.Config.ConfigMotionSpeed.EN_PARAM_SPEED.DECELERATION
				, ref dbValue))
			{
				dbValue				= 0;
			}
			m_labelDecel.Text			= string.Format("{0:0.000}", dbValue);

			if (false == m_InstanceOfMotionSpeed.GetSpeedParameter(m_nIndexOfSelectedItem
				, m_DicForSpeedContent[m_nTabIndexOfContent]
				, FrameOfSystem3.Config.ConfigMotionSpeed.EN_PARAM_SPEED.ACCEL_TIME
				, ref dbValue))
			{
				dbValue				= 0;
			}
			m_labelAccelTime.Text = string.Format("{0:0.000}", dbValue);

			if (false == m_InstanceOfMotionSpeed.GetSpeedParameter(m_nIndexOfSelectedItem
				, m_DicForSpeedContent[m_nTabIndexOfContent]
				, FrameOfSystem3.Config.ConfigMotionSpeed.EN_PARAM_SPEED.DECEL_TIME
				, ref dbValue))
			{
				dbValue				= 0;
			}
			m_labelDecelTime.Text = string.Format("{0:0.000}", dbValue);

			if (false == m_InstanceOfMotionSpeed.GetSpeedParameter(m_nIndexOfSelectedItem
				, m_DicForSpeedContent[m_nTabIndexOfContent]
				, FrameOfSystem3.Config.ConfigMotionSpeed.EN_PARAM_SPEED.MOTION_TIMEOUT
				, ref strValue))
			{
				strValue				= DEFALUT_VALUE;
			}
			m_labelTimeout.Text			= strValue;

			if (false == m_InstanceOfMotionSpeed.GetSpeedParameter(m_nIndexOfSelectedItem
				, m_DicForSpeedContent[m_nTabIndexOfContent]
				, FrameOfSystem3.Config.ConfigMotionSpeed.EN_PARAM_SPEED.VELOCITY
				, ref dbValue))
			{
				dbValue					= 0;
			}
			m_labelVelocity.Text		= string.Format("{0:0.000}", dbValue);

			if (false == m_InstanceOfMotionSpeed.GetSpeedParameter(m_nIndexOfSelectedItem
				, m_DicForSpeedContent[m_nTabIndexOfContent]
				, FrameOfSystem3.Config.ConfigMotionSpeed.EN_PARAM_SPEED.MAX_VELOCITY
				, ref dbValue))
			{
				dbValue					= 0;
			}
			m_labelMaxVelocity.Text     = string.Format("{0:0.000}", dbValue);

            bool bValue                 = false;

            if (false == bUseShortDistanceControls
                || false == m_InstanceOfMotionSpeed.GetSpeedParameter(m_nIndexOfSelectedItem
                , m_DicForSpeedContent[m_nTabIndexOfContent]
                , FrameOfSystem3.Config.ConfigMotionSpeed.EN_PARAM_SPEED.SHORT_DISTANCE
                , ref bValue))
            {
                m_labelShortDistance.Text   = DEFALUT_VALUE;
            }
            else
            {
                m_labelShortDistance.Text   = bValue ? Define.DefineConstant.SelectionList.ENABLE : Define.DefineConstant.SelectionList.DISABLE;
            }

            if (false == bUseShortDistanceControls
                || false == m_InstanceOfMotionSpeed.GetSpeedParameter(m_nIndexOfSelectedItem
                , m_DicForSpeedContent[m_nTabIndexOfContent]
                , FrameOfSystem3.Config.ConfigMotionSpeed.EN_PARAM_SPEED.SHORT_DISTANCE_AUTO
                , ref bValue))
            {
                m_labelShortDistanceAuto.Text   = DEFALUT_VALUE;
            }
            else
            {
                m_labelShortDistanceAuto.Text   = bValue ? Define.DefineConstant.SelectionList.ENABLE : Define.DefineConstant.SelectionList.DISABLE;
            }
		}
		#endregion

		#region UI 이벤트
		/// <summary>
		/// 2020.06.26 by twkang [ADD] Speed Contents 항목을 클릭했을 때
		/// </summary>
		private void Click_SpeedContents(object sender, EventArgs e)
		{
			Control ctrl	= sender as Control;

			if(m_nTabIndexOfContent == ctrl.TabIndex) return;

			if(m_ledSelected != null)
			{
				m_ledSelected.Active	= false;
			}

            bool bActiveShortDistanceControls   = true;

			switch(ctrl.TabIndex)
			{
				case 0:	// RUN
					m_ledSelected	= m_ledRun;
					break;
				case 1:	// JOG_LOW
					m_ledSelected	= m_ledJogLow;
					break;
				case 2:	// JOG_HIGH
					m_ledSelected	= m_ledJogHigh;
					break;
				case 3:	// MANUAL
					m_ledSelected	= m_ledManual;
					break;
				case 4:	// CUSTOM_1
					m_ledSelected	= m_ledCustom1;
					break;	
				case 5: // SHORT_DISTANCE
					m_ledSelected	                = m_ledCustom2;
                    bActiveShortDistanceControls    = false;
					break;
			}

			m_ledSelected.Active	= true;
			m_nTabIndexOfContent	= ctrl.TabIndex;

			SetActiveParameterControls(true);
			SetLabelValues(bActiveShortDistanceControls);
            SetActiveShortDistanceControls(bActiveShortDistanceControls);
		}
		/// <summary>
		/// 2020.06.26 by twkang [ADD] PArameter Label 클릭 이벤트
		/// </summary>
		private void Click_Parameters(object sender, EventArgs e)
		{
			Control ctrl		= sender as Control;

			double dbResult		= 0;
			string strValue		= string.Empty;

			if(false == m_DicForSpeedContent.ContainsKey(m_nTabIndexOfContent))
			{
				return;
			}

			switch(ctrl.TabIndex)
			{
				case 0: // SPEED PATTERN
					if(m_InstanceOfSelectionList.CreateForm(m_lblSpeedPattern.Text
						, Define.DefineEnumProject.SelectionList.EN_SELECTIONLIST.MOTION_SPEED_PATTERN
						, m_labelSpeedPattern.Text))
					{
						m_InstanceOfSelectionList.GetResult(ref strValue);
						if(m_InstanceOfMotionSpeed.SetSpeedParameter(m_nIndexOfSelectedItem, m_DicForSpeedContent[m_nTabIndexOfContent], FrameOfSystem3.Config.ConfigMotionSpeed.EN_PARAM_SPEED.SPEED_PATTERN, strValue))
						{
							m_labelSpeedPattern.Text	= strValue;
						}
					}
					break;
				case 1:	// VELOCITY
					if(m_InstanceOfCalculator.CreateForm(m_labelVelocity.Text, MIN_OF_PARAM, MAX_OF_PARAM))
					{
						m_InstanceOfCalculator.GetResult(ref dbResult);
						if(m_InstanceOfMotionSpeed.SetSpeedParameter(m_nIndexOfSelectedItem
							, m_DicForSpeedContent[m_nTabIndexOfContent]
							, FrameOfSystem3.Config.ConfigMotionSpeed.EN_PARAM_SPEED.VELOCITY
							, dbResult))
						{
							m_labelVelocity.Text	= dbResult.ToString();
						}
					}
					break;
				case 2:	// ACCELERATION TIME
					if(m_InstanceOfCalculator.CreateForm(m_labelAccelTime.Text, MIN_OF_PARAM, MAX_OF_PARAM))
					{
						m_InstanceOfCalculator.GetResult(ref dbResult);
						if(m_InstanceOfMotionSpeed.SetSpeedParameter(m_nIndexOfSelectedItem
							, m_DicForSpeedContent[m_nTabIndexOfContent]
							, FrameOfSystem3.Config.ConfigMotionSpeed.EN_PARAM_SPEED.ACCEL_TIME
							, dbResult))
						{
							m_labelAccelTime.Text	= dbResult.ToString();
						}
					}
					break;
				case 3:	// ACCELERATION JERK
					if(m_InstanceOfCalculator.CreateForm(m_labelJerkAccel.Text, MIN_OF_PARAM, MAX_OF_PARAM))
					{
						m_InstanceOfCalculator.GetResult(ref dbResult);
						if(m_InstanceOfMotionSpeed.SetSpeedParameter(m_nIndexOfSelectedItem
							, m_DicForSpeedContent[m_nTabIndexOfContent]
							, FrameOfSystem3.Config.ConfigMotionSpeed.EN_PARAM_SPEED.JERK_ACCEL
							, dbResult))
						{
							m_labelJerkAccel.Text	= dbResult.ToString();
						}
					}
					break;
				case 4:	// MOTION TIMEOUT
					if(m_InstanceOfCalculator.CreateForm(m_labelTimeout.Text, MIN_OF_PARAM, MAX_OF_PARAM))
					{
						m_InstanceOfCalculator.GetResult(ref dbResult);
						if(m_InstanceOfMotionSpeed.SetSpeedParameter(m_nIndexOfSelectedItem
							, m_DicForSpeedContent[m_nTabIndexOfContent]
							, FrameOfSystem3.Config.ConfigMotionSpeed.EN_PARAM_SPEED.MOTION_TIMEOUT
							, dbResult))
						{
							m_labelTimeout.Text	= dbResult.ToString();
						}
					}
					break;
				case 5:	// MAX VELOCITY
					if(m_InstanceOfCalculator.CreateForm(m_labelMaxVelocity.Text, MIN_OF_PARAM, MAX_OF_PARAM))
					{
						m_InstanceOfCalculator.GetResult(ref dbResult);
						if(m_InstanceOfMotionSpeed.SetSpeedParameter(m_nIndexOfSelectedItem
							, m_DicForSpeedContent[m_nTabIndexOfContent]
							, FrameOfSystem3.Config.ConfigMotionSpeed.EN_PARAM_SPEED.MAX_VELOCITY
							, dbResult))
						{
							m_labelMaxVelocity.Text	= dbResult.ToString();
						}
					}
					break;
				case 6:	// DECELERATION TIME
					if(m_InstanceOfCalculator.CreateForm(m_labelDecelTime.Text, MIN_OF_PARAM, MAX_OF_PARAM))
					{
						m_InstanceOfCalculator.GetResult(ref dbResult);
						if(m_InstanceOfMotionSpeed.SetSpeedParameter(m_nIndexOfSelectedItem
							, m_DicForSpeedContent[m_nTabIndexOfContent]
							, FrameOfSystem3.Config.ConfigMotionSpeed.EN_PARAM_SPEED.DECEL_TIME
							, dbResult))
						{
							m_labelDecelTime.Text	= dbResult.ToString();
						}
					}
					break;
				case 7:	// DECELERATION JERK
					if(m_InstanceOfCalculator.CreateForm(m_labelJerkDecel.Text, MIN_OF_PARAM, MAX_OF_PARAM))
					{
						m_InstanceOfCalculator.GetResult(ref dbResult);
						if(m_InstanceOfMotionSpeed.SetSpeedParameter(m_nIndexOfSelectedItem
							, m_DicForSpeedContent[m_nTabIndexOfContent]
							, FrameOfSystem3.Config.ConfigMotionSpeed.EN_PARAM_SPEED.JERK_DECEL
							, dbResult))
						{
							m_labelJerkDecel.Text	= dbResult.ToString();
						}
					}
					break;

                case 8:
                    if(m_InstanceOfSelectionList.CreateForm(m_lblShortDistance.Text
                        , Define.DefineEnumProject.SelectionList.EN_SELECTIONLIST.ENABLE_DISABLE
                        , m_labelShortDistance.Text))
                    {
                        m_InstanceOfSelectionList.GetResult(ref strValue);

                        if (m_InstanceOfMotionSpeed.SetSpeedParameter(m_nIndexOfSelectedItem
                            , m_DicForSpeedContent[m_nTabIndexOfContent]
                            , FrameOfSystem3.Config.ConfigMotionSpeed.EN_PARAM_SPEED.SHORT_DISTANCE
                            , strValue))
                        {
                            m_labelShortDistance.Text       = strValue;
                        }
                    }
                    break;

                case 9:
                    if (m_InstanceOfSelectionList.CreateForm(m_lblShortDistanceAuto.Text
                        , Define.DefineEnumProject.SelectionList.EN_SELECTIONLIST.ENABLE_DISABLE
                        , m_labelShortDistanceAuto.Text))
                    {
                        m_InstanceOfSelectionList.GetResult(ref strValue);

                        if (m_InstanceOfMotionSpeed.SetSpeedParameter(m_nIndexOfSelectedItem
                            , m_DicForSpeedContent[m_nTabIndexOfContent]
                            , FrameOfSystem3.Config.ConfigMotionSpeed.EN_PARAM_SPEED.SHORT_DISTANCE_AUTO
                            , strValue))
                        {
                            m_labelShortDistanceAuto.Text   = strValue;
                        }
                    }
                    break;
					
				case 10:
					if(m_InstanceOfCalculator.CreateForm(m_labelAccel.Text, MIN_OF_PARAM, MAX_OF_PARAM))
					{
						m_InstanceOfCalculator.GetResult(ref dbResult);
						if(m_InstanceOfMotionSpeed.SetSpeedParameter(m_nIndexOfSelectedItem
							, m_DicForSpeedContent[m_nTabIndexOfContent]
							, FrameOfSystem3.Config.ConfigMotionSpeed.EN_PARAM_SPEED.ACCELERATION
							, dbResult))
						{
							m_labelAccel.Text	= dbResult.ToString();
						}
					}
					break;

				case 11:
					if (m_InstanceOfCalculator.CreateForm(m_labelDecel.Text, MIN_OF_PARAM, MAX_OF_PARAM))
					{
						m_InstanceOfCalculator.GetResult(ref dbResult);
						if (m_InstanceOfMotionSpeed.SetSpeedParameter(m_nIndexOfSelectedItem
							, m_DicForSpeedContent[m_nTabIndexOfContent]
							, FrameOfSystem3.Config.ConfigMotionSpeed.EN_PARAM_SPEED.DECELERATION
							, dbResult))
						{
							m_labelDecel.Text = dbResult.ToString();
						}
					}
					break;
			}

            SetLabelValues(true);
		}
		/// <summary>
		/// 2021.07.26 by twkang [ADD] 스피드 파라미터 복사
		/// </summary>
		private void Click_CopySpeedParameter(object sender, EventArgs e)
		{
			switch(m_nTabIndexOfContent)
			{
				case 0:	// RUN
				case 1:	// JOG_LOW
				case 2:	// JOG_HIGH
				case 3:	// MANUAL
				case 4:	// CUSTOM_1
				case 5: // SHORT_DISTANCE
					break;
				default:
					return;
			}

			List<string> listForMakeSelectionList		= new List<string>();
			string strPreValue							= string.Empty;

			foreach(ConfigMotion.EN_SPEED_CONTENT enSpeed in Enum.GetValues(typeof(ConfigMotion.EN_SPEED_CONTENT)))
			{
				if(m_nTabIndexOfContent == (int)enSpeed)
					strPreValue	= enSpeed.ToString();
				else
					listForMakeSelectionList.Add(enSpeed.ToString());
			}

			string[] arSelectionList	= listForMakeSelectionList.ToArray();

			if(false == m_InstanceOfSelectionList.CreateForm("Select Copy Paramter",arSelectionList, strPreValue, true))
				return ;

			int[] arSelectedItem	= null;
			string strResult		= string.Empty;

			m_InstanceOfSelectionList.GetResult(ref arSelectedItem);
			m_InstanceOfSelectionList.GetResult(ref strResult);
			
			if(m_InsatnceOfMessageBox.ShowMessage(string.Format("Copy [{0}] SpeedParameter to [{1}]?", strPreValue, strResult)))
			{
				string[] arItem	= strResult.Split(new string[] {", "}, StringSplitOptions.None);

				ConfigMotion.EN_SPEED_CONTENT[] arTarget	= new ConfigMotion.EN_SPEED_CONTENT[arSelectedItem.Length];

				bool bResult	= true;

				for(int nIndex = 0, nEnd = arItem.Length; nIndex < nEnd; ++nIndex)
				{
					 bResult &= Enum.TryParse(arItem[nIndex], out arTarget[nIndex]);
				}

				if(bResult)
					m_InstanceOfMotionSpeed.CopySpeedParamter(m_nIndexOfSelectedItem, m_DicForSpeedContent[m_nTabIndexOfContent], arTarget);
			}
		}
		#endregion
	}
}
