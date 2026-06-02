using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using TickCounter_;
using RunningMain_;
using TaskAction_;

using FrameOfSystem3.Work;

namespace FrameOfSystem3.Views.Operation
{
	public partial class Operation_Main : UserControlForMainView.CustomView
	{
		public Operation_Main()
		{
			InitializeComponent();

			MakeMappingTable();

			m_InstanceOfSelectionList = Functional.Form_SelectionList.GetInstance();
			
			InitializeSubPanels();

            MonitoringButtons = new Dictionary<MonitoringSubPanelType, Sys3Controls.Sys3button>
            {
                [MonitoringSubPanelType.Digital] = btnMonitoringDigital,
                [MonitoringSubPanelType.Analog] = btnMonitoringAnalog
            };
            
            #region <Monitoring SubPanels>
            MonitoringSubPanelList = new Dictionary<MonitoringSubPanelType, UserControlForMainView.CustomView>();

            var processType = AppConfigManager.Instance.ProcessType;
            switch (processType)
            {
                case Define.DefineEnumProject.AppConfig.EN_PROCESS_TYPE.BIN_SORTER:
                    {
                        MonitoringSubPanelList.Add(MonitoringSubPanelType.Digital, new EFEM.CustomizedByProcessType.UserInterface.OperationMainMonitoring.PWA500BIN.MainMonitoringSubPanel500BINDigitals());
                        MonitoringSubPanelList.Add(MonitoringSubPanelType.Analog, new EFEM.CustomizedByProcessType.UserInterface.OperationMainMonitoring.PWA500BIN.MainMonitoringSubPanel500BINAnalogs());
                    }
                    break;
                case Define.DefineEnumProject.AppConfig.EN_PROCESS_TYPE.DIE_TRANSFER:
                case Define.DefineEnumProject.AppConfig.EN_PROCESS_TYPE.DIE_TRANSFER_300:
                    {
                        MonitoringSubPanelList.Add(MonitoringSubPanelType.Digital, new EFEM.CustomizedByProcessType.UserInterface.OperationMainMonitoring.PWA500W.MainMonitoringSubPanel500WDigitals());
                        MonitoringSubPanelList.Add(MonitoringSubPanelType.Analog, new EFEM.CustomizedByProcessType.UserInterface.OperationMainMonitoring.PWA500W.MainMonitoringSubPanel500WAnalogs());
                    }
                    break;
                default:
                    break;
            }

            foreach (var item in MonitoringSubPanelList)
            {
                pnMonitoring.Controls.Add(item.Value);
            }

            if (MonitoringSubPanelList.Count > 0)
            {
                _selectedMonitoringPanel = MonitoringSubPanelList[_selectedMonitoringPanelMode];
            }
            #endregion </Monitoring SubPanels>

            btnTest.Visible = System.Diagnostics.Debugger.IsAttached;
            DisplayMonitoringSubPanel();
		}

		#region 상수
		private readonly string STR_INITALIZE_TASK					= "INITIALIZE TASK";
		#endregion

		#region <Enums>
		private enum MonitoringSubPanelType
        {
			Digital,
			Analog
        }
		private enum MainDisplaySubPanels
        {
			Summary,
			ManualOperation,
		}
		#endregion </Enums>

		#region 변수
		Dictionary<string, RUN_MODE> m_DicForRunMode	= new Dictionary<string,RUN_MODE>();
		
		Functional.Form_SelectionList m_InstanceOfSelectionList		= null;
        Functional.Form_MessageBox _messageBox = null;
		//private TickCounter _operationHoldingTicks = null;
		private string _runMode = RUN_MODE.AUTO.ToString();
        
        private readonly Dictionary<MonitoringSubPanelType, UserControlForMainView.CustomView> MonitoringSubPanelList = null;
		private readonly Dictionary<MonitoringSubPanelType, Sys3Controls.Sys3button> MonitoringButtons = null;
        private UserControlForMainView.CustomView _selectedMonitoringPanel = null;
        private MonitoringSubPanelType _selectedMonitoringPanelMode = MonitoringSubPanelType.Digital;

        #region Task
        Task.TaskOperator m_instanceOperator						= null;
		private int m_nCountOfTask									= 0;
		string[] m_arTaskList										= null;
		#endregion

		#region Action
		TaskActionFlow m_instanceActionFlow				= null;
		#endregion

		#region <SubViews>
		private readonly PanelInterface PanelInstance = new PanelInterface();
		#endregion </SubViews>

		#endregion

		#region 상속 인터페이스
		/// <summary>
		/// 
		/// </summary>
		protected override void ProcessWhenActivation()
		{
			m_instanceOperator		= Task.TaskOperator.GetInstance();
            _messageBox = Functional.Form_MessageBox.GetInstance();

			if(m_instanceOperator.GetListOfTask(ref m_nCountOfTask,ref m_arTaskList))
			{

			}

			m_instanceActionFlow		= TaskActionFlow.GetInstance();

			PanelInstance.ProcessWhenActivation();
            if (_selectedMonitoringPanel != null)
            {
                _selectedMonitoringPanel.ActivateView();
            }

        }
		/// <summary>
		/// 
		/// </summary>
		protected override void ProcessWhenDeactivation()
		{
            if (_selectedMonitoringPanel != null)
            {
                _selectedMonitoringPanel.DeactivateView();
            }

            PanelInstance.ProcessWhenDeactivation();
		}

        /// <summary>
		/// 
		/// </summary>
		public override void CallFunctionByTimer()
		{
			PanelInstance.CallFunctionByTimer();

            if (_selectedMonitoringPanel != null)
            {
                _selectedMonitoringPanel.CallFunctionByTimer();
            }
        }

        #endregion

        #region 내부인터페이스
        
		#region <SubViews>
		private void InitializeMonitoringSubPanels()
        {

        }
        private void InitializeSubPanels()
        {
            var addPanelList = new Dictionary<string, List<ParameterGroupPanel>>();

            switch (AppConfigManager.Instance.ProcessType)
            {
                case Define.DefineEnumProject.AppConfig.EN_PROCESS_TYPE.NONE:
                case Define.DefineEnumProject.AppConfig.EN_PROCESS_TYPE.BIN_SORTER:
                    {
                        // UI 할당
                        AddControlToPanelInterface(MainDisplaySubPanels.Summary,
                            new EFEM.CustomizedByProcessType.UserInterface.OperationMainSummary.PWA500BIN.MainDisplaySubPanelSummary500BIN(MainDisplaySubPanels.Summary.ToString()),
                            ref addPanelList);
                        AddControlToPanelInterface(MainDisplaySubPanels.ManualOperation,
                            new EFEM.CustomizedByProcessType.UserInterface.OperationMainManual.PWA500BIN.MainDisplaySubPanelManualOperation500BIN(MainDisplaySubPanels.ManualOperation.ToString()),
                            ref addPanelList);
                    }
                    break;
                case Define.DefineEnumProject.AppConfig.EN_PROCESS_TYPE.DIE_TRANSFER:
                case Define.DefineEnumProject.AppConfig.EN_PROCESS_TYPE.DIE_TRANSFER_300:
                    {
                        // UI 할당
                        AddControlToPanelInterface(MainDisplaySubPanels.Summary,
                            new EFEM.CustomizedByProcessType.UserInterface.OperationMainSummary.PWA500W.MainDisplaySubPanelSummary500W(MainDisplaySubPanels.Summary.ToString()),
                            ref addPanelList);
                        AddControlToPanelInterface(MainDisplaySubPanels.ManualOperation,
                            new EFEM.CustomizedByProcessType.UserInterface.OperationMainManual.PWA500W.MainDisplaySubPanelManualOperation500W(MainDisplaySubPanels.ManualOperation.ToString()),
                            ref addPanelList);
                    }
                    break;
                default:
                    break;
            }

            // 버튼 할당
            var tabButtonList = new Dictionary<Sys3Controls.Sys3button, string>
            {
                { btnSubPanelSummary, MainDisplaySubPanels.Summary.ToString() },
                { btnSubPanelManualOperation, MainDisplaySubPanels.ManualOperation.ToString() },
            };

            PanelInstance.InitializeSubPanels(pnSubMainView, addPanelList, tabButtonList);
        }
		private void AddControlToPanelInterface(MainDisplaySubPanels panelType, ParameterPanel panelControl, ref Dictionary<string, List<ParameterGroupPanel>> panelList)
        {
            string panelSummaryName = panelType.ToString();
            var lpSummaryPanel = new ParameterGroupPanel(panelControl, false, true);
            
            lpSummaryPanel.Dock = DockStyle.Fill;
			lpSummaryPanel.DisableGroupBox();	// 그룹박스 미사용
			
			panelList.Add(panelSummaryName, new List<ParameterGroupPanel>());
            panelList[panelSummaryName].Add(lpSummaryPanel);
        }
        #endregion </SubViews>

        /// <summary>
        /// 2020.07.20 by twkang [ADD] 클래스에서 사용할 MappingTable 을 만든다.
        /// </summary>
        private void MakeMappingTable()
		{
			m_DicForRunMode.Clear();
			foreach(RUN_MODE en in Enum.GetValues(typeof(RUN_MODE)))
			{
				m_DicForRunMode.Add(en.ToString(), en);
			}
		}
		/// <summary>
		/// 2020.09.15 by twkang [ADD] 이니셜라이즈 동작할 테스크를 선택한다.
		/// </summary>
		private bool SetInitializeTask()
		{
			string[] arTask					= null;
			int[] arIndex					= null;
			bool[] arInit					= null;
			string strPreValue				= string.Empty;
			List<string> listInitializeTask	= new List<string>();
			
            if (false == Task.TaskOperator.GetInstance().GetInitializingTask(ref arTask, ref arInit))
			{
				return false;
			}

			for(int nIndex = 0, nEnd = arInit.Length; nIndex < nEnd; ++nIndex)
			{
				// 2023.05.19 by junho [ADD] Initialize button 눌렀을 때에는 항상 모든 Task가 선택되어 있도록 변경
				arInit[nIndex] = false;
				
				if(false == arInit[nIndex])
				{
					listInitializeTask.Add(arTask[nIndex]);
				}
			}
			strPreValue	= string.Join(", ", listInitializeTask);

			if(m_InstanceOfSelectionList.CreateForm(STR_INITALIZE_TASK, arTask, strPreValue, true))
			{
				m_InstanceOfSelectionList.GetResult(ref arIndex);

                for(int nIndex = 0, nEnd = arInit.Length; nIndex < nEnd; ++nIndex)
				{
					arInit[nIndex]	        = false;
				}

				for(int nIndex = 0, nEnd = arIndex.Length; nIndex < nEnd; ++nIndex)
				{
					arInit[arIndex[nIndex]]	= true;
				}

				// Global 강제 할당 : Utility를 위함
				arInit[(int)Define.DefineEnumProject.Task.EN_TASK_LIST.Global] = true;
				arInit[(int)Define.DefineEnumProject.Task.EN_TASK_LIST.AtmRobot] = true;

				return Task.TaskOperator.GetInstance().SetInitializingTask(ref arTask, ref arInit);
			}
			return false;
		}

        private void InitializeUIEventForGroupBox()
        {
   //         gbOperationButton.MouseDown += groupBox_OperationButtons_MouseDown;
			//gbOperationButton.MouseUp += groupBox_OperationButtons_MouseUp;
			//_operationHoldingTicks = new TickCounter();

			gbOperationButton.Text = string.Format("OPERATION - {0} MODE", _runMode);
		}
        #endregion

        #region UI 이벤트
        private void Operation_Main_Load(object sender, EventArgs e)
        {
            InitializeUIEventForGroupBox();
        }


        private void Click_RunMode(object sender, EventArgs e)
        {
            if (false == EquipmentState_.EquipmentState.GetInstance().GetState().Equals(EquipmentState_.EQUIPMENT_STATE.IDLE)
                && false == EquipmentState_.EquipmentState.GetInstance().GetState().Equals(EquipmentState_.EQUIPMENT_STATE.PAUSE))
                return;

            string strValue = string.Empty;

            if (m_InstanceOfSelectionList.CreateForm("RUN MODE", Define.DefineEnumProject.SelectionList.EN_SELECTIONLIST.OPERATION_EQUIPMENT, _runMode))
            {
                m_InstanceOfSelectionList.GetResult(ref strValue);

                if (m_DicForRunMode.ContainsKey(strValue))
                {
                    m_instanceOperator.SetRunMode(m_DicForRunMode[strValue]);
                    _runMode = m_instanceOperator.GetRunMode().ToString();
                }

				m_labelRunMode.Text = strValue;
            }

            gbOperationButton.Text = string.Format("OPERATION - {0} MODE", _runMode);
        }

        /// <summary>
        /// 2020.06.03 by twkang [ADD] INITIALIZE, RUN, STOP 버튼 클릭 이벤트이다. SelectionList 를 호출한다.
        /// </summary>
        private async void Click_OperationButton(object sender, EventArgs e)
		{
			Control ctrl = sender as Control;

			switch (ctrl.TabIndex)
			{
				case 0: // INITIALIZE
					if (SetInitializeTask())
					{
						Task.TaskOperator.GetInstance().SetOperation(OPERATION_EQUIPMENT.INITIALIZE);
					}
					break;
				case 1: // RUN
                    if (false == (await Task.TaskOperator.GetInstance().ProcessBeforeAutorun()))
                    {
                        return;
                    }

                    Task.TaskOperator.GetInstance().SetOperation(OPERATION_EQUIPMENT.RUN);
					break;
				case 2: // STOP
					Task.TaskOperator.GetInstance().SetOperation(OPERATION_EQUIPMENT.STOP);
					return;
			}
		}
        /// <summary>
        /// 2020.07.20 by twkang [ADD] RunMode label 클릭 이벤트, 동작 모드를 설정한다.
        /// </summary>

        //     private void groupBox_OperationButtons_MouseDown(object sender, MouseEventArgs e)
        //     {
        //if (false == EquipmentState_.EquipmentState.GetInstance().GetState().Equals(EquipmentState_.EQUIPMENT_STATE.IDLE)
        //	&& false == EquipmentState_.EquipmentState.GetInstance().GetState().Equals(EquipmentState_.EQUIPMENT_STATE.PAUSE))
        //	return;

        //_operationHoldingTicks.SetTickCount(1500);
        //     }

        //     private void groupBox_OperationButtons_MouseUp(object sender, MouseEventArgs e)
        //     {
        //         if (false == EquipmentState_.EquipmentState.GetInstance().GetState().Equals(EquipmentState_.EQUIPMENT_STATE.IDLE)
        //             && false == EquipmentState_.EquipmentState.GetInstance().GetState().Equals(EquipmentState_.EQUIPMENT_STATE.PAUSE))
        //             return;

        //         if (false == _operationHoldingTicks.IsTickOver(true))
        //             return;

        //         string strValue = string.Empty;

        //         if (m_InstanceOfSelectionList.CreateForm("RUN MODE", Define.DefineEnumProject.SelectionList.EN_SELECTIONLIST.OPERATION_EQUIPMENT, _runMode))
        //         {
        //             m_InstanceOfSelectionList.GetResult(ref strValue);

        //             if (m_DicForRunMode.ContainsKey(strValue))
        //             {
        //                 m_instanceOperator.SetRunMode(m_DicForRunMode[strValue]);
        //                 _runMode = m_instanceOperator.GetRunMode().ToString();
        //             }
        //         }

        //         gbOperationButton.Text = string.Format("OPERATION - {0} MODE", _runMode);
        //         // After 30s, auto change UI to manual
        //         //m_tickCounterGroupBoxHold.SetTickCount(30000);
        //     }

        #region <SubView>
        private void DisplayMonitoringSubPanel()
        {
            if (false == MonitoringSubPanelList.ContainsKey(_selectedMonitoringPanelMode))
                return;

            if (_selectedMonitoringPanel.Equals(MonitoringSubPanelList[_selectedMonitoringPanelMode]))
                return;

            foreach (var item in MonitoringSubPanelList)
            {
                if (item.Key.Equals(_selectedMonitoringPanelMode))
                {
                    _selectedMonitoringPanel = item.Value;

                    item.Value.ActivateView();
                    item.Value.Show();
                }
                else
                {
                    item.Value.Hide();
                    item.Value.DeactivateView();
                }
            }
        }

        private void BtnMonitoringSubPanelClicked(object sender, EventArgs e)
        {
            if (!(sender is Sys3Controls.Sys3button btn)) return;

            foreach (var item in MonitoringButtons)
            {
                if (item.Value.Equals(btn))
                {
                    if (false == MonitoringSubPanelList.ContainsKey(item.Key))
                        return;

                    if (item.Value.ButtonClicked)
                        return;

                    _selectedMonitoringPanelMode = item.Key;
                    break;
                }
            }

            foreach (var item in MonitoringButtons)
            {
                if (item.Value.Equals(btn))
                {
                    item.Value.ButtonClicked = true;
                    item.Value.MainFontColor = Color.White;
                }
                else
                {
                    item.Value.ButtonClicked = false;
                    item.Value.MainFontColor = Color.DarkBlue;
                }
            }

            DisplayMonitoringSubPanel();
        }

		private void BtnSubPanelClicked(object sender, EventArgs e)
        {
            if (!(sender is Sys3Controls.Sys3button btn)) return;

            if (false == PanelInstance.Click_TabButton(btn))
            {
                _messageBox.ShowMessage("Not ready page...");
                return;
            }
        }
		#endregion </SubView>

		#endregion        
        private void GetDuraportStatus(string status)
        {
            const int PresentMask = 1 << 29;
            const int PlacedMask = 1 << 28;
            //const int UndockedMask = 1 << 12;
            const int DockedMask = 1 << 11;
            //const int UnclampedMask = 1 << 10;
            const int ClampedMask = 1 << 9;
            //const int DoorClosedMask = 1 << 3;
            const int DoorOpenMask = 1 << 2;

            const int InitializedMask = 1 << 0;
            //const int FoupModeMask = 1 << 7;
            //const int CassetteModeMask = 1 << 8;
            //const int AutoModeMask = 1 << 23;

            if (status.StartsWith("S"))
            {
                status = status.Substring(1);
            }
            int states = 0;
            try
            {
                states = int.Parse(status, System.Globalization.NumberStyles.HexNumber);
            }
            catch (Exception)
            {
                return;
            }

            // Present
            var _temporaryPresent = (states & PresentMask) != 0;

            // Placed
            var _temporaryPlaced = (states & PlacedMask) != 0;

            // Clamped
            var _temporaryClamped = (states & ClampedMask) != 0;

            // Docked
            var _temporaryDocked = (states & DockedMask) != 0;

            // Door
            var _temporaryDoorState = (states & DoorOpenMask) != 0;

            // Initialized
            var _temporaryInitialized = (states & InitializedMask) != 0;

            Console.WriteLine($"Presence : { _temporaryPresent}, " +
                $"Placed : {_temporaryPlaced}, " +
                $"Clamped : {_temporaryClamped}, " +
                $"Docked : {_temporaryDocked}, " +
                $"Door : {_temporaryDoorState}, " +
                $"Initialized : {_temporaryInitialized}");
        }

        //static string CreateMessage(string transactionId, string message)
        //{
        //    var sb = new StringBuilder();

        //    // 2025.07.29 sjkim: 실제 보내주는 Message
        //    byte[] byCommandLength = new byte[4];
        //    byCommandLength = GetCommandLength(message);
        //    for (int i = 0; i < byCommandLength.Length; i++)
        //    {
        //        sb.Append(byCommandLength[i]);
        //    }
        //    sb.Append(transactionId);
        //    //sb.Append(" ");
        //    sb.Append(message);

        //    return sb.ToString();
        //}

        //// 2025.07.29 sjkim: High Low Bit Calculation
        //private static byte[] GetCommandLength(string Command)
        //{
        //    string strMsg = Command;
        //    int nLength = strMsg.Length;

        //    var parts = SplitUInt((uint)nLength);

        //    byte[] byteLength = new byte[4];

        //    byteLength[0] = parts.b1;
        //    byteLength[1] = parts.b2;
        //    byteLength[2] = parts.b3;
        //    byteLength[3] = parts.b4;

        //    return byteLength;
        //}

        //// 2025.07.29 sjkim: High Low Bit Calculation
        //public static (byte b1, byte b2, byte b3, byte b4) SplitUInt(uint value)
        //{
        //    byte b1 = (byte)(value & 0xFF);
        //    byte b2 = (byte)((value >> 8) & 0xFF);
        //    byte b3 = (byte)((value >> 16) & 0xFF);
        //    byte b4 = (byte)((value >> 24) & 0xFF);
        //    return (b1, b2, b3, b4);
        //}

        private const uint MaskValidity = (1u << 7) | (1u << 8) | (1u << 27);
        private bool GetLoadingMode(uint statusValue, out EFEM.Defines.LoadPort.LoadPortLoadingMode mode)
        {
            mode = EFEM.Defines.LoadPort.LoadPortLoadingMode.Unknown;
            uint selected = statusValue & MaskValidity;

            if (selected == 0)
            {
                return false;
            }

            if ((selected & (selected - 1)) != 0)
            {
                mode = EFEM.Defines.LoadPort.LoadPortLoadingMode.Unknown;
                return true;
            }

            if (selected == (1u << 7))
            {
                mode = EFEM.Defines.LoadPort.LoadPortLoadingMode.Foup;
                return true;
            }

            if (selected == (1u << 8))
            {
                mode = EFEM.Defines.LoadPort.LoadPortLoadingMode.Cassette;
                return true;
            }

            mode = EFEM.Defines.LoadPort.LoadPortLoadingMode.ClosedCassette;
            return true;
        }

        private uint ConvertFromString(string stateString)
        {
            if (stateString.StartsWith("S"))
                stateString = stateString.Substring(1);

            uint.TryParse(stateString, System.Globalization.NumberStyles.AllowHexSpecifier,
                System.Globalization.CultureInfo.InvariantCulture,
                out var value);
            
            return value;
        }
        private void btnTest_Click(object sender, EventArgs e)
		{
			if (false == System.Diagnostics.Debugger.IsAttached)
				return;

            #region <E87ServiceTest>
            //EFEM.Jobs.Manager.IJobManager manager = EFEM.Jobs.Manager.JobManager.Instance;
            //var cj = manager.GetAllControlJobs();
            //foreach (var item in cj)
            //{
            //    if (manager.IsHeadOfQueueControlJob(item.Id) &&
            //        item.State == EFEM.Defines.Job.ControlJobState.Queued)
            //    {
            //        manager.RequestControlJobSelect(item.Id);
            //        break;
            //    }
            //}
            //var pj = manager.GetAllProcessJobs();
            //foreach (var item in pj)
            //{
            //    if (item.State == EFEM.Defines.Job.ProcessJobState.JobQueued)
            //    {
            //        manager.NotifyProcessJobSettingUpStarted(item.Id);
            //        break;
            //    }
            //    if (item.State == EFEM.Defines.Job.ProcessJobState.WaitingForStart)
            //    {
            //        manager.RequestProcessJobCommand(item.Id, EFEM.Defines.Job.ProcessJobCommand.Start);
            //        break;
            //    }
            //    if (item.State == EFEM.Defines.Job.ProcessJobState.Processing)
            //    {
            //        manager.SetProcessJobState(item.Id, EFEM.Defines.Job.ProcessJobState.ProcessComplete);
            //        break;
            //    }
            //}

            //int testIndex = 5;
            //EFEM.Modules.LoadPortManager.Instance.SetLoadPortEnabled(testIndex, true);
            //SECSGEM.ScenarioOperator.Instance.Carrier.SetCarrierIdentifier("LP1", "CARRIER01", 1);

            //SECSGEM.ScenarioOperator.Instance.ProcessJob.SetState("PJOB01", SECSGEM.SecsGemSDK.Gem300.ProcessJobState.JobCanceled);

            //SECSGEM.ScenarioOperator.Instance.ControlJob.RequestSelect("CJOB01");
            //SECSGEM.ScenarioOperator.Instance.ProcessJob.NotifySettingUpStarted("PJOB01");

            //EFEM.Jobs.Manager.JobManager.Instance.NotifyProcessJobSettingUpStarted("PJOB01");
            //var result = SECSGEM.ScenarioOperator.Instance.ProcessJob.NotifySettingUpCompleted("PJOB01");
            //var result = SECSGEM.ScenarioOperator.Instance.ProcessJob.RequestCommand("PJOB01", EFEM.Defines.Job.ProcessJobCommand.Pause);
            //SECSGEM.ScenarioOperator.Instance.ProcessJob.SetState("PJOB01", EFEM.Defines.Job.ProcessJobState.ProcessComplete);
            //SECSGEM.ScenarioOperator.Instance.ControlJob.RequestCommand("CJOB01", SECSGEM.SecsGemSDK.Gem300.ControlJobCommand.Stop, null, null);

            #endregion </E87ServiceTest>

            //uint states;

            //states = ConvertFromString("7342F5AB");
            //GetLoadingMode(states, out var mode);

            //states = ConvertFromString("S7342F52B");
            //GetLoadingMode(states, out mode);

            //states = ConvertFromString("S7B42F42B");
            //GetLoadingMode(states, out mode);


            //GetDuraportStatus("S31D40BB3");
            //GetDuraportStatus("S71D40BB3");
            //GetDuraportStatus("S71D403B3");
            //GetDuraportStatus("S71D413B3");
            //GetDuraportStatus("S61D413B3");
            //Console.WriteLine("Signal Changed : CarrierPlacement -> False");
            //GetDuraportStatus("S61D411B3");
            //GetDuraportStatus("S61D415B3");
            //Console.WriteLine("Signal Changed : CarrierPlacement -> True");
            //GetDuraportStatus("S61D415AB");
            //GetDuraportStatus("S71D415AB");
            //GetDuraportStatus("S31D40BA7");

            //string body = string.Empty;
            //for(int i = 0; i < 2336; ++i)
            //{
            //    body += 1;
            //}

            //var result = CreateMessage("6", body);

            //byte[] bodyeBytes = Encoding.UTF8.GetBytes(body);
            //int bodyLength = bodyeBytes.Length;
            //byte[] message = new byte[8 + bodyLength];
            //BitConverter.GetBytes(bodyLength).CopyTo(message, 0);
            //BitConverter.GetBytes(6).CopyTo(message, 4);

            //Buffer.BlockCopy(bodyeBytes, 0, message, 8, bodyLength);
            //StringBuilder sb = new StringBuilder();
            //for(int i = 0; i < message.Length; ++i)
            //{
            //    sb.Append(message[i]);
            //}


            //FrameOfSystem3.SECSGEM.Communicator.SecsGemHandler.Instance.SetAlarm(1010000);
            //FrameOfSystem3.SECSGEM.Communicator.SecsGemHandler.Instance.UpdateECVParameter(100012, "999999");
            //string oldName = "CARRIER06_LP6.01";
            //string newName = "TEMP";
            //EFEM.MaterialTracking.Substrate substrate = new EFEM.MaterialTracking.Substrate("");
            //EFEM.MaterialTracking.SubstrateManager.Instance.GetSubstrateAtProcessModule(newName, ref substrate);
            //substrate.SetName(oldName);

            //string path = @"\\127.0.0.1\efem\RMS\Upload\Test_Recipe";
            //byte[] byteArrayToUpload = System.IO.File.ReadAllBytes(path);
            //string writeLog = string.Empty;
            //for(int i = 0; i < byteArrayToUpload.Length; ++i)
            //{
            //    if (string.IsNullOrEmpty(writeLog))
            //    {
            //        writeLog = byteArrayToUpload[i].ToString();
            //    }
            //    else
            //    {
            //        writeLog = string.Format("{0} {1}", writeLog, byteArrayToUpload[i]);
            //    }
            //}
            //Console.WriteLine(writeLog);
            //Task.TaskOperator.GetInstance().MakeTaskAlarms();

            //return;
            //string[] name = { Define.DefineEnumProject.Task.EN_TASK_LIST.AtmRobot.ToString() };
            //string[] action = { "GEM_SIMUL" };
            //Task.TaskOperator.GetInstance().SetOperation(ref name, ref action);
            //SECSGEM.Communicator.SecsGemHandler.Instance.UpdateECVParameter(10000, "TEST1");
            //return;
            //return;
            //EFEM.MaterialTracking.SubstrateManager.Instance.SaveRecoveryDataAll();
            //EFEM.Modules.ProcessModuleGroup.Instance.SaveRecoveryDataAll();
            //EFEM.Modules.AtmRobotManager.Instance.RemoveSubstrateAll(0);
            //return;

            // 알람 정보를 등록한다.

            //Task.TaskOperator.GetInstance().MakeTaskAlarms();
        }
    }
}