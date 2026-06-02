using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;

using TaskAction_;
using FileBorn_;

using FrameOfSystem3.DynamicLink_;

namespace FrameOfSystem3.Config
{
    #region ENUMERATION
    public enum EN_STATE_TRANSITION
    {
        DONE_TO_READY,
        READY_TO_WAIT,
        WAIT_TO_BUSY
    }
    #endregion

    public class ConfigDynamicLink
    {
        #region SINGLETON
        private ConfigDynamicLink() { }
        private static ConfigDynamicLink m_instance = new ConfigDynamicLink();
        public static ConfigDynamicLink GetInstance(){ return m_instance; }
        #endregion

        #region STRUCTURE
        private struct LinkItem
        {
            #region FIELD
            private int m_nIndexOfItem;
            private string m_sLinkGroup;
            private string m_sTargetTask;
            private string m_sTargetItem;
            private int m_nGroupKey;
            private string m_sLinkKey;
            #endregion

            #region PROPERTY
            public int IndexOfItem { set { m_nIndexOfItem = value; } get { return m_nIndexOfItem; } }
            public string LinkGroup { set { m_sLinkGroup = value; } get { return m_sLinkGroup; } }
            public string TargetTask { set { m_sTargetTask = value; } get { return m_sTargetTask; } }
            public string TargetItem { set { m_sTargetItem = value; } get { return m_sTargetItem; } }
            public int GroupKey { set { m_nGroupKey = value; } get { return m_nGroupKey; } }
            public string LinkKey { set { m_sLinkKey = value; } get { return m_sLinkKey; } }
            #endregion

            #region CONSTRUCTOR
            public LinkItem(int nIndex, string sLinkGroup, string sTargetTask, string sTargetItem, int nGroupKey, string sLinkKey)
            {
                m_nIndexOfItem = nIndex;
                m_sLinkGroup = sLinkGroup;
                m_sTargetTask = sTargetTask;
                m_sTargetItem = sTargetItem;
                m_nGroupKey = nGroupKey;
                m_sLinkKey = sLinkKey;
                
            }
            #endregion

            #region INTERFACE
            public bool GetNodeLinkGroup(int nGroupKey, string sLinkKey, ref string sLinkGroup)
            {
                if (m_nGroupKey.Equals(nGroupKey) && m_sLinkKey.Equals(sLinkKey))
                {
                    sLinkGroup = m_sLinkGroup;

                    return true;
                }

                return false;
            }
            #endregion
        }
        #endregion

        #region CONSTANT
        private readonly string ITEM_ACTION_NAME        = "ACTION_NAME";
        private readonly string ITEM_ACTION_ENABLE      = "ACTION_ENABLE";

        private readonly string GROUP_NODE_LINKS        = "NODE_LINKS";
        private readonly string GROUP_PORT_LINKS        = "PORT_LINKS";

        private readonly string ITEM_TARGET_TASK        = "TASK_NAME";
        private readonly string ITEM_TARGET_NODE        = "NODE_NAME";
        private readonly string ITEM_TARGET_PORT        = "PORT_NAME";
        private readonly string ITEM_LINK_ENABLE        = "LINK_ENABLE";
        private readonly string ITEM_GROUP_KEY          = "KEY_GROUP";      // 2021.07.26 by jhchoo [MOD] 문자열 맨앞에 GROUP 이 오면 파일구조에서 GROUP LEVEL 로 오인식 
        private readonly string ITEM_LINK_KEY           = "LINK_KEY";
        
        private readonly string GROUP_STATE_CONDITION   = "STATE_CONDITION";
        private readonly string GROUP_STATUS_CONDITION  = "STATUS_CONDITION";

        private const int MAX_LINK_COUNT                = 99;
		private const int EMPTY_ITEM_SIZE				= 0;
        #endregion

        #region FIELD
        Functional.Storage m_instanceStorage = null;
        TaskActionManager m_instanceActionManager = null;   // from here : now

        DynamicLink m_DynamicLink = null;

        string[] m_arTask = null;
        int[] m_arTaskIndex = null;

        #region CONVERSION
        Dictionary<string, EN_ACTION_STATE> m_dicActionStateStringToEnum = new Dictionary<string, EN_ACTION_STATE>();
        Dictionary<EN_ACTION_STATE, string> m_dicActionStateEnumToString = new Dictionary<EN_ACTION_STATE, string>();
        Dictionary<string, EN_STATE_TRANSITION> m_dicStateTransitionStringToEnum = new Dictionary<string, EN_STATE_TRANSITION>();
        Dictionary<EN_STATE_TRANSITION, EN_ACTION_STATE> m_dicStateTransitionToActionState = new Dictionary<EN_STATE_TRANSITION, EN_ACTION_STATE>();
        Dictionary<string, EN_PORT_STATUS> m_dicPortStatusStringToEnum = new Dictionary<string, EN_PORT_STATUS>();
        Dictionary<EN_PORT_STATUS, string> m_dicPortStatusEnumToString = new Dictionary<EN_PORT_STATUS, string>();
        #endregion

        /// <summary>
        /// KEY  : TASK NAME,
        /// VALUE: ACTION GROUP (KEY: ACTION NAME, VALUE: GROUP NAME)
        /// </summary>
        Dictionary<string, Dictionary<string, string>> m_dicTaskNameToActionGroup = new Dictionary<string, Dictionary<string, string>>();

        /// <summary>
        /// KEY  : TASK NAME,
        /// VALUE: LINK GROUP (KEY: ACTION NAME, VALUE: NODE LINKS)
        /// </summary>
        Dictionary<string, Dictionary<string, List<LinkItem>>> m_dicNodeLinkItems = new Dictionary<string,Dictionary<string,List<LinkItem>>>();

        /// <summary>
        /// KEY  : TASK NAME,
        /// VALUE: LINK GROUP (KEY: ACTION NAME, VALUE: PORT LINKS)
        /// </summary>
        Dictionary<string, Dictionary<string, List<LinkItem>>> m_dicPortLinkItems = new Dictionary<string, Dictionary<string, List<LinkItem>>>();

        /// <summary>
        /// KEY  : TASK NAME,
        /// VALUE: ACTION NAMES
        /// </summary>
        Dictionary<string, string[]> m_dicActionNames = new Dictionary<string,string[]>();

        /// <summary>
        /// KEY  : TASK NAME,
        /// VALUE: PORT NAMES
        /// </summary>
        Dictionary<string, string[]> m_dicPortNames = new Dictionary<string, string[]>();
        #endregion

		// 2020.09.16 by twkang [ADD]
		#region 스톱워치
		Dictionary<string, Stopwatch> m_DicForStopwatch	= new Dictionary<string,Stopwatch>();
		Dictionary<string, Dictionary<string, double>> m_dicTimeOfAction = new Dictionary<string,Dictionary<string,double>>();
		#endregion

		#region METHOD

		#region INITIALIZE
        private bool LoadListOfTask()
        {
            int[] arIndexOfInstance = null;
            string[] arTask = null;

            if (false == Config.ConfigTask.GetInstance().GetListOfTask(ref arTask)
                || false == Config.ConfigTask.GetInstance().GetListOfIndexOfInstance(ref arIndexOfInstance))
            {
                return false;
            }

            int nCountOfTask = arTask.Length;

            if (nCountOfTask < 1)
                return false;

            m_arTask = new string[nCountOfTask];
            m_arTaskIndex = new int[nCountOfTask];

            for (int nIndex = 0, nEnd = arTask.Length; nIndex < nEnd; ++nIndex)
            {
                m_arTask[nIndex] = arTask[nIndex];
                m_arTaskIndex[nIndex] = arIndexOfInstance[nIndex];
            }

            return true;
        }
        private bool AddNodeLink()
        {
            // 2022.03.17. by shkim. [ADD] 등록된 Task가 없을 때
            if (m_arTask == null) return false;

            foreach (string sTask in m_arTask)
            {
                m_dicTaskNameToActionGroup.Add(sTask, new Dictionary<string, string>());

                string[] arGroupList = null;

                if(LoadListOfActionGroup(sTask, ref arGroupList))  // GROUP LIST: ACTION_0, ACTION_1, ..., ACTION_N
                {
                    for(int nIndex = 0, nEnd = arGroupList.Length ; nIndex < nEnd ; ++ nIndex)
                    {
                        string sActionName = string.Empty;
                        bool bActionEnable = false;

                        if (LoadNameOfAction(sTask, arGroupList[nIndex], ref sActionName, ref bActionEnable)
                            && m_DynamicLink.AddNodeLink(sTask, sActionName))
                        {
                            m_dicTaskNameToActionGroup[sTask].Add(sActionName, arGroupList[nIndex]);

                            m_DynamicLink.SetActionEnable(sTask, sActionName, bActionEnable);
                        }
                        else
                        {
                            return false;
                        }
                    }

                    string[] arAction = m_dicTaskNameToActionGroup[sTask].Keys.ToArray();
                    m_dicActionNames.Add(sTask, arAction);
                }
            }

            return true;
        }
        private bool AddNodeLinkItem()
        {
            foreach (var kvp in m_dicTaskNameToActionGroup)
            {
                string sTask = kvp.Key;

                m_dicNodeLinkItems.Add(sTask, new Dictionary<string, List<LinkItem>>());

                foreach(var kvpAction in kvp.Value)
                {
                    string sAction = kvpAction.Key;
                    string sActionGroup = kvpAction.Value;
                    
                    m_dicNodeLinkItems[sTask].Add(sAction, new List<LinkItem>());

                    string[] arNodeLinkGroup = null;

                    if (LoadListOfNodeLinkGroup(sTask, sActionGroup, ref arNodeLinkGroup))   // GROUP LIST: GROUP 0, GROUP 1, ..., GROUP N
                    {
                        for(int nIndex = 0, nEnd = arNodeLinkGroup.Length ; nIndex < nEnd ; ++ nIndex)
                        {
                            string sTargetTask = string.Empty;
                            string sTargetAction = string.Empty;
                            bool bLinkEnable = true;
                            int nGroupKey = -1;
                            string sLinkKey = string.Empty;
                            
                            string sNodeLinkGroup = arNodeLinkGroup[nIndex];

                            if(LoadNodeLink(sTask, sActionGroup, sNodeLinkGroup, ref sTargetTask, ref sTargetAction, ref bLinkEnable, ref nGroupKey, ref sLinkKey)
                                && m_DynamicLink.AddNodeLinkItem(sTask, sAction, sTargetTask, sTargetAction, nGroupKey, sLinkKey))
                            {
                                m_DynamicLink.SetNodeLinkEnable(sTask, sAction, nGroupKey, sLinkKey, bLinkEnable);

                                foreach (var kvpLink in m_dicStateTransitionStringToEnum)
                                {
                                    string sStateTransition = kvpLink.Key;
                                    string sStateCondition = string.Empty;

                                    EN_ACTION_STATE enNodeState = m_dicStateTransitionToActionState[kvpLink.Value];

                                    if (LoadStateCondition(sTask, sActionGroup, sNodeLinkGroup, sStateTransition, ref sStateCondition))
                                    {
                                        m_DynamicLink.SetStateCondition(sTask, sAction, nGroupKey, sLinkKey, enNodeState, m_dicActionStateStringToEnum[sStateCondition]);
                                    }
                                }

                                int nNodeLinkIndex = -1;

                                if(int.TryParse(sNodeLinkGroup, out nNodeLinkIndex))
                                {
                                    m_dicNodeLinkItems[sTask][sAction].Add(new LinkItem(nNodeLinkIndex, sNodeLinkGroup, sTargetTask, sTargetAction, nGroupKey, sLinkKey));
                                }
                            }
                        }
                    }
                }
            }

            // PAIR LINK 검색 및 설정
            SetNodeLinkPair();

            return true;
        }
        private bool AddPortLinkItem()
        {
            foreach (var kvp in m_dicTaskNameToActionGroup)
            {
                string sTask = kvp.Key;

                m_dicPortLinkItems.Add(sTask, new Dictionary<string, List<LinkItem>>());

                foreach (var kvpAction in kvp.Value)
                {
                    string sAction = kvpAction.Key;
                    string sActionGroup = kvpAction.Value;

                    m_dicPortLinkItems[sTask].Add(sAction, new List<LinkItem>());

                    string[] arPortLinkGroup = null;

                    if (LoadListOfPortLinkGroup(sTask, sActionGroup, ref arPortLinkGroup))    // GROUP LIST: GROUP 0, GROUP 1, ..., GROUP N
                    {
                        for (int nIndex = 0, nEnd = arPortLinkGroup.Length; nIndex < nEnd; ++nIndex)
                        {
                            string sTargetTask = string.Empty;
                            string sTargetPort = string.Empty;
                            bool bLinkEnable = true;
                            int nGroupKey = -1;
                            string sLinkKey = string.Empty;

                            string sPortLinkGroup = arPortLinkGroup[nIndex];

                            if (LoadPortLink(sTask, sActionGroup, sPortLinkGroup, ref sTargetTask, ref sTargetPort, ref bLinkEnable, ref nGroupKey, ref sLinkKey)
                                && m_DynamicLink.AddPortLinkItem(sTask, sAction, sTargetTask, sTargetPort, nGroupKey, sLinkKey))
                            {
                                m_DynamicLink.SetPortLinkEnable(sTask, sAction, nGroupKey, sLinkKey, bLinkEnable);

                                string sStatusCondition = string.Empty;

                                if (LoadStatusCondition(sTask, sActionGroup, sPortLinkGroup, ref sStatusCondition))
                                {
                                    m_DynamicLink.SetStatusCondition(sTask, sAction, nGroupKey, sLinkKey, m_dicPortStatusStringToEnum[sStatusCondition]);
                                }

                                int nIndexOfPortGroup = -1;

                                if (int.TryParse(sPortLinkGroup, out nIndexOfPortGroup))
                                {
                                    m_dicPortLinkItems[sTask][sAction].Add(new LinkItem(nIndexOfPortGroup
                                        , sPortLinkGroup
                                        , sTargetTask
                                        , sTargetPort
                                        , nGroupKey
                                        , sLinkKey));
                                }
                            }
                        }
                    }
                }
            }

            return true;
        }
		/// <summary>
		/// 2020.09.16 by twkang [ADD] 시간측정을 위한 초기화이다.
		/// </summary>
		private bool AddActionTimerAll()
		{
			foreach(string str in m_dicActionNames.Keys)          // str = Task Name
			{
				m_DicForStopwatch.Add(str, new Stopwatch());

				Dictionary<string, double> dicForTimer	= new Dictionary<string,double>();

				foreach(string sTask in m_dicActionNames[str])  // sTask = Action Name
				{
					dicForTimer.Add(sTask, 0);
				}

				m_dicTimeOfAction.Add(str, dicForTimer);

			}

			return true;
		}
        #endregion

        #region LOAD FILE
        private bool LoadListOfActionGroup(string sTask, ref string[] arGroupList)
        {
            string[] arGroup = new string[]{ sTask };

            return m_instanceStorage.GetListOfGroup(Define.DefineEnumBase.Storage.EN_STORAGE_LIST.DYNAMIC_LINK
                , ref arGroupList
                , arGroup);
        }
        /// <summary>
        /// [NODE_LINKS] GROUP 의 하위 GROUP NAME 의 목록을 반환한다 (GROUP 0, GROUP 1, ..., GROUP N)
        /// </summary>
        private bool LoadListOfNodeLinkGroup(string sTask, string sActionGroup, ref string[] arGroupList)
        {
            string[] arGroup = new string[] { sTask, sActionGroup, GROUP_NODE_LINKS };

            return m_instanceStorage.GetListOfGroup(Define.DefineEnumBase.Storage.EN_STORAGE_LIST.DYNAMIC_LINK
                , ref arGroupList
                , arGroup);
        }
        /// <summary>
        /// [PORT_LINKS] GROUP 의 하위 GROUP NAME 의 목록을 반환한다 (GROUP 0, GROUP 1, ..., GROUP N)
        /// </summary>
        private bool LoadListOfPortLinkGroup(string sTask, string sActionGroup, ref string[] arGroupList)
        {
            string[] arGroup = new string[] { sTask, sActionGroup, GROUP_PORT_LINKS };

            return m_instanceStorage.GetListOfGroup(Define.DefineEnumBase.Storage.EN_STORAGE_LIST.DYNAMIC_LINK
                , ref arGroupList
                , arGroup);
        }
        /// <summary>
        /// 2020.06.16 by yjlee [ADD] Get the name of the action.
        /// </summary>
        private bool LoadNameOfAction(string sTask, string sActionGroup, ref string sAction, ref bool bEnable)
        {
            string[] arGroup = new string[] { sTask, sActionGroup };

            return m_instanceStorage.GetParameter(Define.DefineEnumBase.Storage.EN_STORAGE_LIST.DYNAMIC_LINK
                , ITEM_ACTION_NAME
                , ref sAction
                , ref arGroup)
                && m_instanceStorage.GetParameter(Define.DefineEnumBase.Storage.EN_STORAGE_LIST.DYNAMIC_LINK
                , ITEM_ACTION_ENABLE
                , ref bEnable
                , ref arGroup);
        }
        private bool LoadNodeLink(string sTask, string sActionGroup, string sNodeLinkGroup, 
                                      ref string sTargetTask, ref string sTargetNode, ref bool bLinkEnable, ref int nGroupKey, ref string sLinkKey)
        {
            string[] arGroup = new string[]{sTask, sActionGroup, GROUP_NODE_LINKS, sNodeLinkGroup};

            return m_instanceStorage.GetParameter(Define.DefineEnumBase.Storage.EN_STORAGE_LIST.DYNAMIC_LINK
                , ITEM_TARGET_TASK
                , ref sTargetTask
                , ref arGroup)
                && m_instanceStorage.GetParameter(Define.DefineEnumBase.Storage.EN_STORAGE_LIST.DYNAMIC_LINK
                , ITEM_TARGET_NODE
                , ref sTargetNode
                , ref arGroup)
                && m_instanceStorage.GetParameter(Define.DefineEnumBase.Storage.EN_STORAGE_LIST.DYNAMIC_LINK
                , ITEM_LINK_ENABLE
                , ref bLinkEnable
                , ref arGroup)
                && m_instanceStorage.GetParameter(Define.DefineEnumBase.Storage.EN_STORAGE_LIST.DYNAMIC_LINK
                , ITEM_GROUP_KEY
                , ref nGroupKey
                , ref arGroup)
                && m_instanceStorage.GetParameter(Define.DefineEnumBase.Storage.EN_STORAGE_LIST.DYNAMIC_LINK
                , ITEM_LINK_KEY
                , ref sLinkKey
                , ref arGroup);
                
        }
        private bool LoadPortLink(string sTask, string sActionGroup, string sPortLinkGroup,
                                      ref string sTargetTask, ref string sTargetPort, ref bool bLinkEnable, ref int nGroupKey, ref string sLinkKey)
        {
            string[] arGroup = new string[] { sTask, sActionGroup, GROUP_PORT_LINKS, sPortLinkGroup };

            return m_instanceStorage.GetParameter(Define.DefineEnumBase.Storage.EN_STORAGE_LIST.DYNAMIC_LINK
                , ITEM_TARGET_TASK
                , ref sTargetTask
                , ref arGroup)
                && m_instanceStorage.GetParameter(Define.DefineEnumBase.Storage.EN_STORAGE_LIST.DYNAMIC_LINK
                , ITEM_TARGET_PORT
                , ref sTargetPort
                , ref arGroup)
                && m_instanceStorage.GetParameter(Define.DefineEnumBase.Storage.EN_STORAGE_LIST.DYNAMIC_LINK
                , ITEM_LINK_ENABLE
                , ref bLinkEnable
                , ref arGroup)
                && m_instanceStorage.GetParameter(Define.DefineEnumBase.Storage.EN_STORAGE_LIST.DYNAMIC_LINK
                , ITEM_GROUP_KEY
                , ref nGroupKey
                , ref arGroup)
                && m_instanceStorage.GetParameter(Define.DefineEnumBase.Storage.EN_STORAGE_LIST.DYNAMIC_LINK
                , ITEM_LINK_KEY
                , ref sLinkKey
                , ref arGroup);

        }
        private bool LoadStateCondition(string sTask, string sActionGroup, string sNodeLinkGroup, string sStateTransition, ref string sStateCondition)
        {
            string[] arGroup = new string[]{sTask, sActionGroup, GROUP_NODE_LINKS, sNodeLinkGroup, GROUP_STATE_CONDITION};

            return m_instanceStorage.GetParameter(Define.DefineEnumBase.Storage.EN_STORAGE_LIST.DYNAMIC_LINK
                , sStateTransition
                , ref sStateCondition
                , ref arGroup);
        }
        private bool LoadStatusCondition(string sTask, string sActionGroup, string sPortLinkGroup, ref string sStatusCondition)
        {
            string[] arGroup = new string[] { sTask, sActionGroup, GROUP_PORT_LINKS, sPortLinkGroup };

            return m_instanceStorage.GetParameter(Define.DefineEnumBase.Storage.EN_STORAGE_LIST.DYNAMIC_LINK
                , GROUP_STATUS_CONDITION
                , ref sStatusCondition
                , ref arGroup);
        }
        #endregion

        #region SAVE FILE
        private bool SaveNodeLinkParameters(string sTask, string sAction, int nIndexOfLink, string sTargetTask, string sTargetAction, int nGroupKey, string sLinkKey)
        {
            string strIndexOfLink = nIndexOfLink.ToString();
            string[] arGroup = new string[] { sTask, m_dicTaskNameToActionGroup[sTask][sAction], GROUP_NODE_LINKS, strIndexOfLink };
            bool bLinkEnable = false;

            Define.DefineEnumBase.Storage.EN_STORAGE_LIST enList = Define.DefineEnumBase.Storage.EN_STORAGE_LIST.DYNAMIC_LINK;

            if (m_instanceStorage.SetParameter(enList, ITEM_TARGET_TASK, sTargetTask, ref arGroup, false)
                && m_instanceStorage.SetParameter(enList, ITEM_TARGET_NODE, sTargetAction, ref arGroup, false)
                && m_instanceStorage.SetParameter(enList, ITEM_LINK_KEY, sLinkKey, ref arGroup, false)
                && m_instanceStorage.SetParameter(enList, ITEM_GROUP_KEY, nGroupKey, ref arGroup, false))
            {
                string[] arGroupStateCondition = new string[] { sTask, m_dicTaskNameToActionGroup[sTask][sAction], GROUP_NODE_LINKS, strIndexOfLink, GROUP_STATE_CONDITION };

                foreach (var kvp in m_dicStateTransitionStringToEnum)
                {
                    EN_ACTION_STATE enNodeState = m_dicStateTransitionToActionState[kvp.Value];
                    EN_ACTION_STATE enStateCondition = EN_ACTION_STATE.SKIP;

                    if (!m_DynamicLink.GetStateCondition(sTask, sAction, nGroupKey, sLinkKey, enNodeState, ref enStateCondition)
                        || !m_instanceStorage.SetParameter(enList, kvp.Key, m_dicActionStateEnumToString[enStateCondition], ref arGroupStateCondition, false))
                    {
                        return false;
                    }
                }

                return m_DynamicLink.GetNodeLinkEnable(sTask, sAction, nGroupKey, sLinkKey, ref bLinkEnable)
                    && m_instanceStorage.SetParameter(enList, ITEM_LINK_ENABLE, bLinkEnable, ref arGroup, true);
            }

            return false;
        }
        #endregion

        #region CONVERSION
        private void MakeConversionTables()
        {
            foreach (EN_ACTION_STATE enState in Enum.GetValues(typeof(EN_ACTION_STATE)))
            {
                string strState = enState.ToString();

                m_dicActionStateStringToEnum.Add(strState, enState);
                m_dicActionStateEnumToString.Add(enState, strState);
            }

            foreach (EN_STATE_TRANSITION enState in Enum.GetValues(typeof(EN_STATE_TRANSITION)))
            {
                m_dicStateTransitionStringToEnum.Add(enState.ToString(), enState);
            }

            m_dicStateTransitionToActionState[EN_STATE_TRANSITION.DONE_TO_READY] = EN_ACTION_STATE.DONE;
            m_dicStateTransitionToActionState[EN_STATE_TRANSITION.READY_TO_WAIT] = EN_ACTION_STATE.READY;
            m_dicStateTransitionToActionState[EN_STATE_TRANSITION.WAIT_TO_BUSY] = EN_ACTION_STATE.WAIT;

            foreach (EN_PORT_STATUS enStatus in Enum.GetValues(typeof(EN_PORT_STATUS)))
            {
                string sStatus = enStatus.ToString();

                m_dicPortStatusStringToEnum.Add(sStatus, enStatus);
                m_dicPortStatusEnumToString.Add(enStatus, sStatus);
            }
        }
        #endregion

        #region NODE LINK ITEM
        private bool GetNodeLinkGroup(string sTask, string sAction, int nGroupKey, string sLinkKey, ref string sLinkGroup)
        {
            foreach (LinkItem item in m_dicNodeLinkItems[sTask][sAction])
            {
                if (item.GetNodeLinkGroup(nGroupKey, sLinkKey, ref sLinkGroup))
                    return true;
            }

            return false;
        }
        #endregion

		#region PORT LINK ITEM
		private bool GetPortLinkGroup(string sTask, string sAction, int nGroupKey, string sLinkKey, ref string sLinkGroup)
		{
			foreach(LinkItem item in m_dicPortLinkItems[sTask][sAction])
			{
				if(item.GetNodeLinkGroup(nGroupKey, sLinkKey, ref sLinkGroup))
					return true;
			}

			return	false;
		}
		#endregion

		#endregion

		#region INTERFACE

		#region INITIALIZE
		public bool Init()
        {
            m_instanceStorage = Functional.Storage.GetInstance();
            
			// DYNAMCI_LINK -> ACTION
            if (!m_instanceStorage.Load(Define.DefineEnumBase.Storage.EN_STORAGE_LIST.DYNAMIC_LINK))
                return false;

            m_DynamicLink = DynamicLink.GetInstance();

            MakeConversionTables();

            LoadListOfTask();

            bool bAddNodeLink = AddNodeLink();
            bool bAddActionTimerAll = AddActionTimerAll();
            bool bAddNodeLinkItem = AddNodeLinkItem();
            bool bAddPortLinkItem = AddPortLinkItem();

            return bAddNodeLink && bAddActionTimerAll && bAddNodeLinkItem && bAddPortLinkItem;
        }
        #endregion

        // from here : now
        #region Set Link
        /// <summary>
        /// 2020.06.27 by yjlee [ADD] Add a link of the action.
        /// </summary>
        public bool AddLink(string sTask, string sAction, string sTargetTask, string sTargetAction, int nGroupKey, string sLinkKey)
        {
            if(m_instanceActionManager.AddLink(sTask, sAction, sTargetTask, sTargetAction, nGroupKey, sLinkKey))
            {
                string[] arData = null;
                string[] arGroup = new string[]{sTask, m_dicTaskNameToActionGroup[sTask][sAction]};

                // 새로 추가할 NODE LINK ITEM 의 INDEX
                int nIndexOfLink = m_dicNodeLinkItems[sTask][sAction].Count;

                if(FileBorn.GetInstance().GetItemFrame(BORN_LIST.ACTION, ref arGroup, nIndexOfLink.ToString(), ref arData))
                {
                    if(m_instanceStorage.AddGroupAndItem(Define.DefineEnumBase.Storage.EN_STORAGE_LIST.DYNAMIC_LINK, ref arData)
                        && SaveNodeLinkParameters(sTask, sAction, nIndexOfLink, sTargetTask, sTargetAction, nGroupKey, sLinkKey))
                    {
                        m_dicNodeLinkItems[sTask][sAction].Add(new LinkItem(nIndexOfLink
                            , nIndexOfLink.ToString()
                            , sTargetTask
                            , sTargetAction
                            , nGroupKey
                            , sLinkKey));

                        return true;
                    }
                }
            }

            return false;
        }
        public bool RemoveLink(string sTask, string sAction, string sTargetTask, string sTargetAction, int nGroupKey, string sLinkKey)
        {
            if (m_instanceActionManager.RemoveLink(sTask, sAction, sTargetTask, sTargetAction, nGroupKey, sLinkKey))
            {
                string sLinkGroup = string.Empty;

                if (GetNodeLinkGroup(sTask, sAction, nGroupKey, sLinkKey, ref sLinkGroup))
                {
                    string[] arGroup = new string[]{sTask, m_dicTaskNameToActionGroup[sTask][sAction], GROUP_NODE_LINKS, sLinkGroup};

                    if (m_instanceStorage.RemoveGroup(Define.DefineEnumBase.Storage.EN_STORAGE_LIST.DYNAMIC_LINK, ref arGroup))
                    {
                        var listOfLinkedAction = m_dicNodeLinkItems[sTask][sAction];

                        for(int nIndex = 0, nEnd = listOfLinkedAction.Count ; nIndex < nEnd ; ++ nIndex)
                        {
                            if(listOfLinkedAction[nIndex].LinkGroup.Equals(sLinkGroup))
                            {
                                listOfLinkedAction.RemoveAt(nIndex);

                                return true;
                            }
                        }
                    }
                }
            }

            return false;
        }
        #endregion

        #region ACTION ELAPSED TIME
		/// <summary>
		/// 2020.09.16 by twkang [ADD] 테스크에 해당하는 타이머를 작동시킨다.
		/// </summary>
		public void StartStopWatch(string sTask)
		{
			if(m_DicForStopwatch.ContainsKey(sTask))
			{
				m_DicForStopwatch[sTask].Restart();
			}
		}
		/// <summary>
		/// 2020.09.16 by twkang [ADD] 테스크에 해당하는 타이머를 멈추고, 경과시간을 저장한다.
		/// </summary>
		public void StopStopWatch(string sTask, string sAction)
		{
			m_DicForStopwatch[sTask].Stop();
			var nActionTime		= m_DicForStopwatch[sTask].ElapsedMilliseconds;
			
			m_dicTimeOfAction[sTask][sAction]	= nActionTime;
		}
		/// <summary>
		/// 2020.09.16 by twkang [ADD] Action 경과 시간을 반환한다.
		/// </summary>
		public double GetFinishedTime(string sTask, string sAction)
		{
			if(false == m_dicTimeOfAction.ContainsKey(sTask) || false == m_dicTimeOfAction[sTask].ContainsKey(sAction))
			{
				return 0;
			}

			return m_dicTimeOfAction[sTask][sAction];
		}
        #endregion

        #endregion

        #region GETTER/SETTER
        public bool GetListOfTask(ref int[] arIndex, ref string[] arTask)
        {
            if (null != m_arTask && 0 < m_arTask.Length)
            {
                arTask = m_arTask;
                arIndex = m_arTaskIndex;

                return true;
            }

            return false;
        }
        public bool GetListOfAction(string sTask, ref string[] arAction)
        {
            if (m_dicActionNames.ContainsKey(sTask))
            {
                arAction = m_dicActionNames[sTask];

                return true;
            }

            return false;
        }
        public bool GetActionEnable(string sTask, string sAction)
        {
            bool bEnable = false;

            if (m_dicTaskNameToActionGroup.ContainsKey(sTask))
            {
                m_DynamicLink.GetActionEnable(sTask, sAction, ref bEnable);
            }

            return bEnable;
        }
        public bool SetActionEnable(string sTask, string sAction, bool bEnable)
        {
            string[] arGroup = new string[] { sTask, m_dicTaskNameToActionGroup[sTask][sAction] };

            if (m_instanceStorage.SetParameter(Define.DefineEnumBase.Storage.EN_STORAGE_LIST.DYNAMIC_LINK
                , ITEM_ACTION_ENABLE
                , bEnable
                , ref arGroup))
            {
                return m_DynamicLink.SetActionEnable(sTask, sAction, bEnable);
            }

            return false;
        }
        private void SetNodeLinkPair()
        {
            // 모든 TASK 순회
            foreach (KeyValuePair<string, Dictionary<string, List<LinkItem>>> task in m_dicNodeLinkItems)
            {
                string sSourceTask = task.Key;

                // TASK 별 모든 NODE 순회
                foreach (KeyValuePair<string, List<LinkItem>> node in task.Value)
                {
                    string sSourceNode = node.Key;

                    // NODE 별 LINK 순회
                    foreach (LinkItem link in node.Value)
                    {
                        string sTargetTask = link.TargetTask;
                        string sTargetNode = link.TargetItem;

                        // TARGET LINK 의 NODE 별 LINK 순회
                        foreach (LinkItem pairlink in m_dicNodeLinkItems[sTargetTask][sTargetNode])
                        {
                            string sPairTask = pairlink.TargetTask;
                            string sPairNode = pairlink.TargetItem;

                            if (sSourceTask == sPairTask && sSourceNode == sPairNode)
                            {
                                int nLinkGroupKey = link.GroupKey;
                                string sLinkLinkKey = link.LinkKey;

                                int nPairGroupKey = pairlink.GroupKey;
                                string sPairLinkKey = pairlink.LinkKey;

                                object tPairNodeLinkItem = null;

                                // PAIR LINK 설정
                                m_DynamicLink.GetNodeLinkItem(sTargetTask, sTargetNode, nPairGroupKey, sPairLinkKey, ref tPairNodeLinkItem);
                                m_DynamicLink.SetNodeLinkPair(sSourceTask, sSourceNode, nLinkGroupKey, sLinkLinkKey, tPairNodeLinkItem);
                            }
                        }
                    }
                }
            }
        }
        public bool GetNodeLinkEnable(string sTask, string sAction, int nGroupKey, string sLinkKey)
        {
            bool bEnable = false;

            if (m_DynamicLink.GetNodeLinkEnable(sTask, sAction, nGroupKey, sLinkKey, ref bEnable))
                return bEnable;

            return false;
        }
        public bool SetNodeLinkEnable(string sTask, string sAction, int nGroupKey, string sLinkKey, bool bEnable)
        {
            string sLinkGroup = string.Empty;

            if (GetNodeLinkGroup(sTask, sAction, nGroupKey, sLinkKey, ref sLinkGroup))
            {
                string[] arGroup = new string[] { sTask, m_dicTaskNameToActionGroup[sTask][sAction], GROUP_NODE_LINKS, sLinkGroup };

                if (m_instanceStorage.SetParameter(Define.DefineEnumBase.Storage.EN_STORAGE_LIST.DYNAMIC_LINK
                    , ITEM_LINK_ENABLE
                    , bEnable
                    , ref arGroup))
                {
                    return m_DynamicLink.SetNodeLinkEnable(sTask, sAction, nGroupKey, sLinkKey, bEnable);
                }
            }

            return false;
        }
        public bool GetPortLinkEnable(string sTask, string sAction, int nGroupKey, string sLinkKey)
        {
            bool bEnable = false;

            if (m_DynamicLink.GetPortLinkEnable(sTask, sAction, nGroupKey, sLinkKey, ref bEnable))
                return bEnable;

            return false;
        }
        public bool SetPortLinkEnable(string sTask, string sAction, int nGroupKey, string sLinkKey, bool bEnable)
        {
            string sLinkGroup = string.Empty;

            if (GetPortLinkGroup(sTask, sAction, nGroupKey, sLinkKey, ref sLinkGroup))
            {
                string[] arGroup = new string[] { sTask, m_dicTaskNameToActionGroup[sTask][sAction], GROUP_PORT_LINKS, sLinkGroup };

                if (m_instanceStorage.SetParameter(Define.DefineEnumBase.Storage.EN_STORAGE_LIST.DYNAMIC_LINK
                    , ITEM_LINK_ENABLE
                    , bEnable
                    , ref arGroup))
                {
                    return m_DynamicLink.SetPortLinkEnable(sTask, sAction, nGroupKey, sLinkKey, bEnable);
                }
            }

            return false;
        }
        public void GetStatusCondition(string sTask, string sPort, int nGroupKey, string sLinkKey, ref string sStatusCondition)
        {
            EN_PORT_STATUS enStatusCondition = EN_PORT_STATUS.DISABLE;

            if (m_DynamicLink.GetStatusCondition(sTask, sPort, nGroupKey, sLinkKey, ref enStatusCondition))
            {
                sStatusCondition = m_dicPortStatusEnumToString[enStatusCondition];
            }
        }
        public void GetStateCondition(string sTask, string sAction, int nGroupKey, string sLinkKey, string sStateTransition, ref string sStateCondition)
        {
            EN_STATE_TRANSITION enStateTransition = m_dicStateTransitionStringToEnum[sStateTransition];
            EN_ACTION_STATE enNodeState = m_dicStateTransitionToActionState[enStateTransition];

            EN_ACTION_STATE enStateCondition = EN_ACTION_STATE.SKIP;

            if (m_DynamicLink.GetStateCondition(sTask, sAction, nGroupKey, sLinkKey, enNodeState, ref enStateCondition))
            {
                sStateCondition = m_dicActionStateEnumToString[enStateCondition];
            }
        }
        public bool SetStateCondition(string sTask, string sAction, int nGroupKey, string sLinkKey, string sStateTransition, string sStateCondition)
        {
            string sLinkGroup = string.Empty;

            if (GetNodeLinkGroup(sTask, sAction, nGroupKey, sLinkKey, ref sLinkGroup))
            {
                string[] arGroup = new string[] { sTask, m_dicTaskNameToActionGroup[sTask][sAction], GROUP_NODE_LINKS, sLinkGroup, GROUP_STATE_CONDITION };

                if (m_instanceStorage.SetParameter(Define.DefineEnumBase.Storage.EN_STORAGE_LIST.DYNAMIC_LINK
                    , sStateTransition
                    , sStateCondition
                    , ref arGroup))
                {
                    EN_STATE_TRANSITION enStateTransition = m_dicStateTransitionStringToEnum[sStateTransition];
                    EN_ACTION_STATE enNodeState = m_dicStateTransitionToActionState[enStateTransition];

                    return m_DynamicLink.SetStateCondition(sTask, sAction, nGroupKey, sLinkKey, enNodeState, m_dicActionStateStringToEnum[sStateCondition]);
                }
            }

            return false;
        }
        public bool GetNodeLinkList(string sTask, string sAction, ref string[] arTargetTask, ref string[] arTargetNode, ref int[] arGroupKey, ref string[] arLinkKey, ref int nItemCount)
        {
            if (!m_dicNodeLinkItems.ContainsKey(sTask))
                return false;

            if (!m_dicNodeLinkItems[sTask].ContainsKey(sAction))
                return false;

            nItemCount = m_dicNodeLinkItems[sTask][sAction].Count;

			if(nItemCount == EMPTY_ITEM_SIZE)
				return false;

            arTargetTask = new string[nItemCount];
            arTargetNode = new string[nItemCount];
            arGroupKey = new int[nItemCount];
            arLinkKey = new string[nItemCount];

            foreach (LinkItem item in m_dicNodeLinkItems[sTask][sAction])
            {
                arTargetTask[item.IndexOfItem] = item.TargetTask;
                arTargetNode[item.IndexOfItem] = item.TargetItem;
                arGroupKey[item.IndexOfItem] = item.GroupKey;
                arLinkKey[item.IndexOfItem] = item.LinkKey;
            }

            return true;
        }
        public bool GetPortLinkList(string sTask, string sAction, ref string[] arTargetTask, ref string[] arTargetPort, ref int[] arGroupKey, ref string[] arLinkKey, ref int nItemCount)
        {
            if (!m_dicPortLinkItems.ContainsKey(sTask))
                return false;

            if (!m_dicPortLinkItems[sTask].ContainsKey(sAction))
                return false;

            nItemCount = m_dicPortLinkItems[sTask][sAction].Count;

			if(nItemCount == EMPTY_ITEM_SIZE)
				return false;

            arTargetTask = new string[nItemCount];
            arTargetPort = new string[nItemCount];
            arGroupKey = new int[nItemCount];
            arLinkKey = new string[nItemCount];

            foreach (LinkItem item in m_dicPortLinkItems[sTask][sAction])
            {
                arTargetTask[item.IndexOfItem] = item.TargetTask;
                arTargetPort[item.IndexOfItem] = item.TargetItem;
                arGroupKey[item.IndexOfItem] = item.GroupKey;
                arLinkKey[item.IndexOfItem] = item.LinkKey;
            }

            return true;
        }
        public bool GetAccessNodeList(string sTask, string sAction, ref string[] arTask, ref string[] arAction, ref int nItemCount)
        {
			// 2022.08.30 by junho [ADD] UI에서 접근할 때, 강제종료 현상 발생
			try
			{
				return m_DynamicLink.GetAccessNodeList(sTask, sAction, ref arTask, ref arAction, ref nItemCount);
			}
			catch
			{
				return false;
			}
        }
		public bool SetPortLinkStatus(string sTask, string sAction, int nGroupKey, string sLinkKey, string sStatus)
		{
			string sLinkGroup	= string.Empty;
			
			if(GetPortLinkGroup(sTask, sAction, nGroupKey, sLinkKey, ref sLinkGroup))
			{
				string[] arGroup = new string[] { sTask, m_dicTaskNameToActionGroup[sTask][sAction], GROUP_PORT_LINKS, sLinkGroup };

				if(m_instanceStorage.SetParameter(Define.DefineEnumBase.Storage.EN_STORAGE_LIST.DYNAMIC_LINK
					, GROUP_STATUS_CONDITION
					, sStatus
					, ref arGroup))
				{
					EN_PORT_STATUS enPortStatus	= m_dicPortStatusStringToEnum[sStatus];

					return m_DynamicLink.SetStatusCondition(sTask, sAction,nGroupKey, sLinkKey, enPortStatus);
				}
			}

			return false;
		}
		public bool GetPortList(string sTask, ref string[] arPortName)
		{
			if(false == m_dicPortNames.ContainsKey(sTask))
				return false;

			if(null == m_dicPortNames[sTask] || m_dicPortNames[sTask].Length < 1)
				return false;

			arPortName	=  m_dicPortNames[sTask];

			return true;
		}
		public bool GetPortStatus(string sTask, string sPortName, ref string sPortState)
		{
			EN_PORT_STATUS enStatus	= EN_PORT_STATUS.DISABLE;

			if(m_DynamicLink.GetPortStatus(sTask, sPortName, ref enStatus))
			{
				sPortState = enStatus.ToString();

				return true;
			}

			return false;
		}
		public bool GetActionStatus(string sTask, string sAction, ref string sActionStatus)
		{
			EN_ACTION_STATE enActionState	= EN_ACTION_STATE.BUSY;

			if(m_DynamicLink.GetNodeState(sTask, sAction, ref enActionState))
			{
				sActionStatus	= enActionState.ToString();

				return true;
			}

			return false;
		}
		public int GetCurrentFlowStep(string sTask)
		{
			return m_DynamicLink.GetCurrentFlowStep(sTask);
		}
		public bool IsAccessedNodeCheck(string sTask, string sAction, string sTargetTask, string sTargetAction)
		{
			return m_DynamicLink.IsAccessedNodeCheck(sTask, sAction, sTargetTask, sTargetAction);
		}
        #endregion
    }
}
