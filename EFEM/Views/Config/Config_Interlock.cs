using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using FrameOfSystem3.Config;

namespace FrameOfSystem3.Views.Config
{
    public partial class Config_Interlock : UserControlForMainView.CustomView
    {
        public Config_Interlock()
        {
            InitializeComponent();

			InitializePanel();
		}

		#region Variables

		#region GUI
		private Sys3Controls.Sys3button				m_tabClicked        = null;
		private UserControlForMainView.CustomView   m_selectedPanel     = null;

		private Interlock_Motion		m_viewInterlockMotion				= new Interlock_Motion();
		private Interlock_Cylinder		m_viewInterlockCylinder				= new Interlock_Cylinder();
		private Interlock_DO		    m_viewInterlockDO				    = new Interlock_DO();
// 		private Config_Flow		m_viewConfigFlow				= new Config_Flow();
// 		private Config_Port		m_viewConfigPort				= new Config_Port();
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
			//if (m_tabClicked.TabIndex == nIndex) { return; }

			m_tabClicked.GradientFirstColor = Color.White;
			m_tabClicked.GradientSecondColor = Color.White;
			m_tabClicked.MainFontColor = Color.DarkBlue;
			m_tabClicked.ButtonClicked = false;
			m_selectedPanel.Hide();
			m_selectedPanel.DeactivateView();

			switch (nIndex)
			{
				case 0: //Motion
					m_tabClicked = m_tabMotion;
                    m_selectedPanel = m_viewInterlockMotion;
					break;
				case 1: //Flow
					m_tabClicked = m_tabCylinder;
                    m_selectedPanel = m_viewInterlockCylinder;
					break;
                case 2: //Flow
                    m_tabClicked = m_tabDO;
                    m_selectedPanel = m_viewInterlockDO;
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
            m_panel.Controls.Add(m_viewInterlockMotion);
            m_panel.Controls.Add(m_viewInterlockCylinder);
            m_panel.Controls.Add(m_viewInterlockDO);

            m_viewInterlockMotion.Dock = DockStyle.Fill;
            m_viewInterlockCylinder.Dock = DockStyle.Fill;
            m_viewInterlockDO.Dock = DockStyle.Fill;
//             
			m_tabClicked		= m_tabMotion;
            m_selectedPanel = m_viewInterlockMotion;

            m_viewInterlockCylinder.Hide();
            m_viewInterlockDO.Hide();
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
