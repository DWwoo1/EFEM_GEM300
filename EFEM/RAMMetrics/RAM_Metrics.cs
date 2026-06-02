using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DesignPattern_.Observer_;
using EquipmentState_;
using FrameOfSystem3.Task;
using TickCounter_;

namespace FrameOfSystem3.EquipmentMonitor
{
    #region Enumerable
    public enum MEASURE_TIME_E10_STATE
    {
        NON_SCHEDULED_DOWNTIME = 1,
        UN_SCHEDULED_DOWNTIME  = 2,
        SCHEDULED_DOWNTIME     = 3,
        ENGINEERING_TIME       = 4,
        STANDBY_TIME           = 5,
        PRODUCTIVE_TIME        = 6,
    }
    public enum MEASURE_TIME_E10_TIME
    {
        MTBF,
        MTTR,
        MTBA,
        MTTA,
        MTTS,
    }
    #endregion

    //(RELIABILITY, AVAILABILITY, MAINTAINABILITY)Metrics
    public class RAM_Metrics : IObserver
    {
        #region Singleton
        private static RAM_Metrics _Instance = new RAM_Metrics();

        public static RAM_Metrics GetInstance()
        {
            return _Instance;
        }
        #endregion /Singleton

        #region Variables

        #region Monitoring
        private bool m_bMonitoring = false;

        private TickCounter m_tickOfProductive = new TickCounter();

        private MEASURE_TIME_E10_STATE m_enCurrentState = MEASURE_TIME_E10_STATE.NON_SCHEDULED_DOWNTIME;

        private ConcurrentQueue<string> m_queueOfWriteDescription = new ConcurrentQueue<string>();
        #endregion

        #region Measure
        // MTBF (Mean Time Between Failures) 
        // 평균 고장 간격 시간
        // Total Uptime / failure count (Alarm)
        private TimeSpan m_tsMTBF = TimeSpan.Zero;

        // MTTR (Mean Time To Repair)
        // 평균 수리 시간
        // Unscheduled Downtime / Repair Event
        private TimeSpan m_tsMTTR = TimeSpan.Zero;

        // MTTF (Mean Time To Failures) (SEMI E10 사양 아님)
        // 평균 고장 시간
        // 의미 대로 계산하면 MTBF 계산이 동일함 MTBF = MTTF + MTTR이라고 하니 MTBF – MTTR환산
        private TimeSpan m_tsMTTF = TimeSpan.Zero;

        // MTBA (Mean Time Between Assists) (SEMI E10 사양 아님)
        // 평균 지원 간격 시간
        // Total Assist time / assist count (Stop)
        private TimeSpan m_tsMTBA = TimeSpan.Zero;

        // MTBI (Mean Time Between Incidents) (SEMI E10 사양 아님)
        // 평균 사건 간격 시간
        // Total Operating time (Standby + Productive) / Incidents count (Alarm)
        private TimeSpan m_tsMTBI = TimeSpan.Zero;

        // MTTA (Mean Time To Acknowledge) (SEMI E10 사양 아님)
        // ALARM 발생 후 확인까지의 평균시간= UNSCHEDULED DOWNTIME의 평균
        // UNSCHEDULED DOWNTIME / UNSCHEDULED DOWNTIME count
        private TimeSpan m_tsMTTA = TimeSpan.Zero;

       

        private Dictionary<MEASURE_TIME_E10_STATE, int>      m_dicOfLoadedStateCount = new Dictionary<MEASURE_TIME_E10_STATE, int>();
        private Dictionary<MEASURE_TIME_E10_STATE, TimeSpan> m_dicOfLoadedStateTime  = new Dictionary<MEASURE_TIME_E10_STATE, TimeSpan>();

        private List<E10State> m_listOfLoadedState = new List<E10State>();

        private DateTime m_dtLoadStartTime = DateTime.Now;
        private DateTime m_dtLoadEndTime = DateTime.Now;
        #endregion

        #endregion

        #region Property
        public TimeSpan MTBF { get { return m_tsMTBF; } }
        public TimeSpan MTTR { get { return m_tsMTTR; } }
        public TimeSpan MTTF { get { return m_tsMTTF; } }
        public TimeSpan MTBA { get { return m_tsMTBA; } }
        public TimeSpan MTBI { get { return m_tsMTBI; } }
        public TimeSpan MTTA { get { return m_tsMTTA; } }
        
       
        public Dictionary<MEASURE_TIME_E10_STATE, int>      LoadedStateCount { get { return m_dicOfLoadedStateCount; } }
        public Dictionary<MEASURE_TIME_E10_STATE, TimeSpan> LoadedStateTime  { get { return m_dicOfLoadedStateTime; } }

        public List<E10State> LoadedStateList { get { return m_listOfLoadedState; } }
        #endregion

        #region External Interface

        #region Initialize
        public bool Init()
        {
            RegisterSubject(EquipmentState.GetInstance());

            m_bMonitoring = true;

            return true;
        }
        #endregion

        #region Execute
        public void Execute()
        {
            UpdateProductiveTime();

            DataWrite();
        }
        #endregion
      
        #region Alarm
        public void StartAlarm()
        {
            m_bMonitoring = false;

            ChangeState(MEASURE_TIME_E10_STATE.UN_SCHEDULED_DOWNTIME);
        }
        public void StopAlarm()
        {
            m_bMonitoring = true;
        }
        #endregion

        #region Load
        public bool Load(DateTime start, DateTime end)
        {
            if (EquipmentState_.EquipmentState.GetInstance().GetState() == EquipmentState_.EQUIPMENT_STATE.IDLE
                || EquipmentState_.EquipmentState.GetInstance().GetState() == EquipmentState_.EQUIPMENT_STATE.PAUSE)
            {
                m_dtLoadStartTime = start;
                m_dtLoadEndTime = end;

                ResetLoadData();

                DataLoad(start, end);

                MeasureState();

                ProcessTime();

                return true;
            }
            return false;
        }
        #endregion

        #region Export
        public void ExportLoadedData(string strExportPath)
        {
			try
			{
				string strFileName = m_dtLoadStartTime.ToString("yyyyMMddhhmmss") + "_" + m_dtLoadEndTime.ToString("yyyyMMddhhmmss") + ".csv";

				if (false == Directory.Exists(strExportPath))
				{
					Directory.CreateDirectory(strExportPath);
				}

				string strFilePath = Path.Combine(strExportPath, strFileName);
				List<string> Temps = new List<string>();


				using (FileStream fstream = new FileStream(strFilePath, FileMode.Create, FileAccess.Write))
				{
					using (StreamWriter swriter = new StreamWriter(fstream, Encoding.UTF8))
					{
						string strData;

						strData = "MTBF," + m_tsMTBF.ToString();
						swriter.WriteLine(strData);

						strData = "MTTR," + m_tsMTTR.ToString();
						swriter.WriteLine(strData);

                        strData = "MTTF," + m_tsMTTF.ToString();
                        swriter.WriteLine(strData);

						strData = "MTBA," + m_tsMTBA.ToString();
						swriter.WriteLine(strData);

						strData = "MTBI," + m_tsMTBI.ToString();
						swriter.WriteLine(strData);

                        strData = "MTTA," + m_tsMTTA.ToString();
                        swriter.WriteLine(strData);


						foreach (MEASURE_TIME_E10_STATE en in Enum.GetValues(typeof(MEASURE_TIME_E10_STATE)))
						{
							if (m_dicOfLoadedStateCount.ContainsKey(en))
							{
								strData = en + " COUNT," + m_dicOfLoadedStateCount[en].ToString();
								swriter.WriteLine(strData);
							}
							if (m_dicOfLoadedStateTime.ContainsKey(en))
							{
								strData = en + " TIME," + m_dicOfLoadedStateTime[en].ToString();
								swriter.WriteLine(strData);
							}
						}

						swriter.Close();
					}
					fstream.Close();
				}
			}
			catch(Exception e)
			{
				Console.WriteLine(e.Message);
				return;
			}
        }
        #endregion /Export
        #endregion

        #region Internal Interface
        #region Update Equipment State
        private void UpdateEquipmentState(EQUIPMENT_STATE state)
        {
            if (false == m_bMonitoring)
                return;

            switch (state)
            {
                #region UNDEFINED - Non Scheduled Downtime
                case EQUIPMENT_STATE.UNDEFINED:
                    if (m_enCurrentState == MEASURE_TIME_E10_STATE.NON_SCHEDULED_DOWNTIME)
                        return;

                    m_bMonitoring = false;

                    ChangeState(MEASURE_TIME_E10_STATE.NON_SCHEDULED_DOWNTIME);
                    break;
                #endregion

                #region PAUSE - Scheduled Downtime
                case EQUIPMENT_STATE.PAUSE:
                    if (m_enCurrentState == MEASURE_TIME_E10_STATE.SCHEDULED_DOWNTIME)
                        return;

                    ChangeState(MEASURE_TIME_E10_STATE.SCHEDULED_DOWNTIME);
                    break;
                #endregion

                #region INITIALIZE - Engineering Time
                case EQUIPMENT_STATE.INITIALIZE:
                    if (m_enCurrentState == MEASURE_TIME_E10_STATE.ENGINEERING_TIME)
                        return;

                    ChangeState(MEASURE_TIME_E10_STATE.ENGINEERING_TIME);
                    break;
                #endregion

                #region IDLE - Scheduled Downtime
                case EQUIPMENT_STATE.IDLE:
                    if (m_enCurrentState == MEASURE_TIME_E10_STATE.SCHEDULED_DOWNTIME)
                        return;

                    ChangeState(MEASURE_TIME_E10_STATE.SCHEDULED_DOWNTIME);
                    break;
                #endregion

                #region READY - Engineering Time
                case EQUIPMENT_STATE.READY:
                    if (m_enCurrentState == MEASURE_TIME_E10_STATE.ENGINEERING_TIME)
                        return;

                    ChangeState(MEASURE_TIME_E10_STATE.ENGINEERING_TIME);
                    break;
                #endregion

                #region SETUP - Engineering Time
                case EQUIPMENT_STATE.SETUP:
                    if (m_enCurrentState == MEASURE_TIME_E10_STATE.ENGINEERING_TIME)
                        return;

                    ChangeState(MEASURE_TIME_E10_STATE.ENGINEERING_TIME);
                    break;
                #endregion

                #region EXECUTING - Standby Time
                case EQUIPMENT_STATE.EXECUTING:
                    if (m_enCurrentState == MEASURE_TIME_E10_STATE.STANDBY_TIME)
                        return;

                    ChangeState(MEASURE_TIME_E10_STATE.STANDBY_TIME);
                    break;
                #endregion

                #region FINISHING - Engineering Time
                case EQUIPMENT_STATE.FINISHING:
                    if (m_enCurrentState == MEASURE_TIME_E10_STATE.ENGINEERING_TIME)
                        return;

                    ChangeState(MEASURE_TIME_E10_STATE.ENGINEERING_TIME);
                    break;
                #endregion
            }
        }
        #endregion

        #region Update Productive Time
        private void UpdateProductiveTime()
        {
            switch (EquipmentState_.EquipmentState.GetInstance().GetState())
            {
                case EquipmentState_.EQUIPMENT_STATE.EXECUTING:
                    break;
                default:
                    return;
            }

            if (false == m_tickOfProductive.IsTickOver(true))
                return;

            m_tickOfProductive.SetTickCount(1000);

            bool bWorking = false;
            foreach (Define.DefineEnumProject.Task.EN_TASK_LIST en in Enum.GetValues(typeof(Define.DefineEnumProject.Task.EN_TASK_LIST)))
            {
                RunningMain_.TaskData taskData = null;
                if (Task.TaskOperator.GetInstance().GetTaskInformation((int)en, ref taskData))
                    bWorking |= taskData.strTaskState == "RUN";
            }

            if (bWorking)
            {
                if (m_enCurrentState == MEASURE_TIME_E10_STATE.PRODUCTIVE_TIME)
                    return;

                ChangeState(MEASURE_TIME_E10_STATE.PRODUCTIVE_TIME);
            }
            else
            {
                if (m_enCurrentState == MEASURE_TIME_E10_STATE.STANDBY_TIME)
                    return;

                ChangeState(MEASURE_TIME_E10_STATE.STANDBY_TIME);
            }
        }
        #endregion

        #region Change State
        private void ChangeState(MEASURE_TIME_E10_STATE state)
        {
            string description = string.Empty;
            description += DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            description += ",";
            description += state.ToString();

            m_queueOfWriteDescription.Enqueue(description);

            m_enCurrentState = state;
        }
        #endregion

        #region Write
        private void DataWrite()
        {
            if (m_queueOfWriteDescription.IsEmpty)
                return;

            string filePath = Define.DefineConstant.FilePath.FILEPATH_RAM_MATRICS;
            string fileName = String.Format("{0:0000}{1:00}{2:00}", DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day);
            string fullFile = filePath + "\\" + fileName + Define.DefineConstant.FileFormat.FILEFORMAT_CSV;

            DirectoryInfo info = new DirectoryInfo(filePath);

            if (false == info.Exists)
                info.Create();

            using (StreamWriter writeFile = new StreamWriter(fullFile, true))
            {
                string description = string.Empty;
                while (m_queueOfWriteDescription.TryDequeue(out description))
                {
                    writeFile.WriteLine(description);
                }
            }
        }
        #endregion

        #region Load

        #region Reset Load Data
        private void ResetLoadData()
        {
            m_listOfLoadedState.Clear();
            m_dicOfLoadedStateCount.Clear();
            m_dicOfLoadedStateTime.Clear();

            foreach (MEASURE_TIME_E10_STATE state in Enum.GetValues(typeof(MEASURE_TIME_E10_STATE)))
            {
                m_dicOfLoadedStateCount.Add(state, 0);
                m_dicOfLoadedStateTime.Add(state, TimeSpan.Zero);
            }
        }
        #endregion

        #region Data Load
        private void DataLoad(DateTime start, DateTime end)
        {
            try
            {
                List<string> allDate = GetAllDateBetween(start, end);

                string filePath = Define.DefineConstant.FilePath.FILEPATH_RAM_MATRICS;
                string fileName = string.Empty;
                string fullFile = string.Empty;

                FileInfo info;

                string strPreState = MEASURE_TIME_E10_STATE.NON_SCHEDULED_DOWNTIME.ToString();
              
                foreach (string date in allDate)
                {
                    fileName = date;
                    fullFile = filePath + "\\" + fileName + Define.DefineConstant.FileFormat.FILEFORMAT_CSV;

                    info = new FileInfo(fullFile);

                    if (false == info.Exists)
                        continue;

                    string description = string.Empty;
                    string[] data = null;
                    using (StreamReader readFile = new StreamReader(fullFile))
                    {
                        while (!readFile.EndOfStream)
                        {
                            description = readFile.ReadLine();
                            data = description.Split(',');

                            DateTime time = DateTime.Parse(data[0]);

                            if (time < start)
                            {
                                strPreState = data[1];
                                continue;
                            }

                            if (time > end)
                                continue;

                            if(m_listOfLoadedState.Count == 0)
                                m_listOfLoadedState.Add(new E10State(start, strPreState));
                                   
                            m_listOfLoadedState.Add(new E10State(time, data[1]));
                        }
                    }
                }

                for (int i = 0; i < m_listOfLoadedState.Count - 1; i++)
                {
                    m_listOfLoadedState[i].UpdateEndTimeAndStateTime(m_listOfLoadedState[i + 1].StartTime);
                }
                m_listOfLoadedState[m_listOfLoadedState.Count - 1].UpdateEndTimeAndStateTime(end);
            }
            catch (Exception e)
            {
				Console.WriteLine(e.Message);
            }
        }
        #endregion

        #region Measure State
        private void MeasureState()
        {
            foreach (E10State state in m_listOfLoadedState)
            {
                m_dicOfLoadedStateCount[state.State]++;
                m_dicOfLoadedStateTime[state.State] += state.StateTime;
            }
        }
        #endregion

        #region Process Time
        private void ProcessTime()
        {
            TimeSpan totalUptime    = TimeSpan.Zero;
            TimeSpan totalDowntime  = TimeSpan.Zero;
            TimeSpan totalOperation = TimeSpan.Zero;
            TimeSpan totalAssist    = TimeSpan.Zero;
            TimeSpan totalSupport   = TimeSpan.Zero;

            int targetTime = 0;
            int targetCount = 0;
            int calculateTime = 0;

            totalUptime += m_dicOfLoadedStateTime[MEASURE_TIME_E10_STATE.PRODUCTIVE_TIME];
            totalUptime += m_dicOfLoadedStateTime[MEASURE_TIME_E10_STATE.ENGINEERING_TIME];
            totalUptime += m_dicOfLoadedStateTime[MEASURE_TIME_E10_STATE.STANDBY_TIME];

            totalDowntime += m_dicOfLoadedStateTime[MEASURE_TIME_E10_STATE.NON_SCHEDULED_DOWNTIME];
            totalDowntime += m_dicOfLoadedStateTime[MEASURE_TIME_E10_STATE.UN_SCHEDULED_DOWNTIME];
            totalDowntime += m_dicOfLoadedStateTime[MEASURE_TIME_E10_STATE.SCHEDULED_DOWNTIME];

            totalAssist += m_dicOfLoadedStateTime[MEASURE_TIME_E10_STATE.SCHEDULED_DOWNTIME];
            totalAssist += m_dicOfLoadedStateTime[MEASURE_TIME_E10_STATE.ENGINEERING_TIME];

            totalSupport += m_dicOfLoadedStateTime[MEASURE_TIME_E10_STATE.SCHEDULED_DOWNTIME];
            totalSupport += m_dicOfLoadedStateTime[MEASURE_TIME_E10_STATE.UN_SCHEDULED_DOWNTIME];
            totalSupport += m_dicOfLoadedStateTime[MEASURE_TIME_E10_STATE.ENGINEERING_TIME];

            totalOperation += totalUptime;
            totalOperation += totalDowntime;

            // MTBF (Mean Time Between Failures)
            // 평균 고장 간격 시간
            // Total Uptime / failure count (Alarm)
            targetTime    = (int)totalUptime.TotalSeconds;
            targetCount   = m_dicOfLoadedStateCount[MEASURE_TIME_E10_STATE.UN_SCHEDULED_DOWNTIME];
            calculateTime = targetTime > 0 && targetCount > 0 ? targetTime / targetCount : 0;
            m_tsMTBF      = new TimeSpan(0, 0, calculateTime);

            // MTTR (Mean Time To Repair)
            // 평균 수리 시간
            // Unscheduled Downtime / Repair Event
            targetTime = (int)m_dicOfLoadedStateTime[MEASURE_TIME_E10_STATE.UN_SCHEDULED_DOWNTIME].TotalSeconds;
            targetCount   = m_dicOfLoadedStateCount[MEASURE_TIME_E10_STATE.SCHEDULED_DOWNTIME];
            calculateTime = targetTime > 0 && targetCount > 0 ? targetTime / targetCount : 0;
            m_tsMTTR      = new TimeSpan(0, 0, calculateTime);

            // MTTF (Mean Time To Failures) (SEMI E10 사양 아님)
            // 평균 고장 시간
            // 의미 대로 계산하면 MTBF 계산이 동일함 MTBF = MTTF + MTTR이라고 하니 MTBF – MTTR환산
            m_tsMTTF = m_tsMTBF - m_tsMTTR;

            // MTBA (Mean Time Between Assists) (SEMI E10 사양 아님)
            // 평균 지원 간격 시간
            // Total Assist time / assist count (Stop)
            targetTime = (int)totalAssist.TotalSeconds;
            targetCount = m_dicOfLoadedStateCount[MEASURE_TIME_E10_STATE.SCHEDULED_DOWNTIME];
            calculateTime = targetTime > 0 && targetCount > 0 ? targetTime / targetCount : 0;
            m_tsMTBA      = new TimeSpan(0, 0, calculateTime);

            // MTBI (Mean Time Between Incidents) (SEMI E10 사양 아님)
            // 평균 사건 간격 시간
            // Total Operating time (Standby + Productive) / Incidents count (Alarm)
            targetTime    = (int)totalOperation.TotalSeconds;
            targetCount   = m_dicOfLoadedStateCount[MEASURE_TIME_E10_STATE.UN_SCHEDULED_DOWNTIME];
            calculateTime = targetTime > 0 && targetCount > 0 ? targetTime / targetCount : 0;
            m_tsMTBI      = new TimeSpan(0, 0, calculateTime);

            // MTTA (Mean Time To Acknowledge) (SEMI E10 사양 아님)
            // ALARM 발생 후 확인까지의 평균시간= UNSCHEDULED DOWNTIME의 평균
            // UNSCHEDULED DOWNTIME / UNSCHEDULED DOWNTIME count
            targetTime = (int)m_dicOfLoadedStateTime[MEASURE_TIME_E10_STATE.UN_SCHEDULED_DOWNTIME].TotalSeconds;
            targetCount = m_dicOfLoadedStateCount[MEASURE_TIME_E10_STATE.UN_SCHEDULED_DOWNTIME];
            calculateTime = targetTime > 0 && targetCount > 0 ? targetTime / targetCount : 0;
            m_tsMTTA      = new TimeSpan(0, 0, calculateTime);

        }
        #endregion

        #region Get All Date
        private List<string> GetAllDateBetween(DateTime start, DateTime end)
        {
            List<string> dateList = new List<string>();

            // Start에서 End까지 하루씩 증가시키면서 날짜를 추가합니다.
            for (DateTime date = start; date <= end; date = date.AddDays(1))
            {
                //if (date.Year == DateTime.Now.Year
                //    && date.Month == DateTime.Now.Month
                //    && date.Day == DateTime.Now.Day)
                //    continue;

                dateList.Add(date.ToString("yyyyMMdd"));
            }

            return dateList;
        }
        #endregion

        #endregion

        #endregion

        #region Inherit Interface

        #region Observer
        public void RegisterSubject(Subject pSubject)
        {
            if (pSubject is EquipmentState)
            {
                pSubject.Attach(this);
            }
        }
        public void UpdateObserver(Subject pSubject)
        {
            if (pSubject is EquipmentState)
            {
                UpdateEquipmentState(EquipmentState.GetInstance().GetState());
            }
        }
        #endregion

        #endregion
    }

    #region E10 State
    public class E10State
    {
        #region Constructor
        public E10State() { }
        public E10State(DateTime end, string state)
        {
            m_dStartTime = end;
            Enum.TryParse(state, out m_enState);
        }
        #endregion

        #region Variable
        private DateTime m_dStartTime = DateTime.MinValue;
        private DateTime m_dEndTime   = DateTime.MinValue;

        private TimeSpan m_tsStateTime = TimeSpan.Zero;

        private MEASURE_TIME_E10_STATE m_enState = MEASURE_TIME_E10_STATE.NON_SCHEDULED_DOWNTIME;
        #endregion

        #region Property
        public DateTime StartTime { get { return m_dStartTime; } }
        public DateTime EndTime   { get { return m_dEndTime; } }
        public TimeSpan StateTime { get { return m_tsStateTime; } }

        public MEASURE_TIME_E10_STATE State { get { return m_enState; } }
        #endregion

        #region External Interface

        #region Update Start Time And State Time
        public void UpdateEndTimeAndStateTime(DateTime end)
        {
            m_dEndTime = end;

            m_tsStateTime = m_dEndTime - m_dStartTime;
        }
        #endregion

        #endregion
    }
    #endregion
}
