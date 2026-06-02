using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using DesignPattern_.Observer_;

namespace FrameOfSystem3.Views
{
    public partial class MainMenu : UserControl , IObserver
    {
        public delegate bool callbackFunctionForEvent(int tabIndex);
        public event callbackFunctionForEvent evtButtonClick;

        private Sys3Controls.Sys3button m_btnPreviousClicked;

        public MainMenu()
        {
            InitializeComponent();

            m_btnPreviousClicked = EN_OPERATION;
        }

        #region Internal Interface

        #region GUI
        private void ClickMainMenu(object sender, EventArgs e)
        {
            Sys3Controls.Sys3button btn = (Sys3Controls.Sys3button)sender;

            if(btn.Equals(m_btnPreviousClicked)) { return; }

			// 2024.06.13 by junho [MOD] Text번역을 위해 tab index로 변경
			//if (false == evtButtonClick(btn.Text))
			if (false == evtButtonClick(btn.TabIndex))
			{
				return;
			}

            m_btnPreviousClicked.ButtonClicked = false;

            m_btnPreviousClicked = btn;

            m_btnPreviousClicked.ButtonClicked = true;
			
        }

		/// <summary>
		/// 2021.06.03 by twkang [ADD] 현재계정 메뉴설정
		/// </summary>
		public void SetMenuButton()
		{
			var enAuthority	= Account_.Account.GetInstance().GetLoginedAuthority();
			
			switch(enAuthority)
			{
				case Account_.USER_AUTHORITY.ENGINEER:
					EN_SETUP.Visible		= true;
					EN_CONFIG.Visible		= false;
					break;
				case Account_.USER_AUTHORITY.OPERATOR:
					EN_CONFIG.Visible		= false;
					EN_SETUP.Visible		= false;
					break;
				case Account_.USER_AUTHORITY.MASTER:
					EN_CONFIG.Visible		= true;
					EN_SETUP.Visible		= true;
					break;
				default:
					break;
			}

			/// 계정설정 이후 Operation 화면으로 전환
			ClickMainMenu(EN_OPERATION, new EventArgs());
		}
        #endregion

		#region Observer Interface
		Account_.Account m_subjectAccount			= null;

		/// <summary>
		/// 2020.06.01 by twkang [ADD] 감시자 주체를 등록한다.
		/// </summary>
		public void RegisterSubject(Subject pSubject)
		{
			if (pSubject is Account_.Account)
			{
				m_subjectAccount = pSubject as Account_.Account;

				if (this.InvokeRequired)
				{
					this.Invoke(new MethodInvoker(SetMenuButton));
				}
				else
					SetMenuButton();
			}
			
			pSubject.Attach(this);
		} 
		/// <summary>
		/// 2020.06.01 by twkang [ADD] 감시자를 업데이트 한다.
		/// </summary>
		public void UpdateObserver(Subject pSubject)
		{
			if (pSubject is Account_.Account)
			{
				if(this.InvokeRequired)
				{
					this.Invoke(new MethodInvoker(SetMenuButton));
				}
				else
					SetMenuButton();
			}
		}
		#endregion

		#endregion
	}
}
