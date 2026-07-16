using System.Drawing;
using System.Windows.Forms;

namespace EFEM.CustomizedByProcessType.UserInterface.OperationMainHistory.PWA500Common
{
    partial class SubViewOperationHistoryDatabase
    {
        /// <summary> 필수 디자이너 변수입니다. </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 사용 중인 모든 리소스를 정리합니다. </summary>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region 구성 요소 디자이너에서 생성한 코드

        private void InitializeComponent()
        {
            this.tableLayoutPanelRoot = new System.Windows.Forms.TableLayoutPanel();
            this.pnlSearch = new System.Windows.Forms.Panel();
            this._tlpSearchBar = new System.Windows.Forms.TableLayoutPanel();
            this._pnlStartDateHolder = new System.Windows.Forms.Panel();
            this._startDatePicker = new System.Windows.Forms.DateTimePicker();
            this._lblStartDateHeader = new Sys3Controls.Sys3Label();
            this._pnlEndDateHolder = new System.Windows.Forms.Panel();
            this._endDatePicker = new System.Windows.Forms.DateTimePicker();
            this._lblEndDateHeader = new Sys3Controls.Sys3Label();
            this._pnlButtonRow = new System.Windows.Forms.FlowLayoutPanel();
            this._btnSearch = new Sys3Controls.Sys3button();
            this._btnReset = new Sys3Controls.Sys3button();
            this._btnExport = new Sys3Controls.Sys3button();
            this._lblUnavailable = new Sys3Controls.Sys3Label();
            this._tlpConditions = new System.Windows.Forms.TableLayoutPanel();
            this._lblCaptionTarget = new Sys3Controls.Sys3Label();
            this._lblTargetValue = new Sys3Controls.Sys3Label();
            this._lblCond1Caption = new Sys3Controls.Sys3Label();
            this._lblCond1Value = new Sys3Controls.Sys3Label();
            this._lblCond2Caption = new Sys3Controls.Sys3Label();
            this._lblCond2Value = new Sys3Controls.Sys3Label();
            this._lblOtherCaption = new Sys3Controls.Sys3Label();
            this._lblOtherFieldValue = new Sys3Controls.Sys3Label();
            this._lblOtherMatchToggle = new Sys3Controls.Sys3Label();
            this._lblOtherValueValue = new Sys3Controls.Sys3Label();
            this.tableLayoutPanelContent = new System.Windows.Forms.TableLayoutPanel();
            this.gvResults = new Sys3Controls.Sys3DoubleBufferedDataGridView();
            this.pnlDetail = new System.Windows.Forms.Panel();
            this.tableLayoutPanelRoot.SuspendLayout();
            this.pnlSearch.SuspendLayout();
            this._tlpSearchBar.SuspendLayout();
            this._pnlStartDateHolder.SuspendLayout();
            this._pnlEndDateHolder.SuspendLayout();
            this._pnlButtonRow.SuspendLayout();
            this._tlpConditions.SuspendLayout();
            this.tableLayoutPanelContent.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gvResults)).BeginInit();
            this.SuspendLayout();
            // 
            // tableLayoutPanelRoot
            // 
            this.tableLayoutPanelRoot.ColumnCount = 1;
            this.tableLayoutPanelRoot.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanelRoot.Controls.Add(this.pnlSearch, 0, 0);
            this.tableLayoutPanelRoot.Controls.Add(this.tableLayoutPanelContent, 0, 1);
            this.tableLayoutPanelRoot.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanelRoot.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanelRoot.Margin = new System.Windows.Forms.Padding(0);
            this.tableLayoutPanelRoot.Name = "tableLayoutPanelRoot";
            this.tableLayoutPanelRoot.RowCount = 2;
            this.tableLayoutPanelRoot.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 150F));
            this.tableLayoutPanelRoot.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanelRoot.Size = new System.Drawing.Size(1140, 900);
            this.tableLayoutPanelRoot.TabIndex = 0;
            // 
            // pnlSearch
            // 
            this.pnlSearch.Controls.Add(this._tlpSearchBar);
            this.pnlSearch.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlSearch.Location = new System.Drawing.Point(3, 3);
            this.pnlSearch.Name = "pnlSearch";
            this.pnlSearch.Padding = new System.Windows.Forms.Padding(3);
            this.pnlSearch.Size = new System.Drawing.Size(1134, 144);
            this.pnlSearch.TabIndex = 0;
            // 
            // _tlpSearchBar
            // 
            this._tlpSearchBar.ColumnCount = 3;
            this._tlpSearchBar.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 250F));
            this._tlpSearchBar.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 250F));
            this._tlpSearchBar.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this._tlpSearchBar.Controls.Add(this._pnlStartDateHolder, 0, 0);
            this._tlpSearchBar.Controls.Add(this._pnlEndDateHolder, 1, 0);
            this._tlpSearchBar.Controls.Add(this._pnlButtonRow, 2, 0);
            this._tlpSearchBar.Controls.Add(this._tlpConditions, 0, 1);
            this._tlpSearchBar.Dock = System.Windows.Forms.DockStyle.Fill;
            this._tlpSearchBar.Location = new System.Drawing.Point(3, 3);
            this._tlpSearchBar.Name = "_tlpSearchBar";
            this._tlpSearchBar.RowCount = 2;
            this._tlpSearchBar.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 72F));
            this._tlpSearchBar.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this._tlpSearchBar.Size = new System.Drawing.Size(1128, 138);
            this._tlpSearchBar.TabIndex = 0;
            // 
            // _pnlStartDateHolder
            // 
            this._pnlStartDateHolder.Controls.Add(this._startDatePicker);
            this._pnlStartDateHolder.Controls.Add(this._lblStartDateHeader);
            this._pnlStartDateHolder.Dock = System.Windows.Forms.DockStyle.Fill;
            this._pnlStartDateHolder.Location = new System.Drawing.Point(2, 2);
            this._pnlStartDateHolder.Margin = new System.Windows.Forms.Padding(2);
            this._pnlStartDateHolder.Name = "_pnlStartDateHolder";
            this._pnlStartDateHolder.Size = new System.Drawing.Size(246, 68);
            this._pnlStartDateHolder.TabIndex = 0;
            // 
            // _startDatePicker
            // 
            this._startDatePicker.Font = new System.Drawing.Font("맑은 고딕", 11F);
            this._startDatePicker.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this._startDatePicker.Location = new System.Drawing.Point(8, 34);
            this._startDatePicker.Name = "_startDatePicker";
            this._startDatePicker.Size = new System.Drawing.Size(200, 27);
            this._startDatePicker.TabIndex = 1;
            this._startDatePicker.ValueChanged += new System.EventHandler(this.StartDateChanged);
            // 
            // _lblStartDateHeader
            // 
            this._lblStartDateHeader.BackGroundColor = System.Drawing.Color.LightSteelBlue;
            this._lblStartDateHeader.BorderStroke = 1;
            this._lblStartDateHeader.BorderStyle = System.Windows.Forms.ButtonBorderStyle.Solid;
            this._lblStartDateHeader.Description = "";
            this._lblStartDateHeader.DisabledColor = System.Drawing.Color.DarkGray;
            this._lblStartDateHeader.Dock = System.Windows.Forms.DockStyle.Top;
            this._lblStartDateHeader.EdgeRadius = 1;
            this._lblStartDateHeader.ImagePosition = new System.Drawing.Point(0, 0);
            this._lblStartDateHeader.ImageSize = new System.Drawing.Point(0, 0);
            this._lblStartDateHeader.LoadImage = null;
            this._lblStartDateHeader.Location = new System.Drawing.Point(0, 0);
            this._lblStartDateHeader.MainFont = new System.Drawing.Font("맑은 고딕", 9F, System.Drawing.FontStyle.Bold);
            this._lblStartDateHeader.MainFontColor = System.Drawing.Color.Black;
            this._lblStartDateHeader.Name = "_lblStartDateHeader";
            this._lblStartDateHeader.Size = new System.Drawing.Size(246, 24);
            this._lblStartDateHeader.SubFont = new System.Drawing.Font("맑은 고딕", 10F);
            this._lblStartDateHeader.SubFontColor = System.Drawing.Color.Black;
            this._lblStartDateHeader.SubText = "";
            this._lblStartDateHeader.TabIndex = 2;
            this._lblStartDateHeader.Text = "Start Date";
            this._lblStartDateHeader.TextAlignMain = Sys3Controls.EN_TEXTALIGN.MIDDLE_LEFT;
            this._lblStartDateHeader.TextAlignSub = Sys3Controls.EN_TEXTALIGN.MIDDLE_CENTER;
            this._lblStartDateHeader.TextAlignUnit = Sys3Controls.EN_TEXTALIGN.MIDDLE_CENTER;
            this._lblStartDateHeader.ThemeIndex = 0;
            this._lblStartDateHeader.UnitAreaRate = 30;
            this._lblStartDateHeader.UnitFont = new System.Drawing.Font("맑은 고딕", 10F, System.Drawing.FontStyle.Bold);
            this._lblStartDateHeader.UnitFontColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this._lblStartDateHeader.UnitPositionVertical = false;
            this._lblStartDateHeader.UnitText = "";
            this._lblStartDateHeader.UseBorder = true;
            this._lblStartDateHeader.UseEdgeRadius = false;
            this._lblStartDateHeader.UseImage = false;
            this._lblStartDateHeader.UseSubFont = false;
            this._lblStartDateHeader.UseUnitFont = false;
            // 
            // _pnlEndDateHolder
            // 
            this._pnlEndDateHolder.Controls.Add(this._endDatePicker);
            this._pnlEndDateHolder.Controls.Add(this._lblEndDateHeader);
            this._pnlEndDateHolder.Dock = System.Windows.Forms.DockStyle.Fill;
            this._pnlEndDateHolder.Location = new System.Drawing.Point(252, 2);
            this._pnlEndDateHolder.Margin = new System.Windows.Forms.Padding(2);
            this._pnlEndDateHolder.Name = "_pnlEndDateHolder";
            this._pnlEndDateHolder.Size = new System.Drawing.Size(246, 68);
            this._pnlEndDateHolder.TabIndex = 1;
            // 
            // _endDatePicker
            // 
            this._endDatePicker.Font = new System.Drawing.Font("맑은 고딕", 11F);
            this._endDatePicker.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this._endDatePicker.Location = new System.Drawing.Point(8, 34);
            this._endDatePicker.Name = "_endDatePicker";
            this._endDatePicker.Size = new System.Drawing.Size(200, 27);
            this._endDatePicker.TabIndex = 1;
            this._endDatePicker.ValueChanged += new System.EventHandler(this.EndDateChanged);
            // 
            // _lblEndDateHeader
            // 
            this._lblEndDateHeader.BackGroundColor = System.Drawing.Color.LightSteelBlue;
            this._lblEndDateHeader.BorderStroke = 1;
            this._lblEndDateHeader.BorderStyle = System.Windows.Forms.ButtonBorderStyle.Solid;
            this._lblEndDateHeader.Description = "";
            this._lblEndDateHeader.DisabledColor = System.Drawing.Color.DarkGray;
            this._lblEndDateHeader.Dock = System.Windows.Forms.DockStyle.Top;
            this._lblEndDateHeader.EdgeRadius = 1;
            this._lblEndDateHeader.ImagePosition = new System.Drawing.Point(0, 0);
            this._lblEndDateHeader.ImageSize = new System.Drawing.Point(0, 0);
            this._lblEndDateHeader.LoadImage = null;
            this._lblEndDateHeader.Location = new System.Drawing.Point(0, 0);
            this._lblEndDateHeader.MainFont = new System.Drawing.Font("맑은 고딕", 9F, System.Drawing.FontStyle.Bold);
            this._lblEndDateHeader.MainFontColor = System.Drawing.Color.Black;
            this._lblEndDateHeader.Name = "_lblEndDateHeader";
            this._lblEndDateHeader.Size = new System.Drawing.Size(246, 24);
            this._lblEndDateHeader.SubFont = new System.Drawing.Font("맑은 고딕", 10F);
            this._lblEndDateHeader.SubFontColor = System.Drawing.Color.Black;
            this._lblEndDateHeader.SubText = "";
            this._lblEndDateHeader.TabIndex = 2;
            this._lblEndDateHeader.Text = "End Date";
            this._lblEndDateHeader.TextAlignMain = Sys3Controls.EN_TEXTALIGN.MIDDLE_LEFT;
            this._lblEndDateHeader.TextAlignSub = Sys3Controls.EN_TEXTALIGN.MIDDLE_CENTER;
            this._lblEndDateHeader.TextAlignUnit = Sys3Controls.EN_TEXTALIGN.MIDDLE_CENTER;
            this._lblEndDateHeader.ThemeIndex = 0;
            this._lblEndDateHeader.UnitAreaRate = 30;
            this._lblEndDateHeader.UnitFont = new System.Drawing.Font("맑은 고딕", 10F, System.Drawing.FontStyle.Bold);
            this._lblEndDateHeader.UnitFontColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this._lblEndDateHeader.UnitPositionVertical = false;
            this._lblEndDateHeader.UnitText = "";
            this._lblEndDateHeader.UseBorder = true;
            this._lblEndDateHeader.UseEdgeRadius = false;
            this._lblEndDateHeader.UseImage = false;
            this._lblEndDateHeader.UseSubFont = false;
            this._lblEndDateHeader.UseUnitFont = false;
            // 
            // _pnlButtonRow
            // 
            this._pnlButtonRow.Controls.Add(this._btnSearch);
            this._pnlButtonRow.Controls.Add(this._btnReset);
            this._pnlButtonRow.Controls.Add(this._btnExport);
            this._pnlButtonRow.Controls.Add(this._lblUnavailable);
            this._pnlButtonRow.Dock = System.Windows.Forms.DockStyle.Fill;
            this._pnlButtonRow.Location = new System.Drawing.Point(500, 0);
            this._pnlButtonRow.Margin = new System.Windows.Forms.Padding(0);
            this._pnlButtonRow.Name = "_pnlButtonRow";
            this._pnlButtonRow.Size = new System.Drawing.Size(628, 72);
            this._pnlButtonRow.TabIndex = 3;
            this._pnlButtonRow.WrapContents = false;
            // 
            // _btnSearch
            // 
            this._btnSearch.BorderWidth = 2;
            this._btnSearch.ButtonClicked = false;
            this._btnSearch.ClickedEmphasizeTextColor = System.Drawing.Color.White;
            this._btnSearch.CustomClickedGradientFirstColor = System.Drawing.Color.BlanchedAlmond;
            this._btnSearch.CustomClickedGradientSecondColor = System.Drawing.Color.Gold;
            this._btnSearch.Description = "";
            this._btnSearch.DisabledColor = System.Drawing.Color.Silver;
            this._btnSearch.EdgeRadius = 5;
            this._btnSearch.GradientAngle = 60F;
            this._btnSearch.GradientFirstColor = System.Drawing.Color.PaleGreen;
            this._btnSearch.GradientSecondColor = System.Drawing.Color.ForestGreen;
            this._btnSearch.HoverEmphasizeCustomColor = System.Drawing.Color.Firebrick;
            this._btnSearch.ImagePosition = new System.Drawing.Point(0, 0);
            this._btnSearch.ImageSize = new System.Drawing.Point(0, 0);
            this._btnSearch.LoadImage = null;
            this._btnSearch.Location = new System.Drawing.Point(10, 5);
            this._btnSearch.MainFont = new System.Drawing.Font("맑은 고딕", 11F, System.Drawing.FontStyle.Bold);
            this._btnSearch.MainFontColor = System.Drawing.Color.White;
            this._btnSearch.Margin = new System.Windows.Forms.Padding(10, 5, 2, 5);
            this._btnSearch.Name = "_btnSearch";
            this._btnSearch.Size = new System.Drawing.Size(110, 36);
            this._btnSearch.SubFont = new System.Drawing.Font("맑은 고딕", 9F);
            this._btnSearch.SubFontColor = System.Drawing.Color.Black;
            this._btnSearch.SubText = "";
            this._btnSearch.TabIndex = 0;
            this._btnSearch.Text = "Search";
            this._btnSearch.TextAlignMain = Sys3Controls.EN_TEXTALIGN.MIDDLE_CENTER;
            this._btnSearch.TextAlignSub = Sys3Controls.EN_TEXTALIGN.MIDDLE_CENTER;
            this._btnSearch.ThemeIndex = 0;
            this._btnSearch.UseBorder = true;
            this._btnSearch.UseClickedEmphasizeTextColor = false;
            this._btnSearch.UseCustomizeClickedColor = true;
            this._btnSearch.UseEdge = true;
            this._btnSearch.UseHoverEmphasizeCustomColor = true;
            this._btnSearch.UseImage = false;
            this._btnSearch.UserHoverEmpahsize = true;
            this._btnSearch.UseSubFont = false;
            this._btnSearch.Click += new System.EventHandler(this.BtnSearchClicked);
            // 
            // _btnReset
            // 
            this._btnReset.BorderWidth = 2;
            this._btnReset.ButtonClicked = false;
            this._btnReset.ClickedEmphasizeTextColor = System.Drawing.Color.White;
            this._btnReset.CustomClickedGradientFirstColor = System.Drawing.Color.BlanchedAlmond;
            this._btnReset.CustomClickedGradientSecondColor = System.Drawing.Color.Gold;
            this._btnReset.Description = "";
            this._btnReset.DisabledColor = System.Drawing.Color.Silver;
            this._btnReset.EdgeRadius = 5;
            this._btnReset.GradientAngle = 60F;
            this._btnReset.GradientFirstColor = System.Drawing.Color.Silver;
            this._btnReset.GradientSecondColor = System.Drawing.Color.Gray;
            this._btnReset.HoverEmphasizeCustomColor = System.Drawing.Color.Firebrick;
            this._btnReset.ImagePosition = new System.Drawing.Point(0, 0);
            this._btnReset.ImageSize = new System.Drawing.Point(0, 0);
            this._btnReset.LoadImage = null;
            this._btnReset.Location = new System.Drawing.Point(132, 5);
            this._btnReset.MainFont = new System.Drawing.Font("맑은 고딕", 11F, System.Drawing.FontStyle.Bold);
            this._btnReset.MainFontColor = System.Drawing.Color.White;
            this._btnReset.Margin = new System.Windows.Forms.Padding(10, 5, 2, 5);
            this._btnReset.Name = "_btnReset";
            this._btnReset.Size = new System.Drawing.Size(110, 36);
            this._btnReset.SubFont = new System.Drawing.Font("맑은 고딕", 9F);
            this._btnReset.SubFontColor = System.Drawing.Color.Black;
            this._btnReset.SubText = "";
            this._btnReset.TabIndex = 1;
            this._btnReset.Text = "Reset";
            this._btnReset.TextAlignMain = Sys3Controls.EN_TEXTALIGN.MIDDLE_CENTER;
            this._btnReset.TextAlignSub = Sys3Controls.EN_TEXTALIGN.MIDDLE_CENTER;
            this._btnReset.ThemeIndex = 0;
            this._btnReset.UseBorder = true;
            this._btnReset.UseClickedEmphasizeTextColor = false;
            this._btnReset.UseCustomizeClickedColor = true;
            this._btnReset.UseEdge = true;
            this._btnReset.UseHoverEmphasizeCustomColor = true;
            this._btnReset.UseImage = false;
            this._btnReset.UserHoverEmpahsize = true;
            this._btnReset.UseSubFont = false;
            this._btnReset.Click += new System.EventHandler(this.BtnResetClicked);
            // 
            // _btnExport
            // 
            this._btnExport.BorderWidth = 2;
            this._btnExport.ButtonClicked = false;
            this._btnExport.ClickedEmphasizeTextColor = System.Drawing.Color.White;
            this._btnExport.CustomClickedGradientFirstColor = System.Drawing.Color.BlanchedAlmond;
            this._btnExport.CustomClickedGradientSecondColor = System.Drawing.Color.Gold;
            this._btnExport.Description = "";
            this._btnExport.DisabledColor = System.Drawing.Color.Silver;
            this._btnExport.EdgeRadius = 5;
            this._btnExport.GradientAngle = 60F;
            this._btnExport.GradientFirstColor = System.Drawing.Color.LightSkyBlue;
            this._btnExport.GradientSecondColor = System.Drawing.Color.SteelBlue;
            this._btnExport.HoverEmphasizeCustomColor = System.Drawing.Color.Firebrick;
            this._btnExport.ImagePosition = new System.Drawing.Point(0, 0);
            this._btnExport.ImageSize = new System.Drawing.Point(0, 0);
            this._btnExport.LoadImage = null;
            this._btnExport.Location = new System.Drawing.Point(254, 5);
            this._btnExport.MainFont = new System.Drawing.Font("맑은 고딕", 11F, System.Drawing.FontStyle.Bold);
            this._btnExport.MainFontColor = System.Drawing.Color.White;
            this._btnExport.Margin = new System.Windows.Forms.Padding(10, 5, 2, 5);
            this._btnExport.Name = "_btnExport";
            this._btnExport.Size = new System.Drawing.Size(150, 36);
            this._btnExport.SubFont = new System.Drawing.Font("맑은 고딕", 9F);
            this._btnExport.SubFontColor = System.Drawing.Color.Black;
            this._btnExport.SubText = "";
            this._btnExport.TabIndex = 2;
            this._btnExport.Text = "Export CSV";
            this._btnExport.TextAlignMain = Sys3Controls.EN_TEXTALIGN.MIDDLE_CENTER;
            this._btnExport.TextAlignSub = Sys3Controls.EN_TEXTALIGN.MIDDLE_CENTER;
            this._btnExport.ThemeIndex = 0;
            this._btnExport.UseBorder = true;
            this._btnExport.UseClickedEmphasizeTextColor = false;
            this._btnExport.UseCustomizeClickedColor = true;
            this._btnExport.UseEdge = true;
            this._btnExport.UseHoverEmphasizeCustomColor = true;
            this._btnExport.UseImage = false;
            this._btnExport.UserHoverEmpahsize = true;
            this._btnExport.UseSubFont = false;
            this._btnExport.Click += new System.EventHandler(this.BtnExportClicked);
            // 
            // _lblUnavailable
            // 
            this._lblUnavailable.BackGroundColor = System.Drawing.Color.WhiteSmoke;
            this._lblUnavailable.BorderStroke = 2;
            this._lblUnavailable.BorderStyle = System.Windows.Forms.ButtonBorderStyle.Solid;
            this._lblUnavailable.Description = "";
            this._lblUnavailable.DisabledColor = System.Drawing.Color.DarkGray;
            this._lblUnavailable.EdgeRadius = 1;
            this._lblUnavailable.ImagePosition = new System.Drawing.Point(0, 0);
            this._lblUnavailable.ImageSize = new System.Drawing.Point(0, 0);
            this._lblUnavailable.LoadImage = null;
            this._lblUnavailable.Location = new System.Drawing.Point(416, 5);
            this._lblUnavailable.MainFont = new System.Drawing.Font("맑은 고딕", 10F, System.Drawing.FontStyle.Bold);
            this._lblUnavailable.MainFontColor = System.Drawing.Color.DarkRed;
            this._lblUnavailable.Margin = new System.Windows.Forms.Padding(10, 5, 2, 5);
            this._lblUnavailable.Name = "_lblUnavailable";
            this._lblUnavailable.Size = new System.Drawing.Size(180, 34);
            this._lblUnavailable.SubFont = new System.Drawing.Font("맑은 고딕", 10F);
            this._lblUnavailable.SubFontColor = System.Drawing.Color.Black;
            this._lblUnavailable.SubText = "";
            this._lblUnavailable.TabIndex = 3;
            this._lblUnavailable.Text = "DB Unavailable";
            this._lblUnavailable.TextAlignMain = Sys3Controls.EN_TEXTALIGN.MIDDLE_CENTER;
            this._lblUnavailable.TextAlignSub = Sys3Controls.EN_TEXTALIGN.MIDDLE_CENTER;
            this._lblUnavailable.TextAlignUnit = Sys3Controls.EN_TEXTALIGN.MIDDLE_CENTER;
            this._lblUnavailable.ThemeIndex = 0;
            this._lblUnavailable.UnitAreaRate = 30;
            this._lblUnavailable.UnitFont = new System.Drawing.Font("맑은 고딕", 10F, System.Drawing.FontStyle.Bold);
            this._lblUnavailable.UnitFontColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this._lblUnavailable.UnitPositionVertical = false;
            this._lblUnavailable.UnitText = "";
            this._lblUnavailable.UseBorder = true;
            this._lblUnavailable.UseEdgeRadius = false;
            this._lblUnavailable.UseImage = false;
            this._lblUnavailable.UseSubFont = false;
            this._lblUnavailable.UseUnitFont = false;
            this._lblUnavailable.Visible = false;
            // 
            // _tlpConditions
            // 
            this._tlpConditions.ColumnCount = 10;
            this._tlpSearchBar.SetColumnSpan(this._tlpConditions, 3);
            this._tlpConditions.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 80F));
            this._tlpConditions.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 140F));
            this._tlpConditions.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 70F));
            this._tlpConditions.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 150F));
            this._tlpConditions.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 70F));
            this._tlpConditions.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 150F));
            this._tlpConditions.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 70F));
            this._tlpConditions.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 150F));
            this._tlpConditions.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 80F));
            this._tlpConditions.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this._tlpConditions.Controls.Add(this._lblCaptionTarget, 0, 0);
            this._tlpConditions.Controls.Add(this._lblTargetValue, 1, 0);
            this._tlpConditions.Controls.Add(this._lblCond1Caption, 2, 0);
            this._tlpConditions.Controls.Add(this._lblCond1Value, 3, 0);
            this._tlpConditions.Controls.Add(this._lblCond2Caption, 4, 0);
            this._tlpConditions.Controls.Add(this._lblCond2Value, 5, 0);
            this._tlpConditions.Controls.Add(this._lblOtherCaption, 6, 0);
            this._tlpConditions.Controls.Add(this._lblOtherFieldValue, 7, 0);
            this._tlpConditions.Controls.Add(this._lblOtherMatchToggle, 8, 0);
            this._tlpConditions.Controls.Add(this._lblOtherValueValue, 9, 0);
            this._tlpConditions.Dock = System.Windows.Forms.DockStyle.Fill;
            this._tlpConditions.Location = new System.Drawing.Point(0, 72);
            this._tlpConditions.Margin = new System.Windows.Forms.Padding(0);
            this._tlpConditions.Name = "_tlpConditions";
            this._tlpConditions.RowCount = 1;
            this._tlpConditions.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this._tlpConditions.Size = new System.Drawing.Size(1128, 66);
            this._tlpConditions.TabIndex = 2;
            // 
            // _lblCaptionTarget
            // 
            this._lblCaptionTarget.BackGroundColor = System.Drawing.Color.LightSteelBlue;
            this._lblCaptionTarget.BorderStroke = 2;
            this._lblCaptionTarget.BorderStyle = System.Windows.Forms.ButtonBorderStyle.Solid;
            this._lblCaptionTarget.Description = "";
            this._lblCaptionTarget.DisabledColor = System.Drawing.Color.DarkGray;
            this._lblCaptionTarget.Dock = System.Windows.Forms.DockStyle.Fill;
            this._lblCaptionTarget.EdgeRadius = 1;
            this._lblCaptionTarget.ImagePosition = new System.Drawing.Point(0, 0);
            this._lblCaptionTarget.ImageSize = new System.Drawing.Point(0, 0);
            this._lblCaptionTarget.LoadImage = null;
            this._lblCaptionTarget.Location = new System.Drawing.Point(4, 4);
            this._lblCaptionTarget.MainFont = new System.Drawing.Font("맑은 고딕", 10F, System.Drawing.FontStyle.Bold);
            this._lblCaptionTarget.MainFontColor = System.Drawing.Color.Black;
            this._lblCaptionTarget.Margin = new System.Windows.Forms.Padding(4);
            this._lblCaptionTarget.Name = "_lblCaptionTarget";
            this._lblCaptionTarget.Size = new System.Drawing.Size(72, 58);
            this._lblCaptionTarget.SubFont = new System.Drawing.Font("맑은 고딕", 10F);
            this._lblCaptionTarget.SubFontColor = System.Drawing.Color.Black;
            this._lblCaptionTarget.SubText = "";
            this._lblCaptionTarget.TabIndex = 0;
            this._lblCaptionTarget.Text = "Target";
            this._lblCaptionTarget.TextAlignMain = Sys3Controls.EN_TEXTALIGN.MIDDLE_CENTER;
            this._lblCaptionTarget.TextAlignSub = Sys3Controls.EN_TEXTALIGN.MIDDLE_CENTER;
            this._lblCaptionTarget.TextAlignUnit = Sys3Controls.EN_TEXTALIGN.MIDDLE_CENTER;
            this._lblCaptionTarget.ThemeIndex = 0;
            this._lblCaptionTarget.UnitAreaRate = 30;
            this._lblCaptionTarget.UnitFont = new System.Drawing.Font("맑은 고딕", 10F, System.Drawing.FontStyle.Bold);
            this._lblCaptionTarget.UnitFontColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this._lblCaptionTarget.UnitPositionVertical = false;
            this._lblCaptionTarget.UnitText = "";
            this._lblCaptionTarget.UseBorder = true;
            this._lblCaptionTarget.UseEdgeRadius = false;
            this._lblCaptionTarget.UseImage = false;
            this._lblCaptionTarget.UseSubFont = false;
            this._lblCaptionTarget.UseUnitFont = false;
            // 
            // _lblTargetValue
            // 
            this._lblTargetValue.BackGroundColor = System.Drawing.Color.White;
            this._lblTargetValue.BorderStroke = 2;
            this._lblTargetValue.BorderStyle = System.Windows.Forms.ButtonBorderStyle.Solid;
            this._lblTargetValue.Description = "";
            this._lblTargetValue.DisabledColor = System.Drawing.Color.DarkGray;
            this._lblTargetValue.Dock = System.Windows.Forms.DockStyle.Fill;
            this._lblTargetValue.EdgeRadius = 1;
            this._lblTargetValue.ImagePosition = new System.Drawing.Point(0, 0);
            this._lblTargetValue.ImageSize = new System.Drawing.Point(0, 0);
            this._lblTargetValue.LoadImage = null;
            this._lblTargetValue.Location = new System.Drawing.Point(84, 4);
            this._lblTargetValue.MainFont = new System.Drawing.Font("맑은 고딕", 10F);
            this._lblTargetValue.MainFontColor = System.Drawing.Color.Black;
            this._lblTargetValue.Margin = new System.Windows.Forms.Padding(4);
            this._lblTargetValue.Name = "_lblTargetValue";
            this._lblTargetValue.Size = new System.Drawing.Size(132, 58);
            this._lblTargetValue.SubFont = new System.Drawing.Font("맑은 고딕", 10F);
            this._lblTargetValue.SubFontColor = System.Drawing.Color.Black;
            this._lblTargetValue.SubText = "";
            this._lblTargetValue.TabIndex = 1;
            this._lblTargetValue.TextAlignMain = Sys3Controls.EN_TEXTALIGN.MIDDLE_CENTER;
            this._lblTargetValue.TextAlignSub = Sys3Controls.EN_TEXTALIGN.MIDDLE_CENTER;
            this._lblTargetValue.TextAlignUnit = Sys3Controls.EN_TEXTALIGN.MIDDLE_CENTER;
            this._lblTargetValue.ThemeIndex = 0;
            this._lblTargetValue.UnitAreaRate = 30;
            this._lblTargetValue.UnitFont = new System.Drawing.Font("맑은 고딕", 10F, System.Drawing.FontStyle.Bold);
            this._lblTargetValue.UnitFontColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this._lblTargetValue.UnitPositionVertical = false;
            this._lblTargetValue.UnitText = "";
            this._lblTargetValue.UseBorder = true;
            this._lblTargetValue.UseEdgeRadius = false;
            this._lblTargetValue.UseImage = false;
            this._lblTargetValue.UseSubFont = false;
            this._lblTargetValue.UseUnitFont = false;
            this._lblTargetValue.Click += new System.EventHandler(this.TargetValueClicked);
            // 
            // _lblCond1Caption
            // 
            this._lblCond1Caption.BackGroundColor = System.Drawing.Color.LightSteelBlue;
            this._lblCond1Caption.BorderStroke = 2;
            this._lblCond1Caption.BorderStyle = System.Windows.Forms.ButtonBorderStyle.Solid;
            this._lblCond1Caption.Description = "";
            this._lblCond1Caption.DisabledColor = System.Drawing.Color.DarkGray;
            this._lblCond1Caption.Dock = System.Windows.Forms.DockStyle.Fill;
            this._lblCond1Caption.EdgeRadius = 1;
            this._lblCond1Caption.ImagePosition = new System.Drawing.Point(0, 0);
            this._lblCond1Caption.ImageSize = new System.Drawing.Point(0, 0);
            this._lblCond1Caption.LoadImage = null;
            this._lblCond1Caption.Location = new System.Drawing.Point(224, 4);
            this._lblCond1Caption.MainFont = new System.Drawing.Font("맑은 고딕", 10F, System.Drawing.FontStyle.Bold);
            this._lblCond1Caption.MainFontColor = System.Drawing.Color.Black;
            this._lblCond1Caption.Margin = new System.Windows.Forms.Padding(4);
            this._lblCond1Caption.Name = "_lblCond1Caption";
            this._lblCond1Caption.Size = new System.Drawing.Size(62, 58);
            this._lblCond1Caption.SubFont = new System.Drawing.Font("맑은 고딕", 10F);
            this._lblCond1Caption.SubFontColor = System.Drawing.Color.Black;
            this._lblCond1Caption.SubText = "";
            this._lblCond1Caption.TabIndex = 2;
            this._lblCond1Caption.Text = "Name";
            this._lblCond1Caption.TextAlignMain = Sys3Controls.EN_TEXTALIGN.MIDDLE_CENTER;
            this._lblCond1Caption.TextAlignSub = Sys3Controls.EN_TEXTALIGN.MIDDLE_CENTER;
            this._lblCond1Caption.TextAlignUnit = Sys3Controls.EN_TEXTALIGN.MIDDLE_CENTER;
            this._lblCond1Caption.ThemeIndex = 0;
            this._lblCond1Caption.UnitAreaRate = 30;
            this._lblCond1Caption.UnitFont = new System.Drawing.Font("맑은 고딕", 10F, System.Drawing.FontStyle.Bold);
            this._lblCond1Caption.UnitFontColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this._lblCond1Caption.UnitPositionVertical = false;
            this._lblCond1Caption.UnitText = "";
            this._lblCond1Caption.UseBorder = true;
            this._lblCond1Caption.UseEdgeRadius = false;
            this._lblCond1Caption.UseImage = false;
            this._lblCond1Caption.UseSubFont = false;
            this._lblCond1Caption.UseUnitFont = false;
            // 
            // _lblCond1Value
            // 
            this._lblCond1Value.BackGroundColor = System.Drawing.Color.White;
            this._lblCond1Value.BorderStroke = 2;
            this._lblCond1Value.BorderStyle = System.Windows.Forms.ButtonBorderStyle.Solid;
            this._lblCond1Value.Description = "";
            this._lblCond1Value.DisabledColor = System.Drawing.Color.DarkGray;
            this._lblCond1Value.Dock = System.Windows.Forms.DockStyle.Fill;
            this._lblCond1Value.EdgeRadius = 1;
            this._lblCond1Value.ImagePosition = new System.Drawing.Point(0, 0);
            this._lblCond1Value.ImageSize = new System.Drawing.Point(0, 0);
            this._lblCond1Value.LoadImage = null;
            this._lblCond1Value.Location = new System.Drawing.Point(294, 4);
            this._lblCond1Value.MainFont = new System.Drawing.Font("맑은 고딕", 10F);
            this._lblCond1Value.MainFontColor = System.Drawing.Color.Black;
            this._lblCond1Value.Margin = new System.Windows.Forms.Padding(4);
            this._lblCond1Value.Name = "_lblCond1Value";
            this._lblCond1Value.Size = new System.Drawing.Size(142, 58);
            this._lblCond1Value.SubFont = new System.Drawing.Font("맑은 고딕", 10F);
            this._lblCond1Value.SubFontColor = System.Drawing.Color.Black;
            this._lblCond1Value.SubText = "";
            this._lblCond1Value.TabIndex = 3;
            this._lblCond1Value.TextAlignMain = Sys3Controls.EN_TEXTALIGN.MIDDLE_CENTER;
            this._lblCond1Value.TextAlignSub = Sys3Controls.EN_TEXTALIGN.MIDDLE_CENTER;
            this._lblCond1Value.TextAlignUnit = Sys3Controls.EN_TEXTALIGN.MIDDLE_CENTER;
            this._lblCond1Value.ThemeIndex = 0;
            this._lblCond1Value.UnitAreaRate = 30;
            this._lblCond1Value.UnitFont = new System.Drawing.Font("맑은 고딕", 10F, System.Drawing.FontStyle.Bold);
            this._lblCond1Value.UnitFontColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this._lblCond1Value.UnitPositionVertical = false;
            this._lblCond1Value.UnitText = "";
            this._lblCond1Value.UseBorder = true;
            this._lblCond1Value.UseEdgeRadius = false;
            this._lblCond1Value.UseImage = false;
            this._lblCond1Value.UseSubFont = false;
            this._lblCond1Value.UseUnitFont = false;
            this._lblCond1Value.Click += new System.EventHandler(this.CondValueClicked);
            // 
            // _lblCond2Caption
            // 
            this._lblCond2Caption.BackGroundColor = System.Drawing.Color.LightSteelBlue;
            this._lblCond2Caption.BorderStroke = 2;
            this._lblCond2Caption.BorderStyle = System.Windows.Forms.ButtonBorderStyle.Solid;
            this._lblCond2Caption.Description = "";
            this._lblCond2Caption.DisabledColor = System.Drawing.Color.DarkGray;
            this._lblCond2Caption.Dock = System.Windows.Forms.DockStyle.Fill;
            this._lblCond2Caption.EdgeRadius = 1;
            this._lblCond2Caption.ImagePosition = new System.Drawing.Point(0, 0);
            this._lblCond2Caption.ImageSize = new System.Drawing.Point(0, 0);
            this._lblCond2Caption.LoadImage = null;
            this._lblCond2Caption.Location = new System.Drawing.Point(444, 4);
            this._lblCond2Caption.MainFont = new System.Drawing.Font("맑은 고딕", 10F, System.Drawing.FontStyle.Bold);
            this._lblCond2Caption.MainFontColor = System.Drawing.Color.Black;
            this._lblCond2Caption.Margin = new System.Windows.Forms.Padding(4);
            this._lblCond2Caption.Name = "_lblCond2Caption";
            this._lblCond2Caption.Size = new System.Drawing.Size(62, 58);
            this._lblCond2Caption.SubFont = new System.Drawing.Font("맑은 고딕", 10F);
            this._lblCond2Caption.SubFontColor = System.Drawing.Color.Black;
            this._lblCond2Caption.SubText = "";
            this._lblCond2Caption.TabIndex = 4;
            this._lblCond2Caption.Text = "Lot ID";
            this._lblCond2Caption.TextAlignMain = Sys3Controls.EN_TEXTALIGN.MIDDLE_CENTER;
            this._lblCond2Caption.TextAlignSub = Sys3Controls.EN_TEXTALIGN.MIDDLE_CENTER;
            this._lblCond2Caption.TextAlignUnit = Sys3Controls.EN_TEXTALIGN.MIDDLE_CENTER;
            this._lblCond2Caption.ThemeIndex = 0;
            this._lblCond2Caption.UnitAreaRate = 30;
            this._lblCond2Caption.UnitFont = new System.Drawing.Font("맑은 고딕", 10F, System.Drawing.FontStyle.Bold);
            this._lblCond2Caption.UnitFontColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this._lblCond2Caption.UnitPositionVertical = false;
            this._lblCond2Caption.UnitText = "";
            this._lblCond2Caption.UseBorder = true;
            this._lblCond2Caption.UseEdgeRadius = false;
            this._lblCond2Caption.UseImage = false;
            this._lblCond2Caption.UseSubFont = false;
            this._lblCond2Caption.UseUnitFont = false;
            // 
            // _lblCond2Value
            // 
            this._lblCond2Value.BackGroundColor = System.Drawing.Color.White;
            this._lblCond2Value.BorderStroke = 2;
            this._lblCond2Value.BorderStyle = System.Windows.Forms.ButtonBorderStyle.Solid;
            this._lblCond2Value.Description = "";
            this._lblCond2Value.DisabledColor = System.Drawing.Color.DarkGray;
            this._lblCond2Value.Dock = System.Windows.Forms.DockStyle.Fill;
            this._lblCond2Value.EdgeRadius = 1;
            this._lblCond2Value.ImagePosition = new System.Drawing.Point(0, 0);
            this._lblCond2Value.ImageSize = new System.Drawing.Point(0, 0);
            this._lblCond2Value.LoadImage = null;
            this._lblCond2Value.Location = new System.Drawing.Point(514, 4);
            this._lblCond2Value.MainFont = new System.Drawing.Font("맑은 고딕", 10F);
            this._lblCond2Value.MainFontColor = System.Drawing.Color.Black;
            this._lblCond2Value.Margin = new System.Windows.Forms.Padding(4);
            this._lblCond2Value.Name = "_lblCond2Value";
            this._lblCond2Value.Size = new System.Drawing.Size(142, 58);
            this._lblCond2Value.SubFont = new System.Drawing.Font("맑은 고딕", 10F);
            this._lblCond2Value.SubFontColor = System.Drawing.Color.Black;
            this._lblCond2Value.SubText = "";
            this._lblCond2Value.TabIndex = 5;
            this._lblCond2Value.TextAlignMain = Sys3Controls.EN_TEXTALIGN.MIDDLE_CENTER;
            this._lblCond2Value.TextAlignSub = Sys3Controls.EN_TEXTALIGN.MIDDLE_CENTER;
            this._lblCond2Value.TextAlignUnit = Sys3Controls.EN_TEXTALIGN.MIDDLE_CENTER;
            this._lblCond2Value.ThemeIndex = 0;
            this._lblCond2Value.UnitAreaRate = 30;
            this._lblCond2Value.UnitFont = new System.Drawing.Font("맑은 고딕", 10F, System.Drawing.FontStyle.Bold);
            this._lblCond2Value.UnitFontColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this._lblCond2Value.UnitPositionVertical = false;
            this._lblCond2Value.UnitText = "";
            this._lblCond2Value.UseBorder = true;
            this._lblCond2Value.UseEdgeRadius = false;
            this._lblCond2Value.UseImage = false;
            this._lblCond2Value.UseSubFont = false;
            this._lblCond2Value.UseUnitFont = false;
            this._lblCond2Value.Click += new System.EventHandler(this.CondValueClicked);
            // 
            // _lblOtherCaption
            // 
            this._lblOtherCaption.BackGroundColor = System.Drawing.Color.LightSteelBlue;
            this._lblOtherCaption.BorderStroke = 2;
            this._lblOtherCaption.BorderStyle = System.Windows.Forms.ButtonBorderStyle.Solid;
            this._lblOtherCaption.Description = "";
            this._lblOtherCaption.DisabledColor = System.Drawing.Color.DarkGray;
            this._lblOtherCaption.Dock = System.Windows.Forms.DockStyle.Fill;
            this._lblOtherCaption.EdgeRadius = 1;
            this._lblOtherCaption.ImagePosition = new System.Drawing.Point(0, 0);
            this._lblOtherCaption.ImageSize = new System.Drawing.Point(0, 0);
            this._lblOtherCaption.LoadImage = null;
            this._lblOtherCaption.Location = new System.Drawing.Point(664, 4);
            this._lblOtherCaption.MainFont = new System.Drawing.Font("맑은 고딕", 10F, System.Drawing.FontStyle.Bold);
            this._lblOtherCaption.MainFontColor = System.Drawing.Color.Black;
            this._lblOtherCaption.Margin = new System.Windows.Forms.Padding(4);
            this._lblOtherCaption.Name = "_lblOtherCaption";
            this._lblOtherCaption.Size = new System.Drawing.Size(62, 58);
            this._lblOtherCaption.SubFont = new System.Drawing.Font("맑은 고딕", 10F);
            this._lblOtherCaption.SubFontColor = System.Drawing.Color.Black;
            this._lblOtherCaption.SubText = "";
            this._lblOtherCaption.TabIndex = 8;
            this._lblOtherCaption.Text = "Other";
            this._lblOtherCaption.TextAlignMain = Sys3Controls.EN_TEXTALIGN.MIDDLE_CENTER;
            this._lblOtherCaption.TextAlignSub = Sys3Controls.EN_TEXTALIGN.MIDDLE_CENTER;
            this._lblOtherCaption.TextAlignUnit = Sys3Controls.EN_TEXTALIGN.MIDDLE_CENTER;
            this._lblOtherCaption.ThemeIndex = 0;
            this._lblOtherCaption.UnitAreaRate = 30;
            this._lblOtherCaption.UnitFont = new System.Drawing.Font("맑은 고딕", 10F, System.Drawing.FontStyle.Bold);
            this._lblOtherCaption.UnitFontColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this._lblOtherCaption.UnitPositionVertical = false;
            this._lblOtherCaption.UnitText = "";
            this._lblOtherCaption.UseBorder = true;
            this._lblOtherCaption.UseEdgeRadius = false;
            this._lblOtherCaption.UseImage = false;
            this._lblOtherCaption.UseSubFont = false;
            this._lblOtherCaption.UseUnitFont = false;
            // 
            // _lblOtherFieldValue
            // 
            this._lblOtherFieldValue.BackGroundColor = System.Drawing.Color.White;
            this._lblOtherFieldValue.BorderStroke = 2;
            this._lblOtherFieldValue.BorderStyle = System.Windows.Forms.ButtonBorderStyle.Solid;
            this._lblOtherFieldValue.Description = "";
            this._lblOtherFieldValue.DisabledColor = System.Drawing.Color.DarkGray;
            this._lblOtherFieldValue.Dock = System.Windows.Forms.DockStyle.Fill;
            this._lblOtherFieldValue.EdgeRadius = 1;
            this._lblOtherFieldValue.ImagePosition = new System.Drawing.Point(0, 0);
            this._lblOtherFieldValue.ImageSize = new System.Drawing.Point(0, 0);
            this._lblOtherFieldValue.LoadImage = null;
            this._lblOtherFieldValue.Location = new System.Drawing.Point(734, 4);
            this._lblOtherFieldValue.MainFont = new System.Drawing.Font("맑은 고딕", 10F);
            this._lblOtherFieldValue.MainFontColor = System.Drawing.Color.Black;
            this._lblOtherFieldValue.Margin = new System.Windows.Forms.Padding(4);
            this._lblOtherFieldValue.Name = "_lblOtherFieldValue";
            this._lblOtherFieldValue.Size = new System.Drawing.Size(142, 58);
            this._lblOtherFieldValue.SubFont = new System.Drawing.Font("맑은 고딕", 10F);
            this._lblOtherFieldValue.SubFontColor = System.Drawing.Color.Black;
            this._lblOtherFieldValue.SubText = "";
            this._lblOtherFieldValue.TabIndex = 9;
            this._lblOtherFieldValue.TextAlignMain = Sys3Controls.EN_TEXTALIGN.MIDDLE_CENTER;
            this._lblOtherFieldValue.TextAlignSub = Sys3Controls.EN_TEXTALIGN.MIDDLE_CENTER;
            this._lblOtherFieldValue.TextAlignUnit = Sys3Controls.EN_TEXTALIGN.MIDDLE_CENTER;
            this._lblOtherFieldValue.ThemeIndex = 0;
            this._lblOtherFieldValue.UnitAreaRate = 30;
            this._lblOtherFieldValue.UnitFont = new System.Drawing.Font("맑은 고딕", 10F, System.Drawing.FontStyle.Bold);
            this._lblOtherFieldValue.UnitFontColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this._lblOtherFieldValue.UnitPositionVertical = false;
            this._lblOtherFieldValue.UnitText = "";
            this._lblOtherFieldValue.UseBorder = true;
            this._lblOtherFieldValue.UseEdgeRadius = false;
            this._lblOtherFieldValue.UseImage = false;
            this._lblOtherFieldValue.UseSubFont = false;
            this._lblOtherFieldValue.UseUnitFont = false;
            this._lblOtherFieldValue.Click += new System.EventHandler(this.OtherFieldSelectClicked);
            // 
            // _lblOtherMatchToggle
            // 
            this._lblOtherMatchToggle.BackGroundColor = System.Drawing.Color.LightSteelBlue;
            this._lblOtherMatchToggle.BorderStroke = 2;
            this._lblOtherMatchToggle.BorderStyle = System.Windows.Forms.ButtonBorderStyle.Solid;
            this._lblOtherMatchToggle.Description = "";
            this._lblOtherMatchToggle.DisabledColor = System.Drawing.Color.DarkGray;
            this._lblOtherMatchToggle.Dock = System.Windows.Forms.DockStyle.Fill;
            this._lblOtherMatchToggle.EdgeRadius = 1;
            this._lblOtherMatchToggle.ImagePosition = new System.Drawing.Point(0, 0);
            this._lblOtherMatchToggle.ImageSize = new System.Drawing.Point(0, 0);
            this._lblOtherMatchToggle.LoadImage = null;
            this._lblOtherMatchToggle.Location = new System.Drawing.Point(884, 4);
            this._lblOtherMatchToggle.MainFont = new System.Drawing.Font("맑은 고딕", 10F, System.Drawing.FontStyle.Bold);
            this._lblOtherMatchToggle.MainFontColor = System.Drawing.Color.Black;
            this._lblOtherMatchToggle.Margin = new System.Windows.Forms.Padding(4);
            this._lblOtherMatchToggle.Name = "_lblOtherMatchToggle";
            this._lblOtherMatchToggle.Size = new System.Drawing.Size(72, 58);
            this._lblOtherMatchToggle.SubFont = new System.Drawing.Font("맑은 고딕", 10F);
            this._lblOtherMatchToggle.SubFontColor = System.Drawing.Color.Black;
            this._lblOtherMatchToggle.SubText = "";
            this._lblOtherMatchToggle.TabIndex = 10;
            this._lblOtherMatchToggle.Text = "Partial";
            this._lblOtherMatchToggle.TextAlignMain = Sys3Controls.EN_TEXTALIGN.MIDDLE_CENTER;
            this._lblOtherMatchToggle.TextAlignSub = Sys3Controls.EN_TEXTALIGN.MIDDLE_CENTER;
            this._lblOtherMatchToggle.TextAlignUnit = Sys3Controls.EN_TEXTALIGN.MIDDLE_CENTER;
            this._lblOtherMatchToggle.ThemeIndex = 0;
            this._lblOtherMatchToggle.UnitAreaRate = 30;
            this._lblOtherMatchToggle.UnitFont = new System.Drawing.Font("맑은 고딕", 10F, System.Drawing.FontStyle.Bold);
            this._lblOtherMatchToggle.UnitFontColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this._lblOtherMatchToggle.UnitPositionVertical = false;
            this._lblOtherMatchToggle.UnitText = "";
            this._lblOtherMatchToggle.UseBorder = true;
            this._lblOtherMatchToggle.UseEdgeRadius = false;
            this._lblOtherMatchToggle.UseImage = false;
            this._lblOtherMatchToggle.UseSubFont = false;
            this._lblOtherMatchToggle.UseUnitFont = false;
            this._lblOtherMatchToggle.Click += new System.EventHandler(this.OtherMatchToggleClicked);
            // 
            // _lblOtherValueValue
            // 
            this._lblOtherValueValue.BackGroundColor = System.Drawing.Color.White;
            this._lblOtherValueValue.BorderStroke = 2;
            this._lblOtherValueValue.BorderStyle = System.Windows.Forms.ButtonBorderStyle.Solid;
            this._lblOtherValueValue.Description = "";
            this._lblOtherValueValue.DisabledColor = System.Drawing.Color.DarkGray;
            this._lblOtherValueValue.Dock = System.Windows.Forms.DockStyle.Fill;
            this._lblOtherValueValue.EdgeRadius = 1;
            this._lblOtherValueValue.ImagePosition = new System.Drawing.Point(0, 0);
            this._lblOtherValueValue.ImageSize = new System.Drawing.Point(0, 0);
            this._lblOtherValueValue.LoadImage = null;
            this._lblOtherValueValue.Location = new System.Drawing.Point(964, 4);
            this._lblOtherValueValue.MainFont = new System.Drawing.Font("맑은 고딕", 10F);
            this._lblOtherValueValue.MainFontColor = System.Drawing.Color.Black;
            this._lblOtherValueValue.Margin = new System.Windows.Forms.Padding(4);
            this._lblOtherValueValue.Name = "_lblOtherValueValue";
            this._lblOtherValueValue.Size = new System.Drawing.Size(160, 58);
            this._lblOtherValueValue.SubFont = new System.Drawing.Font("맑은 고딕", 10F);
            this._lblOtherValueValue.SubFontColor = System.Drawing.Color.Black;
            this._lblOtherValueValue.SubText = "";
            this._lblOtherValueValue.TabIndex = 11;
            this._lblOtherValueValue.TextAlignMain = Sys3Controls.EN_TEXTALIGN.MIDDLE_CENTER;
            this._lblOtherValueValue.TextAlignSub = Sys3Controls.EN_TEXTALIGN.MIDDLE_CENTER;
            this._lblOtherValueValue.TextAlignUnit = Sys3Controls.EN_TEXTALIGN.MIDDLE_CENTER;
            this._lblOtherValueValue.ThemeIndex = 0;
            this._lblOtherValueValue.UnitAreaRate = 30;
            this._lblOtherValueValue.UnitFont = new System.Drawing.Font("맑은 고딕", 10F, System.Drawing.FontStyle.Bold);
            this._lblOtherValueValue.UnitFontColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this._lblOtherValueValue.UnitPositionVertical = false;
            this._lblOtherValueValue.UnitText = "";
            this._lblOtherValueValue.UseBorder = true;
            this._lblOtherValueValue.UseEdgeRadius = false;
            this._lblOtherValueValue.UseImage = false;
            this._lblOtherValueValue.UseSubFont = false;
            this._lblOtherValueValue.UseUnitFont = false;
            this._lblOtherValueValue.Click += new System.EventHandler(this.CondValueClicked);
            // 
            // tableLayoutPanelContent
            // 
            this.tableLayoutPanelContent.ColumnCount = 1;
            this.tableLayoutPanelContent.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanelContent.Controls.Add(this.gvResults, 0, 0);
            this.tableLayoutPanelContent.Controls.Add(this.pnlDetail, 0, 1);
            this.tableLayoutPanelContent.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanelContent.Location = new System.Drawing.Point(0, 150);
            this.tableLayoutPanelContent.Margin = new System.Windows.Forms.Padding(0);
            this.tableLayoutPanelContent.Name = "tableLayoutPanelContent";
            this.tableLayoutPanelContent.RowCount = 2;
            this.tableLayoutPanelContent.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 35F));
            this.tableLayoutPanelContent.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 65F));
            this.tableLayoutPanelContent.Size = new System.Drawing.Size(1140, 750);
            this.tableLayoutPanelContent.TabIndex = 1;
            // 
            // gvResults
            // 
            this.gvResults.AllowUserToAddRows = false;
            this.gvResults.AllowUserToDeleteRows = false;
            this.gvResults.AllowUserToResizeRows = false;
            this.gvResults.BackgroundColor = System.Drawing.Color.White;
            this.gvResults.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.gvResults.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gvResults.EditMode = System.Windows.Forms.DataGridViewEditMode.EditProgrammatically;
            this.gvResults.Location = new System.Drawing.Point(3, 3);
            this.gvResults.MultiSelect = false;
            this.gvResults.Name = "gvResults";
            this.gvResults.RowHeadersVisible = false;
            this.gvResults.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.gvResults.Size = new System.Drawing.Size(1134, 256);
            this.gvResults.TabIndex = 0;
            this.gvResults.SelectionChanged += new System.EventHandler(this.GvResultsSelectionChanged);
            // 
            // pnlDetail
            // 
            this.pnlDetail.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlDetail.Location = new System.Drawing.Point(0, 268);
            this.pnlDetail.Margin = new System.Windows.Forms.Padding(0, 6, 0, 0);
            this.pnlDetail.Name = "pnlDetail";
            this.pnlDetail.Size = new System.Drawing.Size(1140, 482);
            this.pnlDetail.TabIndex = 1;
            // 
            // SubViewOperationHistoryDatabase
            // 
            this.BackColor = System.Drawing.Color.WhiteSmoke;
            this.Controls.Add(this.tableLayoutPanelRoot);
            this.Margin = new System.Windows.Forms.Padding(0);
            this.Name = "SubViewOperationHistoryDatabase";
            this.Size = new System.Drawing.Size(1140, 900);
            this.tableLayoutPanelRoot.ResumeLayout(false);
            this.pnlSearch.ResumeLayout(false);
            this._tlpSearchBar.ResumeLayout(false);
            this._pnlStartDateHolder.ResumeLayout(false);
            this._pnlEndDateHolder.ResumeLayout(false);
            this._pnlButtonRow.ResumeLayout(false);
            this._tlpConditions.ResumeLayout(false);
            this.tableLayoutPanelContent.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.gvResults)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel tableLayoutPanelRoot;
        private System.Windows.Forms.Panel pnlSearch;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanelContent;
        private Sys3Controls.Sys3DoubleBufferedDataGridView gvResults;
        private System.Windows.Forms.Panel pnlDetail;

        // 검색바
        private System.Windows.Forms.TableLayoutPanel _tlpSearchBar;
        private System.Windows.Forms.Panel _pnlStartDateHolder;
        private Sys3Controls.Sys3Label _lblStartDateHeader;
        private System.Windows.Forms.DateTimePicker _startDatePicker;
        private System.Windows.Forms.Panel _pnlEndDateHolder;
        private Sys3Controls.Sys3Label _lblEndDateHeader;
        private System.Windows.Forms.DateTimePicker _endDatePicker;
        private System.Windows.Forms.TableLayoutPanel _tlpConditions;
        private Sys3Controls.Sys3Label _lblCaptionTarget;
        private Sys3Controls.Sys3Label _lblTargetValue;
        private Sys3Controls.Sys3Label _lblCond1Caption;
        private Sys3Controls.Sys3Label _lblCond1Value;
        private Sys3Controls.Sys3Label _lblCond2Caption;
        private Sys3Controls.Sys3Label _lblCond2Value;
        private Sys3Controls.Sys3Label _lblOtherCaption;
        private Sys3Controls.Sys3Label _lblOtherFieldValue;
        private Sys3Controls.Sys3Label _lblOtherMatchToggle;
        private Sys3Controls.Sys3Label _lblOtherValueValue;
        private System.Windows.Forms.FlowLayoutPanel _pnlButtonRow;
        private Sys3Controls.Sys3button _btnSearch;
        private Sys3Controls.Sys3button _btnReset;
        private Sys3Controls.Sys3button _btnExport;
        private Sys3Controls.Sys3Label _lblUnavailable;
    }
}
