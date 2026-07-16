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
                { SubViewType.Database, btnSubViewDatabase },
            };

            // 2026.07.06. jhlim [MOD] PWA500BIN/PWA500W 복붙 뷰를 공용 뷰로 통합.
            // 제품 차이(포트별 기판 타입 판별, Bin 목록의 공테이프 포함 여부)만 생성자로 주입한다.
            switch (AppConfigManager.Instance.ProcessType)
            {
                case Define.DefineEnumProject.AppConfig.EN_PROCESS_TYPE.NONE:
                    break;
                case Define.DefineEnumProject.AppConfig.EN_PROCESS_TYPE.BIN_SORTER:
                    {
                        SubViewOperationHistoryCurruntWorking = new EFEM.CustomizedByProcessType.UserInterface.OperationMainHistory.PWA500Common.SubViewOperationHistoryCurrentWorking(
                            lpIndex => FrameOfSystem3.SECSGEM.FunctionsForPWA500BIN_TP.Instance.GetSubstrateTypeByLoadPortIndex(lpIndex),
                            true);
                        SubViewOperationHistoryCurruntWorking.Dock = DockStyle.Fill;

                        SubViewOperationHistoryLotHistory = new EFEM.CustomizedByProcessType.UserInterface.OperationMainHistory.PWA500Common.SubViewOperationHistoryLotHistory();
                        SubViewOperationHistoryLotHistory.Dock = DockStyle.Fill;
                        SubViewOperationHistoryLotHistory.Hide();
                    }
                    break;
                case Define.DefineEnumProject.AppConfig.EN_PROCESS_TYPE.DIE_TRANSFER:
                case Define.DefineEnumProject.AppConfig.EN_PROCESS_TYPE.DIE_TRANSFER_300:
                    {
                        SubViewOperationHistoryCurruntWorking = new EFEM.CustomizedByProcessType.UserInterface.OperationMainHistory.PWA500Common.SubViewOperationHistoryCurrentWorking(
                            lpIndex => FrameOfSystem3.SECSGEM.FunctionsForPWA500W_NRD.Instance.GetSubstrateTypeByLoadPortIndex(lpIndex),
                            false);
                        SubViewOperationHistoryCurruntWorking.Dock = DockStyle.Fill;

                        SubViewOperationHistoryLotHistory = new EFEM.CustomizedByProcessType.UserInterface.OperationMainHistory.PWA500Common.SubViewOperationHistoryLotHistory();
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

                // 2026.07.09. jhlim [ADD] DB 조회 서브뷰는 제품 무관(공용) - 데이터는 MaterialDatabaseQueryProvider에서 얻는다.
                SubViewOperationHistoryDatabase = new EFEM.CustomizedByProcessType.UserInterface.OperationMainHistory.PWA500Common.SubViewOperationHistoryDatabase();
                SubViewOperationHistoryDatabase.Dock = DockStyle.Fill;
                SubViewOperationHistoryDatabase.Hide();
                tableLayoutPanel1.Controls.Add(SubViewOperationHistoryDatabase, 0, 1);

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
        private readonly UserControlForMainView.CustomView SubViewOperationHistoryDatabase = null;
        #endregion </Fields>

        #region <Enum>
        private enum SubViewType
        {
            CurrentWorking,
            History,
            Database,
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
                case SubViewType.Database:
                    _selectedSubView = SubViewOperationHistoryDatabase;
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
