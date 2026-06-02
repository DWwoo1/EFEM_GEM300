using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using Define.DefineEnumBase.Log;

namespace FrameOfSystem3.Views.Functional
{
	public partial class Form_AlarmMessage : Form
	{
		#region 싱글톤
		private static Form_AlarmMessage _Instance	= new Form_AlarmMessage();
		public static Form_AlarmMessage GetInstance()
		{
			return _Instance;
		}
		#endregion

		private Form_AlarmMessage()
		{
			InitializeComponent();
			this.DoubleBuffered	= true;

			foreach(EN_ALARM_GRADE enList in Enum.GetValues(typeof(EN_ALARM_GRADE)))
			{
				m_DicForState.Add((int)enList, enList);
			}

			#region Mapping For Task name
			int[] arTaskIndex	= null;
			string[] arTaskName	= null;
			if(FrameOfSystem3.Config.ConfigDynamicLink.GetInstance().GetListOfTask(ref arTaskIndex, ref arTaskName))
			{
				for (int nIndex = 0, nEnd = arTaskIndex.Length; nIndex < nEnd; ++nIndex)
				{
					m_dicForTaskName.Add(arTaskIndex[nIndex], arTaskName[nIndex]);
				}
			}
			#endregion
		}

		#region 열거형
		public enum EN_ALARM_GRADE
		{
			NOTICE = 1,
			WARNING = 2,
			ERROR = 3,
		}
		#endregion

		#region 상수
		private readonly string STR_BUZZER_OFF						= "BUZZER OFF";
		private readonly string STR_OK								= "OK";
		private readonly string STR_ABORT							= "ABORT";
		private readonly string STR_RESET							= "RESET";

		private const int INTERVAL_TIMER							= 100;
		#endregion

		#region 변수

		#region Mapping
		private Dictionary<int, EN_ALARM_GRADE> m_DicForState				= new Dictionary<int, EN_ALARM_GRADE>();
		private Dictionary<int, string> m_dicForTaskName					= new Dictionary<int, string>();
		#endregion

		private AlarmState		m_structAlarmState							= new AlarmState();
		private Timer m_pTimerForAlarmList									= new Timer();
		private System.Diagnostics.Stopwatch m_Stopwatch					= new System.Diagnostics.Stopwatch();

		private int[] m_nArrGeneratedAlarm									= null;
		private int m_nTopPriorityAlarm										= -1;

		private FrameOfSystem3.Config.ConfigAlarm.EN_ALARM_STATE m_enState	= FrameOfSystem3.Config.ConfigAlarm.EN_ALARM_STATE.ERROR;

		#region Instance
		private Form_AlarmMessage_BackGround m_AlarmBackGround				= Form_AlarmMessage_BackGround.GetInstance();
		private FrameOfSystem3.Config.ConfigAlarm	m_InstanceOfAlarm		= FrameOfSystem3.Config.ConfigAlarm.GetInstance();
		private FrameOfSystem3.Config.ConfigLanguage m_InstanceOfLanguage	= FrameOfSystem3.Config.ConfigLanguage.GetInstance();
		private FrameOfSystem3.Log.LogManager m_InstanceOfLog				= FrameOfSystem3.Log.LogManager.GetInstance();

		private FrameOfSystem3.Config.ConfigAlarm.TaskAlarmInformation m_InstanceOfAlarmInformation	= FrameOfSystem3.Config.ConfigAlarm.TaskAlarmInformation.GetInstance();
		#endregion

        #region Message
        private Dictionary<int, string[]> m_dicOfSubInformation         = new Dictionary<int,string[]>();
        private System.Threading.ReaderWriterLockSlim m_rwlockForSubInformation     = new System.Threading.ReaderWriterLockSlim();
        #endregion

		public bool m_bIsGeneratedAlarm		= false; // 해당 변수는 설비가 켜질 때 한번만 사용함
        public delegate void CallBackFuntion(bool bShow); // Form_Frame 타이머로 AlarmMessage_Form 을 띄워주기 위한 변수
		public event CallBackFuntion evtAlarmMessage;
		#endregion

		#region 내부인터페이스
		/// <summary>
		/// 2020.06.01 by twkang [ADD] 폼의 초기화 작업을 시작한다.
		/// </summary>
		private void InitializeState(EN_ALARM_GRADE enGrade, bool bOption)
		{
			Color colorForAlarm = new Color();
			
			switch(enGrade)
			{
				case EN_ALARM_GRADE.NOTICE:
					m_structAlarmState.useBuzzerOff		= false;
					m_structAlarmState.useAbort			= false;
					m_structAlarmState.useOK			= true;
					m_structAlarmState.usePass			= bOption;
					m_structAlarmState.useReset			= false;
					m_structAlarmState.useSkip			= false;
					colorForAlarm = Color.LimeGreen;
					break;
				case EN_ALARM_GRADE.WARNING:
					m_structAlarmState.useBuzzerOff	= true;
					m_structAlarmState.useAbort		= !bOption;
					m_structAlarmState.useOK		= false;
					m_structAlarmState.usePass		= bOption;
					m_structAlarmState.useReset		= false;
					m_structAlarmState.useSkip		= false;
					colorForAlarm = Color.FromArgb(225, 135, 0);
					break;
				case EN_ALARM_GRADE.ERROR:
					m_structAlarmState.useBuzzerOff	= true;
					m_structAlarmState.useAbort		= false;
					m_structAlarmState.useOK		= false;
					m_structAlarmState.usePass		= false;
					m_structAlarmState.useReset		= true;
					m_structAlarmState.useSkip		= false;
					colorForAlarm = Color.Red;
					break;
			}
			m_groupTitle.EdgeColor			= colorForAlarm;
			m_groupTitle.Invalidate();

			m_lblAlarmSolution.BackGroundColor	= colorForAlarm;
			m_lblTime.BackGroundColor			= colorForAlarm;
			m_lblAlarmCode.BackGroundColor		= colorForAlarm;
			m_lblAlarmList.BackGroundColor		= colorForAlarm;
			m_lblElapsed.BackGroundColor		= colorForAlarm;
			m_lblAlarmMessage.BackGroundColor	= colorForAlarm;
			m_lblSequenceNum.BackGroundColor	= colorForAlarm;
			m_lblActionName.BackGroundColor		= colorForAlarm;
		}
		/// <summary>
		/// 2020.07.13 by twkang [ADD] 폼을 초기화한다.
		/// </summary>
		private void InitializeForm()
		{
			m_btnPass.ButtonClicked			= false;
			m_btnSkip.ButtonClicked			= false;
			m_btnResetOrAbort.ButtonClicked	= false;

			m_nArrGeneratedAlarm		= null;
			m_labelElapsed.Text			= "00:00:00";
			m_labelAlarmCode.Text		= "000000";
			m_labelAlarmMessage.Text	= "AN UNKNOWN ERROR OCCUR.";
			m_labelSequenceNumber.Text	= "00";
			m_labelActionName.Text		= "--";
			m_labelSolution.Text		= "--";
			m_listWaiting.Items.Clear();
		}
		/// <summary>
		/// 2020.06.01 by twkang [ADD] 폼의 레이아웃을 설정한다.
		/// </summary>
		private void SetLayOut()
		{			
			m_btnBuzzerOffOrOk.Text		= m_structAlarmState.useBuzzerOff ? STR_BUZZER_OFF : STR_OK;

			if(m_structAlarmState.useAbort || m_structAlarmState.useReset)
			{
				m_btnResetOrAbort.Visible	= true;
				m_btnResetOrAbort.Text	= m_structAlarmState.useAbort ? STR_ABORT : STR_RESET;
			}
			else
			{
				m_btnResetOrAbort.Visible	= false;
			}
			m_btnPass.Visible = m_structAlarmState.usePass;
			m_btnSkip.Visible = m_structAlarmState.useSkip;
		}
		/// <summary>
		/// 2020.07.10 by twkang [ADD] 라벨 텍스트를 설정한다.
		/// </summary>
		private void SetLabelText(int nIndexOfItem)
		{
            int nAlarmCode          = 0;
			//int nMessageCode		= 0;
            int nSolutionCode		= 0;

			if(m_InstanceOfAlarm.GetParameter(nIndexOfItem, FrameOfSystem3.Config.ConfigAlarm.EN_PARAM_ALARM.ALARMCODE, ref nAlarmCode)
                //&& m_InstanceOfAlarm.GetParameter(nIndexOfItem, FrameOfSystem3.Config.ConfigAlarm.EN_PARAM_ALARM.MESSAGECODE, ref nMessageCode)
				&& m_InstanceOfAlarm.GetParameter(nIndexOfItem, FrameOfSystem3.Config.ConfigAlarm.EN_PARAM_ALARM.SOLUTIONCODE, ref nSolutionCode))
			{
                string strMessage			= string.Empty;
				m_labelAlarmCode.Text		= nAlarmCode.ToString();

				#region For SequenceNumber
				// Task 번호
				int nTaskNumber			= nAlarmCode / 10000;
				string strRunningAction	= string.Empty;
				string strTaskName		= string.Empty;
				int nSequenceStepNumber	= 0;

				if(m_dicForTaskName.ContainsKey(nTaskNumber))
				{
					strTaskName	= m_dicForTaskName[nTaskNumber];
					
					m_InstanceOfAlarmInformation.GetTaskInformation(strTaskName, ref strRunningAction, ref nSequenceStepNumber);
				}

				m_labelActionName.Text		= strRunningAction;
				m_labelSequenceNumber.Text	= nSequenceStepNumber.ToString();
				#endregion

				m_InstanceOfAlarm.GetAlarmMessage(ref nIndexOfItem, ref strMessage);

				m_labelAlarmMessage.Text	= strMessage;

				//if (GetMessageByMessageCode(ref nMessageCode, ref strMessage))
				//{
				//    ConvertMessageBySubInformation(ref nAlarmCode, ref strMessage);
				//
				//    m_InstanceOfAlarm.GetAlarmMessage(ref nIndexOfItem, ref strMessage);
				//}

				if(GetMessageByMessageCode(ref nSolutionCode, ref strMessage))
				{
					m_labelSolution.Text		= strMessage;
				}
			}
		}
		/// <summary>
		/// 2020.07.10 by twkang [ADD] 메세지 코드를 Language 를 통해서 문자열로 반환한다.
		/// </summary>
		private bool GetMessageByMessageCode(ref int nMessageCode, ref string strValue)
		{
			var enLanguage	= m_InstanceOfLanguage.GetLangauge();

			return m_InstanceOfLanguage.GetParameter(FrameOfSystem3.Config.ConfigLanguage.EN_TYPE_FORMAT.SENTENCE, nMessageCode, enLanguage, ref strValue);
		}		
		/// <summary>
		/// 2020.07.13 by twkang [ADD] 대기중인 알람을 리스트뷰에 추가한다.
		/// </summary>
		private void AddAlarmListView(int nIndexOfAlarm)
		{
			int nAlarmCode			= -1;
			int nMessageCode		= -1;
			string strAlarmGrade	= string.Empty;
			string strAlarmMessage	= string.Empty;

			m_InstanceOfAlarm.GetParameter(nIndexOfAlarm, FrameOfSystem3.Config.ConfigAlarm.EN_PARAM_ALARM.ALARMCODE, ref nAlarmCode);
			m_InstanceOfAlarm.GetParameter(nIndexOfAlarm, FrameOfSystem3.Config.ConfigAlarm.EN_PARAM_ALARM.GRADE, ref strAlarmGrade);
			m_InstanceOfAlarm.GetParameter(nIndexOfAlarm, FrameOfSystem3.Config.ConfigAlarm.EN_PARAM_ALARM.MESSAGECODE, ref nMessageCode);

			GetMessageByMessageCode(ref nMessageCode, ref strAlarmMessage);

            ConvertMessageBySubInformation(ref nAlarmCode, ref strAlarmMessage);

			m_listWaiting.Items.Add(string.Format("[{0:D5}] - {1} : {2}", nAlarmCode, strAlarmGrade, strAlarmMessage));
		}
		/// <summary>
		/// 2020.07.13 by twkang [ADD] 타이머에의해 호출되는함수 새로 발생한 알람 리스트를 업데이트한다.
		/// </summary>
		private void FunctionForTimerTick(object sender, EventArgs e)
		{
			m_labelElapsed.Text	= TimeSpan.FromSeconds(m_Stopwatch.Elapsed.TotalSeconds).ToString(@"hh\:mm\:ss");

			string strTime	= string.Empty;
			if (m_InstanceOfAlarm.GetGeneratedTime(m_nTopPriorityAlarm, ref strTime))
			{
				m_labelTime.Text = strTime;
			}

			int[] arGeneratedAlarm	= null;

			if(m_InstanceOfAlarm.GetListOfGeneratedAlarm(ref arGeneratedAlarm))
			{
				// 발생한 알람 정보가 다를경우
				if(m_nArrGeneratedAlarm == null || false == arGeneratedAlarm.SequenceEqual(m_nArrGeneratedAlarm))
				{
					m_listWaiting.Items.Clear();

					for (int nIndex = 1, nEnd = arGeneratedAlarm.Length; nIndex < nEnd; nIndex++)
					{
						AddAlarmListView(arGeneratedAlarm[nIndex]);
					}

					m_nArrGeneratedAlarm	= arGeneratedAlarm;
				}

			}
		}

        #region Message
        /// <summary>
        /// 2020.10.12 by yjlee [ADD] Convert a message by the sub information.
        /// </summary>
        private void ConvertMessageBySubInformation(ref int nAlarmCode, ref string strMessage)
        {
            int nCountOfToken       = Define.DefineConstant.Message.MESSAGE_TOKEN.Length;
            
            m_rwlockForSubInformation.EnterWriteLock();

            if(m_dicOfSubInformation.ContainsKey(nAlarmCode))
            {
                var arSubInformation        = m_dicOfSubInformation[nAlarmCode];

                foreach(var strSubInfo in arSubInformation)
                {
                    int nIndexOfToken       = strMessage.IndexOf(Define.DefineConstant.Message.MESSAGE_TOKEN);

                    if (0 > nIndexOfToken) {  break; }

                    strMessage      = strMessage.Remove(nIndexOfToken, nCountOfToken);
                    strMessage      = strMessage.Insert(nIndexOfToken, strSubInfo);
                }

                //m_dicOfSubInformation.Remove(nAlarmCode);
            }

            m_rwlockForSubInformation.ExitWriteLock();
        }
        #endregion

        #endregion

        #region 외부인터페이스
        /// <summary>
		/// 2020.06.01 by twkang [ADD] AlarmMessange Form 을 출력한다.
		/// </summary>
		public void CreateForm()
		{
            #region 데이터 초기화
            m_nTopPriorityAlarm = -1;
            m_nArrGeneratedAlarm = null;
            #endregion

            bool bOption = true;

            m_enState = m_InstanceOfAlarm.GetState(ref bOption);

            if (false == m_DicForState.ContainsKey((int)m_enState))
            {
                return;
            }

            m_AlarmBackGround.CreateForm((int)m_enState);

            InitializeForm();

            InitializeState(m_DicForState[(int)m_enState], bOption);

            SetLayOut();

            int[] arGeneratedAlarm = null;

            m_InstanceOfAlarm.GetListOfGeneratedAlarm(ref arGeneratedAlarm);

            m_nTopPriorityAlarm = arGeneratedAlarm[0];

            SetLabelText(m_nTopPriorityAlarm);

            #region timer
            m_pTimerForAlarmList.Interval = INTERVAL_TIMER;
            m_pTimerForAlarmList.Tick += new EventHandler(FunctionForTimerTick);
            m_pTimerForAlarmList.Start();
            #endregion

            // 2025.02.10. jhlim [ADD] 동시 접근 시 여기서 Exception 발생
            m_listWaiting.SelectedIndex = (m_listWaiting.Items.Count > 0) ? 0 : -1;

            m_Stopwatch.Restart();
            this.Show();
        }
		/// <summary>
		/// 2020.07.09 by twkang [ADD] 폼을 닫는다.
		/// </summary>
		public void CloseForm()
		{
			m_InstanceOfAlarm.WriteLog(m_nTopPriorityAlarm, EN_ALM_TYPE.RELEASED);
			m_AlarmBackGround.CloseBackGroundForm();
			m_Stopwatch.Stop();
			m_pTimerForAlarmList.Stop();
			this.Hide();
		}
		/// <summary>
		/// 2021.11.01 by twkang [ADD] 발생한 알람이 있는지 확인한다.
		/// GUI 생성이 끝나기 전에 발생한 알람 메시지를 확인하기 위함 (Form_Frame 생성자 마지막에 한번 호출한다)
		/// </summary>
		public void ManualCheckIsGeneratedAlarm()
		{
			if(m_bIsGeneratedAlarm)
			{
				evtAlarmMessage(true);

				m_bIsGeneratedAlarm	= false;
			}
		}
		#endregion

		#region UI 이벤트
		/// <summary>
		/// 2020.06.01 by twkang [ADD] Message 창 버튼 클릭 이벤트
		/// </summary>
		private void Click_Buttons(object sender, EventArgs e)
		{
			Control ctrl	    = sender as Control;

			switch(ctrl.TabIndex)
			{
				case 0: // PASS
					m_InstanceOfAlarm.SetAlarmResult(FrameOfSystem3.Config.ConfigAlarm.EN_ALARM_RESULT.PASS);
					m_btnPass.ButtonClicked		= true;
					break;
				case 1:	// SKIP
					m_InstanceOfAlarm.SetAlarmResult(FrameOfSystem3.Config.ConfigAlarm.EN_ALARM_RESULT.SKIP);
					m_btnSkip.ButtonClicked		= true;
					break;
				case 2:	// RESET, ABORT
					if(m_structAlarmState.useAbort)
					{
						m_InstanceOfAlarm.SetAlarmResult(FrameOfSystem3.Config.ConfigAlarm.EN_ALARM_RESULT.ABORT);
					}
					else
					{
						m_InstanceOfAlarm.SetAlarmResult(FrameOfSystem3.Config.ConfigAlarm.EN_ALARM_RESULT.RESET);
					}
					m_btnResetOrAbort.ButtonClicked	= true;
					break;
				case 3:	// BUZZER OFF, OK
					if(m_structAlarmState.useOK)
					{
						m_InstanceOfAlarm.SetAlarmResult(FrameOfSystem3.Config.ConfigAlarm.EN_ALARM_RESULT.RESET);
					}
					else
					{
						m_InstanceOfAlarm.SetAlarmResult(FrameOfSystem3.Config.ConfigAlarm.EN_ALARM_RESULT.BUZZER);

                        return;
					}
					break;
			}

		}
		/// <summary>
		/// 2020.07.10 by twkang [ADD] TaskOperaor 에서 호출되는 함수, AlarmMessage_Form 을 띄우거나 닫는다.
		/// </summary>
		public void SetAlarmMessageForm(bool bShow)
		{
			if(evtAlarmMessage == null) // 아직 GUI 생성이 안되어 있다
				m_bIsGeneratedAlarm	= true;
			else						// 평상시
				evtAlarmMessage(bShow);
		}

        #region Message
		// 2021.07.02 by twkang 미사용인듯 참조없음

        ///// <summary>
        ///// 2020.10.12 by yjlee [ADD] Add a subinformation to make the message.
        ///// </summary>
        //public void AddSubInformation(ref int nAlarmCode, ref string[] arSubInformation)
        //{
        //    if(null != arSubInformation)
        //    {
        //        m_rwlockForSubInformation.EnterWriteLock();
		//
        //        if (m_dicOfSubInformation.ContainsKey(nAlarmCode))
        //        {
        //            m_dicOfSubInformation[nAlarmCode] = arSubInformation;
        //        }
        //        else
        //        {
        //            m_dicOfSubInformation.Add(nAlarmCode, arSubInformation);
        //        }
		//
        //        m_rwlockForSubInformation.ExitWriteLock();
        //    }
        //}
        #endregion

        #endregion

        #region 내부 구조체
        private struct AlarmState
		{
			public bool useSkip;
			public bool usePass;
			public bool useAbort;
			public bool useReset;
			public bool useBuzzerOff;
			public bool useOK;
		}
		#endregion

		private void Form_AlarmMessage_KeyDown(object sender, KeyEventArgs e)
		{
			int nKeyCode = (int)e.KeyCode;

			switch (e.KeyCode)
			{
				case Keys.Escape: // Esc 입력 시
					if (m_structAlarmState.useAbort)
					{
						m_InstanceOfAlarm.SetAlarmResult(FrameOfSystem3.Config.ConfigAlarm.EN_ALARM_RESULT.ABORT);
					}
					else
					{
						m_InstanceOfAlarm.SetAlarmResult(FrameOfSystem3.Config.ConfigAlarm.EN_ALARM_RESULT.RESET);
					}
					m_btnResetOrAbort.ButtonClicked = true;
					break;
				default:
					break;
			}
		}
	}
}
