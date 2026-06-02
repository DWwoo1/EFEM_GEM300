using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FrameOfSystem3.DynamicLink_
{
    #region ENUMERATION
    public enum EN_PORT_STATUS
    {
        DISABLE,    // 미사용
        UNABLE,     // 사용불가
        EMPTY,      // 자재없음
        EXIST,      // 자재있음
        WORKING,    // 작업중

        SELECTION,
        WORKING_BUT_FINISHED,

        FINISHED    // 작업완료
    }
    #endregion

    // OBJECT ROLE : TASK 의 PORT 상태와 HOLD LINK 정보 제공
    public class ActionPort
    {
        #region FIELD
        private string m_sTaskName = string.Empty;
        private string m_sPortName = string.Empty;
        private EN_PORT_STATUS m_enStatus = EN_PORT_STATUS.DISABLE;

        private bool m_bEnable = true;  // STATUS 변경 가능여부

        // HOLD LINK 로 지정된 NODE 만 독점적으로 연결할 경우 사용
        private bool m_bEnableHoldLink = false;
        private string m_sHoldLinkNode = string.Empty;
        #endregion

        #region PROPERTY
        public string TaskName { set { m_sTaskName = value; } get { return m_sTaskName; } }
        public string PortName { set { m_sPortName = value; } get { return m_sPortName; } }
        public EN_PORT_STATUS Status { set { m_enStatus = value; } get { return m_enStatus; } }
        public bool Enable { set { m_bEnable = value; } get { return m_bEnable; } }
        #endregion

        #region CONSTRUCTOR
        public ActionPort(string sTaskName, string sPortName)
        {
            m_sTaskName = sTaskName;
            m_sPortName = sPortName;
        }
        #endregion

        #region INTERFACE
        public bool IsLinked(string sTaskName, string sActionName)
        {
            bool bLinked = true;

            string sNodeLink = sTaskName + "." + sActionName;

            if (m_bEnableHoldLink)
            {
                // PORT 와 NODE 가 동일한 TASK 에 속한다면 HOLD LINK 의 영향을 받지 않는다
                if (m_sTaskName.Equals(sTaskName).Equals(false))
                {
					// 2023.04.05 by junho [MOD] hold 판단을 node단위가 아니라 task단위로 확인하도록 한다.
					//if (m_sHoldLinkNode.Equals(sNodeLink).Equals(false))
					//	bLinked = false;
					string holdTask = m_sHoldLinkNode.Split('.')[0];
					if (holdTask.Equals(sTaskName).Equals(false))
						bLinked = false;
                }
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
        #endregion

    }
}
