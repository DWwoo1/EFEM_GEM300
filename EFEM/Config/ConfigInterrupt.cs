using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Interrupt_;
using FileBorn_;

namespace FrameOfSystem3.Config
{
	public class ConfigInterrupt
	{
		private ConfigInterrupt() {}

		#region 싱글톤
		private static ConfigInterrupt _Instance	= new ConfigInterrupt();
		public static ConfigInterrupt GetInstance()
		{
			return _Instance;
		}
		#endregion

		#region 열거형
		public enum EN_PARAM_INTERRUPT
		{
			ACTION,
			CODE_ALARM,
			CONDITION,
			ENABLE,
			NAME,
			TARGET,
            DO_INDEX,
        }
		#endregion

		#region 상수
		private const int INITIALIZE_PARAM				= -1;

		private readonly string ITEM_ACTION				= "ACTION";
		private readonly string ITEM_ALARMCODE			= "ALARMCODE";
		private readonly string ITEM_CONDITION			= "CONDITION";
		private readonly string ITEM_ENABLE				= "ENABLE";
		private readonly string ITEM_NAME				= "NAME";
		private readonly string ITEM_INPUTPORT			= "INPUTPORT";
        private readonly string ITEM_DIGITALOUTPUT      = "DIGITALOUTPUT";

		#region Default Value
		private readonly string DEFAULT_NAME			= "NONE";
		private readonly string DEFAULT_CONDITION		= "PAUSE";
		private const bool DEFAULT_ENABLE				= true;
		private const int DEFAULT_ALARMCODE				= 0;
		private const int DEFAULT_ACTION				= (int)INTERRUPT_ACTION.ALARM;
		private const int DEFAULT_INPUTPORT				= -1;
		#endregion

		#endregion

		#region 변수
		Dictionary<EN_PARAM_INTERRUPT, PARAM_LIST> m_DicForItem		= new Dictionary<EN_PARAM_INTERRUPT,PARAM_LIST>();
		Dictionary<EN_PARAM_INTERRUPT, string> m_DicForStorage		= new Dictionary<EN_PARAM_INTERRUPT,string>();
		Dictionary<string, int> m_DicForResult						= new Dictionary<string, int>();
		Dictionary<int, string> m_DicForActionParam					= new Dictionary<int,string>();

		Interrupt m_InstanceOfInterruptDll							= null;
		Functional.Storage m_InstanceOfStorage						= null;
		#endregion

		#region 외부인터페이스
		/// <summary>
		/// 2020.06.17 by twkang [ADD] Interrupt 클래스를 초기화한다.
		/// </summary>
		public bool Init()
		{
			MakeMappingTable();
			
			m_InstanceOfStorage			= Functional.Storage.GetInstance();
			m_InstanceOfInterruptDll	= Interrupt.GetInstance();

			if(false == m_InstanceOfStorage.Load(Define.DefineEnumBase.Storage.EN_STORAGE_LIST.INTERRUPT))
			{
				return false;
			}

			InitializeParameter();

			return true;
		}
		/// <summary>
		/// 2020.06.26 by twkang [ADD] 아이템을 추가한다.
		/// </summary>
		public int AddInterruptItem()
		{
			int nItemNum = m_InstanceOfInterruptDll.AddItem();
			SetDefaultValue(nItemNum);

			string[] arGroup	= null;
			string[] arData		= null;

			if(FileBorn.GetInstance().GetItemFrame(BORN_LIST.INTERRUPT, ref arGroup, nItemNum.ToString(), ref arData)
				&& m_InstanceOfStorage.AddGroupAndItem(Define.DefineEnumBase.Storage.EN_STORAGE_LIST.INTERRUPT, ref arData))
			{
				SaveParameterToStorage(nItemNum);
				return nItemNum;
			}

			m_InstanceOfInterruptDll.RemoveItem(nItemNum);
			return -1;
		}
		/// <summary>
		/// 2020.06.17 by twkang [ADD] 아이템을 삭제한다.
		/// </summary>
		public bool RemoveItem(int nIndexItem)
		{
			string[] strArrGroup	= new string[] {nIndexItem.ToString()};

			if(m_InstanceOfStorage.RemoveGroup(Define.DefineEnumBase.Storage.EN_STORAGE_LIST.INTERRUPT
				, ref strArrGroup))
			{
				return m_InstanceOfInterruptDll.RemoveItem(nIndexItem);
			}
			return false;
		}
		/// <summary>
		/// 2020.06.17 by twkang [ADD] 아이템의 리스트를 반환한다.
		/// </summary>
		public bool GetListOfItem(ref int[] arIndexes)
		{
			return m_InstanceOfInterruptDll.GetListOfItem(ref arIndexes);
		}
		/// <summary>
		/// 2020.06.17 by twkang [ADD] Interrupt 파라미터 값을 변경한다.
		/// </summary>
		public bool SetParameter<T>(int nIndexOfItem, EN_PARAM_INTERRUPT enList, T tValue)
		{
			string[] strArrGroup	= new string[] {nIndexOfItem.ToString()};

			if(false == m_InstanceOfStorage.SetParameter(Define.DefineEnumBase.Storage.EN_STORAGE_LIST.INTERRUPT
				, m_DicForStorage[enList]
				, tValue
				, ref strArrGroup))
			{
				return false;
			}

			switch(enList)
			{
				case EN_PARAM_INTERRUPT.ACTION:
					if(m_DicForResult.ContainsKey(tValue.ToString()))
					{
						return m_InstanceOfInterruptDll.SetParameter(nIndexOfItem, m_DicForItem[enList], m_DicForResult[tValue.ToString()]);
					}
					break;
				default:
					return m_InstanceOfInterruptDll.SetParameter(nIndexOfItem, m_DicForItem[enList], tValue);
			}

			return false;
		}
		/// <summary>
		/// 2020.06.17 by twkang [ADD] Interrupt 파라미터 값을 반환한다.
		/// </summary>
		public bool GetParameter<T>(int nIndexOfItem, EN_PARAM_INTERRUPT enList, ref T tValue)
		{
			if(m_DicForItem.ContainsKey(enList))
			{
				return m_InstanceOfInterruptDll.GetParameter(nIndexOfItem, m_DicForItem[enList], ref tValue);
			}
			return false;
		}
		/// <summary>
		/// 2020.06.19 by twkang [ADD] Condition 상태를 체크한다.
		/// </summary>
		public bool CheckConditionActivation(int nIndexOfItem)
		{
			return m_InstanceOfInterruptDll.CheckConditionActivation(nIndexOfItem);
		}
		#endregion

		#region 내부인터페이스
		/// <summary>
		/// 2020.06.17 by twkang [ADD] Storage 데이터로 Interrupt 파라미터를 초기화한다.
		/// </summary>
		private void InitializeParameter()
		{
			string strValue			= string.Empty;
			
			string[] strArrGroup	= null;
			
			if(m_InstanceOfStorage.GetListOfGroup(Define.DefineEnumBase.Storage.EN_STORAGE_LIST.INTERRUPT, ref strArrGroup))
            {
                for(int nIndex = 0, nEnd = strArrGroup.Length; nIndex < nEnd; ++nIndex)
			    {
			    	string[] arGroup			= new string[1] {strArrGroup[nIndex]};
					int nIndexOfItem			= int.Parse(strArrGroup[nIndex]);

					m_InstanceOfInterruptDll.AddItem(nIndexOfItem);
					SetDefaultValue(nIndexOfItem);

			    	foreach(EN_PARAM_INTERRUPT en in Enum.GetValues(typeof(EN_PARAM_INTERRUPT)))
			    	{
			    		if(m_InstanceOfStorage.GetParameter(Define.DefineEnumBase.Storage.EN_STORAGE_LIST.INTERRUPT
			    			, m_DicForStorage[en]
			    			, ref strValue
			    			, ref arGroup))
			    		{
							switch(en)
							{
								default:
									m_InstanceOfInterruptDll.SetParameter(nIndexOfItem, m_DicForItem[en], strValue);
									break;
								case EN_PARAM_INTERRUPT.ACTION:
									if(m_DicForResult.ContainsKey(strValue))
									{
										m_InstanceOfInterruptDll.SetParameter(nIndexOfItem, m_DicForItem[en], m_DicForResult[strValue]);
									}
									break;
							}
			    		}
			    	}
			    }
            }
		}
		/// <summary>
		/// 2020.06.26 by twkang [ADD] 파라미터의 값을 Storage 데이터로 저장한다.
		/// </summary>
		private bool SaveParameterToStorage(int nIndexOfItem)
		{
			int nValue			= -1;
			bool bValue			= false;
			string[] arGroup	= new string[] {nIndexOfItem.ToString()};

			foreach(EN_PARAM_INTERRUPT en in Enum.GetValues(typeof(EN_PARAM_INTERRUPT)))
			{
				string strValue		= string.Empty;
				switch(en)
				{
					case EN_PARAM_INTERRUPT.ACTION:
						m_InstanceOfInterruptDll.GetParameter(nIndexOfItem, PARAM_LIST.ACTION, ref nValue);
						if(m_DicForActionParam.ContainsKey(nValue))
						{
							strValue	= m_DicForActionParam[nValue];
						}
						break;
					case EN_PARAM_INTERRUPT.ENABLE:
						m_InstanceOfInterruptDll.GetParameter(nIndexOfItem, m_DicForItem[en], ref bValue);
						strValue		= bValue.ToString();
						break;
					default:
						m_InstanceOfInterruptDll.GetParameter(nIndexOfItem, m_DicForItem[en], ref strValue);
						break;
				}
				m_InstanceOfStorage.SetParameter(Define.DefineEnumBase.Storage.EN_STORAGE_LIST.INTERRUPT, m_DicForStorage[en], strValue, ref arGroup, false);
			}

			m_InstanceOfStorage.Save(Define.DefineEnumBase.Storage.EN_STORAGE_LIST.INTERRUPT);
			return true;
		}
		/// <summary>
		/// 2020.06.29 by twkang [ADD] 클래스에서 사용할 Mapping Table 을 만든다.
		/// </summary>
		private void MakeMappingTable()
		{
			#region Param List
			m_DicForItem.Clear();
			m_DicForItem.Add(EN_PARAM_INTERRUPT.ACTION, PARAM_LIST.ACTION);
			m_DicForItem.Add(EN_PARAM_INTERRUPT.CODE_ALARM, PARAM_LIST.CODE_ALARM);
			m_DicForItem.Add(EN_PARAM_INTERRUPT.CONDITION, PARAM_LIST.CONDITION);
			m_DicForItem.Add(EN_PARAM_INTERRUPT.ENABLE, PARAM_LIST.ENABLE);
			m_DicForItem.Add(EN_PARAM_INTERRUPT.NAME, PARAM_LIST.NAME);
            m_DicForItem.Add(EN_PARAM_INTERRUPT.TARGET, PARAM_LIST.TARGET);
            m_DicForItem.Add(EN_PARAM_INTERRUPT.DO_INDEX, PARAM_LIST.DO_INDEX);
			#endregion

			#region Storage
			m_DicForStorage.Clear();
			m_DicForStorage.Add(EN_PARAM_INTERRUPT.ACTION, ITEM_ACTION);
			m_DicForStorage.Add(EN_PARAM_INTERRUPT.CODE_ALARM, ITEM_ALARMCODE);
			m_DicForStorage.Add(EN_PARAM_INTERRUPT.CONDITION, ITEM_CONDITION);
			m_DicForStorage.Add(EN_PARAM_INTERRUPT.ENABLE, ITEM_ENABLE);
			m_DicForStorage.Add(EN_PARAM_INTERRUPT.NAME, ITEM_NAME);
            m_DicForStorage.Add(EN_PARAM_INTERRUPT.TARGET, ITEM_INPUTPORT);
            m_DicForStorage.Add(EN_PARAM_INTERRUPT.DO_INDEX, ITEM_DIGITALOUTPUT);
			#endregion

			#region ActionParam
			m_DicForResult.Clear();
			m_DicForActionParam.Clear();
			foreach (INTERRUPT_ACTION en in Enum.GetValues(typeof(INTERRUPT_ACTION)))
			{
				m_DicForResult.Add(en.ToString(), (int)en);
				m_DicForActionParam.Add((int)en, en.ToString());
			}
			#endregion
		}
		/// <summary>
		/// 2020.09.29 by twkang [ADD] 해당 아이템을 Default 값으로 설정한다.
		/// </summary>
		private void SetDefaultValue(int nIndex)
		{
			m_InstanceOfInterruptDll.SetParameter(nIndex, PARAM_LIST.ACTION, DEFAULT_ACTION);
			m_InstanceOfInterruptDll.SetParameter(nIndex, PARAM_LIST.CODE_ALARM, DEFAULT_ALARMCODE);
			m_InstanceOfInterruptDll.SetParameter(nIndex, PARAM_LIST.CONDITION, DEFAULT_CONDITION);
			m_InstanceOfInterruptDll.SetParameter(nIndex, PARAM_LIST.ENABLE, DEFAULT_ENABLE);
			m_InstanceOfInterruptDll.SetParameter(nIndex, PARAM_LIST.NAME, DEFAULT_NAME + nIndex.ToString());
			m_InstanceOfInterruptDll.SetParameter(nIndex, PARAM_LIST.TARGET, DEFAULT_INPUTPORT);
			m_InstanceOfInterruptDll.SetParameter(nIndex, PARAM_LIST.DO_INDEX, DEFAULT_INPUTPORT);
		}
		#endregion
	}
}
