using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using FrameOfSystem3.Tool;

namespace FrameOfSystem3.Config
{
    class ConfigTool
    {
        #region SINGLETON
        private ConfigTool() { }
        private static ConfigTool m_instance = new ConfigTool();
        public static ConfigTool GetInstance() { return m_instance; }
        #endregion

        #region CONSTANT (미사용?)
		//private static readonly string ITEM_NAME = "NAME";
		//private static readonly string ITEM_ID = "ID";
		//private static readonly string ITEM_ENABLE = "ENABLE";
		//private static readonly string ITEM_STARTUP = "STARTUP";
		//private static readonly string ITEM_STANDSTILL = "STANDSTILL";
		//private static readonly string ITEM_USAGE_TYPE = "USAGE_TYPE";
		//private static readonly string ITEM_WORK_END = "WORK_END";
		//private static readonly string ITEM_NOTICE_LIMIT_COUNT = "NOTICE_LIMIT_COUNT";
		//private static readonly string ITEM_NOTICE_LIMIT_TIME = "NOTICE_LIMIT_TIME";
		//private static readonly string ITEM_NOTICE_ALARM_CODE = "NOTICE_ALARM_CODE";
		//private static readonly string ITEM_NOTICE_INTERVAL_COUNT = "NOTICE_INTERVAL_COUNT";
		//private static readonly string ITEM_NOTICE_INTERVAL_TIME = "NOTICE_INTERVAL_TIME";
		//private static readonly string ITEM_INITIAL_TIME = "INITIAL_TIME";
		//private static readonly string ITEM_WARNING_LIMIT_COUNT = "WARNING_LIMIT_COUNT";
		//private static readonly string ITEM_WARNING_LIMIT_TIME = "WARNING_LIMIT_TIME";
		//private static readonly string ITEM_WARNING_ALARM_CODE = "WARNING_ALARM_CODE";
		//private static readonly string ITEM_WARNING_INTERVAL_COUNT = "WARNING_INTERVAL_COUNT";
		//private static readonly string ITEM_WARNING_INTERVAL_TIME = "WARNING_INTERVAL_TIME";

		//// RECOVERY DATA
		//private static readonly string ITEM_TOOL_STATE = "TOOL_STATE";
		//private static readonly string ITEM_USED_COUNT = "USED_COUNT";
		//private static readonly string ITEM_NOTICE_DIVIDED_VALUE = "NOTICE_DIVIDED_VALUE";
		//private static readonly string ITEM_WARNING_DIVIDED_VALUE = "WARNING_DIVIDED_VALUE";
        #endregion

        #region FIELD
        Functional.Storage m_instanceStorage = null;

        WorkTool m_WorkTool = null;

        string[] m_arUsageTypeEnumNames = null;
        int[] m_arUsageTypeItemValues = null;
        string[] m_arWorkEndEnumNames = null;
        int[] m_arWorkEndItemValues = null;
        string[] m_arToolStateEnumNames = null;
        int[] m_arToolStateItemValues = null;

        #region CONVERSION
        Dictionary<string, EN_USAGE_TYPE> m_dicUsageTypeStringToEnum = new Dictionary<string, EN_USAGE_TYPE>();
        Dictionary<EN_USAGE_TYPE, string> m_dicUsageTypeEnumToString = new Dictionary<EN_USAGE_TYPE, string>();

        Dictionary<string, EN_WORK_END> m_dicWorkEndStringToEnum = new Dictionary<string, EN_WORK_END>();
        Dictionary<EN_WORK_END, string> m_dicWorkEndEnumToString = new Dictionary<EN_WORK_END, string>();

        Dictionary<string, EN_TOOL_STATE> m_dicToolStateStringToEnum = new Dictionary<string, EN_TOOL_STATE>();
        Dictionary<EN_TOOL_STATE, string> m_dicToolStateEnumToString = new Dictionary<EN_TOOL_STATE, string>();
        #endregion

        #endregion

        #region INTERFACE

        #region INITIALIZE
        public bool Init()
        {
            m_instanceStorage = Functional.Storage.GetInstance();

            if (!m_instanceStorage.Load(Define.DefineEnumBase.Storage.EN_STORAGE_LIST.TOOL))
                return false;

            m_WorkTool = WorkTool.GetInstance();

            MakeConversionTables();

            return InitTool();
        }
        #endregion

        #region SETTER/GETTER
        public bool GetToolList(ref string[] arToolIndex)
        {
            if (LoadListOfRootGroup(ref arToolIndex))
                return true;

            return false;
        }
        public bool GetToolName(string sIndex, ref string sToolName)
        {
            string sItemName = EN_TOOL_PARAM.NAME.ToString();

            if (LoadItem(sIndex, sItemName, ref sToolName))
                return true;

            return false;
        }
        public void GetItemsValue(string sIndex, ref string[] sItemNames, ref string[] sItemValues)
        {
            int nIndex = 0;
            int nCounts = Enum.GetValues(typeof(EN_TOOL_PARAM)).Length;

            sItemNames = new string[nCounts];
            sItemValues = new string[nCounts];

            foreach (EN_TOOL_PARAM enItem in Enum.GetValues(typeof(EN_TOOL_PARAM)))
            {
                string sItemName = enItem.ToString();
                string sItemValue = string.Empty;

                if (LoadItem(sIndex, sItemName, ref sItemValue))
                {
                    sItemNames[nIndex] = sItemName;
                    sItemValues[nIndex] = sItemValue;
                }

                ++ nIndex;
            }
        }
        public void GetMonitoringData(string sIndex, ref string[] sItemNames, ref string[] sItemValues)
        {
            int nIndex = 0;
            int nCounts = Enum.GetValues(typeof(EN_TOOL_MONITORING)).Length;

            sItemNames = new string[nCounts];
            sItemValues = new string[nCounts];

            foreach (EN_TOOL_MONITORING enItem in Enum.GetValues(typeof(EN_TOOL_MONITORING)))
            {
                string sItemName = enItem.ToString();
                string sItemValue = string.Empty;

                if (m_WorkTool.GetMonitoringData(int.Parse(sIndex), enItem, ref sItemValue))
                {
                    sItemNames[nIndex] = sItemName;
                    sItemValues[nIndex] = sItemValue;
                }

                ++nIndex;
            }
        }

        // 2025.02.10. by shkim. [ADD] Interval Count 인터페이스 추가
        public bool SetNoticeIntervalCount(int index, int value)
		{
			return SaveItem(index, EN_TOOL_PARAM.NOTICE_INTERVAL_COUNT.ToString(), value.ToString());
		}

		// 2025.02.10. by shkim. [ADD] Interval Count 인터페이스 추가
		public int GetNoticeIntervalCount(int index)
		{
			string strResult;
			m_WorkTool.GetParameter(index, EN_TOOL_PARAM.NOTICE_INTERVAL_COUNT, out strResult);
			int result;
			if (false == int.TryParse(strResult, out result))
				return -1;
			return result;
		}

		// 2025.02.10. by shkim. [ADD] Interval Count 인터페이스 추가
		public bool SetNoticeIntervalTime(int index, TimeSpan value)
		{
			return SaveItem(index, EN_TOOL_PARAM.NOTICE_INTERVAL_TIME.ToString(), value.ToString());
		}

		// 2025.02.10. by shkim. [ADD] Interval Count 인터페이스 추가
		public bool GetNoticeIntervalTime(int index, out TimeSpan result)
		{
			string strResult;
			m_WorkTool.GetParameter(index, EN_TOOL_PARAM.NOTICE_INTERVAL_TIME, out strResult);
			if (false == TimeSpan.TryParse(strResult, out result))
				return false;
			return true;
		}

		// 2025.02.10. by shkim. [ADD] Interval Count 인터페이스 추가
		public bool SetWarningIntervalCount(int index, int value)
		{
            return SaveItem(index, EN_TOOL_PARAM.WARNING_INTERVAL_COUNT.ToString(), value.ToString());
		}

		// 2025.02.10. by shkim. [ADD] Interval Count 인터페이스 추가
		public int GetWarningIntervalCount(int index)
		{
			string strResult;
			m_WorkTool.GetParameter(index, EN_TOOL_PARAM.WARNING_INTERVAL_COUNT, out strResult);
			int result;
			if (false == int.TryParse(strResult, out result))
				return -1;
			return result;
		}

		// 2025.02.10. by shkim. [ADD] Interval Count 인터페이스 추가
		public bool SetWarningIntervalTime(int index, TimeSpan value)
		{
			return SaveItem(index, EN_TOOL_PARAM.WARNING_INTERVAL_TIME.ToString(), value.ToString());
		}

		// 2025.02.10. by shkim. [ADD] Interval Count 인터페이스 추가
		public bool GetWarningIntervalTime(int index, out TimeSpan result)
		{
			string strResult;
			m_WorkTool.GetParameter(index, EN_TOOL_PARAM.WARNING_INTERVAL_TIME, out strResult);
			if (false == TimeSpan.TryParse(strResult, out result))
				return false;
			return true;
		}


		public bool SetNoticeLimitCount(int index, int value)
		{
			return SaveItem(index, EN_TOOL_PARAM.NOTICE_LIMIT_COUNT.ToString(), value.ToString());
		}
		public int GetNoticeLimitCount(int index)
		{
			string strResult;
			m_WorkTool.GetParameter(index, EN_TOOL_PARAM.NOTICE_LIMIT_COUNT, out strResult);

			int result;
			if (false == int.TryParse(strResult, out result))
				return -1;

			return result;
		}
		public bool SetWarningLimitCount(int index, int value)
		{
			return SaveItem(index, EN_TOOL_PARAM.WARNING_LIMIT_COUNT.ToString(), value.ToString());
		}
		public int GetWarningLimitCount(int index)
		{
			string strResult;
			m_WorkTool.GetParameter(index, EN_TOOL_PARAM.WARNING_LIMIT_COUNT, out strResult);

			int result;
			if (false == int.TryParse(strResult, out result))
				return -1;

			return result;
		}
		public bool SetNoticeLimitTime(int index, TimeSpan value)
		{
			return SaveItem(index, EN_TOOL_PARAM.NOTICE_LIMIT_TIME.ToString(), value.ToString());
		}
		public bool GetNoticeLimitTime(int index, out TimeSpan result)
		{
			string strResult;
			m_WorkTool.GetParameter(index, EN_TOOL_PARAM.NOTICE_LIMIT_TIME, out strResult);

			if (false == TimeSpan.TryParse(strResult, out result))
				return false;

			return true;
		}
		public bool SetWarningLimitTime(int index, TimeSpan value)
		{
			return SaveItem(index, EN_TOOL_PARAM.WARNING_LIMIT_TIME.ToString(), value.ToString());
		}
		public bool GetWarningLimitTime(int index, out TimeSpan result)
		{
			string strResult;
			m_WorkTool.GetParameter(index, EN_TOOL_PARAM.WARNING_LIMIT_TIME, out strResult);

			if (false == TimeSpan.TryParse(strResult, out result))
				return false;

			return true;
		}
		public bool SetToolId(int index, string newValue)
		{
			return m_WorkTool.SetToolId(index, newValue);
		}
		public bool GetToolId(int index, ref string result)
		{
			return m_WorkTool.GetToolId(index, ref result);
		}

        public bool SetEnable(int index, bool isEnable)
		{
			return SaveItem(index, EN_TOOL_PARAM.ENABLE.ToString(), isEnable.ToString());
		}
        public bool GetEnable(int index, out bool result)
		{
			string strResult;
			m_WorkTool.GetParameter(index, EN_TOOL_PARAM.ENABLE, out strResult);

			if (false == bool.TryParse(strResult, out result))
				return false;

			return true;
		}
		#endregion

		#region <SERVICE>
		/// <summary>
		/// 2021.11.10 by junho [ADD] Reset button 추가
		/// </summary>
		public void ResetItem(int nIndex, bool bActivate = true)
		{
            m_WorkTool.ResetWorkTool(nIndex, WorkTool.Def_ToolId, bActivate);
		}
		#endregion </SERVICE>

		#endregion

		#region METHOD

		#region INITIALIZE
		private bool InitTool()
        {
            string[] arGroupList = null;    // ARRAY 만 선언 (참조를 전달 받는 객체에서 메모리 할당)

            if (LoadListOfRootGroup(ref arGroupList))  // GROUP LIST: TOOL INDEX
            {
                for (int nTool = 0, nTableEnd = arGroupList.Length; nTool < nTableEnd; ++nTool)
                {
                    string sIndex = arGroupList[nTool];
                    int nIndex = 0;

                    if (int.TryParse(sIndex, out nIndex).Equals(false))
                        continue;

                    if (m_WorkTool.AddTool(nIndex).Equals(false))
                        continue;

                    foreach (EN_TOOL_PARAM enItem in Enum.GetValues(typeof(EN_TOOL_PARAM)))
                    {
                        string sItemName = enItem.ToString();
                        string sItemValue = string.Empty;

                        if (LoadItem(sIndex, sItemName, ref sItemValue))
                        {
                            m_WorkTool.SetParameter(nIndex, enItem, sItemValue);
                        }
                    }
                }
            }

            return true;
        }
        #endregion

        #region LOAD FILE
        private bool LoadListOfRootGroup(ref string[] arGroupList)
        {
            return m_instanceStorage.GetListOfGroup(Define.DefineEnumBase.Storage.EN_STORAGE_LIST.TOOL
                , ref arGroupList);
        }
        public  bool LoadItem(string sIndex, string sItemName, ref string sItemValue)
        {
            string[] arGroup = new string[] { sIndex };

            return m_instanceStorage.GetParameter(Define.DefineEnumBase.Storage.EN_STORAGE_LIST.TOOL
                , sItemName
                , ref sItemValue
                , ref arGroup);
        }
        #endregion

        #region SAVE FILE
        public bool SaveItem(int index, string sItemName, string sItemValue)
        {
			string[] arGroup = new string[] { index.ToString() };

			if (m_instanceStorage.SetParameter(Define.DefineEnumBase.Storage.EN_STORAGE_LIST.TOOL
                , sItemName
                , sItemValue
                , ref arGroup))
			{
				EN_TOOL_PARAM enItem = (EN_TOOL_PARAM)Enum.Parse(typeof(EN_TOOL_PARAM), sItemName);
				return m_WorkTool.SetParameter(index, enItem, sItemValue);
            }

            return false;
        }
        #endregion

        #region CONVERSION
        private void MakeConversionTables()
        {
            var arUsageType = Enum.GetValues(typeof(EN_USAGE_TYPE));

            m_arUsageTypeEnumNames = new string[arUsageType.Length];
            m_arUsageTypeItemValues = new int[arUsageType.Length];

            int nIndex = -1;

            foreach (EN_USAGE_TYPE enUsageType in arUsageType)
            {
                string sUsageType = enUsageType.ToString();

                m_dicUsageTypeStringToEnum.Add(sUsageType, enUsageType);
                m_dicUsageTypeEnumToString.Add(enUsageType, sUsageType);

                m_arUsageTypeEnumNames[++nIndex] = sUsageType;
            }

            var arWorkEnd = Enum.GetValues(typeof(EN_WORK_END));

            m_arWorkEndEnumNames = new string[arWorkEnd.Length];
            m_arWorkEndItemValues = new int[arWorkEnd.Length];

            nIndex = -1;

            foreach (EN_WORK_END enWorkEnd in arWorkEnd)
            {
                string sWorkEnd = enWorkEnd.ToString();

                m_dicWorkEndStringToEnum.Add(sWorkEnd, enWorkEnd);
                m_dicWorkEndEnumToString.Add(enWorkEnd, sWorkEnd);

                m_arWorkEndEnumNames[++nIndex] = sWorkEnd;
            }

            var arToolState = Enum.GetValues(typeof(EN_TOOL_STATE));

            m_arToolStateEnumNames = new string[arToolState.Length];
            m_arToolStateItemValues = new int[arToolState.Length];

            nIndex = -1;

            foreach (EN_TOOL_STATE enToolState in arToolState)
            {
                string sToolState = enToolState.ToString();

                m_dicToolStateStringToEnum.Add(sToolState, enToolState);
                m_dicToolStateEnumToString.Add(enToolState, sToolState);

                m_arWorkEndEnumNames[++nIndex] = sToolState;
            }
        }
        #endregion
        
        #endregion
    }
}
