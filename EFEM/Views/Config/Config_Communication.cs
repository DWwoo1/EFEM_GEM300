using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using FrameOfSystem3.Component;
using FrameOfSystem3.Config;

namespace FrameOfSystem3.Views.Config
{
    public partial class Config_Communication : UserControlForMainView.CustomView
	{
        public Config_Communication()
        {
            InitializeComponent();

			#region TabIndex
			m_tabSocket.TabIndex		= 0;
			m_tabSerial.TabIndex		= 1;
			m_tabWCF.TabIndex		= 2;
			m_tabFTP.TabIndex		= 3;
			#endregion

			InitializePanel();
		}

		#region Variables

		#region GUI
		private Sys3Controls.Sys3button				m_tabClicked        = null;
		private UserControlForMainView.CustomView   m_selectedPanel     = null;

		private Config_Serial		m_viewConfigSerial				= new Config_Serial();
		private Config_Socket		m_viewConfigSocket				= new Config_Socket();
		private Config_WCF			m_viewConfigWCF					= new Config_WCF();
		private Config_FTP			m_viewConfigFTP					= new Config_FTP();
		#endregion
		
		#endregion

		#region 상속 인터페이스
		/// <summary>
        /// 
        /// </summary>
        protected override void ProcessWhenActivation()
        {
			m_selectedPanel.ActivateView();
        }
        /// <summary>
        /// 
        /// </summary>
        protected override void ProcessWhenDeactivation()
        {
			m_selectedPanel.DeactivateView();
        }
		/// <summary>
		/// 
		/// </summary>
		public override void CallFunctionByTimer()
		{
			m_selectedPanel.CallFunctionByTimer();	
        }
        #endregion

		#region Internal Interface
		/// <summary>
		/// 2020.06.23 by twkang [ADD] 패널의 화면을 바꾼다.
		/// </summary>
		private void SetPanelState(int nIndex)
		{
			if (m_tabClicked.TabIndex == nIndex) { return; }

			m_tabClicked.GradientFirstColor = Color.White;
			m_tabClicked.GradientSecondColor = Color.White;
			m_tabClicked.MainFontColor = Color.DarkBlue;
			m_tabClicked.ButtonClicked = false;
			m_selectedPanel.Hide();
			m_selectedPanel.DeactivateView();

			switch (nIndex)
			{
				case 0: //SOCKET
					m_tabClicked = m_tabSocket;
					m_selectedPanel = m_viewConfigSocket;
					break;
				case 1: //SERIAL
					m_tabClicked = m_tabSerial;
					m_selectedPanel = m_viewConfigSerial;
					break;
				case 2: //WCF
					m_tabClicked = m_tabWCF;
					m_selectedPanel = m_viewConfigWCF;
					break;
                case 3: //FTP
                    m_tabClicked = m_tabFTP;
                    m_selectedPanel = m_viewConfigFTP; 
                    break;
				default:
					return;
			}
			m_selectedPanel.ActivateView();
			m_selectedPanel.Show();
			m_tabClicked.ButtonClicked = true;
			m_tabClicked.GradientFirstColor = Color.DarkBlue;
			m_tabClicked.GradientSecondColor = Color.DarkBlue;
			m_tabClicked.MainFontColor = Color.White;
		}
		/// <summary>
		/// 2020.06.23 by twkang [ADD] 패널 초기화를 진행한다.
		/// </summary>
		private void InitializePanel()
		{
			m_panel.Controls.Add(m_viewConfigSerial);
			m_panel.Controls.Add(m_viewConfigSocket);
            m_panel.Controls.Add(m_viewConfigWCF);
            m_panel.Controls.Add(m_viewConfigFTP);

			m_viewConfigSerial.Dock = DockStyle.Fill;
            m_viewConfigSocket.Dock = DockStyle.Fill;
            m_viewConfigWCF.Dock	= DockStyle.Fill;
            m_viewConfigFTP.Dock = DockStyle.Fill;

			m_tabClicked		= m_tabSocket;
			m_selectedPanel		= m_viewConfigSocket;

			m_viewConfigSerial.Hide();
			m_viewConfigWCF.Hide();
            m_viewConfigFTP.Hide();
		}
		#endregion

		#region UI 이벤트
		/// <summary>
		/// 2020.06.23 by twkang [ADD] 패널 버튼 클릭 이벤트
		/// </summary>
		private void Click_Tab(object sender, EventArgs e)
		{
			Control ctr = sender as Control;

			SetPanelState(ctr.TabIndex);
		}
		#endregion

	}
}
