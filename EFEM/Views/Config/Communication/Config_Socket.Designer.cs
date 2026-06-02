namespace FrameOfSystem3.Views.Config
{
    partial class Config_Socket
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
            this.m_listMessage = new System.Windows.Forms.ListBox();
            this.m_dgViewSocket = new Sys3Controls.Sys3DoubleBufferedDataGridView();
            this.ID = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ENABLE = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.MONITORING = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.PASSWORD = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.AUTHORITY = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.TARGETPORT = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.CONDITION = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.m_groupList = new Sys3Controls.Sys3GroupBoxContainer();
            this.m_groupSelectedItem = new Sys3Controls.Sys3GroupBoxContainer();
            this.tableLayoutPanel2 = new System.Windows.Forms.TableLayoutPanel();
            this._tableLayoutPanel_SelectedItem_Right = new System.Windows.Forms.TableLayoutPanel();
            this.m_labelState = new Sys3Controls.Sys3Label();
            this.m_lblState = new Sys3Controls.Sys3Label();
            this._tableLayoutPanel_SelectedItem_Left = new System.Windows.Forms.TableLayoutPanel();
            this.m_lblIndex = new Sys3Controls.Sys3Label();
            this.m_labelIndex = new Sys3Controls.Sys3Label();
            this.m_groupConfiguration = new Sys3Controls.Sys3GroupBoxContainer();
            this._tableLayoutPanel_Config = new System.Windows.Forms.TableLayoutPanel();
            this.m_lblName = new Sys3Controls.Sys3Label();
            this.m_labelLogType = new Sys3Controls.Sys3Label();
            this.m_lblProtocolType = new Sys3Controls.Sys3Label();
            this.m_lblLogType = new Sys3Controls.Sys3Label();
            this.m_lblServerIP = new Sys3Controls.Sys3Label();
            this.m_lblPort = new Sys3Controls.Sys3Label();
            this.m_lblRetryInterval = new Sys3Controls.Sys3Label();
            this.m_lblReceiveTimeout = new Sys3Controls.Sys3Label();
            this.m_labelName = new Sys3Controls.Sys3Label();
            this.m_labelProtocolType = new Sys3Controls.Sys3Label();
            this.m_labelServerIP = new Sys3Controls.Sys3Label();
            this.m_labelPort = new Sys3Controls.Sys3Label();
            this.m_lblTargetPort = new Sys3Controls.Sys3Label();
            this.m_labelReceiveTimeout = new Sys3Controls.Sys3Label();
            this.m_labelTargetPort = new Sys3Controls.Sys3Label();
            this.m_labelRetryInterval = new Sys3Controls.Sys3Label();
            this.m_groupSocketDemonstration = new Sys3Controls.Sys3GroupBoxContainer();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.m_labelMessage = new Sys3Controls.Sys3Label();
            this.m_lblMessage = new Sys3Controls.Sys3Label();
            this.m_btnDisconnect = new Sys3Controls.Sys3button();
            this.m_btnConnect = new Sys3Controls.Sys3button();
            this.m_btnSend = new Sys3Controls.Sys3button();
            this.m_btnClearMessage = new Sys3Controls.Sys3button();
            this._tableLayoutPanelMain = new System.Windows.Forms.TableLayoutPanel();
            this.m_btnRemove = new Sys3Controls.Sys3button();
            this.m_btnAdd = new Sys3Controls.Sys3button();
            this.m_btnDisable = new Sys3Controls.Sys3button();
            this.m_btnEnable = new Sys3Controls.Sys3button();
            this.tableLayoutPanel3 = new System.Windows.Forms.TableLayoutPanel();
            ((System.ComponentModel.ISupportInitialize)(this.m_dgViewSocket)).BeginInit();
            this.m_groupList.SuspendLayout();
            this.m_groupSelectedItem.SuspendLayout();
            this.tableLayoutPanel2.SuspendLayout();
            this._tableLayoutPanel_SelectedItem_Right.SuspendLayout();
            this._tableLayoutPanel_SelectedItem_Left.SuspendLayout();
            this.m_groupConfiguration.SuspendLayout();
            this._tableLayoutPanel_Config.SuspendLayout();
            this.m_groupSocketDemonstration.SuspendLayout();
            this.tableLayoutPanel1.SuspendLayout();
            this._tableLayoutPanelMain.SuspendLayout();
            this.tableLayoutPanel3.SuspendLayout();
            this.SuspendLayout();
            // 
            // m_listMessage
            // 
            this.m_listMessage.BackColor = System.Drawing.Color.Gainsboro;
            this.tableLayoutPanel1.SetColumnSpan(this.m_listMessage, 4);
            this.m_listMessage.Dock = System.Windows.Forms.DockStyle.Fill;
            this.m_listMessage.Font = new System.Drawing.Font("맑은 고딕", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.m_listMessage.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(50)))), ((int)(((byte)(50)))), ((int)(((byte)(50)))));
            this.m_listMessage.FormattingEnabled = true;
            this.m_listMessage.ItemHeight = 17;
            this.m_listMessage.Location = new System.Drawing.Point(8, 7);
            this.m_listMessage.Name = "m_listMessage";
            this.tableLayoutPanel1.SetRowSpan(this.m_listMessage, 6);
            this.m_listMessage.Size = new System.Drawing.Size(554, 288);
            this.m_listMessage.TabIndex = 1363;
            // 
            // m_dgViewSocket
            // 
            this.m_dgViewSocket.AllowUserToAddRows = false;
            this.m_dgViewSocket.AllowUserToDeleteRows = false;
            this.m_dgViewSocket.AllowUserToResizeColumns = false;
            this.m_dgViewSocket.AllowUserToResizeRows = false;
            this.m_dgViewSocket.BackgroundColor = System.Drawing.Color.White;
            this.m_dgViewSocket.CellBorderStyle = System.Windows.Forms.DataGridViewCellBorderStyle.Raised;
            this.m_dgViewSocket.ColumnHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.Single;
            dataGridViewCellStyle4.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle4.BackColor = System.Drawing.Color.LightGray;
            dataGridViewCellStyle4.Font = new System.Drawing.Font("맑은 고딕", 11F, System.Drawing.FontStyle.Bold);
            dataGridViewCellStyle4.ForeColor = System.Drawing.Color.Black;
            dataGridViewCellStyle4.SelectionBackColor = System.Drawing.Color.White;
            dataGridViewCellStyle4.SelectionForeColor = System.Drawing.Color.Black;
            dataGridViewCellStyle4.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.m_dgViewSocket.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle4;
            this.m_dgViewSocket.ColumnHeadersHeight = 30;
            this.m_dgViewSocket.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            this.m_dgViewSocket.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.ID,
            this.ENABLE,
            this.MONITORING,
            this.PASSWORD,
            this.AUTHORITY,
            this.TARGETPORT,
            this.CONDITION});
            dataGridViewCellStyle5.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle5.BackColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle5.Font = new System.Drawing.Font("맑은 고딕", 10F, System.Drawing.FontStyle.Bold);
            dataGridViewCellStyle5.ForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle5.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(120)))), ((int)(((byte)(130)))), ((int)(((byte)(150)))));
            dataGridViewCellStyle5.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle5.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.m_dgViewSocket.DefaultCellStyle = dataGridViewCellStyle5;
            this.m_dgViewSocket.Dock = System.Windows.Forms.DockStyle.Fill;
            this.m_dgViewSocket.EnableHeadersVisualStyles = false;
            this.m_dgViewSocket.GridColor = System.Drawing.Color.FromArgb(((int)(((byte)(120)))), ((int)(((byte)(130)))), ((int)(((byte)(150)))));
            this.m_dgViewSocket.Location = new System.Drawing.Point(3, 32);
            this.m_dgViewSocket.MultiSelect = false;
            this.m_dgViewSocket.Name = "m_dgViewSocket";
            this.m_dgViewSocket.ReadOnly = true;
            this.m_dgViewSocket.RowHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.None;
            dataGridViewCellStyle6.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle6.BackColor = System.Drawing.Color.White;
            dataGridViewCellStyle6.Font = new System.Drawing.Font("맑은 고딕", 11F);
            dataGridViewCellStyle6.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle6.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(120)))), ((int)(((byte)(130)))), ((int)(((byte)(150)))));
            dataGridViewCellStyle6.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle6.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.m_dgViewSocket.RowHeadersDefaultCellStyle = dataGridViewCellStyle6;
            this.m_dgViewSocket.RowHeadersVisible = false;
            this.m_dgViewSocket.RowHeadersWidthSizeMode = System.Windows.Forms.DataGridViewRowHeadersWidthSizeMode.DisableResizing;
            this.m_dgViewSocket.RowTemplate.Height = 23;
            this.m_dgViewSocket.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.m_dgViewSocket.Size = new System.Drawing.Size(1150, 237);
            this.m_dgViewSocket.TabIndex = 1349;
            this.m_dgViewSocket.CellClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.Click_Dgview);
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
            // ENABLE
            // 
            this.ENABLE.HeaderText = "ENABLE";
            this.ENABLE.Name = "ENABLE";
            this.ENABLE.ReadOnly = true;
            this.ENABLE.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.ENABLE.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.ENABLE.Width = 65;
            // 
            // MONITORING
            // 
            this.MONITORING.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.MONITORING.HeaderText = "NAME";
            this.MONITORING.Name = "MONITORING";
            this.MONITORING.ReadOnly = true;
            this.MONITORING.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.MONITORING.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // PASSWORD
            // 
            this.PASSWORD.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.PASSWORD.HeaderText = "PROTOCOL TYPE";
            this.PASSWORD.MaxInputLength = 20;
            this.PASSWORD.Name = "PASSWORD";
            this.PASSWORD.ReadOnly = true;
            this.PASSWORD.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.PASSWORD.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // AUTHORITY
            // 
            this.AUTHORITY.HeaderText = "PORT";
            this.AUTHORITY.MaxInputLength = 20;
            this.AUTHORITY.Name = "AUTHORITY";
            this.AUTHORITY.ReadOnly = true;
            this.AUTHORITY.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.AUTHORITY.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.AUTHORITY.Width = 125;
            // 
            // TARGETPORT
            // 
            this.TARGETPORT.HeaderText = "TARGET PORT";
            this.TARGETPORT.Name = "TARGETPORT";
            this.TARGETPORT.ReadOnly = true;
            this.TARGETPORT.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.TARGETPORT.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.TARGETPORT.Width = 125;
            // 
            // CONDITION
            // 
            this.CONDITION.HeaderText = "STATE";
            this.CONDITION.Name = "CONDITION";
            this.CONDITION.ReadOnly = true;
            this.CONDITION.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.CONDITION.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.CONDITION.Width = 250;
            // 
            // m_groupList
            // 
            this.m_groupList.BackGroundColor = System.Drawing.Color.WhiteSmoke;
            this._tableLayoutPanelMain.SetColumnSpan(this.m_groupList, 2);
            this.m_groupList.Controls.Add(this.m_dgViewSocket);
            this.m_groupList.Dock = System.Windows.Forms.DockStyle.Fill;
            this.m_groupList.EdgeBorderStroke = 2;
            this.m_groupList.EdgeRadius = 2;
            this.m_groupList.LabelFont = new System.Drawing.Font("맑은 고딕", 10F, System.Drawing.FontStyle.Bold);
            this.m_groupList.LabelGradientColorFirst = System.Drawing.Color.FromArgb(((int)(((byte)(208)))), ((int)(((byte)(208)))), ((int)(((byte)(208)))));
            this.m_groupList.LabelGradientColorSecond = System.Drawing.Color.FromArgb(((int)(((byte)(240)))), ((int)(((byte)(240)))), ((int)(((byte)(240)))));
            this.m_groupList.LabelHeight = 32;
            this.m_groupList.LabelTextColor = System.Drawing.Color.Black;
            this.m_groupList.Location = new System.Drawing.Point(0, 0);
            this.m_groupList.Margin = new System.Windows.Forms.Padding(0);
            this.m_groupList.Name = "m_groupList";
            this.m_groupList.Padding = new System.Windows.Forms.Padding(3, 18, 3, 3);
            this.m_groupList.Size = new System.Drawing.Size(1156, 272);
            this.m_groupList.TabIndex = 1380;
            this.m_groupList.TabStop = false;
            this.m_groupList.Text = "SOCKET LIST";
            this.m_groupList.TextAlign = Sys3Controls.EN_TEXTALIGN.MIDDLE_CENTER;
            this.m_groupList.ThemeIndex = 0;
            this.m_groupList.UseLabelBorder = true;
            this.m_groupList.UseTitle = true;
            // 
            // m_groupSelectedItem
            // 
            this.m_groupSelectedItem.BackGroundColor = System.Drawing.Color.WhiteSmoke;
            this._tableLayoutPanelMain.SetColumnSpan(this.m_groupSelectedItem, 2);
            this.m_groupSelectedItem.Controls.Add(this.tableLayoutPanel2);
            this.m_groupSelectedItem.Dock = System.Windows.Forms.DockStyle.Fill;
            this.m_groupSelectedItem.EdgeBorderStroke = 2;
            this.m_groupSelectedItem.EdgeRadius = 2;
            this.m_groupSelectedItem.LabelFont = new System.Drawing.Font("맑은 고딕", 10F, System.Drawing.FontStyle.Bold);
            this.m_groupSelectedItem.LabelGradientColorFirst = System.Drawing.Color.FromArgb(((int)(((byte)(208)))), ((int)(((byte)(208)))), ((int)(((byte)(208)))));
            this.m_groupSelectedItem.LabelGradientColorSecond = System.Drawing.Color.FromArgb(((int)(((byte)(240)))), ((int)(((byte)(240)))), ((int)(((byte)(240)))));
            this.m_groupSelectedItem.LabelHeight = 30;
            this.m_groupSelectedItem.LabelTextColor = System.Drawing.Color.Black;
            this.m_groupSelectedItem.Location = new System.Drawing.Point(0, 272);
            this.m_groupSelectedItem.Margin = new System.Windows.Forms.Padding(0);
            this.m_groupSelectedItem.Name = "m_groupSelectedItem";
            this.m_groupSelectedItem.Padding = new System.Windows.Forms.Padding(3, 18, 3, 3);
            this.m_groupSelectedItem.Size = new System.Drawing.Size(1156, 98);
            this.m_groupSelectedItem.TabIndex = 1381;
            this.m_groupSelectedItem.TabStop = false;
            this.m_groupSelectedItem.Text = "SELECTED ITEM";
            this.m_groupSelectedItem.TextAlign = Sys3Controls.EN_TEXTALIGN.MIDDLE_CENTER;
            this.m_groupSelectedItem.ThemeIndex = 0;
            this.m_groupSelectedItem.UseLabelBorder = true;
            this.m_groupSelectedItem.UseTitle = true;
            // 
            // tableLayoutPanel2
            // 
            this.tableLayoutPanel2.ColumnCount = 2;
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel2.Controls.Add(this._tableLayoutPanel_SelectedItem_Right, 0, 0);
            this.tableLayoutPanel2.Controls.Add(this._tableLayoutPanel_SelectedItem_Left, 0, 0);
            this.tableLayoutPanel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel2.Location = new System.Drawing.Point(3, 32);
            this.tableLayoutPanel2.Margin = new System.Windows.Forms.Padding(0);
            this.tableLayoutPanel2.Name = "tableLayoutPanel2";
            this.tableLayoutPanel2.RowCount = 1;
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel2.Size = new System.Drawing.Size(1150, 63);
            this.tableLayoutPanel2.TabIndex = 0;
            // 
            // _tableLayoutPanel_SelectedItem_Right
            // 
            this._tableLayoutPanel_SelectedItem_Right.ColumnCount = 6;
            this._tableLayoutPanel_SelectedItem_Right.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 2.10728F));
            this._tableLayoutPanel_SelectedItem_Right.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 29.93295F));
            this._tableLayoutPanel_SelectedItem_Right.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 17.95977F));
            this._tableLayoutPanel_SelectedItem_Right.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 29.93295F));
            this._tableLayoutPanel_SelectedItem_Right.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 17.95977F));
            this._tableLayoutPanel_SelectedItem_Right.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 2.10728F));
            this._tableLayoutPanel_SelectedItem_Right.Controls.Add(this.m_lblState, 1, 1);
            this._tableLayoutPanel_SelectedItem_Right.Controls.Add(this.m_labelState, 2, 1);
            this._tableLayoutPanel_SelectedItem_Right.Dock = System.Windows.Forms.DockStyle.Fill;
            this._tableLayoutPanel_SelectedItem_Right.Location = new System.Drawing.Point(575, 0);
            this._tableLayoutPanel_SelectedItem_Right.Margin = new System.Windows.Forms.Padding(0);
            this._tableLayoutPanel_SelectedItem_Right.Name = "_tableLayoutPanel_SelectedItem_Right";
            this._tableLayoutPanel_SelectedItem_Right.RowCount = 3;
            this._tableLayoutPanel_SelectedItem_Right.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 13.23529F));
            this._tableLayoutPanel_SelectedItem_Right.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 73.52941F));
            this._tableLayoutPanel_SelectedItem_Right.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 13.23529F));
            this._tableLayoutPanel_SelectedItem_Right.Size = new System.Drawing.Size(575, 63);
            this._tableLayoutPanel_SelectedItem_Right.TabIndex = 2;
            // 
            // m_labelState
            // 
            this.m_labelState.BackGroundColor = System.Drawing.Color.White;
            this.m_labelState.BorderStroke = 2;
            this.m_labelState.BorderStyle = System.Windows.Forms.ButtonBorderStyle.Solid;
            this._tableLayoutPanel_SelectedItem_Right.SetColumnSpan(this.m_labelState, 3);
            this.m_labelState.Description = "";
            this.m_labelState.DisabledColor = System.Drawing.Color.LightGray;
            this.m_labelState.Dock = System.Windows.Forms.DockStyle.Fill;
            this.m_labelState.EdgeRadius = 1;
            this.m_labelState.Enabled = false;
            this.m_labelState.ImagePosition = new System.Drawing.Point(0, 0);
            this.m_labelState.ImageSize = new System.Drawing.Point(0, 0);
            this.m_labelState.LoadImage = null;
            this.m_labelState.Location = new System.Drawing.Point(185, 9);
            this.m_labelState.MainFont = new System.Drawing.Font("맑은 고딕", 11F, System.Drawing.FontStyle.Bold);
            this.m_labelState.MainFontColor = System.Drawing.Color.Black;
            this.m_labelState.Margin = new System.Windows.Forms.Padding(1);
            this.m_labelState.Name = "m_labelState";
            this.m_labelState.Size = new System.Drawing.Size(376, 44);
            this.m_labelState.SubFont = new System.Drawing.Font("맑은 고딕", 10F);
            this.m_labelState.SubFontColor = System.Drawing.Color.Black;
            this.m_labelState.SubText = "";
            this.m_labelState.TabIndex = 1394;
            this.m_labelState.Text = "--";
            this.m_labelState.TextAlignMain = Sys3Controls.EN_TEXTALIGN.BOTTOM_RIGHT;
            this.m_labelState.TextAlignSub = Sys3Controls.EN_TEXTALIGN.MIDDLE_CENTER;
            this.m_labelState.TextAlignUnit = Sys3Controls.EN_TEXTALIGN.MIDDLE_CENTER;
            this.m_labelState.ThemeIndex = 0;
            this.m_labelState.UnitAreaRate = 40;
            this.m_labelState.UnitFont = new System.Drawing.Font("맑은 고딕", 10F, System.Drawing.FontStyle.Bold);
            this.m_labelState.UnitFontColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.m_labelState.UnitPositionVertical = false;
            this.m_labelState.UnitText = "";
            this.m_labelState.UseBorder = true;
            this.m_labelState.UseEdgeRadius = false;
            this.m_labelState.UseImage = false;
            this.m_labelState.UseSubFont = false;
            this.m_labelState.UseUnitFont = false;
            // 
            // m_lblState
            // 
            this.m_lblState.BackGroundColor = System.Drawing.Color.FromArgb(((int)(((byte)(200)))), ((int)(((byte)(200)))), ((int)(((byte)(220)))));
            this.m_lblState.BorderStroke = 2;
            this.m_lblState.BorderStyle = System.Windows.Forms.ButtonBorderStyle.Solid;
            this.m_lblState.Description = "";
            this.m_lblState.DisabledColor = System.Drawing.Color.DarkGray;
            this.m_lblState.Dock = System.Windows.Forms.DockStyle.Fill;
            this.m_lblState.EdgeRadius = 1;
            this.m_lblState.ImagePosition = new System.Drawing.Point(0, 0);
            this.m_lblState.ImageSize = new System.Drawing.Point(0, 0);
            this.m_lblState.LoadImage = null;
            this.m_lblState.Location = new System.Drawing.Point(13, 9);
            this.m_lblState.MainFont = new System.Drawing.Font("맑은 고딕", 9F, System.Drawing.FontStyle.Bold);
            this.m_lblState.MainFontColor = System.Drawing.Color.Black;
            this.m_lblState.Margin = new System.Windows.Forms.Padding(1);
            this.m_lblState.Name = "m_lblState";
            this.m_lblState.Size = new System.Drawing.Size(170, 44);
            this.m_lblState.SubFont = new System.Drawing.Font("맑은 고딕", 10F);
            this.m_lblState.SubFontColor = System.Drawing.Color.Black;
            this.m_lblState.SubText = "";
            this.m_lblState.TabIndex = 1392;
            this.m_lblState.Text = "STATE";
            this.m_lblState.TextAlignMain = Sys3Controls.EN_TEXTALIGN.TOP_LEFT;
            this.m_lblState.TextAlignSub = Sys3Controls.EN_TEXTALIGN.MIDDLE_CENTER;
            this.m_lblState.TextAlignUnit = Sys3Controls.EN_TEXTALIGN.MIDDLE_CENTER;
            this.m_lblState.ThemeIndex = 0;
            this.m_lblState.UnitAreaRate = 40;
            this.m_lblState.UnitFont = new System.Drawing.Font("맑은 고딕", 10F, System.Drawing.FontStyle.Bold);
            this.m_lblState.UnitFontColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.m_lblState.UnitPositionVertical = false;
            this.m_lblState.UnitText = "";
            this.m_lblState.UseBorder = true;
            this.m_lblState.UseEdgeRadius = false;
            this.m_lblState.UseImage = false;
            this.m_lblState.UseSubFont = false;
            this.m_lblState.UseUnitFont = false;
            // 
            // _tableLayoutPanel_SelectedItem_Left
            // 
            this._tableLayoutPanel_SelectedItem_Left.ColumnCount = 6;
            this._tableLayoutPanel_SelectedItem_Left.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 2.10728F));
            this._tableLayoutPanel_SelectedItem_Left.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 29.93295F));
            this._tableLayoutPanel_SelectedItem_Left.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 17.95977F));
            this._tableLayoutPanel_SelectedItem_Left.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 29.93295F));
            this._tableLayoutPanel_SelectedItem_Left.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 17.95977F));
            this._tableLayoutPanel_SelectedItem_Left.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 2.10728F));
            this._tableLayoutPanel_SelectedItem_Left.Controls.Add(this.m_lblIndex, 1, 1);
            this._tableLayoutPanel_SelectedItem_Left.Controls.Add(this.m_labelIndex, 2, 1);
            this._tableLayoutPanel_SelectedItem_Left.Dock = System.Windows.Forms.DockStyle.Fill;
            this._tableLayoutPanel_SelectedItem_Left.Location = new System.Drawing.Point(0, 0);
            this._tableLayoutPanel_SelectedItem_Left.Margin = new System.Windows.Forms.Padding(0);
            this._tableLayoutPanel_SelectedItem_Left.Name = "_tableLayoutPanel_SelectedItem_Left";
            this._tableLayoutPanel_SelectedItem_Left.RowCount = 3;
            this._tableLayoutPanel_SelectedItem_Left.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 13.23529F));
            this._tableLayoutPanel_SelectedItem_Left.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 73.52941F));
            this._tableLayoutPanel_SelectedItem_Left.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 13.23529F));
            this._tableLayoutPanel_SelectedItem_Left.Size = new System.Drawing.Size(575, 63);
            this._tableLayoutPanel_SelectedItem_Left.TabIndex = 1;
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
            this.m_lblIndex.Location = new System.Drawing.Point(13, 9);
            this.m_lblIndex.MainFont = new System.Drawing.Font("맑은 고딕", 9F, System.Drawing.FontStyle.Bold);
            this.m_lblIndex.MainFontColor = System.Drawing.Color.Black;
            this.m_lblIndex.Margin = new System.Windows.Forms.Padding(1);
            this.m_lblIndex.Name = "m_lblIndex";
            this.m_lblIndex.Size = new System.Drawing.Size(170, 44);
            this.m_lblIndex.SubFont = new System.Drawing.Font("맑은 고딕", 10F);
            this.m_lblIndex.SubFontColor = System.Drawing.Color.Black;
            this.m_lblIndex.SubText = "";
            this.m_lblIndex.TabIndex = 1391;
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
            this._tableLayoutPanel_SelectedItem_Left.SetColumnSpan(this.m_labelIndex, 3);
            this.m_labelIndex.Description = "";
            this.m_labelIndex.DisabledColor = System.Drawing.Color.LightGray;
            this.m_labelIndex.Dock = System.Windows.Forms.DockStyle.Fill;
            this.m_labelIndex.EdgeRadius = 1;
            this.m_labelIndex.Enabled = false;
            this.m_labelIndex.ImagePosition = new System.Drawing.Point(0, 0);
            this.m_labelIndex.ImageSize = new System.Drawing.Point(0, 0);
            this.m_labelIndex.LoadImage = null;
            this.m_labelIndex.Location = new System.Drawing.Point(185, 9);
            this.m_labelIndex.MainFont = new System.Drawing.Font("맑은 고딕", 11F, System.Drawing.FontStyle.Bold);
            this.m_labelIndex.MainFontColor = System.Drawing.Color.Black;
            this.m_labelIndex.Margin = new System.Windows.Forms.Padding(1);
            this.m_labelIndex.Name = "m_labelIndex";
            this.m_labelIndex.Size = new System.Drawing.Size(376, 44);
            this.m_labelIndex.SubFont = new System.Drawing.Font("맑은 고딕", 10F);
            this.m_labelIndex.SubFontColor = System.Drawing.Color.Black;
            this.m_labelIndex.SubText = "";
            this.m_labelIndex.TabIndex = 1393;
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
            // m_groupConfiguration
            // 
            this.m_groupConfiguration.BackGroundColor = System.Drawing.Color.WhiteSmoke;
            this.m_groupConfiguration.Controls.Add(this._tableLayoutPanel_Config);
            this.m_groupConfiguration.Dock = System.Windows.Forms.DockStyle.Fill;
            this.m_groupConfiguration.EdgeBorderStroke = 2;
            this.m_groupConfiguration.EdgeRadius = 2;
            this.m_groupConfiguration.LabelFont = new System.Drawing.Font("맑은 고딕", 10F, System.Drawing.FontStyle.Bold);
            this.m_groupConfiguration.LabelGradientColorFirst = System.Drawing.Color.FromArgb(((int)(((byte)(208)))), ((int)(((byte)(208)))), ((int)(((byte)(208)))));
            this.m_groupConfiguration.LabelGradientColorSecond = System.Drawing.Color.FromArgb(((int)(((byte)(240)))), ((int)(((byte)(240)))), ((int)(((byte)(240)))));
            this.m_groupConfiguration.LabelHeight = 30;
            this.m_groupConfiguration.LabelTextColor = System.Drawing.Color.Black;
            this.m_groupConfiguration.Location = new System.Drawing.Point(0, 370);
            this.m_groupConfiguration.Margin = new System.Windows.Forms.Padding(0);
            this.m_groupConfiguration.Name = "m_groupConfiguration";
            this.m_groupConfiguration.Padding = new System.Windows.Forms.Padding(3, 18, 3, 3);
            this.m_groupConfiguration.Size = new System.Drawing.Size(578, 476);
            this.m_groupConfiguration.TabIndex = 1382;
            this.m_groupConfiguration.TabStop = false;
            this.m_groupConfiguration.Text = "CONFIGURATION";
            this.m_groupConfiguration.TextAlign = Sys3Controls.EN_TEXTALIGN.MIDDLE_CENTER;
            this.m_groupConfiguration.ThemeIndex = 0;
            this.m_groupConfiguration.UseLabelBorder = true;
            this.m_groupConfiguration.UseTitle = true;
            // 
            // _tableLayoutPanel_Config
            // 
            this._tableLayoutPanel_Config.BackColor = System.Drawing.Color.WhiteSmoke;
            this._tableLayoutPanel_Config.ColumnCount = 6;
            this._tableLayoutPanel_Config.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 1.020352F));
            this._tableLayoutPanel_Config.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 24.48982F));
            this._tableLayoutPanel_Config.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 24.48982F));
            this._tableLayoutPanel_Config.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 24.48982F));
            this._tableLayoutPanel_Config.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 24.48982F));
            this._tableLayoutPanel_Config.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 1.020352F));
            this._tableLayoutPanel_Config.Controls.Add(this.m_lblName, 1, 1);
            this._tableLayoutPanel_Config.Controls.Add(this.m_labelLogType, 2, 7);
            this._tableLayoutPanel_Config.Controls.Add(this.m_lblProtocolType, 1, 2);
            this._tableLayoutPanel_Config.Controls.Add(this.m_lblLogType, 1, 7);
            this._tableLayoutPanel_Config.Controls.Add(this.m_lblServerIP, 1, 3);
            this._tableLayoutPanel_Config.Controls.Add(this.m_lblPort, 1, 4);
            this._tableLayoutPanel_Config.Controls.Add(this.m_lblRetryInterval, 1, 5);
            this._tableLayoutPanel_Config.Controls.Add(this.m_lblReceiveTimeout, 1, 6);
            this._tableLayoutPanel_Config.Controls.Add(this.m_labelName, 2, 1);
            this._tableLayoutPanel_Config.Controls.Add(this.m_labelProtocolType, 2, 2);
            this._tableLayoutPanel_Config.Controls.Add(this.m_labelServerIP, 2, 3);
            this._tableLayoutPanel_Config.Controls.Add(this.m_labelPort, 2, 4);
            this._tableLayoutPanel_Config.Controls.Add(this.m_lblTargetPort, 3, 4);
            this._tableLayoutPanel_Config.Controls.Add(this.m_labelReceiveTimeout, 2, 6);
            this._tableLayoutPanel_Config.Controls.Add(this.m_labelTargetPort, 4, 4);
            this._tableLayoutPanel_Config.Controls.Add(this.m_labelRetryInterval, 2, 5);
            this._tableLayoutPanel_Config.Controls.Add(this.tableLayoutPanel3, 1, 9);
            this._tableLayoutPanel_Config.Dock = System.Windows.Forms.DockStyle.Fill;
            this._tableLayoutPanel_Config.Location = new System.Drawing.Point(3, 32);
            this._tableLayoutPanel_Config.Name = "_tableLayoutPanel_Config";
            this._tableLayoutPanel_Config.RowCount = 11;
            this._tableLayoutPanel_Config.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 1.027651F));
            this._tableLayoutPanel_Config.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 11.1568F));
            this._tableLayoutPanel_Config.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 11.1568F));
            this._tableLayoutPanel_Config.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 11.1568F));
            this._tableLayoutPanel_Config.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 11.1568F));
            this._tableLayoutPanel_Config.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 11.1568F));
            this._tableLayoutPanel_Config.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 11.1568F));
            this._tableLayoutPanel_Config.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 11.1568F));
            this._tableLayoutPanel_Config.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 2.411873F));
            this._tableLayoutPanel_Config.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 17.43523F));
            this._tableLayoutPanel_Config.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 1.027651F));
            this._tableLayoutPanel_Config.Size = new System.Drawing.Size(572, 441);
            this._tableLayoutPanel_Config.TabIndex = 0;
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
            this.m_lblName.Location = new System.Drawing.Point(6, 5);
            this.m_lblName.MainFont = new System.Drawing.Font("맑은 고딕", 9F, System.Drawing.FontStyle.Bold);
            this.m_lblName.MainFontColor = System.Drawing.Color.Black;
            this.m_lblName.Margin = new System.Windows.Forms.Padding(1);
            this.m_lblName.Name = "m_lblName";
            this.m_lblName.Size = new System.Drawing.Size(138, 47);
            this.m_lblName.SubFont = new System.Drawing.Font("맑은 고딕", 10F);
            this.m_lblName.SubFontColor = System.Drawing.Color.Black;
            this.m_lblName.SubText = "";
            this.m_lblName.TabIndex = 1384;
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
            // m_labelLogType
            // 
            this.m_labelLogType.BackGroundColor = System.Drawing.Color.White;
            this.m_labelLogType.BorderStroke = 2;
            this.m_labelLogType.BorderStyle = System.Windows.Forms.ButtonBorderStyle.Solid;
            this._tableLayoutPanel_Config.SetColumnSpan(this.m_labelLogType, 3);
            this.m_labelLogType.Description = "";
            this.m_labelLogType.DisabledColor = System.Drawing.Color.LightGray;
            this.m_labelLogType.Dock = System.Windows.Forms.DockStyle.Fill;
            this.m_labelLogType.EdgeRadius = 1;
            this.m_labelLogType.Enabled = false;
            this.m_labelLogType.ImagePosition = new System.Drawing.Point(0, 0);
            this.m_labelLogType.ImageSize = new System.Drawing.Point(0, 0);
            this.m_labelLogType.LoadImage = null;
            this.m_labelLogType.Location = new System.Drawing.Point(146, 299);
            this.m_labelLogType.MainFont = new System.Drawing.Font("맑은 고딕", 11F, System.Drawing.FontStyle.Bold);
            this.m_labelLogType.MainFontColor = System.Drawing.Color.Black;
            this.m_labelLogType.Margin = new System.Windows.Forms.Padding(1);
            this.m_labelLogType.Name = "m_labelLogType";
            this.m_labelLogType.Size = new System.Drawing.Size(418, 47);
            this.m_labelLogType.SubFont = new System.Drawing.Font("맑은 고딕", 10F);
            this.m_labelLogType.SubFontColor = System.Drawing.Color.Black;
            this.m_labelLogType.SubText = "";
            this.m_labelLogType.TabIndex = 7;
            this.m_labelLogType.Text = "--";
            this.m_labelLogType.TextAlignMain = Sys3Controls.EN_TEXTALIGN.BOTTOM_RIGHT;
            this.m_labelLogType.TextAlignSub = Sys3Controls.EN_TEXTALIGN.MIDDLE_CENTER;
            this.m_labelLogType.TextAlignUnit = Sys3Controls.EN_TEXTALIGN.BOTTOM_RIGHT;
            this.m_labelLogType.ThemeIndex = 0;
            this.m_labelLogType.UnitAreaRate = 10;
            this.m_labelLogType.UnitFont = new System.Drawing.Font("맑은 고딕", 10F, System.Drawing.FontStyle.Bold);
            this.m_labelLogType.UnitFontColor = System.Drawing.Color.Black;
            this.m_labelLogType.UnitPositionVertical = false;
            this.m_labelLogType.UnitText = "ms";
            this.m_labelLogType.UseBorder = true;
            this.m_labelLogType.UseEdgeRadius = false;
            this.m_labelLogType.UseImage = false;
            this.m_labelLogType.UseSubFont = false;
            this.m_labelLogType.UseUnitFont = false;
            this.m_labelLogType.Click += new System.EventHandler(this.Click_Configuration);
            // 
            // m_lblProtocolType
            // 
            this.m_lblProtocolType.BackGroundColor = System.Drawing.Color.FromArgb(((int)(((byte)(200)))), ((int)(((byte)(200)))), ((int)(((byte)(220)))));
            this.m_lblProtocolType.BorderStroke = 2;
            this.m_lblProtocolType.BorderStyle = System.Windows.Forms.ButtonBorderStyle.Solid;
            this.m_lblProtocolType.Description = "";
            this.m_lblProtocolType.DisabledColor = System.Drawing.Color.DarkGray;
            this.m_lblProtocolType.Dock = System.Windows.Forms.DockStyle.Fill;
            this.m_lblProtocolType.EdgeRadius = 1;
            this.m_lblProtocolType.ImagePosition = new System.Drawing.Point(0, 0);
            this.m_lblProtocolType.ImageSize = new System.Drawing.Point(0, 0);
            this.m_lblProtocolType.LoadImage = null;
            this.m_lblProtocolType.Location = new System.Drawing.Point(6, 54);
            this.m_lblProtocolType.MainFont = new System.Drawing.Font("맑은 고딕", 9F, System.Drawing.FontStyle.Bold);
            this.m_lblProtocolType.MainFontColor = System.Drawing.Color.Black;
            this.m_lblProtocolType.Margin = new System.Windows.Forms.Padding(1);
            this.m_lblProtocolType.Name = "m_lblProtocolType";
            this.m_lblProtocolType.Size = new System.Drawing.Size(138, 47);
            this.m_lblProtocolType.SubFont = new System.Drawing.Font("맑은 고딕", 10F);
            this.m_lblProtocolType.SubFontColor = System.Drawing.Color.Black;
            this.m_lblProtocolType.SubText = "";
            this.m_lblProtocolType.TabIndex = 1385;
            this.m_lblProtocolType.Text = "PROTOCOL TYPE";
            this.m_lblProtocolType.TextAlignMain = Sys3Controls.EN_TEXTALIGN.TOP_LEFT;
            this.m_lblProtocolType.TextAlignSub = Sys3Controls.EN_TEXTALIGN.MIDDLE_CENTER;
            this.m_lblProtocolType.TextAlignUnit = Sys3Controls.EN_TEXTALIGN.MIDDLE_CENTER;
            this.m_lblProtocolType.ThemeIndex = 0;
            this.m_lblProtocolType.UnitAreaRate = 40;
            this.m_lblProtocolType.UnitFont = new System.Drawing.Font("맑은 고딕", 10F, System.Drawing.FontStyle.Bold);
            this.m_lblProtocolType.UnitFontColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.m_lblProtocolType.UnitPositionVertical = false;
            this.m_lblProtocolType.UnitText = "";
            this.m_lblProtocolType.UseBorder = true;
            this.m_lblProtocolType.UseEdgeRadius = false;
            this.m_lblProtocolType.UseImage = false;
            this.m_lblProtocolType.UseSubFont = false;
            this.m_lblProtocolType.UseUnitFont = false;
            // 
            // m_lblLogType
            // 
            this.m_lblLogType.BackGroundColor = System.Drawing.Color.FromArgb(((int)(((byte)(200)))), ((int)(((byte)(200)))), ((int)(((byte)(220)))));
            this.m_lblLogType.BorderStroke = 2;
            this.m_lblLogType.BorderStyle = System.Windows.Forms.ButtonBorderStyle.Solid;
            this.m_lblLogType.Description = "";
            this.m_lblLogType.DisabledColor = System.Drawing.Color.DarkGray;
            this.m_lblLogType.Dock = System.Windows.Forms.DockStyle.Fill;
            this.m_lblLogType.EdgeRadius = 1;
            this.m_lblLogType.ImagePosition = new System.Drawing.Point(0, 0);
            this.m_lblLogType.ImageSize = new System.Drawing.Point(0, 0);
            this.m_lblLogType.LoadImage = null;
            this.m_lblLogType.Location = new System.Drawing.Point(6, 299);
            this.m_lblLogType.MainFont = new System.Drawing.Font("맑은 고딕", 9F, System.Drawing.FontStyle.Bold);
            this.m_lblLogType.MainFontColor = System.Drawing.Color.Black;
            this.m_lblLogType.Margin = new System.Windows.Forms.Padding(1);
            this.m_lblLogType.Name = "m_lblLogType";
            this.m_lblLogType.Size = new System.Drawing.Size(138, 47);
            this.m_lblLogType.SubFont = new System.Drawing.Font("맑은 고딕", 9F, System.Drawing.FontStyle.Bold);
            this.m_lblLogType.SubFontColor = System.Drawing.Color.Indigo;
            this.m_lblLogType.SubText = "RECEIVE";
            this.m_lblLogType.TabIndex = 1398;
            this.m_lblLogType.Text = "LOG TYPE";
            this.m_lblLogType.TextAlignMain = Sys3Controls.EN_TEXTALIGN.TOP_LEFT;
            this.m_lblLogType.TextAlignSub = Sys3Controls.EN_TEXTALIGN.BOTTOM_RIGHT;
            this.m_lblLogType.TextAlignUnit = Sys3Controls.EN_TEXTALIGN.MIDDLE_CENTER;
            this.m_lblLogType.ThemeIndex = 0;
            this.m_lblLogType.UnitAreaRate = 40;
            this.m_lblLogType.UnitFont = new System.Drawing.Font("맑은 고딕", 10F, System.Drawing.FontStyle.Bold);
            this.m_lblLogType.UnitFontColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.m_lblLogType.UnitPositionVertical = false;
            this.m_lblLogType.UnitText = "";
            this.m_lblLogType.UseBorder = true;
            this.m_lblLogType.UseEdgeRadius = false;
            this.m_lblLogType.UseImage = false;
            this.m_lblLogType.UseSubFont = false;
            this.m_lblLogType.UseUnitFont = false;
            // 
            // m_lblServerIP
            // 
            this.m_lblServerIP.BackGroundColor = System.Drawing.Color.FromArgb(((int)(((byte)(200)))), ((int)(((byte)(200)))), ((int)(((byte)(220)))));
            this.m_lblServerIP.BorderStroke = 2;
            this.m_lblServerIP.BorderStyle = System.Windows.Forms.ButtonBorderStyle.Solid;
            this.m_lblServerIP.Description = "";
            this.m_lblServerIP.DisabledColor = System.Drawing.Color.DarkGray;
            this.m_lblServerIP.Dock = System.Windows.Forms.DockStyle.Fill;
            this.m_lblServerIP.EdgeRadius = 1;
            this.m_lblServerIP.ImagePosition = new System.Drawing.Point(0, 0);
            this.m_lblServerIP.ImageSize = new System.Drawing.Point(0, 0);
            this.m_lblServerIP.LoadImage = null;
            this.m_lblServerIP.Location = new System.Drawing.Point(6, 103);
            this.m_lblServerIP.MainFont = new System.Drawing.Font("맑은 고딕", 9F, System.Drawing.FontStyle.Bold);
            this.m_lblServerIP.MainFontColor = System.Drawing.Color.Black;
            this.m_lblServerIP.Margin = new System.Windows.Forms.Padding(1);
            this.m_lblServerIP.Name = "m_lblServerIP";
            this.m_lblServerIP.Size = new System.Drawing.Size(138, 47);
            this.m_lblServerIP.SubFont = new System.Drawing.Font("맑은 고딕", 10F);
            this.m_lblServerIP.SubFontColor = System.Drawing.Color.Black;
            this.m_lblServerIP.SubText = "";
            this.m_lblServerIP.TabIndex = 1386;
            this.m_lblServerIP.Text = "TARGET IP";
            this.m_lblServerIP.TextAlignMain = Sys3Controls.EN_TEXTALIGN.TOP_LEFT;
            this.m_lblServerIP.TextAlignSub = Sys3Controls.EN_TEXTALIGN.MIDDLE_CENTER;
            this.m_lblServerIP.TextAlignUnit = Sys3Controls.EN_TEXTALIGN.MIDDLE_CENTER;
            this.m_lblServerIP.ThemeIndex = 0;
            this.m_lblServerIP.UnitAreaRate = 40;
            this.m_lblServerIP.UnitFont = new System.Drawing.Font("맑은 고딕", 10F, System.Drawing.FontStyle.Bold);
            this.m_lblServerIP.UnitFontColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.m_lblServerIP.UnitPositionVertical = false;
            this.m_lblServerIP.UnitText = "";
            this.m_lblServerIP.UseBorder = true;
            this.m_lblServerIP.UseEdgeRadius = false;
            this.m_lblServerIP.UseImage = false;
            this.m_lblServerIP.UseSubFont = false;
            this.m_lblServerIP.UseUnitFont = false;
            // 
            // m_lblPort
            // 
            this.m_lblPort.BackGroundColor = System.Drawing.Color.FromArgb(((int)(((byte)(200)))), ((int)(((byte)(200)))), ((int)(((byte)(220)))));
            this.m_lblPort.BorderStroke = 2;
            this.m_lblPort.BorderStyle = System.Windows.Forms.ButtonBorderStyle.Solid;
            this.m_lblPort.Description = "";
            this.m_lblPort.DisabledColor = System.Drawing.Color.DarkGray;
            this.m_lblPort.Dock = System.Windows.Forms.DockStyle.Fill;
            this.m_lblPort.EdgeRadius = 1;
            this.m_lblPort.ImagePosition = new System.Drawing.Point(0, 0);
            this.m_lblPort.ImageSize = new System.Drawing.Point(0, 0);
            this.m_lblPort.LoadImage = null;
            this.m_lblPort.Location = new System.Drawing.Point(6, 152);
            this.m_lblPort.MainFont = new System.Drawing.Font("맑은 고딕", 9F, System.Drawing.FontStyle.Bold);
            this.m_lblPort.MainFontColor = System.Drawing.Color.Black;
            this.m_lblPort.Margin = new System.Windows.Forms.Padding(1);
            this.m_lblPort.Name = "m_lblPort";
            this.m_lblPort.Size = new System.Drawing.Size(138, 47);
            this.m_lblPort.SubFont = new System.Drawing.Font("맑은 고딕", 10F);
            this.m_lblPort.SubFontColor = System.Drawing.Color.Black;
            this.m_lblPort.SubText = "";
            this.m_lblPort.TabIndex = 1387;
            this.m_lblPort.Text = "PORT";
            this.m_lblPort.TextAlignMain = Sys3Controls.EN_TEXTALIGN.TOP_LEFT;
            this.m_lblPort.TextAlignSub = Sys3Controls.EN_TEXTALIGN.MIDDLE_CENTER;
            this.m_lblPort.TextAlignUnit = Sys3Controls.EN_TEXTALIGN.MIDDLE_CENTER;
            this.m_lblPort.ThemeIndex = 0;
            this.m_lblPort.UnitAreaRate = 40;
            this.m_lblPort.UnitFont = new System.Drawing.Font("맑은 고딕", 10F, System.Drawing.FontStyle.Bold);
            this.m_lblPort.UnitFontColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.m_lblPort.UnitPositionVertical = false;
            this.m_lblPort.UnitText = "";
            this.m_lblPort.UseBorder = true;
            this.m_lblPort.UseEdgeRadius = false;
            this.m_lblPort.UseImage = false;
            this.m_lblPort.UseSubFont = false;
            this.m_lblPort.UseUnitFont = false;
            // 
            // m_lblRetryInterval
            // 
            this.m_lblRetryInterval.BackGroundColor = System.Drawing.Color.FromArgb(((int)(((byte)(200)))), ((int)(((byte)(200)))), ((int)(((byte)(220)))));
            this.m_lblRetryInterval.BorderStroke = 2;
            this.m_lblRetryInterval.BorderStyle = System.Windows.Forms.ButtonBorderStyle.Solid;
            this.m_lblRetryInterval.Description = "";
            this.m_lblRetryInterval.DisabledColor = System.Drawing.Color.DarkGray;
            this.m_lblRetryInterval.Dock = System.Windows.Forms.DockStyle.Fill;
            this.m_lblRetryInterval.EdgeRadius = 1;
            this.m_lblRetryInterval.ImagePosition = new System.Drawing.Point(0, 0);
            this.m_lblRetryInterval.ImageSize = new System.Drawing.Point(0, 0);
            this.m_lblRetryInterval.LoadImage = null;
            this.m_lblRetryInterval.Location = new System.Drawing.Point(6, 201);
            this.m_lblRetryInterval.MainFont = new System.Drawing.Font("맑은 고딕", 9F, System.Drawing.FontStyle.Bold);
            this.m_lblRetryInterval.MainFontColor = System.Drawing.Color.Black;
            this.m_lblRetryInterval.Margin = new System.Windows.Forms.Padding(1);
            this.m_lblRetryInterval.Name = "m_lblRetryInterval";
            this.m_lblRetryInterval.Size = new System.Drawing.Size(138, 47);
            this.m_lblRetryInterval.SubFont = new System.Drawing.Font("맑은 고딕", 9F, System.Drawing.FontStyle.Bold);
            this.m_lblRetryInterval.SubFontColor = System.Drawing.Color.Indigo;
            this.m_lblRetryInterval.SubText = "CONNECT";
            this.m_lblRetryInterval.TabIndex = 1389;
            this.m_lblRetryInterval.Text = "RETRY INTERVAL";
            this.m_lblRetryInterval.TextAlignMain = Sys3Controls.EN_TEXTALIGN.TOP_LEFT;
            this.m_lblRetryInterval.TextAlignSub = Sys3Controls.EN_TEXTALIGN.BOTTOM_RIGHT;
            this.m_lblRetryInterval.TextAlignUnit = Sys3Controls.EN_TEXTALIGN.MIDDLE_CENTER;
            this.m_lblRetryInterval.ThemeIndex = 0;
            this.m_lblRetryInterval.UnitAreaRate = 40;
            this.m_lblRetryInterval.UnitFont = new System.Drawing.Font("맑은 고딕", 10F, System.Drawing.FontStyle.Bold);
            this.m_lblRetryInterval.UnitFontColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.m_lblRetryInterval.UnitPositionVertical = false;
            this.m_lblRetryInterval.UnitText = "";
            this.m_lblRetryInterval.UseBorder = true;
            this.m_lblRetryInterval.UseEdgeRadius = false;
            this.m_lblRetryInterval.UseImage = false;
            this.m_lblRetryInterval.UseSubFont = true;
            this.m_lblRetryInterval.UseUnitFont = false;
            // 
            // m_lblReceiveTimeout
            // 
            this.m_lblReceiveTimeout.BackGroundColor = System.Drawing.Color.FromArgb(((int)(((byte)(200)))), ((int)(((byte)(200)))), ((int)(((byte)(220)))));
            this.m_lblReceiveTimeout.BorderStroke = 2;
            this.m_lblReceiveTimeout.BorderStyle = System.Windows.Forms.ButtonBorderStyle.Solid;
            this.m_lblReceiveTimeout.Description = "";
            this.m_lblReceiveTimeout.DisabledColor = System.Drawing.Color.DarkGray;
            this.m_lblReceiveTimeout.Dock = System.Windows.Forms.DockStyle.Fill;
            this.m_lblReceiveTimeout.EdgeRadius = 1;
            this.m_lblReceiveTimeout.ImagePosition = new System.Drawing.Point(0, 0);
            this.m_lblReceiveTimeout.ImageSize = new System.Drawing.Point(0, 0);
            this.m_lblReceiveTimeout.LoadImage = null;
            this.m_lblReceiveTimeout.Location = new System.Drawing.Point(6, 250);
            this.m_lblReceiveTimeout.MainFont = new System.Drawing.Font("맑은 고딕", 9F, System.Drawing.FontStyle.Bold);
            this.m_lblReceiveTimeout.MainFontColor = System.Drawing.Color.Black;
            this.m_lblReceiveTimeout.Margin = new System.Windows.Forms.Padding(1);
            this.m_lblReceiveTimeout.Name = "m_lblReceiveTimeout";
            this.m_lblReceiveTimeout.Size = new System.Drawing.Size(138, 47);
            this.m_lblReceiveTimeout.SubFont = new System.Drawing.Font("맑은 고딕", 9F, System.Drawing.FontStyle.Bold);
            this.m_lblReceiveTimeout.SubFontColor = System.Drawing.Color.Indigo;
            this.m_lblReceiveTimeout.SubText = "RECEIVE";
            this.m_lblReceiveTimeout.TabIndex = 1390;
            this.m_lblReceiveTimeout.Text = "TIME OUT";
            this.m_lblReceiveTimeout.TextAlignMain = Sys3Controls.EN_TEXTALIGN.TOP_LEFT;
            this.m_lblReceiveTimeout.TextAlignSub = Sys3Controls.EN_TEXTALIGN.BOTTOM_RIGHT;
            this.m_lblReceiveTimeout.TextAlignUnit = Sys3Controls.EN_TEXTALIGN.MIDDLE_CENTER;
            this.m_lblReceiveTimeout.ThemeIndex = 0;
            this.m_lblReceiveTimeout.UnitAreaRate = 40;
            this.m_lblReceiveTimeout.UnitFont = new System.Drawing.Font("맑은 고딕", 10F, System.Drawing.FontStyle.Bold);
            this.m_lblReceiveTimeout.UnitFontColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.m_lblReceiveTimeout.UnitPositionVertical = false;
            this.m_lblReceiveTimeout.UnitText = "";
            this.m_lblReceiveTimeout.UseBorder = true;
            this.m_lblReceiveTimeout.UseEdgeRadius = false;
            this.m_lblReceiveTimeout.UseImage = false;
            this.m_lblReceiveTimeout.UseSubFont = true;
            this.m_lblReceiveTimeout.UseUnitFont = false;
            // 
            // m_labelName
            // 
            this.m_labelName.BackGroundColor = System.Drawing.Color.White;
            this.m_labelName.BorderStroke = 2;
            this.m_labelName.BorderStyle = System.Windows.Forms.ButtonBorderStyle.Solid;
            this._tableLayoutPanel_Config.SetColumnSpan(this.m_labelName, 3);
            this.m_labelName.Description = "";
            this.m_labelName.DisabledColor = System.Drawing.Color.LightGray;
            this.m_labelName.Dock = System.Windows.Forms.DockStyle.Fill;
            this.m_labelName.EdgeRadius = 1;
            this.m_labelName.Enabled = false;
            this.m_labelName.ImagePosition = new System.Drawing.Point(0, 0);
            this.m_labelName.ImageSize = new System.Drawing.Point(0, 0);
            this.m_labelName.LoadImage = null;
            this.m_labelName.Location = new System.Drawing.Point(146, 5);
            this.m_labelName.MainFont = new System.Drawing.Font("맑은 고딕", 11F, System.Drawing.FontStyle.Bold);
            this.m_labelName.MainFontColor = System.Drawing.Color.Black;
            this.m_labelName.Margin = new System.Windows.Forms.Padding(1);
            this.m_labelName.Name = "m_labelName";
            this.m_labelName.Size = new System.Drawing.Size(418, 47);
            this.m_labelName.SubFont = new System.Drawing.Font("맑은 고딕", 10F);
            this.m_labelName.SubFontColor = System.Drawing.Color.Black;
            this.m_labelName.SubText = "";
            this.m_labelName.TabIndex = 0;
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
            // m_labelProtocolType
            // 
            this.m_labelProtocolType.BackGroundColor = System.Drawing.Color.White;
            this.m_labelProtocolType.BorderStroke = 2;
            this.m_labelProtocolType.BorderStyle = System.Windows.Forms.ButtonBorderStyle.Solid;
            this._tableLayoutPanel_Config.SetColumnSpan(this.m_labelProtocolType, 3);
            this.m_labelProtocolType.Description = "";
            this.m_labelProtocolType.DisabledColor = System.Drawing.Color.LightGray;
            this.m_labelProtocolType.Dock = System.Windows.Forms.DockStyle.Fill;
            this.m_labelProtocolType.EdgeRadius = 1;
            this.m_labelProtocolType.Enabled = false;
            this.m_labelProtocolType.ImagePosition = new System.Drawing.Point(0, 0);
            this.m_labelProtocolType.ImageSize = new System.Drawing.Point(0, 0);
            this.m_labelProtocolType.LoadImage = null;
            this.m_labelProtocolType.Location = new System.Drawing.Point(146, 54);
            this.m_labelProtocolType.MainFont = new System.Drawing.Font("맑은 고딕", 11F, System.Drawing.FontStyle.Bold);
            this.m_labelProtocolType.MainFontColor = System.Drawing.Color.Black;
            this.m_labelProtocolType.Margin = new System.Windows.Forms.Padding(1);
            this.m_labelProtocolType.Name = "m_labelProtocolType";
            this.m_labelProtocolType.Size = new System.Drawing.Size(418, 47);
            this.m_labelProtocolType.SubFont = new System.Drawing.Font("맑은 고딕", 10F);
            this.m_labelProtocolType.SubFontColor = System.Drawing.Color.Black;
            this.m_labelProtocolType.SubText = "";
            this.m_labelProtocolType.TabIndex = 1;
            this.m_labelProtocolType.Text = "--";
            this.m_labelProtocolType.TextAlignMain = Sys3Controls.EN_TEXTALIGN.BOTTOM_RIGHT;
            this.m_labelProtocolType.TextAlignSub = Sys3Controls.EN_TEXTALIGN.MIDDLE_CENTER;
            this.m_labelProtocolType.TextAlignUnit = Sys3Controls.EN_TEXTALIGN.MIDDLE_CENTER;
            this.m_labelProtocolType.ThemeIndex = 0;
            this.m_labelProtocolType.UnitAreaRate = 40;
            this.m_labelProtocolType.UnitFont = new System.Drawing.Font("맑은 고딕", 10F, System.Drawing.FontStyle.Bold);
            this.m_labelProtocolType.UnitFontColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.m_labelProtocolType.UnitPositionVertical = false;
            this.m_labelProtocolType.UnitText = "";
            this.m_labelProtocolType.UseBorder = true;
            this.m_labelProtocolType.UseEdgeRadius = false;
            this.m_labelProtocolType.UseImage = false;
            this.m_labelProtocolType.UseSubFont = false;
            this.m_labelProtocolType.UseUnitFont = false;
            this.m_labelProtocolType.Click += new System.EventHandler(this.Click_Configuration);
            // 
            // m_labelServerIP
            // 
            this.m_labelServerIP.BackGroundColor = System.Drawing.Color.White;
            this.m_labelServerIP.BorderStroke = 2;
            this.m_labelServerIP.BorderStyle = System.Windows.Forms.ButtonBorderStyle.Solid;
            this._tableLayoutPanel_Config.SetColumnSpan(this.m_labelServerIP, 3);
            this.m_labelServerIP.Description = "";
            this.m_labelServerIP.DisabledColor = System.Drawing.Color.LightGray;
            this.m_labelServerIP.Dock = System.Windows.Forms.DockStyle.Fill;
            this.m_labelServerIP.EdgeRadius = 1;
            this.m_labelServerIP.Enabled = false;
            this.m_labelServerIP.ImagePosition = new System.Drawing.Point(0, 0);
            this.m_labelServerIP.ImageSize = new System.Drawing.Point(0, 0);
            this.m_labelServerIP.LoadImage = null;
            this.m_labelServerIP.Location = new System.Drawing.Point(146, 103);
            this.m_labelServerIP.MainFont = new System.Drawing.Font("맑은 고딕", 11F, System.Drawing.FontStyle.Bold);
            this.m_labelServerIP.MainFontColor = System.Drawing.Color.Black;
            this.m_labelServerIP.Margin = new System.Windows.Forms.Padding(1);
            this.m_labelServerIP.Name = "m_labelServerIP";
            this.m_labelServerIP.Size = new System.Drawing.Size(418, 47);
            this.m_labelServerIP.SubFont = new System.Drawing.Font("맑은 고딕", 10F);
            this.m_labelServerIP.SubFontColor = System.Drawing.Color.Black;
            this.m_labelServerIP.SubText = "";
            this.m_labelServerIP.TabIndex = 2;
            this.m_labelServerIP.Text = "--";
            this.m_labelServerIP.TextAlignMain = Sys3Controls.EN_TEXTALIGN.BOTTOM_RIGHT;
            this.m_labelServerIP.TextAlignSub = Sys3Controls.EN_TEXTALIGN.MIDDLE_CENTER;
            this.m_labelServerIP.TextAlignUnit = Sys3Controls.EN_TEXTALIGN.MIDDLE_CENTER;
            this.m_labelServerIP.ThemeIndex = 0;
            this.m_labelServerIP.UnitAreaRate = 40;
            this.m_labelServerIP.UnitFont = new System.Drawing.Font("맑은 고딕", 10F, System.Drawing.FontStyle.Bold);
            this.m_labelServerIP.UnitFontColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.m_labelServerIP.UnitPositionVertical = false;
            this.m_labelServerIP.UnitText = "";
            this.m_labelServerIP.UseBorder = true;
            this.m_labelServerIP.UseEdgeRadius = false;
            this.m_labelServerIP.UseImage = false;
            this.m_labelServerIP.UseSubFont = false;
            this.m_labelServerIP.UseUnitFont = false;
            this.m_labelServerIP.Click += new System.EventHandler(this.Click_Configuration);
            // 
            // m_labelPort
            // 
            this.m_labelPort.BackGroundColor = System.Drawing.Color.White;
            this.m_labelPort.BorderStroke = 2;
            this.m_labelPort.BorderStyle = System.Windows.Forms.ButtonBorderStyle.Solid;
            this.m_labelPort.Description = "";
            this.m_labelPort.DisabledColor = System.Drawing.Color.LightGray;
            this.m_labelPort.Dock = System.Windows.Forms.DockStyle.Fill;
            this.m_labelPort.EdgeRadius = 1;
            this.m_labelPort.Enabled = false;
            this.m_labelPort.ImagePosition = new System.Drawing.Point(0, 0);
            this.m_labelPort.ImageSize = new System.Drawing.Point(0, 0);
            this.m_labelPort.LoadImage = null;
            this.m_labelPort.Location = new System.Drawing.Point(146, 152);
            this.m_labelPort.MainFont = new System.Drawing.Font("맑은 고딕", 11F, System.Drawing.FontStyle.Bold);
            this.m_labelPort.MainFontColor = System.Drawing.Color.Black;
            this.m_labelPort.Margin = new System.Windows.Forms.Padding(1);
            this.m_labelPort.Name = "m_labelPort";
            this.m_labelPort.Size = new System.Drawing.Size(138, 47);
            this.m_labelPort.SubFont = new System.Drawing.Font("맑은 고딕", 10F);
            this.m_labelPort.SubFontColor = System.Drawing.Color.Black;
            this.m_labelPort.SubText = "";
            this.m_labelPort.TabIndex = 3;
            this.m_labelPort.Text = "--";
            this.m_labelPort.TextAlignMain = Sys3Controls.EN_TEXTALIGN.BOTTOM_RIGHT;
            this.m_labelPort.TextAlignSub = Sys3Controls.EN_TEXTALIGN.MIDDLE_CENTER;
            this.m_labelPort.TextAlignUnit = Sys3Controls.EN_TEXTALIGN.MIDDLE_CENTER;
            this.m_labelPort.ThemeIndex = 0;
            this.m_labelPort.UnitAreaRate = 40;
            this.m_labelPort.UnitFont = new System.Drawing.Font("맑은 고딕", 10F, System.Drawing.FontStyle.Bold);
            this.m_labelPort.UnitFontColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.m_labelPort.UnitPositionVertical = false;
            this.m_labelPort.UnitText = "";
            this.m_labelPort.UseBorder = true;
            this.m_labelPort.UseEdgeRadius = false;
            this.m_labelPort.UseImage = false;
            this.m_labelPort.UseSubFont = false;
            this.m_labelPort.UseUnitFont = false;
            this.m_labelPort.Click += new System.EventHandler(this.Click_Configuration);
            // 
            // m_lblTargetPort
            // 
            this.m_lblTargetPort.BackGroundColor = System.Drawing.Color.FromArgb(((int)(((byte)(200)))), ((int)(((byte)(200)))), ((int)(((byte)(220)))));
            this.m_lblTargetPort.BorderStroke = 2;
            this.m_lblTargetPort.BorderStyle = System.Windows.Forms.ButtonBorderStyle.Solid;
            this.m_lblTargetPort.Description = "";
            this.m_lblTargetPort.DisabledColor = System.Drawing.Color.DarkGray;
            this.m_lblTargetPort.Dock = System.Windows.Forms.DockStyle.Fill;
            this.m_lblTargetPort.EdgeRadius = 1;
            this.m_lblTargetPort.ImagePosition = new System.Drawing.Point(0, 0);
            this.m_lblTargetPort.ImageSize = new System.Drawing.Point(0, 0);
            this.m_lblTargetPort.LoadImage = null;
            this.m_lblTargetPort.Location = new System.Drawing.Point(286, 152);
            this.m_lblTargetPort.MainFont = new System.Drawing.Font("맑은 고딕", 9F, System.Drawing.FontStyle.Bold);
            this.m_lblTargetPort.MainFontColor = System.Drawing.Color.Black;
            this.m_lblTargetPort.Margin = new System.Windows.Forms.Padding(1);
            this.m_lblTargetPort.Name = "m_lblTargetPort";
            this.m_lblTargetPort.Size = new System.Drawing.Size(138, 47);
            this.m_lblTargetPort.SubFont = new System.Drawing.Font("맑은 고딕", 10F);
            this.m_lblTargetPort.SubFontColor = System.Drawing.Color.Black;
            this.m_lblTargetPort.SubText = "";
            this.m_lblTargetPort.TabIndex = 1388;
            this.m_lblTargetPort.Text = "TARGET PORT";
            this.m_lblTargetPort.TextAlignMain = Sys3Controls.EN_TEXTALIGN.TOP_LEFT;
            this.m_lblTargetPort.TextAlignSub = Sys3Controls.EN_TEXTALIGN.MIDDLE_CENTER;
            this.m_lblTargetPort.TextAlignUnit = Sys3Controls.EN_TEXTALIGN.MIDDLE_CENTER;
            this.m_lblTargetPort.ThemeIndex = 0;
            this.m_lblTargetPort.UnitAreaRate = 40;
            this.m_lblTargetPort.UnitFont = new System.Drawing.Font("맑은 고딕", 10F, System.Drawing.FontStyle.Bold);
            this.m_lblTargetPort.UnitFontColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.m_lblTargetPort.UnitPositionVertical = false;
            this.m_lblTargetPort.UnitText = "";
            this.m_lblTargetPort.UseBorder = true;
            this.m_lblTargetPort.UseEdgeRadius = false;
            this.m_lblTargetPort.UseImage = false;
            this.m_lblTargetPort.UseSubFont = false;
            this.m_lblTargetPort.UseUnitFont = false;
            // 
            // m_labelReceiveTimeout
            // 
            this.m_labelReceiveTimeout.BackGroundColor = System.Drawing.Color.White;
            this.m_labelReceiveTimeout.BorderStroke = 2;
            this.m_labelReceiveTimeout.BorderStyle = System.Windows.Forms.ButtonBorderStyle.Solid;
            this._tableLayoutPanel_Config.SetColumnSpan(this.m_labelReceiveTimeout, 3);
            this.m_labelReceiveTimeout.Description = "";
            this.m_labelReceiveTimeout.DisabledColor = System.Drawing.Color.LightGray;
            this.m_labelReceiveTimeout.Dock = System.Windows.Forms.DockStyle.Fill;
            this.m_labelReceiveTimeout.EdgeRadius = 1;
            this.m_labelReceiveTimeout.Enabled = false;
            this.m_labelReceiveTimeout.ImagePosition = new System.Drawing.Point(0, 0);
            this.m_labelReceiveTimeout.ImageSize = new System.Drawing.Point(0, 0);
            this.m_labelReceiveTimeout.LoadImage = null;
            this.m_labelReceiveTimeout.Location = new System.Drawing.Point(146, 250);
            this.m_labelReceiveTimeout.MainFont = new System.Drawing.Font("맑은 고딕", 11F, System.Drawing.FontStyle.Bold);
            this.m_labelReceiveTimeout.MainFontColor = System.Drawing.Color.Black;
            this.m_labelReceiveTimeout.Margin = new System.Windows.Forms.Padding(1);
            this.m_labelReceiveTimeout.Name = "m_labelReceiveTimeout";
            this.m_labelReceiveTimeout.Size = new System.Drawing.Size(418, 47);
            this.m_labelReceiveTimeout.SubFont = new System.Drawing.Font("맑은 고딕", 10F);
            this.m_labelReceiveTimeout.SubFontColor = System.Drawing.Color.Black;
            this.m_labelReceiveTimeout.SubText = "";
            this.m_labelReceiveTimeout.TabIndex = 6;
            this.m_labelReceiveTimeout.Text = "--";
            this.m_labelReceiveTimeout.TextAlignMain = Sys3Controls.EN_TEXTALIGN.BOTTOM_RIGHT;
            this.m_labelReceiveTimeout.TextAlignSub = Sys3Controls.EN_TEXTALIGN.MIDDLE_CENTER;
            this.m_labelReceiveTimeout.TextAlignUnit = Sys3Controls.EN_TEXTALIGN.BOTTOM_RIGHT;
            this.m_labelReceiveTimeout.ThemeIndex = 0;
            this.m_labelReceiveTimeout.UnitAreaRate = 10;
            this.m_labelReceiveTimeout.UnitFont = new System.Drawing.Font("맑은 고딕", 10F, System.Drawing.FontStyle.Bold);
            this.m_labelReceiveTimeout.UnitFontColor = System.Drawing.Color.Black;
            this.m_labelReceiveTimeout.UnitPositionVertical = false;
            this.m_labelReceiveTimeout.UnitText = "ms";
            this.m_labelReceiveTimeout.UseBorder = true;
            this.m_labelReceiveTimeout.UseEdgeRadius = false;
            this.m_labelReceiveTimeout.UseImage = false;
            this.m_labelReceiveTimeout.UseSubFont = false;
            this.m_labelReceiveTimeout.UseUnitFont = true;
            this.m_labelReceiveTimeout.Click += new System.EventHandler(this.Click_Configuration);
            // 
            // m_labelTargetPort
            // 
            this.m_labelTargetPort.BackGroundColor = System.Drawing.Color.White;
            this.m_labelTargetPort.BorderStroke = 2;
            this.m_labelTargetPort.BorderStyle = System.Windows.Forms.ButtonBorderStyle.Solid;
            this.m_labelTargetPort.Description = "";
            this.m_labelTargetPort.DisabledColor = System.Drawing.Color.LightGray;
            this.m_labelTargetPort.Dock = System.Windows.Forms.DockStyle.Fill;
            this.m_labelTargetPort.EdgeRadius = 1;
            this.m_labelTargetPort.Enabled = false;
            this.m_labelTargetPort.ImagePosition = new System.Drawing.Point(0, 0);
            this.m_labelTargetPort.ImageSize = new System.Drawing.Point(0, 0);
            this.m_labelTargetPort.LoadImage = null;
            this.m_labelTargetPort.Location = new System.Drawing.Point(426, 152);
            this.m_labelTargetPort.MainFont = new System.Drawing.Font("맑은 고딕", 11F, System.Drawing.FontStyle.Bold);
            this.m_labelTargetPort.MainFontColor = System.Drawing.Color.Black;
            this.m_labelTargetPort.Margin = new System.Windows.Forms.Padding(1);
            this.m_labelTargetPort.Name = "m_labelTargetPort";
            this.m_labelTargetPort.Size = new System.Drawing.Size(138, 47);
            this.m_labelTargetPort.SubFont = new System.Drawing.Font("맑은 고딕", 10F);
            this.m_labelTargetPort.SubFontColor = System.Drawing.Color.Black;
            this.m_labelTargetPort.SubText = "";
            this.m_labelTargetPort.TabIndex = 4;
            this.m_labelTargetPort.Text = "--";
            this.m_labelTargetPort.TextAlignMain = Sys3Controls.EN_TEXTALIGN.BOTTOM_RIGHT;
            this.m_labelTargetPort.TextAlignSub = Sys3Controls.EN_TEXTALIGN.MIDDLE_CENTER;
            this.m_labelTargetPort.TextAlignUnit = Sys3Controls.EN_TEXTALIGN.MIDDLE_CENTER;
            this.m_labelTargetPort.ThemeIndex = 0;
            this.m_labelTargetPort.UnitAreaRate = 40;
            this.m_labelTargetPort.UnitFont = new System.Drawing.Font("맑은 고딕", 10F, System.Drawing.FontStyle.Bold);
            this.m_labelTargetPort.UnitFontColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.m_labelTargetPort.UnitPositionVertical = false;
            this.m_labelTargetPort.UnitText = "";
            this.m_labelTargetPort.UseBorder = true;
            this.m_labelTargetPort.UseEdgeRadius = false;
            this.m_labelTargetPort.UseImage = false;
            this.m_labelTargetPort.UseSubFont = false;
            this.m_labelTargetPort.UseUnitFont = false;
            this.m_labelTargetPort.Click += new System.EventHandler(this.Click_Configuration);
            // 
            // m_labelRetryInterval
            // 
            this.m_labelRetryInterval.BackGroundColor = System.Drawing.Color.White;
            this.m_labelRetryInterval.BorderStroke = 2;
            this.m_labelRetryInterval.BorderStyle = System.Windows.Forms.ButtonBorderStyle.Solid;
            this._tableLayoutPanel_Config.SetColumnSpan(this.m_labelRetryInterval, 3);
            this.m_labelRetryInterval.Description = "";
            this.m_labelRetryInterval.DisabledColor = System.Drawing.Color.LightGray;
            this.m_labelRetryInterval.Dock = System.Windows.Forms.DockStyle.Fill;
            this.m_labelRetryInterval.EdgeRadius = 1;
            this.m_labelRetryInterval.Enabled = false;
            this.m_labelRetryInterval.ImagePosition = new System.Drawing.Point(0, 0);
            this.m_labelRetryInterval.ImageSize = new System.Drawing.Point(0, 0);
            this.m_labelRetryInterval.LoadImage = null;
            this.m_labelRetryInterval.Location = new System.Drawing.Point(146, 201);
            this.m_labelRetryInterval.MainFont = new System.Drawing.Font("맑은 고딕", 11F, System.Drawing.FontStyle.Bold);
            this.m_labelRetryInterval.MainFontColor = System.Drawing.Color.Black;
            this.m_labelRetryInterval.Margin = new System.Windows.Forms.Padding(1);
            this.m_labelRetryInterval.Name = "m_labelRetryInterval";
            this.m_labelRetryInterval.Size = new System.Drawing.Size(418, 47);
            this.m_labelRetryInterval.SubFont = new System.Drawing.Font("맑은 고딕", 10F);
            this.m_labelRetryInterval.SubFontColor = System.Drawing.Color.Black;
            this.m_labelRetryInterval.SubText = "";
            this.m_labelRetryInterval.TabIndex = 5;
            this.m_labelRetryInterval.Text = "--";
            this.m_labelRetryInterval.TextAlignMain = Sys3Controls.EN_TEXTALIGN.BOTTOM_RIGHT;
            this.m_labelRetryInterval.TextAlignSub = Sys3Controls.EN_TEXTALIGN.MIDDLE_CENTER;
            this.m_labelRetryInterval.TextAlignUnit = Sys3Controls.EN_TEXTALIGN.MIDDLE_CENTER;
            this.m_labelRetryInterval.ThemeIndex = 0;
            this.m_labelRetryInterval.UnitAreaRate = 40;
            this.m_labelRetryInterval.UnitFont = new System.Drawing.Font("맑은 고딕", 10F, System.Drawing.FontStyle.Bold);
            this.m_labelRetryInterval.UnitFontColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.m_labelRetryInterval.UnitPositionVertical = false;
            this.m_labelRetryInterval.UnitText = "";
            this.m_labelRetryInterval.UseBorder = true;
            this.m_labelRetryInterval.UseEdgeRadius = false;
            this.m_labelRetryInterval.UseImage = false;
            this.m_labelRetryInterval.UseSubFont = false;
            this.m_labelRetryInterval.UseUnitFont = false;
            this.m_labelRetryInterval.Click += new System.EventHandler(this.Click_Configuration);
            // 
            // m_groupSocketDemonstration
            // 
            this.m_groupSocketDemonstration.BackGroundColor = System.Drawing.Color.WhiteSmoke;
            this.m_groupSocketDemonstration.Controls.Add(this.tableLayoutPanel1);
            this.m_groupSocketDemonstration.Dock = System.Windows.Forms.DockStyle.Fill;
            this.m_groupSocketDemonstration.EdgeBorderStroke = 2;
            this.m_groupSocketDemonstration.EdgeRadius = 2;
            this.m_groupSocketDemonstration.LabelFont = new System.Drawing.Font("맑은 고딕", 10F, System.Drawing.FontStyle.Bold);
            this.m_groupSocketDemonstration.LabelGradientColorFirst = System.Drawing.Color.FromArgb(((int)(((byte)(208)))), ((int)(((byte)(208)))), ((int)(((byte)(208)))));
            this.m_groupSocketDemonstration.LabelGradientColorSecond = System.Drawing.Color.FromArgb(((int)(((byte)(240)))), ((int)(((byte)(240)))), ((int)(((byte)(240)))));
            this.m_groupSocketDemonstration.LabelHeight = 30;
            this.m_groupSocketDemonstration.LabelTextColor = System.Drawing.Color.Black;
            this.m_groupSocketDemonstration.Location = new System.Drawing.Point(578, 370);
            this.m_groupSocketDemonstration.Margin = new System.Windows.Forms.Padding(0);
            this.m_groupSocketDemonstration.Name = "m_groupSocketDemonstration";
            this.m_groupSocketDemonstration.Padding = new System.Windows.Forms.Padding(3, 18, 3, 3);
            this.m_groupSocketDemonstration.Size = new System.Drawing.Size(578, 476);
            this.m_groupSocketDemonstration.TabIndex = 1383;
            this.m_groupSocketDemonstration.TabStop = false;
            this.m_groupSocketDemonstration.Text = "SOCKET DEMONSTRATION";
            this.m_groupSocketDemonstration.TextAlign = Sys3Controls.EN_TEXTALIGN.MIDDLE_CENTER;
            this.m_groupSocketDemonstration.ThemeIndex = 0;
            this.m_groupSocketDemonstration.UseLabelBorder = true;
            this.m_groupSocketDemonstration.UseTitle = true;
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.BackColor = System.Drawing.Color.WhiteSmoke;
            this.tableLayoutPanel1.ColumnCount = 6;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 1.020352F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 24.48982F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 24.48982F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 24.48982F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 24.48982F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 1.020352F));
            this.tableLayoutPanel1.Controls.Add(this.m_btnConnect, 1, 9);
            this.tableLayoutPanel1.Controls.Add(this.m_btnDisconnect, 2, 9);
            this.tableLayoutPanel1.Controls.Add(this.m_btnClearMessage, 4, 9);
            this.tableLayoutPanel1.Controls.Add(this.m_lblMessage, 1, 7);
            this.tableLayoutPanel1.Controls.Add(this.m_labelMessage, 2, 7);
            this.tableLayoutPanel1.Controls.Add(this.m_listMessage, 1, 1);
            this.tableLayoutPanel1.Controls.Add(this.m_btnSend, 4, 7);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(3, 32);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 11;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 1.027651F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 11.1568F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 11.1568F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 11.1568F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 11.1568F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 11.1568F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 11.1568F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 11.1568F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 2.411873F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 17.43523F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 1.027651F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(572, 441);
            this.tableLayoutPanel1.TabIndex = 1;
            // 
            // m_labelMessage
            // 
            this.m_labelMessage.BackGroundColor = System.Drawing.Color.White;
            this.m_labelMessage.BorderStroke = 2;
            this.m_labelMessage.BorderStyle = System.Windows.Forms.ButtonBorderStyle.Solid;
            this.tableLayoutPanel1.SetColumnSpan(this.m_labelMessage, 2);
            this.m_labelMessage.Description = "";
            this.m_labelMessage.DisabledColor = System.Drawing.Color.LightGray;
            this.m_labelMessage.Dock = System.Windows.Forms.DockStyle.Fill;
            this.m_labelMessage.EdgeRadius = 1;
            this.m_labelMessage.Enabled = false;
            this.m_labelMessage.ImagePosition = new System.Drawing.Point(0, 0);
            this.m_labelMessage.ImageSize = new System.Drawing.Point(0, 0);
            this.m_labelMessage.LoadImage = null;
            this.m_labelMessage.Location = new System.Drawing.Point(146, 299);
            this.m_labelMessage.MainFont = new System.Drawing.Font("맑은 고딕", 11F, System.Drawing.FontStyle.Bold);
            this.m_labelMessage.MainFontColor = System.Drawing.Color.Black;
            this.m_labelMessage.Margin = new System.Windows.Forms.Padding(1);
            this.m_labelMessage.Name = "m_labelMessage";
            this.m_labelMessage.Size = new System.Drawing.Size(278, 47);
            this.m_labelMessage.SubFont = new System.Drawing.Font("맑은 고딕", 10F);
            this.m_labelMessage.SubFontColor = System.Drawing.Color.Black;
            this.m_labelMessage.SubText = "";
            this.m_labelMessage.TabIndex = 0;
            this.m_labelMessage.Text = "--";
            this.m_labelMessage.TextAlignMain = Sys3Controls.EN_TEXTALIGN.BOTTOM_RIGHT;
            this.m_labelMessage.TextAlignSub = Sys3Controls.EN_TEXTALIGN.MIDDLE_CENTER;
            this.m_labelMessage.TextAlignUnit = Sys3Controls.EN_TEXTALIGN.MIDDLE_CENTER;
            this.m_labelMessage.ThemeIndex = 0;
            this.m_labelMessage.UnitAreaRate = 40;
            this.m_labelMessage.UnitFont = new System.Drawing.Font("맑은 고딕", 10F, System.Drawing.FontStyle.Bold);
            this.m_labelMessage.UnitFontColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.m_labelMessage.UnitPositionVertical = false;
            this.m_labelMessage.UnitText = "";
            this.m_labelMessage.UseBorder = true;
            this.m_labelMessage.UseEdgeRadius = false;
            this.m_labelMessage.UseImage = false;
            this.m_labelMessage.UseSubFont = false;
            this.m_labelMessage.UseUnitFont = false;
            this.m_labelMessage.Click += new System.EventHandler(this.Click_SendOrClearMessage);
            // 
            // m_lblMessage
            // 
            this.m_lblMessage.BackGroundColor = System.Drawing.Color.FromArgb(((int)(((byte)(200)))), ((int)(((byte)(200)))), ((int)(((byte)(220)))));
            this.m_lblMessage.BorderStroke = 2;
            this.m_lblMessage.BorderStyle = System.Windows.Forms.ButtonBorderStyle.Solid;
            this.m_lblMessage.Description = "";
            this.m_lblMessage.DisabledColor = System.Drawing.Color.DarkGray;
            this.m_lblMessage.Dock = System.Windows.Forms.DockStyle.Fill;
            this.m_lblMessage.EdgeRadius = 1;
            this.m_lblMessage.ImagePosition = new System.Drawing.Point(0, 0);
            this.m_lblMessage.ImageSize = new System.Drawing.Point(0, 0);
            this.m_lblMessage.LoadImage = null;
            this.m_lblMessage.Location = new System.Drawing.Point(6, 299);
            this.m_lblMessage.MainFont = new System.Drawing.Font("맑은 고딕", 9F, System.Drawing.FontStyle.Bold);
            this.m_lblMessage.MainFontColor = System.Drawing.Color.Black;
            this.m_lblMessage.Margin = new System.Windows.Forms.Padding(1);
            this.m_lblMessage.Name = "m_lblMessage";
            this.m_lblMessage.Size = new System.Drawing.Size(138, 47);
            this.m_lblMessage.SubFont = new System.Drawing.Font("맑은 고딕", 10F);
            this.m_lblMessage.SubFontColor = System.Drawing.Color.Black;
            this.m_lblMessage.SubText = "";
            this.m_lblMessage.TabIndex = 1396;
            this.m_lblMessage.Text = "MESSAGE";
            this.m_lblMessage.TextAlignMain = Sys3Controls.EN_TEXTALIGN.TOP_LEFT;
            this.m_lblMessage.TextAlignSub = Sys3Controls.EN_TEXTALIGN.MIDDLE_CENTER;
            this.m_lblMessage.TextAlignUnit = Sys3Controls.EN_TEXTALIGN.MIDDLE_CENTER;
            this.m_lblMessage.ThemeIndex = 0;
            this.m_lblMessage.UnitAreaRate = 40;
            this.m_lblMessage.UnitFont = new System.Drawing.Font("맑은 고딕", 10F, System.Drawing.FontStyle.Bold);
            this.m_lblMessage.UnitFontColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.m_lblMessage.UnitPositionVertical = false;
            this.m_lblMessage.UnitText = "";
            this.m_lblMessage.UseBorder = true;
            this.m_lblMessage.UseEdgeRadius = false;
            this.m_lblMessage.UseImage = false;
            this.m_lblMessage.UseSubFont = false;
            this.m_lblMessage.UseUnitFont = false;
            // 
            // m_btnDisconnect
            // 
            this.m_btnDisconnect.BorderWidth = 3;
            this.m_btnDisconnect.ButtonClicked = false;
            this.m_btnDisconnect.ClickedEmphasizeTextColor = System.Drawing.Color.White;
            this.m_btnDisconnect.CustomClickedGradientFirstColor = System.Drawing.Color.White;
            this.m_btnDisconnect.CustomClickedGradientSecondColor = System.Drawing.Color.White;
            this.m_btnDisconnect.Description = "";
            this.m_btnDisconnect.DisabledColor = System.Drawing.Color.DarkGray;
            this.m_btnDisconnect.Dock = System.Windows.Forms.DockStyle.Fill;
            this.m_btnDisconnect.EdgeRadius = 5;
            this.m_btnDisconnect.Enabled = false;
            this.m_btnDisconnect.GradientAngle = 70F;
            this.m_btnDisconnect.GradientFirstColor = System.Drawing.Color.White;
            this.m_btnDisconnect.GradientSecondColor = System.Drawing.Color.FromArgb(((int)(((byte)(170)))), ((int)(((byte)(176)))), ((int)(((byte)(183)))));
            this.m_btnDisconnect.HoverEmphasizeCustomColor = System.Drawing.Color.White;
            this.m_btnDisconnect.ImagePosition = new System.Drawing.Point(7, 7);
            this.m_btnDisconnect.ImageSize = new System.Drawing.Point(30, 30);
            this.m_btnDisconnect.LoadImage = global::FrameOfSystem3.Properties.Resources.DISCONNECT_BLACK;
            this.m_btnDisconnect.Location = new System.Drawing.Point(148, 360);
            this.m_btnDisconnect.MainFont = new System.Drawing.Font("맑은 고딕", 11F, System.Drawing.FontStyle.Bold);
            this.m_btnDisconnect.MainFontColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(36)))), ((int)(((byte)(0)))));
            this.m_btnDisconnect.Name = "m_btnDisconnect";
            this.m_btnDisconnect.Size = new System.Drawing.Size(134, 70);
            this.m_btnDisconnect.SubFont = new System.Drawing.Font("맑은 고딕", 8F, System.Drawing.FontStyle.Bold);
            this.m_btnDisconnect.SubFontColor = System.Drawing.Color.DarkBlue;
            this.m_btnDisconnect.SubText = "STATUS";
            this.m_btnDisconnect.TabIndex = 1;
            this.m_btnDisconnect.Text = "DISCONNECT";
            this.m_btnDisconnect.TextAlignMain = Sys3Controls.EN_TEXTALIGN.MIDDLE_RIGHT;
            this.m_btnDisconnect.TextAlignSub = Sys3Controls.EN_TEXTALIGN.TOP_RIGHT;
            this.m_btnDisconnect.ThemeIndex = 0;
            this.m_btnDisconnect.UseBorder = true;
            this.m_btnDisconnect.UseClickedEmphasizeTextColor = false;
            this.m_btnDisconnect.UseCustomizeClickedColor = false;
            this.m_btnDisconnect.UseEdge = true;
            this.m_btnDisconnect.UseHoverEmphasizeCustomColor = false;
            this.m_btnDisconnect.UseImage = true;
            this.m_btnDisconnect.UserHoverEmpahsize = false;
            this.m_btnDisconnect.UseSubFont = true;
            this.m_btnDisconnect.Click += new System.EventHandler(this.Click_ConnectOrDisconnect);
            // 
            // m_btnConnect
            // 
            this.m_btnConnect.BorderWidth = 3;
            this.m_btnConnect.ButtonClicked = false;
            this.m_btnConnect.ClickedEmphasizeTextColor = System.Drawing.Color.White;
            this.m_btnConnect.CustomClickedGradientFirstColor = System.Drawing.Color.White;
            this.m_btnConnect.CustomClickedGradientSecondColor = System.Drawing.Color.White;
            this.m_btnConnect.Description = "";
            this.m_btnConnect.DisabledColor = System.Drawing.Color.DarkGray;
            this.m_btnConnect.Dock = System.Windows.Forms.DockStyle.Fill;
            this.m_btnConnect.EdgeRadius = 5;
            this.m_btnConnect.Enabled = false;
            this.m_btnConnect.GradientAngle = 70F;
            this.m_btnConnect.GradientFirstColor = System.Drawing.Color.White;
            this.m_btnConnect.GradientSecondColor = System.Drawing.Color.FromArgb(((int)(((byte)(170)))), ((int)(((byte)(176)))), ((int)(((byte)(183)))));
            this.m_btnConnect.HoverEmphasizeCustomColor = System.Drawing.Color.White;
            this.m_btnConnect.ImagePosition = new System.Drawing.Point(7, 7);
            this.m_btnConnect.ImageSize = new System.Drawing.Point(30, 30);
            this.m_btnConnect.LoadImage = global::FrameOfSystem3.Properties.Resources.CONNECT_BLACK;
            this.m_btnConnect.Location = new System.Drawing.Point(8, 360);
            this.m_btnConnect.MainFont = new System.Drawing.Font("맑은 고딕", 13F, System.Drawing.FontStyle.Bold);
            this.m_btnConnect.MainFontColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(36)))), ((int)(((byte)(0)))));
            this.m_btnConnect.Name = "m_btnConnect";
            this.m_btnConnect.Size = new System.Drawing.Size(134, 70);
            this.m_btnConnect.SubFont = new System.Drawing.Font("맑은 고딕", 8F, System.Drawing.FontStyle.Bold);
            this.m_btnConnect.SubFontColor = System.Drawing.Color.DarkBlue;
            this.m_btnConnect.SubText = "STATUS";
            this.m_btnConnect.TabIndex = 0;
            this.m_btnConnect.Text = "CONNECT";
            this.m_btnConnect.TextAlignMain = Sys3Controls.EN_TEXTALIGN.MIDDLE_RIGHT;
            this.m_btnConnect.TextAlignSub = Sys3Controls.EN_TEXTALIGN.TOP_RIGHT;
            this.m_btnConnect.ThemeIndex = 0;
            this.m_btnConnect.UseBorder = true;
            this.m_btnConnect.UseClickedEmphasizeTextColor = false;
            this.m_btnConnect.UseCustomizeClickedColor = false;
            this.m_btnConnect.UseEdge = true;
            this.m_btnConnect.UseHoverEmphasizeCustomColor = false;
            this.m_btnConnect.UseImage = true;
            this.m_btnConnect.UserHoverEmpahsize = false;
            this.m_btnConnect.UseSubFont = true;
            this.m_btnConnect.Click += new System.EventHandler(this.Click_ConnectOrDisconnect);
            // 
            // m_btnSend
            // 
            this.m_btnSend.BorderWidth = 3;
            this.m_btnSend.ButtonClicked = false;
            this.m_btnSend.ClickedEmphasizeTextColor = System.Drawing.Color.White;
            this.m_btnSend.CustomClickedGradientFirstColor = System.Drawing.Color.White;
            this.m_btnSend.CustomClickedGradientSecondColor = System.Drawing.Color.White;
            this.m_btnSend.Description = "";
            this.m_btnSend.DisabledColor = System.Drawing.Color.DarkGray;
            this.m_btnSend.Dock = System.Windows.Forms.DockStyle.Fill;
            this.m_btnSend.EdgeRadius = 5;
            this.m_btnSend.Enabled = false;
            this.m_btnSend.GradientAngle = 70F;
            this.m_btnSend.GradientFirstColor = System.Drawing.Color.White;
            this.m_btnSend.GradientSecondColor = System.Drawing.Color.FromArgb(((int)(((byte)(170)))), ((int)(((byte)(176)))), ((int)(((byte)(183)))));
            this.m_btnSend.HoverEmphasizeCustomColor = System.Drawing.Color.White;
            this.m_btnSend.ImagePosition = new System.Drawing.Point(8, 4);
            this.m_btnSend.ImageSize = new System.Drawing.Point(30, 30);
            this.m_btnSend.LoadImage = global::FrameOfSystem3.Properties.Resources.SEND_BLACK;
            this.m_btnSend.Location = new System.Drawing.Point(428, 301);
            this.m_btnSend.MainFont = new System.Drawing.Font("맑은 고딕", 13F, System.Drawing.FontStyle.Bold);
            this.m_btnSend.MainFontColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(36)))), ((int)(((byte)(0)))));
            this.m_btnSend.Name = "m_btnSend";
            this.m_btnSend.Size = new System.Drawing.Size(134, 43);
            this.m_btnSend.SubFont = new System.Drawing.Font("맑은 고딕", 8F, System.Drawing.FontStyle.Bold);
            this.m_btnSend.SubFontColor = System.Drawing.Color.DarkBlue;
            this.m_btnSend.SubText = "STATUS";
            this.m_btnSend.TabIndex = 1;
            this.m_btnSend.Text = "SEND";
            this.m_btnSend.TextAlignMain = Sys3Controls.EN_TEXTALIGN.MIDDLE_RIGHT;
            this.m_btnSend.TextAlignSub = Sys3Controls.EN_TEXTALIGN.TOP_LEFT;
            this.m_btnSend.ThemeIndex = 0;
            this.m_btnSend.UseBorder = true;
            this.m_btnSend.UseClickedEmphasizeTextColor = false;
            this.m_btnSend.UseCustomizeClickedColor = false;
            this.m_btnSend.UseEdge = true;
            this.m_btnSend.UseHoverEmphasizeCustomColor = false;
            this.m_btnSend.UseImage = true;
            this.m_btnSend.UserHoverEmpahsize = false;
            this.m_btnSend.UseSubFont = false;
            this.m_btnSend.Click += new System.EventHandler(this.Click_SendOrClearMessage);
            // 
            // m_btnClearMessage
            // 
            this.m_btnClearMessage.BorderWidth = 3;
            this.m_btnClearMessage.ButtonClicked = false;
            this.m_btnClearMessage.ClickedEmphasizeTextColor = System.Drawing.Color.White;
            this.m_btnClearMessage.CustomClickedGradientFirstColor = System.Drawing.Color.White;
            this.m_btnClearMessage.CustomClickedGradientSecondColor = System.Drawing.Color.White;
            this.m_btnClearMessage.Description = "";
            this.m_btnClearMessage.DisabledColor = System.Drawing.Color.DarkGray;
            this.m_btnClearMessage.Dock = System.Windows.Forms.DockStyle.Fill;
            this.m_btnClearMessage.EdgeRadius = 5;
            this.m_btnClearMessage.Enabled = false;
            this.m_btnClearMessage.GradientAngle = 70F;
            this.m_btnClearMessage.GradientFirstColor = System.Drawing.Color.White;
            this.m_btnClearMessage.GradientSecondColor = System.Drawing.Color.FromArgb(((int)(((byte)(170)))), ((int)(((byte)(176)))), ((int)(((byte)(183)))));
            this.m_btnClearMessage.HoverEmphasizeCustomColor = System.Drawing.Color.White;
            this.m_btnClearMessage.ImagePosition = new System.Drawing.Point(7, 7);
            this.m_btnClearMessage.ImageSize = new System.Drawing.Point(30, 30);
            this.m_btnClearMessage.LoadImage = global::FrameOfSystem3.Properties.Resources.CONFIG_ADD3;
            this.m_btnClearMessage.Location = new System.Drawing.Point(428, 360);
            this.m_btnClearMessage.MainFont = new System.Drawing.Font("맑은 고딕", 13F, System.Drawing.FontStyle.Bold);
            this.m_btnClearMessage.MainFontColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(36)))), ((int)(((byte)(0)))));
            this.m_btnClearMessage.Name = "m_btnClearMessage";
            this.m_btnClearMessage.Size = new System.Drawing.Size(134, 70);
            this.m_btnClearMessage.SubFont = new System.Drawing.Font("맑은 고딕", 8F, System.Drawing.FontStyle.Bold);
            this.m_btnClearMessage.SubFontColor = System.Drawing.Color.DarkBlue;
            this.m_btnClearMessage.SubText = "STATUS";
            this.m_btnClearMessage.TabIndex = 2;
            this.m_btnClearMessage.Text = "CLEAR MESSAGE";
            this.m_btnClearMessage.TextAlignMain = Sys3Controls.EN_TEXTALIGN.MIDDLE_RIGHT;
            this.m_btnClearMessage.TextAlignSub = Sys3Controls.EN_TEXTALIGN.TOP_LEFT;
            this.m_btnClearMessage.ThemeIndex = 0;
            this.m_btnClearMessage.UseBorder = true;
            this.m_btnClearMessage.UseClickedEmphasizeTextColor = false;
            this.m_btnClearMessage.UseCustomizeClickedColor = false;
            this.m_btnClearMessage.UseEdge = true;
            this.m_btnClearMessage.UseHoverEmphasizeCustomColor = false;
            this.m_btnClearMessage.UseImage = false;
            this.m_btnClearMessage.UserHoverEmpahsize = false;
            this.m_btnClearMessage.UseSubFont = false;
            this.m_btnClearMessage.Click += new System.EventHandler(this.Click_SendOrClearMessage);
            // 
            // _tableLayoutPanelMain
            // 
            this._tableLayoutPanelMain.ColumnCount = 2;
            this._tableLayoutPanelMain.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this._tableLayoutPanelMain.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this._tableLayoutPanelMain.Controls.Add(this.m_groupList, 0, 0);
            this._tableLayoutPanelMain.Controls.Add(this.m_groupSelectedItem, 0, 1);
            this._tableLayoutPanelMain.Controls.Add(this.m_groupConfiguration, 0, 2);
            this._tableLayoutPanelMain.Controls.Add(this.m_groupSocketDemonstration, 1, 2);
            this._tableLayoutPanelMain.Dock = System.Windows.Forms.DockStyle.Fill;
            this._tableLayoutPanelMain.Location = new System.Drawing.Point(0, 0);
            this._tableLayoutPanelMain.Margin = new System.Windows.Forms.Padding(0);
            this._tableLayoutPanelMain.Name = "_tableLayoutPanelMain";
            this._tableLayoutPanelMain.RowCount = 3;
            this._tableLayoutPanelMain.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 32.21461F));
            this._tableLayoutPanelMain.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 11.70213F));
            this._tableLayoutPanelMain.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 56.14657F));
            this._tableLayoutPanelMain.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this._tableLayoutPanelMain.Size = new System.Drawing.Size(1156, 846);
            this._tableLayoutPanelMain.TabIndex = 1399;
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
            this.m_btnRemove.Location = new System.Drawing.Point(424, 3);
            this.m_btnRemove.MainFont = new System.Drawing.Font("맑은 고딕", 13F, System.Drawing.FontStyle.Bold);
            this.m_btnRemove.MainFontColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(36)))), ((int)(((byte)(0)))));
            this.m_btnRemove.Name = "m_btnRemove";
            this.m_btnRemove.Size = new System.Drawing.Size(133, 70);
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
            this.m_btnAdd.Location = new System.Drawing.Point(288, 3);
            this.m_btnAdd.MainFont = new System.Drawing.Font("맑은 고딕", 13F, System.Drawing.FontStyle.Bold);
            this.m_btnAdd.MainFontColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(36)))), ((int)(((byte)(0)))));
            this.m_btnAdd.Name = "m_btnAdd";
            this.m_btnAdd.Size = new System.Drawing.Size(130, 70);
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
            this.m_btnDisable.Location = new System.Drawing.Point(139, 3);
            this.m_btnDisable.MainFont = new System.Drawing.Font("맑은 고딕", 13F, System.Drawing.FontStyle.Bold);
            this.m_btnDisable.MainFontColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(36)))), ((int)(((byte)(0)))));
            this.m_btnDisable.Name = "m_btnDisable";
            this.m_btnDisable.Size = new System.Drawing.Size(130, 70);
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
            this.m_btnEnable.Size = new System.Drawing.Size(130, 70);
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
            // tableLayoutPanel3
            // 
            this.tableLayoutPanel3.ColumnCount = 5;
            this._tableLayoutPanel_Config.SetColumnSpan(this.tableLayoutPanel3, 4);
            this.tableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 24.39024F));
            this.tableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 24.39024F));
            this.tableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 2.439024F));
            this.tableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 24.39024F));
            this.tableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 24.39024F));
            this.tableLayoutPanel3.Controls.Add(this.m_btnRemove, 4, 0);
            this.tableLayoutPanel3.Controls.Add(this.m_btnEnable, 0, 0);
            this.tableLayoutPanel3.Controls.Add(this.m_btnAdd, 3, 0);
            this.tableLayoutPanel3.Controls.Add(this.m_btnDisable, 1, 0);
            this.tableLayoutPanel3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel3.Location = new System.Drawing.Point(5, 357);
            this.tableLayoutPanel3.Margin = new System.Windows.Forms.Padding(0);
            this.tableLayoutPanel3.Name = "tableLayoutPanel3";
            this.tableLayoutPanel3.RowCount = 1;
            this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel3.Size = new System.Drawing.Size(560, 76);
            this.tableLayoutPanel3.TabIndex = 1399;
            // 
            // Config_Socket
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.Controls.Add(this._tableLayoutPanelMain);
            this.Margin = new System.Windows.Forms.Padding(0);
            this.Name = "Config_Socket";
            this.Size = new System.Drawing.Size(1156, 846);
            ((System.ComponentModel.ISupportInitialize)(this.m_dgViewSocket)).EndInit();
            this.m_groupList.ResumeLayout(false);
            this.m_groupSelectedItem.ResumeLayout(false);
            this.tableLayoutPanel2.ResumeLayout(false);
            this._tableLayoutPanel_SelectedItem_Right.ResumeLayout(false);
            this._tableLayoutPanel_SelectedItem_Left.ResumeLayout(false);
            this.m_groupConfiguration.ResumeLayout(false);
            this._tableLayoutPanel_Config.ResumeLayout(false);
            this.m_groupSocketDemonstration.ResumeLayout(false);
            this.tableLayoutPanel1.ResumeLayout(false);
            this._tableLayoutPanelMain.ResumeLayout(false);
            this.tableLayoutPanel3.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

		private System.Windows.Forms.ListBox m_listMessage;
		private Sys3Controls.Sys3DoubleBufferedDataGridView m_dgViewSocket;
		private System.Windows.Forms.DataGridViewTextBoxColumn ID;
		private System.Windows.Forms.DataGridViewTextBoxColumn ENABLE;
		private System.Windows.Forms.DataGridViewTextBoxColumn MONITORING;
		private System.Windows.Forms.DataGridViewTextBoxColumn PASSWORD;
		private System.Windows.Forms.DataGridViewTextBoxColumn AUTHORITY;
		private System.Windows.Forms.DataGridViewTextBoxColumn TARGETPORT;
		private System.Windows.Forms.DataGridViewTextBoxColumn CONDITION;
		private Sys3Controls.Sys3GroupBoxContainer m_groupList;
        private Sys3Controls.Sys3GroupBoxContainer m_groupSelectedItem;
        private Sys3Controls.Sys3GroupBoxContainer m_groupConfiguration;
        private Sys3Controls.Sys3GroupBoxContainer m_groupSocketDemonstration;
		private Sys3Controls.Sys3Label m_lblName;
		private Sys3Controls.Sys3Label m_lblProtocolType;
		private Sys3Controls.Sys3Label m_lblServerIP;
		private Sys3Controls.Sys3Label m_lblPort;
		private Sys3Controls.Sys3Label m_lblTargetPort;
		private Sys3Controls.Sys3Label m_lblRetryInterval;
		private Sys3Controls.Sys3Label m_lblReceiveTimeout;
		private Sys3Controls.Sys3Label m_lblIndex;
		private Sys3Controls.Sys3Label m_lblState;
		private Sys3Controls.Sys3Label m_labelName;
		private Sys3Controls.Sys3Label m_labelProtocolType;
		private Sys3Controls.Sys3Label m_labelServerIP;
		private Sys3Controls.Sys3Label m_labelPort;
		private Sys3Controls.Sys3Label m_labelTargetPort;
		private Sys3Controls.Sys3Label m_labelRetryInterval;
		private Sys3Controls.Sys3Label m_labelReceiveTimeout;
		private Sys3Controls.Sys3Label m_labelIndex;
		private Sys3Controls.Sys3Label m_labelState;
		private Sys3Controls.Sys3Label m_labelMessage;
        private Sys3Controls.Sys3Label m_lblMessage;
		private Sys3Controls.Sys3button m_btnDisconnect;
		private Sys3Controls.Sys3button m_btnConnect;
		private Sys3Controls.Sys3button m_btnSend;
		private Sys3Controls.Sys3button m_btnClearMessage;
        private Sys3Controls.Sys3Label m_labelLogType;
        private Sys3Controls.Sys3Label m_lblLogType;
        private System.Windows.Forms.TableLayoutPanel _tableLayoutPanelMain;
        private System.Windows.Forms.TableLayoutPanel _tableLayoutPanel_Config;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel2;
        private System.Windows.Forms.TableLayoutPanel _tableLayoutPanel_SelectedItem_Left;
        private System.Windows.Forms.TableLayoutPanel _tableLayoutPanel_SelectedItem_Right;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private Sys3Controls.Sys3button m_btnRemove;
        private Sys3Controls.Sys3button m_btnAdd;
        private Sys3Controls.Sys3button m_btnDisable;
        private Sys3Controls.Sys3button m_btnEnable;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel3;
    }
}
