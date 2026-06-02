namespace FrameOfSystem3.Views
{
	partial class ParameterPanel
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
			this.SuspendLayout();
			// 
			// panelView
			// 
			this.panelView.BackColor = System.Drawing.SystemColors.Control;
			this.panelView.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.panelView.Dock = System.Windows.Forms.DockStyle.Fill;
			this.panelView.Location = new System.Drawing.Point(0, 0);
			this.panelView.Name = "panelView";
			this.panelView.RightToLeft = System.Windows.Forms.RightToLeft.No;
			this.panelView.Size = new System.Drawing.Size(1140, 100);
			this.panelView.TabIndex = 20728;
			// 
			// ParameterPanel
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
			this.Controls.Add(this.panelView);
			this.Name = "ParameterPanel";
			this.Size = new System.Drawing.Size(1140, 100);
			this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.Panel panelView;
	}
}
