namespace FrameOfSystem3.Views.Operation
{
    partial class Operation_JobInfo
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
            this.m_pnlHeader = new System.Windows.Forms.Panel();
            this.m_btnRefresh = new System.Windows.Forms.Button();
            this.m_lblTitle = new System.Windows.Forms.Label();
            this.m_splitMain = new System.Windows.Forms.SplitContainer();
            this._treeViewJobs = new System.Windows.Forms.TreeView();
            this.txtJobDetail = new System.Windows.Forms.TextBox();
            this.btnStopProcessJob = new Sys3Controls.Sys3button();
            this.btnAbortAllJobs = new Sys3Controls.Sys3button();
            this.m_pnlHeader.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.m_splitMain)).BeginInit();
            this.m_splitMain.Panel1.SuspendLayout();
            this.m_splitMain.Panel2.SuspendLayout();
            this.m_splitMain.SuspendLayout();
            this.SuspendLayout();
            // 
            // m_pnlHeader
            // 
            this.m_pnlHeader.BackColor = System.Drawing.Color.Gainsboro;
            this.m_pnlHeader.Controls.Add(this.m_btnRefresh);
            this.m_pnlHeader.Controls.Add(this.m_lblTitle);
            this.m_pnlHeader.Dock = System.Windows.Forms.DockStyle.Top;
            this.m_pnlHeader.Location = new System.Drawing.Point(0, 0);
            this.m_pnlHeader.Margin = new System.Windows.Forms.Padding(0);
            this.m_pnlHeader.Name = "m_pnlHeader";
            this.m_pnlHeader.Padding = new System.Windows.Forms.Padding(10, 8, 10, 8);
            this.m_pnlHeader.Size = new System.Drawing.Size(1140, 46);
            this.m_pnlHeader.TabIndex = 0;
            // 
            // m_btnRefresh
            // 
            this.m_btnRefresh.Dock = System.Windows.Forms.DockStyle.Right;
            this.m_btnRefresh.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.m_btnRefresh.Location = new System.Drawing.Point(1030, 8);
            this.m_btnRefresh.Margin = new System.Windows.Forms.Padding(0);
            this.m_btnRefresh.Name = "m_btnRefresh";
            this.m_btnRefresh.Size = new System.Drawing.Size(100, 30);
            this.m_btnRefresh.TabIndex = 1;
            this.m_btnRefresh.Text = "Refresh";
            this.m_btnRefresh.UseVisualStyleBackColor = true;
            this.m_btnRefresh.Click += new System.EventHandler(this.BtnRefresh_Click);
            // 
            // m_lblTitle
            // 
            this.m_lblTitle.Dock = System.Windows.Forms.DockStyle.Left;
            this.m_lblTitle.Font = new System.Drawing.Font("맑은 고딕", 11F, System.Drawing.FontStyle.Bold);
            this.m_lblTitle.Location = new System.Drawing.Point(10, 8);
            this.m_lblTitle.Margin = new System.Windows.Forms.Padding(0);
            this.m_lblTitle.Name = "m_lblTitle";
            this.m_lblTitle.Size = new System.Drawing.Size(260, 30);
            this.m_lblTitle.TabIndex = 0;
            this.m_lblTitle.Text = "Job Information";
            this.m_lblTitle.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // m_splitMain
            // 
            this.m_splitMain.Dock = System.Windows.Forms.DockStyle.Fill;
            this.m_splitMain.Location = new System.Drawing.Point(0, 46);
            this.m_splitMain.Margin = new System.Windows.Forms.Padding(0);
            this.m_splitMain.Name = "m_splitMain";
            // 
            // m_splitMain.Panel1
            // 
            this.m_splitMain.Panel1.Controls.Add(this._treeViewJobs);
            this.m_splitMain.Panel1.Padding = new System.Windows.Forms.Padding(8);
            // 
            // m_splitMain.Panel2
            // 
            this.m_splitMain.Panel2.Controls.Add(this.btnAbortAllJobs);
            this.m_splitMain.Panel2.Controls.Add(this.btnStopProcessJob);
            this.m_splitMain.Panel2.Controls.Add(this.txtJobDetail);
            this.m_splitMain.Panel2.Padding = new System.Windows.Forms.Padding(8);
            this.m_splitMain.Size = new System.Drawing.Size(1140, 854);
            this.m_splitMain.SplitterDistance = 570;
            this.m_splitMain.TabIndex = 1;
            // 
            // _treeViewJobs
            // 
            this._treeViewJobs.BackColor = System.Drawing.Color.White;
            this._treeViewJobs.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this._treeViewJobs.Dock = System.Windows.Forms.DockStyle.Fill;
            this._treeViewJobs.Font = new System.Drawing.Font("맑은 고딕", 9F);
            this._treeViewJobs.HideSelection = false;
            this._treeViewJobs.Location = new System.Drawing.Point(8, 8);
            this._treeViewJobs.Margin = new System.Windows.Forms.Padding(0);
            this._treeViewJobs.Name = "_treeViewJobs";
            this._treeViewJobs.Size = new System.Drawing.Size(554, 838);
            this._treeViewJobs.TabIndex = 0;
            this._treeViewJobs.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this.TreeViewJobs_AfterSelect);
            // 
            // txtJobDetail
            // 
            this.txtJobDetail.BackColor = System.Drawing.Color.White;
            this.txtJobDetail.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtJobDetail.Font = new System.Drawing.Font("Consolas", 9F);
            this.txtJobDetail.Location = new System.Drawing.Point(8, 8);
            this.txtJobDetail.Margin = new System.Windows.Forms.Padding(0);
            this.txtJobDetail.Multiline = true;
            this.txtJobDetail.Name = "txtJobDetail";
            this.txtJobDetail.ReadOnly = true;
            this.txtJobDetail.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.txtJobDetail.Size = new System.Drawing.Size(550, 406);
            this.txtJobDetail.TabIndex = 0;
            this.txtJobDetail.WordWrap = false;
            // 
            // btnStopProcessJob
            // 
            this.btnStopProcessJob.BorderWidth = 2;
            this.btnStopProcessJob.ButtonClicked = false;
            this.btnStopProcessJob.ClickedEmphasizeTextColor = System.Drawing.Color.White;
            this.btnStopProcessJob.CustomClickedGradientFirstColor = System.Drawing.Color.BlanchedAlmond;
            this.btnStopProcessJob.CustomClickedGradientSecondColor = System.Drawing.Color.Gold;
            this.btnStopProcessJob.Description = "";
            this.btnStopProcessJob.DisabledColor = System.Drawing.Color.Silver;
            this.btnStopProcessJob.EdgeRadius = 5;
            this.btnStopProcessJob.GradientAngle = 60F;
            this.btnStopProcessJob.GradientFirstColor = System.Drawing.Color.PaleGreen;
            this.btnStopProcessJob.GradientSecondColor = System.Drawing.Color.ForestGreen;
            this.btnStopProcessJob.HoverEmphasizeCustomColor = System.Drawing.Color.Firebrick;
            this.btnStopProcessJob.ImagePosition = new System.Drawing.Point(5, 10);
            this.btnStopProcessJob.ImageSize = new System.Drawing.Point(30, 30);
            this.btnStopProcessJob.LoadImage = global::FrameOfSystem3.Properties.Resources.Start_white;
            this.btnStopProcessJob.Location = new System.Drawing.Point(8, 437);
            this.btnStopProcessJob.MainFont = new System.Drawing.Font("맑은 고딕", 12F, System.Drawing.FontStyle.Bold);
            this.btnStopProcessJob.MainFontColor = System.Drawing.Color.White;
            this.btnStopProcessJob.Name = "btnStopProcessJob";
            this.btnStopProcessJob.Size = new System.Drawing.Size(249, 68);
            this.btnStopProcessJob.SubFont = new System.Drawing.Font("맑은 고딕", 10F);
            this.btnStopProcessJob.SubFontColor = System.Drawing.Color.Black;
            this.btnStopProcessJob.SubText = "";
            this.btnStopProcessJob.TabIndex = 21152;
            this.btnStopProcessJob.Tag = "";
            this.btnStopProcessJob.Text = "STOP ALL JOBS";
            this.btnStopProcessJob.TextAlignMain = Sys3Controls.EN_TEXTALIGN.MIDDLE_CENTER;
            this.btnStopProcessJob.TextAlignSub = Sys3Controls.EN_TEXTALIGN.MIDDLE_CENTER;
            this.btnStopProcessJob.ThemeIndex = 0;
            this.btnStopProcessJob.UseBorder = true;
            this.btnStopProcessJob.UseClickedEmphasizeTextColor = false;
            this.btnStopProcessJob.UseCustomizeClickedColor = true;
            this.btnStopProcessJob.UseEdge = true;
            this.btnStopProcessJob.UseHoverEmphasizeCustomColor = true;
            this.btnStopProcessJob.UseImage = true;
            this.btnStopProcessJob.UserHoverEmpahsize = true;
            this.btnStopProcessJob.UseSubFont = false;
            this.btnStopProcessJob.Click += new System.EventHandler(this.BtnExecuteJobCommand);
            // 
            // btnAbortAllJobs
            // 
            this.btnAbortAllJobs.BorderWidth = 2;
            this.btnAbortAllJobs.ButtonClicked = false;
            this.btnAbortAllJobs.ClickedEmphasizeTextColor = System.Drawing.Color.White;
            this.btnAbortAllJobs.CustomClickedGradientFirstColor = System.Drawing.Color.BlanchedAlmond;
            this.btnAbortAllJobs.CustomClickedGradientSecondColor = System.Drawing.Color.Gold;
            this.btnAbortAllJobs.Description = "";
            this.btnAbortAllJobs.DisabledColor = System.Drawing.Color.Silver;
            this.btnAbortAllJobs.EdgeRadius = 5;
            this.btnAbortAllJobs.GradientAngle = 60F;
            this.btnAbortAllJobs.GradientFirstColor = System.Drawing.Color.PaleGreen;
            this.btnAbortAllJobs.GradientSecondColor = System.Drawing.Color.ForestGreen;
            this.btnAbortAllJobs.HoverEmphasizeCustomColor = System.Drawing.Color.Firebrick;
            this.btnAbortAllJobs.ImagePosition = new System.Drawing.Point(5, 10);
            this.btnAbortAllJobs.ImageSize = new System.Drawing.Point(30, 30);
            this.btnAbortAllJobs.LoadImage = global::FrameOfSystem3.Properties.Resources.Start_white;
            this.btnAbortAllJobs.Location = new System.Drawing.Point(8, 520);
            this.btnAbortAllJobs.MainFont = new System.Drawing.Font("맑은 고딕", 12F, System.Drawing.FontStyle.Bold);
            this.btnAbortAllJobs.MainFontColor = System.Drawing.Color.White;
            this.btnAbortAllJobs.Name = "btnAbortAllJobs";
            this.btnAbortAllJobs.Size = new System.Drawing.Size(249, 68);
            this.btnAbortAllJobs.SubFont = new System.Drawing.Font("맑은 고딕", 10F);
            this.btnAbortAllJobs.SubFontColor = System.Drawing.Color.Black;
            this.btnAbortAllJobs.SubText = "";
            this.btnAbortAllJobs.TabIndex = 21153;
            this.btnAbortAllJobs.Tag = "";
            this.btnAbortAllJobs.Text = "ABORT ALL JOBS";
            this.btnAbortAllJobs.TextAlignMain = Sys3Controls.EN_TEXTALIGN.MIDDLE_CENTER;
            this.btnAbortAllJobs.TextAlignSub = Sys3Controls.EN_TEXTALIGN.MIDDLE_CENTER;
            this.btnAbortAllJobs.ThemeIndex = 0;
            this.btnAbortAllJobs.UseBorder = true;
            this.btnAbortAllJobs.UseClickedEmphasizeTextColor = false;
            this.btnAbortAllJobs.UseCustomizeClickedColor = true;
            this.btnAbortAllJobs.UseEdge = true;
            this.btnAbortAllJobs.UseHoverEmphasizeCustomColor = true;
            this.btnAbortAllJobs.UseImage = true;
            this.btnAbortAllJobs.UserHoverEmpahsize = true;
            this.btnAbortAllJobs.UseSubFont = false;
            this.btnAbortAllJobs.Click += new System.EventHandler(this.BtnExecuteJobCommand);
            // 
            // Operation_JobInfo
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.BackColor = System.Drawing.Color.WhiteSmoke;
            this.Controls.Add(this.m_splitMain);
            this.Controls.Add(this.m_pnlHeader);
            this.Margin = new System.Windows.Forms.Padding(0);
            this.Name = "Operation_JobInfo";
            this.Size = new System.Drawing.Size(1140, 900);
            this.m_pnlHeader.ResumeLayout(false);
            this.m_splitMain.Panel1.ResumeLayout(false);
            this.m_splitMain.Panel2.ResumeLayout(false);
            this.m_splitMain.Panel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.m_splitMain)).EndInit();
            this.m_splitMain.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel m_pnlHeader;
        private System.Windows.Forms.Label m_lblTitle;
        private System.Windows.Forms.Button m_btnRefresh;
        private System.Windows.Forms.SplitContainer m_splitMain;
        private System.Windows.Forms.TreeView _treeViewJobs;
        private System.Windows.Forms.TextBox txtJobDetail;
        private Sys3Controls.Sys3button btnAbortAllJobs;
        private Sys3Controls.Sys3button btnStopProcessJob;
    }
}