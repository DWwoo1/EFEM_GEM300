using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Windows.Forms;

using EFEM.History;

namespace EFEM.CustomizedByProcessType.UserInterface.OperationMainHistory.PWA500Common
{
    /// <summary>
    /// 2026.07.06. jhlim [MOD] PWA500BIN/PWA500W 중복 구현을 공용화하고,
    /// 파일 경로 대신 이력 레코드 리스트를 표시하도록 변경. (조회 소스가 파일이든 DB든 무관)
    /// </summary>
    public partial class MainDisplaySubPanelLotHistoryDisplayer : UserControl
    {
        #region <Constructors>
        public MainDisplaySubPanelLotHistoryDisplayer(bool currentHistory)
        {
            InitializeComponent();

            IsCurrentHistory = currentHistory;

            this.Dock = DockStyle.Fill;
        }
        #endregion </Constructors>

        #region <Fields>
        private const int ColMessage = 4;
        private readonly bool IsCurrentHistory;
        #endregion </Fields>

        #region <Methods>

        #region <Externals>
        public void DisplayHistory(List<HistoryRecord> records)
        {
            BeginInvoke((Action)(() =>
            {
                FillGrid(records);
            }));
        }
        /// <summary>파일 직접 열람용. (조회 계층을 거치지 않는 수동 경로 - 기존 시그니처 유지)</summary>
        public void DisplayHistory(string path)
        {
            var records = new List<HistoryRecord>();
            try
            {
                if (false == string.IsNullOrEmpty(path) && File.Exists(path))
                {
                    using (FileStream fs = new FileStream(path, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
                    using (StreamReader sr = new StreamReader(fs))
                    {
                        while (false == sr.EndOfStream)
                        {
                            if (HistoryLineFormat.TryParse(sr.ReadLine(), DateTime.Now.Year, out HistoryRecord record))
                                records.Add(record);
                        }
                    }
                }
            }
            catch
            {
            }

            DisplayHistory(records);
        }
        public void AddCurrentHistory(string messageToAdd)
        {
            BeginInvoke((Action)(() =>
            {
                try
                {
                    var splitted = messageToAdd.Split('\t');
                    if (splitted.Length != gvLotHistory.ColumnCount)
                        return;

                    int row = gvLotHistory.Rows.Count;

                    gvLotHistory.Rows.Add();
                    for (int i = 0; i < splitted.Length; ++i)
                    {
                        gvLotHistory.Rows[row].Cells[i].Style.BackColor = Color.Bisque;

                        if (i == ColMessage)
                        {
                            gvLotHistory.Rows[row].Cells[i].Style.Alignment = DataGridViewContentAlignment.MiddleLeft;
                        }

                        gvLotHistory[i, row].Value = splitted[i];
                    }

                    gvLotHistory.FirstDisplayedScrollingRowIndex = gvLotHistory.Rows.Count - 1;
                }
                catch
                {
                }
            }));
        }
        public void CallFunctionByTimer()
        {
        }
        public void ProcessWhenDeactivation()
        {
        }
        #endregion </Externals>

        #region <UI Events>
        private void GvCellClicked(object sender, DataGridViewCellEventArgs e)
        {
            var row = e.RowIndex;
            if (row >= 0)
            {
                if (gvLotHistory[ColMessage, row].Value == null)
                    return;

                lblSelectedMessage.Text = gvLotHistory[ColMessage, row].Value.ToString();
            }
        }
        #endregion </UI Events>

        #region <Internal>
        private void FillGrid(List<HistoryRecord> records)
        {
            try
            {
                gvLotHistory.ClearSelection();
                gvLotHistory.Rows.Clear();
                if (records == null || records.Count == 0)
                    return;

                gvLotHistory.BackgroundColor = Color.White;
                for (int i = 0; i < records.Count; ++i)
                {
                    var record = records[i];
                    bool isCarrierEvent = false == string.IsNullOrEmpty(record.CarrierEventCode);

                    gvLotHistory.Rows.Add();
                    var cells = new string[]
                    {
                        HistoryLineFormat.ComposeTimestamp(record.Time),
                        record.CarrierEventCode,
                        record.SubstrateName,
                        record.SubstrateEventCode,
                        record.Message,
                    };
                    for (int j = 0; j < cells.Length; ++j)
                    {
                        gvLotHistory.Rows[i].Cells[j].Style.BackColor = isCarrierEvent ? Color.LightBlue : Color.Bisque;

                        if (j == ColMessage)
                        {
                            gvLotHistory.Rows[i].Cells[j].Style.Alignment = DataGridViewContentAlignment.MiddleLeft;
                        }

                        gvLotHistory[j, i].Value = cells[j];
                    }
                }

                if (false == IsCurrentHistory)
                {
                    gvLotHistory.FirstDisplayedScrollingRowIndex = 0;
                }
                else
                {
                    gvLotHistory.FirstDisplayedScrollingRowIndex = gvLotHistory.Rows.Count - 1;
                }
            }
            catch
            {
            }
        }
        #endregion </Internal>

        #endregion </Methods>
    }
}
