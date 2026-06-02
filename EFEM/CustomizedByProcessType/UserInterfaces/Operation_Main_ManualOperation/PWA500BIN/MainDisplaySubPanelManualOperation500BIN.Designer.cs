
namespace EFEM.CustomizedByProcessType.UserInterface.OperationMainManual.PWA500BIN
{
    partial class MainDisplaySubPanelManualOperation500BIN
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
            this.pnManualOperation = new System.Windows.Forms.Panel();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.panel1 = new System.Windows.Forms.Panel();
            this.btnManualOperationEditor = new Sys3Controls.Sys3button();
            this.btnManualOperationPIO = new Sys3Controls.Sys3button();
            this.btnManualOperationLoadPort = new Sys3Controls.Sys3button();
            this.btnManualOperationUnload = new Sys3Controls.Sys3button();
            this.btnManualOperationLoad = new Sys3Controls.Sys3button();
            this.tableLayoutPanel1.SuspendLayout();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // pnManualOperation
            // 
            this.pnManualOperation.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pnManualOperation.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnManualOperation.Location = new System.Drawing.Point(0, 50);
            this.pnManualOperation.Margin = new System.Windows.Forms.Padding(0);
            this.pnManualOperation.Name = "pnManualOperation";
            this.pnManualOperation.Size = new System.Drawing.Size(1126, 658);
            this.pnManualOperation.TabIndex = 0;
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 1;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.Controls.Add(this.pnManualOperation, 0, 1);
            this.tableLayoutPanel1.Controls.Add(this.panel1, 0, 0);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel1.Margin = new System.Windows.Forms.Padding(0);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 2;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 7.142857F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 92.85714F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(1126, 708);
            this.tableLayoutPanel1.TabIndex = 12;
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.btnManualOperationEditor);
            this.panel1.Controls.Add(this.btnManualOperationPIO);
            this.panel1.Controls.Add(this.btnManualOperationLoadPort);
            this.panel1.Controls.Add(this.btnManualOperationUnload);
            this.panel1.Controls.Add(this.btnManualOperationLoad);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Margin = new System.Windows.Forms.Padding(0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(1126, 50);
            this.panel1.TabIndex = 10;
            // 
            // btnManualOperationEditor
            // 
            this.btnManualOperationEditor.BorderWidth = 2;
            this.btnManualOperationEditor.ButtonClicked = false;
            this.btnManualOperationEditor.ClickedEmphasizeTextColor = System.Drawing.Color.White;
            this.btnManualOperationEditor.CustomClickedGradientFirstColor = System.Drawing.Color.DarkBlue;
            this.btnManualOperationEditor.CustomClickedGradientSecondColor = System.Drawing.Color.DarkBlue;
            this.btnManualOperationEditor.Description = "";
            this.btnManualOperationEditor.DisabledColor = System.Drawing.Color.DarkGray;
            this.btnManualOperationEditor.Dock = System.Windows.Forms.DockStyle.Left;
            this.btnManualOperationEditor.EdgeRadius = 5;
            this.btnManualOperationEditor.GradientAngle = 70F;
            this.btnManualOperationEditor.GradientFirstColor = System.Drawing.Color.White;
            this.btnManualOperationEditor.GradientSecondColor = System.Drawing.Color.White;
            this.btnManualOperationEditor.HoverEmphasizeCustomColor = System.Drawing.Color.White;
            this.btnManualOperationEditor.ImagePosition = new System.Drawing.Point(7, 7);
            this.btnManualOperationEditor.ImageSize = new System.Drawing.Point(30, 30);
            this.btnManualOperationEditor.LoadImage = global::FrameOfSystem3.Properties.Resources.Home_black;
            this.btnManualOperationEditor.Location = new System.Drawing.Point(800, 0);
            this.btnManualOperationEditor.MainFont = new System.Drawing.Font("맑은 고딕", 14.25F, System.Drawing.FontStyle.Bold);
            this.btnManualOperationEditor.MainFontColor = System.Drawing.Color.DarkBlue;
            this.btnManualOperationEditor.Margin = new System.Windows.Forms.Padding(0);
            this.btnManualOperationEditor.Name = "btnManualOperationEditor";
            this.btnManualOperationEditor.Size = new System.Drawing.Size(200, 50);
            this.btnManualOperationEditor.SubFont = new System.Drawing.Font("맑은 고딕", 8F, System.Drawing.FontStyle.Bold);
            this.btnManualOperationEditor.SubFontColor = System.Drawing.Color.DarkBlue;
            this.btnManualOperationEditor.SubText = "STATUS";
            this.btnManualOperationEditor.TabIndex = 5;
            this.btnManualOperationEditor.Text = "MATERIAL EDITOR";
            this.btnManualOperationEditor.TextAlignMain = Sys3Controls.EN_TEXTALIGN.MIDDLE_CENTER;
            this.btnManualOperationEditor.TextAlignSub = Sys3Controls.EN_TEXTALIGN.TOP_RIGHT;
            this.btnManualOperationEditor.ThemeIndex = 0;
            this.btnManualOperationEditor.UseBorder = false;
            this.btnManualOperationEditor.UseClickedEmphasizeTextColor = false;
            this.btnManualOperationEditor.UseCustomizeClickedColor = true;
            this.btnManualOperationEditor.UseEdge = false;
            this.btnManualOperationEditor.UseHoverEmphasizeCustomColor = false;
            this.btnManualOperationEditor.UseImage = false;
            this.btnManualOperationEditor.UserHoverEmpahsize = false;
            this.btnManualOperationEditor.UseSubFont = false;
            this.btnManualOperationEditor.Click += new System.EventHandler(this.BtnSubPanelClicked);
            // 
            // btnManualOperationPIO
            // 
            this.btnManualOperationPIO.BorderWidth = 2;
            this.btnManualOperationPIO.ButtonClicked = false;
            this.btnManualOperationPIO.ClickedEmphasizeTextColor = System.Drawing.Color.White;
            this.btnManualOperationPIO.CustomClickedGradientFirstColor = System.Drawing.Color.DarkBlue;
            this.btnManualOperationPIO.CustomClickedGradientSecondColor = System.Drawing.Color.DarkBlue;
            this.btnManualOperationPIO.Description = "";
            this.btnManualOperationPIO.DisabledColor = System.Drawing.Color.DarkGray;
            this.btnManualOperationPIO.Dock = System.Windows.Forms.DockStyle.Left;
            this.btnManualOperationPIO.EdgeRadius = 5;
            this.btnManualOperationPIO.GradientAngle = 70F;
            this.btnManualOperationPIO.GradientFirstColor = System.Drawing.Color.White;
            this.btnManualOperationPIO.GradientSecondColor = System.Drawing.Color.White;
            this.btnManualOperationPIO.HoverEmphasizeCustomColor = System.Drawing.Color.White;
            this.btnManualOperationPIO.ImagePosition = new System.Drawing.Point(7, 7);
            this.btnManualOperationPIO.ImageSize = new System.Drawing.Point(30, 30);
            this.btnManualOperationPIO.LoadImage = global::FrameOfSystem3.Properties.Resources.Home_black;
            this.btnManualOperationPIO.Location = new System.Drawing.Point(600, 0);
            this.btnManualOperationPIO.MainFont = new System.Drawing.Font("맑은 고딕", 14.25F, System.Drawing.FontStyle.Bold);
            this.btnManualOperationPIO.MainFontColor = System.Drawing.Color.DarkBlue;
            this.btnManualOperationPIO.Margin = new System.Windows.Forms.Padding(0);
            this.btnManualOperationPIO.Name = "btnManualOperationPIO";
            this.btnManualOperationPIO.Size = new System.Drawing.Size(200, 50);
            this.btnManualOperationPIO.SubFont = new System.Drawing.Font("맑은 고딕", 8F, System.Drawing.FontStyle.Bold);
            this.btnManualOperationPIO.SubFontColor = System.Drawing.Color.DarkBlue;
            this.btnManualOperationPIO.SubText = "STATUS";
            this.btnManualOperationPIO.TabIndex = 7;
            this.btnManualOperationPIO.Text = "PARALLEL IO";
            this.btnManualOperationPIO.TextAlignMain = Sys3Controls.EN_TEXTALIGN.MIDDLE_CENTER;
            this.btnManualOperationPIO.TextAlignSub = Sys3Controls.EN_TEXTALIGN.TOP_RIGHT;
            this.btnManualOperationPIO.ThemeIndex = 0;
            this.btnManualOperationPIO.UseBorder = false;
            this.btnManualOperationPIO.UseClickedEmphasizeTextColor = false;
            this.btnManualOperationPIO.UseCustomizeClickedColor = true;
            this.btnManualOperationPIO.UseEdge = false;
            this.btnManualOperationPIO.UseHoverEmphasizeCustomColor = false;
            this.btnManualOperationPIO.UseImage = false;
            this.btnManualOperationPIO.UserHoverEmpahsize = false;
            this.btnManualOperationPIO.UseSubFont = false;
            this.btnManualOperationPIO.Click += new System.EventHandler(this.BtnSubPanelClicked);
            // 
            // btnManualOperationLoadPort
            // 
            this.btnManualOperationLoadPort.BorderWidth = 2;
            this.btnManualOperationLoadPort.ButtonClicked = false;
            this.btnManualOperationLoadPort.ClickedEmphasizeTextColor = System.Drawing.Color.White;
            this.btnManualOperationLoadPort.CustomClickedGradientFirstColor = System.Drawing.Color.DarkBlue;
            this.btnManualOperationLoadPort.CustomClickedGradientSecondColor = System.Drawing.Color.DarkBlue;
            this.btnManualOperationLoadPort.Description = "";
            this.btnManualOperationLoadPort.DisabledColor = System.Drawing.Color.DarkGray;
            this.btnManualOperationLoadPort.Dock = System.Windows.Forms.DockStyle.Left;
            this.btnManualOperationLoadPort.EdgeRadius = 5;
            this.btnManualOperationLoadPort.GradientAngle = 70F;
            this.btnManualOperationLoadPort.GradientFirstColor = System.Drawing.Color.White;
            this.btnManualOperationLoadPort.GradientSecondColor = System.Drawing.Color.White;
            this.btnManualOperationLoadPort.HoverEmphasizeCustomColor = System.Drawing.Color.White;
            this.btnManualOperationLoadPort.ImagePosition = new System.Drawing.Point(7, 7);
            this.btnManualOperationLoadPort.ImageSize = new System.Drawing.Point(30, 30);
            this.btnManualOperationLoadPort.LoadImage = global::FrameOfSystem3.Properties.Resources.Home_black;
            this.btnManualOperationLoadPort.Location = new System.Drawing.Point(400, 0);
            this.btnManualOperationLoadPort.MainFont = new System.Drawing.Font("맑은 고딕", 14.25F, System.Drawing.FontStyle.Bold);
            this.btnManualOperationLoadPort.MainFontColor = System.Drawing.Color.DarkBlue;
            this.btnManualOperationLoadPort.Margin = new System.Windows.Forms.Padding(0);
            this.btnManualOperationLoadPort.Name = "btnManualOperationLoadPort";
            this.btnManualOperationLoadPort.Size = new System.Drawing.Size(200, 50);
            this.btnManualOperationLoadPort.SubFont = new System.Drawing.Font("맑은 고딕", 8F, System.Drawing.FontStyle.Bold);
            this.btnManualOperationLoadPort.SubFontColor = System.Drawing.Color.DarkBlue;
            this.btnManualOperationLoadPort.SubText = "STATUS";
            this.btnManualOperationLoadPort.TabIndex = 6;
            this.btnManualOperationLoadPort.Text = "LOADPORT";
            this.btnManualOperationLoadPort.TextAlignMain = Sys3Controls.EN_TEXTALIGN.MIDDLE_CENTER;
            this.btnManualOperationLoadPort.TextAlignSub = Sys3Controls.EN_TEXTALIGN.TOP_RIGHT;
            this.btnManualOperationLoadPort.ThemeIndex = 0;
            this.btnManualOperationLoadPort.UseBorder = false;
            this.btnManualOperationLoadPort.UseClickedEmphasizeTextColor = false;
            this.btnManualOperationLoadPort.UseCustomizeClickedColor = true;
            this.btnManualOperationLoadPort.UseEdge = false;
            this.btnManualOperationLoadPort.UseHoverEmphasizeCustomColor = false;
            this.btnManualOperationLoadPort.UseImage = false;
            this.btnManualOperationLoadPort.UserHoverEmpahsize = false;
            this.btnManualOperationLoadPort.UseSubFont = false;
            this.btnManualOperationLoadPort.Click += new System.EventHandler(this.BtnSubPanelClicked);
            // 
            // btnManualOperationUnload
            // 
            this.btnManualOperationUnload.BorderWidth = 2;
            this.btnManualOperationUnload.ButtonClicked = false;
            this.btnManualOperationUnload.ClickedEmphasizeTextColor = System.Drawing.Color.White;
            this.btnManualOperationUnload.CustomClickedGradientFirstColor = System.Drawing.Color.DarkBlue;
            this.btnManualOperationUnload.CustomClickedGradientSecondColor = System.Drawing.Color.DarkBlue;
            this.btnManualOperationUnload.Description = "";
            this.btnManualOperationUnload.DisabledColor = System.Drawing.Color.DarkGray;
            this.btnManualOperationUnload.Dock = System.Windows.Forms.DockStyle.Left;
            this.btnManualOperationUnload.EdgeRadius = 5;
            this.btnManualOperationUnload.GradientAngle = 70F;
            this.btnManualOperationUnload.GradientFirstColor = System.Drawing.Color.White;
            this.btnManualOperationUnload.GradientSecondColor = System.Drawing.Color.White;
            this.btnManualOperationUnload.HoverEmphasizeCustomColor = System.Drawing.Color.White;
            this.btnManualOperationUnload.ImagePosition = new System.Drawing.Point(7, 7);
            this.btnManualOperationUnload.ImageSize = new System.Drawing.Point(30, 30);
            this.btnManualOperationUnload.LoadImage = global::FrameOfSystem3.Properties.Resources.Home_black;
            this.btnManualOperationUnload.Location = new System.Drawing.Point(200, 0);
            this.btnManualOperationUnload.MainFont = new System.Drawing.Font("맑은 고딕", 14.25F, System.Drawing.FontStyle.Bold);
            this.btnManualOperationUnload.MainFontColor = System.Drawing.Color.DarkBlue;
            this.btnManualOperationUnload.Margin = new System.Windows.Forms.Padding(0);
            this.btnManualOperationUnload.Name = "btnManualOperationUnload";
            this.btnManualOperationUnload.Size = new System.Drawing.Size(200, 50);
            this.btnManualOperationUnload.SubFont = new System.Drawing.Font("맑은 고딕", 8F, System.Drawing.FontStyle.Bold);
            this.btnManualOperationUnload.SubFontColor = System.Drawing.Color.DarkBlue;
            this.btnManualOperationUnload.SubText = "STATUS";
            this.btnManualOperationUnload.TabIndex = 4;
            this.btnManualOperationUnload.Text = "UNLOAD";
            this.btnManualOperationUnload.TextAlignMain = Sys3Controls.EN_TEXTALIGN.MIDDLE_CENTER;
            this.btnManualOperationUnload.TextAlignSub = Sys3Controls.EN_TEXTALIGN.TOP_RIGHT;
            this.btnManualOperationUnload.ThemeIndex = 0;
            this.btnManualOperationUnload.UseBorder = false;
            this.btnManualOperationUnload.UseClickedEmphasizeTextColor = false;
            this.btnManualOperationUnload.UseCustomizeClickedColor = true;
            this.btnManualOperationUnload.UseEdge = false;
            this.btnManualOperationUnload.UseHoverEmphasizeCustomColor = false;
            this.btnManualOperationUnload.UseImage = false;
            this.btnManualOperationUnload.UserHoverEmpahsize = false;
            this.btnManualOperationUnload.UseSubFont = false;
            this.btnManualOperationUnload.Click += new System.EventHandler(this.BtnSubPanelClicked);
            // 
            // btnManualOperationLoad
            // 
            this.btnManualOperationLoad.BorderWidth = 2;
            this.btnManualOperationLoad.ButtonClicked = true;
            this.btnManualOperationLoad.ClickedEmphasizeTextColor = System.Drawing.Color.White;
            this.btnManualOperationLoad.CustomClickedGradientFirstColor = System.Drawing.Color.DarkBlue;
            this.btnManualOperationLoad.CustomClickedGradientSecondColor = System.Drawing.Color.DarkBlue;
            this.btnManualOperationLoad.Description = "";
            this.btnManualOperationLoad.DisabledColor = System.Drawing.Color.DarkGray;
            this.btnManualOperationLoad.Dock = System.Windows.Forms.DockStyle.Left;
            this.btnManualOperationLoad.EdgeRadius = 5;
            this.btnManualOperationLoad.GradientAngle = 70F;
            this.btnManualOperationLoad.GradientFirstColor = System.Drawing.Color.White;
            this.btnManualOperationLoad.GradientSecondColor = System.Drawing.Color.White;
            this.btnManualOperationLoad.HoverEmphasizeCustomColor = System.Drawing.Color.White;
            this.btnManualOperationLoad.ImagePosition = new System.Drawing.Point(7, 7);
            this.btnManualOperationLoad.ImageSize = new System.Drawing.Point(30, 30);
            this.btnManualOperationLoad.LoadImage = global::FrameOfSystem3.Properties.Resources.Home_black;
            this.btnManualOperationLoad.Location = new System.Drawing.Point(0, 0);
            this.btnManualOperationLoad.MainFont = new System.Drawing.Font("맑은 고딕", 14.25F, System.Drawing.FontStyle.Bold);
            this.btnManualOperationLoad.MainFontColor = System.Drawing.Color.White;
            this.btnManualOperationLoad.Margin = new System.Windows.Forms.Padding(0);
            this.btnManualOperationLoad.Name = "btnManualOperationLoad";
            this.btnManualOperationLoad.Size = new System.Drawing.Size(200, 50);
            this.btnManualOperationLoad.SubFont = new System.Drawing.Font("맑은 고딕", 8F, System.Drawing.FontStyle.Bold);
            this.btnManualOperationLoad.SubFontColor = System.Drawing.Color.DarkBlue;
            this.btnManualOperationLoad.SubText = "STATUS";
            this.btnManualOperationLoad.TabIndex = 3;
            this.btnManualOperationLoad.Text = "LOAD";
            this.btnManualOperationLoad.TextAlignMain = Sys3Controls.EN_TEXTALIGN.MIDDLE_CENTER;
            this.btnManualOperationLoad.TextAlignSub = Sys3Controls.EN_TEXTALIGN.TOP_RIGHT;
            this.btnManualOperationLoad.ThemeIndex = 0;
            this.btnManualOperationLoad.UseBorder = false;
            this.btnManualOperationLoad.UseClickedEmphasizeTextColor = false;
            this.btnManualOperationLoad.UseCustomizeClickedColor = true;
            this.btnManualOperationLoad.UseEdge = false;
            this.btnManualOperationLoad.UseHoverEmphasizeCustomColor = false;
            this.btnManualOperationLoad.UseImage = false;
            this.btnManualOperationLoad.UserHoverEmpahsize = false;
            this.btnManualOperationLoad.UseSubFont = false;
            this.btnManualOperationLoad.Click += new System.EventHandler(this.BtnSubPanelClicked);
            // 
            // MainDisplaySubPanelManualOperation500BIN
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.BackColor = System.Drawing.SystemColors.Control;
            this.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.Controls.Add(this.tableLayoutPanel1);
            this.Margin = new System.Windows.Forms.Padding(0);
            this.Name = "MainDisplaySubPanelManualOperation500BIN";
            this.Size = new System.Drawing.Size(1126, 708);
            this.tableLayoutPanel1.ResumeLayout(false);
            this.panel1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel pnManualOperation;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.Panel panel1;
        private Sys3Controls.Sys3button btnManualOperationEditor;
        private Sys3Controls.Sys3button btnManualOperationLoadPort;
        private Sys3Controls.Sys3button btnManualOperationUnload;
        private Sys3Controls.Sys3button btnManualOperationLoad;
        private Sys3Controls.Sys3button btnManualOperationPIO;
    }
}
