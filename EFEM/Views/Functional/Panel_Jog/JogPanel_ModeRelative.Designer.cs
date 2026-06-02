namespace FrameOfSystem3.Views.Functional.Jog
{
	partial class JogPanel_ModeRelative
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
			this.m_lblRelative = new Sys3Controls.Sys3Label();
			this.lbl_Distance = new Sys3Controls.Sys3Label();
			this.btn_FineControlDown = new Sys3Controls.Sys3button();
			this.btn_Define_1000 = new Sys3Controls.Sys3button();
			this.btn_Define_500 = new Sys3Controls.Sys3button();
			this.btn_Define_100 = new Sys3Controls.Sys3button();
			this.btn_Define_10 = new Sys3Controls.Sys3button();
			this.btn_Define_1 = new Sys3Controls.Sys3button();
			this.btn_FineControlUp = new Sys3Controls.Sys3button();
			this.SuspendLayout();
			// 
			// m_lblRelative
			// 
			this.m_lblRelative.BackGroundColor = System.Drawing.Color.FromArgb(((int)(((byte)(200)))), ((int)(((byte)(200)))), ((int)(((byte)(220)))));
			this.m_lblRelative.BorderStroke = 2;
			this.m_lblRelative.BorderStyle = System.Windows.Forms.ButtonBorderStyle.Solid;
			this.m_lblRelative.Description = "";
			this.m_lblRelative.DisabledColor = System.Drawing.Color.DarkGray;
			this.m_lblRelative.EdgeRadius = 1;
			this.m_lblRelative.ImagePosition = new System.Drawing.Point(0, 0);
			this.m_lblRelative.ImageSize = new System.Drawing.Point(0, 0);
			this.m_lblRelative.LoadImage = null;
			this.m_lblRelative.Location = new System.Drawing.Point(0, 0);
			this.m_lblRelative.MainFont = new System.Drawing.Font("맑은 고딕", 9F, System.Drawing.FontStyle.Bold);
			this.m_lblRelative.MainFontColor = System.Drawing.Color.Black;
			this.m_lblRelative.Margin = new System.Windows.Forms.Padding(1, 1, 0, 0);
			this.m_lblRelative.Name = "m_lblRelative";
			this.m_lblRelative.Size = new System.Drawing.Size(50, 35);
			this.m_lblRelative.SubFont = new System.Drawing.Font("맑은 고딕", 10F);
			this.m_lblRelative.SubFontColor = System.Drawing.Color.Black;
			this.m_lblRelative.SubText = "";
			this.m_lblRelative.TabIndex = 1417;
			this.m_lblRelative.Text = "STEP";
			this.m_lblRelative.TextAlignMain = Sys3Controls.EN_TEXTALIGN.MIDDLE_CENTER;
			this.m_lblRelative.TextAlignSub = Sys3Controls.EN_TEXTALIGN.MIDDLE_CENTER;
			this.m_lblRelative.TextAlignUnit = Sys3Controls.EN_TEXTALIGN.MIDDLE_CENTER;
			this.m_lblRelative.ThemeIndex = 0;
			this.m_lblRelative.UnitAreaRate = 40;
			this.m_lblRelative.UnitFont = new System.Drawing.Font("맑은 고딕", 10F, System.Drawing.FontStyle.Bold);
			this.m_lblRelative.UnitFontColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
			this.m_lblRelative.UnitPositionVertical = false;
			this.m_lblRelative.UnitText = "";
			this.m_lblRelative.UseBorder = true;
			this.m_lblRelative.UseEdgeRadius = false;
			this.m_lblRelative.UseImage = false;
			this.m_lblRelative.UseSubFont = false;
			this.m_lblRelative.UseUnitFont = false;
			// 
			// lbl_Distance
			// 
			this.lbl_Distance.BackGroundColor = System.Drawing.Color.White;
			this.lbl_Distance.BorderStroke = 2;
			this.lbl_Distance.BorderStyle = System.Windows.Forms.ButtonBorderStyle.Solid;
			this.lbl_Distance.Description = "";
			this.lbl_Distance.DisabledColor = System.Drawing.Color.LightGray;
			this.lbl_Distance.EdgeRadius = 1;
			this.lbl_Distance.ImagePosition = new System.Drawing.Point(0, 0);
			this.lbl_Distance.ImageSize = new System.Drawing.Point(0, 0);
			this.lbl_Distance.LoadImage = null;
			this.lbl_Distance.Location = new System.Drawing.Point(51, 0);
			this.lbl_Distance.MainFont = new System.Drawing.Font("맑은 고딕", 11F, System.Drawing.FontStyle.Bold);
			this.lbl_Distance.MainFontColor = System.Drawing.Color.Black;
			this.lbl_Distance.Margin = new System.Windows.Forms.Padding(1, 1, 0, 0);
			this.lbl_Distance.Name = "lbl_Distance";
			this.lbl_Distance.Size = new System.Drawing.Size(203, 35);
			this.lbl_Distance.SubFont = new System.Drawing.Font("맑은 고딕", 10F);
			this.lbl_Distance.SubFontColor = System.Drawing.Color.Black;
			this.lbl_Distance.SubText = "";
			this.lbl_Distance.TabIndex = 1416;
			this.lbl_Distance.Text = "--";
			this.lbl_Distance.TextAlignMain = Sys3Controls.EN_TEXTALIGN.BOTTOM_RIGHT;
			this.lbl_Distance.TextAlignSub = Sys3Controls.EN_TEXTALIGN.BOTTOM_RIGHT;
			this.lbl_Distance.TextAlignUnit = Sys3Controls.EN_TEXTALIGN.BOTTOM_RIGHT;
			this.lbl_Distance.ThemeIndex = 0;
			this.lbl_Distance.UnitAreaRate = 25;
			this.lbl_Distance.UnitFont = new System.Drawing.Font("맑은 고딕", 10F, System.Drawing.FontStyle.Bold);
			this.lbl_Distance.UnitFontColor = System.Drawing.Color.Black;
			this.lbl_Distance.UnitPositionVertical = false;
			this.lbl_Distance.UnitText = "mm";
			this.lbl_Distance.UseBorder = true;
			this.lbl_Distance.UseEdgeRadius = false;
			this.lbl_Distance.UseImage = false;
			this.lbl_Distance.UseSubFont = false;
			this.lbl_Distance.UseUnitFont = true;
			this.lbl_Distance.Click += new System.EventHandler(this.Click_Distance);
			// 
			// btn_FineControlDown
			// 
			this.btn_FineControlDown.BorderWidth = 1;
			this.btn_FineControlDown.ButtonClicked = false;
			this.btn_FineControlDown.ClickedEmphasizeTextColor = System.Drawing.Color.White;
			this.btn_FineControlDown.CustomClickedGradientFirstColor = System.Drawing.Color.White;
			this.btn_FineControlDown.CustomClickedGradientSecondColor = System.Drawing.Color.White;
			this.btn_FineControlDown.Description = "";
			this.btn_FineControlDown.DisabledColor = System.Drawing.Color.DarkGray;
			this.btn_FineControlDown.EdgeRadius = 5;
			this.btn_FineControlDown.GradientAngle = 70F;
			this.btn_FineControlDown.GradientFirstColor = System.Drawing.Color.White;
			this.btn_FineControlDown.GradientSecondColor = System.Drawing.Color.FromArgb(((int)(((byte)(170)))), ((int)(((byte)(176)))), ((int)(((byte)(183)))));
			this.btn_FineControlDown.HoverEmphasizeCustomColor = System.Drawing.Color.White;
			this.btn_FineControlDown.ImagePosition = new System.Drawing.Point(10, 10);
			this.btn_FineControlDown.ImageSize = new System.Drawing.Point(30, 30);
			this.btn_FineControlDown.LoadImage = global::FrameOfSystem3.Properties.Resources.CLEAR_BLACK;
			this.btn_FineControlDown.Location = new System.Drawing.Point(255, 36);
			this.btn_FineControlDown.MainFont = new System.Drawing.Font("맑은 고딕", 15F, System.Drawing.FontStyle.Bold);
			this.btn_FineControlDown.MainFontColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(36)))), ((int)(((byte)(0)))));
			this.btn_FineControlDown.Margin = new System.Windows.Forms.Padding(1, 1, 0, 0);
			this.btn_FineControlDown.Name = "btn_FineControlDown";
			this.btn_FineControlDown.Size = new System.Drawing.Size(32, 35);
			this.btn_FineControlDown.SubFont = new System.Drawing.Font("맑은 고딕", 9F, System.Drawing.FontStyle.Bold);
			this.btn_FineControlDown.SubFontColor = System.Drawing.Color.DarkBlue;
			this.btn_FineControlDown.SubText = "";
			this.btn_FineControlDown.TabIndex = 1423;
			this.btn_FineControlDown.Text = "↓";
			this.btn_FineControlDown.TextAlignMain = Sys3Controls.EN_TEXTALIGN.MIDDLE_CENTER;
			this.btn_FineControlDown.TextAlignSub = Sys3Controls.EN_TEXTALIGN.MIDDLE_LEFT;
			this.btn_FineControlDown.ThemeIndex = 0;
			this.btn_FineControlDown.UseBorder = true;
			this.btn_FineControlDown.UseClickedEmphasizeTextColor = false;
			this.btn_FineControlDown.UseCustomizeClickedColor = false;
			this.btn_FineControlDown.UseEdge = true;
			this.btn_FineControlDown.UseHoverEmphasizeCustomColor = false;
			this.btn_FineControlDown.UseImage = false;
			this.btn_FineControlDown.UserHoverEmpahsize = false;
			this.btn_FineControlDown.UseSubFont = false;
			this.btn_FineControlDown.Click += new System.EventHandler(this.Click_FindControl);
			// 
			// btn_Define_1000
			// 
			this.btn_Define_1000.BorderWidth = 1;
			this.btn_Define_1000.ButtonClicked = false;
			this.btn_Define_1000.ClickedEmphasizeTextColor = System.Drawing.Color.White;
			this.btn_Define_1000.CustomClickedGradientFirstColor = System.Drawing.Color.White;
			this.btn_Define_1000.CustomClickedGradientSecondColor = System.Drawing.Color.White;
			this.btn_Define_1000.Description = "";
			this.btn_Define_1000.DisabledColor = System.Drawing.Color.DarkGray;
			this.btn_Define_1000.EdgeRadius = 5;
			this.btn_Define_1000.GradientAngle = 70F;
			this.btn_Define_1000.GradientFirstColor = System.Drawing.Color.White;
			this.btn_Define_1000.GradientSecondColor = System.Drawing.Color.FromArgb(((int)(((byte)(170)))), ((int)(((byte)(176)))), ((int)(((byte)(183)))));
			this.btn_Define_1000.HoverEmphasizeCustomColor = System.Drawing.Color.White;
			this.btn_Define_1000.ImagePosition = new System.Drawing.Point(10, 10);
			this.btn_Define_1000.ImageSize = new System.Drawing.Point(30, 30);
			this.btn_Define_1000.LoadImage = global::FrameOfSystem3.Properties.Resources.CLEAR_BLACK;
			this.btn_Define_1000.Location = new System.Drawing.Point(204, 36);
			this.btn_Define_1000.MainFont = new System.Drawing.Font("맑은 고딕", 11F, System.Drawing.FontStyle.Bold);
			this.btn_Define_1000.MainFontColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(36)))), ((int)(((byte)(0)))));
			this.btn_Define_1000.Margin = new System.Windows.Forms.Padding(1, 1, 0, 0);
			this.btn_Define_1000.Name = "btn_Define_1000";
			this.btn_Define_1000.Size = new System.Drawing.Size(50, 35);
			this.btn_Define_1000.SubFont = new System.Drawing.Font("맑은 고딕", 9F, System.Drawing.FontStyle.Bold);
			this.btn_Define_1000.SubFontColor = System.Drawing.Color.DarkBlue;
			this.btn_Define_1000.SubText = "";
			this.btn_Define_1000.TabIndex = 1418;
			this.btn_Define_1000.Text = "1000";
			this.btn_Define_1000.TextAlignMain = Sys3Controls.EN_TEXTALIGN.MIDDLE_CENTER;
			this.btn_Define_1000.TextAlignSub = Sys3Controls.EN_TEXTALIGN.MIDDLE_LEFT;
			this.btn_Define_1000.ThemeIndex = 0;
			this.btn_Define_1000.UseBorder = true;
			this.btn_Define_1000.UseClickedEmphasizeTextColor = false;
			this.btn_Define_1000.UseCustomizeClickedColor = false;
			this.btn_Define_1000.UseEdge = true;
			this.btn_Define_1000.UseHoverEmphasizeCustomColor = false;
			this.btn_Define_1000.UseImage = false;
			this.btn_Define_1000.UserHoverEmpahsize = false;
			this.btn_Define_1000.UseSubFont = false;
			this.btn_Define_1000.Click += new System.EventHandler(this.Click_DefinedStep);
			// 
			// btn_Define_500
			// 
			this.btn_Define_500.BorderWidth = 1;
			this.btn_Define_500.ButtonClicked = false;
			this.btn_Define_500.ClickedEmphasizeTextColor = System.Drawing.Color.White;
			this.btn_Define_500.CustomClickedGradientFirstColor = System.Drawing.Color.White;
			this.btn_Define_500.CustomClickedGradientSecondColor = System.Drawing.Color.White;
			this.btn_Define_500.Description = "";
			this.btn_Define_500.DisabledColor = System.Drawing.Color.DarkGray;
			this.btn_Define_500.EdgeRadius = 5;
			this.btn_Define_500.GradientAngle = 70F;
			this.btn_Define_500.GradientFirstColor = System.Drawing.Color.White;
			this.btn_Define_500.GradientSecondColor = System.Drawing.Color.FromArgb(((int)(((byte)(170)))), ((int)(((byte)(176)))), ((int)(((byte)(183)))));
			this.btn_Define_500.HoverEmphasizeCustomColor = System.Drawing.Color.White;
			this.btn_Define_500.ImagePosition = new System.Drawing.Point(10, 10);
			this.btn_Define_500.ImageSize = new System.Drawing.Point(30, 30);
			this.btn_Define_500.LoadImage = global::FrameOfSystem3.Properties.Resources.CLEAR_BLACK;
			this.btn_Define_500.Location = new System.Drawing.Point(153, 36);
			this.btn_Define_500.MainFont = new System.Drawing.Font("맑은 고딕", 11F, System.Drawing.FontStyle.Bold);
			this.btn_Define_500.MainFontColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(36)))), ((int)(((byte)(0)))));
			this.btn_Define_500.Margin = new System.Windows.Forms.Padding(1, 1, 0, 0);
			this.btn_Define_500.Name = "btn_Define_500";
			this.btn_Define_500.Size = new System.Drawing.Size(50, 35);
			this.btn_Define_500.SubFont = new System.Drawing.Font("맑은 고딕", 9F, System.Drawing.FontStyle.Bold);
			this.btn_Define_500.SubFontColor = System.Drawing.Color.DarkBlue;
			this.btn_Define_500.SubText = "";
			this.btn_Define_500.TabIndex = 1419;
			this.btn_Define_500.Text = "500";
			this.btn_Define_500.TextAlignMain = Sys3Controls.EN_TEXTALIGN.MIDDLE_CENTER;
			this.btn_Define_500.TextAlignSub = Sys3Controls.EN_TEXTALIGN.MIDDLE_LEFT;
			this.btn_Define_500.ThemeIndex = 0;
			this.btn_Define_500.UseBorder = true;
			this.btn_Define_500.UseClickedEmphasizeTextColor = false;
			this.btn_Define_500.UseCustomizeClickedColor = false;
			this.btn_Define_500.UseEdge = true;
			this.btn_Define_500.UseHoverEmphasizeCustomColor = false;
			this.btn_Define_500.UseImage = false;
			this.btn_Define_500.UserHoverEmpahsize = false;
			this.btn_Define_500.UseSubFont = false;
			this.btn_Define_500.Click += new System.EventHandler(this.Click_DefinedStep);
			// 
			// btn_Define_100
			// 
			this.btn_Define_100.BorderWidth = 1;
			this.btn_Define_100.ButtonClicked = false;
			this.btn_Define_100.ClickedEmphasizeTextColor = System.Drawing.Color.White;
			this.btn_Define_100.CustomClickedGradientFirstColor = System.Drawing.Color.White;
			this.btn_Define_100.CustomClickedGradientSecondColor = System.Drawing.Color.White;
			this.btn_Define_100.Description = "";
			this.btn_Define_100.DisabledColor = System.Drawing.Color.DarkGray;
			this.btn_Define_100.EdgeRadius = 5;
			this.btn_Define_100.GradientAngle = 70F;
			this.btn_Define_100.GradientFirstColor = System.Drawing.Color.White;
			this.btn_Define_100.GradientSecondColor = System.Drawing.Color.FromArgb(((int)(((byte)(170)))), ((int)(((byte)(176)))), ((int)(((byte)(183)))));
			this.btn_Define_100.HoverEmphasizeCustomColor = System.Drawing.Color.White;
			this.btn_Define_100.ImagePosition = new System.Drawing.Point(10, 10);
			this.btn_Define_100.ImageSize = new System.Drawing.Point(30, 30);
			this.btn_Define_100.LoadImage = global::FrameOfSystem3.Properties.Resources.CLEAR_BLACK;
			this.btn_Define_100.Location = new System.Drawing.Point(102, 36);
			this.btn_Define_100.MainFont = new System.Drawing.Font("맑은 고딕", 11F, System.Drawing.FontStyle.Bold);
			this.btn_Define_100.MainFontColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(36)))), ((int)(((byte)(0)))));
			this.btn_Define_100.Margin = new System.Windows.Forms.Padding(1, 1, 0, 0);
			this.btn_Define_100.Name = "btn_Define_100";
			this.btn_Define_100.Size = new System.Drawing.Size(50, 35);
			this.btn_Define_100.SubFont = new System.Drawing.Font("맑은 고딕", 9F, System.Drawing.FontStyle.Bold);
			this.btn_Define_100.SubFontColor = System.Drawing.Color.DarkBlue;
			this.btn_Define_100.SubText = "";
			this.btn_Define_100.TabIndex = 1420;
			this.btn_Define_100.Text = "100";
			this.btn_Define_100.TextAlignMain = Sys3Controls.EN_TEXTALIGN.MIDDLE_CENTER;
			this.btn_Define_100.TextAlignSub = Sys3Controls.EN_TEXTALIGN.MIDDLE_LEFT;
			this.btn_Define_100.ThemeIndex = 0;
			this.btn_Define_100.UseBorder = true;
			this.btn_Define_100.UseClickedEmphasizeTextColor = false;
			this.btn_Define_100.UseCustomizeClickedColor = false;
			this.btn_Define_100.UseEdge = true;
			this.btn_Define_100.UseHoverEmphasizeCustomColor = false;
			this.btn_Define_100.UseImage = false;
			this.btn_Define_100.UserHoverEmpahsize = false;
			this.btn_Define_100.UseSubFont = false;
			this.btn_Define_100.Click += new System.EventHandler(this.Click_DefinedStep);
			// 
			// btn_Define_10
			// 
			this.btn_Define_10.BorderWidth = 1;
			this.btn_Define_10.ButtonClicked = false;
			this.btn_Define_10.ClickedEmphasizeTextColor = System.Drawing.Color.White;
			this.btn_Define_10.CustomClickedGradientFirstColor = System.Drawing.Color.White;
			this.btn_Define_10.CustomClickedGradientSecondColor = System.Drawing.Color.White;
			this.btn_Define_10.Description = "";
			this.btn_Define_10.DisabledColor = System.Drawing.Color.DarkGray;
			this.btn_Define_10.EdgeRadius = 5;
			this.btn_Define_10.GradientAngle = 70F;
			this.btn_Define_10.GradientFirstColor = System.Drawing.Color.White;
			this.btn_Define_10.GradientSecondColor = System.Drawing.Color.FromArgb(((int)(((byte)(170)))), ((int)(((byte)(176)))), ((int)(((byte)(183)))));
			this.btn_Define_10.HoverEmphasizeCustomColor = System.Drawing.Color.White;
			this.btn_Define_10.ImagePosition = new System.Drawing.Point(10, 10);
			this.btn_Define_10.ImageSize = new System.Drawing.Point(30, 30);
			this.btn_Define_10.LoadImage = global::FrameOfSystem3.Properties.Resources.CLEAR_BLACK;
			this.btn_Define_10.Location = new System.Drawing.Point(51, 36);
			this.btn_Define_10.MainFont = new System.Drawing.Font("맑은 고딕", 11F, System.Drawing.FontStyle.Bold);
			this.btn_Define_10.MainFontColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(36)))), ((int)(((byte)(0)))));
			this.btn_Define_10.Margin = new System.Windows.Forms.Padding(1, 1, 0, 0);
			this.btn_Define_10.Name = "btn_Define_10";
			this.btn_Define_10.Size = new System.Drawing.Size(50, 35);
			this.btn_Define_10.SubFont = new System.Drawing.Font("맑은 고딕", 9F, System.Drawing.FontStyle.Bold);
			this.btn_Define_10.SubFontColor = System.Drawing.Color.DarkBlue;
			this.btn_Define_10.SubText = "";
			this.btn_Define_10.TabIndex = 1421;
			this.btn_Define_10.Text = "10";
			this.btn_Define_10.TextAlignMain = Sys3Controls.EN_TEXTALIGN.MIDDLE_CENTER;
			this.btn_Define_10.TextAlignSub = Sys3Controls.EN_TEXTALIGN.MIDDLE_LEFT;
			this.btn_Define_10.ThemeIndex = 0;
			this.btn_Define_10.UseBorder = true;
			this.btn_Define_10.UseClickedEmphasizeTextColor = false;
			this.btn_Define_10.UseCustomizeClickedColor = false;
			this.btn_Define_10.UseEdge = true;
			this.btn_Define_10.UseHoverEmphasizeCustomColor = false;
			this.btn_Define_10.UseImage = false;
			this.btn_Define_10.UserHoverEmpahsize = false;
			this.btn_Define_10.UseSubFont = false;
			this.btn_Define_10.Click += new System.EventHandler(this.Click_DefinedStep);
			// 
			// btn_Define_1
			// 
			this.btn_Define_1.BorderWidth = 1;
			this.btn_Define_1.ButtonClicked = false;
			this.btn_Define_1.ClickedEmphasizeTextColor = System.Drawing.Color.White;
			this.btn_Define_1.CustomClickedGradientFirstColor = System.Drawing.Color.White;
			this.btn_Define_1.CustomClickedGradientSecondColor = System.Drawing.Color.White;
			this.btn_Define_1.Description = "";
			this.btn_Define_1.DisabledColor = System.Drawing.Color.DarkGray;
			this.btn_Define_1.EdgeRadius = 5;
			this.btn_Define_1.GradientAngle = 70F;
			this.btn_Define_1.GradientFirstColor = System.Drawing.Color.White;
			this.btn_Define_1.GradientSecondColor = System.Drawing.Color.FromArgb(((int)(((byte)(170)))), ((int)(((byte)(176)))), ((int)(((byte)(183)))));
			this.btn_Define_1.HoverEmphasizeCustomColor = System.Drawing.Color.White;
			this.btn_Define_1.ImagePosition = new System.Drawing.Point(10, 10);
			this.btn_Define_1.ImageSize = new System.Drawing.Point(30, 30);
			this.btn_Define_1.LoadImage = global::FrameOfSystem3.Properties.Resources.CLEAR_BLACK;
			this.btn_Define_1.Location = new System.Drawing.Point(0, 36);
			this.btn_Define_1.MainFont = new System.Drawing.Font("맑은 고딕", 11F, System.Drawing.FontStyle.Bold);
			this.btn_Define_1.MainFontColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(36)))), ((int)(((byte)(0)))));
			this.btn_Define_1.Margin = new System.Windows.Forms.Padding(1, 1, 0, 0);
			this.btn_Define_1.Name = "btn_Define_1";
			this.btn_Define_1.Size = new System.Drawing.Size(50, 35);
			this.btn_Define_1.SubFont = new System.Drawing.Font("맑은 고딕", 9F, System.Drawing.FontStyle.Bold);
			this.btn_Define_1.SubFontColor = System.Drawing.Color.DarkBlue;
			this.btn_Define_1.SubText = "";
			this.btn_Define_1.TabIndex = 1422;
			this.btn_Define_1.Text = "1";
			this.btn_Define_1.TextAlignMain = Sys3Controls.EN_TEXTALIGN.MIDDLE_CENTER;
			this.btn_Define_1.TextAlignSub = Sys3Controls.EN_TEXTALIGN.MIDDLE_LEFT;
			this.btn_Define_1.ThemeIndex = 0;
			this.btn_Define_1.UseBorder = true;
			this.btn_Define_1.UseClickedEmphasizeTextColor = false;
			this.btn_Define_1.UseCustomizeClickedColor = false;
			this.btn_Define_1.UseEdge = true;
			this.btn_Define_1.UseHoverEmphasizeCustomColor = false;
			this.btn_Define_1.UseImage = false;
			this.btn_Define_1.UserHoverEmpahsize = false;
			this.btn_Define_1.UseSubFont = false;
			this.btn_Define_1.Click += new System.EventHandler(this.Click_DefinedStep);
			// 
			// btn_FineControlUp
			// 
			this.btn_FineControlUp.BorderWidth = 1;
			this.btn_FineControlUp.ButtonClicked = false;
			this.btn_FineControlUp.ClickedEmphasizeTextColor = System.Drawing.Color.White;
			this.btn_FineControlUp.CustomClickedGradientFirstColor = System.Drawing.Color.White;
			this.btn_FineControlUp.CustomClickedGradientSecondColor = System.Drawing.Color.White;
			this.btn_FineControlUp.Description = "";
			this.btn_FineControlUp.DisabledColor = System.Drawing.Color.DarkGray;
			this.btn_FineControlUp.EdgeRadius = 5;
			this.btn_FineControlUp.GradientAngle = 70F;
			this.btn_FineControlUp.GradientFirstColor = System.Drawing.Color.White;
			this.btn_FineControlUp.GradientSecondColor = System.Drawing.Color.FromArgb(((int)(((byte)(170)))), ((int)(((byte)(176)))), ((int)(((byte)(183)))));
			this.btn_FineControlUp.HoverEmphasizeCustomColor = System.Drawing.Color.White;
			this.btn_FineControlUp.ImagePosition = new System.Drawing.Point(10, 10);
			this.btn_FineControlUp.ImageSize = new System.Drawing.Point(30, 30);
			this.btn_FineControlUp.LoadImage = global::FrameOfSystem3.Properties.Resources.CLEAR_BLACK;
			this.btn_FineControlUp.Location = new System.Drawing.Point(255, 0);
			this.btn_FineControlUp.MainFont = new System.Drawing.Font("맑은 고딕", 15F, System.Drawing.FontStyle.Bold);
			this.btn_FineControlUp.MainFontColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(36)))), ((int)(((byte)(0)))));
			this.btn_FineControlUp.Margin = new System.Windows.Forms.Padding(1, 1, 0, 0);
			this.btn_FineControlUp.Name = "btn_FineControlUp";
			this.btn_FineControlUp.Size = new System.Drawing.Size(32, 35);
			this.btn_FineControlUp.SubFont = new System.Drawing.Font("맑은 고딕", 9F, System.Drawing.FontStyle.Bold);
			this.btn_FineControlUp.SubFontColor = System.Drawing.Color.DarkBlue;
			this.btn_FineControlUp.SubText = "";
			this.btn_FineControlUp.TabIndex = 1424;
			this.btn_FineControlUp.Text = "↑";
			this.btn_FineControlUp.TextAlignMain = Sys3Controls.EN_TEXTALIGN.MIDDLE_CENTER;
			this.btn_FineControlUp.TextAlignSub = Sys3Controls.EN_TEXTALIGN.MIDDLE_LEFT;
			this.btn_FineControlUp.ThemeIndex = 0;
			this.btn_FineControlUp.UseBorder = true;
			this.btn_FineControlUp.UseClickedEmphasizeTextColor = false;
			this.btn_FineControlUp.UseCustomizeClickedColor = false;
			this.btn_FineControlUp.UseEdge = true;
			this.btn_FineControlUp.UseHoverEmphasizeCustomColor = false;
			this.btn_FineControlUp.UseImage = false;
			this.btn_FineControlUp.UserHoverEmpahsize = false;
			this.btn_FineControlUp.UseSubFont = false;
			this.btn_FineControlUp.Click += new System.EventHandler(this.Click_FindControl);
			// 
			// JogPanel_ModeRelative
			// 
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
			this.Controls.Add(this.btn_FineControlDown);
			this.Controls.Add(this.m_lblRelative);
			this.Controls.Add(this.lbl_Distance);
			this.Controls.Add(this.btn_Define_1000);
			this.Controls.Add(this.btn_Define_500);
			this.Controls.Add(this.btn_Define_100);
			this.Controls.Add(this.btn_Define_10);
			this.Controls.Add(this.btn_Define_1);
			this.Controls.Add(this.btn_FineControlUp);
			this.Margin = new System.Windows.Forms.Padding(1, 1, 0, 0);
			this.Name = "JogPanel_ModeRelative";
			this.Padding = new System.Windows.Forms.Padding(1);
			this.Size = new System.Drawing.Size(287, 70);
			this.Tag = "";
			this.ResumeLayout(false);

		}

		#endregion

		private Sys3Controls.Sys3Label m_lblRelative;
		private Sys3Controls.Sys3Label lbl_Distance;
		private Sys3Controls.Sys3button btn_Define_1000;
		private Sys3Controls.Sys3button btn_Define_500;
		private Sys3Controls.Sys3button btn_Define_100;
		private Sys3Controls.Sys3button btn_Define_10;
		private Sys3Controls.Sys3button btn_Define_1;
		private Sys3Controls.Sys3button btn_FineControlDown;
		private Sys3Controls.Sys3button btn_FineControlUp;





	}
}
