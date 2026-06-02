namespace FrameOfSystem3.Views.Config
{
    partial class Config_Language
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
            this.m_dgViewLog = new Sys3Controls.Sys3DoubleBufferedDataGridView();
            this.m_dgViewMessage = new Sys3Controls.Sys3DoubleBufferedDataGridView();
            this.m_dgViewUI = new Sys3Controls.Sys3DoubleBufferedDataGridView();
            this.m_groupTitle = new Sys3Controls.Sys3GroupBoxContainer();
            this._tableLayoutPanel_Table = new System.Windows.Forms.TableLayoutPanel();
            this.m_btnTabUI = new Sys3Controls.Sys3button();
            this.m_btnTabMessage = new Sys3Controls.Sys3button();
            this.m_btnTabLog = new Sys3Controls.Sys3button();
            this.m_groupSelectedItem = new Sys3Controls.Sys3GroupBoxContainer();
            this._tableLayoutPanel_Item = new System.Windows.Forms.TableLayoutPanel();
            this.m_lblKey = new Sys3Controls.Sys3Label();
            this.m_labelKey = new Sys3Controls.Sys3Label();
            this.m_groupConfiguration = new Sys3Controls.Sys3GroupBoxContainer();
            this._tableLayoutPanel_Configuration = new System.Windows.Forms.TableLayoutPanel();
            this.m_btnRemove = new Sys3Controls.Sys3button();
            this.m_btnAdd = new Sys3Controls.Sys3button();
            this.m_lblEng = new Sys3Controls.Sys3Label();
            this.m_lblOther = new Sys3Controls.Sys3Label();
            this.m_labelEng = new Sys3Controls.Sys3Label();
            this.m_labelOther = new Sys3Controls.Sys3Label();
            this._tableLayoutPanel_Main = new System.Windows.Forms.TableLayoutPanel();
            this._tableLayoutPanel_Contents = new System.Windows.Forms.TableLayoutPanel();
            this.INDEX1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ID = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.PASSWORD = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.INDEX = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn2 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn4 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn5 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn6 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            ((System.ComponentModel.ISupportInitialize)(this.m_dgViewLog)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.m_dgViewMessage)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.m_dgViewUI)).BeginInit();
            this.m_groupTitle.SuspendLayout();
            this._tableLayoutPanel_Table.SuspendLayout();
            this.m_groupSelectedItem.SuspendLayout();
            this._tableLayoutPanel_Item.SuspendLayout();
            this.m_groupConfiguration.SuspendLayout();
            this._tableLayoutPanel_Configuration.SuspendLayout();
            this._tableLayoutPanel_Main.SuspendLayout();
            this._tableLayoutPanel_Contents.SuspendLayout();
            this.SuspendLayout();
            // 
            // m_dgViewLog
            // 
            this.m_dgViewLog.AllowUserToAddRows = false;
            this.m_dgViewLog.AllowUserToDeleteRows = false;
            this.m_dgViewLog.AllowUserToResizeColumns = false;
            this.m_dgViewLog.AllowUserToResizeRows = false;
            this.m_dgViewLog.BackgroundColor = System.Drawing.Color.White;
            this.m_dgViewLog.CellBorderStyle = System.Windows.Forms.DataGridViewCellBorderStyle.Raised;
            this.m_dgViewLog.ColumnHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.Single;
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle1.BackColor = System.Drawing.Color.LightGray;
            dataGridViewCellStyle1.Font = new System.Drawing.Font("맑은 고딕", 11F, System.Drawing.FontStyle.Bold);
            dataGridViewCellStyle1.ForeColor = System.Drawing.Color.Black;
            dataGridViewCellStyle1.SelectionBackColor = System.Drawing.Color.White;
            dataGridViewCellStyle1.SelectionForeColor = System.Drawing.Color.Black;
            dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.m_dgViewLog.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
            this.m_dgViewLog.ColumnHeadersHeight = 30;
            this.m_dgViewLog.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            this.m_dgViewLog.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.dataGridViewTextBoxColumn4,
            this.dataGridViewTextBoxColumn5,
            this.dataGridViewTextBoxColumn6});
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle2.BackColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle2.Font = new System.Drawing.Font("맑은 고딕", 10F, System.Drawing.FontStyle.Bold);
            dataGridViewCellStyle2.ForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle2.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(120)))), ((int)(((byte)(130)))), ((int)(((byte)(150)))));
            dataGridViewCellStyle2.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.m_dgViewLog.DefaultCellStyle = dataGridViewCellStyle2;
            this.m_dgViewLog.Dock = System.Windows.Forms.DockStyle.Fill;
            this.m_dgViewLog.EnableHeadersVisualStyles = false;
            this.m_dgViewLog.GridColor = System.Drawing.Color.FromArgb(((int)(((byte)(120)))), ((int)(((byte)(130)))), ((int)(((byte)(150)))));
            this.m_dgViewLog.Location = new System.Drawing.Point(0, 364);
            this.m_dgViewLog.Margin = new System.Windows.Forms.Padding(0);
            this.m_dgViewLog.MultiSelect = false;
            this.m_dgViewLog.Name = "m_dgViewLog";
            this.m_dgViewLog.ReadOnly = true;
            this.m_dgViewLog.RowHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.None;
            dataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle3.BackColor = System.Drawing.Color.White;
            dataGridViewCellStyle3.Font = new System.Drawing.Font("맑은 고딕", 11F);
            dataGridViewCellStyle3.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle3.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(120)))), ((int)(((byte)(130)))), ((int)(((byte)(150)))));
            dataGridViewCellStyle3.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle3.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.m_dgViewLog.RowHeadersDefaultCellStyle = dataGridViewCellStyle3;
            this.m_dgViewLog.RowHeadersVisible = false;
            this.m_dgViewLog.RowHeadersWidthSizeMode = System.Windows.Forms.DataGridViewRowHeadersWidthSizeMode.DisableResizing;
            this.m_dgViewLog.RowTemplate.Height = 23;
            this.m_dgViewLog.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.m_dgViewLog.Size = new System.Drawing.Size(1156, 182);
            this.m_dgViewLog.TabIndex = 1130;
            this.m_dgViewLog.Visible = false;
            this.m_dgViewLog.CellDoubleClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.DoubleClick_Dgview);
            // 
            // m_dgViewMessage
            // 
            this.m_dgViewMessage.AllowUserToAddRows = false;
            this.m_dgViewMessage.AllowUserToDeleteRows = false;
            this.m_dgViewMessage.AllowUserToResizeColumns = false;
            this.m_dgViewMessage.AllowUserToResizeRows = false;
            this.m_dgViewMessage.BackgroundColor = System.Drawing.Color.White;
            this.m_dgViewMessage.CellBorderStyle = System.Windows.Forms.DataGridViewCellBorderStyle.Raised;
            this.m_dgViewMessage.ColumnHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.Single;
            dataGridViewCellStyle4.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle4.BackColor = System.Drawing.Color.LightGray;
            dataGridViewCellStyle4.Font = new System.Drawing.Font("맑은 고딕", 11F, System.Drawing.FontStyle.Bold);
            dataGridViewCellStyle4.ForeColor = System.Drawing.Color.Black;
            dataGridViewCellStyle4.SelectionBackColor = System.Drawing.Color.White;
            dataGridViewCellStyle4.SelectionForeColor = System.Drawing.Color.Black;
            dataGridViewCellStyle4.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.m_dgViewMessage.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle4;
            this.m_dgViewMessage.ColumnHeadersHeight = 30;
            this.m_dgViewMessage.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            this.m_dgViewMessage.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.INDEX,
            this.dataGridViewTextBoxColumn1,
            this.dataGridViewTextBoxColumn2});
            dataGridViewCellStyle5.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle5.BackColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle5.Font = new System.Drawing.Font("맑은 고딕", 10F, System.Drawing.FontStyle.Bold);
            dataGridViewCellStyle5.ForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle5.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(120)))), ((int)(((byte)(130)))), ((int)(((byte)(150)))));
            dataGridViewCellStyle5.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle5.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.m_dgViewMessage.DefaultCellStyle = dataGridViewCellStyle5;
            this.m_dgViewMessage.Dock = System.Windows.Forms.DockStyle.Fill;
            this.m_dgViewMessage.EnableHeadersVisualStyles = false;
            this.m_dgViewMessage.GridColor = System.Drawing.Color.FromArgb(((int)(((byte)(120)))), ((int)(((byte)(130)))), ((int)(((byte)(150)))));
            this.m_dgViewMessage.Location = new System.Drawing.Point(0, 182);
            this.m_dgViewMessage.Margin = new System.Windows.Forms.Padding(0);
            this.m_dgViewMessage.MultiSelect = false;
            this.m_dgViewMessage.Name = "m_dgViewMessage";
            this.m_dgViewMessage.ReadOnly = true;
            this.m_dgViewMessage.RowHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.None;
            dataGridViewCellStyle6.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle6.BackColor = System.Drawing.Color.White;
            dataGridViewCellStyle6.Font = new System.Drawing.Font("맑은 고딕", 11F);
            dataGridViewCellStyle6.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle6.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(120)))), ((int)(((byte)(130)))), ((int)(((byte)(150)))));
            dataGridViewCellStyle6.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle6.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.m_dgViewMessage.RowHeadersDefaultCellStyle = dataGridViewCellStyle6;
            this.m_dgViewMessage.RowHeadersVisible = false;
            this.m_dgViewMessage.RowHeadersWidthSizeMode = System.Windows.Forms.DataGridViewRowHeadersWidthSizeMode.DisableResizing;
            this.m_dgViewMessage.RowTemplate.Height = 23;
            this.m_dgViewMessage.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.m_dgViewMessage.Size = new System.Drawing.Size(1156, 182);
            this.m_dgViewMessage.TabIndex = 1129;
            this.m_dgViewMessage.Visible = false;
            this.m_dgViewMessage.CellDoubleClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.DoubleClick_Dgview);
            // 
            // m_dgViewUI
            // 
            this.m_dgViewUI.AllowUserToAddRows = false;
            this.m_dgViewUI.AllowUserToDeleteRows = false;
            this.m_dgViewUI.AllowUserToResizeColumns = false;
            this.m_dgViewUI.AllowUserToResizeRows = false;
            this.m_dgViewUI.BackgroundColor = System.Drawing.Color.White;
            this.m_dgViewUI.CellBorderStyle = System.Windows.Forms.DataGridViewCellBorderStyle.Raised;
            this.m_dgViewUI.ColumnHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.Single;
            dataGridViewCellStyle7.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle7.BackColor = System.Drawing.Color.LightGray;
            dataGridViewCellStyle7.Font = new System.Drawing.Font("맑은 고딕", 11F, System.Drawing.FontStyle.Bold);
            dataGridViewCellStyle7.ForeColor = System.Drawing.Color.Black;
            dataGridViewCellStyle7.SelectionBackColor = System.Drawing.Color.White;
            dataGridViewCellStyle7.SelectionForeColor = System.Drawing.Color.Black;
            dataGridViewCellStyle7.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.m_dgViewUI.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle7;
            this.m_dgViewUI.ColumnHeadersHeight = 30;
            this.m_dgViewUI.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            this.m_dgViewUI.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.INDEX1,
            this.ID,
            this.PASSWORD});
            dataGridViewCellStyle8.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle8.BackColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle8.Font = new System.Drawing.Font("맑은 고딕", 10F, System.Drawing.FontStyle.Bold);
            dataGridViewCellStyle8.ForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle8.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(120)))), ((int)(((byte)(130)))), ((int)(((byte)(150)))));
            dataGridViewCellStyle8.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle8.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.m_dgViewUI.DefaultCellStyle = dataGridViewCellStyle8;
            this.m_dgViewUI.Dock = System.Windows.Forms.DockStyle.Fill;
            this.m_dgViewUI.EnableHeadersVisualStyles = false;
            this.m_dgViewUI.GridColor = System.Drawing.Color.FromArgb(((int)(((byte)(120)))), ((int)(((byte)(130)))), ((int)(((byte)(150)))));
            this.m_dgViewUI.Location = new System.Drawing.Point(0, 0);
            this.m_dgViewUI.Margin = new System.Windows.Forms.Padding(0);
            this.m_dgViewUI.MultiSelect = false;
            this.m_dgViewUI.Name = "m_dgViewUI";
            this.m_dgViewUI.ReadOnly = true;
            this.m_dgViewUI.RowHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.None;
            dataGridViewCellStyle9.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle9.BackColor = System.Drawing.Color.White;
            dataGridViewCellStyle9.Font = new System.Drawing.Font("맑은 고딕", 11F);
            dataGridViewCellStyle9.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle9.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(120)))), ((int)(((byte)(130)))), ((int)(((byte)(150)))));
            dataGridViewCellStyle9.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle9.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.m_dgViewUI.RowHeadersDefaultCellStyle = dataGridViewCellStyle9;
            this.m_dgViewUI.RowHeadersVisible = false;
            this.m_dgViewUI.RowHeadersWidthSizeMode = System.Windows.Forms.DataGridViewRowHeadersWidthSizeMode.DisableResizing;
            this.m_dgViewUI.RowTemplate.Height = 23;
            this.m_dgViewUI.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.m_dgViewUI.Size = new System.Drawing.Size(1156, 182);
            this.m_dgViewUI.TabIndex = 1121;
            this.m_dgViewUI.CellDoubleClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.DoubleClick_Dgview);
            // 
            // m_groupTitle
            // 
            this.m_groupTitle.BackGroundColor = System.Drawing.Color.WhiteSmoke;
            this.m_groupTitle.Controls.Add(this._tableLayoutPanel_Table);
            this.m_groupTitle.Dock = System.Windows.Forms.DockStyle.Fill;
            this.m_groupTitle.EdgeBorderStroke = 2;
            this.m_groupTitle.EdgeRadius = 2;
            this.m_groupTitle.LabelFont = new System.Drawing.Font("맑은 고딕", 10F, System.Drawing.FontStyle.Bold);
            this.m_groupTitle.LabelGradientColorFirst = System.Drawing.Color.FromArgb(((int)(((byte)(208)))), ((int)(((byte)(208)))), ((int)(((byte)(208)))));
            this.m_groupTitle.LabelGradientColorSecond = System.Drawing.Color.FromArgb(((int)(((byte)(240)))), ((int)(((byte)(240)))), ((int)(((byte)(240)))));
            this.m_groupTitle.LabelHeight = 31;
            this.m_groupTitle.LabelTextColor = System.Drawing.Color.Black;
            this.m_groupTitle.Location = new System.Drawing.Point(0, 0);
            this.m_groupTitle.Margin = new System.Windows.Forms.Padding(0);
            this.m_groupTitle.Name = "m_groupTitle";
            this.m_groupTitle.Padding = new System.Windows.Forms.Padding(3, 18, 3, 3);
            this.m_groupTitle.Size = new System.Drawing.Size(1156, 89);
            this.m_groupTitle.TabIndex = 1350;
            this.m_groupTitle.TabStop = false;
            this.m_groupTitle.Text = "LANGUAGE MAPPING TABLE";
            this.m_groupTitle.TextAlign = Sys3Controls.EN_TEXTALIGN.MIDDLE_CENTER;
            this.m_groupTitle.ThemeIndex = 0;
            this.m_groupTitle.UseLabelBorder = true;
            this.m_groupTitle.UseTitle = true;
            // 
            // _tableLayoutPanel_Table
            // 
            this._tableLayoutPanel_Table.ColumnCount = 10;
            this._tableLayoutPanel_Table.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 9.999999F));
            this._tableLayoutPanel_Table.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 9.999999F));
            this._tableLayoutPanel_Table.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 9.999999F));
            this._tableLayoutPanel_Table.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 9.999999F));
            this._tableLayoutPanel_Table.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 9.999999F));
            this._tableLayoutPanel_Table.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 9.999999F));
            this._tableLayoutPanel_Table.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 9.999999F));
            this._tableLayoutPanel_Table.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 9.999999F));
            this._tableLayoutPanel_Table.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 9.999999F));
            this._tableLayoutPanel_Table.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 9.999999F));
            this._tableLayoutPanel_Table.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this._tableLayoutPanel_Table.Controls.Add(this.m_btnTabUI, 7, 0);
            this._tableLayoutPanel_Table.Controls.Add(this.m_btnTabMessage, 8, 0);
            this._tableLayoutPanel_Table.Controls.Add(this.m_btnTabLog, 9, 0);
            this._tableLayoutPanel_Table.Dock = System.Windows.Forms.DockStyle.Fill;
            this._tableLayoutPanel_Table.Location = new System.Drawing.Point(3, 32);
            this._tableLayoutPanel_Table.Margin = new System.Windows.Forms.Padding(0);
            this._tableLayoutPanel_Table.Name = "_tableLayoutPanel_Table";
            this._tableLayoutPanel_Table.RowCount = 1;
            this._tableLayoutPanel_Table.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this._tableLayoutPanel_Table.Size = new System.Drawing.Size(1150, 54);
            this._tableLayoutPanel_Table.TabIndex = 0;
            // 
            // m_btnTabUI
            // 
            this.m_btnTabUI.BorderWidth = 2;
            this.m_btnTabUI.ButtonClicked = true;
            this.m_btnTabUI.ClickedEmphasizeTextColor = System.Drawing.Color.White;
            this.m_btnTabUI.CustomClickedGradientFirstColor = System.Drawing.Color.White;
            this.m_btnTabUI.CustomClickedGradientSecondColor = System.Drawing.Color.White;
            this.m_btnTabUI.Description = "";
            this.m_btnTabUI.DisabledColor = System.Drawing.Color.DarkGray;
            this.m_btnTabUI.Dock = System.Windows.Forms.DockStyle.Fill;
            this.m_btnTabUI.EdgeRadius = 5;
            this.m_btnTabUI.GradientAngle = 70F;
            this.m_btnTabUI.GradientFirstColor = System.Drawing.Color.White;
            this.m_btnTabUI.GradientSecondColor = System.Drawing.Color.FromArgb(((int)(((byte)(170)))), ((int)(((byte)(176)))), ((int)(((byte)(183)))));
            this.m_btnTabUI.HoverEmphasizeCustomColor = System.Drawing.Color.White;
            this.m_btnTabUI.ImagePosition = new System.Drawing.Point(7, 7);
            this.m_btnTabUI.ImageSize = new System.Drawing.Point(30, 30);
            this.m_btnTabUI.LoadImage = global::FrameOfSystem3.Properties.Resources.CONFIG_ADD3;
            this.m_btnTabUI.Location = new System.Drawing.Point(798, 0);
            this.m_btnTabUI.MainFont = new System.Drawing.Font("맑은 고딕", 13F, System.Drawing.FontStyle.Bold);
            this.m_btnTabUI.MainFontColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(36)))), ((int)(((byte)(0)))));
            this.m_btnTabUI.Margin = new System.Windows.Forms.Padding(0);
            this.m_btnTabUI.Name = "m_btnTabUI";
            this.m_btnTabUI.Size = new System.Drawing.Size(114, 54);
            this.m_btnTabUI.SubFont = new System.Drawing.Font("맑은 고딕", 8F, System.Drawing.FontStyle.Bold);
            this.m_btnTabUI.SubFontColor = System.Drawing.Color.DarkBlue;
            this.m_btnTabUI.SubText = "STATUS";
            this.m_btnTabUI.TabIndex = 0;
            this.m_btnTabUI.Text = "GUI";
            this.m_btnTabUI.TextAlignMain = Sys3Controls.EN_TEXTALIGN.MIDDLE_CENTER;
            this.m_btnTabUI.TextAlignSub = Sys3Controls.EN_TEXTALIGN.TOP_LEFT;
            this.m_btnTabUI.ThemeIndex = 0;
            this.m_btnTabUI.UseBorder = true;
            this.m_btnTabUI.UseClickedEmphasizeTextColor = false;
            this.m_btnTabUI.UseCustomizeClickedColor = false;
            this.m_btnTabUI.UseEdge = false;
            this.m_btnTabUI.UseHoverEmphasizeCustomColor = false;
            this.m_btnTabUI.UseImage = false;
            this.m_btnTabUI.UserHoverEmpahsize = false;
            this.m_btnTabUI.UseSubFont = false;
            this.m_btnTabUI.Click += new System.EventHandler(this.Click_TabButtons);
            // 
            // m_btnTabMessage
            // 
            this.m_btnTabMessage.BorderWidth = 2;
            this.m_btnTabMessage.ButtonClicked = false;
            this.m_btnTabMessage.ClickedEmphasizeTextColor = System.Drawing.Color.White;
            this.m_btnTabMessage.CustomClickedGradientFirstColor = System.Drawing.Color.White;
            this.m_btnTabMessage.CustomClickedGradientSecondColor = System.Drawing.Color.White;
            this.m_btnTabMessage.Description = "";
            this.m_btnTabMessage.DisabledColor = System.Drawing.Color.DarkGray;
            this.m_btnTabMessage.Dock = System.Windows.Forms.DockStyle.Fill;
            this.m_btnTabMessage.EdgeRadius = 5;
            this.m_btnTabMessage.GradientAngle = 70F;
            this.m_btnTabMessage.GradientFirstColor = System.Drawing.Color.White;
            this.m_btnTabMessage.GradientSecondColor = System.Drawing.Color.FromArgb(((int)(((byte)(170)))), ((int)(((byte)(176)))), ((int)(((byte)(183)))));
            this.m_btnTabMessage.HoverEmphasizeCustomColor = System.Drawing.Color.White;
            this.m_btnTabMessage.ImagePosition = new System.Drawing.Point(7, 7);
            this.m_btnTabMessage.ImageSize = new System.Drawing.Point(30, 30);
            this.m_btnTabMessage.LoadImage = global::FrameOfSystem3.Properties.Resources.CONFIG_ADD3;
            this.m_btnTabMessage.Location = new System.Drawing.Point(912, 0);
            this.m_btnTabMessage.MainFont = new System.Drawing.Font("맑은 고딕", 13F, System.Drawing.FontStyle.Bold);
            this.m_btnTabMessage.MainFontColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(36)))), ((int)(((byte)(0)))));
            this.m_btnTabMessage.Margin = new System.Windows.Forms.Padding(0);
            this.m_btnTabMessage.Name = "m_btnTabMessage";
            this.m_btnTabMessage.Size = new System.Drawing.Size(114, 54);
            this.m_btnTabMessage.SubFont = new System.Drawing.Font("맑은 고딕", 8F, System.Drawing.FontStyle.Bold);
            this.m_btnTabMessage.SubFontColor = System.Drawing.Color.DarkBlue;
            this.m_btnTabMessage.SubText = "STATUS";
            this.m_btnTabMessage.TabIndex = 1;
            this.m_btnTabMessage.Text = "MESSAGE";
            this.m_btnTabMessage.TextAlignMain = Sys3Controls.EN_TEXTALIGN.MIDDLE_CENTER;
            this.m_btnTabMessage.TextAlignSub = Sys3Controls.EN_TEXTALIGN.TOP_LEFT;
            this.m_btnTabMessage.ThemeIndex = 0;
            this.m_btnTabMessage.UseBorder = true;
            this.m_btnTabMessage.UseClickedEmphasizeTextColor = false;
            this.m_btnTabMessage.UseCustomizeClickedColor = false;
            this.m_btnTabMessage.UseEdge = false;
            this.m_btnTabMessage.UseHoverEmphasizeCustomColor = false;
            this.m_btnTabMessage.UseImage = false;
            this.m_btnTabMessage.UserHoverEmpahsize = false;
            this.m_btnTabMessage.UseSubFont = false;
            this.m_btnTabMessage.Click += new System.EventHandler(this.Click_TabButtons);
            // 
            // m_btnTabLog
            // 
            this.m_btnTabLog.BorderWidth = 2;
            this.m_btnTabLog.ButtonClicked = false;
            this.m_btnTabLog.ClickedEmphasizeTextColor = System.Drawing.Color.White;
            this.m_btnTabLog.CustomClickedGradientFirstColor = System.Drawing.Color.White;
            this.m_btnTabLog.CustomClickedGradientSecondColor = System.Drawing.Color.White;
            this.m_btnTabLog.Description = "";
            this.m_btnTabLog.DisabledColor = System.Drawing.Color.DarkGray;
            this.m_btnTabLog.Dock = System.Windows.Forms.DockStyle.Fill;
            this.m_btnTabLog.EdgeRadius = 5;
            this.m_btnTabLog.GradientAngle = 70F;
            this.m_btnTabLog.GradientFirstColor = System.Drawing.Color.White;
            this.m_btnTabLog.GradientSecondColor = System.Drawing.Color.FromArgb(((int)(((byte)(170)))), ((int)(((byte)(176)))), ((int)(((byte)(183)))));
            this.m_btnTabLog.HoverEmphasizeCustomColor = System.Drawing.Color.White;
            this.m_btnTabLog.ImagePosition = new System.Drawing.Point(7, 7);
            this.m_btnTabLog.ImageSize = new System.Drawing.Point(30, 30);
            this.m_btnTabLog.LoadImage = global::FrameOfSystem3.Properties.Resources.CONFIG_ADD3;
            this.m_btnTabLog.Location = new System.Drawing.Point(1026, 0);
            this.m_btnTabLog.MainFont = new System.Drawing.Font("맑은 고딕", 13F, System.Drawing.FontStyle.Bold);
            this.m_btnTabLog.MainFontColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(36)))), ((int)(((byte)(0)))));
            this.m_btnTabLog.Margin = new System.Windows.Forms.Padding(0);
            this.m_btnTabLog.Name = "m_btnTabLog";
            this.m_btnTabLog.Size = new System.Drawing.Size(124, 54);
            this.m_btnTabLog.SubFont = new System.Drawing.Font("맑은 고딕", 8F, System.Drawing.FontStyle.Bold);
            this.m_btnTabLog.SubFontColor = System.Drawing.Color.DarkBlue;
            this.m_btnTabLog.SubText = "STATUS";
            this.m_btnTabLog.TabIndex = 2;
            this.m_btnTabLog.Text = "LOG";
            this.m_btnTabLog.TextAlignMain = Sys3Controls.EN_TEXTALIGN.MIDDLE_CENTER;
            this.m_btnTabLog.TextAlignSub = Sys3Controls.EN_TEXTALIGN.TOP_LEFT;
            this.m_btnTabLog.ThemeIndex = 0;
            this.m_btnTabLog.UseBorder = true;
            this.m_btnTabLog.UseClickedEmphasizeTextColor = false;
            this.m_btnTabLog.UseCustomizeClickedColor = false;
            this.m_btnTabLog.UseEdge = false;
            this.m_btnTabLog.UseHoverEmphasizeCustomColor = false;
            this.m_btnTabLog.UseImage = false;
            this.m_btnTabLog.UserHoverEmpahsize = false;
            this.m_btnTabLog.UseSubFont = false;
            this.m_btnTabLog.Click += new System.EventHandler(this.Click_TabButtons);
            // 
            // m_groupSelectedItem
            // 
            this.m_groupSelectedItem.BackGroundColor = System.Drawing.Color.WhiteSmoke;
            this.m_groupSelectedItem.Controls.Add(this._tableLayoutPanel_Item);
            this.m_groupSelectedItem.Dock = System.Windows.Forms.DockStyle.Fill;
            this.m_groupSelectedItem.EdgeBorderStroke = 2;
            this.m_groupSelectedItem.EdgeRadius = 2;
            this.m_groupSelectedItem.LabelFont = new System.Drawing.Font("맑은 고딕", 10F, System.Drawing.FontStyle.Bold);
            this.m_groupSelectedItem.LabelGradientColorFirst = System.Drawing.Color.FromArgb(((int)(((byte)(208)))), ((int)(((byte)(208)))), ((int)(((byte)(208)))));
            this.m_groupSelectedItem.LabelGradientColorSecond = System.Drawing.Color.FromArgb(((int)(((byte)(240)))), ((int)(((byte)(240)))), ((int)(((byte)(240)))));
            this.m_groupSelectedItem.LabelHeight = 31;
            this.m_groupSelectedItem.LabelTextColor = System.Drawing.Color.Black;
            this.m_groupSelectedItem.Location = new System.Drawing.Point(0, 635);
            this.m_groupSelectedItem.Margin = new System.Windows.Forms.Padding(0);
            this.m_groupSelectedItem.Name = "m_groupSelectedItem";
            this.m_groupSelectedItem.Padding = new System.Windows.Forms.Padding(3, 18, 3, 3);
            this.m_groupSelectedItem.Size = new System.Drawing.Size(1156, 91);
            this.m_groupSelectedItem.TabIndex = 1351;
            this.m_groupSelectedItem.TabStop = false;
            this.m_groupSelectedItem.Text = "SELECTED ITEM";
            this.m_groupSelectedItem.TextAlign = Sys3Controls.EN_TEXTALIGN.MIDDLE_CENTER;
            this.m_groupSelectedItem.ThemeIndex = 0;
            this.m_groupSelectedItem.UseLabelBorder = true;
            this.m_groupSelectedItem.UseTitle = true;
            // 
            // _tableLayoutPanel_Item
            // 
            this._tableLayoutPanel_Item.ColumnCount = 8;
            this._tableLayoutPanel_Item.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 2.272727F));
            this._tableLayoutPanel_Item.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 13.63636F));
            this._tableLayoutPanel_Item.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 45.45454F));
            this._tableLayoutPanel_Item.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 4.545455F));
            this._tableLayoutPanel_Item.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 13.63636F));
            this._tableLayoutPanel_Item.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 4.545455F));
            this._tableLayoutPanel_Item.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 13.63636F));
            this._tableLayoutPanel_Item.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 2.272727F));
            this._tableLayoutPanel_Item.Controls.Add(this.m_lblKey, 1, 1);
            this._tableLayoutPanel_Item.Controls.Add(this.m_labelKey, 2, 1);
            this._tableLayoutPanel_Item.Dock = System.Windows.Forms.DockStyle.Fill;
            this._tableLayoutPanel_Item.Location = new System.Drawing.Point(3, 32);
            this._tableLayoutPanel_Item.Margin = new System.Windows.Forms.Padding(0);
            this._tableLayoutPanel_Item.Name = "_tableLayoutPanel_Item";
            this._tableLayoutPanel_Item.RowCount = 3;
            this._tableLayoutPanel_Item.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 8.333332F));
            this._tableLayoutPanel_Item.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 83.33333F));
            this._tableLayoutPanel_Item.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 8.333332F));
            this._tableLayoutPanel_Item.Size = new System.Drawing.Size(1150, 56);
            this._tableLayoutPanel_Item.TabIndex = 0;
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
            this.m_lblKey.Location = new System.Drawing.Point(28, 6);
            this.m_lblKey.MainFont = new System.Drawing.Font("맑은 고딕", 9F, System.Drawing.FontStyle.Bold);
            this.m_lblKey.MainFontColor = System.Drawing.Color.Black;
            this.m_lblKey.Margin = new System.Windows.Forms.Padding(2);
            this.m_lblKey.Name = "m_lblKey";
            this.m_lblKey.Size = new System.Drawing.Size(152, 42);
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
            this.m_labelKey.Location = new System.Drawing.Point(184, 6);
            this.m_labelKey.MainFont = new System.Drawing.Font("맑은 고딕", 11F, System.Drawing.FontStyle.Bold);
            this.m_labelKey.MainFontColor = System.Drawing.Color.Black;
            this.m_labelKey.Margin = new System.Windows.Forms.Padding(2);
            this.m_labelKey.Name = "m_labelKey";
            this.m_labelKey.Size = new System.Drawing.Size(518, 42);
            this.m_labelKey.SubFont = new System.Drawing.Font("맑은 고딕", 10F);
            this.m_labelKey.SubFontColor = System.Drawing.Color.Black;
            this.m_labelKey.SubText = "";
            this.m_labelKey.TabIndex = 1356;
            this.m_labelKey.TextAlignMain = Sys3Controls.EN_TEXTALIGN.BOTTOM_RIGHT;
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
            this.m_groupConfiguration.Controls.Add(this._tableLayoutPanel_Configuration);
            this.m_groupConfiguration.Dock = System.Windows.Forms.DockStyle.Fill;
            this.m_groupConfiguration.EdgeBorderStroke = 2;
            this.m_groupConfiguration.EdgeRadius = 2;
            this.m_groupConfiguration.LabelFont = new System.Drawing.Font("맑은 고딕", 10F, System.Drawing.FontStyle.Bold);
            this.m_groupConfiguration.LabelGradientColorFirst = System.Drawing.Color.FromArgb(((int)(((byte)(208)))), ((int)(((byte)(208)))), ((int)(((byte)(208)))));
            this.m_groupConfiguration.LabelGradientColorSecond = System.Drawing.Color.FromArgb(((int)(((byte)(240)))), ((int)(((byte)(240)))), ((int)(((byte)(240)))));
            this.m_groupConfiguration.LabelHeight = 31;
            this.m_groupConfiguration.LabelTextColor = System.Drawing.Color.Black;
            this.m_groupConfiguration.Location = new System.Drawing.Point(0, 726);
            this.m_groupConfiguration.Margin = new System.Windows.Forms.Padding(0);
            this.m_groupConfiguration.Name = "m_groupConfiguration";
            this.m_groupConfiguration.Padding = new System.Windows.Forms.Padding(3, 18, 3, 3);
            this.m_groupConfiguration.Size = new System.Drawing.Size(1156, 174);
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
            this._tableLayoutPanel_Configuration.ColumnCount = 8;
            this._tableLayoutPanel_Configuration.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 2.272727F));
            this._tableLayoutPanel_Configuration.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 13.63636F));
            this._tableLayoutPanel_Configuration.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 45.45455F));
            this._tableLayoutPanel_Configuration.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 4.545455F));
            this._tableLayoutPanel_Configuration.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 13.63636F));
            this._tableLayoutPanel_Configuration.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 4.545455F));
            this._tableLayoutPanel_Configuration.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 13.63636F));
            this._tableLayoutPanel_Configuration.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 2.272727F));
            this._tableLayoutPanel_Configuration.Controls.Add(this.m_lblEng, 1, 1);
            this._tableLayoutPanel_Configuration.Controls.Add(this.m_btnRemove, 6, 1);
            this._tableLayoutPanel_Configuration.Controls.Add(this.m_lblOther, 1, 2);
            this._tableLayoutPanel_Configuration.Controls.Add(this.m_labelEng, 2, 1);
            this._tableLayoutPanel_Configuration.Controls.Add(this.m_labelOther, 2, 2);
            this._tableLayoutPanel_Configuration.Controls.Add(this.m_btnAdd, 4, 1);
            this._tableLayoutPanel_Configuration.Dock = System.Windows.Forms.DockStyle.Fill;
            this._tableLayoutPanel_Configuration.Location = new System.Drawing.Point(3, 32);
            this._tableLayoutPanel_Configuration.Margin = new System.Windows.Forms.Padding(0);
            this._tableLayoutPanel_Configuration.Name = "_tableLayoutPanel_Configuration";
            this._tableLayoutPanel_Configuration.RowCount = 5;
            this._tableLayoutPanel_Configuration.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 3.125078F));
            this._tableLayoutPanel_Configuration.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 31.25078F));
            this._tableLayoutPanel_Configuration.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 31.24953F));
            this._tableLayoutPanel_Configuration.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 31.24953F));
            this._tableLayoutPanel_Configuration.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 3.125078F));
            this._tableLayoutPanel_Configuration.Size = new System.Drawing.Size(1150, 139);
            this._tableLayoutPanel_Configuration.TabIndex = 1;
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
            this.m_btnRemove.ImagePosition = new System.Drawing.Point(10, 10);
            this.m_btnRemove.ImageSize = new System.Drawing.Point(30, 30);
            this.m_btnRemove.LoadImage = global::FrameOfSystem3.Properties.Resources.REMOVE;
            this.m_btnRemove.Location = new System.Drawing.Point(967, 7);
            this.m_btnRemove.MainFont = new System.Drawing.Font("맑은 고딕", 13F, System.Drawing.FontStyle.Bold);
            this.m_btnRemove.MainFontColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(36)))), ((int)(((byte)(0)))));
            this.m_btnRemove.Name = "m_btnRemove";
            this._tableLayoutPanel_Configuration.SetRowSpan(this.m_btnRemove, 3);
            this.m_btnRemove.Size = new System.Drawing.Size(150, 123);
            this.m_btnRemove.SubFont = new System.Drawing.Font("맑은 고딕", 8F, System.Drawing.FontStyle.Bold);
            this.m_btnRemove.SubFontColor = System.Drawing.Color.DarkBlue;
            this.m_btnRemove.SubText = "STATUS";
            this.m_btnRemove.TabIndex = 2;
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
            this.m_btnRemove.Click += new System.EventHandler(this.Click_ManagementButton);
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
            this.m_btnAdd.ImagePosition = new System.Drawing.Point(10, 10);
            this.m_btnAdd.ImageSize = new System.Drawing.Point(30, 30);
            this.m_btnAdd.LoadImage = global::FrameOfSystem3.Properties.Resources.CONFIG_ADD3;
            this.m_btnAdd.Location = new System.Drawing.Point(759, 7);
            this.m_btnAdd.MainFont = new System.Drawing.Font("맑은 고딕", 13F, System.Drawing.FontStyle.Bold);
            this.m_btnAdd.MainFontColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(36)))), ((int)(((byte)(0)))));
            this.m_btnAdd.Name = "m_btnAdd";
            this._tableLayoutPanel_Configuration.SetRowSpan(this.m_btnAdd, 3);
            this.m_btnAdd.Size = new System.Drawing.Size(150, 123);
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
            this.m_btnAdd.Click += new System.EventHandler(this.Click_ManagementButton);
            // 
            // m_lblEng
            // 
            this.m_lblEng.BackGroundColor = System.Drawing.Color.FromArgb(((int)(((byte)(200)))), ((int)(((byte)(200)))), ((int)(((byte)(220)))));
            this.m_lblEng.BorderStroke = 2;
            this.m_lblEng.BorderStyle = System.Windows.Forms.ButtonBorderStyle.Solid;
            this.m_lblEng.Description = "";
            this.m_lblEng.DisabledColor = System.Drawing.Color.DarkGray;
            this.m_lblEng.Dock = System.Windows.Forms.DockStyle.Fill;
            this.m_lblEng.EdgeRadius = 1;
            this.m_lblEng.ImagePosition = new System.Drawing.Point(0, 0);
            this.m_lblEng.ImageSize = new System.Drawing.Point(0, 0);
            this.m_lblEng.LoadImage = null;
            this.m_lblEng.Location = new System.Drawing.Point(28, 6);
            this.m_lblEng.MainFont = new System.Drawing.Font("맑은 고딕", 9F, System.Drawing.FontStyle.Bold);
            this.m_lblEng.MainFontColor = System.Drawing.Color.Black;
            this.m_lblEng.Margin = new System.Windows.Forms.Padding(2);
            this.m_lblEng.Name = "m_lblEng";
            this.m_lblEng.Size = new System.Drawing.Size(152, 39);
            this.m_lblEng.SubFont = new System.Drawing.Font("맑은 고딕", 10F);
            this.m_lblEng.SubFontColor = System.Drawing.Color.Black;
            this.m_lblEng.SubText = "";
            this.m_lblEng.TabIndex = 1362;
            this.m_lblEng.Text = "ENGLISH";
            this.m_lblEng.TextAlignMain = Sys3Controls.EN_TEXTALIGN.TOP_LEFT;
            this.m_lblEng.TextAlignSub = Sys3Controls.EN_TEXTALIGN.MIDDLE_CENTER;
            this.m_lblEng.TextAlignUnit = Sys3Controls.EN_TEXTALIGN.MIDDLE_CENTER;
            this.m_lblEng.ThemeIndex = 0;
            this.m_lblEng.UnitAreaRate = 40;
            this.m_lblEng.UnitFont = new System.Drawing.Font("맑은 고딕", 10F, System.Drawing.FontStyle.Bold);
            this.m_lblEng.UnitFontColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.m_lblEng.UnitPositionVertical = false;
            this.m_lblEng.UnitText = "";
            this.m_lblEng.UseBorder = true;
            this.m_lblEng.UseEdgeRadius = false;
            this.m_lblEng.UseImage = false;
            this.m_lblEng.UseSubFont = false;
            this.m_lblEng.UseUnitFont = false;
            // 
            // m_lblOther
            // 
            this.m_lblOther.BackGroundColor = System.Drawing.Color.FromArgb(((int)(((byte)(200)))), ((int)(((byte)(200)))), ((int)(((byte)(220)))));
            this.m_lblOther.BorderStroke = 2;
            this.m_lblOther.BorderStyle = System.Windows.Forms.ButtonBorderStyle.Solid;
            this.m_lblOther.Description = "";
            this.m_lblOther.DisabledColor = System.Drawing.Color.DarkGray;
            this.m_lblOther.Dock = System.Windows.Forms.DockStyle.Fill;
            this.m_lblOther.EdgeRadius = 1;
            this.m_lblOther.ImagePosition = new System.Drawing.Point(0, 0);
            this.m_lblOther.ImageSize = new System.Drawing.Point(0, 0);
            this.m_lblOther.LoadImage = null;
            this.m_lblOther.Location = new System.Drawing.Point(28, 49);
            this.m_lblOther.MainFont = new System.Drawing.Font("맑은 고딕", 9F, System.Drawing.FontStyle.Bold);
            this.m_lblOther.MainFontColor = System.Drawing.Color.Black;
            this.m_lblOther.Margin = new System.Windows.Forms.Padding(2);
            this.m_lblOther.Name = "m_lblOther";
            this.m_lblOther.Size = new System.Drawing.Size(152, 39);
            this.m_lblOther.SubFont = new System.Drawing.Font("맑은 고딕", 10F);
            this.m_lblOther.SubFontColor = System.Drawing.Color.Black;
            this.m_lblOther.SubText = "";
            this.m_lblOther.TabIndex = 1362;
            this.m_lblOther.Text = "KOREAN";
            this.m_lblOther.TextAlignMain = Sys3Controls.EN_TEXTALIGN.TOP_LEFT;
            this.m_lblOther.TextAlignSub = Sys3Controls.EN_TEXTALIGN.MIDDLE_CENTER;
            this.m_lblOther.TextAlignUnit = Sys3Controls.EN_TEXTALIGN.MIDDLE_CENTER;
            this.m_lblOther.ThemeIndex = 0;
            this.m_lblOther.UnitAreaRate = 40;
            this.m_lblOther.UnitFont = new System.Drawing.Font("맑은 고딕", 10F, System.Drawing.FontStyle.Bold);
            this.m_lblOther.UnitFontColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.m_lblOther.UnitPositionVertical = false;
            this.m_lblOther.UnitText = "";
            this.m_lblOther.UseBorder = true;
            this.m_lblOther.UseEdgeRadius = false;
            this.m_lblOther.UseImage = false;
            this.m_lblOther.UseSubFont = false;
            this.m_lblOther.UseUnitFont = false;
            // 
            // m_labelEng
            // 
            this.m_labelEng.BackGroundColor = System.Drawing.Color.White;
            this.m_labelEng.BorderStroke = 2;
            this.m_labelEng.BorderStyle = System.Windows.Forms.ButtonBorderStyle.Solid;
            this.m_labelEng.Description = "";
            this.m_labelEng.DisabledColor = System.Drawing.Color.LightGray;
            this.m_labelEng.Dock = System.Windows.Forms.DockStyle.Fill;
            this.m_labelEng.EdgeRadius = 1;
            this.m_labelEng.Enabled = false;
            this.m_labelEng.ImagePosition = new System.Drawing.Point(0, 0);
            this.m_labelEng.ImageSize = new System.Drawing.Point(0, 0);
            this.m_labelEng.LoadImage = null;
            this.m_labelEng.Location = new System.Drawing.Point(184, 6);
            this.m_labelEng.MainFont = new System.Drawing.Font("맑은 고딕", 11F, System.Drawing.FontStyle.Bold);
            this.m_labelEng.MainFontColor = System.Drawing.Color.Black;
            this.m_labelEng.Margin = new System.Windows.Forms.Padding(2);
            this.m_labelEng.Name = "m_labelEng";
            this.m_labelEng.Size = new System.Drawing.Size(518, 39);
            this.m_labelEng.SubFont = new System.Drawing.Font("맑은 고딕", 10F);
            this.m_labelEng.SubFontColor = System.Drawing.Color.Black;
            this.m_labelEng.SubText = "";
            this.m_labelEng.TabIndex = 0;
            this.m_labelEng.TextAlignMain = Sys3Controls.EN_TEXTALIGN.BOTTOM_RIGHT;
            this.m_labelEng.TextAlignSub = Sys3Controls.EN_TEXTALIGN.MIDDLE_CENTER;
            this.m_labelEng.TextAlignUnit = Sys3Controls.EN_TEXTALIGN.MIDDLE_CENTER;
            this.m_labelEng.ThemeIndex = 0;
            this.m_labelEng.UnitAreaRate = 40;
            this.m_labelEng.UnitFont = new System.Drawing.Font("맑은 고딕", 10F, System.Drawing.FontStyle.Bold);
            this.m_labelEng.UnitFontColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.m_labelEng.UnitPositionVertical = false;
            this.m_labelEng.UnitText = "";
            this.m_labelEng.UseBorder = true;
            this.m_labelEng.UseEdgeRadius = false;
            this.m_labelEng.UseImage = false;
            this.m_labelEng.UseSubFont = false;
            this.m_labelEng.UseUnitFont = false;
            this.m_labelEng.Click += new System.EventHandler(this.Click_Labels);
            // 
            // m_labelOther
            // 
            this.m_labelOther.BackGroundColor = System.Drawing.Color.White;
            this.m_labelOther.BorderStroke = 2;
            this.m_labelOther.BorderStyle = System.Windows.Forms.ButtonBorderStyle.Solid;
            this.m_labelOther.Description = "";
            this.m_labelOther.DisabledColor = System.Drawing.Color.LightGray;
            this.m_labelOther.Dock = System.Windows.Forms.DockStyle.Fill;
            this.m_labelOther.EdgeRadius = 1;
            this.m_labelOther.Enabled = false;
            this.m_labelOther.ImagePosition = new System.Drawing.Point(0, 0);
            this.m_labelOther.ImageSize = new System.Drawing.Point(0, 0);
            this.m_labelOther.LoadImage = null;
            this.m_labelOther.Location = new System.Drawing.Point(184, 49);
            this.m_labelOther.MainFont = new System.Drawing.Font("맑은 고딕", 11F, System.Drawing.FontStyle.Bold);
            this.m_labelOther.MainFontColor = System.Drawing.Color.Black;
            this.m_labelOther.Margin = new System.Windows.Forms.Padding(2);
            this.m_labelOther.Name = "m_labelOther";
            this.m_labelOther.Size = new System.Drawing.Size(518, 39);
            this.m_labelOther.SubFont = new System.Drawing.Font("맑은 고딕", 10F);
            this.m_labelOther.SubFontColor = System.Drawing.Color.Black;
            this.m_labelOther.SubText = "";
            this.m_labelOther.TabIndex = 1;
            this.m_labelOther.TextAlignMain = Sys3Controls.EN_TEXTALIGN.BOTTOM_RIGHT;
            this.m_labelOther.TextAlignSub = Sys3Controls.EN_TEXTALIGN.MIDDLE_CENTER;
            this.m_labelOther.TextAlignUnit = Sys3Controls.EN_TEXTALIGN.MIDDLE_CENTER;
            this.m_labelOther.ThemeIndex = 0;
            this.m_labelOther.UnitAreaRate = 40;
            this.m_labelOther.UnitFont = new System.Drawing.Font("맑은 고딕", 10F, System.Drawing.FontStyle.Bold);
            this.m_labelOther.UnitFontColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.m_labelOther.UnitPositionVertical = false;
            this.m_labelOther.UnitText = "";
            this.m_labelOther.UseBorder = true;
            this.m_labelOther.UseEdgeRadius = false;
            this.m_labelOther.UseImage = false;
            this.m_labelOther.UseSubFont = false;
            this.m_labelOther.UseUnitFont = false;
            this.m_labelOther.Click += new System.EventHandler(this.Click_Labels);
            // 
            // _tableLayoutPanel_Main
            // 
            this._tableLayoutPanel_Main.ColumnCount = 1;
            this._tableLayoutPanel_Main.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this._tableLayoutPanel_Main.Controls.Add(this.m_groupTitle, 0, 0);
            this._tableLayoutPanel_Main.Controls.Add(this.m_groupSelectedItem, 0, 2);
            this._tableLayoutPanel_Main.Controls.Add(this.m_groupConfiguration, 0, 3);
            this._tableLayoutPanel_Main.Controls.Add(this._tableLayoutPanel_Contents, 0, 1);
            this._tableLayoutPanel_Main.Dock = System.Windows.Forms.DockStyle.Fill;
            this._tableLayoutPanel_Main.Location = new System.Drawing.Point(0, 0);
            this._tableLayoutPanel_Main.Margin = new System.Windows.Forms.Padding(0);
            this._tableLayoutPanel_Main.Name = "_tableLayoutPanel_Main";
            this._tableLayoutPanel_Main.RowCount = 4;
            this._tableLayoutPanel_Main.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 89F));
            this._tableLayoutPanel_Main.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 67.32429F));
            this._tableLayoutPanel_Main.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 11.22072F));
            this._tableLayoutPanel_Main.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 21.33169F));
            this._tableLayoutPanel_Main.Size = new System.Drawing.Size(1156, 900);
            this._tableLayoutPanel_Main.TabIndex = 1363;
            // 
            // _tableLayoutPanel_Contents
            // 
            this._tableLayoutPanel_Contents.ColumnCount = 1;
            this._tableLayoutPanel_Contents.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this._tableLayoutPanel_Contents.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this._tableLayoutPanel_Contents.Controls.Add(this.m_dgViewUI, 0, 0);
            this._tableLayoutPanel_Contents.Controls.Add(this.m_dgViewLog, 0, 2);
            this._tableLayoutPanel_Contents.Controls.Add(this.m_dgViewMessage, 0, 1);
            this._tableLayoutPanel_Contents.Dock = System.Windows.Forms.DockStyle.Fill;
            this._tableLayoutPanel_Contents.Location = new System.Drawing.Point(0, 89);
            this._tableLayoutPanel_Contents.Margin = new System.Windows.Forms.Padding(0);
            this._tableLayoutPanel_Contents.Name = "_tableLayoutPanel_Contents";
            this._tableLayoutPanel_Contents.RowCount = 3;
            this._tableLayoutPanel_Contents.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 33.33333F));
            this._tableLayoutPanel_Contents.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 33.33333F));
            this._tableLayoutPanel_Contents.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 33.33333F));
            this._tableLayoutPanel_Contents.Size = new System.Drawing.Size(1156, 546);
            this._tableLayoutPanel_Contents.TabIndex = 1353;
            // 
            // INDEX1
            // 
            this.INDEX1.HeaderText = "INDEX";
            this.INDEX1.Name = "INDEX1";
            this.INDEX1.ReadOnly = true;
            this.INDEX1.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.INDEX1.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.INDEX1.Width = 60;
            // 
            // ID
            // 
            this.ID.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.ID.HeaderText = "ENGLISH";
            this.ID.MaxInputLength = 20;
            this.ID.Name = "ID";
            this.ID.ReadOnly = true;
            this.ID.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.ID.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // PASSWORD
            // 
            this.PASSWORD.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.PASSWORD.HeaderText = "KOREAN";
            this.PASSWORD.MaxInputLength = 20;
            this.PASSWORD.Name = "PASSWORD";
            this.PASSWORD.ReadOnly = true;
            this.PASSWORD.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.PASSWORD.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // INDEX
            // 
            this.INDEX.HeaderText = "INDEX";
            this.INDEX.Name = "INDEX";
            this.INDEX.ReadOnly = true;
            this.INDEX.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.INDEX.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.INDEX.Width = 60;
            // 
            // dataGridViewTextBoxColumn1
            // 
            this.dataGridViewTextBoxColumn1.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.dataGridViewTextBoxColumn1.HeaderText = "ENGLISH";
            this.dataGridViewTextBoxColumn1.MaxInputLength = 20;
            this.dataGridViewTextBoxColumn1.Name = "dataGridViewTextBoxColumn1";
            this.dataGridViewTextBoxColumn1.ReadOnly = true;
            this.dataGridViewTextBoxColumn1.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.dataGridViewTextBoxColumn1.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // dataGridViewTextBoxColumn2
            // 
            this.dataGridViewTextBoxColumn2.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.dataGridViewTextBoxColumn2.HeaderText = "KOREAN";
            this.dataGridViewTextBoxColumn2.MaxInputLength = 20;
            this.dataGridViewTextBoxColumn2.Name = "dataGridViewTextBoxColumn2";
            this.dataGridViewTextBoxColumn2.ReadOnly = true;
            this.dataGridViewTextBoxColumn2.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.dataGridViewTextBoxColumn2.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // dataGridViewTextBoxColumn4
            // 
            this.dataGridViewTextBoxColumn4.HeaderText = "INDEX";
            this.dataGridViewTextBoxColumn4.Name = "dataGridViewTextBoxColumn4";
            this.dataGridViewTextBoxColumn4.ReadOnly = true;
            this.dataGridViewTextBoxColumn4.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.dataGridViewTextBoxColumn4.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.dataGridViewTextBoxColumn4.Width = 60;
            // 
            // dataGridViewTextBoxColumn5
            // 
            this.dataGridViewTextBoxColumn5.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.dataGridViewTextBoxColumn5.HeaderText = "ENGLISH";
            this.dataGridViewTextBoxColumn5.MaxInputLength = 20;
            this.dataGridViewTextBoxColumn5.Name = "dataGridViewTextBoxColumn5";
            this.dataGridViewTextBoxColumn5.ReadOnly = true;
            this.dataGridViewTextBoxColumn5.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.dataGridViewTextBoxColumn5.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // dataGridViewTextBoxColumn6
            // 
            this.dataGridViewTextBoxColumn6.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.dataGridViewTextBoxColumn6.HeaderText = "KOREAN";
            this.dataGridViewTextBoxColumn6.MaxInputLength = 20;
            this.dataGridViewTextBoxColumn6.Name = "dataGridViewTextBoxColumn6";
            this.dataGridViewTextBoxColumn6.ReadOnly = true;
            this.dataGridViewTextBoxColumn6.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.dataGridViewTextBoxColumn6.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // Config_Language
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.Controls.Add(this._tableLayoutPanel_Main);
            this.Name = "Config_Language";
            this.Size = new System.Drawing.Size(1156, 900);
            ((System.ComponentModel.ISupportInitialize)(this.m_dgViewLog)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.m_dgViewMessage)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.m_dgViewUI)).EndInit();
            this.m_groupTitle.ResumeLayout(false);
            this._tableLayoutPanel_Table.ResumeLayout(false);
            this.m_groupSelectedItem.ResumeLayout(false);
            this._tableLayoutPanel_Item.ResumeLayout(false);
            this.m_groupConfiguration.ResumeLayout(false);
            this._tableLayoutPanel_Configuration.ResumeLayout(false);
            this._tableLayoutPanel_Main.ResumeLayout(false);
            this._tableLayoutPanel_Contents.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private Sys3Controls.Sys3DoubleBufferedDataGridView m_dgViewLog;
        private Sys3Controls.Sys3DoubleBufferedDataGridView m_dgViewMessage;
        private Sys3Controls.Sys3DoubleBufferedDataGridView m_dgViewUI;
        private Sys3Controls.Sys3GroupBoxContainer m_groupTitle;
		private Sys3Controls.Sys3button m_btnTabUI;
		private Sys3Controls.Sys3button m_btnTabMessage;
		private Sys3Controls.Sys3button m_btnTabLog;
		private Sys3Controls.Sys3GroupBoxContainer m_groupSelectedItem;
        private Sys3Controls.Sys3GroupBoxContainer m_groupConfiguration;
		private Sys3Controls.Sys3Label m_labelKey;
		private Sys3Controls.Sys3Label m_labelEng;
        private Sys3Controls.Sys3Label m_labelOther;
		private Sys3Controls.Sys3Label m_lblKey;
		private Sys3Controls.Sys3Label m_lblEng;
        private Sys3Controls.Sys3Label m_lblOther;
		private Sys3Controls.Sys3button m_btnRemove;
		private Sys3Controls.Sys3button m_btnAdd;
        private System.Windows.Forms.TableLayoutPanel _tableLayoutPanel_Main;
        private System.Windows.Forms.TableLayoutPanel _tableLayoutPanel_Table;
        private System.Windows.Forms.TableLayoutPanel _tableLayoutPanel_Item;
        private System.Windows.Forms.TableLayoutPanel _tableLayoutPanel_Configuration;
        private System.Windows.Forms.TableLayoutPanel _tableLayoutPanel_Contents;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn4;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn5;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn6;
        private System.Windows.Forms.DataGridViewTextBoxColumn INDEX;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn1;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn2;
        private System.Windows.Forms.DataGridViewTextBoxColumn INDEX1;
        private System.Windows.Forms.DataGridViewTextBoxColumn ID;
        private System.Windows.Forms.DataGridViewTextBoxColumn PASSWORD;
    }
}
