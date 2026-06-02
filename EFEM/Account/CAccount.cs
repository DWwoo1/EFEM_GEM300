using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;

using Account_;
using FileBorn_;

namespace FrameOfSystem3.Account
{
	public class CAccount
	{
		private CAccount() { }

		#region 싱글톤
		private static CAccount _Instance		= new CAccount();
		public static CAccount GetInstance()
		{
			return _Instance;
		}
		#endregion

		#region 열거형
		public enum EN_PARAM_ACCOUNT
		{
			ID,
			PASSWORD,
            AUTHORITY,
            LOGOUT_TIME,
            IRIS_LEFT,
            IRIS_RIGHT,
		}
		public enum EN_PARAM_USER_AUTHORITY
		{
			OPERATOR	= 0,
			ENGINEER	= 1,
			MASTER		= 2,
		}
		#endregion

		#region 상수

		#region Default Value
		private const int DEFAULT_AUTHORITY				= (int)USER_AUTHORITY.OPERATOR;
		private readonly string DEFAULT_ID				= "NONE";
		private readonly string DEFAULT_PASSWORD		= "0000";

        private const int c_nInterval = 1000;
		#endregion

		#endregion

		#region 변수
		Functional.Storage m_InstanceOfStorage			= null;
		Account_.Account m_InstanceOfAccount			= null;

		Dictionary<string, int> m_DicForResult			= new Dictionary<string,int>();
		Dictionary<int, string> m_DicForAuthority		= new Dictionary<int,string>();

		private Dictionary<EN_PARAM_ACCOUNT, PARAM_LIST> m_DicForItem	= new Dictionary<EN_PARAM_ACCOUNT,PARAM_LIST>();

        private Timer m_LogoutTimer = new Timer(c_nInterval);
        private DateTime m_dtLogoutCheckStart = DateTime.Now;
        private TimeSpan m_tsLogoutCheckSpendTime = new TimeSpan();
		#endregion
		
		#region 외부인터페이스

		#region 초기화
		/// <summary>
		/// 2020.08.25 by twkang [ADD] Account 클래스를 초기화한다.
		/// </summary>
		public bool Init()
		{
			MakeMappingTable();

			m_InstanceOfStorage				= Functional.Storage.GetInstance();
			m_InstanceOfAccount				= Account_.Account.GetInstance();
			
			if (false == m_InstanceOfStorage.Load(Define.DefineEnumBase.Storage.EN_STORAGE_LIST.ACCOUNT))
			{
				return false;
			}

			InitializeParameter();

            m_LogoutTimer.Elapsed += OnTimedEvent;
            m_LogoutTimer.AutoReset = true;
			return true;
		}
		#endregion

		#region Add & Remove
		/// <summary>
		/// 2020.08.25 by twkang [ADD] Account 아이템을 추가한다.
		/// </summary>
		public int AddAccountItem()
		{
			int nItem = m_InstanceOfAccount.AddItem();
			SetDefaultValue(nItem);

			string[] arGroup		= null;
			string[] arData			= null;

			if(FileBorn.GetInstance().GetItemFrame(BORN_LIST.ACCOUNT, ref arGroup, nItem.ToString(), ref arData)
				&& m_InstanceOfStorage.AddGroupAndItem(Define.DefineEnumBase.Storage.EN_STORAGE_LIST.ACCOUNT, ref arData))
			{
				SaveParamToStorage(nItem);
				return nItem;
			}

			m_InstanceOfAccount.RemoveItem(nItem);
			return -1;
		}
		/// <summary>
		/// 2020.08.25 by twkang [ADD] 아이템을 삭제한다.
		/// </summary>
		public bool RemoveItem(int nIndexOfItem)
		{
			string[] strArrGroup		= new string[] {nIndexOfItem.ToString()};

			if(m_InstanceOfStorage.RemoveGroup(Define.DefineEnumBase.Storage.EN_STORAGE_LIST.ACCOUNT
				, ref strArrGroup))
			{
				return m_InstanceOfAccount.RemoveItem(nIndexOfItem);
			}
			return false;
		}
		#endregion

		#region SetInformation
		/// <summary>
		/// 2020.08.25 by twkang [ADD] Account 파라미터 값을 변경한다.
		/// </summary>
		public bool SetParameter<T>(int nIndexOfItem, EN_PARAM_ACCOUNT enList, T tValue)
		{
			if (false == m_DicForItem.ContainsKey(enList))
			{
				return false;
			}

			string[] strArrGrup	= new string[] { nIndexOfItem.ToString() };

			if (false == m_InstanceOfStorage.SetParameter(Define.DefineEnumBase.Storage.EN_STORAGE_LIST.ACCOUNT
				, enList.ToString()
				, tValue
				, ref strArrGrup))
			{
				return false;
			}

			switch (enList)
			{
				case EN_PARAM_ACCOUNT.AUTHORITY:
					if (m_DicForResult.ContainsKey(tValue.ToString()))
					{
						return m_InstanceOfAccount.SetParameter(nIndexOfItem, m_DicForItem[enList], m_DicForResult[tValue.ToString()]);
					}
					break;
				default:
					return m_InstanceOfAccount.SetParameter(nIndexOfItem, m_DicForItem[enList], tValue);
			}
			
			return false;
		}
		#endregion

		#region GetInformation
		/// <summary>
		/// 2020.08.25 by twkang [ADD] Account 파라미터 값을 반환한다.
		/// </summary>
		public bool GetParameter<T>(int nIndexOfItem, EN_PARAM_ACCOUNT enList, ref T tValue)
		{
			if(m_DicForItem.ContainsKey(enList))
			{
				return m_InstanceOfAccount.GetParameter(nIndexOfItem, m_DicForItem[enList], ref tValue);
			}
			return false;
		}
		/// <summary>
		/// 2020.08.25 by twkang [ADD] 현재 로그인된 ID 를 반환한다.
		/// </summary>
		public string GetLoginedID()
		{
			return m_InstanceOfAccount.GetLoginedID();
		}
		/// <summary>
		/// 2020.08.25 by twkang [ADD] 현재 로그인된 계정 권한을 반환한다.
		/// </summary>
		public EN_PARAM_USER_AUTHORITY GetLoginedAuthority()
		{
			EN_PARAM_USER_AUTHORITY enAuthority	= EN_PARAM_USER_AUTHORITY.OPERATOR;
			
			switch(m_InstanceOfAccount.GetLoginedAuthority())
			{
				case USER_AUTHORITY.ENGINEER:
					enAuthority	= EN_PARAM_USER_AUTHORITY.ENGINEER;
					break;
				case USER_AUTHORITY.MASTER:
					enAuthority	= EN_PARAM_USER_AUTHORITY.MASTER;
					break;
				case USER_AUTHORITY.OPERATOR:
					break;
			}
			return enAuthority;
		}
		/// <summary>
		/// 2020.08.25 by twkang [ADD] 유저 리스트를 반환한다.
		/// </summary>
		public bool GetListOfUserList(ref string[] arUser)
		{
			int[] arIndex	= null;
			if (false == m_InstanceOfAccount.GetListOfItem(ref arIndex))
			{
				return false;
			}

			List<string> listUserList	= new List<string>();
			string strValue				= string.Empty;

			for(int nIndex = 0, nEnd = arIndex.Length; nIndex < nEnd; ++nIndex)
			{
				if(m_InstanceOfAccount.GetParameter(arIndex[nIndex], PARAM_LIST.ID, ref strValue))
				{
					listUserList.Add(strValue);
				}
			}

			arUser		= listUserList.ToArray();
			return true;
		}
		/// <summary>
		/// 2020.08.25 by twkang [ADD] 유저 리스트를 반환한다.
		/// </summary>
		public bool GetListOfUserList(ref int[] arUserIndex)
		{
			return m_InstanceOfAccount.GetListOfItem(ref arUserIndex);
		}

        public string GetLogoutRemainTime()
        {
            int nLogoutTime = m_InstanceOfAccount.GetLoginedLogoutTime();
            TimeSpan LogoutRemainTime = new TimeSpan(0, nLogoutTime, 0) - m_tsLogoutCheckSpendTime;
            return LogoutRemainTime.ToString(@"hh\:mm\:ss");
        }
		#endregion

		#region Login & Logout
		/// <summary>
		/// 2020.08.25 by twkang [ADD] 로그인
		/// </summary>
		public bool Login(string strID, string strPassWord)
		{
            bool bReturn = m_InstanceOfAccount.Login(strID, strPassWord);
           
            if(bReturn)
            {
                int nLogoutTime = Account_.Account.GetInstance().GetLoginedLogoutTime();
                if (nLogoutTime > 0)
                {
                    m_dtLogoutCheckStart = DateTime.Now;
                    m_LogoutTimer.Enabled = true;
                    Functional.GlobalEvent.GetInstance().SetHook();
                }
            }
            return bReturn;
		}
		/// <summary>
		/// 2020.08.25 by twkang [ADD] 로그아웃
		/// </summary>
		public bool Logout()
		{
            bool bReturn = m_InstanceOfAccount.Logout();
            if (bReturn)
            {
                m_LogoutTimer.Enabled = false;
                Functional.GlobalEvent.GetInstance().UnHook();
                m_tsLogoutCheckSpendTime = new TimeSpan();
            }
            return bReturn;
		}

        public void ResetLogoutTime()
        {
            m_dtLogoutCheckStart = DateTime.Now;
        }
		#endregion

		#endregion

		#region 내부인터페이스
		/// <summary>
		/// 2020.08.25 by twkang [ADD] Storage 데이터로 Account 파라미터를 초기화한다.
		/// </summary>
		private void InitializeParameter()
		{
			string strValue		= string.Empty;

			string[] strArrGroup	= null;

			if(m_InstanceOfStorage.GetListOfGroup(Define.DefineEnumBase.Storage.EN_STORAGE_LIST.ACCOUNT, ref strArrGroup))
			{
				for (int nIndex = 0, nEnd = strArrGroup.Length; nIndex < nEnd; ++nIndex)
				{
					string[] arGroup		= new string[] { strArrGroup[nIndex].ToString() };
					int nIndexOfItem		= int.Parse(strArrGroup[nIndex]);

					m_InstanceOfAccount.AddItem(nIndexOfItem);
					SetDefaultValue(nIndexOfItem);

					foreach (EN_PARAM_ACCOUNT en in Enum.GetValues(typeof(EN_PARAM_ACCOUNT)))
					{
						if (m_InstanceOfStorage.GetParameter(Define.DefineEnumBase.Storage.EN_STORAGE_LIST.ACCOUNT
							, en.ToString()
							, ref strValue
							, ref arGroup))
						{
							switch(en)
							{
								case EN_PARAM_ACCOUNT.AUTHORITY:
									if(m_DicForResult.ContainsKey(strValue))
									{
										m_InstanceOfAccount.SetParameter(nIndexOfItem, m_DicForItem[en], m_DicForResult[strValue]);
									}
									break;
								default:
									m_InstanceOfAccount.SetParameter(nIndexOfItem, m_DicForItem[en], strValue);
									break;
							}
						}
					}
				}
			}
		}
		/// <summary>
		/// 2020.08.25 by twkang [ADD] 클래스에서 사용할 Mapping Table 을 만든다.
		/// </summary>
		private void MakeMappingTable()
		{
			#region Param List
			m_DicForItem.Clear();
			m_DicForItem.Add(EN_PARAM_ACCOUNT.AUTHORITY, PARAM_LIST.AUTHORITY);
			m_DicForItem.Add(EN_PARAM_ACCOUNT.ID, PARAM_LIST.ID);
            m_DicForItem.Add(EN_PARAM_ACCOUNT.PASSWORD, PARAM_LIST.PASSWORD);
            m_DicForItem.Add(EN_PARAM_ACCOUNT.LOGOUT_TIME, PARAM_LIST.LOGOUT_TIME);
            m_DicForItem.Add(EN_PARAM_ACCOUNT.IRIS_LEFT, PARAM_LIST.IRIS_LEFT);
            m_DicForItem.Add(EN_PARAM_ACCOUNT.IRIS_RIGHT, PARAM_LIST.IRIS_RIGHT);
			#endregion

			m_DicForResult.Clear();
			m_DicForAuthority.Clear();
			
			#region AUTHORITY
			foreach(USER_AUTHORITY en in Enum.GetValues(typeof(USER_AUTHORITY)))
			{
				m_DicForResult.Add(en.ToString(), (int)en);
				m_DicForAuthority.Add((int)en, en.ToString());
			}
			#endregion
		}
		/// <summary>
		/// 2020.08.25 by twkang [ADD] 
		/// </summary>
		private bool SaveParamToStorage(int nIndexOfItem)
		{
			string[] arGroup			= new string[] {nIndexOfItem.ToString()};

			foreach(EN_PARAM_ACCOUNT en in Enum.GetValues(typeof(EN_PARAM_ACCOUNT)))
			{
				string strValue		= string.Empty;
				switch(en)
				{
					case EN_PARAM_ACCOUNT.AUTHORITY:
						int nValue			= -1;
						m_InstanceOfAccount.GetParameter(nIndexOfItem, m_DicForItem[en], ref nValue);
						strValue	= m_DicForAuthority.ContainsKey(nValue) ? m_DicForAuthority[nValue] : USER_AUTHORITY.OPERATOR.ToString();
						break;
					default:
						m_InstanceOfAccount.GetParameter(nIndexOfItem, m_DicForItem[en], ref strValue);
						break;
				}

				
				m_InstanceOfStorage.SetParameter(Define.DefineEnumBase.Storage.EN_STORAGE_LIST.ACCOUNT, en.ToString(), strValue, ref arGroup);
			}

			m_InstanceOfStorage.Save(Define.DefineEnumBase.Storage.EN_STORAGE_LIST.ACCOUNT);
			return true;
		}
		/// <summary>
		/// 2020.10.06 by twkang [ADD] 해당 아이템을 Default 값으로 설정한다.
		/// </summary>
		private void SetDefaultValue(int nIndex)
		{
			m_InstanceOfAccount.SetParameter(nIndex, PARAM_LIST.AUTHORITY, DEFAULT_AUTHORITY);
			m_InstanceOfAccount.SetParameter(nIndex, PARAM_LIST.ID, DEFAULT_ID);
			m_InstanceOfAccount.SetParameter(nIndex, PARAM_LIST.PASSWORD, DEFAULT_PASSWORD);
        }


        #region LogoutTimer
        private void OnTimedEvent(Object source, ElapsedEventArgs e)
        {
            switch (Account_.Account.GetInstance().GetLoginedAuthority())
            {
                case USER_AUTHORITY.ENGINEER:
                case USER_AUTHORITY.MASTER:
                    int nLogoutTime = Account_.Account.GetInstance().GetLoginedLogoutTime();
                    if(nLogoutTime > 0)
                    {
                        m_tsLogoutCheckSpendTime = DateTime.Now - m_dtLogoutCheckStart;
                        if (m_tsLogoutCheckSpendTime.TotalMinutes > nLogoutTime)
                        {
                            Logout();
                        }
                    }
                    break;
                case USER_AUTHORITY.OPERATOR:
                    break;
            }
        }
        #endregion /LogoutTimer
        #endregion
    }
}
