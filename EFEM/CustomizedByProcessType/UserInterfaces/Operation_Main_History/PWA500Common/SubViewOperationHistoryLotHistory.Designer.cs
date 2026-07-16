
namespace EFEM.CustomizedByProcessType.UserInterface.OperationMainHistory.PWA500Common
{
    partial class SubViewOperationHistoryLotHistory
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(SubViewOperationHistoryLotHistory));
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle3 = new System.Windows.Forms.DataGridViewCellStyle();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.tableLayoutPanel3 = new System.Windows.Forms.TableLayoutPanel();
            this.btnViewSummary = new Sys3Controls.Sys3button();
            this.btnExport = new Sys3Controls.Sys3button();
            this.btnApply = new Sys3Controls.Sys3button();
            this.lblCarrierName = new Sys3Controls.Sys3Label();
            this.sys3Label1 = new Sys3Controls.Sys3Label();
            this.sys3Label2 = new Sys3Controls.Sys3Label();
            this.lblSelectedSubstrateType = new Sys3Controls.Sys3Label();
            this.gvLotList = new Sys3Controls.Sys3DoubleBufferedDataGridView();
            this.colCreatedTime = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colLotName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.calander = new EFEM.CustomizedByProcessType.UserInterface.OperationMainHistory.PWA500Common.SafeMonthCalendar();
            this.MONITORING = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.SOLUTIONCODE = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.PASSWORD = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.GRADE = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ID = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.INDEX = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.tableLayoutPanel1.SuspendLayout();
            this.tableLayoutPanel3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gvLotList)).BeginInit();
            this.SuspendLayout();
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 2;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 20F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 80F));
            this.tableLayoutPanel1.Controls.Add(this.tableLayoutPanel3, 0, 0);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel1.Margin = new System.Windows.Forms.Padding(0);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.Padding = new System.Windows.Forms.Padding(5);
            this.tableLayoutPanel1.RowCount = 1;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(1126, 708);
            this.tableLayoutPanel1.TabIndex = 2;
            // 
            // tableLayoutPanel3
            // 
            this.tableLayoutPanel3.ColumnCount = 1;
            this.tableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel3.Controls.Add(this.btnViewSummary, 0, 8);
            this.tableLayoutPanel3.Controls.Add(this.btnExport, 0, 7);
            this.tableLayoutPanel3.Controls.Add(this.btnApply, 0, 4);
            this.tableLayoutPanel3.Controls.Add(this.lblCarrierName, 0, 6);
            this.tableLayoutPanel3.Controls.Add(this.sys3Label1, 0, 5);
            this.tableLayoutPanel3.Controls.Add(this.sys3Label2, 0, 0);
            this.tableLayoutPanel3.Controls.Add(this.lblSelectedSubstrateType, 0, 1);
            this.tableLayoutPanel3.Controls.Add(this.gvLotList, 0, 3);
            this.tableLayoutPanel3.Controls.Add(this.calander, 0, 2);
            this.tableLayoutPanel3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel3.Location = new System.Drawing.Point(5, 5);
            this.tableLayoutPanel3.Margin = new System.Windows.Forms.Padding(0);
            this.tableLayoutPanel3.Name = "tableLayoutPanel3";
            this.tableLayoutPanel3.RowCount = 9;
            this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 5F));
            this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 5F));
            this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 24F));
            this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 30F));
            this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 6F));
            this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 5F));
            this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 5F));
            this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 14F));
            this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 6F));
            this.tableLayoutPanel3.Size = new System.Drawing.Size(223, 698);
            this.tableLayoutPanel3.TabIndex = 5;
            // 
            // btnViewSummary
            // 
            this.btnViewSummary.BorderWidth = 2;
            this.btnViewSummary.ButtonClicked = false;
            this.btnViewSummary.ClickedEmphasizeTextColor = System.Drawing.Color.White;
            this.btnViewSummary.CustomClickedGradientFirstColor = System.Drawing.Color.BlanchedAlmond;
            this.btnViewSummary.CustomClickedGradientSecondColor = System.Drawing.Color.Gold;
            this.btnViewSummary.Description = "";
            this.btnViewSummary.DisabledColor = System.Drawing.Color.Silver;
            this.btnViewSummary.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnViewSummary.EdgeRadius = 5;
            this.btnViewSummary.GradientAngle = 60F;
            this.btnViewSummary.GradientFirstColor = System.Drawing.Color.PaleGreen;
            this.btnViewSummary.GradientSecondColor = System.Drawing.Color.ForestGreen;
            this.btnViewSummary.HoverEmphasizeCustomColor = System.Drawing.Color.Firebrick;
            this.btnViewSummary.ImagePosition = new System.Drawing.Point(10, 10);
            this.btnViewSummary.ImageSize = new System.Drawing.Point(15, 15);
            this.btnViewSummary.LoadImage = ((System.Drawing.Image)(resources.GetObject("btnViewSummary.LoadImage")));
            this.btnViewSummary.Location = new System.Drawing.Point(2, 652);
            this.btnViewSummary.MainFont = new System.Drawing.Font("맑은 고딕", 12F, System.Drawing.FontStyle.Bold);
            this.btnViewSummary.MainFontColor = System.Drawing.Color.White;
            this.btnViewSummary.Margin = new System.Windows.Forms.Padding(2);
            this.btnViewSummary.Name = "btnViewSummary";
            this.btnViewSummary.Size = new System.Drawing.Size(219, 44);
            this.btnViewSummary.SubFont = new System.Drawing.Font("맑은 고딕", 10F);
            this.btnViewSummary.SubFontColor = System.Drawing.Color.Black;
            this.btnViewSummary.SubText = "";
            this.btnViewSummary.TabIndex = 21154;
            this.btnViewSummary.Tag = "CARRIER_LOADING";
            this.btnViewSummary.Text = "VIEW SUMMARY";
            this.btnViewSummary.TextAlignMain = Sys3Controls.EN_TEXTALIGN.MIDDLE_CENTER;
            this.btnViewSummary.TextAlignSub = Sys3Controls.EN_TEXTALIGN.MIDDLE_CENTER;
            this.btnViewSummary.ThemeIndex = 0;
            this.btnViewSummary.UseBorder = true;
            this.btnViewSummary.UseClickedEmphasizeTextColor = false;
            this.btnViewSummary.UseCustomizeClickedColor = true;
            this.btnViewSummary.UseEdge = true;
            this.btnViewSummary.UseHoverEmphasizeCustomColor = true;
            this.btnViewSummary.UseImage = true;
            this.btnViewSummary.UserHoverEmpahsize = true;
            this.btnViewSummary.UseSubFont = false;
            this.btnViewSummary.Click += new System.EventHandler(this.BtnClicked);
            // 
            // btnExport
            // 
            this.btnExport.BorderWidth = 2;
            this.btnExport.ButtonClicked = false;
            this.btnExport.ClickedEmphasizeTextColor = System.Drawing.Color.White;
            this.btnExport.CustomClickedGradientFirstColor = System.Drawing.Color.BlanchedAlmond;
            this.btnExport.CustomClickedGradientSecondColor = System.Drawing.Color.Gold;
            this.btnExport.Description = "";
            this.btnExport.DisabledColor = System.Drawing.Color.Silver;
            this.btnExport.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnExport.EdgeRadius = 5;
            this.btnExport.GradientAngle = 60F;
            this.btnExport.GradientFirstColor = System.Drawing.Color.LightSkyBlue;
            this.btnExport.GradientSecondColor = System.Drawing.Color.SteelBlue;
            this.btnExport.HoverEmphasizeCustomColor = System.Drawing.Color.Firebrick;
            this.btnExport.ImagePosition = new System.Drawing.Point(10, 10);
            this.btnExport.ImageSize = new System.Drawing.Point(15, 15);
            this.btnExport.LoadImage = ((System.Drawing.Image)(resources.GetObject("btnExport.LoadImage")));
            this.btnExport.Location = new System.Drawing.Point(2, 555);
            this.btnExport.MainFont = new System.Drawing.Font("맑은 고딕", 12F, System.Drawing.FontStyle.Bold);
            this.btnExport.MainFontColor = System.Drawing.Color.White;
            this.btnExport.Margin = new System.Windows.Forms.Padding(2);
            this.btnExport.Name = "btnExport";
            this.btnExport.Size = new System.Drawing.Size(219, 93);
            this.btnExport.SubFont = new System.Drawing.Font("맑은 고딕", 10F);
            this.btnExport.SubFontColor = System.Drawing.Color.Black;
            this.btnExport.SubText = "";
            this.btnExport.TabIndex = 21155;
            this.btnExport.Text = "EXPORT CSV";
            this.btnExport.TextAlignMain = Sys3Controls.EN_TEXTALIGN.MIDDLE_CENTER;
            this.btnExport.TextAlignSub = Sys3Controls.EN_TEXTALIGN.MIDDLE_CENTER;
            this.btnExport.ThemeIndex = 0;
            this.btnExport.UseBorder = true;
            this.btnExport.UseClickedEmphasizeTextColor = false;
            this.btnExport.UseCustomizeClickedColor = true;
            this.btnExport.UseEdge = true;
            this.btnExport.UseHoverEmphasizeCustomColor = true;
            this.btnExport.UseImage = true;
            this.btnExport.UserHoverEmpahsize = true;
            this.btnExport.UseSubFont = false;
            this.btnExport.Click += new System.EventHandler(this.BtnClicked);
            // 
            // btnApply
            // 
            this.btnApply.BorderWidth = 2;
            this.btnApply.ButtonClicked = false;
            this.btnApply.ClickedEmphasizeTextColor = System.Drawing.Color.White;
            this.btnApply.CustomClickedGradientFirstColor = System.Drawing.Color.BlanchedAlmond;
            this.btnApply.CustomClickedGradientSecondColor = System.Drawing.Color.Gold;
            this.btnApply.Description = "";
            this.btnApply.DisabledColor = System.Drawing.Color.Silver;
            this.btnApply.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnApply.EdgeRadius = 5;
            this.btnApply.GradientAngle = 60F;
            this.btnApply.GradientFirstColor = System.Drawing.Color.PaleGreen;
            this.btnApply.GradientSecondColor = System.Drawing.Color.ForestGreen;
            this.btnApply.HoverEmphasizeCustomColor = System.Drawing.Color.Firebrick;
            this.btnApply.ImagePosition = new System.Drawing.Point(10, 10);
            this.btnApply.ImageSize = new System.Drawing.Point(15, 15);
            this.btnApply.LoadImage = ((System.Drawing.Image)(resources.GetObject("btnApply.LoadImage")));
            this.btnApply.Location = new System.Drawing.Point(2, 446);
            this.btnApply.MainFont = new System.Drawing.Font("맑은 고딕", 12F, System.Drawing.FontStyle.Bold);
            this.btnApply.MainFontColor = System.Drawing.Color.White;
            this.btnApply.Margin = new System.Windows.Forms.Padding(2);
            this.btnApply.Name = "btnApply";
            this.btnApply.Size = new System.Drawing.Size(219, 37);
            this.btnApply.SubFont = new System.Drawing.Font("맑은 고딕", 10F);
            this.btnApply.SubFontColor = System.Drawing.Color.Black;
            this.btnApply.SubText = "";
            this.btnApply.TabIndex = 21153;
            this.btnApply.Tag = "CARRIER_LOADING";
            this.btnApply.Text = "APPLY";
            this.btnApply.TextAlignMain = Sys3Controls.EN_TEXTALIGN.MIDDLE_CENTER;
            this.btnApply.TextAlignSub = Sys3Controls.EN_TEXTALIGN.MIDDLE_CENTER;
            this.btnApply.ThemeIndex = 0;
            this.btnApply.UseBorder = true;
            this.btnApply.UseClickedEmphasizeTextColor = false;
            this.btnApply.UseCustomizeClickedColor = true;
            this.btnApply.UseEdge = true;
            this.btnApply.UseHoverEmphasizeCustomColor = true;
            this.btnApply.UseImage = true;
            this.btnApply.UserHoverEmpahsize = true;
            this.btnApply.UseSubFont = false;
            this.btnApply.Click += new System.EventHandler(this.BtnClicked);
            // 
            // lblCarrierName
            // 
            this.lblCarrierName.BackGroundColor = System.Drawing.Color.White;
            this.lblCarrierName.BorderStroke = 2;
            this.lblCarrierName.BorderStyle = System.Windows.Forms.ButtonBorderStyle.Solid;
            this.lblCarrierName.Description = "";
            this.lblCarrierName.DisabledColor = System.Drawing.Color.DarkGray;
            this.lblCarrierName.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblCarrierName.EdgeRadius = 1;
            this.lblCarrierName.ImagePosition = new System.Drawing.Point(0, 0);
            this.lblCarrierName.ImageSize = new System.Drawing.Point(0, 0);
            this.lblCarrierName.LoadImage = null;
            this.lblCarrierName.Location = new System.Drawing.Point(2, 519);
            this.lblCarrierName.MainFont = new System.Drawing.Font("맑은 고딕", 10F);
            this.lblCarrierName.MainFontColor = System.Drawing.Color.Black;
            this.lblCarrierName.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.lblCarrierName.Name = "lblCarrierName";
            this.lblCarrierName.Size = new System.Drawing.Size(219, 34);
            this.lblCarrierName.SubFont = new System.Drawing.Font("맑은 고딕", 10F);
            this.lblCarrierName.SubFontColor = System.Drawing.Color.Black;
            this.lblCarrierName.SubText = "";
            this.lblCarrierName.TabIndex = 21149;
            this.lblCarrierName.Text = "--";
            this.lblCarrierName.TextAlignMain = Sys3Controls.EN_TEXTALIGN.MIDDLE_CENTER;
            this.lblCarrierName.TextAlignSub = Sys3Controls.EN_TEXTALIGN.MIDDLE_CENTER;
            this.lblCarrierName.TextAlignUnit = Sys3Controls.EN_TEXTALIGN.MIDDLE_CENTER;
            this.lblCarrierName.ThemeIndex = 0;
            this.lblCarrierName.UnitAreaRate = 30;
            this.lblCarrierName.UnitFont = new System.Drawing.Font("맑은 고딕", 10F, System.Drawing.FontStyle.Bold);
            this.lblCarrierName.UnitFontColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.lblCarrierName.UnitPositionVertical = false;
            this.lblCarrierName.UnitText = "";
            this.lblCarrierName.UseBorder = true;
            this.lblCarrierName.UseEdgeRadius = false;
            this.lblCarrierName.UseImage = false;
            this.lblCarrierName.UseSubFont = false;
            this.lblCarrierName.UseUnitFont = false;
            // 
            // sys3Label1
            // 
            this.sys3Label1.BackGroundColor = System.Drawing.Color.LightSteelBlue;
            this.sys3Label1.BorderStroke = 2;
            this.sys3Label1.BorderStyle = System.Windows.Forms.ButtonBorderStyle.Solid;
            this.sys3Label1.Description = "";
            this.sys3Label1.DisabledColor = System.Drawing.Color.DarkGray;
            this.sys3Label1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.sys3Label1.EdgeRadius = 1;
            this.sys3Label1.ImagePosition = new System.Drawing.Point(0, 0);
            this.sys3Label1.ImageSize = new System.Drawing.Point(0, 0);
            this.sys3Label1.LoadImage = null;
            this.sys3Label1.Location = new System.Drawing.Point(2, 485);
            this.sys3Label1.MainFont = new System.Drawing.Font("맑은 고딕", 11.25F, System.Drawing.FontStyle.Bold);
            this.sys3Label1.MainFontColor = System.Drawing.Color.Black;
            this.sys3Label1.Margin = new System.Windows.Forms.Padding(2, 0, 2, 2);
            this.sys3Label1.Name = "sys3Label1";
            this.sys3Label1.Size = new System.Drawing.Size(219, 32);
            this.sys3Label1.SubFont = new System.Drawing.Font("맑은 고딕", 10F);
            this.sys3Label1.SubFontColor = System.Drawing.Color.Black;
            this.sys3Label1.SubText = "";
            this.sys3Label1.TabIndex = 21148;
            this.sys3Label1.Text = "CARRIER NAME";
            this.sys3Label1.TextAlignMain = Sys3Controls.EN_TEXTALIGN.MIDDLE_CENTER;
            this.sys3Label1.TextAlignSub = Sys3Controls.EN_TEXTALIGN.MIDDLE_CENTER;
            this.sys3Label1.TextAlignUnit = Sys3Controls.EN_TEXTALIGN.MIDDLE_CENTER;
            this.sys3Label1.ThemeIndex = 0;
            this.sys3Label1.UnitAreaRate = 30;
            this.sys3Label1.UnitFont = new System.Drawing.Font("맑은 고딕", 10F, System.Drawing.FontStyle.Bold);
            this.sys3Label1.UnitFontColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.sys3Label1.UnitPositionVertical = false;
            this.sys3Label1.UnitText = "";
            this.sys3Label1.UseBorder = true;
            this.sys3Label1.UseEdgeRadius = false;
            this.sys3Label1.UseImage = false;
            this.sys3Label1.UseSubFont = false;
            this.sys3Label1.UseUnitFont = false;
            // 
            // sys3Label2
            // 
            this.sys3Label2.BackGroundColor = System.Drawing.Color.LightSteelBlue;
            this.sys3Label2.BorderStroke = 2;
            this.sys3Label2.BorderStyle = System.Windows.Forms.ButtonBorderStyle.Solid;
            this.sys3Label2.Description = "";
            this.sys3Label2.DisabledColor = System.Drawing.Color.DarkGray;
            this.sys3Label2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.sys3Label2.EdgeRadius = 1;
            this.sys3Label2.ImagePosition = new System.Drawing.Point(0, 0);
            this.sys3Label2.ImageSize = new System.Drawing.Point(0, 0);
            this.sys3Label2.LoadImage = null;
            this.sys3Label2.Location = new System.Drawing.Point(2, 0);
            this.sys3Label2.MainFont = new System.Drawing.Font("맑은 고딕", 11.25F, System.Drawing.FontStyle.Bold);
            this.sys3Label2.MainFontColor = System.Drawing.Color.Black;
            this.sys3Label2.Margin = new System.Windows.Forms.Padding(2, 0, 2, 2);
            this.sys3Label2.Name = "sys3Label2";
            this.sys3Label2.Size = new System.Drawing.Size(219, 32);
            this.sys3Label2.SubFont = new System.Drawing.Font("맑은 고딕", 10F);
            this.sys3Label2.SubFontColor = System.Drawing.Color.Black;
            this.sys3Label2.SubText = "";
            this.sys3Label2.TabIndex = 21147;
            this.sys3Label2.Text = "SELECT TYPE";
            this.sys3Label2.TextAlignMain = Sys3Controls.EN_TEXTALIGN.MIDDLE_CENTER;
            this.sys3Label2.TextAlignSub = Sys3Controls.EN_TEXTALIGN.MIDDLE_CENTER;
            this.sys3Label2.TextAlignUnit = Sys3Controls.EN_TEXTALIGN.MIDDLE_CENTER;
            this.sys3Label2.ThemeIndex = 0;
            this.sys3Label2.UnitAreaRate = 30;
            this.sys3Label2.UnitFont = new System.Drawing.Font("맑은 고딕", 10F, System.Drawing.FontStyle.Bold);
            this.sys3Label2.UnitFontColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.sys3Label2.UnitPositionVertical = false;
            this.sys3Label2.UnitText = "";
            this.sys3Label2.UseBorder = true;
            this.sys3Label2.UseEdgeRadius = false;
            this.sys3Label2.UseImage = false;
            this.sys3Label2.UseSubFont = false;
            this.sys3Label2.UseUnitFont = false;
            // 
            // lblSelectedSubstrateType
            // 
            this.lblSelectedSubstrateType.BackGroundColor = System.Drawing.Color.White;
            this.lblSelectedSubstrateType.BorderStroke = 2;
            this.lblSelectedSubstrateType.BorderStyle = System.Windows.Forms.ButtonBorderStyle.Solid;
            this.lblSelectedSubstrateType.Description = "";
            this.lblSelectedSubstrateType.DisabledColor = System.Drawing.Color.DarkGray;
            this.lblSelectedSubstrateType.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblSelectedSubstrateType.EdgeRadius = 1;
            this.lblSelectedSubstrateType.ImagePosition = new System.Drawing.Point(0, 0);
            this.lblSelectedSubstrateType.ImageSize = new System.Drawing.Point(0, 0);
            this.lblSelectedSubstrateType.LoadImage = null;
            this.lblSelectedSubstrateType.Location = new System.Drawing.Point(2, 34);
            this.lblSelectedSubstrateType.MainFont = new System.Drawing.Font("맑은 고딕", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.lblSelectedSubstrateType.MainFontColor = System.Drawing.Color.Black;
            this.lblSelectedSubstrateType.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.lblSelectedSubstrateType.Name = "lblSelectedSubstrateType";
            this.lblSelectedSubstrateType.Size = new System.Drawing.Size(219, 34);
            this.lblSelectedSubstrateType.SubFont = new System.Drawing.Font("맑은 고딕", 10F);
            this.lblSelectedSubstrateType.SubFontColor = System.Drawing.Color.Black;
            this.lblSelectedSubstrateType.SubText = "";
            this.lblSelectedSubstrateType.TabIndex = 21146;
            this.lblSelectedSubstrateType.Text = "--";
            this.lblSelectedSubstrateType.TextAlignMain = Sys3Controls.EN_TEXTALIGN.MIDDLE_CENTER;
            this.lblSelectedSubstrateType.TextAlignSub = Sys3Controls.EN_TEXTALIGN.MIDDLE_CENTER;
            this.lblSelectedSubstrateType.TextAlignUnit = Sys3Controls.EN_TEXTALIGN.MIDDLE_CENTER;
            this.lblSelectedSubstrateType.ThemeIndex = 0;
            this.lblSelectedSubstrateType.UnitAreaRate = 30;
            this.lblSelectedSubstrateType.UnitFont = new System.Drawing.Font("맑은 고딕", 10F, System.Drawing.FontStyle.Bold);
            this.lblSelectedSubstrateType.UnitFontColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.lblSelectedSubstrateType.UnitPositionVertical = false;
            this.lblSelectedSubstrateType.UnitText = "";
            this.lblSelectedSubstrateType.UseBorder = true;
            this.lblSelectedSubstrateType.UseEdgeRadius = false;
            this.lblSelectedSubstrateType.UseImage = false;
            this.lblSelectedSubstrateType.UseSubFont = false;
            this.lblSelectedSubstrateType.UseUnitFont = false;
            this.lblSelectedSubstrateType.Click += new System.EventHandler(this.LblClicked);
            // 
            // gvLotList
            // 
            this.gvLotList.AllowUserToAddRows = false;
            this.gvLotList.AllowUserToDeleteRows = false;
            this.gvLotList.AllowUserToResizeColumns = false;
            this.gvLotList.AllowUserToResizeRows = false;
            this.gvLotList.BackgroundColor = System.Drawing.Color.White;
            this.gvLotList.CellBorderStyle = System.Windows.Forms.DataGridViewCellBorderStyle.Raised;
            this.gvLotList.ColumnHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.Single;
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle1.BackColor = System.Drawing.Color.LightGray;
            dataGridViewCellStyle1.Font = new System.Drawing.Font("맑은 고딕", 9F, System.Drawing.FontStyle.Bold);
            dataGridViewCellStyle1.ForeColor = System.Drawing.Color.Black;
            dataGridViewCellStyle1.SelectionBackColor = System.Drawing.Color.White;
            dataGridViewCellStyle1.SelectionForeColor = System.Drawing.Color.Black;
            dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.gvLotList.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
            this.gvLotList.ColumnHeadersHeight = 25;
            this.gvLotList.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            this.gvLotList.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.colCreatedTime,
            this.colLotName});
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle2.BackColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle2.Font = new System.Drawing.Font("맑은 고딕", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            dataGridViewCellStyle2.ForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle2.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(120)))), ((int)(((byte)(130)))), ((int)(((byte)(150)))));
            dataGridViewCellStyle2.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.gvLotList.DefaultCellStyle = dataGridViewCellStyle2;
            this.gvLotList.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gvLotList.EnableHeadersVisualStyles = false;
            this.gvLotList.GridColor = System.Drawing.Color.FromArgb(((int)(((byte)(120)))), ((int)(((byte)(130)))), ((int)(((byte)(150)))));
            this.gvLotList.Location = new System.Drawing.Point(2, 235);
            this.gvLotList.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.gvLotList.MultiSelect = false;
            this.gvLotList.Name = "gvLotList";
            this.gvLotList.ReadOnly = true;
            this.gvLotList.RowHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.None;
            dataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle3.BackColor = System.Drawing.Color.White;
            dataGridViewCellStyle3.Font = new System.Drawing.Font("맑은 고딕", 10F);
            dataGridViewCellStyle3.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle3.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(120)))), ((int)(((byte)(130)))), ((int)(((byte)(150)))));
            dataGridViewCellStyle3.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle3.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.gvLotList.RowHeadersDefaultCellStyle = dataGridViewCellStyle3;
            this.gvLotList.RowHeadersVisible = false;
            this.gvLotList.RowHeadersWidth = 62;
            this.gvLotList.RowHeadersWidthSizeMode = System.Windows.Forms.DataGridViewRowHeadersWidthSizeMode.DisableResizing;
            this.gvLotList.RowTemplate.Height = 23;
            this.gvLotList.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.gvLotList.Size = new System.Drawing.Size(219, 209);
            this.gvLotList.TabIndex = 21145;
            this.gvLotList.CellClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.GvCellClicked);
            // 
            // colCreatedTime
            // 
            this.colCreatedTime.HeaderText = "CREATED";
            this.colCreatedTime.MinimumWidth = 8;
            this.colCreatedTime.Name = "colCreatedTime";
            this.colCreatedTime.ReadOnly = true;
            this.colCreatedTime.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.colCreatedTime.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.colCreatedTime.Width = 80;
            // 
            // colLotName
            // 
            this.colLotName.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.colLotName.HeaderText = "LOT LIST";
            this.colLotName.MinimumWidth = 8;
            this.colLotName.Name = "colLotName";
            this.colLotName.ReadOnly = true;
            this.colLotName.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.colLotName.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // calander
            // 
            this.calander.Dock = System.Windows.Forms.DockStyle.Fill;
            this.calander.Location = new System.Drawing.Point(1, 71);
            this.calander.Margin = new System.Windows.Forms.Padding(1, 3, 1, 1);
            this.calander.MaxSelectionCount = 1;
            this.calander.Name = "calander";
            this.calander.TabIndex = 21144;
            this.calander.DateChanged += new System.Windows.Forms.DateRangeEventHandler(this.CalanderDateChanged);
            this.calander.DateSelected += new System.Windows.Forms.DateRangeEventHandler(this.CalanderSelected);
            // 
            // MONITORING
            // 
            this.MONITORING.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.MONITORING.HeaderText = "MESSAGE";
            this.MONITORING.MinimumWidth = 8;
            this.MONITORING.Name = "MONITORING";
            this.MONITORING.ReadOnly = true;
            this.MONITORING.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.MONITORING.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // SOLUTIONCODE
            // 
            this.SOLUTIONCODE.HeaderText = "SOLUTION";
            this.SOLUTIONCODE.MinimumWidth = 8;
            this.SOLUTIONCODE.Name = "SOLUTIONCODE";
            this.SOLUTIONCODE.ReadOnly = true;
            this.SOLUTIONCODE.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.SOLUTIONCODE.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.SOLUTIONCODE.Width = 113;
            // 
            // PASSWORD
            // 
            this.PASSWORD.HeaderText = "MESSAGE";
            this.PASSWORD.MaxInputLength = 20;
            this.PASSWORD.MinimumWidth = 8;
            this.PASSWORD.Name = "PASSWORD";
            this.PASSWORD.ReadOnly = true;
            this.PASSWORD.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.PASSWORD.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.PASSWORD.Width = 105;
            // 
            // GRADE
            // 
            this.GRADE.HeaderText = "GRADE";
            this.GRADE.MinimumWidth = 8;
            this.GRADE.Name = "GRADE";
            this.GRADE.ReadOnly = true;
            this.GRADE.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.GRADE.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.GRADE.Width = 150;
            // 
            // ID
            // 
            this.ID.HeaderText = "ALARM CODE";
            this.ID.MaxInputLength = 20;
            this.ID.MinimumWidth = 8;
            this.ID.Name = "ID";
            this.ID.ReadOnly = true;
            this.ID.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.ID.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.ID.Width = 140;
            // 
            // INDEX
            // 
            this.INDEX.HeaderText = "TIME";
            this.INDEX.MinimumWidth = 8;
            this.INDEX.Name = "INDEX";
            this.INDEX.ReadOnly = true;
            this.INDEX.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.INDEX.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.INDEX.Width = 85;
            // 
            // SubViewOperationHistoryLotHistory
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.BackColor = System.Drawing.SystemColors.Control;
            this.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.Controls.Add(this.tableLayoutPanel1);
            this.Margin = new System.Windows.Forms.Padding(0);
            this.Name = "SubViewOperationHistoryLotHistory";
            this.Size = new System.Drawing.Size(1126, 708);
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel3.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.gvLotList)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.DataGridViewTextBoxColumn INDEX;
        private System.Windows.Forms.DataGridViewTextBoxColumn ID;
        private System.Windows.Forms.DataGridViewTextBoxColumn GRADE;
        private System.Windows.Forms.DataGridViewTextBoxColumn PASSWORD;
        private System.Windows.Forms.DataGridViewTextBoxColumn SOLUTIONCODE;
        private System.Windows.Forms.DataGridViewTextBoxColumn MONITORING;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel3;
        private SafeMonthCalendar calander;
        private Sys3Controls.Sys3DoubleBufferedDataGridView gvLotList;
        private System.Windows.Forms.DataGridViewTextBoxColumn colCreatedTime;
        private System.Windows.Forms.DataGridViewTextBoxColumn colLotName;
        private Sys3Controls.Sys3Label lblSelectedSubstrateType;
        private Sys3Controls.Sys3Label sys3Label2;
        private Sys3Controls.Sys3Label lblCarrierName;
        private Sys3Controls.Sys3Label sys3Label1;
        private Sys3Controls.Sys3button btnApply;
        private Sys3Controls.Sys3button btnViewSummary;
        private Sys3Controls.Sys3button btnExport;
    }
}
