namespace FrameOfSystem3.Views.Operation
{
	partial class SubViewOperationSecsGemCommon
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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle4 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle3 = new System.Windows.Forms.DataGridViewCellStyle();
            this.tableLayoutPanel5 = new System.Windows.Forms.TableLayoutPanel();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.gvMessageToSend = new System.Windows.Forms.DataGridView();
            this.col_DateName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.col_DataValue = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.tableLayoutPanel4 = new System.Windows.Forms.TableLayoutPanel();
            this.tglUseSecsgem = new Sys3Controls.Sys3ToggleButton();
            this.sys3Label20 = new Sys3Controls.Sys3Label();
            this.tableLayoutPanel6 = new System.Windows.Forms.TableLayoutPanel();
            this.btnExecuteScenario = new Sys3Controls.Sys3button();
            this.lblScenarioToExecute = new Sys3Controls.Sys3Label();
            this.sys3Label12 = new Sys3Controls.Sys3Label();
            this.tableLayoutPanel7 = new System.Windows.Forms.TableLayoutPanel();
            this.sys3button2 = new Sys3Controls.Sys3button();
            this.sys3button1 = new Sys3Controls.Sys3button();
            this.lbl_SendDataKey = new Sys3Controls.Sys3Label();
            this.tableLayoutPanel5.SuspendLayout();
            this.tableLayoutPanel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gvMessageToSend)).BeginInit();
            this.tableLayoutPanel4.SuspendLayout();
            this.tableLayoutPanel6.SuspendLayout();
            this.tableLayoutPanel7.SuspendLayout();
            this.SuspendLayout();
            // 
            // tableLayoutPanel5
            // 
            this.tableLayoutPanel5.ColumnCount = 2;
            this.tableLayoutPanel5.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 53F));
            this.tableLayoutPanel5.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 47F));
            this.tableLayoutPanel5.Controls.Add(this.tableLayoutPanel1, 0, 0);
            this.tableLayoutPanel5.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel5.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel5.Margin = new System.Windows.Forms.Padding(0);
            this.tableLayoutPanel5.Name = "tableLayoutPanel5";
            this.tableLayoutPanel5.RowCount = 2;
            this.tableLayoutPanel5.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 70F));
            this.tableLayoutPanel5.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 30F));
            this.tableLayoutPanel5.Size = new System.Drawing.Size(1137, 588);
            this.tableLayoutPanel5.TabIndex = 5;
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 1;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.Controls.Add(this.gvMessageToSend, 0, 3);
            this.tableLayoutPanel1.Controls.Add(this.tableLayoutPanel4, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.tableLayoutPanel6, 0, 1);
            this.tableLayoutPanel1.Controls.Add(this.tableLayoutPanel7, 0, 2);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel1.Margin = new System.Windows.Forms.Padding(0);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 4;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 8F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 8F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 8F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 76F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(602, 411);
            this.tableLayoutPanel1.TabIndex = 2;
            // 
            // gvMessageToSend
            // 
            this.gvMessageToSend.AllowUserToAddRows = false;
            this.gvMessageToSend.AllowUserToDeleteRows = false;
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle1.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle1.Font = new System.Drawing.Font("맑은 고딕", 10F, System.Drawing.FontStyle.Bold);
            dataGridViewCellStyle1.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.gvMessageToSend.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
            this.gvMessageToSend.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.gvMessageToSend.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.col_DateName,
            this.col_DataValue});
            this.gvMessageToSend.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gvMessageToSend.Location = new System.Drawing.Point(3, 99);
            this.gvMessageToSend.Name = "gvMessageToSend";
            dataGridViewCellStyle4.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle4.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle4.Font = new System.Drawing.Font("맑은 고딕", 10F, System.Drawing.FontStyle.Bold);
            dataGridViewCellStyle4.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle4.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle4.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle4.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.gvMessageToSend.RowHeadersDefaultCellStyle = dataGridViewCellStyle4;
            this.gvMessageToSend.RowTemplate.Height = 23;
            this.gvMessageToSend.Size = new System.Drawing.Size(596, 309);
            this.gvMessageToSend.TabIndex = 20896;
            // 
            // col_DateName
            // 
            dataGridViewCellStyle2.Font = new System.Drawing.Font("맑은 고딕", 10F, System.Drawing.FontStyle.Bold);
            this.col_DateName.DefaultCellStyle = dataGridViewCellStyle2;
            this.col_DateName.HeaderText = "Name";
            this.col_DateName.Name = "col_DateName";
            this.col_DateName.ReadOnly = true;
            this.col_DateName.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.col_DateName.Width = 150;
            // 
            // col_DataValue
            // 
            this.col_DataValue.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            dataGridViewCellStyle3.Font = new System.Drawing.Font("맑은 고딕", 10F, System.Drawing.FontStyle.Bold);
            this.col_DataValue.DefaultCellStyle = dataGridViewCellStyle3;
            this.col_DataValue.HeaderText = "Data";
            this.col_DataValue.Name = "col_DataValue";
            this.col_DataValue.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // tableLayoutPanel4
            // 
            this.tableLayoutPanel4.ColumnCount = 2;
            this.tableLayoutPanel4.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 33.33333F));
            this.tableLayoutPanel4.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 66.66666F));
            this.tableLayoutPanel4.Controls.Add(this.tglUseSecsgem, 0, 0);
            this.tableLayoutPanel4.Controls.Add(this.sys3Label20, 0, 0);
            this.tableLayoutPanel4.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel4.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel4.Margin = new System.Windows.Forms.Padding(0);
            this.tableLayoutPanel4.Name = "tableLayoutPanel4";
            this.tableLayoutPanel4.RowCount = 1;
            this.tableLayoutPanel4.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel4.Size = new System.Drawing.Size(602, 32);
            this.tableLayoutPanel4.TabIndex = 2;
            // 
            // tglUseSecsgem
            // 
            this.tglUseSecsgem.Active = false;
            this.tglUseSecsgem.ActiveColorFirst = System.Drawing.Color.RoyalBlue;
            this.tglUseSecsgem.ActiveColorSecond = System.Drawing.Color.DodgerBlue;
            this.tglUseSecsgem.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.tglUseSecsgem.BackColor = System.Drawing.SystemColors.Control;
            this.tglUseSecsgem.Location = new System.Drawing.Point(205, 2);
            this.tglUseSecsgem.Margin = new System.Windows.Forms.Padding(5, 1, 0, 0);
            this.tglUseSecsgem.Name = "tglUseSecsgem";
            this.tglUseSecsgem.NormalColorFirst = System.Drawing.Color.DarkGray;
            this.tglUseSecsgem.NormalColorSecond = System.Drawing.Color.Silver;
            this.tglUseSecsgem.Size = new System.Drawing.Size(56, 28);
            this.tglUseSecsgem.TabIndex = 20892;
            this.tglUseSecsgem.Click += new System.EventHandler(this.toggleUseSecsGemClicked);
            // 
            // sys3Label20
            // 
            this.sys3Label20.BackGroundColor = System.Drawing.Color.DarkGray;
            this.sys3Label20.BorderStroke = 2;
            this.sys3Label20.BorderStyle = System.Windows.Forms.ButtonBorderStyle.Solid;
            this.sys3Label20.Description = "";
            this.sys3Label20.DisabledColor = System.Drawing.Color.DarkGray;
            this.sys3Label20.Dock = System.Windows.Forms.DockStyle.Fill;
            this.sys3Label20.EdgeRadius = 1;
            this.sys3Label20.ImagePosition = new System.Drawing.Point(0, 0);
            this.sys3Label20.ImageSize = new System.Drawing.Point(0, 0);
            this.sys3Label20.LoadImage = null;
            this.sys3Label20.Location = new System.Drawing.Point(1, 1);
            this.sys3Label20.MainFont = new System.Drawing.Font("맑은 고딕", 11F, System.Drawing.FontStyle.Bold);
            this.sys3Label20.MainFontColor = System.Drawing.Color.Black;
            this.sys3Label20.Margin = new System.Windows.Forms.Padding(1, 1, 0, 0);
            this.sys3Label20.Name = "sys3Label20";
            this.sys3Label20.Size = new System.Drawing.Size(199, 31);
            this.sys3Label20.SubFont = new System.Drawing.Font("맑은 고딕", 10F, System.Drawing.FontStyle.Bold);
            this.sys3Label20.SubFontColor = System.Drawing.Color.Gray;
            this.sys3Label20.SubText = "";
            this.sys3Label20.TabIndex = 20892;
            this.sys3Label20.Text = "USE";
            this.sys3Label20.TextAlignMain = Sys3Controls.EN_TEXTALIGN.MIDDLE_RIGHT;
            this.sys3Label20.TextAlignSub = Sys3Controls.EN_TEXTALIGN.TOP_LEFT;
            this.sys3Label20.TextAlignUnit = Sys3Controls.EN_TEXTALIGN.MIDDLE_CENTER;
            this.sys3Label20.ThemeIndex = 0;
            this.sys3Label20.UnitAreaRate = 30;
            this.sys3Label20.UnitFont = new System.Drawing.Font("맑은 고딕", 10F, System.Drawing.FontStyle.Bold);
            this.sys3Label20.UnitFontColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.sys3Label20.UnitPositionVertical = false;
            this.sys3Label20.UnitText = "";
            this.sys3Label20.UseBorder = true;
            this.sys3Label20.UseEdgeRadius = false;
            this.sys3Label20.UseImage = false;
            this.sys3Label20.UseSubFont = true;
            this.sys3Label20.UseUnitFont = false;
            // 
            // tableLayoutPanel6
            // 
            this.tableLayoutPanel6.ColumnCount = 3;
            this.tableLayoutPanel6.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 33.33555F));
            this.tableLayoutPanel6.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 46.66311F));
            this.tableLayoutPanel6.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 20.00133F));
            this.tableLayoutPanel6.Controls.Add(this.btnExecuteScenario, 0, 0);
            this.tableLayoutPanel6.Controls.Add(this.lblScenarioToExecute, 0, 0);
            this.tableLayoutPanel6.Controls.Add(this.sys3Label12, 0, 0);
            this.tableLayoutPanel6.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel6.Location = new System.Drawing.Point(0, 32);
            this.tableLayoutPanel6.Margin = new System.Windows.Forms.Padding(0);
            this.tableLayoutPanel6.Name = "tableLayoutPanel6";
            this.tableLayoutPanel6.RowCount = 1;
            this.tableLayoutPanel6.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel6.Size = new System.Drawing.Size(602, 32);
            this.tableLayoutPanel6.TabIndex = 4;
            // 
            // btnExecuteScenario
            // 
            this.btnExecuteScenario.BorderWidth = 2;
            this.btnExecuteScenario.ButtonClicked = false;
            this.btnExecuteScenario.ClickedEmphasizeTextColor = System.Drawing.Color.White;
            this.btnExecuteScenario.CustomClickedGradientFirstColor = System.Drawing.Color.BlanchedAlmond;
            this.btnExecuteScenario.CustomClickedGradientSecondColor = System.Drawing.Color.Gold;
            this.btnExecuteScenario.Description = "";
            this.btnExecuteScenario.DisabledColor = System.Drawing.Color.DarkGray;
            this.btnExecuteScenario.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnExecuteScenario.EdgeRadius = 5;
            this.btnExecuteScenario.GradientAngle = 60F;
            this.btnExecuteScenario.GradientFirstColor = System.Drawing.Color.Silver;
            this.btnExecuteScenario.GradientSecondColor = System.Drawing.Color.Gray;
            this.btnExecuteScenario.HoverEmphasizeCustomColor = System.Drawing.Color.Firebrick;
            this.btnExecuteScenario.ImagePosition = new System.Drawing.Point(37, 25);
            this.btnExecuteScenario.ImageSize = new System.Drawing.Point(30, 30);
            this.btnExecuteScenario.LoadImage = null;
            this.btnExecuteScenario.Location = new System.Drawing.Point(481, 1);
            this.btnExecuteScenario.MainFont = new System.Drawing.Font("맑은 고딕", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.btnExecuteScenario.MainFontColor = System.Drawing.Color.White;
            this.btnExecuteScenario.Margin = new System.Windows.Forms.Padding(1, 1, 0, 0);
            this.btnExecuteScenario.Name = "btnExecuteScenario";
            this.btnExecuteScenario.Size = new System.Drawing.Size(121, 31);
            this.btnExecuteScenario.SubFont = new System.Drawing.Font("맑은 고딕", 10F);
            this.btnExecuteScenario.SubFontColor = System.Drawing.Color.Black;
            this.btnExecuteScenario.SubText = "";
            this.btnExecuteScenario.TabIndex = 20847;
            this.btnExecuteScenario.Text = "RUN";
            this.btnExecuteScenario.TextAlignMain = Sys3Controls.EN_TEXTALIGN.MIDDLE_CENTER;
            this.btnExecuteScenario.TextAlignSub = Sys3Controls.EN_TEXTALIGN.MIDDLE_CENTER;
            this.btnExecuteScenario.ThemeIndex = 0;
            this.btnExecuteScenario.UseBorder = true;
            this.btnExecuteScenario.UseClickedEmphasizeTextColor = false;
            this.btnExecuteScenario.UseCustomizeClickedColor = true;
            this.btnExecuteScenario.UseEdge = true;
            this.btnExecuteScenario.UseHoverEmphasizeCustomColor = true;
            this.btnExecuteScenario.UseImage = true;
            this.btnExecuteScenario.UserHoverEmpahsize = true;
            this.btnExecuteScenario.UseSubFont = false;
            this.btnExecuteScenario.Click += new System.EventHandler(this.Click_ScenarioRun);
            // 
            // lblScenarioToExecute
            // 
            this.lblScenarioToExecute.BackGroundColor = System.Drawing.Color.White;
            this.lblScenarioToExecute.BorderStroke = 2;
            this.lblScenarioToExecute.BorderStyle = System.Windows.Forms.ButtonBorderStyle.Solid;
            this.lblScenarioToExecute.Description = "";
            this.lblScenarioToExecute.DisabledColor = System.Drawing.Color.DarkGray;
            this.lblScenarioToExecute.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblScenarioToExecute.EdgeRadius = 1;
            this.lblScenarioToExecute.ImagePosition = new System.Drawing.Point(0, 0);
            this.lblScenarioToExecute.ImageSize = new System.Drawing.Point(0, 0);
            this.lblScenarioToExecute.LoadImage = null;
            this.lblScenarioToExecute.Location = new System.Drawing.Point(201, 1);
            this.lblScenarioToExecute.MainFont = new System.Drawing.Font("맑은 고딕", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.lblScenarioToExecute.MainFontColor = System.Drawing.Color.Black;
            this.lblScenarioToExecute.Margin = new System.Windows.Forms.Padding(1, 1, 0, 0);
            this.lblScenarioToExecute.Name = "lblScenarioToExecute";
            this.lblScenarioToExecute.Size = new System.Drawing.Size(279, 31);
            this.lblScenarioToExecute.SubFont = new System.Drawing.Font("맑은 고딕", 10F);
            this.lblScenarioToExecute.SubFontColor = System.Drawing.Color.Black;
            this.lblScenarioToExecute.SubText = "WorkTable";
            this.lblScenarioToExecute.TabIndex = 20849;
            this.lblScenarioToExecute.Text = "--";
            this.lblScenarioToExecute.TextAlignMain = Sys3Controls.EN_TEXTALIGN.MIDDLE_LEFT;
            this.lblScenarioToExecute.TextAlignSub = Sys3Controls.EN_TEXTALIGN.MIDDLE_CENTER;
            this.lblScenarioToExecute.TextAlignUnit = Sys3Controls.EN_TEXTALIGN.MIDDLE_CENTER;
            this.lblScenarioToExecute.ThemeIndex = 0;
            this.lblScenarioToExecute.UnitAreaRate = 30;
            this.lblScenarioToExecute.UnitFont = new System.Drawing.Font("맑은 고딕", 10F, System.Drawing.FontStyle.Bold);
            this.lblScenarioToExecute.UnitFontColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.lblScenarioToExecute.UnitPositionVertical = false;
            this.lblScenarioToExecute.UnitText = "";
            this.lblScenarioToExecute.UseBorder = true;
            this.lblScenarioToExecute.UseEdgeRadius = false;
            this.lblScenarioToExecute.UseImage = false;
            this.lblScenarioToExecute.UseSubFont = false;
            this.lblScenarioToExecute.UseUnitFont = false;
            this.lblScenarioToExecute.Click += new System.EventHandler(this.Click_ScenarioSelect);
            // 
            // sys3Label12
            // 
            this.sys3Label12.BackGroundColor = System.Drawing.Color.DarkGray;
            this.sys3Label12.BorderStroke = 2;
            this.sys3Label12.BorderStyle = System.Windows.Forms.ButtonBorderStyle.Solid;
            this.sys3Label12.Description = "";
            this.sys3Label12.DisabledColor = System.Drawing.Color.DarkGray;
            this.sys3Label12.Dock = System.Windows.Forms.DockStyle.Fill;
            this.sys3Label12.EdgeRadius = 1;
            this.sys3Label12.ImagePosition = new System.Drawing.Point(0, 0);
            this.sys3Label12.ImageSize = new System.Drawing.Point(0, 0);
            this.sys3Label12.LoadImage = null;
            this.sys3Label12.Location = new System.Drawing.Point(1, 1);
            this.sys3Label12.MainFont = new System.Drawing.Font("맑은 고딕", 11F, System.Drawing.FontStyle.Bold);
            this.sys3Label12.MainFontColor = System.Drawing.Color.Black;
            this.sys3Label12.Margin = new System.Windows.Forms.Padding(1, 1, 0, 0);
            this.sys3Label12.Name = "sys3Label12";
            this.sys3Label12.Size = new System.Drawing.Size(199, 31);
            this.sys3Label12.SubFont = new System.Drawing.Font("맑은 고딕", 10F, System.Drawing.FontStyle.Bold);
            this.sys3Label12.SubFontColor = System.Drawing.Color.Gray;
            this.sys3Label12.SubText = "";
            this.sys3Label12.TabIndex = 20849;
            this.sys3Label12.Text = "SCENARIO";
            this.sys3Label12.TextAlignMain = Sys3Controls.EN_TEXTALIGN.MIDDLE_RIGHT;
            this.sys3Label12.TextAlignSub = Sys3Controls.EN_TEXTALIGN.TOP_LEFT;
            this.sys3Label12.TextAlignUnit = Sys3Controls.EN_TEXTALIGN.MIDDLE_CENTER;
            this.sys3Label12.ThemeIndex = 0;
            this.sys3Label12.UnitAreaRate = 30;
            this.sys3Label12.UnitFont = new System.Drawing.Font("맑은 고딕", 10F, System.Drawing.FontStyle.Bold);
            this.sys3Label12.UnitFontColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.sys3Label12.UnitPositionVertical = false;
            this.sys3Label12.UnitText = "";
            this.sys3Label12.UseBorder = true;
            this.sys3Label12.UseEdgeRadius = false;
            this.sys3Label12.UseImage = false;
            this.sys3Label12.UseSubFont = true;
            this.sys3Label12.UseUnitFont = false;
            // 
            // tableLayoutPanel7
            // 
            this.tableLayoutPanel7.ColumnCount = 3;
            this.tableLayoutPanel7.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 60F));
            this.tableLayoutPanel7.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 20F));
            this.tableLayoutPanel7.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 20F));
            this.tableLayoutPanel7.Controls.Add(this.sys3button2, 0, 0);
            this.tableLayoutPanel7.Controls.Add(this.sys3button1, 0, 0);
            this.tableLayoutPanel7.Controls.Add(this.lbl_SendDataKey, 0, 0);
            this.tableLayoutPanel7.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel7.Location = new System.Drawing.Point(0, 64);
            this.tableLayoutPanel7.Margin = new System.Windows.Forms.Padding(0);
            this.tableLayoutPanel7.Name = "tableLayoutPanel7";
            this.tableLayoutPanel7.RowCount = 1;
            this.tableLayoutPanel7.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel7.Size = new System.Drawing.Size(602, 32);
            this.tableLayoutPanel7.TabIndex = 5;
            // 
            // sys3button2
            // 
            this.sys3button2.BorderWidth = 2;
            this.sys3button2.ButtonClicked = false;
            this.sys3button2.ClickedEmphasizeTextColor = System.Drawing.Color.White;
            this.sys3button2.CustomClickedGradientFirstColor = System.Drawing.Color.BlanchedAlmond;
            this.sys3button2.CustomClickedGradientSecondColor = System.Drawing.Color.Gold;
            this.sys3button2.Description = "";
            this.sys3button2.DisabledColor = System.Drawing.Color.DarkGray;
            this.sys3button2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.sys3button2.EdgeRadius = 5;
            this.sys3button2.GradientAngle = 60F;
            this.sys3button2.GradientFirstColor = System.Drawing.Color.Silver;
            this.sys3button2.GradientSecondColor = System.Drawing.Color.Gray;
            this.sys3button2.HoverEmphasizeCustomColor = System.Drawing.Color.Firebrick;
            this.sys3button2.ImagePosition = new System.Drawing.Point(37, 25);
            this.sys3button2.ImageSize = new System.Drawing.Point(30, 30);
            this.sys3button2.LoadImage = null;
            this.sys3button2.Location = new System.Drawing.Point(482, 1);
            this.sys3button2.MainFont = new System.Drawing.Font("맑은 고딕", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.sys3button2.MainFontColor = System.Drawing.Color.White;
            this.sys3button2.Margin = new System.Windows.Forms.Padding(1, 1, 0, 0);
            this.sys3button2.Name = "sys3button2";
            this.sys3button2.Size = new System.Drawing.Size(120, 31);
            this.sys3button2.SubFont = new System.Drawing.Font("맑은 고딕", 10F);
            this.sys3button2.SubFontColor = System.Drawing.Color.Black;
            this.sys3button2.SubText = "";
            this.sys3button2.TabIndex = 20847;
            this.sys3button2.Text = "CLEAR";
            this.sys3button2.TextAlignMain = Sys3Controls.EN_TEXTALIGN.MIDDLE_CENTER;
            this.sys3button2.TextAlignSub = Sys3Controls.EN_TEXTALIGN.MIDDLE_CENTER;
            this.sys3button2.ThemeIndex = 0;
            this.sys3button2.UseBorder = true;
            this.sys3button2.UseClickedEmphasizeTextColor = false;
            this.sys3button2.UseCustomizeClickedColor = true;
            this.sys3button2.UseEdge = true;
            this.sys3button2.UseHoverEmphasizeCustomColor = true;
            this.sys3button2.UseImage = true;
            this.sys3button2.UserHoverEmpahsize = true;
            this.sys3button2.UseSubFont = false;
            this.sys3button2.Visible = false;
            this.sys3button2.Click += new System.EventHandler(this.Click_SendDataClear);
            // 
            // sys3button1
            // 
            this.sys3button1.BorderWidth = 2;
            this.sys3button1.ButtonClicked = false;
            this.sys3button1.ClickedEmphasizeTextColor = System.Drawing.Color.White;
            this.sys3button1.CustomClickedGradientFirstColor = System.Drawing.Color.BlanchedAlmond;
            this.sys3button1.CustomClickedGradientSecondColor = System.Drawing.Color.Gold;
            this.sys3button1.Description = "";
            this.sys3button1.DisabledColor = System.Drawing.Color.DarkGray;
            this.sys3button1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.sys3button1.EdgeRadius = 5;
            this.sys3button1.GradientAngle = 60F;
            this.sys3button1.GradientFirstColor = System.Drawing.Color.Silver;
            this.sys3button1.GradientSecondColor = System.Drawing.Color.Gray;
            this.sys3button1.HoverEmphasizeCustomColor = System.Drawing.Color.Firebrick;
            this.sys3button1.ImagePosition = new System.Drawing.Point(37, 25);
            this.sys3button1.ImageSize = new System.Drawing.Point(30, 30);
            this.sys3button1.LoadImage = null;
            this.sys3button1.Location = new System.Drawing.Point(362, 1);
            this.sys3button1.MainFont = new System.Drawing.Font("맑은 고딕", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.sys3button1.MainFontColor = System.Drawing.Color.White;
            this.sys3button1.Margin = new System.Windows.Forms.Padding(1, 1, 0, 0);
            this.sys3button1.Name = "sys3button1";
            this.sys3button1.Size = new System.Drawing.Size(119, 31);
            this.sys3button1.SubFont = new System.Drawing.Font("맑은 고딕", 10F);
            this.sys3button1.SubFontColor = System.Drawing.Color.Black;
            this.sys3button1.SubText = "";
            this.sys3button1.TabIndex = 20847;
            this.sys3button1.Text = "ADD";
            this.sys3button1.TextAlignMain = Sys3Controls.EN_TEXTALIGN.MIDDLE_CENTER;
            this.sys3button1.TextAlignSub = Sys3Controls.EN_TEXTALIGN.MIDDLE_CENTER;
            this.sys3button1.ThemeIndex = 0;
            this.sys3button1.UseBorder = true;
            this.sys3button1.UseClickedEmphasizeTextColor = false;
            this.sys3button1.UseCustomizeClickedColor = true;
            this.sys3button1.UseEdge = true;
            this.sys3button1.UseHoverEmphasizeCustomColor = true;
            this.sys3button1.UseImage = true;
            this.sys3button1.UserHoverEmpahsize = true;
            this.sys3button1.UseSubFont = false;
            this.sys3button1.Visible = false;
            this.sys3button1.Click += new System.EventHandler(this.Click_SendDataAdd);
            // 
            // lbl_SendDataKey
            // 
            this.lbl_SendDataKey.BackGroundColor = System.Drawing.Color.White;
            this.lbl_SendDataKey.BorderStroke = 2;
            this.lbl_SendDataKey.BorderStyle = System.Windows.Forms.ButtonBorderStyle.Solid;
            this.lbl_SendDataKey.Description = "";
            this.lbl_SendDataKey.DisabledColor = System.Drawing.Color.DarkGray;
            this.lbl_SendDataKey.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lbl_SendDataKey.EdgeRadius = 1;
            this.lbl_SendDataKey.ImagePosition = new System.Drawing.Point(0, 0);
            this.lbl_SendDataKey.ImageSize = new System.Drawing.Point(0, 0);
            this.lbl_SendDataKey.LoadImage = null;
            this.lbl_SendDataKey.Location = new System.Drawing.Point(1, 1);
            this.lbl_SendDataKey.MainFont = new System.Drawing.Font("맑은 고딕", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.lbl_SendDataKey.MainFontColor = System.Drawing.Color.Black;
            this.lbl_SendDataKey.Margin = new System.Windows.Forms.Padding(1, 1, 0, 0);
            this.lbl_SendDataKey.Name = "lbl_SendDataKey";
            this.lbl_SendDataKey.Size = new System.Drawing.Size(360, 31);
            this.lbl_SendDataKey.SubFont = new System.Drawing.Font("맑은 고딕", 10F);
            this.lbl_SendDataKey.SubFontColor = System.Drawing.Color.Black;
            this.lbl_SendDataKey.SubText = "WorkTable";
            this.lbl_SendDataKey.TabIndex = 20849;
            this.lbl_SendDataKey.Text = "--";
            this.lbl_SendDataKey.TextAlignMain = Sys3Controls.EN_TEXTALIGN.MIDDLE_LEFT;
            this.lbl_SendDataKey.TextAlignSub = Sys3Controls.EN_TEXTALIGN.MIDDLE_CENTER;
            this.lbl_SendDataKey.TextAlignUnit = Sys3Controls.EN_TEXTALIGN.MIDDLE_CENTER;
            this.lbl_SendDataKey.ThemeIndex = 0;
            this.lbl_SendDataKey.UnitAreaRate = 30;
            this.lbl_SendDataKey.UnitFont = new System.Drawing.Font("맑은 고딕", 10F, System.Drawing.FontStyle.Bold);
            this.lbl_SendDataKey.UnitFontColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.lbl_SendDataKey.UnitPositionVertical = false;
            this.lbl_SendDataKey.UnitText = "";
            this.lbl_SendDataKey.UseBorder = true;
            this.lbl_SendDataKey.UseEdgeRadius = false;
            this.lbl_SendDataKey.UseImage = false;
            this.lbl_SendDataKey.UseSubFont = false;
            this.lbl_SendDataKey.UseUnitFont = false;
            this.lbl_SendDataKey.Visible = false;
            this.lbl_SendDataKey.Click += new System.EventHandler(this.Click_SendDataKey);
            // 
            // SubViewOperationSecsGemCommon
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.Controls.Add(this.tableLayoutPanel5);
            this.Margin = new System.Windows.Forms.Padding(0);
            this.Name = "SubViewOperationSecsGemCommon";
            this.Size = new System.Drawing.Size(1137, 588);
            this.Tag = "";
            this.tableLayoutPanel5.ResumeLayout(false);
            this.tableLayoutPanel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.gvMessageToSend)).EndInit();
            this.tableLayoutPanel4.ResumeLayout(false);
            this.tableLayoutPanel6.ResumeLayout(false);
            this.tableLayoutPanel7.ResumeLayout(false);
            this.ResumeLayout(false);

		}

        #endregion

        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel5;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.DataGridView gvMessageToSend;
        private System.Windows.Forms.DataGridViewTextBoxColumn col_DateName;
        private System.Windows.Forms.DataGridViewTextBoxColumn col_DataValue;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel4;
        private Sys3Controls.Sys3ToggleButton tglUseSecsgem;
        private Sys3Controls.Sys3Label sys3Label20;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel6;
        private Sys3Controls.Sys3button btnExecuteScenario;
        private Sys3Controls.Sys3Label lblScenarioToExecute;
        private Sys3Controls.Sys3Label sys3Label12;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel7;
        private Sys3Controls.Sys3button sys3button2;
        private Sys3Controls.Sys3button sys3button1;
        private Sys3Controls.Sys3Label lbl_SendDataKey;
    }
}
