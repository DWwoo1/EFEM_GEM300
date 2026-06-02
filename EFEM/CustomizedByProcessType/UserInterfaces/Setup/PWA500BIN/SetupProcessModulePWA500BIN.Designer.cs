
namespace EFEM.CustomizedByProcessType.UserInterface.Setup.PWA500BIN
{
    partial class SetupProcessModulePWA500BIN
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
        /// 이 메서드의 내용을 코드 편집기로 수정하지 마세요.
        /// </summary>
        private void InitializeComponent()
        {
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle3 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle4 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle5 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle6 = new System.Windows.Forms.DataGridViewCellStyle();
            this.tableLayoutPanel5 = new System.Windows.Forms.TableLayoutPanel();
            this.pnPWA500BINOptions = new System.Windows.Forms.Panel();
            this.PASSWORD = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ID = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.gvServiceStatus = new Sys3Controls.Sys3DoubleBufferedDataGridView();
            this.dataGridViewTextBoxColumn2 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.sys3Label7 = new Sys3Controls.Sys3Label();
            this.gvClientStatus = new Sys3Controls.Sys3DoubleBufferedDataGridView();
            this.tableLayoutPanel4 = new System.Windows.Forms.TableLayoutPanel();
            this.sys3Label1 = new Sys3Controls.Sys3Label();
            this.sys3Separator1 = new SUserControls.Sys3Separator();
            this.sys3GroupBoxContainer1 = new Sys3Controls.Sys3GroupBoxContainer();
            this.panel1 = new System.Windows.Forms.Panel();
            this.btnUndoParameter = new Sys3Controls.Sys3button();
            this.btnSaveParameter = new Sys3Controls.Sys3button();
            this.tableLayoutPanel3 = new System.Windows.Forms.TableLayoutPanel();
            this.pnLoadPort2 = new System.Windows.Forms.Panel();
            this.pnLoadPort1 = new System.Windows.Forms.Panel();
            this.pnLoadPort3 = new System.Windows.Forms.Panel();
            this.pnLoadPort4 = new System.Windows.Forms.Panel();
            this.pnLoadPort5 = new System.Windows.Forms.Panel();
            this.pnLoadPort6 = new System.Windows.Forms.Panel();
            this.tableLayoutPanel2 = new System.Windows.Forms.TableLayoutPanel();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.tableLayoutPanel5.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gvServiceStatus)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gvClientStatus)).BeginInit();
            this.tableLayoutPanel4.SuspendLayout();
            this.sys3GroupBoxContainer1.SuspendLayout();
            this.panel1.SuspendLayout();
            this.tableLayoutPanel3.SuspendLayout();
            this.tableLayoutPanel2.SuspendLayout();
            this.tableLayoutPanel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // tableLayoutPanel5
            // 
            this.tableLayoutPanel5.ColumnCount = 1;
            this.tableLayoutPanel5.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel5.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel5.Controls.Add(this.pnPWA500BINOptions, 0, 0);
            this.tableLayoutPanel5.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel5.Location = new System.Drawing.Point(570, 0);
            this.tableLayoutPanel5.Margin = new System.Windows.Forms.Padding(0);
            this.tableLayoutPanel5.Name = "tableLayoutPanel5";
            this.tableLayoutPanel5.RowCount = 1;
            this.tableLayoutPanel5.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel5.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel5.Size = new System.Drawing.Size(570, 386);
            this.tableLayoutPanel5.TabIndex = 21133;
            // 
            // pnPWA500BINOptions
            // 
            this.pnPWA500BINOptions.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnPWA500BINOptions.Location = new System.Drawing.Point(0, 0);
            this.pnPWA500BINOptions.Margin = new System.Windows.Forms.Padding(0);
            this.pnPWA500BINOptions.Name = "pnPWA500BINOptions";
            this.pnPWA500BINOptions.Size = new System.Drawing.Size(570, 386);
            this.pnPWA500BINOptions.TabIndex = 21145;
            // 
            // PASSWORD
            // 
            this.PASSWORD.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.PASSWORD.HeaderText = "SERVICE NAME";
            this.PASSWORD.MaxInputLength = 20;
            this.PASSWORD.Name = "PASSWORD";
            this.PASSWORD.ReadOnly = true;
            this.PASSWORD.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.PASSWORD.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // ID
            // 
            this.ID.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.ColumnHeader;
            this.ID.Frozen = true;
            this.ID.HeaderText = "STATUS";
            this.ID.MaxInputLength = 20;
            this.ID.Name = "ID";
            this.ID.ReadOnly = true;
            this.ID.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.ID.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.ID.Width = 70;
            // 
            // gvServiceStatus
            // 
            this.gvServiceStatus.AllowUserToAddRows = false;
            this.gvServiceStatus.AllowUserToDeleteRows = false;
            this.gvServiceStatus.AllowUserToResizeColumns = false;
            this.gvServiceStatus.AllowUserToResizeRows = false;
            this.gvServiceStatus.BackgroundColor = System.Drawing.Color.White;
            this.gvServiceStatus.CellBorderStyle = System.Windows.Forms.DataGridViewCellBorderStyle.Raised;
            this.gvServiceStatus.ColumnHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.Single;
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle1.BackColor = System.Drawing.Color.LightGray;
            dataGridViewCellStyle1.Font = new System.Drawing.Font("맑은 고딕", 11F, System.Drawing.FontStyle.Bold);
            dataGridViewCellStyle1.ForeColor = System.Drawing.Color.Black;
            dataGridViewCellStyle1.SelectionBackColor = System.Drawing.Color.White;
            dataGridViewCellStyle1.SelectionForeColor = System.Drawing.Color.Black;
            dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.gvServiceStatus.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
            this.gvServiceStatus.ColumnHeadersHeight = 30;
            this.gvServiceStatus.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            this.gvServiceStatus.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.ID,
            this.PASSWORD});
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle2.BackColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle2.Font = new System.Drawing.Font("맑은 고딕", 10F, System.Drawing.FontStyle.Bold);
            dataGridViewCellStyle2.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle2.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(120)))), ((int)(((byte)(130)))), ((int)(((byte)(150)))));
            dataGridViewCellStyle2.SelectionForeColor = System.Drawing.Color.White;
            dataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.gvServiceStatus.DefaultCellStyle = dataGridViewCellStyle2;
            this.gvServiceStatus.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gvServiceStatus.EnableHeadersVisualStyles = false;
            this.gvServiceStatus.GridColor = System.Drawing.Color.FromArgb(((int)(((byte)(120)))), ((int)(((byte)(130)))), ((int)(((byte)(150)))));
            this.gvServiceStatus.Location = new System.Drawing.Point(0, 31);
            this.gvServiceStatus.Margin = new System.Windows.Forms.Padding(0, 1, 0, 1);
            this.gvServiceStatus.MultiSelect = false;
            this.gvServiceStatus.Name = "gvServiceStatus";
            this.gvServiceStatus.ReadOnly = true;
            this.gvServiceStatus.RowHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.None;
            dataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle3.BackColor = System.Drawing.Color.White;
            dataGridViewCellStyle3.Font = new System.Drawing.Font("맑은 고딕", 11F);
            dataGridViewCellStyle3.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle3.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(120)))), ((int)(((byte)(130)))), ((int)(((byte)(150)))));
            dataGridViewCellStyle3.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle3.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.gvServiceStatus.RowHeadersDefaultCellStyle = dataGridViewCellStyle3;
            this.gvServiceStatus.RowHeadersVisible = false;
            this.gvServiceStatus.RowHeadersWidthSizeMode = System.Windows.Forms.DataGridViewRowHeadersWidthSizeMode.DisableResizing;
            this.gvServiceStatus.RowTemplate.Height = 23;
            this.gvServiceStatus.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.gvServiceStatus.Size = new System.Drawing.Size(280, 317);
            this.gvServiceStatus.TabIndex = 21128;
            // 
            // dataGridViewTextBoxColumn2
            // 
            this.dataGridViewTextBoxColumn2.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.dataGridViewTextBoxColumn2.HeaderText = "CLIENT NAME";
            this.dataGridViewTextBoxColumn2.MaxInputLength = 20;
            this.dataGridViewTextBoxColumn2.Name = "dataGridViewTextBoxColumn2";
            this.dataGridViewTextBoxColumn2.ReadOnly = true;
            this.dataGridViewTextBoxColumn2.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.dataGridViewTextBoxColumn2.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // dataGridViewTextBoxColumn1
            // 
            this.dataGridViewTextBoxColumn1.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.ColumnHeader;
            this.dataGridViewTextBoxColumn1.Frozen = true;
            this.dataGridViewTextBoxColumn1.HeaderText = "STATUS";
            this.dataGridViewTextBoxColumn1.MaxInputLength = 20;
            this.dataGridViewTextBoxColumn1.Name = "dataGridViewTextBoxColumn1";
            this.dataGridViewTextBoxColumn1.ReadOnly = true;
            this.dataGridViewTextBoxColumn1.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.dataGridViewTextBoxColumn1.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.dataGridViewTextBoxColumn1.Width = 70;
            // 
            // sys3Label7
            // 
            this.sys3Label7.BackGroundColor = System.Drawing.Color.DarkGray;
            this.sys3Label7.BorderStroke = 2;
            this.sys3Label7.BorderStyle = System.Windows.Forms.ButtonBorderStyle.Solid;
            this.sys3Label7.Description = "";
            this.sys3Label7.DisabledColor = System.Drawing.Color.DarkGray;
            this.sys3Label7.Dock = System.Windows.Forms.DockStyle.Fill;
            this.sys3Label7.EdgeRadius = 1;
            this.sys3Label7.ImagePosition = new System.Drawing.Point(0, 0);
            this.sys3Label7.ImageSize = new System.Drawing.Point(0, 0);
            this.sys3Label7.LoadImage = null;
            this.sys3Label7.Location = new System.Drawing.Point(1, 1);
            this.sys3Label7.MainFont = new System.Drawing.Font("맑은 고딕", 11F, System.Drawing.FontStyle.Bold);
            this.sys3Label7.MainFontColor = System.Drawing.Color.Black;
            this.sys3Label7.Margin = new System.Windows.Forms.Padding(1, 1, 0, 0);
            this.sys3Label7.Name = "sys3Label7";
            this.sys3Label7.Size = new System.Drawing.Size(279, 29);
            this.sys3Label7.SubFont = new System.Drawing.Font("맑은 고딕", 10F, System.Drawing.FontStyle.Bold);
            this.sys3Label7.SubFontColor = System.Drawing.Color.DarkRed;
            this.sys3Label7.SubText = "";
            this.sys3Label7.TabIndex = 21129;
            this.sys3Label7.Tag = "";
            this.sys3Label7.Text = "SERVICE STATUS";
            this.sys3Label7.TextAlignMain = Sys3Controls.EN_TEXTALIGN.MIDDLE_CENTER;
            this.sys3Label7.TextAlignSub = Sys3Controls.EN_TEXTALIGN.TOP_LEFT;
            this.sys3Label7.TextAlignUnit = Sys3Controls.EN_TEXTALIGN.MIDDLE_CENTER;
            this.sys3Label7.ThemeIndex = 0;
            this.sys3Label7.UnitAreaRate = 40;
            this.sys3Label7.UnitFont = new System.Drawing.Font("맑은 고딕", 10F, System.Drawing.FontStyle.Bold);
            this.sys3Label7.UnitFontColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.sys3Label7.UnitPositionVertical = false;
            this.sys3Label7.UnitText = "";
            this.sys3Label7.UseBorder = true;
            this.sys3Label7.UseEdgeRadius = false;
            this.sys3Label7.UseImage = false;
            this.sys3Label7.UseSubFont = true;
            this.sys3Label7.UseUnitFont = false;
            // 
            // gvClientStatus
            // 
            this.gvClientStatus.AllowUserToAddRows = false;
            this.gvClientStatus.AllowUserToDeleteRows = false;
            this.gvClientStatus.AllowUserToResizeColumns = false;
            this.gvClientStatus.AllowUserToResizeRows = false;
            this.gvClientStatus.BackgroundColor = System.Drawing.Color.White;
            this.gvClientStatus.CellBorderStyle = System.Windows.Forms.DataGridViewCellBorderStyle.Raised;
            this.gvClientStatus.ColumnHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.Single;
            dataGridViewCellStyle4.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle4.BackColor = System.Drawing.Color.LightGray;
            dataGridViewCellStyle4.Font = new System.Drawing.Font("맑은 고딕", 11F, System.Drawing.FontStyle.Bold);
            dataGridViewCellStyle4.ForeColor = System.Drawing.Color.Black;
            dataGridViewCellStyle4.SelectionBackColor = System.Drawing.Color.White;
            dataGridViewCellStyle4.SelectionForeColor = System.Drawing.Color.Black;
            dataGridViewCellStyle4.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.gvClientStatus.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle4;
            this.gvClientStatus.ColumnHeadersHeight = 30;
            this.gvClientStatus.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            this.gvClientStatus.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.dataGridViewTextBoxColumn1,
            this.dataGridViewTextBoxColumn2});
            dataGridViewCellStyle5.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle5.BackColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle5.Font = new System.Drawing.Font("맑은 고딕", 10F, System.Drawing.FontStyle.Bold);
            dataGridViewCellStyle5.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle5.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(120)))), ((int)(((byte)(130)))), ((int)(((byte)(150)))));
            dataGridViewCellStyle5.SelectionForeColor = System.Drawing.Color.White;
            dataGridViewCellStyle5.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.gvClientStatus.DefaultCellStyle = dataGridViewCellStyle5;
            this.gvClientStatus.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gvClientStatus.EnableHeadersVisualStyles = false;
            this.gvClientStatus.GridColor = System.Drawing.Color.FromArgb(((int)(((byte)(120)))), ((int)(((byte)(130)))), ((int)(((byte)(150)))));
            this.gvClientStatus.Location = new System.Drawing.Point(280, 31);
            this.gvClientStatus.Margin = new System.Windows.Forms.Padding(0, 1, 0, 1);
            this.gvClientStatus.MultiSelect = false;
            this.gvClientStatus.Name = "gvClientStatus";
            this.gvClientStatus.ReadOnly = true;
            this.gvClientStatus.RowHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.None;
            dataGridViewCellStyle6.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle6.BackColor = System.Drawing.Color.White;
            dataGridViewCellStyle6.Font = new System.Drawing.Font("맑은 고딕", 11F);
            dataGridViewCellStyle6.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle6.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(120)))), ((int)(((byte)(130)))), ((int)(((byte)(150)))));
            dataGridViewCellStyle6.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle6.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.gvClientStatus.RowHeadersDefaultCellStyle = dataGridViewCellStyle6;
            this.gvClientStatus.RowHeadersVisible = false;
            this.gvClientStatus.RowHeadersWidthSizeMode = System.Windows.Forms.DataGridViewRowHeadersWidthSizeMode.DisableResizing;
            this.gvClientStatus.RowTemplate.Height = 23;
            this.gvClientStatus.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.gvClientStatus.Size = new System.Drawing.Size(280, 317);
            this.gvClientStatus.TabIndex = 21130;
            // 
            // tableLayoutPanel4
            // 
            this.tableLayoutPanel4.ColumnCount = 2;
            this.tableLayoutPanel4.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel4.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel4.Controls.Add(this.sys3Label7, 0, 0);
            this.tableLayoutPanel4.Controls.Add(this.gvClientStatus, 1, 1);
            this.tableLayoutPanel4.Controls.Add(this.gvServiceStatus, 0, 1);
            this.tableLayoutPanel4.Controls.Add(this.sys3Label1, 1, 0);
            this.tableLayoutPanel4.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel4.Location = new System.Drawing.Point(5, 32);
            this.tableLayoutPanel4.Name = "tableLayoutPanel4";
            this.tableLayoutPanel4.RowCount = 2;
            this.tableLayoutPanel4.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 30F));
            this.tableLayoutPanel4.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel4.Size = new System.Drawing.Size(560, 349);
            this.tableLayoutPanel4.TabIndex = 21132;
            // 
            // sys3Label1
            // 
            this.sys3Label1.BackGroundColor = System.Drawing.Color.DarkGray;
            this.sys3Label1.BorderStroke = 2;
            this.sys3Label1.BorderStyle = System.Windows.Forms.ButtonBorderStyle.Solid;
            this.sys3Label1.Description = "";
            this.sys3Label1.DisabledColor = System.Drawing.Color.DarkGray;
            this.sys3Label1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.sys3Label1.EdgeRadius = 1;
            this.sys3Label1.ImagePosition = new System.Drawing.Point(0, 0);
            this.sys3Label1.ImageSize = new System.Drawing.Point(0, 0);
            this.sys3Label1.LoadImage = null;
            this.sys3Label1.Location = new System.Drawing.Point(281, 1);
            this.sys3Label1.MainFont = new System.Drawing.Font("맑은 고딕", 11F, System.Drawing.FontStyle.Bold);
            this.sys3Label1.MainFontColor = System.Drawing.Color.Black;
            this.sys3Label1.Margin = new System.Windows.Forms.Padding(1, 1, 0, 0);
            this.sys3Label1.Name = "sys3Label1";
            this.sys3Label1.Size = new System.Drawing.Size(279, 29);
            this.sys3Label1.SubFont = new System.Drawing.Font("맑은 고딕", 10F, System.Drawing.FontStyle.Bold);
            this.sys3Label1.SubFontColor = System.Drawing.Color.DarkRed;
            this.sys3Label1.SubText = "";
            this.sys3Label1.TabIndex = 21131;
            this.sys3Label1.Tag = "";
            this.sys3Label1.Text = "CLIENT STATUS";
            this.sys3Label1.TextAlignMain = Sys3Controls.EN_TEXTALIGN.MIDDLE_CENTER;
            this.sys3Label1.TextAlignSub = Sys3Controls.EN_TEXTALIGN.TOP_LEFT;
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
            this.sys3Label1.UseSubFont = true;
            this.sys3Label1.UseUnitFont = false;
            // 
            // sys3Separator1
            // 
            this.sys3Separator1.Dock = System.Windows.Forms.DockStyle.Top;
            this.sys3Separator1.IsVertical = false;
            this.sys3Separator1.Location = new System.Drawing.Point(3, 54);
            this.sys3Separator1.Name = "sys3Separator1";
            this.sys3Separator1.Size = new System.Drawing.Size(1134, 5);
            this.sys3Separator1.TabIndex = 21140;
            this.sys3Separator1.Text = "sys3Separator1";
            this.sys3Separator1.Thickness = 1;
            // 
            // sys3GroupBoxContainer1
            // 
            this.sys3GroupBoxContainer1.BackGroundColor = System.Drawing.Color.WhiteSmoke;
            this.sys3GroupBoxContainer1.Controls.Add(this.tableLayoutPanel4);
            this.sys3GroupBoxContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.sys3GroupBoxContainer1.EdgeBorderStroke = 2;
            this.sys3GroupBoxContainer1.EdgeRadius = 2;
            this.sys3GroupBoxContainer1.LabelFont = new System.Drawing.Font("맑은 고딕", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.sys3GroupBoxContainer1.LabelGradientColorFirst = System.Drawing.Color.DarkGray;
            this.sys3GroupBoxContainer1.LabelGradientColorSecond = System.Drawing.Color.WhiteSmoke;
            this.sys3GroupBoxContainer1.LabelHeight = 30;
            this.sys3GroupBoxContainer1.LabelTextColor = System.Drawing.Color.Black;
            this.sys3GroupBoxContainer1.Location = new System.Drawing.Point(0, 0);
            this.sys3GroupBoxContainer1.Margin = new System.Windows.Forms.Padding(0);
            this.sys3GroupBoxContainer1.Name = "sys3GroupBoxContainer1";
            this.sys3GroupBoxContainer1.Padding = new System.Windows.Forms.Padding(5, 18, 5, 5);
            this.sys3GroupBoxContainer1.Size = new System.Drawing.Size(570, 386);
            this.sys3GroupBoxContainer1.TabIndex = 21132;
            this.sys3GroupBoxContainer1.TabStop = false;
            this.sys3GroupBoxContainer1.Text = "PROCESS MODULE INFORMATION";
            this.sys3GroupBoxContainer1.TextAlign = Sys3Controls.EN_TEXTALIGN.MIDDLE_CENTER;
            this.sys3GroupBoxContainer1.ThemeIndex = 0;
            this.sys3GroupBoxContainer1.UseLabelBorder = true;
            this.sys3GroupBoxContainer1.UseTitle = true;
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.btnUndoParameter);
            this.panel1.Controls.Add(this.btnSaveParameter);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Margin = new System.Windows.Forms.Padding(0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(1140, 51);
            this.panel1.TabIndex = 21147;
            // 
            // btnUndoParameter
            // 
            this.btnUndoParameter.BorderWidth = 2;
            this.btnUndoParameter.ButtonClicked = false;
            this.btnUndoParameter.ClickedEmphasizeTextColor = System.Drawing.Color.White;
            this.btnUndoParameter.CustomClickedGradientFirstColor = System.Drawing.Color.BlanchedAlmond;
            this.btnUndoParameter.CustomClickedGradientSecondColor = System.Drawing.Color.Gold;
            this.btnUndoParameter.Description = "";
            this.btnUndoParameter.DisabledColor = System.Drawing.Color.DarkGray;
            this.btnUndoParameter.Dock = System.Windows.Forms.DockStyle.Right;
            this.btnUndoParameter.EdgeRadius = 5;
            this.btnUndoParameter.GradientAngle = 60F;
            this.btnUndoParameter.GradientFirstColor = System.Drawing.Color.DeepSkyBlue;
            this.btnUndoParameter.GradientSecondColor = System.Drawing.Color.SteelBlue;
            this.btnUndoParameter.HoverEmphasizeCustomColor = System.Drawing.Color.Firebrick;
            this.btnUndoParameter.ImagePosition = new System.Drawing.Point(8, 7);
            this.btnUndoParameter.ImageSize = new System.Drawing.Point(35, 35);
            this.btnUndoParameter.LoadImage = global::FrameOfSystem3.Properties.Resources.undo_52px;
            this.btnUndoParameter.Location = new System.Drawing.Point(1042, 0);
            this.btnUndoParameter.MainFont = new System.Drawing.Font("맑은 고딕", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.btnUndoParameter.MainFontColor = System.Drawing.Color.White;
            this.btnUndoParameter.Margin = new System.Windows.Forms.Padding(1, 1, 0, 0);
            this.btnUndoParameter.Name = "btnUndoParameter";
            this.btnUndoParameter.Size = new System.Drawing.Size(49, 51);
            this.btnUndoParameter.SubFont = new System.Drawing.Font("맑은 고딕", 10F);
            this.btnUndoParameter.SubFontColor = System.Drawing.Color.Black;
            this.btnUndoParameter.SubText = "";
            this.btnUndoParameter.TabIndex = 21138;
            this.btnUndoParameter.Tag = "";
            this.btnUndoParameter.TextAlignMain = Sys3Controls.EN_TEXTALIGN.MIDDLE_CENTER;
            this.btnUndoParameter.TextAlignSub = Sys3Controls.EN_TEXTALIGN.MIDDLE_CENTER;
            this.btnUndoParameter.ThemeIndex = 0;
            this.btnUndoParameter.UseBorder = true;
            this.btnUndoParameter.UseClickedEmphasizeTextColor = false;
            this.btnUndoParameter.UseCustomizeClickedColor = true;
            this.btnUndoParameter.UseEdge = true;
            this.btnUndoParameter.UseHoverEmphasizeCustomColor = true;
            this.btnUndoParameter.UseImage = true;
            this.btnUndoParameter.UserHoverEmpahsize = true;
            this.btnUndoParameter.UseSubFont = false;
            this.btnUndoParameter.Click += new System.EventHandler(this.BtnUndoParameterClicked);
            // 
            // btnSaveParameter
            // 
            this.btnSaveParameter.BorderWidth = 2;
            this.btnSaveParameter.ButtonClicked = false;
            this.btnSaveParameter.ClickedEmphasizeTextColor = System.Drawing.Color.White;
            this.btnSaveParameter.CustomClickedGradientFirstColor = System.Drawing.Color.BlanchedAlmond;
            this.btnSaveParameter.CustomClickedGradientSecondColor = System.Drawing.Color.Gold;
            this.btnSaveParameter.Description = "";
            this.btnSaveParameter.DisabledColor = System.Drawing.Color.DarkGray;
            this.btnSaveParameter.Dock = System.Windows.Forms.DockStyle.Right;
            this.btnSaveParameter.EdgeRadius = 5;
            this.btnSaveParameter.GradientAngle = 60F;
            this.btnSaveParameter.GradientFirstColor = System.Drawing.Color.DeepSkyBlue;
            this.btnSaveParameter.GradientSecondColor = System.Drawing.Color.SteelBlue;
            this.btnSaveParameter.HoverEmphasizeCustomColor = System.Drawing.Color.Firebrick;
            this.btnSaveParameter.ImagePosition = new System.Drawing.Point(8, 7);
            this.btnSaveParameter.ImageSize = new System.Drawing.Point(35, 35);
            this.btnSaveParameter.LoadImage = global::FrameOfSystem3.Properties.Resources.save_52px;
            this.btnSaveParameter.Location = new System.Drawing.Point(1091, 0);
            this.btnSaveParameter.MainFont = new System.Drawing.Font("맑은 고딕", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.btnSaveParameter.MainFontColor = System.Drawing.Color.White;
            this.btnSaveParameter.Margin = new System.Windows.Forms.Padding(1, 1, 0, 0);
            this.btnSaveParameter.Name = "btnSaveParameter";
            this.btnSaveParameter.Size = new System.Drawing.Size(49, 51);
            this.btnSaveParameter.SubFont = new System.Drawing.Font("맑은 고딕", 10F);
            this.btnSaveParameter.SubFontColor = System.Drawing.Color.Black;
            this.btnSaveParameter.SubText = "";
            this.btnSaveParameter.TabIndex = 21139;
            this.btnSaveParameter.Tag = "";
            this.btnSaveParameter.TextAlignMain = Sys3Controls.EN_TEXTALIGN.MIDDLE_CENTER;
            this.btnSaveParameter.TextAlignSub = Sys3Controls.EN_TEXTALIGN.MIDDLE_CENTER;
            this.btnSaveParameter.ThemeIndex = 0;
            this.btnSaveParameter.UseBorder = true;
            this.btnSaveParameter.UseClickedEmphasizeTextColor = false;
            this.btnSaveParameter.UseCustomizeClickedColor = true;
            this.btnSaveParameter.UseEdge = true;
            this.btnSaveParameter.UseHoverEmphasizeCustomColor = true;
            this.btnSaveParameter.UseImage = true;
            this.btnSaveParameter.UserHoverEmpahsize = true;
            this.btnSaveParameter.UseSubFont = false;
            this.btnSaveParameter.Click += new System.EventHandler(this.BtnSaveParameterClicked);
            // 
            // tableLayoutPanel3
            // 
            this.tableLayoutPanel3.ColumnCount = 6;
            this.tableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 16.66667F));
            this.tableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 16.66667F));
            this.tableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 16.66667F));
            this.tableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 16.66667F));
            this.tableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 16.66667F));
            this.tableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 16.66667F));
            this.tableLayoutPanel3.Controls.Add(this.pnLoadPort2, 1, 0);
            this.tableLayoutPanel3.Controls.Add(this.pnLoadPort1, 0, 0);
            this.tableLayoutPanel3.Controls.Add(this.pnLoadPort3, 2, 0);
            this.tableLayoutPanel3.Controls.Add(this.pnLoadPort4, 3, 0);
            this.tableLayoutPanel3.Controls.Add(this.pnLoadPort5, 4, 0);
            this.tableLayoutPanel3.Controls.Add(this.pnLoadPort6, 5, 0);
            this.tableLayoutPanel3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel3.Location = new System.Drawing.Point(3, 467);
            this.tableLayoutPanel3.Name = "tableLayoutPanel3";
            this.tableLayoutPanel3.RowCount = 1;
            this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 381F));
            this.tableLayoutPanel3.Size = new System.Drawing.Size(1134, 381);
            this.tableLayoutPanel3.TabIndex = 21147;
            // 
            // pnLoadPort2
            // 
            this.pnLoadPort2.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pnLoadPort2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnLoadPort2.Location = new System.Drawing.Point(188, 0);
            this.pnLoadPort2.Margin = new System.Windows.Forms.Padding(0);
            this.pnLoadPort2.Name = "pnLoadPort2";
            this.pnLoadPort2.Padding = new System.Windows.Forms.Padding(1);
            this.pnLoadPort2.Size = new System.Drawing.Size(188, 381);
            this.pnLoadPort2.TabIndex = 21133;
            // 
            // pnLoadPort1
            // 
            this.pnLoadPort1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pnLoadPort1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnLoadPort1.Location = new System.Drawing.Point(0, 0);
            this.pnLoadPort1.Margin = new System.Windows.Forms.Padding(0);
            this.pnLoadPort1.Name = "pnLoadPort1";
            this.pnLoadPort1.Padding = new System.Windows.Forms.Padding(1);
            this.pnLoadPort1.Size = new System.Drawing.Size(188, 381);
            this.pnLoadPort1.TabIndex = 21132;
            // 
            // pnLoadPort3
            // 
            this.pnLoadPort3.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pnLoadPort3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnLoadPort3.Location = new System.Drawing.Point(376, 0);
            this.pnLoadPort3.Margin = new System.Windows.Forms.Padding(0);
            this.pnLoadPort3.Name = "pnLoadPort3";
            this.pnLoadPort3.Padding = new System.Windows.Forms.Padding(1);
            this.pnLoadPort3.Size = new System.Drawing.Size(188, 381);
            this.pnLoadPort3.TabIndex = 21134;
            // 
            // pnLoadPort4
            // 
            this.pnLoadPort4.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pnLoadPort4.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnLoadPort4.Location = new System.Drawing.Point(564, 0);
            this.pnLoadPort4.Margin = new System.Windows.Forms.Padding(0);
            this.pnLoadPort4.Name = "pnLoadPort4";
            this.pnLoadPort4.Padding = new System.Windows.Forms.Padding(1);
            this.pnLoadPort4.Size = new System.Drawing.Size(188, 381);
            this.pnLoadPort4.TabIndex = 21135;
            // 
            // pnLoadPort5
            // 
            this.pnLoadPort5.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pnLoadPort5.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnLoadPort5.Location = new System.Drawing.Point(752, 0);
            this.pnLoadPort5.Margin = new System.Windows.Forms.Padding(0);
            this.pnLoadPort5.Name = "pnLoadPort5";
            this.pnLoadPort5.Padding = new System.Windows.Forms.Padding(1);
            this.pnLoadPort5.Size = new System.Drawing.Size(188, 381);
            this.pnLoadPort5.TabIndex = 21136;
            // 
            // pnLoadPort6
            // 
            this.pnLoadPort6.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pnLoadPort6.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnLoadPort6.Location = new System.Drawing.Point(940, 0);
            this.pnLoadPort6.Margin = new System.Windows.Forms.Padding(0);
            this.pnLoadPort6.Name = "pnLoadPort6";
            this.pnLoadPort6.Padding = new System.Windows.Forms.Padding(1);
            this.pnLoadPort6.Size = new System.Drawing.Size(194, 381);
            this.pnLoadPort6.TabIndex = 21137;
            // 
            // tableLayoutPanel2
            // 
            this.tableLayoutPanel2.ColumnCount = 2;
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel2.Controls.Add(this.sys3GroupBoxContainer1, 0, 0);
            this.tableLayoutPanel2.Controls.Add(this.tableLayoutPanel5, 1, 0);
            this.tableLayoutPanel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel2.Location = new System.Drawing.Point(0, 78);
            this.tableLayoutPanel2.Margin = new System.Windows.Forms.Padding(0);
            this.tableLayoutPanel2.Name = "tableLayoutPanel2";
            this.tableLayoutPanel2.RowCount = 1;
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 138F));
            this.tableLayoutPanel2.Size = new System.Drawing.Size(1140, 386);
            this.tableLayoutPanel2.TabIndex = 21147;
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 1;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.Controls.Add(this.panel1, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.sys3Separator1, 0, 1);
            this.tableLayoutPanel1.Controls.Add(this.tableLayoutPanel3, 0, 3);
            this.tableLayoutPanel1.Controls.Add(this.tableLayoutPanel2, 0, 2);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 4;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 51F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 3.448275F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 48.27586F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 48.27586F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(1140, 851);
            this.tableLayoutPanel1.TabIndex = 21147;
            // 
            // SetupProcessModulePWA500BIN
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.BackColor = System.Drawing.SystemColors.Control;
            this.Controls.Add(this.tableLayoutPanel1);
            this.Margin = new System.Windows.Forms.Padding(0);
            this.Name = "SetupProcessModulePWA500BIN";
            this.Size = new System.Drawing.Size(1140, 851);
            this.Tag = "";
            this.tableLayoutPanel5.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.gvServiceStatus)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gvClientStatus)).EndInit();
            this.tableLayoutPanel4.ResumeLayout(false);
            this.sys3GroupBoxContainer1.ResumeLayout(false);
            this.panel1.ResumeLayout(false);
            this.tableLayoutPanel3.ResumeLayout(false);
            this.tableLayoutPanel2.ResumeLayout(false);
            this.tableLayoutPanel1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel5;
        private System.Windows.Forms.Panel pnPWA500BINOptions;
        private System.Windows.Forms.DataGridViewTextBoxColumn PASSWORD;
        private System.Windows.Forms.DataGridViewTextBoxColumn ID;
        private Sys3Controls.Sys3DoubleBufferedDataGridView gvServiceStatus;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn2;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn1;
        private Sys3Controls.Sys3Label sys3Label7;
        private Sys3Controls.Sys3DoubleBufferedDataGridView gvClientStatus;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel4;
        private Sys3Controls.Sys3Label sys3Label1;
        private SUserControls.Sys3Separator sys3Separator1;
        private Sys3Controls.Sys3GroupBoxContainer sys3GroupBoxContainer1;
        private System.Windows.Forms.Panel panel1;
        private Sys3Controls.Sys3button btnUndoParameter;
        private Sys3Controls.Sys3button btnSaveParameter;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel3;
        private System.Windows.Forms.Panel pnLoadPort2;
        private System.Windows.Forms.Panel pnLoadPort1;
        private System.Windows.Forms.Panel pnLoadPort3;
        private System.Windows.Forms.Panel pnLoadPort4;
        private System.Windows.Forms.Panel pnLoadPort5;
        private System.Windows.Forms.Panel pnLoadPort6;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel2;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
    }
}
