namespace FrameOfSystem3.Views
{
    partial class Form_Frame
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

        #region Windows Form 디자이너에서 생성한 코드

        /// <summary>
        /// 디자이너 지원에 필요한 메서드입니다.
        /// 이 메서드의 내용을 코드 편집기로 수정하지 마십시오.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form_Frame));
            this._tableLayoutPanel_FormFrame = new System.Windows.Forms.TableLayoutPanel();
            this.SuspendLayout();
            // 
            // _tableLayoutPanel_FormFrame
            // 
            this._tableLayoutPanel_FormFrame.ColumnCount = 2;
            this._tableLayoutPanel_FormFrame.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this._tableLayoutPanel_FormFrame.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 140F));
            this._tableLayoutPanel_FormFrame.Dock = System.Windows.Forms.DockStyle.Fill;
            this._tableLayoutPanel_FormFrame.Location = new System.Drawing.Point(0, 0);
            this._tableLayoutPanel_FormFrame.Margin = new System.Windows.Forms.Padding(0);
            this._tableLayoutPanel_FormFrame.Name = "_tableLayoutPanel_FormFrame";
            this._tableLayoutPanel_FormFrame.RowCount = 3;
            this._tableLayoutPanel_FormFrame.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 66F));
            this._tableLayoutPanel_FormFrame.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this._tableLayoutPanel_FormFrame.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 58F));
            this._tableLayoutPanel_FormFrame.Size = new System.Drawing.Size(1280, 1024);
            this._tableLayoutPanel_FormFrame.TabIndex = 0;
            // 
            // Form_Frame
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.ClientSize = new System.Drawing.Size(1280, 1024);
            this.ControlBox = false;
            this.Controls.Add(this._tableLayoutPanel_FormFrame);
            this.DoubleBuffered = true;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "Form_Frame";
            this.StartPosition = System.Windows.Forms.FormStartPosition.Manual;
            this.Text = "Form_Frame";
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel _tableLayoutPanel_FormFrame;

    }
}

