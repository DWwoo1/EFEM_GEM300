namespace FrameOfSystem3.Views.Config
{
    partial class Config_Tool
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
			Sys3Controls.Sys3GroupBoxContainer sys3GroupBox1;
			System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle10 = new System.Windows.Forms.DataGridViewCellStyle();
			System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle11 = new System.Windows.Forms.DataGridViewCellStyle();
			System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle12 = new System.Windows.Forms.DataGridViewCellStyle();
			Sys3Controls.Sys3GroupBoxContainer sys3GroupBox3;
			Sys3Controls.Sys3GroupBoxContainer sys3GroupBox2;
			System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle13 = new System.Windows.Forms.DataGridViewCellStyle();
			System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle14 = new System.Windows.Forms.DataGridViewCellStyle();
			System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle15 = new System.Windows.Forms.DataGridViewCellStyle();
			Sys3Controls.Sys3GroupBoxContainer sys3GroupBox4;
			System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle16 = new System.Windows.Forms.DataGridViewCellStyle();
			System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle17 = new System.Windows.Forms.DataGridViewCellStyle();
			System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle18 = new System.Windows.Forms.DataGridViewCellStyle();
			this.m_dgViewToolList = new Sys3Controls.Sys3DoubleBufferedDataGridView();
			this.dataGridViewTextBoxColumn3 = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.dataGridViewTextBoxColumn1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.tableLayoutPanel2 = new System.Windows.Forms.TableLayoutPanel();
			this.btn_Enable = new Sys3Controls.Sys3button();
			this.btn_Reset = new Sys3Controls.Sys3button();
			this.btn_Disable = new Sys3Controls.Sys3button();
			this.m_dgViewItemList = new Sys3Controls.Sys3DoubleBufferedDataGridView();
			this.dataGridViewTextBoxColumn2 = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.dataGridViewTextBoxColumn4 = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.m_dgViewMonitoringData = new Sys3Controls.Sys3DoubleBufferedDataGridView();
			this.dataGridViewTextBoxColumn5 = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.dataGridViewTextBoxColumn6 = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
			this.m_btnRemove = new Sys3Controls.Sys3button();
			this.m_btnAdd = new Sys3Controls.Sys3button();
			sys3GroupBox1 = new Sys3Controls.Sys3GroupBoxContainer();
			sys3GroupBox3 = new Sys3Controls.Sys3GroupBoxContainer();
			sys3GroupBox2 = new Sys3Controls.Sys3GroupBoxContainer();
			sys3GroupBox4 = new Sys3Controls.Sys3GroupBoxContainer();
			sys3GroupBox1.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.m_dgViewToolList)).BeginInit();
			sys3GroupBox3.SuspendLayout();
			this.tableLayoutPanel2.SuspendLayout();
			sys3GroupBox2.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.m_dgViewItemList)).BeginInit();
			sys3GroupBox4.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.m_dgViewMonitoringData)).BeginInit();
			this.tableLayoutPanel1.SuspendLayout();
			this.SuspendLayout();
			// 
			// sys3GroupBox1
			// 
			sys3GroupBox1.BackGroundColor = System.Drawing.Color.WhiteSmoke;
			sys3GroupBox1.Controls.Add(this.m_dgViewToolList);
			sys3GroupBox1.Dock = System.Windows.Forms.DockStyle.Fill;
			sys3GroupBox1.EdgeBorderStroke = 2;
			sys3GroupBox1.EdgeRadius = 2;
			sys3GroupBox1.LabelFont = new System.Drawing.Font("맑은 고딕", 15F, System.Drawing.FontStyle.Bold);
			sys3GroupBox1.LabelGradientColorFirst = System.Drawing.Color.FromArgb(((int)(((byte)(208)))), ((int)(((byte)(208)))), ((int)(((byte)(208)))));
			sys3GroupBox1.LabelGradientColorSecond = System.Drawing.Color.FromArgb(((int)(((byte)(240)))), ((int)(((byte)(240)))), ((int)(((byte)(240)))));
			sys3GroupBox1.LabelHeight = 42;
			sys3GroupBox1.LabelTextColor = System.Drawing.Color.Black;
			sys3GroupBox1.Location = new System.Drawing.Point(0, 0);
			sys3GroupBox1.Margin = new System.Windows.Forms.Padding(0);
			sys3GroupBox1.Name = "sys3GroupBox1";
			sys3GroupBox1.Padding = new System.Windows.Forms.Padding(5, 30, 5, 5);
			sys3GroupBox1.Size = new System.Drawing.Size(311, 739);
			sys3GroupBox1.TabIndex = 1473;
			sys3GroupBox1.TabStop = false;
			sys3GroupBox1.Text = "TOOL LIST";
			sys3GroupBox1.TextAlign = Sys3Controls.EN_TEXTALIGN.MIDDLE_CENTER;
			sys3GroupBox1.ThemeIndex = 0;
			sys3GroupBox1.UseLabelBorder = true;
			sys3GroupBox1.UseTitle = true;
			// 
			// m_dgViewToolList
			// 
			this.m_dgViewToolList.AllowUserToAddRows = false;
			this.m_dgViewToolList.AllowUserToDeleteRows = false;
			this.m_dgViewToolList.AllowUserToResizeColumns = false;
			this.m_dgViewToolList.AllowUserToResizeRows = false;
			this.m_dgViewToolList.BackgroundColor = System.Drawing.Color.White;
			this.m_dgViewToolList.CellBorderStyle = System.Windows.Forms.DataGridViewCellBorderStyle.Raised;
			this.m_dgViewToolList.ColumnHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.Single;
			dataGridViewCellStyle10.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
			dataGridViewCellStyle10.BackColor = System.Drawing.Color.LightGray;
			dataGridViewCellStyle10.Font = new System.Drawing.Font("맑은 고딕", 9F, System.Drawing.FontStyle.Bold);
			dataGridViewCellStyle10.ForeColor = System.Drawing.Color.Black;
			dataGridViewCellStyle10.SelectionBackColor = System.Drawing.Color.White;
			dataGridViewCellStyle10.SelectionForeColor = System.Drawing.Color.Black;
			dataGridViewCellStyle10.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
			this.m_dgViewToolList.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle10;
			this.m_dgViewToolList.ColumnHeadersHeight = 30;
			this.m_dgViewToolList.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
			this.m_dgViewToolList.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.dataGridViewTextBoxColumn3,
            this.dataGridViewTextBoxColumn1});
			dataGridViewCellStyle11.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
			dataGridViewCellStyle11.BackColor = System.Drawing.SystemColors.Window;
			dataGridViewCellStyle11.Font = new System.Drawing.Font("맑은 고딕", 9F, System.Drawing.FontStyle.Bold);
			dataGridViewCellStyle11.ForeColor = System.Drawing.SystemColors.ControlText;
			dataGridViewCellStyle11.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(120)))), ((int)(((byte)(130)))), ((int)(((byte)(150)))));
			dataGridViewCellStyle11.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
			dataGridViewCellStyle11.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
			this.m_dgViewToolList.DefaultCellStyle = dataGridViewCellStyle11;
			this.m_dgViewToolList.Dock = System.Windows.Forms.DockStyle.Fill;
			this.m_dgViewToolList.EnableHeadersVisualStyles = false;
			this.m_dgViewToolList.GridColor = System.Drawing.Color.FromArgb(((int)(((byte)(120)))), ((int)(((byte)(130)))), ((int)(((byte)(150)))));
			this.m_dgViewToolList.Location = new System.Drawing.Point(5, 44);
			this.m_dgViewToolList.MultiSelect = false;
			this.m_dgViewToolList.Name = "m_dgViewToolList";
			this.m_dgViewToolList.ReadOnly = true;
			this.m_dgViewToolList.RowHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.None;
			dataGridViewCellStyle12.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
			dataGridViewCellStyle12.BackColor = System.Drawing.Color.White;
			dataGridViewCellStyle12.Font = new System.Drawing.Font("맑은 고딕", 11F);
			dataGridViewCellStyle12.ForeColor = System.Drawing.SystemColors.WindowText;
			dataGridViewCellStyle12.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(120)))), ((int)(((byte)(130)))), ((int)(((byte)(150)))));
			dataGridViewCellStyle12.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
			dataGridViewCellStyle12.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
			this.m_dgViewToolList.RowHeadersDefaultCellStyle = dataGridViewCellStyle12;
			this.m_dgViewToolList.RowHeadersVisible = false;
			this.m_dgViewToolList.RowHeadersWidthSizeMode = System.Windows.Forms.DataGridViewRowHeadersWidthSizeMode.DisableResizing;
			this.m_dgViewToolList.RowTemplate.Height = 30;
			this.m_dgViewToolList.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
			this.m_dgViewToolList.Size = new System.Drawing.Size(301, 690);
			this.m_dgViewToolList.TabIndex = 1472;
			this.m_dgViewToolList.CellClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.Click_dgViewToolList);
			// 
			// dataGridViewTextBoxColumn3
			// 
			this.dataGridViewTextBoxColumn3.Frozen = true;
			this.dataGridViewTextBoxColumn3.HeaderText = "INDEX";
			this.dataGridViewTextBoxColumn3.MaxInputLength = 20;
			this.dataGridViewTextBoxColumn3.Name = "dataGridViewTextBoxColumn3";
			this.dataGridViewTextBoxColumn3.ReadOnly = true;
			this.dataGridViewTextBoxColumn3.Resizable = System.Windows.Forms.DataGridViewTriState.False;
			this.dataGridViewTextBoxColumn3.Width = 60;
			// 
			// dataGridViewTextBoxColumn1
			// 
			this.dataGridViewTextBoxColumn1.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
			this.dataGridViewTextBoxColumn1.HeaderText = "NAME";
			this.dataGridViewTextBoxColumn1.MaxInputLength = 20;
			this.dataGridViewTextBoxColumn1.Name = "dataGridViewTextBoxColumn1";
			this.dataGridViewTextBoxColumn1.ReadOnly = true;
			this.dataGridViewTextBoxColumn1.Resizable = System.Windows.Forms.DataGridViewTriState.False;
			this.dataGridViewTextBoxColumn1.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
			// 
			// sys3GroupBox3
			// 
			sys3GroupBox3.BackGroundColor = System.Drawing.Color.WhiteSmoke;
			this.tableLayoutPanel1.SetColumnSpan(sys3GroupBox3, 3);
			sys3GroupBox3.Controls.Add(this.tableLayoutPanel2);
			sys3GroupBox3.Dock = System.Windows.Forms.DockStyle.Fill;
			sys3GroupBox3.EdgeBorderStroke = 2;
			sys3GroupBox3.EdgeRadius = 2;
			sys3GroupBox3.LabelFont = new System.Drawing.Font("맑은 고딕", 15F, System.Drawing.FontStyle.Bold);
			sys3GroupBox3.LabelGradientColorFirst = System.Drawing.Color.FromArgb(((int)(((byte)(208)))), ((int)(((byte)(208)))), ((int)(((byte)(208)))));
			sys3GroupBox3.LabelGradientColorSecond = System.Drawing.Color.FromArgb(((int)(((byte)(240)))), ((int)(((byte)(240)))), ((int)(((byte)(240)))));
			sys3GroupBox3.LabelHeight = 42;
			sys3GroupBox3.LabelTextColor = System.Drawing.Color.Black;
			sys3GroupBox3.Location = new System.Drawing.Point(0, 739);
			sys3GroupBox3.Margin = new System.Windows.Forms.Padding(0);
			sys3GroupBox3.Name = "sys3GroupBox3";
			sys3GroupBox3.Padding = new System.Windows.Forms.Padding(5, 30, 5, 5);
			sys3GroupBox3.Size = new System.Drawing.Size(1156, 161);
			sys3GroupBox3.TabIndex = 1477;
			sys3GroupBox3.TabStop = false;
			sys3GroupBox3.Text = "EDIT";
			sys3GroupBox3.TextAlign = Sys3Controls.EN_TEXTALIGN.MIDDLE_CENTER;
			sys3GroupBox3.ThemeIndex = 0;
			sys3GroupBox3.UseLabelBorder = true;
			sys3GroupBox3.UseTitle = true;
			// 
			// tableLayoutPanel2
			// 
			this.tableLayoutPanel2.ColumnCount = 9;
			this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 0.6234917F));
			this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 12.46984F));
			this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 12.46984F));
			this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 4.562962F));
			this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 12.46984F));
			this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 12.46983F));
			this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 31.84088F));
			this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 12.46984F));
			this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 0.6234917F));
			this.tableLayoutPanel2.Controls.Add(this.btn_Enable, 1, 1);
			this.tableLayoutPanel2.Controls.Add(this.btn_Reset, 7, 1);
			this.tableLayoutPanel2.Controls.Add(this.btn_Disable, 2, 1);
			this.tableLayoutPanel2.Controls.Add(this.m_btnRemove, 5, 1);
			this.tableLayoutPanel2.Controls.Add(this.m_btnAdd, 4, 1);
			this.tableLayoutPanel2.Dock = System.Windows.Forms.DockStyle.Fill;
			this.tableLayoutPanel2.Location = new System.Drawing.Point(5, 44);
			this.tableLayoutPanel2.Name = "tableLayoutPanel2";
			this.tableLayoutPanel2.RowCount = 3;
			this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 13.23529F));
			this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 73.52942F));
			this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 13.23529F));
			this.tableLayoutPanel2.Size = new System.Drawing.Size(1146, 112);
			this.tableLayoutPanel2.TabIndex = 0;
			// 
			// btn_Enable
			// 
			this.btn_Enable.BorderWidth = 3;
			this.btn_Enable.ButtonClicked = false;
			this.btn_Enable.ClickedEmphasizeTextColor = System.Drawing.Color.White;
			this.btn_Enable.CustomClickedGradientFirstColor = System.Drawing.Color.White;
			this.btn_Enable.CustomClickedGradientSecondColor = System.Drawing.Color.White;
			this.btn_Enable.Description = "";
			this.btn_Enable.DisabledColor = System.Drawing.Color.LightSlateGray;
			this.btn_Enable.Dock = System.Windows.Forms.DockStyle.Fill;
			this.btn_Enable.EdgeRadius = 5;
			this.btn_Enable.Enabled = false;
			this.btn_Enable.GradientAngle = 70F;
			this.btn_Enable.GradientFirstColor = System.Drawing.Color.White;
			this.btn_Enable.GradientSecondColor = System.Drawing.Color.LightSlateGray;
			this.btn_Enable.HoverEmphasizeCustomColor = System.Drawing.Color.White;
			this.btn_Enable.ImagePosition = new System.Drawing.Point(7, 7);
			this.btn_Enable.ImageSize = new System.Drawing.Point(30, 30);
			this.btn_Enable.LoadImage = global::FrameOfSystem3.Properties.Resources.ENABLE_BLACK;
			this.btn_Enable.Location = new System.Drawing.Point(10, 17);
			this.btn_Enable.MainFont = new System.Drawing.Font("맑은 고딕", 13F, System.Drawing.FontStyle.Bold);
			this.btn_Enable.MainFontColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(36)))), ((int)(((byte)(0)))));
			this.btn_Enable.Name = "btn_Enable";
			this.btn_Enable.Size = new System.Drawing.Size(136, 76);
			this.btn_Enable.SubFont = new System.Drawing.Font("맑은 고딕", 8F, System.Drawing.FontStyle.Bold);
			this.btn_Enable.SubFontColor = System.Drawing.Color.DarkBlue;
			this.btn_Enable.SubText = "STATUS";
			this.btn_Enable.TabIndex = 0;
			this.btn_Enable.Text = "ENABLE";
			this.btn_Enable.TextAlignMain = Sys3Controls.EN_TEXTALIGN.MIDDLE_RIGHT;
			this.btn_Enable.TextAlignSub = Sys3Controls.EN_TEXTALIGN.TOP_RIGHT;
			this.btn_Enable.ThemeIndex = 0;
			this.btn_Enable.UseBorder = true;
			this.btn_Enable.UseClickedEmphasizeTextColor = false;
			this.btn_Enable.UseCustomizeClickedColor = false;
			this.btn_Enable.UseEdge = true;
			this.btn_Enable.UseHoverEmphasizeCustomColor = false;
			this.btn_Enable.UseImage = true;
			this.btn_Enable.UserHoverEmpahsize = false;
			this.btn_Enable.UseSubFont = true;
			this.btn_Enable.Click += new System.EventHandler(this.Click_EnableDisable);
			// 
			// btn_Reset
			// 
			this.btn_Reset.BorderWidth = 3;
			this.btn_Reset.ButtonClicked = false;
			this.btn_Reset.ClickedEmphasizeTextColor = System.Drawing.Color.White;
			this.btn_Reset.CustomClickedGradientFirstColor = System.Drawing.Color.White;
			this.btn_Reset.CustomClickedGradientSecondColor = System.Drawing.Color.White;
			this.btn_Reset.Description = "";
			this.btn_Reset.DisabledColor = System.Drawing.Color.DarkGray;
			this.btn_Reset.Dock = System.Windows.Forms.DockStyle.Fill;
			this.btn_Reset.EdgeRadius = 5;
			this.btn_Reset.GradientAngle = 70F;
			this.btn_Reset.GradientFirstColor = System.Drawing.Color.White;
			this.btn_Reset.GradientSecondColor = System.Drawing.Color.LightSeaGreen;
			this.btn_Reset.HoverEmphasizeCustomColor = System.Drawing.Color.White;
			this.btn_Reset.ImagePosition = new System.Drawing.Point(7, 7);
			this.btn_Reset.ImageSize = new System.Drawing.Point(30, 30);
			this.btn_Reset.LoadImage = global::FrameOfSystem3.Properties.Resources.REPEAT_BLACK;
			this.btn_Reset.Location = new System.Drawing.Point(994, 17);
			this.btn_Reset.MainFont = new System.Drawing.Font("맑은 고딕", 13F, System.Drawing.FontStyle.Bold);
			this.btn_Reset.MainFontColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(36)))), ((int)(((byte)(0)))));
			this.btn_Reset.Name = "btn_Reset";
			this.btn_Reset.Size = new System.Drawing.Size(136, 76);
			this.btn_Reset.SubFont = new System.Drawing.Font("맑은 고딕", 8F, System.Drawing.FontStyle.Bold);
			this.btn_Reset.SubFontColor = System.Drawing.Color.DarkBlue;
			this.btn_Reset.SubText = "STATUS";
			this.btn_Reset.TabIndex = 1;
			this.btn_Reset.Text = "RESET";
			this.btn_Reset.TextAlignMain = Sys3Controls.EN_TEXTALIGN.MIDDLE_RIGHT;
			this.btn_Reset.TextAlignSub = Sys3Controls.EN_TEXTALIGN.TOP_LEFT;
			this.btn_Reset.ThemeIndex = 0;
			this.btn_Reset.UseBorder = true;
			this.btn_Reset.UseClickedEmphasizeTextColor = false;
			this.btn_Reset.UseCustomizeClickedColor = false;
			this.btn_Reset.UseEdge = true;
			this.btn_Reset.UseHoverEmphasizeCustomColor = false;
			this.btn_Reset.UseImage = true;
			this.btn_Reset.UserHoverEmpahsize = false;
			this.btn_Reset.UseSubFont = false;
			this.btn_Reset.Click += new System.EventHandler(this.Click_Reset);
			// 
			// btn_Disable
			// 
			this.btn_Disable.BorderWidth = 3;
			this.btn_Disable.ButtonClicked = false;
			this.btn_Disable.ClickedEmphasizeTextColor = System.Drawing.Color.White;
			this.btn_Disable.CustomClickedGradientFirstColor = System.Drawing.Color.White;
			this.btn_Disable.CustomClickedGradientSecondColor = System.Drawing.Color.White;
			this.btn_Disable.Description = "";
			this.btn_Disable.DisabledColor = System.Drawing.Color.LightSlateGray;
			this.btn_Disable.Dock = System.Windows.Forms.DockStyle.Fill;
			this.btn_Disable.EdgeRadius = 5;
			this.btn_Disable.Enabled = false;
			this.btn_Disable.GradientAngle = 70F;
			this.btn_Disable.GradientFirstColor = System.Drawing.Color.White;
			this.btn_Disable.GradientSecondColor = System.Drawing.Color.LightSlateGray;
			this.btn_Disable.HoverEmphasizeCustomColor = System.Drawing.Color.White;
			this.btn_Disable.ImagePosition = new System.Drawing.Point(7, 7);
			this.btn_Disable.ImageSize = new System.Drawing.Point(30, 30);
			this.btn_Disable.LoadImage = global::FrameOfSystem3.Properties.Resources.DISABLE_BLACK;
			this.btn_Disable.Location = new System.Drawing.Point(152, 17);
			this.btn_Disable.MainFont = new System.Drawing.Font("맑은 고딕", 13F, System.Drawing.FontStyle.Bold);
			this.btn_Disable.MainFontColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(36)))), ((int)(((byte)(0)))));
			this.btn_Disable.Name = "btn_Disable";
			this.btn_Disable.Size = new System.Drawing.Size(136, 76);
			this.btn_Disable.SubFont = new System.Drawing.Font("맑은 고딕", 8F, System.Drawing.FontStyle.Bold);
			this.btn_Disable.SubFontColor = System.Drawing.Color.DarkBlue;
			this.btn_Disable.SubText = "STATUS";
			this.btn_Disable.TabIndex = 1;
			this.btn_Disable.Text = "DISABLE";
			this.btn_Disable.TextAlignMain = Sys3Controls.EN_TEXTALIGN.MIDDLE_RIGHT;
			this.btn_Disable.TextAlignSub = Sys3Controls.EN_TEXTALIGN.TOP_RIGHT;
			this.btn_Disable.ThemeIndex = 0;
			this.btn_Disable.UseBorder = true;
			this.btn_Disable.UseClickedEmphasizeTextColor = false;
			this.btn_Disable.UseCustomizeClickedColor = false;
			this.btn_Disable.UseEdge = true;
			this.btn_Disable.UseHoverEmphasizeCustomColor = false;
			this.btn_Disable.UseImage = true;
			this.btn_Disable.UserHoverEmpahsize = false;
			this.btn_Disable.UseSubFont = true;
			this.btn_Disable.Click += new System.EventHandler(this.Click_EnableDisable);
			// 
			// sys3GroupBox2
			// 
			sys3GroupBox2.BackGroundColor = System.Drawing.Color.WhiteSmoke;
			sys3GroupBox2.Controls.Add(this.m_dgViewItemList);
			sys3GroupBox2.Dock = System.Windows.Forms.DockStyle.Fill;
			sys3GroupBox2.EdgeBorderStroke = 2;
			sys3GroupBox2.EdgeRadius = 2;
			sys3GroupBox2.LabelFont = new System.Drawing.Font("맑은 고딕", 15F, System.Drawing.FontStyle.Bold);
			sys3GroupBox2.LabelGradientColorFirst = System.Drawing.Color.FromArgb(((int)(((byte)(208)))), ((int)(((byte)(208)))), ((int)(((byte)(208)))));
			sys3GroupBox2.LabelGradientColorSecond = System.Drawing.Color.FromArgb(((int)(((byte)(240)))), ((int)(((byte)(240)))), ((int)(((byte)(240)))));
			sys3GroupBox2.LabelHeight = 42;
			sys3GroupBox2.LabelTextColor = System.Drawing.Color.Black;
			sys3GroupBox2.Location = new System.Drawing.Point(311, 0);
			sys3GroupBox2.Margin = new System.Windows.Forms.Padding(0);
			sys3GroupBox2.Name = "sys3GroupBox2";
			sys3GroupBox2.Padding = new System.Windows.Forms.Padding(5, 30, 5, 5);
			sys3GroupBox2.Size = new System.Drawing.Size(444, 739);
			sys3GroupBox2.TabIndex = 1478;
			sys3GroupBox2.TabStop = false;
			sys3GroupBox2.Text = "ITEM LIST";
			sys3GroupBox2.TextAlign = Sys3Controls.EN_TEXTALIGN.MIDDLE_CENTER;
			sys3GroupBox2.ThemeIndex = 0;
			sys3GroupBox2.UseLabelBorder = true;
			sys3GroupBox2.UseTitle = true;
			// 
			// m_dgViewItemList
			// 
			this.m_dgViewItemList.AllowUserToAddRows = false;
			this.m_dgViewItemList.AllowUserToDeleteRows = false;
			this.m_dgViewItemList.AllowUserToResizeColumns = false;
			this.m_dgViewItemList.AllowUserToResizeRows = false;
			this.m_dgViewItemList.BackgroundColor = System.Drawing.Color.White;
			this.m_dgViewItemList.CellBorderStyle = System.Windows.Forms.DataGridViewCellBorderStyle.Raised;
			this.m_dgViewItemList.ColumnHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.Single;
			dataGridViewCellStyle13.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
			dataGridViewCellStyle13.BackColor = System.Drawing.Color.LightGray;
			dataGridViewCellStyle13.Font = new System.Drawing.Font("맑은 고딕", 9F, System.Drawing.FontStyle.Bold);
			dataGridViewCellStyle13.ForeColor = System.Drawing.Color.Black;
			dataGridViewCellStyle13.SelectionBackColor = System.Drawing.Color.White;
			dataGridViewCellStyle13.SelectionForeColor = System.Drawing.Color.Black;
			dataGridViewCellStyle13.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
			this.m_dgViewItemList.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle13;
			this.m_dgViewItemList.ColumnHeadersHeight = 30;
			this.m_dgViewItemList.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
			this.m_dgViewItemList.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.dataGridViewTextBoxColumn2,
            this.dataGridViewTextBoxColumn4});
			dataGridViewCellStyle14.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
			dataGridViewCellStyle14.BackColor = System.Drawing.SystemColors.Window;
			dataGridViewCellStyle14.Font = new System.Drawing.Font("맑은 고딕", 10F, System.Drawing.FontStyle.Bold);
			dataGridViewCellStyle14.ForeColor = System.Drawing.SystemColors.ControlText;
			dataGridViewCellStyle14.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(120)))), ((int)(((byte)(130)))), ((int)(((byte)(150)))));
			dataGridViewCellStyle14.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
			dataGridViewCellStyle14.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
			this.m_dgViewItemList.DefaultCellStyle = dataGridViewCellStyle14;
			this.m_dgViewItemList.Dock = System.Windows.Forms.DockStyle.Fill;
			this.m_dgViewItemList.EnableHeadersVisualStyles = false;
			this.m_dgViewItemList.GridColor = System.Drawing.Color.FromArgb(((int)(((byte)(120)))), ((int)(((byte)(130)))), ((int)(((byte)(150)))));
			this.m_dgViewItemList.Location = new System.Drawing.Point(5, 44);
			this.m_dgViewItemList.MultiSelect = false;
			this.m_dgViewItemList.Name = "m_dgViewItemList";
			this.m_dgViewItemList.ReadOnly = true;
			this.m_dgViewItemList.RowHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.None;
			dataGridViewCellStyle15.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
			dataGridViewCellStyle15.BackColor = System.Drawing.Color.White;
			dataGridViewCellStyle15.Font = new System.Drawing.Font("맑은 고딕", 11F);
			dataGridViewCellStyle15.ForeColor = System.Drawing.SystemColors.WindowText;
			dataGridViewCellStyle15.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(120)))), ((int)(((byte)(130)))), ((int)(((byte)(150)))));
			dataGridViewCellStyle15.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
			dataGridViewCellStyle15.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
			this.m_dgViewItemList.RowHeadersDefaultCellStyle = dataGridViewCellStyle15;
			this.m_dgViewItemList.RowHeadersVisible = false;
			this.m_dgViewItemList.RowHeadersWidthSizeMode = System.Windows.Forms.DataGridViewRowHeadersWidthSizeMode.DisableResizing;
			this.m_dgViewItemList.RowTemplate.Height = 30;
			this.m_dgViewItemList.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
			this.m_dgViewItemList.Size = new System.Drawing.Size(434, 690);
			this.m_dgViewItemList.TabIndex = 1479;
			this.m_dgViewItemList.CellClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.Click_Configuration);
			// 
			// dataGridViewTextBoxColumn2
			// 
			this.dataGridViewTextBoxColumn2.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
			this.dataGridViewTextBoxColumn2.HeaderText = "ITEM";
			this.dataGridViewTextBoxColumn2.MaxInputLength = 30;
			this.dataGridViewTextBoxColumn2.Name = "dataGridViewTextBoxColumn2";
			this.dataGridViewTextBoxColumn2.ReadOnly = true;
			this.dataGridViewTextBoxColumn2.Resizable = System.Windows.Forms.DataGridViewTriState.False;
			// 
			// dataGridViewTextBoxColumn4
			// 
			this.dataGridViewTextBoxColumn4.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
			this.dataGridViewTextBoxColumn4.HeaderText = "VALUE";
			this.dataGridViewTextBoxColumn4.MaxInputLength = 20;
			this.dataGridViewTextBoxColumn4.Name = "dataGridViewTextBoxColumn4";
			this.dataGridViewTextBoxColumn4.ReadOnly = true;
			this.dataGridViewTextBoxColumn4.Resizable = System.Windows.Forms.DataGridViewTriState.False;
			this.dataGridViewTextBoxColumn4.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
			// 
			// sys3GroupBox4
			// 
			sys3GroupBox4.BackGroundColor = System.Drawing.Color.WhiteSmoke;
			sys3GroupBox4.Controls.Add(this.m_dgViewMonitoringData);
			sys3GroupBox4.Dock = System.Windows.Forms.DockStyle.Fill;
			sys3GroupBox4.EdgeBorderStroke = 2;
			sys3GroupBox4.EdgeRadius = 2;
			sys3GroupBox4.LabelFont = new System.Drawing.Font("맑은 고딕", 15F, System.Drawing.FontStyle.Bold);
			sys3GroupBox4.LabelGradientColorFirst = System.Drawing.Color.FromArgb(((int)(((byte)(208)))), ((int)(((byte)(208)))), ((int)(((byte)(208)))));
			sys3GroupBox4.LabelGradientColorSecond = System.Drawing.Color.FromArgb(((int)(((byte)(240)))), ((int)(((byte)(240)))), ((int)(((byte)(240)))));
			sys3GroupBox4.LabelHeight = 42;
			sys3GroupBox4.LabelTextColor = System.Drawing.Color.Black;
			sys3GroupBox4.Location = new System.Drawing.Point(755, 0);
			sys3GroupBox4.Margin = new System.Windows.Forms.Padding(0);
			sys3GroupBox4.Name = "sys3GroupBox4";
			sys3GroupBox4.Padding = new System.Windows.Forms.Padding(5, 30, 5, 5);
			sys3GroupBox4.Size = new System.Drawing.Size(401, 739);
			sys3GroupBox4.TabIndex = 1480;
			sys3GroupBox4.TabStop = false;
			sys3GroupBox4.Text = "MONITORING DATA";
			sys3GroupBox4.TextAlign = Sys3Controls.EN_TEXTALIGN.MIDDLE_CENTER;
			sys3GroupBox4.ThemeIndex = 0;
			sys3GroupBox4.UseLabelBorder = true;
			sys3GroupBox4.UseTitle = true;
			// 
			// m_dgViewMonitoringData
			// 
			this.m_dgViewMonitoringData.AllowUserToAddRows = false;
			this.m_dgViewMonitoringData.AllowUserToDeleteRows = false;
			this.m_dgViewMonitoringData.AllowUserToResizeColumns = false;
			this.m_dgViewMonitoringData.AllowUserToResizeRows = false;
			this.m_dgViewMonitoringData.BackgroundColor = System.Drawing.Color.White;
			this.m_dgViewMonitoringData.CellBorderStyle = System.Windows.Forms.DataGridViewCellBorderStyle.Raised;
			this.m_dgViewMonitoringData.ColumnHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.Single;
			dataGridViewCellStyle16.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
			dataGridViewCellStyle16.BackColor = System.Drawing.Color.LightGray;
			dataGridViewCellStyle16.Font = new System.Drawing.Font("맑은 고딕", 9F, System.Drawing.FontStyle.Bold);
			dataGridViewCellStyle16.ForeColor = System.Drawing.Color.Black;
			dataGridViewCellStyle16.SelectionBackColor = System.Drawing.Color.White;
			dataGridViewCellStyle16.SelectionForeColor = System.Drawing.Color.Black;
			dataGridViewCellStyle16.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
			this.m_dgViewMonitoringData.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle16;
			this.m_dgViewMonitoringData.ColumnHeadersHeight = 30;
			this.m_dgViewMonitoringData.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
			this.m_dgViewMonitoringData.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.dataGridViewTextBoxColumn5,
            this.dataGridViewTextBoxColumn6});
			dataGridViewCellStyle17.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
			dataGridViewCellStyle17.BackColor = System.Drawing.SystemColors.Window;
			dataGridViewCellStyle17.Font = new System.Drawing.Font("맑은 고딕", 10F, System.Drawing.FontStyle.Bold);
			dataGridViewCellStyle17.ForeColor = System.Drawing.SystemColors.ControlText;
			dataGridViewCellStyle17.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(120)))), ((int)(((byte)(130)))), ((int)(((byte)(150)))));
			dataGridViewCellStyle17.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
			dataGridViewCellStyle17.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
			this.m_dgViewMonitoringData.DefaultCellStyle = dataGridViewCellStyle17;
			this.m_dgViewMonitoringData.Dock = System.Windows.Forms.DockStyle.Fill;
			this.m_dgViewMonitoringData.EnableHeadersVisualStyles = false;
			this.m_dgViewMonitoringData.GridColor = System.Drawing.Color.FromArgb(((int)(((byte)(120)))), ((int)(((byte)(130)))), ((int)(((byte)(150)))));
			this.m_dgViewMonitoringData.Location = new System.Drawing.Point(5, 44);
			this.m_dgViewMonitoringData.MultiSelect = false;
			this.m_dgViewMonitoringData.Name = "m_dgViewMonitoringData";
			this.m_dgViewMonitoringData.ReadOnly = true;
			this.m_dgViewMonitoringData.RowHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.None;
			dataGridViewCellStyle18.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
			dataGridViewCellStyle18.BackColor = System.Drawing.Color.White;
			dataGridViewCellStyle18.Font = new System.Drawing.Font("맑은 고딕", 11F);
			dataGridViewCellStyle18.ForeColor = System.Drawing.SystemColors.WindowText;
			dataGridViewCellStyle18.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(120)))), ((int)(((byte)(130)))), ((int)(((byte)(150)))));
			dataGridViewCellStyle18.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
			dataGridViewCellStyle18.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
			this.m_dgViewMonitoringData.RowHeadersDefaultCellStyle = dataGridViewCellStyle18;
			this.m_dgViewMonitoringData.RowHeadersVisible = false;
			this.m_dgViewMonitoringData.RowHeadersWidthSizeMode = System.Windows.Forms.DataGridViewRowHeadersWidthSizeMode.DisableResizing;
			this.m_dgViewMonitoringData.RowTemplate.Height = 30;
			this.m_dgViewMonitoringData.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
			this.m_dgViewMonitoringData.Size = new System.Drawing.Size(391, 690);
			this.m_dgViewMonitoringData.TabIndex = 1481;
			this.m_dgViewMonitoringData.CellClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.Click_dgViewMonitoringData);
			// 
			// dataGridViewTextBoxColumn5
			// 
			this.dataGridViewTextBoxColumn5.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
			this.dataGridViewTextBoxColumn5.HeaderText = "ITEM";
			this.dataGridViewTextBoxColumn5.MaxInputLength = 30;
			this.dataGridViewTextBoxColumn5.Name = "dataGridViewTextBoxColumn5";
			this.dataGridViewTextBoxColumn5.ReadOnly = true;
			this.dataGridViewTextBoxColumn5.Resizable = System.Windows.Forms.DataGridViewTriState.False;
			// 
			// dataGridViewTextBoxColumn6
			// 
			this.dataGridViewTextBoxColumn6.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
			this.dataGridViewTextBoxColumn6.HeaderText = "VALUE";
			this.dataGridViewTextBoxColumn6.MaxInputLength = 20;
			this.dataGridViewTextBoxColumn6.Name = "dataGridViewTextBoxColumn6";
			this.dataGridViewTextBoxColumn6.ReadOnly = true;
			this.dataGridViewTextBoxColumn6.Resizable = System.Windows.Forms.DataGridViewTriState.False;
			this.dataGridViewTextBoxColumn6.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
			// 
			// tableLayoutPanel1
			// 
			this.tableLayoutPanel1.ColumnCount = 3;
			this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 26.90311F));
			this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 38.40831F));
			this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 34.51557F));
			this.tableLayoutPanel1.Controls.Add(sys3GroupBox1, 0, 0);
			this.tableLayoutPanel1.Controls.Add(sys3GroupBox2, 1, 0);
			this.tableLayoutPanel1.Controls.Add(sys3GroupBox4, 2, 0);
			this.tableLayoutPanel1.Controls.Add(sys3GroupBox3, 0, 1);
			this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
			this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
			this.tableLayoutPanel1.Margin = new System.Windows.Forms.Padding(0);
			this.tableLayoutPanel1.Name = "tableLayoutPanel1";
			this.tableLayoutPanel1.RowCount = 2;
			this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 82.22222F));
			this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 17.77778F));
			this.tableLayoutPanel1.Size = new System.Drawing.Size(1156, 900);
			this.tableLayoutPanel1.TabIndex = 1482;
			// 
			// m_btnRemove
			// 
			this.m_btnRemove.BorderWidth = 3;
			this.m_btnRemove.ButtonClicked = false;
			this.m_btnRemove.ClickedEmphasizeTextColor = System.Drawing.Color.White;
			this.m_btnRemove.CustomClickedGradientFirstColor = System.Drawing.Color.White;
			this.m_btnRemove.CustomClickedGradientSecondColor = System.Drawing.Color.White;
			this.m_btnRemove.Description = "";
			this.m_btnRemove.DisabledColor = System.Drawing.Color.DarkGray;
			this.m_btnRemove.Dock = System.Windows.Forms.DockStyle.Fill;
			this.m_btnRemove.EdgeRadius = 5;
			this.m_btnRemove.Enabled = false;
			this.m_btnRemove.GradientAngle = 70F;
			this.m_btnRemove.GradientFirstColor = System.Drawing.Color.White;
			this.m_btnRemove.GradientSecondColor = System.Drawing.Color.FromArgb(((int)(((byte)(170)))), ((int)(((byte)(176)))), ((int)(((byte)(183)))));
			this.m_btnRemove.HoverEmphasizeCustomColor = System.Drawing.Color.White;
			this.m_btnRemove.ImagePosition = new System.Drawing.Point(7, 7);
			this.m_btnRemove.ImageSize = new System.Drawing.Point(30, 30);
			this.m_btnRemove.LoadImage = global::FrameOfSystem3.Properties.Resources.REMOVE;
			this.m_btnRemove.Location = new System.Drawing.Point(488, 17);
			this.m_btnRemove.MainFont = new System.Drawing.Font("맑은 고딕", 13F, System.Drawing.FontStyle.Bold);
			this.m_btnRemove.MainFontColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(36)))), ((int)(((byte)(0)))));
			this.m_btnRemove.Name = "m_btnRemove";
			this.m_btnRemove.Size = new System.Drawing.Size(136, 76);
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
			this.m_btnRemove.UseEdge = true;
			this.m_btnRemove.UseHoverEmphasizeCustomColor = false;
			this.m_btnRemove.UseImage = true;
			this.m_btnRemove.UserHoverEmpahsize = false;
			this.m_btnRemove.UseSubFont = false;
			// 
			// m_btnAdd
			// 
			this.m_btnAdd.BorderWidth = 3;
			this.m_btnAdd.ButtonClicked = false;
			this.m_btnAdd.ClickedEmphasizeTextColor = System.Drawing.Color.White;
			this.m_btnAdd.CustomClickedGradientFirstColor = System.Drawing.Color.White;
			this.m_btnAdd.CustomClickedGradientSecondColor = System.Drawing.Color.White;
			this.m_btnAdd.Description = "";
			this.m_btnAdd.DisabledColor = System.Drawing.Color.DarkGray;
			this.m_btnAdd.Dock = System.Windows.Forms.DockStyle.Fill;
			this.m_btnAdd.EdgeRadius = 5;
			this.m_btnAdd.Enabled = false;
			this.m_btnAdd.GradientAngle = 70F;
			this.m_btnAdd.GradientFirstColor = System.Drawing.Color.White;
			this.m_btnAdd.GradientSecondColor = System.Drawing.Color.FromArgb(((int)(((byte)(170)))), ((int)(((byte)(176)))), ((int)(((byte)(183)))));
			this.m_btnAdd.HoverEmphasizeCustomColor = System.Drawing.Color.White;
			this.m_btnAdd.ImagePosition = new System.Drawing.Point(7, 7);
			this.m_btnAdd.ImageSize = new System.Drawing.Point(30, 30);
			this.m_btnAdd.LoadImage = global::FrameOfSystem3.Properties.Resources.CONFIG_ADD3;
			this.m_btnAdd.Location = new System.Drawing.Point(346, 17);
			this.m_btnAdd.MainFont = new System.Drawing.Font("맑은 고딕", 13F, System.Drawing.FontStyle.Bold);
			this.m_btnAdd.MainFontColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(36)))), ((int)(((byte)(0)))));
			this.m_btnAdd.Name = "m_btnAdd";
			this.m_btnAdd.Size = new System.Drawing.Size(136, 76);
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
			this.m_btnAdd.UseEdge = true;
			this.m_btnAdd.UseHoverEmphasizeCustomColor = false;
			this.m_btnAdd.UseImage = true;
			this.m_btnAdd.UserHoverEmpahsize = false;
			this.m_btnAdd.UseSubFont = false;
			// 
			// Config_Tool
			// 
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
			this.BackColor = System.Drawing.SystemColors.Control;
			this.Controls.Add(this.tableLayoutPanel1);
			this.Margin = new System.Windows.Forms.Padding(0);
			this.Name = "Config_Tool";
			this.Size = new System.Drawing.Size(1156, 900);
			sys3GroupBox1.ResumeLayout(false);
			((System.ComponentModel.ISupportInitialize)(this.m_dgViewToolList)).EndInit();
			sys3GroupBox3.ResumeLayout(false);
			this.tableLayoutPanel2.ResumeLayout(false);
			sys3GroupBox2.ResumeLayout(false);
			((System.ComponentModel.ISupportInitialize)(this.m_dgViewItemList)).EndInit();
			sys3GroupBox4.ResumeLayout(false);
			((System.ComponentModel.ISupportInitialize)(this.m_dgViewMonitoringData)).EndInit();
			this.tableLayoutPanel1.ResumeLayout(false);
			this.ResumeLayout(false);

        }

        #endregion
		private Sys3Controls.Sys3button btn_Disable;
		private Sys3Controls.Sys3button btn_Enable;
        private Sys3Controls.Sys3DoubleBufferedDataGridView m_dgViewToolList;
        private Sys3Controls.Sys3DoubleBufferedDataGridView m_dgViewItemList;
        private Sys3Controls.Sys3DoubleBufferedDataGridView m_dgViewMonitoringData;
		private Sys3Controls.Sys3button btn_Reset;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn3;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn1;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn2;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn4;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn5;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn6;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel2;
		private Sys3Controls.Sys3button m_btnRemove;
		private Sys3Controls.Sys3button m_btnAdd;
	}
}
