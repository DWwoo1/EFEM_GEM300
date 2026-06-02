namespace FrameOfSystem3.Views.Recipe
{
    partial class Recipe_Main
    {
        /// <summary> 
        /// 필수 디자이너 변수입니다.
        /// </summary>
        private System.ComponentModel.IContainer components=null;

        /// <summary> 
        /// 사용 중인 모든 리소스를 정리합니다.
        /// </summary>
        /// <param name="disposing">관리되는 리소스를 삭제해야 하면 true이고, 그렇지 않으면 false입니다.</param>
        protected override void Dispose(bool disposing)
        {
            if(disposing&&(components!=null))
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
			System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle25 = new System.Windows.Forms.DataGridViewCellStyle();
			System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle26 = new System.Windows.Forms.DataGridViewCellStyle();
			System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle27 = new System.Windows.Forms.DataGridViewCellStyle();
			System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle28 = new System.Windows.Forms.DataGridViewCellStyle();
			System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle29 = new System.Windows.Forms.DataGridViewCellStyle();
			System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle30 = new System.Windows.Forms.DataGridViewCellStyle();
			this.m_dgviewGroup = new System.Windows.Forms.DataGridView();
			this.dataGridViewTextBoxColumn1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.PATH = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.m_dgviewFile = new System.Windows.Forms.DataGridView();
			this.dataGridViewTextBoxColumn3 = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.dataGridViewTextBoxColumn5 = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.WriteTime = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.m_labelFullPath = new Sys3Controls.Sys3Label();
			this.m_lblBasePath = new Sys3Controls.Sys3Label();
			this.m_groupRecipeFileManagement = new Sys3Controls.Sys3GroupBoxContainer();
			this._tableLayoutPanel_Main = new System.Windows.Forms.TableLayoutPanel();
			this._tableLayoutPanel_Bottom = new System.Windows.Forms.TableLayoutPanel();
			this.m_btnLoad = new Sys3Controls.Sys3button();
			this.m_lblSelectedRecipe = new Sys3Controls.Sys3Label();
			this.m_labelSelectedRecipe = new Sys3Controls.Sys3Label();
			this._tableLayoutPanel_Top = new System.Windows.Forms.TableLayoutPanel();
			this._tableLayoutPanel_Center = new System.Windows.Forms.TableLayoutPanel();
			this.m_groupGroup = new Sys3Controls.Sys3GroupBoxContainer();
			this.m_groupRecipeFile = new Sys3Controls.Sys3GroupBoxContainer();
			this.m_groupGroupConfig = new Sys3Controls.Sys3GroupBoxContainer();
			this._tableLayoutPanel_GroupFolderConfig = new System.Windows.Forms.TableLayoutPanel();
			this.m_labelGFolder = new Sys3Controls.Sys3Label();
			this.m_btnGCreate = new Sys3Controls.Sys3button();
			this.m_btnGModify = new Sys3Controls.Sys3button();
			this.m_btnEnter = new Sys3Controls.Sys3button();
			this.m_btnGCopy = new Sys3Controls.Sys3button();
			this.m_btnPrevious = new Sys3Controls.Sys3button();
			this.m_lblGFolder = new Sys3Controls.Sys3Label();
			this.m_btnGRemove = new Sys3Controls.Sys3button();
			this.m_groupRecipeConfig = new Sys3Controls.Sys3GroupBoxContainer();
			this._tableLayoutPanel_RecipeFileConfig = new System.Windows.Forms.TableLayoutPanel();
			this.m_labelRFile = new Sys3Controls.Sys3Label();
			this.m_btnRRemove = new Sys3Controls.Sys3button();
			this.m_btnRModify = new Sys3Controls.Sys3button();
			this.m_btnRCopy = new Sys3Controls.Sys3button();
			this.m_btnRCreate = new Sys3Controls.Sys3button();
			this.m_lblRFolder = new Sys3Controls.Sys3Label();
			this.sys3GroupBoxContainer1 = new Sys3Controls.Sys3GroupBoxContainer();
			this.lbl_Filtering = new Sys3Controls.Sys3Label();
			((System.ComponentModel.ISupportInitialize)(this.m_dgviewGroup)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.m_dgviewFile)).BeginInit();
			this.m_groupRecipeFileManagement.SuspendLayout();
			this._tableLayoutPanel_Main.SuspendLayout();
			this._tableLayoutPanel_Bottom.SuspendLayout();
			this._tableLayoutPanel_Top.SuspendLayout();
			this._tableLayoutPanel_Center.SuspendLayout();
			this.m_groupGroup.SuspendLayout();
			this.m_groupRecipeFile.SuspendLayout();
			this.m_groupGroupConfig.SuspendLayout();
			this._tableLayoutPanel_GroupFolderConfig.SuspendLayout();
			this.m_groupRecipeConfig.SuspendLayout();
			this._tableLayoutPanel_RecipeFileConfig.SuspendLayout();
			this.sys3GroupBoxContainer1.SuspendLayout();
			this.SuspendLayout();
			// 
			// m_dgviewGroup
			// 
			this.m_dgviewGroup.AllowUserToAddRows = false;
			this.m_dgviewGroup.AllowUserToDeleteRows = false;
			this.m_dgviewGroup.AllowUserToResizeColumns = false;
			this.m_dgviewGroup.AllowUserToResizeRows = false;
			this.m_dgviewGroup.BackgroundColor = System.Drawing.Color.White;
			this.m_dgviewGroup.CellBorderStyle = System.Windows.Forms.DataGridViewCellBorderStyle.Raised;
			this.m_dgviewGroup.ColumnHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.Single;
			dataGridViewCellStyle25.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
			dataGridViewCellStyle25.BackColor = System.Drawing.Color.LightGray;
			dataGridViewCellStyle25.Font = new System.Drawing.Font("맑은 고딕", 11F, System.Drawing.FontStyle.Bold);
			dataGridViewCellStyle25.ForeColor = System.Drawing.Color.Black;
			dataGridViewCellStyle25.SelectionBackColor = System.Drawing.Color.White;
			dataGridViewCellStyle25.SelectionForeColor = System.Drawing.Color.Black;
			dataGridViewCellStyle25.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
			this.m_dgviewGroup.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle25;
			this.m_dgviewGroup.ColumnHeadersHeight = 30;
			this.m_dgviewGroup.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
			this.m_dgviewGroup.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.dataGridViewTextBoxColumn1,
            this.PATH});
			dataGridViewCellStyle26.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
			dataGridViewCellStyle26.BackColor = System.Drawing.SystemColors.Window;
			dataGridViewCellStyle26.Font = new System.Drawing.Font("맑은 고딕", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
			dataGridViewCellStyle26.ForeColor = System.Drawing.SystemColors.ControlText;
			dataGridViewCellStyle26.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(120)))), ((int)(((byte)(130)))), ((int)(((byte)(150)))));
			dataGridViewCellStyle26.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
			dataGridViewCellStyle26.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
			this.m_dgviewGroup.DefaultCellStyle = dataGridViewCellStyle26;
			this.m_dgviewGroup.Dock = System.Windows.Forms.DockStyle.Fill;
			this.m_dgviewGroup.EnableHeadersVisualStyles = false;
			this.m_dgviewGroup.GridColor = System.Drawing.Color.FromArgb(((int)(((byte)(120)))), ((int)(((byte)(130)))), ((int)(((byte)(150)))));
			this.m_dgviewGroup.Location = new System.Drawing.Point(5, 34);
			this.m_dgviewGroup.MultiSelect = false;
			this.m_dgviewGroup.Name = "m_dgviewGroup";
			this.m_dgviewGroup.ReadOnly = true;
			this.m_dgviewGroup.RowHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.None;
			dataGridViewCellStyle27.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
			dataGridViewCellStyle27.BackColor = System.Drawing.Color.White;
			dataGridViewCellStyle27.Font = new System.Drawing.Font("맑은 고딕", 11F);
			dataGridViewCellStyle27.ForeColor = System.Drawing.SystemColors.WindowText;
			dataGridViewCellStyle27.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(120)))), ((int)(((byte)(130)))), ((int)(((byte)(150)))));
			dataGridViewCellStyle27.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
			dataGridViewCellStyle27.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
			this.m_dgviewGroup.RowHeadersDefaultCellStyle = dataGridViewCellStyle27;
			this.m_dgviewGroup.RowHeadersVisible = false;
			this.m_dgviewGroup.RowHeadersWidthSizeMode = System.Windows.Forms.DataGridViewRowHeadersWidthSizeMode.DisableResizing;
			this.m_dgviewGroup.RowTemplate.Height = 40;
			this.m_dgviewGroup.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
			this.m_dgviewGroup.Size = new System.Drawing.Size(438, 499);
			this.m_dgviewGroup.TabIndex = 1285;
			this.m_dgviewGroup.CellClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.Click_GroupView);
			this.m_dgviewGroup.CellDoubleClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.DoubleClick_GroupView);
			// 
			// dataGridViewTextBoxColumn1
			// 
			this.dataGridViewTextBoxColumn1.HeaderText = "INDEX";
			this.dataGridViewTextBoxColumn1.MaxInputLength = 20;
			this.dataGridViewTextBoxColumn1.Name = "dataGridViewTextBoxColumn1";
			this.dataGridViewTextBoxColumn1.ReadOnly = true;
			this.dataGridViewTextBoxColumn1.Resizable = System.Windows.Forms.DataGridViewTriState.False;
			this.dataGridViewTextBoxColumn1.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
			this.dataGridViewTextBoxColumn1.Width = 80;
			// 
			// PATH
			// 
			this.PATH.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
			this.PATH.HeaderText = "PATH";
			this.PATH.Name = "PATH";
			this.PATH.ReadOnly = true;
			this.PATH.Resizable = System.Windows.Forms.DataGridViewTriState.False;
			this.PATH.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
			// 
			// m_dgviewFile
			// 
			this.m_dgviewFile.AllowUserToAddRows = false;
			this.m_dgviewFile.AllowUserToDeleteRows = false;
			this.m_dgviewFile.AllowUserToResizeColumns = false;
			this.m_dgviewFile.AllowUserToResizeRows = false;
			this.m_dgviewFile.BackgroundColor = System.Drawing.Color.White;
			this.m_dgviewFile.CellBorderStyle = System.Windows.Forms.DataGridViewCellBorderStyle.Raised;
			this.m_dgviewFile.ColumnHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.Single;
			dataGridViewCellStyle28.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
			dataGridViewCellStyle28.BackColor = System.Drawing.Color.LightGray;
			dataGridViewCellStyle28.Font = new System.Drawing.Font("맑은 고딕", 11F, System.Drawing.FontStyle.Bold);
			dataGridViewCellStyle28.ForeColor = System.Drawing.Color.Black;
			dataGridViewCellStyle28.SelectionBackColor = System.Drawing.Color.White;
			dataGridViewCellStyle28.SelectionForeColor = System.Drawing.Color.Black;
			dataGridViewCellStyle28.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
			this.m_dgviewFile.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle28;
			this.m_dgviewFile.ColumnHeadersHeight = 30;
			this.m_dgviewFile.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
			this.m_dgviewFile.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.dataGridViewTextBoxColumn3,
            this.dataGridViewTextBoxColumn5,
            this.WriteTime});
			dataGridViewCellStyle29.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
			dataGridViewCellStyle29.BackColor = System.Drawing.SystemColors.Window;
			dataGridViewCellStyle29.Font = new System.Drawing.Font("맑은 고딕", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
			dataGridViewCellStyle29.ForeColor = System.Drawing.SystemColors.ControlText;
			dataGridViewCellStyle29.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(120)))), ((int)(((byte)(130)))), ((int)(((byte)(150)))));
			dataGridViewCellStyle29.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
			dataGridViewCellStyle29.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
			this.m_dgviewFile.DefaultCellStyle = dataGridViewCellStyle29;
			this.m_dgviewFile.Dock = System.Windows.Forms.DockStyle.Fill;
			this.m_dgviewFile.EnableHeadersVisualStyles = false;
			this.m_dgviewFile.GridColor = System.Drawing.Color.FromArgb(((int)(((byte)(120)))), ((int)(((byte)(130)))), ((int)(((byte)(150)))));
			this.m_dgviewFile.Location = new System.Drawing.Point(5, 34);
			this.m_dgviewFile.MultiSelect = false;
			this.m_dgviewFile.Name = "m_dgviewFile";
			this.m_dgviewFile.ReadOnly = true;
			this.m_dgviewFile.RowHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.None;
			dataGridViewCellStyle30.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
			dataGridViewCellStyle30.BackColor = System.Drawing.Color.White;
			dataGridViewCellStyle30.Font = new System.Drawing.Font("맑은 고딕", 11F);
			dataGridViewCellStyle30.ForeColor = System.Drawing.SystemColors.WindowText;
			dataGridViewCellStyle30.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(120)))), ((int)(((byte)(130)))), ((int)(((byte)(150)))));
			dataGridViewCellStyle30.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
			dataGridViewCellStyle30.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
			this.m_dgviewFile.RowHeadersDefaultCellStyle = dataGridViewCellStyle30;
			this.m_dgviewFile.RowHeadersVisible = false;
			this.m_dgviewFile.RowHeadersWidthSizeMode = System.Windows.Forms.DataGridViewRowHeadersWidthSizeMode.DisableResizing;
			this.m_dgviewFile.RowTemplate.Height = 40;
			this.m_dgviewFile.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
			this.m_dgviewFile.Size = new System.Drawing.Size(671, 417);
			this.m_dgviewFile.TabIndex = 1287;
			this.m_dgviewFile.CellClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.Click_FileView);
			// 
			// dataGridViewTextBoxColumn3
			// 
			this.dataGridViewTextBoxColumn3.HeaderText = "INDEX";
			this.dataGridViewTextBoxColumn3.MaxInputLength = 20;
			this.dataGridViewTextBoxColumn3.Name = "dataGridViewTextBoxColumn3";
			this.dataGridViewTextBoxColumn3.ReadOnly = true;
			this.dataGridViewTextBoxColumn3.Resizable = System.Windows.Forms.DataGridViewTriState.False;
			this.dataGridViewTextBoxColumn3.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
			this.dataGridViewTextBoxColumn3.Width = 80;
			// 
			// dataGridViewTextBoxColumn5
			// 
			this.dataGridViewTextBoxColumn5.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
			this.dataGridViewTextBoxColumn5.HeaderText = "FILE NAME";
			this.dataGridViewTextBoxColumn5.Name = "dataGridViewTextBoxColumn5";
			this.dataGridViewTextBoxColumn5.ReadOnly = true;
			this.dataGridViewTextBoxColumn5.Resizable = System.Windows.Forms.DataGridViewTriState.False;
			this.dataGridViewTextBoxColumn5.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
			// 
			// WriteTime
			// 
			this.WriteTime.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.ColumnHeader;
			this.WriteTime.HeaderText = "LAST MODIFIED DATE";
			this.WriteTime.Name = "WriteTime";
			this.WriteTime.ReadOnly = true;
			this.WriteTime.Resizable = System.Windows.Forms.DataGridViewTriState.False;
			this.WriteTime.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
			this.WriteTime.Width = 173;
			// 
			// m_labelFullPath
			// 
			this.m_labelFullPath.BackGroundColor = System.Drawing.Color.WhiteSmoke;
			this.m_labelFullPath.BorderStroke = 2;
			this.m_labelFullPath.BorderStyle = System.Windows.Forms.ButtonBorderStyle.Solid;
			this.m_labelFullPath.Description = "";
			this.m_labelFullPath.DisabledColor = System.Drawing.Color.LightGray;
			this.m_labelFullPath.Dock = System.Windows.Forms.DockStyle.Fill;
			this.m_labelFullPath.EdgeRadius = 1;
			this.m_labelFullPath.ImagePosition = new System.Drawing.Point(0, 0);
			this.m_labelFullPath.ImageSize = new System.Drawing.Point(0, 0);
			this.m_labelFullPath.LoadImage = null;
			this.m_labelFullPath.Location = new System.Drawing.Point(203, 3);
			this.m_labelFullPath.MainFont = new System.Drawing.Font("맑은 고딕", 14F, System.Drawing.FontStyle.Bold);
			this.m_labelFullPath.MainFontColor = System.Drawing.Color.Black;
			this.m_labelFullPath.Name = "m_labelFullPath";
			this.m_labelFullPath.Size = new System.Drawing.Size(924, 46);
			this.m_labelFullPath.SubFont = new System.Drawing.Font("맑은 고딕", 14F, System.Drawing.FontStyle.Bold);
			this.m_labelFullPath.SubFontColor = System.Drawing.Color.Gray;
			this.m_labelFullPath.SubText = "▶";
			this.m_labelFullPath.TabIndex = 1355;
			this.m_labelFullPath.Text = "--";
			this.m_labelFullPath.TextAlignMain = Sys3Controls.EN_TEXTALIGN.MIDDLE_CENTER;
			this.m_labelFullPath.TextAlignSub = Sys3Controls.EN_TEXTALIGN.MIDDLE_RIGHT;
			this.m_labelFullPath.TextAlignUnit = Sys3Controls.EN_TEXTALIGN.MIDDLE_CENTER;
			this.m_labelFullPath.ThemeIndex = 0;
			this.m_labelFullPath.UnitAreaRate = 40;
			this.m_labelFullPath.UnitFont = new System.Drawing.Font("맑은 고딕", 10F, System.Drawing.FontStyle.Bold);
			this.m_labelFullPath.UnitFontColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
			this.m_labelFullPath.UnitPositionVertical = false;
			this.m_labelFullPath.UnitText = "";
			this.m_labelFullPath.UseBorder = true;
			this.m_labelFullPath.UseEdgeRadius = false;
			this.m_labelFullPath.UseImage = false;
			this.m_labelFullPath.UseSubFont = true;
			this.m_labelFullPath.UseUnitFont = false;
			this.m_labelFullPath.Click += new System.EventHandler(this.Click_FullFilePathLabel);
			// 
			// m_lblBasePath
			// 
			this.m_lblBasePath.BackGroundColor = System.Drawing.Color.FromArgb(((int)(((byte)(200)))), ((int)(((byte)(200)))), ((int)(((byte)(220)))));
			this.m_lblBasePath.BorderStroke = 2;
			this.m_lblBasePath.BorderStyle = System.Windows.Forms.ButtonBorderStyle.Solid;
			this.m_lblBasePath.Description = "";
			this.m_lblBasePath.DisabledColor = System.Drawing.Color.DarkGray;
			this.m_lblBasePath.Dock = System.Windows.Forms.DockStyle.Fill;
			this.m_lblBasePath.EdgeRadius = 1;
			this.m_lblBasePath.ImagePosition = new System.Drawing.Point(0, 0);
			this.m_lblBasePath.ImageSize = new System.Drawing.Point(0, 0);
			this.m_lblBasePath.LoadImage = null;
			this.m_lblBasePath.Location = new System.Drawing.Point(3, 3);
			this.m_lblBasePath.MainFont = new System.Drawing.Font("맑은 고딕", 12F, System.Drawing.FontStyle.Bold);
			this.m_lblBasePath.MainFontColor = System.Drawing.Color.Black;
			this.m_lblBasePath.Name = "m_lblBasePath";
			this.m_lblBasePath.Size = new System.Drawing.Size(194, 46);
			this.m_lblBasePath.SubFont = new System.Drawing.Font("맑은 고딕", 14F, System.Drawing.FontStyle.Bold);
			this.m_lblBasePath.SubFontColor = System.Drawing.Color.Gray;
			this.m_lblBasePath.SubText = "▶";
			this.m_lblBasePath.TabIndex = 1356;
			this.m_lblBasePath.Text = "FULL FILE PATH";
			this.m_lblBasePath.TextAlignMain = Sys3Controls.EN_TEXTALIGN.MIDDLE_LEFT;
			this.m_lblBasePath.TextAlignSub = Sys3Controls.EN_TEXTALIGN.MIDDLE_RIGHT;
			this.m_lblBasePath.TextAlignUnit = Sys3Controls.EN_TEXTALIGN.MIDDLE_CENTER;
			this.m_lblBasePath.ThemeIndex = 0;
			this.m_lblBasePath.UnitAreaRate = 40;
			this.m_lblBasePath.UnitFont = new System.Drawing.Font("맑은 고딕", 10F, System.Drawing.FontStyle.Bold);
			this.m_lblBasePath.UnitFontColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
			this.m_lblBasePath.UnitPositionVertical = false;
			this.m_lblBasePath.UnitText = "";
			this.m_lblBasePath.UseBorder = true;
			this.m_lblBasePath.UseEdgeRadius = false;
			this.m_lblBasePath.UseImage = false;
			this.m_lblBasePath.UseSubFont = false;
			this.m_lblBasePath.UseUnitFont = false;
			// 
			// m_groupRecipeFileManagement
			// 
			this.m_groupRecipeFileManagement.BackGroundColor = System.Drawing.Color.WhiteSmoke;
			this.m_groupRecipeFileManagement.Controls.Add(this._tableLayoutPanel_Main);
			this.m_groupRecipeFileManagement.Dock = System.Windows.Forms.DockStyle.Fill;
			this.m_groupRecipeFileManagement.EdgeBorderStroke = 2;
			this.m_groupRecipeFileManagement.EdgeRadius = 2;
			this.m_groupRecipeFileManagement.LabelFont = new System.Drawing.Font("맑은 고딕", 10F, System.Drawing.FontStyle.Bold);
			this.m_groupRecipeFileManagement.LabelGradientColorFirst = System.Drawing.Color.FromArgb(((int)(((byte)(208)))), ((int)(((byte)(208)))), ((int)(((byte)(208)))));
			this.m_groupRecipeFileManagement.LabelGradientColorSecond = System.Drawing.Color.FromArgb(((int)(((byte)(240)))), ((int)(((byte)(240)))), ((int)(((byte)(240)))));
			this.m_groupRecipeFileManagement.LabelHeight = 30;
			this.m_groupRecipeFileManagement.LabelTextColor = System.Drawing.Color.Black;
			this.m_groupRecipeFileManagement.Location = new System.Drawing.Point(0, 0);
			this.m_groupRecipeFileManagement.Name = "m_groupRecipeFileManagement";
			this.m_groupRecipeFileManagement.Padding = new System.Windows.Forms.Padding(5, 20, 5, 5);
			this.m_groupRecipeFileManagement.Size = new System.Drawing.Size(1140, 900);
			this.m_groupRecipeFileManagement.TabIndex = 1357;
			this.m_groupRecipeFileManagement.TabStop = false;
			this.m_groupRecipeFileManagement.Text = "RECIPE FILE MANAGEMENT";
			this.m_groupRecipeFileManagement.TextAlign = Sys3Controls.EN_TEXTALIGN.MIDDLE_CENTER;
			this.m_groupRecipeFileManagement.ThemeIndex = 0;
			this.m_groupRecipeFileManagement.UseLabelBorder = true;
			this.m_groupRecipeFileManagement.UseTitle = true;
			// 
			// _tableLayoutPanel_Main
			// 
			this._tableLayoutPanel_Main.BackColor = System.Drawing.Color.WhiteSmoke;
			this._tableLayoutPanel_Main.ColumnCount = 1;
			this._tableLayoutPanel_Main.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
			this._tableLayoutPanel_Main.Controls.Add(this._tableLayoutPanel_Bottom, 0, 2);
			this._tableLayoutPanel_Main.Controls.Add(this._tableLayoutPanel_Top, 0, 0);
			this._tableLayoutPanel_Main.Controls.Add(this._tableLayoutPanel_Center, 0, 1);
			this._tableLayoutPanel_Main.Dock = System.Windows.Forms.DockStyle.Fill;
			this._tableLayoutPanel_Main.Location = new System.Drawing.Point(5, 34);
			this._tableLayoutPanel_Main.Name = "_tableLayoutPanel_Main";
			this._tableLayoutPanel_Main.RowCount = 3;
			this._tableLayoutPanel_Main.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 52F));
			this._tableLayoutPanel_Main.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
			this._tableLayoutPanel_Main.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 76F));
			this._tableLayoutPanel_Main.Size = new System.Drawing.Size(1130, 861);
			this._tableLayoutPanel_Main.TabIndex = 1368;
			// 
			// _tableLayoutPanel_Bottom
			// 
			this._tableLayoutPanel_Bottom.ColumnCount = 3;
			this._tableLayoutPanel_Bottom.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 220F));
			this._tableLayoutPanel_Bottom.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
			this._tableLayoutPanel_Bottom.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 256F));
			this._tableLayoutPanel_Bottom.Controls.Add(this.m_btnLoad, 2, 0);
			this._tableLayoutPanel_Bottom.Controls.Add(this.m_lblSelectedRecipe, 0, 0);
			this._tableLayoutPanel_Bottom.Controls.Add(this.m_labelSelectedRecipe, 1, 0);
			this._tableLayoutPanel_Bottom.Dock = System.Windows.Forms.DockStyle.Fill;
			this._tableLayoutPanel_Bottom.Location = new System.Drawing.Point(0, 785);
			this._tableLayoutPanel_Bottom.Margin = new System.Windows.Forms.Padding(0);
			this._tableLayoutPanel_Bottom.Name = "_tableLayoutPanel_Bottom";
			this._tableLayoutPanel_Bottom.RowCount = 1;
			this._tableLayoutPanel_Bottom.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
			this._tableLayoutPanel_Bottom.Size = new System.Drawing.Size(1130, 76);
			this._tableLayoutPanel_Bottom.TabIndex = 0;
			// 
			// m_btnLoad
			// 
			this.m_btnLoad.BorderWidth = 3;
			this.m_btnLoad.ButtonClicked = false;
			this.m_btnLoad.ClickedEmphasizeTextColor = System.Drawing.Color.White;
			this.m_btnLoad.CustomClickedGradientFirstColor = System.Drawing.Color.White;
			this.m_btnLoad.CustomClickedGradientSecondColor = System.Drawing.Color.White;
			this.m_btnLoad.Description = "";
			this.m_btnLoad.DisabledColor = System.Drawing.Color.DarkGray;
			this.m_btnLoad.Dock = System.Windows.Forms.DockStyle.Fill;
			this.m_btnLoad.EdgeRadius = 5;
			this.m_btnLoad.GradientAngle = 78F;
			this.m_btnLoad.GradientFirstColor = System.Drawing.Color.White;
			this.m_btnLoad.GradientSecondColor = System.Drawing.Color.FromArgb(((int)(((byte)(170)))), ((int)(((byte)(176)))), ((int)(((byte)(183)))));
			this.m_btnLoad.HoverEmphasizeCustomColor = System.Drawing.Color.White;
			this.m_btnLoad.ImagePosition = new System.Drawing.Point(15, 15);
			this.m_btnLoad.ImageSize = new System.Drawing.Point(30, 30);
			this.m_btnLoad.LoadImage = global::FrameOfSystem3.Properties.Resources.LOAD;
			this.m_btnLoad.Location = new System.Drawing.Point(877, 3);
			this.m_btnLoad.MainFont = new System.Drawing.Font("맑은 고딕", 15F, System.Drawing.FontStyle.Bold);
			this.m_btnLoad.MainFontColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(36)))), ((int)(((byte)(0)))));
			this.m_btnLoad.Name = "m_btnLoad";
			this.m_btnLoad.Size = new System.Drawing.Size(250, 70);
			this.m_btnLoad.SubFont = new System.Drawing.Font("맑은 고딕", 8F, System.Drawing.FontStyle.Bold);
			this.m_btnLoad.SubFontColor = System.Drawing.Color.DarkBlue;
			this.m_btnLoad.SubText = "STATUS";
			this.m_btnLoad.TabIndex = 1367;
			this.m_btnLoad.Text = "LOAD";
			this.m_btnLoad.TextAlignMain = Sys3Controls.EN_TEXTALIGN.MIDDLE_RIGHT;
			this.m_btnLoad.TextAlignSub = Sys3Controls.EN_TEXTALIGN.TOP_LEFT;
			this.m_btnLoad.ThemeIndex = 0;
			this.m_btnLoad.UseBorder = true;
			this.m_btnLoad.UseClickedEmphasizeTextColor = false;
			this.m_btnLoad.UseCustomizeClickedColor = false;
			this.m_btnLoad.UseEdge = true;
			this.m_btnLoad.UseHoverEmphasizeCustomColor = false;
			this.m_btnLoad.UseImage = true;
			this.m_btnLoad.UserHoverEmpahsize = false;
			this.m_btnLoad.UseSubFont = false;
			this.m_btnLoad.Click += new System.EventHandler(this.Click_RecipeLoad);
			// 
			// m_lblSelectedRecipe
			// 
			this.m_lblSelectedRecipe.BackGroundColor = System.Drawing.Color.FromArgb(((int)(((byte)(200)))), ((int)(((byte)(200)))), ((int)(((byte)(220)))));
			this.m_lblSelectedRecipe.BorderStroke = 2;
			this.m_lblSelectedRecipe.BorderStyle = System.Windows.Forms.ButtonBorderStyle.Solid;
			this.m_lblSelectedRecipe.Description = "";
			this.m_lblSelectedRecipe.DisabledColor = System.Drawing.Color.DarkGray;
			this.m_lblSelectedRecipe.Dock = System.Windows.Forms.DockStyle.Fill;
			this.m_lblSelectedRecipe.EdgeRadius = 1;
			this.m_lblSelectedRecipe.ImagePosition = new System.Drawing.Point(0, 0);
			this.m_lblSelectedRecipe.ImageSize = new System.Drawing.Point(0, 0);
			this.m_lblSelectedRecipe.LoadImage = null;
			this.m_lblSelectedRecipe.Location = new System.Drawing.Point(3, 3);
			this.m_lblSelectedRecipe.MainFont = new System.Drawing.Font("맑은 고딕", 12F, System.Drawing.FontStyle.Bold);
			this.m_lblSelectedRecipe.MainFontColor = System.Drawing.Color.Black;
			this.m_lblSelectedRecipe.Name = "m_lblSelectedRecipe";
			this.m_lblSelectedRecipe.Size = new System.Drawing.Size(214, 70);
			this.m_lblSelectedRecipe.SubFont = new System.Drawing.Font("맑은 고딕", 14F, System.Drawing.FontStyle.Bold);
			this.m_lblSelectedRecipe.SubFontColor = System.Drawing.Color.Gray;
			this.m_lblSelectedRecipe.SubText = "▶";
			this.m_lblSelectedRecipe.TabIndex = 1366;
			this.m_lblSelectedRecipe.Text = "SELECTED RECIPE";
			this.m_lblSelectedRecipe.TextAlignMain = Sys3Controls.EN_TEXTALIGN.MIDDLE_LEFT;
			this.m_lblSelectedRecipe.TextAlignSub = Sys3Controls.EN_TEXTALIGN.MIDDLE_RIGHT;
			this.m_lblSelectedRecipe.TextAlignUnit = Sys3Controls.EN_TEXTALIGN.MIDDLE_CENTER;
			this.m_lblSelectedRecipe.ThemeIndex = 0;
			this.m_lblSelectedRecipe.UnitAreaRate = 40;
			this.m_lblSelectedRecipe.UnitFont = new System.Drawing.Font("맑은 고딕", 10F, System.Drawing.FontStyle.Bold);
			this.m_lblSelectedRecipe.UnitFontColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
			this.m_lblSelectedRecipe.UnitPositionVertical = false;
			this.m_lblSelectedRecipe.UnitText = "";
			this.m_lblSelectedRecipe.UseBorder = true;
			this.m_lblSelectedRecipe.UseEdgeRadius = false;
			this.m_lblSelectedRecipe.UseImage = false;
			this.m_lblSelectedRecipe.UseSubFont = false;
			this.m_lblSelectedRecipe.UseUnitFont = false;
			// 
			// m_labelSelectedRecipe
			// 
			this.m_labelSelectedRecipe.BackGroundColor = System.Drawing.Color.WhiteSmoke;
			this.m_labelSelectedRecipe.BorderStroke = 2;
			this.m_labelSelectedRecipe.BorderStyle = System.Windows.Forms.ButtonBorderStyle.Solid;
			this.m_labelSelectedRecipe.Description = "";
			this.m_labelSelectedRecipe.DisabledColor = System.Drawing.Color.LightGray;
			this.m_labelSelectedRecipe.Dock = System.Windows.Forms.DockStyle.Fill;
			this.m_labelSelectedRecipe.EdgeRadius = 1;
			this.m_labelSelectedRecipe.Enabled = false;
			this.m_labelSelectedRecipe.ImagePosition = new System.Drawing.Point(0, 0);
			this.m_labelSelectedRecipe.ImageSize = new System.Drawing.Point(0, 0);
			this.m_labelSelectedRecipe.LoadImage = null;
			this.m_labelSelectedRecipe.Location = new System.Drawing.Point(223, 3);
			this.m_labelSelectedRecipe.MainFont = new System.Drawing.Font("맑은 고딕", 14F, System.Drawing.FontStyle.Bold);
			this.m_labelSelectedRecipe.MainFontColor = System.Drawing.Color.Black;
			this.m_labelSelectedRecipe.Name = "m_labelSelectedRecipe";
			this.m_labelSelectedRecipe.Size = new System.Drawing.Size(648, 70);
			this.m_labelSelectedRecipe.SubFont = new System.Drawing.Font("맑은 고딕", 14F, System.Drawing.FontStyle.Bold);
			this.m_labelSelectedRecipe.SubFontColor = System.Drawing.Color.Gray;
			this.m_labelSelectedRecipe.SubText = "▶";
			this.m_labelSelectedRecipe.TabIndex = 1365;
			this.m_labelSelectedRecipe.TextAlignMain = Sys3Controls.EN_TEXTALIGN.MIDDLE_CENTER;
			this.m_labelSelectedRecipe.TextAlignSub = Sys3Controls.EN_TEXTALIGN.MIDDLE_RIGHT;
			this.m_labelSelectedRecipe.TextAlignUnit = Sys3Controls.EN_TEXTALIGN.MIDDLE_CENTER;
			this.m_labelSelectedRecipe.ThemeIndex = 0;
			this.m_labelSelectedRecipe.UnitAreaRate = 40;
			this.m_labelSelectedRecipe.UnitFont = new System.Drawing.Font("맑은 고딕", 10F, System.Drawing.FontStyle.Bold);
			this.m_labelSelectedRecipe.UnitFontColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
			this.m_labelSelectedRecipe.UnitPositionVertical = false;
			this.m_labelSelectedRecipe.UnitText = "";
			this.m_labelSelectedRecipe.UseBorder = true;
			this.m_labelSelectedRecipe.UseEdgeRadius = false;
			this.m_labelSelectedRecipe.UseImage = false;
			this.m_labelSelectedRecipe.UseSubFont = false;
			this.m_labelSelectedRecipe.UseUnitFont = false;
			// 
			// _tableLayoutPanel_Top
			// 
			this._tableLayoutPanel_Top.ColumnCount = 2;
			this._tableLayoutPanel_Top.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 200F));
			this._tableLayoutPanel_Top.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
			this._tableLayoutPanel_Top.Controls.Add(this.m_lblBasePath, 0, 0);
			this._tableLayoutPanel_Top.Controls.Add(this.m_labelFullPath, 1, 0);
			this._tableLayoutPanel_Top.Dock = System.Windows.Forms.DockStyle.Fill;
			this._tableLayoutPanel_Top.Location = new System.Drawing.Point(0, 0);
			this._tableLayoutPanel_Top.Margin = new System.Windows.Forms.Padding(0);
			this._tableLayoutPanel_Top.Name = "_tableLayoutPanel_Top";
			this._tableLayoutPanel_Top.RowCount = 1;
			this._tableLayoutPanel_Top.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
			this._tableLayoutPanel_Top.Size = new System.Drawing.Size(1130, 52);
			this._tableLayoutPanel_Top.TabIndex = 1;
			// 
			// _tableLayoutPanel_Center
			// 
			this._tableLayoutPanel_Center.ColumnCount = 2;
			this._tableLayoutPanel_Center.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 39.64602F));
			this._tableLayoutPanel_Center.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 60.35398F));
			this._tableLayoutPanel_Center.Controls.Add(this.m_groupGroup, 0, 0);
			this._tableLayoutPanel_Center.Controls.Add(this.m_groupRecipeFile, 1, 0);
			this._tableLayoutPanel_Center.Controls.Add(this.m_groupRecipeConfig, 1, 2);
			this._tableLayoutPanel_Center.Controls.Add(this.m_groupGroupConfig, 0, 2);
			this._tableLayoutPanel_Center.Controls.Add(this.sys3GroupBoxContainer1, 1, 1);
			this._tableLayoutPanel_Center.Dock = System.Windows.Forms.DockStyle.Fill;
			this._tableLayoutPanel_Center.Location = new System.Drawing.Point(0, 52);
			this._tableLayoutPanel_Center.Margin = new System.Windows.Forms.Padding(0);
			this._tableLayoutPanel_Center.Name = "_tableLayoutPanel_Center";
			this._tableLayoutPanel_Center.RowCount = 3;
			this._tableLayoutPanel_Center.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
			this._tableLayoutPanel_Center.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 82F));
			this._tableLayoutPanel_Center.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 195F));
			this._tableLayoutPanel_Center.Size = new System.Drawing.Size(1130, 733);
			this._tableLayoutPanel_Center.TabIndex = 2;
			// 
			// m_groupGroup
			// 
			this.m_groupGroup.BackGroundColor = System.Drawing.Color.WhiteSmoke;
			this.m_groupGroup.Controls.Add(this.m_dgviewGroup);
			this.m_groupGroup.Dock = System.Windows.Forms.DockStyle.Fill;
			this.m_groupGroup.EdgeBorderStroke = 2;
			this.m_groupGroup.EdgeRadius = 2;
			this.m_groupGroup.LabelFont = new System.Drawing.Font("맑은 고딕", 10F, System.Drawing.FontStyle.Bold);
			this.m_groupGroup.LabelGradientColorFirst = System.Drawing.Color.FromArgb(((int)(((byte)(208)))), ((int)(((byte)(208)))), ((int)(((byte)(208)))));
			this.m_groupGroup.LabelGradientColorSecond = System.Drawing.Color.FromArgb(((int)(((byte)(240)))), ((int)(((byte)(240)))), ((int)(((byte)(240)))));
			this.m_groupGroup.LabelHeight = 31;
			this.m_groupGroup.LabelTextColor = System.Drawing.Color.Black;
			this.m_groupGroup.Location = new System.Drawing.Point(0, 0);
			this.m_groupGroup.Margin = new System.Windows.Forms.Padding(0);
			this.m_groupGroup.Name = "m_groupGroup";
			this.m_groupGroup.Padding = new System.Windows.Forms.Padding(5, 20, 5, 5);
			this._tableLayoutPanel_Center.SetRowSpan(this.m_groupGroup, 2);
			this.m_groupGroup.Size = new System.Drawing.Size(448, 538);
			this.m_groupGroup.TabIndex = 1358;
			this.m_groupGroup.TabStop = false;
			this.m_groupGroup.Text = "GROUP FOLDER";
			this.m_groupGroup.TextAlign = Sys3Controls.EN_TEXTALIGN.MIDDLE_CENTER;
			this.m_groupGroup.ThemeIndex = 0;
			this.m_groupGroup.UseLabelBorder = true;
			this.m_groupGroup.UseTitle = true;
			// 
			// m_groupRecipeFile
			// 
			this.m_groupRecipeFile.BackGroundColor = System.Drawing.Color.WhiteSmoke;
			this.m_groupRecipeFile.Controls.Add(this.m_dgviewFile);
			this.m_groupRecipeFile.Dock = System.Windows.Forms.DockStyle.Fill;
			this.m_groupRecipeFile.EdgeBorderStroke = 2;
			this.m_groupRecipeFile.EdgeRadius = 2;
			this.m_groupRecipeFile.LabelFont = new System.Drawing.Font("맑은 고딕", 10F, System.Drawing.FontStyle.Bold);
			this.m_groupRecipeFile.LabelGradientColorFirst = System.Drawing.Color.FromArgb(((int)(((byte)(208)))), ((int)(((byte)(208)))), ((int)(((byte)(208)))));
			this.m_groupRecipeFile.LabelGradientColorSecond = System.Drawing.Color.FromArgb(((int)(((byte)(240)))), ((int)(((byte)(240)))), ((int)(((byte)(240)))));
			this.m_groupRecipeFile.LabelHeight = 31;
			this.m_groupRecipeFile.LabelTextColor = System.Drawing.Color.Black;
			this.m_groupRecipeFile.Location = new System.Drawing.Point(448, 0);
			this.m_groupRecipeFile.Margin = new System.Windows.Forms.Padding(0);
			this.m_groupRecipeFile.Name = "m_groupRecipeFile";
			this.m_groupRecipeFile.Padding = new System.Windows.Forms.Padding(5, 20, 6, 5);
			this.m_groupRecipeFile.Size = new System.Drawing.Size(682, 456);
			this.m_groupRecipeFile.TabIndex = 1359;
			this.m_groupRecipeFile.TabStop = false;
			this.m_groupRecipeFile.Text = "RECIPE FILE";
			this.m_groupRecipeFile.TextAlign = Sys3Controls.EN_TEXTALIGN.MIDDLE_CENTER;
			this.m_groupRecipeFile.ThemeIndex = 0;
			this.m_groupRecipeFile.UseLabelBorder = true;
			this.m_groupRecipeFile.UseTitle = true;
			// 
			// m_groupGroupConfig
			// 
			this.m_groupGroupConfig.BackGroundColor = System.Drawing.Color.WhiteSmoke;
			this.m_groupGroupConfig.Controls.Add(this._tableLayoutPanel_GroupFolderConfig);
			this.m_groupGroupConfig.Dock = System.Windows.Forms.DockStyle.Fill;
			this.m_groupGroupConfig.EdgeBorderStroke = 2;
			this.m_groupGroupConfig.EdgeRadius = 2;
			this.m_groupGroupConfig.LabelFont = new System.Drawing.Font("맑은 고딕", 10F, System.Drawing.FontStyle.Bold);
			this.m_groupGroupConfig.LabelGradientColorFirst = System.Drawing.Color.FromArgb(((int)(((byte)(208)))), ((int)(((byte)(208)))), ((int)(((byte)(208)))));
			this.m_groupGroupConfig.LabelGradientColorSecond = System.Drawing.Color.FromArgb(((int)(((byte)(240)))), ((int)(((byte)(240)))), ((int)(((byte)(240)))));
			this.m_groupGroupConfig.LabelHeight = 30;
			this.m_groupGroupConfig.LabelTextColor = System.Drawing.Color.Black;
			this.m_groupGroupConfig.Location = new System.Drawing.Point(0, 538);
			this.m_groupGroupConfig.Margin = new System.Windows.Forms.Padding(0);
			this.m_groupGroupConfig.Name = "m_groupGroupConfig";
			this.m_groupGroupConfig.Padding = new System.Windows.Forms.Padding(5, 18, 5, 5);
			this.m_groupGroupConfig.Size = new System.Drawing.Size(448, 195);
			this.m_groupGroupConfig.TabIndex = 1360;
			this.m_groupGroupConfig.TabStop = false;
			this.m_groupGroupConfig.Text = "GROUP FOLDER CONFIGURATION";
			this.m_groupGroupConfig.TextAlign = Sys3Controls.EN_TEXTALIGN.MIDDLE_CENTER;
			this.m_groupGroupConfig.ThemeIndex = 0;
			this.m_groupGroupConfig.UseLabelBorder = true;
			this.m_groupGroupConfig.UseTitle = true;
			// 
			// _tableLayoutPanel_GroupFolderConfig
			// 
			this._tableLayoutPanel_GroupFolderConfig.ColumnCount = 3;
			this._tableLayoutPanel_GroupFolderConfig.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 33.33332F));
			this._tableLayoutPanel_GroupFolderConfig.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 33.33334F));
			this._tableLayoutPanel_GroupFolderConfig.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 33.33334F));
			this._tableLayoutPanel_GroupFolderConfig.Controls.Add(this.m_btnGCreate, 0, 1);
			this._tableLayoutPanel_GroupFolderConfig.Controls.Add(this.m_btnGModify, 1, 1);
			this._tableLayoutPanel_GroupFolderConfig.Controls.Add(this.m_btnEnter, 2, 1);
			this._tableLayoutPanel_GroupFolderConfig.Controls.Add(this.m_btnGCopy, 0, 2);
			this._tableLayoutPanel_GroupFolderConfig.Controls.Add(this.m_btnPrevious, 2, 2);
			this._tableLayoutPanel_GroupFolderConfig.Controls.Add(this.m_lblGFolder, 0, 0);
			this._tableLayoutPanel_GroupFolderConfig.Controls.Add(this.m_labelGFolder, 1, 0);
			this._tableLayoutPanel_GroupFolderConfig.Controls.Add(this.m_btnGRemove, 1, 2);
			this._tableLayoutPanel_GroupFolderConfig.Dock = System.Windows.Forms.DockStyle.Fill;
			this._tableLayoutPanel_GroupFolderConfig.Location = new System.Drawing.Point(5, 32);
			this._tableLayoutPanel_GroupFolderConfig.Margin = new System.Windows.Forms.Padding(0);
			this._tableLayoutPanel_GroupFolderConfig.Name = "_tableLayoutPanel_GroupFolderConfig";
			this._tableLayoutPanel_GroupFolderConfig.RowCount = 3;
			this._tableLayoutPanel_GroupFolderConfig.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 47F));
			this._tableLayoutPanel_GroupFolderConfig.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
			this._tableLayoutPanel_GroupFolderConfig.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
			this._tableLayoutPanel_GroupFolderConfig.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
			this._tableLayoutPanel_GroupFolderConfig.Size = new System.Drawing.Size(438, 158);
			this._tableLayoutPanel_GroupFolderConfig.TabIndex = 1365;
			// 
			// m_labelGFolder
			// 
			this.m_labelGFolder.BackGroundColor = System.Drawing.Color.WhiteSmoke;
			this.m_labelGFolder.BorderStroke = 2;
			this.m_labelGFolder.BorderStyle = System.Windows.Forms.ButtonBorderStyle.Solid;
			this._tableLayoutPanel_GroupFolderConfig.SetColumnSpan(this.m_labelGFolder, 2);
			this.m_labelGFolder.Description = "";
			this.m_labelGFolder.DisabledColor = System.Drawing.Color.LightGray;
			this.m_labelGFolder.Dock = System.Windows.Forms.DockStyle.Fill;
			this.m_labelGFolder.EdgeRadius = 1;
			this.m_labelGFolder.ImagePosition = new System.Drawing.Point(0, 0);
			this.m_labelGFolder.ImageSize = new System.Drawing.Point(0, 0);
			this.m_labelGFolder.LoadImage = null;
			this.m_labelGFolder.Location = new System.Drawing.Point(146, 1);
			this.m_labelGFolder.MainFont = new System.Drawing.Font("맑은 고딕", 11F, System.Drawing.FontStyle.Bold);
			this.m_labelGFolder.MainFontColor = System.Drawing.Color.Black;
			this.m_labelGFolder.Margin = new System.Windows.Forms.Padding(1);
			this.m_labelGFolder.Name = "m_labelGFolder";
			this.m_labelGFolder.Size = new System.Drawing.Size(291, 45);
			this.m_labelGFolder.SubFont = new System.Drawing.Font("맑은 고딕", 14F, System.Drawing.FontStyle.Bold);
			this.m_labelGFolder.SubFontColor = System.Drawing.Color.Gray;
			this.m_labelGFolder.SubText = "";
			this.m_labelGFolder.TabIndex = 0;
			this.m_labelGFolder.TextAlignMain = Sys3Controls.EN_TEXTALIGN.MIDDLE_CENTER;
			this.m_labelGFolder.TextAlignSub = Sys3Controls.EN_TEXTALIGN.MIDDLE_RIGHT;
			this.m_labelGFolder.TextAlignUnit = Sys3Controls.EN_TEXTALIGN.MIDDLE_CENTER;
			this.m_labelGFolder.ThemeIndex = 0;
			this.m_labelGFolder.UnitAreaRate = 40;
			this.m_labelGFolder.UnitFont = new System.Drawing.Font("맑은 고딕", 10F, System.Drawing.FontStyle.Bold);
			this.m_labelGFolder.UnitFontColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
			this.m_labelGFolder.UnitPositionVertical = false;
			this.m_labelGFolder.UnitText = "";
			this.m_labelGFolder.UseBorder = true;
			this.m_labelGFolder.UseEdgeRadius = false;
			this.m_labelGFolder.UseImage = false;
			this.m_labelGFolder.UseSubFont = true;
			this.m_labelGFolder.UseUnitFont = false;
			// 
			// m_btnGCreate
			// 
			this.m_btnGCreate.BorderWidth = 3;
			this.m_btnGCreate.ButtonClicked = false;
			this.m_btnGCreate.ClickedEmphasizeTextColor = System.Drawing.Color.White;
			this.m_btnGCreate.CustomClickedGradientFirstColor = System.Drawing.Color.White;
			this.m_btnGCreate.CustomClickedGradientSecondColor = System.Drawing.Color.White;
			this.m_btnGCreate.Description = "";
			this.m_btnGCreate.DisabledColor = System.Drawing.Color.DarkGray;
			this.m_btnGCreate.Dock = System.Windows.Forms.DockStyle.Fill;
			this.m_btnGCreate.EdgeRadius = 5;
			this.m_btnGCreate.GradientAngle = 60F;
			this.m_btnGCreate.GradientFirstColor = System.Drawing.Color.White;
			this.m_btnGCreate.GradientSecondColor = System.Drawing.Color.FromArgb(((int)(((byte)(170)))), ((int)(((byte)(176)))), ((int)(((byte)(183)))));
			this.m_btnGCreate.HoverEmphasizeCustomColor = System.Drawing.Color.White;
			this.m_btnGCreate.ImagePosition = new System.Drawing.Point(15, 15);
			this.m_btnGCreate.ImageSize = new System.Drawing.Point(30, 30);
			this.m_btnGCreate.LoadImage = global::FrameOfSystem3.Properties.Resources.CREATE_FOLDER;
			this.m_btnGCreate.Location = new System.Drawing.Point(3, 50);
			this.m_btnGCreate.MainFont = new System.Drawing.Font("맑은 고딕", 15F, System.Drawing.FontStyle.Bold);
			this.m_btnGCreate.MainFontColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(36)))), ((int)(((byte)(0)))));
			this.m_btnGCreate.Name = "m_btnGCreate";
			this.m_btnGCreate.Size = new System.Drawing.Size(139, 49);
			this.m_btnGCreate.SubFont = new System.Drawing.Font("맑은 고딕", 8F, System.Drawing.FontStyle.Bold);
			this.m_btnGCreate.SubFontColor = System.Drawing.Color.DarkBlue;
			this.m_btnGCreate.SubText = "STATUS";
			this.m_btnGCreate.TabIndex = 0;
			this.m_btnGCreate.Text = "CREATE";
			this.m_btnGCreate.TextAlignMain = Sys3Controls.EN_TEXTALIGN.MIDDLE_RIGHT;
			this.m_btnGCreate.TextAlignSub = Sys3Controls.EN_TEXTALIGN.TOP_LEFT;
			this.m_btnGCreate.ThemeIndex = 0;
			this.m_btnGCreate.UseBorder = true;
			this.m_btnGCreate.UseClickedEmphasizeTextColor = false;
			this.m_btnGCreate.UseCustomizeClickedColor = false;
			this.m_btnGCreate.UseEdge = true;
			this.m_btnGCreate.UseHoverEmphasizeCustomColor = false;
			this.m_btnGCreate.UseImage = true;
			this.m_btnGCreate.UserHoverEmpahsize = false;
			this.m_btnGCreate.UseSubFont = false;
			this.m_btnGCreate.Click += new System.EventHandler(this.Click_FolderConfiguation);
			// 
			// m_btnGModify
			// 
			this.m_btnGModify.BorderWidth = 3;
			this.m_btnGModify.ButtonClicked = false;
			this.m_btnGModify.ClickedEmphasizeTextColor = System.Drawing.Color.White;
			this.m_btnGModify.CustomClickedGradientFirstColor = System.Drawing.Color.White;
			this.m_btnGModify.CustomClickedGradientSecondColor = System.Drawing.Color.White;
			this.m_btnGModify.Description = "";
			this.m_btnGModify.DisabledColor = System.Drawing.Color.DarkGray;
			this.m_btnGModify.Dock = System.Windows.Forms.DockStyle.Fill;
			this.m_btnGModify.EdgeRadius = 5;
			this.m_btnGModify.GradientAngle = 60F;
			this.m_btnGModify.GradientFirstColor = System.Drawing.Color.White;
			this.m_btnGModify.GradientSecondColor = System.Drawing.Color.FromArgb(((int)(((byte)(170)))), ((int)(((byte)(176)))), ((int)(((byte)(183)))));
			this.m_btnGModify.HoverEmphasizeCustomColor = System.Drawing.Color.White;
			this.m_btnGModify.ImagePosition = new System.Drawing.Point(15, 15);
			this.m_btnGModify.ImageSize = new System.Drawing.Point(30, 30);
			this.m_btnGModify.LoadImage = global::FrameOfSystem3.Properties.Resources.MODIFY_CHANGE;
			this.m_btnGModify.Location = new System.Drawing.Point(148, 50);
			this.m_btnGModify.MainFont = new System.Drawing.Font("맑은 고딕", 15F, System.Drawing.FontStyle.Bold);
			this.m_btnGModify.MainFontColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(36)))), ((int)(((byte)(0)))));
			this.m_btnGModify.Name = "m_btnGModify";
			this.m_btnGModify.Size = new System.Drawing.Size(140, 49);
			this.m_btnGModify.SubFont = new System.Drawing.Font("맑은 고딕", 8F, System.Drawing.FontStyle.Bold);
			this.m_btnGModify.SubFontColor = System.Drawing.Color.DarkBlue;
			this.m_btnGModify.SubText = "STATUS";
			this.m_btnGModify.TabIndex = 1;
			this.m_btnGModify.Text = "MODIFY";
			this.m_btnGModify.TextAlignMain = Sys3Controls.EN_TEXTALIGN.MIDDLE_RIGHT;
			this.m_btnGModify.TextAlignSub = Sys3Controls.EN_TEXTALIGN.TOP_LEFT;
			this.m_btnGModify.ThemeIndex = 0;
			this.m_btnGModify.UseBorder = true;
			this.m_btnGModify.UseClickedEmphasizeTextColor = false;
			this.m_btnGModify.UseCustomizeClickedColor = false;
			this.m_btnGModify.UseEdge = true;
			this.m_btnGModify.UseHoverEmphasizeCustomColor = false;
			this.m_btnGModify.UseImage = true;
			this.m_btnGModify.UserHoverEmpahsize = false;
			this.m_btnGModify.UseSubFont = false;
			this.m_btnGModify.Click += new System.EventHandler(this.Click_FolderConfiguation);
			// 
			// m_btnEnter
			// 
			this.m_btnEnter.BorderWidth = 3;
			this.m_btnEnter.ButtonClicked = false;
			this.m_btnEnter.ClickedEmphasizeTextColor = System.Drawing.Color.White;
			this.m_btnEnter.CustomClickedGradientFirstColor = System.Drawing.Color.White;
			this.m_btnEnter.CustomClickedGradientSecondColor = System.Drawing.Color.White;
			this.m_btnEnter.Description = "";
			this.m_btnEnter.DisabledColor = System.Drawing.Color.DarkGray;
			this.m_btnEnter.Dock = System.Windows.Forms.DockStyle.Fill;
			this.m_btnEnter.EdgeRadius = 5;
			this.m_btnEnter.GradientAngle = 70F;
			this.m_btnEnter.GradientFirstColor = System.Drawing.Color.White;
			this.m_btnEnter.GradientSecondColor = System.Drawing.Color.FromArgb(((int)(((byte)(170)))), ((int)(((byte)(176)))), ((int)(((byte)(183)))));
			this.m_btnEnter.HoverEmphasizeCustomColor = System.Drawing.Color.White;
			this.m_btnEnter.ImagePosition = new System.Drawing.Point(13, 13);
			this.m_btnEnter.ImageSize = new System.Drawing.Point(30, 30);
			this.m_btnEnter.LoadImage = global::FrameOfSystem3.Properties.Resources.ENTER;
			this.m_btnEnter.Location = new System.Drawing.Point(294, 50);
			this.m_btnEnter.MainFont = new System.Drawing.Font("맑은 고딕", 13F, System.Drawing.FontStyle.Bold);
			this.m_btnEnter.MainFontColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(36)))), ((int)(((byte)(0)))));
			this.m_btnEnter.Name = "m_btnEnter";
			this.m_btnEnter.Size = new System.Drawing.Size(141, 49);
			this.m_btnEnter.SubFont = new System.Drawing.Font("맑은 고딕", 8F, System.Drawing.FontStyle.Bold);
			this.m_btnEnter.SubFontColor = System.Drawing.Color.DarkBlue;
			this.m_btnEnter.SubText = "STATUS";
			this.m_btnEnter.TabIndex = 0;
			this.m_btnEnter.Text = "ENTER";
			this.m_btnEnter.TextAlignMain = Sys3Controls.EN_TEXTALIGN.MIDDLE_RIGHT;
			this.m_btnEnter.TextAlignSub = Sys3Controls.EN_TEXTALIGN.TOP_LEFT;
			this.m_btnEnter.ThemeIndex = 0;
			this.m_btnEnter.UseBorder = true;
			this.m_btnEnter.UseClickedEmphasizeTextColor = false;
			this.m_btnEnter.UseCustomizeClickedColor = false;
			this.m_btnEnter.UseEdge = true;
			this.m_btnEnter.UseHoverEmphasizeCustomColor = false;
			this.m_btnEnter.UseImage = true;
			this.m_btnEnter.UserHoverEmpahsize = false;
			this.m_btnEnter.UseSubFont = false;
			this.m_btnEnter.Click += new System.EventHandler(this.Click_FolderControl);
			// 
			// m_btnGCopy
			// 
			this.m_btnGCopy.BorderWidth = 3;
			this.m_btnGCopy.ButtonClicked = false;
			this.m_btnGCopy.ClickedEmphasizeTextColor = System.Drawing.Color.White;
			this.m_btnGCopy.CustomClickedGradientFirstColor = System.Drawing.Color.White;
			this.m_btnGCopy.CustomClickedGradientSecondColor = System.Drawing.Color.White;
			this.m_btnGCopy.Description = "";
			this.m_btnGCopy.DisabledColor = System.Drawing.Color.DarkGray;
			this.m_btnGCopy.Dock = System.Windows.Forms.DockStyle.Fill;
			this.m_btnGCopy.EdgeRadius = 5;
			this.m_btnGCopy.GradientAngle = 60F;
			this.m_btnGCopy.GradientFirstColor = System.Drawing.Color.White;
			this.m_btnGCopy.GradientSecondColor = System.Drawing.Color.FromArgb(((int)(((byte)(170)))), ((int)(((byte)(176)))), ((int)(((byte)(183)))));
			this.m_btnGCopy.HoverEmphasizeCustomColor = System.Drawing.Color.White;
			this.m_btnGCopy.ImagePosition = new System.Drawing.Point(15, 15);
			this.m_btnGCopy.ImageSize = new System.Drawing.Point(30, 30);
			this.m_btnGCopy.LoadImage = global::FrameOfSystem3.Properties.Resources.COPY;
			this.m_btnGCopy.Location = new System.Drawing.Point(3, 105);
			this.m_btnGCopy.MainFont = new System.Drawing.Font("맑은 고딕", 15F, System.Drawing.FontStyle.Bold);
			this.m_btnGCopy.MainFontColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(36)))), ((int)(((byte)(0)))));
			this.m_btnGCopy.Name = "m_btnGCopy";
			this.m_btnGCopy.Size = new System.Drawing.Size(139, 50);
			this.m_btnGCopy.SubFont = new System.Drawing.Font("맑은 고딕", 8F, System.Drawing.FontStyle.Bold);
			this.m_btnGCopy.SubFontColor = System.Drawing.Color.DarkBlue;
			this.m_btnGCopy.SubText = "STATUS";
			this.m_btnGCopy.TabIndex = 2;
			this.m_btnGCopy.Text = "COPY";
			this.m_btnGCopy.TextAlignMain = Sys3Controls.EN_TEXTALIGN.MIDDLE_RIGHT;
			this.m_btnGCopy.TextAlignSub = Sys3Controls.EN_TEXTALIGN.TOP_LEFT;
			this.m_btnGCopy.ThemeIndex = 0;
			this.m_btnGCopy.UseBorder = true;
			this.m_btnGCopy.UseClickedEmphasizeTextColor = false;
			this.m_btnGCopy.UseCustomizeClickedColor = false;
			this.m_btnGCopy.UseEdge = true;
			this.m_btnGCopy.UseHoverEmphasizeCustomColor = false;
			this.m_btnGCopy.UseImage = true;
			this.m_btnGCopy.UserHoverEmpahsize = false;
			this.m_btnGCopy.UseSubFont = false;
			this.m_btnGCopy.Click += new System.EventHandler(this.Click_FolderConfiguation);
			// 
			// m_btnPrevious
			// 
			this.m_btnPrevious.BorderWidth = 3;
			this.m_btnPrevious.ButtonClicked = false;
			this.m_btnPrevious.ClickedEmphasizeTextColor = System.Drawing.Color.White;
			this.m_btnPrevious.CustomClickedGradientFirstColor = System.Drawing.Color.White;
			this.m_btnPrevious.CustomClickedGradientSecondColor = System.Drawing.Color.White;
			this.m_btnPrevious.Description = "";
			this.m_btnPrevious.DisabledColor = System.Drawing.Color.DarkGray;
			this.m_btnPrevious.Dock = System.Windows.Forms.DockStyle.Fill;
			this.m_btnPrevious.EdgeRadius = 5;
			this.m_btnPrevious.GradientAngle = 70F;
			this.m_btnPrevious.GradientFirstColor = System.Drawing.Color.White;
			this.m_btnPrevious.GradientSecondColor = System.Drawing.Color.FromArgb(((int)(((byte)(170)))), ((int)(((byte)(176)))), ((int)(((byte)(183)))));
			this.m_btnPrevious.HoverEmphasizeCustomColor = System.Drawing.Color.White;
			this.m_btnPrevious.ImagePosition = new System.Drawing.Point(13, 13);
			this.m_btnPrevious.ImageSize = new System.Drawing.Point(30, 30);
			this.m_btnPrevious.LoadImage = global::FrameOfSystem3.Properties.Resources.PREVIOUS;
			this.m_btnPrevious.Location = new System.Drawing.Point(294, 105);
			this.m_btnPrevious.MainFont = new System.Drawing.Font("맑은 고딕", 13F, System.Drawing.FontStyle.Bold);
			this.m_btnPrevious.MainFontColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(36)))), ((int)(((byte)(0)))));
			this.m_btnPrevious.Name = "m_btnPrevious";
			this.m_btnPrevious.Size = new System.Drawing.Size(141, 50);
			this.m_btnPrevious.SubFont = new System.Drawing.Font("맑은 고딕", 8F, System.Drawing.FontStyle.Bold);
			this.m_btnPrevious.SubFontColor = System.Drawing.Color.DarkBlue;
			this.m_btnPrevious.SubText = "STATUS";
			this.m_btnPrevious.TabIndex = 1;
			this.m_btnPrevious.Text = "PREVIOUS";
			this.m_btnPrevious.TextAlignMain = Sys3Controls.EN_TEXTALIGN.MIDDLE_RIGHT;
			this.m_btnPrevious.TextAlignSub = Sys3Controls.EN_TEXTALIGN.TOP_LEFT;
			this.m_btnPrevious.ThemeIndex = 0;
			this.m_btnPrevious.UseBorder = true;
			this.m_btnPrevious.UseClickedEmphasizeTextColor = false;
			this.m_btnPrevious.UseCustomizeClickedColor = false;
			this.m_btnPrevious.UseEdge = true;
			this.m_btnPrevious.UseHoverEmphasizeCustomColor = false;
			this.m_btnPrevious.UseImage = true;
			this.m_btnPrevious.UserHoverEmpahsize = false;
			this.m_btnPrevious.UseSubFont = false;
			this.m_btnPrevious.Click += new System.EventHandler(this.Click_FolderControl);
			// 
			// m_lblGFolder
			// 
			this.m_lblGFolder.BackGroundColor = System.Drawing.Color.FromArgb(((int)(((byte)(200)))), ((int)(((byte)(200)))), ((int)(((byte)(220)))));
			this.m_lblGFolder.BorderStroke = 2;
			this.m_lblGFolder.BorderStyle = System.Windows.Forms.ButtonBorderStyle.Solid;
			this.m_lblGFolder.Description = "";
			this.m_lblGFolder.DisabledColor = System.Drawing.Color.DarkGray;
			this.m_lblGFolder.Dock = System.Windows.Forms.DockStyle.Fill;
			this.m_lblGFolder.EdgeRadius = 1;
			this.m_lblGFolder.ImagePosition = new System.Drawing.Point(0, 0);
			this.m_lblGFolder.ImageSize = new System.Drawing.Point(0, 0);
			this.m_lblGFolder.LoadImage = null;
			this.m_lblGFolder.Location = new System.Drawing.Point(1, 1);
			this.m_lblGFolder.MainFont = new System.Drawing.Font("맑은 고딕", 10F, System.Drawing.FontStyle.Bold);
			this.m_lblGFolder.MainFontColor = System.Drawing.Color.Black;
			this.m_lblGFolder.Margin = new System.Windows.Forms.Padding(1);
			this.m_lblGFolder.Name = "m_lblGFolder";
			this.m_lblGFolder.Size = new System.Drawing.Size(143, 45);
			this.m_lblGFolder.SubFont = new System.Drawing.Font("맑은 고딕", 14F, System.Drawing.FontStyle.Bold);
			this.m_lblGFolder.SubFontColor = System.Drawing.Color.Gray;
			this.m_lblGFolder.SubText = "▶";
			this.m_lblGFolder.TabIndex = 1362;
			this.m_lblGFolder.Text = "FOLDER NAME";
			this.m_lblGFolder.TextAlignMain = Sys3Controls.EN_TEXTALIGN.TOP_LEFT;
			this.m_lblGFolder.TextAlignSub = Sys3Controls.EN_TEXTALIGN.MIDDLE_RIGHT;
			this.m_lblGFolder.TextAlignUnit = Sys3Controls.EN_TEXTALIGN.MIDDLE_CENTER;
			this.m_lblGFolder.ThemeIndex = 0;
			this.m_lblGFolder.UnitAreaRate = 40;
			this.m_lblGFolder.UnitFont = new System.Drawing.Font("맑은 고딕", 10F, System.Drawing.FontStyle.Bold);
			this.m_lblGFolder.UnitFontColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
			this.m_lblGFolder.UnitPositionVertical = false;
			this.m_lblGFolder.UnitText = "";
			this.m_lblGFolder.UseBorder = true;
			this.m_lblGFolder.UseEdgeRadius = false;
			this.m_lblGFolder.UseImage = false;
			this.m_lblGFolder.UseSubFont = false;
			this.m_lblGFolder.UseUnitFont = false;
			// 
			// m_btnGRemove
			// 
			this.m_btnGRemove.BorderWidth = 3;
			this.m_btnGRemove.ButtonClicked = false;
			this.m_btnGRemove.ClickedEmphasizeTextColor = System.Drawing.Color.White;
			this.m_btnGRemove.CustomClickedGradientFirstColor = System.Drawing.Color.White;
			this.m_btnGRemove.CustomClickedGradientSecondColor = System.Drawing.Color.White;
			this.m_btnGRemove.Description = "";
			this.m_btnGRemove.DisabledColor = System.Drawing.Color.DarkGray;
			this.m_btnGRemove.Dock = System.Windows.Forms.DockStyle.Fill;
			this.m_btnGRemove.EdgeRadius = 5;
			this.m_btnGRemove.GradientAngle = 60F;
			this.m_btnGRemove.GradientFirstColor = System.Drawing.Color.White;
			this.m_btnGRemove.GradientSecondColor = System.Drawing.Color.FromArgb(((int)(((byte)(170)))), ((int)(((byte)(176)))), ((int)(((byte)(183)))));
			this.m_btnGRemove.HoverEmphasizeCustomColor = System.Drawing.Color.White;
			this.m_btnGRemove.ImagePosition = new System.Drawing.Point(15, 15);
			this.m_btnGRemove.ImageSize = new System.Drawing.Point(30, 30);
			this.m_btnGRemove.LoadImage = global::FrameOfSystem3.Properties.Resources.REMOVE;
			this.m_btnGRemove.Location = new System.Drawing.Point(148, 105);
			this.m_btnGRemove.MainFont = new System.Drawing.Font("맑은 고딕", 15F, System.Drawing.FontStyle.Bold);
			this.m_btnGRemove.MainFontColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(36)))), ((int)(((byte)(0)))));
			this.m_btnGRemove.Name = "m_btnGRemove";
			this.m_btnGRemove.Size = new System.Drawing.Size(140, 50);
			this.m_btnGRemove.SubFont = new System.Drawing.Font("맑은 고딕", 8F, System.Drawing.FontStyle.Bold);
			this.m_btnGRemove.SubFontColor = System.Drawing.Color.DarkBlue;
			this.m_btnGRemove.SubText = "STATUS";
			this.m_btnGRemove.TabIndex = 3;
			this.m_btnGRemove.Text = "REMOVE";
			this.m_btnGRemove.TextAlignMain = Sys3Controls.EN_TEXTALIGN.MIDDLE_RIGHT;
			this.m_btnGRemove.TextAlignSub = Sys3Controls.EN_TEXTALIGN.TOP_LEFT;
			this.m_btnGRemove.ThemeIndex = 0;
			this.m_btnGRemove.UseBorder = true;
			this.m_btnGRemove.UseClickedEmphasizeTextColor = false;
			this.m_btnGRemove.UseCustomizeClickedColor = false;
			this.m_btnGRemove.UseEdge = true;
			this.m_btnGRemove.UseHoverEmphasizeCustomColor = false;
			this.m_btnGRemove.UseImage = true;
			this.m_btnGRemove.UserHoverEmpahsize = false;
			this.m_btnGRemove.UseSubFont = false;
			this.m_btnGRemove.Click += new System.EventHandler(this.Click_FolderConfiguation);
			// 
			// m_groupRecipeConfig
			// 
			this.m_groupRecipeConfig.BackGroundColor = System.Drawing.Color.WhiteSmoke;
			this.m_groupRecipeConfig.Controls.Add(this._tableLayoutPanel_RecipeFileConfig);
			this.m_groupRecipeConfig.Dock = System.Windows.Forms.DockStyle.Fill;
			this.m_groupRecipeConfig.EdgeBorderStroke = 2;
			this.m_groupRecipeConfig.EdgeRadius = 2;
			this.m_groupRecipeConfig.LabelFont = new System.Drawing.Font("맑은 고딕", 10F, System.Drawing.FontStyle.Bold);
			this.m_groupRecipeConfig.LabelGradientColorFirst = System.Drawing.Color.FromArgb(((int)(((byte)(208)))), ((int)(((byte)(208)))), ((int)(((byte)(208)))));
			this.m_groupRecipeConfig.LabelGradientColorSecond = System.Drawing.Color.FromArgb(((int)(((byte)(240)))), ((int)(((byte)(240)))), ((int)(((byte)(240)))));
			this.m_groupRecipeConfig.LabelHeight = 30;
			this.m_groupRecipeConfig.LabelTextColor = System.Drawing.Color.Black;
			this.m_groupRecipeConfig.Location = new System.Drawing.Point(448, 538);
			this.m_groupRecipeConfig.Margin = new System.Windows.Forms.Padding(0);
			this.m_groupRecipeConfig.Name = "m_groupRecipeConfig";
			this.m_groupRecipeConfig.Padding = new System.Windows.Forms.Padding(5, 18, 5, 5);
			this.m_groupRecipeConfig.Size = new System.Drawing.Size(682, 195);
			this.m_groupRecipeConfig.TabIndex = 1361;
			this.m_groupRecipeConfig.TabStop = false;
			this.m_groupRecipeConfig.Text = "RECIPE FILE CONFIGURATION";
			this.m_groupRecipeConfig.TextAlign = Sys3Controls.EN_TEXTALIGN.MIDDLE_CENTER;
			this.m_groupRecipeConfig.ThemeIndex = 0;
			this.m_groupRecipeConfig.UseLabelBorder = true;
			this.m_groupRecipeConfig.UseTitle = true;
			// 
			// _tableLayoutPanel_RecipeFileConfig
			// 
			this._tableLayoutPanel_RecipeFileConfig.ColumnCount = 4;
			this._tableLayoutPanel_RecipeFileConfig.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 25F));
			this._tableLayoutPanel_RecipeFileConfig.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 25F));
			this._tableLayoutPanel_RecipeFileConfig.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 25F));
			this._tableLayoutPanel_RecipeFileConfig.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 25F));
			this._tableLayoutPanel_RecipeFileConfig.Controls.Add(this.m_btnRRemove, 3, 1);
			this._tableLayoutPanel_RecipeFileConfig.Controls.Add(this.m_lblRFolder, 0, 0);
			this._tableLayoutPanel_RecipeFileConfig.Controls.Add(this.m_btnRModify, 2, 1);
			this._tableLayoutPanel_RecipeFileConfig.Controls.Add(this.m_labelRFile, 1, 0);
			this._tableLayoutPanel_RecipeFileConfig.Controls.Add(this.m_btnRCreate, 0, 1);
			this._tableLayoutPanel_RecipeFileConfig.Controls.Add(this.m_btnRCopy, 1, 1);
			this._tableLayoutPanel_RecipeFileConfig.Dock = System.Windows.Forms.DockStyle.Fill;
			this._tableLayoutPanel_RecipeFileConfig.Location = new System.Drawing.Point(5, 32);
			this._tableLayoutPanel_RecipeFileConfig.Margin = new System.Windows.Forms.Padding(0);
			this._tableLayoutPanel_RecipeFileConfig.Name = "_tableLayoutPanel_RecipeFileConfig";
			this._tableLayoutPanel_RecipeFileConfig.RowCount = 3;
			this._tableLayoutPanel_RecipeFileConfig.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 47F));
			this._tableLayoutPanel_RecipeFileConfig.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
			this._tableLayoutPanel_RecipeFileConfig.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
			this._tableLayoutPanel_RecipeFileConfig.Size = new System.Drawing.Size(672, 158);
			this._tableLayoutPanel_RecipeFileConfig.TabIndex = 1366;
			// 
			// m_labelRFile
			// 
			this.m_labelRFile.BackGroundColor = System.Drawing.Color.WhiteSmoke;
			this.m_labelRFile.BorderStroke = 2;
			this.m_labelRFile.BorderStyle = System.Windows.Forms.ButtonBorderStyle.Solid;
			this._tableLayoutPanel_RecipeFileConfig.SetColumnSpan(this.m_labelRFile, 3);
			this.m_labelRFile.Description = "";
			this.m_labelRFile.DisabledColor = System.Drawing.Color.LightGray;
			this.m_labelRFile.Dock = System.Windows.Forms.DockStyle.Fill;
			this.m_labelRFile.EdgeRadius = 1;
			this.m_labelRFile.ImagePosition = new System.Drawing.Point(0, 0);
			this.m_labelRFile.ImageSize = new System.Drawing.Point(0, 0);
			this.m_labelRFile.LoadImage = null;
			this.m_labelRFile.Location = new System.Drawing.Point(169, 1);
			this.m_labelRFile.MainFont = new System.Drawing.Font("맑은 고딕", 11F, System.Drawing.FontStyle.Bold);
			this.m_labelRFile.MainFontColor = System.Drawing.Color.Black;
			this.m_labelRFile.Margin = new System.Windows.Forms.Padding(1);
			this.m_labelRFile.Name = "m_labelRFile";
			this.m_labelRFile.Size = new System.Drawing.Size(502, 45);
			this.m_labelRFile.SubFont = new System.Drawing.Font("맑은 고딕", 14F, System.Drawing.FontStyle.Bold);
			this.m_labelRFile.SubFontColor = System.Drawing.Color.Gray;
			this.m_labelRFile.SubText = "";
			this.m_labelRFile.TabIndex = 0;
			this.m_labelRFile.TextAlignMain = Sys3Controls.EN_TEXTALIGN.MIDDLE_CENTER;
			this.m_labelRFile.TextAlignSub = Sys3Controls.EN_TEXTALIGN.MIDDLE_RIGHT;
			this.m_labelRFile.TextAlignUnit = Sys3Controls.EN_TEXTALIGN.MIDDLE_CENTER;
			this.m_labelRFile.ThemeIndex = 0;
			this.m_labelRFile.UnitAreaRate = 40;
			this.m_labelRFile.UnitFont = new System.Drawing.Font("맑은 고딕", 10F, System.Drawing.FontStyle.Bold);
			this.m_labelRFile.UnitFontColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
			this.m_labelRFile.UnitPositionVertical = false;
			this.m_labelRFile.UnitText = "";
			this.m_labelRFile.UseBorder = true;
			this.m_labelRFile.UseEdgeRadius = false;
			this.m_labelRFile.UseImage = false;
			this.m_labelRFile.UseSubFont = true;
			this.m_labelRFile.UseUnitFont = false;
			// 
			// m_btnRRemove
			// 
			this.m_btnRRemove.BorderWidth = 3;
			this.m_btnRRemove.ButtonClicked = false;
			this.m_btnRRemove.ClickedEmphasizeTextColor = System.Drawing.Color.White;
			this.m_btnRRemove.CustomClickedGradientFirstColor = System.Drawing.Color.White;
			this.m_btnRRemove.CustomClickedGradientSecondColor = System.Drawing.Color.White;
			this.m_btnRRemove.Description = "";
			this.m_btnRRemove.DisabledColor = System.Drawing.Color.DarkGray;
			this.m_btnRRemove.Dock = System.Windows.Forms.DockStyle.Fill;
			this.m_btnRRemove.EdgeRadius = 5;
			this.m_btnRRemove.GradientAngle = 80F;
			this.m_btnRRemove.GradientFirstColor = System.Drawing.Color.White;
			this.m_btnRRemove.GradientSecondColor = System.Drawing.Color.FromArgb(((int)(((byte)(170)))), ((int)(((byte)(176)))), ((int)(((byte)(183)))));
			this.m_btnRRemove.HoverEmphasizeCustomColor = System.Drawing.Color.White;
			this.m_btnRRemove.ImagePosition = new System.Drawing.Point(13, 13);
			this.m_btnRRemove.ImageSize = new System.Drawing.Point(30, 30);
			this.m_btnRRemove.LoadImage = global::FrameOfSystem3.Properties.Resources.REMOVE;
			this.m_btnRRemove.Location = new System.Drawing.Point(507, 50);
			this.m_btnRRemove.MainFont = new System.Drawing.Font("맑은 고딕", 15F, System.Drawing.FontStyle.Bold);
			this.m_btnRRemove.MainFontColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(36)))), ((int)(((byte)(0)))));
			this.m_btnRRemove.Name = "m_btnRRemove";
			this._tableLayoutPanel_RecipeFileConfig.SetRowSpan(this.m_btnRRemove, 2);
			this.m_btnRRemove.Size = new System.Drawing.Size(162, 105);
			this.m_btnRRemove.SubFont = new System.Drawing.Font("맑은 고딕", 8F, System.Drawing.FontStyle.Bold);
			this.m_btnRRemove.SubFontColor = System.Drawing.Color.DarkBlue;
			this.m_btnRRemove.SubText = "STATUS";
			this.m_btnRRemove.TabIndex = 3;
			this.m_btnRRemove.Text = "REMOVE";
			this.m_btnRRemove.TextAlignMain = Sys3Controls.EN_TEXTALIGN.MIDDLE_RIGHT;
			this.m_btnRRemove.TextAlignSub = Sys3Controls.EN_TEXTALIGN.TOP_LEFT;
			this.m_btnRRemove.ThemeIndex = 0;
			this.m_btnRRemove.UseBorder = true;
			this.m_btnRRemove.UseClickedEmphasizeTextColor = false;
			this.m_btnRRemove.UseCustomizeClickedColor = false;
			this.m_btnRRemove.UseEdge = true;
			this.m_btnRRemove.UseHoverEmphasizeCustomColor = false;
			this.m_btnRRemove.UseImage = true;
			this.m_btnRRemove.UserHoverEmpahsize = false;
			this.m_btnRRemove.UseSubFont = false;
			this.m_btnRRemove.Click += new System.EventHandler(this.Click_FileConfiguration);
			// 
			// m_btnRModify
			// 
			this.m_btnRModify.BorderWidth = 3;
			this.m_btnRModify.ButtonClicked = false;
			this.m_btnRModify.ClickedEmphasizeTextColor = System.Drawing.Color.White;
			this.m_btnRModify.CustomClickedGradientFirstColor = System.Drawing.Color.White;
			this.m_btnRModify.CustomClickedGradientSecondColor = System.Drawing.Color.White;
			this.m_btnRModify.Description = "";
			this.m_btnRModify.DisabledColor = System.Drawing.Color.DarkGray;
			this.m_btnRModify.Dock = System.Windows.Forms.DockStyle.Fill;
			this.m_btnRModify.EdgeRadius = 5;
			this.m_btnRModify.GradientAngle = 80F;
			this.m_btnRModify.GradientFirstColor = System.Drawing.Color.White;
			this.m_btnRModify.GradientSecondColor = System.Drawing.Color.FromArgb(((int)(((byte)(170)))), ((int)(((byte)(176)))), ((int)(((byte)(183)))));
			this.m_btnRModify.HoverEmphasizeCustomColor = System.Drawing.Color.White;
			this.m_btnRModify.ImagePosition = new System.Drawing.Point(13, 13);
			this.m_btnRModify.ImageSize = new System.Drawing.Point(30, 30);
			this.m_btnRModify.LoadImage = global::FrameOfSystem3.Properties.Resources.COPY;
			this.m_btnRModify.Location = new System.Drawing.Point(339, 50);
			this.m_btnRModify.MainFont = new System.Drawing.Font("맑은 고딕", 15F, System.Drawing.FontStyle.Bold);
			this.m_btnRModify.MainFontColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(36)))), ((int)(((byte)(0)))));
			this.m_btnRModify.Name = "m_btnRModify";
			this._tableLayoutPanel_RecipeFileConfig.SetRowSpan(this.m_btnRModify, 2);
			this.m_btnRModify.Size = new System.Drawing.Size(162, 105);
			this.m_btnRModify.SubFont = new System.Drawing.Font("맑은 고딕", 8F, System.Drawing.FontStyle.Bold);
			this.m_btnRModify.SubFontColor = System.Drawing.Color.DarkBlue;
			this.m_btnRModify.SubText = "STATUS";
			this.m_btnRModify.TabIndex = 2;
			this.m_btnRModify.Text = "RENAME";
			this.m_btnRModify.TextAlignMain = Sys3Controls.EN_TEXTALIGN.MIDDLE_RIGHT;
			this.m_btnRModify.TextAlignSub = Sys3Controls.EN_TEXTALIGN.TOP_LEFT;
			this.m_btnRModify.ThemeIndex = 0;
			this.m_btnRModify.UseBorder = true;
			this.m_btnRModify.UseClickedEmphasizeTextColor = false;
			this.m_btnRModify.UseCustomizeClickedColor = false;
			this.m_btnRModify.UseEdge = true;
			this.m_btnRModify.UseHoverEmphasizeCustomColor = false;
			this.m_btnRModify.UseImage = true;
			this.m_btnRModify.UserHoverEmpahsize = false;
			this.m_btnRModify.UseSubFont = false;
			this.m_btnRModify.Click += new System.EventHandler(this.Click_FileConfiguration);
			// 
			// m_btnRCopy
			// 
			this.m_btnRCopy.BorderWidth = 3;
			this.m_btnRCopy.ButtonClicked = false;
			this.m_btnRCopy.ClickedEmphasizeTextColor = System.Drawing.Color.White;
			this.m_btnRCopy.CustomClickedGradientFirstColor = System.Drawing.Color.White;
			this.m_btnRCopy.CustomClickedGradientSecondColor = System.Drawing.Color.White;
			this.m_btnRCopy.Description = "";
			this.m_btnRCopy.DisabledColor = System.Drawing.Color.DarkGray;
			this.m_btnRCopy.Dock = System.Windows.Forms.DockStyle.Fill;
			this.m_btnRCopy.EdgeRadius = 5;
			this.m_btnRCopy.GradientAngle = 80F;
			this.m_btnRCopy.GradientFirstColor = System.Drawing.Color.White;
			this.m_btnRCopy.GradientSecondColor = System.Drawing.Color.FromArgb(((int)(((byte)(170)))), ((int)(((byte)(176)))), ((int)(((byte)(183)))));
			this.m_btnRCopy.HoverEmphasizeCustomColor = System.Drawing.Color.White;
			this.m_btnRCopy.ImagePosition = new System.Drawing.Point(13, 13);
			this.m_btnRCopy.ImageSize = new System.Drawing.Point(30, 30);
			this.m_btnRCopy.LoadImage = global::FrameOfSystem3.Properties.Resources.COPY;
			this.m_btnRCopy.Location = new System.Drawing.Point(171, 50);
			this.m_btnRCopy.MainFont = new System.Drawing.Font("맑은 고딕", 15F, System.Drawing.FontStyle.Bold);
			this.m_btnRCopy.MainFontColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(36)))), ((int)(((byte)(0)))));
			this.m_btnRCopy.Name = "m_btnRCopy";
			this._tableLayoutPanel_RecipeFileConfig.SetRowSpan(this.m_btnRCopy, 2);
			this.m_btnRCopy.Size = new System.Drawing.Size(162, 105);
			this.m_btnRCopy.SubFont = new System.Drawing.Font("맑은 고딕", 8F, System.Drawing.FontStyle.Bold);
			this.m_btnRCopy.SubFontColor = System.Drawing.Color.DarkBlue;
			this.m_btnRCopy.SubText = "STATUS";
			this.m_btnRCopy.TabIndex = 1;
			this.m_btnRCopy.Text = "COPY";
			this.m_btnRCopy.TextAlignMain = Sys3Controls.EN_TEXTALIGN.MIDDLE_RIGHT;
			this.m_btnRCopy.TextAlignSub = Sys3Controls.EN_TEXTALIGN.TOP_LEFT;
			this.m_btnRCopy.ThemeIndex = 0;
			this.m_btnRCopy.UseBorder = true;
			this.m_btnRCopy.UseClickedEmphasizeTextColor = false;
			this.m_btnRCopy.UseCustomizeClickedColor = false;
			this.m_btnRCopy.UseEdge = true;
			this.m_btnRCopy.UseHoverEmphasizeCustomColor = false;
			this.m_btnRCopy.UseImage = true;
			this.m_btnRCopy.UserHoverEmpahsize = false;
			this.m_btnRCopy.UseSubFont = false;
			this.m_btnRCopy.Click += new System.EventHandler(this.Click_FileConfiguration);
			// 
			// m_btnRCreate
			// 
			this.m_btnRCreate.BorderWidth = 3;
			this.m_btnRCreate.ButtonClicked = false;
			this.m_btnRCreate.ClickedEmphasizeTextColor = System.Drawing.Color.White;
			this.m_btnRCreate.CustomClickedGradientFirstColor = System.Drawing.Color.White;
			this.m_btnRCreate.CustomClickedGradientSecondColor = System.Drawing.Color.White;
			this.m_btnRCreate.Description = "";
			this.m_btnRCreate.DisabledColor = System.Drawing.Color.DarkGray;
			this.m_btnRCreate.Dock = System.Windows.Forms.DockStyle.Fill;
			this.m_btnRCreate.EdgeRadius = 5;
			this.m_btnRCreate.GradientAngle = 80F;
			this.m_btnRCreate.GradientFirstColor = System.Drawing.Color.White;
			this.m_btnRCreate.GradientSecondColor = System.Drawing.Color.FromArgb(((int)(((byte)(170)))), ((int)(((byte)(176)))), ((int)(((byte)(183)))));
			this.m_btnRCreate.HoverEmphasizeCustomColor = System.Drawing.Color.White;
			this.m_btnRCreate.ImagePosition = new System.Drawing.Point(13, 13);
			this.m_btnRCreate.ImageSize = new System.Drawing.Point(30, 30);
			this.m_btnRCreate.LoadImage = global::FrameOfSystem3.Properties.Resources.CREATE_FILE;
			this.m_btnRCreate.Location = new System.Drawing.Point(3, 50);
			this.m_btnRCreate.MainFont = new System.Drawing.Font("맑은 고딕", 15F, System.Drawing.FontStyle.Bold);
			this.m_btnRCreate.MainFontColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(36)))), ((int)(((byte)(0)))));
			this.m_btnRCreate.Name = "m_btnRCreate";
			this._tableLayoutPanel_RecipeFileConfig.SetRowSpan(this.m_btnRCreate, 2);
			this.m_btnRCreate.Size = new System.Drawing.Size(162, 105);
			this.m_btnRCreate.SubFont = new System.Drawing.Font("맑은 고딕", 8F, System.Drawing.FontStyle.Bold);
			this.m_btnRCreate.SubFontColor = System.Drawing.Color.DarkBlue;
			this.m_btnRCreate.SubText = "STATUS";
			this.m_btnRCreate.TabIndex = 0;
			this.m_btnRCreate.Text = "CREATE";
			this.m_btnRCreate.TextAlignMain = Sys3Controls.EN_TEXTALIGN.MIDDLE_RIGHT;
			this.m_btnRCreate.TextAlignSub = Sys3Controls.EN_TEXTALIGN.TOP_LEFT;
			this.m_btnRCreate.ThemeIndex = 0;
			this.m_btnRCreate.UseBorder = true;
			this.m_btnRCreate.UseClickedEmphasizeTextColor = false;
			this.m_btnRCreate.UseCustomizeClickedColor = false;
			this.m_btnRCreate.UseEdge = true;
			this.m_btnRCreate.UseHoverEmphasizeCustomColor = false;
			this.m_btnRCreate.UseImage = true;
			this.m_btnRCreate.UserHoverEmpahsize = false;
			this.m_btnRCreate.UseSubFont = false;
			this.m_btnRCreate.Click += new System.EventHandler(this.Click_FileConfiguration);
			// 
			// m_lblRFolder
			// 
			this.m_lblRFolder.BackGroundColor = System.Drawing.Color.FromArgb(((int)(((byte)(200)))), ((int)(((byte)(200)))), ((int)(((byte)(220)))));
			this.m_lblRFolder.BorderStroke = 2;
			this.m_lblRFolder.BorderStyle = System.Windows.Forms.ButtonBorderStyle.Solid;
			this.m_lblRFolder.Description = "";
			this.m_lblRFolder.DisabledColor = System.Drawing.Color.DarkGray;
			this.m_lblRFolder.Dock = System.Windows.Forms.DockStyle.Fill;
			this.m_lblRFolder.EdgeRadius = 1;
			this.m_lblRFolder.ImagePosition = new System.Drawing.Point(0, 0);
			this.m_lblRFolder.ImageSize = new System.Drawing.Point(0, 0);
			this.m_lblRFolder.LoadImage = null;
			this.m_lblRFolder.Location = new System.Drawing.Point(1, 1);
			this.m_lblRFolder.MainFont = new System.Drawing.Font("맑은 고딕", 10F, System.Drawing.FontStyle.Bold);
			this.m_lblRFolder.MainFontColor = System.Drawing.Color.Black;
			this.m_lblRFolder.Margin = new System.Windows.Forms.Padding(1);
			this.m_lblRFolder.Name = "m_lblRFolder";
			this.m_lblRFolder.Size = new System.Drawing.Size(166, 45);
			this.m_lblRFolder.SubFont = new System.Drawing.Font("맑은 고딕", 14F, System.Drawing.FontStyle.Bold);
			this.m_lblRFolder.SubFontColor = System.Drawing.Color.Gray;
			this.m_lblRFolder.SubText = "▶";
			this.m_lblRFolder.TabIndex = 1364;
			this.m_lblRFolder.Text = "RECIPE FILE";
			this.m_lblRFolder.TextAlignMain = Sys3Controls.EN_TEXTALIGN.TOP_LEFT;
			this.m_lblRFolder.TextAlignSub = Sys3Controls.EN_TEXTALIGN.MIDDLE_RIGHT;
			this.m_lblRFolder.TextAlignUnit = Sys3Controls.EN_TEXTALIGN.MIDDLE_CENTER;
			this.m_lblRFolder.ThemeIndex = 0;
			this.m_lblRFolder.UnitAreaRate = 40;
			this.m_lblRFolder.UnitFont = new System.Drawing.Font("맑은 고딕", 10F, System.Drawing.FontStyle.Bold);
			this.m_lblRFolder.UnitFontColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
			this.m_lblRFolder.UnitPositionVertical = false;
			this.m_lblRFolder.UnitText = "";
			this.m_lblRFolder.UseBorder = true;
			this.m_lblRFolder.UseEdgeRadius = false;
			this.m_lblRFolder.UseImage = false;
			this.m_lblRFolder.UseSubFont = false;
			this.m_lblRFolder.UseUnitFont = false;
			// 
			// sys3GroupBoxContainer1
			// 
			this.sys3GroupBoxContainer1.BackGroundColor = System.Drawing.Color.WhiteSmoke;
			this.sys3GroupBoxContainer1.Controls.Add(this.lbl_Filtering);
			this.sys3GroupBoxContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
			this.sys3GroupBoxContainer1.EdgeBorderStroke = 2;
			this.sys3GroupBoxContainer1.EdgeRadius = 2;
			this.sys3GroupBoxContainer1.LabelFont = new System.Drawing.Font("맑은 고딕", 10F, System.Drawing.FontStyle.Bold);
			this.sys3GroupBoxContainer1.LabelGradientColorFirst = System.Drawing.Color.FromArgb(((int)(((byte)(208)))), ((int)(((byte)(208)))), ((int)(((byte)(208)))));
			this.sys3GroupBoxContainer1.LabelGradientColorSecond = System.Drawing.Color.FromArgb(((int)(((byte)(240)))), ((int)(((byte)(240)))), ((int)(((byte)(240)))));
			this.sys3GroupBoxContainer1.LabelHeight = 30;
			this.sys3GroupBoxContainer1.LabelTextColor = System.Drawing.Color.Black;
			this.sys3GroupBoxContainer1.Location = new System.Drawing.Point(448, 456);
			this.sys3GroupBoxContainer1.Margin = new System.Windows.Forms.Padding(0);
			this.sys3GroupBoxContainer1.Name = "sys3GroupBoxContainer1";
			this.sys3GroupBoxContainer1.Padding = new System.Windows.Forms.Padding(5, 18, 5, 5);
			this.sys3GroupBoxContainer1.Size = new System.Drawing.Size(682, 82);
			this.sys3GroupBoxContainer1.TabIndex = 1362;
			this.sys3GroupBoxContainer1.TabStop = false;
			this.sys3GroupBoxContainer1.Text = "FILTERING";
			this.sys3GroupBoxContainer1.TextAlign = Sys3Controls.EN_TEXTALIGN.MIDDLE_CENTER;
			this.sys3GroupBoxContainer1.ThemeIndex = 0;
			this.sys3GroupBoxContainer1.UseLabelBorder = true;
			this.sys3GroupBoxContainer1.UseTitle = true;
			// 
			// lbl_Filtering
			// 
			this.lbl_Filtering.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.lbl_Filtering.BackGroundColor = System.Drawing.Color.WhiteSmoke;
			this.lbl_Filtering.BorderStroke = 2;
			this.lbl_Filtering.BorderStyle = System.Windows.Forms.ButtonBorderStyle.Solid;
			this.lbl_Filtering.Description = "";
			this.lbl_Filtering.DisabledColor = System.Drawing.Color.LightGray;
			this.lbl_Filtering.EdgeRadius = 1;
			this.lbl_Filtering.ImagePosition = new System.Drawing.Point(0, 0);
			this.lbl_Filtering.ImageSize = new System.Drawing.Point(0, 0);
			this.lbl_Filtering.LoadImage = null;
			this.lbl_Filtering.Location = new System.Drawing.Point(6, 33);
			this.lbl_Filtering.MainFont = new System.Drawing.Font("맑은 고딕", 11F, System.Drawing.FontStyle.Bold);
			this.lbl_Filtering.MainFontColor = System.Drawing.Color.Red;
			this.lbl_Filtering.Margin = new System.Windows.Forms.Padding(1, 1, 0, 0);
			this.lbl_Filtering.Name = "lbl_Filtering";
			this.lbl_Filtering.Size = new System.Drawing.Size(671, 43);
			this.lbl_Filtering.SubFont = new System.Drawing.Font("맑은 고딕", 14F, System.Drawing.FontStyle.Bold);
			this.lbl_Filtering.SubFontColor = System.Drawing.Color.Gray;
			this.lbl_Filtering.SubText = "";
			this.lbl_Filtering.TabIndex = 1369;
			this.lbl_Filtering.TextAlignMain = Sys3Controls.EN_TEXTALIGN.MIDDLE_LEFT;
			this.lbl_Filtering.TextAlignSub = Sys3Controls.EN_TEXTALIGN.MIDDLE_RIGHT;
			this.lbl_Filtering.TextAlignUnit = Sys3Controls.EN_TEXTALIGN.MIDDLE_CENTER;
			this.lbl_Filtering.ThemeIndex = 0;
			this.lbl_Filtering.UnitAreaRate = 30;
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
			// Recipe_Main
			// 
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
			this.Controls.Add(this.m_groupRecipeFileManagement);
			this.Name = "Recipe_Main";
			this.Size = new System.Drawing.Size(1140, 900);
			((System.ComponentModel.ISupportInitialize)(this.m_dgviewGroup)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.m_dgviewFile)).EndInit();
			this.m_groupRecipeFileManagement.ResumeLayout(false);
			this._tableLayoutPanel_Main.ResumeLayout(false);
			this._tableLayoutPanel_Bottom.ResumeLayout(false);
			this._tableLayoutPanel_Top.ResumeLayout(false);
			this._tableLayoutPanel_Center.ResumeLayout(false);
			this.m_groupGroup.ResumeLayout(false);
			this.m_groupRecipeFile.ResumeLayout(false);
			this.m_groupGroupConfig.ResumeLayout(false);
			this._tableLayoutPanel_GroupFolderConfig.ResumeLayout(false);
			this.m_groupRecipeConfig.ResumeLayout(false);
			this._tableLayoutPanel_RecipeFileConfig.ResumeLayout(false);
			this.sys3GroupBoxContainer1.ResumeLayout(false);
			this.ResumeLayout(false);

        }

        #endregion

		private System.Windows.Forms.DataGridView m_dgviewGroup;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn1;
		private System.Windows.Forms.DataGridViewTextBoxColumn PATH;
		private System.Windows.Forms.DataGridView m_dgviewFile;
		private Sys3Controls.Sys3Label m_labelFullPath;
		private Sys3Controls.Sys3Label m_lblBasePath;
		private Sys3Controls.Sys3GroupBoxContainer m_groupRecipeFileManagement;
        private Sys3Controls.Sys3GroupBoxContainer m_groupGroup;
        private Sys3Controls.Sys3GroupBoxContainer m_groupRecipeFile;
        private Sys3Controls.Sys3GroupBoxContainer m_groupGroupConfig;
        private Sys3Controls.Sys3GroupBoxContainer m_groupRecipeConfig;
		private Sys3Controls.Sys3Label m_labelGFolder;
		private Sys3Controls.Sys3Label m_labelRFile;
		private Sys3Controls.Sys3Label m_lblGFolder;
		private Sys3Controls.Sys3Label m_lblRFolder;
		private Sys3Controls.Sys3Label m_labelSelectedRecipe;
		private Sys3Controls.Sys3Label m_lblSelectedRecipe;
		private Sys3Controls.Sys3button m_btnGCreate;
		private Sys3Controls.Sys3button m_btnGCopy;
		private Sys3Controls.Sys3button m_btnGModify;
		private Sys3Controls.Sys3button m_btnGRemove;
		private Sys3Controls.Sys3button m_btnEnter;
		private Sys3Controls.Sys3button m_btnPrevious;
		private Sys3Controls.Sys3button m_btnRCreate;
		private Sys3Controls.Sys3button m_btnRCopy;
		private Sys3Controls.Sys3button m_btnRModify;
		private Sys3Controls.Sys3button m_btnRRemove;
		private Sys3Controls.Sys3button m_btnLoad;
		private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn3;
		private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn5;
        private System.Windows.Forms.DataGridViewTextBoxColumn WriteTime;
        private System.Windows.Forms.TableLayoutPanel _tableLayoutPanel_Main;
        private System.Windows.Forms.TableLayoutPanel _tableLayoutPanel_Bottom;
        private System.Windows.Forms.TableLayoutPanel _tableLayoutPanel_Top;
        private System.Windows.Forms.TableLayoutPanel _tableLayoutPanel_Center;
        private System.Windows.Forms.TableLayoutPanel _tableLayoutPanel_GroupFolderConfig;
        private System.Windows.Forms.TableLayoutPanel _tableLayoutPanel_RecipeFileConfig;
		private Sys3Controls.Sys3GroupBoxContainer sys3GroupBoxContainer1;
		private Sys3Controls.Sys3Label lbl_Filtering;
	}
}
