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
    public partial class Config_JogManage : UserControlForMainView.CustomView
    {
		public Config_JogManage()
        {
            InitializeComponent();
			InitializePanel();
		}

		#region Variables

		#region GUI
		private Sys3Controls.Sys3button				m_tabClicked        = null;
		private UserControlForMainView.CustomView m_selectedPanel = null;
		private UserControlForMainView.CustomView m_selectedPanelSub = null;

		private Config_Jog m_viewConfigJog = new Config_Jog();
		private Config_JogReverse m_viewConfigJogReverse = new Config_JogReverse();
		private bool _viewSubPanel = false;
		#endregion
		
		#endregion

		#region 상속 인터페이스
		/// <summary>
        /// 
        /// </summary>
        protected override void ProcessWhenActivation()
        {
			if (false == _viewSubPanel)
				m_selectedPanel.ActivateView();
			else 
				m_selectedPanelSub.ActivateView();
        }
        /// <summary>
        /// 
        /// </summary>
        protected override void ProcessWhenDeactivation()
        {
			if (false == _viewSubPanel)
				m_selectedPanel.DeactivateView();
			else
				m_selectedPanelSub.DeactivateView();
        }
        /// <summary>
        /// 
        /// </summary>
        public override void CallFunctionByTimer()
        {
			if (false == _viewSubPanel)
				m_selectedPanel.CallFunctionByTimer();
			else
				m_selectedPanelSub.CallFunctionByTimer();
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

			UserControlForMainView.CustomView selectPanel;
			bool isSubPanel;

			switch (nIndex)
			{
				case 0: //Main
					m_tabClicked = m_tabMain;
					selectPanel = m_viewConfigJog;
					isSubPanel = false;
					break;

				case 1: //Reverse
					m_tabClicked = m_tabReverse;
					selectPanel = m_viewConfigJogReverse;
					isSubPanel = true;
					break;
				default:
					return;
			}

			m_tabClicked.ButtonClicked = true;
			m_tabClicked.GradientFirstColor = Color.DarkBlue;
			m_tabClicked.GradientSecondColor = Color.DarkBlue;
			m_tabClicked.MainFontColor = Color.White;

			if (false == isSubPanel)
			{
				m_selectedPanel.Hide();
				m_selectedPanel.DeactivateView();

				m_selectedPanel = selectPanel;

				m_selectedPanel.ActivateView();
				m_selectedPanel.Show();

				m_selectedPanelSub.Hide();
				m_selectedPanelSub.DeactivateView();
				m_panelSub.Hide();
			}
			else
			{
				m_selectedPanelSub.Hide();
				m_selectedPanelSub.DeactivateView();

				m_selectedPanelSub = selectPanel;

				m_selectedPanelSub.ActivateView();
				m_selectedPanelSub.Show();
				m_panelSub.Show();
			}

			_viewSubPanel = isSubPanel;
		}
		/// <summary>
		/// 2020.06.23 by twkang [ADD] 패널 초기화를 진행한다.
		/// </summary>
		private void InitializePanel()
		{
            m_panel.Controls.Add(m_viewConfigJog);
            m_viewConfigJog.Dock = DockStyle.Fill;
            
			m_tabClicked		= m_tabMain;
			m_selectedPanel		= m_viewConfigJog;

			m_selectedPanelSub = m_viewConfigJogReverse;
			m_panelSub.Controls.Add(m_viewConfigJogReverse);
			m_panelSub.Hide();
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
