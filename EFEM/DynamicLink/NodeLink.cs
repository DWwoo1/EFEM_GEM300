using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FrameOfSystem3.DynamicLink_
{
    // OBJECT ROLE : NODE 와 NODE 간 동적연결과 조건검사를 실시하여 상태전이를 결정한다
    public class NodeLink
    {
        #region FIELD
        private ActionNode m_SourceNode = null;

        // KEY: LINK GROUP NAME
        private Dictionary<int, NodeLinkGroup> m_dicGroups = new Dictionary<int, NodeLinkGroup>();
        #endregion

        #region PROPERTY
        public EN_ACTION_STATE State { set { m_SourceNode.State = value; } get { return m_SourceNode.State; } }
        public bool Enable { set { m_SourceNode.Enable = value; } get { return m_SourceNode.Enable; } }
        public ActionNode Node { get { return m_SourceNode; } }
        #endregion

        #region CONSTRUCTOR
        public NodeLink(ActionNode node)
        {
            m_SourceNode = node;
        }
        #endregion

        #region INTERFACE

        #region CREATE LINK
        public bool AddLink(int nGroupKey, string sLinkKey, ActionNode tTargetNode)
        {
            if (m_dicGroups.ContainsKey(nGroupKey).Equals(false))
            {
                NodeLinkGroup tNodeLinkGroup = new NodeLinkGroup();

                m_dicGroups[nGroupKey] = tNodeLinkGroup;
            }

            if (m_dicGroups[nGroupKey].AddLink(sLinkKey, tTargetNode))
            {
                string sAccessNode = m_SourceNode.TaskName + "." + m_SourceNode.ActionName;

                tTargetNode.SetAccessNode(sAccessNode);

                return true;
            }

            return false;
        }
        #endregion

        #region CONTROL
        public void InitializeLink()
        {
            foreach (KeyValuePair<int, NodeLinkGroup> group in m_dicGroups)
            {
                // 모든 LINK 의 사용을 초기화 (독점 LINK 해제)
                group.Value.InitializeLink();
            }
        }
        /// <summary>
        /// //SOURCE NODE 와 LINK 된 TARGET NODE 의 STATE 를 확인한다
        /// </summary>
        public void CheckState()
        {
            // 상태전이 조건 확인
            foreach (KeyValuePair<int, NodeLinkGroup> group in m_dicGroups)
            {
                group.Value.CheckState(m_SourceNode);
            }
        }
        public bool UpdateState(PortLink pPort, FlowLink pFlow)
        {
			// 2023.04.05 by junho [DEL] 독점 link 해제 시점 변경 DONE>READY로 바뀌기 직전에 수행하도록 한다.
			//if (m_SourceNode.State.Equals(EN_ACTION_STATE.DONE))
			//{
			//	foreach (KeyValuePair<int, NodeLinkGroup> group in m_dicGroups)
			//	{
			//		// 한 ACTION 의 TRANSACTION 이 완료되었으므로 모든 LINK 의 사용을 초기화 (독점 LINK 해제)
			//		group.Value.InitializeLink();
			//	}
			//	// 독점 PORT LINK 해제
			//	// PORT 를 사용하지 않는 경우가 있으므로 PortLink 의 Instance 유무를 확인한다
			//	if (pPort != null)
			//		pPort.UpdateStatus(pFlow);
			//}

            List<bool> lstCondition = new List<bool>();

            // 상태전이 조건 확인
            foreach (KeyValuePair<int, NodeLinkGroup> group in m_dicGroups)
            {
                lstCondition.Add(group.Value.CheckState(m_SourceNode));
            }
            // GROUP 간 조건은 전부 AND LOGIC 이므로 FALSE 가 있는 경우에는 FALSE 를 반환한다
            foreach (bool condition in lstCondition)
            {
                if (condition.Equals(false))
                    return false;
            }
            // 역으로 연결된 NODE 확인 후 접근하지 않은 경우 : 상태전이 불가
            if (m_SourceNode.IsAccessed().Equals(false))
                return false;
            
            // 모든 그룹 조건이 TRUE 인 경우 : 상태전이 가능
            switch (m_SourceNode.State)
            {
                case EN_ACTION_STATE.READY:
                    EN_CONDITION_RESULT enResult = m_SourceNode.CheckNodeFinalCondition();

                    switch (enResult)
                    {
                        case EN_CONDITION_RESULT.OK:
                            // 사전조건이 성립하여 READY > WAIT 로 상태전이
                            // 이후 ACTION 이 시작되기 전 DoExecutingPrecondition() 호출됨
                            foreach (KeyValuePair<int, NodeLinkGroup> group in m_dicGroups)
                            {
                                // GROUP 내 LINK 가 2개 이상일 경우 선택된 1개 LINK 만 사용 (독점 LINK 설정)
                                group.Value.UpdateLink(m_SourceNode);
                            }
                            // 독점 PORT LINK 설정
                            // PORT 를 사용하지 않는 경우가 있으므로 PortLink 의 Instance 유무를 확인한다
                            if (pPort != null)
                                pPort.UpdateStatus(pFlow);

                            m_SourceNode.State = EN_ACTION_STATE.WAIT;
                            break;

                        case EN_CONDITION_RESULT.NG:
                            // 사전조건이 성립 또는 탈출 할 때까지 반복확인
                            return false;

                        case EN_CONDITION_RESULT.ESCAPE:
                            // 사전조건이 성립 또는 탈출 할 때까지 반복확인
                            return false;
                    }
                    break;

                case EN_ACTION_STATE.WAIT:
                    m_SourceNode.State = EN_ACTION_STATE.BUSY;
                    break;

                case EN_ACTION_STATE.BUSY:
                    // ACTION 수행 완료 후 RunningTask 에 의해 [DONE] 으로 상태전이
                    break;

				case EN_ACTION_STATE.DONE:
					// 2023.04.05 by junho [ADD] 독점 link 해제 시점 변경 DONE>READY로 바뀌기 직전에 수행하도록 한다.
					foreach (KeyValuePair<int, NodeLinkGroup> group in m_dicGroups)
					{
						// 한 ACTION 의 TRANSACTION 이 완료되었으므로 모든 LINK 의 사용을 초기화 (독점 LINK 해제)
						group.Value.InitializeLink();
					}
					// 독점 PORT LINK 해제
					// PORT 를 사용하지 않는 경우가 있으므로 PortLink 의 Instance 유무를 확인한다
					if (pPort != null)
						pPort.UpdateStatus(pFlow);

                    m_SourceNode.State = EN_ACTION_STATE.READY;
                    break;
            }
            // 역으로 연결된 NODE 의 접근상태 해제
            m_SourceNode.ResetAccess();

            return true;
        }

        // 2023.11.27 by shkim [ADD] PWA-500W 링크에서 Done 유지 상태에서 독점 링크해제해야해서 임시 추가
        public bool UpdateStateWithResetExclusiveLinkWhenDoneState(PortLink pPort, FlowLink pFlow)
        {
            // 2023.11.27 by shkim [MOD] PWA-500W 링크에서 Done 유지 상태에서 독점 링크해제해야 했다.
            // 2023.04.05 by junho [DEL] 독점 link 해제 시점 변경 DONE>READY로 바뀌기 직전에 수행하도록 한다.
            if (m_SourceNode.State.Equals(EN_ACTION_STATE.DONE))
            {
                foreach (KeyValuePair<int, NodeLinkGroup> group in m_dicGroups)
                {
                    // 한 ACTION 의 TRANSACTION 이 완료되었으므로 모든 LINK 의 사용을 초기화 (독점 LINK 해제)
                    group.Value.InitializeLink();
                }
                // 독점 PORT LINK 해제
                // PORT 를 사용하지 않는 경우가 있으므로 PortLink 의 Instance 유무를 확인한다
                if (pPort != null)
                    pPort.UpdateStatus(pFlow);
            }

            List<bool> lstCondition = new List<bool>();

            // 상태전이 조건 확인
            foreach (KeyValuePair<int, NodeLinkGroup> group in m_dicGroups)
            {
                lstCondition.Add(group.Value.CheckState(m_SourceNode));
            }
            // GROUP 간 조건은 전부 AND LOGIC 이므로 FALSE 가 있는 경우에는 FALSE 를 반환한다
            foreach (bool condition in lstCondition)
            {
                if (condition.Equals(false))
                    return false;
            }
            // 역으로 연결된 NODE 확인 후 접근하지 않은 경우 : 상태전이 불가
            if (m_SourceNode.IsAccessed().Equals(false))
                return false;

            // 모든 그룹 조건이 TRUE 인 경우 : 상태전이 가능
            switch (m_SourceNode.State)
            {
                case EN_ACTION_STATE.READY:
                    EN_CONDITION_RESULT enResult = m_SourceNode.CheckNodeFinalCondition();

                    switch (enResult)
                    {
                        case EN_CONDITION_RESULT.OK:
                            // 사전조건이 성립하여 READY > WAIT 로 상태전이
                            // 이후 ACTION 이 시작되기 전 DoExecutingPrecondition() 호출됨
                            foreach (KeyValuePair<int, NodeLinkGroup> group in m_dicGroups)
                            {
                                // GROUP 내 LINK 가 2개 이상일 경우 선택된 1개 LINK 만 사용 (독점 LINK 설정)
                                group.Value.UpdateLink(m_SourceNode);
                            }
                            // 독점 PORT LINK 설정
                            // PORT 를 사용하지 않는 경우가 있으므로 PortLink 의 Instance 유무를 확인한다
                            if (pPort != null)
                                pPort.UpdateStatus(pFlow);

                            m_SourceNode.State = EN_ACTION_STATE.WAIT;
                            break;

                        case EN_CONDITION_RESULT.NG:
                            // 사전조건이 성립 또는 탈출 할 때까지 반복확인
                            return false;

                        case EN_CONDITION_RESULT.ESCAPE:
                            // 사전조건이 성립 또는 탈출 할 때까지 반복확인
                            return false;
                    }
                    break;

                case EN_ACTION_STATE.WAIT:
                    m_SourceNode.State = EN_ACTION_STATE.BUSY;
                    break;

                case EN_ACTION_STATE.BUSY:
                    // ACTION 수행 완료 후 RunningTask 에 의해 [DONE] 으로 상태전이
                    break;

                case EN_ACTION_STATE.DONE:
                    // 2023.11.27 by shkim [DEL] PWA-500W 링크에서 Done 유지 상태에서 독점 링크해제해야 했다.
                    //// 2023.04.05 by junho [ADD] 독점 link 해제 시점 변경 DONE>READY로 바뀌기 직전에 수행하도록 한다.
                    //foreach (KeyValuePair<int, NodeLinkGroup> group in m_dicGroups)
                    //{
                    //    // 한 ACTION 의 TRANSACTION 이 완료되었으므로 모든 LINK 의 사용을 초기화 (독점 LINK 해제)
                    //    group.Value.InitializeLink();
                    //}
                    //// 독점 PORT LINK 해제
                    //// PORT 를 사용하지 않는 경우가 있으므로 PortLink 의 Instance 유무를 확인한다
                    //if (pPort != null)
                    //    pPort.UpdateStatus(pFlow);

                    m_SourceNode.State = EN_ACTION_STATE.READY;
                    break;
            }
            // 역으로 연결된 NODE 의 접근상태 해제
            m_SourceNode.ResetAccess();

            return true;
        }


		/// 2021.08.07 by junho [ADD] 해당 Node와 Hold되어 있는지 여부 반환 
		public bool IsHoldThisNode(string taskName, string actionName)
		{
			return m_SourceNode.IsHoldThisNode(taskName, actionName);
		}
		// 2021.08.09 by junho [ADD] Source Node의 Port selected list에 TargetTask가 있는지 여부 반환
		public bool IsExistThisTaskInSelectedList(string taskName)
		{
			return m_SourceNode.IsExistThisTaskInExpectedList(taskName);
		}
        #endregion

        #endregion

        #region SETTER/GETTER
        public bool GetAccessNodeList(ref string[] arTask, ref string[] arAction, ref int nItemCount)
        {
            if (m_SourceNode != null)
                return m_SourceNode.GetAccessNodeList(ref arTask, ref arAction, ref nItemCount);

            return false;
        }
        public object GetNodeLinkItem(int nGroupKey, string sLinkKey)
        {
            if (m_dicGroups.ContainsKey(nGroupKey))
                return m_dicGroups[nGroupKey].GetNodeLinkItem(sLinkKey);

            return null;
        }
        public bool SetPairLink(int nGroupKey, string sLinkKey, object tNodeLinkItem)
        {
            if (m_dicGroups.ContainsKey(nGroupKey))
            {
                NodeLinkItem tNode = tNodeLinkItem as NodeLinkItem;

                if (tNode != null)
                    return m_dicGroups[nGroupKey].SetPairLink(sLinkKey, tNode);
            }

            return false;
        }
        public bool GetLinkEnable(int nGroupKey, string sLinkKey)
        {
            if (m_dicGroups.ContainsKey(nGroupKey))
                return m_dicGroups[nGroupKey].GetLinkEnable(sLinkKey);

            return false;
        }
        public bool SetLinkEnable(int nGroupKey, string sLinkKey, bool bEnable)
        {
            if (m_dicGroups.ContainsKey(nGroupKey))
                return m_dicGroups[nGroupKey].SetLinkEnable(sLinkKey, bEnable); ;

            return false;
        }
        public EN_ACTION_STATE GetStateCondition(int nGroupKey, string sLinkKey, EN_ACTION_STATE enNodeState)
        {
            if (m_dicGroups.ContainsKey(nGroupKey))
                return m_dicGroups[nGroupKey].GetStateCondition(sLinkKey, enNodeState);

            return EN_ACTION_STATE.SKIP;
        }
        public bool SetStateCondition(int nGroupKey, string sLinkKey, EN_ACTION_STATE enNodeState, EN_ACTION_STATE enStateCondition)
        {
            if (m_dicGroups.ContainsKey(nGroupKey))
            {
                if (m_dicGroups[nGroupKey].SetStateCondition(sLinkKey, enNodeState, enStateCondition))
                    return true;
            }

            return false;
        }

		// 2021.07.26 by junho [ADD] Add Flow/Link condition interface
		public void SetFlowPrecondition(ActionNode.CheckConditionDelegate pDelegate)
		{
			m_SourceNode.SetFlowPrecondition(ref pDelegate);
		}
		public void SetFlowPostcondition(ActionNode.CheckActionDelegate pDelegate)
		{
			m_SourceNode.SetFlowPostcondition(ref pDelegate);
		}
		public void SetPortFinalCondition(ActionNode.CheckConditionDelegate pDelegate)
		{
			m_SourceNode.SetPortFinalCondition(ref pDelegate);
		}
		public void SetNodeFinalCondition(ActionNode.CheckConditionDelegate pDelegate)
		{
			m_SourceNode.SetNodeFinalCondition(ref pDelegate);
		}
        #endregion

        private class NodeLinkGroup
        {
            #region FIELD
            // KEY: LINK ITEM NAME
            private Dictionary<string, NodeLinkItem> m_dicItems = new Dictionary<string, NodeLinkItem>();
            #endregion

            #region CREATE LINK
            public bool AddLink(string sLinkKey, ActionNode tTargetNode)
            {
                if (m_dicItems.ContainsKey(sLinkKey).Equals(false))
                {
                    NodeLinkItem tNodeLinkItem = new NodeLinkItem(tTargetNode);

                    m_dicItems[sLinkKey] = tNodeLinkItem;

                    return true;
                }

                return false;
            }
            #endregion

            #region INTERFACE
            public void InitializeLink()
            {
                // from here : LINK 의 사용유무를 초기상태로 만들기 위해 EQ Parameter 값으로 재설정하는 부분 추가 필요
                // 이유 : Task 사용유무가 옵션인 것도 있기 때문이다
                foreach (KeyValuePair<string, NodeLinkItem> item in m_dicItems)
                {
                    if (item.Value.Enable)
                        item.Value.ResetExclusiveLink();    // 독점 LINK 해제

                    item.Value.Enable = true;
                }
            }
            public bool CheckState(ActionNode node)
            {
                List<NodeLinkItem> lstItems = new List<NodeLinkItem>();

                // 설정 조건 확인
                foreach (KeyValuePair<string, NodeLinkItem> item in m_dicItems)
                {
                    if (item.Value.CheckCondition(node))
                    {
                        lstItems.Add(item.Value);
                    }
                }
                // ITEM 간 조건은 전부 OR LOGIC 이므로 TRUE 가 있는 경우에는 TRUE 를 반환한다
                for (int i = 0; i < lstItems.Count; ++i)
                {
					// 2021.08.09 by junho [ADD] 동일 TASK는 확인하지 않아도 무방.
					if (lstItems[i].TaskName.Equals(node.TaskName))
						return true;

					// TASK가 다를 경우 조건 확인 필요.
                    if (node.PortSelectedTaskNames.Count.Equals(0))
                    {
                        // from here : OR LOGIC 인 경우 TRUE 의 다수조건 중 선택 옵션 추가 필요
                        // 빠른 순번 우선 선택 방식
                        if (i.Equals(0))
                        {
                            return true;
                        }
                    }
                    else
                    {
                        // PORT LINK 의 CheckStatus() 에서 설정한 예상되는 TASK NAME 이 있으므로 우선 확인한다
                        if (node.PortSelectedTaskNames.Contains(lstItems[i].TaskName))
                            return true;
                    }
                }
                return false;
            }
            // LINK 가 2개 이상일 경우 조건확인이 결정된 1개의 LINK 만 사용할 수 있도록 한다
            public void UpdateLink(ActionNode node)
            {
                List<NodeLinkItem> lstItems = new List<NodeLinkItem>();

                // 설정 조건 확인
                foreach (KeyValuePair<string, NodeLinkItem> item in m_dicItems)
                {
                    if (item.Value.CheckCondition(node))
                    {
                        lstItems.Add(item.Value);
                    }

                    // GROUP 내 모든 NODE LINK 를 확인 불가능으로 설정한다
                    // 아래 루프에서 확인 가능한 NODE LINK 를 1개만 설정한다
                    item.Value.Enable = false;
                }

                bool bCheckedEnable = false;

                // ITEM 간 조건은 전부 OR LOGIC 이므로 TRUE 가 있는 경우에는 TRUE 를 반환한다
                for (int i = 0; i < lstItems.Count; ++i)
                {
                    if (node.PortSelectedTaskNames.Count.Equals(0))
                    {
                        // from here : OR LOGIC 인 경우 TRUE 의 다수조건 중 선택 옵션 추가 필요
                        // 빠른 순번 우선 선택 방식
                        if (i.Equals(0))
                        {
                            //foreach (KeyValuePair<string, NodeLinkItem> item in m_dicItems)
                            //{
                            //    // GROUP 내 모든 NODE LINK 를 확인 불가능으로 설정한다
                            //    // 아래 루프에서 확인 가능한 NODE LINK 를 1개만 설정한다
                            //    item.Value.Enable = false;
                            //}

                            // GROUP 내 1개 PORT LINK 만 확인 가능으로 설정
                            lstItems[i].Enable = true;
                            bCheckedEnable = true;

                            // 독점 LINK 설정 : LINK ITEM 이 2개 이상인 경우 선택된 LINK ITEM 만 해당
                            if (m_dicItems.Count > 1)
                                lstItems[i].SetExclusiveLink(node);

                            return;
                        }
                    }
                    else
                    {
                        // PORT LINK 의 CheckStatus() 에서 설정한 예상되는 TASK NAME 이 있으므로 우선 확인한다
                        if (node.PortSelectedTaskNames.Contains(lstItems[i].TaskName))
                        {
                            //foreach (KeyValuePair<string, NodeLinkItem> item in m_dicItems)
                            //{
                            //    // GROUP 내 모든 NODE LINK 를 확인 불가능으로 설정한다
                            //    // 아래 루프에서 확인 가능한 NODE LINK 를 1개만 설정한다
                            //    item.Value.Enable = false;
                            //}

                            // GROUP 내 1개 PORT LINK 만 확인 가능으로 설정
                            lstItems[i].Enable = true;
                            bCheckedEnable = true;

                            // 독점 LINK 설정 : LINK ITEM 이 2개 이상인 경우 선택된 LINK ITEM 만 해당
                            if (m_dicItems.Count > 1)
                                lstItems[i].SetExclusiveLink(node);
                            
                            return;
                        }
                    }
                }

                // 2021.09.10 by jhchoo [ADD] 확인 가능한 ITEM 이 없다면 모든 ITEM 을 재 확인이 가능하도록 설정
                if (bCheckedEnable.Equals(false))
                {
                    foreach (NodeLinkItem item in m_dicItems.Values)
                    {
                        item.Enable = true;
                    }
                }
            }
            #endregion

            #region GETTER/SETTER
            public NodeLinkItem GetNodeLinkItem(string sLinkKey)
            {
                if (m_dicItems.ContainsKey(sLinkKey))
                    return m_dicItems[sLinkKey];

                return null;
            }
            public bool SetPairLink(string sLinkKey, NodeLinkItem tNode)
            {
                if (m_dicItems.ContainsKey(sLinkKey))
                {
                    m_dicItems[sLinkKey].SetPairLink(tNode);

                    return true;
                }

                return false;
            }
            public bool GetLinkEnable(string sLinkKey)
            {
                if (m_dicItems.ContainsKey(sLinkKey))
                    return m_dicItems[sLinkKey].Enable;

                return false;
            }
            public bool SetLinkEnable(string sLinkKey, bool bEnable)
            {
                if (m_dicItems.ContainsKey(sLinkKey))
                {
                    m_dicItems[sLinkKey].Enable = bEnable;

                    return true;
                }

                return false;
            }
            public EN_ACTION_STATE GetStateCondition(string sLinkKey, EN_ACTION_STATE enNodeState)
            {
                if (m_dicItems.ContainsKey(sLinkKey))
                    return m_dicItems[sLinkKey].GetStateCondition(enNodeState);

                return EN_ACTION_STATE.SKIP;
            }
            public bool SetStateCondition(string sLinkKey, EN_ACTION_STATE enNodeState, EN_ACTION_STATE enStateCondition)
            {
                if (m_dicItems.ContainsKey(sLinkKey))
                {
                    if (m_dicItems[sLinkKey].SetStateCondition(enNodeState, enStateCondition))
                        return true;
                }
                
                return false;
            }
            #endregion
        }

        private class NodeLinkItem
        {
            #region FIELD
            private ActionNode m_LinkedNode = null;
            private NodeLinkItem m_PairLink = null;     // Instance 주입 필요

            private bool m_bEnable = true;  // NODE LINK 확인 가능여부

            // KEY  : CURRENT NODE STATE
            // VALUE: STATE CONDITION
            private Dictionary<EN_ACTION_STATE, EN_ACTION_STATE> m_dicStateCondition = new Dictionary<EN_ACTION_STATE, EN_ACTION_STATE>();
            #endregion

            #region PROPERTY
            public bool Enable
            {
                set
                {
                    m_bEnable = value;

                    if (m_PairLink != null)
                    {
                        // 상호 참조로 무한루프 발생하므로 반드시 현재 값과 입력 값을 비교하여 다를 경우만 설정할 것
                        if (m_PairLink.Enable != value)
                        {
                            m_PairLink.Enable = value;
                        }
                    }
                }

                get { return m_bEnable; }
            }
            public string TaskName { set { m_LinkedNode.TaskName = value; } get { return m_LinkedNode.TaskName; } }
            #endregion

            #region CONSTRUCTOR
            public NodeLinkItem(ActionNode node)
            {
                m_LinkedNode = node;
            }
            #endregion

            #region INTERFACE
            public bool CheckCondition(ActionNode node)
            {
                // 연결된 NODE 의 접근상태 설정
                m_LinkedNode.SetAccess(node.TaskName, node.ActionName);

                if (m_bEnable.Equals(false))
					return false;

				if (m_LinkedNode.IsLinked(node.TaskName, node.ActionName).Equals(false))
					return false;

                if (m_dicStateCondition.ContainsKey(node.State).Equals(false))
                    return false;

                switch (m_dicStateCondition[node.State])
                {
                    case EN_ACTION_STATE.PASS:
                        return true;

                    case EN_ACTION_STATE.SKIP:
                        return false;
                }

                return (m_dicStateCondition[node.State] == m_LinkedNode.State);
            }
            // 독점 LINK 가 설정된 NODE 는 지정된 NODE 이외의 다른 NODE 들과는 LINK 불가
            public void SetExclusiveLink(ActionNode node)
            {
				// 2023.04.05 by junho [ADD] 동일 Task끼리는 독점 link를 설정 하지 않는다.
				if (m_LinkedNode.TaskName.Equals(node.TaskName))
					return;

                m_LinkedNode.SetHoldLink(node.TaskName, node.ActionName);
            }
            public void ResetExclusiveLink()
            {
                m_LinkedNode.ResetHoldLink();
            }
            #endregion

            #region GETTER/SETTER
            public void SetPairLink(NodeLinkItem tNode)
            {
                m_PairLink = tNode;
            }
            public EN_ACTION_STATE GetStateCondition(EN_ACTION_STATE enNodeState)
		    {
			    if (m_dicStateCondition.ContainsKey(enNodeState))
                    return m_dicStateCondition[enNodeState];

                return EN_ACTION_STATE.SKIP;
		    }
            public bool SetStateCondition(EN_ACTION_STATE enNodeState, EN_ACTION_STATE enStateCondition)
            {
                switch (enNodeState)
                {
                    case EN_ACTION_STATE.DONE:
                    case EN_ACTION_STATE.READY:
                    case EN_ACTION_STATE.WAIT:
                        m_dicStateCondition[enNodeState] = enStateCondition;
                        return true;
                }

                return false;
            }
            #endregion
        }
    }
}
