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

namespace FrameOfSystem3.Views
{
	public partial class ParameterGroupPanel : UserControlForMainView.CustomView
	{
		const int SIZE_EXTAND_OFFSET = 5;
		bool _isExtend = true;
		ParameterPanel _myPanel;
		ControlInterface _controlInterface = new ControlInterface();
		bool _isExtendOnly = false;
		List<ParameterPanel> _extendPanels = new List<ParameterPanel>();
		int _totalExtendPanelHeight = 0;

		public ParameterGroupPanel(ParameterPanel myPanel, bool isDefaultReduce = false, bool isExtendOnly = false)
		{
			InitializeComponent();

			_myPanel = myPanel;
			this.Dock = DockStyle.Top;
			panelView.Controls.Add(_myPanel);
			this.Hide();
			this.Height = _myPanel.Height + panelSubject.Height + SIZE_EXTAND_OFFSET;
			panelView.Height = _myPanel.Height;

            if (_myPanel.Tag == null)
                _myPanel.Tag = string.Empty;
				
			gb_SubjectLabel.Text = _myPanel.Tag.ToString();

			_isExtendOnly = isExtendOnly;
			if (isDefaultReduce)
			{
				PanelExtendReduce(false);
			}

			_controlInterface.AssignControlsWithAutoRefresh(_myPanel.Controls);
			_myPanel.LinkControlInterface(_controlInterface);
		}

		#region <OVERRIDE>
		public override void CallFunctionByTimer()
		{
			//_controlInterface.RefreshValueParameter();
			_myPanel.CallFunctionByTimer();

			foreach(var panel in _extendPanels)
			{
				panel.CallFunctionByTimer();
			}

			base.CallFunctionByTimer();
		}
		protected override void ProcessWhenActivation()
		{
			_myPanel.ActivateView();

			foreach (var panel in _extendPanels)
			{
				panel.ActivateView();
			}

			if (false == _isExtend)
			{
				panelView.Hide();
				_myPanel.Hide();
				foreach (var panel in _extendPanels)
				{
					panel.Hide();
				}
			}
			else
			{
				panelView.Show();
				_myPanel.Show();
				foreach (var panel in _extendPanels)
				{
					panel.Show();
				}
			}

			_controlInterface.RefreshValueParameter();
			base.ProcessWhenActivation();
		}
		protected override void ProcessWhenDeactivation()
		{
			base.ProcessWhenDeactivation();
			_myPanel.DeactivateView();

			foreach (var panel in _extendPanels)
			{
				panel.DeactivateView();
			}
		}
		#endregion </OVERRIDE>

        // 2024.03.08. jhlim [ADD] 그룹박스를 사용하지 않을 때 옵션을 위한 인터페이스
        public void DisableGroupBox()
        {
            this.Controls.Remove(this.panelSubject);
            panelSubject.Dispose();

            this.Height = _myPanel.Height + _totalExtendPanelHeight;
        }

		// 2024.06.24 by junho [ADD] panel 덧붙이기
		public void ExtendPanel(params ParameterPanel[] newPanels)
		{
			if (newPanels == null || newPanels.Length < 1)
				return;

			foreach(var panel in newPanels)
			{
				_extendPanels.Add(panel);
				panel.Location = new Point(panel.Location.X, panelView.Height);
				_totalExtendPanelHeight += panel.Height;
				panelView.Height += panel.Height;
				this.Height += panel.Height;

				panelView.Controls.Add(panel);

				_controlInterface.AssignControlsWithAutoRefresh(panel.Controls);
				panel.LinkControlInterface(_controlInterface);
			}
		}
		public void SetGroupTitle(string newTitle)
		{
			gb_SubjectLabel.Text = newTitle;
		}

        private void gb_SubjectLabel_Click(object sender, EventArgs e)
        {
            PanelExtendReduce(!_isExtend);
        }

		public void PanelExtendReduce(bool doExtend)
		{
			if (doExtend)
			{
				panelView.Show();
				_myPanel.Show();
				foreach (var panel in _extendPanels)
				{
					panel.Show();
				}

				this.Height = _myPanel.Height + panelSubject.Height + SIZE_EXTAND_OFFSET + _totalExtendPanelHeight;
				_isExtend = true;
				gb_SubjectLabel.LabelGradientColorFirst = Color.DarkGray;
				gb_SubjectLabel.LabelGradientColorSecond = Color.WhiteSmoke;
			}
			else
			{
				if (_isExtendOnly) return;

				panelView.Hide();
				_myPanel.Hide();
				foreach (var panel in _extendPanels)
				{
					panel.Hide();
				}

				this.Height = panelSubject.Height;
				_isExtend = false;
				gb_SubjectLabel.LabelGradientColorFirst = Color.DimGray;
				gb_SubjectLabel.LabelGradientColorSecond = Color.DarkGray;
			}
		}

		public ControlInterface GetControlInterface()
		{
			return _controlInterface;
		}

		public void ChangeTargetTask(string taskFrom, string taskTo)
		{
			_controlInterface.ChangeTargetTask(taskFrom, taskTo);
		}

		public void SelectedMyPanel(string panelName)
		{
			_myPanel.SelectedMe(_controlInterface, panelName);
			foreach (var panel in _extendPanels)
			{
				panel.SelectedMe(_controlInterface, panelName);
			}
		}
		public bool IsThisPanel(ParameterPanel target)
		{
			return _myPanel.Equals(target);
		}
	}
}
