namespace FrameOfSystem3.Views.Functional
{
    partial class Form_ProgressBar
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components=null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if(disposing&&(components!=null))
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
			this.m_groupTitle = new Sys3Controls.Sys3GroupBox();
			this.m_btnClose = new Sys3Controls.Sys3button();
			this.SuspendLayout();
			// 
			// m_groupTitle
			// 
			this.m_groupTitle.BackGroundColor = System.Drawing.Color.WhiteSmoke;
			this.m_groupTitle.EdgeBorderStroke = 2;
			this.m_groupTitle.EdgeRadius = 2;
			this.m_groupTitle.LabelFont = new System.Drawing.Font("맑은 고딕", 12F, System.Drawing.FontStyle.Bold);
			this.m_groupTitle.LabelGradientColorFirst = System.Drawing.Color.FromArgb(((int)(((byte)(208)))), ((int)(((byte)(208)))), ((int)(((byte)(208)))));
			this.m_groupTitle.LabelGradientColorSecond = System.Drawing.Color.FromArgb(((int)(((byte)(240)))), ((int)(((byte)(240)))), ((int)(((byte)(240)))));
			this.m_groupTitle.LabelHeight = 50;
			this.m_groupTitle.LabelTextColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(36)))), ((int)(((byte)(0)))));
			this.m_groupTitle.Location = new System.Drawing.Point(-1, 0);
			this.m_groupTitle.Name = "m_groupTitle";
			this.m_groupTitle.Size = new System.Drawing.Size(706, 122);
			this.m_groupTitle.TabIndex = 1372;
			this.m_groupTitle.Text = "THE OPERATION IN PROGRESS ...";
			this.m_groupTitle.TextAlign = Sys3Controls.EN_TEXTALIGN.MIDDLE_CENTER;
			this.m_groupTitle.UseLabelBorder = true;
			// 
			// m_btnClose
			// 
			this.m_btnClose.BorderWidth = 3;
			this.m_btnClose.ButtonClicked = false;
			this.m_btnClose.DisabledColor = System.Drawing.Color.DarkGray;
			this.m_btnClose.EdgeRadius = 5;
			this.m_btnClose.GradientAngle = 70F;
			this.m_btnClose.GradientFirstColor = System.Drawing.Color.White;
			this.m_btnClose.GradientSecondColor = System.Drawing.Color.LightSlateGray;
			this.m_btnClose.ImagePosition = new System.Drawing.Point(10, 10);
			this.m_btnClose.ImageSize = new System.Drawing.Point(30, 30);
			this.m_btnClose.LoadImage = global::FrameOfSystem3.Properties.Resources.CLEAR_BLACK;
			this.m_btnClose.Location = new System.Drawing.Point(594, 5);
			this.m_btnClose.MainFont = new System.Drawing.Font("맑은 고딕", 11F, System.Drawing.FontStyle.Bold);
			this.m_btnClose.MainFontColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(36)))), ((int)(((byte)(0)))));
			this.m_btnClose.Name = "m_btnClose";
			this.m_btnClose.Size = new System.Drawing.Size(106, 41);
			this.m_btnClose.SubFont = new System.Drawing.Font("맑은 고딕", 8F, System.Drawing.FontStyle.Bold);
			this.m_btnClose.SubFontColor = System.Drawing.Color.DarkBlue;
			this.m_btnClose.SubText = "STATUS";
			this.m_btnClose.TabIndex = 1373;
			this.m_btnClose.Text = "CLOSE";
			this.m_btnClose.TextAlignMain = Sys3Controls.EN_TEXTALIGN.MIDDLE_CENTER;
			this.m_btnClose.TextAlignSub = Sys3Controls.EN_TEXTALIGN.TOP_LEFT;
			this.m_btnClose.UseBorder = true;
			this.m_btnClose.UseEdge = true;
			this.m_btnClose.UseImage = false;
			this.m_btnClose.UseSubFont = false;
			this.m_btnClose.Click += new System.EventHandler(this.Click_Close);
			// 
			// Form_ProgressBar
			// 
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
			this.ClientSize = new System.Drawing.Size(703, 121);
			this.ControlBox = false;
			this.Controls.Add(this.m_btnClose);
			this.Controls.Add(this.m_groupTitle);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.Name = "Form_ProgressBar";
			this.ShowIcon = false;
			this.ShowInTaskbar = false;
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
			this.Text = "Form_ProgressBar";
			this.TopMost = true;
			this.ResumeLayout(false);

        }

        #endregion

		private Sys3Controls.Sys3GroupBox m_groupTitle;
		private Sys3Controls.Sys3button m_btnClose;

	}
}