namespace FrameOfSystem3.Views.Operation
{
	partial class Operation_StateMonitor
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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle10 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle11 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle12 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle13 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle14 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle15 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle7 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle8 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle9 = new System.Windows.Forms.DataGridViewCellStyle();
            this.m_dgViewTask = new Sys3Controls.Sys3DoubleBufferedDataGridView();
            this.INDEX = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.MONITOR = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ID = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.GRADE = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.BESEQ = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.AESEQ = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Flow = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.m_ledAlarm = new Sys3Controls.Sys3LedLabel();
            this.m_ledRun = new Sys3Controls.Sys3LedLabel();
            this.m_ledSetup = new Sys3Controls.Sys3LedLabel();
            this.m_ledIdle = new Sys3Controls.Sys3LedLabel();
            this.m_ledInit = new Sys3Controls.Sys3LedLabel();
            this.m_ledPause = new Sys3Controls.Sys3LedLabel();
            this.m_dgViewAction = new Sys3Controls.Sys3DoubleBufferedDataGridView();
            this.dataGridViewTextBoxColumn3 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn2 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.TIMER = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.m_groupMonitoring = new Sys3Controls.Sys3GroupBoxContainer();
            this.m_groupTaskState = new Sys3Controls.Sys3GroupBoxContainer();
            this.m_groupActionState = new Sys3Controls.Sys3GroupBoxContainer();
            this.sys3GroupBox1 = new Sys3Controls.Sys3GroupBoxContainer();
            this.sys3GroupBox2 = new Sys3Controls.Sys3GroupBoxContainer();
            this.m_lblTimerPause = new Sys3Controls.Sys3Label();
            this.m_lblTimerInit = new Sys3Controls.Sys3Label();
            this.m_lblTimerIdle = new Sys3Controls.Sys3Label();
            this.m_lblTimerSetup = new Sys3Controls.Sys3Label();
            this.m_lblTimerRun = new Sys3Controls.Sys3Label();
            this.m_lblTimerAlarm = new Sys3Controls.Sys3Label();
            this.m_labelTimerPause = new Sys3Controls.Sys3Label();
            this.m_labelTimerInit = new Sys3Controls.Sys3Label();
            this.m_labelTimerIdle = new Sys3Controls.Sys3Label();
            this.m_labelTimerSetup = new Sys3Controls.Sys3Label();
            this.m_labelTimerRun = new Sys3Controls.Sys3Label();
            this.m_labelTimerAlarm = new Sys3Controls.Sys3Label();
            this.m_lblMainThread = new Sys3Controls.Sys3Label();
            this.m_lblObserver = new Sys3Controls.Sys3Label();
            this.m_lblFileIO = new Sys3Controls.Sys3Label();
            this.m_lblDigitalIO = new Sys3Controls.Sys3Label();
            this.m_lblAnalogIO = new Sys3Controls.Sys3Label();
            this.m_lblMotion = new Sys3Controls.Sys3Label();
            this.m_lblGathering = new Sys3Controls.Sys3Label();
            this.m_lblCommunication = new Sys3Controls.Sys3Label();
            this.m_lblEtcInstance = new Sys3Controls.Sys3Label();
            this.m_labelMainInterval = new Sys3Controls.Sys3Label();
            this.m_labelMainMax = new Sys3Controls.Sys3Label();
            this.m_labelMainAverage = new Sys3Controls.Sys3Label();
            this.m_labelObserverInterval = new Sys3Controls.Sys3Label();
            this.m_labelObserverMax = new Sys3Controls.Sys3Label();
            this.m_labelObserverAverage = new Sys3Controls.Sys3Label();
            this.m_labelFileIOInterval = new Sys3Controls.Sys3Label();
            this.m_labelFileIOMax = new Sys3Controls.Sys3Label();
            this.m_labelFileIOAverage = new Sys3Controls.Sys3Label();
            this.m_labelDegitalIOInterval = new Sys3Controls.Sys3Label();
            this.m_labelDegitalIOMax = new Sys3Controls.Sys3Label();
            this.m_labelDegitalIOAverage = new Sys3Controls.Sys3Label();
            this.m_labelAnalogIOInterval = new Sys3Controls.Sys3Label();
            this.m_labelAnalogIOMax = new Sys3Controls.Sys3Label();
            this.m_labelAnalogIOAverage = new Sys3Controls.Sys3Label();
            this.m_labelMotionInterval = new Sys3Controls.Sys3Label();
            this.m_labelMotionMax = new Sys3Controls.Sys3Label();
            this.m_labelMotionAverage = new Sys3Controls.Sys3Label();
            this.m_labelGatheringInterval = new Sys3Controls.Sys3Label();
            this.m_labelGatheringMax = new Sys3Controls.Sys3Label();
            this.m_labelGatheringAverage = new Sys3Controls.Sys3Label();
            this.m_labelCommunicationInterval = new Sys3Controls.Sys3Label();
            this.m_labelCommunicationMax = new Sys3Controls.Sys3Label();
            this.m_labelCommunicationAverage = new Sys3Controls.Sys3Label();
            this.m_labelEtcInstanceInterval = new Sys3Controls.Sys3Label();
            this.m_labelEtcInstanceMax = new Sys3Controls.Sys3Label();
            this.m_labelEtcInstanceAverage = new Sys3Controls.Sys3Label();
            this.m_labelMainProcess = new Sys3Controls.Sys3Label();
            this.m_labelObserverProcess = new Sys3Controls.Sys3Label();
            this.m_labelFileIOProcess = new Sys3Controls.Sys3Label();
            this.m_labelDegitalIOProcess = new Sys3Controls.Sys3Label();
            this.m_labelAnalogIOProcess = new Sys3Controls.Sys3Label();
            this.m_labelMotionProcess = new Sys3Controls.Sys3Label();
            this.m_labelGatheringProcess = new Sys3Controls.Sys3Label();
            this.m_labelCommunicationProcess = new Sys3Controls.Sys3Label();
            this.m_labelEtcInstanceProcess = new Sys3Controls.Sys3Label();
            this.m_dgViewLinkedAction = new Sys3Controls.Sys3DoubleBufferedDataGridView();
            this.dataGridViewTextBoxColumn12 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn13 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn14 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.m_dgViewDerefAction = new Sys3Controls.Sys3DoubleBufferedDataGridView();
            this.dataGridViewTextBoxColumn4 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn5 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn6 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn7 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.m_dgViewActionPort = new Sys3Controls.Sys3DoubleBufferedDataGridView();
            this.ColumnTask = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn8 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this._tableLayoutPanel_Main = new System.Windows.Forms.TableLayoutPanel();
            this._tableLayoutPanel_LeftBottom = new System.Windows.Forms.TableLayoutPanel();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this._tableLayoutPanel_ActionState = new System.Windows.Forms.TableLayoutPanel();
            ((System.ComponentModel.ISupportInitialize)(this.m_dgViewTask)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.m_dgViewAction)).BeginInit();
            this.m_groupMonitoring.SuspendLayout();
            this.m_groupTaskState.SuspendLayout();
            this.m_groupActionState.SuspendLayout();
            this.sys3GroupBox1.SuspendLayout();
            this.sys3GroupBox2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.m_dgViewLinkedAction)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.m_dgViewDerefAction)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.m_dgViewActionPort)).BeginInit();
            this._tableLayoutPanel_Main.SuspendLayout();
            this._tableLayoutPanel_LeftBottom.SuspendLayout();
            this.tableLayoutPanel1.SuspendLayout();
            this._tableLayoutPanel_ActionState.SuspendLayout();
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
            dataGridViewCellStyle1.Font = new System.Drawing.Font("맑은 고딕", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            dataGridViewCellStyle1.ForeColor = System.Drawing.Color.Black;
            dataGridViewCellStyle1.SelectionBackColor = System.Drawing.Color.White;
            dataGridViewCellStyle1.SelectionForeColor = System.Drawing.Color.Black;
            dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.m_dgViewTask.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
            this.m_dgViewTask.ColumnHeadersHeight = 30;
            this.m_dgViewTask.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            this.m_dgViewTask.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.INDEX,
            this.MONITOR,
            this.ID,
            this.GRADE,
            this.BESEQ,
            this.AESEQ,
            this.Flow});
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
            this.m_dgViewTask.Size = new System.Drawing.Size(788, 176);
            this.m_dgViewTask.TabIndex = 1285;
            this.m_dgViewTask.CellClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.Click_Task);
            // 
            // INDEX
            // 
            this.INDEX.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.INDEX.HeaderText = "TASK";
            this.INDEX.Name = "INDEX";
            this.INDEX.ReadOnly = true;
            this.INDEX.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.INDEX.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // MONITOR
            // 
            this.MONITOR.HeaderText = "STATE";
            this.MONITOR.Name = "MONITOR";
            this.MONITOR.ReadOnly = true;
            this.MONITOR.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.MONITOR.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.MONITOR.Width = 80;
            // 
            // ID
            // 
            this.ID.HeaderText = "ACTION";
            this.ID.MaxInputLength = 20;
            this.ID.Name = "ID";
            this.ID.ReadOnly = true;
            this.ID.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.ID.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.ID.Width = 250;
            // 
            // GRADE
            // 
            this.GRADE.HeaderText = "SEQ";
            this.GRADE.Name = "GRADE";
            this.GRADE.ReadOnly = true;
            this.GRADE.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.GRADE.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.GRADE.Width = 65;
            // 
            // BESEQ
            // 
            this.BESEQ.HeaderText = "BESEQ";
            this.BESEQ.Name = "BESEQ";
            this.BESEQ.ReadOnly = true;
            this.BESEQ.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.BESEQ.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.BESEQ.Width = 65;
            // 
            // AESEQ
            // 
            this.AESEQ.HeaderText = "AESEQ";
            this.AESEQ.Name = "AESEQ";
            this.AESEQ.ReadOnly = true;
            this.AESEQ.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.AESEQ.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.AESEQ.Width = 65;
            // 
            // Flow
            // 
            this.Flow.HeaderText = "FLOW";
            this.Flow.Name = "Flow";
            this.Flow.ReadOnly = true;
            this.Flow.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.Flow.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.Flow.Width = 65;
            // 
            // m_ledAlarm
            // 
            this.m_ledAlarm.Active = false;
            this.m_ledAlarm.ColorActive = System.Drawing.Color.DodgerBlue;
            this.m_ledAlarm.ColorNormal = System.Drawing.Color.DimGray;
            this.m_ledAlarm.Dock = System.Windows.Forms.DockStyle.Fill;
            this.m_ledAlarm.Location = new System.Drawing.Point(15, 178);
            this.m_ledAlarm.Margin = new System.Windows.Forms.Padding(1);
            this.m_ledAlarm.Name = "m_ledAlarm";
            this.m_ledAlarm.Size = new System.Drawing.Size(23, 33);
            this.m_ledAlarm.TabIndex = 1274;
            // 
            // m_ledRun
            // 
            this.m_ledRun.Active = false;
            this.m_ledRun.ColorActive = System.Drawing.Color.DodgerBlue;
            this.m_ledRun.ColorNormal = System.Drawing.Color.DimGray;
            this.m_ledRun.Dock = System.Windows.Forms.DockStyle.Fill;
            this.m_ledRun.Location = new System.Drawing.Point(15, 143);
            this.m_ledRun.Margin = new System.Windows.Forms.Padding(1);
            this.m_ledRun.Name = "m_ledRun";
            this.m_ledRun.Size = new System.Drawing.Size(23, 33);
            this.m_ledRun.TabIndex = 1272;
            // 
            // m_ledSetup
            // 
            this.m_ledSetup.Active = false;
            this.m_ledSetup.ColorActive = System.Drawing.Color.DodgerBlue;
            this.m_ledSetup.ColorNormal = System.Drawing.Color.DimGray;
            this.m_ledSetup.Dock = System.Windows.Forms.DockStyle.Fill;
            this.m_ledSetup.Location = new System.Drawing.Point(15, 108);
            this.m_ledSetup.Margin = new System.Windows.Forms.Padding(1);
            this.m_ledSetup.Name = "m_ledSetup";
            this.m_ledSetup.Size = new System.Drawing.Size(23, 33);
            this.m_ledSetup.TabIndex = 1270;
            // 
            // m_ledIdle
            // 
            this.m_ledIdle.Active = false;
            this.m_ledIdle.ColorActive = System.Drawing.Color.DodgerBlue;
            this.m_ledIdle.ColorNormal = System.Drawing.Color.DimGray;
            this.m_ledIdle.Dock = System.Windows.Forms.DockStyle.Fill;
            this.m_ledIdle.Location = new System.Drawing.Point(15, 73);
            this.m_ledIdle.Margin = new System.Windows.Forms.Padding(1);
            this.m_ledIdle.Name = "m_ledIdle";
            this.m_ledIdle.Size = new System.Drawing.Size(23, 33);
            this.m_ledIdle.TabIndex = 1268;
            // 
            // m_ledInit
            // 
            this.m_ledInit.Active = false;
            this.m_ledInit.ColorActive = System.Drawing.Color.DodgerBlue;
            this.m_ledInit.ColorNormal = System.Drawing.Color.DimGray;
            this.m_ledInit.Dock = System.Windows.Forms.DockStyle.Fill;
            this.m_ledInit.Location = new System.Drawing.Point(15, 38);
            this.m_ledInit.Margin = new System.Windows.Forms.Padding(1);
            this.m_ledInit.Name = "m_ledInit";
            this.m_ledInit.Size = new System.Drawing.Size(23, 33);
            this.m_ledInit.TabIndex = 1266;
            // 
            // m_ledPause
            // 
            this.m_ledPause.Active = false;
            this.m_ledPause.ColorActive = System.Drawing.Color.DodgerBlue;
            this.m_ledPause.ColorNormal = System.Drawing.Color.DimGray;
            this.m_ledPause.Dock = System.Windows.Forms.DockStyle.Fill;
            this.m_ledPause.Location = new System.Drawing.Point(15, 3);
            this.m_ledPause.Margin = new System.Windows.Forms.Padding(1);
            this.m_ledPause.Name = "m_ledPause";
            this.m_ledPause.Size = new System.Drawing.Size(23, 33);
            this.m_ledPause.TabIndex = 1264;
            // 
            // m_dgViewAction
            // 
            this.m_dgViewAction.AllowUserToAddRows = false;
            this.m_dgViewAction.AllowUserToDeleteRows = false;
            this.m_dgViewAction.AllowUserToResizeColumns = false;
            this.m_dgViewAction.AllowUserToResizeRows = false;
            this.m_dgViewAction.BackgroundColor = System.Drawing.Color.White;
            this.m_dgViewAction.CellBorderStyle = System.Windows.Forms.DataGridViewCellBorderStyle.Raised;
            this.m_dgViewAction.ColumnHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.Single;
            dataGridViewCellStyle4.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle4.BackColor = System.Drawing.Color.LightGray;
            dataGridViewCellStyle4.Font = new System.Drawing.Font("맑은 고딕", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            dataGridViewCellStyle4.ForeColor = System.Drawing.Color.Black;
            dataGridViewCellStyle4.SelectionBackColor = System.Drawing.Color.White;
            dataGridViewCellStyle4.SelectionForeColor = System.Drawing.Color.Black;
            dataGridViewCellStyle4.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.m_dgViewAction.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle4;
            this.m_dgViewAction.ColumnHeadersHeight = 30;
            this.m_dgViewAction.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            this.m_dgViewAction.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.dataGridViewTextBoxColumn3,
            this.dataGridViewTextBoxColumn2,
            this.TIMER});
            dataGridViewCellStyle5.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle5.BackColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle5.Font = new System.Drawing.Font("맑은 고딕", 10F, System.Drawing.FontStyle.Bold);
            dataGridViewCellStyle5.ForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle5.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(120)))), ((int)(((byte)(130)))), ((int)(((byte)(150)))));
            dataGridViewCellStyle5.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle5.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.m_dgViewAction.DefaultCellStyle = dataGridViewCellStyle5;
            this.m_dgViewAction.Dock = System.Windows.Forms.DockStyle.Fill;
            this.m_dgViewAction.EnableHeadersVisualStyles = false;
            this.m_dgViewAction.GridColor = System.Drawing.Color.FromArgb(((int)(((byte)(120)))), ((int)(((byte)(130)))), ((int)(((byte)(150)))));
            this.m_dgViewAction.Location = new System.Drawing.Point(0, 0);
            this.m_dgViewAction.Margin = new System.Windows.Forms.Padding(0, 0, 1, 0);
            this.m_dgViewAction.MultiSelect = false;
            this.m_dgViewAction.Name = "m_dgViewAction";
            this.m_dgViewAction.ReadOnly = true;
            this.m_dgViewAction.RowHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.None;
            dataGridViewCellStyle6.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle6.BackColor = System.Drawing.Color.White;
            dataGridViewCellStyle6.Font = new System.Drawing.Font("맑은 고딕", 11F);
            dataGridViewCellStyle6.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle6.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(120)))), ((int)(((byte)(130)))), ((int)(((byte)(150)))));
            dataGridViewCellStyle6.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle6.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.m_dgViewAction.RowHeadersDefaultCellStyle = dataGridViewCellStyle6;
            this.m_dgViewAction.RowHeadersVisible = false;
            this.m_dgViewAction.RowHeadersWidthSizeMode = System.Windows.Forms.DataGridViewRowHeadersWidthSizeMode.DisableResizing;
            this.m_dgViewAction.RowTemplate.Height = 30;
            this.m_dgViewAction.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.m_dgViewAction.Size = new System.Drawing.Size(393, 176);
            this.m_dgViewAction.TabIndex = 1287;
            this.m_dgViewAction.CellClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.Click_Action);
            // 
            // dataGridViewTextBoxColumn3
            // 
            this.dataGridViewTextBoxColumn3.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.dataGridViewTextBoxColumn3.HeaderText = "ACTION";
            this.dataGridViewTextBoxColumn3.MaxInputLength = 20;
            this.dataGridViewTextBoxColumn3.Name = "dataGridViewTextBoxColumn3";
            this.dataGridViewTextBoxColumn3.ReadOnly = true;
            this.dataGridViewTextBoxColumn3.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.dataGridViewTextBoxColumn3.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // dataGridViewTextBoxColumn2
            // 
            this.dataGridViewTextBoxColumn2.HeaderText = "ACTION STATE";
            this.dataGridViewTextBoxColumn2.Name = "dataGridViewTextBoxColumn2";
            this.dataGridViewTextBoxColumn2.ReadOnly = true;
            this.dataGridViewTextBoxColumn2.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.dataGridViewTextBoxColumn2.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.dataGridViewTextBoxColumn2.Width = 125;
            // 
            // TIMER
            // 
            this.TIMER.HeaderText = "TIME";
            this.TIMER.Name = "TIMER";
            this.TIMER.ReadOnly = true;
            this.TIMER.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.TIMER.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.TIMER.Width = 110;
            // 
            // m_groupMonitoring
            // 
            this.m_groupMonitoring.BackGroundColor = System.Drawing.Color.WhiteSmoke;
            this.m_groupMonitoring.Controls.Add(this._tableLayoutPanel_Main);
            this.m_groupMonitoring.Dock = System.Windows.Forms.DockStyle.Fill;
            this.m_groupMonitoring.EdgeBorderStroke = 2;
            this.m_groupMonitoring.EdgeRadius = 2;
            this.m_groupMonitoring.LabelFont = new System.Drawing.Font("맑은 고딕", 10F, System.Drawing.FontStyle.Bold);
            this.m_groupMonitoring.LabelGradientColorFirst = System.Drawing.Color.FromArgb(((int)(((byte)(208)))), ((int)(((byte)(208)))), ((int)(((byte)(208)))));
            this.m_groupMonitoring.LabelGradientColorSecond = System.Drawing.Color.FromArgb(((int)(((byte)(240)))), ((int)(((byte)(240)))), ((int)(((byte)(240)))));
            this.m_groupMonitoring.LabelHeight = 32;
            this.m_groupMonitoring.LabelTextColor = System.Drawing.Color.Black;
            this.m_groupMonitoring.Location = new System.Drawing.Point(0, 0);
            this.m_groupMonitoring.Margin = new System.Windows.Forms.Padding(0);
            this.m_groupMonitoring.Name = "m_groupMonitoring";
            this.m_groupMonitoring.Padding = new System.Windows.Forms.Padding(3, 20, 3, 3);
            this.m_groupMonitoring.Size = new System.Drawing.Size(1140, 900);
            this.m_groupMonitoring.TabIndex = 1371;
            this.m_groupMonitoring.TabStop = false;
            this.m_groupMonitoring.Text = "STATE MONITORING";
            this.m_groupMonitoring.TextAlign = Sys3Controls.EN_TEXTALIGN.MIDDLE_CENTER;
            this.m_groupMonitoring.ThemeIndex = 0;
            this.m_groupMonitoring.UseLabelBorder = true;
            this.m_groupMonitoring.UseTitle = true;
            // 
            // m_groupTaskState
            // 
            this.m_groupTaskState.BackGroundColor = System.Drawing.Color.WhiteSmoke;
            this.m_groupTaskState.Controls.Add(this.m_dgViewTask);
            this.m_groupTaskState.Dock = System.Windows.Forms.DockStyle.Fill;
            this.m_groupTaskState.EdgeBorderStroke = 2;
            this.m_groupTaskState.EdgeRadius = 2;
            this.m_groupTaskState.LabelFont = new System.Drawing.Font("맑은 고딕", 10F, System.Drawing.FontStyle.Bold);
            this.m_groupTaskState.LabelGradientColorFirst = System.Drawing.Color.FromArgb(((int)(((byte)(208)))), ((int)(((byte)(208)))), ((int)(((byte)(208)))));
            this.m_groupTaskState.LabelGradientColorSecond = System.Drawing.Color.FromArgb(((int)(((byte)(240)))), ((int)(((byte)(240)))), ((int)(((byte)(240)))));
            this.m_groupTaskState.LabelHeight = 32;
            this.m_groupTaskState.LabelTextColor = System.Drawing.Color.Black;
            this.m_groupTaskState.Location = new System.Drawing.Point(336, 0);
            this.m_groupTaskState.Margin = new System.Windows.Forms.Padding(0);
            this.m_groupTaskState.Name = "m_groupTaskState";
            this.m_groupTaskState.Padding = new System.Windows.Forms.Padding(5, 20, 5, 5);
            this.m_groupTaskState.Size = new System.Drawing.Size(798, 215);
            this.m_groupTaskState.TabIndex = 1372;
            this.m_groupTaskState.TabStop = false;
            this.m_groupTaskState.Text = "TASK STATE";
            this.m_groupTaskState.TextAlign = Sys3Controls.EN_TEXTALIGN.MIDDLE_CENTER;
            this.m_groupTaskState.ThemeIndex = 0;
            this.m_groupTaskState.UseLabelBorder = true;
            this.m_groupTaskState.UseTitle = true;
            // 
            // m_groupActionState
            // 
            this.m_groupActionState.BackGroundColor = System.Drawing.Color.WhiteSmoke;
            this.m_groupActionState.Controls.Add(this._tableLayoutPanel_ActionState);
            this.m_groupActionState.Dock = System.Windows.Forms.DockStyle.Fill;
            this.m_groupActionState.EdgeBorderStroke = 2;
            this.m_groupActionState.EdgeRadius = 2;
            this.m_groupActionState.LabelFont = new System.Drawing.Font("맑은 고딕", 10F, System.Drawing.FontStyle.Bold);
            this.m_groupActionState.LabelGradientColorFirst = System.Drawing.Color.FromArgb(((int)(((byte)(208)))), ((int)(((byte)(208)))), ((int)(((byte)(208)))));
            this.m_groupActionState.LabelGradientColorSecond = System.Drawing.Color.FromArgb(((int)(((byte)(240)))), ((int)(((byte)(240)))), ((int)(((byte)(240)))));
            this.m_groupActionState.LabelHeight = 32;
            this.m_groupActionState.LabelTextColor = System.Drawing.Color.Black;
            this.m_groupActionState.Location = new System.Drawing.Point(336, 215);
            this.m_groupActionState.Margin = new System.Windows.Forms.Padding(0);
            this.m_groupActionState.Name = "m_groupActionState";
            this.m_groupActionState.Padding = new System.Windows.Forms.Padding(5, 20, 5, 5);
            this.m_groupActionState.Size = new System.Drawing.Size(798, 215);
            this.m_groupActionState.TabIndex = 1373;
            this.m_groupActionState.TabStop = false;
            this.m_groupActionState.Text = "ACTION STATE";
            this.m_groupActionState.TextAlign = Sys3Controls.EN_TEXTALIGN.MIDDLE_CENTER;
            this.m_groupActionState.ThemeIndex = 0;
            this.m_groupActionState.UseLabelBorder = true;
            this.m_groupActionState.UseTitle = true;
            // 
            // sys3GroupBox1
            // 
            this.sys3GroupBox1.BackGroundColor = System.Drawing.Color.WhiteSmoke;
            this.sys3GroupBox1.Controls.Add(this.m_dgViewLinkedAction);
            this.sys3GroupBox1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.sys3GroupBox1.EdgeBorderStroke = 2;
            this.sys3GroupBox1.EdgeRadius = 2;
            this.sys3GroupBox1.LabelFont = new System.Drawing.Font("맑은 고딕", 10F, System.Drawing.FontStyle.Bold);
            this.sys3GroupBox1.LabelGradientColorFirst = System.Drawing.Color.FromArgb(((int)(((byte)(208)))), ((int)(((byte)(208)))), ((int)(((byte)(208)))));
            this.sys3GroupBox1.LabelGradientColorSecond = System.Drawing.Color.FromArgb(((int)(((byte)(240)))), ((int)(((byte)(240)))), ((int)(((byte)(240)))));
            this.sys3GroupBox1.LabelHeight = 32;
            this.sys3GroupBox1.LabelTextColor = System.Drawing.Color.Black;
            this.sys3GroupBox1.Location = new System.Drawing.Point(336, 430);
            this.sys3GroupBox1.Margin = new System.Windows.Forms.Padding(0);
            this.sys3GroupBox1.Name = "sys3GroupBox1";
            this.sys3GroupBox1.Padding = new System.Windows.Forms.Padding(5, 20, 5, 5);
            this.sys3GroupBox1.Size = new System.Drawing.Size(798, 215);
            this.sys3GroupBox1.TabIndex = 1374;
            this.sys3GroupBox1.TabStop = false;
            this.sys3GroupBox1.Text = "LINKED NODE STATE";
            this.sys3GroupBox1.TextAlign = Sys3Controls.EN_TEXTALIGN.MIDDLE_CENTER;
            this.sys3GroupBox1.ThemeIndex = 0;
            this.sys3GroupBox1.UseLabelBorder = true;
            this.sys3GroupBox1.UseTitle = true;
            // 
            // sys3GroupBox2
            // 
            this.sys3GroupBox2.BackGroundColor = System.Drawing.Color.WhiteSmoke;
            this.sys3GroupBox2.Controls.Add(this.m_dgViewDerefAction);
            this.sys3GroupBox2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.sys3GroupBox2.EdgeBorderStroke = 2;
            this.sys3GroupBox2.EdgeRadius = 2;
            this.sys3GroupBox2.LabelFont = new System.Drawing.Font("맑은 고딕", 10F, System.Drawing.FontStyle.Bold);
            this.sys3GroupBox2.LabelGradientColorFirst = System.Drawing.Color.FromArgb(((int)(((byte)(208)))), ((int)(((byte)(208)))), ((int)(((byte)(208)))));
            this.sys3GroupBox2.LabelGradientColorSecond = System.Drawing.Color.FromArgb(((int)(((byte)(240)))), ((int)(((byte)(240)))), ((int)(((byte)(240)))));
            this.sys3GroupBox2.LabelHeight = 32;
            this.sys3GroupBox2.LabelTextColor = System.Drawing.Color.Black;
            this.sys3GroupBox2.Location = new System.Drawing.Point(336, 645);
            this.sys3GroupBox2.Margin = new System.Windows.Forms.Padding(0);
            this.sys3GroupBox2.Name = "sys3GroupBox2";
            this.sys3GroupBox2.Padding = new System.Windows.Forms.Padding(5, 20, 5, 5);
            this.sys3GroupBox2.Size = new System.Drawing.Size(798, 218);
            this.sys3GroupBox2.TabIndex = 1375;
            this.sys3GroupBox2.TabStop = false;
            this.sys3GroupBox2.Text = "ACCESS NODE STATE";
            this.sys3GroupBox2.TextAlign = Sys3Controls.EN_TEXTALIGN.MIDDLE_CENTER;
            this.sys3GroupBox2.ThemeIndex = 0;
            this.sys3GroupBox2.UseLabelBorder = true;
            this.sys3GroupBox2.UseTitle = true;
            // 
            // m_lblTimerPause
            // 
            this.m_lblTimerPause.BackGroundColor = System.Drawing.Color.FromArgb(((int)(((byte)(200)))), ((int)(((byte)(200)))), ((int)(((byte)(220)))));
            this.m_lblTimerPause.BorderStroke = 2;
            this.m_lblTimerPause.BorderStyle = System.Windows.Forms.ButtonBorderStyle.Solid;
            this.m_lblTimerPause.Description = "";
            this.m_lblTimerPause.DisabledColor = System.Drawing.Color.DarkGray;
            this.m_lblTimerPause.Dock = System.Windows.Forms.DockStyle.Fill;
            this.m_lblTimerPause.EdgeRadius = 1;
            this.m_lblTimerPause.ImagePosition = new System.Drawing.Point(0, 0);
            this.m_lblTimerPause.ImageSize = new System.Drawing.Point(0, 0);
            this.m_lblTimerPause.LoadImage = null;
            this.m_lblTimerPause.Location = new System.Drawing.Point(40, 3);
            this.m_lblTimerPause.MainFont = new System.Drawing.Font("맑은 고딕", 9F, System.Drawing.FontStyle.Bold);
            this.m_lblTimerPause.MainFontColor = System.Drawing.Color.Black;
            this.m_lblTimerPause.Margin = new System.Windows.Forms.Padding(1);
            this.m_lblTimerPause.Name = "m_lblTimerPause";
            this.m_lblTimerPause.Size = new System.Drawing.Size(139, 33);
            this.m_lblTimerPause.SubFont = new System.Drawing.Font("맑은 고딕", 8F, System.Drawing.FontStyle.Bold);
            this.m_lblTimerPause.SubFontColor = System.Drawing.Color.DarkBlue;
            this.m_lblTimerPause.SubText = "TIMER";
            this.m_lblTimerPause.TabIndex = 1378;
            this.m_lblTimerPause.Text = "PAUSE";
            this.m_lblTimerPause.TextAlignMain = Sys3Controls.EN_TEXTALIGN.TOP_LEFT;
            this.m_lblTimerPause.TextAlignSub = Sys3Controls.EN_TEXTALIGN.BOTTOM_RIGHT;
            this.m_lblTimerPause.TextAlignUnit = Sys3Controls.EN_TEXTALIGN.MIDDLE_CENTER;
            this.m_lblTimerPause.ThemeIndex = 0;
            this.m_lblTimerPause.UnitAreaRate = 40;
            this.m_lblTimerPause.UnitFont = new System.Drawing.Font("맑은 고딕", 10F, System.Drawing.FontStyle.Bold);
            this.m_lblTimerPause.UnitFontColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.m_lblTimerPause.UnitPositionVertical = false;
            this.m_lblTimerPause.UnitText = "";
            this.m_lblTimerPause.UseBorder = true;
            this.m_lblTimerPause.UseEdgeRadius = false;
            this.m_lblTimerPause.UseImage = false;
            this.m_lblTimerPause.UseSubFont = true;
            this.m_lblTimerPause.UseUnitFont = false;
            // 
            // m_lblTimerInit
            // 
            this.m_lblTimerInit.BackGroundColor = System.Drawing.Color.FromArgb(((int)(((byte)(200)))), ((int)(((byte)(200)))), ((int)(((byte)(220)))));
            this.m_lblTimerInit.BorderStroke = 2;
            this.m_lblTimerInit.BorderStyle = System.Windows.Forms.ButtonBorderStyle.Solid;
            this.m_lblTimerInit.Description = "";
            this.m_lblTimerInit.DisabledColor = System.Drawing.Color.DarkGray;
            this.m_lblTimerInit.Dock = System.Windows.Forms.DockStyle.Fill;
            this.m_lblTimerInit.EdgeRadius = 1;
            this.m_lblTimerInit.ImagePosition = new System.Drawing.Point(0, 0);
            this.m_lblTimerInit.ImageSize = new System.Drawing.Point(0, 0);
            this.m_lblTimerInit.LoadImage = null;
            this.m_lblTimerInit.Location = new System.Drawing.Point(40, 38);
            this.m_lblTimerInit.MainFont = new System.Drawing.Font("맑은 고딕", 9F, System.Drawing.FontStyle.Bold);
            this.m_lblTimerInit.MainFontColor = System.Drawing.Color.Black;
            this.m_lblTimerInit.Margin = new System.Windows.Forms.Padding(1);
            this.m_lblTimerInit.Name = "m_lblTimerInit";
            this.m_lblTimerInit.Size = new System.Drawing.Size(139, 33);
            this.m_lblTimerInit.SubFont = new System.Drawing.Font("맑은 고딕", 8F, System.Drawing.FontStyle.Bold);
            this.m_lblTimerInit.SubFontColor = System.Drawing.Color.DarkBlue;
            this.m_lblTimerInit.SubText = "TIMER";
            this.m_lblTimerInit.TabIndex = 1378;
            this.m_lblTimerInit.Text = "INIT";
            this.m_lblTimerInit.TextAlignMain = Sys3Controls.EN_TEXTALIGN.TOP_LEFT;
            this.m_lblTimerInit.TextAlignSub = Sys3Controls.EN_TEXTALIGN.BOTTOM_RIGHT;
            this.m_lblTimerInit.TextAlignUnit = Sys3Controls.EN_TEXTALIGN.MIDDLE_CENTER;
            this.m_lblTimerInit.ThemeIndex = 0;
            this.m_lblTimerInit.UnitAreaRate = 40;
            this.m_lblTimerInit.UnitFont = new System.Drawing.Font("맑은 고딕", 10F, System.Drawing.FontStyle.Bold);
            this.m_lblTimerInit.UnitFontColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.m_lblTimerInit.UnitPositionVertical = false;
            this.m_lblTimerInit.UnitText = "";
            this.m_lblTimerInit.UseBorder = true;
            this.m_lblTimerInit.UseEdgeRadius = false;
            this.m_lblTimerInit.UseImage = false;
            this.m_lblTimerInit.UseSubFont = true;
            this.m_lblTimerInit.UseUnitFont = false;
            // 
            // m_lblTimerIdle
            // 
            this.m_lblTimerIdle.BackGroundColor = System.Drawing.Color.FromArgb(((int)(((byte)(200)))), ((int)(((byte)(200)))), ((int)(((byte)(220)))));
            this.m_lblTimerIdle.BorderStroke = 2;
            this.m_lblTimerIdle.BorderStyle = System.Windows.Forms.ButtonBorderStyle.Solid;
            this.m_lblTimerIdle.Description = "";
            this.m_lblTimerIdle.DisabledColor = System.Drawing.Color.DarkGray;
            this.m_lblTimerIdle.Dock = System.Windows.Forms.DockStyle.Fill;
            this.m_lblTimerIdle.EdgeRadius = 1;
            this.m_lblTimerIdle.ImagePosition = new System.Drawing.Point(0, 0);
            this.m_lblTimerIdle.ImageSize = new System.Drawing.Point(0, 0);
            this.m_lblTimerIdle.LoadImage = null;
            this.m_lblTimerIdle.Location = new System.Drawing.Point(40, 73);
            this.m_lblTimerIdle.MainFont = new System.Drawing.Font("맑은 고딕", 9F, System.Drawing.FontStyle.Bold);
            this.m_lblTimerIdle.MainFontColor = System.Drawing.Color.Black;
            this.m_lblTimerIdle.Margin = new System.Windows.Forms.Padding(1);
            this.m_lblTimerIdle.Name = "m_lblTimerIdle";
            this.m_lblTimerIdle.Size = new System.Drawing.Size(139, 33);
            this.m_lblTimerIdle.SubFont = new System.Drawing.Font("맑은 고딕", 8F, System.Drawing.FontStyle.Bold);
            this.m_lblTimerIdle.SubFontColor = System.Drawing.Color.DarkBlue;
            this.m_lblTimerIdle.SubText = "TIMER";
            this.m_lblTimerIdle.TabIndex = 1378;
            this.m_lblTimerIdle.Text = "IDLE";
            this.m_lblTimerIdle.TextAlignMain = Sys3Controls.EN_TEXTALIGN.TOP_LEFT;
            this.m_lblTimerIdle.TextAlignSub = Sys3Controls.EN_TEXTALIGN.BOTTOM_RIGHT;
            this.m_lblTimerIdle.TextAlignUnit = Sys3Controls.EN_TEXTALIGN.MIDDLE_CENTER;
            this.m_lblTimerIdle.ThemeIndex = 0;
            this.m_lblTimerIdle.UnitAreaRate = 40;
            this.m_lblTimerIdle.UnitFont = new System.Drawing.Font("맑은 고딕", 10F, System.Drawing.FontStyle.Bold);
            this.m_lblTimerIdle.UnitFontColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.m_lblTimerIdle.UnitPositionVertical = false;
            this.m_lblTimerIdle.UnitText = "";
            this.m_lblTimerIdle.UseBorder = true;
            this.m_lblTimerIdle.UseEdgeRadius = false;
            this.m_lblTimerIdle.UseImage = false;
            this.m_lblTimerIdle.UseSubFont = true;
            this.m_lblTimerIdle.UseUnitFont = false;
            // 
            // m_lblTimerSetup
            // 
            this.m_lblTimerSetup.BackGroundColor = System.Drawing.Color.FromArgb(((int)(((byte)(200)))), ((int)(((byte)(200)))), ((int)(((byte)(220)))));
            this.m_lblTimerSetup.BorderStroke = 2;
            this.m_lblTimerSetup.BorderStyle = System.Windows.Forms.ButtonBorderStyle.Solid;
            this.m_lblTimerSetup.Description = "";
            this.m_lblTimerSetup.DisabledColor = System.Drawing.Color.DarkGray;
            this.m_lblTimerSetup.Dock = System.Windows.Forms.DockStyle.Fill;
            this.m_lblTimerSetup.EdgeRadius = 1;
            this.m_lblTimerSetup.ImagePosition = new System.Drawing.Point(0, 0);
            this.m_lblTimerSetup.ImageSize = new System.Drawing.Point(0, 0);
            this.m_lblTimerSetup.LoadImage = null;
            this.m_lblTimerSetup.Location = new System.Drawing.Point(40, 108);
            this.m_lblTimerSetup.MainFont = new System.Drawing.Font("맑은 고딕", 9F, System.Drawing.FontStyle.Bold);
            this.m_lblTimerSetup.MainFontColor = System.Drawing.Color.Black;
            this.m_lblTimerSetup.Margin = new System.Windows.Forms.Padding(1);
            this.m_lblTimerSetup.Name = "m_lblTimerSetup";
            this.m_lblTimerSetup.Size = new System.Drawing.Size(139, 33);
            this.m_lblTimerSetup.SubFont = new System.Drawing.Font("맑은 고딕", 8F, System.Drawing.FontStyle.Bold);
            this.m_lblTimerSetup.SubFontColor = System.Drawing.Color.DarkBlue;
            this.m_lblTimerSetup.SubText = "TIMER";
            this.m_lblTimerSetup.TabIndex = 1378;
            this.m_lblTimerSetup.Text = "SETUP";
            this.m_lblTimerSetup.TextAlignMain = Sys3Controls.EN_TEXTALIGN.TOP_LEFT;
            this.m_lblTimerSetup.TextAlignSub = Sys3Controls.EN_TEXTALIGN.BOTTOM_RIGHT;
            this.m_lblTimerSetup.TextAlignUnit = Sys3Controls.EN_TEXTALIGN.MIDDLE_CENTER;
            this.m_lblTimerSetup.ThemeIndex = 0;
            this.m_lblTimerSetup.UnitAreaRate = 40;
            this.m_lblTimerSetup.UnitFont = new System.Drawing.Font("맑은 고딕", 10F, System.Drawing.FontStyle.Bold);
            this.m_lblTimerSetup.UnitFontColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.m_lblTimerSetup.UnitPositionVertical = false;
            this.m_lblTimerSetup.UnitText = "";
            this.m_lblTimerSetup.UseBorder = true;
            this.m_lblTimerSetup.UseEdgeRadius = false;
            this.m_lblTimerSetup.UseImage = false;
            this.m_lblTimerSetup.UseSubFont = true;
            this.m_lblTimerSetup.UseUnitFont = false;
            // 
            // m_lblTimerRun
            // 
            this.m_lblTimerRun.BackGroundColor = System.Drawing.Color.FromArgb(((int)(((byte)(200)))), ((int)(((byte)(200)))), ((int)(((byte)(220)))));
            this.m_lblTimerRun.BorderStroke = 2;
            this.m_lblTimerRun.BorderStyle = System.Windows.Forms.ButtonBorderStyle.Solid;
            this.m_lblTimerRun.Description = "";
            this.m_lblTimerRun.DisabledColor = System.Drawing.Color.DarkGray;
            this.m_lblTimerRun.Dock = System.Windows.Forms.DockStyle.Fill;
            this.m_lblTimerRun.EdgeRadius = 1;
            this.m_lblTimerRun.ImagePosition = new System.Drawing.Point(0, 0);
            this.m_lblTimerRun.ImageSize = new System.Drawing.Point(0, 0);
            this.m_lblTimerRun.LoadImage = null;
            this.m_lblTimerRun.Location = new System.Drawing.Point(40, 143);
            this.m_lblTimerRun.MainFont = new System.Drawing.Font("맑은 고딕", 9F, System.Drawing.FontStyle.Bold);
            this.m_lblTimerRun.MainFontColor = System.Drawing.Color.Black;
            this.m_lblTimerRun.Margin = new System.Windows.Forms.Padding(1);
            this.m_lblTimerRun.Name = "m_lblTimerRun";
            this.m_lblTimerRun.Size = new System.Drawing.Size(139, 33);
            this.m_lblTimerRun.SubFont = new System.Drawing.Font("맑은 고딕", 8F, System.Drawing.FontStyle.Bold);
            this.m_lblTimerRun.SubFontColor = System.Drawing.Color.DarkBlue;
            this.m_lblTimerRun.SubText = "TIMER";
            this.m_lblTimerRun.TabIndex = 1378;
            this.m_lblTimerRun.Text = "RUN";
            this.m_lblTimerRun.TextAlignMain = Sys3Controls.EN_TEXTALIGN.TOP_LEFT;
            this.m_lblTimerRun.TextAlignSub = Sys3Controls.EN_TEXTALIGN.BOTTOM_RIGHT;
            this.m_lblTimerRun.TextAlignUnit = Sys3Controls.EN_TEXTALIGN.MIDDLE_CENTER;
            this.m_lblTimerRun.ThemeIndex = 0;
            this.m_lblTimerRun.UnitAreaRate = 40;
            this.m_lblTimerRun.UnitFont = new System.Drawing.Font("맑은 고딕", 10F, System.Drawing.FontStyle.Bold);
            this.m_lblTimerRun.UnitFontColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.m_lblTimerRun.UnitPositionVertical = false;
            this.m_lblTimerRun.UnitText = "";
            this.m_lblTimerRun.UseBorder = true;
            this.m_lblTimerRun.UseEdgeRadius = false;
            this.m_lblTimerRun.UseImage = false;
            this.m_lblTimerRun.UseSubFont = true;
            this.m_lblTimerRun.UseUnitFont = false;
            // 
            // m_lblTimerAlarm
            // 
            this.m_lblTimerAlarm.BackGroundColor = System.Drawing.Color.FromArgb(((int)(((byte)(200)))), ((int)(((byte)(200)))), ((int)(((byte)(220)))));
            this.m_lblTimerAlarm.BorderStroke = 2;
            this.m_lblTimerAlarm.BorderStyle = System.Windows.Forms.ButtonBorderStyle.Solid;
            this.m_lblTimerAlarm.Description = "";
            this.m_lblTimerAlarm.DisabledColor = System.Drawing.Color.DarkGray;
            this.m_lblTimerAlarm.Dock = System.Windows.Forms.DockStyle.Fill;
            this.m_lblTimerAlarm.EdgeRadius = 1;
            this.m_lblTimerAlarm.ImagePosition = new System.Drawing.Point(0, 0);
            this.m_lblTimerAlarm.ImageSize = new System.Drawing.Point(0, 0);
            this.m_lblTimerAlarm.LoadImage = null;
            this.m_lblTimerAlarm.Location = new System.Drawing.Point(40, 178);
            this.m_lblTimerAlarm.MainFont = new System.Drawing.Font("맑은 고딕", 9F, System.Drawing.FontStyle.Bold);
            this.m_lblTimerAlarm.MainFontColor = System.Drawing.Color.Black;
            this.m_lblTimerAlarm.Margin = new System.Windows.Forms.Padding(1);
            this.m_lblTimerAlarm.Name = "m_lblTimerAlarm";
            this.m_lblTimerAlarm.Size = new System.Drawing.Size(139, 33);
            this.m_lblTimerAlarm.SubFont = new System.Drawing.Font("맑은 고딕", 8F, System.Drawing.FontStyle.Bold);
            this.m_lblTimerAlarm.SubFontColor = System.Drawing.Color.DarkBlue;
            this.m_lblTimerAlarm.SubText = "TIMER";
            this.m_lblTimerAlarm.TabIndex = 1378;
            this.m_lblTimerAlarm.Text = "ALARM";
            this.m_lblTimerAlarm.TextAlignMain = Sys3Controls.EN_TEXTALIGN.TOP_LEFT;
            this.m_lblTimerAlarm.TextAlignSub = Sys3Controls.EN_TEXTALIGN.BOTTOM_RIGHT;
            this.m_lblTimerAlarm.TextAlignUnit = Sys3Controls.EN_TEXTALIGN.MIDDLE_CENTER;
            this.m_lblTimerAlarm.ThemeIndex = 0;
            this.m_lblTimerAlarm.UnitAreaRate = 40;
            this.m_lblTimerAlarm.UnitFont = new System.Drawing.Font("맑은 고딕", 10F, System.Drawing.FontStyle.Bold);
            this.m_lblTimerAlarm.UnitFontColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.m_lblTimerAlarm.UnitPositionVertical = false;
            this.m_lblTimerAlarm.UnitText = "";
            this.m_lblTimerAlarm.UseBorder = true;
            this.m_lblTimerAlarm.UseEdgeRadius = false;
            this.m_lblTimerAlarm.UseImage = false;
            this.m_lblTimerAlarm.UseSubFont = true;
            this.m_lblTimerAlarm.UseUnitFont = false;
            // 
            // m_labelTimerPause
            // 
            this.m_labelTimerPause.BackGroundColor = System.Drawing.Color.White;
            this.m_labelTimerPause.BorderStroke = 2;
            this.m_labelTimerPause.BorderStyle = System.Windows.Forms.ButtonBorderStyle.Solid;
            this.m_labelTimerPause.Description = "";
            this.m_labelTimerPause.DisabledColor = System.Drawing.Color.White;
            this.m_labelTimerPause.Dock = System.Windows.Forms.DockStyle.Fill;
            this.m_labelTimerPause.EdgeRadius = 1;
            this.m_labelTimerPause.Enabled = false;
            this.m_labelTimerPause.ImagePosition = new System.Drawing.Point(0, 0);
            this.m_labelTimerPause.ImageSize = new System.Drawing.Point(0, 0);
            this.m_labelTimerPause.LoadImage = null;
            this.m_labelTimerPause.Location = new System.Drawing.Point(181, 3);
            this.m_labelTimerPause.MainFont = new System.Drawing.Font("맑은 고딕", 11F, System.Drawing.FontStyle.Bold);
            this.m_labelTimerPause.MainFontColor = System.Drawing.Color.Black;
            this.m_labelTimerPause.Margin = new System.Windows.Forms.Padding(1);
            this.m_labelTimerPause.Name = "m_labelTimerPause";
            this.m_labelTimerPause.Size = new System.Drawing.Size(139, 33);
            this.m_labelTimerPause.SubFont = new System.Drawing.Font("맑은 고딕", 10F);
            this.m_labelTimerPause.SubFontColor = System.Drawing.Color.Black;
            this.m_labelTimerPause.SubText = "";
            this.m_labelTimerPause.TabIndex = 1379;
            this.m_labelTimerPause.Text = "--";
            this.m_labelTimerPause.TextAlignMain = Sys3Controls.EN_TEXTALIGN.BOTTOM_RIGHT;
            this.m_labelTimerPause.TextAlignSub = Sys3Controls.EN_TEXTALIGN.MIDDLE_CENTER;
            this.m_labelTimerPause.TextAlignUnit = Sys3Controls.EN_TEXTALIGN.MIDDLE_CENTER;
            this.m_labelTimerPause.ThemeIndex = 0;
            this.m_labelTimerPause.UnitAreaRate = 40;
            this.m_labelTimerPause.UnitFont = new System.Drawing.Font("맑은 고딕", 10F, System.Drawing.FontStyle.Bold);
            this.m_labelTimerPause.UnitFontColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.m_labelTimerPause.UnitPositionVertical = false;
            this.m_labelTimerPause.UnitText = "";
            this.m_labelTimerPause.UseBorder = true;
            this.m_labelTimerPause.UseEdgeRadius = false;
            this.m_labelTimerPause.UseImage = false;
            this.m_labelTimerPause.UseSubFont = false;
            this.m_labelTimerPause.UseUnitFont = false;
            // 
            // m_labelTimerInit
            // 
            this.m_labelTimerInit.BackGroundColor = System.Drawing.Color.White;
            this.m_labelTimerInit.BorderStroke = 2;
            this.m_labelTimerInit.BorderStyle = System.Windows.Forms.ButtonBorderStyle.Solid;
            this.m_labelTimerInit.Description = "";
            this.m_labelTimerInit.DisabledColor = System.Drawing.Color.White;
            this.m_labelTimerInit.Dock = System.Windows.Forms.DockStyle.Fill;
            this.m_labelTimerInit.EdgeRadius = 1;
            this.m_labelTimerInit.Enabled = false;
            this.m_labelTimerInit.ImagePosition = new System.Drawing.Point(0, 0);
            this.m_labelTimerInit.ImageSize = new System.Drawing.Point(0, 0);
            this.m_labelTimerInit.LoadImage = null;
            this.m_labelTimerInit.Location = new System.Drawing.Point(181, 38);
            this.m_labelTimerInit.MainFont = new System.Drawing.Font("맑은 고딕", 11F, System.Drawing.FontStyle.Bold);
            this.m_labelTimerInit.MainFontColor = System.Drawing.Color.Black;
            this.m_labelTimerInit.Margin = new System.Windows.Forms.Padding(1);
            this.m_labelTimerInit.Name = "m_labelTimerInit";
            this.m_labelTimerInit.Size = new System.Drawing.Size(139, 33);
            this.m_labelTimerInit.SubFont = new System.Drawing.Font("맑은 고딕", 10F);
            this.m_labelTimerInit.SubFontColor = System.Drawing.Color.Black;
            this.m_labelTimerInit.SubText = "";
            this.m_labelTimerInit.TabIndex = 1379;
            this.m_labelTimerInit.Text = "--";
            this.m_labelTimerInit.TextAlignMain = Sys3Controls.EN_TEXTALIGN.BOTTOM_RIGHT;
            this.m_labelTimerInit.TextAlignSub = Sys3Controls.EN_TEXTALIGN.MIDDLE_CENTER;
            this.m_labelTimerInit.TextAlignUnit = Sys3Controls.EN_TEXTALIGN.MIDDLE_CENTER;
            this.m_labelTimerInit.ThemeIndex = 0;
            this.m_labelTimerInit.UnitAreaRate = 40;
            this.m_labelTimerInit.UnitFont = new System.Drawing.Font("맑은 고딕", 10F, System.Drawing.FontStyle.Bold);
            this.m_labelTimerInit.UnitFontColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.m_labelTimerInit.UnitPositionVertical = false;
            this.m_labelTimerInit.UnitText = "";
            this.m_labelTimerInit.UseBorder = true;
            this.m_labelTimerInit.UseEdgeRadius = false;
            this.m_labelTimerInit.UseImage = false;
            this.m_labelTimerInit.UseSubFont = false;
            this.m_labelTimerInit.UseUnitFont = false;
            // 
            // m_labelTimerIdle
            // 
            this.m_labelTimerIdle.BackGroundColor = System.Drawing.Color.White;
            this.m_labelTimerIdle.BorderStroke = 2;
            this.m_labelTimerIdle.BorderStyle = System.Windows.Forms.ButtonBorderStyle.Solid;
            this.m_labelTimerIdle.Description = "";
            this.m_labelTimerIdle.DisabledColor = System.Drawing.Color.White;
            this.m_labelTimerIdle.Dock = System.Windows.Forms.DockStyle.Fill;
            this.m_labelTimerIdle.EdgeRadius = 1;
            this.m_labelTimerIdle.Enabled = false;
            this.m_labelTimerIdle.ImagePosition = new System.Drawing.Point(0, 0);
            this.m_labelTimerIdle.ImageSize = new System.Drawing.Point(0, 0);
            this.m_labelTimerIdle.LoadImage = null;
            this.m_labelTimerIdle.Location = new System.Drawing.Point(181, 73);
            this.m_labelTimerIdle.MainFont = new System.Drawing.Font("맑은 고딕", 11F, System.Drawing.FontStyle.Bold);
            this.m_labelTimerIdle.MainFontColor = System.Drawing.Color.Black;
            this.m_labelTimerIdle.Margin = new System.Windows.Forms.Padding(1);
            this.m_labelTimerIdle.Name = "m_labelTimerIdle";
            this.m_labelTimerIdle.Size = new System.Drawing.Size(139, 33);
            this.m_labelTimerIdle.SubFont = new System.Drawing.Font("맑은 고딕", 10F);
            this.m_labelTimerIdle.SubFontColor = System.Drawing.Color.Black;
            this.m_labelTimerIdle.SubText = "";
            this.m_labelTimerIdle.TabIndex = 1379;
            this.m_labelTimerIdle.Text = "--";
            this.m_labelTimerIdle.TextAlignMain = Sys3Controls.EN_TEXTALIGN.BOTTOM_RIGHT;
            this.m_labelTimerIdle.TextAlignSub = Sys3Controls.EN_TEXTALIGN.MIDDLE_CENTER;
            this.m_labelTimerIdle.TextAlignUnit = Sys3Controls.EN_TEXTALIGN.MIDDLE_CENTER;
            this.m_labelTimerIdle.ThemeIndex = 0;
            this.m_labelTimerIdle.UnitAreaRate = 40;
            this.m_labelTimerIdle.UnitFont = new System.Drawing.Font("맑은 고딕", 10F, System.Drawing.FontStyle.Bold);
            this.m_labelTimerIdle.UnitFontColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.m_labelTimerIdle.UnitPositionVertical = false;
            this.m_labelTimerIdle.UnitText = "";
            this.m_labelTimerIdle.UseBorder = true;
            this.m_labelTimerIdle.UseEdgeRadius = false;
            this.m_labelTimerIdle.UseImage = false;
            this.m_labelTimerIdle.UseSubFont = false;
            this.m_labelTimerIdle.UseUnitFont = false;
            // 
            // m_labelTimerSetup
            // 
            this.m_labelTimerSetup.BackGroundColor = System.Drawing.Color.White;
            this.m_labelTimerSetup.BorderStroke = 2;
            this.m_labelTimerSetup.BorderStyle = System.Windows.Forms.ButtonBorderStyle.Solid;
            this.m_labelTimerSetup.Description = "";
            this.m_labelTimerSetup.DisabledColor = System.Drawing.Color.White;
            this.m_labelTimerSetup.Dock = System.Windows.Forms.DockStyle.Fill;
            this.m_labelTimerSetup.EdgeRadius = 1;
            this.m_labelTimerSetup.Enabled = false;
            this.m_labelTimerSetup.ImagePosition = new System.Drawing.Point(0, 0);
            this.m_labelTimerSetup.ImageSize = new System.Drawing.Point(0, 0);
            this.m_labelTimerSetup.LoadImage = null;
            this.m_labelTimerSetup.Location = new System.Drawing.Point(181, 108);
            this.m_labelTimerSetup.MainFont = new System.Drawing.Font("맑은 고딕", 11F, System.Drawing.FontStyle.Bold);
            this.m_labelTimerSetup.MainFontColor = System.Drawing.Color.Black;
            this.m_labelTimerSetup.Margin = new System.Windows.Forms.Padding(1);
            this.m_labelTimerSetup.Name = "m_labelTimerSetup";
            this.m_labelTimerSetup.Size = new System.Drawing.Size(139, 33);
            this.m_labelTimerSetup.SubFont = new System.Drawing.Font("맑은 고딕", 10F);
            this.m_labelTimerSetup.SubFontColor = System.Drawing.Color.Black;
            this.m_labelTimerSetup.SubText = "";
            this.m_labelTimerSetup.TabIndex = 1379;
            this.m_labelTimerSetup.Text = "--";
            this.m_labelTimerSetup.TextAlignMain = Sys3Controls.EN_TEXTALIGN.BOTTOM_RIGHT;
            this.m_labelTimerSetup.TextAlignSub = Sys3Controls.EN_TEXTALIGN.MIDDLE_CENTER;
            this.m_labelTimerSetup.TextAlignUnit = Sys3Controls.EN_TEXTALIGN.MIDDLE_CENTER;
            this.m_labelTimerSetup.ThemeIndex = 0;
            this.m_labelTimerSetup.UnitAreaRate = 40;
            this.m_labelTimerSetup.UnitFont = new System.Drawing.Font("맑은 고딕", 10F, System.Drawing.FontStyle.Bold);
            this.m_labelTimerSetup.UnitFontColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.m_labelTimerSetup.UnitPositionVertical = false;
            this.m_labelTimerSetup.UnitText = "";
            this.m_labelTimerSetup.UseBorder = true;
            this.m_labelTimerSetup.UseEdgeRadius = false;
            this.m_labelTimerSetup.UseImage = false;
            this.m_labelTimerSetup.UseSubFont = false;
            this.m_labelTimerSetup.UseUnitFont = false;
            // 
            // m_labelTimerRun
            // 
            this.m_labelTimerRun.BackGroundColor = System.Drawing.Color.White;
            this.m_labelTimerRun.BorderStroke = 2;
            this.m_labelTimerRun.BorderStyle = System.Windows.Forms.ButtonBorderStyle.Solid;
            this.m_labelTimerRun.Description = "";
            this.m_labelTimerRun.DisabledColor = System.Drawing.Color.White;
            this.m_labelTimerRun.Dock = System.Windows.Forms.DockStyle.Fill;
            this.m_labelTimerRun.EdgeRadius = 1;
            this.m_labelTimerRun.Enabled = false;
            this.m_labelTimerRun.ImagePosition = new System.Drawing.Point(0, 0);
            this.m_labelTimerRun.ImageSize = new System.Drawing.Point(0, 0);
            this.m_labelTimerRun.LoadImage = null;
            this.m_labelTimerRun.Location = new System.Drawing.Point(181, 143);
            this.m_labelTimerRun.MainFont = new System.Drawing.Font("맑은 고딕", 11F, System.Drawing.FontStyle.Bold);
            this.m_labelTimerRun.MainFontColor = System.Drawing.Color.Black;
            this.m_labelTimerRun.Margin = new System.Windows.Forms.Padding(1);
            this.m_labelTimerRun.Name = "m_labelTimerRun";
            this.m_labelTimerRun.Size = new System.Drawing.Size(139, 33);
            this.m_labelTimerRun.SubFont = new System.Drawing.Font("맑은 고딕", 10F);
            this.m_labelTimerRun.SubFontColor = System.Drawing.Color.Black;
            this.m_labelTimerRun.SubText = "";
            this.m_labelTimerRun.TabIndex = 1379;
            this.m_labelTimerRun.Text = "--";
            this.m_labelTimerRun.TextAlignMain = Sys3Controls.EN_TEXTALIGN.BOTTOM_RIGHT;
            this.m_labelTimerRun.TextAlignSub = Sys3Controls.EN_TEXTALIGN.MIDDLE_CENTER;
            this.m_labelTimerRun.TextAlignUnit = Sys3Controls.EN_TEXTALIGN.MIDDLE_CENTER;
            this.m_labelTimerRun.ThemeIndex = 0;
            this.m_labelTimerRun.UnitAreaRate = 40;
            this.m_labelTimerRun.UnitFont = new System.Drawing.Font("맑은 고딕", 10F, System.Drawing.FontStyle.Bold);
            this.m_labelTimerRun.UnitFontColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.m_labelTimerRun.UnitPositionVertical = false;
            this.m_labelTimerRun.UnitText = "";
            this.m_labelTimerRun.UseBorder = true;
            this.m_labelTimerRun.UseEdgeRadius = false;
            this.m_labelTimerRun.UseImage = false;
            this.m_labelTimerRun.UseSubFont = false;
            this.m_labelTimerRun.UseUnitFont = false;
            // 
            // m_labelTimerAlarm
            // 
            this.m_labelTimerAlarm.BackGroundColor = System.Drawing.Color.White;
            this.m_labelTimerAlarm.BorderStroke = 2;
            this.m_labelTimerAlarm.BorderStyle = System.Windows.Forms.ButtonBorderStyle.Solid;
            this.m_labelTimerAlarm.Description = "";
            this.m_labelTimerAlarm.DisabledColor = System.Drawing.Color.White;
            this.m_labelTimerAlarm.Dock = System.Windows.Forms.DockStyle.Fill;
            this.m_labelTimerAlarm.EdgeRadius = 1;
            this.m_labelTimerAlarm.Enabled = false;
            this.m_labelTimerAlarm.ImagePosition = new System.Drawing.Point(0, 0);
            this.m_labelTimerAlarm.ImageSize = new System.Drawing.Point(0, 0);
            this.m_labelTimerAlarm.LoadImage = null;
            this.m_labelTimerAlarm.Location = new System.Drawing.Point(181, 178);
            this.m_labelTimerAlarm.MainFont = new System.Drawing.Font("맑은 고딕", 11F, System.Drawing.FontStyle.Bold);
            this.m_labelTimerAlarm.MainFontColor = System.Drawing.Color.Black;
            this.m_labelTimerAlarm.Margin = new System.Windows.Forms.Padding(1);
            this.m_labelTimerAlarm.Name = "m_labelTimerAlarm";
            this.m_labelTimerAlarm.Size = new System.Drawing.Size(139, 33);
            this.m_labelTimerAlarm.SubFont = new System.Drawing.Font("맑은 고딕", 10F);
            this.m_labelTimerAlarm.SubFontColor = System.Drawing.Color.Black;
            this.m_labelTimerAlarm.SubText = "";
            this.m_labelTimerAlarm.TabIndex = 1379;
            this.m_labelTimerAlarm.Text = "--";
            this.m_labelTimerAlarm.TextAlignMain = Sys3Controls.EN_TEXTALIGN.BOTTOM_RIGHT;
            this.m_labelTimerAlarm.TextAlignSub = Sys3Controls.EN_TEXTALIGN.MIDDLE_CENTER;
            this.m_labelTimerAlarm.TextAlignUnit = Sys3Controls.EN_TEXTALIGN.MIDDLE_CENTER;
            this.m_labelTimerAlarm.ThemeIndex = 0;
            this.m_labelTimerAlarm.UnitAreaRate = 40;
            this.m_labelTimerAlarm.UnitFont = new System.Drawing.Font("맑은 고딕", 10F, System.Drawing.FontStyle.Bold);
            this.m_labelTimerAlarm.UnitFontColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.m_labelTimerAlarm.UnitPositionVertical = false;
            this.m_labelTimerAlarm.UnitText = "";
            this.m_labelTimerAlarm.UseBorder = true;
            this.m_labelTimerAlarm.UseEdgeRadius = false;
            this.m_labelTimerAlarm.UseImage = false;
            this.m_labelTimerAlarm.UseSubFont = false;
            this.m_labelTimerAlarm.UseUnitFont = false;
            // 
            // m_lblMainThread
            // 
            this.m_lblMainThread.BackGroundColor = System.Drawing.Color.FromArgb(((int)(((byte)(200)))), ((int)(((byte)(200)))), ((int)(((byte)(220)))));
            this.m_lblMainThread.BorderStroke = 2;
            this.m_lblMainThread.BorderStyle = System.Windows.Forms.ButtonBorderStyle.Solid;
            this.m_lblMainThread.Description = "";
            this.m_lblMainThread.DisabledColor = System.Drawing.Color.DarkGray;
            this.m_lblMainThread.Dock = System.Windows.Forms.DockStyle.Fill;
            this.m_lblMainThread.EdgeRadius = 1;
            this.m_lblMainThread.ImagePosition = new System.Drawing.Point(0, 0);
            this.m_lblMainThread.ImageSize = new System.Drawing.Point(0, 0);
            this.m_lblMainThread.LoadImage = null;
            this.m_lblMainThread.Location = new System.Drawing.Point(4, 7);
            this.m_lblMainThread.MainFont = new System.Drawing.Font("맑은 고딕", 9F, System.Drawing.FontStyle.Bold);
            this.m_lblMainThread.MainFontColor = System.Drawing.Color.Black;
            this.m_lblMainThread.Margin = new System.Windows.Forms.Padding(1);
            this.m_lblMainThread.Name = "m_lblMainThread";
            this._tableLayoutPanel_LeftBottom.SetRowSpan(this.m_lblMainThread, 2);
            this.m_lblMainThread.Size = new System.Drawing.Size(100, 68);
            this.m_lblMainThread.SubFont = new System.Drawing.Font("맑은 고딕", 8F, System.Drawing.FontStyle.Bold);
            this.m_lblMainThread.SubFontColor = System.Drawing.Color.DarkBlue;
            this.m_lblMainThread.SubText = "THREAD";
            this.m_lblMainThread.TabIndex = 1380;
            this.m_lblMainThread.Text = "MAIN";
            this.m_lblMainThread.TextAlignMain = Sys3Controls.EN_TEXTALIGN.TOP_LEFT;
            this.m_lblMainThread.TextAlignSub = Sys3Controls.EN_TEXTALIGN.BOTTOM_RIGHT;
            this.m_lblMainThread.TextAlignUnit = Sys3Controls.EN_TEXTALIGN.MIDDLE_CENTER;
            this.m_lblMainThread.ThemeIndex = 0;
            this.m_lblMainThread.UnitAreaRate = 40;
            this.m_lblMainThread.UnitFont = new System.Drawing.Font("맑은 고딕", 10F, System.Drawing.FontStyle.Bold);
            this.m_lblMainThread.UnitFontColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.m_lblMainThread.UnitPositionVertical = false;
            this.m_lblMainThread.UnitText = "";
            this.m_lblMainThread.UseBorder = true;
            this.m_lblMainThread.UseEdgeRadius = false;
            this.m_lblMainThread.UseImage = false;
            this.m_lblMainThread.UseSubFont = true;
            this.m_lblMainThread.UseUnitFont = false;
            // 
            // m_lblObserver
            // 
            this.m_lblObserver.BackGroundColor = System.Drawing.Color.FromArgb(((int)(((byte)(200)))), ((int)(((byte)(200)))), ((int)(((byte)(220)))));
            this.m_lblObserver.BorderStroke = 2;
            this.m_lblObserver.BorderStyle = System.Windows.Forms.ButtonBorderStyle.Solid;
            this.m_lblObserver.Description = "";
            this.m_lblObserver.DisabledColor = System.Drawing.Color.DarkGray;
            this.m_lblObserver.Dock = System.Windows.Forms.DockStyle.Fill;
            this.m_lblObserver.EdgeRadius = 1;
            this.m_lblObserver.ImagePosition = new System.Drawing.Point(0, 0);
            this.m_lblObserver.ImageSize = new System.Drawing.Point(0, 0);
            this.m_lblObserver.LoadImage = null;
            this.m_lblObserver.Location = new System.Drawing.Point(4, 77);
            this.m_lblObserver.MainFont = new System.Drawing.Font("맑은 고딕", 9F, System.Drawing.FontStyle.Bold);
            this.m_lblObserver.MainFontColor = System.Drawing.Color.Black;
            this.m_lblObserver.Margin = new System.Windows.Forms.Padding(1);
            this.m_lblObserver.Name = "m_lblObserver";
            this._tableLayoutPanel_LeftBottom.SetRowSpan(this.m_lblObserver, 2);
            this.m_lblObserver.Size = new System.Drawing.Size(100, 68);
            this.m_lblObserver.SubFont = new System.Drawing.Font("맑은 고딕", 8F, System.Drawing.FontStyle.Bold);
            this.m_lblObserver.SubFontColor = System.Drawing.Color.DarkBlue;
            this.m_lblObserver.SubText = "THREAD";
            this.m_lblObserver.TabIndex = 1380;
            this.m_lblObserver.Text = "OBSERVER";
            this.m_lblObserver.TextAlignMain = Sys3Controls.EN_TEXTALIGN.TOP_LEFT;
            this.m_lblObserver.TextAlignSub = Sys3Controls.EN_TEXTALIGN.BOTTOM_RIGHT;
            this.m_lblObserver.TextAlignUnit = Sys3Controls.EN_TEXTALIGN.MIDDLE_CENTER;
            this.m_lblObserver.ThemeIndex = 0;
            this.m_lblObserver.UnitAreaRate = 40;
            this.m_lblObserver.UnitFont = new System.Drawing.Font("맑은 고딕", 10F, System.Drawing.FontStyle.Bold);
            this.m_lblObserver.UnitFontColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.m_lblObserver.UnitPositionVertical = false;
            this.m_lblObserver.UnitText = "";
            this.m_lblObserver.UseBorder = true;
            this.m_lblObserver.UseEdgeRadius = false;
            this.m_lblObserver.UseImage = false;
            this.m_lblObserver.UseSubFont = true;
            this.m_lblObserver.UseUnitFont = false;
            // 
            // m_lblFileIO
            // 
            this.m_lblFileIO.BackGroundColor = System.Drawing.Color.FromArgb(((int)(((byte)(200)))), ((int)(((byte)(200)))), ((int)(((byte)(220)))));
            this.m_lblFileIO.BorderStroke = 2;
            this.m_lblFileIO.BorderStyle = System.Windows.Forms.ButtonBorderStyle.Solid;
            this.m_lblFileIO.Description = "";
            this.m_lblFileIO.DisabledColor = System.Drawing.Color.DarkGray;
            this.m_lblFileIO.Dock = System.Windows.Forms.DockStyle.Fill;
            this.m_lblFileIO.EdgeRadius = 1;
            this.m_lblFileIO.ImagePosition = new System.Drawing.Point(0, 0);
            this.m_lblFileIO.ImageSize = new System.Drawing.Point(0, 0);
            this.m_lblFileIO.LoadImage = null;
            this.m_lblFileIO.Location = new System.Drawing.Point(4, 147);
            this.m_lblFileIO.MainFont = new System.Drawing.Font("맑은 고딕", 9F, System.Drawing.FontStyle.Bold);
            this.m_lblFileIO.MainFontColor = System.Drawing.Color.Black;
            this.m_lblFileIO.Margin = new System.Windows.Forms.Padding(1);
            this.m_lblFileIO.Name = "m_lblFileIO";
            this._tableLayoutPanel_LeftBottom.SetRowSpan(this.m_lblFileIO, 2);
            this.m_lblFileIO.Size = new System.Drawing.Size(100, 68);
            this.m_lblFileIO.SubFont = new System.Drawing.Font("맑은 고딕", 8F, System.Drawing.FontStyle.Bold);
            this.m_lblFileIO.SubFontColor = System.Drawing.Color.DarkBlue;
            this.m_lblFileIO.SubText = "THREAD";
            this.m_lblFileIO.TabIndex = 1380;
            this.m_lblFileIO.Text = "FILEIO";
            this.m_lblFileIO.TextAlignMain = Sys3Controls.EN_TEXTALIGN.TOP_LEFT;
            this.m_lblFileIO.TextAlignSub = Sys3Controls.EN_TEXTALIGN.BOTTOM_RIGHT;
            this.m_lblFileIO.TextAlignUnit = Sys3Controls.EN_TEXTALIGN.MIDDLE_CENTER;
            this.m_lblFileIO.ThemeIndex = 0;
            this.m_lblFileIO.UnitAreaRate = 40;
            this.m_lblFileIO.UnitFont = new System.Drawing.Font("맑은 고딕", 10F, System.Drawing.FontStyle.Bold);
            this.m_lblFileIO.UnitFontColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.m_lblFileIO.UnitPositionVertical = false;
            this.m_lblFileIO.UnitText = "";
            this.m_lblFileIO.UseBorder = true;
            this.m_lblFileIO.UseEdgeRadius = false;
            this.m_lblFileIO.UseImage = false;
            this.m_lblFileIO.UseSubFont = true;
            this.m_lblFileIO.UseUnitFont = false;
            // 
            // m_lblDigitalIO
            // 
            this.m_lblDigitalIO.BackGroundColor = System.Drawing.Color.FromArgb(((int)(((byte)(200)))), ((int)(((byte)(200)))), ((int)(((byte)(220)))));
            this.m_lblDigitalIO.BorderStroke = 2;
            this.m_lblDigitalIO.BorderStyle = System.Windows.Forms.ButtonBorderStyle.Solid;
            this.m_lblDigitalIO.Description = "";
            this.m_lblDigitalIO.DisabledColor = System.Drawing.Color.DarkGray;
            this.m_lblDigitalIO.Dock = System.Windows.Forms.DockStyle.Fill;
            this.m_lblDigitalIO.EdgeRadius = 1;
            this.m_lblDigitalIO.ImagePosition = new System.Drawing.Point(0, 0);
            this.m_lblDigitalIO.ImageSize = new System.Drawing.Point(0, 0);
            this.m_lblDigitalIO.LoadImage = null;
            this.m_lblDigitalIO.Location = new System.Drawing.Point(4, 217);
            this.m_lblDigitalIO.MainFont = new System.Drawing.Font("맑은 고딕", 9F, System.Drawing.FontStyle.Bold);
            this.m_lblDigitalIO.MainFontColor = System.Drawing.Color.Black;
            this.m_lblDigitalIO.Margin = new System.Windows.Forms.Padding(1);
            this.m_lblDigitalIO.Name = "m_lblDigitalIO";
            this._tableLayoutPanel_LeftBottom.SetRowSpan(this.m_lblDigitalIO, 2);
            this.m_lblDigitalIO.Size = new System.Drawing.Size(100, 68);
            this.m_lblDigitalIO.SubFont = new System.Drawing.Font("맑은 고딕", 8F, System.Drawing.FontStyle.Bold);
            this.m_lblDigitalIO.SubFontColor = System.Drawing.Color.DarkBlue;
            this.m_lblDigitalIO.SubText = "THREAD";
            this.m_lblDigitalIO.TabIndex = 1380;
            this.m_lblDigitalIO.Text = "DIGITALIO";
            this.m_lblDigitalIO.TextAlignMain = Sys3Controls.EN_TEXTALIGN.TOP_LEFT;
            this.m_lblDigitalIO.TextAlignSub = Sys3Controls.EN_TEXTALIGN.BOTTOM_RIGHT;
            this.m_lblDigitalIO.TextAlignUnit = Sys3Controls.EN_TEXTALIGN.MIDDLE_CENTER;
            this.m_lblDigitalIO.ThemeIndex = 0;
            this.m_lblDigitalIO.UnitAreaRate = 40;
            this.m_lblDigitalIO.UnitFont = new System.Drawing.Font("맑은 고딕", 10F, System.Drawing.FontStyle.Bold);
            this.m_lblDigitalIO.UnitFontColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.m_lblDigitalIO.UnitPositionVertical = false;
            this.m_lblDigitalIO.UnitText = "";
            this.m_lblDigitalIO.UseBorder = true;
            this.m_lblDigitalIO.UseEdgeRadius = false;
            this.m_lblDigitalIO.UseImage = false;
            this.m_lblDigitalIO.UseSubFont = true;
            this.m_lblDigitalIO.UseUnitFont = false;
            // 
            // m_lblAnalogIO
            // 
            this.m_lblAnalogIO.BackGroundColor = System.Drawing.Color.FromArgb(((int)(((byte)(200)))), ((int)(((byte)(200)))), ((int)(((byte)(220)))));
            this.m_lblAnalogIO.BorderStroke = 2;
            this.m_lblAnalogIO.BorderStyle = System.Windows.Forms.ButtonBorderStyle.Solid;
            this.m_lblAnalogIO.Description = "";
            this.m_lblAnalogIO.DisabledColor = System.Drawing.Color.DarkGray;
            this.m_lblAnalogIO.Dock = System.Windows.Forms.DockStyle.Fill;
            this.m_lblAnalogIO.EdgeRadius = 1;
            this.m_lblAnalogIO.ImagePosition = new System.Drawing.Point(0, 0);
            this.m_lblAnalogIO.ImageSize = new System.Drawing.Point(0, 0);
            this.m_lblAnalogIO.LoadImage = null;
            this.m_lblAnalogIO.Location = new System.Drawing.Point(4, 287);
            this.m_lblAnalogIO.MainFont = new System.Drawing.Font("맑은 고딕", 9F, System.Drawing.FontStyle.Bold);
            this.m_lblAnalogIO.MainFontColor = System.Drawing.Color.Black;
            this.m_lblAnalogIO.Margin = new System.Windows.Forms.Padding(1);
            this.m_lblAnalogIO.Name = "m_lblAnalogIO";
            this._tableLayoutPanel_LeftBottom.SetRowSpan(this.m_lblAnalogIO, 2);
            this.m_lblAnalogIO.Size = new System.Drawing.Size(100, 68);
            this.m_lblAnalogIO.SubFont = new System.Drawing.Font("맑은 고딕", 8F, System.Drawing.FontStyle.Bold);
            this.m_lblAnalogIO.SubFontColor = System.Drawing.Color.DarkBlue;
            this.m_lblAnalogIO.SubText = "THREAD";
            this.m_lblAnalogIO.TabIndex = 1380;
            this.m_lblAnalogIO.Text = "ANALOGIO";
            this.m_lblAnalogIO.TextAlignMain = Sys3Controls.EN_TEXTALIGN.TOP_LEFT;
            this.m_lblAnalogIO.TextAlignSub = Sys3Controls.EN_TEXTALIGN.BOTTOM_RIGHT;
            this.m_lblAnalogIO.TextAlignUnit = Sys3Controls.EN_TEXTALIGN.MIDDLE_CENTER;
            this.m_lblAnalogIO.ThemeIndex = 0;
            this.m_lblAnalogIO.UnitAreaRate = 40;
            this.m_lblAnalogIO.UnitFont = new System.Drawing.Font("맑은 고딕", 10F, System.Drawing.FontStyle.Bold);
            this.m_lblAnalogIO.UnitFontColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.m_lblAnalogIO.UnitPositionVertical = false;
            this.m_lblAnalogIO.UnitText = "";
            this.m_lblAnalogIO.UseBorder = true;
            this.m_lblAnalogIO.UseEdgeRadius = false;
            this.m_lblAnalogIO.UseImage = false;
            this.m_lblAnalogIO.UseSubFont = true;
            this.m_lblAnalogIO.UseUnitFont = false;
            // 
            // m_lblMotion
            // 
            this.m_lblMotion.BackGroundColor = System.Drawing.Color.FromArgb(((int)(((byte)(200)))), ((int)(((byte)(200)))), ((int)(((byte)(220)))));
            this.m_lblMotion.BorderStroke = 2;
            this.m_lblMotion.BorderStyle = System.Windows.Forms.ButtonBorderStyle.Solid;
            this.m_lblMotion.Description = "";
            this.m_lblMotion.DisabledColor = System.Drawing.Color.DarkGray;
            this.m_lblMotion.Dock = System.Windows.Forms.DockStyle.Fill;
            this.m_lblMotion.EdgeRadius = 1;
            this.m_lblMotion.ImagePosition = new System.Drawing.Point(0, 0);
            this.m_lblMotion.ImageSize = new System.Drawing.Point(0, 0);
            this.m_lblMotion.LoadImage = null;
            this.m_lblMotion.Location = new System.Drawing.Point(4, 357);
            this.m_lblMotion.MainFont = new System.Drawing.Font("맑은 고딕", 9F, System.Drawing.FontStyle.Bold);
            this.m_lblMotion.MainFontColor = System.Drawing.Color.Black;
            this.m_lblMotion.Margin = new System.Windows.Forms.Padding(1);
            this.m_lblMotion.Name = "m_lblMotion";
            this._tableLayoutPanel_LeftBottom.SetRowSpan(this.m_lblMotion, 2);
            this.m_lblMotion.Size = new System.Drawing.Size(100, 68);
            this.m_lblMotion.SubFont = new System.Drawing.Font("맑은 고딕", 8F, System.Drawing.FontStyle.Bold);
            this.m_lblMotion.SubFontColor = System.Drawing.Color.DarkBlue;
            this.m_lblMotion.SubText = "THREAD";
            this.m_lblMotion.TabIndex = 1380;
            this.m_lblMotion.Text = "MOTION";
            this.m_lblMotion.TextAlignMain = Sys3Controls.EN_TEXTALIGN.TOP_LEFT;
            this.m_lblMotion.TextAlignSub = Sys3Controls.EN_TEXTALIGN.BOTTOM_RIGHT;
            this.m_lblMotion.TextAlignUnit = Sys3Controls.EN_TEXTALIGN.MIDDLE_CENTER;
            this.m_lblMotion.ThemeIndex = 0;
            this.m_lblMotion.UnitAreaRate = 40;
            this.m_lblMotion.UnitFont = new System.Drawing.Font("맑은 고딕", 10F, System.Drawing.FontStyle.Bold);
            this.m_lblMotion.UnitFontColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.m_lblMotion.UnitPositionVertical = false;
            this.m_lblMotion.UnitText = "";
            this.m_lblMotion.UseBorder = true;
            this.m_lblMotion.UseEdgeRadius = false;
            this.m_lblMotion.UseImage = false;
            this.m_lblMotion.UseSubFont = true;
            this.m_lblMotion.UseUnitFont = false;
            // 
            // m_lblGathering
            // 
            this.m_lblGathering.BackGroundColor = System.Drawing.Color.FromArgb(((int)(((byte)(200)))), ((int)(((byte)(200)))), ((int)(((byte)(220)))));
            this.m_lblGathering.BorderStroke = 2;
            this.m_lblGathering.BorderStyle = System.Windows.Forms.ButtonBorderStyle.Solid;
            this.m_lblGathering.Description = "";
            this.m_lblGathering.DisabledColor = System.Drawing.Color.DarkGray;
            this.m_lblGathering.Dock = System.Windows.Forms.DockStyle.Fill;
            this.m_lblGathering.EdgeRadius = 1;
            this.m_lblGathering.ImagePosition = new System.Drawing.Point(0, 0);
            this.m_lblGathering.ImageSize = new System.Drawing.Point(0, 0);
            this.m_lblGathering.LoadImage = null;
            this.m_lblGathering.Location = new System.Drawing.Point(4, 427);
            this.m_lblGathering.MainFont = new System.Drawing.Font("맑은 고딕", 9F, System.Drawing.FontStyle.Bold);
            this.m_lblGathering.MainFontColor = System.Drawing.Color.Black;
            this.m_lblGathering.Margin = new System.Windows.Forms.Padding(1);
            this.m_lblGathering.Name = "m_lblGathering";
            this._tableLayoutPanel_LeftBottom.SetRowSpan(this.m_lblGathering, 2);
            this.m_lblGathering.Size = new System.Drawing.Size(100, 68);
            this.m_lblGathering.SubFont = new System.Drawing.Font("맑은 고딕", 8F, System.Drawing.FontStyle.Bold);
            this.m_lblGathering.SubFontColor = System.Drawing.Color.DarkBlue;
            this.m_lblGathering.SubText = "THREAD";
            this.m_lblGathering.TabIndex = 1380;
            this.m_lblGathering.Text = "GATHERING";
            this.m_lblGathering.TextAlignMain = Sys3Controls.EN_TEXTALIGN.TOP_LEFT;
            this.m_lblGathering.TextAlignSub = Sys3Controls.EN_TEXTALIGN.BOTTOM_RIGHT;
            this.m_lblGathering.TextAlignUnit = Sys3Controls.EN_TEXTALIGN.MIDDLE_CENTER;
            this.m_lblGathering.ThemeIndex = 0;
            this.m_lblGathering.UnitAreaRate = 40;
            this.m_lblGathering.UnitFont = new System.Drawing.Font("맑은 고딕", 10F, System.Drawing.FontStyle.Bold);
            this.m_lblGathering.UnitFontColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.m_lblGathering.UnitPositionVertical = false;
            this.m_lblGathering.UnitText = "";
            this.m_lblGathering.UseBorder = true;
            this.m_lblGathering.UseEdgeRadius = false;
            this.m_lblGathering.UseImage = false;
            this.m_lblGathering.UseSubFont = true;
            this.m_lblGathering.UseUnitFont = false;
            // 
            // m_lblCommunication
            // 
            this.m_lblCommunication.BackGroundColor = System.Drawing.Color.FromArgb(((int)(((byte)(200)))), ((int)(((byte)(200)))), ((int)(((byte)(220)))));
            this.m_lblCommunication.BorderStroke = 2;
            this.m_lblCommunication.BorderStyle = System.Windows.Forms.ButtonBorderStyle.Solid;
            this.m_lblCommunication.Description = "";
            this.m_lblCommunication.DisabledColor = System.Drawing.Color.DarkGray;
            this.m_lblCommunication.Dock = System.Windows.Forms.DockStyle.Fill;
            this.m_lblCommunication.EdgeRadius = 1;
            this.m_lblCommunication.ImagePosition = new System.Drawing.Point(0, 0);
            this.m_lblCommunication.ImageSize = new System.Drawing.Point(0, 0);
            this.m_lblCommunication.LoadImage = null;
            this.m_lblCommunication.Location = new System.Drawing.Point(4, 497);
            this.m_lblCommunication.MainFont = new System.Drawing.Font("맑은 고딕", 9F, System.Drawing.FontStyle.Bold);
            this.m_lblCommunication.MainFontColor = System.Drawing.Color.Black;
            this.m_lblCommunication.Margin = new System.Windows.Forms.Padding(1);
            this.m_lblCommunication.Name = "m_lblCommunication";
            this._tableLayoutPanel_LeftBottom.SetRowSpan(this.m_lblCommunication, 2);
            this.m_lblCommunication.Size = new System.Drawing.Size(100, 68);
            this.m_lblCommunication.SubFont = new System.Drawing.Font("맑은 고딕", 8F, System.Drawing.FontStyle.Bold);
            this.m_lblCommunication.SubFontColor = System.Drawing.Color.DarkBlue;
            this.m_lblCommunication.SubText = "THREAD";
            this.m_lblCommunication.TabIndex = 1380;
            this.m_lblCommunication.Text = "COMMUNICATION";
            this.m_lblCommunication.TextAlignMain = Sys3Controls.EN_TEXTALIGN.TOP_LEFT;
            this.m_lblCommunication.TextAlignSub = Sys3Controls.EN_TEXTALIGN.BOTTOM_RIGHT;
            this.m_lblCommunication.TextAlignUnit = Sys3Controls.EN_TEXTALIGN.MIDDLE_CENTER;
            this.m_lblCommunication.ThemeIndex = 0;
            this.m_lblCommunication.UnitAreaRate = 40;
            this.m_lblCommunication.UnitFont = new System.Drawing.Font("맑은 고딕", 10F, System.Drawing.FontStyle.Bold);
            this.m_lblCommunication.UnitFontColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.m_lblCommunication.UnitPositionVertical = false;
            this.m_lblCommunication.UnitText = "";
            this.m_lblCommunication.UseBorder = true;
            this.m_lblCommunication.UseEdgeRadius = false;
            this.m_lblCommunication.UseImage = false;
            this.m_lblCommunication.UseSubFont = true;
            this.m_lblCommunication.UseUnitFont = false;
            // 
            // m_lblEtcInstance
            // 
            this.m_lblEtcInstance.BackGroundColor = System.Drawing.Color.FromArgb(((int)(((byte)(200)))), ((int)(((byte)(200)))), ((int)(((byte)(220)))));
            this.m_lblEtcInstance.BorderStroke = 2;
            this.m_lblEtcInstance.BorderStyle = System.Windows.Forms.ButtonBorderStyle.Solid;
            this.m_lblEtcInstance.Description = "";
            this.m_lblEtcInstance.DisabledColor = System.Drawing.Color.DarkGray;
            this.m_lblEtcInstance.Dock = System.Windows.Forms.DockStyle.Fill;
            this.m_lblEtcInstance.EdgeRadius = 1;
            this.m_lblEtcInstance.ImagePosition = new System.Drawing.Point(0, 0);
            this.m_lblEtcInstance.ImageSize = new System.Drawing.Point(0, 0);
            this.m_lblEtcInstance.LoadImage = null;
            this.m_lblEtcInstance.Location = new System.Drawing.Point(4, 567);
            this.m_lblEtcInstance.MainFont = new System.Drawing.Font("맑은 고딕", 9F, System.Drawing.FontStyle.Bold);
            this.m_lblEtcInstance.MainFontColor = System.Drawing.Color.Black;
            this.m_lblEtcInstance.Margin = new System.Windows.Forms.Padding(1);
            this.m_lblEtcInstance.Name = "m_lblEtcInstance";
            this._tableLayoutPanel_LeftBottom.SetRowSpan(this.m_lblEtcInstance, 2);
            this.m_lblEtcInstance.Size = new System.Drawing.Size(100, 68);
            this.m_lblEtcInstance.SubFont = new System.Drawing.Font("맑은 고딕", 8F, System.Drawing.FontStyle.Bold);
            this.m_lblEtcInstance.SubFontColor = System.Drawing.Color.DarkBlue;
            this.m_lblEtcInstance.SubText = "THREAD";
            this.m_lblEtcInstance.TabIndex = 1380;
            this.m_lblEtcInstance.Text = "ETC_INSTANCE";
            this.m_lblEtcInstance.TextAlignMain = Sys3Controls.EN_TEXTALIGN.TOP_LEFT;
            this.m_lblEtcInstance.TextAlignSub = Sys3Controls.EN_TEXTALIGN.BOTTOM_RIGHT;
            this.m_lblEtcInstance.TextAlignUnit = Sys3Controls.EN_TEXTALIGN.MIDDLE_CENTER;
            this.m_lblEtcInstance.ThemeIndex = 0;
            this.m_lblEtcInstance.UnitAreaRate = 40;
            this.m_lblEtcInstance.UnitFont = new System.Drawing.Font("맑은 고딕", 10F, System.Drawing.FontStyle.Bold);
            this.m_lblEtcInstance.UnitFontColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.m_lblEtcInstance.UnitPositionVertical = false;
            this.m_lblEtcInstance.UnitText = "";
            this.m_lblEtcInstance.UseBorder = true;
            this.m_lblEtcInstance.UseEdgeRadius = false;
            this.m_lblEtcInstance.UseImage = false;
            this.m_lblEtcInstance.UseSubFont = true;
            this.m_lblEtcInstance.UseUnitFont = false;
            // 
            // m_labelMainInterval
            // 
            this.m_labelMainInterval.BackGroundColor = System.Drawing.Color.White;
            this.m_labelMainInterval.BorderStroke = 2;
            this.m_labelMainInterval.BorderStyle = System.Windows.Forms.ButtonBorderStyle.Solid;
            this.m_labelMainInterval.Description = "";
            this.m_labelMainInterval.DisabledColor = System.Drawing.Color.White;
            this.m_labelMainInterval.Dock = System.Windows.Forms.DockStyle.Fill;
            this.m_labelMainInterval.EdgeRadius = 1;
            this.m_labelMainInterval.Enabled = false;
            this.m_labelMainInterval.ImagePosition = new System.Drawing.Point(0, 0);
            this.m_labelMainInterval.ImageSize = new System.Drawing.Point(0, 0);
            this.m_labelMainInterval.LoadImage = null;
            this.m_labelMainInterval.Location = new System.Drawing.Point(106, 7);
            this.m_labelMainInterval.MainFont = new System.Drawing.Font("맑은 고딕", 10F, System.Drawing.FontStyle.Bold);
            this.m_labelMainInterval.MainFontColor = System.Drawing.Color.Black;
            this.m_labelMainInterval.Margin = new System.Windows.Forms.Padding(1);
            this.m_labelMainInterval.Name = "m_labelMainInterval";
            this.m_labelMainInterval.Size = new System.Drawing.Size(111, 33);
            this.m_labelMainInterval.SubFont = new System.Drawing.Font("맑은 고딕", 8F, System.Drawing.FontStyle.Bold);
            this.m_labelMainInterval.SubFontColor = System.Drawing.Color.Black;
            this.m_labelMainInterval.SubText = "ITV";
            this.m_labelMainInterval.TabIndex = 1381;
            this.m_labelMainInterval.Text = "0";
            this.m_labelMainInterval.TextAlignMain = Sys3Controls.EN_TEXTALIGN.BOTTOM_RIGHT;
            this.m_labelMainInterval.TextAlignSub = Sys3Controls.EN_TEXTALIGN.TOP_LEFT;
            this.m_labelMainInterval.TextAlignUnit = Sys3Controls.EN_TEXTALIGN.BOTTOM_RIGHT;
            this.m_labelMainInterval.ThemeIndex = 0;
            this.m_labelMainInterval.UnitAreaRate = 40;
            this.m_labelMainInterval.UnitFont = new System.Drawing.Font("맑은 고딕", 10F, System.Drawing.FontStyle.Bold);
            this.m_labelMainInterval.UnitFontColor = System.Drawing.Color.Black;
            this.m_labelMainInterval.UnitPositionVertical = false;
            this.m_labelMainInterval.UnitText = "ms";
            this.m_labelMainInterval.UseBorder = true;
            this.m_labelMainInterval.UseEdgeRadius = false;
            this.m_labelMainInterval.UseImage = false;
            this.m_labelMainInterval.UseSubFont = true;
            this.m_labelMainInterval.UseUnitFont = true;
            // 
            // m_labelMainMax
            // 
            this.m_labelMainMax.BackGroundColor = System.Drawing.Color.White;
            this.m_labelMainMax.BorderStroke = 2;
            this.m_labelMainMax.BorderStyle = System.Windows.Forms.ButtonBorderStyle.Solid;
            this.m_labelMainMax.Description = "";
            this.m_labelMainMax.DisabledColor = System.Drawing.Color.White;
            this.m_labelMainMax.Dock = System.Windows.Forms.DockStyle.Fill;
            this.m_labelMainMax.EdgeRadius = 1;
            this.m_labelMainMax.Enabled = false;
            this.m_labelMainMax.ImagePosition = new System.Drawing.Point(0, 0);
            this.m_labelMainMax.ImageSize = new System.Drawing.Point(0, 0);
            this.m_labelMainMax.LoadImage = null;
            this.m_labelMainMax.Location = new System.Drawing.Point(219, 42);
            this.m_labelMainMax.MainFont = new System.Drawing.Font("맑은 고딕", 10F, System.Drawing.FontStyle.Bold);
            this.m_labelMainMax.MainFontColor = System.Drawing.Color.Black;
            this.m_labelMainMax.Margin = new System.Windows.Forms.Padding(1);
            this.m_labelMainMax.Name = "m_labelMainMax";
            this.m_labelMainMax.Size = new System.Drawing.Size(111, 33);
            this.m_labelMainMax.SubFont = new System.Drawing.Font("맑은 고딕", 8F, System.Drawing.FontStyle.Bold);
            this.m_labelMainMax.SubFontColor = System.Drawing.Color.Black;
            this.m_labelMainMax.SubText = "MAX";
            this.m_labelMainMax.TabIndex = 1381;
            this.m_labelMainMax.Text = "0";
            this.m_labelMainMax.TextAlignMain = Sys3Controls.EN_TEXTALIGN.BOTTOM_RIGHT;
            this.m_labelMainMax.TextAlignSub = Sys3Controls.EN_TEXTALIGN.TOP_LEFT;
            this.m_labelMainMax.TextAlignUnit = Sys3Controls.EN_TEXTALIGN.BOTTOM_RIGHT;
            this.m_labelMainMax.ThemeIndex = 0;
            this.m_labelMainMax.UnitAreaRate = 40;
            this.m_labelMainMax.UnitFont = new System.Drawing.Font("맑은 고딕", 10F, System.Drawing.FontStyle.Bold);
            this.m_labelMainMax.UnitFontColor = System.Drawing.Color.Black;
            this.m_labelMainMax.UnitPositionVertical = false;
            this.m_labelMainMax.UnitText = "ms";
            this.m_labelMainMax.UseBorder = true;
            this.m_labelMainMax.UseEdgeRadius = false;
            this.m_labelMainMax.UseImage = false;
            this.m_labelMainMax.UseSubFont = true;
            this.m_labelMainMax.UseUnitFont = true;
            // 
            // m_labelMainAverage
            // 
            this.m_labelMainAverage.BackGroundColor = System.Drawing.Color.White;
            this.m_labelMainAverage.BorderStroke = 2;
            this.m_labelMainAverage.BorderStyle = System.Windows.Forms.ButtonBorderStyle.Solid;
            this.m_labelMainAverage.Description = "";
            this.m_labelMainAverage.DisabledColor = System.Drawing.Color.White;
            this.m_labelMainAverage.Dock = System.Windows.Forms.DockStyle.Fill;
            this.m_labelMainAverage.EdgeRadius = 1;
            this.m_labelMainAverage.Enabled = false;
            this.m_labelMainAverage.ImagePosition = new System.Drawing.Point(0, 0);
            this.m_labelMainAverage.ImageSize = new System.Drawing.Point(0, 0);
            this.m_labelMainAverage.LoadImage = null;
            this.m_labelMainAverage.Location = new System.Drawing.Point(106, 42);
            this.m_labelMainAverage.MainFont = new System.Drawing.Font("맑은 고딕", 10F, System.Drawing.FontStyle.Bold);
            this.m_labelMainAverage.MainFontColor = System.Drawing.Color.Black;
            this.m_labelMainAverage.Margin = new System.Windows.Forms.Padding(1);
            this.m_labelMainAverage.Name = "m_labelMainAverage";
            this.m_labelMainAverage.Size = new System.Drawing.Size(111, 33);
            this.m_labelMainAverage.SubFont = new System.Drawing.Font("맑은 고딕", 8F, System.Drawing.FontStyle.Bold);
            this.m_labelMainAverage.SubFontColor = System.Drawing.Color.Black;
            this.m_labelMainAverage.SubText = "AVG";
            this.m_labelMainAverage.TabIndex = 1381;
            this.m_labelMainAverage.Text = "0";
            this.m_labelMainAverage.TextAlignMain = Sys3Controls.EN_TEXTALIGN.BOTTOM_RIGHT;
            this.m_labelMainAverage.TextAlignSub = Sys3Controls.EN_TEXTALIGN.TOP_LEFT;
            this.m_labelMainAverage.TextAlignUnit = Sys3Controls.EN_TEXTALIGN.BOTTOM_RIGHT;
            this.m_labelMainAverage.ThemeIndex = 0;
            this.m_labelMainAverage.UnitAreaRate = 40;
            this.m_labelMainAverage.UnitFont = new System.Drawing.Font("맑은 고딕", 10F, System.Drawing.FontStyle.Bold);
            this.m_labelMainAverage.UnitFontColor = System.Drawing.Color.Black;
            this.m_labelMainAverage.UnitPositionVertical = false;
            this.m_labelMainAverage.UnitText = "ms";
            this.m_labelMainAverage.UseBorder = true;
            this.m_labelMainAverage.UseEdgeRadius = false;
            this.m_labelMainAverage.UseImage = false;
            this.m_labelMainAverage.UseSubFont = true;
            this.m_labelMainAverage.UseUnitFont = true;
            // 
            // m_labelObserverInterval
            // 
            this.m_labelObserverInterval.BackGroundColor = System.Drawing.Color.White;
            this.m_labelObserverInterval.BorderStroke = 2;
            this.m_labelObserverInterval.BorderStyle = System.Windows.Forms.ButtonBorderStyle.Solid;
            this.m_labelObserverInterval.Description = "";
            this.m_labelObserverInterval.DisabledColor = System.Drawing.Color.White;
            this.m_labelObserverInterval.Dock = System.Windows.Forms.DockStyle.Fill;
            this.m_labelObserverInterval.EdgeRadius = 1;
            this.m_labelObserverInterval.Enabled = false;
            this.m_labelObserverInterval.ImagePosition = new System.Drawing.Point(0, 0);
            this.m_labelObserverInterval.ImageSize = new System.Drawing.Point(0, 0);
            this.m_labelObserverInterval.LoadImage = null;
            this.m_labelObserverInterval.Location = new System.Drawing.Point(106, 77);
            this.m_labelObserverInterval.MainFont = new System.Drawing.Font("맑은 고딕", 10F, System.Drawing.FontStyle.Bold);
            this.m_labelObserverInterval.MainFontColor = System.Drawing.Color.Black;
            this.m_labelObserverInterval.Margin = new System.Windows.Forms.Padding(1);
            this.m_labelObserverInterval.Name = "m_labelObserverInterval";
            this.m_labelObserverInterval.Size = new System.Drawing.Size(111, 33);
            this.m_labelObserverInterval.SubFont = new System.Drawing.Font("맑은 고딕", 8F, System.Drawing.FontStyle.Bold);
            this.m_labelObserverInterval.SubFontColor = System.Drawing.Color.Black;
            this.m_labelObserverInterval.SubText = "ITV";
            this.m_labelObserverInterval.TabIndex = 1381;
            this.m_labelObserverInterval.Text = "0";
            this.m_labelObserverInterval.TextAlignMain = Sys3Controls.EN_TEXTALIGN.BOTTOM_RIGHT;
            this.m_labelObserverInterval.TextAlignSub = Sys3Controls.EN_TEXTALIGN.TOP_LEFT;
            this.m_labelObserverInterval.TextAlignUnit = Sys3Controls.EN_TEXTALIGN.BOTTOM_RIGHT;
            this.m_labelObserverInterval.ThemeIndex = 0;
            this.m_labelObserverInterval.UnitAreaRate = 40;
            this.m_labelObserverInterval.UnitFont = new System.Drawing.Font("맑은 고딕", 10F, System.Drawing.FontStyle.Bold);
            this.m_labelObserverInterval.UnitFontColor = System.Drawing.Color.Black;
            this.m_labelObserverInterval.UnitPositionVertical = false;
            this.m_labelObserverInterval.UnitText = "ms";
            this.m_labelObserverInterval.UseBorder = true;
            this.m_labelObserverInterval.UseEdgeRadius = false;
            this.m_labelObserverInterval.UseImage = false;
            this.m_labelObserverInterval.UseSubFont = true;
            this.m_labelObserverInterval.UseUnitFont = true;
            // 
            // m_labelObserverMax
            // 
            this.m_labelObserverMax.BackGroundColor = System.Drawing.Color.White;
            this.m_labelObserverMax.BorderStroke = 2;
            this.m_labelObserverMax.BorderStyle = System.Windows.Forms.ButtonBorderStyle.Solid;
            this.m_labelObserverMax.Description = "";
            this.m_labelObserverMax.DisabledColor = System.Drawing.Color.White;
            this.m_labelObserverMax.Dock = System.Windows.Forms.DockStyle.Fill;
            this.m_labelObserverMax.EdgeRadius = 1;
            this.m_labelObserverMax.Enabled = false;
            this.m_labelObserverMax.ImagePosition = new System.Drawing.Point(0, 0);
            this.m_labelObserverMax.ImageSize = new System.Drawing.Point(0, 0);
            this.m_labelObserverMax.LoadImage = null;
            this.m_labelObserverMax.Location = new System.Drawing.Point(219, 112);
            this.m_labelObserverMax.MainFont = new System.Drawing.Font("맑은 고딕", 10F, System.Drawing.FontStyle.Bold);
            this.m_labelObserverMax.MainFontColor = System.Drawing.Color.Black;
            this.m_labelObserverMax.Margin = new System.Windows.Forms.Padding(1);
            this.m_labelObserverMax.Name = "m_labelObserverMax";
            this.m_labelObserverMax.Size = new System.Drawing.Size(111, 33);
            this.m_labelObserverMax.SubFont = new System.Drawing.Font("맑은 고딕", 8F, System.Drawing.FontStyle.Bold);
            this.m_labelObserverMax.SubFontColor = System.Drawing.Color.Black;
            this.m_labelObserverMax.SubText = "MAX";
            this.m_labelObserverMax.TabIndex = 1381;
            this.m_labelObserverMax.Text = "0";
            this.m_labelObserverMax.TextAlignMain = Sys3Controls.EN_TEXTALIGN.BOTTOM_RIGHT;
            this.m_labelObserverMax.TextAlignSub = Sys3Controls.EN_TEXTALIGN.TOP_LEFT;
            this.m_labelObserverMax.TextAlignUnit = Sys3Controls.EN_TEXTALIGN.BOTTOM_RIGHT;
            this.m_labelObserverMax.ThemeIndex = 0;
            this.m_labelObserverMax.UnitAreaRate = 40;
            this.m_labelObserverMax.UnitFont = new System.Drawing.Font("맑은 고딕", 10F, System.Drawing.FontStyle.Bold);
            this.m_labelObserverMax.UnitFontColor = System.Drawing.Color.Black;
            this.m_labelObserverMax.UnitPositionVertical = false;
            this.m_labelObserverMax.UnitText = "ms";
            this.m_labelObserverMax.UseBorder = true;
            this.m_labelObserverMax.UseEdgeRadius = false;
            this.m_labelObserverMax.UseImage = false;
            this.m_labelObserverMax.UseSubFont = true;
            this.m_labelObserverMax.UseUnitFont = true;
            // 
            // m_labelObserverAverage
            // 
            this.m_labelObserverAverage.BackGroundColor = System.Drawing.Color.White;
            this.m_labelObserverAverage.BorderStroke = 2;
            this.m_labelObserverAverage.BorderStyle = System.Windows.Forms.ButtonBorderStyle.Solid;
            this.m_labelObserverAverage.Description = "";
            this.m_labelObserverAverage.DisabledColor = System.Drawing.Color.White;
            this.m_labelObserverAverage.Dock = System.Windows.Forms.DockStyle.Fill;
            this.m_labelObserverAverage.EdgeRadius = 1;
            this.m_labelObserverAverage.Enabled = false;
            this.m_labelObserverAverage.ImagePosition = new System.Drawing.Point(0, 0);
            this.m_labelObserverAverage.ImageSize = new System.Drawing.Point(0, 0);
            this.m_labelObserverAverage.LoadImage = null;
            this.m_labelObserverAverage.Location = new System.Drawing.Point(106, 112);
            this.m_labelObserverAverage.MainFont = new System.Drawing.Font("맑은 고딕", 10F, System.Drawing.FontStyle.Bold);
            this.m_labelObserverAverage.MainFontColor = System.Drawing.Color.Black;
            this.m_labelObserverAverage.Margin = new System.Windows.Forms.Padding(1);
            this.m_labelObserverAverage.Name = "m_labelObserverAverage";
            this.m_labelObserverAverage.Size = new System.Drawing.Size(111, 33);
            this.m_labelObserverAverage.SubFont = new System.Drawing.Font("맑은 고딕", 8F, System.Drawing.FontStyle.Bold);
            this.m_labelObserverAverage.SubFontColor = System.Drawing.Color.Black;
            this.m_labelObserverAverage.SubText = "AVG";
            this.m_labelObserverAverage.TabIndex = 1381;
            this.m_labelObserverAverage.Text = "0";
            this.m_labelObserverAverage.TextAlignMain = Sys3Controls.EN_TEXTALIGN.BOTTOM_RIGHT;
            this.m_labelObserverAverage.TextAlignSub = Sys3Controls.EN_TEXTALIGN.TOP_LEFT;
            this.m_labelObserverAverage.TextAlignUnit = Sys3Controls.EN_TEXTALIGN.BOTTOM_RIGHT;
            this.m_labelObserverAverage.ThemeIndex = 0;
            this.m_labelObserverAverage.UnitAreaRate = 40;
            this.m_labelObserverAverage.UnitFont = new System.Drawing.Font("맑은 고딕", 10F, System.Drawing.FontStyle.Bold);
            this.m_labelObserverAverage.UnitFontColor = System.Drawing.Color.Black;
            this.m_labelObserverAverage.UnitPositionVertical = false;
            this.m_labelObserverAverage.UnitText = "ms";
            this.m_labelObserverAverage.UseBorder = true;
            this.m_labelObserverAverage.UseEdgeRadius = false;
            this.m_labelObserverAverage.UseImage = false;
            this.m_labelObserverAverage.UseSubFont = true;
            this.m_labelObserverAverage.UseUnitFont = true;
            // 
            // m_labelFileIOInterval
            // 
            this.m_labelFileIOInterval.BackGroundColor = System.Drawing.Color.White;
            this.m_labelFileIOInterval.BorderStroke = 2;
            this.m_labelFileIOInterval.BorderStyle = System.Windows.Forms.ButtonBorderStyle.Solid;
            this.m_labelFileIOInterval.Description = "";
            this.m_labelFileIOInterval.DisabledColor = System.Drawing.Color.White;
            this.m_labelFileIOInterval.Dock = System.Windows.Forms.DockStyle.Fill;
            this.m_labelFileIOInterval.EdgeRadius = 1;
            this.m_labelFileIOInterval.Enabled = false;
            this.m_labelFileIOInterval.ImagePosition = new System.Drawing.Point(0, 0);
            this.m_labelFileIOInterval.ImageSize = new System.Drawing.Point(0, 0);
            this.m_labelFileIOInterval.LoadImage = null;
            this.m_labelFileIOInterval.Location = new System.Drawing.Point(106, 147);
            this.m_labelFileIOInterval.MainFont = new System.Drawing.Font("맑은 고딕", 10F, System.Drawing.FontStyle.Bold);
            this.m_labelFileIOInterval.MainFontColor = System.Drawing.Color.Black;
            this.m_labelFileIOInterval.Margin = new System.Windows.Forms.Padding(1);
            this.m_labelFileIOInterval.Name = "m_labelFileIOInterval";
            this.m_labelFileIOInterval.Size = new System.Drawing.Size(111, 33);
            this.m_labelFileIOInterval.SubFont = new System.Drawing.Font("맑은 고딕", 8F, System.Drawing.FontStyle.Bold);
            this.m_labelFileIOInterval.SubFontColor = System.Drawing.Color.Black;
            this.m_labelFileIOInterval.SubText = "ITV";
            this.m_labelFileIOInterval.TabIndex = 1381;
            this.m_labelFileIOInterval.Text = "0";
            this.m_labelFileIOInterval.TextAlignMain = Sys3Controls.EN_TEXTALIGN.BOTTOM_RIGHT;
            this.m_labelFileIOInterval.TextAlignSub = Sys3Controls.EN_TEXTALIGN.TOP_LEFT;
            this.m_labelFileIOInterval.TextAlignUnit = Sys3Controls.EN_TEXTALIGN.BOTTOM_RIGHT;
            this.m_labelFileIOInterval.ThemeIndex = 0;
            this.m_labelFileIOInterval.UnitAreaRate = 40;
            this.m_labelFileIOInterval.UnitFont = new System.Drawing.Font("맑은 고딕", 10F, System.Drawing.FontStyle.Bold);
            this.m_labelFileIOInterval.UnitFontColor = System.Drawing.Color.Black;
            this.m_labelFileIOInterval.UnitPositionVertical = false;
            this.m_labelFileIOInterval.UnitText = "ms";
            this.m_labelFileIOInterval.UseBorder = true;
            this.m_labelFileIOInterval.UseEdgeRadius = false;
            this.m_labelFileIOInterval.UseImage = false;
            this.m_labelFileIOInterval.UseSubFont = true;
            this.m_labelFileIOInterval.UseUnitFont = true;
            // 
            // m_labelFileIOMax
            // 
            this.m_labelFileIOMax.BackGroundColor = System.Drawing.Color.White;
            this.m_labelFileIOMax.BorderStroke = 2;
            this.m_labelFileIOMax.BorderStyle = System.Windows.Forms.ButtonBorderStyle.Solid;
            this.m_labelFileIOMax.Description = "";
            this.m_labelFileIOMax.DisabledColor = System.Drawing.Color.White;
            this.m_labelFileIOMax.Dock = System.Windows.Forms.DockStyle.Fill;
            this.m_labelFileIOMax.EdgeRadius = 1;
            this.m_labelFileIOMax.Enabled = false;
            this.m_labelFileIOMax.ImagePosition = new System.Drawing.Point(0, 0);
            this.m_labelFileIOMax.ImageSize = new System.Drawing.Point(0, 0);
            this.m_labelFileIOMax.LoadImage = null;
            this.m_labelFileIOMax.Location = new System.Drawing.Point(219, 182);
            this.m_labelFileIOMax.MainFont = new System.Drawing.Font("맑은 고딕", 10F, System.Drawing.FontStyle.Bold);
            this.m_labelFileIOMax.MainFontColor = System.Drawing.Color.Black;
            this.m_labelFileIOMax.Margin = new System.Windows.Forms.Padding(1);
            this.m_labelFileIOMax.Name = "m_labelFileIOMax";
            this.m_labelFileIOMax.Size = new System.Drawing.Size(111, 33);
            this.m_labelFileIOMax.SubFont = new System.Drawing.Font("맑은 고딕", 8F, System.Drawing.FontStyle.Bold);
            this.m_labelFileIOMax.SubFontColor = System.Drawing.Color.Black;
            this.m_labelFileIOMax.SubText = "MAX";
            this.m_labelFileIOMax.TabIndex = 1381;
            this.m_labelFileIOMax.Text = "0";
            this.m_labelFileIOMax.TextAlignMain = Sys3Controls.EN_TEXTALIGN.BOTTOM_RIGHT;
            this.m_labelFileIOMax.TextAlignSub = Sys3Controls.EN_TEXTALIGN.TOP_LEFT;
            this.m_labelFileIOMax.TextAlignUnit = Sys3Controls.EN_TEXTALIGN.BOTTOM_RIGHT;
            this.m_labelFileIOMax.ThemeIndex = 0;
            this.m_labelFileIOMax.UnitAreaRate = 40;
            this.m_labelFileIOMax.UnitFont = new System.Drawing.Font("맑은 고딕", 10F, System.Drawing.FontStyle.Bold);
            this.m_labelFileIOMax.UnitFontColor = System.Drawing.Color.Black;
            this.m_labelFileIOMax.UnitPositionVertical = false;
            this.m_labelFileIOMax.UnitText = "ms";
            this.m_labelFileIOMax.UseBorder = true;
            this.m_labelFileIOMax.UseEdgeRadius = false;
            this.m_labelFileIOMax.UseImage = false;
            this.m_labelFileIOMax.UseSubFont = true;
            this.m_labelFileIOMax.UseUnitFont = true;
            // 
            // m_labelFileIOAverage
            // 
            this.m_labelFileIOAverage.BackGroundColor = System.Drawing.Color.White;
            this.m_labelFileIOAverage.BorderStroke = 2;
            this.m_labelFileIOAverage.BorderStyle = System.Windows.Forms.ButtonBorderStyle.Solid;
            this.m_labelFileIOAverage.Description = "";
            this.m_labelFileIOAverage.DisabledColor = System.Drawing.Color.White;
            this.m_labelFileIOAverage.Dock = System.Windows.Forms.DockStyle.Fill;
            this.m_labelFileIOAverage.EdgeRadius = 1;
            this.m_labelFileIOAverage.Enabled = false;
            this.m_labelFileIOAverage.ImagePosition = new System.Drawing.Point(0, 0);
            this.m_labelFileIOAverage.ImageSize = new System.Drawing.Point(0, 0);
            this.m_labelFileIOAverage.LoadImage = null;
            this.m_labelFileIOAverage.Location = new System.Drawing.Point(106, 182);
            this.m_labelFileIOAverage.MainFont = new System.Drawing.Font("맑은 고딕", 10F, System.Drawing.FontStyle.Bold);
            this.m_labelFileIOAverage.MainFontColor = System.Drawing.Color.Black;
            this.m_labelFileIOAverage.Margin = new System.Windows.Forms.Padding(1);
            this.m_labelFileIOAverage.Name = "m_labelFileIOAverage";
            this.m_labelFileIOAverage.Size = new System.Drawing.Size(111, 33);
            this.m_labelFileIOAverage.SubFont = new System.Drawing.Font("맑은 고딕", 8F, System.Drawing.FontStyle.Bold);
            this.m_labelFileIOAverage.SubFontColor = System.Drawing.Color.Black;
            this.m_labelFileIOAverage.SubText = "AVG";
            this.m_labelFileIOAverage.TabIndex = 1381;
            this.m_labelFileIOAverage.Text = "0";
            this.m_labelFileIOAverage.TextAlignMain = Sys3Controls.EN_TEXTALIGN.BOTTOM_RIGHT;
            this.m_labelFileIOAverage.TextAlignSub = Sys3Controls.EN_TEXTALIGN.TOP_LEFT;
            this.m_labelFileIOAverage.TextAlignUnit = Sys3Controls.EN_TEXTALIGN.BOTTOM_RIGHT;
            this.m_labelFileIOAverage.ThemeIndex = 0;
            this.m_labelFileIOAverage.UnitAreaRate = 40;
            this.m_labelFileIOAverage.UnitFont = new System.Drawing.Font("맑은 고딕", 10F, System.Drawing.FontStyle.Bold);
            this.m_labelFileIOAverage.UnitFontColor = System.Drawing.Color.Black;
            this.m_labelFileIOAverage.UnitPositionVertical = false;
            this.m_labelFileIOAverage.UnitText = "ms";
            this.m_labelFileIOAverage.UseBorder = true;
            this.m_labelFileIOAverage.UseEdgeRadius = false;
            this.m_labelFileIOAverage.UseImage = false;
            this.m_labelFileIOAverage.UseSubFont = true;
            this.m_labelFileIOAverage.UseUnitFont = true;
            // 
            // m_labelDegitalIOInterval
            // 
            this.m_labelDegitalIOInterval.BackGroundColor = System.Drawing.Color.White;
            this.m_labelDegitalIOInterval.BorderStroke = 2;
            this.m_labelDegitalIOInterval.BorderStyle = System.Windows.Forms.ButtonBorderStyle.Solid;
            this.m_labelDegitalIOInterval.Description = "";
            this.m_labelDegitalIOInterval.DisabledColor = System.Drawing.Color.White;
            this.m_labelDegitalIOInterval.Dock = System.Windows.Forms.DockStyle.Fill;
            this.m_labelDegitalIOInterval.EdgeRadius = 1;
            this.m_labelDegitalIOInterval.Enabled = false;
            this.m_labelDegitalIOInterval.ImagePosition = new System.Drawing.Point(0, 0);
            this.m_labelDegitalIOInterval.ImageSize = new System.Drawing.Point(0, 0);
            this.m_labelDegitalIOInterval.LoadImage = null;
            this.m_labelDegitalIOInterval.Location = new System.Drawing.Point(106, 217);
            this.m_labelDegitalIOInterval.MainFont = new System.Drawing.Font("맑은 고딕", 10F, System.Drawing.FontStyle.Bold);
            this.m_labelDegitalIOInterval.MainFontColor = System.Drawing.Color.Black;
            this.m_labelDegitalIOInterval.Margin = new System.Windows.Forms.Padding(1);
            this.m_labelDegitalIOInterval.Name = "m_labelDegitalIOInterval";
            this.m_labelDegitalIOInterval.Size = new System.Drawing.Size(111, 33);
            this.m_labelDegitalIOInterval.SubFont = new System.Drawing.Font("맑은 고딕", 8F, System.Drawing.FontStyle.Bold);
            this.m_labelDegitalIOInterval.SubFontColor = System.Drawing.Color.Black;
            this.m_labelDegitalIOInterval.SubText = "ITV";
            this.m_labelDegitalIOInterval.TabIndex = 1381;
            this.m_labelDegitalIOInterval.Text = "0";
            this.m_labelDegitalIOInterval.TextAlignMain = Sys3Controls.EN_TEXTALIGN.BOTTOM_RIGHT;
            this.m_labelDegitalIOInterval.TextAlignSub = Sys3Controls.EN_TEXTALIGN.TOP_LEFT;
            this.m_labelDegitalIOInterval.TextAlignUnit = Sys3Controls.EN_TEXTALIGN.BOTTOM_RIGHT;
            this.m_labelDegitalIOInterval.ThemeIndex = 0;
            this.m_labelDegitalIOInterval.UnitAreaRate = 40;
            this.m_labelDegitalIOInterval.UnitFont = new System.Drawing.Font("맑은 고딕", 10F, System.Drawing.FontStyle.Bold);
            this.m_labelDegitalIOInterval.UnitFontColor = System.Drawing.Color.Black;
            this.m_labelDegitalIOInterval.UnitPositionVertical = false;
            this.m_labelDegitalIOInterval.UnitText = "ms";
            this.m_labelDegitalIOInterval.UseBorder = true;
            this.m_labelDegitalIOInterval.UseEdgeRadius = false;
            this.m_labelDegitalIOInterval.UseImage = false;
            this.m_labelDegitalIOInterval.UseSubFont = true;
            this.m_labelDegitalIOInterval.UseUnitFont = true;
            // 
            // m_labelDegitalIOMax
            // 
            this.m_labelDegitalIOMax.BackGroundColor = System.Drawing.Color.White;
            this.m_labelDegitalIOMax.BorderStroke = 2;
            this.m_labelDegitalIOMax.BorderStyle = System.Windows.Forms.ButtonBorderStyle.Solid;
            this.m_labelDegitalIOMax.Description = "";
            this.m_labelDegitalIOMax.DisabledColor = System.Drawing.Color.White;
            this.m_labelDegitalIOMax.Dock = System.Windows.Forms.DockStyle.Fill;
            this.m_labelDegitalIOMax.EdgeRadius = 1;
            this.m_labelDegitalIOMax.Enabled = false;
            this.m_labelDegitalIOMax.ImagePosition = new System.Drawing.Point(0, 0);
            this.m_labelDegitalIOMax.ImageSize = new System.Drawing.Point(0, 0);
            this.m_labelDegitalIOMax.LoadImage = null;
            this.m_labelDegitalIOMax.Location = new System.Drawing.Point(219, 252);
            this.m_labelDegitalIOMax.MainFont = new System.Drawing.Font("맑은 고딕", 10F, System.Drawing.FontStyle.Bold);
            this.m_labelDegitalIOMax.MainFontColor = System.Drawing.Color.Black;
            this.m_labelDegitalIOMax.Margin = new System.Windows.Forms.Padding(1);
            this.m_labelDegitalIOMax.Name = "m_labelDegitalIOMax";
            this.m_labelDegitalIOMax.Size = new System.Drawing.Size(111, 33);
            this.m_labelDegitalIOMax.SubFont = new System.Drawing.Font("맑은 고딕", 8F, System.Drawing.FontStyle.Bold);
            this.m_labelDegitalIOMax.SubFontColor = System.Drawing.Color.Black;
            this.m_labelDegitalIOMax.SubText = "MAX";
            this.m_labelDegitalIOMax.TabIndex = 1381;
            this.m_labelDegitalIOMax.Text = "0";
            this.m_labelDegitalIOMax.TextAlignMain = Sys3Controls.EN_TEXTALIGN.BOTTOM_RIGHT;
            this.m_labelDegitalIOMax.TextAlignSub = Sys3Controls.EN_TEXTALIGN.TOP_LEFT;
            this.m_labelDegitalIOMax.TextAlignUnit = Sys3Controls.EN_TEXTALIGN.BOTTOM_RIGHT;
            this.m_labelDegitalIOMax.ThemeIndex = 0;
            this.m_labelDegitalIOMax.UnitAreaRate = 40;
            this.m_labelDegitalIOMax.UnitFont = new System.Drawing.Font("맑은 고딕", 10F, System.Drawing.FontStyle.Bold);
            this.m_labelDegitalIOMax.UnitFontColor = System.Drawing.Color.Black;
            this.m_labelDegitalIOMax.UnitPositionVertical = false;
            this.m_labelDegitalIOMax.UnitText = "ms";
            this.m_labelDegitalIOMax.UseBorder = true;
            this.m_labelDegitalIOMax.UseEdgeRadius = false;
            this.m_labelDegitalIOMax.UseImage = false;
            this.m_labelDegitalIOMax.UseSubFont = true;
            this.m_labelDegitalIOMax.UseUnitFont = true;
            // 
            // m_labelDegitalIOAverage
            // 
            this.m_labelDegitalIOAverage.BackGroundColor = System.Drawing.Color.White;
            this.m_labelDegitalIOAverage.BorderStroke = 2;
            this.m_labelDegitalIOAverage.BorderStyle = System.Windows.Forms.ButtonBorderStyle.Solid;
            this.m_labelDegitalIOAverage.Description = "";
            this.m_labelDegitalIOAverage.DisabledColor = System.Drawing.Color.White;
            this.m_labelDegitalIOAverage.Dock = System.Windows.Forms.DockStyle.Fill;
            this.m_labelDegitalIOAverage.EdgeRadius = 1;
            this.m_labelDegitalIOAverage.Enabled = false;
            this.m_labelDegitalIOAverage.ImagePosition = new System.Drawing.Point(0, 0);
            this.m_labelDegitalIOAverage.ImageSize = new System.Drawing.Point(0, 0);
            this.m_labelDegitalIOAverage.LoadImage = null;
            this.m_labelDegitalIOAverage.Location = new System.Drawing.Point(106, 252);
            this.m_labelDegitalIOAverage.MainFont = new System.Drawing.Font("맑은 고딕", 10F, System.Drawing.FontStyle.Bold);
            this.m_labelDegitalIOAverage.MainFontColor = System.Drawing.Color.Black;
            this.m_labelDegitalIOAverage.Margin = new System.Windows.Forms.Padding(1);
            this.m_labelDegitalIOAverage.Name = "m_labelDegitalIOAverage";
            this.m_labelDegitalIOAverage.Size = new System.Drawing.Size(111, 33);
            this.m_labelDegitalIOAverage.SubFont = new System.Drawing.Font("맑은 고딕", 8F, System.Drawing.FontStyle.Bold);
            this.m_labelDegitalIOAverage.SubFontColor = System.Drawing.Color.Black;
            this.m_labelDegitalIOAverage.SubText = "AVG";
            this.m_labelDegitalIOAverage.TabIndex = 1381;
            this.m_labelDegitalIOAverage.Text = "0";
            this.m_labelDegitalIOAverage.TextAlignMain = Sys3Controls.EN_TEXTALIGN.BOTTOM_RIGHT;
            this.m_labelDegitalIOAverage.TextAlignSub = Sys3Controls.EN_TEXTALIGN.TOP_LEFT;
            this.m_labelDegitalIOAverage.TextAlignUnit = Sys3Controls.EN_TEXTALIGN.BOTTOM_RIGHT;
            this.m_labelDegitalIOAverage.ThemeIndex = 0;
            this.m_labelDegitalIOAverage.UnitAreaRate = 40;
            this.m_labelDegitalIOAverage.UnitFont = new System.Drawing.Font("맑은 고딕", 10F, System.Drawing.FontStyle.Bold);
            this.m_labelDegitalIOAverage.UnitFontColor = System.Drawing.Color.Black;
            this.m_labelDegitalIOAverage.UnitPositionVertical = false;
            this.m_labelDegitalIOAverage.UnitText = "ms";
            this.m_labelDegitalIOAverage.UseBorder = true;
            this.m_labelDegitalIOAverage.UseEdgeRadius = false;
            this.m_labelDegitalIOAverage.UseImage = false;
            this.m_labelDegitalIOAverage.UseSubFont = true;
            this.m_labelDegitalIOAverage.UseUnitFont = true;
            // 
            // m_labelAnalogIOInterval
            // 
            this.m_labelAnalogIOInterval.BackGroundColor = System.Drawing.Color.White;
            this.m_labelAnalogIOInterval.BorderStroke = 2;
            this.m_labelAnalogIOInterval.BorderStyle = System.Windows.Forms.ButtonBorderStyle.Solid;
            this.m_labelAnalogIOInterval.Description = "";
            this.m_labelAnalogIOInterval.DisabledColor = System.Drawing.Color.White;
            this.m_labelAnalogIOInterval.Dock = System.Windows.Forms.DockStyle.Fill;
            this.m_labelAnalogIOInterval.EdgeRadius = 1;
            this.m_labelAnalogIOInterval.Enabled = false;
            this.m_labelAnalogIOInterval.ImagePosition = new System.Drawing.Point(0, 0);
            this.m_labelAnalogIOInterval.ImageSize = new System.Drawing.Point(0, 0);
            this.m_labelAnalogIOInterval.LoadImage = null;
            this.m_labelAnalogIOInterval.Location = new System.Drawing.Point(106, 287);
            this.m_labelAnalogIOInterval.MainFont = new System.Drawing.Font("맑은 고딕", 10F, System.Drawing.FontStyle.Bold);
            this.m_labelAnalogIOInterval.MainFontColor = System.Drawing.Color.Black;
            this.m_labelAnalogIOInterval.Margin = new System.Windows.Forms.Padding(1);
            this.m_labelAnalogIOInterval.Name = "m_labelAnalogIOInterval";
            this.m_labelAnalogIOInterval.Size = new System.Drawing.Size(111, 33);
            this.m_labelAnalogIOInterval.SubFont = new System.Drawing.Font("맑은 고딕", 8F, System.Drawing.FontStyle.Bold);
            this.m_labelAnalogIOInterval.SubFontColor = System.Drawing.Color.Black;
            this.m_labelAnalogIOInterval.SubText = "ITV";
            this.m_labelAnalogIOInterval.TabIndex = 1381;
            this.m_labelAnalogIOInterval.Text = "0";
            this.m_labelAnalogIOInterval.TextAlignMain = Sys3Controls.EN_TEXTALIGN.BOTTOM_RIGHT;
            this.m_labelAnalogIOInterval.TextAlignSub = Sys3Controls.EN_TEXTALIGN.TOP_LEFT;
            this.m_labelAnalogIOInterval.TextAlignUnit = Sys3Controls.EN_TEXTALIGN.BOTTOM_RIGHT;
            this.m_labelAnalogIOInterval.ThemeIndex = 0;
            this.m_labelAnalogIOInterval.UnitAreaRate = 40;
            this.m_labelAnalogIOInterval.UnitFont = new System.Drawing.Font("맑은 고딕", 10F, System.Drawing.FontStyle.Bold);
            this.m_labelAnalogIOInterval.UnitFontColor = System.Drawing.Color.Black;
            this.m_labelAnalogIOInterval.UnitPositionVertical = false;
            this.m_labelAnalogIOInterval.UnitText = "ms";
            this.m_labelAnalogIOInterval.UseBorder = true;
            this.m_labelAnalogIOInterval.UseEdgeRadius = false;
            this.m_labelAnalogIOInterval.UseImage = false;
            this.m_labelAnalogIOInterval.UseSubFont = true;
            this.m_labelAnalogIOInterval.UseUnitFont = true;
            // 
            // m_labelAnalogIOMax
            // 
            this.m_labelAnalogIOMax.BackGroundColor = System.Drawing.Color.White;
            this.m_labelAnalogIOMax.BorderStroke = 2;
            this.m_labelAnalogIOMax.BorderStyle = System.Windows.Forms.ButtonBorderStyle.Solid;
            this.m_labelAnalogIOMax.Description = "";
            this.m_labelAnalogIOMax.DisabledColor = System.Drawing.Color.White;
            this.m_labelAnalogIOMax.Dock = System.Windows.Forms.DockStyle.Fill;
            this.m_labelAnalogIOMax.EdgeRadius = 1;
            this.m_labelAnalogIOMax.Enabled = false;
            this.m_labelAnalogIOMax.ImagePosition = new System.Drawing.Point(0, 0);
            this.m_labelAnalogIOMax.ImageSize = new System.Drawing.Point(0, 0);
            this.m_labelAnalogIOMax.LoadImage = null;
            this.m_labelAnalogIOMax.Location = new System.Drawing.Point(219, 322);
            this.m_labelAnalogIOMax.MainFont = new System.Drawing.Font("맑은 고딕", 10F, System.Drawing.FontStyle.Bold);
            this.m_labelAnalogIOMax.MainFontColor = System.Drawing.Color.Black;
            this.m_labelAnalogIOMax.Margin = new System.Windows.Forms.Padding(1);
            this.m_labelAnalogIOMax.Name = "m_labelAnalogIOMax";
            this.m_labelAnalogIOMax.Size = new System.Drawing.Size(111, 33);
            this.m_labelAnalogIOMax.SubFont = new System.Drawing.Font("맑은 고딕", 8F, System.Drawing.FontStyle.Bold);
            this.m_labelAnalogIOMax.SubFontColor = System.Drawing.Color.Black;
            this.m_labelAnalogIOMax.SubText = "MAX";
            this.m_labelAnalogIOMax.TabIndex = 1381;
            this.m_labelAnalogIOMax.Text = "0";
            this.m_labelAnalogIOMax.TextAlignMain = Sys3Controls.EN_TEXTALIGN.BOTTOM_RIGHT;
            this.m_labelAnalogIOMax.TextAlignSub = Sys3Controls.EN_TEXTALIGN.TOP_LEFT;
            this.m_labelAnalogIOMax.TextAlignUnit = Sys3Controls.EN_TEXTALIGN.BOTTOM_RIGHT;
            this.m_labelAnalogIOMax.ThemeIndex = 0;
            this.m_labelAnalogIOMax.UnitAreaRate = 40;
            this.m_labelAnalogIOMax.UnitFont = new System.Drawing.Font("맑은 고딕", 10F, System.Drawing.FontStyle.Bold);
            this.m_labelAnalogIOMax.UnitFontColor = System.Drawing.Color.Black;
            this.m_labelAnalogIOMax.UnitPositionVertical = false;
            this.m_labelAnalogIOMax.UnitText = "ms";
            this.m_labelAnalogIOMax.UseBorder = true;
            this.m_labelAnalogIOMax.UseEdgeRadius = false;
            this.m_labelAnalogIOMax.UseImage = false;
            this.m_labelAnalogIOMax.UseSubFont = true;
            this.m_labelAnalogIOMax.UseUnitFont = true;
            // 
            // m_labelAnalogIOAverage
            // 
            this.m_labelAnalogIOAverage.BackGroundColor = System.Drawing.Color.White;
            this.m_labelAnalogIOAverage.BorderStroke = 2;
            this.m_labelAnalogIOAverage.BorderStyle = System.Windows.Forms.ButtonBorderStyle.Solid;
            this.m_labelAnalogIOAverage.Description = "";
            this.m_labelAnalogIOAverage.DisabledColor = System.Drawing.Color.White;
            this.m_labelAnalogIOAverage.Dock = System.Windows.Forms.DockStyle.Fill;
            this.m_labelAnalogIOAverage.EdgeRadius = 1;
            this.m_labelAnalogIOAverage.Enabled = false;
            this.m_labelAnalogIOAverage.ImagePosition = new System.Drawing.Point(0, 0);
            this.m_labelAnalogIOAverage.ImageSize = new System.Drawing.Point(0, 0);
            this.m_labelAnalogIOAverage.LoadImage = null;
            this.m_labelAnalogIOAverage.Location = new System.Drawing.Point(106, 322);
            this.m_labelAnalogIOAverage.MainFont = new System.Drawing.Font("맑은 고딕", 10F, System.Drawing.FontStyle.Bold);
            this.m_labelAnalogIOAverage.MainFontColor = System.Drawing.Color.Black;
            this.m_labelAnalogIOAverage.Margin = new System.Windows.Forms.Padding(1);
            this.m_labelAnalogIOAverage.Name = "m_labelAnalogIOAverage";
            this.m_labelAnalogIOAverage.Size = new System.Drawing.Size(111, 33);
            this.m_labelAnalogIOAverage.SubFont = new System.Drawing.Font("맑은 고딕", 8F, System.Drawing.FontStyle.Bold);
            this.m_labelAnalogIOAverage.SubFontColor = System.Drawing.Color.Black;
            this.m_labelAnalogIOAverage.SubText = "AVG";
            this.m_labelAnalogIOAverage.TabIndex = 1381;
            this.m_labelAnalogIOAverage.Text = "0";
            this.m_labelAnalogIOAverage.TextAlignMain = Sys3Controls.EN_TEXTALIGN.BOTTOM_RIGHT;
            this.m_labelAnalogIOAverage.TextAlignSub = Sys3Controls.EN_TEXTALIGN.TOP_LEFT;
            this.m_labelAnalogIOAverage.TextAlignUnit = Sys3Controls.EN_TEXTALIGN.BOTTOM_RIGHT;
            this.m_labelAnalogIOAverage.ThemeIndex = 0;
            this.m_labelAnalogIOAverage.UnitAreaRate = 40;
            this.m_labelAnalogIOAverage.UnitFont = new System.Drawing.Font("맑은 고딕", 10F, System.Drawing.FontStyle.Bold);
            this.m_labelAnalogIOAverage.UnitFontColor = System.Drawing.Color.Black;
            this.m_labelAnalogIOAverage.UnitPositionVertical = false;
            this.m_labelAnalogIOAverage.UnitText = "ms";
            this.m_labelAnalogIOAverage.UseBorder = true;
            this.m_labelAnalogIOAverage.UseEdgeRadius = false;
            this.m_labelAnalogIOAverage.UseImage = false;
            this.m_labelAnalogIOAverage.UseSubFont = true;
            this.m_labelAnalogIOAverage.UseUnitFont = true;
            // 
            // m_labelMotionInterval
            // 
            this.m_labelMotionInterval.BackGroundColor = System.Drawing.Color.White;
            this.m_labelMotionInterval.BorderStroke = 2;
            this.m_labelMotionInterval.BorderStyle = System.Windows.Forms.ButtonBorderStyle.Solid;
            this.m_labelMotionInterval.Description = "";
            this.m_labelMotionInterval.DisabledColor = System.Drawing.Color.White;
            this.m_labelMotionInterval.Dock = System.Windows.Forms.DockStyle.Fill;
            this.m_labelMotionInterval.EdgeRadius = 1;
            this.m_labelMotionInterval.Enabled = false;
            this.m_labelMotionInterval.ImagePosition = new System.Drawing.Point(0, 0);
            this.m_labelMotionInterval.ImageSize = new System.Drawing.Point(0, 0);
            this.m_labelMotionInterval.LoadImage = null;
            this.m_labelMotionInterval.Location = new System.Drawing.Point(106, 357);
            this.m_labelMotionInterval.MainFont = new System.Drawing.Font("맑은 고딕", 10F, System.Drawing.FontStyle.Bold);
            this.m_labelMotionInterval.MainFontColor = System.Drawing.Color.Black;
            this.m_labelMotionInterval.Margin = new System.Windows.Forms.Padding(1);
            this.m_labelMotionInterval.Name = "m_labelMotionInterval";
            this.m_labelMotionInterval.Size = new System.Drawing.Size(111, 33);
            this.m_labelMotionInterval.SubFont = new System.Drawing.Font("맑은 고딕", 8F, System.Drawing.FontStyle.Bold);
            this.m_labelMotionInterval.SubFontColor = System.Drawing.Color.Black;
            this.m_labelMotionInterval.SubText = "ITV";
            this.m_labelMotionInterval.TabIndex = 1381;
            this.m_labelMotionInterval.Text = "0";
            this.m_labelMotionInterval.TextAlignMain = Sys3Controls.EN_TEXTALIGN.BOTTOM_RIGHT;
            this.m_labelMotionInterval.TextAlignSub = Sys3Controls.EN_TEXTALIGN.TOP_LEFT;
            this.m_labelMotionInterval.TextAlignUnit = Sys3Controls.EN_TEXTALIGN.BOTTOM_RIGHT;
            this.m_labelMotionInterval.ThemeIndex = 0;
            this.m_labelMotionInterval.UnitAreaRate = 40;
            this.m_labelMotionInterval.UnitFont = new System.Drawing.Font("맑은 고딕", 10F, System.Drawing.FontStyle.Bold);
            this.m_labelMotionInterval.UnitFontColor = System.Drawing.Color.Black;
            this.m_labelMotionInterval.UnitPositionVertical = false;
            this.m_labelMotionInterval.UnitText = "ms";
            this.m_labelMotionInterval.UseBorder = true;
            this.m_labelMotionInterval.UseEdgeRadius = false;
            this.m_labelMotionInterval.UseImage = false;
            this.m_labelMotionInterval.UseSubFont = true;
            this.m_labelMotionInterval.UseUnitFont = true;
            // 
            // m_labelMotionMax
            // 
            this.m_labelMotionMax.BackGroundColor = System.Drawing.Color.White;
            this.m_labelMotionMax.BorderStroke = 2;
            this.m_labelMotionMax.BorderStyle = System.Windows.Forms.ButtonBorderStyle.Solid;
            this.m_labelMotionMax.Description = "";
            this.m_labelMotionMax.DisabledColor = System.Drawing.Color.White;
            this.m_labelMotionMax.Dock = System.Windows.Forms.DockStyle.Fill;
            this.m_labelMotionMax.EdgeRadius = 1;
            this.m_labelMotionMax.Enabled = false;
            this.m_labelMotionMax.ImagePosition = new System.Drawing.Point(0, 0);
            this.m_labelMotionMax.ImageSize = new System.Drawing.Point(0, 0);
            this.m_labelMotionMax.LoadImage = null;
            this.m_labelMotionMax.Location = new System.Drawing.Point(219, 392);
            this.m_labelMotionMax.MainFont = new System.Drawing.Font("맑은 고딕", 10F, System.Drawing.FontStyle.Bold);
            this.m_labelMotionMax.MainFontColor = System.Drawing.Color.Black;
            this.m_labelMotionMax.Margin = new System.Windows.Forms.Padding(1);
            this.m_labelMotionMax.Name = "m_labelMotionMax";
            this.m_labelMotionMax.Size = new System.Drawing.Size(111, 33);
            this.m_labelMotionMax.SubFont = new System.Drawing.Font("맑은 고딕", 8F, System.Drawing.FontStyle.Bold);
            this.m_labelMotionMax.SubFontColor = System.Drawing.Color.Black;
            this.m_labelMotionMax.SubText = "MAX";
            this.m_labelMotionMax.TabIndex = 1381;
            this.m_labelMotionMax.Text = "0";
            this.m_labelMotionMax.TextAlignMain = Sys3Controls.EN_TEXTALIGN.BOTTOM_RIGHT;
            this.m_labelMotionMax.TextAlignSub = Sys3Controls.EN_TEXTALIGN.TOP_LEFT;
            this.m_labelMotionMax.TextAlignUnit = Sys3Controls.EN_TEXTALIGN.BOTTOM_RIGHT;
            this.m_labelMotionMax.ThemeIndex = 0;
            this.m_labelMotionMax.UnitAreaRate = 40;
            this.m_labelMotionMax.UnitFont = new System.Drawing.Font("맑은 고딕", 10F, System.Drawing.FontStyle.Bold);
            this.m_labelMotionMax.UnitFontColor = System.Drawing.Color.Black;
            this.m_labelMotionMax.UnitPositionVertical = false;
            this.m_labelMotionMax.UnitText = "ms";
            this.m_labelMotionMax.UseBorder = true;
            this.m_labelMotionMax.UseEdgeRadius = false;
            this.m_labelMotionMax.UseImage = false;
            this.m_labelMotionMax.UseSubFont = true;
            this.m_labelMotionMax.UseUnitFont = true;
            // 
            // m_labelMotionAverage
            // 
            this.m_labelMotionAverage.BackGroundColor = System.Drawing.Color.White;
            this.m_labelMotionAverage.BorderStroke = 2;
            this.m_labelMotionAverage.BorderStyle = System.Windows.Forms.ButtonBorderStyle.Solid;
            this.m_labelMotionAverage.Description = "";
            this.m_labelMotionAverage.DisabledColor = System.Drawing.Color.White;
            this.m_labelMotionAverage.Dock = System.Windows.Forms.DockStyle.Fill;
            this.m_labelMotionAverage.EdgeRadius = 1;
            this.m_labelMotionAverage.Enabled = false;
            this.m_labelMotionAverage.ImagePosition = new System.Drawing.Point(0, 0);
            this.m_labelMotionAverage.ImageSize = new System.Drawing.Point(0, 0);
            this.m_labelMotionAverage.LoadImage = null;
            this.m_labelMotionAverage.Location = new System.Drawing.Point(106, 392);
            this.m_labelMotionAverage.MainFont = new System.Drawing.Font("맑은 고딕", 10F, System.Drawing.FontStyle.Bold);
            this.m_labelMotionAverage.MainFontColor = System.Drawing.Color.Black;
            this.m_labelMotionAverage.Margin = new System.Windows.Forms.Padding(1);
            this.m_labelMotionAverage.Name = "m_labelMotionAverage";
            this.m_labelMotionAverage.Size = new System.Drawing.Size(111, 33);
            this.m_labelMotionAverage.SubFont = new System.Drawing.Font("맑은 고딕", 8F, System.Drawing.FontStyle.Bold);
            this.m_labelMotionAverage.SubFontColor = System.Drawing.Color.Black;
            this.m_labelMotionAverage.SubText = "AVG";
            this.m_labelMotionAverage.TabIndex = 1381;
            this.m_labelMotionAverage.Text = "0";
            this.m_labelMotionAverage.TextAlignMain = Sys3Controls.EN_TEXTALIGN.BOTTOM_RIGHT;
            this.m_labelMotionAverage.TextAlignSub = Sys3Controls.EN_TEXTALIGN.TOP_LEFT;
            this.m_labelMotionAverage.TextAlignUnit = Sys3Controls.EN_TEXTALIGN.BOTTOM_RIGHT;
            this.m_labelMotionAverage.ThemeIndex = 0;
            this.m_labelMotionAverage.UnitAreaRate = 40;
            this.m_labelMotionAverage.UnitFont = new System.Drawing.Font("맑은 고딕", 10F, System.Drawing.FontStyle.Bold);
            this.m_labelMotionAverage.UnitFontColor = System.Drawing.Color.Black;
            this.m_labelMotionAverage.UnitPositionVertical = false;
            this.m_labelMotionAverage.UnitText = "ms";
            this.m_labelMotionAverage.UseBorder = true;
            this.m_labelMotionAverage.UseEdgeRadius = false;
            this.m_labelMotionAverage.UseImage = false;
            this.m_labelMotionAverage.UseSubFont = true;
            this.m_labelMotionAverage.UseUnitFont = true;
            // 
            // m_labelGatheringInterval
            // 
            this.m_labelGatheringInterval.BackGroundColor = System.Drawing.Color.White;
            this.m_labelGatheringInterval.BorderStroke = 2;
            this.m_labelGatheringInterval.BorderStyle = System.Windows.Forms.ButtonBorderStyle.Solid;
            this.m_labelGatheringInterval.Description = "";
            this.m_labelGatheringInterval.DisabledColor = System.Drawing.Color.White;
            this.m_labelGatheringInterval.Dock = System.Windows.Forms.DockStyle.Fill;
            this.m_labelGatheringInterval.EdgeRadius = 1;
            this.m_labelGatheringInterval.Enabled = false;
            this.m_labelGatheringInterval.ImagePosition = new System.Drawing.Point(0, 0);
            this.m_labelGatheringInterval.ImageSize = new System.Drawing.Point(0, 0);
            this.m_labelGatheringInterval.LoadImage = null;
            this.m_labelGatheringInterval.Location = new System.Drawing.Point(106, 427);
            this.m_labelGatheringInterval.MainFont = new System.Drawing.Font("맑은 고딕", 10F, System.Drawing.FontStyle.Bold);
            this.m_labelGatheringInterval.MainFontColor = System.Drawing.Color.Black;
            this.m_labelGatheringInterval.Margin = new System.Windows.Forms.Padding(1);
            this.m_labelGatheringInterval.Name = "m_labelGatheringInterval";
            this.m_labelGatheringInterval.Size = new System.Drawing.Size(111, 33);
            this.m_labelGatheringInterval.SubFont = new System.Drawing.Font("맑은 고딕", 8F, System.Drawing.FontStyle.Bold);
            this.m_labelGatheringInterval.SubFontColor = System.Drawing.Color.Black;
            this.m_labelGatheringInterval.SubText = "ITV";
            this.m_labelGatheringInterval.TabIndex = 1381;
            this.m_labelGatheringInterval.Text = "0";
            this.m_labelGatheringInterval.TextAlignMain = Sys3Controls.EN_TEXTALIGN.BOTTOM_RIGHT;
            this.m_labelGatheringInterval.TextAlignSub = Sys3Controls.EN_TEXTALIGN.TOP_LEFT;
            this.m_labelGatheringInterval.TextAlignUnit = Sys3Controls.EN_TEXTALIGN.BOTTOM_RIGHT;
            this.m_labelGatheringInterval.ThemeIndex = 0;
            this.m_labelGatheringInterval.UnitAreaRate = 40;
            this.m_labelGatheringInterval.UnitFont = new System.Drawing.Font("맑은 고딕", 10F, System.Drawing.FontStyle.Bold);
            this.m_labelGatheringInterval.UnitFontColor = System.Drawing.Color.Black;
            this.m_labelGatheringInterval.UnitPositionVertical = false;
            this.m_labelGatheringInterval.UnitText = "ms";
            this.m_labelGatheringInterval.UseBorder = true;
            this.m_labelGatheringInterval.UseEdgeRadius = false;
            this.m_labelGatheringInterval.UseImage = false;
            this.m_labelGatheringInterval.UseSubFont = true;
            this.m_labelGatheringInterval.UseUnitFont = true;
            // 
            // m_labelGatheringMax
            // 
            this.m_labelGatheringMax.BackGroundColor = System.Drawing.Color.White;
            this.m_labelGatheringMax.BorderStroke = 2;
            this.m_labelGatheringMax.BorderStyle = System.Windows.Forms.ButtonBorderStyle.Solid;
            this.m_labelGatheringMax.Description = "";
            this.m_labelGatheringMax.DisabledColor = System.Drawing.Color.White;
            this.m_labelGatheringMax.Dock = System.Windows.Forms.DockStyle.Fill;
            this.m_labelGatheringMax.EdgeRadius = 1;
            this.m_labelGatheringMax.Enabled = false;
            this.m_labelGatheringMax.ImagePosition = new System.Drawing.Point(0, 0);
            this.m_labelGatheringMax.ImageSize = new System.Drawing.Point(0, 0);
            this.m_labelGatheringMax.LoadImage = null;
            this.m_labelGatheringMax.Location = new System.Drawing.Point(219, 462);
            this.m_labelGatheringMax.MainFont = new System.Drawing.Font("맑은 고딕", 10F, System.Drawing.FontStyle.Bold);
            this.m_labelGatheringMax.MainFontColor = System.Drawing.Color.Black;
            this.m_labelGatheringMax.Margin = new System.Windows.Forms.Padding(1);
            this.m_labelGatheringMax.Name = "m_labelGatheringMax";
            this.m_labelGatheringMax.Size = new System.Drawing.Size(111, 33);
            this.m_labelGatheringMax.SubFont = new System.Drawing.Font("맑은 고딕", 8F, System.Drawing.FontStyle.Bold);
            this.m_labelGatheringMax.SubFontColor = System.Drawing.Color.Black;
            this.m_labelGatheringMax.SubText = "MAX";
            this.m_labelGatheringMax.TabIndex = 1381;
            this.m_labelGatheringMax.Text = "0";
            this.m_labelGatheringMax.TextAlignMain = Sys3Controls.EN_TEXTALIGN.BOTTOM_RIGHT;
            this.m_labelGatheringMax.TextAlignSub = Sys3Controls.EN_TEXTALIGN.TOP_LEFT;
            this.m_labelGatheringMax.TextAlignUnit = Sys3Controls.EN_TEXTALIGN.BOTTOM_RIGHT;
            this.m_labelGatheringMax.ThemeIndex = 0;
            this.m_labelGatheringMax.UnitAreaRate = 40;
            this.m_labelGatheringMax.UnitFont = new System.Drawing.Font("맑은 고딕", 10F, System.Drawing.FontStyle.Bold);
            this.m_labelGatheringMax.UnitFontColor = System.Drawing.Color.Black;
            this.m_labelGatheringMax.UnitPositionVertical = false;
            this.m_labelGatheringMax.UnitText = "ms";
            this.m_labelGatheringMax.UseBorder = true;
            this.m_labelGatheringMax.UseEdgeRadius = false;
            this.m_labelGatheringMax.UseImage = false;
            this.m_labelGatheringMax.UseSubFont = true;
            this.m_labelGatheringMax.UseUnitFont = true;
            // 
            // m_labelGatheringAverage
            // 
            this.m_labelGatheringAverage.BackGroundColor = System.Drawing.Color.White;
            this.m_labelGatheringAverage.BorderStroke = 2;
            this.m_labelGatheringAverage.BorderStyle = System.Windows.Forms.ButtonBorderStyle.Solid;
            this.m_labelGatheringAverage.Description = "";
            this.m_labelGatheringAverage.DisabledColor = System.Drawing.Color.White;
            this.m_labelGatheringAverage.Dock = System.Windows.Forms.DockStyle.Fill;
            this.m_labelGatheringAverage.EdgeRadius = 1;
            this.m_labelGatheringAverage.Enabled = false;
            this.m_labelGatheringAverage.ImagePosition = new System.Drawing.Point(0, 0);
            this.m_labelGatheringAverage.ImageSize = new System.Drawing.Point(0, 0);
            this.m_labelGatheringAverage.LoadImage = null;
            this.m_labelGatheringAverage.Location = new System.Drawing.Point(106, 462);
            this.m_labelGatheringAverage.MainFont = new System.Drawing.Font("맑은 고딕", 10F, System.Drawing.FontStyle.Bold);
            this.m_labelGatheringAverage.MainFontColor = System.Drawing.Color.Black;
            this.m_labelGatheringAverage.Margin = new System.Windows.Forms.Padding(1);
            this.m_labelGatheringAverage.Name = "m_labelGatheringAverage";
            this.m_labelGatheringAverage.Size = new System.Drawing.Size(111, 33);
            this.m_labelGatheringAverage.SubFont = new System.Drawing.Font("맑은 고딕", 8F, System.Drawing.FontStyle.Bold);
            this.m_labelGatheringAverage.SubFontColor = System.Drawing.Color.Black;
            this.m_labelGatheringAverage.SubText = "AVG";
            this.m_labelGatheringAverage.TabIndex = 1381;
            this.m_labelGatheringAverage.Text = "0";
            this.m_labelGatheringAverage.TextAlignMain = Sys3Controls.EN_TEXTALIGN.BOTTOM_RIGHT;
            this.m_labelGatheringAverage.TextAlignSub = Sys3Controls.EN_TEXTALIGN.TOP_LEFT;
            this.m_labelGatheringAverage.TextAlignUnit = Sys3Controls.EN_TEXTALIGN.BOTTOM_RIGHT;
            this.m_labelGatheringAverage.ThemeIndex = 0;
            this.m_labelGatheringAverage.UnitAreaRate = 40;
            this.m_labelGatheringAverage.UnitFont = new System.Drawing.Font("맑은 고딕", 10F, System.Drawing.FontStyle.Bold);
            this.m_labelGatheringAverage.UnitFontColor = System.Drawing.Color.Black;
            this.m_labelGatheringAverage.UnitPositionVertical = false;
            this.m_labelGatheringAverage.UnitText = "ms";
            this.m_labelGatheringAverage.UseBorder = true;
            this.m_labelGatheringAverage.UseEdgeRadius = false;
            this.m_labelGatheringAverage.UseImage = false;
            this.m_labelGatheringAverage.UseSubFont = true;
            this.m_labelGatheringAverage.UseUnitFont = true;
            // 
            // m_labelCommunicationInterval
            // 
            this.m_labelCommunicationInterval.BackGroundColor = System.Drawing.Color.White;
            this.m_labelCommunicationInterval.BorderStroke = 2;
            this.m_labelCommunicationInterval.BorderStyle = System.Windows.Forms.ButtonBorderStyle.Solid;
            this.m_labelCommunicationInterval.Description = "";
            this.m_labelCommunicationInterval.DisabledColor = System.Drawing.Color.White;
            this.m_labelCommunicationInterval.Dock = System.Windows.Forms.DockStyle.Fill;
            this.m_labelCommunicationInterval.EdgeRadius = 1;
            this.m_labelCommunicationInterval.Enabled = false;
            this.m_labelCommunicationInterval.ImagePosition = new System.Drawing.Point(0, 0);
            this.m_labelCommunicationInterval.ImageSize = new System.Drawing.Point(0, 0);
            this.m_labelCommunicationInterval.LoadImage = null;
            this.m_labelCommunicationInterval.Location = new System.Drawing.Point(106, 497);
            this.m_labelCommunicationInterval.MainFont = new System.Drawing.Font("맑은 고딕", 10F, System.Drawing.FontStyle.Bold);
            this.m_labelCommunicationInterval.MainFontColor = System.Drawing.Color.Black;
            this.m_labelCommunicationInterval.Margin = new System.Windows.Forms.Padding(1);
            this.m_labelCommunicationInterval.Name = "m_labelCommunicationInterval";
            this.m_labelCommunicationInterval.Size = new System.Drawing.Size(111, 33);
            this.m_labelCommunicationInterval.SubFont = new System.Drawing.Font("맑은 고딕", 8F, System.Drawing.FontStyle.Bold);
            this.m_labelCommunicationInterval.SubFontColor = System.Drawing.Color.Black;
            this.m_labelCommunicationInterval.SubText = "ITV";
            this.m_labelCommunicationInterval.TabIndex = 1381;
            this.m_labelCommunicationInterval.Text = "0";
            this.m_labelCommunicationInterval.TextAlignMain = Sys3Controls.EN_TEXTALIGN.BOTTOM_RIGHT;
            this.m_labelCommunicationInterval.TextAlignSub = Sys3Controls.EN_TEXTALIGN.TOP_LEFT;
            this.m_labelCommunicationInterval.TextAlignUnit = Sys3Controls.EN_TEXTALIGN.BOTTOM_RIGHT;
            this.m_labelCommunicationInterval.ThemeIndex = 0;
            this.m_labelCommunicationInterval.UnitAreaRate = 40;
            this.m_labelCommunicationInterval.UnitFont = new System.Drawing.Font("맑은 고딕", 10F, System.Drawing.FontStyle.Bold);
            this.m_labelCommunicationInterval.UnitFontColor = System.Drawing.Color.Black;
            this.m_labelCommunicationInterval.UnitPositionVertical = false;
            this.m_labelCommunicationInterval.UnitText = "ms";
            this.m_labelCommunicationInterval.UseBorder = true;
            this.m_labelCommunicationInterval.UseEdgeRadius = false;
            this.m_labelCommunicationInterval.UseImage = false;
            this.m_labelCommunicationInterval.UseSubFont = true;
            this.m_labelCommunicationInterval.UseUnitFont = true;
            // 
            // m_labelCommunicationMax
            // 
            this.m_labelCommunicationMax.BackGroundColor = System.Drawing.Color.White;
            this.m_labelCommunicationMax.BorderStroke = 2;
            this.m_labelCommunicationMax.BorderStyle = System.Windows.Forms.ButtonBorderStyle.Solid;
            this.m_labelCommunicationMax.Description = "";
            this.m_labelCommunicationMax.DisabledColor = System.Drawing.Color.White;
            this.m_labelCommunicationMax.Dock = System.Windows.Forms.DockStyle.Fill;
            this.m_labelCommunicationMax.EdgeRadius = 1;
            this.m_labelCommunicationMax.Enabled = false;
            this.m_labelCommunicationMax.ImagePosition = new System.Drawing.Point(0, 0);
            this.m_labelCommunicationMax.ImageSize = new System.Drawing.Point(0, 0);
            this.m_labelCommunicationMax.LoadImage = null;
            this.m_labelCommunicationMax.Location = new System.Drawing.Point(219, 532);
            this.m_labelCommunicationMax.MainFont = new System.Drawing.Font("맑은 고딕", 10F, System.Drawing.FontStyle.Bold);
            this.m_labelCommunicationMax.MainFontColor = System.Drawing.Color.Black;
            this.m_labelCommunicationMax.Margin = new System.Windows.Forms.Padding(1);
            this.m_labelCommunicationMax.Name = "m_labelCommunicationMax";
            this.m_labelCommunicationMax.Size = new System.Drawing.Size(111, 33);
            this.m_labelCommunicationMax.SubFont = new System.Drawing.Font("맑은 고딕", 8F, System.Drawing.FontStyle.Bold);
            this.m_labelCommunicationMax.SubFontColor = System.Drawing.Color.Black;
            this.m_labelCommunicationMax.SubText = "MAX";
            this.m_labelCommunicationMax.TabIndex = 1381;
            this.m_labelCommunicationMax.Text = "0";
            this.m_labelCommunicationMax.TextAlignMain = Sys3Controls.EN_TEXTALIGN.BOTTOM_RIGHT;
            this.m_labelCommunicationMax.TextAlignSub = Sys3Controls.EN_TEXTALIGN.TOP_LEFT;
            this.m_labelCommunicationMax.TextAlignUnit = Sys3Controls.EN_TEXTALIGN.BOTTOM_RIGHT;
            this.m_labelCommunicationMax.ThemeIndex = 0;
            this.m_labelCommunicationMax.UnitAreaRate = 40;
            this.m_labelCommunicationMax.UnitFont = new System.Drawing.Font("맑은 고딕", 10F, System.Drawing.FontStyle.Bold);
            this.m_labelCommunicationMax.UnitFontColor = System.Drawing.Color.Black;
            this.m_labelCommunicationMax.UnitPositionVertical = false;
            this.m_labelCommunicationMax.UnitText = "ms";
            this.m_labelCommunicationMax.UseBorder = true;
            this.m_labelCommunicationMax.UseEdgeRadius = false;
            this.m_labelCommunicationMax.UseImage = false;
            this.m_labelCommunicationMax.UseSubFont = true;
            this.m_labelCommunicationMax.UseUnitFont = true;
            // 
            // m_labelCommunicationAverage
            // 
            this.m_labelCommunicationAverage.BackGroundColor = System.Drawing.Color.White;
            this.m_labelCommunicationAverage.BorderStroke = 2;
            this.m_labelCommunicationAverage.BorderStyle = System.Windows.Forms.ButtonBorderStyle.Solid;
            this.m_labelCommunicationAverage.Description = "";
            this.m_labelCommunicationAverage.DisabledColor = System.Drawing.Color.White;
            this.m_labelCommunicationAverage.Dock = System.Windows.Forms.DockStyle.Fill;
            this.m_labelCommunicationAverage.EdgeRadius = 1;
            this.m_labelCommunicationAverage.Enabled = false;
            this.m_labelCommunicationAverage.ImagePosition = new System.Drawing.Point(0, 0);
            this.m_labelCommunicationAverage.ImageSize = new System.Drawing.Point(0, 0);
            this.m_labelCommunicationAverage.LoadImage = null;
            this.m_labelCommunicationAverage.Location = new System.Drawing.Point(106, 532);
            this.m_labelCommunicationAverage.MainFont = new System.Drawing.Font("맑은 고딕", 10F, System.Drawing.FontStyle.Bold);
            this.m_labelCommunicationAverage.MainFontColor = System.Drawing.Color.Black;
            this.m_labelCommunicationAverage.Margin = new System.Windows.Forms.Padding(1);
            this.m_labelCommunicationAverage.Name = "m_labelCommunicationAverage";
            this.m_labelCommunicationAverage.Size = new System.Drawing.Size(111, 33);
            this.m_labelCommunicationAverage.SubFont = new System.Drawing.Font("맑은 고딕", 8F, System.Drawing.FontStyle.Bold);
            this.m_labelCommunicationAverage.SubFontColor = System.Drawing.Color.Black;
            this.m_labelCommunicationAverage.SubText = "AVG";
            this.m_labelCommunicationAverage.TabIndex = 1381;
            this.m_labelCommunicationAverage.Text = "0";
            this.m_labelCommunicationAverage.TextAlignMain = Sys3Controls.EN_TEXTALIGN.BOTTOM_RIGHT;
            this.m_labelCommunicationAverage.TextAlignSub = Sys3Controls.EN_TEXTALIGN.TOP_LEFT;
            this.m_labelCommunicationAverage.TextAlignUnit = Sys3Controls.EN_TEXTALIGN.BOTTOM_RIGHT;
            this.m_labelCommunicationAverage.ThemeIndex = 0;
            this.m_labelCommunicationAverage.UnitAreaRate = 40;
            this.m_labelCommunicationAverage.UnitFont = new System.Drawing.Font("맑은 고딕", 10F, System.Drawing.FontStyle.Bold);
            this.m_labelCommunicationAverage.UnitFontColor = System.Drawing.Color.Black;
            this.m_labelCommunicationAverage.UnitPositionVertical = false;
            this.m_labelCommunicationAverage.UnitText = "ms";
            this.m_labelCommunicationAverage.UseBorder = true;
            this.m_labelCommunicationAverage.UseEdgeRadius = false;
            this.m_labelCommunicationAverage.UseImage = false;
            this.m_labelCommunicationAverage.UseSubFont = true;
            this.m_labelCommunicationAverage.UseUnitFont = true;
            // 
            // m_labelEtcInstanceInterval
            // 
            this.m_labelEtcInstanceInterval.BackGroundColor = System.Drawing.Color.White;
            this.m_labelEtcInstanceInterval.BorderStroke = 2;
            this.m_labelEtcInstanceInterval.BorderStyle = System.Windows.Forms.ButtonBorderStyle.Solid;
            this.m_labelEtcInstanceInterval.Description = "";
            this.m_labelEtcInstanceInterval.DisabledColor = System.Drawing.Color.White;
            this.m_labelEtcInstanceInterval.Dock = System.Windows.Forms.DockStyle.Fill;
            this.m_labelEtcInstanceInterval.EdgeRadius = 1;
            this.m_labelEtcInstanceInterval.Enabled = false;
            this.m_labelEtcInstanceInterval.ImagePosition = new System.Drawing.Point(0, 0);
            this.m_labelEtcInstanceInterval.ImageSize = new System.Drawing.Point(0, 0);
            this.m_labelEtcInstanceInterval.LoadImage = null;
            this.m_labelEtcInstanceInterval.Location = new System.Drawing.Point(106, 567);
            this.m_labelEtcInstanceInterval.MainFont = new System.Drawing.Font("맑은 고딕", 10F, System.Drawing.FontStyle.Bold);
            this.m_labelEtcInstanceInterval.MainFontColor = System.Drawing.Color.Black;
            this.m_labelEtcInstanceInterval.Margin = new System.Windows.Forms.Padding(1);
            this.m_labelEtcInstanceInterval.Name = "m_labelEtcInstanceInterval";
            this.m_labelEtcInstanceInterval.Size = new System.Drawing.Size(111, 33);
            this.m_labelEtcInstanceInterval.SubFont = new System.Drawing.Font("맑은 고딕", 8F, System.Drawing.FontStyle.Bold);
            this.m_labelEtcInstanceInterval.SubFontColor = System.Drawing.Color.Black;
            this.m_labelEtcInstanceInterval.SubText = "ITV";
            this.m_labelEtcInstanceInterval.TabIndex = 1381;
            this.m_labelEtcInstanceInterval.Text = "0";
            this.m_labelEtcInstanceInterval.TextAlignMain = Sys3Controls.EN_TEXTALIGN.BOTTOM_RIGHT;
            this.m_labelEtcInstanceInterval.TextAlignSub = Sys3Controls.EN_TEXTALIGN.TOP_LEFT;
            this.m_labelEtcInstanceInterval.TextAlignUnit = Sys3Controls.EN_TEXTALIGN.BOTTOM_RIGHT;
            this.m_labelEtcInstanceInterval.ThemeIndex = 0;
            this.m_labelEtcInstanceInterval.UnitAreaRate = 40;
            this.m_labelEtcInstanceInterval.UnitFont = new System.Drawing.Font("맑은 고딕", 10F, System.Drawing.FontStyle.Bold);
            this.m_labelEtcInstanceInterval.UnitFontColor = System.Drawing.Color.Black;
            this.m_labelEtcInstanceInterval.UnitPositionVertical = false;
            this.m_labelEtcInstanceInterval.UnitText = "ms";
            this.m_labelEtcInstanceInterval.UseBorder = true;
            this.m_labelEtcInstanceInterval.UseEdgeRadius = false;
            this.m_labelEtcInstanceInterval.UseImage = false;
            this.m_labelEtcInstanceInterval.UseSubFont = true;
            this.m_labelEtcInstanceInterval.UseUnitFont = true;
            // 
            // m_labelEtcInstanceMax
            // 
            this.m_labelEtcInstanceMax.BackGroundColor = System.Drawing.Color.White;
            this.m_labelEtcInstanceMax.BorderStroke = 2;
            this.m_labelEtcInstanceMax.BorderStyle = System.Windows.Forms.ButtonBorderStyle.Solid;
            this.m_labelEtcInstanceMax.Description = "";
            this.m_labelEtcInstanceMax.DisabledColor = System.Drawing.Color.White;
            this.m_labelEtcInstanceMax.Dock = System.Windows.Forms.DockStyle.Fill;
            this.m_labelEtcInstanceMax.EdgeRadius = 1;
            this.m_labelEtcInstanceMax.Enabled = false;
            this.m_labelEtcInstanceMax.ImagePosition = new System.Drawing.Point(0, 0);
            this.m_labelEtcInstanceMax.ImageSize = new System.Drawing.Point(0, 0);
            this.m_labelEtcInstanceMax.LoadImage = null;
            this.m_labelEtcInstanceMax.Location = new System.Drawing.Point(219, 602);
            this.m_labelEtcInstanceMax.MainFont = new System.Drawing.Font("맑은 고딕", 10F, System.Drawing.FontStyle.Bold);
            this.m_labelEtcInstanceMax.MainFontColor = System.Drawing.Color.Black;
            this.m_labelEtcInstanceMax.Margin = new System.Windows.Forms.Padding(1);
            this.m_labelEtcInstanceMax.Name = "m_labelEtcInstanceMax";
            this.m_labelEtcInstanceMax.Size = new System.Drawing.Size(111, 33);
            this.m_labelEtcInstanceMax.SubFont = new System.Drawing.Font("맑은 고딕", 8F, System.Drawing.FontStyle.Bold);
            this.m_labelEtcInstanceMax.SubFontColor = System.Drawing.Color.Black;
            this.m_labelEtcInstanceMax.SubText = "MAX";
            this.m_labelEtcInstanceMax.TabIndex = 1381;
            this.m_labelEtcInstanceMax.Text = "0";
            this.m_labelEtcInstanceMax.TextAlignMain = Sys3Controls.EN_TEXTALIGN.BOTTOM_RIGHT;
            this.m_labelEtcInstanceMax.TextAlignSub = Sys3Controls.EN_TEXTALIGN.TOP_LEFT;
            this.m_labelEtcInstanceMax.TextAlignUnit = Sys3Controls.EN_TEXTALIGN.BOTTOM_RIGHT;
            this.m_labelEtcInstanceMax.ThemeIndex = 0;
            this.m_labelEtcInstanceMax.UnitAreaRate = 40;
            this.m_labelEtcInstanceMax.UnitFont = new System.Drawing.Font("맑은 고딕", 10F, System.Drawing.FontStyle.Bold);
            this.m_labelEtcInstanceMax.UnitFontColor = System.Drawing.Color.Black;
            this.m_labelEtcInstanceMax.UnitPositionVertical = false;
            this.m_labelEtcInstanceMax.UnitText = "ms";
            this.m_labelEtcInstanceMax.UseBorder = true;
            this.m_labelEtcInstanceMax.UseEdgeRadius = false;
            this.m_labelEtcInstanceMax.UseImage = false;
            this.m_labelEtcInstanceMax.UseSubFont = true;
            this.m_labelEtcInstanceMax.UseUnitFont = true;
            // 
            // m_labelEtcInstanceAverage
            // 
            this.m_labelEtcInstanceAverage.BackGroundColor = System.Drawing.Color.White;
            this.m_labelEtcInstanceAverage.BorderStroke = 2;
            this.m_labelEtcInstanceAverage.BorderStyle = System.Windows.Forms.ButtonBorderStyle.Solid;
            this.m_labelEtcInstanceAverage.Description = "";
            this.m_labelEtcInstanceAverage.DisabledColor = System.Drawing.Color.White;
            this.m_labelEtcInstanceAverage.Dock = System.Windows.Forms.DockStyle.Fill;
            this.m_labelEtcInstanceAverage.EdgeRadius = 1;
            this.m_labelEtcInstanceAverage.Enabled = false;
            this.m_labelEtcInstanceAverage.ImagePosition = new System.Drawing.Point(0, 0);
            this.m_labelEtcInstanceAverage.ImageSize = new System.Drawing.Point(0, 0);
            this.m_labelEtcInstanceAverage.LoadImage = null;
            this.m_labelEtcInstanceAverage.Location = new System.Drawing.Point(106, 602);
            this.m_labelEtcInstanceAverage.MainFont = new System.Drawing.Font("맑은 고딕", 10F, System.Drawing.FontStyle.Bold);
            this.m_labelEtcInstanceAverage.MainFontColor = System.Drawing.Color.Black;
            this.m_labelEtcInstanceAverage.Margin = new System.Windows.Forms.Padding(1);
            this.m_labelEtcInstanceAverage.Name = "m_labelEtcInstanceAverage";
            this.m_labelEtcInstanceAverage.Size = new System.Drawing.Size(111, 33);
            this.m_labelEtcInstanceAverage.SubFont = new System.Drawing.Font("맑은 고딕", 8F, System.Drawing.FontStyle.Bold);
            this.m_labelEtcInstanceAverage.SubFontColor = System.Drawing.Color.Black;
            this.m_labelEtcInstanceAverage.SubText = "AVG";
            this.m_labelEtcInstanceAverage.TabIndex = 1381;
            this.m_labelEtcInstanceAverage.Text = "0";
            this.m_labelEtcInstanceAverage.TextAlignMain = Sys3Controls.EN_TEXTALIGN.BOTTOM_RIGHT;
            this.m_labelEtcInstanceAverage.TextAlignSub = Sys3Controls.EN_TEXTALIGN.TOP_LEFT;
            this.m_labelEtcInstanceAverage.TextAlignUnit = Sys3Controls.EN_TEXTALIGN.BOTTOM_RIGHT;
            this.m_labelEtcInstanceAverage.ThemeIndex = 0;
            this.m_labelEtcInstanceAverage.UnitAreaRate = 40;
            this.m_labelEtcInstanceAverage.UnitFont = new System.Drawing.Font("맑은 고딕", 10F, System.Drawing.FontStyle.Bold);
            this.m_labelEtcInstanceAverage.UnitFontColor = System.Drawing.Color.Black;
            this.m_labelEtcInstanceAverage.UnitPositionVertical = false;
            this.m_labelEtcInstanceAverage.UnitText = "ms";
            this.m_labelEtcInstanceAverage.UseBorder = true;
            this.m_labelEtcInstanceAverage.UseEdgeRadius = false;
            this.m_labelEtcInstanceAverage.UseImage = false;
            this.m_labelEtcInstanceAverage.UseSubFont = true;
            this.m_labelEtcInstanceAverage.UseUnitFont = true;
            // 
            // m_labelMainProcess
            // 
            this.m_labelMainProcess.BackGroundColor = System.Drawing.Color.White;
            this.m_labelMainProcess.BorderStroke = 2;
            this.m_labelMainProcess.BorderStyle = System.Windows.Forms.ButtonBorderStyle.Solid;
            this.m_labelMainProcess.Description = "";
            this.m_labelMainProcess.DisabledColor = System.Drawing.Color.White;
            this.m_labelMainProcess.Dock = System.Windows.Forms.DockStyle.Fill;
            this.m_labelMainProcess.EdgeRadius = 1;
            this.m_labelMainProcess.Enabled = false;
            this.m_labelMainProcess.ImagePosition = new System.Drawing.Point(0, 0);
            this.m_labelMainProcess.ImageSize = new System.Drawing.Point(0, 0);
            this.m_labelMainProcess.LoadImage = null;
            this.m_labelMainProcess.Location = new System.Drawing.Point(219, 7);
            this.m_labelMainProcess.MainFont = new System.Drawing.Font("맑은 고딕", 10F, System.Drawing.FontStyle.Bold);
            this.m_labelMainProcess.MainFontColor = System.Drawing.Color.Black;
            this.m_labelMainProcess.Margin = new System.Windows.Forms.Padding(1);
            this.m_labelMainProcess.Name = "m_labelMainProcess";
            this.m_labelMainProcess.Size = new System.Drawing.Size(111, 33);
            this.m_labelMainProcess.SubFont = new System.Drawing.Font("맑은 고딕", 8F, System.Drawing.FontStyle.Bold);
            this.m_labelMainProcess.SubFontColor = System.Drawing.Color.Black;
            this.m_labelMainProcess.SubText = "PRC";
            this.m_labelMainProcess.TabIndex = 1381;
            this.m_labelMainProcess.Text = "0";
            this.m_labelMainProcess.TextAlignMain = Sys3Controls.EN_TEXTALIGN.BOTTOM_RIGHT;
            this.m_labelMainProcess.TextAlignSub = Sys3Controls.EN_TEXTALIGN.TOP_LEFT;
            this.m_labelMainProcess.TextAlignUnit = Sys3Controls.EN_TEXTALIGN.BOTTOM_RIGHT;
            this.m_labelMainProcess.ThemeIndex = 0;
            this.m_labelMainProcess.UnitAreaRate = 40;
            this.m_labelMainProcess.UnitFont = new System.Drawing.Font("맑은 고딕", 10F, System.Drawing.FontStyle.Bold);
            this.m_labelMainProcess.UnitFontColor = System.Drawing.Color.Black;
            this.m_labelMainProcess.UnitPositionVertical = false;
            this.m_labelMainProcess.UnitText = "ms";
            this.m_labelMainProcess.UseBorder = true;
            this.m_labelMainProcess.UseEdgeRadius = false;
            this.m_labelMainProcess.UseImage = false;
            this.m_labelMainProcess.UseSubFont = true;
            this.m_labelMainProcess.UseUnitFont = true;
            // 
            // m_labelObserverProcess
            // 
            this.m_labelObserverProcess.BackGroundColor = System.Drawing.Color.White;
            this.m_labelObserverProcess.BorderStroke = 2;
            this.m_labelObserverProcess.BorderStyle = System.Windows.Forms.ButtonBorderStyle.Solid;
            this.m_labelObserverProcess.Description = "";
            this.m_labelObserverProcess.DisabledColor = System.Drawing.Color.White;
            this.m_labelObserverProcess.Dock = System.Windows.Forms.DockStyle.Fill;
            this.m_labelObserverProcess.EdgeRadius = 1;
            this.m_labelObserverProcess.Enabled = false;
            this.m_labelObserverProcess.ImagePosition = new System.Drawing.Point(0, 0);
            this.m_labelObserverProcess.ImageSize = new System.Drawing.Point(0, 0);
            this.m_labelObserverProcess.LoadImage = null;
            this.m_labelObserverProcess.Location = new System.Drawing.Point(219, 77);
            this.m_labelObserverProcess.MainFont = new System.Drawing.Font("맑은 고딕", 10F, System.Drawing.FontStyle.Bold);
            this.m_labelObserverProcess.MainFontColor = System.Drawing.Color.Black;
            this.m_labelObserverProcess.Margin = new System.Windows.Forms.Padding(1);
            this.m_labelObserverProcess.Name = "m_labelObserverProcess";
            this.m_labelObserverProcess.Size = new System.Drawing.Size(111, 33);
            this.m_labelObserverProcess.SubFont = new System.Drawing.Font("맑은 고딕", 8F, System.Drawing.FontStyle.Bold);
            this.m_labelObserverProcess.SubFontColor = System.Drawing.Color.Black;
            this.m_labelObserverProcess.SubText = "PRC";
            this.m_labelObserverProcess.TabIndex = 1381;
            this.m_labelObserverProcess.Text = "0";
            this.m_labelObserverProcess.TextAlignMain = Sys3Controls.EN_TEXTALIGN.BOTTOM_RIGHT;
            this.m_labelObserverProcess.TextAlignSub = Sys3Controls.EN_TEXTALIGN.TOP_LEFT;
            this.m_labelObserverProcess.TextAlignUnit = Sys3Controls.EN_TEXTALIGN.BOTTOM_RIGHT;
            this.m_labelObserverProcess.ThemeIndex = 0;
            this.m_labelObserverProcess.UnitAreaRate = 40;
            this.m_labelObserverProcess.UnitFont = new System.Drawing.Font("맑은 고딕", 10F, System.Drawing.FontStyle.Bold);
            this.m_labelObserverProcess.UnitFontColor = System.Drawing.Color.Black;
            this.m_labelObserverProcess.UnitPositionVertical = false;
            this.m_labelObserverProcess.UnitText = "ms";
            this.m_labelObserverProcess.UseBorder = true;
            this.m_labelObserverProcess.UseEdgeRadius = false;
            this.m_labelObserverProcess.UseImage = false;
            this.m_labelObserverProcess.UseSubFont = true;
            this.m_labelObserverProcess.UseUnitFont = true;
            // 
            // m_labelFileIOProcess
            // 
            this.m_labelFileIOProcess.BackGroundColor = System.Drawing.Color.White;
            this.m_labelFileIOProcess.BorderStroke = 2;
            this.m_labelFileIOProcess.BorderStyle = System.Windows.Forms.ButtonBorderStyle.Solid;
            this.m_labelFileIOProcess.Description = "";
            this.m_labelFileIOProcess.DisabledColor = System.Drawing.Color.White;
            this.m_labelFileIOProcess.Dock = System.Windows.Forms.DockStyle.Fill;
            this.m_labelFileIOProcess.EdgeRadius = 1;
            this.m_labelFileIOProcess.Enabled = false;
            this.m_labelFileIOProcess.ImagePosition = new System.Drawing.Point(0, 0);
            this.m_labelFileIOProcess.ImageSize = new System.Drawing.Point(0, 0);
            this.m_labelFileIOProcess.LoadImage = null;
            this.m_labelFileIOProcess.Location = new System.Drawing.Point(219, 147);
            this.m_labelFileIOProcess.MainFont = new System.Drawing.Font("맑은 고딕", 10F, System.Drawing.FontStyle.Bold);
            this.m_labelFileIOProcess.MainFontColor = System.Drawing.Color.Black;
            this.m_labelFileIOProcess.Margin = new System.Windows.Forms.Padding(1);
            this.m_labelFileIOProcess.Name = "m_labelFileIOProcess";
            this.m_labelFileIOProcess.Size = new System.Drawing.Size(111, 33);
            this.m_labelFileIOProcess.SubFont = new System.Drawing.Font("맑은 고딕", 8F, System.Drawing.FontStyle.Bold);
            this.m_labelFileIOProcess.SubFontColor = System.Drawing.Color.Black;
            this.m_labelFileIOProcess.SubText = "PRC";
            this.m_labelFileIOProcess.TabIndex = 1381;
            this.m_labelFileIOProcess.Text = "0";
            this.m_labelFileIOProcess.TextAlignMain = Sys3Controls.EN_TEXTALIGN.BOTTOM_RIGHT;
            this.m_labelFileIOProcess.TextAlignSub = Sys3Controls.EN_TEXTALIGN.TOP_LEFT;
            this.m_labelFileIOProcess.TextAlignUnit = Sys3Controls.EN_TEXTALIGN.BOTTOM_RIGHT;
            this.m_labelFileIOProcess.ThemeIndex = 0;
            this.m_labelFileIOProcess.UnitAreaRate = 40;
            this.m_labelFileIOProcess.UnitFont = new System.Drawing.Font("맑은 고딕", 10F, System.Drawing.FontStyle.Bold);
            this.m_labelFileIOProcess.UnitFontColor = System.Drawing.Color.Black;
            this.m_labelFileIOProcess.UnitPositionVertical = false;
            this.m_labelFileIOProcess.UnitText = "ms";
            this.m_labelFileIOProcess.UseBorder = true;
            this.m_labelFileIOProcess.UseEdgeRadius = false;
            this.m_labelFileIOProcess.UseImage = false;
            this.m_labelFileIOProcess.UseSubFont = true;
            this.m_labelFileIOProcess.UseUnitFont = true;
            // 
            // m_labelDegitalIOProcess
            // 
            this.m_labelDegitalIOProcess.BackGroundColor = System.Drawing.Color.White;
            this.m_labelDegitalIOProcess.BorderStroke = 2;
            this.m_labelDegitalIOProcess.BorderStyle = System.Windows.Forms.ButtonBorderStyle.Solid;
            this.m_labelDegitalIOProcess.Description = "";
            this.m_labelDegitalIOProcess.DisabledColor = System.Drawing.Color.White;
            this.m_labelDegitalIOProcess.Dock = System.Windows.Forms.DockStyle.Fill;
            this.m_labelDegitalIOProcess.EdgeRadius = 1;
            this.m_labelDegitalIOProcess.Enabled = false;
            this.m_labelDegitalIOProcess.ImagePosition = new System.Drawing.Point(0, 0);
            this.m_labelDegitalIOProcess.ImageSize = new System.Drawing.Point(0, 0);
            this.m_labelDegitalIOProcess.LoadImage = null;
            this.m_labelDegitalIOProcess.Location = new System.Drawing.Point(219, 217);
            this.m_labelDegitalIOProcess.MainFont = new System.Drawing.Font("맑은 고딕", 10F, System.Drawing.FontStyle.Bold);
            this.m_labelDegitalIOProcess.MainFontColor = System.Drawing.Color.Black;
            this.m_labelDegitalIOProcess.Margin = new System.Windows.Forms.Padding(1);
            this.m_labelDegitalIOProcess.Name = "m_labelDegitalIOProcess";
            this.m_labelDegitalIOProcess.Size = new System.Drawing.Size(111, 33);
            this.m_labelDegitalIOProcess.SubFont = new System.Drawing.Font("맑은 고딕", 8F, System.Drawing.FontStyle.Bold);
            this.m_labelDegitalIOProcess.SubFontColor = System.Drawing.Color.Black;
            this.m_labelDegitalIOProcess.SubText = "PRC";
            this.m_labelDegitalIOProcess.TabIndex = 1381;
            this.m_labelDegitalIOProcess.Text = "0";
            this.m_labelDegitalIOProcess.TextAlignMain = Sys3Controls.EN_TEXTALIGN.BOTTOM_RIGHT;
            this.m_labelDegitalIOProcess.TextAlignSub = Sys3Controls.EN_TEXTALIGN.TOP_LEFT;
            this.m_labelDegitalIOProcess.TextAlignUnit = Sys3Controls.EN_TEXTALIGN.BOTTOM_RIGHT;
            this.m_labelDegitalIOProcess.ThemeIndex = 0;
            this.m_labelDegitalIOProcess.UnitAreaRate = 40;
            this.m_labelDegitalIOProcess.UnitFont = new System.Drawing.Font("맑은 고딕", 10F, System.Drawing.FontStyle.Bold);
            this.m_labelDegitalIOProcess.UnitFontColor = System.Drawing.Color.Black;
            this.m_labelDegitalIOProcess.UnitPositionVertical = false;
            this.m_labelDegitalIOProcess.UnitText = "ms";
            this.m_labelDegitalIOProcess.UseBorder = true;
            this.m_labelDegitalIOProcess.UseEdgeRadius = false;
            this.m_labelDegitalIOProcess.UseImage = false;
            this.m_labelDegitalIOProcess.UseSubFont = true;
            this.m_labelDegitalIOProcess.UseUnitFont = true;
            // 
            // m_labelAnalogIOProcess
            // 
            this.m_labelAnalogIOProcess.BackGroundColor = System.Drawing.Color.White;
            this.m_labelAnalogIOProcess.BorderStroke = 2;
            this.m_labelAnalogIOProcess.BorderStyle = System.Windows.Forms.ButtonBorderStyle.Solid;
            this.m_labelAnalogIOProcess.Description = "";
            this.m_labelAnalogIOProcess.DisabledColor = System.Drawing.Color.White;
            this.m_labelAnalogIOProcess.Dock = System.Windows.Forms.DockStyle.Fill;
            this.m_labelAnalogIOProcess.EdgeRadius = 1;
            this.m_labelAnalogIOProcess.Enabled = false;
            this.m_labelAnalogIOProcess.ImagePosition = new System.Drawing.Point(0, 0);
            this.m_labelAnalogIOProcess.ImageSize = new System.Drawing.Point(0, 0);
            this.m_labelAnalogIOProcess.LoadImage = null;
            this.m_labelAnalogIOProcess.Location = new System.Drawing.Point(219, 287);
            this.m_labelAnalogIOProcess.MainFont = new System.Drawing.Font("맑은 고딕", 10F, System.Drawing.FontStyle.Bold);
            this.m_labelAnalogIOProcess.MainFontColor = System.Drawing.Color.Black;
            this.m_labelAnalogIOProcess.Margin = new System.Windows.Forms.Padding(1);
            this.m_labelAnalogIOProcess.Name = "m_labelAnalogIOProcess";
            this.m_labelAnalogIOProcess.Size = new System.Drawing.Size(111, 33);
            this.m_labelAnalogIOProcess.SubFont = new System.Drawing.Font("맑은 고딕", 8F, System.Drawing.FontStyle.Bold);
            this.m_labelAnalogIOProcess.SubFontColor = System.Drawing.Color.Black;
            this.m_labelAnalogIOProcess.SubText = "PRC";
            this.m_labelAnalogIOProcess.TabIndex = 1381;
            this.m_labelAnalogIOProcess.Text = "0";
            this.m_labelAnalogIOProcess.TextAlignMain = Sys3Controls.EN_TEXTALIGN.BOTTOM_RIGHT;
            this.m_labelAnalogIOProcess.TextAlignSub = Sys3Controls.EN_TEXTALIGN.TOP_LEFT;
            this.m_labelAnalogIOProcess.TextAlignUnit = Sys3Controls.EN_TEXTALIGN.BOTTOM_RIGHT;
            this.m_labelAnalogIOProcess.ThemeIndex = 0;
            this.m_labelAnalogIOProcess.UnitAreaRate = 40;
            this.m_labelAnalogIOProcess.UnitFont = new System.Drawing.Font("맑은 고딕", 10F, System.Drawing.FontStyle.Bold);
            this.m_labelAnalogIOProcess.UnitFontColor = System.Drawing.Color.Black;
            this.m_labelAnalogIOProcess.UnitPositionVertical = false;
            this.m_labelAnalogIOProcess.UnitText = "ms";
            this.m_labelAnalogIOProcess.UseBorder = true;
            this.m_labelAnalogIOProcess.UseEdgeRadius = false;
            this.m_labelAnalogIOProcess.UseImage = false;
            this.m_labelAnalogIOProcess.UseSubFont = true;
            this.m_labelAnalogIOProcess.UseUnitFont = true;
            // 
            // m_labelMotionProcess
            // 
            this.m_labelMotionProcess.BackGroundColor = System.Drawing.Color.White;
            this.m_labelMotionProcess.BorderStroke = 2;
            this.m_labelMotionProcess.BorderStyle = System.Windows.Forms.ButtonBorderStyle.Solid;
            this.m_labelMotionProcess.Description = "";
            this.m_labelMotionProcess.DisabledColor = System.Drawing.Color.White;
            this.m_labelMotionProcess.Dock = System.Windows.Forms.DockStyle.Fill;
            this.m_labelMotionProcess.EdgeRadius = 1;
            this.m_labelMotionProcess.Enabled = false;
            this.m_labelMotionProcess.ImagePosition = new System.Drawing.Point(0, 0);
            this.m_labelMotionProcess.ImageSize = new System.Drawing.Point(0, 0);
            this.m_labelMotionProcess.LoadImage = null;
            this.m_labelMotionProcess.Location = new System.Drawing.Point(219, 357);
            this.m_labelMotionProcess.MainFont = new System.Drawing.Font("맑은 고딕", 10F, System.Drawing.FontStyle.Bold);
            this.m_labelMotionProcess.MainFontColor = System.Drawing.Color.Black;
            this.m_labelMotionProcess.Margin = new System.Windows.Forms.Padding(1);
            this.m_labelMotionProcess.Name = "m_labelMotionProcess";
            this.m_labelMotionProcess.Size = new System.Drawing.Size(111, 33);
            this.m_labelMotionProcess.SubFont = new System.Drawing.Font("맑은 고딕", 8F, System.Drawing.FontStyle.Bold);
            this.m_labelMotionProcess.SubFontColor = System.Drawing.Color.Black;
            this.m_labelMotionProcess.SubText = "PRC";
            this.m_labelMotionProcess.TabIndex = 1381;
            this.m_labelMotionProcess.Text = "0";
            this.m_labelMotionProcess.TextAlignMain = Sys3Controls.EN_TEXTALIGN.BOTTOM_RIGHT;
            this.m_labelMotionProcess.TextAlignSub = Sys3Controls.EN_TEXTALIGN.TOP_LEFT;
            this.m_labelMotionProcess.TextAlignUnit = Sys3Controls.EN_TEXTALIGN.BOTTOM_RIGHT;
            this.m_labelMotionProcess.ThemeIndex = 0;
            this.m_labelMotionProcess.UnitAreaRate = 40;
            this.m_labelMotionProcess.UnitFont = new System.Drawing.Font("맑은 고딕", 10F, System.Drawing.FontStyle.Bold);
            this.m_labelMotionProcess.UnitFontColor = System.Drawing.Color.Black;
            this.m_labelMotionProcess.UnitPositionVertical = false;
            this.m_labelMotionProcess.UnitText = "ms";
            this.m_labelMotionProcess.UseBorder = true;
            this.m_labelMotionProcess.UseEdgeRadius = false;
            this.m_labelMotionProcess.UseImage = false;
            this.m_labelMotionProcess.UseSubFont = true;
            this.m_labelMotionProcess.UseUnitFont = true;
            // 
            // m_labelGatheringProcess
            // 
            this.m_labelGatheringProcess.BackGroundColor = System.Drawing.Color.White;
            this.m_labelGatheringProcess.BorderStroke = 2;
            this.m_labelGatheringProcess.BorderStyle = System.Windows.Forms.ButtonBorderStyle.Solid;
            this.m_labelGatheringProcess.Description = "";
            this.m_labelGatheringProcess.DisabledColor = System.Drawing.Color.White;
            this.m_labelGatheringProcess.Dock = System.Windows.Forms.DockStyle.Fill;
            this.m_labelGatheringProcess.EdgeRadius = 1;
            this.m_labelGatheringProcess.Enabled = false;
            this.m_labelGatheringProcess.ImagePosition = new System.Drawing.Point(0, 0);
            this.m_labelGatheringProcess.ImageSize = new System.Drawing.Point(0, 0);
            this.m_labelGatheringProcess.LoadImage = null;
            this.m_labelGatheringProcess.Location = new System.Drawing.Point(219, 427);
            this.m_labelGatheringProcess.MainFont = new System.Drawing.Font("맑은 고딕", 10F, System.Drawing.FontStyle.Bold);
            this.m_labelGatheringProcess.MainFontColor = System.Drawing.Color.Black;
            this.m_labelGatheringProcess.Margin = new System.Windows.Forms.Padding(1);
            this.m_labelGatheringProcess.Name = "m_labelGatheringProcess";
            this.m_labelGatheringProcess.Size = new System.Drawing.Size(111, 33);
            this.m_labelGatheringProcess.SubFont = new System.Drawing.Font("맑은 고딕", 8F, System.Drawing.FontStyle.Bold);
            this.m_labelGatheringProcess.SubFontColor = System.Drawing.Color.Black;
            this.m_labelGatheringProcess.SubText = "PRC";
            this.m_labelGatheringProcess.TabIndex = 1381;
            this.m_labelGatheringProcess.Text = "0";
            this.m_labelGatheringProcess.TextAlignMain = Sys3Controls.EN_TEXTALIGN.BOTTOM_RIGHT;
            this.m_labelGatheringProcess.TextAlignSub = Sys3Controls.EN_TEXTALIGN.TOP_LEFT;
            this.m_labelGatheringProcess.TextAlignUnit = Sys3Controls.EN_TEXTALIGN.BOTTOM_RIGHT;
            this.m_labelGatheringProcess.ThemeIndex = 0;
            this.m_labelGatheringProcess.UnitAreaRate = 40;
            this.m_labelGatheringProcess.UnitFont = new System.Drawing.Font("맑은 고딕", 10F, System.Drawing.FontStyle.Bold);
            this.m_labelGatheringProcess.UnitFontColor = System.Drawing.Color.Black;
            this.m_labelGatheringProcess.UnitPositionVertical = false;
            this.m_labelGatheringProcess.UnitText = "ms";
            this.m_labelGatheringProcess.UseBorder = true;
            this.m_labelGatheringProcess.UseEdgeRadius = false;
            this.m_labelGatheringProcess.UseImage = false;
            this.m_labelGatheringProcess.UseSubFont = true;
            this.m_labelGatheringProcess.UseUnitFont = true;
            // 
            // m_labelCommunicationProcess
            // 
            this.m_labelCommunicationProcess.BackGroundColor = System.Drawing.Color.White;
            this.m_labelCommunicationProcess.BorderStroke = 2;
            this.m_labelCommunicationProcess.BorderStyle = System.Windows.Forms.ButtonBorderStyle.Solid;
            this.m_labelCommunicationProcess.Description = "";
            this.m_labelCommunicationProcess.DisabledColor = System.Drawing.Color.White;
            this.m_labelCommunicationProcess.Dock = System.Windows.Forms.DockStyle.Fill;
            this.m_labelCommunicationProcess.EdgeRadius = 1;
            this.m_labelCommunicationProcess.Enabled = false;
            this.m_labelCommunicationProcess.ImagePosition = new System.Drawing.Point(0, 0);
            this.m_labelCommunicationProcess.ImageSize = new System.Drawing.Point(0, 0);
            this.m_labelCommunicationProcess.LoadImage = null;
            this.m_labelCommunicationProcess.Location = new System.Drawing.Point(219, 497);
            this.m_labelCommunicationProcess.MainFont = new System.Drawing.Font("맑은 고딕", 10F, System.Drawing.FontStyle.Bold);
            this.m_labelCommunicationProcess.MainFontColor = System.Drawing.Color.Black;
            this.m_labelCommunicationProcess.Margin = new System.Windows.Forms.Padding(1);
            this.m_labelCommunicationProcess.Name = "m_labelCommunicationProcess";
            this.m_labelCommunicationProcess.Size = new System.Drawing.Size(111, 33);
            this.m_labelCommunicationProcess.SubFont = new System.Drawing.Font("맑은 고딕", 8F, System.Drawing.FontStyle.Bold);
            this.m_labelCommunicationProcess.SubFontColor = System.Drawing.Color.Black;
            this.m_labelCommunicationProcess.SubText = "PRC";
            this.m_labelCommunicationProcess.TabIndex = 1381;
            this.m_labelCommunicationProcess.Text = "0";
            this.m_labelCommunicationProcess.TextAlignMain = Sys3Controls.EN_TEXTALIGN.BOTTOM_RIGHT;
            this.m_labelCommunicationProcess.TextAlignSub = Sys3Controls.EN_TEXTALIGN.TOP_LEFT;
            this.m_labelCommunicationProcess.TextAlignUnit = Sys3Controls.EN_TEXTALIGN.BOTTOM_RIGHT;
            this.m_labelCommunicationProcess.ThemeIndex = 0;
            this.m_labelCommunicationProcess.UnitAreaRate = 40;
            this.m_labelCommunicationProcess.UnitFont = new System.Drawing.Font("맑은 고딕", 10F, System.Drawing.FontStyle.Bold);
            this.m_labelCommunicationProcess.UnitFontColor = System.Drawing.Color.Black;
            this.m_labelCommunicationProcess.UnitPositionVertical = false;
            this.m_labelCommunicationProcess.UnitText = "ms";
            this.m_labelCommunicationProcess.UseBorder = true;
            this.m_labelCommunicationProcess.UseEdgeRadius = false;
            this.m_labelCommunicationProcess.UseImage = false;
            this.m_labelCommunicationProcess.UseSubFont = true;
            this.m_labelCommunicationProcess.UseUnitFont = true;
            // 
            // m_labelEtcInstanceProcess
            // 
            this.m_labelEtcInstanceProcess.BackGroundColor = System.Drawing.Color.White;
            this.m_labelEtcInstanceProcess.BorderStroke = 2;
            this.m_labelEtcInstanceProcess.BorderStyle = System.Windows.Forms.ButtonBorderStyle.Solid;
            this.m_labelEtcInstanceProcess.Description = "";
            this.m_labelEtcInstanceProcess.DisabledColor = System.Drawing.Color.White;
            this.m_labelEtcInstanceProcess.Dock = System.Windows.Forms.DockStyle.Fill;
            this.m_labelEtcInstanceProcess.EdgeRadius = 1;
            this.m_labelEtcInstanceProcess.Enabled = false;
            this.m_labelEtcInstanceProcess.ImagePosition = new System.Drawing.Point(0, 0);
            this.m_labelEtcInstanceProcess.ImageSize = new System.Drawing.Point(0, 0);
            this.m_labelEtcInstanceProcess.LoadImage = null;
            this.m_labelEtcInstanceProcess.Location = new System.Drawing.Point(219, 567);
            this.m_labelEtcInstanceProcess.MainFont = new System.Drawing.Font("맑은 고딕", 10F, System.Drawing.FontStyle.Bold);
            this.m_labelEtcInstanceProcess.MainFontColor = System.Drawing.Color.Black;
            this.m_labelEtcInstanceProcess.Margin = new System.Windows.Forms.Padding(1);
            this.m_labelEtcInstanceProcess.Name = "m_labelEtcInstanceProcess";
            this.m_labelEtcInstanceProcess.Size = new System.Drawing.Size(111, 33);
            this.m_labelEtcInstanceProcess.SubFont = new System.Drawing.Font("맑은 고딕", 8F, System.Drawing.FontStyle.Bold);
            this.m_labelEtcInstanceProcess.SubFontColor = System.Drawing.Color.Black;
            this.m_labelEtcInstanceProcess.SubText = "PRC";
            this.m_labelEtcInstanceProcess.TabIndex = 1381;
            this.m_labelEtcInstanceProcess.Text = "0";
            this.m_labelEtcInstanceProcess.TextAlignMain = Sys3Controls.EN_TEXTALIGN.BOTTOM_RIGHT;
            this.m_labelEtcInstanceProcess.TextAlignSub = Sys3Controls.EN_TEXTALIGN.TOP_LEFT;
            this.m_labelEtcInstanceProcess.TextAlignUnit = Sys3Controls.EN_TEXTALIGN.BOTTOM_RIGHT;
            this.m_labelEtcInstanceProcess.ThemeIndex = 0;
            this.m_labelEtcInstanceProcess.UnitAreaRate = 40;
            this.m_labelEtcInstanceProcess.UnitFont = new System.Drawing.Font("맑은 고딕", 10F, System.Drawing.FontStyle.Bold);
            this.m_labelEtcInstanceProcess.UnitFontColor = System.Drawing.Color.Black;
            this.m_labelEtcInstanceProcess.UnitPositionVertical = false;
            this.m_labelEtcInstanceProcess.UnitText = "ms";
            this.m_labelEtcInstanceProcess.UseBorder = true;
            this.m_labelEtcInstanceProcess.UseEdgeRadius = false;
            this.m_labelEtcInstanceProcess.UseImage = false;
            this.m_labelEtcInstanceProcess.UseSubFont = true;
            this.m_labelEtcInstanceProcess.UseUnitFont = true;
            // 
            // m_dgViewLinkedAction
            // 
            this.m_dgViewLinkedAction.AllowUserToAddRows = false;
            this.m_dgViewLinkedAction.AllowUserToDeleteRows = false;
            this.m_dgViewLinkedAction.AllowUserToResizeColumns = false;
            this.m_dgViewLinkedAction.AllowUserToResizeRows = false;
            this.m_dgViewLinkedAction.BackgroundColor = System.Drawing.Color.White;
            this.m_dgViewLinkedAction.CellBorderStyle = System.Windows.Forms.DataGridViewCellBorderStyle.Raised;
            this.m_dgViewLinkedAction.ColumnHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.Single;
            dataGridViewCellStyle10.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle10.BackColor = System.Drawing.Color.LightGray;
            dataGridViewCellStyle10.Font = new System.Drawing.Font("맑은 고딕", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            dataGridViewCellStyle10.ForeColor = System.Drawing.Color.Black;
            dataGridViewCellStyle10.SelectionBackColor = System.Drawing.Color.White;
            dataGridViewCellStyle10.SelectionForeColor = System.Drawing.Color.Black;
            dataGridViewCellStyle10.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.m_dgViewLinkedAction.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle10;
            this.m_dgViewLinkedAction.ColumnHeadersHeight = 30;
            this.m_dgViewLinkedAction.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            this.m_dgViewLinkedAction.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.dataGridViewTextBoxColumn12,
            this.dataGridViewTextBoxColumn13,
            this.dataGridViewTextBoxColumn14});
            dataGridViewCellStyle11.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle11.BackColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle11.Font = new System.Drawing.Font("맑은 고딕", 10F, System.Drawing.FontStyle.Bold);
            dataGridViewCellStyle11.ForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle11.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(120)))), ((int)(((byte)(130)))), ((int)(((byte)(150)))));
            dataGridViewCellStyle11.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle11.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.m_dgViewLinkedAction.DefaultCellStyle = dataGridViewCellStyle11;
            this.m_dgViewLinkedAction.Dock = System.Windows.Forms.DockStyle.Fill;
            this.m_dgViewLinkedAction.EnableHeadersVisualStyles = false;
            this.m_dgViewLinkedAction.GridColor = System.Drawing.Color.FromArgb(((int)(((byte)(120)))), ((int)(((byte)(130)))), ((int)(((byte)(150)))));
            this.m_dgViewLinkedAction.Location = new System.Drawing.Point(5, 34);
            this.m_dgViewLinkedAction.MultiSelect = false;
            this.m_dgViewLinkedAction.Name = "m_dgViewLinkedAction";
            this.m_dgViewLinkedAction.ReadOnly = true;
            this.m_dgViewLinkedAction.RowHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.None;
            dataGridViewCellStyle12.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle12.BackColor = System.Drawing.Color.White;
            dataGridViewCellStyle12.Font = new System.Drawing.Font("맑은 고딕", 11F);
            dataGridViewCellStyle12.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle12.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(120)))), ((int)(((byte)(130)))), ((int)(((byte)(150)))));
            dataGridViewCellStyle12.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle12.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.m_dgViewLinkedAction.RowHeadersDefaultCellStyle = dataGridViewCellStyle12;
            this.m_dgViewLinkedAction.RowHeadersVisible = false;
            this.m_dgViewLinkedAction.RowHeadersWidthSizeMode = System.Windows.Forms.DataGridViewRowHeadersWidthSizeMode.DisableResizing;
            this.m_dgViewLinkedAction.RowTemplate.Height = 30;
            this.m_dgViewLinkedAction.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.m_dgViewLinkedAction.Size = new System.Drawing.Size(788, 176);
            this.m_dgViewLinkedAction.TabIndex = 1382;
            // 
            // dataGridViewTextBoxColumn12
            // 
            this.dataGridViewTextBoxColumn12.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            this.dataGridViewTextBoxColumn12.Frozen = true;
            this.dataGridViewTextBoxColumn12.HeaderText = "TASK";
            this.dataGridViewTextBoxColumn12.Name = "dataGridViewTextBoxColumn12";
            this.dataGridViewTextBoxColumn12.ReadOnly = true;
            this.dataGridViewTextBoxColumn12.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.dataGridViewTextBoxColumn12.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.dataGridViewTextBoxColumn12.Width = 250;
            // 
            // dataGridViewTextBoxColumn13
            // 
            this.dataGridViewTextBoxColumn13.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.dataGridViewTextBoxColumn13.HeaderText = "ACTION";
            this.dataGridViewTextBoxColumn13.MaxInputLength = 20;
            this.dataGridViewTextBoxColumn13.Name = "dataGridViewTextBoxColumn13";
            this.dataGridViewTextBoxColumn13.ReadOnly = true;
            this.dataGridViewTextBoxColumn13.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.dataGridViewTextBoxColumn13.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // dataGridViewTextBoxColumn14
            // 
            this.dataGridViewTextBoxColumn14.HeaderText = "ACTION STATE";
            this.dataGridViewTextBoxColumn14.Name = "dataGridViewTextBoxColumn14";
            this.dataGridViewTextBoxColumn14.ReadOnly = true;
            this.dataGridViewTextBoxColumn14.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.dataGridViewTextBoxColumn14.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.dataGridViewTextBoxColumn14.Width = 125;
            // 
            // m_dgViewDerefAction
            // 
            this.m_dgViewDerefAction.AllowUserToAddRows = false;
            this.m_dgViewDerefAction.AllowUserToDeleteRows = false;
            this.m_dgViewDerefAction.AllowUserToResizeColumns = false;
            this.m_dgViewDerefAction.AllowUserToResizeRows = false;
            this.m_dgViewDerefAction.BackgroundColor = System.Drawing.Color.White;
            this.m_dgViewDerefAction.CellBorderStyle = System.Windows.Forms.DataGridViewCellBorderStyle.Raised;
            this.m_dgViewDerefAction.ColumnHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.Single;
            dataGridViewCellStyle13.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle13.BackColor = System.Drawing.Color.LightGray;
            dataGridViewCellStyle13.Font = new System.Drawing.Font("맑은 고딕", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            dataGridViewCellStyle13.ForeColor = System.Drawing.Color.Black;
            dataGridViewCellStyle13.SelectionBackColor = System.Drawing.Color.White;
            dataGridViewCellStyle13.SelectionForeColor = System.Drawing.Color.Black;
            dataGridViewCellStyle13.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.m_dgViewDerefAction.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle13;
            this.m_dgViewDerefAction.ColumnHeadersHeight = 30;
            this.m_dgViewDerefAction.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            this.m_dgViewDerefAction.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.dataGridViewTextBoxColumn4,
            this.dataGridViewTextBoxColumn5,
            this.dataGridViewTextBoxColumn6,
            this.dataGridViewTextBoxColumn7});
            dataGridViewCellStyle14.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle14.BackColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle14.Font = new System.Drawing.Font("맑은 고딕", 10F, System.Drawing.FontStyle.Bold);
            dataGridViewCellStyle14.ForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle14.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(120)))), ((int)(((byte)(130)))), ((int)(((byte)(150)))));
            dataGridViewCellStyle14.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle14.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.m_dgViewDerefAction.DefaultCellStyle = dataGridViewCellStyle14;
            this.m_dgViewDerefAction.Dock = System.Windows.Forms.DockStyle.Fill;
            this.m_dgViewDerefAction.EnableHeadersVisualStyles = false;
            this.m_dgViewDerefAction.GridColor = System.Drawing.Color.FromArgb(((int)(((byte)(120)))), ((int)(((byte)(130)))), ((int)(((byte)(150)))));
            this.m_dgViewDerefAction.Location = new System.Drawing.Point(5, 34);
            this.m_dgViewDerefAction.MultiSelect = false;
            this.m_dgViewDerefAction.Name = "m_dgViewDerefAction";
            this.m_dgViewDerefAction.ReadOnly = true;
            this.m_dgViewDerefAction.RowHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.None;
            dataGridViewCellStyle15.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle15.BackColor = System.Drawing.Color.White;
            dataGridViewCellStyle15.Font = new System.Drawing.Font("맑은 고딕", 11F);
            dataGridViewCellStyle15.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle15.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(120)))), ((int)(((byte)(130)))), ((int)(((byte)(150)))));
            dataGridViewCellStyle15.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle15.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.m_dgViewDerefAction.RowHeadersDefaultCellStyle = dataGridViewCellStyle15;
            this.m_dgViewDerefAction.RowHeadersVisible = false;
            this.m_dgViewDerefAction.RowHeadersWidthSizeMode = System.Windows.Forms.DataGridViewRowHeadersWidthSizeMode.DisableResizing;
            this.m_dgViewDerefAction.RowTemplate.Height = 30;
            this.m_dgViewDerefAction.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.m_dgViewDerefAction.Size = new System.Drawing.Size(788, 179);
            this.m_dgViewDerefAction.TabIndex = 1383;
            // 
            // dataGridViewTextBoxColumn4
            // 
            this.dataGridViewTextBoxColumn4.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            this.dataGridViewTextBoxColumn4.Frozen = true;
            this.dataGridViewTextBoxColumn4.HeaderText = "TASK";
            this.dataGridViewTextBoxColumn4.Name = "dataGridViewTextBoxColumn4";
            this.dataGridViewTextBoxColumn4.ReadOnly = true;
            this.dataGridViewTextBoxColumn4.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.dataGridViewTextBoxColumn4.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.dataGridViewTextBoxColumn4.Width = 250;
            // 
            // dataGridViewTextBoxColumn5
            // 
            this.dataGridViewTextBoxColumn5.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.dataGridViewTextBoxColumn5.HeaderText = "ACTION";
            this.dataGridViewTextBoxColumn5.MaxInputLength = 20;
            this.dataGridViewTextBoxColumn5.Name = "dataGridViewTextBoxColumn5";
            this.dataGridViewTextBoxColumn5.ReadOnly = true;
            this.dataGridViewTextBoxColumn5.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.dataGridViewTextBoxColumn5.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // dataGridViewTextBoxColumn6
            // 
            this.dataGridViewTextBoxColumn6.HeaderText = "ACTION STATE";
            this.dataGridViewTextBoxColumn6.Name = "dataGridViewTextBoxColumn6";
            this.dataGridViewTextBoxColumn6.ReadOnly = true;
            this.dataGridViewTextBoxColumn6.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.dataGridViewTextBoxColumn6.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.dataGridViewTextBoxColumn6.Width = 125;
            // 
            // dataGridViewTextBoxColumn7
            // 
            this.dataGridViewTextBoxColumn7.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.ColumnHeader;
            this.dataGridViewTextBoxColumn7.HeaderText = "CHECK STATE";
            this.dataGridViewTextBoxColumn7.Name = "dataGridViewTextBoxColumn7";
            this.dataGridViewTextBoxColumn7.ReadOnly = true;
            this.dataGridViewTextBoxColumn7.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.dataGridViewTextBoxColumn7.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.dataGridViewTextBoxColumn7.Width = 98;
            // 
            // m_dgViewActionPort
            // 
            this.m_dgViewActionPort.AllowUserToAddRows = false;
            this.m_dgViewActionPort.AllowUserToDeleteRows = false;
            this.m_dgViewActionPort.AllowUserToResizeColumns = false;
            this.m_dgViewActionPort.AllowUserToResizeRows = false;
            this.m_dgViewActionPort.BackgroundColor = System.Drawing.Color.White;
            this.m_dgViewActionPort.CellBorderStyle = System.Windows.Forms.DataGridViewCellBorderStyle.Raised;
            this.m_dgViewActionPort.ColumnHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.Single;
            dataGridViewCellStyle7.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle7.BackColor = System.Drawing.Color.LightGray;
            dataGridViewCellStyle7.Font = new System.Drawing.Font("맑은 고딕", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            dataGridViewCellStyle7.ForeColor = System.Drawing.Color.Black;
            dataGridViewCellStyle7.SelectionBackColor = System.Drawing.Color.White;
            dataGridViewCellStyle7.SelectionForeColor = System.Drawing.Color.Black;
            dataGridViewCellStyle7.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.m_dgViewActionPort.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle7;
            this.m_dgViewActionPort.ColumnHeadersHeight = 30;
            this.m_dgViewActionPort.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            this.m_dgViewActionPort.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.ColumnTask,
            this.dataGridViewTextBoxColumn1,
            this.dataGridViewTextBoxColumn8});
            dataGridViewCellStyle8.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle8.BackColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle8.Font = new System.Drawing.Font("맑은 고딕", 10F, System.Drawing.FontStyle.Bold);
            dataGridViewCellStyle8.ForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle8.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(120)))), ((int)(((byte)(130)))), ((int)(((byte)(150)))));
            dataGridViewCellStyle8.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle8.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.m_dgViewActionPort.DefaultCellStyle = dataGridViewCellStyle8;
            this.m_dgViewActionPort.Dock = System.Windows.Forms.DockStyle.Fill;
            this.m_dgViewActionPort.EnableHeadersVisualStyles = false;
            this.m_dgViewActionPort.GridColor = System.Drawing.Color.FromArgb(((int)(((byte)(120)))), ((int)(((byte)(130)))), ((int)(((byte)(150)))));
            this.m_dgViewActionPort.Location = new System.Drawing.Point(395, 0);
            this.m_dgViewActionPort.Margin = new System.Windows.Forms.Padding(1, 0, 0, 0);
            this.m_dgViewActionPort.MultiSelect = false;
            this.m_dgViewActionPort.Name = "m_dgViewActionPort";
            this.m_dgViewActionPort.ReadOnly = true;
            this.m_dgViewActionPort.RowHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.None;
            dataGridViewCellStyle9.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle9.BackColor = System.Drawing.Color.White;
            dataGridViewCellStyle9.Font = new System.Drawing.Font("맑은 고딕", 11F);
            dataGridViewCellStyle9.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle9.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(120)))), ((int)(((byte)(130)))), ((int)(((byte)(150)))));
            dataGridViewCellStyle9.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle9.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.m_dgViewActionPort.RowHeadersDefaultCellStyle = dataGridViewCellStyle9;
            this.m_dgViewActionPort.RowHeadersVisible = false;
            this.m_dgViewActionPort.RowHeadersWidthSizeMode = System.Windows.Forms.DataGridViewRowHeadersWidthSizeMode.DisableResizing;
            this.m_dgViewActionPort.RowTemplate.Height = 30;
            this.m_dgViewActionPort.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.m_dgViewActionPort.Size = new System.Drawing.Size(393, 176);
            this.m_dgViewActionPort.TabIndex = 1287;
            this.m_dgViewActionPort.CellClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.Click_Action);
            // 
            // ColumnTask
            // 
            this.ColumnTask.HeaderText = "TASK";
            this.ColumnTask.Name = "ColumnTask";
            this.ColumnTask.ReadOnly = true;
            this.ColumnTask.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.ColumnTask.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.ColumnTask.Width = 125;
            // 
            // dataGridViewTextBoxColumn1
            // 
            this.dataGridViewTextBoxColumn1.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.dataGridViewTextBoxColumn1.HeaderText = "PORT NAME";
            this.dataGridViewTextBoxColumn1.MaxInputLength = 20;
            this.dataGridViewTextBoxColumn1.Name = "dataGridViewTextBoxColumn1";
            this.dataGridViewTextBoxColumn1.ReadOnly = true;
            this.dataGridViewTextBoxColumn1.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.dataGridViewTextBoxColumn1.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // dataGridViewTextBoxColumn8
            // 
            this.dataGridViewTextBoxColumn8.HeaderText = "PORT STATE";
            this.dataGridViewTextBoxColumn8.Name = "dataGridViewTextBoxColumn8";
            this.dataGridViewTextBoxColumn8.ReadOnly = true;
            this.dataGridViewTextBoxColumn8.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.dataGridViewTextBoxColumn8.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.dataGridViewTextBoxColumn8.Width = 110;
            // 
            // _tableLayoutPanel_Main
            // 
            this._tableLayoutPanel_Main.ColumnCount = 2;
            this._tableLayoutPanel_Main.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 29.66175F));
            this._tableLayoutPanel_Main.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 70.33825F));
            this._tableLayoutPanel_Main.Controls.Add(this._tableLayoutPanel_LeftBottom, 0, 1);
            this._tableLayoutPanel_Main.Controls.Add(this.tableLayoutPanel1, 0, 0);
            this._tableLayoutPanel_Main.Controls.Add(this.m_groupTaskState, 1, 0);
            this._tableLayoutPanel_Main.Controls.Add(this.m_groupActionState, 1, 1);
            this._tableLayoutPanel_Main.Controls.Add(this.sys3GroupBox1, 1, 2);
            this._tableLayoutPanel_Main.Controls.Add(this.sys3GroupBox2, 1, 3);
            this._tableLayoutPanel_Main.Dock = System.Windows.Forms.DockStyle.Fill;
            this._tableLayoutPanel_Main.Location = new System.Drawing.Point(3, 34);
            this._tableLayoutPanel_Main.Name = "_tableLayoutPanel_Main";
            this._tableLayoutPanel_Main.RowCount = 4;
            this._tableLayoutPanel_Main.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this._tableLayoutPanel_Main.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this._tableLayoutPanel_Main.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this._tableLayoutPanel_Main.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this._tableLayoutPanel_Main.Size = new System.Drawing.Size(1134, 863);
            this._tableLayoutPanel_Main.TabIndex = 1384;
            // 
            // _tableLayoutPanel_LeftBottom
            // 
            this._tableLayoutPanel_LeftBottom.ColumnCount = 5;
            this._tableLayoutPanel_LeftBottom.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 1.04659F));
            this._tableLayoutPanel_LeftBottom.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 30.38488F));
            this._tableLayoutPanel_LeftBottom.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 33.76097F));
            this._tableLayoutPanel_LeftBottom.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 33.76097F));
            this._tableLayoutPanel_LeftBottom.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 1.04659F));
            this._tableLayoutPanel_LeftBottom.Controls.Add(this.m_lblMainThread, 1, 1);
            this._tableLayoutPanel_LeftBottom.Controls.Add(this.m_lblObserver, 1, 3);
            this._tableLayoutPanel_LeftBottom.Controls.Add(this.m_lblFileIO, 1, 5);
            this._tableLayoutPanel_LeftBottom.Controls.Add(this.m_labelEtcInstanceMax, 3, 18);
            this._tableLayoutPanel_LeftBottom.Controls.Add(this.m_labelEtcInstanceAverage, 2, 18);
            this._tableLayoutPanel_LeftBottom.Controls.Add(this.m_lblDigitalIO, 1, 7);
            this._tableLayoutPanel_LeftBottom.Controls.Add(this.m_labelCommunicationAverage, 2, 16);
            this._tableLayoutPanel_LeftBottom.Controls.Add(this.m_labelEtcInstanceProcess, 3, 17);
            this._tableLayoutPanel_LeftBottom.Controls.Add(this.m_labelEtcInstanceInterval, 2, 17);
            this._tableLayoutPanel_LeftBottom.Controls.Add(this.m_lblAnalogIO, 1, 9);
            this._tableLayoutPanel_LeftBottom.Controls.Add(this.m_labelCommunicationMax, 3, 16);
            this._tableLayoutPanel_LeftBottom.Controls.Add(this.m_labelGatheringAverage, 2, 14);
            this._tableLayoutPanel_LeftBottom.Controls.Add(this.m_lblMotion, 1, 11);
            this._tableLayoutPanel_LeftBottom.Controls.Add(this.m_labelMotionAverage, 2, 12);
            this._tableLayoutPanel_LeftBottom.Controls.Add(this.m_labelCommunicationInterval, 2, 15);
            this._tableLayoutPanel_LeftBottom.Controls.Add(this.m_labelCommunicationProcess, 3, 15);
            this._tableLayoutPanel_LeftBottom.Controls.Add(this.m_lblGathering, 1, 13);
            this._tableLayoutPanel_LeftBottom.Controls.Add(this.m_labelGatheringMax, 3, 14);
            this._tableLayoutPanel_LeftBottom.Controls.Add(this.m_labelAnalogIOAverage, 2, 10);
            this._tableLayoutPanel_LeftBottom.Controls.Add(this.m_lblCommunication, 1, 15);
            this._tableLayoutPanel_LeftBottom.Controls.Add(this.m_labelDegitalIOAverage, 2, 8);
            this._tableLayoutPanel_LeftBottom.Controls.Add(this.m_labelGatheringInterval, 2, 13);
            this._tableLayoutPanel_LeftBottom.Controls.Add(this.m_lblEtcInstance, 1, 17);
            this._tableLayoutPanel_LeftBottom.Controls.Add(this.m_labelGatheringProcess, 3, 13);
            this._tableLayoutPanel_LeftBottom.Controls.Add(this.m_labelMotionMax, 3, 12);
            this._tableLayoutPanel_LeftBottom.Controls.Add(this.m_labelFileIOAverage, 2, 6);
            this._tableLayoutPanel_LeftBottom.Controls.Add(this.m_labelMainInterval, 2, 1);
            this._tableLayoutPanel_LeftBottom.Controls.Add(this.m_labelObserverAverage, 2, 4);
            this._tableLayoutPanel_LeftBottom.Controls.Add(this.m_labelMotionInterval, 2, 11);
            this._tableLayoutPanel_LeftBottom.Controls.Add(this.m_labelMainProcess, 3, 1);
            this._tableLayoutPanel_LeftBottom.Controls.Add(this.m_labelAnalogIOMax, 3, 10);
            this._tableLayoutPanel_LeftBottom.Controls.Add(this.m_labelMotionProcess, 3, 11);
            this._tableLayoutPanel_LeftBottom.Controls.Add(this.m_labelMainAverage, 2, 2);
            this._tableLayoutPanel_LeftBottom.Controls.Add(this.m_labelMainMax, 3, 2);
            this._tableLayoutPanel_LeftBottom.Controls.Add(this.m_labelObserverInterval, 2, 3);
            this._tableLayoutPanel_LeftBottom.Controls.Add(this.m_labelAnalogIOInterval, 2, 9);
            this._tableLayoutPanel_LeftBottom.Controls.Add(this.m_labelObserverProcess, 3, 3);
            this._tableLayoutPanel_LeftBottom.Controls.Add(this.m_labelDegitalIOMax, 3, 8);
            this._tableLayoutPanel_LeftBottom.Controls.Add(this.m_labelObserverMax, 3, 4);
            this._tableLayoutPanel_LeftBottom.Controls.Add(this.m_labelAnalogIOProcess, 3, 9);
            this._tableLayoutPanel_LeftBottom.Controls.Add(this.m_labelFileIOInterval, 2, 5);
            this._tableLayoutPanel_LeftBottom.Controls.Add(this.m_labelFileIOProcess, 3, 5);
            this._tableLayoutPanel_LeftBottom.Controls.Add(this.m_labelDegitalIOInterval, 2, 7);
            this._tableLayoutPanel_LeftBottom.Controls.Add(this.m_labelFileIOMax, 3, 6);
            this._tableLayoutPanel_LeftBottom.Controls.Add(this.m_labelDegitalIOProcess, 3, 7);
            this._tableLayoutPanel_LeftBottom.Dock = System.Windows.Forms.DockStyle.Fill;
            this._tableLayoutPanel_LeftBottom.Location = new System.Drawing.Point(0, 215);
            this._tableLayoutPanel_LeftBottom.Margin = new System.Windows.Forms.Padding(0);
            this._tableLayoutPanel_LeftBottom.Name = "_tableLayoutPanel_LeftBottom";
            this._tableLayoutPanel_LeftBottom.RowCount = 20;
            this._tableLayoutPanel_Main.SetRowSpan(this._tableLayoutPanel_LeftBottom, 3);
            this._tableLayoutPanel_LeftBottom.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 1.001742F));
            this._tableLayoutPanel_LeftBottom.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 5.44425F));
            this._tableLayoutPanel_LeftBottom.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 5.44425F));
            this._tableLayoutPanel_LeftBottom.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 5.44425F));
            this._tableLayoutPanel_LeftBottom.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 5.44425F));
            this._tableLayoutPanel_LeftBottom.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 5.44425F));
            this._tableLayoutPanel_LeftBottom.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 5.44425F));
            this._tableLayoutPanel_LeftBottom.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 5.44425F));
            this._tableLayoutPanel_LeftBottom.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 5.44425F));
            this._tableLayoutPanel_LeftBottom.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 5.44425F));
            this._tableLayoutPanel_LeftBottom.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 5.44425F));
            this._tableLayoutPanel_LeftBottom.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 5.44425F));
            this._tableLayoutPanel_LeftBottom.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 5.44425F));
            this._tableLayoutPanel_LeftBottom.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 5.44425F));
            this._tableLayoutPanel_LeftBottom.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 5.44425F));
            this._tableLayoutPanel_LeftBottom.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 5.44425F));
            this._tableLayoutPanel_LeftBottom.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 5.44425F));
            this._tableLayoutPanel_LeftBottom.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 5.44425F));
            this._tableLayoutPanel_LeftBottom.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 5.44425F));
            this._tableLayoutPanel_LeftBottom.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 1.001742F));
            this._tableLayoutPanel_LeftBottom.Size = new System.Drawing.Size(336, 648);
            this._tableLayoutPanel_LeftBottom.TabIndex = 0;
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 5;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 4.545455F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 25F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 45.45454F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 45.45454F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 4.545455F));
            this.tableLayoutPanel1.Controls.Add(this.m_ledPause, 1, 1);
            this.tableLayoutPanel1.Controls.Add(this.m_lblTimerPause, 2, 1);
            this.tableLayoutPanel1.Controls.Add(this.m_labelTimerPause, 3, 1);
            this.tableLayoutPanel1.Controls.Add(this.m_ledInit, 1, 2);
            this.tableLayoutPanel1.Controls.Add(this.m_ledIdle, 1, 3);
            this.tableLayoutPanel1.Controls.Add(this.m_ledSetup, 1, 4);
            this.tableLayoutPanel1.Controls.Add(this.m_ledRun, 1, 5);
            this.tableLayoutPanel1.Controls.Add(this.m_ledAlarm, 1, 6);
            this.tableLayoutPanel1.Controls.Add(this.m_lblTimerInit, 2, 2);
            this.tableLayoutPanel1.Controls.Add(this.m_labelTimerInit, 3, 2);
            this.tableLayoutPanel1.Controls.Add(this.m_lblTimerIdle, 2, 3);
            this.tableLayoutPanel1.Controls.Add(this.m_labelTimerIdle, 3, 3);
            this.tableLayoutPanel1.Controls.Add(this.m_lblTimerSetup, 2, 4);
            this.tableLayoutPanel1.Controls.Add(this.m_labelTimerSetup, 3, 4);
            this.tableLayoutPanel1.Controls.Add(this.m_lblTimerRun, 2, 5);
            this.tableLayoutPanel1.Controls.Add(this.m_labelTimerRun, 3, 5);
            this.tableLayoutPanel1.Controls.Add(this.m_lblTimerAlarm, 2, 6);
            this.tableLayoutPanel1.Controls.Add(this.m_labelTimerAlarm, 3, 6);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel1.Margin = new System.Windows.Forms.Padding(0);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 8;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 1.01241F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 16.3292F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 16.3292F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 16.3292F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 16.3292F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 16.3292F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 16.3292F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 1.01241F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(336, 215);
            this.tableLayoutPanel1.TabIndex = 1;
            // 
            // _tableLayoutPanel_ActionState
            // 
            this._tableLayoutPanel_ActionState.ColumnCount = 2;
            this._tableLayoutPanel_ActionState.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this._tableLayoutPanel_ActionState.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this._tableLayoutPanel_ActionState.Controls.Add(this.m_dgViewActionPort, 1, 0);
            this._tableLayoutPanel_ActionState.Controls.Add(this.m_dgViewAction, 0, 0);
            this._tableLayoutPanel_ActionState.Dock = System.Windows.Forms.DockStyle.Fill;
            this._tableLayoutPanel_ActionState.Location = new System.Drawing.Point(5, 34);
            this._tableLayoutPanel_ActionState.Name = "_tableLayoutPanel_ActionState";
            this._tableLayoutPanel_ActionState.RowCount = 1;
            this._tableLayoutPanel_ActionState.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this._tableLayoutPanel_ActionState.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this._tableLayoutPanel_ActionState.Size = new System.Drawing.Size(788, 176);
            this._tableLayoutPanel_ActionState.TabIndex = 0;
            // 
            // Operation_StateMonitor
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.Controls.Add(this.m_groupMonitoring);
            this.Margin = new System.Windows.Forms.Padding(0);
            this.Name = "Operation_StateMonitor";
            this.Size = new System.Drawing.Size(1140, 900);
            ((System.ComponentModel.ISupportInitialize)(this.m_dgViewTask)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.m_dgViewAction)).EndInit();
            this.m_groupMonitoring.ResumeLayout(false);
            this.m_groupTaskState.ResumeLayout(false);
            this.m_groupActionState.ResumeLayout(false);
            this.sys3GroupBox1.ResumeLayout(false);
            this.sys3GroupBox2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.m_dgViewLinkedAction)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.m_dgViewDerefAction)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.m_dgViewActionPort)).EndInit();
            this._tableLayoutPanel_Main.ResumeLayout(false);
            this._tableLayoutPanel_LeftBottom.ResumeLayout(false);
            this.tableLayoutPanel1.ResumeLayout(false);
            this._tableLayoutPanel_ActionState.ResumeLayout(false);
            this.ResumeLayout(false);

		}

		#endregion

		private Sys3Controls.Sys3DoubleBufferedDataGridView m_dgViewTask;
		private Sys3Controls.Sys3LedLabel m_ledAlarm;
		private Sys3Controls.Sys3LedLabel m_ledRun;
		private Sys3Controls.Sys3LedLabel m_ledSetup;
		private Sys3Controls.Sys3LedLabel m_ledIdle;
		private Sys3Controls.Sys3LedLabel m_ledInit;
		private Sys3Controls.Sys3LedLabel m_ledPause;
		private Sys3Controls.Sys3DoubleBufferedDataGridView m_dgViewAction;
		private Sys3Controls.Sys3GroupBoxContainer m_groupMonitoring;
        private Sys3Controls.Sys3GroupBoxContainer m_groupTaskState;
        private Sys3Controls.Sys3GroupBoxContainer m_groupActionState;
        private Sys3Controls.Sys3GroupBoxContainer sys3GroupBox1;
        private Sys3Controls.Sys3GroupBoxContainer sys3GroupBox2;
		private Sys3Controls.Sys3Label m_lblTimerPause;
		private Sys3Controls.Sys3Label m_lblTimerInit;
		private Sys3Controls.Sys3Label m_lblTimerIdle;
		private Sys3Controls.Sys3Label m_lblTimerSetup;
		private Sys3Controls.Sys3Label m_lblTimerRun;
		private Sys3Controls.Sys3Label m_lblTimerAlarm;
		private Sys3Controls.Sys3Label m_labelTimerPause;
		private Sys3Controls.Sys3Label m_labelTimerInit;
		private Sys3Controls.Sys3Label m_labelTimerIdle;
		private Sys3Controls.Sys3Label m_labelTimerSetup;
		private Sys3Controls.Sys3Label m_labelTimerRun;
		private Sys3Controls.Sys3Label m_labelTimerAlarm;
		private Sys3Controls.Sys3Label m_lblMainThread;
		private Sys3Controls.Sys3Label m_lblObserver;
		private Sys3Controls.Sys3Label m_lblFileIO;
		private Sys3Controls.Sys3Label m_lblDigitalIO;
		private Sys3Controls.Sys3Label m_lblAnalogIO;
		private Sys3Controls.Sys3Label m_lblMotion;
		private Sys3Controls.Sys3Label m_lblGathering;
		private Sys3Controls.Sys3Label m_lblCommunication;
		private Sys3Controls.Sys3Label m_lblEtcInstance;
		private Sys3Controls.Sys3Label m_labelMainInterval;
		private Sys3Controls.Sys3Label m_labelMainMax;
		private Sys3Controls.Sys3Label m_labelMainAverage;
		private Sys3Controls.Sys3Label m_labelObserverInterval;
		private Sys3Controls.Sys3Label m_labelObserverMax;
		private Sys3Controls.Sys3Label m_labelObserverAverage;
		private Sys3Controls.Sys3Label m_labelFileIOInterval;
		private Sys3Controls.Sys3Label m_labelFileIOMax;
		private Sys3Controls.Sys3Label m_labelFileIOAverage;
		private Sys3Controls.Sys3Label m_labelDegitalIOInterval;
		private Sys3Controls.Sys3Label m_labelDegitalIOMax;
		private Sys3Controls.Sys3Label m_labelDegitalIOAverage;
		private Sys3Controls.Sys3Label m_labelAnalogIOInterval;
		private Sys3Controls.Sys3Label m_labelAnalogIOMax;
		private Sys3Controls.Sys3Label m_labelAnalogIOAverage;
		private Sys3Controls.Sys3Label m_labelMotionInterval;
		private Sys3Controls.Sys3Label m_labelMotionMax;
		private Sys3Controls.Sys3Label m_labelMotionAverage;
		private Sys3Controls.Sys3Label m_labelGatheringInterval;
		private Sys3Controls.Sys3Label m_labelGatheringMax;
		private Sys3Controls.Sys3Label m_labelGatheringAverage;
		private Sys3Controls.Sys3Label m_labelCommunicationInterval;
		private Sys3Controls.Sys3Label m_labelCommunicationMax;
		private Sys3Controls.Sys3Label m_labelCommunicationAverage;
		private Sys3Controls.Sys3Label m_labelEtcInstanceInterval;
		private Sys3Controls.Sys3Label m_labelEtcInstanceMax;
		private Sys3Controls.Sys3Label m_labelEtcInstanceAverage;
		private Sys3Controls.Sys3Label m_labelMainProcess;
		private Sys3Controls.Sys3Label m_labelObserverProcess;
		private Sys3Controls.Sys3Label m_labelFileIOProcess;
		private Sys3Controls.Sys3Label m_labelDegitalIOProcess;
		private Sys3Controls.Sys3Label m_labelAnalogIOProcess;
		private Sys3Controls.Sys3Label m_labelMotionProcess;
		private Sys3Controls.Sys3Label m_labelGatheringProcess;
		private Sys3Controls.Sys3Label m_labelCommunicationProcess;
		private Sys3Controls.Sys3Label m_labelEtcInstanceProcess;
		private System.Windows.Forms.DataGridViewTextBoxColumn INDEX;
		private System.Windows.Forms.DataGridViewTextBoxColumn MONITOR;
		private System.Windows.Forms.DataGridViewTextBoxColumn ID;
		private System.Windows.Forms.DataGridViewTextBoxColumn GRADE;
		private System.Windows.Forms.DataGridViewTextBoxColumn BESEQ;
		private System.Windows.Forms.DataGridViewTextBoxColumn AESEQ;
		private System.Windows.Forms.DataGridViewTextBoxColumn Flow;
		private Sys3Controls.Sys3DoubleBufferedDataGridView m_dgViewLinkedAction;
		private Sys3Controls.Sys3DoubleBufferedDataGridView m_dgViewDerefAction;
		private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn3;
		private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn2;
		private System.Windows.Forms.DataGridViewTextBoxColumn TIMER;
		private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn12;
		private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn13;
		private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn14;
		private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn4;
		private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn5;
		private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn6;
		private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn7;
		private Sys3Controls.Sys3DoubleBufferedDataGridView m_dgViewActionPort;
		private System.Windows.Forms.DataGridViewTextBoxColumn ColumnTask;
		private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn1;
		private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn8;
        private System.Windows.Forms.TableLayoutPanel _tableLayoutPanel_Main;
        private System.Windows.Forms.TableLayoutPanel _tableLayoutPanel_LeftBottom;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.TableLayoutPanel _tableLayoutPanel_ActionState;
	}
}
