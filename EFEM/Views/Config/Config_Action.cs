using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using FrameOfSystem3.Views.Config.Action;

namespace FrameOfSystem3.Views.Config
{
    public partial class Config_Action : UserControlForMainView.CustomView
    {
        public Config_Action()
        {
            InitializeComponent();

// 			#region TabIndex
// 			m_tabLink.TabIndex		= 0;
// 			m_tabPort.TabIndex		= 1;
// 			#endregion

			InitializePanel();
		}

		#region Variables

		#region GUI
		private Sys3Controls.Sys3button				m_tabClicked        = null;
		private UserControlForMainView.CustomView   m_selectedPanel     = null;

		private Config_Link		m_viewConfigLink				= new Config_Link();
		private Config_Flow		m_viewConfigFlow				= new Config_Flow();
		private Config_Port		m_viewConfigPort				= new Config_Port();
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
				case 0: //Link
					m_tabClicked = m_tabLink;
					m_selectedPanel = m_viewConfigLink;
					break;
				case 1: //Flow
					m_tabClicked = m_tabFlow;
					m_selectedPanel = m_viewConfigFlow;
					break;
                case 2: //Flow
                    m_tabClicked = m_tabPort;
                    m_selectedPanel = m_viewConfigPort;
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
            m_panel.Controls.Add(m_viewConfigLink);
            m_panel.Controls.Add(m_viewConfigFlow);
            m_panel.Controls.Add(m_viewConfigPort);

            m_viewConfigLink.Dock = DockStyle.Fill;
            m_viewConfigFlow.Dock = DockStyle.Fill;
            m_viewConfigPort.Dock = DockStyle.Fill;
            
			m_tabClicked		= m_tabLink;
			m_selectedPanel		= m_viewConfigLink;

            m_viewConfigFlow.Hide();
            m_viewConfigPort.Hide();
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
