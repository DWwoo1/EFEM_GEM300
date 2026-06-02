using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using System.Diagnostics;

//using Define.DefineEnumeration.ThreadTimer;
using FrameOfSystem3.DynamicLink_;
using FrameOfSystem3.Functional;
using FrameOfSystem3.SubSequence;
using FrameOfSystem3.Task;
using ThreadTimer_;
using DesignPattern_.Observer_;
using EquipmentState_;
using FrameOfSystem3.Views.Functional;
using RunningMain_;
//using Define.DefineEnumeration.Task;
using FrameOfSystem3.Component;
using FrameOfSystem3.EquipmentMonitor;
using FrameOfSystem3.Log;

using RecipeManager_;

namespace FrameOfSystem3.Views.Operation
{
    public partial class Operation_RAMMetrics : UserControlForMainView.CustomView
    {
        #region FIELD
        private LogManager      m_instanceLog        = LogManager.GetInstance();
        private Form_MessageBox m_instanceMessageBox = Form_MessageBox.GetInstance();

        private EquipmentMonitor.RAM_Metrics m_instanceEquipmentMonitor = EquipmentMonitor.RAM_Metrics.GetInstance();

        private int m_nSelectedTime = 0;
        #endregion

        public Operation_RAMMetrics()
        {
            InitializeComponent();
        }

        #region 상속 인터페이스
        /// <summary>
        /// 2020.06.15 by yjlee [ADD] Get the information of the task.
        /// </summary>
        protected override void ProcessWhenActivation()
        {
            SetDefaultDate();
            UpdatePathDate();
            m_dgViewTimeList.Rows.Clear();
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
        }
        #endregion

        #region Event Interface

        #region Click
        private void Click_Load(object sender, EventArgs e)
        {
            DateTime dateStart = m_datetimeStartDay.Value;
            DateTime dateEnd   = m_datetimeEndDay.Value;

            if (false == m_instanceEquipmentMonitor.Load(dateStart, dateEnd))
            {
                m_instanceMessageBox.ShowMessage("The equipment operation must be stopped in order to load.");
                return;
            }

            UpdateLoadData();
        }
        #endregion

        #region Value Changed
        private void ChangeDate(object sender, EventArgs e)
        {
            DateTime dateStart = m_datetimeStartDay.Value;
            DateTime dateEnd   = m_datetimeEndDay.Value;

            if (dateStart > dateEnd)
            {
                m_datetimeStartDay.Value = dateEnd;
            }
        }
        #endregion

        #endregion

        #region Internal Interface

        #region Default Date
        private void SetDefaultDate()
        {
            DateTime pTime = DateTime.Now;

            m_datetimeEndDay.Value   = pTime;
            m_datetimeStartDay.Value = pTime.AddDays(-1);

        }
        private void UpdatePathDate()
        {
            m_lblRawDataPath.Text = System.IO.Path.GetFullPath(Define.DefineConstant.FilePath.FILEPATH_RAM_MATRICS);
            m_lblExportPath.Text = RecipeManager.GetInstance().GetValue(RECIPE_TYPE.EQUIPMENT, FrameOfSystem3.Recipe.PARAM_EQUIPMENT.RAM_METRICS_EXPORT_PATH.ToString(), 0, RECIPE_PARAM_TYPE.VALUE, "");
        }
        #endregion

        #region Update Load Data
        private void UpdateLoadData()
        {
            m_nSelectedTime = 0;

            UpdateCountAndTotalTime();
            UpdateTimeList();
            UpdateMeasureTime();
        }
        #endregion

        #region Update Count And Total Time
        private void UpdateCountAndTotalTime()
        {
            TimeSpan allTime = TimeSpan.Zero;

            m_lblCountNonScheduled.Text =  m_instanceEquipmentMonitor.LoadedStateCount[MEASURE_TIME_E10_STATE.NON_SCHEDULED_DOWNTIME].ToString();
            m_lblTimeNonScheduled.Text  =  ConvertTimeSpan(m_instanceEquipmentMonitor.LoadedStateTime[MEASURE_TIME_E10_STATE.NON_SCHEDULED_DOWNTIME]);
            allTime                     += m_instanceEquipmentMonitor.LoadedStateTime[MEASURE_TIME_E10_STATE.NON_SCHEDULED_DOWNTIME];

            m_lblCountUnScheduled.Text =  m_instanceEquipmentMonitor.LoadedStateCount[MEASURE_TIME_E10_STATE.UN_SCHEDULED_DOWNTIME].ToString();
            m_lblTimeUnScheduled.Text  =  ConvertTimeSpan(m_instanceEquipmentMonitor.LoadedStateTime[MEASURE_TIME_E10_STATE.UN_SCHEDULED_DOWNTIME]);
            allTime                    += m_instanceEquipmentMonitor.LoadedStateTime[MEASURE_TIME_E10_STATE.UN_SCHEDULED_DOWNTIME];

            m_lblCountScheduled.Text =  m_instanceEquipmentMonitor.LoadedStateCount[MEASURE_TIME_E10_STATE.SCHEDULED_DOWNTIME].ToString();
            m_lblTimeScheduled.Text  =  ConvertTimeSpan(m_instanceEquipmentMonitor.LoadedStateTime[MEASURE_TIME_E10_STATE.SCHEDULED_DOWNTIME]);
            allTime                  += m_instanceEquipmentMonitor.LoadedStateTime[MEASURE_TIME_E10_STATE.SCHEDULED_DOWNTIME];

            m_lblCountEngineering.Text =  m_instanceEquipmentMonitor.LoadedStateCount[MEASURE_TIME_E10_STATE.ENGINEERING_TIME].ToString();
            m_lblTimeEngineering.Text  =  ConvertTimeSpan(m_instanceEquipmentMonitor.LoadedStateTime[MEASURE_TIME_E10_STATE.ENGINEERING_TIME]);
            allTime                    += m_instanceEquipmentMonitor.LoadedStateTime[MEASURE_TIME_E10_STATE.ENGINEERING_TIME];

            m_lblCountStandby.Text =  m_instanceEquipmentMonitor.LoadedStateCount[MEASURE_TIME_E10_STATE.STANDBY_TIME].ToString();
            m_lblTimeStandby.Text  =  ConvertTimeSpan(m_instanceEquipmentMonitor.LoadedStateTime[MEASURE_TIME_E10_STATE.STANDBY_TIME]);
            allTime                += m_instanceEquipmentMonitor.LoadedStateTime[MEASURE_TIME_E10_STATE.STANDBY_TIME];

            m_lblCountProductive.Text =  m_instanceEquipmentMonitor.LoadedStateCount[MEASURE_TIME_E10_STATE.PRODUCTIVE_TIME].ToString();
            m_lblTimeProductive.Text  =  ConvertTimeSpan(m_instanceEquipmentMonitor.LoadedStateTime[MEASURE_TIME_E10_STATE.PRODUCTIVE_TIME]);
            allTime                   += m_instanceEquipmentMonitor.LoadedStateTime[MEASURE_TIME_E10_STATE.PRODUCTIVE_TIME];

            m_lblCountAll.Text = m_instanceEquipmentMonitor.LoadedStateList.Count.ToString();
            m_lblTimeAll.Text  = ConvertTimeSpan(allTime);
        }
        #endregion

        #region Update Time List
        private void UpdateTimeList()
        {
            m_dgViewTimeList.Rows.Clear();

            int index = 0;
            foreach (E10State state in m_instanceEquipmentMonitor.LoadedStateList)
            {
                if (m_nSelectedTime > 0)
                {
                    if (m_nSelectedTime != (int)state.State)
                        continue;
                }

                m_dgViewTimeList.Rows.Add();

                m_dgViewTimeList.Rows[index].Cells[0].Value = (index + 1).ToString();
                m_dgViewTimeList.Rows[index].Cells[1].Value = state.State.ToString();
                m_dgViewTimeList.Rows[index].Cells[2].Value = state.StartTime.ToString("yyyy-MM-dd HH:mm:ss");
                m_dgViewTimeList.Rows[index].Cells[3].Value = state.EndTime.ToString("yyyy-MM-dd HH:mm:ss");
                m_dgViewTimeList.Rows[index].Cells[4].Value = ConvertTimeSpan(state.StateTime);

                index++;
            }
        }
        #endregion

        #region Update Measure Time
        private void UpdateMeasureTime()
        {
            m_lblMeasureMTBA.Text = ConvertTimeSpan(m_instanceEquipmentMonitor.MTBA);
            m_lblMeasureMTBF.Text = ConvertTimeSpan(m_instanceEquipmentMonitor.MTBF);
            m_lblMeasureMTBI.Text = ConvertTimeSpan(m_instanceEquipmentMonitor.MTBI);
            m_lblMeasureMTTA.Text = ConvertTimeSpan(m_instanceEquipmentMonitor.MTTA);
            m_lblMeasureMTTR.Text = ConvertTimeSpan(m_instanceEquipmentMonitor.MTTR);
            m_lblMeasureMTTF.Text = ConvertTimeSpan(m_instanceEquipmentMonitor.MTTF);
        }
        #endregion

        #region Convert Time Span
        private string ConvertTimeSpan(TimeSpan time)
        {
            return String.Format("{0:00}D {1:00}:{2:00}:{3:00}", time.Days, time.Hours, time.Minutes, time.Seconds);
        }
        #endregion

        #endregion

        #region Event Interface

        #region Time Button
        private void Click_TimeButton(object sender, EventArgs e)
        {
            Control control = sender as Control;

            if (control == null)
                return;

            m_nSelectedTime = control.TabIndex;

            UpdateTimeList();
        }
        #endregion

        private void Click_Path(object sender, EventArgs e)
        {
            Control ctr = sender as Control;
 
            switch(ctr.TabIndex)
            {
                case 0:
                    Process.Start(Define.DefineConstant.FilePath.FILEPATH_RAM_MATRICS);
                    break;

                case 1:
                    FolderBrowserDialog fDialog = new FolderBrowserDialog();
                    fDialog.SelectedPath = RecipeManager.GetInstance().GetValue(RECIPE_TYPE.EQUIPMENT, FrameOfSystem3.Recipe.PARAM_EQUIPMENT.RAM_METRICS_EXPORT_PATH.ToString(), 0, RECIPE_PARAM_TYPE.VALUE, "");
                    switch (fDialog.ShowDialog())
                    {
                        case DialogResult.OK:
                            FrameOfSystem3.Recipe.Recipe.GetInstance().SetValue(FrameOfSystem3.Recipe.EN_RECIPE_TYPE.EQUIPMENT, FrameOfSystem3.Recipe.PARAM_EQUIPMENT.RAM_METRICS_EXPORT_PATH.ToString(), 0, FrameOfSystem3.Recipe.EN_RECIPE_PARAM_TYPE.VALUE, fDialog.SelectedPath);
                            Process.Start(fDialog.SelectedPath);
                            break;
                        default:
                            return;
                    }
                    UpdatePathDate();
                    break;
            }
        }

        #endregion

        private void Click_Export(object sender, EventArgs e)
        {
			try
			{
				string strEportPath = RecipeManager.GetInstance().GetValue(RECIPE_TYPE.EQUIPMENT, FrameOfSystem3.Recipe.PARAM_EQUIPMENT.RAM_METRICS_EXPORT_PATH.ToString(), 0, RECIPE_PARAM_TYPE.VALUE, "");
				m_instanceEquipmentMonitor.ExportLoadedData(strEportPath);
				Process.Start(strEportPath);
			}
			catch(Exception ex)
			{
				Console.WriteLine(ex.Message);
				return;
			}
        }
    }
}