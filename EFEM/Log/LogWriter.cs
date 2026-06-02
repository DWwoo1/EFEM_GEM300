using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections.Concurrent;
using System.Threading;
using System.Threading.Tasks;
using System.IO;

using Define.DefineEnumProject;
using Define.DefineEnumBase.Log;
using Define.DefineConstant;

namespace FrameOfSystem3.Log
{
    public struct LogMsg
    {
        public DateTime dateTime;
        public string fileName;
        public string msg;
    }
    public class LogWriter
    {
        static private LogWriter _Instance = null;

        #region 싱글톤
        private LogWriter() 
        {
			System.Windows.Forms.Application.ApplicationExit += Deactivate;
		}
		public static LogWriter GetInstance()
        {
            if (_Instance == null)
            {
                _Instance = new LogWriter();
			}
			return _Instance;
        }
        #endregion

		#region 변수
		// True : 초기상태 신호 있음, False : 초기상태 신호 없음.
        AutoResetEvent m_WaitHandle = new AutoResetEvent(false);
        private bool m_bConfirmReq;
        public bool ConfirmReq
        {
            get
            {
                return m_bConfirmReq;
            }
            set
            {
                m_bConfirmReq = value;
                if(value)
                {
                    m_WaitHandle.Set();
                }
            }
        }
        public int LogCount
        {
            get
            {
                if (logQueue == null) return 0;
                return logQueue.Count;
            }
        }
        
        private Thread m_thread = null;

        ConcurrentQueue<LogMsg> logQueue = new ConcurrentQueue<LogMsg>();
        // 2019.4.5 by shkim [MOD] 리소스 관리 위해 수정(사이즈 초기화).
        static StringBuilder sb = new StringBuilder(1023);
        bool bRequestExit = false;
		#endregion

		public void Activate()
        {
            // 2025.03.18. by shkim. [ADD] 프로그램스타트 로그 추가
            // (어느 시점부터 기록 안되고 있었음)
            MakeLog("ProgramStartEnd\\PROGRAM", "START");
            // 2025.03.18. by shkim. [END]

            m_thread = new Thread(WriteLogMsg);
            m_thread.IsBackground = true;
            m_thread.Name = "LogWriterThread";
            m_thread.Priority = ThreadPriority.BelowNormal;
            m_thread.Start();
        }
        private void Deactivate(object sender, EventArgs e)
        {
			bRequestExit = true;

			MakeLog("ProgramStartEnd\\PROGRAM", "END");
            while(ConfirmReq)
            {
                System.Windows.Forms.Application.DoEvents();
                Thread.Sleep(1);
            }
            if(m_thread != null)
            {
                m_thread.Join();
                m_thread = null;
            }
        }
        private void EnqueueLog(LogMsg _logMsg)
        {
            logQueue.Enqueue(_logMsg);
            ConfirmReq = true;
        }

        private void WriteLogMsg()
        {
            while (true)
            {
                m_WaitHandle.WaitOne();
                if (ConfirmReq)
                {
                    LogMsg newLog;
                    if (logQueue.TryDequeue(out newLog))
                    {
                        WriteLog(newLog);
                    }
                    if (logQueue.TryPeek(out newLog)) ConfirmReq = true;
                    else ConfirmReq = false;
                    Thread.Sleep(10);
                }

				if (false == ConfirmReq && bRequestExit)
					break;
            }
        }
        private static void WriteLog(LogMsg _logMsg)
        {
            try
            {
                sb.Clear();
                if (false == Directory.Exists(Path.GetDirectoryName(_logMsg.fileName)))
                {
                    Directory.CreateDirectory(Path.GetDirectoryName(_logMsg.fileName));
                }

                using (StreamWriter sw = new StreamWriter(_logMsg.fileName, true))
                {    
                    sb.AppendLine(_logMsg.msg);
                    sw.Write(sb.ToString());
                }
            }
            catch
            {
                Console.WriteLine("LogWriterException발생!");
            }
        }

        #region 외부함수 - 외부에서 Log Queue에 Enqueue만 해준다.

        #region <VisionLog>
		// 2024.11.14. by shkkim. [MOD] 로그 시간 기록 버그 수정 (Second->Millisecond)
        public void RequestVisionLog(string strMsg)
        {
            LogMsg tempLogMsg;
            try
            {
                tempLogMsg.dateTime = DateTime.Now;
                tempLogMsg.fileName = string.Format(@"{0}\VISION_LOG\{1,0000:d4}{2,00:d2}{3,00:d2}_{4,00:d2}.txt", Define.DefineConstant.FilePath.FILEPATH_LOG, tempLogMsg.dateTime.Year, tempLogMsg.dateTime.Month, tempLogMsg.dateTime.Day, tempLogMsg.dateTime.Hour);
                tempLogMsg.msg = string.Format("{0,0000:d4}{1,00:d2}{2,00:d2}_{3,00:d2}:{4,00:d2}:{5,00:d2}.{6,000:d3} {7}",
                    tempLogMsg.dateTime.Year, tempLogMsg.dateTime.Month, tempLogMsg.dateTime.Day, tempLogMsg.dateTime.Hour, tempLogMsg.dateTime.Minute, tempLogMsg.dateTime.Second, tempLogMsg.dateTime.Millisecond, strMsg);
                EnqueueLog(tempLogMsg);
            }
            catch
            {

            }
        }
        public static string MakeVisionSendLog(string sendData)
        {
            return string.Format("[SEND_T]\t{0}", sendData);
        }
        public static string MakeVisionSocketReceiveLog(string receiveData)
        {
            return string.Format("[RECV] {0}", receiveData);
        }
        public static string MakeVisionReceiveLog(string receiveData)
        {
            return string.Format("[RECV_T]\t{0}", receiveData);
        }
        #endregion </VisionLog>

        public void RequestCutomLog(string filePath, string logData)
        {
            try
            {
                LogMsg tempLogMsg;
                try
                {
                    tempLogMsg.dateTime = DateTime.Now;
                    tempLogMsg.fileName = filePath;
                    tempLogMsg.msg = logData;
                    EnqueueLog(tempLogMsg);
                }
                catch
                {

                }
            }
            catch
            {

            }
        }
		public void MakeLog(string strFileName, string strMsg)
        {
            LogMsg tempLogMsg;
			string sFormat = ".log";

            try
            {
                tempLogMsg.dateTime = DateTime.Now;
				tempLogMsg.fileName = String.Format("{0}\\{1}\\{2}-{3}{4,00:d2}{5,00:d2}{6}", FilePath.FILEPATH_LOG, "LogWriter", strFileName, tempLogMsg.dateTime.Year, tempLogMsg.dateTime.Month, tempLogMsg.dateTime.Day, sFormat);
				tempLogMsg.msg = String.Format("[{0,00:d2}:{1,00:d2}:{2,00:d2}:{3,000:d3}]\t{4}", tempLogMsg.dateTime.Hour, tempLogMsg.dateTime.Minute, tempLogMsg.dateTime.Second, tempLogMsg.dateTime.Millisecond, strMsg);
                EnqueueLog(tempLogMsg);
            }
            catch
            {

            }
        }
		/// <summary>
		/// 2023.08.14 by junho [ADD] 동시에 접근할 수도 있으므로 csv file은 별도의 interface를 사용하도록 개선
		/// </summary>
		public void MakeCsvLog(string fileName, string message)
		{
			LogMsg tempLogMsg;
			string extension = ".csv";

			try
			{
				tempLogMsg.dateTime = DateTime.Now;
				tempLogMsg.fileName = String.Format("{0}\\{1}{2}", FilePath.FILEPATH_LOG, fileName, extension);
				tempLogMsg.msg = String.Format(message);
				EnqueueLog(tempLogMsg);
			}
			catch
			{

			}
		}
        #endregion
    }
}
