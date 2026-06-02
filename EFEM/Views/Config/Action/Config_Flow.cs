using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using TaskAction_;

using FrameOfSystem3.DynamicLink_;

namespace FrameOfSystem3.Views.Config.Action
{
    extern alias AlarmInstance;

    public partial class Config_Flow : UserControlForMainView.CustomView
    {
        #region CONSTANT
        private const int SELECT_NONE = -1;
        private const int STARTING_INDEX = 0;

        private readonly Color COLOR_DISABLE = Color.White;
        private readonly Color COLOR_ENABLE = Color.DodgerBlue;

        #region Column Index
        private const int DGVIEW_TASK_INDEX = 0;
        private const int DGVIEW_TASK_NAME = 1;

        private const int DGVIEW_TABLE_INDEX = 0;
        private const int DGVIEW_TABLE_NAME = 1;

        private const int DGVIEW_ACTION_ENABLE = 0;
        private const int DGVIEW_ACTION_NAME = 1;

        private const int DGVIEW_FLOW_INDEX = 0;
        private const int DGVIEW_FLOW_ACTION = 1;
        private const int DGVIEW_FLOW_PRE_OK = 2;
        private const int DGVIEW_FLOW_PRE_NG = 3;
        private const int DGVIEW_FLOW_PRE_ESCAPE = 4;
        private const int DGVIEW_FLOW_POST_SUCCESS = 5;
        private const int DGVIEW_FLOW_POST_FAIL = 6;
        private const int DGVIEW_FLOW_POST_INVALID = 7;
        #endregion

        #endregion

        #region FIELD

		#region Instance
		FrameOfSystem3.Config.ConfigDynamicLink m_ConfigDynamicLink	= null;
		FrameOfSystem3.Config.ConfigFlow m_instanceFlow	= null;

		Functional.Form_Calculator m_instanceCalculator = null;
		Functional.Form_SelectionList m_instanceSelectionList = null;
		#endregion

        private int m_nSelectedTask             = SELECT_NONE;
        private string m_strSelectedTask        = string.Empty;
		
		private int m_nSelectedTable			= SELECT_NONE;
		private string m_strSelectedTable		= string.Empty;

		private string[] m_arTableName			= null;
        private string[] m_arActionName         = null;

        private int m_nSelectedFlow             = SELECT_NONE;

        private Dictionary<EN_CONDITION_RESULT, int> m_mappingForPreconditionType			= new Dictionary<EN_CONDITION_RESULT,int>();
        private Dictionary<EN_ACTION_RESULT, int> m_mappingForPostconditionType					= new Dictionary<EN_ACTION_RESULT,int>();
        private Dictionary<int, EN_CONDITION_RESULT> m_mappingForPreconditionColumnIndex		= new Dictionary<int,EN_CONDITION_RESULT>();
        private Dictionary<int, EN_ACTION_RESULT> m_mappingForPostconditionColumnIndex			= new Dictionary<int,EN_ACTION_RESULT>();
        #endregion

        #region CONSTRUCTOR
        public Config_Flow()
        {
            InitializeComponent();

            InitializeVariables();
        }
        #endregion

        #region OVERRIDE
        protected override void ProcessWhenActivation()
        {
            m_ConfigDynamicLink		= FrameOfSystem3.Config.ConfigDynamicLink.GetInstance();
            m_instanceFlow			= FrameOfSystem3.Config.ConfigFlow.GetInstance();

            m_instanceCalculator	= Functional.Form_Calculator.GetInstance();
            m_instanceSelectionList = Functional.Form_SelectionList.GetInstance();

            UpdateTaskList();
            UpdateActionList();
        }

        /// <summary>
        /// 2020.06.19 by yjlee [ADD] It will be called when the form has deactivated.
        /// </summary>
        protected override void ProcessWhenDeactivation()
        {
        }

        /// <summary>
        /// 2020.06.19 by yjlee [ADD] It will be called continuously when the form is being activated.
        /// </summary>
        public override void CallFunctionByTimer()
        {
        }
        #endregion

        #region METHOD
        /// <summary>
        /// 2020.06.22 by yjlee [ADD] Make the mapping tables for the type.
        /// </summary>
        private void InitializeVariables()
        {
            #region Make mapping tables
            m_mappingForPreconditionType.Add(EN_CONDITION_RESULT.OK, DGVIEW_FLOW_PRE_OK);
            m_mappingForPreconditionType.Add(EN_CONDITION_RESULT.NG, DGVIEW_FLOW_PRE_NG);
            m_mappingForPreconditionType.Add(EN_CONDITION_RESULT.ESCAPE, DGVIEW_FLOW_PRE_ESCAPE);
            m_mappingForPreconditionColumnIndex.Add(DGVIEW_FLOW_PRE_OK, EN_CONDITION_RESULT.OK);
            m_mappingForPreconditionColumnIndex.Add(DGVIEW_FLOW_PRE_NG, EN_CONDITION_RESULT.NG);
            m_mappingForPreconditionColumnIndex.Add(DGVIEW_FLOW_PRE_ESCAPE, EN_CONDITION_RESULT.ESCAPE);

            m_mappingForPostconditionType.Add(EN_ACTION_RESULT.SUCCESS, DGVIEW_FLOW_POST_SUCCESS);
            m_mappingForPostconditionType.Add(EN_ACTION_RESULT.FAILURE, DGVIEW_FLOW_POST_FAIL);
            m_mappingForPostconditionType.Add(EN_ACTION_RESULT.INVALID, DGVIEW_FLOW_POST_INVALID);
            m_mappingForPostconditionColumnIndex.Add(DGVIEW_FLOW_POST_SUCCESS, EN_ACTION_RESULT.SUCCESS);
            m_mappingForPostconditionColumnIndex.Add(DGVIEW_FLOW_POST_FAIL, EN_ACTION_RESULT.FAILURE);
            m_mappingForPostconditionColumnIndex.Add(DGVIEW_FLOW_POST_INVALID, EN_ACTION_RESULT.INVALID);
            #endregion
        }
		/// <summary>
		/// 2021.08.05 by twkang [ADD] Update the list of the task
		/// </summary>
        private void UpdateTaskList()
        {
            m_dgViewTask.Rows.Clear();

            int[] arIndex = null;
            string[] arList = null;

            if(m_ConfigDynamicLink.GetListOfTask(ref arIndex, ref arList))
            {
                for(int nIndex = 0, nEnd = arIndex.Length ; nIndex < nEnd ; ++ nIndex)
                {
                    m_dgViewTask.Rows.Add();

                    m_dgViewTask[DGVIEW_TASK_INDEX, nIndex].Value = arIndex[nIndex];
                    m_dgViewTask[DGVIEW_TASK_NAME, nIndex].Value = arList[nIndex];
                }

                m_nSelectedTask = STARTING_INDEX;
                m_strSelectedTask = arList[STARTING_INDEX];
            }
            else
            {
                m_nSelectedTask = SELECT_NONE;
            }

			UpdateTableList();
            UpdateFlowList();
        }

		/// <summary>
		/// 2021.07.30 by twkang [ADD] Update the list of the flow table
		/// </summary>
		private void UpdateTableList()
		{
			m_dgViewFlowTable.Rows.Clear();

			if( SELECT_NONE == m_nSelectedTask) { return; }
			
			if(m_instanceFlow.GetListOfFlowTable(m_strSelectedTask, ref m_arTableName))
			{
				for(int nIndex = 0, nEnd = m_arTableName.Length; nIndex < nEnd; ++nIndex)
				{
					m_dgViewFlowTable.Rows.Add();
					
					m_dgViewFlowTable[DGVIEW_TABLE_INDEX, nIndex].Value		= nIndex;
					m_dgViewFlowTable[DGVIEW_TABLE_NAME, nIndex].Value		= m_arTableName[nIndex];
				}

				m_nSelectedTable = STARTING_INDEX;
				m_strSelectedTable = m_arTableName[STARTING_INDEX];
			}
		}
		/// <summary>
		/// 2021.08.05 by twkang [ADD] Update the list of the action
		/// </summary>
        private void UpdateActionList()
        {
            m_dgViewAction.Rows.Clear();

            if( SELECT_NONE == m_nSelectedTask)
                return; 

            if(m_ConfigDynamicLink.GetListOfAction(m_strSelectedTask, ref m_arActionName))
            {
                for(int nIndex = 0, nEnd = m_arActionName.Length ; nIndex < nEnd ; ++ nIndex)
                {
                    m_dgViewAction.Rows.Add();

                    m_dgViewAction[DGVIEW_ACTION_NAME, nIndex].Value = m_arActionName[nIndex];
                    
                    bool bEnable = m_ConfigDynamicLink.GetActionEnable(m_strSelectedTask, m_arActionName[nIndex]);

                    m_dgViewAction.Rows[nIndex].Cells[DGVIEW_ACTION_ENABLE].Style.BackColor = bEnable ? COLOR_ENABLE : COLOR_DISABLE;
                }

                m_dgViewAction.ClearSelection();
            }
        }

        /// <summary>
        /// 2020.06.19 by yjlee [ADD] Update the data grid view for the flow.
        /// </summary>
        private void UpdateFlowList()
        {
            m_dgViewFlow.Rows.Clear();

            if (SELECT_NONE == m_nSelectedTask || SELECT_NONE == m_nSelectedTable)
                return;

            int nCountOfItem = m_instanceFlow.GetCountOfFlowItem(m_strSelectedTask, m_strSelectedTable);

            for(int i = 0 ; i < nCountOfItem ; ++ i)
            {
                AddFlowItem();
            }

            m_nSelectedFlow = nCountOfItem > 0 ? STARTING_INDEX : SELECT_NONE;
        }
        private void AddFlowItem()
        {
            int nIndexOfRow = m_dgViewFlow.RowCount;

            m_dgViewFlow.Rows.Add();

            m_dgViewFlow[DGVIEW_FLOW_INDEX, nIndexOfRow].Value = nIndexOfRow;
			
            string sActionName = Define.DefineConstant.Common.NONE;

			m_instanceFlow.GetActionNameOfFlowItem(m_strSelectedTask, m_strSelectedTable, nIndexOfRow, ref sActionName);

            m_dgViewFlow[DGVIEW_FLOW_ACTION, nIndexOfRow].Value = sActionName;

            foreach(var kvp in m_mappingForPreconditionType)
            {
                m_dgViewFlow[kvp.Value, nIndexOfRow].Value = m_instanceFlow.GetPreconditionFlowNo(m_strSelectedTask, m_strSelectedTable, nIndexOfRow, kvp.Key);
            }

            foreach(var kvp in m_mappingForPostconditionType)
            {
                m_dgViewFlow[kvp.Value, nIndexOfRow].Value = m_instanceFlow.GetPostconditionFlowNo(m_strSelectedTask, m_strSelectedTable, nIndexOfRow, kvp.Key);
            }
        }
        #endregion

        #region GUI Event
        /// <summary>
        /// 2020.06.19 by yjlee [ADD] Click the list in the task.
        /// </summary>
        private void Click_Task(object sender, DataGridViewCellEventArgs e)
        {
            int nRowIndex       = e.RowIndex;

            if(nRowIndex < 0 || nRowIndex >= m_dgViewTask.RowCount
                || nRowIndex == m_nSelectedTask) { return;}

            m_nSelectedTask     = nRowIndex;
            m_strSelectedTask   = m_dgViewTask[DGVIEW_TASK_NAME, nRowIndex].Value.ToString();

            UpdateActionList();
			UpdateTableList();
            UpdateFlowList();
        }
        /// <summary>
        /// 2022.03.02 by shkim [MOD] Fixed for new dynamic link
        /// 2020.06.22 by yjlee [ADD] Click the cell in the flow list.
        /// </summary>
        private void Click_Cell(object sender, DataGridViewCellEventArgs e)
		{
            int nRowIndex = e.RowIndex;

            if (nRowIndex < 0 || nRowIndex >= m_dgViewFlow.RowCount) { return; }

            m_nSelectedFlow = nRowIndex;
		}
        /// <summary>
        /// 2022.03.02 by shkim [MOD] Fixed for new dynamic link
        /// 2020.06.22 by yjlee [ADD] Double click the cell in the flow list.
        /// </summary>
        private void DoubleClick_Cell(object sender, DataGridViewCellEventArgs e)
		{
			int nRowIndex       = e.RowIndex;
            int nColumnIndex    = e.ColumnIndex;

            if (nRowIndex < 0 || nRowIndex >= m_dgViewFlow.RowCount
                || nColumnIndex < 1) { return; }

            switch (nColumnIndex)
            {
                case DGVIEW_FLOW_ACTION:
                    return;

                case DGVIEW_FLOW_PRE_OK:
                case DGVIEW_FLOW_PRE_NG:
                case DGVIEW_FLOW_PRE_ESCAPE:
                    if (m_instanceCalculator.CreateForm(m_dgViewFlow[nColumnIndex, nRowIndex].Value.ToString()
                        , "0"
                        , (m_dgViewFlow.RowCount + 1).ToString()))
                    {
                        int nResult = -1;
                        m_instanceCalculator.GetResult(ref nResult);

                        m_instanceFlow.SavePreconditionTargetNo(m_strSelectedTask, m_strSelectedTable, m_nSelectedFlow, (EN_CONDITION_RESULT)(nColumnIndex - DGVIEW_FLOW_PRE_OK), nResult);
                    }
                    break;
                case DGVIEW_FLOW_POST_SUCCESS:
                case DGVIEW_FLOW_POST_FAIL:
                case DGVIEW_FLOW_POST_INVALID:
                    if (m_instanceCalculator.CreateForm(m_dgViewFlow[nColumnIndex, nRowIndex].Value.ToString()
                        , "0"
                        , (m_dgViewFlow.RowCount + 1).ToString()))
                    {
                        int nResult = -1;
                        m_instanceCalculator.GetResult(ref nResult);

                        m_instanceFlow.SavePostconditionTargetNo(m_strSelectedTask, m_strSelectedTable, m_nSelectedFlow, (EN_ACTION_RESULT)(nColumnIndex - DGVIEW_FLOW_POST_SUCCESS), nResult);
                    }
                    break;
            }
            UpdateFlowList();
		}
        /// <summary>
        /// 2022.03.02 by shkim [MOD] Fixed for new dynamic link
        /// 2020.06.22 by yjlee [ADD] Add a flow item on the last index.
        /// </summary>
        private void Click_Add(object sender, EventArgs e)
		{
             Functional.Form_SelectionList formActionSelect = Functional.Form_SelectionList.GetInstance();
 
             string[] arrAction = null;
             if (false == m_ConfigDynamicLink.GetListOfAction(m_strSelectedTask, ref arrAction))
                 return;
 
             if (false == formActionSelect.CreateForm("FLOW ACTION 선택", arrAction, ""))
             {
                 return;
             }
             string strSelectedAction = string.Empty;
             
             formActionSelect.GetResult(ref strSelectedAction);

             m_instanceFlow.AddFlowItem(m_strSelectedTask, m_strSelectedTable, strSelectedAction);

             UpdateFlowList();
		}

        /// <summary>
        /// 2022.03.02 by shkim [MOD] Fixed for new dynamic link
        /// 2020.06.25 by yjlee [ADD] Insert a flow item on the selected index.
        /// </summary>
        private void Click_Insert(object sender, EventArgs e)
		{
            Functional.Form_SelectionList formActionSelect = Functional.Form_SelectionList.GetInstance();
 
             string[] arrAction = null;
             if (false == m_ConfigDynamicLink.GetListOfAction(m_strSelectedTask, ref arrAction))
                 return;
 
             if (false == formActionSelect.CreateForm("FLOW ACTION 선택", arrAction, ""))
             {
                 return;
             }
             string strSelectedAction = string.Empty;
             formActionSelect.GetResult(ref strSelectedAction);

             m_instanceFlow.InsertFlowItem(m_strSelectedTask, m_strSelectedTable, strSelectedAction, m_nSelectedFlow);

             UpdateFlowList();
		}

        /// <summary>
        /// 2022.03.02 by shkim [MOD] Fixed for new dynamic link
        /// 2020.06.25 by yjlee [ADD] Remove the selecte flow item from the list.
        /// </summary>
        private void Click_Remove(object sender, EventArgs e)
		{
            if(m_instanceFlow.RemoveFlowItem(m_strSelectedTask, m_strSelectedTable, m_nSelectedFlow))
            {
                UpdateFlowList();
            }
		}
		/// <summary>
		/// 2021.07.30 by twkang [ADD] Flow Table 클릭 이벤트
		/// </summary>
		private void Click_Table(object sender, DataGridViewCellEventArgs e)
		{
			int nRowIndex       = e.RowIndex;
			int nColumnIndex    = e.ColumnIndex;

            if (nRowIndex < 0 || nRowIndex >= m_dgViewFlowTable.RowCount || nColumnIndex < 1)
                return;

			m_nSelectedTable	= nRowIndex;
			m_strSelectedTable	= m_arTableName[nRowIndex];

			UpdateFlowList();
		}
        #endregion
    }
}
