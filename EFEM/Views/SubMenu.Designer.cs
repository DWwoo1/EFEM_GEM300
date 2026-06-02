namespace FrameOfSystem3.Views
{
    partial class SubMenu
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
			this.m_groupColorPalette = new Sys3Controls.Sys3GroupBox();
			this.m_btn1 = new Sys3Controls.Sys3button();
			this.m_btn2 = new Sys3Controls.Sys3button();
			this.m_btn3 = new Sys3Controls.Sys3button();
			this.m_btn4 = new Sys3Controls.Sys3button();
			this.m_btn5 = new Sys3Controls.Sys3button();
			this.m_btn7 = new Sys3Controls.Sys3button();
			this.m_btn6 = new Sys3Controls.Sys3button();
			this.m_btn8 = new Sys3Controls.Sys3button();
			this.m_btn10 = new Sys3Controls.Sys3button();
			this.m_btn9 = new Sys3Controls.Sys3button();
			this.m_btn11 = new Sys3Controls.Sys3button();
			this.m_btn13 = new Sys3Controls.Sys3button();
			this.m_btn12 = new Sys3Controls.Sys3button();
			this.m_btn14 = new Sys3Controls.Sys3button();
			this.m_btn15 = new Sys3Controls.Sys3button();
			this.m_btn16 = new Sys3Controls.Sys3button();
			this.SuspendLayout();
			// 
			// m_groupColorPalette
			// 
			this.m_groupColorPalette.BackGroundColor = System.Drawing.Color.WhiteSmoke;
			this.m_groupColorPalette.Dock = System.Windows.Forms.DockStyle.Fill;
			this.m_groupColorPalette.EdgeBorderStroke = 2;
			this.m_groupColorPalette.EdgeRadius = 2;
			this.m_groupColorPalette.LabelFont = new System.Drawing.Font("맑은 고딕", 12.75F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Italic))));
			this.m_groupColorPalette.LabelGradientColorFirst = System.Drawing.Color.DarkGray;
			this.m_groupColorPalette.LabelGradientColorSecond = System.Drawing.Color.DimGray;
			this.m_groupColorPalette.LabelHeight = 30;
			this.m_groupColorPalette.LabelTextColor = System.Drawing.Color.White;
			this.m_groupColorPalette.Location = new System.Drawing.Point(0, 0);
			this.m_groupColorPalette.Name = "m_groupColorPalette";
			this.m_groupColorPalette.Size = new System.Drawing.Size(140, 898);
			this.m_groupColorPalette.TabIndex = 1373;
			this.m_groupColorPalette.Text = "SUB MENU";
			this.m_groupColorPalette.TextAlign = Sys3Controls.EN_TEXTALIGN.MIDDLE_CENTER;
			this.m_groupColorPalette.ThemeIndex = 0;
			this.m_groupColorPalette.UseLabelBorder = true;
			// 
			// m_btn1
			// 
			this.m_btn1.BorderWidth = 2;
			this.m_btn1.ButtonClicked = true;
			this.m_btn1.ClickedEmphasizeTextColor = System.Drawing.Color.White;
			this.m_btn1.CustomClickedGradientFirstColor = System.Drawing.Color.Gray;
			this.m_btn1.CustomClickedGradientSecondColor = System.Drawing.Color.Gray;
			this.m_btn1.Description = "";
			this.m_btn1.DisabledColor = System.Drawing.Color.DarkGray;
			this.m_btn1.EdgeRadius = 5;
			this.m_btn1.GradientAngle = 70F;
			this.m_btn1.GradientFirstColor = System.Drawing.Color.White;
			this.m_btn1.GradientSecondColor = System.Drawing.Color.White;
			this.m_btn1.HoverEmphasizeCustomColor = System.Drawing.Color.Firebrick;
			this.m_btn1.ImagePosition = new System.Drawing.Point(7, 7);
			this.m_btn1.ImageSize = new System.Drawing.Point(30, 30);
			this.m_btn1.LoadImage = global::FrameOfSystem3.Properties.Resources.CONNECT_BLACK;
			this.m_btn1.Location = new System.Drawing.Point(5, 37);
			this.m_btn1.MainFont = new System.Drawing.Font("맑은 고딕", 11F, System.Drawing.FontStyle.Bold);
			this.m_btn1.MainFontColor = System.Drawing.Color.FromArgb(((int)(((byte)(124)))), ((int)(((byte)(123)))), ((int)(((byte)(131)))));
			this.m_btn1.Name = "m_btn1";
			this.m_btn1.Size = new System.Drawing.Size(130, 50);
			this.m_btn1.SubFont = new System.Drawing.Font("맑은 고딕", 8F, System.Drawing.FontStyle.Bold);
			this.m_btn1.SubFontColor = System.Drawing.Color.DarkBlue;
			this.m_btn1.SubText = "MOVE";
			this.m_btn1.TabIndex = 0;
			this.m_btn1.Text = "OPERATION";
			this.m_btn1.TextAlignMain = Sys3Controls.EN_TEXTALIGN.MIDDLE_CENTER;
			this.m_btn1.TextAlignSub = Sys3Controls.EN_TEXTALIGN.TOP_RIGHT;
			this.m_btn1.ThemeIndex = 0;
			this.m_btn1.UseBorder = false;
			this.m_btn1.UseClickedEmphasizeTextColor = false;
			this.m_btn1.UseCustomizeClickedColor = true;
			this.m_btn1.UseEdge = false;
			this.m_btn1.UseHoverEmphasizeCustomColor = true;
			this.m_btn1.UseImage = false;
			this.m_btn1.UserHoverEmpahsize = true;
			this.m_btn1.UseSubFont = false;
			this.m_btn1.Click += new System.EventHandler(this.ClickButtons);
			// 
			// m_btn2
			// 
			this.m_btn2.BorderWidth = 2;
			this.m_btn2.ButtonClicked = false;
			this.m_btn2.ClickedEmphasizeTextColor = System.Drawing.Color.White;
			this.m_btn2.CustomClickedGradientFirstColor = System.Drawing.Color.Gray;
			this.m_btn2.CustomClickedGradientSecondColor = System.Drawing.Color.Gray;
			this.m_btn2.Description = "";
			this.m_btn2.DisabledColor = System.Drawing.Color.DarkGray;
			this.m_btn2.EdgeRadius = 5;
			this.m_btn2.GradientAngle = 70F;
			this.m_btn2.GradientFirstColor = System.Drawing.Color.White;
			this.m_btn2.GradientSecondColor = System.Drawing.Color.White;
			this.m_btn2.HoverEmphasizeCustomColor = System.Drawing.Color.Firebrick;
			this.m_btn2.ImagePosition = new System.Drawing.Point(7, 7);
			this.m_btn2.ImageSize = new System.Drawing.Point(30, 30);
			this.m_btn2.LoadImage = global::FrameOfSystem3.Properties.Resources.CONNECT_BLACK;
			this.m_btn2.Location = new System.Drawing.Point(5, 90);
			this.m_btn2.MainFont = new System.Drawing.Font("맑은 고딕", 11F, System.Drawing.FontStyle.Bold);
			this.m_btn2.MainFontColor = System.Drawing.Color.FromArgb(((int)(((byte)(124)))), ((int)(((byte)(123)))), ((int)(((byte)(131)))));
			this.m_btn2.Name = "m_btn2";
			this.m_btn2.Size = new System.Drawing.Size(130, 50);
			this.m_btn2.SubFont = new System.Drawing.Font("맑은 고딕", 8F, System.Drawing.FontStyle.Bold);
			this.m_btn2.SubFontColor = System.Drawing.Color.DarkBlue;
			this.m_btn2.SubText = "MOVE";
			this.m_btn2.TabIndex = 1;
			this.m_btn2.Text = "OPERATION";
			this.m_btn2.TextAlignMain = Sys3Controls.EN_TEXTALIGN.MIDDLE_CENTER;
			this.m_btn2.TextAlignSub = Sys3Controls.EN_TEXTALIGN.TOP_RIGHT;
			this.m_btn2.ThemeIndex = 0;
			this.m_btn2.UseBorder = false;
			this.m_btn2.UseClickedEmphasizeTextColor = false;
			this.m_btn2.UseCustomizeClickedColor = true;
			this.m_btn2.UseEdge = false;
			this.m_btn2.UseHoverEmphasizeCustomColor = true;
			this.m_btn2.UseImage = false;
			this.m_btn2.UserHoverEmpahsize = true;
			this.m_btn2.UseSubFont = false;
			this.m_btn2.Click += new System.EventHandler(this.ClickButtons);
			// 
			// m_btn3
			// 
			this.m_btn3.BorderWidth = 2;
			this.m_btn3.ButtonClicked = false;
			this.m_btn3.ClickedEmphasizeTextColor = System.Drawing.Color.White;
			this.m_btn3.CustomClickedGradientFirstColor = System.Drawing.Color.Gray;
			this.m_btn3.CustomClickedGradientSecondColor = System.Drawing.Color.Gray;
			this.m_btn3.Description = "";
			this.m_btn3.DisabledColor = System.Drawing.Color.DarkGray;
			this.m_btn3.EdgeRadius = 5;
			this.m_btn3.GradientAngle = 70F;
			this.m_btn3.GradientFirstColor = System.Drawing.Color.White;
			this.m_btn3.GradientSecondColor = System.Drawing.Color.White;
			this.m_btn3.HoverEmphasizeCustomColor = System.Drawing.Color.Firebrick;
			this.m_btn3.ImagePosition = new System.Drawing.Point(7, 7);
			this.m_btn3.ImageSize = new System.Drawing.Point(30, 30);
			this.m_btn3.LoadImage = global::FrameOfSystem3.Properties.Resources.CONNECT_BLACK;
			this.m_btn3.Location = new System.Drawing.Point(5, 143);
			this.m_btn3.MainFont = new System.Drawing.Font("맑은 고딕", 11F, System.Drawing.FontStyle.Bold);
			this.m_btn3.MainFontColor = System.Drawing.Color.FromArgb(((int)(((byte)(124)))), ((int)(((byte)(123)))), ((int)(((byte)(131)))));
			this.m_btn3.Name = "m_btn3";
			this.m_btn3.Size = new System.Drawing.Size(130, 50);
			this.m_btn3.SubFont = new System.Drawing.Font("맑은 고딕", 8F, System.Drawing.FontStyle.Bold);
			this.m_btn3.SubFontColor = System.Drawing.Color.DarkBlue;
			this.m_btn3.SubText = "MOVE";
			this.m_btn3.TabIndex = 2;
			this.m_btn3.Text = "OPERATION";
			this.m_btn3.TextAlignMain = Sys3Controls.EN_TEXTALIGN.MIDDLE_CENTER;
			this.m_btn3.TextAlignSub = Sys3Controls.EN_TEXTALIGN.TOP_RIGHT;
			this.m_btn3.ThemeIndex = 0;
			this.m_btn3.UseBorder = false;
			this.m_btn3.UseClickedEmphasizeTextColor = false;
			this.m_btn3.UseCustomizeClickedColor = true;
			this.m_btn3.UseEdge = false;
			this.m_btn3.UseHoverEmphasizeCustomColor = true;
			this.m_btn3.UseImage = false;
			this.m_btn3.UserHoverEmpahsize = true;
			this.m_btn3.UseSubFont = false;
			this.m_btn3.Click += new System.EventHandler(this.ClickButtons);
			// 
			// m_btn4
			// 
			this.m_btn4.BorderWidth = 2;
			this.m_btn4.ButtonClicked = false;
			this.m_btn4.ClickedEmphasizeTextColor = System.Drawing.Color.White;
			this.m_btn4.CustomClickedGradientFirstColor = System.Drawing.Color.Gray;
			this.m_btn4.CustomClickedGradientSecondColor = System.Drawing.Color.Gray;
			this.m_btn4.Description = "";
			this.m_btn4.DisabledColor = System.Drawing.Color.DarkGray;
			this.m_btn4.EdgeRadius = 5;
			this.m_btn4.GradientAngle = 70F;
			this.m_btn4.GradientFirstColor = System.Drawing.Color.White;
			this.m_btn4.GradientSecondColor = System.Drawing.Color.White;
			this.m_btn4.HoverEmphasizeCustomColor = System.Drawing.Color.Firebrick;
			this.m_btn4.ImagePosition = new System.Drawing.Point(7, 7);
			this.m_btn4.ImageSize = new System.Drawing.Point(30, 30);
			this.m_btn4.LoadImage = global::FrameOfSystem3.Properties.Resources.CONNECT_BLACK;
			this.m_btn4.Location = new System.Drawing.Point(5, 196);
			this.m_btn4.MainFont = new System.Drawing.Font("맑은 고딕", 11F, System.Drawing.FontStyle.Bold);
			this.m_btn4.MainFontColor = System.Drawing.Color.FromArgb(((int)(((byte)(124)))), ((int)(((byte)(123)))), ((int)(((byte)(131)))));
			this.m_btn4.Name = "m_btn4";
			this.m_btn4.Size = new System.Drawing.Size(130, 50);
			this.m_btn4.SubFont = new System.Drawing.Font("맑은 고딕", 8F, System.Drawing.FontStyle.Bold);
			this.m_btn4.SubFontColor = System.Drawing.Color.DarkBlue;
			this.m_btn4.SubText = "MOVE";
			this.m_btn4.TabIndex = 3;
			this.m_btn4.Text = "OPERATION";
			this.m_btn4.TextAlignMain = Sys3Controls.EN_TEXTALIGN.MIDDLE_CENTER;
			this.m_btn4.TextAlignSub = Sys3Controls.EN_TEXTALIGN.TOP_RIGHT;
			this.m_btn4.ThemeIndex = 0;
			this.m_btn4.UseBorder = false;
			this.m_btn4.UseClickedEmphasizeTextColor = false;
			this.m_btn4.UseCustomizeClickedColor = true;
			this.m_btn4.UseEdge = false;
			this.m_btn4.UseHoverEmphasizeCustomColor = true;
			this.m_btn4.UseImage = false;
			this.m_btn4.UserHoverEmpahsize = true;
			this.m_btn4.UseSubFont = false;
			this.m_btn4.Click += new System.EventHandler(this.ClickButtons);
			// 
			// m_btn5
			// 
			this.m_btn5.BorderWidth = 2;
			this.m_btn5.ButtonClicked = false;
			this.m_btn5.ClickedEmphasizeTextColor = System.Drawing.Color.White;
			this.m_btn5.CustomClickedGradientFirstColor = System.Drawing.Color.Gray;
			this.m_btn5.CustomClickedGradientSecondColor = System.Drawing.Color.Gray;
			this.m_btn5.Description = "";
			this.m_btn5.DisabledColor = System.Drawing.Color.DarkGray;
			this.m_btn5.EdgeRadius = 5;
			this.m_btn5.GradientAngle = 70F;
			this.m_btn5.GradientFirstColor = System.Drawing.Color.White;
			this.m_btn5.GradientSecondColor = System.Drawing.Color.White;
			this.m_btn5.HoverEmphasizeCustomColor = System.Drawing.Color.Firebrick;
			this.m_btn5.ImagePosition = new System.Drawing.Point(7, 7);
			this.m_btn5.ImageSize = new System.Drawing.Point(30, 30);
			this.m_btn5.LoadImage = global::FrameOfSystem3.Properties.Resources.CONNECT_BLACK;
			this.m_btn5.Location = new System.Drawing.Point(5, 249);
			this.m_btn5.MainFont = new System.Drawing.Font("맑은 고딕", 11F, System.Drawing.FontStyle.Bold);
			this.m_btn5.MainFontColor = System.Drawing.Color.FromArgb(((int)(((byte)(124)))), ((int)(((byte)(123)))), ((int)(((byte)(131)))));
			this.m_btn5.Name = "m_btn5";
			this.m_btn5.Size = new System.Drawing.Size(130, 50);
			this.m_btn5.SubFont = new System.Drawing.Font("맑은 고딕", 8F, System.Drawing.FontStyle.Bold);
			this.m_btn5.SubFontColor = System.Drawing.Color.DarkBlue;
			this.m_btn5.SubText = "MOVE";
			this.m_btn5.TabIndex = 4;
			this.m_btn5.Text = "OPERATION";
			this.m_btn5.TextAlignMain = Sys3Controls.EN_TEXTALIGN.MIDDLE_CENTER;
			this.m_btn5.TextAlignSub = Sys3Controls.EN_TEXTALIGN.TOP_RIGHT;
			this.m_btn5.ThemeIndex = 0;
			this.m_btn5.UseBorder = false;
			this.m_btn5.UseClickedEmphasizeTextColor = false;
			this.m_btn5.UseCustomizeClickedColor = true;
			this.m_btn5.UseEdge = false;
			this.m_btn5.UseHoverEmphasizeCustomColor = true;
			this.m_btn5.UseImage = false;
			this.m_btn5.UserHoverEmpahsize = true;
			this.m_btn5.UseSubFont = false;
			this.m_btn5.Click += new System.EventHandler(this.ClickButtons);
			// 
			// m_btn7
			// 
			this.m_btn7.BorderWidth = 2;
			this.m_btn7.ButtonClicked = false;
			this.m_btn7.ClickedEmphasizeTextColor = System.Drawing.Color.White;
			this.m_btn7.CustomClickedGradientFirstColor = System.Drawing.Color.Gray;
			this.m_btn7.CustomClickedGradientSecondColor = System.Drawing.Color.Gray;
			this.m_btn7.Description = "";
			this.m_btn7.DisabledColor = System.Drawing.Color.DarkGray;
			this.m_btn7.EdgeRadius = 5;
			this.m_btn7.GradientAngle = 70F;
			this.m_btn7.GradientFirstColor = System.Drawing.Color.White;
			this.m_btn7.GradientSecondColor = System.Drawing.Color.White;
			this.m_btn7.HoverEmphasizeCustomColor = System.Drawing.Color.Firebrick;
			this.m_btn7.ImagePosition = new System.Drawing.Point(7, 7);
			this.m_btn7.ImageSize = new System.Drawing.Point(30, 30);
			this.m_btn7.LoadImage = global::FrameOfSystem3.Properties.Resources.CONNECT_BLACK;
			this.m_btn7.Location = new System.Drawing.Point(5, 355);
			this.m_btn7.MainFont = new System.Drawing.Font("맑은 고딕", 11F, System.Drawing.FontStyle.Bold);
			this.m_btn7.MainFontColor = System.Drawing.Color.FromArgb(((int)(((byte)(124)))), ((int)(((byte)(123)))), ((int)(((byte)(131)))));
			this.m_btn7.Name = "m_btn7";
			this.m_btn7.Size = new System.Drawing.Size(130, 50);
			this.m_btn7.SubFont = new System.Drawing.Font("맑은 고딕", 8F, System.Drawing.FontStyle.Bold);
			this.m_btn7.SubFontColor = System.Drawing.Color.DarkBlue;
			this.m_btn7.SubText = "MOVE";
			this.m_btn7.TabIndex = 6;
			this.m_btn7.Text = "OPERATION";
			this.m_btn7.TextAlignMain = Sys3Controls.EN_TEXTALIGN.MIDDLE_CENTER;
			this.m_btn7.TextAlignSub = Sys3Controls.EN_TEXTALIGN.TOP_RIGHT;
			this.m_btn7.ThemeIndex = 0;
			this.m_btn7.UseBorder = false;
			this.m_btn7.UseClickedEmphasizeTextColor = false;
			this.m_btn7.UseCustomizeClickedColor = true;
			this.m_btn7.UseEdge = false;
			this.m_btn7.UseHoverEmphasizeCustomColor = true;
			this.m_btn7.UseImage = false;
			this.m_btn7.UserHoverEmpahsize = true;
			this.m_btn7.UseSubFont = false;
			this.m_btn7.Click += new System.EventHandler(this.ClickButtons);
			// 
			// m_btn6
			// 
			this.m_btn6.BorderWidth = 2;
			this.m_btn6.ButtonClicked = false;
			this.m_btn6.ClickedEmphasizeTextColor = System.Drawing.Color.White;
			this.m_btn6.CustomClickedGradientFirstColor = System.Drawing.Color.Gray;
			this.m_btn6.CustomClickedGradientSecondColor = System.Drawing.Color.Gray;
			this.m_btn6.Description = "";
			this.m_btn6.DisabledColor = System.Drawing.Color.DarkGray;
			this.m_btn6.EdgeRadius = 5;
			this.m_btn6.GradientAngle = 70F;
			this.m_btn6.GradientFirstColor = System.Drawing.Color.White;
			this.m_btn6.GradientSecondColor = System.Drawing.Color.White;
			this.m_btn6.HoverEmphasizeCustomColor = System.Drawing.Color.Firebrick;
			this.m_btn6.ImagePosition = new System.Drawing.Point(7, 7);
			this.m_btn6.ImageSize = new System.Drawing.Point(30, 30);
			this.m_btn6.LoadImage = global::FrameOfSystem3.Properties.Resources.CONNECT_BLACK;
			this.m_btn6.Location = new System.Drawing.Point(5, 302);
			this.m_btn6.MainFont = new System.Drawing.Font("맑은 고딕", 11F, System.Drawing.FontStyle.Bold);
			this.m_btn6.MainFontColor = System.Drawing.Color.FromArgb(((int)(((byte)(124)))), ((int)(((byte)(123)))), ((int)(((byte)(131)))));
			this.m_btn6.Name = "m_btn6";
			this.m_btn6.Size = new System.Drawing.Size(130, 50);
			this.m_btn6.SubFont = new System.Drawing.Font("맑은 고딕", 8F, System.Drawing.FontStyle.Bold);
			this.m_btn6.SubFontColor = System.Drawing.Color.DarkBlue;
			this.m_btn6.SubText = "MOVE";
			this.m_btn6.TabIndex = 5;
			this.m_btn6.Text = "OPERATION";
			this.m_btn6.TextAlignMain = Sys3Controls.EN_TEXTALIGN.MIDDLE_CENTER;
			this.m_btn6.TextAlignSub = Sys3Controls.EN_TEXTALIGN.TOP_RIGHT;
			this.m_btn6.ThemeIndex = 0;
			this.m_btn6.UseBorder = false;
			this.m_btn6.UseClickedEmphasizeTextColor = false;
			this.m_btn6.UseCustomizeClickedColor = true;
			this.m_btn6.UseEdge = false;
			this.m_btn6.UseHoverEmphasizeCustomColor = true;
			this.m_btn6.UseImage = false;
			this.m_btn6.UserHoverEmpahsize = true;
			this.m_btn6.UseSubFont = false;
			this.m_btn6.Click += new System.EventHandler(this.ClickButtons);
			// 
			// m_btn8
			// 
			this.m_btn8.BorderWidth = 2;
			this.m_btn8.ButtonClicked = false;
			this.m_btn8.ClickedEmphasizeTextColor = System.Drawing.Color.White;
			this.m_btn8.CustomClickedGradientFirstColor = System.Drawing.Color.Gray;
			this.m_btn8.CustomClickedGradientSecondColor = System.Drawing.Color.Gray;
			this.m_btn8.Description = "";
			this.m_btn8.DisabledColor = System.Drawing.Color.DarkGray;
			this.m_btn8.EdgeRadius = 5;
			this.m_btn8.GradientAngle = 70F;
			this.m_btn8.GradientFirstColor = System.Drawing.Color.White;
			this.m_btn8.GradientSecondColor = System.Drawing.Color.White;
			this.m_btn8.HoverEmphasizeCustomColor = System.Drawing.Color.Firebrick;
			this.m_btn8.ImagePosition = new System.Drawing.Point(7, 7);
			this.m_btn8.ImageSize = new System.Drawing.Point(30, 30);
			this.m_btn8.LoadImage = global::FrameOfSystem3.Properties.Resources.CONNECT_BLACK;
			this.m_btn8.Location = new System.Drawing.Point(5, 408);
			this.m_btn8.MainFont = new System.Drawing.Font("맑은 고딕", 11F, System.Drawing.FontStyle.Bold);
			this.m_btn8.MainFontColor = System.Drawing.Color.FromArgb(((int)(((byte)(124)))), ((int)(((byte)(123)))), ((int)(((byte)(131)))));
			this.m_btn8.Name = "m_btn8";
			this.m_btn8.Size = new System.Drawing.Size(130, 50);
			this.m_btn8.SubFont = new System.Drawing.Font("맑은 고딕", 8F, System.Drawing.FontStyle.Bold);
			this.m_btn8.SubFontColor = System.Drawing.Color.DarkBlue;
			this.m_btn8.SubText = "MOVE";
			this.m_btn8.TabIndex = 7;
			this.m_btn8.Text = "OPERATION";
			this.m_btn8.TextAlignMain = Sys3Controls.EN_TEXTALIGN.MIDDLE_CENTER;
			this.m_btn8.TextAlignSub = Sys3Controls.EN_TEXTALIGN.TOP_RIGHT;
			this.m_btn8.ThemeIndex = 0;
			this.m_btn8.UseBorder = false;
			this.m_btn8.UseClickedEmphasizeTextColor = false;
			this.m_btn8.UseCustomizeClickedColor = true;
			this.m_btn8.UseEdge = false;
			this.m_btn8.UseHoverEmphasizeCustomColor = true;
			this.m_btn8.UseImage = false;
			this.m_btn8.UserHoverEmpahsize = true;
			this.m_btn8.UseSubFont = false;
			this.m_btn8.Click += new System.EventHandler(this.ClickButtons);
			// 
			// m_btn10
			// 
			this.m_btn10.BorderWidth = 2;
			this.m_btn10.ButtonClicked = false;
			this.m_btn10.ClickedEmphasizeTextColor = System.Drawing.Color.White;
			this.m_btn10.CustomClickedGradientFirstColor = System.Drawing.Color.Gray;
			this.m_btn10.CustomClickedGradientSecondColor = System.Drawing.Color.Gray;
			this.m_btn10.Description = "";
			this.m_btn10.DisabledColor = System.Drawing.Color.DarkGray;
			this.m_btn10.EdgeRadius = 5;
			this.m_btn10.GradientAngle = 70F;
			this.m_btn10.GradientFirstColor = System.Drawing.Color.White;
			this.m_btn10.GradientSecondColor = System.Drawing.Color.White;
			this.m_btn10.HoverEmphasizeCustomColor = System.Drawing.Color.Firebrick;
			this.m_btn10.ImagePosition = new System.Drawing.Point(7, 7);
			this.m_btn10.ImageSize = new System.Drawing.Point(30, 30);
			this.m_btn10.LoadImage = global::FrameOfSystem3.Properties.Resources.CONNECT_BLACK;
			this.m_btn10.Location = new System.Drawing.Point(5, 514);
			this.m_btn10.MainFont = new System.Drawing.Font("맑은 고딕", 11F, System.Drawing.FontStyle.Bold);
			this.m_btn10.MainFontColor = System.Drawing.Color.FromArgb(((int)(((byte)(124)))), ((int)(((byte)(123)))), ((int)(((byte)(131)))));
			this.m_btn10.Name = "m_btn10";
			this.m_btn10.Size = new System.Drawing.Size(130, 50);
			this.m_btn10.SubFont = new System.Drawing.Font("맑은 고딕", 8F, System.Drawing.FontStyle.Bold);
			this.m_btn10.SubFontColor = System.Drawing.Color.DarkBlue;
			this.m_btn10.SubText = "MOVE";
			this.m_btn10.TabIndex = 9;
			this.m_btn10.Text = "OPERATION";
			this.m_btn10.TextAlignMain = Sys3Controls.EN_TEXTALIGN.MIDDLE_CENTER;
			this.m_btn10.TextAlignSub = Sys3Controls.EN_TEXTALIGN.TOP_RIGHT;
			this.m_btn10.ThemeIndex = 0;
			this.m_btn10.UseBorder = false;
			this.m_btn10.UseClickedEmphasizeTextColor = false;
			this.m_btn10.UseCustomizeClickedColor = true;
			this.m_btn10.UseEdge = false;
			this.m_btn10.UseHoverEmphasizeCustomColor = true;
			this.m_btn10.UseImage = false;
			this.m_btn10.UserHoverEmpahsize = true;
			this.m_btn10.UseSubFont = false;
			this.m_btn10.Click += new System.EventHandler(this.ClickButtons);
			// 
			// m_btn9
			// 
			this.m_btn9.BorderWidth = 2;
			this.m_btn9.ButtonClicked = false;
			this.m_btn9.ClickedEmphasizeTextColor = System.Drawing.Color.White;
			this.m_btn9.CustomClickedGradientFirstColor = System.Drawing.Color.Gray;
			this.m_btn9.CustomClickedGradientSecondColor = System.Drawing.Color.Gray;
			this.m_btn9.Description = "";
			this.m_btn9.DisabledColor = System.Drawing.Color.DarkGray;
			this.m_btn9.EdgeRadius = 5;
			this.m_btn9.GradientAngle = 70F;
			this.m_btn9.GradientFirstColor = System.Drawing.Color.White;
			this.m_btn9.GradientSecondColor = System.Drawing.Color.White;
			this.m_btn9.HoverEmphasizeCustomColor = System.Drawing.Color.Firebrick;
			this.m_btn9.ImagePosition = new System.Drawing.Point(7, 7);
			this.m_btn9.ImageSize = new System.Drawing.Point(30, 30);
			this.m_btn9.LoadImage = global::FrameOfSystem3.Properties.Resources.CONNECT_BLACK;
			this.m_btn9.Location = new System.Drawing.Point(5, 461);
			this.m_btn9.MainFont = new System.Drawing.Font("맑은 고딕", 11F, System.Drawing.FontStyle.Bold);
			this.m_btn9.MainFontColor = System.Drawing.Color.FromArgb(((int)(((byte)(124)))), ((int)(((byte)(123)))), ((int)(((byte)(131)))));
			this.m_btn9.Name = "m_btn9";
			this.m_btn9.Size = new System.Drawing.Size(130, 50);
			this.m_btn9.SubFont = new System.Drawing.Font("맑은 고딕", 8F, System.Drawing.FontStyle.Bold);
			this.m_btn9.SubFontColor = System.Drawing.Color.DarkBlue;
			this.m_btn9.SubText = "MOVE";
			this.m_btn9.TabIndex = 8;
			this.m_btn9.Text = "OPERATION";
			this.m_btn9.TextAlignMain = Sys3Controls.EN_TEXTALIGN.MIDDLE_CENTER;
			this.m_btn9.TextAlignSub = Sys3Controls.EN_TEXTALIGN.TOP_RIGHT;
			this.m_btn9.ThemeIndex = 0;
			this.m_btn9.UseBorder = false;
			this.m_btn9.UseClickedEmphasizeTextColor = false;
			this.m_btn9.UseCustomizeClickedColor = true;
			this.m_btn9.UseEdge = false;
			this.m_btn9.UseHoverEmphasizeCustomColor = true;
			this.m_btn9.UseImage = false;
			this.m_btn9.UserHoverEmpahsize = true;
			this.m_btn9.UseSubFont = false;
			this.m_btn9.Click += new System.EventHandler(this.ClickButtons);
			// 
			// m_btn11
			// 
			this.m_btn11.BorderWidth = 2;
			this.m_btn11.ButtonClicked = false;
			this.m_btn11.ClickedEmphasizeTextColor = System.Drawing.Color.White;
			this.m_btn11.CustomClickedGradientFirstColor = System.Drawing.Color.Gray;
			this.m_btn11.CustomClickedGradientSecondColor = System.Drawing.Color.Gray;
			this.m_btn11.Description = "";
			this.m_btn11.DisabledColor = System.Drawing.Color.DarkGray;
			this.m_btn11.EdgeRadius = 5;
			this.m_btn11.GradientAngle = 70F;
			this.m_btn11.GradientFirstColor = System.Drawing.Color.White;
			this.m_btn11.GradientSecondColor = System.Drawing.Color.White;
			this.m_btn11.HoverEmphasizeCustomColor = System.Drawing.Color.Firebrick;
			this.m_btn11.ImagePosition = new System.Drawing.Point(7, 7);
			this.m_btn11.ImageSize = new System.Drawing.Point(30, 30);
			this.m_btn11.LoadImage = global::FrameOfSystem3.Properties.Resources.CONNECT_BLACK;
			this.m_btn11.Location = new System.Drawing.Point(5, 567);
			this.m_btn11.MainFont = new System.Drawing.Font("맑은 고딕", 11F, System.Drawing.FontStyle.Bold);
			this.m_btn11.MainFontColor = System.Drawing.Color.FromArgb(((int)(((byte)(124)))), ((int)(((byte)(123)))), ((int)(((byte)(131)))));
			this.m_btn11.Name = "m_btn11";
			this.m_btn11.Size = new System.Drawing.Size(130, 50);
			this.m_btn11.SubFont = new System.Drawing.Font("맑은 고딕", 8F, System.Drawing.FontStyle.Bold);
			this.m_btn11.SubFontColor = System.Drawing.Color.DarkBlue;
			this.m_btn11.SubText = "MOVE";
			this.m_btn11.TabIndex = 10;
			this.m_btn11.Text = "OPERATION";
			this.m_btn11.TextAlignMain = Sys3Controls.EN_TEXTALIGN.MIDDLE_CENTER;
			this.m_btn11.TextAlignSub = Sys3Controls.EN_TEXTALIGN.TOP_RIGHT;
			this.m_btn11.ThemeIndex = 0;
			this.m_btn11.UseBorder = false;
			this.m_btn11.UseClickedEmphasizeTextColor = false;
			this.m_btn11.UseCustomizeClickedColor = true;
			this.m_btn11.UseEdge = false;
			this.m_btn11.UseHoverEmphasizeCustomColor = true;
			this.m_btn11.UseImage = false;
			this.m_btn11.UserHoverEmpahsize = true;
			this.m_btn11.UseSubFont = false;
			this.m_btn11.Click += new System.EventHandler(this.ClickButtons);
			// 
			// m_btn13
			// 
			this.m_btn13.BorderWidth = 2;
			this.m_btn13.ButtonClicked = false;
			this.m_btn13.ClickedEmphasizeTextColor = System.Drawing.Color.White;
			this.m_btn13.CustomClickedGradientFirstColor = System.Drawing.Color.Gray;
			this.m_btn13.CustomClickedGradientSecondColor = System.Drawing.Color.Gray;
			this.m_btn13.Description = "";
			this.m_btn13.DisabledColor = System.Drawing.Color.DarkGray;
			this.m_btn13.EdgeRadius = 5;
			this.m_btn13.GradientAngle = 70F;
			this.m_btn13.GradientFirstColor = System.Drawing.Color.White;
			this.m_btn13.GradientSecondColor = System.Drawing.Color.White;
			this.m_btn13.HoverEmphasizeCustomColor = System.Drawing.Color.Firebrick;
			this.m_btn13.ImagePosition = new System.Drawing.Point(7, 7);
			this.m_btn13.ImageSize = new System.Drawing.Point(30, 30);
			this.m_btn13.LoadImage = global::FrameOfSystem3.Properties.Resources.CONNECT_BLACK;
			this.m_btn13.Location = new System.Drawing.Point(5, 673);
			this.m_btn13.MainFont = new System.Drawing.Font("맑은 고딕", 11F, System.Drawing.FontStyle.Bold);
			this.m_btn13.MainFontColor = System.Drawing.Color.FromArgb(((int)(((byte)(124)))), ((int)(((byte)(123)))), ((int)(((byte)(131)))));
			this.m_btn13.Name = "m_btn13";
			this.m_btn13.Size = new System.Drawing.Size(130, 50);
			this.m_btn13.SubFont = new System.Drawing.Font("맑은 고딕", 8F, System.Drawing.FontStyle.Bold);
			this.m_btn13.SubFontColor = System.Drawing.Color.DarkBlue;
			this.m_btn13.SubText = "MOVE";
			this.m_btn13.TabIndex = 12;
			this.m_btn13.Text = "OPERATION";
			this.m_btn13.TextAlignMain = Sys3Controls.EN_TEXTALIGN.MIDDLE_CENTER;
			this.m_btn13.TextAlignSub = Sys3Controls.EN_TEXTALIGN.TOP_RIGHT;
			this.m_btn13.ThemeIndex = 0;
			this.m_btn13.UseBorder = false;
			this.m_btn13.UseClickedEmphasizeTextColor = false;
			this.m_btn13.UseCustomizeClickedColor = true;
			this.m_btn13.UseEdge = false;
			this.m_btn13.UseHoverEmphasizeCustomColor = true;
			this.m_btn13.UseImage = false;
			this.m_btn13.UserHoverEmpahsize = true;
			this.m_btn13.UseSubFont = false;
			this.m_btn13.Click += new System.EventHandler(this.ClickButtons);
			// 
			// m_btn12
			// 
			this.m_btn12.BorderWidth = 2;
			this.m_btn12.ButtonClicked = false;
			this.m_btn12.ClickedEmphasizeTextColor = System.Drawing.Color.White;
			this.m_btn12.CustomClickedGradientFirstColor = System.Drawing.Color.Gray;
			this.m_btn12.CustomClickedGradientSecondColor = System.Drawing.Color.Gray;
			this.m_btn12.Description = "";
			this.m_btn12.DisabledColor = System.Drawing.Color.DarkGray;
			this.m_btn12.EdgeRadius = 5;
			this.m_btn12.GradientAngle = 70F;
			this.m_btn12.GradientFirstColor = System.Drawing.Color.White;
			this.m_btn12.GradientSecondColor = System.Drawing.Color.White;
			this.m_btn12.HoverEmphasizeCustomColor = System.Drawing.Color.Firebrick;
			this.m_btn12.ImagePosition = new System.Drawing.Point(7, 7);
			this.m_btn12.ImageSize = new System.Drawing.Point(30, 30);
			this.m_btn12.LoadImage = global::FrameOfSystem3.Properties.Resources.CONNECT_BLACK;
			this.m_btn12.Location = new System.Drawing.Point(5, 620);
			this.m_btn12.MainFont = new System.Drawing.Font("맑은 고딕", 11F, System.Drawing.FontStyle.Bold);
			this.m_btn12.MainFontColor = System.Drawing.Color.FromArgb(((int)(((byte)(124)))), ((int)(((byte)(123)))), ((int)(((byte)(131)))));
			this.m_btn12.Name = "m_btn12";
			this.m_btn12.Size = new System.Drawing.Size(130, 50);
			this.m_btn12.SubFont = new System.Drawing.Font("맑은 고딕", 8F, System.Drawing.FontStyle.Bold);
			this.m_btn12.SubFontColor = System.Drawing.Color.DarkBlue;
			this.m_btn12.SubText = "MOVE";
			this.m_btn12.TabIndex = 11;
			this.m_btn12.Text = "OPERATION";
			this.m_btn12.TextAlignMain = Sys3Controls.EN_TEXTALIGN.MIDDLE_CENTER;
			this.m_btn12.TextAlignSub = Sys3Controls.EN_TEXTALIGN.TOP_RIGHT;
			this.m_btn12.ThemeIndex = 0;
			this.m_btn12.UseBorder = false;
			this.m_btn12.UseClickedEmphasizeTextColor = false;
			this.m_btn12.UseCustomizeClickedColor = true;
			this.m_btn12.UseEdge = false;
			this.m_btn12.UseHoverEmphasizeCustomColor = true;
			this.m_btn12.UseImage = false;
			this.m_btn12.UserHoverEmpahsize = true;
			this.m_btn12.UseSubFont = false;
			this.m_btn12.Click += new System.EventHandler(this.ClickButtons);
			// 
			// m_btn14
			// 
			this.m_btn14.BorderWidth = 2;
			this.m_btn14.ButtonClicked = false;
			this.m_btn14.ClickedEmphasizeTextColor = System.Drawing.Color.White;
			this.m_btn14.CustomClickedGradientFirstColor = System.Drawing.Color.Gray;
			this.m_btn14.CustomClickedGradientSecondColor = System.Drawing.Color.Gray;
			this.m_btn14.Description = "";
			this.m_btn14.DisabledColor = System.Drawing.Color.DarkGray;
			this.m_btn14.EdgeRadius = 5;
			this.m_btn14.GradientAngle = 70F;
			this.m_btn14.GradientFirstColor = System.Drawing.Color.White;
			this.m_btn14.GradientSecondColor = System.Drawing.Color.White;
			this.m_btn14.HoverEmphasizeCustomColor = System.Drawing.Color.Firebrick;
			this.m_btn14.ImagePosition = new System.Drawing.Point(7, 7);
			this.m_btn14.ImageSize = new System.Drawing.Point(30, 30);
			this.m_btn14.LoadImage = global::FrameOfSystem3.Properties.Resources.CONNECT_BLACK;
			this.m_btn14.Location = new System.Drawing.Point(5, 726);
			this.m_btn14.MainFont = new System.Drawing.Font("맑은 고딕", 11F, System.Drawing.FontStyle.Bold);
			this.m_btn14.MainFontColor = System.Drawing.Color.FromArgb(((int)(((byte)(124)))), ((int)(((byte)(123)))), ((int)(((byte)(131)))));
			this.m_btn14.Name = "m_btn14";
			this.m_btn14.Size = new System.Drawing.Size(130, 50);
			this.m_btn14.SubFont = new System.Drawing.Font("맑은 고딕", 8F, System.Drawing.FontStyle.Bold);
			this.m_btn14.SubFontColor = System.Drawing.Color.DarkBlue;
			this.m_btn14.SubText = "MOVE";
			this.m_btn14.TabIndex = 13;
			this.m_btn14.Text = "OPERATION";
			this.m_btn14.TextAlignMain = Sys3Controls.EN_TEXTALIGN.MIDDLE_CENTER;
			this.m_btn14.TextAlignSub = Sys3Controls.EN_TEXTALIGN.TOP_RIGHT;
			this.m_btn14.ThemeIndex = 0;
			this.m_btn14.UseBorder = false;
			this.m_btn14.UseClickedEmphasizeTextColor = false;
			this.m_btn14.UseCustomizeClickedColor = true;
			this.m_btn14.UseEdge = false;
			this.m_btn14.UseHoverEmphasizeCustomColor = true;
			this.m_btn14.UseImage = false;
			this.m_btn14.UserHoverEmpahsize = true;
			this.m_btn14.UseSubFont = false;
			this.m_btn14.Click += new System.EventHandler(this.ClickButtons);
			// 
			// m_btn15
			// 
			this.m_btn15.BorderWidth = 2;
			this.m_btn15.ButtonClicked = false;
			this.m_btn15.ClickedEmphasizeTextColor = System.Drawing.Color.White;
			this.m_btn15.CustomClickedGradientFirstColor = System.Drawing.Color.Gray;
			this.m_btn15.CustomClickedGradientSecondColor = System.Drawing.Color.Gray;
			this.m_btn15.Description = "";
			this.m_btn15.DisabledColor = System.Drawing.Color.DarkGray;
			this.m_btn15.EdgeRadius = 5;
			this.m_btn15.GradientAngle = 70F;
			this.m_btn15.GradientFirstColor = System.Drawing.Color.White;
			this.m_btn15.GradientSecondColor = System.Drawing.Color.White;
			this.m_btn15.HoverEmphasizeCustomColor = System.Drawing.Color.Firebrick;
			this.m_btn15.ImagePosition = new System.Drawing.Point(7, 7);
			this.m_btn15.ImageSize = new System.Drawing.Point(30, 30);
			this.m_btn15.LoadImage = global::FrameOfSystem3.Properties.Resources.CONNECT_BLACK;
			this.m_btn15.Location = new System.Drawing.Point(5, 779);
			this.m_btn15.MainFont = new System.Drawing.Font("맑은 고딕", 11F, System.Drawing.FontStyle.Bold);
			this.m_btn15.MainFontColor = System.Drawing.Color.FromArgb(((int)(((byte)(124)))), ((int)(((byte)(123)))), ((int)(((byte)(131)))));
			this.m_btn15.Name = "m_btn15";
			this.m_btn15.Size = new System.Drawing.Size(130, 50);
			this.m_btn15.SubFont = new System.Drawing.Font("맑은 고딕", 8F, System.Drawing.FontStyle.Bold);
			this.m_btn15.SubFontColor = System.Drawing.Color.DarkBlue;
			this.m_btn15.SubText = "MOVE";
			this.m_btn15.TabIndex = 14;
			this.m_btn15.Text = "OPERATION";
			this.m_btn15.TextAlignMain = Sys3Controls.EN_TEXTALIGN.MIDDLE_CENTER;
			this.m_btn15.TextAlignSub = Sys3Controls.EN_TEXTALIGN.TOP_RIGHT;
			this.m_btn15.ThemeIndex = 0;
			this.m_btn15.UseBorder = false;
			this.m_btn15.UseClickedEmphasizeTextColor = false;
			this.m_btn15.UseCustomizeClickedColor = true;
			this.m_btn15.UseEdge = false;
			this.m_btn15.UseHoverEmphasizeCustomColor = true;
			this.m_btn15.UseImage = false;
			this.m_btn15.UserHoverEmpahsize = true;
			this.m_btn15.UseSubFont = false;
			this.m_btn15.Click += new System.EventHandler(this.ClickButtons);
			// 
			// m_btn16
			// 
			this.m_btn16.BorderWidth = 2;
			this.m_btn16.ButtonClicked = false;
			this.m_btn16.ClickedEmphasizeTextColor = System.Drawing.Color.White;
			this.m_btn16.CustomClickedGradientFirstColor = System.Drawing.Color.Gray;
			this.m_btn16.CustomClickedGradientSecondColor = System.Drawing.Color.Gray;
			this.m_btn16.Description = "";
			this.m_btn16.DisabledColor = System.Drawing.Color.DarkGray;
			this.m_btn16.EdgeRadius = 5;
			this.m_btn16.GradientAngle = 70F;
			this.m_btn16.GradientFirstColor = System.Drawing.Color.White;
			this.m_btn16.GradientSecondColor = System.Drawing.Color.White;
			this.m_btn16.HoverEmphasizeCustomColor = System.Drawing.Color.Firebrick;
			this.m_btn16.ImagePosition = new System.Drawing.Point(7, 7);
			this.m_btn16.ImageSize = new System.Drawing.Point(30, 30);
			this.m_btn16.LoadImage = global::FrameOfSystem3.Properties.Resources.CONNECT_BLACK;
			this.m_btn16.Location = new System.Drawing.Point(5, 832);
			this.m_btn16.MainFont = new System.Drawing.Font("맑은 고딕", 11F, System.Drawing.FontStyle.Bold);
			this.m_btn16.MainFontColor = System.Drawing.Color.FromArgb(((int)(((byte)(124)))), ((int)(((byte)(123)))), ((int)(((byte)(131)))));
			this.m_btn16.Name = "m_btn16";
			this.m_btn16.Size = new System.Drawing.Size(130, 50);
			this.m_btn16.SubFont = new System.Drawing.Font("맑은 고딕", 8F, System.Drawing.FontStyle.Bold);
			this.m_btn16.SubFontColor = System.Drawing.Color.DarkBlue;
			this.m_btn16.SubText = "MOVE";
			this.m_btn16.TabIndex = 15;
			this.m_btn16.Text = "OPERATION";
			this.m_btn16.TextAlignMain = Sys3Controls.EN_TEXTALIGN.MIDDLE_CENTER;
			this.m_btn16.TextAlignSub = Sys3Controls.EN_TEXTALIGN.TOP_RIGHT;
			this.m_btn16.ThemeIndex = 0;
			this.m_btn16.UseBorder = false;
			this.m_btn16.UseClickedEmphasizeTextColor = false;
			this.m_btn16.UseCustomizeClickedColor = true;
			this.m_btn16.UseEdge = false;
			this.m_btn16.UseHoverEmphasizeCustomColor = true;
			this.m_btn16.UseImage = false;
			this.m_btn16.UserHoverEmpahsize = true;
			this.m_btn16.UseSubFont = false;
			this.m_btn16.Click += new System.EventHandler(this.ClickButtons);
			// 
			// SubMenu
			// 
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
			this.BackColor = System.Drawing.Color.Transparent;
			this.Controls.Add(this.m_btn16);
			this.Controls.Add(this.m_btn15);
			this.Controls.Add(this.m_btn14);
			this.Controls.Add(this.m_btn11);
			this.Controls.Add(this.m_btn8);
			this.Controls.Add(this.m_btn4);
			this.Controls.Add(this.m_btn12);
			this.Controls.Add(this.m_btn9);
			this.Controls.Add(this.m_btn13);
			this.Controls.Add(this.m_btn6);
			this.Controls.Add(this.m_btn10);
			this.Controls.Add(this.m_btn2);
			this.Controls.Add(this.m_btn7);
			this.Controls.Add(this.m_btn3);
			this.Controls.Add(this.m_btn5);
			this.Controls.Add(this.m_btn1);
			this.Controls.Add(this.m_groupColorPalette);
			this.Margin = new System.Windows.Forms.Padding(0);
			this.Name = "SubMenu";
			this.Size = new System.Drawing.Size(140, 898);
			this.ResumeLayout(false);

        }

        #endregion

		private Sys3Controls.Sys3GroupBox m_groupColorPalette;
		private Sys3Controls.Sys3button m_btn1;
		private Sys3Controls.Sys3button m_btn2;
		private Sys3Controls.Sys3button m_btn3;
		private Sys3Controls.Sys3button m_btn4;
		private Sys3Controls.Sys3button m_btn5;
		private Sys3Controls.Sys3button m_btn7;
		private Sys3Controls.Sys3button m_btn6;
		private Sys3Controls.Sys3button m_btn8;
		private Sys3Controls.Sys3button m_btn10;
		private Sys3Controls.Sys3button m_btn9;
		private Sys3Controls.Sys3button m_btn11;
		private Sys3Controls.Sys3button m_btn13;
		private Sys3Controls.Sys3button m_btn12;
		private Sys3Controls.Sys3button m_btn14;
		private Sys3Controls.Sys3button m_btn15;
        private Sys3Controls.Sys3button m_btn16;



    }
}
