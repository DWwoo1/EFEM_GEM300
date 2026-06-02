using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using System.Runtime.InteropServices;
using System.Diagnostics;

namespace FrameOfSystem3.Views.Functional
{
	public partial class Form_Keyboard : Form
	{
		#region Keyborad Event
		[DllImport("user32.dll")] // Key 입력 이벤트
		static extern void keybd_event(byte bVk, byte bScan, uint dwFlags, UIntPtr dwExtraInfo);
		#endregion

		#region 싱글톤
		private Form_Keyboard()
		{
			this.DoubleBuffered = true;

			InitializeComponent();

			// 기능키 Tabindex 설정
			#region Tabindex 
			m_btnCancel.TabIndex		= (int)Keys.Escape;
			m_btnCapsLock.TabIndex		= (int)Keys.CapsLock;
			m_btnBackspace.TabIndex		= (int)Keys.Back;
			m_btnSpace.TabIndex			= (int)Keys.Space;
			m_btnShiftLeft.TabIndex		= (int)Keys.ShiftKey;
			m_btnShiftRight.TabIndex	= (int)Keys.ShiftKey;
			m_btnIME.TabIndex			= (int)Keys.HangulMode;
			m_btnEnter.TabIndex			= (int)Keys.Enter;
			#endregion

			#region Mapping
			m_dicOfButton.Add(Keys.D0, m_btn0);
			m_dicOfButton.Add(Keys.D7, m_btn7);
			m_dicOfButton.Add(Keys.D8, m_btn8);
			m_dicOfButton.Add(Keys.D9, m_btn9);
			m_dicOfButton.Add(Keys.D5, m_btn5);
			m_dicOfButton.Add(Keys.D6, m_btn6);
			m_dicOfButton.Add(Keys.D3, m_btn3);
			m_dicOfButton.Add(Keys.D4, m_btn4);
			m_dicOfButton.Add(Keys.D1, m_btn1);
			m_dicOfButton.Add(Keys.D2, m_btn2);
			m_dicOfButton.Add(Keys.A, m_btnA);
			m_dicOfButton.Add(Keys.OemMinus, m_btnMinus);
			m_dicOfButton.Add(Keys.Oemplus, m_btnequal);
			m_dicOfButton.Add(Keys.Oemtilde, m_btnTilde);
			m_dicOfButton.Add(Keys.Q, m_btnQ);
			m_dicOfButton.Add(Keys.W, m_btnW);
			m_dicOfButton.Add(Keys.E, m_btnE);
			m_dicOfButton.Add(Keys.R, m_btnR);
			m_dicOfButton.Add(Keys.T, m_btnT);
			m_dicOfButton.Add(Keys.Y, m_btnY);
			m_dicOfButton.Add(Keys.U, m_btnU);
			m_dicOfButton.Add(Keys.I, m_btnI);
			m_dicOfButton.Add(Keys.O, m_btnO);
			m_dicOfButton.Add(Keys.P, m_btnP);
			m_dicOfButton.Add(Keys.OemOpenBrackets, m_btnOpenBrackets);
			m_dicOfButton.Add(Keys.OemCloseBrackets, m_btnCloseBrackets);
			m_dicOfButton.Add(Keys.OemPipe, m_btnPipe);
			m_dicOfButton.Add(Keys.S, m_btnS);
			m_dicOfButton.Add(Keys.D, m_btnD);
			m_dicOfButton.Add(Keys.F, m_btnF);
			m_dicOfButton.Add(Keys.G, m_btnG);
			m_dicOfButton.Add(Keys.H, m_btnH);
			m_dicOfButton.Add(Keys.J, m_btnJ);
			m_dicOfButton.Add(Keys.K, m_btnK);
			m_dicOfButton.Add(Keys.L, m_btnL);
			m_dicOfButton.Add(Keys.Z, m_btnZ);
			m_dicOfButton.Add(Keys.X, m_btnX);
			m_dicOfButton.Add(Keys.C, m_btnC);
			m_dicOfButton.Add(Keys.V, m_btnV);
			m_dicOfButton.Add(Keys.B, m_btnB);
			m_dicOfButton.Add(Keys.N, m_btnN);
			m_dicOfButton.Add(Keys.M, m_btnM);
			m_dicOfButton.Add(Keys.Back, m_btnBackspace);
			m_dicOfButton.Add(Keys.OemQuotes, m_btnQuotes);
			m_dicOfButton.Add(Keys.OemSemicolon, m_btnSemicolon);
			m_dicOfButton.Add(Keys.Oemcomma, m_btnComma);
			m_dicOfButton.Add(Keys.OemPeriod, m_btnDot);
			m_dicOfButton.Add(Keys.OemQuestion, m_btnQuestion);
			m_dicOfButton.Add(Keys.Space, m_btnSpace);
			m_dicOfButton.Add(Keys.HanguelMode, m_btnIME);
			#endregion

			// 2025.04.23 by junho [ADD] UI언어가 한글도 영어도 아니라면 keyboard를 영문 고정하도록 변경.
			switch(Language_.Language.GetInstance().GetLangauge())
			{
				case Language_.TYPE_LANGUAGE.ENGLISH:
				case Language_.TYPE_LANGUAGE.KOREAN:
					break;
				default:
					{
						ENGLISH_ONLY = true;

						for (int i = (int)Keys.A; i <= (int)Keys.Z; ++i)
						{
							if (false == Enum.IsDefined(typeof(Keys), i))
								continue;

							Keys key = (Keys)i;
							m_dicOfButton[key].Text = m_dicOfButton[key].SubText;
							m_dicOfButton[key].UseSubFont = false;
							m_dicOfButton[key].TextAlignMain = Sys3Controls.EN_TEXTALIGN.MIDDLE_CENTER;
						}

						m_btnIME.Visible = false;
						m_btnIME.Enabled = false;
					}
					break;
			}
		}

		private static Form_Keyboard _Instance = new Form_Keyboard();
		public static Form_Keyboard GetInstance()
		{
			return _Instance;
		}
		#endregion

		#region 열거형

		#endregion

		#region 상수
		private readonly char PASSWORD_CHAR_NOT_USE		= '\0';
		private readonly char PASSWORD_CHAR				= '*';

		readonly bool ENGLISH_ONLY = false;
		#endregion

		#region 변수

		#region ButtonMapping
		Dictionary<Keys, Sys3Controls.Sys3button> m_dicOfButton	= new Dictionary<Keys, Sys3Controls.Sys3button>();
		#endregion

		#region GUI
		private bool m_bMouseDownTitle					= false;
		private Point m_pMouseDownCoordinate			= new Point();
		#endregion

		private bool m_bCapsLock		= false;
		private bool m_bShift			= false;
		private bool m_bPassWordMode	= false;

		private string m_strResult		= ""; // 입력한 결과값을 반환한다.
		#endregion

		#region 외부 인터페이스
		/// <summary>
		/// 2025.06.13 by junho [ADD] Previous value 추가
		/// public bool CreateForm(string strPreValue = "", int nMaxLength = 200, bool bHideMode = false, string strTitle = "")
		/// 2018.06.12 by yjlee [ADD] 키보드 폼을 생성한다.
		/// </summary>
		public bool CreateForm(string strPresentValue = "", int nMaxLength = 200, bool bHideMode = false, string strTitle = "", string strUnit = "", string strPrevious = "", string strPreviousTime = "")
		{
			// 2024.01.12 by junho [ADD] 타이틀 추가
			m_TitleBar.Text = strTitle;

			// 키보드 값 초기화
			m_labelPresentValue.Text			= strPresentValue;
			m_TextBox.MaxLength				= nMaxLength;
			m_strResult						= string.Empty;
			m_bPassWordMode					= bHideMode;

			if(false == string.IsNullOrWhiteSpace(strUnit))
			{
				m_labelPresentValue.UseUnitFont = true;
				m_labelPresentValue.UnitText = strUnit;
			}
			else
			{
				m_labelPresentValue.UseUnitFont = false;
			}

			DateTime tPreviousTime;
			if(DateTime.TryParse(strPreviousTime, out tPreviousTime))
			{
				m_labelPreviousValue.Visible = true;
				btn_CopyPrevious.Visible = true;
				m_labelPreviousValue.Text = strPrevious;
				m_labelPreviousValue.UnitText = tPreviousTime.ToString("yyyy-MM-dd HH:mm:ss");
			}
			else
			{
				m_labelPreviousValue.Visible = false;
				btn_CopyPrevious.Visible = false;
			}

			// 2021.05.31 by twkang [ADD] 비밀번호 입력 등 입력문자를 가리기 위한 설정
			SetViewModeOfTextBox(bHideMode);

			InitializeForm();

			this.ActiveControl	= m_TextBox;
			
			this.ShowDialog();

			if (this.DialogResult == System.Windows.Forms.DialogResult.OK)
			{
				return true;
			}

			return false;
		}
		/// <summary>
		/// 2020.06.29 by twkang [ADD] 결과값을 반환한다.
		/// </summary>
		public void GetResult(ref string strResult)
		{
			strResult = m_strResult;
		}
		#endregion

		#region 내부 인터페이스

		#region UI
		/// <summary>
		/// 2020.05.08 by twkang [ADD] 쉬프트 키에 따라 UI상태를 바꾼다.
		/// </summary>
		private void ChangeKeyTextByShift()
		{
			if (ENGLISH_ONLY)
				return;

			m_btnQ.Text = m_bShift ? "ㅃ" : "ㅂ";
			m_btnW.Text = m_bShift ? "ㅉ" : "ㅈ";
			m_btnE.Text = m_bShift ? "ㄸ" : "ㄷ";
			m_btnR.Text = m_bShift ? "ㄲ" : "ㄱ";
			m_btnT.Text = m_bShift ? "ㅆ" : "ㅅ";
			m_btnO.Text = m_bShift ? "ㅒ" : "ㅐ";
			m_btnP.Text = m_bShift ? "ㅖ" : "ㅔ";
		}
		/// <summary>
		/// 2020.10.22 by twkang [ADD] 버튼의 클릭상태를 초기한다.
		/// </summary>
		private void InitializeForm()
		{
			m_TextBox.Text = string.Empty;

			foreach (var kvp in m_dicOfButton)
			{
				kvp.Value.ButtonClicked = false;
			}

			m_bCapsLock		= Control.IsKeyLocked(Keys.CapsLock);

			m_btnCapsLock.ButtonClicked = m_bCapsLock;
			m_btnIME.ButtonClicked		= m_TextBox.ImeMode == System.Windows.Forms.ImeMode.Hangul;
			m_btnShowText.Visible		= m_bPassWordMode;
		}
		/// <summary>
		/// 2021.05.31 by twkang [ADD] 텍스트 박스의 보기 형식을 설정한다. (Password)
		/// </summary>
		private void SetViewModeOfTextBox(bool bHidePassword)
		{
			m_TextBox.PasswordChar			= bHidePassword ? PASSWORD_CHAR : PASSWORD_CHAR_NOT_USE;
			m_TextBox.UseSystemPasswordChar	= bHidePassword;
		}
		#endregion

		#region 버튼이벤트 처리
		/// <summary>
		/// 2021.05.26 by twkang [ADD] Key 입력 처리
		/// </summary>
		private void ActionButtonEvent(int enKeyCode)
		{
			Keys enClickedKeys	= (Keys)enKeyCode;

			switch(enClickedKeys)
			{
				case Keys.Escape:
					this.DialogResult	= System.Windows.Forms.DialogResult.Cancel;

					this.Close();
					return;

				case Keys.Enter:
					m_strResult			= m_TextBox.Text;
					this.DialogResult	= System.Windows.Forms.DialogResult.OK;

					this.Close();
					return;

				case Keys.ShiftKey:
					m_bShift		= m_bShift ? false : true;

					m_btnShiftLeft.ButtonClicked	= m_bShift;
					m_btnShiftRight.ButtonClicked	= m_bShift;

					ChangeKeyTextByShift();
					break;

				case Keys.CapsLock:
					m_bCapsLock		= Control.IsKeyLocked(Keys.CapsLock);
					
					m_btnCapsLock.ButtonClicked		= m_bCapsLock;
					break;

				case Keys.HanguelMode:
					switch (m_TextBox.ImeMode)
					{
						case System.Windows.Forms.ImeMode.Hangul:
							m_TextBox.ImeMode = System.Windows.Forms.ImeMode.Disable;
							break;
						default:
							m_TextBox.ImeMode = System.Windows.Forms.ImeMode.Hangul;
							break;
					}
					break;
			}
		}
		#endregion

		#endregion

		#region UI 이벤트
		/// <summary>
		/// 2021.05.27 by twkang [ADD] Enter, ESC, CapsLock, Shift 입력 이벤트
		/// </summary>
		private void Click_ETCkey(object sender, EventArgs e)
		{
			Control ctrl	= sender as Control;

			switch (ctrl.TabIndex)
			{
				case (int)Keys.CapsLock:
					// Capslock 버튼 입력 이벤트
					keybd_event(0x14, 0x45, 0x1, (UIntPtr)0);
					keybd_event(0x14, 0x45, 0x1 | 0x2, (UIntPtr)0);
					break;
				default:
					break;
			}

			// Tabindex = KeyCode
			ActionButtonEvent(ctrl.TabIndex);
		}
		/// <summary>
		/// 2021.05.27 by twKang [ADD] 스페이스, 백스페이스 버튼 클릭 이벤트
		/// </summary>
		private void Click_SpaceOrBackSpace(object sender, EventArgs e)
		{
			Control ctr = sender as Control;

			string strSendMessage	= string.Empty;

			switch(ctr.TabIndex)
			{
				case (int)Keys.Back:
					strSendMessage	= "{BACKSPACE}";
					break;
				case (int)Keys.Space:
					strSendMessage	= " ";
					break;
			}

			SendKeys.SendWait(strSendMessage);
		}
		/// <summary>
		/// 2018,06.18 by yjlee [ADD] 문자 입력 이벤트
		/// 2021.05.27 by twkang [MOD]
		/// </summary>
		private void Click_Characters(object sender, EventArgs e)
		{
			Sys3Controls.Sys3button ctrl	= sender as Sys3Controls.Sys3button;

			string strEnterKey		= m_bShift || m_bCapsLock ? ctrl.SubText.ToUpper() : ctrl.SubText.ToLower();

			m_TextBox.Text			+= strEnterKey;

			m_TextBox.SelectionStart = m_TextBox.Text.Length;
		}
		/// <summary>
		/// 2018.06.18 by yjlee [ADD] 숫자 또는 특수문자 이벤트
		/// 2021.05.27 by twkang [MOD]
		/// </summary>
		private void Click_DualKeyByShift(object sender, EventArgs e)
		{
			Sys3Controls.Sys3button ctrl	= sender as Sys3Controls.Sys3button;

			string strEnterKey	= m_bShift ? ctrl.SubText : ctrl.Text;

			m_TextBox.Text		+= strEnterKey;
			
			m_TextBox.SelectionStart = m_TextBox.Text.Length;
		}
		/// <summary>
		/// 2018.06.18 by yjlee [ADD] 키보드에서 직접 키를 입력했을 경우의 처리
		/// 2021.05.27 by twkang [MOD]
		/// </summary>
		private void Form_Keyboard_KeyDown(object sender, KeyEventArgs e)
		{
			int nKeyCode = (int)e.KeyCode;

			if (m_dicOfButton.ContainsKey(e.KeyCode))
			{
				m_dicOfButton[e.KeyCode].ButtonClicked	= true;
			}

			switch(e.KeyCode)
			{
				case Keys.ShiftKey:
					if (m_bShift)
						return;
					else
						break;
			}

			ActionButtonEvent((int)e.KeyCode);
		}
		/// <summary>
		/// 2018.06.18 by yjlee [ADD] 키보드에서 직접 키를 떼었을 경우 처리
		/// 2021.05.27 by twkang [MOD]
		/// </summary>
		private void Form_Keyboard_KeyUp(object sender, KeyEventArgs e)
		{
			if(m_dicOfButton.ContainsKey(e.KeyCode))
			{
				m_dicOfButton[e.KeyCode].ButtonClicked	= false;
			}

			if (e.KeyCode == Keys.ShiftKey && m_bShift)
			{
				ActionButtonEvent((int)e.KeyCode);
			}
		}
		/// <summary>
		/// 2021.05.31 by twkang [ADD] 마우스 Down 이벤트
		/// </summary>
		private void MouseDown_ShowButton(object sender, MouseEventArgs e)
		{
			SetViewModeOfTextBox(false);
		}

		/// <summary>
		///  2021.05.31 by twkang [ADD] 마우스 Up 이벤트
		/// </summary>
		private void MouseUP_ShowButton(object sender, MouseEventArgs e)
		{
			SetViewModeOfTextBox(true);
		}
		/// <summary>
		/// 2021.05.27 by twkang [ADD] Copy 버튼 클릭 이벤트
		/// </summary>
		private void Click_Copy(object sender, EventArgs e)
		{
			string value = string.Empty;

			if (sender == btn_CopyPresent)
				value = m_labelPresentValue.Text;
			else if (sender == btn_CopyPrevious)
				value = m_labelPreviousValue.Text;
			else return;

			if (string.Empty == value)
				return;

			Clipboard.SetText(value);
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
		#endregion
	}
}
