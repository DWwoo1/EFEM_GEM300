namespace FrameOfSystem3.Views.Config
{
    partial class Config_Motion
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
			System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle5 = new System.Windows.Forms.DataGridViewCellStyle();
			System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
			System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle3 = new System.Windows.Forms.DataGridViewCellStyle();
			this.m_dgViewList = new Sys3Controls.Sys3DoubleBufferedDataGridView();
			this.ID = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.PASSWORD = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.TARGET = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.ENABLE = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.AUTHORITY = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.CONDITION = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.Column2 = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.BLINK = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.Led = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.MOTION = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.m_panel = new System.Windows.Forms.Panel();
			this.m_GroupBox = new Sys3Controls.Sys3GroupBoxContainer();
			this.m_ProgressBar = new Sys3Controls.Sys3ProgressBar();
			this.m_btnExtend = new Sys3Controls.Sys3button();
			this.m_btnRemove = new Sys3Controls.Sys3button();
			this.m_btnAdd = new Sys3Controls.Sys3button();
			this.m_groupList = new Sys3Controls.Sys3GroupBoxContainer();
			this.m_groupConfiguration = new Sys3Controls.Sys3GroupBoxContainer();
			this._tableLayoutPanel_Configuration = new System.Windows.Forms.TableLayoutPanel();
			this.m_tabState = new Sys3Controls.Sys3button();
			this.m_tabGeneral = new Sys3Controls.Sys3button();
			this.m_tabLimit = new Sys3Controls.Sys3button();
			this.m_tabSpeed = new Sys3Controls.Sys3button();
			this.m_tabGantry = new Sys3Controls.Sys3button();
			this.m_tabHome = new Sys3Controls.Sys3button();
			this.m_lblIndex = new Sys3Controls.Sys3Label();
			this.m_labelTag = new Sys3Controls.Sys3Label();
			this.m_lblTag = new Sys3Controls.Sys3Label();
			this.m_labelIndex = new Sys3Controls.Sys3Label();
			this.m_labelTarget = new Sys3Controls.Sys3Label();
			this.m_labelName = new Sys3Controls.Sys3Label();
			this.m_lblName = new Sys3Controls.Sys3Label();
			this.m_lblTarget = new Sys3Controls.Sys3Label();
			this.m_lblPower = new Sys3Controls.Sys3Label();
			this.m_labelPower = new Sys3Controls.Sys3Label();
			this.m_btnHome = new Sys3Controls.Sys3button();
			this.m_btnAllStop = new Sys3Controls.Sys3button();
			this.m_btnDisable = new Sys3Controls.Sys3button();
			this.m_btnStop = new Sys3Controls.Sys3button();
			this.m_btnEnable = new Sys3Controls.Sys3button();
			this.m_btnMotorOn = new Sys3Controls.Sys3button();
			this.m_btnMotorOff = new Sys3Controls.Sys3button();
			this.m_btnReset = new Sys3Controls.Sys3button();
			this.m_btnAllReset = new Sys3Controls.Sys3button();
			this.m_btnAllMotorOff = new Sys3Controls.Sys3button();
			this.m_btnAllMotorOn = new Sys3Controls.Sys3button();
			this.m_labelScroll = new Sys3Controls.Sys3Label();
			this._tableLayoutPanel_Main = new System.Windows.Forms.TableLayoutPanel();
			this._flowLayoutPanel_AddRemove = new System.Windows.Forms.FlowLayoutPanel();
			this.lbl_Filtering = new Sys3Controls.Sys3Label();
			((System.ComponentModel.ISupportInitialize)(this.m_dgViewList)).BeginInit();
			this.m_panel.SuspendLayout();
			this.m_GroupBox.SuspendLayout();
			this.m_groupList.SuspendLayout();
			this.m_groupConfiguration.SuspendLayout();
			this._tableLayoutPanel_Configuration.SuspendLayout();
			this._tableLayoutPanel_Main.SuspendLayout();
			this._flowLayoutPanel_AddRemove.SuspendLayout();
			this.SuspendLayout();
			// 
			// m_dgViewList
			// 
			this.m_dgViewList.AllowUserToAddRows = false;
			this.m_dgViewList.AllowUserToDeleteRows = false;
			this.m_dgViewList.AllowUserToResizeColumns = false;
			this.m_dgViewList.AllowUserToResizeRows = false;
			this.m_dgViewList.BackgroundColor = System.Drawing.Color.White;
			this.m_dgViewList.CellBorderStyle = System.Windows.Forms.DataGridViewCellBorderStyle.Raised;
			this.m_dgViewList.ColumnHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.Single;
			dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
			dataGridViewCellStyle1.BackColor = System.Drawing.Color.LightGray;
			dataGridViewCellStyle1.Font = new System.Drawing.Font("맑은 고딕", 11F, System.Drawing.FontStyle.Bold);
			dataGridViewCellStyle1.ForeColor = System.Drawing.Color.Black;
			dataGridViewCellStyle1.SelectionBackColor = System.Drawing.Color.White;
			dataGridViewCellStyle1.SelectionForeColor = System.Drawing.Color.Black;
			dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
			this.m_dgViewList.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
			this.m_dgViewList.ColumnHeadersHeight = 30;
			this.m_dgViewList.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
			this.m_dgViewList.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.ID,
            this.PASSWORD,
            this.TARGET,
            this.ENABLE,
            this.AUTHORITY,
            this.CONDITION,
            this.Column2,
            this.BLINK,
            this.Led,
            this.MOTION});
			dataGridViewCellStyle4.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
			dataGridViewCellStyle4.BackColor = System.Drawing.SystemColors.Window;
			dataGridViewCellStyle4.Font = new System.Drawing.Font("맑은 고딕", 10F, System.Drawing.FontStyle.Bold);
			dataGridViewCellStyle4.ForeColor = System.Drawing.SystemColors.ControlText;
			dataGridViewCellStyle4.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(120)))), ((int)(((byte)(130)))), ((int)(((byte)(150)))));
			dataGridViewCellStyle4.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
			dataGridViewCellStyle4.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
			this.m_dgViewList.DefaultCellStyle = dataGridViewCellStyle4;
			this.m_dgViewList.Dock = System.Windows.Forms.DockStyle.Fill;
			this.m_dgViewList.EnableHeadersVisualStyles = false;
			this.m_dgViewList.GridColor = System.Drawing.Color.FromArgb(((int)(((byte)(120)))), ((int)(((byte)(130)))), ((int)(((byte)(150)))));
			this.m_dgViewList.Location = new System.Drawing.Point(3, 42);
			this.m_dgViewList.MultiSelect = false;
			this.m_dgViewList.Name = "m_dgViewList";
			this.m_dgViewList.ReadOnly = true;
			this.m_dgViewList.RowHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.None;
			dataGridViewCellStyle5.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
			dataGridViewCellStyle5.BackColor = System.Drawing.Color.White;
			dataGridViewCellStyle5.Font = new System.Drawing.Font("맑은 고딕", 11F);
			dataGridViewCellStyle5.ForeColor = System.Drawing.SystemColors.WindowText;
			dataGridViewCellStyle5.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(120)))), ((int)(((byte)(130)))), ((int)(((byte)(150)))));
			dataGridViewCellStyle5.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
			dataGridViewCellStyle5.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
			this.m_dgViewList.RowHeadersDefaultCellStyle = dataGridViewCellStyle5;
			this.m_dgViewList.RowHeadersVisible = false;
			this.m_dgViewList.RowHeadersWidthSizeMode = System.Windows.Forms.DataGridViewRowHeadersWidthSizeMode.DisableResizing;
			this.m_dgViewList.RowTemplate.Height = 23;
			this.m_dgViewList.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
			this.m_dgViewList.Size = new System.Drawing.Size(1150, 146);
			this.m_dgViewList.TabIndex = 1150;
			this.m_dgViewList.CellClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.Click_Dgview);
			this.m_dgViewList.CellDoubleClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.Click_Dgview);
			// 
			// ID
			// 
			this.ID.Frozen = true;
			this.ID.HeaderText = "INDEX";
			this.ID.MaxInputLength = 20;
			this.ID.Name = "ID";
			this.ID.ReadOnly = true;
			this.ID.Resizable = System.Windows.Forms.DataGridViewTriState.False;
			this.ID.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
			this.ID.Width = 65;
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
			// TARGET
			// 
			this.TARGET.HeaderText = "TARGET";
			this.TARGET.Name = "TARGET";
			this.TARGET.ReadOnly = true;
			this.TARGET.Resizable = System.Windows.Forms.DataGridViewTriState.False;
			this.TARGET.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
			this.TARGET.Width = 65;
			// 
			// ENABLE
			// 
			this.ENABLE.HeaderText = "ENABLE";
			this.ENABLE.Name = "ENABLE";
			this.ENABLE.ReadOnly = true;
			this.ENABLE.Resizable = System.Windows.Forms.DataGridViewTriState.False;
			this.ENABLE.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
			this.ENABLE.Width = 80;
			// 
			// AUTHORITY
			// 
			dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
			this.AUTHORITY.DefaultCellStyle = dataGridViewCellStyle2;
			this.AUTHORITY.HeaderText = "COMMAND";
			this.AUTHORITY.MaxInputLength = 20;
			this.AUTHORITY.Name = "AUTHORITY";
			this.AUTHORITY.ReadOnly = true;
			this.AUTHORITY.Resizable = System.Windows.Forms.DataGridViewTriState.False;
			this.AUTHORITY.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
			this.AUTHORITY.Width = 113;
			// 
			// CONDITION
			// 
			dataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
			this.CONDITION.DefaultCellStyle = dataGridViewCellStyle3;
			this.CONDITION.HeaderText = "ACTUAL";
			this.CONDITION.Name = "CONDITION";
			this.CONDITION.ReadOnly = true;
			this.CONDITION.Resizable = System.Windows.Forms.DataGridViewTriState.False;
			this.CONDITION.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
			this.CONDITION.Width = 113;
			// 
			// Column2
			// 
			this.Column2.HeaderText = "ALARM";
			this.Column2.Name = "Column2";
			this.Column2.ReadOnly = true;
			this.Column2.Resizable = System.Windows.Forms.DataGridViewTriState.False;
			this.Column2.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
			// 
			// BLINK
			// 
			this.BLINK.HeaderText = "MOTOR ON";
			this.BLINK.Name = "BLINK";
			this.BLINK.ReadOnly = true;
			this.BLINK.Resizable = System.Windows.Forms.DataGridViewTriState.False;
			this.BLINK.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
			// 
			// Led
			// 
			this.Led.HeaderText = "HOME END";
			this.Led.Name = "Led";
			this.Led.ReadOnly = true;
			this.Led.Resizable = System.Windows.Forms.DataGridViewTriState.False;
			this.Led.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
			// 
			// MOTION
			// 
			this.MOTION.HeaderText = "DONE";
			this.MOTION.Name = "MOTION";
			this.MOTION.ReadOnly = true;
			this.MOTION.Resizable = System.Windows.Forms.DataGridViewTriState.False;
			this.MOTION.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
			// 
			// m_panel
			// 
			this._tableLayoutPanel_Configuration.SetColumnSpan(this.m_panel, 9);
			this.m_panel.Controls.Add(this.m_GroupBox);
			this.m_panel.Dock = System.Windows.Forms.DockStyle.Fill;
			this.m_panel.Location = new System.Drawing.Point(0, 120);
			this.m_panel.Margin = new System.Windows.Forms.Padding(0);
			this.m_panel.Name = "m_panel";
			this.m_panel.Size = new System.Drawing.Size(1156, 411);
			this.m_panel.TabIndex = 1154;
			// 
			// m_GroupBox
			// 
			this.m_GroupBox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)));
			this.m_GroupBox.BackGroundColor = System.Drawing.Color.WhiteSmoke;
			this.m_GroupBox.Controls.Add(this.m_ProgressBar);
			this.m_GroupBox.EdgeBorderStroke = 2;
			this.m_GroupBox.EdgeRadius = 2;
			this.m_GroupBox.LabelFont = new System.Drawing.Font("맑은 고딕", 15F);
			this.m_GroupBox.LabelGradientColorFirst = System.Drawing.Color.FromArgb(((int)(((byte)(120)))), ((int)(((byte)(130)))), ((int)(((byte)(150)))));
			this.m_GroupBox.LabelGradientColorSecond = System.Drawing.Color.FromArgb(((int)(((byte)(90)))), ((int)(((byte)(90)))), ((int)(((byte)(100)))));
			this.m_GroupBox.LabelHeight = 50;
			this.m_GroupBox.LabelTextColor = System.Drawing.Color.White;
			this.m_GroupBox.Location = new System.Drawing.Point(295, 20);
			this.m_GroupBox.Name = "m_GroupBox";
			this.m_GroupBox.Padding = new System.Windows.Forms.Padding(0, 40, 0, 5);
			this.m_GroupBox.Size = new System.Drawing.Size(559, 142);
			this.m_GroupBox.TabIndex = 2;
			this.m_GroupBox.TabStop = false;
			this.m_GroupBox.Text = "ALL MOTERS ON ...";
			this.m_GroupBox.TextAlign = Sys3Controls.EN_TEXTALIGN.MIDDLE_CENTER;
			this.m_GroupBox.ThemeIndex = 0;
			this.m_GroupBox.UseLabelBorder = true;
			this.m_GroupBox.UseTitle = true;
			// 
			// m_ProgressBar
			// 
			this.m_ProgressBar.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)));
			this.m_ProgressBar.BackGroundColor = System.Drawing.Color.White;
			this.m_ProgressBar.BorderStroke = 1;
			this.m_ProgressBar.FirstColor = System.Drawing.Color.Green;
			this.m_ProgressBar.GageCount = 15;
			this.m_ProgressBar.LabelWidth = 80;
			this.m_ProgressBar.Location = new System.Drawing.Point(4, 54);
			this.m_ProgressBar.MaxTickCount = ((uint)(100u));
			this.m_ProgressBar.MinTickCount = ((uint)(1u));
			this.m_ProgressBar.Name = "m_ProgressBar";
			this.m_ProgressBar.NormalGageColor = System.Drawing.Color.LightGray;
			this.m_ProgressBar.OffSet = 3;
			this.m_ProgressBar.SecondColor = System.Drawing.Color.Red;
			this.m_ProgressBar.Size = new System.Drawing.Size(548, 83);
			this.m_ProgressBar.TabIndex = 0;
			this.m_ProgressBar.Tick = ((uint)(50u));
			this.m_ProgressBar.TickFont = new System.Drawing.Font("맑은 고딕", 14F, System.Drawing.FontStyle.Bold);
			this.m_ProgressBar.TickFontColor = System.Drawing.Color.Black;
			this.m_ProgressBar.UseBorder = true;
			this.m_ProgressBar.UseControlBorder = true;
			this.m_ProgressBar.UseLinearGradientMode = true;
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
			this.m_btnExtend.Location = new System.Drawing.Point(276, 0);
			this.m_btnExtend.MainFont = new System.Drawing.Font("맑은 고딕", 10F, System.Drawing.FontStyle.Bold);
			this.m_btnExtend.MainFontColor = System.Drawing.Color.Black;
			this.m_btnExtend.Margin = new System.Windows.Forms.Padding(0);
			this.m_btnExtend.Name = "m_btnExtend";
			this.m_btnExtend.Size = new System.Drawing.Size(41, 41);
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
			this.m_btnRemove.Location = new System.Drawing.Point(138, 0);
			this.m_btnRemove.MainFont = new System.Drawing.Font("맑은 고딕", 10F, System.Drawing.FontStyle.Bold);
			this.m_btnRemove.MainFontColor = System.Drawing.Color.Black;
			this.m_btnRemove.Margin = new System.Windows.Forms.Padding(0);
			this.m_btnRemove.Name = "m_btnRemove";
			this.m_btnRemove.Size = new System.Drawing.Size(138, 41);
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
			this.m_btnRemove.Click += new System.EventHandler(this.Click_AddOrRemove);
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
			this.m_btnAdd.Size = new System.Drawing.Size(138, 41);
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
			this.m_btnAdd.Click += new System.EventHandler(this.Click_AddOrRemove);
			// 
			// m_groupList
			// 
			this.m_groupList.BackGroundColor = System.Drawing.Color.WhiteSmoke;
			this._tableLayoutPanel_Main.SetColumnSpan(this.m_groupList, 10);
			this.m_groupList.Controls.Add(this.lbl_Filtering);
			this.m_groupList.Controls.Add(this.m_dgViewList);
			this.m_groupList.Dock = System.Windows.Forms.DockStyle.Fill;
			this.m_groupList.EdgeBorderStroke = 1;
			this.m_groupList.EdgeRadius = 0;
			this.m_groupList.LabelFont = new System.Drawing.Font("맑은 고딕", 10F, System.Drawing.FontStyle.Bold);
			this.m_groupList.LabelGradientColorFirst = System.Drawing.Color.FromArgb(((int)(((byte)(208)))), ((int)(((byte)(208)))), ((int)(((byte)(208)))));
			this.m_groupList.LabelGradientColorSecond = System.Drawing.Color.FromArgb(((int)(((byte)(240)))), ((int)(((byte)(240)))), ((int)(((byte)(240)))));
			this.m_groupList.LabelHeight = 40;
			this.m_groupList.LabelTextColor = System.Drawing.Color.Black;
			this.m_groupList.Location = new System.Drawing.Point(0, 0);
			this.m_groupList.Margin = new System.Windows.Forms.Padding(0);
			this.m_groupList.Name = "m_groupList";
			this.m_groupList.Padding = new System.Windows.Forms.Padding(3, 28, 3, 3);
			this.m_groupList.Size = new System.Drawing.Size(1156, 191);
			this.m_groupList.TabIndex = 1349;
			this.m_groupList.TabStop = false;
			this.m_groupList.Text = "MOTION LIST";
			this.m_groupList.TextAlign = Sys3Controls.EN_TEXTALIGN.MIDDLE_CENTER;
			this.m_groupList.ThemeIndex = 0;
			this.m_groupList.UseLabelBorder = true;
			this.m_groupList.UseTitle = true;
			// 
			// m_groupConfiguration
			// 
			this.m_groupConfiguration.BackGroundColor = System.Drawing.Color.WhiteSmoke;
			this._tableLayoutPanel_Main.SetColumnSpan(this.m_groupConfiguration, 10);
			this.m_groupConfiguration.Controls.Add(this._tableLayoutPanel_Configuration);
			this.m_groupConfiguration.Dock = System.Windows.Forms.DockStyle.Fill;
			this.m_groupConfiguration.EdgeBorderStroke = 1;
			this.m_groupConfiguration.EdgeRadius = 0;
			this.m_groupConfiguration.LabelFont = new System.Drawing.Font("맑은 고딕", 10F, System.Drawing.FontStyle.Bold);
			this.m_groupConfiguration.LabelGradientColorFirst = System.Drawing.Color.FromArgb(((int)(((byte)(208)))), ((int)(((byte)(208)))), ((int)(((byte)(208)))));
			this.m_groupConfiguration.LabelGradientColorSecond = System.Drawing.Color.FromArgb(((int)(((byte)(240)))), ((int)(((byte)(240)))), ((int)(((byte)(240)))));
			this.m_groupConfiguration.LabelHeight = 30;
			this.m_groupConfiguration.LabelTextColor = System.Drawing.Color.Black;
			this.m_groupConfiguration.Location = new System.Drawing.Point(0, 217);
			this.m_groupConfiguration.Margin = new System.Windows.Forms.Padding(0);
			this.m_groupConfiguration.Name = "m_groupConfiguration";
			this.m_groupConfiguration.Padding = new System.Windows.Forms.Padding(0, 18, 0, 0);
			this.m_groupConfiguration.Size = new System.Drawing.Size(1156, 563);
			this.m_groupConfiguration.TabIndex = 1350;
			this.m_groupConfiguration.TabStop = false;
			this.m_groupConfiguration.Text = "CONFIGURATION";
			this.m_groupConfiguration.TextAlign = Sys3Controls.EN_TEXTALIGN.MIDDLE_CENTER;
			this.m_groupConfiguration.ThemeIndex = 0;
			this.m_groupConfiguration.UseLabelBorder = true;
			this.m_groupConfiguration.UseTitle = true;
			// 
			// _tableLayoutPanel_Configuration
			// 
			this._tableLayoutPanel_Configuration.BackColor = System.Drawing.Color.WhiteSmoke;
			this._tableLayoutPanel_Configuration.ColumnCount = 9;
			this._tableLayoutPanel_Configuration.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 11.11111F));
			this._tableLayoutPanel_Configuration.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 11.11111F));
			this._tableLayoutPanel_Configuration.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 11.11111F));
			this._tableLayoutPanel_Configuration.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 11.11111F));
			this._tableLayoutPanel_Configuration.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 11.11111F));
			this._tableLayoutPanel_Configuration.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 11.11111F));
			this._tableLayoutPanel_Configuration.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 11.11111F));
			this._tableLayoutPanel_Configuration.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 11.11111F));
			this._tableLayoutPanel_Configuration.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 11.11111F));
			this._tableLayoutPanel_Configuration.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 20F));
			this._tableLayoutPanel_Configuration.Controls.Add(this.m_tabState, 0, 2);
			this._tableLayoutPanel_Configuration.Controls.Add(this.m_tabGeneral, 1, 2);
			this._tableLayoutPanel_Configuration.Controls.Add(this.m_tabLimit, 2, 2);
			this._tableLayoutPanel_Configuration.Controls.Add(this.m_tabSpeed, 3, 2);
			this._tableLayoutPanel_Configuration.Controls.Add(this.m_panel, 0, 3);
			this._tableLayoutPanel_Configuration.Controls.Add(this.m_tabGantry, 5, 2);
			this._tableLayoutPanel_Configuration.Controls.Add(this.m_tabHome, 4, 2);
			this._tableLayoutPanel_Configuration.Controls.Add(this.m_lblIndex, 0, 0);
			this._tableLayoutPanel_Configuration.Controls.Add(this.m_labelTag, 1, 1);
			this._tableLayoutPanel_Configuration.Controls.Add(this.m_lblTag, 0, 1);
			this._tableLayoutPanel_Configuration.Controls.Add(this.m_labelIndex, 1, 0);
			this._tableLayoutPanel_Configuration.Controls.Add(this.m_labelTarget, 4, 1);
			this._tableLayoutPanel_Configuration.Controls.Add(this.m_labelName, 4, 0);
			this._tableLayoutPanel_Configuration.Controls.Add(this.m_lblName, 3, 0);
			this._tableLayoutPanel_Configuration.Controls.Add(this.m_lblTarget, 3, 1);
			this._tableLayoutPanel_Configuration.Controls.Add(this.m_lblPower, 6, 1);
			this._tableLayoutPanel_Configuration.Controls.Add(this.m_labelPower, 7, 1);
			this._tableLayoutPanel_Configuration.Dock = System.Windows.Forms.DockStyle.Fill;
			this._tableLayoutPanel_Configuration.Location = new System.Drawing.Point(0, 32);
			this._tableLayoutPanel_Configuration.Margin = new System.Windows.Forms.Padding(0);
			this._tableLayoutPanel_Configuration.Name = "_tableLayoutPanel_Configuration";
			this._tableLayoutPanel_Configuration.RowCount = 4;
			this._tableLayoutPanel_Configuration.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 40F));
			this._tableLayoutPanel_Configuration.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 40F));
			this._tableLayoutPanel_Configuration.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 40F));
			this._tableLayoutPanel_Configuration.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
			this._tableLayoutPanel_Configuration.Size = new System.Drawing.Size(1156, 531);
			this._tableLayoutPanel_Configuration.TabIndex = 0;
			// 
			// m_tabState
			// 
			this.m_tabState.BorderWidth = 2;
			this.m_tabState.ButtonClicked = true;
			this.m_tabState.ClickedEmphasizeTextColor = System.Drawing.Color.White;
			this.m_tabState.CustomClickedGradientFirstColor = System.Drawing.Color.White;
			this.m_tabState.CustomClickedGradientSecondColor = System.Drawing.Color.White;
			this.m_tabState.Description = "";
			this.m_tabState.DisabledColor = System.Drawing.Color.DarkGray;
			this.m_tabState.Dock = System.Windows.Forms.DockStyle.Fill;
			this.m_tabState.EdgeRadius = 5;
			this.m_tabState.GradientAngle = 70F;
			this.m_tabState.GradientFirstColor = System.Drawing.Color.DarkBlue;
			this.m_tabState.GradientSecondColor = System.Drawing.Color.DarkBlue;
			this.m_tabState.HoverEmphasizeCustomColor = System.Drawing.Color.White;
			this.m_tabState.ImagePosition = new System.Drawing.Point(7, 7);
			this.m_tabState.ImageSize = new System.Drawing.Point(30, 30);
			this.m_tabState.LoadImage = global::FrameOfSystem3.Properties.Resources.Home_black;
			this.m_tabState.Location = new System.Drawing.Point(0, 80);
			this.m_tabState.MainFont = new System.Drawing.Font("맑은 고딕", 11F, System.Drawing.FontStyle.Bold);
			this.m_tabState.MainFontColor = System.Drawing.Color.White;
			this.m_tabState.Margin = new System.Windows.Forms.Padding(0);
			this.m_tabState.Name = "m_tabState";
			this.m_tabState.Size = new System.Drawing.Size(128, 40);
			this.m_tabState.SubFont = new System.Drawing.Font("맑은 고딕", 8F, System.Drawing.FontStyle.Bold);
			this.m_tabState.SubFontColor = System.Drawing.Color.DarkBlue;
			this.m_tabState.SubText = "STATUS";
			this.m_tabState.TabIndex = 0;
			this.m_tabState.Text = "STATE";
			this.m_tabState.TextAlignMain = Sys3Controls.EN_TEXTALIGN.MIDDLE_CENTER;
			this.m_tabState.TextAlignSub = Sys3Controls.EN_TEXTALIGN.TOP_RIGHT;
			this.m_tabState.ThemeIndex = 0;
			this.m_tabState.UseBorder = false;
			this.m_tabState.UseClickedEmphasizeTextColor = false;
			this.m_tabState.UseCustomizeClickedColor = false;
			this.m_tabState.UseEdge = false;
			this.m_tabState.UseHoverEmphasizeCustomColor = false;
			this.m_tabState.UseImage = false;
			this.m_tabState.UserHoverEmpahsize = false;
			this.m_tabState.UseSubFont = false;
			this.m_tabState.Click += new System.EventHandler(this.Clicked_Panel);
			// 
			// m_tabGeneral
			// 
			this.m_tabGeneral.BorderWidth = 2;
			this.m_tabGeneral.ButtonClicked = false;
			this.m_tabGeneral.ClickedEmphasizeTextColor = System.Drawing.Color.White;
			this.m_tabGeneral.CustomClickedGradientFirstColor = System.Drawing.Color.White;
			this.m_tabGeneral.CustomClickedGradientSecondColor = System.Drawing.Color.White;
			this.m_tabGeneral.Description = "";
			this.m_tabGeneral.DisabledColor = System.Drawing.Color.DarkGray;
			this.m_tabGeneral.Dock = System.Windows.Forms.DockStyle.Fill;
			this.m_tabGeneral.EdgeRadius = 5;
			this.m_tabGeneral.GradientAngle = 70F;
			this.m_tabGeneral.GradientFirstColor = System.Drawing.Color.White;
			this.m_tabGeneral.GradientSecondColor = System.Drawing.Color.White;
			this.m_tabGeneral.HoverEmphasizeCustomColor = System.Drawing.Color.White;
			this.m_tabGeneral.ImagePosition = new System.Drawing.Point(7, 7);
			this.m_tabGeneral.ImageSize = new System.Drawing.Point(30, 30);
			this.m_tabGeneral.LoadImage = global::FrameOfSystem3.Properties.Resources.Home_black;
			this.m_tabGeneral.Location = new System.Drawing.Point(128, 80);
			this.m_tabGeneral.MainFont = new System.Drawing.Font("맑은 고딕", 11F, System.Drawing.FontStyle.Bold);
			this.m_tabGeneral.MainFontColor = System.Drawing.Color.DarkBlue;
			this.m_tabGeneral.Margin = new System.Windows.Forms.Padding(0);
			this.m_tabGeneral.Name = "m_tabGeneral";
			this.m_tabGeneral.Size = new System.Drawing.Size(128, 40);
			this.m_tabGeneral.SubFont = new System.Drawing.Font("맑은 고딕", 8F, System.Drawing.FontStyle.Bold);
			this.m_tabGeneral.SubFontColor = System.Drawing.Color.DarkBlue;
			this.m_tabGeneral.SubText = "STATUS";
			this.m_tabGeneral.TabIndex = 1;
			this.m_tabGeneral.Text = "GENERAL";
			this.m_tabGeneral.TextAlignMain = Sys3Controls.EN_TEXTALIGN.MIDDLE_CENTER;
			this.m_tabGeneral.TextAlignSub = Sys3Controls.EN_TEXTALIGN.TOP_RIGHT;
			this.m_tabGeneral.ThemeIndex = 0;
			this.m_tabGeneral.UseBorder = false;
			this.m_tabGeneral.UseClickedEmphasizeTextColor = false;
			this.m_tabGeneral.UseCustomizeClickedColor = false;
			this.m_tabGeneral.UseEdge = false;
			this.m_tabGeneral.UseHoverEmphasizeCustomColor = false;
			this.m_tabGeneral.UseImage = false;
			this.m_tabGeneral.UserHoverEmpahsize = false;
			this.m_tabGeneral.UseSubFont = false;
			this.m_tabGeneral.Click += new System.EventHandler(this.Clicked_Panel);
			// 
			// m_tabLimit
			// 
			this.m_tabLimit.BorderWidth = 2;
			this.m_tabLimit.ButtonClicked = false;
			this.m_tabLimit.ClickedEmphasizeTextColor = System.Drawing.Color.White;
			this.m_tabLimit.CustomClickedGradientFirstColor = System.Drawing.Color.White;
			this.m_tabLimit.CustomClickedGradientSecondColor = System.Drawing.Color.White;
			this.m_tabLimit.Description = "";
			this.m_tabLimit.DisabledColor = System.Drawing.Color.DarkGray;
			this.m_tabLimit.Dock = System.Windows.Forms.DockStyle.Fill;
			this.m_tabLimit.EdgeRadius = 5;
			this.m_tabLimit.GradientAngle = 70F;
			this.m_tabLimit.GradientFirstColor = System.Drawing.Color.White;
			this.m_tabLimit.GradientSecondColor = System.Drawing.Color.White;
			this.m_tabLimit.HoverEmphasizeCustomColor = System.Drawing.Color.White;
			this.m_tabLimit.ImagePosition = new System.Drawing.Point(7, 7);
			this.m_tabLimit.ImageSize = new System.Drawing.Point(30, 30);
			this.m_tabLimit.LoadImage = global::FrameOfSystem3.Properties.Resources.Home_black;
			this.m_tabLimit.Location = new System.Drawing.Point(256, 80);
			this.m_tabLimit.MainFont = new System.Drawing.Font("맑은 고딕", 11F, System.Drawing.FontStyle.Bold);
			this.m_tabLimit.MainFontColor = System.Drawing.Color.DarkBlue;
			this.m_tabLimit.Margin = new System.Windows.Forms.Padding(0);
			this.m_tabLimit.Name = "m_tabLimit";
			this.m_tabLimit.Size = new System.Drawing.Size(128, 40);
			this.m_tabLimit.SubFont = new System.Drawing.Font("맑은 고딕", 8F, System.Drawing.FontStyle.Bold);
			this.m_tabLimit.SubFontColor = System.Drawing.Color.DarkBlue;
			this.m_tabLimit.SubText = "STATUS";
			this.m_tabLimit.TabIndex = 2;
			this.m_tabLimit.Text = "LIMIT";
			this.m_tabLimit.TextAlignMain = Sys3Controls.EN_TEXTALIGN.MIDDLE_CENTER;
			this.m_tabLimit.TextAlignSub = Sys3Controls.EN_TEXTALIGN.TOP_RIGHT;
			this.m_tabLimit.ThemeIndex = 0;
			this.m_tabLimit.UseBorder = false;
			this.m_tabLimit.UseClickedEmphasizeTextColor = false;
			this.m_tabLimit.UseCustomizeClickedColor = false;
			this.m_tabLimit.UseEdge = false;
			this.m_tabLimit.UseHoverEmphasizeCustomColor = false;
			this.m_tabLimit.UseImage = false;
			this.m_tabLimit.UserHoverEmpahsize = false;
			this.m_tabLimit.UseSubFont = false;
			this.m_tabLimit.Click += new System.EventHandler(this.Clicked_Panel);
			// 
			// m_tabSpeed
			// 
			this.m_tabSpeed.BorderWidth = 2;
			this.m_tabSpeed.ButtonClicked = false;
			this.m_tabSpeed.ClickedEmphasizeTextColor = System.Drawing.Color.White;
			this.m_tabSpeed.CustomClickedGradientFirstColor = System.Drawing.Color.White;
			this.m_tabSpeed.CustomClickedGradientSecondColor = System.Drawing.Color.White;
			this.m_tabSpeed.Description = "";
			this.m_tabSpeed.DisabledColor = System.Drawing.Color.DarkGray;
			this.m_tabSpeed.Dock = System.Windows.Forms.DockStyle.Fill;
			this.m_tabSpeed.EdgeRadius = 5;
			this.m_tabSpeed.GradientAngle = 70F;
			this.m_tabSpeed.GradientFirstColor = System.Drawing.Color.White;
			this.m_tabSpeed.GradientSecondColor = System.Drawing.Color.White;
			this.m_tabSpeed.HoverEmphasizeCustomColor = System.Drawing.Color.White;
			this.m_tabSpeed.ImagePosition = new System.Drawing.Point(7, 7);
			this.m_tabSpeed.ImageSize = new System.Drawing.Point(30, 30);
			this.m_tabSpeed.LoadImage = global::FrameOfSystem3.Properties.Resources.Home_black;
			this.m_tabSpeed.Location = new System.Drawing.Point(384, 80);
			this.m_tabSpeed.MainFont = new System.Drawing.Font("맑은 고딕", 11F, System.Drawing.FontStyle.Bold);
			this.m_tabSpeed.MainFontColor = System.Drawing.Color.DarkBlue;
			this.m_tabSpeed.Margin = new System.Windows.Forms.Padding(0);
			this.m_tabSpeed.Name = "m_tabSpeed";
			this.m_tabSpeed.Size = new System.Drawing.Size(128, 40);
			this.m_tabSpeed.SubFont = new System.Drawing.Font("맑은 고딕", 8F, System.Drawing.FontStyle.Bold);
			this.m_tabSpeed.SubFontColor = System.Drawing.Color.DarkBlue;
			this.m_tabSpeed.SubText = "STATUS";
			this.m_tabSpeed.TabIndex = 3;
			this.m_tabSpeed.Text = "SPEED";
			this.m_tabSpeed.TextAlignMain = Sys3Controls.EN_TEXTALIGN.MIDDLE_CENTER;
			this.m_tabSpeed.TextAlignSub = Sys3Controls.EN_TEXTALIGN.TOP_RIGHT;
			this.m_tabSpeed.ThemeIndex = 0;
			this.m_tabSpeed.UseBorder = false;
			this.m_tabSpeed.UseClickedEmphasizeTextColor = false;
			this.m_tabSpeed.UseCustomizeClickedColor = false;
			this.m_tabSpeed.UseEdge = false;
			this.m_tabSpeed.UseHoverEmphasizeCustomColor = false;
			this.m_tabSpeed.UseImage = false;
			this.m_tabSpeed.UserHoverEmpahsize = false;
			this.m_tabSpeed.UseSubFont = false;
			this.m_tabSpeed.Click += new System.EventHandler(this.Clicked_Panel);
			// 
			// m_tabGantry
			// 
			this.m_tabGantry.BorderWidth = 2;
			this.m_tabGantry.ButtonClicked = false;
			this.m_tabGantry.ClickedEmphasizeTextColor = System.Drawing.Color.White;
			this.m_tabGantry.CustomClickedGradientFirstColor = System.Drawing.Color.White;
			this.m_tabGantry.CustomClickedGradientSecondColor = System.Drawing.Color.White;
			this.m_tabGantry.Description = "";
			this.m_tabGantry.DisabledColor = System.Drawing.Color.DarkGray;
			this.m_tabGantry.Dock = System.Windows.Forms.DockStyle.Fill;
			this.m_tabGantry.EdgeRadius = 5;
			this.m_tabGantry.GradientAngle = 70F;
			this.m_tabGantry.GradientFirstColor = System.Drawing.Color.White;
			this.m_tabGantry.GradientSecondColor = System.Drawing.Color.White;
			this.m_tabGantry.HoverEmphasizeCustomColor = System.Drawing.Color.White;
			this.m_tabGantry.ImagePosition = new System.Drawing.Point(7, 7);
			this.m_tabGantry.ImageSize = new System.Drawing.Point(30, 30);
			this.m_tabGantry.LoadImage = global::FrameOfSystem3.Properties.Resources.Home_black;
			this.m_tabGantry.Location = new System.Drawing.Point(640, 80);
			this.m_tabGantry.MainFont = new System.Drawing.Font("맑은 고딕", 11F, System.Drawing.FontStyle.Bold);
			this.m_tabGantry.MainFontColor = System.Drawing.Color.DarkBlue;
			this.m_tabGantry.Margin = new System.Windows.Forms.Padding(0);
			this.m_tabGantry.Name = "m_tabGantry";
			this.m_tabGantry.Size = new System.Drawing.Size(128, 40);
			this.m_tabGantry.SubFont = new System.Drawing.Font("맑은 고딕", 8F, System.Drawing.FontStyle.Bold);
			this.m_tabGantry.SubFontColor = System.Drawing.Color.DarkBlue;
			this.m_tabGantry.SubText = "STATUS";
			this.m_tabGantry.TabIndex = 5;
			this.m_tabGantry.Text = "GANTRY";
			this.m_tabGantry.TextAlignMain = Sys3Controls.EN_TEXTALIGN.MIDDLE_CENTER;
			this.m_tabGantry.TextAlignSub = Sys3Controls.EN_TEXTALIGN.TOP_RIGHT;
			this.m_tabGantry.ThemeIndex = 0;
			this.m_tabGantry.UseBorder = false;
			this.m_tabGantry.UseClickedEmphasizeTextColor = false;
			this.m_tabGantry.UseCustomizeClickedColor = false;
			this.m_tabGantry.UseEdge = false;
			this.m_tabGantry.UseHoverEmphasizeCustomColor = false;
			this.m_tabGantry.UseImage = false;
			this.m_tabGantry.UserHoverEmpahsize = false;
			this.m_tabGantry.UseSubFont = false;
			this.m_tabGantry.Click += new System.EventHandler(this.Clicked_Panel);
			// 
			// m_tabHome
			// 
			this.m_tabHome.BorderWidth = 2;
			this.m_tabHome.ButtonClicked = false;
			this.m_tabHome.ClickedEmphasizeTextColor = System.Drawing.Color.White;
			this.m_tabHome.CustomClickedGradientFirstColor = System.Drawing.Color.White;
			this.m_tabHome.CustomClickedGradientSecondColor = System.Drawing.Color.White;
			this.m_tabHome.Description = "";
			this.m_tabHome.DisabledColor = System.Drawing.Color.DarkGray;
			this.m_tabHome.Dock = System.Windows.Forms.DockStyle.Fill;
			this.m_tabHome.EdgeRadius = 5;
			this.m_tabHome.GradientAngle = 70F;
			this.m_tabHome.GradientFirstColor = System.Drawing.Color.White;
			this.m_tabHome.GradientSecondColor = System.Drawing.Color.White;
			this.m_tabHome.HoverEmphasizeCustomColor = System.Drawing.Color.White;
			this.m_tabHome.ImagePosition = new System.Drawing.Point(7, 7);
			this.m_tabHome.ImageSize = new System.Drawing.Point(30, 30);
			this.m_tabHome.LoadImage = global::FrameOfSystem3.Properties.Resources.Home_black;
			this.m_tabHome.Location = new System.Drawing.Point(512, 80);
			this.m_tabHome.MainFont = new System.Drawing.Font("맑은 고딕", 11F, System.Drawing.FontStyle.Bold);
			this.m_tabHome.MainFontColor = System.Drawing.Color.DarkBlue;
			this.m_tabHome.Margin = new System.Windows.Forms.Padding(0);
			this.m_tabHome.Name = "m_tabHome";
			this.m_tabHome.Size = new System.Drawing.Size(128, 40);
			this.m_tabHome.SubFont = new System.Drawing.Font("맑은 고딕", 8F, System.Drawing.FontStyle.Bold);
			this.m_tabHome.SubFontColor = System.Drawing.Color.DarkBlue;
			this.m_tabHome.SubText = "STATUS";
			this.m_tabHome.TabIndex = 4;
			this.m_tabHome.Text = "HOME";
			this.m_tabHome.TextAlignMain = Sys3Controls.EN_TEXTALIGN.MIDDLE_CENTER;
			this.m_tabHome.TextAlignSub = Sys3Controls.EN_TEXTALIGN.TOP_RIGHT;
			this.m_tabHome.ThemeIndex = 0;
			this.m_tabHome.UseBorder = false;
			this.m_tabHome.UseClickedEmphasizeTextColor = false;
			this.m_tabHome.UseCustomizeClickedColor = false;
			this.m_tabHome.UseEdge = false;
			this.m_tabHome.UseHoverEmphasizeCustomColor = false;
			this.m_tabHome.UseImage = false;
			this.m_tabHome.UserHoverEmpahsize = false;
			this.m_tabHome.UseSubFont = false;
			this.m_tabHome.Click += new System.EventHandler(this.Clicked_Panel);
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
			this.m_lblIndex.Location = new System.Drawing.Point(1, 1);
			this.m_lblIndex.MainFont = new System.Drawing.Font("맑은 고딕", 9F, System.Drawing.FontStyle.Bold);
			this.m_lblIndex.MainFontColor = System.Drawing.Color.Black;
			this.m_lblIndex.Margin = new System.Windows.Forms.Padding(1);
			this.m_lblIndex.Name = "m_lblIndex";
			this.m_lblIndex.Size = new System.Drawing.Size(126, 38);
			this.m_lblIndex.SubFont = new System.Drawing.Font("맑은 고딕", 10F);
			this.m_lblIndex.SubFontColor = System.Drawing.Color.Black;
			this.m_lblIndex.SubText = "";
			this.m_lblIndex.TabIndex = 1376;
			this.m_lblIndex.Text = "INDEX";
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
			// m_labelTag
			// 
			this.m_labelTag.BackGroundColor = System.Drawing.Color.White;
			this.m_labelTag.BorderStroke = 2;
			this.m_labelTag.BorderStyle = System.Windows.Forms.ButtonBorderStyle.Solid;
			this._tableLayoutPanel_Configuration.SetColumnSpan(this.m_labelTag, 2);
			this.m_labelTag.Description = "";
			this.m_labelTag.DisabledColor = System.Drawing.Color.LightGray;
			this.m_labelTag.Dock = System.Windows.Forms.DockStyle.Fill;
			this.m_labelTag.EdgeRadius = 1;
			this.m_labelTag.Enabled = false;
			this.m_labelTag.ImagePosition = new System.Drawing.Point(0, 0);
			this.m_labelTag.ImageSize = new System.Drawing.Point(0, 0);
			this.m_labelTag.LoadImage = null;
			this.m_labelTag.Location = new System.Drawing.Point(129, 41);
			this.m_labelTag.MainFont = new System.Drawing.Font("맑은 고딕", 11F, System.Drawing.FontStyle.Bold);
			this.m_labelTag.MainFontColor = System.Drawing.Color.Black;
			this.m_labelTag.Margin = new System.Windows.Forms.Padding(1);
			this.m_labelTag.Name = "m_labelTag";
			this.m_labelTag.Size = new System.Drawing.Size(254, 38);
			this.m_labelTag.SubFont = new System.Drawing.Font("맑은 고딕", 10F);
			this.m_labelTag.SubFontColor = System.Drawing.Color.Black;
			this.m_labelTag.SubText = "";
			this.m_labelTag.TabIndex = 2;
			this.m_labelTag.TextAlignMain = Sys3Controls.EN_TEXTALIGN.MIDDLE_CENTER;
			this.m_labelTag.TextAlignSub = Sys3Controls.EN_TEXTALIGN.MIDDLE_CENTER;
			this.m_labelTag.TextAlignUnit = Sys3Controls.EN_TEXTALIGN.MIDDLE_CENTER;
			this.m_labelTag.ThemeIndex = 0;
			this.m_labelTag.UnitAreaRate = 40;
			this.m_labelTag.UnitFont = new System.Drawing.Font("맑은 고딕", 10F, System.Drawing.FontStyle.Bold);
			this.m_labelTag.UnitFontColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
			this.m_labelTag.UnitPositionVertical = false;
			this.m_labelTag.UnitText = "";
			this.m_labelTag.UseBorder = true;
			this.m_labelTag.UseEdgeRadius = false;
			this.m_labelTag.UseImage = false;
			this.m_labelTag.UseSubFont = false;
			this.m_labelTag.UseUnitFont = false;
			this.m_labelTag.Click += new System.EventHandler(this.Click_ConfigurationLabels);
			// 
			// m_lblTag
			// 
			this.m_lblTag.BackGroundColor = System.Drawing.Color.FromArgb(((int)(((byte)(200)))), ((int)(((byte)(200)))), ((int)(((byte)(220)))));
			this.m_lblTag.BorderStroke = 2;
			this.m_lblTag.BorderStyle = System.Windows.Forms.ButtonBorderStyle.Solid;
			this.m_lblTag.Description = "";
			this.m_lblTag.DisabledColor = System.Drawing.Color.DarkGray;
			this.m_lblTag.Dock = System.Windows.Forms.DockStyle.Fill;
			this.m_lblTag.EdgeRadius = 1;
			this.m_lblTag.ImagePosition = new System.Drawing.Point(0, 0);
			this.m_lblTag.ImageSize = new System.Drawing.Point(0, 0);
			this.m_lblTag.LoadImage = null;
			this.m_lblTag.Location = new System.Drawing.Point(1, 41);
			this.m_lblTag.MainFont = new System.Drawing.Font("맑은 고딕", 9F, System.Drawing.FontStyle.Bold);
			this.m_lblTag.MainFontColor = System.Drawing.Color.Black;
			this.m_lblTag.Margin = new System.Windows.Forms.Padding(1);
			this.m_lblTag.Name = "m_lblTag";
			this.m_lblTag.Size = new System.Drawing.Size(126, 38);
			this.m_lblTag.SubFont = new System.Drawing.Font("맑은 고딕", 10F);
			this.m_lblTag.SubFontColor = System.Drawing.Color.Black;
			this.m_lblTag.SubText = "";
			this.m_lblTag.TabIndex = 1376;
			this.m_lblTag.Text = "TAG NUMBER";
			this.m_lblTag.TextAlignMain = Sys3Controls.EN_TEXTALIGN.TOP_LEFT;
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
			// m_labelIndex
			// 
			this.m_labelIndex.BackGroundColor = System.Drawing.Color.White;
			this.m_labelIndex.BorderStroke = 2;
			this.m_labelIndex.BorderStyle = System.Windows.Forms.ButtonBorderStyle.Solid;
			this._tableLayoutPanel_Configuration.SetColumnSpan(this.m_labelIndex, 2);
			this.m_labelIndex.Description = "";
			this.m_labelIndex.DisabledColor = System.Drawing.Color.LightGray;
			this.m_labelIndex.Dock = System.Windows.Forms.DockStyle.Fill;
			this.m_labelIndex.EdgeRadius = 1;
			this.m_labelIndex.Enabled = false;
			this.m_labelIndex.ImagePosition = new System.Drawing.Point(0, 0);
			this.m_labelIndex.ImageSize = new System.Drawing.Point(0, 0);
			this.m_labelIndex.LoadImage = null;
			this.m_labelIndex.Location = new System.Drawing.Point(129, 1);
			this.m_labelIndex.MainFont = new System.Drawing.Font("맑은 고딕", 11F, System.Drawing.FontStyle.Bold);
			this.m_labelIndex.MainFontColor = System.Drawing.Color.Black;
			this.m_labelIndex.Margin = new System.Windows.Forms.Padding(1);
			this.m_labelIndex.Name = "m_labelIndex";
			this.m_labelIndex.Size = new System.Drawing.Size(254, 38);
			this.m_labelIndex.SubFont = new System.Drawing.Font("맑은 고딕", 10F);
			this.m_labelIndex.SubFontColor = System.Drawing.Color.Black;
			this.m_labelIndex.SubText = "";
			this.m_labelIndex.TabIndex = 1385;
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
			// m_labelTarget
			// 
			this.m_labelTarget.BackGroundColor = System.Drawing.Color.White;
			this.m_labelTarget.BorderStroke = 2;
			this.m_labelTarget.BorderStyle = System.Windows.Forms.ButtonBorderStyle.Solid;
			this._tableLayoutPanel_Configuration.SetColumnSpan(this.m_labelTarget, 2);
			this.m_labelTarget.Description = "";
			this.m_labelTarget.DisabledColor = System.Drawing.Color.LightGray;
			this.m_labelTarget.Dock = System.Windows.Forms.DockStyle.Fill;
			this.m_labelTarget.EdgeRadius = 1;
			this.m_labelTarget.Enabled = false;
			this.m_labelTarget.ImagePosition = new System.Drawing.Point(0, 0);
			this.m_labelTarget.ImageSize = new System.Drawing.Point(0, 0);
			this.m_labelTarget.LoadImage = null;
			this.m_labelTarget.Location = new System.Drawing.Point(513, 41);
			this.m_labelTarget.MainFont = new System.Drawing.Font("맑은 고딕", 11F, System.Drawing.FontStyle.Bold);
			this.m_labelTarget.MainFontColor = System.Drawing.Color.Black;
			this.m_labelTarget.Margin = new System.Windows.Forms.Padding(1);
			this.m_labelTarget.Name = "m_labelTarget";
			this.m_labelTarget.Size = new System.Drawing.Size(254, 38);
			this.m_labelTarget.SubFont = new System.Drawing.Font("맑은 고딕", 10F);
			this.m_labelTarget.SubFontColor = System.Drawing.Color.Black;
			this.m_labelTarget.SubText = "";
			this.m_labelTarget.TabIndex = 1;
			this.m_labelTarget.Text = "--";
			this.m_labelTarget.TextAlignMain = Sys3Controls.EN_TEXTALIGN.BOTTOM_RIGHT;
			this.m_labelTarget.TextAlignSub = Sys3Controls.EN_TEXTALIGN.MIDDLE_CENTER;
			this.m_labelTarget.TextAlignUnit = Sys3Controls.EN_TEXTALIGN.MIDDLE_CENTER;
			this.m_labelTarget.ThemeIndex = 0;
			this.m_labelTarget.UnitAreaRate = 40;
			this.m_labelTarget.UnitFont = new System.Drawing.Font("맑은 고딕", 10F, System.Drawing.FontStyle.Bold);
			this.m_labelTarget.UnitFontColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
			this.m_labelTarget.UnitPositionVertical = false;
			this.m_labelTarget.UnitText = "";
			this.m_labelTarget.UseBorder = true;
			this.m_labelTarget.UseEdgeRadius = false;
			this.m_labelTarget.UseImage = false;
			this.m_labelTarget.UseSubFont = false;
			this.m_labelTarget.UseUnitFont = false;
			this.m_labelTarget.Click += new System.EventHandler(this.Click_ConfigurationLabels);
			// 
			// m_labelName
			// 
			this.m_labelName.BackGroundColor = System.Drawing.Color.White;
			this.m_labelName.BorderStroke = 2;
			this.m_labelName.BorderStyle = System.Windows.Forms.ButtonBorderStyle.Solid;
			this._tableLayoutPanel_Configuration.SetColumnSpan(this.m_labelName, 5);
			this.m_labelName.Description = "";
			this.m_labelName.DisabledColor = System.Drawing.Color.LightGray;
			this.m_labelName.Dock = System.Windows.Forms.DockStyle.Fill;
			this.m_labelName.EdgeRadius = 1;
			this.m_labelName.Enabled = false;
			this.m_labelName.ImagePosition = new System.Drawing.Point(0, 0);
			this.m_labelName.ImageSize = new System.Drawing.Point(0, 0);
			this.m_labelName.LoadImage = null;
			this.m_labelName.Location = new System.Drawing.Point(513, 1);
			this.m_labelName.MainFont = new System.Drawing.Font("맑은 고딕", 11F, System.Drawing.FontStyle.Bold);
			this.m_labelName.MainFontColor = System.Drawing.Color.Black;
			this.m_labelName.Margin = new System.Windows.Forms.Padding(1);
			this.m_labelName.Name = "m_labelName";
			this.m_labelName.Size = new System.Drawing.Size(642, 38);
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
			this.m_labelName.Click += new System.EventHandler(this.Click_ConfigurationLabels);
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
			this.m_lblName.Location = new System.Drawing.Point(385, 1);
			this.m_lblName.MainFont = new System.Drawing.Font("맑은 고딕", 9F, System.Drawing.FontStyle.Bold);
			this.m_lblName.MainFontColor = System.Drawing.Color.Black;
			this.m_lblName.Margin = new System.Windows.Forms.Padding(1);
			this.m_lblName.Name = "m_lblName";
			this.m_lblName.Size = new System.Drawing.Size(126, 38);
			this.m_lblName.SubFont = new System.Drawing.Font("맑은 고딕", 10F);
			this.m_lblName.SubFontColor = System.Drawing.Color.Black;
			this.m_lblName.SubText = "";
			this.m_lblName.TabIndex = 1377;
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
			// m_lblTarget
			// 
			this.m_lblTarget.BackGroundColor = System.Drawing.Color.FromArgb(((int)(((byte)(200)))), ((int)(((byte)(200)))), ((int)(((byte)(220)))));
			this.m_lblTarget.BorderStroke = 2;
			this.m_lblTarget.BorderStyle = System.Windows.Forms.ButtonBorderStyle.Solid;
			this.m_lblTarget.Description = "";
			this.m_lblTarget.DisabledColor = System.Drawing.Color.DarkGray;
			this.m_lblTarget.Dock = System.Windows.Forms.DockStyle.Fill;
			this.m_lblTarget.EdgeRadius = 1;
			this.m_lblTarget.ImagePosition = new System.Drawing.Point(0, 0);
			this.m_lblTarget.ImageSize = new System.Drawing.Point(0, 0);
			this.m_lblTarget.LoadImage = null;
			this.m_lblTarget.Location = new System.Drawing.Point(385, 41);
			this.m_lblTarget.MainFont = new System.Drawing.Font("맑은 고딕", 9F, System.Drawing.FontStyle.Bold);
			this.m_lblTarget.MainFontColor = System.Drawing.Color.Black;
			this.m_lblTarget.Margin = new System.Windows.Forms.Padding(1);
			this.m_lblTarget.Name = "m_lblTarget";
			this.m_lblTarget.Size = new System.Drawing.Size(126, 38);
			this.m_lblTarget.SubFont = new System.Drawing.Font("맑은 고딕", 10F);
			this.m_lblTarget.SubFontColor = System.Drawing.Color.Black;
			this.m_lblTarget.SubText = "";
			this.m_lblTarget.TabIndex = 1376;
			this.m_lblTarget.Text = "TARGET";
			this.m_lblTarget.TextAlignMain = Sys3Controls.EN_TEXTALIGN.TOP_LEFT;
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
			// m_lblPower
			// 
			this.m_lblPower.BackGroundColor = System.Drawing.Color.FromArgb(((int)(((byte)(200)))), ((int)(((byte)(200)))), ((int)(((byte)(220)))));
			this.m_lblPower.BorderStroke = 2;
			this.m_lblPower.BorderStyle = System.Windows.Forms.ButtonBorderStyle.Solid;
			this.m_lblPower.Description = "";
			this.m_lblPower.DisabledColor = System.Drawing.Color.DarkGray;
			this.m_lblPower.Dock = System.Windows.Forms.DockStyle.Fill;
			this.m_lblPower.EdgeRadius = 1;
			this.m_lblPower.ImagePosition = new System.Drawing.Point(0, 0);
			this.m_lblPower.ImageSize = new System.Drawing.Point(0, 0);
			this.m_lblPower.LoadImage = null;
			this.m_lblPower.Location = new System.Drawing.Point(769, 41);
			this.m_lblPower.MainFont = new System.Drawing.Font("맑은 고딕", 9F, System.Drawing.FontStyle.Bold);
			this.m_lblPower.MainFontColor = System.Drawing.Color.Black;
			this.m_lblPower.Margin = new System.Windows.Forms.Padding(1);
			this.m_lblPower.Name = "m_lblPower";
			this.m_lblPower.Size = new System.Drawing.Size(126, 38);
			this.m_lblPower.SubFont = new System.Drawing.Font("맑은 고딕", 10F);
			this.m_lblPower.SubFontColor = System.Drawing.Color.Black;
			this.m_lblPower.SubText = "";
			this.m_lblPower.TabIndex = 1378;
			this.m_lblPower.Text = "POWER";
			this.m_lblPower.TextAlignMain = Sys3Controls.EN_TEXTALIGN.TOP_LEFT;
			this.m_lblPower.TextAlignSub = Sys3Controls.EN_TEXTALIGN.MIDDLE_CENTER;
			this.m_lblPower.TextAlignUnit = Sys3Controls.EN_TEXTALIGN.MIDDLE_CENTER;
			this.m_lblPower.ThemeIndex = 0;
			this.m_lblPower.UnitAreaRate = 40;
			this.m_lblPower.UnitFont = new System.Drawing.Font("맑은 고딕", 10F, System.Drawing.FontStyle.Bold);
			this.m_lblPower.UnitFontColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
			this.m_lblPower.UnitPositionVertical = false;
			this.m_lblPower.UnitText = "";
			this.m_lblPower.UseBorder = true;
			this.m_lblPower.UseEdgeRadius = false;
			this.m_lblPower.UseImage = false;
			this.m_lblPower.UseSubFont = false;
			this.m_lblPower.UseUnitFont = false;
			// 
			// m_labelPower
			// 
			this.m_labelPower.BackGroundColor = System.Drawing.Color.White;
			this.m_labelPower.BorderStroke = 2;
			this.m_labelPower.BorderStyle = System.Windows.Forms.ButtonBorderStyle.Solid;
			this._tableLayoutPanel_Configuration.SetColumnSpan(this.m_labelPower, 2);
			this.m_labelPower.Description = "";
			this.m_labelPower.DisabledColor = System.Drawing.Color.LightGray;
			this.m_labelPower.Dock = System.Windows.Forms.DockStyle.Fill;
			this.m_labelPower.EdgeRadius = 1;
			this.m_labelPower.Enabled = false;
			this.m_labelPower.ImagePosition = new System.Drawing.Point(0, 0);
			this.m_labelPower.ImageSize = new System.Drawing.Point(0, 0);
			this.m_labelPower.LoadImage = null;
			this.m_labelPower.Location = new System.Drawing.Point(897, 41);
			this.m_labelPower.MainFont = new System.Drawing.Font("맑은 고딕", 11F, System.Drawing.FontStyle.Bold);
			this.m_labelPower.MainFontColor = System.Drawing.Color.Black;
			this.m_labelPower.Margin = new System.Windows.Forms.Padding(1);
			this.m_labelPower.Name = "m_labelPower";
			this.m_labelPower.Size = new System.Drawing.Size(258, 38);
			this.m_labelPower.SubFont = new System.Drawing.Font("맑은 고딕", 10F);
			this.m_labelPower.SubFontColor = System.Drawing.Color.Black;
			this.m_labelPower.SubText = "";
			this.m_labelPower.TabIndex = 3;
			this.m_labelPower.Text = "--";
			this.m_labelPower.TextAlignMain = Sys3Controls.EN_TEXTALIGN.BOTTOM_RIGHT;
			this.m_labelPower.TextAlignSub = Sys3Controls.EN_TEXTALIGN.MIDDLE_CENTER;
			this.m_labelPower.TextAlignUnit = Sys3Controls.EN_TEXTALIGN.BOTTOM_RIGHT;
			this.m_labelPower.ThemeIndex = 0;
			this.m_labelPower.UnitAreaRate = 20;
			this.m_labelPower.UnitFont = new System.Drawing.Font("맑은 고딕", 10F, System.Drawing.FontStyle.Bold);
			this.m_labelPower.UnitFontColor = System.Drawing.Color.Black;
			this.m_labelPower.UnitPositionVertical = false;
			this.m_labelPower.UnitText = "%";
			this.m_labelPower.UseBorder = true;
			this.m_labelPower.UseEdgeRadius = false;
			this.m_labelPower.UseImage = false;
			this.m_labelPower.UseSubFont = false;
			this.m_labelPower.UseUnitFont = true;
			this.m_labelPower.Click += new System.EventHandler(this.Click_ConfigurationLabels);
			// 
			// m_btnHome
			// 
			this.m_btnHome.BorderWidth = 3;
			this.m_btnHome.ButtonClicked = false;
			this.m_btnHome.ClickedEmphasizeTextColor = System.Drawing.Color.White;
			this.m_btnHome.CustomClickedGradientFirstColor = System.Drawing.Color.White;
			this.m_btnHome.CustomClickedGradientSecondColor = System.Drawing.Color.White;
			this.m_btnHome.Description = "";
			this.m_btnHome.DisabledColor = System.Drawing.Color.DarkGray;
			this.m_btnHome.Dock = System.Windows.Forms.DockStyle.Fill;
			this.m_btnHome.EdgeRadius = 5;
			this.m_btnHome.Enabled = false;
			this.m_btnHome.GradientAngle = 70F;
			this.m_btnHome.GradientFirstColor = System.Drawing.Color.White;
			this.m_btnHome.GradientSecondColor = System.Drawing.Color.FromArgb(((int)(((byte)(170)))), ((int)(((byte)(176)))), ((int)(((byte)(183)))));
			this.m_btnHome.HoverEmphasizeCustomColor = System.Drawing.Color.White;
			this.m_btnHome.ImagePosition = new System.Drawing.Point(7, 7);
			this.m_btnHome.ImageSize = new System.Drawing.Point(30, 30);
			this.m_btnHome.LoadImage = global::FrameOfSystem3.Properties.Resources.Home_black;
			this.m_btnHome.Location = new System.Drawing.Point(552, 782);
			this.m_btnHome.MainFont = new System.Drawing.Font("맑은 고딕", 13F, System.Drawing.FontStyle.Bold);
			this.m_btnHome.MainFontColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(36)))), ((int)(((byte)(0)))));
			this.m_btnHome.Margin = new System.Windows.Forms.Padding(2);
			this.m_btnHome.Name = "m_btnHome";
			this._tableLayoutPanel_Main.SetRowSpan(this.m_btnHome, 2);
			this.m_btnHome.Size = new System.Drawing.Size(131, 116);
			this.m_btnHome.SubFont = new System.Drawing.Font("맑은 고딕", 8F, System.Drawing.FontStyle.Bold);
			this.m_btnHome.SubFontColor = System.Drawing.Color.DarkBlue;
			this.m_btnHome.SubText = "MOVE";
			this.m_btnHome.TabIndex = 2;
			this.m_btnHome.Text = "HOME";
			this.m_btnHome.TextAlignMain = Sys3Controls.EN_TEXTALIGN.MIDDLE_RIGHT;
			this.m_btnHome.TextAlignSub = Sys3Controls.EN_TEXTALIGN.TOP_RIGHT;
			this.m_btnHome.ThemeIndex = 0;
			this.m_btnHome.UseBorder = true;
			this.m_btnHome.UseClickedEmphasizeTextColor = false;
			this.m_btnHome.UseCustomizeClickedColor = false;
			this.m_btnHome.UseEdge = true;
			this.m_btnHome.UseHoverEmphasizeCustomColor = false;
			this.m_btnHome.UseImage = true;
			this.m_btnHome.UserHoverEmpahsize = false;
			this.m_btnHome.UseSubFont = true;
			this.m_btnHome.Click += new System.EventHandler(this.Click_Move);
			// 
			// m_btnAllStop
			// 
			this.m_btnAllStop.BorderWidth = 3;
			this.m_btnAllStop.ButtonClicked = false;
			this.m_btnAllStop.ClickedEmphasizeTextColor = System.Drawing.Color.White;
			this.m_btnAllStop.CustomClickedGradientFirstColor = System.Drawing.Color.White;
			this.m_btnAllStop.CustomClickedGradientSecondColor = System.Drawing.Color.White;
			this.m_btnAllStop.Description = "";
			this.m_btnAllStop.DisabledColor = System.Drawing.Color.DarkGray;
			this.m_btnAllStop.Dock = System.Windows.Forms.DockStyle.Fill;
			this.m_btnAllStop.EdgeRadius = 5;
			this.m_btnAllStop.GradientAngle = 70F;
			this.m_btnAllStop.GradientFirstColor = System.Drawing.Color.White;
			this.m_btnAllStop.GradientSecondColor = System.Drawing.Color.FromArgb(((int)(((byte)(170)))), ((int)(((byte)(176)))), ((int)(((byte)(183)))));
			this.m_btnAllStop.HoverEmphasizeCustomColor = System.Drawing.Color.White;
			this.m_btnAllStop.ImagePosition = new System.Drawing.Point(7, 7);
			this.m_btnAllStop.ImageSize = new System.Drawing.Point(30, 30);
			this.m_btnAllStop.LoadImage = global::FrameOfSystem3.Properties.Resources.DISCONNECT_BLACK;
			this.m_btnAllStop.Location = new System.Drawing.Point(417, 782);
			this.m_btnAllStop.MainFont = new System.Drawing.Font("맑은 고딕", 13F, System.Drawing.FontStyle.Bold);
			this.m_btnAllStop.MainFontColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(36)))), ((int)(((byte)(0)))));
			this.m_btnAllStop.Margin = new System.Windows.Forms.Padding(2);
			this.m_btnAllStop.Name = "m_btnAllStop";
			this._tableLayoutPanel_Main.SetRowSpan(this.m_btnAllStop, 2);
			this.m_btnAllStop.Size = new System.Drawing.Size(131, 116);
			this.m_btnAllStop.SubFont = new System.Drawing.Font("맑은 고딕", 8F, System.Drawing.FontStyle.Bold);
			this.m_btnAllStop.SubFontColor = System.Drawing.Color.DarkBlue;
			this.m_btnAllStop.SubText = "MOVE";
			this.m_btnAllStop.TabIndex = 1;
			this.m_btnAllStop.Text = "ALL STOP";
			this.m_btnAllStop.TextAlignMain = Sys3Controls.EN_TEXTALIGN.MIDDLE_RIGHT;
			this.m_btnAllStop.TextAlignSub = Sys3Controls.EN_TEXTALIGN.TOP_RIGHT;
			this.m_btnAllStop.ThemeIndex = 0;
			this.m_btnAllStop.UseBorder = true;
			this.m_btnAllStop.UseClickedEmphasizeTextColor = false;
			this.m_btnAllStop.UseCustomizeClickedColor = false;
			this.m_btnAllStop.UseEdge = true;
			this.m_btnAllStop.UseHoverEmphasizeCustomColor = false;
			this.m_btnAllStop.UseImage = false;
			this.m_btnAllStop.UserHoverEmpahsize = false;
			this.m_btnAllStop.UseSubFont = true;
			this.m_btnAllStop.Click += new System.EventHandler(this.Click_Move);
			// 
			// m_btnDisable
			// 
			this.m_btnDisable.BorderWidth = 3;
			this.m_btnDisable.ButtonClicked = false;
			this.m_btnDisable.ClickedEmphasizeTextColor = System.Drawing.Color.White;
			this.m_btnDisable.CustomClickedGradientFirstColor = System.Drawing.Color.White;
			this.m_btnDisable.CustomClickedGradientSecondColor = System.Drawing.Color.White;
			this.m_btnDisable.Description = "";
			this.m_btnDisable.DisabledColor = System.Drawing.Color.LightSlateGray;
			this.m_btnDisable.Dock = System.Windows.Forms.DockStyle.Fill;
			this.m_btnDisable.EdgeRadius = 5;
			this.m_btnDisable.Enabled = false;
			this.m_btnDisable.GradientAngle = 70F;
			this.m_btnDisable.GradientFirstColor = System.Drawing.Color.White;
			this.m_btnDisable.GradientSecondColor = System.Drawing.Color.LightSlateGray;
			this.m_btnDisable.HoverEmphasizeCustomColor = System.Drawing.Color.White;
			this.m_btnDisable.ImagePosition = new System.Drawing.Point(7, 7);
			this.m_btnDisable.ImageSize = new System.Drawing.Point(30, 30);
			this.m_btnDisable.LoadImage = global::FrameOfSystem3.Properties.Resources.DISABLE_BLACK;
			this.m_btnDisable.Location = new System.Drawing.Point(137, 782);
			this.m_btnDisable.MainFont = new System.Drawing.Font("맑은 고딕", 13F, System.Drawing.FontStyle.Bold);
			this.m_btnDisable.MainFontColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(36)))), ((int)(((byte)(0)))));
			this.m_btnDisable.Margin = new System.Windows.Forms.Padding(2);
			this.m_btnDisable.Name = "m_btnDisable";
			this._tableLayoutPanel_Main.SetRowSpan(this.m_btnDisable, 2);
			this.m_btnDisable.Size = new System.Drawing.Size(131, 116);
			this.m_btnDisable.SubFont = new System.Drawing.Font("맑은 고딕", 8F, System.Drawing.FontStyle.Bold);
			this.m_btnDisable.SubFontColor = System.Drawing.Color.DarkBlue;
			this.m_btnDisable.SubText = "STATUS";
			this.m_btnDisable.TabIndex = 1;
			this.m_btnDisable.Text = "DISABLE";
			this.m_btnDisable.TextAlignMain = Sys3Controls.EN_TEXTALIGN.MIDDLE_RIGHT;
			this.m_btnDisable.TextAlignSub = Sys3Controls.EN_TEXTALIGN.TOP_RIGHT;
			this.m_btnDisable.ThemeIndex = 0;
			this.m_btnDisable.UseBorder = true;
			this.m_btnDisable.UseClickedEmphasizeTextColor = false;
			this.m_btnDisable.UseCustomizeClickedColor = false;
			this.m_btnDisable.UseEdge = true;
			this.m_btnDisable.UseHoverEmphasizeCustomColor = false;
			this.m_btnDisable.UseImage = true;
			this.m_btnDisable.UserHoverEmpahsize = false;
			this.m_btnDisable.UseSubFont = true;
			this.m_btnDisable.Click += new System.EventHandler(this.Click_EnableOrDisable);
			// 
			// m_btnStop
			// 
			this.m_btnStop.BorderWidth = 3;
			this.m_btnStop.ButtonClicked = false;
			this.m_btnStop.ClickedEmphasizeTextColor = System.Drawing.Color.White;
			this.m_btnStop.CustomClickedGradientFirstColor = System.Drawing.Color.White;
			this.m_btnStop.CustomClickedGradientSecondColor = System.Drawing.Color.White;
			this.m_btnStop.Description = "";
			this.m_btnStop.DisabledColor = System.Drawing.Color.DarkGray;
			this.m_btnStop.Dock = System.Windows.Forms.DockStyle.Fill;
			this.m_btnStop.EdgeRadius = 5;
			this.m_btnStop.Enabled = false;
			this.m_btnStop.GradientAngle = 70F;
			this.m_btnStop.GradientFirstColor = System.Drawing.Color.White;
			this.m_btnStop.GradientSecondColor = System.Drawing.Color.FromArgb(((int)(((byte)(170)))), ((int)(((byte)(176)))), ((int)(((byte)(183)))));
			this.m_btnStop.HoverEmphasizeCustomColor = System.Drawing.Color.White;
			this.m_btnStop.ImagePosition = new System.Drawing.Point(7, 7);
			this.m_btnStop.ImageSize = new System.Drawing.Point(30, 30);
			this.m_btnStop.LoadImage = global::FrameOfSystem3.Properties.Resources.CONNECT_BLACK;
			this.m_btnStop.Location = new System.Drawing.Point(282, 782);
			this.m_btnStop.MainFont = new System.Drawing.Font("맑은 고딕", 13F, System.Drawing.FontStyle.Bold);
			this.m_btnStop.MainFontColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(36)))), ((int)(((byte)(0)))));
			this.m_btnStop.Margin = new System.Windows.Forms.Padding(2);
			this.m_btnStop.Name = "m_btnStop";
			this._tableLayoutPanel_Main.SetRowSpan(this.m_btnStop, 2);
			this.m_btnStop.Size = new System.Drawing.Size(131, 116);
			this.m_btnStop.SubFont = new System.Drawing.Font("맑은 고딕", 8F, System.Drawing.FontStyle.Bold);
			this.m_btnStop.SubFontColor = System.Drawing.Color.DarkBlue;
			this.m_btnStop.SubText = "MOVE";
			this.m_btnStop.TabIndex = 0;
			this.m_btnStop.Text = "STOP";
			this.m_btnStop.TextAlignMain = Sys3Controls.EN_TEXTALIGN.MIDDLE_RIGHT;
			this.m_btnStop.TextAlignSub = Sys3Controls.EN_TEXTALIGN.TOP_RIGHT;
			this.m_btnStop.ThemeIndex = 0;
			this.m_btnStop.UseBorder = true;
			this.m_btnStop.UseClickedEmphasizeTextColor = false;
			this.m_btnStop.UseCustomizeClickedColor = false;
			this.m_btnStop.UseEdge = true;
			this.m_btnStop.UseHoverEmphasizeCustomColor = false;
			this.m_btnStop.UseImage = false;
			this.m_btnStop.UserHoverEmpahsize = false;
			this.m_btnStop.UseSubFont = true;
			this.m_btnStop.Click += new System.EventHandler(this.Click_Move);
			// 
			// m_btnEnable
			// 
			this.m_btnEnable.BorderWidth = 3;
			this.m_btnEnable.ButtonClicked = false;
			this.m_btnEnable.ClickedEmphasizeTextColor = System.Drawing.Color.White;
			this.m_btnEnable.CustomClickedGradientFirstColor = System.Drawing.Color.White;
			this.m_btnEnable.CustomClickedGradientSecondColor = System.Drawing.Color.White;
			this.m_btnEnable.Description = "";
			this.m_btnEnable.DisabledColor = System.Drawing.Color.LightSlateGray;
			this.m_btnEnable.Dock = System.Windows.Forms.DockStyle.Fill;
			this.m_btnEnable.EdgeRadius = 5;
			this.m_btnEnable.Enabled = false;
			this.m_btnEnable.GradientAngle = 70F;
			this.m_btnEnable.GradientFirstColor = System.Drawing.Color.White;
			this.m_btnEnable.GradientSecondColor = System.Drawing.Color.LightSlateGray;
			this.m_btnEnable.HoverEmphasizeCustomColor = System.Drawing.Color.White;
			this.m_btnEnable.ImagePosition = new System.Drawing.Point(7, 7);
			this.m_btnEnable.ImageSize = new System.Drawing.Point(30, 30);
			this.m_btnEnable.LoadImage = global::FrameOfSystem3.Properties.Resources.ENABLE_BLACK;
			this.m_btnEnable.Location = new System.Drawing.Point(2, 782);
			this.m_btnEnable.MainFont = new System.Drawing.Font("맑은 고딕", 13F, System.Drawing.FontStyle.Bold);
			this.m_btnEnable.MainFontColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(36)))), ((int)(((byte)(0)))));
			this.m_btnEnable.Margin = new System.Windows.Forms.Padding(2);
			this.m_btnEnable.Name = "m_btnEnable";
			this._tableLayoutPanel_Main.SetRowSpan(this.m_btnEnable, 2);
			this.m_btnEnable.Size = new System.Drawing.Size(131, 116);
			this.m_btnEnable.SubFont = new System.Drawing.Font("맑은 고딕", 8F, System.Drawing.FontStyle.Bold);
			this.m_btnEnable.SubFontColor = System.Drawing.Color.DarkBlue;
			this.m_btnEnable.SubText = "STATUS";
			this.m_btnEnable.TabIndex = 0;
			this.m_btnEnable.Text = "ENABLE";
			this.m_btnEnable.TextAlignMain = Sys3Controls.EN_TEXTALIGN.MIDDLE_RIGHT;
			this.m_btnEnable.TextAlignSub = Sys3Controls.EN_TEXTALIGN.TOP_RIGHT;
			this.m_btnEnable.ThemeIndex = 0;
			this.m_btnEnable.UseBorder = true;
			this.m_btnEnable.UseClickedEmphasizeTextColor = false;
			this.m_btnEnable.UseCustomizeClickedColor = false;
			this.m_btnEnable.UseEdge = true;
			this.m_btnEnable.UseHoverEmphasizeCustomColor = false;
			this.m_btnEnable.UseImage = true;
			this.m_btnEnable.UserHoverEmpahsize = false;
			this.m_btnEnable.UseSubFont = true;
			this.m_btnEnable.Click += new System.EventHandler(this.Click_EnableOrDisable);
			// 
			// m_btnMotorOn
			// 
			this.m_btnMotorOn.BorderWidth = 3;
			this.m_btnMotorOn.ButtonClicked = false;
			this.m_btnMotorOn.ClickedEmphasizeTextColor = System.Drawing.Color.White;
			this.m_btnMotorOn.CustomClickedGradientFirstColor = System.Drawing.Color.White;
			this.m_btnMotorOn.CustomClickedGradientSecondColor = System.Drawing.Color.White;
			this.m_btnMotorOn.Description = "";
			this.m_btnMotorOn.DisabledColor = System.Drawing.Color.DarkGray;
			this.m_btnMotorOn.Dock = System.Windows.Forms.DockStyle.Fill;
			this.m_btnMotorOn.EdgeRadius = 5;
			this.m_btnMotorOn.Enabled = false;
			this.m_btnMotorOn.GradientAngle = 70F;
			this.m_btnMotorOn.GradientFirstColor = System.Drawing.Color.White;
			this.m_btnMotorOn.GradientSecondColor = System.Drawing.Color.FromArgb(((int)(((byte)(170)))), ((int)(((byte)(176)))), ((int)(((byte)(183)))));
			this.m_btnMotorOn.HoverEmphasizeCustomColor = System.Drawing.Color.White;
			this.m_btnMotorOn.ImagePosition = new System.Drawing.Point(7, 7);
			this.m_btnMotorOn.ImageSize = new System.Drawing.Point(30, 30);
			this.m_btnMotorOn.LoadImage = global::FrameOfSystem3.Properties.Resources.Home_black;
			this.m_btnMotorOn.Location = new System.Drawing.Point(697, 782);
			this.m_btnMotorOn.MainFont = new System.Drawing.Font("맑은 고딕", 11F, System.Drawing.FontStyle.Bold);
			this.m_btnMotorOn.MainFontColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(36)))), ((int)(((byte)(0)))));
			this.m_btnMotorOn.Margin = new System.Windows.Forms.Padding(2);
			this.m_btnMotorOn.Name = "m_btnMotorOn";
			this.m_btnMotorOn.Size = new System.Drawing.Size(149, 56);
			this.m_btnMotorOn.SubFont = new System.Drawing.Font("맑은 고딕", 8F, System.Drawing.FontStyle.Bold);
			this.m_btnMotorOn.SubFontColor = System.Drawing.Color.DarkBlue;
			this.m_btnMotorOn.SubText = "STATUS";
			this.m_btnMotorOn.TabIndex = 0;
			this.m_btnMotorOn.Text = "MOTOR ON";
			this.m_btnMotorOn.TextAlignMain = Sys3Controls.EN_TEXTALIGN.BOTTOM_RIGHT;
			this.m_btnMotorOn.TextAlignSub = Sys3Controls.EN_TEXTALIGN.TOP_RIGHT;
			this.m_btnMotorOn.ThemeIndex = 0;
			this.m_btnMotorOn.UseBorder = true;
			this.m_btnMotorOn.UseClickedEmphasizeTextColor = false;
			this.m_btnMotorOn.UseCustomizeClickedColor = false;
			this.m_btnMotorOn.UseEdge = true;
			this.m_btnMotorOn.UseHoverEmphasizeCustomColor = false;
			this.m_btnMotorOn.UseImage = false;
			this.m_btnMotorOn.UserHoverEmpahsize = false;
			this.m_btnMotorOn.UseSubFont = true;
			this.m_btnMotorOn.Click += new System.EventHandler(this.Click_Status);
			// 
			// m_btnMotorOff
			// 
			this.m_btnMotorOff.BorderWidth = 3;
			this.m_btnMotorOff.ButtonClicked = false;
			this.m_btnMotorOff.ClickedEmphasizeTextColor = System.Drawing.Color.White;
			this.m_btnMotorOff.CustomClickedGradientFirstColor = System.Drawing.Color.White;
			this.m_btnMotorOff.CustomClickedGradientSecondColor = System.Drawing.Color.White;
			this.m_btnMotorOff.Description = "";
			this.m_btnMotorOff.DisabledColor = System.Drawing.Color.DarkGray;
			this.m_btnMotorOff.Dock = System.Windows.Forms.DockStyle.Fill;
			this.m_btnMotorOff.EdgeRadius = 5;
			this.m_btnMotorOff.Enabled = false;
			this.m_btnMotorOff.GradientAngle = 70F;
			this.m_btnMotorOff.GradientFirstColor = System.Drawing.Color.White;
			this.m_btnMotorOff.GradientSecondColor = System.Drawing.Color.FromArgb(((int)(((byte)(170)))), ((int)(((byte)(176)))), ((int)(((byte)(183)))));
			this.m_btnMotorOff.HoverEmphasizeCustomColor = System.Drawing.Color.White;
			this.m_btnMotorOff.ImagePosition = new System.Drawing.Point(7, 7);
			this.m_btnMotorOff.ImageSize = new System.Drawing.Point(30, 30);
			this.m_btnMotorOff.LoadImage = global::FrameOfSystem3.Properties.Resources.Home_black;
			this.m_btnMotorOff.Location = new System.Drawing.Point(850, 782);
			this.m_btnMotorOff.MainFont = new System.Drawing.Font("맑은 고딕", 11F, System.Drawing.FontStyle.Bold);
			this.m_btnMotorOff.MainFontColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(36)))), ((int)(((byte)(0)))));
			this.m_btnMotorOff.Margin = new System.Windows.Forms.Padding(2);
			this.m_btnMotorOff.Name = "m_btnMotorOff";
			this.m_btnMotorOff.Size = new System.Drawing.Size(149, 56);
			this.m_btnMotorOff.SubFont = new System.Drawing.Font("맑은 고딕", 8F, System.Drawing.FontStyle.Bold);
			this.m_btnMotorOff.SubFontColor = System.Drawing.Color.DarkBlue;
			this.m_btnMotorOff.SubText = "STATUS";
			this.m_btnMotorOff.TabIndex = 1;
			this.m_btnMotorOff.Text = "MOTOR OFF";
			this.m_btnMotorOff.TextAlignMain = Sys3Controls.EN_TEXTALIGN.BOTTOM_RIGHT;
			this.m_btnMotorOff.TextAlignSub = Sys3Controls.EN_TEXTALIGN.TOP_RIGHT;
			this.m_btnMotorOff.ThemeIndex = 0;
			this.m_btnMotorOff.UseBorder = true;
			this.m_btnMotorOff.UseClickedEmphasizeTextColor = false;
			this.m_btnMotorOff.UseCustomizeClickedColor = false;
			this.m_btnMotorOff.UseEdge = true;
			this.m_btnMotorOff.UseHoverEmphasizeCustomColor = false;
			this.m_btnMotorOff.UseImage = false;
			this.m_btnMotorOff.UserHoverEmpahsize = false;
			this.m_btnMotorOff.UseSubFont = true;
			this.m_btnMotorOff.Click += new System.EventHandler(this.Click_Status);
			// 
			// m_btnReset
			// 
			this.m_btnReset.BorderWidth = 3;
			this.m_btnReset.ButtonClicked = false;
			this.m_btnReset.ClickedEmphasizeTextColor = System.Drawing.Color.White;
			this.m_btnReset.CustomClickedGradientFirstColor = System.Drawing.Color.White;
			this.m_btnReset.CustomClickedGradientSecondColor = System.Drawing.Color.White;
			this.m_btnReset.Description = "";
			this.m_btnReset.DisabledColor = System.Drawing.Color.DarkGray;
			this.m_btnReset.Dock = System.Windows.Forms.DockStyle.Fill;
			this.m_btnReset.EdgeRadius = 5;
			this.m_btnReset.Enabled = false;
			this.m_btnReset.GradientAngle = 70F;
			this.m_btnReset.GradientFirstColor = System.Drawing.Color.White;
			this.m_btnReset.GradientSecondColor = System.Drawing.Color.FromArgb(((int)(((byte)(170)))), ((int)(((byte)(176)))), ((int)(((byte)(183)))));
			this.m_btnReset.HoverEmphasizeCustomColor = System.Drawing.Color.White;
			this.m_btnReset.ImagePosition = new System.Drawing.Point(7, 7);
			this.m_btnReset.ImageSize = new System.Drawing.Point(30, 30);
			this.m_btnReset.LoadImage = global::FrameOfSystem3.Properties.Resources.Home_black;
			this.m_btnReset.Location = new System.Drawing.Point(1003, 782);
			this.m_btnReset.MainFont = new System.Drawing.Font("맑은 고딕", 11F, System.Drawing.FontStyle.Bold);
			this.m_btnReset.MainFontColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(36)))), ((int)(((byte)(0)))));
			this.m_btnReset.Margin = new System.Windows.Forms.Padding(2);
			this.m_btnReset.Name = "m_btnReset";
			this.m_btnReset.Size = new System.Drawing.Size(151, 56);
			this.m_btnReset.SubFont = new System.Drawing.Font("맑은 고딕", 8F, System.Drawing.FontStyle.Bold);
			this.m_btnReset.SubFontColor = System.Drawing.Color.DarkBlue;
			this.m_btnReset.SubText = "STATE";
			this.m_btnReset.TabIndex = 0;
			this.m_btnReset.Text = "RESET";
			this.m_btnReset.TextAlignMain = Sys3Controls.EN_TEXTALIGN.BOTTOM_RIGHT;
			this.m_btnReset.TextAlignSub = Sys3Controls.EN_TEXTALIGN.TOP_RIGHT;
			this.m_btnReset.ThemeIndex = 0;
			this.m_btnReset.UseBorder = true;
			this.m_btnReset.UseClickedEmphasizeTextColor = false;
			this.m_btnReset.UseCustomizeClickedColor = false;
			this.m_btnReset.UseEdge = true;
			this.m_btnReset.UseHoverEmphasizeCustomColor = false;
			this.m_btnReset.UseImage = false;
			this.m_btnReset.UserHoverEmpahsize = false;
			this.m_btnReset.UseSubFont = true;
			this.m_btnReset.Click += new System.EventHandler(this.Click_State);
			// 
			// m_btnAllReset
			// 
			this.m_btnAllReset.BorderWidth = 3;
			this.m_btnAllReset.ButtonClicked = false;
			this.m_btnAllReset.ClickedEmphasizeTextColor = System.Drawing.Color.White;
			this.m_btnAllReset.CustomClickedGradientFirstColor = System.Drawing.Color.White;
			this.m_btnAllReset.CustomClickedGradientSecondColor = System.Drawing.Color.White;
			this.m_btnAllReset.Description = "";
			this.m_btnAllReset.DisabledColor = System.Drawing.Color.DarkGray;
			this.m_btnAllReset.Dock = System.Windows.Forms.DockStyle.Fill;
			this.m_btnAllReset.EdgeRadius = 5;
			this.m_btnAllReset.GradientAngle = 70F;
			this.m_btnAllReset.GradientFirstColor = System.Drawing.Color.White;
			this.m_btnAllReset.GradientSecondColor = System.Drawing.Color.FromArgb(((int)(((byte)(170)))), ((int)(((byte)(176)))), ((int)(((byte)(183)))));
			this.m_btnAllReset.HoverEmphasizeCustomColor = System.Drawing.Color.White;
			this.m_btnAllReset.ImagePosition = new System.Drawing.Point(7, 7);
			this.m_btnAllReset.ImageSize = new System.Drawing.Point(30, 30);
			this.m_btnAllReset.LoadImage = global::FrameOfSystem3.Properties.Resources.Home_black;
			this.m_btnAllReset.Location = new System.Drawing.Point(1003, 842);
			this.m_btnAllReset.MainFont = new System.Drawing.Font("맑은 고딕", 11F, System.Drawing.FontStyle.Bold);
			this.m_btnAllReset.MainFontColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(36)))), ((int)(((byte)(0)))));
			this.m_btnAllReset.Margin = new System.Windows.Forms.Padding(2);
			this.m_btnAllReset.Name = "m_btnAllReset";
			this.m_btnAllReset.Size = new System.Drawing.Size(151, 56);
			this.m_btnAllReset.SubFont = new System.Drawing.Font("맑은 고딕", 8F, System.Drawing.FontStyle.Bold);
			this.m_btnAllReset.SubFontColor = System.Drawing.Color.DarkBlue;
			this.m_btnAllReset.SubText = "STATE";
			this.m_btnAllReset.TabIndex = 1;
			this.m_btnAllReset.Text = "ALL RESET";
			this.m_btnAllReset.TextAlignMain = Sys3Controls.EN_TEXTALIGN.BOTTOM_RIGHT;
			this.m_btnAllReset.TextAlignSub = Sys3Controls.EN_TEXTALIGN.TOP_RIGHT;
			this.m_btnAllReset.ThemeIndex = 0;
			this.m_btnAllReset.UseBorder = true;
			this.m_btnAllReset.UseClickedEmphasizeTextColor = false;
			this.m_btnAllReset.UseCustomizeClickedColor = false;
			this.m_btnAllReset.UseEdge = true;
			this.m_btnAllReset.UseHoverEmphasizeCustomColor = false;
			this.m_btnAllReset.UseImage = false;
			this.m_btnAllReset.UserHoverEmpahsize = false;
			this.m_btnAllReset.UseSubFont = true;
			this.m_btnAllReset.Click += new System.EventHandler(this.Click_State);
			// 
			// m_btnAllMotorOff
			// 
			this.m_btnAllMotorOff.BorderWidth = 3;
			this.m_btnAllMotorOff.ButtonClicked = false;
			this.m_btnAllMotorOff.ClickedEmphasizeTextColor = System.Drawing.Color.White;
			this.m_btnAllMotorOff.CustomClickedGradientFirstColor = System.Drawing.Color.White;
			this.m_btnAllMotorOff.CustomClickedGradientSecondColor = System.Drawing.Color.White;
			this.m_btnAllMotorOff.Description = "";
			this.m_btnAllMotorOff.DisabledColor = System.Drawing.Color.DarkGray;
			this.m_btnAllMotorOff.Dock = System.Windows.Forms.DockStyle.Fill;
			this.m_btnAllMotorOff.EdgeRadius = 5;
			this.m_btnAllMotorOff.GradientAngle = 70F;
			this.m_btnAllMotorOff.GradientFirstColor = System.Drawing.Color.White;
			this.m_btnAllMotorOff.GradientSecondColor = System.Drawing.Color.FromArgb(((int)(((byte)(170)))), ((int)(((byte)(176)))), ((int)(((byte)(183)))));
			this.m_btnAllMotorOff.HoverEmphasizeCustomColor = System.Drawing.Color.White;
			this.m_btnAllMotorOff.ImagePosition = new System.Drawing.Point(7, 7);
			this.m_btnAllMotorOff.ImageSize = new System.Drawing.Point(30, 30);
			this.m_btnAllMotorOff.LoadImage = global::FrameOfSystem3.Properties.Resources.Home_black;
			this.m_btnAllMotorOff.Location = new System.Drawing.Point(850, 842);
			this.m_btnAllMotorOff.MainFont = new System.Drawing.Font("맑은 고딕", 11F, System.Drawing.FontStyle.Bold);
			this.m_btnAllMotorOff.MainFontColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(36)))), ((int)(((byte)(0)))));
			this.m_btnAllMotorOff.Margin = new System.Windows.Forms.Padding(2);
			this.m_btnAllMotorOff.Name = "m_btnAllMotorOff";
			this.m_btnAllMotorOff.Size = new System.Drawing.Size(149, 56);
			this.m_btnAllMotorOff.SubFont = new System.Drawing.Font("맑은 고딕", 8F, System.Drawing.FontStyle.Bold);
			this.m_btnAllMotorOff.SubFontColor = System.Drawing.Color.DarkBlue;
			this.m_btnAllMotorOff.SubText = "STATUS";
			this.m_btnAllMotorOff.TabIndex = 3;
			this.m_btnAllMotorOff.Text = "ALL MOTOR OFF";
			this.m_btnAllMotorOff.TextAlignMain = Sys3Controls.EN_TEXTALIGN.BOTTOM_RIGHT;
			this.m_btnAllMotorOff.TextAlignSub = Sys3Controls.EN_TEXTALIGN.TOP_RIGHT;
			this.m_btnAllMotorOff.ThemeIndex = 0;
			this.m_btnAllMotorOff.UseBorder = true;
			this.m_btnAllMotorOff.UseClickedEmphasizeTextColor = false;
			this.m_btnAllMotorOff.UseCustomizeClickedColor = false;
			this.m_btnAllMotorOff.UseEdge = true;
			this.m_btnAllMotorOff.UseHoverEmphasizeCustomColor = false;
			this.m_btnAllMotorOff.UseImage = false;
			this.m_btnAllMotorOff.UserHoverEmpahsize = false;
			this.m_btnAllMotorOff.UseSubFont = true;
			this.m_btnAllMotorOff.Click += new System.EventHandler(this.Click_Status);
			// 
			// m_btnAllMotorOn
			// 
			this.m_btnAllMotorOn.BorderWidth = 3;
			this.m_btnAllMotorOn.ButtonClicked = false;
			this.m_btnAllMotorOn.ClickedEmphasizeTextColor = System.Drawing.Color.White;
			this.m_btnAllMotorOn.CustomClickedGradientFirstColor = System.Drawing.Color.White;
			this.m_btnAllMotorOn.CustomClickedGradientSecondColor = System.Drawing.Color.White;
			this.m_btnAllMotorOn.Description = "";
			this.m_btnAllMotorOn.DisabledColor = System.Drawing.Color.DarkGray;
			this.m_btnAllMotorOn.Dock = System.Windows.Forms.DockStyle.Fill;
			this.m_btnAllMotorOn.EdgeRadius = 5;
			this.m_btnAllMotorOn.GradientAngle = 70F;
			this.m_btnAllMotorOn.GradientFirstColor = System.Drawing.Color.White;
			this.m_btnAllMotorOn.GradientSecondColor = System.Drawing.Color.FromArgb(((int)(((byte)(170)))), ((int)(((byte)(176)))), ((int)(((byte)(183)))));
			this.m_btnAllMotorOn.HoverEmphasizeCustomColor = System.Drawing.Color.White;
			this.m_btnAllMotorOn.ImagePosition = new System.Drawing.Point(7, 7);
			this.m_btnAllMotorOn.ImageSize = new System.Drawing.Point(30, 30);
			this.m_btnAllMotorOn.LoadImage = global::FrameOfSystem3.Properties.Resources.Home_black;
			this.m_btnAllMotorOn.Location = new System.Drawing.Point(697, 842);
			this.m_btnAllMotorOn.MainFont = new System.Drawing.Font("맑은 고딕", 11F, System.Drawing.FontStyle.Bold);
			this.m_btnAllMotorOn.MainFontColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(36)))), ((int)(((byte)(0)))));
			this.m_btnAllMotorOn.Margin = new System.Windows.Forms.Padding(2);
			this.m_btnAllMotorOn.Name = "m_btnAllMotorOn";
			this.m_btnAllMotorOn.Size = new System.Drawing.Size(149, 56);
			this.m_btnAllMotorOn.SubFont = new System.Drawing.Font("맑은 고딕", 8F, System.Drawing.FontStyle.Bold);
			this.m_btnAllMotorOn.SubFontColor = System.Drawing.Color.DarkBlue;
			this.m_btnAllMotorOn.SubText = "STATUS";
			this.m_btnAllMotorOn.TabIndex = 2;
			this.m_btnAllMotorOn.Text = "ALL MOTOR ON";
			this.m_btnAllMotorOn.TextAlignMain = Sys3Controls.EN_TEXTALIGN.BOTTOM_RIGHT;
			this.m_btnAllMotorOn.TextAlignSub = Sys3Controls.EN_TEXTALIGN.TOP_RIGHT;
			this.m_btnAllMotorOn.ThemeIndex = 0;
			this.m_btnAllMotorOn.UseBorder = true;
			this.m_btnAllMotorOn.UseClickedEmphasizeTextColor = false;
			this.m_btnAllMotorOn.UseCustomizeClickedColor = false;
			this.m_btnAllMotorOn.UseEdge = true;
			this.m_btnAllMotorOn.UseHoverEmphasizeCustomColor = false;
			this.m_btnAllMotorOn.UseImage = false;
			this.m_btnAllMotorOn.UserHoverEmpahsize = false;
			this.m_btnAllMotorOn.UseSubFont = true;
			this.m_btnAllMotorOn.Click += new System.EventHandler(this.Click_Status);
			// 
			// m_labelScroll
			// 
			this.m_labelScroll.BackGroundColor = System.Drawing.Color.FromArgb(((int)(((byte)(208)))), ((int)(((byte)(208)))), ((int)(((byte)(208)))));
			this.m_labelScroll.BorderStroke = 2;
			this.m_labelScroll.BorderStyle = System.Windows.Forms.ButtonBorderStyle.Solid;
			this._tableLayoutPanel_Main.SetColumnSpan(this.m_labelScroll, 10);
			this.m_labelScroll.Description = "";
			this.m_labelScroll.DisabledColor = System.Drawing.Color.DarkGray;
			this.m_labelScroll.Dock = System.Windows.Forms.DockStyle.Fill;
			this.m_labelScroll.EdgeRadius = 1;
			this.m_labelScroll.ImagePosition = new System.Drawing.Point(0, 0);
			this.m_labelScroll.ImageSize = new System.Drawing.Point(0, 0);
			this.m_labelScroll.LoadImage = null;
			this.m_labelScroll.Location = new System.Drawing.Point(0, 191);
			this.m_labelScroll.MainFont = new System.Drawing.Font("맑은 고딕", 10F, System.Drawing.FontStyle.Bold);
			this.m_labelScroll.MainFontColor = System.Drawing.Color.Black;
			this.m_labelScroll.Margin = new System.Windows.Forms.Padding(0);
			this.m_labelScroll.Name = "m_labelScroll";
			this.m_labelScroll.Size = new System.Drawing.Size(1156, 26);
			this.m_labelScroll.SubFont = new System.Drawing.Font("맑은 고딕", 10F);
			this.m_labelScroll.SubFontColor = System.Drawing.Color.Black;
			this.m_labelScroll.SubText = "";
			this.m_labelScroll.TabIndex = 1386;
			this.m_labelScroll.Text = "▼";
			this.m_labelScroll.TextAlignMain = Sys3Controls.EN_TEXTALIGN.MIDDLE_CENTER;
			this.m_labelScroll.TextAlignSub = Sys3Controls.EN_TEXTALIGN.MIDDLE_CENTER;
			this.m_labelScroll.TextAlignUnit = Sys3Controls.EN_TEXTALIGN.MIDDLE_CENTER;
			this.m_labelScroll.ThemeIndex = 0;
			this.m_labelScroll.UnitAreaRate = 40;
			this.m_labelScroll.UnitFont = new System.Drawing.Font("맑은 고딕", 10F, System.Drawing.FontStyle.Bold);
			this.m_labelScroll.UnitFontColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
			this.m_labelScroll.UnitPositionVertical = false;
			this.m_labelScroll.UnitText = "";
			this.m_labelScroll.UseBorder = true;
			this.m_labelScroll.UseEdgeRadius = false;
			this.m_labelScroll.UseImage = false;
			this.m_labelScroll.UseSubFont = false;
			this.m_labelScroll.UseUnitFont = false;
			this.m_labelScroll.Click += new System.EventHandler(this.Click_LabelScroll);
			// 
			// _tableLayoutPanel_Main
			// 
			this._tableLayoutPanel_Main.BackColor = System.Drawing.Color.WhiteSmoke;
			this._tableLayoutPanel_Main.ColumnCount = 10;
			this._tableLayoutPanel_Main.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 11.89238F));
			this._tableLayoutPanel_Main.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 11.89238F));
			this._tableLayoutPanel_Main.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 10F));
			this._tableLayoutPanel_Main.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 11.89238F));
			this._tableLayoutPanel_Main.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 11.89238F));
			this._tableLayoutPanel_Main.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 11.89238F));
			this._tableLayoutPanel_Main.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 10F));
			this._tableLayoutPanel_Main.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 13.5127F));
			this._tableLayoutPanel_Main.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 13.5127F));
			this._tableLayoutPanel_Main.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 13.5127F));
			this._tableLayoutPanel_Main.Controls.Add(this.m_groupList, 0, 0);
			this._tableLayoutPanel_Main.Controls.Add(this.m_labelScroll, 0, 1);
			this._tableLayoutPanel_Main.Controls.Add(this.m_groupConfiguration, 0, 2);
			this._tableLayoutPanel_Main.Controls.Add(this.m_btnEnable, 0, 3);
			this._tableLayoutPanel_Main.Controls.Add(this.m_btnDisable, 1, 3);
			this._tableLayoutPanel_Main.Controls.Add(this.m_btnStop, 3, 3);
			this._tableLayoutPanel_Main.Controls.Add(this.m_btnAllStop, 4, 3);
			this._tableLayoutPanel_Main.Controls.Add(this.m_btnHome, 5, 3);
			this._tableLayoutPanel_Main.Controls.Add(this.m_btnMotorOn, 7, 3);
			this._tableLayoutPanel_Main.Controls.Add(this.m_btnAllReset, 9, 4);
			this._tableLayoutPanel_Main.Controls.Add(this.m_btnAllMotorOff, 8, 4);
			this._tableLayoutPanel_Main.Controls.Add(this.m_btnAllMotorOn, 7, 4);
			this._tableLayoutPanel_Main.Controls.Add(this.m_btnMotorOff, 8, 3);
			this._tableLayoutPanel_Main.Controls.Add(this.m_btnReset, 9, 3);
			this._tableLayoutPanel_Main.Dock = System.Windows.Forms.DockStyle.Fill;
			this._tableLayoutPanel_Main.Location = new System.Drawing.Point(0, 0);
			this._tableLayoutPanel_Main.Margin = new System.Windows.Forms.Padding(0);
			this._tableLayoutPanel_Main.Name = "_tableLayoutPanel_Main";
			this._tableLayoutPanel_Main.RowCount = 5;
			this._tableLayoutPanel_Main.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
			this._tableLayoutPanel_Main.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 26F));
			this._tableLayoutPanel_Main.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 563F));
			this._tableLayoutPanel_Main.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 60F));
			this._tableLayoutPanel_Main.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 60F));
			this._tableLayoutPanel_Main.Size = new System.Drawing.Size(1156, 900);
			this._tableLayoutPanel_Main.TabIndex = 1387;
			// 
			// _flowLayoutPanel_AddRemove
			// 
			this._flowLayoutPanel_AddRemove.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this._flowLayoutPanel_AddRemove.AutoSize = true;
			this._flowLayoutPanel_AddRemove.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
			this._flowLayoutPanel_AddRemove.BackColor = System.Drawing.Color.Transparent;
			this._flowLayoutPanel_AddRemove.Controls.Add(this.m_btnExtend);
			this._flowLayoutPanel_AddRemove.Controls.Add(this.m_btnRemove);
			this._flowLayoutPanel_AddRemove.Controls.Add(this.m_btnAdd);
			this._flowLayoutPanel_AddRemove.FlowDirection = System.Windows.Forms.FlowDirection.RightToLeft;
			this._flowLayoutPanel_AddRemove.Location = new System.Drawing.Point(837, 0);
			this._flowLayoutPanel_AddRemove.Margin = new System.Windows.Forms.Padding(0);
			this._flowLayoutPanel_AddRemove.Name = "_flowLayoutPanel_AddRemove";
			this._flowLayoutPanel_AddRemove.Size = new System.Drawing.Size(317, 41);
			this._flowLayoutPanel_AddRemove.TabIndex = 1388;
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
			this.lbl_Filtering.Location = new System.Drawing.Point(1, 0);
			this.lbl_Filtering.MainFont = new System.Drawing.Font("맑은 고딕", 12F, System.Drawing.FontStyle.Bold);
			this.lbl_Filtering.MainFontColor = System.Drawing.Color.Red;
			this.lbl_Filtering.Margin = new System.Windows.Forms.Padding(0);
			this.lbl_Filtering.Name = "lbl_Filtering";
			this.lbl_Filtering.Size = new System.Drawing.Size(511, 41);
			this.lbl_Filtering.SubFont = new System.Drawing.Font("맑은 고딕", 8F, System.Drawing.FontStyle.Bold);
			this.lbl_Filtering.SubFontColor = System.Drawing.Color.DodgerBlue;
			this.lbl_Filtering.SubText = "FILTERING";
			this.lbl_Filtering.TabIndex = 1349;
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
			// Config_Motion
			// 
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
			this.BackColor = System.Drawing.Color.White;
			this.Controls.Add(this._flowLayoutPanel_AddRemove);
			this.Controls.Add(this._tableLayoutPanel_Main);
			this.Name = "Config_Motion";
			this.Size = new System.Drawing.Size(1156, 900);
			((System.ComponentModel.ISupportInitialize)(this.m_dgViewList)).EndInit();
			this.m_panel.ResumeLayout(false);
			this.m_GroupBox.ResumeLayout(false);
			this.m_groupList.ResumeLayout(false);
			this.m_groupConfiguration.ResumeLayout(false);
			this._tableLayoutPanel_Configuration.ResumeLayout(false);
			this._tableLayoutPanel_Main.ResumeLayout(false);
			this._flowLayoutPanel_AddRemove.ResumeLayout(false);
			this.ResumeLayout(false);
			this.PerformLayout();

        }

        #endregion

		private Sys3Controls.Sys3DoubleBufferedDataGridView m_dgViewList;
		private System.Windows.Forms.Panel m_panel;
		private Sys3Controls.Sys3button m_btnExtend;
		private Sys3Controls.Sys3button m_btnRemove;
		private Sys3Controls.Sys3button m_btnAdd;
		private Sys3Controls.Sys3GroupBoxContainer m_groupList;
        private Sys3Controls.Sys3GroupBoxContainer m_groupConfiguration;
		private Sys3Controls.Sys3Label m_lblIndex;
		private Sys3Controls.Sys3Label m_lblTarget;
		private Sys3Controls.Sys3Label m_lblName;
		private Sys3Controls.Sys3Label m_lblPower;
		private Sys3Controls.Sys3Label m_labelIndex;
		private Sys3Controls.Sys3Label m_labelTarget;
		private Sys3Controls.Sys3Label m_labelName;
		private Sys3Controls.Sys3Label m_labelPower;
		private Sys3Controls.Sys3button m_btnDisable;
		private Sys3Controls.Sys3button m_btnEnable;
		private Sys3Controls.Sys3button m_btnAllStop;
		private Sys3Controls.Sys3button m_btnStop;
		private Sys3Controls.Sys3button m_btnHome;
		private Sys3Controls.Sys3button m_btnMotorOn;
		private Sys3Controls.Sys3button m_btnMotorOff;
		private Sys3Controls.Sys3button m_btnReset;
		private Sys3Controls.Sys3button m_btnAllReset;
		private Sys3Controls.Sys3button m_btnAllMotorOff;
		private Sys3Controls.Sys3button m_btnAllMotorOn;
		private Sys3Controls.Sys3button m_tabState;
		private Sys3Controls.Sys3button m_tabGeneral;
		private Sys3Controls.Sys3button m_tabSpeed;
		private Sys3Controls.Sys3button m_tabHome;
		private Sys3Controls.Sys3button m_tabGantry;
		private Sys3Controls.Sys3Label m_labelScroll;
		private Sys3Controls.Sys3Label m_lblTag;
        private Sys3Controls.Sys3Label m_labelTag;
		private Sys3Controls.Sys3button m_tabLimit;
        private Sys3Controls.Sys3ProgressBar m_ProgressBar;
        private Sys3Controls.Sys3GroupBoxContainer m_GroupBox;
        private System.Windows.Forms.TableLayoutPanel _tableLayoutPanel_Main;
        private System.Windows.Forms.FlowLayoutPanel _flowLayoutPanel_AddRemove;
        private System.Windows.Forms.TableLayoutPanel _tableLayoutPanel_Configuration;
        private System.Windows.Forms.DataGridViewTextBoxColumn ID;
        private System.Windows.Forms.DataGridViewTextBoxColumn PASSWORD;
        private System.Windows.Forms.DataGridViewTextBoxColumn TARGET;
        private System.Windows.Forms.DataGridViewTextBoxColumn ENABLE;
        private System.Windows.Forms.DataGridViewTextBoxColumn AUTHORITY;
        private System.Windows.Forms.DataGridViewTextBoxColumn CONDITION;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column2;
        private System.Windows.Forms.DataGridViewTextBoxColumn BLINK;
        private System.Windows.Forms.DataGridViewTextBoxColumn Led;
        private System.Windows.Forms.DataGridViewTextBoxColumn MOTION;
		private Sys3Controls.Sys3Label lbl_Filtering;
    }
}
