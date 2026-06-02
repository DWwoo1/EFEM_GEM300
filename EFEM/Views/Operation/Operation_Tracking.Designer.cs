namespace FrameOfSystem3.Views.Operation
{
	partial class Operation_Tracking
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Operation_Tracking));
            this.zedGraphControl1 = new ZedGraph.ZedGraphControl();
            this.m_btnAdd1 = new Sys3Controls.Sys3button();
            this._groupBox_DeviceView = new Sys3Controls.Sys3GroupBoxContainer();
            this._tableLayoutPanel_DeviceView = new System.Windows.Forms.TableLayoutPanel();
            this._tableLayoutPanel_DeviceViewRight = new System.Windows.Forms.TableLayoutPanel();
            this._label_Cylinder = new Sys3Controls.Sys3Label();
            this._label_Analog = new Sys3Controls.Sys3Label();
            this._label_Digital = new Sys3Controls.Sys3Label();
            this._label_Motion = new Sys3Controls.Sys3Label();
            this.m_btnAdd3 = new Sys3Controls.Sys3button();
            this.m_btnAdd4 = new Sys3Controls.Sys3button();
            this.m_btnAdd2 = new Sys3Controls.Sys3button();
            this._btn_ClearItem = new Sys3Controls.Sys3button();
            this._groupBox_DeviceItem = new Sys3Controls.Sys3GroupBoxContainer();
            this._tableLayoutPanel_DeviceItem = new System.Windows.Forms.TableLayoutPanel();
            this._tableLayoutPanel_Tracking = new System.Windows.Forms.TableLayoutPanel();
            this.m_labelStart = new Sys3Controls.Sys3Label();
            this.m_ToggleInputOnDelay = new Sys3Controls.Sys3ToggleButton();
            this._tableLayoutPanel_Main = new System.Windows.Forms.TableLayoutPanel();
            this._panel_TrackingSwitch = new System.Windows.Forms.Panel();
            this._groupBox_DeviceView.SuspendLayout();
            this._tableLayoutPanel_DeviceView.SuspendLayout();
            this._tableLayoutPanel_DeviceViewRight.SuspendLayout();
            this._groupBox_DeviceItem.SuspendLayout();
            this._tableLayoutPanel_DeviceItem.SuspendLayout();
            this._tableLayoutPanel_Tracking.SuspendLayout();
            this._tableLayoutPanel_Main.SuspendLayout();
            this._panel_TrackingSwitch.SuspendLayout();
            this.SuspendLayout();
            // 
            // zedGraphControl1
            // 
            this.zedGraphControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.zedGraphControl1.Location = new System.Drawing.Point(0, 0);
            this.zedGraphControl1.Margin = new System.Windows.Forms.Padding(0);
            this.zedGraphControl1.Name = "zedGraphControl1";
            this.zedGraphControl1.ScrollGrace = 0D;
            this.zedGraphControl1.ScrollMaxX = 0D;
            this.zedGraphControl1.ScrollMaxY = 0D;
            this.zedGraphControl1.ScrollMaxY2 = 0D;
            this.zedGraphControl1.ScrollMinX = 0D;
            this.zedGraphControl1.ScrollMinY = 0D;
            this.zedGraphControl1.ScrollMinY2 = 0D;
            this.zedGraphControl1.Size = new System.Drawing.Size(994, 702);
            this.zedGraphControl1.TabIndex = 0;
            // 
            // m_btnAdd1
            // 
            this.m_btnAdd1.BorderWidth = 3;
            this.m_btnAdd1.ButtonClicked = false;
            this.m_btnAdd1.ClickedEmphasizeTextColor = System.Drawing.Color.White;
            this.m_btnAdd1.CustomClickedGradientFirstColor = System.Drawing.Color.White;
            this.m_btnAdd1.CustomClickedGradientSecondColor = System.Drawing.Color.White;
            this.m_btnAdd1.Description = "";
            this.m_btnAdd1.DisabledColor = System.Drawing.Color.DarkGray;
            this.m_btnAdd1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.m_btnAdd1.EdgeRadius = 5;
            this.m_btnAdd1.GradientAngle = 50F;
            this.m_btnAdd1.GradientFirstColor = System.Drawing.Color.White;
            this.m_btnAdd1.GradientSecondColor = System.Drawing.Color.FromArgb(((int)(((byte)(170)))), ((int)(((byte)(176)))), ((int)(((byte)(183)))));
            this.m_btnAdd1.HoverEmphasizeCustomColor = System.Drawing.Color.White;
            this.m_btnAdd1.ImagePosition = new System.Drawing.Point(12, 10);
            this.m_btnAdd1.ImageSize = new System.Drawing.Point(30, 30);
            this.m_btnAdd1.LoadImage = global::FrameOfSystem3.Properties.Resources.CONFIG_ADD3;
            this.m_btnAdd1.Location = new System.Drawing.Point(19, 14);
            this.m_btnAdd1.MainFont = new System.Drawing.Font("맑은 고딕", 14F, System.Drawing.FontStyle.Bold);
            this.m_btnAdd1.MainFontColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(36)))), ((int)(((byte)(0)))));
            this.m_btnAdd1.Name = "m_btnAdd1";
            this.m_btnAdd1.Size = new System.Drawing.Size(162, 98);
            this.m_btnAdd1.SubFont = new System.Drawing.Font("맑은 고딕", 12F, System.Drawing.FontStyle.Bold);
            this.m_btnAdd1.SubFontColor = System.Drawing.Color.DarkBlue;
            this.m_btnAdd1.SubText = "MOTION";
            this.m_btnAdd1.TabIndex = 0;
            this.m_btnAdd1.Text = "\\nADD";
            this.m_btnAdd1.TextAlignMain = Sys3Controls.EN_TEXTALIGN.MIDDLE_RIGHT;
            this.m_btnAdd1.TextAlignSub = Sys3Controls.EN_TEXTALIGN.TOP_RIGHT;
            this.m_btnAdd1.ThemeIndex = 0;
            this.m_btnAdd1.UseBorder = true;
            this.m_btnAdd1.UseClickedEmphasizeTextColor = false;
            this.m_btnAdd1.UseCustomizeClickedColor = false;
            this.m_btnAdd1.UseEdge = true;
            this.m_btnAdd1.UseHoverEmphasizeCustomColor = false;
            this.m_btnAdd1.UseImage = true;
            this.m_btnAdd1.UserHoverEmpahsize = false;
            this.m_btnAdd1.UseSubFont = true;
            this.m_btnAdd1.Click += new System.EventHandler(this.Click_AddButton);
            // 
            // _groupBox_DeviceView
            // 
            this._groupBox_DeviceView.BackGroundColor = System.Drawing.Color.WhiteSmoke;
            this._groupBox_DeviceView.Controls.Add(this._tableLayoutPanel_DeviceView);
            this._groupBox_DeviceView.Dock = System.Windows.Forms.DockStyle.Fill;
            this._groupBox_DeviceView.EdgeBorderStroke = 2;
            this._groupBox_DeviceView.EdgeRadius = 2;
            this._groupBox_DeviceView.LabelFont = new System.Drawing.Font("맑은 고딕", 10F, System.Drawing.FontStyle.Bold);
            this._groupBox_DeviceView.LabelGradientColorFirst = System.Drawing.Color.FromArgb(((int)(((byte)(208)))), ((int)(((byte)(208)))), ((int)(((byte)(208)))));
            this._groupBox_DeviceView.LabelGradientColorSecond = System.Drawing.Color.FromArgb(((int)(((byte)(240)))), ((int)(((byte)(240)))), ((int)(((byte)(240)))));
            this._groupBox_DeviceView.LabelHeight = 30;
            this._groupBox_DeviceView.LabelTextColor = System.Drawing.Color.Black;
            this._groupBox_DeviceView.Location = new System.Drawing.Point(0, 0);
            this._groupBox_DeviceView.Margin = new System.Windows.Forms.Padding(0);
            this._groupBox_DeviceView.Name = "_groupBox_DeviceView";
            this._groupBox_DeviceView.Padding = new System.Windows.Forms.Padding(3, 18, 3, 3);
            this._groupBox_DeviceView.Size = new System.Drawing.Size(1140, 737);
            this._groupBox_DeviceView.TabIndex = 1353;
            this._groupBox_DeviceView.TabStop = false;
            this._groupBox_DeviceView.Text = "DEVICE VIEW";
            this._groupBox_DeviceView.TextAlign = Sys3Controls.EN_TEXTALIGN.MIDDLE_CENTER;
            this._groupBox_DeviceView.ThemeIndex = 0;
            this._groupBox_DeviceView.UseLabelBorder = true;
            this._groupBox_DeviceView.UseTitle = true;
            // 
            // _tableLayoutPanel_DeviceView
            // 
            this._tableLayoutPanel_DeviceView.ColumnCount = 2;
            this._tableLayoutPanel_DeviceView.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 87.74251F));
            this._tableLayoutPanel_DeviceView.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 12.2575F));
            this._tableLayoutPanel_DeviceView.Controls.Add(this.zedGraphControl1, 0, 0);
            this._tableLayoutPanel_DeviceView.Controls.Add(this._tableLayoutPanel_DeviceViewRight, 1, 0);
            this._tableLayoutPanel_DeviceView.Dock = System.Windows.Forms.DockStyle.Fill;
            this._tableLayoutPanel_DeviceView.Location = new System.Drawing.Point(3, 32);
            this._tableLayoutPanel_DeviceView.Name = "_tableLayoutPanel_DeviceView";
            this._tableLayoutPanel_DeviceView.RowCount = 1;
            this._tableLayoutPanel_DeviceView.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this._tableLayoutPanel_DeviceView.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this._tableLayoutPanel_DeviceView.Size = new System.Drawing.Size(1134, 702);
            this._tableLayoutPanel_DeviceView.TabIndex = 0;
            // 
            // _tableLayoutPanel_DeviceViewRight
            // 
            this._tableLayoutPanel_DeviceViewRight.ColumnCount = 3;
            this._tableLayoutPanel_DeviceViewRight.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 5.357143F));
            this._tableLayoutPanel_DeviceViewRight.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 89.28571F));
            this._tableLayoutPanel_DeviceViewRight.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 5.357143F));
            this._tableLayoutPanel_DeviceViewRight.Controls.Add(this._label_Cylinder, 1, 1);
            this._tableLayoutPanel_DeviceViewRight.Controls.Add(this._label_Analog, 1, 2);
            this._tableLayoutPanel_DeviceViewRight.Controls.Add(this._label_Digital, 1, 3);
            this._tableLayoutPanel_DeviceViewRight.Controls.Add(this._label_Motion, 1, 4);
            this._tableLayoutPanel_DeviceViewRight.Dock = System.Windows.Forms.DockStyle.Fill;
            this._tableLayoutPanel_DeviceViewRight.Location = new System.Drawing.Point(994, 0);
            this._tableLayoutPanel_DeviceViewRight.Margin = new System.Windows.Forms.Padding(0);
            this._tableLayoutPanel_DeviceViewRight.Name = "_tableLayoutPanel_DeviceViewRight";
            this._tableLayoutPanel_DeviceViewRight.RowCount = 6;
            this._tableLayoutPanel_DeviceViewRight.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 16.66667F));
            this._tableLayoutPanel_DeviceViewRight.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 16.66667F));
            this._tableLayoutPanel_DeviceViewRight.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 16.66667F));
            this._tableLayoutPanel_DeviceViewRight.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 16.66667F));
            this._tableLayoutPanel_DeviceViewRight.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 16.66667F));
            this._tableLayoutPanel_DeviceViewRight.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 16.66667F));
            this._tableLayoutPanel_DeviceViewRight.Size = new System.Drawing.Size(140, 702);
            this._tableLayoutPanel_DeviceViewRight.TabIndex = 1;
            // 
            // _label_Cylinder
            // 
            this._label_Cylinder.BackGroundColor = System.Drawing.Color.FromArgb(((int)(((byte)(200)))), ((int)(((byte)(200)))), ((int)(((byte)(220)))));
            this._label_Cylinder.BorderStroke = 2;
            this._label_Cylinder.BorderStyle = System.Windows.Forms.ButtonBorderStyle.Solid;
            this._label_Cylinder.Description = "";
            this._label_Cylinder.DisabledColor = System.Drawing.Color.Silver;
            this._label_Cylinder.Dock = System.Windows.Forms.DockStyle.Fill;
            this._label_Cylinder.EdgeRadius = 1;
            this._label_Cylinder.ImagePosition = new System.Drawing.Point(0, 0);
            this._label_Cylinder.ImageSize = new System.Drawing.Point(0, 0);
            this._label_Cylinder.LoadImage = null;
            this._label_Cylinder.Location = new System.Drawing.Point(8, 117);
            this._label_Cylinder.MainFont = new System.Drawing.Font("맑은 고딕", 12F, System.Drawing.FontStyle.Bold);
            this._label_Cylinder.MainFontColor = System.Drawing.Color.Black;
            this._label_Cylinder.Margin = new System.Windows.Forms.Padding(1);
            this._label_Cylinder.Name = "_label_Cylinder";
            this._label_Cylinder.Size = new System.Drawing.Size(123, 114);
            this._label_Cylinder.SubFont = new System.Drawing.Font("맑은 고딕", 10F);
            this._label_Cylinder.SubFontColor = System.Drawing.Color.Black;
            this._label_Cylinder.SubText = "";
            this._label_Cylinder.TabIndex = 1358;
            this._label_Cylinder.Text = "CYLINDER";
            this._label_Cylinder.TextAlignMain = Sys3Controls.EN_TEXTALIGN.MIDDLE_CENTER;
            this._label_Cylinder.TextAlignSub = Sys3Controls.EN_TEXTALIGN.MIDDLE_CENTER;
            this._label_Cylinder.TextAlignUnit = Sys3Controls.EN_TEXTALIGN.MIDDLE_CENTER;
            this._label_Cylinder.ThemeIndex = 0;
            this._label_Cylinder.UnitAreaRate = 40;
            this._label_Cylinder.UnitFont = new System.Drawing.Font("맑은 고딕", 10F, System.Drawing.FontStyle.Bold);
            this._label_Cylinder.UnitFontColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this._label_Cylinder.UnitPositionVertical = false;
            this._label_Cylinder.UnitText = "";
            this._label_Cylinder.UseBorder = true;
            this._label_Cylinder.UseEdgeRadius = false;
            this._label_Cylinder.UseImage = false;
            this._label_Cylinder.UseSubFont = false;
            this._label_Cylinder.UseUnitFont = false;
            // 
            // _label_Analog
            // 
            this._label_Analog.BackGroundColor = System.Drawing.Color.FromArgb(((int)(((byte)(200)))), ((int)(((byte)(200)))), ((int)(((byte)(220)))));
            this._label_Analog.BorderStroke = 2;
            this._label_Analog.BorderStyle = System.Windows.Forms.ButtonBorderStyle.Solid;
            this._label_Analog.Description = "";
            this._label_Analog.DisabledColor = System.Drawing.Color.Silver;
            this._label_Analog.Dock = System.Windows.Forms.DockStyle.Fill;
            this._label_Analog.EdgeRadius = 1;
            this._label_Analog.ImagePosition = new System.Drawing.Point(0, 0);
            this._label_Analog.ImageSize = new System.Drawing.Point(0, 0);
            this._label_Analog.LoadImage = null;
            this._label_Analog.Location = new System.Drawing.Point(8, 233);
            this._label_Analog.MainFont = new System.Drawing.Font("맑은 고딕", 12F, System.Drawing.FontStyle.Bold);
            this._label_Analog.MainFontColor = System.Drawing.Color.Black;
            this._label_Analog.Margin = new System.Windows.Forms.Padding(1);
            this._label_Analog.Name = "_label_Analog";
            this._label_Analog.Size = new System.Drawing.Size(123, 114);
            this._label_Analog.SubFont = new System.Drawing.Font("맑은 고딕", 10F);
            this._label_Analog.SubFontColor = System.Drawing.Color.Black;
            this._label_Analog.SubText = "";
            this._label_Analog.TabIndex = 1358;
            this._label_Analog.Text = "ANALOG";
            this._label_Analog.TextAlignMain = Sys3Controls.EN_TEXTALIGN.MIDDLE_CENTER;
            this._label_Analog.TextAlignSub = Sys3Controls.EN_TEXTALIGN.MIDDLE_CENTER;
            this._label_Analog.TextAlignUnit = Sys3Controls.EN_TEXTALIGN.MIDDLE_CENTER;
            this._label_Analog.ThemeIndex = 0;
            this._label_Analog.UnitAreaRate = 40;
            this._label_Analog.UnitFont = new System.Drawing.Font("맑은 고딕", 10F, System.Drawing.FontStyle.Bold);
            this._label_Analog.UnitFontColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this._label_Analog.UnitPositionVertical = false;
            this._label_Analog.UnitText = "";
            this._label_Analog.UseBorder = true;
            this._label_Analog.UseEdgeRadius = false;
            this._label_Analog.UseImage = false;
            this._label_Analog.UseSubFont = false;
            this._label_Analog.UseUnitFont = false;
            // 
            // _label_Digital
            // 
            this._label_Digital.BackGroundColor = System.Drawing.Color.FromArgb(((int)(((byte)(200)))), ((int)(((byte)(200)))), ((int)(((byte)(220)))));
            this._label_Digital.BorderStroke = 2;
            this._label_Digital.BorderStyle = System.Windows.Forms.ButtonBorderStyle.Solid;
            this._label_Digital.Description = "";
            this._label_Digital.DisabledColor = System.Drawing.Color.Silver;
            this._label_Digital.Dock = System.Windows.Forms.DockStyle.Fill;
            this._label_Digital.EdgeRadius = 1;
            this._label_Digital.ImagePosition = new System.Drawing.Point(0, 0);
            this._label_Digital.ImageSize = new System.Drawing.Point(0, 0);
            this._label_Digital.LoadImage = null;
            this._label_Digital.Location = new System.Drawing.Point(8, 349);
            this._label_Digital.MainFont = new System.Drawing.Font("맑은 고딕", 12F, System.Drawing.FontStyle.Bold);
            this._label_Digital.MainFontColor = System.Drawing.Color.Black;
            this._label_Digital.Margin = new System.Windows.Forms.Padding(1);
            this._label_Digital.Name = "_label_Digital";
            this._label_Digital.Size = new System.Drawing.Size(123, 114);
            this._label_Digital.SubFont = new System.Drawing.Font("맑은 고딕", 10F);
            this._label_Digital.SubFontColor = System.Drawing.Color.Black;
            this._label_Digital.SubText = "";
            this._label_Digital.TabIndex = 1358;
            this._label_Digital.Text = "DIGITAL";
            this._label_Digital.TextAlignMain = Sys3Controls.EN_TEXTALIGN.MIDDLE_CENTER;
            this._label_Digital.TextAlignSub = Sys3Controls.EN_TEXTALIGN.MIDDLE_CENTER;
            this._label_Digital.TextAlignUnit = Sys3Controls.EN_TEXTALIGN.MIDDLE_CENTER;
            this._label_Digital.ThemeIndex = 0;
            this._label_Digital.UnitAreaRate = 40;
            this._label_Digital.UnitFont = new System.Drawing.Font("맑은 고딕", 10F, System.Drawing.FontStyle.Bold);
            this._label_Digital.UnitFontColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this._label_Digital.UnitPositionVertical = false;
            this._label_Digital.UnitText = "";
            this._label_Digital.UseBorder = true;
            this._label_Digital.UseEdgeRadius = false;
            this._label_Digital.UseImage = false;
            this._label_Digital.UseSubFont = false;
            this._label_Digital.UseUnitFont = false;
            // 
            // _label_Motion
            // 
            this._label_Motion.BackGroundColor = System.Drawing.Color.FromArgb(((int)(((byte)(200)))), ((int)(((byte)(200)))), ((int)(((byte)(220)))));
            this._label_Motion.BorderStroke = 2;
            this._label_Motion.BorderStyle = System.Windows.Forms.ButtonBorderStyle.Solid;
            this._label_Motion.Description = "";
            this._label_Motion.DisabledColor = System.Drawing.Color.Silver;
            this._label_Motion.Dock = System.Windows.Forms.DockStyle.Fill;
            this._label_Motion.EdgeRadius = 1;
            this._label_Motion.ImagePosition = new System.Drawing.Point(0, 0);
            this._label_Motion.ImageSize = new System.Drawing.Point(0, 0);
            this._label_Motion.LoadImage = null;
            this._label_Motion.Location = new System.Drawing.Point(8, 465);
            this._label_Motion.MainFont = new System.Drawing.Font("맑은 고딕", 12F, System.Drawing.FontStyle.Bold);
            this._label_Motion.MainFontColor = System.Drawing.Color.Black;
            this._label_Motion.Margin = new System.Windows.Forms.Padding(1);
            this._label_Motion.Name = "_label_Motion";
            this._label_Motion.Size = new System.Drawing.Size(123, 114);
            this._label_Motion.SubFont = new System.Drawing.Font("맑은 고딕", 10F);
            this._label_Motion.SubFontColor = System.Drawing.Color.Black;
            this._label_Motion.SubText = "";
            this._label_Motion.TabIndex = 1358;
            this._label_Motion.Text = "MOTION";
            this._label_Motion.TextAlignMain = Sys3Controls.EN_TEXTALIGN.MIDDLE_CENTER;
            this._label_Motion.TextAlignSub = Sys3Controls.EN_TEXTALIGN.MIDDLE_CENTER;
            this._label_Motion.TextAlignUnit = Sys3Controls.EN_TEXTALIGN.MIDDLE_CENTER;
            this._label_Motion.ThemeIndex = 0;
            this._label_Motion.UnitAreaRate = 40;
            this._label_Motion.UnitFont = new System.Drawing.Font("맑은 고딕", 10F, System.Drawing.FontStyle.Bold);
            this._label_Motion.UnitFontColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this._label_Motion.UnitPositionVertical = false;
            this._label_Motion.UnitText = "";
            this._label_Motion.UseBorder = true;
            this._label_Motion.UseEdgeRadius = false;
            this._label_Motion.UseImage = false;
            this._label_Motion.UseSubFont = false;
            this._label_Motion.UseUnitFont = false;
            // 
            // m_btnAdd3
            // 
            this.m_btnAdd3.BorderWidth = 3;
            this.m_btnAdd3.ButtonClicked = false;
            this.m_btnAdd3.ClickedEmphasizeTextColor = System.Drawing.Color.White;
            this.m_btnAdd3.CustomClickedGradientFirstColor = System.Drawing.Color.White;
            this.m_btnAdd3.CustomClickedGradientSecondColor = System.Drawing.Color.White;
            this.m_btnAdd3.Description = "";
            this.m_btnAdd3.DisabledColor = System.Drawing.Color.DarkGray;
            this.m_btnAdd3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.m_btnAdd3.EdgeRadius = 5;
            this.m_btnAdd3.GradientAngle = 50F;
            this.m_btnAdd3.GradientFirstColor = System.Drawing.Color.White;
            this.m_btnAdd3.GradientSecondColor = System.Drawing.Color.FromArgb(((int)(((byte)(170)))), ((int)(((byte)(176)))), ((int)(((byte)(183)))));
            this.m_btnAdd3.HoverEmphasizeCustomColor = System.Drawing.Color.White;
            this.m_btnAdd3.ImagePosition = new System.Drawing.Point(12, 10);
            this.m_btnAdd3.ImageSize = new System.Drawing.Point(30, 30);
            this.m_btnAdd3.LoadImage = ((System.Drawing.Image)(resources.GetObject("m_btnAdd3.LoadImage")));
            this.m_btnAdd3.Location = new System.Drawing.Point(187, 14);
            this.m_btnAdd3.MainFont = new System.Drawing.Font("맑은 고딕", 14F, System.Drawing.FontStyle.Bold);
            this.m_btnAdd3.MainFontColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(36)))), ((int)(((byte)(0)))));
            this.m_btnAdd3.Name = "m_btnAdd3";
            this.m_btnAdd3.Size = new System.Drawing.Size(162, 98);
            this.m_btnAdd3.SubFont = new System.Drawing.Font("맑은 고딕", 12F, System.Drawing.FontStyle.Bold);
            this.m_btnAdd3.SubFontColor = System.Drawing.Color.DarkBlue;
            this.m_btnAdd3.SubText = "DIGITAL";
            this.m_btnAdd3.TabIndex = 1;
            this.m_btnAdd3.Text = "\\nADD";
            this.m_btnAdd3.TextAlignMain = Sys3Controls.EN_TEXTALIGN.MIDDLE_RIGHT;
            this.m_btnAdd3.TextAlignSub = Sys3Controls.EN_TEXTALIGN.TOP_RIGHT;
            this.m_btnAdd3.ThemeIndex = 0;
            this.m_btnAdd3.UseBorder = true;
            this.m_btnAdd3.UseClickedEmphasizeTextColor = false;
            this.m_btnAdd3.UseCustomizeClickedColor = false;
            this.m_btnAdd3.UseEdge = true;
            this.m_btnAdd3.UseHoverEmphasizeCustomColor = false;
            this.m_btnAdd3.UseImage = true;
            this.m_btnAdd3.UserHoverEmpahsize = false;
            this.m_btnAdd3.UseSubFont = true;
            this.m_btnAdd3.Click += new System.EventHandler(this.Click_AddButton);
            // 
            // m_btnAdd4
            // 
            this.m_btnAdd4.BorderWidth = 3;
            this.m_btnAdd4.ButtonClicked = false;
            this.m_btnAdd4.ClickedEmphasizeTextColor = System.Drawing.Color.White;
            this.m_btnAdd4.CustomClickedGradientFirstColor = System.Drawing.Color.White;
            this.m_btnAdd4.CustomClickedGradientSecondColor = System.Drawing.Color.White;
            this.m_btnAdd4.Description = "";
            this.m_btnAdd4.DisabledColor = System.Drawing.Color.DarkGray;
            this.m_btnAdd4.Dock = System.Windows.Forms.DockStyle.Fill;
            this.m_btnAdd4.EdgeRadius = 5;
            this.m_btnAdd4.GradientAngle = 50F;
            this.m_btnAdd4.GradientFirstColor = System.Drawing.Color.White;
            this.m_btnAdd4.GradientSecondColor = System.Drawing.Color.FromArgb(((int)(((byte)(170)))), ((int)(((byte)(176)))), ((int)(((byte)(183)))));
            this.m_btnAdd4.HoverEmphasizeCustomColor = System.Drawing.Color.White;
            this.m_btnAdd4.ImagePosition = new System.Drawing.Point(12, 10);
            this.m_btnAdd4.ImageSize = new System.Drawing.Point(30, 30);
            this.m_btnAdd4.LoadImage = ((System.Drawing.Image)(resources.GetObject("m_btnAdd4.LoadImage")));
            this.m_btnAdd4.Location = new System.Drawing.Point(523, 14);
            this.m_btnAdd4.MainFont = new System.Drawing.Font("맑은 고딕", 14F, System.Drawing.FontStyle.Bold);
            this.m_btnAdd4.MainFontColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(36)))), ((int)(((byte)(0)))));
            this.m_btnAdd4.Name = "m_btnAdd4";
            this.m_btnAdd4.Size = new System.Drawing.Size(162, 98);
            this.m_btnAdd4.SubFont = new System.Drawing.Font("맑은 고딕", 12F, System.Drawing.FontStyle.Bold);
            this.m_btnAdd4.SubFontColor = System.Drawing.Color.DarkBlue;
            this.m_btnAdd4.SubText = "CYLINDER";
            this.m_btnAdd4.TabIndex = 3;
            this.m_btnAdd4.Text = "\\nADD";
            this.m_btnAdd4.TextAlignMain = Sys3Controls.EN_TEXTALIGN.MIDDLE_RIGHT;
            this.m_btnAdd4.TextAlignSub = Sys3Controls.EN_TEXTALIGN.TOP_RIGHT;
            this.m_btnAdd4.ThemeIndex = 0;
            this.m_btnAdd4.UseBorder = true;
            this.m_btnAdd4.UseClickedEmphasizeTextColor = false;
            this.m_btnAdd4.UseCustomizeClickedColor = false;
            this.m_btnAdd4.UseEdge = true;
            this.m_btnAdd4.UseHoverEmphasizeCustomColor = false;
            this.m_btnAdd4.UseImage = true;
            this.m_btnAdd4.UserHoverEmpahsize = false;
            this.m_btnAdd4.UseSubFont = true;
            this.m_btnAdd4.Click += new System.EventHandler(this.Click_AddButton);
            // 
            // m_btnAdd2
            // 
            this.m_btnAdd2.BorderWidth = 3;
            this.m_btnAdd2.ButtonClicked = false;
            this.m_btnAdd2.ClickedEmphasizeTextColor = System.Drawing.Color.White;
            this.m_btnAdd2.CustomClickedGradientFirstColor = System.Drawing.Color.White;
            this.m_btnAdd2.CustomClickedGradientSecondColor = System.Drawing.Color.White;
            this.m_btnAdd2.Description = "";
            this.m_btnAdd2.DisabledColor = System.Drawing.Color.DarkGray;
            this.m_btnAdd2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.m_btnAdd2.EdgeRadius = 5;
            this.m_btnAdd2.GradientAngle = 50F;
            this.m_btnAdd2.GradientFirstColor = System.Drawing.Color.White;
            this.m_btnAdd2.GradientSecondColor = System.Drawing.Color.FromArgb(((int)(((byte)(170)))), ((int)(((byte)(176)))), ((int)(((byte)(183)))));
            this.m_btnAdd2.HoverEmphasizeCustomColor = System.Drawing.Color.White;
            this.m_btnAdd2.ImagePosition = new System.Drawing.Point(12, 10);
            this.m_btnAdd2.ImageSize = new System.Drawing.Point(30, 30);
            this.m_btnAdd2.LoadImage = ((System.Drawing.Image)(resources.GetObject("m_btnAdd2.LoadImage")));
            this.m_btnAdd2.Location = new System.Drawing.Point(355, 14);
            this.m_btnAdd2.MainFont = new System.Drawing.Font("맑은 고딕", 14F, System.Drawing.FontStyle.Bold);
            this.m_btnAdd2.MainFontColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(36)))), ((int)(((byte)(0)))));
            this.m_btnAdd2.Name = "m_btnAdd2";
            this.m_btnAdd2.Size = new System.Drawing.Size(162, 98);
            this.m_btnAdd2.SubFont = new System.Drawing.Font("맑은 고딕", 12F, System.Drawing.FontStyle.Bold);
            this.m_btnAdd2.SubFontColor = System.Drawing.Color.DarkBlue;
            this.m_btnAdd2.SubText = "ANALOG";
            this.m_btnAdd2.TabIndex = 2;
            this.m_btnAdd2.Text = "\\nADD";
            this.m_btnAdd2.TextAlignMain = Sys3Controls.EN_TEXTALIGN.MIDDLE_RIGHT;
            this.m_btnAdd2.TextAlignSub = Sys3Controls.EN_TEXTALIGN.TOP_RIGHT;
            this.m_btnAdd2.ThemeIndex = 0;
            this.m_btnAdd2.UseBorder = true;
            this.m_btnAdd2.UseClickedEmphasizeTextColor = false;
            this.m_btnAdd2.UseCustomizeClickedColor = false;
            this.m_btnAdd2.UseEdge = true;
            this.m_btnAdd2.UseHoverEmphasizeCustomColor = false;
            this.m_btnAdd2.UseImage = true;
            this.m_btnAdd2.UserHoverEmpahsize = false;
            this.m_btnAdd2.UseSubFont = true;
            this.m_btnAdd2.Click += new System.EventHandler(this.Click_AddButton);
            // 
            // _btn_ClearItem
            // 
            this._btn_ClearItem.BorderWidth = 3;
            this._btn_ClearItem.ButtonClicked = false;
            this._btn_ClearItem.ClickedEmphasizeTextColor = System.Drawing.Color.White;
            this._btn_ClearItem.CustomClickedGradientFirstColor = System.Drawing.Color.White;
            this._btn_ClearItem.CustomClickedGradientSecondColor = System.Drawing.Color.White;
            this._btn_ClearItem.Description = "";
            this._btn_ClearItem.DisabledColor = System.Drawing.Color.DarkGray;
            this._btn_ClearItem.Dock = System.Windows.Forms.DockStyle.Fill;
            this._btn_ClearItem.EdgeRadius = 5;
            this._btn_ClearItem.GradientAngle = 70F;
            this._btn_ClearItem.GradientFirstColor = System.Drawing.Color.White;
            this._btn_ClearItem.GradientSecondColor = System.Drawing.Color.LightSlateGray;
            this._btn_ClearItem.HoverEmphasizeCustomColor = System.Drawing.Color.White;
            this._btn_ClearItem.ImagePosition = new System.Drawing.Point(10, 10);
            this._btn_ClearItem.ImageSize = new System.Drawing.Point(30, 30);
            this._btn_ClearItem.LoadImage = global::FrameOfSystem3.Properties.Resources.CLEAR_BLACK;
            this._btn_ClearItem.Location = new System.Drawing.Point(948, 14);
            this._btn_ClearItem.MainFont = new System.Drawing.Font("맑은 고딕", 14F, System.Drawing.FontStyle.Bold);
            this._btn_ClearItem.MainFontColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(36)))), ((int)(((byte)(0)))));
            this._btn_ClearItem.Name = "_btn_ClearItem";
            this._btn_ClearItem.Size = new System.Drawing.Size(162, 98);
            this._btn_ClearItem.SubFont = new System.Drawing.Font("맑은 고딕", 8F, System.Drawing.FontStyle.Bold);
            this._btn_ClearItem.SubFontColor = System.Drawing.Color.DarkBlue;
            this._btn_ClearItem.SubText = "STATUS";
            this._btn_ClearItem.TabIndex = 9;
            this._btn_ClearItem.Text = "CLEAR ITEM";
            this._btn_ClearItem.TextAlignMain = Sys3Controls.EN_TEXTALIGN.MIDDLE_RIGHT;
            this._btn_ClearItem.TextAlignSub = Sys3Controls.EN_TEXTALIGN.TOP_LEFT;
            this._btn_ClearItem.ThemeIndex = 0;
            this._btn_ClearItem.UseBorder = true;
            this._btn_ClearItem.UseClickedEmphasizeTextColor = false;
            this._btn_ClearItem.UseCustomizeClickedColor = false;
            this._btn_ClearItem.UseEdge = true;
            this._btn_ClearItem.UseHoverEmphasizeCustomColor = false;
            this._btn_ClearItem.UseImage = false;
            this._btn_ClearItem.UserHoverEmpahsize = false;
            this._btn_ClearItem.UseSubFont = false;
            this._btn_ClearItem.Click += new System.EventHandler(this.Click_ClearItem);
            // 
            // _groupBox_DeviceItem
            // 
            this._groupBox_DeviceItem.BackGroundColor = System.Drawing.Color.WhiteSmoke;
            this._groupBox_DeviceItem.Controls.Add(this._tableLayoutPanel_DeviceItem);
            this._groupBox_DeviceItem.Dock = System.Windows.Forms.DockStyle.Fill;
            this._groupBox_DeviceItem.EdgeBorderStroke = 2;
            this._groupBox_DeviceItem.EdgeRadius = 2;
            this._groupBox_DeviceItem.LabelFont = new System.Drawing.Font("맑은 고딕", 10F, System.Drawing.FontStyle.Bold);
            this._groupBox_DeviceItem.LabelGradientColorFirst = System.Drawing.Color.FromArgb(((int)(((byte)(208)))), ((int)(((byte)(208)))), ((int)(((byte)(208)))));
            this._groupBox_DeviceItem.LabelGradientColorSecond = System.Drawing.Color.FromArgb(((int)(((byte)(240)))), ((int)(((byte)(240)))), ((int)(((byte)(240)))));
            this._groupBox_DeviceItem.LabelHeight = 30;
            this._groupBox_DeviceItem.LabelTextColor = System.Drawing.Color.Black;
            this._groupBox_DeviceItem.Location = new System.Drawing.Point(0, 737);
            this._groupBox_DeviceItem.Margin = new System.Windows.Forms.Padding(0);
            this._groupBox_DeviceItem.Name = "_groupBox_DeviceItem";
            this._groupBox_DeviceItem.Padding = new System.Windows.Forms.Padding(3, 18, 3, 3);
            this._groupBox_DeviceItem.Size = new System.Drawing.Size(1140, 163);
            this._groupBox_DeviceItem.TabIndex = 1354;
            this._groupBox_DeviceItem.TabStop = false;
            this._groupBox_DeviceItem.Text = "DEVICE ITEM";
            this._groupBox_DeviceItem.TextAlign = Sys3Controls.EN_TEXTALIGN.MIDDLE_CENTER;
            this._groupBox_DeviceItem.ThemeIndex = 0;
            this._groupBox_DeviceItem.UseLabelBorder = true;
            this._groupBox_DeviceItem.UseTitle = true;
            // 
            // _tableLayoutPanel_DeviceItem
            // 
            this._tableLayoutPanel_DeviceItem.BackColor = System.Drawing.Color.WhiteSmoke;
            this._tableLayoutPanel_DeviceItem.ColumnCount = 9;
            this._tableLayoutPanel_DeviceItem.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 1.454736F));
            this._tableLayoutPanel_DeviceItem.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 14.8608F));
            this._tableLayoutPanel_DeviceItem.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 14.8608F));
            this._tableLayoutPanel_DeviceItem.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 14.8608F));
            this._tableLayoutPanel_DeviceItem.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 14.8608F));
            this._tableLayoutPanel_DeviceItem.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 2.972159F));
            this._tableLayoutPanel_DeviceItem.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 19.81439F));
            this._tableLayoutPanel_DeviceItem.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 14.8608F));
            this._tableLayoutPanel_DeviceItem.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 1.454737F));
            this._tableLayoutPanel_DeviceItem.Controls.Add(this.m_btnAdd1, 1, 1);
            this._tableLayoutPanel_DeviceItem.Controls.Add(this._tableLayoutPanel_Tracking, 6, 1);
            this._tableLayoutPanel_DeviceItem.Controls.Add(this.m_btnAdd3, 2, 1);
            this._tableLayoutPanel_DeviceItem.Controls.Add(this.m_btnAdd2, 3, 1);
            this._tableLayoutPanel_DeviceItem.Controls.Add(this.m_btnAdd4, 4, 1);
            this._tableLayoutPanel_DeviceItem.Controls.Add(this._btn_ClearItem, 7, 1);
            this._tableLayoutPanel_DeviceItem.Dock = System.Windows.Forms.DockStyle.Fill;
            this._tableLayoutPanel_DeviceItem.Location = new System.Drawing.Point(3, 32);
            this._tableLayoutPanel_DeviceItem.Name = "_tableLayoutPanel_DeviceItem";
            this._tableLayoutPanel_DeviceItem.RowCount = 3;
            this._tableLayoutPanel_DeviceItem.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 9.016394F));
            this._tableLayoutPanel_DeviceItem.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 81.96721F));
            this._tableLayoutPanel_DeviceItem.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 9.016394F));
            this._tableLayoutPanel_DeviceItem.Size = new System.Drawing.Size(1134, 128);
            this._tableLayoutPanel_DeviceItem.TabIndex = 0;
            // 
            // _tableLayoutPanel_Tracking
            // 
            this._tableLayoutPanel_Tracking.BackColor = System.Drawing.Color.Transparent;
            this._tableLayoutPanel_Tracking.ColumnCount = 2;
            this._tableLayoutPanel_Tracking.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this._tableLayoutPanel_Tracking.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this._tableLayoutPanel_Tracking.Controls.Add(this._panel_TrackingSwitch, 1, 0);
            this._tableLayoutPanel_Tracking.Controls.Add(this.m_labelStart, 0, 0);
            this._tableLayoutPanel_Tracking.Dock = System.Windows.Forms.DockStyle.Fill;
            this._tableLayoutPanel_Tracking.Location = new System.Drawing.Point(721, 11);
            this._tableLayoutPanel_Tracking.Margin = new System.Windows.Forms.Padding(0);
            this._tableLayoutPanel_Tracking.Name = "_tableLayoutPanel_Tracking";
            this._tableLayoutPanel_Tracking.RowCount = 1;
            this._tableLayoutPanel_Tracking.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this._tableLayoutPanel_Tracking.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this._tableLayoutPanel_Tracking.Size = new System.Drawing.Size(224, 104);
            this._tableLayoutPanel_Tracking.TabIndex = 1357;
            // 
            // m_labelStart
            // 
            this.m_labelStart.BackGroundColor = System.Drawing.Color.WhiteSmoke;
            this.m_labelStart.BorderStroke = 0;
            this.m_labelStart.BorderStyle = System.Windows.Forms.ButtonBorderStyle.Solid;
            this.m_labelStart.Description = "";
            this.m_labelStart.DisabledColor = System.Drawing.Color.Silver;
            this.m_labelStart.Dock = System.Windows.Forms.DockStyle.Fill;
            this.m_labelStart.EdgeRadius = 1;
            this.m_labelStart.ImagePosition = new System.Drawing.Point(0, 0);
            this.m_labelStart.ImageSize = new System.Drawing.Point(0, 0);
            this.m_labelStart.LoadImage = null;
            this.m_labelStart.Location = new System.Drawing.Point(3, 3);
            this.m_labelStart.MainFont = new System.Drawing.Font("맑은 고딕", 14F, System.Drawing.FontStyle.Bold);
            this.m_labelStart.MainFontColor = System.Drawing.Color.Black;
            this.m_labelStart.Name = "m_labelStart";
            this.m_labelStart.Size = new System.Drawing.Size(106, 98);
            this.m_labelStart.SubFont = new System.Drawing.Font("맑은 고딕", 10F);
            this.m_labelStart.SubFontColor = System.Drawing.Color.Black;
            this.m_labelStart.SubText = "";
            this.m_labelStart.TabIndex = 1356;
            this.m_labelStart.Text = "OFF / ON";
            this.m_labelStart.TextAlignMain = Sys3Controls.EN_TEXTALIGN.MIDDLE_CENTER;
            this.m_labelStart.TextAlignSub = Sys3Controls.EN_TEXTALIGN.MIDDLE_CENTER;
            this.m_labelStart.TextAlignUnit = Sys3Controls.EN_TEXTALIGN.MIDDLE_CENTER;
            this.m_labelStart.ThemeIndex = 0;
            this.m_labelStart.UnitAreaRate = 40;
            this.m_labelStart.UnitFont = new System.Drawing.Font("맑은 고딕", 10F, System.Drawing.FontStyle.Bold);
            this.m_labelStart.UnitFontColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.m_labelStart.UnitPositionVertical = false;
            this.m_labelStart.UnitText = "";
            this.m_labelStart.UseBorder = true;
            this.m_labelStart.UseEdgeRadius = false;
            this.m_labelStart.UseImage = false;
            this.m_labelStart.UseSubFont = false;
            this.m_labelStart.UseUnitFont = false;
            // 
            // m_ToggleInputOnDelay
            // 
            this.m_ToggleInputOnDelay.Active = false;
            this.m_ToggleInputOnDelay.ActiveColorFirst = System.Drawing.Color.RoyalBlue;
            this.m_ToggleInputOnDelay.ActiveColorSecond = System.Drawing.Color.DodgerBlue;
            this.m_ToggleInputOnDelay.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.m_ToggleInputOnDelay.BackColor = System.Drawing.Color.WhiteSmoke;
            this.m_ToggleInputOnDelay.Location = new System.Drawing.Point(6, 26);
            this.m_ToggleInputOnDelay.Margin = new System.Windows.Forms.Padding(0);
            this.m_ToggleInputOnDelay.Name = "m_ToggleInputOnDelay";
            this.m_ToggleInputOnDelay.NormalColorFirst = System.Drawing.Color.DarkGray;
            this.m_ToggleInputOnDelay.NormalColorSecond = System.Drawing.Color.Silver;
            this.m_ToggleInputOnDelay.Size = new System.Drawing.Size(98, 49);
            this.m_ToggleInputOnDelay.TabIndex = 1355;
            this.m_ToggleInputOnDelay.Click += new System.EventHandler(this.Click_ToggleButton);
            this.m_ToggleInputOnDelay.DoubleClick += new System.EventHandler(this.Click_ToggleButton);
            // 
            // _tableLayoutPanel_Main
            // 
            this._tableLayoutPanel_Main.ColumnCount = 1;
            this._tableLayoutPanel_Main.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this._tableLayoutPanel_Main.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this._tableLayoutPanel_Main.Controls.Add(this._groupBox_DeviceView, 0, 0);
            this._tableLayoutPanel_Main.Controls.Add(this._groupBox_DeviceItem, 0, 1);
            this._tableLayoutPanel_Main.Dock = System.Windows.Forms.DockStyle.Fill;
            this._tableLayoutPanel_Main.Location = new System.Drawing.Point(0, 0);
            this._tableLayoutPanel_Main.Margin = new System.Windows.Forms.Padding(0);
            this._tableLayoutPanel_Main.Name = "_tableLayoutPanel_Main";
            this._tableLayoutPanel_Main.RowCount = 2;
            this._tableLayoutPanel_Main.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 81.88889F));
            this._tableLayoutPanel_Main.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 18.11111F));
            this._tableLayoutPanel_Main.Size = new System.Drawing.Size(1140, 900);
            this._tableLayoutPanel_Main.TabIndex = 1359;
            // 
            // _panel_TrackingSwitch
            // 
            this._panel_TrackingSwitch.Controls.Add(this.m_ToggleInputOnDelay);
            this._panel_TrackingSwitch.Dock = System.Windows.Forms.DockStyle.Fill;
            this._panel_TrackingSwitch.Location = new System.Drawing.Point(112, 0);
            this._panel_TrackingSwitch.Margin = new System.Windows.Forms.Padding(0);
            this._panel_TrackingSwitch.Name = "_panel_TrackingSwitch";
            this._panel_TrackingSwitch.Size = new System.Drawing.Size(112, 104);
            this._panel_TrackingSwitch.TabIndex = 0;
            // 
            // Operation_Tracking
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.Controls.Add(this._tableLayoutPanel_Main);
            this.Margin = new System.Windows.Forms.Padding(0);
            this.Name = "Operation_Tracking";
            this.Size = new System.Drawing.Size(1140, 900);
            this._groupBox_DeviceView.ResumeLayout(false);
            this._tableLayoutPanel_DeviceView.ResumeLayout(false);
            this._tableLayoutPanel_DeviceViewRight.ResumeLayout(false);
            this._groupBox_DeviceItem.ResumeLayout(false);
            this._tableLayoutPanel_DeviceItem.ResumeLayout(false);
            this._tableLayoutPanel_Tracking.ResumeLayout(false);
            this._tableLayoutPanel_Main.ResumeLayout(false);
            this._panel_TrackingSwitch.ResumeLayout(false);
            this.ResumeLayout(false);

		}

		#endregion

		private ZedGraph.ZedGraphControl zedGraphControl1;
		private Sys3Controls.Sys3button m_btnAdd1;
        private Sys3Controls.Sys3GroupBoxContainer _groupBox_DeviceView;
		private Sys3Controls.Sys3button m_btnAdd3;
		private Sys3Controls.Sys3button m_btnAdd4;
		private Sys3Controls.Sys3button m_btnAdd2;
		private Sys3Controls.Sys3button _btn_ClearItem;
		private Sys3Controls.Sys3GroupBoxContainer _groupBox_DeviceItem;
		private Sys3Controls.Sys3ToggleButton m_ToggleInputOnDelay;
        private Sys3Controls.Sys3Label m_labelStart;
		private Sys3Controls.Sys3Label _label_Cylinder;
		private Sys3Controls.Sys3Label _label_Analog;
		private Sys3Controls.Sys3Label _label_Digital;
		private Sys3Controls.Sys3Label _label_Motion;
        private System.Windows.Forms.TableLayoutPanel _tableLayoutPanel_Main;
        private System.Windows.Forms.TableLayoutPanel _tableLayoutPanel_DeviceView;
        private System.Windows.Forms.TableLayoutPanel _tableLayoutPanel_DeviceItem;
        private System.Windows.Forms.TableLayoutPanel _tableLayoutPanel_Tracking;
        private System.Windows.Forms.TableLayoutPanel _tableLayoutPanel_DeviceViewRight;
        private System.Windows.Forms.Panel _panel_TrackingSwitch;
	}
}
