namespace FrameOfSystem3.Views.Login
{
    partial class Form_IrisRecognition
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
            this.m_groupLogin = new Sys3Controls.Sys3GroupBox();
            this.m_picturePreview = new System.Windows.Forms.PictureBox();
            this.m_btnExit = new Sys3Controls.Sys3button();
            this.m_labelComment = new Sys3Controls.Sys3Label();
            ((System.ComponentModel.ISupportInitialize)(this.m_picturePreview)).BeginInit();
            this.SuspendLayout();
            // 
            // m_groupLogin
            // 
            this.m_groupLogin.BackColor = System.Drawing.SystemColors.Control;
            this.m_groupLogin.BackGroundColor = System.Drawing.Color.WhiteSmoke;
            this.m_groupLogin.EdgeBorderStroke = 2;
            this.m_groupLogin.EdgeRadius = 5;
            this.m_groupLogin.LabelFont = new System.Drawing.Font("맑은 고딕", 11F, System.Drawing.FontStyle.Bold);
            this.m_groupLogin.LabelGradientColorFirst = System.Drawing.Color.FromArgb(((int)(((byte)(240)))), ((int)(((byte)(240)))), ((int)(((byte)(240)))));
            this.m_groupLogin.LabelGradientColorSecond = System.Drawing.Color.FromArgb(((int)(((byte)(208)))), ((int)(((byte)(208)))), ((int)(((byte)(208)))));
            this.m_groupLogin.LabelHeight = 40;
            this.m_groupLogin.LabelTextColor = System.Drawing.Color.DimGray;
            this.m_groupLogin.Location = new System.Drawing.Point(-2, 1);
            this.m_groupLogin.Name = "m_groupLogin";
            this.m_groupLogin.Size = new System.Drawing.Size(646, 577);
            this.m_groupLogin.TabIndex = 1367;
            this.m_groupLogin.Text = "IRIS RECOGNITION";
            this.m_groupLogin.TextAlign = Sys3Controls.EN_TEXTALIGN.MIDDLE_CENTER;
            this.m_groupLogin.UseLabelBorder = true;
            // 
            // m_picturePreview
            // 
            this.m_picturePreview.Location = new System.Drawing.Point(1, 42);
            this.m_picturePreview.Name = "m_picturePreview";
            this.m_picturePreview.Size = new System.Drawing.Size(640, 480);
            this.m_picturePreview.TabIndex = 1368;
            this.m_picturePreview.TabStop = false;
            // 
            // m_btnExit
            // 
            this.m_btnExit.BorderWidth = 2;
            this.m_btnExit.ButtonClicked = false;
            this.m_btnExit.DisabledColor = System.Drawing.Color.DarkGray;
            this.m_btnExit.EdgeRadius = 5;
            this.m_btnExit.GradientAngle = 45F;
            this.m_btnExit.GradientFirstColor = System.Drawing.Color.White;
            this.m_btnExit.GradientSecondColor = System.Drawing.Color.Gray;
            this.m_btnExit.ImagePosition = new System.Drawing.Point(13, 13);
            this.m_btnExit.ImageSize = new System.Drawing.Point(30, 30);
            this.m_btnExit.LoadImage = global::FrameOfSystem3.Properties.Resources.CREATE_FILE;
            this.m_btnExit.Location = new System.Drawing.Point(603, 1);
            this.m_btnExit.MainFont = new System.Drawing.Font("맑은 고딕", 15F, System.Drawing.FontStyle.Bold);
            this.m_btnExit.MainFontColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(36)))), ((int)(((byte)(0)))));
            this.m_btnExit.Name = "m_btnExit";
            this.m_btnExit.Size = new System.Drawing.Size(41, 42);
            this.m_btnExit.SubFont = new System.Drawing.Font("맑은 고딕", 8F, System.Drawing.FontStyle.Bold);
            this.m_btnExit.SubFontColor = System.Drawing.Color.DarkBlue;
            this.m_btnExit.SubText = "STATUS";
            this.m_btnExit.TabIndex = 1369;
            this.m_btnExit.Text = "X";
            this.m_btnExit.TextAlignMain = Sys3Controls.EN_TEXTALIGN.MIDDLE_CENTER;
            this.m_btnExit.TextAlignSub = Sys3Controls.EN_TEXTALIGN.TOP_LEFT;
            this.m_btnExit.UseBorder = false;
            this.m_btnExit.UseEdge = false;
            this.m_btnExit.UseImage = false;
            this.m_btnExit.UseSubFont = false;
            this.m_btnExit.Click += new System.EventHandler(this.Click_Exit);
            // 
            // m_labelComment
            // 
            this.m_labelComment.BackGroundColor = System.Drawing.Color.Transparent;
            this.m_labelComment.BorderStroke = 2;
            this.m_labelComment.BorderStyle = System.Windows.Forms.ButtonBorderStyle.Dotted;
            this.m_labelComment.DisabledColor = System.Drawing.Color.White;
            this.m_labelComment.EdgeRadius = 1;
            this.m_labelComment.Enabled = false;
            this.m_labelComment.Location = new System.Drawing.Point(1, 523);
            this.m_labelComment.MainFont = new System.Drawing.Font("맑은 고딕", 20F, System.Drawing.FontStyle.Bold);
            this.m_labelComment.MainFontColor = System.Drawing.Color.Navy;
            this.m_labelComment.Name = "m_labelComment";
            this.m_labelComment.Size = new System.Drawing.Size(640, 52);
            this.m_labelComment.SubFont = new System.Drawing.Font("맑은 고딕", 10F);
            this.m_labelComment.SubFontColor = System.Drawing.Color.Black;
            this.m_labelComment.SubText = "";
            this.m_labelComment.TabIndex = 1370;
            this.m_labelComment.Text = "IRIS RECOGNITION......";
            this.m_labelComment.TextAlignMain = Sys3Controls.EN_TEXTALIGN.MIDDLE_CENTER;
            this.m_labelComment.TextAlignSub = Sys3Controls.EN_TEXTALIGN.MIDDLE_CENTER;
            this.m_labelComment.TextAlignUnit = Sys3Controls.EN_TEXTALIGN.MIDDLE_CENTER;
            this.m_labelComment.UnitAreaRate = 30;
            this.m_labelComment.UnitFont = new System.Drawing.Font("맑은 고딕", 10F, System.Drawing.FontStyle.Bold);
            this.m_labelComment.UnitFontColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.m_labelComment.UnitPositionVertical = false;
            this.m_labelComment.UnitText = "";
            this.m_labelComment.UseBorder = true;
            this.m_labelComment.UseEdgeRadius = false;
            this.m_labelComment.UseSubFont = false;
            this.m_labelComment.UseUnitFont = false;
            // 
            // Form_IrisRecognition
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(643, 577);
            this.ControlBox = false;
            this.Controls.Add(this.m_labelComment);
            this.Controls.Add(this.m_btnExit);
            this.Controls.Add(this.m_picturePreview);
            this.Controls.Add(this.m_groupLogin);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "Form_IrisRecognition";
            this.ShowIcon = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Form_IrisRecognition";
            this.TopMost = true;
            ((System.ComponentModel.ISupportInitialize)(this.m_picturePreview)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private Sys3Controls.Sys3GroupBox m_groupLogin;
        private System.Windows.Forms.PictureBox m_picturePreview;
        private Sys3Controls.Sys3button m_btnExit;
        private Sys3Controls.Sys3Label m_labelComment;
    }
}