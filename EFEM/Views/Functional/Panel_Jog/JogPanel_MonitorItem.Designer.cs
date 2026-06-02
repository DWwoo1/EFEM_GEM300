namespace FrameOfSystem3.Views.Functional.Jog
{
	partial class JogPanel_MonitorItem
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
			this.m_lblPositionAxis = new Sys3Controls.Sys3Label();
			this.lbl_ParameterValue = new Sys3Controls.Sys3Label();
			this.btn_Measure = new Sys3Controls.Sys3button();
			this.btn_Go = new Sys3Controls.Sys3button();
			this.btn_Set = new Sys3Controls.Sys3button();
			this.lbl_AxisName = new Sys3Controls.Sys3Label();
			this.lbl_Actual = new Sys3Controls.Sys3Label();
			this.lbl_Command = new Sys3Controls.Sys3Label();
			this.lbl_ParameterName = new Sys3Controls.Sys3Label();
			this.sys3Label6 = new Sys3Controls.Sys3Label();
			this.sys3Label4 = new Sys3Controls.Sys3Label();
			this.panel_StackItem = new System.Windows.Forms.Panel();
			this.panel_StackItem.SuspendLayout();
			this.SuspendLayout();
			// 
			// m_lblPositionAxis
			// 
			this.m_lblPositionAxis.BackGroundColor = System.Drawing.Color.Silver;
			this.m_lblPositionAxis.BorderStroke = 2;
			this.m_lblPositionAxis.BorderStyle = System.Windows.Forms.ButtonBorderStyle.Solid;
			this.m_lblPositionAxis.Description = "";
			this.m_lblPositionAxis.DisabledColor = System.Drawing.Color.DarkGray;
			this.m_lblPositionAxis.EdgeRadius = 1;
			this.m_lblPositionAxis.ImagePosition = new System.Drawing.Point(0, 0);
			this.m_lblPositionAxis.ImageSize = new System.Drawing.Point(0, 0);
			this.m_lblPositionAxis.LoadImage = null;
			this.m_lblPositionAxis.Location = new System.Drawing.Point(1, 1);
			this.m_lblPositionAxis.MainFont = new System.Drawing.Font("맑은 고딕", 9F, System.Drawing.FontStyle.Bold);
			this.m_lblPositionAxis.MainFontColor = System.Drawing.Color.Black;
			this.m_lblPositionAxis.Margin = new System.Windows.Forms.Padding(1, 1, 0, 0);
			this.m_lblPositionAxis.Name = "m_lblPositionAxis";
			this.m_lblPositionAxis.Size = new System.Drawing.Size(307, 20);
			this.m_lblPositionAxis.SubFont = new System.Drawing.Font("맑은 고딕", 10F);
			this.m_lblPositionAxis.SubFontColor = System.Drawing.Color.Black;
			this.m_lblPositionAxis.SubText = "";
			this.m_lblPositionAxis.TabIndex = 1430;
			this.m_lblPositionAxis.Text = "AXIS NAME";
			this.m_lblPositionAxis.TextAlignMain = Sys3Controls.EN_TEXTALIGN.MIDDLE_CENTER;
			this.m_lblPositionAxis.TextAlignSub = Sys3Controls.EN_TEXTALIGN.MIDDLE_CENTER;
			this.m_lblPositionAxis.TextAlignUnit = Sys3Controls.EN_TEXTALIGN.MIDDLE_CENTER;
			this.m_lblPositionAxis.ThemeIndex = 0;
			this.m_lblPositionAxis.UnitAreaRate = 40;
			this.m_lblPositionAxis.UnitFont = new System.Drawing.Font("맑은 고딕", 10F, System.Drawing.FontStyle.Bold);
			this.m_lblPositionAxis.UnitFontColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
			this.m_lblPositionAxis.UnitPositionVertical = false;
			this.m_lblPositionAxis.UnitText = "";
			this.m_lblPositionAxis.UseBorder = true;
			this.m_lblPositionAxis.UseEdgeRadius = false;
			this.m_lblPositionAxis.UseImage = false;
			this.m_lblPositionAxis.UseSubFont = false;
			this.m_lblPositionAxis.UseUnitFont = false;
			// 
			// lbl_ParameterValue
			// 
			this.lbl_ParameterValue.BackGroundColor = System.Drawing.Color.White;
			this.lbl_ParameterValue.BorderStroke = 2;
			this.lbl_ParameterValue.BorderStyle = System.Windows.Forms.ButtonBorderStyle.Solid;
			this.lbl_ParameterValue.Description = "";
			this.lbl_ParameterValue.DisabledColor = System.Drawing.Color.LightGray;
			this.lbl_ParameterValue.EdgeRadius = 1;
			this.lbl_ParameterValue.ImagePosition = new System.Drawing.Point(0, 0);
			this.lbl_ParameterValue.ImageSize = new System.Drawing.Point(0, 0);
			this.lbl_ParameterValue.LoadImage = null;
			this.lbl_ParameterValue.Location = new System.Drawing.Point(316, 22);
			this.lbl_ParameterValue.MainFont = new System.Drawing.Font("맑은 고딕", 11F, System.Drawing.FontStyle.Bold);
			this.lbl_ParameterValue.MainFontColor = System.Drawing.Color.Black;
			this.lbl_ParameterValue.Margin = new System.Windows.Forms.Padding(1, 1, 0, 0);
			this.lbl_ParameterValue.Name = "lbl_ParameterValue";
			this.lbl_ParameterValue.Size = new System.Drawing.Size(195, 27);
			this.lbl_ParameterValue.SubFont = new System.Drawing.Font("맑은 고딕", 10F);
			this.lbl_ParameterValue.SubFontColor = System.Drawing.Color.Black;
			this.lbl_ParameterValue.SubText = "";
			this.lbl_ParameterValue.TabIndex = 1451;
			this.lbl_ParameterValue.Text = "--";
			this.lbl_ParameterValue.TextAlignMain = Sys3Controls.EN_TEXTALIGN.BOTTOM_RIGHT;
			this.lbl_ParameterValue.TextAlignSub = Sys3Controls.EN_TEXTALIGN.BOTTOM_RIGHT;
			this.lbl_ParameterValue.TextAlignUnit = Sys3Controls.EN_TEXTALIGN.BOTTOM_RIGHT;
			this.lbl_ParameterValue.ThemeIndex = 0;
			this.lbl_ParameterValue.UnitAreaRate = 25;
			this.lbl_ParameterValue.UnitFont = new System.Drawing.Font("맑은 고딕", 10F, System.Drawing.FontStyle.Bold);
			this.lbl_ParameterValue.UnitFontColor = System.Drawing.Color.Black;
			this.lbl_ParameterValue.UnitPositionVertical = false;
			this.lbl_ParameterValue.UnitText = "mm";
			this.lbl_ParameterValue.UseBorder = true;
			this.lbl_ParameterValue.UseEdgeRadius = false;
			this.lbl_ParameterValue.UseImage = false;
			this.lbl_ParameterValue.UseSubFont = false;
			this.lbl_ParameterValue.UseUnitFont = true;
			this.lbl_ParameterValue.Click += new System.EventHandler(this.Click_ParameterValue);
			// 
			// btn_Measure
			// 
			this.btn_Measure.BorderWidth = 3;
			this.btn_Measure.ButtonClicked = false;
			this.btn_Measure.ClickedEmphasizeTextColor = System.Drawing.Color.White;
			this.btn_Measure.CustomClickedGradientFirstColor = System.Drawing.Color.White;
			this.btn_Measure.CustomClickedGradientSecondColor = System.Drawing.Color.White;
			this.btn_Measure.Description = "";
			this.btn_Measure.DisabledColor = System.Drawing.Color.DarkGray;
			this.btn_Measure.EdgeRadius = 5;
			this.btn_Measure.GradientAngle = 70F;
			this.btn_Measure.GradientFirstColor = System.Drawing.Color.White;
			this.btn_Measure.GradientSecondColor = System.Drawing.Color.DarkGray;
			this.btn_Measure.HoverEmphasizeCustomColor = System.Drawing.Color.White;
			this.btn_Measure.ImagePosition = new System.Drawing.Point(10, 10);
			this.btn_Measure.ImageSize = new System.Drawing.Point(34, 30);
			this.btn_Measure.LoadImage = global::FrameOfSystem3.Properties.Resources.length_52px;
			this.btn_Measure.Location = new System.Drawing.Point(253, 50);
			this.btn_Measure.MainFont = new System.Drawing.Font("맑은 고딕", 11F, System.Drawing.FontStyle.Bold);
			this.btn_Measure.MainFontColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(36)))), ((int)(((byte)(0)))));
			this.btn_Measure.Margin = new System.Windows.Forms.Padding(1, 1, 0, 0);
			this.btn_Measure.Name = "btn_Measure";
			this.btn_Measure.Size = new System.Drawing.Size(55, 48);
			this.btn_Measure.SubFont = new System.Drawing.Font("맑은 고딕", 9F, System.Drawing.FontStyle.Bold);
			this.btn_Measure.SubFontColor = System.Drawing.Color.DarkBlue;
			this.btn_Measure.SubText = "";
			this.btn_Measure.TabIndex = 1449;
			this.btn_Measure.TextAlignMain = Sys3Controls.EN_TEXTALIGN.MIDDLE_CENTER;
			this.btn_Measure.TextAlignSub = Sys3Controls.EN_TEXTALIGN.MIDDLE_LEFT;
			this.btn_Measure.ThemeIndex = 0;
			this.btn_Measure.UseBorder = true;
			this.btn_Measure.UseClickedEmphasizeTextColor = false;
			this.btn_Measure.UseCustomizeClickedColor = false;
			this.btn_Measure.UseEdge = true;
			this.btn_Measure.UseHoverEmphasizeCustomColor = false;
			this.btn_Measure.UseImage = true;
			this.btn_Measure.UserHoverEmpahsize = false;
			this.btn_Measure.UseSubFont = false;
			this.btn_Measure.Click += new System.EventHandler(this.Click_Measure);
			// 
			// btn_Go
			// 
			this.btn_Go.BorderWidth = 3;
			this.btn_Go.ButtonClicked = false;
			this.btn_Go.ClickedEmphasizeTextColor = System.Drawing.Color.White;
			this.btn_Go.CustomClickedGradientFirstColor = System.Drawing.Color.White;
			this.btn_Go.CustomClickedGradientSecondColor = System.Drawing.Color.White;
			this.btn_Go.Description = "";
			this.btn_Go.DisabledColor = System.Drawing.Color.DarkGray;
			this.btn_Go.EdgeRadius = 5;
			this.btn_Go.GradientAngle = 70F;
			this.btn_Go.GradientFirstColor = System.Drawing.Color.White;
			this.btn_Go.GradientSecondColor = System.Drawing.Color.DarkGray;
			this.btn_Go.HoverEmphasizeCustomColor = System.Drawing.Color.White;
			this.btn_Go.ImagePosition = new System.Drawing.Point(10, 10);
			this.btn_Go.ImageSize = new System.Drawing.Point(30, 30);
			this.btn_Go.LoadImage = global::FrameOfSystem3.Properties.Resources.CLEAR_BLACK;
			this.btn_Go.Location = new System.Drawing.Point(456, 50);
			this.btn_Go.MainFont = new System.Drawing.Font("맑은 고딕", 11F, System.Drawing.FontStyle.Bold);
			this.btn_Go.MainFontColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(36)))), ((int)(((byte)(0)))));
			this.btn_Go.Margin = new System.Windows.Forms.Padding(1, 1, 0, 0);
			this.btn_Go.Name = "btn_Go";
			this.btn_Go.Size = new System.Drawing.Size(55, 48);
			this.btn_Go.SubFont = new System.Drawing.Font("맑은 고딕", 9F, System.Drawing.FontStyle.Bold);
			this.btn_Go.SubFontColor = System.Drawing.Color.DarkBlue;
			this.btn_Go.SubText = "";
			this.btn_Go.TabIndex = 1448;
			this.btn_Go.Text = "GO";
			this.btn_Go.TextAlignMain = Sys3Controls.EN_TEXTALIGN.MIDDLE_RIGHT;
			this.btn_Go.TextAlignSub = Sys3Controls.EN_TEXTALIGN.MIDDLE_LEFT;
			this.btn_Go.ThemeIndex = 0;
			this.btn_Go.UseBorder = true;
			this.btn_Go.UseClickedEmphasizeTextColor = false;
			this.btn_Go.UseCustomizeClickedColor = false;
			this.btn_Go.UseEdge = true;
			this.btn_Go.UseHoverEmphasizeCustomColor = false;
			this.btn_Go.UseImage = false;
			this.btn_Go.UserHoverEmpahsize = false;
			this.btn_Go.UseSubFont = false;
			this.btn_Go.Click += new System.EventHandler(this.Click_Go);
			// 
			// btn_Set
			// 
			this.btn_Set.BorderWidth = 3;
			this.btn_Set.ButtonClicked = false;
			this.btn_Set.ClickedEmphasizeTextColor = System.Drawing.Color.White;
			this.btn_Set.CustomClickedGradientFirstColor = System.Drawing.Color.White;
			this.btn_Set.CustomClickedGradientSecondColor = System.Drawing.Color.White;
			this.btn_Set.Description = "";
			this.btn_Set.DisabledColor = System.Drawing.Color.DarkGray;
			this.btn_Set.EdgeRadius = 5;
			this.btn_Set.GradientAngle = 70F;
			this.btn_Set.GradientFirstColor = System.Drawing.Color.White;
			this.btn_Set.GradientSecondColor = System.Drawing.Color.DarkGray;
			this.btn_Set.HoverEmphasizeCustomColor = System.Drawing.Color.White;
			this.btn_Set.ImagePosition = new System.Drawing.Point(10, 10);
			this.btn_Set.ImageSize = new System.Drawing.Point(30, 30);
			this.btn_Set.LoadImage = global::FrameOfSystem3.Properties.Resources.CLEAR_BLACK;
			this.btn_Set.Location = new System.Drawing.Point(400, 50);
			this.btn_Set.MainFont = new System.Drawing.Font("맑은 고딕", 11F, System.Drawing.FontStyle.Bold);
			this.btn_Set.MainFontColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(36)))), ((int)(((byte)(0)))));
			this.btn_Set.Margin = new System.Windows.Forms.Padding(1, 1, 0, 0);
			this.btn_Set.Name = "btn_Set";
			this.btn_Set.Size = new System.Drawing.Size(55, 48);
			this.btn_Set.SubFont = new System.Drawing.Font("맑은 고딕", 9F, System.Drawing.FontStyle.Bold);
			this.btn_Set.SubFontColor = System.Drawing.Color.DarkBlue;
			this.btn_Set.SubText = "";
			this.btn_Set.TabIndex = 1444;
			this.btn_Set.Text = "SET";
			this.btn_Set.TextAlignMain = Sys3Controls.EN_TEXTALIGN.MIDDLE_RIGHT;
			this.btn_Set.TextAlignSub = Sys3Controls.EN_TEXTALIGN.MIDDLE_LEFT;
			this.btn_Set.ThemeIndex = 0;
			this.btn_Set.UseBorder = true;
			this.btn_Set.UseClickedEmphasizeTextColor = false;
			this.btn_Set.UseCustomizeClickedColor = false;
			this.btn_Set.UseEdge = true;
			this.btn_Set.UseHoverEmphasizeCustomColor = false;
			this.btn_Set.UseImage = false;
			this.btn_Set.UserHoverEmpahsize = false;
			this.btn_Set.UseSubFont = false;
			this.btn_Set.Click += new System.EventHandler(this.Click_Set);
			// 
			// lbl_AxisName
			// 
			this.lbl_AxisName.BackGroundColor = System.Drawing.Color.White;
			this.lbl_AxisName.BorderStroke = 2;
			this.lbl_AxisName.BorderStyle = System.Windows.Forms.ButtonBorderStyle.Solid;
			this.lbl_AxisName.Description = "";
			this.lbl_AxisName.DisabledColor = System.Drawing.Color.LightGray;
			this.lbl_AxisName.EdgeRadius = 1;
			this.lbl_AxisName.Enabled = false;
			this.lbl_AxisName.ImagePosition = new System.Drawing.Point(0, 0);
			this.lbl_AxisName.ImageSize = new System.Drawing.Point(0, 0);
			this.lbl_AxisName.LoadImage = null;
			this.lbl_AxisName.Location = new System.Drawing.Point(1, 22);
			this.lbl_AxisName.MainFont = new System.Drawing.Font("맑은 고딕", 10F, System.Drawing.FontStyle.Bold);
			this.lbl_AxisName.MainFontColor = System.Drawing.Color.Black;
			this.lbl_AxisName.Margin = new System.Windows.Forms.Padding(1, 1, 0, 0);
			this.lbl_AxisName.Name = "lbl_AxisName";
			this.lbl_AxisName.Size = new System.Drawing.Size(307, 27);
			this.lbl_AxisName.SubFont = new System.Drawing.Font("맑은 고딕", 12F, System.Drawing.FontStyle.Bold);
			this.lbl_AxisName.SubFontColor = System.Drawing.Color.Black;
			this.lbl_AxisName.SubText = "X";
			this.lbl_AxisName.TabIndex = 1439;
			this.lbl_AxisName.Text = "--";
			this.lbl_AxisName.TextAlignMain = Sys3Controls.EN_TEXTALIGN.BOTTOM_RIGHT;
			this.lbl_AxisName.TextAlignSub = Sys3Controls.EN_TEXTALIGN.MIDDLE_LEFT;
			this.lbl_AxisName.TextAlignUnit = Sys3Controls.EN_TEXTALIGN.BOTTOM_RIGHT;
			this.lbl_AxisName.ThemeIndex = 0;
			this.lbl_AxisName.UnitAreaRate = 25;
			this.lbl_AxisName.UnitFont = new System.Drawing.Font("맑은 고딕", 10F, System.Drawing.FontStyle.Bold);
			this.lbl_AxisName.UnitFontColor = System.Drawing.Color.Black;
			this.lbl_AxisName.UnitPositionVertical = false;
			this.lbl_AxisName.UnitText = "mm";
			this.lbl_AxisName.UseBorder = true;
			this.lbl_AxisName.UseEdgeRadius = false;
			this.lbl_AxisName.UseImage = false;
			this.lbl_AxisName.UseSubFont = true;
			this.lbl_AxisName.UseUnitFont = false;
			// 
			// lbl_Actual
			// 
			this.lbl_Actual.BackGroundColor = System.Drawing.Color.White;
			this.lbl_Actual.BorderStroke = 2;
			this.lbl_Actual.BorderStyle = System.Windows.Forms.ButtonBorderStyle.Solid;
			this.lbl_Actual.Description = "";
			this.lbl_Actual.DisabledColor = System.Drawing.Color.LightGray;
			this.lbl_Actual.EdgeRadius = 1;
			this.lbl_Actual.Enabled = false;
			this.lbl_Actual.ImagePosition = new System.Drawing.Point(0, 0);
			this.lbl_Actual.ImageSize = new System.Drawing.Point(0, 0);
			this.lbl_Actual.LoadImage = null;
			this.lbl_Actual.Location = new System.Drawing.Point(127, 71);
			this.lbl_Actual.MainFont = new System.Drawing.Font("맑은 고딕", 11F, System.Drawing.FontStyle.Bold);
			this.lbl_Actual.MainFontColor = System.Drawing.Color.Black;
			this.lbl_Actual.Margin = new System.Windows.Forms.Padding(1, 1, 0, 0);
			this.lbl_Actual.Name = "lbl_Actual";
			this.lbl_Actual.Size = new System.Drawing.Size(125, 27);
			this.lbl_Actual.SubFont = new System.Drawing.Font("맑은 고딕", 10F);
			this.lbl_Actual.SubFontColor = System.Drawing.Color.Black;
			this.lbl_Actual.SubText = "";
			this.lbl_Actual.TabIndex = 1431;
			this.lbl_Actual.Text = "--";
			this.lbl_Actual.TextAlignMain = Sys3Controls.EN_TEXTALIGN.BOTTOM_RIGHT;
			this.lbl_Actual.TextAlignSub = Sys3Controls.EN_TEXTALIGN.BOTTOM_RIGHT;
			this.lbl_Actual.TextAlignUnit = Sys3Controls.EN_TEXTALIGN.BOTTOM_RIGHT;
			this.lbl_Actual.ThemeIndex = 0;
			this.lbl_Actual.UnitAreaRate = 25;
			this.lbl_Actual.UnitFont = new System.Drawing.Font("맑은 고딕", 8F, System.Drawing.FontStyle.Bold);
			this.lbl_Actual.UnitFontColor = System.Drawing.Color.Black;
			this.lbl_Actual.UnitPositionVertical = false;
			this.lbl_Actual.UnitText = "mm";
			this.lbl_Actual.UseBorder = true;
			this.lbl_Actual.UseEdgeRadius = false;
			this.lbl_Actual.UseImage = false;
			this.lbl_Actual.UseSubFont = false;
			this.lbl_Actual.UseUnitFont = true;
			// 
			// lbl_Command
			// 
			this.lbl_Command.BackGroundColor = System.Drawing.Color.White;
			this.lbl_Command.BorderStroke = 2;
			this.lbl_Command.BorderStyle = System.Windows.Forms.ButtonBorderStyle.Solid;
			this.lbl_Command.Description = "";
			this.lbl_Command.DisabledColor = System.Drawing.Color.LightGray;
			this.lbl_Command.EdgeRadius = 1;
			this.lbl_Command.Enabled = false;
			this.lbl_Command.ImagePosition = new System.Drawing.Point(0, 0);
			this.lbl_Command.ImageSize = new System.Drawing.Point(0, 0);
			this.lbl_Command.LoadImage = null;
			this.lbl_Command.Location = new System.Drawing.Point(1, 71);
			this.lbl_Command.MainFont = new System.Drawing.Font("맑은 고딕", 11F, System.Drawing.FontStyle.Bold);
			this.lbl_Command.MainFontColor = System.Drawing.Color.Black;
			this.lbl_Command.Margin = new System.Windows.Forms.Padding(1, 1, 0, 0);
			this.lbl_Command.Name = "lbl_Command";
			this.lbl_Command.Size = new System.Drawing.Size(125, 27);
			this.lbl_Command.SubFont = new System.Drawing.Font("맑은 고딕", 10F);
			this.lbl_Command.SubFontColor = System.Drawing.Color.Black;
			this.lbl_Command.SubText = "";
			this.lbl_Command.TabIndex = 1432;
			this.lbl_Command.Text = "--";
			this.lbl_Command.TextAlignMain = Sys3Controls.EN_TEXTALIGN.BOTTOM_RIGHT;
			this.lbl_Command.TextAlignSub = Sys3Controls.EN_TEXTALIGN.BOTTOM_RIGHT;
			this.lbl_Command.TextAlignUnit = Sys3Controls.EN_TEXTALIGN.BOTTOM_RIGHT;
			this.lbl_Command.ThemeIndex = 0;
			this.lbl_Command.UnitAreaRate = 25;
			this.lbl_Command.UnitFont = new System.Drawing.Font("맑은 고딕", 8F, System.Drawing.FontStyle.Bold);
			this.lbl_Command.UnitFontColor = System.Drawing.Color.Black;
			this.lbl_Command.UnitPositionVertical = false;
			this.lbl_Command.UnitText = "mm";
			this.lbl_Command.UseBorder = true;
			this.lbl_Command.UseEdgeRadius = false;
			this.lbl_Command.UseImage = false;
			this.lbl_Command.UseSubFont = false;
			this.lbl_Command.UseUnitFont = true;
			// 
			// lbl_ParameterName
			// 
			this.lbl_ParameterName.BackGroundColor = System.Drawing.Color.Silver;
			this.lbl_ParameterName.BorderStroke = 2;
			this.lbl_ParameterName.BorderStyle = System.Windows.Forms.ButtonBorderStyle.Solid;
			this.lbl_ParameterName.Description = "";
			this.lbl_ParameterName.DisabledColor = System.Drawing.Color.DarkGray;
			this.lbl_ParameterName.EdgeRadius = 1;
			this.lbl_ParameterName.ImagePosition = new System.Drawing.Point(0, 0);
			this.lbl_ParameterName.ImageSize = new System.Drawing.Point(0, 0);
			this.lbl_ParameterName.LoadImage = null;
			this.lbl_ParameterName.Location = new System.Drawing.Point(316, 1);
			this.lbl_ParameterName.MainFont = new System.Drawing.Font("맑은 고딕", 9F, System.Drawing.FontStyle.Bold);
			this.lbl_ParameterName.MainFontColor = System.Drawing.Color.Black;
			this.lbl_ParameterName.Margin = new System.Windows.Forms.Padding(1, 1, 0, 0);
			this.lbl_ParameterName.Name = "lbl_ParameterName";
			this.lbl_ParameterName.Size = new System.Drawing.Size(195, 20);
			this.lbl_ParameterName.SubFont = new System.Drawing.Font("맑은 고딕", 10F);
			this.lbl_ParameterName.SubFontColor = System.Drawing.Color.Black;
			this.lbl_ParameterName.SubText = "";
			this.lbl_ParameterName.TabIndex = 1425;
			this.lbl_ParameterName.Text = "ABSOLUTE DESTINATION";
			this.lbl_ParameterName.TextAlignMain = Sys3Controls.EN_TEXTALIGN.MIDDLE_CENTER;
			this.lbl_ParameterName.TextAlignSub = Sys3Controls.EN_TEXTALIGN.MIDDLE_CENTER;
			this.lbl_ParameterName.TextAlignUnit = Sys3Controls.EN_TEXTALIGN.MIDDLE_CENTER;
			this.lbl_ParameterName.ThemeIndex = 0;
			this.lbl_ParameterName.UnitAreaRate = 40;
			this.lbl_ParameterName.UnitFont = new System.Drawing.Font("맑은 고딕", 10F, System.Drawing.FontStyle.Bold);
			this.lbl_ParameterName.UnitFontColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
			this.lbl_ParameterName.UnitPositionVertical = false;
			this.lbl_ParameterName.UnitText = "";
			this.lbl_ParameterName.UseBorder = true;
			this.lbl_ParameterName.UseEdgeRadius = false;
			this.lbl_ParameterName.UseImage = false;
			this.lbl_ParameterName.UseSubFont = false;
			this.lbl_ParameterName.UseUnitFont = false;
			// 
			// sys3Label6
			// 
			this.sys3Label6.BackGroundColor = System.Drawing.Color.Silver;
			this.sys3Label6.BorderStroke = 2;
			this.sys3Label6.BorderStyle = System.Windows.Forms.ButtonBorderStyle.Solid;
			this.sys3Label6.Description = "";
			this.sys3Label6.DisabledColor = System.Drawing.Color.DarkGray;
			this.sys3Label6.EdgeRadius = 1;
			this.sys3Label6.ImagePosition = new System.Drawing.Point(0, 0);
			this.sys3Label6.ImageSize = new System.Drawing.Point(0, 0);
			this.sys3Label6.LoadImage = null;
			this.sys3Label6.Location = new System.Drawing.Point(127, 50);
			this.sys3Label6.MainFont = new System.Drawing.Font("맑은 고딕", 9F, System.Drawing.FontStyle.Bold);
			this.sys3Label6.MainFontColor = System.Drawing.Color.Black;
			this.sys3Label6.Margin = new System.Windows.Forms.Padding(1, 1, 0, 0);
			this.sys3Label6.Name = "sys3Label6";
			this.sys3Label6.Size = new System.Drawing.Size(125, 20);
			this.sys3Label6.SubFont = new System.Drawing.Font("맑은 고딕", 10F);
			this.sys3Label6.SubFontColor = System.Drawing.Color.Black;
			this.sys3Label6.SubText = "";
			this.sys3Label6.TabIndex = 1421;
			this.sys3Label6.Text = "ACTUAL";
			this.sys3Label6.TextAlignMain = Sys3Controls.EN_TEXTALIGN.MIDDLE_CENTER;
			this.sys3Label6.TextAlignSub = Sys3Controls.EN_TEXTALIGN.MIDDLE_CENTER;
			this.sys3Label6.TextAlignUnit = Sys3Controls.EN_TEXTALIGN.MIDDLE_CENTER;
			this.sys3Label6.ThemeIndex = 0;
			this.sys3Label6.UnitAreaRate = 40;
			this.sys3Label6.UnitFont = new System.Drawing.Font("맑은 고딕", 10F, System.Drawing.FontStyle.Bold);
			this.sys3Label6.UnitFontColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
			this.sys3Label6.UnitPositionVertical = false;
			this.sys3Label6.UnitText = "";
			this.sys3Label6.UseBorder = true;
			this.sys3Label6.UseEdgeRadius = false;
			this.sys3Label6.UseImage = false;
			this.sys3Label6.UseSubFont = false;
			this.sys3Label6.UseUnitFont = false;
			// 
			// sys3Label4
			// 
			this.sys3Label4.BackGroundColor = System.Drawing.Color.Silver;
			this.sys3Label4.BorderStroke = 2;
			this.sys3Label4.BorderStyle = System.Windows.Forms.ButtonBorderStyle.Solid;
			this.sys3Label4.Description = "";
			this.sys3Label4.DisabledColor = System.Drawing.Color.DarkGray;
			this.sys3Label4.EdgeRadius = 1;
			this.sys3Label4.ImagePosition = new System.Drawing.Point(0, 0);
			this.sys3Label4.ImageSize = new System.Drawing.Point(0, 0);
			this.sys3Label4.LoadImage = null;
			this.sys3Label4.Location = new System.Drawing.Point(1, 50);
			this.sys3Label4.MainFont = new System.Drawing.Font("맑은 고딕", 9F, System.Drawing.FontStyle.Bold);
			this.sys3Label4.MainFontColor = System.Drawing.Color.Black;
			this.sys3Label4.Margin = new System.Windows.Forms.Padding(1, 1, 0, 0);
			this.sys3Label4.Name = "sys3Label4";
			this.sys3Label4.Size = new System.Drawing.Size(125, 20);
			this.sys3Label4.SubFont = new System.Drawing.Font("맑은 고딕", 10F);
			this.sys3Label4.SubFontColor = System.Drawing.Color.Black;
			this.sys3Label4.SubText = "";
			this.sys3Label4.TabIndex = 1417;
			this.sys3Label4.Text = "COMMAND";
			this.sys3Label4.TextAlignMain = Sys3Controls.EN_TEXTALIGN.MIDDLE_CENTER;
			this.sys3Label4.TextAlignSub = Sys3Controls.EN_TEXTALIGN.MIDDLE_CENTER;
			this.sys3Label4.TextAlignUnit = Sys3Controls.EN_TEXTALIGN.MIDDLE_CENTER;
			this.sys3Label4.ThemeIndex = 0;
			this.sys3Label4.UnitAreaRate = 40;
			this.sys3Label4.UnitFont = new System.Drawing.Font("맑은 고딕", 10F, System.Drawing.FontStyle.Bold);
			this.sys3Label4.UnitFontColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
			this.sys3Label4.UnitPositionVertical = false;
			this.sys3Label4.UnitText = "";
			this.sys3Label4.UseBorder = true;
			this.sys3Label4.UseEdgeRadius = false;
			this.sys3Label4.UseImage = false;
			this.sys3Label4.UseSubFont = false;
			this.sys3Label4.UseUnitFont = false;
			// 
			// panel_StackItem
			// 
			this.panel_StackItem.Controls.Add(this.m_lblPositionAxis);
			this.panel_StackItem.Controls.Add(this.sys3Label4);
			this.panel_StackItem.Controls.Add(this.lbl_ParameterValue);
			this.panel_StackItem.Controls.Add(this.sys3Label6);
			this.panel_StackItem.Controls.Add(this.btn_Measure);
			this.panel_StackItem.Controls.Add(this.lbl_ParameterName);
			this.panel_StackItem.Controls.Add(this.btn_Go);
			this.panel_StackItem.Controls.Add(this.lbl_Command);
			this.panel_StackItem.Controls.Add(this.btn_Set);
			this.panel_StackItem.Controls.Add(this.lbl_Actual);
			this.panel_StackItem.Controls.Add(this.lbl_AxisName);
			this.panel_StackItem.Dock = System.Windows.Forms.DockStyle.Top;
			this.panel_StackItem.Location = new System.Drawing.Point(1, 1);
			this.panel_StackItem.Margin = new System.Windows.Forms.Padding(1, 1, 0, 0);
			this.panel_StackItem.Name = "panel_StackItem";
			this.panel_StackItem.Size = new System.Drawing.Size(511, 105);
			this.panel_StackItem.TabIndex = 1452;
			// 
			// JogPanel_MonitorItem
			// 
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
			this.Controls.Add(this.panel_StackItem);
			this.Margin = new System.Windows.Forms.Padding(1, 1, 0, 0);
			this.Name = "JogPanel_MonitorItem";
			this.Padding = new System.Windows.Forms.Padding(1);
			this.Size = new System.Drawing.Size(513, 105);
			this.Tag = "";
			this.panel_StackItem.ResumeLayout(false);
			this.ResumeLayout(false);

		}

		#endregion

		private Sys3Controls.Sys3Label m_lblPositionAxis;
		private Sys3Controls.Sys3Label lbl_ParameterValue;
		private Sys3Controls.Sys3button btn_Measure;
		private Sys3Controls.Sys3button btn_Go;
		private Sys3Controls.Sys3button btn_Set;
		private Sys3Controls.Sys3Label lbl_AxisName;
		private Sys3Controls.Sys3Label lbl_Actual;
		private Sys3Controls.Sys3Label lbl_Command;
		private Sys3Controls.Sys3Label lbl_ParameterName;
		private Sys3Controls.Sys3Label sys3Label6;
		private Sys3Controls.Sys3Label sys3Label4;
		private System.Windows.Forms.Panel panel_StackItem;






	}
}
