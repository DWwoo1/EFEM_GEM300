namespace FrameOfSystem3.Views.Operation
{
	partial class Operation_History
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
            this.panel1 = new System.Windows.Forms.Panel();
            this.btnSubViewHistory = new Sys3Controls.Sys3button();
            this.btnSubViewCurrentWorking = new Sys3Controls.Sys3button();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.panel1.SuspendLayout();
            this.tableLayoutPanel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.btnSubViewHistory);
            this.panel1.Controls.Add(this.btnSubViewCurrentWorking);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel1.Location = new System.Drawing.Point(3, 3);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(1134, 39);
            this.panel1.TabIndex = 3;
            // 
            // btnSubViewHistory
            // 
            this.btnSubViewHistory.BorderWidth = 2;
            this.btnSubViewHistory.ButtonClicked = false;
            this.btnSubViewHistory.ClickedEmphasizeTextColor = System.Drawing.Color.White;
            this.btnSubViewHistory.CustomClickedGradientFirstColor = System.Drawing.Color.DarkBlue;
            this.btnSubViewHistory.CustomClickedGradientSecondColor = System.Drawing.Color.DarkBlue;
            this.btnSubViewHistory.Description = "";
            this.btnSubViewHistory.DisabledColor = System.Drawing.Color.DarkGray;
            this.btnSubViewHistory.Dock = System.Windows.Forms.DockStyle.Left;
            this.btnSubViewHistory.EdgeRadius = 5;
            this.btnSubViewHistory.GradientAngle = 70F;
            this.btnSubViewHistory.GradientFirstColor = System.Drawing.Color.White;
            this.btnSubViewHistory.GradientSecondColor = System.Drawing.Color.White;
            this.btnSubViewHistory.HoverEmphasizeCustomColor = System.Drawing.Color.White;
            this.btnSubViewHistory.ImagePosition = new System.Drawing.Point(7, 7);
            this.btnSubViewHistory.ImageSize = new System.Drawing.Point(30, 30);
            this.btnSubViewHistory.LoadImage = global::FrameOfSystem3.Properties.Resources.Home_black;
            this.btnSubViewHistory.Location = new System.Drawing.Point(239, 0);
            this.btnSubViewHistory.MainFont = new System.Drawing.Font("맑은 고딕", 14.25F, System.Drawing.FontStyle.Bold);
            this.btnSubViewHistory.MainFontColor = System.Drawing.Color.DarkBlue;
            this.btnSubViewHistory.Margin = new System.Windows.Forms.Padding(0);
            this.btnSubViewHistory.Name = "btnSubViewHistory";
            this.btnSubViewHistory.Size = new System.Drawing.Size(239, 39);
            this.btnSubViewHistory.SubFont = new System.Drawing.Font("맑은 고딕", 8F, System.Drawing.FontStyle.Bold);
            this.btnSubViewHistory.SubFontColor = System.Drawing.Color.DarkBlue;
            this.btnSubViewHistory.SubText = "STATUS";
            this.btnSubViewHistory.TabIndex = 10414;
            this.btnSubViewHistory.Text = "HISTORY";
            this.btnSubViewHistory.TextAlignMain = Sys3Controls.EN_TEXTALIGN.MIDDLE_CENTER;
            this.btnSubViewHistory.TextAlignSub = Sys3Controls.EN_TEXTALIGN.TOP_RIGHT;
            this.btnSubViewHistory.ThemeIndex = 0;
            this.btnSubViewHistory.UseBorder = false;
            this.btnSubViewHistory.UseClickedEmphasizeTextColor = false;
            this.btnSubViewHistory.UseCustomizeClickedColor = true;
            this.btnSubViewHistory.UseEdge = false;
            this.btnSubViewHistory.UseHoverEmphasizeCustomColor = false;
            this.btnSubViewHistory.UseImage = false;
            this.btnSubViewHistory.UserHoverEmpahsize = false;
            this.btnSubViewHistory.UseSubFont = false;
            this.btnSubViewHistory.Click += new System.EventHandler(this.BtnSubViewClicked);
            // 
            // btnSubViewCurrentWorking
            // 
            this.btnSubViewCurrentWorking.BorderWidth = 2;
            this.btnSubViewCurrentWorking.ButtonClicked = true;
            this.btnSubViewCurrentWorking.ClickedEmphasizeTextColor = System.Drawing.Color.White;
            this.btnSubViewCurrentWorking.CustomClickedGradientFirstColor = System.Drawing.Color.DarkBlue;
            this.btnSubViewCurrentWorking.CustomClickedGradientSecondColor = System.Drawing.Color.DarkBlue;
            this.btnSubViewCurrentWorking.Description = "";
            this.btnSubViewCurrentWorking.DisabledColor = System.Drawing.Color.DarkGray;
            this.btnSubViewCurrentWorking.Dock = System.Windows.Forms.DockStyle.Left;
            this.btnSubViewCurrentWorking.EdgeRadius = 5;
            this.btnSubViewCurrentWorking.GradientAngle = 70F;
            this.btnSubViewCurrentWorking.GradientFirstColor = System.Drawing.Color.White;
            this.btnSubViewCurrentWorking.GradientSecondColor = System.Drawing.Color.White;
            this.btnSubViewCurrentWorking.HoverEmphasizeCustomColor = System.Drawing.Color.White;
            this.btnSubViewCurrentWorking.ImagePosition = new System.Drawing.Point(7, 7);
            this.btnSubViewCurrentWorking.ImageSize = new System.Drawing.Point(30, 30);
            this.btnSubViewCurrentWorking.LoadImage = global::FrameOfSystem3.Properties.Resources.Home_black;
            this.btnSubViewCurrentWorking.Location = new System.Drawing.Point(0, 0);
            this.btnSubViewCurrentWorking.MainFont = new System.Drawing.Font("맑은 고딕", 14.25F, System.Drawing.FontStyle.Bold);
            this.btnSubViewCurrentWorking.MainFontColor = System.Drawing.Color.White;
            this.btnSubViewCurrentWorking.Margin = new System.Windows.Forms.Padding(0);
            this.btnSubViewCurrentWorking.Name = "btnSubViewCurrentWorking";
            this.btnSubViewCurrentWorking.Size = new System.Drawing.Size(239, 39);
            this.btnSubViewCurrentWorking.SubFont = new System.Drawing.Font("맑은 고딕", 8F, System.Drawing.FontStyle.Bold);
            this.btnSubViewCurrentWorking.SubFontColor = System.Drawing.Color.DarkBlue;
            this.btnSubViewCurrentWorking.SubText = "STATUS";
            this.btnSubViewCurrentWorking.TabIndex = 10413;
            this.btnSubViewCurrentWorking.Text = "CURRENT WORKING";
            this.btnSubViewCurrentWorking.TextAlignMain = Sys3Controls.EN_TEXTALIGN.MIDDLE_CENTER;
            this.btnSubViewCurrentWorking.TextAlignSub = Sys3Controls.EN_TEXTALIGN.TOP_RIGHT;
            this.btnSubViewCurrentWorking.ThemeIndex = 0;
            this.btnSubViewCurrentWorking.UseBorder = false;
            this.btnSubViewCurrentWorking.UseClickedEmphasizeTextColor = false;
            this.btnSubViewCurrentWorking.UseCustomizeClickedColor = true;
            this.btnSubViewCurrentWorking.UseEdge = false;
            this.btnSubViewCurrentWorking.UseHoverEmphasizeCustomColor = false;
            this.btnSubViewCurrentWorking.UseImage = false;
            this.btnSubViewCurrentWorking.UserHoverEmpahsize = false;
            this.btnSubViewCurrentWorking.UseSubFont = false;
            this.btnSubViewCurrentWorking.Click += new System.EventHandler(this.BtnSubViewClicked);
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 1;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.Controls.Add(this.panel1, 0, 0);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel1.Margin = new System.Windows.Forms.Padding(0);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 2;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 5F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 95F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(1140, 900);
            this.tableLayoutPanel1.TabIndex = 4;
            // 
            // Operation_History
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.BackColor = System.Drawing.Color.WhiteSmoke;
            this.Controls.Add(this.tableLayoutPanel1);
            this.Margin = new System.Windows.Forms.Padding(0);
            this.Name = "Operation_History";
            this.Size = new System.Drawing.Size(1140, 900);
            this.panel1.ResumeLayout(false);
            this.tableLayoutPanel1.ResumeLayout(false);
            this.ResumeLayout(false);

		}

        #endregion

        private System.Windows.Forms.Panel panel1;
        private Sys3Controls.Sys3button btnSubViewHistory;
        private Sys3Controls.Sys3button btnSubViewCurrentWorking;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
    }
}
