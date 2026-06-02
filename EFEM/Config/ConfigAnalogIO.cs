using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using FileBorn_;
using AnalogIO_;

namespace FrameOfSystem3.Config
{
	public class ConfigAnalogIO
	{
		private ConfigAnalogIO() {}

		#region 싱글톤
		private static ConfigAnalogIO _Instance	= new ConfigAnalogIO();
		public static ConfigAnalogIO GetInstance()
		{
			return _Instance;
		}		
		#endregion

		#region 열거형
		public enum EN_PARAM_ANALOGIO
		{
			ENABLE,
			NAME,
			BIT,
			TAG_NO,
			TARGET,
			MIN,
			MAX,
            RAW_DATA_TYPE   // 2022.10.31. by shkim. [ADD] 모듈에 따라 분류하기 위한 RAW DATA TYPE 추가
		}
		public enum EN_TABLE_TYPE
		{
			VOLT,
			VALUE,
		}
		#endregion

		#region 상수
		private const int MAX_ITEM_COUNT				= 32;
		private const int MAX_TABLE_COUNT				= 20;

		private readonly string GROUP_INPUT				= "INPUT";
		private readonly string GROUP_OUTPUT			= "OUTPUT";

		private readonly string ITEM_ENABLE				= "ENABLE";
		private readonly string ITEM_NAME				= "NAME";
		private readonly string ITEM_BIT				= "BIT";
		private readonly string ITEM_TARGET				= "TARGET";
		private readonly string ITEM_TAG_NUMBER			= "TAG_NO";
		private readonly string ITEM_MIN				= "MIN";
		private readonly string ITEM_MAX				= "MAX";
        private readonly string RAW_DATA_TYPE           = "RAW_DATA_TYPE";    // 2022.10.31. by shkim. [ADD] 모듈에 따라 분류하기 위한 RAW DATA TYPE 추가

		private readonly string ITEM_VOLT				= "VOLT";
		private readonly string ITEM_VLAUE				= "VALUE";

		#region Default Value
		private readonly string DEFAULT_NAME			= "NONE";
		private readonly string DEFAULT_TAG_NUMBER		= "0000";
		private const bool DEFAULT_ENABLE				= true;
		private const int DEFAULT_TARGET				= -1;
		private const int DEFAULT_CTRL_BIT				= 0;
		private const int DEFAULT_VLOT_MIN				= 0;
		private const int DEFAULT_VOLT_MAX				= 0;
		private const int DEFAULT_TABLE_VALUE			= 0;
        private const int DEFAULT_RAW_DATA_TYPE         = 0; // 2022.10.31. by shkim. [ADD] 모듈에 따라 분류하기 위한 RAW DATA TYPE 추가
		#endregion

		#endregion

		#region 변수
		Functional.Storage m_InstanceOfStorage							= null;
		AnalogIO m_InstanceOfAnalogIODLL								= null;

		private Dictionary<EN_PARAM_ANALOGIO, string> m_DicForStorage		= new Dictionary<EN_PARAM_ANALOGIO,string>();
		private Dictionary<EN_PARAM_ANALOGIO, PARAM_LIST> m_DicForItem		= new Dictionary<EN_PARAM_ANALOGIO, PARAM_LIST>();
		private List<string[]> m_listForInitialize						= new List<string[]>();
		#endregion

		#region 외부인터페이스
		/// <summary>
		/// 2020.06.08 by twkang [ADD] AnalogIO 클래스를 초기화한다.
		/// </summary>
		public bool Init()
		{
			MakeMappingTable();

			m_InstanceOfStorage		= Functional.Storage.GetInstance();
			m_InstanceOfAnalogIODLL	= AnalogIO.GetInstance();

			string[] strArrInput	= new string[] { GROUP_INPUT};
			string[] strArrOutput	= new string[] { GROUP_OUTPUT};
			
			m_listForInitialize.Clear();
			m_listForInitialize.Add(strArrInput);
			m_listForInitialize.Add(strArrOutput);

			if(false == m_InstanceOfStorage.Load(Define.DefineEnumBase.Storage.EN_STORAGE_LIST.ANALOG))
			{
				return false;
			}
			
			InitializeIOParameter();

			if(false == m_InstanceOfStorage.Load(Define.DefineEnumBase.Storage.EN_STORAGE_LIST.ANALOG_LOOKUPTABLE))
			{
				return false;
			}
			
			InitializeTableParameter();

			return true;
		}
		/// <summary>
		/// 2020.06.08 by twkang [ADD] 원하는 파라미터 값을 읽어온다.
		/// </summary>
		public bool GetParameter<T>(bool bInput, int nIndex, EN_PARAM_ANALOGIO enParam, ref T tValue)
		{
			if(m_DicForItem.ContainsKey(enParam))
			{
				return m_InstanceOfAnalogIODLL.GetParameter(bInput, nIndex, m_DicForItem[enParam], ref tValue);
			}
			
			return false;
		}
		/// <summary>
		/// 2020.06.08 by twkang [ADD] 원하는 파라미터 값을 변경한다.
		/// </summary>
		public bool SetParameter<T>(bool bInput, int nIndex, EN_PARAM_ANALOGIO enParam, T tValue)
		{
			string[] strArrGroup	= new string[] {bInput ? GROUP_INPUT : GROUP_OUTPUT, nIndex.ToString()};

			if(m_InstanceOfStorage.SetParameter(Define.DefineEnumBase.Storage.EN_STORAGE_LIST.ANALOG
				, m_DicForStorage[enParam]
				, tValue
				, ref strArrGroup))
			{
				if(m_DicForItem.ContainsKey(enParam))
				{
					return m_InstanceOfAnalogIODLL.SetParameter(bInput, nIndex, m_DicForItem[enParam], tValue);					
				}
			}
			return false;
		}
		/// <summary>
		/// 2020.11.05 by twkang [ADD] AnalogLookupTable 의 데이터 값을 변경한다.
		/// </summary>
		public bool SetDataTable(bool bInput, int nIndexOfItem, int nIndexOfTable, EN_TABLE_TYPE enType, double dbData)
		{
			double dbVolt			= 0;
			double dbValue			= 0;

			if(false == m_InstanceOfAnalogIODLL.GetDataFromTable(bInput, nIndexOfItem, nIndexOfTable, ref dbVolt, ref dbValue))
			{
				return false;
			}

			string[] strArrGroup	= new string[] {bInput ? GROUP_INPUT : GROUP_OUTPUT
				, nIndexOfItem.ToString()
				, nIndexOfTable.ToString()};

			switch(enType)
			{
				case EN_TABLE_TYPE.VALUE:
					dbValue		= dbData;
					break;
				case EN_TABLE_TYPE.VOLT:
					dbVolt		= dbData;
					break;
			}

			if (false == m_InstanceOfAnalogIODLL.SetDataToTable(bInput, nIndexOfItem, nIndexOfTable, dbVolt, dbValue))
			{
				return false;
			}
			
			switch(enType)
			{
				case EN_TABLE_TYPE.VALUE:
					return m_InstanceOfStorage.SetParameter(Define.DefineEnumBase.Storage.EN_STORAGE_LIST.ANALOG_LOOKUPTABLE, ITEM_VLAUE, dbValue, ref strArrGroup);
				case EN_TABLE_TYPE.VOLT:
					return m_InstanceOfStorage.SetParameter(Define.DefineEnumBase.Storage.EN_STORAGE_LIST.ANALOG_LOOKUPTABLE, ITEM_VOLT, dbVolt, ref strArrGroup);
			}
			return false;
		}
		/// <summary>
		/// 2020.06.08 by twkang [ADD] Lookup Table 의 데이터를 반환한다.
		/// </summary>
		public bool GetDataFromTable(bool bInput, int nIndexOfItem, int nIndexOfTable, ref double dbVolt, ref double dbValue)
		{
			return m_InstanceOfAnalogIODLL.GetDataFromTable(bInput, nIndexOfItem, nIndexOfTable, ref dbVolt, ref dbValue);
		}
		/// <summary>
		/// 2020.06.29 by twkang [ADD] AnalogIO 아이템을 추가한다.
		/// </summary>
		public int AddAnalogItem(bool bInput)
		{
			int nItemNum		= m_InstanceOfAnalogIODLL.AddItem(bInput);
			SetDefaultValue(bInput, nItemNum);

			string[] arGroup	= new string[] {bInput ? GROUP_INPUT : GROUP_OUTPUT};
			string[] arData		= null;

			if(FileBorn.GetInstance().GetItemFrame(BORN_LIST.ANALOG, ref arGroup, nItemNum.ToString(), ref arData)
				&& m_InstanceOfStorage.AddGroupAndItem(Define.DefineEnumBase.Storage.EN_STORAGE_LIST.ANALOG, ref arData))
			{
				SaveParameterToStorage(bInput, nItemNum); 
				return nItemNum;
			}

			m_InstanceOfAnalogIODLL.RemoveItem(bInput, nItemNum);
			return -1;
		}
		/// <summary>
		/// 2022.07.20 by junho [ADD] List에 있는 항목들로 아이템을 추가한다.
		/// </summary>
		public bool AddAnalogItemArray(bool isInput, Dictionary<int, string> itemList)
		{
			foreach (KeyValuePair<int, string> kvp in itemList)
			{
				int nItemNum = m_InstanceOfAnalogIODLL.AddItem(isInput);
				SetDefaultValue(isInput, nItemNum);

				m_InstanceOfAnalogIODLL.SetParameter(isInput, nItemNum, PARAM_LIST.NAME, kvp.Value);
				m_InstanceOfAnalogIODLL.SetParameter(isInput, nItemNum, PARAM_LIST.TARGET, kvp.Key);

				string[] arGroup = new string[] { isInput ? GROUP_INPUT : GROUP_OUTPUT };
				string[] arData = null;

				if (FileBorn.GetInstance().GetItemFrame(BORN_LIST.ANALOG, ref arGroup, nItemNum.ToString(), ref arData)
					&& m_InstanceOfStorage.AddGroupAndItem(Define.DefineEnumBase.Storage.EN_STORAGE_LIST.ANALOG, ref arData))
				{
					SaveParameterToStorage(isInput, nItemNum);
					continue;
				}

				m_InstanceOfAnalogIODLL.RemoveItem(isInput, nItemNum);
				return false;
			}

			return true;
		}
		/// <summary>
		/// 2020.07.20 by twkang [ADD] AnalogIO 아이템을 추가한다.
		/// </summary>
		public bool AddAnalogItem(bool bInput, int nIndexOfItem)
		{
			if(nIndexOfItem < 0 || false == m_InstanceOfAnalogIODLL.AddItem(bInput, nIndexOfItem))
			{
				return false;
			}
			SetDefaultValue(bInput, nIndexOfItem);

			string[] arGroup	= new string[] { bInput ? GROUP_INPUT : GROUP_OUTPUT };
			string[] arData		= null;

			if (FileBorn.GetInstance().GetItemFrame(BORN_LIST.ANALOG, ref arGroup, nIndexOfItem.ToString(), ref arData)
				&& m_InstanceOfStorage.AddGroupAndItem(Define.DefineEnumBase.Storage.EN_STORAGE_LIST.ANALOG, ref arData))
			{
				return SaveParameterToStorage(bInput, nIndexOfItem);
			}
			return false;
		}
		/// <summary>
		/// 2020.07.01 by twkang [ADD] 아이템을 삭제한다.
		/// </summary>
		public bool RemoveItem(bool bInput, int nIndexOfItem)
		{
			string[] arGroup	= new string[] {bInput ? GROUP_INPUT : GROUP_OUTPUT, nIndexOfItem.ToString()};

			if(m_InstanceOfStorage.RemoveGroup(Define.DefineEnumBase.Storage.EN_STORAGE_LIST.ANALOG, ref arGroup))
			{
				m_InstanceOfStorage.RemoveGroup(Define.DefineEnumBase.Storage.EN_STORAGE_LIST.ANALOG_LOOKUPTABLE, ref arGroup);
				return m_InstanceOfAnalogIODLL.RemoveItem(bInput, nIndexOfItem);
			}
			return false;
		}
		/// <summary>
		/// 2020.06.08 by twkang [ADD] Input 또는 Output 그룹의 아이템 수를 읽어온다.
		/// </summary>
		public bool GetListOfItem(bool bInput, ref int[] nArr)
		{			
			return m_InstanceOfAnalogIODLL.GetListOfItem(bInput, ref nArr);
		}
		/// <summary>
		/// 2020.06.08 by twkang [ADD] Input 또는 Output 그룹의 이름 리스트를 반환한다.
		/// </summary>
		public bool GetListOfName(bool bInput, ref string[] arNameList)
		{
			int[] arIndexes	= null;
			if(false == m_InstanceOfAnalogIODLL.GetListOfItem(bInput, ref arIndexes))
			{
				return false;
			}

			arNameList				= new string[arIndexes.Length];

			for(int nIndex = 0, nEnd = arIndexes.Length; nIndex < nEnd; ++nIndex)
			{
				string strName		= string.Empty;
				m_InstanceOfAnalogIODLL.GetParameter(bInput, arIndexes[nIndex], PARAM_LIST.NAME, ref strName);
				arNameList[nIndex]	= strName;
			}
			return true;
		}
        public string GetName(bool bInput, int nIndex)
        {
            string strName = string.Empty;

            m_InstanceOfAnalogIODLL.GetParameter(bInput, nIndex, PARAM_LIST.NAME, ref strName);

            return strName;
        }
		/// <summary>
		/// 2020.06.08 by twkang [ADD] Input Item 의 Volt 값을 반환한다.
		/// </summary>
		public double ReadInputVolt(int nIndexOfItem)
		{
			return m_InstanceOfAnalogIODLL.ReadInputVolt(nIndexOfItem);
		}
		/// <summary>
		/// 2020.06.08 by twkang [ADD] Input Item 의 Value 값을 반환한다.
		/// </summary>
		public double ReadInputValue(int nIndexOfItem)
		{			
			return m_InstanceOfAnalogIODLL.ReadInputValue(nIndexOfItem);
		}
		/// <summary>
		/// 2020.06.08 by twkang [ADD] Output Item 의 Volt 값을 반환한다.
		/// </summary>
		public double ReadOutputVolt(int nIndexOfItem)
		{
			return m_InstanceOfAnalogIODLL.ReadOutputVolt(nIndexOfItem);
		}
		public bool GetDataAll(bool bInput, int nIndexOfItem, ref double[] dbVolt, ref double[] dbValue)
		{			
			return m_InstanceOfAnalogIODLL.GetDataAll(bInput, nIndexOfItem, ref dbVolt, ref dbValue);
		}
		/// <summary>
		/// 2020.06.08 by twkang [ADD] Output Item 의 Value 값을 반환하다.
		/// </summary>
		public double ReadOutputValue(int nIndexOfItem)
		{
			return m_InstanceOfAnalogIODLL.ReadOutputValue(nIndexOfItem);
		}
		/// <summary> 
		/// 2020.06.08 by twkang [ADD] 
		/// </summary>
		public int GetColumnOfTable()
		{
			return m_InstanceOfAnalogIODLL.GetColumnOfTable();
		}
		/// <summary>
		/// 2020.06.08 by twkang [ADD] Output Value 값을 설정한다.
		/// </summary>
		public void WriteOutputValue(int nIndexOfItem, double dbValue)
		{
			m_InstanceOfAnalogIODLL.WriteOutputValue(nIndexOfItem, dbValue);
		}
		/// <summary>
		/// 2020.06.08 by twkang [ADD] Output Volt 값을 설정한다.
		/// </summary>
		public void WriteOutputVolt(int nIndexOfItem, double dbVolt)
		{
			m_InstanceOfAnalogIODLL.WriteOutputVolt(nIndexOfItem, dbVolt);
		}
        /// <summary>
        /// 2022.03.03 by wdw [ADD] 해당 인덱스 테이블 데이터의 최소/최대값를 반환한다.
        /// </summary>
        public bool GetDataMinMaxFromTable(bool bInput, int nIndexOfItem, ref double dMin, ref double dMax)
        {
            double[] dbVolt = new double[] { };
            double[] dbValue = new double[] { };
            if (m_InstanceOfAnalogIODLL.GetDataAll(bInput, nIndexOfItem, ref dbVolt, ref dbValue))
            {
                dMin = dbValue.Min();
                dMax = dbValue.Max();
                return true;
            }
            return false;
        }
		#endregion

		#region 내부인터페이스
		/// <summary>
		/// 2020.06.08 by twkang [ADD] Storage 데이터로 AnalogIO 파라미터를 초기화한다.
		/// </summary>
		private void InitializeIOParameter()
		{
			string strValue		= string.Empty;
			string strParamName	= string.Empty;

			for(int i = 0; i < m_listForInitialize.Count; ++i)
			{
				string[] strArrGroup	= m_listForInitialize.ElementAt(i);
				string[] arGroup		= null;

				if(m_InstanceOfStorage.GetListOfGroup(Define.DefineEnumBase.Storage.EN_STORAGE_LIST.ANALOG, ref arGroup, strArrGroup))
                {
					Array.Resize(ref strArrGroup, 2);

                    for(int nIndex = 0, nEnd = arGroup.Length; nIndex < nEnd; ++nIndex)
				    {
				    	//Group 번호
				    	strArrGroup[1] = arGroup[nIndex];
				    	
				    	//INPUT, OUTPUT
				    	bool bInput		= strArrGroup[0].Equals(GROUP_INPUT);

						int nItemNum	= int.Parse(arGroup[nIndex]);

				    	m_InstanceOfAnalogIODLL.AddItem(bInput, nItemNum);
						SetDefaultValue(bInput, nItemNum);

				    	foreach(EN_PARAM_ANALOGIO en in Enum.GetValues(typeof(EN_PARAM_ANALOGIO)))
				    	{
				    		if(m_InstanceOfStorage.GetParameter(Define.DefineEnumBase.Storage.EN_STORAGE_LIST.ANALOG
				    			, m_DicForStorage[en]
				    			, ref strValue
				    			, ref strArrGroup))
				    		{
				    			m_InstanceOfAnalogIODLL.SetParameter(bInput, nItemNum, m_DicForItem[en], strValue);
				    		}
				    	}
				    }
                }
			}
		}
		/// <summary>
		/// 2020.06.08 by twkang [ADD] Storage 데이터로 AnalogIOTable 파라미터를 초기화한다.
		/// </summary>
		private void InitializeTableParameter()
		{
			for(int nCount = 0; nCount < 2; ++nCount)
			{
				bool bInput	= nCount == 0;

				string[] arLevel				= new string[] {bInput ? GROUP_INPUT : GROUP_OUTPUT};
				string[] arTableGroup			= null;
				int[] arAnalogItem				= null;
				List<string> listOfTableGroup	= new List<string>();

				m_InstanceOfStorage.GetListOfGroup(Define.DefineEnumBase.Storage.EN_STORAGE_LIST.ANALOG_LOOKUPTABLE, ref arTableGroup, arLevel);
				m_InstanceOfAnalogIODLL.GetListOfItem(bInput, ref arAnalogItem);

				// 아이템이 없을경우
				if(null == arTableGroup && null == arAnalogItem)
				{
					continue;
				}
				// Analog 아이템만 있을경우
				else if(null == arTableGroup)
				{
					foreach(int nItem in arAnalogItem)
					{
						AddTableItem(bInput, nItem);
					}
					continue;
				}
				else if(null == arAnalogItem)
				{
					foreach(string str in arTableGroup)
					{
						RemoveTableGroup(bInput, str);
					}
					continue;
				}

				foreach(string str in arTableGroup)
				{
					if(false == listOfTableGroup.Contains(str))
					{
						listOfTableGroup.Add(str);
					}
				}

				for(int nIndex = 0, nEnd = arAnalogItem.Length; nIndex < nEnd; ++nIndex)
				{
					SetDefaultTableValue(bInput, arAnalogItem[nIndex]);

					string strGroup		= arAnalogItem[nIndex].ToString();

					if(listOfTableGroup.Remove(strGroup))
					{
						for(int i = 0; i < MAX_TABLE_COUNT; ++i)
						{
							arLevel		= new string[] {bInput ? GROUP_INPUT : GROUP_OUTPUT, strGroup, i.ToString()};
							double dbValue	= 0;
							double dbVolt	= 0;

							m_InstanceOfStorage.GetParameter(Define.DefineEnumBase.Storage.EN_STORAGE_LIST.ANALOG_LOOKUPTABLE, ITEM_VLAUE, ref dbValue, ref arLevel);
							m_InstanceOfStorage.GetParameter(Define.DefineEnumBase.Storage.EN_STORAGE_LIST.ANALOG_LOOKUPTABLE, ITEM_VOLT, ref dbVolt, ref arLevel);
							m_InstanceOfAnalogIODLL.SetDataToTable(bInput, arAnalogItem[nIndex], i, dbVolt, dbValue);
						}
					}
					else
					{
						AddTableItem(bInput, arAnalogItem[nIndex]);
					}
				}

				foreach(string str in listOfTableGroup)
				{
					RemoveTableGroup(bInput, str);
				}
			}
		}
		/// <summary>
		/// 
		/// </summary>
		public void AddTableItem(bool bInput, int nItem)
		{
			string[] arGroup	= new string[] {bInput ? GROUP_INPUT : GROUP_OUTPUT, nItem.ToString()};
			string[] arData		= null;

			for(int nIndex = 0, nEnd = MAX_TABLE_COUNT; nIndex < nEnd; ++nIndex)
			{
				if (FileBorn.GetInstance().GetItemFrame(BORN_LIST.ANALOG_LOOKUPTABLE, ref arGroup, nIndex.ToString(), ref arData)
					&& m_InstanceOfStorage.AddGroupAndItem(Define.DefineEnumBase.Storage.EN_STORAGE_LIST.ANALOG_LOOKUPTABLE, ref arData))
				{
					SetDefaultTableValue(bInput, nIndex);
				}
			}
		}
		/// <summary>
		/// 2020.11.05 by twkang [ADD] Analog Table 아이템을 삭제한다.
		/// </summary>
		private bool RemoveTableGroup(bool bInput, string strGroup)
		{
			string[] arGroup	= new string[] {bInput ? GROUP_INPUT : GROUP_OUTPUT, strGroup};
			return m_InstanceOfStorage.RemoveGroup(Define.DefineEnumBase.Storage.EN_STORAGE_LIST.ANALOG_LOOKUPTABLE, ref arGroup);
		}
		/// <summary>
		/// 2020.06.29 by twkang [ADD] 파라미터의 값을 Storage 데이터로 저장한다.
		/// </summary>
		private bool SaveParameterToStorage(bool bInput, int nIndexOfItem)
		{
			string strValue		= string.Empty;
			bool bValue			= false;
			string[] arGroup	= new string[] {bInput ? GROUP_INPUT : GROUP_OUTPUT, nIndexOfItem.ToString()};

			foreach(EN_PARAM_ANALOGIO en in Enum.GetValues(typeof(EN_PARAM_ANALOGIO)))
			{
				switch(en)
				{
					case EN_PARAM_ANALOGIO.ENABLE:
						m_InstanceOfAnalogIODLL.GetParameter(bInput, nIndexOfItem, m_DicForItem[en], ref bValue);
						strValue	= bValue.ToString();
						break;
					default:
						m_InstanceOfAnalogIODLL.GetParameter(bInput, nIndexOfItem, m_DicForItem[en], ref strValue);
						break;
				}
				
				m_InstanceOfStorage.SetParameter(Define.DefineEnumBase.Storage.EN_STORAGE_LIST.ANALOG, m_DicForStorage[en], strValue, ref arGroup, false);
			}
			m_InstanceOfStorage.Save(Define.DefineEnumBase.Storage.EN_STORAGE_LIST.ANALOG);
			return true;
		}
		/// <summary>
		/// 2020.06.29 by twkang [ADD] 클래스에서 사용할 Mapping Table 을 만든다.
		/// </summary>
		private void MakeMappingTable()
		{
			#region Parameter
			m_DicForItem.Clear();
			m_DicForItem.Add(EN_PARAM_ANALOGIO.ENABLE, PARAM_LIST.ENABLE);
			m_DicForItem.Add(EN_PARAM_ANALOGIO.NAME, PARAM_LIST.NAME);
			m_DicForItem.Add(EN_PARAM_ANALOGIO.BIT, PARAM_LIST.CTRL_BIT);
			m_DicForItem.Add(EN_PARAM_ANALOGIO.TARGET, PARAM_LIST.TARGET);
			m_DicForItem.Add(EN_PARAM_ANALOGIO.MIN, PARAM_LIST.VOLT_MIN);
			m_DicForItem.Add(EN_PARAM_ANALOGIO.MAX, PARAM_LIST.VOLT_MAX);
            m_DicForItem.Add(EN_PARAM_ANALOGIO.TAG_NO, PARAM_LIST.TAG_NO);
            m_DicForItem.Add(EN_PARAM_ANALOGIO.RAW_DATA_TYPE, PARAM_LIST.RAW_DATA_TYPE);       // 2022.10.31. by shkim. [ADD] 모듈에 따라 분류하기 위한 RAW DATA TYPE 추가
			#endregion

			#region Storage
			m_DicForStorage.Clear();
			m_DicForStorage.Add(EN_PARAM_ANALOGIO.TAG_NO, ITEM_TAG_NUMBER);
			m_DicForStorage.Add(EN_PARAM_ANALOGIO.ENABLE, ITEM_ENABLE);
			m_DicForStorage.Add(EN_PARAM_ANALOGIO.NAME, ITEM_NAME);
			m_DicForStorage.Add(EN_PARAM_ANALOGIO.BIT, ITEM_BIT);
			m_DicForStorage.Add(EN_PARAM_ANALOGIO.TARGET, ITEM_TARGET);
			m_DicForStorage.Add(EN_PARAM_ANALOGIO.MIN, ITEM_MIN);
            m_DicForStorage.Add(EN_PARAM_ANALOGIO.MAX, ITEM_MAX);
            m_DicForStorage.Add(EN_PARAM_ANALOGIO.RAW_DATA_TYPE, RAW_DATA_TYPE);
			#endregion
		}
		/// <summary>
		/// 2020.09.29 by twkang [ADD] 해당 아이템을 Default 값으로 설정한다.
		/// </summary>
		private void SetDefaultValue(bool bInput, int nIndex)
		{
			m_InstanceOfAnalogIODLL.SetParameter(bInput, nIndex, PARAM_LIST.CTRL_BIT, DEFAULT_CTRL_BIT);
			m_InstanceOfAnalogIODLL.SetParameter(bInput, nIndex, PARAM_LIST.ENABLE, DEFAULT_ENABLE);
			m_InstanceOfAnalogIODLL.SetParameter(bInput, nIndex, PARAM_LIST.NAME, DEFAULT_NAME + nIndex.ToString());
			m_InstanceOfAnalogIODLL.SetParameter(bInput, nIndex, PARAM_LIST.TARGET, DEFAULT_TARGET);
			m_InstanceOfAnalogIODLL.SetParameter(bInput, nIndex, PARAM_LIST.VOLT_MAX, DEFAULT_VOLT_MAX);
			m_InstanceOfAnalogIODLL.SetParameter(bInput, nIndex, PARAM_LIST.VOLT_MIN, DEFAULT_VLOT_MIN);
            m_InstanceOfAnalogIODLL.SetParameter(bInput, nIndex, PARAM_LIST.TAG_NO, DEFAULT_TAG_NUMBER);
            m_InstanceOfAnalogIODLL.SetParameter(bInput, nIndex, PARAM_LIST.RAW_DATA_TYPE, DEFAULT_RAW_DATA_TYPE);   // 2022.10.31. by shkim. [ADD] 모듈에 따라 분류하기 위한 RAW DATA TYPE 추가
		}
		/// <summary>
		/// 2020.11.04 by twkang [ADD] Analog Table 아이템 값을 Default 값으로 설정한다.
		/// </summary>
		private void SetDefaultTableValue(bool bInput, int nIndex)
		{
			for (int nTableIndex = 0; nTableIndex < MAX_TABLE_COUNT; ++nTableIndex)
			{
				m_InstanceOfAnalogIODLL.SetDataToTable(bInput, nIndex, nTableIndex, DEFAULT_TABLE_VALUE, DEFAULT_TABLE_VALUE);
			}
		}
		#endregion
	}
}
