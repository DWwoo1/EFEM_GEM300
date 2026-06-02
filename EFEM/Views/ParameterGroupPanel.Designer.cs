namespace FrameOfSystem3.Views
{
	partial class ParameterGroupPanel
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
            this.panelView = new System.Windows.Forms.Panel();
            this.panelSubject = new System.Windows.Forms.Panel();
            this.gb_SubjectLabel = new Sys3Controls.Sys3GroupBox();
            this.panelSubject.SuspendLayout();
            this.SuspendLayout();
            // 
            // panelView
            // 
            this.panelView.BackColor = System.Drawing.SystemColors.Control;
            this.panelView.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panelView.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelView.Location = new System.Drawing.Point(0, 35);
            this.panelView.Margin = new System.Windows.Forms.Padding(0);
            this.panelView.Name = "panelView";
            this.panelView.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.panelView.Size = new System.Drawing.Size(1140, 65);
            this.panelView.TabIndex = 20728;
            // 
            // panelSubject
            // 
            this.panelSubject.BackColor = System.Drawing.SystemColors.Control;
            this.panelSubject.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panelSubject.Controls.Add(this.gb_SubjectLabel);
            this.panelSubject.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelSubject.Location = new System.Drawing.Point(0, 0);
            this.panelSubject.Name = "panelSubject";
            this.panelSubject.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.panelSubject.Size = new System.Drawing.Size(1140, 35);
            this.panelSubject.TabIndex = 20730;
            // 
            // gb_SubjectLabel
            // 
            this.gb_SubjectLabel.BackGroundColor = System.Drawing.Color.WhiteSmoke;
            this.gb_SubjectLabel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gb_SubjectLabel.EdgeBorderStroke = 2;
            this.gb_SubjectLabel.EdgeRadius = 0;
            this.gb_SubjectLabel.LabelFont = new System.Drawing.Font("맑은 고딕", 12F, System.Drawing.FontStyle.Bold);
            this.gb_SubjectLabel.LabelGradientColorFirst = System.Drawing.Color.DarkGray;
            this.gb_SubjectLabel.LabelGradientColorSecond = System.Drawing.Color.WhiteSmoke;
            this.gb_SubjectLabel.LabelHeight = 30;
            this.gb_SubjectLabel.LabelTextColor = System.Drawing.Color.Black;
            this.gb_SubjectLabel.Location = new System.Drawing.Point(0, 0);
            this.gb_SubjectLabel.Name = "gb_SubjectLabel";
            this.gb_SubjectLabel.Size = new System.Drawing.Size(1138, 33);
            this.gb_SubjectLabel.TabIndex = 20730;
            this.gb_SubjectLabel.Tag = "";
            this.gb_SubjectLabel.Text = "SUBJECT";
            this.gb_SubjectLabel.TextAlign = Sys3Controls.EN_TEXTALIGN.MIDDLE_CENTER;
            this.gb_SubjectLabel.ThemeIndex = 0;
            this.gb_SubjectLabel.UseLabelBorder = false;
            this.gb_SubjectLabel.Click += new System.EventHandler(this.gb_SubjectLabel_Click);
            // 
            // ParameterGroupPanel
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.panelView);
            this.Controls.Add(this.panelSubject);
            this.Margin = new System.Windows.Forms.Padding(0);
            this.Name = "ParameterGroupPanel";
            this.Size = new System.Drawing.Size(1140, 100);
            this.panelSubject.ResumeLayout(false);
            this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.Panel panelView;
		private System.Windows.Forms.Panel panelSubject;
        private Sys3Controls.Sys3GroupBox gb_SubjectLabel;
    }
}
