using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using TaskAction_;
using FileBorn_;

using FrameOfSystem3.DynamicLink_;

namespace FrameOfSystem3.Config
{
    public class ConfigFlow
    {
        #region SINGLETON
        private ConfigFlow() { }
        private static ConfigFlow m_instance = new ConfigFlow();
        public static ConfigFlow GetInstance(){ return m_instance; }
        #endregion

        #region CONSTANT
        private static readonly string ITEM_FLOW_ACTION    = "ACTION_NAME";
        private static readonly string GROUP_PRECONDITION  = "PRE_CONDITION";
        private static readonly string GROUP_POSTCONDITION = "POST_CONDITION";
        #endregion

        #region FIELD
        Functional.Storage m_instanceStorage = null;
		//TaskActionFlow m_instanceFlow = null;		// 미사용?

        DynamicLink m_DynamicLink = null;

        string[] m_arPreconditionEnumNames = null;
        int[] m_arPreconditionItemValues = null;
        string[] m_arPostconditionEnumNames = null;
        int[] m_arPostconditionItemValues = null;

        #region CONVERSION
        Dictionary<string, EN_CONDITION_RESULT> m_dicPreStringToEnum = new Dictionary<string, EN_CONDITION_RESULT>();
        Dictionary<EN_CONDITION_RESULT, string> m_dicPreEnumToString = new Dictionary<EN_CONDITION_RESULT, string>();
        Dictionary<string, EN_ACTION_RESULT> m_dicPostStringToEnum = new Dictionary<string, EN_ACTION_RESULT>();
        Dictionary<EN_ACTION_RESULT, string> m_dicPostEnumToString = new Dictionary<EN_ACTION_RESULT, string>();
        #endregion
        
        #endregion

        #region INTERFACE

        #region INITIALIZE
        public bool Init()
        {
            m_instanceStorage = Functional.Storage.GetInstance();

            if (!m_instanceStorage.Load(Define.DefineEnumBase.Storage.EN_STORAGE_LIST.FLOW))
                return false;

            m_DynamicLink = DynamicLink.GetInstance();

            MakeConversionTables();

            return InitFlow();
        }
        #endregion

        #region Get Information
        /// <summary>
        /// 2020.06.19 by yjlee [ADD] Get the count of the flow item in the task.
        /// </summary>
        public int GetCountOfFlowItem(string sTask, string sTable)
        {
            return m_DynamicLink.GetCountOfFlowItem(sTask, sTable);
        }

        /// <summary>
        /// 2020.06.22 by yjlee [ADD] Get the name of the action in the flow item.
		/// 2021.07.30 by twkang [MOD]
        /// </summary>
        public bool GetActionNameOfFlowItem(string strTask, string strTableName, int nFlowIndex, ref string strActionName)
        {
			strActionName = string.Empty;

			strActionName = m_DynamicLink.GetActionName(strTask, strTableName, nFlowIndex);

            return strActionName != string.Empty;
        }
        public int GetPreconditionFlowNo(string strTask, string strTableName, int nFlowIndex, EN_CONDITION_RESULT enType)
        {
            return m_DynamicLink.GetPreconditionFlowNo(strTask, strTableName, nFlowIndex, enType);
        }
        public int GetPostconditionFlowNo(string strTask, string strTableName, int nFlowIndex, EN_ACTION_RESULT enType)
        {
            return m_DynamicLink.GetPostconditionFlowNo(strTask, strTableName, nFlowIndex, enType);
        }

		/// <summary>
		/// 2021.07.30 by twkang [ADD] Get the list of flowtable
		/// </summary>
		public bool GetListOfFlowTable(string strTask, ref string[] arFlowTable)
		{
			return m_DynamicLink.GetFlowTableList(strTask, ref arFlowTable);
		}
        #endregion

        // from here : now
        #region Set Information
        /// <summary>
        /// 2022.03.02 by shkim [MOD] Fixed for new dynamic link
        /// 2020.06.22 by yjlee [ADD] Add a flow item in the structure.
        /// </summary>
        public bool AddFlowItem(string sTask, string sTable, string sActionName)
        {
            string[] arGroup = new string[] { sTask, sTable };
            string[] arData = null;

            int nCountOfFlowItem = 0;

            ActionNode tSourceNode;

            if (!m_DynamicLink.GetActionNode(sTask, sActionName, out tSourceNode)) return false;

            nCountOfFlowItem = m_DynamicLink.GetCountOfFlowItem(sTask, sTable);

            int nAddFlowNo = nCountOfFlowItem;
            if (m_DynamicLink.AddFlowItem(sTask, sTable, nAddFlowNo, tSourceNode))
            {
                for (int nIndexOfResult = 0; nIndexOfResult < m_arPreconditionEnumNames.Length; ++nIndexOfResult)
                {
                    int nTargetNo = -1;

                    m_DynamicLink.SetPreconditionFlowNo(sTask, sTable, nAddFlowNo, m_dicPreStringToEnum[m_arPreconditionEnumNames[nIndexOfResult]], nTargetNo);
                }

                for (int nIndexOfResult = 0; nIndexOfResult < m_arPostconditionEnumNames.Length; ++nIndexOfResult)
                {
                    int nTargetNo = -1;

                    m_DynamicLink.SetPostconditionFlowNo(sTask, sTable, nAddFlowNo, m_dicPostStringToEnum[m_arPostconditionEnumNames[nIndexOfResult]], nTargetNo);
                }

                if (FileBorn.GetInstance().GetItemFrame(BORN_LIST.FLOW, ref arGroup, nAddFlowNo.ToString(), ref arData)
                    && m_instanceStorage.AddGroupAndItem(Define.DefineEnumBase.Storage.EN_STORAGE_LIST.FLOW, ref arData))
                {
                    return SaveParameterToStorage(sTask, sTable, nCountOfFlowItem);
                }
            }

            return false;
        }

        /// <summary>
        /// 2022.03.02 by shkim [MOD] Fixed for new dynamic link
        /// 2020.06.25 by yjlee [ADD] Insert a flow item in the structure.
        /// </summary>
        public bool InsertFlowItem(string sTask, string sTable, string sActionName, int nFlowNo, bool bModifyOthersStepNumber = false)
        {
            string[] arGroup = new string[] { sTask, sTable, nFlowNo.ToString() };
            string[] arData = null;

            int insertFlowNo = nFlowNo;

            ActionNode tSourceNode;
            if (!m_DynamicLink.GetActionNode(sTask, sActionName, out tSourceNode)) return false;

            int nCountOfFlowItem = 0;
            nCountOfFlowItem = m_DynamicLink.GetCountOfFlowItem(sTask, sTable);

            for (; nFlowNo < nCountOfFlowItem; --nCountOfFlowItem)
            {
                arGroup[2] = (nCountOfFlowItem - 1).ToString();

                if (false == m_instanceStorage.CopyGroup(Define.DefineEnumBase.Storage.EN_STORAGE_LIST.FLOW
                    , nCountOfFlowItem.ToString()
                    , ref arGroup))
                {
                    return false;
                }

                if (false == m_instanceStorage.RemoveGroup(Define.DefineEnumBase.Storage.EN_STORAGE_LIST.FLOW
                    , ref arGroup))
                {
                    return false;
                }
            }

            if (m_DynamicLink.InsertFlowItem(sTask, sTable, nFlowNo, tSourceNode))
            {
                for (int nIndexOfResult = 0; nIndexOfResult < m_arPreconditionEnumNames.Length; ++nIndexOfResult)
                {
                    int nTargetNo = -1;

                    m_DynamicLink.SetPreconditionFlowNo(sTask,
                                                        sTable,
                                                        insertFlowNo,
                                                        m_dicPreStringToEnum[m_arPreconditionEnumNames[nIndexOfResult]],
                                                        nIndexOfResult == (int)EN_CONDITION_RESULT.OK ? nFlowNo : nTargetNo);
                }

                for (int nIndexOfResult = 0; nIndexOfResult < m_arPostconditionEnumNames.Length; ++nIndexOfResult)
                {
                    int nTargetNo = -1;
                    m_DynamicLink.SetPostconditionFlowNo(sTask
                                                        , sTable
                                                        , insertFlowNo
                                                        , m_dicPostStringToEnum[m_arPostconditionEnumNames[nIndexOfResult]]
                                                        , nTargetNo);
                }

                if (FileBorn.GetInstance().GetItemFrame(BORN_LIST.FLOW, ref arGroup, insertFlowNo.ToString(), ref arData)
                    && m_instanceStorage.AddGroupAndItem(Define.DefineEnumBase.Storage.EN_STORAGE_LIST.FLOW, ref arData))
                {
                    bool bInsertFlowItemSaveResult = SaveParameterToStorage(sTask, sTable, nCountOfFlowItem);

                    if (bInsertFlowItemSaveResult && bModifyOthersStepNumber)
                    {
                        const int START_END_STEP_NUMBER = 0;
                        const int UNDEFINED_STEP_NUBBER = -1;
                        
                        nCountOfFlowItem = m_DynamicLink.GetCountOfFlowItem(sTask, sTable);
                        for (int nModifyingTargetStepNo = 0; nModifyingTargetStepNo < nCountOfFlowItem; nModifyingTargetStepNo++)
                        {
                            if (nModifyingTargetStepNo <= nFlowNo)
                            {
                                continue;
                            }

                            for (int nIndexOfResult = 0; nIndexOfResult < m_arPreconditionEnumNames.Length; ++nIndexOfResult)
                            {
                                int nCurrentTargetNo = m_DynamicLink.GetPreconditionFlowNo(sTask, sTable, nModifyingTargetStepNo, m_dicPreStringToEnum[m_arPreconditionEnumNames[nIndexOfResult]]);
                                if (nCurrentTargetNo != START_END_STEP_NUMBER && nCurrentTargetNo != UNDEFINED_STEP_NUBBER)
                                {
                                    m_DynamicLink.SetPreconditionFlowNo(sTask, sTable, nModifyingTargetStepNo, m_dicPreStringToEnum[m_arPreconditionEnumNames[nIndexOfResult]], ++nCurrentTargetNo);
                                }
                            }

                            for (int nIndexOfResult = 0; nIndexOfResult < m_arPostconditionEnumNames.Length; ++nIndexOfResult)
                            {
                                int nCurrentTargetNo = m_DynamicLink.GetPostconditionFlowNo(sTask, sTable, nModifyingTargetStepNo, m_dicPostStringToEnum[m_arPostconditionEnumNames[nIndexOfResult]]);
                                if (nCurrentTargetNo != START_END_STEP_NUMBER && nCurrentTargetNo != UNDEFINED_STEP_NUBBER)
                                {

                                    m_DynamicLink.SetPostconditionFlowNo(sTask, sTable, nModifyingTargetStepNo, m_dicPostStringToEnum[m_arPostconditionEnumNames[nIndexOfResult]], ++nCurrentTargetNo);
                                }
                            }
                            bInsertFlowItemSaveResult &= SaveParameterToStorage(sTask, sTable, nModifyingTargetStepNo);
                        }
                    }
                    return bInsertFlowItemSaveResult;
                }
            }
            return false;
        }

        /// <summary>
        /// 2022.03.02 by shkim [MOD] Fixed for new dynamic link
        /// 2020.06.25 by yjlee [ADD] Remove a flow item in the structure.
        /// </summary>
        public bool RemoveFlowItem(string sTask, string sTable, int nFlowNumber, bool bModifyOtherStepNumber = false)
        {
            int nRemoveTargetFlowNumber = nFlowNumber;
            string[] arGroup = new string[] { sTask, sTable, nRemoveTargetFlowNumber.ToString() };

            if (m_DynamicLink.RemoveFlowItem(sTask, sTable, nRemoveTargetFlowNumber))
            {
                if (m_instanceStorage.RemoveGroup(Define.DefineEnumBase.Storage.EN_STORAGE_LIST.FLOW, ref arGroup))
                {
                    int nCountOfFlowItem = 0;
                    nCountOfFlowItem = m_DynamicLink.GetCountOfFlowItem(sTask, sTable);

                    for (; nFlowNumber < nCountOfFlowItem; ++nFlowNumber)
                    {
                        arGroup[2] = (nFlowNumber + 1).ToString();

                        if (false == m_instanceStorage.CopyGroup(Define.DefineEnumBase.Storage.EN_STORAGE_LIST.FLOW
                            , nFlowNumber.ToString()
                            , ref arGroup))
                        {
                            return false;
                        }

                        if (false == m_instanceStorage.RemoveGroup(Define.DefineEnumBase.Storage.EN_STORAGE_LIST.FLOW
                            , ref arGroup))
                        {
                            return false;
                        }
                    }

                    if (false == bModifyOtherStepNumber)
                    {
                        return true;
                    }
                    else
                    {
                        const int START_END_STEP_NUMBER = 0;
                        const int UNDEFINED_STEP_NUBBER = -1;

                        nCountOfFlowItem = m_DynamicLink.GetCountOfFlowItem(sTask, sTable);
                        for (int nModifyingTargetStepNo = 0; nModifyingTargetStepNo < nCountOfFlowItem; nModifyingTargetStepNo++)
                        {
                            for (int nIndexOfResult = 0; nIndexOfResult < m_arPreconditionEnumNames.Length; ++nIndexOfResult)
                            {
                                int nCurrentSettingTargetNo = m_DynamicLink.GetPreconditionFlowNo(sTask
                                                                                        , sTable
                                                                                        , nModifyingTargetStepNo
                                                                                        , m_dicPreStringToEnum[m_arPreconditionEnumNames[nIndexOfResult]]);

                                if (nCurrentSettingTargetNo != START_END_STEP_NUMBER && nCurrentSettingTargetNo != UNDEFINED_STEP_NUBBER)
                                {
                                    // Pre 조건에서는 현재 셋팅된 번호가 지워진 번호보다 작으면 수정할 필요가 없다.
                                    if (nCurrentSettingTargetNo < nRemoveTargetFlowNumber)
                                    {
                                        continue;
                                    }
                                    m_DynamicLink.SetPreconditionFlowNo(sTask
                                                                    , sTable
                                                                    , nModifyingTargetStepNo
                                                                    , m_dicPreStringToEnum[m_arPreconditionEnumNames[nIndexOfResult]]
                                                                    , --nCurrentSettingTargetNo);
                                }
                            }

                            for (int nIndexOfResult = 0; nIndexOfResult < m_arPostconditionEnumNames.Length; ++nIndexOfResult)
                            {
                                int nCurrentSettingTargetNo = m_DynamicLink.GetPostconditionFlowNo(sTask
                                                                                        , sTable
                                                                                        , nModifyingTargetStepNo
                                                                                        , m_dicPostStringToEnum[m_arPostconditionEnumNames[nIndexOfResult]]);

                                if (nCurrentSettingTargetNo != START_END_STEP_NUMBER && nCurrentSettingTargetNo != UNDEFINED_STEP_NUBBER)
                                {
                                    // Post 조건에서는 현재 셋팅된 번호가 지워진 번호보다 작거나 같으면 수정할 필요가 없다.
                                    if (nCurrentSettingTargetNo <= nRemoveTargetFlowNumber)
                                    {
                                        continue;
                                    }
                                    m_DynamicLink.SetPostconditionFlowNo(sTask
                                                                        , sTable
                                                                        , nModifyingTargetStepNo
                                                                        , m_dicPostStringToEnum[m_arPostconditionEnumNames[nIndexOfResult]]
                                                                        , --nCurrentSettingTargetNo);
                                }
                            }
                            bModifyOtherStepNumber &= SaveParameterToStorage(sTask, sTable, nModifyingTargetStepNo);
                        }
                        return bModifyOtherStepNumber;
                    }
                }
            }

            return false;
        }

        public bool RemoveFlowTableAtLinkAndConfigFile(string sTask, string sTable)
        {
            bool bResultOfDeletingTableAtLink = m_DynamicLink.RemoveFlowTable(sTask, sTable);
            
            string [] targetGroups = new string[]{ sTask, sTable};

            bool bResultOfDeletingTableAtConfigFile = m_instanceStorage.RemoveGroup(Define.DefineEnumBase.Storage.EN_STORAGE_LIST.FLOW, ref targetGroups);

            return (bResultOfDeletingTableAtLink && bResultOfDeletingTableAtConfigFile);
        }

        #endregion

        #endregion

        #region METHOD

        #region INITIALIZE
        private bool InitFlow()
        {
            string[] arTask = null;

            if (!Config.ConfigTask.GetInstance().GetListOfTask(ref arTask))
                return false;

            for(int nIndex = 0, nEnd = arTask.Length ; nIndex < nEnd ; ++ nIndex)
            {
                string sTask = arTask[nIndex];

                m_DynamicLink.AddFlowLink(sTask);

                string[] arGroupList = null;    // ARRAY 만 선언 (참조를 전달 받는 객체에서 메모리 할당)

                if (LoadListOfTableGroup(sTask, ref arGroupList))  // GROUP LIST: TABLE NAMES
                {
                    for (int nTable = 0, nTableEnd = arGroupList.Length; nTable < nTableEnd; ++nTable)
                    {
                        string sTable = arGroupList[nTable];

                        m_DynamicLink.AddFlowTable(sTask, sTable);

                        string[] arFlowItem = null;     // ARRAY 만 선언 (참조를 전달 받는 객체에서 메모리 할당)

                        if (LoadListOfItemGroup(sTask, sTable, ref arFlowItem)) // GROUP LIST: FLOW INDEX
                        {
                            int nFlowItemCount = arFlowItem.Length;

                            string[] arAction = new string[nFlowItemCount];

                            for (int i = 0; i < nFlowItemCount; ++i)
                            {
                                string sAction = string.Empty;
                                int nFlowIndex = 0;

                                if (false == int.TryParse(arFlowItem[i], out nFlowIndex))
                                    continue;

                                if (LoadActionName(sTask, sTable, nFlowIndex, ref sAction))
                                {
                                    arAction[nFlowIndex] = sAction;
                                }
                            }

                            for (int i = 0; i < nFlowItemCount; ++i)
                            {
                                int nFlowIndex = 0;

                                if (false == int.TryParse(arFlowItem[i], out nFlowIndex))
                                    continue;

                                ActionNode tSourceNode;

                                if (!m_DynamicLink.GetActionNode(sTask, arAction[nFlowIndex], out tSourceNode))
                                    continue;

                                if (!m_DynamicLink.AddFlowItem(sTask, sTable, nFlowIndex, tSourceNode))
                                    continue;

                                if (LoadCondition(sTask, sTable, nFlowIndex, GROUP_PRECONDITION, m_arPreconditionEnumNames, ref m_arPreconditionItemValues))
                                {
                                    for (int nIndexOfResult = 0; nIndexOfResult < m_arPreconditionEnumNames.Length; ++nIndexOfResult)
                                    {
                                        int nTargetNo = m_arPreconditionItemValues[nIndexOfResult];

                                        m_DynamicLink.SetPreconditionFlowNo(sTask, sTable, nFlowIndex, m_dicPreStringToEnum[m_arPreconditionEnumNames[nIndexOfResult]], nTargetNo);
                                    }
                                }

                                if (LoadCondition(sTask, sTable, nFlowIndex, GROUP_POSTCONDITION, m_arPostconditionEnumNames, ref m_arPostconditionItemValues))
                                {
                                    for (int nIndexOfResult = 0; nIndexOfResult < m_arPostconditionEnumNames.Length; ++nIndexOfResult)
                                    {
                                        int nTargetNo = m_arPostconditionItemValues[nIndexOfResult];

                                        m_DynamicLink.SetPostconditionFlowNo(sTask, sTable, nFlowIndex, m_dicPostStringToEnum[m_arPostconditionEnumNames[nIndexOfResult]], nTargetNo);
                                    }
                                }
                            }
                        }
                    }
                }
            }

            return true;
        }
        #endregion

        #region LOAD FILE
        private bool LoadListOfTableGroup(string sTask, ref string[] arGroupList)
        {
            string[] arGroup = new string[] { sTask };

            return m_instanceStorage.GetListOfGroup(Define.DefineEnumBase.Storage.EN_STORAGE_LIST.FLOW
                , ref arGroupList
                , arGroup);
        }
        private bool LoadListOfItemGroup(string sTask, string sTable, ref string[] arItemList)
        {
            string[] arGroup = new string[] { sTask, sTable };

            return m_instanceStorage.GetListOfGroup(Define.DefineEnumBase.Storage.EN_STORAGE_LIST.FLOW
                , ref arItemList
                , arGroup);
        }
        private bool LoadActionName(string sTask, string sTable, int nFlowNo, ref string sAction)
        {
            string[] arGroup = new string[] { sTask, sTable, nFlowNo.ToString() };

            return m_instanceStorage.GetParameter(Define.DefineEnumBase.Storage.EN_STORAGE_LIST.FLOW
                , ITEM_FLOW_ACTION
                , ref sAction
                , ref arGroup);
        }
        private bool LoadCondition(string sTask, string sTable, int nFlowNo, string sConditionGroup, string[] sItems, ref int[] nValues)
        {
            string[] arGroup = new string[] { sTask, sTable, nFlowNo.ToString(), sConditionGroup };

            for (int nIndex = 0; nIndex < sItems.Length; ++nIndex)
            {
                if (false == m_instanceStorage.GetParameter(Define.DefineEnumBase.Storage.EN_STORAGE_LIST.FLOW
                    , sItems[nIndex]
                    , ref nValues[nIndex]
                    , ref arGroup))
                {
                    return false;
                }
            }

            return true;
        }
        #endregion

        #region SAVE FILE
        public bool SaveActionName(string sTask, string sTable, int nFlowNo, string sAction)
        {
            string[] arGroup = new string[] { sTask, sTable, nFlowNo.ToString() };

            if (m_instanceStorage.SetParameter(Define.DefineEnumBase.Storage.EN_STORAGE_LIST.FLOW
                , ITEM_FLOW_ACTION
                , sAction
                , ref arGroup))
            {
                return m_DynamicLink.SetActionName(sTask, sTable, nFlowNo, sAction);
            }

            return false;
        }
        /// <summary>
        /// 2022.03.02 by shkim [MOD] Fixed method name
        /// </summary>
        public bool SavePreconditionTargetNo(string sTask, string sTable, int nFlowIndex, EN_CONDITION_RESULT enResult, int nTargetNo)
        {
            string[] arGroup = new string[] { sTask, sTable, nFlowIndex.ToString(), GROUP_PRECONDITION };

            if (m_instanceStorage.SetParameter(Define.DefineEnumBase.Storage.EN_STORAGE_LIST.FLOW
                , m_dicPreEnumToString[enResult]
                , nTargetNo
                , ref arGroup))
            {
                return m_DynamicLink.SetPreconditionFlowNo(sTask, sTable, nFlowIndex, enResult, nTargetNo);
            }

            return false;
        }
        /// <summary>
        /// 2022.03.02 by shkim [MOD] Fixed method name
        /// </summary>
        public bool SavePostconditionTargetNo(string sTask, string sTable, int nFlowNo, EN_ACTION_RESULT enResult, int nTargetNo)
        {
            string[] arGroup = new string[] { sTask, sTable, nFlowNo.ToString(), GROUP_POSTCONDITION };

            if (m_instanceStorage.SetParameter(Define.DefineEnumBase.Storage.EN_STORAGE_LIST.FLOW
                , m_dicPostEnumToString[enResult]
                , nTargetNo
                , ref arGroup))
            {
                return m_DynamicLink.SetPostconditionFlowNo(sTask, sTable, nFlowNo, enResult, nTargetNo);
            }

            return false;
        }
        public bool SaveParameterToStorage(string sTask, string sTable, int nFlowIndex)
        {
            string[] arGroup = new string[] { sTask, sTable, nFlowIndex.ToString() };
            string sActionName = Define.DefineConstant.Common.NONE;

            sActionName = m_DynamicLink.GetActionName(sTask, sTable, nFlowIndex);

            m_instanceStorage.SetParameter(Define.DefineEnumBase.Storage.EN_STORAGE_LIST.FLOW
                , ITEM_FLOW_ACTION, sActionName, ref arGroup);

            string[] arGroupCondition = new string[] { sTask, sTable, nFlowIndex.ToString(), GROUP_PRECONDITION };

            bool isSetParameterSuccess = false;

            foreach (var kvp in m_dicPreEnumToString)
            {
                int nFlowNo = m_DynamicLink.GetPreconditionFlowNo(sTask, sTable, nFlowIndex, kvp.Key);

                isSetParameterSuccess = m_instanceStorage.SetParameter(Define.DefineEnumBase.Storage.EN_STORAGE_LIST.FLOW
                    , kvp.Value
                    , nFlowNo
                    , ref arGroupCondition
                    , false);
            }

            arGroupCondition[3] = GROUP_POSTCONDITION;
            foreach (var kvp in m_dicPostEnumToString)
            {
                int nFlowNo = m_DynamicLink.GetPostconditionFlowNo(sTask, sTable, nFlowIndex, kvp.Key);

                m_instanceStorage.SetParameter(Define.DefineEnumBase.Storage.EN_STORAGE_LIST.FLOW
                    , kvp.Value
                    , nFlowNo
                    , ref arGroupCondition
                    , false);
            }
            return m_instanceStorage.Save(Define.DefineEnumBase.Storage.EN_STORAGE_LIST.FLOW);
        }

        
        #endregion

        #region CONVERSION
        private void MakeConversionTables()
        {
            var arPreResult = Enum.GetValues(typeof(EN_CONDITION_RESULT));
            
            m_arPreconditionEnumNames = new string[arPreResult.Length];
            m_arPreconditionItemValues = new int[arPreResult.Length];

            int nPreIndex = -1;

            foreach (EN_CONDITION_RESULT enResult in arPreResult)
            {
                string sResult = enResult.ToString();

                m_dicPreStringToEnum.Add(sResult, enResult);
                m_dicPreEnumToString.Add(enResult, sResult);

                m_arPreconditionEnumNames[++nPreIndex] = sResult;
            }

            var arPostResult = Enum.GetValues(typeof(EN_ACTION_RESULT));
            m_arPostconditionEnumNames = new string[arPostResult.Length];
            m_arPostconditionItemValues = new int[arPostResult.Length];

            int nPostIndex = -1;

            foreach (EN_ACTION_RESULT enResult in arPreResult)
            {
                string sResult = enResult.ToString();

                m_dicPostStringToEnum.Add(sResult, enResult);
                m_dicPostEnumToString.Add(enResult, sResult);

                m_arPostconditionEnumNames[++nPostIndex] = sResult;
            }
        }
        #endregion

        #endregion
    }
}
