using System;
using System.Collections.Generic;
using System.Drawing;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

using EFEM.History;
using EFEM.MaterialTracking;
using EFEM.MaterialTracking.Inspection;

namespace EFEM.CustomizedByProcessType.UserInterface.OperationMainHistory.PWA500Common
{
    /// <summary>
    /// 2026.07.10. jhlim [ADD] DB 조회 페이지의 "캐리어 상세" 뷰(메인 SubViewOperationHistoryDatabase 에서 분리).
    /// 페이지: Substrates In Carrier / Lot History / Extra(+SlotMap). 페이지 버튼 + 패널 Visible 토글로 전환.
    /// 메인은 LoadDetail(row)/ClearDetail()로 구동하고, "Search This Substrate"는 SearchSubstrateRequested 이벤트로
    /// 상향 위임(실제 기판 검색은 메인이 수행). 데이터는 MaterialDatabaseQueryProvider(static)에서 직접 조회한다.
    /// </summary>
    public partial class CarrierDetailView : UserControl
    {
        #region <Fields>
        private readonly Dictionary<int, Sys3Controls.Sys3button> _carrierPageButtons;
        private static readonly Color PageButtonAccent = Color.SeaGreen;

        private Panel[] _carrierPagePanels;
        private int _carrierPageIndex;

        private CarrierSearchRow _currentCarrierRow;
        private int _detailSequence = 0;

        /// <summary>"Search This Substrate" 클릭 시 선택 안착기판의 UniqueKey로 발생. 실제 검색은 메인이 수행.</summary>
        public event Action<string> SearchSubstrateRequested;
        #endregion </Fields>

        #region <Constructor>
        public CarrierDetailView()
        {
            InitializeComponent();

            _carrierPagePanels = new[] { _pnlCarrierPageSubstrates, _pnlCarrierPageHistory, _pnlCarrierPageExtra };

            // 페이지 버튼/액션 버튼은 Designer 의 페이지 바(TableLayoutPanel)에 배치됨. 여기선 인덱스 매핑만 한다.
            _carrierPageButtons = new Dictionary<int, Sys3Controls.Sys3button>();
            _carrierPageButtons[0] = btnSubstratesInCarrier;
            _carrierPageButtons[1] = btnLotHistory;
            _carrierPageButtons[2] = btnExtra;

            SetupSubstratesInCarrierColumns();
            ReleaseGridReadOnly();
            ShowCarrierPage();
        }
        #endregion </Constructor>

        #region <Public API>
        /// <summary>선택된 캐리어(row)의 상세를 백그라운드 조회해 채운다. 출처 앵커(row.Source/ArchiveDbPath)로 같은 DB 를 재조회.</summary>
        public void LoadDetail(CarrierSearchRow row)
        {
            if (row == null)
                return;

            _currentCarrierRow = row;
            int sequence = Interlocked.Increment(ref _detailSequence);
            Task.Run(() =>
            {
                var carrier = MaterialDatabaseQueryProvider.Instance.GetCarrier(row);
                var substrates = MaterialDatabaseQueryProvider.Instance.GetSubstratesInCarrier(row);
                var history = MaterialDatabaseQueryProvider.Instance.GetCarrierLotHistory(row);
                InvokeOnUi(() =>
                {
                    if (sequence != _detailSequence)
                        return;

                    FillCarrierExtraDetail(carrier);
                    FillSubstratesInCarrier(substrates);
                    FillLotHistoryGrid(_gvCarrierLotHistory, history);
                });
            });
        }
        public void ClearDetail()
        {
            _gvCarrierSlotMap.Rows.Clear();
            _gvCarrierExtra.Rows.Clear();
            _gvSubstratesInCarrier.Rows.Clear();
            _gvCarrierLotHistory.Rows.Clear();
            _currentCarrierRow = null;

            // 새 검색/결과 로드 시 항상 첫 페이지("Substrates In Carrier")로 리셋.
            _carrierPageIndex = 0;
            ShowCarrierPage();
        }
        #endregion </Public API>

        #region <Detail grid fill>
        /// <summary>기본 속성은 메인 결과 그리드에 이미 표시되므로, 상세에서는 Extra(+SlotMap)만 채운다.</summary>
        private void FillCarrierExtraDetail(CarrierItem carrier)
        {
            _gvCarrierSlotMap.Rows.Clear();
            _gvCarrierExtra.Rows.Clear();
            if (carrier == null)
                return;

            if (carrier.SlotMaps != null)
            {
                foreach (var kv in carrier.SlotMaps)
                    _gvCarrierSlotMap.Rows.Add(kv.Key.ToString(), kv.Value.ToString());
            }
            FillExtraGrid(_gvCarrierExtra, carrier.Extra);
        }
        /// <summary>안착 기판 그리드 컬럼 구성(기판 기본 속성 전부, UniqueKey 는 마지막 = Search This Substrate/더블클릭이 참조).
        /// 컬럼이 많아 화면 폭을 넘어가므로 기본 폭 + 가로 스크롤을 허용한다.</summary>
        private void SetupSubstratesInCarrierColumns()
        {
            _gvSubstratesInCarrier.Columns.Clear();
            AddTextColumn(_gvSubstratesInCarrier, "Name");
            AddTextColumn(_gvSubstratesInCarrier, "OriginName");
            AddTextColumn(_gvSubstratesInCarrier, "Location");
            AddTextColumn(_gvSubstratesInCarrier, "Src Port");
            AddTextColumn(_gvSubstratesInCarrier, "Src Slot");
            AddTextColumn(_gvSubstratesInCarrier, "Src Carrier");
            AddTextColumn(_gvSubstratesInCarrier, "Current Carrier Key");
            AddTextColumn(_gvSubstratesInCarrier, "Dest Port");
            AddTextColumn(_gvSubstratesInCarrier, "Dest Slot");
            AddTextColumn(_gvSubstratesInCarrier, "Lot ID");
            AddTextColumn(_gvSubstratesInCarrier, "Recipe ID");
            AddTextColumn(_gvSubstratesInCarrier, "Process Job");
            AddTextColumn(_gvSubstratesInCarrier, "Control Job");
            AddTextColumn(_gvSubstratesInCarrier, "Transport Status");
            AddTextColumn(_gvSubstratesInCarrier, "Processing Status");
            AddTextColumn(_gvSubstratesInCarrier, "Id Reading Status");
            AddTextColumn(_gvSubstratesInCarrier, "DoNotProcess");
            AddTextColumn(_gvSubstratesInCarrier, "Usage");
            AddTextColumn(_gvSubstratesInCarrier, "UniqueKey");
        }
        private static void AddTextColumn(DataGridView grid, string header)
        {
            grid.Columns.Add(new DataGridViewTextBoxColumn
            {
                HeaderText = header,
                SortMode = DataGridViewColumnSortMode.NotSortable
            });
        }
        private void FillSubstratesInCarrier(List<SubstrateItem> substrates)
        {
            _gvSubstratesInCarrier.Rows.Clear();
            if (substrates == null)
                return;

            for (int i = 0; i < substrates.Count; ++i)
            {
                var s = substrates[i];
                _gvSubstratesInCarrier.Rows.Add(
                    s.Name, s.OriginName, s.LocationId,
                    s.SourcePortId.ToString(), s.SourceSlot.ToString(), s.SourceCarrierId, s.CurrentCarrierKey,
                    s.DestinationPortId.ToString(), s.DestinationSlot.ToString(), s.LotId, s.RecipeId, s.ProcessJobId, s.ControlJobId,
                    s.TransportStatus.ToString(), s.ProcessingStatus.ToString(), s.IdReadingStatus.ToString(),
                    s.DoNotProcessFlag.ToString(), s.Usage.ToString(), s.UniqueKey);
            }
        }
        private static void FillLotHistoryGrid(DataGridView grid, List<HistoryRecord> records)
        {
            grid.Rows.Clear();
            if (records == null)
                return;

            for (int i = 0; i < records.Count; ++i)
            {
                var r = records[i];
                grid.Rows.Add(r.Time.ToString("yyyy-MM-dd HH:mm:ss.fff"), r.CarrierEventCode, r.SubstrateName, r.SubstrateEventCode, r.Message);
            }
        }
        private static void FillExtraGrid(DataGridView grid, Dictionary<string, string> extra)
        {
            grid.Rows.Clear();
            if (extra == null)
                return;

            foreach (var kv in extra)
                grid.Rows.Add(kv.Key, kv.Value);
        }
        #endregion </Detail grid fill>

        #region <Page 전환 (버튼 + 패널 토글)>
        private void CarrierPageButtonClicked(object sender, EventArgs e)
        {
            int index = -1;
            foreach (var item in _carrierPageButtons)
            {
                if (sender.Equals(item.Value))
                {
                    index = item.Key;
                    break;
                }
            }

            if (index < 0)
                return;

            _carrierPageIndex = index;

            ShowCarrierPage();
        }
        private void ShowCarrierPage()
        {
            for (int i = 0; i < _carrierPagePanels.Length; ++i)
            {
                _carrierPagePanels[i].Visible = (i == _carrierPageIndex);
            }

            HighlightPageButtons(_carrierPageButtons, _carrierPageIndex);
        }
        private static void HighlightPageButtons(Dictionary<int, Sys3Controls.Sys3button> buttons, int selectedIndex)
        {
            foreach (var item in buttons)
            {
                bool selected = (item.Key == selectedIndex);
                item.Value.ButtonClicked = selected;
                item.Value.MainFontColor = selected ? Color.White : PageButtonAccent;
            }
        }
        #endregion </Page 전환>

        #region <안착 기판: 재검색 / 상세 팝업>
        private void BtnSearchSelectedSubstrateClicked(object sender, EventArgs e)
        {
            var rowIndex = _gvSubstratesInCarrier.CurrentRow != null ? _gvSubstratesInCarrier.CurrentRow.Index : -1;
            if (rowIndex < 0)
                return;

            // 마지막(숨은) 컬럼 = UniqueKey. 이름(LIKE)은 동명 기판이 중복되므로 UniqueKey 정확일치로 1건만 조회(메인이 수행).
            int uniqueKeyColumnIndex = _gvSubstratesInCarrier.Columns.Count - 1;
            var uniqueKey = Convert.ToString(_gvSubstratesInCarrier.Rows[rowIndex].Cells[uniqueKeyColumnIndex].Value);
            if (string.IsNullOrEmpty(uniqueKey))
                return;

            var handler = SearchSubstrateRequested;
            if (handler != null)
                handler(uniqueKey);
        }
        private void GvSubstratesInCarrierCellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0 || _currentCarrierRow == null)
                return;

            int uniqueKeyColumnIndex = _gvSubstratesInCarrier.Columns.Count - 1;
            var uniqueKey = Convert.ToString(_gvSubstratesInCarrier.Rows[e.RowIndex].Cells[uniqueKeyColumnIndex].Value);
            if (string.IsNullOrEmpty(uniqueKey))
                return;

            var substrateRow = new SubstrateSearchRow
            {
                UniqueKey = uniqueKey,
                Source = _currentCarrierRow.Source,
                ArchiveDbPath = _currentCarrierRow.ArchiveDbPath
            };

            Task.Run(() =>
            {
                var item = MaterialDatabaseQueryProvider.Instance.GetSubstrate(substrateRow);
                InvokeOnUi(() =>
                {
                    if (item == null)
                        return;

                    using (var popup = new FormSubstrateInspectionDetail())
                        popup.ShowSubstrate(item);
                });
            });
        }
        #endregion </안착 기판: 재검색 / 상세 팝업>

        #region <그리드 ReadOnly 해제>
        /// <summary>모든 상세 그리드의 ReadOnly 를 풀고 F2 편집모드를 허용한다(F2 로 편집창을 열어 셀 내용 선택/복사).
        /// 이 그리드들은 DataSource 없이 코드로 채우는 표시 전용이라 편집해도 DB 로 반영되는 경로가 없다.</summary>
        private void ReleaseGridReadOnly()
        {
            var grids = new DataGridView[] { _gvCarrierSlotMap, _gvCarrierExtra, _gvCarrierLotHistory, _gvSubstratesInCarrier };
            foreach (var grid in grids)
            {
                grid.ReadOnly = false;
                grid.EditMode = DataGridViewEditMode.EditOnF2;
            }
        }
        #endregion </그리드 ReadOnly 해제>

        #region <Helpers>
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
        #endregion </Helpers>
    }
}
