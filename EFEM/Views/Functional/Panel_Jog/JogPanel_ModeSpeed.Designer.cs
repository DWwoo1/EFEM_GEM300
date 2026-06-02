namespace FrameOfSystem3.Views.Functional.Jog
{
	partial class JogPanel_ModeSpeed
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
			this.pb_speed = new Sys3Controls.Sys3ProgressBar();
			this.btn_Define_100 = new Sys3Controls.Sys3button();
			this.btn_Define_80 = new Sys3Controls.Sys3button();
			this.btn_Define_60 = new Sys3Controls.Sys3button();
			this.btn_Define_40 = new Sys3Controls.Sys3button();
			this.btn_Define_20 = new Sys3Controls.Sys3button();
			this.SuspendLayout();
			// 
			// pb_speed
			// 
			this.pb_speed.BackGroundColor = System.Drawing.Color.White;
			this.pb_speed.BorderStroke = 1;
			this.pb_speed.FirstColor = System.Drawing.Color.LightGray;
			this.pb_speed.GageCount = 10;
			this.pb_speed.LabelWidth = 50;
			this.pb_speed.Location = new System.Drawing.Point(0, 0);
			this.pb_speed.Margin = new System.Windows.Forms.Padding(1, 1, 0, 0);
			this.pb_speed.MaxTickCount = ((uint)(100u));
			this.pb_speed.MinTickCount = ((uint)(1u));
			this.pb_speed.Name = "pb_speed";
			this.pb_speed.NormalGageColor = System.Drawing.Color.LightGray;
			this.pb_speed.OffSet = 2;
			this.pb_speed.SecondColor = System.Drawing.Color.DodgerBlue;
			this.pb_speed.Size = new System.Drawing.Size(282, 36);
			this.pb_speed.TabIndex = 1416;
			this.pb_speed.Tick = ((uint)(100u));
			this.pb_speed.TickFont = new System.Drawing.Font("맑은 고딕", 14F, System.Drawing.FontStyle.Bold);
			this.pb_speed.TickFontColor = System.Drawing.Color.Black;
			this.pb_speed.UseBorder = true;
			this.pb_speed.UseControlBorder = true;
			this.pb_speed.UseLinearGradientMode = true;
			this.pb_speed.Click += new System.EventHandler(this.Click_ProgressBar);
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
			this.btn_Define_100.Location = new System.Drawing.Point(228, 37);
			this.btn_Define_100.MainFont = new System.Drawing.Font("맑은 고딕", 11F, System.Drawing.FontStyle.Bold);
			this.btn_Define_100.MainFontColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(36)))), ((int)(((byte)(0)))));
			this.btn_Define_100.Margin = new System.Windows.Forms.Padding(1, 1, 0, 0);
			this.btn_Define_100.Name = "btn_Define_100";
			this.btn_Define_100.Size = new System.Drawing.Size(56, 33);
			this.btn_Define_100.SubFont = new System.Drawing.Font("맑은 고딕", 9F, System.Drawing.FontStyle.Bold);
			this.btn_Define_100.SubFontColor = System.Drawing.Color.DarkBlue;
			this.btn_Define_100.SubText = "";
			this.btn_Define_100.TabIndex = 1417;
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
			this.btn_Define_100.Click += new System.EventHandler(this.Click_DefinedSpeed);
			// 
			// btn_Define_80
			// 
			this.btn_Define_80.BorderWidth = 1;
			this.btn_Define_80.ButtonClicked = false;
			this.btn_Define_80.ClickedEmphasizeTextColor = System.Drawing.Color.White;
			this.btn_Define_80.CustomClickedGradientFirstColor = System.Drawing.Color.White;
			this.btn_Define_80.CustomClickedGradientSecondColor = System.Drawing.Color.White;
			this.btn_Define_80.Description = "";
			this.btn_Define_80.DisabledColor = System.Drawing.Color.DarkGray;
			this.btn_Define_80.EdgeRadius = 5;
			this.btn_Define_80.GradientAngle = 70F;
			this.btn_Define_80.GradientFirstColor = System.Drawing.Color.White;
			this.btn_Define_80.GradientSecondColor = System.Drawing.Color.FromArgb(((int)(((byte)(170)))), ((int)(((byte)(176)))), ((int)(((byte)(183)))));
			this.btn_Define_80.HoverEmphasizeCustomColor = System.Drawing.Color.White;
			this.btn_Define_80.ImagePosition = new System.Drawing.Point(10, 10);
			this.btn_Define_80.ImageSize = new System.Drawing.Point(30, 30);
			this.btn_Define_80.LoadImage = global::FrameOfSystem3.Properties.Resources.CLEAR_BLACK;
			this.btn_Define_80.Location = new System.Drawing.Point(171, 37);
			this.btn_Define_80.MainFont = new System.Drawing.Font("맑은 고딕", 11F, System.Drawing.FontStyle.Bold);
			this.btn_Define_80.MainFontColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(36)))), ((int)(((byte)(0)))));
			this.btn_Define_80.Margin = new System.Windows.Forms.Padding(1, 1, 0, 0);
			this.btn_Define_80.Name = "btn_Define_80";
			this.btn_Define_80.Size = new System.Drawing.Size(56, 33);
			this.btn_Define_80.SubFont = new System.Drawing.Font("맑은 고딕", 9F, System.Drawing.FontStyle.Bold);
			this.btn_Define_80.SubFontColor = System.Drawing.Color.DarkBlue;
			this.btn_Define_80.SubText = "";
			this.btn_Define_80.TabIndex = 1418;
			this.btn_Define_80.Text = "80";
			this.btn_Define_80.TextAlignMain = Sys3Controls.EN_TEXTALIGN.MIDDLE_CENTER;
			this.btn_Define_80.TextAlignSub = Sys3Controls.EN_TEXTALIGN.MIDDLE_LEFT;
			this.btn_Define_80.ThemeIndex = 0;
			this.btn_Define_80.UseBorder = true;
			this.btn_Define_80.UseClickedEmphasizeTextColor = false;
			this.btn_Define_80.UseCustomizeClickedColor = false;
			this.btn_Define_80.UseEdge = true;
			this.btn_Define_80.UseHoverEmphasizeCustomColor = false;
			this.btn_Define_80.UseImage = false;
			this.btn_Define_80.UserHoverEmpahsize = false;
			this.btn_Define_80.UseSubFont = false;
			this.btn_Define_80.Click += new System.EventHandler(this.Click_DefinedSpeed);
			// 
			// btn_Define_60
			// 
			this.btn_Define_60.BorderWidth = 1;
			this.btn_Define_60.ButtonClicked = false;
			this.btn_Define_60.ClickedEmphasizeTextColor = System.Drawing.Color.White;
			this.btn_Define_60.CustomClickedGradientFirstColor = System.Drawing.Color.White;
			this.btn_Define_60.CustomClickedGradientSecondColor = System.Drawing.Color.White;
			this.btn_Define_60.Description = "";
			this.btn_Define_60.DisabledColor = System.Drawing.Color.DarkGray;
			this.btn_Define_60.EdgeRadius = 5;
			this.btn_Define_60.GradientAngle = 70F;
			this.btn_Define_60.GradientFirstColor = System.Drawing.Color.White;
			this.btn_Define_60.GradientSecondColor = System.Drawing.Color.FromArgb(((int)(((byte)(170)))), ((int)(((byte)(176)))), ((int)(((byte)(183)))));
			this.btn_Define_60.HoverEmphasizeCustomColor = System.Drawing.Color.White;
			this.btn_Define_60.ImagePosition = new System.Drawing.Point(10, 10);
			this.btn_Define_60.ImageSize = new System.Drawing.Point(30, 30);
			this.btn_Define_60.LoadImage = global::FrameOfSystem3.Properties.Resources.CLEAR_BLACK;
			this.btn_Define_60.Location = new System.Drawing.Point(114, 37);
			this.btn_Define_60.MainFont = new System.Drawing.Font("맑은 고딕", 11F, System.Drawing.FontStyle.Bold);
			this.btn_Define_60.MainFontColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(36)))), ((int)(((byte)(0)))));
			this.btn_Define_60.Margin = new System.Windows.Forms.Padding(1, 1, 0, 0);
			this.btn_Define_60.Name = "btn_Define_60";
			this.btn_Define_60.Size = new System.Drawing.Size(56, 33);
			this.btn_Define_60.SubFont = new System.Drawing.Font("맑은 고딕", 9F, System.Drawing.FontStyle.Bold);
			this.btn_Define_60.SubFontColor = System.Drawing.Color.DarkBlue;
			this.btn_Define_60.SubText = "";
			this.btn_Define_60.TabIndex = 1419;
			this.btn_Define_60.Text = "60";
			this.btn_Define_60.TextAlignMain = Sys3Controls.EN_TEXTALIGN.MIDDLE_CENTER;
			this.btn_Define_60.TextAlignSub = Sys3Controls.EN_TEXTALIGN.MIDDLE_LEFT;
			this.btn_Define_60.ThemeIndex = 0;
			this.btn_Define_60.UseBorder = true;
			this.btn_Define_60.UseClickedEmphasizeTextColor = false;
			this.btn_Define_60.UseCustomizeClickedColor = false;
			this.btn_Define_60.UseEdge = true;
			this.btn_Define_60.UseHoverEmphasizeCustomColor = false;
			this.btn_Define_60.UseImage = false;
			this.btn_Define_60.UserHoverEmpahsize = false;
			this.btn_Define_60.UseSubFont = false;
			this.btn_Define_60.Click += new System.EventHandler(this.Click_DefinedSpeed);
			// 
			// btn_Define_40
			// 
			this.btn_Define_40.BorderWidth = 1;
			this.btn_Define_40.ButtonClicked = false;
			this.btn_Define_40.ClickedEmphasizeTextColor = System.Drawing.Color.White;
			this.btn_Define_40.CustomClickedGradientFirstColor = System.Drawing.Color.White;
			this.btn_Define_40.CustomClickedGradientSecondColor = System.Drawing.Color.White;
			this.btn_Define_40.Description = "";
			this.btn_Define_40.DisabledColor = System.Drawing.Color.DarkGray;
			this.btn_Define_40.EdgeRadius = 5;
			this.btn_Define_40.GradientAngle = 70F;
			this.btn_Define_40.GradientFirstColor = System.Drawing.Color.White;
			this.btn_Define_40.GradientSecondColor = System.Drawing.Color.FromArgb(((int)(((byte)(170)))), ((int)(((byte)(176)))), ((int)(((byte)(183)))));
			this.btn_Define_40.HoverEmphasizeCustomColor = System.Drawing.Color.White;
			this.btn_Define_40.ImagePosition = new System.Drawing.Point(10, 10);
			this.btn_Define_40.ImageSize = new System.Drawing.Point(30, 30);
			this.btn_Define_40.LoadImage = global::FrameOfSystem3.Properties.Resources.CLEAR_BLACK;
			this.btn_Define_40.Location = new System.Drawing.Point(57, 37);
			this.btn_Define_40.MainFont = new System.Drawing.Font("맑은 고딕", 11F, System.Drawing.FontStyle.Bold);
			this.btn_Define_40.MainFontColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(36)))), ((int)(((byte)(0)))));
			this.btn_Define_40.Margin = new System.Windows.Forms.Padding(1, 1, 0, 0);
			this.btn_Define_40.Name = "btn_Define_40";
			this.btn_Define_40.Size = new System.Drawing.Size(56, 33);
			this.btn_Define_40.SubFont = new System.Drawing.Font("맑은 고딕", 9F, System.Drawing.FontStyle.Bold);
			this.btn_Define_40.SubFontColor = System.Drawing.Color.DarkBlue;
			this.btn_Define_40.SubText = "";
			this.btn_Define_40.TabIndex = 1420;
			this.btn_Define_40.Text = "40";
			this.btn_Define_40.TextAlignMain = Sys3Controls.EN_TEXTALIGN.MIDDLE_CENTER;
			this.btn_Define_40.TextAlignSub = Sys3Controls.EN_TEXTALIGN.MIDDLE_LEFT;
			this.btn_Define_40.ThemeIndex = 0;
			this.btn_Define_40.UseBorder = true;
			this.btn_Define_40.UseClickedEmphasizeTextColor = false;
			this.btn_Define_40.UseCustomizeClickedColor = false;
			this.btn_Define_40.UseEdge = true;
			this.btn_Define_40.UseHoverEmphasizeCustomColor = false;
			this.btn_Define_40.UseImage = false;
			this.btn_Define_40.UserHoverEmpahsize = false;
			this.btn_Define_40.UseSubFont = false;
			this.btn_Define_40.Click += new System.EventHandler(this.Click_DefinedSpeed);
			// 
			// btn_Define_20
			// 
			this.btn_Define_20.BorderWidth = 1;
			this.btn_Define_20.ButtonClicked = false;
			this.btn_Define_20.ClickedEmphasizeTextColor = System.Drawing.Color.White;
			this.btn_Define_20.CustomClickedGradientFirstColor = System.Drawing.Color.White;
			this.btn_Define_20.CustomClickedGradientSecondColor = System.Drawing.Color.White;
			this.btn_Define_20.Description = "";
			this.btn_Define_20.DisabledColor = System.Drawing.Color.DarkGray;
			this.btn_Define_20.EdgeRadius = 5;
			this.btn_Define_20.GradientAngle = 70F;
			this.btn_Define_20.GradientFirstColor = System.Drawing.Color.White;
			this.btn_Define_20.GradientSecondColor = System.Drawing.Color.FromArgb(((int)(((byte)(170)))), ((int)(((byte)(176)))), ((int)(((byte)(183)))));
			this.btn_Define_20.HoverEmphasizeCustomColor = System.Drawing.Color.White;
			this.btn_Define_20.ImagePosition = new System.Drawing.Point(10, 10);
			this.btn_Define_20.ImageSize = new System.Drawing.Point(30, 30);
			this.btn_Define_20.LoadImage = global::FrameOfSystem3.Properties.Resources.CLEAR_BLACK;
			this.btn_Define_20.Location = new System.Drawing.Point(0, 37);
			this.btn_Define_20.MainFont = new System.Drawing.Font("맑은 고딕", 11F, System.Drawing.FontStyle.Bold);
			this.btn_Define_20.MainFontColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(36)))), ((int)(((byte)(0)))));
			this.btn_Define_20.Margin = new System.Windows.Forms.Padding(1, 1, 0, 0);
			this.btn_Define_20.Name = "btn_Define_20";
			this.btn_Define_20.Size = new System.Drawing.Size(56, 33);
			this.btn_Define_20.SubFont = new System.Drawing.Font("맑은 고딕", 9F, System.Drawing.FontStyle.Bold);
			this.btn_Define_20.SubFontColor = System.Drawing.Color.DarkBlue;
			this.btn_Define_20.SubText = "";
			this.btn_Define_20.TabIndex = 1421;
			this.btn_Define_20.Text = "20";
			this.btn_Define_20.TextAlignMain = Sys3Controls.EN_TEXTALIGN.MIDDLE_CENTER;
			this.btn_Define_20.TextAlignSub = Sys3Controls.EN_TEXTALIGN.MIDDLE_LEFT;
			this.btn_Define_20.ThemeIndex = 0;
			this.btn_Define_20.UseBorder = true;
			this.btn_Define_20.UseClickedEmphasizeTextColor = false;
			this.btn_Define_20.UseCustomizeClickedColor = false;
			this.btn_Define_20.UseEdge = true;
			this.btn_Define_20.UseHoverEmphasizeCustomColor = false;
			this.btn_Define_20.UseImage = false;
			this.btn_Define_20.UserHoverEmpahsize = false;
			this.btn_Define_20.UseSubFont = false;
			this.btn_Define_20.Click += new System.EventHandler(this.Click_DefinedSpeed);
			// 
			// JogPanel_ModeSpeed
			// 
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
			this.Controls.Add(this.pb_speed);
			this.Controls.Add(this.btn_Define_100);
			this.Controls.Add(this.btn_Define_80);
			this.Controls.Add(this.btn_Define_60);
			this.Controls.Add(this.btn_Define_40);
			this.Controls.Add(this.btn_Define_20);
			this.Margin = new System.Windows.Forms.Padding(1, 1, 0, 0);
			this.Name = "JogPanel_ModeSpeed";
			this.Padding = new System.Windows.Forms.Padding(1);
			this.Size = new System.Drawing.Size(287, 70);
			this.Tag = "";
			this.ResumeLayout(false);

		}

		#endregion

		private Sys3Controls.Sys3ProgressBar pb_speed;
		private Sys3Controls.Sys3button btn_Define_100;
		private Sys3Controls.Sys3button btn_Define_80;
		private Sys3Controls.Sys3button btn_Define_60;
		private Sys3Controls.Sys3button btn_Define_40;
		private Sys3Controls.Sys3button btn_Define_20;





	}
}
