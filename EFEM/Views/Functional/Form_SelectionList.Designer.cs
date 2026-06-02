namespace FrameOfSystem3.Views.Functional
{
    partial class Form_SelectionList
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
            this.btn_SelectedPrev = new Sys3Controls.Sys3button();
            this.btn_SelectedPage = new Sys3Controls.Sys3button();
            this.btn_SelectedNext = new Sys3Controls.Sys3button();
            this.btn_ChoiceNext = new Sys3Controls.Sys3button();
            this.btn_ChoicePage = new Sys3Controls.Sys3button();
            this.btn_ChoicePrev = new Sys3Controls.Sys3button();
            this.m_btnApply = new Sys3Controls.Sys3button();
            this.m_btnCancel = new Sys3Controls.Sys3button();
            this.btn_AllSelect = new Sys3Controls.Sys3button();
            this.btn_AllUnSelect = new Sys3Controls.Sys3button();
            this.lbl_Filtering = new Sys3Controls.Sys3Label();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this._gbSelectionItems = new Sys3Controls.Sys3GroupBoxContainer();
            this.btn_PreviousSet = new Sys3Controls.Sys3button();
            this._gbSelectedItems = new Sys3Controls.Sys3GroupBoxContainer();
            this._gb_Header = new Sys3Controls.Sys3GroupBox();
            this._lbl_Title = new Sys3Controls.Sys3Label();
            this.tableLayoutPanel1.SuspendLayout();
            this._gbSelectionItems.SuspendLayout();
            this._gbSelectedItems.SuspendLayout();
            this.SuspendLayout();
            // 
            // btn_SelectedPrev
            // 
            this.btn_SelectedPrev.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btn_SelectedPrev.BorderWidth = 2;
            this.btn_SelectedPrev.ButtonClicked = false;
            this.btn_SelectedPrev.ClickedEmphasizeTextColor = System.Drawing.Color.White;
            this.btn_SelectedPrev.CustomClickedGradientFirstColor = System.Drawing.Color.White;
            this.btn_SelectedPrev.CustomClickedGradientSecondColor = System.Drawing.Color.White;
            this.btn_SelectedPrev.Description = "";
            this.btn_SelectedPrev.DisabledColor = System.Drawing.Color.DarkGray;
            this.btn_SelectedPrev.EdgeRadius = 5;
            this.btn_SelectedPrev.GradientAngle = 60F;
            this.btn_SelectedPrev.GradientFirstColor = System.Drawing.Color.White;
            this.btn_SelectedPrev.GradientSecondColor = System.Drawing.Color.FromArgb(((int)(((byte)(190)))), ((int)(((byte)(196)))), ((int)(((byte)(203)))));
            this.btn_SelectedPrev.HoverEmphasizeCustomColor = System.Drawing.Color.White;
            this.btn_SelectedPrev.ImagePosition = new System.Drawing.Point(7, 7);
            this.btn_SelectedPrev.ImageSize = new System.Drawing.Point(30, 30);
            this.btn_SelectedPrev.LoadImage = global::FrameOfSystem3.Properties.Resources.CONFIG_ADD3;
            this.btn_SelectedPrev.Location = new System.Drawing.Point(12, 65);
            this.btn_SelectedPrev.MainFont = new System.Drawing.Font("맑은 고딕", 14F, System.Drawing.FontStyle.Bold);
            this.btn_SelectedPrev.MainFontColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(36)))), ((int)(((byte)(0)))));
            this.btn_SelectedPrev.Name = "btn_SelectedPrev";
            this.btn_SelectedPrev.Size = new System.Drawing.Size(65, 50);
            this.btn_SelectedPrev.SubFont = new System.Drawing.Font("맑은 고딕", 8F, System.Drawing.FontStyle.Bold);
            this.btn_SelectedPrev.SubFontColor = System.Drawing.Color.DarkBlue;
            this.btn_SelectedPrev.SubText = "STATUS";
            this.btn_SelectedPrev.TabIndex = 2;
            this.btn_SelectedPrev.Text = "<<";
            this.btn_SelectedPrev.TextAlignMain = Sys3Controls.EN_TEXTALIGN.MIDDLE_CENTER;
            this.btn_SelectedPrev.TextAlignSub = Sys3Controls.EN_TEXTALIGN.TOP_LEFT;
            this.btn_SelectedPrev.ThemeIndex = 0;
            this.btn_SelectedPrev.UseBorder = true;
            this.btn_SelectedPrev.UseClickedEmphasizeTextColor = false;
            this.btn_SelectedPrev.UseCustomizeClickedColor = false;
            this.btn_SelectedPrev.UseEdge = true;
            this.btn_SelectedPrev.UseHoverEmphasizeCustomColor = false;
            this.btn_SelectedPrev.UseImage = false;
            this.btn_SelectedPrev.UserHoverEmpahsize = false;
            this.btn_SelectedPrev.UseSubFont = false;
            this.btn_SelectedPrev.Click += new System.EventHandler(this.Click_MovePage);
            // 
            // btn_SelectedPage
            // 
            this.btn_SelectedPage.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btn_SelectedPage.BorderWidth = 2;
            this.btn_SelectedPage.ButtonClicked = false;
            this.btn_SelectedPage.ClickedEmphasizeTextColor = System.Drawing.Color.White;
            this.btn_SelectedPage.CustomClickedGradientFirstColor = System.Drawing.Color.White;
            this.btn_SelectedPage.CustomClickedGradientSecondColor = System.Drawing.Color.White;
            this.btn_SelectedPage.Description = "";
            this.btn_SelectedPage.DisabledColor = System.Drawing.Color.DarkGray;
            this.btn_SelectedPage.EdgeRadius = 5;
            this.btn_SelectedPage.GradientAngle = 60F;
            this.btn_SelectedPage.GradientFirstColor = System.Drawing.Color.White;
            this.btn_SelectedPage.GradientSecondColor = System.Drawing.Color.FromArgb(((int)(((byte)(190)))), ((int)(((byte)(196)))), ((int)(((byte)(203)))));
            this.btn_SelectedPage.HoverEmphasizeCustomColor = System.Drawing.Color.White;
            this.btn_SelectedPage.ImagePosition = new System.Drawing.Point(7, 7);
            this.btn_SelectedPage.ImageSize = new System.Drawing.Point(30, 30);
            this.btn_SelectedPage.LoadImage = global::FrameOfSystem3.Properties.Resources.CONFIG_ADD3;
            this.btn_SelectedPage.Location = new System.Drawing.Point(83, 65);
            this.btn_SelectedPage.MainFont = new System.Drawing.Font("맑은 고딕", 14F, System.Drawing.FontStyle.Bold);
            this.btn_SelectedPage.MainFontColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(36)))), ((int)(((byte)(0)))));
            this.btn_SelectedPage.Name = "btn_SelectedPage";
            this.btn_SelectedPage.Size = new System.Drawing.Size(55, 50);
            this.btn_SelectedPage.SubFont = new System.Drawing.Font("맑은 고딕", 14F, System.Drawing.FontStyle.Bold);
            this.btn_SelectedPage.SubFontColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(36)))), ((int)(((byte)(0)))));
            this.btn_SelectedPage.SubText = "/ 1";
            this.btn_SelectedPage.TabIndex = 1375;
            this.btn_SelectedPage.Text = "1";
            this.btn_SelectedPage.TextAlignMain = Sys3Controls.EN_TEXTALIGN.MIDDLE_LEFT;
            this.btn_SelectedPage.TextAlignSub = Sys3Controls.EN_TEXTALIGN.MIDDLE_RIGHT;
            this.btn_SelectedPage.ThemeIndex = 0;
            this.btn_SelectedPage.UseBorder = true;
            this.btn_SelectedPage.UseClickedEmphasizeTextColor = false;
            this.btn_SelectedPage.UseCustomizeClickedColor = false;
            this.btn_SelectedPage.UseEdge = true;
            this.btn_SelectedPage.UseHoverEmphasizeCustomColor = false;
            this.btn_SelectedPage.UseImage = false;
            this.btn_SelectedPage.UserHoverEmpahsize = false;
            this.btn_SelectedPage.UseSubFont = true;
            this.btn_SelectedPage.Click += new System.EventHandler(this.Click_MovePage);
            // 
            // btn_SelectedNext
            // 
            this.btn_SelectedNext.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btn_SelectedNext.BorderWidth = 2;
            this.btn_SelectedNext.ButtonClicked = false;
            this.btn_SelectedNext.ClickedEmphasizeTextColor = System.Drawing.Color.White;
            this.btn_SelectedNext.CustomClickedGradientFirstColor = System.Drawing.Color.White;
            this.btn_SelectedNext.CustomClickedGradientSecondColor = System.Drawing.Color.White;
            this.btn_SelectedNext.Description = "";
            this.btn_SelectedNext.DisabledColor = System.Drawing.Color.DarkGray;
            this.btn_SelectedNext.EdgeRadius = 5;
            this.btn_SelectedNext.GradientAngle = 60F;
            this.btn_SelectedNext.GradientFirstColor = System.Drawing.Color.White;
            this.btn_SelectedNext.GradientSecondColor = System.Drawing.Color.FromArgb(((int)(((byte)(190)))), ((int)(((byte)(196)))), ((int)(((byte)(203)))));
            this.btn_SelectedNext.HoverEmphasizeCustomColor = System.Drawing.Color.White;
            this.btn_SelectedNext.ImagePosition = new System.Drawing.Point(7, 7);
            this.btn_SelectedNext.ImageSize = new System.Drawing.Point(30, 30);
            this.btn_SelectedNext.LoadImage = global::FrameOfSystem3.Properties.Resources.CONFIG_ADD3;
            this.btn_SelectedNext.Location = new System.Drawing.Point(144, 65);
            this.btn_SelectedNext.MainFont = new System.Drawing.Font("맑은 고딕", 14F, System.Drawing.FontStyle.Bold);
            this.btn_SelectedNext.MainFontColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(36)))), ((int)(((byte)(0)))));
            this.btn_SelectedNext.Name = "btn_SelectedNext";
            this.btn_SelectedNext.Size = new System.Drawing.Size(65, 50);
            this.btn_SelectedNext.SubFont = new System.Drawing.Font("맑은 고딕", 8F, System.Drawing.FontStyle.Bold);
            this.btn_SelectedNext.SubFontColor = System.Drawing.Color.DarkBlue;
            this.btn_SelectedNext.SubText = "STATUS";
            this.btn_SelectedNext.TabIndex = 3;
            this.btn_SelectedNext.Text = ">>";
            this.btn_SelectedNext.TextAlignMain = Sys3Controls.EN_TEXTALIGN.MIDDLE_CENTER;
            this.btn_SelectedNext.TextAlignSub = Sys3Controls.EN_TEXTALIGN.TOP_LEFT;
            this.btn_SelectedNext.ThemeIndex = 0;
            this.btn_SelectedNext.UseBorder = true;
            this.btn_SelectedNext.UseClickedEmphasizeTextColor = false;
            this.btn_SelectedNext.UseCustomizeClickedColor = false;
            this.btn_SelectedNext.UseEdge = true;
            this.btn_SelectedNext.UseHoverEmphasizeCustomColor = false;
            this.btn_SelectedNext.UseImage = false;
            this.btn_SelectedNext.UserHoverEmpahsize = false;
            this.btn_SelectedNext.UseSubFont = false;
            this.btn_SelectedNext.Click += new System.EventHandler(this.Click_MovePage);
            // 
            // btn_ChoiceNext
            // 
            this.btn_ChoiceNext.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btn_ChoiceNext.BorderWidth = 2;
            this.btn_ChoiceNext.ButtonClicked = false;
            this.btn_ChoiceNext.ClickedEmphasizeTextColor = System.Drawing.Color.White;
            this.btn_ChoiceNext.CustomClickedGradientFirstColor = System.Drawing.Color.White;
            this.btn_ChoiceNext.CustomClickedGradientSecondColor = System.Drawing.Color.White;
            this.btn_ChoiceNext.Description = "";
            this.btn_ChoiceNext.DisabledColor = System.Drawing.Color.DarkGray;
            this.btn_ChoiceNext.EdgeRadius = 5;
            this.btn_ChoiceNext.GradientAngle = 60F;
            this.btn_ChoiceNext.GradientFirstColor = System.Drawing.Color.White;
            this.btn_ChoiceNext.GradientSecondColor = System.Drawing.Color.FromArgb(((int)(((byte)(190)))), ((int)(((byte)(196)))), ((int)(((byte)(203)))));
            this.btn_ChoiceNext.HoverEmphasizeCustomColor = System.Drawing.Color.White;
            this.btn_ChoiceNext.ImagePosition = new System.Drawing.Point(7, 7);
            this.btn_ChoiceNext.ImageSize = new System.Drawing.Point(30, 30);
            this.btn_ChoiceNext.LoadImage = global::FrameOfSystem3.Properties.Resources.CONFIG_ADD3;
            this.btn_ChoiceNext.Location = new System.Drawing.Point(144, 65);
            this.btn_ChoiceNext.MainFont = new System.Drawing.Font("맑은 고딕", 14F, System.Drawing.FontStyle.Bold);
            this.btn_ChoiceNext.MainFontColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(36)))), ((int)(((byte)(0)))));
            this.btn_ChoiceNext.Name = "btn_ChoiceNext";
            this.btn_ChoiceNext.Size = new System.Drawing.Size(65, 50);
            this.btn_ChoiceNext.SubFont = new System.Drawing.Font("맑은 고딕", 8F, System.Drawing.FontStyle.Bold);
            this.btn_ChoiceNext.SubFontColor = System.Drawing.Color.DarkBlue;
            this.btn_ChoiceNext.SubText = "STATUS";
            this.btn_ChoiceNext.TabIndex = 1;
            this.btn_ChoiceNext.Text = ">>";
            this.btn_ChoiceNext.TextAlignMain = Sys3Controls.EN_TEXTALIGN.MIDDLE_CENTER;
            this.btn_ChoiceNext.TextAlignSub = Sys3Controls.EN_TEXTALIGN.TOP_LEFT;
            this.btn_ChoiceNext.ThemeIndex = 0;
            this.btn_ChoiceNext.UseBorder = true;
            this.btn_ChoiceNext.UseClickedEmphasizeTextColor = false;
            this.btn_ChoiceNext.UseCustomizeClickedColor = false;
            this.btn_ChoiceNext.UseEdge = true;
            this.btn_ChoiceNext.UseHoverEmphasizeCustomColor = false;
            this.btn_ChoiceNext.UseImage = false;
            this.btn_ChoiceNext.UserHoverEmpahsize = false;
            this.btn_ChoiceNext.UseSubFont = false;
            this.btn_ChoiceNext.Click += new System.EventHandler(this.Click_MovePage);
            // 
            // btn_ChoicePage
            // 
            this.btn_ChoicePage.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btn_ChoicePage.BorderWidth = 2;
            this.btn_ChoicePage.ButtonClicked = false;
            this.btn_ChoicePage.ClickedEmphasizeTextColor = System.Drawing.Color.White;
            this.btn_ChoicePage.CustomClickedGradientFirstColor = System.Drawing.Color.White;
            this.btn_ChoicePage.CustomClickedGradientSecondColor = System.Drawing.Color.White;
            this.btn_ChoicePage.Description = "";
            this.btn_ChoicePage.DisabledColor = System.Drawing.Color.DarkGray;
            this.btn_ChoicePage.EdgeRadius = 5;
            this.btn_ChoicePage.GradientAngle = 60F;
            this.btn_ChoicePage.GradientFirstColor = System.Drawing.Color.White;
            this.btn_ChoicePage.GradientSecondColor = System.Drawing.Color.FromArgb(((int)(((byte)(190)))), ((int)(((byte)(196)))), ((int)(((byte)(203)))));
            this.btn_ChoicePage.HoverEmphasizeCustomColor = System.Drawing.Color.White;
            this.btn_ChoicePage.ImagePosition = new System.Drawing.Point(7, 7);
            this.btn_ChoicePage.ImageSize = new System.Drawing.Point(30, 30);
            this.btn_ChoicePage.LoadImage = global::FrameOfSystem3.Properties.Resources.CONFIG_ADD3;
            this.btn_ChoicePage.Location = new System.Drawing.Point(83, 65);
            this.btn_ChoicePage.MainFont = new System.Drawing.Font("맑은 고딕", 14F, System.Drawing.FontStyle.Bold);
            this.btn_ChoicePage.MainFontColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(36)))), ((int)(((byte)(0)))));
            this.btn_ChoicePage.Name = "btn_ChoicePage";
            this.btn_ChoicePage.Size = new System.Drawing.Size(55, 50);
            this.btn_ChoicePage.SubFont = new System.Drawing.Font("맑은 고딕", 14F, System.Drawing.FontStyle.Bold);
            this.btn_ChoicePage.SubFontColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(36)))), ((int)(((byte)(0)))));
            this.btn_ChoicePage.SubText = "/ 1";
            this.btn_ChoicePage.TabIndex = 1377;
            this.btn_ChoicePage.Text = "1";
            this.btn_ChoicePage.TextAlignMain = Sys3Controls.EN_TEXTALIGN.MIDDLE_LEFT;
            this.btn_ChoicePage.TextAlignSub = Sys3Controls.EN_TEXTALIGN.MIDDLE_RIGHT;
            this.btn_ChoicePage.ThemeIndex = 0;
            this.btn_ChoicePage.UseBorder = true;
            this.btn_ChoicePage.UseClickedEmphasizeTextColor = false;
            this.btn_ChoicePage.UseCustomizeClickedColor = false;
            this.btn_ChoicePage.UseEdge = true;
            this.btn_ChoicePage.UseHoverEmphasizeCustomColor = false;
            this.btn_ChoicePage.UseImage = false;
            this.btn_ChoicePage.UserHoverEmpahsize = false;
            this.btn_ChoicePage.UseSubFont = true;
            this.btn_ChoicePage.Click += new System.EventHandler(this.Click_MovePage);
            // 
            // btn_ChoicePrev
            // 
            this.btn_ChoicePrev.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btn_ChoicePrev.BorderWidth = 2;
            this.btn_ChoicePrev.ButtonClicked = false;
            this.btn_ChoicePrev.ClickedEmphasizeTextColor = System.Drawing.Color.White;
            this.btn_ChoicePrev.CustomClickedGradientFirstColor = System.Drawing.Color.White;
            this.btn_ChoicePrev.CustomClickedGradientSecondColor = System.Drawing.Color.White;
            this.btn_ChoicePrev.Description = "";
            this.btn_ChoicePrev.DisabledColor = System.Drawing.Color.DarkGray;
            this.btn_ChoicePrev.EdgeRadius = 5;
            this.btn_ChoicePrev.GradientAngle = 60F;
            this.btn_ChoicePrev.GradientFirstColor = System.Drawing.Color.White;
            this.btn_ChoicePrev.GradientSecondColor = System.Drawing.Color.FromArgb(((int)(((byte)(190)))), ((int)(((byte)(196)))), ((int)(((byte)(203)))));
            this.btn_ChoicePrev.HoverEmphasizeCustomColor = System.Drawing.Color.White;
            this.btn_ChoicePrev.ImagePosition = new System.Drawing.Point(7, 7);
            this.btn_ChoicePrev.ImageSize = new System.Drawing.Point(30, 30);
            this.btn_ChoicePrev.LoadImage = global::FrameOfSystem3.Properties.Resources.CONFIG_ADD3;
            this.btn_ChoicePrev.Location = new System.Drawing.Point(12, 65);
            this.btn_ChoicePrev.MainFont = new System.Drawing.Font("맑은 고딕", 14F, System.Drawing.FontStyle.Bold);
            this.btn_ChoicePrev.MainFontColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(36)))), ((int)(((byte)(0)))));
            this.btn_ChoicePrev.Name = "btn_ChoicePrev";
            this.btn_ChoicePrev.Size = new System.Drawing.Size(65, 50);
            this.btn_ChoicePrev.SubFont = new System.Drawing.Font("맑은 고딕", 8F, System.Drawing.FontStyle.Bold);
            this.btn_ChoicePrev.SubFontColor = System.Drawing.Color.DarkBlue;
            this.btn_ChoicePrev.SubText = "STATUS";
            this.btn_ChoicePrev.TabIndex = 0;
            this.btn_ChoicePrev.Text = "<<";
            this.btn_ChoicePrev.TextAlignMain = Sys3Controls.EN_TEXTALIGN.MIDDLE_CENTER;
            this.btn_ChoicePrev.TextAlignSub = Sys3Controls.EN_TEXTALIGN.TOP_LEFT;
            this.btn_ChoicePrev.ThemeIndex = 0;
            this.btn_ChoicePrev.UseBorder = true;
            this.btn_ChoicePrev.UseClickedEmphasizeTextColor = false;
            this.btn_ChoicePrev.UseCustomizeClickedColor = false;
            this.btn_ChoicePrev.UseEdge = true;
            this.btn_ChoicePrev.UseHoverEmphasizeCustomColor = false;
            this.btn_ChoicePrev.UseImage = false;
            this.btn_ChoicePrev.UserHoverEmpahsize = false;
            this.btn_ChoicePrev.UseSubFont = false;
            this.btn_ChoicePrev.Click += new System.EventHandler(this.Click_MovePage);
            // 
            // m_btnApply
            // 
            this.m_btnApply.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.m_btnApply.BorderWidth = 3;
            this.m_btnApply.ButtonClicked = false;
            this.m_btnApply.ClickedEmphasizeTextColor = System.Drawing.Color.White;
            this.m_btnApply.CustomClickedGradientFirstColor = System.Drawing.Color.White;
            this.m_btnApply.CustomClickedGradientSecondColor = System.Drawing.Color.White;
            this.m_btnApply.Description = "";
            this.m_btnApply.DisabledColor = System.Drawing.Color.DarkGray;
            this.m_btnApply.EdgeRadius = 5;
            this.m_btnApply.GradientAngle = 70F;
            this.m_btnApply.GradientFirstColor = System.Drawing.Color.White;
            this.m_btnApply.GradientSecondColor = System.Drawing.Color.LightSlateGray;
            this.m_btnApply.HoverEmphasizeCustomColor = System.Drawing.Color.White;
            this.m_btnApply.ImagePosition = new System.Drawing.Point(10, 10);
            this.m_btnApply.ImageSize = new System.Drawing.Point(30, 30);
            this.m_btnApply.LoadImage = global::FrameOfSystem3.Properties.Resources.CLEAR_BLACK;
            this.m_btnApply.Location = new System.Drawing.Point(210, 65);
            this.m_btnApply.MainFont = new System.Drawing.Font("맑은 고딕", 12.5F, System.Drawing.FontStyle.Bold);
            this.m_btnApply.MainFontColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(36)))), ((int)(((byte)(0)))));
            this.m_btnApply.Name = "m_btnApply";
            this.m_btnApply.Size = new System.Drawing.Size(95, 50);
            this.m_btnApply.SubFont = new System.Drawing.Font("맑은 고딕", 8F, System.Drawing.FontStyle.Bold);
            this.m_btnApply.SubFontColor = System.Drawing.Color.DarkBlue;
            this.m_btnApply.SubText = "MOVE";
            this.m_btnApply.TabIndex = 0;
            this.m_btnApply.Text = "APPLY";
            this.m_btnApply.TextAlignMain = Sys3Controls.EN_TEXTALIGN.MIDDLE_CENTER;
            this.m_btnApply.TextAlignSub = Sys3Controls.EN_TEXTALIGN.TOP_RIGHT;
            this.m_btnApply.ThemeIndex = 0;
            this.m_btnApply.UseBorder = true;
            this.m_btnApply.UseClickedEmphasizeTextColor = false;
            this.m_btnApply.UseCustomizeClickedColor = false;
            this.m_btnApply.UseEdge = true;
            this.m_btnApply.UseHoverEmphasizeCustomColor = false;
            this.m_btnApply.UseImage = false;
            this.m_btnApply.UserHoverEmpahsize = false;
            this.m_btnApply.UseSubFont = false;
            this.m_btnApply.Click += new System.EventHandler(this.Click_ApplyOrCancel);
            // 
            // m_btnCancel
            // 
            this.m_btnCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.m_btnCancel.BorderWidth = 3;
            this.m_btnCancel.ButtonClicked = false;
            this.m_btnCancel.ClickedEmphasizeTextColor = System.Drawing.Color.White;
            this.m_btnCancel.CustomClickedGradientFirstColor = System.Drawing.Color.White;
            this.m_btnCancel.CustomClickedGradientSecondColor = System.Drawing.Color.White;
            this.m_btnCancel.Description = "";
            this.m_btnCancel.DisabledColor = System.Drawing.Color.DarkGray;
            this.m_btnCancel.EdgeRadius = 5;
            this.m_btnCancel.GradientAngle = 70F;
            this.m_btnCancel.GradientFirstColor = System.Drawing.Color.White;
            this.m_btnCancel.GradientSecondColor = System.Drawing.Color.LightSlateGray;
            this.m_btnCancel.HoverEmphasizeCustomColor = System.Drawing.Color.White;
            this.m_btnCancel.ImagePosition = new System.Drawing.Point(10, 10);
            this.m_btnCancel.ImageSize = new System.Drawing.Point(30, 30);
            this.m_btnCancel.LoadImage = global::FrameOfSystem3.Properties.Resources.CLEAR_BLACK;
            this.m_btnCancel.Location = new System.Drawing.Point(311, 65);
            this.m_btnCancel.MainFont = new System.Drawing.Font("맑은 고딕", 12.5F, System.Drawing.FontStyle.Bold);
            this.m_btnCancel.MainFontColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(36)))), ((int)(((byte)(0)))));
            this.m_btnCancel.Name = "m_btnCancel";
            this.m_btnCancel.Size = new System.Drawing.Size(95, 50);
            this.m_btnCancel.SubFont = new System.Drawing.Font("맑은 고딕", 8F, System.Drawing.FontStyle.Bold);
            this.m_btnCancel.SubFontColor = System.Drawing.Color.DarkBlue;
            this.m_btnCancel.SubText = "MOVE";
            this.m_btnCancel.TabIndex = 1;
            this.m_btnCancel.Text = "CANCEL";
            this.m_btnCancel.TextAlignMain = Sys3Controls.EN_TEXTALIGN.MIDDLE_CENTER;
            this.m_btnCancel.TextAlignSub = Sys3Controls.EN_TEXTALIGN.TOP_RIGHT;
            this.m_btnCancel.ThemeIndex = 0;
            this.m_btnCancel.UseBorder = true;
            this.m_btnCancel.UseClickedEmphasizeTextColor = false;
            this.m_btnCancel.UseCustomizeClickedColor = false;
            this.m_btnCancel.UseEdge = true;
            this.m_btnCancel.UseHoverEmphasizeCustomColor = false;
            this.m_btnCancel.UseImage = false;
            this.m_btnCancel.UserHoverEmpahsize = false;
            this.m_btnCancel.UseSubFont = false;
            this.m_btnCancel.Click += new System.EventHandler(this.Click_ApplyOrCancel);
            // 
            // btn_AllSelect
            // 
            this.btn_AllSelect.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btn_AllSelect.BorderWidth = 3;
            this.btn_AllSelect.ButtonClicked = false;
            this.btn_AllSelect.ClickedEmphasizeTextColor = System.Drawing.Color.White;
            this.btn_AllSelect.CustomClickedGradientFirstColor = System.Drawing.Color.White;
            this.btn_AllSelect.CustomClickedGradientSecondColor = System.Drawing.Color.White;
            this.btn_AllSelect.Description = "";
            this.btn_AllSelect.DisabledColor = System.Drawing.Color.DarkGray;
            this.btn_AllSelect.EdgeRadius = 5;
            this.btn_AllSelect.GradientAngle = 70F;
            this.btn_AllSelect.GradientFirstColor = System.Drawing.Color.White;
            this.btn_AllSelect.GradientSecondColor = System.Drawing.Color.LightSlateGray;
            this.btn_AllSelect.HoverEmphasizeCustomColor = System.Drawing.Color.White;
            this.btn_AllSelect.ImagePosition = new System.Drawing.Point(10, 10);
            this.btn_AllSelect.ImageSize = new System.Drawing.Point(30, 30);
            this.btn_AllSelect.LoadImage = global::FrameOfSystem3.Properties.Resources.CLEAR_BLACK;
            this.btn_AllSelect.Location = new System.Drawing.Point(315, 3);
            this.btn_AllSelect.MainFont = new System.Drawing.Font("맑은 고딕", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.btn_AllSelect.MainFontColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(36)))), ((int)(((byte)(0)))));
            this.btn_AllSelect.Name = "btn_AllSelect";
            this.btn_AllSelect.Size = new System.Drawing.Size(95, 44);
            this.btn_AllSelect.SubFont = new System.Drawing.Font("맑은 고딕", 8F, System.Drawing.FontStyle.Bold);
            this.btn_AllSelect.SubFontColor = System.Drawing.Color.DarkBlue;
            this.btn_AllSelect.SubText = "MOVE";
            this.btn_AllSelect.TabIndex = 1;
            this.btn_AllSelect.Text = "SELECT\\nALL";
            this.btn_AllSelect.TextAlignMain = Sys3Controls.EN_TEXTALIGN.MIDDLE_CENTER;
            this.btn_AllSelect.TextAlignSub = Sys3Controls.EN_TEXTALIGN.TOP_RIGHT;
            this.btn_AllSelect.ThemeIndex = 0;
            this.btn_AllSelect.UseBorder = true;
            this.btn_AllSelect.UseClickedEmphasizeTextColor = false;
            this.btn_AllSelect.UseCustomizeClickedColor = false;
            this.btn_AllSelect.UseEdge = true;
            this.btn_AllSelect.UseHoverEmphasizeCustomColor = false;
            this.btn_AllSelect.UseImage = false;
            this.btn_AllSelect.UserHoverEmpahsize = false;
            this.btn_AllSelect.UseSubFont = false;
            this.btn_AllSelect.Click += new System.EventHandler(this.Click_SelectButton);
            // 
            // btn_AllUnSelect
            // 
            this.btn_AllUnSelect.BorderWidth = 3;
            this.btn_AllUnSelect.ButtonClicked = false;
            this.btn_AllUnSelect.ClickedEmphasizeTextColor = System.Drawing.Color.White;
            this.btn_AllUnSelect.CustomClickedGradientFirstColor = System.Drawing.Color.White;
            this.btn_AllUnSelect.CustomClickedGradientSecondColor = System.Drawing.Color.White;
            this.btn_AllUnSelect.Description = "";
            this.btn_AllUnSelect.DisabledColor = System.Drawing.Color.DarkGray;
            this.btn_AllUnSelect.EdgeRadius = 5;
            this.btn_AllUnSelect.GradientAngle = 70F;
            this.btn_AllUnSelect.GradientFirstColor = System.Drawing.Color.White;
            this.btn_AllUnSelect.GradientSecondColor = System.Drawing.Color.LightSlateGray;
            this.btn_AllUnSelect.HoverEmphasizeCustomColor = System.Drawing.Color.White;
            this.btn_AllUnSelect.ImagePosition = new System.Drawing.Point(10, 10);
            this.btn_AllUnSelect.ImageSize = new System.Drawing.Point(30, 30);
            this.btn_AllUnSelect.LoadImage = global::FrameOfSystem3.Properties.Resources.CLEAR_BLACK;
            this.btn_AllUnSelect.Location = new System.Drawing.Point(9, 3);
            this.btn_AllUnSelect.MainFont = new System.Drawing.Font("맑은 고딕", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.btn_AllUnSelect.MainFontColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(36)))), ((int)(((byte)(0)))));
            this.btn_AllUnSelect.Name = "btn_AllUnSelect";
            this.btn_AllUnSelect.Size = new System.Drawing.Size(95, 44);
            this.btn_AllUnSelect.SubFont = new System.Drawing.Font("맑은 고딕", 8F, System.Drawing.FontStyle.Bold);
            this.btn_AllUnSelect.SubFontColor = System.Drawing.Color.DarkBlue;
            this.btn_AllUnSelect.SubText = "MOVE";
            this.btn_AllUnSelect.TabIndex = 0;
            this.btn_AllUnSelect.Text = "UNSELECT ALL";
            this.btn_AllUnSelect.TextAlignMain = Sys3Controls.EN_TEXTALIGN.MIDDLE_CENTER;
            this.btn_AllUnSelect.TextAlignSub = Sys3Controls.EN_TEXTALIGN.TOP_RIGHT;
            this.btn_AllUnSelect.ThemeIndex = 0;
            this.btn_AllUnSelect.UseBorder = true;
            this.btn_AllUnSelect.UseClickedEmphasizeTextColor = false;
            this.btn_AllUnSelect.UseCustomizeClickedColor = false;
            this.btn_AllUnSelect.UseEdge = true;
            this.btn_AllUnSelect.UseHoverEmphasizeCustomColor = false;
            this.btn_AllUnSelect.UseImage = false;
            this.btn_AllUnSelect.UserHoverEmpahsize = false;
            this.btn_AllUnSelect.UseSubFont = false;
            this.btn_AllUnSelect.Click += new System.EventHandler(this.Click_SelectButton);
            // 
            // lbl_Filtering
            // 
            this.lbl_Filtering.BackGroundColor = System.Drawing.Color.WhiteSmoke;
            this.lbl_Filtering.BorderStroke = 2;
            this.lbl_Filtering.BorderStyle = System.Windows.Forms.ButtonBorderStyle.Solid;
            this.lbl_Filtering.Description = "";
            this.lbl_Filtering.DisabledColor = System.Drawing.Color.Silver;
            this.lbl_Filtering.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lbl_Filtering.EdgeRadius = 1;
            this.lbl_Filtering.ImagePosition = new System.Drawing.Point(0, 0);
            this.lbl_Filtering.ImageSize = new System.Drawing.Point(0, 0);
            this.lbl_Filtering.LoadImage = null;
            this.lbl_Filtering.Location = new System.Drawing.Point(420, 0);
            this.lbl_Filtering.MainFont = new System.Drawing.Font("맑은 고딕", 12F, System.Drawing.FontStyle.Bold);
            this.lbl_Filtering.MainFontColor = System.Drawing.Color.Red;
            this.lbl_Filtering.Margin = new System.Windows.Forms.Padding(0);
            this.lbl_Filtering.Name = "lbl_Filtering";
            this.lbl_Filtering.Size = new System.Drawing.Size(420, 40);
            this.lbl_Filtering.SubFont = new System.Drawing.Font("맑은 고딕", 8F, System.Drawing.FontStyle.Bold);
            this.lbl_Filtering.SubFontColor = System.Drawing.Color.DodgerBlue;
            this.lbl_Filtering.SubText = "FILTERING";
            this.lbl_Filtering.TabIndex = 1378;
            this.lbl_Filtering.TextAlignMain = Sys3Controls.EN_TEXTALIGN.MIDDLE_CENTER;
            this.lbl_Filtering.TextAlignSub = Sys3Controls.EN_TEXTALIGN.TOP_LEFT;
            this.lbl_Filtering.TextAlignUnit = Sys3Controls.EN_TEXTALIGN.MIDDLE_CENTER;
            this.lbl_Filtering.ThemeIndex = 0;
            this.lbl_Filtering.UnitAreaRate = 40;
            this.lbl_Filtering.UnitFont = new System.Drawing.Font("맑은 고딕", 10F, System.Drawing.FontStyle.Bold);
            this.lbl_Filtering.UnitFontColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.lbl_Filtering.UnitPositionVertical = false;
            this.lbl_Filtering.UnitText = "";
            this.lbl_Filtering.UseBorder = true;
            this.lbl_Filtering.UseEdgeRadius = false;
            this.lbl_Filtering.UseImage = false;
            this.lbl_Filtering.UseSubFont = true;
            this.lbl_Filtering.UseUnitFont = false;
            this.lbl_Filtering.Click += new System.EventHandler(this.lbl_Filtering_Click);
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 2;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel1.Controls.Add(this._lbl_Title, 0, 1);
            this.tableLayoutPanel1.Controls.Add(this._gbSelectionItems, 1, 2);
            this.tableLayoutPanel1.Controls.Add(this._gbSelectedItems, 0, 2);
            this.tableLayoutPanel1.Controls.Add(this._gb_Header, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.lbl_Filtering, 1, 0);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel1.Margin = new System.Windows.Forms.Padding(0);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 3;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 40F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 30F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(840, 200);
            this.tableLayoutPanel1.TabIndex = 1379;
            // 
            // _gbSelectionItems
            // 
            this._gbSelectionItems.BackGroundColor = System.Drawing.Color.WhiteSmoke;
            this._gbSelectionItems.Controls.Add(this.btn_PreviousSet);
            this._gbSelectionItems.Controls.Add(this.btn_AllSelect);
            this._gbSelectionItems.Controls.Add(this.m_btnCancel);
            this._gbSelectionItems.Controls.Add(this.btn_ChoicePage);
            this._gbSelectionItems.Controls.Add(this.m_btnApply);
            this._gbSelectionItems.Controls.Add(this.btn_ChoicePrev);
            this._gbSelectionItems.Controls.Add(this.btn_ChoiceNext);
            this._gbSelectionItems.Dock = System.Windows.Forms.DockStyle.Fill;
            this._gbSelectionItems.EdgeBorderStroke = 2;
            this._gbSelectionItems.EdgeRadius = 2;
            this._gbSelectionItems.LabelFont = new System.Drawing.Font("맑은 고딕", 15F, System.Drawing.FontStyle.Bold);
            this._gbSelectionItems.LabelGradientColorFirst = System.Drawing.Color.FromArgb(((int)(((byte)(208)))), ((int)(((byte)(208)))), ((int)(((byte)(208)))));
            this._gbSelectionItems.LabelGradientColorSecond = System.Drawing.Color.FromArgb(((int)(((byte)(240)))), ((int)(((byte)(240)))), ((int)(((byte)(240)))));
            this._gbSelectionItems.LabelHeight = 50;
            this._gbSelectionItems.LabelTextColor = System.Drawing.Color.Black;
            this._gbSelectionItems.Location = new System.Drawing.Point(420, 70);
            this._gbSelectionItems.Margin = new System.Windows.Forms.Padding(0);
            this._gbSelectionItems.Name = "_gbSelectionItems";
            this._gbSelectionItems.Padding = new System.Windows.Forms.Padding(0);
            this._gbSelectionItems.Size = new System.Drawing.Size(420, 130);
            this._gbSelectionItems.TabIndex = 1381;
            this._gbSelectionItems.TabStop = false;
            this._gbSelectionItems.TextAlign = Sys3Controls.EN_TEXTALIGN.MIDDLE_CENTER;
            this._gbSelectionItems.ThemeIndex = 0;
            this._gbSelectionItems.UseLabelBorder = true;
            this._gbSelectionItems.UseTitle = true;
            // 
            // btn_PreviousSet
            // 
            this.btn_PreviousSet.BorderWidth = 3;
            this.btn_PreviousSet.ButtonClicked = false;
            this.btn_PreviousSet.ClickedEmphasizeTextColor = System.Drawing.Color.White;
            this.btn_PreviousSet.CustomClickedGradientFirstColor = System.Drawing.Color.White;
            this.btn_PreviousSet.CustomClickedGradientSecondColor = System.Drawing.Color.White;
            this.btn_PreviousSet.Description = "";
            this.btn_PreviousSet.DisabledColor = System.Drawing.Color.DarkGray;
            this.btn_PreviousSet.EdgeRadius = 5;
            this.btn_PreviousSet.GradientAngle = 70F;
            this.btn_PreviousSet.GradientFirstColor = System.Drawing.Color.LightCyan;
            this.btn_PreviousSet.GradientSecondColor = System.Drawing.Color.SkyBlue;
            this.btn_PreviousSet.HoverEmphasizeCustomColor = System.Drawing.Color.White;
            this.btn_PreviousSet.ImagePosition = new System.Drawing.Point(10, 10);
            this.btn_PreviousSet.ImageSize = new System.Drawing.Point(30, 30);
            this.btn_PreviousSet.LoadImage = global::FrameOfSystem3.Properties.Resources.CLEAR_BLACK;
            this.btn_PreviousSet.Location = new System.Drawing.Point(3, 3);
            this.btn_PreviousSet.MainFont = new System.Drawing.Font("맑은 고딕", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.btn_PreviousSet.MainFontColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(36)))), ((int)(((byte)(0)))));
            this.btn_PreviousSet.Name = "btn_PreviousSet";
            this.btn_PreviousSet.Size = new System.Drawing.Size(214, 44);
            this.btn_PreviousSet.SubFont = new System.Drawing.Font("맑은 고딕", 8F, System.Drawing.FontStyle.Bold);
            this.btn_PreviousSet.SubFontColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(36)))), ((int)(((byte)(0)))));
            this.btn_PreviousSet.SubText = "yyyy-MM-dd HH:mm:ss";
            this.btn_PreviousSet.TabIndex = 1380;
            this.btn_PreviousSet.Text = "PREVIOUS";
            this.btn_PreviousSet.TextAlignMain = Sys3Controls.EN_TEXTALIGN.TOP_LEFT;
            this.btn_PreviousSet.TextAlignSub = Sys3Controls.EN_TEXTALIGN.BOTTOM_RIGHT;
            this.btn_PreviousSet.ThemeIndex = 0;
            this.btn_PreviousSet.UseBorder = true;
            this.btn_PreviousSet.UseClickedEmphasizeTextColor = false;
            this.btn_PreviousSet.UseCustomizeClickedColor = false;
            this.btn_PreviousSet.UseEdge = true;
            this.btn_PreviousSet.UseHoverEmphasizeCustomColor = false;
            this.btn_PreviousSet.UseImage = false;
            this.btn_PreviousSet.UserHoverEmpahsize = false;
            this.btn_PreviousSet.UseSubFont = true;
            this.btn_PreviousSet.Click += new System.EventHandler(this.Click_PreviousSet);
            // 
            // _gbSelectedItems
            // 
            this._gbSelectedItems.BackGroundColor = System.Drawing.Color.WhiteSmoke;
            this._gbSelectedItems.Controls.Add(this.btn_AllUnSelect);
            this._gbSelectedItems.Controls.Add(this.btn_SelectedPrev);
            this._gbSelectedItems.Controls.Add(this.btn_SelectedPage);
            this._gbSelectedItems.Controls.Add(this.btn_SelectedNext);
            this._gbSelectedItems.Dock = System.Windows.Forms.DockStyle.Fill;
            this._gbSelectedItems.EdgeBorderStroke = 2;
            this._gbSelectedItems.EdgeRadius = 2;
            this._gbSelectedItems.LabelFont = new System.Drawing.Font("맑은 고딕", 15F, System.Drawing.FontStyle.Bold);
            this._gbSelectedItems.LabelGradientColorFirst = System.Drawing.Color.FromArgb(((int)(((byte)(208)))), ((int)(((byte)(208)))), ((int)(((byte)(208)))));
            this._gbSelectedItems.LabelGradientColorSecond = System.Drawing.Color.FromArgb(((int)(((byte)(240)))), ((int)(((byte)(240)))), ((int)(((byte)(240)))));
            this._gbSelectedItems.LabelHeight = 50;
            this._gbSelectedItems.LabelTextColor = System.Drawing.Color.Black;
            this._gbSelectedItems.Location = new System.Drawing.Point(0, 70);
            this._gbSelectedItems.Margin = new System.Windows.Forms.Padding(0);
            this._gbSelectedItems.Name = "_gbSelectedItems";
            this._gbSelectedItems.Padding = new System.Windows.Forms.Padding(0);
            this._gbSelectedItems.Size = new System.Drawing.Size(420, 130);
            this._gbSelectedItems.TabIndex = 1380;
            this._gbSelectedItems.TabStop = false;
            this._gbSelectedItems.Text = "SELECTED ITEMS";
            this._gbSelectedItems.TextAlign = Sys3Controls.EN_TEXTALIGN.MIDDLE_CENTER;
            this._gbSelectedItems.ThemeIndex = 0;
            this._gbSelectedItems.UseLabelBorder = true;
            this._gbSelectedItems.UseTitle = true;
            // 
            // _gb_Header
            // 
            this._gb_Header.BackGroundColor = System.Drawing.Color.WhiteSmoke;
            this._gb_Header.Dock = System.Windows.Forms.DockStyle.Fill;
            this._gb_Header.EdgeBorderStroke = 2;
            this._gb_Header.EdgeRadius = 2;
            this._gb_Header.LabelFont = new System.Drawing.Font("맑은 고딕", 15F, System.Drawing.FontStyle.Bold);
            this._gb_Header.LabelGradientColorFirst = System.Drawing.Color.FromArgb(((int)(((byte)(208)))), ((int)(((byte)(208)))), ((int)(((byte)(208)))));
            this._gb_Header.LabelGradientColorSecond = System.Drawing.Color.FromArgb(((int)(((byte)(240)))), ((int)(((byte)(240)))), ((int)(((byte)(240)))));
            this._gb_Header.LabelHeight = 40;
            this._gb_Header.LabelTextColor = System.Drawing.Color.Black;
            this._gb_Header.Location = new System.Drawing.Point(0, 0);
            this._gb_Header.Margin = new System.Windows.Forms.Padding(0);
            this._gb_Header.Name = "_gb_Header";
            this._gb_Header.Size = new System.Drawing.Size(420, 40);
            this._gb_Header.TabIndex = 1380;
            this._gb_Header.Text = "SELECTION LIST";
            this._gb_Header.TextAlign = Sys3Controls.EN_TEXTALIGN.MIDDLE_LEFT;
            this._gb_Header.ThemeIndex = 0;
            this._gb_Header.UseLabelBorder = true;
            this._gb_Header.MouseDown += new System.Windows.Forms.MouseEventHandler(this.MouseDown_Title);
            this._gb_Header.MouseMove += new System.Windows.Forms.MouseEventHandler(this.MouseMove_Title);
            this._gb_Header.MouseUp += new System.Windows.Forms.MouseEventHandler(this.MouseUp_Title);
            // 
            // _lbl_Title
            // 
            this._lbl_Title.BackGroundColor = System.Drawing.Color.WhiteSmoke;
            this._lbl_Title.BorderStroke = 2;
            this._lbl_Title.BorderStyle = System.Windows.Forms.ButtonBorderStyle.Solid;
            this.tableLayoutPanel1.SetColumnSpan(this._lbl_Title, 2);
            this._lbl_Title.Description = "";
            this._lbl_Title.DisabledColor = System.Drawing.Color.Silver;
            this._lbl_Title.Dock = System.Windows.Forms.DockStyle.Fill;
            this._lbl_Title.EdgeRadius = 1;
            this._lbl_Title.ImagePosition = new System.Drawing.Point(0, 0);
            this._lbl_Title.ImageSize = new System.Drawing.Point(0, 0);
            this._lbl_Title.LoadImage = null;
            this._lbl_Title.Location = new System.Drawing.Point(0, 40);
            this._lbl_Title.MainFont = new System.Drawing.Font("맑은 고딕", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this._lbl_Title.MainFontColor = System.Drawing.Color.Black;
            this._lbl_Title.Margin = new System.Windows.Forms.Padding(0);
            this._lbl_Title.Name = "_lbl_Title";
            this._lbl_Title.Size = new System.Drawing.Size(840, 30);
            this._lbl_Title.SubFont = new System.Drawing.Font("맑은 고딕", 8F, System.Drawing.FontStyle.Bold);
            this._lbl_Title.SubFontColor = System.Drawing.Color.DodgerBlue;
            this._lbl_Title.SubText = "";
            this._lbl_Title.TabIndex = 1382;
            this._lbl_Title.Text = "Parameter Name";
            this._lbl_Title.TextAlignMain = Sys3Controls.EN_TEXTALIGN.MIDDLE_CENTER;
            this._lbl_Title.TextAlignSub = Sys3Controls.EN_TEXTALIGN.TOP_LEFT;
            this._lbl_Title.TextAlignUnit = Sys3Controls.EN_TEXTALIGN.MIDDLE_CENTER;
            this._lbl_Title.ThemeIndex = 0;
            this._lbl_Title.UnitAreaRate = 40;
            this._lbl_Title.UnitFont = new System.Drawing.Font("맑은 고딕", 10F, System.Drawing.FontStyle.Bold);
            this._lbl_Title.UnitFontColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this._lbl_Title.UnitPositionVertical = false;
            this._lbl_Title.UnitText = "";
            this._lbl_Title.UseBorder = true;
            this._lbl_Title.UseEdgeRadius = false;
            this._lbl_Title.UseImage = false;
            this._lbl_Title.UseSubFont = true;
            this._lbl_Title.UseUnitFont = false;
            // 
            // Form_SelectionList
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(80)))), ((int)(((byte)(80)))), ((int)(((byte)(80)))));
            this.ClientSize = new System.Drawing.Size(840, 200);
            this.ControlBox = false;
            this.Controls.Add(this.tableLayoutPanel1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.KeyPreview = true;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "Form_SelectionList";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Form_SelectionList";
            this.TopMost = true;
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.Form_SelectionList_KeyDown);
            this.tableLayoutPanel1.ResumeLayout(false);
            this._gbSelectionItems.ResumeLayout(false);
            this._gbSelectedItems.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion
		private Sys3Controls.Sys3button btn_SelectedPrev;
		private Sys3Controls.Sys3button btn_SelectedPage;
		private Sys3Controls.Sys3button btn_SelectedNext;
		private Sys3Controls.Sys3button btn_ChoiceNext;
		private Sys3Controls.Sys3button btn_ChoicePage;
		private Sys3Controls.Sys3button btn_ChoicePrev;
		private Sys3Controls.Sys3button m_btnApply;
		private Sys3Controls.Sys3button m_btnCancel;
		private Sys3Controls.Sys3button btn_AllSelect;
		private Sys3Controls.Sys3button btn_AllUnSelect;
		private Sys3Controls.Sys3Label lbl_Filtering;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private Sys3Controls.Sys3GroupBox _gb_Header;
        private Sys3Controls.Sys3GroupBoxContainer _gbSelectedItems;
        private Sys3Controls.Sys3GroupBoxContainer _gbSelectionItems;
        private Sys3Controls.Sys3button btn_PreviousSet;
        private Sys3Controls.Sys3Label _lbl_Title;
    }
}