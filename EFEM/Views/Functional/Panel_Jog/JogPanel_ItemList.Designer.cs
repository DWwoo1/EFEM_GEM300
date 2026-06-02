namespace FrameOfSystem3.Views.Functional.Jog
{
	partial class JogPanel_ItemList
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
			this.grid_ItemList = new Sys3Controls.Sys3DoubleBufferedDataGridView();
			this.ID = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.PASSWORD = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.Column1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.Column2 = new System.Windows.Forms.DataGridViewTextBoxColumn();
			((System.ComponentModel.ISupportInitialize)(this.grid_ItemList)).BeginInit();
			this.SuspendLayout();
			// 
			// grid_ItemList
			// 
			this.grid_ItemList.AllowUserToAddRows = false;
			this.grid_ItemList.AllowUserToDeleteRows = false;
			this.grid_ItemList.AllowUserToResizeColumns = false;
			this.grid_ItemList.AllowUserToResizeRows = false;
			this.grid_ItemList.BackgroundColor = System.Drawing.Color.White;
			this.grid_ItemList.CellBorderStyle = System.Windows.Forms.DataGridViewCellBorderStyle.Raised;
			this.grid_ItemList.ColumnHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.Single;
			dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
			dataGridViewCellStyle1.BackColor = System.Drawing.Color.LightGray;
			dataGridViewCellStyle1.Font = new System.Drawing.Font("맑은 고딕", 11F, System.Drawing.FontStyle.Bold);
			dataGridViewCellStyle1.ForeColor = System.Drawing.Color.Black;
			dataGridViewCellStyle1.SelectionBackColor = System.Drawing.Color.White;
			dataGridViewCellStyle1.SelectionForeColor = System.Drawing.Color.Black;
			dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
			this.grid_ItemList.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
			this.grid_ItemList.ColumnHeadersHeight = 25;
			this.grid_ItemList.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
			this.grid_ItemList.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.ID,
            this.PASSWORD,
            this.Column1,
            this.Column2});
			dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
			dataGridViewCellStyle2.BackColor = System.Drawing.SystemColors.Window;
			dataGridViewCellStyle2.Font = new System.Drawing.Font("맑은 고딕", 10F, System.Drawing.FontStyle.Bold);
			dataGridViewCellStyle2.ForeColor = System.Drawing.SystemColors.ControlText;
			dataGridViewCellStyle2.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(120)))), ((int)(((byte)(130)))), ((int)(((byte)(150)))));
			dataGridViewCellStyle2.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
			dataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
			this.grid_ItemList.DefaultCellStyle = dataGridViewCellStyle2;
			this.grid_ItemList.Dock = System.Windows.Forms.DockStyle.Fill;
			this.grid_ItemList.EnableHeadersVisualStyles = false;
			this.grid_ItemList.GridColor = System.Drawing.Color.FromArgb(((int)(((byte)(120)))), ((int)(((byte)(130)))), ((int)(((byte)(150)))));
			this.grid_ItemList.Location = new System.Drawing.Point(1, 1);
			this.grid_ItemList.MultiSelect = false;
			this.grid_ItemList.Name = "grid_ItemList";
			this.grid_ItemList.ReadOnly = true;
			this.grid_ItemList.RowHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.None;
			dataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
			dataGridViewCellStyle3.BackColor = System.Drawing.Color.White;
			dataGridViewCellStyle3.Font = new System.Drawing.Font("맑은 고딕", 11F);
			dataGridViewCellStyle3.ForeColor = System.Drawing.SystemColors.WindowText;
			dataGridViewCellStyle3.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(120)))), ((int)(((byte)(130)))), ((int)(((byte)(150)))));
			dataGridViewCellStyle3.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
			dataGridViewCellStyle3.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
			this.grid_ItemList.RowHeadersDefaultCellStyle = dataGridViewCellStyle3;
			this.grid_ItemList.RowHeadersVisible = false;
			this.grid_ItemList.RowHeadersWidthSizeMode = System.Windows.Forms.DataGridViewRowHeadersWidthSizeMode.DisableResizing;
			this.grid_ItemList.RowTemplate.Height = 25;
			this.grid_ItemList.RowTemplate.ReadOnly = true;
			this.grid_ItemList.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
			this.grid_ItemList.Size = new System.Drawing.Size(398, 472);
			this.grid_ItemList.TabIndex = 1190;
			this.grid_ItemList.CellClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.Click_Cell);
			this.grid_ItemList.KeyDown += new System.Windows.Forms.KeyEventHandler(this.KeyDown_Grid);
			// 
			// ID
			// 
			this.ID.Frozen = true;
			this.ID.HeaderText = "No.";
			this.ID.MaxInputLength = 20;
			this.ID.Name = "ID";
			this.ID.ReadOnly = true;
			this.ID.Resizable = System.Windows.Forms.DataGridViewTriState.False;
			this.ID.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
			this.ID.Width = 35;
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
			// Column1
			// 
			this.Column1.HeaderText = "X";
			this.Column1.Name = "Column1";
			this.Column1.ReadOnly = true;
			this.Column1.Resizable = System.Windows.Forms.DataGridViewTriState.False;
			this.Column1.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
			this.Column1.Width = 70;
			// 
			// Column2
			// 
			this.Column2.HeaderText = "Y";
			this.Column2.Name = "Column2";
			this.Column2.ReadOnly = true;
			this.Column2.Resizable = System.Windows.Forms.DataGridViewTriState.False;
			this.Column2.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
			this.Column2.Width = 70;
			// 
			// JogPanel_ItemList
			// 
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
			this.Controls.Add(this.grid_ItemList);
			this.Margin = new System.Windows.Forms.Padding(1, 1, 0, 0);
			this.Name = "JogPanel_ItemList";
			this.Padding = new System.Windows.Forms.Padding(1);
			this.Size = new System.Drawing.Size(400, 474);
			this.Tag = "";
			((System.ComponentModel.ISupportInitialize)(this.grid_ItemList)).EndInit();
			this.ResumeLayout(false);

		}

		#endregion

		private Sys3Controls.Sys3DoubleBufferedDataGridView grid_ItemList;
		private System.Windows.Forms.DataGridViewTextBoxColumn ID;
		private System.Windows.Forms.DataGridViewTextBoxColumn PASSWORD;
		private System.Windows.Forms.DataGridViewTextBoxColumn Column1;
		private System.Windows.Forms.DataGridViewTextBoxColumn Column2;






	}
}
