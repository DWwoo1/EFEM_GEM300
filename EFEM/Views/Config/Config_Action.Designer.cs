namespace FrameOfSystem3.Views.Config
{
    partial class Config_Action
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
            this.m_tabLink = new Sys3Controls.Sys3button();
            this.m_tabPort = new Sys3Controls.Sys3button();
            this.m_panel = new System.Windows.Forms.Panel();
            this.m_tabFlow = new Sys3Controls.Sys3button();
            this._tableLayoutPanel_Main = new System.Windows.Forms.TableLayoutPanel();
            this._tableLayoutPanel_Main.SuspendLayout();
            this.SuspendLayout();
            // 
            // m_tabLink
            // 
            this.m_tabLink.BorderWidth = 2;
            this.m_tabLink.ButtonClicked = true;
            this.m_tabLink.ClickedEmphasizeTextColor = System.Drawing.Color.White;
            this.m_tabLink.CustomClickedGradientFirstColor = System.Drawing.Color.White;
            this.m_tabLink.CustomClickedGradientSecondColor = System.Drawing.Color.White;
            this.m_tabLink.Description = "";
            this.m_tabLink.DisabledColor = System.Drawing.Color.DarkGray;
            this.m_tabLink.Dock = System.Windows.Forms.DockStyle.Fill;
            this.m_tabLink.EdgeRadius = 5;
            this.m_tabLink.GradientAngle = 70F;
            this.m_tabLink.GradientFirstColor = System.Drawing.Color.DarkBlue;
            this.m_tabLink.GradientSecondColor = System.Drawing.Color.DarkBlue;
            this.m_tabLink.HoverEmphasizeCustomColor = System.Drawing.Color.White;
            this.m_tabLink.ImagePosition = new System.Drawing.Point(7, 7);
            this.m_tabLink.ImageSize = new System.Drawing.Point(30, 30);
            this.m_tabLink.LoadImage = global::FrameOfSystem3.Properties.Resources.Home_black;
            this.m_tabLink.Location = new System.Drawing.Point(768, 0);
            this.m_tabLink.MainFont = new System.Drawing.Font("맑은 고딕", 11F, System.Drawing.FontStyle.Bold);
            this.m_tabLink.MainFontColor = System.Drawing.Color.White;
            this.m_tabLink.Margin = new System.Windows.Forms.Padding(0);
            this.m_tabLink.Name = "m_tabLink";
            this.m_tabLink.Size = new System.Drawing.Size(128, 49);
            this.m_tabLink.SubFont = new System.Drawing.Font("맑은 고딕", 8F, System.Drawing.FontStyle.Bold);
            this.m_tabLink.SubFontColor = System.Drawing.Color.DarkBlue;
            this.m_tabLink.SubText = "STATUS";
            this.m_tabLink.TabIndex = 0;
            this.m_tabLink.Text = "LINK";
            this.m_tabLink.TextAlignMain = Sys3Controls.EN_TEXTALIGN.MIDDLE_CENTER;
            this.m_tabLink.TextAlignSub = Sys3Controls.EN_TEXTALIGN.TOP_RIGHT;
            this.m_tabLink.ThemeIndex = 0;
            this.m_tabLink.UseBorder = false;
            this.m_tabLink.UseClickedEmphasizeTextColor = false;
            this.m_tabLink.UseCustomizeClickedColor = false;
            this.m_tabLink.UseEdge = false;
            this.m_tabLink.UseHoverEmphasizeCustomColor = false;
            this.m_tabLink.UseImage = false;
            this.m_tabLink.UserHoverEmpahsize = false;
            this.m_tabLink.UseSubFont = false;
            this.m_tabLink.Click += new System.EventHandler(this.Click_Tab);
            // 
            // m_tabPort
            // 
            this.m_tabPort.BorderWidth = 2;
            this.m_tabPort.ButtonClicked = false;
            this.m_tabPort.ClickedEmphasizeTextColor = System.Drawing.Color.White;
            this.m_tabPort.CustomClickedGradientFirstColor = System.Drawing.Color.White;
            this.m_tabPort.CustomClickedGradientSecondColor = System.Drawing.Color.White;
            this.m_tabPort.Description = "";
            this.m_tabPort.DisabledColor = System.Drawing.Color.DarkGray;
            this.m_tabPort.Dock = System.Windows.Forms.DockStyle.Fill;
            this.m_tabPort.EdgeRadius = 5;
            this.m_tabPort.GradientAngle = 70F;
            this.m_tabPort.GradientFirstColor = System.Drawing.Color.White;
            this.m_tabPort.GradientSecondColor = System.Drawing.Color.White;
            this.m_tabPort.HoverEmphasizeCustomColor = System.Drawing.Color.White;
            this.m_tabPort.ImagePosition = new System.Drawing.Point(7, 7);
            this.m_tabPort.ImageSize = new System.Drawing.Point(30, 30);
            this.m_tabPort.LoadImage = global::FrameOfSystem3.Properties.Resources.Home_black;
            this.m_tabPort.Location = new System.Drawing.Point(1024, 0);
            this.m_tabPort.MainFont = new System.Drawing.Font("맑은 고딕", 11F, System.Drawing.FontStyle.Bold);
            this.m_tabPort.MainFontColor = System.Drawing.Color.DarkBlue;
            this.m_tabPort.Margin = new System.Windows.Forms.Padding(0);
            this.m_tabPort.Name = "m_tabPort";
            this.m_tabPort.Size = new System.Drawing.Size(132, 49);
            this.m_tabPort.SubFont = new System.Drawing.Font("맑은 고딕", 8F, System.Drawing.FontStyle.Bold);
            this.m_tabPort.SubFontColor = System.Drawing.Color.DarkBlue;
            this.m_tabPort.SubText = "Port";
            this.m_tabPort.TabIndex = 2;
            this.m_tabPort.Text = "PORT";
            this.m_tabPort.TextAlignMain = Sys3Controls.EN_TEXTALIGN.MIDDLE_CENTER;
            this.m_tabPort.TextAlignSub = Sys3Controls.EN_TEXTALIGN.TOP_RIGHT;
            this.m_tabPort.ThemeIndex = 0;
            this.m_tabPort.UseBorder = false;
            this.m_tabPort.UseClickedEmphasizeTextColor = false;
            this.m_tabPort.UseCustomizeClickedColor = false;
            this.m_tabPort.UseEdge = false;
            this.m_tabPort.UseHoverEmphasizeCustomColor = false;
            this.m_tabPort.UseImage = false;
            this.m_tabPort.UserHoverEmpahsize = false;
            this.m_tabPort.UseSubFont = false;
            this.m_tabPort.Click += new System.EventHandler(this.Click_Tab);
            // 
            // m_panel
            // 
            this._tableLayoutPanel_Main.SetColumnSpan(this.m_panel, 9);
            this.m_panel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.m_panel.Location = new System.Drawing.Point(0, 49);
            this.m_panel.Margin = new System.Windows.Forms.Padding(0);
            this.m_panel.Name = "m_panel";
            this.m_panel.Size = new System.Drawing.Size(1156, 851);
            this.m_panel.TabIndex = 1155;
            // 
            // m_tabFlow
            // 
            this.m_tabFlow.BorderWidth = 2;
            this.m_tabFlow.ButtonClicked = false;
            this.m_tabFlow.ClickedEmphasizeTextColor = System.Drawing.Color.White;
            this.m_tabFlow.CustomClickedGradientFirstColor = System.Drawing.Color.White;
            this.m_tabFlow.CustomClickedGradientSecondColor = System.Drawing.Color.White;
            this.m_tabFlow.Description = "";
            this.m_tabFlow.DisabledColor = System.Drawing.Color.DarkGray;
            this.m_tabFlow.Dock = System.Windows.Forms.DockStyle.Fill;
            this.m_tabFlow.EdgeRadius = 5;
            this.m_tabFlow.GradientAngle = 70F;
            this.m_tabFlow.GradientFirstColor = System.Drawing.Color.White;
            this.m_tabFlow.GradientSecondColor = System.Drawing.Color.White;
            this.m_tabFlow.HoverEmphasizeCustomColor = System.Drawing.Color.White;
            this.m_tabFlow.ImagePosition = new System.Drawing.Point(7, 7);
            this.m_tabFlow.ImageSize = new System.Drawing.Point(30, 30);
            this.m_tabFlow.LoadImage = global::FrameOfSystem3.Properties.Resources.Home_black;
            this.m_tabFlow.Location = new System.Drawing.Point(896, 0);
            this.m_tabFlow.MainFont = new System.Drawing.Font("맑은 고딕", 11F, System.Drawing.FontStyle.Bold);
            this.m_tabFlow.MainFontColor = System.Drawing.Color.DarkBlue;
            this.m_tabFlow.Margin = new System.Windows.Forms.Padding(0);
            this.m_tabFlow.Name = "m_tabFlow";
            this.m_tabFlow.Size = new System.Drawing.Size(128, 49);
            this.m_tabFlow.SubFont = new System.Drawing.Font("맑은 고딕", 8F, System.Drawing.FontStyle.Bold);
            this.m_tabFlow.SubFontColor = System.Drawing.Color.DarkBlue;
            this.m_tabFlow.SubText = "STATUS";
            this.m_tabFlow.TabIndex = 1;
            this.m_tabFlow.Text = "FLOW";
            this.m_tabFlow.TextAlignMain = Sys3Controls.EN_TEXTALIGN.MIDDLE_CENTER;
            this.m_tabFlow.TextAlignSub = Sys3Controls.EN_TEXTALIGN.TOP_RIGHT;
            this.m_tabFlow.ThemeIndex = 0;
            this.m_tabFlow.UseBorder = false;
            this.m_tabFlow.UseClickedEmphasizeTextColor = false;
            this.m_tabFlow.UseCustomizeClickedColor = false;
            this.m_tabFlow.UseEdge = false;
            this.m_tabFlow.UseHoverEmphasizeCustomColor = false;
            this.m_tabFlow.UseImage = false;
            this.m_tabFlow.UserHoverEmpahsize = false;
            this.m_tabFlow.UseSubFont = false;
            this.m_tabFlow.Click += new System.EventHandler(this.Click_Tab);
            // 
            // _tableLayoutPanel_Main
            // 
            this._tableLayoutPanel_Main.ColumnCount = 9;
            this._tableLayoutPanel_Main.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 11.11111F));
            this._tableLayoutPanel_Main.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 11.11111F));
            this._tableLayoutPanel_Main.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 11.11111F));
            this._tableLayoutPanel_Main.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 11.11111F));
            this._tableLayoutPanel_Main.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 11.11111F));
            this._tableLayoutPanel_Main.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 11.11111F));
            this._tableLayoutPanel_Main.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 11.11111F));
            this._tableLayoutPanel_Main.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 11.11111F));
            this._tableLayoutPanel_Main.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 11.11111F));
            this._tableLayoutPanel_Main.Controls.Add(this.m_tabLink, 6, 0);
            this._tableLayoutPanel_Main.Controls.Add(this.m_panel, 0, 1);
            this._tableLayoutPanel_Main.Controls.Add(this.m_tabFlow, 7, 0);
            this._tableLayoutPanel_Main.Controls.Add(this.m_tabPort, 8, 0);
            this._tableLayoutPanel_Main.Dock = System.Windows.Forms.DockStyle.Fill;
            this._tableLayoutPanel_Main.Location = new System.Drawing.Point(0, 0);
            this._tableLayoutPanel_Main.Margin = new System.Windows.Forms.Padding(0);
            this._tableLayoutPanel_Main.Name = "_tableLayoutPanel_Main";
            this._tableLayoutPanel_Main.RowCount = 2;
            this._tableLayoutPanel_Main.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 5.475169F));
            this._tableLayoutPanel_Main.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 94.52483F));
            this._tableLayoutPanel_Main.Size = new System.Drawing.Size(1156, 900);
            this._tableLayoutPanel_Main.TabIndex = 1156;
            // 
            // Config_Action
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.Controls.Add(this._tableLayoutPanel_Main);
            this.Margin = new System.Windows.Forms.Padding(0);
            this.Name = "Config_Action";
            this.Size = new System.Drawing.Size(1156, 900);
            this._tableLayoutPanel_Main.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

		private Sys3Controls.Sys3button m_tabLink;
		private Sys3Controls.Sys3button m_tabPort;
		private System.Windows.Forms.Panel m_panel;
        private Sys3Controls.Sys3button m_tabFlow;
        private System.Windows.Forms.TableLayoutPanel _tableLayoutPanel_Main;

	}
}
