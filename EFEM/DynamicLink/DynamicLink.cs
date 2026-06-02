using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FrameOfSystem3.DynamicLink_
{
    // OBJECT ROLE : 단위 ACTION 들을 연속하여 실행시킬 수 있는 LOGIC 을 제공한다
    public class DynamicLink
    {
        #region SINGLETON
        private DynamicLink() { }
        private static DynamicLink m_instance = new DynamicLink();
        public static DynamicLink GetInstance() { return m_instance; }
        #endregion

        #region FIELD
        // PORT 객체모음
        // KEY: TaskName.PortName
        Dictionary<string, ActionPort> m_dicPorts = new Dictionary<string, ActionPort>();

        // PORT 와 연결된 NODE 의 실행여부를 위한 연결모음
        // KEY: TaskName.ActionName
        private Dictionary<string, PortLink> m_dicPortLinks = new Dictionary<string, PortLink>();
        
        // FLOW 로 지정된 ACTION 의 실행순서를 위한 조건모음
        // KEY: TaskName
        private Dictionary<string, FlowLink> m_dicFlowConditions = new Dictionary<string, FlowLink>();

        // LINK 로 연결된 ACTION 의 상태전이를 위한 연결모음
        // KEY: TaskName.ActionName
        private Dictionary<string, NodeLink> m_dicNodeLinks = new Dictionary<string, NodeLink>();
        #endregion

        #region METHOD
        /// <summary>
        /// TASK 에 할당된 ACTION NAME 목록을 반환한다
        /// </summary>
        private bool GetActionList(string sTaskName, out string[] arNode)
        {
            string[] arNodeLinks = m_dicNodeLinks.Keys.ToArray();

            List<string> lstNode = new List<string>();

            foreach (string node in arNodeLinks)
            {
                // TEXT : TaskName.ActionName
                char cSplit = '.';
                string[] sSplit = node.Split(cSplit);

                // sSplit[0] : TASK NAME
                // sSplit[1] : ACTION NAME
                if (sSplit[0].Equals(sTaskName))
                {
                    lstNode.Add(sSplit[1]);
                }
            }

            if (lstNode.Count.Equals(0))
            {
                arNode = null;
                return false;
            }

            arNode = new string[lstNode.Count];

            arNode = lstNode.ToArray();

            return true;
        }
        /// <summary>
        /// 상태확인 : 연결노드의 상태확인만 한다
        /// </summary>
        private void CheckState(string sTaskName, string sActionName)
        {
            string sNodeLink = sTaskName + "." + sActionName;

            m_dicNodeLinks[sNodeLink].CheckState();
        }
        /// <summary>
        /// ACTION STATE 상태전이
        /// </summary>
        private bool TransitState(string sTaskName, string sActionName)
        {
            PortLink tPortLink = null;
            FlowLink tFlowLink = null;
            NodeLink tNodeLink = null;

            string sNodeName = sTaskName + "." + sActionName;

            // [CONDITION 확인순서]
            // 1. PORT LINK : NODE 와 LINK 된 PORT 의 STATUS 를 확인한다
            // 2. FLOW CONDITION : PRECONDITION > (ACTION) > POSTCONDITION
            // 3. NODE LINK : CHECK STATE > FINALCONDITION > (UPDATE STATUS), UPDATE STATE

            // PORT 를 사용하지 않는 경우가 있으므로 PortLink 의 Instance 유무를 확인한다
            if (m_dicPortLinks.TryGetValue(sNodeName, out tPortLink))
            {
                if (tPortLink.CheckStatus().Equals(false))
				{
                    // PORT STATUS 가 FALSE 인 경우 NODE LINK 를 확인하지 않기 때문에 ACCESS NODE 가
                    // 항상 FALSE 인 경우가 있으므로 NODE LINK 확인을 실시한다
                    m_dicNodeLinks[sNodeName].CheckState();
                    return false;
                }
            }

            // FLOW 를 사용하지 않는 경우가 있으므로 FlowLink 의 Instance 유무를 확인한다
            if (m_dicFlowConditions.TryGetValue(sTaskName, out tFlowLink))
            {
                if (tFlowLink.CheckCondition(sActionName).Equals(false))
                {
                    // FLOW CONDITION 이 FALSE 인 경우 NODE LINK 를 확인하지 않기 때문에 ACCESS NODE 가
                    // 항상 FALSE 인 경우가 있으므로 NODE LINK 확인을 실시한다
                    m_dicNodeLinks[sNodeName].CheckState();
                    return false;
                }
            }
            // NODE 를 사용하지 않는 경우는 상태전이할 대상이 없으므로 FALSE 를 반환한다
            if (m_dicNodeLinks.TryGetValue(sNodeName, out tNodeLink))
            {
				if (tNodeLink.UpdateState(tPortLink, tFlowLink))
					return true;
				// 2023.11.27 by shkim [MOD] PWA-500W 링크에서 Done 유지 상태에서 독점 링크해제해야했다. 같은 조건일 경우 아래 코드 사용	
				//if (tNodeLink.UpdateStateWithResetExclusiveLinkWhenDoneState(tPortLink, tFlowLink))
                //    return true;   				
					
					
					
            }

            return false;
        }
        /// <summary>
        /// 활성여부 : 모든 DYNAMIC LINK 조건이 유효하여 실행가능 ACTION 인지 여부 (NODE STATE 가 BUSY 인지 여부)
        /// </summary>
        private bool IsActivated(string sTaskName, string sActionName)
        {
            string sNodeLink = sTaskName + "." + sActionName;

            return (m_dicNodeLinks[sNodeLink].State == EN_ACTION_STATE.BUSY);
        }
        #endregion

        #region INTERFACE

        #region CREATE

        #region PORT LINK
        public bool AddPort(string sTaskName, string sPortName)
        {
            string sPort = sTaskName + "." + sPortName;

            if (m_dicPorts.ContainsKey(sPort).Equals(false))
            {
                m_dicPorts[sPort] = new ActionPort(sTaskName, sPortName);

                return true;
            }

            return false;
        }
        public bool AddPortLink(string sTaskName, string sActionName, ActionNode tNode)
        {
            string sPortLink = sTaskName + "." + sActionName;

            if (m_dicPortLinks.ContainsKey(sPortLink).Equals(false))
            {
                m_dicPortLinks[sPortLink] = new PortLink(tNode);

                return true;
            }

            return false;
        }
        public bool AddPortLinkItem(string sTaskName, string sActionName, string sTargetTask, string sTargetPort, int nGroupKey, string sLinkKey)
        {
            string sNodeLink = sTaskName + "." + sActionName;

            if (m_dicPortLinks.ContainsKey(sNodeLink).Equals(false))
            {
                m_dicPortLinks[sNodeLink] = new PortLink(m_dicNodeLinks[sNodeLink].Node);
            }

            string sTargetPortLink = sTargetTask + "." + sTargetPort;

            if (m_dicPorts.ContainsKey(sTargetPortLink))
            {
                if (m_dicPortLinks[sNodeLink].AddLink(nGroupKey, sLinkKey, m_dicPorts[sTargetPortLink]))
                    return true;
            }

            return false;
        }
        #endregion

        #region NODE LINK
        public bool AddNodeLink(string sTaskName, string sActionName)
        {
            string sNodeLink = sTaskName + "." + sActionName;

            if (m_dicNodeLinks.ContainsKey(sNodeLink).Equals(false))
            {
                ActionNode node = new ActionNode(sTaskName, sActionName);

                m_dicNodeLinks[sNodeLink] = new NodeLink(node);

                return true;
            }

            return false;
        }
        public bool AddNodeLinkItem(string sTaskName, string sActionName, string sTargetTask, string sTargetNode, int nGroupKey, string sLinkKey)
        {
            string sNodeLink = sTaskName + "." + sActionName;
            string sTargetNodeLink = sTargetTask + "." + sTargetNode;

            if (m_dicNodeLinks.ContainsKey(sNodeLink) && m_dicNodeLinks.ContainsKey(sTargetNodeLink))
            {
                if (m_dicNodeLinks[sNodeLink].AddLink(nGroupKey, sLinkKey, m_dicNodeLinks[sTargetNodeLink].Node))
                    return true;
            }

            return false;
        }
        #endregion

        #region FLOW CONDITION
        public bool AddFlowLink(string sTaskName)   
	    {
            if (m_dicFlowConditions.ContainsKey(sTaskName).Equals(false))
            {
                m_dicFlowConditions[sTaskName] = new FlowLink();

                return true;
            }

            return false;
	    }
        public bool AddFlowTable(string sTaskName, string sTableName)
        {
            if (m_dicFlowConditions.ContainsKey(sTaskName))
            {
                return m_dicFlowConditions[sTaskName].AddFlowTable(sTableName);
            }

            return false;
        }

        // 2022.03.02 [ADD] For modifying flow item
        public bool RemoveFlowTable(string sTaskName, string sTableName)
        {
            if (m_dicFlowConditions.ContainsKey(sTaskName))
            {
                return m_dicFlowConditions[sTaskName].RemoveFlowTable(sTableName);
            }
            return true;
        }

        public bool AddFlowItem(string sTaskName, string sTableName, int nFlowNo, ActionNode tNode)
	    {
            if (m_dicFlowConditions.ContainsKey(sTaskName))
		    {
                return m_dicFlowConditions[sTaskName].AddFlowItem(sTableName, nFlowNo, tNode);
		    }

		    return false;
	    }
        // 2022.03.02 [ADD] For modifying flow item
        public bool InsertFlowItem(string sTaskName, string sTableName, int nFlowNo, ActionNode tNode)
        {
            if (m_dicFlowConditions.ContainsKey(sTaskName))
            {
                return m_dicFlowConditions[sTaskName].InsertFlowItem(sTableName, nFlowNo, tNode);
            }
            return false;
        }
        // 2022.03.02 [ADD] For modifying flow item
        public bool RemoveFlowItem(string sTaskName, string sTableName, int nFlowNo)
        {
            if (m_dicFlowConditions.ContainsKey(sTaskName))
            {
                return m_dicFlowConditions[sTaskName].RemoveFlowItem(sTableName, nFlowNo);
            }
            return false;
        }
        #endregion

        #endregion

        #region CONTROL
        /// <summary>
        /// PORT 의 모든 LINK 사용을 초기화 (독점 LINK 해제)
        /// </summary>
        public bool InitializePortLink(string sTaskName)
        {
            string[] arAction;

            if (GetActionList(sTaskName, out arAction))
            {
                foreach (string sActionName in arAction)
                {
                    string sPortLink = sTaskName + "." + sActionName;

                    if (m_dicPortLinks.ContainsKey(sPortLink).Equals(false))
                        continue;

                    m_dicPortLinks[sPortLink].InitializeLink();
                }
            }

            return true;
        }
        /// <summary>
        /// NODE 의 모든 LINK 사용을 초기화 (독점 LINK 해제)
        /// </summary>
        public bool InitializeNodeLink(string sTaskName)
        {
            string[] arAction;

            if (GetActionList(sTaskName, out arAction))
            {
                foreach (string sActionName in arAction)
                {
                    string sLinkNode = sTaskName + "." + sActionName;

                    if (m_dicNodeLinks.ContainsKey(sLinkNode).Equals(false))
                        return false;

                    m_dicNodeLinks[sLinkNode].InitializeLink();
                }
            }

            return true;
        }
        /// <summary>
        /// TASK 의 DoEntrySequence() 에서 할당된 ACTION STATE 를 모두 초기화(READY) 한다
        /// </summary>
        public bool InitializeNodeState(string sTaskName)
        {
            string[] arAction;

            if (GetActionList(sTaskName, out arAction))
            {
                foreach(string sActionName in arAction)
                {
					if (SetNodeState(sTaskName, sActionName, EN_ACTION_STATE.READY).Equals(false))
						return false;
                }
            }

            return true;
        }
        /// <summary>
        /// FLOW 를 시작하기 위한 이벤트를 FLOW PRECONDITION 에서 발생
        /// 24.08.30 by wdw [Fix] 작업 복구를 위한 진입 시점 설정 변수 추가.
        /// </summary>
        public void InitiateWorkFlow(string sTaskName, int nEntryFlowNo = 1)
        {
            m_dicFlowConditions[sTaskName].BeginActionFlow(nEntryFlowNo);
        }
        /// <summary>
        /// FLOW 를 종료하기 위한 이벤트를 FLOW POSTCONDITION 에서 발생
        /// </summary>
        public void TerminateWorkFlow(string sTaskName)
        {
            m_dicFlowConditions[sTaskName].EndActionFlow();
        }
        /// <summary>
        /// ACTION 종료 시 FLOW 의 POSTCONDITION 을 호출하여 다음 ACTION 을 결정한다
        /// </summary>
        public void FinishAction(string sTaskName)
        {
            m_dicFlowConditions[sTaskName].SetNextAction();
        }
        /// <summary>
        /// 상태확인 : TASK 의 모든 연결노드의 상태확인만 하며, TASK 객체의 DoExecutingSequence() 에서 주기호출
        /// </summary>
        public void CheckState(string sTaskName)
        {
            string[] arAction;

            if (GetActionList(sTaskName, out arAction))
            {
                foreach (string sActionName in arAction)
                {
                    CheckState(sTaskName, sActionName);
                }
            }
        }
        /// <summary>
        /// ACTION STATE 상태전이 : TASK 객체의 SelectExecutingSequence() 에서 주기호출을 해야한다
        /// </summary>
        public bool TransitState(string sTaskName, out string sActivatedActionName)
        {
            sActivatedActionName = string.Empty;

            string[] arAction;

            if (GetActionList(sTaskName, out arAction))
            {
                foreach (string sActionName in arAction)
                {
                    if (TransitState(sTaskName, sActionName))
                    {
                        if (IsActivated(sTaskName, sActionName))
                        {
                            sActivatedActionName = sActionName;

                            return true;
                        }
                    }
                }
            }

            return false;
        }
        #endregion

        #region <CONFIRM>
        // 2021.08.07 by junho [ADD] Flow가 진행중인지 여부 반환
        public bool IsContinuousFlow(string sTaskName)
        {
            return m_dicFlowConditions[sTaskName].IsContinuousFlow();
        }

        // 2021.08.09 by junho [ADD] 해당 Node의 Port selected list에 TargetTask가 있는지 여부 반환
        public bool IsExistThisTaskInSelectedList(string sTaskName, string sActionName, string sTargetTask)
        {
            string sLinkNode = sTaskName + "." + sActionName;
            if (false == m_dicNodeLinks.ContainsKey(sLinkNode))
                return false;

            return m_dicNodeLinks[sLinkNode].IsExistThisTaskInSelectedList(sTargetTask);
        }

        /// <summary>
        /// 2022.11.02. by shkim. [ADD] Post Condition에 따라 Flow를 분기할 때,
        //  해당하는 EN_ACTION_RESULT 에 Flow Number가 적절하게 설정되어있는지 확인한다.
        /// </summary>
        /// <param name="sTaskName"></param>
        /// <param name="enActionResult"></param>
        /// <returns></returns>
        public bool IsNextFlowNumberValid(string sTaskName, EN_ACTION_RESULT enActionResult)
        {
            string currentTableName = string.Empty;;
            if (false == GetCurrentFlowTable(sTaskName, ref currentTableName)
                || string.IsNullOrEmpty(currentTableName))
            {
                return false;
            }
            int currentFlowNo = GetCurrentFlowStep(sTaskName);
            int postFlowNo = GetPostconditionFlowNo(sTaskName
                                                , currentTableName
                                                , currentFlowNo
                                                , enActionResult);
            if (postFlowNo == -1)
            {
                return false;
            }
            return true;
        }
        #endregion </CONFIRM>

        #endregion

        #region GETTER/SETTER

        #region PORT LINK
        public bool GetPortEnable(string sTaskName, string sPortName, ref bool bEnable)
        {
            string sPort = sTaskName + "." + sPortName;

            if (m_dicPorts.ContainsKey(sPort))
            {
                bEnable = m_dicPorts[sPort].Enable;

                return true;
            }

            return false;
        }
        public bool SetPortEnable(string sTaskName, string sPortName, bool bEnable)
        {
            string sPort = sTaskName + "." + sPortName;

            if (m_dicPorts.ContainsKey(sPort))
            {
                m_dicPorts[sPort].Enable = bEnable;

                return true;
            }

            return false;
        }
        /// <summary>
        /// PORT 의 상태를 변경하고 CONFIG FILE 에 저장한다
        /// </summary>
        public bool SetPortStatus(string sTaskName, string sPortName, EN_PORT_STATUS enStatus, bool bSaved = false)
        {
            string sPort = sTaskName + "." + sPortName;

            if (m_dicPorts.ContainsKey(sPort))
            {
                if (m_dicPorts[sPort].Status.Equals(enStatus))
                    return true;

                // bSaved : CONFIG FILE 에 저장하는 LOGIC 이 이미 실행됐으면 TRUE 이다
                if (bSaved.Equals(false))
                {
                    string[] arGroup = new string[] { sTaskName, sPortName };

                    if (Functional.Storage.GetInstance().SetParameter(Define.DefineEnumBase.Storage.EN_STORAGE_LIST.PORT
                        , "STATUS"
                        , enStatus.ToString()
                        , ref arGroup).Equals(false))
                    {
                        return false;
                    }
                }

                m_dicPorts[sPort].Status = enStatus;

                //24.01.26. by wdw [add] Port 상태가 변하면 Material을 갱신한다.
                EquipmentProperty.RawMaterialPortManager.GetInstance().SetRawMaterialExist(sPort, enStatus);

                return true;
            }

            return false;
        }
		/// <summary>
		/// 2021.08.04 by twkang [ADD] Get the port status 
		/// </summary>
		public bool GetPortStatus(string sTaskName, string sPortName, ref EN_PORT_STATUS enStatus)
		{
			string sPort = sTaskName + "." + sPortName;

			if (m_dicPorts.ContainsKey(sPort))
			{
				enStatus = m_dicPorts[sPort].Status;

				return true;
			}

			return false;
		}
        /// <summary>
        /// 2024.01.26 by wdw [ADD] Get the port status use stringkey
        /// </summary>
        public bool GetPortStatus(string strKey, ref EN_PORT_STATUS enStatus)
        {
            if (m_dicPorts.ContainsKey(strKey))
            {
                enStatus = m_dicPorts[strKey].Status;

                return true;
            }

            return false;
        }
        #endregion

        #region NODE LINK
        public bool GetActionNode(string sTaskName, string sActionName, out ActionNode tNode)
        {
            string sNodeLink = sTaskName + "." + sActionName;

            tNode = null;

            // from here : FLOW NUMER 가 0 인 경우 (더 좋은 방법 연구 필요 !!!)
            if (sActionName.Equals("NONE"))
            {
                tNode = new ActionNode(sTaskName, sActionName);
                tNode.IsActionFlow = true;
                return true;
            }

            if (m_dicNodeLinks.ContainsKey(sNodeLink))
            {
                tNode = m_dicNodeLinks[sNodeLink].Node;
                return true;
            }

            return false;
        }
        public bool GetAccessNodeList(string sTaskName, string sActionName, ref string[] arTask, ref string[] arAction, ref int nItemCount)
        {
            string sNodeLink = sTaskName + "." + sActionName;

            if (m_dicNodeLinks.ContainsKey(sNodeLink))
                return m_dicNodeLinks[sNodeLink].GetAccessNodeList(ref arTask, ref arAction, ref nItemCount); ;

            return false;
        }
        public bool GetNodeLinkItem(string sTaskName, string sActionName, int nGroupKey, string sLinkKey, ref object tNodeLinkItem)
        {
            string sNodeLink = sTaskName + "." + sActionName;

            if (m_dicNodeLinks.ContainsKey(sNodeLink))
            {
                tNodeLinkItem = m_dicNodeLinks[sNodeLink].GetNodeLinkItem(nGroupKey, sLinkKey);

                return true;
            }

            return false;
        }
        public bool SetNodeLinkPair(string sTaskName, string sActionName, int nGroupKey, string sLinkKey, object tNodeLinkItem)
        {
            string sNodeLink = sTaskName + "." + sActionName;

            if (m_dicNodeLinks.ContainsKey(sNodeLink))
            {
                return m_dicNodeLinks[sNodeLink].SetPairLink(nGroupKey, sLinkKey, tNodeLinkItem);
            }

            return false;
        }
        public bool GetNodeLinkEnable(string sTaskName, string sActionName, int nGroupKey, string sLinkKey, ref bool bEnable)
        {
            string sNodeLink = sTaskName + "." + sActionName;

            if (m_dicNodeLinks.ContainsKey(sNodeLink))
            {
                bEnable = m_dicNodeLinks[sNodeLink].GetLinkEnable(nGroupKey, sLinkKey);

                return true;
            }

            return false;
        }
        public bool SetNodeLinkEnable(string sTaskName, string sActionName, int nGroupKey, string sLinkKey, bool bEnable)
        {
            string sNodeLink = sTaskName + "." + sActionName;

            if (m_dicNodeLinks.ContainsKey(sNodeLink))
            {
                return m_dicNodeLinks[sNodeLink].SetLinkEnable(nGroupKey, sLinkKey, bEnable);
            }

            return false;
        }
        public bool GetPortLinkEnable(string sTaskName, string sActionName, int nGroupKey, string sLinkKey, ref bool bEnable)
        {
            string sPortLink = sTaskName + "." + sActionName;

            if (m_dicPortLinks.ContainsKey(sPortLink))
            {
                bEnable = m_dicPortLinks[sPortLink].GetLinkEnable(nGroupKey, sLinkKey);

                return true;
            }

            return false;
        }
        public bool SetPortLinkEnable(string sTaskName, string sActionName, int nGroupKey, string sLinkKey, bool bEnable)
        {
            string sNodeLink = sTaskName + "." + sActionName;

            if (m_dicPortLinks.ContainsKey(sNodeLink))
            {
                return m_dicPortLinks[sNodeLink].SetLinkEnable(nGroupKey, sLinkKey, bEnable);
            }

            return false;
        }
        public bool GetActionEnable(string sTaskName, string sActionName, ref bool bEnable)
        {
            string sNodeLink = sTaskName + "." + sActionName;

            if (m_dicNodeLinks.ContainsKey(sNodeLink))
            {
                bEnable = m_dicNodeLinks[sNodeLink].Enable;

                return true;
            }

            return false;
        }
        public bool SetActionEnable(string sTaskName, string sActionName, bool bEnable)
        {
            string sNodeLink = sTaskName + "." + sActionName;

            if (m_dicNodeLinks.ContainsKey(sNodeLink))
            {
                m_dicNodeLinks[sNodeLink].Enable = bEnable;

                return true;
            }

            return false;
        }
        public bool GetStateCondition(string sTaskName, string sActionName, int nGroupKey, string sLinkKey, EN_ACTION_STATE enNodeState, ref EN_ACTION_STATE enStateCondition)
        {
            string sNodeLink = sTaskName + "." + sActionName;

            if (m_dicNodeLinks.ContainsKey(sNodeLink))
            {
                enStateCondition = m_dicNodeLinks[sNodeLink].GetStateCondition(nGroupKey, sLinkKey, enNodeState);

                return true;
            }

            return false;
        }
        public bool SetStateCondition(string sTaskName, string sActionName, int nGroupKey, string sLinkKey, EN_ACTION_STATE enNodeState, EN_ACTION_STATE enStateCondition)
        {
            string sNodeLink = sTaskName + "." + sActionName;

            if (m_dicNodeLinks.ContainsKey(sNodeLink))
            {
                if (m_dicNodeLinks[sNodeLink].SetStateCondition(nGroupKey, sLinkKey, enNodeState, enStateCondition))
                    return true;
            }

            return false;
        }
        public bool GetStatusCondition(string sTaskName, string sPortName, int nGroupKey, string sLinkKey, ref EN_PORT_STATUS enStatusCondition)
        {
            string sLinkPort = sTaskName + "." + sPortName;

            if (m_dicPortLinks.ContainsKey(sLinkPort))
            {
                enStatusCondition = m_dicPortLinks[sLinkPort].GetStatusCondition(nGroupKey, sLinkKey);

                return true;
            }

            return false;
        }
        public bool SetStatusCondition(string sTaskName, string sPortName, int nGroupKey, string sLinkKey, EN_PORT_STATUS enStatusCondition)
        {
            string sLinkPort = sTaskName + "." + sPortName;

            if (m_dicPortLinks.ContainsKey(sLinkPort))
            {
                if (m_dicPortLinks[sLinkPort].SetStatusCondition(nGroupKey, sLinkKey, enStatusCondition))
                    return true;
            }

            return false;
        }

		public bool IsAccessedNodeCheck(string sTaskName, string sActionName, string sTasrgetTask, string sTargetActionName)
		{
			ActionNode pNode;

			if (GetActionNode(sTaskName, sActionName, out pNode))
			{
				return pNode.IsAccessedCheck(sTasrgetTask, sTargetActionName);
			}

			return false;
		}
        #endregion

		#region FLOW
		/// <summary>
		/// 2021.07.30 by twkang [ADD] Get the List of flowtable
		/// </summary>
		public bool GetFlowTableList(string sTaskName, ref string[] arTalbeItem)
		{
			if (m_dicFlowConditions.ContainsKey(sTaskName))
			{
				return m_dicFlowConditions[sTaskName].GetFlowTableList(ref arTalbeItem);
			}

			return false;
		}
		/// <summary>
		/// 2021.07.30 by twkang [ADD] Get the count of flowitem
		/// </summary>
		public int GetCountOfFlowItem(string sTaskName, string sTableName)
		{
			if (m_dicFlowConditions.ContainsKey(sTaskName))
			{
				return m_dicFlowConditions[sTaskName].GetCountOfFlowItem(sTableName);
			}

			return 0;
		}
		/// <summary>
		/// 2021.08.05 by twkang [ADD] Get the current flow step
		/// </summary>
		public int GetCurrentFlowStep(string sTaskName)
		{
			if (m_dicFlowConditions.ContainsKey(sTaskName))
			{
				return m_dicFlowConditions[sTaskName].GetCurrentFlowStep();
			}

			return 0;
		}

        /// <summary>
        ///  2022.06.30. by shkim. [ADD] Flow Table에 등록된 Action을 Manual 동작할 때,
        ///  UI에 현재 진행중인 Action이 Table에 어떤 Flow 에 해당되는지 표시를 위해 설정한다.
        /// </summary>
        /// <param name="sTaskName"></param>
        /// <param name="sTableName"></param>
        /// <param name="nFlostStep"></param>
        /// <returns></returns>
        public bool SetCurrentFlowStep(string sTaskName, string sTableName, int nFlostStep)
        {
            if (m_dicFlowConditions.ContainsKey(sTaskName))
            {
                return m_dicFlowConditions[sTaskName].SetCurrentFlowStep(sTableName, nFlostStep);
            }

            return false;
        }

		#region FLOW CONDITION
		public string GetActionName(string sTaskName, string sTableName, int nFlowIndex)
		{
			if (m_dicFlowConditions.ContainsKey(sTaskName))
			{
				return m_dicFlowConditions[sTaskName].GetActionName(sTableName, nFlowIndex);
			}

			return string.Empty;
		}
		public bool SetActionName(string sTaskName, string sTableName, int nFlowNo, string sActionName)
		{
			if (m_dicFlowConditions.ContainsKey(sTaskName))
			{
				return m_dicFlowConditions[sTaskName].SetActionName(sTableName, nFlowNo, sActionName);
			}

			return false;
		}
		public int GetPreconditionFlowNo(string sTaskName, string sTableName, int nFlowIndex, EN_CONDITION_RESULT enResult)
		{
			if (m_dicFlowConditions.ContainsKey(sTaskName))
			{
				return m_dicFlowConditions[sTaskName].GetPreconditionFlowNo(sTableName, nFlowIndex, enResult);
			}

			return 0;
		}
		public bool SetPreconditionFlowNo(string sTaskName, string sTableName, int nFlowIndex, EN_CONDITION_RESULT enResult, int nFlowNo)
		{
			if (m_dicFlowConditions.ContainsKey(sTaskName))
			{
                return m_dicFlowConditions[sTaskName].SetPreconditionFlowNo(sTableName, nFlowIndex, enResult, nFlowNo);
			}

			return false;
		}
		public int GetPostconditionFlowNo(string sTaskName, string sTableName, int nFlowIndex, EN_ACTION_RESULT enResult)
		{
			if (m_dicFlowConditions.ContainsKey(sTaskName))
			{
				return m_dicFlowConditions[sTaskName].GetPostconditionFlowNo(sTableName, nFlowIndex, enResult);
			}

			return 0;
		}
        public bool SetPostconditionFlowNo(string sTaskName, string sTableName, int nFlowIndex, EN_ACTION_RESULT enResult, int nFlowNo)
		{
			if (m_dicFlowConditions.ContainsKey(sTaskName))
			{
                return m_dicFlowConditions[sTaskName].SetPostconditionFlowNo(sTableName, nFlowIndex, enResult, nFlowNo);
			}

			return false;
		}
		#endregion

		#endregion

        /// <summary>
        /// 인자로 지정한 TASK 의 ACTION 중 SEQUENCE 가 진행 중인 ACTION NAME 반환
        /// </summary>
        public bool GetRunningAction(string sTaskName, out string sRunningActionName)
        {
            sRunningActionName = string.Empty;

            string[] arAction;

            if (GetActionList(sTaskName, out arAction))
            {
                foreach (string sActionName in arAction)
                {
                    string sNodeLink = sTaskName + "." + sActionName;

                    if (m_dicNodeLinks.ContainsKey(sNodeLink))
                    {
                        if (m_dicNodeLinks[sNodeLink].State.Equals(EN_ACTION_STATE.BUSY))
                        {
                            sRunningActionName = sActionName;
                            return true;
                        }
                    }
                }
            }

            return false;
        }
        /// <summary>
        /// 인자로 지정한 FLOW TABLE 로 변경 (최초 한 번은 설정해야 한다)
        /// </summary>
        public bool SetFlowTable(string sTaskName, string sTableName)
        {
            FlowLink flowLink = null;
            if (m_dicFlowConditions.TryGetValue(sTaskName, out flowLink))
            {
                flowLink.TableName = sTableName;
                return true;
            }
            return false;
        }

        /// <summary>
        /// 2022.03.03 by shkim [ADD] 현재 진행중인 FlowTable 이름 확인
        /// </summary>
        public bool GetCurrentFlowTable(string sTaskName, ref string sTableName)
        {
            FlowLink flowLink = null;
            if (m_dicFlowConditions.TryGetValue(sTaskName, out flowLink))
            {
                sTableName = flowLink.TableName;
                return true;
            }
            sTableName = string.Empty;
            return false;
        }

		/// <summary>
		/// FLOW PRECONDITION CALLBACK FUNCTION 할당
		/// </summary>
		public bool SetFlowPrecondition(string sTaskName, string sActionName, ActionNode.CheckConditionDelegate pDelegate)
		{
			string sLinkNode = sTaskName + "." + sActionName;
			if (false == m_dicNodeLinks.ContainsKey(sLinkNode))
				return false;

			m_dicNodeLinks[sLinkNode].SetFlowPrecondition(pDelegate);
			return true;
		}
        /// <summary>
        /// FLOW POSTCONDITION CALLBACK FUNCTION 할당
        /// </summary>
		public bool SetFlowPostcondition(string sTaskName, string sActionName, ActionNode.CheckActionDelegate pDelegate)
		{
			string sLinkNode = sTaskName + "." + sActionName;
			if (false == m_dicNodeLinks.ContainsKey(sLinkNode))
				return false;

			m_dicNodeLinks[sLinkNode].SetFlowPostcondition(pDelegate);
			return true;
		}
        /// <summary>
        /// PORT FINALCONDITION CALLBACK FUNCTION 할당
        /// </summary>
		public bool SetPortFinalCondition(string sTaskName, string sActionName, ActionNode.CheckConditionDelegate pDelegate)
		{
			string sLinkNode = sTaskName + "." + sActionName;
			if (false == m_dicNodeLinks.ContainsKey(sLinkNode))
				return false;

			m_dicNodeLinks[sLinkNode].SetPortFinalCondition(pDelegate);
			return true;
		}
        /// <summary>
        /// NODE FINALCONDITION CALLBACK FUNCTION 할당
        /// </summary>
		public bool SetNodeFinalCondition(string sTaskName, string sActionName, ActionNode.CheckConditionDelegate pDelegate)
		{
			string sLinkNode = sTaskName + "." + sActionName;
			if (false == m_dicNodeLinks.ContainsKey(sLinkNode))
				return false;

			m_dicNodeLinks[sLinkNode].SetNodeFinalCondition(pDelegate);
			return true;
		}
		/// <summary>
		/// NODE STATE 설정
		/// </summary>
		public bool SetNodeState(string sTaskName, string sActionName, EN_ACTION_STATE enState)
		{
			string sLinkNode = sTaskName + "." + sActionName;
			if (false == m_dicNodeLinks.ContainsKey(sLinkNode))
				return false;

			m_dicNodeLinks[sLinkNode].State = enState;

            // 2023.03.01 by jhchoo [ADD] DONE 으로 상태전환 시 NodeLink.UpdateState() 호출하지 않아 Access Node 의 접근상태 해제를 실시
			if (enState == EN_ACTION_STATE.DONE)
				m_dicNodeLinks[sLinkNode].Node.ResetAccess();

			return true;
		}
        /// <summary>
        /// NODE STATE 조회
        /// </summary>
		public bool GetNodeState(string sTaskName, string sActionName, ref EN_ACTION_STATE enState)
		{
			string sLinkNode = sTaskName + "." + sActionName;
			if (false == m_dicNodeLinks.ContainsKey(sLinkNode))
				return false;

			enState = m_dicNodeLinks[sLinkNode].State;
			return true;
		}

        #endregion
    }
}
