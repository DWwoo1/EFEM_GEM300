using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Cylinder_;
using Define.DefineEnumBase.Common;
using FileBorn_;

namespace FrameOfSystem3.Config
{
	extern alias CylinderInstance;

    public class ConfigCylinder
	{
		#region 싱글톤
		private static ConfigCylinder _Instance	 = new ConfigCylinder();
		public static ConfigCylinder GetInstance()
		{
			return _Instance;
		}
		private ConfigCylinder() { }
		#endregion

		#region 열거형
		public enum EN_PARAM_CYLINDER
		{
			NAME				= 0,
			ENABLE				= 1,
			MONITORING			= 2,
			FORWARD_DELAY		= 3,
			FORWARD_TIMEOUT		= 4,
			BACKWARD_DELAY		= 5,
			BACKWARD_TIMEOUT	= 6,
			//// 2025.03.12. by shkim. [ADD] INPUT CHECK를 AND 조건으로 볼지 OR 조건으로 볼지에 대한 기능 추가
			FORWARD_CHECK_LOGIC_OR = 7,
			BACKWARD_CHECK_LOGIC_OR = 8,
			//// 2025.03.12. by shkim. [END]
		}
		public enum EN_PARAM_LIST_IO
		{
			FORWARD_IN		= 0,
			FORWARD_OUT		= 1,
			BACKWARD_IN		= 2,
			BACKWARD_OUT	= 3,
		}
		#endregion

		#region 상수
		private const int INITIALIZE_PARAM					= -1;
		private const int INTERVAL_REPEAT_CHECK_FUNCTION	= 50;

		private readonly string GROUP_FORWARD			= "FORWARD";
		private readonly string GROUP_BACKWARD			= "BACKWARD";

		private readonly string ITEM_NAME				= "NAME";
		private readonly string ITEM_ENABLE				= "ENABLE";
		private readonly string ITEM_MONITORING			= "MONITORING";
		private readonly string ITEM_INPUT				= "INPUT";
		private readonly string ITEM_OUTPUT				= "OUTPUT";
		private readonly string ITEM_DELAY				= "DELAY";
		private readonly string ITEM_TIMEOUT			= "TIMEOUT";

        // 2025.03.12. by shkim. [ADD] INPUT CHECK를 AND 조건으로 볼지 OR 조건으로 볼지에 대한 기능 추가
        private readonly string ITEM_FORWARD_CHECK_LOGIC_OR = "FORWARD_CHECK_LOGIC_OR";
        private readonly string ITEM_BACKWARD_CHECK_LOGIC_OR = "BACKWARD_CHECK_LOGIC_OR";
        // 2025.03.12. by shkim. [END]

        #region Default Value
        private readonly string DEFAULT_NAME			= "NONE";
		private const bool DEFALUT_ENABLE				= true;
        private const bool DEFALUT_DISABLE				= false;
        private const int DEFAULT_MONITORING			= (int)MONITORING_MODE.ENABLE;
		private const int DEFALUT_IN					= -1;
		private const int DEFALUT_OUT					= -1;
		private const int DEFALUT_DELAY					= 1000;
		private const int DEFAULT_TIMEOUT				= 10000;
		#endregion

		#endregion

		#region 변수
		Functional.Storage m_InstanceOfStorage			= null;
		System.Timers.Timer	m_timerForRepeat			= null;
		Cylinder m_InstanceOfCylinderDLL				= null;

		private List<int> m_ListForRepeat								= new List<int>();
		private System.Threading.ReaderWriterLockSlim RWLock			= new System.Threading.ReaderWriterLockSlim();

		private Dictionary<string, int> m_DicForResult					= new Dictionary<string, int>();
		private Dictionary<int, string> m_DicForMonitoringMode			= new Dictionary<int, string>();

		private Dictionary<EN_PARAM_CYLINDER, string> m_DicForStorage		= new Dictionary<EN_PARAM_CYLINDER, string>();
		private Dictionary<EN_PARAM_CYLINDER, PARAM_LIST> m_DicForItem		= new Dictionary<EN_PARAM_CYLINDER, PARAM_LIST>();
		private Dictionary<EN_PARAM_LIST_IO, PARAM_LIST_IO> m_DicForIOItem	= new Dictionary<EN_PARAM_LIST_IO,PARAM_LIST_IO>();

        ConfigInterlock m_InstanceOfInterlock = ConfigInterlock.GetInstance();
        #endregion

        #region 외부인터페이스
        /// <summary>
		/// 2020.06.09 by twkang [ADD] Cylinder 클래스를 초기화한다.
		/// </summary>
		public bool Init()
		{
			MakeMappingTabel();

			m_InstanceOfStorage		= Functional.Storage.GetInstance();
			m_InstanceOfCylinderDLL	= Cylinder.GetInstance();

			if (false == m_InstanceOfStorage.Load(Define.DefineEnumBase.Storage.EN_STORAGE_LIST.CYLINDER))
			{
				return false;
			}
			
			InitializeParameter();

			#region Repeat
			m_ListForRepeat.Clear();
			m_timerForRepeat = new System.Timers.Timer();
			m_timerForRepeat.BeginInit();
			m_timerForRepeat.Elapsed += new System.Timers.ElapsedEventHandler(CallbackFunctionForTimer);
			m_timerForRepeat.Interval = INTERVAL_REPEAT_CHECK_FUNCTION;
			m_timerForRepeat.EndInit();
			m_timerForRepeat.Start();
			#endregion

			return true;
		}
		/// <summary>
		/// 2020.06.09 by twkang [ADD] Cylinder 의 파라미터 값을 변경한다.
		/// </summary>
		public bool SetParameter<T>(int nIndexOfItem, EN_PARAM_CYLINDER enParam, T tValue)
		{
			if(m_DicForStorage.ContainsKey(enParam))
			{
				string[] strArrGroup	= null;

				switch(enParam)
				{
					case EN_PARAM_CYLINDER.NAME:
					case EN_PARAM_CYLINDER.MONITORING:
					case EN_PARAM_CYLINDER.ENABLE:
					//// 2025.03.12. by shkim. [ADD] INPUT CHECK를 AND 조건으로 볼지 OR 조건으로 볼지에 대한 기능 추가
					case EN_PARAM_CYLINDER.FORWARD_CHECK_LOGIC_OR:
					case EN_PARAM_CYLINDER.BACKWARD_CHECK_LOGIC_OR:
						//	// 2025.03.12. by shkim. [END]
						strArrGroup = new string[] {nIndexOfItem.ToString()};
						break;
					case EN_PARAM_CYLINDER.FORWARD_DELAY:
					case EN_PARAM_CYLINDER.FORWARD_TIMEOUT:
						strArrGroup		= new string[] {nIndexOfItem.ToString(), GROUP_FORWARD};
						break;
					case EN_PARAM_CYLINDER.BACKWARD_DELAY:
					case EN_PARAM_CYLINDER.BACKWARD_TIMEOUT:
						strArrGroup		= new string[] {nIndexOfItem.ToString(), GROUP_BACKWARD};
						break;
				}

				if(m_InstanceOfStorage.SetParameter(Define.DefineEnumBase.Storage.EN_STORAGE_LIST.CYLINDER
					, m_DicForStorage[enParam]
					, tValue
					, ref strArrGroup))
				{
					switch(enParam)
					{
						case EN_PARAM_CYLINDER.MONITORING:
							if(m_DicForResult.ContainsKey(tValue.ToString()))
							{
								return m_InstanceOfCylinderDLL.SetParameter(nIndexOfItem, m_DicForItem[enParam], m_DicForResult[tValue.ToString()]);
							}
							break;

						// 2025.03.12. by shkim. [ADD] INPUT CHECK를 AND 조건으로 볼지 OR 조건으로 볼지에 대한 기능 추가

						case EN_PARAM_CYLINDER.FORWARD_CHECK_LOGIC_OR:
						case EN_PARAM_CYLINDER.BACKWARD_CHECK_LOGIC_OR:
							const int ON = 1;
							const int OFF = 0;
							bool bValue = Convert.ToBoolean(tValue);
							return m_InstanceOfCylinderDLL.SetParameter(nIndexOfItem, m_DicForItem[enParam], (bValue ? ON : OFF));

						// 2025.03.12. by shkim. [END]

						default:
							return m_InstanceOfCylinderDLL.SetParameter(nIndexOfItem, m_DicForItem[enParam], tValue);
					}
				}
			}
			return false;
		}
		/// <summary>
		/// 2020.11.16 by twkang [ADD] Cylinder 의 IO 파라미터 값을 변경한다.
		/// </summary>
		public bool SetParameter(int nIndexOfItem, EN_PARAM_LIST_IO enParam, int nCount, ref int[] arParam)
		{
			string strGroupName		= string.Empty;
			string[] arGroup		= null;

			
			string strValue		= nCount == 0 ? "-1" : string.Join(", ", arParam);

			switch(enParam)
			{
				case EN_PARAM_LIST_IO.FORWARD_OUT:
					arGroup			= new string[] {nIndexOfItem.ToString(), GROUP_FORWARD};
					strGroupName	= ITEM_OUTPUT;
					break;
				case EN_PARAM_LIST_IO.FORWARD_IN:
					arGroup			= new string[] {nIndexOfItem.ToString(), GROUP_FORWARD};
					strGroupName	= ITEM_INPUT;
					break;
				case EN_PARAM_LIST_IO.BACKWARD_OUT:
					arGroup			= new string[] {nIndexOfItem.ToString(), GROUP_BACKWARD};
					strGroupName	= ITEM_OUTPUT;
					break;
				case EN_PARAM_LIST_IO.BACKWARD_IN:
					arGroup			= new string[] {nIndexOfItem.ToString(), GROUP_BACKWARD};
					strGroupName	= ITEM_INPUT;
					break;
			}

			if(m_InstanceOfStorage.SetParameter(Define.DefineEnumBase.Storage.EN_STORAGE_LIST.CYLINDER
				, strGroupName
				, strValue
				, ref arGroup))
			{
				return m_InstanceOfCylinderDLL.SetParameter(nIndexOfItem, m_DicForIOItem[enParam], nCount, ref arParam);
			}
			return false;
		}
		/// <summary>
		/// 2020.06.09 by twkang [ADD] Cylinder 파라미터 값을 반환한다.
		/// </summary>
		public bool GetParameter<T>(int nIndexOfItem, EN_PARAM_CYLINDER enParam, ref T tValue)
		{
			if(m_DicForItem.ContainsKey(enParam))
			{
				// 2025.03.12. by shkim. [MOD] INPUT CHECK를 AND 조건으로 볼지 OR 조건으로 볼지에 대한 기능 추가
				switch (enParam)
				{
					// 2025.03.12. by shkim. [ADD] INPUT CHECK를 AND 조건으로 볼지 OR 조건으로 볼지에 대한 기능 추가
					case EN_PARAM_CYLINDER.FORWARD_CHECK_LOGIC_OR:
					case EN_PARAM_CYLINDER.BACKWARD_CHECK_LOGIC_OR:
						int nValue = -1;
						if (m_InstanceOfCylinderDLL.GetParameter(nIndexOfItem, m_DicForItem[enParam], ref nValue))
						{
							tValue = (T)Convert.ChangeType(nValue == 1, typeof(T));
							return true;
						}
						return false;
					// 2025.03.12. by shkim. [END]

					default:
						return m_InstanceOfCylinderDLL.GetParameter(nIndexOfItem, m_DicForItem[enParam], ref tValue);
				}
				// 2025.03.12. by shkim. [END]
			}

			return false;
		}
		/// <summary>
		/// 2020.11.16 by twkang [ADD] Cylinder IO 파라미터 값을 반환한다.
		/// </summary>
		public bool GetParameter(int nIndexOfItem, EN_PARAM_LIST_IO enParam, ref int nCount, ref int[] arParam)
		{
			if(m_DicForIOItem.ContainsKey(enParam))
			{
				return m_InstanceOfCylinderDLL.GetParameter(nIndexOfItem, m_DicForIOItem[enParam], ref nCount, ref arParam);
			}
			return false;
		}
		/// <summary>
		/// 2020.06.09 by twkang [ADD] Cylinder Item 수를 반환한다.
		/// </summary>
		public bool GetListOfItem(ref int[] arIndexes)
		{
			return m_InstanceOfCylinderDLL.GetListOfItem(ref arIndexes);
		}
        /// <summary>
        /// 2021.11.24 by wdw [ADD] Cylinder 아이템의 Name 를 반환한다.
        /// </summary>
        public string GetName(int nIndex)
        {
            string strName = string.Empty;

            m_InstanceOfCylinderDLL.GetParameter(nIndex, PARAM_LIST.NAME, ref strName);

            return strName;
        }
		/// <summary>
		/// 2020.10.13 by twkang [ADD] Cylinder 아이템들의 NameList 를 반환한다.
		/// </summary>
		public bool GetListOfName(ref string[] arNameList)
		{
			int[] arIndexes	= null;

			if(false == m_InstanceOfCylinderDLL.GetListOfItem(ref arIndexes))
			{
				return false;
			}

			arNameList		= new string[arIndexes.Length];

			for(int nIndex = 0, nEnd = arIndexes.Length; nIndex < nEnd; ++nIndex)
			{
				string strName		= string.Empty;
				m_InstanceOfCylinderDLL.GetParameter(arIndexes[nIndex], PARAM_LIST.NAME, ref strName);
				arNameList[nIndex]	= strName;
			}
			return true;
		}
		/// <summary>
		/// 2020.06.09 by twkang [ADD] 해당 아이템의 상태를 반환한다.
		/// </summary>
		public void GetCylinderState(int nIndexOfItem, ref string strState)
		{			
			strState = m_InstanceOfCylinderDLL.GetCylinderState(nIndexOfItem).ToString();
		}
		/// <summary>
		/// 2020.06.29 by twkang [ADD] 아이템을 추가한다.
		/// </summary>
		public int AddCylinderItem()
		{
			int nItemNum = m_InstanceOfCylinderDLL.AddItem();
			SetDefaultValue(nItemNum);

			string[] arGroup	= null;
			string[] arData		= null;

			if(FileBorn.GetInstance().GetItemFrame(BORN_LIST.CYLINDER, ref arGroup, nItemNum.ToString(), ref arData)
				&& m_InstanceOfStorage.AddGroupAndItem(Define.DefineEnumBase.Storage.EN_STORAGE_LIST.CYLINDER, ref arData))
			{
				SaveParameterToStorage(nItemNum);
				return nItemNum;
			}

			m_InstanceOfCylinderDLL.RemoveItem(nItemNum);
			return -1;
		}
		/// <summary>
		/// 2022.07.20 by junho [ADD] List에 있는 항목들로 아이템을 추가한다.
		/// </summary>
		public bool AddCylinderItemArray(string[] nameList)
		{
			foreach (string name in nameList)
			{
				int nItemNum = m_InstanceOfCylinderDLL.AddItem();
				SetDefaultValue(nItemNum);

				m_InstanceOfCylinderDLL.SetParameter(nItemNum, PARAM_LIST.NAME, name);


				string[] arGroup = null;
				string[] arData = null;

				if (FileBorn.GetInstance().GetItemFrame(BORN_LIST.CYLINDER, ref arGroup, nItemNum.ToString(), ref arData)
				&& m_InstanceOfStorage.AddGroupAndItem(Define.DefineEnumBase.Storage.EN_STORAGE_LIST.CYLINDER, ref arData))
				{
					SaveParameterToStorage(nItemNum);
					continue;
				}

				m_InstanceOfCylinderDLL.RemoveItem(nItemNum);
				return false;
			}

			return true;
		}
		/// <summary>
		/// 2020.06.11 by twkang [ADD] 아이템을 삭제한다.
		/// </summary>
		public bool RemoveItem(int nIndexOfItem)
		{
			string[] arGroup	= new string[] { nIndexOfItem.ToString() };
			if(m_InstanceOfStorage.RemoveGroup(Define.DefineEnumBase.Storage.EN_STORAGE_LIST.CYLINDER, ref arGroup))
			{
				return m_InstanceOfCylinderDLL.RemoveItem(nIndexOfItem);
			}
			return false;
		}
		/// <summary>
        /// 2022.02.15 by WDW [ADD] INTERLOCK 추가.
        /// 2020.06.11 by twkang [ADD] 실린더를 포워드 방향으로 이동한다.
		/// </summary>
        public CYLINDER_RESULT MoveForward(int nIndexOfItem)
		{
            string strInterlockDiscription = string.Empty;
            CYLINDER_RESULT enReuslt = m_InstanceOfCylinderDLL.MoveForward(nIndexOfItem);
            if (enReuslt == CYLINDER_RESULT.OCCUR_INTERLOCK)
            {
                string strTitle = "INTERLOCK CYLINDER " + GetName(nIndexOfItem);
                strInterlockDiscription = m_InstanceOfInterlock.GetCylinderLastInterference(nIndexOfItem);
                Views.Functional.Form_MessageBox.GetInstance().ShowMessage(strInterlockDiscription, strTitle);
            }
            else if (enReuslt != CYLINDER_RESULT.OK)
            {
                Views.Functional.Form_MessageBox.GetInstance().ShowMessage(enReuslt.ToString());
            }
            return enReuslt;
		}
		/// <summary>
        /// 2022.02.15 by WDW [ADD] INTERLOCK 추가.
        /// 2020.06.11 by twkang [ADD] 실린더를 백워드 방향으로 이동한다.
		/// </summary>
        public Cylinder_.CYLINDER_RESULT MoveBackward(int nIndexOfItem)
        {
            string strInterlockDiscription = string.Empty;
            CYLINDER_RESULT enReuslt = m_InstanceOfCylinderDLL.MoveBackward(nIndexOfItem);
            if (enReuslt == CYLINDER_RESULT.OCCUR_INTERLOCK)
            {
                string strTitle = "INTERLOCK CYLINDER " + GetName(nIndexOfItem);
                strInterlockDiscription = m_InstanceOfInterlock.GetCylinderLastInterference(nIndexOfItem);
                Views.Functional.Form_MessageBox.GetInstance().ShowMessage(strInterlockDiscription, strTitle);
            }
            else if (enReuslt != CYLINDER_RESULT.OK)
            {
                Views.Functional.Form_MessageBox.GetInstance().ShowMessage(enReuslt.ToString());
            }
            return enReuslt;
        }
		/// <summary>
		/// 2020.06.11 by twkang [ADD] 실린더의 Repeat 동작을 지시한다.
		/// </summary>
		public void SetRepeat(int nIndexOfItem)
		{
			if(IsRepeat(nIndexOfItem))
			{
				RWLock.EnterWriteLock();
				m_ListForRepeat.Remove(nIndexOfItem);
				RWLock.ExitWriteLock();
				return;
			}
			RWLock.EnterWriteLock();
			m_ListForRepeat.Add(nIndexOfItem);
			RWLock.ExitWriteLock();
		}
		/// <summary>
		/// 2020.06.11 by twkang [ADD] 실린더가 리핏 상태인지 확인한다.
		/// </summary>
		public bool IsRepeat(int nIndexOfItem)
		{
			RWLock.EnterReadLock();
			bool temp = m_ListForRepeat.Contains(nIndexOfItem);
			RWLock.ExitReadLock();

			return temp;
		}
		#endregion

		#region 내부인터페이스
		/// <summary>
		/// 2020.06.09 by twkang [ADD] Storage 데이터로 Cylinder 파라미터를 초기화한다.
		/// </summary>
		private void InitializeParameter()
		{
			string strValue				= string.Empty;
			string[] strArrGroup		= null;

			if(m_InstanceOfStorage.GetListOfGroup(Define.DefineEnumBase.Storage.EN_STORAGE_LIST.CYLINDER, ref strArrGroup))
            {
                for(int nIndex = 0, nEnd = strArrGroup.Length; nIndex < nEnd; ++nIndex)
			    {
					int nIndexOfItem		= int.Parse(strArrGroup[nIndex]);

					m_InstanceOfCylinderDLL.AddItem(nIndexOfItem);
					SetDefaultValue(nIndexOfItem);

                    //2022.02.04 by wdw [Fix] 순서 변경. IO 설정 전에 Monitoring 설정 시 쓰레기 값 확인 하여 초기 Status 이상
                    #region IO
                    foreach (EN_PARAM_LIST_IO enParameter in Enum.GetValues(typeof(EN_PARAM_LIST_IO)))
                    {
                        string[] arGroup = null;
                        string strItem = null;

                        switch (enParameter)
                        {
                            case EN_PARAM_LIST_IO.BACKWARD_IN:
                                arGroup = new string[] { nIndexOfItem.ToString(), GROUP_BACKWARD };
                                strItem = ITEM_INPUT;
                                break;
                            case EN_PARAM_LIST_IO.BACKWARD_OUT:
                                arGroup = new string[] { nIndexOfItem.ToString(), GROUP_BACKWARD };
                                strItem = ITEM_OUTPUT;
                                break;
                            case EN_PARAM_LIST_IO.FORWARD_IN:
                                arGroup = new string[] { nIndexOfItem.ToString(), GROUP_FORWARD };
                                strItem = ITEM_INPUT;
                                break;
                            case EN_PARAM_LIST_IO.FORWARD_OUT:
                                arGroup = new string[] { nIndexOfItem.ToString(), GROUP_FORWARD };
                                strItem = ITEM_OUTPUT;
                                break;
                        }

                        if (m_InstanceOfStorage.GetParameter(Define.DefineEnumBase.Storage.EN_STORAGE_LIST.CYLINDER
                            , strItem
                            , ref strValue
                            , ref arGroup))
                        {
                            string[] arValue = strValue.Split(new string[] { ", " }, StringSplitOptions.None);

                            int[] arData = new int[arValue.Length];
                            for (int i = 0, nLast = arValue.Length; i < nLast; ++i)
                            {
                                if (false == int.TryParse(arValue[i], out arData[i]))
                                {
                                    arData[i] = DEFALUT_IN;
                                }
                            }
                            int nCoutn = -1;
                            m_InstanceOfCylinderDLL.SetParameter(nIndexOfItem, m_DicForIOItem[enParameter], arData.Length, ref arData);
                            m_InstanceOfCylinderDLL.GetParameter(nIndexOfItem, m_DicForIOItem[enParameter], ref nCoutn, ref arData);
                        }
                    }
                    #endregion

					#region Config
					foreach (EN_PARAM_CYLINDER en in Enum.GetValues(typeof(EN_PARAM_CYLINDER)))
			    	{
						string[] arGroup		= null;

			    		switch(en)
			    		{
			    			case EN_PARAM_CYLINDER.BACKWARD_DELAY:
			    			case EN_PARAM_CYLINDER.BACKWARD_TIMEOUT:
								arGroup	= new string[] {nIndexOfItem.ToString(), GROUP_BACKWARD};
			    				break;
			    			case EN_PARAM_CYLINDER.FORWARD_DELAY:
			    			case EN_PARAM_CYLINDER.FORWARD_TIMEOUT:
								arGroup	= new string[] {nIndexOfItem.ToString(), GROUP_FORWARD};
			    				break;
							default:
								arGroup	= new string[] {nIndexOfItem.ToString()};
			    				break;
			    		}

			    		if(m_InstanceOfStorage.GetParameter(Define.DefineEnumBase.Storage.EN_STORAGE_LIST.CYLINDER
			    			, m_DicForStorage[en]
			    			, ref strValue
			    			, ref arGroup))
			    		{
							switch(en)
							{
								case EN_PARAM_CYLINDER.MONITORING:
									if(m_DicForResult.ContainsKey(strValue))
									{
										m_InstanceOfCylinderDLL.SetParameter(nIndexOfItem, m_DicForItem[en], m_DicForResult[strValue]);
									}
									break;
								default:
									m_InstanceOfCylinderDLL.SetParameter(nIndexOfItem, m_DicForItem[en], strValue);
									break;
							}
			    		}
			    	}
					#endregion

			    }
            }
		}
		/// <summary>
		/// 2020.06.29 by twkang [ADD] 클래스에서 사용할 Mapping Table 을 만든다.
		/// </summary>
		private void MakeMappingTabel()
		{
			#region Storage
			m_DicForStorage.Clear();
			m_DicForStorage.Add(EN_PARAM_CYLINDER.NAME, ITEM_NAME);
			m_DicForStorage.Add(EN_PARAM_CYLINDER.MONITORING, ITEM_MONITORING);
			m_DicForStorage.Add(EN_PARAM_CYLINDER.FORWARD_TIMEOUT, ITEM_TIMEOUT);
			m_DicForStorage.Add(EN_PARAM_CYLINDER.FORWARD_DELAY, ITEM_DELAY);
			m_DicForStorage.Add(EN_PARAM_CYLINDER.ENABLE, ITEM_ENABLE);
			m_DicForStorage.Add(EN_PARAM_CYLINDER.BACKWARD_TIMEOUT, ITEM_TIMEOUT);
			m_DicForStorage.Add(EN_PARAM_CYLINDER.BACKWARD_DELAY, ITEM_DELAY);

			// 2025.03.12. by shkim. [ADD] INPUT CHECK를 AND 조건으로 볼지 OR 조건으로 볼지에 대한 기능 추가
			m_DicForStorage.Add(EN_PARAM_CYLINDER.FORWARD_CHECK_LOGIC_OR, ITEM_FORWARD_CHECK_LOGIC_OR);
			m_DicForStorage.Add(EN_PARAM_CYLINDER.BACKWARD_CHECK_LOGIC_OR, ITEM_BACKWARD_CHECK_LOGIC_OR);
			// 2025.03.12. by shkim. [END]

			#endregion

			#region Parameter Name
			m_DicForItem.Clear();
			m_DicForItem.Add(EN_PARAM_CYLINDER.NAME, PARAM_LIST.NAME);
			m_DicForItem.Add(EN_PARAM_CYLINDER.MONITORING, PARAM_LIST.MONITORING);
			m_DicForItem.Add(EN_PARAM_CYLINDER.FORWARD_TIMEOUT, PARAM_LIST.FORWARD_TIMEOUT);
			m_DicForItem.Add(EN_PARAM_CYLINDER.FORWARD_DELAY, PARAM_LIST.FORWARD_DELAY);
			m_DicForItem.Add(EN_PARAM_CYLINDER.ENABLE, PARAM_LIST.ENABLE);
			m_DicForItem.Add(EN_PARAM_CYLINDER.BACKWARD_TIMEOUT, PARAM_LIST.BACKWARD_TIMEOUT);
			m_DicForItem.Add(EN_PARAM_CYLINDER.BACKWARD_DELAY, PARAM_LIST.BACKWARD_DELAY);

			// 2025.03.12. by shkim. [ADD] INPUT CHECK를 AND 조건으로 볼지 OR 조건으로 볼지에 대한 기능 추가
			m_DicForItem.Add(EN_PARAM_CYLINDER.FORWARD_CHECK_LOGIC_OR, PARAM_LIST.FORWARD_CHECK_LOGIC_OR);
			m_DicForItem.Add(EN_PARAM_CYLINDER.BACKWARD_CHECK_LOGIC_OR, PARAM_LIST.BACKWARD_CHECK_LOGIC_OR);
			// 2025.03.12. by shkim. [END]
			#endregion

			#region IO Parameter
			m_DicForIOItem.Clear();
			m_DicForIOItem.Add(EN_PARAM_LIST_IO.BACKWARD_IN, PARAM_LIST_IO.BACKWARD_IN);
			m_DicForIOItem.Add(EN_PARAM_LIST_IO.BACKWARD_OUT, PARAM_LIST_IO.BACKWARD_OUT);
			m_DicForIOItem.Add(EN_PARAM_LIST_IO.FORWARD_IN, PARAM_LIST_IO.FORWARD_IN);
			m_DicForIOItem.Add(EN_PARAM_LIST_IO.FORWARD_OUT, PARAM_LIST_IO.FORWARD_OUT);
			#endregion

			#region Monitoring
			m_DicForResult.Clear();
			m_DicForMonitoringMode.Clear();
			foreach (MONITORING_MODE en in Enum.GetValues(typeof(MONITORING_MODE)))
			{
				m_DicForResult.Add(en.ToString(), (int)en);
				m_DicForMonitoringMode.Add((int)en, en.ToString());
			}
			#endregion
		}
		/// <summary>
		/// 2020.06.12 by twkang [ADD] 타이머에 의해 호출되는 함수, Repeat 동작을 담당하는 함수
		/// </summary>
		private void CallbackFunctionForTimer(object sender, System.Timers.ElapsedEventArgs args)
		{
            RWLock.EnterReadLock();

			if(m_ListForRepeat.Count > 0)
			{
				foreach(int nIndexOfItem in m_ListForRepeat)
				{
					switch(m_InstanceOfCylinderDLL.GetCylinderState(nIndexOfItem))
					{
						case CylinderInstance.Cylinder_.CYLINDER_STATE.BACKWARD:
							MoveForward(nIndexOfItem);
							break;
						case CylinderInstance.Cylinder_.CYLINDER_STATE.FORWARD:
							MoveBackward(nIndexOfItem);
							break;
					}
				}
			}

            RWLock.ExitReadLock();
		}
		/// <summary>
		/// 2020.06.29 by twkang [ADD] 파라미터의 값을 Storage 데이터로 저장한다.
		/// </summary>
		private bool SaveParameterToStorage(int nIndexOfItem)
		{
			string strValue				= string.Empty;
			string[] arGroup			= null;
			List<string> listForGroup	= new List<string>();

			int nValue		= -1;
			bool bValue		= false;

			#region Config
			foreach (EN_PARAM_CYLINDER en in Enum.GetValues(typeof(EN_PARAM_CYLINDER)))
			{
				listForGroup.Clear();
				listForGroup.Add(nIndexOfItem.ToString());

				switch(en)
				{
					case EN_PARAM_CYLINDER.MONITORING:
						m_InstanceOfCylinderDLL.GetParameter(nIndexOfItem, m_DicForItem[en], ref nValue);						
						if(m_DicForMonitoringMode.ContainsKey(nValue))
						{
							strValue	= m_DicForMonitoringMode[nValue];
						}
						else
						{
							strValue	= Define.DefineConstant.Common.NONE;
						}
						break;
					case EN_PARAM_CYLINDER.BACKWARD_DELAY:
					case EN_PARAM_CYLINDER.BACKWARD_TIMEOUT:
						m_InstanceOfCylinderDLL.GetParameter(nIndexOfItem, m_DicForItem[en], ref strValue);
						if(string.IsNullOrEmpty(strValue))
						{
							strValue	= INITIALIZE_PARAM.ToString();
						}
						listForGroup.Add(GROUP_BACKWARD);
						break;
					case EN_PARAM_CYLINDER.FORWARD_DELAY:
					case EN_PARAM_CYLINDER.FORWARD_TIMEOUT:
						m_InstanceOfCylinderDLL.GetParameter(nIndexOfItem, m_DicForItem[en], ref strValue);
						if (string.IsNullOrEmpty(strValue))
						{
							strValue = INITIALIZE_PARAM.ToString();
						}
						listForGroup.Add(GROUP_FORWARD);
						break;
					case EN_PARAM_CYLINDER.NAME:
						m_InstanceOfCylinderDLL.GetParameter(nIndexOfItem, m_DicForItem[en], ref strValue);						
						break;
					case EN_PARAM_CYLINDER.ENABLE:
						m_InstanceOfCylinderDLL.GetParameter(nIndexOfItem, m_DicForItem[en], ref bValue);
						strValue	= bValue.ToString();						
						break;
				}
				arGroup		= listForGroup.ToArray();
				m_InstanceOfStorage.SetParameter(Define.DefineEnumBase.Storage.EN_STORAGE_LIST.CYLINDER, m_DicForStorage[en], strValue ,ref arGroup, false);
			}
			#endregion

			#region IO
			foreach (EN_PARAM_LIST_IO enList in Enum.GetValues(typeof(EN_PARAM_LIST_IO)))
			{
				listForGroup.Clear();
				listForGroup.Add(nIndexOfItem.ToString());

				string strItemName	= string.Empty;
				int[] arParam		= null;
				int nCount			= 0;

				switch(enList)
				{
					case EN_PARAM_LIST_IO.BACKWARD_OUT:
						strItemName		= ITEM_OUTPUT;
						m_InstanceOfCylinderDLL.GetParameter(nIndexOfItem, PARAM_LIST_IO.BACKWARD_OUT, ref nCount, ref arParam);
						listForGroup.Add(GROUP_BACKWARD);
						break;
					case EN_PARAM_LIST_IO.BACKWARD_IN:
						strItemName		= ITEM_INPUT;
						m_InstanceOfCylinderDLL.GetParameter(nIndexOfItem, PARAM_LIST_IO.BACKWARD_IN, ref nCount, ref arParam);
						listForGroup.Add(GROUP_BACKWARD);
						break;
					case EN_PARAM_LIST_IO.FORWARD_IN:
						strItemName		= ITEM_INPUT;
						m_InstanceOfCylinderDLL.GetParameter(nIndexOfItem, PARAM_LIST_IO.FORWARD_IN, ref nCount, ref arParam);
						listForGroup.Add(GROUP_FORWARD);
						break;
					case EN_PARAM_LIST_IO.FORWARD_OUT:
						strItemName		= ITEM_OUTPUT;
						m_InstanceOfCylinderDLL.GetParameter(nIndexOfItem, PARAM_LIST_IO.FORWARD_OUT, ref nCount, ref arParam);
						listForGroup.Add(GROUP_FORWARD);
						break;
				}

				strValue		= arParam == null ? "-1" : string.Join(", ", arParam);
				arGroup			= listForGroup.ToArray();
				m_InstanceOfStorage.SetParameter(Define.DefineEnumBase.Storage.EN_STORAGE_LIST.CYLINDER, strItemName, strValue, ref arGroup);
			}
			#endregion

			m_InstanceOfStorage.Save(Define.DefineEnumBase.Storage.EN_STORAGE_LIST.CYLINDER);
			return true;
		}
		
		/// <summary>
		/// 2020.09.29 by twkang [ADD] 해당 아이템의 값을 Default 값으로 설정한다
		/// </summary>
		private void SetDefaultValue(int nIndex)
		{
			int[] arParam	= null;

			m_InstanceOfCylinderDLL.SetParameter(nIndex, PARAM_LIST.NAME, DEFAULT_NAME + nIndex.ToString());
			m_InstanceOfCylinderDLL.SetParameter(nIndex, PARAM_LIST.BACKWARD_DELAY, DEFALUT_DELAY);
			m_InstanceOfCylinderDLL.SetParameter(nIndex, PARAM_LIST.BACKWARD_TIMEOUT, DEFAULT_TIMEOUT);
			m_InstanceOfCylinderDLL.SetParameter(nIndex, PARAM_LIST.ENABLE, DEFALUT_ENABLE);
			m_InstanceOfCylinderDLL.SetParameter(nIndex, PARAM_LIST.FORWARD_DELAY, DEFALUT_DELAY);
			m_InstanceOfCylinderDLL.SetParameter(nIndex, PARAM_LIST.FORWARD_TIMEOUT, DEFAULT_TIMEOUT);
			m_InstanceOfCylinderDLL.SetParameter(nIndex, PARAM_LIST.MONITORING, DEFAULT_MONITORING);
			m_InstanceOfCylinderDLL.SetParameter(nIndex, PARAM_LIST_IO.BACKWARD_IN, 0, ref arParam);
			m_InstanceOfCylinderDLL.SetParameter(nIndex, PARAM_LIST_IO.BACKWARD_OUT, 0,  ref arParam);
			m_InstanceOfCylinderDLL.SetParameter(nIndex, PARAM_LIST_IO.FORWARD_IN, 0,  ref arParam);
			m_InstanceOfCylinderDLL.SetParameter(nIndex, PARAM_LIST_IO.FORWARD_OUT, 0,  ref arParam);

			// 2025.03.12. by shkim. [ADD] INPUT CHECK를 AND 조건으로 볼지 OR 조건으로 볼지에 대한 기능 추가
			m_InstanceOfCylinderDLL.SetParameter(nIndex, PARAM_LIST.FORWARD_CHECK_LOGIC_OR, DEFALUT_DISABLE);
			m_InstanceOfCylinderDLL.SetParameter(nIndex, PARAM_LIST.BACKWARD_CHECK_LOGIC_OR, DEFALUT_DISABLE);
			// 2025.03.12. by shkim. [END]
		}
		#endregion
	}
}
