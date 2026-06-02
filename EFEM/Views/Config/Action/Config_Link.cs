using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace FrameOfSystem3.Views.Config.Action
{
    public partial class Config_Link : UserControlForMainView.CustomView
    {
        #region CONSTANT
        private const int SELECT_NONE               = -1;
        private const int STARTING_INDEX            = 0;

        private const int DGVIEW_TASK_INDEX         = 0;
        private const int DGVIEW_TASK_NAME          = 1;

        private const int DGVIEW_ACTION_ENABLE      = 0;
        private const int DGVIEW_ACTION_NAME        = 1;

        private const int DGVIEW_NODELINK_TASK = 0;
        private const int DGVIEW_NODELINK_NODE = 1;

        private const int DGVIEW_PORTLINK_TASK = 0;
        private const int DGVIEW_PORTLINK_NODE = 1;

        private const int DGVIEW_DEREFACTION_ENABLE     = 0;
        private const int DGVIEW_DEREFACTION_TASK       = 1;
        private const int DGVIEW_DEREFACTION_ACTION     = 2;

        private readonly Color COLOR_ENABLED            = Color.DodgerBlue;
        private readonly Color COLOR_DISABLED           = Color.White;

        private readonly string CLEAR_LABEL             = "-";

        private const int TABINDEX_ENABLE_ACTION        = 0;

        private const int TABINDEX_ENABLE_LINK          = 0;
        private const int TABINDEX_COMPARE_STATE_READY  = 1;
        private const int TABINDEX_COMPARE_STATE_WAIT   = 2;
        private const int TABINDEX_COMPARE_STATE_BUSY   = 3;

		private const int TABINDEX_ENABLE_PORT          = 0;
		private const int TABINDEX_STATUS_PORT			= 1;

        private const string GROUP_KEY_MIN              = "0";
        private const string GROUP_KEY_MAX              = "99";
        #endregion

        #region FIELD
        FrameOfSystem3.Config.ConfigDynamicLink m_ConfigDynamicLink	= null;
		Functional.Form_SelectionList m_instanceSelectionList =	null;
		Functional.Form_Calculator m_instanceCalculator	= null;
		Functional.Form_Keyboard m_instanceKeyboard	= null;

        private string m_strSelectedTask = null;
        private int m_nSelectedTask = SELECT_NONE;
        private string m_strSelectedAction = null;
        private int m_nSelectedAction = SELECT_NONE;

        private string m_strSelectedLinkedAction = null;
        private int m_nSelectedLinkedAction = SELECT_NONE;
        private int m_nSelectedLinkedActionGroupKey = 0;
        private string m_strSelectedLinkedActionLineKey = null;

        private string m_strSelectedPortLink = null;
        private int m_nSelectedPortLink = SELECT_NONE;
        private int m_nSelectedPortLinkGroupKey = 0;
        private string m_strSelectedPortLinkLinkKey = null;

        private string[] m_arListOfActionAll = null;
        #endregion

        #region CONSTRUCTOR
        public Config_Link()
        {
            InitializeComponent();
        }
        #endregion

        #region OVERRIDE
        protected override void ProcessWhenActivation()
        {
            m_ConfigDynamicLink			= FrameOfSystem3.Config.ConfigDynamicLink.GetInstance();
            m_instanceSelectionList		= Functional.Form_SelectionList.GetInstance();
            m_instanceCalculator		= Functional.Form_Calculator.GetInstance();
            m_instanceKeyboard			= Functional.Form_Keyboard.GetInstance();

            UpdateTaskList();
        }

        /// <summary>
        /// 2020.06.18 by yjlee [ADD] It will be called when the form has deactivated.
        /// </summary>
        protected override void ProcessWhenDeactivation()
        {
        }

        /// <summary>
        /// 2020.06.18 by yjlee [ADD] It will be called continuously when the form is being activated.
        /// </summary>
        public override void CallFunctionByTimer()
        {
        }
        #endregion

        #region METHOD

        #region Task
        private void UpdateTaskList()
        {
            m_dgViewTask.Rows.Clear();

            int[] arTaskIndex = null;
            string[] arTaskName = null;

            if (m_ConfigDynamicLink.GetListOfTask(ref arTaskIndex, ref arTaskName))
            {
                for(int nIndex = 0, nEnd = arTaskName.Length ; nIndex < nEnd ; ++ nIndex)
                {
                    m_dgViewTask.Rows.Add();

                    m_dgViewTask[DGVIEW_TASK_INDEX, nIndex].Value = arTaskIndex[nIndex];
                    m_dgViewTask[DGVIEW_TASK_NAME, nIndex].Value = arTaskName[nIndex];
                }

                m_nSelectedTask = STARTING_INDEX;
                m_strSelectedTask = arTaskName[STARTING_INDEX];

                MakeListOfAllAction(ref arTaskName);
            }
            else
            {
                m_nSelectedTask = SELECT_NONE;
            }

            UpdateActionList();
        }
        #endregion

        #region Action
        private void UpdateActionList()
        {
            m_dgViewAction.Rows.Clear();

            if (SELECT_NONE == m_nSelectedTask)
                return;

            string[] arActionName = null;

            if (m_ConfigDynamicLink.GetListOfAction(m_strSelectedTask, ref arActionName))
            {
                for (int nIndex = 0, nEnd = arActionName.Length ; nIndex < nEnd; ++nIndex)
                {
                    m_dgViewAction.Rows.Add();

                    m_dgViewAction[DGVIEW_ACTION_NAME, nIndex].Value = arActionName[nIndex];

                    SetEnableColorOfAction(ref nIndex, ref arActionName[nIndex]);
                }

                m_nSelectedAction = STARTING_INDEX;
                m_strSelectedAction = arActionName[STARTING_INDEX];
                DisplayActionParameter();
            }
            else
            {
                m_nSelectedAction = SELECT_NONE;
                ClearActionParameter();
            }

            UpdateNodeLink();
            UpdateAccessAction();
			UpdatePortLink();
        }
        private void UpdateNodeLink()
        {
            m_dgViewLinkedAction.Rows.Clear();

            if (SELECT_NONE == m_nSelectedAction)
			{
				ClearNodeLinkParameter();

				return;
			}

            int nItemCount = 0;
            string[] arTask = null;
            string[] arAction = null;
            int[] arGroupKey = null;
            string[] arLinkKey = null;

            if (m_ConfigDynamicLink.GetNodeLinkList(m_strSelectedTask, m_strSelectedAction, ref arTask, ref arAction, ref arGroupKey, ref arLinkKey, ref nItemCount))
            {
                for(int nIndex = 0 ; nIndex < nItemCount ; ++ nIndex)
                {
                    m_dgViewLinkedAction.Rows.Add();
                    m_dgViewLinkedAction[DGVIEW_NODELINK_TASK, nIndex].Value = arTask[nIndex];
                    m_dgViewLinkedAction[DGVIEW_NODELINK_NODE, nIndex].Value = arAction[nIndex];
                }

                m_nSelectedLinkedAction = STARTING_INDEX;

                // 2021.08.04. by shkim [MODIFY2] 링크 검증
                // 링크를 사용하지 않는 경우 대비한 예외처리
                if (arAction != null && arAction.Length > 0)
                {
                    m_strSelectedLinkedAction = arAction[STARTING_INDEX];

                    DisplayNodeLinkParameter(arTask[m_nSelectedLinkedAction], m_strSelectedAction, arAction[m_nSelectedLinkedAction], arGroupKey[m_nSelectedLinkedAction], arLinkKey[m_nSelectedLinkedAction]);
            	}
			}
            else
            {
                m_nSelectedLinkedAction = SELECT_NONE;

                ClearNodeLinkParameter();
            }
        }
        private void UpdatePortLink()
        {
            m_dgViewPortLink.Rows.Clear();

            if (SELECT_NONE == m_nSelectedAction)
			{
				ClearPortLinkParameter();

				return;
			}

            int nItemCount = 0;
            string[] arTask = null;
            string[] arPort = null;
            int[] arGroupKey = null;
            string[] arLinkKey = null;

            if (m_ConfigDynamicLink.GetPortLinkList(m_strSelectedTask, m_strSelectedAction, ref arTask, ref arPort, ref arGroupKey, ref arLinkKey, ref nItemCount))
            {
                for (int nIndex = 0; nIndex < nItemCount; ++nIndex)
                {
                    m_dgViewPortLink.Rows.Add();
                    m_dgViewPortLink[DGVIEW_PORTLINK_TASK, nIndex].Value = arTask[nIndex];
                    m_dgViewPortLink[DGVIEW_PORTLINK_NODE, nIndex].Value = arPort[nIndex];
                }

                m_nSelectedPortLink = STARTING_INDEX;

                if (arPort != null && arPort.Length > 0)
                {
                    m_strSelectedPortLink = arPort[STARTING_INDEX];

                    DisplayPortLinkParameter(arTask[m_nSelectedPortLink], arPort[m_nSelectedPortLink], arGroupKey[m_nSelectedPortLink], arLinkKey[m_nSelectedPortLink]);
                }
            }
            else
            {
                m_nSelectedPortLink = SELECT_NONE;

                ClearPortLinkParameter();
            }
        }
        private void UpdateAccessAction()
        {
            m_dgViewDereference.Rows.Clear();

            if (SELECT_NONE == m_nSelectedAction)
                return;

            string[] arTask = null;
            string[] arAction = null;
            int nItemCount = 0;

            if (m_ConfigDynamicLink.GetAccessNodeList(m_strSelectedTask, m_strSelectedAction, ref arTask, ref arAction, ref nItemCount))
            {
                for(int nIndex = 0 ; nIndex < nItemCount ; ++ nIndex)
                {
                    m_dgViewDereference.Rows.Add();

                    m_dgViewDereference[DGVIEW_DEREFACTION_TASK, nIndex].Value = arTask[nIndex];
                    m_dgViewDereference[DGVIEW_DEREFACTION_ACTION, nIndex].Value = arAction[nIndex];

                    SetEnableColorOfDerefAction(ref nIndex);
                }

                m_dgViewDereference.ClearSelection();
            }
        }
        private void MakeListOfAllAction(ref string[] arTaskName)
        {
            List<string> listOfTemp = new List<string>();

            for(int nIndex = 0, nEnd = arTaskName.Length ; nIndex < nEnd ; ++ nIndex)
            {
                string[] arAction = null;

                if (m_ConfigDynamicLink.GetListOfAction(arTaskName[nIndex], ref arAction))
                {
                    for(int nIndexOfAction = 0, nEndOfAction = arAction.Length ; nIndexOfAction < nEndOfAction ; ++ nIndexOfAction)
                    {
                        listOfTemp.Add(string.Format("{0} - {1}", arTaskName[nIndex], arAction[nIndexOfAction]));
                    }
                }
            }

            m_arListOfActionAll = listOfTemp.ToArray();
        }
        #endregion

        #region GUI
        private void SetEnableColorOfAction(ref int nIndexOfRow, ref string strActionName)
        {
            bool bEnable = m_ConfigDynamicLink.GetActionEnable(m_strSelectedTask, strActionName);

            m_dgViewAction.Rows[nIndexOfRow].Cells[DGVIEW_ACTION_ENABLE].Style.BackColor = bEnable ? COLOR_ENABLED : COLOR_DISABLED;
            m_dgViewAction.Rows[nIndexOfRow].Cells[DGVIEW_ACTION_ENABLE].Style.SelectionBackColor = bEnable ? COLOR_ENABLED : COLOR_DISABLED;
        }
        private void SetEnableColorOfDerefAction(ref int nIndexOfRow)
        {
            string strTaskName = m_dgViewDereference[DGVIEW_DEREFACTION_TASK, nIndexOfRow].Value.ToString();
            string strActionName = m_dgViewDereference[DGVIEW_DEREFACTION_ACTION, nIndexOfRow].Value.ToString();

            bool bEnable = m_ConfigDynamicLink.GetActionEnable(m_strSelectedTask, strActionName);
            
            m_dgViewDereference.Rows[nIndexOfRow].Cells[DGVIEW_ACTION_ENABLE].Style.BackColor = bEnable ? COLOR_ENABLED : COLOR_DISABLED;
            m_dgViewDereference.Rows[nIndexOfRow].Cells[DGVIEW_ACTION_ENABLE].Style.SelectionBackColor = bEnable ? COLOR_ENABLED : COLOR_DISABLED;
        }
        private void ClearActionParameter()
        {
            m_labelActionTask.Text		= CLEAR_LABEL;
            m_labelActionName.Text		= CLEAR_LABEL;
            m_labelActionEnable.Text	= CLEAR_LABEL;

            SetEanbleActionParameters(false);
        }
        private void DisplayActionParameter()
        {
            m_labelActionTask.Text = m_strSelectedTask;
            m_labelActionName.Text = m_strSelectedAction;

            m_labelActionEnable.Text = m_ConfigDynamicLink.GetActionEnable(m_strSelectedTask, m_strSelectedAction) ? Define.DefineConstant.SelectionList.ENABLE : Define.DefineConstant.SelectionList.DISABLE;

            SetEanbleActionParameters(true);
        }
        #endregion

        #region Parameter
        private void SetEanbleActionParameters(bool bEnable)
        {
            m_labelActionEnable.Enabled = bEnable;
        }
        private void ClearNodeLinkParameter()
        {
            m_labelLinkedActionTask.Text = CLEAR_LABEL;
            m_labelLinkedActionName.Text = CLEAR_LABEL;
            m_labelLinkedActionGroupKey.Text = CLEAR_LABEL;
            m_labelLinkedActionLineKey.Text = CLEAR_LABEL;

            m_labelLinkedActionEnable.Text = CLEAR_LABEL;
            m_labelCompareStateReady.Text = CLEAR_LABEL;
            m_labelCompareStateWait.Text = CLEAR_LABEL;
            m_labelCompareStateBusy.Text = CLEAR_LABEL;

            SetEanbleNodeLinkParameters(false);
        }
        private void ClearPortLinkParameter()
        {
            m_labelPortLinkTask.Text = CLEAR_LABEL;
            m_labelPortLinkPort.Text = CLEAR_LABEL;
            m_labelPortLinkGroupKey.Text = CLEAR_LABEL;
            m_labelPortLinkLinkKey.Text = CLEAR_LABEL;
            m_labelPortLinkEnable.Text = CLEAR_LABEL;
            m_labelPortLinkStatus.Text = CLEAR_LABEL;

            SetEanblePortLinkParameters(false);
        }
        private void DisplayNodeLinkParameter(string sTask, string sAction, string sNode, int nGroupKey, string sLinkKey)
        {
            bool bEnable = m_ConfigDynamicLink.GetNodeLinkEnable(sTask, sAction, nGroupKey, sLinkKey);

            m_nSelectedLinkedActionGroupKey = nGroupKey;
            m_strSelectedLinkedActionLineKey = sLinkKey;

            m_labelLinkedActionTask.Text = sTask;
            m_labelLinkedActionName.Text = sNode;
            m_labelLinkedActionGroupKey.Text = nGroupKey.ToString();
            m_labelLinkedActionLineKey.Text = sLinkKey;
            m_labelLinkedActionEnable.Text = bEnable ? Define.DefineConstant.SelectionList.ENABLE : Define.DefineConstant.SelectionList.DISABLE;

            #region STATE CONDITION
            string sStateCondition = Define.DefineConstant.Common.NONE;

            m_ConfigDynamicLink.GetStateCondition(m_strSelectedTask, m_strSelectedAction, nGroupKey, sLinkKey, m_lblCompareStateReady.SubText, ref sStateCondition);
            m_labelCompareStateReady.Text = sStateCondition;

            sStateCondition = Define.DefineConstant.Common.NONE;

            m_ConfigDynamicLink.GetStateCondition(m_strSelectedTask, m_strSelectedAction, nGroupKey, sLinkKey, m_lblCompareStateWait.SubText, ref sStateCondition);
            m_labelCompareStateWait.Text = sStateCondition;

            sStateCondition = Define.DefineConstant.Common.NONE;

            m_ConfigDynamicLink.GetStateCondition(m_strSelectedTask, m_strSelectedAction, nGroupKey, sLinkKey, m_lblCompareStateBusy.SubText, ref sStateCondition);
            m_labelCompareStateBusy.Text = sStateCondition;
            #endregion

            SetEanbleNodeLinkParameters(true);
        }
        private void SetEanbleNodeLinkParameters(bool bEnable)
        {
            m_labelLinkedActionEnable.Enabled = bEnable;
            m_labelCompareStateReady.Enabled = bEnable;
            m_labelCompareStateWait.Enabled = bEnable;
            m_labelCompareStateBusy.Enabled = bEnable;
            m_btnRemoveLink.Enabled = bEnable;
        }
        private void DisplayPortLinkParameter(string sTask, string sPort, int nGroupKey, string sLinkKey)
        {
            bool bEnable = m_ConfigDynamicLink.GetPortLinkEnable(m_strSelectedTask, m_strSelectedAction, nGroupKey, sLinkKey);

            m_nSelectedPortLinkGroupKey = nGroupKey;
            m_strSelectedPortLinkLinkKey = sLinkKey;

            m_labelPortLinkTask.Text = sTask;
            m_labelPortLinkPort.Text = sPort;
            m_labelPortLinkGroupKey.Text = nGroupKey.ToString();
            m_labelPortLinkLinkKey.Text = sLinkKey;
            m_labelPortLinkEnable.Text = bEnable ? Define.DefineConstant.SelectionList.ENABLE : Define.DefineConstant.SelectionList.DISABLE;

            string sStatusCondition = Define.DefineConstant.Common.NONE;

            m_ConfigDynamicLink.GetStatusCondition(m_strSelectedTask, m_strSelectedAction, nGroupKey, sLinkKey, ref sStatusCondition);
            m_labelPortLinkStatus.Text = sStatusCondition;

            SetEanblePortLinkParameters(true);
        }
        private void SetEanblePortLinkParameters(bool bEnable)
        {
            m_labelPortLinkEnable.Enabled = bEnable;
            m_labelPortLinkStatus.Enabled = bEnable;
            m_btnPortLinkRemove.Enabled = bEnable;
        }
        #endregion

        #endregion

        #region GUI Event
        private void Click_TaskList(object sender, DataGridViewCellEventArgs e)
        {
            int nIndexOfRow     = e.RowIndex;

            if (nIndexOfRow < 0 || nIndexOfRow >= m_dgViewTask.RowCount
                || nIndexOfRow == m_nSelectedTask) { return; }

            m_nSelectedTask         = nIndexOfRow;
            m_strSelectedTask       = m_dgViewTask[DGVIEW_TASK_NAME, m_nSelectedTask].Value.ToString();

            UpdateActionList();
        }
        private void Click_ActionList(object sender, DataGridViewCellEventArgs e)
        {
            int nIndexOfRow     = e.RowIndex;

            if (nIndexOfRow < 0 || nIndexOfRow >= m_dgViewAction.RowCount
                || nIndexOfRow == m_nSelectedAction) { return; }

            m_nSelectedAction       = nIndexOfRow;
            m_strSelectedAction     = m_dgViewAction[DGVIEW_ACTION_NAME, m_nSelectedAction].Value.ToString();

            UpdateNodeLink();

            UpdateAccessAction();

            UpdatePortLink();
			
			DisplayActionParameter();
        }
        private void Click_LinkedAction(object sender, DataGridViewCellEventArgs e)
        {
            int nIndexOfRow     = e.RowIndex;

            if (nIndexOfRow < 0 || nIndexOfRow >= m_dgViewLinkedAction.RowCount
                || nIndexOfRow == m_nSelectedLinkedAction) { return; }

            m_nSelectedLinkedAction         = nIndexOfRow;
            
            int nItemCount = 0;
            string[] arTask = null;
            string[] arAction = null;
            int[] arGroupKey = null;
            string[] arLinkKey = null;

            if (m_ConfigDynamicLink.GetNodeLinkList(m_strSelectedTask, m_strSelectedAction, ref arTask, ref arAction, ref arGroupKey, ref arLinkKey, ref nItemCount))
            {
                m_strSelectedLinkedAction = arAction[m_nSelectedLinkedAction];
                DisplayNodeLinkParameter(m_strSelectedTask, m_strSelectedAction, m_strSelectedLinkedAction, arGroupKey[m_nSelectedLinkedAction], arLinkKey[m_nSelectedLinkedAction]);
            }
        }
		private void Click_LinkedPort(object sender, DataGridViewCellEventArgs e)
		{
			int nIndexOfRow = e.RowIndex;

			if (nIndexOfRow < 0 || nIndexOfRow >= m_dgViewPortLink.RowCount
				|| nIndexOfRow == m_nSelectedPortLink) { return; }

			m_nSelectedPortLink = nIndexOfRow;

			int nItemCount = 0;
			string[] arTask = null;
			string[] arAction = null;
			int[] arGroupKey = null;
			string[] arLinkKey = null;

			if(m_ConfigDynamicLink.GetPortLinkList(m_strSelectedTask, m_strSelectedAction, ref arTask, ref arAction, ref arGroupKey, ref arLinkKey, ref nItemCount))
			{
				m_strSelectedPortLink = arAction[m_nSelectedPortLink];
				DisplayPortLinkParameter(arTask[m_nSelectedPortLink], m_strSelectedPortLink, arGroupKey[m_nSelectedPortLink], arLinkKey[m_nSelectedPortLink]);
			}
		}
        private void Click_ActionParameter(object sender, EventArgs e)
        {
            if (SELECT_NONE == m_nSelectedAction)
                return;

            Control ctr = sender as Control;

            switch(ctr.TabIndex)
            {
                case TABINDEX_ENABLE_ACTION:
                    if(m_instanceSelectionList.CreateForm(m_lblEnableAction.Text
                        , Define.DefineEnumProject.SelectionList.EN_SELECTIONLIST.ENABLE_DISABLE
                        , m_labelActionEnable.Text))
                    {
                        string strResult = null;
                        m_instanceSelectionList.GetResult(ref strResult);
                        
                        bool bEnable = strResult.Equals(Define.DefineConstant.SelectionList.ENABLE);

                        if(m_ConfigDynamicLink.SetActionEnable(m_strSelectedTask, m_strSelectedAction, bEnable))
                        {
                            m_labelActionEnable.Text = strResult;

                            SetEnableColorOfAction(ref m_nSelectedAction, ref m_strSelectedAction);
                        }
                    }
                    break;
            }
        }
        private void Click_LinkedActionParameter(object sender, EventArgs e)
        {
            if (SELECT_NONE == m_nSelectedLinkedAction)
                return;

            Control ctr = sender as Control;

            switch(ctr.TabIndex)
            {
                case TABINDEX_ENABLE_LINK:
                    if(m_instanceSelectionList.CreateForm(m_lblEnableLink.Text, Define.DefineEnumProject.SelectionList.EN_SELECTIONLIST.ENABLE_DISABLE, m_labelLinkedActionEnable.Text))
                    {
                        string strResult = null;
                        m_instanceSelectionList.GetResult(ref strResult);
                        
                        bool bEnable = strResult.Equals(Define.DefineConstant.SelectionList.ENABLE);

                        if(m_ConfigDynamicLink.SetNodeLinkEnable(m_strSelectedTask, m_strSelectedAction, m_nSelectedLinkedActionGroupKey, m_strSelectedLinkedActionLineKey, bEnable))
                        {
                            m_labelLinkedActionEnable.Text = strResult;
                        }
                    }
                    break;

                case TABINDEX_COMPARE_STATE_READY:
                    if(m_instanceSelectionList.CreateForm(m_lblCompareStateReady.Text, Define.DefineEnumProject.SelectionList.EN_SELECTIONLIST.ACTION_COMPARE_STATE, m_labelCompareStateReady.Text))
                    {
                        string strResult = null;
                        m_instanceSelectionList.GetResult(ref strResult);

                        if (m_ConfigDynamicLink.SetStateCondition(m_strSelectedTask, m_strSelectedAction, m_nSelectedLinkedActionGroupKey, m_strSelectedLinkedActionLineKey, m_lblCompareStateReady.SubText, strResult))
                        {
                            m_labelCompareStateReady.Text = strResult;
                        }
                    }
                    break;

                case TABINDEX_COMPARE_STATE_WAIT:
                    if(m_instanceSelectionList.CreateForm(m_lblCompareStateWait.Text, Define.DefineEnumProject.SelectionList.EN_SELECTIONLIST.ACTION_COMPARE_STATE, m_labelCompareStateWait.Text))
                    {
                        string strResult = null;
                        m_instanceSelectionList.GetResult(ref strResult);

                        if (m_ConfigDynamicLink.SetStateCondition(m_strSelectedTask, m_strSelectedAction, m_nSelectedLinkedActionGroupKey, m_strSelectedLinkedActionLineKey, m_lblCompareStateWait.SubText, strResult))
                        {
                            m_labelCompareStateWait.Text = strResult;
                        }
                    }
                    break;

                case TABINDEX_COMPARE_STATE_BUSY:
                    if(m_instanceSelectionList.CreateForm(m_lblCompareStateBusy.Text, Define.DefineEnumProject.SelectionList.EN_SELECTIONLIST.ACTION_COMPARE_STATE, m_labelCompareStateBusy.Text))
                    {
                        string strResult = null;
                        m_instanceSelectionList.GetResult(ref strResult);

                        if (m_ConfigDynamicLink.SetStateCondition(m_strSelectedTask, m_strSelectedAction, m_nSelectedLinkedActionGroupKey, m_strSelectedLinkedActionLineKey, m_lblCompareStateBusy.SubText, strResult))
                        {
                            m_labelCompareStateBusy.Text = strResult;
                        }
                    }
                    break;
            }
        }

		/// <summary>
		/// 2021.08.04 by twkang [ADD] Click the port link list label
		/// </summary>
		private void Click_LinkedPortParameter(object sender, EventArgs e)
		{
			if (SELECT_NONE == m_nSelectedPortLink)
				return;

			Control ctrl	= sender as Control;

			switch(ctrl.TabIndex)
			{
				case TABINDEX_ENABLE_LINK:
					if (m_instanceSelectionList.CreateForm(m_lblEnablePort.Text, Define.DefineEnumProject.SelectionList.EN_SELECTIONLIST.ENABLE_DISABLE, m_labelPortLinkEnable.Text))
					{
						string strResult = null;
						m_instanceSelectionList.GetResult(ref strResult);

						bool bEnable = strResult.Equals(Define.DefineConstant.SelectionList.ENABLE);
						
						if (m_ConfigDynamicLink.SetPortLinkEnable(m_strSelectedTask, m_strSelectedAction, m_nSelectedPortLinkGroupKey, m_strSelectedPortLinkLinkKey, bEnable))
						{
							m_labelPortLinkEnable.Text = strResult;
						}
					}
					break;
				case TABINDEX_STATUS_PORT:
					if(m_instanceSelectionList.CreateForm(m_lblPortStatus.Text, Define.DefineEnumProject.SelectionList.EN_SELECTIONLIST.PORT_STATUS, m_labelPortLinkStatus.Text))
					{
						string strResult = null;
						m_instanceSelectionList.GetResult(ref strResult);
						
						if(m_ConfigDynamicLink.SetPortLinkStatus(m_strSelectedTask, m_strSelectedAction, m_nSelectedPortLinkGroupKey, m_strSelectedPortLinkLinkKey, strResult))
						{
							m_labelPortLinkStatus.Text		= strResult;
						}
					}
					break;
			}
		}
		
        /// <summary>
        /// 2020.06.25 by yjlee [ADD] Add a link of the action.
        /// </summary>
        private void Click_AddLink(object sender, EventArgs e)
        {
            if(false == m_instanceSelectionList.CreateForm(m_btnAddLink.Text, m_arListOfActionAll, Define.DefineConstant.Common.NONE))
                return;

            // 2020.06.27 by yjlee [ADD] Parse the selected action task and name.
            //  - Data format : TASK - ACTION
            string strSelectedAction = null;
            m_instanceSelectionList.GetResult(ref strSelectedAction);

            string[] arSelectedAction = strSelectedAction.Replace(" ", "").Split('-');

            string strSelectedActionTask = arSelectedAction[0];
            string strSelectedActionName = arSelectedAction[1];

            if(false == m_instanceCalculator.CreateForm(m_lblGroupKey.Text, GROUP_KEY_MIN, GROUP_KEY_MAX))
                return;

            // 2020.06.27 by yjlee [ADD] Get the group key that user inputed.
            int nGroupKey = -1;
			m_instanceCalculator.GetResult(ref nGroupKey);

            if(false == m_instanceKeyboard.CreateForm())
                return;

            string strLineKey = null;
			m_instanceKeyboard.GetResult(ref strLineKey);

            if(m_ConfigDynamicLink.AddLink( m_strSelectedTask, m_strSelectedAction, strSelectedActionTask, strSelectedActionName, nGroupKey, strLineKey))
            {
                UpdateNodeLink();
            }
        }
        /// <summary>
        /// 2020.06.27 by yjlee [ADD] Remove the linked action.
        /// </summary>
        private void Click_RemoveLink(object sender, EventArgs e)
        {
            if (m_nSelectedLinkedAction == SELECT_NONE)
                return;

            string strTargetTask = m_labelLinkedActionTask.Text;

            if (m_ConfigDynamicLink.RemoveLink(m_strSelectedTask, m_strSelectedAction, strTargetTask, m_strSelectedLinkedAction, m_nSelectedLinkedActionGroupKey, m_strSelectedLinkedActionLineKey))
            {
                UpdateNodeLink();
            }
        }
        #endregion
    }
}