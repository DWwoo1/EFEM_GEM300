using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Trigger_;
using FileBorn_;

namespace FrameOfSystem3.Config
{
	public class ConfigTrigger
	{
		private ConfigTrigger() { }

		#region 싱글톤
		private static ConfigTrigger _Instance	= new ConfigTrigger();
		public static ConfigTrigger GetInstance()
		{
			return _Instance;
		}
		#endregion

		#region 열거형
		public enum EN_PARAM_TRIGGER
		{
			BLINK_CONDITION_ALARM,
			BLINK_CONDITION_EQUIPMENT,
			CONDITION_ALARM,
			CONDITION_EQUIPMENT,
			ENABLE,
			INTERVAL,
            CHECK_INTERVAL,
			NAME,
			TARGET
		}
		#endregion

		#region 상수
		private const int INITIALIZE_INTERVAL				= 500;
		private const int INITIALIZE_PARAM					= -1;

		private readonly string ITEM_NAME					= "NAME";
		private readonly string ITEM_ENABLE					= "ENABLE";
		private readonly string ITEM_IOPORT					= "IOPORT";
		private readonly string ITEM_INTERVAL				= "INTERVAL";
        private readonly string ITEM_CHECK_INTERVAL			= "CHECK_INTERVAL";
		private readonly string ITEM_BLINKCONDITION_EQUIP	= "BLINKCONDITION_EQUIP";
		private readonly string ITEM_CONDITION_EQUIP		= "CONDITION_EQUIP";
		private readonly string ITEM_BLINKCONDITION_ALARM	= "BLINKCONDITION_ALARM";
		private readonly string ITEM_CONDITION_ALARM		= "CONDITION_ALARM";

		#region Default Value
		private const bool DEFAULT_ENABLE					= true;
		private const int DEFAULT_INTERVAL					= 500;
		private const int DEFAULT_IOPORT					= -1;
		private readonly string DEFAULT_NAME				= "NONE";
		private readonly string DEFAULT_CONDITION_ALARM		= "ERROR";
		private readonly string DEFAULT_CONDITION_EQUIP		= "PAUSE";
		#endregion

		#endregion

		#region 변수
		Dictionary<EN_PARAM_TRIGGER, PARAM_LIST> m_DicForItem	= new Dictionary<EN_PARAM_TRIGGER,PARAM_LIST>();
		Dictionary<EN_PARAM_TRIGGER, string> m_DICForStorage	= new Dictionary<EN_PARAM_TRIGGER,string>();

		Trigger m_InstanceOfTriggerDll				= null;
		Functional.Storage m_InstanceOfStorage		= null;
		#endregion

		#region 외부인터페이스
		/// <summary>
		/// 2020.06.18 by twkang [ADD] Trigger 클래스를 초기화한다.
		/// </summary>
		public bool Init()
		{
			#region Mapping Table
			m_DicForItem.Clear();
			m_DicForItem.Add(EN_PARAM_TRIGGER.BLINK_CONDITION_ALARM, PARAM_LIST.BLINK_CONDITION_ALARM);
			m_DicForItem.Add(EN_PARAM_TRIGGER.BLINK_CONDITION_EQUIPMENT, PARAM_LIST.BLINK_CONDITION_EQUIPMENT);
			m_DicForItem.Add(EN_PARAM_TRIGGER.CONDITION_ALARM, PARAM_LIST.CONDITION_ALARM);
			m_DicForItem.Add(EN_PARAM_TRIGGER.CONDITION_EQUIPMENT, PARAM_LIST.CONDITION_EQUIPMENT);
			m_DicForItem.Add(EN_PARAM_TRIGGER.ENABLE, PARAM_LIST.ENABLE);
			m_DicForItem.Add(EN_PARAM_TRIGGER.INTERVAL, PARAM_LIST.INTERVAL);
            m_DicForItem.Add(EN_PARAM_TRIGGER.CHECK_INTERVAL, PARAM_LIST.CHECK_INTERVAL);
			m_DicForItem.Add(EN_PARAM_TRIGGER.NAME, PARAM_LIST.NAME);
			m_DicForItem.Add(EN_PARAM_TRIGGER.TARGET, PARAM_LIST.TARGET);

			m_DICForStorage.Clear();
			m_DICForStorage.Add(EN_PARAM_TRIGGER.BLINK_CONDITION_ALARM, ITEM_BLINKCONDITION_ALARM);
			m_DICForStorage.Add(EN_PARAM_TRIGGER.BLINK_CONDITION_EQUIPMENT, ITEM_BLINKCONDITION_EQUIP);
			m_DICForStorage.Add(EN_PARAM_TRIGGER.CONDITION_ALARM, ITEM_CONDITION_ALARM);
			m_DICForStorage.Add(EN_PARAM_TRIGGER.CONDITION_EQUIPMENT, ITEM_CONDITION_EQUIP);
			m_DICForStorage.Add(EN_PARAM_TRIGGER.ENABLE, ITEM_ENABLE);
			m_DICForStorage.Add(EN_PARAM_TRIGGER.INTERVAL, ITEM_INTERVAL);
            m_DICForStorage.Add(EN_PARAM_TRIGGER.CHECK_INTERVAL, ITEM_CHECK_INTERVAL);
			m_DICForStorage.Add(EN_PARAM_TRIGGER.NAME, ITEM_NAME);
			m_DICForStorage.Add(EN_PARAM_TRIGGER.TARGET, ITEM_IOPORT);			
			#endregion

			m_InstanceOfTriggerDll	= Trigger.GetInstance();
			m_InstanceOfStorage		= Functional.Storage.GetInstance();
			
			if(false == m_InstanceOfStorage.Load(Define.DefineEnumBase.Storage.EN_STORAGE_LIST.TRIGGER))
			{
				return false;
			}

			InitializeParameter();


			return true;
		}
		/// <summary>
		/// 2020.06.30 by twkang [ADD] 아이템을 추가한다.
		/// </summary>
		public int AddTriggerItem()
		{
			int nItemNum		= m_InstanceOfTriggerDll.AddItem();
			SetDefaultValue(nItemNum);

			string[] arGroup	= null;
			string[] arData		= null;

			if(FileBorn.GetInstance().GetItemFrame(BORN_LIST.TRIGGER, ref arGroup, nItemNum.ToString(), ref arData)
				&& m_InstanceOfStorage.AddGroupAndItem(Define.DefineEnumBase.Storage.EN_STORAGE_LIST.TRIGGER, ref arData))
			{
				SaveParamToStorage(nItemNum);
				return nItemNum;
			}

			m_InstanceOfTriggerDll.RemoveItem(nItemNum);
			return -1;
		}
		/// <summary>
		/// 2020.06.18 by twkang [ADD] 아이템을 삭제한다.
		/// </summary>
		public bool RemoveItem(int nIndexItem)
		{
			string[] strARrGroup		= new string[] {nIndexItem.ToString()};

			if(m_InstanceOfStorage.RemoveGroup(Define.DefineEnumBase.Storage.EN_STORAGE_LIST.TRIGGER, ref strARrGroup))
			{
				return m_InstanceOfTriggerDll.RemoveItem(nIndexItem);
			}

			return false;
		}
		/// <summary>
		/// 2020.06.18 by twkang [ADD] 아이템의 리스트를 반환한다.
		/// </summary>
		public bool GetListOfItem(ref int[] arIndexes)
		{			
			return m_InstanceOfTriggerDll.GetListOfItem(ref arIndexes);
		}
		/// <summary>
		/// 2020.06.18 by twkang [ADD] Trigger 파라미터 값을 변경한다.
		/// </summary>
		public bool SetParameter<T>(int nIndexOfItem, EN_PARAM_TRIGGER enList, T tValue)
		{
			string[] strArrGroup		= new string[] {nIndexOfItem.ToString()};

			if(false == m_InstanceOfStorage.SetParameter(Define.DefineEnumBase.Storage.EN_STORAGE_LIST.TRIGGER
				, m_DICForStorage[enList]
				, tValue
				, ref strArrGroup))
			{
				return false;
			}
			return m_InstanceOfTriggerDll.SetParameter(nIndexOfItem, m_DicForItem[enList], tValue);
		}
		/// <summary>
		/// 2020.06.18 by twkang [ADD] Trigger 파라미터 값을 반환한다.
		/// </summary>
		public bool GetParameter<T>(int nIndexOfItem, EN_PARAM_TRIGGER enList, ref T tValue)
		{
			if(m_DicForItem.ContainsKey(enList))
			{				
				return m_InstanceOfTriggerDll.GetParameter(nIndexOfItem, m_DicForItem[enList], ref tValue);
			}
			return false;
		}
		/// <summary>
		/// 2020.06.18 by twkang [ADD] Condition 상태를 체크한다.
		/// </summary>
		public bool CheckConditionActivation(int nIndexOfItem)
		{
			return m_InstanceOfTriggerDll.CheckConditionActivation(nIndexOfItem);
		}
		/// <summary>
		/// 2020.06.18 by twkang [ADD] Blink Condition 상태를 체크한다.
		/// </summary>
		public bool CheckConditionBlinking(int nIndexOfItem)
		{
			return m_InstanceOfTriggerDll.CheckConditionBlinking(nIndexOfItem);
		}
		#endregion

		#region 내부인터페이스
		/// <summary>
		/// 2020.06.18 by twkang [ADD] Storage 데이터로 Trigger 파라미터를 초기화한다.
		/// </summary>
		private void InitializeParameter()
		{
			string strValue			= string.Empty;			

			string[] strArrGroup	= null;

			if(m_InstanceOfStorage.GetListOfGroup(Define.DefineEnumBase.Storage.EN_STORAGE_LIST.TRIGGER, ref strArrGroup))
            {
                for(int nIndex = 0, nEnd = strArrGroup.Length; nIndex < nEnd; ++nIndex)
			    {
			    	string[] arGroup	= new string[1] {strArrGroup[nIndex]};

					int nIndexOfItem		= int.Parse(strArrGroup[nIndex]);
					m_InstanceOfTriggerDll.AddItem(nIndexOfItem);
					SetDefaultValue(nIndexOfItem);

			    	foreach(EN_PARAM_TRIGGER en in Enum.GetValues(typeof(EN_PARAM_TRIGGER)))
			    	{
			    		if (m_InstanceOfStorage.GetParameter(Define.DefineEnumBase.Storage.EN_STORAGE_LIST.TRIGGER
			    			, m_DICForStorage[en]
			    			, ref strValue
			    			, ref arGroup))
			    		{
			    			m_InstanceOfTriggerDll.SetParameter(nIndexOfItem, m_DicForItem[en], strValue);
			    		}
			    	}
			    }
            }
		}
		/// <summary>
		/// 2020.06.30 by twkang [ADD] 파라미터의 값을 Storage 데이터로 저장한다.
		/// </summary>
		private bool SaveParamToStorage(int nIndexOfItem)
		{
			string[] arGroup		= new string[] {nIndexOfItem.ToString()};

			foreach(EN_PARAM_TRIGGER en in Enum.GetValues(typeof(EN_PARAM_TRIGGER)))
			{
				string strValue		= string.Empty;

				switch(en)
				{
					case EN_PARAM_TRIGGER.ENABLE:
						bool bValue		= false;
						m_InstanceOfTriggerDll.GetParameter(nIndexOfItem, m_DicForItem[en], ref bValue);
						strValue		= bValue.ToString();
						break;
					default:
						m_InstanceOfTriggerDll.GetParameter(nIndexOfItem, m_DicForItem[en], ref strValue);
						break;

				}
				m_InstanceOfStorage.SetParameter(Define.DefineEnumBase.Storage.EN_STORAGE_LIST.TRIGGER, m_DICForStorage[en], strValue, ref arGroup, false);
			}
			m_InstanceOfStorage.Save(Define.DefineEnumBase.Storage.EN_STORAGE_LIST.TRIGGER);

			return true;
		}
		/// <summary>
		/// 2020.10.05 by twkang [ADD] 해당 아이템을 Default 값으로 설정한다.
		/// </summary>
		private void SetDefaultValue(int nIndex)
		{
			m_InstanceOfTriggerDll.SetParameter(nIndex, PARAM_LIST.BLINK_CONDITION_ALARM, DEFAULT_CONDITION_ALARM);
			m_InstanceOfTriggerDll.SetParameter(nIndex, PARAM_LIST.BLINK_CONDITION_EQUIPMENT, DEFAULT_CONDITION_EQUIP);
			m_InstanceOfTriggerDll.SetParameter(nIndex, PARAM_LIST.CONDITION_ALARM, DEFAULT_CONDITION_ALARM);
			m_InstanceOfTriggerDll.SetParameter(nIndex, PARAM_LIST.CONDITION_EQUIPMENT, DEFAULT_CONDITION_EQUIP);
			m_InstanceOfTriggerDll.SetParameter(nIndex, PARAM_LIST.ENABLE, DEFAULT_ENABLE);
			m_InstanceOfTriggerDll.SetParameter(nIndex, PARAM_LIST.INTERVAL, DEFAULT_INTERVAL);
            m_InstanceOfTriggerDll.SetParameter(nIndex, PARAM_LIST.CHECK_INTERVAL, DEFAULT_INTERVAL);
			m_InstanceOfTriggerDll.SetParameter(nIndex, PARAM_LIST.NAME, DEFAULT_NAME + nIndex.ToString());
			m_InstanceOfTriggerDll.SetParameter(nIndex, PARAM_LIST.TARGET, DEFAULT_IOPORT);
		}
		#endregion
	}
}
