using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Motion_;
using JogManager_;
using FileBorn_;

namespace FrameOfSystem3.Config
{
	public class ConfigJog
	{
		private ConfigJog() { }

		#region 싱글톤
		private static ConfigJog _Instance		= new ConfigJog();
		public static ConfigJog GetInstance()
		{
			return _Instance;
		}
		#endregion

		#region 열거형
        /// <summary>
        /// 2020.08.11 by yjlee [ADD] Enumerate the parameters of the Jog.
        /// </summary>
		public enum EN_PARAM_JOG
		{
            NAME                    = 0,
            INDEX_LEFT_RIGHT,
            INDEX_UP_DOWN,
            ENABLE_LEFT_RIGHT,
            ENABLE_UP_DOWN,

			INDEX_EXTRA_LEFT_RIGHT,
			INDEX_EXTRA_UP_DOWN,
			ENABLE_EXTRA_LEFT_RIGHT,
			ENABLE_EXTRA_UP_DOWN,
		}
		/// <summary>
		/// 2022.10.24 by junho [ADD] Add enum for jog reverse
		/// </summary>
		public enum EN_PARAM_JOG_REVERSE
		{
			KEY = 0,
			NAME,
		}
		#endregion

		#region 상수

		#region Default Value
		private readonly string DEFAULT_NAME		= "NONE";
		private const bool DEFAULT_ENABLE			= false;
		private const int DEFAULT_INDEX				= -1;
		#endregion

		#endregion

		#region 변수
		private Functional.Storage m_InstanceOfStorage	    = null;
		private Config.ConfigMotion m_InstanceMotionDll     = null;
        private JogManager m_instanceJogManager             = null;

        private Dictionary<EN_PARAM_JOG, JogManager_.PARAM_LIST> m_dicOfParam       = new Dictionary<EN_PARAM_JOG,JogManager_.PARAM_LIST>();
        private Dictionary<EN_PARAM_JOG, string> m_dicOfEnumParam       = new Dictionary<EN_PARAM_JOG,string>();

		private Dictionary<EN_PARAM_JOG_REVERSE, JogManager_.PARAM_REVERSE> m_dicOfParamReverse = new Dictionary<EN_PARAM_JOG_REVERSE, PARAM_REVERSE>();
		private Dictionary<EN_PARAM_JOG_REVERSE, string> m_dicOfEnumParamReverse = new Dictionary<EN_PARAM_JOG_REVERSE, string>();
		#endregion

		#region 외부인터페이스
		/// <summary>
		/// 2020.07.16 
		/// </summary>
		public bool Init()
		{
			MakeMappingTable();

			m_InstanceOfStorage				= Functional.Storage.GetInstance();
			m_InstanceMotionDll				= Config.ConfigMotion.GetInstance();
            m_instanceJogManager            = JogManager.GetInstance();

			if(false == m_instanceJogManager.Init()
                || false == m_InstanceOfStorage.Load(Define.DefineEnumBase.Storage.EN_STORAGE_LIST.JOG)
				|| false == m_InstanceOfStorage.Load(Define.DefineEnumBase.Storage.EN_STORAGE_LIST.JOG_REVERSE))
			{
				return false;
			}

			InitializeParameter();

			return true;
		}
		/// <summary>
		/// 2020.08.11 by yjlee [MOD] Add new jog item in the structure.
		/// </summary>
		public int AddJogItem()
		{
            int nIndexOfItem    = m_instanceJogManager.AddItem();
			SetDefaultValue(nIndexOfItem);

            if(-1 < nIndexOfItem)
            {
                string[] arGroup    = null;
                string[] arData     = null;

                if (FileBorn.GetInstance().GetItemFrame(BORN_LIST.JOG, ref arGroup, nIndexOfItem.ToString(), ref arData)
                    && m_InstanceOfStorage.AddGroupAndItem(Define.DefineEnumBase.Storage.EN_STORAGE_LIST.JOG, ref arData))
                {
                    return SaveParameterToStorage(nIndexOfItem) ? nIndexOfItem : -1;
                }
            }
			return -1;
		}
		/// <summary>
		/// 2020.08.11 by yjlee [ADD] Remove the item that has the index.
		/// </summary>
		public bool RemoveJogItem(int nIndexItem)
		{
			string[] strArrGroup	= new string[] {nIndexItem.ToString()};

			if(m_InstanceOfStorage.RemoveGroup(Define.DefineEnumBase.Storage.EN_STORAGE_LIST.JOG, ref strArrGroup))
			{
				return m_instanceJogManager.RemoveItem(nIndexItem);
			}
			return false;
		}
		/// <summary>
		/// 2020.08.12 by twkang [ADD] 아이템의 리스트를 반환한다.
		/// </summary>
		public bool GetListOfItem(ref int[] arIndexes)
		{
			return m_instanceJogManager.GetListOfItem(ref arIndexes);
		}
		/// <summary>
		/// 2020.08.11 by yjlee [MOD] Get a parameter of the item.
		/// </summary>
		public bool GetParameter<T>(int nIndexOfItem, EN_PARAM_JOG enList, ref T tValue)
		{
            string[] arGroup        = new string[]{ nIndexOfItem.ToString()};

			return m_InstanceOfStorage.GetParameter(Define.DefineEnumBase.Storage.EN_STORAGE_LIST.JOG
                , m_dicOfEnumParam[enList]
                , ref tValue
                , ref arGroup);
		}
		/// <summary>
		/// 2020.08.12 by twkang [ADD] Axis Name 을 반환한다.
		/// </summary>
		public string GetAxisNameByIndex(int nIndexOfAxis)
		{
			string strAxisName	= string.Empty;

			m_InstanceMotionDll.GetMotionParameter(nIndexOfAxis, ConfigMotion.EN_PARAM_MOTION.NAME, ref strAxisName);

			return strAxisName;
		}
		/// <summary>
		/// 2020.08.11 by yjlee [MOD] Set a parameter of the jog instance.
		/// </summary>
		public bool SetParameter<T>(int nIndexOfItem, EN_PARAM_JOG enList, T tValue)
		{
            string[] arGroup        = new string[]{ nIndexOfItem.ToString()};

            if(m_InstanceOfStorage.SetParameter(Define.DefineEnumBase.Storage.EN_STORAGE_LIST.JOG
                , m_dicOfEnumParam[enList]
                , tValue
                , ref arGroup))
            {
				return m_instanceJogManager.SetParameter(nIndexOfItem, m_dicOfParam[enList], tValue);
            }

			return false;
		}


		// 2022.10.25 by junho [ADD] for jog reverse
		public int AddJogReverseItem()
		{
			int nIndexOfItem = m_instanceJogManager.AddReverseItem();
			SetDefaultValueReverse(nIndexOfItem);

			if (-1 < nIndexOfItem)
			{
				string[] arGroup = null;
				string[] arData = null;

				if (FileBorn.GetInstance().GetItemFrame(BORN_LIST.JOG_REVERSE, ref arGroup, nIndexOfItem.ToString(), ref arData)
					&& m_InstanceOfStorage.AddGroupAndItem(Define.DefineEnumBase.Storage.EN_STORAGE_LIST.JOG_REVERSE, ref arData))
				{
					return SaveParameterToStorageReverse(nIndexOfItem) ? nIndexOfItem : -1;
				}
			}
			return -1;
		}
		public bool RemoveJogReverseItem(int nIndexItem)
		{
			string[] strArrGroup = new string[] { nIndexItem.ToString() };

			if (m_InstanceOfStorage.RemoveGroup(Define.DefineEnumBase.Storage.EN_STORAGE_LIST.JOG_REVERSE, ref strArrGroup))
			{
				return m_instanceJogManager.RemoveReverseItem(nIndexItem);
			}
			return false;
		}
		public bool GetListOfReverseItem(ref int[] arIndexes)
		{
			return m_instanceJogManager.GetListOfReverseItem(ref arIndexes);
		}
		public bool GetParameterReverse<T>(int nIndexOfItem, EN_PARAM_JOG_REVERSE enList, ref T tValue)
		{
			string[] arGroup = new string[] { nIndexOfItem.ToString() };

			return m_InstanceOfStorage.GetParameter(Define.DefineEnumBase.Storage.EN_STORAGE_LIST.JOG_REVERSE
				, m_dicOfEnumParamReverse[enList]
				, ref tValue
				, ref arGroup);
		}
		public bool SetParameterReverse<T>(int nIndexOfItem, EN_PARAM_JOG_REVERSE enList, T tValue)
		{
			string[] arGroup = new string[] { nIndexOfItem.ToString() };

			if (m_InstanceOfStorage.SetParameter(Define.DefineEnumBase.Storage.EN_STORAGE_LIST.JOG_REVERSE
				, m_dicOfEnumParamReverse[enList]
				, tValue
				, ref arGroup))
			{
				return m_instanceJogManager.SetParameter(nIndexOfItem, m_dicOfParamReverse[enList], tValue);
			}

			return false;
		}
		#endregion

		#region 내부인터페이스
		/// <summary>
		/// 2020.08.11 by yjlee [ADD] Make the mapping tables for the jog instance.
		/// </summary>
		private void MakeMappingTable()
		{
            m_dicOfParam.Add(EN_PARAM_JOG.INDEX_UP_DOWN, JogManager_.PARAM_LIST.INDEX_UP_DOWN);
            m_dicOfParam.Add(EN_PARAM_JOG.INDEX_LEFT_RIGHT, JogManager_.PARAM_LIST.INDEX_LEFT_RIGHT);
            m_dicOfParam.Add(EN_PARAM_JOG.ENABLE_UP_DOWN, JogManager_.PARAM_LIST.ENABLE_UP_DOWN);
			m_dicOfParam.Add(EN_PARAM_JOG.ENABLE_LEFT_RIGHT, JogManager_.PARAM_LIST.ENABLE_LEFT_RIGHT);

			// 2022.11.01 by junho [ADD] extra axis
			m_dicOfParam.Add(EN_PARAM_JOG.INDEX_EXTRA_LEFT_RIGHT, JogManager_.PARAM_LIST.INDEX_EXTRA_LEFT_RIGHT);
			m_dicOfParam.Add(EN_PARAM_JOG.INDEX_EXTRA_UP_DOWN, JogManager_.PARAM_LIST.INDEX_EXTRA_UP_DOWN);
			m_dicOfParam.Add(EN_PARAM_JOG.ENABLE_EXTRA_LEFT_RIGHT, JogManager_.PARAM_LIST.ENABLE_EXTRA_LEFT_RIGHT);
			m_dicOfParam.Add(EN_PARAM_JOG.ENABLE_EXTRA_UP_DOWN, JogManager_.PARAM_LIST.ENABLE_EXTRA_UP_DOWN);
            m_dicOfParam.Add(EN_PARAM_JOG.NAME, JogManager_.PARAM_LIST.NAME);

            foreach(var kvp in m_dicOfParam)
            {
                m_dicOfEnumParam.Add(kvp.Key, kvp.Value.ToString());
            }

			// 2022.10.24 by junho [ADD] for jog reverse
			m_dicOfParamReverse.Add(EN_PARAM_JOG_REVERSE.KEY, JogManager_.PARAM_REVERSE.KEY);
			m_dicOfParamReverse.Add(EN_PARAM_JOG_REVERSE.NAME, JogManager_.PARAM_REVERSE.NAME);

			foreach (var kvp in m_dicOfParamReverse)
			{
				m_dicOfEnumParamReverse.Add(kvp.Key, kvp.Value.ToString());
			}
		}
		/// <summary>
		/// 2020.08.12 by twkang [ADD] 
		/// </summary>
		private void InitializeParameter()
		{
			string strValue = string.Empty;

			string[] strArrGroup = null;

			if (m_InstanceOfStorage.GetListOfGroup(Define.DefineEnumBase.Storage.EN_STORAGE_LIST.JOG, ref strArrGroup))
			{
				for (int nIndex = 0, nEnd = strArrGroup.Length; nIndex < nEnd; ++nIndex)
				{
					string[] arGroup = new string[1] { strArrGroup[nIndex] };
					int nIndexOfItem = int.Parse(strArrGroup[nIndex]);

					m_instanceJogManager.AddItem(nIndexOfItem);
					SetDefaultValue(nIndexOfItem);

					foreach (EN_PARAM_JOG en in Enum.GetValues(typeof(EN_PARAM_JOG)))
					{
						if (m_InstanceOfStorage.GetParameter(Define.DefineEnumBase.Storage.EN_STORAGE_LIST.JOG
							, m_dicOfParam[en].ToString()
							, ref strValue
							, ref arGroup))
						{
							m_instanceJogManager.SetParameter(nIndex, m_dicOfParam[en], strValue);
						}
					}

				}
			}
			if (m_InstanceOfStorage.GetListOfGroup(Define.DefineEnumBase.Storage.EN_STORAGE_LIST.JOG_REVERSE, ref strArrGroup))
			{
				for (int nIndex = 0, nEnd = strArrGroup.Length; nIndex < nEnd; ++nIndex)
				{
					string[] arGroup = new string[1] { strArrGroup[nIndex] };
					int nIndexOfItem = int.Parse(strArrGroup[nIndex]);

					m_instanceJogManager.AddReverseItem(nIndexOfItem);
					SetDefaultValueReverse(nIndexOfItem);

					foreach (EN_PARAM_JOG_REVERSE en in Enum.GetValues(typeof(EN_PARAM_JOG_REVERSE)))
					{
						if (m_InstanceOfStorage.GetParameter(Define.DefineEnumBase.Storage.EN_STORAGE_LIST.JOG_REVERSE
							, m_dicOfParamReverse[en].ToString()
							, ref strValue
							, ref arGroup))
						{
							m_instanceJogManager.SetParameter(nIndex, m_dicOfParamReverse[en], strValue);
						}
					}

				}
			}
		}

		/// <summary>
		/// 2020.08.11 by yjlee [MOD] Set the parameters to the storage by the dll instance.
		/// </summary>
		private bool SaveParameterToStorage(int nIndexOfItem)
		{
            string[] arGroup        = new string[]{nIndexOfItem.ToString()};

			bool bValue				= false;
			
            foreach(var kvp in m_dicOfParam)
            {
                string strValue     = string.Empty;

				switch(kvp.Key)
				{
					case EN_PARAM_JOG.ENABLE_LEFT_RIGHT:
					case EN_PARAM_JOG.ENABLE_UP_DOWN:

					// 2022.11.01 by junho [ADD] extra axis
					case EN_PARAM_JOG.ENABLE_EXTRA_LEFT_RIGHT:
					case EN_PARAM_JOG.ENABLE_EXTRA_UP_DOWN:
						m_instanceJogManager.GetParameter(nIndexOfItem, kvp.Value, ref bValue);
						strValue	= bValue.ToString();
						break;
					default:
						m_instanceJogManager.GetParameter(nIndexOfItem, kvp.Value, ref strValue);
						break;
				}
                m_InstanceOfStorage.SetParameter(Define.DefineEnumBase.Storage.EN_STORAGE_LIST.JOG
                    , m_dicOfEnumParam[kvp.Key]
                    , strValue
                    , ref arGroup
                    , false);
            }
			return m_InstanceOfStorage.Save(Define.DefineEnumBase.Storage.EN_STORAGE_LIST.JOG);
		}
		/// <summary>
		/// 2020.09.29 by twkang [ADD] 해당 아이템을 Default 값으로 설정한다.
		/// </summary>
		private void SetDefaultValue(int nIndex)
		{
			m_instanceJogManager.SetParameter(nIndex, JogManager_.PARAM_LIST.ENABLE_LEFT_RIGHT, DEFAULT_ENABLE);
			m_instanceJogManager.SetParameter(nIndex, JogManager_.PARAM_LIST.ENABLE_UP_DOWN, DEFAULT_ENABLE);
			m_instanceJogManager.SetParameter(nIndex, JogManager_.PARAM_LIST.INDEX_LEFT_RIGHT, DEFAULT_INDEX);
			m_instanceJogManager.SetParameter(nIndex, JogManager_.PARAM_LIST.INDEX_UP_DOWN, DEFAULT_INDEX);
			m_instanceJogManager.SetParameter(nIndex, JogManager_.PARAM_LIST.NAME, DEFAULT_NAME + nIndex.ToString());
		}

		/// <summary>
		/// 2022.10.24 by junho [ADD] for jog reverse
		/// </summary>
		private bool SaveParameterToStorageReverse(int nIndexOfItem)
		{
			string[] arGroup = new string[] { nIndexOfItem.ToString() };
			foreach (var kvp in m_dicOfParamReverse)
			{
				string strValue = string.Empty;
				m_instanceJogManager.GetParameter(nIndexOfItem, kvp.Value, ref strValue);
				m_InstanceOfStorage.SetParameter(Define.DefineEnumBase.Storage.EN_STORAGE_LIST.JOG_REVERSE
					, m_dicOfEnumParamReverse[kvp.Key]
					, strValue
					, ref arGroup
					, false);
			}
			return m_InstanceOfStorage.Save(Define.DefineEnumBase.Storage.EN_STORAGE_LIST.JOG_REVERSE);
		}
		/// <summary>
		/// 2022.10.24 by junho [ADD] for jog reverse
		/// </summary>
		private void SetDefaultValueReverse(int nIndex)
		{
			m_instanceJogManager.SetParameter(nIndex, JogManager_.PARAM_REVERSE.NAME, DEFAULT_NAME + nIndex.ToString());
		}
		#endregion
	}
}
