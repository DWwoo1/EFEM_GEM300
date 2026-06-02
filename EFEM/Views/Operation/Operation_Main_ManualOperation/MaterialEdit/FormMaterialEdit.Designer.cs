namespace FrameOfSystem3.Views.Functional
{
    partial class FormMaterialEdit
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
            if (disposing)
            {
                if (pgSubstrateAttribute != null)
                {
                    pgSubstrateAttribute.Dispose();
                    pgSubstrateAttribute = null;
                }
                if (components != null)
                {
                    components.Dispose();
                }
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
            this.btnCancel = new Sys3Controls.Sys3button();
            this.btnOK = new Sys3Controls.Sys3button();
            this.lblTitleBar = new Sys3Controls.Sys3GroupBox();
            this.pgSubstrateAttribute = new System.Windows.Forms.PropertyGrid();
            this.SuspendLayout();
            // 
            // m_groupTitle
            // 
            this.m_groupTitle.BackGroundColor = System.Drawing.Color.WhiteSmoke;
            this.m_groupTitle.EdgeBorderStroke = 2;
            this.m_groupTitle.EdgeRadius = 2;
            this.m_groupTitle.LabelFont = new System.Drawing.Font("맑은 고딕", 10F, System.Drawing.FontStyle.Bold);
            this.m_groupTitle.LabelGradientColorFirst = System.Drawing.Color.FromArgb(((int)(((byte)(208)))), ((int)(((byte)(208)))), ((int)(((byte)(208)))));
            this.m_groupTitle.LabelGradientColorSecond = System.Drawing.Color.FromArgb(((int)(((byte)(240)))), ((int)(((byte)(240)))), ((int)(((byte)(240)))));
            this.m_groupTitle.LabelHeight = 35;
            this.m_groupTitle.LabelTextColor = System.Drawing.Color.Black;
            this.m_groupTitle.Location = new System.Drawing.Point(0, 0);
            this.m_groupTitle.Name = "m_groupTitle";
            this.m_groupTitle.Size = new System.Drawing.Size(550, 643);
            this.m_groupTitle.TabIndex = 3;
            this.m_groupTitle.Text = "Confirmation Message";
            this.m_groupTitle.TextAlign = Sys3Controls.EN_TEXTALIGN.MIDDLE_CENTER;
            this.m_groupTitle.ThemeIndex = 0;
            this.m_groupTitle.UseLabelBorder = true;
            // 
            // btnCancel
            // 
            this.btnCancel.BorderWidth = 3;
            this.btnCancel.ButtonClicked = false;
            this.btnCancel.ClickedEmphasizeTextColor = System.Drawing.Color.White;
            this.btnCancel.CustomClickedGradientFirstColor = System.Drawing.Color.White;
            this.btnCancel.CustomClickedGradientSecondColor = System.Drawing.Color.White;
            this.btnCancel.Description = "";
            this.btnCancel.DisabledColor = System.Drawing.Color.DarkGray;
            this.btnCancel.EdgeRadius = 5;
            this.btnCancel.GradientAngle = 70F;
            this.btnCancel.GradientFirstColor = System.Drawing.Color.White;
            this.btnCancel.GradientSecondColor = System.Drawing.Color.FromArgb(((int)(((byte)(170)))), ((int)(((byte)(176)))), ((int)(((byte)(183)))));
            this.btnCancel.HoverEmphasizeCustomColor = System.Drawing.Color.White;
            this.btnCancel.ImagePosition = new System.Drawing.Point(10, 10);
            this.btnCancel.ImageSize = new System.Drawing.Point(30, 30);
            this.btnCancel.LoadImage = global::FrameOfSystem3.Properties.Resources.CLEAR_BLACK;
            this.btnCancel.Location = new System.Drawing.Point(278, 582);
            this.btnCancel.MainFont = new System.Drawing.Font("맑은 고딕", 11F, System.Drawing.FontStyle.Bold);
            this.btnCancel.MainFontColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(36)))), ((int)(((byte)(0)))));
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(130, 50);
            this.btnCancel.SubFont = new System.Drawing.Font("맑은 고딕", 8F, System.Drawing.FontStyle.Bold);
            this.btnCancel.SubFontColor = System.Drawing.Color.DarkBlue;
            this.btnCancel.SubText = "STATUS";
            this.btnCancel.TabIndex = 1;
            this.btnCancel.Text = "CANCEL";
            this.btnCancel.TextAlignMain = Sys3Controls.EN_TEXTALIGN.MIDDLE_CENTER;
            this.btnCancel.TextAlignSub = Sys3Controls.EN_TEXTALIGN.TOP_LEFT;
            this.btnCancel.ThemeIndex = 0;
            this.btnCancel.UseBorder = true;
            this.btnCancel.UseClickedEmphasizeTextColor = false;
            this.btnCancel.UseCustomizeClickedColor = false;
            this.btnCancel.UseEdge = true;
            this.btnCancel.UseHoverEmphasizeCustomColor = false;
            this.btnCancel.UseImage = false;
            this.btnCancel.UserHoverEmpahsize = false;
            this.btnCancel.UseSubFont = false;
            this.btnCancel.Click += new System.EventHandler(this.BtnOkorCancelClicked);
            // 
            // btnOK
            // 
            this.btnOK.BorderWidth = 3;
            this.btnOK.ButtonClicked = false;
            this.btnOK.ClickedEmphasizeTextColor = System.Drawing.Color.White;
            this.btnOK.CustomClickedGradientFirstColor = System.Drawing.Color.White;
            this.btnOK.CustomClickedGradientSecondColor = System.Drawing.Color.White;
            this.btnOK.Description = "";
            this.btnOK.DisabledColor = System.Drawing.Color.DarkGray;
            this.btnOK.EdgeRadius = 5;
            this.btnOK.GradientAngle = 70F;
            this.btnOK.GradientFirstColor = System.Drawing.Color.White;
            this.btnOK.GradientSecondColor = System.Drawing.Color.LightSlateGray;
            this.btnOK.HoverEmphasizeCustomColor = System.Drawing.Color.White;
            this.btnOK.ImagePosition = new System.Drawing.Point(10, 10);
            this.btnOK.ImageSize = new System.Drawing.Point(30, 30);
            this.btnOK.LoadImage = global::FrameOfSystem3.Properties.Resources.CLEAR_BLACK;
            this.btnOK.Location = new System.Drawing.Point(142, 582);
            this.btnOK.MainFont = new System.Drawing.Font("맑은 고딕", 11F, System.Drawing.FontStyle.Bold);
            this.btnOK.MainFontColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(36)))), ((int)(((byte)(0)))));
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size(130, 50);
            this.btnOK.SubFont = new System.Drawing.Font("맑은 고딕", 8F, System.Drawing.FontStyle.Bold);
            this.btnOK.SubFontColor = System.Drawing.Color.DarkBlue;
            this.btnOK.SubText = "STATUS";
            this.btnOK.TabIndex = 0;
            this.btnOK.Text = "OK";
            this.btnOK.TextAlignMain = Sys3Controls.EN_TEXTALIGN.MIDDLE_CENTER;
            this.btnOK.TextAlignSub = Sys3Controls.EN_TEXTALIGN.TOP_LEFT;
            this.btnOK.ThemeIndex = 0;
            this.btnOK.UseBorder = true;
            this.btnOK.UseClickedEmphasizeTextColor = false;
            this.btnOK.UseCustomizeClickedColor = false;
            this.btnOK.UseEdge = true;
            this.btnOK.UseHoverEmphasizeCustomColor = false;
            this.btnOK.UseImage = false;
            this.btnOK.UserHoverEmpahsize = false;
            this.btnOK.UseSubFont = false;
            this.btnOK.Click += new System.EventHandler(this.BtnOkorCancelClicked);
            // 
            // lblTitleBar
            // 
            this.lblTitleBar.BackGroundColor = System.Drawing.Color.WhiteSmoke;
            this.lblTitleBar.EdgeBorderStroke = 2;
            this.lblTitleBar.EdgeRadius = 2;
            this.lblTitleBar.LabelFont = new System.Drawing.Font("맑은 고딕", 10F, System.Drawing.FontStyle.Bold);
            this.lblTitleBar.LabelGradientColorFirst = System.Drawing.Color.FromArgb(((int)(((byte)(208)))), ((int)(((byte)(208)))), ((int)(((byte)(208)))));
            this.lblTitleBar.LabelGradientColorSecond = System.Drawing.Color.FromArgb(((int)(((byte)(240)))), ((int)(((byte)(240)))), ((int)(((byte)(240)))));
            this.lblTitleBar.LabelHeight = 35;
            this.lblTitleBar.LabelTextColor = System.Drawing.Color.Black;
            this.lblTitleBar.Location = new System.Drawing.Point(0, 0);
            this.lblTitleBar.Name = "lblTitleBar";
            this.lblTitleBar.Size = new System.Drawing.Size(550, 37);
            this.lblTitleBar.TabIndex = 6;
            this.lblTitleBar.Text = "Substrate Attribute Editor";
            this.lblTitleBar.TextAlign = Sys3Controls.EN_TEXTALIGN.MIDDLE_CENTER;
            this.lblTitleBar.ThemeIndex = 0;
            this.lblTitleBar.UseLabelBorder = true;
            this.lblTitleBar.MouseDown += new System.Windows.Forms.MouseEventHandler(this.MouseDown_Title);
            this.lblTitleBar.MouseMove += new System.Windows.Forms.MouseEventHandler(this.MouseMove_Title);
            this.lblTitleBar.MouseUp += new System.Windows.Forms.MouseEventHandler(this.MouseUp_Title);
            // 
            // pgSubstrateAttribute
            // 
            this.pgSubstrateAttribute.Font = new System.Drawing.Font("굴림", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.pgSubstrateAttribute.Location = new System.Drawing.Point(9, 41);
            this.pgSubstrateAttribute.Name = "pgSubstrateAttribute";
            this.pgSubstrateAttribute.Size = new System.Drawing.Size(532, 535);
            this.pgSubstrateAttribute.TabIndex = 7;
            this.pgSubstrateAttribute.SelectedGridItemChanged += new System.Windows.Forms.SelectedGridItemChangedEventHandler(this.PgItemSelected);
            // 
            // FormMaterialEdit
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.ClientSize = new System.Drawing.Size(550, 644);
            this.ControlBox = false;
            this.Controls.Add(this.pgSubstrateAttribute);
            this.Controls.Add(this.lblTitleBar);
            this.Controls.Add(this.btnOK);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.m_groupTitle);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FormMaterialEdit";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
            this.Text = "`";
            this.TopMost = true;
            this.ResumeLayout(false);

        }

        #endregion

		private Sys3Controls.Sys3GroupBox m_groupTitle;
		private Sys3Controls.Sys3button btnCancel;
		private Sys3Controls.Sys3button btnOK;
		private Sys3Controls.Sys3GroupBox lblTitleBar;
        private System.Windows.Forms.PropertyGrid pgSubstrateAttribute;
    }
}