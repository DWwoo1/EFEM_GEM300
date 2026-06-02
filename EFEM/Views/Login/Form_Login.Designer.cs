namespace FrameOfSystem3.Views.Login
{
    partial class Form_Login
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle4 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle5 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle6 = new System.Windows.Forms.DataGridViewCellStyle();
            this.m_dgViewUserList = new Sys3Controls.Sys3DoubleBufferedDataGridView();
            this.m_labelAuthority = new Sys3Controls.Sys3Label();
            this.m_labelLoginPW = new Sys3Controls.Sys3Label();
            this.m_labelLoginID = new Sys3Controls.Sys3Label();
            this.m_labelPW = new Sys3Controls.Sys3Label();
            this.m_labelID = new Sys3Controls.Sys3Label();
            this.m_groupLogin = new Sys3Controls.Sys3GroupBox();
            this.sys3GroupBox2 = new Sys3Controls.Sys3GroupBox();
            this.m_btnProtec = new Sys3Controls.Sys3button();
            this.m_btnCancel = new Sys3Controls.Sys3button();
            this.m_btnCloseManagement = new Sys3Controls.Sys3button();
            this.m_btnRemove = new Sys3Controls.Sys3button();
            this.m_btnModify = new Sys3Controls.Sys3button();
            this.m_btnAdd = new Sys3Controls.Sys3button();
            this.m_btnAuthority = new Sys3Controls.Sys3button();
            this.m_btnUseManagement = new Sys3Controls.Sys3button();
            this.m_btnLogout = new Sys3Controls.Sys3button();
            this.m_btnLogin = new Sys3Controls.Sys3button();
            this.m_btnPopupList = new Sys3Controls.Sys3button();
            this.m_btnLoginIrisRecognition = new Sys3Controls.Sys3button();
            this.m_labelIrisCodeLeft = new Sys3Controls.Sys3Label();
            this.m_labelIrisCodeRight = new Sys3Controls.Sys3Label();
            this.INDEX1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.INDEX = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.GRADE = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.LOGOUT_TIME = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.IRIS_CODE = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.m_labelLogoutTime = new Sys3Controls.Sys3Label();
            ((System.ComponentModel.ISupportInitialize)(this.m_dgViewUserList)).BeginInit();
            this.SuspendLayout();
            // 
            // m_dgViewUserList
            // 
            this.m_dgViewUserList.AllowUserToAddRows = false;
            this.m_dgViewUserList.AllowUserToDeleteRows = false;
            this.m_dgViewUserList.AllowUserToResizeColumns = false;
            this.m_dgViewUserList.AllowUserToResizeRows = false;
            this.m_dgViewUserList.BackgroundColor = System.Drawing.Color.White;
            this.m_dgViewUserList.CellBorderStyle = System.Windows.Forms.DataGridViewCellBorderStyle.Raised;
            this.m_dgViewUserList.ColumnHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.Single;
            dataGridViewCellStyle4.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle4.BackColor = System.Drawing.Color.LightGray;
            dataGridViewCellStyle4.Font = new System.Drawing.Font("맑은 고딕", 11F, System.Drawing.FontStyle.Bold);
            dataGridViewCellStyle4.ForeColor = System.Drawing.Color.Black;
            dataGridViewCellStyle4.SelectionBackColor = System.Drawing.Color.White;
            dataGridViewCellStyle4.SelectionForeColor = System.Drawing.Color.Black;
            dataGridViewCellStyle4.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.m_dgViewUserList.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle4;
            this.m_dgViewUserList.ColumnHeadersHeight = 30;
            this.m_dgViewUserList.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            this.m_dgViewUserList.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.INDEX1,
            this.INDEX,
            this.dataGridViewTextBoxColumn1,
            this.GRADE,
            this.LOGOUT_TIME,
            this.IRIS_CODE});
            dataGridViewCellStyle5.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle5.BackColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle5.Font = new System.Drawing.Font("맑은 고딕", 10F, System.Drawing.FontStyle.Bold);
            dataGridViewCellStyle5.ForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle5.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(120)))), ((int)(((byte)(130)))), ((int)(((byte)(150)))));
            dataGridViewCellStyle5.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle5.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.m_dgViewUserList.DefaultCellStyle = dataGridViewCellStyle5;
            this.m_dgViewUserList.EnableHeadersVisualStyles = false;
            this.m_dgViewUserList.GridColor = System.Drawing.Color.FromArgb(((int)(((byte)(120)))), ((int)(((byte)(130)))), ((int)(((byte)(150)))));
            this.m_dgViewUserList.Location = new System.Drawing.Point(490, 433);
            this.m_dgViewUserList.MultiSelect = false;
            this.m_dgViewUserList.Name = "m_dgViewUserList";
            this.m_dgViewUserList.ReadOnly = true;
            this.m_dgViewUserList.RowHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.None;
            dataGridViewCellStyle6.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle6.BackColor = System.Drawing.Color.White;
            dataGridViewCellStyle6.Font = new System.Drawing.Font("맑은 고딕", 11F);
            dataGridViewCellStyle6.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle6.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(120)))), ((int)(((byte)(130)))), ((int)(((byte)(150)))));
            dataGridViewCellStyle6.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle6.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.m_dgViewUserList.RowHeadersDefaultCellStyle = dataGridViewCellStyle6;
            this.m_dgViewUserList.RowHeadersVisible = false;
            this.m_dgViewUserList.RowHeadersWidthSizeMode = System.Windows.Forms.DataGridViewRowHeadersWidthSizeMode.DisableResizing;
            this.m_dgViewUserList.RowTemplate.Height = 23;
            this.m_dgViewUserList.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.m_dgViewUserList.Size = new System.Drawing.Size(748, 412);
            this.m_dgViewUserList.TabIndex = 1256;
            this.m_dgViewUserList.CellDoubleClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.m_dgViewUserList_CellClick);
            // 
            // m_labelAuthority
            // 
            this.m_labelAuthority.BackGroundColor = System.Drawing.Color.WhiteSmoke;
            this.m_labelAuthority.BorderStroke = 2;
            this.m_labelAuthority.BorderStyle = System.Windows.Forms.ButtonBorderStyle.Solid;
            this.m_labelAuthority.Description = "";
            this.m_labelAuthority.DisabledColor = System.Drawing.Color.LightGray;
            this.m_labelAuthority.EdgeRadius = 1;
            this.m_labelAuthority.Enabled = false;
            this.m_labelAuthority.ImagePosition = new System.Drawing.Point(0, 0);
            this.m_labelAuthority.ImageSize = new System.Drawing.Point(0, 0);
            this.m_labelAuthority.LoadImage = null;
            this.m_labelAuthority.Location = new System.Drawing.Point(60, 549);
            this.m_labelAuthority.MainFont = new System.Drawing.Font("맑은 고딕", 12F, System.Drawing.FontStyle.Bold);
            this.m_labelAuthority.MainFontColor = System.Drawing.Color.DimGray;
            this.m_labelAuthority.Name = "m_labelAuthority";
            this.m_labelAuthority.Size = new System.Drawing.Size(328, 40);
            this.m_labelAuthority.SubFont = new System.Drawing.Font("맑은 고딕", 10F);
            this.m_labelAuthority.SubFontColor = System.Drawing.Color.Black;
            this.m_labelAuthority.SubText = "";
            this.m_labelAuthority.TabIndex = 1362;
            this.m_labelAuthority.Text = "Enter User Authority";
            this.m_labelAuthority.TextAlignMain = Sys3Controls.EN_TEXTALIGN.MIDDLE_LEFT;
            this.m_labelAuthority.TextAlignSub = Sys3Controls.EN_TEXTALIGN.MIDDLE_CENTER;
            this.m_labelAuthority.TextAlignUnit = Sys3Controls.EN_TEXTALIGN.MIDDLE_CENTER;
            this.m_labelAuthority.ThemeIndex = 0;
            this.m_labelAuthority.UnitAreaRate = 40;
            this.m_labelAuthority.UnitFont = new System.Drawing.Font("맑은 고딕", 10F, System.Drawing.FontStyle.Bold);
            this.m_labelAuthority.UnitFontColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.m_labelAuthority.UnitPositionVertical = false;
            this.m_labelAuthority.UnitText = "";
            this.m_labelAuthority.UseBorder = true;
            this.m_labelAuthority.UseEdgeRadius = false;
            this.m_labelAuthority.UseImage = false;
            this.m_labelAuthority.UseSubFont = false;
            this.m_labelAuthority.UseUnitFont = false;
            // 
            // m_labelLoginPW
            // 
            this.m_labelLoginPW.BackGroundColor = System.Drawing.Color.WhiteSmoke;
            this.m_labelLoginPW.BorderStroke = 2;
            this.m_labelLoginPW.BorderStyle = System.Windows.Forms.ButtonBorderStyle.Solid;
            this.m_labelLoginPW.Description = "";
            this.m_labelLoginPW.DisabledColor = System.Drawing.Color.LightGray;
            this.m_labelLoginPW.EdgeRadius = 1;
            this.m_labelLoginPW.Enabled = false;
            this.m_labelLoginPW.ImagePosition = new System.Drawing.Point(0, 0);
            this.m_labelLoginPW.ImageSize = new System.Drawing.Point(0, 0);
            this.m_labelLoginPW.LoadImage = null;
            this.m_labelLoginPW.Location = new System.Drawing.Point(60, 499);
            this.m_labelLoginPW.MainFont = new System.Drawing.Font("맑은 고딕", 12F, System.Drawing.FontStyle.Bold);
            this.m_labelLoginPW.MainFontColor = System.Drawing.Color.DimGray;
            this.m_labelLoginPW.Name = "m_labelLoginPW";
            this.m_labelLoginPW.Size = new System.Drawing.Size(382, 40);
            this.m_labelLoginPW.SubFont = new System.Drawing.Font("맑은 고딕", 14F, System.Drawing.FontStyle.Bold);
            this.m_labelLoginPW.SubFontColor = System.Drawing.Color.Gray;
            this.m_labelLoginPW.SubText = "▶";
            this.m_labelLoginPW.TabIndex = 1;
            this.m_labelLoginPW.Text = "Enter User Password";
            this.m_labelLoginPW.TextAlignMain = Sys3Controls.EN_TEXTALIGN.MIDDLE_LEFT;
            this.m_labelLoginPW.TextAlignSub = Sys3Controls.EN_TEXTALIGN.MIDDLE_RIGHT;
            this.m_labelLoginPW.TextAlignUnit = Sys3Controls.EN_TEXTALIGN.MIDDLE_CENTER;
            this.m_labelLoginPW.ThemeIndex = 0;
            this.m_labelLoginPW.UnitAreaRate = 40;
            this.m_labelLoginPW.UnitFont = new System.Drawing.Font("맑은 고딕", 10F, System.Drawing.FontStyle.Bold);
            this.m_labelLoginPW.UnitFontColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.m_labelLoginPW.UnitPositionVertical = false;
            this.m_labelLoginPW.UnitText = "";
            this.m_labelLoginPW.UseBorder = true;
            this.m_labelLoginPW.UseEdgeRadius = false;
            this.m_labelLoginPW.UseImage = false;
            this.m_labelLoginPW.UseSubFont = true;
            this.m_labelLoginPW.UseUnitFont = false;
            this.m_labelLoginPW.Click += new System.EventHandler(this.Click_ManagementLabel);
            // 
            // m_labelLoginID
            // 
            this.m_labelLoginID.BackGroundColor = System.Drawing.Color.WhiteSmoke;
            this.m_labelLoginID.BorderStroke = 2;
            this.m_labelLoginID.BorderStyle = System.Windows.Forms.ButtonBorderStyle.Solid;
            this.m_labelLoginID.Description = "";
            this.m_labelLoginID.DisabledColor = System.Drawing.Color.LightGray;
            this.m_labelLoginID.EdgeRadius = 1;
            this.m_labelLoginID.Enabled = false;
            this.m_labelLoginID.ImagePosition = new System.Drawing.Point(0, 0);
            this.m_labelLoginID.ImageSize = new System.Drawing.Point(0, 0);
            this.m_labelLoginID.LoadImage = null;
            this.m_labelLoginID.Location = new System.Drawing.Point(60, 449);
            this.m_labelLoginID.MainFont = new System.Drawing.Font("맑은 고딕", 12F, System.Drawing.FontStyle.Bold);
            this.m_labelLoginID.MainFontColor = System.Drawing.Color.DimGray;
            this.m_labelLoginID.Name = "m_labelLoginID";
            this.m_labelLoginID.Size = new System.Drawing.Size(382, 40);
            this.m_labelLoginID.SubFont = new System.Drawing.Font("맑은 고딕", 14F, System.Drawing.FontStyle.Bold);
            this.m_labelLoginID.SubFontColor = System.Drawing.Color.Gray;
            this.m_labelLoginID.SubText = "▶";
            this.m_labelLoginID.TabIndex = 0;
            this.m_labelLoginID.Text = "Enter User ID";
            this.m_labelLoginID.TextAlignMain = Sys3Controls.EN_TEXTALIGN.MIDDLE_LEFT;
            this.m_labelLoginID.TextAlignSub = Sys3Controls.EN_TEXTALIGN.MIDDLE_RIGHT;
            this.m_labelLoginID.TextAlignUnit = Sys3Controls.EN_TEXTALIGN.MIDDLE_CENTER;
            this.m_labelLoginID.ThemeIndex = 0;
            this.m_labelLoginID.UnitAreaRate = 40;
            this.m_labelLoginID.UnitFont = new System.Drawing.Font("맑은 고딕", 10F, System.Drawing.FontStyle.Bold);
            this.m_labelLoginID.UnitFontColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.m_labelLoginID.UnitPositionVertical = false;
            this.m_labelLoginID.UnitText = "";
            this.m_labelLoginID.UseBorder = true;
            this.m_labelLoginID.UseEdgeRadius = false;
            this.m_labelLoginID.UseImage = false;
            this.m_labelLoginID.UseSubFont = true;
            this.m_labelLoginID.UseUnitFont = false;
            this.m_labelLoginID.Click += new System.EventHandler(this.Click_ManagementLabel);
            // 
            // m_labelPW
            // 
            this.m_labelPW.BackGroundColor = System.Drawing.Color.WhiteSmoke;
            this.m_labelPW.BorderStroke = 2;
            this.m_labelPW.BorderStyle = System.Windows.Forms.ButtonBorderStyle.Solid;
            this.m_labelPW.Description = "";
            this.m_labelPW.DisabledColor = System.Drawing.Color.LightGray;
            this.m_labelPW.EdgeRadius = 1;
            this.m_labelPW.ImagePosition = new System.Drawing.Point(0, 0);
            this.m_labelPW.ImageSize = new System.Drawing.Point(0, 0);
            this.m_labelPW.LoadImage = null;
            this.m_labelPW.Location = new System.Drawing.Point(444, 447);
            this.m_labelPW.MainFont = new System.Drawing.Font("맑은 고딕", 12F, System.Drawing.FontStyle.Bold);
            this.m_labelPW.MainFontColor = System.Drawing.Color.DimGray;
            this.m_labelPW.Name = "m_labelPW";
            this.m_labelPW.Size = new System.Drawing.Size(410, 40);
            this.m_labelPW.SubFont = new System.Drawing.Font("맑은 고딕", 14F, System.Drawing.FontStyle.Bold);
            this.m_labelPW.SubFontColor = System.Drawing.Color.Gray;
            this.m_labelPW.SubText = "▶";
            this.m_labelPW.TabIndex = 1;
            this.m_labelPW.Text = "Enter Your Password";
            this.m_labelPW.TextAlignMain = Sys3Controls.EN_TEXTALIGN.MIDDLE_LEFT;
            this.m_labelPW.TextAlignSub = Sys3Controls.EN_TEXTALIGN.MIDDLE_RIGHT;
            this.m_labelPW.TextAlignUnit = Sys3Controls.EN_TEXTALIGN.MIDDLE_CENTER;
            this.m_labelPW.ThemeIndex = 0;
            this.m_labelPW.UnitAreaRate = 40;
            this.m_labelPW.UnitFont = new System.Drawing.Font("맑은 고딕", 10F, System.Drawing.FontStyle.Bold);
            this.m_labelPW.UnitFontColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.m_labelPW.UnitPositionVertical = false;
            this.m_labelPW.UnitText = "";
            this.m_labelPW.UseBorder = true;
            this.m_labelPW.UseEdgeRadius = false;
            this.m_labelPW.UseImage = false;
            this.m_labelPW.UseSubFont = true;
            this.m_labelPW.UseUnitFont = false;
            this.m_labelPW.Click += new System.EventHandler(this.Click_Password);
            // 
            // m_labelID
            // 
            this.m_labelID.BackGroundColor = System.Drawing.Color.WhiteSmoke;
            this.m_labelID.BorderStroke = 2;
            this.m_labelID.BorderStyle = System.Windows.Forms.ButtonBorderStyle.Solid;
            this.m_labelID.Description = "";
            this.m_labelID.DisabledColor = System.Drawing.Color.LightGray;
            this.m_labelID.EdgeRadius = 1;
            this.m_labelID.Enabled = false;
            this.m_labelID.ImagePosition = new System.Drawing.Point(0, 0);
            this.m_labelID.ImageSize = new System.Drawing.Point(0, 0);
            this.m_labelID.LoadImage = null;
            this.m_labelID.Location = new System.Drawing.Point(444, 397);
            this.m_labelID.MainFont = new System.Drawing.Font("맑은 고딕", 12F, System.Drawing.FontStyle.Bold);
            this.m_labelID.MainFontColor = System.Drawing.Color.DimGray;
            this.m_labelID.Name = "m_labelID";
            this.m_labelID.Size = new System.Drawing.Size(370, 40);
            this.m_labelID.SubFont = new System.Drawing.Font("맑은 고딕", 10F);
            this.m_labelID.SubFontColor = System.Drawing.Color.Black;
            this.m_labelID.SubText = "";
            this.m_labelID.TabIndex = 0;
            this.m_labelID.Text = "Enter Your ID";
            this.m_labelID.TextAlignMain = Sys3Controls.EN_TEXTALIGN.MIDDLE_LEFT;
            this.m_labelID.TextAlignSub = Sys3Controls.EN_TEXTALIGN.MIDDLE_CENTER;
            this.m_labelID.TextAlignUnit = Sys3Controls.EN_TEXTALIGN.MIDDLE_CENTER;
            this.m_labelID.ThemeIndex = 0;
            this.m_labelID.UnitAreaRate = 40;
            this.m_labelID.UnitFont = new System.Drawing.Font("맑은 고딕", 10F, System.Drawing.FontStyle.Bold);
            this.m_labelID.UnitFontColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.m_labelID.UnitPositionVertical = false;
            this.m_labelID.UnitText = "";
            this.m_labelID.UseBorder = true;
            this.m_labelID.UseEdgeRadius = false;
            this.m_labelID.UseImage = false;
            this.m_labelID.UseSubFont = false;
            this.m_labelID.UseUnitFont = false;
            // 
            // m_groupLogin
            // 
            this.m_groupLogin.BackColor = System.Drawing.SystemColors.Control;
            this.m_groupLogin.BackGroundColor = System.Drawing.Color.WhiteSmoke;
            this.m_groupLogin.EdgeBorderStroke = 2;
            this.m_groupLogin.EdgeRadius = 5;
            this.m_groupLogin.LabelFont = new System.Drawing.Font("맑은 고딕", 40F, System.Drawing.FontStyle.Bold);
            this.m_groupLogin.LabelGradientColorFirst = System.Drawing.Color.FromArgb(((int)(((byte)(240)))), ((int)(((byte)(240)))), ((int)(((byte)(240)))));
            this.m_groupLogin.LabelGradientColorSecond = System.Drawing.Color.FromArgb(((int)(((byte)(208)))), ((int)(((byte)(208)))), ((int)(((byte)(208)))));
            this.m_groupLogin.LabelHeight = 100;
            this.m_groupLogin.LabelTextColor = System.Drawing.Color.DimGray;
            this.m_groupLogin.Location = new System.Drawing.Point(1, 1);
            this.m_groupLogin.Name = "m_groupLogin";
            this.m_groupLogin.Size = new System.Drawing.Size(1280, 958);
            this.m_groupLogin.TabIndex = 1366;
            this.m_groupLogin.Text = "LOGIN";
            this.m_groupLogin.TextAlign = Sys3Controls.EN_TEXTALIGN.MIDDLE_CENTER;
            this.m_groupLogin.ThemeIndex = 0;
            this.m_groupLogin.UseLabelBorder = true;
            // 
            // sys3GroupBox2
            // 
            this.sys3GroupBox2.BackColor = System.Drawing.SystemColors.Control;
            this.sys3GroupBox2.BackGroundColor = System.Drawing.Color.WhiteSmoke;
            this.sys3GroupBox2.EdgeBorderStroke = 2;
            this.sys3GroupBox2.EdgeRadius = 5;
            this.sys3GroupBox2.LabelFont = new System.Drawing.Font("맑은 고딕", 12F, System.Drawing.FontStyle.Bold);
            this.sys3GroupBox2.LabelGradientColorFirst = System.Drawing.Color.FromArgb(((int)(((byte)(208)))), ((int)(((byte)(208)))), ((int)(((byte)(208)))));
            this.sys3GroupBox2.LabelGradientColorSecond = System.Drawing.Color.FromArgb(((int)(((byte)(240)))), ((int)(((byte)(240)))), ((int)(((byte)(240)))));
            this.sys3GroupBox2.LabelHeight = 60;
            this.sys3GroupBox2.LabelTextColor = System.Drawing.Color.DimGray;
            this.sys3GroupBox2.Location = new System.Drawing.Point(23, 353);
            this.sys3GroupBox2.Name = "sys3GroupBox2";
            this.sys3GroupBox2.Size = new System.Drawing.Size(1228, 507);
            this.sys3GroupBox2.TabIndex = 1368;
            this.sys3GroupBox2.Text = "USER MANAGEMENT";
            this.sys3GroupBox2.TextAlign = Sys3Controls.EN_TEXTALIGN.MIDDLE_CENTER;
            this.sys3GroupBox2.ThemeIndex = 0;
            this.sys3GroupBox2.UseLabelBorder = true;
            // 
            // m_btnProtec
            // 
            this.m_btnProtec.BorderWidth = 3;
            this.m_btnProtec.ButtonClicked = false;
            this.m_btnProtec.ClickedEmphasizeTextColor = System.Drawing.Color.White;
            this.m_btnProtec.CustomClickedGradientFirstColor = System.Drawing.Color.White;
            this.m_btnProtec.CustomClickedGradientSecondColor = System.Drawing.Color.White;
            this.m_btnProtec.Description = "";
            this.m_btnProtec.DisabledColor = System.Drawing.Color.WhiteSmoke;
            this.m_btnProtec.EdgeRadius = 5;
            this.m_btnProtec.GradientAngle = 60F;
            this.m_btnProtec.GradientFirstColor = System.Drawing.Color.WhiteSmoke;
            this.m_btnProtec.GradientSecondColor = System.Drawing.Color.WhiteSmoke;
            this.m_btnProtec.HoverEmphasizeCustomColor = System.Drawing.Color.White;
            this.m_btnProtec.ImagePosition = new System.Drawing.Point(30, 10);
            this.m_btnProtec.ImageSize = new System.Drawing.Point(592, 126);
            this.m_btnProtec.LoadImage = global::FrameOfSystem3.Properties.Resources.프로텍_로고;
            this.m_btnProtec.Location = new System.Drawing.Point(330, 177);
            this.m_btnProtec.MainFont = new System.Drawing.Font("맑은 고딕", 15F, System.Drawing.FontStyle.Bold);
            this.m_btnProtec.MainFontColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(36)))), ((int)(((byte)(0)))));
            this.m_btnProtec.Name = "m_btnProtec";
            this.m_btnProtec.Size = new System.Drawing.Size(636, 143);
            this.m_btnProtec.SubFont = new System.Drawing.Font("맑은 고딕", 8F, System.Drawing.FontStyle.Bold);
            this.m_btnProtec.SubFontColor = System.Drawing.Color.DarkBlue;
            this.m_btnProtec.SubText = "STATUS";
            this.m_btnProtec.TabIndex = 1367;
            this.m_btnProtec.TextAlignMain = Sys3Controls.EN_TEXTALIGN.MIDDLE_RIGHT;
            this.m_btnProtec.TextAlignSub = Sys3Controls.EN_TEXTALIGN.TOP_LEFT;
            this.m_btnProtec.ThemeIndex = 0;
            this.m_btnProtec.UseBorder = true;
            this.m_btnProtec.UseClickedEmphasizeTextColor = false;
            this.m_btnProtec.UseCustomizeClickedColor = false;
            this.m_btnProtec.UseEdge = true;
            this.m_btnProtec.UseHoverEmphasizeCustomColor = false;
            this.m_btnProtec.UseImage = true;
            this.m_btnProtec.UserHoverEmpahsize = false;
            this.m_btnProtec.UseSubFont = false;
            // 
            // m_btnCancel
            // 
            this.m_btnCancel.BorderWidth = 3;
            this.m_btnCancel.ButtonClicked = false;
            this.m_btnCancel.ClickedEmphasizeTextColor = System.Drawing.Color.White;
            this.m_btnCancel.CustomClickedGradientFirstColor = System.Drawing.Color.White;
            this.m_btnCancel.CustomClickedGradientSecondColor = System.Drawing.Color.White;
            this.m_btnCancel.Description = "";
            this.m_btnCancel.DisabledColor = System.Drawing.Color.DarkGray;
            this.m_btnCancel.EdgeRadius = 5;
            this.m_btnCancel.GradientAngle = 60F;
            this.m_btnCancel.GradientFirstColor = System.Drawing.Color.White;
            this.m_btnCancel.GradientSecondColor = System.Drawing.Color.FromArgb(((int)(((byte)(170)))), ((int)(((byte)(176)))), ((int)(((byte)(183)))));
            this.m_btnCancel.HoverEmphasizeCustomColor = System.Drawing.Color.White;
            this.m_btnCancel.ImagePosition = new System.Drawing.Point(10, 10);
            this.m_btnCancel.ImageSize = new System.Drawing.Point(40, 40);
            this.m_btnCancel.LoadImage = global::FrameOfSystem3.Properties.Resources.Main_Exit_blueblack;
            this.m_btnCancel.Location = new System.Drawing.Point(1093, 876);
            this.m_btnCancel.MainFont = new System.Drawing.Font("맑은 고딕", 15F, System.Drawing.FontStyle.Bold);
            this.m_btnCancel.MainFontColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(36)))), ((int)(((byte)(0)))));
            this.m_btnCancel.Name = "m_btnCancel";
            this.m_btnCancel.Size = new System.Drawing.Size(175, 70);
            this.m_btnCancel.SubFont = new System.Drawing.Font("맑은 고딕", 8F, System.Drawing.FontStyle.Bold);
            this.m_btnCancel.SubFontColor = System.Drawing.Color.DarkBlue;
            this.m_btnCancel.SubText = "STATUS";
            this.m_btnCancel.TabIndex = 1;
            this.m_btnCancel.Text = "CANCEL";
            this.m_btnCancel.TextAlignMain = Sys3Controls.EN_TEXTALIGN.MIDDLE_RIGHT;
            this.m_btnCancel.TextAlignSub = Sys3Controls.EN_TEXTALIGN.TOP_LEFT;
            this.m_btnCancel.ThemeIndex = 0;
            this.m_btnCancel.UseBorder = true;
            this.m_btnCancel.UseClickedEmphasizeTextColor = false;
            this.m_btnCancel.UseCustomizeClickedColor = false;
            this.m_btnCancel.UseEdge = true;
            this.m_btnCancel.UseHoverEmphasizeCustomColor = false;
            this.m_btnCancel.UseImage = true;
            this.m_btnCancel.UserHoverEmpahsize = false;
            this.m_btnCancel.UseSubFont = false;
            this.m_btnCancel.Click += new System.EventHandler(this.Click_ApplyOrCancel);
            // 
            // m_btnCloseManagement
            // 
            this.m_btnCloseManagement.BorderWidth = 3;
            this.m_btnCloseManagement.ButtonClicked = false;
            this.m_btnCloseManagement.ClickedEmphasizeTextColor = System.Drawing.Color.White;
            this.m_btnCloseManagement.CustomClickedGradientFirstColor = System.Drawing.Color.White;
            this.m_btnCloseManagement.CustomClickedGradientSecondColor = System.Drawing.Color.White;
            this.m_btnCloseManagement.Description = "";
            this.m_btnCloseManagement.DisabledColor = System.Drawing.Color.DarkGray;
            this.m_btnCloseManagement.EdgeRadius = 5;
            this.m_btnCloseManagement.GradientAngle = 60F;
            this.m_btnCloseManagement.GradientFirstColor = System.Drawing.Color.White;
            this.m_btnCloseManagement.GradientSecondColor = System.Drawing.Color.FromArgb(((int)(((byte)(170)))), ((int)(((byte)(176)))), ((int)(((byte)(183)))));
            this.m_btnCloseManagement.HoverEmphasizeCustomColor = System.Drawing.Color.White;
            this.m_btnCloseManagement.ImagePosition = new System.Drawing.Point(10, 7);
            this.m_btnCloseManagement.ImageSize = new System.Drawing.Point(26, 26);
            this.m_btnCloseManagement.LoadImage = global::FrameOfSystem3.Properties.Resources.Main_Exit_blueblack;
            this.m_btnCloseManagement.Location = new System.Drawing.Point(1084, 367);
            this.m_btnCloseManagement.MainFont = new System.Drawing.Font("맑은 고딕", 15F, System.Drawing.FontStyle.Bold);
            this.m_btnCloseManagement.MainFontColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(36)))), ((int)(((byte)(0)))));
            this.m_btnCloseManagement.Name = "m_btnCloseManagement";
            this.m_btnCloseManagement.Size = new System.Drawing.Size(154, 39);
            this.m_btnCloseManagement.SubFont = new System.Drawing.Font("맑은 고딕", 8F, System.Drawing.FontStyle.Bold);
            this.m_btnCloseManagement.SubFontColor = System.Drawing.Color.DarkBlue;
            this.m_btnCloseManagement.SubText = "STATUS";
            this.m_btnCloseManagement.TabIndex = 1369;
            this.m_btnCloseManagement.Text = "CLOSE";
            this.m_btnCloseManagement.TextAlignMain = Sys3Controls.EN_TEXTALIGN.MIDDLE_RIGHT;
            this.m_btnCloseManagement.TextAlignSub = Sys3Controls.EN_TEXTALIGN.TOP_LEFT;
            this.m_btnCloseManagement.ThemeIndex = 0;
            this.m_btnCloseManagement.UseBorder = true;
            this.m_btnCloseManagement.UseClickedEmphasizeTextColor = false;
            this.m_btnCloseManagement.UseCustomizeClickedColor = false;
            this.m_btnCloseManagement.UseEdge = true;
            this.m_btnCloseManagement.UseHoverEmphasizeCustomColor = false;
            this.m_btnCloseManagement.UseImage = true;
            this.m_btnCloseManagement.UserHoverEmpahsize = false;
            this.m_btnCloseManagement.UseSubFont = false;
            this.m_btnCloseManagement.Click += new System.EventHandler(this.Click_CloseManagement);
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
            this.m_btnRemove.EdgeRadius = 5;
            this.m_btnRemove.Enabled = false;
            this.m_btnRemove.GradientAngle = 60F;
            this.m_btnRemove.GradientFirstColor = System.Drawing.Color.White;
            this.m_btnRemove.GradientSecondColor = System.Drawing.Color.FromArgb(((int)(((byte)(170)))), ((int)(((byte)(176)))), ((int)(((byte)(183)))));
            this.m_btnRemove.HoverEmphasizeCustomColor = System.Drawing.Color.White;
            this.m_btnRemove.ImagePosition = new System.Drawing.Point(10, 7);
            this.m_btnRemove.ImageSize = new System.Drawing.Point(26, 26);
            this.m_btnRemove.LoadImage = global::FrameOfSystem3.Properties.Resources.REMOVE;
            this.m_btnRemove.Location = new System.Drawing.Point(334, 748);
            this.m_btnRemove.MainFont = new System.Drawing.Font("맑은 고딕", 15F, System.Drawing.FontStyle.Bold);
            this.m_btnRemove.MainFontColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(36)))), ((int)(((byte)(0)))));
            this.m_btnRemove.Name = "m_btnRemove";
            this.m_btnRemove.Size = new System.Drawing.Size(143, 97);
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
            this.m_btnRemove.Click += new System.EventHandler(this.Click_ManagementButtons);
            // 
            // m_btnModify
            // 
            this.m_btnModify.BorderWidth = 3;
            this.m_btnModify.ButtonClicked = false;
            this.m_btnModify.ClickedEmphasizeTextColor = System.Drawing.Color.White;
            this.m_btnModify.CustomClickedGradientFirstColor = System.Drawing.Color.White;
            this.m_btnModify.CustomClickedGradientSecondColor = System.Drawing.Color.White;
            this.m_btnModify.Description = "";
            this.m_btnModify.DisabledColor = System.Drawing.Color.DarkGray;
            this.m_btnModify.EdgeRadius = 5;
            this.m_btnModify.Enabled = false;
            this.m_btnModify.GradientAngle = 60F;
            this.m_btnModify.GradientFirstColor = System.Drawing.Color.White;
            this.m_btnModify.GradientSecondColor = System.Drawing.Color.FromArgb(((int)(((byte)(170)))), ((int)(((byte)(176)))), ((int)(((byte)(183)))));
            this.m_btnModify.HoverEmphasizeCustomColor = System.Drawing.Color.White;
            this.m_btnModify.ImagePosition = new System.Drawing.Point(10, 7);
            this.m_btnModify.ImageSize = new System.Drawing.Point(26, 26);
            this.m_btnModify.LoadImage = global::FrameOfSystem3.Properties.Resources.MODIFY_CHANGE;
            this.m_btnModify.Location = new System.Drawing.Point(185, 748);
            this.m_btnModify.MainFont = new System.Drawing.Font("맑은 고딕", 15F, System.Drawing.FontStyle.Bold);
            this.m_btnModify.MainFontColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(36)))), ((int)(((byte)(0)))));
            this.m_btnModify.Name = "m_btnModify";
            this.m_btnModify.Size = new System.Drawing.Size(143, 97);
            this.m_btnModify.SubFont = new System.Drawing.Font("맑은 고딕", 8F, System.Drawing.FontStyle.Bold);
            this.m_btnModify.SubFontColor = System.Drawing.Color.DarkBlue;
            this.m_btnModify.SubText = "STATUS";
            this.m_btnModify.TabIndex = 1;
            this.m_btnModify.Text = "MODIFY";
            this.m_btnModify.TextAlignMain = Sys3Controls.EN_TEXTALIGN.MIDDLE_RIGHT;
            this.m_btnModify.TextAlignSub = Sys3Controls.EN_TEXTALIGN.TOP_LEFT;
            this.m_btnModify.ThemeIndex = 0;
            this.m_btnModify.UseBorder = true;
            this.m_btnModify.UseClickedEmphasizeTextColor = false;
            this.m_btnModify.UseCustomizeClickedColor = false;
            this.m_btnModify.UseEdge = true;
            this.m_btnModify.UseHoverEmphasizeCustomColor = false;
            this.m_btnModify.UseImage = true;
            this.m_btnModify.UserHoverEmpahsize = false;
            this.m_btnModify.UseSubFont = false;
            this.m_btnModify.Click += new System.EventHandler(this.Click_ManagementButtons);
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
            this.m_btnAdd.EdgeRadius = 5;
            this.m_btnAdd.GradientAngle = 60F;
            this.m_btnAdd.GradientFirstColor = System.Drawing.Color.White;
            this.m_btnAdd.GradientSecondColor = System.Drawing.Color.FromArgb(((int)(((byte)(170)))), ((int)(((byte)(176)))), ((int)(((byte)(183)))));
            this.m_btnAdd.HoverEmphasizeCustomColor = System.Drawing.Color.White;
            this.m_btnAdd.ImagePosition = new System.Drawing.Point(10, 7);
            this.m_btnAdd.ImageSize = new System.Drawing.Point(26, 26);
            this.m_btnAdd.LoadImage = global::FrameOfSystem3.Properties.Resources.ADD_ID_BLACK;
            this.m_btnAdd.Location = new System.Drawing.Point(36, 748);
            this.m_btnAdd.MainFont = new System.Drawing.Font("맑은 고딕", 15F, System.Drawing.FontStyle.Bold);
            this.m_btnAdd.MainFontColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(36)))), ((int)(((byte)(0)))));
            this.m_btnAdd.Name = "m_btnAdd";
            this.m_btnAdd.Size = new System.Drawing.Size(143, 97);
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
            this.m_btnAdd.Click += new System.EventHandler(this.Click_ManagementButtons);
            // 
            // m_btnAuthority
            // 
            this.m_btnAuthority.BorderWidth = 2;
            this.m_btnAuthority.ButtonClicked = false;
            this.m_btnAuthority.ClickedEmphasizeTextColor = System.Drawing.Color.White;
            this.m_btnAuthority.CustomClickedGradientFirstColor = System.Drawing.Color.White;
            this.m_btnAuthority.CustomClickedGradientSecondColor = System.Drawing.Color.White;
            this.m_btnAuthority.Description = "";
            this.m_btnAuthority.DisabledColor = System.Drawing.Color.DarkGray;
            this.m_btnAuthority.EdgeRadius = 5;
            this.m_btnAuthority.Enabled = false;
            this.m_btnAuthority.GradientAngle = 45F;
            this.m_btnAuthority.GradientFirstColor = System.Drawing.Color.White;
            this.m_btnAuthority.GradientSecondColor = System.Drawing.Color.FromArgb(((int)(((byte)(170)))), ((int)(((byte)(176)))), ((int)(((byte)(183)))));
            this.m_btnAuthority.HoverEmphasizeCustomColor = System.Drawing.Color.White;
            this.m_btnAuthority.ImagePosition = new System.Drawing.Point(13, 13);
            this.m_btnAuthority.ImageSize = new System.Drawing.Point(30, 30);
            this.m_btnAuthority.LoadImage = global::FrameOfSystem3.Properties.Resources.CREATE_FILE;
            this.m_btnAuthority.Location = new System.Drawing.Point(386, 548);
            this.m_btnAuthority.MainFont = new System.Drawing.Font("맑은 고딕", 15F, System.Drawing.FontStyle.Bold);
            this.m_btnAuthority.MainFontColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(36)))), ((int)(((byte)(0)))));
            this.m_btnAuthority.Name = "m_btnAuthority";
            this.m_btnAuthority.Size = new System.Drawing.Size(56, 42);
            this.m_btnAuthority.SubFont = new System.Drawing.Font("맑은 고딕", 8F, System.Drawing.FontStyle.Bold);
            this.m_btnAuthority.SubFontColor = System.Drawing.Color.DarkBlue;
            this.m_btnAuthority.SubText = "STATUS";
            this.m_btnAuthority.TabIndex = 1;
            this.m_btnAuthority.Text = "▼";
            this.m_btnAuthority.TextAlignMain = Sys3Controls.EN_TEXTALIGN.MIDDLE_CENTER;
            this.m_btnAuthority.TextAlignSub = Sys3Controls.EN_TEXTALIGN.TOP_LEFT;
            this.m_btnAuthority.ThemeIndex = 0;
            this.m_btnAuthority.UseBorder = false;
            this.m_btnAuthority.UseClickedEmphasizeTextColor = false;
            this.m_btnAuthority.UseCustomizeClickedColor = false;
            this.m_btnAuthority.UseEdge = false;
            this.m_btnAuthority.UseHoverEmphasizeCustomColor = false;
            this.m_btnAuthority.UseImage = false;
            this.m_btnAuthority.UserHoverEmpahsize = false;
            this.m_btnAuthority.UseSubFont = false;
            this.m_btnAuthority.Click += new System.EventHandler(this.Click_DownList);
            // 
            // m_btnUseManagement
            // 
            this.m_btnUseManagement.BorderWidth = 3;
            this.m_btnUseManagement.ButtonClicked = false;
            this.m_btnUseManagement.ClickedEmphasizeTextColor = System.Drawing.Color.White;
            this.m_btnUseManagement.CustomClickedGradientFirstColor = System.Drawing.Color.White;
            this.m_btnUseManagement.CustomClickedGradientSecondColor = System.Drawing.Color.White;
            this.m_btnUseManagement.Description = "";
            this.m_btnUseManagement.DisabledColor = System.Drawing.Color.DarkGray;
            this.m_btnUseManagement.EdgeRadius = 5;
            this.m_btnUseManagement.GradientAngle = 60F;
            this.m_btnUseManagement.GradientFirstColor = System.Drawing.Color.White;
            this.m_btnUseManagement.GradientSecondColor = System.Drawing.Color.FromArgb(((int)(((byte)(170)))), ((int)(((byte)(176)))), ((int)(((byte)(183)))));
            this.m_btnUseManagement.HoverEmphasizeCustomColor = System.Drawing.Color.White;
            this.m_btnUseManagement.ImagePosition = new System.Drawing.Point(10, 10);
            this.m_btnUseManagement.ImageSize = new System.Drawing.Point(40, 40);
            this.m_btnUseManagement.LoadImage = global::FrameOfSystem3.Properties.Resources.Main_User_blueblack;
            this.m_btnUseManagement.Location = new System.Drawing.Point(444, 706);
            this.m_btnUseManagement.MainFont = new System.Drawing.Font("맑은 고딕", 15F, System.Drawing.FontStyle.Bold);
            this.m_btnUseManagement.MainFontColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(36)))), ((int)(((byte)(0)))));
            this.m_btnUseManagement.Name = "m_btnUseManagement";
            this.m_btnUseManagement.Size = new System.Drawing.Size(410, 64);
            this.m_btnUseManagement.SubFont = new System.Drawing.Font("맑은 고딕", 8F, System.Drawing.FontStyle.Bold);
            this.m_btnUseManagement.SubFontColor = System.Drawing.Color.DarkBlue;
            this.m_btnUseManagement.SubText = "STATUS";
            this.m_btnUseManagement.TabIndex = 1370;
            this.m_btnUseManagement.Text = "MANAGEMENT";
            this.m_btnUseManagement.TextAlignMain = Sys3Controls.EN_TEXTALIGN.MIDDLE_RIGHT;
            this.m_btnUseManagement.TextAlignSub = Sys3Controls.EN_TEXTALIGN.TOP_LEFT;
            this.m_btnUseManagement.ThemeIndex = 0;
            this.m_btnUseManagement.UseBorder = true;
            this.m_btnUseManagement.UseClickedEmphasizeTextColor = false;
            this.m_btnUseManagement.UseCustomizeClickedColor = false;
            this.m_btnUseManagement.UseEdge = true;
            this.m_btnUseManagement.UseHoverEmphasizeCustomColor = false;
            this.m_btnUseManagement.UseImage = true;
            this.m_btnUseManagement.UserHoverEmpahsize = false;
            this.m_btnUseManagement.UseSubFont = false;
            this.m_btnUseManagement.Click += new System.EventHandler(this.Click_OpenManagement);
            // 
            // m_btnLogout
            // 
            this.m_btnLogout.BorderWidth = 3;
            this.m_btnLogout.ButtonClicked = false;
            this.m_btnLogout.ClickedEmphasizeTextColor = System.Drawing.Color.White;
            this.m_btnLogout.CustomClickedGradientFirstColor = System.Drawing.Color.White;
            this.m_btnLogout.CustomClickedGradientSecondColor = System.Drawing.Color.White;
            this.m_btnLogout.Description = "";
            this.m_btnLogout.DisabledColor = System.Drawing.Color.DarkGray;
            this.m_btnLogout.EdgeRadius = 5;
            this.m_btnLogout.GradientAngle = 60F;
            this.m_btnLogout.GradientFirstColor = System.Drawing.Color.White;
            this.m_btnLogout.GradientSecondColor = System.Drawing.Color.FromArgb(((int)(((byte)(170)))), ((int)(((byte)(176)))), ((int)(((byte)(183)))));
            this.m_btnLogout.HoverEmphasizeCustomColor = System.Drawing.Color.White;
            this.m_btnLogout.ImagePosition = new System.Drawing.Point(10, 10);
            this.m_btnLogout.ImageSize = new System.Drawing.Point(40, 40);
            this.m_btnLogout.LoadImage = global::FrameOfSystem3.Properties.Resources.LOCK_BLACK;
            this.m_btnLogout.Location = new System.Drawing.Point(444, 636);
            this.m_btnLogout.MainFont = new System.Drawing.Font("맑은 고딕", 15F, System.Drawing.FontStyle.Bold);
            this.m_btnLogout.MainFontColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(36)))), ((int)(((byte)(0)))));
            this.m_btnLogout.Name = "m_btnLogout";
            this.m_btnLogout.Size = new System.Drawing.Size(410, 64);
            this.m_btnLogout.SubFont = new System.Drawing.Font("맑은 고딕", 8F, System.Drawing.FontStyle.Bold);
            this.m_btnLogout.SubFontColor = System.Drawing.Color.DarkBlue;
            this.m_btnLogout.SubText = "STATUS";
            this.m_btnLogout.TabIndex = 2;
            this.m_btnLogout.Text = "LOGOUT";
            this.m_btnLogout.TextAlignMain = Sys3Controls.EN_TEXTALIGN.MIDDLE_RIGHT;
            this.m_btnLogout.TextAlignSub = Sys3Controls.EN_TEXTALIGN.TOP_LEFT;
            this.m_btnLogout.ThemeIndex = 0;
            this.m_btnLogout.UseBorder = true;
            this.m_btnLogout.UseClickedEmphasizeTextColor = false;
            this.m_btnLogout.UseCustomizeClickedColor = false;
            this.m_btnLogout.UseEdge = true;
            this.m_btnLogout.UseHoverEmphasizeCustomColor = false;
            this.m_btnLogout.UseImage = true;
            this.m_btnLogout.UserHoverEmpahsize = false;
            this.m_btnLogout.UseSubFont = false;
            this.m_btnLogout.Click += new System.EventHandler(this.Click_Logout);
            // 
            // m_btnLogin
            // 
            this.m_btnLogin.BorderWidth = 3;
            this.m_btnLogin.ButtonClicked = false;
            this.m_btnLogin.ClickedEmphasizeTextColor = System.Drawing.Color.White;
            this.m_btnLogin.CustomClickedGradientFirstColor = System.Drawing.Color.White;
            this.m_btnLogin.CustomClickedGradientSecondColor = System.Drawing.Color.White;
            this.m_btnLogin.Description = "";
            this.m_btnLogin.DisabledColor = System.Drawing.Color.DimGray;
            this.m_btnLogin.EdgeRadius = 5;
            this.m_btnLogin.GradientAngle = 60F;
            this.m_btnLogin.GradientFirstColor = System.Drawing.Color.FromArgb(((int)(((byte)(115)))), ((int)(((byte)(150)))), ((int)(((byte)(255)))));
            this.m_btnLogin.GradientSecondColor = System.Drawing.Color.FromArgb(((int)(((byte)(30)))), ((int)(((byte)(65)))), ((int)(((byte)(200)))));
            this.m_btnLogin.HoverEmphasizeCustomColor = System.Drawing.Color.White;
            this.m_btnLogin.ImagePosition = new System.Drawing.Point(10, 10);
            this.m_btnLogin.ImageSize = new System.Drawing.Point(40, 40);
            this.m_btnLogin.LoadImage = global::FrameOfSystem3.Properties.Resources.UNLOCK_WHITE;
            this.m_btnLogin.Location = new System.Drawing.Point(444, 497);
            this.m_btnLogin.MainFont = new System.Drawing.Font("맑은 고딕", 15F, System.Drawing.FontStyle.Bold);
            this.m_btnLogin.MainFontColor = System.Drawing.Color.White;
            this.m_btnLogin.Name = "m_btnLogin";
            this.m_btnLogin.Size = new System.Drawing.Size(410, 64);
            this.m_btnLogin.SubFont = new System.Drawing.Font("맑은 고딕", 8F, System.Drawing.FontStyle.Bold);
            this.m_btnLogin.SubFontColor = System.Drawing.Color.DarkBlue;
            this.m_btnLogin.SubText = "STATUS";
            this.m_btnLogin.TabIndex = 0;
            this.m_btnLogin.Text = "LOGIN";
            this.m_btnLogin.TextAlignMain = Sys3Controls.EN_TEXTALIGN.MIDDLE_RIGHT;
            this.m_btnLogin.TextAlignSub = Sys3Controls.EN_TEXTALIGN.TOP_LEFT;
            this.m_btnLogin.ThemeIndex = 0;
            this.m_btnLogin.UseBorder = true;
            this.m_btnLogin.UseClickedEmphasizeTextColor = false;
            this.m_btnLogin.UseCustomizeClickedColor = false;
            this.m_btnLogin.UseEdge = true;
            this.m_btnLogin.UseHoverEmphasizeCustomColor = false;
            this.m_btnLogin.UseImage = true;
            this.m_btnLogin.UserHoverEmpahsize = false;
            this.m_btnLogin.UseSubFont = false;
            this.m_btnLogin.Click += new System.EventHandler(this.Click_ApplyOrCancel);
            // 
            // m_btnPopupList
            // 
            this.m_btnPopupList.BorderWidth = 2;
            this.m_btnPopupList.ButtonClicked = false;
            this.m_btnPopupList.ClickedEmphasizeTextColor = System.Drawing.Color.White;
            this.m_btnPopupList.CustomClickedGradientFirstColor = System.Drawing.Color.White;
            this.m_btnPopupList.CustomClickedGradientSecondColor = System.Drawing.Color.White;
            this.m_btnPopupList.Description = "";
            this.m_btnPopupList.DisabledColor = System.Drawing.Color.DarkGray;
            this.m_btnPopupList.EdgeRadius = 5;
            this.m_btnPopupList.GradientAngle = 45F;
            this.m_btnPopupList.GradientFirstColor = System.Drawing.Color.White;
            this.m_btnPopupList.GradientSecondColor = System.Drawing.Color.FromArgb(((int)(((byte)(170)))), ((int)(((byte)(176)))), ((int)(((byte)(183)))));
            this.m_btnPopupList.HoverEmphasizeCustomColor = System.Drawing.Color.White;
            this.m_btnPopupList.ImagePosition = new System.Drawing.Point(13, 13);
            this.m_btnPopupList.ImageSize = new System.Drawing.Point(30, 30);
            this.m_btnPopupList.LoadImage = global::FrameOfSystem3.Properties.Resources.CREATE_FILE;
            this.m_btnPopupList.Location = new System.Drawing.Point(813, 396);
            this.m_btnPopupList.MainFont = new System.Drawing.Font("맑은 고딕", 15F, System.Drawing.FontStyle.Bold);
            this.m_btnPopupList.MainFontColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(36)))), ((int)(((byte)(0)))));
            this.m_btnPopupList.Name = "m_btnPopupList";
            this.m_btnPopupList.Size = new System.Drawing.Size(41, 42);
            this.m_btnPopupList.SubFont = new System.Drawing.Font("맑은 고딕", 8F, System.Drawing.FontStyle.Bold);
            this.m_btnPopupList.SubFontColor = System.Drawing.Color.DarkBlue;
            this.m_btnPopupList.SubText = "STATUS";
            this.m_btnPopupList.TabIndex = 0;
            this.m_btnPopupList.Text = "▼";
            this.m_btnPopupList.TextAlignMain = Sys3Controls.EN_TEXTALIGN.MIDDLE_CENTER;
            this.m_btnPopupList.TextAlignSub = Sys3Controls.EN_TEXTALIGN.TOP_LEFT;
            this.m_btnPopupList.ThemeIndex = 0;
            this.m_btnPopupList.UseBorder = false;
            this.m_btnPopupList.UseClickedEmphasizeTextColor = false;
            this.m_btnPopupList.UseCustomizeClickedColor = false;
            this.m_btnPopupList.UseEdge = false;
            this.m_btnPopupList.UseHoverEmphasizeCustomColor = false;
            this.m_btnPopupList.UseImage = false;
            this.m_btnPopupList.UserHoverEmpahsize = false;
            this.m_btnPopupList.UseSubFont = false;
            this.m_btnPopupList.Click += new System.EventHandler(this.Click_DownList);
            // 
            // m_btnLoginIrisRecognition
            // 
            this.m_btnLoginIrisRecognition.BorderWidth = 3;
            this.m_btnLoginIrisRecognition.ButtonClicked = false;
            this.m_btnLoginIrisRecognition.ClickedEmphasizeTextColor = System.Drawing.Color.White;
            this.m_btnLoginIrisRecognition.CustomClickedGradientFirstColor = System.Drawing.Color.White;
            this.m_btnLoginIrisRecognition.CustomClickedGradientSecondColor = System.Drawing.Color.White;
            this.m_btnLoginIrisRecognition.Description = "";
            this.m_btnLoginIrisRecognition.DisabledColor = System.Drawing.Color.DimGray;
            this.m_btnLoginIrisRecognition.EdgeRadius = 5;
            this.m_btnLoginIrisRecognition.GradientAngle = 60F;
            this.m_btnLoginIrisRecognition.GradientFirstColor = System.Drawing.Color.FromArgb(((int)(((byte)(115)))), ((int)(((byte)(150)))), ((int)(((byte)(255)))));
            this.m_btnLoginIrisRecognition.GradientSecondColor = System.Drawing.Color.FromArgb(((int)(((byte)(30)))), ((int)(((byte)(65)))), ((int)(((byte)(200)))));
            this.m_btnLoginIrisRecognition.HoverEmphasizeCustomColor = System.Drawing.Color.White;
            this.m_btnLoginIrisRecognition.ImagePosition = new System.Drawing.Point(10, 10);
            this.m_btnLoginIrisRecognition.ImageSize = new System.Drawing.Point(40, 40);
            this.m_btnLoginIrisRecognition.LoadImage = global::FrameOfSystem3.Properties.Resources.UNLOCK_WHITE;
            this.m_btnLoginIrisRecognition.Location = new System.Drawing.Point(444, 567);
            this.m_btnLoginIrisRecognition.MainFont = new System.Drawing.Font("맑은 고딕", 15F, System.Drawing.FontStyle.Bold);
            this.m_btnLoginIrisRecognition.MainFontColor = System.Drawing.Color.White;
            this.m_btnLoginIrisRecognition.Name = "m_btnLoginIrisRecognition";
            this.m_btnLoginIrisRecognition.Size = new System.Drawing.Size(410, 64);
            this.m_btnLoginIrisRecognition.SubFont = new System.Drawing.Font("맑은 고딕", 8F, System.Drawing.FontStyle.Bold);
            this.m_btnLoginIrisRecognition.SubFontColor = System.Drawing.Color.DarkBlue;
            this.m_btnLoginIrisRecognition.SubText = "STATUS";
            this.m_btnLoginIrisRecognition.TabIndex = 1371;
            this.m_btnLoginIrisRecognition.Text = "IRIS RECOGNITION";
            this.m_btnLoginIrisRecognition.TextAlignMain = Sys3Controls.EN_TEXTALIGN.MIDDLE_RIGHT;
            this.m_btnLoginIrisRecognition.TextAlignSub = Sys3Controls.EN_TEXTALIGN.TOP_LEFT;
            this.m_btnLoginIrisRecognition.ThemeIndex = 0;
            this.m_btnLoginIrisRecognition.UseBorder = true;
            this.m_btnLoginIrisRecognition.UseClickedEmphasizeTextColor = false;
            this.m_btnLoginIrisRecognition.UseCustomizeClickedColor = false;
            this.m_btnLoginIrisRecognition.UseEdge = true;
            this.m_btnLoginIrisRecognition.UseHoverEmphasizeCustomColor = false;
            this.m_btnLoginIrisRecognition.UseImage = true;
            this.m_btnLoginIrisRecognition.UserHoverEmpahsize = false;
            this.m_btnLoginIrisRecognition.UseSubFont = false;
            this.m_btnLoginIrisRecognition.Click += new System.EventHandler(this.Click_IrisRecognition);
            // 
            // m_labelIrisCodeLeft
            // 
            this.m_labelIrisCodeLeft.BackGroundColor = System.Drawing.Color.WhiteSmoke;
            this.m_labelIrisCodeLeft.BorderStroke = 2;
            this.m_labelIrisCodeLeft.BorderStyle = System.Windows.Forms.ButtonBorderStyle.Solid;
            this.m_labelIrisCodeLeft.Description = "";
            this.m_labelIrisCodeLeft.DisabledColor = System.Drawing.Color.LightGray;
            this.m_labelIrisCodeLeft.EdgeRadius = 1;
            this.m_labelIrisCodeLeft.Enabled = false;
            this.m_labelIrisCodeLeft.ImagePosition = new System.Drawing.Point(0, 0);
            this.m_labelIrisCodeLeft.ImageSize = new System.Drawing.Point(0, 0);
            this.m_labelIrisCodeLeft.LoadImage = null;
            this.m_labelIrisCodeLeft.Location = new System.Drawing.Point(60, 651);
            this.m_labelIrisCodeLeft.MainFont = new System.Drawing.Font("맑은 고딕", 12F, System.Drawing.FontStyle.Bold);
            this.m_labelIrisCodeLeft.MainFontColor = System.Drawing.Color.DimGray;
            this.m_labelIrisCodeLeft.Name = "m_labelIrisCodeLeft";
            this.m_labelIrisCodeLeft.Size = new System.Drawing.Size(382, 40);
            this.m_labelIrisCodeLeft.SubFont = new System.Drawing.Font("맑은 고딕", 14F, System.Drawing.FontStyle.Bold);
            this.m_labelIrisCodeLeft.SubFontColor = System.Drawing.Color.Gray;
            this.m_labelIrisCodeLeft.SubText = "▶";
            this.m_labelIrisCodeLeft.TabIndex = 1372;
            this.m_labelIrisCodeLeft.Text = "Set User Left Iris Recognition";
            this.m_labelIrisCodeLeft.TextAlignMain = Sys3Controls.EN_TEXTALIGN.MIDDLE_LEFT;
            this.m_labelIrisCodeLeft.TextAlignSub = Sys3Controls.EN_TEXTALIGN.MIDDLE_RIGHT;
            this.m_labelIrisCodeLeft.TextAlignUnit = Sys3Controls.EN_TEXTALIGN.MIDDLE_CENTER;
            this.m_labelIrisCodeLeft.ThemeIndex = 0;
            this.m_labelIrisCodeLeft.UnitAreaRate = 40;
            this.m_labelIrisCodeLeft.UnitFont = new System.Drawing.Font("맑은 고딕", 10F, System.Drawing.FontStyle.Bold);
            this.m_labelIrisCodeLeft.UnitFontColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.m_labelIrisCodeLeft.UnitPositionVertical = false;
            this.m_labelIrisCodeLeft.UnitText = "";
            this.m_labelIrisCodeLeft.UseBorder = true;
            this.m_labelIrisCodeLeft.UseEdgeRadius = false;
            this.m_labelIrisCodeLeft.UseImage = false;
            this.m_labelIrisCodeLeft.UseSubFont = true;
            this.m_labelIrisCodeLeft.UseUnitFont = false;
            // 
            // m_labelIrisCodeRight
            // 
            this.m_labelIrisCodeRight.BackGroundColor = System.Drawing.Color.WhiteSmoke;
            this.m_labelIrisCodeRight.BorderStroke = 2;
            this.m_labelIrisCodeRight.BorderStyle = System.Windows.Forms.ButtonBorderStyle.Solid;
            this.m_labelIrisCodeRight.Description = "";
            this.m_labelIrisCodeRight.DisabledColor = System.Drawing.Color.LightGray;
            this.m_labelIrisCodeRight.EdgeRadius = 1;
            this.m_labelIrisCodeRight.Enabled = false;
            this.m_labelIrisCodeRight.ImagePosition = new System.Drawing.Point(0, 0);
            this.m_labelIrisCodeRight.ImageSize = new System.Drawing.Point(0, 0);
            this.m_labelIrisCodeRight.LoadImage = null;
            this.m_labelIrisCodeRight.Location = new System.Drawing.Point(60, 702);
            this.m_labelIrisCodeRight.MainFont = new System.Drawing.Font("맑은 고딕", 12F, System.Drawing.FontStyle.Bold);
            this.m_labelIrisCodeRight.MainFontColor = System.Drawing.Color.DimGray;
            this.m_labelIrisCodeRight.Name = "m_labelIrisCodeRight";
            this.m_labelIrisCodeRight.Size = new System.Drawing.Size(382, 40);
            this.m_labelIrisCodeRight.SubFont = new System.Drawing.Font("맑은 고딕", 14F, System.Drawing.FontStyle.Bold);
            this.m_labelIrisCodeRight.SubFontColor = System.Drawing.Color.Gray;
            this.m_labelIrisCodeRight.SubText = "▶";
            this.m_labelIrisCodeRight.TabIndex = 1373;
            this.m_labelIrisCodeRight.Text = "Set User Right Iris Recognition";
            this.m_labelIrisCodeRight.TextAlignMain = Sys3Controls.EN_TEXTALIGN.MIDDLE_LEFT;
            this.m_labelIrisCodeRight.TextAlignSub = Sys3Controls.EN_TEXTALIGN.MIDDLE_RIGHT;
            this.m_labelIrisCodeRight.TextAlignUnit = Sys3Controls.EN_TEXTALIGN.MIDDLE_CENTER;
            this.m_labelIrisCodeRight.ThemeIndex = 0;
            this.m_labelIrisCodeRight.UnitAreaRate = 40;
            this.m_labelIrisCodeRight.UnitFont = new System.Drawing.Font("맑은 고딕", 10F, System.Drawing.FontStyle.Bold);
            this.m_labelIrisCodeRight.UnitFontColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.m_labelIrisCodeRight.UnitPositionVertical = false;
            this.m_labelIrisCodeRight.UnitText = "";
            this.m_labelIrisCodeRight.UseBorder = true;
            this.m_labelIrisCodeRight.UseEdgeRadius = false;
            this.m_labelIrisCodeRight.UseImage = false;
            this.m_labelIrisCodeRight.UseSubFont = true;
            this.m_labelIrisCodeRight.UseUnitFont = false;
            // 
            // INDEX1
            // 
            this.INDEX1.Frozen = true;
            this.INDEX1.HeaderText = "";
            this.INDEX1.MaxInputLength = 20;
            this.INDEX1.Name = "INDEX1";
            this.INDEX1.ReadOnly = true;
            this.INDEX1.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.INDEX1.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.INDEX1.Width = 35;
            // 
            // INDEX
            // 
            this.INDEX.Frozen = true;
            this.INDEX.HeaderText = "ID";
            this.INDEX.MaxInputLength = 20;
            this.INDEX.Name = "INDEX";
            this.INDEX.ReadOnly = true;
            this.INDEX.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.INDEX.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.INDEX.Width = 160;
            // 
            // dataGridViewTextBoxColumn1
            // 
            this.dataGridViewTextBoxColumn1.Frozen = true;
            this.dataGridViewTextBoxColumn1.HeaderText = "PASSWORD";
            this.dataGridViewTextBoxColumn1.MaxInputLength = 20;
            this.dataGridViewTextBoxColumn1.Name = "dataGridViewTextBoxColumn1";
            this.dataGridViewTextBoxColumn1.ReadOnly = true;
            this.dataGridViewTextBoxColumn1.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.dataGridViewTextBoxColumn1.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.dataGridViewTextBoxColumn1.Width = 140;
            // 
            // GRADE
            // 
            this.GRADE.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            this.GRADE.Frozen = true;
            this.GRADE.HeaderText = "AUTHORITY";
            this.GRADE.MaxInputLength = 20;
            this.GRADE.Name = "GRADE";
            this.GRADE.ReadOnly = true;
            this.GRADE.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.GRADE.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.GRADE.Width = 191;
            // 
            // LOGOUT_TIME
            // 
            this.LOGOUT_TIME.Frozen = true;
            this.LOGOUT_TIME.HeaderText = "LOGOUT TIME";
            this.LOGOUT_TIME.Name = "LOGOUT_TIME";
            this.LOGOUT_TIME.ReadOnly = true;
            this.LOGOUT_TIME.Width = 120;
            // 
            // IRIS_CODE
            // 
            this.IRIS_CODE.HeaderText = "IRIS CODE";
            this.IRIS_CODE.Name = "IRIS_CODE";
            this.IRIS_CODE.ReadOnly = true;
            this.IRIS_CODE.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.IRIS_CODE.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // m_labelLogoutTime
            // 
            this.m_labelLogoutTime.BackGroundColor = System.Drawing.Color.WhiteSmoke;
            this.m_labelLogoutTime.BorderStroke = 2;
            this.m_labelLogoutTime.BorderStyle = System.Windows.Forms.ButtonBorderStyle.Solid;
            this.m_labelLogoutTime.Description = "";
            this.m_labelLogoutTime.DisabledColor = System.Drawing.Color.LightGray;
            this.m_labelLogoutTime.EdgeRadius = 1;
            this.m_labelLogoutTime.Enabled = false;
            this.m_labelLogoutTime.ImagePosition = new System.Drawing.Point(0, 0);
            this.m_labelLogoutTime.ImageSize = new System.Drawing.Point(0, 0);
            this.m_labelLogoutTime.LoadImage = null;
            this.m_labelLogoutTime.Location = new System.Drawing.Point(60, 599);
            this.m_labelLogoutTime.MainFont = new System.Drawing.Font("맑은 고딕", 12F, System.Drawing.FontStyle.Bold);
            this.m_labelLogoutTime.MainFontColor = System.Drawing.Color.DimGray;
            this.m_labelLogoutTime.Name = "m_labelLogoutTime";
            this.m_labelLogoutTime.Size = new System.Drawing.Size(382, 40);
            this.m_labelLogoutTime.SubFont = new System.Drawing.Font("맑은 고딕", 14F, System.Drawing.FontStyle.Bold);
            this.m_labelLogoutTime.SubFontColor = System.Drawing.Color.Gray;
            this.m_labelLogoutTime.SubText = "▶";
            this.m_labelLogoutTime.TabIndex = 2;
            this.m_labelLogoutTime.Text = "Enter Logout Time";
            this.m_labelLogoutTime.TextAlignMain = Sys3Controls.EN_TEXTALIGN.MIDDLE_LEFT;
            this.m_labelLogoutTime.TextAlignSub = Sys3Controls.EN_TEXTALIGN.MIDDLE_RIGHT;
            this.m_labelLogoutTime.TextAlignUnit = Sys3Controls.EN_TEXTALIGN.MIDDLE_CENTER;
            this.m_labelLogoutTime.ThemeIndex = 0;
            this.m_labelLogoutTime.UnitAreaRate = 40;
            this.m_labelLogoutTime.UnitFont = new System.Drawing.Font("맑은 고딕", 10F, System.Drawing.FontStyle.Bold);
            this.m_labelLogoutTime.UnitFontColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.m_labelLogoutTime.UnitPositionVertical = false;
            this.m_labelLogoutTime.UnitText = "";
            this.m_labelLogoutTime.UseBorder = true;
            this.m_labelLogoutTime.UseEdgeRadius = false;
            this.m_labelLogoutTime.UseImage = false;
            this.m_labelLogoutTime.UseSubFont = true;
            this.m_labelLogoutTime.UseUnitFont = false;
            this.m_labelLogoutTime.Click += new System.EventHandler(this.Click_ManagementLabel);
            // 
            // Form_Login
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.ClientSize = new System.Drawing.Size(1280, 958);
            this.ControlBox = false;
            this.Controls.Add(this.m_dgViewUserList);
            this.Controls.Add(this.m_btnLoginIrisRecognition);
            this.Controls.Add(this.m_btnProtec);
            this.Controls.Add(this.m_btnCancel);
            this.Controls.Add(this.m_btnUseManagement);
            this.Controls.Add(this.m_btnLogout);
            this.Controls.Add(this.m_btnLogin);
            this.Controls.Add(this.m_labelPW);
            this.Controls.Add(this.m_labelID);
            this.Controls.Add(this.m_btnPopupList);
            this.Controls.Add(this.m_btnCloseManagement);
            this.Controls.Add(this.m_btnRemove);
            this.Controls.Add(this.m_btnModify);
            this.Controls.Add(this.m_btnAdd);
            this.Controls.Add(this.m_btnAuthority);
            this.Controls.Add(this.m_labelAuthority);
            this.Controls.Add(this.m_labelLogoutTime);
            this.Controls.Add(this.m_labelLoginPW);
            this.Controls.Add(this.m_labelLoginID);
            this.Controls.Add(this.m_labelIrisCodeLeft);
            this.Controls.Add(this.m_labelIrisCodeRight);
            this.Controls.Add(this.sys3GroupBox2);
            this.Controls.Add(this.m_groupLogin);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Location = new System.Drawing.Point(0, 66);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "Form_Login";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.Manual;
            this.Text = "Form_Login";
            this.TopMost = true;
            ((System.ComponentModel.ISupportInitialize)(this.m_dgViewUserList)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private Sys3Controls.Sys3DoubleBufferedDataGridView m_dgViewUserList;
		private Sys3Controls.Sys3button m_btnPopupList;
		private Sys3Controls.Sys3Label m_labelID;
		private Sys3Controls.Sys3Label m_labelPW;
		private Sys3Controls.Sys3Label m_labelLoginID;
		private Sys3Controls.Sys3Label m_labelLoginPW;
		private Sys3Controls.Sys3Label m_labelAuthority;
		private Sys3Controls.Sys3button m_btnAuthority;
		private Sys3Controls.Sys3button m_btnLogin;
		private Sys3Controls.Sys3button m_btnCancel;
		private Sys3Controls.Sys3button m_btnLogout;
		private Sys3Controls.Sys3button m_btnAdd;
		private Sys3Controls.Sys3button m_btnModify;
		private Sys3Controls.Sys3button m_btnRemove;
		private Sys3Controls.Sys3GroupBox m_groupLogin;
		private Sys3Controls.Sys3button m_btnProtec;
		private Sys3Controls.Sys3GroupBox sys3GroupBox2;
		private Sys3Controls.Sys3button m_btnCloseManagement;
		private Sys3Controls.Sys3button m_btnUseManagement;
        private Sys3Controls.Sys3button m_btnLoginIrisRecognition;
        private Sys3Controls.Sys3Label m_labelIrisCodeLeft;
        private Sys3Controls.Sys3Label m_labelIrisCodeRight;
        private System.Windows.Forms.DataGridViewTextBoxColumn INDEX1;
        private System.Windows.Forms.DataGridViewTextBoxColumn INDEX;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn1;
        private System.Windows.Forms.DataGridViewTextBoxColumn GRADE;
        private System.Windows.Forms.DataGridViewTextBoxColumn LOGOUT_TIME;
        private System.Windows.Forms.DataGridViewTextBoxColumn IRIS_CODE;
        private Sys3Controls.Sys3Label m_labelLogoutTime;

    }
}