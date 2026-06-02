using Sys3Controls;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace FrameOfSystem3.Views.Functional
{
    /// <summary>
    /// 2018.06.23 by yjlee [ADD] OK , CANCEL을 위한 확인용 메시지 폼이다.
    /// </summary>
    public partial class Form_MessageBox : Form
    {
        #region 싱글톤
        private static Form_MessageBox _Instance = null;
        public static Form_MessageBox GetInstance()
        {
            if (_Instance == null)
            {
                _Instance = new Form_MessageBox();
            }

            return _Instance;
        }
        private Form_MessageBox()
        {
            DoubleBuffered = true;

			m_InstanceOfLanguage	= FrameOfSystem3.Config.ConfigLanguage.GetInstance();

            InitializeComponent();

			DEFAULT_SIZE_LABEL = new Size(m_labelMessage.Size.Width, m_labelMessage.Size.Height);
			DEFAULT_SIZE_FORM = new Size(this.Size.Width, this.Size.Height);

			DEFAULT_SIZE_EXCEPT_FOR_MESSAGE_LABEL = new Size(DEFAULT_SIZE_FORM.Width - m_labelMessage.Width, DEFAULT_SIZE_FORM.Height - m_labelMessage.Height);

			MAX_SIZE_FORM = new Size(Screen.PrimaryScreen.Bounds.Width - SCREEN_MARGIN_PIXELS
										, Screen.PrimaryScreen.Bounds.Height - SCREEN_MARGIN_PIXELS);

			MAX_SIZE_LABEL = new Size(MAX_SIZE_FORM.Width - DEFAULT_SIZE_EXCEPT_FOR_MESSAGE_LABEL.Width, MAX_SIZE_FORM.Height - DEFAULT_SIZE_EXCEPT_FOR_MESSAGE_LABEL.Height);
        }
		#endregion

		#region 상수
		const int SCREEN_MARGIN_PIXELS = 200;

		readonly string OK_BUTTON_TEXT = "OK";
		readonly string CANCEL_BUTTON_TEXT = "CANCEL";
		readonly string YES_BUTTON_TEXT = "YES";
		readonly string NO_BUTTON_TEXT = "NO";

		readonly Size MAX_SIZE_LABEL;
		readonly Size MAX_SIZE_FORM;

		readonly Size DEFAULT_SIZE_EXCEPT_FOR_MESSAGE_LABEL;
		readonly Size DEFAULT_SIZE_LABEL;
		readonly Size DEFAULT_SIZE_FORM;
		#endregion

		#region 변수

		#region Instance
		FrameOfSystem3.Config.ConfigLanguage m_InstanceOfLanguage		= null;
		#endregion

		#region GUI
		private bool m_bMouseDownTitle					= false;
		private Point m_pMouseDownCoordinate			= new Point();
		#endregion

		#endregion

		#region Internal Interface
		/// <summary>
		/// 2021.08.24 by twkang [ADD] 이벤트 처리
		/// </summary>
		private void ProcessingEvent(Keys enInputedKey)
		{
			switch(enInputedKey)
			{
				case Keys.Enter:
					this.DialogResult = System.Windows.Forms.DialogResult.OK;
					break;
				case Keys.Escape:
					this.DialogResult = System.Windows.Forms.DialogResult.Cancel;
					break;
				default:
					return;
			}

			this.Close();
		}

		/// <summary>
		/// 2024.12.02. by shkim. [MOD] TableLayout 활용하여 Style에 따라 버튼 표시 유무와 위치 조절
		/// 2024.01.17 by junho [ADD] button style 추가
		/// </summary>
		private void ChangeStyle(EN_STYLE style)
		{
			this._panel_Button.Controls.Remove(this.m_btnOK);
			this._panel_Button.Controls.Remove(this.m_btnCancel);

			switch (style)
			{
				case EN_STYLE.YesNo:
					m_btnOK.Text = m_InstanceOfLanguage.TranslateWord(YES_BUTTON_TEXT);
					m_btnCancel.Text = m_InstanceOfLanguage.TranslateWord(NO_BUTTON_TEXT);
					m_btnCancel.Visible = true;

					// m_btnOK.Location = new Point(140, 138);
					this._panel_Button.Controls.Add(this.m_btnOK, 1, 1);
					this._panel_Button.Controls.Add(this.m_btnCancel, 3, 1);
					_panel_Button.SetColumnSpan(m_btnOK, 1);
					break;
				case EN_STYLE.OkOnly:
					m_btnOK.Text = m_InstanceOfLanguage.TranslateWord(OK_BUTTON_TEXT);
					m_btnCancel.Text = m_InstanceOfLanguage.TranslateWord(CANCEL_BUTTON_TEXT);
					m_btnCancel.Visible = false;

					// m_btnOK.Location = new Point(200, 138);
					this._panel_Button.Controls.Add(this.m_btnOK, 1, 1);
					_panel_Button.SetColumnSpan(m_btnOK, 3);
					break;
				default:
					m_btnOK.Text = m_InstanceOfLanguage.TranslateWord(OK_BUTTON_TEXT);
					m_btnCancel.Text = m_InstanceOfLanguage.TranslateWord(CANCEL_BUTTON_TEXT);
					m_btnCancel.Visible = true;

					// m_btnOK.Location = new Point(140, 138);
					this._panel_Button.Controls.Add(this.m_btnOK, 1, 1);
					this._panel_Button.Controls.Add(this.m_btnCancel, 3, 1);
					_panel_Button.SetColumnSpan(m_btnOK, 1);
					break;
			}
		}

		// 2024.12.02. by shkim. [ADD] Sys3Control에서 EN_TEXTALIGN 으로 StringFormat 설정할 때 사용하는 로직
		static class TextAlignerByEnum
		{
			static public void SetTextAlign(ref StringFormat strFormat, EN_TEXTALIGN enTextAlign)
			{
				switch (enTextAlign)
				{
					case EN_TEXTALIGN.TOP_LEFT:
						strFormat.LineAlignment = StringAlignment.Near;
						strFormat.Alignment = StringAlignment.Near;
						break;
					case EN_TEXTALIGN.TOP_CENTER:
						strFormat.LineAlignment = StringAlignment.Near;
						strFormat.Alignment = StringAlignment.Center;
						break;
					case EN_TEXTALIGN.TOP_RIGHT:
						strFormat.LineAlignment = StringAlignment.Near;
						strFormat.Alignment = StringAlignment.Far;
						break;
					case EN_TEXTALIGN.MIDDLE_LEFT:
						strFormat.LineAlignment = StringAlignment.Center;
						strFormat.Alignment = StringAlignment.Near;
						break;
					case EN_TEXTALIGN.MIDDLE_CENTER:
						strFormat.LineAlignment = StringAlignment.Center;
						strFormat.Alignment = StringAlignment.Center;
						break;
					case EN_TEXTALIGN.MIDDLE_RIGHT:
						strFormat.LineAlignment = StringAlignment.Center;
						strFormat.Alignment = StringAlignment.Far;
						break;
					case EN_TEXTALIGN.BOTTOM_LEFT:
						strFormat.LineAlignment = StringAlignment.Far;
						strFormat.Alignment = StringAlignment.Near;
						break;
					case EN_TEXTALIGN.BOTTOM_CENTER:
						strFormat.LineAlignment = StringAlignment.Far;
						strFormat.Alignment = StringAlignment.Center;
						break;
					case EN_TEXTALIGN.BOTTOM_RIGHT:
						strFormat.LineAlignment = StringAlignment.Far;
						strFormat.Alignment = StringAlignment.Far;
						break;
				}
			}
		}

		/// <summary>
		/// 2024.12.02. by shkim. [MOD] Font->MainFont Bug Fix, Table Layout 변경에 따른 사이즈 계산 로직 변경
		/// </summary>
		/// <param name="message"></param>
		private void AdjustFormSize(string message)
		{
			const int SIZE_OF_INFLATE = 3;	// Sys3Control에서 Text 영역에 대해 inflate(-) 하는 상수
			int scrollbarWidth = SystemInformation.VerticalScrollBarWidth;

			const int HORIZONTAL_TEXT_MARGIN = 50;	// 좌우여백

            using (Graphics g = this.CreateGraphics())
			{
				StringFormat format = new StringFormat();

				try
				{
					TextAlignerByEnum.SetTextAlign(ref format, m_labelMessage.TextAlignMain);
					
					
					// 텍스트 폭에 감안해야할 것들 : 스크롤바 폭, 라벨의 마진, 축소된 텍스트 영역
					SizeF measuredStringSize = g.MeasureString(message
										, m_labelMessage.MainFont
										, new SizeF(MAX_SIZE_LABEL.Width - (scrollbarWidth + m_labelMessage.Margin.Left + m_labelMessage.Margin.Right + SIZE_OF_INFLATE + HORIZONTAL_TEXT_MARGIN)
										, int.MaxValue), format);

					// 실제 필요한 라벨 크기(라벨은 스크롤바를 위해 패널의 스크롤바 영역보다 작아야한다.)
					measuredStringSize.Width += (m_labelMessage.Margin.Left + m_labelMessage.Margin.Right + SIZE_OF_INFLATE + HORIZONTAL_TEXT_MARGIN);
					measuredStringSize.Height += (m_labelMessage.Margin.Top + m_labelMessage.Margin.Bottom + SIZE_OF_INFLATE);

					bool isWidthInflated = measuredStringSize.Width > DEFAULT_SIZE_LABEL.Width;
					bool isHeightInflated = measuredStringSize.Height > DEFAULT_SIZE_LABEL.Height;

                    bool isHeightMaxOver = measuredStringSize.Height > MAX_SIZE_LABEL.Height;

					if (isHeightMaxOver)
					{
						m_labelMessage.Dock = DockStyle.None;
						m_labelMessage.Location = new Point(0, 0);

						if (isWidthInflated)
						{
                            m_labelMessage.Size = new Size((int)Math.Ceiling(measuredStringSize.Width), (int)Math.Ceiling(measuredStringSize.Height));
                        }
						else
						{
							m_labelMessage.Size = new Size(DEFAULT_SIZE_LABEL.Width - scrollbarWidth, (int)Math.Ceiling(measuredStringSize.Height));
                        }

						int addtionalWidthNeeded = 0;
						int additionalHeightNeeded = 0;

						if (isWidthInflated)
						{
							addtionalWidthNeeded = m_labelMessage.Size.Width - DEFAULT_SIZE_LABEL.Width + scrollbarWidth;
						}

						additionalHeightNeeded = MAX_SIZE_LABEL.Height - DEFAULT_SIZE_LABEL.Height;
						this.Size = new Size(DEFAULT_SIZE_FORM.Width + addtionalWidthNeeded, DEFAULT_SIZE_FORM.Height + additionalHeightNeeded);
					}
					else
					{
						m_labelMessage.Dock = DockStyle.Fill;

                        int addtionalWidthNeeded = 0;
                        int additionalHeightNeeded = 0;

                        if (isWidthInflated)
						{
                            addtionalWidthNeeded = (int)measuredStringSize.Width - DEFAULT_SIZE_LABEL.Width;
                        }
						if (isHeightInflated)
						{
                            additionalHeightNeeded = (int)measuredStringSize.Height - DEFAULT_SIZE_LABEL.Height;
                        }

                        this.Size = new Size(DEFAULT_SIZE_FORM.Width + addtionalWidthNeeded, DEFAULT_SIZE_FORM.Height + additionalHeightNeeded);
                    }
				}
				finally
				{
					format.Dispose();
				}	
			}

			////// 메시지 내용에 따라 폼 크기 조정
			//Size textSize = TextRenderer.MeasureText(m_labelMessage.Text, m_labelMessage.Font
			//	, new Size(Screen.PrimaryScreen.Bounds.Width - 200, 0), TextFormatFlags.WordBreak);

			//textSize.Width += 20;   // 텍스트 너비에 여유 공간 20 추가
			//textSize.Height += 50; // 텍스트 높이에 여유 공간 50 추가

			//// 크기 계산
			//int offsetWidth = (textSize.Width > DEFAULT_SIZE_TEXTBOX.Width) ? textSize.Width - DEFAULT_SIZE_TEXTBOX.Width : 0;
			//int offsetHeight = (textSize.Height > DEFAULT_SIZE_TEXTBOX.Height) ? textSize.Height - DEFAULT_SIZE_TEXTBOX.Height : 0;

			//// 폼 크기 조정
			//this.Size = new Size(DEFAULT_SIZE_FORM.Width + offsetWidth, DEFAULT_SIZE_FORM.Height + offsetHeight);
			//panel_Main.Size = new Size(DEFAULT_SIZE_PANEL.Width + offsetWidth, DEFAULT_SIZE_PANEL.Height + offsetHeight);
		}
		#endregion

		#region 외부 인터페이스
		/// <summary>
		/// 2018.06.23 by yjlee [ADD] 메시지 창을 화면에 출력한다.
		/// </summary>
		/// <param name="strMessage"></param>
		/// <returns></returns>
		public bool ShowMessage(string strMessage, string strTitle = "CONFIRMATION MESSAGE", EN_STYLE style = EN_STYLE.OkCancel)
        {
			// 2024.12.02. by shkim. [MOD] TrasnslateWord된 Text를 기반으로 사이즈 변경했어야 한다.
			ChangeStyle(style);

			m_TitleBar.Text		= m_InstanceOfLanguage.TranslateWord(strTitle);
			strMessage = m_InstanceOfLanguage.TranslateWord(strMessage);

			AdjustFormSize(strMessage);

			m_labelMessage.Text = strMessage;
			// 2024.12.02. by shkim. [END]

			this.CenterToScreen();

            if(!this.Modal)
                this.ShowDialog();

            if (this.DialogResult == System.Windows.Forms.DialogResult.OK)
            {
                return true;
            }

            return false;
        }
		public bool ShowMessage(string message, EN_STYLE style, string title = "CONFIRMATION MESSAGE")
		{
			return ShowMessage(message, title, style);
		}
		private delegate bool DelegateShowWarningMessage(string strMessage, EN_STYLE style, string strTitle = "WARNING MESSAGE", int indexOfBuzzer = -1, bool bBuzzer = false);
		public bool ShowWarningMessage(string strMessage, EN_STYLE style = EN_STYLE.OkCancel, string strTitle = "WARNING MESSAGE", int indexOfBuzzer = -1, bool bBuzzer = false)
        {
            try
            {
                if (this.InvokeRequired)
                {
                    DelegateShowWarningMessage d = new DelegateShowWarningMessage(ShowWarningMessage);
                    IAsyncResult result = this.BeginInvoke(d, new object[] { strMessage, strTitle });
                    return d.EndInvoke(result);
                }
                else
                {
					if (bBuzzer && indexOfBuzzer >= 0)
					{
						DigitalIO_.DigitalIO.GetInstance().WriteOutput(indexOfBuzzer, bBuzzer);
					}

                    m_labelMessage.Text = strMessage;
                    m_labelMessage.BackGroundColor = Color.OrangeRed;
                    m_labelMessage.MainFontColor = Color.White;

                    m_TitleBar.Text = m_InstanceOfLanguage.TranslateWord(strTitle);
                    //m_btnOK.Text = m_InstanceOfLanguage.TranslateWord(OK_BUTTON_TEXT);
                    //m_btnCancel.Text = m_InstanceOfLanguage.TranslateWord(CANCEL_BUTTON_TEXT);

					ChangeStyle(style);
					AdjustFormSize(strMessage);

					CenterToScreen();
                    TopMost = true;
                    if (false == IsFormOpening())
                    {
                        ShowDialog();
                    }

                    if (bBuzzer && indexOfBuzzer >= 0)
                        DigitalIO_.DigitalIO.GetInstance().WriteOutput(indexOfBuzzer, false);

                    m_labelMessage.BackGroundColor = Color.WhiteSmoke;
                    m_labelMessage.MainFontColor = Color.Black;

                    if (DialogResult == System.Windows.Forms.DialogResult.OK)
                    {
                        return true;
                    }

                    return false;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return false;
            }
        }
        private bool IsFormOpening()
        {
            FormCollection fc = Application.OpenForms;
            bool bIsFormOpened = false;
            foreach (Form frm in fc)
            {
                //iterate through
                if (frm.Name == "Form_MessageBox")
                {
                    bIsFormOpened = true;
                    break;
                }
            }

            return bIsFormOpened;
        }
        #endregion

        #region UI 이벤트 처리
        /// <summary>
        /// 2018.06.23 by yjlee [ADD] OK 또는 CANCEL 버튼을 눌렀을 경우 처리
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Click_OkorCancel(object sender, EventArgs e)
        {
            Control ctr = sender as Control;

            switch (ctr.TabIndex)
            {
                case 0: // OK
					ProcessingEvent(Keys.Enter);
                    break;
                case 1: // CANCEL
					ProcessingEvent(Keys.Escape);
                    break;
            }
        }
		/// <summary>
		/// 2021.07.28 by twkang [ADD] TitleBar Mouse Down 이벤트
		/// </summary>
		private void MouseDown_Title(object sender, MouseEventArgs e)
		{
			m_bMouseDownTitle = true;
			m_pMouseDownCoordinate = e.Location;
		}
		/// <summary>
		/// 2021.07.28 by twkang [ADD] TitleBar Mouse Move 이벤트
		/// </summary>
		private void MouseMove_Title(object sender, MouseEventArgs e)
		{
			if (m_bMouseDownTitle)
			{
				this.SetDesktopLocation(MousePosition.X - m_pMouseDownCoordinate.X, MousePosition.Y - m_pMouseDownCoordinate.Y);
			}
		}
		/// <summary>
		/// 2021.07.28 by twkang [ADD] TitleBar Mouse Up 이벤트
		/// </summary>
		private void MouseUp_Title(object sender, MouseEventArgs e)
		{
			m_bMouseDownTitle = false;
		}

		/// <summary>
		/// 2021.08.24 by twkang [ADD] 키입력 이벤트
		/// </summary>
		private void KeyDown_Event(object sender, KeyEventArgs e)
		{
			ProcessingEvent(e.KeyCode);
		}
        #endregion

		private void Form_MessageBox_KeyDown(object sender, KeyEventArgs e)
		{
			int nKeyCode = (int)e.KeyCode;

			switch (e.KeyCode)
			{
				case Keys.Escape: // Esc 입력 시
				case Keys.Back:	// 백스페이스 입력 시
					ProcessingEvent(Keys.Escape);
					break;
				case Keys.Enter: // 엔터 입력 시
					ProcessingEvent(Keys.Enter);
					break;
				default:
					break;
			}
		}

		public enum EN_STYLE
		{
			OkCancel,
			OkOnly,
			YesNo,
		}
    }
}
