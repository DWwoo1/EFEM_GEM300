namespace FrameOfSystem3.Views.Config
{
	partial class Config_JogManage
    {
        /// <summary> 
        /// 필수 디자이너 변수입니다.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// 사용 중인 모든 리소스를 정리합니다.
        /// </summary>
        /// <param name="disposing">관리되는 리소스를 삭제해야 하면 true이고, 그렇지 않으면 false입니다.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region 구성 요소 디자이너에서 생성한 코드

        /// <summary> 
        /// 디자이너 지원에 필요한 메서드입니다. 
        /// 이 메서드의 내용을 코드 편집기로 수정하지 마십시오.
        /// </summary>
        private void InitializeComponent()
        {
            this.m_tabMain = new Sys3Controls.Sys3button();
            this.m_panel = new System.Windows.Forms.Panel();
            this.m_panelSub = new System.Windows.Forms.Panel();
            this.m_tabReverse = new Sys3Controls.Sys3button();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.tableLayoutPanel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // m_tabMain
            // 
            this.m_tabMain.BorderWidth = 2;
            this.m_tabMain.ButtonClicked = true;
            this.m_tabMain.ClickedEmphasizeTextColor = System.Drawing.Color.White;
            this.m_tabMain.CustomClickedGradientFirstColor = System.Drawing.Color.White;
            this.m_tabMain.CustomClickedGradientSecondColor = System.Drawing.Color.White;
            this.m_tabMain.Description = "";
            this.m_tabMain.DisabledColor = System.Drawing.Color.DarkGray;
            this.m_tabMain.Dock = System.Windows.Forms.DockStyle.Fill;
            this.m_tabMain.EdgeRadius = 5;
            this.m_tabMain.GradientAngle = 70F;
            this.m_tabMain.GradientFirstColor = System.Drawing.Color.DarkBlue;
            this.m_tabMain.GradientSecondColor = System.Drawing.Color.DarkBlue;
            this.m_tabMain.HoverEmphasizeCustomColor = System.Drawing.Color.White;
            this.m_tabMain.ImagePosition = new System.Drawing.Point(7, 7);
            this.m_tabMain.ImageSize = new System.Drawing.Point(30, 30);
            this.m_tabMain.LoadImage = global::FrameOfSystem3.Properties.Resources.Home_black;
            this.m_tabMain.Location = new System.Drawing.Point(1024, 0);
            this.m_tabMain.MainFont = new System.Drawing.Font("맑은 고딕", 11F, System.Drawing.FontStyle.Bold);
            this.m_tabMain.MainFontColor = System.Drawing.Color.White;
            this.m_tabMain.Margin = new System.Windows.Forms.Padding(0);
            this.m_tabMain.Name = "m_tabMain";
            this.m_tabMain.Size = new System.Drawing.Size(132, 49);
            this.m_tabMain.SubFont = new System.Drawing.Font("맑은 고딕", 8F, System.Drawing.FontStyle.Bold);
            this.m_tabMain.SubFontColor = System.Drawing.Color.DarkBlue;
            this.m_tabMain.SubText = "STATUS";
            this.m_tabMain.TabIndex = 0;
            this.m_tabMain.Text = "MAIN";
            this.m_tabMain.TextAlignMain = Sys3Controls.EN_TEXTALIGN.MIDDLE_CENTER;
            this.m_tabMain.TextAlignSub = Sys3Controls.EN_TEXTALIGN.TOP_RIGHT;
            this.m_tabMain.ThemeIndex = 0;
            this.m_tabMain.UseBorder = false;
            this.m_tabMain.UseClickedEmphasizeTextColor = false;
            this.m_tabMain.UseCustomizeClickedColor = false;
            this.m_tabMain.UseEdge = false;
            this.m_tabMain.UseHoverEmphasizeCustomColor = false;
            this.m_tabMain.UseImage = false;
            this.m_tabMain.UserHoverEmpahsize = false;
            this.m_tabMain.UseSubFont = false;
            this.m_tabMain.Click += new System.EventHandler(this.Click_Tab);
            // 
            // m_panel
            // 
            this.tableLayoutPanel1.SetColumnSpan(this.m_panel, 9);
            this.m_panel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.m_panel.Location = new System.Drawing.Point(0, 49);
            this.m_panel.Margin = new System.Windows.Forms.Padding(0);
            this.m_panel.Name = "m_panel";
            this.m_panel.Size = new System.Drawing.Size(1156, 851);
            this.m_panel.TabIndex = 1155;
            // 
            // m_panelSub
            // 
            this.m_panelSub.Location = new System.Drawing.Point(1316, 49);
            this.m_panelSub.Name = "m_panelSub";
            this.m_panelSub.Size = new System.Drawing.Size(650, 856);
            this.m_panelSub.TabIndex = 1156;
            // 
            // m_tabReverse
            // 
            this.m_tabReverse.BorderWidth = 2;
            this.m_tabReverse.ButtonClicked = false;
            this.m_tabReverse.ClickedEmphasizeTextColor = System.Drawing.Color.White;
            this.m_tabReverse.CustomClickedGradientFirstColor = System.Drawing.Color.White;
            this.m_tabReverse.CustomClickedGradientSecondColor = System.Drawing.Color.White;
            this.m_tabReverse.Description = "";
            this.m_tabReverse.DisabledColor = System.Drawing.Color.DarkGray;
            this.m_tabReverse.EdgeRadius = 5;
            this.m_tabReverse.GradientAngle = 70F;
            this.m_tabReverse.GradientFirstColor = System.Drawing.Color.White;
            this.m_tabReverse.GradientSecondColor = System.Drawing.Color.White;
            this.m_tabReverse.HoverEmphasizeCustomColor = System.Drawing.Color.White;
            this.m_tabReverse.ImagePosition = new System.Drawing.Point(7, 7);
            this.m_tabReverse.ImageSize = new System.Drawing.Point(30, 30);
            this.m_tabReverse.LoadImage = global::FrameOfSystem3.Properties.Resources.Home_black;
            this.m_tabReverse.Location = new System.Drawing.Point(1853, 3);
            this.m_tabReverse.MainFont = new System.Drawing.Font("맑은 고딕", 11F, System.Drawing.FontStyle.Bold);
            this.m_tabReverse.MainFontColor = System.Drawing.Color.DarkBlue;
            this.m_tabReverse.Name = "m_tabReverse";
            this.m_tabReverse.Size = new System.Drawing.Size(113, 45);
            this.m_tabReverse.SubFont = new System.Drawing.Font("맑은 고딕", 8F, System.Drawing.FontStyle.Bold);
            this.m_tabReverse.SubFontColor = System.Drawing.Color.DarkBlue;
            this.m_tabReverse.SubText = "STATUS";
            this.m_tabReverse.TabIndex = 1;
            this.m_tabReverse.Text = "REVERSE";
            this.m_tabReverse.TextAlignMain = Sys3Controls.EN_TEXTALIGN.MIDDLE_CENTER;
            this.m_tabReverse.TextAlignSub = Sys3Controls.EN_TEXTALIGN.TOP_RIGHT;
            this.m_tabReverse.ThemeIndex = 0;
            this.m_tabReverse.UseBorder = false;
            this.m_tabReverse.UseClickedEmphasizeTextColor = false;
            this.m_tabReverse.UseCustomizeClickedColor = false;
            this.m_tabReverse.UseEdge = false;
            this.m_tabReverse.UseHoverEmphasizeCustomColor = false;
            this.m_tabReverse.UseImage = false;
            this.m_tabReverse.UserHoverEmpahsize = false;
            this.m_tabReverse.UseSubFont = false;
            this.m_tabReverse.Click += new System.EventHandler(this.Click_Tab);
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 9;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 11.11111F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 11.11111F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 11.11111F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 11.11111F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 11.11111F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 11.11111F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 11.11111F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 11.11111F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 11.11111F));
            this.tableLayoutPanel1.Controls.Add(this.m_tabMain, 8, 0);
            this.tableLayoutPanel1.Controls.Add(this.m_panel, 0, 1);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 2;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 5.444445F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 94.55556F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(1156, 900);
            this.tableLayoutPanel1.TabIndex = 1156;
            // 
            // Config_JogManage
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.Controls.Add(this.tableLayoutPanel1);
            this.Controls.Add(this.m_tabReverse);
            this.Controls.Add(this.m_panelSub);
            this.Margin = new System.Windows.Forms.Padding(0);
            this.Name = "Config_JogManage";
            this.Size = new System.Drawing.Size(1156, 900);
            this.tableLayoutPanel1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

		private Sys3Controls.Sys3button m_tabMain;
		private System.Windows.Forms.Panel m_panel;
        private Sys3Controls.Sys3button m_tabReverse;
		private System.Windows.Forms.Panel m_panelSub;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;

	}
}
