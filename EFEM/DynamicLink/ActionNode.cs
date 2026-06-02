using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FrameOfSystem3.DynamicLink_
{
    #region ENUMERATION
    public enum EN_ACTION_STATE
    {
        READY,  // 준비: 액션의 초기상태
        WAIT,   // 대기: 다른 액션과 동시 실행되기 위해 기다리는 상태
        BUSY,   // 실행: 액션이 실행되고 있는 상태
        DONE,   // 완료: 액션의 실행이 완료된 상태
        PASS,   // 통과: LINK CONDITION 확인 시 항상 TURE 를 반환
        SKIP    // 무시: LINK CONDITION 확인 시 항상 FALSE 를 반환
    }
    #endregion

    // OBJECT ROLE : 동적 연결을 위한 ACTION 의 정적 기본단위
    public class ActionNode
    {
        #region FIELD
        private string m_sTaskName = string.Empty;
        private string m_sActionName = string.Empty;
        private EN_ACTION_STATE m_enState = EN_ACTION_STATE.READY;

        private bool m_bEnable = true;  // STATE 전이 가능여부

        // PORT LINK 가 2개 이상인 경우 LINK 가 예상되는 TASK NAME
        private List<string> m_lstPortSelectedTaskNames = new List<string>();

        // HOLD LINK 로 지정된 NODE 만 독점적으로 연결할 경우 사용
        private bool m_bEnableHoldLink = false;
        private string m_sHoldLinkNode = string.Empty;

        // NODE 의 상태를 확인하는 다른 NODE 들의 접근여부
        // key : TaskName.ActionName (접근노드)
        // value : 접근여부
        private Dictionary<string, bool> m_dicAccessNode = new Dictionary<string, bool>();

        private bool m_nIsActionFlow = false;   // FLOW LINK 사용 여부

        public delegate EN_CONDITION_RESULT CheckConditionDelegate();
        public delegate EN_ACTION_RESULT CheckActionDelegate();

        // PORT STATUS 확인 후 콜백 호출
        public CheckConditionDelegate PortFinalConditionCallbackFunc = null;

        // FLOW NUMBER 확인 후 콜백 호출
        public CheckConditionDelegate FlowPreconditionCallbackFunc = null;

        // ACTION 완료 후 콜백 호출
        public CheckActionDelegate FlowPostconditionCallbackFunc = null;
        
        // NODE STATE 확인 후 콜백 호출 (상태 전이 직전)
		public CheckConditionDelegate NodeFinalConditionCallbackFunc = null;
        #endregion

        #region PROPERTY
        public string TaskName { set { m_sTaskName = value; } get { return m_sTaskName; } }
        public string ActionName { set { m_sActionName = value; } get { return m_sActionName; } }
        public EN_ACTION_STATE State { set{ m_enState = value; } get{ return m_enState; } }
        public bool Enable { set{ m_bEnable = value; } get{ return m_bEnable; } }
		public List<string> PortSelectedTaskNames { set { m_lstPortSelectedTaskNames = value; } get { return m_lstPortSelectedTaskNames; } }
        public bool IsActionFlow { set { m_nIsActionFlow = value; } get { return m_nIsActionFlow; } }
        #endregion

        #region CONSTRUCTOR
        public ActionNode(string sTaskName, string sActionName)
        {
            m_sTaskName = sTaskName;
            m_sActionName = sActionName;
        }
        #endregion

        #region INTERFACE
        public void SetFlowPrecondition(ref CheckConditionDelegate pDelegate)
        {
            FlowPreconditionCallbackFunc = pDelegate; 
        }
        public void SetFlowPostcondition(ref CheckActionDelegate pDelegate) 
        {
            FlowPostconditionCallbackFunc = pDelegate; 
        }
        public void SetPortFinalCondition(ref CheckConditionDelegate pDelegate)
        {
            PortFinalConditionCallbackFunc = pDelegate;
        }
		public void SetNodeFinalCondition(ref CheckConditionDelegate pDelegate)
		{
			NodeFinalConditionCallbackFunc = pDelegate;
		}
        public EN_CONDITION_RESULT CheckFlowPrecondition()
	    {
            if (FlowPreconditionCallbackFunc == null)
		    {
			    return EN_CONDITION_RESULT.OK;
		    }

            return FlowPreconditionCallbackFunc();
	    }
        public EN_ACTION_RESULT CheckFlowPostcondition()
        {
            if (FlowPostconditionCallbackFunc == null)
            {
                return EN_ACTION_RESULT.SUCCESS;
            }

            return FlowPostconditionCallbackFunc();
        }
        public EN_CONDITION_RESULT CheckPortFinalCondition()
        {
            if (PortFinalConditionCallbackFunc == null)
            {
                return EN_CONDITION_RESULT.OK;
            }

            return PortFinalConditionCallbackFunc();
        }
		public EN_CONDITION_RESULT CheckNodeFinalCondition()
		{
			if (NodeFinalConditionCallbackFunc == null)
			{
				return EN_CONDITION_RESULT.OK;
			}

			return NodeFinalConditionCallbackFunc();
		}
        public bool IsLinked(string sTaskName, string sActionName)
        {
            bool bLinked = true;

            string sNodeLink = sTaskName + "." + sActionName;

            if (m_bEnableHoldLink)
            {
                if (m_sHoldLinkNode.Equals(sNodeLink).Equals(false))
                    bLinked = false;
            }

            return m_bEnable && bLinked;
        }
        public void SetHoldLink(string sTaskName, string sActionName)
        {
            m_bEnableHoldLink = true;
            m_sHoldLinkNode = sTaskName + "." + sActionName;
        }
        public void ResetHoldLink()
        {
            m_bEnableHoldLink = false;
            m_sHoldLinkNode = string.Empty;
        }
        // 상태전이한 NODE 의 상태를 확인하는 모든 NODE 들의 접근여부 (배타적 상태전이 방지)
        public bool IsAccessed()
        {
            foreach (KeyValuePair<string, bool> node in m_dicAccessNode)
            {
                if (node.Value.Equals(false))
                    return false;
            }

            return true;
        }
        public void SetAccess(string sTaskName, string sActionName)
        {
            string sNode = sTaskName + "." + sActionName;

            m_dicAccessNode[sNode] = true;
        }
        public void ResetAccess()
        {
            foreach (string node in m_dicAccessNode.Keys.ToList())
            {
                m_dicAccessNode[node] = false;
            }
        }
		// 2021.08.07 by junho [ADD] 해당 Node와 Hold되어 있는지 여부 반환 
		public bool IsHoldThisNode(string taskName, string actionName)
		{
			if (false == m_bEnableHoldLink)
				return false;

			return m_sHoldLinkNode.Equals(string.Format("{0}.{1}", taskName, actionName));
		}
		// 2021.08.07 by junho [ADD] 예상 port list에 해당 Task가 있는지 여부 반환
		public bool IsExistThisTaskInExpectedList(string taskName)
		{
			return m_lstPortSelectedTaskNames.Contains(taskName);
		}
        #endregion

        #region GETTER/SETTER
        public bool GetAccessNodeList(ref string[] arTask, ref string[] arNode, ref int nItemCount)
        {
            nItemCount = m_dicAccessNode.Count;

            if (nItemCount > 0)
            {
                arTask = new string[nItemCount];
                arNode = new string[nItemCount];

                int nIndex = 0;

                foreach (KeyValuePair<string, bool> node in m_dicAccessNode)
                {
                    char cSplit = '.';
                    string[] sSplit = node.Key.Split(cSplit);

                    arTask[nIndex] = sSplit[0];
                    arNode[nIndex] = sSplit[1];

                    ++nIndex;
                }

                return true;
            }

            return false;
        }
        public void SetAccessNode(string sAccessNode)
        {
            if (m_dicAccessNode.ContainsKey(sAccessNode).Equals(false))
            {
                m_dicAccessNode[sAccessNode] = false;
            }
        }
		/// <summary>
		/// 2021.08.05 by twkang [ADD] 
		/// </summary>
		public bool IsAccessedCheck(string sTaskName, string sActionName)
		{
			string sNode = sTaskName + "." + sActionName;

			if (m_dicAccessNode.ContainsKey(sNode))
			{
				return m_dicAccessNode[sNode];
			}

			return false;
		}
        #endregion
    }
}