using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;

using Alarm_;
using FileComposite_;
using FileIOManager_;
using FrameOfSystem3.Task;
using Define.DefineEnumBase.Common;
using Define.DefineConstant;

namespace FrameOfSystem3.Tool
{
    #region ENUMERATION
    public enum EN_USAGE_TYPE
    {
        NONE,               // 제한 없음
        COUNT,              // 사용 횟수 
        TIME,               // 사용 시간 (기존 방식) Timer 사용
    }
    public enum EN_WORK_END
    {
        UNIT_PROCESS,   // 단위 공정 작업을 마치고 정지
        ALL_PROCESS,    // 모든 공정 작업을 마치고 자재 배출 후 정지
        LOT_END,        // LOT END 자재의 작업을 마치고 배출 후 정지 (장비 내 자재 없음)
        ARBITRARY       // 임의의 ACTION 종료 후 정지
    }
    public enum EN_TOOL_STATE
    {
        NORMAL,         // 정상
        ALMOST,         // 한계 접근
        EXCESS,         // 한계 초과
        PERMISSION,     // 한계 초과 허용
    }
    public enum EN_TOOL_PARAM
    {
        NAME,
		//ID, // 2022.02.17 by junho [DEL] Change of Tool ID save file (config -> recovery)
        ENABLE,
        STARTUP,
        STANDSTILL,
        EVENT_EXCHANGE,
        USAGE_TYPE,
        WORK_END,
        NOTICE_LIMIT_COUNT,
        NOTICE_LIMIT_TIME,
        NOTICE_ALARM_CODE,
        NOTICE_INTERVAL_COUNT,
        NOTICE_INTERVAL_TIME,
		//INITIAL_TIME,		// 2021.11.10 by junho [DEL] Initialize time 저장을 Recovery에 하도록 개선 (Reset 해야함)
        WARNING_LIMIT_COUNT,
        WARNING_LIMIT_TIME,
        WARNING_ALARM_CODE,
        WARNING_INTERVAL_COUNT,
        WARNING_INTERVAL_TIME
    }
    public enum EN_TOOL_MONITORING
    {
		TOOL_ID,					// 2022.02.17 by junho [ADD] save Tool ID in recovery data
        TOOL_STATE,
        USED_COUNT,
        NOTICE_DIVIDED_VALUE,
        WARNING_DIVIDED_VALUE,
		INITIAL_TIME,				// 2021.11.10 by junho [ADD] Initialize time 저장을 Recovery에 하도록 개선 (Reset 해야함)
        USED_TIME,                      //23.07.10 by wdw [MOD] 변경 SPENT TIME -> USED TIME
        LAST_USED_TIME,                      //23.07.10 by wdw [MOD] 변경 SPENT TIME -> USED TIME
        TIMER_ACTIVATE,            // 23.07.10 by wdw [ADD] Timer 갱신 여부.
        LAST_ACTIVATE_TIME,               // 23.07.10 by wdw [ADD] 마지막 Activate 시작 시간.
    }
    #endregion

    // OBJECT ROLE : TOOL 의 교환시기를 시스템에 알리기 위해 알람을 발생시킨다.
    public class WorkTool
    {
        #region SINGLETON
        private WorkTool() { }
        private static WorkTool m_instance = new WorkTool();
        public static WorkTool GetInstance() { return m_instance; }
        #endregion

        #region CONSTANT
        private const int c_nInterval = 200;        // 200 msec
		public static string Def_ToolId = "NONE";
        #endregion

        #region FIELD
        // key : Index (등록 순번)
        private static Dictionary<int, WorkToolItem> m_dicTools = new Dictionary<int, WorkToolItem>();

        private static Timer m_tmrLifetime = new Timer(c_nInterval);

        public delegate void EventAlarmDelegate(EN_ALARM_GRADE enGrade, int nCode);
        #endregion

        #region INTERFACE

        #region INITIALIZE
        public void Init()
        {
            m_tmrLifetime.Elapsed += OnTimedEvent;
            m_tmrLifetime.AutoReset = true;
            m_tmrLifetime.Enabled = true;
        }
        #endregion

        #region CREATE
        public bool AddTool(int nIndex)
        {
            if (m_dicTools.ContainsKey(nIndex).Equals(false))
            {
                m_dicTools[nIndex] = new WorkToolItem(nIndex);

                return true;
            }

            return false;
        }
        #endregion

        #region SERVICE
        /// <summary>
        /// 주기적으로 교환시기를 확인 (Timer Callback Function, Interval 200 ms)
        /// </summary>
        public static void OnTimedEvent(Object source, ElapsedEventArgs e)
        {
            foreach (KeyValuePair<int, WorkToolItem> kvp in m_dicTools)
            {
                WorkToolItem item = kvp.Value;

                EN_ALARM_GRADE enAlarmGrade = item.CheckOverLimit(EN_USAGE_TYPE.TIME);
            }
        }
        /// <summary>
        /// TOOL 사용 후 수명 한계 값을 넘었는지 확인하여 알람 발생 여부를 결정
		/// junho [NOTE] TOOL 사용 함을 공지
        /// </summary>
        public EN_ALARM_GRADE UseWorkTool(int nIndex)
        {
            if(m_dicTools.ContainsKey(nIndex))
            {
                WorkToolItem item = m_dicTools[nIndex];

                item.IncreaseUsedCount();

                return item.CheckOverLimit(EN_USAGE_TYPE.COUNT);
            }

            return EN_ALARM_GRADE.NORMAL;
        }

        /// <summary>
        /// 23.07.04 by wdw [ADD] MANUAL 시간 설정. External Device 사용시간 설정 시 등 사용
        /// TOOL 사용 후 수명 한계 값을 넘었는지 확인하여 알람 발생 여부를 결정
        /// wdw [NOTE] External Device 사용 시간을 설정
        /// </summary>
        public EN_ALARM_GRADE SetWorkToolUsedTime(int nIndex, TimeSpan UseTime)
        {
            if (m_dicTools.ContainsKey(nIndex))
            {
                WorkToolItem item = m_dicTools[nIndex];

                item.SetUsedTime(UseTime);

                return item.CheckOverLimit(EN_USAGE_TYPE.TIME);
            }

            return EN_ALARM_GRADE.NORMAL;
        }

        /// <summary>
        /// 23.07.04 by wdw [ADD] MANUAL 시간 증가. External Device 사용시간 설정 시 등 사용
        /// TOOL 사용 후 수명 한계 값을 넘었는지 확인하여 알람 발생 여부를 결정
        /// </summary>
        public EN_ALARM_GRADE IncreaseWorkToolUsedTime(int nIndex, TimeSpan UseTime)
        {
            if (m_dicTools.ContainsKey(nIndex))
            {
                WorkToolItem item = m_dicTools[nIndex];

                item.IncreaseUsedTime(UseTime);

                return item.CheckOverLimit(EN_USAGE_TYPE.TIME);
            }

            return EN_ALARM_GRADE.NORMAL;
        }

        /// <summary>
        /// 23.07.10 by wdw [ADD] Timer Activate 설정
       /// </summary>
        public void SetActivate(int nIndex, bool bActivate, bool bUpdateUsedTime = true)
        {
            if (m_dicTools.ContainsKey(nIndex))
            {
                WorkToolItem item = m_dicTools[nIndex];

                item.SetActivate(bActivate, bUpdateUsedTime);
            }
        }

        /// <summary>
        /// TOOL 교환 후 새로운 ID 를 설정하고 누적 COUNT 또는 TIME 을 0 으로 변경
        /// </summary>
        public void ResetWorkTool(int nIndex, string sID, bool bActivate = true)
        {
            if (m_dicTools.ContainsKey(nIndex))
            {
                WorkToolItem item = m_dicTools[nIndex];

                item.SetZero(bActivate);

                // 2024.08.21 by junho [ADD] 다를 떄에만 변경하도록 변경 (File write 최소화)
                if(item.ID != sID) 
                    item.ID = sID;
			}
        }
        /// <summary>
        /// WARNING 을 발생시켜 공정을 중단하기 위해 작업의 종료를 알린다
        /// </summary>
        public bool NotifyWorkEnd(EN_WORK_END enEnd)
        {
            bool bIsAlarm = false;

            foreach (KeyValuePair<int, WorkToolItem> kvp in m_dicTools)
            {
                WorkToolItem item = kvp.Value;

                if (item.CheckConsumedTool(enEnd))
                    bIsAlarm = true;
            }

            return bIsAlarm;
        }
		/// <summary>
		/// 2021.11.06 by junho [ADD] 특정 Item만 확인 하는 Interface 추가
		/// WARNING 을 발생시켜 공정을 중단하기 위해 작업의 종료를 알린다
		/// </summary>
		public bool NotifyWorkEnd(int nIndex, EN_WORK_END enEnd)
		{
			if (m_dicTools.ContainsKey(nIndex).Equals(false))
				return false;	// 없으면 false 반환?

			return m_dicTools[nIndex].CheckConsumedTool(enEnd);
		}
		/// <summary>
		/// 2021.11.06 by junho [ADD] 특정 Item만 확인 하는 Interface 추가
		/// WARNING 을 발생시켜 공정을 중단하기 위해 작업의 종료를 알린다
		/// </summary>
		public bool NotifyWorkEnd(List<int> listIndex, EN_WORK_END enEnd)
		{
			bool bIsAlarm = false;

			foreach(int index in listIndex)
			{
				if (NotifyWorkEnd(index, enEnd))
					bIsAlarm = true;
			}

			return bIsAlarm;
		}
        /// <summary>
        /// AUTO RUN 시 TOOL 이 수명 한계 값을 넘었으면 운전을 중단하기 위해 작업의 시작을 알린다
        /// </summary>
        public bool NotifyWorkBegin()
        {
            bool bIsAlarm = false;

            foreach (KeyValuePair<int, WorkToolItem> kvp in m_dicTools)
            {
                WorkToolItem item = kvp.Value;

                if (item.CheckConsumedTool())
                    bIsAlarm = true;
            }

            return bIsAlarm;
		}
		/// <summary>
		/// 2021.11.06 by junho [ADD] 특정 Item만 확인 하는 Interface 추가
		/// AUTO RUN 시 TOOL 이 수명 한계 값을 넘었으면 운전을 중단하기 위해 작업의 시작을 알린다
		/// </summary>
		public bool NotifyWorkBegin(int nIndex)
		{
			if (m_dicTools.ContainsKey(nIndex).Equals(false))
				return false;	// 없으면 false 반환?

			return m_dicTools[nIndex].CheckConsumedTool();
		}
		/// <summary>
		/// 2021.11.06 by junho [ADD] 특정 Item만 확인 하는 Interface 추가
		/// AUTO RUN 시 TOOL 이 수명 한계 값을 넘었으면 운전을 중단하기 위해 작업의 시작을 알린다
		/// </summary>
		public bool NotifyWorkBegin(List<int> listIndex)
		{
			bool bIsAlarm = false;

			foreach (int index in listIndex)
			{
				if (NotifyWorkBegin(index))
					bIsAlarm = true;
			}

			return bIsAlarm;
		}
        /// <summary>
        /// 2025.03.27 by junho [NOTE] 아이디가 설정 되어있는지 확인한다.
        /// 현재 알람발생은 하지 않음.
        /// </summary>
        public bool NotifyCheckIdBegin(int nIndex)
        {
            if (m_dicTools.ContainsKey(nIndex).Equals(false))
                return false;	

            return m_dicTools[nIndex].CheckIdTool();
		}
		/// <summary>
		/// 2025.03.27 by junho [NOTE] 아이디가 설정 되어있는지 확인한다.
		/// 현재 알람발생은 하지 않음.
		/// </summary>
		public bool NotifyCheckIdBegin(List<int> listIndex)
        {
            bool bIsAlarm = false;

            foreach (int index in listIndex)
            {
                if (NotifyCheckIdBegin(index))
                    bIsAlarm = true;
            }

            return bIsAlarm;
        }
        #endregion

        #endregion

		#region SETTER/GETTER
		public bool GetParameter(int nIndex, EN_TOOL_PARAM enParam, out string result)
        {
			result = string.Empty; 
		
            if (m_dicTools.ContainsKey(nIndex))
            {
                WorkToolItem item = m_dicTools[nIndex];

                switch (enParam)
                {
                    case EN_TOOL_PARAM.NAME:
                        result = item.Name;
                        break;

					// 2022.02.17 by junho [DEL] Change of Tool ID save file (config -> recovery)
					//case EN_TOOL_PARAM.ID:
					//	oItemValue = item.ID;
					//	break;

                    case EN_TOOL_PARAM.ENABLE:
                        result = item.Enable.ToString();
                        break;
                    case EN_TOOL_PARAM.STARTUP:
						result = item.Startup.ToString();
                        break;
                    case EN_TOOL_PARAM.STANDSTILL:
						result = item.Standstill.ToString();
                        break;
                    case EN_TOOL_PARAM.EVENT_EXCHANGE:
						result = item.EventExchange.ToString();
                        break;
                    case EN_TOOL_PARAM.USAGE_TYPE:
						result = item.UsageType.ToString();
                        break;
                    case EN_TOOL_PARAM.WORK_END:
						result = item.WorkEnd.ToString();
                        break;

                    case EN_TOOL_PARAM.NOTICE_LIMIT_COUNT:
						result = item.NoticeLimitCount.ToString();
                        break;
                    case EN_TOOL_PARAM.NOTICE_LIMIT_TIME:
						result = item.NoticeLimitTime.ToString();
                        break;
                    case EN_TOOL_PARAM.NOTICE_ALARM_CODE:
						result = item.NoticeAlarmCode.ToString();
                        break;
                    case EN_TOOL_PARAM.NOTICE_INTERVAL_COUNT:
						result = item.NoticeIntervalCount.ToString();
                        break;
                    case EN_TOOL_PARAM.NOTICE_INTERVAL_TIME:
						result = item.NoticeIntervalTime.ToString();
                        break;

					// 2021.11.10 by junho [DEL] Initialize time 저장을 Recovery에 하도록 개선 (Reset 해야함)
					//case EN_TOOL_PARAM.INITIAL_TIME:
					//	oItemValue = item.InitialTime;
					//	break;

                    case EN_TOOL_PARAM.WARNING_LIMIT_COUNT:
						result = item.WarningLimitCount.ToString();
                        break;
                    case EN_TOOL_PARAM.WARNING_LIMIT_TIME:
						result = item.WarningLimitTime.ToString();
                        break;
                    case EN_TOOL_PARAM.WARNING_ALARM_CODE:
						result = item.WarningAlarmCode.ToString();
                        break;
                    case EN_TOOL_PARAM.WARNING_INTERVAL_COUNT:
						result = item.WarningIntervalCount.ToString();
                        break;
                    case EN_TOOL_PARAM.WARNING_INTERVAL_TIME:
						result = item.WarningIntervalTime.ToString();
                        break;
					default: result = string.Empty; return false;
                }

                return true;
            }

			result = string.Empty; 

            return false;
        }
        public bool GetParameter(int nIndex, EN_TOOL_PARAM enParam, ref TimeSpan tItemValue)
        {
            if (m_dicTools.ContainsKey(nIndex))
            {
                WorkToolItem item = m_dicTools[nIndex];

                switch (enParam)
                {
                    case EN_TOOL_PARAM.NOTICE_LIMIT_TIME:
                        tItemValue = item.NoticeLimitTime;
                        break;
                    case EN_TOOL_PARAM.NOTICE_INTERVAL_TIME:
                        tItemValue = item.NoticeIntervalTime;
                        break;
                    case EN_TOOL_PARAM.WARNING_LIMIT_TIME:
                        tItemValue = item.WarningLimitTime;
                        break;
                    case EN_TOOL_PARAM.WARNING_INTERVAL_TIME:
                        tItemValue = item.WarningIntervalTime;
                        break;
                }

                return true;
            }

            return false;
        }
        public bool SetParameter(int nIndex, EN_TOOL_PARAM enParam, string sItemValue)
        {
            if (m_dicTools.ContainsKey(nIndex))
            {
                WorkToolItem item = m_dicTools[nIndex];

                switch(enParam)
                {
                    case EN_TOOL_PARAM.NAME:
                        item.Name = sItemValue;
                        break;

					// 2022.02.17 by junho [DEL] Change of Tool ID save file (config -> recovery)
					//case EN_TOOL_PARAM.ID:
					//	item.ID = sItemValue;
					//	break;

                    case EN_TOOL_PARAM.ENABLE:
                        item.Enable = sItemValue.ToUpper().Equals("TRUE") ? true : false;
                        break;
                    case EN_TOOL_PARAM.STARTUP:
                        item.Startup = sItemValue.ToUpper().Equals("TRUE") ? true : false;
                        break;
                    case EN_TOOL_PARAM.STANDSTILL:
                        item.Standstill = sItemValue.ToUpper().Equals("TRUE") ? true : false;
                        break;
                    case EN_TOOL_PARAM.EVENT_EXCHANGE:
                        item.EventExchange = sItemValue.ToUpper().Equals("TRUE") ? true : false;
                        break;
                    case EN_TOOL_PARAM.USAGE_TYPE:
                        item.UsageType = (EN_USAGE_TYPE)Enum.Parse(typeof(EN_USAGE_TYPE), sItemValue);
                        break;
                    case EN_TOOL_PARAM.WORK_END:
                        item.WorkEnd = (EN_WORK_END)Enum.Parse(typeof(EN_WORK_END), sItemValue);
                        break;

                    case EN_TOOL_PARAM.NOTICE_LIMIT_COUNT:
                        item.NoticeLimitCount = int.Parse(sItemValue);
                        break;
                    case EN_TOOL_PARAM.NOTICE_LIMIT_TIME:
                        item.NoticeLimitTime = TimeSpan.Parse(sItemValue);
                        break;
                    case EN_TOOL_PARAM.NOTICE_ALARM_CODE:
                        item.NoticeAlarmCode = int.Parse(sItemValue);
                        break;
                    case EN_TOOL_PARAM.NOTICE_INTERVAL_COUNT:
                        item.NoticeIntervalCount = int.Parse(sItemValue);
                        break;
                    case EN_TOOL_PARAM.NOTICE_INTERVAL_TIME:
                        item.NoticeIntervalTime = TimeSpan.Parse(sItemValue);
                        break;

					// 2021.11.10 by junho [DEL] Initialize time 저장을 Recovery에 하도록 개선 (Reset 해야함)
					//case EN_TOOL_PARAM.INITIAL_TIME:
					//	item.InitialTime = DateTime.Parse(sItemValue);
					//	break;

                    case EN_TOOL_PARAM.WARNING_LIMIT_COUNT:
                        item.WarningLimitCount = int.Parse(sItemValue);
                        break;
                    case EN_TOOL_PARAM.WARNING_LIMIT_TIME:
                        item.WarningLimitTime = TimeSpan.Parse(sItemValue);
                        break;
                    case EN_TOOL_PARAM.WARNING_ALARM_CODE:
                        item.WarningAlarmCode = int.Parse(sItemValue);
                        break;
                    case EN_TOOL_PARAM.WARNING_INTERVAL_COUNT:
                        item.WarningIntervalCount = int.Parse(sItemValue);
                        break;
                    case EN_TOOL_PARAM.WARNING_INTERVAL_TIME:
                        item.WarningIntervalTime = TimeSpan.Parse(sItemValue);
                        break;
                }

                return true;
            }

            return false;
        }
        public bool SetParameter(int nIndex, EN_TOOL_PARAM enParam, TimeSpan tItemValue)
        {
            if (m_dicTools.ContainsKey(nIndex))
            {
                WorkToolItem item = m_dicTools[nIndex];

                switch (enParam)
                {
                    case EN_TOOL_PARAM.NOTICE_LIMIT_TIME:
                        item.NoticeLimitTime = tItemValue;
                        break;
                    case EN_TOOL_PARAM.NOTICE_INTERVAL_TIME:
                        item.NoticeIntervalTime = tItemValue;
                        break;
                    case EN_TOOL_PARAM.WARNING_LIMIT_TIME:
                        item.WarningLimitTime = tItemValue;
                        break;
                    case EN_TOOL_PARAM.WARNING_INTERVAL_TIME:
                        item.WarningIntervalTime = tItemValue;
                        break;
                }

                return true;
            }

            return false;
        }
        public void SetExchangeTool(int nIndex, EventAlarmDelegate pDelegate)
        {
            if (m_dicTools.ContainsKey(nIndex))
            {
                m_dicTools[nIndex].SetExchangeTool(ref pDelegate);
            }
        }
        public bool GetMonitoringData(int nIndex, EN_TOOL_MONITORING enData, ref string sData)
        {
            if (m_dicTools.ContainsKey(nIndex))
            {
                WorkToolItem item = m_dicTools[nIndex];

                switch (enData)
                {
					// 2022.02.17 by junho [ADD] save Tool ID in recovery data
					case EN_TOOL_MONITORING.TOOL_ID:
						sData = item.ID;
						break;

                    case EN_TOOL_MONITORING.TOOL_STATE:
                        sData = item.ToolState.ToString();
                        break;
                    case EN_TOOL_MONITORING.USED_COUNT:
                        sData = item.UsedCount.ToString();
                        break;
                    case EN_TOOL_MONITORING.NOTICE_DIVIDED_VALUE:
                        sData = item.NoticeDividedValue.ToString();
                        break;
                    case EN_TOOL_MONITORING.WARNING_DIVIDED_VALUE:
                        sData = item.WarningDividedValue.ToString();
                        break;

					// 2021.11.10 by junho [ADD] Initialize time 저장을 Recovery에 하도록 개선 (Reset 해야함)
					case EN_TOOL_MONITORING.INITIAL_TIME:
						sData = item.InitialTime.ToString();
						break;

                    case EN_TOOL_MONITORING.USED_TIME:
                        sData = item.UsedTime.ToString();
                        break;
                    case EN_TOOL_MONITORING.LAST_USED_TIME:
                        sData = item.LastUsedTime.ToString();
                        break;

                    case EN_TOOL_MONITORING.TIMER_ACTIVATE:
                        sData = item.TimerActivate.ToString();
                        break;
                    case EN_TOOL_MONITORING.LAST_ACTIVATE_TIME:
                        sData = item.LastActivateTime.ToString();
                        break;
                }

                return true;
            }

            return false;
        }
        public bool GetMonitoringData(int nIndex, EN_TOOL_MONITORING enData, ref DateTime tData)
        {
            if (m_dicTools.ContainsKey(nIndex))
            {
                WorkToolItem item = m_dicTools[nIndex];

                switch (enData)
                {
                    case EN_TOOL_MONITORING.INITIAL_TIME:
                        tData = item.InitialTime;
                        break;
                }

                return true;
            }

            return false;
        }
		public bool SetToolId(int index, string value)
		{
			if(m_dicTools.ContainsKey(index))
			{
				// 2024.08.21 by junho [ADD] 다를 떄에만 변경하도록 변경 (File write 최소화)
				if (m_dicTools[index].ID != value)
                    m_dicTools[index].ID = value;
				return true;
			}

			return false;
		}
		public bool GetToolId(int index, ref string value)
		{
			if (false == m_dicTools.ContainsKey(index))
				return false;

			WorkToolItem item = m_dicTools[index];
			value = item.ID;
			return true;
		}
        #endregion

        private class WorkToolItem
        {
            #region CONSTANT
			// 2022.02.17 by junho [ADD] save Tool ID in recovery data
			private const string TOOL_ID = "TOOL_ID";

            private const string TOOL_STATE = "TOOL_STATE";
            private const string USED_COUNT = "USED_COUNT";
            private const string NOTICE_DIVIDED_VALUE = "NOTICE_DIVIDED_VALUE";
            private const string WARNING_DIVIDED_VALUE = "WARNING_DIVIDED_VALUE";

			// 2021.11.10 by junho [ADD] Initialize time 저장을 Recovery에 하도록 개선 (Reset 해야함)
			private const string INITIAL_TIME = "INITIAL_TIME";

            //23.07.10 by wdw [ADD] Recovery Data 추가 사용시간, Timer 활성, Timer 활성 시작 시간.
            private const string LAST_USED_TIME = "LAST_USED_TIME";
            private const string TIMER_ACTIVATE = "TIMER_ACTIVATE";
            private const string LAST_ACTIVATE_TIME = "LAST_ACTIVATE_TIME";

            #endregion

            #region FIELD
            private Alarm m_InstanceAlarm = null;

            private int m_nIndex = 0;
            private string m_sName = string.Empty;
            private string m_ID = string.Empty;     // Tool 관리 코드
            private bool m_bEnable = true;

            private bool m_bStartup = true;         // Option : 시작 시 알람 발생 여부
            private bool m_bStandstill = true;      // Option : 정지상태 유지 시 알람 발생 여부
            private bool m_bEventExchange = true;   // Option : TOOL 교환 이벤트 발생 여부 

            private EN_USAGE_TYPE m_enUsageType = EN_USAGE_TYPE.NONE;
            private EN_WORK_END m_enWorkEnd = EN_WORK_END.UNIT_PROCESS;
            private EN_TOOL_STATE m_enToolState = EN_TOOL_STATE.NORMAL;     // NEED TO FILE SAVE

            // COUNT
            private int m_nUsedCount = 0;           // 사용된 횟수 (NEED TO FILE SAVE)
            private int m_nNoticeLimitCount = 0;    // Notice Alarm 한계 횟수
            private int m_nWarningLimitCount = 0;   // Warning Alarm 한계 횟수

            // TIME (TimpSpan Fomat > DAY.HOUR:MIN:SEC)
            private DateTime m_tInitialTime = DateTime.Now;         // 초기 시각 (시점)
            private TimeSpan m_tLastUsedTime = TimeSpan.Zero;           //23.07.10 by wdw [MOD] 변경 SPENT TIME -> USED TIME
            private TimeSpan m_tNoticeLimitTime = TimeSpan.Zero;    // Notice Alarm 한계 시간 (간격)
            private TimeSpan m_tWarningLimitTime = TimeSpan.Zero;   // Warning Alarm 한계 시간 (간격)

            // ALARM CODE
            private int m_NoticeAlarmCode;
            private int m_WarningAlarmCode;

            // ~IntervalCount : 한계 값 초과 후 알람을 재발생 시키기 위한 횟수
            // ~IntervalTime : 한계 값 초과 후 알람을 재발생 시키기 위한 시간 간격
            // ~DividedValue : 시간은 횟수와 달리 나눈 수가 정수일 경우가 거의 없으므로 나눈 횟수로 알람의 재발생 시기를 확인

            // NOTICE ALARM ATTRIBUTE
            private int m_nNoticeIntervalCount = 0;
            private TimeSpan m_tNoticeIntervalTime = TimeSpan.Zero;
            private int m_nNoticeDividedValue = 0;      // NEED TO FILE SAVE

            // WARNING ALARM ATTRIBUTE
            private int m_nWarningIntervalCount = 0;
            private TimeSpan m_tWarningIntervalTime = TimeSpan.Zero;
            private int m_nWarningDividedValue = 0;     // NEED TO FILE SAVE

            private bool m_bUnSaveFlag = false;

            // 알람 대신 이벤트 발생 시 콜백 호출
            public EventAlarmDelegate ExchangeToolCallbackFunc = null;

            //23.07.10 by wdw [ADD] Timer 활성, Timer 활성 시작 시간.
            private bool m_bTimerActivate = true;
            private DateTime m_tLastActivateTime = DateTime.Now;         // Activate 시작 시각 (시점)

            #endregion

            #region PROPERTY
            public int Index { get { return m_nIndex; } }
			public string Name { set { m_sName = value; } get { return m_sName; } }
			public string ID { set { m_ID = value; Write(); } get { return m_ID; } }
            public bool Enable { set { m_bEnable = value; } get { return m_bEnable; } }

            public bool Startup { set { m_bStartup = value; } get { return m_bStartup; } }
            public bool Standstill { set { m_bStandstill = value; } get { return m_bStandstill; } }
            public bool EventExchange { set { m_bEventExchange = value; } get { return m_bEventExchange; } }

            public EN_USAGE_TYPE UsageType { set { m_enUsageType = value; } get { return m_enUsageType; } }
            public EN_WORK_END WorkEnd { set { m_enWorkEnd = value; } get { return m_enWorkEnd; } }

			public int NoticeLimitCount { set { m_nNoticeLimitCount = value;  } get { return m_nNoticeLimitCount; } }
			public int WarningLimitCount { set { m_nWarningLimitCount = value; } get { return m_nWarningLimitCount; } }

			public DateTime InitialTime { set { m_tInitialTime = value; Write(); } get { return m_tInitialTime; } }

            //23.07.10 by wdw [Mod] 변경 SPENT TIME -> USED TIME
            public TimeSpan UsedTime 
            { 
                get 
                {
                    TimeSpan tSpentTime = TimeSpan.Zero;

                    if (m_bTimerActivate)
                        tSpentTime = DateTime.Now - m_tLastActivateTime;

                   return m_tLastUsedTime + tSpentTime;
                } 
            }
			public TimeSpan LastUsedTime { set { m_tLastUsedTime = value; Write(); } get { return m_tLastUsedTime; } } 

			public TimeSpan NoticeLimitTime { set { m_tNoticeLimitTime = value; } get { return m_tNoticeLimitTime; } }
			public TimeSpan WarningLimitTime { set { m_tWarningLimitTime = value; } get { return m_tWarningLimitTime; } }

			public int NoticeAlarmCode { set { m_NoticeAlarmCode = value; } get { return m_NoticeAlarmCode; } }
			public int WarningAlarmCode { set { m_WarningAlarmCode = value; } get { return m_WarningAlarmCode; } }

			public int NoticeIntervalCount { set { m_nNoticeIntervalCount = value; } get { return m_nNoticeIntervalCount; } }
			public TimeSpan NoticeIntervalTime { set { m_tNoticeIntervalTime = value;  } get { return m_tNoticeIntervalTime; } }

			public int WarningIntervalCount { set { m_nWarningIntervalCount = value; } get { return m_nWarningIntervalCount; } }
			public TimeSpan WarningIntervalTime { set { m_tWarningIntervalTime = value; } get { return m_tWarningIntervalTime; } }

            //23.07.10 by wdw [ADD] Timer 활성, Timer 활성 시작 시간.
			public bool TimerActivate { set { m_bTimerActivate = value; Write(); } get { return m_bTimerActivate; } }
			public DateTime LastActivateTime { set { m_tLastActivateTime = value; Write(); } get { return m_tLastActivateTime; } }
            #region RECOVERY DATA
            public EN_TOOL_STATE ToolState
            {
                set
                {
                    if (m_enToolState.Equals(value).Equals(false))
                    {
                        m_enToolState = value;
                        Write();
                    }
                }
                get { return m_enToolState; }
            }
            public int UsedCount
            {
                set
                {
                    if (m_nUsedCount.Equals(value).Equals(false))
                    {
                        m_nUsedCount = value;
                        Write();
                    }
                }
                get { return m_nUsedCount; }
            }
            public int NoticeDividedValue
            {
                set
                {
                    if (m_nNoticeDividedValue.Equals(value).Equals(false))
                    {
                        m_nNoticeDividedValue = value;
                        Write();
                    }
                }
                get { return m_nNoticeDividedValue; }
            }
            public int WarningDividedValue
            {
                set
                {
                    if (m_nWarningDividedValue.Equals(value).Equals(false))
                    {
                        m_nWarningDividedValue = value;
                        Write();
                    }
                }
                get { return m_nWarningDividedValue; }
            }
            #endregion

            #endregion

            #region CONSTRUCTOR
            public WorkToolItem(int nIndex)
            {
                m_InstanceAlarm = Alarm.GetInstance();

                m_nIndex = nIndex;

                LoadRecoveryData();
            }
            #endregion

            #region METHOD
            /// <summary>
            /// 알람 또는 이벤트 발생
            /// </summary>
            private void GenerateAlarm(EN_ALARM_GRADE enGrade, bool bPassBtn)
            {
                int nCode = 0;
                
                switch(enGrade)
                {
                    case EN_ALARM_GRADE.NOTICE:
                        nCode = m_NoticeAlarmCode;
                        break;

                    case EN_ALARM_GRADE.WARNING:
                        nCode = m_WarningAlarmCode;
                        break;

                    default:
                        return;
                }

                if (m_bEventExchange)
                {
                    if (ExchangeToolCallbackFunc != null)
                        ExchangeToolCallbackFunc(enGrade, nCode);
                }
                else
                {
                    m_InstanceAlarm.GenerateAlarm(0, 0, nCode, bPassBtn);
                }
            }
            private bool CheckCountNoticeAlarm()
            {
                switch (m_enToolState)
                {
                    case EN_TOOL_STATE.NORMAL:
                        if (m_nUsedCount >= m_nNoticeLimitCount)
                        {
                            // 오직 Notice 한계 값 이상일 때만 ALMOST 로 상태전이
                            ToolState = EN_TOOL_STATE.ALMOST;
                            return true;
                        }
                        break;

                    case EN_TOOL_STATE.ALMOST:
                    case EN_TOOL_STATE.EXCESS:
                        if (m_nUsedCount >= m_nNoticeLimitCount)
                        {
                            if (m_nNoticeIntervalCount.Equals(0))
                                break;

                            int nBaseCount = m_nUsedCount - m_nNoticeLimitCount;
                            int nZeroCount = nBaseCount % m_nNoticeIntervalCount;

                            if (nZeroCount.Equals(0))
                            {
                                return true;
                            }
                        }
                        break;

                    case EN_TOOL_STATE.PERMISSION:
                        break;
                }

                return false;
            }
            private bool CheckCountWarningAlarm()
            {
                switch (m_enToolState)
                {
                    case EN_TOOL_STATE.NORMAL:
                    case EN_TOOL_STATE.ALMOST:
                        if (m_nUsedCount >= m_nWarningLimitCount)
                        {
                            // 오직 Warning 한계 값 이상일 때만 EXCESS 로 상태전이
                            ToolState = EN_TOOL_STATE.EXCESS;
                            return true;
                        }
                        break;

                    case EN_TOOL_STATE.EXCESS:
                        if (m_nUsedCount >= m_nWarningLimitCount)
                        {
                            if (m_nWarningIntervalCount.Equals(0))
                                break;

                            int nBaseCount = m_nUsedCount - m_nWarningLimitCount;
                            int nZeroCount = nBaseCount % m_nWarningIntervalCount;

                            if (nZeroCount.Equals(0))
                            {
                                return true;
                            }
                        }
                        break;

                    case EN_TOOL_STATE.PERMISSION:
                        ToolState = EN_TOOL_STATE.NORMAL;
                        break;
                }

                return false;
            }
            private bool CheckTimeNoticeAlarm()
            {
                switch (m_enToolState)
                {
                    case EN_TOOL_STATE.NORMAL:
                        if (UsedTime >= m_tNoticeLimitTime)
                        {
                            // 오직 Notice 한계 값 이상일 때만 ALMOST 로 상태전이
                            ToolState = EN_TOOL_STATE.ALMOST;
                            return true;
                        }
                        break;

                    case EN_TOOL_STATE.ALMOST:
                    case EN_TOOL_STATE.EXCESS:
                        if (UsedTime >= m_tNoticeLimitTime)
                        {
                            if (m_tNoticeIntervalTime.Equals(TimeSpan.Zero))
                                break;

                            TimeSpan tBaseTime = UsedTime - m_tNoticeLimitTime;
                            int nDividedValue = (int)tBaseTime.TotalSeconds / (int)m_tNoticeIntervalTime.TotalSeconds;  // 소수점 이하 버림

                            if (nDividedValue > m_nNoticeDividedValue)
                            {
                                NoticeDividedValue = nDividedValue;
                                return true;
                            }
                        }
                        break;

                    case EN_TOOL_STATE.PERMISSION:
                        break;
                }

                return false;
            }
            private bool CheckTimeWarningAlarm()
            {
                switch (m_enToolState)
                {
                    case EN_TOOL_STATE.NORMAL:
                    case EN_TOOL_STATE.ALMOST:
                        if (UsedTime >= m_tWarningLimitTime)
                        {
                            // 오직 Warning 한계 값 이상일 때만 EXCESS 로 상태전이
                            ToolState = EN_TOOL_STATE.EXCESS;
                            return true;
                        }
                        break;

                    case EN_TOOL_STATE.EXCESS:
                        if (UsedTime >= m_tWarningLimitTime)
                        {
                            if (m_tWarningIntervalTime.Equals(TimeSpan.Zero))
                                break;

                            TimeSpan tBaseTime = UsedTime - m_tWarningLimitTime;
                            int nDividedValue = (int)tBaseTime.TotalSeconds / (int)m_tWarningIntervalTime.TotalSeconds; // 소수점 이하 버림

                            if (nDividedValue > m_nWarningDividedValue)
                            {
                                WarningDividedValue = nDividedValue;
                                return true;
                            }
                        }
                        break;

                    case EN_TOOL_STATE.PERMISSION:
                        ToolState = EN_TOOL_STATE.NORMAL;
                        break;
                }

                return false;
            }
            protected void Write()
            {
                if (m_bUnSaveFlag)
                    return;

                SaveRecoveryData();
            }
            #endregion

            #region INTERFACE
            public void SetExchangeTool(ref EventAlarmDelegate pDelegate)
            {
                ExchangeToolCallbackFunc = pDelegate;
            }
            /// <summary>
            /// 소모된 TOOL 이 있는지 확인하여 알람을 발생
            /// </summary>
            public bool CheckConsumedTool(EN_WORK_END enEnd)
            {
                if (m_bEnable && m_enWorkEnd.Equals(enEnd))
                {
                    if (m_enToolState.Equals(EN_TOOL_STATE.EXCESS))
                    {
                        GenerateAlarm(EN_ALARM_GRADE.WARNING, false);
                        return true;
                    }
                }

                return false;
            }
            public bool CheckConsumedTool()
            {
                if (m_bEnable && m_bStartup && m_enToolState.Equals(EN_TOOL_STATE.EXCESS))
                {
					// 2023.11.01 by junho [MOD] Entry에서 option 사용된 alarm 창이 떴을때 skip/pass 누르면 이상 동작하는 버그 있으므로 일단 option사용 안하도록 변경
					//GenerateAlarm(EN_ALARM_GRADE.WARNING, true);
					GenerateAlarm(EN_ALARM_GRADE.WARNING, false);
                    return true;
                }

                return false;
            }
            public bool CheckIdTool()
            {
                if (m_bEnable && m_bStartup && m_ID == Def_ToolId)
                {
                    //GenerateAlarm(EN_ALARM_GRADE.WARNING, false);
                    return true;
                }

                return false;
            }
            /// <summary>
            /// 한계 값 이상일 경우 ALARM 을 예약(WARNING) 하거나 즉각(NOTICE) 발생
            /// </summary>
            public EN_ALARM_GRADE CheckOverLimit(EN_USAGE_TYPE enUsageType)
            {
                if (m_bEnable.Equals(false))
                    return EN_ALARM_GRADE.NORMAL;

                if(m_enUsageType.Equals(enUsageType).Equals(false))
                    return EN_ALARM_GRADE.NORMAL;

                switch(m_enUsageType)
                {
                    case EN_USAGE_TYPE.COUNT:
                        if (CheckCountWarningAlarm())
                        {
                            // TOOL STATE 를 EXCESS 로 전환하여 알람을 예약한다 (설정된 조건으로 알람 발생)
                            return EN_ALARM_GRADE.WARNING;
                        }

                        if (CheckCountNoticeAlarm())
                        {
                            if (m_bStandstill || TaskOperator.GetInstance().IsRunningMode())
                            {
                                // 즉각 알람 발생
                                GenerateAlarm(EN_ALARM_GRADE.NOTICE, false);
                            }
                            return EN_ALARM_GRADE.NOTICE;
                        }
                        break;

                    case EN_USAGE_TYPE.TIME:
                        if (CheckTimeWarningAlarm())
                        {
                            // TOOL STATE 를 EXCESS 로 전환하여 알람을 예약한다 (설정된 조건으로 알람 발생)
                            return EN_ALARM_GRADE.WARNING;
                        }

                        if (CheckTimeNoticeAlarm())
                        {
                            if (m_bStandstill || TaskOperator.GetInstance().IsRunningMode())
                            {
                                // 즉각 알람 발생
                                GenerateAlarm(EN_ALARM_GRADE.NOTICE, false);
                            }
                            return EN_ALARM_GRADE.NOTICE;
                        }
                        break;
                }

                return EN_ALARM_GRADE.NORMAL;
            }
            /// <summary>
            /// 사용량을 증가 시킴
            /// </summary>
            public void IncreaseUsedCount()
            {
                if (m_enUsageType.Equals(EN_USAGE_TYPE.COUNT))
                {
                    UsedCount++;
                }
            }
            /// <summary>
            /// 23.07.04 by wdw [ADD] EXTERNAL DEVICE 시간 설정
            /// 사용량을 증가 시킴
            /// </summary>
            public void SetUsedTime(TimeSpan tUsedTime)
            {
                m_tLastUsedTime = tUsedTime;
                Write();
            }
            /// <summary>
            /// 23.07.04 by wdw [ADD] EXTERNAL DEVICE 시간 증가
            /// 사용량을 증가 시킴
            /// </summary>
            public void IncreaseUsedTime(TimeSpan tUsedTime)
            {
                m_tLastUsedTime += tUsedTime;
                Write();
            }
            /// <summary>
            /// 사용횟수 = 0, 초기시각 = 현재시각, 사용시간 = 0 으로 설정
            /// </summary>
            public void SetZero(bool bActivateTimer = true)
            {
                m_enToolState = EN_TOOL_STATE.NORMAL;

                m_nUsedCount = 0;

                DateTime CurrentTime = DateTime.Now;
                m_tInitialTime = CurrentTime;
                m_tLastUsedTime = TimeSpan.Zero;

                m_bTimerActivate = bActivateTimer;
                m_tLastActivateTime = CurrentTime;

                m_nNoticeDividedValue = 0;
                m_nWarningDividedValue = 0;

                Write();
            }
            /// <summary>
            /// 23.07.04 by wdw [ADD] EXTERNAL DEVICE 시간 증가
            /// 사용량을 증가 시킴
            /// </summary>
            public void SetActivate(bool bActivate, bool bUpdateUsedTime)
            {
                if (m_bTimerActivate == bActivate)
                    return;

                m_bTimerActivate = bActivate;

                if (bActivate)
                {
                    m_tLastActivateTime = DateTime.Now;
                }
                else
                {
                    if(bUpdateUsedTime)
                    {
                        TimeSpan tSpentTime = DateTime.Now - m_tLastActivateTime;
                        m_tLastUsedTime += tSpentTime;
                    }
                }
                Write();
            }
            /// <summary>
            /// 서로 다른 RECOVER DATA 에 연속으로 값을 할당할 경우 파일로 저장을 하지 않는다 (파일저장 최적화)
            /// </summary>
            public void UnSave()
            {
                m_bUnSaveFlag = true;
            }
            /// <summary>
            /// UNSAVE 를 한 경우 서로 다른 RECOVERY DATA 에 할당된 값을 파일로 저장한다 (파일저장 최적화)
            /// </summary>
            public void Save()
            {
                m_bUnSaveFlag = false;
                Write();
            }
            #endregion

            #region FILE I/O
            private bool SaveRecoveryData()
            {
                FileComposite fComp = FileComposite.GetInstance();

                string sRootGroupName = "TOOL_" + m_nIndex.ToString("00");

				// 2021.11.10 by junho [ADD] Recovery data save 안되는 현상 개선
				fComp.RemoveRoot(sRootGroupName);
				if (fComp.CreateRoot(sRootGroupName).Equals(false))
					return false;

				string[] groupName = new string[] { "BASE" };
				if (fComp.AddGroup(sRootGroupName, groupName).Equals(false)) return false;

				bool addResult = true;

				// 2022.02.17 by junho [ADD] save Tool ID in recovery data
				addResult &= fComp.AddItem(sRootGroupName, TOOL_ID, m_ID, groupName);

				addResult &= fComp.AddItem(sRootGroupName, TOOL_STATE, m_enToolState.ToString(), groupName);
				addResult &= fComp.AddItem(sRootGroupName, USED_COUNT, m_nUsedCount, groupName);
				addResult &= fComp.AddItem(sRootGroupName, NOTICE_DIVIDED_VALUE, m_nNoticeDividedValue, groupName);
				addResult &= fComp.AddItem(sRootGroupName, WARNING_DIVIDED_VALUE, m_nWarningDividedValue, groupName);

				// 2021.11.10 by junho [ADD] Initialize time 저장을 Recovery에 하도록 개선 (Reset 해야함)
                addResult &= fComp.AddItem(sRootGroupName, INITIAL_TIME, m_tInitialTime.ToString("yyyy-MM-dd HH:mm:ss.fff"), groupName);

                //23.07.10 by wdw [ADD] Recovery Data 추가 사용시간, Timer 활성, Timer 활성 시작 시간.
                addResult &= fComp.AddItem(sRootGroupName, LAST_USED_TIME, m_tLastUsedTime.ToString(), groupName);
                addResult &= fComp.AddItem(sRootGroupName, TIMER_ACTIVATE, m_bTimerActivate.ToString(), groupName);
                addResult &= fComp.AddItem(sRootGroupName, LAST_ACTIVATE_TIME, m_tLastActivateTime.ToString("yyyy-MM-dd HH:mm:ss.fff"), groupName);

				if (addResult.Equals(false))
					return false;

                FileIOManager fIO = FileIOManager.GetInstance();

                string sFilePath = FilePath.FILEPATH_RECOVERY;
                string sFileName = sRootGroupName + FileFormat.FILEFORMAT_RECOVERY;
                string sFileData = string.Empty;

				if (fComp.GetStructureAsString(sRootGroupName, ref sFileData).Equals(false))
                    return false;

                return fIO.Write(sFilePath, sFileName, sFileData, false, false);
            }
            private void LoadRecoveryData()
            {
                string sRootGroupName = "TOOL_" + m_nIndex.ToString("00");

                string sFilePath = FilePath.FILEPATH_RECOVERY;
                string sFileName = sRootGroupName + FileFormat.FILEFORMAT_RECOVERY;
                string sFullFilePath = sFilePath + "\\" + sFileName;

                if (System.IO.File.Exists(sFullFilePath).Equals(false))
                    return;

                FileIOManager fIO = FileIOManager.GetInstance();

                string sFileData = string.Empty;

                if (fIO.Read(sFilePath, sFileName, ref sFileData, FileIO.TIMEOUT_READ).Equals(false))
                    return;

                FileComposite fComp = FileComposite.GetInstance();

				// 2021.07.03. by shkim. [ADD] 다시 읽을 때는 루트를 지워줘야 한다.
				fComp.RemoveRoot(sRootGroupName);
                string[] arData = sFileData.Split('\n');

				if (fComp.CreateRootByString(ref arData).Equals(false))
					return;

				// 2021.11.10 by junho [ADD] Recovery data save 안되는 현상 개선
				bool getResult = true;
				string sToolState = string.Empty, sDateTime = string.Empty;
				string[] groupName = new string[] { "BASE" };

				// 2022.02.17 by junho [ADD] save Tool ID in recovery data
				getResult &= fComp.GetValue(sRootGroupName, TOOL_ID, ref m_ID, groupName);

				getResult &= fComp.GetValue(sRootGroupName, TOOL_STATE, ref sToolState, groupName);
				getResult &= fComp.GetValue(sRootGroupName, USED_COUNT, ref m_nUsedCount, groupName);
				getResult &= fComp.GetValue(sRootGroupName, NOTICE_DIVIDED_VALUE, ref m_nNoticeDividedValue, groupName);
				getResult &= fComp.GetValue(sRootGroupName, WARNING_DIVIDED_VALUE, ref m_nWarningDividedValue, groupName);

				// 2021.11.10 by junho [ADD] Initialize time 저장을 Recovery에 하도록 개선 (Reset 해야함)
				getResult &= fComp.GetValue(sRootGroupName, INITIAL_TIME, ref sDateTime, groupName);

                //23.07.10 by wdw [ADD] Recovery Data 추가 사용시간, Timer 활성, Timer 활성 시작 시간.
				string sUsedTime = string.Empty, sActivateTime = string.Empty;
                getResult &= fComp.GetValue(sRootGroupName, LAST_USED_TIME, ref sUsedTime, groupName);
                getResult &= fComp.GetValue(sRootGroupName, TIMER_ACTIVATE, ref m_bTimerActivate, groupName);
                getResult &= fComp.GetValue(sRootGroupName, LAST_ACTIVATE_TIME, ref sActivateTime, groupName);
                getResult &= TimeSpan.TryParse(sUsedTime, out m_tLastUsedTime);
                getResult &= DateTime.TryParse(sActivateTime, out m_tLastActivateTime);

				getResult &= Enum.TryParse(sToolState, out m_enToolState);
				getResult &= DateTime.TryParse(sDateTime, out m_tInitialTime);

				if (getResult.Equals(false))
					return;
			}
            #endregion
        }
    }
}
