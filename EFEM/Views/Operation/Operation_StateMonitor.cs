using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using Define.DefineEnumBase.ThreadTimer;

using ThreadTimer_;

namespace FrameOfSystem3.Views.Operation
{
	public partial class Operation_StateMonitor : UserControlForMainView.CustomView
	{
		#region Constants
		private const int SELECT_NONE               = -1;
		private const int STARTING_INDEX            = 0;

		private const int DGVIEW_TASKSTATE_TASK					= 0;
		private const int DGVIEW_TASKSTATE_STATE				= 1;
		private const int DGVIEW_TASKSTATE_ACTION				= 2;
		private const int DGVIEW_TASKSTATE_SEQ					= 3;
		private const int DGVIEW_TASKSTATE_BSEQ					= 4;
		private const int DGVIEW_TASKSTATE_ASEQ					= 5;
		private const int DGVIEW_TASKSTATE_FLW					= 6;

		private const int DGVIEW_ACTION_ACTION					= 0;
		private const int DGVIEW_ACTION_ACTION_STATE			= 1;
		private const int DGVIEW_ACTION_TIME					= 2;

		private const int DGVIEW_PORT_TASK						= 0;
		private const int DGVIEW_PORT_NAME						= 1;
		private const int DGVIEW_PORT_STATE						= 2;

		private const int DGVIEW_LINKEDACTION_TASK				= 0;
		private const int DGVIEW_LINKEDACTION_ACTION			= 1;
		private const int DGVIEW_LINKEDACTION_ACTION_STATE		= 2;

		private const int DGVIEW_DERFACTION_TASK				= 0;
		private const int DGVIEW_DERFACTION_ACTION				= 1;
		private const int DGVIEW_DERFACTION_ACTION_STATE		= 2;
		private const int DGVIEW_DERFACTION_ACTION_CHECK		= 3;

		#endregion

		#region FIELD
		Dictionary<EN_THREADTIMER_INDEX, Sys3Controls.Sys3Label> m_dicOfIntervalLabel			= new Dictionary<EN_THREADTIMER_INDEX, Sys3Controls.Sys3Label>();
		Dictionary<EN_THREADTIMER_INDEX, Sys3Controls.Sys3Label> m_dicOfAverageIntervalLabel	= new Dictionary<EN_THREADTIMER_INDEX, Sys3Controls.Sys3Label>();
		Dictionary<EN_THREADTIMER_INDEX, Sys3Controls.Sys3Label> m_dicOfMaximumIntervalLabel	= new Dictionary<EN_THREADTIMER_INDEX, Sys3Controls.Sys3Label>();
		Dictionary<EN_THREADTIMER_INDEX, Sys3Controls.Sys3Label> m_dicOfProcessLabel			= new Dictionary<EN_THREADTIMER_INDEX,Sys3Controls.Sys3Label>();

        #region Task
        Task.TaskOperator m_instanceOperator        = null;
        int m_nCountOfTask              = 0;
        string[] m_arTaskList           = null;

        int m_nSelectedIndexOfTask      = 0;
        string m_strSelectedTask        = string.Empty;

        #region Action
		FrameOfSystem3.Config.ConfigDynamicLink m_ConfigDynamicLink = null;
		FrameOfSystem3.Config.ConfigPort m_ConfigPort				= null;

        int m_nSelectedIndexOfAction    = 0;
        string m_strSelectedAction      = string.Empty;
        #endregion

        #endregion

        #endregion

        public Operation_StateMonitor()
		{
			InitializeComponent();

			#region Initialize the variables
			m_dicOfIntervalLabel.Add(EN_THREADTIMER_INDEX.THREADTIMER_INDEX_MAIN, m_labelMainInterval);
			m_dicOfProcessLabel.Add(EN_THREADTIMER_INDEX.THREADTIMER_INDEX_MAIN, m_labelMainProcess);
			m_dicOfAverageIntervalLabel.Add(EN_THREADTIMER_INDEX.THREADTIMER_INDEX_MAIN, m_labelMainAverage);
			m_dicOfMaximumIntervalLabel.Add(EN_THREADTIMER_INDEX.THREADTIMER_INDEX_MAIN, m_labelMainMax);

			m_dicOfIntervalLabel.Add(EN_THREADTIMER_INDEX.THREADTIMER_INDEX_OBSERVER, m_labelObserverInterval);
			m_dicOfProcessLabel.Add(EN_THREADTIMER_INDEX.THREADTIMER_INDEX_OBSERVER, m_labelObserverProcess);
			m_dicOfAverageIntervalLabel.Add(EN_THREADTIMER_INDEX.THREADTIMER_INDEX_OBSERVER, m_labelObserverAverage);
			m_dicOfMaximumIntervalLabel.Add(EN_THREADTIMER_INDEX.THREADTIMER_INDEX_OBSERVER, m_labelObserverMax);

			m_dicOfIntervalLabel.Add(EN_THREADTIMER_INDEX.THREADTIMER_INDEX_FILEIO, m_labelFileIOInterval);
			m_dicOfProcessLabel.Add(EN_THREADTIMER_INDEX.THREADTIMER_INDEX_FILEIO, m_labelFileIOProcess);
			m_dicOfAverageIntervalLabel.Add(EN_THREADTIMER_INDEX.THREADTIMER_INDEX_FILEIO, m_labelFileIOAverage);
			m_dicOfMaximumIntervalLabel.Add(EN_THREADTIMER_INDEX.THREADTIMER_INDEX_FILEIO, m_labelFileIOMax);

			m_dicOfIntervalLabel.Add(EN_THREADTIMER_INDEX.THREADTIMER_INDEX_DIGITALIO, m_labelDegitalIOInterval);
			m_dicOfProcessLabel.Add(EN_THREADTIMER_INDEX.THREADTIMER_INDEX_DIGITALIO, m_labelDegitalIOProcess);
			m_dicOfAverageIntervalLabel.Add(EN_THREADTIMER_INDEX.THREADTIMER_INDEX_DIGITALIO, m_labelDegitalIOAverage);
			m_dicOfMaximumIntervalLabel.Add(EN_THREADTIMER_INDEX.THREADTIMER_INDEX_DIGITALIO, m_labelDegitalIOMax);

			m_dicOfIntervalLabel.Add(EN_THREADTIMER_INDEX.THERADTIMER_INDEX_ANALOGIO, m_labelAnalogIOInterval);
			m_dicOfProcessLabel.Add(EN_THREADTIMER_INDEX.THERADTIMER_INDEX_ANALOGIO, m_labelAnalogIOProcess);
			m_dicOfAverageIntervalLabel.Add(EN_THREADTIMER_INDEX.THERADTIMER_INDEX_ANALOGIO, m_labelAnalogIOAverage);
			m_dicOfMaximumIntervalLabel.Add(EN_THREADTIMER_INDEX.THERADTIMER_INDEX_ANALOGIO, m_labelAnalogIOMax);

			m_dicOfIntervalLabel.Add(EN_THREADTIMER_INDEX.THREADTIMER_INDEX_MOTION, m_labelMotionInterval);
			m_dicOfProcessLabel.Add(EN_THREADTIMER_INDEX.THREADTIMER_INDEX_MOTION, m_labelMotionProcess);
			m_dicOfAverageIntervalLabel.Add(EN_THREADTIMER_INDEX.THREADTIMER_INDEX_MOTION, m_labelMotionAverage);
			m_dicOfMaximumIntervalLabel.Add(EN_THREADTIMER_INDEX.THREADTIMER_INDEX_MOTION, m_labelMotionMax);

			m_dicOfIntervalLabel.Add(EN_THREADTIMER_INDEX.THREADTIMER_INDEX_MOTION_GATHERING, m_labelGatheringInterval);
			m_dicOfProcessLabel.Add(EN_THREADTIMER_INDEX.THREADTIMER_INDEX_MOTION_GATHERING, m_labelGatheringProcess);
			m_dicOfAverageIntervalLabel.Add(EN_THREADTIMER_INDEX.THREADTIMER_INDEX_MOTION_GATHERING, m_labelGatheringAverage);
			m_dicOfMaximumIntervalLabel.Add(EN_THREADTIMER_INDEX.THREADTIMER_INDEX_MOTION_GATHERING, m_labelGatheringMax);

			m_dicOfIntervalLabel.Add(EN_THREADTIMER_INDEX.THREADTIMER_INDEX_COMMUNICATION, m_labelCommunicationInterval);
			m_dicOfProcessLabel.Add(EN_THREADTIMER_INDEX.THREADTIMER_INDEX_COMMUNICATION, m_labelCommunicationProcess);
			m_dicOfAverageIntervalLabel.Add(EN_THREADTIMER_INDEX.THREADTIMER_INDEX_COMMUNICATION, m_labelCommunicationAverage);
			m_dicOfMaximumIntervalLabel.Add(EN_THREADTIMER_INDEX.THREADTIMER_INDEX_COMMUNICATION, m_labelCommunicationMax);

			m_dicOfIntervalLabel.Add(EN_THREADTIMER_INDEX.THREADTIMER_INDEX_ETC_INSTANCES, m_labelEtcInstanceInterval);
			m_dicOfProcessLabel.Add(EN_THREADTIMER_INDEX.THREADTIMER_INDEX_ETC_INSTANCES, m_labelEtcInstanceProcess);
			m_dicOfAverageIntervalLabel.Add(EN_THREADTIMER_INDEX.THREADTIMER_INDEX_ETC_INSTANCES, m_labelEtcInstanceAverage);
			m_dicOfMaximumIntervalLabel.Add(EN_THREADTIMER_INDEX.THREADTIMER_INDEX_ETC_INSTANCES, m_labelEtcInstanceMax);
			#endregion
		}

		#region 상속 인터페이스
		/// <summary>
		/// 2020.06.15 by yjlee [ADD] Get the information of the task.
		/// </summary>
		protected override void ProcessWhenActivation()
        {
            #region Task
            m_instanceOperator          = Task.TaskOperator.GetInstance();
            m_nSelectedIndexOfTask      = 0;

            if (m_instanceOperator.GetListOfTask(ref m_nCountOfTask, ref m_arTaskList))
            {
                m_dgViewTask.Rows.Clear();

                for(int nIndex = 0 ; nIndex < m_nCountOfTask ; ++ nIndex)
                {
                    m_dgViewTask.Rows.Add();

                    m_dgViewTask[DGVIEW_TASKSTATE_TASK, nIndex].Value       = m_arTaskList[nIndex];
                }

                m_strSelectedTask       = m_arTaskList[m_nSelectedIndexOfTask];
            }
            else
            {
                m_nCountOfTask      = 0;
            }
            #endregion

            #region Action
			m_ConfigDynamicLink			= FrameOfSystem3.Config.ConfigDynamicLink.GetInstance();
			m_ConfigPort				= FrameOfSystem3.Config.ConfigPort.GetInstance();

            m_nSelectedIndexOfAction = 0;
            #endregion

            UpdateActionList();
        }

		/// <summary>
		/// 
		/// </summary>
		protected override void ProcessWhenDeactivation()
		{
		}

		/// <summary>
		/// 2020.06.15 by yjlee [ADD] Update the task information and action.
		/// </summary>
		public override void CallFunctionByTimer()
        {
            #region Thread Timer
            foreach (EN_THREADTIMER_INDEX enTimer in Enum.GetValues(typeof(EN_THREADTIMER_INDEX)))
			{
				double dblThreadInterval		= 0;
				double dblProcessTime			= 0;
				double dblMaximumProcessTime	= 0;
				double dblAverageProcessTime	= 0;

				ThreadTimer.GetInstance().GetTimeInformation((int)enTimer, ref dblThreadInterval, ref dblProcessTime, ref dblMaximumProcessTime, ref dblAverageProcessTime);

				m_dicOfProcessLabel[enTimer].Text			= dblProcessTime.ToString("F3");
				m_dicOfIntervalLabel[enTimer].Text			= dblThreadInterval.ToString("F3");
				m_dicOfAverageIntervalLabel[enTimer].Text	= dblAverageProcessTime.ToString("F3");
				m_dicOfMaximumIntervalLabel[enTimer].Text	= dblMaximumProcessTime.ToString("F3");
            }
            #endregion

            #region Task
            for(int i = 0 ; i < m_nCountOfTask ; ++ i)
            {
                RunningMain_.TaskData tData     = null;

                if(m_instanceOperator.GetTaskInformation(i, ref tData))
                {
					if (i >= m_dgViewTask.Rows.Count)
						return;

                    m_dgViewTask[DGVIEW_TASKSTATE_STATE, i].Value   = tData.strTaskState;
                    m_dgViewTask[DGVIEW_TASKSTATE_SEQ, i].Value		= tData.nSequenceStepNumber;
                    m_dgViewTask[DGVIEW_TASKSTATE_BSEQ, i].Value    = tData.nBeforeErrorSequenceStepNumber;
                    m_dgViewTask[DGVIEW_TASKSTATE_ASEQ, i].Value    = tData.nAfterErrorSequenceStepNumber;
					m_dgViewTask[DGVIEW_TASKSTATE_FLW, i].Value		= m_ConfigDynamicLink.GetCurrentFlowStep(m_arTaskList[i]);

                    string strTaskAction        = tData.strRunningAction;
				    
                    // 2020.06.15 by yjlee [ADD] Get the running action of the task.
                    if(string.IsNullOrEmpty(strTaskAction))
                    {
                        strTaskAction           = Define.DefineConstant.Action.ACTION_NONE;
                    }

                    m_dgViewTask[DGVIEW_TASKSTATE_ACTION, i].Value    = strTaskAction;
                }
            }
            #endregion

            #region Action
			UpdateStateActionGridView();
			UpdateStatePortGridView();
			UpdateStateLinkedGridView();
			UpdateStateDerfActionGridView();
            #endregion

			#region ActionTime
			UpdateActionTime();
			#endregion

		}
		#endregion

        #region Internal Interface
        /// <summary>
        /// 2020.06.15 by yjlee [ADD] Update the list of the action.
        /// </summary>
        private void UpdateActionList()
        {
            m_dgViewAction.Rows.Clear();
			
            if(m_nCountOfTask < 1)
                return;
            
            string[] arAction = null;

            if(m_ConfigDynamicLink.GetListOfAction(m_strSelectedTask, ref arAction))
            {
                m_nSelectedIndexOfAction = 0;

                for(int nIndex = 0, nEnd = arAction.Length; nIndex < nEnd ; ++ nIndex)
                {
                    m_dgViewAction.Rows.Add();
                    
                    m_dgViewAction[DGVIEW_ACTION_ACTION, nIndex].Value	= arAction[nIndex];
                }

                m_strSelectedAction = arAction[m_nSelectedIndexOfAction];
            }
            else
            {
                m_nSelectedIndexOfAction = -1;
            }

			UpdatePortList();
            UpdateActionLinkAndDereference();
        }

		/// <summary>
		/// 2021.08.05 by twkang [ADD] Update the list of port
		/// </summary>
		private void UpdatePortList()
		{
			m_dgViewActionPort.Rows.Clear();

			string[] arTargetTask	= null;
			string[] arTargetPort	= null;
			int[] arGroupKey		= null;
			string[] arLinkKey		= null;
			int nItemCount			= -1;

			if(m_ConfigDynamicLink.GetPortLinkList(m_strSelectedTask, m_strSelectedAction, ref arTargetTask, ref arTargetPort, ref arGroupKey, ref arLinkKey, ref nItemCount))
			{
				for(int nIndex = 0; nIndex < nItemCount; ++nIndex)
				{
					m_dgViewActionPort.Rows.Add();

					m_dgViewActionPort[DGVIEW_PORT_TASK, nIndex].Value	= arTargetTask[nIndex];
					m_dgViewActionPort[DGVIEW_PORT_NAME, nIndex].Value	= arTargetPort[nIndex];
				}

				m_dgViewActionPort.ClearSelection();
			}
		}

		/// <summary>
		/// 2021.08.05 by twkang [ADD] Update the list of the accessed node
		/// </summary>
        private void UpdateActionLinkAndDereference()
        {
            m_dgViewLinkedAction.Rows.Clear();
            m_dgViewDerefAction.Rows.Clear();

            if (m_nSelectedIndexOfAction == -1)
                return;

            int nItemCount = 0;
            string[] arTaskName = null;
            string[] arActionName = null;
            int[] arGroupKey = null;
            string[] arLinkKey = null;

            #region NODE LINK
            if (m_ConfigDynamicLink.GetNodeLinkList(m_strSelectedTask, m_strSelectedAction, ref arTaskName, ref arActionName, ref arGroupKey, ref arLinkKey, ref nItemCount))
            {
                for(int nIndex = 0 ; nIndex < nItemCount ; ++ nIndex)
                {
                    m_dgViewLinkedAction.Rows.Add();

                    m_dgViewLinkedAction[DGVIEW_LINKEDACTION_TASK, nIndex].Value = arTaskName[nIndex];
                    m_dgViewLinkedAction[DGVIEW_LINKEDACTION_ACTION, nIndex].Value = arActionName[nIndex];
                }

                m_dgViewLinkedAction.ClearSelection();
            }
            #endregion

            #region ACCESS NODE

            if (m_ConfigDynamicLink.GetAccessNodeList(m_strSelectedTask, m_strSelectedAction, ref arTaskName, ref arActionName, ref nItemCount))
            {
                for (int nIndex = 0; nIndex < nItemCount; ++nIndex)
                {
                    m_dgViewDerefAction.Rows.Add();

                    m_dgViewDerefAction[DGVIEW_DERFACTION_TASK, nIndex].Value	= arTaskName[nIndex];
                    m_dgViewDerefAction[DGVIEW_DERFACTION_ACTION, nIndex].Value = arActionName[nIndex];
                }

                m_dgViewDerefAction.ClearSelection();
            }
            #endregion
        }

		/// <summary>
		/// 2021.08.05 by twkang [ADD] Update the state of the action grid view
		/// </summary>
		private void UpdateStateActionGridView()
		{
			for(int nIndex = 0, nEnd = m_dgViewAction.RowCount; nIndex < nEnd; ++nIndex)
			{
				string strActionName	= m_dgViewAction[DGVIEW_ACTION_ACTION, nIndex].Value.ToString();
				string strActionState	= string.Empty;

				// Action State	
				if (false == m_ConfigDynamicLink.GetActionStatus(m_strSelectedTask, strActionName, ref strActionState))
				{
					strActionState = Define.DefineConstant.Action.ACTION_NONE;
				}

				m_dgViewAction[DGVIEW_ACTION_ACTION_STATE, nIndex].Value = strActionState;
			}
		}

		/// <summary>
		/// 2021.08.05 by twkang [ADD] Update the state of the port grid veiw
		/// </summary>
		private void UpdateStatePortGridView()
		{
			for(int nIndex = 0, nEnd = m_dgViewActionPort.RowCount; nIndex < nEnd; ++nIndex)
			{
				string strTaskName		= m_dgViewActionPort[DGVIEW_PORT_TASK, nIndex].Value.ToString();
				string strPortName		= m_dgViewActionPort[DGVIEW_PORT_NAME, nIndex].Value.ToString();
				string strPortStatus	= string.Empty;

				// Action State	
				if (false == m_ConfigDynamicLink.GetPortStatus(strTaskName, strPortName, ref strPortStatus))
				{
					strPortStatus = Define.DefineConstant.Action.ACTION_NONE;
				}

				m_dgViewActionPort[DGVIEW_PORT_STATE, nIndex].Value = strPortStatus;
			}
		}

		/// <summary>
		/// 2021.08.05 by twkang [ADD] Update the state of linked node
		/// </summary>
		private void UpdateStateLinkedGridView()
		{
			for (int nIndex = 0, nEnd = m_dgViewLinkedAction.RowCount; nIndex < nEnd; ++nIndex)
			{
				string strTaskName		= m_dgViewLinkedAction[DGVIEW_LINKEDACTION_TASK, nIndex].Value.ToString();
				string strActionName	= m_dgViewLinkedAction[DGVIEW_LINKEDACTION_ACTION, nIndex].Value.ToString();
				string strActionState	= string.Empty;

				// Action State	
				if (false == m_ConfigDynamicLink.GetActionStatus(strTaskName, strActionName, ref strActionState))
				{
					strActionState = Define.DefineConstant.Action.ACTION_NONE;
				}

				m_dgViewLinkedAction[DGVIEW_LINKEDACTION_ACTION_STATE, nIndex].Value = strActionState;
			}
		}

		/// <summary>
		/// 2021.08.05 by twkang [ADD] Update the state of accessed node
		/// </summary>
		private void UpdateStateDerfActionGridView()
		{
			for (int nIndex = 0, nEnd = m_dgViewDerefAction.RowCount; nIndex < nEnd; ++nIndex)
			{
				string strTaskName		= m_dgViewDerefAction[DGVIEW_DERFACTION_TASK, nIndex].Value.ToString();
				string strActionName	= m_dgViewDerefAction[DGVIEW_DERFACTION_ACTION, nIndex].Value.ToString();
				string strActionState	= string.Empty;

				// Action State	
				if (false == m_ConfigDynamicLink.GetActionStatus(strTaskName, strActionName, ref strActionState))
				{
					strActionState = Define.DefineConstant.Action.ACTION_NONE;
				}

				m_dgViewDerefAction[DGVIEW_LINKEDACTION_ACTION_STATE, nIndex].Value = strActionState;

				bool bCheckAccessed	= m_ConfigDynamicLink.IsAccessedNodeCheck(m_strSelectedTask, m_strSelectedAction, strTaskName, strActionName);

				m_dgViewDerefAction[DGVIEW_DERFACTION_ACTION_CHECK, nIndex].Value	= bCheckAccessed.ToString().ToUpper();
			}
		}

        /// <summary>
        /// 2020.06.15 by yjlee [ADD] Update the action state in the view.
        /// </summary>
        private void UpdateState(ref Sys3Controls.Sys3DoubleBufferedDataGridView dgView)
        {
			//for(int nIndex = 0, nEnd = dgView.RowCount ; nIndex < nEnd ; ++ nIndex)
			//{
			//	string strTaskName      = dgView[0, nIndex].Value.ToString();
			//	string strActionName    = dgView[1, nIndex].Value.ToString();
			//	string strActionState   = string.Empty;

			//	// Action State	
			//	if(false == m_ConfigDynamicLink.GetActionStatus(strTaskName, strActionName, ref strActionState))
			//	{
			//		strActionState      = Define.DefineConstant.Action.ACTION_NONE;
			//	}

			//	dgView[DGVIEW_ACTION_ACTION_STATE, nIndex].Value = strActionState;
			//}
        }
		/// <summary>
		/// 2020.09.17 by twkang [ADD] Action 동작 시간을 업데이트한다.
		/// </summary>
		private void UpdateActionTime()
		{
			for (int nIndex = 0, nEnd = m_dgViewAction.RowCount; nIndex < nEnd; ++nIndex)
			{
				string strTaskName      = m_strSelectedTask;
				string strActionName    = m_dgViewAction[DGVIEW_ACTION_ACTION, nIndex].Value.ToString();
				long lInterval          = 0;

                m_instanceOperator.GetActionCycleTime(strTaskName, strActionName, ref lInterval);

				m_dgViewAction[DGVIEW_ACTION_TIME, nIndex].Value = TimeSpan.FromMilliseconds(lInterval).ToString(@"mm\:ss\.fff");
			}
		}
        #endregion

        #region GUI Event
        /// <summary>
        /// 2020.06.15 by yjlee [ADD] When the view is clicked, update the action list.
        /// </summary>
        private void Click_Task(object sender, DataGridViewCellEventArgs e)
		{
            int nIndexOfRow     = e.RowIndex;

            if (nIndexOfRow < 0 || nIndexOfRow == m_nSelectedIndexOfTask
                || nIndexOfRow >= m_dgViewTask.RowCount) { return; }
            
            m_nSelectedIndexOfTask          = nIndexOfRow;
            m_nSelectedIndexOfAction        = 0;

            m_strSelectedTask               = m_arTaskList[m_nSelectedIndexOfTask];

            UpdateActionList();
        }

        /// <summary>
        /// 2020.06.15 by yjlee [ADD] When the view is clicked, update the list and dereference list.
        /// </summary>
        private void Click_Action(object sender, DataGridViewCellEventArgs e)
        {
			if (sender != m_dgViewAction) return;

            int nIndexOfRow = e.RowIndex;

            if (nIndexOfRow < 0 || nIndexOfRow == m_nSelectedIndexOfAction
                || nIndexOfRow >= m_dgViewAction.RowCount) { return; }

            m_nSelectedIndexOfAction        = nIndexOfRow;
            m_strSelectedAction             = m_dgViewAction[DGVIEW_ACTION_ACTION, nIndexOfRow].Value.ToString();

			UpdatePortList();
            UpdateActionLinkAndDereference();
        }
        #endregion
    }
}