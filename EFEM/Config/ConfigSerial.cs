using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO.Ports;

using Serial_;
using FileBorn_;

namespace FrameOfSystem3.Config
{
	public class ConfigSerial
	{
		private ConfigSerial() { }

		#region 싱글톤
		private static ConfigSerial _Instance	= new ConfigSerial();
		public static ConfigSerial GetInstance()
		{
			return _Instance;
		}
		#endregion

		#region 열거형
		public enum EN_PARAM_SERIAL
		{
			BAUDRATE,
			DATABIT,
			ENABLE,
			NAME,
			PARITYBIT,
			PORT,
			RECEIVE_TIMEOUT,
			STOPBIT,
            LOG_TYPE,
            SEND_START_CMD_CHARACTER,      // 2022.03.25. 전송제어문자 파라미터 추가
            SEND_END_CMD_CHARACTER,        // 2022.03.25. 전송제어문자 파라미터 추가
            RECEIVE_START_CMD_CHARACTER,   // 2022.03.25. 전송제어문자 파라미터 추가
            RECEIVE_END_CMD_CHARACTER,     // 2022.03.25. 전송제어문자 파라미터 추가
		}
		public enum EN_SERIAL_STATE
		{
			DISABLED,
			OPENED,
			READY,
			TIMEOUT_RECEIVE,
			WAITING_RECEIVE
		}
		public enum EN_SERIAL_BAUDRATE
		{
			CBR_9600 = 9600,
			CBR_14400 = 14400,
			CBR_19200 = 19200,
			CBR_38400 = 38400,
			CBR_57600 = 57600,
			CBR_115200 = 115200,
		}

        // 2022.03.26. by shkim. [MOD] Serial.dll Enum 순서와 매칭
		public enum EN_SERIAL_STOPBIT
		{
// 			ONE_STOPBIT = 0,
// 			ONE_5STOPBIT = 1,
// 			TWO_STOPBITS = 2,

            NONE = 0,
            ONE_STOPBIT = 1,
            TWO_STOPBITS = 2,
            ONE_5STOPBIT = 3,
		}
		public enum SERIAL_PARITY
		{
			NOPARITY = 0,
			ODDPARITY = 1,
			EVENPARITY = 2,
			MARKPARITY = 3,
			SPACEPARITY = 4,
		}
		public enum EN_SERIAL_BYTESIZE
		{
			BYTE_5 = 5,
			BYTE_6 = 6,
			BYTE_7 = 7,
			BYTE_8 = 8,
		}
		#endregion

		#region 상수
		private const int INITIALIZE_PARAM				= -1;

		private readonly string ITEM_ENABLE 			= "ENABLE";
		private readonly string ITEM_NAME 				= "NAME";
		private readonly string ITEM_PORT 				= "PORT";
		private readonly string ITEM_BAUDRATE 			= "BAUDRATE";
		private readonly string ITEM_DATABIT 			= "DATABIT";
		private readonly string ITEM_STOPBIT 			= "STOPBIT";
		private readonly string ITEM_PARITYBIT 			= "PARITYBIT";
		private readonly string ITEM_RCVTIMEOUT 		= "RCVTIMEOUT";
        private readonly string ITEM_LOG_TYPE 		    = "LOG_TYPE";

        // 2022.03.25. by shkim [ADD] 토큰 관련 파라미터 이름
        private readonly string ITEM_SEND_START_CMD_CHARACTER = "SS_SEND_START_CMD_CHARACTER";
        private readonly string ITEM_SEND_END_CMD_CHARACTER = "SE_SEND_END_CMD_CHARACTER";
        private readonly string ITEM_RECEIVE_START_CMD_CHARACTER = "RS_RECEIVE_START_CMD_CHARACTER";
        private readonly string ITEM_RECEIVE_END_CMD_CHARACTER = "RE_RECEIVE_END_CMD_CHARACTER";

        // 2022.03.25. by shkim [ADD] 토큰 STRING 정의
        private readonly string STRING_NUL = "NUL";
        private readonly string STRING_STX = "STX";
        private readonly string STRING_ETX = "ETX";
        private readonly string STRING_LF = "LF";
        private readonly string STRING_CR = "CR";

		#region Default Value
		private readonly string DEFAULT_NAME			= "NONE";
		private const int DEFAULT_BAUDRATE				= (int)SERIAL_BAUDRATE.CBR_9600;
		private const int DEFAULT_DATABIT				= (int)SERIAL_BYTESIZE.BYTE_8;
		private const bool DEFAULT_ENABLE				= true;
		private const int DEFAULT_PARITYBIT				= (int)SERIAL_PARITY.NOPARITY;
		private const int DEFAULT_PORT					= -1;
		private	const int DEFAULT_RCVTIMEOUT			= 1000;
		private const int DEFAULT_STOPBIT				= (int)SERIAL_STOPBIT.ONE_STOPBIT;
        private const int DEFAULT_LOG_TYPE				= (int)LOG_TYPE.DISABLE;
		#endregion

		#endregion

		#region 변수
		Dictionary<EN_PARAM_SERIAL, PARAM_LIST> m_DicForItem			= new Dictionary<EN_PARAM_SERIAL,PARAM_LIST>();
		Dictionary<EN_PARAM_SERIAL, string> m_DicForStorage				= new Dictionary<EN_PARAM_SERIAL,string>();
		Dictionary<SERIAL_ITEM_STATE, EN_SERIAL_STATE> m_DicForState	= new Dictionary<SERIAL_ITEM_STATE,EN_SERIAL_STATE>();

		Dictionary<int, string> m_DicForBaudrate						= new Dictionary<int,string>();
		Dictionary<int, string> m_DicForStopBit							= new Dictionary<int,string>();
		Dictionary<int, string> m_DicForParitybit						= new Dictionary<int,string>();
		Dictionary<int, string> m_DicForDataBit							= new Dictionary<int,string>();
        Dictionary<int, string> m_DicForLogType							= new Dictionary<int,string>();

		Dictionary<string, int> m_DicForResult							= new Dictionary<string,int>();


        // 2022.03.26. by shkim [ADD] 토큰 맵핑
        Dictionary<int, string> m_DicStringTypeTokenMap = new Dictionary<int, string>();
        Dictionary<int, string> m_DicForToken = new Dictionary<int, string>();



        Serial m_InstanceOfSerialDLL = null;
		Functional.Storage m_InstanceOfStorage		= null;
		#endregion

		#region 외부인터페이스
		/// <summary>
		/// 2020.06.16 by twkang [ADD] Serial 클래스를 초기화한다.
		/// </summary>
		public bool Init()
		{
			MakeMappingTable();

			m_InstanceOfSerialDLL			= Serial.GetInstance();
			m_InstanceOfStorage				= Functional.Storage.GetInstance();

			if(false == m_InstanceOfStorage.Load(Define.DefineEnumBase.Storage.EN_STORAGE_LIST.SERIAL))
			{
				return false;
			}
			
			InitializeSerialParameter();

			return true;
		}
		/// <summary>
		/// 2020.06.16 by twkang [ADD] Serial 포트를 오픈한다.
		/// </summary>
		public bool Open(int nIndexOfItem)
		{
            if (m_InstanceOfSerialDLL == null)
                return false;
			return m_InstanceOfSerialDLL.Open(nIndexOfItem);
		}
		/// <summary>
		/// 2020.06.16 by twkang [ADD] Serial 포트를 닫는다.
		/// </summary>
		public void Close(int nIndexOfItem)
		{
			m_InstanceOfSerialDLL.Close(nIndexOfItem);
		}
		/// <summary>
		/// 2020.11.25 by twkang [ADD] Serial 포트가 오픈 상태인지 확인한다.
		/// </summary>
		public bool IsOpen(int nIndexOfItem)
		{
			if (nIndexOfItem < 0) { return false; }
            if (m_InstanceOfSerialDLL == null)
                return false;
			var enState	= m_InstanceOfSerialDLL.GetState(nIndexOfItem);
			
			bool bOpen	= false;

			switch(enState)
			{
				case SERIAL_ITEM_STATE.DISABLED:
				case SERIAL_ITEM_STATE.READY:
					bOpen	 =false;
					break;
				default:
					bOpen	= true;
					break;
			}

			return bOpen;
		}
		/// <summary>
		/// 2020.11.25 by twkang [ADD] TimeOut 인지 확인한다.
		/// </summary>
		public bool IsReceiveTimeOut(int nIndexOfItem)
		{
			return SERIAL_ITEM_STATE.TIMEOUT_RECEIVE == m_InstanceOfSerialDLL.GetState(nIndexOfItem);
		}
		/// <summary>
		/// 2020.06.16 by twkang [ADD] Serial 파라미터 값을 변경한다.
		/// </summary>
		public bool SetParameter<T>(int nIndexOfItem, EN_PARAM_SERIAL enList, T tValue)
		{
			string[] strArrGroup	= new string[]{nIndexOfItem.ToString()};

			if(false == m_InstanceOfStorage.SetParameter(Define.DefineEnumBase.Storage.EN_STORAGE_LIST.SERIAL
				, m_DicForStorage[enList]
				, tValue
				, ref strArrGroup))
			{
				return false;
			}
			
			switch(enList)
			{
                // 2022.03.25. by shkim. [ADD] 토큰 파라미터는 다른 함수를 사용
                case EN_PARAM_SERIAL.SEND_START_CMD_CHARACTER:
                case EN_PARAM_SERIAL.SEND_END_CMD_CHARACTER:
                case EN_PARAM_SERIAL.RECEIVE_START_CMD_CHARACTER:
                case EN_PARAM_SERIAL.RECEIVE_END_CMD_CHARACTER:
                    return false;

				case EN_PARAM_SERIAL.PORT:
					return m_InstanceOfSerialDLL.SetParameter(nIndexOfItem, m_DicForItem[enList], tValue.ToString());
				case EN_PARAM_SERIAL.BAUDRATE:
				case EN_PARAM_SERIAL.DATABIT:					
				case EN_PARAM_SERIAL.PARITYBIT:
				case EN_PARAM_SERIAL.STOPBIT:
                case EN_PARAM_SERIAL.LOG_TYPE:
					if(false == m_DicForResult.ContainsKey(tValue.ToString()))
					{
						return false;
					}
					return m_InstanceOfSerialDLL.SetParameter(nIndexOfItem, m_DicForItem[enList], m_DicForResult[tValue.ToString()]);					
			}

			return m_InstanceOfSerialDLL.SetParameter(nIndexOfItem, m_DicForItem[enList], tValue);
		}
		/// <summary>
		/// 2020.06.16 by twkang [ADD] Serial 파라미터 값을 반환한다.
		/// </summary>
		public bool GetParameter<T>(int nIndexOfItem, EN_PARAM_SERIAL enList, ref T tValue)
		{
            // 2022.03.25. by shkim. [ADD] 토큰 파라미터 읽기
            switch (enList)
            {
                case EN_PARAM_SERIAL.SEND_START_CMD_CHARACTER:
                case EN_PARAM_SERIAL.SEND_END_CMD_CHARACTER:
                case EN_PARAM_SERIAL.RECEIVE_START_CMD_CHARACTER:
                case EN_PARAM_SERIAL.RECEIVE_END_CMD_CHARACTER:
                    int[] arParam = null;
                    if (false == typeof(T).Equals(typeof(string))) return false;

                    if(m_InstanceOfSerialDLL.GetParameter(nIndexOfItem, m_DicForItem[enList], ref arParam))
                    {
                        string strTokenValue = string.Empty;
                        if (arParam == null)
                        {
                            strTokenValue = string.Empty;
                        }
                        else
                        {
                            string[] strArrParam = (from n in arParam
                                                    from d in m_DicStringTypeTokenMap
                                                    where d.Key == n
                                                    select d.Value).Cast<string>().ToArray();

                            strTokenValue = (arParam == null) ? string.Empty : string.Join(", ", strArrParam);
                        }
                        tValue = (T)Convert.ChangeType(strTokenValue, typeof(T));
                        return true;
                    }
                    return false;
            }

			return m_InstanceOfSerialDLL.GetParameter(nIndexOfItem, m_DicForItem[enList], ref tValue);
		}

        // 2022.03.25. by shkim. [ADD] 토큰 관련 파라미터 설정
        public bool SetParameter(int nIndexOfItem, EN_PARAM_SERIAL enList, ref int[] arParam)
        {
            string strGroupName		= string.Empty;
			string[] arGroup		= null;

            // string strValue = nCount == 0 ? "-1" : string.Join(", ", arParam);
            string strValue = (arParam == null) ? string.Empty : string.Join(", ", arParam);

            switch (enList)
            {
                case EN_PARAM_SERIAL.SEND_START_CMD_CHARACTER:
                    arGroup = new string[] { nIndexOfItem.ToString() };
                    strGroupName = ITEM_SEND_START_CMD_CHARACTER;
                    break;

                case EN_PARAM_SERIAL.SEND_END_CMD_CHARACTER:
                    arGroup = new string[] { nIndexOfItem.ToString() };
                    strGroupName = ITEM_SEND_END_CMD_CHARACTER;
                    break;

                case EN_PARAM_SERIAL.RECEIVE_START_CMD_CHARACTER:
                    arGroup = new string[] { nIndexOfItem.ToString() };
                    strGroupName = ITEM_RECEIVE_START_CMD_CHARACTER;
                    break;

                case EN_PARAM_SERIAL.RECEIVE_END_CMD_CHARACTER:
                    arGroup = new string[] { nIndexOfItem.ToString() };
                    strGroupName = ITEM_RECEIVE_END_CMD_CHARACTER;
                    break;

                default:
                    return false;
            }
             if (m_InstanceOfStorage.SetParameter(Define.DefineEnumBase.Storage.EN_STORAGE_LIST.SERIAL
                 , strGroupName
                 , strValue
                 , ref arGroup))
             {
                 return m_InstanceOfSerialDLL.SetParameter(nIndexOfItem, m_DicForItem[enList], ref arParam);
             }
             return false;
        }
        
		/// <summary>
		/// 2020.06.16 by twkang [ADD] 아이템의 상태를 반환한다.
		/// </summary>
		public EN_SERIAL_STATE GetState(int nIndexOfItem)
		{			
			return m_DicForState[m_InstanceOfSerialDLL.GetState(nIndexOfItem)];
		}
		/// <summary>
		/// 2020.06.17 by twkang [ADD] 메세지를 송신한다.
		/// </summary>
		public bool Write(int nIndexOfItem, string strData)
		{
			return m_InstanceOfSerialDLL.Write(nIndexOfItem, strData);
		}
		/// <summary>
		/// 2020.11.24 by twkang [ADD] 메세지를 송신한다.
		/// </summary>
		public bool Write(int nIndexOfItem, byte[] arData)
		{
			return m_InstanceOfSerialDLL.Write(nIndexOfItem, arData);
		}
		/// <summary>
		/// 2020.06.17 by twkang [ADD] 메세지를 수신한다.
		/// </summary>
		public bool Read(int nIndexOfItem, ref string strData)
		{			
			return m_InstanceOfSerialDLL.Read(nIndexOfItem, ref strData);
		}
        public bool Read(int nIndexOfItem, ref byte[] arData)
        {
            return m_InstanceOfSerialDLL.Read(nIndexOfItem, ref arData);
        }
		/// <summary>
		/// 2020.06.29 by twkang [ADD] Serial Item 을 추가한다.
		/// </summary>
		public int AddSerialItem()
		{
			int nItemNum		= m_InstanceOfSerialDLL.AddItem();
			SetDefaultValue(nItemNum);

			string[] arGroup	= null;
			string[] arData		= null;

			if(FileBorn.GetInstance().GetItemFrame(BORN_LIST.SERIAL, ref arGroup, nItemNum.ToString(), ref arData)
				&& m_InstanceOfStorage.AddGroupAndItem(Define.DefineEnumBase.Storage.EN_STORAGE_LIST.SERIAL, ref arData))
			{
				SaveParamToStorage(nItemNum);
				return nItemNum;
			}

			m_InstanceOfSerialDLL.RemoveItem(nItemNum);
			return -1;
		}
		/// <summary>
		/// 2022.07.20 by junho [ADD] List에 있는 항목들로 아이템을 추가한다.
		/// </summary>
		public bool AddSerialItemArray(Dictionary<int, string> itemList)
		{
			foreach (KeyValuePair<int, string> kvp in itemList)
			{
				int nItemNum = m_InstanceOfSerialDLL.AddItem();
				SetDefaultValue(nItemNum);

				m_InstanceOfSerialDLL.SetParameter(nItemNum, PARAM_LIST.NAME, kvp.Value);
				m_InstanceOfSerialDLL.SetParameter(nItemNum, PARAM_LIST.PORT, kvp.Key);

				string[] arGroup = null;
				string[] arData = null;

				if (FileBorn.GetInstance().GetItemFrame(BORN_LIST.SERIAL, ref arGroup, nItemNum.ToString(), ref arData)
					&& m_InstanceOfStorage.AddGroupAndItem(Define.DefineEnumBase.Storage.EN_STORAGE_LIST.SERIAL, ref arData))
				{
					SaveParamToStorage(nItemNum);
					continue;
				}

				m_InstanceOfSerialDLL.RemoveItem(nItemNum);
				return false;
			}
			return true;
		}
		/// <summary>
		/// 2020.06.16 by twkang [ADD] 아이템을 삭제한다.
		/// </summary>
		public bool RemoveItem(int nIndexOfItem)
		{
			string[] strArrGroup	= new string[] {nIndexOfItem.ToString()};

			if(m_InstanceOfStorage.RemoveGroup(Define.DefineEnumBase.Storage.EN_STORAGE_LIST.SERIAL, ref strArrGroup))
			{
				return m_InstanceOfSerialDLL.RemoveItem(nIndexOfItem);
			}

			return false;
		}
		/// <summary>
		/// 2020.06.16 by twkang [ADD] Serial Item 의 수를 반환한다.
		/// </summary>
		public bool GetListOfItem(ref int[] nArrItem)
		{
			return m_InstanceOfSerialDLL.GetListOfItem(ref nArrItem);
		}
		/// <summary>
		/// 2020.06.16 by twkang [ADD] 현재 연결된 포트 리스트를 가져온다.
		/// </summary>
		public bool GetListOfPortName(ref string[] arList)
		{
			arList = SerialPort.GetPortNames();

			if (arList.Length < 1) { return false; }

			return true;
		}
        /// <summary>
        /// 2021.01.20 by yjlee [ADD] Convert the type from the integer to the string.
        /// </summary>
        public string ConvertIntToStringLogType(int nLogType)
        {
            if(m_DicForLogType.ContainsKey(nLogType))
            {
                return m_DicForLogType[nLogType];
            }

            return Define.DefineConstant.Common.NONE;
        }
		#endregion

		#region 내부인터페이스
		/// <summary>
		/// 2020.06.16 by twkang [ADD] Storage 데이터로 Serial 파라미터를 초기화한다.
		/// </summary>
		private void InitializeSerialParameter()
		{
			string strValue			= string.Empty;			

			string[] strArrGroup	= null;

			if(m_InstanceOfStorage.GetListOfGroup(Define.DefineEnumBase.Storage.EN_STORAGE_LIST.SERIAL, ref strArrGroup))
            {
                for(int nIndex = 0, nEnd = strArrGroup.Length; nIndex < nEnd; ++nIndex)
			    {
			    	string[] arGroup	= new string[1] {strArrGroup[nIndex]};

					int nIndexOfItem	= int.Parse(strArrGroup[nIndex]);
					m_InstanceOfSerialDLL.AddItem(nIndexOfItem);
					SetDefaultValue(nIndexOfItem);

                    foreach (EN_PARAM_SERIAL en in Enum.GetValues(typeof(EN_PARAM_SERIAL)))
                    {
                        if (m_InstanceOfStorage.GetParameter(Define.DefineEnumBase.Storage.EN_STORAGE_LIST.SERIAL
                            , m_DicForStorage[en]
                            , ref strValue
                            , ref arGroup))
                        {
                            switch (en)
                            {
								// 2022.03.26. by shkim [ADD] 토큰 관련 파라미터 초기화
                                case EN_PARAM_SERIAL.SEND_START_CMD_CHARACTER:
                                case EN_PARAM_SERIAL.SEND_END_CMD_CHARACTER:
                                case EN_PARAM_SERIAL.RECEIVE_START_CMD_CHARACTER:
                                case EN_PARAM_SERIAL.RECEIVE_END_CMD_CHARACTER:
                                    string[] strArrValue = strValue.Split(new string[] { ", " }, StringSplitOptions.None);
                                    int[] nArrValue = null;

                                    if (strArrValue != null && strArrValue.Length >= 1 && false == string.IsNullOrEmpty(strArrValue[0]))
                                    {
                                        nArrValue = new int[strArrValue.Length];
                                        for (int i = 0; i < strArrValue.Length; i++)
                                        {
                                            int.TryParse(strArrValue[i], out nArrValue[i]);
                                        }
                                    }
                                    SetParameter(nIndexOfItem, en, ref nArrValue);
                                    break;

                                case EN_PARAM_SERIAL.BAUDRATE:
                                case EN_PARAM_SERIAL.DATABIT:
                                case EN_PARAM_SERIAL.STOPBIT:
                                case EN_PARAM_SERIAL.PARITYBIT:
                                case EN_PARAM_SERIAL.LOG_TYPE:
                                    if (m_DicForResult.ContainsKey(strValue))
                                    {
                                        m_InstanceOfSerialDLL.SetParameter(nIndex, m_DicForItem[en], m_DicForResult[strValue]);
                                    }
                                    break;
                                case EN_PARAM_SERIAL.PORT:
                                case EN_PARAM_SERIAL.RECEIVE_TIMEOUT:
                                case EN_PARAM_SERIAL.ENABLE:
                                case EN_PARAM_SERIAL.NAME:
                                    m_InstanceOfSerialDLL.SetParameter(nIndex, m_DicForItem[en], strValue);
                                    break;
                            }
                        }					
			    	}
			    }
            }
		}
		/// <summary>
		/// 2020.06.29 by twkang [ADD] 클래스에서 사용할 Mapping Table 을 만든다.
		/// </summary>
		private void MakeMappingTable()
		{
			#region Parameter List
			m_DicForItem.Clear();
			m_DicForItem.Add(EN_PARAM_SERIAL.BAUDRATE, PARAM_LIST.BAUDRATE);
			m_DicForItem.Add(EN_PARAM_SERIAL.DATABIT, PARAM_LIST.DATABIT);
			m_DicForItem.Add(EN_PARAM_SERIAL.ENABLE, PARAM_LIST.ENABLE);
			m_DicForItem.Add(EN_PARAM_SERIAL.NAME, PARAM_LIST.NAME);
			m_DicForItem.Add(EN_PARAM_SERIAL.PARITYBIT, PARAM_LIST.PARITYBIT);
			m_DicForItem.Add(EN_PARAM_SERIAL.PORT, PARAM_LIST.PORT);
			m_DicForItem.Add(EN_PARAM_SERIAL.RECEIVE_TIMEOUT, PARAM_LIST.RECEIVE_TIMEOUT);
			m_DicForItem.Add(EN_PARAM_SERIAL.STOPBIT, PARAM_LIST.STOPBIT);
            m_DicForItem.Add(EN_PARAM_SERIAL.LOG_TYPE, PARAM_LIST.LOG_TYPE);

            // 2022.03.26. by shkim. [ADD] 토큰 관련 파라미터 아이템 추가
            m_DicForItem.Add(EN_PARAM_SERIAL.SEND_START_CMD_CHARACTER, PARAM_LIST.SEND_START_CMD_CHARACTER);
            m_DicForItem.Add(EN_PARAM_SERIAL.SEND_END_CMD_CHARACTER, PARAM_LIST.SEND_END_CMD_CHARACTER);
            m_DicForItem.Add(EN_PARAM_SERIAL.RECEIVE_START_CMD_CHARACTER, PARAM_LIST.RECEIVE_START_CMD_CHARACTER);
            m_DicForItem.Add(EN_PARAM_SERIAL.RECEIVE_END_CMD_CHARACTER, PARAM_LIST.RECEIVE_END_CMD_CHARACTER);
			#endregion

			#region Storage
			m_DicForStorage.Clear();
			m_DicForStorage.Add(EN_PARAM_SERIAL.BAUDRATE, ITEM_BAUDRATE);
			m_DicForStorage.Add(EN_PARAM_SERIAL.DATABIT, ITEM_DATABIT);
			m_DicForStorage.Add(EN_PARAM_SERIAL.ENABLE, ITEM_ENABLE);
			m_DicForStorage.Add(EN_PARAM_SERIAL.NAME, ITEM_NAME);
			m_DicForStorage.Add(EN_PARAM_SERIAL.PARITYBIT, ITEM_PARITYBIT);
			m_DicForStorage.Add(EN_PARAM_SERIAL.PORT, ITEM_PORT);
			m_DicForStorage.Add(EN_PARAM_SERIAL.RECEIVE_TIMEOUT, ITEM_RCVTIMEOUT);
			m_DicForStorage.Add(EN_PARAM_SERIAL.STOPBIT, ITEM_STOPBIT);
            m_DicForStorage.Add(EN_PARAM_SERIAL.LOG_TYPE, ITEM_LOG_TYPE);

            // 2022.03.26. by shkim. [ADD] 토큰 관련 파라미터 아이템 추가
            m_DicForStorage.Add(EN_PARAM_SERIAL.SEND_START_CMD_CHARACTER, ITEM_SEND_START_CMD_CHARACTER);
            m_DicForStorage.Add(EN_PARAM_SERIAL.SEND_END_CMD_CHARACTER, ITEM_SEND_END_CMD_CHARACTER);
            m_DicForStorage.Add(EN_PARAM_SERIAL.RECEIVE_START_CMD_CHARACTER, ITEM_RECEIVE_START_CMD_CHARACTER);
            m_DicForStorage.Add(EN_PARAM_SERIAL.RECEIVE_END_CMD_CHARACTER, ITEM_RECEIVE_END_CMD_CHARACTER);

			#endregion

			#region State
			m_DicForState.Clear();
			m_DicForState.Add(SERIAL_ITEM_STATE.DISABLED, EN_SERIAL_STATE.DISABLED);
			m_DicForState.Add(SERIAL_ITEM_STATE.OPENED, EN_SERIAL_STATE.OPENED);
			m_DicForState.Add(SERIAL_ITEM_STATE.READY, EN_SERIAL_STATE.READY);
			m_DicForState.Add(SERIAL_ITEM_STATE.TIMEOUT_RECEIVE, EN_SERIAL_STATE.TIMEOUT_RECEIVE);
			m_DicForState.Add(SERIAL_ITEM_STATE.WAITING_RECEIVE, EN_SERIAL_STATE.WAITING_RECEIVE);
			#endregion

			#region Result
			m_DicForResult.Clear();
			m_DicForBaudrate.Clear();
			m_DicForDataBit.Clear();
			m_DicForParitybit.Clear();
			m_DicForStopBit.Clear();
			foreach (SERIAL_BAUDRATE en in Enum.GetValues(typeof(SERIAL_BAUDRATE)))
			{
				m_DicForResult.Add(en.ToString(), (int)en);
				m_DicForBaudrate.Add((int)en, en.ToString());
			}
			foreach (SERIAL_BYTESIZE en in Enum.GetValues(typeof(SERIAL_BYTESIZE)))
			{
				m_DicForResult.Add(en.ToString(), (int)en);
				m_DicForDataBit.Add((int)en, en.ToString());
			}
			foreach (SERIAL_PARITY en in Enum.GetValues(typeof(SERIAL_PARITY)))
			{
				m_DicForResult.Add(en.ToString(), (int)en);
				m_DicForParitybit.Add((int)en, en.ToString());
			}
			foreach (SERIAL_STOPBIT en in Enum.GetValues(typeof(SERIAL_STOPBIT)))
			{
				m_DicForResult.Add(en.ToString(), (int)en);
				m_DicForStopBit.Add((int)en, en.ToString());
			}
            foreach (LOG_TYPE en in Enum.GetValues(typeof(LOG_TYPE)))
            {
                m_DicForResult.Add(en.ToString(), (int)en);
                m_DicForLogType.Add((int)en, en.ToString());
            }
			#endregion

            // 2022.03.26. by shkim [ADD] 토큰처리를 위한 Mapping Table을 만든다.
            m_DicStringTypeTokenMap.Add((int)COM_CONTROL_CHARACTER.NUL, STRING_NUL);
            m_DicStringTypeTokenMap.Add((int)COM_CONTROL_CHARACTER.STX, STRING_STX);
            m_DicStringTypeTokenMap.Add((int)COM_CONTROL_CHARACTER.ETX, STRING_ETX);
            m_DicStringTypeTokenMap.Add((int)COM_CONTROL_CHARACTER.LF, STRING_LF);
            m_DicStringTypeTokenMap.Add((int)COM_CONTROL_CHARACTER.CR, STRING_CR);
		}
		/// <summary>
		/// 2020.06.29 by twkang [ADD] 파라미터 값을 Storage 데이터로 저장한다.
		/// </summary>
		private bool SaveParamToStorage(int nIndexOfItem)
		{
			int nValue			= -1;
			string strValue		= string.Empty;
			bool bValue			= false;
			string[] arGroup	= new string[] {nIndexOfItem.ToString()};
			foreach(EN_PARAM_SERIAL en in Enum.GetValues(typeof(EN_PARAM_SERIAL)))
			{
				switch(en)
				{
					case EN_PARAM_SERIAL.BAUDRATE:
						m_InstanceOfSerialDLL.GetParameter(nIndexOfItem, m_DicForItem[en], ref nValue);
						strValue			= m_DicForBaudrate.ContainsKey(nValue) ? m_DicForBaudrate[nValue] : Define.DefineConstant.Common.NONE;
						break;
					case EN_PARAM_SERIAL.DATABIT:
						m_InstanceOfSerialDLL.GetParameter(nIndexOfItem, m_DicForItem[en], ref nValue);
						strValue			= m_DicForDataBit.ContainsKey(nValue) ? m_DicForDataBit[nValue] : Define.DefineConstant.Common.NONE;
						break;
					case EN_PARAM_SERIAL.PARITYBIT:
						m_InstanceOfSerialDLL.GetParameter(nIndexOfItem, m_DicForItem[en], ref nValue);
						strValue			= m_DicForParitybit.ContainsKey(nValue) ? m_DicForParitybit[nValue] : Define.DefineConstant.Common.NONE;
						break;	
					case EN_PARAM_SERIAL.STOPBIT:
						m_InstanceOfSerialDLL.GetParameter(nIndexOfItem, m_DicForItem[en], ref nValue);
						strValue			= m_DicForStopBit.ContainsKey(nValue) ? m_DicForStopBit[nValue] : Define.DefineConstant.Common.NONE;
						break;
					case EN_PARAM_SERIAL.ENABLE:
						m_InstanceOfSerialDLL.GetParameter(nIndexOfItem, m_DicForItem[en], ref bValue);
						strValue			= bValue.ToString();
						break;
					case EN_PARAM_SERIAL.PORT:
					case EN_PARAM_SERIAL.RECEIVE_TIMEOUT:
					case EN_PARAM_SERIAL.NAME:
						m_InstanceOfSerialDLL.GetParameter(nIndexOfItem, m_DicForItem[en], ref strValue);
						if(string.IsNullOrEmpty(strValue))
						{
							strValue		= Define.DefineConstant.Common.NONE;
						}
						break;

					// 2022.03.26. by shkim. [ADD] 토큰 관련 파라미터 저장
                    case EN_PARAM_SERIAL.SEND_START_CMD_CHARACTER:
                    case EN_PARAM_SERIAL.SEND_END_CMD_CHARACTER:
                    case EN_PARAM_SERIAL.RECEIVE_START_CMD_CHARACTER:
                    case EN_PARAM_SERIAL.RECEIVE_END_CMD_CHARACTER:
                        int[] arParam = null;
                        if (m_InstanceOfSerialDLL.GetParameter(nIndexOfItem, m_DicForItem[en], ref arParam))
                        {
                            string strTokenValue = string.Empty;
                            if (arParam == null)
                            {
                                strTokenValue = string.Empty;
                            }
                            else
                            {
                                string[] strArrParam = (from n in arParam
                                                        from d in m_DicStringTypeTokenMap
                                                        where d.Key == n
                                                        select d.Value).Cast<string>().ToArray();

                                strTokenValue = (arParam == null) ? string.Empty : string.Join(", ", strArrParam);
                            }
                            strValue = strTokenValue;
                        }
                        break;
				}
				m_InstanceOfStorage.SetParameter(Define.DefineEnumBase.Storage.EN_STORAGE_LIST.SERIAL, m_DicForStorage[en], strValue, ref arGroup, false);
			}
			m_InstanceOfStorage.Save(Define.DefineEnumBase.Storage.EN_STORAGE_LIST.SERIAL);
			return true;
		}
		/// <summary>
		/// 2020.10.05 by twkang [ADD] 해당 아이템을 Default 값으로 설정한다.
		/// </summary>
		private void SetDefaultValue(int nIndex)
		{
			m_InstanceOfSerialDLL.SetParameter(nIndex, PARAM_LIST.BAUDRATE, DEFAULT_BAUDRATE);
			m_InstanceOfSerialDLL.SetParameter(nIndex, PARAM_LIST.DATABIT, DEFAULT_DATABIT);
			m_InstanceOfSerialDLL.SetParameter(nIndex, PARAM_LIST.ENABLE, DEFAULT_ENABLE);
			m_InstanceOfSerialDLL.SetParameter(nIndex, PARAM_LIST.NAME, DEFAULT_NAME + nIndex.ToString());
			m_InstanceOfSerialDLL.SetParameter(nIndex, PARAM_LIST.PARITYBIT, DEFAULT_PARITYBIT);
			m_InstanceOfSerialDLL.SetParameter(nIndex, PARAM_LIST.PORT, DEFAULT_PORT);
			m_InstanceOfSerialDLL.SetParameter(nIndex, PARAM_LIST.RECEIVE_TIMEOUT, DEFAULT_RCVTIMEOUT);
			m_InstanceOfSerialDLL.SetParameter(nIndex, PARAM_LIST.STOPBIT, DEFAULT_STOPBIT);
            m_InstanceOfSerialDLL.SetParameter(nIndex, PARAM_LIST.LOG_TYPE, DEFAULT_LOG_TYPE);
		}
		#endregion
	}
}
