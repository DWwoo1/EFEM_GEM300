namespace FrameOfSystem3.Views.Config
{
	partial class Config_Jog
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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle7 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle8 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle9 = new System.Windows.Forms.DataGridViewCellStyle();
            this.m_dgViewJog = new Sys3Controls.Sys3DoubleBufferedDataGridView();
            this.INDEX = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ID = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.AXIS = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ENABLE_LR = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.SOLUTIONCODE = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ENABLE_UD = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ENABLE_EX_LR = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column2 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ENABLE_EX_UD = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.m_JogGroupList = new Sys3Controls.Sys3GroupBoxContainer();
            this.m_groupSelectedItem = new Sys3Controls.Sys3GroupBoxContainer();
            this.tableLayoutPanel2 = new System.Windows.Forms.TableLayoutPanel();
            this.m_lblIndex = new Sys3Controls.Sys3Label();
            this.m_labelExtraAxis2 = new Sys3Controls.Sys3Label();
            this.m_labelExtraAxis1 = new Sys3Controls.Sys3Label();
            this.m_labelAxis2 = new Sys3Controls.Sys3Label();
            this.m_lblGroupName = new Sys3Controls.Sys3Label();
            this.m_labelGroupName = new Sys3Controls.Sys3Label();
            this.m_radioUseExtraAxisUD = new Sys3Controls.Sys3ToggleButton();
            this.m_labelAxis1 = new Sys3Controls.Sys3Label();
            this.m_radioUseExtraAxisRL = new Sys3Controls.Sys3ToggleButton();
            this.m_lblAxis1 = new Sys3Controls.Sys3Label();
            this.m_lblExtraAxis2 = new Sys3Controls.Sys3Label();
            this.m_radioUseAxisRL = new Sys3Controls.Sys3ToggleButton();
            this.m_lblExtraAxis1 = new Sys3Controls.Sys3Label();
            this.m_radioUseAxisUD = new Sys3Controls.Sys3ToggleButton();
            this.m_lblAxis2 = new Sys3Controls.Sys3Label();
            this.m_labelIndex = new Sys3Controls.Sys3Label();
            this.m_btnRemove = new Sys3Controls.Sys3button();
            this.m_btnAdd = new Sys3Controls.Sys3button();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.tableLayoutPanel3 = new System.Windows.Forms.TableLayoutPanel();
            ((System.ComponentModel.ISupportInitialize)(this.m_dgViewJog)).BeginInit();
            this.m_JogGroupList.SuspendLayout();
            this.m_groupSelectedItem.SuspendLayout();
            this.tableLayoutPanel2.SuspendLayout();
            this.tableLayoutPanel1.SuspendLayout();
            this.tableLayoutPanel3.SuspendLayout();
            this.SuspendLayout();
            // 
            // m_dgViewJog
            // 
            this.m_dgViewJog.AllowUserToAddRows = false;
            this.m_dgViewJog.AllowUserToDeleteRows = false;
            this.m_dgViewJog.AllowUserToResizeColumns = false;
            this.m_dgViewJog.AllowUserToResizeRows = false;
            this.m_dgViewJog.BackgroundColor = System.Drawing.Color.White;
            this.m_dgViewJog.CellBorderStyle = System.Windows.Forms.DataGridViewCellBorderStyle.Raised;
            this.m_dgViewJog.ColumnHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.Single;
            dataGridViewCellStyle7.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle7.BackColor = System.Drawing.Color.LightGray;
            dataGridViewCellStyle7.Font = new System.Drawing.Font("맑은 고딕", 11F, System.Drawing.FontStyle.Bold);
            dataGridViewCellStyle7.ForeColor = System.Drawing.Color.Black;
            dataGridViewCellStyle7.SelectionBackColor = System.Drawing.Color.White;
            dataGridViewCellStyle7.SelectionForeColor = System.Drawing.Color.Black;
            dataGridViewCellStyle7.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.m_dgViewJog.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle7;
            this.m_dgViewJog.ColumnHeadersHeight = 30;
            this.m_dgViewJog.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            this.m_dgViewJog.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.INDEX,
            this.ID,
            this.AXIS,
            this.ENABLE_LR,
            this.SOLUTIONCODE,
            this.ENABLE_UD,
            this.Column1,
            this.ENABLE_EX_LR,
            this.Column2,
            this.ENABLE_EX_UD});
            dataGridViewCellStyle8.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle8.BackColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle8.Font = new System.Drawing.Font("맑은 고딕", 10F, System.Drawing.FontStyle.Bold);
            dataGridViewCellStyle8.ForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle8.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(120)))), ((int)(((byte)(130)))), ((int)(((byte)(150)))));
            dataGridViewCellStyle8.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle8.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.m_dgViewJog.DefaultCellStyle = dataGridViewCellStyle8;
            this.m_dgViewJog.Dock = System.Windows.Forms.DockStyle.Fill;
            this.m_dgViewJog.EnableHeadersVisualStyles = false;
            this.m_dgViewJog.GridColor = System.Drawing.Color.FromArgb(((int)(((byte)(120)))), ((int)(((byte)(130)))), ((int)(((byte)(150)))));
            this.m_dgViewJog.Location = new System.Drawing.Point(5, 34);
            this.m_dgViewJog.Margin = new System.Windows.Forms.Padding(1, 1, 0, 0);
            this.m_dgViewJog.MultiSelect = false;
            this.m_dgViewJog.Name = "m_dgViewJog";
            this.m_dgViewJog.ReadOnly = true;
            this.m_dgViewJog.RowHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.None;
            dataGridViewCellStyle9.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle9.BackColor = System.Drawing.Color.White;
            dataGridViewCellStyle9.Font = new System.Drawing.Font("맑은 고딕", 11F);
            dataGridViewCellStyle9.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle9.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(120)))), ((int)(((byte)(130)))), ((int)(((byte)(150)))));
            dataGridViewCellStyle9.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle9.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.m_dgViewJog.RowHeadersDefaultCellStyle = dataGridViewCellStyle9;
            this.m_dgViewJog.RowHeadersVisible = false;
            this.m_dgViewJog.RowHeadersWidthSizeMode = System.Windows.Forms.DataGridViewRowHeadersWidthSizeMode.DisableResizing;
            this.m_dgViewJog.RowTemplate.Height = 30;
            this.m_dgViewJog.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.m_dgViewJog.Size = new System.Drawing.Size(1146, 587);
            this.m_dgViewJog.TabIndex = 1275;
            this.m_dgViewJog.CellClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.Click_Item);
            // 
            // INDEX
            // 
            this.INDEX.Frozen = true;
            this.INDEX.HeaderText = "INDEX";
            this.INDEX.Name = "INDEX";
            this.INDEX.ReadOnly = true;
            this.INDEX.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.INDEX.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.INDEX.Width = 70;
            // 
            // ID
            // 
            this.ID.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.ID.HeaderText = "GROUP NAME";
            this.ID.MaxInputLength = 20;
            this.ID.Name = "ID";
            this.ID.ReadOnly = true;
            this.ID.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.ID.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // AXIS
            // 
            this.AXIS.HeaderText = "AXIS(←, →)";
            this.AXIS.Name = "AXIS";
            this.AXIS.ReadOnly = true;
            this.AXIS.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.AXIS.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.AXIS.Width = 150;
            // 
            // ENABLE_LR
            // 
            this.ENABLE_LR.HeaderText = " ";
            this.ENABLE_LR.Name = "ENABLE_LR";
            this.ENABLE_LR.ReadOnly = true;
            this.ENABLE_LR.Width = 30;
            // 
            // SOLUTIONCODE
            // 
            this.SOLUTIONCODE.HeaderText = "AXIS(↑,↓)";
            this.SOLUTIONCODE.Name = "SOLUTIONCODE";
            this.SOLUTIONCODE.ReadOnly = true;
            this.SOLUTIONCODE.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.SOLUTIONCODE.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.SOLUTIONCODE.Width = 150;
            // 
            // ENABLE_UD
            // 
            this.ENABLE_UD.HeaderText = " ";
            this.ENABLE_UD.Name = "ENABLE_UD";
            this.ENABLE_UD.ReadOnly = true;
            this.ENABLE_UD.Width = 30;
            // 
            // Column1
            // 
            this.Column1.HeaderText = "EXTRA AXIS(←, →)";
            this.Column1.Name = "Column1";
            this.Column1.ReadOnly = true;
            this.Column1.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.Column1.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.Column1.Width = 150;
            // 
            // ENABLE_EX_LR
            // 
            this.ENABLE_EX_LR.HeaderText = " ";
            this.ENABLE_EX_LR.Name = "ENABLE_EX_LR";
            this.ENABLE_EX_LR.ReadOnly = true;
            this.ENABLE_EX_LR.Width = 30;
            // 
            // Column2
            // 
            this.Column2.HeaderText = "EXTRA AXIS(↑,↓)";
            this.Column2.Name = "Column2";
            this.Column2.ReadOnly = true;
            this.Column2.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.Column2.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.Column2.Width = 150;
            // 
            // ENABLE_EX_UD
            // 
            this.ENABLE_EX_UD.HeaderText = " ";
            this.ENABLE_EX_UD.Name = "ENABLE_EX_UD";
            this.ENABLE_EX_UD.ReadOnly = true;
            this.ENABLE_EX_UD.Width = 30;
            // 
            // m_JogGroupList
            // 
            this.m_JogGroupList.BackGroundColor = System.Drawing.Color.WhiteSmoke;
            this.m_JogGroupList.Controls.Add(this.m_dgViewJog);
            this.m_JogGroupList.Dock = System.Windows.Forms.DockStyle.Fill;
            this.m_JogGroupList.EdgeBorderStroke = 2;
            this.m_JogGroupList.EdgeRadius = 2;
            this.m_JogGroupList.LabelFont = new System.Drawing.Font("맑은 고딕", 10F, System.Drawing.FontStyle.Bold);
            this.m_JogGroupList.LabelGradientColorFirst = System.Drawing.Color.FromArgb(((int)(((byte)(208)))), ((int)(((byte)(208)))), ((int)(((byte)(208)))));
            this.m_JogGroupList.LabelGradientColorSecond = System.Drawing.Color.FromArgb(((int)(((byte)(240)))), ((int)(((byte)(240)))), ((int)(((byte)(240)))));
            this.m_JogGroupList.LabelHeight = 32;
            this.m_JogGroupList.LabelTextColor = System.Drawing.Color.Black;
            this.m_JogGroupList.Location = new System.Drawing.Point(0, 0);
            this.m_JogGroupList.Margin = new System.Windows.Forms.Padding(0);
            this.m_JogGroupList.Name = "m_JogGroupList";
            this.m_JogGroupList.Padding = new System.Windows.Forms.Padding(5, 20, 5, 5);
            this.m_JogGroupList.Size = new System.Drawing.Size(1156, 626);
            this.m_JogGroupList.TabIndex = 1372;
            this.m_JogGroupList.TabStop = false;
            this.m_JogGroupList.Text = "GROUP LIST";
            this.m_JogGroupList.TextAlign = Sys3Controls.EN_TEXTALIGN.MIDDLE_CENTER;
            this.m_JogGroupList.ThemeIndex = 0;
            this.m_JogGroupList.UseLabelBorder = true;
            this.m_JogGroupList.UseTitle = true;
            // 
            // m_groupSelectedItem
            // 
            this.m_groupSelectedItem.BackGroundColor = System.Drawing.Color.WhiteSmoke;
            this.m_groupSelectedItem.Controls.Add(this.tableLayoutPanel2);
            this.m_groupSelectedItem.Dock = System.Windows.Forms.DockStyle.Fill;
            this.m_groupSelectedItem.EdgeBorderStroke = 2;
            this.m_groupSelectedItem.EdgeRadius = 2;
            this.m_groupSelectedItem.LabelFont = new System.Drawing.Font("맑은 고딕", 10F, System.Drawing.FontStyle.Bold);
            this.m_groupSelectedItem.LabelGradientColorFirst = System.Drawing.Color.FromArgb(((int)(((byte)(208)))), ((int)(((byte)(208)))), ((int)(((byte)(208)))));
            this.m_groupSelectedItem.LabelGradientColorSecond = System.Drawing.Color.FromArgb(((int)(((byte)(240)))), ((int)(((byte)(240)))), ((int)(((byte)(240)))));
            this.m_groupSelectedItem.LabelHeight = 32;
            this.m_groupSelectedItem.LabelTextColor = System.Drawing.Color.Black;
            this.m_groupSelectedItem.Location = new System.Drawing.Point(0, 626);
            this.m_groupSelectedItem.Margin = new System.Windows.Forms.Padding(0);
            this.m_groupSelectedItem.Name = "m_groupSelectedItem";
            this.m_groupSelectedItem.Padding = new System.Windows.Forms.Padding(5, 20, 5, 5);
            this.m_groupSelectedItem.Size = new System.Drawing.Size(1156, 225);
            this.m_groupSelectedItem.TabIndex = 1373;
            this.m_groupSelectedItem.TabStop = false;
            this.m_groupSelectedItem.Text = "CONFIGURATION";
            this.m_groupSelectedItem.TextAlign = Sys3Controls.EN_TEXTALIGN.MIDDLE_CENTER;
            this.m_groupSelectedItem.ThemeIndex = 0;
            this.m_groupSelectedItem.UseLabelBorder = true;
            this.m_groupSelectedItem.UseTitle = true;
            // 
            // tableLayoutPanel2
            // 
            this.tableLayoutPanel2.ColumnCount = 9;
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 3.203291F));
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 12.81317F));
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 8.118423F));
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 25.37007F));
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 0.990099F));
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 12.81317F));
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 8.118423F));
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 25.37007F));
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 3.203292F));
            this.tableLayoutPanel2.Controls.Add(this.m_lblIndex, 1, 1);
            this.tableLayoutPanel2.Controls.Add(this.m_labelExtraAxis2, 7, 4);
            this.tableLayoutPanel2.Controls.Add(this.m_labelExtraAxis1, 7, 3);
            this.tableLayoutPanel2.Controls.Add(this.m_labelIndex, 2, 1);
            this.tableLayoutPanel2.Controls.Add(this.m_labelAxis2, 3, 4);
            this.tableLayoutPanel2.Controls.Add(this.m_lblGroupName, 1, 2);
            this.tableLayoutPanel2.Controls.Add(this.m_labelGroupName, 2, 2);
            this.tableLayoutPanel2.Controls.Add(this.m_radioUseExtraAxisUD, 6, 4);
            this.tableLayoutPanel2.Controls.Add(this.m_labelAxis1, 3, 3);
            this.tableLayoutPanel2.Controls.Add(this.m_radioUseExtraAxisRL, 6, 3);
            this.tableLayoutPanel2.Controls.Add(this.m_lblAxis1, 1, 3);
            this.tableLayoutPanel2.Controls.Add(this.m_lblExtraAxis2, 5, 4);
            this.tableLayoutPanel2.Controls.Add(this.m_radioUseAxisRL, 2, 3);
            this.tableLayoutPanel2.Controls.Add(this.m_lblExtraAxis1, 5, 3);
            this.tableLayoutPanel2.Controls.Add(this.m_radioUseAxisUD, 2, 4);
            this.tableLayoutPanel2.Controls.Add(this.m_lblAxis2, 1, 4);
            this.tableLayoutPanel2.Controls.Add(this.tableLayoutPanel3, 7, 1);
            this.tableLayoutPanel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel2.Location = new System.Drawing.Point(5, 34);
            this.tableLayoutPanel2.Name = "tableLayoutPanel2";
            this.tableLayoutPanel2.RowCount = 6;
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 4.669085F));
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 22.66545F));
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 22.66546F));
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 22.66546F));
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 22.66546F));
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 4.669085F));
            this.tableLayoutPanel2.Size = new System.Drawing.Size(1146, 186);
            this.tableLayoutPanel2.TabIndex = 1381;
            // 
            // m_lblIndex
            // 
            this.m_lblIndex.BackGroundColor = System.Drawing.Color.FromArgb(((int)(((byte)(200)))), ((int)(((byte)(200)))), ((int)(((byte)(220)))));
            this.m_lblIndex.BorderStroke = 2;
            this.m_lblIndex.BorderStyle = System.Windows.Forms.ButtonBorderStyle.Solid;
            this.m_lblIndex.Description = "";
            this.m_lblIndex.DisabledColor = System.Drawing.Color.DarkGray;
            this.m_lblIndex.Dock = System.Windows.Forms.DockStyle.Fill;
            this.m_lblIndex.EdgeRadius = 1;
            this.m_lblIndex.ImagePosition = new System.Drawing.Point(0, 0);
            this.m_lblIndex.ImageSize = new System.Drawing.Point(0, 0);
            this.m_lblIndex.LoadImage = null;
            this.m_lblIndex.Location = new System.Drawing.Point(37, 9);
            this.m_lblIndex.MainFont = new System.Drawing.Font("맑은 고딕", 9F, System.Drawing.FontStyle.Bold);
            this.m_lblIndex.MainFontColor = System.Drawing.Color.Black;
            this.m_lblIndex.Margin = new System.Windows.Forms.Padding(1, 1, 0, 0);
            this.m_lblIndex.Name = "m_lblIndex";
            this.m_lblIndex.Size = new System.Drawing.Size(145, 41);
            this.m_lblIndex.SubFont = new System.Drawing.Font("맑은 고딕", 10F);
            this.m_lblIndex.SubFontColor = System.Drawing.Color.Black;
            this.m_lblIndex.SubText = "";
            this.m_lblIndex.TabIndex = 1376;
            this.m_lblIndex.Text = "KEY";
            this.m_lblIndex.TextAlignMain = Sys3Controls.EN_TEXTALIGN.TOP_LEFT;
            this.m_lblIndex.TextAlignSub = Sys3Controls.EN_TEXTALIGN.MIDDLE_CENTER;
            this.m_lblIndex.TextAlignUnit = Sys3Controls.EN_TEXTALIGN.MIDDLE_CENTER;
            this.m_lblIndex.ThemeIndex = 0;
            this.m_lblIndex.UnitAreaRate = 40;
            this.m_lblIndex.UnitFont = new System.Drawing.Font("맑은 고딕", 10F, System.Drawing.FontStyle.Bold);
            this.m_lblIndex.UnitFontColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.m_lblIndex.UnitPositionVertical = false;
            this.m_lblIndex.UnitText = "";
            this.m_lblIndex.UseBorder = true;
            this.m_lblIndex.UseEdgeRadius = false;
            this.m_lblIndex.UseImage = false;
            this.m_lblIndex.UseSubFont = false;
            this.m_lblIndex.UseUnitFont = false;
            // 
            // m_labelExtraAxis2
            // 
            this.m_labelExtraAxis2.BackGroundColor = System.Drawing.Color.White;
            this.m_labelExtraAxis2.BorderStroke = 2;
            this.m_labelExtraAxis2.BorderStyle = System.Windows.Forms.ButtonBorderStyle.Solid;
            this.m_labelExtraAxis2.Description = "";
            this.m_labelExtraAxis2.DisabledColor = System.Drawing.Color.LightGray;
            this.m_labelExtraAxis2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.m_labelExtraAxis2.EdgeRadius = 1;
            this.m_labelExtraAxis2.Enabled = false;
            this.m_labelExtraAxis2.ImagePosition = new System.Drawing.Point(0, 0);
            this.m_labelExtraAxis2.ImageSize = new System.Drawing.Point(0, 0);
            this.m_labelExtraAxis2.LoadImage = null;
            this.m_labelExtraAxis2.Location = new System.Drawing.Point(816, 135);
            this.m_labelExtraAxis2.MainFont = new System.Drawing.Font("맑은 고딕", 11F, System.Drawing.FontStyle.Bold);
            this.m_labelExtraAxis2.MainFontColor = System.Drawing.Color.Black;
            this.m_labelExtraAxis2.Margin = new System.Windows.Forms.Padding(1, 1, 0, 0);
            this.m_labelExtraAxis2.Name = "m_labelExtraAxis2";
            this.m_labelExtraAxis2.Size = new System.Drawing.Size(289, 41);
            this.m_labelExtraAxis2.SubFont = new System.Drawing.Font("맑은 고딕", 10F, System.Drawing.FontStyle.Bold);
            this.m_labelExtraAxis2.SubFontColor = System.Drawing.Color.Black;
            this.m_labelExtraAxis2.SubText = "[ -1 ]";
            this.m_labelExtraAxis2.TabIndex = 4;
            this.m_labelExtraAxis2.Text = "--";
            this.m_labelExtraAxis2.TextAlignMain = Sys3Controls.EN_TEXTALIGN.BOTTOM_RIGHT;
            this.m_labelExtraAxis2.TextAlignSub = Sys3Controls.EN_TEXTALIGN.TOP_LEFT;
            this.m_labelExtraAxis2.TextAlignUnit = Sys3Controls.EN_TEXTALIGN.MIDDLE_CENTER;
            this.m_labelExtraAxis2.ThemeIndex = 0;
            this.m_labelExtraAxis2.UnitAreaRate = 40;
            this.m_labelExtraAxis2.UnitFont = new System.Drawing.Font("맑은 고딕", 10F, System.Drawing.FontStyle.Bold);
            this.m_labelExtraAxis2.UnitFontColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.m_labelExtraAxis2.UnitPositionVertical = false;
            this.m_labelExtraAxis2.UnitText = "";
            this.m_labelExtraAxis2.UseBorder = true;
            this.m_labelExtraAxis2.UseEdgeRadius = false;
            this.m_labelExtraAxis2.UseImage = false;
            this.m_labelExtraAxis2.UseSubFont = true;
            this.m_labelExtraAxis2.UseUnitFont = false;
            this.m_labelExtraAxis2.Click += new System.EventHandler(this.Click_Labels);
            // 
            // m_labelExtraAxis1
            // 
            this.m_labelExtraAxis1.BackGroundColor = System.Drawing.Color.White;
            this.m_labelExtraAxis1.BorderStroke = 2;
            this.m_labelExtraAxis1.BorderStyle = System.Windows.Forms.ButtonBorderStyle.Solid;
            this.m_labelExtraAxis1.Description = "";
            this.m_labelExtraAxis1.DisabledColor = System.Drawing.Color.LightGray;
            this.m_labelExtraAxis1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.m_labelExtraAxis1.EdgeRadius = 1;
            this.m_labelExtraAxis1.Enabled = false;
            this.m_labelExtraAxis1.ImagePosition = new System.Drawing.Point(0, 0);
            this.m_labelExtraAxis1.ImageSize = new System.Drawing.Point(0, 0);
            this.m_labelExtraAxis1.LoadImage = null;
            this.m_labelExtraAxis1.Location = new System.Drawing.Point(816, 93);
            this.m_labelExtraAxis1.MainFont = new System.Drawing.Font("맑은 고딕", 11F, System.Drawing.FontStyle.Bold);
            this.m_labelExtraAxis1.MainFontColor = System.Drawing.Color.Black;
            this.m_labelExtraAxis1.Margin = new System.Windows.Forms.Padding(1, 1, 0, 0);
            this.m_labelExtraAxis1.Name = "m_labelExtraAxis1";
            this.m_labelExtraAxis1.Size = new System.Drawing.Size(289, 41);
            this.m_labelExtraAxis1.SubFont = new System.Drawing.Font("맑은 고딕", 10F, System.Drawing.FontStyle.Bold);
            this.m_labelExtraAxis1.SubFontColor = System.Drawing.Color.Black;
            this.m_labelExtraAxis1.SubText = "[ -1 ]";
            this.m_labelExtraAxis1.TabIndex = 3;
            this.m_labelExtraAxis1.Text = "--";
            this.m_labelExtraAxis1.TextAlignMain = Sys3Controls.EN_TEXTALIGN.BOTTOM_RIGHT;
            this.m_labelExtraAxis1.TextAlignSub = Sys3Controls.EN_TEXTALIGN.TOP_LEFT;
            this.m_labelExtraAxis1.TextAlignUnit = Sys3Controls.EN_TEXTALIGN.MIDDLE_CENTER;
            this.m_labelExtraAxis1.ThemeIndex = 0;
            this.m_labelExtraAxis1.UnitAreaRate = 40;
            this.m_labelExtraAxis1.UnitFont = new System.Drawing.Font("맑은 고딕", 10F, System.Drawing.FontStyle.Bold);
            this.m_labelExtraAxis1.UnitFontColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.m_labelExtraAxis1.UnitPositionVertical = false;
            this.m_labelExtraAxis1.UnitText = "";
            this.m_labelExtraAxis1.UseBorder = true;
            this.m_labelExtraAxis1.UseEdgeRadius = false;
            this.m_labelExtraAxis1.UseImage = false;
            this.m_labelExtraAxis1.UseSubFont = true;
            this.m_labelExtraAxis1.UseUnitFont = false;
            this.m_labelExtraAxis1.Click += new System.EventHandler(this.Click_Labels);
            // 
            // m_labelAxis2
            // 
            this.m_labelAxis2.BackGroundColor = System.Drawing.Color.White;
            this.m_labelAxis2.BorderStroke = 2;
            this.m_labelAxis2.BorderStyle = System.Windows.Forms.ButtonBorderStyle.Solid;
            this.m_labelAxis2.Description = "";
            this.m_labelAxis2.DisabledColor = System.Drawing.Color.LightGray;
            this.m_labelAxis2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.m_labelAxis2.EdgeRadius = 1;
            this.m_labelAxis2.Enabled = false;
            this.m_labelAxis2.ImagePosition = new System.Drawing.Point(0, 0);
            this.m_labelAxis2.ImageSize = new System.Drawing.Point(0, 0);
            this.m_labelAxis2.LoadImage = null;
            this.m_labelAxis2.Location = new System.Drawing.Point(276, 135);
            this.m_labelAxis2.MainFont = new System.Drawing.Font("맑은 고딕", 11F, System.Drawing.FontStyle.Bold);
            this.m_labelAxis2.MainFontColor = System.Drawing.Color.Black;
            this.m_labelAxis2.Margin = new System.Windows.Forms.Padding(1, 1, 0, 0);
            this.m_labelAxis2.Name = "m_labelAxis2";
            this.m_labelAxis2.Size = new System.Drawing.Size(289, 41);
            this.m_labelAxis2.SubFont = new System.Drawing.Font("맑은 고딕", 10F, System.Drawing.FontStyle.Bold);
            this.m_labelAxis2.SubFontColor = System.Drawing.Color.Black;
            this.m_labelAxis2.SubText = "[ -1 ]";
            this.m_labelAxis2.TabIndex = 2;
            this.m_labelAxis2.Text = "--";
            this.m_labelAxis2.TextAlignMain = Sys3Controls.EN_TEXTALIGN.BOTTOM_RIGHT;
            this.m_labelAxis2.TextAlignSub = Sys3Controls.EN_TEXTALIGN.TOP_LEFT;
            this.m_labelAxis2.TextAlignUnit = Sys3Controls.EN_TEXTALIGN.MIDDLE_CENTER;
            this.m_labelAxis2.ThemeIndex = 0;
            this.m_labelAxis2.UnitAreaRate = 40;
            this.m_labelAxis2.UnitFont = new System.Drawing.Font("맑은 고딕", 10F, System.Drawing.FontStyle.Bold);
            this.m_labelAxis2.UnitFontColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.m_labelAxis2.UnitPositionVertical = false;
            this.m_labelAxis2.UnitText = "";
            this.m_labelAxis2.UseBorder = true;
            this.m_labelAxis2.UseEdgeRadius = false;
            this.m_labelAxis2.UseImage = false;
            this.m_labelAxis2.UseSubFont = true;
            this.m_labelAxis2.UseUnitFont = false;
            this.m_labelAxis2.Click += new System.EventHandler(this.Click_Labels);
            // 
            // m_lblGroupName
            // 
            this.m_lblGroupName.BackGroundColor = System.Drawing.Color.FromArgb(((int)(((byte)(200)))), ((int)(((byte)(200)))), ((int)(((byte)(220)))));
            this.m_lblGroupName.BorderStroke = 2;
            this.m_lblGroupName.BorderStyle = System.Windows.Forms.ButtonBorderStyle.Solid;
            this.m_lblGroupName.Description = "";
            this.m_lblGroupName.DisabledColor = System.Drawing.Color.DarkGray;
            this.m_lblGroupName.Dock = System.Windows.Forms.DockStyle.Fill;
            this.m_lblGroupName.EdgeRadius = 1;
            this.m_lblGroupName.ImagePosition = new System.Drawing.Point(0, 0);
            this.m_lblGroupName.ImageSize = new System.Drawing.Point(0, 0);
            this.m_lblGroupName.LoadImage = null;
            this.m_lblGroupName.Location = new System.Drawing.Point(37, 51);
            this.m_lblGroupName.MainFont = new System.Drawing.Font("맑은 고딕", 9F, System.Drawing.FontStyle.Bold);
            this.m_lblGroupName.MainFontColor = System.Drawing.Color.Black;
            this.m_lblGroupName.Margin = new System.Windows.Forms.Padding(1, 1, 0, 0);
            this.m_lblGroupName.Name = "m_lblGroupName";
            this.m_lblGroupName.Size = new System.Drawing.Size(145, 41);
            this.m_lblGroupName.SubFont = new System.Drawing.Font("맑은 고딕", 10F);
            this.m_lblGroupName.SubFontColor = System.Drawing.Color.Black;
            this.m_lblGroupName.SubText = "";
            this.m_lblGroupName.TabIndex = 1376;
            this.m_lblGroupName.Text = "GROUP NAME";
            this.m_lblGroupName.TextAlignMain = Sys3Controls.EN_TEXTALIGN.TOP_LEFT;
            this.m_lblGroupName.TextAlignSub = Sys3Controls.EN_TEXTALIGN.MIDDLE_CENTER;
            this.m_lblGroupName.TextAlignUnit = Sys3Controls.EN_TEXTALIGN.MIDDLE_CENTER;
            this.m_lblGroupName.ThemeIndex = 0;
            this.m_lblGroupName.UnitAreaRate = 40;
            this.m_lblGroupName.UnitFont = new System.Drawing.Font("맑은 고딕", 10F, System.Drawing.FontStyle.Bold);
            this.m_lblGroupName.UnitFontColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.m_lblGroupName.UnitPositionVertical = false;
            this.m_lblGroupName.UnitText = "";
            this.m_lblGroupName.UseBorder = true;
            this.m_lblGroupName.UseEdgeRadius = false;
            this.m_lblGroupName.UseImage = false;
            this.m_lblGroupName.UseSubFont = false;
            this.m_lblGroupName.UseUnitFont = false;
            // 
            // m_labelGroupName
            // 
            this.m_labelGroupName.BackGroundColor = System.Drawing.Color.White;
            this.m_labelGroupName.BorderStroke = 2;
            this.m_labelGroupName.BorderStyle = System.Windows.Forms.ButtonBorderStyle.Solid;
            this.tableLayoutPanel2.SetColumnSpan(this.m_labelGroupName, 2);
            this.m_labelGroupName.Description = "";
            this.m_labelGroupName.DisabledColor = System.Drawing.Color.LightGray;
            this.m_labelGroupName.Dock = System.Windows.Forms.DockStyle.Fill;
            this.m_labelGroupName.EdgeRadius = 1;
            this.m_labelGroupName.Enabled = false;
            this.m_labelGroupName.ImagePosition = new System.Drawing.Point(0, 0);
            this.m_labelGroupName.ImageSize = new System.Drawing.Point(0, 0);
            this.m_labelGroupName.LoadImage = null;
            this.m_labelGroupName.Location = new System.Drawing.Point(183, 51);
            this.m_labelGroupName.MainFont = new System.Drawing.Font("맑은 고딕", 11F, System.Drawing.FontStyle.Bold);
            this.m_labelGroupName.MainFontColor = System.Drawing.Color.Black;
            this.m_labelGroupName.Margin = new System.Windows.Forms.Padding(1, 1, 0, 0);
            this.m_labelGroupName.Name = "m_labelGroupName";
            this.m_labelGroupName.Size = new System.Drawing.Size(382, 41);
            this.m_labelGroupName.SubFont = new System.Drawing.Font("맑은 고딕", 10F, System.Drawing.FontStyle.Bold);
            this.m_labelGroupName.SubFontColor = System.Drawing.Color.Black;
            this.m_labelGroupName.SubText = "[ -1 ]";
            this.m_labelGroupName.TabIndex = 0;
            this.m_labelGroupName.TextAlignMain = Sys3Controls.EN_TEXTALIGN.BOTTOM_RIGHT;
            this.m_labelGroupName.TextAlignSub = Sys3Controls.EN_TEXTALIGN.TOP_LEFT;
            this.m_labelGroupName.TextAlignUnit = Sys3Controls.EN_TEXTALIGN.MIDDLE_CENTER;
            this.m_labelGroupName.ThemeIndex = 0;
            this.m_labelGroupName.UnitAreaRate = 40;
            this.m_labelGroupName.UnitFont = new System.Drawing.Font("맑은 고딕", 10F, System.Drawing.FontStyle.Bold);
            this.m_labelGroupName.UnitFontColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.m_labelGroupName.UnitPositionVertical = false;
            this.m_labelGroupName.UnitText = "";
            this.m_labelGroupName.UseBorder = true;
            this.m_labelGroupName.UseEdgeRadius = false;
            this.m_labelGroupName.UseImage = false;
            this.m_labelGroupName.UseSubFont = false;
            this.m_labelGroupName.UseUnitFont = false;
            this.m_labelGroupName.Click += new System.EventHandler(this.Click_Labels);
            // 
            // m_radioUseExtraAxisUD
            // 
            this.m_radioUseExtraAxisUD.Active = false;
            this.m_radioUseExtraAxisUD.ActiveColorFirst = System.Drawing.Color.RoyalBlue;
            this.m_radioUseExtraAxisUD.ActiveColorSecond = System.Drawing.Color.DodgerBlue;
            this.m_radioUseExtraAxisUD.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.m_radioUseExtraAxisUD.BackColor = System.Drawing.Color.WhiteSmoke;
            this.m_radioUseExtraAxisUD.Location = new System.Drawing.Point(724, 136);
            this.m_radioUseExtraAxisUD.Margin = new System.Windows.Forms.Padding(2);
            this.m_radioUseExtraAxisUD.Name = "m_radioUseExtraAxisUD";
            this.m_radioUseExtraAxisUD.NormalColorFirst = System.Drawing.Color.DarkGray;
            this.m_radioUseExtraAxisUD.NormalColorSecond = System.Drawing.Color.Silver;
            this.m_radioUseExtraAxisUD.Size = new System.Drawing.Size(76, 38);
            this.m_radioUseExtraAxisUD.TabIndex = 3;
            this.m_radioUseExtraAxisUD.Click += new System.EventHandler(this.Click_UseOption);
            this.m_radioUseExtraAxisUD.DoubleClick += new System.EventHandler(this.Click_UseOption);
            // 
            // m_labelAxis1
            // 
            this.m_labelAxis1.BackGroundColor = System.Drawing.Color.White;
            this.m_labelAxis1.BorderStroke = 2;
            this.m_labelAxis1.BorderStyle = System.Windows.Forms.ButtonBorderStyle.Solid;
            this.m_labelAxis1.Description = "";
            this.m_labelAxis1.DisabledColor = System.Drawing.Color.LightGray;
            this.m_labelAxis1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.m_labelAxis1.EdgeRadius = 1;
            this.m_labelAxis1.Enabled = false;
            this.m_labelAxis1.ImagePosition = new System.Drawing.Point(0, 0);
            this.m_labelAxis1.ImageSize = new System.Drawing.Point(0, 0);
            this.m_labelAxis1.LoadImage = null;
            this.m_labelAxis1.Location = new System.Drawing.Point(276, 93);
            this.m_labelAxis1.MainFont = new System.Drawing.Font("맑은 고딕", 11F, System.Drawing.FontStyle.Bold);
            this.m_labelAxis1.MainFontColor = System.Drawing.Color.Black;
            this.m_labelAxis1.Margin = new System.Windows.Forms.Padding(1, 1, 0, 0);
            this.m_labelAxis1.Name = "m_labelAxis1";
            this.m_labelAxis1.Size = new System.Drawing.Size(289, 41);
            this.m_labelAxis1.SubFont = new System.Drawing.Font("맑은 고딕", 10F, System.Drawing.FontStyle.Bold);
            this.m_labelAxis1.SubFontColor = System.Drawing.Color.Black;
            this.m_labelAxis1.SubText = "[ -1 ]";
            this.m_labelAxis1.TabIndex = 1;
            this.m_labelAxis1.Text = "--";
            this.m_labelAxis1.TextAlignMain = Sys3Controls.EN_TEXTALIGN.BOTTOM_RIGHT;
            this.m_labelAxis1.TextAlignSub = Sys3Controls.EN_TEXTALIGN.TOP_LEFT;
            this.m_labelAxis1.TextAlignUnit = Sys3Controls.EN_TEXTALIGN.MIDDLE_CENTER;
            this.m_labelAxis1.ThemeIndex = 0;
            this.m_labelAxis1.UnitAreaRate = 40;
            this.m_labelAxis1.UnitFont = new System.Drawing.Font("맑은 고딕", 10F, System.Drawing.FontStyle.Bold);
            this.m_labelAxis1.UnitFontColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.m_labelAxis1.UnitPositionVertical = false;
            this.m_labelAxis1.UnitText = "";
            this.m_labelAxis1.UseBorder = true;
            this.m_labelAxis1.UseEdgeRadius = false;
            this.m_labelAxis1.UseImage = false;
            this.m_labelAxis1.UseSubFont = true;
            this.m_labelAxis1.UseUnitFont = false;
            this.m_labelAxis1.Click += new System.EventHandler(this.Click_Labels);
            // 
            // m_radioUseExtraAxisRL
            // 
            this.m_radioUseExtraAxisRL.Active = false;
            this.m_radioUseExtraAxisRL.ActiveColorFirst = System.Drawing.Color.RoyalBlue;
            this.m_radioUseExtraAxisRL.ActiveColorSecond = System.Drawing.Color.DodgerBlue;
            this.m_radioUseExtraAxisRL.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.m_radioUseExtraAxisRL.BackColor = System.Drawing.Color.WhiteSmoke;
            this.m_radioUseExtraAxisRL.Location = new System.Drawing.Point(724, 94);
            this.m_radioUseExtraAxisRL.Margin = new System.Windows.Forms.Padding(2);
            this.m_radioUseExtraAxisRL.Name = "m_radioUseExtraAxisRL";
            this.m_radioUseExtraAxisRL.NormalColorFirst = System.Drawing.Color.DarkGray;
            this.m_radioUseExtraAxisRL.NormalColorSecond = System.Drawing.Color.Silver;
            this.m_radioUseExtraAxisRL.Size = new System.Drawing.Size(76, 38);
            this.m_radioUseExtraAxisRL.TabIndex = 2;
            this.m_radioUseExtraAxisRL.Click += new System.EventHandler(this.Click_UseOption);
            this.m_radioUseExtraAxisRL.DoubleClick += new System.EventHandler(this.Click_UseOption);
            // 
            // m_lblAxis1
            // 
            this.m_lblAxis1.BackGroundColor = System.Drawing.Color.FromArgb(((int)(((byte)(200)))), ((int)(((byte)(200)))), ((int)(((byte)(220)))));
            this.m_lblAxis1.BorderStroke = 2;
            this.m_lblAxis1.BorderStyle = System.Windows.Forms.ButtonBorderStyle.Solid;
            this.m_lblAxis1.Description = "";
            this.m_lblAxis1.DisabledColor = System.Drawing.Color.DarkGray;
            this.m_lblAxis1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.m_lblAxis1.EdgeRadius = 1;
            this.m_lblAxis1.ImagePosition = new System.Drawing.Point(0, 0);
            this.m_lblAxis1.ImageSize = new System.Drawing.Point(0, 0);
            this.m_lblAxis1.LoadImage = null;
            this.m_lblAxis1.Location = new System.Drawing.Point(37, 93);
            this.m_lblAxis1.MainFont = new System.Drawing.Font("맑은 고딕", 9F, System.Drawing.FontStyle.Bold);
            this.m_lblAxis1.MainFontColor = System.Drawing.Color.Black;
            this.m_lblAxis1.Margin = new System.Windows.Forms.Padding(1, 1, 0, 0);
            this.m_lblAxis1.Name = "m_lblAxis1";
            this.m_lblAxis1.Size = new System.Drawing.Size(145, 41);
            this.m_lblAxis1.SubFont = new System.Drawing.Font("맑은 고딕", 9F, System.Drawing.FontStyle.Bold);
            this.m_lblAxis1.SubFontColor = System.Drawing.Color.Blue;
            this.m_lblAxis1.SubText = "MOTION INDEX";
            this.m_lblAxis1.TabIndex = 1376;
            this.m_lblAxis1.Text = "AXIS ← →";
            this.m_lblAxis1.TextAlignMain = Sys3Controls.EN_TEXTALIGN.TOP_LEFT;
            this.m_lblAxis1.TextAlignSub = Sys3Controls.EN_TEXTALIGN.BOTTOM_RIGHT;
            this.m_lblAxis1.TextAlignUnit = Sys3Controls.EN_TEXTALIGN.MIDDLE_CENTER;
            this.m_lblAxis1.ThemeIndex = 0;
            this.m_lblAxis1.UnitAreaRate = 40;
            this.m_lblAxis1.UnitFont = new System.Drawing.Font("맑은 고딕", 10F, System.Drawing.FontStyle.Bold);
            this.m_lblAxis1.UnitFontColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.m_lblAxis1.UnitPositionVertical = false;
            this.m_lblAxis1.UnitText = "";
            this.m_lblAxis1.UseBorder = true;
            this.m_lblAxis1.UseEdgeRadius = false;
            this.m_lblAxis1.UseImage = false;
            this.m_lblAxis1.UseSubFont = true;
            this.m_lblAxis1.UseUnitFont = false;
            // 
            // m_lblExtraAxis2
            // 
            this.m_lblExtraAxis2.BackGroundColor = System.Drawing.Color.FromArgb(((int)(((byte)(200)))), ((int)(((byte)(200)))), ((int)(((byte)(220)))));
            this.m_lblExtraAxis2.BorderStroke = 2;
            this.m_lblExtraAxis2.BorderStyle = System.Windows.Forms.ButtonBorderStyle.Solid;
            this.m_lblExtraAxis2.Description = "";
            this.m_lblExtraAxis2.DisabledColor = System.Drawing.Color.DarkGray;
            this.m_lblExtraAxis2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.m_lblExtraAxis2.EdgeRadius = 1;
            this.m_lblExtraAxis2.ImagePosition = new System.Drawing.Point(0, 0);
            this.m_lblExtraAxis2.ImageSize = new System.Drawing.Point(0, 0);
            this.m_lblExtraAxis2.LoadImage = null;
            this.m_lblExtraAxis2.Location = new System.Drawing.Point(577, 135);
            this.m_lblExtraAxis2.MainFont = new System.Drawing.Font("맑은 고딕", 9F, System.Drawing.FontStyle.Bold);
            this.m_lblExtraAxis2.MainFontColor = System.Drawing.Color.Black;
            this.m_lblExtraAxis2.Margin = new System.Windows.Forms.Padding(1, 1, 0, 0);
            this.m_lblExtraAxis2.Name = "m_lblExtraAxis2";
            this.m_lblExtraAxis2.Size = new System.Drawing.Size(145, 41);
            this.m_lblExtraAxis2.SubFont = new System.Drawing.Font("맑은 고딕", 9F, System.Drawing.FontStyle.Bold);
            this.m_lblExtraAxis2.SubFontColor = System.Drawing.Color.Blue;
            this.m_lblExtraAxis2.SubText = "MOTION INDEX";
            this.m_lblExtraAxis2.TabIndex = 1376;
            this.m_lblExtraAxis2.Text = "EXTRA AXIS↑↓";
            this.m_lblExtraAxis2.TextAlignMain = Sys3Controls.EN_TEXTALIGN.TOP_LEFT;
            this.m_lblExtraAxis2.TextAlignSub = Sys3Controls.EN_TEXTALIGN.BOTTOM_RIGHT;
            this.m_lblExtraAxis2.TextAlignUnit = Sys3Controls.EN_TEXTALIGN.MIDDLE_CENTER;
            this.m_lblExtraAxis2.ThemeIndex = 0;
            this.m_lblExtraAxis2.UnitAreaRate = 40;
            this.m_lblExtraAxis2.UnitFont = new System.Drawing.Font("맑은 고딕", 10F, System.Drawing.FontStyle.Bold);
            this.m_lblExtraAxis2.UnitFontColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.m_lblExtraAxis2.UnitPositionVertical = false;
            this.m_lblExtraAxis2.UnitText = "";
            this.m_lblExtraAxis2.UseBorder = true;
            this.m_lblExtraAxis2.UseEdgeRadius = false;
            this.m_lblExtraAxis2.UseImage = false;
            this.m_lblExtraAxis2.UseSubFont = true;
            this.m_lblExtraAxis2.UseUnitFont = false;
            // 
            // m_radioUseAxisRL
            // 
            this.m_radioUseAxisRL.Active = false;
            this.m_radioUseAxisRL.ActiveColorFirst = System.Drawing.Color.RoyalBlue;
            this.m_radioUseAxisRL.ActiveColorSecond = System.Drawing.Color.DodgerBlue;
            this.m_radioUseAxisRL.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.m_radioUseAxisRL.BackColor = System.Drawing.Color.WhiteSmoke;
            this.m_radioUseAxisRL.Location = new System.Drawing.Point(184, 94);
            this.m_radioUseAxisRL.Margin = new System.Windows.Forms.Padding(2);
            this.m_radioUseAxisRL.Name = "m_radioUseAxisRL";
            this.m_radioUseAxisRL.NormalColorFirst = System.Drawing.Color.DarkGray;
            this.m_radioUseAxisRL.NormalColorSecond = System.Drawing.Color.Silver;
            this.m_radioUseAxisRL.Size = new System.Drawing.Size(76, 38);
            this.m_radioUseAxisRL.TabIndex = 0;
            this.m_radioUseAxisRL.Click += new System.EventHandler(this.Click_UseOption);
            this.m_radioUseAxisRL.DoubleClick += new System.EventHandler(this.Click_UseOption);
            // 
            // m_lblExtraAxis1
            // 
            this.m_lblExtraAxis1.BackGroundColor = System.Drawing.Color.FromArgb(((int)(((byte)(200)))), ((int)(((byte)(200)))), ((int)(((byte)(220)))));
            this.m_lblExtraAxis1.BorderStroke = 2;
            this.m_lblExtraAxis1.BorderStyle = System.Windows.Forms.ButtonBorderStyle.Solid;
            this.m_lblExtraAxis1.Description = "";
            this.m_lblExtraAxis1.DisabledColor = System.Drawing.Color.DarkGray;
            this.m_lblExtraAxis1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.m_lblExtraAxis1.EdgeRadius = 1;
            this.m_lblExtraAxis1.ImagePosition = new System.Drawing.Point(0, 0);
            this.m_lblExtraAxis1.ImageSize = new System.Drawing.Point(0, 0);
            this.m_lblExtraAxis1.LoadImage = null;
            this.m_lblExtraAxis1.Location = new System.Drawing.Point(577, 93);
            this.m_lblExtraAxis1.MainFont = new System.Drawing.Font("맑은 고딕", 9F, System.Drawing.FontStyle.Bold);
            this.m_lblExtraAxis1.MainFontColor = System.Drawing.Color.Black;
            this.m_lblExtraAxis1.Margin = new System.Windows.Forms.Padding(1, 1, 0, 0);
            this.m_lblExtraAxis1.Name = "m_lblExtraAxis1";
            this.m_lblExtraAxis1.Size = new System.Drawing.Size(145, 41);
            this.m_lblExtraAxis1.SubFont = new System.Drawing.Font("맑은 고딕", 9F, System.Drawing.FontStyle.Bold);
            this.m_lblExtraAxis1.SubFontColor = System.Drawing.Color.Blue;
            this.m_lblExtraAxis1.SubText = "MOTION INDEX";
            this.m_lblExtraAxis1.TabIndex = 1376;
            this.m_lblExtraAxis1.Text = "EXTRA AXIS ← →";
            this.m_lblExtraAxis1.TextAlignMain = Sys3Controls.EN_TEXTALIGN.TOP_LEFT;
            this.m_lblExtraAxis1.TextAlignSub = Sys3Controls.EN_TEXTALIGN.BOTTOM_RIGHT;
            this.m_lblExtraAxis1.TextAlignUnit = Sys3Controls.EN_TEXTALIGN.MIDDLE_CENTER;
            this.m_lblExtraAxis1.ThemeIndex = 0;
            this.m_lblExtraAxis1.UnitAreaRate = 40;
            this.m_lblExtraAxis1.UnitFont = new System.Drawing.Font("맑은 고딕", 10F, System.Drawing.FontStyle.Bold);
            this.m_lblExtraAxis1.UnitFontColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.m_lblExtraAxis1.UnitPositionVertical = false;
            this.m_lblExtraAxis1.UnitText = "";
            this.m_lblExtraAxis1.UseBorder = true;
            this.m_lblExtraAxis1.UseEdgeRadius = false;
            this.m_lblExtraAxis1.UseImage = false;
            this.m_lblExtraAxis1.UseSubFont = true;
            this.m_lblExtraAxis1.UseUnitFont = false;
            // 
            // m_radioUseAxisUD
            // 
            this.m_radioUseAxisUD.Active = false;
            this.m_radioUseAxisUD.ActiveColorFirst = System.Drawing.Color.RoyalBlue;
            this.m_radioUseAxisUD.ActiveColorSecond = System.Drawing.Color.DodgerBlue;
            this.m_radioUseAxisUD.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.m_radioUseAxisUD.BackColor = System.Drawing.Color.WhiteSmoke;
            this.m_radioUseAxisUD.Location = new System.Drawing.Point(184, 136);
            this.m_radioUseAxisUD.Margin = new System.Windows.Forms.Padding(2);
            this.m_radioUseAxisUD.Name = "m_radioUseAxisUD";
            this.m_radioUseAxisUD.NormalColorFirst = System.Drawing.Color.DarkGray;
            this.m_radioUseAxisUD.NormalColorSecond = System.Drawing.Color.Silver;
            this.m_radioUseAxisUD.Size = new System.Drawing.Size(76, 38);
            this.m_radioUseAxisUD.TabIndex = 1;
            this.m_radioUseAxisUD.Click += new System.EventHandler(this.Click_UseOption);
            this.m_radioUseAxisUD.DoubleClick += new System.EventHandler(this.Click_UseOption);
            // 
            // m_lblAxis2
            // 
            this.m_lblAxis2.BackGroundColor = System.Drawing.Color.FromArgb(((int)(((byte)(200)))), ((int)(((byte)(200)))), ((int)(((byte)(220)))));
            this.m_lblAxis2.BorderStroke = 2;
            this.m_lblAxis2.BorderStyle = System.Windows.Forms.ButtonBorderStyle.Solid;
            this.m_lblAxis2.Description = "";
            this.m_lblAxis2.DisabledColor = System.Drawing.Color.DarkGray;
            this.m_lblAxis2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.m_lblAxis2.EdgeRadius = 1;
            this.m_lblAxis2.ImagePosition = new System.Drawing.Point(0, 0);
            this.m_lblAxis2.ImageSize = new System.Drawing.Point(0, 0);
            this.m_lblAxis2.LoadImage = null;
            this.m_lblAxis2.Location = new System.Drawing.Point(37, 135);
            this.m_lblAxis2.MainFont = new System.Drawing.Font("맑은 고딕", 9F, System.Drawing.FontStyle.Bold);
            this.m_lblAxis2.MainFontColor = System.Drawing.Color.Black;
            this.m_lblAxis2.Margin = new System.Windows.Forms.Padding(1, 1, 0, 0);
            this.m_lblAxis2.Name = "m_lblAxis2";
            this.m_lblAxis2.Size = new System.Drawing.Size(145, 41);
            this.m_lblAxis2.SubFont = new System.Drawing.Font("맑은 고딕", 9F, System.Drawing.FontStyle.Bold);
            this.m_lblAxis2.SubFontColor = System.Drawing.Color.Blue;
            this.m_lblAxis2.SubText = "MOTION INDEX";
            this.m_lblAxis2.TabIndex = 1376;
            this.m_lblAxis2.Text = "AXIS↑↓";
            this.m_lblAxis2.TextAlignMain = Sys3Controls.EN_TEXTALIGN.TOP_LEFT;
            this.m_lblAxis2.TextAlignSub = Sys3Controls.EN_TEXTALIGN.BOTTOM_RIGHT;
            this.m_lblAxis2.TextAlignUnit = Sys3Controls.EN_TEXTALIGN.MIDDLE_CENTER;
            this.m_lblAxis2.ThemeIndex = 0;
            this.m_lblAxis2.UnitAreaRate = 40;
            this.m_lblAxis2.UnitFont = new System.Drawing.Font("맑은 고딕", 10F, System.Drawing.FontStyle.Bold);
            this.m_lblAxis2.UnitFontColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.m_lblAxis2.UnitPositionVertical = false;
            this.m_lblAxis2.UnitText = "";
            this.m_lblAxis2.UseBorder = true;
            this.m_lblAxis2.UseEdgeRadius = false;
            this.m_lblAxis2.UseImage = false;
            this.m_lblAxis2.UseSubFont = true;
            this.m_lblAxis2.UseUnitFont = false;
            // 
            // m_labelIndex
            // 
            this.m_labelIndex.BackGroundColor = System.Drawing.Color.White;
            this.m_labelIndex.BorderStroke = 2;
            this.m_labelIndex.BorderStyle = System.Windows.Forms.ButtonBorderStyle.Solid;
            this.tableLayoutPanel2.SetColumnSpan(this.m_labelIndex, 2);
            this.m_labelIndex.Description = "";
            this.m_labelIndex.DisabledColor = System.Drawing.Color.LightGray;
            this.m_labelIndex.Dock = System.Windows.Forms.DockStyle.Fill;
            this.m_labelIndex.EdgeRadius = 1;
            this.m_labelIndex.Enabled = false;
            this.m_labelIndex.ImagePosition = new System.Drawing.Point(0, 0);
            this.m_labelIndex.ImageSize = new System.Drawing.Point(0, 0);
            this.m_labelIndex.LoadImage = null;
            this.m_labelIndex.Location = new System.Drawing.Point(183, 9);
            this.m_labelIndex.MainFont = new System.Drawing.Font("맑은 고딕", 11F, System.Drawing.FontStyle.Bold);
            this.m_labelIndex.MainFontColor = System.Drawing.Color.Black;
            this.m_labelIndex.Margin = new System.Windows.Forms.Padding(1, 1, 0, 0);
            this.m_labelIndex.Name = "m_labelIndex";
            this.m_labelIndex.Size = new System.Drawing.Size(382, 41);
            this.m_labelIndex.SubFont = new System.Drawing.Font("맑은 고딕", 10F);
            this.m_labelIndex.SubFontColor = System.Drawing.Color.Black;
            this.m_labelIndex.SubText = "";
            this.m_labelIndex.TabIndex = 1379;
            this.m_labelIndex.TextAlignMain = Sys3Controls.EN_TEXTALIGN.MIDDLE_CENTER;
            this.m_labelIndex.TextAlignSub = Sys3Controls.EN_TEXTALIGN.MIDDLE_CENTER;
            this.m_labelIndex.TextAlignUnit = Sys3Controls.EN_TEXTALIGN.MIDDLE_CENTER;
            this.m_labelIndex.ThemeIndex = 0;
            this.m_labelIndex.UnitAreaRate = 40;
            this.m_labelIndex.UnitFont = new System.Drawing.Font("맑은 고딕", 10F, System.Drawing.FontStyle.Bold);
            this.m_labelIndex.UnitFontColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.m_labelIndex.UnitPositionVertical = false;
            this.m_labelIndex.UnitText = "";
            this.m_labelIndex.UseBorder = true;
            this.m_labelIndex.UseEdgeRadius = false;
            this.m_labelIndex.UseImage = false;
            this.m_labelIndex.UseSubFont = false;
            this.m_labelIndex.UseUnitFont = false;
            // 
            // m_btnRemove
            // 
            this.m_btnRemove.BorderWidth = 2;
            this.m_btnRemove.ButtonClicked = false;
            this.m_btnRemove.ClickedEmphasizeTextColor = System.Drawing.Color.White;
            this.m_btnRemove.CustomClickedGradientFirstColor = System.Drawing.Color.White;
            this.m_btnRemove.CustomClickedGradientSecondColor = System.Drawing.Color.White;
            this.m_btnRemove.Description = "";
            this.m_btnRemove.DisabledColor = System.Drawing.Color.DarkGray;
            this.m_btnRemove.Dock = System.Windows.Forms.DockStyle.Fill;
            this.m_btnRemove.EdgeRadius = 5;
            this.m_btnRemove.GradientAngle = 70F;
            this.m_btnRemove.GradientFirstColor = System.Drawing.Color.White;
            this.m_btnRemove.GradientSecondColor = System.Drawing.Color.FromArgb(((int)(((byte)(170)))), ((int)(((byte)(176)))), ((int)(((byte)(183)))));
            this.m_btnRemove.HoverEmphasizeCustomColor = System.Drawing.Color.White;
            this.m_btnRemove.ImagePosition = new System.Drawing.Point(15, 15);
            this.m_btnRemove.ImageSize = new System.Drawing.Point(30, 30);
            this.m_btnRemove.LoadImage = global::FrameOfSystem3.Properties.Resources.REMOVE;
            this.m_btnRemove.Location = new System.Drawing.Point(150, 5);
            this.m_btnRemove.MainFont = new System.Drawing.Font("맑은 고딕", 13F, System.Drawing.FontStyle.Bold);
            this.m_btnRemove.MainFontColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(36)))), ((int)(((byte)(0)))));
            this.m_btnRemove.Margin = new System.Windows.Forms.Padding(5);
            this.m_btnRemove.Name = "m_btnRemove";
            this.m_btnRemove.Size = new System.Drawing.Size(135, 74);
            this.m_btnRemove.SubFont = new System.Drawing.Font("맑은 고딕", 8F, System.Drawing.FontStyle.Bold);
            this.m_btnRemove.SubFontColor = System.Drawing.Color.DarkBlue;
            this.m_btnRemove.SubText = "STATUS";
            this.m_btnRemove.TabIndex = 1;
            this.m_btnRemove.Text = "REMOVE";
            this.m_btnRemove.TextAlignMain = Sys3Controls.EN_TEXTALIGN.MIDDLE_RIGHT;
            this.m_btnRemove.TextAlignSub = Sys3Controls.EN_TEXTALIGN.TOP_LEFT;
            this.m_btnRemove.ThemeIndex = 0;
            this.m_btnRemove.UseBorder = true;
            this.m_btnRemove.UseClickedEmphasizeTextColor = false;
            this.m_btnRemove.UseCustomizeClickedColor = false;
            this.m_btnRemove.UseEdge = false;
            this.m_btnRemove.UseHoverEmphasizeCustomColor = false;
            this.m_btnRemove.UseImage = true;
            this.m_btnRemove.UserHoverEmpahsize = false;
            this.m_btnRemove.UseSubFont = false;
            this.m_btnRemove.Click += new System.EventHandler(this.Click_AddOrRemove);
            // 
            // m_btnAdd
            // 
            this.m_btnAdd.BorderWidth = 2;
            this.m_btnAdd.ButtonClicked = false;
            this.m_btnAdd.ClickedEmphasizeTextColor = System.Drawing.Color.White;
            this.m_btnAdd.CustomClickedGradientFirstColor = System.Drawing.Color.White;
            this.m_btnAdd.CustomClickedGradientSecondColor = System.Drawing.Color.White;
            this.m_btnAdd.Description = "";
            this.m_btnAdd.DisabledColor = System.Drawing.Color.DarkGray;
            this.m_btnAdd.Dock = System.Windows.Forms.DockStyle.Fill;
            this.m_btnAdd.EdgeRadius = 5;
            this.m_btnAdd.GradientAngle = 70F;
            this.m_btnAdd.GradientFirstColor = System.Drawing.Color.White;
            this.m_btnAdd.GradientSecondColor = System.Drawing.Color.FromArgb(((int)(((byte)(170)))), ((int)(((byte)(176)))), ((int)(((byte)(183)))));
            this.m_btnAdd.HoverEmphasizeCustomColor = System.Drawing.Color.White;
            this.m_btnAdd.ImagePosition = new System.Drawing.Point(15, 15);
            this.m_btnAdd.ImageSize = new System.Drawing.Point(30, 30);
            this.m_btnAdd.LoadImage = global::FrameOfSystem3.Properties.Resources.CONFIG_ADD3;
            this.m_btnAdd.Location = new System.Drawing.Point(5, 5);
            this.m_btnAdd.MainFont = new System.Drawing.Font("맑은 고딕", 13F, System.Drawing.FontStyle.Bold);
            this.m_btnAdd.MainFontColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(36)))), ((int)(((byte)(0)))));
            this.m_btnAdd.Margin = new System.Windows.Forms.Padding(5);
            this.m_btnAdd.Name = "m_btnAdd";
            this.m_btnAdd.Size = new System.Drawing.Size(135, 74);
            this.m_btnAdd.SubFont = new System.Drawing.Font("맑은 고딕", 8F, System.Drawing.FontStyle.Bold);
            this.m_btnAdd.SubFontColor = System.Drawing.Color.DarkBlue;
            this.m_btnAdd.SubText = "STATUS";
            this.m_btnAdd.TabIndex = 0;
            this.m_btnAdd.Text = "ADD";
            this.m_btnAdd.TextAlignMain = Sys3Controls.EN_TEXTALIGN.MIDDLE_RIGHT;
            this.m_btnAdd.TextAlignSub = Sys3Controls.EN_TEXTALIGN.TOP_LEFT;
            this.m_btnAdd.ThemeIndex = 0;
            this.m_btnAdd.UseBorder = true;
            this.m_btnAdd.UseClickedEmphasizeTextColor = false;
            this.m_btnAdd.UseCustomizeClickedColor = false;
            this.m_btnAdd.UseEdge = false;
            this.m_btnAdd.UseHoverEmphasizeCustomColor = false;
            this.m_btnAdd.UseImage = true;
            this.m_btnAdd.UserHoverEmpahsize = false;
            this.m_btnAdd.UseSubFont = false;
            this.m_btnAdd.Click += new System.EventHandler(this.Click_AddOrRemove);
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 1;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel1.Controls.Add(this.m_JogGroupList, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.m_groupSelectedItem, 0, 1);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel1.Margin = new System.Windows.Forms.Padding(0);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 2;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 73.66666F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 26.33333F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(1156, 851);
            this.tableLayoutPanel1.TabIndex = 1380;
            // 
            // tableLayoutPanel3
            // 
            this.tableLayoutPanel3.ColumnCount = 2;
            this.tableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel3.Controls.Add(this.m_btnAdd, 0, 0);
            this.tableLayoutPanel3.Controls.Add(this.m_btnRemove, 1, 0);
            this.tableLayoutPanel3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel3.Location = new System.Drawing.Point(815, 8);
            this.tableLayoutPanel3.Margin = new System.Windows.Forms.Padding(0);
            this.tableLayoutPanel3.Name = "tableLayoutPanel3";
            this.tableLayoutPanel3.RowCount = 1;
            this.tableLayoutPanel2.SetRowSpan(this.tableLayoutPanel3, 2);
            this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel3.Size = new System.Drawing.Size(290, 84);
            this.tableLayoutPanel3.TabIndex = 1380;
            // 
            // Config_Jog
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.Controls.Add(this.tableLayoutPanel1);
            this.Name = "Config_Jog";
            this.Size = new System.Drawing.Size(1156, 851);
            ((System.ComponentModel.ISupportInitialize)(this.m_dgViewJog)).EndInit();
            this.m_JogGroupList.ResumeLayout(false);
            this.m_groupSelectedItem.ResumeLayout(false);
            this.tableLayoutPanel2.ResumeLayout(false);
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel3.ResumeLayout(false);
            this.ResumeLayout(false);

		}

		#endregion

		private Sys3Controls.Sys3DoubleBufferedDataGridView m_dgViewJog;
		private Sys3Controls.Sys3GroupBoxContainer m_JogGroupList;
        private Sys3Controls.Sys3GroupBoxContainer m_groupSelectedItem;
		private Sys3Controls.Sys3Label m_lblIndex;
		private Sys3Controls.Sys3Label m_lblGroupName;
		private Sys3Controls.Sys3Label m_lblAxis1;
		private Sys3Controls.Sys3Label m_lblAxis2;
		private Sys3Controls.Sys3ToggleButton m_radioUseAxisRL;
		private Sys3Controls.Sys3ToggleButton m_radioUseAxisUD;
		private Sys3Controls.Sys3button m_btnRemove;
		private Sys3Controls.Sys3button m_btnAdd;
		private Sys3Controls.Sys3Label m_labelAxis1;
		private Sys3Controls.Sys3Label m_labelIndex;
		private Sys3Controls.Sys3Label m_labelAxis2;
		private Sys3Controls.Sys3Label m_labelGroupName;
		private Sys3Controls.Sys3Label m_lblExtraAxis1;
		private Sys3Controls.Sys3Label m_lblExtraAxis2;
		private Sys3Controls.Sys3ToggleButton m_radioUseExtraAxisRL;
		private Sys3Controls.Sys3ToggleButton m_radioUseExtraAxisUD;
		private Sys3Controls.Sys3Label m_labelExtraAxis1;
		private Sys3Controls.Sys3Label m_labelExtraAxis2;
		private System.Windows.Forms.DataGridViewTextBoxColumn INDEX;
		private System.Windows.Forms.DataGridViewTextBoxColumn ID;
		private System.Windows.Forms.DataGridViewTextBoxColumn AXIS;
		private System.Windows.Forms.DataGridViewTextBoxColumn ENABLE_LR;
		private System.Windows.Forms.DataGridViewTextBoxColumn SOLUTIONCODE;
		private System.Windows.Forms.DataGridViewTextBoxColumn ENABLE_UD;
		private System.Windows.Forms.DataGridViewTextBoxColumn Column1;
		private System.Windows.Forms.DataGridViewTextBoxColumn ENABLE_EX_LR;
		private System.Windows.Forms.DataGridViewTextBoxColumn Column2;
		private System.Windows.Forms.DataGridViewTextBoxColumn ENABLE_EX_UD;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel2;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel3;
	}
}
