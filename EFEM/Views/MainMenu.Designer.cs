namespace FrameOfSystem3.Views
{
    partial class MainMenu
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
            this.EN_RECIPE = new Sys3Controls.Sys3button();
            this.EN_SETUP = new Sys3Controls.Sys3button();
            this.EN_EXIT = new Sys3Controls.Sys3button();
            this.EN_OPERATION = new Sys3Controls.Sys3button();
            this.EN_History = new Sys3Controls.Sys3button();
            this.EN_CONFIG = new Sys3Controls.Sys3button();
            this.EN_USER = new Sys3Controls.Sys3button();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.tableLayoutPanel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // EN_RECIPE
            // 
            this.EN_RECIPE.BorderWidth = 2;
            this.EN_RECIPE.ButtonClicked = false;
            this.EN_RECIPE.ClickedEmphasizeTextColor = System.Drawing.Color.White;
            this.EN_RECIPE.CustomClickedGradientFirstColor = System.Drawing.Color.DimGray;
            this.EN_RECIPE.CustomClickedGradientSecondColor = System.Drawing.Color.DimGray;
            this.EN_RECIPE.Description = "";
            this.EN_RECIPE.DisabledColor = System.Drawing.Color.DarkGray;
            this.EN_RECIPE.Dock = System.Windows.Forms.DockStyle.Fill;
            this.EN_RECIPE.EdgeRadius = 5;
            this.EN_RECIPE.GradientAngle = 80F;
            this.EN_RECIPE.GradientFirstColor = System.Drawing.Color.White;
            this.EN_RECIPE.GradientSecondColor = System.Drawing.Color.Silver;
            this.EN_RECIPE.HoverEmphasizeCustomColor = System.Drawing.Color.Firebrick;
            this.EN_RECIPE.ImagePosition = new System.Drawing.Point(25, 10);
            this.EN_RECIPE.ImageSize = new System.Drawing.Point(30, 30);
            this.EN_RECIPE.LoadImage = global::FrameOfSystem3.Properties.Resources.Main_Recipe_blueblack;
            this.EN_RECIPE.Location = new System.Drawing.Point(187, 3);
            this.EN_RECIPE.MainFont = new System.Drawing.Font("맑은 고딕", 11F, System.Drawing.FontStyle.Bold);
            this.EN_RECIPE.MainFontColor = System.Drawing.Color.Black;
            this.EN_RECIPE.Margin = new System.Windows.Forms.Padding(5, 3, 5, 3);
            this.EN_RECIPE.Name = "EN_RECIPE";
            this.EN_RECIPE.Size = new System.Drawing.Size(172, 52);
            this.EN_RECIPE.SubFont = new System.Drawing.Font("맑은 고딕", 10F);
            this.EN_RECIPE.SubFontColor = System.Drawing.Color.Black;
            this.EN_RECIPE.SubText = "";
            this.EN_RECIPE.TabIndex = 1;
            this.EN_RECIPE.Text = "　     RECIPE";
            this.EN_RECIPE.TextAlignMain = Sys3Controls.EN_TEXTALIGN.MIDDLE_CENTER;
            this.EN_RECIPE.TextAlignSub = Sys3Controls.EN_TEXTALIGN.MIDDLE_CENTER;
            this.EN_RECIPE.ThemeIndex = 0;
            this.EN_RECIPE.UseBorder = false;
            this.EN_RECIPE.UseClickedEmphasizeTextColor = false;
            this.EN_RECIPE.UseCustomizeClickedColor = true;
            this.EN_RECIPE.UseEdge = false;
            this.EN_RECIPE.UseHoverEmphasizeCustomColor = true;
            this.EN_RECIPE.UseImage = true;
            this.EN_RECIPE.UserHoverEmpahsize = true;
            this.EN_RECIPE.UseSubFont = false;
            this.EN_RECIPE.Click += new System.EventHandler(this.ClickMainMenu);
            // 
            // EN_SETUP
            // 
            this.EN_SETUP.BorderWidth = 2;
            this.EN_SETUP.ButtonClicked = false;
            this.EN_SETUP.ClickedEmphasizeTextColor = System.Drawing.Color.White;
            this.EN_SETUP.CustomClickedGradientFirstColor = System.Drawing.Color.DimGray;
            this.EN_SETUP.CustomClickedGradientSecondColor = System.Drawing.Color.DimGray;
            this.EN_SETUP.Description = "";
            this.EN_SETUP.DisabledColor = System.Drawing.Color.DarkGray;
            this.EN_SETUP.Dock = System.Windows.Forms.DockStyle.Fill;
            this.EN_SETUP.EdgeRadius = 5;
            this.EN_SETUP.GradientAngle = 80F;
            this.EN_SETUP.GradientFirstColor = System.Drawing.Color.White;
            this.EN_SETUP.GradientSecondColor = System.Drawing.Color.Silver;
            this.EN_SETUP.HoverEmphasizeCustomColor = System.Drawing.Color.Firebrick;
            this.EN_SETUP.ImagePosition = new System.Drawing.Point(25, 10);
            this.EN_SETUP.ImageSize = new System.Drawing.Point(30, 30);
            this.EN_SETUP.LoadImage = global::FrameOfSystem3.Properties.Resources.Main_Setup_blueblack;
            this.EN_SETUP.Location = new System.Drawing.Point(551, 3);
            this.EN_SETUP.MainFont = new System.Drawing.Font("맑은 고딕", 11F, System.Drawing.FontStyle.Bold);
            this.EN_SETUP.MainFontColor = System.Drawing.Color.Black;
            this.EN_SETUP.Margin = new System.Windows.Forms.Padding(5, 3, 5, 3);
            this.EN_SETUP.Name = "EN_SETUP";
            this.EN_SETUP.Size = new System.Drawing.Size(172, 52);
            this.EN_SETUP.SubFont = new System.Drawing.Font("맑은 고딕", 10F);
            this.EN_SETUP.SubFontColor = System.Drawing.Color.Black;
            this.EN_SETUP.SubText = "";
            this.EN_SETUP.TabIndex = 3;
            this.EN_SETUP.Text = "　     SETUP";
            this.EN_SETUP.TextAlignMain = Sys3Controls.EN_TEXTALIGN.MIDDLE_CENTER;
            this.EN_SETUP.TextAlignSub = Sys3Controls.EN_TEXTALIGN.MIDDLE_CENTER;
            this.EN_SETUP.ThemeIndex = 0;
            this.EN_SETUP.UseBorder = false;
            this.EN_SETUP.UseClickedEmphasizeTextColor = false;
            this.EN_SETUP.UseCustomizeClickedColor = true;
            this.EN_SETUP.UseEdge = false;
            this.EN_SETUP.UseHoverEmphasizeCustomColor = true;
            this.EN_SETUP.UseImage = true;
            this.EN_SETUP.UserHoverEmpahsize = true;
            this.EN_SETUP.UseSubFont = false;
            this.EN_SETUP.Click += new System.EventHandler(this.ClickMainMenu);
            // 
            // EN_EXIT
            // 
            this.EN_EXIT.BorderWidth = 2;
            this.EN_EXIT.ButtonClicked = false;
            this.EN_EXIT.ClickedEmphasizeTextColor = System.Drawing.Color.White;
            this.EN_EXIT.CustomClickedGradientFirstColor = System.Drawing.Color.DimGray;
            this.EN_EXIT.CustomClickedGradientSecondColor = System.Drawing.Color.DimGray;
            this.EN_EXIT.Description = "";
            this.EN_EXIT.DisabledColor = System.Drawing.Color.DarkGray;
            this.EN_EXIT.Dock = System.Windows.Forms.DockStyle.Fill;
            this.EN_EXIT.EdgeRadius = 5;
            this.EN_EXIT.GradientAngle = 80F;
            this.EN_EXIT.GradientFirstColor = System.Drawing.Color.White;
            this.EN_EXIT.GradientSecondColor = System.Drawing.Color.Silver;
            this.EN_EXIT.HoverEmphasizeCustomColor = System.Drawing.Color.Firebrick;
            this.EN_EXIT.ImagePosition = new System.Drawing.Point(25, 10);
            this.EN_EXIT.ImageSize = new System.Drawing.Point(30, 30);
            this.EN_EXIT.LoadImage = global::FrameOfSystem3.Properties.Resources.Main_Exit_blueblack;
            this.EN_EXIT.Location = new System.Drawing.Point(1097, 3);
            this.EN_EXIT.MainFont = new System.Drawing.Font("맑은 고딕", 11F, System.Drawing.FontStyle.Bold);
            this.EN_EXIT.MainFontColor = System.Drawing.Color.Black;
            this.EN_EXIT.Margin = new System.Windows.Forms.Padding(5, 3, 5, 3);
            this.EN_EXIT.Name = "EN_EXIT";
            this.EN_EXIT.Size = new System.Drawing.Size(178, 52);
            this.EN_EXIT.SubFont = new System.Drawing.Font("맑은 고딕", 10F);
            this.EN_EXIT.SubFontColor = System.Drawing.Color.Black;
            this.EN_EXIT.SubText = "";
            this.EN_EXIT.TabIndex = 6;
            this.EN_EXIT.Text = "　  EXIT";
            this.EN_EXIT.TextAlignMain = Sys3Controls.EN_TEXTALIGN.MIDDLE_CENTER;
            this.EN_EXIT.TextAlignSub = Sys3Controls.EN_TEXTALIGN.MIDDLE_CENTER;
            this.EN_EXIT.ThemeIndex = 0;
            this.EN_EXIT.UseBorder = false;
            this.EN_EXIT.UseClickedEmphasizeTextColor = false;
            this.EN_EXIT.UseCustomizeClickedColor = true;
            this.EN_EXIT.UseEdge = false;
            this.EN_EXIT.UseHoverEmphasizeCustomColor = true;
            this.EN_EXIT.UseImage = true;
            this.EN_EXIT.UserHoverEmpahsize = true;
            this.EN_EXIT.UseSubFont = false;
            this.EN_EXIT.Click += new System.EventHandler(this.ClickMainMenu);
            // 
            // EN_OPERATION
            // 
            this.EN_OPERATION.BorderWidth = 2;
            this.EN_OPERATION.ButtonClicked = true;
            this.EN_OPERATION.ClickedEmphasizeTextColor = System.Drawing.Color.White;
            this.EN_OPERATION.CustomClickedGradientFirstColor = System.Drawing.Color.DimGray;
            this.EN_OPERATION.CustomClickedGradientSecondColor = System.Drawing.Color.DimGray;
            this.EN_OPERATION.Description = "";
            this.EN_OPERATION.DisabledColor = System.Drawing.Color.DarkGray;
            this.EN_OPERATION.Dock = System.Windows.Forms.DockStyle.Fill;
            this.EN_OPERATION.EdgeRadius = 5;
            this.EN_OPERATION.GradientAngle = 80F;
            this.EN_OPERATION.GradientFirstColor = System.Drawing.Color.White;
            this.EN_OPERATION.GradientSecondColor = System.Drawing.Color.Silver;
            this.EN_OPERATION.HoverEmphasizeCustomColor = System.Drawing.Color.Firebrick;
            this.EN_OPERATION.ImagePosition = new System.Drawing.Point(25, 10);
            this.EN_OPERATION.ImageSize = new System.Drawing.Point(30, 30);
            this.EN_OPERATION.LoadImage = global::FrameOfSystem3.Properties.Resources.Main_Operation_blueBlack;
            this.EN_OPERATION.Location = new System.Drawing.Point(5, 3);
            this.EN_OPERATION.MainFont = new System.Drawing.Font("맑은 고딕", 11F, System.Drawing.FontStyle.Bold);
            this.EN_OPERATION.MainFontColor = System.Drawing.Color.Black;
            this.EN_OPERATION.Margin = new System.Windows.Forms.Padding(5, 3, 5, 3);
            this.EN_OPERATION.Name = "EN_OPERATION";
            this.EN_OPERATION.Size = new System.Drawing.Size(172, 52);
            this.EN_OPERATION.SubFont = new System.Drawing.Font("맑은 고딕", 10F);
            this.EN_OPERATION.SubFontColor = System.Drawing.Color.Black;
            this.EN_OPERATION.SubText = "";
            this.EN_OPERATION.TabIndex = 0;
            this.EN_OPERATION.Text = "　     OPERATION";
            this.EN_OPERATION.TextAlignMain = Sys3Controls.EN_TEXTALIGN.MIDDLE_CENTER;
            this.EN_OPERATION.TextAlignSub = Sys3Controls.EN_TEXTALIGN.MIDDLE_CENTER;
            this.EN_OPERATION.ThemeIndex = 0;
            this.EN_OPERATION.UseBorder = false;
            this.EN_OPERATION.UseClickedEmphasizeTextColor = false;
            this.EN_OPERATION.UseCustomizeClickedColor = true;
            this.EN_OPERATION.UseEdge = false;
            this.EN_OPERATION.UseHoverEmphasizeCustomColor = true;
            this.EN_OPERATION.UseImage = true;
            this.EN_OPERATION.UserHoverEmpahsize = true;
            this.EN_OPERATION.UseSubFont = false;
            this.EN_OPERATION.Click += new System.EventHandler(this.ClickMainMenu);
            // 
            // EN_History
            // 
            this.EN_History.BorderWidth = 2;
            this.EN_History.ButtonClicked = false;
            this.EN_History.ClickedEmphasizeTextColor = System.Drawing.Color.White;
            this.EN_History.CustomClickedGradientFirstColor = System.Drawing.Color.DimGray;
            this.EN_History.CustomClickedGradientSecondColor = System.Drawing.Color.DimGray;
            this.EN_History.Description = "";
            this.EN_History.DisabledColor = System.Drawing.Color.DarkGray;
            this.EN_History.Dock = System.Windows.Forms.DockStyle.Fill;
            this.EN_History.EdgeRadius = 5;
            this.EN_History.GradientAngle = 80F;
            this.EN_History.GradientFirstColor = System.Drawing.Color.White;
            this.EN_History.GradientSecondColor = System.Drawing.Color.Silver;
            this.EN_History.HoverEmphasizeCustomColor = System.Drawing.Color.Firebrick;
            this.EN_History.ImagePosition = new System.Drawing.Point(25, 10);
            this.EN_History.ImageSize = new System.Drawing.Point(30, 30);
            this.EN_History.LoadImage = global::FrameOfSystem3.Properties.Resources.Main_History_blueblack;
            this.EN_History.Location = new System.Drawing.Point(369, 3);
            this.EN_History.MainFont = new System.Drawing.Font("맑은 고딕", 11F, System.Drawing.FontStyle.Bold);
            this.EN_History.MainFontColor = System.Drawing.Color.Black;
            this.EN_History.Margin = new System.Windows.Forms.Padding(5, 3, 5, 3);
            this.EN_History.Name = "EN_History";
            this.EN_History.Size = new System.Drawing.Size(172, 52);
            this.EN_History.SubFont = new System.Drawing.Font("맑은 고딕", 10F);
            this.EN_History.SubFontColor = System.Drawing.Color.Black;
            this.EN_History.SubText = "";
            this.EN_History.TabIndex = 2;
            this.EN_History.Text = "　     HISTORY";
            this.EN_History.TextAlignMain = Sys3Controls.EN_TEXTALIGN.MIDDLE_CENTER;
            this.EN_History.TextAlignSub = Sys3Controls.EN_TEXTALIGN.MIDDLE_CENTER;
            this.EN_History.ThemeIndex = 0;
            this.EN_History.UseBorder = false;
            this.EN_History.UseClickedEmphasizeTextColor = false;
            this.EN_History.UseCustomizeClickedColor = true;
            this.EN_History.UseEdge = false;
            this.EN_History.UseHoverEmphasizeCustomColor = true;
            this.EN_History.UseImage = true;
            this.EN_History.UserHoverEmpahsize = true;
            this.EN_History.UseSubFont = false;
            this.EN_History.Click += new System.EventHandler(this.ClickMainMenu);
            // 
            // EN_CONFIG
            // 
            this.EN_CONFIG.BorderWidth = 2;
            this.EN_CONFIG.ButtonClicked = false;
            this.EN_CONFIG.ClickedEmphasizeTextColor = System.Drawing.Color.White;
            this.EN_CONFIG.CustomClickedGradientFirstColor = System.Drawing.Color.DimGray;
            this.EN_CONFIG.CustomClickedGradientSecondColor = System.Drawing.Color.DimGray;
            this.EN_CONFIG.Description = "";
            this.EN_CONFIG.DisabledColor = System.Drawing.Color.DarkGray;
            this.EN_CONFIG.Dock = System.Windows.Forms.DockStyle.Fill;
            this.EN_CONFIG.EdgeRadius = 5;
            this.EN_CONFIG.GradientAngle = 80F;
            this.EN_CONFIG.GradientFirstColor = System.Drawing.Color.White;
            this.EN_CONFIG.GradientSecondColor = System.Drawing.Color.Silver;
            this.EN_CONFIG.HoverEmphasizeCustomColor = System.Drawing.Color.Firebrick;
            this.EN_CONFIG.ImagePosition = new System.Drawing.Point(25, 10);
            this.EN_CONFIG.ImageSize = new System.Drawing.Point(30, 30);
            this.EN_CONFIG.LoadImage = global::FrameOfSystem3.Properties.Resources.Main_Config_blueblack;
            this.EN_CONFIG.Location = new System.Drawing.Point(733, 3);
            this.EN_CONFIG.MainFont = new System.Drawing.Font("맑은 고딕", 11F, System.Drawing.FontStyle.Bold);
            this.EN_CONFIG.MainFontColor = System.Drawing.Color.Black;
            this.EN_CONFIG.Margin = new System.Windows.Forms.Padding(5, 3, 5, 3);
            this.EN_CONFIG.Name = "EN_CONFIG";
            this.EN_CONFIG.Size = new System.Drawing.Size(172, 52);
            this.EN_CONFIG.SubFont = new System.Drawing.Font("맑은 고딕", 10F);
            this.EN_CONFIG.SubFontColor = System.Drawing.Color.Black;
            this.EN_CONFIG.SubText = "";
            this.EN_CONFIG.TabIndex = 4;
            this.EN_CONFIG.Text = "　     CONFIG";
            this.EN_CONFIG.TextAlignMain = Sys3Controls.EN_TEXTALIGN.MIDDLE_CENTER;
            this.EN_CONFIG.TextAlignSub = Sys3Controls.EN_TEXTALIGN.MIDDLE_CENTER;
            this.EN_CONFIG.ThemeIndex = 0;
            this.EN_CONFIG.UseBorder = false;
            this.EN_CONFIG.UseClickedEmphasizeTextColor = false;
            this.EN_CONFIG.UseCustomizeClickedColor = true;
            this.EN_CONFIG.UseEdge = false;
            this.EN_CONFIG.UseHoverEmphasizeCustomColor = true;
            this.EN_CONFIG.UseImage = true;
            this.EN_CONFIG.UserHoverEmpahsize = true;
            this.EN_CONFIG.UseSubFont = false;
            this.EN_CONFIG.Click += new System.EventHandler(this.ClickMainMenu);
            // 
            // EN_USER
            // 
            this.EN_USER.BorderWidth = 2;
            this.EN_USER.ButtonClicked = false;
            this.EN_USER.ClickedEmphasizeTextColor = System.Drawing.Color.White;
            this.EN_USER.CustomClickedGradientFirstColor = System.Drawing.Color.DimGray;
            this.EN_USER.CustomClickedGradientSecondColor = System.Drawing.Color.DimGray;
            this.EN_USER.Description = "";
            this.EN_USER.DisabledColor = System.Drawing.Color.DarkGray;
            this.EN_USER.Dock = System.Windows.Forms.DockStyle.Fill;
            this.EN_USER.EdgeRadius = 5;
            this.EN_USER.GradientAngle = 80F;
            this.EN_USER.GradientFirstColor = System.Drawing.Color.White;
            this.EN_USER.GradientSecondColor = System.Drawing.Color.Silver;
            this.EN_USER.HoverEmphasizeCustomColor = System.Drawing.Color.Firebrick;
            this.EN_USER.ImagePosition = new System.Drawing.Point(25, 10);
            this.EN_USER.ImageSize = new System.Drawing.Point(30, 30);
            this.EN_USER.LoadImage = global::FrameOfSystem3.Properties.Resources.Main_User_blueblack;
            this.EN_USER.Location = new System.Drawing.Point(915, 3);
            this.EN_USER.MainFont = new System.Drawing.Font("맑은 고딕", 11F, System.Drawing.FontStyle.Bold);
            this.EN_USER.MainFontColor = System.Drawing.Color.Black;
            this.EN_USER.Margin = new System.Windows.Forms.Padding(5, 3, 5, 3);
            this.EN_USER.Name = "EN_USER";
            this.EN_USER.Size = new System.Drawing.Size(172, 52);
            this.EN_USER.SubFont = new System.Drawing.Font("맑은 고딕", 10F);
            this.EN_USER.SubFontColor = System.Drawing.Color.Black;
            this.EN_USER.SubText = "";
            this.EN_USER.TabIndex = 5;
            this.EN_USER.Text = "　  USER";
            this.EN_USER.TextAlignMain = Sys3Controls.EN_TEXTALIGN.MIDDLE_CENTER;
            this.EN_USER.TextAlignSub = Sys3Controls.EN_TEXTALIGN.MIDDLE_CENTER;
            this.EN_USER.ThemeIndex = 0;
            this.EN_USER.UseBorder = false;
            this.EN_USER.UseClickedEmphasizeTextColor = false;
            this.EN_USER.UseCustomizeClickedColor = true;
            this.EN_USER.UseEdge = false;
            this.EN_USER.UseHoverEmphasizeCustomColor = true;
            this.EN_USER.UseImage = true;
            this.EN_USER.UserHoverEmpahsize = true;
            this.EN_USER.UseSubFont = false;
            this.EN_USER.Click += new System.EventHandler(this.ClickMainMenu);
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 7;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 14.28571F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 14.28571F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 14.28571F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 14.28571F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 14.28571F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 14.28571F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 14.28571F));
            this.tableLayoutPanel1.Controls.Add(this.EN_OPERATION, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.EN_RECIPE, 1, 0);
            this.tableLayoutPanel1.Controls.Add(this.EN_History, 2, 0);
            this.tableLayoutPanel1.Controls.Add(this.EN_SETUP, 3, 0);
            this.tableLayoutPanel1.Controls.Add(this.EN_CONFIG, 4, 0);
            this.tableLayoutPanel1.Controls.Add(this.EN_USER, 5, 0);
            this.tableLayoutPanel1.Controls.Add(this.EN_EXIT, 6, 0);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 1;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(1280, 58);
            this.tableLayoutPanel1.TabIndex = 14;
            // 
            // MainMenu
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.BackColor = System.Drawing.Color.DarkGray;
            this.Controls.Add(this.tableLayoutPanel1);
            this.Margin = new System.Windows.Forms.Padding(0);
            this.Name = "MainMenu";
            this.Size = new System.Drawing.Size(1280, 58);
            this.tableLayoutPanel1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

		private Sys3Controls.Sys3button EN_USER;
		private Sys3Controls.Sys3button EN_CONFIG;
		private Sys3Controls.Sys3button EN_History;
		private Sys3Controls.Sys3button EN_OPERATION;
		private Sys3Controls.Sys3button EN_EXIT;
		private Sys3Controls.Sys3button EN_SETUP;
        private Sys3Controls.Sys3button EN_RECIPE;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;

    }
}
