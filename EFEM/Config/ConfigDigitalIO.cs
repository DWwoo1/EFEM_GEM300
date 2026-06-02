using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using DigitalIO_;
using FileBorn_;

namespace FrameOfSystem3.Config
{
	/// <summary>
	/// 2020.06.04 by twkang [ADD] DigitalIO 의 INPUT, OUTPUT 을 관리하기 위한 클래스이다.
	/// </summary>
    public class ConfigDigitalIO
	{
		#region 열거형
		public enum EN_PARAM_DIGITALIO
		{
			ENABLE,
			NAME,
			TAG_NUMBER,
			OFFDELAY,
			ONDELAY,
			REVERSE,
			TARGET,
		}
		#endregion

		#region 상수
		private readonly string GROUP_INPUT			= "INPUT";
		private readonly string GROUP_OUTPUT		= "OUTPUT";

		private readonly string ITEM_TAG_NUMBER		= "TAG_NO";
		private readonly string ITEM_ENABLE			= "ENABLE";
		private readonly string ITEM_NAME			= "NAME";
		private readonly string ITEM_OFFDELAY		= "OFFDELAY";
		private readonly string ITEM_ONDELAY		= "ONDELAY";
		private readonly string ITEM_REVERS			= "REVERSE";
		private readonly string ITEM_TARGET			= "TARGET";

        private const int SELECTED_NONE             = -1;

		#region DefaultValue
		private readonly string DEFAULT_NAME		= "NONE";
		private readonly string DEFAULT_TAGNUMBER	= "0000";
		private const int DEFAULT_DELAY				= 100;
		private const bool DEFAULT_ENABLE			= true;
		private const bool DEFAULT_REVERSE			= false;
		private const int DEFAULT_TARGET			= -1;
		#endregion

		#endregion

		#region 변수
		Functional.Storage m_InstanceOfStorage								= null;
		DigitalIO m_InstanceOfDigitalIODLL									= null;

		private Dictionary<EN_PARAM_DIGITALIO, string> m_DicForStorage		= new Dictionary<EN_PARAM_DIGITALIO,string>();
		private Dictionary<EN_PARAM_DIGITALIO, PARAM_LIST> m_dicForItem		= new Dictionary<EN_PARAM_DIGITALIO, PARAM_LIST>();

		private List<string[]> m_listForInitialize							= new List<string[]>();

        ConfigInterlock m_InstanceOfInterlock = ConfigInterlock.GetInstance();
		#endregion
		
		#region Constructor
		private ConfigDigitalIO() { }
        private static ConfigDigitalIO m_instance     = new ConfigDigitalIO();
        public static ConfigDigitalIO GetInstance() { return m_instance;}
        #endregion

        #region Internal Interface
		/// <summary>
		/// 2020.06.04 by twkang [ADD] Storage 데이터 DigitalIO 파라미터를 초기화한다.
		/// </summary>
		private void InitializeParameter()
		{
			string strValue		= string.Empty;
			string strParamName	= string.Empty;
			bool bInput			= false;

			for(int i = 0; i < m_listForInitialize.Count; ++i)
			{
				string[] strArrGroup		= m_listForInitialize.ElementAt(i);
				string[] arGroup			= null;

				if(m_InstanceOfStorage.GetListOfGroup(Define.DefineEnumBase.Storage.EN_STORAGE_LIST.DIGITALIO, ref arGroup, strArrGroup))
                {
					Array.Resize(ref strArrGroup, 2);
                    for(int nIndex = 0, nEnd = arGroup.Length; nIndex < nEnd; ++nIndex)
				    {
				    	strArrGroup[1] = arGroup[nIndex];

				    	bInput = strArrGroup[0].Equals(GROUP_INPUT);

						int nIndexOfItem	= int.Parse(arGroup[nIndex]);

						m_InstanceOfDigitalIODLL.AddItem(bInput, nIndexOfItem);
						SetDefaultValue(bInput, nIndexOfItem);

				    	foreach(EN_PARAM_DIGITALIO en in Enum.GetValues(typeof(EN_PARAM_DIGITALIO)))
				    	{
				    		if(m_InstanceOfStorage.GetParameter(Define.DefineEnumBase.Storage.EN_STORAGE_LIST.DIGITALIO
				    			, m_DicForStorage[en]
				    			, ref strValue
				    			, ref strArrGroup))
				    		{
								m_InstanceOfDigitalIODLL.SetParameter(bInput, nIndexOfItem, m_dicForItem[en], strValue);
				    		}
				    	}
				    }
                }
			}
		}
		/// <summary>
		/// 2020.06.29 by twkang [ADD] 파라미터의 값을 Storage 데이터로 저장한다.
		/// </summary>
		private bool SaveParameterToStorage(bool bInput, int nIndexOfItem)
		{
			string strValue			= string.Empty;
			bool bValue				= false;

			string[] arGroup	= new string[] {bInput ? GROUP_INPUT : GROUP_OUTPUT, nIndexOfItem.ToString()};

			foreach(EN_PARAM_DIGITALIO en in Enum.GetValues(typeof(EN_PARAM_DIGITALIO)))
			{
				switch(en)
				{
					case EN_PARAM_DIGITALIO.ENABLE:
						m_InstanceOfDigitalIODLL.GetParameter(bInput, nIndexOfItem, m_dicForItem[en], ref bValue);
						strValue	= bValue.ToString();
						break;
					default:
						m_InstanceOfDigitalIODLL.GetParameter(bInput, nIndexOfItem, m_dicForItem[en], ref strValue);
						break;
				}
				m_InstanceOfStorage.SetParameter(Define.DefineEnumBase.Storage.EN_STORAGE_LIST.DIGITALIO, m_DicForStorage[en], strValue, ref arGroup, false);
			}
			m_InstanceOfStorage.Save(Define.DefineEnumBase.Storage.EN_STORAGE_LIST.DIGITALIO);
			return true;
		}
		/// <summary>
		/// 2020.09.29 by twkang [ADD] 해당 아이템의 값을 Default 값으로 설정한다.
		/// </summary>
		private void SetDefaultValue(bool bInput, int nIndex)
		{
			m_InstanceOfDigitalIODLL.SetParameter(bInput, nIndex, PARAM_LIST.ENABLE, DEFAULT_ENABLE);
			m_InstanceOfDigitalIODLL.SetParameter(bInput, nIndex, PARAM_LIST.NAME, DEFAULT_NAME + nIndex.ToString());
			m_InstanceOfDigitalIODLL.SetParameter(bInput, nIndex, PARAM_LIST.TAG_NO, DEFAULT_TAGNUMBER);
			m_InstanceOfDigitalIODLL.SetParameter(bInput, nIndex, PARAM_LIST.OFF_DELAY, DEFAULT_DELAY);
			m_InstanceOfDigitalIODLL.SetParameter(bInput, nIndex, PARAM_LIST.ON_DELAY, DEFAULT_DELAY);
			m_InstanceOfDigitalIODLL.SetParameter(bInput, nIndex, PARAM_LIST.REVERSE, DEFAULT_REVERSE);
			m_InstanceOfDigitalIODLL.SetParameter(bInput, nIndex, PARAM_LIST.TARGET, nIndex);
		}
        #endregion

        #region External Interface
		/// <summary>
		/// 2020.06.04 by twkang [ADD] DigitalIO 클래스를 초기화한다.
		/// </summary>
        public bool Init()
		{
			#region mapping tabel
			m_DicForStorage.Clear();
			m_DicForStorage.Add(EN_PARAM_DIGITALIO.ENABLE, ITEM_ENABLE);
			m_DicForStorage.Add(EN_PARAM_DIGITALIO.NAME, ITEM_NAME);
			m_DicForStorage.Add(EN_PARAM_DIGITALIO.OFFDELAY, ITEM_OFFDELAY);
			m_DicForStorage.Add(EN_PARAM_DIGITALIO.ONDELAY, ITEM_ONDELAY);
			m_DicForStorage.Add(EN_PARAM_DIGITALIO.REVERSE, ITEM_REVERS);
			m_DicForStorage.Add(EN_PARAM_DIGITALIO.TARGET, ITEM_TARGET);
			m_DicForStorage.Add(EN_PARAM_DIGITALIO.TAG_NUMBER, ITEM_TAG_NUMBER);

			m_dicForItem.Clear();
			m_dicForItem.Add(EN_PARAM_DIGITALIO.ENABLE, PARAM_LIST.ENABLE);
			m_dicForItem.Add(EN_PARAM_DIGITALIO.NAME, PARAM_LIST.NAME);
			m_dicForItem.Add(EN_PARAM_DIGITALIO.TAG_NUMBER, PARAM_LIST.TAG_NO);
			m_dicForItem.Add(EN_PARAM_DIGITALIO.OFFDELAY, PARAM_LIST.OFF_DELAY);
			m_dicForItem.Add(EN_PARAM_DIGITALIO.ONDELAY, PARAM_LIST.ON_DELAY);
			m_dicForItem.Add(EN_PARAM_DIGITALIO.REVERSE, PARAM_LIST.REVERSE);
			m_dicForItem.Add(EN_PARAM_DIGITALIO.TARGET, PARAM_LIST.TARGET);
			#endregion

			m_InstanceOfStorage			= Functional.Storage.GetInstance();
			m_InstanceOfDigitalIODLL	= DigitalIO.GetInstance();
			
			string[] strArrInput	= new string[]{GROUP_INPUT};
			string[] strArrOutput	= new string[]{GROUP_OUTPUT};
			
			m_listForInitialize.Clear();
			m_listForInitialize.Add(strArrInput);
			m_listForInitialize.Add(strArrOutput);

            if(false == m_InstanceOfStorage.Load(Define.DefineEnumBase.Storage.EN_STORAGE_LIST.DIGITALIO))
			{
				return false;
			}
			
			InitializeParameter();

			return true;
        }

		#region Get Information
		/// <summary>
		/// 2020.06.04 by twkang [ADD] 원하는 파라미터값을 읽어온다.
		/// </summary>
		public bool GetParameter<T>(bool bInput, int nIndex, EN_PARAM_DIGITALIO enParam, ref T tValue)
		{
			if (m_dicForItem.ContainsKey(enParam))
			{
				return m_InstanceOfDigitalIODLL.GetParameter(bInput, nIndex, m_dicForItem[enParam], ref tValue);
			}
			return false;
		}
		/// <summary>
		/// 2020.06.04 by twkang [ADD] Input 또는 Output 그룹의 아이템 수를 읽어온다.
		/// </summary>
		public bool GetListOfItem(bool nInput, ref int[] nArr)
		{
			return m_InstanceOfDigitalIODLL.GetListOfItem(nInput, ref nArr);
		}

		/// <summary>
		/// 2020.06.04 by twkang [ADD] Input 또는 Output 그룹의 Pulse 값을 읽어온다.
		/// </summary>
		public bool ReadItem(bool bInput, int nIndex)
		{
            if (nIndex == SELECTED_NONE)
            {
                return false;
            }

			if (bInput)
			{
				return m_InstanceOfDigitalIODLL.ReadInput(nIndex);
			}

			return m_InstanceOfDigitalIODLL.ReadOutput(nIndex);
		}
        /// <summary>
        /// 2021.11.24 by wdw [ADD] Digital 아이템의 Name 를 반환한다.
        /// </summary>
        public string GetName(bool bInput, int nIndex)
        {
            string strName = string.Empty;

            m_InstanceOfDigitalIODLL.GetParameter(bInput, nIndex, PARAM_LIST.NAME, ref strName);

            return strName;
        }
		/// <summary>
		/// 2020.06.29 by twkang [ADD] Input, 또는 Output 아이템들의 NameList 를 반환한다.
		/// </summary>
		public bool GetListOfName(bool bInput, ref string[] strArrName)
		{
			int[] arIndexes			= null;
			if(false == m_InstanceOfDigitalIODLL.GetListOfItem(bInput, ref arIndexes))
			{
				return false;
			}

			strArrName				= new string[arIndexes.Length];

			for(int nIndex = 0, nEnd = arIndexes.Length; nIndex < nEnd; nIndex++)
			{
				string strName		= string.Empty;
				m_InstanceOfDigitalIODLL.GetParameter(bInput, arIndexes[nIndex], PARAM_LIST.NAME, ref strName);
				strArrName[nIndex]	= strName;
			}

			return true;
		}
		#endregion

		#region Set Information
		/// <summary>
		/// 2020.06.04 by twkang [ADD] 원하는 파라미터값을 변경한다.
		/// </summary>
		public bool SetParameter<T>(bool bInput, int nIndex, EN_PARAM_DIGITALIO enParam, T tValue)
		{
			string[] strArrGroup	= new string[] {bInput ? GROUP_INPUT : GROUP_OUTPUT, nIndex.ToString()};

			if (m_InstanceOfStorage.SetParameter(Define.DefineEnumBase.Storage.EN_STORAGE_LIST.DIGITALIO, m_DicForStorage[enParam], tValue, ref strArrGroup))
			{
				if (m_dicForItem.ContainsKey(enParam))
				{
					return m_InstanceOfDigitalIODLL.SetParameter(bInput, nIndex, m_dicForItem[enParam], tValue);
				}
			}
			return false;
		}
		/// <summary>
        /// 2022.02.15 by WDW [ADD] INTERLOCK 추가.
        /// 2020.06.04 by twkang [ADD] Output Pulse 값을 정한다.
		/// </summary>
        public DIO_RESULT WriteOutput(bool bValue, int nIndex)
		{
            string strInterlockDiscription = string.Empty;
            DIO_RESULT enReuslt = m_InstanceOfDigitalIODLL.WriteOutput(nIndex, bValue);
            if (enReuslt == DIO_RESULT.OCCUR_INTERLOCK)
            {
                string strTitle = "INTERLOCK DO " + GetName(false, nIndex);
                strInterlockDiscription = m_InstanceOfInterlock.GetDOLastInterference(nIndex);
                Views.Functional.Form_MessageBox.GetInstance().ShowMessage(strInterlockDiscription, strTitle);
            }
            else if (enReuslt != DIO_RESULT.OK)
            {
                Views.Functional.Form_MessageBox.GetInstance().ShowMessage(enReuslt.ToString());
            }
            return enReuslt;
		}
		/// <summary>
		/// 2020.12.20 by twkang [ADD] Output Pulse 값을 반전시킨다.
		/// </summary>
		public void ToggleOutput(int nIndex)
		{
			bool bReadValue	= m_InstanceOfDigitalIODLL.ReadOutput(nIndex);

			m_InstanceOfDigitalIODLL.WriteOutput(nIndex, !bReadValue);
		}
		#endregion

		/// <summary>
		/// 2020.06.29 by twkang [ADD] DigitalIO 아이템을 추가한다.
		/// </summary>
		public int AddDigitalItem(bool bInput)
		{
			int nItemNum			= m_InstanceOfDigitalIODLL.AddItem(bInput);
			SetDefaultValue(bInput, nItemNum);

			string[] arGroup		= new string[] {bInput ? GROUP_INPUT : GROUP_OUTPUT};
			string[] arData			= null;

			if(FileBorn.GetInstance().GetItemFrame(BORN_LIST.DIGITALIO, ref arGroup, nItemNum.ToString(), ref arData)
				&& m_InstanceOfStorage.AddGroupAndItem(Define.DefineEnumBase.Storage.EN_STORAGE_LIST.DIGITALIO, ref arData))
			{
				SaveParameterToStorage(bInput, nItemNum); 
				return nItemNum;
			}

			m_InstanceOfDigitalIODLL.RemoveItem(bInput, nItemNum);
			return -1;
		}
		/// <summary>
		/// 2022.07.20 by junho [ADD] List에 있는 항목들로 아이템을 추가한다.
		/// </summary>
		public bool AddDigitalItemArray(bool bInput, Dictionary<int, string> itemList)
		{
			foreach (KeyValuePair<int, string> kvp in itemList)
			{
				int nItemNum = m_InstanceOfDigitalIODLL.AddItem(bInput);
				SetDefaultValue(bInput, nItemNum);

				m_InstanceOfDigitalIODLL.SetParameter(bInput, nItemNum, PARAM_LIST.NAME, kvp.Value);
				m_InstanceOfDigitalIODLL.SetParameter(bInput, nItemNum, PARAM_LIST.TAG_NO, kvp.Key);

				string[] arGroup = new string[] { bInput ? GROUP_INPUT : GROUP_OUTPUT };
				string[] arData = null;

				if (FileBorn.GetInstance().GetItemFrame(BORN_LIST.DIGITALIO, ref arGroup, nItemNum.ToString(), ref arData)
				&& m_InstanceOfStorage.AddGroupAndItem(Define.DefineEnumBase.Storage.EN_STORAGE_LIST.DIGITALIO, ref arData))
				{
					SaveParameterToStorage(bInput, nItemNum);
					continue;
				}

				m_InstanceOfDigitalIODLL.RemoveItem(bInput, nItemNum);
				return false;
			}

			return true;
		}
		/// <summary>
		/// 2020.07.20 by twkang [ADD] DigitalIO 아이템을 추가한다.
		/// </summary>
		public bool AddDigitalItem(bool bInput, int nIndex)
		{
			if(false == m_InstanceOfDigitalIODLL.AddItem(bInput, nIndex))
			{
				return false;
			}

			SetDefaultValue(bInput, nIndex);

			string[] arGroup		= new string[] { bInput ? GROUP_INPUT : GROUP_OUTPUT };
			string[] arData			= null;

			if (FileBorn.GetInstance().GetItemFrame(BORN_LIST.DIGITALIO, ref arGroup, nIndex.ToString(), ref arData)
				&& m_InstanceOfStorage.AddGroupAndItem(Define.DefineEnumBase.Storage.EN_STORAGE_LIST.DIGITALIO, ref arData))
			{
				return SaveParameterToStorage(bInput, nIndex);
			}
			
			return false;
		}
		/// <summary>
		/// 2020.07.01 by twkang [ADD] 아이템을 삭제한다.
		/// </summary>
		public bool RemoveItem(bool bInput, int nIndexOfItem)
		{
			string[] arGroup	= new string[] {bInput ? GROUP_INPUT : GROUP_OUTPUT, nIndexOfItem.ToString()};

			if(m_InstanceOfStorage.RemoveGroup(Define.DefineEnumBase.Storage.EN_STORAGE_LIST.DIGITALIO, ref arGroup))
			{
				return m_InstanceOfDigitalIODLL.RemoveItem(bInput, nIndexOfItem);
			}
			return false;
		}
        #endregion
    }
}
