namespace FrameOfSystem3.Views.Functional
{
    partial class FormMaterialAttributeEdit
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
                DestroyRows();

                if (pnFields != null)
                {
                    pnFields.Dispose();
                    pnFields = null;
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
            this.btnSearch = new Sys3Controls.Sys3button();
            this.lblKeyword = new Sys3Controls.Sys3Label();
            this.btnExpandAll = new Sys3Controls.Sys3button();
            this.btnCollapseAll = new Sys3Controls.Sys3button();
            this.pnFields = new System.Windows.Forms.FlowLayoutPanel();
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
            // btnSearch
            //
            this.btnSearch.BorderWidth = 3;
            this.btnSearch.ButtonClicked = false;
            this.btnSearch.ClickedEmphasizeTextColor = System.Drawing.Color.White;
            this.btnSearch.CustomClickedGradientFirstColor = System.Drawing.Color.White;
            this.btnSearch.CustomClickedGradientSecondColor = System.Drawing.Color.White;
            this.btnSearch.Description = "";
            this.btnSearch.DisabledColor = System.Drawing.Color.DarkGray;
            this.btnSearch.EdgeRadius = 5;
            this.btnSearch.GradientAngle = 70F;
            this.btnSearch.GradientFirstColor = System.Drawing.Color.White;
            this.btnSearch.GradientSecondColor = System.Drawing.Color.LightSlateGray;
            this.btnSearch.HoverEmphasizeCustomColor = System.Drawing.Color.White;
            this.btnSearch.ImagePosition = new System.Drawing.Point(10, 10);
            this.btnSearch.ImageSize = new System.Drawing.Point(30, 30);
            this.btnSearch.LoadImage = null;
            this.btnSearch.Location = new System.Drawing.Point(243, 43);
            this.btnSearch.MainFont = new System.Drawing.Font("맑은 고딕", 10F, System.Drawing.FontStyle.Bold);
            this.btnSearch.MainFontColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(36)))), ((int)(((byte)(0)))));
            this.btnSearch.Name = "btnSearch";
            this.btnSearch.Size = new System.Drawing.Size(80, 34);
            this.btnSearch.SubFont = new System.Drawing.Font("맑은 고딕", 8F, System.Drawing.FontStyle.Bold);
            this.btnSearch.SubFontColor = System.Drawing.Color.DarkBlue;
            this.btnSearch.SubText = "STATUS";
            this.btnSearch.TabIndex = 8;
            this.btnSearch.Text = "검색";
            this.btnSearch.TextAlignMain = Sys3Controls.EN_TEXTALIGN.MIDDLE_CENTER;
            this.btnSearch.TextAlignSub = Sys3Controls.EN_TEXTALIGN.TOP_LEFT;
            this.btnSearch.ThemeIndex = 0;
            this.btnSearch.UseBorder = true;
            this.btnSearch.UseClickedEmphasizeTextColor = false;
            this.btnSearch.UseCustomizeClickedColor = false;
            this.btnSearch.UseEdge = true;
            this.btnSearch.UseHoverEmphasizeCustomColor = false;
            this.btnSearch.UseImage = false;
            this.btnSearch.UserHoverEmpahsize = false;
            this.btnSearch.UseSubFont = false;
            this.btnSearch.Click += new System.EventHandler(this.BtnSearchClicked);
            //
            // lblKeyword
            //
            this.lblKeyword.BackGroundColor = System.Drawing.Color.White;
            this.lblKeyword.BorderStroke = 1;
            this.lblKeyword.BorderStyle = System.Windows.Forms.ButtonBorderStyle.Solid;
            this.lblKeyword.Description = "";
            this.lblKeyword.DisabledColor = System.Drawing.Color.Silver;
            this.lblKeyword.EdgeRadius = 1;
            this.lblKeyword.ImagePosition = new System.Drawing.Point(0, 0);
            this.lblKeyword.ImageSize = new System.Drawing.Point(0, 0);
            this.lblKeyword.LoadImage = null;
            this.lblKeyword.Location = new System.Drawing.Point(9, 43);
            this.lblKeyword.MainFont = new System.Drawing.Font("맑은 고딕", 10F, System.Drawing.FontStyle.Bold);
            this.lblKeyword.MainFontColor = System.Drawing.Color.Gray;
            this.lblKeyword.Name = "lblKeyword";
            this.lblKeyword.Size = new System.Drawing.Size(230, 34);
            this.lblKeyword.SubFont = new System.Drawing.Font("맑은 고딕", 8F, System.Drawing.FontStyle.Bold);
            this.lblKeyword.SubFontColor = System.Drawing.Color.DarkBlue;
            this.lblKeyword.SubText = "";
            this.lblKeyword.TabIndex = 9;
            this.lblKeyword.Text = "(전체)";
            this.lblKeyword.TextAlignMain = Sys3Controls.EN_TEXTALIGN.MIDDLE_LEFT;
            this.lblKeyword.TextAlignSub = Sys3Controls.EN_TEXTALIGN.TOP_LEFT;
            this.lblKeyword.TextAlignUnit = Sys3Controls.EN_TEXTALIGN.BOTTOM_RIGHT;
            this.lblKeyword.ThemeIndex = 0;
            this.lblKeyword.UnitFont = new System.Drawing.Font("맑은 고딕", 9F, System.Drawing.FontStyle.Bold);
            this.lblKeyword.UnitFontColor = System.Drawing.Color.Red;
            this.lblKeyword.UnitText = "";
            this.lblKeyword.UseBorder = true;
            this.lblKeyword.UseEdgeRadius = false;
            this.lblKeyword.UseImage = false;
            this.lblKeyword.UseSubFont = false;
            this.lblKeyword.UseUnitFont = false;
            this.lblKeyword.Click += new System.EventHandler(this.EditKeywordClicked);
            //
            // btnExpandAll
            //
            this.btnExpandAll.BorderWidth = 3;
            this.btnExpandAll.ButtonClicked = false;
            this.btnExpandAll.ClickedEmphasizeTextColor = System.Drawing.Color.White;
            this.btnExpandAll.CustomClickedGradientFirstColor = System.Drawing.Color.White;
            this.btnExpandAll.CustomClickedGradientSecondColor = System.Drawing.Color.White;
            this.btnExpandAll.Description = "";
            this.btnExpandAll.DisabledColor = System.Drawing.Color.DarkGray;
            this.btnExpandAll.EdgeRadius = 5;
            this.btnExpandAll.GradientAngle = 70F;
            this.btnExpandAll.GradientFirstColor = System.Drawing.Color.White;
            this.btnExpandAll.GradientSecondColor = System.Drawing.Color.LightSlateGray;
            this.btnExpandAll.HoverEmphasizeCustomColor = System.Drawing.Color.White;
            this.btnExpandAll.ImagePosition = new System.Drawing.Point(10, 10);
            this.btnExpandAll.ImageSize = new System.Drawing.Point(30, 30);
            this.btnExpandAll.LoadImage = null;
            this.btnExpandAll.Location = new System.Drawing.Point(327, 43);
            this.btnExpandAll.MainFont = new System.Drawing.Font("맑은 고딕", 10F, System.Drawing.FontStyle.Bold);
            this.btnExpandAll.MainFontColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(36)))), ((int)(((byte)(0)))));
            this.btnExpandAll.Name = "btnExpandAll";
            this.btnExpandAll.Size = new System.Drawing.Size(104, 34);
            this.btnExpandAll.SubFont = new System.Drawing.Font("맑은 고딕", 8F, System.Drawing.FontStyle.Bold);
            this.btnExpandAll.SubFontColor = System.Drawing.Color.DarkBlue;
            this.btnExpandAll.SubText = "STATUS";
            this.btnExpandAll.TabIndex = 10;
            this.btnExpandAll.Text = "전체 펼침";
            this.btnExpandAll.TextAlignMain = Sys3Controls.EN_TEXTALIGN.MIDDLE_CENTER;
            this.btnExpandAll.TextAlignSub = Sys3Controls.EN_TEXTALIGN.TOP_LEFT;
            this.btnExpandAll.ThemeIndex = 0;
            this.btnExpandAll.UseBorder = true;
            this.btnExpandAll.UseClickedEmphasizeTextColor = false;
            this.btnExpandAll.UseCustomizeClickedColor = false;
            this.btnExpandAll.UseEdge = true;
            this.btnExpandAll.UseHoverEmphasizeCustomColor = false;
            this.btnExpandAll.UseImage = false;
            this.btnExpandAll.UserHoverEmpahsize = false;
            this.btnExpandAll.UseSubFont = false;
            this.btnExpandAll.Click += new System.EventHandler(this.BtnExpandAllClicked);
            //
            // btnCollapseAll
            //
            this.btnCollapseAll.BorderWidth = 3;
            this.btnCollapseAll.ButtonClicked = false;
            this.btnCollapseAll.ClickedEmphasizeTextColor = System.Drawing.Color.White;
            this.btnCollapseAll.CustomClickedGradientFirstColor = System.Drawing.Color.White;
            this.btnCollapseAll.CustomClickedGradientSecondColor = System.Drawing.Color.White;
            this.btnCollapseAll.Description = "";
            this.btnCollapseAll.DisabledColor = System.Drawing.Color.DarkGray;
            this.btnCollapseAll.EdgeRadius = 5;
            this.btnCollapseAll.GradientAngle = 70F;
            this.btnCollapseAll.GradientFirstColor = System.Drawing.Color.White;
            this.btnCollapseAll.GradientSecondColor = System.Drawing.Color.LightSlateGray;
            this.btnCollapseAll.HoverEmphasizeCustomColor = System.Drawing.Color.White;
            this.btnCollapseAll.ImagePosition = new System.Drawing.Point(10, 10);
            this.btnCollapseAll.ImageSize = new System.Drawing.Point(30, 30);
            this.btnCollapseAll.LoadImage = null;
            this.btnCollapseAll.Location = new System.Drawing.Point(435, 43);
            this.btnCollapseAll.MainFont = new System.Drawing.Font("맑은 고딕", 10F, System.Drawing.FontStyle.Bold);
            this.btnCollapseAll.MainFontColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(36)))), ((int)(((byte)(0)))));
            this.btnCollapseAll.Name = "btnCollapseAll";
            this.btnCollapseAll.Size = new System.Drawing.Size(104, 34);
            this.btnCollapseAll.SubFont = new System.Drawing.Font("맑은 고딕", 8F, System.Drawing.FontStyle.Bold);
            this.btnCollapseAll.SubFontColor = System.Drawing.Color.DarkBlue;
            this.btnCollapseAll.SubText = "STATUS";
            this.btnCollapseAll.TabIndex = 11;
            this.btnCollapseAll.Text = "전체 접기";
            this.btnCollapseAll.TextAlignMain = Sys3Controls.EN_TEXTALIGN.MIDDLE_CENTER;
            this.btnCollapseAll.TextAlignSub = Sys3Controls.EN_TEXTALIGN.TOP_LEFT;
            this.btnCollapseAll.ThemeIndex = 0;
            this.btnCollapseAll.UseBorder = true;
            this.btnCollapseAll.UseClickedEmphasizeTextColor = false;
            this.btnCollapseAll.UseCustomizeClickedColor = false;
            this.btnCollapseAll.UseEdge = true;
            this.btnCollapseAll.UseHoverEmphasizeCustomColor = false;
            this.btnCollapseAll.UseImage = false;
            this.btnCollapseAll.UserHoverEmpahsize = false;
            this.btnCollapseAll.UseSubFont = false;
            this.btnCollapseAll.Click += new System.EventHandler(this.BtnCollapseAllClicked);
            //
            // pnFields
            //
            this.pnFields.AutoScroll = true;
            this.pnFields.BackColor = System.Drawing.Color.WhiteSmoke;
            this.pnFields.FlowDirection = System.Windows.Forms.FlowDirection.TopDown;
            this.pnFields.Location = new System.Drawing.Point(9, 83);
            this.pnFields.Name = "pnFields";
            this.pnFields.Size = new System.Drawing.Size(532, 493);
            this.pnFields.TabIndex = 7;
            this.pnFields.WrapContents = false;
            //
            // FormMaterialAttributeEdit
            //
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.ClientSize = new System.Drawing.Size(550, 644);
            this.ControlBox = false;
            this.Controls.Add(this.pnFields);
            this.Controls.Add(this.btnCollapseAll);
            this.Controls.Add(this.btnExpandAll);
            this.Controls.Add(this.lblKeyword);
            this.Controls.Add(this.btnSearch);
            this.Controls.Add(this.lblTitleBar);
            this.Controls.Add(this.btnOK);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.m_groupTitle);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FormMaterialAttributeEdit";
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
        private Sys3Controls.Sys3button btnSearch;
        private Sys3Controls.Sys3Label lblKeyword;
        private Sys3Controls.Sys3button btnExpandAll;
        private Sys3Controls.Sys3button btnCollapseAll;
        private System.Windows.Forms.FlowLayoutPanel pnFields;
    }
}
