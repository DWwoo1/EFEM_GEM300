namespace FrameOfSystem3.Views.Functional.Jog
{
	partial class Form_Jog
	{
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.IContainer components = null;

		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		/// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
		protected override void Dispose(bool disposing)
		{
			if (disposing && (components != null))
			{
				components.Dispose();
			}
			base.Dispose(disposing);
		}

		#region Windows Form Designer generated code

		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
            this.lbl_SelectedName = new Sys3Controls.Sys3Label();
            this.m_lblName = new Sys3Controls.Sys3Label();
            this.panel_Jog = new System.Windows.Forms.Panel();
            this.panel_Mode = new System.Windows.Forms.Panel();
            this.btn_Extend = new Sys3Controls.Sys3button();
            this.btn_ModeSpeed = new Sys3Controls.Sys3button();
            this.btn_ModeRelative = new Sys3Controls.Sys3button();
            this.m_btnClose = new Sys3Controls.Sys3button();
            this._flowLayoutPanel_Main = new System.Windows.Forms.FlowLayoutPanel();
            this._panelJogMain = new System.Windows.Forms.Panel();
            this._flowLayoutPanel_Main.SuspendLayout();
            this._panelJogMain.SuspendLayout();
            this.SuspendLayout();
            // 
            // lbl_SelectedName
            // 
            this.lbl_SelectedName.BackGroundColor = System.Drawing.Color.White;
            this.lbl_SelectedName.BorderStroke = 2;
            this.lbl_SelectedName.BorderStyle = System.Windows.Forms.ButtonBorderStyle.Solid;
            this.lbl_SelectedName.Description = "";
            this.lbl_SelectedName.DisabledColor = System.Drawing.Color.LightGray;
            this.lbl_SelectedName.Dock = System.Windows.Forms.DockStyle.Top;
            this.lbl_SelectedName.EdgeRadius = 1;
            this.lbl_SelectedName.ImagePosition = new System.Drawing.Point(0, 0);
            this.lbl_SelectedName.ImageSize = new System.Drawing.Point(0, 0);
            this.lbl_SelectedName.LoadImage = null;
            this.lbl_SelectedName.Location = new System.Drawing.Point(0, 20);
            this.lbl_SelectedName.MainFont = new System.Drawing.Font("맑은 고딕", 10F, System.Drawing.FontStyle.Bold);
            this.lbl_SelectedName.MainFontColor = System.Drawing.Color.Black;
            this.lbl_SelectedName.Margin = new System.Windows.Forms.Padding(0);
            this.lbl_SelectedName.Name = "lbl_SelectedName";
            this.lbl_SelectedName.Size = new System.Drawing.Size(287, 35);
            this.lbl_SelectedName.SubFont = new System.Drawing.Font("맑은 고딕", 10F);
            this.lbl_SelectedName.SubFontColor = System.Drawing.Color.Black;
            this.lbl_SelectedName.SubText = "";
            this.lbl_SelectedName.TabIndex = 1418;
            this.lbl_SelectedName.TextAlignMain = Sys3Controls.EN_TEXTALIGN.BOTTOM_RIGHT;
            this.lbl_SelectedName.TextAlignSub = Sys3Controls.EN_TEXTALIGN.BOTTOM_RIGHT;
            this.lbl_SelectedName.TextAlignUnit = Sys3Controls.EN_TEXTALIGN.BOTTOM_RIGHT;
            this.lbl_SelectedName.ThemeIndex = 0;
            this.lbl_SelectedName.UnitAreaRate = 40;
            this.lbl_SelectedName.UnitFont = new System.Drawing.Font("맑은 고딕", 10F, System.Drawing.FontStyle.Bold);
            this.lbl_SelectedName.UnitFontColor = System.Drawing.Color.Black;
            this.lbl_SelectedName.UnitPositionVertical = false;
            this.lbl_SelectedName.UnitText = "hours";
            this.lbl_SelectedName.UseBorder = true;
            this.lbl_SelectedName.UseEdgeRadius = false;
            this.lbl_SelectedName.UseImage = false;
            this.lbl_SelectedName.UseSubFont = false;
            this.lbl_SelectedName.UseUnitFont = false;
            this.lbl_SelectedName.Click += new System.EventHandler(this.Click_ExtendList);
            // 
            // m_lblName
            // 
            this.m_lblName.BackGroundColor = System.Drawing.Color.Silver;
            this.m_lblName.BorderStroke = 2;
            this.m_lblName.BorderStyle = System.Windows.Forms.ButtonBorderStyle.Solid;
            this.m_lblName.Description = "";
            this.m_lblName.DisabledColor = System.Drawing.Color.DarkGray;
            this.m_lblName.Dock = System.Windows.Forms.DockStyle.Top;
            this.m_lblName.EdgeRadius = 1;
            this.m_lblName.ImagePosition = new System.Drawing.Point(0, 0);
            this.m_lblName.ImageSize = new System.Drawing.Point(0, 0);
            this.m_lblName.LoadImage = null;
            this.m_lblName.Location = new System.Drawing.Point(0, 0);
            this.m_lblName.MainFont = new System.Drawing.Font("맑은 고딕", 9F, System.Drawing.FontStyle.Bold);
            this.m_lblName.MainFontColor = System.Drawing.Color.Black;
            this.m_lblName.Margin = new System.Windows.Forms.Padding(0);
            this.m_lblName.Name = "m_lblName";
            this.m_lblName.Size = new System.Drawing.Size(287, 20);
            this.m_lblName.SubFont = new System.Drawing.Font("맑은 고딕", 10F);
            this.m_lblName.SubFontColor = System.Drawing.Color.Black;
            this.m_lblName.SubText = "";
            this.m_lblName.TabIndex = 1419;
            this.m_lblName.Text = "SELECTED NAME";
            this.m_lblName.TextAlignMain = Sys3Controls.EN_TEXTALIGN.MIDDLE_CENTER;
            this.m_lblName.TextAlignSub = Sys3Controls.EN_TEXTALIGN.MIDDLE_CENTER;
            this.m_lblName.TextAlignUnit = Sys3Controls.EN_TEXTALIGN.MIDDLE_CENTER;
            this.m_lblName.ThemeIndex = 0;
            this.m_lblName.UnitAreaRate = 40;
            this.m_lblName.UnitFont = new System.Drawing.Font("맑은 고딕", 10F, System.Drawing.FontStyle.Bold);
            this.m_lblName.UnitFontColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.m_lblName.UnitPositionVertical = false;
            this.m_lblName.UnitText = "";
            this.m_lblName.UseBorder = true;
            this.m_lblName.UseEdgeRadius = false;
            this.m_lblName.UseImage = false;
            this.m_lblName.UseSubFont = false;
            this.m_lblName.UseUnitFont = false;
            // 
            // panel_Jog
            // 
            this.panel_Jog.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel_Jog.Location = new System.Drawing.Point(0, 55);
            this.panel_Jog.Margin = new System.Windows.Forms.Padding(0);
            this.panel_Jog.Name = "panel_Jog";
            this.panel_Jog.Size = new System.Drawing.Size(287, 287);
            this.panel_Jog.TabIndex = 1424;
            // 
            // panel_Mode
            // 
            this.panel_Mode.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel_Mode.Location = new System.Drawing.Point(0, 342);
            this.panel_Mode.Margin = new System.Windows.Forms.Padding(0);
            this.panel_Mode.Name = "panel_Mode";
            this.panel_Mode.Size = new System.Drawing.Size(287, 70);
            this.panel_Mode.TabIndex = 1425;
            // 
            // btn_Extend
            // 
            this.btn_Extend.BorderWidth = 3;
            this.btn_Extend.ButtonClicked = false;
            this.btn_Extend.ClickedEmphasizeTextColor = System.Drawing.Color.White;
            this.btn_Extend.CustomClickedGradientFirstColor = System.Drawing.Color.White;
            this.btn_Extend.CustomClickedGradientSecondColor = System.Drawing.Color.White;
            this.btn_Extend.Description = "";
            this.btn_Extend.DisabledColor = System.Drawing.Color.DarkGray;
            this.btn_Extend.EdgeRadius = 1;
            this.btn_Extend.GradientAngle = 60F;
            this.btn_Extend.GradientFirstColor = System.Drawing.Color.White;
            this.btn_Extend.GradientSecondColor = System.Drawing.Color.DarkGray;
            this.btn_Extend.HoverEmphasizeCustomColor = System.Drawing.Color.White;
            this.btn_Extend.ImagePosition = new System.Drawing.Point(6, 15);
            this.btn_Extend.ImageSize = new System.Drawing.Point(27, 27);
            this.btn_Extend.LoadImage = null;
            this.btn_Extend.Location = new System.Drawing.Point(290, 1);
            this.btn_Extend.MainFont = new System.Drawing.Font("맑은 고딕", 15F, System.Drawing.FontStyle.Bold);
            this.btn_Extend.MainFontColor = System.Drawing.Color.Black;
            this.btn_Extend.Margin = new System.Windows.Forms.Padding(1, 1, 0, 0);
            this.btn_Extend.Name = "btn_Extend";
            this.btn_Extend.Size = new System.Drawing.Size(38, 474);
            this.btn_Extend.SubFont = new System.Drawing.Font("맑은 고딕", 10F);
            this.btn_Extend.SubFontColor = System.Drawing.Color.Black;
            this.btn_Extend.SubText = "";
            this.btn_Extend.TabIndex = 1423;
            this.btn_Extend.Text = "▶";
            this.btn_Extend.TextAlignMain = Sys3Controls.EN_TEXTALIGN.MIDDLE_CENTER;
            this.btn_Extend.TextAlignSub = Sys3Controls.EN_TEXTALIGN.MIDDLE_CENTER;
            this.btn_Extend.ThemeIndex = 0;
            this.btn_Extend.UseBorder = true;
            this.btn_Extend.UseClickedEmphasizeTextColor = false;
            this.btn_Extend.UseCustomizeClickedColor = false;
            this.btn_Extend.UseEdge = true;
            this.btn_Extend.UseHoverEmphasizeCustomColor = false;
            this.btn_Extend.UseImage = false;
            this.btn_Extend.UserHoverEmpahsize = false;
            this.btn_Extend.UseSubFont = false;
            this.btn_Extend.Click += new System.EventHandler(this.Click_ExtendMonitor);
            // 
            // btn_ModeSpeed
            // 
            this.btn_ModeSpeed.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btn_ModeSpeed.BorderWidth = 3;
            this.btn_ModeSpeed.ButtonClicked = false;
            this.btn_ModeSpeed.ClickedEmphasizeTextColor = System.Drawing.Color.White;
            this.btn_ModeSpeed.CustomClickedGradientFirstColor = System.Drawing.Color.White;
            this.btn_ModeSpeed.CustomClickedGradientSecondColor = System.Drawing.Color.White;
            this.btn_ModeSpeed.Description = "";
            this.btn_ModeSpeed.DisabledColor = System.Drawing.Color.DarkGray;
            this.btn_ModeSpeed.EdgeRadius = 5;
            this.btn_ModeSpeed.GradientAngle = 70F;
            this.btn_ModeSpeed.GradientFirstColor = System.Drawing.Color.White;
            this.btn_ModeSpeed.GradientSecondColor = System.Drawing.Color.DarkGray;
            this.btn_ModeSpeed.HoverEmphasizeCustomColor = System.Drawing.Color.White;
            this.btn_ModeSpeed.ImagePosition = new System.Drawing.Point(10, 10);
            this.btn_ModeSpeed.ImageSize = new System.Drawing.Point(30, 30);
            this.btn_ModeSpeed.LoadImage = global::FrameOfSystem3.Properties.Resources.CLEAR_BLACK;
            this.btn_ModeSpeed.Location = new System.Drawing.Point(1, 413);
            this.btn_ModeSpeed.MainFont = new System.Drawing.Font("맑은 고딕", 11F, System.Drawing.FontStyle.Bold);
            this.btn_ModeSpeed.MainFontColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(36)))), ((int)(((byte)(0)))));
            this.btn_ModeSpeed.Margin = new System.Windows.Forms.Padding(1, 1, 0, 0);
            this.btn_ModeSpeed.Name = "btn_ModeSpeed";
            this.btn_ModeSpeed.Size = new System.Drawing.Size(90, 58);
            this.btn_ModeSpeed.SubFont = new System.Drawing.Font("맑은 고딕", 9F, System.Drawing.FontStyle.Bold);
            this.btn_ModeSpeed.SubFontColor = System.Drawing.Color.DarkBlue;
            this.btn_ModeSpeed.SubText = "MODE";
            this.btn_ModeSpeed.TabIndex = 1421;
            this.btn_ModeSpeed.Text = "SPEED";
            this.btn_ModeSpeed.TextAlignMain = Sys3Controls.EN_TEXTALIGN.BOTTOM_RIGHT;
            this.btn_ModeSpeed.TextAlignSub = Sys3Controls.EN_TEXTALIGN.TOP_RIGHT;
            this.btn_ModeSpeed.ThemeIndex = 0;
            this.btn_ModeSpeed.UseBorder = true;
            this.btn_ModeSpeed.UseClickedEmphasizeTextColor = false;
            this.btn_ModeSpeed.UseCustomizeClickedColor = false;
            this.btn_ModeSpeed.UseEdge = true;
            this.btn_ModeSpeed.UseHoverEmphasizeCustomColor = false;
            this.btn_ModeSpeed.UseImage = false;
            this.btn_ModeSpeed.UserHoverEmpahsize = false;
            this.btn_ModeSpeed.UseSubFont = true;
            this.btn_ModeSpeed.Click += new System.EventHandler(this.Click_MotionMode);
            // 
            // btn_ModeRelative
            // 
            this.btn_ModeRelative.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btn_ModeRelative.BorderWidth = 3;
            this.btn_ModeRelative.ButtonClicked = false;
            this.btn_ModeRelative.ClickedEmphasizeTextColor = System.Drawing.Color.White;
            this.btn_ModeRelative.CustomClickedGradientFirstColor = System.Drawing.Color.White;
            this.btn_ModeRelative.CustomClickedGradientSecondColor = System.Drawing.Color.White;
            this.btn_ModeRelative.Description = "";
            this.btn_ModeRelative.DisabledColor = System.Drawing.Color.DarkGray;
            this.btn_ModeRelative.EdgeRadius = 5;
            this.btn_ModeRelative.GradientAngle = 70F;
            this.btn_ModeRelative.GradientFirstColor = System.Drawing.Color.White;
            this.btn_ModeRelative.GradientSecondColor = System.Drawing.Color.DarkGray;
            this.btn_ModeRelative.HoverEmphasizeCustomColor = System.Drawing.Color.White;
            this.btn_ModeRelative.ImagePosition = new System.Drawing.Point(10, 10);
            this.btn_ModeRelative.ImageSize = new System.Drawing.Point(30, 30);
            this.btn_ModeRelative.LoadImage = global::FrameOfSystem3.Properties.Resources.CLEAR_BLACK;
            this.btn_ModeRelative.Location = new System.Drawing.Point(92, 413);
            this.btn_ModeRelative.MainFont = new System.Drawing.Font("맑은 고딕", 11F, System.Drawing.FontStyle.Bold);
            this.btn_ModeRelative.MainFontColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(36)))), ((int)(((byte)(0)))));
            this.btn_ModeRelative.Margin = new System.Windows.Forms.Padding(1, 1, 0, 0);
            this.btn_ModeRelative.Name = "btn_ModeRelative";
            this.btn_ModeRelative.Size = new System.Drawing.Size(90, 58);
            this.btn_ModeRelative.SubFont = new System.Drawing.Font("맑은 고딕", 9F, System.Drawing.FontStyle.Bold);
            this.btn_ModeRelative.SubFontColor = System.Drawing.Color.DarkBlue;
            this.btn_ModeRelative.SubText = "MODE";
            this.btn_ModeRelative.TabIndex = 1422;
            this.btn_ModeRelative.Text = "RELATIVE";
            this.btn_ModeRelative.TextAlignMain = Sys3Controls.EN_TEXTALIGN.BOTTOM_RIGHT;
            this.btn_ModeRelative.TextAlignSub = Sys3Controls.EN_TEXTALIGN.TOP_RIGHT;
            this.btn_ModeRelative.ThemeIndex = 0;
            this.btn_ModeRelative.UseBorder = true;
            this.btn_ModeRelative.UseClickedEmphasizeTextColor = false;
            this.btn_ModeRelative.UseCustomizeClickedColor = false;
            this.btn_ModeRelative.UseEdge = true;
            this.btn_ModeRelative.UseHoverEmphasizeCustomColor = false;
            this.btn_ModeRelative.UseImage = false;
            this.btn_ModeRelative.UserHoverEmpahsize = false;
            this.btn_ModeRelative.UseSubFont = true;
            this.btn_ModeRelative.Click += new System.EventHandler(this.Click_MotionMode);
            // 
            // m_btnClose
            // 
            this.m_btnClose.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.m_btnClose.BorderWidth = 3;
            this.m_btnClose.ButtonClicked = false;
            this.m_btnClose.ClickedEmphasizeTextColor = System.Drawing.Color.White;
            this.m_btnClose.CustomClickedGradientFirstColor = System.Drawing.Color.White;
            this.m_btnClose.CustomClickedGradientSecondColor = System.Drawing.Color.White;
            this.m_btnClose.Description = "";
            this.m_btnClose.DisabledColor = System.Drawing.Color.DarkGray;
            this.m_btnClose.EdgeRadius = 5;
            this.m_btnClose.GradientAngle = 70F;
            this.m_btnClose.GradientFirstColor = System.Drawing.Color.White;
            this.m_btnClose.GradientSecondColor = System.Drawing.Color.DarkGray;
            this.m_btnClose.HoverEmphasizeCustomColor = System.Drawing.Color.White;
            this.m_btnClose.ImagePosition = new System.Drawing.Point(10, 10);
            this.m_btnClose.ImageSize = new System.Drawing.Point(30, 30);
            this.m_btnClose.LoadImage = global::FrameOfSystem3.Properties.Resources.CLEAR_BLACK;
            this.m_btnClose.Location = new System.Drawing.Point(194, 413);
            this.m_btnClose.MainFont = new System.Drawing.Font("맑은 고딕", 11F, System.Drawing.FontStyle.Bold);
            this.m_btnClose.MainFontColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(36)))), ((int)(((byte)(0)))));
            this.m_btnClose.Margin = new System.Windows.Forms.Padding(1, 1, 0, 0);
            this.m_btnClose.Name = "m_btnClose";
            this.m_btnClose.Size = new System.Drawing.Size(91, 58);
            this.m_btnClose.SubFont = new System.Drawing.Font("맑은 고딕", 8F, System.Drawing.FontStyle.Bold);
            this.m_btnClose.SubFontColor = System.Drawing.Color.DarkBlue;
            this.m_btnClose.SubText = "MOVE";
            this.m_btnClose.TabIndex = 1420;
            this.m_btnClose.Text = "CLOSE";
            this.m_btnClose.TextAlignMain = Sys3Controls.EN_TEXTALIGN.MIDDLE_CENTER;
            this.m_btnClose.TextAlignSub = Sys3Controls.EN_TEXTALIGN.TOP_RIGHT;
            this.m_btnClose.ThemeIndex = 0;
            this.m_btnClose.UseBorder = true;
            this.m_btnClose.UseClickedEmphasizeTextColor = false;
            this.m_btnClose.UseCustomizeClickedColor = false;
            this.m_btnClose.UseEdge = true;
            this.m_btnClose.UseHoverEmphasizeCustomColor = false;
            this.m_btnClose.UseImage = false;
            this.m_btnClose.UserHoverEmpahsize = false;
            this.m_btnClose.UseSubFont = false;
            this.m_btnClose.Click += new System.EventHandler(this.Click_Close);
            // 
            // _flowLayoutPanel_Main
            // 
            this._flowLayoutPanel_Main.AutoSize = true;
            this._flowLayoutPanel_Main.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this._flowLayoutPanel_Main.Controls.Add(this._panelJogMain);
            this._flowLayoutPanel_Main.Controls.Add(this.btn_Extend);
            this._flowLayoutPanel_Main.Location = new System.Drawing.Point(0, 0);
            this._flowLayoutPanel_Main.Margin = new System.Windows.Forms.Padding(0);
            this._flowLayoutPanel_Main.Name = "_flowLayoutPanel_Main";
            this._flowLayoutPanel_Main.Size = new System.Drawing.Size(328, 475);
            this._flowLayoutPanel_Main.TabIndex = 1427;
            // 
            // _panelJogMain
            // 
            this._panelJogMain.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this._panelJogMain.Controls.Add(this.m_btnClose);
            this._panelJogMain.Controls.Add(this.btn_ModeRelative);
            this._panelJogMain.Controls.Add(this.btn_ModeSpeed);
            this._panelJogMain.Controls.Add(this.panel_Mode);
            this._panelJogMain.Controls.Add(this.panel_Jog);
            this._panelJogMain.Controls.Add(this.lbl_SelectedName);
            this._panelJogMain.Controls.Add(this.m_lblName);
            this._panelJogMain.Location = new System.Drawing.Point(2, 0);
            this._panelJogMain.Margin = new System.Windows.Forms.Padding(2, 0, 0, 0);
            this._panelJogMain.Name = "_panelJogMain";
            this._panelJogMain.Size = new System.Drawing.Size(287, 475);
            this._panelJogMain.TabIndex = 0;
            // 
            // Form_Jog
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.AutoSize = true;
            this.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(577, 561);
            this.ControlBox = false;
            this.Controls.Add(this._flowLayoutPanel_Main);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.KeyPreview = true;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "Form_Jog";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "JOG";
            this.TopMost = true;
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.Form_Jog_KeyDown);
            this.KeyUp += new System.Windows.Forms.KeyEventHandler(this.Form_Jog_KeyUp);
            this._flowLayoutPanel_Main.ResumeLayout(false);
            this._panelJogMain.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

		}

		#endregion

		private Sys3Controls.Sys3Label lbl_SelectedName;
		private Sys3Controls.Sys3Label m_lblName;
		private Sys3Controls.Sys3button m_btnClose;
		private Sys3Controls.Sys3button btn_ModeRelative;
		private Sys3Controls.Sys3button btn_ModeSpeed;
		private Sys3Controls.Sys3button btn_Extend;
		private System.Windows.Forms.Panel panel_Jog;
		private System.Windows.Forms.Panel panel_Mode;
        private System.Windows.Forms.FlowLayoutPanel _flowLayoutPanel_Main;
        private System.Windows.Forms.Panel _panelJogMain;
    }
}