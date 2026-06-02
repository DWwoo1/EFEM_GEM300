using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace FrameOfSystem3.Views
{   
    /// <summary>
    /// 2024.01.29. by shkim. 박준호 과장이 만든 Parameter Panel을 상속받은 것으로
    /// Parameter Panel 안에 Tab Button으로 Sub Parameter Panel이 있다.
    /// </summary>
    public class ParameterPanelWithTabView : ParameterPanel
    {
        Panel _panelForTabView = null;

        //Sys3Controls.Sys3button _firstTabButton = null;
        Dictionary<int, Sys3Controls.Sys3button> _dicTabButton = new Dictionary<int, Sys3Controls.Sys3button>();
        Dictionary<Sys3Controls.Sys3button, ParameterPanel> _dicSubView = new Dictionary<Sys3Controls.Sys3button, ParameterPanel>();

        // ParameterPanel _selectedPanel = null;

        Sys3Controls.Sys3button _selectedTabButton = null;

        /// <summary>
        /// SubView가 할당될 Panel을 Child View로부터 할당받는다.
        /// </summary>
        /// <param name="panel"></param>
        protected void AssignPanelForSubView(Panel panel)
        {
            if (_panelForTabView == null)
            {
                _panelForTabView = panel;
            }
        }

        /// <summary>
        /// Child에 생성된 panel을 주입하고, subParameterPanel을 Add한다.
        /// </summary>
        /// <param name="panel"></param>
        /// <param name="subParameterPanel"></param>
        protected void AddSubViewToPanel(Sys3Controls.Sys3button[] tabButtons, ParameterPanel[] subParameterPanels)
        {
            if(_panelForTabView == null || tabButtons == null || subParameterPanels == null || (tabButtons.Length != subParameterPanels.Length))
            {
                return;
            }
            int count = tabButtons.Length;

            for(int tabIndex = 0 ; tabIndex < count; tabIndex++)
            {
                tabButtons[tabIndex].Click += Clicked_TabButton;

                _dicTabButton.Add(tabIndex, tabButtons[tabIndex]);
                _dicSubView.Add(tabButtons[tabIndex], subParameterPanels[tabIndex]);

                _panelForTabView.Controls.Add(subParameterPanels[tabIndex]);
                subParameterPanels[tabIndex].Dock = DockStyle.Fill; // 2024.02.13. by shkim. [ADD] Group Box 내에서 스크롤바 제거 목적

                if (_selectedTabButton == null)
                {
                    _selectedTabButton = tabButtons[tabIndex];
                    subParameterPanels[tabIndex].Show();

                    _selectedTabButton.ButtonClicked = true;
                }
                else
                {
                    // 2024.03.21. by shkim. [ADD] 최초에는 HIDE 상태여야한다.
                    subParameterPanels[tabIndex].Hide();
                }
            }
        }

        protected override void ProcessWhenActivation()
        {
            base.ProcessWhenActivation();
        }
        protected override void ProcessWhenDeactivation()
        {
            base.ProcessWhenDeactivation();
        }
        public override void CallFunctionByTimer()
        {
            if (_selectedTabButton != null)
            {
                _dicSubView[_selectedTabButton].CallFunctionByTimer();
            }
            base.CallFunctionByTimer();
        }

        #region <내부함수>
        private void Clicked_TabButton(object sender, EventArgs e)
        {
            if (sender is Sys3Controls.Sys3button)
            {
                Sys3Controls.Sys3button btnClickedTab = sender as Sys3Controls.Sys3button;

                if (btnClickedTab.Equals(_selectedTabButton))
                {
                    return;
                }

                _selectedTabButton.ButtonClicked = false;
                _dicSubView[_selectedTabButton].Hide();
                btnClickedTab.ButtonClicked = true;

                _selectedTabButton = btnClickedTab;
                _dicSubView[_selectedTabButton].Show();
            }
        }

        #endregion </내부함수>
    }
}