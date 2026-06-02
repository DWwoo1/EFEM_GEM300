namespace FrameOfSystem3.Views.Operation
{
    partial class SubViewOperationSecsGemEventList
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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle3 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle5 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle8 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle6 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle7 = new System.Windows.Forms.DataGridViewCellStyle();
            this.gvStatusVariableList = new Sys3Controls.Sys3DoubleBufferedDataGridView();
            this.dataGridViewTextBoxColumn1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn2 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.btnClearSelection = new Sys3Controls.Sys3button();
            this.tableLayoutPanel5 = new System.Windows.Forms.TableLayoutPanel();
            this.sys3Label1 = new Sys3Controls.Sys3Label();
            this.tableLayoutPanel4 = new System.Windows.Forms.TableLayoutPanel();
            this.gvCollectionEvent = new Sys3Controls.Sys3DoubleBufferedDataGridView();
            this.ColID = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ColName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.sys3Label12 = new Sys3Controls.Sys3Label();
            ((System.ComponentModel.ISupportInitialize)(this.gvStatusVariableList)).BeginInit();
            this.tableLayoutPanel1.SuspendLayout();
            this.tableLayoutPanel5.SuspendLayout();
            this.tableLayoutPanel4.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gvCollectionEvent)).BeginInit();
            this.SuspendLayout();
            // 
            // gvStatusVariableList
            // 
            this.gvStatusVariableList.AllowUserToAddRows = false;
            this.gvStatusVariableList.AllowUserToDeleteRows = false;
            this.gvStatusVariableList.AllowUserToResizeColumns = false;
            this.gvStatusVariableList.AllowUserToResizeRows = false;
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle1.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle1.Font = new System.Drawing.Font("굴림", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            dataGridViewCellStyle1.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.gvStatusVariableList.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
            this.gvStatusVariableList.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            this.gvStatusVariableList.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.dataGridViewTextBoxColumn1,
            this.dataGridViewTextBoxColumn2});
            dataGridViewCellStyle4.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle4.BackColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle4.Font = new System.Drawing.Font("굴림", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            dataGridViewCellStyle4.ForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle4.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle4.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle4.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.gvStatusVariableList.DefaultCellStyle = dataGridViewCellStyle4;
            this.gvStatusVariableList.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gvStatusVariableList.Location = new System.Drawing.Point(5, 36);
            this.gvStatusVariableList.Margin = new System.Windows.Forms.Padding(5, 1, 5, 3);
            this.gvStatusVariableList.MultiSelect = false;
            this.gvStatusVariableList.Name = "gvStatusVariableList";
            this.gvStatusVariableList.ReadOnly = true;
            this.gvStatusVariableList.RowHeadersVisible = false;
            this.gvStatusVariableList.RowHeadersWidthSizeMode = System.Windows.Forms.DataGridViewRowHeadersWidthSizeMode.DisableResizing;
            this.gvStatusVariableList.RowTemplate.Height = 23;
            this.gvStatusVariableList.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.gvStatusVariableList.Size = new System.Drawing.Size(463, 549);
            this.gvStatusVariableList.TabIndex = 20901;
            // 
            // dataGridViewTextBoxColumn1
            // 
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            this.dataGridViewTextBoxColumn1.DefaultCellStyle = dataGridViewCellStyle2;
            this.dataGridViewTextBoxColumn1.HeaderText = "ID";
            this.dataGridViewTextBoxColumn1.Name = "dataGridViewTextBoxColumn1";
            this.dataGridViewTextBoxColumn1.ReadOnly = true;
            this.dataGridViewTextBoxColumn1.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.dataGridViewTextBoxColumn1.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // dataGridViewTextBoxColumn2
            // 
            this.dataGridViewTextBoxColumn2.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            dataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            this.dataGridViewTextBoxColumn2.DefaultCellStyle = dataGridViewCellStyle3;
            this.dataGridViewTextBoxColumn2.HeaderText = "NAME";
            this.dataGridViewTextBoxColumn2.Name = "dataGridViewTextBoxColumn2";
            this.dataGridViewTextBoxColumn2.ReadOnly = true;
            this.dataGridViewTextBoxColumn2.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.dataGridViewTextBoxColumn2.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 3;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 41.66667F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 41.66667F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 16.66667F));
            this.tableLayoutPanel1.Controls.Add(this.btnClearSelection, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.tableLayoutPanel5, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.tableLayoutPanel4, 0, 0);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel1.Margin = new System.Windows.Forms.Padding(0);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 1;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(1137, 588);
            this.tableLayoutPanel1.TabIndex = 4;
            // 
            // btnClearSelection
            // 
            this.btnClearSelection.BorderWidth = 2;
            this.btnClearSelection.ButtonClicked = false;
            this.btnClearSelection.ClickedEmphasizeTextColor = System.Drawing.Color.White;
            this.btnClearSelection.CustomClickedGradientFirstColor = System.Drawing.Color.BlanchedAlmond;
            this.btnClearSelection.CustomClickedGradientSecondColor = System.Drawing.Color.Gold;
            this.btnClearSelection.Description = "";
            this.btnClearSelection.DisabledColor = System.Drawing.Color.DarkGray;
            this.btnClearSelection.Dock = System.Windows.Forms.DockStyle.Top;
            this.btnClearSelection.EdgeRadius = 5;
            this.btnClearSelection.GradientAngle = 60F;
            this.btnClearSelection.GradientFirstColor = System.Drawing.Color.Silver;
            this.btnClearSelection.GradientSecondColor = System.Drawing.Color.Gray;
            this.btnClearSelection.HoverEmphasizeCustomColor = System.Drawing.Color.Firebrick;
            this.btnClearSelection.ImagePosition = new System.Drawing.Point(37, 25);
            this.btnClearSelection.ImageSize = new System.Drawing.Point(30, 30);
            this.btnClearSelection.LoadImage = null;
            this.btnClearSelection.Location = new System.Drawing.Point(951, 3);
            this.btnClearSelection.MainFont = new System.Drawing.Font("맑은 고딕", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.btnClearSelection.MainFontColor = System.Drawing.Color.White;
            this.btnClearSelection.Margin = new System.Windows.Forms.Padding(5, 3, 5, 3);
            this.btnClearSelection.Name = "btnClearSelection";
            this.btnClearSelection.Size = new System.Drawing.Size(181, 61);
            this.btnClearSelection.SubFont = new System.Drawing.Font("맑은 고딕", 10F);
            this.btnClearSelection.SubFontColor = System.Drawing.Color.Black;
            this.btnClearSelection.SubText = "";
            this.btnClearSelection.TabIndex = 20902;
            this.btnClearSelection.Text = "CLEAR";
            this.btnClearSelection.TextAlignMain = Sys3Controls.EN_TEXTALIGN.MIDDLE_CENTER;
            this.btnClearSelection.TextAlignSub = Sys3Controls.EN_TEXTALIGN.MIDDLE_CENTER;
            this.btnClearSelection.ThemeIndex = 0;
            this.btnClearSelection.UseBorder = true;
            this.btnClearSelection.UseClickedEmphasizeTextColor = false;
            this.btnClearSelection.UseCustomizeClickedColor = true;
            this.btnClearSelection.UseEdge = true;
            this.btnClearSelection.UseHoverEmphasizeCustomColor = true;
            this.btnClearSelection.UseImage = true;
            this.btnClearSelection.UserHoverEmpahsize = true;
            this.btnClearSelection.UseSubFont = false;
            this.btnClearSelection.Click += new System.EventHandler(this.BtnClearSelectionClicked);
            // 
            // tableLayoutPanel5
            // 
            this.tableLayoutPanel5.ColumnCount = 1;
            this.tableLayoutPanel5.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel5.Controls.Add(this.gvStatusVariableList, 0, 1);
            this.tableLayoutPanel5.Controls.Add(this.sys3Label1, 0, 0);
            this.tableLayoutPanel5.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel5.Location = new System.Drawing.Point(473, 0);
            this.tableLayoutPanel5.Margin = new System.Windows.Forms.Padding(0);
            this.tableLayoutPanel5.Name = "tableLayoutPanel5";
            this.tableLayoutPanel5.RowCount = 2;
            this.tableLayoutPanel5.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 6F));
            this.tableLayoutPanel5.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 94F));
            this.tableLayoutPanel5.Size = new System.Drawing.Size(473, 588);
            this.tableLayoutPanel5.TabIndex = 3;
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
            this.sys3Label1.Location = new System.Drawing.Point(5, 1);
            this.sys3Label1.MainFont = new System.Drawing.Font("맑은 고딕", 11F, System.Drawing.FontStyle.Bold);
            this.sys3Label1.MainFontColor = System.Drawing.Color.Black;
            this.sys3Label1.Margin = new System.Windows.Forms.Padding(5, 1, 5, 1);
            this.sys3Label1.Name = "sys3Label1";
            this.sys3Label1.Size = new System.Drawing.Size(463, 33);
            this.sys3Label1.SubFont = new System.Drawing.Font("맑은 고딕", 10F, System.Drawing.FontStyle.Bold);
            this.sys3Label1.SubFontColor = System.Drawing.Color.Gray;
            this.sys3Label1.SubText = "";
            this.sys3Label1.TabIndex = 20849;
            this.sys3Label1.Text = "COLLECTION EVENT LIST";
            this.sys3Label1.TextAlignMain = Sys3Controls.EN_TEXTALIGN.MIDDLE_CENTER;
            this.sys3Label1.TextAlignSub = Sys3Controls.EN_TEXTALIGN.TOP_LEFT;
            this.sys3Label1.TextAlignUnit = Sys3Controls.EN_TEXTALIGN.MIDDLE_CENTER;
            this.sys3Label1.ThemeIndex = 0;
            this.sys3Label1.UnitAreaRate = 30;
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
            // tableLayoutPanel4
            // 
            this.tableLayoutPanel4.ColumnCount = 1;
            this.tableLayoutPanel4.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel4.Controls.Add(this.gvCollectionEvent, 0, 1);
            this.tableLayoutPanel4.Controls.Add(this.sys3Label12, 0, 0);
            this.tableLayoutPanel4.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel4.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel4.Margin = new System.Windows.Forms.Padding(0);
            this.tableLayoutPanel4.Name = "tableLayoutPanel4";
            this.tableLayoutPanel4.RowCount = 2;
            this.tableLayoutPanel4.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 6F));
            this.tableLayoutPanel4.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 94F));
            this.tableLayoutPanel4.Size = new System.Drawing.Size(473, 588);
            this.tableLayoutPanel4.TabIndex = 2;
            // 
            // gvCollectionEvent
            // 
            this.gvCollectionEvent.AllowUserToAddRows = false;
            this.gvCollectionEvent.AllowUserToDeleteRows = false;
            this.gvCollectionEvent.AllowUserToResizeColumns = false;
            this.gvCollectionEvent.AllowUserToResizeRows = false;
            dataGridViewCellStyle5.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle5.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle5.Font = new System.Drawing.Font("굴림", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            dataGridViewCellStyle5.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle5.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle5.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle5.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.gvCollectionEvent.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle5;
            this.gvCollectionEvent.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            this.gvCollectionEvent.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.ColID,
            this.ColName});
            dataGridViewCellStyle8.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle8.BackColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle8.Font = new System.Drawing.Font("굴림", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            dataGridViewCellStyle8.ForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle8.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle8.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle8.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.gvCollectionEvent.DefaultCellStyle = dataGridViewCellStyle8;
            this.gvCollectionEvent.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gvCollectionEvent.Location = new System.Drawing.Point(5, 36);
            this.gvCollectionEvent.Margin = new System.Windows.Forms.Padding(5, 1, 5, 3);
            this.gvCollectionEvent.MultiSelect = false;
            this.gvCollectionEvent.Name = "gvCollectionEvent";
            this.gvCollectionEvent.ReadOnly = true;
            this.gvCollectionEvent.RowHeadersVisible = false;
            this.gvCollectionEvent.RowHeadersWidthSizeMode = System.Windows.Forms.DataGridViewRowHeadersWidthSizeMode.DisableResizing;
            this.gvCollectionEvent.RowTemplate.Height = 23;
            this.gvCollectionEvent.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.gvCollectionEvent.Size = new System.Drawing.Size(463, 549);
            this.gvCollectionEvent.TabIndex = 20896;
            this.gvCollectionEvent.SelectionChanged += new System.EventHandler(this.GvCollectionEventSelectionChanged);
            // 
            // ColID
            // 
            dataGridViewCellStyle6.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            this.ColID.DefaultCellStyle = dataGridViewCellStyle6;
            this.ColID.HeaderText = "ID";
            this.ColID.Name = "ColID";
            this.ColID.ReadOnly = true;
            this.ColID.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.ColID.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // ColName
            // 
            this.ColName.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            dataGridViewCellStyle7.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            this.ColName.DefaultCellStyle = dataGridViewCellStyle7;
            this.ColName.HeaderText = "NAME";
            this.ColName.Name = "ColName";
            this.ColName.ReadOnly = true;
            this.ColName.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.ColName.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // sys3Label12
            // 
            this.sys3Label12.BackGroundColor = System.Drawing.Color.DarkGray;
            this.sys3Label12.BorderStroke = 2;
            this.sys3Label12.BorderStyle = System.Windows.Forms.ButtonBorderStyle.Solid;
            this.sys3Label12.Description = "";
            this.sys3Label12.DisabledColor = System.Drawing.Color.DarkGray;
            this.sys3Label12.Dock = System.Windows.Forms.DockStyle.Fill;
            this.sys3Label12.EdgeRadius = 1;
            this.sys3Label12.ImagePosition = new System.Drawing.Point(0, 0);
            this.sys3Label12.ImageSize = new System.Drawing.Point(0, 0);
            this.sys3Label12.LoadImage = null;
            this.sys3Label12.Location = new System.Drawing.Point(5, 1);
            this.sys3Label12.MainFont = new System.Drawing.Font("맑은 고딕", 11F, System.Drawing.FontStyle.Bold);
            this.sys3Label12.MainFontColor = System.Drawing.Color.Black;
            this.sys3Label12.Margin = new System.Windows.Forms.Padding(5, 1, 5, 1);
            this.sys3Label12.Name = "sys3Label12";
            this.sys3Label12.Size = new System.Drawing.Size(463, 33);
            this.sys3Label12.SubFont = new System.Drawing.Font("맑은 고딕", 10F, System.Drawing.FontStyle.Bold);
            this.sys3Label12.SubFontColor = System.Drawing.Color.Gray;
            this.sys3Label12.SubText = "";
            this.sys3Label12.TabIndex = 20849;
            this.sys3Label12.Text = "COLLECTION EVENT LIST";
            this.sys3Label12.TextAlignMain = Sys3Controls.EN_TEXTALIGN.MIDDLE_CENTER;
            this.sys3Label12.TextAlignSub = Sys3Controls.EN_TEXTALIGN.TOP_LEFT;
            this.sys3Label12.TextAlignUnit = Sys3Controls.EN_TEXTALIGN.MIDDLE_CENTER;
            this.sys3Label12.ThemeIndex = 0;
            this.sys3Label12.UnitAreaRate = 30;
            this.sys3Label12.UnitFont = new System.Drawing.Font("맑은 고딕", 10F, System.Drawing.FontStyle.Bold);
            this.sys3Label12.UnitFontColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.sys3Label12.UnitPositionVertical = false;
            this.sys3Label12.UnitText = "";
            this.sys3Label12.UseBorder = true;
            this.sys3Label12.UseEdgeRadius = false;
            this.sys3Label12.UseImage = false;
            this.sys3Label12.UseSubFont = true;
            this.sys3Label12.UseUnitFont = false;
            // 
            // SubViewOperationSecsGemEventList
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.Controls.Add(this.tableLayoutPanel1);
            this.Margin = new System.Windows.Forms.Padding(0);
            this.Name = "SubViewOperationSecsGemEventList";
            this.Size = new System.Drawing.Size(1137, 588);
            this.Tag = "";
            ((System.ComponentModel.ISupportInitialize)(this.gvStatusVariableList)).EndInit();
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel5.ResumeLayout(false);
            this.tableLayoutPanel4.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.gvCollectionEvent)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private Sys3Controls.Sys3DoubleBufferedDataGridView gvStatusVariableList;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn1;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn2;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private Sys3Controls.Sys3button btnClearSelection;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel5;
        private Sys3Controls.Sys3Label sys3Label1;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel4;
        private Sys3Controls.Sys3DoubleBufferedDataGridView gvCollectionEvent;
        private System.Windows.Forms.DataGridViewTextBoxColumn ColID;
        private System.Windows.Forms.DataGridViewTextBoxColumn ColName;
        private Sys3Controls.Sys3Label sys3Label12;
    }
}
