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
    /// 2026.07.10. jhlim [ADD] DB 조회 페이지의 "기판 상세" 뷰(메인 SubViewOperationHistoryDatabase 에서 분리).
    /// 페이지: Extra / Lot History / Location History / Processing History. 페이지 버튼 + 패널 Visible 토글로 전환.
    /// 메인은 LoadDetail(row)/ClearDetail()로 구동하고, "담긴 캐리어로 이동"은 GoToCarrierRequested 이벤트로 상향 위임
    /// (실제 캐리어 조회/모드전환은 메인이 수행). 데이터는 MaterialDatabaseQueryProvider(static)에서 직접 조회한다.
    /// </summary>
    public partial class SubstrateDetailView : UserControl
    {
        #region <Fields>
        private static readonly Color PageButtonAccent = Color.SeaGreen;

        private Panel[] _substratePagePanels;
        private readonly Dictionary<int, Sys3Controls.Sys3button> _substratePageButtons;
        private int _substratePageIndex;


        private SubstrateSearchRow _currentSubstrateRow;
        private int _detailSequence = 0;

        /// <summary>"담긴 캐리어로 이동" 클릭 시 현재 상세 기판 row로 발생. 실제 캐리어 조회/전환은 메인이 수행.</summary>
        public event Action<SubstrateSearchRow> GoToCarrierRequested;
        #endregion </Fields>

        #region <Constructor>
        public SubstrateDetailView()
        {
            InitializeComponent();

            _substratePagePanels = new[] { _pnlSubstratePageExtra, _pnlSubstratePageHistory, _pnlSubstratePageLocation, _pnlSubstratePageProcessing };

            // 페이지 버튼/액션 버튼은 Designer 의 페이지 바(TableLayoutPanel)에 배치됨. 여기선 인덱스 매핑만 한다.
            _substratePageButtons = new Dictionary<int, Sys3Controls.Sys3button>();
            _substratePageButtons[0] = btnExtra;
            _substratePageButtons[1] = btnLotHistory;
            _substratePageButtons[2] = btnLocationHistory;
            _substratePageButtons[3] = btnProcessingHistory;

            ReleaseGridReadOnly();
            ShowSubstratePage();
        }
        #endregion </Constructor>

        #region <Public API>
        public void LoadDetail(SubstrateSearchRow row)
        {
            if (row == null)
                return;

            _currentSubstrateRow = row;
            int sequence = Interlocked.Increment(ref _detailSequence);
            Task.Run(() =>
            {
                var substrate = MaterialDatabaseQueryProvider.Instance.GetSubstrate(row);
                var history = MaterialDatabaseQueryProvider.Instance.GetSubstrateLotHistory(row);
                var processing = MaterialDatabaseQueryProvider.Instance.GetProcessingHistory(row);
                var location = MaterialDatabaseQueryProvider.Instance.GetLocationHistory(row);
                InvokeOnUi(() =>
                {
                    if (sequence != _detailSequence)
                        return;

                    FillSubstrateExtraDetail(substrate);
                    FillLotHistoryGrid(_gvSubstrateLotHistory, history);
                    FillProcessingGrid(processing);
                    FillLocationGrid(location);
                });
            });
        }
        public void ClearDetail()
        {
            _gvSubstrateExtra.Rows.Clear();
            _gvSubstrateLotHistory.Rows.Clear();
            _gvProcessing.Rows.Clear();
            _gvLocation.Rows.Clear();
            _currentSubstrateRow = null;

            // 새 검색/결과 로드 시 항상 첫 페이지("Extra")로 리셋.
            _substratePageIndex = 0;
            ShowSubstratePage();
        }
        #endregion </Public API>

        #region <Detail grid fill>
        /// <summary>기본 속성은 메인 결과 그리드에 이미 표시되므로 상세에서는 Extra 만 채운다.</summary>
        private void FillSubstrateExtraDetail(SubstrateItem substrate)
        {
            _gvSubstrateExtra.Rows.Clear();
            if (substrate == null)
                return;

            FillExtraGrid(_gvSubstrateExtra, substrate.Extra);
        }
        private void FillProcessingGrid(List<SubstrateProcessingHistoryItem> items)
        {
            _gvProcessing.Rows.Clear();
            if (items == null)
                return;

            for (int i = 0; i < items.Count; ++i)
            {
                var it = items[i];
                _gvProcessing.Rows.Add(FormatLocalTime(it.EventTime), it.OldState, it.NewState, it.ControlJobId, it.ProcessJobId, it.LocationId, it.Description);
            }
        }
        private void FillLocationGrid(List<SubstrateLocationChangeItem> items)
        {
            _gvLocation.Rows.Clear();
            if (items == null)
                return;

            for (int i = 0; i < items.Count; ++i)
            {
                var it = items[i];
                _gvLocation.Rows.Add(FormatLocalTime(it.ChangeTime), it.FromLocationName, it.FromLocationKind.ToString(), it.ToLocationName, it.ToLocationKind.ToString(), it.Reason);
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
        private void SubstratePageButtonClicked(object sender, EventArgs e)
        {
            int index = -1;
            foreach (var item in _substratePageButtons)
            {
                if(sender.Equals(item.Value))
                {
                    index = item.Key;
                    break;
                }
            }
            
            if (index < 0)
                return;
            
            _substratePageIndex = index;
            ShowSubstratePage();
        }
        private void ShowSubstratePage()
        {
            for (int i = 0; i < _substratePagePanels.Length; ++i)
                _substratePagePanels[i].Visible = (i == _substratePageIndex);
            
            HighlightPageButtons(_substratePageButtons, _substratePageIndex);
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

        #region <담긴 캐리어로 이동>
        private void BtnGoToCarrierClicked(object sender, EventArgs e)
        {
            if (_currentSubstrateRow == null)
                return;

            string carrierKey = _currentSubstrateRow.CurrentCarrierKey;
            if (string.IsNullOrEmpty(carrierKey))
            {
                MessageBox.Show("이 기판은 현재 캐리어에 담겨있지 않습니다.", "담긴 캐리어로 이동",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            var handler = GoToCarrierRequested;
            if (handler != null)
                handler(_currentSubstrateRow);
        }
        #endregion </담긴 캐리어로 이동>

        #region <그리드 ReadOnly 해제>
        /// <summary>모든 상세 그리드의 ReadOnly 를 풀고 F2 편집모드를 허용한다(F2 로 편집창을 열어 셀 내용 선택/복사).
        /// 이 그리드들은 DataSource 없이 코드로 채우는 표시 전용이라 편집해도 DB 로 반영되는 경로가 없다.</summary>
        private void ReleaseGridReadOnly()
        {
            var grids = new DataGridView[] { _gvSubstrateExtra, _gvSubstrateLotHistory, _gvProcessing, _gvLocation };
            foreach (var grid in grids)
            {
                grid.ReadOnly = false;
                grid.EditMode = DataGridViewEditMode.EditOnF2;
            }
        }
        #endregion </그리드 ReadOnly 해제>

        #region <Helpers>
        /// <summary>처리/위치 이력 시각은 UTC(Kind=Utc)로 파싱되므로 표시 시 로컬로 변환한다.</summary>
        private static string FormatLocalTime(DateTime time)
        {
            if (time == DateTime.MinValue)
                return string.Empty;

            DateTime local = time.Kind == DateTimeKind.Utc ? time.ToLocalTime() : time;
            return local.ToString("yyyy-MM-dd HH:mm:ss.fff");
        }
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
