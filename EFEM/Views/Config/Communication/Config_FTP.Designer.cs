namespace FrameOfSystem3.Views.Config
{
    partial class Config_FTP
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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle3 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle4 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle5 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle6 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle7 = new System.Windows.Forms.DataGridViewCellStyle();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.tableLayoutPanel3 = new System.Windows.Forms.TableLayoutPanel();
            this._lbl_SelectedName = new Sys3Controls.Sys3Label();
            this._lbl_SelectedIndex = new Sys3Controls.Sys3Label();
            this._lbl_SelectedItem_Index = new Sys3Controls.Sys3Label();
            this._lbl_SelectedItem_ServiceItemIndex = new Sys3Controls.Sys3Label();
            this.sys3GroupBoxContainer1 = new Sys3Controls.Sys3GroupBoxContainer();
            this._dgv_FTP_FileList = new Sys3Controls.Sys3DoubleBufferedDataGridView();
            this.dataGridViewTextBoxColumn6 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.m_groupboxDemonstration = new Sys3Controls.Sys3GroupBoxContainer();
            this._dgv_FTP_Server = new Sys3Controls.Sys3DoubleBufferedDataGridView();
            this.dataGridViewTextBoxColumn1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.NAME = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn3 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn2 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.SERVER_STATE = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn4 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Path = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.tableLayoutPanel2 = new System.Windows.Forms.TableLayoutPanel();
            this.sys3button4 = new Sys3Controls.Sys3button();
            this.sys3button3 = new Sys3Controls.Sys3button();
            this.sys3button2 = new Sys3Controls.Sys3button();
            this.sys3button1 = new Sys3Controls.Sys3button();
            this._btn_Upload = new Sys3Controls.Sys3button();
            this._btn_Add_Server = new Sys3Controls.Sys3button();
            this.tableLayoutPanel1.SuspendLayout();
            this.tableLayoutPanel3.SuspendLayout();
            this.sys3GroupBoxContainer1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this._dgv_FTP_FileList)).BeginInit();
            this.m_groupboxDemonstration.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this._dgv_FTP_Server)).BeginInit();
            this.tableLayoutPanel2.SuspendLayout();
            this.SuspendLayout();
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 1;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.Controls.Add(this.tableLayoutPanel3, 0, 1);
            this.tableLayoutPanel1.Controls.Add(this.sys3GroupBoxContainer1, 0, 2);
            this.tableLayoutPanel1.Controls.Add(this.m_groupboxDemonstration, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.tableLayoutPanel2, 0, 3);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel1.Margin = new System.Windows.Forms.Padding(0);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 4;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 40F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 5F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 35F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 20F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(1156, 846);
            this.tableLayoutPanel1.TabIndex = 0;
            // 
            // tableLayoutPanel3
            // 
            this.tableLayoutPanel3.ColumnCount = 4;
            this.tableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 10F));
            this.tableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 30F));
            this.tableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 10F));
            this.tableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel3.Controls.Add(this._lbl_SelectedName, 3, 0);
            this.tableLayoutPanel3.Controls.Add(this._lbl_SelectedIndex, 1, 0);
            this.tableLayoutPanel3.Controls.Add(this._lbl_SelectedItem_Index, 0, 0);
            this.tableLayoutPanel3.Controls.Add(this._lbl_SelectedItem_ServiceItemIndex, 2, 0);
            this.tableLayoutPanel3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel3.Location = new System.Drawing.Point(3, 341);
            this.tableLayoutPanel3.Name = "tableLayoutPanel3";
            this.tableLayoutPanel3.RowCount = 1;
            this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel3.Size = new System.Drawing.Size(1150, 36);
            this.tableLayoutPanel3.TabIndex = 1388;
            // 
            // _lbl_SelectedName
            // 
            this._lbl_SelectedName.BackGroundColor = System.Drawing.Color.White;
            this._lbl_SelectedName.BorderStroke = 2;
            this._lbl_SelectedName.BorderStyle = System.Windows.Forms.ButtonBorderStyle.Solid;
            this._lbl_SelectedName.Description = "";
            this._lbl_SelectedName.DisabledColor = System.Drawing.Color.LightGray;
            this._lbl_SelectedName.Dock = System.Windows.Forms.DockStyle.Fill;
            this._lbl_SelectedName.EdgeRadius = 1;
            this._lbl_SelectedName.ImagePosition = new System.Drawing.Point(0, 0);
            this._lbl_SelectedName.ImageSize = new System.Drawing.Point(0, 0);
            this._lbl_SelectedName.LoadImage = null;
            this._lbl_SelectedName.Location = new System.Drawing.Point(576, 1);
            this._lbl_SelectedName.MainFont = new System.Drawing.Font("맑은 고딕", 11F, System.Drawing.FontStyle.Bold);
            this._lbl_SelectedName.MainFontColor = System.Drawing.Color.Black;
            this._lbl_SelectedName.Margin = new System.Windows.Forms.Padding(1);
            this._lbl_SelectedName.Name = "_lbl_SelectedName";
            this._lbl_SelectedName.Size = new System.Drawing.Size(573, 34);
            this._lbl_SelectedName.SubFont = new System.Drawing.Font("맑은 고딕", 10F);
            this._lbl_SelectedName.SubFontColor = System.Drawing.Color.Black;
            this._lbl_SelectedName.SubText = "";
            this._lbl_SelectedName.TabIndex = 1404;
            this._lbl_SelectedName.TextAlignMain = Sys3Controls.EN_TEXTALIGN.BOTTOM_RIGHT;
            this._lbl_SelectedName.TextAlignSub = Sys3Controls.EN_TEXTALIGN.MIDDLE_CENTER;
            this._lbl_SelectedName.TextAlignUnit = Sys3Controls.EN_TEXTALIGN.MIDDLE_CENTER;
            this._lbl_SelectedName.ThemeIndex = 0;
            this._lbl_SelectedName.UnitAreaRate = 40;
            this._lbl_SelectedName.UnitFont = new System.Drawing.Font("맑은 고딕", 10F, System.Drawing.FontStyle.Bold);
            this._lbl_SelectedName.UnitFontColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this._lbl_SelectedName.UnitPositionVertical = false;
            this._lbl_SelectedName.UnitText = "";
            this._lbl_SelectedName.UseBorder = true;
            this._lbl_SelectedName.UseEdgeRadius = false;
            this._lbl_SelectedName.UseImage = false;
            this._lbl_SelectedName.UseSubFont = false;
            this._lbl_SelectedName.UseUnitFont = false;
            // 
            // _lbl_SelectedIndex
            // 
            this._lbl_SelectedIndex.BackGroundColor = System.Drawing.Color.White;
            this._lbl_SelectedIndex.BorderStroke = 2;
            this._lbl_SelectedIndex.BorderStyle = System.Windows.Forms.ButtonBorderStyle.Solid;
            this._lbl_SelectedIndex.Description = "";
            this._lbl_SelectedIndex.DisabledColor = System.Drawing.Color.LightGray;
            this._lbl_SelectedIndex.Dock = System.Windows.Forms.DockStyle.Fill;
            this._lbl_SelectedIndex.EdgeRadius = 1;
            this._lbl_SelectedIndex.ImagePosition = new System.Drawing.Point(0, 0);
            this._lbl_SelectedIndex.ImageSize = new System.Drawing.Point(0, 0);
            this._lbl_SelectedIndex.LoadImage = null;
            this._lbl_SelectedIndex.Location = new System.Drawing.Point(116, 1);
            this._lbl_SelectedIndex.MainFont = new System.Drawing.Font("맑은 고딕", 11F, System.Drawing.FontStyle.Bold);
            this._lbl_SelectedIndex.MainFontColor = System.Drawing.Color.Black;
            this._lbl_SelectedIndex.Margin = new System.Windows.Forms.Padding(1);
            this._lbl_SelectedIndex.Name = "_lbl_SelectedIndex";
            this._lbl_SelectedIndex.Size = new System.Drawing.Size(343, 34);
            this._lbl_SelectedIndex.SubFont = new System.Drawing.Font("맑은 고딕", 10F);
            this._lbl_SelectedIndex.SubFontColor = System.Drawing.Color.Black;
            this._lbl_SelectedIndex.SubText = "";
            this._lbl_SelectedIndex.TabIndex = 1405;
            this._lbl_SelectedIndex.TextAlignMain = Sys3Controls.EN_TEXTALIGN.BOTTOM_RIGHT;
            this._lbl_SelectedIndex.TextAlignSub = Sys3Controls.EN_TEXTALIGN.MIDDLE_CENTER;
            this._lbl_SelectedIndex.TextAlignUnit = Sys3Controls.EN_TEXTALIGN.MIDDLE_CENTER;
            this._lbl_SelectedIndex.ThemeIndex = 0;
            this._lbl_SelectedIndex.UnitAreaRate = 40;
            this._lbl_SelectedIndex.UnitFont = new System.Drawing.Font("맑은 고딕", 10F, System.Drawing.FontStyle.Bold);
            this._lbl_SelectedIndex.UnitFontColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this._lbl_SelectedIndex.UnitPositionVertical = false;
            this._lbl_SelectedIndex.UnitText = "";
            this._lbl_SelectedIndex.UseBorder = true;
            this._lbl_SelectedIndex.UseEdgeRadius = false;
            this._lbl_SelectedIndex.UseImage = false;
            this._lbl_SelectedIndex.UseSubFont = false;
            this._lbl_SelectedIndex.UseUnitFont = false;
            // 
            // _lbl_SelectedItem_Index
            // 
            this._lbl_SelectedItem_Index.BackGroundColor = System.Drawing.Color.FromArgb(((int)(((byte)(200)))), ((int)(((byte)(200)))), ((int)(((byte)(220)))));
            this._lbl_SelectedItem_Index.BorderStroke = 2;
            this._lbl_SelectedItem_Index.BorderStyle = System.Windows.Forms.ButtonBorderStyle.Solid;
            this._lbl_SelectedItem_Index.Description = "";
            this._lbl_SelectedItem_Index.DisabledColor = System.Drawing.Color.DarkGray;
            this._lbl_SelectedItem_Index.Dock = System.Windows.Forms.DockStyle.Fill;
            this._lbl_SelectedItem_Index.EdgeRadius = 1;
            this._lbl_SelectedItem_Index.ImagePosition = new System.Drawing.Point(0, 0);
            this._lbl_SelectedItem_Index.ImageSize = new System.Drawing.Point(0, 0);
            this._lbl_SelectedItem_Index.LoadImage = null;
            this._lbl_SelectedItem_Index.Location = new System.Drawing.Point(1, 1);
            this._lbl_SelectedItem_Index.MainFont = new System.Drawing.Font("맑은 고딕", 9F, System.Drawing.FontStyle.Bold);
            this._lbl_SelectedItem_Index.MainFontColor = System.Drawing.Color.Black;
            this._lbl_SelectedItem_Index.Margin = new System.Windows.Forms.Padding(1);
            this._lbl_SelectedItem_Index.Name = "_lbl_SelectedItem_Index";
            this._lbl_SelectedItem_Index.Size = new System.Drawing.Size(113, 34);
            this._lbl_SelectedItem_Index.SubFont = new System.Drawing.Font("맑은 고딕", 10F);
            this._lbl_SelectedItem_Index.SubFontColor = System.Drawing.Color.Black;
            this._lbl_SelectedItem_Index.SubText = "";
            this._lbl_SelectedItem_Index.TabIndex = 1407;
            this._lbl_SelectedItem_Index.Text = "INDEX";
            this._lbl_SelectedItem_Index.TextAlignMain = Sys3Controls.EN_TEXTALIGN.TOP_LEFT;
            this._lbl_SelectedItem_Index.TextAlignSub = Sys3Controls.EN_TEXTALIGN.MIDDLE_CENTER;
            this._lbl_SelectedItem_Index.TextAlignUnit = Sys3Controls.EN_TEXTALIGN.MIDDLE_CENTER;
            this._lbl_SelectedItem_Index.ThemeIndex = 0;
            this._lbl_SelectedItem_Index.UnitAreaRate = 40;
            this._lbl_SelectedItem_Index.UnitFont = new System.Drawing.Font("맑은 고딕", 10F, System.Drawing.FontStyle.Bold);
            this._lbl_SelectedItem_Index.UnitFontColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this._lbl_SelectedItem_Index.UnitPositionVertical = false;
            this._lbl_SelectedItem_Index.UnitText = "";
            this._lbl_SelectedItem_Index.UseBorder = true;
            this._lbl_SelectedItem_Index.UseEdgeRadius = false;
            this._lbl_SelectedItem_Index.UseImage = false;
            this._lbl_SelectedItem_Index.UseSubFont = false;
            this._lbl_SelectedItem_Index.UseUnitFont = false;
            // 
            // _lbl_SelectedItem_ServiceItemIndex
            // 
            this._lbl_SelectedItem_ServiceItemIndex.BackGroundColor = System.Drawing.Color.FromArgb(((int)(((byte)(200)))), ((int)(((byte)(200)))), ((int)(((byte)(220)))));
            this._lbl_SelectedItem_ServiceItemIndex.BorderStroke = 2;
            this._lbl_SelectedItem_ServiceItemIndex.BorderStyle = System.Windows.Forms.ButtonBorderStyle.Solid;
            this._lbl_SelectedItem_ServiceItemIndex.Description = "";
            this._lbl_SelectedItem_ServiceItemIndex.DisabledColor = System.Drawing.Color.DarkGray;
            this._lbl_SelectedItem_ServiceItemIndex.Dock = System.Windows.Forms.DockStyle.Fill;
            this._lbl_SelectedItem_ServiceItemIndex.EdgeRadius = 1;
            this._lbl_SelectedItem_ServiceItemIndex.ImagePosition = new System.Drawing.Point(0, 0);
            this._lbl_SelectedItem_ServiceItemIndex.ImageSize = new System.Drawing.Point(0, 0);
            this._lbl_SelectedItem_ServiceItemIndex.LoadImage = null;
            this._lbl_SelectedItem_ServiceItemIndex.Location = new System.Drawing.Point(461, 1);
            this._lbl_SelectedItem_ServiceItemIndex.MainFont = new System.Drawing.Font("맑은 고딕", 9F, System.Drawing.FontStyle.Bold);
            this._lbl_SelectedItem_ServiceItemIndex.MainFontColor = System.Drawing.Color.Black;
            this._lbl_SelectedItem_ServiceItemIndex.Margin = new System.Windows.Forms.Padding(1);
            this._lbl_SelectedItem_ServiceItemIndex.Name = "_lbl_SelectedItem_ServiceItemIndex";
            this._lbl_SelectedItem_ServiceItemIndex.Size = new System.Drawing.Size(113, 34);
            this._lbl_SelectedItem_ServiceItemIndex.SubFont = new System.Drawing.Font("맑은 고딕", 10F);
            this._lbl_SelectedItem_ServiceItemIndex.SubFontColor = System.Drawing.Color.Black;
            this._lbl_SelectedItem_ServiceItemIndex.SubText = "";
            this._lbl_SelectedItem_ServiceItemIndex.TabIndex = 1406;
            this._lbl_SelectedItem_ServiceItemIndex.Text = "NAME";
            this._lbl_SelectedItem_ServiceItemIndex.TextAlignMain = Sys3Controls.EN_TEXTALIGN.TOP_LEFT;
            this._lbl_SelectedItem_ServiceItemIndex.TextAlignSub = Sys3Controls.EN_TEXTALIGN.MIDDLE_CENTER;
            this._lbl_SelectedItem_ServiceItemIndex.TextAlignUnit = Sys3Controls.EN_TEXTALIGN.MIDDLE_CENTER;
            this._lbl_SelectedItem_ServiceItemIndex.ThemeIndex = 0;
            this._lbl_SelectedItem_ServiceItemIndex.UnitAreaRate = 40;
            this._lbl_SelectedItem_ServiceItemIndex.UnitFont = new System.Drawing.Font("맑은 고딕", 10F, System.Drawing.FontStyle.Bold);
            this._lbl_SelectedItem_ServiceItemIndex.UnitFontColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this._lbl_SelectedItem_ServiceItemIndex.UnitPositionVertical = false;
            this._lbl_SelectedItem_ServiceItemIndex.UnitText = "";
            this._lbl_SelectedItem_ServiceItemIndex.UseBorder = true;
            this._lbl_SelectedItem_ServiceItemIndex.UseEdgeRadius = false;
            this._lbl_SelectedItem_ServiceItemIndex.UseImage = false;
            this._lbl_SelectedItem_ServiceItemIndex.UseSubFont = false;
            this._lbl_SelectedItem_ServiceItemIndex.UseUnitFont = false;
            // 
            // sys3GroupBoxContainer1
            // 
            this.sys3GroupBoxContainer1.BackGroundColor = System.Drawing.Color.WhiteSmoke;
            this.sys3GroupBoxContainer1.Controls.Add(this._dgv_FTP_FileList);
            this.sys3GroupBoxContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.sys3GroupBoxContainer1.EdgeBorderStroke = 2;
            this.sys3GroupBoxContainer1.EdgeRadius = 2;
            this.sys3GroupBoxContainer1.LabelFont = new System.Drawing.Font("맑은 고딕", 10F, System.Drawing.FontStyle.Bold);
            this.sys3GroupBoxContainer1.LabelGradientColorFirst = System.Drawing.Color.FromArgb(((int)(((byte)(208)))), ((int)(((byte)(208)))), ((int)(((byte)(208)))));
            this.sys3GroupBoxContainer1.LabelGradientColorSecond = System.Drawing.Color.FromArgb(((int)(((byte)(240)))), ((int)(((byte)(240)))), ((int)(((byte)(240)))));
            this.sys3GroupBoxContainer1.LabelHeight = 30;
            this.sys3GroupBoxContainer1.LabelTextColor = System.Drawing.Color.Black;
            this.sys3GroupBoxContainer1.Location = new System.Drawing.Point(0, 380);
            this.sys3GroupBoxContainer1.Margin = new System.Windows.Forms.Padding(0);
            this.sys3GroupBoxContainer1.Name = "sys3GroupBoxContainer1";
            this.sys3GroupBoxContainer1.Padding = new System.Windows.Forms.Padding(3, 18, 3, 3);
            this.sys3GroupBoxContainer1.Size = new System.Drawing.Size(1156, 296);
            this.sys3GroupBoxContainer1.TabIndex = 1387;
            this.sys3GroupBoxContainer1.TabStop = false;
            this.sys3GroupBoxContainer1.Text = "FILE LIST";
            this.sys3GroupBoxContainer1.TextAlign = Sys3Controls.EN_TEXTALIGN.MIDDLE_CENTER;
            this.sys3GroupBoxContainer1.ThemeIndex = 0;
            this.sys3GroupBoxContainer1.UseLabelBorder = true;
            this.sys3GroupBoxContainer1.UseTitle = true;
            // 
            // _dgv_FTP_FileList
            // 
            this._dgv_FTP_FileList.AllowUserToAddRows = false;
            this._dgv_FTP_FileList.AllowUserToDeleteRows = false;
            this._dgv_FTP_FileList.AllowUserToResizeColumns = false;
            this._dgv_FTP_FileList.AllowUserToResizeRows = false;
            this._dgv_FTP_FileList.BackgroundColor = System.Drawing.Color.White;
            this._dgv_FTP_FileList.CellBorderStyle = System.Windows.Forms.DataGridViewCellBorderStyle.Raised;
            this._dgv_FTP_FileList.ColumnHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.Single;
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle1.BackColor = System.Drawing.Color.LightGray;
            dataGridViewCellStyle1.Font = new System.Drawing.Font("맑은 고딕", 11F, System.Drawing.FontStyle.Bold);
            dataGridViewCellStyle1.ForeColor = System.Drawing.Color.Black;
            dataGridViewCellStyle1.SelectionBackColor = System.Drawing.Color.White;
            dataGridViewCellStyle1.SelectionForeColor = System.Drawing.Color.Black;
            dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this._dgv_FTP_FileList.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
            this._dgv_FTP_FileList.ColumnHeadersHeight = 40;
            this._dgv_FTP_FileList.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            this._dgv_FTP_FileList.ColumnHeadersVisible = false;
            this._dgv_FTP_FileList.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.dataGridViewTextBoxColumn6});
            dataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle3.BackColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle3.Font = new System.Drawing.Font("맑은 고딕", 10F, System.Drawing.FontStyle.Bold);
            dataGridViewCellStyle3.ForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle3.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(120)))), ((int)(((byte)(130)))), ((int)(((byte)(150)))));
            dataGridViewCellStyle3.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle3.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this._dgv_FTP_FileList.DefaultCellStyle = dataGridViewCellStyle3;
            this._dgv_FTP_FileList.Dock = System.Windows.Forms.DockStyle.Fill;
            this._dgv_FTP_FileList.EnableHeadersVisualStyles = false;
            this._dgv_FTP_FileList.GridColor = System.Drawing.Color.FromArgb(((int)(((byte)(120)))), ((int)(((byte)(130)))), ((int)(((byte)(150)))));
            this._dgv_FTP_FileList.Location = new System.Drawing.Point(3, 32);
            this._dgv_FTP_FileList.Margin = new System.Windows.Forms.Padding(0);
            this._dgv_FTP_FileList.MultiSelect = false;
            this._dgv_FTP_FileList.Name = "_dgv_FTP_FileList";
            this._dgv_FTP_FileList.ReadOnly = true;
            this._dgv_FTP_FileList.RowHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.None;
            dataGridViewCellStyle4.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle4.BackColor = System.Drawing.Color.White;
            dataGridViewCellStyle4.Font = new System.Drawing.Font("맑은 고딕", 11F);
            dataGridViewCellStyle4.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle4.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(120)))), ((int)(((byte)(130)))), ((int)(((byte)(150)))));
            dataGridViewCellStyle4.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle4.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this._dgv_FTP_FileList.RowHeadersDefaultCellStyle = dataGridViewCellStyle4;
            this._dgv_FTP_FileList.RowHeadersVisible = false;
            this._dgv_FTP_FileList.RowHeadersWidthSizeMode = System.Windows.Forms.DataGridViewRowHeadersWidthSizeMode.DisableResizing;
            this._dgv_FTP_FileList.RowTemplate.Height = 23;
            this._dgv_FTP_FileList.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this._dgv_FTP_FileList.Size = new System.Drawing.Size(1150, 261);
            this._dgv_FTP_FileList.TabIndex = 1351;
            this._dgv_FTP_FileList.CellClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.Click_FileGrid);
            this._dgv_FTP_FileList.CellContentDoubleClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.DoubleClick_FileGrid);
            this._dgv_FTP_FileList.CellDoubleClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.DoubleClick_FileGrid);
            // 
            // dataGridViewTextBoxColumn6
            // 
            this.dataGridViewTextBoxColumn6.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            this.dataGridViewTextBoxColumn6.DefaultCellStyle = dataGridViewCellStyle2;
            this.dataGridViewTextBoxColumn6.HeaderText = "NAME";
            this.dataGridViewTextBoxColumn6.Name = "dataGridViewTextBoxColumn6";
            this.dataGridViewTextBoxColumn6.ReadOnly = true;
            this.dataGridViewTextBoxColumn6.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // m_groupboxDemonstration
            // 
            this.m_groupboxDemonstration.BackGroundColor = System.Drawing.Color.WhiteSmoke;
            this.m_groupboxDemonstration.Controls.Add(this._dgv_FTP_Server);
            this.m_groupboxDemonstration.Dock = System.Windows.Forms.DockStyle.Fill;
            this.m_groupboxDemonstration.EdgeBorderStroke = 2;
            this.m_groupboxDemonstration.EdgeRadius = 2;
            this.m_groupboxDemonstration.LabelFont = new System.Drawing.Font("맑은 고딕", 10F, System.Drawing.FontStyle.Bold);
            this.m_groupboxDemonstration.LabelGradientColorFirst = System.Drawing.Color.FromArgb(((int)(((byte)(208)))), ((int)(((byte)(208)))), ((int)(((byte)(208)))));
            this.m_groupboxDemonstration.LabelGradientColorSecond = System.Drawing.Color.FromArgb(((int)(((byte)(240)))), ((int)(((byte)(240)))), ((int)(((byte)(240)))));
            this.m_groupboxDemonstration.LabelHeight = 30;
            this.m_groupboxDemonstration.LabelTextColor = System.Drawing.Color.Black;
            this.m_groupboxDemonstration.Location = new System.Drawing.Point(0, 0);
            this.m_groupboxDemonstration.Margin = new System.Windows.Forms.Padding(0);
            this.m_groupboxDemonstration.Name = "m_groupboxDemonstration";
            this.m_groupboxDemonstration.Padding = new System.Windows.Forms.Padding(3, 18, 3, 3);
            this.m_groupboxDemonstration.Size = new System.Drawing.Size(1156, 338);
            this.m_groupboxDemonstration.TabIndex = 1385;
            this.m_groupboxDemonstration.TabStop = false;
            this.m_groupboxDemonstration.Text = "SERVER LIST";
            this.m_groupboxDemonstration.TextAlign = Sys3Controls.EN_TEXTALIGN.MIDDLE_CENTER;
            this.m_groupboxDemonstration.ThemeIndex = 0;
            this.m_groupboxDemonstration.UseLabelBorder = true;
            this.m_groupboxDemonstration.UseTitle = true;
            // 
            // _dgv_FTP_Server
            // 
            this._dgv_FTP_Server.AllowUserToAddRows = false;
            this._dgv_FTP_Server.AllowUserToDeleteRows = false;
            this._dgv_FTP_Server.AllowUserToResizeColumns = false;
            this._dgv_FTP_Server.AllowUserToResizeRows = false;
            this._dgv_FTP_Server.BackgroundColor = System.Drawing.Color.White;
            this._dgv_FTP_Server.CellBorderStyle = System.Windows.Forms.DataGridViewCellBorderStyle.Raised;
            this._dgv_FTP_Server.ColumnHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.Single;
            dataGridViewCellStyle5.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle5.BackColor = System.Drawing.Color.LightGray;
            dataGridViewCellStyle5.Font = new System.Drawing.Font("맑은 고딕", 11F, System.Drawing.FontStyle.Bold);
            dataGridViewCellStyle5.ForeColor = System.Drawing.Color.Black;
            dataGridViewCellStyle5.SelectionBackColor = System.Drawing.Color.White;
            dataGridViewCellStyle5.SelectionForeColor = System.Drawing.Color.Black;
            dataGridViewCellStyle5.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this._dgv_FTP_Server.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle5;
            this._dgv_FTP_Server.ColumnHeadersHeight = 40;
            this._dgv_FTP_Server.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            this._dgv_FTP_Server.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.dataGridViewTextBoxColumn1,
            this.NAME,
            this.dataGridViewTextBoxColumn3,
            this.dataGridViewTextBoxColumn2,
            this.SERVER_STATE,
            this.dataGridViewTextBoxColumn4,
            this.Path});
            dataGridViewCellStyle6.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle6.BackColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle6.Font = new System.Drawing.Font("맑은 고딕", 10F, System.Drawing.FontStyle.Bold);
            dataGridViewCellStyle6.ForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle6.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(120)))), ((int)(((byte)(130)))), ((int)(((byte)(150)))));
            dataGridViewCellStyle6.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle6.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this._dgv_FTP_Server.DefaultCellStyle = dataGridViewCellStyle6;
            this._dgv_FTP_Server.Dock = System.Windows.Forms.DockStyle.Fill;
            this._dgv_FTP_Server.EnableHeadersVisualStyles = false;
            this._dgv_FTP_Server.GridColor = System.Drawing.Color.FromArgb(((int)(((byte)(120)))), ((int)(((byte)(130)))), ((int)(((byte)(150)))));
            this._dgv_FTP_Server.Location = new System.Drawing.Point(3, 32);
            this._dgv_FTP_Server.Margin = new System.Windows.Forms.Padding(0);
            this._dgv_FTP_Server.MultiSelect = false;
            this._dgv_FTP_Server.Name = "_dgv_FTP_Server";
            this._dgv_FTP_Server.ReadOnly = true;
            this._dgv_FTP_Server.RowHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.None;
            dataGridViewCellStyle7.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle7.BackColor = System.Drawing.Color.White;
            dataGridViewCellStyle7.Font = new System.Drawing.Font("맑은 고딕", 11F);
            dataGridViewCellStyle7.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle7.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(120)))), ((int)(((byte)(130)))), ((int)(((byte)(150)))));
            dataGridViewCellStyle7.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle7.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this._dgv_FTP_Server.RowHeadersDefaultCellStyle = dataGridViewCellStyle7;
            this._dgv_FTP_Server.RowHeadersVisible = false;
            this._dgv_FTP_Server.RowHeadersWidthSizeMode = System.Windows.Forms.DataGridViewRowHeadersWidthSizeMode.DisableResizing;
            this._dgv_FTP_Server.RowTemplate.Height = 23;
            this._dgv_FTP_Server.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this._dgv_FTP_Server.Size = new System.Drawing.Size(1150, 303);
            this._dgv_FTP_Server.TabIndex = 1351;
            this._dgv_FTP_Server.CellClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.Click_ServerGrid);
            this._dgv_FTP_Server.CellContentDoubleClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.DoubleClick_ServerGrid);
            this._dgv_FTP_Server.CellDoubleClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.DoubleClick_ServerGrid);
            // 
            // dataGridViewTextBoxColumn1
            // 
            this.dataGridViewTextBoxColumn1.Frozen = true;
            this.dataGridViewTextBoxColumn1.HeaderText = "INDEX";
            this.dataGridViewTextBoxColumn1.MaxInputLength = 20;
            this.dataGridViewTextBoxColumn1.Name = "dataGridViewTextBoxColumn1";
            this.dataGridViewTextBoxColumn1.ReadOnly = true;
            this.dataGridViewTextBoxColumn1.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.dataGridViewTextBoxColumn1.Width = 80;
            // 
            // NAME
            // 
            this.NAME.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.NAME.HeaderText = "NAME";
            this.NAME.Name = "NAME";
            this.NAME.ReadOnly = true;
            this.NAME.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // dataGridViewTextBoxColumn3
            // 
            this.dataGridViewTextBoxColumn3.HeaderText = "ADDRESS";
            this.dataGridViewTextBoxColumn3.MaxInputLength = 20;
            this.dataGridViewTextBoxColumn3.Name = "dataGridViewTextBoxColumn3";
            this.dataGridViewTextBoxColumn3.ReadOnly = true;
            this.dataGridViewTextBoxColumn3.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.dataGridViewTextBoxColumn3.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.dataGridViewTextBoxColumn3.Width = 300;
            // 
            // dataGridViewTextBoxColumn2
            // 
            this.dataGridViewTextBoxColumn2.HeaderText = "ID";
            this.dataGridViewTextBoxColumn2.Name = "dataGridViewTextBoxColumn2";
            this.dataGridViewTextBoxColumn2.ReadOnly = true;
            this.dataGridViewTextBoxColumn2.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.dataGridViewTextBoxColumn2.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.dataGridViewTextBoxColumn2.Width = 125;
            // 
            // SERVER_STATE
            // 
            this.SERVER_STATE.HeaderText = "PASSWORD";
            this.SERVER_STATE.Name = "SERVER_STATE";
            this.SERVER_STATE.ReadOnly = true;
            this.SERVER_STATE.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.SERVER_STATE.Width = 125;
            // 
            // dataGridViewTextBoxColumn4
            // 
            this.dataGridViewTextBoxColumn4.HeaderText = "PORT";
            this.dataGridViewTextBoxColumn4.MaxInputLength = 20;
            this.dataGridViewTextBoxColumn4.Name = "dataGridViewTextBoxColumn4";
            this.dataGridViewTextBoxColumn4.ReadOnly = true;
            this.dataGridViewTextBoxColumn4.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.dataGridViewTextBoxColumn4.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // Path
            // 
            this.Path.HeaderText = "PATH";
            this.Path.Name = "Path";
            this.Path.ReadOnly = true;
            this.Path.Width = 200;
            // 
            // tableLayoutPanel2
            // 
            this.tableLayoutPanel2.ColumnCount = 4;
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.tableLayoutPanel2.Controls.Add(this.sys3button4, 3, 0);
            this.tableLayoutPanel2.Controls.Add(this.sys3button3, 1, 1);
            this.tableLayoutPanel2.Controls.Add(this.sys3button2, 2, 0);
            this.tableLayoutPanel2.Controls.Add(this.sys3button1, 0, 1);
            this.tableLayoutPanel2.Controls.Add(this._btn_Upload, 1, 0);
            this.tableLayoutPanel2.Controls.Add(this._btn_Add_Server, 0, 0);
            this.tableLayoutPanel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel2.Location = new System.Drawing.Point(3, 679);
            this.tableLayoutPanel2.Name = "tableLayoutPanel2";
            this.tableLayoutPanel2.RowCount = 2;
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel2.Size = new System.Drawing.Size(1150, 164);
            this.tableLayoutPanel2.TabIndex = 1386;
            // 
            // sys3button4
            // 
            this.sys3button4.BorderWidth = 3;
            this.sys3button4.ButtonClicked = false;
            this.sys3button4.ClickedEmphasizeTextColor = System.Drawing.Color.White;
            this.sys3button4.CustomClickedGradientFirstColor = System.Drawing.Color.White;
            this.sys3button4.CustomClickedGradientSecondColor = System.Drawing.Color.White;
            this.sys3button4.Description = "";
            this.sys3button4.DisabledColor = System.Drawing.Color.DarkGray;
            this.sys3button4.Dock = System.Windows.Forms.DockStyle.Fill;
            this.sys3button4.EdgeRadius = 5;
            this.sys3button4.GradientAngle = 70F;
            this.sys3button4.GradientFirstColor = System.Drawing.Color.White;
            this.sys3button4.GradientSecondColor = System.Drawing.Color.FromArgb(((int)(((byte)(170)))), ((int)(((byte)(176)))), ((int)(((byte)(183)))));
            this.sys3button4.HoverEmphasizeCustomColor = System.Drawing.Color.White;
            this.sys3button4.ImagePosition = new System.Drawing.Point(7, 7);
            this.sys3button4.ImageSize = new System.Drawing.Point(30, 30);
            this.sys3button4.LoadImage = global::FrameOfSystem3.Properties.Resources.REMOVE;
            this.sys3button4.Location = new System.Drawing.Point(864, 3);
            this.sys3button4.MainFont = new System.Drawing.Font("맑은 고딕", 13F, System.Drawing.FontStyle.Bold);
            this.sys3button4.MainFontColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(36)))), ((int)(((byte)(0)))));
            this.sys3button4.Name = "sys3button4";
            this.sys3button4.Size = new System.Drawing.Size(283, 76);
            this.sys3button4.SubFont = new System.Drawing.Font("맑은 고딕", 8F, System.Drawing.FontStyle.Bold);
            this.sys3button4.SubFontColor = System.Drawing.Color.DarkBlue;
            this.sys3button4.SubText = "STATUS";
            this.sys3button4.TabIndex = 3;
            this.sys3button4.Text = "DELETE";
            this.sys3button4.TextAlignMain = Sys3Controls.EN_TEXTALIGN.MIDDLE_RIGHT;
            this.sys3button4.TextAlignSub = Sys3Controls.EN_TEXTALIGN.TOP_LEFT;
            this.sys3button4.ThemeIndex = 0;
            this.sys3button4.UseBorder = true;
            this.sys3button4.UseClickedEmphasizeTextColor = false;
            this.sys3button4.UseCustomizeClickedColor = false;
            this.sys3button4.UseEdge = true;
            this.sys3button4.UseHoverEmphasizeCustomColor = false;
            this.sys3button4.UseImage = true;
            this.sys3button4.UserHoverEmpahsize = false;
            this.sys3button4.UseSubFont = false;
            this.sys3button4.Click += new System.EventHandler(this.Click_Button);
            // 
            // sys3button3
            // 
            this.sys3button3.BorderWidth = 3;
            this.sys3button3.ButtonClicked = false;
            this.sys3button3.ClickedEmphasizeTextColor = System.Drawing.Color.White;
            this.sys3button3.CustomClickedGradientFirstColor = System.Drawing.Color.White;
            this.sys3button3.CustomClickedGradientSecondColor = System.Drawing.Color.White;
            this.sys3button3.Description = "";
            this.sys3button3.DisabledColor = System.Drawing.Color.DarkGray;
            this.sys3button3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.sys3button3.EdgeRadius = 5;
            this.sys3button3.GradientAngle = 70F;
            this.sys3button3.GradientFirstColor = System.Drawing.Color.White;
            this.sys3button3.GradientSecondColor = System.Drawing.Color.FromArgb(((int)(((byte)(170)))), ((int)(((byte)(176)))), ((int)(((byte)(183)))));
            this.sys3button3.HoverEmphasizeCustomColor = System.Drawing.Color.White;
            this.sys3button3.ImagePosition = new System.Drawing.Point(7, 7);
            this.sys3button3.ImageSize = new System.Drawing.Point(30, 30);
            this.sys3button3.LoadImage = global::FrameOfSystem3.Properties.Resources.INSERT_BLACK;
            this.sys3button3.Location = new System.Drawing.Point(290, 85);
            this.sys3button3.MainFont = new System.Drawing.Font("맑은 고딕", 13F, System.Drawing.FontStyle.Bold);
            this.sys3button3.MainFontColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(36)))), ((int)(((byte)(0)))));
            this.sys3button3.Name = "sys3button3";
            this.sys3button3.Size = new System.Drawing.Size(281, 76);
            this.sys3button3.SubFont = new System.Drawing.Font("맑은 고딕", 8F, System.Drawing.FontStyle.Bold);
            this.sys3button3.SubFontColor = System.Drawing.Color.DarkBlue;
            this.sys3button3.SubText = "STATUS";
            this.sys3button3.TabIndex = 5;
            this.sys3button3.Text = "DOWNLOAD";
            this.sys3button3.TextAlignMain = Sys3Controls.EN_TEXTALIGN.MIDDLE_RIGHT;
            this.sys3button3.TextAlignSub = Sys3Controls.EN_TEXTALIGN.TOP_LEFT;
            this.sys3button3.ThemeIndex = 0;
            this.sys3button3.UseBorder = true;
            this.sys3button3.UseClickedEmphasizeTextColor = false;
            this.sys3button3.UseCustomizeClickedColor = false;
            this.sys3button3.UseEdge = true;
            this.sys3button3.UseHoverEmphasizeCustomColor = false;
            this.sys3button3.UseImage = true;
            this.sys3button3.UserHoverEmpahsize = false;
            this.sys3button3.UseSubFont = false;
            this.sys3button3.Click += new System.EventHandler(this.Click_Button);
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
            this.sys3button2.GradientAngle = 70F;
            this.sys3button2.GradientFirstColor = System.Drawing.Color.White;
            this.sys3button2.GradientSecondColor = System.Drawing.Color.FromArgb(((int)(((byte)(170)))), ((int)(((byte)(176)))), ((int)(((byte)(183)))));
            this.sys3button2.HoverEmphasizeCustomColor = System.Drawing.Color.White;
            this.sys3button2.ImagePosition = new System.Drawing.Point(7, 7);
            this.sys3button2.ImageSize = new System.Drawing.Point(30, 30);
            this.sys3button2.LoadImage = global::FrameOfSystem3.Properties.Resources.CREATE_FOLDER;
            this.sys3button2.Location = new System.Drawing.Point(577, 3);
            this.sys3button2.MainFont = new System.Drawing.Font("맑은 고딕", 13F, System.Drawing.FontStyle.Bold);
            this.sys3button2.MainFontColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(36)))), ((int)(((byte)(0)))));
            this.sys3button2.Name = "sys3button2";
            this.sys3button2.Size = new System.Drawing.Size(281, 76);
            this.sys3button2.SubFont = new System.Drawing.Font("맑은 고딕", 8F, System.Drawing.FontStyle.Bold);
            this.sys3button2.SubFontColor = System.Drawing.Color.DarkBlue;
            this.sys3button2.SubText = "STATUS";
            this.sys3button2.TabIndex = 2;
            this.sys3button2.Text = "CREATE FOLDER";
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
            this.sys3button2.Click += new System.EventHandler(this.Click_Button);
            // 
            // sys3button1
            // 
            this.sys3button1.BorderWidth = 3;
            this.sys3button1.ButtonClicked = false;
            this.sys3button1.ClickedEmphasizeTextColor = System.Drawing.Color.White;
            this.sys3button1.CustomClickedGradientFirstColor = System.Drawing.Color.White;
            this.sys3button1.CustomClickedGradientSecondColor = System.Drawing.Color.White;
            this.sys3button1.Description = "";
            this.sys3button1.DisabledColor = System.Drawing.Color.DarkGray;
            this.sys3button1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.sys3button1.EdgeRadius = 5;
            this.sys3button1.GradientAngle = 70F;
            this.sys3button1.GradientFirstColor = System.Drawing.Color.White;
            this.sys3button1.GradientSecondColor = System.Drawing.Color.FromArgb(((int)(((byte)(170)))), ((int)(((byte)(176)))), ((int)(((byte)(183)))));
            this.sys3button1.HoverEmphasizeCustomColor = System.Drawing.Color.White;
            this.sys3button1.ImagePosition = new System.Drawing.Point(7, 7);
            this.sys3button1.ImageSize = new System.Drawing.Point(30, 30);
            this.sys3button1.LoadImage = global::FrameOfSystem3.Properties.Resources.REPEAT_BLACK;
            this.sys3button1.Location = new System.Drawing.Point(3, 85);
            this.sys3button1.MainFont = new System.Drawing.Font("맑은 고딕", 13F, System.Drawing.FontStyle.Bold);
            this.sys3button1.MainFontColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(36)))), ((int)(((byte)(0)))));
            this.sys3button1.Name = "sys3button1";
            this.sys3button1.Size = new System.Drawing.Size(281, 76);
            this.sys3button1.SubFont = new System.Drawing.Font("맑은 고딕", 8F, System.Drawing.FontStyle.Bold);
            this.sys3button1.SubFontColor = System.Drawing.Color.DarkBlue;
            this.sys3button1.SubText = "STATUS";
            this.sys3button1.TabIndex = 4;
            this.sys3button1.Text = "REFRESH\\nFILE LIST";
            this.sys3button1.TextAlignMain = Sys3Controls.EN_TEXTALIGN.MIDDLE_RIGHT;
            this.sys3button1.TextAlignSub = Sys3Controls.EN_TEXTALIGN.TOP_LEFT;
            this.sys3button1.ThemeIndex = 0;
            this.sys3button1.UseBorder = true;
            this.sys3button1.UseClickedEmphasizeTextColor = false;
            this.sys3button1.UseCustomizeClickedColor = false;
            this.sys3button1.UseEdge = true;
            this.sys3button1.UseHoverEmphasizeCustomColor = false;
            this.sys3button1.UseImage = true;
            this.sys3button1.UserHoverEmpahsize = false;
            this.sys3button1.UseSubFont = false;
            this.sys3button1.Click += new System.EventHandler(this.Click_Button);
            // 
            // _btn_Upload
            // 
            this._btn_Upload.BorderWidth = 3;
            this._btn_Upload.ButtonClicked = false;
            this._btn_Upload.ClickedEmphasizeTextColor = System.Drawing.Color.White;
            this._btn_Upload.CustomClickedGradientFirstColor = System.Drawing.Color.White;
            this._btn_Upload.CustomClickedGradientSecondColor = System.Drawing.Color.White;
            this._btn_Upload.Description = "";
            this._btn_Upload.DisabledColor = System.Drawing.Color.DarkGray;
            this._btn_Upload.Dock = System.Windows.Forms.DockStyle.Fill;
            this._btn_Upload.EdgeRadius = 5;
            this._btn_Upload.GradientAngle = 70F;
            this._btn_Upload.GradientFirstColor = System.Drawing.Color.White;
            this._btn_Upload.GradientSecondColor = System.Drawing.Color.FromArgb(((int)(((byte)(170)))), ((int)(((byte)(176)))), ((int)(((byte)(183)))));
            this._btn_Upload.HoverEmphasizeCustomColor = System.Drawing.Color.White;
            this._btn_Upload.ImagePosition = new System.Drawing.Point(7, 7);
            this._btn_Upload.ImageSize = new System.Drawing.Point(30, 30);
            this._btn_Upload.LoadImage = global::FrameOfSystem3.Properties.Resources.SEND_BLACK;
            this._btn_Upload.Location = new System.Drawing.Point(290, 3);
            this._btn_Upload.MainFont = new System.Drawing.Font("맑은 고딕", 13F, System.Drawing.FontStyle.Bold);
            this._btn_Upload.MainFontColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(36)))), ((int)(((byte)(0)))));
            this._btn_Upload.Name = "_btn_Upload";
            this._btn_Upload.Size = new System.Drawing.Size(281, 76);
            this._btn_Upload.SubFont = new System.Drawing.Font("맑은 고딕", 8F, System.Drawing.FontStyle.Bold);
            this._btn_Upload.SubFontColor = System.Drawing.Color.DarkBlue;
            this._btn_Upload.SubText = "STATUS";
            this._btn_Upload.TabIndex = 1;
            this._btn_Upload.Text = "UPLOAD";
            this._btn_Upload.TextAlignMain = Sys3Controls.EN_TEXTALIGN.MIDDLE_RIGHT;
            this._btn_Upload.TextAlignSub = Sys3Controls.EN_TEXTALIGN.TOP_LEFT;
            this._btn_Upload.ThemeIndex = 0;
            this._btn_Upload.UseBorder = true;
            this._btn_Upload.UseClickedEmphasizeTextColor = false;
            this._btn_Upload.UseCustomizeClickedColor = false;
            this._btn_Upload.UseEdge = true;
            this._btn_Upload.UseHoverEmphasizeCustomColor = false;
            this._btn_Upload.UseImage = true;
            this._btn_Upload.UserHoverEmpahsize = false;
            this._btn_Upload.UseSubFont = false;
            this._btn_Upload.Click += new System.EventHandler(this.Click_Button);
            // 
            // _btn_Add_Server
            // 
            this._btn_Add_Server.BorderWidth = 3;
            this._btn_Add_Server.ButtonClicked = false;
            this._btn_Add_Server.ClickedEmphasizeTextColor = System.Drawing.Color.White;
            this._btn_Add_Server.CustomClickedGradientFirstColor = System.Drawing.Color.White;
            this._btn_Add_Server.CustomClickedGradientSecondColor = System.Drawing.Color.White;
            this._btn_Add_Server.Description = "";
            this._btn_Add_Server.DisabledColor = System.Drawing.Color.DarkGray;
            this._btn_Add_Server.Dock = System.Windows.Forms.DockStyle.Fill;
            this._btn_Add_Server.EdgeRadius = 5;
            this._btn_Add_Server.GradientAngle = 70F;
            this._btn_Add_Server.GradientFirstColor = System.Drawing.Color.White;
            this._btn_Add_Server.GradientSecondColor = System.Drawing.Color.FromArgb(((int)(((byte)(170)))), ((int)(((byte)(176)))), ((int)(((byte)(183)))));
            this._btn_Add_Server.HoverEmphasizeCustomColor = System.Drawing.Color.White;
            this._btn_Add_Server.ImagePosition = new System.Drawing.Point(7, 7);
            this._btn_Add_Server.ImageSize = new System.Drawing.Point(30, 30);
            this._btn_Add_Server.LoadImage = global::FrameOfSystem3.Properties.Resources.CONFIG_ADD3;
            this._btn_Add_Server.Location = new System.Drawing.Point(3, 3);
            this._btn_Add_Server.MainFont = new System.Drawing.Font("맑은 고딕", 13F, System.Drawing.FontStyle.Bold);
            this._btn_Add_Server.MainFontColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(36)))), ((int)(((byte)(0)))));
            this._btn_Add_Server.Name = "_btn_Add_Server";
            this._btn_Add_Server.Size = new System.Drawing.Size(281, 76);
            this._btn_Add_Server.SubFont = new System.Drawing.Font("맑은 고딕", 8F, System.Drawing.FontStyle.Bold);
            this._btn_Add_Server.SubFontColor = System.Drawing.Color.DarkBlue;
            this._btn_Add_Server.SubText = "STATUS";
            this._btn_Add_Server.TabIndex = 0;
            this._btn_Add_Server.Text = "ADD\\n(SERVER)";
            this._btn_Add_Server.TextAlignMain = Sys3Controls.EN_TEXTALIGN.MIDDLE_RIGHT;
            this._btn_Add_Server.TextAlignSub = Sys3Controls.EN_TEXTALIGN.TOP_LEFT;
            this._btn_Add_Server.ThemeIndex = 0;
            this._btn_Add_Server.UseBorder = true;
            this._btn_Add_Server.UseClickedEmphasizeTextColor = false;
            this._btn_Add_Server.UseCustomizeClickedColor = false;
            this._btn_Add_Server.UseEdge = true;
            this._btn_Add_Server.UseHoverEmphasizeCustomColor = false;
            this._btn_Add_Server.UseImage = true;
            this._btn_Add_Server.UserHoverEmpahsize = false;
            this._btn_Add_Server.UseSubFont = false;
            this._btn_Add_Server.Click += new System.EventHandler(this.Click_Button);
            // 
            // Config_FTP
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.Controls.Add(this.tableLayoutPanel1);
            this.Name = "Config_FTP";
            this.Size = new System.Drawing.Size(1156, 846);
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel3.ResumeLayout(false);
            this.sys3GroupBoxContainer1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this._dgv_FTP_FileList)).EndInit();
            this.m_groupboxDemonstration.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this._dgv_FTP_Server)).EndInit();
            this.tableLayoutPanel2.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private Sys3Controls.Sys3GroupBoxContainer m_groupboxDemonstration;
        private Sys3Controls.Sys3DoubleBufferedDataGridView _dgv_FTP_Server;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel2;
        private Sys3Controls.Sys3button _btn_Add_Server;
        private Sys3Controls.Sys3button _btn_Upload;
        private Sys3Controls.Sys3GroupBoxContainer sys3GroupBoxContainer1;
        private Sys3Controls.Sys3DoubleBufferedDataGridView _dgv_FTP_FileList;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn1;
        private System.Windows.Forms.DataGridViewTextBoxColumn NAME;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn3;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn2;
        private System.Windows.Forms.DataGridViewTextBoxColumn SERVER_STATE;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn4;
        private System.Windows.Forms.DataGridViewTextBoxColumn Path;
        private Sys3Controls.Sys3button sys3button1;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel3;
        private Sys3Controls.Sys3Label _lbl_SelectedName;
        private Sys3Controls.Sys3Label _lbl_SelectedIndex;
        private Sys3Controls.Sys3Label _lbl_SelectedItem_Index;
        private Sys3Controls.Sys3Label _lbl_SelectedItem_ServiceItemIndex;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn6;
        private Sys3Controls.Sys3button sys3button2;
        private Sys3Controls.Sys3button sys3button4;
        private Sys3Controls.Sys3button sys3button3;

    }
}
