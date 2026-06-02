using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Motion_;
using FileBorn_;

using FileComposite_;
using FileIOManager_;

namespace FrameOfSystem3.Config
{
	extern alias MotionInstance;
	public class ConfigMotion
	{
		private ConfigMotion() { }

		#region 싱글톤
		private static ConfigMotion _Instance	= new ConfigMotion();
		public static ConfigMotion GetInstance()
		{
			return _Instance;
		}
		#endregion

		#region 열거형
		public enum EN_PARAM_MOTION
		{
			ENABLE,
			NAME,
			POWER,
			TAG_NO,
			TARGET,
		}
		public enum EN_PARAM_CONFIG
		{
			MOTOR_TYPE      = 0,
			MOTION_TYPE,
            MOTOR_INPUT,
            MOTOR_OUTPUT,
			INPUT_SCALE_CPR,
			INPUT_SCALE_MPR,
			OUTPUT_SCALE_CPR,
			OUTPUT_SCALE_MPR,
			DIR_COMMAND,
			DIR_ENCODER,
			ALARM_LOGIC,
			ALARM_STOPMODE,
			ZPHASE_LOGIC,
			LIMIT_POSITIVE_ENABLE ,
			LIMIT_POSITIVE_LOGIC,
			LIMIT_POSITIVE_STOPMODE,
			LIMIT_NEGATIVE_ENABLE,
			LIMIT_NEGATIVE_LOGIC,
			LIMIT_NEGATIVE_STOPMODE,
			SWLIMIT_POSITIVE_ENABLE,
			SWLIMIT_POSITIVE_POSITION,
			SWLIMIT_POSITIVE_STOPMODE,
			SWLIMIT_NEGATIVE_ENABLE,
			SWLIMIT_NEGATIVE_POSITION,
			SWLIMIT_NEGATIVE_STOPMODE,
		}
		public enum EN_PARAM_HOME
		{
			ENABLE_HOME_END = 0,
			HOME_MODE = 1,
			HOME_LOGIC = 2,
			HOME_DIRECTION = 3,
			HOME_ESCAPE_DISTANCE = 4,
			HOME_OFFSET = 5,
			HOME_COUNT_Z = 6,
			HOME_BASE_POSITION = 7,
			HOME_ABSOLUTE_POSITION = 8,
			TIMEOUT = 9,
			HOME_SPEED_PATTERN = 10,
			HOME_VELOCITY_START = 11,
			HOME_VELOCITY_END = 12,
			HOME_ACCEL = 13,
			HOME_DECEL = 14,
			HOME_TIME_ACCEL = 15,
			HOME_TIME_DECEL = 16,
			HOME_JERK_ACCEL = 17,
			HOME_JERK_DECEL = 18,
		}
		public enum EN_PARAM_GANTRY
		{
			ENABLE,
			INDEX_MASTER,
			INDEX_SLAVE,
			REVERSE_SLAVE_DIR
		}
		public enum EN_MOTOR_STATE
		{
			MOTOR_ON,
			ALARM,
			LIMIT_POSITIVE,
			LIMIT_NEGATIVE,
			SWLIMIT_POSITIVE,
			SWLIMIT_NEGATIVE,
			MOTION_DONE,
			MOTION_TIMEOUT,
			HOME_SWITCH,
			HOME_END,
			HOME_TIMEOUT,			
		}
		public enum EN_SPEED_CONTENT
		{
			RUN,
			JOG_LOW,
			JOG_HIGH,
			MANUAL,
			CUSTOM_1,
			SHORT_DISTANCE,
		}
		#endregion

		#region 상수
		private readonly string GROUP_CONFIG									= "CONFIG";
		private readonly string GROUP_HOME										= "HOME";
		private readonly string GROUP_GANTRY									= "GANTRY";

		private const int INTERVAL_DONE_CHECK									= 200;

		#region Default Value
		private readonly string DEFAULT_NAME									= "NONE";
		private readonly string DEFAULT_TAGNUMBER								= "0000";
		private const bool DEFAULT_ENABLE										= true;
		private const int DEFAULT_POWER											= 100;
		private const int DEFAULT_TARGET										= -1;

		private const bool DEFAULT_DIR											= true;
		private const bool DEFAULT_CONFIG_ENABLE								= false;
		private const bool DEFAULT_LOGIC										= false;
		private const bool DEFAULT_STOPMODE										= false;
		private const int DEFAULT_MOTION_TYPE									= (int)MOTION_TYPE.LINEAR;
		private const int DEFAULT_MOTOR_TYPE									= (int)MOTOR_TYPE.SERVO;
        private const int DEFAULT_MOTOR_INMODE									= (int)MOTOR_INPUTMODE.AB1X;
		private const int DEFAULT_MOTOR_OUTMODE									= (int)MOTOR_OUTPUTMODE.CWCCW_LOW;
		private const int DEFAULT_SCALE_CPR										= 1;
		private const int DEFAULT_SCALE_MPR										= 1;
		private const int DEFAULT_POSITIVE_POSITION								= 10;
		private const int DEFAULT_NEGATIVE_POSITION								= 10;

		private const bool DEFAULT_GANTRY_ENALBE								= false;
		private const int DEFAULT_INDEX_MASTER									= -1;
		private const int DEFAULT_INDEX_SLAVE									= -1;
		private const bool DEFAULT_REVERSE_SLAVE_DIR							= false;

		private const bool DEFAULT_ENABLE_HOME_END								= false;
		private const int DEFAULT_HOME_BASE_POSITION							= 0;
		private const int DEFAULT_HOMH_ABSOLUTE_POSITION						= 0;
		private const int DEFAULT_HOME_COUNT_Z									= 0;
		private const bool DEFAULT_HOME_DIR										= false;
		private const int DEFAULT_HOME_ESCAPE_DISTANCE							= 0;
		private const int DEFAULT_HOME_JERK_ACCEL								= 1;
		private const int DEFAULT_HOME_JERK_DECEL								= 1;
		private const int DEFAULT_HOME_MODE										= (int)HOME_MODE.ORG_EZ_TWOSTEP_STOP;
		private const int DEFAULT_HOME_OFFSET									= 10;
		private const int DEFAULT_HOME_SPEED_PATTERN							= (int)MOTION_SPEED_PATTERN.S_CURVE;
		private const int DEFAULT_HOME_ACCEL_VELOCITY							= 1;
		private const int DEFAULT_HOME_DECEL_VELOCITY							= 1;
		private const int DEFAULT_HOME_VELOCITY_END								= 10;
		private const int DEFAULT_HOME_VELOCITY_START							= 10;
		private const int DEFAULT_HOME_TIME_OUT									= 60000;
		#endregion

		#endregion

		#region 변수
		private System.Threading.ReaderWriterLockSlim RWLock					= new System.Threading.ReaderWriterLockSlim();
		private System.Timers.Timer	m_TimerForRepeat							= null;
		private Dictionary<int, RepeatItem> m_DicForRepeatItem					= new Dictionary<int,RepeatItem>();
		
		#region Mapping
		Dictionary<EN_SPEED_CONTENT, MotionInstance::Motion_.MOTION_SPEED_CONTENT>	m_DicForSpeedContent
			= new Dictionary<EN_SPEED_CONTENT, MotionInstance::Motion_.MOTION_SPEED_CONTENT>();
		Dictionary<EN_PARAM_MOTION, PARAM_LIST> m_DicForMotionParam				= new Dictionary<EN_PARAM_MOTION,PARAM_LIST>();
		Dictionary<EN_PARAM_GANTRY, PARAM_LIST_GANTRY> m_DicForGantryParam		= new Dictionary<EN_PARAM_GANTRY,PARAM_LIST_GANTRY>();
		Dictionary<EN_PARAM_HOME, PARAM_LIST_HOME> m_DicForHomeParam			= new Dictionary<EN_PARAM_HOME,PARAM_LIST_HOME>();
		Dictionary<EN_PARAM_CONFIG, PARAM_LIST_CONFIG> m_DicForConfig			= new Dictionary<EN_PARAM_CONFIG,PARAM_LIST_CONFIG>();

		Dictionary<string, int> m_DicForResult									= new Dictionary<string,int>();

		Dictionary<int, string> m_DicForHomeMode								= new Dictionary<int, string>();
		Dictionary<int, string> m_DicForSpeedPattern							= new Dictionary<int, string>();
		Dictionary<int, string> m_DicForMotorType								= new Dictionary<int, string>();
		Dictionary<int, string> m_DicForMotionType								= new Dictionary<int, string>();
        Dictionary<int, string> m_DicForMotorInmode							    = new Dictionary<int, string>();
        Dictionary<int, string> m_DicForMotorOutmode							= new Dictionary<int, string>();
		#endregion

		Functional.Storage m_InstanceOfStorage									= null;
		Motion m_InstanceOfMotionDll											= null;
		ConfigMotionSpeed m_InstanceOfMotionSpeed								= ConfigMotionSpeed.GetInstance();
        ConfigInterlock m_InstanceOfInterlock                                   = ConfigInterlock.GetInstance();
        #endregion

		#region 외부인터페이스
		/// <summary>
		/// 2020.06.23 by twkang [ADD] Motion 클래스를 초기화한다.
		/// </summary>
		public bool Init()
		{
			MakeMappingTabel();
			
			m_InstanceOfStorage				= Functional.Storage.GetInstance();
			m_InstanceOfMotionDll			= Motion.GetInstance();
			
            // 2021.02.04 by yjlee [ADD] Make the mapping tables for the interpolation.
            MakeMappingForInterpolation();

            MOTION_POSITION[,] arData       = new MOTION_POSITION[5, 3];

            for(int nCol = 0 ; nCol < 5 ; ++ nCol)
            {
                for(int nRow = 0 ; nRow < 3 ; ++ nRow)
                {
                    arData[nCol, nRow]      = new MOTION_POSITION();

                    arData[nCol, nRow].dblPositionX = nRow;
                    arData[nCol, nRow].dblPositionY = nCol;
                    arData[nCol, nRow].dblResultX   = nRow * nRow;
                    arData[nCol, nRow].dblResultY   = nCol * nCol;
                }
            }
			
			// 2025.02.24 by junho [DEL] Not used
            //MakeInterpolationData(1, 2, 3, 5, ref arData);
            //LoadInterpolationData(1, 2, MOTION_INTERPOLATION.BICUBIC);

			if(false == m_InstanceOfStorage.Load(Define.DefineEnumBase.Storage.EN_STORAGE_LIST.MOTION))
			{
				return false;
			}

			InitializeParameter();

			#region InitTimer
			m_TimerForRepeat				= new System.Timers.Timer();
			m_TimerForRepeat.BeginInit();
			m_TimerForRepeat.Elapsed		+= new System.Timers.ElapsedEventHandler(CallbackFunctionForTimer);
			m_TimerForRepeat.Interval		= INTERVAL_DONE_CHECK;
			m_TimerForRepeat.EndInit();
			m_TimerForRepeat.Start();
			#endregion

			return true;
		}
		/// <summary>
		/// 2020.06.23 by twkang [ADD] 아이템을 추가한다.
		/// </summary>
		public int AddMotionItem()
		{
			int nItemNum			= m_InstanceOfMotionDll.AddItem();
			SetDefaultValue(nItemNum);

			string[] arGroup		= null;
			string[] arData			= null;

			if(FileBorn.GetInstance().GetItemFrame(BORN_LIST.MOTION, ref arGroup, nItemNum.ToString(), ref arData)
				&& m_InstanceOfStorage.AddGroupAndItem(Define.DefineEnumBase.Storage.EN_STORAGE_LIST.MOTION, ref arData)
				&& SaveParamToStorage(nItemNum))
			{
				if(m_InstanceOfMotionSpeed.AddMotionSpeedItem(nItemNum))
				{
					return nItemNum;
				}
				arGroup	= new string[] {nItemNum.ToString()};
				m_InstanceOfStorage.RemoveGroup(Define.DefineEnumBase.Storage.EN_STORAGE_LIST.MOTION, ref arGroup);
			}
			m_InstanceOfMotionDll.RemoveItem(nItemNum);
			return -1;
		}
		/// <summary>
		/// 2022.07.20 by junho [ADD] List에 있는 항목들로 아이템을 추가한다.
		/// </summary>
		public bool AddMotionItemArray(Dictionary<int, string> itemList)
		{
			foreach (KeyValuePair<int, string> kvp in itemList)
			{
				int nItemNum = m_InstanceOfMotionDll.AddItem();
				SetDefaultValue(nItemNum);

				m_InstanceOfMotionDll.SetParameter(nItemNum, PARAM_LIST.NAME, kvp.Value);
				m_InstanceOfMotionDll.SetParameter(nItemNum, PARAM_LIST.TARGET, kvp.Key);

				string[] arGroup = null;
				string[] arData = null;

				if (FileBorn.GetInstance().GetItemFrame(BORN_LIST.MOTION, ref arGroup, nItemNum.ToString(), ref arData)
				&& m_InstanceOfStorage.AddGroupAndItem(Define.DefineEnumBase.Storage.EN_STORAGE_LIST.MOTION, ref arData)
				&& SaveParamToStorage(nItemNum))
				{
					if (m_InstanceOfMotionSpeed.AddMotionSpeedItem(nItemNum))
					{
						continue;
					}
					arGroup = new string[] { nItemNum.ToString() };
					m_InstanceOfStorage.RemoveGroup(Define.DefineEnumBase.Storage.EN_STORAGE_LIST.MOTION, ref arGroup);
				}
				m_InstanceOfMotionDll.RemoveItem(nItemNum);
				return false;
			}

			return true;
		}

		/// <summary>
		/// 2020.06.23 by twkang [ADD] 아이템을 삭제한다.
		/// </summary>
		public bool RemoveItem(int nIndexOfItem)
		{
			string strGroup			= nIndexOfItem.ToString();
			string[] strArrGroup	= new string[] {strGroup};

			if(m_InstanceOfStorage.RemoveGroup(Define.DefineEnumBase.Storage.EN_STORAGE_LIST.MOTION, ref strArrGroup))
			{
				m_InstanceOfMotionSpeed.RemoveGroup(strGroup);
				return m_InstanceOfMotionDll.RemoveItem(nIndexOfItem);
			}
			return false;
		}

        /// <summary>
        /// 2020.09.24 by yjlee [ADD] Get the state of the all state with the enstate.
        /// </summary>
        public bool GetState(ref int nState, MotionInstance::Motion_.MOTOR_STATE enState)
        {
            return 1 == ((nState >> (int)enState) & 1);
        }

		#region Move 관련 함수
		/// <summary>
		/// 2020.06.26 by twkang [ADD] 홈으로 이동한다.
		/// </summary>
		public void MoveToHome(int nIndexOfItem)
		{
			m_InstanceOfMotionDll.MoveToHome(nIndexOfItem, 0);
		}
        /// <summary>
        /// 2024.09.13 by jhshin [ADD] overloading method with instance index, action name, retry count
        /// </summary>
        public void MoveToHome(int nIndexOfInstance, int nIndexOfItem, string actionName, int retry = 0)
        {
            m_InstanceOfMotionDll.MoveToHome(nIndexOfInstance,  nIndexOfItem, actionName, 0, retry);
        }
        /// <summary>
        /// 2020.06.26 by twkang [ADD] 모션을 정지시킨다.
        /// </summary>
        public void StopMotion(int nIndexOfItem, bool bEmergency)
		{
			m_InstanceOfMotionDll.StopMotion(nIndexOfItem, bEmergency, 0);
		}
		/// <summary>
		/// 2024.09.13 by jhshin [ADD] overloading method with instance index, action name, retry count
		/// </summary>
		public void StopMotion(int nIndexOfInstance, int nIndexOfItem, string actionName, bool bEmergency, int retry = 0)
        {
            m_InstanceOfMotionDll.StopMotion(nIndexOfInstance, nIndexOfItem, actionName, bEmergency, 0, captionRetried:retry);
        }
        /// <summary>
        /// 2021.11.25 by WDW [ADD] INTERLOCK 추가.
        /// 2020.06.30 by twkang [ADD] 절대좌표계로 이동시킨다.
        /// </summary>
        public MOTION_RESULT MoveAbsolutely( int nIndexOfItem, double dblDestination, MOTION_SPEED_CONTENT enContent, int nRatio, int nDelay, int nPreDelay = 0)
        {
            string strInterlockDiscription = string.Empty;
            MOTION_RESULT enReuslt = m_InstanceOfMotionDll.MoveAbsolutely(nIndexOfItem, dblDestination, 0, enContent, nRatio, nDelay, nPreDelay);//custem speed 우선 0
            if (enReuslt == MOTION_RESULT.OCCUR_INTERLOCK)
            {
                string strTitle = "INTERLOCK MOTION " + ConfigMotion.GetInstance().GetName(nIndexOfItem);
                strInterlockDiscription = m_InstanceOfInterlock.GetMotionLastInterference(nIndexOfItem);
                Views.Functional.Form_MessageBox.GetInstance().ShowMessage(strInterlockDiscription, strTitle);
            }
            else if (enReuslt != MOTION_RESULT.OK)
            {
                Views.Functional.Form_MessageBox.GetInstance().ShowMessage(enReuslt.ToString());
            }
            return enReuslt;
        }

		// 2025.03.18. by shkim [ADD] MoveAbsolutely With customSpeed 인터페이스 추가
        public MOTION_RESULT MoveAbsolutely(int nIndexOfItem, double dblDestination, double customSpeed, MOTION_SPEED_CONTENT enContent, int nRatio, int nDelay, int nPreDelay = 0)
        {
            string strInterlockDiscription = string.Empty;
            MOTION_RESULT enReuslt = m_InstanceOfMotionDll.MoveAbsolutely(nIndexOfItem, dblDestination, customSpeed, enContent, nRatio, nDelay, nPreDelay);//custem speed 우선 0
            if (enReuslt == MOTION_RESULT.OCCUR_INTERLOCK)
            {
                string strTitle = "INTERLOCK MOTION " + ConfigMotion.GetInstance().GetName(nIndexOfItem);
                strInterlockDiscription = m_InstanceOfInterlock.GetMotionLastInterference(nIndexOfItem);
                Views.Functional.Form_MessageBox.GetInstance().ShowMessage(strInterlockDiscription, strTitle);
            }
            else if (enReuslt != MOTION_RESULT.OK)
            {
                Views.Functional.Form_MessageBox.GetInstance().ShowMessage(enReuslt.ToString());
            }
            return enReuslt;
        }


        public void MoveAbsolutelyWithTorqueLimitEvent(int nIndexOfItem, double dblDestination, double dCustomSpeed, EN_SPEED_CONTENT enContent, int nRatio, int nDealy, double dCustomSettlingDistance)
        {
            string strInterlockDiscription = "";
            if (m_DicForSpeedContent.ContainsKey(enContent))
            {
                MOTION_RESULT enReuslt = m_InstanceOfMotionDll.MoveAbsolutelyWithToqueLimitPositionEvent(nIndexOfItem, dblDestination, dCustomSpeed, m_DicForSpeedContent[enContent], nRatio, nDealy, dCustomSettlingDistance);
                if (enReuslt == MOTION_RESULT.OCCUR_INTERLOCK)
                {
                    string strTitle = "INTERLOCK MOTION " + ConfigMotion.GetInstance().GetName(nIndexOfItem);
                    strInterlockDiscription = m_InstanceOfInterlock.GetMotionLastInterference(nIndexOfItem);
                    Views.Functional.Form_MessageBox.GetInstance().ShowMessage(strInterlockDiscription, strTitle);
                }
                else if (enReuslt != MOTION_RESULT.OK)
                {
                    Views.Functional.Form_MessageBox.GetInstance().ShowMessage(enReuslt.ToString());
                }
            }
        }


		/// <summary>
        /// 2021.11.25 by WDW [ADD] INTERLOCK 추가.
        /// 2020.06.30 by twkang [ADD] 상대좌표계로 이동시킨다.
		/// </summary>
        public MOTION_RESULT MoveReleatively(int nIndexOfItem, double dblDestination, MOTION_SPEED_CONTENT enContent, int nRatio, int nDelay, int nPreDelay = 0)
        {
            string strInterlockDiscription = string.Empty;
            MOTION_RESULT enReuslt = m_InstanceOfMotionDll.MoveReleatively(nIndexOfItem, dblDestination, 0, enContent, nRatio, nDelay, nPreDelay);
            if (enReuslt == MOTION_RESULT.OCCUR_INTERLOCK)
            {
                string strTitle = "INTERLOCK MOTION " + ConfigMotion.GetInstance().GetName(nIndexOfItem);
                strInterlockDiscription = m_InstanceOfInterlock.GetMotionLastInterference(nIndexOfItem);
                Views.Functional.Form_MessageBox.GetInstance().ShowMessage(strInterlockDiscription, strTitle);
            }
            else if (enReuslt != MOTION_RESULT.OK)
            {
                Views.Functional.Form_MessageBox.GetInstance().ShowMessage(enReuslt.ToString());
            }
            return enReuslt;
        }
		/// <summary>
		/// 2020.08.13 by twkang [ADD] Axis Speed 이동
		/// </summary>
        public MOTION_RESULT MoveAtSpeed(int nIndexOfItem, bool bDirection, int nRatio, int nPreDelay = 0)
		{
            return m_InstanceOfMotionDll.MoveAtSpeed(nIndexOfItem, 0, bDirection, nRatio, nPreDelay);
		}
		/// <summary>
		/// 2023.03.28 by junho [ADD] MoveAtSpeed에 CustomSpeed 추가
		/// </summary>
		public void MoveAtSpeed(int nIndexOfItem, double dCustomSpeed, bool bDirection, int nRatio, int nPreDelay = 0)
		{
			m_InstanceOfMotionDll.MoveAtSpeed(nIndexOfItem, dCustomSpeed, bDirection, nRatio, nPreDelay);
		}
		/// <summary>
		/// 2024.09.13 by jhshin [ADD] overloading method with instance index, action name, retry count
		/// </summary>
		public MOTION_RESULT MoveAtSpeed(int nIndexOfInstance, int nIndexOfItem, string actionName, bool bDirection, int nRatio, int nPreDelay = 0, int retry = 0)
        {
            return m_InstanceOfMotionDll.MoveAtSpeed(nIndexOfInstance, nIndexOfItem, actionName, 0, bDirection, nRatio, nPreDelay, retry);
        }
		/// <summary>
		/// 2024.09.13 by jhshin [ADD] overloading method with instance index, action name, retry count
		/// </summary>
		public void MoveAtSpeed(int nIndexOfInstance, int nIndexOfItem, string actionName, double dCustomSpeed, bool bDirection, int nRatio, int nPreDelay = 0, int retry = 0)
        {
            m_InstanceOfMotionDll.MoveAtSpeed(nIndexOfInstance, nIndexOfItem, actionName, dCustomSpeed, bDirection, nRatio, nPreDelay, retry);
        }
        #endregion

        #region 상태 관련 함수
        /// <summary>
        /// 2020.06.26 by twkang [ADD] 해당아이템을 리셋시킨다.
        /// </summary>
        public void DoReset(int nIndexOfItem)
		{
			m_InstanceOfMotionDll.DoReset(nIndexOfItem);
		}
		/// <summary>
		/// 2020.06.26 by twkang [ADD] 
		/// </summary>
		public void DoMotorOn(int nIndexOfItem, bool bOn)
		{
			m_InstanceOfMotionDll.DoMotorOn(nIndexOfItem, bOn);
		}
		/// <summary>
		/// 2020.06.26 by twkang [ADD] 해당아이템이 리핏중인지 확인한다.
		/// </summary>
		public bool IsRepeat(int nIndexOfItem)
		{
			RWLock.EnterReadLock();
			bool bResult	= m_DicForRepeatItem.ContainsKey(nIndexOfItem);
			RWLock.ExitReadLock();

			return bResult;
		}
		/// <summary>
		/// 2020.06.26 by twkang [ADD] 리핏 설정을 한다. 리핏중이면 정지, 정지면 리핏상태로 설정한다.
		/// </summary>
		public void SetRepeat(int nIndexOfItem, double dblPosFirst, int nDelayFirst, double dblPosSecond, int nDelaySecond)
		{
			if(IsRepeat(nIndexOfItem))
			{
				RWLock.EnterWriteLock();
				m_DicForRepeatItem.Remove(nIndexOfItem);
				RWLock.ExitWriteLock();
				return;
			}

			RWLock.EnterWriteLock();
			m_DicForRepeatItem.Add(nIndexOfItem, new RepeatItem(dblPosFirst, nDelayFirst, dblPosSecond, nDelaySecond));
			RWLock.ExitWriteLock();

            MoveAbsolutely(nIndexOfItem, dblPosFirst, MOTION_SPEED_CONTENT.RUN, 100, 0);
		}
        public bool IsAlarmState(int nIndexOfItem)
        {
            int nState = 0;

            GetMotorState(nIndexOfItem, ref nState);

            return GetState(ref nState, MotionInstance.Motion_.MOTOR_STATE.ALARM);
        }
		#endregion

		#region Get Information
        /// <summary>
        /// 2021.11.24 by wdw [ADD] Motion 아이템의 Name 를 반환한다.
        /// </summary>
        public string GetName(int nIndex)
        {
            string strName = string.Empty;

            m_InstanceOfMotionDll.GetParameter(nIndex, PARAM_LIST.NAME, ref strName);

            return strName;
        }
		/// <summary>
		/// 2020.06.25 by twkang [ADD] Motion 아이템의 Name List 를 반환한다.
		/// </summary>
		public bool GetListOfName(ref string[] nArr)
		{
			int[] arIndexes	= null;
			if(false == m_InstanceOfMotionDll.GetListOfItem(ref arIndexes))
			{
				return false;
			}

			nArr			= new string[arIndexes.Length];

			for(int nIndex = 0, nEnd = arIndexes.Length; nIndex < nEnd; nIndex++)
			{
				string strName	= string.Empty;
				m_InstanceOfMotionDll.GetParameter(arIndexes[nIndex], PARAM_LIST.NAME, ref strName);
				nArr[nIndex]	= strName;
			}

			return true;
		}
		/// <summary>
		/// 2020.06.23 by twkang [ADD] 아이템 인덱스 리스트를 받아온다.
		/// </summary>
		public bool GetListOfItem(ref int[] arIndexes)
		{
			return m_InstanceOfMotionDll.GetListOfItem(ref arIndexes);
		}
		/// <summary>
		/// 2020.06.23 by twkang [ADD] 아이템의 CommandPosition 을 반환한다.
		/// </summary>
		public double GetCommandPosition(int nIndexOfItem)
		{
			return m_InstanceOfMotionDll.GetCommandPosition(nIndexOfItem);
		}
		/// <summary>
		/// 2020.06.23 by twkang [ADD] 아이템의 ActualPosition 을 반환한다.
		/// </summary>
		public double GetActualPosition(int nIndexOfItem)
		{
			return m_InstanceOfMotionDll.GetActualPosition(nIndexOfItem);
		}
		/// <summary>
		/// 2020.11.16 by twkang [ADD] 아이템의 AbsolutePosition 을 반환한다.
		/// </summary>
		public double GetAbsolutePosition(int nIndexOfItem)
		{
			return m_InstanceOfMotionDll.GetAbsolutePosition(nIndexOfItem);
		}
		/// <summary>
		/// 2020.06.23 by twkang [ADD] 모터의 상태를 반환한다.
		/// </summary>
		public void GetMotorState(int nIndexOfItem, ref int nState)
		{
			m_InstanceOfMotionDll.GetMotorState(nIndexOfItem, ref nState);
		}
		public bool GetRepeatItemState(int nIndexOfItem, ref RepeatItem.EN_REPEAT_STATE enState)
		{
			if(false == m_DicForRepeatItem.ContainsKey(nIndexOfItem))
			{
				return false;
			}

			enState = m_DicForRepeatItem[nIndexOfItem].GetState();
			return true;
		}
		/// <summary>
		/// 2020.06.23 by twkang [ADD] Motion 의 기본 파라미터 값을 반환한다.
		/// </summary>
		public bool GetMotionParameter<T>(int nIndexOfItem, EN_PARAM_MOTION enParam, ref T tvalue)
		{
			return m_InstanceOfMotionDll.GetParameter(nIndexOfItem, m_DicForMotionParam[enParam], ref tvalue);
		}
		/// <summary>
		/// 2020.06.23 by twkang [ADD] Motion 의 Config 파라미터 값을 반환한다.
		/// </summary>
		public bool GetConfigParameter<T>(int nIndexOfItem, EN_PARAM_CONFIG enParam, ref T tValue)
		{
			return m_InstanceOfMotionDll.GetParameter(nIndexOfItem, m_DicForConfig[enParam], ref tValue);
		}
		/// <summary>
		/// 2020.06.23 by twkang [ADD] Motion 의 Home 파라미터 값을 반환한다.
		/// </summary>
		public bool GetHomeParameter<T>(int nIndexOfItem, EN_PARAM_HOME enParam, ref T tValue)
		{
			return m_InstanceOfMotionDll.GetParameter(nIndexOfItem, m_DicForHomeParam[enParam], ref tValue);
		}
		/// <summary>
		/// 2020.06.23 by twkang [ADD] Motion 의 Gantry 파라미터 값을 반환한다.
		/// </summary>
		public bool GetGantryParameter<T>(int nIndexOfItem, EN_PARAM_GANTRY enParam, ref T tValue)
		{
			return m_InstanceOfMotionDll.GetParameter(nIndexOfItem, m_DicForGantryParam[enParam], ref tValue);
		}
		/// <summary>
		/// 2020.07.21 by twkang [ADD] 리핏아이템의 딜레이 값을 반환한다.
		/// </summary>
		public bool GetRepeatDelay(int nIndexOfItem, ref int nFirstDelay, ref int nSecondDelay)
		{
			if(IsRepeat(nIndexOfItem))
			{
				RWLock.EnterReadLock();
				nFirstDelay		= m_DicForRepeatItem[nIndexOfItem].GetFirstDelay();
				nSecondDelay	= m_DicForRepeatItem[nIndexOfItem].GetSecondDelay();
				RWLock.ExitReadLock();
				return true;
			}
			return false;
		}
		/// <summary>
		/// 2020.07.21 by twkang [ADD] 리핏아이템의 포지션 값을 반한다.
		/// </summary>
		public bool GetRepeatPosition(int nIndexOfItem, ref double dbFirstPosition, ref double dbSecondPosition)
		{
			if(IsRepeat(nIndexOfItem))
			{
				RWLock.EnterReadLock();
				dbFirstPosition		= m_DicForRepeatItem[nIndexOfItem].GetFirstPosition();
				dbSecondPosition	= m_DicForRepeatItem[nIndexOfItem].GetSecondPosition();
				RWLock.ExitReadLock();
				return true;
			}
			return false;
		}

		/// <summary>
		/// 2021.06.21 by twkang [ADD] Gain Table Index 번호를 반환한다
		/// </summary>
		public bool GetGainTable(int nIndexOfItem, ref int nIndexOfTable)
		{
			return m_InstanceOfMotionDll.GetGainTable(nIndexOfItem, ref nIndexOfTable);
		}
		/// <summary>
		/// 2022.02.15 by twkang [ADD] Filter Offset Value 값을 반환한다.
		/// </summary>
		public double GetOutputOffset(int nIndexOfItem)
        {
			return m_InstanceOfMotionDll.GetOutputOffset(nIndexOfItem);
        }
		#endregion
		
		#region Set Information
		/// <summary>
		/// 2020.06.23 by twkang [ADD] Motion 의 기본 파라미터 값을 설정한다.
		/// </summary>
		public bool SetMotionParameter<T>(int nIndexOfItem, EN_PARAM_MOTION enParam, T tValue)
		{
			string[] arGroup	= new string[] {nIndexOfItem.ToString()};
			
			if(false == m_InstanceOfStorage.SetParameter(Define.DefineEnumBase.Storage.EN_STORAGE_LIST.MOTION, enParam.ToString(), tValue, ref arGroup))
			{
				return false;
			}
			
			switch(enParam)
			{
				case EN_PARAM_MOTION.NAME:
					m_InstanceOfStorage.SetParameter(Define.DefineEnumBase.Storage.EN_STORAGE_LIST.MOTION_SPEED, enParam.ToString(), tValue, ref arGroup);
					break;
				default:
					break;
			}
			return m_InstanceOfMotionDll.SetParameter(nIndexOfItem, m_DicForMotionParam[enParam], tValue);
		}
		/// <summary>
		/// 2020.06.23 by twkang [ADD] Motion 의 Config 파라미터 값을 설정한다.
		/// </summary>
		public bool SetConfigParameter<T>(int nIndexOfItem, EN_PARAM_CONFIG enParam, T tValue)
		{
			string[] arGroup	= new string[] {nIndexOfItem.ToString(), GROUP_CONFIG};

			if(false == m_InstanceOfStorage.SetParameter(Define.DefineEnumBase.Storage.EN_STORAGE_LIST.MOTION, enParam.ToString(), tValue, ref arGroup))
			{
				return false;
			}
			
			switch(enParam)
			{
				default:
					return m_InstanceOfMotionDll.SetParameter(nIndexOfItem, m_DicForConfig[enParam], tValue);							
				case EN_PARAM_CONFIG.DIR_ENCODER:
				case EN_PARAM_CONFIG.DIR_COMMAND:
					bool bDir	= tValue.ToString().Equals(Define.DefineConstant.Motion.INVERT_OFF);
					return m_InstanceOfMotionDll.SetParameter(nIndexOfItem, m_DicForConfig[enParam], bDir);
				case EN_PARAM_CONFIG.LIMIT_NEGATIVE_STOPMODE:
				case EN_PARAM_CONFIG.LIMIT_POSITIVE_STOPMODE:
				case EN_PARAM_CONFIG.SWLIMIT_NEGATIVE_STOPMODE:
				case EN_PARAM_CONFIG.SWLIMIT_POSITIVE_STOPMODE:
				case EN_PARAM_CONFIG.ALARM_STOPMODE:
					bool bInstantStop	= tValue.ToString().Equals(Define.DefineConstant.Motion.STOPMODE_EMG);
					return m_InstanceOfMotionDll.SetParameter(nIndexOfItem, m_DicForConfig[enParam], bInstantStop);
				case EN_PARAM_CONFIG.LIMIT_NEGATIVE_LOGIC:
				case EN_PARAM_CONFIG.LIMIT_POSITIVE_LOGIC:
				case EN_PARAM_CONFIG.ALARM_LOGIC:
				case EN_PARAM_CONFIG.ZPHASE_LOGIC:
					bool bActiveHigh	= tValue.ToString().Equals(Define.DefineConstant.Motion.LOGIC_ACTIVE_HIGH);
					return m_InstanceOfMotionDll.SetParameter(nIndexOfItem, m_DicForConfig[enParam], bActiveHigh);
				case EN_PARAM_CONFIG.MOTION_TYPE:
				case EN_PARAM_CONFIG.MOTOR_TYPE:
                case EN_PARAM_CONFIG.MOTOR_INPUT:
                case EN_PARAM_CONFIG.MOTOR_OUTPUT:
					if(m_DicForResult.ContainsKey(tValue.ToString()))
					{
						return m_InstanceOfMotionDll.SetParameter(nIndexOfItem, m_DicForConfig[enParam], m_DicForResult[tValue.ToString()]);
					}
					break;					
			}
			return false;
		}
		/// <summary>
		/// 2020.06.23 by twkang [ADD] Motion 의 Home 파라미터 값을 설정한다.
		/// </summary>
		public bool SetHomeParameter<T>(int nIndexOfItem, EN_PARAM_HOME enParam, T tValue)
		{
			string[] arGroup	= new string[] {nIndexOfItem.ToString(), GROUP_HOME};

			if(false == m_InstanceOfStorage.SetParameter(Define.DefineEnumBase.Storage.EN_STORAGE_LIST.MOTION, enParam.ToString(), tValue, ref arGroup))
			{
				return false;
			}
			
			switch(enParam)
			{
				default:
					return m_InstanceOfMotionDll.SetParameter(nIndexOfItem, m_DicForHomeParam[enParam], tValue);
				case EN_PARAM_HOME.HOME_LOGIC:
					bool bActiveHigh	= tValue.ToString().Equals(Define.DefineConstant.Motion.LOGIC_ACTIVE_HIGH);
					return m_InstanceOfMotionDll.SetParameter(nIndexOfItem, m_DicForHomeParam[enParam], bActiveHigh);
				case EN_PARAM_HOME.HOME_DIRECTION:
					bool bPositive		= tValue.ToString().Equals(Define.DefineConstant.Motion.DIRECTION_POSITIVE);
					return m_InstanceOfMotionDll.SetParameter(nIndexOfItem, m_DicForHomeParam[enParam], bPositive);
				case EN_PARAM_HOME.HOME_MODE:
				case EN_PARAM_HOME.HOME_SPEED_PATTERN:
					if(m_DicForResult.ContainsKey(tValue.ToString()))
					{
						return m_InstanceOfMotionDll.SetParameter(nIndexOfItem, m_DicForHomeParam[enParam], m_DicForResult[tValue.ToString()]);
					}
					break;
				case EN_PARAM_HOME.HOME_ACCEL:
				case EN_PARAM_HOME.HOME_DECEL:
				case EN_PARAM_HOME.HOME_VELOCITY_START:
				case EN_PARAM_HOME.HOME_TIME_ACCEL:
				case EN_PARAM_HOME.HOME_TIME_DECEL:
					if(m_InstanceOfMotionDll.SetParameter(nIndexOfItem, m_DicForHomeParam[enParam], tValue))
					{
						SaveParamToStorage(nIndexOfItem);
					}
					break;
			}
			return false;
		}
		/// <summary>
		/// 2020.06.23 by twkang [ADD] Motion 의 Gantry 파라미터 값을 설정한다.
		/// </summary>
		public bool SetGantryParameter<T>(int nIndexOfItem, EN_PARAM_GANTRY enParam, T tValue)
		{
			string[] arGroup	= new string[] {nIndexOfItem.ToString(), GROUP_GANTRY};

			if(false == m_InstanceOfStorage.SetParameter(Define.DefineEnumBase.Storage.EN_STORAGE_LIST.MOTION, enParam.ToString(), tValue, ref arGroup))
			{
				return false;
			}

			return m_InstanceOfMotionDll.SetParameter(nIndexOfItem, m_DicForGantryParam[enParam], tValue);
		}
		/// <summary>
		/// 2020.07.01 by twkang [ADD] 아이템의 ActualPosition 을 설정한다.
		/// </summary>
		public void SetActualPosition(int nIndexOfItem, double dblPosition)
		{
			m_InstanceOfMotionDll.SetActualPosition(nIndexOfItem, dblPosition);
		}
		/// <summary>
		/// 2020.07.01 by twkang [ADD] 아이템의 CommandPosition 을 설정한다.
		/// </summary>
		public void SetCommandPosition(int nIndexOfItem, double dblPosition)
		{
			m_InstanceOfMotionDll.SetCommandPosition(nIndexOfItem, dblPosition);
		}
		/// <summary>
		/// 2021.06.21 by twkang [ADD] Gain Table 셋
		/// </summary>
		public void SetGainTable(int nIndexOfItem, int nIndexOfTable)
		{
			m_InstanceOfMotionDll.SetGainTable(nIndexOfItem, nIndexOfTable);
		}
        /// <summary>
        /// 2022.05.20 by wdw [ADD] Gain Type 셋
        ///   // 테이블 번호
        //0 : None
        //1 : Motion_Only
        /// </summary>
        public void SetGainType(int nIndexOfItem, int nIndexOfGainType)
        {
            m_InstanceOfMotionDll.SetGainType(nIndexOfItem, nIndexOfGainType);
        }
		/// <summary>
		/// 2022.02.15 by twkang [ADD] Filter Offset Value 셋
		/// </summary>
		public void SetOutputOffset(int nIndexOfItem, double dblOffsetValue)
        {
            m_InstanceOfMotionDll.SetOutputOffset(nIndexOfItem, dblOffsetValue);
        }
        #endregion

        #endregion

        #region 내부인터페이스
        /// <summary>
        /// 2020.06.23 by twkang [ADD] Motion 클래스에서 사용할 Mapping Table 을 만든다.
        /// </summary>
        private void MakeMappingTabel()
		{
			#region MOTION_PARAM
			m_DicForMotionParam.Clear();
			m_DicForMotionParam.Add(EN_PARAM_MOTION.ENABLE, PARAM_LIST.ENABLE);
			m_DicForMotionParam.Add(EN_PARAM_MOTION.NAME, PARAM_LIST.NAME);
			m_DicForMotionParam.Add(EN_PARAM_MOTION.POWER, PARAM_LIST.POWER);
			m_DicForMotionParam.Add(EN_PARAM_MOTION.TAG_NO, PARAM_LIST.TAG_NO);
			m_DicForMotionParam.Add(EN_PARAM_MOTION.TARGET, PARAM_LIST.TARGET);
			#endregion

			#region CONFIG_PARAM
			m_DicForConfig.Clear();
			m_DicForConfig.Add(EN_PARAM_CONFIG.DIR_COMMAND, PARAM_LIST_CONFIG.DIR_COMMAND);
			m_DicForConfig.Add(EN_PARAM_CONFIG.DIR_ENCODER, PARAM_LIST_CONFIG.DIR_ENCODER);
			m_DicForConfig.Add(EN_PARAM_CONFIG.LIMIT_NEGATIVE_ENABLE, PARAM_LIST_CONFIG.LIMIT_NEGATIVE_ENABLE);
			m_DicForConfig.Add(EN_PARAM_CONFIG.LIMIT_NEGATIVE_LOGIC, PARAM_LIST_CONFIG.LIMIT_NEGATIVE_LOGIC);
			m_DicForConfig.Add(EN_PARAM_CONFIG.LIMIT_NEGATIVE_STOPMODE, PARAM_LIST_CONFIG.LIMIT_NEGATIVE_STOPMODE);
			m_DicForConfig.Add(EN_PARAM_CONFIG.LIMIT_POSITIVE_ENABLE, PARAM_LIST_CONFIG.LIMIT_POSITIVE_ENABLE);
			m_DicForConfig.Add(EN_PARAM_CONFIG.LIMIT_POSITIVE_LOGIC, PARAM_LIST_CONFIG.LIMIT_POSITIVE_LOGIC);
			m_DicForConfig.Add(EN_PARAM_CONFIG.LIMIT_POSITIVE_STOPMODE, PARAM_LIST_CONFIG.LIMIT_POSITIVE_STOPMODE);
			m_DicForConfig.Add(EN_PARAM_CONFIG.MOTOR_TYPE, PARAM_LIST_CONFIG.MOTOR_TYPE);
			m_DicForConfig.Add(EN_PARAM_CONFIG.MOTION_TYPE, PARAM_LIST_CONFIG.MOTION_TYPE);
            m_DicForConfig.Add(EN_PARAM_CONFIG.MOTOR_INPUT, PARAM_LIST_CONFIG.INPUT_MODE);
            m_DicForConfig.Add(EN_PARAM_CONFIG.MOTOR_OUTPUT, PARAM_LIST_CONFIG.OUTPUT_MODE);
			m_DicForConfig.Add(EN_PARAM_CONFIG.INPUT_SCALE_CPR, PARAM_LIST_CONFIG.INPUT_SCALE_CPR);
			m_DicForConfig.Add(EN_PARAM_CONFIG.INPUT_SCALE_MPR, PARAM_LIST_CONFIG.INPUT_SCALE_MPR);
			m_DicForConfig.Add(EN_PARAM_CONFIG.OUTPUT_SCALE_CPR, PARAM_LIST_CONFIG.OUTPUT_SCALE_CPR);
			m_DicForConfig.Add(EN_PARAM_CONFIG.OUTPUT_SCALE_MPR, PARAM_LIST_CONFIG.OUTPUT_SCALE_MPR);
			m_DicForConfig.Add(EN_PARAM_CONFIG.ALARM_LOGIC, PARAM_LIST_CONFIG.ALARM_LOGIC);
			m_DicForConfig.Add(EN_PARAM_CONFIG.ALARM_STOPMODE, PARAM_LIST_CONFIG.ALARM_STOPMODE);
			m_DicForConfig.Add(EN_PARAM_CONFIG.ZPHASE_LOGIC, PARAM_LIST_CONFIG.ZPHASE_LOGIC);
			m_DicForConfig.Add(EN_PARAM_CONFIG.SWLIMIT_NEGATIVE_ENABLE, PARAM_LIST_CONFIG.SWLIMIT_NEGATIVE_ENABLE);
			m_DicForConfig.Add(EN_PARAM_CONFIG.SWLIMIT_NEGATIVE_POSITION, PARAM_LIST_CONFIG.SWLIMIT_NEGATIVE_POSITION);
			m_DicForConfig.Add(EN_PARAM_CONFIG.SWLIMIT_NEGATIVE_STOPMODE, PARAM_LIST_CONFIG.SWLIMIT_NEGATIVE_STOPMODE);
			m_DicForConfig.Add(EN_PARAM_CONFIG.SWLIMIT_POSITIVE_ENABLE, PARAM_LIST_CONFIG.SWLIMIT_POSITIVE_ENABLE);
			m_DicForConfig.Add(EN_PARAM_CONFIG.SWLIMIT_POSITIVE_POSITION, PARAM_LIST_CONFIG.SWLIMIT_POSITIVE_POSITION);
			m_DicForConfig.Add(EN_PARAM_CONFIG.SWLIMIT_POSITIVE_STOPMODE, PARAM_LIST_CONFIG.SWLIMIT_POSITIVE_STOPMODE);
			#endregion

			#region GANTRY_PARAM
			m_DicForGantryParam.Clear();
			m_DicForGantryParam.Add(EN_PARAM_GANTRY.ENABLE, PARAM_LIST_GANTRY.ENABLE);
			m_DicForGantryParam.Add(EN_PARAM_GANTRY.INDEX_MASTER, PARAM_LIST_GANTRY.INDEX_MASTER);
			m_DicForGantryParam.Add(EN_PARAM_GANTRY.INDEX_SLAVE, PARAM_LIST_GANTRY.INDEX_SLAVE);
			m_DicForGantryParam.Add(EN_PARAM_GANTRY.REVERSE_SLAVE_DIR, PARAM_LIST_GANTRY.REVERSE_SLAVE_DIR);
			#endregion

			#region HOME_PARAM
			m_DicForHomeParam.Clear();
			m_DicForHomeParam.Add(EN_PARAM_HOME.ENABLE_HOME_END, PARAM_LIST_HOME.ENABLE_HOME_END);
			m_DicForHomeParam.Add(EN_PARAM_HOME.HOME_BASE_POSITION, PARAM_LIST_HOME.HOME_BASE_POSITION);
			m_DicForHomeParam.Add(EN_PARAM_HOME.HOME_COUNT_Z, PARAM_LIST_HOME.HOME_COUNT_Z);
			m_DicForHomeParam.Add(EN_PARAM_HOME.HOME_DIRECTION, PARAM_LIST_HOME.HOME_DIRECTION);
			m_DicForHomeParam.Add(EN_PARAM_HOME.HOME_ABSOLUTE_POSITION, PARAM_LIST_HOME.HOME_ABSOLUTE_POSITION);
			m_DicForHomeParam.Add(EN_PARAM_HOME.HOME_ESCAPE_DISTANCE, PARAM_LIST_HOME.HOME_ESCAPE_DISTANCE);
			m_DicForHomeParam.Add(EN_PARAM_HOME.HOME_JERK_ACCEL, PARAM_LIST_HOME.HOME_JERK_ACCEL);
			m_DicForHomeParam.Add(EN_PARAM_HOME.HOME_JERK_DECEL, PARAM_LIST_HOME.HOME_JERK_DECEL);
			m_DicForHomeParam.Add(EN_PARAM_HOME.HOME_LOGIC, PARAM_LIST_HOME.HOME_LOGIC);
			m_DicForHomeParam.Add(EN_PARAM_HOME.HOME_MODE, PARAM_LIST_HOME.HOME_MODE);
			m_DicForHomeParam.Add(EN_PARAM_HOME.HOME_OFFSET, PARAM_LIST_HOME.HOME_OFFSET);
			m_DicForHomeParam.Add(EN_PARAM_HOME.HOME_SPEED_PATTERN, PARAM_LIST_HOME.HOME_SPEED_PATTERN);
			m_DicForHomeParam.Add(EN_PARAM_HOME.HOME_ACCEL, PARAM_LIST_HOME.HOME_ACCEL);
			m_DicForHomeParam.Add(EN_PARAM_HOME.HOME_DECEL, PARAM_LIST_HOME.HOME_DECEL);
			m_DicForHomeParam.Add(EN_PARAM_HOME.HOME_TIME_ACCEL, PARAM_LIST_HOME.HOME_TIME_ACCEL);
			m_DicForHomeParam.Add(EN_PARAM_HOME.HOME_TIME_DECEL, PARAM_LIST_HOME.HOME_TIME_DECEL);
			m_DicForHomeParam.Add(EN_PARAM_HOME.HOME_VELOCITY_END, PARAM_LIST_HOME.HOME_VELOCITY_END);
			m_DicForHomeParam.Add(EN_PARAM_HOME.HOME_VELOCITY_START, PARAM_LIST_HOME.HOME_VELOCITY_START);
			m_DicForHomeParam.Add(EN_PARAM_HOME.TIMEOUT, PARAM_LIST_HOME.TIMEOUT);
			#endregion

			m_DicForResult.Clear();

			#region MOTOR_TYPE
			m_DicForMotorType.Clear();
			foreach(MOTOR_TYPE en in Enum.GetValues(typeof(MOTOR_TYPE)))
			{
				m_DicForResult.Add(en.ToString(), (int)en);
				m_DicForMotorType.Add((int)en, en.ToString());
			}
			#endregion

			#region MOTION_TYPE
			m_DicForMotionType.Clear();
			foreach(MOTION_TYPE en in Enum.GetValues(typeof(MOTION_TYPE)))
			{
				m_DicForResult.Add(en.ToString(), (int)en);
				m_DicForMotionType.Add((int)en, en.ToString());
			}
			#endregion

            #region MOTOR_INMODE
            m_DicForMotorInmode.Clear();
            foreach (MOTOR_INPUTMODE en in Enum.GetValues(typeof(MOTOR_INPUTMODE)))
            {
                m_DicForResult.Add(en.ToString(), (int)en);
                m_DicForMotorInmode.Add((int)en, en.ToString());
            }
            #endregion

            #region MOTOR_OUTMODE
            m_DicForMotorOutmode.Clear();
            foreach (MOTOR_OUTPUTMODE en in Enum.GetValues(typeof(MOTOR_OUTPUTMODE)))
            {
                m_DicForResult.Add(en.ToString(), (int)en);
                m_DicForMotorOutmode.Add((int)en, en.ToString());
            }
            #endregion

            #region MOTION_SPEED_PATTERN
            m_DicForSpeedPattern.Clear();
			foreach(MOTION_SPEED_PATTERN en in Enum.GetValues(typeof(MOTION_SPEED_PATTERN)))
			{
				m_DicForResult.Add(en.ToString(), (int)en);
				m_DicForSpeedPattern.Add((int)en, en.ToString());
			}
			#endregion

			#region HOME_MODE
			m_DicForHomeMode.Clear();
			foreach(HOME_MODE en in Enum.GetValues(typeof(HOME_MODE)))
			{
				m_DicForResult.Add(en.ToString(), (int)en);
				m_DicForHomeMode.Add((int)en, en.ToString());
			}
			#endregion

			#region MOTION_SPEED_CONTENT
			m_DicForSpeedContent.Clear();
			m_DicForSpeedContent.Add(EN_SPEED_CONTENT.CUSTOM_1, MotionInstance::Motion_.MOTION_SPEED_CONTENT.CUSTOM_1);
			m_DicForSpeedContent.Add(EN_SPEED_CONTENT.SHORT_DISTANCE, MotionInstance::Motion_.MOTION_SPEED_CONTENT.SHORT_DISTANCE);
			m_DicForSpeedContent.Add(EN_SPEED_CONTENT.JOG_HIGH, MotionInstance::Motion_.MOTION_SPEED_CONTENT.JOG_HIGH);
			m_DicForSpeedContent.Add(EN_SPEED_CONTENT.JOG_LOW, MotionInstance::Motion_.MOTION_SPEED_CONTENT.JOG_LOW);
			m_DicForSpeedContent.Add(EN_SPEED_CONTENT.MANUAL, MotionInstance::Motion_.MOTION_SPEED_CONTENT.MANUAL);
			m_DicForSpeedContent.Add(EN_SPEED_CONTENT.RUN, MotionInstance::Motion_.MOTION_SPEED_CONTENT.RUN);			
			#endregion
		}
		/// <summary>
		/// 2020.06.23 by twkang [ADD] Storage 데이터로 Motion 파라미터를 초기화한다.
		/// </summary>
		private void InitializeParameter()
		{
			// 2025.03.18 by junho [ADD] parameter initialize 할 때 gathering thread 정지 시키도록 개선
			m_InstanceOfMotionDll.SetGatheringThreadPause(true);
			m_InstanceOfMotionDll.ClearParameterQueue();
			
			string strValue				= string.Empty;
			string[] strArrGroup		= null;
			bool bValue					= false;
			
			if(false == m_InstanceOfStorage.GetListOfGroup(Define.DefineEnumBase.Storage.EN_STORAGE_LIST.MOTION, ref strArrGroup))
			{
				return;
			}

			for(int nIndex = 0, nEnd = strArrGroup.Length; nIndex < nEnd; ++nIndex)
			{
				string[] arGroup	= new string[] {strArrGroup[nIndex], GROUP_GANTRY};
				int nIndexOfItem	= int.Parse(strArrGroup[nIndex]);
				
				m_InstanceOfMotionDll.AddItem(nIndexOfItem);
				SetDefaultValue(nIndexOfItem);
				
				#region EN_PARAM_GANTRY
				foreach (EN_PARAM_GANTRY en in Enum.GetValues(typeof(EN_PARAM_GANTRY)))
				{
					if(m_InstanceOfStorage.GetParameter(Define.DefineEnumBase.Storage.EN_STORAGE_LIST.MOTION
						, en.ToString()
						, ref strValue
						, ref arGroup))
					{
						m_InstanceOfMotionDll.SetParameter(nIndexOfItem, m_DicForGantryParam[en], strValue);
					}
				}
				#endregion

				#region PARAM_HOME
				arGroup[1] = GROUP_HOME;
				foreach (EN_PARAM_HOME en in Enum.GetValues(typeof(EN_PARAM_HOME)))
				{
					if (m_InstanceOfStorage.GetParameter(Define.DefineEnumBase.Storage.EN_STORAGE_LIST.MOTION
						, en.ToString()
						, ref strValue
						, ref arGroup))
					{
						switch(en)
						{
							case EN_PARAM_HOME.HOME_DIRECTION:
								bValue	= strValue.Equals(Define.DefineConstant.Motion.DIRECTION_POSITIVE);
								m_InstanceOfMotionDll.SetParameter(nIndexOfItem, m_DicForHomeParam[en], bValue);
								break;
							case EN_PARAM_HOME.HOME_LOGIC:
								bValue	= strValue.Equals(Define.DefineConstant.Motion.LOGIC_ACTIVE_HIGH);
								m_InstanceOfMotionDll.SetParameter(nIndexOfItem, m_DicForHomeParam[en], bValue);
								break;
							case EN_PARAM_HOME.HOME_MODE:
							case EN_PARAM_HOME.HOME_SPEED_PATTERN:
								if(m_DicForResult.ContainsKey(strValue))
								{
									m_InstanceOfMotionDll.SetParameter(nIndexOfItem, m_DicForHomeParam[en], m_DicForResult[strValue]);
								}
								break;
							default:
								m_InstanceOfMotionDll.SetParameter(nIndexOfItem, m_DicForHomeParam[en], strValue);
								break;
						}
					}
				}
				#endregion
				
				#region PARAM_CONFIG
				arGroup[1] = GROUP_CONFIG;
				foreach (EN_PARAM_CONFIG en in Enum.GetValues(typeof(EN_PARAM_CONFIG)))
				{
					if (m_InstanceOfStorage.GetParameter(Define.DefineEnumBase.Storage.EN_STORAGE_LIST.MOTION
						, en.ToString()
						, ref strValue
						, ref arGroup))
					{
						switch (en)
						{
							case EN_PARAM_CONFIG.DIR_COMMAND:
							case EN_PARAM_CONFIG.DIR_ENCODER:
								bValue	= strValue.Equals(Define.DefineConstant.Motion.INVERT_OFF);
								m_InstanceOfMotionDll.SetParameter(nIndexOfItem, m_DicForConfig[en], bValue);
								break;
							case EN_PARAM_CONFIG.SWLIMIT_NEGATIVE_ENABLE:
							case EN_PARAM_CONFIG.SWLIMIT_POSITIVE_ENABLE:
							case EN_PARAM_CONFIG.LIMIT_NEGATIVE_ENABLE:
							case EN_PARAM_CONFIG.LIMIT_POSITIVE_ENABLE:
							case EN_PARAM_CONFIG.INPUT_SCALE_CPR:
							case EN_PARAM_CONFIG.INPUT_SCALE_MPR:
							case EN_PARAM_CONFIG.OUTPUT_SCALE_MPR:
							case EN_PARAM_CONFIG.OUTPUT_SCALE_CPR:
							case EN_PARAM_CONFIG.SWLIMIT_NEGATIVE_POSITION:
							case EN_PARAM_CONFIG.SWLIMIT_POSITIVE_POSITION:
								m_InstanceOfMotionDll.SetParameter(nIndexOfItem, m_DicForConfig[en], strValue);
								break;
                            case EN_PARAM_CONFIG.ALARM_LOGIC:
                            case EN_PARAM_CONFIG.ZPHASE_LOGIC:
							case EN_PARAM_CONFIG.LIMIT_NEGATIVE_LOGIC:
							case EN_PARAM_CONFIG.LIMIT_POSITIVE_LOGIC:
								bValue	= strValue.Equals(Define.DefineConstant.Motion.LOGIC_ACTIVE_HIGH);
								m_InstanceOfMotionDll.SetParameter(nIndexOfItem, m_DicForConfig[en], bValue);
								break;
                            case EN_PARAM_CONFIG.ALARM_STOPMODE:
							case EN_PARAM_CONFIG.LIMIT_NEGATIVE_STOPMODE:
							case EN_PARAM_CONFIG.LIMIT_POSITIVE_STOPMODE:
							case EN_PARAM_CONFIG.SWLIMIT_POSITIVE_STOPMODE:							
							case EN_PARAM_CONFIG.SWLIMIT_NEGATIVE_STOPMODE:
								bValue	= strValue.Equals(Define.DefineConstant.Motion.STOPMODE_EMG);
								m_InstanceOfMotionDll.SetParameter(nIndexOfItem, m_DicForConfig[en], bValue);
								break;
							case EN_PARAM_CONFIG.MOTION_TYPE:
							case EN_PARAM_CONFIG.MOTOR_TYPE:
                            case EN_PARAM_CONFIG.MOTOR_INPUT:
                            case EN_PARAM_CONFIG.MOTOR_OUTPUT:
								if(m_DicForResult.ContainsKey(strValue))
								{
									m_InstanceOfMotionDll.SetParameter(nIndexOfItem, m_DicForConfig[en], m_DicForResult[strValue]);
								}								
								break;
                            
						}						
					}
				}
				#endregion				

				#region PARAM_MOTION
				Array.Resize(ref arGroup, 1);
				foreach (EN_PARAM_MOTION en in Enum.GetValues(typeof(EN_PARAM_MOTION)))
				{
					if (m_InstanceOfStorage.GetParameter(Define.DefineEnumBase.Storage.EN_STORAGE_LIST.MOTION
						, en.ToString()
						, ref strValue
						, ref arGroup))
					{
						switch (en)
						{
							case EN_PARAM_MOTION.TAG_NO:
							case EN_PARAM_MOTION.NAME:
								m_InstanceOfMotionDll.SetParameter(nIndexOfItem, m_DicForMotionParam[en], strValue);
								break;
							case EN_PARAM_MOTION.ENABLE:
							case EN_PARAM_MOTION.POWER:
								m_InstanceOfMotionDll.SetParameter(nIndexOfItem, m_DicForMotionParam[en], strValue);
								break;
							case EN_PARAM_MOTION.TARGET:
								int nTarget			= -1;
								if(int.TryParse(strValue, out nTarget))
								{
									m_InstanceOfMotionDll.SetParameter(nIndexOfItem, m_DicForMotionParam[en], nTarget);
								}
								break;
						}
					}
				}
				#endregion
			}


			// 2025.03.18 by junho [ADD] parameter initialize 할 때 gathering thread 정지 시키도록 개선
			m_InstanceOfMotionDll.SetGatheringThreadPause(false);
		}
		/// <summary>
		/// 2020.07.01 by twkang [ADD] 주기적으로 리핏아이템의 동작완료 상태를 확인한다.
		/// </summary>
		private void CallbackFunctionForTimer(object sender, System.Timers.ElapsedEventArgs args)
		{
			RWLock.EnterReadLock();
			if(m_DicForRepeatItem.Count > 0)
			{
				foreach (var kvp in m_DicForRepeatItem)
				{
					int nState      = 0;

					m_InstanceOfMotionDll.GetMotorState(kvp.Key, ref nState);

					if (false == GetState(ref nState, MotionInstance::Motion_.MOTOR_STATE.MOTION_DONE))
					{
						continue;
					}

					switch (kvp.Value.GetState())
					{
						case RepeatItem.EN_REPEAT_STATE.DELAY_FIRST:
						case RepeatItem.EN_REPEAT_STATE.DELAY_SECOND:
							kvp.Value.IsDelayOver();
							break;
						case RepeatItem.EN_REPEAT_STATE.FIRST_MOTION_DELAY_FINISH:
							m_InstanceOfMotionDll.MoveAbsolutely(kvp.Key, kvp.Value.GetDestination(false), 0, MotionInstance::Motion_.MOTION_SPEED_CONTENT.RUN, 100, 0, 0);
							kvp.Value.StartNestStep();
							break;
						case RepeatItem.EN_REPEAT_STATE.SECOND_MOTION_DELAY_FINISH:
							m_InstanceOfMotionDll.MoveAbsolutely(kvp.Key, kvp.Value.GetDestination(true), 0, MotionInstance::Motion_.MOTION_SPEED_CONTENT.RUN, 100, 0, 0);
							kvp.Value.StartNestStep();
							break;
						case RepeatItem.EN_REPEAT_STATE.WATING_SET_DELAY_FIRST:
						case RepeatItem.EN_REPEAT_STATE.WATING_SET_DELAY_SECOND:
							kvp.Value.SetDelay();
							break;
					}
				}
			}
			RWLock.ExitReadLock();
		}
		/// <summary>
		/// 2020.06.23 by twkang [ADD] 파라미터를 파일로 저장한다.
		/// </summary>
		private bool SaveParamToStorage(int nIndexOfItem)
		{
			string strValue		= string.Empty;
			bool bValue			= false;
			int nValue			= -1;

			#region GROUP_CONFIG
			string[] arGroup	= new string[] { nIndexOfItem.ToString(), GROUP_CONFIG };
			foreach (EN_PARAM_CONFIG en in Enum.GetValues(typeof(EN_PARAM_CONFIG)))
			{
				switch (en)
				{
					case EN_PARAM_CONFIG.DIR_COMMAND:
					case EN_PARAM_CONFIG.DIR_ENCODER:
						m_InstanceOfMotionDll.GetParameter(nIndexOfItem, m_DicForConfig[en], ref bValue);
						strValue	= bValue ? Define.DefineConstant.Motion.INVERT_OFF : Define.DefineConstant.Motion.INVERT_ON;
						break;
					case EN_PARAM_CONFIG.LIMIT_NEGATIVE_LOGIC:
					case EN_PARAM_CONFIG.LIMIT_POSITIVE_LOGIC:
					case EN_PARAM_CONFIG.ALARM_LOGIC:
					case EN_PARAM_CONFIG.ZPHASE_LOGIC:
						m_InstanceOfMotionDll.GetParameter(nIndexOfItem, m_DicForConfig[en], ref bValue);
						strValue	= bValue ? Define.DefineConstant.Motion.LOGIC_ACTIVE_HIGH : Define.DefineConstant.Motion.LOGIC_ACTIVE_LOW;
						break;
					case EN_PARAM_CONFIG.LIMIT_NEGATIVE_STOPMODE:
					case EN_PARAM_CONFIG.LIMIT_POSITIVE_STOPMODE:
					case EN_PARAM_CONFIG.SWLIMIT_NEGATIVE_STOPMODE:
					case EN_PARAM_CONFIG.SWLIMIT_POSITIVE_STOPMODE:
					case EN_PARAM_CONFIG.ALARM_STOPMODE:
						m_InstanceOfMotionDll.GetParameter(nIndexOfItem, m_DicForConfig[en], ref bValue);
						strValue	= bValue ? Define.DefineConstant.Motion.STOPMODE_EMG : Define.DefineConstant.Motion.STOPMODE_DECEL;
						break;
					case EN_PARAM_CONFIG.MOTION_TYPE:
						m_InstanceOfMotionDll.GetParameter(nIndexOfItem, m_DicForConfig[en], ref nValue);
						strValue	= m_DicForMotionType.ContainsKey(nValue) ? m_DicForMotionType[nValue] : Define.DefineConstant.Common.NONE;
						break;
					case EN_PARAM_CONFIG.MOTOR_TYPE:
						m_InstanceOfMotionDll.GetParameter(nIndexOfItem, m_DicForConfig[en], ref nValue);
						strValue	= m_DicForMotorType.ContainsKey(nValue) ? m_DicForMotorType[nValue] : Define.DefineConstant.Common.NONE;
						break;
					case EN_PARAM_CONFIG.MOTOR_INPUT:
						m_InstanceOfMotionDll.GetParameter(nIndexOfItem, m_DicForConfig[en], ref nValue);
						strValue	= m_DicForMotorInmode.ContainsKey(nValue) ? m_DicForMotorInmode[nValue] : Define.DefineConstant.Common.NONE;
						break;
					case EN_PARAM_CONFIG.MOTOR_OUTPUT:
						m_InstanceOfMotionDll.GetParameter(nIndexOfItem, m_DicForConfig[en], ref nValue);
						strValue	= m_DicForMotorOutmode.ContainsKey(nValue) ? m_DicForMotorOutmode[nValue] : Define.DefineConstant.Common.NONE;
						break;
					case EN_PARAM_CONFIG.LIMIT_NEGATIVE_ENABLE:
					case EN_PARAM_CONFIG.LIMIT_POSITIVE_ENABLE:
					case EN_PARAM_CONFIG.SWLIMIT_NEGATIVE_ENABLE:
					case EN_PARAM_CONFIG.SWLIMIT_POSITIVE_ENABLE:
						m_InstanceOfMotionDll.GetParameter(nIndexOfItem, m_DicForConfig[en], ref bValue);
						strValue	= bValue.ToString();
						break;
					default:
						m_InstanceOfMotionDll.GetParameter(nIndexOfItem, m_DicForConfig[en], ref strValue);
						break;
				}
				m_InstanceOfStorage.SetParameter(Define.DefineEnumBase.Storage.EN_STORAGE_LIST.MOTION, en.ToString(), strValue, ref arGroup, false);
			}
			#endregion

			#region GROUP_HOME
			arGroup[1] = GROUP_HOME;
			foreach (EN_PARAM_HOME en in Enum.GetValues(typeof(EN_PARAM_HOME)))
			{
				switch (en)
				{
					case EN_PARAM_HOME.HOME_DIRECTION:
						m_InstanceOfMotionDll.GetParameter(nIndexOfItem, m_DicForHomeParam[en], ref bValue);
						strValue = bValue ? Define.DefineConstant.Motion.DIRECTION_POSITIVE : Define.DefineConstant.Motion.DIRECITON_NEGATIVE;
						break;
					case EN_PARAM_HOME.HOME_LOGIC:
						m_InstanceOfMotionDll.GetParameter(nIndexOfItem, m_DicForHomeParam[en], ref bValue);
						strValue = bValue ? Define.DefineConstant.Motion.LOGIC_ACTIVE_HIGH : Define.DefineConstant.Motion.LOGIC_ACTIVE_LOW;
						break;
					case EN_PARAM_HOME.HOME_MODE:
						m_InstanceOfMotionDll.GetParameter(nIndexOfItem, m_DicForHomeParam[en], ref nValue);
						strValue = m_DicForHomeMode.ContainsKey(nValue) ? m_DicForHomeMode[nValue] : Define.DefineConstant.Common.NONE;
						break;
					case EN_PARAM_HOME.HOME_SPEED_PATTERN:
						m_InstanceOfMotionDll.GetParameter(nIndexOfItem, m_DicForHomeParam[en], ref nValue);
						strValue = m_DicForSpeedPattern.ContainsKey(nValue) ? m_DicForSpeedPattern[nValue] : Define.DefineConstant.Common.NONE;
						break;
					default:
						m_InstanceOfMotionDll.GetParameter(nIndexOfItem, m_DicForHomeParam[en], ref strValue);
						break;
				}
				m_InstanceOfStorage.SetParameter(Define.DefineEnumBase.Storage.EN_STORAGE_LIST.MOTION, en.ToString(), strValue, ref arGroup, false);
			}
			#endregion

			#region GROUP_GANTRY
			arGroup[1] = GROUP_GANTRY;
			foreach (EN_PARAM_GANTRY en in Enum.GetValues(typeof(EN_PARAM_GANTRY)))
			{
				switch(en)
				{
					case EN_PARAM_GANTRY.ENABLE:
					case EN_PARAM_GANTRY.REVERSE_SLAVE_DIR:
						m_InstanceOfMotionDll.GetParameter(nIndexOfItem, m_DicForGantryParam[en], ref bValue);
						strValue	= bValue.ToString();
						break;
					default:
						m_InstanceOfMotionDll.GetParameter(nIndexOfItem, m_DicForGantryParam[en], ref strValue);
						break;
				}
				
				m_InstanceOfStorage.SetParameter(Define.DefineEnumBase.Storage.EN_STORAGE_LIST.MOTION, en.ToString(), strValue, ref arGroup, false);
			}
			#endregion

			#region STATE
			Array.Resize(ref arGroup, 1);
			foreach (EN_PARAM_MOTION en in Enum.GetValues(typeof(EN_PARAM_MOTION)))
			{
				switch(en)
				{
					case EN_PARAM_MOTION.ENABLE:
						m_InstanceOfMotionDll.GetParameter(nIndexOfItem, m_DicForMotionParam[en], ref bValue);
						strValue	= bValue.ToString();
						break;
					default:
						m_InstanceOfMotionDll.GetParameter(nIndexOfItem, m_DicForMotionParam[en], ref strValue);
						break;
				}
				m_InstanceOfStorage.SetParameter(Define.DefineEnumBase.Storage.EN_STORAGE_LIST.MOTION, en.ToString(), strValue, ref arGroup, false);
			}
			#endregion

			m_InstanceOfStorage.Save(Define.DefineEnumBase.Storage.EN_STORAGE_LIST.MOTION);

			return true;
		}
		/// <summary>
		/// 2020.10.05 by twkang [ADD] 해당 아이템의 Default 값을 설정한다.
		/// </summary>
		/// <param name="nIndex"></param>
		private void SetDefaultValue(int nIndex)
		{
			#region Common
			m_InstanceOfMotionDll.SetParameter(nIndex, PARAM_LIST.ENABLE, DEFAULT_ENABLE);
			m_InstanceOfMotionDll.SetParameter(nIndex, PARAM_LIST.NAME, DEFAULT_NAME + nIndex.ToString());
			m_InstanceOfMotionDll.SetParameter(nIndex, PARAM_LIST.POWER, DEFAULT_POWER);
			m_InstanceOfMotionDll.SetParameter(nIndex, PARAM_LIST.TARGET, nIndex);
			m_InstanceOfMotionDll.SetParameter(nIndex, PARAM_LIST.TAG_NO, DEFAULT_TAGNUMBER);
			#endregion

			#region Config
			m_InstanceOfMotionDll.SetParameter(nIndex, PARAM_LIST_CONFIG.DIR_COMMAND, DEFAULT_DIR);
			m_InstanceOfMotionDll.SetParameter(nIndex, PARAM_LIST_CONFIG.DIR_ENCODER, DEFAULT_DIR);
			m_InstanceOfMotionDll.SetParameter(nIndex, PARAM_LIST_CONFIG.LIMIT_NEGATIVE_ENABLE, DEFAULT_CONFIG_ENABLE);
			m_InstanceOfMotionDll.SetParameter(nIndex, PARAM_LIST_CONFIG.LIMIT_NEGATIVE_LOGIC, DEFAULT_LOGIC);
			m_InstanceOfMotionDll.SetParameter(nIndex, PARAM_LIST_CONFIG.LIMIT_NEGATIVE_STOPMODE, DEFAULT_STOPMODE);
			m_InstanceOfMotionDll.SetParameter(nIndex, PARAM_LIST_CONFIG.LIMIT_POSITIVE_ENABLE, DEFAULT_CONFIG_ENABLE);
			m_InstanceOfMotionDll.SetParameter(nIndex, PARAM_LIST_CONFIG.LIMIT_POSITIVE_LOGIC, DEFAULT_LOGIC);
			m_InstanceOfMotionDll.SetParameter(nIndex, PARAM_LIST_CONFIG.LIMIT_POSITIVE_STOPMODE, DEFAULT_STOPMODE);
			m_InstanceOfMotionDll.SetParameter(nIndex, PARAM_LIST_CONFIG.MOTOR_TYPE, DEFAULT_MOTION_TYPE);
			m_InstanceOfMotionDll.SetParameter(nIndex, PARAM_LIST_CONFIG.MOTION_TYPE, DEFAULT_MOTION_TYPE);
            m_InstanceOfMotionDll.SetParameter(nIndex, PARAM_LIST_CONFIG.INPUT_MODE, DEFAULT_MOTOR_INMODE);
            m_InstanceOfMotionDll.SetParameter(nIndex, PARAM_LIST_CONFIG.OUTPUT_MODE, DEFAULT_MOTOR_OUTMODE);
			m_InstanceOfMotionDll.SetParameter(nIndex, PARAM_LIST_CONFIG.ALARM_LOGIC, DEFAULT_LOGIC);
			m_InstanceOfMotionDll.SetParameter(nIndex, PARAM_LIST_CONFIG.ALARM_STOPMODE, DEFAULT_STOPMODE);
			m_InstanceOfMotionDll.SetParameter(nIndex, PARAM_LIST_CONFIG.ZPHASE_LOGIC, DEFAULT_LOGIC);
			m_InstanceOfMotionDll.SetParameter(nIndex, PARAM_LIST_CONFIG.INPUT_SCALE_CPR, DEFAULT_SCALE_CPR);
			m_InstanceOfMotionDll.SetParameter(nIndex, PARAM_LIST_CONFIG.INPUT_SCALE_MPR, DEFAULT_SCALE_MPR);
			m_InstanceOfMotionDll.SetParameter(nIndex, PARAM_LIST_CONFIG.OUTPUT_SCALE_CPR, DEFAULT_SCALE_CPR);
			m_InstanceOfMotionDll.SetParameter(nIndex, PARAM_LIST_CONFIG.OUTPUT_SCALE_MPR, DEFAULT_SCALE_MPR);
			m_InstanceOfMotionDll.SetParameter(nIndex, PARAM_LIST_CONFIG.SWLIMIT_NEGATIVE_ENABLE, DEFAULT_CONFIG_ENABLE);
			m_InstanceOfMotionDll.SetParameter(nIndex, PARAM_LIST_CONFIG.SWLIMIT_NEGATIVE_POSITION, DEFAULT_NEGATIVE_POSITION);
			m_InstanceOfMotionDll.SetParameter(nIndex, PARAM_LIST_CONFIG.SWLIMIT_NEGATIVE_STOPMODE, DEFAULT_STOPMODE);
			m_InstanceOfMotionDll.SetParameter(nIndex, PARAM_LIST_CONFIG.SWLIMIT_POSITIVE_ENABLE, DEFAULT_CONFIG_ENABLE);
			m_InstanceOfMotionDll.SetParameter(nIndex, PARAM_LIST_CONFIG.SWLIMIT_POSITIVE_POSITION, DEFAULT_POSITIVE_POSITION);
			m_InstanceOfMotionDll.SetParameter(nIndex, PARAM_LIST_CONFIG.SWLIMIT_POSITIVE_STOPMODE, DEFAULT_STOPMODE);
			#endregion
			
			#region Gantry
			m_InstanceOfMotionDll.SetParameter(nIndex, PARAM_LIST_GANTRY.ENABLE, DEFAULT_GANTRY_ENALBE);
			m_InstanceOfMotionDll.SetParameter(nIndex, PARAM_LIST_GANTRY.INDEX_MASTER, DEFAULT_INDEX_MASTER);
			m_InstanceOfMotionDll.SetParameter(nIndex, PARAM_LIST_GANTRY.INDEX_SLAVE, DEFAULT_INDEX_SLAVE);
			m_InstanceOfMotionDll.SetParameter(nIndex, PARAM_LIST_GANTRY.REVERSE_SLAVE_DIR, DEFAULT_REVERSE_SLAVE_DIR);
			#endregion

			#region Home
			m_InstanceOfMotionDll.SetParameter(nIndex, PARAM_LIST_HOME.ENABLE_HOME_END, DEFAULT_ENABLE_HOME_END);
			m_InstanceOfMotionDll.SetParameter(nIndex, PARAM_LIST_HOME.HOME_BASE_POSITION, DEFAULT_HOME_BASE_POSITION);
			m_InstanceOfMotionDll.SetParameter(nIndex, PARAM_LIST_HOME.HOME_ABSOLUTE_POSITION, DEFAULT_HOMH_ABSOLUTE_POSITION);
			m_InstanceOfMotionDll.SetParameter(nIndex, PARAM_LIST_HOME.HOME_COUNT_Z, DEFAULT_HOME_COUNT_Z);
			m_InstanceOfMotionDll.SetParameter(nIndex, PARAM_LIST_HOME.HOME_DIRECTION, DEFAULT_HOME_DIR);
			m_InstanceOfMotionDll.SetParameter(nIndex, PARAM_LIST_HOME.HOME_ESCAPE_DISTANCE, DEFAULT_HOME_ESCAPE_DISTANCE);
			m_InstanceOfMotionDll.SetParameter(nIndex, PARAM_LIST_HOME.HOME_JERK_ACCEL, DEFAULT_HOME_JERK_ACCEL);
			m_InstanceOfMotionDll.SetParameter(nIndex, PARAM_LIST_HOME.HOME_JERK_DECEL, DEFAULT_HOME_JERK_DECEL);
			m_InstanceOfMotionDll.SetParameter(nIndex, PARAM_LIST_HOME.HOME_LOGIC, DEFAULT_LOGIC);
			m_InstanceOfMotionDll.SetParameter(nIndex, PARAM_LIST_HOME.HOME_MODE, DEFAULT_HOME_MODE);
			m_InstanceOfMotionDll.SetParameter(nIndex, PARAM_LIST_HOME.HOME_OFFSET, DEFAULT_HOME_OFFSET);
			m_InstanceOfMotionDll.SetParameter(nIndex, PARAM_LIST_HOME.HOME_SPEED_PATTERN, DEFAULT_HOME_SPEED_PATTERN);
			m_InstanceOfMotionDll.SetParameter(nIndex, PARAM_LIST_HOME.HOME_ACCEL, DEFAULT_HOME_ACCEL_VELOCITY);
			m_InstanceOfMotionDll.SetParameter(nIndex, PARAM_LIST_HOME.HOME_DECEL, DEFAULT_HOME_DECEL_VELOCITY);
			m_InstanceOfMotionDll.SetParameter(nIndex, PARAM_LIST_HOME.HOME_VELOCITY_END, DEFAULT_HOME_VELOCITY_END);
			m_InstanceOfMotionDll.SetParameter(nIndex, PARAM_LIST_HOME.HOME_VELOCITY_START, DEFAULT_HOME_VELOCITY_START);
			m_InstanceOfMotionDll.SetParameter(nIndex, PARAM_LIST_HOME.TIMEOUT, DEFAULT_HOME_TIME_OUT);
			#endregion
		}

		/// <summary>
		/// 2020.06.30 by twkang [ADD] 모션클래스의 리핏동작을 위한 클래스이다.
		/// </summary>
		public class RepeatItem
		{
			private double m_dblPosFirst				= 0;
			private double m_dblPosSecond				= 0;
			private uint m_nDelayFirst					= 0;
			private uint m_nDelaySecond					= 0;

			private EN_REPEAT_STATE m_enumState			= EN_REPEAT_STATE.WATING_SET_DELAY_FIRST;
			TickCounter_.TickCounter m_TickCounter		= new TickCounter_.TickCounter();

			public enum EN_REPEAT_STATE
			{
				WATING_SET_DELAY_FIRST,			// First 딜레이 이후 Second Position 이동 준비
				WATING_SET_DELAY_SECOND,		// Second 딜레이 이후 First Position 이동 준비
				DELAY_FIRST,					// 딜레이중
				DELAY_SECOND,					// 딜레이중
				FIRST_MOTION_DELAY_FINISH,		// 
				SECOND_MOTION_DELAY_FINISH,		// 
			}

			public RepeatItem(double dblPosFirst, int nDelayFirst, double dblPosSecond, int nDelaySecond)
			{
				m_dblPosFirst		= dblPosFirst;
				m_dblPosSecond		= dblPosSecond;
				m_nDelayFirst		= (uint)nDelayFirst;
				m_nDelaySecond		= (uint)nDelaySecond;
			}

			/// <summary>
			/// 2020.07.01 by twkang [ADD] 딜레이를 설정한다.
			/// </summary>
			public void SetDelay()
			{
				switch(m_enumState)
				{
					case EN_REPEAT_STATE.WATING_SET_DELAY_FIRST:
						m_TickCounter.SetTickCount(m_nDelayFirst);
						m_enumState		= EN_REPEAT_STATE.DELAY_FIRST;
						break;	
					case EN_REPEAT_STATE.WATING_SET_DELAY_SECOND:
						m_TickCounter.SetTickCount(m_nDelaySecond);
						m_enumState		= EN_REPEAT_STATE.DELAY_SECOND;
						break;
				}
			}
			/// <summary>
			/// 2020.07.01 by twkang [ADD] 현재 리핏동작의 상태를 반환한다.
			/// </summary>
			public EN_REPEAT_STATE GetState()
			{
				return m_enumState;
			}
			/// <summary>
			/// 2020.07.01 by twkang [ADD] 목표 위치를 반환한다.
			/// </summary>
			public double GetDestination(bool bFirst)
			{
				return bFirst ? m_dblPosFirst : m_dblPosSecond;
			}
			/// <summary>
			/// 2020.07.01 by twkang [ADD] 설정 시간 경과 여부를 확인한다.
			/// </summary>
			public bool IsDelayOver()
			{
				if(m_TickCounter.IsTickOver(true))
				{
					switch(m_enumState)
					{
						case EN_REPEAT_STATE.DELAY_FIRST:
							m_enumState		= EN_REPEAT_STATE.FIRST_MOTION_DELAY_FINISH;
							break;	
						case EN_REPEAT_STATE.DELAY_SECOND:
							m_enumState		= EN_REPEAT_STATE.SECOND_MOTION_DELAY_FINISH;
							break;
					}
					return true;
				}
				return false;
			}
			/// <summary>
			/// 2020.07.01 by twkang [ADD] 다음동작을 위한 준비 상태로 바꾼다.
			/// </summary>
			public void StartNestStep()
			{
				switch(m_enumState)
				{
					case EN_REPEAT_STATE.FIRST_MOTION_DELAY_FINISH:
						m_enumState		= EN_REPEAT_STATE.WATING_SET_DELAY_SECOND;
						break;
					case EN_REPEAT_STATE.SECOND_MOTION_DELAY_FINISH:
						m_enumState		= EN_REPEAT_STATE.WATING_SET_DELAY_FIRST;
						break;
				}
			}
			/// <summary>
			/// 
			/// </summary>
			public int GetFirstDelay()
			{
				return (int)m_nDelayFirst;
			}
			/// <summary>
			/// 
			/// </summary>
			public int GetSecondDelay()
			{
				return (int)m_nDelaySecond;
			}
			public double GetFirstPosition()
			{
				return m_dblPosFirst;
			}
			public double GetSecondPosition()
			{
				return m_dblPosSecond;
			}
		}
		#endregion

        #region Interpolation

        #region Variables
        private readonly string ROOT_INTERPOLATION      = "INTERPOLATION";
        private readonly string ITEM_COUNT_X            = "COUNT_X";
        private readonly string ITEM_COUNT_Y            = "COUNT_Y";
        private readonly string ITEM_POSITION_X         = "POS_X";
        private readonly string ITEM_POSITION_Y         = "POS_Y";
        private readonly string ITEM_RESULT_X           = "RES_X";
        private readonly string ITEM_RESULT_Y           = "RES_Y";

        private const int MAXSIZE_DATA                      = 100;

        private string[] m_arMappingOfInteger               = null;
        private Dictionary<string, int> m_mappingOfString   = new Dictionary<string,int>(); // Key String(int), value int
        #endregion End - Variables

        #region Internal
        /// <summary>
        /// 2021.02.04 by yjlee [ADD] Make the mapping tables for the interpolation.
        /// </summary>
        private void MakeMappingForInterpolation()
        {
            m_arMappingOfInteger        = new string[MAXSIZE_DATA];

            for(int nIndex = 0 ; nIndex < MAXSIZE_DATA ; ++ nIndex)
            {
                string strData      = nIndex.ToString();

                m_arMappingOfInteger[nIndex]        = strData;
                m_mappingOfString.Add(strData, nIndex);
            }
        }
        /// <summary>
        /// 2021.02.04 by yjlee [ADD] Set the interpolation data to the motion.
        /// </summary>
        private void SetInterpolationData(ref int nIndexOfItemX, ref int nIndexOfItemY, ref MOTION_INTERPOLATION enInterpolation, ref int nX, ref int nY, ref MOTION_POSITION[,] arData)
        {
            m_InstanceOfMotionDll.SetInterpolation(nIndexOfItemX, nIndexOfItemY, enInterpolation, nX, nY, ref arData);
        }
        /// <summary>
        /// 2021.02.04 by yjlee [ADD] Get the interpolation data.
        /// </summary>
        private string GetInterpolationFileName(ref int nIndexOfItemX, ref int nIndexOfItemY)
        {
            return string.Format("X{0}_Y{1}{2}", nIndexOfItemX, nIndexOfItemY, Define.DefineConstant.FileFormat.FILEFORMAT_INTERPOLATION);
        }
        #endregion End - Internal

        #region External
        /// <summary>
        /// 2021.02.04 by yjlee [ADD] Load the interpolation data.
        /// </summary>
        public bool LoadInterpolationData(int nIndexOfItemX, int nIndexOfItemY, MOTION_INTERPOLATION enInterpolation)
        {
            string strData          = string.Empty;

            if(FileIOManager.GetInstance().Read(Define.DefineConstant.FilePath.FILEPATH_INTERPOLATION
                , GetInterpolationFileName(ref nIndexOfItemX, ref nIndexOfItemY)
                , ref strData
                , Define.DefineConstant.FileIO.TIMEOUT_READ))
            {
                string[] arData     = strData.Split('\n');

                FileComposite instanceComposite     = FileComposite.GetInstance();

                if(instanceComposite.CreateRootByString(ref arData))
                {
                    int nX          = 0;
                    int nY          = 0;

                    if(instanceComposite.GetValue(ROOT_INTERPOLATION, ITEM_COUNT_X, ref nX)
                        && instanceComposite.GetValue(ROOT_INTERPOLATION, ITEM_COUNT_Y, ref nY))
                    {
                        MOTION_POSITION[,] arInterpolationData      = new MOTION_POSITION[nY, nX];

                        string[] arGroup        = new string[]{string.Empty, string.Empty};

                        for(int nCol = 0 ; nCol < nY ; ++ nCol)
                        {
                            arGroup[0]          = m_arMappingOfInteger[nCol];

                            for(int nRow = 0 ; nRow < nX ; ++ nRow)
                            {
                                arGroup[1]      = m_arMappingOfInteger[nRow];

                                var data        = new MOTION_POSITION();

                                if(instanceComposite.GetValue(ROOT_INTERPOLATION, ITEM_POSITION_X, ref data.dblPositionX, arGroup)
                                    && instanceComposite.GetValue(ROOT_INTERPOLATION, ITEM_POSITION_Y, ref data.dblPositionY, arGroup)
                                    && instanceComposite.GetValue(ROOT_INTERPOLATION, ITEM_RESULT_X, ref data.dblResultX, arGroup)
                                    && instanceComposite.GetValue(ROOT_INTERPOLATION, ITEM_RESULT_Y, ref data.dblResultY, arGroup))
                                {
                                    arInterpolationData[nCol, nRow]     = data;    
                                }
                                else
                                {
                                    instanceComposite.RemoveRoot(ROOT_INTERPOLATION);

                                    return false;
                                }
                            }
                        }

                        instanceComposite.RemoveRoot(ROOT_INTERPOLATION);

                        return m_InstanceOfMotionDll.SetInterpolation(nIndexOfItemX, nIndexOfItemY, enInterpolation, nX, nY, ref arInterpolationData);
                    }
                }
            }

            return false;
        }

		public bool GetInterpolationResult(int nIndexOfItemX, int nIndexOfItemY, ref MOTION_POSITION pResult)
		{
			bool bResult = m_InstanceOfMotionDll.GetInterpolationResult(nIndexOfItemX, nIndexOfItemY, ref pResult);

			pResult.dblResultX	+= pResult.dblPositionX;
			pResult.dblResultY	+= pResult.dblPositionY;

			return bResult;
		}

        /// <summary>
        /// 2021.02.04 by yjlee [ADD] Make the interpolation file.
        /// </summary>
        public void MakeInterpolationData(int nIndexOfItemX, int nIndexOfItemY, int nX, int nY, ref MOTION_POSITION[,] arData)
        {
            FileComposite instanceComposite     = FileComposite.GetInstance();

            instanceComposite.CreateRoot(ROOT_INTERPOLATION);

            instanceComposite.AddItem(ROOT_INTERPOLATION, ITEM_COUNT_X, nX);
            instanceComposite.AddItem(ROOT_INTERPOLATION, ITEM_COUNT_Y, nY);

            string[] arGroup        = new string[]{string.Empty, string.Empty};

            for(int nCol = 0 ; nCol < nY ; ++ nCol)
            {
                arGroup[0]          = m_arMappingOfInteger[nCol];

                for(int nRow = 0 ; nRow < nX ; ++ nRow)
                {
                    arGroup[1]      = m_arMappingOfInteger[nRow];

                    instanceComposite.AddGroup(ROOT_INTERPOLATION, arGroup);

                    instanceComposite.AddItem(ROOT_INTERPOLATION, ITEM_POSITION_X, arData[nCol, nRow].dblPositionX, arGroup);
                    instanceComposite.AddItem(ROOT_INTERPOLATION, ITEM_POSITION_Y, arData[nCol, nRow].dblPositionY, arGroup);
                    instanceComposite.AddItem(ROOT_INTERPOLATION, ITEM_RESULT_X, arData[nCol, nRow].dblResultX, arGroup);
                    instanceComposite.AddItem(ROOT_INTERPOLATION, ITEM_RESULT_Y, arData[nCol, nRow].dblResultY, arGroup);
                }
            }

            string strData      = string.Empty;

            instanceComposite.GetStructureAsString(ROOT_INTERPOLATION, ref strData);

            FileIOManager.GetInstance().Write(Define.DefineConstant.FilePath.FILEPATH_INTERPOLATION
                , GetInterpolationFileName(ref nIndexOfItemX, ref nIndexOfItemY)
                , strData
                , false
                , false);

            instanceComposite.RemoveRoot(ROOT_INTERPOLATION);
        }
        #endregion End - External

        #endregion End - Interpolation
    }
}
