namespace FrameOfSystem3.Views.Setup
{
	partial class Setup_ProcessModules
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
            this.pnMainView = new System.Windows.Forms.Panel();
            this.SuspendLayout();
            // 
            // pnMainView
            // 
            this.pnMainView.AutoScroll = true;
            this.pnMainView.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnMainView.Location = new System.Drawing.Point(0, 0);
            this.pnMainView.Margin = new System.Windows.Forms.Padding(1, 1, 0, 0);
            this.pnMainView.Name = "pnMainView";
            this.pnMainView.Size = new System.Drawing.Size(1140, 900);
            this.pnMainView.TabIndex = 5;
            // 
            // Setup_ProcessModules
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.Controls.Add(this.pnMainView);
            this.Margin = new System.Windows.Forms.Padding(0);
            this.Name = "Setup_ProcessModules";
            this.Size = new System.Drawing.Size(1140, 900);
            this.ResumeLayout(false);

		}

		#endregion
		private System.Windows.Forms.Panel pnMainView;

	}
}
