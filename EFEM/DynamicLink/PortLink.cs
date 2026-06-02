using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FrameOfSystem3.DynamicLink_
{
    // OBJECT ROLE : NODE 와 PORT 간 동적연결과 조건검사를 실시한다
    public class PortLink
    {
        #region CONSTANT
        private const int FLOW_START_NUMBER = 1;
        #endregion

        #region FIELD
        private ActionNode m_SourceNode = null;

        // KEY: LINK GROUP NAME
        private Dictionary<int, PortLinkGroup> m_dicGroups = new Dictionary<int, PortLinkGroup>();
        #endregion

        #region CONSTRUCTOR
        public PortLink(ActionNode node)
        {
            m_SourceNode = node;
        }
        #endregion

        #region INTERFACE

        #region CREATE LINK
        public bool AddLink(int nGroupKey, string sLinkKey, ActionPort tTargetPort)
        {
            if (m_dicGroups.ContainsKey(nGroupKey).Equals(false))
            {
                PortLinkGroup tNodeLinkGroup = new PortLinkGroup();

                m_dicGroups[nGroupKey] = tNodeLinkGroup;
            }

            if (m_dicGroups[nGroupKey].AddLink(sLinkKey, tTargetPort))
                return true;

            return false;
        }
        #endregion

        #region CONTROL
        // 2021.09.06 by junho [ADD] Port 상태 초기화 추가
        public void InitializeLink()
        {
            m_SourceNode.PortSelectedTaskNames.Clear();

            foreach (KeyValuePair<int, PortLinkGroup> group in m_dicGroups)
            {
                group.Value.InitializeLink();
            }
        }
        /// <summary>
        /// NODE 와 LINK 된 PORT 의 STATUS 를 확인한다
        /// </summary>
        public bool CheckStatus()
        {
            // 설정 조건(GROUP) 이 없는 경우에는 PORT LINK 를 확인하지 않는다
            // 다음 조건(FLOW LINK)을 확인할 수 있도록 TRUE 를 반환한다
            if (m_dicGroups.Count.Equals(0))
                return true;

            switch (m_SourceNode.State)
            {
                case EN_ACTION_STATE.READY:
                    // NODE STATE 가 READY 일 경우에만 PORT LINK 를 확인한다
                    break;

                case EN_ACTION_STATE.DONE:
                    // PORT LINK 가 2개 이상인 경우 LINK 가 예상되는 TASK NAME 삭제
                    m_SourceNode.PortSelectedTaskNames.Clear();

                    // NODE STATE 가 DONE 일 경우에는 PORT LINK 를 확인하지 않는다
                    // 다음 조건(FLOW LINK)을 확인할 수 있도록 TRUE 를 반환한다
                    return true;
                
                default:
                    // NODE STATE 가 READY 가 아닐 경우에는 PORT LINK 를 확인하지 않는다
                    // 다음 조건(FLOW LINK)을 확인할 수 있도록 TRUE 를 반환한다
                    return true;
            }

            List<bool> lstCondition = new List<bool>();

            // 설정 조건 확인
            foreach (KeyValuePair<int, PortLinkGroup> group in m_dicGroups)
            {
                lstCondition.Add(group.Value.CheckStatus(m_SourceNode));
            }
		
            // GROUP 간 조건은 전부 AND LOGIC 이므로 FALSE 가 1 개라도 있는 경우에는 FALSE 를 반환한다
            foreach (bool condition in lstCondition)
            {
				if (condition.Equals(false))
					return false;
            }

            // 2021.08.11 by jhchoo [MOD] CheckPortPrecondition > CheckPortFinalCondition
			EN_CONDITION_RESULT enResult = m_SourceNode.CheckPortFinalCondition();

			switch (enResult)
			{
				case EN_CONDITION_RESULT.OK:
					return true;

				case EN_CONDITION_RESULT.NG:
					return false;

				case EN_CONDITION_RESULT.ESCAPE:
					m_SourceNode.PortSelectedTaskNames.Clear();
					return false;
			}

            return true;
        }
        public void UpdateStatus(FlowLink flow)
        {
            switch (m_SourceNode.State)
            {
                case EN_ACTION_STATE.READY:
                    foreach (KeyValuePair<int, PortLinkGroup> group in m_dicGroups)
                    {
                        // GROUP 내 PORT 가 2개 이상일 경우 선택된 1개 PORT 만 사용 (독점 LINK 설정)
                        group.Value.UpdateLink(m_SourceNode, flow);
                    }
                    break;

                case EN_ACTION_STATE.DONE:
                    foreach (KeyValuePair<int, PortLinkGroup> group in m_dicGroups)
                    {
                        // 한 ACTION 의 TRANSACTION 이 완료되었으므로 모든 PORT 의 사용을 초기화 (독점 LINK 해제)
                        group.Value.InitializeLink(flow);
                    }
                    break;
            }
        }
        #endregion

        #endregion

        #region GETTER/SETTER
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
        public EN_PORT_STATUS GetStatusCondition(int nGroupKey, string sLinkKey)
        {
            if (m_dicGroups.ContainsKey(nGroupKey))
                return m_dicGroups[nGroupKey].GetStatusCondition(sLinkKey);

            return EN_PORT_STATUS.DISABLE;
        }
        public bool SetStatusCondition(int nGroupKey, string sLinkKey, EN_PORT_STATUS enStatusCondition)
        {
            if (m_dicGroups.ContainsKey(nGroupKey))
            {
                if (m_dicGroups[nGroupKey].SetStatusCondition(sLinkKey, enStatusCondition))
                    return true;
            }

            return false;
        }
        #endregion

        private class PortLinkGroup
        {
            #region FILED
            // KEY: LINK ITEM NAME
            private Dictionary<string, PortLinkItem> m_dicItems = new Dictionary<string, PortLinkItem>();

            private List<PortLinkItem> m_lstCheckedItems = new List<PortLinkItem>();
            #endregion

            #region CREATE LINK
            public bool AddLink(string sLinkKey, ActionPort tTargetPort)
            {
                if (m_dicItems.ContainsKey(sLinkKey).Equals(false))
                {
                    PortLinkItem tPortLinkItem = new PortLinkItem(tTargetPort);

                    m_dicItems[sLinkKey] = tPortLinkItem;

                    return true;
                }

                return false;
            }
            #endregion

            #region INTERFACE
            // 2021.09.06 by junho [ADD] Port 상태 초기화 추가
            public void InitializeLink()
            {
                // 이전 상태와 무관하게 무조건 초기화를 해주는 인터페이스가 필요
                // 이유 : Task 사용유무가 옵션인 것도 있기 때문이다
                foreach (KeyValuePair<string, PortLinkItem> item in m_dicItems)
                {
                    item.Value.ResetExclusiveLink();
                    item.Value.Enable = true;
                }
            }
            public void InitializeLink(FlowLink flow)
            {
                // from here : LINK 의 사용유무를 초기상태로 만들기 위해 EQ Parameter 값으로 재설정하는 부분 추가 필요
                // 이유 : Task 사용유무가 옵션인 것도 있기 때문이다
                foreach (KeyValuePair<string, PortLinkItem> item in m_dicItems)
                {
                    if (item.Value.Enable)
                        item.Value.ResetExclusiveLink(flow);    // 독점 LINK 해제

                    item.Value.Enable = true;
                }
            }
            public bool CheckStatus(ActionNode node)
            {
                // 설정 조건(ITEM) 이 없는 경우에는 PORT LINK 를 확인하지 않는다
                // AND LOGIC 판단에 제외될 수 있도록 TRUE 를 반환한다
                if (m_dicItems.Count.Equals(0))
                    return true;

				List<PortLinkItem> lstItems = new List<PortLinkItem>();

                // 설정 조건 확인
                foreach (KeyValuePair<string, PortLinkItem> item in m_dicItems)
                {
                    if (item.Value.CheckCondition(node))
                    {
						lstItems.Add(item.Value);
                    }
                }

                // ITEM 간 조건은 전부 OR LOGIC 이므로 TRUE 가 1개도 없는 경우에는 FALSE 를 반환한다
				if (lstItems.Count.Equals(0))
				{
					m_lstCheckedItems.Clear();
					return false;
				}

                // 2023.02.03 by jhchoo [ADD] PORT LINK 가 1개 인 경우 선택할 TASK 도 없으므로 리턴한다
                if (m_dicItems.Count.Equals(1))
                    return true;

                // 2021.08.08 by junho [MOD] OR LOGIC 순환 선택 방식
                // 이전 ITEM 조건 확인으로 선택된 TASK NAME 을 삭제한다
                // 조건에 맞는 ITEM 이 다수 존재할 수 있으므로 순환하며 선택하기 위해서다
                // 단, 다른 GROUP 에서 선택된 TASK NAME 은 삭제하지 않는다 (LIST.Clear() 금지)
				foreach(PortLinkItem item in m_dicItems.Values)
				{
					if (node.PortSelectedTaskNames.Contains(item.TaskName))
						node.PortSelectedTaskNames.Remove(item.TaskName);
				}

                // 조건에 맞는 ITEM 을 매번 LIST 에 ADD 했다가 LIST 에 없는 ITEM 의 TASK NAME 을 선택하는 방식
                foreach (PortLinkItem item in lstItems)
                {
                    if (m_lstCheckedItems.Contains(item).Equals(false))
                    {
                        // 2024.08.08 by junho [MOD] 동일 task면 추가하지 않도록 개선 (어차피 확인도 안함)
                        //m_lstCheckedItems.Add(item);
                        //node.PortSelectedTaskNames.Add(item.TaskName);
                        if (false == node.TaskName.Equals(item.TaskName))
                        {
                            m_lstCheckedItems.Add(item);
                            node.PortSelectedTaskNames.Add(item.TaskName);
                        }
                        return true;
                    }
                }

                m_lstCheckedItems.Clear();
                // 2024.08.08 by junho [MOD] 동일 task면 추가하지 않도록 개선 (어차피 확인도 안함)
                //node.PortSelectedTaskNames.Add(lstItems[0].TaskName);
                if (m_lstCheckedItems.Contains(lstItems[0]).Equals(false))
                    node.PortSelectedTaskNames.Add(lstItems[0].TaskName);

                return true;
            }
            // PORT 가 2개 이상일 경우 조건확인이 결정된 1개의 PORT 만 사용할 수 있도록 한다
            public void UpdateLink(ActionNode node, FlowLink flow)
            {
				List<PortLinkItem> lstItems = new List<PortLinkItem>();

                // 설정 조건 확인
                foreach (KeyValuePair<string, PortLinkItem> item in m_dicItems)
                {
					if (item.Value.CheckCondition(node))
					{
						lstItems.Add(item.Value);
					}

                    // GROUP 내 모든 PORT LINK 를 확인 불가능으로 설정한다
                    // 아래 루프에서 확인 가능한 NODE LINK 를 1개만 설정한다
                    item.Value.Enable = false;
                }

				bool bCheckedEnable = false;

				// 2021.08.09 by junho [ADD] Port selected list에서 선택 했던 Task인지 확인하여 사용한다.
				foreach (PortLinkItem item in lstItems)
				{
					if (node.PortSelectedTaskNames.Contains(item.TaskName))
					{
						// GROUP 내 1개 PORT LINK 만 확인 가능으로 설정
						item.Enable = true;
						bCheckedEnable = true;

						// 독점 PORT 설정 : LINK ITEM 이 2개 이상인 경우 선택된 LINK ITEM 만 해당
						if (m_dicItems.Count > 1)
                            item.SetExclusiveLink(node, flow);

						return;
					}
				}

                // 2021.08.09 by junho [ADD] 확인 가능한 ITEM 이 없다면 모든 ITEM 을 재 확인이 가능하도록 설정
				if (bCheckedEnable.Equals(false))
				{
					foreach(PortLinkItem item in m_dicItems.Values)
					{
						item.Enable = true;
					}
				}
            }
            #endregion

            #region GETTER/SETTER
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
            public EN_PORT_STATUS GetStatusCondition(string sLinkKey)
            {
                if (m_dicItems.ContainsKey(sLinkKey))
                    return m_dicItems[sLinkKey].StatusCondition;

                return EN_PORT_STATUS.DISABLE;
            }
            public bool SetStatusCondition(string sLinkKey, EN_PORT_STATUS enStatusCondition)
            {
                if (m_dicItems.ContainsKey(sLinkKey))
                {
                    m_dicItems[sLinkKey].StatusCondition = enStatusCondition;

                    return true;
                }

                return false;
            }
            #endregion
        }

        private class PortLinkItem
        {
            #region FIELD
            private ActionPort m_LinkedPort = null;

            private bool m_bEnable = true;  // PORT LINK 확인 가능여부

            private EN_PORT_STATUS m_enStatusCondition = EN_PORT_STATUS.DISABLE;
            #endregion

            #region PROPERTY
			public bool Enable { set { m_bEnable = value; } get { return m_bEnable; } }
            public EN_PORT_STATUS StatusCondition { set { m_enStatusCondition = value; } get { return m_enStatusCondition; } }
            public string TaskName { set { m_LinkedPort.TaskName = value; } get { return m_LinkedPort.TaskName; } }
            #endregion

            #region CONSTRUCTOR
            public PortLinkItem(ActionPort port)
            {
                m_LinkedPort = port;
            }
            #endregion

            #region INTERFACE

            #region CONTROL
            public bool CheckCondition(ActionNode node)
            {
                if (m_bEnable.Equals(false))
                    return false;

                if (m_LinkedPort.IsLinked(node.TaskName, node.ActionName).Equals(false))
                    return false;

                switch (m_LinkedPort.Status)
                {
                    case EN_PORT_STATUS.DISABLE:
                    case EN_PORT_STATUS.UNABLE:
                        return false;
                }

                return (m_LinkedPort.Status.Equals(m_enStatusCondition));
            }
            // 독점 LINK 가 설정된 NODE 는 지정된 NODE 이외의 다른 NODE 들과는 LINK 불가
            public void SetExclusiveLink(ActionNode node, FlowLink flow)
            {
                // PORT 와 NODE 가 동일한 TASK 에 속한다면 독점 LINK 를 설정할 수 없다
                if (m_LinkedPort.TaskName.Equals(node.TaskName))
                    return;

                // PORT 독점 LINK 는 FLOW 진행 중 변경될 수 없다
                // 단, FLOW NO 가 1 인 경우 PORT 독점 LINK 설정이 가능하다
                // FLOW NO = 1 이란 FLOW 의 시작 ACTION 을 의미한다
                if (node.IsActionFlow.Equals(false))
                {
                    m_LinkedPort.SetHoldLink(node.TaskName, node.ActionName);
                }
                else if (flow != null && flow.IsContinuousFlow())
                {
                    if (node.ActionName.Equals(flow.GetActionName(flow.TableName, FLOW_START_NUMBER)))
                        m_LinkedPort.SetHoldLink(node.TaskName, node.ActionName);
                }
            }
            // FLOW 가 아닌 ACTION 이라고 반드시 독점 LINK 가 해제되는 것은 아니다
            // 일반 ACTION 에 이어 바로 FLOW 가 시작될 경우 독점 LINK 는 유지된다
            // 이유 : 일반 ACTION 이 FLOW ACTION 으로 변경 전 FLOW POSTCONDITION 에서 먼저 FLOW NO 를 1 로 변경하기 때문
            // 또한 마지막 FLOW ACTION 은 FLOW POSTCONDITION 에서 먼저 FLOW NO 를 0 으로 변경하기 때문에 독점 LINK 가 해제될 수 있다
            public void ResetExclusiveLink(FlowLink flow)
            {
                // NEXT FLOW NO = 0 : FLOW 가 진행 중이 아니란 의미 (STOP WORK FLOW)
                // 독점 LINK 는 FLOW 진행 중 해제될 수 없다
                if (flow.IsContinuousFlow().Equals(false))
                    m_LinkedPort.ResetHoldLink();
            }
			// 2021.09.06 by junho [ADD] Port 상태 초기화 추가
			public void ResetExclusiveLink()
			{
				m_LinkedPort.ResetHoldLink();
			}
            #endregion

            #endregion

        }
    }
}
