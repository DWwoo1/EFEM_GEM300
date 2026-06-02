using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;

using System.Text.RegularExpressions;

namespace EFEM.CustomizedByProcessType.UserInterface.OperationMainHistory.PWA500W
{
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
        private const int NormalMessageLength = 5;
        private const int ColCarrierEvent = 1;
        private const int ColMessage = 4;
        private readonly bool IsCurrentHistory;
        #endregion </Fields>

        #region <Properties>
        #endregion </Properties>

        #region <Methods>

        #region <Externals>
        public async void DisplayHistory(string path)
        {
            await DisplayGridControl(path);
        }
        public async void AddCurrentHistory(string messageToAdd)
        {
            await AddHistoryAtLast(messageToAdd);
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

                string selectedData = gvLotHistory[ColMessage, row].Value.ToString();
                DisplayMessageContent(selectedData);
            }
        }
        #endregion </UI Events>

        #region <Internal>
        private async Task AddHistoryAtLast(string history)
        {
            var tcs = new TaskCompletionSource<bool>();

            BeginInvoke((Action)(() =>
            {
                try
                {
                    var splitted = history.Split('\t');
                    if (splitted.Length != gvLotHistory.ColumnCount)
                        return;

                    int row = gvLotHistory.Rows.Count;
                    
                    gvLotHistory.Rows.Add();
                    for (int i = 0; i < splitted.Length; ++i)
                    {
                        gvLotHistory.Rows[row].Cells[i].Style.BackColor = Color.Bisque;

                        if (i == 4)
                        {
                            gvLotHistory.Rows[row].Cells[i].Style.Alignment = DataGridViewContentAlignment.MiddleLeft;
                        }

                        gvLotHistory[i, row].Value = splitted[i];
                    }

                    gvLotHistory.FirstDisplayedScrollingRowIndex = gvLotHistory.Rows.Count - 1;
                }
                catch (Exception ex)
                {
                    tcs.SetException(ex);
                }
            }));

            await tcs.Task; // UI 업데이트 완료 대기         
        }

        private async Task DisplayGridControl(string historyPath)
        {
            var tcs = new TaskCompletionSource<bool>();

            BeginInvoke((Action)(() =>
            {
                string temoporaryFilePath = string.Empty;

                try
                {
                    gvLotHistory.ClearSelection();
                    gvLotHistory.Rows.Clear();
                    if (string.IsNullOrEmpty(historyPath))
                    {
                        return;
                    }

                    var dirName = Path.GetDirectoryName(historyPath);
                    if (false == Directory.Exists(dirName))
                    {
                        Directory.CreateDirectory(dirName);
                    }

                    if (false == File.Exists(historyPath))
                    {
                        return;
                    }

                    var fileName = Path.GetFileNameWithoutExtension(historyPath);
                    temoporaryFilePath = string.Format(@"{0}\Temporary_{1}.txt", dirName, fileName);
                    File.Copy(historyPath, temoporaryFilePath);
                    if (false == File.Exists(temoporaryFilePath))
                    {
                        return;
                    }

                    string[] lines = null;
                    using (FileStream fs = new FileStream(temoporaryFilePath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
                    using (StreamReader sr = new StreamReader(fs))
                    {
                        var linesToEnd = sr.ReadToEnd();
                        lines = Regex.Split(linesToEnd, @"\r\n|\r|\n");//linesToEnd.Split('\n');
                    }

                    gvLotHistory.BackgroundColor = Color.White;
                    for (int i = 0; lines != null && i < lines.Length; ++i)
                    {
                        if (string.IsNullOrEmpty(lines[i]))
                            continue;

                        gvLotHistory.Rows.Add();
                        var line = lines[i].Split('\t');
                        for (int j = 0; j < line.Length && line.Length == 5; ++j)
                        {
                            if (false == string.IsNullOrEmpty(line[1]))
                            {
                                gvLotHistory.Rows[i].Cells[j].Style.BackColor = Color.LightBlue;
                            }
                            else
                            {
                                gvLotHistory.Rows[i].Cells[j].Style.BackColor = Color.Bisque;
                            }

                            if (j == 4)
                            {
                                gvLotHistory.Rows[i].Cells[j].Style.Alignment = DataGridViewContentAlignment.MiddleLeft;
                            }

                            gvLotHistory[j, i].Value = line[j];
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

                    File.Delete(temoporaryFilePath);
                }
                catch (Exception ex)
                {
                    if (false == string.IsNullOrEmpty(temoporaryFilePath))
                    {
                        if (File.Exists(temoporaryFilePath))
                            File.Delete(temoporaryFilePath);
                    }
                    tcs.SetException(ex);
                }

                if (false == string.IsNullOrEmpty(temoporaryFilePath))
                {
                    if (File.Exists(temoporaryFilePath))
                        File.Delete(temoporaryFilePath);
                }
            }));

            await tcs.Task; // UI 업데이트 완료 대기            
        }

        private void DisplayMessageContent(string messageToDisplay)
        {
            lblSelectedMessage.Text = messageToDisplay;
        }
        #endregion </Internal>

        #endregion </Methods>
    }
}