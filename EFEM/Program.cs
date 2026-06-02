using System;
using System.Windows.Forms;

using System.Threading;
using System.Runtime.ExceptionServices;

using Define.DefineConstant;
using System.Net.Sockets;
using System.Reflection;
using System.Security.Principal;
using System.Diagnostics;

namespace FrameOfSystem3
{
	static class Program
	{
		public static string AssemblyName { get; private set; }
		public static bool IsAdministratorMode { get; private set;}

		[STAThread]
		static void Main(string[] args)
		{
			bool bNotRunning = false;
			AssemblyName = System.Reflection.Assembly.GetEntryAssembly().GetName().Name;
			Mutex mutexApp = new Mutex(true, AssemblyName, out bNotRunning);

			if (false == bNotRunning)
			{
				MessageBox.Show("This software is already running.");
				Application.Exit();
				return;
			}

			// 2025.02.28. by shkim. [ADD] MongoDB 서비스가 다운되는 증상이 있어,
			// 서비스 재실행을 위해 프로그램을 강제로 관리자 권한으로 실행하도록 함
			// (프로그램 업데이트, 단축아이콘 변경 등으로 발생할 수 있는 휴먼미스 예방)
			// 관리자 권한 재실행이 무한히 반복되는 것을 예방하기 위해 재시작시 매개변수 전달을 통해 프로그램 시작
			if (false == IsAdministrator() && false == IsRestartConditionForAdminstrator(args) && false == System.Diagnostics.Debugger.IsAttached)
			{
				MessageBox.Show("관리자 권한이 아니라 재시도 중");

				try
				{
					ProcessStartInfo procInfo = new ProcessStartInfo();
					procInfo.UseShellExecute = true;
					procInfo.FileName = Application.ExecutablePath;
					procInfo.WorkingDirectory = Environment.CurrentDirectory;
					procInfo.Verb = "runas";    // runas : 관리자 권한으로 실행
					SetRestartArgument(ref args);
					procInfo.Arguments = string.Join(" ", args); // 전달된 매개변수를 다시 전달
					
					mutexApp.ReleaseMutex();	// 프로세스 재실행이 빠르면, Mutex에 제한될 수 있어 해제 후 시작
					Process.Start(procInfo);

					Application.Exit();
				}
				catch (Exception ex)
				{
					// 프로세스 재실행 중 예외 발생 시 메시지 박스 표시
					MessageBox.Show(ex.Message);
					return;
				}
			}
			else
			{
				if (bNotRunning)
				{
					IsAdministratorMode = IsAdministrator();

					if(false == IsAdministratorMode && true == System.Diagnostics.Debugger.IsAttached)
					{
						MessageBox.Show("Visual Studio가 관리자권한으로 실행되지 않았습니다.\nVisual Studio를 관리자 권한으로 재실행하세요.");
						return;
					}

					// MessageBox.Show($"{(IsAdministratorMode ? "관리자모드 실행 중" : "일반권한 실행 중")}");

					if (false == IsAdministratorMode && true == IsRestartConditionForAdminstrator(args))
					{
						MessageBox.Show("관리자 실행을 실패하여 일반 권한으로 프로그램이 실행됩니다.");
					}

					// 2024.6.15 by wdw [ADD] 실행시 file들 backup 기능 추가
					Functional.FunctionsETC.ImportantFileBackup();
					// 1. Make a directory for the exception logs.
					MakeDefaultDirectory();

					// 2. Register the exceptions
					Application.ThreadException += new ThreadExceptionEventHandler(WriteLogOnThreadException);
					AppDomain.CurrentDomain.FirstChanceException += new EventHandler<FirstChanceExceptionEventArgs>(WriteLogOnOccurredException);

					// 3. Init taskoperator.
					Task.TaskOperator.GetInstance().Init();

					// 4. Run Main Form
					Application.EnableVisualStyles();
					Application.SetCompatibleTextRenderingDefault(false);
					Application.Run(new Views.Form_Frame());

					// 5. Exit taskoperator.
					Task.TaskOperator.GetInstance().Exit();

					mutexApp.ReleaseMutex();
				}
				else
				{
					MessageBox.Show("This software is already running.");
					Application.Exit();
				}
			}
		}
		static void SetRestartArgument(ref string[] args)
		{
			if (args == null)
			{
				args = new string[] { "runas" };
			}
			else
			{
				Array.Resize(ref args, args.Length + 1);
				args[args.Length - 1] = "runas";
			}
		}
		static bool IsRestartConditionForAdminstrator(string[] args)
		{
			return args != null && args.Length >= 1 && true == args[args.Length - 1].Equals("runas");
		}
		public static bool IsAdministrator()
		{
			WindowsIdentity identity = WindowsIdentity.GetCurrent();

			if (identity != null)
			{
				WindowsPrincipal principal = new WindowsPrincipal(identity);
				return principal.IsInRole(WindowsBuiltInRole.Administrator);
			}
			return false;
		}

		#region Exceptions
		/// <summary>
		/// 2020.02.05 by yjlee [ADD] This function will be called when the exception on thread occur.
		/// </summary>
		private static void WriteLogOnThreadException(object sender, ThreadExceptionEventArgs e)
		{
			string strFileName = "ThreadException_" + GetCurrentDate() + ".txt";

			WriteLog(strFileName
				, e.Exception.ToString());

		}

		/// <summary>
		/// 2020.02.05 by yjlee [ADD] This function will be called when all exception occur.
		/// </summary>
		private static void WriteLogOnOccurredException(object source, FirstChanceExceptionEventArgs e)
		{
			var ex = e.Exception.InnerException;
			if (ex == null)
            {
				ex = e.Exception;
            }

            if (ex is System.ServiceModel.CommunicationObjectFaultedException ||
                ex is System.ServiceModel.CommunicationException ||
                ex is System.ServiceModel.EndpointNotFoundException ||
                ex is SocketException)
            {
                return;
            }

            //if (ex.InnerException != null)
            //{
            //    if (ex.InnerException is System.ServiceModel.CommunicationObjectFaultedException ||
            //        ex.InnerException is System.ServiceModel.CommunicationException ||
            //        ex.InnerException is System.ServiceModel.EndpointNotFoundException)
            //    {
            //        return;
            //    }
            //}

            var oce = e.Exception as OperationCanceledException;
            if (oce != null)
            {
                var ct = oce.CancellationToken;

                // 정상 취소로 판단되면 로그 제외
                if (ct.CanBeCanceled && ct.IsCancellationRequested)
                    return;
            }

            string strFileName = "Exception_" + GetCurrentDate() + ".txt";
			
			WriteLog(strFileName
                , string.Format("### OCCURRED EXCEPTION : {0}\r\n{1}"
                , e.Exception.Message
                , Environment.StackTrace));
        }
        #endregion

        #region Write Logs
        private static ReaderWriterLockSlim m_rwLockForLogging      = new ReaderWriterLockSlim();

        /// <summary>
        /// 2020.02.05 by yjlee [ADD] Write a log for the exceptions.
        /// </summary>
        private static void WriteLog(string strFileName, string strData)
        {
            m_rwLockForLogging.EnterWriteLock();
			
            try
            {
                using(System.IO.StreamWriter outputFile = new System.IO.StreamWriter(FilePath.FILEPATH_EXCEPTION_LOG
                    + "\\" + strFileName
                    , true))
                {
                    outputFile.WriteLine("######### OCCURRUNCE TIME : " + GetCurrentTime());
                    outputFile.WriteLine(strData + "\r\n");
                }
            }
            finally
            {
                m_rwLockForLogging.ExitWriteLock();
            }
        }
        #endregion

        #region Directory
        /// <summary>
        /// 2020.02.05 by yjlee [ADD] Make a default directory for the exception log.
        /// </summary>
        private static void MakeDefaultDirectory()
        {
            System.IO.DirectoryInfo dirInfor = new System.IO.DirectoryInfo(FilePath.FILEPATH_EXCEPTION_LOG);

            // 해당 경로의 폴더가 존재하지 않을 경우
            if(false == dirInfor.Exists)
            {
                dirInfor.Create();
            }

            // 2024.06.19. by shkim. [ADD] 임시코드. Log\Main 없으면 Exception 발생 (Sys3Log에서 처리했으면 삭제할 것)
            dirInfor = new System.IO.DirectoryInfo(FilePath.FILEPATH_LOG_MAIN);
            if (false == dirInfor.Exists)
            {
                dirInfor.Create();
            }
            // 2024.06.19. by shkim. [END]
        }
        #endregion

        #region Time
        /// <summary>
        /// 2020.02.05 by yjlee [ADD] Get a current date as string type.
        /// </summary>
        private static string GetCurrentDate()
        {
            DateTime nowDate    = System.DateTime.Now;

            return string.Format("{0:0000}{1:00}{2:00}"
                , nowDate.Year
                , nowDate.Month
                , nowDate.Day);
        }
        /// <summary>
        /// 2020.02.05 by yjlee [ADD] Get a current time as string type.
        /// </summary>
        private static string GetCurrentTime()
        {
            DateTime nowDate    = System.DateTime.Now;

            return string.Format("{0:00}h {1:00}m {2:00}s {3:000}ms"
                , nowDate.Hour
                , nowDate.Minute
                , nowDate.Second
                , nowDate.Millisecond);
        }
        #endregion
    }
}

