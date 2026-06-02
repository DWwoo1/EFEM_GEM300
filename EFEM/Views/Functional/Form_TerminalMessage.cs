using System;
using System.Collections.Generic;
using System.Collections.Concurrent;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using FrameOfSystem3.SECSGEM;

namespace FrameOfSystem3.Views.Functional
{
    /// <summary>
    /// 2021.05.23 jhlim : Host 로부터 받은 메시지를 출력하기 위한 창
    /// </summary>
    public partial class Form_TerminalMessage : Form
    {
        #region 싱글톤
        private static Form_TerminalMessage _Instance = null;
        public static Form_TerminalMessage GetInstance()
        {
            if (_Instance == null || _Instance.IsDisposed)
            {
                _Instance = new Form_TerminalMessage();
            }

            return _Instance;
        }
        private Form_TerminalMessage()
        {
            DoubleBuffered = true;

            InitializeComponent();

            gvMessageList.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            FormClosing += Form_TerminalMessage_FormClosing;
        }
        #endregion

        #region 변수
        private const int MAX_LENGTH = 32767;
        //private StringBuilder m_logSb = new StringBuilder(33000);
        private Dictionary<string, string> _messages = new Dictionary<string, string>();
        private static readonly string TIME_FORMAT = "yyyy/MM/dd HH:mm:ss.fff";
        #endregion

        #region <Private methods>
        private void AddMessage(string message)
        {
            if (_messages.Count > 20)
            {
                string temp = String.Empty;
                _messages.Remove(_messages.First().Key);
            }

            string curTime = DateTime.Now.ToString(TIME_FORMAT);
            _messages.Add(curTime, message);
            //if (_messages.ContainsKey(curTime))
            //{
            //}
            //else
            //{
            //    _messages.TryAdd(curTime, message);
            //}
        }

        private void DisplayGridViewAll()
        {
            gvMessageList.ClearSelection();
            gvMessageList.Rows.Clear();

            // 열이 정의되어 있는지 확인하고, 없다면 열 추가
            if (gvMessageList.Columns.Count == 0)
            {
                gvMessageList.Columns.Add("TIME", "TIME");
            }

            int index = 0;
            _messages = _messages.OrderBy(item => item.Key).ToDictionary(x => x.Key, x => x.Value);

            foreach (var item in _messages)
            {
                gvMessageList.Rows.Add();
                gvMessageList.Rows[index].Height = 30;
                gvMessageList[0, index].Value = item.Key;
                ++index;
            }

            int last = gvMessageList.Rows.Count - 1;

            // 마지막 셀 선택
            if (last >= 0)
            {
                gvMessageList.CurrentCell = gvMessageList.Rows[last].Cells[0];
            }
        }

        private void SelectAndDisplayMessage(int row)
        { 
            if (gvMessageList.Rows.Count > row)
            {
                string selectedTime = gvMessageList[0, row].Value.ToString();

                if (false == _messages.ContainsKey(selectedTime))
                    return;
            }
        }

        private void ClearMessage()
        {
            _messages.Clear();
            txtTerminalMessage.Clear();

            DisplayGridViewAll();
        }
        #endregion </Private methods>

        #region <Public methods>
        delegate void deleTerminalMessage_Called(string strMessage);
        public void EnqueueTerminalMessage(string strMessage)
        {
            if (this.InvokeRequired)
            {
                deleTerminalMessage_Called d = new deleTerminalMessage_Called(EnqueueTerminalMessage);
                this.BeginInvoke(d, new object[] { strMessage });
            }
            else
            {
                try
                {
                    if (false == String.IsNullOrEmpty(strMessage))
                    {
                        // 1. 메시지 추가
                        AddMessage(strMessage);

                        // 2. 그리드 새로 그림
                        DisplayGridViewAll();

                        // 3. 추가된 메시지를 표시한다.
                        txtTerminalMessage.Text = strMessage;
                        txtTerminalMessage.DeselectAll();
                    }

                    this.Show();
                    this.BringToFront();
                }
                catch (System.Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
            }
        }

        // 2022.01.12 by  Thienvv [ADD]
        public void ShowForm(bool bShow)
        {
            if (this.InvokeRequired)
            {
                this.BeginInvoke(new MethodInvoker(delegate()
                    {
                        if (bShow)
                        {
                            this.Show();
                        }
                        else
                        {
                            this.Hide();
                        }
                    }));
            }
            else
            {
                if (bShow)
                {
                    this.Show();
                }
                else
                {
                    this.Hide();
                }
            }
        }        

        public void ExitForm()
        {
            FormClosing -= Form_TerminalMessage_FormClosing;
            this.Close();
        }
        #endregion </Public methods>

        #region <UI Events>
        private void Control_Click(object sender, EventArgs e)
        {
            if (sender.Equals(btn_Clear))
            {
                ClearMessage();
            }
            if (sender.Equals(btn_Close))
            {
                this.Hide();
            }
        }

        private void gvMessageListCellClicked(object sender, DataGridViewCellEventArgs e)
        {
            int row = e.RowIndex;
            int col = e.ColumnIndex;

            if (row >= 0 && col == 0)
            {
                string selectedTime = gvMessageList[col, row].Value.ToString();

                if (false == _messages.ContainsKey(selectedTime))
                    return;

                txtTerminalMessage.Text = _messages[selectedTime];
            }
        }
        private void Form_TerminalMessage_FormClosing(object sender, FormClosingEventArgs e)
        {
            e.Cancel = true;  // 폼이 완전히 닫히는 것을 방지
            this.Hide();      // 단순히 숨기기
        }
        #endregion </UI Events>
    }
}
