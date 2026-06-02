namespace FrameOfSystem3.Views.Functional.TimeSelector
{
	partial class TimePanel_Dial
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
			this.components = new System.ComponentModel.Container();
			this.lbl_Type = new Sys3Controls.Sys3Label();
			this.btn_Up = new Sys3Controls.Sys3button();
			this.btn_Down = new Sys3Controls.Sys3button();
			this.lbl_Value = new Sys3Controls.Sys3Label();
			this.timerClicked = new System.Windows.Forms.Timer(this.components);
			this.SuspendLayout();
			// 
			// lbl_Type
			// 
			this.lbl_Type.BackGroundColor = System.Drawing.Color.DarkGray;
			this.lbl_Type.BorderStroke = 2;
			this.lbl_Type.BorderStyle = System.Windows.Forms.ButtonBorderStyle.Solid;
			this.lbl_Type.Description = "";
			this.lbl_Type.DisabledColor = System.Drawing.Color.DarkGray;
			this.lbl_Type.EdgeRadius = 1;
			this.lbl_Type.ImagePosition = new System.Drawing.Point(0, 0);
			this.lbl_Type.ImageSize = new System.Drawing.Point(0, 0);
			this.lbl_Type.LoadImage = null;
			this.lbl_Type.Location = new System.Drawing.Point(3, 3);
			this.lbl_Type.MainFont = new System.Drawing.Font("맑은 고딕", 15F, System.Drawing.FontStyle.Bold);
			this.lbl_Type.MainFontColor = System.Drawing.Color.Black;
			this.lbl_Type.Margin = new System.Windows.Forms.Padding(1, 1, 0, 0);
			this.lbl_Type.Name = "lbl_Type";
			this.lbl_Type.Size = new System.Drawing.Size(65, 31);
			this.lbl_Type.SubFont = new System.Drawing.Font("맑은 고딕", 10F, System.Drawing.FontStyle.Bold);
			this.lbl_Type.SubFontColor = System.Drawing.Color.DarkRed;
			this.lbl_Type.SubText = "";
			this.lbl_Type.TabIndex = 21105;
			this.lbl_Type.Tag = "";
			this.lbl_Type.Text = "MTH";
			this.lbl_Type.TextAlignMain = Sys3Controls.EN_TEXTALIGN.MIDDLE_CENTER;
			this.lbl_Type.TextAlignSub = Sys3Controls.EN_TEXTALIGN.TOP_LEFT;
			this.lbl_Type.TextAlignUnit = Sys3Controls.EN_TEXTALIGN.MIDDLE_CENTER;
			this.lbl_Type.ThemeIndex = 0;
			this.lbl_Type.UnitAreaRate = 40;
			this.lbl_Type.UnitFont = new System.Drawing.Font("맑은 고딕", 10F, System.Drawing.FontStyle.Bold);
			this.lbl_Type.UnitFontColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
			this.lbl_Type.UnitPositionVertical = false;
			this.lbl_Type.UnitText = "";
			this.lbl_Type.UseBorder = true;
			this.lbl_Type.UseEdgeRadius = false;
			this.lbl_Type.UseImage = false;
			this.lbl_Type.UseSubFont = true;
			this.lbl_Type.UseUnitFont = false;
			// 
			// btn_Up
			// 
			this.btn_Up.BorderWidth = 2;
			this.btn_Up.ButtonClicked = false;
			this.btn_Up.ClickedEmphasizeTextColor = System.Drawing.Color.White;
			this.btn_Up.CustomClickedGradientFirstColor = System.Drawing.Color.White;
			this.btn_Up.CustomClickedGradientSecondColor = System.Drawing.Color.White;
			this.btn_Up.Description = "";
			this.btn_Up.DisabledColor = System.Drawing.Color.DarkGray;
			this.btn_Up.EdgeRadius = 1;
			this.btn_Up.GradientAngle = 70F;
			this.btn_Up.GradientFirstColor = System.Drawing.Color.WhiteSmoke;
			this.btn_Up.GradientSecondColor = System.Drawing.Color.Gray;
			this.btn_Up.HoverEmphasizeCustomColor = System.Drawing.Color.White;
			this.btn_Up.ImagePosition = new System.Drawing.Point(7, 7);
			this.btn_Up.ImageSize = new System.Drawing.Point(30, 30);
			this.btn_Up.LoadImage = global::FrameOfSystem3.Properties.Resources.CONFIG_ADD3;
			this.btn_Up.Location = new System.Drawing.Point(3, 44);
			this.btn_Up.MainFont = new System.Drawing.Font("맑은 고딕", 15F, System.Drawing.FontStyle.Bold);
			this.btn_Up.MainFontColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(36)))), ((int)(((byte)(0)))));
			this.btn_Up.Margin = new System.Windows.Forms.Padding(1, 1, 0, 0);
			this.btn_Up.Name = "btn_Up";
			this.btn_Up.Size = new System.Drawing.Size(65, 40);
			this.btn_Up.SubFont = new System.Drawing.Font("맑은 고딕", 8F, System.Drawing.FontStyle.Bold);
			this.btn_Up.SubFontColor = System.Drawing.Color.DarkBlue;
			this.btn_Up.SubText = "STATUS";
			this.btn_Up.TabIndex = 21103;
			this.btn_Up.Text = "▲";
			this.btn_Up.TextAlignMain = Sys3Controls.EN_TEXTALIGN.MIDDLE_CENTER;
			this.btn_Up.TextAlignSub = Sys3Controls.EN_TEXTALIGN.TOP_LEFT;
			this.btn_Up.ThemeIndex = 0;
			this.btn_Up.UseBorder = true;
			this.btn_Up.UseClickedEmphasizeTextColor = false;
			this.btn_Up.UseCustomizeClickedColor = false;
			this.btn_Up.UseEdge = true;
			this.btn_Up.UseHoverEmphasizeCustomColor = false;
			this.btn_Up.UseImage = false;
			this.btn_Up.UserHoverEmpahsize = false;
			this.btn_Up.UseSubFont = false;
			this.btn_Up.MouseDown += new System.Windows.Forms.MouseEventHandler(this.btn_MouseDown);
			this.btn_Up.MouseUp += new System.Windows.Forms.MouseEventHandler(this.btn_MouseUp);
			// 
			// btn_Down
			// 
			this.btn_Down.BorderWidth = 2;
			this.btn_Down.ButtonClicked = false;
			this.btn_Down.ClickedEmphasizeTextColor = System.Drawing.Color.White;
			this.btn_Down.CustomClickedGradientFirstColor = System.Drawing.Color.White;
			this.btn_Down.CustomClickedGradientSecondColor = System.Drawing.Color.White;
			this.btn_Down.Description = "";
			this.btn_Down.DisabledColor = System.Drawing.Color.DarkGray;
			this.btn_Down.EdgeRadius = 1;
			this.btn_Down.GradientAngle = 70F;
			this.btn_Down.GradientFirstColor = System.Drawing.Color.WhiteSmoke;
			this.btn_Down.GradientSecondColor = System.Drawing.Color.Gray;
			this.btn_Down.HoverEmphasizeCustomColor = System.Drawing.Color.White;
			this.btn_Down.ImagePosition = new System.Drawing.Point(7, 7);
			this.btn_Down.ImageSize = new System.Drawing.Point(30, 30);
			this.btn_Down.LoadImage = global::FrameOfSystem3.Properties.Resources.CONFIG_ADD3;
			this.btn_Down.Location = new System.Drawing.Point(3, 138);
			this.btn_Down.MainFont = new System.Drawing.Font("맑은 고딕", 15F, System.Drawing.FontStyle.Bold);
			this.btn_Down.MainFontColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(36)))), ((int)(((byte)(0)))));
			this.btn_Down.Margin = new System.Windows.Forms.Padding(1, 1, 0, 0);
			this.btn_Down.Name = "btn_Down";
			this.btn_Down.Size = new System.Drawing.Size(65, 40);
			this.btn_Down.SubFont = new System.Drawing.Font("맑은 고딕", 8F, System.Drawing.FontStyle.Bold);
			this.btn_Down.SubFontColor = System.Drawing.Color.DarkBlue;
			this.btn_Down.SubText = "STATUS";
			this.btn_Down.TabIndex = 21102;
			this.btn_Down.Text = "▼";
			this.btn_Down.TextAlignMain = Sys3Controls.EN_TEXTALIGN.MIDDLE_CENTER;
			this.btn_Down.TextAlignSub = Sys3Controls.EN_TEXTALIGN.TOP_LEFT;
			this.btn_Down.ThemeIndex = 0;
			this.btn_Down.UseBorder = true;
			this.btn_Down.UseClickedEmphasizeTextColor = false;
			this.btn_Down.UseCustomizeClickedColor = false;
			this.btn_Down.UseEdge = true;
			this.btn_Down.UseHoverEmphasizeCustomColor = false;
			this.btn_Down.UseImage = false;
			this.btn_Down.UserHoverEmpahsize = false;
			this.btn_Down.UseSubFont = false;
			this.btn_Down.MouseDown += new System.Windows.Forms.MouseEventHandler(this.btn_MouseDown);
			this.btn_Down.MouseUp += new System.Windows.Forms.MouseEventHandler(this.btn_MouseUp);
			// 
			// lbl_Value
			// 
			this.lbl_Value.BackGroundColor = System.Drawing.Color.WhiteSmoke;
			this.lbl_Value.BorderStroke = 2;
			this.lbl_Value.BorderStyle = System.Windows.Forms.ButtonBorderStyle.Solid;
			this.lbl_Value.Description = "";
			this.lbl_Value.DisabledColor = System.Drawing.Color.DarkGray;
			this.lbl_Value.EdgeRadius = 1;
			this.lbl_Value.ImagePosition = new System.Drawing.Point(0, 0);
			this.lbl_Value.ImageSize = new System.Drawing.Point(0, 0);
			this.lbl_Value.LoadImage = null;
			this.lbl_Value.Location = new System.Drawing.Point(4, 85);
			this.lbl_Value.MainFont = new System.Drawing.Font("맑은 고딕", 20F, System.Drawing.FontStyle.Bold);
			this.lbl_Value.MainFontColor = System.Drawing.Color.Black;
			this.lbl_Value.Margin = new System.Windows.Forms.Padding(1, 1, 0, 0);
			this.lbl_Value.Name = "lbl_Value";
			this.lbl_Value.Size = new System.Drawing.Size(63, 52);
			this.lbl_Value.SubFont = new System.Drawing.Font("맑은 고딕", 8F, System.Drawing.FontStyle.Bold);
			this.lbl_Value.SubFontColor = System.Drawing.Color.Black;
			this.lbl_Value.SubText = "";
			this.lbl_Value.TabIndex = 21106;
			this.lbl_Value.Text = "0";
			this.lbl_Value.TextAlignMain = Sys3Controls.EN_TEXTALIGN.MIDDLE_CENTER;
			this.lbl_Value.TextAlignSub = Sys3Controls.EN_TEXTALIGN.MIDDLE_RIGHT;
			this.lbl_Value.TextAlignUnit = Sys3Controls.EN_TEXTALIGN.MIDDLE_RIGHT;
			this.lbl_Value.ThemeIndex = 0;
			this.lbl_Value.UnitAreaRate = 20;
			this.lbl_Value.UnitFont = new System.Drawing.Font("맑은 고딕", 10F, System.Drawing.FontStyle.Bold);
			this.lbl_Value.UnitFontColor = System.Drawing.Color.Black;
			this.lbl_Value.UnitPositionVertical = false;
			this.lbl_Value.UnitText = "No.";
			this.lbl_Value.UseBorder = true;
			this.lbl_Value.UseEdgeRadius = false;
			this.lbl_Value.UseImage = false;
			this.lbl_Value.UseSubFont = false;
			this.lbl_Value.UseUnitFont = false;
			this.lbl_Value.Click += new System.EventHandler(this.Click_Value);
			this.lbl_Value.MouseDown += new System.Windows.Forms.MouseEventHandler(this.Click_DragModeStart);
			this.lbl_Value.MouseUp += new System.Windows.Forms.MouseEventHandler(this.Click_DragModeEnd);
			// 
			// TimePanel_Dial
			// 
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
			this.Controls.Add(this.lbl_Value);
			this.Controls.Add(this.lbl_Type);
			this.Controls.Add(this.btn_Up);
			this.Controls.Add(this.btn_Down);
			this.Margin = new System.Windows.Forms.Padding(1, 1, 0, 0);
			this.Name = "TimePanel_Dial";
			this.Padding = new System.Windows.Forms.Padding(2);
			this.Size = new System.Drawing.Size(71, 180);
			this.Tag = "";
			this.ResumeLayout(false);

		}

		#endregion

		private Sys3Controls.Sys3Label lbl_Type;
		private Sys3Controls.Sys3button btn_Up;
		private Sys3Controls.Sys3button btn_Down;
		private Sys3Controls.Sys3Label lbl_Value;
		private System.Windows.Forms.Timer timerClicked;
	}
}
