using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using DesignPattern_.Observer_;
using EquipmentState_;

using FrameOfSystem3.Component;
using FrameOfSystem3.Config;
using FrameOfSystem3.Work;
using FrameOfSystem3.Recipe;
using Define.DefineEnumBase.Common;
using FrameOfSystem3.Views.Functional;
using FrameOfSystem3.SECSGEM.Communicator;
using FrameOfSystem3.SECSGEM.DefineSecsGem;

namespace FrameOfSystem3.Views.Operation
{
    public partial class Operation_SecsGem : UserControlForMainView.CustomView
    {
        #region <Constructors>
        public Operation_SecsGem()
        {
            InitializeComponent();

            InitializeSecsGem();

            InitializeView();

            _messageBox = Form_MessageBox.GetInstance();

            string btnName;
            switch (Work.AppConfigManager.Instance.ProcessType)
            {
                case Define.DefineEnumProject.AppConfig.EN_PROCESS_TYPE.BIN_SORTER:
                    btnName = "PWA500BIN";
                    break;
                case Define.DefineEnumProject.AppConfig.EN_PROCESS_TYPE.DIE_TRANSFER:
                case Define.DefineEnumProject.AppConfig.EN_PROCESS_TYPE.DIE_TRANSFER_300:
                    btnName = "PWA500W";
                    break;

                default:
                    btnName = "Customized";
                    break;
            }
            btnSubViewSecsGemCustomized.Text = btnName;

            SubViewButtons = new Dictionary<SubViewType, Sys3Controls.Sys3button>
            {
                { SubViewType.Common, btnSubViewSecsGemCommon },
                { SubViewType.EventList, btnSubViewSecsGemEventList },
                { SubViewType.Custom, btnSubViewSecsGemCustomized }
                //{ SubViewType.Custom, btnSubViewSecsGemCustom }
            };

            SubViewOperationSecsGemCommon = new SubViewOperationSecsGemCommon()
            {
                Dock = DockStyle.Fill
            };
            pnSubView.Controls.Add(SubViewOperationSecsGemCommon);

            SubViewOperationSecsGemEventList = new SubViewOperationSecsGemEventList()
            {
                Dock = DockStyle.Fill
            };
            SubViewOperationSecsGemEventList.Hide();
            pnSubView.Controls.Add(SubViewOperationSecsGemEventList);

            switch (AppConfigManager.Instance.Customer)
            {
                case Define.DefineEnumProject.AppConfig.EN_CUSTOMER.NONE:
                    break;
                case Define.DefineEnumProject.AppConfig.EN_CUSTOMER.S_TP:
                    {
                        SubViewOperationSecsGemCustomized = new EFEM.CustomizedByProcessType.UserInterface.OperationSecsGem.PWA500BIN.SubViewOperationSecsGemCustomized()
                        {
                            Dock = DockStyle.Fill
                        };
                        SubViewOperationSecsGemCustomized.Hide();
                        pnSubView.Controls.Add(SubViewOperationSecsGemCustomized);
                    }
                    break;
                case Define.DefineEnumProject.AppConfig.EN_CUSTOMER.S_NRD:
                case Define.DefineEnumProject.AppConfig.EN_CUSTOMER.S_NRD_300:
                    {
                        SubViewOperationSecsGemCustomized = new EFEM.CustomizedByProcessType.UserInterface.OperationSecsGem.PWA500W.SubViewOperationSecsGemCustomized()
                        {
                            Dock = DockStyle.Fill
                        };
                        SubViewOperationSecsGemCustomized.Hide();
                        pnSubView.Controls.Add(SubViewOperationSecsGemCustomized);
                    }
                    break;
                default:
                    break;
            }

            _selectedSubView = SubViewOperationSecsGemCommon;
        }
        #endregion </Constructors>

        #region <Fields>

        //private SecsGemConfig m_instanceScenario = SecsGemConfig.GetInstance();
        private SecsGemHandler _gemHandler = SecsGemHandler.Instance;
        private FrameOfSystem3.Recipe.Recipe _recipe = FrameOfSystem3.Recipe.Recipe.GetInstance();

        private EN_COMM_STATE _prevCommState = EN_COMM_STATE.DISABLED;
        private EN_CONTROL_STATE _prevControlState;

        private readonly StringBuilder LogToWrite = new StringBuilder(33000);
        private static Form_MessageBox _messageBox = null;

        private readonly Dictionary<SubViewType, Sys3Controls.Sys3button> SubViewButtons = null;
        private SubViewType _selectedSubViewType;
        private UserControlForMainView.CustomView _selectedSubView = null;
        private readonly SubViewOperationSecsGemCommon SubViewOperationSecsGemCommon = null;
        private readonly SubViewOperationSecsGemEventList SubViewOperationSecsGemEventList = null;
        private readonly UserControlForMainView.CustomView SubViewOperationSecsGemCustomized = null;
        #endregion </Fields>

        #region <Enum>
        private enum SubViewType
        {
            Common,
            EventList,
            Custom,
            //Custom,
        }
        #endregion </Enum>

        #region <Properties>
        private EQUIPMENT_STATE EnEqpState
        {
            get { return EquipmentState.GetInstance().GetState(); }
        }
        #endregion </Properties>

        #region <Methods>

        #region <Initialize>
        private void InitializeSecsGem()
        {
            _prevCommState = _gemHandler.GetCommState();
            lblCommunicationState.Text = _prevCommState.ToString();

            _gemHandler.AttachDisplayLog(DisplayLog);
        }
        private void InitializeView()
        {

        }
        #endregion </Initialize>

        #region <Override>
        protected override void ProcessWhenActivation()
        {
            base.ProcessWhenActivation();

            UpdateView();
        }
        protected override void ProcessWhenDeactivation()
        {
            if (_selectedSubView != null)
            {
                _selectedSubView.DeactivateView();
            }
        }
        public override void CallFunctionByTimer()
        {
            UpdateStatus();

            if (_selectedSubView != null)
            {
                _selectedSubView.CallFunctionByTimer();
            }
        }
        #endregion </Override>

        #region <UI Event>
        private void BtnShowTerminalMessegeBoxClicked(object sender, EventArgs e)
        {
            Form_TerminalMessage.GetInstance().ShowForm(true);
        }
        private void ControlStateChangeClicked(object sender, EventArgs e)
        {
            if (EQUIPMENT_STATE.PAUSE != EnEqpState && EQUIPMENT_STATE.IDLE != EnEqpState)
                return;

            if (!(sender is Sys3Controls.Sys3button btn))
                return;

            EN_CONTROL_STATE controlState;
            if (false == Enum.TryParse(btn.Tag.ToString(), out controlState))
                return;

            if (_prevControlState.Equals(controlState))
                return;

            _gemHandler.SetControlState(controlState);
        }
        private void MaintModeSettingClicked(object sender, EventArgs e)
        {
            if (EQUIPMENT_STATE.PAUSE != EnEqpState && EQUIPMENT_STATE.IDLE != EnEqpState)
                return;

            bool pmMode = _gemHandler.MaintenanceMode;
            string message = pmMode ? String.Format("PM 모드를 해제하시겠습니까?")
                : String.Format("PM 모드로 설정하시겠습니까?");
            if (_messageBox.ShowMessage(message, "PM MODE SETTING"))
            {
                _gemHandler.MaintenanceMode = !pmMode;
            }
        }
        private void BtnSubViewClicked(object sender, EventArgs e)
        {
            if (!(sender is Sys3Controls.Sys3button btn)) return;

            foreach (var item in SubViewButtons)
            {
                if (item.Value.Equals(btn))
                {
                    SelectSubView(item.Key);

                    item.Value.ButtonClicked = true;
                    item.Value.MainFontColor = Color.White;
                }
                else
                {
                    item.Value.ButtonClicked = false;
                    item.Value.MainFontColor = Color.DarkBlue;
                }
            }

            DisplaySubPanel();
        }
        private void BtnClearLogClicked(object sender, EventArgs e)
        {
            ClearLog();
        }
        #endregion </UI Event>

        #region <Log>
        delegate void deleDisplayLog_Called(string strMessage);
        private void DisplayLog(string strMessage)
        {
            if (this.InvokeRequired)
            {
                deleDisplayLog_Called d = new deleDisplayLog_Called(DisplayLog);
                this.BeginInvoke(d, new object[] { strMessage });
            }
            else
            {
                LogToWrite.Insert(0, String.Format("{0}\r\n", strMessage));
                if (LogToWrite.Length >= txt_Log.MaxLength)
                {
                    txt_Log.Clear();
                    LogToWrite.Clear();
                    LogToWrite.Length = 0;
                    LogToWrite.AppendFormat("{0}\r\n", strMessage);
                }

                txt_Log.Text = LogToWrite.ToString();
            }
        }

        private void ClearLog()
        {
            txt_Log.Clear();
            LogToWrite.Clear();
        }
        #endregion </Log>

        #region <Display>
        private void SelectSubView(SubViewType subViewType)
        {
            _selectedSubViewType = subViewType;
        }
        private void DisplaySubPanel()
        {
            if (_selectedSubView == null)
                return;

            _selectedSubView.Hide();
            _selectedSubView.DeactivateView();

            switch (_selectedSubViewType)
            {
                case SubViewType.Common:                
                    _selectedSubView = SubViewOperationSecsGemCommon;
                    break;
                case SubViewType.EventList:
                    _selectedSubView = SubViewOperationSecsGemEventList;
                    break;
                case SubViewType.Custom:
                    _selectedSubView = SubViewOperationSecsGemCustomized;
                    break;
                default:
                    break;
            }

            if (_selectedSubView == null)
                return;

            _selectedSubView.ActivateView();
            _selectedSubView.Show();
        }
        private void UpdateStatus()
        {
            if (_prevCommState != _gemHandler.GetCommState())
            {
                _prevCommState = _gemHandler.GetCommState();
                lblCommunicationState.Text = _prevCommState.ToString();

                switch (_prevCommState)
                {
                    case EN_COMM_STATE.COMMUNICATING:
                        lblCommunicationState.BackGroundColor = Color.LimeGreen;
                        break;
                    case EN_COMM_STATE.WAIT_CRA:
                        lblCommunicationState.BackGroundColor = Color.DarkOrange;
                        break;
                    default:
                        lblCommunicationState.BackGroundColor = Color.Transparent;
                        break;
                }
            }

            if (_gemHandler.MaintenanceMode)
            {
                lblMaintMode.BackGroundColor = Color.Salmon;
                lblMaintMode.Text = "PM - ON";
            }
            else
            {
                lblMaintMode.BackGroundColor = Color.LightGray;
                lblMaintMode.Text = "PM - OFF";
            }

            EN_CONTROL_STATE newState = _gemHandler.GetControlState();
            DisplayUIForSecsGemControlState(newState);
        }
        private void UpdateView()
        {
            _selectedSubView.ActivateView();
        }
        private void DisplayUIForSecsGemControlState(EN_CONTROL_STATE state)
        {
            if (false == state.Equals(_prevControlState))
            {
                _prevControlState = state;

                switch (state)
                {
                    case EN_CONTROL_STATE.OFFLINE:
                    case EN_CONTROL_STATE.ATTEMP_ONLINE:
                    case EN_CONTROL_STATE.HOST_OFFLINE:
                        {
                            btnOnlineRemote.GradientFirstColor = Color.LightGray;
                            btnOnlineRemote.GradientSecondColor = Color.LightGray;

                            btnOnlineLocal.GradientFirstColor = Color.LightGray;
                            btnOnlineLocal.GradientSecondColor = Color.LightGray;

                            btnOffline.GradientFirstColor = Color.Gray;
                            btnOffline.GradientSecondColor = Color.Gray;

                            btnOnlineRemote.ButtonClicked = false;
                            btnOnlineLocal.ButtonClicked = false;
                            btnOffline.ButtonClicked = true;
                        }
                        break;
                    case EN_CONTROL_STATE.LOCAL:
                        {
                            btnOnlineRemote.GradientFirstColor = Color.LightGray;
                            btnOnlineRemote.GradientSecondColor = Color.LightGray;

                            btnOnlineLocal.GradientFirstColor = Color.Orange;
                            btnOnlineLocal.GradientSecondColor = Color.Orange;

                            btnOffline.GradientFirstColor = Color.LightGray;
                            btnOffline.GradientSecondColor = Color.LightGray;


                            btnOnlineRemote.ButtonClicked = false;
                            btnOnlineLocal.ButtonClicked = true;
                            btnOffline.ButtonClicked = false;
                        }
                        break;
                    case EN_CONTROL_STATE.REMOTE:
                        {
                            btnOnlineRemote.GradientFirstColor = Color.Lime;
                            btnOnlineRemote.GradientSecondColor = Color.Lime;

                            btnOnlineLocal.GradientFirstColor = Color.LightGray;
                            btnOnlineLocal.GradientSecondColor = Color.LightGray;

                            btnOffline.GradientFirstColor = Color.LightGray;
                            btnOffline.GradientSecondColor = Color.LightGray;

                            btnOnlineRemote.ButtonClicked = true;
                            btnOnlineLocal.ButtonClicked = false;
                            btnOffline.ButtonClicked = false;
                        }
                        break;
                }
            }

        }
        #endregion </Display>

        #endregion </Methods>
    }
}
