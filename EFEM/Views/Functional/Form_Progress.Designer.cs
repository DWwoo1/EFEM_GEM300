namespace FrameOfSystem3.Views.Functional
{
    partial class Form_Progress
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
			System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
			System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle3 = new System.Windows.Forms.DataGridViewCellStyle();
			System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle4 = new System.Windows.Forms.DataGridViewCellStyle();
			System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
			this.m_dgViewProGress = new Sys3Controls.Sys3DoubleBufferedDataGridView();
			this.ID = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.ENABLE = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.m_groupTitle = new Sys3Controls.Sys3GroupBox();
			this.m_btnConfirm = new Sys3Controls.Sys3button();
			((System.ComponentModel.ISupportInitialize)(this.m_dgViewProGress)).BeginInit();
			this.SuspendLayout();
			// 
			// m_dgViewProGress
			// 
			this.m_dgViewProGress.AllowUserToAddRows = false;
			this.m_dgViewProGress.AllowUserToDeleteRows = false;
			this.m_dgViewProGress.AllowUserToResizeColumns = false;
			this.m_dgViewProGress.AllowUserToResizeRows = false;
			this.m_dgViewProGress.BackgroundColor = System.Drawing.Color.White;
			this.m_dgViewProGress.BorderStyle = System.Windows.Forms.BorderStyle.None;
			this.m_dgViewProGress.CellBorderStyle = System.Windows.Forms.DataGridViewCellBorderStyle.None;
			this.m_dgViewProGress.ColumnHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.Single;
			dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
			dataGridViewCellStyle1.BackColor = System.Drawing.Color.LightGray;
			dataGridViewCellStyle1.Font = new System.Drawing.Font("맑은 고딕", 11F, System.Drawing.FontStyle.Bold);
			dataGridViewCellStyle1.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(60)))), ((int)(((byte)(60)))), ((int)(((byte)(60)))));
			dataGridViewCellStyle1.SelectionBackColor = System.Drawing.Color.White;
			dataGridViewCellStyle1.SelectionForeColor = System.Drawing.Color.Black;
			dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
			this.m_dgViewProGress.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
			this.m_dgViewProGress.ColumnHeadersHeight = 30;
			this.m_dgViewProGress.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
			this.m_dgViewProGress.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.ID,
            this.ENABLE});
			dataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
			dataGridViewCellStyle3.BackColor = System.Drawing.SystemColors.Window;
			dataGridViewCellStyle3.Font = new System.Drawing.Font("맑은 고딕", 10F, System.Drawing.FontStyle.Bold);
			dataGridViewCellStyle3.ForeColor = System.Drawing.SystemColors.WindowText;
			dataGridViewCellStyle3.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(180)))), ((int)(((byte)(190)))), ((int)(((byte)(210)))));
			dataGridViewCellStyle3.SelectionForeColor = System.Drawing.Color.White;
			dataGridViewCellStyle3.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
			this.m_dgViewProGress.DefaultCellStyle = dataGridViewCellStyle3;
			this.m_dgViewProGress.EnableHeadersVisualStyles = false;
			this.m_dgViewProGress.GridColor = System.Drawing.Color.White;
			this.m_dgViewProGress.Location = new System.Drawing.Point(12, 82);
			this.m_dgViewProGress.MultiSelect = false;
			this.m_dgViewProGress.Name = "m_dgViewProGress";
			this.m_dgViewProGress.ReadOnly = true;
			this.m_dgViewProGress.RowHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.None;
			dataGridViewCellStyle4.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
			dataGridViewCellStyle4.BackColor = System.Drawing.Color.White;
			dataGridViewCellStyle4.Font = new System.Drawing.Font("맑은 고딕", 11F);
			dataGridViewCellStyle4.ForeColor = System.Drawing.SystemColors.WindowText;
			dataGridViewCellStyle4.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(120)))), ((int)(((byte)(130)))), ((int)(((byte)(150)))));
			dataGridViewCellStyle4.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
			dataGridViewCellStyle4.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
			this.m_dgViewProGress.RowHeadersDefaultCellStyle = dataGridViewCellStyle4;
			this.m_dgViewProGress.RowHeadersVisible = false;
			this.m_dgViewProGress.RowHeadersWidthSizeMode = System.Windows.Forms.DataGridViewRowHeadersWidthSizeMode.DisableResizing;
			this.m_dgViewProGress.RowTemplate.Height = 20;
			this.m_dgViewProGress.RowTemplate.ReadOnly = true;
			this.m_dgViewProGress.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
			this.m_dgViewProGress.Size = new System.Drawing.Size(660, 98);
			this.m_dgViewProGress.TabIndex = 1125;
			// 
			// ID
			// 
			this.ID.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
			dataGridViewCellStyle2.Font = new System.Drawing.Font("맑은 고딕", 12.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
			this.ID.DefaultCellStyle = dataGridViewCellStyle2;
			this.ID.HeaderText = "CONTENTS";
			this.ID.Name = "ID";
			this.ID.ReadOnly = true;
			this.ID.Resizable = System.Windows.Forms.DataGridViewTriState.False;
			this.ID.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
			// 
			// ENABLE
			// 
			this.ENABLE.HeaderText = "RESULT";
			this.ENABLE.Name = "ENABLE";
			this.ENABLE.ReadOnly = true;
			this.ENABLE.Resizable = System.Windows.Forms.DataGridViewTriState.False;
			this.ENABLE.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
			this.ENABLE.Width = 75;
			// 
			// m_groupTitle
			// 
			this.m_groupTitle.BackGroundColor = System.Drawing.Color.WhiteSmoke;
			this.m_groupTitle.EdgeBorderStroke = 2;
			this.m_groupTitle.EdgeRadius = 2;
			this.m_groupTitle.LabelFont = new System.Drawing.Font("맑은 고딕", 30F, System.Drawing.FontStyle.Bold);
			this.m_groupTitle.LabelGradientColorFirst = System.Drawing.Color.FromArgb(((int)(((byte)(208)))), ((int)(((byte)(208)))), ((int)(((byte)(208)))));
			this.m_groupTitle.LabelGradientColorSecond = System.Drawing.Color.FromArgb(((int)(((byte)(240)))), ((int)(((byte)(240)))), ((int)(((byte)(240)))));
			this.m_groupTitle.LabelHeight = 70;
			this.m_groupTitle.LabelTextColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(36)))), ((int)(((byte)(0)))));
			this.m_groupTitle.Location = new System.Drawing.Point(0, 0);
			this.m_groupTitle.Name = "m_groupTitle";
			this.m_groupTitle.Size = new System.Drawing.Size(693, 256);
			this.m_groupTitle.TabIndex = 1400;
			this.m_groupTitle.Text = "Protec";
			this.m_groupTitle.TextAlign = Sys3Controls.EN_TEXTALIGN.MIDDLE_CENTER;
			this.m_groupTitle.ThemeIndex = 0;
			this.m_groupTitle.UseLabelBorder = true;
			// 
			// m_btnConfirm
			// 
			this.m_btnConfirm.BorderWidth = 3;
			this.m_btnConfirm.ButtonClicked = false;
			this.m_btnConfirm.ClickedEmphasizeTextColor = System.Drawing.Color.White;
			this.m_btnConfirm.CustomClickedGradientFirstColor = System.Drawing.Color.White;
			this.m_btnConfirm.CustomClickedGradientSecondColor = System.Drawing.Color.White;
			this.m_btnConfirm.Description = "";
			this.m_btnConfirm.DisabledColor = System.Drawing.Color.DarkGray;
			this.m_btnConfirm.EdgeRadius = 5;
			this.m_btnConfirm.GradientAngle = 70F;
			this.m_btnConfirm.GradientFirstColor = System.Drawing.Color.White;
			this.m_btnConfirm.GradientSecondColor = System.Drawing.Color.FromArgb(((int)(((byte)(170)))), ((int)(((byte)(176)))), ((int)(((byte)(183)))));
			this.m_btnConfirm.HoverEmphasizeCustomColor = System.Drawing.Color.White;
			this.m_btnConfirm.ImagePosition = new System.Drawing.Point(10, 10);
			this.m_btnConfirm.ImageSize = new System.Drawing.Point(30, 30);
			this.m_btnConfirm.LoadImage = global::FrameOfSystem3.Properties.Resources.CLEAR_BLACK;
			this.m_btnConfirm.Location = new System.Drawing.Point(393, 192);
			this.m_btnConfirm.MainFont = new System.Drawing.Font("맑은 고딕", 12F, System.Drawing.FontStyle.Bold);
			this.m_btnConfirm.MainFontColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(36)))), ((int)(((byte)(0)))));
			this.m_btnConfirm.Name = "m_btnConfirm";
			this.m_btnConfirm.Size = new System.Drawing.Size(242, 50);
			this.m_btnConfirm.SubFont = new System.Drawing.Font("맑은 고딕", 8F, System.Drawing.FontStyle.Bold);
			this.m_btnConfirm.SubFontColor = System.Drawing.Color.DarkBlue;
			this.m_btnConfirm.SubText = "STATUS";
			this.m_btnConfirm.TabIndex = 1401;
			this.m_btnConfirm.Text = "CONFIRMATION";
			this.m_btnConfirm.TextAlignMain = Sys3Controls.EN_TEXTALIGN.MIDDLE_CENTER;
			this.m_btnConfirm.TextAlignSub = Sys3Controls.EN_TEXTALIGN.TOP_LEFT;
			this.m_btnConfirm.ThemeIndex = 0;
			this.m_btnConfirm.UseBorder = true;
			this.m_btnConfirm.UseClickedEmphasizeTextColor = false;
			this.m_btnConfirm.UseCustomizeClickedColor = false;
			this.m_btnConfirm.UseEdge = true;
			this.m_btnConfirm.UseHoverEmphasizeCustomColor = false;
			this.m_btnConfirm.UseImage = false;
			this.m_btnConfirm.UserHoverEmpahsize = false;
			this.m_btnConfirm.UseSubFont = false;
			this.m_btnConfirm.Visible = false;
			this.m_btnConfirm.Click += new System.EventHandler(this.m_btnConfirm_Click);
			// 
			// Form_Progress
			// 
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
			this.ClientSize = new System.Drawing.Size(693, 254);
			this.ControlBox = false;
			this.Controls.Add(this.m_btnConfirm);
			this.Controls.Add(this.m_dgViewProGress);
			this.Controls.Add(this.m_groupTitle);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.Name = "Form_Progress";
			this.ShowIcon = false;
			this.ShowInTaskbar = false;
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
			this.Tag = "0";
			this.Text = "Form_Progress";
			this.TopMost = true;
			((System.ComponentModel.ISupportInitialize)(this.m_dgViewProGress)).EndInit();
			this.ResumeLayout(false);

        }

        #endregion

		private Sys3Controls.Sys3DoubleBufferedDataGridView m_dgViewProGress;
        private System.Windows.Forms.DataGridViewTextBoxColumn ID;
        private System.Windows.Forms.DataGridViewTextBoxColumn ENABLE;
		private Sys3Controls.Sys3GroupBox m_groupTitle;
		private Sys3Controls.Sys3button m_btnConfirm;
    }
}