using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

using FrameOfSystem3.Views;
using FrameOfSystem3.Views.Functional;

using EFEM.CustomizedByProcessType.PWA500Common;
using EFEM.History;

namespace EFEM.CustomizedByProcessType.UserInterface.OperationMainHistory.PWA500Common
{
    /// <summary>
    /// 2026.07.06. jhlim [MOD] PWA500BIN/PWA500W 중복 구현을 공용화.
    /// 데이터 접근을 폴더 스캔/파일 파싱에서 IHistoryQuery(파일 우선 + DB 폴백 자동 선택)로 교체하고
    /// CSV 내보내기를 추가.
    /// </summary>
    public partial class SubViewOperationHistoryLotHistory : UserControlForMainView.CustomView
    {
        #region <Constructors>
        public SubViewOperationHistoryLotHistory()
        {
            InitializeComponent();

            _lotHistoryLog = LotHistoryLog.Instance;
            _selectionList = Form_SelectionList.GetInstance();
            _formSummary = FormProductionSummary.Instance;

            UserControlSelectedHistory = new MainDisplaySubPanelLotHistoryDisplayer(false)
            {
                Dock = DockStyle.Fill
            };
            tableLayoutPanel1.Controls.Add(UserControlSelectedHistory, 1, 0);

            _substrateType = SubType.Core;
            LotList = new List<LotSummary>();
            _displayedRecords = new List<HistoryRecord>();

            SubstrateTypes = new Dictionary<int, string>();
            var subTypes = Enum.GetNames(typeof(SubType));
            for (int i = 0; i < subTypes.Length; ++i)
            {
                SubstrateTypes[i] = subTypes[i].ToString();
            }

            Dock = DockStyle.Fill;
        }
        #endregion </Constructors>

        #region <Type>
        enum SubType
        {
            Core,
            Bin
        }
        #endregion </Type>

        #region <Fields>
        private static LotHistoryLog _lotHistoryLog = null;
        private static Form_SelectionList _selectionList = null;
        private static FormProductionSummary _formSummary = null;

        #region <History Variables>
        private SubType _substrateType;
        private readonly List<LotSummary> LotList = null;

        private DateTime _selectedDate;
        private string _selectedLotId = string.Empty;
        private string _selectedCarrierId = string.Empty;

        // 마지막으로 표시한 랏 상세 (CSV 내보내기 대상)
        private List<HistoryRecord> _displayedRecords = null;

        // 조회는 UI 스레드를 막지 않도록 백그라운드에서 수행한다.
        // 연타로 이전 조회 결과가 늦게 도착하는 역전은 시퀀스 번호로 무시한다.
        private int _lotListQuerySequence = 0;
        private int _detailQuerySequence = 0;

        private readonly Dictionary<int, string> SubstrateTypes = null;
        #endregion </History Variables>

        #region <User Control>
        private readonly MainDisplaySubPanelLotHistoryDisplayer UserControlSelectedHistory = null;
        #endregion </User Control>

        #endregion </Fields>

        #region <Methods>

        #region <Override Methods>
        protected override void ProcessWhenActivation()
        {
            BeginInvoke((MethodInvoker)delegate
            {
                calander.MaxDate = DateTime.Today;
                calander.SelectionStart = DateTime.Today;
                calander.SelectionEnd = DateTime.Today;
            });

            base.ProcessWhenActivation();
        }
        public override void CallFunctionByTimer()
        {
            lblCarrierName.Text = _selectedCarrierId;
            lblSelectedSubstrateType.Text = _substrateType.ToString();

            if (LotList == null ||
                LotList.Count == 0)
                btnApply.Enabled = false;
            else
                btnApply.Enabled = true;

            btnExport.Enabled = _displayedRecords != null && _displayedRecords.Count > 0;

            base.CallFunctionByTimer();
        }
        protected override void ProcessWhenDeactivation()
        {
        }
        #endregion </Override Methods>

        #region <UI Events>
        private void CalanderDateChanged(object sender, DateRangeEventArgs e)
        {
            _selectedDate = e.Start;

            // 조회(파일 스캔 + DB)는 백그라운드에서 수행하고 결과만 UI에 반영한다.
            var date = _selectedDate;
            var category = CategoryName();
            int sequence = Interlocked.Increment(ref _lotListQuerySequence);
            Task.Run(() =>
            {
                var lots = _lotHistoryLog.GetQuery().GetLots(date, category);
                InvokeOnUi(() =>
                {
                    if (sequence != _lotListQuerySequence)
                        return;

                    LotList.Clear();
                    LotList.AddRange(lots);
                    UpdateGridView();
                });
            });
        }
        private void CalanderSelected(object sender, DateRangeEventArgs e)
        {

        }
        private void LblClicked(object sender, EventArgs e)
        {
            if (_selectionList.CreateForm("Select Type", SubstrateTypes.Values.ToArray(), SubstrateTypes.Keys.ToArray(), (int)_substrateType))
            {
                string selectedType = string.Empty;
                _selectionList.GetResult(ref selectedType);
                Enum.TryParse(selectedType, out _substrateType);
            }
        }
        private void BtnClicked(object sender, EventArgs e)
        {
            if (sender.Equals(btnApply))
            {
                if (false == string.IsNullOrEmpty(_selectedLotId))
                {
                    var date = _selectedDate;
                    var category = CategoryName();
                    var lotId = _selectedLotId;
                    int sequence = Interlocked.Increment(ref _detailQuerySequence);
                    Task.Run(() =>
                    {
                        var records = _lotHistoryLog.GetQuery().GetLotHistory(date, category, lotId);
                        InvokeOnUi(() =>
                        {
                            if (sequence != _detailQuerySequence)
                                return;

                            if (records == null || records.Count == 0)
                                return;

                            UserControlSelectedHistory.DisplayHistory(records);
                            _displayedRecords = records;
                            _selectedCarrierId = FindCarrierId(records);
                        });
                    });
                }
            }
            else if (sender.Equals(btnViewSummary))
            {
                bool isCore = _substrateType.Equals(SubType.Core) ? true : false;
                var date = _selectedDate;
                var category = CategoryName();
                Task.Run(() =>
                {
                    var lotSubstrates = _lotHistoryLog.GetQuery().GetLotSubstrates(date, category);
                    InvokeOnUi(() =>
                    {
                        _formSummary.ShowMessage(isCore, date, lotSubstrates);
                    });
                });
            }
            else if (sender.Equals(btnExport))
            {
                ExportDisplayedRecords();
            }
        }

        private void GvCellClicked(object sender, DataGridViewCellEventArgs e)
        {
            var row = e.RowIndex;
            if (row >= 0)
            {
                _selectedLotId = gvLotList[colLotName.Index, row].Value.ToString();
            }
            else
            {
                _selectedLotId = string.Empty;
            }
        }
        #endregion </UI Events>

        #region <Internals>
        private string CategoryName()
        {
            return _substrateType.ToString();
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
        private void UpdateGridView()
        {
            gvLotList.Rows.Clear();
            for (int i = 0; i < LotList.Count; ++i)
            {
                gvLotList.Rows.Add();
                gvLotList[colCreatedTime.Index, i].Value = LotList[i].CreatedTime.ToString("HH:mm:ss");
                gvLotList[colLotName.Index, i].Value = LotList[i].LotId;
            }

            gvLotList.ClearSelection();
        }
        private static string FindCarrierId(List<HistoryRecord> records)
        {
            for (int i = 0; i < records.Count; ++i)
            {
                if (false == string.IsNullOrEmpty(records[i].CarrierId))
                    return records[i].CarrierId;
            }

            return string.Empty;
        }
        private void ExportDisplayedRecords()
        {
            if (_displayedRecords == null || _displayedRecords.Count == 0)
                return;

            using (var dialog = new SaveFileDialog())
            {
                dialog.Filter = "CSV 파일 (*.csv)|*.csv";
                dialog.FileName = string.Format("LotHistory_{0}_{1:yyyyMMdd}.csv", _selectedLotId, _selectedDate);
                if (dialog.ShowDialog() != DialogResult.OK)
                    return;

                var builder = new StringBuilder();
                builder.AppendLine("TIME,PORT EVENT,WAFER,WAFER EVENT,MESSAGE");
                for (int i = 0; i < _displayedRecords.Count; ++i)
                {
                    var record = _displayedRecords[i];
                    builder.AppendLine(string.Join(",",
                        EscapeCsv(record.Time.ToString("yyyy-MM-dd HH:mm:ss.fff")),
                        EscapeCsv(record.CarrierEventCode),
                        EscapeCsv(record.SubstrateName),
                        EscapeCsv(record.SubstrateEventCode),
                        EscapeCsv(record.Message)));
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
        #endregion </Internals>

        #endregion </Methods>
    }
}
