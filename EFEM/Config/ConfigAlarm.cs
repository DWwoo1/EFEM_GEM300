using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;

using Alarm_;
using DesignPattern_.Observer_;
using FileBorn_;

using Define.DefineEnumBase.Log;

namespace FrameOfSystem3.Config
{
	extern alias AlarmInstance;
	
	public class ConfigAlarm : IObserver
	{
		#region <Alarm Rule>
		///
		/// Alarm ID : ABCDEF
		///AB0000 : AB Task의 SequenceTimeout
		///
		///[AB] 
		///정의 : Task Number
		///범위 : 01~99
		///구분 : 01~99 Task
		///총계 : 99 개
		///종류 : Task
		///
		///[CD]
		///정의 : Device Object Index
		///범위 : 00, 01~99
		///구분
		///	1) 00 : 각 Task에서의 SequenceTimeout을 포함한 사용자 정의 알람 
		///	2) 01~99 : Motion, Digital Input, Digital Output, Analog Input, Analog Output, Cylinder가 TaskDevice에 등록된 순서의 Index
		///총계 : 100 개
		///종류 : Task 관련 예약 알람, Motion, Digital Input, Digital Output, Cylinder, Analog Input, Analog Output
		///
		///[EF]
		///정의 : Alarm Code
		///범위 : 00~99
		///구분
		///	1) DeviceID가 00인 경우
		///			00 : Task Sequence Timeout Alarm
		///			01~99 : 태스크 관련 예약 (Sequence 내의 사용자 정의 알람)
		///	2) DeviceID가 00이 아닌 경우
		///		   10~29 : 모션 관련 예약
		///		   30~39 : 실린더 관련 예약
		///		   40~44 : Digital Input 예약
		///		   45~49 : Digital Output 예약
		///		   50~54 : Analog Input 예약
		///		   55~59 : Analog Output 예약
		///		   
		#endregion </Alarm Rule>

		private ConfigAlarm() {}
		
        #region 열거형
		public enum EN_PARAM_ALARM
		{
			ALARMCODE = 0,
			GRADE = 1,
			MESSAGECODE = 2,
			SOLUTIONCODE = 3,
			OUTPUT_BUZZER = 4,
		}
		public enum EN_ALARM_STATE
		{
			NORMAL	= 0,
			NOTICE	= 1,
			WARNING = 2,
			ERROR	= 3,
		}
		public enum EN_ALARM_RESULT
		{
			NONE	= 0,
			SKIP	= 1,
			PASS	= 2,
			ABORT	= 3,
			BUZZER	= 4,
			RESET	= 5,
		}
		#endregion

		#region 싱글톤
		private static ConfigAlarm _Instance	= new ConfigAlarm();
		public static ConfigAlarm GetInstance()
		{
			return _Instance;
		}
		#endregion

		#region 상수

		private const int RESERVED_NUMBER			= 0;

		#region Message
		private const int MESSAGE_CODE_NUMBERING	= 1000;
		#endregion

		#region Default Value
		private readonly string DEFAULT_GRADE		= "ERROR";
		private const int DEFAULT_ALARMCODE			= -1;
		private const int DEFAULT_MESSAGECODE		= -1;
		private const int DEFAULT_SOLUTIONCODE		= -1;
		private const int DEFAULT_BUZZERIO			= -1;
		#endregion
		
		#endregion

		#region 변수
		Dictionary<EN_PARAM_ALARM, PARAM_LIST> m_DicForItem				= new Dictionary<EN_PARAM_ALARM, PARAM_LIST>();
		Dictionary<EN_PARAM_ALARM, string> m_DicForStorage				= new Dictionary<EN_PARAM_ALARM, string>();

		Dictionary<EN_ALARM_RESULT, AlarmInstance.Alarm_.ALARM_RESULT> m_DicForAlarmResult	
																		= new Dictionary<EN_ALARM_RESULT,AlarmInstance.Alarm_.ALARM_RESULT>();
		Dictionary<string, int> m_DicForAlarmGrade						= new Dictionary<string, int>();
		Dictionary<int, EN_ALARM_STATE> m_DicForAlarmGradeToEnum		= new Dictionary<int,EN_ALARM_STATE>();

		Dictionary<int, string> m_DicForGeneratedTime					= new Dictionary<int,string>();

		System.Timers.Timer m_timer										= null;

        #region Instances
        private ConfigTask m_instanceOfTask                             = null;
        private FrameOfSystem3.Log.LogManager m_InstanceOfLog			= null;
		private Alarm m_InstanceOfAlaramDLL								= null;
		private Functional.Storage m_InstanceOfStorage					= null;
        private ConfigLanguage m_instanceLangage                        = null;
        #endregion End - Instances

        private int m_nCountOfGeneratedAlarm							= 0;
		private int[] m_arGeneratedAlarm								= null;

        #region Message
        private Dictionary<int, string[]> m_dicOfSubInformation         = new Dictionary<int,string[]>();
        private Dictionary<int, string> m_dicOfAlarmMessage             = new Dictionary<int,string>();
        private System.Threading.ReaderWriterLockSlim m_rwlockForSubInformation     = new System.Threading.ReaderWriterLockSlim();
        #endregion

		#endregion

		#region 감시자 관련
		/// <summary>
		/// 2020.08.17 by twkang [ADD] 감시자의 주체를 업데이트한다.
		/// </summary>
		public void UpdateObserver(Subject pSubject)
		{
            if(m_timer != null)
                m_timer.Start();
		}
		/// <summary>
		/// 2020.08.17 by twkang [ADD] 감시자를 등록한다.
		/// </summary>
		public void RegisterSubject(Subject pSubject)
		{
			if (pSubject is Alarm)
			{
				m_InstanceOfAlaramDLL = pSubject as Alarm;
			}

			pSubject.Attach(this);
		}
		#endregion

		#region 외부인터페이스
		/// <summary>
		/// 2020.06.22 by twkang [ADD] Alarm 클래스를 초기화한다.
		/// </summary>
		public bool Init()
		{
			MakeMappingTable();

            m_instanceOfTask        = ConfigTask.GetInstance();
			m_InstanceOfAlaramDLL	= Alarm.GetInstance();
			m_InstanceOfStorage		= Functional.Storage.GetInstance();
			m_InstanceOfLog			= FrameOfSystem3.Log.LogManager.GetInstance();
            m_instanceLangage       = ConfigLanguage.GetInstance();

			if (false == m_InstanceOfStorage.Load(Define.DefineEnumBase.Storage.EN_STORAGE_LIST.ALARM))
			{
				return false;
			}

			InitializeAlarmParameter();

            #region Timer
            m_timer					= new System.Timers.Timer();
			m_timer.BeginInit();
			m_timer.AutoReset		= false;
			m_timer.Elapsed			+= FunctionForTimer;
			m_timer.EndInit();
			#endregion

			return true;
		}

		#region SetInformation
		/// <summary>
		/// 2020.06.22 by twkang [ADD] Alarm 의 파라미터 값을 변경한다.
		/// </summary>
		public bool SetParameter<T>(int nIndexOfItem, EN_PARAM_ALARM enList, T tValue, bool bSave = true)
		{
			string[] strArrGroup	= new string[] { nIndexOfItem.ToString()};

			if (m_InstanceOfStorage.SetParameter(Define.DefineEnumBase.Storage.EN_STORAGE_LIST.ALARM
				, m_DicForStorage[enList]
				, tValue
				, ref strArrGroup
				, bSave))
			{
				return m_InstanceOfAlaramDLL.SetParameter(nIndexOfItem, m_DicForItem[enList], tValue);
			}
			return false;
		}
		/// <summary>
		/// 2020.07.10 by twkang [ADD] 알람 결과를 설정한다.
		/// </summary>
		public void SetAlarmResult(EN_ALARM_RESULT enResult)
		{
			if (false == m_DicForAlarmResult.ContainsKey(enResult))
			{
				return;
			}
			m_InstanceOfAlaramDLL.SetAlarmResult(m_DicForAlarmResult[enResult]);
		}
		#endregion

        #region GetInformation
        /// <summary>
		/// 2020.06.22 by twkang [ADD] Alarm 파라미터 값을 반환한다.
		/// </summary>
		public bool GetParameter<T>(int nIndexOfitem, EN_PARAM_ALARM enList, ref T tValue)
		{
			return m_InstanceOfAlaramDLL.GetParameter(nIndexOfitem, m_DicForItem[enList], ref tValue);
		}
		/// <summary>
		/// 2020.06.22 by twkang [ADD] 아이템 리스트를 반환한다.
		/// </summary>
		public bool GetListOfItem(ref int[] arrIndex)
		{
			return m_InstanceOfAlaramDLL.GetListOfItem(ref arrIndex);
		}
		/// <summary>
		/// 2020.07.09 by twkang [ADD] 발생한 알람 리스트들을 반환한다.
		/// </summary>
		public bool GetListOfGeneratedAlarm(ref int[] arIndexes)
		{
			return m_InstanceOfAlaramDLL.GetListOfGeneratedAlarm(ref arIndexes);
		}
		/// <summary>
		/// 2020.07.09 by twkang [ADD] 알람 상태를 반환한다. 
		/// </summary>
		public EN_ALARM_STATE GetState(ref bool bUseOption)
		{
			return m_DicForAlarmGradeToEnum[(int)m_InstanceOfAlaramDLL.GetAlarmState(ref bUseOption)];
		}
		/// <summary>
		/// 2020.11.03 by twkang [ADD] 알람 발생한 시간을 반환한다.
		/// </summary>
		public bool GetGeneratedTime(int nIndexOfItem, ref string strTime)
		{
			if(false == m_DicForGeneratedTime.ContainsKey(nIndexOfItem))
			{
				return false;
			}

			strTime	= m_DicForGeneratedTime[nIndexOfItem];
			m_DicForGeneratedTime.Remove(nIndexOfItem);
			return true;
		}

		#endregion

		#region ADD & REMOVE
		/// <summary>
		/// 2020.06.26 by twkang [ADD] 아이템을 추가한다.
		/// </summary>
		public int AddAlarmItem()
		{
			int nItemNum		= m_InstanceOfAlaramDLL.AddItem();
			SetDefaultValue(nItemNum);

			string[] arGroup	= null;
			string[] arData		= null;

			if(FileBorn.GetInstance().GetItemFrame(BORN_LIST.ALARM, ref arGroup, nItemNum.ToString(), ref arData)
				&& m_InstanceOfStorage.AddGroupAndItem(Define.DefineEnumBase.Storage.EN_STORAGE_LIST.ALARM, ref arData))
			{
				SaveParameterToStorage(nItemNum);
				return nItemNum;
			}

			m_InstanceOfAlaramDLL.RemoveItem(nItemNum);
			return -1;
		}
		/// <summary>
		/// 2020.06.22 by twkang [ADD] 아이템을 삭제한다.
		/// </summary>
		public bool RemoveItem(int nIndexOfItem)
		{
			string[] strArrGroup		= new string[1] { nIndexOfItem.ToString() };

			if (m_InstanceOfStorage.RemoveGroup(Define.DefineEnumBase.Storage.EN_STORAGE_LIST.ALARM, ref strArrGroup))
			{
				return m_InstanceOfAlaramDLL.RemoveItem(nIndexOfItem);
			}
			
			return false;
		}

        /// <summary>
        /// 2022.04.07 by junho [ADD] Array로 신규 알람 생성하기
        /// </summary>
        public bool AddAlarmItemArray(int addCount, int[] alarmCodeList, int[] messageCodeList, string[] gradeList, int[] solutionCodeList, int[] buzzerList)
        {
            if (addCount != alarmCodeList.Length
                || addCount != messageCodeList.Length
                || addCount != gradeList.Length
                || addCount != solutionCodeList.Length
                || addCount != buzzerList.Length)
                return false;

            for (int i = 0; i < addCount; ++i)
            {
                // 동일 알람코드가 이미 등록되어있는지 확인
                int nItemNum = m_InstanceOfAlaramDLL.GetAlarmIndexByAlarmCode(alarmCodeList[i]);

                // 없으면 새로 생성
                if(nItemNum == -1)
                    nItemNum = m_InstanceOfAlaramDLL.AddItem();

                m_InstanceOfAlaramDLL.SetParameter(nItemNum, PARAM_LIST.ALARM_GRADE, gradeList[i]);
                m_InstanceOfAlaramDLL.SetParameter(nItemNum, PARAM_LIST.CODE_ALARM, alarmCodeList[i]);
                m_InstanceOfAlaramDLL.SetParameter(nItemNum, PARAM_LIST.CODE_MESSAGE, messageCodeList[i]);
                m_InstanceOfAlaramDLL.SetParameter(nItemNum, PARAM_LIST.CODE_SOLUTION, solutionCodeList[i]);
                m_InstanceOfAlaramDLL.SetParameter(nItemNum, PARAM_LIST.OUTPUT_BUZZER, buzzerList[i]);

                string[] arGroup = null;
                string[] arData = null;

                if (FileBorn.GetInstance().GetItemFrame(BORN_LIST.ALARM, ref arGroup, nItemNum.ToString(), ref arData)
                    && m_InstanceOfStorage.AddGroupAndItem(Define.DefineEnumBase.Storage.EN_STORAGE_LIST.ALARM, ref arData))
                {
                    SaveParameterToStorage(nItemNum);
                    continue;
                }

                m_InstanceOfAlaramDLL.RemoveItem(nItemNum);
                return false;
            }

            return true;
        }
		#endregion

        #region Write Logging
        /// <summary>
        /// 2020.10.06 by twkang [ADD] 발생한 알람을 로그로 작성한다.
        /// </summary>
        public void WriteLog(int nAlarmIndex, EN_ALM_TYPE enType)
        {
            string strGrade     = string.Empty;
            int nAlarmCode      = 0;
            int nMessageCode    = -1;
            string strModule    = "EQUIPMENT";
            string strMessage   = string.Empty;

            // 1. Get the information of the alarm.
            m_InstanceOfAlaramDLL.GetParameter(nAlarmIndex, PARAM_LIST.ALARM_GRADE, ref strGrade);
            m_InstanceOfAlaramDLL.GetParameter(nAlarmIndex, PARAM_LIST.CODE_ALARM, ref nAlarmCode);
            m_InstanceOfAlaramDLL.GetParameter(nAlarmIndex, PARAM_LIST.CODE_MESSAGE, ref nMessageCode);

            // 2. Get the message on the alarm code.
            GetAlarmMessage(ref nAlarmIndex, ref strMessage);

            // 3. Parse the module by the code.
            if(nAlarmCode >= (int)AlarmInstance::Alarm_.ALARM_CODE_SECTION.TASK)
            {
                int nInstanceOfTask     = nAlarmCode / (int)AlarmInstance::Alarm_.ALARM_CODE_SECTION.TASK;

                m_instanceOfTask.GetTaskNameByInstance(ref nInstanceOfTask, ref strModule);
            }

            m_InstanceOfLog.WriteAlarmLog(ref strModule, enType, ref strGrade, ref nAlarmCode, ref strMessage);

            // 4. If the releasing the alarm, remove the sub information.
            switch(enType)
            {
                case EN_ALM_TYPE.OCCURRED:
                    SECSGEM.ScenarioOperator.Instance.ExecuteAlarmScenario(nAlarmCode, SECSGEM.DefineSecsGem.EN_GEM_ALARM_STATE.OCCURED);
                    break;
                case EN_ALM_TYPE.RELEASED:
                    SECSGEM.ScenarioOperator.Instance.ExecuteAlarmScenario(nAlarmCode, SECSGEM.DefineSecsGem.EN_GEM_ALARM_STATE.CLEARED);
					RemoveSubInformation(ref nAlarmCode);
                    break;
                default:
                    break;
            }
        }
        #endregion End - Write Logging

        #region Message
        /// <summary>
        /// 2020.10.12 by yjlee [ADD] Add a subinformation to make the message.
        /// </summary>
        public void AddSubInformation(ref int nAlarmCode, ref string[] arSubInformation)
        {
            if (null != arSubInformation)
            {
                m_rwlockForSubInformation.EnterWriteLock();

                if (m_dicOfSubInformation.ContainsKey(nAlarmCode))
                {
                    m_dicOfSubInformation[nAlarmCode] = arSubInformation;
                }
                else
                {
                    m_dicOfSubInformation.Add(nAlarmCode, arSubInformation);
                }

                m_rwlockForSubInformation.ExitWriteLock();
            }
        }
        /// <summary>
        /// 2021.01.15 by yjlee [ADD] Get the alarm message.
        /// </summary>
        public void GetAlarmMessage(ref int nIndexOfItem, ref string strMessage)
        {
            int nAlarmCode      = 0;

            if(m_InstanceOfAlaramDLL.GetParameter(nIndexOfItem, PARAM_LIST.CODE_ALARM, ref nAlarmCode))
            {
                m_rwlockForSubInformation.EnterWriteLock();

                if (m_dicOfAlarmMessage.ContainsKey(nAlarmCode))
                {
                    strMessage          = m_dicOfAlarmMessage[nAlarmCode];
                }
                else
                {
                    int nMessageCode    = 0;
                    var enLanguage      = m_instanceLangage.GetLangauge();

                    if(m_InstanceOfAlaramDLL.GetParameter(nIndexOfItem, PARAM_LIST.CODE_MESSAGE, ref nMessageCode)
                        && m_instanceLangage.GetParameter(ConfigLanguage.EN_TYPE_FORMAT.SENTENCE, nMessageCode, enLanguage, ref strMessage))
                    { 
                        if(m_dicOfSubInformation.ContainsKey(nAlarmCode))
                        {
                            int nCountOfToken       = Define.DefineConstant.Message.MESSAGE_TOKEN.Length;
                            var arSubInformation    = m_dicOfSubInformation[nAlarmCode];

                            foreach(var strSubInfo in arSubInformation)
                            {
                                int nIndexOfToken       = strMessage.IndexOf(Define.DefineConstant.Message.MESSAGE_TOKEN);

                                if (0 > nIndexOfToken) {  break; }

                                strMessage      = strMessage.Remove(nIndexOfToken, nCountOfToken);
                                strMessage      = strMessage.Insert(nIndexOfToken, strSubInfo);
                            }
                        }
                        
                        // 2021.01.15 by yjlee [ADD] Save the description of the alarm.
                        if(false == m_dicOfAlarmMessage.ContainsKey(nAlarmCode))
                        {
                            m_dicOfAlarmMessage.Add(nAlarmCode, strMessage);
                        }
                        else
                        {
                            m_dicOfAlarmMessage[nAlarmCode]     = strMessage;
                        }
                    }
                }

                m_rwlockForSubInformation.ExitWriteLock();
            }
        }
        #endregion

		#endregion

		#region 내부인터페이스
		/// <summary>
		/// 2020.06.22 by twkang [ADD] Storage 데이터로 Alarm 파라미터를 초기화한다.
		/// </summary>
		private void InitializeAlarmParameter()
		{
			string strValue			= string.Empty;
			string[] strArrGroup	= null;

			bool bShouldBeSave		= false;

			if(m_InstanceOfStorage.GetListOfGroup(Define.DefineEnumBase.Storage.EN_STORAGE_LIST.ALARM, ref strArrGroup))
            {
                for(int nIndex = 0, nEnd = strArrGroup.Length; nIndex < nEnd; ++nIndex)
			    {
					string[] arGroup		= new string[] { strArrGroup[nIndex] };
					int nIndexOfItem		= int.Parse(strArrGroup[nIndex]);

					m_InstanceOfAlaramDLL.AddItem(nIndexOfItem);
					SetDefaultValue(nIndexOfItem);

			    	foreach(EN_PARAM_ALARM en in Enum.GetValues(typeof(EN_PARAM_ALARM)))
			    	{
			    		if(m_InstanceOfStorage.GetParameter(Define.DefineEnumBase.Storage.EN_STORAGE_LIST.ALARM
			    			, m_DicForStorage[en]
			    			, ref strValue
			    			, ref arGroup))
			    		{
							switch(en)
							{
								case EN_PARAM_ALARM.MESSAGECODE:
									if(strValue != DEFAULT_MESSAGECODE.ToString())
										break;

									if(SetDefaultMessageCode(nIndexOfItem))
									{
										bShouldBeSave = true;
										continue;
									}
									break;
								default:
									break;
							}

							m_InstanceOfAlaramDLL.SetParameter(nIndexOfItem, m_DicForItem[en], strValue);
			    		}
						else 
						{
							bShouldBeSave = true;
							m_InstanceOfStorage.AddItem(Define.DefineEnumBase.Storage.EN_STORAGE_LIST.ALARM, m_DicForItem[en].ToString(), string.Empty, ref arGroup, false);
						}
			    	}
			    }
            }

			if(bShouldBeSave)
				m_InstanceOfStorage.Save(Define.DefineEnumBase.Storage.EN_STORAGE_LIST.ALARM);
		}
		/// <summary>
		/// 2020.06.26 by twkang [ADD] 파라미터 값을 Storage 데이터로 저장한다.
		/// </summary>
		private bool SaveParameterToStorage(int nIndexOfItem)
		{
			string[] arGroup	= null;
			string[] arData		= null;

			if (false == FileBorn.GetInstance().GetItemFrame(BORN_LIST.ALARM, ref arGroup, nIndexOfItem.ToString(), ref arData)
				|| false == m_InstanceOfStorage.AddGroupAndItem(Define.DefineEnumBase.Storage.EN_STORAGE_LIST.ALARM, ref arData))
			{
				return false;
			}
			
			arGroup				= new string[] {nIndexOfItem.ToString()};

			foreach(EN_PARAM_ALARM en in Enum.GetValues(typeof(EN_PARAM_ALARM)))
			{
				string strValue		= string.Empty;

				m_InstanceOfAlaramDLL.GetParameter(nIndexOfItem, m_DicForItem[en], ref strValue);

				switch(en)
				{
					case EN_PARAM_ALARM.MESSAGECODE:
						if(strValue != DEFAULT_MESSAGECODE.ToString())
							break;

						if (SetDefaultMessageCode(nIndexOfItem))
							continue;
						break;
					default:
						break;
				}

				m_InstanceOfStorage.SetParameter(Define.DefineEnumBase.Storage.EN_STORAGE_LIST.ALARM, m_DicForStorage[en], strValue, ref arGroup, false);
			}
			m_InstanceOfStorage.Save(Define.DefineEnumBase.Storage.EN_STORAGE_LIST.ALARM);

			return true;
		}
		/// <summary>
		/// 2020.07.02 by twkang [ADD] 클래스에서 사용할 Mapping Table 을 만든다.
		/// </summary>
		private void MakeMappingTable()
		{
			#region Parameter
			m_DicForItem.Clear();
			m_DicForItem.Add(EN_PARAM_ALARM.ALARMCODE, PARAM_LIST.CODE_ALARM);
			m_DicForItem.Add(EN_PARAM_ALARM.GRADE, PARAM_LIST.ALARM_GRADE);
			m_DicForItem.Add(EN_PARAM_ALARM.MESSAGECODE, PARAM_LIST.CODE_MESSAGE);
			m_DicForItem.Add(EN_PARAM_ALARM.SOLUTIONCODE, PARAM_LIST.CODE_SOLUTION);
			m_DicForItem.Add(EN_PARAM_ALARM.OUTPUT_BUZZER, PARAM_LIST.OUTPUT_BUZZER);
			#endregion

			#region Storage
			m_DicForStorage.Clear();
			foreach (EN_PARAM_ALARM en in Enum.GetValues(typeof(EN_PARAM_ALARM)))
			{
				m_DicForStorage.Add(en, en.ToString());
			}
			#endregion

			#region Alaram Grade
			m_DicForAlarmGradeToEnum.Clear();
			foreach (EN_ALARM_STATE en in Enum.GetValues(typeof(EN_ALARM_STATE)))
			{
				m_DicForAlarmGradeToEnum.Add((int)en, en);
			}
			#endregion

			#region Alarm Result
			m_DicForAlarmResult.Clear();
			m_DicForAlarmResult.Add(EN_ALARM_RESULT.ABORT, AlarmInstance.Alarm_.ALARM_RESULT.ABORT);
			m_DicForAlarmResult.Add(EN_ALARM_RESULT.BUZZER, AlarmInstance.Alarm_.ALARM_RESULT.BUZZER);
			m_DicForAlarmResult.Add(EN_ALARM_RESULT.NONE, AlarmInstance.Alarm_.ALARM_RESULT.NONE);
			m_DicForAlarmResult.Add(EN_ALARM_RESULT.PASS, AlarmInstance.Alarm_.ALARM_RESULT.PASS);
			m_DicForAlarmResult.Add(EN_ALARM_RESULT.RESET, AlarmInstance.Alarm_.ALARM_RESULT.RESET);
			m_DicForAlarmResult.Add(EN_ALARM_RESULT.SKIP, AlarmInstance.Alarm_.ALARM_RESULT.SKIP);
			#endregion
		}
		/// <summary>
		/// 2020.09.29 by twkang [ADD] 해당 아이템의 값을 기본값으로 설정한다.
		/// </summary>
		private void SetDefaultValue(int nIndex)
		{
			m_InstanceOfAlaramDLL.SetParameter(nIndex, PARAM_LIST.ALARM_GRADE, DEFAULT_GRADE);
			m_InstanceOfAlaramDLL.SetParameter(nIndex, PARAM_LIST.CODE_ALARM, DEFAULT_ALARMCODE);
			m_InstanceOfAlaramDLL.SetParameter(nIndex, PARAM_LIST.CODE_MESSAGE, DEFAULT_MESSAGECODE);
			m_InstanceOfAlaramDLL.SetParameter(nIndex, PARAM_LIST.CODE_SOLUTION, DEFAULT_SOLUTIONCODE);
			m_InstanceOfAlaramDLL.SetParameter(nIndex, PARAM_LIST.OUTPUT_BUZZER, DEFAULT_BUZZERIO);
		}
		/// <summary>
		/// 2021.08.18 by twkang [ADD] 기본알람의 Message Code 설정
		/// </summary>
		private bool SetDefaultMessageCode(int nIndex)
		{
			int nAlarmCode				= -1;
			int nTempAlarmMessageCode	= -1;
			int nDeviceNummber			= -1;

			if(m_InstanceOfAlaramDLL.GetParameter(nIndex, PARAM_LIST.CODE_ALARM, ref nAlarmCode))
			{
				nTempAlarmMessageCode		= nAlarmCode % 100;
				nDeviceNummber				= (nAlarmCode / 100) % 100;

				if(Enum.IsDefined(typeof(AlarmInstance::Alarm_.ALARM_CONTENT), nTempAlarmMessageCode))
                {
                    switch ((AlarmInstance::Alarm_.ALARM_CONTENT)nTempAlarmMessageCode)
                    {
						case AlarmInstance::Alarm_.ALARM_CONTENT.TASK_TIMEOUT_SEQUENCE:
							if (nDeviceNummber != RESERVED_NUMBER)
								return false;
							break;

						default:
							if (nDeviceNummber == RESERVED_NUMBER)
								return false;
							break;
                    }

                    nTempAlarmMessageCode	+=	MESSAGE_CODE_NUMBERING;

					return SetParameter(nIndex, EN_PARAM_ALARM.MESSAGECODE, nTempAlarmMessageCode, false);
				}
			}

			return false;
		}
		/// <summary>
		/// 2020.10.06 by twkang [ADD] 감시자가 업데이트 되면 호출된다.
		/// </summary>
		private void FunctionForTimer(object sender, System.Timers.ElapsedEventArgs e)
		{
			int [] arIndex		= null;
			int [] arItems		= null;

			if (m_InstanceOfAlaramDLL.GetUnknownItem(ref arItems))
			{
				foreach (int nIndex in arItems)
				{
					SaveParameterToStorage(nIndex);
				}
			}

			if (m_InstanceOfAlaramDLL.GetListOfGeneratedAlarm(ref arIndex)
				&& m_nCountOfGeneratedAlarm != arIndex.Length)
			{
				if (m_nCountOfGeneratedAlarm < arIndex.Length)
				{
					List<int> listForCheck		= new List<int>();

					for (int nIndex = 0, nEnd = m_nCountOfGeneratedAlarm; nIndex < nEnd; ++nIndex)
					{
						listForCheck.Add(m_arGeneratedAlarm[nIndex]);
					}

					foreach (int n in arIndex)
					{
						/// MessageForm에 발생시각을 표시해 주기 위함
						if (false == m_DicForGeneratedTime.ContainsKey(n))
						{
							m_DicForGeneratedTime.Add(n, System.DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
						}

						if (false == listForCheck.Contains(n))
						{
							WriteLog(n, EN_ALM_TYPE.OCCURRED);
						}
					}
				}

				m_arGeneratedAlarm			= arIndex;
				m_nCountOfGeneratedAlarm	= arIndex.Length;

				return;
			}

			m_arGeneratedAlarm				= null;
			m_nCountOfGeneratedAlarm		= 0;
        }

        #region Message
        /// <summary>
        /// 2020.10.12 by yjlee [ADD] Add a subinformation to make the message.
        /// </summary>
        public void RemoveSubInformation(ref int nAlarmCode)
        {
            m_rwlockForSubInformation.EnterWriteLock();

            if (m_dicOfSubInformation.ContainsKey(nAlarmCode))
            {
                m_dicOfSubInformation.Remove(nAlarmCode);
            }

            if(m_dicOfAlarmMessage.ContainsKey(nAlarmCode))
            {
                m_dicOfAlarmMessage.Remove(nAlarmCode);
            }

            m_rwlockForSubInformation.ExitWriteLock();
        }
        #endregion End - Message

        #endregion

		public class TaskAlarmInformation
		{
			private static TaskAlarmInformation _Instance	= new TaskAlarmInformation();
			private TaskAlarmInformation(){	}
			public static TaskAlarmInformation GetInstance()
			{
				return _Instance;
			}

			#region Variables
			private Dictionary<string, TaskInformation> m_dicForTaskData	= new Dictionary<string, TaskInformation>();
			System.Threading.ReaderWriterLockSlim m_pLockForTaskInformation	= new System.Threading.ReaderWriterLockSlim();
			#endregion

			#region Internal Interface
			/// <summary>
			/// 2021.05.10 by twkang [ADD] 알람발생시 상태정보를 업데이트한다.
			/// </summary>
			private void UpdateTaskInformation(string strTaskName, AlarmInstance::Alarm_.ALARM_STATE enState, string strRunningAction, int nAlarmOccurredStepNumber)
			{
				var pTaskData = m_dicForTaskData[strTaskName];

				if(pTaskData.m_enAlarmState == enState)
					return;

				pTaskData.m_enAlarmState				= enState;
				pTaskData.m_strRunningAction			= strRunningAction;
				pTaskData.m_nAlarmOccurredStepNumber	= nAlarmOccurredStepNumber;

				m_dicForTaskData[strTaskName]		= pTaskData;
			}
			#endregion

			#region External Interface
			/// <summary>
			/// 2021.05.10 by twkang [ADD] 테스크 알람 정보를 업테이트 한다.
			/// </summary>
			public void UpdateTaskInformation(RunningMain_.TaskAlarmData pTaskData)
			{
				m_pLockForTaskInformation.EnterWriteLock();

				if(false == m_dicForTaskData.ContainsKey(pTaskData.strTaskName))
				{
					m_dicForTaskData.Add(pTaskData.strTaskName, new TaskInformation(pTaskData.strTaskName
						, pTaskData.enAlarmState
						, pTaskData.strAlarmOccurredAction
						, pTaskData.nAlarmOccurredStepNumber));
				}

				UpdateTaskInformation(pTaskData.strTaskName, pTaskData.enAlarmState, pTaskData.strAlarmOccurredAction, pTaskData.nAlarmOccurredStepNumber);

				m_pLockForTaskInformation.ExitWriteLock();
			}
			/// <summary>
			/// 2021.05.10 by twkang [ADD] 테스크 알람 정보
			/// </summary>
			public bool GetTaskInformation(string strTaskName, ref string strRunningAction, ref int nAlarmOccurredStep)
			{
				m_pLockForTaskInformation.EnterReadLock();

				if(false == m_dicForTaskData.ContainsKey(strTaskName))
				{
					m_pLockForTaskInformation.ExitReadLock();

					return false;
				}

				var pTaskInformation	= m_dicForTaskData[strTaskName];

				strRunningAction	= pTaskInformation.m_strRunningAction;

				nAlarmOccurredStep	= pTaskInformation.m_nAlarmOccurredStepNumber;

				m_pLockForTaskInformation.ExitReadLock();

				return true;
			}
			#endregion

			/// <summary>
			/// 2021.05.10 by twkang [ADD] Task Alarm 상태
			/// </summary>
			private struct TaskInformation
			{
				public string m_strTaskName;
				public AlarmInstance::Alarm_.ALARM_STATE m_enAlarmState;
				public int m_nAlarmOccurredStepNumber;
				public string m_strRunningAction;

				public TaskInformation(string strTaskName, AlarmInstance::Alarm_.ALARM_STATE enAlarmState, string strActionName, int nAlarmStepNumber)
				{
					m_strTaskName				= strTaskName;
					m_enAlarmState				= enAlarmState;
					m_strRunningAction			= strActionName;
					m_nAlarmOccurredStepNumber	= nAlarmStepNumber;
				}
			}
		}
    }
}
