namespace FrameOfSystem3.Views.Config
{
    partial class Config_Digital
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
			System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle4 = new System.Windows.Forms.DataGridViewCellStyle();
			System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle5 = new System.Windows.Forms.DataGridViewCellStyle();
			System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle6 = new System.Windows.Forms.DataGridViewCellStyle();
			this.m_dgViewOutput = new Sys3Controls.Sys3DoubleBufferedDataGridView();
			this.dataGridViewTextBoxColumn1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.dataGridViewTextBoxColumn2 = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.dataGridViewTextBoxColumn3 = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.TAG1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.dataGridViewTextBoxColumn4 = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.ON_DELAY = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.OFF_DELAY = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.dataGridViewTextBoxColumn5 = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.dataGridViewTextBoxColumn6 = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.m_dgViewInput = new Sys3Controls.Sys3DoubleBufferedDataGridView();
			this.ID = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.ENABLE = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.PASSWORD = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.TAG = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.MONITORING = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.DELAY = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.DELAYOUT = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.CONDITION = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.Led = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.m_ToggleIndex = new Sys3Controls.Sys3ToggleButton();
			this.m_ToggleName = new Sys3Controls.Sys3ToggleButton();
			this.m_ToggleTarget = new Sys3Controls.Sys3ToggleButton();
			this.m_ToggleOnDelay = new Sys3Controls.Sys3ToggleButton();
			this.m_ToggleOffDelay = new Sys3Controls.Sys3ToggleButton();
			this.m_ToggleReverse = new Sys3Controls.Sys3ToggleButton();
			this.m_lblIndex = new Sys3Controls.Sys3Label();
			this.m_lblName = new Sys3Controls.Sys3Label();
			this.m_lblTarget = new Sys3Controls.Sys3Label();
			this.m_lblOnDelay = new Sys3Controls.Sys3Label();
			this.m_lblOffDelay = new Sys3Controls.Sys3Label();
			this.m_lblReverse = new Sys3Controls.Sys3Label();
			this.m_groupViewOptionInput = new Sys3Controls.Sys3GroupBox();
			this.m_groupList = new Sys3Controls.Sys3GroupBox();
			this.m_btnExtend = new Sys3Controls.Sys3button();
			this.m_btnAdd = new Sys3Controls.Sys3button();
			this.m_btnRemove = new Sys3Controls.Sys3button();
			this._groupBox_ChangeList = new Sys3Controls.Sys3GroupBox();
			this.m_lblTag = new Sys3Controls.Sys3Label();
			this.m_ToggleTag = new Sys3Controls.Sys3ToggleButton();
			this.m_btnOutput = new Sys3Controls.Sys3button();
			this.m_btnInput = new Sys3Controls.Sys3button();
			this._label_OptionReserved = new Sys3Controls.Sys3Label();
			this.m_btnIO = new Sys3Controls.Sys3button();
			this.m_ViewInput = new Sys3Controls.Sys3button();
			this.m_ViewOutPut = new Sys3Controls.Sys3button();
			this._tableLayoutPanel_Main = new System.Windows.Forms.TableLayoutPanel();
			this._tableLayoutPanel_GridView = new System.Windows.Forms.TableLayoutPanel();
			this._panel_ItemList = new System.Windows.Forms.Panel();
			this._flowLayoutPanel_AddRemove = new System.Windows.Forms.FlowLayoutPanel();
			this.lbl_Filtering = new Sys3Controls.Sys3Label();
			this._tableLayoutPanel_Option = new System.Windows.Forms.TableLayoutPanel();
			this._panel_Reserve = new System.Windows.Forms.Panel();
			this._panel_Reverse = new System.Windows.Forms.Panel();
			this._panel_OffDelay = new System.Windows.Forms.Panel();
			this._panel_OnDelay = new System.Windows.Forms.Panel();
			this._panel_TagNumber = new System.Windows.Forms.Panel();
			this._panel_Target = new System.Windows.Forms.Panel();
			this._panel_Index = new System.Windows.Forms.Panel();
			this._panel_Name = new System.Windows.Forms.Panel();
			((System.ComponentModel.ISupportInitialize)(this.m_dgViewOutput)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.m_dgViewInput)).BeginInit();
			this._tableLayoutPanel_Main.SuspendLayout();
			this._tableLayoutPanel_GridView.SuspendLayout();
			this._panel_ItemList.SuspendLayout();
			this._flowLayoutPanel_AddRemove.SuspendLayout();
			this._tableLayoutPanel_Option.SuspendLayout();
			this._panel_Reserve.SuspendLayout();
			this._panel_Reverse.SuspendLayout();
			this._panel_OffDelay.SuspendLayout();
			this._panel_OnDelay.SuspendLayout();
			this._panel_TagNumber.SuspendLayout();
			this._panel_Target.SuspendLayout();
			this._panel_Index.SuspendLayout();
			this._panel_Name.SuspendLayout();
			this.SuspendLayout();
			// 
			// m_dgViewOutput
			// 
			this.m_dgViewOutput.AllowUserToAddRows = false;
			this.m_dgViewOutput.AllowUserToDeleteRows = false;
			this.m_dgViewOutput.AllowUserToResizeColumns = false;
			this.m_dgViewOutput.AllowUserToResizeRows = false;
			this.m_dgViewOutput.BackgroundColor = System.Drawing.Color.White;
			this.m_dgViewOutput.CellBorderStyle = System.Windows.Forms.DataGridViewCellBorderStyle.Raised;
			this.m_dgViewOutput.ColumnHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.Single;
			dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
			dataGridViewCellStyle1.BackColor = System.Drawing.Color.LightGray;
			dataGridViewCellStyle1.Font = new System.Drawing.Font("맑은 고딕", 11F, System.Drawing.FontStyle.Bold);
			dataGridViewCellStyle1.ForeColor = System.Drawing.Color.Black;
			dataGridViewCellStyle1.SelectionBackColor = System.Drawing.Color.White;
			dataGridViewCellStyle1.SelectionForeColor = System.Drawing.Color.Black;
			dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
			this.m_dgViewOutput.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
			this.m_dgViewOutput.ColumnHeadersHeight = 30;
			this.m_dgViewOutput.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
			this.m_dgViewOutput.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.dataGridViewTextBoxColumn1,
            this.dataGridViewTextBoxColumn2,
            this.dataGridViewTextBoxColumn3,
            this.TAG1,
            this.dataGridViewTextBoxColumn4,
            this.ON_DELAY,
            this.OFF_DELAY,
            this.dataGridViewTextBoxColumn5,
            this.dataGridViewTextBoxColumn6});
			dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
			dataGridViewCellStyle2.BackColor = System.Drawing.SystemColors.Window;
			dataGridViewCellStyle2.Font = new System.Drawing.Font("맑은 고딕", 10F, System.Drawing.FontStyle.Bold);
			dataGridViewCellStyle2.ForeColor = System.Drawing.SystemColors.ControlText;
			dataGridViewCellStyle2.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(120)))), ((int)(((byte)(130)))), ((int)(((byte)(150)))));
			dataGridViewCellStyle2.SelectionForeColor = System.Drawing.Color.White;
			dataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
			this.m_dgViewOutput.DefaultCellStyle = dataGridViewCellStyle2;
			this.m_dgViewOutput.Dock = System.Windows.Forms.DockStyle.Fill;
			this.m_dgViewOutput.EnableHeadersVisualStyles = false;
			this.m_dgViewOutput.GridColor = System.Drawing.Color.FromArgb(((int)(((byte)(120)))), ((int)(((byte)(130)))), ((int)(((byte)(150)))));
			this.m_dgViewOutput.Location = new System.Drawing.Point(72, 356);
			this.m_dgViewOutput.Margin = new System.Windows.Forms.Padding(0, 1, 0, 1);
			this.m_dgViewOutput.MultiSelect = false;
			this.m_dgViewOutput.Name = "m_dgViewOutput";
			this.m_dgViewOutput.ReadOnly = true;
			this.m_dgViewOutput.RowHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.None;
			dataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
			dataGridViewCellStyle3.BackColor = System.Drawing.Color.White;
			dataGridViewCellStyle3.Font = new System.Drawing.Font("맑은 고딕", 11F);
			dataGridViewCellStyle3.ForeColor = System.Drawing.SystemColors.WindowText;
			dataGridViewCellStyle3.SelectionBackColor = System.Drawing.Color.White;
			dataGridViewCellStyle3.SelectionForeColor = System.Drawing.SystemColors.WindowText;
			dataGridViewCellStyle3.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
			this.m_dgViewOutput.RowHeadersDefaultCellStyle = dataGridViewCellStyle3;
			this.m_dgViewOutput.RowHeadersVisible = false;
			this.m_dgViewOutput.RowHeadersWidthSizeMode = System.Windows.Forms.DataGridViewRowHeadersWidthSizeMode.DisableResizing;
			this.m_dgViewOutput.RowTemplate.Height = 23;
			this.m_dgViewOutput.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
			this.m_dgViewOutput.Size = new System.Drawing.Size(1084, 353);
			this.m_dgViewOutput.TabIndex = 1;
			this.m_dgViewOutput.Visible = false;
			this.m_dgViewOutput.CellClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.Click_ItemList);
			this.m_dgViewOutput.CellDoubleClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.DoubleClick_OutputItem);
			this.m_dgViewOutput.Click += new System.EventHandler(this.Click_DataGrid);
			// 
			// dataGridViewTextBoxColumn1
			// 
			this.dataGridViewTextBoxColumn1.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.ColumnHeader;
			this.dataGridViewTextBoxColumn1.Frozen = true;
			this.dataGridViewTextBoxColumn1.HeaderText = "INDEX";
			this.dataGridViewTextBoxColumn1.MaxInputLength = 20;
			this.dataGridViewTextBoxColumn1.Name = "dataGridViewTextBoxColumn1";
			this.dataGridViewTextBoxColumn1.ReadOnly = true;
			this.dataGridViewTextBoxColumn1.Resizable = System.Windows.Forms.DataGridViewTriState.False;
			this.dataGridViewTextBoxColumn1.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
			this.dataGridViewTextBoxColumn1.Width = 60;
			// 
			// dataGridViewTextBoxColumn2
			// 
			this.dataGridViewTextBoxColumn2.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.ColumnHeader;
			this.dataGridViewTextBoxColumn2.Frozen = true;
			this.dataGridViewTextBoxColumn2.HeaderText = "ENABLE";
			this.dataGridViewTextBoxColumn2.Name = "dataGridViewTextBoxColumn2";
			this.dataGridViewTextBoxColumn2.ReadOnly = true;
			this.dataGridViewTextBoxColumn2.Resizable = System.Windows.Forms.DataGridViewTriState.False;
			this.dataGridViewTextBoxColumn2.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
			this.dataGridViewTextBoxColumn2.Width = 71;
			// 
			// dataGridViewTextBoxColumn3
			// 
			this.dataGridViewTextBoxColumn3.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
			this.dataGridViewTextBoxColumn3.HeaderText = "NAME";
			this.dataGridViewTextBoxColumn3.MaxInputLength = 20;
			this.dataGridViewTextBoxColumn3.Name = "dataGridViewTextBoxColumn3";
			this.dataGridViewTextBoxColumn3.ReadOnly = true;
			this.dataGridViewTextBoxColumn3.Resizable = System.Windows.Forms.DataGridViewTriState.False;
			this.dataGridViewTextBoxColumn3.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
			// 
			// TAG1
			// 
			this.TAG1.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.ColumnHeader;
			this.TAG1.HeaderText = "TAG NUMBER";
			this.TAG1.Name = "TAG1";
			this.TAG1.ReadOnly = true;
			this.TAG1.Resizable = System.Windows.Forms.DataGridViewTriState.False;
			this.TAG1.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
			this.TAG1.Width = 115;
			// 
			// dataGridViewTextBoxColumn4
			// 
			this.dataGridViewTextBoxColumn4.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.ColumnHeader;
			this.dataGridViewTextBoxColumn4.HeaderText = "TARGET";
			this.dataGridViewTextBoxColumn4.Name = "dataGridViewTextBoxColumn4";
			this.dataGridViewTextBoxColumn4.ReadOnly = true;
			this.dataGridViewTextBoxColumn4.Resizable = System.Windows.Forms.DataGridViewTriState.False;
			this.dataGridViewTextBoxColumn4.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
			this.dataGridViewTextBoxColumn4.Width = 72;
			// 
			// ON_DELAY
			// 
			this.ON_DELAY.HeaderText = "ON DELAY";
			this.ON_DELAY.Name = "ON_DELAY";
			this.ON_DELAY.ReadOnly = true;
			this.ON_DELAY.Width = 90;
			// 
			// OFF_DELAY
			// 
			this.OFF_DELAY.HeaderText = "OFF DELAY";
			this.OFF_DELAY.Name = "OFF_DELAY";
			this.OFF_DELAY.ReadOnly = true;
			// 
			// dataGridViewTextBoxColumn5
			// 
			this.dataGridViewTextBoxColumn5.HeaderText = "REVERSE";
			this.dataGridViewTextBoxColumn5.Name = "dataGridViewTextBoxColumn5";
			this.dataGridViewTextBoxColumn5.ReadOnly = true;
			this.dataGridViewTextBoxColumn5.Resizable = System.Windows.Forms.DataGridViewTriState.False;
			this.dataGridViewTextBoxColumn5.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
			this.dataGridViewTextBoxColumn5.Width = 90;
			// 
			// dataGridViewTextBoxColumn6
			// 
			this.dataGridViewTextBoxColumn6.HeaderText = "ON / OFF";
			this.dataGridViewTextBoxColumn6.Name = "dataGridViewTextBoxColumn6";
			this.dataGridViewTextBoxColumn6.ReadOnly = true;
			this.dataGridViewTextBoxColumn6.Resizable = System.Windows.Forms.DataGridViewTriState.False;
			this.dataGridViewTextBoxColumn6.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
			this.dataGridViewTextBoxColumn6.Width = 90;
			// 
			// m_dgViewInput
			// 
			this.m_dgViewInput.AllowUserToAddRows = false;
			this.m_dgViewInput.AllowUserToDeleteRows = false;
			this.m_dgViewInput.AllowUserToResizeColumns = false;
			this.m_dgViewInput.AllowUserToResizeRows = false;
			this.m_dgViewInput.BackgroundColor = System.Drawing.Color.White;
			this.m_dgViewInput.CellBorderStyle = System.Windows.Forms.DataGridViewCellBorderStyle.Raised;
			this.m_dgViewInput.ColumnHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.Single;
			dataGridViewCellStyle4.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
			dataGridViewCellStyle4.BackColor = System.Drawing.Color.LightGray;
			dataGridViewCellStyle4.Font = new System.Drawing.Font("맑은 고딕", 11F, System.Drawing.FontStyle.Bold);
			dataGridViewCellStyle4.ForeColor = System.Drawing.Color.Black;
			dataGridViewCellStyle4.SelectionBackColor = System.Drawing.Color.White;
			dataGridViewCellStyle4.SelectionForeColor = System.Drawing.Color.Black;
			dataGridViewCellStyle4.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
			this.m_dgViewInput.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle4;
			this.m_dgViewInput.ColumnHeadersHeight = 30;
			this.m_dgViewInput.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
			this.m_dgViewInput.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.ID,
            this.ENABLE,
            this.PASSWORD,
            this.TAG,
            this.MONITORING,
            this.DELAY,
            this.DELAYOUT,
            this.CONDITION,
            this.Led});
			dataGridViewCellStyle5.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
			dataGridViewCellStyle5.BackColor = System.Drawing.SystemColors.Window;
			dataGridViewCellStyle5.Font = new System.Drawing.Font("맑은 고딕", 10F, System.Drawing.FontStyle.Bold);
			dataGridViewCellStyle5.ForeColor = System.Drawing.SystemColors.WindowText;
			dataGridViewCellStyle5.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(120)))), ((int)(((byte)(130)))), ((int)(((byte)(150)))));
			dataGridViewCellStyle5.SelectionForeColor = System.Drawing.Color.White;
			dataGridViewCellStyle5.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
			this.m_dgViewInput.DefaultCellStyle = dataGridViewCellStyle5;
			this.m_dgViewInput.Dock = System.Windows.Forms.DockStyle.Fill;
			this.m_dgViewInput.EnableHeadersVisualStyles = false;
			this.m_dgViewInput.GridColor = System.Drawing.Color.FromArgb(((int)(((byte)(120)))), ((int)(((byte)(130)))), ((int)(((byte)(150)))));
			this.m_dgViewInput.Location = new System.Drawing.Point(72, 1);
			this.m_dgViewInput.Margin = new System.Windows.Forms.Padding(0, 1, 0, 1);
			this.m_dgViewInput.MultiSelect = false;
			this.m_dgViewInput.Name = "m_dgViewInput";
			this.m_dgViewInput.ReadOnly = true;
			this.m_dgViewInput.RowHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.None;
			dataGridViewCellStyle6.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
			dataGridViewCellStyle6.BackColor = System.Drawing.Color.White;
			dataGridViewCellStyle6.Font = new System.Drawing.Font("맑은 고딕", 11F);
			dataGridViewCellStyle6.ForeColor = System.Drawing.SystemColors.WindowText;
			dataGridViewCellStyle6.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(120)))), ((int)(((byte)(130)))), ((int)(((byte)(150)))));
			dataGridViewCellStyle6.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
			dataGridViewCellStyle6.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
			this.m_dgViewInput.RowHeadersDefaultCellStyle = dataGridViewCellStyle6;
			this.m_dgViewInput.RowHeadersVisible = false;
			this.m_dgViewInput.RowHeadersWidthSizeMode = System.Windows.Forms.DataGridViewRowHeadersWidthSizeMode.DisableResizing;
			this.m_dgViewInput.RowTemplate.Height = 23;
			this.m_dgViewInput.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
			this.m_dgViewInput.Size = new System.Drawing.Size(1084, 353);
			this.m_dgViewInput.TabIndex = 0;
			this.m_dgViewInput.CellClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.Click_ItemList);
			this.m_dgViewInput.CellDoubleClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.DoubleClick_InputItem);
			this.m_dgViewInput.Click += new System.EventHandler(this.Click_DataGrid);
			// 
			// ID
			// 
			this.ID.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.ColumnHeader;
			this.ID.Frozen = true;
			this.ID.HeaderText = "INDEX";
			this.ID.MaxInputLength = 20;
			this.ID.Name = "ID";
			this.ID.ReadOnly = true;
			this.ID.Resizable = System.Windows.Forms.DataGridViewTriState.False;
			this.ID.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
			this.ID.Width = 60;
			// 
			// ENABLE
			// 
			this.ENABLE.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.ColumnHeader;
			this.ENABLE.Frozen = true;
			this.ENABLE.HeaderText = "ENABLE";
			this.ENABLE.Name = "ENABLE";
			this.ENABLE.ReadOnly = true;
			this.ENABLE.Resizable = System.Windows.Forms.DataGridViewTriState.False;
			this.ENABLE.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
			this.ENABLE.Width = 71;
			// 
			// PASSWORD
			// 
			this.PASSWORD.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
			this.PASSWORD.HeaderText = "NAME";
			this.PASSWORD.MaxInputLength = 20;
			this.PASSWORD.Name = "PASSWORD";
			this.PASSWORD.ReadOnly = true;
			this.PASSWORD.Resizable = System.Windows.Forms.DataGridViewTriState.False;
			this.PASSWORD.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
			// 
			// TAG
			// 
			this.TAG.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.ColumnHeader;
			this.TAG.HeaderText = "TAG NUMBER";
			this.TAG.Name = "TAG";
			this.TAG.ReadOnly = true;
			this.TAG.Resizable = System.Windows.Forms.DataGridViewTriState.False;
			this.TAG.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
			this.TAG.Width = 115;
			// 
			// MONITORING
			// 
			this.MONITORING.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.ColumnHeader;
			this.MONITORING.HeaderText = "TARGET";
			this.MONITORING.Name = "MONITORING";
			this.MONITORING.ReadOnly = true;
			this.MONITORING.Resizable = System.Windows.Forms.DataGridViewTriState.False;
			this.MONITORING.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
			this.MONITORING.Width = 72;
			// 
			// DELAY
			// 
			this.DELAY.HeaderText = "ON DELAY";
			this.DELAY.Name = "DELAY";
			this.DELAY.ReadOnly = true;
			this.DELAY.Resizable = System.Windows.Forms.DataGridViewTriState.False;
			this.DELAY.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
			this.DELAY.Width = 90;
			// 
			// DELAYOUT
			// 
			this.DELAYOUT.HeaderText = "OFF DELAY";
			this.DELAYOUT.Name = "DELAYOUT";
			this.DELAYOUT.ReadOnly = true;
			this.DELAYOUT.Resizable = System.Windows.Forms.DataGridViewTriState.False;
			this.DELAYOUT.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
			// 
			// CONDITION
			// 
			this.CONDITION.HeaderText = "REVERSE";
			this.CONDITION.Name = "CONDITION";
			this.CONDITION.ReadOnly = true;
			this.CONDITION.Resizable = System.Windows.Forms.DataGridViewTriState.False;
			this.CONDITION.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
			this.CONDITION.Width = 90;
			// 
			// Led
			// 
			this.Led.HeaderText = "ON / OFF";
			this.Led.Name = "Led";
			this.Led.ReadOnly = true;
			this.Led.Resizable = System.Windows.Forms.DataGridViewTriState.False;
			this.Led.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
			this.Led.Width = 90;
			// 
			// m_ToggleIndex
			// 
			this.m_ToggleIndex.Active = true;
			this.m_ToggleIndex.ActiveColorFirst = System.Drawing.Color.RoyalBlue;
			this.m_ToggleIndex.ActiveColorSecond = System.Drawing.Color.DodgerBlue;
			this.m_ToggleIndex.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.m_ToggleIndex.BackColor = System.Drawing.Color.WhiteSmoke;
			this.m_ToggleIndex.Location = new System.Drawing.Point(130, 6);
			this.m_ToggleIndex.Name = "m_ToggleIndex";
			this.m_ToggleIndex.NormalColorFirst = System.Drawing.Color.DarkGray;
			this.m_ToggleIndex.NormalColorSecond = System.Drawing.Color.Silver;
			this.m_ToggleIndex.Size = new System.Drawing.Size(88, 44);
			this.m_ToggleIndex.TabIndex = 0;
			this.m_ToggleIndex.Click += new System.EventHandler(this.Click_ViewOption);
			this.m_ToggleIndex.DoubleClick += new System.EventHandler(this.Click_ViewOption);
			// 
			// m_ToggleName
			// 
			this.m_ToggleName.Active = true;
			this.m_ToggleName.ActiveColorFirst = System.Drawing.Color.RoyalBlue;
			this.m_ToggleName.ActiveColorSecond = System.Drawing.Color.DodgerBlue;
			this.m_ToggleName.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.m_ToggleName.BackColor = System.Drawing.Color.WhiteSmoke;
			this.m_ToggleName.Location = new System.Drawing.Point(130, 6);
			this.m_ToggleName.Name = "m_ToggleName";
			this.m_ToggleName.NormalColorFirst = System.Drawing.Color.DarkGray;
			this.m_ToggleName.NormalColorSecond = System.Drawing.Color.Silver;
			this.m_ToggleName.Size = new System.Drawing.Size(88, 44);
			this.m_ToggleName.TabIndex = 2;
			this.m_ToggleName.Click += new System.EventHandler(this.Click_ViewOption);
			this.m_ToggleName.DoubleClick += new System.EventHandler(this.Click_ViewOption);
			// 
			// m_ToggleTarget
			// 
			this.m_ToggleTarget.Active = true;
			this.m_ToggleTarget.ActiveColorFirst = System.Drawing.Color.RoyalBlue;
			this.m_ToggleTarget.ActiveColorSecond = System.Drawing.Color.DodgerBlue;
			this.m_ToggleTarget.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.m_ToggleTarget.BackColor = System.Drawing.Color.WhiteSmoke;
			this.m_ToggleTarget.Location = new System.Drawing.Point(130, 6);
			this.m_ToggleTarget.Name = "m_ToggleTarget";
			this.m_ToggleTarget.NormalColorFirst = System.Drawing.Color.DarkGray;
			this.m_ToggleTarget.NormalColorSecond = System.Drawing.Color.Silver;
			this.m_ToggleTarget.Size = new System.Drawing.Size(88, 44);
			this.m_ToggleTarget.TabIndex = 4;
			this.m_ToggleTarget.Click += new System.EventHandler(this.Click_ViewOption);
			this.m_ToggleTarget.DoubleClick += new System.EventHandler(this.Click_ViewOption);
			// 
			// m_ToggleOnDelay
			// 
			this.m_ToggleOnDelay.Active = true;
			this.m_ToggleOnDelay.ActiveColorFirst = System.Drawing.Color.RoyalBlue;
			this.m_ToggleOnDelay.ActiveColorSecond = System.Drawing.Color.DodgerBlue;
			this.m_ToggleOnDelay.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.m_ToggleOnDelay.BackColor = System.Drawing.Color.WhiteSmoke;
			this.m_ToggleOnDelay.Location = new System.Drawing.Point(128, 6);
			this.m_ToggleOnDelay.Name = "m_ToggleOnDelay";
			this.m_ToggleOnDelay.NormalColorFirst = System.Drawing.Color.DarkGray;
			this.m_ToggleOnDelay.NormalColorSecond = System.Drawing.Color.Silver;
			this.m_ToggleOnDelay.Size = new System.Drawing.Size(88, 44);
			this.m_ToggleOnDelay.TabIndex = 5;
			this.m_ToggleOnDelay.Click += new System.EventHandler(this.Click_ViewOption);
			this.m_ToggleOnDelay.DoubleClick += new System.EventHandler(this.Click_ViewOption);
			// 
			// m_ToggleOffDelay
			// 
			this.m_ToggleOffDelay.Active = true;
			this.m_ToggleOffDelay.ActiveColorFirst = System.Drawing.Color.RoyalBlue;
			this.m_ToggleOffDelay.ActiveColorSecond = System.Drawing.Color.DodgerBlue;
			this.m_ToggleOffDelay.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.m_ToggleOffDelay.BackColor = System.Drawing.Color.WhiteSmoke;
			this.m_ToggleOffDelay.Location = new System.Drawing.Point(128, 6);
			this.m_ToggleOffDelay.Name = "m_ToggleOffDelay";
			this.m_ToggleOffDelay.NormalColorFirst = System.Drawing.Color.DarkGray;
			this.m_ToggleOffDelay.NormalColorSecond = System.Drawing.Color.Silver;
			this.m_ToggleOffDelay.Size = new System.Drawing.Size(88, 44);
			this.m_ToggleOffDelay.TabIndex = 6;
			this.m_ToggleOffDelay.Click += new System.EventHandler(this.Click_ViewOption);
			this.m_ToggleOffDelay.DoubleClick += new System.EventHandler(this.Click_ViewOption);
			// 
			// m_ToggleReverse
			// 
			this.m_ToggleReverse.Active = true;
			this.m_ToggleReverse.ActiveColorFirst = System.Drawing.Color.RoyalBlue;
			this.m_ToggleReverse.ActiveColorSecond = System.Drawing.Color.DodgerBlue;
			this.m_ToggleReverse.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.m_ToggleReverse.BackColor = System.Drawing.Color.WhiteSmoke;
			this.m_ToggleReverse.Location = new System.Drawing.Point(128, 6);
			this.m_ToggleReverse.Name = "m_ToggleReverse";
			this.m_ToggleReverse.NormalColorFirst = System.Drawing.Color.DarkGray;
			this.m_ToggleReverse.NormalColorSecond = System.Drawing.Color.Silver;
			this.m_ToggleReverse.Size = new System.Drawing.Size(88, 44);
			this.m_ToggleReverse.TabIndex = 7;
			this.m_ToggleReverse.Click += new System.EventHandler(this.Click_ViewOption);
			this.m_ToggleReverse.DoubleClick += new System.EventHandler(this.Click_ViewOption);
			// 
			// m_lblIndex
			// 
			this.m_lblIndex.BackGroundColor = System.Drawing.Color.WhiteSmoke;
			this.m_lblIndex.BorderStroke = 2;
			this.m_lblIndex.BorderStyle = System.Windows.Forms.ButtonBorderStyle.Solid;
			this.m_lblIndex.Description = "";
			this.m_lblIndex.DisabledColor = System.Drawing.Color.Silver;
			this.m_lblIndex.Dock = System.Windows.Forms.DockStyle.Fill;
			this.m_lblIndex.EdgeRadius = 1;
			this.m_lblIndex.ImagePosition = new System.Drawing.Point(0, 0);
			this.m_lblIndex.ImageSize = new System.Drawing.Point(0, 0);
			this.m_lblIndex.LoadImage = null;
			this.m_lblIndex.Location = new System.Drawing.Point(0, 0);
			this.m_lblIndex.MainFont = new System.Drawing.Font("맑은 고딕", 12F, System.Drawing.FontStyle.Bold);
			this.m_lblIndex.MainFontColor = System.Drawing.Color.Black;
			this.m_lblIndex.Margin = new System.Windows.Forms.Padding(0);
			this.m_lblIndex.Name = "m_lblIndex";
			this.m_lblIndex.Size = new System.Drawing.Size(222, 56);
			this.m_lblIndex.SubFont = new System.Drawing.Font("맑은 고딕", 10F);
			this.m_lblIndex.SubFontColor = System.Drawing.Color.Black;
			this.m_lblIndex.SubText = "";
			this.m_lblIndex.TabIndex = 1347;
			this.m_lblIndex.Text = "INDEX";
			this.m_lblIndex.TextAlignMain = Sys3Controls.EN_TEXTALIGN.MIDDLE_LEFT;
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
			// m_lblName
			// 
			this.m_lblName.BackGroundColor = System.Drawing.Color.WhiteSmoke;
			this.m_lblName.BorderStroke = 2;
			this.m_lblName.BorderStyle = System.Windows.Forms.ButtonBorderStyle.Solid;
			this.m_lblName.Description = "";
			this.m_lblName.DisabledColor = System.Drawing.Color.Silver;
			this.m_lblName.Dock = System.Windows.Forms.DockStyle.Fill;
			this.m_lblName.EdgeRadius = 1;
			this.m_lblName.ImagePosition = new System.Drawing.Point(0, 0);
			this.m_lblName.ImageSize = new System.Drawing.Point(0, 0);
			this.m_lblName.LoadImage = null;
			this.m_lblName.Location = new System.Drawing.Point(0, 0);
			this.m_lblName.MainFont = new System.Drawing.Font("맑은 고딕", 12F, System.Drawing.FontStyle.Bold);
			this.m_lblName.MainFontColor = System.Drawing.Color.Black;
			this.m_lblName.Margin = new System.Windows.Forms.Padding(0);
			this.m_lblName.Name = "m_lblName";
			this.m_lblName.Size = new System.Drawing.Size(222, 56);
			this.m_lblName.SubFont = new System.Drawing.Font("맑은 고딕", 10F);
			this.m_lblName.SubFontColor = System.Drawing.Color.Black;
			this.m_lblName.SubText = "";
			this.m_lblName.TabIndex = 1347;
			this.m_lblName.Text = "NAME";
			this.m_lblName.TextAlignMain = Sys3Controls.EN_TEXTALIGN.MIDDLE_LEFT;
			this.m_lblName.TextAlignSub = Sys3Controls.EN_TEXTALIGN.MIDDLE_CENTER;
			this.m_lblName.TextAlignUnit = Sys3Controls.EN_TEXTALIGN.MIDDLE_CENTER;
			this.m_lblName.ThemeIndex = 0;
			this.m_lblName.UnitAreaRate = 40;
			this.m_lblName.UnitFont = new System.Drawing.Font("맑은 고딕", 10F, System.Drawing.FontStyle.Bold);
			this.m_lblName.UnitFontColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
			this.m_lblName.UnitPositionVertical = false;
			this.m_lblName.UnitText = "";
			this.m_lblName.UseBorder = true;
			this.m_lblName.UseEdgeRadius = false;
			this.m_lblName.UseImage = false;
			this.m_lblName.UseSubFont = false;
			this.m_lblName.UseUnitFont = false;
			// 
			// m_lblTarget
			// 
			this.m_lblTarget.BackGroundColor = System.Drawing.Color.WhiteSmoke;
			this.m_lblTarget.BorderStroke = 2;
			this.m_lblTarget.BorderStyle = System.Windows.Forms.ButtonBorderStyle.Solid;
			this.m_lblTarget.Description = "";
			this.m_lblTarget.DisabledColor = System.Drawing.Color.Silver;
			this.m_lblTarget.Dock = System.Windows.Forms.DockStyle.Fill;
			this.m_lblTarget.EdgeRadius = 1;
			this.m_lblTarget.ImagePosition = new System.Drawing.Point(0, 0);
			this.m_lblTarget.ImageSize = new System.Drawing.Point(0, 0);
			this.m_lblTarget.LoadImage = null;
			this.m_lblTarget.Location = new System.Drawing.Point(0, 0);
			this.m_lblTarget.MainFont = new System.Drawing.Font("맑은 고딕", 12F, System.Drawing.FontStyle.Bold);
			this.m_lblTarget.MainFontColor = System.Drawing.Color.Black;
			this.m_lblTarget.Margin = new System.Windows.Forms.Padding(0);
			this.m_lblTarget.Name = "m_lblTarget";
			this.m_lblTarget.Size = new System.Drawing.Size(222, 56);
			this.m_lblTarget.SubFont = new System.Drawing.Font("맑은 고딕", 10F);
			this.m_lblTarget.SubFontColor = System.Drawing.Color.Black;
			this.m_lblTarget.SubText = "";
			this.m_lblTarget.TabIndex = 1347;
			this.m_lblTarget.Text = "TARGET";
			this.m_lblTarget.TextAlignMain = Sys3Controls.EN_TEXTALIGN.MIDDLE_LEFT;
			this.m_lblTarget.TextAlignSub = Sys3Controls.EN_TEXTALIGN.MIDDLE_CENTER;
			this.m_lblTarget.TextAlignUnit = Sys3Controls.EN_TEXTALIGN.MIDDLE_CENTER;
			this.m_lblTarget.ThemeIndex = 0;
			this.m_lblTarget.UnitAreaRate = 40;
			this.m_lblTarget.UnitFont = new System.Drawing.Font("맑은 고딕", 10F, System.Drawing.FontStyle.Bold);
			this.m_lblTarget.UnitFontColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
			this.m_lblTarget.UnitPositionVertical = false;
			this.m_lblTarget.UnitText = "";
			this.m_lblTarget.UseBorder = true;
			this.m_lblTarget.UseEdgeRadius = false;
			this.m_lblTarget.UseImage = false;
			this.m_lblTarget.UseSubFont = false;
			this.m_lblTarget.UseUnitFont = false;
			// 
			// m_lblOnDelay
			// 
			this.m_lblOnDelay.BackGroundColor = System.Drawing.Color.WhiteSmoke;
			this.m_lblOnDelay.BorderStroke = 2;
			this.m_lblOnDelay.BorderStyle = System.Windows.Forms.ButtonBorderStyle.Solid;
			this.m_lblOnDelay.Description = "";
			this.m_lblOnDelay.DisabledColor = System.Drawing.Color.Silver;
			this.m_lblOnDelay.Dock = System.Windows.Forms.DockStyle.Fill;
			this.m_lblOnDelay.EdgeRadius = 1;
			this.m_lblOnDelay.ImagePosition = new System.Drawing.Point(0, 0);
			this.m_lblOnDelay.ImageSize = new System.Drawing.Point(0, 0);
			this.m_lblOnDelay.LoadImage = null;
			this.m_lblOnDelay.Location = new System.Drawing.Point(0, 0);
			this.m_lblOnDelay.MainFont = new System.Drawing.Font("맑은 고딕", 12F, System.Drawing.FontStyle.Bold);
			this.m_lblOnDelay.MainFontColor = System.Drawing.Color.Black;
			this.m_lblOnDelay.Margin = new System.Windows.Forms.Padding(0);
			this.m_lblOnDelay.Name = "m_lblOnDelay";
			this.m_lblOnDelay.Size = new System.Drawing.Size(222, 56);
			this.m_lblOnDelay.SubFont = new System.Drawing.Font("맑은 고딕", 10F);
			this.m_lblOnDelay.SubFontColor = System.Drawing.Color.Black;
			this.m_lblOnDelay.SubText = "";
			this.m_lblOnDelay.TabIndex = 1347;
			this.m_lblOnDelay.Text = "ON DELAY";
			this.m_lblOnDelay.TextAlignMain = Sys3Controls.EN_TEXTALIGN.MIDDLE_LEFT;
			this.m_lblOnDelay.TextAlignSub = Sys3Controls.EN_TEXTALIGN.MIDDLE_CENTER;
			this.m_lblOnDelay.TextAlignUnit = Sys3Controls.EN_TEXTALIGN.MIDDLE_CENTER;
			this.m_lblOnDelay.ThemeIndex = 0;
			this.m_lblOnDelay.UnitAreaRate = 40;
			this.m_lblOnDelay.UnitFont = new System.Drawing.Font("맑은 고딕", 10F, System.Drawing.FontStyle.Bold);
			this.m_lblOnDelay.UnitFontColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
			this.m_lblOnDelay.UnitPositionVertical = false;
			this.m_lblOnDelay.UnitText = "";
			this.m_lblOnDelay.UseBorder = true;
			this.m_lblOnDelay.UseEdgeRadius = false;
			this.m_lblOnDelay.UseImage = false;
			this.m_lblOnDelay.UseSubFont = false;
			this.m_lblOnDelay.UseUnitFont = false;
			// 
			// m_lblOffDelay
			// 
			this.m_lblOffDelay.BackGroundColor = System.Drawing.Color.WhiteSmoke;
			this.m_lblOffDelay.BorderStroke = 2;
			this.m_lblOffDelay.BorderStyle = System.Windows.Forms.ButtonBorderStyle.Solid;
			this.m_lblOffDelay.Description = "";
			this.m_lblOffDelay.DisabledColor = System.Drawing.Color.Silver;
			this.m_lblOffDelay.Dock = System.Windows.Forms.DockStyle.Fill;
			this.m_lblOffDelay.EdgeRadius = 1;
			this.m_lblOffDelay.ImagePosition = new System.Drawing.Point(0, 0);
			this.m_lblOffDelay.ImageSize = new System.Drawing.Point(0, 0);
			this.m_lblOffDelay.LoadImage = null;
			this.m_lblOffDelay.Location = new System.Drawing.Point(0, 0);
			this.m_lblOffDelay.MainFont = new System.Drawing.Font("맑은 고딕", 12F, System.Drawing.FontStyle.Bold);
			this.m_lblOffDelay.MainFontColor = System.Drawing.Color.Black;
			this.m_lblOffDelay.Margin = new System.Windows.Forms.Padding(0);
			this.m_lblOffDelay.Name = "m_lblOffDelay";
			this.m_lblOffDelay.Size = new System.Drawing.Size(222, 56);
			this.m_lblOffDelay.SubFont = new System.Drawing.Font("맑은 고딕", 10F);
			this.m_lblOffDelay.SubFontColor = System.Drawing.Color.Black;
			this.m_lblOffDelay.SubText = "";
			this.m_lblOffDelay.TabIndex = 1347;
			this.m_lblOffDelay.Text = "OFF DELAY";
			this.m_lblOffDelay.TextAlignMain = Sys3Controls.EN_TEXTALIGN.MIDDLE_LEFT;
			this.m_lblOffDelay.TextAlignSub = Sys3Controls.EN_TEXTALIGN.MIDDLE_CENTER;
			this.m_lblOffDelay.TextAlignUnit = Sys3Controls.EN_TEXTALIGN.MIDDLE_CENTER;
			this.m_lblOffDelay.ThemeIndex = 0;
			this.m_lblOffDelay.UnitAreaRate = 40;
			this.m_lblOffDelay.UnitFont = new System.Drawing.Font("맑은 고딕", 10F, System.Drawing.FontStyle.Bold);
			this.m_lblOffDelay.UnitFontColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
			this.m_lblOffDelay.UnitPositionVertical = false;
			this.m_lblOffDelay.UnitText = "";
			this.m_lblOffDelay.UseBorder = true;
			this.m_lblOffDelay.UseEdgeRadius = false;
			this.m_lblOffDelay.UseImage = false;
			this.m_lblOffDelay.UseSubFont = false;
			this.m_lblOffDelay.UseUnitFont = false;
			// 
			// m_lblReverse
			// 
			this.m_lblReverse.BackGroundColor = System.Drawing.Color.WhiteSmoke;
			this.m_lblReverse.BorderStroke = 2;
			this.m_lblReverse.BorderStyle = System.Windows.Forms.ButtonBorderStyle.Solid;
			this.m_lblReverse.Description = "";
			this.m_lblReverse.DisabledColor = System.Drawing.Color.Silver;
			this.m_lblReverse.Dock = System.Windows.Forms.DockStyle.Fill;
			this.m_lblReverse.EdgeRadius = 1;
			this.m_lblReverse.ImagePosition = new System.Drawing.Point(0, 0);
			this.m_lblReverse.ImageSize = new System.Drawing.Point(0, 0);
			this.m_lblReverse.LoadImage = null;
			this.m_lblReverse.Location = new System.Drawing.Point(0, 0);
			this.m_lblReverse.MainFont = new System.Drawing.Font("맑은 고딕", 12F, System.Drawing.FontStyle.Bold);
			this.m_lblReverse.MainFontColor = System.Drawing.Color.Black;
			this.m_lblReverse.Margin = new System.Windows.Forms.Padding(0);
			this.m_lblReverse.Name = "m_lblReverse";
			this.m_lblReverse.Size = new System.Drawing.Size(222, 56);
			this.m_lblReverse.SubFont = new System.Drawing.Font("맑은 고딕", 10F);
			this.m_lblReverse.SubFontColor = System.Drawing.Color.Black;
			this.m_lblReverse.SubText = "";
			this.m_lblReverse.TabIndex = 1347;
			this.m_lblReverse.Text = "REVERSE";
			this.m_lblReverse.TextAlignMain = Sys3Controls.EN_TEXTALIGN.MIDDLE_LEFT;
			this.m_lblReverse.TextAlignSub = Sys3Controls.EN_TEXTALIGN.MIDDLE_CENTER;
			this.m_lblReverse.TextAlignUnit = Sys3Controls.EN_TEXTALIGN.MIDDLE_CENTER;
			this.m_lblReverse.ThemeIndex = 0;
			this.m_lblReverse.UnitAreaRate = 40;
			this.m_lblReverse.UnitFont = new System.Drawing.Font("맑은 고딕", 10F, System.Drawing.FontStyle.Bold);
			this.m_lblReverse.UnitFontColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
			this.m_lblReverse.UnitPositionVertical = false;
			this.m_lblReverse.UnitText = "";
			this.m_lblReverse.UseBorder = true;
			this.m_lblReverse.UseEdgeRadius = false;
			this.m_lblReverse.UseImage = false;
			this.m_lblReverse.UseSubFont = false;
			this.m_lblReverse.UseUnitFont = false;
			// 
			// m_groupViewOptionInput
			// 
			this.m_groupViewOptionInput.BackGroundColor = System.Drawing.Color.WhiteSmoke;
			this._tableLayoutPanel_Option.SetColumnSpan(this.m_groupViewOptionInput, 4);
			this.m_groupViewOptionInput.Dock = System.Windows.Forms.DockStyle.Fill;
			this.m_groupViewOptionInput.EdgeBorderStroke = 2;
			this.m_groupViewOptionInput.EdgeRadius = 2;
			this.m_groupViewOptionInput.LabelFont = new System.Drawing.Font("맑은 고딕", 10F, System.Drawing.FontStyle.Bold);
			this.m_groupViewOptionInput.LabelGradientColorFirst = System.Drawing.Color.FromArgb(((int)(((byte)(208)))), ((int)(((byte)(208)))), ((int)(((byte)(208)))));
			this.m_groupViewOptionInput.LabelGradientColorSecond = System.Drawing.Color.FromArgb(((int)(((byte)(240)))), ((int)(((byte)(240)))), ((int)(((byte)(240)))));
			this.m_groupViewOptionInput.LabelHeight = 30;
			this.m_groupViewOptionInput.LabelTextColor = System.Drawing.Color.Black;
			this.m_groupViewOptionInput.Location = new System.Drawing.Point(0, 0);
			this.m_groupViewOptionInput.Margin = new System.Windows.Forms.Padding(0);
			this.m_groupViewOptionInput.Name = "m_groupViewOptionInput";
			this.m_groupViewOptionInput.Size = new System.Drawing.Size(888, 30);
			this.m_groupViewOptionInput.TabIndex = 1348;
			this.m_groupViewOptionInput.TabStop = false;
			this.m_groupViewOptionInput.Text = "VIEW OPTION";
			this.m_groupViewOptionInput.TextAlign = Sys3Controls.EN_TEXTALIGN.MIDDLE_CENTER;
			this.m_groupViewOptionInput.ThemeIndex = 0;
			this.m_groupViewOptionInput.UseLabelBorder = true;
			// 
			// m_groupList
			// 
			this.m_groupList.BackGroundColor = System.Drawing.Color.WhiteSmoke;
			this.m_groupList.Dock = System.Windows.Forms.DockStyle.Fill;
			this.m_groupList.EdgeBorderStroke = 2;
			this.m_groupList.EdgeRadius = 2;
			this.m_groupList.LabelFont = new System.Drawing.Font("맑은 고딕", 10F, System.Drawing.FontStyle.Bold);
			this.m_groupList.LabelGradientColorFirst = System.Drawing.Color.FromArgb(((int)(((byte)(208)))), ((int)(((byte)(208)))), ((int)(((byte)(208)))));
			this.m_groupList.LabelGradientColorSecond = System.Drawing.Color.FromArgb(((int)(((byte)(240)))), ((int)(((byte)(240)))), ((int)(((byte)(240)))));
			this.m_groupList.LabelHeight = 47;
			this.m_groupList.LabelTextColor = System.Drawing.Color.Black;
			this.m_groupList.Location = new System.Drawing.Point(0, 0);
			this.m_groupList.Name = "m_groupList";
			this.m_groupList.Size = new System.Drawing.Size(1156, 48);
			this.m_groupList.TabIndex = 1350;
			this.m_groupList.TabStop = false;
			this.m_groupList.Text = "ITEM LIST";
			this.m_groupList.TextAlign = Sys3Controls.EN_TEXTALIGN.MIDDLE_CENTER;
			this.m_groupList.ThemeIndex = 0;
			this.m_groupList.UseLabelBorder = true;
			// 
			// m_btnExtend
			// 
			this.m_btnExtend.BorderWidth = 3;
			this.m_btnExtend.ButtonClicked = false;
			this.m_btnExtend.ClickedEmphasizeTextColor = System.Drawing.Color.White;
			this.m_btnExtend.CustomClickedGradientFirstColor = System.Drawing.Color.White;
			this.m_btnExtend.CustomClickedGradientSecondColor = System.Drawing.Color.White;
			this.m_btnExtend.Description = "";
			this.m_btnExtend.DisabledColor = System.Drawing.Color.DarkGray;
			this.m_btnExtend.EdgeRadius = 1;
			this.m_btnExtend.GradientAngle = 60F;
			this.m_btnExtend.GradientFirstColor = System.Drawing.Color.White;
			this.m_btnExtend.GradientSecondColor = System.Drawing.Color.FromArgb(((int)(((byte)(170)))), ((int)(((byte)(176)))), ((int)(((byte)(183)))));
			this.m_btnExtend.HoverEmphasizeCustomColor = System.Drawing.Color.White;
			this.m_btnExtend.ImagePosition = new System.Drawing.Point(0, 0);
			this.m_btnExtend.ImageSize = new System.Drawing.Point(0, 0);
			this.m_btnExtend.LoadImage = null;
			this.m_btnExtend.Location = new System.Drawing.Point(384, 0);
			this.m_btnExtend.MainFont = new System.Drawing.Font("맑은 고딕", 10F, System.Drawing.FontStyle.Bold);
			this.m_btnExtend.MainFontColor = System.Drawing.Color.Black;
			this.m_btnExtend.Margin = new System.Windows.Forms.Padding(0);
			this.m_btnExtend.Name = "m_btnExtend";
			this.m_btnExtend.Size = new System.Drawing.Size(45, 42);
			this.m_btnExtend.SubFont = new System.Drawing.Font("맑은 고딕", 10F);
			this.m_btnExtend.SubFontColor = System.Drawing.Color.Black;
			this.m_btnExtend.SubText = "";
			this.m_btnExtend.TabIndex = 0;
			this.m_btnExtend.Text = "◀";
			this.m_btnExtend.TextAlignMain = Sys3Controls.EN_TEXTALIGN.MIDDLE_CENTER;
			this.m_btnExtend.TextAlignSub = Sys3Controls.EN_TEXTALIGN.MIDDLE_CENTER;
			this.m_btnExtend.ThemeIndex = 0;
			this.m_btnExtend.UseBorder = true;
			this.m_btnExtend.UseClickedEmphasizeTextColor = false;
			this.m_btnExtend.UseCustomizeClickedColor = false;
			this.m_btnExtend.UseEdge = true;
			this.m_btnExtend.UseHoverEmphasizeCustomColor = false;
			this.m_btnExtend.UseImage = false;
			this.m_btnExtend.UserHoverEmpahsize = false;
			this.m_btnExtend.UseSubFont = false;
			this.m_btnExtend.Click += new System.EventHandler(this.Click_ArrowButton);
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
			this.m_btnAdd.EdgeRadius = 1;
			this.m_btnAdd.GradientAngle = 80F;
			this.m_btnAdd.GradientFirstColor = System.Drawing.Color.White;
			this.m_btnAdd.GradientSecondColor = System.Drawing.Color.FromArgb(((int)(((byte)(170)))), ((int)(((byte)(176)))), ((int)(((byte)(183)))));
			this.m_btnAdd.HoverEmphasizeCustomColor = System.Drawing.Color.White;
			this.m_btnAdd.ImagePosition = new System.Drawing.Point(0, 0);
			this.m_btnAdd.ImageSize = new System.Drawing.Point(0, 0);
			this.m_btnAdd.LoadImage = null;
			this.m_btnAdd.Location = new System.Drawing.Point(0, 0);
			this.m_btnAdd.MainFont = new System.Drawing.Font("맑은 고딕", 10F, System.Drawing.FontStyle.Bold);
			this.m_btnAdd.MainFontColor = System.Drawing.Color.Black;
			this.m_btnAdd.Margin = new System.Windows.Forms.Padding(0);
			this.m_btnAdd.Name = "m_btnAdd";
			this.m_btnAdd.Size = new System.Drawing.Size(192, 42);
			this.m_btnAdd.SubFont = new System.Drawing.Font("맑은 고딕", 10F);
			this.m_btnAdd.SubFontColor = System.Drawing.Color.Black;
			this.m_btnAdd.SubText = "";
			this.m_btnAdd.TabIndex = 0;
			this.m_btnAdd.Text = "ADD";
			this.m_btnAdd.TextAlignMain = Sys3Controls.EN_TEXTALIGN.MIDDLE_CENTER;
			this.m_btnAdd.TextAlignSub = Sys3Controls.EN_TEXTALIGN.MIDDLE_CENTER;
			this.m_btnAdd.ThemeIndex = 0;
			this.m_btnAdd.UseBorder = true;
			this.m_btnAdd.UseClickedEmphasizeTextColor = false;
			this.m_btnAdd.UseCustomizeClickedColor = false;
			this.m_btnAdd.UseEdge = true;
			this.m_btnAdd.UseHoverEmphasizeCustomColor = false;
			this.m_btnAdd.UseImage = false;
			this.m_btnAdd.UserHoverEmpahsize = false;
			this.m_btnAdd.UseSubFont = false;
			this.m_btnAdd.Visible = false;
			this.m_btnAdd.Click += new System.EventHandler(this.Click_ViewButtons);
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
			this.m_btnRemove.EdgeRadius = 1;
			this.m_btnRemove.Enabled = false;
			this.m_btnRemove.GradientAngle = 80F;
			this.m_btnRemove.GradientFirstColor = System.Drawing.Color.White;
			this.m_btnRemove.GradientSecondColor = System.Drawing.Color.FromArgb(((int)(((byte)(170)))), ((int)(((byte)(176)))), ((int)(((byte)(183)))));
			this.m_btnRemove.HoverEmphasizeCustomColor = System.Drawing.Color.White;
			this.m_btnRemove.ImagePosition = new System.Drawing.Point(0, 0);
			this.m_btnRemove.ImageSize = new System.Drawing.Point(0, 0);
			this.m_btnRemove.LoadImage = null;
			this.m_btnRemove.Location = new System.Drawing.Point(192, 0);
			this.m_btnRemove.MainFont = new System.Drawing.Font("맑은 고딕", 10F, System.Drawing.FontStyle.Bold);
			this.m_btnRemove.MainFontColor = System.Drawing.Color.Black;
			this.m_btnRemove.Margin = new System.Windows.Forms.Padding(0);
			this.m_btnRemove.Name = "m_btnRemove";
			this.m_btnRemove.Size = new System.Drawing.Size(192, 42);
			this.m_btnRemove.SubFont = new System.Drawing.Font("맑은 고딕", 10F);
			this.m_btnRemove.SubFontColor = System.Drawing.Color.Black;
			this.m_btnRemove.SubText = "";
			this.m_btnRemove.TabIndex = 1;
			this.m_btnRemove.Text = "REMOVE";
			this.m_btnRemove.TextAlignMain = Sys3Controls.EN_TEXTALIGN.MIDDLE_CENTER;
			this.m_btnRemove.TextAlignSub = Sys3Controls.EN_TEXTALIGN.MIDDLE_CENTER;
			this.m_btnRemove.ThemeIndex = 0;
			this.m_btnRemove.UseBorder = true;
			this.m_btnRemove.UseClickedEmphasizeTextColor = false;
			this.m_btnRemove.UseCustomizeClickedColor = false;
			this.m_btnRemove.UseEdge = true;
			this.m_btnRemove.UseHoverEmphasizeCustomColor = false;
			this.m_btnRemove.UseImage = false;
			this.m_btnRemove.UserHoverEmpahsize = false;
			this.m_btnRemove.UseSubFont = false;
			this.m_btnRemove.Visible = false;
			this.m_btnRemove.Click += new System.EventHandler(this.Click_ViewButtons);
			// 
			// _groupBox_ChangeList
			// 
			this._groupBox_ChangeList.BackGroundColor = System.Drawing.Color.WhiteSmoke;
			this._tableLayoutPanel_Option.SetColumnSpan(this._groupBox_ChangeList, 2);
			this._groupBox_ChangeList.Dock = System.Windows.Forms.DockStyle.Fill;
			this._groupBox_ChangeList.EdgeBorderStroke = 2;
			this._groupBox_ChangeList.EdgeRadius = 2;
			this._groupBox_ChangeList.LabelFont = new System.Drawing.Font("맑은 고딕", 10F, System.Drawing.FontStyle.Bold);
			this._groupBox_ChangeList.LabelGradientColorFirst = System.Drawing.Color.FromArgb(((int)(((byte)(208)))), ((int)(((byte)(208)))), ((int)(((byte)(208)))));
			this._groupBox_ChangeList.LabelGradientColorSecond = System.Drawing.Color.FromArgb(((int)(((byte)(240)))), ((int)(((byte)(240)))), ((int)(((byte)(240)))));
			this._groupBox_ChangeList.LabelHeight = 30;
			this._groupBox_ChangeList.LabelTextColor = System.Drawing.Color.Black;
			this._groupBox_ChangeList.Location = new System.Drawing.Point(888, 0);
			this._groupBox_ChangeList.Margin = new System.Windows.Forms.Padding(0);
			this._groupBox_ChangeList.Name = "_groupBox_ChangeList";
			this._groupBox_ChangeList.Size = new System.Drawing.Size(268, 30);
			this._groupBox_ChangeList.TabIndex = 1352;
			this._groupBox_ChangeList.TabStop = false;
			this._groupBox_ChangeList.Text = "CHANGE LIST";
			this._groupBox_ChangeList.TextAlign = Sys3Controls.EN_TEXTALIGN.MIDDLE_CENTER;
			this._groupBox_ChangeList.ThemeIndex = 0;
			this._groupBox_ChangeList.UseLabelBorder = true;
			// 
			// m_lblTag
			// 
			this.m_lblTag.BackGroundColor = System.Drawing.Color.WhiteSmoke;
			this.m_lblTag.BorderStroke = 2;
			this.m_lblTag.BorderStyle = System.Windows.Forms.ButtonBorderStyle.Solid;
			this.m_lblTag.Description = "";
			this.m_lblTag.DisabledColor = System.Drawing.Color.Silver;
			this.m_lblTag.Dock = System.Windows.Forms.DockStyle.Fill;
			this.m_lblTag.EdgeRadius = 1;
			this.m_lblTag.ImagePosition = new System.Drawing.Point(0, 0);
			this.m_lblTag.ImageSize = new System.Drawing.Point(0, 0);
			this.m_lblTag.LoadImage = null;
			this.m_lblTag.Location = new System.Drawing.Point(0, 0);
			this.m_lblTag.MainFont = new System.Drawing.Font("맑은 고딕", 12F, System.Drawing.FontStyle.Bold);
			this.m_lblTag.MainFontColor = System.Drawing.Color.Black;
			this.m_lblTag.Margin = new System.Windows.Forms.Padding(0);
			this.m_lblTag.Name = "m_lblTag";
			this.m_lblTag.Size = new System.Drawing.Size(222, 56);
			this.m_lblTag.SubFont = new System.Drawing.Font("맑은 고딕", 10F);
			this.m_lblTag.SubFontColor = System.Drawing.Color.Black;
			this.m_lblTag.SubText = "";
			this.m_lblTag.TabIndex = 1347;
			this.m_lblTag.Text = "TAG NUMBER";
			this.m_lblTag.TextAlignMain = Sys3Controls.EN_TEXTALIGN.MIDDLE_LEFT;
			this.m_lblTag.TextAlignSub = Sys3Controls.EN_TEXTALIGN.MIDDLE_CENTER;
			this.m_lblTag.TextAlignUnit = Sys3Controls.EN_TEXTALIGN.MIDDLE_CENTER;
			this.m_lblTag.ThemeIndex = 0;
			this.m_lblTag.UnitAreaRate = 40;
			this.m_lblTag.UnitFont = new System.Drawing.Font("맑은 고딕", 10F, System.Drawing.FontStyle.Bold);
			this.m_lblTag.UnitFontColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
			this.m_lblTag.UnitPositionVertical = false;
			this.m_lblTag.UnitText = "";
			this.m_lblTag.UseBorder = true;
			this.m_lblTag.UseEdgeRadius = false;
			this.m_lblTag.UseImage = false;
			this.m_lblTag.UseSubFont = false;
			this.m_lblTag.UseUnitFont = false;
			// 
			// m_ToggleTag
			// 
			this.m_ToggleTag.Active = true;
			this.m_ToggleTag.ActiveColorFirst = System.Drawing.Color.RoyalBlue;
			this.m_ToggleTag.ActiveColorSecond = System.Drawing.Color.DodgerBlue;
			this.m_ToggleTag.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.m_ToggleTag.BackColor = System.Drawing.Color.WhiteSmoke;
			this.m_ToggleTag.Location = new System.Drawing.Point(130, 6);
			this.m_ToggleTag.Name = "m_ToggleTag";
			this.m_ToggleTag.NormalColorFirst = System.Drawing.Color.DarkGray;
			this.m_ToggleTag.NormalColorSecond = System.Drawing.Color.Silver;
			this.m_ToggleTag.Size = new System.Drawing.Size(88, 44);
			this.m_ToggleTag.TabIndex = 3;
			this.m_ToggleTag.Click += new System.EventHandler(this.Click_ViewOption);
			this.m_ToggleTag.DoubleClick += new System.EventHandler(this.Click_ViewOption);
			// 
			// m_btnOutput
			// 
			this.m_btnOutput.BorderWidth = 2;
			this.m_btnOutput.ButtonClicked = false;
			this.m_btnOutput.ClickedEmphasizeTextColor = System.Drawing.Color.White;
			this.m_btnOutput.CustomClickedGradientFirstColor = System.Drawing.Color.White;
			this.m_btnOutput.CustomClickedGradientSecondColor = System.Drawing.Color.White;
			this.m_btnOutput.Description = "";
			this.m_btnOutput.DisabledColor = System.Drawing.Color.DarkGray;
			this.m_btnOutput.Dock = System.Windows.Forms.DockStyle.Fill;
			this.m_btnOutput.EdgeRadius = 5;
			this.m_btnOutput.GradientAngle = 70F;
			this.m_btnOutput.GradientFirstColor = System.Drawing.Color.White;
			this.m_btnOutput.GradientSecondColor = System.Drawing.Color.White;
			this.m_btnOutput.HoverEmphasizeCustomColor = System.Drawing.Color.White;
			this.m_btnOutput.ImagePosition = new System.Drawing.Point(7, 7);
			this.m_btnOutput.ImageSize = new System.Drawing.Point(30, 30);
			this.m_btnOutput.LoadImage = global::FrameOfSystem3.Properties.Resources.Home_black;
			this.m_btnOutput.Location = new System.Drawing.Point(1021, 30);
			this.m_btnOutput.MainFont = new System.Drawing.Font("맑은 고딕", 11F, System.Drawing.FontStyle.Bold);
			this.m_btnOutput.MainFontColor = System.Drawing.Color.DarkBlue;
			this.m_btnOutput.Margin = new System.Windows.Forms.Padding(0);
			this.m_btnOutput.Name = "m_btnOutput";
			this.m_btnOutput.Size = new System.Drawing.Size(135, 56);
			this.m_btnOutput.SubFont = new System.Drawing.Font("맑은 고딕", 8F, System.Drawing.FontStyle.Bold);
			this.m_btnOutput.SubFontColor = System.Drawing.Color.DarkBlue;
			this.m_btnOutput.SubText = "STATUS";
			this.m_btnOutput.TabIndex = 1;
			this.m_btnOutput.Text = "OUTPUT";
			this.m_btnOutput.TextAlignMain = Sys3Controls.EN_TEXTALIGN.MIDDLE_CENTER;
			this.m_btnOutput.TextAlignSub = Sys3Controls.EN_TEXTALIGN.TOP_RIGHT;
			this.m_btnOutput.ThemeIndex = 0;
			this.m_btnOutput.UseBorder = false;
			this.m_btnOutput.UseClickedEmphasizeTextColor = false;
			this.m_btnOutput.UseCustomizeClickedColor = false;
			this.m_btnOutput.UseEdge = false;
			this.m_btnOutput.UseHoverEmphasizeCustomColor = false;
			this.m_btnOutput.UseImage = false;
			this.m_btnOutput.UserHoverEmpahsize = false;
			this.m_btnOutput.UseSubFont = false;
			this.m_btnOutput.Click += new System.EventHandler(this.Click_ChangeList);
			// 
			// m_btnInput
			// 
			this.m_btnInput.BorderWidth = 2;
			this.m_btnInput.ButtonClicked = true;
			this.m_btnInput.ClickedEmphasizeTextColor = System.Drawing.Color.White;
			this.m_btnInput.CustomClickedGradientFirstColor = System.Drawing.Color.White;
			this.m_btnInput.CustomClickedGradientSecondColor = System.Drawing.Color.White;
			this.m_btnInput.Description = "";
			this.m_btnInput.DisabledColor = System.Drawing.Color.DarkGray;
			this.m_btnInput.Dock = System.Windows.Forms.DockStyle.Fill;
			this.m_btnInput.EdgeRadius = 5;
			this.m_btnInput.GradientAngle = 70F;
			this.m_btnInput.GradientFirstColor = System.Drawing.Color.DarkBlue;
			this.m_btnInput.GradientSecondColor = System.Drawing.Color.DarkBlue;
			this.m_btnInput.HoverEmphasizeCustomColor = System.Drawing.Color.White;
			this.m_btnInput.ImagePosition = new System.Drawing.Point(7, 7);
			this.m_btnInput.ImageSize = new System.Drawing.Point(30, 30);
			this.m_btnInput.LoadImage = global::FrameOfSystem3.Properties.Resources.Home_black;
			this.m_btnInput.Location = new System.Drawing.Point(888, 30);
			this.m_btnInput.MainFont = new System.Drawing.Font("맑은 고딕", 11F, System.Drawing.FontStyle.Bold);
			this.m_btnInput.MainFontColor = System.Drawing.Color.White;
			this.m_btnInput.Margin = new System.Windows.Forms.Padding(0);
			this.m_btnInput.Name = "m_btnInput";
			this.m_btnInput.Size = new System.Drawing.Size(133, 56);
			this.m_btnInput.SubFont = new System.Drawing.Font("맑은 고딕", 8F, System.Drawing.FontStyle.Bold);
			this.m_btnInput.SubFontColor = System.Drawing.Color.DarkBlue;
			this.m_btnInput.SubText = "STATUS";
			this.m_btnInput.TabIndex = 0;
			this.m_btnInput.Text = "INPUT";
			this.m_btnInput.TextAlignMain = Sys3Controls.EN_TEXTALIGN.MIDDLE_CENTER;
			this.m_btnInput.TextAlignSub = Sys3Controls.EN_TEXTALIGN.TOP_RIGHT;
			this.m_btnInput.ThemeIndex = 0;
			this.m_btnInput.UseBorder = false;
			this.m_btnInput.UseClickedEmphasizeTextColor = false;
			this.m_btnInput.UseCustomizeClickedColor = false;
			this.m_btnInput.UseEdge = false;
			this.m_btnInput.UseHoverEmphasizeCustomColor = false;
			this.m_btnInput.UseImage = false;
			this.m_btnInput.UserHoverEmpahsize = false;
			this.m_btnInput.UseSubFont = false;
			this.m_btnInput.Click += new System.EventHandler(this.Click_ChangeList);
			// 
			// _label_OptionReserved
			// 
			this._label_OptionReserved.BackGroundColor = System.Drawing.Color.WhiteSmoke;
			this._label_OptionReserved.BorderStroke = 2;
			this._label_OptionReserved.BorderStyle = System.Windows.Forms.ButtonBorderStyle.Solid;
			this._label_OptionReserved.Description = "";
			this._label_OptionReserved.DisabledColor = System.Drawing.Color.Silver;
			this._label_OptionReserved.Dock = System.Windows.Forms.DockStyle.Fill;
			this._label_OptionReserved.EdgeRadius = 1;
			this._label_OptionReserved.ImagePosition = new System.Drawing.Point(0, 0);
			this._label_OptionReserved.ImageSize = new System.Drawing.Point(0, 0);
			this._label_OptionReserved.LoadImage = null;
			this._label_OptionReserved.Location = new System.Drawing.Point(0, 0);
			this._label_OptionReserved.MainFont = new System.Drawing.Font("맑은 고딕", 12F, System.Drawing.FontStyle.Bold);
			this._label_OptionReserved.MainFontColor = System.Drawing.Color.Black;
			this._label_OptionReserved.Margin = new System.Windows.Forms.Padding(0);
			this._label_OptionReserved.Name = "_label_OptionReserved";
			this._label_OptionReserved.Size = new System.Drawing.Size(222, 56);
			this._label_OptionReserved.SubFont = new System.Drawing.Font("맑은 고딕", 10F);
			this._label_OptionReserved.SubFontColor = System.Drawing.Color.Black;
			this._label_OptionReserved.SubText = "";
			this._label_OptionReserved.TabIndex = 1347;
			this._label_OptionReserved.TextAlignMain = Sys3Controls.EN_TEXTALIGN.MIDDLE_LEFT;
			this._label_OptionReserved.TextAlignSub = Sys3Controls.EN_TEXTALIGN.MIDDLE_CENTER;
			this._label_OptionReserved.TextAlignUnit = Sys3Controls.EN_TEXTALIGN.MIDDLE_CENTER;
			this._label_OptionReserved.ThemeIndex = 0;
			this._label_OptionReserved.UnitAreaRate = 40;
			this._label_OptionReserved.UnitFont = new System.Drawing.Font("맑은 고딕", 10F, System.Drawing.FontStyle.Bold);
			this._label_OptionReserved.UnitFontColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
			this._label_OptionReserved.UnitPositionVertical = false;
			this._label_OptionReserved.UnitText = "";
			this._label_OptionReserved.UseBorder = true;
			this._label_OptionReserved.UseEdgeRadius = false;
			this._label_OptionReserved.UseImage = false;
			this._label_OptionReserved.UseSubFont = false;
			this._label_OptionReserved.UseUnitFont = false;
			// 
			// m_btnIO
			// 
			this.m_btnIO.BorderWidth = 2;
			this.m_btnIO.ButtonClicked = false;
			this.m_btnIO.ClickedEmphasizeTextColor = System.Drawing.Color.White;
			this._tableLayoutPanel_Option.SetColumnSpan(this.m_btnIO, 2);
			this.m_btnIO.CustomClickedGradientFirstColor = System.Drawing.Color.White;
			this.m_btnIO.CustomClickedGradientSecondColor = System.Drawing.Color.White;
			this.m_btnIO.Description = "";
			this.m_btnIO.DisabledColor = System.Drawing.Color.DarkGray;
			this.m_btnIO.Dock = System.Windows.Forms.DockStyle.Fill;
			this.m_btnIO.EdgeRadius = 5;
			this.m_btnIO.GradientAngle = 70F;
			this.m_btnIO.GradientFirstColor = System.Drawing.Color.White;
			this.m_btnIO.GradientSecondColor = System.Drawing.Color.White;
			this.m_btnIO.HoverEmphasizeCustomColor = System.Drawing.Color.White;
			this.m_btnIO.ImagePosition = new System.Drawing.Point(7, 7);
			this.m_btnIO.ImageSize = new System.Drawing.Point(30, 30);
			this.m_btnIO.LoadImage = global::FrameOfSystem3.Properties.Resources.Home_black;
			this.m_btnIO.Location = new System.Drawing.Point(888, 86);
			this.m_btnIO.MainFont = new System.Drawing.Font("맑은 고딕", 11F, System.Drawing.FontStyle.Bold);
			this.m_btnIO.MainFontColor = System.Drawing.Color.DarkBlue;
			this.m_btnIO.Margin = new System.Windows.Forms.Padding(0);
			this.m_btnIO.Name = "m_btnIO";
			this.m_btnIO.Size = new System.Drawing.Size(268, 56);
			this.m_btnIO.SubFont = new System.Drawing.Font("맑은 고딕", 8F, System.Drawing.FontStyle.Bold);
			this.m_btnIO.SubFontColor = System.Drawing.Color.DarkBlue;
			this.m_btnIO.SubText = "STATUS";
			this.m_btnIO.TabIndex = 2;
			this.m_btnIO.Text = "INPUT AND OUTPUT";
			this.m_btnIO.TextAlignMain = Sys3Controls.EN_TEXTALIGN.MIDDLE_CENTER;
			this.m_btnIO.TextAlignSub = Sys3Controls.EN_TEXTALIGN.TOP_RIGHT;
			this.m_btnIO.ThemeIndex = 0;
			this.m_btnIO.UseBorder = false;
			this.m_btnIO.UseClickedEmphasizeTextColor = false;
			this.m_btnIO.UseCustomizeClickedColor = false;
			this.m_btnIO.UseEdge = false;
			this.m_btnIO.UseHoverEmphasizeCustomColor = false;
			this.m_btnIO.UseImage = false;
			this.m_btnIO.UserHoverEmpahsize = false;
			this.m_btnIO.UseSubFont = false;
			this.m_btnIO.Click += new System.EventHandler(this.Click_ChangeList);
			// 
			// m_ViewInput
			// 
			this.m_ViewInput.BorderWidth = 2;
			this.m_ViewInput.ButtonClicked = true;
			this.m_ViewInput.ClickedEmphasizeTextColor = System.Drawing.Color.White;
			this.m_ViewInput.CustomClickedGradientFirstColor = System.Drawing.Color.White;
			this.m_ViewInput.CustomClickedGradientSecondColor = System.Drawing.Color.White;
			this.m_ViewInput.Description = "";
			this.m_ViewInput.DisabledColor = System.Drawing.Color.DarkGray;
			this.m_ViewInput.Dock = System.Windows.Forms.DockStyle.Fill;
			this.m_ViewInput.EdgeRadius = 5;
			this.m_ViewInput.GradientAngle = 70F;
			this.m_ViewInput.GradientFirstColor = System.Drawing.Color.White;
			this.m_ViewInput.GradientSecondColor = System.Drawing.Color.White;
			this.m_ViewInput.HoverEmphasizeCustomColor = System.Drawing.Color.White;
			this.m_ViewInput.ImagePosition = new System.Drawing.Point(7, 7);
			this.m_ViewInput.ImageSize = new System.Drawing.Point(30, 30);
			this.m_ViewInput.LoadImage = global::FrameOfSystem3.Properties.Resources.Home_black;
			this.m_ViewInput.Location = new System.Drawing.Point(0, 0);
			this.m_ViewInput.MainFont = new System.Drawing.Font("맑은 고딕", 11F, System.Drawing.FontStyle.Bold);
			this.m_ViewInput.MainFontColor = System.Drawing.Color.DarkBlue;
			this.m_ViewInput.Margin = new System.Windows.Forms.Padding(0);
			this.m_ViewInput.Name = "m_ViewInput";
			this.m_ViewInput.Size = new System.Drawing.Size(72, 355);
			this.m_ViewInput.SubFont = new System.Drawing.Font("맑은 고딕", 8F, System.Drawing.FontStyle.Bold);
			this.m_ViewInput.SubFontColor = System.Drawing.Color.DarkBlue;
			this.m_ViewInput.SubText = "STATUS";
			this.m_ViewInput.TabIndex = 1353;
			this.m_ViewInput.Text = "INPUT";
			this.m_ViewInput.TextAlignMain = Sys3Controls.EN_TEXTALIGN.MIDDLE_CENTER;
			this.m_ViewInput.TextAlignSub = Sys3Controls.EN_TEXTALIGN.TOP_RIGHT;
			this.m_ViewInput.ThemeIndex = 0;
			this.m_ViewInput.UseBorder = false;
			this.m_ViewInput.UseClickedEmphasizeTextColor = false;
			this.m_ViewInput.UseCustomizeClickedColor = false;
			this.m_ViewInput.UseEdge = false;
			this.m_ViewInput.UseHoverEmphasizeCustomColor = false;
			this.m_ViewInput.UseImage = false;
			this.m_ViewInput.UserHoverEmpahsize = false;
			this.m_ViewInput.UseSubFont = false;
			// 
			// m_ViewOutPut
			// 
			this.m_ViewOutPut.BorderWidth = 2;
			this.m_ViewOutPut.ButtonClicked = true;
			this.m_ViewOutPut.ClickedEmphasizeTextColor = System.Drawing.Color.White;
			this.m_ViewOutPut.CustomClickedGradientFirstColor = System.Drawing.Color.White;
			this.m_ViewOutPut.CustomClickedGradientSecondColor = System.Drawing.Color.White;
			this.m_ViewOutPut.Description = "";
			this.m_ViewOutPut.DisabledColor = System.Drawing.Color.DarkGray;
			this.m_ViewOutPut.Dock = System.Windows.Forms.DockStyle.Fill;
			this.m_ViewOutPut.EdgeRadius = 5;
			this.m_ViewOutPut.GradientAngle = 70F;
			this.m_ViewOutPut.GradientFirstColor = System.Drawing.Color.White;
			this.m_ViewOutPut.GradientSecondColor = System.Drawing.Color.White;
			this.m_ViewOutPut.HoverEmphasizeCustomColor = System.Drawing.Color.White;
			this.m_ViewOutPut.ImagePosition = new System.Drawing.Point(7, 7);
			this.m_ViewOutPut.ImageSize = new System.Drawing.Point(30, 30);
			this.m_ViewOutPut.LoadImage = global::FrameOfSystem3.Properties.Resources.Home_black;
			this.m_ViewOutPut.Location = new System.Drawing.Point(0, 355);
			this.m_ViewOutPut.MainFont = new System.Drawing.Font("맑은 고딕", 11F, System.Drawing.FontStyle.Bold);
			this.m_ViewOutPut.MainFontColor = System.Drawing.Color.DarkBlue;
			this.m_ViewOutPut.Margin = new System.Windows.Forms.Padding(0);
			this.m_ViewOutPut.Name = "m_ViewOutPut";
			this.m_ViewOutPut.Size = new System.Drawing.Size(72, 355);
			this.m_ViewOutPut.SubFont = new System.Drawing.Font("맑은 고딕", 8F, System.Drawing.FontStyle.Bold);
			this.m_ViewOutPut.SubFontColor = System.Drawing.Color.DarkBlue;
			this.m_ViewOutPut.SubText = "STATUS";
			this.m_ViewOutPut.TabIndex = 1354;
			this.m_ViewOutPut.Text = "OUTPUT";
			this.m_ViewOutPut.TextAlignMain = Sys3Controls.EN_TEXTALIGN.MIDDLE_CENTER;
			this.m_ViewOutPut.TextAlignSub = Sys3Controls.EN_TEXTALIGN.TOP_RIGHT;
			this.m_ViewOutPut.ThemeIndex = 0;
			this.m_ViewOutPut.UseBorder = false;
			this.m_ViewOutPut.UseClickedEmphasizeTextColor = false;
			this.m_ViewOutPut.UseCustomizeClickedColor = false;
			this.m_ViewOutPut.UseEdge = false;
			this.m_ViewOutPut.UseHoverEmphasizeCustomColor = false;
			this.m_ViewOutPut.UseImage = false;
			this.m_ViewOutPut.UserHoverEmpahsize = false;
			this.m_ViewOutPut.UseSubFont = false;
			// 
			// _tableLayoutPanel_Main
			// 
			this._tableLayoutPanel_Main.ColumnCount = 1;
			this._tableLayoutPanel_Main.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
			this._tableLayoutPanel_Main.Controls.Add(this._tableLayoutPanel_GridView, 0, 2);
			this._tableLayoutPanel_Main.Controls.Add(this._panel_ItemList, 0, 1);
			this._tableLayoutPanel_Main.Controls.Add(this._tableLayoutPanel_Option, 0, 0);
			this._tableLayoutPanel_Main.Dock = System.Windows.Forms.DockStyle.Fill;
			this._tableLayoutPanel_Main.Location = new System.Drawing.Point(0, 0);
			this._tableLayoutPanel_Main.Margin = new System.Windows.Forms.Padding(0);
			this._tableLayoutPanel_Main.Name = "_tableLayoutPanel_Main";
			this._tableLayoutPanel_Main.RowCount = 3;
			this._tableLayoutPanel_Main.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 142F));
			this._tableLayoutPanel_Main.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 48F));
			this._tableLayoutPanel_Main.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
			this._tableLayoutPanel_Main.Size = new System.Drawing.Size(1156, 900);
			this._tableLayoutPanel_Main.TabIndex = 1355;
			// 
			// _tableLayoutPanel_GridView
			// 
			this._tableLayoutPanel_GridView.ColumnCount = 2;
			this._tableLayoutPanel_GridView.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 72F));
			this._tableLayoutPanel_GridView.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
			this._tableLayoutPanel_GridView.Controls.Add(this.m_ViewInput, 0, 0);
			this._tableLayoutPanel_GridView.Controls.Add(this.m_dgViewOutput, 1, 1);
			this._tableLayoutPanel_GridView.Controls.Add(this.m_dgViewInput, 1, 0);
			this._tableLayoutPanel_GridView.Controls.Add(this.m_ViewOutPut, 0, 1);
			this._tableLayoutPanel_GridView.Dock = System.Windows.Forms.DockStyle.Fill;
			this._tableLayoutPanel_GridView.Location = new System.Drawing.Point(0, 190);
			this._tableLayoutPanel_GridView.Margin = new System.Windows.Forms.Padding(0);
			this._tableLayoutPanel_GridView.Name = "_tableLayoutPanel_GridView";
			this._tableLayoutPanel_GridView.RowCount = 2;
			this._tableLayoutPanel_GridView.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
			this._tableLayoutPanel_GridView.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
			this._tableLayoutPanel_GridView.Size = new System.Drawing.Size(1156, 710);
			this._tableLayoutPanel_GridView.TabIndex = 1356;
			// 
			// _panel_ItemList
			// 
			this._panel_ItemList.Controls.Add(this.lbl_Filtering);
			this._panel_ItemList.Controls.Add(this._flowLayoutPanel_AddRemove);
			this._panel_ItemList.Controls.Add(this.m_groupList);
			this._panel_ItemList.Dock = System.Windows.Forms.DockStyle.Fill;
			this._panel_ItemList.Location = new System.Drawing.Point(0, 142);
			this._panel_ItemList.Margin = new System.Windows.Forms.Padding(0);
			this._panel_ItemList.Name = "_panel_ItemList";
			this._panel_ItemList.Size = new System.Drawing.Size(1156, 48);
			this._panel_ItemList.TabIndex = 1357;
			// 
			// _flowLayoutPanel_AddRemove
			// 
			this._flowLayoutPanel_AddRemove.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this._flowLayoutPanel_AddRemove.AutoSize = true;
			this._flowLayoutPanel_AddRemove.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
			this._flowLayoutPanel_AddRemove.Controls.Add(this.m_btnExtend);
			this._flowLayoutPanel_AddRemove.Controls.Add(this.m_btnRemove);
			this._flowLayoutPanel_AddRemove.Controls.Add(this.m_btnAdd);
			this._flowLayoutPanel_AddRemove.FlowDirection = System.Windows.Forms.FlowDirection.RightToLeft;
			this._flowLayoutPanel_AddRemove.Location = new System.Drawing.Point(723, 2);
			this._flowLayoutPanel_AddRemove.Name = "_flowLayoutPanel_AddRemove";
			this._flowLayoutPanel_AddRemove.Size = new System.Drawing.Size(429, 42);
			this._flowLayoutPanel_AddRemove.TabIndex = 1356;
			// 
			// lbl_Filtering
			// 
			this.lbl_Filtering.BackGroundColor = System.Drawing.Color.WhiteSmoke;
			this.lbl_Filtering.BorderStroke = 2;
			this.lbl_Filtering.BorderStyle = System.Windows.Forms.ButtonBorderStyle.Solid;
			this.lbl_Filtering.Description = "";
			this.lbl_Filtering.DisabledColor = System.Drawing.Color.Silver;
			this.lbl_Filtering.EdgeRadius = 1;
			this.lbl_Filtering.ImagePosition = new System.Drawing.Point(0, 0);
			this.lbl_Filtering.ImageSize = new System.Drawing.Point(0, 0);
			this.lbl_Filtering.LoadImage = null;
			this.lbl_Filtering.Location = new System.Drawing.Point(0, 2);
			this.lbl_Filtering.MainFont = new System.Drawing.Font("맑은 고딕", 12F, System.Drawing.FontStyle.Bold);
			this.lbl_Filtering.MainFontColor = System.Drawing.Color.Red;
			this.lbl_Filtering.Margin = new System.Windows.Forms.Padding(0);
			this.lbl_Filtering.Name = "lbl_Filtering";
			this.lbl_Filtering.Size = new System.Drawing.Size(529, 42);
			this.lbl_Filtering.SubFont = new System.Drawing.Font("맑은 고딕", 8F, System.Drawing.FontStyle.Bold);
			this.lbl_Filtering.SubFontColor = System.Drawing.Color.DodgerBlue;
			this.lbl_Filtering.SubText = "FILTERING";
			this.lbl_Filtering.TabIndex = 1348;
			this.lbl_Filtering.TextAlignMain = Sys3Controls.EN_TEXTALIGN.MIDDLE_CENTER;
			this.lbl_Filtering.TextAlignSub = Sys3Controls.EN_TEXTALIGN.TOP_LEFT;
			this.lbl_Filtering.TextAlignUnit = Sys3Controls.EN_TEXTALIGN.MIDDLE_CENTER;
			this.lbl_Filtering.ThemeIndex = 0;
			this.lbl_Filtering.UnitAreaRate = 40;
			this.lbl_Filtering.UnitFont = new System.Drawing.Font("맑은 고딕", 10F, System.Drawing.FontStyle.Bold);
			this.lbl_Filtering.UnitFontColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
			this.lbl_Filtering.UnitPositionVertical = false;
			this.lbl_Filtering.UnitText = "";
			this.lbl_Filtering.UseBorder = true;
			this.lbl_Filtering.UseEdgeRadius = false;
			this.lbl_Filtering.UseImage = false;
			this.lbl_Filtering.UseSubFont = true;
			this.lbl_Filtering.UseUnitFont = false;
			this.lbl_Filtering.Click += new System.EventHandler(this.lbl_Filtering_Click);
			// 
			// _tableLayoutPanel_Option
			// 
			this._tableLayoutPanel_Option.ColumnCount = 6;
			this._tableLayoutPanel_Option.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 19.24261F));
			this._tableLayoutPanel_Option.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 19.24261F));
			this._tableLayoutPanel_Option.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 19.24261F));
			this._tableLayoutPanel_Option.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 19.24261F));
			this._tableLayoutPanel_Option.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 11.51478F));
			this._tableLayoutPanel_Option.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 11.51478F));
			this._tableLayoutPanel_Option.Controls.Add(this._panel_Reserve, 3, 2);
			this._tableLayoutPanel_Option.Controls.Add(this.m_groupViewOptionInput, 0, 0);
			this._tableLayoutPanel_Option.Controls.Add(this._panel_Reverse, 2, 2);
			this._tableLayoutPanel_Option.Controls.Add(this._groupBox_ChangeList, 4, 0);
			this._tableLayoutPanel_Option.Controls.Add(this._panel_OffDelay, 1, 2);
			this._tableLayoutPanel_Option.Controls.Add(this.m_btnInput, 4, 1);
			this._tableLayoutPanel_Option.Controls.Add(this._panel_OnDelay, 0, 2);
			this._tableLayoutPanel_Option.Controls.Add(this.m_btnIO, 4, 2);
			this._tableLayoutPanel_Option.Controls.Add(this._panel_TagNumber, 3, 1);
			this._tableLayoutPanel_Option.Controls.Add(this.m_btnOutput, 5, 1);
			this._tableLayoutPanel_Option.Controls.Add(this._panel_Target, 2, 1);
			this._tableLayoutPanel_Option.Controls.Add(this._panel_Index, 0, 1);
			this._tableLayoutPanel_Option.Controls.Add(this._panel_Name, 1, 1);
			this._tableLayoutPanel_Option.Dock = System.Windows.Forms.DockStyle.Fill;
			this._tableLayoutPanel_Option.Location = new System.Drawing.Point(0, 0);
			this._tableLayoutPanel_Option.Margin = new System.Windows.Forms.Padding(0);
			this._tableLayoutPanel_Option.Name = "_tableLayoutPanel_Option";
			this._tableLayoutPanel_Option.RowCount = 3;
			this._tableLayoutPanel_Option.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 30F));
			this._tableLayoutPanel_Option.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
			this._tableLayoutPanel_Option.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
			this._tableLayoutPanel_Option.Size = new System.Drawing.Size(1156, 142);
			this._tableLayoutPanel_Option.TabIndex = 0;
			// 
			// _panel_Reserve
			// 
			this._panel_Reserve.Controls.Add(this._label_OptionReserved);
			this._panel_Reserve.Dock = System.Windows.Forms.DockStyle.Fill;
			this._panel_Reserve.Location = new System.Drawing.Point(666, 86);
			this._panel_Reserve.Margin = new System.Windows.Forms.Padding(0);
			this._panel_Reserve.Name = "_panel_Reserve";
			this._panel_Reserve.Size = new System.Drawing.Size(222, 56);
			this._panel_Reserve.TabIndex = 1363;
			// 
			// _panel_Reverse
			// 
			this._panel_Reverse.Controls.Add(this.m_ToggleReverse);
			this._panel_Reverse.Controls.Add(this.m_lblReverse);
			this._panel_Reverse.Dock = System.Windows.Forms.DockStyle.Fill;
			this._panel_Reverse.Location = new System.Drawing.Point(444, 86);
			this._panel_Reverse.Margin = new System.Windows.Forms.Padding(0);
			this._panel_Reverse.Name = "_panel_Reverse";
			this._panel_Reverse.Size = new System.Drawing.Size(222, 56);
			this._panel_Reverse.TabIndex = 1362;
			// 
			// _panel_OffDelay
			// 
			this._panel_OffDelay.Controls.Add(this.m_ToggleOffDelay);
			this._panel_OffDelay.Controls.Add(this.m_lblOffDelay);
			this._panel_OffDelay.Dock = System.Windows.Forms.DockStyle.Fill;
			this._panel_OffDelay.Location = new System.Drawing.Point(222, 86);
			this._panel_OffDelay.Margin = new System.Windows.Forms.Padding(0);
			this._panel_OffDelay.Name = "_panel_OffDelay";
			this._panel_OffDelay.Size = new System.Drawing.Size(222, 56);
			this._panel_OffDelay.TabIndex = 1361;
			// 
			// _panel_OnDelay
			// 
			this._panel_OnDelay.Controls.Add(this.m_ToggleOnDelay);
			this._panel_OnDelay.Controls.Add(this.m_lblOnDelay);
			this._panel_OnDelay.Dock = System.Windows.Forms.DockStyle.Fill;
			this._panel_OnDelay.Location = new System.Drawing.Point(0, 86);
			this._panel_OnDelay.Margin = new System.Windows.Forms.Padding(0);
			this._panel_OnDelay.Name = "_panel_OnDelay";
			this._panel_OnDelay.Size = new System.Drawing.Size(222, 56);
			this._panel_OnDelay.TabIndex = 1360;
			// 
			// _panel_TagNumber
			// 
			this._panel_TagNumber.Controls.Add(this.m_ToggleTag);
			this._panel_TagNumber.Controls.Add(this.m_lblTag);
			this._panel_TagNumber.Dock = System.Windows.Forms.DockStyle.Fill;
			this._panel_TagNumber.Location = new System.Drawing.Point(666, 30);
			this._panel_TagNumber.Margin = new System.Windows.Forms.Padding(0);
			this._panel_TagNumber.Name = "_panel_TagNumber";
			this._panel_TagNumber.Size = new System.Drawing.Size(222, 56);
			this._panel_TagNumber.TabIndex = 1359;
			// 
			// _panel_Target
			// 
			this._panel_Target.Controls.Add(this.m_ToggleTarget);
			this._panel_Target.Controls.Add(this.m_lblTarget);
			this._panel_Target.Dock = System.Windows.Forms.DockStyle.Fill;
			this._panel_Target.Location = new System.Drawing.Point(444, 30);
			this._panel_Target.Margin = new System.Windows.Forms.Padding(0);
			this._panel_Target.Name = "_panel_Target";
			this._panel_Target.Size = new System.Drawing.Size(222, 56);
			this._panel_Target.TabIndex = 1358;
			// 
			// _panel_Index
			// 
			this._panel_Index.Controls.Add(this.m_ToggleIndex);
			this._panel_Index.Controls.Add(this.m_lblIndex);
			this._panel_Index.Dock = System.Windows.Forms.DockStyle.Fill;
			this._panel_Index.Location = new System.Drawing.Point(0, 30);
			this._panel_Index.Margin = new System.Windows.Forms.Padding(0);
			this._panel_Index.Name = "_panel_Index";
			this._panel_Index.Size = new System.Drawing.Size(222, 56);
			this._panel_Index.TabIndex = 1356;
			// 
			// _panel_Name
			// 
			this._panel_Name.Controls.Add(this.m_ToggleName);
			this._panel_Name.Controls.Add(this.m_lblName);
			this._panel_Name.Dock = System.Windows.Forms.DockStyle.Fill;
			this._panel_Name.Location = new System.Drawing.Point(222, 30);
			this._panel_Name.Margin = new System.Windows.Forms.Padding(0);
			this._panel_Name.Name = "_panel_Name";
			this._panel_Name.Size = new System.Drawing.Size(222, 56);
			this._panel_Name.TabIndex = 1357;
			// 
			// Config_Digital
			// 
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
			this.Controls.Add(this._tableLayoutPanel_Main);
			this.Margin = new System.Windows.Forms.Padding(0);
			this.Name = "Config_Digital";
			this.Size = new System.Drawing.Size(1156, 900);
			((System.ComponentModel.ISupportInitialize)(this.m_dgViewOutput)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.m_dgViewInput)).EndInit();
			this._tableLayoutPanel_Main.ResumeLayout(false);
			this._tableLayoutPanel_GridView.ResumeLayout(false);
			this._panel_ItemList.ResumeLayout(false);
			this._panel_ItemList.PerformLayout();
			this._flowLayoutPanel_AddRemove.ResumeLayout(false);
			this._tableLayoutPanel_Option.ResumeLayout(false);
			this._panel_Reserve.ResumeLayout(false);
			this._panel_Reverse.ResumeLayout(false);
			this._panel_OffDelay.ResumeLayout(false);
			this._panel_OnDelay.ResumeLayout(false);
			this._panel_TagNumber.ResumeLayout(false);
			this._panel_Target.ResumeLayout(false);
			this._panel_Index.ResumeLayout(false);
			this._panel_Name.ResumeLayout(false);
			this.ResumeLayout(false);

        }

        #endregion

		private Sys3Controls.Sys3DoubleBufferedDataGridView m_dgViewOutput;
		private Sys3Controls.Sys3DoubleBufferedDataGridView m_dgViewInput;
		private Sys3Controls.Sys3ToggleButton m_ToggleIndex;
		private Sys3Controls.Sys3ToggleButton m_ToggleName;
		private Sys3Controls.Sys3ToggleButton m_ToggleTarget;
		private Sys3Controls.Sys3ToggleButton m_ToggleOnDelay;
		private Sys3Controls.Sys3ToggleButton m_ToggleOffDelay;
		private Sys3Controls.Sys3ToggleButton m_ToggleReverse;
		private Sys3Controls.Sys3Label m_lblIndex;
		private Sys3Controls.Sys3Label m_lblName;
		private Sys3Controls.Sys3Label m_lblTarget;
		private Sys3Controls.Sys3Label m_lblOnDelay;
		private Sys3Controls.Sys3Label m_lblOffDelay;
		private Sys3Controls.Sys3Label m_lblReverse;
        private Sys3Controls.Sys3GroupBox m_groupViewOptionInput;
        private Sys3Controls.Sys3GroupBox m_groupList;
		private Sys3Controls.Sys3button m_btnExtend;
		private Sys3Controls.Sys3button m_btnAdd;
		private Sys3Controls.Sys3button m_btnRemove;
        private Sys3Controls.Sys3GroupBox _groupBox_ChangeList;
		private Sys3Controls.Sys3Label m_lblTag;
		private Sys3Controls.Sys3ToggleButton m_ToggleTag;
		private Sys3Controls.Sys3button m_btnOutput;
		private Sys3Controls.Sys3button m_btnInput;
		private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn1;
		private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn2;
		private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn3;
		private System.Windows.Forms.DataGridViewTextBoxColumn TAG1;
		private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn4;
		private System.Windows.Forms.DataGridViewTextBoxColumn ON_DELAY;
		private System.Windows.Forms.DataGridViewTextBoxColumn OFF_DELAY;
		private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn5;
		private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn6;
		private System.Windows.Forms.DataGridViewTextBoxColumn ID;
		private System.Windows.Forms.DataGridViewTextBoxColumn ENABLE;
		private System.Windows.Forms.DataGridViewTextBoxColumn PASSWORD;
		private System.Windows.Forms.DataGridViewTextBoxColumn TAG;
		private System.Windows.Forms.DataGridViewTextBoxColumn MONITORING;
		private System.Windows.Forms.DataGridViewTextBoxColumn DELAY;
		private System.Windows.Forms.DataGridViewTextBoxColumn DELAYOUT;
		private System.Windows.Forms.DataGridViewTextBoxColumn CONDITION;
		private System.Windows.Forms.DataGridViewTextBoxColumn Led;
		private Sys3Controls.Sys3Label _label_OptionReserved;
        private Sys3Controls.Sys3button m_btnIO;
        private Sys3Controls.Sys3button m_ViewInput;
        private Sys3Controls.Sys3button m_ViewOutPut;
        private System.Windows.Forms.TableLayoutPanel _tableLayoutPanel_Main;
        private System.Windows.Forms.TableLayoutPanel _tableLayoutPanel_Option;
        private System.Windows.Forms.Panel _panel_Index;
        private System.Windows.Forms.Panel _panel_Name;
        private System.Windows.Forms.Panel _panel_Target;
        private System.Windows.Forms.Panel _panel_TagNumber;
        private System.Windows.Forms.Panel _panel_OnDelay;
        private System.Windows.Forms.Panel _panel_OffDelay;
        private System.Windows.Forms.Panel _panel_Reverse;
        private System.Windows.Forms.Panel _panel_Reserve;
        private System.Windows.Forms.FlowLayoutPanel _flowLayoutPanel_AddRemove;
        private System.Windows.Forms.Panel _panel_ItemList;
        private System.Windows.Forms.TableLayoutPanel _tableLayoutPanel_GridView;
		private Sys3Controls.Sys3Label lbl_Filtering;
    }
}
