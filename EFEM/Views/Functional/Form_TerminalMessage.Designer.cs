namespace FrameOfSystem3.Views.Functional
{
    partial class Form_TerminalMessage
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
            Sys3Controls.Sys3GroupBox groupBox_Title_;
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle3 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle4 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            this.btn_Close = new Sys3Controls.Sys3button();
            this.txtTerminalMessage = new System.Windows.Forms.TextBox();
            this.btn_Clear = new Sys3Controls.Sys3button();
            this.gvMessageList = new Sys3Controls.Sys3DoubleBufferedDataGridView();
            this.dataGridViewTextBoxColumn2 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            groupBox_Title_ = new Sys3Controls.Sys3GroupBox();
            ((System.ComponentModel.ISupportInitialize)(this.gvMessageList)).BeginInit();
            this.SuspendLayout();
            // 
            // groupBox_Title_
            // 
            groupBox_Title_.BackGroundColor = System.Drawing.Color.WhiteSmoke;
            groupBox_Title_.EdgeBorderStroke = 1;
            groupBox_Title_.EdgeRadius = 0;
            groupBox_Title_.LabelFont = new System.Drawing.Font("맑은 고딕", 10F, System.Drawing.FontStyle.Bold);
            groupBox_Title_.LabelGradientColorFirst = System.Drawing.Color.LightSteelBlue;
            groupBox_Title_.LabelGradientColorSecond = System.Drawing.Color.LightSteelBlue;
            groupBox_Title_.LabelHeight = 30;
            groupBox_Title_.LabelTextColor = System.Drawing.Color.Black;
            groupBox_Title_.Location = new System.Drawing.Point(231, 0);
            groupBox_Title_.Name = "groupBox_Title_";
            groupBox_Title_.Size = new System.Drawing.Size(431, 33);
            groupBox_Title_.TabIndex = 1006;
            groupBox_Title_.TabStop = false;
            groupBox_Title_.Text = "MESSAGE";
            groupBox_Title_.TextAlign = Sys3Controls.EN_TEXTALIGN.MIDDLE_CENTER;
            groupBox_Title_.ThemeIndex = 0;
            groupBox_Title_.UseLabelBorder = true;
            // 
            // btn_Close
            // 
            this.btn_Close.BorderWidth = 3;
            this.btn_Close.ButtonClicked = false;
            this.btn_Close.ClickedEmphasizeTextColor = System.Drawing.Color.White;
            this.btn_Close.CustomClickedGradientFirstColor = System.Drawing.Color.White;
            this.btn_Close.CustomClickedGradientSecondColor = System.Drawing.Color.White;
            this.btn_Close.Description = "";
            this.btn_Close.DisabledColor = System.Drawing.Color.DarkGray;
            this.btn_Close.EdgeRadius = 5;
            this.btn_Close.GradientAngle = 70F;
            this.btn_Close.GradientFirstColor = System.Drawing.Color.White;
            this.btn_Close.GradientSecondColor = System.Drawing.Color.LightSlateGray;
            this.btn_Close.HoverEmphasizeCustomColor = System.Drawing.Color.White;
            this.btn_Close.ImagePosition = new System.Drawing.Point(10, 10);
            this.btn_Close.ImageSize = new System.Drawing.Point(30, 30);
            this.btn_Close.LoadImage = global::FrameOfSystem3.Properties.Resources.CLEAR_BLACK;
            this.btn_Close.Location = new System.Drawing.Point(371, 367);
            this.btn_Close.MainFont = new System.Drawing.Font("맑은 고딕", 11F, System.Drawing.FontStyle.Bold);
            this.btn_Close.MainFontColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(36)))), ((int)(((byte)(0)))));
            this.btn_Close.Name = "btn_Close";
            this.btn_Close.Size = new System.Drawing.Size(184, 50);
            this.btn_Close.SubFont = new System.Drawing.Font("맑은 고딕", 8F, System.Drawing.FontStyle.Bold);
            this.btn_Close.SubFontColor = System.Drawing.Color.DarkBlue;
            this.btn_Close.SubText = "STATUS";
            this.btn_Close.TabIndex = 1000;
            this.btn_Close.Text = "CLOSE";
            this.btn_Close.TextAlignMain = Sys3Controls.EN_TEXTALIGN.MIDDLE_CENTER;
            this.btn_Close.TextAlignSub = Sys3Controls.EN_TEXTALIGN.TOP_LEFT;
            this.btn_Close.ThemeIndex = 0;
            this.btn_Close.UseBorder = true;
            this.btn_Close.UseClickedEmphasizeTextColor = false;
            this.btn_Close.UseCustomizeClickedColor = false;
            this.btn_Close.UseEdge = true;
            this.btn_Close.UseHoverEmphasizeCustomColor = false;
            this.btn_Close.UseImage = false;
            this.btn_Close.UserHoverEmpahsize = false;
            this.btn_Close.UseSubFont = false;
            this.btn_Close.Click += new System.EventHandler(this.Control_Click);
            // 
            // txtTerminalMessage
            // 
            this.txtTerminalMessage.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtTerminalMessage.Font = new System.Drawing.Font("맑은 고딕", 12F, System.Drawing.FontStyle.Bold);
            this.txtTerminalMessage.Location = new System.Drawing.Point(231, 33);
            this.txtTerminalMessage.Multiline = true;
            this.txtTerminalMessage.Name = "txtTerminalMessage";
            this.txtTerminalMessage.ReadOnly = true;
            this.txtTerminalMessage.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.txtTerminalMessage.Size = new System.Drawing.Size(431, 331);
            this.txtTerminalMessage.TabIndex = 1000;
            // 
            // btn_Clear
            // 
            this.btn_Clear.BorderWidth = 3;
            this.btn_Clear.ButtonClicked = false;
            this.btn_Clear.ClickedEmphasizeTextColor = System.Drawing.Color.White;
            this.btn_Clear.CustomClickedGradientFirstColor = System.Drawing.Color.White;
            this.btn_Clear.CustomClickedGradientSecondColor = System.Drawing.Color.White;
            this.btn_Clear.Description = "";
            this.btn_Clear.DisabledColor = System.Drawing.Color.DarkGray;
            this.btn_Clear.EdgeRadius = 5;
            this.btn_Clear.GradientAngle = 70F;
            this.btn_Clear.GradientFirstColor = System.Drawing.Color.White;
            this.btn_Clear.GradientSecondColor = System.Drawing.Color.LightSlateGray;
            this.btn_Clear.HoverEmphasizeCustomColor = System.Drawing.Color.White;
            this.btn_Clear.ImagePosition = new System.Drawing.Point(10, 10);
            this.btn_Clear.ImageSize = new System.Drawing.Point(30, 30);
            this.btn_Clear.LoadImage = global::FrameOfSystem3.Properties.Resources.CLEAR_BLACK;
            this.btn_Clear.Location = new System.Drawing.Point(109, 367);
            this.btn_Clear.MainFont = new System.Drawing.Font("맑은 고딕", 11F, System.Drawing.FontStyle.Bold);
            this.btn_Clear.MainFontColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(36)))), ((int)(((byte)(0)))));
            this.btn_Clear.Name = "btn_Clear";
            this.btn_Clear.Size = new System.Drawing.Size(184, 50);
            this.btn_Clear.SubFont = new System.Drawing.Font("맑은 고딕", 8F, System.Drawing.FontStyle.Bold);
            this.btn_Clear.SubFontColor = System.Drawing.Color.DarkBlue;
            this.btn_Clear.SubText = "STATUS";
            this.btn_Clear.TabIndex = 1000;
            this.btn_Clear.Text = "CLEAR ALL";
            this.btn_Clear.TextAlignMain = Sys3Controls.EN_TEXTALIGN.MIDDLE_CENTER;
            this.btn_Clear.TextAlignSub = Sys3Controls.EN_TEXTALIGN.TOP_LEFT;
            this.btn_Clear.ThemeIndex = 0;
            this.btn_Clear.UseBorder = true;
            this.btn_Clear.UseClickedEmphasizeTextColor = false;
            this.btn_Clear.UseCustomizeClickedColor = false;
            this.btn_Clear.UseEdge = true;
            this.btn_Clear.UseHoverEmphasizeCustomColor = false;
            this.btn_Clear.UseImage = false;
            this.btn_Clear.UserHoverEmpahsize = false;
            this.btn_Clear.UseSubFont = false;
            this.btn_Clear.Click += new System.EventHandler(this.Control_Click);
            // 
            // gvMessageList
            // 
            this.gvMessageList.AllowUserToAddRows = false;
            this.gvMessageList.AllowUserToDeleteRows = false;
            this.gvMessageList.AllowUserToResizeColumns = false;
            this.gvMessageList.AllowUserToResizeRows = false;
            this.gvMessageList.BackgroundColor = System.Drawing.Color.White;
            this.gvMessageList.CellBorderStyle = System.Windows.Forms.DataGridViewCellBorderStyle.Raised;
            this.gvMessageList.ColumnHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.Single;
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle1.BackColor = System.Drawing.Color.LightGray;
            dataGridViewCellStyle1.Font = new System.Drawing.Font("맑은 고딕", 11F, System.Drawing.FontStyle.Bold);
            dataGridViewCellStyle1.ForeColor = System.Drawing.Color.Black;
            dataGridViewCellStyle1.SelectionBackColor = System.Drawing.Color.White;
            dataGridViewCellStyle1.SelectionForeColor = System.Drawing.Color.Black;
            dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.gvMessageList.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
            this.gvMessageList.ColumnHeadersHeight = 30;
            this.gvMessageList.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            this.gvMessageList.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.dataGridViewTextBoxColumn2});
            dataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle3.BackColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle3.Font = new System.Drawing.Font("맑은 고딕", 10F, System.Drawing.FontStyle.Bold);
            dataGridViewCellStyle3.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle3.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(120)))), ((int)(((byte)(130)))), ((int)(((byte)(150)))));
            dataGridViewCellStyle3.SelectionForeColor = System.Drawing.Color.White;
            dataGridViewCellStyle3.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.gvMessageList.DefaultCellStyle = dataGridViewCellStyle3;
            this.gvMessageList.EnableHeadersVisualStyles = false;
            this.gvMessageList.GridColor = System.Drawing.Color.FromArgb(((int)(((byte)(120)))), ((int)(((byte)(130)))), ((int)(((byte)(150)))));
            this.gvMessageList.Location = new System.Drawing.Point(1, 2);
            this.gvMessageList.MultiSelect = false;
            this.gvMessageList.Name = "gvMessageList";
            this.gvMessageList.ReadOnly = true;
            this.gvMessageList.RowHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.None;
            dataGridViewCellStyle4.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle4.BackColor = System.Drawing.Color.White;
            dataGridViewCellStyle4.Font = new System.Drawing.Font("맑은 고딕", 11F);
            dataGridViewCellStyle4.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle4.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(120)))), ((int)(((byte)(130)))), ((int)(((byte)(150)))));
            dataGridViewCellStyle4.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle4.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.gvMessageList.RowHeadersDefaultCellStyle = dataGridViewCellStyle4;
            this.gvMessageList.RowHeadersVisible = false;
            this.gvMessageList.RowHeadersWidthSizeMode = System.Windows.Forms.DataGridViewRowHeadersWidthSizeMode.DisableResizing;
            this.gvMessageList.RowTemplate.Height = 23;
            this.gvMessageList.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.gvMessageList.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.gvMessageList.Size = new System.Drawing.Size(228, 362);
            this.gvMessageList.TabIndex = 1009;
            this.gvMessageList.CellClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.gvMessageListCellClicked);
            // 
            // dataGridViewTextBoxColumn2
            // 
            this.dataGridViewTextBoxColumn2.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
            this.dataGridViewTextBoxColumn2.DefaultCellStyle = dataGridViewCellStyle2;
            this.dataGridViewTextBoxColumn2.HeaderText = "TIME";
            this.dataGridViewTextBoxColumn2.Name = "dataGridViewTextBoxColumn2";
            this.dataGridViewTextBoxColumn2.ReadOnly = true;
            this.dataGridViewTextBoxColumn2.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.dataGridViewTextBoxColumn2.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // Form_TerminalMessage
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.ClientSize = new System.Drawing.Size(659, 417);
            this.ControlBox = false;
            this.Controls.Add(this.gvMessageList);
            this.Controls.Add(groupBox_Title_);
            this.Controls.Add(this.btn_Clear);
            this.Controls.Add(this.txtTerminalMessage);
            this.Controls.Add(this.btn_Close);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "Form_TerminalMessage";
            this.ShowIcon = false;
            this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
            this.Text = "Terminal Message";
            this.TopMost = true;
            ((System.ComponentModel.ISupportInitialize)(this.gvMessageList)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private Sys3Controls.Sys3button btn_Close;
        private System.Windows.Forms.TextBox txtTerminalMessage;
        private Sys3Controls.Sys3button btn_Clear;
        private Sys3Controls.Sys3DoubleBufferedDataGridView gvMessageList;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn2;
    }
}