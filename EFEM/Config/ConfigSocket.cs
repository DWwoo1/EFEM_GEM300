using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Socket_;
using FileBorn_;

namespace FrameOfSystem3.Config
{
	public class ConfigSocket
	{
		private ConfigSocket() { }

		#region 열거형
		public enum EN_PARAM_SOCKET
		{
			ENABLE,
			NAME,
			PORT,
			PROTOCOL_MODE,
			RECEIVE_TIMEOUT,
			RETRY_INTERVAL,
			TARGET_IP,
			TARGET_PORT,
            LOG_TYPE,
		}
		public enum EN_SOCKET_STATE
		{
			CONNECTED,
			DISABLED,
			DISCONNECTED,
			READY,
			TIMEOUT_RECEIVE,
			WAITING_CONNECTION,
			WAITING_RECEIVE,
		}
		#endregion
		
		#region 싱글톤
		private static ConfigSocket _Instance			= new ConfigSocket();
		public static ConfigSocket GetInstance()
		{
			return _Instance;
		}
		#endregion

		#region 상수
		private const int INITIALIZE_PARAM				= -1;

		private readonly string ITEM_ENABLE				= "ENABLE";
		private readonly string ITEM_NAME				= "NAME";
		private readonly string ITEM_PORT				= "PORT";
		private readonly string ITEM_PROTOCOL_TYPE		= "PROTOCOL";		
		private readonly string ITEM_RECEIVE_TIMEOUT	= "RCVTIMEOUT";
		private readonly string ITEM_RETRY_INTERVAL		= "RETRYINTERVAL";
		private readonly string ITEM_TARGET_IP			= "SERVERIP";
		private readonly string ITEM_TARGET_PORT		= "TARGETPORT";
        private readonly string ITEM_LOG_TYPE		    = "LOG_TYPE";

		#region Default Value
		private const bool DEFAULT_ENABLE				= true;
		private readonly string DEFAULT_NAME			= "NONE";
		private const int DEFAULT_PORT					= -1;
		private const int DEFAULT_PROTOCOL				= (int)PROTOCOL_TYPE.TCP_CLIENT;
		private const int DEFAULT_TIMEOUT				= 1000;
		private const int DEFAULT_RETRY_INTERVAL		= 500;
		private readonly string DEFAULT_IP				= "127.0.0.1";
		private const int DEFAULT_TARGETPORT			= -1;
        private const int DEFAULT_LOGTYPE				= (int)LOG_TYPE.DISABLE;
		#endregion

		#endregion

		#region 변수
		Dictionary<EN_PARAM_SOCKET, PARAM_LIST> m_DicForItem			= new Dictionary<EN_PARAM_SOCKET,PARAM_LIST>();
		Dictionary<EN_PARAM_SOCKET, string> m_DicForStorage				= new Dictionary<EN_PARAM_SOCKET,string>();
		Dictionary<SOCKET_ITEM_STATE, EN_SOCKET_STATE> m_DicForState	= new Dictionary<SOCKET_ITEM_STATE,EN_SOCKET_STATE>();

		Dictionary<string, int> m_DicForProtocolName					= new Dictionary<string,int>();
        Dictionary<string, int> m_DicForLogTypeName					    = new Dictionary<string,int>();

		Dictionary<int, string> m_DicForProtocolType					= new Dictionary<int,string>();
        Dictionary<int, string> m_DicForLogType					        = new Dictionary<int,string>();

		Socket m_InstanceOfSocketDLL									= null;
		Functional.Storage m_InstanceOfStorage							= null;
		#endregion

		#region 외부인터페이스
		/// <summary>
		/// 2020.06.15 by twkang [ADD] Socket 클래스를 초기화한다.
		/// </summary>
		public bool Init()
		{
			MakeMappingTable();

			m_InstanceOfSocketDLL	= Socket.GetInstance();
			m_InstanceOfStorage		= Functional.Storage.GetInstance();

			if(false == m_InstanceOfStorage.Load(Define.DefineEnumBase.Storage.EN_STORAGE_LIST.SOCKET))
			{
				return false;
			}
			
			InitializeSocketParameter();

			return true;
		}
		/// <summary>
		/// 2020.06.15 by twkang [ADD] Socket 파라미터 값을 변경한다.
		/// </summary>
		public bool SetParameter<T>(int nIndexOfItem, EN_PARAM_SOCKET enList, T tValue)
		{
			string[] strArrGroup = new string[]{nIndexOfItem.ToString()};

			if(false == m_InstanceOfStorage.SetParameter(Define.DefineEnumBase.Storage.EN_STORAGE_LIST.SOCKET
				, m_DicForStorage[enList]
				, tValue
				, ref strArrGroup))
			{
				return false;
			}

            string strValue     = tValue.ToString();

			switch(enList)
			{
				case EN_PARAM_SOCKET.PROTOCOL_MODE:
					if(m_DicForProtocolName.ContainsKey(strValue))
					{
						return m_InstanceOfSocketDLL.SetParameter(nIndexOfItem, m_DicForItem[enList], m_DicForProtocolName[strValue]);
					}
					break;

                case EN_PARAM_SOCKET.LOG_TYPE:
                    if (m_DicForLogTypeName.ContainsKey(strValue))
                    {
                        return m_InstanceOfSocketDLL.SetParameter(nIndexOfItem, m_DicForItem[enList], m_DicForLogTypeName[strValue]);
                    }
                    break;

				default:
					return m_InstanceOfSocketDLL.SetParameter(nIndexOfItem, m_DicForItem[enList], strValue);
			}
			return false;
		}
		/// <summary>
		/// 2020.06.15 by twkang [ADD] Socket 파라미터 값을 반환한다.
		/// </summary>
		public bool GetParameter<T>(int nIndexOfItem, EN_PARAM_SOCKET enList, ref T tValue)
		{
			if(m_DicForItem.ContainsKey(enList))
			{
                return m_InstanceOfSocketDLL.GetParameter(nIndexOfItem, m_DicForItem[enList], ref tValue);
			}
			
			return false;
		}
		/// <summary>
		/// 2020.06.15 by twkang [ADD] 아이템의 상태를 반환한다.
		/// </summary>
		public EN_SOCKET_STATE GetState(int nIndexOfItem)
		{
			return m_DicForState[m_InstanceOfSocketDLL.GetState(nIndexOfItem)];
		}		
		/// <summary>
		/// 2020.06.29 by twkang [ADD] 아이템을 추가한다.
		/// </summary>
		public int AddSocketItem()
		{
			int nItemNum	= m_InstanceOfSocketDLL.AddItem();
			SetDefaultValue(nItemNum);

			string[] arGroup		= null;
			string[] arData			= null;
			if(FileBorn.GetInstance().GetItemFrame(BORN_LIST.SOCKET, ref arGroup, nItemNum.ToString(), ref arData)
				&& m_InstanceOfStorage.AddGroupAndItem(Define.DefineEnumBase.Storage.EN_STORAGE_LIST.SOCKET, ref arData))
			{
				SaveParamToStorage(nItemNum);
				return nItemNum;
			}

			m_InstanceOfSocketDLL.RemoveItem(nItemNum);
			return -1;
		}
		/// <summary>
		/// 2020.06.15 by twkang [ADD] 아이템을 삭제한다.
		/// </summary>
		public bool RemoveItem(int nIndexOfItem)
		{
			string[] strArrGroup	= new string[] {nIndexOfItem.ToString()};
			
			if(m_InstanceOfStorage.RemoveGroup(Define.DefineEnumBase.Storage.EN_STORAGE_LIST.SOCKET
				, ref strArrGroup))
			{
				return m_InstanceOfSocketDLL.RemoveItem(nIndexOfItem);
			}
			return false;
		}
		/// <summary>
		/// 2020.06.15 by twkang [ADD] 아이템의 리스트를 반환한다.
		/// </summary>
		public bool GetListOfItem(ref int[] arIndexes)
		{
			return	m_InstanceOfSocketDLL.GetListOfItem(ref arIndexes);
		}
		/// <summary>
		/// 2020.06.15 by twkang [ADD] 해당아이템을 연결한다.
		/// </summary>
		public bool Connect(ref int nIndexOfItem)
		{
			return m_InstanceOfSocketDLL.Connect(nIndexOfItem);
		}
		/// <summary>
		/// 2020.06.15 by twkang [ADD] 아이템의 연결을 종료한다.
		/// </summary>
		public void DisConnect(ref int nIndexOfItem)
		{
			m_InstanceOfSocketDLL.Disconnect(nIndexOfItem);
		}
        /// <summary>
        /// 2021.08.17 by wdw [ADD] 연결상태를 확인한다.
        /// </summary>
        public bool IsConnected(ref int nIndexOfItem)
        {
            SOCKET_ITEM_STATE enState = m_InstanceOfSocketDLL.GetState(nIndexOfItem);
            if (enState == SOCKET_ITEM_STATE.DISABLED
                || enState == SOCKET_ITEM_STATE.DISCONNECTED
                || enState == SOCKET_ITEM_STATE.WAITING_CONNECTION
                || enState == SOCKET_ITEM_STATE.READY)
                return false;
            return true;
        }
		/// <summary>
		/// 2020.06.15 by twkang [ADD] 문자 데이터를 전송한다.
		/// </summary>
		public bool send(int nIndexOfItem, string strData)
		{
            return m_InstanceOfSocketDLL.Send(nIndexOfItem, strData);
		}
        /// <summary>
        /// 2020.06.15 by twkang [ADD] byte 데이터를 전송한다.
        /// </summary>
        public bool send(int nIndexOfItem, byte[] arData)
        {
            return m_InstanceOfSocketDLL.Send(nIndexOfItem, arData);
        }
		/// <summary>
		/// 2020.06.15 by twkang [ADD] 문자 데이터를 수신한다.
		/// </summary>
		public bool Receive(int nIndexOfItem, ref string strData)
		{			
            return m_InstanceOfSocketDLL.Receive(nIndexOfItem, ref strData);
		}
        /// <summary>
        /// 2020.06.15 by twkang [ADD] byte 데이터를 수신한다.
        /// </summary>
        public bool Receive(int nIndexOfItem, ref byte[] arData)
        {
            return m_InstanceOfSocketDLL.Receive(nIndexOfItem, ref arData);
        }
        /// <summary>
        /// 2021.01.19 by yjlee [ADD] Convert the integer type to the string type.
        /// </summary>
        public string ConvertIntToStringLogType(int nLogType)
        {
            if(m_DicForLogType.ContainsKey(nLogType))
            {
                return m_DicForLogType[nLogType];
            }

            return Define.DefineConstant.Common.NONE;
        }

		public void SetLogFunction(ref DelegateWritingSocketLog delWriteLog)
		{
            m_InstanceOfSocketDLL.SetLogFunction(ref delWriteLog, true);
        }
		#endregion

		#region 내부인터페이스
		/// <summary>
		/// 2020.06.15 by twkang [ADD] Storage 데이터로 Socket 파라미터를 초기화한다.
		/// </summary>
		private void InitializeSocketParameter()
		{
			string strValue			= string.Empty;
			string[] strArrGroup	= null;

			if(m_InstanceOfStorage.GetListOfGroup(Define.DefineEnumBase.Storage.EN_STORAGE_LIST.SOCKET, ref strArrGroup))
            {
                for (int nIndex = 0, nEnd = strArrGroup.Length; nIndex < nEnd; ++nIndex)
                {
                    string[] arGroup		= new string[1] { strArrGroup[nIndex] };
					int nIndexOfItem		= int.Parse(strArrGroup[nIndex]);
					m_InstanceOfSocketDLL.AddItem(nIndexOfItem);
					SetDefaultValue(nIndexOfItem);

                    foreach (EN_PARAM_SOCKET en in Enum.GetValues(typeof(EN_PARAM_SOCKET)))
                    {
                        if (m_InstanceOfStorage.GetParameter(Define.DefineEnumBase.Storage.EN_STORAGE_LIST.SOCKET
                            , m_DicForStorage[en]
                            , ref strValue
                            , ref arGroup))
                        {
							switch(en)
							{
								case EN_PARAM_SOCKET.PROTOCOL_MODE:
									if(m_DicForProtocolName.ContainsKey(strValue))
									{
										m_InstanceOfSocketDLL.SetParameter(nIndexOfItem, m_DicForItem[en], m_DicForProtocolName[strValue]);
									}
									break;

                                case EN_PARAM_SOCKET.LOG_TYPE:
                                    if(m_DicForLogTypeName.ContainsKey(strValue))
                                    {
                                        m_InstanceOfSocketDLL.SetParameter(nIndexOfItem, m_DicForItem[en], m_DicForLogTypeName[strValue]);
                                    }
                                    break;

								default:
									m_InstanceOfSocketDLL.SetParameter(nIndexOfItem, m_DicForItem[en], strValue);
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
			#region Parameter
			m_DicForItem.Clear();
			m_DicForItem.Add(EN_PARAM_SOCKET.ENABLE, PARAM_LIST.ENABLE);
			m_DicForItem.Add(EN_PARAM_SOCKET.NAME, PARAM_LIST.NAME);
			m_DicForItem.Add(EN_PARAM_SOCKET.PORT, PARAM_LIST.PORT);
			m_DicForItem.Add(EN_PARAM_SOCKET.PROTOCOL_MODE, PARAM_LIST.PROTOCOL_TYPE);
			m_DicForItem.Add(EN_PARAM_SOCKET.RECEIVE_TIMEOUT, PARAM_LIST.RECEIVE_TIMEOUT);
			m_DicForItem.Add(EN_PARAM_SOCKET.RETRY_INTERVAL, PARAM_LIST.RETRY_INTERVAL);
			m_DicForItem.Add(EN_PARAM_SOCKET.TARGET_IP, PARAM_LIST.TARGET_IP);
			m_DicForItem.Add(EN_PARAM_SOCKET.TARGET_PORT, PARAM_LIST.TARGET_PORT);
            m_DicForItem.Add(EN_PARAM_SOCKET.LOG_TYPE, PARAM_LIST.LOG_TYPE);
			#endregion

			#region Storage
			m_DicForStorage.Clear();
			m_DicForStorage.Add(EN_PARAM_SOCKET.ENABLE, ITEM_ENABLE);
			m_DicForStorage.Add(EN_PARAM_SOCKET.NAME, ITEM_NAME);
			m_DicForStorage.Add(EN_PARAM_SOCKET.PORT, ITEM_PORT);
			m_DicForStorage.Add(EN_PARAM_SOCKET.PROTOCOL_MODE, ITEM_PROTOCOL_TYPE);
			m_DicForStorage.Add(EN_PARAM_SOCKET.RECEIVE_TIMEOUT, ITEM_RECEIVE_TIMEOUT);
			m_DicForStorage.Add(EN_PARAM_SOCKET.RETRY_INTERVAL, ITEM_RETRY_INTERVAL);
			m_DicForStorage.Add(EN_PARAM_SOCKET.TARGET_IP, ITEM_TARGET_IP);
			m_DicForStorage.Add(EN_PARAM_SOCKET.TARGET_PORT, ITEM_TARGET_PORT);
            m_DicForStorage.Add(EN_PARAM_SOCKET.LOG_TYPE, ITEM_LOG_TYPE);
			#endregion

			#region State
			m_DicForState.Clear();
			m_DicForState.Add(SOCKET_ITEM_STATE.WAITING_RECEIVE, EN_SOCKET_STATE.WAITING_RECEIVE);
			m_DicForState.Add(SOCKET_ITEM_STATE.WAITING_CONNECTION, EN_SOCKET_STATE.WAITING_CONNECTION);
			m_DicForState.Add(SOCKET_ITEM_STATE.TIMEOUT_RECEIVE, EN_SOCKET_STATE.TIMEOUT_RECEIVE);
			m_DicForState.Add(SOCKET_ITEM_STATE.READY, EN_SOCKET_STATE.READY);
			m_DicForState.Add(SOCKET_ITEM_STATE.DISCONNECTED, EN_SOCKET_STATE.DISCONNECTED);
			m_DicForState.Add(SOCKET_ITEM_STATE.DISABLED, EN_SOCKET_STATE.DISABLED);
			m_DicForState.Add(SOCKET_ITEM_STATE.CONNECTED, EN_SOCKET_STATE.CONNECTED);
			#endregion

			m_DicForProtocolName.Clear();

			#region PROTOCOL_TYPE
			m_DicForProtocolType.Clear();
			foreach(PROTOCOL_TYPE en in Enum.GetValues(typeof(PROTOCOL_TYPE)))
			{
				m_DicForProtocolName.Add(en.ToString(), (int)en);
				m_DicForProtocolType.Add((int)en, en.ToString());
			}
			#endregion

            #region LOG_TYPE
            m_DicForLogTypeName.Clear();
            foreach (LOG_TYPE en in Enum.GetValues(typeof(LOG_TYPE)))
            {
                m_DicForLogTypeName.Add(en.ToString(), (int)en);
                m_DicForLogType.Add((int)en, en.ToString());
            }
            #endregion
		}
		/// <summary>
		/// 2020.06.29 by twkang [ADD] 파라미터의 값을 Storage 데이터로 저장한다.
		/// </summary>
		private bool SaveParamToStorage(int nIndexOfItem)
		{
			int nValue			= -1;
			string[] arGroup	= new string[] {nIndexOfItem.ToString()};
			
			foreach(EN_PARAM_SOCKET en in Enum.GetValues(typeof(EN_PARAM_SOCKET)))
			{
				string strValue		= string.Empty;

				switch(en)
				{
					case EN_PARAM_SOCKET.PROTOCOL_MODE:
						m_InstanceOfSocketDLL.GetParameter(nIndexOfItem, m_DicForItem[en], ref nValue);
						strValue		= m_DicForProtocolType.ContainsKey(nValue) ? m_DicForProtocolType[nValue] : Define.DefineConstant.Common.NONE;
						break;

					case EN_PARAM_SOCKET.ENABLE:
						bool bValue			= false;
						m_InstanceOfSocketDLL.GetParameter(nIndexOfItem, m_DicForItem[en], ref bValue);
						strValue		= bValue.ToString();
						break;

                    case EN_PARAM_SOCKET.LOG_TYPE:
                        m_InstanceOfSocketDLL.GetParameter(nIndexOfItem, m_DicForItem[en], ref nValue);
                        strValue		= m_DicForLogType.ContainsKey(nValue) ? m_DicForLogType[nValue] : Define.DefineConstant.Common.NONE;
                        break;

					default:
						m_InstanceOfSocketDLL.GetParameter(nIndexOfItem, m_DicForItem[en], ref strValue);
						break;
				}
				m_InstanceOfStorage.SetParameter(Define.DefineEnumBase.Storage.EN_STORAGE_LIST.SOCKET, m_DicForStorage[en], strValue, ref arGroup, false);
			}
			m_InstanceOfStorage.Save(Define.DefineEnumBase.Storage.EN_STORAGE_LIST.SOCKET);
			return true;
		}
		/// <summary>
		/// 2020.10.05 by twkang [ADD] 해당 아이템을 Default 값으로 설정한다.
		/// </summary>
		private void SetDefaultValue(int nIndex)
		{
			m_InstanceOfSocketDLL.SetParameter(nIndex, PARAM_LIST.ENABLE, DEFAULT_ENABLE);
			m_InstanceOfSocketDLL.SetParameter(nIndex, PARAM_LIST.NAME, DEFAULT_NAME + nIndex.ToString());
			m_InstanceOfSocketDLL.SetParameter(nIndex, PARAM_LIST.PORT, DEFAULT_PORT);
			m_InstanceOfSocketDLL.SetParameter(nIndex, PARAM_LIST.PROTOCOL_TYPE, DEFAULT_PROTOCOL);
			m_InstanceOfSocketDLL.SetParameter(nIndex, PARAM_LIST.RECEIVE_TIMEOUT, DEFAULT_TIMEOUT);
			m_InstanceOfSocketDLL.SetParameter(nIndex, PARAM_LIST.RETRY_INTERVAL, DEFAULT_RETRY_INTERVAL);
			m_InstanceOfSocketDLL.SetParameter(nIndex, PARAM_LIST.TARGET_IP, DEFAULT_IP);
			m_InstanceOfSocketDLL.SetParameter(nIndex, PARAM_LIST.TARGET_PORT, DEFAULT_TARGETPORT);
            m_InstanceOfSocketDLL.SetParameter(nIndex, PARAM_LIST.LOG_TYPE, DEFAULT_LOGTYPE);
		}
		#endregion
	}
}
