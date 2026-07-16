using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using System.Diagnostics;

using Account_;

using Define.DefineConstant;
using Define.DefineEnumProject.ButtonEvent;
using Define.DefineEnumBase.ButtonEvent;
using System.Runtime.InteropServices;
using EquipmentState_;

namespace FrameOfSystem3.Views
{
    public partial class Form_Frame : Form
    {
        #region 32 bit API
        [DllImport("user32.dll", EntryPoint = "SetWindowPos")]
        public static extern IntPtr SetWindowPos(IntPtr hWnd, int hWndInsertAfter, int x, int Y, int cx, int cy, int wFlags);

        /// <summary>
        /// 2021.01.13 by yjlee [ADD] Enumerate the special flags for the setting the position of the windows.
        /// </summary>
        private enum WINPOS_SPFLAG
        {
            HWND_TOP        = 0,
            HWND_BOTTOM     = 1,
            HWND_TOPMOST    = -1,
            HWND_NOTOPMOST  = -2,
        }
        /// <summary>
        /// 2021.01.13 by yjlee [ADD] Enumerate the flags for the setting the position of the windows.
        /// </summary>
        private enum WINPOS_FLAG
        {
            NOSIZE          = 0x0001,
            NOMOVE          = 0x0002,
            NOZORDER        = 0x0004,
            NOREDRAW        = 0x0008,
            NOACTIVATE      = 0x0010,
            DRAWFRAME       = 0x0020,
            FRAMECHANGED    = 0x0020,
            SHOWWINDOW      = 0x0040,
            HIDEWINDOW      = 0x0080,
            NOCOPYBITS      = 0x0100,
            NOOWNERZORDER   = 0x0200,
            NOREPOSITION    = 0x0200,
            NOSENDCHANGING  = 0x0400,
            DEFERERASE      = 0x2000,
            ASYNCWINDOWPOS  = 0x4000,
        }
        #endregion End - 32 bit API

        #region Variables

        #region Panel

        TitleBar  m_viewTitleBar      = new TitleBar();
        SubMenu   m_viewSubMenu       = new SubMenu();
        MainMenu  m_viewMainMenu      = new MainMenu();

        private Dictionary<EN_BUTTONEVENT_MAINMENU, List<string>> m_dicOfSubMenus       = new Dictionary<EN_BUTTONEVENT_MAINMENU, List<string>>();
        private Dictionary<EN_BUTTONEVENT_MAINMENU, Dictionary<int, EN_BUTTONEVENT_SUBMENU>> _subMenuEventByDisplayIndex = new Dictionary<EN_BUTTONEVENT_MAINMENU, Dictionary<int, EN_BUTTONEVENT_SUBMENU>>();

        EN_BUTTONEVENT_MAINMENU m_enClickedMainMenu     = EN_BUTTONEVENT_MAINMENU.OPERATION;
        EN_BUTTONEVENT_SUBMENU m_enClickedSubMenu       = EN_BUTTONEVENT_SUBMENU.OPERATION_MAIN;

        #region Main view
        Dictionary<EN_BUTTONEVENT_SUBMENU, UserControlForMainView.CustomView> m_dicOfMainView           = new Dictionary<EN_BUTTONEVENT_SUBMENU, UserControlForMainView.CustomView>();
        Dictionary<EN_BUTTONEVENT_MAINMENU, EN_BUTTONEVENT_SUBMENU> m_dicOfMainViewByMainButtonEvent    = new Dictionary<EN_BUTTONEVENT_MAINMENU, EN_BUTTONEVENT_SUBMENU>();
        Dictionary<EN_BUTTONEVENT_SUBMENU, string> m_mappingForButtonName                               = new Dictionary<EN_BUTTONEVENT_SUBMENU,string>();
        Dictionary<EN_BUTTONEVENT_SUBMENU, int> m_dicOfTimerInterval                                    = new Dictionary<EN_BUTTONEVENT_SUBMENU, int>();

        UserControlForMainView.CustomView m_viewRecentView;

		#region Operation
		Views.Operation.Operation_Main			m_viewOperationMain				= new Operation.Operation_Main();
		Views.Operation.Operation_StateMonitor	m_viewOperationMonitor			= new Operation.Operation_StateMonitor();
		Views.Operation.Operation_Tracking		m_viewOperationTracking			= new Operation.Operation_Tracking();
        Views.Operation.Operation_SecsGem       m_viewOperationSecsGem          = new Operation.Operation_SecsGem();
        Views.Operation.Operation_History       m_viewOperationHistory          = new Operation.Operation_History();
        Views.Operation.Operation_JobInfo       m_viewOperationJobInfo          = new Operation.Operation_JobInfo();
        Views.Operation.Operation_RAMMetrics    m_viewOperationRAMMetrics       = new Operation.Operation_RAMMetrics();
		#endregion

		#region Recipe
		Views.Recipe.Recipe_Main	m_viewRecipeMain	= new Recipe.Recipe_Main();
		Views.Recipe.Recipe_Options m_viewRecipeOptions = new Recipe.Recipe_Options();
        #endregion

		#region Setup
		Views.Setup.Setup_Options				m_viewSetupOptions				= new Setup.Setup_Options();
		Views.Setup.Setup_ProcessModules		m_viewSetupProcessModule		= new Setup.Setup_ProcessModules();
        Views.Setup.Setup_LoadPort              m_viewSetupLoadPort             = new Setup.Setup_LoadPort();
        Views.Setup.Setup_AtmRobot              m_viewSetupAtmRobot             = new Setup.Setup_AtmRobot();
        Views.Setup.Setup_RFID                  m_viewSetupRFID                 = new Setup.Setup_RFID();
        Views.Setup.Setup_FanFilterUnit         m_viewSetupFanFilterUnit        = new Setup.Setup_FanFilterUnit();
        //Views.Setup.Setup_Vision				m_viewSetupVision			    = new Setup.Setup_Vision();
		#endregion

		#region History
        Views.History.History_MainLog       m_viewHistoryMainLog            = new History.History_MainLog();
        #endregion

        #region Config
        Views.Config.Config_Alarm           m_viewConfigAlarm               = new Config.Config_Alarm();
        Views.Config.Config_Analog          m_viewConfigAnalog              = new Config.Config_Analog();
        Views.Config.Config_Cylinder        m_viewConfigCylindre            = new Config.Config_Cylinder();
        Views.Config.Config_Digital         m_viewConfigDigital             = new Config.Config_Digital();
        Views.Config.Config_Trigger			m_viewConfigTrigger				= new Config.Config_Trigger();
        Views.Config.Config_Language        m_viewConfigLanguage            = new Config.Config_Language();
        Views.Config.Config_Motion          m_viewConfigMotion              = new Config.Config_Motion();
		
		// 2021.08.06 by junho [MOD] merge Socket and Serial to Communication
		//Views.Config.Config_Serial          m_viewConfigSerial              = new Config.Config_Serial();
		//Views.Config.Config_Socket          m_viewConfigSocket              = new Config.Config_Socket();
		Views.Config.Config_Communication m_viewConfigCommunication = new Config.Config_Communication();

        Views.Config.Config_Interrupt       m_viewConfigInterrupt           = new Config.Config_Interrupt();
        Views.Config.Config_Tool            m_viewConfigTool                = new Config.Config_Tool();     // 2021.10.06 jhchoo [ADD]

		// Link, Flow, Port -> Action 내의 탭 전환 페이지로 변경
		Views.Config.Config_Action          m_viewConfigAction              = new Config.Config_Action();

		// 2022.10.25 by junho [MOD] for jog manager
		//Views.Config.Config_Jog				m_viewConfigJog					= new Config.Config_Jog();
		Views.Config.Config_JogManage		m_viewConfigJog					= new Config.Config_JogManage();

		Views.Config.Config_Device			m_viewConfigDevice				= new Config.Config_Device();
		Views.Config.Config_Parameters		m_viewConfigParameter			= new Config.Config_Parameters();
		Views.Config.Config_Interlock		m_viewConfigInterlock			= new Config.Config_Interlock(); // 2022.02.15 WDW [ADD]
        #endregion

        #region Log Process
        Process m_processLog                = null;
        #endregion End - Log Process

        #endregion

        #endregion

        #region Functional Form
        private Views.Login.Form_Login				m_fLogin            = new Views.Login.Form_Login();
        #endregion

		#region Log
		private Log.LogManager m_InstanceOfLog				= null;
		#endregion

		#region AlarmForm
		private Views.Functional.Form_AlarmMessage		m_fAlarmMessage		= Views.Functional.Form_AlarmMessage.GetInstance();

		private Queue<EN_ALARM_FORM_STATE> queueState	= new Queue<EN_ALARM_FORM_STATE>();
		private System.Threading.ReaderWriterLockSlim m_RWLock	= new System.Threading.ReaderWriterLockSlim();
		#endregion

		#region ProgressBar Form
		private Views.Functional.Form_ProgressBar		m_fProgressBar		= null;
		private EN_PROGRESS_BAR_FORM_STATE enProgressBarState				= EN_PROGRESS_BAR_FORM_STATE.WATING;
		#endregion

		#region Timer
		System.Windows.Forms.Timer m_pTimerForUpdateGUI						= new System.Windows.Forms.Timer();
		System.Windows.Forms.Timer m_pTimerForAlarm							= new System.Windows.Forms.Timer();
        #endregion

        #endregion

        public Form_Frame()
        {
            InitializeComponent();

            SetSizeByCustomer();

            InitializePanel();

            InitializeMainView();

            InitializeInstances();

			RegisterEventForGUI();

			InitializeFuntional();

            SetExternalProcess();

            // 2021.10.06. jhlim [ADD] 통신 및 시나리오를 초기화한다.
            InitScenarioHandler();
			// GUI 생성 전 발생한 알람이 있는지 확인한다.
			m_fAlarmMessage.ManualCheckIsGeneratedAlarm();

            // 2023.09.11. by shkim. [ADD] 초기 개발 시 Form_Frame 사이즈에 따른 페이지 사이즈 확인용
            if (System.Diagnostics.Debugger.IsAttached)
            {
                Console.WriteLine("[UI Frame Size]");
                Console.WriteLine("Main Frame : ({0},{1})", this.Size.Width, this.Size.Height);
                Console.WriteLine("Table Layout Panel : ({0},{1})", _tableLayoutPanel_FormFrame.Size.Width, _tableLayoutPanel_FormFrame.Size.Height);
                Console.WriteLine("Contents : ({0},{1})"
                    , _tableLayoutPanel_FormFrame.Size.Width - _tableLayoutPanel_FormFrame.ColumnStyles[1].Width
                    , _tableLayoutPanel_FormFrame.Size.Height - _tableLayoutPanel_FormFrame.RowStyles[0].Height - _tableLayoutPanel_FormFrame.RowStyles[2].Height);
                Console.WriteLine("Title Bar : ({0},{1})"
                    , _tableLayoutPanel_FormFrame.Size.Width
                    , _tableLayoutPanel_FormFrame.RowStyles[0].Height);
                Console.WriteLine("Main Menu : ({0},{1})"
                    , _tableLayoutPanel_FormFrame.Size.Width
                    , _tableLayoutPanel_FormFrame.RowStyles[2].Height);
                Console.WriteLine("Sub Menu : ({0},{1})"
                    , _tableLayoutPanel_FormFrame.ColumnStyles[1].Width
                    , _tableLayoutPanel_FormFrame.Size.Height - _tableLayoutPanel_FormFrame.RowStyles[0].Height - _tableLayoutPanel_FormFrame.RowStyles[2].Height);
            }
            // 2023.09.11. by shkim. [END]

			// 2024.06.13 by junho [ADD] UI 언어변환 
			var langualge = Language_.Language.GetInstance();
			DateTime start = DateTime.Now;
			foreach(Control control in this.Controls)
			{
				TranslateTextAllControls(ref langualge, control);
			}
			Console.WriteLine(string.Format("translate all span:{0}ms", (DateTime.Now - start).TotalMilliseconds.ToString()));

			if (InitializeFinished != null)
				InitializeFinished();
        }

		private void TranslateTextAllControls(ref Language_.Language langualge, Control control)
		{
            if (control.GetType() == typeof(DataGridView))
            {
                DataGridView dgControl = control as DataGridView;

                for (int nCol = 0; nCol < dgControl.Columns.Count; nCol++)
                {
                    for (int nRow = 0; nRow < dgControl.Rows.Count; nRow++)
                    {
                        if (dgControl[nCol, nRow].Value != null)
							//dgControl[nCol, nRow].Value = langualge.TranslateWord(dgControl[nCol, nRow].Value.ToString().Trim());	// 2024.10.08 by junho [MOD] trim 제거
							dgControl[nCol, nRow].Value = langualge.TranslateWord(dgControl[nCol, nRow].Value.ToString());
					}
                }
            }
            else
            {
                //control.Text = langualge.TranslateWord(control.Text.Trim());    // 2024.10.08 by junho [MOD] trim 제거
				control.Text = langualge.TranslateWord(control.Text);
			}
			foreach(Control subControl in control.Controls)
			{
				TranslateTextAllControls(ref langualge, subControl);
			}
		}

		public enum EN_ALARM_FORM_STATE
		{
			WATING,
			DISPLAYING,
			OFF,
		}
		public enum EN_PROGRESS_BAR_FORM_STATE
		{
			WATING,
			DISPLAYING,
			OFF,
		}

        #region Initialize Panel
        /// <summary>
        /// 2023.09.07 by shkim [MOD] 복수의 Panel -> TableLayoutPanel로 변경 (Layout 확장성 개선)
        /// 2020.02.05 by yjlee [ADD] Initialize the panels.
        /// </summary>
        private void InitializePanel()
        {
            // 1. TitleBar
            // m_panelTitleBar.Controls.Add(m_viewTitleBar);
            _tableLayoutPanel_FormFrame.Controls.Add(m_viewTitleBar, 0, 0);
            _tableLayoutPanel_FormFrame.SetColumnSpan(m_viewTitleBar, 2);
            m_viewTitleBar.Dock = DockStyle.Fill;
            
            // 2. MainMenu
            m_viewMainMenu.evtButtonClick   += ReceiveEventFromMainMenu;
            // m_panelMainMenu.Controls.Add(m_viewMainMenu);
            _tableLayoutPanel_FormFrame.Controls.Add(m_viewMainMenu, 0, 2);
            _tableLayoutPanel_FormFrame.SetColumnSpan(m_viewMainMenu, 2);
            m_viewMainMenu.Dock = DockStyle.Fill;
            
            // 3. SubMenu
            m_viewSubMenu.evtButtonClick    += ReceiveEventFromSubMenu;
            // m_panelSubMenu.Controls.Add(m_viewSubMenu);
            _tableLayoutPanel_FormFrame.Controls.Add(m_viewSubMenu, 1, 1);
            m_viewSubMenu.Dock = DockStyle.Fill;
            

            // 4. Make default mapping tables.
            foreach(EN_BUTTONEVENT_MAINMENU enMainMenu in Enum.GetValues(typeof(EN_BUTTONEVENT_MAINMENU)))
            {
                m_dicOfSubMenus.Add(enMainMenu, new List<string>());
                _subMenuEventByDisplayIndex.Add(enMainMenu, new Dictionary<int, EN_BUTTONEVENT_SUBMENU>());
            }

            foreach(EN_BUTTONEVENT_SUBMENU enSubMenu in Enum.GetValues(typeof(EN_BUTTONEVENT_SUBMENU)))
            {
                string strSubMenu       = enSubMenu.ToString();
                int nIndexOfToken       = strSubMenu.IndexOf('_');

                if(-1 != nIndexOfToken)
                {
                    EN_BUTTONEVENT_MAINMENU enMainMenu  = EN_BUTTONEVENT_MAINMENU.EXIT;

                    string strMainMenu      = strSubMenu.Substring(0, nIndexOfToken);
                    
                    if (Enum.TryParse(strMainMenu, out enMainMenu))
                    {
                        string strButtonName    = strSubMenu.Substring(nIndexOfToken + 1).Replace('_', ' ');

                        if (string.Equals(strButtonName, "JOB INFO", StringComparison.OrdinalIgnoreCase) &&
                            Work.AppConfigManager.Instance.GemSpecification != Define.DefineEnumProject.AppConfig.EN_SECSGEM_SPEC.GEM300)
                        {
                            continue;
                        }

                        if (strButtonName.Equals("EFEM SIMULATOR"))
                        {
                            if (false == Debugger.IsAttached && false == Work.AppConfigManager.Instance.LoadPortControllerType.Equals(Define.DefineEnumProject.AppConfig.EN_LOADPORT_CONTROLLER.NONE))
                                continue;
                        }
                        
                        // 공정 종류에 따라 SubMenu 이름을 바꾼다.
                        if (enSubMenu.Equals(EN_BUTTONEVENT_SUBMENU.SETUP_PROCESSMODULE))
                        {
                            switch (Work.AppConfigManager.Instance.ProcessType)
                            {
                                case Define.DefineEnumProject.AppConfig.EN_PROCESS_TYPE.NONE:
                                    break;
                                case Define.DefineEnumProject.AppConfig.EN_PROCESS_TYPE.BIN_SORTER:
                                    strButtonName = "PWA500BIN";
                                    break;
                                case Define.DefineEnumProject.AppConfig.EN_PROCESS_TYPE.DIE_TRANSFER:
                                case Define.DefineEnumProject.AppConfig.EN_PROCESS_TYPE.DIE_TRANSFER_300:
                                    strButtonName = "PWA500W";
                                    break;
                                default:
                                    break;
                            }
                        }

                        int nDisplayIndex = m_dicOfSubMenus[enMainMenu].Count;

                        m_dicOfSubMenus[enMainMenu].Add(strButtonName);
                        _subMenuEventByDisplayIndex[enMainMenu].Add(nDisplayIndex, enSubMenu);
                        m_mappingForButtonName.Add(enSubMenu, strButtonName);
                    }
                }
            }

            // 5. set initialization state
            m_viewSubMenu.SetButtons(
                m_dicOfSubMenus[m_enClickedMainMenu],
                _subMenuEventByDisplayIndex[m_enClickedMainMenu]);
        }
        #endregion

        #region Initialize Main View
        /// <summary>
        /// 2020.05.15 by yjlee [ADD] Initialize the data structure for the main views.
        /// </summary>
        private void InitializeMainView()
        {
            #region Make mapping table
			m_dicOfMainViewByMainButtonEvent.Add(EN_BUTTONEVENT_MAINMENU.OPERATION, EN_BUTTONEVENT_SUBMENU.OPERATION_MAIN);
            //m_dicOfMainViewByMainButtonEvent.Add(EN_BUTTONEVENT_MAINMENU.CONFIG, EN_BUTTONEVENT_SUBMENU.CONFIG_MOTION);
            m_dicOfMainViewByMainButtonEvent.Add(EN_BUTTONEVENT_MAINMENU.CONFIG, EN_BUTTONEVENT_SUBMENU.CONFIG_ANALOG);
            m_dicOfMainViewByMainButtonEvent.Add(EN_BUTTONEVENT_MAINMENU.RECIPE, EN_BUTTONEVENT_SUBMENU.RECIPE_MAIN);
            m_dicOfMainViewByMainButtonEvent.Add(EN_BUTTONEVENT_MAINMENU.HISTORY, EN_BUTTONEVENT_SUBMENU.HISTORY_MAIN_LOG);
            //m_dicOfMainViewByMainButtonEvent.Add(EN_BUTTONEVENT_MAINMENU.SETUP, EN_BUTTONEVENT_SUBMENU.SETUP_OPTIONS);
            m_dicOfMainViewByMainButtonEvent.Add(EN_BUTTONEVENT_MAINMENU.SETUP, EN_BUTTONEVENT_SUBMENU.SETUP_LOADPORT);
            #endregion

            #region Operation
            m_dicOfMainView.Add(EN_BUTTONEVENT_SUBMENU.OPERATION_MAIN, m_viewOperationMain);
			m_dicOfMainView.Add(EN_BUTTONEVENT_SUBMENU.OPERATION_MONITORING, m_viewOperationMonitor);
			m_dicOfMainView.Add(EN_BUTTONEVENT_SUBMENU.OPERATION_RAM_METRICS, m_viewOperationRAMMetrics);
			m_dicOfMainView.Add(EN_BUTTONEVENT_SUBMENU.OPERATION_TRACKING, m_viewOperationTracking);
            m_dicOfMainView.Add(EN_BUTTONEVENT_SUBMENU.OPERATION_SECSGEM, m_viewOperationSecsGem);
            m_dicOfMainView.Add(EN_BUTTONEVENT_SUBMENU.OPERATION_LOT_HISTORY, m_viewOperationHistory);
            m_dicOfMainView.Add(EN_BUTTONEVENT_SUBMENU.OPERATION_JOB_INFO, m_viewOperationJobInfo);
            #endregion

            #region Recipe
            m_dicOfMainView.Add(EN_BUTTONEVENT_SUBMENU.RECIPE_MAIN, m_viewRecipeMain);
			//m_dicOfMainView.Add(EN_BUTTONEVENT_SUBMENU.RECIPE_OPTIONS, m_viewRecipeOptions);
            #endregion

            #region History
            m_dicOfMainView.Add(EN_BUTTONEVENT_SUBMENU.HISTORY_MAIN_LOG, m_viewHistoryMainLog);
            #endregion

			#region Setup
			//m_dicOfMainView.Add(EN_BUTTONEVENT_SUBMENU.SETUP_OPTIONS, m_viewSetupOptions);
            m_dicOfMainView.Add(EN_BUTTONEVENT_SUBMENU.SETUP_LOADPORT, m_viewSetupLoadPort);
            m_dicOfMainView.Add(EN_BUTTONEVENT_SUBMENU.SETUP_RFID, m_viewSetupRFID);
            m_dicOfMainView.Add(EN_BUTTONEVENT_SUBMENU.SETUP_ROBOT, m_viewSetupAtmRobot);
			m_dicOfMainView.Add(EN_BUTTONEVENT_SUBMENU.SETUP_PROCESSMODULE, m_viewSetupProcessModule);
            m_dicOfMainView.Add(EN_BUTTONEVENT_SUBMENU.SETUP_FFU, m_viewSetupFanFilterUnit);
            //m_dicOfMainView.Add(EN_BUTTONEVENT_SUBMENU.SETUP_VISION, m_viewSetupVision);
            #endregion

            #region Config
            m_dicOfMainView.Add(EN_BUTTONEVENT_SUBMENU.CONFIG_ALARM, m_viewConfigAlarm);
            m_dicOfMainView.Add(EN_BUTTONEVENT_SUBMENU.CONFIG_ANALOG, m_viewConfigAnalog);
            m_dicOfMainView.Add(EN_BUTTONEVENT_SUBMENU.CONFIG_CYLINDER, m_viewConfigCylindre);
            m_dicOfMainView.Add(EN_BUTTONEVENT_SUBMENU.CONFIG_DIGITAL, m_viewConfigDigital);
            m_dicOfMainView.Add(EN_BUTTONEVENT_SUBMENU.CONFIG_TRIGGER, m_viewConfigTrigger);
			m_dicOfMainView.Add(EN_BUTTONEVENT_SUBMENU.CONFIG_LANGUAGE, m_viewConfigLanguage);
            //m_dicOfMainView.Add(EN_BUTTONEVENT_SUBMENU.CONFIG_MOTION, m_viewConfigMotion);
			m_dicOfMainView.Add(EN_BUTTONEVENT_SUBMENU.CONFIG_COMMUNICATION, m_viewConfigCommunication);
			m_dicOfMainView.Add(EN_BUTTONEVENT_SUBMENU.CONFIG_INTERRUPT, m_viewConfigInterrupt);
			m_dicOfMainView.Add(EN_BUTTONEVENT_SUBMENU.CONFIG_ACTION, m_viewConfigAction);
            //m_dicOfMainView.Add(EN_BUTTONEVENT_SUBMENU.CONFIG_TOOL, m_viewConfigTool);      // 2021.10.06 jhchoo [ADD]
			//m_dicOfMainView.Add(EN_BUTTONEVENT_SUBMENU.CONFIG_JOG, m_viewConfigJog);
			m_dicOfMainView.Add(EN_BUTTONEVENT_SUBMENU.CONFIG_DEVICE, m_viewConfigDevice);
            m_dicOfMainView.Add(EN_BUTTONEVENT_SUBMENU.CONFIG_PARAMETERS, m_viewConfigParameter);
            //m_dicOfMainView.Add(EN_BUTTONEVENT_SUBMENU.CONFIG_INTERLOCK, m_viewConfigInterlock);
            #endregion

			#region SetPage TimerInterval
			foreach (var kvp in m_dicOfMainView)
			{
				// m_panelMainView.Controls.Add(kvp.Value);
                _tableLayoutPanel_FormFrame.Controls.Add(kvp.Value, 0, 1);
				kvp.Value.Hide();
                kvp.Value.Dock = DockStyle.Fill;

				switch (kvp.Key)
				{
					default:
						m_dicOfTimerInterval.Add(kvp.Key, Define.DefineConstant.MainForm.INTERVAL_UPDATE_GUI);
						break;

					//case EN_BUTTONEVENT_SUBMENU.SETUP_VISION:
					case EN_BUTTONEVENT_SUBMENU.OPERATION_TRACKING:
						m_dicOfTimerInterval.Add(kvp.Key, Define.DefineConstant.MainForm.INTERVAL_GRAPH);
						break;
				}
			}
			#endregion

			m_viewRecentView = m_viewOperationMain;

			m_viewRecentView.Show();
			m_viewRecentView.ActivateView();
        }
        #endregion

        #region Initialize Intances
        private void InitializeInstances()
        {
			#region LOG
			m_InstanceOfLog			= Log.LogManager.GetInstance();
			#endregion

            #region GUI Timer
            m_pTimerForUpdateGUI.Tick       += new EventHandler(FunctionForTimerTick);

            m_pTimerForUpdateGUI.Start();
            #endregion

			#region ProgressBar
			m_fProgressBar	= Views.Functional.Form_ProgressBar.GetInstance();
			#endregion

		}
        #endregion

        #region Internal Interface
        /// <summary>
        /// 2020.05.15 by yjlee [ADD] Set a main view that clicked.
        /// </summary>
        private void SetMainView(UserControlForMainView.CustomView viewMain, int nTimerInterval)
        {
            m_pTimerForUpdateGUI.Stop();

            m_viewRecentView.Hide();
            m_viewRecentView.DeactivateView();

            m_viewRecentView    = viewMain;

            m_viewRecentView.BringToFront();

            m_viewRecentView.Show();
            m_viewRecentView.ActivateView();

            m_pTimerForUpdateGUI.Interval       = nTimerInterval;
			m_pTimerForUpdateGUI.Start();
        }
		/// <summary>
		/// 2020.07.09 by twkang [ADD] 
		/// </summary>
		private void InitializeFuntional()
		{
			m_pTimerForAlarm.Interval	= 100;
			m_pTimerForAlarm.Tick		+= new EventHandler(FuntionForAlarm);
			Views.Functional.Form_AlarmMessage.GetInstance().evtAlarmMessage	+= SetAlarmMessageForm;
			Views.Functional.Form_ProgressBar.GetInstance().evtProgressBar		+= SetProgressBarForm;
			m_pTimerForAlarm.Start();
		}
		
        #region Timer
        /// <summary>
        /// 2020.05.15 by yjlee [ADD] This function will be called by the system timer to update the GUI.
        /// </summary>
        private void FunctionForTimerTick(object sender, EventArgs e)
        {
            m_viewRecentView.CallFunctionByTimer();
        }
		/// <summary>
		/// 2020.07.10 by twkang [ADD] 타이머에 의해 호출되는함수, 
		/// </summary>
		private void FuntionForAlarm(object sender, EventArgs e)
		{
			m_RWLock.EnterReadLock();
			int nQueueCount	= queueState.Count;
			m_RWLock.ExitReadLock();
			
			if(nQueueCount > 0)
			{
				m_RWLock.EnterWriteLock();
				var enState = queueState.Dequeue();
				m_RWLock.ExitWriteLock();

				switch (enState)
				{
					case EN_ALARM_FORM_STATE.DISPLAYING: // 알람창 발생
						Views.Functional.Form_AlarmMessage.GetInstance().CreateForm();
						SetProgressBarForm(false);
						break;
					case EN_ALARM_FORM_STATE.OFF:
						Views.Functional.Form_AlarmMessage.GetInstance().CloseForm();
						break;
					default:
						break;
				}
			}
			
			switch(enProgressBarState)
			{
				case EN_PROGRESS_BAR_FORM_STATE.WATING:
					break;
				case EN_PROGRESS_BAR_FORM_STATE.DISPLAYING:
					Views.Functional.Form_ProgressBar.GetInstance().ShowByMainForm();
					enProgressBarState	= EN_PROGRESS_BAR_FORM_STATE.WATING;
					break;
				case EN_PROGRESS_BAR_FORM_STATE.OFF:
					Views.Functional.Form_ProgressBar.GetInstance().HideByMainForm();
					enProgressBarState	= EN_PROGRESS_BAR_FORM_STATE.WATING;
					break;
			}
		}
        #endregion

        private void SetSizeByCustomer()
        {
            switch (Work.AppConfigManager.Instance.ProcessType)
            {
                case Define.DefineEnumProject.AppConfig.EN_PROCESS_TYPE.DIE_TRANSFER:
                case Define.DefineEnumProject.AppConfig.EN_PROCESS_TYPE.DIE_TRANSFER_300:
                    {
                        Size = new Size(1920, 1080);
                    }
                    break;
                default:
                    {
                        Size = new Size(1280, 1024);
                    }
                    break;
            }
        }

        private Size GetLogProgramWindowSize()
        {
            Size size = new Size();

            size.Width = this.Size.Width;
            size.Height = _tableLayoutPanel_FormFrame.Height - m_viewTitleBar.Height - m_viewMainMenu.Height;

            return size;
        }

        private Size GetLoginControlSize()
        {
            Size size = new Size();

            size.Width = this.Size.Width;
            size.Height = _tableLayoutPanel_FormFrame.Height - m_viewTitleBar.Height;

            return size;
        }

        #endregion
		
		#region Register Event
		/// <summary>
		/// 2020.06.01 by yjlee [ADD] Register the events for the GUI.
		/// </summary>
		private void RegisterEventForGUI()
		{
			m_viewTitleBar.RegisterSubject(EquipmentState_.EquipmentState.GetInstance());
			m_viewTitleBar.RegisterSubject(Account_.Account.GetInstance());
			m_viewMainMenu.RegisterSubject(Account_.Account.GetInstance());
		}
		#endregion

        #region External Process
        /// <summary>
        /// 2021.01.13 by yjlee [ADD] Set the external process.
        /// </summary>
        private void SetExternalProcess()
        {
            ExitExternalProcess();

            string strLogProcessName                = ExternalProcess.PROCESS_NAME_LOG + ".exe";

            System.IO.FileInfo fInfo                = new System.IO.FileInfo(strLogProcessName);

            if(fInfo.Exists)
            {
                m_processLog    = new Process();

                m_processLog.StartInfo.FileName = ExternalProcess.PROCESS_NAME_LOG + ".exe";

				// 2025.01.14. by shkim. [MOD] 경로를 전달할 때 경로에 공백이 있을 때는 시작과 끝에 \를 붙여서 전달하면 전달받는 Process에서 하나의 Argument로 인식한다.
				// 첫 번째 Argument에 로그파일의 경로를 전달한다.
				// 두 번째 Argument에 부모프로세스(설비프로그램)의 ID를 전달한다. 부모 프로세스가 죽으면, 로그도 죽인다. 로그는 부모프로세스의 ID를 이용한다.

				// m_processLog.StartInfo.Arguments = FilePath.FILEPATH_LOG_MAIN;	// 2022.11.14 by junho [ADD] for have same Log path
				//m_processLog.StartInfo.Arguments = FilePath.FILEPATH_LOG_MAIN + " " + Process.GetCurrentProcess().Id.ToString();

				// m_processLog.StartInfo.Arguments = $"\"{FilePath.FILEPATH_LOG_MAIN}\" {Process.GetCurrentProcess().Id}"; // 보간문자열 버전 VS2022
                m_processLog.StartInfo.Arguments = string.Format("\"{0}\" {1}", FilePath.FILEPATH_LOG_MAIN, Process.GetCurrentProcess().Id);

                // 2025.01.14. by shkim. [END]

                m_processLog.Start();

                while (IntPtr.Zero == m_processLog.MainWindowHandle)
                    System.Threading.Thread.Sleep(50);

                while (true)
                {
                    System.Threading.Thread.Sleep(50);

                    // 2023.09.07. by shkim [MOD] Size 가변가능하도록 수정
                    var rtn = SetWindowPos(m_processLog.MainWindowHandle, (int)WINPOS_SPFLAG.HWND_BOTTOM
                        , m_viewOperationMain.Location.X
                        , m_viewOperationMain.Location.Y - 1
                        , GetLogProgramWindowSize().Width
                        , GetLogProgramWindowSize().Height
                        , (int)WINPOS_FLAG.HIDEWINDOW);

                    Console.WriteLine(rtn.ToString());

                    if (rtn != IntPtr.Zero) break;
                }
            }
        }
        /// <summary>
        /// 2021.01.22 by yjlee [ADD] Exit the external process.
        /// </summary>
        private void ExitExternalProcess()
        {
            foreach(Process pr in Process.GetProcesses())
            {
                if(pr.ProcessName.StartsWith(ExternalProcess.PROCESS_NAME_LOG))
                {
					if(false == pr.CloseMainWindow())
                    {
                        // 2022.11.03. by shkim. [MOD] 이미 프로세스가 종료됬을 때에 대한 예외처리 추가
                        try
                        {
                            pr.Kill();    
                        }
                        catch
                        {

                        }
                    }
				}
            }

            if(null != m_processLog)
            {
                m_processLog.Dispose();
                m_processLog    = null;
            }
        }
        #endregion End - External Process

        #region GUI Event
        /// <summary>
        /// 2020.02.05 by yjlee [ADD] When a user click the main menu, this function will be called.
        /// </summary>
        private bool ReceiveEventFromMainMenu(int tabIndex)
        {
			if (false == Enum.IsDefined(typeof(EN_BUTTONEVENT_MAINMENU), tabIndex))
				return false;

			EN_BUTTONEVENT_MAINMENU enButtonEvent = (EN_BUTTONEVENT_MAINMENU)tabIndex;
                switch(enButtonEvent)
                {
                    case EN_BUTTONEVENT_MAINMENU.USER:
                        m_fLogin.Size = new System.Drawing.Size(GetLoginControlSize().Width, GetLoginControlSize().Height);
                        m_fLogin.CreateForm();
                        return false;

                    case EN_BUTTONEVENT_MAINMENU.HISTORY:
                        if(null != m_processLog)
                        {
							SetWindowPos(m_processLog.MainWindowHandle
                                        , (int)WINPOS_SPFLAG.HWND_TOPMOST
                                        , 0
                                        , 0
                                        , 0
                                        , 0
                                        , (int)WINPOS_FLAG.SHOWWINDOW | (int)WINPOS_FLAG.NOSIZE | (int)WINPOS_FLAG.NOMOVE);
                        }
                        break;

                    case EN_BUTTONEVENT_MAINMENU.EXIT:
						// 2021.11.20 by Thienvv [ADD] Do not exit program when perform autorun
                        EQUIPMENT_STATE enState = EquipmentState.GetInstance().GetState();
                        if (enState != EQUIPMENT_STATE.IDLE && enState != EQUIPMENT_STATE.PAUSE)
                        {
                            return false;
                        }
                        if (Functional.Form_MessageBox.GetInstance().ShowMessage("Do you want Exit program?") == false)
                            return false;
                        // 여기서 프로그램 종료 전 꺼야하는 DIGITAL IO 처리한다.

                        FrameOfSystem3.Task.TaskOperator.GetInstance().IsExiting = true;

                        // 2026.07.02. jhlim [ADD] 종료 전 큐에 남은 랏 이력을 모두 기록한다.
                        EFEM.CustomizedByProcessType.PWA500Common.LotHistoryLog.Instance.FlushAll();

                        // 2021.10.06. jhlim [ADD] 통신 및 시나리오를 종료한다.
                        CloseScenarioHandler();

						Log.LogManager.GetInstance().Exit();

                        ExitExternalProcess();
                        
                        Application.Exit();
                        return false;

                    default:
                        if(null != m_processLog)
                        {
                            SetWindowPos(m_processLog.MainWindowHandle, (int)WINPOS_SPFLAG.HWND_BOTTOM, 0, 0, 0, 0, (int)WINPOS_FLAG.HIDEWINDOW | (int)WINPOS_FLAG.NOSIZE | (int)WINPOS_FLAG.NOMOVE);
                        }
                        break;
                }

                m_enClickedMainMenu     = enButtonEvent;

                //m_viewSubMenu.SetButtons(m_dicOfSubMenus[enButtonEvent]);
                m_viewSubMenu.SetButtons(
                    m_dicOfSubMenus[enButtonEvent],
                    _subMenuEventByDisplayIndex[enButtonEvent]);

                m_viewSubMenu.SetClickedButton(m_mappingForButtonName[m_dicOfMainViewByMainButtonEvent[enButtonEvent]]);

                var enSubmenuEvent      = m_dicOfMainViewByMainButtonEvent[enButtonEvent];

                SetMainView(m_dicOfMainView[enSubmenuEvent], m_dicOfTimerInterval[enSubmenuEvent]);

                return true;
        }
        /// <summary>
        /// 2020.02.05 by yjlee [ADD] When a user click the sub menu, this function will be called.
        ///	2024.06.13 by junho [MOD] 한글화를 위해 tabindex를 받아서 처리하도록 변경
        /// </summary>
        //     private bool ReceiveEventFromSubMenu(int tabIndex)
        //     {
        //	int enumNo = (int)m_enClickedMainMenu * 100 + tabIndex;
        //if (false == Enum.IsDefined(typeof(EN_BUTTONEVENT_SUBMENU), enumNo))
        //	return false;

        //EN_BUTTONEVENT_SUBMENU enButtonEvent = (EN_BUTTONEVENT_SUBMENU)enumNo;			

        ////if (EN_BUTTONEVENT_SUBMENU.SETUP_JOG == enButtonEvent)
        ////{
        ////	Functional.Jog.Form_Jog.GetInstance().CreateForm();
        ////	return false;
        ////}

        //         if (EN_BUTTONEVENT_SUBMENU.OPERATION_EFEM_SIMULATOR == enButtonEvent)
        //         {
        //             EFEM_Simulator.Form_EFEMSimulator.Instance.CreateForm();
        //             return false;
        //         }

        //         //if (Enum.TryParse(strSubMenu, out enButtonEvent))
        //         {
        //             if (m_enClickedSubMenu == enButtonEvent) { return false; }

        //             m_enClickedSubMenu      = enButtonEvent;

        //             m_dicOfMainViewByMainButtonEvent[m_enClickedMainMenu]   = m_enClickedSubMenu;

        //             SetMainView(m_dicOfMainView[m_enClickedSubMenu], m_dicOfTimerInterval[m_enClickedSubMenu]);

        //             return true;
        //         }

        //         //else
        //         //{
        //             // 메뉴 이름이 Enum과 다른 경우
        //             //foreach (var item in m_mappingForButtonName)
        //             //{
        //             //    if (item.Value.Equals(strEnum))
        //             //    {
        //             //        enButtonEvent = item.Key;
        //             //        break;
        //             //    }
        //             //}

        //             //if (m_enClickedSubMenu == enButtonEvent) { return false; }

        //             //m_enClickedSubMenu = enButtonEvent;

        //             //m_dicOfMainViewByMainButtonEvent[m_enClickedMainMenu] = m_enClickedSubMenu;

        //             //SetMainView(m_dicOfMainView[m_enClickedSubMenu], m_dicOfTimerInterval[m_enClickedSubMenu]);

        //             //return true;
        //         //}
        //     }
        private bool ReceiveEventFromSubMenu(int subMenuValue)
        {
            if (false == Enum.IsDefined(typeof(EN_BUTTONEVENT_SUBMENU), subMenuValue))
                return false;

            EN_BUTTONEVENT_SUBMENU enButtonEvent = (EN_BUTTONEVENT_SUBMENU)subMenuValue;

            if (false == m_mappingForButtonName.ContainsKey(enButtonEvent))
                return false;

            if (EN_BUTTONEVENT_SUBMENU.OPERATION_EFEM_SIMULATOR.Equals(enButtonEvent))
            {
                EFEM_Simulator.Form_EFEMSimulator.Instance.CreateForm();
                return false;
            }

            if (false == m_dicOfMainView.ContainsKey(enButtonEvent))
                return false;

            if (m_enClickedSubMenu == enButtonEvent)
                return false;

            m_enClickedSubMenu = enButtonEvent;
            m_dicOfMainViewByMainButtonEvent[m_enClickedMainMenu] = m_enClickedSubMenu;

            SetMainView(m_dicOfMainView[m_enClickedSubMenu], m_dicOfTimerInterval[m_enClickedSubMenu]);

            return true;
        }
        #endregion

        /// <summary>
        /// 2020.07.10 by twkang [ADD] AlamMessage Form 을 위한 상태를 바꿔준다.
        /// </summary>
        public void SetAlarmMessageForm(bool bShow)
		{
			m_RWLock.EnterWriteLock();
			queueState.Enqueue(bShow ? EN_ALARM_FORM_STATE.DISPLAYING : EN_ALARM_FORM_STATE.OFF);
			m_RWLock.ExitWriteLock();
		}
		/// <summary>
		/// 2021.06.14 by twkang [ADD] ProgressBar 폼을 설정해준다.
		/// </summary>
		public void SetProgressBarForm(bool bShow)
		{
			enProgressBarState = bShow ? EN_PROGRESS_BAR_FORM_STATE.DISPLAYING : EN_PROGRESS_BAR_FORM_STATE.OFF;
		}

        #region Secs/Gem 관련
		private Functional.Form_TerminalMessage m_instanceTerminal = Functional.Form_TerminalMessage.GetInstance();
		delegate void deleTerminalMessage_Called(string strMessage);
        private void EnqueueTerminalMessage(string strMessage)
        {
            if (this.InvokeRequired)
            {
                deleTerminalMessage_Called d = new deleTerminalMessage_Called(EnqueueTerminalMessage);
                this.BeginInvoke(d, new object[] { strMessage });
            }
            else
            {
                m_instanceTerminal.EnqueueTerminalMessage(strMessage);
            }
        }

        private Functional.Form_OperatorCall m_instanceOperatorCallForm = Functional.Form_OperatorCall.GetInstance();
		private delegate bool deleOperatorCall_Called(SECSGEM.DefineSecsGem.EN_OPCALL_LEVEL enLevel, string strOperatorId, bool bBuzzer, string strMsg);
		private bool ShowOperatorCallForm(SECSGEM.DefineSecsGem.EN_OPCALL_LEVEL enLevel, string strOperatorId, bool bBuzzer, string strMsg)
		{
			if (this.InvokeRequired)
			{
				deleOperatorCall_Called d = new deleOperatorCall_Called(ShowOperatorCallForm);
				this.BeginInvoke(d, new object[] { enLevel, strOperatorId, bBuzzer, strMsg });
			}
			else
			{
				return m_instanceOperatorCallForm.ShowMessage(enLevel, strOperatorId, bBuzzer, strMsg);
			}
			return true;
		}
        // 2021.10.06. jhlim [ADD] 델리게이트를 등록한다.
        private void RegistSecsGemFunctions()
        {
			var isntSecsGemHandler = SECSGEM.Communicator.SecsGemHandler.Instance;
			isntSecsGemHandler.LinkTerminalMessage(EnqueueTerminalMessage);
			isntSecsGemHandler.LinkShowOperatorCall(ShowOperatorCallForm);

			m_instanceTerminal.ShowForm(true);
			m_instanceTerminal.ShowForm(false);

			m_instanceOperatorCallForm.ShowForm(true);
			m_instanceOperatorCallForm.ShowForm(false);
        }

        // 2021.10.06. jhlim [ADD] 통신 및 시나리오를 초기화한다.
        private void InitScenarioHandler()
        {
            //SECSGEM.ProcessingScenario scenario = null;
            //SECSGEM.SecsGem driver = null;
            //         string cfgPath = "";
            //         string recipePath = string.Empty;

            //         Vision_.Vision.GetInstance().SetUseVision(false);

            //         // project에 맞게 설정
            //         Define.DefineEnumProject.AppConfig.EN_CUSTOMER customer = Work.AppConfigManager.Instance.Customer;
            //         switch (customer)
            //         {
            //             case Define.DefineEnumProject.AppConfig.EN_CUSTOMER.S_TP:
            //                 {
            //                     scenario = new SECSGEM.Scenario.ProcessingScenarioPWA500BIN_TP();
            //                     int clientIndex = 4;
            //                     //if (Work.AppConfigManager.Instance.ProcessModuleSimulation)
            //                     //{
            //                     //    clientIndex = 9;
            //                     //}
            //                     driver = new SECSGEM.SecsGemDll.XGemPro300WithWCF(1, new int[] { clientIndex });
            //                     cfgPath = string.Format(@"{0}{1}\{2}", SECSGEM.DefineSecsGem.PATH.FILE_PATH_CFG, customer, Work.AppConfigManager.Instance.ProcessType.ToString());                        
            //                     recipePath = string.Format(@"{0}\EFEM\RMS", Define.DefineConstant.FilePath.FILEPATH_RECIPE);//string.Format(@"\\127.0.0.1\EFEM\RMS");
            //                 }
            //                 break;

            //             case Define.DefineEnumProject.AppConfig.EN_CUSTOMER.S_NRD:
            //             case Define.DefineEnumProject.AppConfig.EN_CUSTOMER.S_NRD_300:
            //                 {
            //                     scenario = new SECSGEM.Scenario.ProcessingScenarioPWA500W_NRD();
            //                     int clientIndex = 6;
            //                     driver = new SECSGEM.SecsGemDll.XGemPro300WithWCF(1, new int[] { clientIndex });
            //                     cfgPath = string.Format(@"{0}{1}\{2}", SECSGEM.DefineSecsGem.PATH.FILE_PATH_CFG, customer, Work.AppConfigManager.Instance.ProcessType.ToString());
            //                     recipePath = string.Format(@"{0}\EFEM\RMS", Define.DefineConstant.FilePath.FILEPATH_RECIPE);
            //                     break;

            //                 }
            //             default: return; // 사용 안하는 조건이므로 정상 반환
            //         }

            //         if (scenario == null)
            //	return;	// 사용 안하는 조건이므로 정상 반환
            //bool scenarioInit = SECSGEM.ScenarioOperator.Instance.Initialize(scenario, driver, cfgPath, recipePath);

            bool scenarioInit = SECSGEM.ScenarioOperator.Instance.Initialize();

			if (scenarioInit)
			{
				FrameOfSystem3.Functional.PostOffice.GetInstance().SendMail(Define.DefineEnumProject.Mail.EN_SUBSCRIBER.ScenarioCirculator
					, Define.DefineEnumProject.Mail.EN_MAIL.ScenarioCurculatorRun, true);
			}

			RegistSecsGemFunctions();
		}

        // 2021.10.06. jhlim [ADD] 통신 및 시나리오를 종료한다.
        private void CloseScenarioHandler()
        {
			SECSGEM.ScenarioOperator.Instance.Exit();

            m_instanceTerminal.ExitForm();
        }
        #endregion

		// 2024.07.09 by junho [ADD] Form 생성 완료 event 추가
		public static event System.Action InitializeFinished = null;
    }
}
