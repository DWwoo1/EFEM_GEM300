using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using Define.DefineEnumProject.DigitalIO;
using FrameOfSystem3.Config;
using System.Drawing;

using FrameOfSystem3.SECSGEM.DefineSecsGem;

namespace FrameOfSystem3.Views.Functional
{
    public partial class Form_OperatorCall : Form
    {
        #region <Constant>
        private readonly Color m_clError = Color.Red;
        private readonly Color m_clWarning = Color.OrangeRed;
        private readonly Color m_clInfo = Color.LimeGreen;
        #endregion </Constant>

        #region <Singleton>
        private static Form_OperatorCall _Instance = null;
        public static Form_OperatorCall GetInstance()
        {
            if (_Instance == null)
            {
                _Instance = new Form_OperatorCall();
            }

            return _Instance;
        }
        private Form_OperatorCall()
        {
            DoubleBuffered = true;

            InitializeComponent();
        }
        #endregion </Singleton>

        #region <Variable>
        //private bool m_bShown = false;

        private const int MAX_LENGTH = 32767;
        private StringBuilder m_logSb = new StringBuilder(33000);
        private int IndexOFBuzzer = -1;
        #endregion </Variable>

        #region <Private methods>
        private void SetBuzzer(bool bDoOn)
        {
            if (IndexOFBuzzer >= 0)
            {
                DigitalIO_.DigitalIO.GetInstance().WriteOutput(IndexOFBuzzer, bDoOn);
            }
        }

        private void ScrollToLast()
        {
            txt_Messsage.SelectionStart = 0;
            txt_Messsage.ScrollToCaret(); // 2022.01.26 by Thienvv [ADD]
        }

        private void ClearMessage()
        {
            m_logSb.Clear();
            txt_Messsage.Clear();
        }
        #endregion </Private methods>

        #region <Public methods>
        delegate bool deleOperatorCall_Called(EN_OPCALL_LEVEL enLevel, string strOperatorId, bool bBuzzer, string strMsg);
        public bool ShowMessage(EN_OPCALL_LEVEL enLevel, string strOperatorId, bool bBuzzer, string strMsg)
        {
			if (this.InvokeRequired)
            {
                deleOperatorCall_Called d = new deleOperatorCall_Called(ShowMessage);
                this.BeginInvoke(d, new object[] { enLevel, strOperatorId, bBuzzer, strMsg });
            }
            else
            {
                try
                {
                    if (bBuzzer)
                        SetBuzzer(true);

                    switch (enLevel)
                    {
                        case EN_OPCALL_LEVEL.INFO:
                            groupBox_Title.LabelGradientColorFirst = m_clInfo;
                            groupBox_Title.LabelGradientColorSecond = m_clInfo;
                            break;

                        case EN_OPCALL_LEVEL.WARNING:
                            groupBox_Title.LabelGradientColorFirst = m_clWarning;
                            groupBox_Title.LabelGradientColorSecond = m_clWarning;
                            break;

                        case EN_OPCALL_LEVEL.ERROR:
                            groupBox_Title.LabelGradientColorFirst = m_clError;
                            groupBox_Title.LabelGradientColorSecond = m_clError;
                            break;

                        case EN_OPCALL_LEVEL.DOWN:
                            groupBox_Title.LabelGradientColorFirst = m_clError;
                            groupBox_Title.LabelGradientColorSecond = m_clError;
                            break;

                        case EN_OPCALL_LEVEL.ETC:
                            groupBox_Title.LabelGradientColorFirst = m_clInfo;
                            groupBox_Title.LabelGradientColorSecond = m_clInfo;
                            break;

                        default:
                            groupBox_Title.LabelGradientColorFirst = m_clInfo;
                            groupBox_Title.LabelGradientColorSecond = m_clInfo;
                            break;
                    }

                    groupBox_Title.Text = string.Format("OPERATOR CALL - {0}", enLevel.ToString());

                    DateTime today = DateTime.Now;
                    string strText = string.Format("[{0:d2}/{1:d2}-{2:d2}:{3:d2}:{4:d2}.{5:d3}]\r\nOP_LEVEL: [ {6} ] \r\nOPCALL_ID: [ {7} ]\r\n{8}",
                        today.Month, today.Day,
                        today.Hour, today.Minute, today.Second, today.Millisecond,
                        enLevel.ToString(),
                        strOperatorId,
                        strMsg);
                    while (m_logSb.Length > MAX_LENGTH)
                    {
                        m_logSb.Remove(MAX_LENGTH - 1, m_logSb.Length - MAX_LENGTH);
                    }
                    m_logSb.Insert(0, string.Format("{0}\r\n-----------------------------------------------------------------------\r\n", strText));

                    txt_Messsage.Text = m_logSb.ToString();
                    ScrollToLast();

                    this.Show();
                    this.BringToFront();
                    //m_bShown = true;
                }
                catch (System.Exception ex)
                {
                    Console.WriteLine(ex.Message);
                    return false;
                }
            }

            return true;
        }

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

        public void SetBuzzerIndex(int index)
        {
            IndexOFBuzzer = index;
        }
        #endregion </Public methods>

        #region <UI Events>
        private void Buttons_Click(object sender, EventArgs e)
        {
            if (sender.Equals(btn_Close))
            {
                //m_bShown = false;
                ClearMessage();
                SetBuzzer(false);

                this.Hide();
            }
            if (sender.Equals(btn_BuzzerOff))
            {
                SetBuzzer(false);
            }
        }
        #endregion </UI Events>
    }
}
