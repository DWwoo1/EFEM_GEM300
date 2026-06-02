namespace FrameOfSystem3.Views.Config
{
    partial class Config_Interrupt
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
            this.m_ledInput = new Sys3Controls.Sys3LedLabel();
            this.m_dgViewInterrupt = new Sys3Controls.Sys3DoubleBufferedDataGridView();
            this.ID = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ENABLE = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.PASSWORD = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.AUTHORITY = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.TARGET_ACT = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ACTION = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn7 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.DO_INDEX = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.CONDITION = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.STATE = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.m_groupTitle = new Sys3Controls.Sys3GroupBoxContainer();
            this.m_groupSelectedItem = new Sys3Controls.Sys3GroupBoxContainer();
            this._tableLayoutPanel_InterruptKey = new System.Windows.Forms.TableLayoutPanel();
            this.m_lblKey = new Sys3Controls.Sys3Label();
            this.m_labelKey = new Sys3Controls.Sys3Label();
            this.m_groupConfiguration = new Sys3Controls.Sys3GroupBoxContainer();
            this.m_lblName = new Sys3Controls.Sys3Label();
            this.m_lblInput = new Sys3Controls.Sys3Label();
            this.m_lblCondition = new Sys3Controls.Sys3Label();
            this.m_lblAlarmCode = new Sys3Controls.Sys3Label();
            this.m_lblAction = new Sys3Controls.Sys3Label();
            this.m_labelName = new Sys3Controls.Sys3Label();
            this.m_labelInput = new Sys3Controls.Sys3Label();
            this.m_labelCondition = new Sys3Controls.Sys3Label();
            this.m_labelAlarmCode = new Sys3Controls.Sys3Label();
            this.m_labelAction = new Sys3Controls.Sys3Label();
            this.m_btnDisable = new Sys3Controls.Sys3button();
            this.m_btnEnable = new Sys3Controls.Sys3button();
            this.m_btnRemove = new Sys3Controls.Sys3button();
            this.m_btnAdd = new Sys3Controls.Sys3button();
            this.sys3Label1 = new Sys3Controls.Sys3Label();
            this.m_labelDO = new Sys3Controls.Sys3Label();
            this._tableLayoutPanel_Main = new System.Windows.Forms.TableLayoutPanel();
            this._tableLayoutPanel_InterruptConfiguration = new System.Windows.Forms.TableLayoutPanel();
            this._tableLayoutPanel_Buttons = new System.Windows.Forms.TableLayoutPanel();
            ((System.ComponentModel.ISupportInitialize)(this.m_dgViewInterrupt)).BeginInit();
            this.m_groupTitle.SuspendLayout();
            this.m_groupSelectedItem.SuspendLayout();
            this._tableLayoutPanel_InterruptKey.SuspendLayout();
            this.m_groupConfiguration.SuspendLayout();
            this._tableLayoutPanel_Main.SuspendLayout();
            this._tableLayoutPanel_InterruptConfiguration.SuspendLayout();
            this._tableLayoutPanel_Buttons.SuspendLayout();
            this.SuspendLayout();
            // 
            // m_ledInput
            // 
            this.m_ledInput.Active = false;
            this.m_ledInput.ColorActive = System.Drawing.Color.FromArgb(((int)(((byte)(75)))), ((int)(((byte)(216)))), ((int)(((byte)(101)))));
            this.m_ledInput.ColorNormal = System.Drawing.Color.DimGray;
            this.m_ledInput.Dock = System.Windows.Forms.DockStyle.Fill;
            this.m_ledInput.Location = new System.Drawing.Point(184, 104);
            this.m_ledInput.Margin = new System.Windows.Forms.Padding(3, 3, 0, 3);
            this.m_ledInput.Name = "m_ledInput";
            this.m_ledInput.Size = new System.Drawing.Size(17, 42);
            this.m_ledInput.TabIndex = 1170;
            // 
            // m_dgViewInterrupt
            // 
            this.m_dgViewInterrupt.AllowUserToAddRows = false;
            this.m_dgViewInterrupt.AllowUserToDeleteRows = false;
            this.m_dgViewInterrupt.AllowUserToResizeColumns = false;
            this.m_dgViewInterrupt.AllowUserToResizeRows = false;
            this.m_dgViewInterrupt.BackgroundColor = System.Drawing.Color.White;
            this.m_dgViewInterrupt.CellBorderStyle = System.Windows.Forms.DataGridViewCellBorderStyle.Raised;
            this.m_dgViewInterrupt.ColumnHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.Single;
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle1.BackColor = System.Drawing.Color.LightGray;
            dataGridViewCellStyle1.Font = new System.Drawing.Font("맑은 고딕", 11F, System.Drawing.FontStyle.Bold);
            dataGridViewCellStyle1.ForeColor = System.Drawing.Color.Black;
            dataGridViewCellStyle1.SelectionBackColor = System.Drawing.Color.White;
            dataGridViewCellStyle1.SelectionForeColor = System.Drawing.Color.Black;
            dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.m_dgViewInterrupt.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
            this.m_dgViewInterrupt.ColumnHeadersHeight = 30;
            this.m_dgViewInterrupt.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            this.m_dgViewInterrupt.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.ID,
            this.ENABLE,
            this.PASSWORD,
            this.AUTHORITY,
            this.TARGET_ACT,
            this.ACTION,
            this.dataGridViewTextBoxColumn7,
            this.DO_INDEX,
            this.CONDITION,
            this.STATE});
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle2.BackColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle2.Font = new System.Drawing.Font("맑은 고딕", 10F, System.Drawing.FontStyle.Bold);
            dataGridViewCellStyle2.ForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle2.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(120)))), ((int)(((byte)(130)))), ((int)(((byte)(150)))));
            dataGridViewCellStyle2.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.m_dgViewInterrupt.DefaultCellStyle = dataGridViewCellStyle2;
            this.m_dgViewInterrupt.Dock = System.Windows.Forms.DockStyle.Fill;
            this.m_dgViewInterrupt.EnableHeadersVisualStyles = false;
            this.m_dgViewInterrupt.GridColor = System.Drawing.Color.FromArgb(((int)(((byte)(120)))), ((int)(((byte)(130)))), ((int)(((byte)(150)))));
            this.m_dgViewInterrupt.Location = new System.Drawing.Point(3, 32);
            this.m_dgViewInterrupt.MultiSelect = false;
            this.m_dgViewInterrupt.Name = "m_dgViewInterrupt";
            this.m_dgViewInterrupt.ReadOnly = true;
            this.m_dgViewInterrupt.RowHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.None;
            dataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle3.BackColor = System.Drawing.Color.White;
            dataGridViewCellStyle3.Font = new System.Drawing.Font("맑은 고딕", 11F);
            dataGridViewCellStyle3.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle3.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(120)))), ((int)(((byte)(130)))), ((int)(((byte)(150)))));
            dataGridViewCellStyle3.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle3.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.m_dgViewInterrupt.RowHeadersDefaultCellStyle = dataGridViewCellStyle3;
            this.m_dgViewInterrupt.RowHeadersVisible = false;
            this.m_dgViewInterrupt.RowHeadersWidthSizeMode = System.Windows.Forms.DataGridViewRowHeadersWidthSizeMode.DisableResizing;
            this.m_dgViewInterrupt.RowTemplate.Height = 23;
            this.m_dgViewInterrupt.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.m_dgViewInterrupt.Size = new System.Drawing.Size(1150, 481);
            this.m_dgViewInterrupt.TabIndex = 1162;
            this.m_dgViewInterrupt.CellClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.Click_Dgview);
            // 
            // ID
            // 
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
            this.ENABLE.HeaderText = "ENABLE";
            this.ENABLE.Name = "ENABLE";
            this.ENABLE.ReadOnly = true;
            this.ENABLE.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.ENABLE.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.ENABLE.Width = 63;
            // 
            // PASSWORD
            // 
            this.PASSWORD.HeaderText = "NAME";
            this.PASSWORD.MaxInputLength = 20;
            this.PASSWORD.Name = "PASSWORD";
            this.PASSWORD.ReadOnly = true;
            this.PASSWORD.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.PASSWORD.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.PASSWORD.Width = 325;
            // 
            // AUTHORITY
            // 
            this.AUTHORITY.HeaderText = "TARGET";
            this.AUTHORITY.MaxInputLength = 20;
            this.AUTHORITY.Name = "AUTHORITY";
            this.AUTHORITY.ReadOnly = true;
            this.AUTHORITY.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.AUTHORITY.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.AUTHORITY.Width = 65;
            // 
            // TARGET_ACT
            // 
            this.TARGET_ACT.HeaderText = " ";
            this.TARGET_ACT.Name = "TARGET_ACT";
            this.TARGET_ACT.ReadOnly = true;
            this.TARGET_ACT.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.TARGET_ACT.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.TARGET_ACT.Width = 30;
            // 
            // ACTION
            // 
            this.ACTION.HeaderText = "ACTION";
            this.ACTION.Name = "ACTION";
            this.ACTION.ReadOnly = true;
            this.ACTION.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.ACTION.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.ACTION.Width = 175;
            // 
            // dataGridViewTextBoxColumn7
            // 
            this.dataGridViewTextBoxColumn7.HeaderText = "CODE";
            this.dataGridViewTextBoxColumn7.Name = "dataGridViewTextBoxColumn7";
            this.dataGridViewTextBoxColumn7.ReadOnly = true;
            this.dataGridViewTextBoxColumn7.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.dataGridViewTextBoxColumn7.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.dataGridViewTextBoxColumn7.Width = 70;
            // 
            // DO_INDEX
            // 
            this.DO_INDEX.HeaderText = "OUTPUT";
            this.DO_INDEX.Name = "DO_INDEX";
            this.DO_INDEX.ReadOnly = true;
            this.DO_INDEX.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.DO_INDEX.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.DO_INDEX.Width = 80;
            // 
            // CONDITION
            // 
            this.CONDITION.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.CONDITION.HeaderText = "CONDITION";
            this.CONDITION.Name = "CONDITION";
            this.CONDITION.ReadOnly = true;
            this.CONDITION.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.CONDITION.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // STATE
            // 
            this.STATE.HeaderText = "";
            this.STATE.Name = "STATE";
            this.STATE.ReadOnly = true;
            this.STATE.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.STATE.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.STATE.Width = 30;
            // 
            // m_groupTitle
            // 
            this.m_groupTitle.BackGroundColor = System.Drawing.Color.WhiteSmoke;
            this.m_groupTitle.Controls.Add(this.m_dgViewInterrupt);
            this.m_groupTitle.Dock = System.Windows.Forms.DockStyle.Fill;
            this.m_groupTitle.EdgeBorderStroke = 2;
            this.m_groupTitle.EdgeRadius = 2;
            this.m_groupTitle.LabelFont = new System.Drawing.Font("맑은 고딕", 10F, System.Drawing.FontStyle.Bold);
            this.m_groupTitle.LabelGradientColorFirst = System.Drawing.Color.FromArgb(((int)(((byte)(208)))), ((int)(((byte)(208)))), ((int)(((byte)(208)))));
            this.m_groupTitle.LabelGradientColorSecond = System.Drawing.Color.FromArgb(((int)(((byte)(240)))), ((int)(((byte)(240)))), ((int)(((byte)(240)))));
            this.m_groupTitle.LabelHeight = 32;
            this.m_groupTitle.LabelTextColor = System.Drawing.Color.Black;
            this.m_groupTitle.Location = new System.Drawing.Point(0, 0);
            this.m_groupTitle.Margin = new System.Windows.Forms.Padding(0);
            this.m_groupTitle.Name = "m_groupTitle";
            this.m_groupTitle.Padding = new System.Windows.Forms.Padding(3, 18, 3, 3);
            this.m_groupTitle.Size = new System.Drawing.Size(1156, 516);
            this.m_groupTitle.TabIndex = 1350;
            this.m_groupTitle.TabStop = false;
            this.m_groupTitle.Text = "INTERRUPT LIST";
            this.m_groupTitle.TextAlign = Sys3Controls.EN_TEXTALIGN.MIDDLE_CENTER;
            this.m_groupTitle.ThemeIndex = 0;
            this.m_groupTitle.UseLabelBorder = true;
            this.m_groupTitle.UseTitle = true;
            // 
            // m_groupSelectedItem
            // 
            this.m_groupSelectedItem.BackGroundColor = System.Drawing.Color.WhiteSmoke;
            this.m_groupSelectedItem.Controls.Add(this._tableLayoutPanel_InterruptKey);
            this.m_groupSelectedItem.Dock = System.Windows.Forms.DockStyle.Fill;
            this.m_groupSelectedItem.EdgeBorderStroke = 2;
            this.m_groupSelectedItem.EdgeRadius = 2;
            this.m_groupSelectedItem.LabelFont = new System.Drawing.Font("맑은 고딕", 10F, System.Drawing.FontStyle.Bold);
            this.m_groupSelectedItem.LabelGradientColorFirst = System.Drawing.Color.FromArgb(((int)(((byte)(208)))), ((int)(((byte)(208)))), ((int)(((byte)(208)))));
            this.m_groupSelectedItem.LabelGradientColorSecond = System.Drawing.Color.FromArgb(((int)(((byte)(240)))), ((int)(((byte)(240)))), ((int)(((byte)(240)))));
            this.m_groupSelectedItem.LabelHeight = 32;
            this.m_groupSelectedItem.LabelTextColor = System.Drawing.Color.Black;
            this.m_groupSelectedItem.Location = new System.Drawing.Point(0, 516);
            this.m_groupSelectedItem.Margin = new System.Windows.Forms.Padding(0);
            this.m_groupSelectedItem.Name = "m_groupSelectedItem";
            this.m_groupSelectedItem.Padding = new System.Windows.Forms.Padding(3, 18, 3, 3);
            this.m_groupSelectedItem.Size = new System.Drawing.Size(1156, 102);
            this.m_groupSelectedItem.TabIndex = 1351;
            this.m_groupSelectedItem.TabStop = false;
            this.m_groupSelectedItem.Text = "ITEM";
            this.m_groupSelectedItem.TextAlign = Sys3Controls.EN_TEXTALIGN.MIDDLE_CENTER;
            this.m_groupSelectedItem.ThemeIndex = 0;
            this.m_groupSelectedItem.UseLabelBorder = true;
            this.m_groupSelectedItem.UseTitle = true;
            // 
            // _tableLayoutPanel_InterruptKey
            // 
            this._tableLayoutPanel_InterruptKey.ColumnCount = 7;
            this._tableLayoutPanel_InterruptKey.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 1.890941F));
            this._tableLayoutPanel_InterruptKey.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 14.18206F));
            this._tableLayoutPanel_InterruptKey.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 32.98153F));
            this._tableLayoutPanel_InterruptKey.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 1.890941F));
            this._tableLayoutPanel_InterruptKey.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 14.18206F));
            this._tableLayoutPanel_InterruptKey.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 32.98153F));
            this._tableLayoutPanel_InterruptKey.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 1.890941F));
            this._tableLayoutPanel_InterruptKey.Controls.Add(this.m_lblKey, 1, 1);
            this._tableLayoutPanel_InterruptKey.Controls.Add(this.m_labelKey, 2, 1);
            this._tableLayoutPanel_InterruptKey.Dock = System.Windows.Forms.DockStyle.Fill;
            this._tableLayoutPanel_InterruptKey.Location = new System.Drawing.Point(3, 32);
            this._tableLayoutPanel_InterruptKey.Margin = new System.Windows.Forms.Padding(0);
            this._tableLayoutPanel_InterruptKey.Name = "_tableLayoutPanel_InterruptKey";
            this._tableLayoutPanel_InterruptKey.RowCount = 3;
            this._tableLayoutPanel_InterruptKey.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 8.333332F));
            this._tableLayoutPanel_InterruptKey.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 83.33333F));
            this._tableLayoutPanel_InterruptKey.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 8.333332F));
            this._tableLayoutPanel_InterruptKey.Size = new System.Drawing.Size(1150, 67);
            this._tableLayoutPanel_InterruptKey.TabIndex = 0;
            // 
            // m_lblKey
            // 
            this.m_lblKey.BackGroundColor = System.Drawing.Color.FromArgb(((int)(((byte)(200)))), ((int)(((byte)(200)))), ((int)(((byte)(220)))));
            this.m_lblKey.BorderStroke = 2;
            this.m_lblKey.BorderStyle = System.Windows.Forms.ButtonBorderStyle.Solid;
            this.m_lblKey.Description = "";
            this.m_lblKey.DisabledColor = System.Drawing.Color.DarkGray;
            this.m_lblKey.Dock = System.Windows.Forms.DockStyle.Fill;
            this.m_lblKey.EdgeRadius = 1;
            this.m_lblKey.ImagePosition = new System.Drawing.Point(0, 0);
            this.m_lblKey.ImageSize = new System.Drawing.Point(0, 0);
            this.m_lblKey.LoadImage = null;
            this.m_lblKey.Location = new System.Drawing.Point(23, 7);
            this.m_lblKey.MainFont = new System.Drawing.Font("맑은 고딕", 9F, System.Drawing.FontStyle.Bold);
            this.m_lblKey.MainFontColor = System.Drawing.Color.Black;
            this.m_lblKey.Margin = new System.Windows.Forms.Padding(2);
            this.m_lblKey.Name = "m_lblKey";
            this.m_lblKey.Size = new System.Drawing.Size(159, 51);
            this.m_lblKey.SubFont = new System.Drawing.Font("맑은 고딕", 10F);
            this.m_lblKey.SubFontColor = System.Drawing.Color.Black;
            this.m_lblKey.SubText = "";
            this.m_lblKey.TabIndex = 1362;
            this.m_lblKey.Text = "KEY";
            this.m_lblKey.TextAlignMain = Sys3Controls.EN_TEXTALIGN.TOP_LEFT;
            this.m_lblKey.TextAlignSub = Sys3Controls.EN_TEXTALIGN.MIDDLE_CENTER;
            this.m_lblKey.TextAlignUnit = Sys3Controls.EN_TEXTALIGN.MIDDLE_CENTER;
            this.m_lblKey.ThemeIndex = 0;
            this.m_lblKey.UnitAreaRate = 40;
            this.m_lblKey.UnitFont = new System.Drawing.Font("맑은 고딕", 10F, System.Drawing.FontStyle.Bold);
            this.m_lblKey.UnitFontColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.m_lblKey.UnitPositionVertical = false;
            this.m_lblKey.UnitText = "";
            this.m_lblKey.UseBorder = true;
            this.m_lblKey.UseEdgeRadius = false;
            this.m_lblKey.UseImage = false;
            this.m_lblKey.UseSubFont = false;
            this.m_lblKey.UseUnitFont = false;
            // 
            // m_labelKey
            // 
            this.m_labelKey.BackGroundColor = System.Drawing.Color.White;
            this.m_labelKey.BorderStroke = 2;
            this.m_labelKey.BorderStyle = System.Windows.Forms.ButtonBorderStyle.Solid;
            this.m_labelKey.Description = "";
            this.m_labelKey.DisabledColor = System.Drawing.Color.LightGray;
            this.m_labelKey.Dock = System.Windows.Forms.DockStyle.Fill;
            this.m_labelKey.EdgeRadius = 1;
            this.m_labelKey.Enabled = false;
            this.m_labelKey.ImagePosition = new System.Drawing.Point(0, 0);
            this.m_labelKey.ImageSize = new System.Drawing.Point(0, 0);
            this.m_labelKey.LoadImage = null;
            this.m_labelKey.Location = new System.Drawing.Point(186, 7);
            this.m_labelKey.MainFont = new System.Drawing.Font("맑은 고딕", 11F, System.Drawing.FontStyle.Bold);
            this.m_labelKey.MainFontColor = System.Drawing.Color.Black;
            this.m_labelKey.Margin = new System.Windows.Forms.Padding(2);
            this.m_labelKey.Name = "m_labelKey";
            this.m_labelKey.Size = new System.Drawing.Size(375, 51);
            this.m_labelKey.SubFont = new System.Drawing.Font("맑은 고딕", 10F);
            this.m_labelKey.SubFontColor = System.Drawing.Color.Black;
            this.m_labelKey.SubText = "";
            this.m_labelKey.TabIndex = 1368;
            this.m_labelKey.TextAlignMain = Sys3Controls.EN_TEXTALIGN.MIDDLE_CENTER;
            this.m_labelKey.TextAlignSub = Sys3Controls.EN_TEXTALIGN.MIDDLE_CENTER;
            this.m_labelKey.TextAlignUnit = Sys3Controls.EN_TEXTALIGN.MIDDLE_CENTER;
            this.m_labelKey.ThemeIndex = 0;
            this.m_labelKey.UnitAreaRate = 40;
            this.m_labelKey.UnitFont = new System.Drawing.Font("맑은 고딕", 10F, System.Drawing.FontStyle.Bold);
            this.m_labelKey.UnitFontColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.m_labelKey.UnitPositionVertical = false;
            this.m_labelKey.UnitText = "";
            this.m_labelKey.UseBorder = true;
            this.m_labelKey.UseEdgeRadius = false;
            this.m_labelKey.UseImage = false;
            this.m_labelKey.UseSubFont = false;
            this.m_labelKey.UseUnitFont = false;
            // 
            // m_groupConfiguration
            // 
            this.m_groupConfiguration.BackGroundColor = System.Drawing.Color.WhiteSmoke;
            this.m_groupConfiguration.Controls.Add(this._tableLayoutPanel_InterruptConfiguration);
            this.m_groupConfiguration.Dock = System.Windows.Forms.DockStyle.Fill;
            this.m_groupConfiguration.EdgeBorderStroke = 2;
            this.m_groupConfiguration.EdgeRadius = 2;
            this.m_groupConfiguration.LabelFont = new System.Drawing.Font("맑은 고딕", 10F, System.Drawing.FontStyle.Bold);
            this.m_groupConfiguration.LabelGradientColorFirst = System.Drawing.Color.FromArgb(((int)(((byte)(208)))), ((int)(((byte)(208)))), ((int)(((byte)(208)))));
            this.m_groupConfiguration.LabelGradientColorSecond = System.Drawing.Color.FromArgb(((int)(((byte)(240)))), ((int)(((byte)(240)))), ((int)(((byte)(240)))));
            this.m_groupConfiguration.LabelHeight = 32;
            this.m_groupConfiguration.LabelTextColor = System.Drawing.Color.Black;
            this.m_groupConfiguration.Location = new System.Drawing.Point(0, 618);
            this.m_groupConfiguration.Margin = new System.Windows.Forms.Padding(0);
            this.m_groupConfiguration.Name = "m_groupConfiguration";
            this.m_groupConfiguration.Padding = new System.Windows.Forms.Padding(3, 18, 3, 3);
            this.m_groupConfiguration.Size = new System.Drawing.Size(1156, 282);
            this.m_groupConfiguration.TabIndex = 1352;
            this.m_groupConfiguration.TabStop = false;
            this.m_groupConfiguration.Text = "CONFIGURATION";
            this.m_groupConfiguration.TextAlign = Sys3Controls.EN_TEXTALIGN.MIDDLE_CENTER;
            this.m_groupConfiguration.ThemeIndex = 0;
            this.m_groupConfiguration.UseLabelBorder = true;
            this.m_groupConfiguration.UseTitle = true;
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
            this.m_lblName.Location = new System.Drawing.Point(23, 7);
            this.m_lblName.MainFont = new System.Drawing.Font("맑은 고딕", 9F, System.Drawing.FontStyle.Bold);
            this.m_lblName.MainFontColor = System.Drawing.Color.Black;
            this.m_lblName.Margin = new System.Windows.Forms.Padding(2);
            this.m_lblName.Name = "m_lblName";
            this.m_lblName.Size = new System.Drawing.Size(156, 44);
            this.m_lblName.SubFont = new System.Drawing.Font("맑은 고딕", 10F);
            this.m_lblName.SubFontColor = System.Drawing.Color.Black;
            this.m_lblName.SubText = "";
            this.m_lblName.TabIndex = 1363;
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
            // m_lblInput
            // 
            this.m_lblInput.BackGroundColor = System.Drawing.Color.FromArgb(((int)(((byte)(200)))), ((int)(((byte)(200)))), ((int)(((byte)(220)))));
            this.m_lblInput.BorderStroke = 2;
            this.m_lblInput.BorderStyle = System.Windows.Forms.ButtonBorderStyle.Solid;
            this.m_lblInput.Description = "";
            this.m_lblInput.DisabledColor = System.Drawing.Color.DarkGray;
            this.m_lblInput.Dock = System.Windows.Forms.DockStyle.Fill;
            this.m_lblInput.EdgeRadius = 1;
            this.m_lblInput.ImagePosition = new System.Drawing.Point(0, 0);
            this.m_lblInput.ImageSize = new System.Drawing.Point(0, 0);
            this.m_lblInput.LoadImage = null;
            this.m_lblInput.Location = new System.Drawing.Point(23, 55);
            this.m_lblInput.MainFont = new System.Drawing.Font("맑은 고딕", 9F, System.Drawing.FontStyle.Bold);
            this.m_lblInput.MainFontColor = System.Drawing.Color.Black;
            this.m_lblInput.Margin = new System.Windows.Forms.Padding(2);
            this.m_lblInput.Name = "m_lblInput";
            this.m_lblInput.Size = new System.Drawing.Size(156, 44);
            this.m_lblInput.SubFont = new System.Drawing.Font("맑은 고딕", 10F);
            this.m_lblInput.SubFontColor = System.Drawing.Color.Black;
            this.m_lblInput.SubText = "";
            this.m_lblInput.TabIndex = 1364;
            this.m_lblInput.Text = "INPUT TARGET";
            this.m_lblInput.TextAlignMain = Sys3Controls.EN_TEXTALIGN.TOP_LEFT;
            this.m_lblInput.TextAlignSub = Sys3Controls.EN_TEXTALIGN.MIDDLE_CENTER;
            this.m_lblInput.TextAlignUnit = Sys3Controls.EN_TEXTALIGN.MIDDLE_CENTER;
            this.m_lblInput.ThemeIndex = 0;
            this.m_lblInput.UnitAreaRate = 40;
            this.m_lblInput.UnitFont = new System.Drawing.Font("맑은 고딕", 10F, System.Drawing.FontStyle.Bold);
            this.m_lblInput.UnitFontColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.m_lblInput.UnitPositionVertical = false;
            this.m_lblInput.UnitText = "";
            this.m_lblInput.UseBorder = true;
            this.m_lblInput.UseEdgeRadius = false;
            this.m_lblInput.UseImage = false;
            this.m_lblInput.UseSubFont = false;
            this.m_lblInput.UseUnitFont = false;
            // 
            // m_lblCondition
            // 
            this.m_lblCondition.BackGroundColor = System.Drawing.Color.FromArgb(((int)(((byte)(200)))), ((int)(((byte)(200)))), ((int)(((byte)(220)))));
            this.m_lblCondition.BorderStroke = 2;
            this.m_lblCondition.BorderStyle = System.Windows.Forms.ButtonBorderStyle.Solid;
            this.m_lblCondition.Description = "";
            this.m_lblCondition.DisabledColor = System.Drawing.Color.DarkGray;
            this.m_lblCondition.Dock = System.Windows.Forms.DockStyle.Fill;
            this.m_lblCondition.EdgeRadius = 1;
            this.m_lblCondition.ImagePosition = new System.Drawing.Point(0, 0);
            this.m_lblCondition.ImageSize = new System.Drawing.Point(0, 0);
            this.m_lblCondition.LoadImage = null;
            this.m_lblCondition.Location = new System.Drawing.Point(23, 103);
            this.m_lblCondition.MainFont = new System.Drawing.Font("맑은 고딕", 9F, System.Drawing.FontStyle.Bold);
            this.m_lblCondition.MainFontColor = System.Drawing.Color.Black;
            this.m_lblCondition.Margin = new System.Windows.Forms.Padding(2);
            this.m_lblCondition.Name = "m_lblCondition";
            this.m_lblCondition.Size = new System.Drawing.Size(156, 44);
            this.m_lblCondition.SubFont = new System.Drawing.Font("맑은 고딕", 9F, System.Drawing.FontStyle.Bold);
            this.m_lblCondition.SubFontColor = System.Drawing.Color.DarkBlue;
            this.m_lblCondition.SubText = "EQUIPMENT STATE";
            this.m_lblCondition.TabIndex = 1365;
            this.m_lblCondition.Text = "CONDITION";
            this.m_lblCondition.TextAlignMain = Sys3Controls.EN_TEXTALIGN.TOP_LEFT;
            this.m_lblCondition.TextAlignSub = Sys3Controls.EN_TEXTALIGN.BOTTOM_RIGHT;
            this.m_lblCondition.TextAlignUnit = Sys3Controls.EN_TEXTALIGN.MIDDLE_CENTER;
            this.m_lblCondition.ThemeIndex = 0;
            this.m_lblCondition.UnitAreaRate = 40;
            this.m_lblCondition.UnitFont = new System.Drawing.Font("맑은 고딕", 10F, System.Drawing.FontStyle.Bold);
            this.m_lblCondition.UnitFontColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.m_lblCondition.UnitPositionVertical = false;
            this.m_lblCondition.UnitText = "";
            this.m_lblCondition.UseBorder = true;
            this.m_lblCondition.UseEdgeRadius = false;
            this.m_lblCondition.UseImage = false;
            this.m_lblCondition.UseSubFont = true;
            this.m_lblCondition.UseUnitFont = false;
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
            this.m_lblAlarmCode.Location = new System.Drawing.Point(596, 7);
            this.m_lblAlarmCode.MainFont = new System.Drawing.Font("맑은 고딕", 9F, System.Drawing.FontStyle.Bold);
            this.m_lblAlarmCode.MainFontColor = System.Drawing.Color.Black;
            this.m_lblAlarmCode.Margin = new System.Windows.Forms.Padding(2);
            this.m_lblAlarmCode.Name = "m_lblAlarmCode";
            this.m_lblAlarmCode.Size = new System.Drawing.Size(156, 44);
            this.m_lblAlarmCode.SubFont = new System.Drawing.Font("맑은 고딕", 10F);
            this.m_lblAlarmCode.SubFontColor = System.Drawing.Color.Black;
            this.m_lblAlarmCode.SubText = "";
            this.m_lblAlarmCode.TabIndex = 1366;
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
            // m_lblAction
            // 
            this.m_lblAction.BackGroundColor = System.Drawing.Color.FromArgb(((int)(((byte)(200)))), ((int)(((byte)(200)))), ((int)(((byte)(220)))));
            this.m_lblAction.BorderStroke = 2;
            this.m_lblAction.BorderStyle = System.Windows.Forms.ButtonBorderStyle.Solid;
            this.m_lblAction.Description = "";
            this.m_lblAction.DisabledColor = System.Drawing.Color.DarkGray;
            this.m_lblAction.Dock = System.Windows.Forms.DockStyle.Fill;
            this.m_lblAction.EdgeRadius = 1;
            this.m_lblAction.ImagePosition = new System.Drawing.Point(0, 0);
            this.m_lblAction.ImageSize = new System.Drawing.Point(0, 0);
            this.m_lblAction.LoadImage = null;
            this.m_lblAction.Location = new System.Drawing.Point(596, 55);
            this.m_lblAction.MainFont = new System.Drawing.Font("맑은 고딕", 9F, System.Drawing.FontStyle.Bold);
            this.m_lblAction.MainFontColor = System.Drawing.Color.Black;
            this.m_lblAction.Margin = new System.Windows.Forms.Padding(2);
            this.m_lblAction.Name = "m_lblAction";
            this.m_lblAction.Size = new System.Drawing.Size(156, 44);
            this.m_lblAction.SubFont = new System.Drawing.Font("맑은 고딕", 10F);
            this.m_lblAction.SubFontColor = System.Drawing.Color.Black;
            this.m_lblAction.SubText = "";
            this.m_lblAction.TabIndex = 1367;
            this.m_lblAction.Text = "ACTION";
            this.m_lblAction.TextAlignMain = Sys3Controls.EN_TEXTALIGN.TOP_LEFT;
            this.m_lblAction.TextAlignSub = Sys3Controls.EN_TEXTALIGN.MIDDLE_CENTER;
            this.m_lblAction.TextAlignUnit = Sys3Controls.EN_TEXTALIGN.MIDDLE_CENTER;
            this.m_lblAction.ThemeIndex = 0;
            this.m_lblAction.UnitAreaRate = 40;
            this.m_lblAction.UnitFont = new System.Drawing.Font("맑은 고딕", 10F, System.Drawing.FontStyle.Bold);
            this.m_lblAction.UnitFontColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.m_lblAction.UnitPositionVertical = false;
            this.m_lblAction.UnitText = "";
            this.m_lblAction.UseBorder = true;
            this.m_lblAction.UseEdgeRadius = false;
            this.m_lblAction.UseImage = false;
            this.m_lblAction.UseSubFont = false;
            this.m_lblAction.UseUnitFont = false;
            // 
            // m_labelName
            // 
            this.m_labelName.BackGroundColor = System.Drawing.Color.White;
            this.m_labelName.BorderStroke = 2;
            this.m_labelName.BorderStyle = System.Windows.Forms.ButtonBorderStyle.Solid;
            this._tableLayoutPanel_InterruptConfiguration.SetColumnSpan(this.m_labelName, 2);
            this.m_labelName.Description = "";
            this.m_labelName.DisabledColor = System.Drawing.Color.LightGray;
            this.m_labelName.Dock = System.Windows.Forms.DockStyle.Fill;
            this.m_labelName.EdgeRadius = 1;
            this.m_labelName.Enabled = false;
            this.m_labelName.ImagePosition = new System.Drawing.Point(0, 0);
            this.m_labelName.ImageSize = new System.Drawing.Point(0, 0);
            this.m_labelName.LoadImage = null;
            this.m_labelName.Location = new System.Drawing.Point(183, 7);
            this.m_labelName.MainFont = new System.Drawing.Font("맑은 고딕", 11F, System.Drawing.FontStyle.Bold);
            this.m_labelName.MainFontColor = System.Drawing.Color.Black;
            this.m_labelName.Margin = new System.Windows.Forms.Padding(2);
            this.m_labelName.Name = "m_labelName";
            this.m_labelName.Size = new System.Drawing.Size(388, 44);
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
            this.m_labelName.Click += new System.EventHandler(this.Click_Configuration);
            // 
            // m_labelInput
            // 
            this.m_labelInput.BackGroundColor = System.Drawing.Color.White;
            this.m_labelInput.BorderStroke = 2;
            this.m_labelInput.BorderStyle = System.Windows.Forms.ButtonBorderStyle.Solid;
            this._tableLayoutPanel_InterruptConfiguration.SetColumnSpan(this.m_labelInput, 2);
            this.m_labelInput.Description = "";
            this.m_labelInput.DisabledColor = System.Drawing.Color.LightGray;
            this.m_labelInput.Dock = System.Windows.Forms.DockStyle.Fill;
            this.m_labelInput.EdgeRadius = 1;
            this.m_labelInput.Enabled = false;
            this.m_labelInput.ImagePosition = new System.Drawing.Point(0, 0);
            this.m_labelInput.ImageSize = new System.Drawing.Point(0, 0);
            this.m_labelInput.LoadImage = null;
            this.m_labelInput.Location = new System.Drawing.Point(183, 55);
            this.m_labelInput.MainFont = new System.Drawing.Font("맑은 고딕", 11F, System.Drawing.FontStyle.Bold);
            this.m_labelInput.MainFontColor = System.Drawing.Color.Black;
            this.m_labelInput.Margin = new System.Windows.Forms.Padding(2);
            this.m_labelInput.Name = "m_labelInput";
            this.m_labelInput.Size = new System.Drawing.Size(388, 44);
            this.m_labelInput.SubFont = new System.Drawing.Font("맑은 고딕", 10F, System.Drawing.FontStyle.Bold);
            this.m_labelInput.SubFontColor = System.Drawing.Color.Black;
            this.m_labelInput.SubText = "[ -1 ]";
            this.m_labelInput.TabIndex = 1;
            this.m_labelInput.Text = "--";
            this.m_labelInput.TextAlignMain = Sys3Controls.EN_TEXTALIGN.BOTTOM_RIGHT;
            this.m_labelInput.TextAlignSub = Sys3Controls.EN_TEXTALIGN.TOP_LEFT;
            this.m_labelInput.TextAlignUnit = Sys3Controls.EN_TEXTALIGN.MIDDLE_CENTER;
            this.m_labelInput.ThemeIndex = 0;
            this.m_labelInput.UnitAreaRate = 40;
            this.m_labelInput.UnitFont = new System.Drawing.Font("맑은 고딕", 10F, System.Drawing.FontStyle.Bold);
            this.m_labelInput.UnitFontColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.m_labelInput.UnitPositionVertical = false;
            this.m_labelInput.UnitText = "";
            this.m_labelInput.UseBorder = true;
            this.m_labelInput.UseEdgeRadius = false;
            this.m_labelInput.UseImage = false;
            this.m_labelInput.UseSubFont = true;
            this.m_labelInput.UseUnitFont = false;
            this.m_labelInput.Click += new System.EventHandler(this.Click_Configuration);
            // 
            // m_labelCondition
            // 
            this.m_labelCondition.BackGroundColor = System.Drawing.Color.White;
            this.m_labelCondition.BorderStroke = 2;
            this.m_labelCondition.BorderStyle = System.Windows.Forms.ButtonBorderStyle.Solid;
            this.m_labelCondition.Description = "";
            this.m_labelCondition.DisabledColor = System.Drawing.Color.LightGray;
            this.m_labelCondition.Dock = System.Windows.Forms.DockStyle.Fill;
            this.m_labelCondition.EdgeRadius = 1;
            this.m_labelCondition.Enabled = false;
            this.m_labelCondition.ImagePosition = new System.Drawing.Point(0, 0);
            this.m_labelCondition.ImageSize = new System.Drawing.Point(0, 0);
            this.m_labelCondition.LoadImage = null;
            this.m_labelCondition.Location = new System.Drawing.Point(201, 104);
            this.m_labelCondition.MainFont = new System.Drawing.Font("맑은 고딕", 11F, System.Drawing.FontStyle.Bold);
            this.m_labelCondition.MainFontColor = System.Drawing.Color.Black;
            this.m_labelCondition.Margin = new System.Windows.Forms.Padding(0, 3, 3, 3);
            this.m_labelCondition.Name = "m_labelCondition";
            this.m_labelCondition.Size = new System.Drawing.Size(369, 42);
            this.m_labelCondition.SubFont = new System.Drawing.Font("맑은 고딕", 10F);
            this.m_labelCondition.SubFontColor = System.Drawing.Color.Black;
            this.m_labelCondition.SubText = "";
            this.m_labelCondition.TabIndex = 2;
            this.m_labelCondition.Text = "--";
            this.m_labelCondition.TextAlignMain = Sys3Controls.EN_TEXTALIGN.BOTTOM_RIGHT;
            this.m_labelCondition.TextAlignSub = Sys3Controls.EN_TEXTALIGN.MIDDLE_CENTER;
            this.m_labelCondition.TextAlignUnit = Sys3Controls.EN_TEXTALIGN.MIDDLE_CENTER;
            this.m_labelCondition.ThemeIndex = 0;
            this.m_labelCondition.UnitAreaRate = 40;
            this.m_labelCondition.UnitFont = new System.Drawing.Font("맑은 고딕", 10F, System.Drawing.FontStyle.Bold);
            this.m_labelCondition.UnitFontColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.m_labelCondition.UnitPositionVertical = false;
            this.m_labelCondition.UnitText = "";
            this.m_labelCondition.UseBorder = true;
            this.m_labelCondition.UseEdgeRadius = false;
            this.m_labelCondition.UseImage = false;
            this.m_labelCondition.UseSubFont = false;
            this.m_labelCondition.UseUnitFont = false;
            this.m_labelCondition.Click += new System.EventHandler(this.Click_Configuration);
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
            this.m_labelAlarmCode.Location = new System.Drawing.Point(756, 7);
            this.m_labelAlarmCode.MainFont = new System.Drawing.Font("맑은 고딕", 11F, System.Drawing.FontStyle.Bold);
            this.m_labelAlarmCode.MainFontColor = System.Drawing.Color.Black;
            this.m_labelAlarmCode.Margin = new System.Windows.Forms.Padding(2);
            this.m_labelAlarmCode.Name = "m_labelAlarmCode";
            this.m_labelAlarmCode.Size = new System.Drawing.Size(368, 44);
            this.m_labelAlarmCode.SubFont = new System.Drawing.Font("맑은 고딕", 10F);
            this.m_labelAlarmCode.SubFontColor = System.Drawing.Color.Black;
            this.m_labelAlarmCode.SubText = "";
            this.m_labelAlarmCode.TabIndex = 3;
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
            // m_labelAction
            // 
            this.m_labelAction.BackGroundColor = System.Drawing.Color.White;
            this.m_labelAction.BorderStroke = 2;
            this.m_labelAction.BorderStyle = System.Windows.Forms.ButtonBorderStyle.Solid;
            this.m_labelAction.Description = "";
            this.m_labelAction.DisabledColor = System.Drawing.Color.LightGray;
            this.m_labelAction.Dock = System.Windows.Forms.DockStyle.Fill;
            this.m_labelAction.EdgeRadius = 1;
            this.m_labelAction.Enabled = false;
            this.m_labelAction.ImagePosition = new System.Drawing.Point(0, 0);
            this.m_labelAction.ImageSize = new System.Drawing.Point(0, 0);
            this.m_labelAction.LoadImage = null;
            this.m_labelAction.Location = new System.Drawing.Point(756, 55);
            this.m_labelAction.MainFont = new System.Drawing.Font("맑은 고딕", 11F, System.Drawing.FontStyle.Bold);
            this.m_labelAction.MainFontColor = System.Drawing.Color.Black;
            this.m_labelAction.Margin = new System.Windows.Forms.Padding(2);
            this.m_labelAction.Name = "m_labelAction";
            this.m_labelAction.Size = new System.Drawing.Size(368, 44);
            this.m_labelAction.SubFont = new System.Drawing.Font("맑은 고딕", 10F);
            this.m_labelAction.SubFontColor = System.Drawing.Color.Black;
            this.m_labelAction.SubText = "";
            this.m_labelAction.TabIndex = 4;
            this.m_labelAction.Text = "--";
            this.m_labelAction.TextAlignMain = Sys3Controls.EN_TEXTALIGN.BOTTOM_RIGHT;
            this.m_labelAction.TextAlignSub = Sys3Controls.EN_TEXTALIGN.MIDDLE_CENTER;
            this.m_labelAction.TextAlignUnit = Sys3Controls.EN_TEXTALIGN.MIDDLE_CENTER;
            this.m_labelAction.ThemeIndex = 0;
            this.m_labelAction.UnitAreaRate = 40;
            this.m_labelAction.UnitFont = new System.Drawing.Font("맑은 고딕", 10F, System.Drawing.FontStyle.Bold);
            this.m_labelAction.UnitFontColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.m_labelAction.UnitPositionVertical = false;
            this.m_labelAction.UnitText = "";
            this.m_labelAction.UseBorder = true;
            this.m_labelAction.UseEdgeRadius = false;
            this.m_labelAction.UseImage = false;
            this.m_labelAction.UseSubFont = false;
            this.m_labelAction.UseUnitFont = false;
            this.m_labelAction.Click += new System.EventHandler(this.Click_Configuration);
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
            this.m_btnDisable.Location = new System.Drawing.Point(129, 3);
            this.m_btnDisable.MainFont = new System.Drawing.Font("맑은 고딕", 13F, System.Drawing.FontStyle.Bold);
            this.m_btnDisable.MainFontColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(36)))), ((int)(((byte)(0)))));
            this.m_btnDisable.Name = "m_btnDisable";
            this.m_btnDisable.Size = new System.Drawing.Size(120, 80);
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
            this.m_btnEnable.Location = new System.Drawing.Point(3, 3);
            this.m_btnEnable.MainFont = new System.Drawing.Font("맑은 고딕", 13F, System.Drawing.FontStyle.Bold);
            this.m_btnEnable.MainFontColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(36)))), ((int)(((byte)(0)))));
            this.m_btnEnable.Name = "m_btnEnable";
            this.m_btnEnable.Size = new System.Drawing.Size(120, 80);
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
            this.m_btnRemove.Location = new System.Drawing.Point(408, 3);
            this.m_btnRemove.MainFont = new System.Drawing.Font("맑은 고딕", 13F, System.Drawing.FontStyle.Bold);
            this.m_btnRemove.MainFontColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(36)))), ((int)(((byte)(0)))));
            this.m_btnRemove.Name = "m_btnRemove";
            this.m_btnRemove.Size = new System.Drawing.Size(121, 80);
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
            this.m_btnAdd.Dock = System.Windows.Forms.DockStyle.Fill;
            this.m_btnAdd.EdgeRadius = 5;
            this.m_btnAdd.GradientAngle = 70F;
            this.m_btnAdd.GradientFirstColor = System.Drawing.Color.White;
            this.m_btnAdd.GradientSecondColor = System.Drawing.Color.FromArgb(((int)(((byte)(170)))), ((int)(((byte)(176)))), ((int)(((byte)(183)))));
            this.m_btnAdd.HoverEmphasizeCustomColor = System.Drawing.Color.White;
            this.m_btnAdd.ImagePosition = new System.Drawing.Point(7, 7);
            this.m_btnAdd.ImageSize = new System.Drawing.Point(30, 30);
            this.m_btnAdd.LoadImage = global::FrameOfSystem3.Properties.Resources.CONFIG_ADD3;
            this.m_btnAdd.Location = new System.Drawing.Point(282, 3);
            this.m_btnAdd.MainFont = new System.Drawing.Font("맑은 고딕", 13F, System.Drawing.FontStyle.Bold);
            this.m_btnAdd.MainFontColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(36)))), ((int)(((byte)(0)))));
            this.m_btnAdd.Name = "m_btnAdd";
            this.m_btnAdd.Size = new System.Drawing.Size(120, 80);
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
            this.m_btnAdd.Click += new System.EventHandler(this.Click_AddOrRemove);
            // 
            // sys3Label1
            // 
            this.sys3Label1.BackGroundColor = System.Drawing.Color.FromArgb(((int)(((byte)(200)))), ((int)(((byte)(200)))), ((int)(((byte)(220)))));
            this.sys3Label1.BorderStroke = 2;
            this.sys3Label1.BorderStyle = System.Windows.Forms.ButtonBorderStyle.Solid;
            this.sys3Label1.Description = "";
            this.sys3Label1.DisabledColor = System.Drawing.Color.DarkGray;
            this.sys3Label1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.sys3Label1.EdgeRadius = 1;
            this.sys3Label1.ImagePosition = new System.Drawing.Point(0, 0);
            this.sys3Label1.ImageSize = new System.Drawing.Point(0, 0);
            this.sys3Label1.LoadImage = null;
            this.sys3Label1.Location = new System.Drawing.Point(596, 103);
            this.sys3Label1.MainFont = new System.Drawing.Font("맑은 고딕", 9F, System.Drawing.FontStyle.Bold);
            this.sys3Label1.MainFontColor = System.Drawing.Color.Black;
            this.sys3Label1.Margin = new System.Windows.Forms.Padding(2);
            this.sys3Label1.Name = "sys3Label1";
            this.sys3Label1.Size = new System.Drawing.Size(156, 44);
            this.sys3Label1.SubFont = new System.Drawing.Font("맑은 고딕", 10F);
            this.sys3Label1.SubFontColor = System.Drawing.Color.Black;
            this.sys3Label1.SubText = "";
            this.sys3Label1.TabIndex = 1366;
            this.sys3Label1.Text = "DIGITAL OUTPUT INDEX";
            this.sys3Label1.TextAlignMain = Sys3Controls.EN_TEXTALIGN.TOP_LEFT;
            this.sys3Label1.TextAlignSub = Sys3Controls.EN_TEXTALIGN.MIDDLE_CENTER;
            this.sys3Label1.TextAlignUnit = Sys3Controls.EN_TEXTALIGN.MIDDLE_CENTER;
            this.sys3Label1.ThemeIndex = 0;
            this.sys3Label1.UnitAreaRate = 40;
            this.sys3Label1.UnitFont = new System.Drawing.Font("맑은 고딕", 10F, System.Drawing.FontStyle.Bold);
            this.sys3Label1.UnitFontColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.sys3Label1.UnitPositionVertical = false;
            this.sys3Label1.UnitText = "";
            this.sys3Label1.UseBorder = true;
            this.sys3Label1.UseEdgeRadius = false;
            this.sys3Label1.UseImage = false;
            this.sys3Label1.UseSubFont = false;
            this.sys3Label1.UseUnitFont = false;
            // 
            // m_labelDO
            // 
            this.m_labelDO.BackGroundColor = System.Drawing.Color.White;
            this.m_labelDO.BorderStroke = 2;
            this.m_labelDO.BorderStyle = System.Windows.Forms.ButtonBorderStyle.Solid;
            this.m_labelDO.Description = "";
            this.m_labelDO.DisabledColor = System.Drawing.Color.LightGray;
            this.m_labelDO.Dock = System.Windows.Forms.DockStyle.Fill;
            this.m_labelDO.EdgeRadius = 1;
            this.m_labelDO.Enabled = false;
            this.m_labelDO.ImagePosition = new System.Drawing.Point(0, 0);
            this.m_labelDO.ImageSize = new System.Drawing.Point(0, 0);
            this.m_labelDO.LoadImage = null;
            this.m_labelDO.Location = new System.Drawing.Point(756, 103);
            this.m_labelDO.MainFont = new System.Drawing.Font("맑은 고딕", 11F, System.Drawing.FontStyle.Bold);
            this.m_labelDO.MainFontColor = System.Drawing.Color.Black;
            this.m_labelDO.Margin = new System.Windows.Forms.Padding(2);
            this.m_labelDO.Name = "m_labelDO";
            this.m_labelDO.Size = new System.Drawing.Size(368, 44);
            this.m_labelDO.SubFont = new System.Drawing.Font("맑은 고딕", 10F, System.Drawing.FontStyle.Bold);
            this.m_labelDO.SubFontColor = System.Drawing.Color.Black;
            this.m_labelDO.SubText = "[ -1 ]";
            this.m_labelDO.TabIndex = 5;
            this.m_labelDO.Text = "--";
            this.m_labelDO.TextAlignMain = Sys3Controls.EN_TEXTALIGN.BOTTOM_RIGHT;
            this.m_labelDO.TextAlignSub = Sys3Controls.EN_TEXTALIGN.TOP_LEFT;
            this.m_labelDO.TextAlignUnit = Sys3Controls.EN_TEXTALIGN.MIDDLE_CENTER;
            this.m_labelDO.ThemeIndex = 0;
            this.m_labelDO.UnitAreaRate = 40;
            this.m_labelDO.UnitFont = new System.Drawing.Font("맑은 고딕", 10F, System.Drawing.FontStyle.Bold);
            this.m_labelDO.UnitFontColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.m_labelDO.UnitPositionVertical = false;
            this.m_labelDO.UnitText = "";
            this.m_labelDO.UseBorder = true;
            this.m_labelDO.UseEdgeRadius = false;
            this.m_labelDO.UseImage = false;
            this.m_labelDO.UseSubFont = true;
            this.m_labelDO.UseUnitFont = false;
            this.m_labelDO.Click += new System.EventHandler(this.Click_Configuration);
            // 
            // _tableLayoutPanel_Main
            // 
            this._tableLayoutPanel_Main.ColumnCount = 1;
            this._tableLayoutPanel_Main.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this._tableLayoutPanel_Main.Controls.Add(this.m_groupTitle, 0, 0);
            this._tableLayoutPanel_Main.Controls.Add(this.m_groupSelectedItem, 0, 1);
            this._tableLayoutPanel_Main.Controls.Add(this.m_groupConfiguration, 0, 2);
            this._tableLayoutPanel_Main.Dock = System.Windows.Forms.DockStyle.Fill;
            this._tableLayoutPanel_Main.Location = new System.Drawing.Point(0, 0);
            this._tableLayoutPanel_Main.Margin = new System.Windows.Forms.Padding(0);
            this._tableLayoutPanel_Main.Name = "_tableLayoutPanel_Main";
            this._tableLayoutPanel_Main.RowCount = 3;
            this._tableLayoutPanel_Main.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 57.44444F));
            this._tableLayoutPanel_Main.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 11.44444F));
            this._tableLayoutPanel_Main.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 31.22222F));
            this._tableLayoutPanel_Main.Size = new System.Drawing.Size(1156, 900);
            this._tableLayoutPanel_Main.TabIndex = 1369;
            // 
            // _tableLayoutPanel_InterruptConfiguration
            // 
            this._tableLayoutPanel_InterruptConfiguration.ColumnCount = 8;
            this._tableLayoutPanel_InterruptConfiguration.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 1.890941F));
            this._tableLayoutPanel_InterruptConfiguration.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 14.18206F));
            this._tableLayoutPanel_InterruptConfiguration.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this._tableLayoutPanel_InterruptConfiguration.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 32.98153F));
            this._tableLayoutPanel_InterruptConfiguration.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 1.890941F));
            this._tableLayoutPanel_InterruptConfiguration.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 14.18206F));
            this._tableLayoutPanel_InterruptConfiguration.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 32.98153F));
            this._tableLayoutPanel_InterruptConfiguration.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 1.890941F));
            this._tableLayoutPanel_InterruptConfiguration.Controls.Add(this.m_lblName, 1, 1);
            this._tableLayoutPanel_InterruptConfiguration.Controls.Add(this.m_lblInput, 1, 2);
            this._tableLayoutPanel_InterruptConfiguration.Controls.Add(this.m_lblCondition, 1, 3);
            this._tableLayoutPanel_InterruptConfiguration.Controls.Add(this.m_labelCondition, 3, 3);
            this._tableLayoutPanel_InterruptConfiguration.Controls.Add(this.m_ledInput, 2, 3);
            this._tableLayoutPanel_InterruptConfiguration.Controls.Add(this.m_labelDO, 6, 3);
            this._tableLayoutPanel_InterruptConfiguration.Controls.Add(this.m_labelAction, 6, 2);
            this._tableLayoutPanel_InterruptConfiguration.Controls.Add(this.m_labelName, 2, 1);
            this._tableLayoutPanel_InterruptConfiguration.Controls.Add(this.m_labelAlarmCode, 6, 1);
            this._tableLayoutPanel_InterruptConfiguration.Controls.Add(this.m_labelInput, 2, 2);
            this._tableLayoutPanel_InterruptConfiguration.Controls.Add(this.m_lblAlarmCode, 5, 1);
            this._tableLayoutPanel_InterruptConfiguration.Controls.Add(this.sys3Label1, 5, 3);
            this._tableLayoutPanel_InterruptConfiguration.Controls.Add(this.m_lblAction, 5, 2);
            this._tableLayoutPanel_InterruptConfiguration.Controls.Add(this._tableLayoutPanel_Buttons, 5, 5);
            this._tableLayoutPanel_InterruptConfiguration.Dock = System.Windows.Forms.DockStyle.Fill;
            this._tableLayoutPanel_InterruptConfiguration.Location = new System.Drawing.Point(3, 32);
            this._tableLayoutPanel_InterruptConfiguration.Margin = new System.Windows.Forms.Padding(0);
            this._tableLayoutPanel_InterruptConfiguration.Name = "_tableLayoutPanel_InterruptConfiguration";
            this._tableLayoutPanel_InterruptConfiguration.RowCount = 7;
            this._tableLayoutPanel_InterruptConfiguration.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 2.12053F));
            this._tableLayoutPanel_InterruptConfiguration.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 19.4792F));
            this._tableLayoutPanel_InterruptConfiguration.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 19.4792F));
            this._tableLayoutPanel_InterruptConfiguration.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 19.4792F));
            this._tableLayoutPanel_InterruptConfiguration.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 2.120531F));
            this._tableLayoutPanel_InterruptConfiguration.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 35.20082F));
            this._tableLayoutPanel_InterruptConfiguration.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 2.120531F));
            this._tableLayoutPanel_InterruptConfiguration.Size = new System.Drawing.Size(1150, 247);
            this._tableLayoutPanel_InterruptConfiguration.TabIndex = 1;
            // 
            // _tableLayoutPanel_Buttons
            // 
            this._tableLayoutPanel_Buttons.ColumnCount = 5;
            this._tableLayoutPanel_InterruptConfiguration.SetColumnSpan(this._tableLayoutPanel_Buttons, 2);
            this._tableLayoutPanel_Buttons.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 23.69387F));
            this._tableLayoutPanel_Buttons.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 23.69387F));
            this._tableLayoutPanel_Buttons.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 5.224499F));
            this._tableLayoutPanel_Buttons.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 23.69387F));
            this._tableLayoutPanel_Buttons.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 23.69387F));
            this._tableLayoutPanel_Buttons.Controls.Add(this.m_btnEnable, 0, 0);
            this._tableLayoutPanel_Buttons.Controls.Add(this.m_btnRemove, 4, 0);
            this._tableLayoutPanel_Buttons.Controls.Add(this.m_btnDisable, 1, 0);
            this._tableLayoutPanel_Buttons.Controls.Add(this.m_btnAdd, 3, 0);
            this._tableLayoutPanel_Buttons.Dock = System.Windows.Forms.DockStyle.Fill;
            this._tableLayoutPanel_Buttons.Location = new System.Drawing.Point(594, 154);
            this._tableLayoutPanel_Buttons.Margin = new System.Windows.Forms.Padding(0);
            this._tableLayoutPanel_Buttons.Name = "_tableLayoutPanel_Buttons";
            this._tableLayoutPanel_Buttons.RowCount = 1;
            this._tableLayoutPanel_Buttons.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this._tableLayoutPanel_Buttons.Size = new System.Drawing.Size(532, 86);
            this._tableLayoutPanel_Buttons.TabIndex = 1368;
            // 
            // Config_Interrupt
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.Controls.Add(this._tableLayoutPanel_Main);
            this.Name = "Config_Interrupt";
            this.Size = new System.Drawing.Size(1156, 900);
            ((System.ComponentModel.ISupportInitialize)(this.m_dgViewInterrupt)).EndInit();
            this.m_groupTitle.ResumeLayout(false);
            this.m_groupSelectedItem.ResumeLayout(false);
            this._tableLayoutPanel_InterruptKey.ResumeLayout(false);
            this.m_groupConfiguration.ResumeLayout(false);
            this._tableLayoutPanel_Main.ResumeLayout(false);
            this._tableLayoutPanel_InterruptConfiguration.ResumeLayout(false);
            this._tableLayoutPanel_Buttons.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

		private Sys3Controls.Sys3LedLabel m_ledInput;
        private Sys3Controls.Sys3DoubleBufferedDataGridView m_dgViewInterrupt;
		private Sys3Controls.Sys3GroupBoxContainer m_groupTitle;
        private Sys3Controls.Sys3GroupBoxContainer m_groupSelectedItem;
        private Sys3Controls.Sys3GroupBoxContainer m_groupConfiguration;
		private Sys3Controls.Sys3Label m_lblKey;
		private Sys3Controls.Sys3Label m_lblName;
		private Sys3Controls.Sys3Label m_lblInput;
		private Sys3Controls.Sys3Label m_lblCondition;
		private Sys3Controls.Sys3Label m_lblAlarmCode;
		private Sys3Controls.Sys3Label m_lblAction;
		private Sys3Controls.Sys3Label m_labelKey;
		private Sys3Controls.Sys3Label m_labelName;
		private Sys3Controls.Sys3Label m_labelInput;
		private Sys3Controls.Sys3Label m_labelCondition;
		private Sys3Controls.Sys3Label m_labelAlarmCode;
		private Sys3Controls.Sys3Label m_labelAction;
		private Sys3Controls.Sys3button m_btnRemove;
		private Sys3Controls.Sys3button m_btnAdd;
		private Sys3Controls.Sys3button m_btnDisable;
		private Sys3Controls.Sys3button m_btnEnable;
        private Sys3Controls.Sys3Label sys3Label1;
        private Sys3Controls.Sys3Label m_labelDO;
        private System.Windows.Forms.DataGridViewTextBoxColumn ID;
        private System.Windows.Forms.DataGridViewTextBoxColumn ENABLE;
        private System.Windows.Forms.DataGridViewTextBoxColumn PASSWORD;
        private System.Windows.Forms.DataGridViewTextBoxColumn AUTHORITY;
        private System.Windows.Forms.DataGridViewTextBoxColumn TARGET_ACT;
        private System.Windows.Forms.DataGridViewTextBoxColumn ACTION;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn7;
        private System.Windows.Forms.DataGridViewTextBoxColumn DO_INDEX;
        private System.Windows.Forms.DataGridViewTextBoxColumn CONDITION;
        private System.Windows.Forms.DataGridViewTextBoxColumn STATE;
        private System.Windows.Forms.TableLayoutPanel _tableLayoutPanel_Main;
        private System.Windows.Forms.TableLayoutPanel _tableLayoutPanel_InterruptKey;
        private System.Windows.Forms.TableLayoutPanel _tableLayoutPanel_InterruptConfiguration;
        private System.Windows.Forms.TableLayoutPanel _tableLayoutPanel_Buttons;
    }
}
