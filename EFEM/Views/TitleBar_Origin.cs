using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Reflection;
using System.Diagnostics;

using EquipmentState_;
using DesignPattern_.Observer_;
using Account_;

using FrameOfSystem3.Work;

namespace FrameOfSystem3.Views
{
    public partial class TitleBar_Origin : UserControl, IObserver
	{
		#region 상수
		// private const int GUI_INTERVAL			= 1000;
        private const int GUI_INTERVAL = 500;

		#endregion

		#region 변수
		private Timer m_TimerForUpdateGUI		= new Timer();
        private FrameOfSystem3.Recipe.Recipe m_instanceRecipe = FrameOfSystem3.Recipe.Recipe.GetInstance();
        #endregion

        #region 속성
        #endregion

        public TitleBar_Origin()
        {
            InitializeComponent();

			InitializeInstances();
		}

		#region Internal Interface
		/// <summary>
		/// 
		/// </summary>
		private void InitializeInstances()
		{
			//m_labelMachineName.Text = Assembly.GetEntryAssembly().GetName().Name;
			m_labelMachineName.Text = AppConfigControl.GetValue(
				Define.DefineEnumProject.AppConfig.KEY.MachineName.ToString(), "NONE");
			
			m_TimerForUpdateGUI.Interval = GUI_INTERVAL;
			m_TimerForUpdateGUI.Tick += new EventHandler(FunctionForTimerTick);
			m_TimerForUpdateGUI.Start();

			FileVersionInfo fv		= FileVersionInfo.GetVersionInfo(Assembly.GetExecutingAssembly().Location);

			lblVersion.Text = fv.FileVersion.ToString();

			// 2022.06.08 by junho [ADD] 고유 어셈블리 버전 정보를 타이틀바 버전에 툴팁으로 표시하도록 기능 추가
			var assemblyVersion = System.Reflection.Assembly.GetExecutingAssembly().GetName().Version;
			var fullVersion = string.Format("{0}.{1}.{2}", lblVersion.Text, assemblyVersion.Build.ToString(), assemblyVersion.Revision.ToString());
			this.tt_FullVersion.ToolTipTitle = "Full Version";
			this.tt_FullVersion.SetToolTip(this.lblVersion, fullVersion);
		}

		/// <summary>
		/// 2020.06.01 by twkang [ADD] 설비의 상태가 변하면 호출되는 함수이다.
		/// </summary>
		private void SetMachineState(string strState)
		{
			m_labelMachineState.Text		= strState;
		}
		/// <summary>
		/// 2020.08.25 by twkang [ADD] 계정 권한이 변하면 호출되는 함수이다.
		/// </summary>
		private void SetUserAccount(string strUserID, string strAuthority)
		{
			m_labelUserID.Text				= strUserID;
			m_labelUserAuthority.Text		= strAuthority;
		}
		/// <summary>
		/// 2020.06.01 by twkang [ADD] 타이머 처리를 위한 함수, UI를 갱신한다.
		/// </summary>
		private void FunctionForTimerTick(object sender, EventArgs e)
		{
			var nowTime = System.DateTime.Now;
			m_labelTimer.Text = nowTime.ToString("yyyy-MM-dd HH:mm:ss");
			
            string strFilePath = string.Empty;
            string strFileName = string.Empty;
            m_instanceRecipe.GetProcessFileInformation(ref strFilePath, ref strFileName);

            m_labelRecipeName.Text = System.IO.Path.GetFileNameWithoutExtension(strFileName);

			// 2022.12.23 by junho [ADD] 매 정각마다 중요 file들 backup 기능 추가
			if (_fileCopiedHour != nowTime.Hour)
			{
				_fileCopiedHour = nowTime.Hour;
				FrameOfSystem3.Functional.FunctionsETC.ImportantFileBackup();
			}
		}
		private int _fileCopiedHour = 0;
		#endregion

		#region Observer Interface
		EquipmentState m_subjectEquipmentState		= null;
		Account_.Account m_subjectAccount			= null;

		/// <summary>
		/// 2020.06.01 by twkang [ADD] 감시자 주체를 등록한다.
		/// </summary>
		public void RegisterSubject(Subject pSubject)
		{
			if(pSubject is EquipmentState)
			{
				m_subjectEquipmentState		= pSubject as EquipmentState;

				SetMachineState(m_subjectEquipmentState.GetState().ToString());
			}
			
			else if(pSubject is Account_.Account)
			{
				m_subjectAccount			= pSubject as Account_.Account;

				SetUserAccount(m_subjectAccount.GetLoginedID(), m_subjectAccount.GetLoginedAuthority().ToString());
			}


			pSubject.Attach(this);
		}
		/// <summary>
		/// 2020.06.01 by twkang [ADD] 감시자를 업데이트 한다.
		/// </summary>
		public void UpdateObserver(Subject pSubject)
		{
			if(pSubject is EquipmentState)
			{
				SetMachineState(m_subjectEquipmentState.GetState().ToString());
			}
			else if(pSubject is Account_.Account)
			{
				SetUserAccount(m_subjectAccount.GetLoginedID(), m_subjectAccount.GetLoginedAuthority().ToString());
			}
		}
		#endregion
	}
}
