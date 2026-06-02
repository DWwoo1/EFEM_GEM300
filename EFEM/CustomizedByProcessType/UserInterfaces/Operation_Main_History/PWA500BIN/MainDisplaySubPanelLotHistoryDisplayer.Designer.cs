
namespace EFEM.CustomizedByProcessType.UserInterface.OperationMainHistory.PWA500BIN
{
    partial class MainDisplaySubPanelLotHistoryDisplayer
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
            this.MONITORING = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.SOLUTIONCODE = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.PASSWORD = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.GRADE = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ID = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.INDEX = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.gvLotHistory = new Sys3Controls.Sys3DoubleBufferedDataGridView();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.lblSelectedMessage = new System.Windows.Forms.TextBox();
            this.dataGridViewTextBoxColumn1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn2 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn3 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn4 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn6 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            ((System.ComponentModel.ISupportInitialize)(this.gvLotHistory)).BeginInit();
            this.tableLayoutPanel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // MONITORING
            // 
            this.MONITORING.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.MONITORING.HeaderText = "MESSAGE";
            this.MONITORING.Name = "MONITORING";
            this.MONITORING.ReadOnly = true;
            this.MONITORING.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.MONITORING.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // SOLUTIONCODE
            // 
            this.SOLUTIONCODE.HeaderText = "SOLUTION";
            this.SOLUTIONCODE.Name = "SOLUTIONCODE";
            this.SOLUTIONCODE.ReadOnly = true;
            this.SOLUTIONCODE.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.SOLUTIONCODE.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.SOLUTIONCODE.Width = 113;
            // 
            // PASSWORD
            // 
            this.PASSWORD.HeaderText = "MESSAGE";
            this.PASSWORD.MaxInputLength = 20;
            this.PASSWORD.Name = "PASSWORD";
            this.PASSWORD.ReadOnly = true;
            this.PASSWORD.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.PASSWORD.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.PASSWORD.Width = 105;
            // 
            // GRADE
            // 
            this.GRADE.HeaderText = "GRADE";
            this.GRADE.Name = "GRADE";
            this.GRADE.ReadOnly = true;
            this.GRADE.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.GRADE.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // ID
            // 
            this.ID.HeaderText = "ALARM CODE";
            this.ID.MaxInputLength = 20;
            this.ID.Name = "ID";
            this.ID.ReadOnly = true;
            this.ID.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.ID.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.ID.Width = 140;
            // 
            // INDEX
            // 
            this.INDEX.HeaderText = "TIME";
            this.INDEX.Name = "INDEX";
            this.INDEX.ReadOnly = true;
            this.INDEX.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.INDEX.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.INDEX.Width = 85;
            // 
            // gvLotHistory
            // 
            this.gvLotHistory.AllowUserToAddRows = false;
            this.gvLotHistory.AllowUserToDeleteRows = false;
            this.gvLotHistory.AllowUserToResizeColumns = false;
            this.gvLotHistory.AllowUserToResizeRows = false;
            this.gvLotHistory.BackgroundColor = System.Drawing.Color.White;
            this.gvLotHistory.CellBorderStyle = System.Windows.Forms.DataGridViewCellBorderStyle.Raised;
            this.gvLotHistory.ColumnHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.Single;
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle1.BackColor = System.Drawing.Color.LightGray;
            dataGridViewCellStyle1.Font = new System.Drawing.Font("맑은 고딕", 8F, System.Drawing.FontStyle.Bold);
            dataGridViewCellStyle1.ForeColor = System.Drawing.Color.Black;
            dataGridViewCellStyle1.SelectionBackColor = System.Drawing.Color.White;
            dataGridViewCellStyle1.SelectionForeColor = System.Drawing.Color.Black;
            dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.gvLotHistory.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
            this.gvLotHistory.ColumnHeadersHeight = 30;
            this.gvLotHistory.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            this.gvLotHistory.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.dataGridViewTextBoxColumn1,
            this.dataGridViewTextBoxColumn2,
            this.dataGridViewTextBoxColumn3,
            this.dataGridViewTextBoxColumn4,
            this.dataGridViewTextBoxColumn6});
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle2.BackColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle2.Font = new System.Drawing.Font("맑은 고딕", 8F);
            dataGridViewCellStyle2.ForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle2.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(120)))), ((int)(((byte)(130)))), ((int)(((byte)(150)))));
            dataGridViewCellStyle2.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.gvLotHistory.DefaultCellStyle = dataGridViewCellStyle2;
            this.gvLotHistory.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gvLotHistory.EnableHeadersVisualStyles = false;
            this.gvLotHistory.GridColor = System.Drawing.Color.FromArgb(((int)(((byte)(120)))), ((int)(((byte)(130)))), ((int)(((byte)(150)))));
            this.gvLotHistory.Location = new System.Drawing.Point(3, 3);
            this.gvLotHistory.Margin = new System.Windows.Forms.Padding(3, 3, 3, 0);
            this.gvLotHistory.MultiSelect = false;
            this.gvLotHistory.Name = "gvLotHistory";
            this.gvLotHistory.ReadOnly = true;
            this.gvLotHistory.RowHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.None;
            dataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle3.BackColor = System.Drawing.Color.White;
            dataGridViewCellStyle3.Font = new System.Drawing.Font("맑은 고딕", 10F);
            dataGridViewCellStyle3.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle3.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(120)))), ((int)(((byte)(130)))), ((int)(((byte)(150)))));
            dataGridViewCellStyle3.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle3.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.gvLotHistory.RowHeadersDefaultCellStyle = dataGridViewCellStyle3;
            this.gvLotHistory.RowHeadersVisible = false;
            this.gvLotHistory.RowHeadersWidthSizeMode = System.Windows.Forms.DataGridViewRowHeadersWidthSizeMode.DisableResizing;
            this.gvLotHistory.RowTemplate.Height = 23;
            this.gvLotHistory.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.gvLotHistory.Size = new System.Drawing.Size(944, 294);
            this.gvLotHistory.TabIndex = 1259;
            this.gvLotHistory.CellClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.GvCellClicked);
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 1;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.Controls.Add(this.lblSelectedMessage, 0, 1);
            this.tableLayoutPanel1.Controls.Add(this.gvLotHistory, 0, 0);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 2;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 85F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 15F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(950, 350);
            this.tableLayoutPanel1.TabIndex = 1260;
            // 
            // lblSelectedMessage
            // 
            this.lblSelectedMessage.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lblSelectedMessage.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblSelectedMessage.Font = new System.Drawing.Font("맑은 고딕", 8F);
            this.lblSelectedMessage.Location = new System.Drawing.Point(3, 300);
            this.lblSelectedMessage.Multiline = true;
            this.lblSelectedMessage.Name = "lblSelectedMessage";
            this.lblSelectedMessage.ReadOnly = true;
            this.lblSelectedMessage.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.lblSelectedMessage.Size = new System.Drawing.Size(944, 47);
            this.lblSelectedMessage.TabIndex = 10408;
            this.lblSelectedMessage.Text = " ";
            // 
            // dataGridViewTextBoxColumn1
            // 
            this.dataGridViewTextBoxColumn1.HeaderText = "TIME";
            this.dataGridViewTextBoxColumn1.Name = "dataGridViewTextBoxColumn1";
            this.dataGridViewTextBoxColumn1.ReadOnly = true;
            this.dataGridViewTextBoxColumn1.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.dataGridViewTextBoxColumn1.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.dataGridViewTextBoxColumn1.Width = 110;
            // 
            // dataGridViewTextBoxColumn2
            // 
            this.dataGridViewTextBoxColumn2.HeaderText = "PORT EVENT";
            this.dataGridViewTextBoxColumn2.MaxInputLength = 20;
            this.dataGridViewTextBoxColumn2.Name = "dataGridViewTextBoxColumn2";
            this.dataGridViewTextBoxColumn2.ReadOnly = true;
            this.dataGridViewTextBoxColumn2.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.dataGridViewTextBoxColumn2.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.dataGridViewTextBoxColumn2.Width = 80;
            // 
            // dataGridViewTextBoxColumn3
            // 
            this.dataGridViewTextBoxColumn3.HeaderText = "WAFER";
            this.dataGridViewTextBoxColumn3.Name = "dataGridViewTextBoxColumn3";
            this.dataGridViewTextBoxColumn3.ReadOnly = true;
            this.dataGridViewTextBoxColumn3.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.dataGridViewTextBoxColumn3.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.dataGridViewTextBoxColumn3.Width = 85;
            // 
            // dataGridViewTextBoxColumn4
            // 
            this.dataGridViewTextBoxColumn4.HeaderText = "WAFER EVENT";
            this.dataGridViewTextBoxColumn4.MaxInputLength = 20;
            this.dataGridViewTextBoxColumn4.Name = "dataGridViewTextBoxColumn4";
            this.dataGridViewTextBoxColumn4.ReadOnly = true;
            this.dataGridViewTextBoxColumn4.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.dataGridViewTextBoxColumn4.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.dataGridViewTextBoxColumn4.Width = 90;
            // 
            // dataGridViewTextBoxColumn6
            // 
            this.dataGridViewTextBoxColumn6.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.dataGridViewTextBoxColumn6.HeaderText = "MESSAGE";
            this.dataGridViewTextBoxColumn6.Name = "dataGridViewTextBoxColumn6";
            this.dataGridViewTextBoxColumn6.ReadOnly = true;
            this.dataGridViewTextBoxColumn6.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.dataGridViewTextBoxColumn6.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // MainDisplaySubPanelLotHistoryDisplayer
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.BackColor = System.Drawing.SystemColors.Control;
            this.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.Controls.Add(this.tableLayoutPanel1);
            this.Margin = new System.Windows.Forms.Padding(0);
            this.Name = "MainDisplaySubPanelLotHistoryDisplayer";
            this.Size = new System.Drawing.Size(950, 350);
            ((System.ComponentModel.ISupportInitialize)(this.gvLotHistory)).EndInit();
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.DataGridViewTextBoxColumn INDEX;
        private System.Windows.Forms.DataGridViewTextBoxColumn ID;
        private System.Windows.Forms.DataGridViewTextBoxColumn GRADE;
        private System.Windows.Forms.DataGridViewTextBoxColumn PASSWORD;
        private System.Windows.Forms.DataGridViewTextBoxColumn SOLUTIONCODE;
        private System.Windows.Forms.DataGridViewTextBoxColumn MONITORING;
        private Sys3Controls.Sys3DoubleBufferedDataGridView gvLotHistory;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.TextBox lblSelectedMessage;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn1;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn2;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn3;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn4;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn6;
    }
}
