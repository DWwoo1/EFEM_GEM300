using System;
using System.Collections.Generic;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

using FrameOfSystem3.Views;
using FrameOfSystem3.Views.Functional;

using EFEM.MaterialTracking;
using EFEM.MaterialTracking.Inspection;

namespace EFEM.CustomizedByProcessType.UserInterface.OperationMainHistory.PWA500Common
{
    /// <summary>
    /// 2026.07.09. jhlim [ADD] SQLite(MaterialDbContext) 자유 조건 조회 서브뷰.
    ///
    /// - 대상(Carrier/Substrate) + 날짜 범위 + 조건(이름/Lot ID + 기타 조건 1개)으로 검색(main + 일자별 archive 병합).
    /// - 결과 그리드(gvResults)는 Extra 를 제외한 기본 속성 전부를 보여준다(캐리어/기판 공용, 컬럼만 모드별로 교체).
    /// - 상세는 캐리어/기판 각각 별도 UserControl(CarrierDetailView/SubstrateDetailView)로 분리해 pnlDetail 에 겹쳐두고 스왑.
    ///   상세 → 메인 역방향 동작(안착 기판 검색 / 담긴 캐리어로 이동)은 상세 뷰의 이벤트로 상향 위임받아 이 메인이 수행한다.
    /// 데이터는 MaterialDatabaseQueryProvider.Instance(초기화 시 Initializer 가 장착)에서 얻는다.
    /// 조회는 UI 를 막지 않도록 Task.Run 백그라운드에서 수행하고, 연타 역전은 시퀀스 번호로 무시한다.
    ///
    /// 2026.07.10. jhlim [MOD] 검색바/결과그리드/모드전환만 이 파일에 남기고, 캐리어/기판 상세를 별도 UserControl 로 분리.
    /// </summary>
    public partial class SubViewOperationHistoryDatabase : UserControlForMainView.CustomView
    {
        #region <Constructors>
        public SubViewOperationHistoryDatabase()
        {
            InitializeComponent();

            _keyboard = Form_Keyboard.GetInstance();
            _selectionList = Form_SelectionList.GetInstance();

            // 캐리어/기판 상세 뷰를 pnlDetail 에 겹쳐 배치하고 모드에 따라 Visible 토글로 스왑.
            _carrierDetailView = new CarrierDetailView { Dock = DockStyle.Fill };
            _substrateDetailView = new SubstrateDetailView { Dock = DockStyle.Fill };
            pnlDetail.Controls.Add(_carrierDetailView);
            pnlDetail.Controls.Add(_substrateDetailView);

            // 상세 → 메인 역방향 동작(이벤트 위임).
            _carrierDetailView.SearchSubstrateRequested += OnSearchSubstrateRequested;
            _substrateDetailView.GoToCarrierRequested += OnGoToCarrierRequested;

            // 결과 그리드 ReadOnly 해제(셀 선택/복사 허용, 편집 차단).
            ReleaseGridReadOnly();

            _mode = SearchMode.Carrier;
            _startDate = DateTime.Today.AddDays(-7);
            _endDate = DateTime.Today;

            ApplyModeLayout();
            RefreshSearchBarTexts();
            ResetOtherCondition();

            Dock = DockStyle.Fill;
        }
        #endregion </Constructors>

        #region <Types>
        private enum SearchMode
        {
            Carrier,
            Substrate
        }
        #endregion </Types>

        #region <Fields>
        private const string TargetCarrier = "Carrier";
        private const string TargetSubstrate = "Substrate";
        private const string OtherFieldPlaceholder = "(Select Field)";
        private const string MatchPartial = "Partial";
        private const string MatchExact = "Exact";

        private static Form_Keyboard _keyboard = null;
        private static Form_SelectionList _selectionList = null;

        private SearchMode _mode;
        private DateTime _startDate;
        private DateTime _endDate;

        // 조회는 백그라운드에서 수행한다. 연타로 이전 결과가 늦게 도착하는 역전은 시퀀스 번호로 무시한다.
        private int _searchSequence = 0;

        // 검색 결과(그리드 행 인덱스 = 리스트 인덱스). 상세 조회/CSV 대상.
        private readonly List<CarrierSearchRow> _carrierResults = new List<CarrierSearchRow>();
        private readonly List<SubstrateSearchRow> _substrateResults = new List<SubstrateSearchRow>();

        // 그리드 채우는 동안 SelectionChanged 로 상세 조회가 폭주하지 않도록 억제.
        private bool _suppressSelection = false;

        // 상세 뷰(캐리어/기판). Designer 의 pnlDetail 에 코드로 배치.
        private CarrierDetailView _carrierDetailView;
        private SubstrateDetailView _substrateDetailView;


        // "Search This Substrate"로 검색을 시작한 경우, 결과 도착 후 이 UniqueKey 행을 자동 선택(클릭 상태)한다.
        private string _pendingSelectSubstrateUniqueKey;

        #endregion </Fields>

        #region <Override Methods>
        protected override void ProcessWhenActivation()
        {
            BeginInvoke((MethodInvoker)delegate
            {
                bool available = MaterialDatabaseQueryProvider.IsAvailable;
                _lblUnavailable.Visible = false == available;
                SetInputsEnabled(available);

                // 날짜 선택기 기본값: 최근 7일 ~ 오늘 (미래 선택 방지)
                _startDatePicker.MaxDate = DateTime.Today;
                _endDatePicker.MaxDate = DateTime.Today;
                _startDatePicker.Value = _startDate;
                _endDatePicker.Value = _endDate;
            });

            base.ProcessWhenActivation();
        }
        public override void CallFunctionByTimer()
        {
            _btnExport.Enabled = MaterialDatabaseQueryProvider.IsAvailable && gvResults.Rows.Count > 0;
            base.CallFunctionByTimer();
        }
        protected override void ProcessWhenDeactivation()
        {
        }
        #endregion </Override Methods>

        #region <UI Events>
        private void TargetValueClicked(object sender, EventArgs e)
        {
            string[] names = { TargetCarrier, TargetSubstrate };
            int[] keys = { (int)SearchMode.Carrier, (int)SearchMode.Substrate };
            int selected = (int)_mode;
            if (false == _selectionList.CreateForm("Select Target", names, keys, selected))
                return;

            _selectionList.GetResult(ref selected);
            _mode = selected == (int)SearchMode.Substrate ? SearchMode.Substrate : SearchMode.Carrier;

            ApplyModeLayout();
            RefreshSearchBarTexts();
            ResetOtherCondition();
            ClearResults();
        }
        private void StartDateChanged(object sender, EventArgs e)
        {
            _startDate = _startDatePicker.Value.Date;
            if (_startDate > _endDate)
            {
                _endDate = _startDate;
                _endDatePicker.Value = _endDate;
            }
        }
        private void EndDateChanged(object sender, EventArgs e)
        {
            _endDate = _endDatePicker.Value.Date;
            if (_endDate < _startDate)
            {
                _startDate = _endDate;
                _startDatePicker.Value = _startDate;
            }
        }
        private void CondValueClicked(object sender, EventArgs e)
        {
            var label = sender as Sys3Controls.Sys3Label;
            if (label == null)
                return;

            string value = label.Text ?? string.Empty;
            if (false == _keyboard.CreateForm(value, 100, false, "Enter Value"))
                return;

            _keyboard.GetResult(ref value);
            label.Text = value == null ? string.Empty : value.Trim();
        }
        private void OtherFieldSelectClicked(object sender, EventArgs e)
        {
            var fields = new List<string>();
            fields.AddRange(_mode == SearchMode.Carrier ? MaterialDatabaseQuery.CarrierBaseFieldNames : MaterialDatabaseQuery.SubstrateBaseFieldNames);

            if (MaterialDatabaseQueryProvider.IsAvailable)
            {
                var extraKeys = _mode == SearchMode.Carrier
                    ? MaterialDatabaseQueryProvider.Instance.CarrierExtraKeys
                    : MaterialDatabaseQueryProvider.Instance.SubstrateExtraKeys;
                if (extraKeys != null)
                    fields.AddRange(extraKeys);
            }

            var keys = new int[fields.Count];
            for (int i = 0; i < keys.Length; ++i) keys[i] = i;

            int selected = fields.IndexOf(_lblOtherFieldValue.Text);
            if (selected < 0) selected = 0;

            if (false == _selectionList.CreateForm("Select Field", fields.ToArray(), keys, selected))
                return;

            _selectionList.GetResult(ref selected);
            if (selected >= 0 && selected < fields.Count)
                _lblOtherFieldValue.Text = fields[selected];
        }
        private void OtherMatchToggleClicked(object sender, EventArgs e)
        {
            _lblOtherMatchToggle.Text = _lblOtherMatchToggle.Text == MatchExact ? MatchPartial : MatchExact;
        }
        private void BtnSearchClicked(object sender, EventArgs e)
        {
            if (false == MaterialDatabaseQueryProvider.IsAvailable)
                return;

            if (_mode == SearchMode.Carrier)
                SearchCarriers();
            else
                SearchSubstrates();
        }
        private void GvResultsSelectionChanged(object sender, EventArgs e)
        {
            if (_suppressSelection)
                return;

            var index = CurrentRowIndex();
            if (index < 0)
                return;

            if (_mode == SearchMode.Carrier)
            {
                if (index < _carrierResults.Count)
                    _carrierDetailView.LoadDetail(_carrierResults[index]);
            }
            else
            {
                if (index < _substrateResults.Count)
                    _substrateDetailView.LoadDetail(_substrateResults[index]);
            }
        }
        private void BtnExportClicked(object sender, EventArgs e)
        {
            ExportResults();
        }
        /// <summary>검색 조건(이름/Lot ID/그 외 조건 + 날짜 범위)을 모두 기본값으로 되돌린다. 대상(Carrier/Substrate)과 결과 그리드는 유지.</summary>
        private void BtnResetClicked(object sender, EventArgs e)
        {
            _lblCond1Value.Text = string.Empty;
            _lblCond2Value.Text = string.Empty;
            ResetOtherCondition();

            _startDate = DateTime.Today.AddDays(-7);
            _endDate = DateTime.Today;
            _startDatePicker.Value = _startDate;
            _endDatePicker.Value = _endDate;
        }
        #endregion </UI Events>

        #region <상세 → 메인 역방향 동작>
        /// <summary>캐리어 상세의 "Search This Substrate": 선택 안착기판을 UniqueKey 정확일치로 기판 검색(1건).</summary>
        private void OnSearchSubstrateRequested(string uniqueKey)
        {
            if (string.IsNullOrEmpty(uniqueKey))
                return;

            _mode = SearchMode.Substrate;
            ApplyModeLayout();
            RefreshSearchBarTexts();

            _lblCond1Value.Text = string.Empty;
            _lblCond2Value.Text = string.Empty;
            _lblOtherFieldValue.Text = "UniqueKey";
            _lblOtherValueValue.Text = uniqueKey;
            _lblOtherMatchToggle.Text = MatchExact;

            // 결과 도착 후 이 기판 행을 자동 선택(클릭 상태)하여 상세가 바로 보이도록.
            _pendingSelectSubstrateUniqueKey = uniqueKey;
            SearchSubstrates();
        }
        /// <summary>기판 상세의 "담긴 캐리어로 이동": 기판의 CurrentCarrierKey(=캐리어 UniqueKey)와 출처 앵커로
        /// 캐리어를 정확 조회(날짜/이름 무관)해 캐리어 모드로 전환하고 결과 그리드에 1건 표시 후 선택→상세 로드.</summary>
        private void OnGoToCarrierRequested(SubstrateSearchRow substrateRow)
        {
            if (substrateRow == null)
                return;

            var anchorRow = new CarrierSearchRow
            {
                UniqueKey = substrateRow.CurrentCarrierKey,
                Source = substrateRow.Source,
                ArchiveDbPath = substrateRow.ArchiveDbPath
            };

            Task.Run(() =>
            {
                var carrier = MaterialDatabaseQueryProvider.Instance.GetCarrier(anchorRow);
                InvokeOnUi(() =>
                {
                    if (carrier == null)
                    {
                        MessageBox.Show("해당 캐리어를 찾을 수 없습니다.", "담긴 캐리어로 이동",
                            MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    }

                    var row = new CarrierSearchRow
                    {
                        UniqueKey = carrier.UniqueKey,
                        CarrierId = carrier.CarrierId,
                        LotId = carrier.LotId,
                        PortId = carrier.PortId,
                        AccessStatus = carrier.AccessStatus,
                        Capacity = carrier.Capacity,
                        LoadTime = carrier.LoadTime,
                        UnloadTime = carrier.UnloadTime,
                        Source = anchorRow.Source,
                        ArchiveDbPath = anchorRow.ArchiveDbPath
                    };

                    _mode = SearchMode.Carrier;
                    ApplyModeLayout();
                    RefreshSearchBarTexts();
                    _lblCond1Value.Text = string.Empty;
                    _lblCond2Value.Text = string.Empty;
                    ResetOtherCondition();

                    _carrierResults.Clear();
                    _carrierResults.Add(row);
                    FillCarrierResultGrid();

                    // 담긴 캐리어로 이동 시 첫 페이지("Substrates In Carrier")부터 보여주도록 리셋.
                    _carrierDetailView.ClearDetail();

                    if (gvResults.Rows.Count > 0)
                    {
                        gvResults.ClearSelection();
                        gvResults.CurrentCell = gvResults.Rows[0].Cells[0];
                        gvResults.Rows[0].Selected = true;
                    }
                });
            });
        }
        #endregion </상세 → 메인 역방향 동작>

        #region <Search>
        private void SearchCarriers()
        {
            var criteria = new CarrierSearchCriteria
            {
                CarrierId = _lblCond1Value.Text,
                LotId = _lblCond2Value.Text,
                StartDate = _startDate,
                EndDate = _endDate
            };
            ApplyOtherCondition(criteria);

            int sequence = Interlocked.Increment(ref _searchSequence);
            Task.Run(() =>
            {
                var rows = MaterialDatabaseQueryProvider.Instance.SearchCarriers(criteria);
                InvokeOnUi(() =>
                {
                    if (sequence != _searchSequence)
                        return;

                    _carrierResults.Clear();
                    _carrierResults.AddRange(rows);
                    FillCarrierResultGrid();
                    _carrierDetailView.ClearDetail();
                });
            });
        }
        private void SearchSubstrates()
        {
            var criteria = new SubstrateSearchCriteria
            {
                Name = _lblCond1Value.Text,
                LotId = _lblCond2Value.Text,
                StartDate = _startDate,
                EndDate = _endDate
            };
            ApplyOtherCondition(criteria);

            int sequence = Interlocked.Increment(ref _searchSequence);
            Task.Run(() =>
            {
                var rows = MaterialDatabaseQueryProvider.Instance.SearchSubstrates(criteria);
                InvokeOnUi(() =>
                {
                    if (sequence != _searchSequence)
                        return;

                    _substrateResults.Clear();
                    _substrateResults.AddRange(rows);
                    FillSubstrateResultGrid();
                    _substrateDetailView.ClearDetail();
                    SelectPendingSubstrate();
                });
            });
        }
        /// <summary>"Search This Substrate"로 시작한 검색이면 결과에서 해당 UniqueKey 행을 선택(클릭 상태)해 상세를 즉시 표시한다.</summary>
        private void SelectPendingSubstrate()
        {
            if (string.IsNullOrEmpty(_pendingSelectSubstrateUniqueKey))
                return;

            string key = _pendingSelectSubstrateUniqueKey;
            _pendingSelectSubstrateUniqueKey = null;

            for (int i = 0; i < _substrateResults.Count; ++i)
            {
                if (_substrateResults[i].UniqueKey == key)
                {
                    if (i < gvResults.Rows.Count)
                    {
                        gvResults.ClearSelection();
                        gvResults.CurrentCell = gvResults.Rows[i].Cells[0];
                        gvResults.Rows[i].Selected = true;
                    }
                    return;
                }
            }
        }
        /// <summary>"그 외 조건" 라벨 상태를 CarrierSearchCriteria 에 반영한다. Placeholder/공란이면 미사용.</summary>
        private void ApplyOtherCondition(CarrierSearchCriteria criteria)
        {
            var field = _lblOtherFieldValue.Text;
            bool used = field != OtherFieldPlaceholder && false == string.IsNullOrWhiteSpace(field);
            criteria.OtherFieldName = used ? field : string.Empty;
            criteria.OtherFieldValue = _lblOtherValueValue.Text;
            criteria.OtherFieldExactMatch = _lblOtherMatchToggle.Text == MatchExact;
        }
        /// <summary>"그 외 조건" 라벨 상태를 SubstrateSearchCriteria 에 반영한다. Placeholder/공란이면 미사용.</summary>
        private void ApplyOtherCondition(SubstrateSearchCriteria criteria)
        {
            var field = _lblOtherFieldValue.Text;
            bool used = field != OtherFieldPlaceholder && false == string.IsNullOrWhiteSpace(field);
            criteria.OtherFieldName = used ? field : string.Empty;
            criteria.OtherFieldValue = _lblOtherValueValue.Text;
            criteria.OtherFieldExactMatch = _lblOtherMatchToggle.Text == MatchExact;
        }
        private void ResetOtherCondition()
        {
            _lblOtherFieldValue.Text = OtherFieldPlaceholder;
            _lblOtherValueValue.Text = string.Empty;
            _lblOtherMatchToggle.Text = MatchPartial;
        }
        #endregion </Search>

        #region <Result grid fill>
        private void FillCarrierResultGrid()
        {
            _suppressSelection = true;
            try
            {
                SetResultColumns("출처", "UniqueKey", "Carrier ID", "Lot ID", "Port ID", "Access Status", "Capacity", "Load Time", "Unload Time");
                gvResults.Rows.Clear();
                for (int i = 0; i < _carrierResults.Count; ++i)
                {
                    var r = _carrierResults[i];
                    gvResults.Rows.Add(SourceText(r.Source), r.UniqueKey, r.CarrierId, r.LotId, r.PortId.ToString(),
                        r.AccessStatus.ToString(), r.Capacity.ToString(), r.LoadTime, r.UnloadTime);
                }
                gvResults.ClearSelection();
            }
            finally
            {
                _suppressSelection = false;
            }
        }
        private void FillSubstrateResultGrid()
        {
            _suppressSelection = true;
            try
            {
                SetResultColumns("출처", "UniqueKey", "Name", "OriginName", "Location",
                    "Src Port", "Src Slot", "Src Carrier", "Current Carrier Key",
                    "Dest Port", "Dest Slot", "Lot ID", "Recipe ID", "Process Job", "Control Job",
                    "Transport Status", "Processing Status", "Id Reading Status", "DoNotProcess", "Usage");
                gvResults.Rows.Clear();
                for (int i = 0; i < _substrateResults.Count; ++i)
                {
                    var r = _substrateResults[i];
                    gvResults.Rows.Add(
                        SourceText(r.Source), r.UniqueKey, r.Name, r.OriginName, r.LocationId,
                        r.SourcePortId.ToString(), r.SourceSlot.ToString(), r.SourceCarrierId, r.CurrentCarrierKey,
                        r.DestinationPortId.ToString(), r.DestinationSlot.ToString(), r.LotId, r.RecipeId, r.ProcessJobId, r.ControlJobId,
                        r.TransportStatus.ToString(), r.ProcessingStatus.ToString(), r.IdReadingStatus.ToString(),
                        r.DoNotProcessFlag.ToString(), r.Usage.ToString());
                }
                gvResults.ClearSelection();
            }
            finally
            {
                _suppressSelection = false;
            }
        }
        /// <summary>컬럼이 많아 화면 폭을 넘어가므로 Fill 강제를 쓰지 않고 기본 폭 + 가로 스크롤을 허용한다.</summary>
        private void SetResultColumns(params string[] headers)
        {
            gvResults.Rows.Clear();
            gvResults.Columns.Clear();
            foreach (var h in headers)
            {
                var col = new DataGridViewTextBoxColumn
                {
                    HeaderText = h,
                    SortMode = DataGridViewColumnSortMode.NotSortable
                };
                gvResults.Columns.Add(col);
            }
        }
        #endregion </Result grid fill>

        #region <Layout / clearing>
        private void RefreshSearchBarTexts()
        {
            _lblTargetValue.Text = _mode == SearchMode.Substrate ? TargetSubstrate : TargetCarrier;
        }
        /// <summary>모드에 따라 캐리어/기판 상세 뷰를 스왑한다.</summary>
        private void ApplyModeLayout()
        {
            if (_mode == SearchMode.Carrier)
            {
                if (_carrierDetailView != null) _carrierDetailView.Visible = true;
                if (_substrateDetailView != null) _substrateDetailView.Visible = false;
            }
            else
            {
                if (_carrierDetailView != null) _carrierDetailView.Visible = false;
                if (_substrateDetailView != null) _substrateDetailView.Visible = true;
            }
        }
        private void ClearResults()
        {
            _suppressSelection = true;
            try
            {
                _carrierResults.Clear();
                _substrateResults.Clear();
                gvResults.Rows.Clear();
                gvResults.Columns.Clear();
            }
            finally
            {
                _suppressSelection = false;
            }
            _carrierDetailView.ClearDetail();
            _substrateDetailView.ClearDetail();
        }
        private void SetInputsEnabled(bool enabled)
        {
            _lblTargetValue.Enabled = enabled;
            _startDatePicker.Enabled = enabled;
            _endDatePicker.Enabled = enabled;
            _lblCond1Value.Enabled = enabled;
            _lblCond2Value.Enabled = enabled;
            _lblOtherFieldValue.Enabled = enabled;
            _lblOtherMatchToggle.Enabled = enabled;
            _lblOtherValueValue.Enabled = enabled;
            _btnSearch.Enabled = enabled;
            _btnExport.Enabled = enabled;
        }
        #endregion </Layout / clearing>

        #region <결과 그리드 ReadOnly 해제>
        /// <summary>결과 그리드의 ReadOnly 를 풀고 F2 편집모드를 허용한다(F2 로 편집창을 열어 셀 내용 선택/복사).
        /// gvResults 는 DataSource 없이 코드로 채우는 표시 전용이라 편집해도 DB 로 반영되는 경로가 없다.</summary>
        private void ReleaseGridReadOnly()
        {
            gvResults.ReadOnly = false;
            gvResults.EditMode = DataGridViewEditMode.EditOnF2;
        }
        #endregion </결과 그리드 ReadOnly 해제>

        #region <CSV Export>
        private void ExportResults()
        {
            if (gvResults.Rows.Count == 0 || gvResults.Columns.Count == 0)
                return;

            using (var dialog = new SaveFileDialog())
            {
                dialog.Filter = "CSV 파일 (*.csv)|*.csv";
                dialog.FileName = string.Format("DbQuery_{0}_{1:yyyyMMdd_HHmmss}.csv",
                    _mode == SearchMode.Carrier ? "Carrier" : "Substrate", DateTime.Now);
                if (dialog.ShowDialog() != DialogResult.OK)
                    return;

                var builder = new StringBuilder();

                var headers = new List<string>();
                for (int c = 0; c < gvResults.Columns.Count; ++c)
                    headers.Add(EscapeCsv(gvResults.Columns[c].HeaderText));
                builder.AppendLine(string.Join(",", headers));

                for (int r = 0; r < gvResults.Rows.Count; ++r)
                {
                    var cells = new List<string>();
                    for (int c = 0; c < gvResults.Columns.Count; ++c)
                    {
                        var value = gvResults.Rows[r].Cells[c].Value;
                        cells.Add(EscapeCsv(value == null ? string.Empty : value.ToString()));
                    }
                    builder.AppendLine(string.Join(",", cells));
                }

                // 엑셀 호환을 위해 UTF-8 BOM 포함
                File.WriteAllText(dialog.FileName, builder.ToString(), new UTF8Encoding(true));
            }
        }
        private static string EscapeCsv(string value)
        {
            if (string.IsNullOrEmpty(value))
                return string.Empty;

            if (value.IndexOfAny(new char[] { ',', '"', '\r', '\n' }) < 0)
                return value;

            return "\"" + value.Replace("\"", "\"\"") + "\"";
        }
        #endregion </CSV Export>

        #region <Helpers>
        private int CurrentRowIndex()
        {
            return gvResults.CurrentRow != null ? gvResults.CurrentRow.Index : -1;
        }
        private static string SourceText(MaterialSource source)
        {
            return source == MaterialSource.Main ? "현재" : "Archive";
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
        #endregion </Helpers>
    }
}
