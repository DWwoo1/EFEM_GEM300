namespace FrameOfSystem3.Views.Config
{
	partial class Config_Communication
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
            this.m_tabSerial = new Sys3Controls.Sys3button();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.m_tabWCF = new Sys3Controls.Sys3button();
            this.m_panel = new System.Windows.Forms.Panel();
            this.m_tabSocket = new Sys3Controls.Sys3button();
            this.m_tabFTP = new Sys3Controls.Sys3button();
            this.tableLayoutPanel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // m_tabSerial
            // 
            this.m_tabSerial.BorderWidth = 2;
            this.m_tabSerial.ButtonClicked = false;
            this.m_tabSerial.ClickedEmphasizeTextColor = System.Drawing.Color.White;
            this.m_tabSerial.CustomClickedGradientFirstColor = System.Drawing.Color.White;
            this.m_tabSerial.CustomClickedGradientSecondColor = System.Drawing.Color.White;
            this.m_tabSerial.Description = "";
            this.m_tabSerial.DisabledColor = System.Drawing.Color.DarkGray;
            this.m_tabSerial.Dock = System.Windows.Forms.DockStyle.Fill;
            this.m_tabSerial.EdgeRadius = 5;
            this.m_tabSerial.GradientAngle = 70F;
            this.m_tabSerial.GradientFirstColor = System.Drawing.Color.White;
            this.m_tabSerial.GradientSecondColor = System.Drawing.Color.White;
            this.m_tabSerial.HoverEmphasizeCustomColor = System.Drawing.Color.White;
            this.m_tabSerial.ImagePosition = new System.Drawing.Point(7, 7);
            this.m_tabSerial.ImageSize = new System.Drawing.Point(30, 30);
            this.m_tabSerial.LoadImage = global::FrameOfSystem3.Properties.Resources.Home_black;
            this.m_tabSerial.Location = new System.Drawing.Point(1024, 0);
            this.m_tabSerial.MainFont = new System.Drawing.Font("맑은 고딕", 11F, System.Drawing.FontStyle.Bold);
            this.m_tabSerial.MainFontColor = System.Drawing.Color.DarkBlue;
            this.m_tabSerial.Margin = new System.Windows.Forms.Padding(0);
            this.m_tabSerial.Name = "m_tabSerial";
            this.m_tabSerial.Size = new System.Drawing.Size(132, 54);
            this.m_tabSerial.SubFont = new System.Drawing.Font("맑은 고딕", 8F, System.Drawing.FontStyle.Bold);
            this.m_tabSerial.SubFontColor = System.Drawing.Color.DarkBlue;
            this.m_tabSerial.SubText = "STATUS";
            this.m_tabSerial.TabIndex = 3;
            this.m_tabSerial.Text = "SERIAL";
            this.m_tabSerial.TextAlignMain = Sys3Controls.EN_TEXTALIGN.MIDDLE_CENTER;
            this.m_tabSerial.TextAlignSub = Sys3Controls.EN_TEXTALIGN.TOP_RIGHT;
            this.m_tabSerial.ThemeIndex = 0;
            this.m_tabSerial.UseBorder = false;
            this.m_tabSerial.UseClickedEmphasizeTextColor = false;
            this.m_tabSerial.UseCustomizeClickedColor = false;
            this.m_tabSerial.UseEdge = false;
            this.m_tabSerial.UseHoverEmphasizeCustomColor = false;
            this.m_tabSerial.UseImage = false;
            this.m_tabSerial.UserHoverEmpahsize = false;
            this.m_tabSerial.UseSubFont = false;
            this.m_tabSerial.Click += new System.EventHandler(this.Click_Tab);
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
            this.tableLayoutPanel1.Controls.Add(this.m_tabFTP, 5, 0);
            this.tableLayoutPanel1.Controls.Add(this.m_tabWCF, 7, 0);
            this.tableLayoutPanel1.Controls.Add(this.m_panel, 0, 1);
            this.tableLayoutPanel1.Controls.Add(this.m_tabSocket, 5, 0);
            this.tableLayoutPanel1.Controls.Add(this.m_tabSerial, 8, 0);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel1.Margin = new System.Windows.Forms.Padding(0);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 2;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 6F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 94F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(1156, 900);
            this.tableLayoutPanel1.TabIndex = 1156;
            // 
            // m_tabWCF
            // 
            this.m_tabWCF.BorderWidth = 2;
            this.m_tabWCF.ButtonClicked = false;
            this.m_tabWCF.ClickedEmphasizeTextColor = System.Drawing.Color.White;
            this.m_tabWCF.CustomClickedGradientFirstColor = System.Drawing.Color.White;
            this.m_tabWCF.CustomClickedGradientSecondColor = System.Drawing.Color.White;
            this.m_tabWCF.Description = "";
            this.m_tabWCF.DisabledColor = System.Drawing.Color.DarkGray;
            this.m_tabWCF.Dock = System.Windows.Forms.DockStyle.Fill;
            this.m_tabWCF.EdgeRadius = 5;
            this.m_tabWCF.GradientAngle = 70F;
            this.m_tabWCF.GradientFirstColor = System.Drawing.Color.White;
            this.m_tabWCF.GradientSecondColor = System.Drawing.Color.White;
            this.m_tabWCF.HoverEmphasizeCustomColor = System.Drawing.Color.White;
            this.m_tabWCF.ImagePosition = new System.Drawing.Point(7, 7);
            this.m_tabWCF.ImageSize = new System.Drawing.Point(30, 30);
            this.m_tabWCF.LoadImage = global::FrameOfSystem3.Properties.Resources.Home_black;
            this.m_tabWCF.Location = new System.Drawing.Point(896, 0);
            this.m_tabWCF.MainFont = new System.Drawing.Font("맑은 고딕", 11F, System.Drawing.FontStyle.Bold);
            this.m_tabWCF.MainFontColor = System.Drawing.Color.DarkBlue;
            this.m_tabWCF.Margin = new System.Windows.Forms.Padding(0);
            this.m_tabWCF.Name = "m_tabWCF";
            this.m_tabWCF.Size = new System.Drawing.Size(128, 54);
            this.m_tabWCF.SubFont = new System.Drawing.Font("맑은 고딕", 8F, System.Drawing.FontStyle.Bold);
            this.m_tabWCF.SubFontColor = System.Drawing.Color.DarkBlue;
            this.m_tabWCF.SubText = "STATUS";
            this.m_tabWCF.TabIndex = 2;
            this.m_tabWCF.Text = "WCF";
            this.m_tabWCF.TextAlignMain = Sys3Controls.EN_TEXTALIGN.MIDDLE_CENTER;
            this.m_tabWCF.TextAlignSub = Sys3Controls.EN_TEXTALIGN.TOP_RIGHT;
            this.m_tabWCF.ThemeIndex = 0;
            this.m_tabWCF.UseBorder = false;
            this.m_tabWCF.UseClickedEmphasizeTextColor = false;
            this.m_tabWCF.UseCustomizeClickedColor = false;
            this.m_tabWCF.UseEdge = false;
            this.m_tabWCF.UseHoverEmphasizeCustomColor = false;
            this.m_tabWCF.UseImage = false;
            this.m_tabWCF.UserHoverEmpahsize = false;
            this.m_tabWCF.UseSubFont = false;
            this.m_tabWCF.Click += new System.EventHandler(this.Click_Tab);
            // 
            // m_panel
            // 
            this.tableLayoutPanel1.SetColumnSpan(this.m_panel, 9);
            this.m_panel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.m_panel.Location = new System.Drawing.Point(0, 54);
            this.m_panel.Margin = new System.Windows.Forms.Padding(0);
            this.m_panel.Name = "m_panel";
            this.m_panel.Size = new System.Drawing.Size(1156, 846);
            this.m_panel.TabIndex = 1155;
            // 
            // m_tabSocket
            // 
            this.m_tabSocket.BorderWidth = 2;
            this.m_tabSocket.ButtonClicked = true;
            this.m_tabSocket.ClickedEmphasizeTextColor = System.Drawing.Color.White;
            this.m_tabSocket.CustomClickedGradientFirstColor = System.Drawing.Color.White;
            this.m_tabSocket.CustomClickedGradientSecondColor = System.Drawing.Color.White;
            this.m_tabSocket.Description = "";
            this.m_tabSocket.DisabledColor = System.Drawing.Color.DarkGray;
            this.m_tabSocket.Dock = System.Windows.Forms.DockStyle.Fill;
            this.m_tabSocket.EdgeRadius = 5;
            this.m_tabSocket.GradientAngle = 70F;
            this.m_tabSocket.GradientFirstColor = System.Drawing.Color.DarkBlue;
            this.m_tabSocket.GradientSecondColor = System.Drawing.Color.DarkBlue;
            this.m_tabSocket.HoverEmphasizeCustomColor = System.Drawing.Color.White;
            this.m_tabSocket.ImagePosition = new System.Drawing.Point(7, 7);
            this.m_tabSocket.ImageSize = new System.Drawing.Point(30, 30);
            this.m_tabSocket.LoadImage = global::FrameOfSystem3.Properties.Resources.Home_black;
            this.m_tabSocket.Location = new System.Drawing.Point(640, 0);
            this.m_tabSocket.MainFont = new System.Drawing.Font("맑은 고딕", 11F, System.Drawing.FontStyle.Bold);
            this.m_tabSocket.MainFontColor = System.Drawing.Color.White;
            this.m_tabSocket.Margin = new System.Windows.Forms.Padding(0);
            this.m_tabSocket.Name = "m_tabSocket";
            this.m_tabSocket.Size = new System.Drawing.Size(128, 54);
            this.m_tabSocket.SubFont = new System.Drawing.Font("맑은 고딕", 8F, System.Drawing.FontStyle.Bold);
            this.m_tabSocket.SubFontColor = System.Drawing.Color.DarkBlue;
            this.m_tabSocket.SubText = "STATUS";
            this.m_tabSocket.TabIndex = 2;
            this.m_tabSocket.Text = "SOCKET";
            this.m_tabSocket.TextAlignMain = Sys3Controls.EN_TEXTALIGN.MIDDLE_CENTER;
            this.m_tabSocket.TextAlignSub = Sys3Controls.EN_TEXTALIGN.TOP_RIGHT;
            this.m_tabSocket.ThemeIndex = 0;
            this.m_tabSocket.UseBorder = false;
            this.m_tabSocket.UseClickedEmphasizeTextColor = false;
            this.m_tabSocket.UseCustomizeClickedColor = false;
            this.m_tabSocket.UseEdge = false;
            this.m_tabSocket.UseHoverEmphasizeCustomColor = false;
            this.m_tabSocket.UseImage = false;
            this.m_tabSocket.UserHoverEmpahsize = false;
            this.m_tabSocket.UseSubFont = false;
            this.m_tabSocket.Click += new System.EventHandler(this.Click_Tab);
            // 
            // m_tabFTP
            // 
            this.m_tabFTP.BorderWidth = 2;
            this.m_tabFTP.ButtonClicked = false;
            this.m_tabFTP.ClickedEmphasizeTextColor = System.Drawing.Color.White;
            this.m_tabFTP.CustomClickedGradientFirstColor = System.Drawing.Color.White;
            this.m_tabFTP.CustomClickedGradientSecondColor = System.Drawing.Color.White;
            this.m_tabFTP.Description = "";
            this.m_tabFTP.DisabledColor = System.Drawing.Color.DarkGray;
            this.m_tabFTP.Dock = System.Windows.Forms.DockStyle.Fill;
            this.m_tabFTP.EdgeRadius = 5;
            this.m_tabFTP.GradientAngle = 70F;
            this.m_tabFTP.GradientFirstColor = System.Drawing.Color.White;
            this.m_tabFTP.GradientSecondColor = System.Drawing.Color.White;
            this.m_tabFTP.HoverEmphasizeCustomColor = System.Drawing.Color.White;
            this.m_tabFTP.ImagePosition = new System.Drawing.Point(7, 7);
            this.m_tabFTP.ImageSize = new System.Drawing.Point(30, 30);
            this.m_tabFTP.LoadImage = global::FrameOfSystem3.Properties.Resources.Home_black;
            this.m_tabFTP.Location = new System.Drawing.Point(768, 0);
            this.m_tabFTP.MainFont = new System.Drawing.Font("맑은 고딕", 11F, System.Drawing.FontStyle.Bold);
            this.m_tabFTP.MainFontColor = System.Drawing.Color.DarkBlue;
            this.m_tabFTP.Margin = new System.Windows.Forms.Padding(0);
            this.m_tabFTP.Name = "m_tabFTP";
            this.m_tabFTP.Size = new System.Drawing.Size(128, 54);
            this.m_tabFTP.SubFont = new System.Drawing.Font("맑은 고딕", 8F, System.Drawing.FontStyle.Bold);
            this.m_tabFTP.SubFontColor = System.Drawing.Color.DarkBlue;
            this.m_tabFTP.SubText = "STATUS";
            this.m_tabFTP.TabIndex = 1156;
            this.m_tabFTP.Text = "FTP";
            this.m_tabFTP.TextAlignMain = Sys3Controls.EN_TEXTALIGN.MIDDLE_CENTER;
            this.m_tabFTP.TextAlignSub = Sys3Controls.EN_TEXTALIGN.TOP_RIGHT;
            this.m_tabFTP.ThemeIndex = 0;
            this.m_tabFTP.UseBorder = false;
            this.m_tabFTP.UseClickedEmphasizeTextColor = false;
            this.m_tabFTP.UseCustomizeClickedColor = false;
            this.m_tabFTP.UseEdge = false;
            this.m_tabFTP.UseHoverEmphasizeCustomColor = false;
            this.m_tabFTP.UseImage = false;
            this.m_tabFTP.UserHoverEmpahsize = false;
            this.m_tabFTP.UseSubFont = false;
            this.m_tabFTP.Click += new System.EventHandler(this.Click_Tab);
            // 
            // Config_Communication
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.Controls.Add(this.tableLayoutPanel1);
            this.Margin = new System.Windows.Forms.Padding(0);
            this.Name = "Config_Communication";
            this.Size = new System.Drawing.Size(1156, 900);
            this.tableLayoutPanel1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private Sys3Controls.Sys3button m_tabSerial;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.Panel m_panel;
        private Sys3Controls.Sys3button m_tabWCF;
        private Sys3Controls.Sys3button m_tabFTP;
        private Sys3Controls.Sys3button m_tabSocket;
    }
}
