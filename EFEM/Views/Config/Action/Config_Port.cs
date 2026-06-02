using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using FrameOfSystem3.DynamicLink_;

namespace FrameOfSystem3.Views.Config.Action
{
    //extern alias AlarmInstance;

    public partial class Config_Port : UserControlForMainView.CustomView
    {
        #region CONSTANT
        private const int SELECT_NONE           = -1;
        private const int STARTING_INDEX        = 0;

		private readonly Color COLOR_ENABLED            = Color.DodgerBlue;
		private readonly Color COLOR_DISABLED           = Color.White;

		private readonly string DEFAULT_LABEL		= "--";

        #region Column Index
        private const int DGVIEW_TASK_INDEX			= 0;
        private const int DGVIEW_TASK_NAME			= 1;

		private const int DGVIEW_PORTLIST_INDEX		= 0;
		private const int DGVIEW_PORTLIST_ENABLE	= 1;
		private const int DGVIEW_PORTLIST_NAME		= 2;
		private const int DGVIEW_PORTLIST_STATUS	= 3;

        #endregion

		#region Tab Index
		private const int TABINDEX_BUTTON_ENABLE	= 0;
		private const int TABINDEX_BUTTON_DISABLE	= 1;

		private const int TABINDEX_LABEL_STATUS		= 0;
		#endregion

		#endregion

		#region FIELD

		#region Instance
		FrameOfSystem3.Config.ConfigDynamicLink m_ConfigDynamicLink		= null;
		FrameOfSystem3.Config.ConfigPort m_ConfigPort					= null;
		Functional.Form_SelectionList m_instanceSelectionList			= null;
		#endregion

        private int m_nSelectedTask             = SELECT_NONE;
        private string m_strSelectedTask        = string.Empty;

		private int m_nSelectedPort				= SELECT_NONE;
		private string m_strSelectedPort		= string.Empty;
        #endregion

        #region CONSTRUCTOR
        public Config_Port()
        {
            InitializeComponent();

			#region TabIndex
			m_btnDisable.TabIndex	= TABINDEX_BUTTON_DISABLE;
			m_btnEnable.TabIndex	= TABINDEX_BUTTON_ENABLE;

			m_labelStatus.TabIndex	= TABINDEX_LABEL_STATUS;
			#endregion
		}
        #endregion

        #region OVERRIDE
        protected override void ProcessWhenActivation()
        {
            m_ConfigDynamicLink			= FrameOfSystem3.Config.ConfigDynamicLink.GetInstance();
			m_ConfigPort				= FrameOfSystem3.Config.ConfigPort.GetInstance();
            m_instanceSelectionList		= Functional.Form_SelectionList.GetInstance();

            UpdateTaskList();
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
			PeriodicFunctionForUpdate();
        }
        #endregion

        #region Internal Interface
		/// <summary>
		/// 2021.08.04 by twkang [ADD] Update grid view of the task list
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

			UpdatePortList();
        }

		/// <summary>
		/// 2021.09.13 by twkang [ADD] Update grid view of port list.
		/// </summary>
		private void UpdatePortList()
		{
			m_dgViewPortList.Rows.Clear();

			if (SELECT_NONE == m_nSelectedTask)
				return;

			string[] arPortList = null;

			if(m_ConfigPort.GetPortList(m_strSelectedTask, ref arPortList))
			{
				for (int nIndex = 0, nEnd = arPortList.Length; nIndex < nEnd; ++nIndex)
				{
					string strStatus	= string.Empty;

					m_dgViewPortList.Rows.Add();
					
					SetEnableColorOfPort(nIndex, m_ConfigPort.GetPortEnable(m_strSelectedTask, arPortList[nIndex]));

					m_dgViewPortList[DGVIEW_PORTLIST_INDEX, nIndex].Value	= nIndex;

					m_dgViewPortList[DGVIEW_PORTLIST_NAME, nIndex].Value	= arPortList[nIndex];

					m_ConfigPort.GetPortStatus(m_strSelectedTask, arPortList[nIndex] ,ref strStatus);

					m_dgViewPortList[DGVIEW_PORTLIST_STATUS, nIndex].Value	= strStatus;
				}

				m_nSelectedPort		= STARTING_INDEX;
                if (arPortList.Length > 0)
				m_strSelectedPort	= arPortList[STARTING_INDEX];
			}
			else
			{
				m_nSelectedPort		= SELECT_NONE;
				m_strSelectedPort	= string.Empty;

				ResetPortLabel();

				ActivatePortLabelControls(false);
			}

			UpdateSelectedItem(m_nSelectedPort);
		}

		#region GUI
		/// <summary>
		/// 2021.08.05 by twkang [ADD] Set the color of action gridview cell
		/// </summary>
		private void SetEnableColorOfPort(int nIndexOfRow, bool bEnable)
		{
			m_dgViewPortList.Rows[nIndexOfRow].Cells[DGVIEW_PORTLIST_ENABLE].Style.BackColor			= bEnable ? COLOR_ENABLED : COLOR_DISABLED;
			m_dgViewPortList.Rows[nIndexOfRow].Cells[DGVIEW_PORTLIST_ENABLE].Style.SelectionBackColor	= bEnable ? COLOR_ENABLED : COLOR_DISABLED;
		}
		
		/// <summary>
		/// 2021.08.05 by twkang [ADD] Set the enable property of port label
		/// </summary>
		private void ActivatePortLabelControls(bool bEnable)
		{
			if(EquipmentState_.EquipmentState.GetInstance().GetState() == EquipmentState_.EQUIPMENT_STATE.EXECUTING)
			{
				m_labelStatus.Enabled	= false;
				m_btnDisable.Enabled	= false;
				m_btnEnable.Enabled		= false;
			}
			else
			{
				m_labelStatus.Enabled		= bEnable;
				m_btnDisable.Enabled		= bEnable;
				m_btnEnable.Enabled			= !bEnable;
			}
		}

		/// <summary>
		/// 2021.08.05 by twkang [ADD] Display port label data
		/// </summary>
		private void UpdateSelectedItem(int nRowIndex)
		{
			if(nRowIndex == SELECT_NONE || m_nSelectedTask == SELECT_NONE || m_nSelectedPort == SELECT_NONE)
				return ;

			m_labelTaskName.Text	= m_strSelectedTask;
			m_labelPortName.Text	= m_strSelectedPort;
            if (m_dgViewPortList.Rows.Count > 0)
            {
                m_labelStatus.Text = m_dgViewPortList[DGVIEW_PORTLIST_STATUS, nRowIndex].Value.ToString();

                bool bEnable = m_dgViewPortList[DGVIEW_PORTLIST_ENABLE, nRowIndex].Style.BackColor == COLOR_ENABLED;

                ActivatePortLabelControls(bEnable);
            }
		}
		/// <summary>
		/// 2021.08.05 by twkang [ADD] Reset Port label data
		/// </summary>
		private void ResetPortLabel()
		{
			m_labelPortName.Text	= DEFAULT_LABEL;
			m_labelTaskName.Text	= DEFAULT_LABEL;
			m_labelStatus.Text		= DEFAULT_LABEL;
		}
		/// <summary>
		/// 2021.09.14 by twkang [ADD] 주기함수
		/// </summary>
		private void PeriodicFunctionForUpdate()
		{
			if(m_nSelectedTask == SELECT_NONE)
				return;

			string[] arPortList	= null;
			
			if(false == m_ConfigPort.GetPortList(m_strSelectedTask, ref arPortList))
				return;

			for(int nIndex = 0, nEnd = arPortList.Length; nIndex < nEnd; ++nIndex)
			{
				string strStatus	= string.Empty;

				SetEnableColorOfPort(nIndex, m_ConfigPort.GetPortEnable(m_strSelectedTask, arPortList[nIndex]));

				m_dgViewPortList[DGVIEW_PORTLIST_INDEX, nIndex].Value = nIndex;

				m_dgViewPortList[DGVIEW_PORTLIST_NAME, nIndex].Value = arPortList[nIndex];

				m_ConfigPort.GetPortStatus(m_strSelectedTask, arPortList[nIndex], ref strStatus);

				m_dgViewPortList[DGVIEW_PORTLIST_STATUS, nIndex].Value = strStatus;

				if(nIndex == m_nSelectedPort)
				{
					UpdateSelectedItem(m_nSelectedPort);
				}
			}
		}
		#endregion

		#endregion

		#region GUI Event
		/// <summary>
		/// 2021.08.05 by twkang [ADD] Task data grid view Click event
		/// </summary>
		private void Click_Task(object sender, DataGridViewCellEventArgs e)
		{
			int nRowIndex       = e.RowIndex;

			if (nRowIndex < 0 || nRowIndex >= m_dgViewTask.RowCount
				|| nRowIndex == m_nSelectedTask) { return; }

			m_nSelectedTask = nRowIndex;
			m_strSelectedTask = m_dgViewTask[DGVIEW_TASK_NAME, nRowIndex].Value.ToString();

			UpdatePortList();
		}
		/// <summary>
		/// 2021.08.05 by twkang [ADD] Port data grid view click event
		/// </summary>
		private void Click_PortList(object sender, DataGridViewCellEventArgs e)
		{
			int nIndexOfRow     = e.RowIndex;

			if (nIndexOfRow < 0 || nIndexOfRow >= m_dgViewPortList.RowCount
				|| nIndexOfRow == m_nSelectedPort) { return; }

			m_nSelectedPort		= nIndexOfRow;
			m_strSelectedPort	= m_dgViewPortList[DGVIEW_PORTLIST_NAME, nIndexOfRow].Value.ToString();

			UpdateSelectedItem(nIndexOfRow);
		}
		/// <summary>
		/// 2021.09.14 by twkang [ADD] 사용 미사용 버튼 클릭 이벤트
		/// </summary>
		private void Click_EnableDisable(object sender, EventArgs e)
		{
			if(m_nSelectedPort == SELECT_NONE || m_nSelectedTask == SELECT_NONE)
				return;

			Control ctrl	= sender as Control;

			bool bEnable	= false;

			switch(ctrl.TabIndex)
			{
				case TABINDEX_BUTTON_ENABLE:

					bEnable	= true;
					
					break;
				case TABINDEX_BUTTON_DISABLE:

					bEnable	 = false;

					break;
			}

			m_ConfigPort.SetPortEnable(m_strSelectedTask, m_strSelectedPort, bEnable);

			ActivatePortLabelControls(bEnable);
		}
		/// <summary>
		/// 2021.09.14 by twkang [ADD] 라벨 클릭 이벤트
		/// </summary>
		private void Click_LabelItem(object sender, EventArgs e)
		{
			if (m_nSelectedPort == SELECT_NONE || m_nSelectedTask == SELECT_NONE)
				return;

			Control ctrl	= sender as Control;

			switch(ctrl.TabIndex)
			{
				case TABINDEX_LABEL_STATUS:
					if(m_instanceSelectionList.CreateForm(Define.DefineEnumProject.SelectionList.EN_SELECTIONLIST.PORT_STATUS.ToString()
						, Define.DefineEnumProject.SelectionList.EN_SELECTIONLIST.PORT_STATUS
						, m_labelStatus.Text))
					{
						string strResult	= string.Empty;

						m_instanceSelectionList.GetResult(ref strResult);

						m_ConfigPort.SetPortStatus(m_strSelectedTask, m_strSelectedPort, strResult);
					}
					break;
			}

			UpdateSelectedItem(m_nSelectedPort);
		}
        #endregion
    }
}
