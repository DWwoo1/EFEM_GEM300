using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using Define.DefineEnumProject.DigitalIO;
using DigitalIO_;

namespace FrameOfSystem3.Views.Functional
{
    public partial class Form_UserWaitingRequest : Form
    {
        public enum EN_RESULT
        {
            PROCESS,
            FINISH,
        }


        public static Form_UserWaitingRequest m_instance = null;
        public static Form_UserWaitingRequest GetInstance()
        {
            if(m_instance == null)
            {
                m_instance = new Form_UserWaitingRequest();
            }
            return m_instance;
        }
        Form_UserWaitingRequest()
        {
            InitializeComponent();
        }

        public EN_RESULT RESULT
        {
            get
            {
                return m_Result;
            }
        }

        delegate void DelShowDialogAsync();
        delegate void DelSetMessage(string msg = "", string title = "완료");

        Timer m_timer = new Timer();

        TickCounter_.TickCounter m_tickWaitingTime = new TickCounter_.TickCounter();
        
        string m_sTitleMsg = string.Empty;
        string m_sInformMessage = string.Empty;
        string m_sBtnOkMessage = string.Empty;
        string m_sBtnCancelMessage = string.Empty;
        bool m_bUseTimer = false;

        System.Threading.CancellationTokenSource _cancleToken;

        EN_RESULT m_Result = EN_RESULT.PROCESS;

        Point m_gbMouseDownPoint = new Point();

        public async void Inform(string title, System.Threading.CancellationTokenSource cancleToken)
        {
            _cancleToken = cancleToken;

            m_sTitleMsg = title;;
            m_sInformMessage = "요청하신 작업의 완료를 대기중입니다.\n잠시만 기다려주세요.";

            m_Result = EN_RESULT.PROCESS;

            var result2 = System.Threading.Tasks.Task.Run(() => ShowDialogAsync());
            await result2;
        }

        void ShowDialogAsync()
        {
            if(this.InvokeRequired)
            {
                DelShowDialogAsync d = new DelShowDialogAsync(ShowDialogAsync);
                this.BeginInvoke(d, new object[] { });
            }
            else
            {
                m_gbUserMessage.Text = m_sTitleMsg;
                m_lbMessage.Text = m_sInformMessage;

                if (false == this.Visible)
                {
                    _btnAbort.Visible = true;
                    m_btnClose.Visible = false;
                    this.StartPosition = FormStartPosition.CenterParent;

                    this.ShowDialog();
                }
                else
                {
                    this.BringToFront();
                    this.TopMost = true;
                }
            }
            return;
        }
        public void CloseDialogAsync()
        {
            if (this.InvokeRequired)
            {
                DelShowDialogAsync d = new DelShowDialogAsync(CloseDialogAsync);
                this.BeginInvoke(d, new object[] { });
            }
            else
            {
                m_Result = EN_RESULT.FINISH;

                this.StartPosition = FormStartPosition.CenterParent;
                this.DialogResult = System.Windows.Forms.DialogResult.OK;
            }
            return;
        }

        public void SetMessage(string msg = "", string title = "완료")
        {
            if (this.InvokeRequired)
            {
                DelSetMessage d = new DelSetMessage(SetMessage);
                this.BeginInvoke(d, new object[] { msg, title });
            }
            else
            {
                _btnAbort.Visible = false;
                m_btnClose.Visible = true;
                if(string.IsNullOrEmpty(msg))
                {
                    m_sTitleMsg = title;
                    m_sInformMessage = "요청하신 작업이 완료되었습니다";
                }
                else
                {
                    m_sTitleMsg = title;
                    m_sInformMessage = msg;
                }

                m_gbUserMessage.Text = m_sTitleMsg;
                m_lbMessage.Text = m_sInformMessage;
                ShowDialogAsync();
            }
        }

        void Close_Click(object sender, EventArgs e)
        {
            this.DialogResult = System.Windows.Forms.DialogResult.OK;
        }

        void Form_Load(object sender, EventArgs e)
        {
            m_gbUserMessage.Text = m_sTitleMsg;
        }

        void m_timer_Tick(object sender, EventArgs e)
        {
            if(m_tickWaitingTime.IsTickOver(false))
            {
                Close_Click(null, null);
            }
        }

        void Form_InformOccurNG_VisibleChanged(object sender, EventArgs e)
        {
            if(Visible)
            {
                this.BringToFront();
                this.TopMost = true;
                if (m_bUseTimer)
                {
                    m_timer.Start();
                }
            }
            else
            {
                m_timer.Stop();
            }
        }
        void Form_UserInformTimer_Activated(object sender, EventArgs e)
        {
            this.BringToFront();
            this.TopMost = true;
        }

        void Abort_Click(object sender, EventArgs e)
        {
            if(_cancleToken != null)
            {
                _cancleToken.Cancel();
                // this.DialogResult = System.Windows.Forms.DialogResult.OK;

            }

            m_Result = EN_RESULT.FINISH;
            this.DialogResult = System.Windows.Forms.DialogResult.OK;
            //this.Hide();
        }

        void m_gbUserMessage_MouseDown(object sender, MouseEventArgs e)
        {
            m_gbMouseDownPoint = new Point(e.X, e.Y);
        }

        void m_gbUserMessage_MouseMove(object sender, MouseEventArgs e)
        {
            if((e.Button & MouseButtons.Left) == MouseButtons.Left)
            {
                this.Location = new Point(this.Left - (m_gbMouseDownPoint.X - e.X), this.Top - (m_gbMouseDownPoint.Y - e.Y));
            }
        }
    }
}
