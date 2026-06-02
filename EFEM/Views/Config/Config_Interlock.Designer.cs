namespace FrameOfSystem3.Views.Config
{
    partial class Config_Interlock
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
            this.m_tabMotion = new Sys3Controls.Sys3button();
            this.m_tabDO = new Sys3Controls.Sys3button();
            this.m_panel = new System.Windows.Forms.Panel();
            this.m_tabCylinder = new Sys3Controls.Sys3button();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.tableLayoutPanel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // m_tabMotion
            // 
            this.m_tabMotion.BorderWidth = 2;
            this.m_tabMotion.ButtonClicked = true;
            this.m_tabMotion.ClickedEmphasizeTextColor = System.Drawing.Color.White;
            this.m_tabMotion.CustomClickedGradientFirstColor = System.Drawing.Color.White;
            this.m_tabMotion.CustomClickedGradientSecondColor = System.Drawing.Color.White;
            this.m_tabMotion.Description = "";
            this.m_tabMotion.DisabledColor = System.Drawing.Color.DarkGray;
            this.m_tabMotion.Dock = System.Windows.Forms.DockStyle.Fill;
            this.m_tabMotion.EdgeRadius = 5;
            this.m_tabMotion.GradientAngle = 70F;
            this.m_tabMotion.GradientFirstColor = System.Drawing.Color.DarkBlue;
            this.m_tabMotion.GradientSecondColor = System.Drawing.Color.DarkBlue;
            this.m_tabMotion.HoverEmphasizeCustomColor = System.Drawing.Color.White;
            this.m_tabMotion.ImagePosition = new System.Drawing.Point(7, 7);
            this.m_tabMotion.ImageSize = new System.Drawing.Point(30, 30);
            this.m_tabMotion.LoadImage = global::FrameOfSystem3.Properties.Resources.Home_black;
            this.m_tabMotion.Location = new System.Drawing.Point(768, 0);
            this.m_tabMotion.MainFont = new System.Drawing.Font("맑은 고딕", 11F, System.Drawing.FontStyle.Bold);
            this.m_tabMotion.MainFontColor = System.Drawing.Color.White;
            this.m_tabMotion.Margin = new System.Windows.Forms.Padding(0);
            this.m_tabMotion.Name = "m_tabMotion";
            this.m_tabMotion.Size = new System.Drawing.Size(128, 49);
            this.m_tabMotion.SubFont = new System.Drawing.Font("맑은 고딕", 8F, System.Drawing.FontStyle.Bold);
            this.m_tabMotion.SubFontColor = System.Drawing.Color.DarkBlue;
            this.m_tabMotion.SubText = "STATUS";
            this.m_tabMotion.TabIndex = 0;
            this.m_tabMotion.Text = "MOTION";
            this.m_tabMotion.TextAlignMain = Sys3Controls.EN_TEXTALIGN.MIDDLE_CENTER;
            this.m_tabMotion.TextAlignSub = Sys3Controls.EN_TEXTALIGN.TOP_RIGHT;
            this.m_tabMotion.ThemeIndex = 0;
            this.m_tabMotion.UseBorder = false;
            this.m_tabMotion.UseClickedEmphasizeTextColor = false;
            this.m_tabMotion.UseCustomizeClickedColor = false;
            this.m_tabMotion.UseEdge = false;
            this.m_tabMotion.UseHoverEmphasizeCustomColor = false;
            this.m_tabMotion.UseImage = false;
            this.m_tabMotion.UserHoverEmpahsize = false;
            this.m_tabMotion.UseSubFont = false;
            this.m_tabMotion.Click += new System.EventHandler(this.Click_Tab);
            // 
            // m_tabDO
            // 
            this.m_tabDO.BorderWidth = 2;
            this.m_tabDO.ButtonClicked = false;
            this.m_tabDO.ClickedEmphasizeTextColor = System.Drawing.Color.White;
            this.m_tabDO.CustomClickedGradientFirstColor = System.Drawing.Color.White;
            this.m_tabDO.CustomClickedGradientSecondColor = System.Drawing.Color.White;
            this.m_tabDO.Description = "";
            this.m_tabDO.DisabledColor = System.Drawing.Color.DarkGray;
            this.m_tabDO.Dock = System.Windows.Forms.DockStyle.Fill;
            this.m_tabDO.EdgeRadius = 5;
            this.m_tabDO.GradientAngle = 70F;
            this.m_tabDO.GradientFirstColor = System.Drawing.Color.White;
            this.m_tabDO.GradientSecondColor = System.Drawing.Color.White;
            this.m_tabDO.HoverEmphasizeCustomColor = System.Drawing.Color.White;
            this.m_tabDO.ImagePosition = new System.Drawing.Point(7, 7);
            this.m_tabDO.ImageSize = new System.Drawing.Point(30, 30);
            this.m_tabDO.LoadImage = global::FrameOfSystem3.Properties.Resources.Home_black;
            this.m_tabDO.Location = new System.Drawing.Point(1024, 0);
            this.m_tabDO.MainFont = new System.Drawing.Font("맑은 고딕", 11F, System.Drawing.FontStyle.Bold);
            this.m_tabDO.MainFontColor = System.Drawing.Color.DarkBlue;
            this.m_tabDO.Margin = new System.Windows.Forms.Padding(0);
            this.m_tabDO.Name = "m_tabDO";
            this.m_tabDO.Size = new System.Drawing.Size(132, 49);
            this.m_tabDO.SubFont = new System.Drawing.Font("맑은 고딕", 8F, System.Drawing.FontStyle.Bold);
            this.m_tabDO.SubFontColor = System.Drawing.Color.DarkBlue;
            this.m_tabDO.SubText = "Port";
            this.m_tabDO.TabIndex = 2;
            this.m_tabDO.Text = "DIGITAL OUT";
            this.m_tabDO.TextAlignMain = Sys3Controls.EN_TEXTALIGN.MIDDLE_CENTER;
            this.m_tabDO.TextAlignSub = Sys3Controls.EN_TEXTALIGN.TOP_RIGHT;
            this.m_tabDO.ThemeIndex = 0;
            this.m_tabDO.UseBorder = false;
            this.m_tabDO.UseClickedEmphasizeTextColor = false;
            this.m_tabDO.UseCustomizeClickedColor = false;
            this.m_tabDO.UseEdge = false;
            this.m_tabDO.UseHoverEmphasizeCustomColor = false;
            this.m_tabDO.UseImage = false;
            this.m_tabDO.UserHoverEmpahsize = false;
            this.m_tabDO.UseSubFont = false;
            this.m_tabDO.Click += new System.EventHandler(this.Click_Tab);
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
            // m_tabCylinder
            // 
            this.m_tabCylinder.BorderWidth = 2;
            this.m_tabCylinder.ButtonClicked = false;
            this.m_tabCylinder.ClickedEmphasizeTextColor = System.Drawing.Color.White;
            this.m_tabCylinder.CustomClickedGradientFirstColor = System.Drawing.Color.White;
            this.m_tabCylinder.CustomClickedGradientSecondColor = System.Drawing.Color.White;
            this.m_tabCylinder.Description = "";
            this.m_tabCylinder.DisabledColor = System.Drawing.Color.DarkGray;
            this.m_tabCylinder.Dock = System.Windows.Forms.DockStyle.Fill;
            this.m_tabCylinder.EdgeRadius = 5;
            this.m_tabCylinder.GradientAngle = 70F;
            this.m_tabCylinder.GradientFirstColor = System.Drawing.Color.White;
            this.m_tabCylinder.GradientSecondColor = System.Drawing.Color.White;
            this.m_tabCylinder.HoverEmphasizeCustomColor = System.Drawing.Color.White;
            this.m_tabCylinder.ImagePosition = new System.Drawing.Point(7, 7);
            this.m_tabCylinder.ImageSize = new System.Drawing.Point(30, 30);
            this.m_tabCylinder.LoadImage = global::FrameOfSystem3.Properties.Resources.Home_black;
            this.m_tabCylinder.Location = new System.Drawing.Point(896, 0);
            this.m_tabCylinder.MainFont = new System.Drawing.Font("맑은 고딕", 11F, System.Drawing.FontStyle.Bold);
            this.m_tabCylinder.MainFontColor = System.Drawing.Color.DarkBlue;
            this.m_tabCylinder.Margin = new System.Windows.Forms.Padding(0);
            this.m_tabCylinder.Name = "m_tabCylinder";
            this.m_tabCylinder.Size = new System.Drawing.Size(128, 49);
            this.m_tabCylinder.SubFont = new System.Drawing.Font("맑은 고딕", 8F, System.Drawing.FontStyle.Bold);
            this.m_tabCylinder.SubFontColor = System.Drawing.Color.DarkBlue;
            this.m_tabCylinder.SubText = "STATUS";
            this.m_tabCylinder.TabIndex = 1;
            this.m_tabCylinder.Text = "CYLINDER";
            this.m_tabCylinder.TextAlignMain = Sys3Controls.EN_TEXTALIGN.MIDDLE_CENTER;
            this.m_tabCylinder.TextAlignSub = Sys3Controls.EN_TEXTALIGN.TOP_RIGHT;
            this.m_tabCylinder.ThemeIndex = 0;
            this.m_tabCylinder.UseBorder = false;
            this.m_tabCylinder.UseClickedEmphasizeTextColor = false;
            this.m_tabCylinder.UseCustomizeClickedColor = false;
            this.m_tabCylinder.UseEdge = false;
            this.m_tabCylinder.UseHoverEmphasizeCustomColor = false;
            this.m_tabCylinder.UseImage = false;
            this.m_tabCylinder.UserHoverEmpahsize = false;
            this.m_tabCylinder.UseSubFont = false;
            this.m_tabCylinder.Click += new System.EventHandler(this.Click_Tab);
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
            this.tableLayoutPanel1.Controls.Add(this.m_tabMotion, 6, 0);
            this.tableLayoutPanel1.Controls.Add(this.m_panel, 0, 1);
            this.tableLayoutPanel1.Controls.Add(this.m_tabCylinder, 7, 0);
            this.tableLayoutPanel1.Controls.Add(this.m_tabDO, 8, 0);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel1.Margin = new System.Windows.Forms.Padding(0);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 2;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 5.555555F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 94.44444F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(1156, 900);
            this.tableLayoutPanel1.TabIndex = 1156;
            // 
            // Config_Interlock
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.Controls.Add(this.tableLayoutPanel1);
            this.Margin = new System.Windows.Forms.Padding(0);
            this.Name = "Config_Interlock";
            this.Size = new System.Drawing.Size(1156, 900);
            this.tableLayoutPanel1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

		private Sys3Controls.Sys3button m_tabMotion;
		private Sys3Controls.Sys3button m_tabDO;
		private System.Windows.Forms.Panel m_panel;
        private Sys3Controls.Sys3button m_tabCylinder;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;

	}
}
