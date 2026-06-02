namespace FrameOfSystem3.Views.Config.Analog
{
    partial class Config_AnalogItemDisplay
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
            this.m_dgView = new Sys3Controls.Sys3DoubleBufferedDataGridView();
            this.ID = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ENABLE = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.PASSWORD = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.TAGID = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.MONITORING = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.BIT = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.DATA_TYPE = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.AUTHORITY = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.CONDITION = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column2 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.BLINK = new System.Windows.Forms.DataGridViewTextBoxColumn();
            ((System.ComponentModel.ISupportInitialize)(this.m_dgView)).BeginInit();
            this.SuspendLayout();
            // 
            // m_dgView
            // 
            this.m_dgView.AllowUserToAddRows = false;
            this.m_dgView.AllowUserToDeleteRows = false;
            this.m_dgView.AllowUserToResizeColumns = false;
            this.m_dgView.AllowUserToResizeRows = false;
            this.m_dgView.BackgroundColor = System.Drawing.Color.White;
            this.m_dgView.CellBorderStyle = System.Windows.Forms.DataGridViewCellBorderStyle.Raised;
            this.m_dgView.ColumnHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.Single;
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle1.BackColor = System.Drawing.Color.LightGray;
            dataGridViewCellStyle1.Font = new System.Drawing.Font("맑은 고딕", 11F, System.Drawing.FontStyle.Bold);
            dataGridViewCellStyle1.ForeColor = System.Drawing.Color.Black;
            dataGridViewCellStyle1.SelectionBackColor = System.Drawing.Color.White;
            dataGridViewCellStyle1.SelectionForeColor = System.Drawing.Color.Black;
            dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.m_dgView.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
            this.m_dgView.ColumnHeadersHeight = 45;
            this.m_dgView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            this.m_dgView.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.ID,
            this.ENABLE,
            this.PASSWORD,
            this.TAGID,
            this.MONITORING,
            this.BIT,
            this.DATA_TYPE,
            this.AUTHORITY,
            this.CONDITION,
            this.Column2,
            this.BLINK});
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle2.BackColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle2.Font = new System.Drawing.Font("맑은 고딕", 10F, System.Drawing.FontStyle.Bold);
            dataGridViewCellStyle2.ForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle2.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(120)))), ((int)(((byte)(130)))), ((int)(((byte)(150)))));
            dataGridViewCellStyle2.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.m_dgView.DefaultCellStyle = dataGridViewCellStyle2;
            this.m_dgView.Dock = System.Windows.Forms.DockStyle.Fill;
            this.m_dgView.EnableHeadersVisualStyles = false;
            this.m_dgView.GridColor = System.Drawing.Color.FromArgb(((int)(((byte)(120)))), ((int)(((byte)(130)))), ((int)(((byte)(150)))));
            this.m_dgView.Location = new System.Drawing.Point(0, 0);
            this.m_dgView.MultiSelect = false;
            this.m_dgView.Name = "m_dgView";
            this.m_dgView.ReadOnly = true;
            this.m_dgView.RowHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.None;
            dataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle3.BackColor = System.Drawing.Color.White;
            dataGridViewCellStyle3.Font = new System.Drawing.Font("맑은 고딕", 11F);
            dataGridViewCellStyle3.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle3.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(120)))), ((int)(((byte)(130)))), ((int)(((byte)(150)))));
            dataGridViewCellStyle3.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle3.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.m_dgView.RowHeadersDefaultCellStyle = dataGridViewCellStyle3;
            this.m_dgView.RowHeadersVisible = false;
            this.m_dgView.RowHeadersWidthSizeMode = System.Windows.Forms.DataGridViewRowHeadersWidthSizeMode.DisableResizing;
            this.m_dgView.RowTemplate.Height = 23;
            this.m_dgView.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.m_dgView.Size = new System.Drawing.Size(915, 328);
            this.m_dgView.TabIndex = 1107;
            // 
            // ID
            // 
            this.ID.HeaderText = "";
            this.ID.MaxInputLength = 20;
            this.ID.Name = "ID";
            this.ID.ReadOnly = true;
            this.ID.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.ID.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.ID.Width = 35;
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
            // PASSWORD
            // 
            this.PASSWORD.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.PASSWORD.HeaderText = "NAME";
            this.PASSWORD.MaxInputLength = 20;
            this.PASSWORD.Name = "PASSWORD";
            this.PASSWORD.ReadOnly = true;
            this.PASSWORD.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.PASSWORD.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // TAGID
            // 
            this.TAGID.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            this.TAGID.HeaderText = "TAG NUMBER";
            this.TAGID.Name = "TAGID";
            this.TAGID.ReadOnly = true;
            this.TAGID.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.TAGID.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.TAGID.Width = 80;
            // 
            // MONITORING
            // 
            this.MONITORING.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.ColumnHeader;
            this.MONITORING.HeaderText = "TARGET";
            this.MONITORING.Name = "MONITORING";
            this.MONITORING.ReadOnly = true;
            this.MONITORING.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.MONITORING.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.MONITORING.Width = 72;
            // 
            // BIT
            // 
            this.BIT.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.ColumnHeader;
            this.BIT.HeaderText = "BIT";
            this.BIT.Name = "BIT";
            this.BIT.ReadOnly = true;
            this.BIT.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.BIT.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.BIT.Width = 38;
            // 
            // DATA_TYPE
            // 
            this.DATA_TYPE.HeaderText = "DATA TYPE";
            this.DATA_TYPE.MaxInputLength = 120;
            this.DATA_TYPE.Name = "DATA_TYPE";
            this.DATA_TYPE.ReadOnly = true;
            this.DATA_TYPE.Width = 65;
            // 
            // AUTHORITY
            // 
            this.AUTHORITY.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.ColumnHeader;
            this.AUTHORITY.HeaderText = "MIN";
            this.AUTHORITY.MaxInputLength = 20;
            this.AUTHORITY.Name = "AUTHORITY";
            this.AUTHORITY.ReadOnly = true;
            this.AUTHORITY.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.AUTHORITY.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.AUTHORITY.Width = 45;
            // 
            // CONDITION
            // 
            this.CONDITION.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.ColumnHeader;
            this.CONDITION.HeaderText = "MAX";
            this.CONDITION.Name = "CONDITION";
            this.CONDITION.ReadOnly = true;
            this.CONDITION.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.CONDITION.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.CONDITION.Width = 49;
            // 
            // Column2
            // 
            this.Column2.HeaderText = "VOLT";
            this.Column2.Name = "Column2";
            this.Column2.ReadOnly = true;
            this.Column2.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.Column2.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.Column2.Width = 62;
            // 
            // BLINK
            // 
            this.BLINK.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.ColumnHeader;
            this.BLINK.HeaderText = "VALUE";
            this.BLINK.Name = "BLINK";
            this.BLINK.ReadOnly = true;
            this.BLINK.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.BLINK.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.BLINK.Width = 62;
            // 
            // Config_AnalogItemDisplay
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.BackColor = System.Drawing.Color.White;
            this.Controls.Add(this.m_dgView);
            this.Name = "Config_AnalogItemDisplay";
            this.Size = new System.Drawing.Size(915, 328);
            ((System.ComponentModel.ISupportInitialize)(this.m_dgView)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private Sys3Controls.Sys3DoubleBufferedDataGridView m_dgView;
        private System.Windows.Forms.DataGridViewTextBoxColumn ID;
        private System.Windows.Forms.DataGridViewTextBoxColumn ENABLE;
        private System.Windows.Forms.DataGridViewTextBoxColumn PASSWORD;
        private System.Windows.Forms.DataGridViewTextBoxColumn TAGID;
        private System.Windows.Forms.DataGridViewTextBoxColumn MONITORING;
        private System.Windows.Forms.DataGridViewTextBoxColumn BIT;
        private System.Windows.Forms.DataGridViewTextBoxColumn DATA_TYPE;
        private System.Windows.Forms.DataGridViewTextBoxColumn AUTHORITY;
        private System.Windows.Forms.DataGridViewTextBoxColumn CONDITION;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column2;
        private System.Windows.Forms.DataGridViewTextBoxColumn BLINK;
    }
}
