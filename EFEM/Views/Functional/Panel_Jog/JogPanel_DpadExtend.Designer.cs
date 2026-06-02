namespace FrameOfSystem3.Views.Functional.Jog
{
	partial class JogPanel_DpadExtend
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
			this.btn_Stop = new Sys3Controls.Sys3button();
			this.btn_Right = new Sys3Controls.Sys3button();
			this.btn_UpRight = new Sys3Controls.Sys3button();
			this.btn_Up = new Sys3Controls.Sys3button();
			this.btn_UpLeft = new Sys3Controls.Sys3button();
			this.btn_Left = new Sys3Controls.Sys3button();
			this.btn_ExtraDown = new Sys3Controls.Sys3button();
			this.btn_ExtraUp = new Sys3Controls.Sys3button();
			this.btn_ExtraRight = new Sys3Controls.Sys3button();
			this.btn_ExtraLeft = new Sys3Controls.Sys3button();
			this.btn_DownLeft = new Sys3Controls.Sys3button();
			this.btn_Down = new Sys3Controls.Sys3button();
			this.btn_DownRight = new Sys3Controls.Sys3button();
			this.SuspendLayout();
			// 
			// btn_Stop
			// 
			this.btn_Stop.BorderWidth = 3;
			this.btn_Stop.ButtonClicked = false;
			this.btn_Stop.ClickedEmphasizeTextColor = System.Drawing.Color.White;
			this.btn_Stop.CustomClickedGradientFirstColor = System.Drawing.Color.White;
			this.btn_Stop.CustomClickedGradientSecondColor = System.Drawing.Color.White;
			this.btn_Stop.Description = "";
			this.btn_Stop.DisabledColor = System.Drawing.Color.DarkGray;
			this.btn_Stop.EdgeRadius = 5;
			this.btn_Stop.GradientAngle = 70F;
			this.btn_Stop.GradientFirstColor = System.Drawing.Color.LightPink;
			this.btn_Stop.GradientSecondColor = System.Drawing.Color.DarkRed;
			this.btn_Stop.HoverEmphasizeCustomColor = System.Drawing.Color.White;
			this.btn_Stop.ImagePosition = new System.Drawing.Point(11, 11);
			this.btn_Stop.ImageSize = new System.Drawing.Point(60, 60);
			this.btn_Stop.LoadImage = global::FrameOfSystem3.Properties.Resources.stop_sign_w_104px;
			this.btn_Stop.Location = new System.Drawing.Point(81, 81);
			this.btn_Stop.MainFont = new System.Drawing.Font("맑은 고딕", 11F, System.Drawing.FontStyle.Bold);
			this.btn_Stop.MainFontColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(36)))), ((int)(((byte)(0)))));
			this.btn_Stop.Margin = new System.Windows.Forms.Padding(1, 1, 0, 0);
			this.btn_Stop.Name = "btn_Stop";
			this.btn_Stop.Size = new System.Drawing.Size(80, 80);
			this.btn_Stop.SubFont = new System.Drawing.Font("맑은 고딕", 8F, System.Drawing.FontStyle.Bold);
			this.btn_Stop.SubFontColor = System.Drawing.Color.DarkBlue;
			this.btn_Stop.SubText = "";
			this.btn_Stop.TabIndex = 13;
			this.btn_Stop.TextAlignMain = Sys3Controls.EN_TEXTALIGN.MIDDLE_CENTER;
			this.btn_Stop.TextAlignSub = Sys3Controls.EN_TEXTALIGN.TOP_RIGHT;
			this.btn_Stop.ThemeIndex = 0;
			this.btn_Stop.UseBorder = true;
			this.btn_Stop.UseClickedEmphasizeTextColor = false;
			this.btn_Stop.UseCustomizeClickedColor = false;
			this.btn_Stop.UseEdge = true;
			this.btn_Stop.UseHoverEmphasizeCustomColor = false;
			this.btn_Stop.UseImage = true;
			this.btn_Stop.UserHoverEmpahsize = false;
			this.btn_Stop.UseSubFont = false;
			this.btn_Stop.Click += new System.EventHandler(this.Click_Stop);
			// 
			// btn_Right
			// 
			this.btn_Right.BorderWidth = 3;
			this.btn_Right.ButtonClicked = false;
			this.btn_Right.ClickedEmphasizeTextColor = System.Drawing.Color.White;
			this.btn_Right.CustomClickedGradientFirstColor = System.Drawing.Color.White;
			this.btn_Right.CustomClickedGradientSecondColor = System.Drawing.Color.White;
			this.btn_Right.Description = "";
			this.btn_Right.DisabledColor = System.Drawing.Color.DarkGray;
			this.btn_Right.EdgeRadius = 5;
			this.btn_Right.GradientAngle = 70F;
			this.btn_Right.GradientFirstColor = System.Drawing.Color.White;
			this.btn_Right.GradientSecondColor = System.Drawing.Color.DarkGray;
			this.btn_Right.HoverEmphasizeCustomColor = System.Drawing.Color.White;
			this.btn_Right.ImagePosition = new System.Drawing.Point(20, 20);
			this.btn_Right.ImageSize = new System.Drawing.Point(40, 40);
			this.btn_Right.LoadImage = global::FrameOfSystem3.Properties.Resources.right_104px;
			this.btn_Right.Location = new System.Drawing.Point(162, 81);
			this.btn_Right.MainFont = new System.Drawing.Font("맑은 고딕", 30F, System.Drawing.FontStyle.Bold);
			this.btn_Right.MainFontColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(36)))), ((int)(((byte)(0)))));
			this.btn_Right.Margin = new System.Windows.Forms.Padding(1, 1, 0, 0);
			this.btn_Right.Name = "btn_Right";
			this.btn_Right.Size = new System.Drawing.Size(80, 80);
			this.btn_Right.SubFont = new System.Drawing.Font("맑은 고딕", 8F, System.Drawing.FontStyle.Bold);
			this.btn_Right.SubFontColor = System.Drawing.Color.DarkBlue;
			this.btn_Right.SubText = "MOVE";
			this.btn_Right.TabIndex = 14;
			this.btn_Right.TextAlignMain = Sys3Controls.EN_TEXTALIGN.MIDDLE_CENTER;
			this.btn_Right.TextAlignSub = Sys3Controls.EN_TEXTALIGN.TOP_RIGHT;
			this.btn_Right.ThemeIndex = 0;
			this.btn_Right.UseBorder = true;
			this.btn_Right.UseClickedEmphasizeTextColor = false;
			this.btn_Right.UseCustomizeClickedColor = false;
			this.btn_Right.UseEdge = true;
			this.btn_Right.UseHoverEmphasizeCustomColor = false;
			this.btn_Right.UseImage = true;
			this.btn_Right.UserHoverEmpahsize = false;
			this.btn_Right.UseSubFont = false;
			this.btn_Right.MouseDown += new System.Windows.Forms.MouseEventHandler(this.Down_Mouse);
			this.btn_Right.MouseUp += new System.Windows.Forms.MouseEventHandler(this.Up_Mouse);
			// 
			// btn_UpRight
			// 
			this.btn_UpRight.BorderWidth = 3;
			this.btn_UpRight.ButtonClicked = false;
			this.btn_UpRight.ClickedEmphasizeTextColor = System.Drawing.Color.White;
			this.btn_UpRight.CustomClickedGradientFirstColor = System.Drawing.Color.White;
			this.btn_UpRight.CustomClickedGradientSecondColor = System.Drawing.Color.White;
			this.btn_UpRight.Description = "";
			this.btn_UpRight.DisabledColor = System.Drawing.Color.DarkGray;
			this.btn_UpRight.EdgeRadius = 5;
			this.btn_UpRight.GradientAngle = 70F;
			this.btn_UpRight.GradientFirstColor = System.Drawing.Color.White;
			this.btn_UpRight.GradientSecondColor = System.Drawing.Color.DarkGray;
			this.btn_UpRight.HoverEmphasizeCustomColor = System.Drawing.Color.White;
			this.btn_UpRight.ImagePosition = new System.Drawing.Point(20, 20);
			this.btn_UpRight.ImageSize = new System.Drawing.Point(40, 40);
			this.btn_UpRight.LoadImage = global::FrameOfSystem3.Properties.Resources.up_right_104px;
			this.btn_UpRight.Location = new System.Drawing.Point(162, 0);
			this.btn_UpRight.MainFont = new System.Drawing.Font("맑은 고딕", 30F, System.Drawing.FontStyle.Bold);
			this.btn_UpRight.MainFontColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(36)))), ((int)(((byte)(0)))));
			this.btn_UpRight.Margin = new System.Windows.Forms.Padding(1, 1, 0, 0);
			this.btn_UpRight.Name = "btn_UpRight";
			this.btn_UpRight.Size = new System.Drawing.Size(80, 80);
			this.btn_UpRight.SubFont = new System.Drawing.Font("맑은 고딕", 8F, System.Drawing.FontStyle.Bold);
			this.btn_UpRight.SubFontColor = System.Drawing.Color.DarkBlue;
			this.btn_UpRight.SubText = "MOVE";
			this.btn_UpRight.TabIndex = 11;
			this.btn_UpRight.TextAlignMain = Sys3Controls.EN_TEXTALIGN.MIDDLE_CENTER;
			this.btn_UpRight.TextAlignSub = Sys3Controls.EN_TEXTALIGN.TOP_RIGHT;
			this.btn_UpRight.ThemeIndex = 0;
			this.btn_UpRight.UseBorder = true;
			this.btn_UpRight.UseClickedEmphasizeTextColor = false;
			this.btn_UpRight.UseCustomizeClickedColor = false;
			this.btn_UpRight.UseEdge = true;
			this.btn_UpRight.UseHoverEmphasizeCustomColor = false;
			this.btn_UpRight.UseImage = true;
			this.btn_UpRight.UserHoverEmpahsize = false;
			this.btn_UpRight.UseSubFont = false;
			this.btn_UpRight.MouseDown += new System.Windows.Forms.MouseEventHandler(this.Down_Mouse);
			this.btn_UpRight.MouseUp += new System.Windows.Forms.MouseEventHandler(this.Up_Mouse);
			// 
			// btn_Up
			// 
			this.btn_Up.BorderWidth = 3;
			this.btn_Up.ButtonClicked = false;
			this.btn_Up.ClickedEmphasizeTextColor = System.Drawing.Color.White;
			this.btn_Up.CustomClickedGradientFirstColor = System.Drawing.Color.White;
			this.btn_Up.CustomClickedGradientSecondColor = System.Drawing.Color.White;
			this.btn_Up.Description = "";
			this.btn_Up.DisabledColor = System.Drawing.Color.DarkGray;
			this.btn_Up.EdgeRadius = 5;
			this.btn_Up.GradientAngle = 70F;
			this.btn_Up.GradientFirstColor = System.Drawing.Color.White;
			this.btn_Up.GradientSecondColor = System.Drawing.Color.DarkGray;
			this.btn_Up.HoverEmphasizeCustomColor = System.Drawing.Color.White;
			this.btn_Up.ImagePosition = new System.Drawing.Point(20, 20);
			this.btn_Up.ImageSize = new System.Drawing.Point(40, 40);
			this.btn_Up.LoadImage = global::FrameOfSystem3.Properties.Resources.up_104px;
			this.btn_Up.Location = new System.Drawing.Point(81, 0);
			this.btn_Up.MainFont = new System.Drawing.Font("맑은 고딕", 30F, System.Drawing.FontStyle.Bold);
			this.btn_Up.MainFontColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(36)))), ((int)(((byte)(0)))));
			this.btn_Up.Margin = new System.Windows.Forms.Padding(1, 1, 0, 0);
			this.btn_Up.Name = "btn_Up";
			this.btn_Up.Size = new System.Drawing.Size(80, 80);
			this.btn_Up.SubFont = new System.Drawing.Font("맑은 고딕", 8F, System.Drawing.FontStyle.Bold);
			this.btn_Up.SubFontColor = System.Drawing.Color.DarkBlue;
			this.btn_Up.SubText = "MOVE";
			this.btn_Up.TabIndex = 10;
			this.btn_Up.TextAlignMain = Sys3Controls.EN_TEXTALIGN.MIDDLE_CENTER;
			this.btn_Up.TextAlignSub = Sys3Controls.EN_TEXTALIGN.TOP_RIGHT;
			this.btn_Up.ThemeIndex = 0;
			this.btn_Up.UseBorder = true;
			this.btn_Up.UseClickedEmphasizeTextColor = false;
			this.btn_Up.UseCustomizeClickedColor = false;
			this.btn_Up.UseEdge = true;
			this.btn_Up.UseHoverEmphasizeCustomColor = false;
			this.btn_Up.UseImage = true;
			this.btn_Up.UserHoverEmpahsize = false;
			this.btn_Up.UseSubFont = false;
			this.btn_Up.MouseDown += new System.Windows.Forms.MouseEventHandler(this.Down_Mouse);
			this.btn_Up.MouseUp += new System.Windows.Forms.MouseEventHandler(this.Up_Mouse);
			// 
			// btn_UpLeft
			// 
			this.btn_UpLeft.BorderWidth = 3;
			this.btn_UpLeft.ButtonClicked = false;
			this.btn_UpLeft.ClickedEmphasizeTextColor = System.Drawing.Color.White;
			this.btn_UpLeft.CustomClickedGradientFirstColor = System.Drawing.Color.White;
			this.btn_UpLeft.CustomClickedGradientSecondColor = System.Drawing.Color.White;
			this.btn_UpLeft.Description = "";
			this.btn_UpLeft.DisabledColor = System.Drawing.Color.DarkGray;
			this.btn_UpLeft.EdgeRadius = 5;
			this.btn_UpLeft.GradientAngle = 70F;
			this.btn_UpLeft.GradientFirstColor = System.Drawing.Color.White;
			this.btn_UpLeft.GradientSecondColor = System.Drawing.Color.DarkGray;
			this.btn_UpLeft.HoverEmphasizeCustomColor = System.Drawing.Color.White;
			this.btn_UpLeft.ImagePosition = new System.Drawing.Point(20, 20);
			this.btn_UpLeft.ImageSize = new System.Drawing.Point(40, 40);
			this.btn_UpLeft.LoadImage = global::FrameOfSystem3.Properties.Resources.up_left_104px;
			this.btn_UpLeft.Location = new System.Drawing.Point(0, 0);
			this.btn_UpLeft.MainFont = new System.Drawing.Font("맑은 고딕", 30F, System.Drawing.FontStyle.Bold);
			this.btn_UpLeft.MainFontColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(36)))), ((int)(((byte)(0)))));
			this.btn_UpLeft.Margin = new System.Windows.Forms.Padding(1, 1, 0, 0);
			this.btn_UpLeft.Name = "btn_UpLeft";
			this.btn_UpLeft.Size = new System.Drawing.Size(80, 80);
			this.btn_UpLeft.SubFont = new System.Drawing.Font("맑은 고딕", 8F, System.Drawing.FontStyle.Bold);
			this.btn_UpLeft.SubFontColor = System.Drawing.Color.DarkBlue;
			this.btn_UpLeft.SubText = "MOVE";
			this.btn_UpLeft.TabIndex = 9;
			this.btn_UpLeft.TextAlignMain = Sys3Controls.EN_TEXTALIGN.MIDDLE_CENTER;
			this.btn_UpLeft.TextAlignSub = Sys3Controls.EN_TEXTALIGN.TOP_RIGHT;
			this.btn_UpLeft.ThemeIndex = 0;
			this.btn_UpLeft.UseBorder = true;
			this.btn_UpLeft.UseClickedEmphasizeTextColor = false;
			this.btn_UpLeft.UseCustomizeClickedColor = false;
			this.btn_UpLeft.UseEdge = true;
			this.btn_UpLeft.UseHoverEmphasizeCustomColor = false;
			this.btn_UpLeft.UseImage = true;
			this.btn_UpLeft.UserHoverEmpahsize = false;
			this.btn_UpLeft.UseSubFont = false;
			this.btn_UpLeft.MouseDown += new System.Windows.Forms.MouseEventHandler(this.Down_Mouse);
			this.btn_UpLeft.MouseUp += new System.Windows.Forms.MouseEventHandler(this.Up_Mouse);
			// 
			// btn_Left
			// 
			this.btn_Left.BorderWidth = 3;
			this.btn_Left.ButtonClicked = false;
			this.btn_Left.ClickedEmphasizeTextColor = System.Drawing.Color.White;
			this.btn_Left.CustomClickedGradientFirstColor = System.Drawing.Color.White;
			this.btn_Left.CustomClickedGradientSecondColor = System.Drawing.Color.White;
			this.btn_Left.Description = "";
			this.btn_Left.DisabledColor = System.Drawing.Color.DarkGray;
			this.btn_Left.EdgeRadius = 5;
			this.btn_Left.GradientAngle = 70F;
			this.btn_Left.GradientFirstColor = System.Drawing.Color.White;
			this.btn_Left.GradientSecondColor = System.Drawing.Color.DarkGray;
			this.btn_Left.HoverEmphasizeCustomColor = System.Drawing.Color.White;
			this.btn_Left.ImagePosition = new System.Drawing.Point(20, 20);
			this.btn_Left.ImageSize = new System.Drawing.Point(40, 40);
			this.btn_Left.LoadImage = global::FrameOfSystem3.Properties.Resources.left_104px;
			this.btn_Left.Location = new System.Drawing.Point(0, 81);
			this.btn_Left.MainFont = new System.Drawing.Font("맑은 고딕", 30F, System.Drawing.FontStyle.Bold);
			this.btn_Left.MainFontColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(36)))), ((int)(((byte)(0)))));
			this.btn_Left.Margin = new System.Windows.Forms.Padding(1, 1, 0, 0);
			this.btn_Left.Name = "btn_Left";
			this.btn_Left.Size = new System.Drawing.Size(80, 80);
			this.btn_Left.SubFont = new System.Drawing.Font("맑은 고딕", 8F, System.Drawing.FontStyle.Bold);
			this.btn_Left.SubFontColor = System.Drawing.Color.DarkBlue;
			this.btn_Left.SubText = "MOVE";
			this.btn_Left.TabIndex = 12;
			this.btn_Left.TextAlignMain = Sys3Controls.EN_TEXTALIGN.MIDDLE_CENTER;
			this.btn_Left.TextAlignSub = Sys3Controls.EN_TEXTALIGN.TOP_RIGHT;
			this.btn_Left.ThemeIndex = 0;
			this.btn_Left.UseBorder = true;
			this.btn_Left.UseClickedEmphasizeTextColor = false;
			this.btn_Left.UseCustomizeClickedColor = false;
			this.btn_Left.UseEdge = true;
			this.btn_Left.UseHoverEmphasizeCustomColor = false;
			this.btn_Left.UseImage = true;
			this.btn_Left.UserHoverEmpahsize = false;
			this.btn_Left.UseSubFont = false;
			this.btn_Left.MouseDown += new System.Windows.Forms.MouseEventHandler(this.Down_Mouse);
			this.btn_Left.MouseUp += new System.Windows.Forms.MouseEventHandler(this.Up_Mouse);
			// 
			// btn_ExtraDown
			// 
			this.btn_ExtraDown.BorderWidth = 3;
			this.btn_ExtraDown.ButtonClicked = false;
			this.btn_ExtraDown.ClickedEmphasizeTextColor = System.Drawing.Color.White;
			this.btn_ExtraDown.CustomClickedGradientFirstColor = System.Drawing.Color.White;
			this.btn_ExtraDown.CustomClickedGradientSecondColor = System.Drawing.Color.White;
			this.btn_ExtraDown.Description = "";
			this.btn_ExtraDown.DisabledColor = System.Drawing.Color.DarkGray;
			this.btn_ExtraDown.EdgeRadius = 5;
			this.btn_ExtraDown.GradientAngle = 70F;
			this.btn_ExtraDown.GradientFirstColor = System.Drawing.Color.White;
			this.btn_ExtraDown.GradientSecondColor = System.Drawing.Color.DarkGray;
			this.btn_ExtraDown.HoverEmphasizeCustomColor = System.Drawing.Color.White;
			this.btn_ExtraDown.ImagePosition = new System.Drawing.Point(7, 30);
			this.btn_ExtraDown.ImageSize = new System.Drawing.Point(30, 60);
			this.btn_ExtraDown.LoadImage = global::FrameOfSystem3.Properties.Resources.down_96px;
			this.btn_ExtraDown.Location = new System.Drawing.Point(243, 122);
			this.btn_ExtraDown.MainFont = new System.Drawing.Font("맑은 고딕", 30F, System.Drawing.FontStyle.Bold);
			this.btn_ExtraDown.MainFontColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(36)))), ((int)(((byte)(0)))));
			this.btn_ExtraDown.Margin = new System.Windows.Forms.Padding(1, 1, 0, 0);
			this.btn_ExtraDown.Name = "btn_ExtraDown";
			this.btn_ExtraDown.Size = new System.Drawing.Size(44, 120);
			this.btn_ExtraDown.SubFont = new System.Drawing.Font("맑은 고딕", 8F, System.Drawing.FontStyle.Bold);
			this.btn_ExtraDown.SubFontColor = System.Drawing.Color.DarkBlue;
			this.btn_ExtraDown.SubText = "MOVE";
			this.btn_ExtraDown.TabIndex = 15;
			this.btn_ExtraDown.TextAlignMain = Sys3Controls.EN_TEXTALIGN.MIDDLE_CENTER;
			this.btn_ExtraDown.TextAlignSub = Sys3Controls.EN_TEXTALIGN.TOP_RIGHT;
			this.btn_ExtraDown.ThemeIndex = 0;
			this.btn_ExtraDown.UseBorder = true;
			this.btn_ExtraDown.UseClickedEmphasizeTextColor = false;
			this.btn_ExtraDown.UseCustomizeClickedColor = false;
			this.btn_ExtraDown.UseEdge = true;
			this.btn_ExtraDown.UseHoverEmphasizeCustomColor = false;
			this.btn_ExtraDown.UseImage = true;
			this.btn_ExtraDown.UserHoverEmpahsize = false;
			this.btn_ExtraDown.UseSubFont = false;
			this.btn_ExtraDown.MouseDown += new System.Windows.Forms.MouseEventHandler(this.Down_Mouse);
			this.btn_ExtraDown.MouseUp += new System.Windows.Forms.MouseEventHandler(this.Up_Mouse);
			// 
			// btn_ExtraUp
			// 
			this.btn_ExtraUp.BorderWidth = 3;
			this.btn_ExtraUp.ButtonClicked = false;
			this.btn_ExtraUp.ClickedEmphasizeTextColor = System.Drawing.Color.White;
			this.btn_ExtraUp.CustomClickedGradientFirstColor = System.Drawing.Color.White;
			this.btn_ExtraUp.CustomClickedGradientSecondColor = System.Drawing.Color.White;
			this.btn_ExtraUp.Description = "";
			this.btn_ExtraUp.DisabledColor = System.Drawing.Color.DarkGray;
			this.btn_ExtraUp.EdgeRadius = 5;
			this.btn_ExtraUp.GradientAngle = 70F;
			this.btn_ExtraUp.GradientFirstColor = System.Drawing.Color.White;
			this.btn_ExtraUp.GradientSecondColor = System.Drawing.Color.DarkGray;
			this.btn_ExtraUp.HoverEmphasizeCustomColor = System.Drawing.Color.White;
			this.btn_ExtraUp.ImagePosition = new System.Drawing.Point(7, 30);
			this.btn_ExtraUp.ImageSize = new System.Drawing.Point(30, 60);
			this.btn_ExtraUp.LoadImage = global::FrameOfSystem3.Properties.Resources.up_96px;
			this.btn_ExtraUp.Location = new System.Drawing.Point(243, 0);
			this.btn_ExtraUp.MainFont = new System.Drawing.Font("맑은 고딕", 30F, System.Drawing.FontStyle.Bold);
			this.btn_ExtraUp.MainFontColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(36)))), ((int)(((byte)(0)))));
			this.btn_ExtraUp.Margin = new System.Windows.Forms.Padding(1, 1, 0, 0);
			this.btn_ExtraUp.Name = "btn_ExtraUp";
			this.btn_ExtraUp.Size = new System.Drawing.Size(44, 120);
			this.btn_ExtraUp.SubFont = new System.Drawing.Font("맑은 고딕", 8F, System.Drawing.FontStyle.Bold);
			this.btn_ExtraUp.SubFontColor = System.Drawing.Color.DarkBlue;
			this.btn_ExtraUp.SubText = "MOVE";
			this.btn_ExtraUp.TabIndex = 15;
			this.btn_ExtraUp.TextAlignMain = Sys3Controls.EN_TEXTALIGN.MIDDLE_CENTER;
			this.btn_ExtraUp.TextAlignSub = Sys3Controls.EN_TEXTALIGN.TOP_RIGHT;
			this.btn_ExtraUp.ThemeIndex = 0;
			this.btn_ExtraUp.UseBorder = true;
			this.btn_ExtraUp.UseClickedEmphasizeTextColor = false;
			this.btn_ExtraUp.UseCustomizeClickedColor = false;
			this.btn_ExtraUp.UseEdge = true;
			this.btn_ExtraUp.UseHoverEmphasizeCustomColor = false;
			this.btn_ExtraUp.UseImage = true;
			this.btn_ExtraUp.UserHoverEmpahsize = false;
			this.btn_ExtraUp.UseSubFont = false;
			this.btn_ExtraUp.MouseDown += new System.Windows.Forms.MouseEventHandler(this.Down_Mouse);
			this.btn_ExtraUp.MouseUp += new System.Windows.Forms.MouseEventHandler(this.Up_Mouse);
			// 
			// btn_ExtraRight
			// 
			this.btn_ExtraRight.BorderWidth = 3;
			this.btn_ExtraRight.ButtonClicked = false;
			this.btn_ExtraRight.ClickedEmphasizeTextColor = System.Drawing.Color.White;
			this.btn_ExtraRight.CustomClickedGradientFirstColor = System.Drawing.Color.White;
			this.btn_ExtraRight.CustomClickedGradientSecondColor = System.Drawing.Color.White;
			this.btn_ExtraRight.Description = "";
			this.btn_ExtraRight.DisabledColor = System.Drawing.Color.DarkGray;
			this.btn_ExtraRight.EdgeRadius = 5;
			this.btn_ExtraRight.GradientAngle = 70F;
			this.btn_ExtraRight.GradientFirstColor = System.Drawing.Color.White;
			this.btn_ExtraRight.GradientSecondColor = System.Drawing.Color.DarkGray;
			this.btn_ExtraRight.HoverEmphasizeCustomColor = System.Drawing.Color.White;
			this.btn_ExtraRight.ImagePosition = new System.Drawing.Point(30, 8);
			this.btn_ExtraRight.ImageSize = new System.Drawing.Point(60, 30);
			this.btn_ExtraRight.LoadImage = global::FrameOfSystem3.Properties.Resources.right_96px;
			this.btn_ExtraRight.Location = new System.Drawing.Point(122, 243);
			this.btn_ExtraRight.MainFont = new System.Drawing.Font("맑은 고딕", 30F, System.Drawing.FontStyle.Bold);
			this.btn_ExtraRight.MainFontColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(36)))), ((int)(((byte)(0)))));
			this.btn_ExtraRight.Margin = new System.Windows.Forms.Padding(1, 1, 0, 0);
			this.btn_ExtraRight.Name = "btn_ExtraRight";
			this.btn_ExtraRight.Size = new System.Drawing.Size(120, 44);
			this.btn_ExtraRight.SubFont = new System.Drawing.Font("맑은 고딕", 8F, System.Drawing.FontStyle.Bold);
			this.btn_ExtraRight.SubFontColor = System.Drawing.Color.DarkBlue;
			this.btn_ExtraRight.SubText = "MOVE";
			this.btn_ExtraRight.TabIndex = 15;
			this.btn_ExtraRight.TextAlignMain = Sys3Controls.EN_TEXTALIGN.MIDDLE_CENTER;
			this.btn_ExtraRight.TextAlignSub = Sys3Controls.EN_TEXTALIGN.TOP_RIGHT;
			this.btn_ExtraRight.ThemeIndex = 0;
			this.btn_ExtraRight.UseBorder = true;
			this.btn_ExtraRight.UseClickedEmphasizeTextColor = false;
			this.btn_ExtraRight.UseCustomizeClickedColor = false;
			this.btn_ExtraRight.UseEdge = true;
			this.btn_ExtraRight.UseHoverEmphasizeCustomColor = false;
			this.btn_ExtraRight.UseImage = true;
			this.btn_ExtraRight.UserHoverEmpahsize = false;
			this.btn_ExtraRight.UseSubFont = false;
			this.btn_ExtraRight.MouseDown += new System.Windows.Forms.MouseEventHandler(this.Down_Mouse);
			this.btn_ExtraRight.MouseUp += new System.Windows.Forms.MouseEventHandler(this.Up_Mouse);
			// 
			// btn_ExtraLeft
			// 
			this.btn_ExtraLeft.BorderWidth = 3;
			this.btn_ExtraLeft.ButtonClicked = false;
			this.btn_ExtraLeft.ClickedEmphasizeTextColor = System.Drawing.Color.White;
			this.btn_ExtraLeft.CustomClickedGradientFirstColor = System.Drawing.Color.White;
			this.btn_ExtraLeft.CustomClickedGradientSecondColor = System.Drawing.Color.White;
			this.btn_ExtraLeft.Description = "";
			this.btn_ExtraLeft.DisabledColor = System.Drawing.Color.DarkGray;
			this.btn_ExtraLeft.EdgeRadius = 5;
			this.btn_ExtraLeft.GradientAngle = 70F;
			this.btn_ExtraLeft.GradientFirstColor = System.Drawing.Color.White;
			this.btn_ExtraLeft.GradientSecondColor = System.Drawing.Color.DarkGray;
			this.btn_ExtraLeft.HoverEmphasizeCustomColor = System.Drawing.Color.White;
			this.btn_ExtraLeft.ImagePosition = new System.Drawing.Point(30, 8);
			this.btn_ExtraLeft.ImageSize = new System.Drawing.Point(60, 30);
			this.btn_ExtraLeft.LoadImage = global::FrameOfSystem3.Properties.Resources.left_96px;
			this.btn_ExtraLeft.Location = new System.Drawing.Point(0, 243);
			this.btn_ExtraLeft.MainFont = new System.Drawing.Font("맑은 고딕", 30F, System.Drawing.FontStyle.Bold);
			this.btn_ExtraLeft.MainFontColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(36)))), ((int)(((byte)(0)))));
			this.btn_ExtraLeft.Margin = new System.Windows.Forms.Padding(1, 1, 0, 0);
			this.btn_ExtraLeft.Name = "btn_ExtraLeft";
			this.btn_ExtraLeft.Size = new System.Drawing.Size(120, 44);
			this.btn_ExtraLeft.SubFont = new System.Drawing.Font("맑은 고딕", 8F, System.Drawing.FontStyle.Bold);
			this.btn_ExtraLeft.SubFontColor = System.Drawing.Color.DarkBlue;
			this.btn_ExtraLeft.SubText = "MOVE";
			this.btn_ExtraLeft.TabIndex = 15;
			this.btn_ExtraLeft.TextAlignMain = Sys3Controls.EN_TEXTALIGN.MIDDLE_CENTER;
			this.btn_ExtraLeft.TextAlignSub = Sys3Controls.EN_TEXTALIGN.TOP_RIGHT;
			this.btn_ExtraLeft.ThemeIndex = 0;
			this.btn_ExtraLeft.UseBorder = true;
			this.btn_ExtraLeft.UseClickedEmphasizeTextColor = false;
			this.btn_ExtraLeft.UseCustomizeClickedColor = false;
			this.btn_ExtraLeft.UseEdge = true;
			this.btn_ExtraLeft.UseHoverEmphasizeCustomColor = false;
			this.btn_ExtraLeft.UseImage = true;
			this.btn_ExtraLeft.UserHoverEmpahsize = false;
			this.btn_ExtraLeft.UseSubFont = false;
			this.btn_ExtraLeft.MouseDown += new System.Windows.Forms.MouseEventHandler(this.Down_Mouse);
			this.btn_ExtraLeft.MouseUp += new System.Windows.Forms.MouseEventHandler(this.Up_Mouse);
			// 
			// btn_DownLeft
			// 
			this.btn_DownLeft.BorderWidth = 3;
			this.btn_DownLeft.ButtonClicked = false;
			this.btn_DownLeft.ClickedEmphasizeTextColor = System.Drawing.Color.White;
			this.btn_DownLeft.CustomClickedGradientFirstColor = System.Drawing.Color.White;
			this.btn_DownLeft.CustomClickedGradientSecondColor = System.Drawing.Color.White;
			this.btn_DownLeft.Description = "";
			this.btn_DownLeft.DisabledColor = System.Drawing.Color.DarkGray;
			this.btn_DownLeft.EdgeRadius = 5;
			this.btn_DownLeft.GradientAngle = 70F;
			this.btn_DownLeft.GradientFirstColor = System.Drawing.Color.White;
			this.btn_DownLeft.GradientSecondColor = System.Drawing.Color.DarkGray;
			this.btn_DownLeft.HoverEmphasizeCustomColor = System.Drawing.Color.White;
			this.btn_DownLeft.ImagePosition = new System.Drawing.Point(20, 20);
			this.btn_DownLeft.ImageSize = new System.Drawing.Point(40, 40);
			this.btn_DownLeft.LoadImage = global::FrameOfSystem3.Properties.Resources.down_left_104px;
			this.btn_DownLeft.Location = new System.Drawing.Point(0, 162);
			this.btn_DownLeft.MainFont = new System.Drawing.Font("맑은 고딕", 30F, System.Drawing.FontStyle.Bold);
			this.btn_DownLeft.MainFontColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(36)))), ((int)(((byte)(0)))));
			this.btn_DownLeft.Margin = new System.Windows.Forms.Padding(1, 1, 0, 0);
			this.btn_DownLeft.Name = "btn_DownLeft";
			this.btn_DownLeft.Size = new System.Drawing.Size(80, 80);
			this.btn_DownLeft.SubFont = new System.Drawing.Font("맑은 고딕", 8F, System.Drawing.FontStyle.Bold);
			this.btn_DownLeft.SubFontColor = System.Drawing.Color.DarkBlue;
			this.btn_DownLeft.SubText = "MOVE";
			this.btn_DownLeft.TabIndex = 15;
			this.btn_DownLeft.TextAlignMain = Sys3Controls.EN_TEXTALIGN.MIDDLE_CENTER;
			this.btn_DownLeft.TextAlignSub = Sys3Controls.EN_TEXTALIGN.TOP_RIGHT;
			this.btn_DownLeft.ThemeIndex = 0;
			this.btn_DownLeft.UseBorder = true;
			this.btn_DownLeft.UseClickedEmphasizeTextColor = false;
			this.btn_DownLeft.UseCustomizeClickedColor = false;
			this.btn_DownLeft.UseEdge = true;
			this.btn_DownLeft.UseHoverEmphasizeCustomColor = false;
			this.btn_DownLeft.UseImage = true;
			this.btn_DownLeft.UserHoverEmpahsize = false;
			this.btn_DownLeft.UseSubFont = false;
			this.btn_DownLeft.MouseDown += new System.Windows.Forms.MouseEventHandler(this.Down_Mouse);
			this.btn_DownLeft.MouseUp += new System.Windows.Forms.MouseEventHandler(this.Up_Mouse);
			// 
			// btn_Down
			// 
			this.btn_Down.BorderWidth = 3;
			this.btn_Down.ButtonClicked = false;
			this.btn_Down.ClickedEmphasizeTextColor = System.Drawing.Color.White;
			this.btn_Down.CustomClickedGradientFirstColor = System.Drawing.Color.White;
			this.btn_Down.CustomClickedGradientSecondColor = System.Drawing.Color.White;
			this.btn_Down.Description = "";
			this.btn_Down.DisabledColor = System.Drawing.Color.DarkGray;
			this.btn_Down.EdgeRadius = 5;
			this.btn_Down.GradientAngle = 70F;
			this.btn_Down.GradientFirstColor = System.Drawing.Color.White;
			this.btn_Down.GradientSecondColor = System.Drawing.Color.DarkGray;
			this.btn_Down.HoverEmphasizeCustomColor = System.Drawing.Color.White;
			this.btn_Down.ImagePosition = new System.Drawing.Point(20, 20);
			this.btn_Down.ImageSize = new System.Drawing.Point(40, 40);
			this.btn_Down.LoadImage = global::FrameOfSystem3.Properties.Resources.down_104px;
			this.btn_Down.Location = new System.Drawing.Point(81, 162);
			this.btn_Down.MainFont = new System.Drawing.Font("맑은 고딕", 30F, System.Drawing.FontStyle.Bold);
			this.btn_Down.MainFontColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(36)))), ((int)(((byte)(0)))));
			this.btn_Down.Margin = new System.Windows.Forms.Padding(1, 1, 0, 0);
			this.btn_Down.Name = "btn_Down";
			this.btn_Down.Size = new System.Drawing.Size(80, 80);
			this.btn_Down.SubFont = new System.Drawing.Font("맑은 고딕", 8F, System.Drawing.FontStyle.Bold);
			this.btn_Down.SubFontColor = System.Drawing.Color.DarkBlue;
			this.btn_Down.SubText = "MOVE";
			this.btn_Down.TabIndex = 16;
			this.btn_Down.TextAlignMain = Sys3Controls.EN_TEXTALIGN.MIDDLE_CENTER;
			this.btn_Down.TextAlignSub = Sys3Controls.EN_TEXTALIGN.TOP_RIGHT;
			this.btn_Down.ThemeIndex = 0;
			this.btn_Down.UseBorder = true;
			this.btn_Down.UseClickedEmphasizeTextColor = false;
			this.btn_Down.UseCustomizeClickedColor = false;
			this.btn_Down.UseEdge = true;
			this.btn_Down.UseHoverEmphasizeCustomColor = false;
			this.btn_Down.UseImage = true;
			this.btn_Down.UserHoverEmpahsize = false;
			this.btn_Down.UseSubFont = false;
			this.btn_Down.MouseDown += new System.Windows.Forms.MouseEventHandler(this.Down_Mouse);
			this.btn_Down.MouseUp += new System.Windows.Forms.MouseEventHandler(this.Up_Mouse);
			// 
			// btn_DownRight
			// 
			this.btn_DownRight.BorderWidth = 3;
			this.btn_DownRight.ButtonClicked = false;
			this.btn_DownRight.ClickedEmphasizeTextColor = System.Drawing.Color.White;
			this.btn_DownRight.CustomClickedGradientFirstColor = System.Drawing.Color.White;
			this.btn_DownRight.CustomClickedGradientSecondColor = System.Drawing.Color.White;
			this.btn_DownRight.Description = "";
			this.btn_DownRight.DisabledColor = System.Drawing.Color.DarkGray;
			this.btn_DownRight.EdgeRadius = 5;
			this.btn_DownRight.GradientAngle = 70F;
			this.btn_DownRight.GradientFirstColor = System.Drawing.Color.White;
			this.btn_DownRight.GradientSecondColor = System.Drawing.Color.DarkGray;
			this.btn_DownRight.HoverEmphasizeCustomColor = System.Drawing.Color.White;
			this.btn_DownRight.ImagePosition = new System.Drawing.Point(20, 20);
			this.btn_DownRight.ImageSize = new System.Drawing.Point(40, 40);
			this.btn_DownRight.LoadImage = global::FrameOfSystem3.Properties.Resources.down_right_104px;
			this.btn_DownRight.Location = new System.Drawing.Point(162, 162);
			this.btn_DownRight.MainFont = new System.Drawing.Font("맑은 고딕", 30F, System.Drawing.FontStyle.Bold);
			this.btn_DownRight.MainFontColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(36)))), ((int)(((byte)(0)))));
			this.btn_DownRight.Margin = new System.Windows.Forms.Padding(1, 1, 0, 0);
			this.btn_DownRight.Name = "btn_DownRight";
			this.btn_DownRight.Size = new System.Drawing.Size(80, 80);
			this.btn_DownRight.SubFont = new System.Drawing.Font("맑은 고딕", 8F, System.Drawing.FontStyle.Bold);
			this.btn_DownRight.SubFontColor = System.Drawing.Color.DarkBlue;
			this.btn_DownRight.SubText = "MOVE";
			this.btn_DownRight.TabIndex = 17;
			this.btn_DownRight.TextAlignMain = Sys3Controls.EN_TEXTALIGN.MIDDLE_CENTER;
			this.btn_DownRight.TextAlignSub = Sys3Controls.EN_TEXTALIGN.TOP_RIGHT;
			this.btn_DownRight.ThemeIndex = 0;
			this.btn_DownRight.UseBorder = true;
			this.btn_DownRight.UseClickedEmphasizeTextColor = false;
			this.btn_DownRight.UseCustomizeClickedColor = false;
			this.btn_DownRight.UseEdge = true;
			this.btn_DownRight.UseHoverEmphasizeCustomColor = false;
			this.btn_DownRight.UseImage = true;
			this.btn_DownRight.UserHoverEmpahsize = false;
			this.btn_DownRight.UseSubFont = false;
			this.btn_DownRight.MouseDown += new System.Windows.Forms.MouseEventHandler(this.Down_Mouse);
			this.btn_DownRight.MouseUp += new System.Windows.Forms.MouseEventHandler(this.Up_Mouse);
			// 
			// JogPanel_DpadExtend
			// 
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
			this.Controls.Add(this.btn_Stop);
			this.Controls.Add(this.btn_Right);
			this.Controls.Add(this.btn_UpRight);
			this.Controls.Add(this.btn_Up);
			this.Controls.Add(this.btn_UpLeft);
			this.Controls.Add(this.btn_Left);
			this.Controls.Add(this.btn_ExtraDown);
			this.Controls.Add(this.btn_ExtraUp);
			this.Controls.Add(this.btn_ExtraRight);
			this.Controls.Add(this.btn_ExtraLeft);
			this.Controls.Add(this.btn_DownLeft);
			this.Controls.Add(this.btn_Down);
			this.Controls.Add(this.btn_DownRight);
			this.Margin = new System.Windows.Forms.Padding(1, 1, 0, 0);
			this.Name = "JogPanel_DpadExtend";
			this.Padding = new System.Windows.Forms.Padding(1);
			this.Size = new System.Drawing.Size(287, 287);
			this.Tag = "";
			this.ResumeLayout(false);

		}

		#endregion

		private Sys3Controls.Sys3button btn_Stop;
		private Sys3Controls.Sys3button btn_Right;
		private Sys3Controls.Sys3button btn_UpRight;
		private Sys3Controls.Sys3button btn_Up;
		private Sys3Controls.Sys3button btn_UpLeft;
		private Sys3Controls.Sys3button btn_Left;
		private Sys3Controls.Sys3button btn_DownLeft;
		private Sys3Controls.Sys3button btn_Down;
		private Sys3Controls.Sys3button btn_DownRight;
		private Sys3Controls.Sys3button btn_ExtraLeft;
		private Sys3Controls.Sys3button btn_ExtraRight;
		private Sys3Controls.Sys3button btn_ExtraUp;
		private Sys3Controls.Sys3button btn_ExtraDown;




	}
}
