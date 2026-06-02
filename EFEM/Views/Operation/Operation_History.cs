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

namespace FrameOfSystem3.Views.Operation
{
    public partial class Operation_History : UserControlForMainView.CustomView
    {
        #region <Constructors>
        public Operation_History()
        {
            InitializeComponent();

            _messageBox = Form_MessageBox.GetInstance();

            SubViewButtons = new Dictionary<SubViewType, Sys3Controls.Sys3button>
            {
                { SubViewType.CurrentWorking, btnSubViewCurrentWorking },
                { SubViewType.History, btnSubViewHistory },
            };

            switch (AppConfigManager.Instance.ProcessType)
            {
                case Define.DefineEnumProject.AppConfig.EN_PROCESS_TYPE.NONE:
                    break;
                case Define.DefineEnumProject.AppConfig.EN_PROCESS_TYPE.BIN_SORTER:
                    {
                        SubViewOperationHistoryCurruntWorking = new EFEM.CustomizedByProcessType.UserInterface.OperationMainHistory.PWA500BIN.SubViewOperationHistoryCurruntWorking500BIN();
                        SubViewOperationHistoryCurruntWorking.Dock = DockStyle.Fill;

                        SubViewOperationHistoryLotHistory = new EFEM.CustomizedByProcessType.UserInterface.OperationMainHistory.PWA500BIN.SubViewOperationHistoryLotHistory500BIN();
                        SubViewOperationHistoryLotHistory.Dock = DockStyle.Fill;
                        SubViewOperationHistoryLotHistory.Hide();
                    }
                    break;
                case Define.DefineEnumProject.AppConfig.EN_PROCESS_TYPE.DIE_TRANSFER:
                case Define.DefineEnumProject.AppConfig.EN_PROCESS_TYPE.DIE_TRANSFER_300:
                    {
                        SubViewOperationHistoryCurruntWorking = new EFEM.CustomizedByProcessType.UserInterface.OperationMainHistory.PWA500W.SubViewOperationHistoryCurruntWorking500W();
                        SubViewOperationHistoryCurruntWorking.Dock = DockStyle.Fill;

                        SubViewOperationHistoryLotHistory = new EFEM.CustomizedByProcessType.UserInterface.OperationMainHistory.PWA500W.SubViewOperationHistoryLotHistory500W();
                        SubViewOperationHistoryLotHistory.Dock = DockStyle.Fill;
                        SubViewOperationHistoryLotHistory.Hide();
                    }
                    break;
                default:
                    break;
            }

            if( SubViewOperationHistoryCurruntWorking != null)
            {
                tableLayoutPanel1.Controls.Add(SubViewOperationHistoryCurruntWorking, 0, 1);
                tableLayoutPanel1.Controls.Add(SubViewOperationHistoryLotHistory, 0, 1);

                _selectedSubView = SubViewOperationHistoryCurruntWorking;
            }
        }
        #endregion </Constructors>

        #region <Fields>
        private static Form_MessageBox _messageBox = null;

        private readonly Dictionary<SubViewType, Sys3Controls.Sys3button> SubViewButtons = null;
        private SubViewType _selectedSubViewType;
        private UserControlForMainView.CustomView _selectedSubView = null;

        private readonly UserControlForMainView.CustomView SubViewOperationHistoryCurruntWorking = null;
        private readonly UserControlForMainView.CustomView SubViewOperationHistoryLotHistory = null;
        #endregion </Fields>

        #region <Enum>
        private enum SubViewType
        {
            CurrentWorking,
            History,
        }
        #endregion </Enum>

        #region <Properties>
        private EQUIPMENT_STATE EnEqpState
        {
            get { return EquipmentState.GetInstance().GetState(); }
        }
        #endregion </Properties>

        #region <Methods>

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
            if (_selectedSubView != null)
            {
                _selectedSubView.CallFunctionByTimer();
            }
        }
        #endregion </Override>

        #region <UI Event>
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
        #endregion </UI Event>

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
                case SubViewType.CurrentWorking:                
                    _selectedSubView = SubViewOperationHistoryCurruntWorking;
                    break;
                case SubViewType.History:
                    _selectedSubView = SubViewOperationHistoryLotHistory;
                    break;

                default:
                    return;
            }

            _selectedSubView.ActivateView();
            _selectedSubView.Show();
        }
        private void UpdateView()
        {
            if (_selectedSubView == null)
                return;
            _selectedSubView.ActivateView();
        }
        #endregion </Display>

        #endregion </Methods>
    }
}
