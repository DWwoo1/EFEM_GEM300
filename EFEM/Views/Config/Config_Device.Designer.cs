namespace FrameOfSystem3.Views.Config
{
	partial class Config_Device
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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle7 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle8 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle9 = new System.Windows.Forms.DataGridViewCellStyle();
            this.m_dgViewTask = new Sys3Controls.Sys3DoubleBufferedDataGridView();
            this.dataGridViewTextBoxColumn1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn2 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.m_groupTaskList = new Sys3Controls.Sys3GroupBoxContainer();
            this.m_groupDeviceList = new Sys3Controls.Sys3GroupBoxContainer();
            this.m_dgViewDevice = new Sys3Controls.Sys3DoubleBufferedDataGridView();
            this.dataGridViewTextBoxColumn3 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn4 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.m_dgViewItem = new Sys3Controls.Sys3DoubleBufferedDataGridView();
            this.dataGridViewTextBoxColumn5 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn6 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.POST_SKIP = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.m_groupItemList = new Sys3Controls.Sys3GroupBoxContainer();
            this.m_lblName = new Sys3Controls.Sys3Label();
            this.m_labelName = new Sys3Controls.Sys3Label();
            this.m_groupConfiguration = new Sys3Controls.Sys3GroupBoxContainer();
            this.tableLayoutPanel2 = new System.Windows.Forms.TableLayoutPanel();
            this.tableLayoutPanel3 = new System.Windows.Forms.TableLayoutPanel();
            this.m_labelTargetIndex = new Sys3Controls.Sys3Label();
            this.m_lblTargetIndex = new Sys3Controls.Sys3Label();
            this.tableLayoutPanel4 = new System.Windows.Forms.TableLayoutPanel();
            this.m_btnAdd = new Sys3Controls.Sys3button();
            this.m_btnRemove = new Sys3Controls.Sys3button();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            ((System.ComponentModel.ISupportInitialize)(this.m_dgViewTask)).BeginInit();
            this.m_groupTaskList.SuspendLayout();
            this.m_groupDeviceList.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.m_dgViewDevice)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.m_dgViewItem)).BeginInit();
            this.m_groupItemList.SuspendLayout();
            this.m_groupConfiguration.SuspendLayout();
            this.tableLayoutPanel2.SuspendLayout();
            this.tableLayoutPanel3.SuspendLayout();
            this.tableLayoutPanel4.SuspendLayout();
            this.tableLayoutPanel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // m_dgViewTask
            // 
            this.m_dgViewTask.AllowUserToAddRows = false;
            this.m_dgViewTask.AllowUserToDeleteRows = false;
            this.m_dgViewTask.AllowUserToResizeColumns = false;
            this.m_dgViewTask.AllowUserToResizeRows = false;
            this.m_dgViewTask.BackgroundColor = System.Drawing.Color.White;
            this.m_dgViewTask.CellBorderStyle = System.Windows.Forms.DataGridViewCellBorderStyle.Raised;
            this.m_dgViewTask.ColumnHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.Single;
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle1.BackColor = System.Drawing.Color.LightGray;
            dataGridViewCellStyle1.Font = new System.Drawing.Font("맑은 고딕", 11F, System.Drawing.FontStyle.Bold);
            dataGridViewCellStyle1.ForeColor = System.Drawing.Color.Black;
            dataGridViewCellStyle1.SelectionBackColor = System.Drawing.Color.White;
            dataGridViewCellStyle1.SelectionForeColor = System.Drawing.Color.Black;
            dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.m_dgViewTask.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
            this.m_dgViewTask.ColumnHeadersHeight = 30;
            this.m_dgViewTask.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            this.m_dgViewTask.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.dataGridViewTextBoxColumn1,
            this.dataGridViewTextBoxColumn2});
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle2.BackColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle2.Font = new System.Drawing.Font("맑은 고딕", 10F, System.Drawing.FontStyle.Bold);
            dataGridViewCellStyle2.ForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle2.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(120)))), ((int)(((byte)(130)))), ((int)(((byte)(150)))));
            dataGridViewCellStyle2.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.m_dgViewTask.DefaultCellStyle = dataGridViewCellStyle2;
            this.m_dgViewTask.Dock = System.Windows.Forms.DockStyle.Fill;
            this.m_dgViewTask.EnableHeadersVisualStyles = false;
            this.m_dgViewTask.GridColor = System.Drawing.Color.FromArgb(((int)(((byte)(120)))), ((int)(((byte)(130)))), ((int)(((byte)(150)))));
            this.m_dgViewTask.Location = new System.Drawing.Point(5, 34);
            this.m_dgViewTask.MultiSelect = false;
            this.m_dgViewTask.Name = "m_dgViewTask";
            this.m_dgViewTask.ReadOnly = true;
            this.m_dgViewTask.RowHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.None;
            dataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle3.BackColor = System.Drawing.Color.White;
            dataGridViewCellStyle3.Font = new System.Drawing.Font("맑은 고딕", 11F);
            dataGridViewCellStyle3.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle3.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(120)))), ((int)(((byte)(130)))), ((int)(((byte)(150)))));
            dataGridViewCellStyle3.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle3.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.m_dgViewTask.RowHeadersDefaultCellStyle = dataGridViewCellStyle3;
            this.m_dgViewTask.RowHeadersVisible = false;
            this.m_dgViewTask.RowHeadersWidthSizeMode = System.Windows.Forms.DataGridViewRowHeadersWidthSizeMode.DisableResizing;
            this.m_dgViewTask.RowTemplate.Height = 30;
            this.m_dgViewTask.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.m_dgViewTask.Size = new System.Drawing.Size(417, 422);
            this.m_dgViewTask.TabIndex = 1372;
            this.m_dgViewTask.CellClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.Click_TaskList);
            // 
            // dataGridViewTextBoxColumn1
            // 
            this.dataGridViewTextBoxColumn1.Frozen = true;
            this.dataGridViewTextBoxColumn1.HeaderText = "INDEX";
            this.dataGridViewTextBoxColumn1.MaxInputLength = 20;
            this.dataGridViewTextBoxColumn1.Name = "dataGridViewTextBoxColumn1";
            this.dataGridViewTextBoxColumn1.ReadOnly = true;
            this.dataGridViewTextBoxColumn1.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.dataGridViewTextBoxColumn1.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.dataGridViewTextBoxColumn1.Width = 65;
            // 
            // dataGridViewTextBoxColumn2
            // 
            this.dataGridViewTextBoxColumn2.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.dataGridViewTextBoxColumn2.HeaderText = "NAME";
            this.dataGridViewTextBoxColumn2.MaxInputLength = 20;
            this.dataGridViewTextBoxColumn2.Name = "dataGridViewTextBoxColumn2";
            this.dataGridViewTextBoxColumn2.ReadOnly = true;
            this.dataGridViewTextBoxColumn2.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.dataGridViewTextBoxColumn2.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // m_groupTaskList
            // 
            this.m_groupTaskList.BackGroundColor = System.Drawing.Color.WhiteSmoke;
            this.m_groupTaskList.Controls.Add(this.m_dgViewTask);
            this.m_groupTaskList.Dock = System.Windows.Forms.DockStyle.Fill;
            this.m_groupTaskList.EdgeBorderStroke = 2;
            this.m_groupTaskList.EdgeRadius = 2;
            this.m_groupTaskList.LabelFont = new System.Drawing.Font("맑은 고딕", 10F, System.Drawing.FontStyle.Bold);
            this.m_groupTaskList.LabelGradientColorFirst = System.Drawing.Color.FromArgb(((int)(((byte)(208)))), ((int)(((byte)(208)))), ((int)(((byte)(208)))));
            this.m_groupTaskList.LabelGradientColorSecond = System.Drawing.Color.FromArgb(((int)(((byte)(240)))), ((int)(((byte)(240)))), ((int)(((byte)(240)))));
            this.m_groupTaskList.LabelHeight = 32;
            this.m_groupTaskList.LabelTextColor = System.Drawing.Color.Black;
            this.m_groupTaskList.Location = new System.Drawing.Point(0, 0);
            this.m_groupTaskList.Margin = new System.Windows.Forms.Padding(0);
            this.m_groupTaskList.Name = "m_groupTaskList";
            this.m_groupTaskList.Padding = new System.Windows.Forms.Padding(5, 20, 5, 5);
            this.m_groupTaskList.Size = new System.Drawing.Size(427, 461);
            this.m_groupTaskList.TabIndex = 1373;
            this.m_groupTaskList.TabStop = false;
            this.m_groupTaskList.Text = "TASK LIST";
            this.m_groupTaskList.TextAlign = Sys3Controls.EN_TEXTALIGN.MIDDLE_CENTER;
            this.m_groupTaskList.ThemeIndex = 0;
            this.m_groupTaskList.UseLabelBorder = true;
            this.m_groupTaskList.UseTitle = true;
            // 
            // m_groupDeviceList
            // 
            this.m_groupDeviceList.BackGroundColor = System.Drawing.Color.WhiteSmoke;
            this.m_groupDeviceList.Controls.Add(this.m_dgViewDevice);
            this.m_groupDeviceList.Dock = System.Windows.Forms.DockStyle.Fill;
            this.m_groupDeviceList.EdgeBorderStroke = 2;
            this.m_groupDeviceList.EdgeRadius = 2;
            this.m_groupDeviceList.LabelFont = new System.Drawing.Font("맑은 고딕", 10F, System.Drawing.FontStyle.Bold);
            this.m_groupDeviceList.LabelGradientColorFirst = System.Drawing.Color.FromArgb(((int)(((byte)(208)))), ((int)(((byte)(208)))), ((int)(((byte)(208)))));
            this.m_groupDeviceList.LabelGradientColorSecond = System.Drawing.Color.FromArgb(((int)(((byte)(240)))), ((int)(((byte)(240)))), ((int)(((byte)(240)))));
            this.m_groupDeviceList.LabelHeight = 32;
            this.m_groupDeviceList.LabelTextColor = System.Drawing.Color.Black;
            this.m_groupDeviceList.Location = new System.Drawing.Point(0, 461);
            this.m_groupDeviceList.Margin = new System.Windows.Forms.Padding(0);
            this.m_groupDeviceList.Name = "m_groupDeviceList";
            this.m_groupDeviceList.Padding = new System.Windows.Forms.Padding(5, 20, 5, 5);
            this.tableLayoutPanel1.SetRowSpan(this.m_groupDeviceList, 2);
            this.m_groupDeviceList.Size = new System.Drawing.Size(427, 439);
            this.m_groupDeviceList.TabIndex = 1373;
            this.m_groupDeviceList.TabStop = false;
            this.m_groupDeviceList.Text = "DEVICE LIST";
            this.m_groupDeviceList.TextAlign = Sys3Controls.EN_TEXTALIGN.MIDDLE_CENTER;
            this.m_groupDeviceList.ThemeIndex = 0;
            this.m_groupDeviceList.UseLabelBorder = true;
            this.m_groupDeviceList.UseTitle = true;
            // 
            // m_dgViewDevice
            // 
            this.m_dgViewDevice.AllowUserToAddRows = false;
            this.m_dgViewDevice.AllowUserToDeleteRows = false;
            this.m_dgViewDevice.AllowUserToResizeColumns = false;
            this.m_dgViewDevice.AllowUserToResizeRows = false;
            this.m_dgViewDevice.BackgroundColor = System.Drawing.Color.White;
            this.m_dgViewDevice.CellBorderStyle = System.Windows.Forms.DataGridViewCellBorderStyle.Raised;
            this.m_dgViewDevice.ColumnHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.Single;
            dataGridViewCellStyle4.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle4.BackColor = System.Drawing.Color.LightGray;
            dataGridViewCellStyle4.Font = new System.Drawing.Font("맑은 고딕", 11F, System.Drawing.FontStyle.Bold);
            dataGridViewCellStyle4.ForeColor = System.Drawing.Color.Black;
            dataGridViewCellStyle4.SelectionBackColor = System.Drawing.Color.White;
            dataGridViewCellStyle4.SelectionForeColor = System.Drawing.Color.Black;
            dataGridViewCellStyle4.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.m_dgViewDevice.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle4;
            this.m_dgViewDevice.ColumnHeadersHeight = 30;
            this.m_dgViewDevice.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            this.m_dgViewDevice.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.dataGridViewTextBoxColumn3,
            this.dataGridViewTextBoxColumn4});
            dataGridViewCellStyle5.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle5.BackColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle5.Font = new System.Drawing.Font("맑은 고딕", 10F, System.Drawing.FontStyle.Bold);
            dataGridViewCellStyle5.ForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle5.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(120)))), ((int)(((byte)(130)))), ((int)(((byte)(150)))));
            dataGridViewCellStyle5.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle5.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.m_dgViewDevice.DefaultCellStyle = dataGridViewCellStyle5;
            this.m_dgViewDevice.Dock = System.Windows.Forms.DockStyle.Fill;
            this.m_dgViewDevice.EnableHeadersVisualStyles = false;
            this.m_dgViewDevice.GridColor = System.Drawing.Color.FromArgb(((int)(((byte)(120)))), ((int)(((byte)(130)))), ((int)(((byte)(150)))));
            this.m_dgViewDevice.Location = new System.Drawing.Point(5, 34);
            this.m_dgViewDevice.MultiSelect = false;
            this.m_dgViewDevice.Name = "m_dgViewDevice";
            this.m_dgViewDevice.ReadOnly = true;
            this.m_dgViewDevice.RowHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.None;
            dataGridViewCellStyle6.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle6.BackColor = System.Drawing.Color.White;
            dataGridViewCellStyle6.Font = new System.Drawing.Font("맑은 고딕", 11F);
            dataGridViewCellStyle6.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle6.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(120)))), ((int)(((byte)(130)))), ((int)(((byte)(150)))));
            dataGridViewCellStyle6.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle6.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.m_dgViewDevice.RowHeadersDefaultCellStyle = dataGridViewCellStyle6;
            this.m_dgViewDevice.RowHeadersVisible = false;
            this.m_dgViewDevice.RowHeadersWidthSizeMode = System.Windows.Forms.DataGridViewRowHeadersWidthSizeMode.DisableResizing;
            this.m_dgViewDevice.RowTemplate.Height = 30;
            this.m_dgViewDevice.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.m_dgViewDevice.Size = new System.Drawing.Size(417, 400);
            this.m_dgViewDevice.TabIndex = 1372;
            this.m_dgViewDevice.CellClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.Click_DeviceList);
            // 
            // dataGridViewTextBoxColumn3
            // 
            this.dataGridViewTextBoxColumn3.Frozen = true;
            this.dataGridViewTextBoxColumn3.HeaderText = "INDEX";
            this.dataGridViewTextBoxColumn3.MaxInputLength = 20;
            this.dataGridViewTextBoxColumn3.Name = "dataGridViewTextBoxColumn3";
            this.dataGridViewTextBoxColumn3.ReadOnly = true;
            this.dataGridViewTextBoxColumn3.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.dataGridViewTextBoxColumn3.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.dataGridViewTextBoxColumn3.Width = 65;
            // 
            // dataGridViewTextBoxColumn4
            // 
            this.dataGridViewTextBoxColumn4.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.dataGridViewTextBoxColumn4.HeaderText = "NAME";
            this.dataGridViewTextBoxColumn4.MaxInputLength = 20;
            this.dataGridViewTextBoxColumn4.Name = "dataGridViewTextBoxColumn4";
            this.dataGridViewTextBoxColumn4.ReadOnly = true;
            this.dataGridViewTextBoxColumn4.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.dataGridViewTextBoxColumn4.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // m_dgViewItem
            // 
            this.m_dgViewItem.AllowUserToAddRows = false;
            this.m_dgViewItem.AllowUserToDeleteRows = false;
            this.m_dgViewItem.AllowUserToResizeColumns = false;
            this.m_dgViewItem.AllowUserToResizeRows = false;
            this.m_dgViewItem.BackgroundColor = System.Drawing.Color.White;
            this.m_dgViewItem.CellBorderStyle = System.Windows.Forms.DataGridViewCellBorderStyle.Raised;
            this.m_dgViewItem.ColumnHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.Single;
            dataGridViewCellStyle7.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle7.BackColor = System.Drawing.Color.LightGray;
            dataGridViewCellStyle7.Font = new System.Drawing.Font("맑은 고딕", 11F, System.Drawing.FontStyle.Bold);
            dataGridViewCellStyle7.ForeColor = System.Drawing.Color.Black;
            dataGridViewCellStyle7.SelectionBackColor = System.Drawing.Color.White;
            dataGridViewCellStyle7.SelectionForeColor = System.Drawing.Color.Black;
            dataGridViewCellStyle7.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.m_dgViewItem.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle7;
            this.m_dgViewItem.ColumnHeadersHeight = 30;
            this.m_dgViewItem.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            this.m_dgViewItem.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.dataGridViewTextBoxColumn5,
            this.dataGridViewTextBoxColumn6,
            this.POST_SKIP});
            dataGridViewCellStyle8.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle8.BackColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle8.Font = new System.Drawing.Font("맑은 고딕", 10F, System.Drawing.FontStyle.Bold);
            dataGridViewCellStyle8.ForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle8.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(120)))), ((int)(((byte)(130)))), ((int)(((byte)(150)))));
            dataGridViewCellStyle8.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle8.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.m_dgViewItem.DefaultCellStyle = dataGridViewCellStyle8;
            this.m_dgViewItem.Dock = System.Windows.Forms.DockStyle.Fill;
            this.m_dgViewItem.EnableHeadersVisualStyles = false;
            this.m_dgViewItem.GridColor = System.Drawing.Color.FromArgb(((int)(((byte)(120)))), ((int)(((byte)(130)))), ((int)(((byte)(150)))));
            this.m_dgViewItem.Location = new System.Drawing.Point(5, 34);
            this.m_dgViewItem.MultiSelect = false;
            this.m_dgViewItem.Name = "m_dgViewItem";
            this.m_dgViewItem.ReadOnly = true;
            this.m_dgViewItem.RowHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.None;
            dataGridViewCellStyle9.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle9.BackColor = System.Drawing.Color.White;
            dataGridViewCellStyle9.Font = new System.Drawing.Font("맑은 고딕", 11F);
            dataGridViewCellStyle9.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle9.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(120)))), ((int)(((byte)(130)))), ((int)(((byte)(150)))));
            dataGridViewCellStyle9.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle9.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.m_dgViewItem.RowHeadersDefaultCellStyle = dataGridViewCellStyle9;
            this.m_dgViewItem.RowHeadersVisible = false;
            this.m_dgViewItem.RowHeadersWidthSizeMode = System.Windows.Forms.DataGridViewRowHeadersWidthSizeMode.DisableResizing;
            this.m_dgViewItem.RowTemplate.Height = 30;
            this.m_dgViewItem.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.m_dgViewItem.Size = new System.Drawing.Size(719, 655);
            this.m_dgViewItem.TabIndex = 1374;
            this.m_dgViewItem.CellClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.Click_ItemList);
            this.m_dgViewItem.CellDoubleClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.Click_ItemList);
            // 
            // dataGridViewTextBoxColumn5
            // 
            this.dataGridViewTextBoxColumn5.Frozen = true;
            this.dataGridViewTextBoxColumn5.HeaderText = "INDEX";
            this.dataGridViewTextBoxColumn5.MaxInputLength = 20;
            this.dataGridViewTextBoxColumn5.Name = "dataGridViewTextBoxColumn5";
            this.dataGridViewTextBoxColumn5.ReadOnly = true;
            this.dataGridViewTextBoxColumn5.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.dataGridViewTextBoxColumn5.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // dataGridViewTextBoxColumn6
            // 
            this.dataGridViewTextBoxColumn6.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.dataGridViewTextBoxColumn6.HeaderText = "NAME";
            this.dataGridViewTextBoxColumn6.MaxInputLength = 20;
            this.dataGridViewTextBoxColumn6.Name = "dataGridViewTextBoxColumn6";
            this.dataGridViewTextBoxColumn6.ReadOnly = true;
            this.dataGridViewTextBoxColumn6.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.dataGridViewTextBoxColumn6.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // POST_SKIP
            // 
            this.POST_SKIP.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.ColumnHeader;
            this.POST_SKIP.HeaderText = "TARGET INDEX";
            this.POST_SKIP.Name = "POST_SKIP";
            this.POST_SKIP.ReadOnly = true;
            this.POST_SKIP.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.POST_SKIP.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.POST_SKIP.Width = 123;
            // 
            // m_groupItemList
            // 
            this.m_groupItemList.BackGroundColor = System.Drawing.Color.WhiteSmoke;
            this.m_groupItemList.Controls.Add(this.m_dgViewItem);
            this.m_groupItemList.Dock = System.Windows.Forms.DockStyle.Fill;
            this.m_groupItemList.EdgeBorderStroke = 2;
            this.m_groupItemList.EdgeRadius = 2;
            this.m_groupItemList.LabelFont = new System.Drawing.Font("맑은 고딕", 10F, System.Drawing.FontStyle.Bold);
            this.m_groupItemList.LabelGradientColorFirst = System.Drawing.Color.FromArgb(((int)(((byte)(208)))), ((int)(((byte)(208)))), ((int)(((byte)(208)))));
            this.m_groupItemList.LabelGradientColorSecond = System.Drawing.Color.FromArgb(((int)(((byte)(240)))), ((int)(((byte)(240)))), ((int)(((byte)(240)))));
            this.m_groupItemList.LabelHeight = 32;
            this.m_groupItemList.LabelTextColor = System.Drawing.Color.Black;
            this.m_groupItemList.Location = new System.Drawing.Point(427, 0);
            this.m_groupItemList.Margin = new System.Windows.Forms.Padding(0);
            this.m_groupItemList.Name = "m_groupItemList";
            this.m_groupItemList.Padding = new System.Windows.Forms.Padding(5, 20, 5, 5);
            this.tableLayoutPanel1.SetRowSpan(this.m_groupItemList, 2);
            this.m_groupItemList.Size = new System.Drawing.Size(729, 694);
            this.m_groupItemList.TabIndex = 1375;
            this.m_groupItemList.TabStop = false;
            this.m_groupItemList.Text = "ITEM LIST";
            this.m_groupItemList.TextAlign = Sys3Controls.EN_TEXTALIGN.MIDDLE_CENTER;
            this.m_groupItemList.ThemeIndex = 0;
            this.m_groupItemList.UseLabelBorder = true;
            this.m_groupItemList.UseTitle = true;
            // 
            // m_lblName
            // 
            this.m_lblName.BackGroundColor = System.Drawing.Color.FromArgb(((int)(((byte)(200)))), ((int)(((byte)(200)))), ((int)(((byte)(220)))));
            this.m_lblName.BorderStroke = 2;
            this.m_lblName.BorderStyle = System.Windows.Forms.ButtonBorderStyle.Solid;
            this.m_lblName.Description = "";
            this.m_lblName.DisabledColor = System.Drawing.Color.DarkGray;
            this.m_lblName.Dock = System.Windows.Forms.DockStyle.Fill;
            this.m_lblName.EdgeRadius = 1;
            this.m_lblName.ImagePosition = new System.Drawing.Point(0, 0);
            this.m_lblName.ImageSize = new System.Drawing.Point(0, 0);
            this.m_lblName.LoadImage = null;
            this.m_lblName.Location = new System.Drawing.Point(34, 22);
            this.m_lblName.MainFont = new System.Drawing.Font("맑은 고딕", 9F, System.Drawing.FontStyle.Bold);
            this.m_lblName.MainFontColor = System.Drawing.Color.Black;
            this.m_lblName.Margin = new System.Windows.Forms.Padding(1);
            this.m_lblName.Name = "m_lblName";
            this.m_lblName.Size = new System.Drawing.Size(131, 60);
            this.m_lblName.SubFont = new System.Drawing.Font("맑은 고딕", 10F);
            this.m_lblName.SubFontColor = System.Drawing.Color.Black;
            this.m_lblName.SubText = "";
            this.m_lblName.TabIndex = 1379;
            this.m_lblName.Text = "NAME";
            this.m_lblName.TextAlignMain = Sys3Controls.EN_TEXTALIGN.TOP_LEFT;
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
            // m_labelName
            // 
            this.m_labelName.BackGroundColor = System.Drawing.Color.White;
            this.m_labelName.BorderStroke = 2;
            this.m_labelName.BorderStyle = System.Windows.Forms.ButtonBorderStyle.Solid;
            this.m_labelName.Description = "";
            this.m_labelName.DisabledColor = System.Drawing.Color.LightGray;
            this.m_labelName.Dock = System.Windows.Forms.DockStyle.Fill;
            this.m_labelName.EdgeRadius = 1;
            this.m_labelName.Enabled = false;
            this.m_labelName.ImagePosition = new System.Drawing.Point(0, 0);
            this.m_labelName.ImageSize = new System.Drawing.Point(0, 0);
            this.m_labelName.LoadImage = null;
            this.m_labelName.Location = new System.Drawing.Point(167, 22);
            this.m_labelName.MainFont = new System.Drawing.Font("맑은 고딕", 11F, System.Drawing.FontStyle.Bold);
            this.m_labelName.MainFontColor = System.Drawing.Color.Black;
            this.m_labelName.Margin = new System.Windows.Forms.Padding(1);
            this.m_labelName.Name = "m_labelName";
            this.m_labelName.Size = new System.Drawing.Size(331, 60);
            this.m_labelName.SubFont = new System.Drawing.Font("맑은 고딕", 10F);
            this.m_labelName.SubFontColor = System.Drawing.Color.Black;
            this.m_labelName.SubText = "";
            this.m_labelName.TabIndex = 0;
            this.m_labelName.Text = "--";
            this.m_labelName.TextAlignMain = Sys3Controls.EN_TEXTALIGN.BOTTOM_RIGHT;
            this.m_labelName.TextAlignSub = Sys3Controls.EN_TEXTALIGN.MIDDLE_CENTER;
            this.m_labelName.TextAlignUnit = Sys3Controls.EN_TEXTALIGN.MIDDLE_CENTER;
            this.m_labelName.ThemeIndex = 0;
            this.m_labelName.UnitAreaRate = 40;
            this.m_labelName.UnitFont = new System.Drawing.Font("맑은 고딕", 10F, System.Drawing.FontStyle.Bold);
            this.m_labelName.UnitFontColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.m_labelName.UnitPositionVertical = false;
            this.m_labelName.UnitText = "";
            this.m_labelName.UseBorder = true;
            this.m_labelName.UseEdgeRadius = false;
            this.m_labelName.UseImage = false;
            this.m_labelName.UseSubFont = false;
            this.m_labelName.UseUnitFont = false;
            // 
            // m_groupConfiguration
            // 
            this.m_groupConfiguration.BackGroundColor = System.Drawing.Color.WhiteSmoke;
            this.m_groupConfiguration.Controls.Add(this.tableLayoutPanel2);
            this.m_groupConfiguration.Dock = System.Windows.Forms.DockStyle.Fill;
            this.m_groupConfiguration.EdgeBorderStroke = 2;
            this.m_groupConfiguration.EdgeRadius = 2;
            this.m_groupConfiguration.LabelFont = new System.Drawing.Font("맑은 고딕", 10F, System.Drawing.FontStyle.Bold);
            this.m_groupConfiguration.LabelGradientColorFirst = System.Drawing.Color.FromArgb(((int)(((byte)(208)))), ((int)(((byte)(208)))), ((int)(((byte)(208)))));
            this.m_groupConfiguration.LabelGradientColorSecond = System.Drawing.Color.FromArgb(((int)(((byte)(240)))), ((int)(((byte)(240)))), ((int)(((byte)(240)))));
            this.m_groupConfiguration.LabelHeight = 32;
            this.m_groupConfiguration.LabelTextColor = System.Drawing.Color.Black;
            this.m_groupConfiguration.Location = new System.Drawing.Point(427, 694);
            this.m_groupConfiguration.Margin = new System.Windows.Forms.Padding(0);
            this.m_groupConfiguration.Name = "m_groupConfiguration";
            this.m_groupConfiguration.Padding = new System.Windows.Forms.Padding(3, 20, 3, 3);
            this.m_groupConfiguration.Size = new System.Drawing.Size(729, 206);
            this.m_groupConfiguration.TabIndex = 1377;
            this.m_groupConfiguration.TabStop = false;
            this.m_groupConfiguration.Text = "CONFIGURATION";
            this.m_groupConfiguration.TextAlign = Sys3Controls.EN_TEXTALIGN.MIDDLE_CENTER;
            this.m_groupConfiguration.ThemeIndex = 0;
            this.m_groupConfiguration.UseLabelBorder = true;
            this.m_groupConfiguration.UseTitle = true;
            // 
            // tableLayoutPanel2
            // 
            this.tableLayoutPanel2.ColumnCount = 2;
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 73.72061F));
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 26.27939F));
            this.tableLayoutPanel2.Controls.Add(this.tableLayoutPanel3, 0, 0);
            this.tableLayoutPanel2.Controls.Add(this.tableLayoutPanel4, 1, 0);
            this.tableLayoutPanel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel2.Location = new System.Drawing.Point(3, 34);
            this.tableLayoutPanel2.Name = "tableLayoutPanel2";
            this.tableLayoutPanel2.RowCount = 1;
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel2.Size = new System.Drawing.Size(723, 169);
            this.tableLayoutPanel2.TabIndex = 0;
            // 
            // tableLayoutPanel3
            // 
            this.tableLayoutPanel3.ColumnCount = 4;
            this.tableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 6.25F));
            this.tableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.tableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 62.5F));
            this.tableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 6.25F));
            this.tableLayoutPanel3.Controls.Add(this.m_lblName, 1, 1);
            this.tableLayoutPanel3.Controls.Add(this.m_labelTargetIndex, 2, 2);
            this.tableLayoutPanel3.Controls.Add(this.m_lblTargetIndex, 1, 2);
            this.tableLayoutPanel3.Controls.Add(this.m_labelName, 2, 1);
            this.tableLayoutPanel3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel3.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel3.Margin = new System.Windows.Forms.Padding(0);
            this.tableLayoutPanel3.Name = "tableLayoutPanel3";
            this.tableLayoutPanel3.RowCount = 4;
            this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 12.83784F));
            this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 37.16216F));
            this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 37.16216F));
            this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 12.83784F));
            this.tableLayoutPanel3.Size = new System.Drawing.Size(533, 169);
            this.tableLayoutPanel3.TabIndex = 0;
            // 
            // m_labelTargetIndex
            // 
            this.m_labelTargetIndex.BackGroundColor = System.Drawing.Color.White;
            this.m_labelTargetIndex.BorderStroke = 2;
            this.m_labelTargetIndex.BorderStyle = System.Windows.Forms.ButtonBorderStyle.Solid;
            this.m_labelTargetIndex.Description = "";
            this.m_labelTargetIndex.DisabledColor = System.Drawing.Color.LightGray;
            this.m_labelTargetIndex.Dock = System.Windows.Forms.DockStyle.Fill;
            this.m_labelTargetIndex.EdgeRadius = 1;
            this.m_labelTargetIndex.Enabled = false;
            this.m_labelTargetIndex.ImagePosition = new System.Drawing.Point(0, 0);
            this.m_labelTargetIndex.ImageSize = new System.Drawing.Point(0, 0);
            this.m_labelTargetIndex.LoadImage = null;
            this.m_labelTargetIndex.Location = new System.Drawing.Point(167, 84);
            this.m_labelTargetIndex.MainFont = new System.Drawing.Font("맑은 고딕", 11F, System.Drawing.FontStyle.Bold);
            this.m_labelTargetIndex.MainFontColor = System.Drawing.Color.Black;
            this.m_labelTargetIndex.Margin = new System.Windows.Forms.Padding(1);
            this.m_labelTargetIndex.Name = "m_labelTargetIndex";
            this.m_labelTargetIndex.Size = new System.Drawing.Size(331, 60);
            this.m_labelTargetIndex.SubFont = new System.Drawing.Font("맑은 고딕", 10F, System.Drawing.FontStyle.Bold);
            this.m_labelTargetIndex.SubFontColor = System.Drawing.Color.Black;
            this.m_labelTargetIndex.SubText = "[ -1 ]";
            this.m_labelTargetIndex.TabIndex = 1;
            this.m_labelTargetIndex.Text = "--";
            this.m_labelTargetIndex.TextAlignMain = Sys3Controls.EN_TEXTALIGN.BOTTOM_RIGHT;
            this.m_labelTargetIndex.TextAlignSub = Sys3Controls.EN_TEXTALIGN.TOP_LEFT;
            this.m_labelTargetIndex.TextAlignUnit = Sys3Controls.EN_TEXTALIGN.MIDDLE_CENTER;
            this.m_labelTargetIndex.ThemeIndex = 0;
            this.m_labelTargetIndex.UnitAreaRate = 40;
            this.m_labelTargetIndex.UnitFont = new System.Drawing.Font("맑은 고딕", 10F, System.Drawing.FontStyle.Bold);
            this.m_labelTargetIndex.UnitFontColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.m_labelTargetIndex.UnitPositionVertical = false;
            this.m_labelTargetIndex.UnitText = "";
            this.m_labelTargetIndex.UseBorder = true;
            this.m_labelTargetIndex.UseEdgeRadius = false;
            this.m_labelTargetIndex.UseImage = false;
            this.m_labelTargetIndex.UseSubFont = false;
            this.m_labelTargetIndex.UseUnitFont = false;
            this.m_labelTargetIndex.Click += new System.EventHandler(this.Click_TargetIndex);
            // 
            // m_lblTargetIndex
            // 
            this.m_lblTargetIndex.BackGroundColor = System.Drawing.Color.FromArgb(((int)(((byte)(200)))), ((int)(((byte)(200)))), ((int)(((byte)(220)))));
            this.m_lblTargetIndex.BorderStroke = 2;
            this.m_lblTargetIndex.BorderStyle = System.Windows.Forms.ButtonBorderStyle.Solid;
            this.m_lblTargetIndex.Description = "";
            this.m_lblTargetIndex.DisabledColor = System.Drawing.Color.DarkGray;
            this.m_lblTargetIndex.Dock = System.Windows.Forms.DockStyle.Fill;
            this.m_lblTargetIndex.EdgeRadius = 1;
            this.m_lblTargetIndex.ImagePosition = new System.Drawing.Point(0, 0);
            this.m_lblTargetIndex.ImageSize = new System.Drawing.Point(0, 0);
            this.m_lblTargetIndex.LoadImage = null;
            this.m_lblTargetIndex.Location = new System.Drawing.Point(34, 84);
            this.m_lblTargetIndex.MainFont = new System.Drawing.Font("맑은 고딕", 9F, System.Drawing.FontStyle.Bold);
            this.m_lblTargetIndex.MainFontColor = System.Drawing.Color.Black;
            this.m_lblTargetIndex.Margin = new System.Windows.Forms.Padding(1);
            this.m_lblTargetIndex.Name = "m_lblTargetIndex";
            this.m_lblTargetIndex.Size = new System.Drawing.Size(131, 60);
            this.m_lblTargetIndex.SubFont = new System.Drawing.Font("맑은 고딕", 9F, System.Drawing.FontStyle.Bold);
            this.m_lblTargetIndex.SubFontColor = System.Drawing.Color.Black;
            this.m_lblTargetIndex.SubText = "TARGET NAME";
            this.m_lblTargetIndex.TabIndex = 1379;
            this.m_lblTargetIndex.Text = "TARGET INDEX";
            this.m_lblTargetIndex.TextAlignMain = Sys3Controls.EN_TEXTALIGN.TOP_LEFT;
            this.m_lblTargetIndex.TextAlignSub = Sys3Controls.EN_TEXTALIGN.BOTTOM_RIGHT;
            this.m_lblTargetIndex.TextAlignUnit = Sys3Controls.EN_TEXTALIGN.MIDDLE_CENTER;
            this.m_lblTargetIndex.ThemeIndex = 0;
            this.m_lblTargetIndex.UnitAreaRate = 40;
            this.m_lblTargetIndex.UnitFont = new System.Drawing.Font("맑은 고딕", 10F, System.Drawing.FontStyle.Bold);
            this.m_lblTargetIndex.UnitFontColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.m_lblTargetIndex.UnitPositionVertical = false;
            this.m_lblTargetIndex.UnitText = "";
            this.m_lblTargetIndex.UseBorder = true;
            this.m_lblTargetIndex.UseEdgeRadius = false;
            this.m_lblTargetIndex.UseImage = false;
            this.m_lblTargetIndex.UseSubFont = false;
            this.m_lblTargetIndex.UseUnitFont = false;
            // 
            // tableLayoutPanel4
            // 
            this.tableLayoutPanel4.ColumnCount = 1;
            this.tableLayoutPanel4.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel4.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel4.Controls.Add(this.m_btnAdd, 0, 0);
            this.tableLayoutPanel4.Controls.Add(this.m_btnRemove, 0, 1);
            this.tableLayoutPanel4.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel4.Location = new System.Drawing.Point(533, 0);
            this.tableLayoutPanel4.Margin = new System.Windows.Forms.Padding(0);
            this.tableLayoutPanel4.Name = "tableLayoutPanel4";
            this.tableLayoutPanel4.RowCount = 2;
            this.tableLayoutPanel4.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel4.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel4.Size = new System.Drawing.Size(190, 169);
            this.tableLayoutPanel4.TabIndex = 1;
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
            this.m_btnAdd.GradientAngle = 70F;
            this.m_btnAdd.GradientFirstColor = System.Drawing.Color.White;
            this.m_btnAdd.GradientSecondColor = System.Drawing.Color.FromArgb(((int)(((byte)(170)))), ((int)(((byte)(176)))), ((int)(((byte)(183)))));
            this.m_btnAdd.HoverEmphasizeCustomColor = System.Drawing.Color.White;
            this.m_btnAdd.ImagePosition = new System.Drawing.Point(7, 7);
            this.m_btnAdd.ImageSize = new System.Drawing.Point(30, 30);
            this.m_btnAdd.LoadImage = global::FrameOfSystem3.Properties.Resources.CONFIG_ADD3;
            this.m_btnAdd.Location = new System.Drawing.Point(3, 3);
            this.m_btnAdd.MainFont = new System.Drawing.Font("맑은 고딕", 13F, System.Drawing.FontStyle.Bold);
            this.m_btnAdd.MainFontColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(36)))), ((int)(((byte)(0)))));
            this.m_btnAdd.Name = "m_btnAdd";
            this.m_btnAdd.Size = new System.Drawing.Size(184, 78);
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
            this.m_btnAdd.Click += new System.EventHandler(this.Click_Button);
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
            this.m_btnRemove.Location = new System.Drawing.Point(3, 87);
            this.m_btnRemove.MainFont = new System.Drawing.Font("맑은 고딕", 13F, System.Drawing.FontStyle.Bold);
            this.m_btnRemove.MainFontColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(36)))), ((int)(((byte)(0)))));
            this.m_btnRemove.Name = "m_btnRemove";
            this.m_btnRemove.Size = new System.Drawing.Size(184, 79);
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
            this.m_btnRemove.Click += new System.EventHandler(this.Click_Button);
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 2;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 37.02422F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 62.97578F));
            this.tableLayoutPanel1.Controls.Add(this.m_groupTaskList, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.m_groupDeviceList, 0, 1);
            this.tableLayoutPanel1.Controls.Add(this.m_groupItemList, 1, 0);
            this.tableLayoutPanel1.Controls.Add(this.m_groupConfiguration, 1, 2);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 3;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 66.42651F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 33.57349F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 205F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(1156, 900);
            this.tableLayoutPanel1.TabIndex = 1380;
            // 
            // Config_Device
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.Controls.Add(this.tableLayoutPanel1);
            this.Margin = new System.Windows.Forms.Padding(0);
            this.Name = "Config_Device";
            this.Size = new System.Drawing.Size(1156, 900);
            ((System.ComponentModel.ISupportInitialize)(this.m_dgViewTask)).EndInit();
            this.m_groupTaskList.ResumeLayout(false);
            this.m_groupDeviceList.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.m_dgViewDevice)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.m_dgViewItem)).EndInit();
            this.m_groupItemList.ResumeLayout(false);
            this.m_groupConfiguration.ResumeLayout(false);
            this.tableLayoutPanel2.ResumeLayout(false);
            this.tableLayoutPanel3.ResumeLayout(false);
            this.tableLayoutPanel4.ResumeLayout(false);
            this.tableLayoutPanel1.ResumeLayout(false);
            this.ResumeLayout(false);

		}

		#endregion

		private Sys3Controls.Sys3DoubleBufferedDataGridView m_dgViewTask;
		private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn1;
		private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn2;
		private Sys3Controls.Sys3GroupBoxContainer m_groupTaskList;
        private Sys3Controls.Sys3GroupBoxContainer m_groupDeviceList;
		private Sys3Controls.Sys3DoubleBufferedDataGridView m_dgViewDevice;
		private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn3;
		private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn4;
		private Sys3Controls.Sys3DoubleBufferedDataGridView m_dgViewItem;
        private Sys3Controls.Sys3GroupBoxContainer m_groupItemList;
		private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn5;
		private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn6;
		private System.Windows.Forms.DataGridViewTextBoxColumn POST_SKIP;
		private Sys3Controls.Sys3Label m_lblName;
		private Sys3Controls.Sys3Label m_labelName;
        private Sys3Controls.Sys3GroupBoxContainer m_groupConfiguration;
		private Sys3Controls.Sys3button m_btnRemove;
		private Sys3Controls.Sys3button m_btnAdd;
		private Sys3Controls.Sys3Label m_labelTargetIndex;
		private Sys3Controls.Sys3Label m_lblTargetIndex;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel2;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel3;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel4;
	}
}
