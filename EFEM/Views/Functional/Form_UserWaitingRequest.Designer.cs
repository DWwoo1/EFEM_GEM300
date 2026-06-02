namespace FrameOfSystem3.Views.Functional
{
    partial class Form_UserWaitingRequest
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
            this.m_gbUserMessage = new Sys3Controls.Sys3GroupBox();
            this.m_lbMessage = new Sys3Controls.Sys3Label();
            this.m_btnClose = new Sys3Controls.Sys3button();
            this._btnAbort = new Sys3Controls.Sys3button();
            this.SuspendLayout();
            // 
            // m_gbUserMessage
            // 
            this.m_gbUserMessage.BackGroundColor = System.Drawing.Color.WhiteSmoke;
            this.m_gbUserMessage.Dock = System.Windows.Forms.DockStyle.Fill;
            this.m_gbUserMessage.EdgeBorderStroke = 2;
            this.m_gbUserMessage.EdgeRadius = 2;
            this.m_gbUserMessage.LabelFont = new System.Drawing.Font("맑은 고딕", 18F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.m_gbUserMessage.LabelGradientColorFirst = System.Drawing.Color.OrangeRed;
            this.m_gbUserMessage.LabelGradientColorSecond = System.Drawing.Color.Crimson;
            this.m_gbUserMessage.LabelHeight = 50;
            this.m_gbUserMessage.LabelTextColor = System.Drawing.Color.White;
            this.m_gbUserMessage.Location = new System.Drawing.Point(0, 0);
            this.m_gbUserMessage.Name = "m_gbUserMessage";
            this.m_gbUserMessage.Size = new System.Drawing.Size(831, 466);
            this.m_gbUserMessage.TabIndex = 0;
            this.m_gbUserMessage.Text = "잠시만 기다려주세요.";
            this.m_gbUserMessage.TextAlign = Sys3Controls.EN_TEXTALIGN.MIDDLE_CENTER;
            this.m_gbUserMessage.ThemeIndex = 0;
            this.m_gbUserMessage.UseLabelBorder = true;
            this.m_gbUserMessage.MouseDown += new System.Windows.Forms.MouseEventHandler(this.m_gbUserMessage_MouseDown);
            this.m_gbUserMessage.MouseMove += new System.Windows.Forms.MouseEventHandler(this.m_gbUserMessage_MouseMove);
            // 
            // m_lbMessage
            // 
            this.m_lbMessage.BackGroundColor = System.Drawing.Color.White;
            this.m_lbMessage.BorderStroke = 2;
            this.m_lbMessage.BorderStyle = System.Windows.Forms.ButtonBorderStyle.Solid;
            this.m_lbMessage.Description = "";
            this.m_lbMessage.DisabledColor = System.Drawing.Color.DarkGray;
            this.m_lbMessage.EdgeRadius = 1;
            this.m_lbMessage.ImagePosition = new System.Drawing.Point(0, 0);
            this.m_lbMessage.ImageSize = new System.Drawing.Point(0, 0);
            this.m_lbMessage.LoadImage = null;
            this.m_lbMessage.Location = new System.Drawing.Point(17, 62);
            this.m_lbMessage.MainFont = new System.Drawing.Font("맑은 고딕", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.m_lbMessage.MainFontColor = System.Drawing.Color.Black;
            this.m_lbMessage.Name = "m_lbMessage";
            this.m_lbMessage.Size = new System.Drawing.Size(802, 296);
            this.m_lbMessage.SubFont = new System.Drawing.Font("맑은 고딕", 10F);
            this.m_lbMessage.SubFontColor = System.Drawing.Color.Black;
            this.m_lbMessage.SubText = "";
            this.m_lbMessage.TabIndex = 1;
            this.m_lbMessage.Text = "요청하신 작업의 완료를 대기중입니다.";
            this.m_lbMessage.TextAlignMain = Sys3Controls.EN_TEXTALIGN.MIDDLE_CENTER;
            this.m_lbMessage.TextAlignSub = Sys3Controls.EN_TEXTALIGN.MIDDLE_CENTER;
            this.m_lbMessage.TextAlignUnit = Sys3Controls.EN_TEXTALIGN.MIDDLE_CENTER;
            this.m_lbMessage.ThemeIndex = 0;
            this.m_lbMessage.UnitAreaRate = 30;
            this.m_lbMessage.UnitFont = new System.Drawing.Font("맑은 고딕", 10F, System.Drawing.FontStyle.Bold);
            this.m_lbMessage.UnitFontColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.m_lbMessage.UnitPositionVertical = false;
            this.m_lbMessage.UnitText = "";
            this.m_lbMessage.UseBorder = true;
            this.m_lbMessage.UseEdgeRadius = false;
            this.m_lbMessage.UseImage = false;
            this.m_lbMessage.UseSubFont = false;
            this.m_lbMessage.UseUnitFont = false;
            // 
            // m_btnClose
            // 
            this.m_btnClose.BorderWidth = 2;
            this.m_btnClose.ButtonClicked = false;
            this.m_btnClose.ClickedEmphasizeTextColor = System.Drawing.Color.White;
            this.m_btnClose.CustomClickedGradientFirstColor = System.Drawing.Color.White;
            this.m_btnClose.CustomClickedGradientSecondColor = System.Drawing.Color.White;
            this.m_btnClose.Description = "";
            this.m_btnClose.DisabledColor = System.Drawing.Color.DarkGray;
            this.m_btnClose.EdgeRadius = 1;
            this.m_btnClose.GradientAngle = 70F;
            this.m_btnClose.GradientFirstColor = System.Drawing.Color.LemonChiffon;
            this.m_btnClose.GradientSecondColor = System.Drawing.Color.Gold;
            this.m_btnClose.HoverEmphasizeCustomColor = System.Drawing.Color.White;
            this.m_btnClose.ImagePosition = new System.Drawing.Point(0, 0);
            this.m_btnClose.ImageSize = new System.Drawing.Point(0, 0);
            this.m_btnClose.LoadImage = null;
            this.m_btnClose.Location = new System.Drawing.Point(305, 364);
            this.m_btnClose.MainFont = new System.Drawing.Font("맑은 고딕", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.m_btnClose.MainFontColor = System.Drawing.Color.Black;
            this.m_btnClose.Name = "m_btnClose";
            this.m_btnClose.Size = new System.Drawing.Size(227, 80);
            this.m_btnClose.SubFont = new System.Drawing.Font("맑은 고딕", 10F);
            this.m_btnClose.SubFontColor = System.Drawing.Color.Black;
            this.m_btnClose.SubText = "";
            this.m_btnClose.TabIndex = 20067;
            this.m_btnClose.Tag = "";
            this.m_btnClose.Text = "CLOSE";
            this.m_btnClose.TextAlignMain = Sys3Controls.EN_TEXTALIGN.MIDDLE_CENTER;
            this.m_btnClose.TextAlignSub = Sys3Controls.EN_TEXTALIGN.MIDDLE_CENTER;
            this.m_btnClose.ThemeIndex = 0;
            this.m_btnClose.UseBorder = false;
            this.m_btnClose.UseClickedEmphasizeTextColor = false;
            this.m_btnClose.UseCustomizeClickedColor = false;
            this.m_btnClose.UseEdge = false;
            this.m_btnClose.UseHoverEmphasizeCustomColor = false;
            this.m_btnClose.UseImage = false;
            this.m_btnClose.UserHoverEmpahsize = false;
            this.m_btnClose.UseSubFont = false;
            this.m_btnClose.Click += new System.EventHandler(this.Close_Click);
            // 
            // _btnAbort
            // 
            this._btnAbort.BorderWidth = 2;
            this._btnAbort.ButtonClicked = false;
            this._btnAbort.ClickedEmphasizeTextColor = System.Drawing.Color.White;
            this._btnAbort.CustomClickedGradientFirstColor = System.Drawing.Color.White;
            this._btnAbort.CustomClickedGradientSecondColor = System.Drawing.Color.White;
            this._btnAbort.Description = "";
            this._btnAbort.DisabledColor = System.Drawing.Color.DarkGray;
            this._btnAbort.EdgeRadius = 1;
            this._btnAbort.GradientAngle = 70F;
            this._btnAbort.GradientFirstColor = System.Drawing.Color.White;
            this._btnAbort.GradientSecondColor = System.Drawing.Color.Crimson;
            this._btnAbort.HoverEmphasizeCustomColor = System.Drawing.Color.White;
            this._btnAbort.ImagePosition = new System.Drawing.Point(0, 0);
            this._btnAbort.ImageSize = new System.Drawing.Point(0, 0);
            this._btnAbort.LoadImage = null;
            this._btnAbort.Location = new System.Drawing.Point(305, 364);
            this._btnAbort.MainFont = new System.Drawing.Font("맑은 고딕", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this._btnAbort.MainFontColor = System.Drawing.Color.White;
            this._btnAbort.Name = "_btnAbort";
            this._btnAbort.Size = new System.Drawing.Size(227, 80);
            this._btnAbort.SubFont = new System.Drawing.Font("맑은 고딕", 10F);
            this._btnAbort.SubFontColor = System.Drawing.Color.Black;
            this._btnAbort.SubText = "";
            this._btnAbort.TabIndex = 20068;
            this._btnAbort.Tag = "";
            this._btnAbort.Text = "ABORT";
            this._btnAbort.TextAlignMain = Sys3Controls.EN_TEXTALIGN.MIDDLE_CENTER;
            this._btnAbort.TextAlignSub = Sys3Controls.EN_TEXTALIGN.MIDDLE_CENTER;
            this._btnAbort.ThemeIndex = 0;
            this._btnAbort.UseBorder = false;
            this._btnAbort.UseClickedEmphasizeTextColor = false;
            this._btnAbort.UseCustomizeClickedColor = false;
            this._btnAbort.UseEdge = false;
            this._btnAbort.UseHoverEmphasizeCustomColor = false;
            this._btnAbort.UseImage = false;
            this._btnAbort.UserHoverEmpahsize = false;
            this._btnAbort.UseSubFont = false;
            this._btnAbort.Click += new System.EventHandler(this.Abort_Click);
            // 
            // Form_UserWaitingRequest
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.ClientSize = new System.Drawing.Size(831, 466);
            this.Controls.Add(this._btnAbort);
            this.Controls.Add(this.m_btnClose);
            this.Controls.Add(this.m_lbMessage);
            this.Controls.Add(this.m_gbUserMessage);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "Form_UserWaitingRequest";
            this.Text = "Form_InformOccurNG";
            this.Activated += new System.EventHandler(this.Form_UserInformTimer_Activated);
            this.Load += new System.EventHandler(this.Form_Load);
            this.VisibleChanged += new System.EventHandler(this.Form_InformOccurNG_VisibleChanged);
            this.ResumeLayout(false);

        }

        #endregion

        private Sys3Controls.Sys3GroupBox m_gbUserMessage;
        private Sys3Controls.Sys3Label m_lbMessage;
        private Sys3Controls.Sys3button m_btnClose;
        private Sys3Controls.Sys3button _btnAbort;
    }
}