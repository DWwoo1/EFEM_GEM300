using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using System.IO;

using FileIOManager_;
using FileComposite_;
using Define.DefineConstant;

namespace FrameOfSystem3.Functional
{
    public class Scheduler
    {
        public delegate void delGenerateFunction();

        public struct Schedule
        {
            public int Hour;
            public delGenerateFunction delFunction;
        }

        private readonly string strFileName = "Scheduler";
        #region 싱글톤
        private Scheduler() 
        {
            m_dicOfSchedule = new Dictionary<string, Schedule>();
            m_dicOfLastGeneratedTime = new Dictionary<string, DateTime>();
        }

        private static readonly Lazy<Scheduler> lazyInstance = new Lazy<Scheduler>(() => new Scheduler());
        static public Scheduler GetInstance() 
        {
            return lazyInstance.Value; 
        }
        #endregion

        #region Field
        private bool m_bInit = false;
        Dictionary<string, Schedule> m_dicOfSchedule;
        Dictionary<string, DateTime> m_dicOfLastGeneratedTime;
        #endregion

        #region Method
        public bool Init()
        {
            LoadGeneratedTime();
            m_bInit = true;
            return true;
        }
        public void Excute()
        {
            if (m_bInit == false)
                return;

            foreach(var schedule in m_dicOfSchedule)
            {
                if(!m_dicOfLastGeneratedTime.ContainsKey(schedule.Key)
                  || CheckSchedule(schedule.Key, schedule.Value))
                  {
                      Thread SchedulerThread = new Thread(GenerateFunction);
                      SchedulerThread.Name = schedule.Key + "Scheduler";
                      SchedulerThread.Start(schedule.Value); //schedule 작업은 별도 Thread로 실행 
                      SaveGeneratedTime(schedule.Key);
                  }
            }
        }

        public void AddSchedule(string strName, Schedule schedule)
        {
            m_dicOfSchedule.Add(strName, schedule);
        }

        bool CheckSchedule(string strName, Schedule schdule)
        {
            DateTime Now = DateTime.Now;

            //지난 실행 시점에서 하루 이상 지났다면 실행
            TimeSpan LastGeneratedTimeDev = Now - m_dicOfLastGeneratedTime[strName];
            if (LastGeneratedTimeDev.Days > 0)
                return true;

            if (Now.Hour >= schdule.Hour)
            {
                if (m_dicOfLastGeneratedTime[strName].Day == Now.Day
                 && m_dicOfLastGeneratedTime[strName].Hour >= schdule.Hour)
                    return false;

                return true;
            }

            return false;
        }

        void SaveGeneratedTime(string strScheduleName)
        {
            DateTime Now = DateTime.Now;
            if (m_dicOfLastGeneratedTime.ContainsKey(strScheduleName))
                m_dicOfLastGeneratedTime[strScheduleName] = Now;
            else
                m_dicOfLastGeneratedTime.Add(strScheduleName, Now);

            FileComposite fComp = FileComposite.GetInstance();

            fComp.RemoveRoot(strFileName);
            if (!fComp.CreateRoot(strFileName))
                return;

            foreach (var GeneratedTime in m_dicOfLastGeneratedTime)
            {
                fComp.AddItem(strFileName, GeneratedTime.Key, GeneratedTime.Value.ToString());
            }

            FileIOManager fIO = FileIOManager.GetInstance();

            string sFilePath = FilePath.FILEPATH_RECOVERY;
            string sFileName = strFileName + FileFormat.FILEFORMAT_RECOVERY;
            string sFileData = string.Empty;

            if (!fComp.GetStructureAsString(strFileName, ref sFileData))
                return;

            fComp.RemoveRoot(strFileName);

            if (!fIO.Write(sFilePath, sFileName, sFileData, false, false))
                return;

            return;
        }

        private void LoadGeneratedTime()
        {
            string strType = string.Empty;

            string sFilePath = FilePath.FILEPATH_RECOVERY;
            string sFileName = strFileName + FileFormat.FILEFORMAT_RECOVERY;

            if (!System.IO.File.Exists(Path.Combine(sFilePath, sFileName)))
                return;

            FileIOManager fIO = FileIOManager.GetInstance();

            string sFileData = string.Empty;

            if (!fIO.Read(sFilePath, sFileName, ref sFileData, FileIO.TIMEOUT_READ))
                return;

            FileComposite fComp = FileComposite.GetInstance();

            string[] arData = sFileData.Split('\n');
            fComp.RemoveRoot(strFileName);

            if (!fComp.CreateRootByString(ref arData))
                return;

            string[] arItemList = null;
            fComp.GetListOfItem(strFileName, ref arItemList);

            foreach (string sItem in arItemList)
            {

                string sValue = "";
                if (fComp.GetValue(strFileName, sItem, ref sValue))
                {
                    DateTime LastGeneratedTime;

                    if(DateTime.TryParse(sValue, out LastGeneratedTime))
                    {
                        m_dicOfLastGeneratedTime.Add(sItem, LastGeneratedTime);
                    }
                }
            }
            return;
        }

        void GenerateFunction(object schedule)
        {
            if (schedule == null) return;
            if (false == schedule is Schedule) return; 
            Schedule GenerateSchedule = (Schedule)schedule;
            GenerateSchedule.delFunction();
        }
        #endregion

        #region ETC 
        //외부로 빼야함

        #endregion ETC
    }
}
