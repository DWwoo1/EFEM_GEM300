namespace FrameOfSystem3.Views.Functional
{
    partial class Form_ColorPalette
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
            this.m_btnCancel = new Sys3Controls.Sys3button();
            this.m_btnApply = new Sys3Controls.Sys3button();
            this.m_ledColor = new Sys3Controls.Sys3LedLabel();
            this.m_pBarBlue = new Sys3Controls.Sys3ProgressBar();
            this.m_pBarGreen = new Sys3Controls.Sys3ProgressBar();
            this.m_pBarRed = new Sys3Controls.Sys3ProgressBar();
            this.m_groupColorPalette = new Sys3Controls.Sys3GroupBox();
            this.SuspendLayout();
            // 
            // m_btnCancel
            // 
            this.m_btnCancel.BorderWidth = 3;
            this.m_btnCancel.ButtonClicked = false;
            this.m_btnCancel.ClickedEmphasizeTextColor = System.Drawing.Color.White;
            this.m_btnCancel.CustomClickedGradientFirstColor = System.Drawing.Color.White;
            this.m_btnCancel.CustomClickedGradientSecondColor = System.Drawing.Color.White;
            this.m_btnCancel.Description = "";
            this.m_btnCancel.DisabledColor = System.Drawing.Color.Silver;
            this.m_btnCancel.EdgeRadius = 10;
            this.m_btnCancel.GradientAngle = 90F;
            this.m_btnCancel.GradientFirstColor = System.Drawing.Color.FromArgb(((int)(((byte)(120)))), ((int)(((byte)(130)))), ((int)(((byte)(150)))));
            this.m_btnCancel.GradientSecondColor = System.Drawing.Color.FromArgb(((int)(((byte)(90)))), ((int)(((byte)(90)))), ((int)(((byte)(100)))));
            this.m_btnCancel.HoverEmphasizeCustomColor = System.Drawing.Color.White;
            this.m_btnCancel.ImagePosition = new System.Drawing.Point(0, 0);
            this.m_btnCancel.ImageSize = new System.Drawing.Point(0, 0);
            this.m_btnCancel.LoadImage = null;
            this.m_btnCancel.Location = new System.Drawing.Point(126, 192);
            this.m_btnCancel.MainFont = new System.Drawing.Font("맑은 고딕", 10F, System.Drawing.FontStyle.Bold);
            this.m_btnCancel.MainFontColor = System.Drawing.Color.White;
            this.m_btnCancel.Name = "m_btnCancel";
            this.m_btnCancel.Size = new System.Drawing.Size(115, 69);
            this.m_btnCancel.SubFont = new System.Drawing.Font("맑은 고딕", 10F, System.Drawing.FontStyle.Bold);
            this.m_btnCancel.SubFontColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.m_btnCancel.SubText = "";
            this.m_btnCancel.TabIndex = 9;
            this.m_btnCancel.Text = "CANCEL";
            this.m_btnCancel.TextAlignMain = Sys3Controls.EN_TEXTALIGN.MIDDLE_CENTER;
            this.m_btnCancel.TextAlignSub = Sys3Controls.EN_TEXTALIGN.MIDDLE_CENTER;
            this.m_btnCancel.ThemeIndex = 0;
            this.m_btnCancel.UseBorder = true;
            this.m_btnCancel.UseClickedEmphasizeTextColor = false;
            this.m_btnCancel.UseCustomizeClickedColor = false;
            this.m_btnCancel.UseEdge = false;
            this.m_btnCancel.UseHoverEmphasizeCustomColor = false;
            this.m_btnCancel.UseImage = false;
            this.m_btnCancel.UserHoverEmpahsize = false;
            this.m_btnCancel.UseSubFont = false;
            this.m_btnCancel.Click += new System.EventHandler(this.ClIck_Cancel);
            // 
            // m_btnApply
            // 
            this.m_btnApply.BorderWidth = 3;
            this.m_btnApply.ButtonClicked = false;
            this.m_btnApply.ClickedEmphasizeTextColor = System.Drawing.Color.White;
            this.m_btnApply.CustomClickedGradientFirstColor = System.Drawing.Color.White;
            this.m_btnApply.CustomClickedGradientSecondColor = System.Drawing.Color.White;
            this.m_btnApply.Description = "";
            this.m_btnApply.DisabledColor = System.Drawing.Color.Silver;
            this.m_btnApply.EdgeRadius = 10;
            this.m_btnApply.GradientAngle = 90F;
            this.m_btnApply.GradientFirstColor = System.Drawing.Color.FromArgb(((int)(((byte)(120)))), ((int)(((byte)(130)))), ((int)(((byte)(150)))));
            this.m_btnApply.GradientSecondColor = System.Drawing.Color.FromArgb(((int)(((byte)(90)))), ((int)(((byte)(90)))), ((int)(((byte)(100)))));
            this.m_btnApply.HoverEmphasizeCustomColor = System.Drawing.Color.White;
            this.m_btnApply.ImagePosition = new System.Drawing.Point(0, 0);
            this.m_btnApply.ImageSize = new System.Drawing.Point(0, 0);
            this.m_btnApply.LoadImage = null;
            this.m_btnApply.Location = new System.Drawing.Point(5, 192);
            this.m_btnApply.MainFont = new System.Drawing.Font("맑은 고딕", 10F, System.Drawing.FontStyle.Bold);
            this.m_btnApply.MainFontColor = System.Drawing.Color.White;
            this.m_btnApply.Name = "m_btnApply";
            this.m_btnApply.Size = new System.Drawing.Size(115, 69);
            this.m_btnApply.SubFont = new System.Drawing.Font("맑은 고딕", 10F, System.Drawing.FontStyle.Bold);
            this.m_btnApply.SubFontColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.m_btnApply.SubText = "";
            this.m_btnApply.TabIndex = 8;
            this.m_btnApply.Text = "APPLY";
            this.m_btnApply.TextAlignMain = Sys3Controls.EN_TEXTALIGN.MIDDLE_CENTER;
            this.m_btnApply.TextAlignSub = Sys3Controls.EN_TEXTALIGN.MIDDLE_CENTER;
            this.m_btnApply.ThemeIndex = 0;
            this.m_btnApply.UseBorder = true;
            this.m_btnApply.UseClickedEmphasizeTextColor = false;
            this.m_btnApply.UseCustomizeClickedColor = false;
            this.m_btnApply.UseEdge = false;
            this.m_btnApply.UseHoverEmphasizeCustomColor = false;
            this.m_btnApply.UseImage = false;
            this.m_btnApply.UserHoverEmpahsize = false;
            this.m_btnApply.UseSubFont = false;
            this.m_btnApply.Click += new System.EventHandler(this.Click_Apply);
            // 
            // m_ledColor
            // 
            this.m_ledColor.Active = true;
            this.m_ledColor.ColorActive = System.Drawing.Color.FromArgb(((int)(((byte)(127)))), ((int)(((byte)(127)))), ((int)(((byte)(127)))));
            this.m_ledColor.ColorNormal = System.Drawing.Color.DimGray;
            this.m_ledColor.Location = new System.Drawing.Point(372, 39);
            this.m_ledColor.Name = "m_ledColor";
            this.m_ledColor.Size = new System.Drawing.Size(222, 222);
            this.m_ledColor.TabIndex = 7;
            // 
            // m_pBarBlue
            // 
            this.m_pBarBlue.BackGroundColor = System.Drawing.Color.Transparent;
            this.m_pBarBlue.BorderStroke = 1;
            this.m_pBarBlue.FirstColor = System.Drawing.Color.Black;
            this.m_pBarBlue.GageCount = 10;
            this.m_pBarBlue.LabelWidth = 50;
            this.m_pBarBlue.Location = new System.Drawing.Point(5, 141);
            this.m_pBarBlue.MaxTickCount = ((uint)(255u));
            this.m_pBarBlue.MinTickCount = ((uint)(1u));
            this.m_pBarBlue.Name = "m_pBarBlue";
            this.m_pBarBlue.NormalGageColor = System.Drawing.Color.WhiteSmoke;
            this.m_pBarBlue.OffSet = 2;
            this.m_pBarBlue.SecondColor = System.Drawing.Color.Blue;
            this.m_pBarBlue.Size = new System.Drawing.Size(352, 45);
            this.m_pBarBlue.TabIndex = 2;
            this.m_pBarBlue.Tick = ((uint)(150u));
            this.m_pBarBlue.TickFont = new System.Drawing.Font("맑은 고딕", 10F, System.Drawing.FontStyle.Bold);
            this.m_pBarBlue.TickFontColor = System.Drawing.Color.Black;
            this.m_pBarBlue.UseBorder = true;
            this.m_pBarBlue.UseControlBorder = true;
            this.m_pBarBlue.UseLinearGradientMode = true;
            this.m_pBarBlue.MouseUp += new System.Windows.Forms.MouseEventHandler(this.MouseUp_ProgressBar);
            // 
            // m_pBarGreen
            // 
            this.m_pBarGreen.BackGroundColor = System.Drawing.Color.Transparent;
            this.m_pBarGreen.BorderStroke = 1;
            this.m_pBarGreen.FirstColor = System.Drawing.Color.Black;
            this.m_pBarGreen.GageCount = 10;
            this.m_pBarGreen.LabelWidth = 50;
            this.m_pBarGreen.Location = new System.Drawing.Point(5, 90);
            this.m_pBarGreen.MaxTickCount = ((uint)(255u));
            this.m_pBarGreen.MinTickCount = ((uint)(1u));
            this.m_pBarGreen.Name = "m_pBarGreen";
            this.m_pBarGreen.NormalGageColor = System.Drawing.Color.WhiteSmoke;
            this.m_pBarGreen.OffSet = 2;
            this.m_pBarGreen.SecondColor = System.Drawing.Color.Green;
            this.m_pBarGreen.Size = new System.Drawing.Size(352, 45);
            this.m_pBarGreen.TabIndex = 1;
            this.m_pBarGreen.Tick = ((uint)(150u));
            this.m_pBarGreen.TickFont = new System.Drawing.Font("맑은 고딕", 10F, System.Drawing.FontStyle.Bold);
            this.m_pBarGreen.TickFontColor = System.Drawing.Color.Black;
            this.m_pBarGreen.UseBorder = true;
            this.m_pBarGreen.UseControlBorder = true;
            this.m_pBarGreen.UseLinearGradientMode = true;
            this.m_pBarGreen.MouseUp += new System.Windows.Forms.MouseEventHandler(this.MouseUp_ProgressBar);
            // 
            // m_pBarRed
            // 
            this.m_pBarRed.BackGroundColor = System.Drawing.Color.Transparent;
            this.m_pBarRed.BorderStroke = 1;
            this.m_pBarRed.FirstColor = System.Drawing.Color.Black;
            this.m_pBarRed.GageCount = 10;
            this.m_pBarRed.LabelWidth = 50;
            this.m_pBarRed.Location = new System.Drawing.Point(5, 39);
            this.m_pBarRed.MaxTickCount = ((uint)(255u));
            this.m_pBarRed.MinTickCount = ((uint)(1u));
            this.m_pBarRed.Name = "m_pBarRed";
            this.m_pBarRed.NormalGageColor = System.Drawing.Color.WhiteSmoke;
            this.m_pBarRed.OffSet = 2;
            this.m_pBarRed.SecondColor = System.Drawing.Color.Red;
            this.m_pBarRed.Size = new System.Drawing.Size(352, 45);
            this.m_pBarRed.TabIndex = 0;
            this.m_pBarRed.Tick = ((uint)(150u));
            this.m_pBarRed.TickFont = new System.Drawing.Font("맑은 고딕", 10F, System.Drawing.FontStyle.Bold);
            this.m_pBarRed.TickFontColor = System.Drawing.Color.Black;
            this.m_pBarRed.UseBorder = true;
            this.m_pBarRed.UseControlBorder = true;
            this.m_pBarRed.UseLinearGradientMode = true;
            this.m_pBarRed.MouseUp += new System.Windows.Forms.MouseEventHandler(this.MouseUp_ProgressBar);
            // 
            // m_groupColorPalette
            // 
            this.m_groupColorPalette.BackGroundColor = System.Drawing.Color.Gainsboro;
            this.m_groupColorPalette.EdgeBorderStroke = 2;
            this.m_groupColorPalette.EdgeRadius = 2;
            this.m_groupColorPalette.LabelFont = new System.Drawing.Font("맑은 고딕", 10F, System.Drawing.FontStyle.Bold);
            this.m_groupColorPalette.LabelGradientColorFirst = System.Drawing.Color.FromArgb(((int)(((byte)(120)))), ((int)(((byte)(130)))), ((int)(((byte)(150)))));
            this.m_groupColorPalette.LabelGradientColorSecond = System.Drawing.Color.FromArgb(((int)(((byte)(90)))), ((int)(((byte)(90)))), ((int)(((byte)(100)))));
            this.m_groupColorPalette.LabelHeight = 30;
            this.m_groupColorPalette.LabelTextColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.m_groupColorPalette.Location = new System.Drawing.Point(0, 0);
            this.m_groupColorPalette.Name = "m_groupColorPalette";
            this.m_groupColorPalette.Size = new System.Drawing.Size(600, 267);
            this.m_groupColorPalette.TabIndex = 0;
            this.m_groupColorPalette.Text = "COLOR PALETTE";
            this.m_groupColorPalette.TextAlign = Sys3Controls.EN_TEXTALIGN.MIDDLE_CENTER;
            this.m_groupColorPalette.ThemeIndex = 0;
            this.m_groupColorPalette.UseLabelBorder = true;
            this.m_groupColorPalette.MouseDown += new System.Windows.Forms.MouseEventHandler(this.OnMouseDown_FormGroupBox);
            this.m_groupColorPalette.MouseMove += new System.Windows.Forms.MouseEventHandler(this.OnMouseMove_FormGroupBox);
            // 
            // Form_ColorPalette
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.ClientSize = new System.Drawing.Size(600, 267);
            this.ControlBox = false;
            this.Controls.Add(this.m_pBarBlue);
            this.Controls.Add(this.m_pBarGreen);
            this.Controls.Add(this.m_pBarRed);
            this.Controls.Add(this.m_btnCancel);
            this.Controls.Add(this.m_btnApply);
            this.Controls.Add(this.m_ledColor);
            this.Controls.Add(this.m_groupColorPalette);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "Form_ColorPalette";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.Text = "Form_ColorPalette";
            this.TopMost = true;
            this.ResumeLayout(false);

        }

        #endregion

		private Sys3Controls.Sys3GroupBox m_groupColorPalette;
		private Sys3Controls.Sys3ProgressBar m_pBarRed;
		private Sys3Controls.Sys3ProgressBar m_pBarGreen;
		private Sys3Controls.Sys3ProgressBar m_pBarBlue;
		private Sys3Controls.Sys3LedLabel m_ledColor;
		private Sys3Controls.Sys3button m_btnApply;
		private Sys3Controls.Sys3button m_btnCancel;
    }
}