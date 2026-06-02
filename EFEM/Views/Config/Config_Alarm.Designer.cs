namespace FrameOfSystem3.Views.Config
{
    partial class Config_Alarm
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
            this.m_dgViewAlarm = new Sys3Controls.Sys3DoubleBufferedDataGridView();
            this.INDEX = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ID = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.GRADE = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.PASSWORD = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.SOLUTIONCODE = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.MONITORING = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.m_groupConfiguration = new Sys3Controls.Sys3GroupBoxContainer();
            this._tableLayoutPanel_Configuration = new System.Windows.Forms.TableLayoutPanel();
            this.m_lblAlarmCode = new Sys3Controls.Sys3Label();
            this.m_lblBuzzerIO = new Sys3Controls.Sys3Label();
            this.m_btnAdd = new Sys3Controls.Sys3button();
            this._btnDiffuse = new Sys3Controls.Sys3button();
            this.m_lblAlarmGrade = new Sys3Controls.Sys3Label();
            this.m_labelSolutionCode = new Sys3Controls.Sys3Label();
            this.m_labelAlarmGrade = new Sys3Controls.Sys3Label();
            this.m_lblMessageCode = new Sys3Controls.Sys3Label();
            this.m_lblSolutionCode = new Sys3Controls.Sys3Label();
            this.m_labelAlarmCode = new Sys3Controls.Sys3Label();
            this.m_labelMessageCode = new Sys3Controls.Sys3Label();
            this.m_labelBuzzerIO = new Sys3Controls.Sys3Label();
            this.m_btnRemove = new Sys3Controls.Sys3button();
            this.m_groupSelectedItem = new Sys3Controls.Sys3GroupBoxContainer();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.m_lblIndex = new Sys3Controls.Sys3Label();
            this.m_labelIndex = new Sys3Controls.Sys3Label();
            this._btnExport = new Sys3Controls.Sys3button();
            this.m_groupList = new Sys3Controls.Sys3GroupBoxContainer();
            this._tableLayoutPanel_Main = new System.Windows.Forms.TableLayoutPanel();
            ((System.ComponentModel.ISupportInitialize)(this.m_dgViewAlarm)).BeginInit();
            this.m_groupConfiguration.SuspendLayout();
            this._tableLayoutPanel_Configuration.SuspendLayout();
            this.m_groupSelectedItem.SuspendLayout();
            this.tableLayoutPanel1.SuspendLayout();
            this.m_groupList.SuspendLayout();
            this._tableLayoutPanel_Main.SuspendLayout();
            this.SuspendLayout();
            // 
            // m_dgViewAlarm
            // 
            this.m_dgViewAlarm.AllowUserToAddRows = false;
            this.m_dgViewAlarm.AllowUserToDeleteRows = false;
            this.m_dgViewAlarm.AllowUserToResizeColumns = false;
            this.m_dgViewAlarm.AllowUserToResizeRows = false;
            this.m_dgViewAlarm.BackgroundColor = System.Drawing.Color.White;
            this.m_dgViewAlarm.CellBorderStyle = System.Windows.Forms.DataGridViewCellBorderStyle.Raised;
            this.m_dgViewAlarm.ColumnHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.Single;
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle1.BackColor = System.Drawing.Color.LightGray;
            dataGridViewCellStyle1.Font = new System.Drawing.Font("맑은 고딕", 11F, System.Drawing.FontStyle.Bold);
            dataGridViewCellStyle1.ForeColor = System.Drawing.Color.Black;
            dataGridViewCellStyle1.SelectionBackColor = System.Drawing.Color.White;
            dataGridViewCellStyle1.SelectionForeColor = System.Drawing.Color.Black;
            dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.m_dgViewAlarm.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
            this.m_dgViewAlarm.ColumnHeadersHeight = 30;
            this.m_dgViewAlarm.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            this.m_dgViewAlarm.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.INDEX,
            this.ID,
            this.GRADE,
            this.PASSWORD,
            this.SOLUTIONCODE,
            this.MONITORING});
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle2.BackColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle2.Font = new System.Drawing.Font("맑은 고딕", 10F, System.Drawing.FontStyle.Bold);
            dataGridViewCellStyle2.ForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle2.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(120)))), ((int)(((byte)(130)))), ((int)(((byte)(150)))));
            dataGridViewCellStyle2.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.m_dgViewAlarm.DefaultCellStyle = dataGridViewCellStyle2;
            this.m_dgViewAlarm.Dock = System.Windows.Forms.DockStyle.Fill;
            this.m_dgViewAlarm.EnableHeadersVisualStyles = false;
            this.m_dgViewAlarm.GridColor = System.Drawing.Color.FromArgb(((int)(((byte)(120)))), ((int)(((byte)(130)))), ((int)(((byte)(150)))));
            this.m_dgViewAlarm.Location = new System.Drawing.Point(3, 32);
            this.m_dgViewAlarm.MultiSelect = false;
            this.m_dgViewAlarm.Name = "m_dgViewAlarm";
            this.m_dgViewAlarm.ReadOnly = true;
            this.m_dgViewAlarm.RowHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.None;
            dataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle3.BackColor = System.Drawing.Color.White;
            dataGridViewCellStyle3.Font = new System.Drawing.Font("맑은 고딕", 11F);
            dataGridViewCellStyle3.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle3.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(120)))), ((int)(((byte)(130)))), ((int)(((byte)(150)))));
            dataGridViewCellStyle3.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle3.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.m_dgViewAlarm.RowHeadersDefaultCellStyle = dataGridViewCellStyle3;
            this.m_dgViewAlarm.RowHeadersVisible = false;
            this.m_dgViewAlarm.RowHeadersWidthSizeMode = System.Windows.Forms.DataGridViewRowHeadersWidthSizeMode.DisableResizing;
            this.m_dgViewAlarm.RowTemplate.Height = 23;
            this.m_dgViewAlarm.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.m_dgViewAlarm.Size = new System.Drawing.Size(1150, 595);
            this.m_dgViewAlarm.TabIndex = 1255;
            this.m_dgViewAlarm.CellClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.Click_Dgview);
            // 
            // INDEX
            // 
            this.INDEX.HeaderText = "INDEX";
            this.INDEX.Name = "INDEX";
            this.INDEX.ReadOnly = true;
            this.INDEX.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.INDEX.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.INDEX.Width = 85;
            // 
            // ID
            // 
            this.ID.HeaderText = "ALARM CODE";
            this.ID.MaxInputLength = 20;
            this.ID.Name = "ID";
            this.ID.ReadOnly = true;
            this.ID.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.ID.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.ID.Width = 140;
            // 
            // GRADE
            // 
            this.GRADE.HeaderText = "GRADE";
            this.GRADE.Name = "GRADE";
            this.GRADE.ReadOnly = true;
            this.GRADE.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.GRADE.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // PASSWORD
            // 
            this.PASSWORD.HeaderText = "MESSAGE";
            this.PASSWORD.MaxInputLength = 20;
            this.PASSWORD.Name = "PASSWORD";
            this.PASSWORD.ReadOnly = true;
            this.PASSWORD.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.PASSWORD.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.PASSWORD.Width = 105;
            // 
            // SOLUTIONCODE
            // 
            this.SOLUTIONCODE.HeaderText = "SOLUTION";
            this.SOLUTIONCODE.Name = "SOLUTIONCODE";
            this.SOLUTIONCODE.ReadOnly = true;
            this.SOLUTIONCODE.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.SOLUTIONCODE.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.SOLUTIONCODE.Width = 113;
            // 
            // MONITORING
            // 
            this.MONITORING.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.MONITORING.HeaderText = "MESSAGE";
            this.MONITORING.Name = "MONITORING";
            this.MONITORING.ReadOnly = true;
            this.MONITORING.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.MONITORING.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // m_groupConfiguration
            // 
            this.m_groupConfiguration.BackGroundColor = System.Drawing.Color.WhiteSmoke;
            this.m_groupConfiguration.Controls.Add(this._tableLayoutPanel_Configuration);
            this.m_groupConfiguration.Dock = System.Windows.Forms.DockStyle.Fill;
            this.m_groupConfiguration.EdgeBorderStroke = 2;
            this.m_groupConfiguration.EdgeRadius = 2;
            this.m_groupConfiguration.LabelFont = new System.Drawing.Font("맑은 고딕", 10F, System.Drawing.FontStyle.Bold);
            this.m_groupConfiguration.LabelGradientColorFirst = System.Drawing.Color.FromArgb(((int)(((byte)(208)))), ((int)(((byte)(208)))), ((int)(((byte)(208)))));
            this.m_groupConfiguration.LabelGradientColorSecond = System.Drawing.Color.FromArgb(((int)(((byte)(240)))), ((int)(((byte)(240)))), ((int)(((byte)(240)))));
            this.m_groupConfiguration.LabelHeight = 30;
            this.m_groupConfiguration.LabelTextColor = System.Drawing.Color.Black;
            this.m_groupConfiguration.Location = new System.Drawing.Point(0, 720);
            this.m_groupConfiguration.Margin = new System.Windows.Forms.Padding(0);
            this.m_groupConfiguration.Name = "m_groupConfiguration";
            this.m_groupConfiguration.Padding = new System.Windows.Forms.Padding(3, 18, 3, 3);
            this.m_groupConfiguration.Size = new System.Drawing.Size(1156, 180);
            this.m_groupConfiguration.TabIndex = 1352;
            this.m_groupConfiguration.TabStop = false;
            this.m_groupConfiguration.Text = "CONFIGURATION";
            this.m_groupConfiguration.TextAlign = Sys3Controls.EN_TEXTALIGN.MIDDLE_CENTER;
            this.m_groupConfiguration.ThemeIndex = 0;
            this.m_groupConfiguration.UseLabelBorder = true;
            this.m_groupConfiguration.UseTitle = true;
            // 
            // _tableLayoutPanel_Configuration
            // 
            this._tableLayoutPanel_Configuration.ColumnCount = 10;
            this._tableLayoutPanel_Configuration.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 2.13017F));
            this._tableLayoutPanel_Configuration.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 11.62027F));
            this._tableLayoutPanel_Configuration.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 17.43041F));
            this._tableLayoutPanel_Configuration.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 2.13017F));
            this._tableLayoutPanel_Configuration.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 11.62027F));
            this._tableLayoutPanel_Configuration.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 17.37834F));
            this._tableLayoutPanel_Configuration.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 14.13575F));
            this._tableLayoutPanel_Configuration.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 10.71157F));
            this._tableLayoutPanel_Configuration.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 10.71288F));
            this._tableLayoutPanel_Configuration.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 2.13017F));
            this._tableLayoutPanel_Configuration.Controls.Add(this.m_lblAlarmCode, 1, 1);
            this._tableLayoutPanel_Configuration.Controls.Add(this.m_lblBuzzerIO, 4, 2);
            this._tableLayoutPanel_Configuration.Controls.Add(this.m_btnAdd, 7, 1);
            this._tableLayoutPanel_Configuration.Controls.Add(this._btnDiffuse, 6, 3);
            this._tableLayoutPanel_Configuration.Controls.Add(this.m_lblAlarmGrade, 1, 2);
            this._tableLayoutPanel_Configuration.Controls.Add(this.m_labelSolutionCode, 2, 3);
            this._tableLayoutPanel_Configuration.Controls.Add(this.m_labelAlarmGrade, 2, 2);
            this._tableLayoutPanel_Configuration.Controls.Add(this.m_lblMessageCode, 4, 1);
            this._tableLayoutPanel_Configuration.Controls.Add(this.m_lblSolutionCode, 1, 3);
            this._tableLayoutPanel_Configuration.Controls.Add(this.m_labelAlarmCode, 2, 1);
            this._tableLayoutPanel_Configuration.Controls.Add(this.m_labelMessageCode, 5, 1);
            this._tableLayoutPanel_Configuration.Controls.Add(this.m_labelBuzzerIO, 5, 2);
            this._tableLayoutPanel_Configuration.Controls.Add(this.m_btnRemove, 8, 1);
            this._tableLayoutPanel_Configuration.Dock = System.Windows.Forms.DockStyle.Fill;
            this._tableLayoutPanel_Configuration.Location = new System.Drawing.Point(3, 32);
            this._tableLayoutPanel_Configuration.Margin = new System.Windows.Forms.Padding(0);
            this._tableLayoutPanel_Configuration.Name = "_tableLayoutPanel_Configuration";
            this._tableLayoutPanel_Configuration.RowCount = 5;
            this._tableLayoutPanel_Configuration.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 1.612893F));
            this._tableLayoutPanel_Configuration.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 32.25786F));
            this._tableLayoutPanel_Configuration.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 32.25818F));
            this._tableLayoutPanel_Configuration.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 32.25818F));
            this._tableLayoutPanel_Configuration.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 1.612893F));
            this._tableLayoutPanel_Configuration.Size = new System.Drawing.Size(1150, 145);
            this._tableLayoutPanel_Configuration.TabIndex = 0;
            // 
            // m_lblAlarmCode
            // 
            this.m_lblAlarmCode.BackGroundColor = System.Drawing.Color.FromArgb(((int)(((byte)(200)))), ((int)(((byte)(200)))), ((int)(((byte)(220)))));
            this.m_lblAlarmCode.BorderStroke = 2;
            this.m_lblAlarmCode.BorderStyle = System.Windows.Forms.ButtonBorderStyle.Solid;
            this.m_lblAlarmCode.Description = "";
            this.m_lblAlarmCode.DisabledColor = System.Drawing.Color.DarkGray;
            this.m_lblAlarmCode.Dock = System.Windows.Forms.DockStyle.Fill;
            this.m_lblAlarmCode.EdgeRadius = 1;
            this.m_lblAlarmCode.ImagePosition = new System.Drawing.Point(0, 0);
            this.m_lblAlarmCode.ImageSize = new System.Drawing.Point(0, 0);
            this.m_lblAlarmCode.LoadImage = null;
            this.m_lblAlarmCode.Location = new System.Drawing.Point(26, 4);
            this.m_lblAlarmCode.MainFont = new System.Drawing.Font("맑은 고딕", 9F, System.Drawing.FontStyle.Bold);
            this.m_lblAlarmCode.MainFontColor = System.Drawing.Color.Black;
            this.m_lblAlarmCode.Margin = new System.Windows.Forms.Padding(2);
            this.m_lblAlarmCode.Name = "m_lblAlarmCode";
            this.m_lblAlarmCode.Size = new System.Drawing.Size(129, 42);
            this.m_lblAlarmCode.SubFont = new System.Drawing.Font("맑은 고딕", 10F);
            this.m_lblAlarmCode.SubFontColor = System.Drawing.Color.Black;
            this.m_lblAlarmCode.SubText = "";
            this.m_lblAlarmCode.TabIndex = 1388;
            this.m_lblAlarmCode.Text = "ALARM CODE";
            this.m_lblAlarmCode.TextAlignMain = Sys3Controls.EN_TEXTALIGN.TOP_LEFT;
            this.m_lblAlarmCode.TextAlignSub = Sys3Controls.EN_TEXTALIGN.MIDDLE_CENTER;
            this.m_lblAlarmCode.TextAlignUnit = Sys3Controls.EN_TEXTALIGN.MIDDLE_CENTER;
            this.m_lblAlarmCode.ThemeIndex = 0;
            this.m_lblAlarmCode.UnitAreaRate = 40;
            this.m_lblAlarmCode.UnitFont = new System.Drawing.Font("맑은 고딕", 10F, System.Drawing.FontStyle.Bold);
            this.m_lblAlarmCode.UnitFontColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.m_lblAlarmCode.UnitPositionVertical = false;
            this.m_lblAlarmCode.UnitText = "";
            this.m_lblAlarmCode.UseBorder = true;
            this.m_lblAlarmCode.UseEdgeRadius = false;
            this.m_lblAlarmCode.UseImage = false;
            this.m_lblAlarmCode.UseSubFont = false;
            this.m_lblAlarmCode.UseUnitFont = false;
            // 
            // m_lblBuzzerIO
            // 
            this.m_lblBuzzerIO.BackGroundColor = System.Drawing.Color.FromArgb(((int)(((byte)(200)))), ((int)(((byte)(200)))), ((int)(((byte)(220)))));
            this.m_lblBuzzerIO.BorderStroke = 2;
            this.m_lblBuzzerIO.BorderStyle = System.Windows.Forms.ButtonBorderStyle.Solid;
            this.m_lblBuzzerIO.Description = "";
            this.m_lblBuzzerIO.DisabledColor = System.Drawing.Color.DarkGray;
            this.m_lblBuzzerIO.Dock = System.Windows.Forms.DockStyle.Fill;
            this.m_lblBuzzerIO.EdgeRadius = 1;
            this.m_lblBuzzerIO.ImagePosition = new System.Drawing.Point(0, 0);
            this.m_lblBuzzerIO.ImageSize = new System.Drawing.Point(0, 0);
            this.m_lblBuzzerIO.LoadImage = null;
            this.m_lblBuzzerIO.Location = new System.Drawing.Point(383, 50);
            this.m_lblBuzzerIO.MainFont = new System.Drawing.Font("맑은 고딕", 9F, System.Drawing.FontStyle.Bold);
            this.m_lblBuzzerIO.MainFontColor = System.Drawing.Color.Black;
            this.m_lblBuzzerIO.Margin = new System.Windows.Forms.Padding(2);
            this.m_lblBuzzerIO.Name = "m_lblBuzzerIO";
            this.m_lblBuzzerIO.Size = new System.Drawing.Size(129, 42);
            this.m_lblBuzzerIO.SubFont = new System.Drawing.Font("맑은 고딕", 10F);
            this.m_lblBuzzerIO.SubFontColor = System.Drawing.Color.Black;
            this.m_lblBuzzerIO.SubText = "";
            this.m_lblBuzzerIO.TabIndex = 1388;
            this.m_lblBuzzerIO.Text = "BUZZER OUTPUT";
            this.m_lblBuzzerIO.TextAlignMain = Sys3Controls.EN_TEXTALIGN.TOP_LEFT;
            this.m_lblBuzzerIO.TextAlignSub = Sys3Controls.EN_TEXTALIGN.MIDDLE_CENTER;
            this.m_lblBuzzerIO.TextAlignUnit = Sys3Controls.EN_TEXTALIGN.MIDDLE_CENTER;
            this.m_lblBuzzerIO.ThemeIndex = 0;
            this.m_lblBuzzerIO.UnitAreaRate = 40;
            this.m_lblBuzzerIO.UnitFont = new System.Drawing.Font("맑은 고딕", 10F, System.Drawing.FontStyle.Bold);
            this.m_lblBuzzerIO.UnitFontColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.m_lblBuzzerIO.UnitPositionVertical = false;
            this.m_lblBuzzerIO.UnitText = "";
            this.m_lblBuzzerIO.UseBorder = true;
            this.m_lblBuzzerIO.UseEdgeRadius = false;
            this.m_lblBuzzerIO.UseImage = false;
            this.m_lblBuzzerIO.UseSubFont = false;
            this.m_lblBuzzerIO.UseUnitFont = false;
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
            this.m_btnAdd.GradientAngle = 60F;
            this.m_btnAdd.GradientFirstColor = System.Drawing.Color.White;
            this.m_btnAdd.GradientSecondColor = System.Drawing.Color.FromArgb(((int)(((byte)(170)))), ((int)(((byte)(176)))), ((int)(((byte)(183)))));
            this.m_btnAdd.HoverEmphasizeCustomColor = System.Drawing.Color.White;
            this.m_btnAdd.ImagePosition = new System.Drawing.Point(15, 15);
            this.m_btnAdd.ImageSize = new System.Drawing.Point(30, 30);
            this.m_btnAdd.LoadImage = global::FrameOfSystem3.Properties.Resources.CONFIG_ADD3;
            this.m_btnAdd.Location = new System.Drawing.Point(878, 5);
            this.m_btnAdd.MainFont = new System.Drawing.Font("맑은 고딕", 15F, System.Drawing.FontStyle.Bold);
            this.m_btnAdd.MainFontColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(36)))), ((int)(((byte)(0)))));
            this.m_btnAdd.Name = "m_btnAdd";
            this._tableLayoutPanel_Configuration.SetRowSpan(this.m_btnAdd, 3);
            this.m_btnAdd.Size = new System.Drawing.Size(117, 132);
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
            this.m_btnAdd.Click += new System.EventHandler(this.Click_Buttons);
            // 
            // _btnDiffuse
            // 
            this._btnDiffuse.BorderWidth = 3;
            this._btnDiffuse.ButtonClicked = false;
            this._btnDiffuse.ClickedEmphasizeTextColor = System.Drawing.Color.White;
            this._btnDiffuse.CustomClickedGradientFirstColor = System.Drawing.Color.White;
            this._btnDiffuse.CustomClickedGradientSecondColor = System.Drawing.Color.White;
            this._btnDiffuse.Description = "";
            this._btnDiffuse.DisabledColor = System.Drawing.Color.DarkGray;
            this._btnDiffuse.Dock = System.Windows.Forms.DockStyle.Fill;
            this._btnDiffuse.EdgeRadius = 5;
            this._btnDiffuse.GradientAngle = 60F;
            this._btnDiffuse.GradientFirstColor = System.Drawing.Color.White;
            this._btnDiffuse.GradientSecondColor = System.Drawing.Color.FromArgb(((int)(((byte)(170)))), ((int)(((byte)(176)))), ((int)(((byte)(183)))));
            this._btnDiffuse.HoverEmphasizeCustomColor = System.Drawing.Color.White;
            this._btnDiffuse.ImagePosition = new System.Drawing.Point(15, 15);
            this._btnDiffuse.ImageSize = new System.Drawing.Point(30, 30);
            this._btnDiffuse.LoadImage = null;
            this._btnDiffuse.Location = new System.Drawing.Point(716, 97);
            this._btnDiffuse.MainFont = new System.Drawing.Font("맑은 고딕", 15F, System.Drawing.FontStyle.Bold);
            this._btnDiffuse.MainFontColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(36)))), ((int)(((byte)(0)))));
            this._btnDiffuse.Name = "_btnDiffuse";
            this._btnDiffuse.Size = new System.Drawing.Size(156, 40);
            this._btnDiffuse.SubFont = new System.Drawing.Font("맑은 고딕", 8F, System.Drawing.FontStyle.Bold);
            this._btnDiffuse.SubFontColor = System.Drawing.Color.DarkBlue;
            this._btnDiffuse.SubText = "STATUS";
            this._btnDiffuse.TabIndex = 0;
            this._btnDiffuse.Text = "DIFFUSE";
            this._btnDiffuse.TextAlignMain = Sys3Controls.EN_TEXTALIGN.MIDDLE_RIGHT;
            this._btnDiffuse.TextAlignSub = Sys3Controls.EN_TEXTALIGN.TOP_LEFT;
            this._btnDiffuse.ThemeIndex = 0;
            this._btnDiffuse.UseBorder = true;
            this._btnDiffuse.UseClickedEmphasizeTextColor = false;
            this._btnDiffuse.UseCustomizeClickedColor = false;
            this._btnDiffuse.UseEdge = true;
            this._btnDiffuse.UseHoverEmphasizeCustomColor = false;
            this._btnDiffuse.UseImage = true;
            this._btnDiffuse.UserHoverEmpahsize = false;
            this._btnDiffuse.UseSubFont = false;
            this._btnDiffuse.Click += new System.EventHandler(this.Click_BuzzerDiffuse);
            // 
            // m_lblAlarmGrade
            // 
            this.m_lblAlarmGrade.BackGroundColor = System.Drawing.Color.FromArgb(((int)(((byte)(200)))), ((int)(((byte)(200)))), ((int)(((byte)(220)))));
            this.m_lblAlarmGrade.BorderStroke = 2;
            this.m_lblAlarmGrade.BorderStyle = System.Windows.Forms.ButtonBorderStyle.Solid;
            this.m_lblAlarmGrade.Description = "";
            this.m_lblAlarmGrade.DisabledColor = System.Drawing.Color.DarkGray;
            this.m_lblAlarmGrade.Dock = System.Windows.Forms.DockStyle.Fill;
            this.m_lblAlarmGrade.EdgeRadius = 1;
            this.m_lblAlarmGrade.ImagePosition = new System.Drawing.Point(0, 0);
            this.m_lblAlarmGrade.ImageSize = new System.Drawing.Point(0, 0);
            this.m_lblAlarmGrade.LoadImage = null;
            this.m_lblAlarmGrade.Location = new System.Drawing.Point(26, 50);
            this.m_lblAlarmGrade.MainFont = new System.Drawing.Font("맑은 고딕", 9F, System.Drawing.FontStyle.Bold);
            this.m_lblAlarmGrade.MainFontColor = System.Drawing.Color.Black;
            this.m_lblAlarmGrade.Margin = new System.Windows.Forms.Padding(2);
            this.m_lblAlarmGrade.Name = "m_lblAlarmGrade";
            this.m_lblAlarmGrade.Size = new System.Drawing.Size(129, 42);
            this.m_lblAlarmGrade.SubFont = new System.Drawing.Font("맑은 고딕", 10F);
            this.m_lblAlarmGrade.SubFontColor = System.Drawing.Color.Black;
            this.m_lblAlarmGrade.SubText = "";
            this.m_lblAlarmGrade.TabIndex = 1388;
            this.m_lblAlarmGrade.Text = "ALARM GRADE";
            this.m_lblAlarmGrade.TextAlignMain = Sys3Controls.EN_TEXTALIGN.TOP_LEFT;
            this.m_lblAlarmGrade.TextAlignSub = Sys3Controls.EN_TEXTALIGN.MIDDLE_CENTER;
            this.m_lblAlarmGrade.TextAlignUnit = Sys3Controls.EN_TEXTALIGN.MIDDLE_CENTER;
            this.m_lblAlarmGrade.ThemeIndex = 0;
            this.m_lblAlarmGrade.UnitAreaRate = 40;
            this.m_lblAlarmGrade.UnitFont = new System.Drawing.Font("맑은 고딕", 10F, System.Drawing.FontStyle.Bold);
            this.m_lblAlarmGrade.UnitFontColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.m_lblAlarmGrade.UnitPositionVertical = false;
            this.m_lblAlarmGrade.UnitText = "";
            this.m_lblAlarmGrade.UseBorder = true;
            this.m_lblAlarmGrade.UseEdgeRadius = false;
            this.m_lblAlarmGrade.UseImage = false;
            this.m_lblAlarmGrade.UseSubFont = false;
            this.m_lblAlarmGrade.UseUnitFont = false;
            // 
            // m_labelSolutionCode
            // 
            this.m_labelSolutionCode.BackGroundColor = System.Drawing.Color.White;
            this.m_labelSolutionCode.BorderStroke = 2;
            this.m_labelSolutionCode.BorderStyle = System.Windows.Forms.ButtonBorderStyle.Solid;
            this.m_labelSolutionCode.Description = "";
            this.m_labelSolutionCode.DisabledColor = System.Drawing.Color.LightGray;
            this.m_labelSolutionCode.Dock = System.Windows.Forms.DockStyle.Fill;
            this.m_labelSolutionCode.EdgeRadius = 1;
            this.m_labelSolutionCode.Enabled = false;
            this.m_labelSolutionCode.ImagePosition = new System.Drawing.Point(0, 0);
            this.m_labelSolutionCode.ImageSize = new System.Drawing.Point(0, 0);
            this.m_labelSolutionCode.LoadImage = null;
            this.m_labelSolutionCode.Location = new System.Drawing.Point(159, 96);
            this.m_labelSolutionCode.MainFont = new System.Drawing.Font("맑은 고딕", 11F, System.Drawing.FontStyle.Bold);
            this.m_labelSolutionCode.MainFontColor = System.Drawing.Color.Black;
            this.m_labelSolutionCode.Margin = new System.Windows.Forms.Padding(2);
            this.m_labelSolutionCode.Name = "m_labelSolutionCode";
            this.m_labelSolutionCode.Size = new System.Drawing.Size(196, 42);
            this.m_labelSolutionCode.SubFont = new System.Drawing.Font("맑은 고딕", 10F);
            this.m_labelSolutionCode.SubFontColor = System.Drawing.Color.Black;
            this.m_labelSolutionCode.SubText = "";
            this.m_labelSolutionCode.TabIndex = 2;
            this.m_labelSolutionCode.Text = "--";
            this.m_labelSolutionCode.TextAlignMain = Sys3Controls.EN_TEXTALIGN.BOTTOM_RIGHT;
            this.m_labelSolutionCode.TextAlignSub = Sys3Controls.EN_TEXTALIGN.MIDDLE_CENTER;
            this.m_labelSolutionCode.TextAlignUnit = Sys3Controls.EN_TEXTALIGN.MIDDLE_CENTER;
            this.m_labelSolutionCode.ThemeIndex = 0;
            this.m_labelSolutionCode.UnitAreaRate = 40;
            this.m_labelSolutionCode.UnitFont = new System.Drawing.Font("맑은 고딕", 10F, System.Drawing.FontStyle.Bold);
            this.m_labelSolutionCode.UnitFontColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.m_labelSolutionCode.UnitPositionVertical = false;
            this.m_labelSolutionCode.UnitText = "";
            this.m_labelSolutionCode.UseBorder = true;
            this.m_labelSolutionCode.UseEdgeRadius = false;
            this.m_labelSolutionCode.UseImage = false;
            this.m_labelSolutionCode.UseSubFont = false;
            this.m_labelSolutionCode.UseUnitFont = false;
            this.m_labelSolutionCode.Click += new System.EventHandler(this.Click_Configuration);
            // 
            // m_labelAlarmGrade
            // 
            this.m_labelAlarmGrade.BackGroundColor = System.Drawing.Color.White;
            this.m_labelAlarmGrade.BorderStroke = 2;
            this.m_labelAlarmGrade.BorderStyle = System.Windows.Forms.ButtonBorderStyle.Solid;
            this.m_labelAlarmGrade.Description = "";
            this.m_labelAlarmGrade.DisabledColor = System.Drawing.Color.LightGray;
            this.m_labelAlarmGrade.Dock = System.Windows.Forms.DockStyle.Fill;
            this.m_labelAlarmGrade.EdgeRadius = 1;
            this.m_labelAlarmGrade.Enabled = false;
            this.m_labelAlarmGrade.ImagePosition = new System.Drawing.Point(0, 0);
            this.m_labelAlarmGrade.ImageSize = new System.Drawing.Point(0, 0);
            this.m_labelAlarmGrade.LoadImage = null;
            this.m_labelAlarmGrade.Location = new System.Drawing.Point(159, 50);
            this.m_labelAlarmGrade.MainFont = new System.Drawing.Font("맑은 고딕", 11F, System.Drawing.FontStyle.Bold);
            this.m_labelAlarmGrade.MainFontColor = System.Drawing.Color.Black;
            this.m_labelAlarmGrade.Margin = new System.Windows.Forms.Padding(2);
            this.m_labelAlarmGrade.Name = "m_labelAlarmGrade";
            this.m_labelAlarmGrade.Size = new System.Drawing.Size(196, 42);
            this.m_labelAlarmGrade.SubFont = new System.Drawing.Font("맑은 고딕", 10F);
            this.m_labelAlarmGrade.SubFontColor = System.Drawing.Color.Black;
            this.m_labelAlarmGrade.SubText = "";
            this.m_labelAlarmGrade.TabIndex = 1;
            this.m_labelAlarmGrade.Text = "--";
            this.m_labelAlarmGrade.TextAlignMain = Sys3Controls.EN_TEXTALIGN.BOTTOM_RIGHT;
            this.m_labelAlarmGrade.TextAlignSub = Sys3Controls.EN_TEXTALIGN.MIDDLE_CENTER;
            this.m_labelAlarmGrade.TextAlignUnit = Sys3Controls.EN_TEXTALIGN.MIDDLE_CENTER;
            this.m_labelAlarmGrade.ThemeIndex = 0;
            this.m_labelAlarmGrade.UnitAreaRate = 40;
            this.m_labelAlarmGrade.UnitFont = new System.Drawing.Font("맑은 고딕", 10F, System.Drawing.FontStyle.Bold);
            this.m_labelAlarmGrade.UnitFontColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.m_labelAlarmGrade.UnitPositionVertical = false;
            this.m_labelAlarmGrade.UnitText = "";
            this.m_labelAlarmGrade.UseBorder = true;
            this.m_labelAlarmGrade.UseEdgeRadius = false;
            this.m_labelAlarmGrade.UseImage = false;
            this.m_labelAlarmGrade.UseSubFont = false;
            this.m_labelAlarmGrade.UseUnitFont = false;
            this.m_labelAlarmGrade.Click += new System.EventHandler(this.Click_Configuration);
            // 
            // m_lblMessageCode
            // 
            this.m_lblMessageCode.BackGroundColor = System.Drawing.Color.FromArgb(((int)(((byte)(200)))), ((int)(((byte)(200)))), ((int)(((byte)(220)))));
            this.m_lblMessageCode.BorderStroke = 2;
            this.m_lblMessageCode.BorderStyle = System.Windows.Forms.ButtonBorderStyle.Solid;
            this.m_lblMessageCode.Description = "";
            this.m_lblMessageCode.DisabledColor = System.Drawing.Color.DarkGray;
            this.m_lblMessageCode.Dock = System.Windows.Forms.DockStyle.Fill;
            this.m_lblMessageCode.EdgeRadius = 1;
            this.m_lblMessageCode.ImagePosition = new System.Drawing.Point(0, 0);
            this.m_lblMessageCode.ImageSize = new System.Drawing.Point(0, 0);
            this.m_lblMessageCode.LoadImage = null;
            this.m_lblMessageCode.Location = new System.Drawing.Point(383, 4);
            this.m_lblMessageCode.MainFont = new System.Drawing.Font("맑은 고딕", 9F, System.Drawing.FontStyle.Bold);
            this.m_lblMessageCode.MainFontColor = System.Drawing.Color.Black;
            this.m_lblMessageCode.Margin = new System.Windows.Forms.Padding(2);
            this.m_lblMessageCode.Name = "m_lblMessageCode";
            this.m_lblMessageCode.Size = new System.Drawing.Size(129, 42);
            this.m_lblMessageCode.SubFont = new System.Drawing.Font("맑은 고딕", 10F);
            this.m_lblMessageCode.SubFontColor = System.Drawing.Color.Black;
            this.m_lblMessageCode.SubText = "";
            this.m_lblMessageCode.TabIndex = 1388;
            this.m_lblMessageCode.Text = "MESSAGE CODE";
            this.m_lblMessageCode.TextAlignMain = Sys3Controls.EN_TEXTALIGN.TOP_LEFT;
            this.m_lblMessageCode.TextAlignSub = Sys3Controls.EN_TEXTALIGN.MIDDLE_CENTER;
            this.m_lblMessageCode.TextAlignUnit = Sys3Controls.EN_TEXTALIGN.MIDDLE_CENTER;
            this.m_lblMessageCode.ThemeIndex = 0;
            this.m_lblMessageCode.UnitAreaRate = 40;
            this.m_lblMessageCode.UnitFont = new System.Drawing.Font("맑은 고딕", 10F, System.Drawing.FontStyle.Bold);
            this.m_lblMessageCode.UnitFontColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.m_lblMessageCode.UnitPositionVertical = false;
            this.m_lblMessageCode.UnitText = "";
            this.m_lblMessageCode.UseBorder = true;
            this.m_lblMessageCode.UseEdgeRadius = false;
            this.m_lblMessageCode.UseImage = false;
            this.m_lblMessageCode.UseSubFont = false;
            this.m_lblMessageCode.UseUnitFont = false;
            // 
            // m_lblSolutionCode
            // 
            this.m_lblSolutionCode.BackGroundColor = System.Drawing.Color.FromArgb(((int)(((byte)(200)))), ((int)(((byte)(200)))), ((int)(((byte)(220)))));
            this.m_lblSolutionCode.BorderStroke = 2;
            this.m_lblSolutionCode.BorderStyle = System.Windows.Forms.ButtonBorderStyle.Solid;
            this.m_lblSolutionCode.Description = "";
            this.m_lblSolutionCode.DisabledColor = System.Drawing.Color.DarkGray;
            this.m_lblSolutionCode.Dock = System.Windows.Forms.DockStyle.Fill;
            this.m_lblSolutionCode.EdgeRadius = 1;
            this.m_lblSolutionCode.ImagePosition = new System.Drawing.Point(0, 0);
            this.m_lblSolutionCode.ImageSize = new System.Drawing.Point(0, 0);
            this.m_lblSolutionCode.LoadImage = null;
            this.m_lblSolutionCode.Location = new System.Drawing.Point(26, 96);
            this.m_lblSolutionCode.MainFont = new System.Drawing.Font("맑은 고딕", 9F, System.Drawing.FontStyle.Bold);
            this.m_lblSolutionCode.MainFontColor = System.Drawing.Color.Black;
            this.m_lblSolutionCode.Margin = new System.Windows.Forms.Padding(2);
            this.m_lblSolutionCode.Name = "m_lblSolutionCode";
            this.m_lblSolutionCode.Size = new System.Drawing.Size(129, 42);
            this.m_lblSolutionCode.SubFont = new System.Drawing.Font("맑은 고딕", 10F);
            this.m_lblSolutionCode.SubFontColor = System.Drawing.Color.Black;
            this.m_lblSolutionCode.SubText = "";
            this.m_lblSolutionCode.TabIndex = 1388;
            this.m_lblSolutionCode.Text = "SOLUTION CODE";
            this.m_lblSolutionCode.TextAlignMain = Sys3Controls.EN_TEXTALIGN.TOP_LEFT;
            this.m_lblSolutionCode.TextAlignSub = Sys3Controls.EN_TEXTALIGN.MIDDLE_CENTER;
            this.m_lblSolutionCode.TextAlignUnit = Sys3Controls.EN_TEXTALIGN.MIDDLE_CENTER;
            this.m_lblSolutionCode.ThemeIndex = 0;
            this.m_lblSolutionCode.UnitAreaRate = 40;
            this.m_lblSolutionCode.UnitFont = new System.Drawing.Font("맑은 고딕", 10F, System.Drawing.FontStyle.Bold);
            this.m_lblSolutionCode.UnitFontColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.m_lblSolutionCode.UnitPositionVertical = false;
            this.m_lblSolutionCode.UnitText = "";
            this.m_lblSolutionCode.UseBorder = true;
            this.m_lblSolutionCode.UseEdgeRadius = false;
            this.m_lblSolutionCode.UseImage = false;
            this.m_lblSolutionCode.UseSubFont = false;
            this.m_lblSolutionCode.UseUnitFont = false;
            // 
            // m_labelAlarmCode
            // 
            this.m_labelAlarmCode.BackGroundColor = System.Drawing.Color.White;
            this.m_labelAlarmCode.BorderStroke = 2;
            this.m_labelAlarmCode.BorderStyle = System.Windows.Forms.ButtonBorderStyle.Solid;
            this.m_labelAlarmCode.Description = "";
            this.m_labelAlarmCode.DisabledColor = System.Drawing.Color.LightGray;
            this.m_labelAlarmCode.Dock = System.Windows.Forms.DockStyle.Fill;
            this.m_labelAlarmCode.EdgeRadius = 1;
            this.m_labelAlarmCode.Enabled = false;
            this.m_labelAlarmCode.ImagePosition = new System.Drawing.Point(0, 0);
            this.m_labelAlarmCode.ImageSize = new System.Drawing.Point(0, 0);
            this.m_labelAlarmCode.LoadImage = null;
            this.m_labelAlarmCode.Location = new System.Drawing.Point(159, 4);
            this.m_labelAlarmCode.MainFont = new System.Drawing.Font("맑은 고딕", 11F, System.Drawing.FontStyle.Bold);
            this.m_labelAlarmCode.MainFontColor = System.Drawing.Color.Black;
            this.m_labelAlarmCode.Margin = new System.Windows.Forms.Padding(2);
            this.m_labelAlarmCode.Name = "m_labelAlarmCode";
            this.m_labelAlarmCode.Size = new System.Drawing.Size(196, 42);
            this.m_labelAlarmCode.SubFont = new System.Drawing.Font("맑은 고딕", 10F);
            this.m_labelAlarmCode.SubFontColor = System.Drawing.Color.Black;
            this.m_labelAlarmCode.SubText = "";
            this.m_labelAlarmCode.TabIndex = 0;
            this.m_labelAlarmCode.Text = "--";
            this.m_labelAlarmCode.TextAlignMain = Sys3Controls.EN_TEXTALIGN.BOTTOM_RIGHT;
            this.m_labelAlarmCode.TextAlignSub = Sys3Controls.EN_TEXTALIGN.MIDDLE_CENTER;
            this.m_labelAlarmCode.TextAlignUnit = Sys3Controls.EN_TEXTALIGN.MIDDLE_CENTER;
            this.m_labelAlarmCode.ThemeIndex = 0;
            this.m_labelAlarmCode.UnitAreaRate = 40;
            this.m_labelAlarmCode.UnitFont = new System.Drawing.Font("맑은 고딕", 10F, System.Drawing.FontStyle.Bold);
            this.m_labelAlarmCode.UnitFontColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.m_labelAlarmCode.UnitPositionVertical = false;
            this.m_labelAlarmCode.UnitText = "";
            this.m_labelAlarmCode.UseBorder = true;
            this.m_labelAlarmCode.UseEdgeRadius = false;
            this.m_labelAlarmCode.UseImage = false;
            this.m_labelAlarmCode.UseSubFont = false;
            this.m_labelAlarmCode.UseUnitFont = false;
            this.m_labelAlarmCode.Click += new System.EventHandler(this.Click_Configuration);
            // 
            // m_labelMessageCode
            // 
            this.m_labelMessageCode.BackGroundColor = System.Drawing.Color.White;
            this.m_labelMessageCode.BorderStroke = 2;
            this.m_labelMessageCode.BorderStyle = System.Windows.Forms.ButtonBorderStyle.Solid;
            this._tableLayoutPanel_Configuration.SetColumnSpan(this.m_labelMessageCode, 2);
            this.m_labelMessageCode.Description = "";
            this.m_labelMessageCode.DisabledColor = System.Drawing.Color.LightGray;
            this.m_labelMessageCode.Dock = System.Windows.Forms.DockStyle.Fill;
            this.m_labelMessageCode.EdgeRadius = 1;
            this.m_labelMessageCode.Enabled = false;
            this.m_labelMessageCode.ImagePosition = new System.Drawing.Point(0, 0);
            this.m_labelMessageCode.ImageSize = new System.Drawing.Point(0, 0);
            this.m_labelMessageCode.LoadImage = null;
            this.m_labelMessageCode.Location = new System.Drawing.Point(516, 4);
            this.m_labelMessageCode.MainFont = new System.Drawing.Font("맑은 고딕", 11F, System.Drawing.FontStyle.Bold);
            this.m_labelMessageCode.MainFontColor = System.Drawing.Color.Black;
            this.m_labelMessageCode.Margin = new System.Windows.Forms.Padding(2);
            this.m_labelMessageCode.Name = "m_labelMessageCode";
            this.m_labelMessageCode.Size = new System.Drawing.Size(357, 42);
            this.m_labelMessageCode.SubFont = new System.Drawing.Font("맑은 고딕", 10F);
            this.m_labelMessageCode.SubFontColor = System.Drawing.Color.Black;
            this.m_labelMessageCode.SubText = "";
            this.m_labelMessageCode.TabIndex = 3;
            this.m_labelMessageCode.Text = "--";
            this.m_labelMessageCode.TextAlignMain = Sys3Controls.EN_TEXTALIGN.BOTTOM_RIGHT;
            this.m_labelMessageCode.TextAlignSub = Sys3Controls.EN_TEXTALIGN.MIDDLE_CENTER;
            this.m_labelMessageCode.TextAlignUnit = Sys3Controls.EN_TEXTALIGN.MIDDLE_CENTER;
            this.m_labelMessageCode.ThemeIndex = 0;
            this.m_labelMessageCode.UnitAreaRate = 40;
            this.m_labelMessageCode.UnitFont = new System.Drawing.Font("맑은 고딕", 10F, System.Drawing.FontStyle.Bold);
            this.m_labelMessageCode.UnitFontColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.m_labelMessageCode.UnitPositionVertical = false;
            this.m_labelMessageCode.UnitText = "";
            this.m_labelMessageCode.UseBorder = true;
            this.m_labelMessageCode.UseEdgeRadius = false;
            this.m_labelMessageCode.UseImage = false;
            this.m_labelMessageCode.UseSubFont = false;
            this.m_labelMessageCode.UseUnitFont = false;
            this.m_labelMessageCode.Click += new System.EventHandler(this.Click_Configuration);
            // 
            // m_labelBuzzerIO
            // 
            this.m_labelBuzzerIO.BackGroundColor = System.Drawing.Color.White;
            this.m_labelBuzzerIO.BorderStroke = 2;
            this.m_labelBuzzerIO.BorderStyle = System.Windows.Forms.ButtonBorderStyle.Solid;
            this._tableLayoutPanel_Configuration.SetColumnSpan(this.m_labelBuzzerIO, 2);
            this.m_labelBuzzerIO.Description = "";
            this.m_labelBuzzerIO.DisabledColor = System.Drawing.Color.LightGray;
            this.m_labelBuzzerIO.Dock = System.Windows.Forms.DockStyle.Fill;
            this.m_labelBuzzerIO.EdgeRadius = 1;
            this.m_labelBuzzerIO.Enabled = false;
            this.m_labelBuzzerIO.ImagePosition = new System.Drawing.Point(0, 0);
            this.m_labelBuzzerIO.ImageSize = new System.Drawing.Point(0, 0);
            this.m_labelBuzzerIO.LoadImage = null;
            this.m_labelBuzzerIO.Location = new System.Drawing.Point(516, 50);
            this.m_labelBuzzerIO.MainFont = new System.Drawing.Font("맑은 고딕", 11F, System.Drawing.FontStyle.Bold);
            this.m_labelBuzzerIO.MainFontColor = System.Drawing.Color.Black;
            this.m_labelBuzzerIO.Margin = new System.Windows.Forms.Padding(2);
            this.m_labelBuzzerIO.Name = "m_labelBuzzerIO";
            this.m_labelBuzzerIO.Size = new System.Drawing.Size(357, 42);
            this.m_labelBuzzerIO.SubFont = new System.Drawing.Font("맑은 고딕", 10F, System.Drawing.FontStyle.Bold);
            this.m_labelBuzzerIO.SubFontColor = System.Drawing.Color.Black;
            this.m_labelBuzzerIO.SubText = "[ -1 ]";
            this.m_labelBuzzerIO.TabIndex = 4;
            this.m_labelBuzzerIO.Text = "--";
            this.m_labelBuzzerIO.TextAlignMain = Sys3Controls.EN_TEXTALIGN.BOTTOM_RIGHT;
            this.m_labelBuzzerIO.TextAlignSub = Sys3Controls.EN_TEXTALIGN.TOP_LEFT;
            this.m_labelBuzzerIO.TextAlignUnit = Sys3Controls.EN_TEXTALIGN.MIDDLE_CENTER;
            this.m_labelBuzzerIO.ThemeIndex = 0;
            this.m_labelBuzzerIO.UnitAreaRate = 40;
            this.m_labelBuzzerIO.UnitFont = new System.Drawing.Font("맑은 고딕", 10F, System.Drawing.FontStyle.Bold);
            this.m_labelBuzzerIO.UnitFontColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.m_labelBuzzerIO.UnitPositionVertical = false;
            this.m_labelBuzzerIO.UnitText = "";
            this.m_labelBuzzerIO.UseBorder = true;
            this.m_labelBuzzerIO.UseEdgeRadius = false;
            this.m_labelBuzzerIO.UseImage = false;
            this.m_labelBuzzerIO.UseSubFont = true;
            this.m_labelBuzzerIO.UseUnitFont = false;
            this.m_labelBuzzerIO.Click += new System.EventHandler(this.Click_Configuration);
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
            this.m_btnRemove.GradientAngle = 60F;
            this.m_btnRemove.GradientFirstColor = System.Drawing.Color.White;
            this.m_btnRemove.GradientSecondColor = System.Drawing.Color.FromArgb(((int)(((byte)(170)))), ((int)(((byte)(176)))), ((int)(((byte)(183)))));
            this.m_btnRemove.HoverEmphasizeCustomColor = System.Drawing.Color.White;
            this.m_btnRemove.ImagePosition = new System.Drawing.Point(15, 15);
            this.m_btnRemove.ImageSize = new System.Drawing.Point(30, 30);
            this.m_btnRemove.LoadImage = global::FrameOfSystem3.Properties.Resources.REMOVE;
            this.m_btnRemove.Location = new System.Drawing.Point(1001, 5);
            this.m_btnRemove.MainFont = new System.Drawing.Font("맑은 고딕", 15F, System.Drawing.FontStyle.Bold);
            this.m_btnRemove.MainFontColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(36)))), ((int)(((byte)(0)))));
            this.m_btnRemove.Name = "m_btnRemove";
            this._tableLayoutPanel_Configuration.SetRowSpan(this.m_btnRemove, 3);
            this.m_btnRemove.Size = new System.Drawing.Size(117, 132);
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
            this.m_btnRemove.Click += new System.EventHandler(this.Click_Buttons);
            // 
            // m_groupSelectedItem
            // 
            this.m_groupSelectedItem.BackGroundColor = System.Drawing.Color.WhiteSmoke;
            this.m_groupSelectedItem.Controls.Add(this.tableLayoutPanel1);
            this.m_groupSelectedItem.Dock = System.Windows.Forms.DockStyle.Fill;
            this.m_groupSelectedItem.EdgeBorderStroke = 2;
            this.m_groupSelectedItem.EdgeRadius = 2;
            this.m_groupSelectedItem.LabelFont = new System.Drawing.Font("맑은 고딕", 10F, System.Drawing.FontStyle.Bold);
            this.m_groupSelectedItem.LabelGradientColorFirst = System.Drawing.Color.FromArgb(((int)(((byte)(208)))), ((int)(((byte)(208)))), ((int)(((byte)(208)))));
            this.m_groupSelectedItem.LabelGradientColorSecond = System.Drawing.Color.FromArgb(((int)(((byte)(240)))), ((int)(((byte)(240)))), ((int)(((byte)(240)))));
            this.m_groupSelectedItem.LabelHeight = 30;
            this.m_groupSelectedItem.LabelTextColor = System.Drawing.Color.Black;
            this.m_groupSelectedItem.Location = new System.Drawing.Point(0, 630);
            this.m_groupSelectedItem.Margin = new System.Windows.Forms.Padding(0);
            this.m_groupSelectedItem.Name = "m_groupSelectedItem";
            this.m_groupSelectedItem.Padding = new System.Windows.Forms.Padding(3, 18, 3, 3);
            this.m_groupSelectedItem.Size = new System.Drawing.Size(1156, 90);
            this.m_groupSelectedItem.TabIndex = 1351;
            this.m_groupSelectedItem.TabStop = false;
            this.m_groupSelectedItem.Text = "SELECTED ITEM";
            this.m_groupSelectedItem.TextAlign = Sys3Controls.EN_TEXTALIGN.MIDDLE_CENTER;
            this.m_groupSelectedItem.ThemeIndex = 0;
            this.m_groupSelectedItem.UseLabelBorder = true;
            this.m_groupSelectedItem.UseTitle = true;
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 10;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 2.130452F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 11.62157F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 17.4273F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 2.130452F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 11.62157F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 17.37682F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 14.1357F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 10.71284F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 10.71284F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 2.130452F));
            this.tableLayoutPanel1.Controls.Add(this.m_lblIndex, 1, 1);
            this.tableLayoutPanel1.Controls.Add(this.m_labelIndex, 2, 1);
            this.tableLayoutPanel1.Controls.Add(this._btnExport, 8, 1);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(3, 32);
            this.tableLayoutPanel1.Margin = new System.Windows.Forms.Padding(0);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 3;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 8.333332F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 83.33333F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 8.333332F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(1150, 55);
            this.tableLayoutPanel1.TabIndex = 1;
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
            this.m_lblIndex.Location = new System.Drawing.Point(26, 6);
            this.m_lblIndex.MainFont = new System.Drawing.Font("맑은 고딕", 9F, System.Drawing.FontStyle.Bold);
            this.m_lblIndex.MainFontColor = System.Drawing.Color.Black;
            this.m_lblIndex.Margin = new System.Windows.Forms.Padding(2);
            this.m_lblIndex.Name = "m_lblIndex";
            this.m_lblIndex.Size = new System.Drawing.Size(129, 41);
            this.m_lblIndex.SubFont = new System.Drawing.Font("맑은 고딕", 10F);
            this.m_lblIndex.SubFontColor = System.Drawing.Color.Black;
            this.m_lblIndex.SubText = "";
            this.m_lblIndex.TabIndex = 1388;
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
            // m_labelIndex
            // 
            this.m_labelIndex.BackGroundColor = System.Drawing.Color.White;
            this.m_labelIndex.BorderStroke = 2;
            this.m_labelIndex.BorderStyle = System.Windows.Forms.ButtonBorderStyle.Solid;
            this.m_labelIndex.Description = "";
            this.m_labelIndex.DisabledColor = System.Drawing.Color.LightGray;
            this.m_labelIndex.Dock = System.Windows.Forms.DockStyle.Fill;
            this.m_labelIndex.EdgeRadius = 1;
            this.m_labelIndex.Enabled = false;
            this.m_labelIndex.ImagePosition = new System.Drawing.Point(0, 0);
            this.m_labelIndex.ImageSize = new System.Drawing.Point(0, 0);
            this.m_labelIndex.LoadImage = null;
            this.m_labelIndex.Location = new System.Drawing.Point(159, 6);
            this.m_labelIndex.MainFont = new System.Drawing.Font("맑은 고딕", 11F, System.Drawing.FontStyle.Bold);
            this.m_labelIndex.MainFontColor = System.Drawing.Color.Black;
            this.m_labelIndex.Margin = new System.Windows.Forms.Padding(2);
            this.m_labelIndex.Name = "m_labelIndex";
            this.m_labelIndex.Size = new System.Drawing.Size(196, 41);
            this.m_labelIndex.SubFont = new System.Drawing.Font("맑은 고딕", 10F);
            this.m_labelIndex.SubFontColor = System.Drawing.Color.Black;
            this.m_labelIndex.SubText = "";
            this.m_labelIndex.TabIndex = 1387;
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
            // _btnExport
            // 
            this._btnExport.BorderWidth = 3;
            this._btnExport.ButtonClicked = false;
            this._btnExport.ClickedEmphasizeTextColor = System.Drawing.Color.White;
            this._btnExport.CustomClickedGradientFirstColor = System.Drawing.Color.White;
            this._btnExport.CustomClickedGradientSecondColor = System.Drawing.Color.White;
            this._btnExport.Description = "";
            this._btnExport.DisabledColor = System.Drawing.Color.DarkGray;
            this._btnExport.Dock = System.Windows.Forms.DockStyle.Fill;
            this._btnExport.EdgeRadius = 5;
            this._btnExport.GradientAngle = 60F;
            this._btnExport.GradientFirstColor = System.Drawing.Color.White;
            this._btnExport.GradientSecondColor = System.Drawing.Color.FromArgb(((int)(((byte)(170)))), ((int)(((byte)(176)))), ((int)(((byte)(183)))));
            this._btnExport.HoverEmphasizeCustomColor = System.Drawing.Color.White;
            this._btnExport.ImagePosition = new System.Drawing.Point(15, 15);
            this._btnExport.ImageSize = new System.Drawing.Point(30, 30);
            this._btnExport.LoadImage = null;
            this._btnExport.Location = new System.Drawing.Point(1001, 7);
            this._btnExport.MainFont = new System.Drawing.Font("맑은 고딕", 15F, System.Drawing.FontStyle.Bold);
            this._btnExport.MainFontColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(36)))), ((int)(((byte)(0)))));
            this._btnExport.Name = "_btnExport";
            this._btnExport.Size = new System.Drawing.Size(117, 39);
            this._btnExport.SubFont = new System.Drawing.Font("맑은 고딕", 8F, System.Drawing.FontStyle.Bold);
            this._btnExport.SubFontColor = System.Drawing.Color.DarkBlue;
            this._btnExport.SubText = "STATUS";
            this._btnExport.TabIndex = 0;
            this._btnExport.Text = "EXPORT";
            this._btnExport.TextAlignMain = Sys3Controls.EN_TEXTALIGN.MIDDLE_RIGHT;
            this._btnExport.TextAlignSub = Sys3Controls.EN_TEXTALIGN.TOP_LEFT;
            this._btnExport.ThemeIndex = 0;
            this._btnExport.UseBorder = true;
            this._btnExport.UseClickedEmphasizeTextColor = false;
            this._btnExport.UseCustomizeClickedColor = false;
            this._btnExport.UseEdge = true;
            this._btnExport.UseHoverEmphasizeCustomColor = false;
            this._btnExport.UseImage = true;
            this._btnExport.UserHoverEmpahsize = false;
            this._btnExport.UseSubFont = false;
            this._btnExport.Click += new System.EventHandler(this.Click_Export);
            // 
            // m_groupList
            // 
            this.m_groupList.BackGroundColor = System.Drawing.Color.WhiteSmoke;
            this.m_groupList.Controls.Add(this.m_dgViewAlarm);
            this.m_groupList.Dock = System.Windows.Forms.DockStyle.Fill;
            this.m_groupList.EdgeBorderStroke = 2;
            this.m_groupList.EdgeRadius = 2;
            this.m_groupList.LabelFont = new System.Drawing.Font("맑은 고딕", 10F, System.Drawing.FontStyle.Bold);
            this.m_groupList.LabelGradientColorFirst = System.Drawing.Color.FromArgb(((int)(((byte)(208)))), ((int)(((byte)(208)))), ((int)(((byte)(208)))));
            this.m_groupList.LabelGradientColorSecond = System.Drawing.Color.FromArgb(((int)(((byte)(240)))), ((int)(((byte)(240)))), ((int)(((byte)(240)))));
            this.m_groupList.LabelHeight = 30;
            this.m_groupList.LabelTextColor = System.Drawing.Color.Black;
            this.m_groupList.Location = new System.Drawing.Point(0, 0);
            this.m_groupList.Margin = new System.Windows.Forms.Padding(0);
            this.m_groupList.Name = "m_groupList";
            this.m_groupList.Padding = new System.Windows.Forms.Padding(3, 18, 3, 3);
            this.m_groupList.Size = new System.Drawing.Size(1156, 630);
            this.m_groupList.TabIndex = 1350;
            this.m_groupList.TabStop = false;
            this.m_groupList.Text = "ALARM LIST";
            this.m_groupList.TextAlign = Sys3Controls.EN_TEXTALIGN.MIDDLE_CENTER;
            this.m_groupList.ThemeIndex = 0;
            this.m_groupList.UseLabelBorder = true;
            this.m_groupList.UseTitle = true;
            // 
            // _tableLayoutPanel_Main
            // 
            this._tableLayoutPanel_Main.ColumnCount = 1;
            this._tableLayoutPanel_Main.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this._tableLayoutPanel_Main.Controls.Add(this.m_groupList, 0, 0);
            this._tableLayoutPanel_Main.Controls.Add(this.m_groupSelectedItem, 0, 1);
            this._tableLayoutPanel_Main.Controls.Add(this.m_groupConfiguration, 0, 2);
            this._tableLayoutPanel_Main.Dock = System.Windows.Forms.DockStyle.Fill;
            this._tableLayoutPanel_Main.Location = new System.Drawing.Point(0, 0);
            this._tableLayoutPanel_Main.Margin = new System.Windows.Forms.Padding(0);
            this._tableLayoutPanel_Main.Name = "_tableLayoutPanel_Main";
            this._tableLayoutPanel_Main.RowCount = 3;
            this._tableLayoutPanel_Main.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 70.10989F));
            this._tableLayoutPanel_Main.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 10.10989F));
            this._tableLayoutPanel_Main.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 19.78022F));
            this._tableLayoutPanel_Main.Size = new System.Drawing.Size(1156, 900);
            this._tableLayoutPanel_Main.TabIndex = 1389;
            // 
            // Config_Alarm
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.Controls.Add(this._tableLayoutPanel_Main);
            this.Margin = new System.Windows.Forms.Padding(0);
            this.Name = "Config_Alarm";
            this.Size = new System.Drawing.Size(1156, 900);
            ((System.ComponentModel.ISupportInitialize)(this.m_dgViewAlarm)).EndInit();
            this.m_groupConfiguration.ResumeLayout(false);
            this._tableLayoutPanel_Configuration.ResumeLayout(false);
            this.m_groupSelectedItem.ResumeLayout(false);
            this.tableLayoutPanel1.ResumeLayout(false);
            this.m_groupList.ResumeLayout(false);
            this._tableLayoutPanel_Main.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

		private Sys3Controls.Sys3DoubleBufferedDataGridView m_dgViewAlarm;
		private Sys3Controls.Sys3GroupBoxContainer m_groupList;
        private Sys3Controls.Sys3GroupBoxContainer m_groupSelectedItem;
        private Sys3Controls.Sys3GroupBoxContainer m_groupConfiguration;
		private Sys3Controls.Sys3button m_btnRemove;
		private Sys3Controls.Sys3button m_btnAdd;
		private Sys3Controls.Sys3Label m_lblIndex;
		private Sys3Controls.Sys3Label m_labelIndex;
		private Sys3Controls.Sys3Label m_lblAlarmCode;
		private Sys3Controls.Sys3Label m_lblAlarmGrade;
		private Sys3Controls.Sys3Label m_lblSolutionCode;
		private Sys3Controls.Sys3Label m_lblMessageCode;
		private Sys3Controls.Sys3Label m_lblBuzzerIO;
		private Sys3Controls.Sys3Label m_labelAlarmCode;
		private Sys3Controls.Sys3Label m_labelSolutionCode;
		private Sys3Controls.Sys3Label m_labelAlarmGrade;
		private Sys3Controls.Sys3Label m_labelBuzzerIO;
		private Sys3Controls.Sys3Label m_labelMessageCode;
		private System.Windows.Forms.DataGridViewTextBoxColumn INDEX;
		private System.Windows.Forms.DataGridViewTextBoxColumn ID;
		private System.Windows.Forms.DataGridViewTextBoxColumn GRADE;
		private System.Windows.Forms.DataGridViewTextBoxColumn PASSWORD;
		private System.Windows.Forms.DataGridViewTextBoxColumn SOLUTIONCODE;
		private System.Windows.Forms.DataGridViewTextBoxColumn MONITORING;
		private Sys3Controls.Sys3button _btnExport;
		private Sys3Controls.Sys3button _btnDiffuse;
        private System.Windows.Forms.TableLayoutPanel _tableLayoutPanel_Main;
        private System.Windows.Forms.TableLayoutPanel _tableLayoutPanel_Configuration;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
    }
}
