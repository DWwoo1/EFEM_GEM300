using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

using EquipmentState_;

using FrameOfSystem3.Views;
using FrameOfSystem3.Views.Functional;

using EFEM.Modules;
using EFEM.MaterialTracking;
using EFEM.CustomizedByProcessType.PWA500Common;
using EFEM.History;

namespace EFEM.CustomizedByProcessType.UserInterface.OperationMainHistory.PWA500Common
{
    /// <summary>
    /// 2026.07.06. jhlim [MOD] PWA500BIN/PWA500W 중복 구현을 공용화.
    /// 제품 차이는 생성자 주입:
    /// - substrateTypeByLoadPortIndex : 로드포트 인덱스 -> 기판 타입 (기존 FunctionsForPWA500*.GetSubstrateTypeByLoadPortIndex)
    /// - includeEmptySubstrates       : Bin 목록에 Empty(공테이프) 포함 여부 (BIN=true, W=false)
    /// 데이터 접근은 IHistoryQuery(파일 우선 + DB 폴백 자동 선택)로 교체.
    /// </summary>
    public partial class SubViewOperationHistoryCurrentWorking : UserControlForMainView.CustomView
    {
        #region <Constructors>
        public SubViewOperationHistoryCurrentWorking(Func<int, SubstrateType> substrateTypeByLoadPortIndex, bool includeEmptySubstrates)
        {
            InitializeComponent();

            _loadPortManager = LoadPortManager.Instance;
            _substrateManager = SubstrateManager.Instance;
            _carrierServer = CarrierManagementServer.Instance;
            _lotHistoryLog = LotHistoryLog.Instance;
            _selectionList = Form_SelectionList.GetInstance();

            _substrateTypeByLoadPortIndex = substrateTypeByLoadPortIndex ?? throw new ArgumentNullException("substrateTypeByLoadPortIndex");
            _includeEmptySubstrates = includeEmptySubstrates;

            _temporarySubstrates = new List<Substrate>();

            CurrentCoreCarriers = new Dictionary<int, string>();

            UserControlCoreCarrierHistory = new MainDisplaySubPanelLotHistoryDisplayer(true)
            {
                Dock = DockStyle.Fill
            };
            tableLayoutPanel1.Controls.Add(UserControlCoreCarrierHistory, 1, 1);

            CurrentBinSubstrates = new List<string>();

            UserControlBinWaferHistory = new MainDisplaySubPanelLotHistoryDisplayer(true)
            {
                Dock = DockStyle.Fill
            };
            tableLayoutPanel1.Controls.Add(UserControlBinWaferHistory, 1, 0);

            // 자동 업데이트는 일단 막자.
            //_lotHistoryLog.AttachDisplayLogAction(AddCurrentPortHistory);

            this.Dock = DockStyle.Fill;
        }
        #endregion </Constructors>

        #region <Fields>
        private static SubstrateManager _substrateManager = null;
        private static CarrierManagementServer _carrierServer = null;
        private static LoadPortManager _loadPortManager = null;
        private static LotHistoryLog _lotHistoryLog = null;
        private static Form_SelectionList _selectionList = null;

        private const string CategoryBin = "Bin";

        private readonly Func<int, SubstrateType> _substrateTypeByLoadPortIndex;
        private readonly bool _includeEmptySubstrates;

        #region <Core Carrier>
        private List<Substrate> _temporarySubstrates = null;
        private readonly Dictionary<int, string> CurrentCoreCarriers = null;
        private int _selectedCoreCarrierPort = -1;
        private string _selectedCarrierId = string.Empty;
        #endregion </Core Carrier>

        #region <Bin Wafer>
        private readonly List<string> CurrentBinSubstrates = null;
        private string _selectedBinWafer = string.Empty;
        #endregion </Bin Wafer>

        // 조회는 UI 스레드를 막지 않도록 백그라운드에서 수행한다. (연타 역전은 시퀀스 번호로 무시)
        private int _coreQuerySequence = 0;
        private int _binQuerySequence = 0;

        #region <User Control>
        private readonly MainDisplaySubPanelLotHistoryDisplayer UserControlCoreCarrierHistory = null;
        private readonly MainDisplaySubPanelLotHistoryDisplayer UserControlBinWaferHistory = null;
        #endregion </User Control>

        #endregion </Fields>

        #region <Methods>

        #region <Override Methods>
        protected override void ProcessWhenActivation()
        {
            // 모든 자재 리스트는 아래 조건에서 다시 갱신한다.
            // 1. 화면전환 시
            // 2. Label 클릭

            // 코어 리스트는 작업 중인 캐리어 리스트만 포함한다.
            // BinList는 설비내(EFEM, 본설비) 모든 자재 중 Empty, Bin1~3 자재 중 Processing, Processed 상태만 리스트로 가져온다.

            // 리프레시는 현재 선택된 자재의 이력만 다시 읽는다.

            // Core 리스트 갱신
            RefreshCoreCarrierList();
            RefreshCoreCarrierHistory(false);

            // Bin 리스트 갱신
            RefreshBinSubstrateList();
            RefreshBinSubstrateHistory(false);

            base.ProcessWhenActivation();
        }
        public override void CallFunctionByTimer()
        {
            lblSelectedCoreCarrier.Text = _selectedCarrierId;
            lblSelectedBinWafer.Text = _selectedBinWafer;

            base.CallFunctionByTimer();
        }
        protected override void ProcessWhenDeactivation()
        {
        }
        #endregion </Override Methods>

        #region <UI Events>
        private void AddCurrentPortHistory(int port, string messageToAdd)
        {
            if (false == Visible)
                return;

            if (port.Equals(_selectedCoreCarrierPort))
            {
                UserControlCoreCarrierHistory.AddCurrentHistory(messageToAdd);
            }
        }

        private void BtnRefreshClicked(object sender, EventArgs e)
        {
            if (sender.Equals(btnRefreshCoreCarrierHistory))
            {
                RefreshCoreCarrierHistory(true);
            }
            else if (sender.Equals(btnRefreshBinWaferHistory))
            {
                RefreshBinSubstrateHistory(true);
            }
        }
        private void HistorySelectionLabelClicked(object sender, EventArgs e)
        {
            if (sender.Equals(lblSelectedCoreCarrier))
            {
                RefreshCoreCarrierList();
                if (_selectionList.CreateForm("Select Core Carrier", CurrentCoreCarriers.Values.ToArray(), CurrentCoreCarriers.Keys.ToArray(), _selectedCoreCarrierPort))
                {
                    _selectionList.GetResult(ref _selectedCoreCarrierPort);

                    RefreshCoreCarrierHistory(false);
                }
            }
            else if (sender.Equals(lblSelectedBinWafer))
            {
                RefreshBinSubstrateList();
                List<int> indexOfSubstrates = new List<int>();
                int index = 0;
                for (int i = 0; i < CurrentBinSubstrates.Count; ++i)
                {
                    indexOfSubstrates.Add(index++);
                }

                if (indexOfSubstrates.Count <= 0)
                    return;

                if (_selectionList.CreateForm("Select Bin Substrate", CurrentBinSubstrates.ToArray(), indexOfSubstrates.ToArray(), 0))
                {
                    _selectionList.GetResult(ref _selectedBinWafer);

                    RefreshBinSubstrateHistory(false);
                }
            }
        }
        #endregion </UI Events>

        #region <Internals>
        private void RefreshBinSubstrateList()
        {
            CurrentBinSubstrates.Clear();

            _temporarySubstrates.Clear();
            if (_substrateManager.GetSubstratesAll(ref _temporarySubstrates))
            {
                for (int i = 0; i < _temporarySubstrates.Count; ++i)
                {
                    var status = _temporarySubstrates[i].ProcessingStatus;
                    if (false == status.Equals(Defines.MaterialTracking.ProcessingStates.InProcess) &&
                        false == status.Equals(Defines.MaterialTracking.ProcessingStates.Processed))
                        continue;

                    var substrateTypeString = _temporarySubstrates[i].GetAttribute(PWA500SubstrateAttributes.SubstrateType);
                    if (false == Enum.TryParse(substrateTypeString, out SubstrateType substrateType))
                        continue;

                    switch (substrateType)
                    {
                        case SubstrateType.Empty:
                            {
                                // 제품별 차이 : BIN은 공테이프 포함, W는 미포함
                                if (_includeEmptySubstrates)
                                {
                                    CurrentBinSubstrates.Add(_temporarySubstrates[i].Name);
                                }
                            }
                            break;
                        case SubstrateType.Bin1:
                        case SubstrateType.Bin2:
                        case SubstrateType.Bin3:
                            {
                                CurrentBinSubstrates.Add(_temporarySubstrates[i].Name);
                            }
                            break;
                        default:
                            break;
                    }
                }
            }
        }
        private void RefreshBinSubstrateHistory(bool resetSelectionWhenEmpty)
        {
            if (string.IsNullOrEmpty(_selectedBinWafer) ||
                false == CurrentBinSubstrates.Contains(_selectedBinWafer))
            {
                _selectedBinWafer = string.Empty;
                UserControlBinWaferHistory.DisplayHistory(new List<HistoryRecord>());
                return;
            }

            // 조회는 백그라운드에서 수행하고 결과만 UI에 반영한다.
            var substrateName = _selectedBinWafer;
            int sequence = Interlocked.Increment(ref _binQuerySequence);
            Task.Run(() =>
            {
                var records = _lotHistoryLog.GetQuery().GetWorkingSubstrateHistory(substrateName, CategoryBin);
                InvokeOnUi(() =>
                {
                    if (sequence != _binQuerySequence)
                        return;

                    UserControlBinWaferHistory.DisplayHistory(records);

                    if (records.Count == 0 && resetSelectionWhenEmpty)
                    {
                        _selectedBinWafer = string.Empty;
                    }
                });
            });
        }

        private void RefreshCoreCarrierList()
        {
            CurrentCoreCarriers.Clear();

            for (int i = 0; i < _loadPortManager.Count; ++i)
            {
                var substrateType = _substrateTypeByLoadPortIndex(i);
                if (false == substrateType.Equals(SubstrateType.Core))
                    continue;

                if (false == _loadPortManager.IsLoadPortEnabled(i))
                    continue;

                int portId = _loadPortManager.GetLoadPortPortId(i);
                if (false == _carrierServer.HasCarrier(portId))
                    continue;

                CurrentCoreCarriers[portId] = _carrierServer.GetCarrierId(portId);
            }
        }
        private void RefreshCoreCarrierHistory(bool resetSelectionWhenEmpty)
        {
            if (false == CurrentCoreCarriers.TryGetValue(_selectedCoreCarrierPort, out string carrierId))
            {
                _selectedCarrierId = string.Empty;
                UserControlCoreCarrierHistory.DisplayHistory(new List<HistoryRecord>());
                return;
            }

            _selectedCarrierId = carrierId;

            // 조회는 백그라운드에서 수행하고 결과만 UI에 반영한다.
            var portId = _selectedCoreCarrierPort;
            int sequence = Interlocked.Increment(ref _coreQuerySequence);
            Task.Run(() =>
            {
                var records = _lotHistoryLog.GetQuery().GetWorkingCarrierHistory(portId, carrierId);
                InvokeOnUi(() =>
                {
                    if (sequence != _coreQuerySequence)
                        return;

                    UserControlCoreCarrierHistory.DisplayHistory(records);

                    if (records.Count == 0 && resetSelectionWhenEmpty)
                    {
                        _selectedCoreCarrierPort = -1;
                        _selectedCarrierId = string.Empty;
                    }
                });
            });
        }
        /// <summary>백그라운드 조회 결과를 UI 스레드로 반영한다. (핸들 미생성 등 예외는 무시)</summary>
        private void InvokeOnUi(Action action)
        {
            try
            {
                if (InvokeRequired)
                    BeginInvoke(action);
                else
                    action();
            }
            catch
            {
            }
        }
        #endregion </Internals>

        #endregion </Methods>
    }
}
