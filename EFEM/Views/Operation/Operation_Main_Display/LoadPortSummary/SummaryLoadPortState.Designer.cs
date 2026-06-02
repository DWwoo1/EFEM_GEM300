
namespace FrameOfSystem3.Views.Operation.SubPanelSummary.LoadPortSummary
{
    partial class SummaryLoadPortState
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
            this.components = new System.ComponentModel.Container();
            this.tooltipLoadPortStates = new System.Windows.Forms.ToolTip(this.components);
            this.ledDocked = new Sys3Controls.Sys3LedLabel();
            this.ledOpened = new Sys3Controls.Sys3LedLabel();
            this.ledClamped = new Sys3Controls.Sys3LedLabel();
            this.ledPresent = new Sys3Controls.Sys3LedLabel();
            this.ledPlaced = new Sys3Controls.Sys3LedLabel();
            this.btnLoadPortState = new Sys3Controls.Sys3button();
            this.btnLoadPortStateParallelIO = new Sys3Controls.Sys3button();
            this.ledInitialized = new Sys3Controls.Sys3LedLabel();
            this.btnLoadPortStateSlotInfo = new Sys3Controls.Sys3button();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.tableLayoutPanel2 = new System.Windows.Forms.TableLayoutPanel();
            this.tableLayoutPanel3 = new System.Windows.Forms.TableLayoutPanel();
            this.pnLoadPortState = new System.Windows.Forms.Panel();
            this.sys3Label1 = new Sys3Controls.Sys3Label();
            this.sys3Label2 = new Sys3Controls.Sys3Label();
            this.sys3Label4 = new Sys3Controls.Sys3Label();
            this.lblCarrierId = new Sys3Controls.Sys3Label();
            this.tableLayoutPanel1.SuspendLayout();
            this.tableLayoutPanel2.SuspendLayout();
            this.tableLayoutPanel3.SuspendLayout();
            this.SuspendLayout();
            // 
            // tooltipLoadPortStates
            // 
            this.tooltipLoadPortStates.ToolTipIcon = System.Windows.Forms.ToolTipIcon.Info;
            // 
            // ledDocked
            // 
            this.ledDocked.Active = false;
            this.ledDocked.ColorActive = System.Drawing.Color.LimeGreen;
            this.ledDocked.ColorNormal = System.Drawing.Color.WhiteSmoke;
            this.ledDocked.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ledDocked.Location = new System.Drawing.Point(117, 1);
            this.ledDocked.Margin = new System.Windows.Forms.Padding(1);
            this.ledDocked.Name = "ledDocked";
            this.ledDocked.Size = new System.Drawing.Size(27, 12);
            this.ledDocked.TabIndex = 5;
            this.ledDocked.Tag = "Dock";
            this.ledDocked.MouseHover += new System.EventHandler(this.LedLoadPortStatesHovered);
            // 
            // ledOpened
            // 
            this.ledOpened.Active = false;
            this.ledOpened.ColorActive = System.Drawing.Color.LimeGreen;
            this.ledOpened.ColorNormal = System.Drawing.Color.WhiteSmoke;
            this.ledOpened.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ledOpened.Location = new System.Drawing.Point(146, 1);
            this.ledOpened.Margin = new System.Windows.Forms.Padding(1);
            this.ledOpened.Name = "ledOpened";
            this.ledOpened.Size = new System.Drawing.Size(28, 12);
            this.ledOpened.TabIndex = 4;
            this.ledOpened.Tag = "Open";
            this.ledOpened.MouseHover += new System.EventHandler(this.LedLoadPortStatesHovered);
            // 
            // ledClamped
            // 
            this.ledClamped.Active = false;
            this.ledClamped.ColorActive = System.Drawing.Color.LimeGreen;
            this.ledClamped.ColorNormal = System.Drawing.Color.WhiteSmoke;
            this.ledClamped.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ledClamped.Location = new System.Drawing.Point(88, 1);
            this.ledClamped.Margin = new System.Windows.Forms.Padding(1);
            this.ledClamped.Name = "ledClamped";
            this.ledClamped.Size = new System.Drawing.Size(27, 12);
            this.ledClamped.TabIndex = 3;
            this.ledClamped.Tag = "Clamp";
            this.ledClamped.MouseHover += new System.EventHandler(this.LedLoadPortStatesHovered);
            // 
            // ledPresent
            // 
            this.ledPresent.Active = false;
            this.ledPresent.ColorActive = System.Drawing.Color.LimeGreen;
            this.ledPresent.ColorNormal = System.Drawing.Color.WhiteSmoke;
            this.ledPresent.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ledPresent.Location = new System.Drawing.Point(30, 1);
            this.ledPresent.Margin = new System.Windows.Forms.Padding(1);
            this.ledPresent.Name = "ledPresent";
            this.ledPresent.Size = new System.Drawing.Size(27, 12);
            this.ledPresent.TabIndex = 2;
            this.ledPresent.Tag = "Present";
            this.ledPresent.MouseHover += new System.EventHandler(this.LedLoadPortStatesHovered);
            // 
            // ledPlaced
            // 
            this.ledPlaced.Active = false;
            this.ledPlaced.ColorActive = System.Drawing.Color.LimeGreen;
            this.ledPlaced.ColorNormal = System.Drawing.Color.WhiteSmoke;
            this.ledPlaced.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ledPlaced.Location = new System.Drawing.Point(59, 1);
            this.ledPlaced.Margin = new System.Windows.Forms.Padding(1);
            this.ledPlaced.Name = "ledPlaced";
            this.ledPlaced.Size = new System.Drawing.Size(27, 12);
            this.ledPlaced.TabIndex = 1;
            this.ledPlaced.Tag = "Place";
            this.ledPlaced.MouseHover += new System.EventHandler(this.LedLoadPortStatesHovered);
            // 
            // btnLoadPortState
            // 
            this.btnLoadPortState.BorderWidth = 5;
            this.btnLoadPortState.ButtonClicked = false;
            this.btnLoadPortState.ClickedEmphasizeTextColor = System.Drawing.Color.White;
            this.btnLoadPortState.CustomClickedGradientFirstColor = System.Drawing.Color.LightSteelBlue;
            this.btnLoadPortState.CustomClickedGradientSecondColor = System.Drawing.Color.RoyalBlue;
            this.btnLoadPortState.Description = "";
            this.btnLoadPortState.DisabledColor = System.Drawing.Color.DarkGray;
            this.btnLoadPortState.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnLoadPortState.EdgeRadius = 1;
            this.btnLoadPortState.GradientAngle = 60F;
            this.btnLoadPortState.GradientFirstColor = System.Drawing.Color.WhiteSmoke;
            this.btnLoadPortState.GradientSecondColor = System.Drawing.Color.Gray;
            this.btnLoadPortState.HoverEmphasizeCustomColor = System.Drawing.Color.MidnightBlue;
            this.btnLoadPortState.ImagePosition = new System.Drawing.Point(37, 25);
            this.btnLoadPortState.ImageSize = new System.Drawing.Point(30, 30);
            this.btnLoadPortState.LoadImage = null;
            this.btnLoadPortState.Location = new System.Drawing.Point(58, 0);
            this.btnLoadPortState.MainFont = new System.Drawing.Font("맑은 고딕", 8.25F, System.Drawing.FontStyle.Bold);
            this.btnLoadPortState.MainFontColor = System.Drawing.Color.White;
            this.btnLoadPortState.Margin = new System.Windows.Forms.Padding(0);
            this.btnLoadPortState.Name = "btnLoadPortState";
            this.btnLoadPortState.Size = new System.Drawing.Size(62, 42);
            this.btnLoadPortState.SubFont = new System.Drawing.Font("맑은 고딕", 10F);
            this.btnLoadPortState.SubFontColor = System.Drawing.Color.Black;
            this.btnLoadPortState.SubText = "";
            this.btnLoadPortState.TabIndex = 20539;
            this.btnLoadPortState.Tag = "";
            this.btnLoadPortState.Text = "STATE";
            this.btnLoadPortState.TextAlignMain = Sys3Controls.EN_TEXTALIGN.MIDDLE_CENTER;
            this.btnLoadPortState.TextAlignSub = Sys3Controls.EN_TEXTALIGN.TOP_LEFT;
            this.btnLoadPortState.ThemeIndex = 0;
            this.btnLoadPortState.UseBorder = true;
            this.btnLoadPortState.UseClickedEmphasizeTextColor = false;
            this.btnLoadPortState.UseCustomizeClickedColor = true;
            this.btnLoadPortState.UseEdge = true;
            this.btnLoadPortState.UseHoverEmphasizeCustomColor = true;
            this.btnLoadPortState.UseImage = true;
            this.btnLoadPortState.UserHoverEmpahsize = true;
            this.btnLoadPortState.UseSubFont = true;
            this.btnLoadPortState.Click += new System.EventHandler(this.BtnSubPanelButtonClicked);
            // 
            // btnLoadPortStateParallelIO
            // 
            this.btnLoadPortStateParallelIO.BorderWidth = 5;
            this.btnLoadPortStateParallelIO.ButtonClicked = false;
            this.btnLoadPortStateParallelIO.ClickedEmphasizeTextColor = System.Drawing.Color.White;
            this.btnLoadPortStateParallelIO.CustomClickedGradientFirstColor = System.Drawing.Color.LightSteelBlue;
            this.btnLoadPortStateParallelIO.CustomClickedGradientSecondColor = System.Drawing.Color.RoyalBlue;
            this.btnLoadPortStateParallelIO.Description = "";
            this.btnLoadPortStateParallelIO.DisabledColor = System.Drawing.Color.DarkGray;
            this.btnLoadPortStateParallelIO.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnLoadPortStateParallelIO.EdgeRadius = 1;
            this.btnLoadPortStateParallelIO.GradientAngle = 60F;
            this.btnLoadPortStateParallelIO.GradientFirstColor = System.Drawing.Color.WhiteSmoke;
            this.btnLoadPortStateParallelIO.GradientSecondColor = System.Drawing.Color.Gray;
            this.btnLoadPortStateParallelIO.HoverEmphasizeCustomColor = System.Drawing.Color.MidnightBlue;
            this.btnLoadPortStateParallelIO.ImagePosition = new System.Drawing.Point(37, 25);
            this.btnLoadPortStateParallelIO.ImageSize = new System.Drawing.Point(30, 30);
            this.btnLoadPortStateParallelIO.LoadImage = null;
            this.btnLoadPortStateParallelIO.Location = new System.Drawing.Point(120, 0);
            this.btnLoadPortStateParallelIO.MainFont = new System.Drawing.Font("맑은 고딕", 8.25F, System.Drawing.FontStyle.Bold);
            this.btnLoadPortStateParallelIO.MainFontColor = System.Drawing.Color.White;
            this.btnLoadPortStateParallelIO.Margin = new System.Windows.Forms.Padding(0);
            this.btnLoadPortStateParallelIO.Name = "btnLoadPortStateParallelIO";
            this.btnLoadPortStateParallelIO.Size = new System.Drawing.Size(55, 42);
            this.btnLoadPortStateParallelIO.SubFont = new System.Drawing.Font("맑은 고딕", 10F);
            this.btnLoadPortStateParallelIO.SubFontColor = System.Drawing.Color.Black;
            this.btnLoadPortStateParallelIO.SubText = "";
            this.btnLoadPortStateParallelIO.TabIndex = 20538;
            this.btnLoadPortStateParallelIO.Tag = "";
            this.btnLoadPortStateParallelIO.Text = "PIO";
            this.btnLoadPortStateParallelIO.TextAlignMain = Sys3Controls.EN_TEXTALIGN.MIDDLE_CENTER;
            this.btnLoadPortStateParallelIO.TextAlignSub = Sys3Controls.EN_TEXTALIGN.TOP_LEFT;
            this.btnLoadPortStateParallelIO.ThemeIndex = 0;
            this.btnLoadPortStateParallelIO.UseBorder = true;
            this.btnLoadPortStateParallelIO.UseClickedEmphasizeTextColor = false;
            this.btnLoadPortStateParallelIO.UseCustomizeClickedColor = true;
            this.btnLoadPortStateParallelIO.UseEdge = true;
            this.btnLoadPortStateParallelIO.UseHoverEmphasizeCustomColor = true;
            this.btnLoadPortStateParallelIO.UseImage = true;
            this.btnLoadPortStateParallelIO.UserHoverEmpahsize = true;
            this.btnLoadPortStateParallelIO.UseSubFont = true;
            this.btnLoadPortStateParallelIO.Click += new System.EventHandler(this.BtnSubPanelButtonClicked);
            // 
            // ledInitialized
            // 
            this.ledInitialized.Active = false;
            this.ledInitialized.ColorActive = System.Drawing.Color.LimeGreen;
            this.ledInitialized.ColorNormal = System.Drawing.Color.Red;
            this.ledInitialized.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ledInitialized.Location = new System.Drawing.Point(1, 1);
            this.ledInitialized.Margin = new System.Windows.Forms.Padding(1);
            this.ledInitialized.Name = "ledInitialized";
            this.ledInitialized.Size = new System.Drawing.Size(27, 12);
            this.ledInitialized.TabIndex = 6;
            this.ledInitialized.Tag = "Initialized";
            this.ledInitialized.MouseHover += new System.EventHandler(this.LedLoadPortStatesHovered);
            // 
            // btnLoadPortStateSlotInfo
            // 
            this.btnLoadPortStateSlotInfo.BorderWidth = 5;
            this.btnLoadPortStateSlotInfo.ButtonClicked = true;
            this.btnLoadPortStateSlotInfo.ClickedEmphasizeTextColor = System.Drawing.Color.White;
            this.btnLoadPortStateSlotInfo.CustomClickedGradientFirstColor = System.Drawing.Color.LightSteelBlue;
            this.btnLoadPortStateSlotInfo.CustomClickedGradientSecondColor = System.Drawing.Color.RoyalBlue;
            this.btnLoadPortStateSlotInfo.Description = "";
            this.btnLoadPortStateSlotInfo.DisabledColor = System.Drawing.Color.DarkGray;
            this.btnLoadPortStateSlotInfo.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnLoadPortStateSlotInfo.EdgeRadius = 1;
            this.btnLoadPortStateSlotInfo.GradientAngle = 60F;
            this.btnLoadPortStateSlotInfo.GradientFirstColor = System.Drawing.Color.WhiteSmoke;
            this.btnLoadPortStateSlotInfo.GradientSecondColor = System.Drawing.Color.Gray;
            this.btnLoadPortStateSlotInfo.HoverEmphasizeCustomColor = System.Drawing.Color.MidnightBlue;
            this.btnLoadPortStateSlotInfo.ImagePosition = new System.Drawing.Point(37, 25);
            this.btnLoadPortStateSlotInfo.ImageSize = new System.Drawing.Point(30, 30);
            this.btnLoadPortStateSlotInfo.LoadImage = null;
            this.btnLoadPortStateSlotInfo.Location = new System.Drawing.Point(0, 0);
            this.btnLoadPortStateSlotInfo.MainFont = new System.Drawing.Font("맑은 고딕", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.btnLoadPortStateSlotInfo.MainFontColor = System.Drawing.Color.White;
            this.btnLoadPortStateSlotInfo.Margin = new System.Windows.Forms.Padding(0);
            this.btnLoadPortStateSlotInfo.Name = "btnLoadPortStateSlotInfo";
            this.btnLoadPortStateSlotInfo.Size = new System.Drawing.Size(58, 42);
            this.btnLoadPortStateSlotInfo.SubFont = new System.Drawing.Font("맑은 고딕", 10F);
            this.btnLoadPortStateSlotInfo.SubFontColor = System.Drawing.Color.Black;
            this.btnLoadPortStateSlotInfo.SubText = "";
            this.btnLoadPortStateSlotInfo.TabIndex = 20537;
            this.btnLoadPortStateSlotInfo.Tag = "";
            this.btnLoadPortStateSlotInfo.Text = "SLOT";
            this.btnLoadPortStateSlotInfo.TextAlignMain = Sys3Controls.EN_TEXTALIGN.MIDDLE_CENTER;
            this.btnLoadPortStateSlotInfo.TextAlignSub = Sys3Controls.EN_TEXTALIGN.TOP_LEFT;
            this.btnLoadPortStateSlotInfo.ThemeIndex = 0;
            this.btnLoadPortStateSlotInfo.UseBorder = true;
            this.btnLoadPortStateSlotInfo.UseClickedEmphasizeTextColor = false;
            this.btnLoadPortStateSlotInfo.UseCustomizeClickedColor = true;
            this.btnLoadPortStateSlotInfo.UseEdge = true;
            this.btnLoadPortStateSlotInfo.UseHoverEmphasizeCustomColor = true;
            this.btnLoadPortStateSlotInfo.UseImage = true;
            this.btnLoadPortStateSlotInfo.UserHoverEmpahsize = true;
            this.btnLoadPortStateSlotInfo.UseSubFont = true;
            this.btnLoadPortStateSlotInfo.Click += new System.EventHandler(this.BtnSubPanelButtonClicked);
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 1;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.Controls.Add(this.tableLayoutPanel2, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.tableLayoutPanel3, 0, 1);
            this.tableLayoutPanel1.Controls.Add(this.pnLoadPortState, 0, 2);
            this.tableLayoutPanel1.Controls.Add(this.sys3Label1, 0, 3);
            this.tableLayoutPanel1.Controls.Add(this.sys3Label2, 0, 4);
            this.tableLayoutPanel1.Controls.Add(this.sys3Label4, 0, 5);
            this.tableLayoutPanel1.Controls.Add(this.lblCarrierId, 0, 6);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel1.Margin = new System.Windows.Forms.Padding(0);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 7;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 5F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 15F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 7.5F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 7.5F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 7.5F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 7.5F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(175, 281);
            this.tableLayoutPanel1.TabIndex = 1;
            // 
            // tableLayoutPanel2
            // 
            this.tableLayoutPanel2.ColumnCount = 6;
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 16.66667F));
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 16.66667F));
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 16.66667F));
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 16.66667F));
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 16.66667F));
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 16.66667F));
            this.tableLayoutPanel2.Controls.Add(this.ledInitialized, 0, 0);
            this.tableLayoutPanel2.Controls.Add(this.ledDocked, 1, 0);
            this.tableLayoutPanel2.Controls.Add(this.ledOpened, 1, 0);
            this.tableLayoutPanel2.Controls.Add(this.ledClamped, 1, 0);
            this.tableLayoutPanel2.Controls.Add(this.ledPresent, 1, 0);
            this.tableLayoutPanel2.Controls.Add(this.ledPlaced, 1, 0);
            this.tableLayoutPanel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel2.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel2.Margin = new System.Windows.Forms.Padding(0);
            this.tableLayoutPanel2.Name = "tableLayoutPanel2";
            this.tableLayoutPanel2.RowCount = 1;
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel2.Size = new System.Drawing.Size(175, 14);
            this.tableLayoutPanel2.TabIndex = 20546;
            // 
            // tableLayoutPanel3
            // 
            this.tableLayoutPanel3.ColumnCount = 3;
            this.tableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 33.67347F));
            this.tableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 35.71429F));
            this.tableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 30.61225F));
            this.tableLayoutPanel3.Controls.Add(this.btnLoadPortState, 0, 0);
            this.tableLayoutPanel3.Controls.Add(this.btnLoadPortStateParallelIO, 1, 0);
            this.tableLayoutPanel3.Controls.Add(this.btnLoadPortStateSlotInfo, 0, 0);
            this.tableLayoutPanel3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel3.Location = new System.Drawing.Point(0, 14);
            this.tableLayoutPanel3.Margin = new System.Windows.Forms.Padding(0);
            this.tableLayoutPanel3.Name = "tableLayoutPanel3";
            this.tableLayoutPanel3.RowCount = 1;
            this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel3.Size = new System.Drawing.Size(175, 42);
            this.tableLayoutPanel3.TabIndex = 20547;
            // 
            // pnLoadPortState
            // 
            this.pnLoadPortState.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnLoadPortState.Location = new System.Drawing.Point(0, 56);
            this.pnLoadPortState.Margin = new System.Windows.Forms.Padding(0);
            this.pnLoadPortState.Name = "pnLoadPortState";
            this.pnLoadPortState.Size = new System.Drawing.Size(175, 140);
            this.pnLoadPortState.TabIndex = 20539;
            // 
            // sys3Label1
            // 
            this.sys3Label1.BackGroundColor = System.Drawing.Color.LightBlue;
            this.sys3Label1.BorderStroke = 2;
            this.sys3Label1.BorderStyle = System.Windows.Forms.ButtonBorderStyle.Solid;
            this.sys3Label1.Description = "";
            this.sys3Label1.DisabledColor = System.Drawing.Color.DarkGray;
            this.sys3Label1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.sys3Label1.EdgeRadius = 1;
            this.sys3Label1.ImagePosition = new System.Drawing.Point(0, 0);
            this.sys3Label1.ImageSize = new System.Drawing.Point(0, 0);
            this.sys3Label1.LoadImage = null;
            this.sys3Label1.Location = new System.Drawing.Point(0, 197);
            this.sys3Label1.MainFont = new System.Drawing.Font("맑은 고딕", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.sys3Label1.MainFontColor = System.Drawing.Color.Black;
            this.sys3Label1.Margin = new System.Windows.Forms.Padding(0, 1, 0, 0);
            this.sys3Label1.Name = "sys3Label1";
            this.sys3Label1.Size = new System.Drawing.Size(175, 20);
            this.sys3Label1.SubFont = new System.Drawing.Font("맑은 고딕", 10F);
            this.sys3Label1.SubFontColor = System.Drawing.Color.Black;
            this.sys3Label1.SubText = "";
            this.sys3Label1.TabIndex = 20541;
            this.sys3Label1.Text = "LOT ID";
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
            this.sys3Label2.BackGroundColor = System.Drawing.Color.Bisque;
            this.sys3Label2.BorderStroke = 2;
            this.sys3Label2.BorderStyle = System.Windows.Forms.ButtonBorderStyle.Solid;
            this.sys3Label2.Description = "";
            this.sys3Label2.DisabledColor = System.Drawing.Color.DarkGray;
            this.sys3Label2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.sys3Label2.EdgeRadius = 1;
            this.sys3Label2.ImagePosition = new System.Drawing.Point(0, 0);
            this.sys3Label2.ImageSize = new System.Drawing.Point(0, 0);
            this.sys3Label2.LoadImage = null;
            this.sys3Label2.Location = new System.Drawing.Point(0, 217);
            this.sys3Label2.MainFont = new System.Drawing.Font("맑은 고딕", 10F);
            this.sys3Label2.MainFontColor = System.Drawing.Color.Black;
            this.sys3Label2.Margin = new System.Windows.Forms.Padding(0);
            this.sys3Label2.Name = "sys3Label2";
            this.sys3Label2.Size = new System.Drawing.Size(175, 21);
            this.sys3Label2.SubFont = new System.Drawing.Font("맑은 고딕", 10F);
            this.sys3Label2.SubFontColor = System.Drawing.Color.Black;
            this.sys3Label2.SubText = "";
            this.sys3Label2.TabIndex = 20542;
            this.sys3Label2.Text = "Lot Id";
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
            // sys3Label4
            // 
            this.sys3Label4.BackGroundColor = System.Drawing.Color.LightBlue;
            this.sys3Label4.BorderStroke = 2;
            this.sys3Label4.BorderStyle = System.Windows.Forms.ButtonBorderStyle.Solid;
            this.sys3Label4.Description = "";
            this.sys3Label4.DisabledColor = System.Drawing.Color.DarkGray;
            this.sys3Label4.Dock = System.Windows.Forms.DockStyle.Fill;
            this.sys3Label4.EdgeRadius = 1;
            this.sys3Label4.ImagePosition = new System.Drawing.Point(0, 0);
            this.sys3Label4.ImageSize = new System.Drawing.Point(0, 0);
            this.sys3Label4.LoadImage = null;
            this.sys3Label4.Location = new System.Drawing.Point(0, 239);
            this.sys3Label4.MainFont = new System.Drawing.Font("맑은 고딕", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.sys3Label4.MainFontColor = System.Drawing.Color.Black;
            this.sys3Label4.Margin = new System.Windows.Forms.Padding(0, 1, 0, 0);
            this.sys3Label4.Name = "sys3Label4";
            this.sys3Label4.Size = new System.Drawing.Size(175, 20);
            this.sys3Label4.SubFont = new System.Drawing.Font("맑은 고딕", 10F);
            this.sys3Label4.SubFontColor = System.Drawing.Color.Black;
            this.sys3Label4.SubText = "";
            this.sys3Label4.TabIndex = 20544;
            this.sys3Label4.Text = "CARRIER ID";
            this.sys3Label4.TextAlignMain = Sys3Controls.EN_TEXTALIGN.MIDDLE_CENTER;
            this.sys3Label4.TextAlignSub = Sys3Controls.EN_TEXTALIGN.MIDDLE_CENTER;
            this.sys3Label4.TextAlignUnit = Sys3Controls.EN_TEXTALIGN.MIDDLE_CENTER;
            this.sys3Label4.ThemeIndex = 0;
            this.sys3Label4.UnitAreaRate = 30;
            this.sys3Label4.UnitFont = new System.Drawing.Font("맑은 고딕", 10F, System.Drawing.FontStyle.Bold);
            this.sys3Label4.UnitFontColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.sys3Label4.UnitPositionVertical = false;
            this.sys3Label4.UnitText = "";
            this.sys3Label4.UseBorder = true;
            this.sys3Label4.UseEdgeRadius = false;
            this.sys3Label4.UseImage = false;
            this.sys3Label4.UseSubFont = false;
            this.sys3Label4.UseUnitFont = false;
            // 
            // lblCarrierId
            // 
            this.lblCarrierId.BackGroundColor = System.Drawing.Color.Bisque;
            this.lblCarrierId.BorderStroke = 2;
            this.lblCarrierId.BorderStyle = System.Windows.Forms.ButtonBorderStyle.Solid;
            this.lblCarrierId.Description = "";
            this.lblCarrierId.DisabledColor = System.Drawing.Color.DarkGray;
            this.lblCarrierId.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblCarrierId.EdgeRadius = 1;
            this.lblCarrierId.ImagePosition = new System.Drawing.Point(0, 0);
            this.lblCarrierId.ImageSize = new System.Drawing.Point(0, 0);
            this.lblCarrierId.LoadImage = null;
            this.lblCarrierId.Location = new System.Drawing.Point(0, 259);
            this.lblCarrierId.MainFont = new System.Drawing.Font("맑은 고딕", 10F);
            this.lblCarrierId.MainFontColor = System.Drawing.Color.Black;
            this.lblCarrierId.Margin = new System.Windows.Forms.Padding(0, 0, 0, 1);
            this.lblCarrierId.Name = "lblCarrierId";
            this.lblCarrierId.Size = new System.Drawing.Size(175, 21);
            this.lblCarrierId.SubFont = new System.Drawing.Font("맑은 고딕", 10F);
            this.lblCarrierId.SubFontColor = System.Drawing.Color.Black;
            this.lblCarrierId.SubText = "";
            this.lblCarrierId.TabIndex = 20543;
            this.lblCarrierId.Text = "Carrier Id";
            this.lblCarrierId.TextAlignMain = Sys3Controls.EN_TEXTALIGN.MIDDLE_CENTER;
            this.lblCarrierId.TextAlignSub = Sys3Controls.EN_TEXTALIGN.MIDDLE_CENTER;
            this.lblCarrierId.TextAlignUnit = Sys3Controls.EN_TEXTALIGN.MIDDLE_CENTER;
            this.lblCarrierId.ThemeIndex = 0;
            this.lblCarrierId.UnitAreaRate = 30;
            this.lblCarrierId.UnitFont = new System.Drawing.Font("맑은 고딕", 10F, System.Drawing.FontStyle.Bold);
            this.lblCarrierId.UnitFontColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.lblCarrierId.UnitPositionVertical = false;
            this.lblCarrierId.UnitText = "";
            this.lblCarrierId.UseBorder = true;
            this.lblCarrierId.UseEdgeRadius = false;
            this.lblCarrierId.UseImage = false;
            this.lblCarrierId.UseSubFont = false;
            this.lblCarrierId.UseUnitFont = false;
            // 
            // SummaryLoadPortState
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.Controls.Add(this.tableLayoutPanel1);
            this.Margin = new System.Windows.Forms.Padding(0);
            this.Name = "SummaryLoadPortState";
            this.Size = new System.Drawing.Size(175, 281);
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel2.ResumeLayout(false);
            this.tableLayoutPanel3.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.ToolTip tooltipLoadPortStates;
        private Sys3Controls.Sys3LedLabel ledDocked;
        private Sys3Controls.Sys3LedLabel ledOpened;
        private Sys3Controls.Sys3LedLabel ledClamped;
        private Sys3Controls.Sys3LedLabel ledPresent;
        private Sys3Controls.Sys3LedLabel ledPlaced;
        private Sys3Controls.Sys3button btnLoadPortState;
        private Sys3Controls.Sys3button btnLoadPortStateParallelIO;
        private Sys3Controls.Sys3LedLabel ledInitialized;
        private Sys3Controls.Sys3button btnLoadPortStateSlotInfo;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel2;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel3;
        private System.Windows.Forms.Panel pnLoadPortState;
        private Sys3Controls.Sys3Label sys3Label1;
        private Sys3Controls.Sys3Label sys3Label2;
        private Sys3Controls.Sys3Label sys3Label4;
        private Sys3Controls.Sys3Label lblCarrierId;
    }
}
