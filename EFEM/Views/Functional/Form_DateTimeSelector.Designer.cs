namespace FrameOfSystem3.Views.Functional
{
    partial class Form_DateTimeSelector
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
			this.components = new System.ComponentModel.Container();
			this.btn_Cancel = new Sys3Controls.Sys3button();
			this.btn_Apply = new Sys3Controls.Sys3button();
			this.panel_Buttons = new System.Windows.Forms.Panel();
			this.btn_Zero = new Sys3Controls.Sys3button();
			this.panel_Dials = new System.Windows.Forms.Panel();
			this.timer_DragMode = new System.Windows.Forms.Timer(this.components);
			this.panel_Buttons.SuspendLayout();
			this.SuspendLayout();
			// 
			// btn_Cancel
			// 
			this.btn_Cancel.BorderWidth = 2;
			this.btn_Cancel.ButtonClicked = false;
			this.btn_Cancel.ClickedEmphasizeTextColor = System.Drawing.Color.White;
			this.btn_Cancel.CustomClickedGradientFirstColor = System.Drawing.Color.White;
			this.btn_Cancel.CustomClickedGradientSecondColor = System.Drawing.Color.White;
			this.btn_Cancel.Description = "";
			this.btn_Cancel.DisabledColor = System.Drawing.Color.DarkGray;
			this.btn_Cancel.EdgeRadius = 5;
			this.btn_Cancel.GradientAngle = 70F;
			this.btn_Cancel.GradientFirstColor = System.Drawing.Color.White;
			this.btn_Cancel.GradientSecondColor = System.Drawing.Color.LightSlateGray;
			this.btn_Cancel.HoverEmphasizeCustomColor = System.Drawing.Color.White;
			this.btn_Cancel.ImagePosition = new System.Drawing.Point(7, 7);
			this.btn_Cancel.ImageSize = new System.Drawing.Point(30, 30);
			this.btn_Cancel.LoadImage = global::FrameOfSystem3.Properties.Resources.CONFIG_ADD3;
			this.btn_Cancel.Location = new System.Drawing.Point(10, 127);
			this.btn_Cancel.MainFont = new System.Drawing.Font("맑은 고딕", 15F, System.Drawing.FontStyle.Bold);
			this.btn_Cancel.MainFontColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(36)))), ((int)(((byte)(0)))));
			this.btn_Cancel.Margin = new System.Windows.Forms.Padding(1, 1, 0, 0);
			this.btn_Cancel.Name = "btn_Cancel";
			this.btn_Cancel.Size = new System.Drawing.Size(102, 50);
			this.btn_Cancel.SubFont = new System.Drawing.Font("맑은 고딕", 8F, System.Drawing.FontStyle.Bold);
			this.btn_Cancel.SubFontColor = System.Drawing.Color.DarkBlue;
			this.btn_Cancel.SubText = "STATUS";
			this.btn_Cancel.TabIndex = 1;
			this.btn_Cancel.Text = "CANCEL";
			this.btn_Cancel.TextAlignMain = Sys3Controls.EN_TEXTALIGN.MIDDLE_CENTER;
			this.btn_Cancel.TextAlignSub = Sys3Controls.EN_TEXTALIGN.TOP_LEFT;
			this.btn_Cancel.ThemeIndex = 0;
			this.btn_Cancel.UseBorder = true;
			this.btn_Cancel.UseClickedEmphasizeTextColor = false;
			this.btn_Cancel.UseCustomizeClickedColor = false;
			this.btn_Cancel.UseEdge = true;
			this.btn_Cancel.UseHoverEmphasizeCustomColor = false;
			this.btn_Cancel.UseImage = false;
			this.btn_Cancel.UserHoverEmpahsize = false;
			this.btn_Cancel.UseSubFont = false;
			this.btn_Cancel.Click += new System.EventHandler(this.Click_Cancel);
			// 
			// btn_Apply
			// 
			this.btn_Apply.BorderWidth = 2;
			this.btn_Apply.ButtonClicked = false;
			this.btn_Apply.ClickedEmphasizeTextColor = System.Drawing.Color.White;
			this.btn_Apply.CustomClickedGradientFirstColor = System.Drawing.Color.White;
			this.btn_Apply.CustomClickedGradientSecondColor = System.Drawing.Color.White;
			this.btn_Apply.Description = "";
			this.btn_Apply.DisabledColor = System.Drawing.Color.DarkGray;
			this.btn_Apply.EdgeRadius = 5;
			this.btn_Apply.GradientAngle = 70F;
			this.btn_Apply.GradientFirstColor = System.Drawing.Color.White;
			this.btn_Apply.GradientSecondColor = System.Drawing.Color.LightSlateGray;
			this.btn_Apply.HoverEmphasizeCustomColor = System.Drawing.Color.White;
			this.btn_Apply.ImagePosition = new System.Drawing.Point(7, 7);
			this.btn_Apply.ImageSize = new System.Drawing.Point(30, 30);
			this.btn_Apply.LoadImage = global::FrameOfSystem3.Properties.Resources.CONFIG_ADD3;
			this.btn_Apply.Location = new System.Drawing.Point(10, 77);
			this.btn_Apply.MainFont = new System.Drawing.Font("맑은 고딕", 15F, System.Drawing.FontStyle.Bold);
			this.btn_Apply.MainFontColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(36)))), ((int)(((byte)(0)))));
			this.btn_Apply.Margin = new System.Windows.Forms.Padding(1, 1, 0, 0);
			this.btn_Apply.Name = "btn_Apply";
			this.btn_Apply.Size = new System.Drawing.Size(102, 50);
			this.btn_Apply.SubFont = new System.Drawing.Font("맑은 고딕", 8F, System.Drawing.FontStyle.Bold);
			this.btn_Apply.SubFontColor = System.Drawing.Color.DarkBlue;
			this.btn_Apply.SubText = "STATUS";
			this.btn_Apply.TabIndex = 0;
			this.btn_Apply.Text = "APPLY";
			this.btn_Apply.TextAlignMain = Sys3Controls.EN_TEXTALIGN.MIDDLE_CENTER;
			this.btn_Apply.TextAlignSub = Sys3Controls.EN_TEXTALIGN.TOP_LEFT;
			this.btn_Apply.ThemeIndex = 0;
			this.btn_Apply.UseBorder = true;
			this.btn_Apply.UseClickedEmphasizeTextColor = false;
			this.btn_Apply.UseCustomizeClickedColor = false;
			this.btn_Apply.UseEdge = true;
			this.btn_Apply.UseHoverEmphasizeCustomColor = false;
			this.btn_Apply.UseImage = false;
			this.btn_Apply.UserHoverEmpahsize = false;
			this.btn_Apply.UseSubFont = false;
			this.btn_Apply.Click += new System.EventHandler(this.Click_Apply);
			// 
			// panel_Buttons
			// 
			this.panel_Buttons.BackColor = System.Drawing.Color.WhiteSmoke;
			this.panel_Buttons.Controls.Add(this.btn_Zero);
			this.panel_Buttons.Controls.Add(this.btn_Apply);
			this.panel_Buttons.Controls.Add(this.btn_Cancel);
			this.panel_Buttons.Dock = System.Windows.Forms.DockStyle.Right;
			this.panel_Buttons.Location = new System.Drawing.Point(299, 0);
			this.panel_Buttons.Name = "panel_Buttons";
			this.panel_Buttons.Size = new System.Drawing.Size(121, 180);
			this.panel_Buttons.TabIndex = 1376;
			// 
			// btn_Zero
			// 
			this.btn_Zero.BorderWidth = 2;
			this.btn_Zero.ButtonClicked = false;
			this.btn_Zero.ClickedEmphasizeTextColor = System.Drawing.Color.White;
			this.btn_Zero.CustomClickedGradientFirstColor = System.Drawing.Color.White;
			this.btn_Zero.CustomClickedGradientSecondColor = System.Drawing.Color.White;
			this.btn_Zero.Description = "";
			this.btn_Zero.DisabledColor = System.Drawing.Color.DarkGray;
			this.btn_Zero.EdgeRadius = 5;
			this.btn_Zero.GradientAngle = 70F;
			this.btn_Zero.GradientFirstColor = System.Drawing.Color.White;
			this.btn_Zero.GradientSecondColor = System.Drawing.Color.LightSlateGray;
			this.btn_Zero.HoverEmphasizeCustomColor = System.Drawing.Color.White;
			this.btn_Zero.ImagePosition = new System.Drawing.Point(7, 7);
			this.btn_Zero.ImageSize = new System.Drawing.Point(30, 30);
			this.btn_Zero.LoadImage = global::FrameOfSystem3.Properties.Resources.CONFIG_ADD3;
			this.btn_Zero.Location = new System.Drawing.Point(10, 2);
			this.btn_Zero.MainFont = new System.Drawing.Font("맑은 고딕", 15F, System.Drawing.FontStyle.Bold);
			this.btn_Zero.MainFontColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(36)))), ((int)(((byte)(0)))));
			this.btn_Zero.Margin = new System.Windows.Forms.Padding(1, 1, 0, 0);
			this.btn_Zero.Name = "btn_Zero";
			this.btn_Zero.Size = new System.Drawing.Size(102, 35);
			this.btn_Zero.SubFont = new System.Drawing.Font("맑은 고딕", 8F, System.Drawing.FontStyle.Bold);
			this.btn_Zero.SubFontColor = System.Drawing.Color.DarkBlue;
			this.btn_Zero.SubText = "STATUS";
			this.btn_Zero.TabIndex = 0;
			this.btn_Zero.Text = "ZERO";
			this.btn_Zero.TextAlignMain = Sys3Controls.EN_TEXTALIGN.MIDDLE_CENTER;
			this.btn_Zero.TextAlignSub = Sys3Controls.EN_TEXTALIGN.TOP_LEFT;
			this.btn_Zero.ThemeIndex = 0;
			this.btn_Zero.UseBorder = true;
			this.btn_Zero.UseClickedEmphasizeTextColor = false;
			this.btn_Zero.UseCustomizeClickedColor = false;
			this.btn_Zero.UseEdge = true;
			this.btn_Zero.UseHoverEmphasizeCustomColor = false;
			this.btn_Zero.UseImage = false;
			this.btn_Zero.UserHoverEmpahsize = false;
			this.btn_Zero.UseSubFont = false;
			this.btn_Zero.Click += new System.EventHandler(this.Click_Zero);
			// 
			// panel_Dials
			// 
			this.panel_Dials.AutoSize = true;
			this.panel_Dials.BackColor = System.Drawing.Color.WhiteSmoke;
			this.panel_Dials.Dock = System.Windows.Forms.DockStyle.Fill;
			this.panel_Dials.Location = new System.Drawing.Point(0, 0);
			this.panel_Dials.Name = "panel_Dials";
			this.panel_Dials.Size = new System.Drawing.Size(299, 180);
			this.panel_Dials.TabIndex = 1377;
			// 
			// Form_DateTimeSelector
			// 
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
			this.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
			this.BackColor = System.Drawing.Color.Gray;
			this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
			this.ClientSize = new System.Drawing.Size(420, 180);
			this.ControlBox = false;
			this.Controls.Add(this.panel_Dials);
			this.Controls.Add(this.panel_Buttons);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
			this.KeyPreview = true;
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.Name = "Form_DateTimeSelector";
			this.ShowIcon = false;
			this.ShowInTaskbar = false;
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
			this.Text = "DateTimeSelector";
			this.TopMost = true;
			this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Form_DateTimeSelector_FormClosing);
			this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.Form_EditNumber_KeyDown);
			this.panel_Buttons.ResumeLayout(false);
			this.ResumeLayout(false);
			this.PerformLayout();

        }

        #endregion
		private Sys3Controls.Sys3button btn_Apply;
		private Sys3Controls.Sys3button btn_Cancel;
		private System.Windows.Forms.Panel panel_Buttons;
		private System.Windows.Forms.Panel panel_Dials;
		private Sys3Controls.Sys3button btn_Zero;
		private System.Windows.Forms.Timer timer_DragMode;
	}
}
