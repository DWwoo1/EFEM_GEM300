namespace FrameOfSystem3.Views.Config.Action
{
    partial class Config_Port
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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle4 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle5 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle6 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle3 = new System.Windows.Forms.DataGridViewCellStyle();
            this.sys3GroupBox2 = new Sys3Controls.Sys3GroupBoxContainer();
            this.sys3GroupBox1 = new Sys3Controls.Sys3GroupBoxContainer();
            this.sys3GroupBox3 = new Sys3Controls.Sys3GroupBoxContainer();
            this.m_lbTaskName = new Sys3Controls.Sys3Label();
            this.m_lblPortName = new Sys3Controls.Sys3Label();
            this.m_lblStatus = new Sys3Controls.Sys3Label();
            this.m_dgViewPortList = new Sys3Controls.Sys3DoubleBufferedDataGridView();
            this.dataGridViewTextBoxColumn3 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ENABLE = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn4 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.STATUS = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn2 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.m_dgViewTask = new Sys3Controls.Sys3DoubleBufferedDataGridView();
            this.m_btnDisable = new Sys3Controls.Sys3button();
            this.m_btnEnable = new Sys3Controls.Sys3button();
            this.sys3button2 = new Sys3Controls.Sys3button();
            this.m_btnAdd = new Sys3Controls.Sys3button();
            this.m_labelTaskName = new Sys3Controls.Sys3Label();
            this.sys3Separator1 = new SUserControls.Sys3Separator();
            this.m_labelPortName = new Sys3Controls.Sys3Label();
            this.m_labelStatus = new Sys3Controls.Sys3Label();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.tableLayoutPanel2 = new System.Windows.Forms.TableLayoutPanel();
            this.tableLayoutPanel3 = new System.Windows.Forms.TableLayoutPanel();
            this.sys3GroupBox2.SuspendLayout();
            this.sys3GroupBox1.SuspendLayout();
            this.sys3GroupBox3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.m_dgViewPortList)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.m_dgViewTask)).BeginInit();
            this.tableLayoutPanel1.SuspendLayout();
            this.tableLayoutPanel2.SuspendLayout();
            this.tableLayoutPanel3.SuspendLayout();
            this.SuspendLayout();
            // 
            // sys3GroupBox2
            // 
            this.sys3GroupBox2.BackGroundColor = System.Drawing.Color.WhiteSmoke;
            this.sys3GroupBox2.Controls.Add(this.m_dgViewTask);
            this.sys3GroupBox2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.sys3GroupBox2.EdgeBorderStroke = 2;
            this.sys3GroupBox2.EdgeRadius = 2;
            this.sys3GroupBox2.LabelFont = new System.Drawing.Font("맑은 고딕", 15F, System.Drawing.FontStyle.Bold);
            this.sys3GroupBox2.LabelGradientColorFirst = System.Drawing.Color.FromArgb(((int)(((byte)(208)))), ((int)(((byte)(208)))), ((int)(((byte)(208)))));
            this.sys3GroupBox2.LabelGradientColorSecond = System.Drawing.Color.FromArgb(((int)(((byte)(240)))), ((int)(((byte)(240)))), ((int)(((byte)(240)))));
            this.sys3GroupBox2.LabelHeight = 42;
            this.sys3GroupBox2.LabelTextColor = System.Drawing.Color.Black;
            this.sys3GroupBox2.Location = new System.Drawing.Point(0, 0);
            this.sys3GroupBox2.Margin = new System.Windows.Forms.Padding(0);
            this.sys3GroupBox2.Name = "sys3GroupBox2";
            this.sys3GroupBox2.Padding = new System.Windows.Forms.Padding(5, 30, 5, 5);
            this.sys3GroupBox2.Size = new System.Drawing.Size(403, 468);
            this.sys3GroupBox2.TabIndex = 1470;
            this.sys3GroupBox2.TabStop = false;
            this.sys3GroupBox2.Text = "TASK LIST";
            this.sys3GroupBox2.TextAlign = Sys3Controls.EN_TEXTALIGN.MIDDLE_CENTER;
            this.sys3GroupBox2.ThemeIndex = 0;
            this.sys3GroupBox2.UseLabelBorder = true;
            this.sys3GroupBox2.UseTitle = true;
            // 
            // sys3GroupBox1
            // 
            this.sys3GroupBox1.BackGroundColor = System.Drawing.Color.WhiteSmoke;
            this.sys3GroupBox1.Controls.Add(this.m_dgViewPortList);
            this.sys3GroupBox1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.sys3GroupBox1.EdgeBorderStroke = 2;
            this.sys3GroupBox1.EdgeRadius = 2;
            this.sys3GroupBox1.LabelFont = new System.Drawing.Font("맑은 고딕", 15F, System.Drawing.FontStyle.Bold);
            this.sys3GroupBox1.LabelGradientColorFirst = System.Drawing.Color.FromArgb(((int)(((byte)(208)))), ((int)(((byte)(208)))), ((int)(((byte)(208)))));
            this.sys3GroupBox1.LabelGradientColorSecond = System.Drawing.Color.FromArgb(((int)(((byte)(240)))), ((int)(((byte)(240)))), ((int)(((byte)(240)))));
            this.sys3GroupBox1.LabelHeight = 42;
            this.sys3GroupBox1.LabelTextColor = System.Drawing.Color.Black;
            this.sys3GroupBox1.Location = new System.Drawing.Point(403, 0);
            this.sys3GroupBox1.Margin = new System.Windows.Forms.Padding(0);
            this.sys3GroupBox1.Name = "sys3GroupBox1";
            this.sys3GroupBox1.Padding = new System.Windows.Forms.Padding(5, 30, 5, 5);
            this.sys3GroupBox1.Size = new System.Drawing.Size(753, 468);
            this.sys3GroupBox1.TabIndex = 1471;
            this.sys3GroupBox1.TabStop = false;
            this.sys3GroupBox1.Text = "PORT LIST";
            this.sys3GroupBox1.TextAlign = Sys3Controls.EN_TEXTALIGN.MIDDLE_CENTER;
            this.sys3GroupBox1.ThemeIndex = 0;
            this.sys3GroupBox1.UseLabelBorder = true;
            this.sys3GroupBox1.UseTitle = true;
            // 
            // sys3GroupBox3
            // 
            this.sys3GroupBox3.BackGroundColor = System.Drawing.Color.WhiteSmoke;
            this.tableLayoutPanel1.SetColumnSpan(this.sys3GroupBox3, 2);
            this.sys3GroupBox3.Controls.Add(this.tableLayoutPanel2);
            this.sys3GroupBox3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.sys3GroupBox3.EdgeBorderStroke = 2;
            this.sys3GroupBox3.EdgeRadius = 2;
            this.sys3GroupBox3.LabelFont = new System.Drawing.Font("맑은 고딕", 15F, System.Drawing.FontStyle.Bold);
            this.sys3GroupBox3.LabelGradientColorFirst = System.Drawing.Color.FromArgb(((int)(((byte)(208)))), ((int)(((byte)(208)))), ((int)(((byte)(208)))));
            this.sys3GroupBox3.LabelGradientColorSecond = System.Drawing.Color.FromArgb(((int)(((byte)(240)))), ((int)(((byte)(240)))), ((int)(((byte)(240)))));
            this.sys3GroupBox3.LabelHeight = 42;
            this.sys3GroupBox3.LabelTextColor = System.Drawing.Color.Black;
            this.sys3GroupBox3.Location = new System.Drawing.Point(0, 468);
            this.sys3GroupBox3.Margin = new System.Windows.Forms.Padding(0);
            this.sys3GroupBox3.Name = "sys3GroupBox3";
            this.sys3GroupBox3.Padding = new System.Windows.Forms.Padding(3, 30, 3, 3);
            this.sys3GroupBox3.Size = new System.Drawing.Size(1156, 383);
            this.sys3GroupBox3.TabIndex = 1472;
            this.sys3GroupBox3.TabStop = false;
            this.sys3GroupBox3.Text = "SELECTED ITEM";
            this.sys3GroupBox3.TextAlign = Sys3Controls.EN_TEXTALIGN.MIDDLE_CENTER;
            this.sys3GroupBox3.ThemeIndex = 0;
            this.sys3GroupBox3.UseLabelBorder = true;
            this.sys3GroupBox3.UseTitle = true;
            // 
            // m_lbTaskName
            // 
            this.m_lbTaskName.BackGroundColor = System.Drawing.Color.FromArgb(((int)(((byte)(200)))), ((int)(((byte)(200)))), ((int)(((byte)(220)))));
            this.m_lbTaskName.BorderStroke = 2;
            this.m_lbTaskName.BorderStyle = System.Windows.Forms.ButtonBorderStyle.Solid;
            this.m_lbTaskName.Description = "";
            this.m_lbTaskName.DisabledColor = System.Drawing.Color.DarkGray;
            this.m_lbTaskName.Dock = System.Windows.Forms.DockStyle.Fill;
            this.m_lbTaskName.EdgeRadius = 1;
            this.m_lbTaskName.ImagePosition = new System.Drawing.Point(0, 0);
            this.m_lbTaskName.ImageSize = new System.Drawing.Point(0, 0);
            this.m_lbTaskName.LoadImage = null;
            this.m_lbTaskName.Location = new System.Drawing.Point(8, 11);
            this.m_lbTaskName.MainFont = new System.Drawing.Font("맑은 고딕", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.m_lbTaskName.MainFontColor = System.Drawing.Color.Black;
            this.m_lbTaskName.Margin = new System.Windows.Forms.Padding(1);
            this.m_lbTaskName.Name = "m_lbTaskName";
            this.m_lbTaskName.Size = new System.Drawing.Size(153, 50);
            this.m_lbTaskName.SubFont = new System.Drawing.Font("맑은 고딕", 10F);
            this.m_lbTaskName.SubFontColor = System.Drawing.Color.Black;
            this.m_lbTaskName.SubText = "";
            this.m_lbTaskName.TabIndex = 1477;
            this.m_lbTaskName.Text = "TASK NAME";
            this.m_lbTaskName.TextAlignMain = Sys3Controls.EN_TEXTALIGN.TOP_LEFT;
            this.m_lbTaskName.TextAlignSub = Sys3Controls.EN_TEXTALIGN.MIDDLE_CENTER;
            this.m_lbTaskName.TextAlignUnit = Sys3Controls.EN_TEXTALIGN.MIDDLE_CENTER;
            this.m_lbTaskName.ThemeIndex = 0;
            this.m_lbTaskName.UnitAreaRate = 40;
            this.m_lbTaskName.UnitFont = new System.Drawing.Font("맑은 고딕", 10F, System.Drawing.FontStyle.Bold);
            this.m_lbTaskName.UnitFontColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.m_lbTaskName.UnitPositionVertical = false;
            this.m_lbTaskName.UnitText = "";
            this.m_lbTaskName.UseBorder = true;
            this.m_lbTaskName.UseEdgeRadius = false;
            this.m_lbTaskName.UseImage = false;
            this.m_lbTaskName.UseSubFont = false;
            this.m_lbTaskName.UseUnitFont = false;
            // 
            // m_lblPortName
            // 
            this.m_lblPortName.BackGroundColor = System.Drawing.Color.FromArgb(((int)(((byte)(200)))), ((int)(((byte)(200)))), ((int)(((byte)(220)))));
            this.m_lblPortName.BorderStroke = 2;
            this.m_lblPortName.BorderStyle = System.Windows.Forms.ButtonBorderStyle.Solid;
            this.m_lblPortName.Description = "";
            this.m_lblPortName.DisabledColor = System.Drawing.Color.DarkGray;
            this.m_lblPortName.Dock = System.Windows.Forms.DockStyle.Fill;
            this.m_lblPortName.EdgeRadius = 1;
            this.m_lblPortName.ImagePosition = new System.Drawing.Point(0, 0);
            this.m_lblPortName.ImageSize = new System.Drawing.Point(0, 0);
            this.m_lblPortName.LoadImage = null;
            this.m_lblPortName.Location = new System.Drawing.Point(593, 11);
            this.m_lblPortName.MainFont = new System.Drawing.Font("맑은 고딕", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.m_lblPortName.MainFontColor = System.Drawing.Color.Black;
            this.m_lblPortName.Margin = new System.Windows.Forms.Padding(1);
            this.m_lblPortName.Name = "m_lblPortName";
            this.m_lblPortName.Size = new System.Drawing.Size(153, 50);
            this.m_lblPortName.SubFont = new System.Drawing.Font("맑은 고딕", 10F);
            this.m_lblPortName.SubFontColor = System.Drawing.Color.Black;
            this.m_lblPortName.SubText = "";
            this.m_lblPortName.TabIndex = 1477;
            this.m_lblPortName.Text = "PORT NAME";
            this.m_lblPortName.TextAlignMain = Sys3Controls.EN_TEXTALIGN.TOP_LEFT;
            this.m_lblPortName.TextAlignSub = Sys3Controls.EN_TEXTALIGN.MIDDLE_CENTER;
            this.m_lblPortName.TextAlignUnit = Sys3Controls.EN_TEXTALIGN.MIDDLE_CENTER;
            this.m_lblPortName.ThemeIndex = 0;
            this.m_lblPortName.UnitAreaRate = 40;
            this.m_lblPortName.UnitFont = new System.Drawing.Font("맑은 고딕", 10F, System.Drawing.FontStyle.Bold);
            this.m_lblPortName.UnitFontColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.m_lblPortName.UnitPositionVertical = false;
            this.m_lblPortName.UnitText = "";
            this.m_lblPortName.UseBorder = true;
            this.m_lblPortName.UseEdgeRadius = false;
            this.m_lblPortName.UseImage = false;
            this.m_lblPortName.UseSubFont = false;
            this.m_lblPortName.UseUnitFont = false;
            // 
            // m_lblStatus
            // 
            this.m_lblStatus.BackGroundColor = System.Drawing.Color.FromArgb(((int)(((byte)(200)))), ((int)(((byte)(200)))), ((int)(((byte)(220)))));
            this.m_lblStatus.BorderStroke = 2;
            this.m_lblStatus.BorderStyle = System.Windows.Forms.ButtonBorderStyle.Solid;
            this.m_lblStatus.Description = "";
            this.m_lblStatus.DisabledColor = System.Drawing.Color.DarkGray;
            this.m_lblStatus.Dock = System.Windows.Forms.DockStyle.Fill;
            this.m_lblStatus.EdgeRadius = 1;
            this.m_lblStatus.ImagePosition = new System.Drawing.Point(0, 0);
            this.m_lblStatus.ImageSize = new System.Drawing.Point(0, 0);
            this.m_lblStatus.LoadImage = null;
            this.m_lblStatus.Location = new System.Drawing.Point(8, 115);
            this.m_lblStatus.MainFont = new System.Drawing.Font("맑은 고딕", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.m_lblStatus.MainFontColor = System.Drawing.Color.Black;
            this.m_lblStatus.Margin = new System.Windows.Forms.Padding(1);
            this.m_lblStatus.Name = "m_lblStatus";
            this.m_lblStatus.Size = new System.Drawing.Size(153, 50);
            this.m_lblStatus.SubFont = new System.Drawing.Font("맑은 고딕", 10F);
            this.m_lblStatus.SubFontColor = System.Drawing.Color.Black;
            this.m_lblStatus.SubText = "";
            this.m_lblStatus.TabIndex = 1477;
            this.m_lblStatus.Text = "STATUS";
            this.m_lblStatus.TextAlignMain = Sys3Controls.EN_TEXTALIGN.TOP_LEFT;
            this.m_lblStatus.TextAlignSub = Sys3Controls.EN_TEXTALIGN.MIDDLE_CENTER;
            this.m_lblStatus.TextAlignUnit = Sys3Controls.EN_TEXTALIGN.MIDDLE_CENTER;
            this.m_lblStatus.ThemeIndex = 0;
            this.m_lblStatus.UnitAreaRate = 40;
            this.m_lblStatus.UnitFont = new System.Drawing.Font("맑은 고딕", 10F, System.Drawing.FontStyle.Bold);
            this.m_lblStatus.UnitFontColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.m_lblStatus.UnitPositionVertical = false;
            this.m_lblStatus.UnitText = "";
            this.m_lblStatus.UseBorder = true;
            this.m_lblStatus.UseEdgeRadius = false;
            this.m_lblStatus.UseImage = false;
            this.m_lblStatus.UseSubFont = false;
            this.m_lblStatus.UseUnitFont = false;
            // 
            // m_dgViewPortList
            // 
            this.m_dgViewPortList.AllowUserToAddRows = false;
            this.m_dgViewPortList.AllowUserToDeleteRows = false;
            this.m_dgViewPortList.AllowUserToResizeColumns = false;
            this.m_dgViewPortList.AllowUserToResizeRows = false;
            this.m_dgViewPortList.BackgroundColor = System.Drawing.Color.White;
            this.m_dgViewPortList.CellBorderStyle = System.Windows.Forms.DataGridViewCellBorderStyle.Raised;
            this.m_dgViewPortList.ColumnHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.Single;
            dataGridViewCellStyle4.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle4.BackColor = System.Drawing.Color.LightGray;
            dataGridViewCellStyle4.Font = new System.Drawing.Font("맑은 고딕", 9F, System.Drawing.FontStyle.Bold);
            dataGridViewCellStyle4.ForeColor = System.Drawing.Color.Black;
            dataGridViewCellStyle4.SelectionBackColor = System.Drawing.Color.White;
            dataGridViewCellStyle4.SelectionForeColor = System.Drawing.Color.Black;
            dataGridViewCellStyle4.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.m_dgViewPortList.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle4;
            this.m_dgViewPortList.ColumnHeadersHeight = 30;
            this.m_dgViewPortList.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            this.m_dgViewPortList.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.dataGridViewTextBoxColumn3,
            this.ENABLE,
            this.dataGridViewTextBoxColumn4,
            this.STATUS});
            dataGridViewCellStyle5.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle5.BackColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle5.Font = new System.Drawing.Font("맑은 고딕", 10F, System.Drawing.FontStyle.Bold);
            dataGridViewCellStyle5.ForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle5.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(120)))), ((int)(((byte)(130)))), ((int)(((byte)(150)))));
            dataGridViewCellStyle5.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle5.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.m_dgViewPortList.DefaultCellStyle = dataGridViewCellStyle5;
            this.m_dgViewPortList.Dock = System.Windows.Forms.DockStyle.Fill;
            this.m_dgViewPortList.EnableHeadersVisualStyles = false;
            this.m_dgViewPortList.GridColor = System.Drawing.Color.FromArgb(((int)(((byte)(120)))), ((int)(((byte)(130)))), ((int)(((byte)(150)))));
            this.m_dgViewPortList.Location = new System.Drawing.Point(5, 44);
            this.m_dgViewPortList.MultiSelect = false;
            this.m_dgViewPortList.Name = "m_dgViewPortList";
            this.m_dgViewPortList.ReadOnly = true;
            this.m_dgViewPortList.RowHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.None;
            dataGridViewCellStyle6.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle6.BackColor = System.Drawing.Color.White;
            dataGridViewCellStyle6.Font = new System.Drawing.Font("맑은 고딕", 11F);
            dataGridViewCellStyle6.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle6.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(120)))), ((int)(((byte)(130)))), ((int)(((byte)(150)))));
            dataGridViewCellStyle6.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle6.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.m_dgViewPortList.RowHeadersDefaultCellStyle = dataGridViewCellStyle6;
            this.m_dgViewPortList.RowHeadersVisible = false;
            this.m_dgViewPortList.RowHeadersWidthSizeMode = System.Windows.Forms.DataGridViewRowHeadersWidthSizeMode.DisableResizing;
            this.m_dgViewPortList.RowTemplate.Height = 30;
            this.m_dgViewPortList.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.m_dgViewPortList.Size = new System.Drawing.Size(743, 419);
            this.m_dgViewPortList.TabIndex = 1378;
            this.m_dgViewPortList.CellClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.Click_PortList);
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
            // ENABLE
            // 
            this.ENABLE.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.ColumnHeader;
            this.ENABLE.Frozen = true;
            this.ENABLE.HeaderText = "ENABLE";
            this.ENABLE.MaxInputLength = 20;
            this.ENABLE.Name = "ENABLE";
            this.ENABLE.ReadOnly = true;
            this.ENABLE.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.ENABLE.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.ENABLE.Width = 56;
            // 
            // dataGridViewTextBoxColumn4
            // 
            this.dataGridViewTextBoxColumn4.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.dataGridViewTextBoxColumn4.HeaderText = "PORT NAME";
            this.dataGridViewTextBoxColumn4.MaxInputLength = 20;
            this.dataGridViewTextBoxColumn4.Name = "dataGridViewTextBoxColumn4";
            this.dataGridViewTextBoxColumn4.ReadOnly = true;
            this.dataGridViewTextBoxColumn4.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.dataGridViewTextBoxColumn4.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // STATUS
            // 
            this.STATUS.HeaderText = "STATUS";
            this.STATUS.MaxInputLength = 500;
            this.STATUS.Name = "STATUS";
            this.STATUS.ReadOnly = true;
            this.STATUS.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.STATUS.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.STATUS.Width = 200;
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
            dataGridViewCellStyle1.Font = new System.Drawing.Font("맑은 고딕", 9F, System.Drawing.FontStyle.Bold);
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
            this.m_dgViewTask.Location = new System.Drawing.Point(5, 44);
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
            this.m_dgViewTask.Size = new System.Drawing.Size(393, 419);
            this.m_dgViewTask.TabIndex = 1379;
            this.m_dgViewTask.CellClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.Click_Task);
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
            this.m_btnDisable.Location = new System.Drawing.Point(142, 67);
            this.m_btnDisable.MainFont = new System.Drawing.Font("맑은 고딕", 13F, System.Drawing.FontStyle.Bold);
            this.m_btnDisable.MainFontColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(36)))), ((int)(((byte)(0)))));
            this.m_btnDisable.Name = "m_btnDisable";
            this.m_btnDisable.Size = new System.Drawing.Size(133, 87);
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
            this.m_btnDisable.Click += new System.EventHandler(this.Click_EnableDisable);
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
            this.m_btnEnable.Location = new System.Drawing.Point(3, 67);
            this.m_btnEnable.MainFont = new System.Drawing.Font("맑은 고딕", 13F, System.Drawing.FontStyle.Bold);
            this.m_btnEnable.MainFontColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(36)))), ((int)(((byte)(0)))));
            this.m_btnEnable.Name = "m_btnEnable";
            this.m_btnEnable.Size = new System.Drawing.Size(133, 87);
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
            this.m_btnEnable.Click += new System.EventHandler(this.Click_EnableDisable);
            // 
            // sys3button2
            // 
            this.sys3button2.BorderWidth = 3;
            this.sys3button2.ButtonClicked = false;
            this.sys3button2.ClickedEmphasizeTextColor = System.Drawing.Color.White;
            this.sys3button2.CustomClickedGradientFirstColor = System.Drawing.Color.White;
            this.sys3button2.CustomClickedGradientSecondColor = System.Drawing.Color.White;
            this.sys3button2.Description = "";
            this.sys3button2.DisabledColor = System.Drawing.Color.DarkGray;
            this.sys3button2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.sys3button2.EdgeRadius = 5;
            this.sys3button2.Enabled = false;
            this.sys3button2.GradientAngle = 70F;
            this.sys3button2.GradientFirstColor = System.Drawing.Color.White;
            this.sys3button2.GradientSecondColor = System.Drawing.Color.FromArgb(((int)(((byte)(170)))), ((int)(((byte)(176)))), ((int)(((byte)(183)))));
            this.sys3button2.HoverEmphasizeCustomColor = System.Drawing.Color.White;
            this.sys3button2.ImagePosition = new System.Drawing.Point(7, 7);
            this.sys3button2.ImageSize = new System.Drawing.Point(30, 30);
            this.sys3button2.LoadImage = global::FrameOfSystem3.Properties.Resources.REMOVE;
            this.sys3button2.Location = new System.Drawing.Point(447, 67);
            this.sys3button2.MainFont = new System.Drawing.Font("맑은 고딕", 13F, System.Drawing.FontStyle.Bold);
            this.sys3button2.MainFontColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(36)))), ((int)(((byte)(0)))));
            this.sys3button2.Name = "sys3button2";
            this.sys3button2.Size = new System.Drawing.Size(135, 87);
            this.sys3button2.SubFont = new System.Drawing.Font("맑은 고딕", 8F, System.Drawing.FontStyle.Bold);
            this.sys3button2.SubFontColor = System.Drawing.Color.DarkBlue;
            this.sys3button2.SubText = "STATUS";
            this.sys3button2.TabIndex = 1476;
            this.sys3button2.Text = "REMOVE";
            this.sys3button2.TextAlignMain = Sys3Controls.EN_TEXTALIGN.MIDDLE_RIGHT;
            this.sys3button2.TextAlignSub = Sys3Controls.EN_TEXTALIGN.TOP_LEFT;
            this.sys3button2.ThemeIndex = 0;
            this.sys3button2.UseBorder = true;
            this.sys3button2.UseClickedEmphasizeTextColor = false;
            this.sys3button2.UseCustomizeClickedColor = false;
            this.sys3button2.UseEdge = true;
            this.sys3button2.UseHoverEmphasizeCustomColor = false;
            this.sys3button2.UseImage = true;
            this.sys3button2.UserHoverEmpahsize = false;
            this.sys3button2.UseSubFont = false;
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
            this.m_btnAdd.Location = new System.Drawing.Point(308, 67);
            this.m_btnAdd.MainFont = new System.Drawing.Font("맑은 고딕", 13F, System.Drawing.FontStyle.Bold);
            this.m_btnAdd.MainFontColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(36)))), ((int)(((byte)(0)))));
            this.m_btnAdd.Name = "m_btnAdd";
            this.m_btnAdd.Size = new System.Drawing.Size(133, 87);
            this.m_btnAdd.SubFont = new System.Drawing.Font("맑은 고딕", 8F, System.Drawing.FontStyle.Bold);
            this.m_btnAdd.SubFontColor = System.Drawing.Color.DarkBlue;
            this.m_btnAdd.SubText = "STATUS";
            this.m_btnAdd.TabIndex = 1474;
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
            // m_labelTaskName
            // 
            this.m_labelTaskName.BackGroundColor = System.Drawing.Color.White;
            this.m_labelTaskName.BorderStroke = 2;
            this.m_labelTaskName.BorderStyle = System.Windows.Forms.ButtonBorderStyle.Solid;
            this.m_labelTaskName.Description = "";
            this.m_labelTaskName.DisabledColor = System.Drawing.Color.LightGray;
            this.m_labelTaskName.Dock = System.Windows.Forms.DockStyle.Fill;
            this.m_labelTaskName.EdgeRadius = 1;
            this.m_labelTaskName.Enabled = false;
            this.m_labelTaskName.ImagePosition = new System.Drawing.Point(0, 0);
            this.m_labelTaskName.ImageSize = new System.Drawing.Point(0, 0);
            this.m_labelTaskName.LoadImage = null;
            this.m_labelTaskName.Location = new System.Drawing.Point(163, 11);
            this.m_labelTaskName.MainFont = new System.Drawing.Font("맑은 고딕", 11F, System.Drawing.FontStyle.Bold);
            this.m_labelTaskName.MainFontColor = System.Drawing.Color.Black;
            this.m_labelTaskName.Margin = new System.Windows.Forms.Padding(1);
            this.m_labelTaskName.Name = "m_labelTaskName";
            this.m_labelTaskName.Size = new System.Drawing.Size(389, 50);
            this.m_labelTaskName.SubFont = new System.Drawing.Font("맑은 고딕", 10F);
            this.m_labelTaskName.SubFontColor = System.Drawing.Color.Black;
            this.m_labelTaskName.SubText = "";
            this.m_labelTaskName.TabIndex = 1478;
            this.m_labelTaskName.TextAlignMain = Sys3Controls.EN_TEXTALIGN.MIDDLE_CENTER;
            this.m_labelTaskName.TextAlignSub = Sys3Controls.EN_TEXTALIGN.MIDDLE_CENTER;
            this.m_labelTaskName.TextAlignUnit = Sys3Controls.EN_TEXTALIGN.MIDDLE_CENTER;
            this.m_labelTaskName.ThemeIndex = 0;
            this.m_labelTaskName.UnitAreaRate = 40;
            this.m_labelTaskName.UnitFont = new System.Drawing.Font("맑은 고딕", 10F, System.Drawing.FontStyle.Bold);
            this.m_labelTaskName.UnitFontColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.m_labelTaskName.UnitPositionVertical = false;
            this.m_labelTaskName.UnitText = "";
            this.m_labelTaskName.UseBorder = true;
            this.m_labelTaskName.UseEdgeRadius = false;
            this.m_labelTaskName.UseImage = false;
            this.m_labelTaskName.UseSubFont = false;
            this.m_labelTaskName.UseUnitFont = false;
            // 
            // sys3Separator1
            // 
            this.sys3Separator1.BackColor = System.Drawing.Color.WhiteSmoke;
            this.tableLayoutPanel2.SetColumnSpan(this.sys3Separator1, 5);
            this.sys3Separator1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.sys3Separator1.IsVertical = false;
            this.sys3Separator1.Location = new System.Drawing.Point(10, 65);
            this.sys3Separator1.Name = "sys3Separator1";
            this.sys3Separator1.Size = new System.Drawing.Size(1125, 46);
            this.sys3Separator1.TabIndex = 20253;
            this.sys3Separator1.Text = "sys3Separator1";
            this.sys3Separator1.Thickness = 1;
            // 
            // m_labelPortName
            // 
            this.m_labelPortName.BackGroundColor = System.Drawing.Color.White;
            this.m_labelPortName.BorderStroke = 2;
            this.m_labelPortName.BorderStyle = System.Windows.Forms.ButtonBorderStyle.Solid;
            this.m_labelPortName.Description = "";
            this.m_labelPortName.DisabledColor = System.Drawing.Color.LightGray;
            this.m_labelPortName.Dock = System.Windows.Forms.DockStyle.Fill;
            this.m_labelPortName.EdgeRadius = 1;
            this.m_labelPortName.Enabled = false;
            this.m_labelPortName.ImagePosition = new System.Drawing.Point(0, 0);
            this.m_labelPortName.ImageSize = new System.Drawing.Point(0, 0);
            this.m_labelPortName.LoadImage = null;
            this.m_labelPortName.Location = new System.Drawing.Point(748, 11);
            this.m_labelPortName.MainFont = new System.Drawing.Font("맑은 고딕", 11F, System.Drawing.FontStyle.Bold);
            this.m_labelPortName.MainFontColor = System.Drawing.Color.Black;
            this.m_labelPortName.Margin = new System.Windows.Forms.Padding(1);
            this.m_labelPortName.Name = "m_labelPortName";
            this.m_labelPortName.Size = new System.Drawing.Size(389, 50);
            this.m_labelPortName.SubFont = new System.Drawing.Font("맑은 고딕", 10F);
            this.m_labelPortName.SubFontColor = System.Drawing.Color.Black;
            this.m_labelPortName.SubText = "";
            this.m_labelPortName.TabIndex = 1478;
            this.m_labelPortName.TextAlignMain = Sys3Controls.EN_TEXTALIGN.MIDDLE_CENTER;
            this.m_labelPortName.TextAlignSub = Sys3Controls.EN_TEXTALIGN.MIDDLE_CENTER;
            this.m_labelPortName.TextAlignUnit = Sys3Controls.EN_TEXTALIGN.MIDDLE_CENTER;
            this.m_labelPortName.ThemeIndex = 0;
            this.m_labelPortName.UnitAreaRate = 40;
            this.m_labelPortName.UnitFont = new System.Drawing.Font("맑은 고딕", 10F, System.Drawing.FontStyle.Bold);
            this.m_labelPortName.UnitFontColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.m_labelPortName.UnitPositionVertical = false;
            this.m_labelPortName.UnitText = "";
            this.m_labelPortName.UseBorder = true;
            this.m_labelPortName.UseEdgeRadius = false;
            this.m_labelPortName.UseImage = false;
            this.m_labelPortName.UseSubFont = false;
            this.m_labelPortName.UseUnitFont = false;
            // 
            // m_labelStatus
            // 
            this.m_labelStatus.BackGroundColor = System.Drawing.Color.White;
            this.m_labelStatus.BorderStroke = 2;
            this.m_labelStatus.BorderStyle = System.Windows.Forms.ButtonBorderStyle.Solid;
            this.m_labelStatus.Description = "";
            this.m_labelStatus.DisabledColor = System.Drawing.Color.LightGray;
            this.m_labelStatus.Dock = System.Windows.Forms.DockStyle.Fill;
            this.m_labelStatus.EdgeRadius = 1;
            this.m_labelStatus.Enabled = false;
            this.m_labelStatus.ImagePosition = new System.Drawing.Point(0, 0);
            this.m_labelStatus.ImageSize = new System.Drawing.Point(0, 0);
            this.m_labelStatus.LoadImage = null;
            this.m_labelStatus.Location = new System.Drawing.Point(163, 115);
            this.m_labelStatus.MainFont = new System.Drawing.Font("맑은 고딕", 11F, System.Drawing.FontStyle.Bold);
            this.m_labelStatus.MainFontColor = System.Drawing.Color.Black;
            this.m_labelStatus.Margin = new System.Windows.Forms.Padding(1);
            this.m_labelStatus.Name = "m_labelStatus";
            this.m_labelStatus.Size = new System.Drawing.Size(389, 50);
            this.m_labelStatus.SubFont = new System.Drawing.Font("맑은 고딕", 10F);
            this.m_labelStatus.SubFontColor = System.Drawing.Color.Black;
            this.m_labelStatus.SubText = "";
            this.m_labelStatus.TabIndex = 1478;
            this.m_labelStatus.TextAlignMain = Sys3Controls.EN_TEXTALIGN.MIDDLE_CENTER;
            this.m_labelStatus.TextAlignSub = Sys3Controls.EN_TEXTALIGN.MIDDLE_CENTER;
            this.m_labelStatus.TextAlignUnit = Sys3Controls.EN_TEXTALIGN.MIDDLE_CENTER;
            this.m_labelStatus.ThemeIndex = 0;
            this.m_labelStatus.UnitAreaRate = 40;
            this.m_labelStatus.UnitFont = new System.Drawing.Font("맑은 고딕", 10F, System.Drawing.FontStyle.Bold);
            this.m_labelStatus.UnitFontColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.m_labelStatus.UnitPositionVertical = false;
            this.m_labelStatus.UnitText = "";
            this.m_labelStatus.UseBorder = true;
            this.m_labelStatus.UseEdgeRadius = false;
            this.m_labelStatus.UseImage = false;
            this.m_labelStatus.UseSubFont = false;
            this.m_labelStatus.UseUnitFont = false;
            this.m_labelStatus.Click += new System.EventHandler(this.Click_LabelItem);
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 2;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 34.86159F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 65.1384F));
            this.tableLayoutPanel1.Controls.Add(this.sys3GroupBox2, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.sys3GroupBox1, 1, 0);
            this.tableLayoutPanel1.Controls.Add(this.sys3GroupBox3, 0, 1);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel1.Margin = new System.Windows.Forms.Padding(0);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 2;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 55.11163F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 44.88837F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(1156, 851);
            this.tableLayoutPanel1.TabIndex = 20254;
            // 
            // tableLayoutPanel2
            // 
            this.tableLayoutPanel2.BackColor = System.Drawing.Color.WhiteSmoke;
            this.tableLayoutPanel2.ColumnCount = 7;
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 0.6815704F));
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 13.53599F));
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 34.07851F));
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 3.407852F));
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 13.53599F));
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 34.07852F));
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 0.6815703F));
            this.tableLayoutPanel2.Controls.Add(this.m_lbTaskName, 1, 1);
            this.tableLayoutPanel2.Controls.Add(this.m_labelTaskName, 2, 1);
            this.tableLayoutPanel2.Controls.Add(this.m_labelStatus, 2, 3);
            this.tableLayoutPanel2.Controls.Add(this.m_labelPortName, 5, 1);
            this.tableLayoutPanel2.Controls.Add(this.m_lblStatus, 1, 3);
            this.tableLayoutPanel2.Controls.Add(this.m_lblPortName, 4, 1);
            this.tableLayoutPanel2.Controls.Add(this.sys3Separator1, 1, 2);
            this.tableLayoutPanel2.Controls.Add(this.tableLayoutPanel3, 3, 4);
            this.tableLayoutPanel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel2.Location = new System.Drawing.Point(3, 44);
            this.tableLayoutPanel2.Margin = new System.Windows.Forms.Padding(0);
            this.tableLayoutPanel2.Name = "tableLayoutPanel2";
            this.tableLayoutPanel2.RowCount = 6;
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 3.125F));
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 15.625F));
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 15.625F));
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 15.625F));
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 46.875F));
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 3.125F));
            this.tableLayoutPanel2.Size = new System.Drawing.Size(1150, 336);
            this.tableLayoutPanel2.TabIndex = 0;
            // 
            // tableLayoutPanel3
            // 
            this.tableLayoutPanel3.ColumnCount = 5;
            this.tableLayoutPanel2.SetColumnSpan(this.tableLayoutPanel3, 3);
            this.tableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 23.80952F));
            this.tableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 23.80952F));
            this.tableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 4.761905F));
            this.tableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 23.80952F));
            this.tableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 23.80952F));
            this.tableLayoutPanel3.Controls.Add(this.m_btnEnable, 0, 1);
            this.tableLayoutPanel3.Controls.Add(this.sys3button2, 4, 1);
            this.tableLayoutPanel3.Controls.Add(this.m_btnDisable, 1, 1);
            this.tableLayoutPanel3.Controls.Add(this.m_btnAdd, 3, 1);
            this.tableLayoutPanel3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel3.Location = new System.Drawing.Point(553, 166);
            this.tableLayoutPanel3.Margin = new System.Windows.Forms.Padding(0);
            this.tableLayoutPanel3.Name = "tableLayoutPanel3";
            this.tableLayoutPanel3.RowCount = 2;
            this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 40.76433F));
            this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 59.23567F));
            this.tableLayoutPanel3.Size = new System.Drawing.Size(585, 157);
            this.tableLayoutPanel3.TabIndex = 20254;
            // 
            // Config_Port
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.Controls.Add(this.tableLayoutPanel1);
            this.Margin = new System.Windows.Forms.Padding(0);
            this.Name = "Config_Port";
            this.Size = new System.Drawing.Size(1156, 851);
            this.sys3GroupBox2.ResumeLayout(false);
            this.sys3GroupBox1.ResumeLayout(false);
            this.sys3GroupBox3.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.m_dgViewPortList)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.m_dgViewTask)).EndInit();
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel2.ResumeLayout(false);
            this.tableLayoutPanel3.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

		private Sys3Controls.Sys3DoubleBufferedDataGridView m_dgViewPortList;
		private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn2;
		private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn1;
		private Sys3Controls.Sys3DoubleBufferedDataGridView m_dgViewTask;
		private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn3;
		private System.Windows.Forms.DataGridViewTextBoxColumn ENABLE;
		private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn4;
		private System.Windows.Forms.DataGridViewTextBoxColumn STATUS;
		private Sys3Controls.Sys3button m_btnDisable;
		private Sys3Controls.Sys3button m_btnEnable;
		private Sys3Controls.Sys3button sys3button2;
		private Sys3Controls.Sys3button m_btnAdd;
		private Sys3Controls.Sys3Label m_labelTaskName;
		private SUserControls.Sys3Separator sys3Separator1;
		private Sys3Controls.Sys3Label m_labelPortName;
		private Sys3Controls.Sys3Label m_labelStatus;
        private Sys3Controls.Sys3GroupBoxContainer sys3GroupBox2;
        private Sys3Controls.Sys3GroupBoxContainer sys3GroupBox1;
        private Sys3Controls.Sys3GroupBoxContainer sys3GroupBox3;
        private Sys3Controls.Sys3Label m_lbTaskName;
        private Sys3Controls.Sys3Label m_lblPortName;
        private Sys3Controls.Sys3Label m_lblStatus;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel2;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel3;

	}
}
