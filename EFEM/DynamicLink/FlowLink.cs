using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FrameOfSystem3.DynamicLink_
{
    #region ENUMERATION
    public enum EN_CONDITION_RESULT
    {
        OK,         // 조건 만족
        NG,         // 조건 불만족
        ESCAPE      // 탈출
    }
    public enum EN_ACTION_RESULT
    {
        SUCCESS,    // 성공
        FAILURE,    // 실패
        INVALID,    // 무효
    }
    #endregion

    // OBJECT ROLE : 정적인 ACTION 실행순서를 결정
    public class FlowLink
    {
        #region FIELD
        private string m_sCurrentTableName = string.Empty;
        private int m_nNextFlowNo = 0;

        // KEY: TABLE NAME
        private Dictionary<string, FlowTable> m_dicTables = new Dictionary<string, FlowTable>();
        #endregion

        #region PROPERTY
        public string TableName { set { m_sCurrentTableName = value; } get { return m_sCurrentTableName; } }
        #endregion

        #region INTERFACE

        #region CREATE/REMOVE ITEM
        public bool AddFlowTable(string sTableName)
	    {
            if (m_dicTables.ContainsKey(sTableName).Equals(false))
            {
                m_dicTables[sTableName] = new FlowTable();

                return true;
            }
		    
		    return false;
	    }
        public bool RemoveFlowTable(string sTableName)
        {
            if (m_dicTables.ContainsKey(sTableName).Equals(true))
            {
                return m_dicTables.Remove(sTableName);
            }
            return true;
        }
        public bool AddFlowItem(string sTableName, int nFlowNo, ActionNode tNode)
        {
            if (m_dicTables.ContainsKey(sTableName))
            {
                return m_dicTables[sTableName].AddFlowItem(nFlowNo, tNode);
            }

            return false;
        }

        public bool InsertFlowItem(string sTableName, int nFlowNo, ActionNode tNode)
        {
            if (m_dicTables.ContainsKey(sTableName))
            {
                return m_dicTables[sTableName].InsertFlowItem(nFlowNo, tNode);
            }

            return false;
        }

        // 2022.03.02 [ADD] For modifying flow item
        public bool RemoveFlowItem(string sTableName, int nFlowNo)
        {
            if (m_dicTables.ContainsKey(sTableName))
            {
                return m_dicTables[sTableName].RemoveFlowItem(nFlowNo);
            }

            return false;
        }
        #endregion

        #region CONTROL
        public bool CheckCondition(string sActionName)
        {
            // FLOW 가 진행 중이면 FLOW ITEM 이 아닌 NODE 는 상태전이 불가
            if (IsContinuousFlow().Equals(false))     
                return true;

            FlowTable tFlowTable = m_dicTables[m_sCurrentTableName];

			List<FlowItem> listFlowItem = tFlowTable.GetFlowItemList(sActionName);

			if (listFlowItem.Count.Equals(0))
				return false;

            // 확인해야 할 ACTION 이 실행을 위해 이미 지정된 FLOW NO 와 같은지 판단
			FlowItem tFlowItem = null;
			foreach (FlowItem flowItem in listFlowItem)
			{
				if (flowItem.EqualsFlowNo(m_nNextFlowNo))
				{
					tFlowItem = flowItem;
					break;
				}
			}

			if (tFlowItem == null)
				return false;

            // 상태전이
            switch (tFlowItem.State)
            {
                case EN_ACTION_STATE.READY:
                    EN_CONDITION_RESULT enResult = tFlowItem.CheckPrecondition();

                    switch(enResult)
                    {
                        case EN_CONDITION_RESULT.OK:
                            // 사전조건이 성립하여 READY > WAIT 로 상태전이
                            // 이후 ACTION 이 시작되기 전 DoExecutingPrecondition() 호출됨
                            // true 를 반환하여 ActionLink::UpdateState() 에서 상태전이
                            // 이유 : FLOW NODE 이지만 LINK 도 포함되어 있을 수 있기 때문임
                            return true;

                        case EN_CONDITION_RESULT.NG:
                            // 사전조건이 성립 또는 탈출 할 때까지 반복확인

							// 현재 FLOW 의 PRECONDITION 을 탈출할 경우 지정된 Flow No 로 이동
							m_nNextFlowNo = tFlowItem.GetPreconditionFlowNo(enResult);
                            break;

                        case EN_CONDITION_RESULT.ESCAPE:
                            // 현재 FLOW 의 PRECONDITION 을 탈출할 경우 지정된 Flow No 로 이동
                            m_nNextFlowNo = tFlowItem.GetPreconditionFlowNo(enResult);
                            break;
                    }
                    break;

                case EN_ACTION_STATE.WAIT:
                    // m_nNextFlowNo 로 지정된 NODE 는 WAIT > BUSY 으로 조건 없이 상태전이
                    // true 를 반환하여 ActionLink::UpdateState() 에서 상태전이
                    // 이유 : FLOW NODE 이지만 LINK 도 포함되어 있을 수 있기 때문임
                    return true;

                case EN_ACTION_STATE.BUSY:
                    // ACTION 수행 완료 후 RunningTask 에 의해 [DONE] 으로 상태전이
                    break;

                case EN_ACTION_STATE.DONE:
                    // m_nNextFlowNo 로 지정된 NODE 는 WAIT > BUSY 으로 조건 없이 상태전이
                    // true 를 반환하여 ActionLink::UpdateState() 에서 상태전이
                    // 이유 : FLOW NODE 이지만 LINK 도 포함되어 있을 수 있기 때문임
                    return true;
            }

            return false;
        }
        // FLOW 가 진행 중인지 여부 확인
        public bool IsContinuousFlow()
        {
            if (m_nNextFlowNo.Equals(0))
                return false;

            return true;
        }
        /// 24.08.30 by wdw [Fix] 작업 복구를 위한 진입 시점 설정 변수 추가.
        public void BeginActionFlow(int nEntryFlowNo = 1)
        {
            m_nNextFlowNo = nEntryFlowNo;
        }
        public void EndActionFlow()
        {
            m_nNextFlowNo = 0;
        }
        public void SetNextAction()
        {
			// 2021.08.08 by junho [ADD] improve code
			if (m_sCurrentTableName.Equals(string.Empty))
				return;

            int nThisFlowNo = m_nNextFlowNo;

            FlowTable tFlowTable = m_dicTables[m_sCurrentTableName];
            FlowItem tFlowItem = tFlowTable.GetFlowItem(nThisFlowNo);

            EN_ACTION_RESULT enResult = tFlowItem.CheckPostcondition();

            m_nNextFlowNo = tFlowItem.GetPostconditionFlowNo(enResult);
        }
        #endregion

        #endregion

        #region GETTER/SETTER
		/// <summary>
		/// 2021.07.30 by twkang [ADD] Get the list of flow table
		/// </summary>
		public bool GetFlowTableList(ref string[] arTableName)
		{
			if (m_dicTables.Count > 0)
			{
				arTableName = new string[m_dicTables.Count];

				int nIndex = -1;

				foreach(string strTableName in m_dicTables.Keys)
				{
					arTableName[++nIndex]	= strTableName;
				}

				return true;
			}

			return false;
		}
		/// <summary>
		/// 2021.07.30 by twkang [ADD] Get the count of flow item
		/// </summary>
		public int GetCountOfFlowItem(string sTableName)
		{
			if (m_dicTables.ContainsKey(sTableName))
			{
				return m_dicTables[sTableName].GetCountOfFlowItem();
			}

			return 0;
		}

		/// <summary>
		/// 2021.08.05 by twkang [ADD] Get the current flow step
		/// </summary>
		public int GetCurrentFlowStep()
		{
			return m_nNextFlowNo;
		}

        /// <summary>
        /// 2022.06.30. by shkim [ADD] Set the current flow step
        /// </summary>
        public bool SetCurrentFlowStep(string sTableName, int nNextFlowNo)
        {
            if (false == m_dicTables.ContainsKey(sTableName)) return false;
            if (nNextFlowNo > m_dicTables[sTableName].GetCountOfFlowItem())
            {
                return false;
            }
            m_nNextFlowNo = nNextFlowNo;
            return true;
        }

        public string GetActionName(string sTableName, int nFlowIndex)
        {
            if (m_dicTables.ContainsKey(sTableName))
            {
                return m_dicTables[sTableName].GetActionName(nFlowIndex);
            }

            return string.Empty;
        }
        public bool SetActionName(string sTableName, int nFlowNo, string sActionName)
        {
            if (m_dicTables.ContainsKey(sTableName))
            {
                return m_dicTables[sTableName].SetActionName(nFlowNo, sActionName);
            }

            return false;
        }
        public int GetPreconditionFlowNo(string sTableName, int nFlowIndex, EN_CONDITION_RESULT enResult)
        {
            if (m_dicTables.ContainsKey(sTableName))
            {
                return m_dicTables[sTableName].GetPreconditionFlowNo(nFlowIndex, enResult);
            }

            return 0;
        }
        public bool SetPreconditionFlowNo(string sTableName, int nFlowIndex, EN_CONDITION_RESULT enResult, int nFlowNo)
        {
            if (m_dicTables.ContainsKey(sTableName))
            {
                if (m_dicTables[sTableName].SetPreconditionFlowNo(nFlowIndex, enResult, nFlowNo))
                    return true;
            }

            return false;
        }
        public int GetPostconditionFlowNo(string sTableName, int nFlowIndex, EN_ACTION_RESULT enResult)
        {
            if (m_dicTables.ContainsKey(sTableName))
            {
                return m_dicTables[sTableName].GetPostconditionFlowNo(nFlowIndex, enResult);
            }

            return 0;
        }
        public bool SetPostconditionFlowNo(string sTableName, int nFlowIndex, EN_ACTION_RESULT enResult, int nFlowNo)
        {
            if (m_dicTables.ContainsKey(sTableName))
            {
                if (m_dicTables[sTableName].SetPostconditionFlowNo(nFlowIndex, enResult, nFlowNo))
                    return true;
            }

            return false;
        }
        #endregion

        private class FlowTable
        {
            #region FIELD
            // KEY: FLOW NUMBER
            private Dictionary<int, FlowItem> m_dicItems = new Dictionary<int, FlowItem>();
            #endregion
            
            #region INTERFACE

            #region CREATE/REMOVE ITEM
            public bool AddFlowItem(int nFlowNo, ActionNode tNode)
            {
                if (m_dicItems.ContainsKey(nFlowNo).Equals(false))
                {
                    tNode.IsActionFlow = true;

                    m_dicItems[nFlowNo] = new FlowItem(nFlowNo, tNode);

                    return true;
                }

                return false;
            }
            public bool InsertFlowItem(int nFlowNo, ActionNode tNode)
            {
                if (m_dicItems.ContainsKey(nFlowNo).Equals(true))
                {
                    int nTotalItemCount = m_dicItems.Count;
                    for (; nFlowNo < nTotalItemCount; --nTotalItemCount)
                    {
                        m_dicItems[nTotalItemCount - 1].FlowNo = nTotalItemCount;
                        m_dicItems[nTotalItemCount] = m_dicItems[nTotalItemCount - 1];
                    }
                    m_dicItems.Remove(nFlowNo);
                    tNode.IsActionFlow = true;
                    m_dicItems[nFlowNo] = new FlowItem(nFlowNo, tNode);
                    return true;
                }

                return false;
            }


            // 2022.03.02 by shkim [ADD] For modifying flow item
            public bool RemoveFlowItem(int nFlowNo)
            {
                if (m_dicItems.ContainsKey(nFlowNo).Equals(true))
                {
                    if(m_dicItems.Remove(nFlowNo))
                    {
                        int nTotalItemCount = m_dicItems.Count;
                        for(;nFlowNo < nTotalItemCount; ++nFlowNo)
                        {
                            m_dicItems[nFlowNo + 1].FlowNo = nFlowNo;
                            m_dicItems[nFlowNo] = m_dicItems[nFlowNo + 1];
                            m_dicItems.Remove(nFlowNo + 1);
                        }
                        return true;
                    }
                }
                return false;
            }
            #endregion

            #region CONTROL
            public FlowItem GetFlowItem (int nFlowNumber)
            {
                return m_dicItems[nFlowNumber];
            }

			public FlowItem GetFlowItem(string sActionName)
            {
                foreach (KeyValuePair<int, FlowItem> kvp in m_dicItems)
                {
                    if (kvp.Value.ActionName == sActionName)
                    {
                        return kvp.Value;
                    }
                }

                return null;
            }
			// 2021.08.11 by junho [ADD] GetFlowItem을 List로 반환 받을 수 있도록 한다.
			public List<FlowItem> GetFlowItemList(string sActionName)
			{
				List<FlowItem> result = new List<FlowItem>();

				foreach (KeyValuePair<int, FlowItem> kvp in m_dicItems)
				{
					if (kvp.Value.ActionName == sActionName)
					{
						result.Add(kvp.Value);
					}
				}

				return result;
			}

            #endregion

            #endregion

            #region GETTER/SETTER
			/// <summary>
			/// 2021.07.30 by twkang [ADD] Get the count of flow item
			/// </summary>
			public int GetCountOfFlowItem()
			{
				return m_dicItems.Count;
			}
            public string GetActionName(int nFlowIndex)
            {
                if (m_dicItems.ContainsKey(nFlowIndex))
                {
                    return m_dicItems[nFlowIndex].ActionName;
                }

                return string.Empty;
            }
            public bool SetActionName(int nFlowNo, string sActionName)
            {
                if (m_dicItems.ContainsKey(nFlowNo))
                {
                    m_dicItems[nFlowNo].ActionName = sActionName;

                    return true;
                }

                return false;
            }
            public int GetPreconditionFlowNo(int nFlowInex, EN_CONDITION_RESULT enResult)
            {
                if (m_dicItems.ContainsKey(nFlowInex))
                {
                    return m_dicItems[nFlowInex].GetPreconditionFlowNo(enResult);
                }

                return 0;
            }
            public bool SetPreconditionFlowNo(int nFlowInex, EN_CONDITION_RESULT enResult, int nFlowNo)
            {
                if (m_dicItems.ContainsKey(nFlowInex))
                {
                    if (m_dicItems[nFlowInex].SetPreconditionFlowNo(enResult, nFlowNo))
                        return true;
                }

                return false;
            }
            public int GetPostconditionFlowNo(int nFlowInex, EN_ACTION_RESULT enResult)
            {
                if (m_dicItems.ContainsKey(nFlowInex))
                {
                    return m_dicItems[nFlowInex].GetPostconditionFlowNo(enResult);
                }

                return 0;
            }
            public bool SetPostconditionFlowNo(int nFlowInex, EN_ACTION_RESULT enResult, int nFlowNo)
            {
                if (m_dicItems.ContainsKey(nFlowInex))
                {
                    if (m_dicItems[nFlowInex].SetPostconditionFlowNo(enResult, nFlowNo))
                        return true;
                }

                return false;
            }
            #endregion
        }

        private class FlowItem
        {
            #region FIELD
            private int m_FlowNo = 0;
            private ActionNode m_ElementNode = null;

            // VALUE: FLOW NUMBER
            private Dictionary<EN_CONDITION_RESULT, int> m_dicPrecondition = new Dictionary<EN_CONDITION_RESULT, int>();
            private Dictionary<EN_ACTION_RESULT, int> m_dicPostcondition = new Dictionary<EN_ACTION_RESULT, int>();
            #endregion

            #region PROPERTY
            public int FlowNo { set { m_FlowNo = value; } get { return m_FlowNo; } }
            public EN_ACTION_STATE State { set { m_ElementNode.State = value; } get { return m_ElementNode.State; } }
            public string ActionName { set { m_ElementNode.ActionName = value; } get { return m_ElementNode.ActionName; } }
            #endregion

            #region CONSTRUCTOR
            public FlowItem(int nFlowNo, ActionNode node)
            {
                m_FlowNo = nFlowNo;
                m_ElementNode = node;
            }
            #endregion

            #region INTERFACE
            public bool EqualsFlowNo(int nFlowNo)
            {
                if (m_FlowNo.Equals(nFlowNo))
                    return true;

                return false;
            }
            public EN_CONDITION_RESULT CheckPrecondition()
            {
                return m_ElementNode.CheckFlowPrecondition();
            }
            public EN_ACTION_RESULT CheckPostcondition()
            {
                return m_ElementNode.CheckFlowPostcondition();
            }
            #endregion

            #region GETTER/SETTER
            public int GetPreconditionFlowNo(EN_CONDITION_RESULT enResult)
            {
                return m_dicPrecondition[enResult];
            }
            public bool SetPreconditionFlowNo(EN_CONDITION_RESULT enResult, int nFlowNo)
            {
                m_dicPrecondition[enResult] = nFlowNo;

                return true;
            }
            public int GetPostconditionFlowNo(EN_ACTION_RESULT enResult)
            {
                return m_dicPostcondition[enResult];
            }
            public bool SetPostconditionFlowNo(EN_ACTION_RESULT enResult, int nFlowNo)
            {
                m_dicPostcondition[enResult] = nFlowNo;

                return true;
            }
            #endregion
        }
    }
}
