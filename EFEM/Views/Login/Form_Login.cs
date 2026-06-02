using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using FrameOfSystem3.Views.Functional;

namespace FrameOfSystem3.Views.Login
{
    public partial class Form_Login : Form
	{
		#region 열거형
		public enum EN_USER_AUTHORITY
		{
			OPERATOR	= 0,
			ENGINEER	= 1,
			MASTER		= 2,
		}
		#endregion

		#region 상수
		private const int HEIGHT_OF_ROWS			= 30;

		private const int COLUMN_INDEX_OF_INDEX		= 0;
		private const int COLUMN_INDEX_OF_ID		= 1;
		private const int COLUMN_INDEX_OF_PASSWORD	= 2;
		private const int COLUMN_INDEX_OF_AUTHORITY	= 3;
        private const int COLUMN_INDEX_OF_LOGOUTTIME = 4;

		private readonly string LOGIN_ID_LABEL			= "Enter Your ID";
		private readonly string LOGIN_PASSWORD_LABEL	= "Enter Your Password";
		private readonly string USER_ID_LABE			= "Enter User ID";
		private readonly string USER_PASSWORD_LABEL		= "Enter User Password";
		private readonly string USER_LOGOUTTIME_LABEL	= "Enter Logout Time";
		private readonly string USER_AUTHORITY_LABEL	= "Enter User Authority";
        #endregion

        #region 변수
		private int m_nSelectedItem								= -1;
		private int m_nSelectedRow								= -1;

		private string m_strInputPW								= string.Empty;

		Dictionary<int, string> m_DicOfUserAuthority			= new Dictionary<int,string>();

        private Form_Keyboard m_InstanceKeyboard                = Form_Keyboard.GetInstance();  // 키보드 사용을 위한 변수
		Account.CAccount m_InstanceOfAccount					= null;
		Form_SelectionList m_instanceSelection					= null;
		Form_MessageBox m_instanceOfMessageBox					= null;

        #region Iris Recognition
        //Form_IrisRecognition m_formIrisRecognition              = null;
        #endregion End - Iris Recognition

        #endregion

        public Form_Login()
        {
            DoubleBuffered			= true;

            InitializeComponent();

			m_InstanceOfAccount = Account.CAccount.GetInstance();
			m_instanceSelection = Form_SelectionList.GetInstance();
			m_instanceOfMessageBox = Form_MessageBox.GetInstance();

			#region Mapping
			m_DicOfUserAuthority.Clear();
			foreach(FrameOfSystem3.Account.CAccount.EN_PARAM_USER_AUTHORITY en in Enum.GetValues(typeof(FrameOfSystem3.Account.CAccount.EN_PARAM_USER_AUTHORITY)))
			{
				m_DicOfUserAuthority.Add((int)en, en.ToString());
			}
			#endregion
		}

        #region 외부 인터페이스
        /// <summary>
        /// 2018.06.21 by yjlee [ADD] 로그인 폼을 생성한다.
        /// </summary>
        public void CreateForm()
        {
            // 값 초기화 작업
            m_labelPW.Text		= string.Empty;
			m_strInputPW		= string.Empty;

			m_labelID.Text		= m_InstanceOfAccount.GetLoginedID();

			SetLoginState(!m_labelID.Text.Equals(Account.CAccount.EN_PARAM_USER_AUTHORITY.OPERATOR.ToString()));

			CheckMasterMode();

			ShowLogInDisplay();

            this.Show();
        }
        #endregion

        #region 내부 인터페이스
		/// <summary>
		/// 2020.08.25 by twkang [ADD] 폼 및 컨트롤들을 로그인 상태에 따라 설정한다.
		/// </summary>
		private void SetLoginState(bool bLogined)
		{
			m_labelPW.Enabled		            = !bLogined;
			m_btnPopupList.Enabled	            = !bLogined;
			m_btnLogin.Enabled		            = !bLogined;
            m_btnLoginIrisRecognition.Enabled   = !bLogined;
			m_btnLogout.Enabled		            = bLogined;
		}
		/// <summary>
        /// 2018.06.23 by yjlee [ADD] 마스터 모드 로그인 시 확장 기능을 제공한다.
        /// </summary>
        private void CheckMasterMode()
        {
            // 로그인 된 계정이 마스터 권한일 경우
            if (m_InstanceOfAccount.GetLoginedAuthority() == Account.CAccount.EN_PARAM_USER_AUTHORITY.MASTER)
            {
				m_btnUseManagement.Show();
            }
            else
            {
				m_btnUseManagement.Hide();
            }
        }
		/// <summary>
		/// 2020.08.25 by twkang [ADD] 아이템을 GridView 에 추가한다.
		/// </summary>
		private void AddLoginItemToList(int nIndexOfItem)
		{
			int nIndexOfRow		= m_dgViewUserList.Rows.Count;

			m_dgViewUserList.Rows.Add();

			string strValue		= string.Empty;
			int nValue			= -1;

			m_dgViewUserList.Rows[nIndexOfRow].Height	 = HEIGHT_OF_ROWS;
			m_dgViewUserList[COLUMN_INDEX_OF_INDEX, nIndexOfRow].Value = nIndexOfItem.ToString();
			m_dgViewUserList.Rows[nIndexOfRow].Cells[COLUMN_INDEX_OF_INDEX].Style.BackColor = Color.Silver;
			m_dgViewUserList.Rows[nIndexOfRow].Cells[COLUMN_INDEX_OF_INDEX].Style.SelectionBackColor = Color.Silver;
			m_dgViewUserList.Rows[nIndexOfRow].Cells[COLUMN_INDEX_OF_INDEX].Style.SelectionForeColor = Color.Black;

			if(false == m_InstanceOfAccount.GetParameter(nIndexOfItem, Account.CAccount.EN_PARAM_ACCOUNT.ID, ref strValue))
			{
				strValue	= string.Empty;
			}
			m_dgViewUserList[COLUMN_INDEX_OF_ID, nIndexOfRow].Value			= strValue;

			if (false == m_InstanceOfAccount.GetParameter(nIndexOfItem, Account.CAccount.EN_PARAM_ACCOUNT.PASSWORD, ref strValue))
			{
				strValue	= string.Empty;
			}
			m_dgViewUserList[COLUMN_INDEX_OF_PASSWORD, nIndexOfRow].Value	= strValue;

			if (m_InstanceOfAccount.GetParameter(nIndexOfItem, Account.CAccount.EN_PARAM_ACCOUNT.AUTHORITY, ref nValue)
				|| m_DicOfUserAuthority.ContainsKey(nValue))
			{
				m_dgViewUserList[COLUMN_INDEX_OF_AUTHORITY, nIndexOfRow].Value = m_DicOfUserAuthority[nValue];
			}

            if (false == m_InstanceOfAccount.GetParameter(nIndexOfItem, Account.CAccount.EN_PARAM_ACCOUNT.LOGOUT_TIME, ref strValue))
            {
                strValue = string.Empty;
            }
            m_dgViewUserList[COLUMN_INDEX_OF_LOGOUTTIME, nIndexOfRow].Value = strValue;
			m_dgViewUserList.Rows[nIndexOfRow].Selected		= false;
		}
		/// <summary>
		/// 2020.08.26 by twkang [ADD] 선택한 아이템을 지운다.
		/// </summary>
		private bool RemoveItemOnGrid()
		{
			if(m_nSelectedRow < 0)
			{
				return false;
			}

			if(m_InstanceOfAccount.RemoveItem(m_nSelectedItem))
			{
				m_dgViewUserList.Rows.RemoveAt(m_nSelectedRow);
				return true;
			}
			return false;
		}
		/// <summary>
		/// 2020.08.25 by twkang [ADD] GridView 를 업데이트 한다.
		/// </summary>
		private void UpdateUserList()
		{
			m_dgViewUserList.Rows.Clear();

			int[] arIndex	= null;
			if(false == m_InstanceOfAccount.GetListOfUserList(ref arIndex))
			{
				return;
			}

			for(int nIndex = 0, nEnd = arIndex.Length; nIndex < nEnd; ++nIndex)
			{
				AddLoginItemToList(arIndex[nIndex]);
			}
		}
		/// <summary>
		/// 2020.08.26 by twkang [ADD] 아이템 선택 여부를 초기화한다.
		/// </summary>
		private void ResetSelectedItem()
		{
			m_nSelectedItem				= -1;
			m_nSelectedRow				= -1;

			m_labelLoginID.Text				= USER_ID_LABE;
			m_labelLoginID.MainFontColor	= Color.DimGray;
			m_labelLoginPW.Text				= USER_PASSWORD_LABEL;
			m_labelLoginPW.MainFontColor	= Color.DimGray;
			m_labelAuthority.Text			= USER_AUTHORITY_LABEL;
			m_labelAuthority.MainFontColor	= Color.DimGray;

            m_labelLogoutTime.Text          = USER_LOGOUTTIME_LABEL;
            m_labelLogoutTime.MainFontColor = Color.DimGray;
			SetActiveControls(false);
		}
		/// <summary>
		/// 2020.08.26 by twkang [ADD] 컨트롤들의 활성화 여부를 설정한다.
		/// </summary>
		private void SetActiveControls(bool bEnable)
		{
			m_labelLoginID.Enabled		    = bEnable;
			m_labelLoginPW.Enabled		    = bEnable;
			m_labelAuthority.Enabled	    = bEnable;
            m_labelLogoutTime.Enabled       = bEnable;
            m_labelIrisCodeLeft.Enabled     = bEnable;
            m_labelIrisCodeRight.Enabled    = bEnable;

			m_btnAuthority.Enabled		    = bEnable;
			m_btnModify.Enabled			    = bEnable;
			m_btnRemove.Enabled			    = bEnable;
		}
		/// <summary>
		/// 2020.08.26 by twkang [ADD] 유저 정보를 수정한다.
		/// </summary>
		private void ModifyUserInformation()
		{
			if (m_nSelectedRow < 0)
			{
				return;
			}
			if(m_InstanceOfAccount.SetParameter(m_nSelectedItem, Account.CAccount.EN_PARAM_ACCOUNT.ID, m_labelLoginID.Text))
			{
				m_dgViewUserList[COLUMN_INDEX_OF_ID, m_nSelectedItem].Value				= m_labelLoginID.Text;
			}
			if(m_InstanceOfAccount.SetParameter(m_nSelectedItem, Account.CAccount.EN_PARAM_ACCOUNT.PASSWORD, m_labelLoginPW.Text))
			{
				m_dgViewUserList[COLUMN_INDEX_OF_PASSWORD, m_nSelectedItem].Value		= m_labelLoginPW.Text;
			}
			if(m_InstanceOfAccount.SetParameter(m_nSelectedItem, Account.CAccount.EN_PARAM_ACCOUNT.AUTHORITY, m_labelAuthority.Text))
			{
				m_dgViewUserList[COLUMN_INDEX_OF_AUTHORITY, m_nSelectedItem].Value		= m_labelAuthority.Text;
			}
            if(m_InstanceOfAccount.SetParameter(m_nSelectedItem, Account.CAccount.EN_PARAM_ACCOUNT.LOGOUT_TIME, m_labelLogoutTime.Text))
			{
                m_dgViewUserList[COLUMN_INDEX_OF_LOGOUTTIME, m_nSelectedItem].Value      = m_labelLogoutTime.Text;
			}
		}
		/// <summary>
		/// 2020.09.29 by twkang [ADD] Login 화면으로 전환한다.
		/// </summary>
		private void ShowLogInDisplay()
		{
			m_groupLogin.BringToFront();
			m_btnProtec.BringToFront();
			m_btnCancel.BringToFront();

			m_labelID.BringToFront();
			m_labelPW.BringToFront();
			m_btnPopupList.BringToFront();

			m_btnLogin.BringToFront();
            m_btnLoginIrisRecognition.BringToFront();
			m_btnLogout.BringToFront();
			m_btnUseManagement.BringToFront();
		}
		/// <summary>
		/// 2020.09.29 by twkang [ADD] 유저관리 화면으로 전환한다.
		/// </summary>
		private void ShowManagementDisplay()
		{
			m_btnProtec.SendToBack();
			m_btnCancel.SendToBack();

			m_labelID.SendToBack();
			m_labelPW.SendToBack();
			m_btnPopupList.SendToBack();

			m_btnLogin.SendToBack();
			m_btnLogout.SendToBack();
			m_btnUseManagement.SendToBack();

			m_groupLogin.SendToBack();
            m_btnLoginIrisRecognition.SendToBack();
		}
        #endregion

        #region UI 이벤트 처리
        /// <summary>
        /// 2020.08.26 by twkang [ADD] 패스워드 입력창을 눌렀을 때
        /// </summary>
        private void Click_Password(object sender, EventArgs e)
        {
            Control ctr = sender as Control;

            // 키보드로 부터 입력받는다.
            if(false == m_InstanceKeyboard.CreateForm(string.Empty, 15, true))
            {
                return;
            }

			m_InstanceKeyboard.GetResult(ref m_strInputPW);

			m_labelPW.Text			= string.Empty;
			for(int nIndex = 0, nEnd = m_strInputPW.Length; nIndex < nEnd; ++nIndex)
			{
				m_labelPW.Text		+= "●";
			}

			m_labelPW.MainFontColor	= Color.Black;
        }
        /// <summary>
        /// 2020.08.26 by twkang [ADD] 로그인, 취소 버튼을 눌렀을 때
        /// </summary>
        private void Click_ApplyOrCancel(object sender, EventArgs e)
        {
            Control ctr = sender as Control;

            switch(ctr.TabIndex)
            {
                case 0: // APPLY
					if(m_InstanceOfAccount.Login(m_labelID.Text, m_strInputPW))
					{
						if(m_instanceOfMessageBox.ShowMessage("The login was successful! Do you want to close this form?"))
						{
							this.Hide();
						}

						ResetSelectedItem();
						SetLoginState(true);
						CheckMasterMode();

						m_strInputPW	= string.Empty;
						m_labelPW.Text	= "";
					}
					else
					{
						m_instanceOfMessageBox.ShowMessage("Check your id and password");
					}
					break;
                case 1: // CANCEL
                    this.Hide();
                    break;
            }
        }
		/// <summary>
		/// 2018.06.21 by yjlee [ADD] LOGOUT 버튼을 눌렀을 경우
		/// </summary>
		private void Click_Logout(object sender, EventArgs e)
		{
			SetLoginState(false);

			m_InstanceOfAccount.Logout();

			m_labelID.Text			= LOGIN_ID_LABEL;
			m_labelID.MainFontColor	= Color.DimGray;
			m_labelPW.Text			= LOGIN_PASSWORD_LABEL;
			m_labelPW.MainFontColor	= Color.DimGray;

			// 마스터모드인지 확인한다.
			CheckMasterMode();
		}
		/// <summary>
		/// 2020.08.26 by twkang [ADD] 화면의 드롭리스트 버튼을 눌렀을 때
		/// </summary>
		private void Click_DownList(object sender, EventArgs e)
		{
			Control ctr			= sender as Control;
			string strValue		= string.Empty;
			switch(ctr.TabIndex)
			{
				case 0: // USER LIST
					string[] arUserList	= null;
					if (false == m_InstanceOfAccount.GetListOfUserList(ref arUserList))
					{
						return;
					}
					if(m_instanceSelection.CreateForm("USER LIST", arUserList, m_labelID.Text))
					{
						m_instanceSelection.GetResult(ref strValue);
						m_labelID.Text		= strValue;
						m_labelID.MainFontColor	= Color.Black;
					}
					break;
				case 1: // AUTHORITY
					if(m_instanceSelection.CreateForm("AUTHORITY", Define.DefineEnumProject.SelectionList.EN_SELECTIONLIST.USER_AUTHORITY, m_labelAuthority.Text))
					{
						m_instanceSelection.GetResult(ref strValue);
						m_labelAuthority.Text		= strValue;
					}
					break;
			}

		}
		/// <summary>
		/// 2020.08.25 by twkang [ADD] 유저 목록을 클릭 했을 경우
		/// </summary>
		private void m_dgViewUserList_CellClick(object sender, DataGridViewCellEventArgs e)
		{
			int nRowIndex		= e.RowIndex;

			if (nRowIndex < 0 || nRowIndex >= m_dgViewUserList.RowCount)
			{
				return;
			}

			m_nSelectedItem			= int.Parse(m_dgViewUserList[COLUMN_INDEX_OF_INDEX, nRowIndex].Value.ToString());
			m_nSelectedRow			= nRowIndex;

			SetActiveControls(true);

			m_labelLoginID.Text     = m_dgViewUserList[COLUMN_INDEX_OF_ID, nRowIndex].Value.ToString();
			m_labelLoginID.MainFontColor	= Color.Black;
            m_labelLoginPW.Text     = m_dgViewUserList[COLUMN_INDEX_OF_PASSWORD, nRowIndex].Value.ToString();
			m_labelLoginPW.MainFontColor	= Color.Black;
            m_labelAuthority.Text   = m_dgViewUserList[COLUMN_INDEX_OF_AUTHORITY, nRowIndex].Value.ToString();
			m_labelAuthority.MainFontColor	= Color.Black;
            m_labelLogoutTime.Text = m_dgViewUserList[COLUMN_INDEX_OF_LOGOUTTIME, nRowIndex].Value.ToString();
            m_labelLogoutTime.MainFontColor = Color.Black;
		}
		/// <summary>
		/// 2020.08.25 by twkang [ADD] 유저 관리창 버튼 클릭했을 경우
		/// </summary>
		private void Click_ManagementButtons(object sender, EventArgs e)
		{
			Control ctrl	= sender as Control;
			
			switch(ctrl.TabIndex)
			{
				case 0: // ADD
					int nIndex	= m_InstanceOfAccount.AddAccountItem();
					AddLoginItemToList(nIndex);
					ResetSelectedItem();
					break;
				case 1: // MODIFY
					ModifyUserInformation();
					break;
				case 2: // REMOVE
					if(RemoveItemOnGrid())
					{
						ResetSelectedItem();
					}
					break;
			}
		}
		/// <summary>
		/// 2020.09.28 by twkang [ADD] 유저 관리창 라벨을 클릭했을 경우
		/// </summary>
		private void Click_ManagementLabel(object sender, EventArgs e)
		{
			Sys3Controls.Sys3Label ctrl		= sender as Sys3Controls.Sys3Label;
			string strValue		= string.Empty;

			if(false == m_InstanceKeyboard.CreateForm(ctrl.Text))
			{
				return;
			}
			m_InstanceKeyboard.GetResult(ref strValue);
			ctrl.Text			= strValue;
			ctrl.MainFontColor	= Color.Black;
		}
		/// <summary>
		/// 2020.09.28 by twkang [ADD] 유저 관리창을 닫는다.
		/// </summary>
		private void Click_CloseManagement(object sender, EventArgs e)
		{
			ShowLogInDisplay();
		}
		/// <summary>
		/// 2020.09.28 by twkang [ADD] 유저 관리창을 연다.
		/// </summary>
		private void Click_OpenManagement(object sender, EventArgs e)
		{
			UpdateUserList();

			ShowManagementDisplay();
		}

        /// <summary>
        /// 2021.04.07 by yjlee [ADD] Click the iris recognition to login on the system.
        /// </summary>
        private void Click_IrisRecognition(object sender, EventArgs e)
        {
            //m_formIrisRecognition       = new Form_IrisRecognition(true);
			//
            //m_formIrisRecognition.ShowDialog();
			//
            //m_formIrisRecognition.Dispose();
            //m_formIrisRecognition       = null;
        }
        #endregion
    }
}
