namespace FrameOfSystem3.Views.Setup
{
	partial class Setup_FanFilterUnit
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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle3 = new System.Windows.Forms.DataGridViewCellStyle();
            this.gvFanFilterUnitStatus = new Sys3Controls.Sys3DoubleBufferedDataGridView();
            this.sys3Label7 = new Sys3Controls.Sys3Label();
            this.sys3Label4 = new Sys3Controls.Sys3Label();
            this.btnSetSpeedAll = new Sys3Controls.Sys3button();
            this.btnStop = new Sys3Controls.Sys3button();
            this.tableLayoutPanel8 = new System.Windows.Forms.TableLayoutPanel();
            this.sys3Label1 = new Sys3Controls.Sys3Label();
            this.sys3Label9 = new Sys3Controls.Sys3Label();
            this.sys3Label10 = new Sys3Controls.Sys3Label();
            this.tableLayoutPanel7 = new System.Windows.Forms.TableLayoutPanel();
            this.lbl_FFU_Speed = new Sys3Controls.Sys3Label();
            this.lbl_FFU_Pressure = new Sys3Controls.Sys3Label();
            this.lbl_FFU_AlarmCode = new Sys3Controls.Sys3Label();
            this.toggleDifferentialPressureMode = new Sys3Controls.Sys3ToggleButton();
            this.toggleUseFFU = new Sys3Controls.Sys3ToggleButton();
            this.sys3Label5 = new Sys3Controls.Sys3Label();
            this.tableLayoutPanel6 = new System.Windows.Forms.TableLayoutPanel();
            this.lbl_FFU_UnitCount = new Sys3Controls.Sys3Label();
            this.tableLayoutPanel4 = new System.Windows.Forms.TableLayoutPanel();
            this.tableLayoutPanel9 = new System.Windows.Forms.TableLayoutPanel();
            this.tableLayoutPanel2 = new System.Windows.Forms.TableLayoutPanel();
            this.tableLayoutPanel5 = new System.Windows.Forms.TableLayoutPanel();
            this.tableLayoutPanel3 = new System.Windows.Forms.TableLayoutPanel();
            this.tableLayoutPanel10 = new System.Windows.Forms.TableLayoutPanel();
            this.btnSetPressure = new Sys3Controls.Sys3button();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.dataGridViewTextBoxColumn3 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn4 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.SettingSpeed = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.CurrentSpeed = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.CurrentDifferentialPressure = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Status = new System.Windows.Forms.DataGridViewTextBoxColumn();
            ((System.ComponentModel.ISupportInitialize)(this.gvFanFilterUnitStatus)).BeginInit();
            this.tableLayoutPanel8.SuspendLayout();
            this.tableLayoutPanel7.SuspendLayout();
            this.tableLayoutPanel6.SuspendLayout();
            this.tableLayoutPanel4.SuspendLayout();
            this.tableLayoutPanel9.SuspendLayout();
            this.tableLayoutPanel5.SuspendLayout();
            this.tableLayoutPanel3.SuspendLayout();
            this.tableLayoutPanel10.SuspendLayout();
            this.tableLayoutPanel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // gvFanFilterUnitStatus
            // 
            this.gvFanFilterUnitStatus.AllowUserToAddRows = false;
            this.gvFanFilterUnitStatus.AllowUserToDeleteRows = false;
            this.gvFanFilterUnitStatus.AllowUserToResizeColumns = false;
            this.gvFanFilterUnitStatus.AllowUserToResizeRows = false;
            this.gvFanFilterUnitStatus.BackgroundColor = System.Drawing.Color.White;
            this.gvFanFilterUnitStatus.CellBorderStyle = System.Windows.Forms.DataGridViewCellBorderStyle.Raised;
            this.gvFanFilterUnitStatus.ColumnHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.Single;
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle1.BackColor = System.Drawing.Color.LightGray;
            dataGridViewCellStyle1.Font = new System.Drawing.Font("맑은 고딕", 11F, System.Drawing.FontStyle.Bold);
            dataGridViewCellStyle1.ForeColor = System.Drawing.Color.Black;
            dataGridViewCellStyle1.SelectionBackColor = System.Drawing.Color.White;
            dataGridViewCellStyle1.SelectionForeColor = System.Drawing.Color.Black;
            dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.gvFanFilterUnitStatus.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
            this.gvFanFilterUnitStatus.ColumnHeadersHeight = 30;
            this.gvFanFilterUnitStatus.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            this.gvFanFilterUnitStatus.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.dataGridViewTextBoxColumn3,
            this.dataGridViewTextBoxColumn4,
            this.SettingSpeed,
            this.CurrentSpeed,
            this.CurrentDifferentialPressure,
            this.Status});
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle2.BackColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle2.Font = new System.Drawing.Font("맑은 고딕", 10F, System.Drawing.FontStyle.Bold);
            dataGridViewCellStyle2.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle2.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(120)))), ((int)(((byte)(130)))), ((int)(((byte)(150)))));
            dataGridViewCellStyle2.SelectionForeColor = System.Drawing.Color.White;
            dataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.gvFanFilterUnitStatus.DefaultCellStyle = dataGridViewCellStyle2;
            this.gvFanFilterUnitStatus.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gvFanFilterUnitStatus.EnableHeadersVisualStyles = false;
            this.gvFanFilterUnitStatus.GridColor = System.Drawing.Color.FromArgb(((int)(((byte)(120)))), ((int)(((byte)(130)))), ((int)(((byte)(150)))));
            this.gvFanFilterUnitStatus.Location = new System.Drawing.Point(2, 182);
            this.gvFanFilterUnitStatus.Margin = new System.Windows.Forms.Padding(2, 2, 0, 0);
            this.gvFanFilterUnitStatus.MultiSelect = false;
            this.gvFanFilterUnitStatus.Name = "gvFanFilterUnitStatus";
            this.gvFanFilterUnitStatus.ReadOnly = true;
            this.gvFanFilterUnitStatus.RowHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.None;
            dataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle3.BackColor = System.Drawing.Color.White;
            dataGridViewCellStyle3.Font = new System.Drawing.Font("맑은 고딕", 11F);
            dataGridViewCellStyle3.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle3.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(120)))), ((int)(((byte)(130)))), ((int)(((byte)(150)))));
            dataGridViewCellStyle3.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle3.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.gvFanFilterUnitStatus.RowHeadersDefaultCellStyle = dataGridViewCellStyle3;
            this.gvFanFilterUnitStatus.RowHeadersVisible = false;
            this.gvFanFilterUnitStatus.RowHeadersWidthSizeMode = System.Windows.Forms.DataGridViewRowHeadersWidthSizeMode.DisableResizing;
            this.gvFanFilterUnitStatus.RowTemplate.Height = 23;
            this.gvFanFilterUnitStatus.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.gvFanFilterUnitStatus.Size = new System.Drawing.Size(1138, 268);
            this.gvFanFilterUnitStatus.TabIndex = 21130;
            // 
            // sys3Label7
            // 
            this.sys3Label7.BackGroundColor = System.Drawing.Color.DarkGray;
            this.sys3Label7.BorderStroke = 2;
            this.sys3Label7.BorderStyle = System.Windows.Forms.ButtonBorderStyle.Solid;
            this.sys3Label7.Description = "";
            this.sys3Label7.DisabledColor = System.Drawing.Color.DarkGray;
            this.sys3Label7.Dock = System.Windows.Forms.DockStyle.Fill;
            this.sys3Label7.EdgeRadius = 1;
            this.sys3Label7.ImagePosition = new System.Drawing.Point(0, 0);
            this.sys3Label7.ImageSize = new System.Drawing.Point(0, 0);
            this.sys3Label7.LoadImage = null;
            this.sys3Label7.Location = new System.Drawing.Point(1, 1);
            this.sys3Label7.MainFont = new System.Drawing.Font("맑은 고딕", 11F, System.Drawing.FontStyle.Bold);
            this.sys3Label7.MainFontColor = System.Drawing.Color.Black;
            this.sys3Label7.Margin = new System.Windows.Forms.Padding(1, 1, 0, 0);
            this.sys3Label7.Name = "sys3Label7";
            this.sys3Label7.Size = new System.Drawing.Size(287, 43);
            this.sys3Label7.SubFont = new System.Drawing.Font("맑은 고딕", 10F, System.Drawing.FontStyle.Bold);
            this.sys3Label7.SubFontColor = System.Drawing.Color.Gray;
            this.sys3Label7.SubText = "";
            this.sys3Label7.TabIndex = 20602;
            this.sys3Label7.Text = "ALARM CODE";
            this.sys3Label7.TextAlignMain = Sys3Controls.EN_TEXTALIGN.MIDDLE_RIGHT;
            this.sys3Label7.TextAlignSub = Sys3Controls.EN_TEXTALIGN.TOP_LEFT;
            this.sys3Label7.TextAlignUnit = Sys3Controls.EN_TEXTALIGN.MIDDLE_CENTER;
            this.sys3Label7.ThemeIndex = 0;
            this.sys3Label7.UnitAreaRate = 30;
            this.sys3Label7.UnitFont = new System.Drawing.Font("맑은 고딕", 10F, System.Drawing.FontStyle.Bold);
            this.sys3Label7.UnitFontColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.sys3Label7.UnitPositionVertical = false;
            this.sys3Label7.UnitText = "";
            this.sys3Label7.UseBorder = true;
            this.sys3Label7.UseEdgeRadius = false;
            this.sys3Label7.UseImage = false;
            this.sys3Label7.UseSubFont = true;
            this.sys3Label7.UseUnitFont = false;
            // 
            // sys3Label4
            // 
            this.sys3Label4.BackGroundColor = System.Drawing.Color.DarkGray;
            this.sys3Label4.BorderStroke = 2;
            this.sys3Label4.BorderStyle = System.Windows.Forms.ButtonBorderStyle.Solid;
            this.sys3Label4.Description = "";
            this.sys3Label4.DisabledColor = System.Drawing.Color.DarkGray;
            this.sys3Label4.Dock = System.Windows.Forms.DockStyle.Fill;
            this.sys3Label4.EdgeRadius = 1;
            this.sys3Label4.ImagePosition = new System.Drawing.Point(0, 0);
            this.sys3Label4.ImageSize = new System.Drawing.Point(0, 0);
            this.sys3Label4.LoadImage = null;
            this.sys3Label4.Location = new System.Drawing.Point(1, 1);
            this.sys3Label4.MainFont = new System.Drawing.Font("맑은 고딕", 11F, System.Drawing.FontStyle.Bold);
            this.sys3Label4.MainFontColor = System.Drawing.Color.Black;
            this.sys3Label4.Margin = new System.Windows.Forms.Padding(1, 1, 0, 0);
            this.sys3Label4.Name = "sys3Label4";
            this.sys3Label4.Size = new System.Drawing.Size(191, 43);
            this.sys3Label4.SubFont = new System.Drawing.Font("맑은 고딕", 10F, System.Drawing.FontStyle.Bold);
            this.sys3Label4.SubFontColor = System.Drawing.Color.Gray;
            this.sys3Label4.SubText = "";
            this.sys3Label4.TabIndex = 20600;
            this.sys3Label4.Text = "SPEED (RPM)";
            this.sys3Label4.TextAlignMain = Sys3Controls.EN_TEXTALIGN.MIDDLE_RIGHT;
            this.sys3Label4.TextAlignSub = Sys3Controls.EN_TEXTALIGN.TOP_LEFT;
            this.sys3Label4.TextAlignUnit = Sys3Controls.EN_TEXTALIGN.MIDDLE_CENTER;
            this.sys3Label4.ThemeIndex = 0;
            this.sys3Label4.UnitAreaRate = 30;
            this.sys3Label4.UnitFont = new System.Drawing.Font("맑은 고딕", 10F, System.Drawing.FontStyle.Bold);
            this.sys3Label4.UnitFontColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.sys3Label4.UnitPositionVertical = false;
            this.sys3Label4.UnitText = "";
            this.sys3Label4.UseBorder = true;
            this.sys3Label4.UseEdgeRadius = false;
            this.sys3Label4.UseImage = false;
            this.sys3Label4.UseSubFont = true;
            this.sys3Label4.UseUnitFont = false;
            // 
            // btnSetSpeedAll
            // 
            this.btnSetSpeedAll.BorderWidth = 2;
            this.btnSetSpeedAll.ButtonClicked = false;
            this.btnSetSpeedAll.ClickedEmphasizeTextColor = System.Drawing.Color.White;
            this.btnSetSpeedAll.CustomClickedGradientFirstColor = System.Drawing.Color.BlanchedAlmond;
            this.btnSetSpeedAll.CustomClickedGradientSecondColor = System.Drawing.Color.Gold;
            this.btnSetSpeedAll.Description = "";
            this.btnSetSpeedAll.DisabledColor = System.Drawing.Color.DarkGray;
            this.btnSetSpeedAll.EdgeRadius = 5;
            this.btnSetSpeedAll.GradientAngle = 60F;
            this.btnSetSpeedAll.GradientFirstColor = System.Drawing.Color.Silver;
            this.btnSetSpeedAll.GradientSecondColor = System.Drawing.Color.Gray;
            this.btnSetSpeedAll.HoverEmphasizeCustomColor = System.Drawing.Color.Firebrick;
            this.btnSetSpeedAll.ImagePosition = new System.Drawing.Point(7, 7);
            this.btnSetSpeedAll.ImageSize = new System.Drawing.Point(30, 30);
            this.btnSetSpeedAll.LoadImage = null;
            this.btnSetSpeedAll.Location = new System.Drawing.Point(1, 1);
            this.btnSetSpeedAll.MainFont = new System.Drawing.Font("맑은 고딕", 12F, System.Drawing.FontStyle.Bold);
            this.btnSetSpeedAll.MainFontColor = System.Drawing.Color.SpringGreen;
            this.btnSetSpeedAll.Margin = new System.Windows.Forms.Padding(1);
            this.btnSetSpeedAll.Name = "btnSetSpeedAll";
            this.btnSetSpeedAll.Size = new System.Drawing.Size(89, 42);
            this.btnSetSpeedAll.SubFont = new System.Drawing.Font("맑은 고딕", 8F, System.Drawing.FontStyle.Bold);
            this.btnSetSpeedAll.SubFontColor = System.Drawing.Color.DarkBlue;
            this.btnSetSpeedAll.SubText = "";
            this.btnSetSpeedAll.TabIndex = 20627;
            this.btnSetSpeedAll.Text = "SET";
            this.btnSetSpeedAll.TextAlignMain = Sys3Controls.EN_TEXTALIGN.MIDDLE_CENTER;
            this.btnSetSpeedAll.TextAlignSub = Sys3Controls.EN_TEXTALIGN.TOP_LEFT;
            this.btnSetSpeedAll.ThemeIndex = 0;
            this.btnSetSpeedAll.UseBorder = true;
            this.btnSetSpeedAll.UseClickedEmphasizeTextColor = false;
            this.btnSetSpeedAll.UseCustomizeClickedColor = true;
            this.btnSetSpeedAll.UseEdge = true;
            this.btnSetSpeedAll.UseHoverEmphasizeCustomColor = true;
            this.btnSetSpeedAll.UseImage = true;
            this.btnSetSpeedAll.UserHoverEmpahsize = true;
            this.btnSetSpeedAll.UseSubFont = false;
            this.btnSetSpeedAll.Click += new System.EventHandler(this.Click_SetTargetAll);
            // 
            // btnStop
            // 
            this.btnStop.BorderWidth = 2;
            this.btnStop.ButtonClicked = false;
            this.btnStop.ClickedEmphasizeTextColor = System.Drawing.Color.White;
            this.btnStop.CustomClickedGradientFirstColor = System.Drawing.Color.BlanchedAlmond;
            this.btnStop.CustomClickedGradientSecondColor = System.Drawing.Color.Gold;
            this.btnStop.Description = "";
            this.btnStop.DisabledColor = System.Drawing.Color.DarkGray;
            this.btnStop.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnStop.EdgeRadius = 5;
            this.btnStop.GradientAngle = 60F;
            this.btnStop.GradientFirstColor = System.Drawing.Color.Silver;
            this.btnStop.GradientSecondColor = System.Drawing.Color.Gray;
            this.btnStop.HoverEmphasizeCustomColor = System.Drawing.Color.Firebrick;
            this.btnStop.ImagePosition = new System.Drawing.Point(7, 7);
            this.btnStop.ImageSize = new System.Drawing.Point(30, 30);
            this.btnStop.LoadImage = null;
            this.btnStop.Location = new System.Drawing.Point(569, 1);
            this.btnStop.MainFont = new System.Drawing.Font("맑은 고딕", 12F, System.Drawing.FontStyle.Bold);
            this.btnStop.MainFontColor = System.Drawing.Color.Crimson;
            this.btnStop.Margin = new System.Windows.Forms.Padding(1);
            this.btnStop.Name = "btnStop";
            this.btnStop.Size = new System.Drawing.Size(89, 87);
            this.btnStop.SubFont = new System.Drawing.Font("맑은 고딕", 8F, System.Drawing.FontStyle.Bold);
            this.btnStop.SubFontColor = System.Drawing.Color.DarkBlue;
            this.btnStop.SubText = "";
            this.btnStop.TabIndex = 20632;
            this.btnStop.Text = "STOP";
            this.btnStop.TextAlignMain = Sys3Controls.EN_TEXTALIGN.MIDDLE_CENTER;
            this.btnStop.TextAlignSub = Sys3Controls.EN_TEXTALIGN.TOP_LEFT;
            this.btnStop.ThemeIndex = 0;
            this.btnStop.UseBorder = true;
            this.btnStop.UseClickedEmphasizeTextColor = false;
            this.btnStop.UseCustomizeClickedColor = true;
            this.btnStop.UseEdge = true;
            this.btnStop.UseHoverEmphasizeCustomColor = true;
            this.btnStop.UseImage = true;
            this.btnStop.UserHoverEmpahsize = true;
            this.btnStop.UseSubFont = false;
            this.btnStop.Click += new System.EventHandler(this.Click_Stop);
            // 
            // tableLayoutPanel8
            // 
            this.tableLayoutPanel8.ColumnCount = 1;
            this.tableLayoutPanel8.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel8.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel8.Controls.Add(this.sys3Label4, 0, 0);
            this.tableLayoutPanel8.Controls.Add(this.sys3Label1, 0, 1);
            this.tableLayoutPanel8.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel8.Location = new System.Drawing.Point(285, 1);
            this.tableLayoutPanel8.Margin = new System.Windows.Forms.Padding(1, 1, 0, 0);
            this.tableLayoutPanel8.Name = "tableLayoutPanel8";
            this.tableLayoutPanel8.RowCount = 2;
            this.tableLayoutPanel8.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel8.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel8.Size = new System.Drawing.Size(192, 88);
            this.tableLayoutPanel8.TabIndex = 3;
            // 
            // sys3Label1
            // 
            this.sys3Label1.BackGroundColor = System.Drawing.Color.DarkGray;
            this.sys3Label1.BorderStroke = 2;
            this.sys3Label1.BorderStyle = System.Windows.Forms.ButtonBorderStyle.Solid;
            this.sys3Label1.Description = "";
            this.sys3Label1.DisabledColor = System.Drawing.Color.DarkGray;
            this.sys3Label1.EdgeRadius = 1;
            this.sys3Label1.ImagePosition = new System.Drawing.Point(0, 0);
            this.sys3Label1.ImageSize = new System.Drawing.Point(0, 0);
            this.sys3Label1.LoadImage = null;
            this.sys3Label1.Location = new System.Drawing.Point(1, 45);
            this.sys3Label1.MainFont = new System.Drawing.Font("맑은 고딕", 11F, System.Drawing.FontStyle.Bold);
            this.sys3Label1.MainFontColor = System.Drawing.Color.Black;
            this.sys3Label1.Margin = new System.Windows.Forms.Padding(1, 1, 0, 0);
            this.sys3Label1.Name = "sys3Label1";
            this.sys3Label1.Size = new System.Drawing.Size(191, 43);
            this.sys3Label1.SubFont = new System.Drawing.Font("맑은 고딕", 10F, System.Drawing.FontStyle.Bold);
            this.sys3Label1.SubFontColor = System.Drawing.Color.Gray;
            this.sys3Label1.SubText = "";
            this.sys3Label1.TabIndex = 20603;
            this.sys3Label1.Text = "DIFFERENTIAL PRESSURE (mmAq)";
            this.sys3Label1.TextAlignMain = Sys3Controls.EN_TEXTALIGN.MIDDLE_RIGHT;
            this.sys3Label1.TextAlignSub = Sys3Controls.EN_TEXTALIGN.TOP_LEFT;
            this.sys3Label1.TextAlignUnit = Sys3Controls.EN_TEXTALIGN.MIDDLE_CENTER;
            this.sys3Label1.ThemeIndex = 0;
            this.sys3Label1.UnitAreaRate = 30;
            this.sys3Label1.UnitFont = new System.Drawing.Font("맑은 고딕", 10F, System.Drawing.FontStyle.Bold);
            this.sys3Label1.UnitFontColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.sys3Label1.UnitPositionVertical = false;
            this.sys3Label1.UnitText = "";
            this.sys3Label1.UseBorder = true;
            this.sys3Label1.UseEdgeRadius = false;
            this.sys3Label1.UseImage = false;
            this.sys3Label1.UseSubFont = true;
            this.sys3Label1.UseUnitFont = false;
            // 
            // sys3Label9
            // 
            this.sys3Label9.BackGroundColor = System.Drawing.Color.DarkGray;
            this.sys3Label9.BorderStroke = 2;
            this.sys3Label9.BorderStyle = System.Windows.Forms.ButtonBorderStyle.Solid;
            this.sys3Label9.Description = "";
            this.sys3Label9.DisabledColor = System.Drawing.Color.DarkGray;
            this.sys3Label9.Dock = System.Windows.Forms.DockStyle.Fill;
            this.sys3Label9.EdgeRadius = 1;
            this.sys3Label9.ImagePosition = new System.Drawing.Point(0, 0);
            this.sys3Label9.ImageSize = new System.Drawing.Point(0, 0);
            this.sys3Label9.LoadImage = null;
            this.sys3Label9.Location = new System.Drawing.Point(341, 1);
            this.sys3Label9.MainFont = new System.Drawing.Font("맑은 고딕", 11F, System.Drawing.FontStyle.Bold);
            this.sys3Label9.MainFontColor = System.Drawing.Color.Black;
            this.sys3Label9.Margin = new System.Windows.Forms.Padding(1, 1, 0, 0);
            this.sys3Label9.Name = "sys3Label9";
            this.sys3Label9.Size = new System.Drawing.Size(192, 42);
            this.sys3Label9.SubFont = new System.Drawing.Font("맑은 고딕", 10F, System.Drawing.FontStyle.Bold);
            this.sys3Label9.SubFontColor = System.Drawing.Color.Gray;
            this.sys3Label9.SubText = "";
            this.sys3Label9.TabIndex = 20638;
            this.sys3Label9.Text = "FFU UNIT COUNT";
            this.sys3Label9.TextAlignMain = Sys3Controls.EN_TEXTALIGN.MIDDLE_RIGHT;
            this.sys3Label9.TextAlignSub = Sys3Controls.EN_TEXTALIGN.TOP_LEFT;
            this.sys3Label9.TextAlignUnit = Sys3Controls.EN_TEXTALIGN.MIDDLE_CENTER;
            this.sys3Label9.ThemeIndex = 0;
            this.sys3Label9.UnitAreaRate = 30;
            this.sys3Label9.UnitFont = new System.Drawing.Font("맑은 고딕", 10F, System.Drawing.FontStyle.Bold);
            this.sys3Label9.UnitFontColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.sys3Label9.UnitPositionVertical = false;
            this.sys3Label9.UnitText = "";
            this.sys3Label9.UseBorder = true;
            this.sys3Label9.UseEdgeRadius = false;
            this.sys3Label9.UseImage = false;
            this.sys3Label9.UseSubFont = true;
            this.sys3Label9.UseUnitFont = false;
            // 
            // sys3Label10
            // 
            this.sys3Label10.BackGroundColor = System.Drawing.Color.DarkGray;
            this.sys3Label10.BorderStroke = 2;
            this.sys3Label10.BorderStyle = System.Windows.Forms.ButtonBorderStyle.Solid;
            this.sys3Label10.Description = "";
            this.sys3Label10.DisabledColor = System.Drawing.Color.DarkGray;
            this.sys3Label10.Dock = System.Windows.Forms.DockStyle.Fill;
            this.sys3Label10.EdgeRadius = 1;
            this.sys3Label10.ImagePosition = new System.Drawing.Point(0, 0);
            this.sys3Label10.ImageSize = new System.Drawing.Point(0, 0);
            this.sys3Label10.LoadImage = null;
            this.sys3Label10.Location = new System.Drawing.Point(681, 1);
            this.sys3Label10.MainFont = new System.Drawing.Font("맑은 고딕", 11F, System.Drawing.FontStyle.Bold);
            this.sys3Label10.MainFontColor = System.Drawing.Color.Black;
            this.sys3Label10.Margin = new System.Windows.Forms.Padding(1, 1, 0, 0);
            this.sys3Label10.Name = "sys3Label10";
            this.sys3Label10.Size = new System.Drawing.Size(374, 42);
            this.sys3Label10.SubFont = new System.Drawing.Font("맑은 고딕", 10F, System.Drawing.FontStyle.Bold);
            this.sys3Label10.SubFontColor = System.Drawing.Color.Gray;
            this.sys3Label10.SubText = "";
            this.sys3Label10.TabIndex = 20640;
            this.sys3Label10.Text = "Use Differential Pressure Mode ";
            this.sys3Label10.TextAlignMain = Sys3Controls.EN_TEXTALIGN.MIDDLE_RIGHT;
            this.sys3Label10.TextAlignSub = Sys3Controls.EN_TEXTALIGN.TOP_LEFT;
            this.sys3Label10.TextAlignUnit = Sys3Controls.EN_TEXTALIGN.MIDDLE_CENTER;
            this.sys3Label10.ThemeIndex = 0;
            this.sys3Label10.UnitAreaRate = 30;
            this.sys3Label10.UnitFont = new System.Drawing.Font("맑은 고딕", 10F, System.Drawing.FontStyle.Bold);
            this.sys3Label10.UnitFontColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.sys3Label10.UnitPositionVertical = false;
            this.sys3Label10.UnitText = "";
            this.sys3Label10.UseBorder = true;
            this.sys3Label10.UseEdgeRadius = false;
            this.sys3Label10.UseImage = false;
            this.sys3Label10.UseSubFont = true;
            this.sys3Label10.UseUnitFont = false;
            // 
            // tableLayoutPanel7
            // 
            this.tableLayoutPanel7.ColumnCount = 1;
            this.tableLayoutPanel7.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel7.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel7.Controls.Add(this.lbl_FFU_Speed, 0, 0);
            this.tableLayoutPanel7.Controls.Add(this.lbl_FFU_Pressure, 0, 1);
            this.tableLayoutPanel7.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel7.Location = new System.Drawing.Point(1, 1);
            this.tableLayoutPanel7.Margin = new System.Windows.Forms.Padding(1, 1, 0, 0);
            this.tableLayoutPanel7.Name = "tableLayoutPanel7";
            this.tableLayoutPanel7.RowCount = 2;
            this.tableLayoutPanel7.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel7.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel7.Size = new System.Drawing.Size(283, 88);
            this.tableLayoutPanel7.TabIndex = 2;
            // 
            // lbl_FFU_Speed
            // 
            this.lbl_FFU_Speed.BackGroundColor = System.Drawing.Color.White;
            this.lbl_FFU_Speed.BorderStroke = 2;
            this.lbl_FFU_Speed.BorderStyle = System.Windows.Forms.ButtonBorderStyle.Solid;
            this.lbl_FFU_Speed.Description = "";
            this.lbl_FFU_Speed.DisabledColor = System.Drawing.Color.LightGray;
            this.lbl_FFU_Speed.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lbl_FFU_Speed.EdgeRadius = 1;
            this.lbl_FFU_Speed.ImagePosition = new System.Drawing.Point(0, 0);
            this.lbl_FFU_Speed.ImageSize = new System.Drawing.Point(0, 0);
            this.lbl_FFU_Speed.LoadImage = null;
            this.lbl_FFU_Speed.Location = new System.Drawing.Point(1, 1);
            this.lbl_FFU_Speed.MainFont = new System.Drawing.Font("맑은 고딕", 11F, System.Drawing.FontStyle.Bold);
            this.lbl_FFU_Speed.MainFontColor = System.Drawing.Color.Black;
            this.lbl_FFU_Speed.Margin = new System.Windows.Forms.Padding(1);
            this.lbl_FFU_Speed.Name = "lbl_FFU_Speed";
            this.lbl_FFU_Speed.Size = new System.Drawing.Size(281, 42);
            this.lbl_FFU_Speed.SubFont = new System.Drawing.Font("맑은 고딕", 10F);
            this.lbl_FFU_Speed.SubFontColor = System.Drawing.Color.Black;
            this.lbl_FFU_Speed.SubText = "";
            this.lbl_FFU_Speed.TabIndex = 21131;
            this.lbl_FFU_Speed.Tag = "";
            this.lbl_FFU_Speed.TextAlignMain = Sys3Controls.EN_TEXTALIGN.BOTTOM_RIGHT;
            this.lbl_FFU_Speed.TextAlignSub = Sys3Controls.EN_TEXTALIGN.MIDDLE_CENTER;
            this.lbl_FFU_Speed.TextAlignUnit = Sys3Controls.EN_TEXTALIGN.MIDDLE_CENTER;
            this.lbl_FFU_Speed.ThemeIndex = 0;
            this.lbl_FFU_Speed.UnitAreaRate = 40;
            this.lbl_FFU_Speed.UnitFont = new System.Drawing.Font("맑은 고딕", 10F, System.Drawing.FontStyle.Bold);
            this.lbl_FFU_Speed.UnitFontColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.lbl_FFU_Speed.UnitPositionVertical = false;
            this.lbl_FFU_Speed.UnitText = "";
            this.lbl_FFU_Speed.UseBorder = true;
            this.lbl_FFU_Speed.UseEdgeRadius = false;
            this.lbl_FFU_Speed.UseImage = false;
            this.lbl_FFU_Speed.UseSubFont = false;
            this.lbl_FFU_Speed.UseUnitFont = false;
            this.lbl_FFU_Speed.Click += new System.EventHandler(this.Click_FullSpeed);
            // 
            // lbl_FFU_Pressure
            // 
            this.lbl_FFU_Pressure.BackGroundColor = System.Drawing.Color.White;
            this.lbl_FFU_Pressure.BorderStroke = 2;
            this.lbl_FFU_Pressure.BorderStyle = System.Windows.Forms.ButtonBorderStyle.Solid;
            this.lbl_FFU_Pressure.Description = "";
            this.lbl_FFU_Pressure.DisabledColor = System.Drawing.Color.LightGray;
            this.lbl_FFU_Pressure.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lbl_FFU_Pressure.EdgeRadius = 1;
            this.lbl_FFU_Pressure.ImagePosition = new System.Drawing.Point(0, 0);
            this.lbl_FFU_Pressure.ImageSize = new System.Drawing.Point(0, 0);
            this.lbl_FFU_Pressure.LoadImage = null;
            this.lbl_FFU_Pressure.Location = new System.Drawing.Point(1, 45);
            this.lbl_FFU_Pressure.MainFont = new System.Drawing.Font("맑은 고딕", 11F, System.Drawing.FontStyle.Bold);
            this.lbl_FFU_Pressure.MainFontColor = System.Drawing.Color.Black;
            this.lbl_FFU_Pressure.Margin = new System.Windows.Forms.Padding(1);
            this.lbl_FFU_Pressure.Name = "lbl_FFU_Pressure";
            this.lbl_FFU_Pressure.Size = new System.Drawing.Size(281, 42);
            this.lbl_FFU_Pressure.SubFont = new System.Drawing.Font("맑은 고딕", 10F);
            this.lbl_FFU_Pressure.SubFontColor = System.Drawing.Color.Black;
            this.lbl_FFU_Pressure.SubText = "";
            this.lbl_FFU_Pressure.TabIndex = 21133;
            this.lbl_FFU_Pressure.Tag = "";
            this.lbl_FFU_Pressure.TextAlignMain = Sys3Controls.EN_TEXTALIGN.BOTTOM_RIGHT;
            this.lbl_FFU_Pressure.TextAlignSub = Sys3Controls.EN_TEXTALIGN.MIDDLE_CENTER;
            this.lbl_FFU_Pressure.TextAlignUnit = Sys3Controls.EN_TEXTALIGN.MIDDLE_CENTER;
            this.lbl_FFU_Pressure.ThemeIndex = 0;
            this.lbl_FFU_Pressure.UnitAreaRate = 40;
            this.lbl_FFU_Pressure.UnitFont = new System.Drawing.Font("맑은 고딕", 10F, System.Drawing.FontStyle.Bold);
            this.lbl_FFU_Pressure.UnitFontColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.lbl_FFU_Pressure.UnitPositionVertical = false;
            this.lbl_FFU_Pressure.UnitText = "";
            this.lbl_FFU_Pressure.UseBorder = true;
            this.lbl_FFU_Pressure.UseEdgeRadius = false;
            this.lbl_FFU_Pressure.UseImage = false;
            this.lbl_FFU_Pressure.UseSubFont = false;
            this.lbl_FFU_Pressure.UseUnitFont = false;
            this.lbl_FFU_Pressure.Click += new System.EventHandler(this.Click_Pressure);
            // 
            // lbl_FFU_AlarmCode
            // 
            this.lbl_FFU_AlarmCode.BackGroundColor = System.Drawing.Color.White;
            this.lbl_FFU_AlarmCode.BorderStroke = 2;
            this.lbl_FFU_AlarmCode.BorderStyle = System.Windows.Forms.ButtonBorderStyle.Solid;
            this.lbl_FFU_AlarmCode.Description = "";
            this.lbl_FFU_AlarmCode.DisabledColor = System.Drawing.Color.LightGray;
            this.lbl_FFU_AlarmCode.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lbl_FFU_AlarmCode.EdgeRadius = 1;
            this.lbl_FFU_AlarmCode.ImagePosition = new System.Drawing.Point(0, 0);
            this.lbl_FFU_AlarmCode.ImageSize = new System.Drawing.Point(0, 0);
            this.lbl_FFU_AlarmCode.LoadImage = null;
            this.lbl_FFU_AlarmCode.Location = new System.Drawing.Point(289, 1);
            this.lbl_FFU_AlarmCode.MainFont = new System.Drawing.Font("맑은 고딕", 11F, System.Drawing.FontStyle.Bold);
            this.lbl_FFU_AlarmCode.MainFontColor = System.Drawing.Color.Black;
            this.lbl_FFU_AlarmCode.Margin = new System.Windows.Forms.Padding(1, 1, 0, 0);
            this.lbl_FFU_AlarmCode.Name = "lbl_FFU_AlarmCode";
            this.lbl_FFU_AlarmCode.Size = new System.Drawing.Size(191, 43);
            this.lbl_FFU_AlarmCode.SubFont = new System.Drawing.Font("맑은 고딕", 10F);
            this.lbl_FFU_AlarmCode.SubFontColor = System.Drawing.Color.Black;
            this.lbl_FFU_AlarmCode.SubText = "";
            this.lbl_FFU_AlarmCode.TabIndex = 21132;
            this.lbl_FFU_AlarmCode.Tag = "";
            this.lbl_FFU_AlarmCode.TextAlignMain = Sys3Controls.EN_TEXTALIGN.BOTTOM_RIGHT;
            this.lbl_FFU_AlarmCode.TextAlignSub = Sys3Controls.EN_TEXTALIGN.MIDDLE_CENTER;
            this.lbl_FFU_AlarmCode.TextAlignUnit = Sys3Controls.EN_TEXTALIGN.MIDDLE_CENTER;
            this.lbl_FFU_AlarmCode.ThemeIndex = 0;
            this.lbl_FFU_AlarmCode.UnitAreaRate = 40;
            this.lbl_FFU_AlarmCode.UnitFont = new System.Drawing.Font("맑은 고딕", 10F, System.Drawing.FontStyle.Bold);
            this.lbl_FFU_AlarmCode.UnitFontColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.lbl_FFU_AlarmCode.UnitPositionVertical = false;
            this.lbl_FFU_AlarmCode.UnitText = "";
            this.lbl_FFU_AlarmCode.UseBorder = true;
            this.lbl_FFU_AlarmCode.UseEdgeRadius = false;
            this.lbl_FFU_AlarmCode.UseImage = false;
            this.lbl_FFU_AlarmCode.UseSubFont = false;
            this.lbl_FFU_AlarmCode.UseUnitFont = false;
            // 
            // toggleDifferentialPressureMode
            // 
            this.toggleDifferentialPressureMode.Active = false;
            this.toggleDifferentialPressureMode.ActiveColorFirst = System.Drawing.Color.RoyalBlue;
            this.toggleDifferentialPressureMode.ActiveColorSecond = System.Drawing.Color.DodgerBlue;
            this.toggleDifferentialPressureMode.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.toggleDifferentialPressureMode.Location = new System.Drawing.Point(1058, 6);
            this.toggleDifferentialPressureMode.Name = "toggleDifferentialPressureMode";
            this.toggleDifferentialPressureMode.NormalColorFirst = System.Drawing.Color.DarkGray;
            this.toggleDifferentialPressureMode.NormalColorSecond = System.Drawing.Color.Silver;
            this.toggleDifferentialPressureMode.Size = new System.Drawing.Size(60, 30);
            this.toggleDifferentialPressureMode.TabIndex = 20635;
            this.toggleDifferentialPressureMode.Click += new System.EventHandler(this.ToggleClicked);
            // 
            // toggleUseFFU
            // 
            this.toggleUseFFU.Active = false;
            this.toggleUseFFU.ActiveColorFirst = System.Drawing.Color.RoyalBlue;
            this.toggleUseFFU.ActiveColorSecond = System.Drawing.Color.DodgerBlue;
            this.toggleUseFFU.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.toggleUseFFU.Location = new System.Drawing.Point(264, 6);
            this.toggleUseFFU.Name = "toggleUseFFU";
            this.toggleUseFFU.NormalColorFirst = System.Drawing.Color.DarkGray;
            this.toggleUseFFU.NormalColorSecond = System.Drawing.Color.Silver;
            this.toggleUseFFU.Size = new System.Drawing.Size(60, 30);
            this.toggleUseFFU.TabIndex = 20636;
            this.toggleUseFFU.Click += new System.EventHandler(this.ToggleClicked);
            // 
            // sys3Label5
            // 
            this.sys3Label5.BackGroundColor = System.Drawing.Color.DarkGray;
            this.sys3Label5.BorderStroke = 2;
            this.sys3Label5.BorderStyle = System.Windows.Forms.ButtonBorderStyle.Solid;
            this.sys3Label5.Description = "";
            this.sys3Label5.DisabledColor = System.Drawing.Color.DarkGray;
            this.sys3Label5.Dock = System.Windows.Forms.DockStyle.Fill;
            this.sys3Label5.EdgeRadius = 1;
            this.sys3Label5.ImagePosition = new System.Drawing.Point(0, 0);
            this.sys3Label5.ImageSize = new System.Drawing.Point(0, 0);
            this.sys3Label5.LoadImage = null;
            this.sys3Label5.Location = new System.Drawing.Point(1, 1);
            this.sys3Label5.MainFont = new System.Drawing.Font("맑은 고딕", 11F, System.Drawing.FontStyle.Bold);
            this.sys3Label5.MainFontColor = System.Drawing.Color.Black;
            this.sys3Label5.Margin = new System.Windows.Forms.Padding(1, 1, 0, 0);
            this.sys3Label5.Name = "sys3Label5";
            this.sys3Label5.Size = new System.Drawing.Size(260, 42);
            this.sys3Label5.SubFont = new System.Drawing.Font("맑은 고딕", 10F, System.Drawing.FontStyle.Bold);
            this.sys3Label5.SubFontColor = System.Drawing.Color.Gray;
            this.sys3Label5.SubText = "";
            this.sys3Label5.TabIndex = 20596;
            this.sys3Label5.Text = "USE";
            this.sys3Label5.TextAlignMain = Sys3Controls.EN_TEXTALIGN.MIDDLE_RIGHT;
            this.sys3Label5.TextAlignSub = Sys3Controls.EN_TEXTALIGN.TOP_LEFT;
            this.sys3Label5.TextAlignUnit = Sys3Controls.EN_TEXTALIGN.MIDDLE_CENTER;
            this.sys3Label5.ThemeIndex = 0;
            this.sys3Label5.UnitAreaRate = 30;
            this.sys3Label5.UnitFont = new System.Drawing.Font("맑은 고딕", 10F, System.Drawing.FontStyle.Bold);
            this.sys3Label5.UnitFontColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.sys3Label5.UnitPositionVertical = false;
            this.sys3Label5.UnitText = "";
            this.sys3Label5.UseBorder = true;
            this.sys3Label5.UseEdgeRadius = false;
            this.sys3Label5.UseImage = false;
            this.sys3Label5.UseSubFont = true;
            this.sys3Label5.UseUnitFont = false;
            // 
            // tableLayoutPanel6
            // 
            this.tableLayoutPanel6.ColumnCount = 6;
            this.tableLayoutPanel6.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 23F));
            this.tableLayoutPanel6.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 7F));
            this.tableLayoutPanel6.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 17F));
            this.tableLayoutPanel6.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 13F));
            this.tableLayoutPanel6.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 33F));
            this.tableLayoutPanel6.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 7F));
            this.tableLayoutPanel6.Controls.Add(this.lbl_FFU_UnitCount, 3, 0);
            this.tableLayoutPanel6.Controls.Add(this.toggleDifferentialPressureMode, 5, 0);
            this.tableLayoutPanel6.Controls.Add(this.toggleUseFFU, 1, 0);
            this.tableLayoutPanel6.Controls.Add(this.sys3Label5, 0, 0);
            this.tableLayoutPanel6.Controls.Add(this.sys3Label9, 2, 0);
            this.tableLayoutPanel6.Controls.Add(this.sys3Label10, 4, 0);
            this.tableLayoutPanel6.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel6.Location = new System.Drawing.Point(2, 2);
            this.tableLayoutPanel6.Margin = new System.Windows.Forms.Padding(2, 2, 0, 0);
            this.tableLayoutPanel6.Name = "tableLayoutPanel6";
            this.tableLayoutPanel6.RowCount = 1;
            this.tableLayoutPanel6.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel6.Size = new System.Drawing.Size(1138, 43);
            this.tableLayoutPanel6.TabIndex = 3;
            // 
            // lbl_FFU_UnitCount
            // 
            this.lbl_FFU_UnitCount.BackGroundColor = System.Drawing.Color.White;
            this.lbl_FFU_UnitCount.BorderStroke = 2;
            this.lbl_FFU_UnitCount.BorderStyle = System.Windows.Forms.ButtonBorderStyle.Solid;
            this.lbl_FFU_UnitCount.Description = "";
            this.lbl_FFU_UnitCount.DisabledColor = System.Drawing.Color.LightGray;
            this.lbl_FFU_UnitCount.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lbl_FFU_UnitCount.EdgeRadius = 1;
            this.lbl_FFU_UnitCount.ImagePosition = new System.Drawing.Point(0, 0);
            this.lbl_FFU_UnitCount.ImageSize = new System.Drawing.Point(0, 0);
            this.lbl_FFU_UnitCount.LoadImage = null;
            this.lbl_FFU_UnitCount.Location = new System.Drawing.Point(534, 1);
            this.lbl_FFU_UnitCount.MainFont = new System.Drawing.Font("맑은 고딕", 11F, System.Drawing.FontStyle.Bold);
            this.lbl_FFU_UnitCount.MainFontColor = System.Drawing.Color.Black;
            this.lbl_FFU_UnitCount.Margin = new System.Windows.Forms.Padding(1);
            this.lbl_FFU_UnitCount.Name = "lbl_FFU_UnitCount";
            this.lbl_FFU_UnitCount.Size = new System.Drawing.Size(145, 41);
            this.lbl_FFU_UnitCount.SubFont = new System.Drawing.Font("맑은 고딕", 10F);
            this.lbl_FFU_UnitCount.SubFontColor = System.Drawing.Color.Black;
            this.lbl_FFU_UnitCount.SubText = "";
            this.lbl_FFU_UnitCount.TabIndex = 21132;
            this.lbl_FFU_UnitCount.Tag = "";
            this.lbl_FFU_UnitCount.TextAlignMain = Sys3Controls.EN_TEXTALIGN.BOTTOM_RIGHT;
            this.lbl_FFU_UnitCount.TextAlignSub = Sys3Controls.EN_TEXTALIGN.MIDDLE_CENTER;
            this.lbl_FFU_UnitCount.TextAlignUnit = Sys3Controls.EN_TEXTALIGN.MIDDLE_CENTER;
            this.lbl_FFU_UnitCount.ThemeIndex = 0;
            this.lbl_FFU_UnitCount.UnitAreaRate = 40;
            this.lbl_FFU_UnitCount.UnitFont = new System.Drawing.Font("맑은 고딕", 10F, System.Drawing.FontStyle.Bold);
            this.lbl_FFU_UnitCount.UnitFontColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.lbl_FFU_UnitCount.UnitPositionVertical = false;
            this.lbl_FFU_UnitCount.UnitText = "";
            this.lbl_FFU_UnitCount.UseBorder = true;
            this.lbl_FFU_UnitCount.UseEdgeRadius = false;
            this.lbl_FFU_UnitCount.UseImage = false;
            this.lbl_FFU_UnitCount.UseSubFont = false;
            this.lbl_FFU_UnitCount.UseUnitFont = false;
            // 
            // tableLayoutPanel4
            // 
            this.tableLayoutPanel4.ColumnCount = 1;
            this.tableLayoutPanel4.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel4.Controls.Add(this.tableLayoutPanel9, 0, 2);
            this.tableLayoutPanel4.Controls.Add(this.tableLayoutPanel6, 0, 0);
            this.tableLayoutPanel4.Controls.Add(this.tableLayoutPanel5, 0, 1);
            this.tableLayoutPanel4.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel4.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel4.Margin = new System.Windows.Forms.Padding(0);
            this.tableLayoutPanel4.Name = "tableLayoutPanel4";
            this.tableLayoutPanel4.RowCount = 3;
            this.tableLayoutPanel4.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.tableLayoutPanel4.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel4.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.tableLayoutPanel4.Size = new System.Drawing.Size(1140, 180);
            this.tableLayoutPanel4.TabIndex = 2;
            // 
            // tableLayoutPanel9
            // 
            this.tableLayoutPanel9.ColumnCount = 2;
            this.tableLayoutPanel9.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 42F));
            this.tableLayoutPanel9.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 58F));
            this.tableLayoutPanel9.Controls.Add(this.tableLayoutPanel2, 0, 0);
            this.tableLayoutPanel9.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel9.Location = new System.Drawing.Point(0, 135);
            this.tableLayoutPanel9.Margin = new System.Windows.Forms.Padding(0);
            this.tableLayoutPanel9.Name = "tableLayoutPanel9";
            this.tableLayoutPanel9.RowCount = 1;
            this.tableLayoutPanel9.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel9.Size = new System.Drawing.Size(1140, 45);
            this.tableLayoutPanel9.TabIndex = 21132;
            // 
            // tableLayoutPanel2
            // 
            this.tableLayoutPanel2.ColumnCount = 2;
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 59.52381F));
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 40.47619F));
            this.tableLayoutPanel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel2.Location = new System.Drawing.Point(1, 1);
            this.tableLayoutPanel2.Margin = new System.Windows.Forms.Padding(1, 1, 0, 0);
            this.tableLayoutPanel2.Name = "tableLayoutPanel2";
            this.tableLayoutPanel2.RowCount = 1;
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel2.Size = new System.Drawing.Size(477, 44);
            this.tableLayoutPanel2.TabIndex = 21131;
            // 
            // tableLayoutPanel5
            // 
            this.tableLayoutPanel5.ColumnCount = 5;
            this.tableLayoutPanel5.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.tableLayoutPanel5.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 17F));
            this.tableLayoutPanel5.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 8F));
            this.tableLayoutPanel5.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 8F));
            this.tableLayoutPanel5.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 42F));
            this.tableLayoutPanel5.Controls.Add(this.btnStop, 3, 0);
            this.tableLayoutPanel5.Controls.Add(this.tableLayoutPanel3, 4, 0);
            this.tableLayoutPanel5.Controls.Add(this.tableLayoutPanel8, 0, 0);
            this.tableLayoutPanel5.Controls.Add(this.tableLayoutPanel7, 0, 0);
            this.tableLayoutPanel5.Controls.Add(this.tableLayoutPanel10, 2, 0);
            this.tableLayoutPanel5.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel5.Location = new System.Drawing.Point(1, 46);
            this.tableLayoutPanel5.Margin = new System.Windows.Forms.Padding(1, 1, 0, 0);
            this.tableLayoutPanel5.Name = "tableLayoutPanel5";
            this.tableLayoutPanel5.RowCount = 1;
            this.tableLayoutPanel5.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel5.Size = new System.Drawing.Size(1139, 89);
            this.tableLayoutPanel5.TabIndex = 2;
            // 
            // tableLayoutPanel3
            // 
            this.tableLayoutPanel3.ColumnCount = 2;
            this.tableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 60F));
            this.tableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 40F));
            this.tableLayoutPanel3.Controls.Add(this.sys3Label7, 0, 0);
            this.tableLayoutPanel3.Controls.Add(this.lbl_FFU_AlarmCode, 1, 0);
            this.tableLayoutPanel3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel3.Location = new System.Drawing.Point(659, 0);
            this.tableLayoutPanel3.Margin = new System.Windows.Forms.Padding(0);
            this.tableLayoutPanel3.Name = "tableLayoutPanel3";
            this.tableLayoutPanel3.RowCount = 2;
            this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel3.Size = new System.Drawing.Size(480, 89);
            this.tableLayoutPanel3.TabIndex = 21132;
            // 
            // tableLayoutPanel10
            // 
            this.tableLayoutPanel10.ColumnCount = 1;
            this.tableLayoutPanel10.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel10.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel10.Controls.Add(this.btnSetSpeedAll, 0, 0);
            this.tableLayoutPanel10.Controls.Add(this.btnSetPressure, 0, 1);
            this.tableLayoutPanel10.Location = new System.Drawing.Point(477, 0);
            this.tableLayoutPanel10.Margin = new System.Windows.Forms.Padding(0);
            this.tableLayoutPanel10.Name = "tableLayoutPanel10";
            this.tableLayoutPanel10.RowCount = 2;
            this.tableLayoutPanel10.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel10.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel10.Size = new System.Drawing.Size(91, 89);
            this.tableLayoutPanel10.TabIndex = 21133;
            // 
            // btnSetPressure
            // 
            this.btnSetPressure.BorderWidth = 2;
            this.btnSetPressure.ButtonClicked = false;
            this.btnSetPressure.ClickedEmphasizeTextColor = System.Drawing.Color.White;
            this.btnSetPressure.CustomClickedGradientFirstColor = System.Drawing.Color.BlanchedAlmond;
            this.btnSetPressure.CustomClickedGradientSecondColor = System.Drawing.Color.Gold;
            this.btnSetPressure.Description = "";
            this.btnSetPressure.DisabledColor = System.Drawing.Color.DarkGray;
            this.btnSetPressure.EdgeRadius = 5;
            this.btnSetPressure.GradientAngle = 60F;
            this.btnSetPressure.GradientFirstColor = System.Drawing.Color.Silver;
            this.btnSetPressure.GradientSecondColor = System.Drawing.Color.Gray;
            this.btnSetPressure.HoverEmphasizeCustomColor = System.Drawing.Color.Firebrick;
            this.btnSetPressure.ImagePosition = new System.Drawing.Point(7, 7);
            this.btnSetPressure.ImageSize = new System.Drawing.Point(30, 30);
            this.btnSetPressure.LoadImage = null;
            this.btnSetPressure.Location = new System.Drawing.Point(1, 45);
            this.btnSetPressure.MainFont = new System.Drawing.Font("맑은 고딕", 12F, System.Drawing.FontStyle.Bold);
            this.btnSetPressure.MainFontColor = System.Drawing.Color.SpringGreen;
            this.btnSetPressure.Margin = new System.Windows.Forms.Padding(1);
            this.btnSetPressure.Name = "btnSetPressure";
            this.btnSetPressure.Size = new System.Drawing.Size(89, 43);
            this.btnSetPressure.SubFont = new System.Drawing.Font("맑은 고딕", 8F, System.Drawing.FontStyle.Bold);
            this.btnSetPressure.SubFontColor = System.Drawing.Color.DarkBlue;
            this.btnSetPressure.SubText = "";
            this.btnSetPressure.TabIndex = 21131;
            this.btnSetPressure.Text = "SET";
            this.btnSetPressure.TextAlignMain = Sys3Controls.EN_TEXTALIGN.MIDDLE_CENTER;
            this.btnSetPressure.TextAlignSub = Sys3Controls.EN_TEXTALIGN.TOP_LEFT;
            this.btnSetPressure.ThemeIndex = 0;
            this.btnSetPressure.UseBorder = true;
            this.btnSetPressure.UseClickedEmphasizeTextColor = false;
            this.btnSetPressure.UseCustomizeClickedColor = true;
            this.btnSetPressure.UseEdge = true;
            this.btnSetPressure.UseHoverEmphasizeCustomColor = true;
            this.btnSetPressure.UseImage = true;
            this.btnSetPressure.UserHoverEmpahsize = true;
            this.btnSetPressure.UseSubFont = false;
            this.btnSetPressure.Click += new System.EventHandler(this.Click_SetPressure);
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 1;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.Controls.Add(this.tableLayoutPanel4, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.gvFanFilterUnitStatus, 0, 1);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel1.Margin = new System.Windows.Forms.Padding(0);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 3;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 20F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 30F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(1140, 900);
            this.tableLayoutPanel1.TabIndex = 4;
            // 
            // dataGridViewTextBoxColumn3
            // 
            this.dataGridViewTextBoxColumn3.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.ColumnHeader;
            this.dataGridViewTextBoxColumn3.Frozen = true;
            this.dataGridViewTextBoxColumn3.HeaderText = "INDEX";
            this.dataGridViewTextBoxColumn3.MaxInputLength = 20;
            this.dataGridViewTextBoxColumn3.Name = "dataGridViewTextBoxColumn3";
            this.dataGridViewTextBoxColumn3.ReadOnly = true;
            this.dataGridViewTextBoxColumn3.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.dataGridViewTextBoxColumn3.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.dataGridViewTextBoxColumn3.Width = 60;
            // 
            // dataGridViewTextBoxColumn4
            // 
            this.dataGridViewTextBoxColumn4.HeaderText = "NAME";
            this.dataGridViewTextBoxColumn4.MaxInputLength = 20;
            this.dataGridViewTextBoxColumn4.Name = "dataGridViewTextBoxColumn4";
            this.dataGridViewTextBoxColumn4.ReadOnly = true;
            this.dataGridViewTextBoxColumn4.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.dataGridViewTextBoxColumn4.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.dataGridViewTextBoxColumn4.Width = 150;
            // 
            // SettingSpeed
            // 
            this.SettingSpeed.HeaderText = "SETTING SPEED (RPM)";
            this.SettingSpeed.Name = "SettingSpeed";
            this.SettingSpeed.ReadOnly = true;
            this.SettingSpeed.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.SettingSpeed.Width = 190;
            // 
            // CurrentSpeed
            // 
            this.CurrentSpeed.HeaderText = "CURRENT SPEED (RPM)";
            this.CurrentSpeed.Name = "CurrentSpeed";
            this.CurrentSpeed.ReadOnly = true;
            this.CurrentSpeed.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.CurrentSpeed.Width = 190;
            // 
            // CurrentDifferentialPressure
            // 
            this.CurrentDifferentialPressure.HeaderText = "CURRENT PRESSURE (mmAq)";
            this.CurrentDifferentialPressure.Name = "CurrentDifferentialPressure";
            this.CurrentDifferentialPressure.ReadOnly = true;
            this.CurrentDifferentialPressure.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.CurrentDifferentialPressure.Width = 225;
            // 
            // Status
            // 
            this.Status.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.Status.HeaderText = "STATUS";
            this.Status.Name = "Status";
            this.Status.ReadOnly = true;
            this.Status.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // Setup_FanFilterUnit
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.Controls.Add(this.tableLayoutPanel1);
            this.Margin = new System.Windows.Forms.Padding(0);
            this.Name = "Setup_FanFilterUnit";
            this.Size = new System.Drawing.Size(1140, 900);
            ((System.ComponentModel.ISupportInitialize)(this.gvFanFilterUnitStatus)).EndInit();
            this.tableLayoutPanel8.ResumeLayout(false);
            this.tableLayoutPanel7.ResumeLayout(false);
            this.tableLayoutPanel6.ResumeLayout(false);
            this.tableLayoutPanel4.ResumeLayout(false);
            this.tableLayoutPanel9.ResumeLayout(false);
            this.tableLayoutPanel5.ResumeLayout(false);
            this.tableLayoutPanel3.ResumeLayout(false);
            this.tableLayoutPanel10.ResumeLayout(false);
            this.tableLayoutPanel1.ResumeLayout(false);
            this.ResumeLayout(false);

		}

        #endregion
        private Sys3Controls.Sys3DoubleBufferedDataGridView gvFanFilterUnitStatus;
        private Sys3Controls.Sys3Label sys3Label7;
        private Sys3Controls.Sys3Label sys3Label4;
        private Sys3Controls.Sys3button btnSetSpeedAll;
        private Sys3Controls.Sys3button btnStop;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel8;
        private Sys3Controls.Sys3Label sys3Label9;
        private Sys3Controls.Sys3Label sys3Label10;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel7;
        private Sys3Controls.Sys3ToggleButton toggleDifferentialPressureMode;
        private Sys3Controls.Sys3ToggleButton toggleUseFFU;
        private Sys3Controls.Sys3Label sys3Label5;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel6;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel4;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel5;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private Sys3Controls.Sys3Label lbl_FFU_AlarmCode;
        private Sys3Controls.Sys3Label lbl_FFU_Speed;
        private Sys3Controls.Sys3Label lbl_FFU_UnitCount;
        private Sys3Controls.Sys3Label sys3Label1;
        private Sys3Controls.Sys3Label lbl_FFU_Pressure;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel3;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel9;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel2;
        private Sys3Controls.Sys3button btnSetPressure;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel10;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn3;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn4;
        private System.Windows.Forms.DataGridViewTextBoxColumn SettingSpeed;
        private System.Windows.Forms.DataGridViewTextBoxColumn CurrentSpeed;
        private System.Windows.Forms.DataGridViewTextBoxColumn CurrentDifferentialPressure;
        private System.Windows.Forms.DataGridViewTextBoxColumn Status;
    }
}
