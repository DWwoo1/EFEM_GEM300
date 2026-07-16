namespace EFEM.CustomizedByProcessType.UserInterface.OperationMainHistory.PWA500Common
{
    partial class SubstrateDetailView
    {
        private System.ComponentModel.IContainer components = null;

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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(SubstrateDetailView));
            this._tlpSubstrateDetailWrapper = new System.Windows.Forms.TableLayoutPanel();
            this._pnlSubstratePageBar = new System.Windows.Forms.TableLayoutPanel();
            this.btnReturnToCarrier = new Sys3Controls.Sys3button();
            this.btnProcessingHistory = new Sys3Controls.Sys3button();
            this.btnLocationHistory = new Sys3Controls.Sys3button();
            this.btnLotHistory = new Sys3Controls.Sys3button();
            this.btnExtra = new Sys3Controls.Sys3button();
            this._pnlSubstratePagesHost = new System.Windows.Forms.Panel();
            this._pnlSubstratePageExtra = new System.Windows.Forms.Panel();
            this._pnlSubstrateExtraSection = new System.Windows.Forms.Panel();
            this._gvSubstrateExtra = new Sys3Controls.Sys3DoubleBufferedDataGridView();
            this._colSubstrateExtraKey = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this._colSubstrateExtraValue = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this._lblSubstrateExtraSectionHeader = new Sys3Controls.Sys3Label();
            this._pnlSubstratePageHistory = new System.Windows.Forms.Panel();
            this._gvSubstrateLotHistory = new Sys3Controls.Sys3DoubleBufferedDataGridView();
            this._colSubHistTime = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this._colSubHistPortEvent = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this._colSubHistWafer = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this._colSubHistWaferEvent = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this._colSubHistMessage = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this._pnlSubstratePageProcessing = new System.Windows.Forms.Panel();
            this._gvProcessing = new Sys3Controls.Sys3DoubleBufferedDataGridView();
            this._colProcTime = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this._colProcOld = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this._colProcNew = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this._colProcControlJob = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this._colProcProcessJob = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this._colProcLocation = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this._colProcDesc = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this._pnlSubstratePageLocation = new System.Windows.Forms.Panel();
            this._gvLocation = new Sys3Controls.Sys3DoubleBufferedDataGridView();
            this._colLocTime = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this._colLocFrom = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this._colLocFromKind = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this._colLocTo = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this._colLocToKind = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this._colLocReason = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this._tlpSubstrateDetailWrapper.SuspendLayout();
            this._pnlSubstratePageBar.SuspendLayout();
            this._pnlSubstratePagesHost.SuspendLayout();
            this._pnlSubstratePageExtra.SuspendLayout();
            this._pnlSubstrateExtraSection.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this._gvSubstrateExtra)).BeginInit();
            this._pnlSubstratePageHistory.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this._gvSubstrateLotHistory)).BeginInit();
            this._pnlSubstratePageProcessing.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this._gvProcessing)).BeginInit();
            this._pnlSubstratePageLocation.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this._gvLocation)).BeginInit();
            this.SuspendLayout();
            // 
            // _tlpSubstrateDetailWrapper
            // 
            this._tlpSubstrateDetailWrapper.ColumnCount = 1;
            this._tlpSubstrateDetailWrapper.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this._tlpSubstrateDetailWrapper.Controls.Add(this._pnlSubstratePageBar, 0, 0);
            this._tlpSubstrateDetailWrapper.Controls.Add(this._pnlSubstratePagesHost, 0, 1);
            this._tlpSubstrateDetailWrapper.Dock = System.Windows.Forms.DockStyle.Fill;
            this._tlpSubstrateDetailWrapper.Location = new System.Drawing.Point(0, 0);
            this._tlpSubstrateDetailWrapper.Name = "_tlpSubstrateDetailWrapper";
            this._tlpSubstrateDetailWrapper.RowCount = 2;
            this._tlpSubstrateDetailWrapper.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 40F));
            this._tlpSubstrateDetailWrapper.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this._tlpSubstrateDetailWrapper.Size = new System.Drawing.Size(1140, 384);
            this._tlpSubstrateDetailWrapper.TabIndex = 0;
            // 
            // _pnlSubstratePageBar
            // 
            this._pnlSubstratePageBar.ColumnCount = 5;
            this._pnlSubstratePageBar.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 210F));
            this._pnlSubstratePageBar.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 210F));
            this._pnlSubstratePageBar.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 210F));
            this._pnlSubstratePageBar.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 210F));
            this._pnlSubstratePageBar.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this._pnlSubstratePageBar.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this._pnlSubstratePageBar.Controls.Add(this.btnReturnToCarrier, 4, 0);
            this._pnlSubstratePageBar.Controls.Add(this.btnProcessingHistory, 3, 0);
            this._pnlSubstratePageBar.Controls.Add(this.btnLocationHistory, 2, 0);
            this._pnlSubstratePageBar.Controls.Add(this.btnLotHistory, 1, 0);
            this._pnlSubstratePageBar.Controls.Add(this.btnExtra, 0, 0);
            this._pnlSubstratePageBar.Dock = System.Windows.Forms.DockStyle.Fill;
            this._pnlSubstratePageBar.Location = new System.Drawing.Point(3, 3);
            this._pnlSubstratePageBar.Name = "_pnlSubstratePageBar";
            this._pnlSubstratePageBar.RowCount = 1;
            this._pnlSubstratePageBar.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this._pnlSubstratePageBar.Size = new System.Drawing.Size(1134, 34);
            this._pnlSubstratePageBar.TabIndex = 3;
            // 
            // btnReturnToCarrier
            // 
            this.btnReturnToCarrier.BorderWidth = 2;
            this.btnReturnToCarrier.ButtonClicked = false;
            this.btnReturnToCarrier.ClickedEmphasizeTextColor = System.Drawing.Color.White;
            this.btnReturnToCarrier.CustomClickedGradientFirstColor = System.Drawing.Color.BlanchedAlmond;
            this.btnReturnToCarrier.CustomClickedGradientSecondColor = System.Drawing.Color.Gold;
            this.btnReturnToCarrier.Description = "";
            this.btnReturnToCarrier.DisabledColor = System.Drawing.Color.Silver;
            this.btnReturnToCarrier.Dock = System.Windows.Forms.DockStyle.Right;
            this.btnReturnToCarrier.EdgeRadius = 5;
            this.btnReturnToCarrier.GradientAngle = 70F;
            this.btnReturnToCarrier.GradientFirstColor = System.Drawing.Color.Goldenrod;
            this.btnReturnToCarrier.GradientSecondColor = System.Drawing.Color.DarkGoldenrod;
            this.btnReturnToCarrier.HoverEmphasizeCustomColor = System.Drawing.Color.Firebrick;
            this.btnReturnToCarrier.ImagePosition = new System.Drawing.Point(10, 10);
            this.btnReturnToCarrier.ImageSize = new System.Drawing.Point(15, 15);
            this.btnReturnToCarrier.LoadImage = ((System.Drawing.Image)(resources.GetObject("btnReturnToCarrier.LoadImage")));
            this.btnReturnToCarrier.Location = new System.Drawing.Point(922, 2);
            this.btnReturnToCarrier.MainFont = new System.Drawing.Font("맑은 고딕", 12F, System.Drawing.FontStyle.Bold);
            this.btnReturnToCarrier.MainFontColor = System.Drawing.Color.White;
            this.btnReturnToCarrier.Margin = new System.Windows.Forms.Padding(2);
            this.btnReturnToCarrier.Name = "btnReturnToCarrier";
            this.btnReturnToCarrier.Size = new System.Drawing.Size(210, 30);
            this.btnReturnToCarrier.SubFont = new System.Drawing.Font("맑은 고딕", 10F);
            this.btnReturnToCarrier.SubFontColor = System.Drawing.Color.SeaGreen;
            this.btnReturnToCarrier.SubText = "";
            this.btnReturnToCarrier.TabIndex = 7;
            this.btnReturnToCarrier.Tag = "";
            this.btnReturnToCarrier.Text = "Return To Carrier";
            this.btnReturnToCarrier.TextAlignMain = Sys3Controls.EN_TEXTALIGN.MIDDLE_CENTER;
            this.btnReturnToCarrier.TextAlignSub = Sys3Controls.EN_TEXTALIGN.MIDDLE_CENTER;
            this.btnReturnToCarrier.ThemeIndex = 0;
            this.btnReturnToCarrier.UseBorder = false;
            this.btnReturnToCarrier.UseClickedEmphasizeTextColor = false;
            this.btnReturnToCarrier.UseCustomizeClickedColor = true;
            this.btnReturnToCarrier.UseEdge = false;
            this.btnReturnToCarrier.UseHoverEmphasizeCustomColor = false;
            this.btnReturnToCarrier.UseImage = false;
            this.btnReturnToCarrier.UserHoverEmpahsize = false;
            this.btnReturnToCarrier.UseSubFont = false;
            this.btnReturnToCarrier.Click += new System.EventHandler(this.BtnGoToCarrierClicked);
            // 
            // btnProcessingHistory
            // 
            this.btnProcessingHistory.BorderWidth = 2;
            this.btnProcessingHistory.ButtonClicked = false;
            this.btnProcessingHistory.ClickedEmphasizeTextColor = System.Drawing.Color.White;
            this.btnProcessingHistory.CustomClickedGradientFirstColor = System.Drawing.Color.SeaGreen;
            this.btnProcessingHistory.CustomClickedGradientSecondColor = System.Drawing.Color.SeaGreen;
            this.btnProcessingHistory.Description = "";
            this.btnProcessingHistory.DisabledColor = System.Drawing.Color.DarkGray;
            this.btnProcessingHistory.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnProcessingHistory.EdgeRadius = 5;
            this.btnProcessingHistory.GradientAngle = 70F;
            this.btnProcessingHistory.GradientFirstColor = System.Drawing.Color.White;
            this.btnProcessingHistory.GradientSecondColor = System.Drawing.Color.White;
            this.btnProcessingHistory.HoverEmphasizeCustomColor = System.Drawing.Color.White;
            this.btnProcessingHistory.ImagePosition = new System.Drawing.Point(10, 10);
            this.btnProcessingHistory.ImageSize = new System.Drawing.Point(15, 15);
            this.btnProcessingHistory.LoadImage = ((System.Drawing.Image)(resources.GetObject("btnProcessingHistory.LoadImage")));
            this.btnProcessingHistory.Location = new System.Drawing.Point(632, 2);
            this.btnProcessingHistory.MainFont = new System.Drawing.Font("맑은 고딕", 12F, System.Drawing.FontStyle.Bold);
            this.btnProcessingHistory.MainFontColor = System.Drawing.Color.SeaGreen;
            this.btnProcessingHistory.Margin = new System.Windows.Forms.Padding(2);
            this.btnProcessingHistory.Name = "btnProcessingHistory";
            this.btnProcessingHistory.Size = new System.Drawing.Size(206, 30);
            this.btnProcessingHistory.SubFont = new System.Drawing.Font("맑은 고딕", 10F);
            this.btnProcessingHistory.SubFontColor = System.Drawing.Color.SeaGreen;
            this.btnProcessingHistory.SubText = "";
            this.btnProcessingHistory.TabIndex = 3;
            this.btnProcessingHistory.Tag = "CARRIER_LOADING";
            this.btnProcessingHistory.Text = "Processing History";
            this.btnProcessingHistory.TextAlignMain = Sys3Controls.EN_TEXTALIGN.MIDDLE_CENTER;
            this.btnProcessingHistory.TextAlignSub = Sys3Controls.EN_TEXTALIGN.MIDDLE_CENTER;
            this.btnProcessingHistory.ThemeIndex = 0;
            this.btnProcessingHistory.UseBorder = false;
            this.btnProcessingHistory.UseClickedEmphasizeTextColor = false;
            this.btnProcessingHistory.UseCustomizeClickedColor = true;
            this.btnProcessingHistory.UseEdge = false;
            this.btnProcessingHistory.UseHoverEmphasizeCustomColor = false;
            this.btnProcessingHistory.UseImage = false;
            this.btnProcessingHistory.UserHoverEmpahsize = false;
            this.btnProcessingHistory.UseSubFont = false;
            this.btnProcessingHistory.Click += new System.EventHandler(this.SubstratePageButtonClicked);
            // 
            // btnLocationHistory
            // 
            this.btnLocationHistory.BorderWidth = 2;
            this.btnLocationHistory.ButtonClicked = false;
            this.btnLocationHistory.ClickedEmphasizeTextColor = System.Drawing.Color.White;
            this.btnLocationHistory.CustomClickedGradientFirstColor = System.Drawing.Color.SeaGreen;
            this.btnLocationHistory.CustomClickedGradientSecondColor = System.Drawing.Color.SeaGreen;
            this.btnLocationHistory.Description = "";
            this.btnLocationHistory.DisabledColor = System.Drawing.Color.DarkGray;
            this.btnLocationHistory.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnLocationHistory.EdgeRadius = 5;
            this.btnLocationHistory.GradientAngle = 70F;
            this.btnLocationHistory.GradientFirstColor = System.Drawing.Color.White;
            this.btnLocationHistory.GradientSecondColor = System.Drawing.Color.White;
            this.btnLocationHistory.HoverEmphasizeCustomColor = System.Drawing.Color.White;
            this.btnLocationHistory.ImagePosition = new System.Drawing.Point(10, 10);
            this.btnLocationHistory.ImageSize = new System.Drawing.Point(15, 15);
            this.btnLocationHistory.LoadImage = ((System.Drawing.Image)(resources.GetObject("btnLocationHistory.LoadImage")));
            this.btnLocationHistory.Location = new System.Drawing.Point(422, 2);
            this.btnLocationHistory.MainFont = new System.Drawing.Font("맑은 고딕", 12F, System.Drawing.FontStyle.Bold);
            this.btnLocationHistory.MainFontColor = System.Drawing.Color.SeaGreen;
            this.btnLocationHistory.Margin = new System.Windows.Forms.Padding(2);
            this.btnLocationHistory.Name = "btnLocationHistory";
            this.btnLocationHistory.Size = new System.Drawing.Size(206, 30);
            this.btnLocationHistory.SubFont = new System.Drawing.Font("맑은 고딕", 10F);
            this.btnLocationHistory.SubFontColor = System.Drawing.Color.SeaGreen;
            this.btnLocationHistory.SubText = "";
            this.btnLocationHistory.TabIndex = 2;
            this.btnLocationHistory.Tag = "CARRIER_LOADING";
            this.btnLocationHistory.Text = "Location History";
            this.btnLocationHistory.TextAlignMain = Sys3Controls.EN_TEXTALIGN.MIDDLE_CENTER;
            this.btnLocationHistory.TextAlignSub = Sys3Controls.EN_TEXTALIGN.MIDDLE_CENTER;
            this.btnLocationHistory.ThemeIndex = 0;
            this.btnLocationHistory.UseBorder = false;
            this.btnLocationHistory.UseClickedEmphasizeTextColor = false;
            this.btnLocationHistory.UseCustomizeClickedColor = true;
            this.btnLocationHistory.UseEdge = false;
            this.btnLocationHistory.UseHoverEmphasizeCustomColor = false;
            this.btnLocationHistory.UseImage = false;
            this.btnLocationHistory.UserHoverEmpahsize = false;
            this.btnLocationHistory.UseSubFont = false;
            this.btnLocationHistory.Click += new System.EventHandler(this.SubstratePageButtonClicked);
            // 
            // btnLotHistory
            // 
            this.btnLotHistory.BorderWidth = 2;
            this.btnLotHistory.ButtonClicked = false;
            this.btnLotHistory.ClickedEmphasizeTextColor = System.Drawing.Color.White;
            this.btnLotHistory.CustomClickedGradientFirstColor = System.Drawing.Color.SeaGreen;
            this.btnLotHistory.CustomClickedGradientSecondColor = System.Drawing.Color.SeaGreen;
            this.btnLotHistory.Description = "";
            this.btnLotHistory.DisabledColor = System.Drawing.Color.DarkGray;
            this.btnLotHistory.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnLotHistory.EdgeRadius = 5;
            this.btnLotHistory.GradientAngle = 70F;
            this.btnLotHistory.GradientFirstColor = System.Drawing.Color.White;
            this.btnLotHistory.GradientSecondColor = System.Drawing.Color.White;
            this.btnLotHistory.HoverEmphasizeCustomColor = System.Drawing.Color.White;
            this.btnLotHistory.ImagePosition = new System.Drawing.Point(10, 10);
            this.btnLotHistory.ImageSize = new System.Drawing.Point(15, 15);
            this.btnLotHistory.LoadImage = ((System.Drawing.Image)(resources.GetObject("btnLotHistory.LoadImage")));
            this.btnLotHistory.Location = new System.Drawing.Point(212, 2);
            this.btnLotHistory.MainFont = new System.Drawing.Font("맑은 고딕", 12F, System.Drawing.FontStyle.Bold);
            this.btnLotHistory.MainFontColor = System.Drawing.Color.SeaGreen;
            this.btnLotHistory.Margin = new System.Windows.Forms.Padding(2);
            this.btnLotHistory.Name = "btnLotHistory";
            this.btnLotHistory.Size = new System.Drawing.Size(206, 30);
            this.btnLotHistory.SubFont = new System.Drawing.Font("맑은 고딕", 10F);
            this.btnLotHistory.SubFontColor = System.Drawing.Color.SeaGreen;
            this.btnLotHistory.SubText = "";
            this.btnLotHistory.TabIndex = 1;
            this.btnLotHistory.Tag = "CARRIER_LOADING";
            this.btnLotHistory.Text = "Lot History";
            this.btnLotHistory.TextAlignMain = Sys3Controls.EN_TEXTALIGN.MIDDLE_CENTER;
            this.btnLotHistory.TextAlignSub = Sys3Controls.EN_TEXTALIGN.MIDDLE_CENTER;
            this.btnLotHistory.ThemeIndex = 0;
            this.btnLotHistory.UseBorder = false;
            this.btnLotHistory.UseClickedEmphasizeTextColor = false;
            this.btnLotHistory.UseCustomizeClickedColor = true;
            this.btnLotHistory.UseEdge = false;
            this.btnLotHistory.UseHoverEmphasizeCustomColor = false;
            this.btnLotHistory.UseImage = false;
            this.btnLotHistory.UserHoverEmpahsize = false;
            this.btnLotHistory.UseSubFont = false;
            this.btnLotHistory.Click += new System.EventHandler(this.SubstratePageButtonClicked);
            // 
            // btnExtra
            // 
            this.btnExtra.BorderWidth = 2;
            this.btnExtra.ButtonClicked = false;
            this.btnExtra.ClickedEmphasizeTextColor = System.Drawing.Color.White;
            this.btnExtra.CustomClickedGradientFirstColor = System.Drawing.Color.SeaGreen;
            this.btnExtra.CustomClickedGradientSecondColor = System.Drawing.Color.SeaGreen;
            this.btnExtra.Description = "";
            this.btnExtra.DisabledColor = System.Drawing.Color.DarkGray;
            this.btnExtra.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnExtra.EdgeRadius = 5;
            this.btnExtra.GradientAngle = 70F;
            this.btnExtra.GradientFirstColor = System.Drawing.Color.White;
            this.btnExtra.GradientSecondColor = System.Drawing.Color.White;
            this.btnExtra.HoverEmphasizeCustomColor = System.Drawing.Color.White;
            this.btnExtra.ImagePosition = new System.Drawing.Point(10, 10);
            this.btnExtra.ImageSize = new System.Drawing.Point(15, 15);
            this.btnExtra.LoadImage = ((System.Drawing.Image)(resources.GetObject("btnExtra.LoadImage")));
            this.btnExtra.Location = new System.Drawing.Point(2, 2);
            this.btnExtra.MainFont = new System.Drawing.Font("맑은 고딕", 12F, System.Drawing.FontStyle.Bold);
            this.btnExtra.MainFontColor = System.Drawing.Color.SeaGreen;
            this.btnExtra.Margin = new System.Windows.Forms.Padding(2);
            this.btnExtra.Name = "btnExtra";
            this.btnExtra.Size = new System.Drawing.Size(206, 30);
            this.btnExtra.SubFont = new System.Drawing.Font("맑은 고딕", 10F);
            this.btnExtra.SubFontColor = System.Drawing.Color.SeaGreen;
            this.btnExtra.SubText = "";
            this.btnExtra.TabIndex = 0;
            this.btnExtra.Tag = "CARRIER_LOADING";
            this.btnExtra.Text = "Extra";
            this.btnExtra.TextAlignMain = Sys3Controls.EN_TEXTALIGN.MIDDLE_CENTER;
            this.btnExtra.TextAlignSub = Sys3Controls.EN_TEXTALIGN.MIDDLE_CENTER;
            this.btnExtra.ThemeIndex = 0;
            this.btnExtra.UseBorder = false;
            this.btnExtra.UseClickedEmphasizeTextColor = false;
            this.btnExtra.UseCustomizeClickedColor = true;
            this.btnExtra.UseEdge = false;
            this.btnExtra.UseHoverEmphasizeCustomColor = false;
            this.btnExtra.UseImage = false;
            this.btnExtra.UserHoverEmpahsize = false;
            this.btnExtra.UseSubFont = false;
            this.btnExtra.Click += new System.EventHandler(this.SubstratePageButtonClicked);
            // 
            // _pnlSubstratePagesHost
            // 
            this._pnlSubstratePagesHost.Controls.Add(this._pnlSubstratePageExtra);
            this._pnlSubstratePagesHost.Controls.Add(this._pnlSubstratePageHistory);
            this._pnlSubstratePagesHost.Controls.Add(this._pnlSubstratePageProcessing);
            this._pnlSubstratePagesHost.Controls.Add(this._pnlSubstratePageLocation);
            this._pnlSubstratePagesHost.Dock = System.Windows.Forms.DockStyle.Fill;
            this._pnlSubstratePagesHost.Location = new System.Drawing.Point(3, 43);
            this._pnlSubstratePagesHost.Name = "_pnlSubstratePagesHost";
            this._pnlSubstratePagesHost.Size = new System.Drawing.Size(1134, 338);
            this._pnlSubstratePagesHost.TabIndex = 1;
            // 
            // _pnlSubstratePageExtra
            // 
            this._pnlSubstratePageExtra.Controls.Add(this._pnlSubstrateExtraSection);
            this._pnlSubstratePageExtra.Dock = System.Windows.Forms.DockStyle.Fill;
            this._pnlSubstratePageExtra.Location = new System.Drawing.Point(0, 0);
            this._pnlSubstratePageExtra.Name = "_pnlSubstratePageExtra";
            this._pnlSubstratePageExtra.Size = new System.Drawing.Size(1134, 338);
            this._pnlSubstratePageExtra.TabIndex = 0;
            // 
            // _pnlSubstrateExtraSection
            // 
            this._pnlSubstrateExtraSection.Controls.Add(this._gvSubstrateExtra);
            this._pnlSubstrateExtraSection.Controls.Add(this._lblSubstrateExtraSectionHeader);
            this._pnlSubstrateExtraSection.Dock = System.Windows.Forms.DockStyle.Fill;
            this._pnlSubstrateExtraSection.Location = new System.Drawing.Point(0, 0);
            this._pnlSubstrateExtraSection.Name = "_pnlSubstrateExtraSection";
            this._pnlSubstrateExtraSection.Size = new System.Drawing.Size(1134, 338);
            this._pnlSubstrateExtraSection.TabIndex = 0;
            // 
            // _gvSubstrateExtra
            // 
            this._gvSubstrateExtra.AllowUserToAddRows = false;
            this._gvSubstrateExtra.AllowUserToDeleteRows = false;
            this._gvSubstrateExtra.AllowUserToResizeRows = false;
            this._gvSubstrateExtra.BackgroundColor = System.Drawing.Color.White;
            this._gvSubstrateExtra.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this._gvSubstrateExtra.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this._colSubstrateExtraKey,
            this._colSubstrateExtraValue});
            this._gvSubstrateExtra.Dock = System.Windows.Forms.DockStyle.Fill;
            this._gvSubstrateExtra.EditMode = System.Windows.Forms.DataGridViewEditMode.EditProgrammatically;
            this._gvSubstrateExtra.Location = new System.Drawing.Point(0, 24);
            this._gvSubstrateExtra.MultiSelect = false;
            this._gvSubstrateExtra.Name = "_gvSubstrateExtra";
            this._gvSubstrateExtra.RowHeadersVisible = false;
            this._gvSubstrateExtra.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this._gvSubstrateExtra.Size = new System.Drawing.Size(1134, 314);
            this._gvSubstrateExtra.TabIndex = 1;
            // 
            // _colSubstrateExtraKey
            // 
            this._colSubstrateExtraKey.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this._colSubstrateExtraKey.HeaderText = "Extra Key";
            this._colSubstrateExtraKey.Name = "_colSubstrateExtraKey";
            this._colSubstrateExtraKey.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // _colSubstrateExtraValue
            // 
            this._colSubstrateExtraValue.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this._colSubstrateExtraValue.HeaderText = "값";
            this._colSubstrateExtraValue.Name = "_colSubstrateExtraValue";
            this._colSubstrateExtraValue.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // _lblSubstrateExtraSectionHeader
            // 
            this._lblSubstrateExtraSectionHeader.BackGroundColor = System.Drawing.Color.LightSteelBlue;
            this._lblSubstrateExtraSectionHeader.BorderStroke = 1;
            this._lblSubstrateExtraSectionHeader.BorderStyle = System.Windows.Forms.ButtonBorderStyle.Solid;
            this._lblSubstrateExtraSectionHeader.Description = "";
            this._lblSubstrateExtraSectionHeader.DisabledColor = System.Drawing.Color.DarkGray;
            this._lblSubstrateExtraSectionHeader.Dock = System.Windows.Forms.DockStyle.Top;
            this._lblSubstrateExtraSectionHeader.EdgeRadius = 1;
            this._lblSubstrateExtraSectionHeader.ImagePosition = new System.Drawing.Point(0, 0);
            this._lblSubstrateExtraSectionHeader.ImageSize = new System.Drawing.Point(0, 0);
            this._lblSubstrateExtraSectionHeader.LoadImage = null;
            this._lblSubstrateExtraSectionHeader.Location = new System.Drawing.Point(0, 0);
            this._lblSubstrateExtraSectionHeader.MainFont = new System.Drawing.Font("맑은 고딕", 9F, System.Drawing.FontStyle.Bold);
            this._lblSubstrateExtraSectionHeader.MainFontColor = System.Drawing.Color.Black;
            this._lblSubstrateExtraSectionHeader.Name = "_lblSubstrateExtraSectionHeader";
            this._lblSubstrateExtraSectionHeader.Size = new System.Drawing.Size(1134, 24);
            this._lblSubstrateExtraSectionHeader.SubFont = new System.Drawing.Font("맑은 고딕", 10F);
            this._lblSubstrateExtraSectionHeader.SubFontColor = System.Drawing.Color.Black;
            this._lblSubstrateExtraSectionHeader.SubText = "";
            this._lblSubstrateExtraSectionHeader.TabIndex = 2;
            this._lblSubstrateExtraSectionHeader.Text = "Extra";
            this._lblSubstrateExtraSectionHeader.TextAlignMain = Sys3Controls.EN_TEXTALIGN.MIDDLE_LEFT;
            this._lblSubstrateExtraSectionHeader.TextAlignSub = Sys3Controls.EN_TEXTALIGN.MIDDLE_CENTER;
            this._lblSubstrateExtraSectionHeader.TextAlignUnit = Sys3Controls.EN_TEXTALIGN.MIDDLE_CENTER;
            this._lblSubstrateExtraSectionHeader.ThemeIndex = 0;
            this._lblSubstrateExtraSectionHeader.UnitAreaRate = 30;
            this._lblSubstrateExtraSectionHeader.UnitFont = new System.Drawing.Font("맑은 고딕", 10F, System.Drawing.FontStyle.Bold);
            this._lblSubstrateExtraSectionHeader.UnitFontColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this._lblSubstrateExtraSectionHeader.UnitPositionVertical = false;
            this._lblSubstrateExtraSectionHeader.UnitText = "";
            this._lblSubstrateExtraSectionHeader.UseBorder = true;
            this._lblSubstrateExtraSectionHeader.UseEdgeRadius = false;
            this._lblSubstrateExtraSectionHeader.UseImage = false;
            this._lblSubstrateExtraSectionHeader.UseSubFont = false;
            this._lblSubstrateExtraSectionHeader.UseUnitFont = false;
            // 
            // _pnlSubstratePageHistory
            // 
            this._pnlSubstratePageHistory.Controls.Add(this._gvSubstrateLotHistory);
            this._pnlSubstratePageHistory.Dock = System.Windows.Forms.DockStyle.Fill;
            this._pnlSubstratePageHistory.Location = new System.Drawing.Point(0, 0);
            this._pnlSubstratePageHistory.Name = "_pnlSubstratePageHistory";
            this._pnlSubstratePageHistory.Size = new System.Drawing.Size(1134, 338);
            this._pnlSubstratePageHistory.TabIndex = 1;
            this._pnlSubstratePageHistory.Visible = false;
            // 
            // _gvSubstrateLotHistory
            // 
            this._gvSubstrateLotHistory.AllowUserToAddRows = false;
            this._gvSubstrateLotHistory.AllowUserToDeleteRows = false;
            this._gvSubstrateLotHistory.AllowUserToResizeRows = false;
            this._gvSubstrateLotHistory.BackgroundColor = System.Drawing.Color.White;
            this._gvSubstrateLotHistory.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this._gvSubstrateLotHistory.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this._colSubHistTime,
            this._colSubHistPortEvent,
            this._colSubHistWafer,
            this._colSubHistWaferEvent,
            this._colSubHistMessage});
            this._gvSubstrateLotHistory.Dock = System.Windows.Forms.DockStyle.Fill;
            this._gvSubstrateLotHistory.EditMode = System.Windows.Forms.DataGridViewEditMode.EditProgrammatically;
            this._gvSubstrateLotHistory.Location = new System.Drawing.Point(0, 0);
            this._gvSubstrateLotHistory.MultiSelect = false;
            this._gvSubstrateLotHistory.Name = "_gvSubstrateLotHistory";
            this._gvSubstrateLotHistory.RowHeadersVisible = false;
            this._gvSubstrateLotHistory.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this._gvSubstrateLotHistory.Size = new System.Drawing.Size(1134, 338);
            this._gvSubstrateLotHistory.TabIndex = 0;
            // 
            // _colSubHistTime
            // 
            this._colSubHistTime.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this._colSubHistTime.HeaderText = "TIME";
            this._colSubHistTime.Name = "_colSubHistTime";
            this._colSubHistTime.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // _colSubHistPortEvent
            // 
            this._colSubHistPortEvent.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this._colSubHistPortEvent.HeaderText = "PORT EVENT";
            this._colSubHistPortEvent.Name = "_colSubHistPortEvent";
            this._colSubHistPortEvent.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // _colSubHistWafer
            // 
            this._colSubHistWafer.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this._colSubHistWafer.HeaderText = "WAFER";
            this._colSubHistWafer.Name = "_colSubHistWafer";
            this._colSubHistWafer.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // _colSubHistWaferEvent
            // 
            this._colSubHistWaferEvent.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this._colSubHistWaferEvent.HeaderText = "WAFER EVENT";
            this._colSubHistWaferEvent.Name = "_colSubHistWaferEvent";
            this._colSubHistWaferEvent.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // _colSubHistMessage
            // 
            this._colSubHistMessage.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this._colSubHistMessage.HeaderText = "MESSAGE";
            this._colSubHistMessage.Name = "_colSubHistMessage";
            this._colSubHistMessage.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // _pnlSubstratePageProcessing
            // 
            this._pnlSubstratePageProcessing.Controls.Add(this._gvProcessing);
            this._pnlSubstratePageProcessing.Dock = System.Windows.Forms.DockStyle.Fill;
            this._pnlSubstratePageProcessing.Location = new System.Drawing.Point(0, 0);
            this._pnlSubstratePageProcessing.Name = "_pnlSubstratePageProcessing";
            this._pnlSubstratePageProcessing.Size = new System.Drawing.Size(1134, 338);
            this._pnlSubstratePageProcessing.TabIndex = 2;
            this._pnlSubstratePageProcessing.Visible = false;
            // 
            // _gvProcessing
            // 
            this._gvProcessing.AllowUserToAddRows = false;
            this._gvProcessing.AllowUserToDeleteRows = false;
            this._gvProcessing.AllowUserToResizeRows = false;
            this._gvProcessing.BackgroundColor = System.Drawing.Color.White;
            this._gvProcessing.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this._gvProcessing.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this._colProcTime,
            this._colProcOld,
            this._colProcNew,
            this._colProcControlJob,
            this._colProcProcessJob,
            this._colProcLocation,
            this._colProcDesc});
            this._gvProcessing.Dock = System.Windows.Forms.DockStyle.Fill;
            this._gvProcessing.EditMode = System.Windows.Forms.DataGridViewEditMode.EditProgrammatically;
            this._gvProcessing.Location = new System.Drawing.Point(0, 0);
            this._gvProcessing.MultiSelect = false;
            this._gvProcessing.Name = "_gvProcessing";
            this._gvProcessing.RowHeadersVisible = false;
            this._gvProcessing.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this._gvProcessing.Size = new System.Drawing.Size(1134, 338);
            this._gvProcessing.TabIndex = 0;
            // 
            // _colProcTime
            // 
            this._colProcTime.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this._colProcTime.HeaderText = "TIME";
            this._colProcTime.Name = "_colProcTime";
            this._colProcTime.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // _colProcOld
            // 
            this._colProcOld.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this._colProcOld.HeaderText = "이전 상태";
            this._colProcOld.Name = "_colProcOld";
            this._colProcOld.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // _colProcNew
            // 
            this._colProcNew.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this._colProcNew.HeaderText = "이후 상태";
            this._colProcNew.Name = "_colProcNew";
            this._colProcNew.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // _colProcControlJob
            // 
            this._colProcControlJob.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this._colProcControlJob.HeaderText = "Control Job";
            this._colProcControlJob.Name = "_colProcControlJob";
            this._colProcControlJob.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // _colProcProcessJob
            // 
            this._colProcProcessJob.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this._colProcProcessJob.HeaderText = "Process Job";
            this._colProcProcessJob.Name = "_colProcProcessJob";
            this._colProcProcessJob.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // _colProcLocation
            // 
            this._colProcLocation.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this._colProcLocation.HeaderText = "Location";
            this._colProcLocation.Name = "_colProcLocation";
            this._colProcLocation.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // _colProcDesc
            // 
            this._colProcDesc.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this._colProcDesc.HeaderText = "설명";
            this._colProcDesc.Name = "_colProcDesc";
            this._colProcDesc.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // _pnlSubstratePageLocation
            // 
            this._pnlSubstratePageLocation.Controls.Add(this._gvLocation);
            this._pnlSubstratePageLocation.Dock = System.Windows.Forms.DockStyle.Fill;
            this._pnlSubstratePageLocation.Location = new System.Drawing.Point(0, 0);
            this._pnlSubstratePageLocation.Name = "_pnlSubstratePageLocation";
            this._pnlSubstratePageLocation.Size = new System.Drawing.Size(1134, 338);
            this._pnlSubstratePageLocation.TabIndex = 3;
            this._pnlSubstratePageLocation.Visible = false;
            // 
            // _gvLocation
            // 
            this._gvLocation.AllowUserToAddRows = false;
            this._gvLocation.AllowUserToDeleteRows = false;
            this._gvLocation.AllowUserToResizeRows = false;
            this._gvLocation.BackgroundColor = System.Drawing.Color.White;
            this._gvLocation.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this._gvLocation.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this._colLocTime,
            this._colLocFrom,
            this._colLocFromKind,
            this._colLocTo,
            this._colLocToKind,
            this._colLocReason});
            this._gvLocation.Dock = System.Windows.Forms.DockStyle.Fill;
            this._gvLocation.EditMode = System.Windows.Forms.DataGridViewEditMode.EditProgrammatically;
            this._gvLocation.Location = new System.Drawing.Point(0, 0);
            this._gvLocation.MultiSelect = false;
            this._gvLocation.Name = "_gvLocation";
            this._gvLocation.RowHeadersVisible = false;
            this._gvLocation.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this._gvLocation.Size = new System.Drawing.Size(1134, 338);
            this._gvLocation.TabIndex = 0;
            // 
            // _colLocTime
            // 
            this._colLocTime.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this._colLocTime.HeaderText = "TIME";
            this._colLocTime.Name = "_colLocTime";
            this._colLocTime.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // _colLocFrom
            // 
            this._colLocFrom.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this._colLocFrom.HeaderText = "From";
            this._colLocFrom.Name = "_colLocFrom";
            this._colLocFrom.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // _colLocFromKind
            // 
            this._colLocFromKind.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this._colLocFromKind.HeaderText = "FromKind";
            this._colLocFromKind.Name = "_colLocFromKind";
            this._colLocFromKind.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // _colLocTo
            // 
            this._colLocTo.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this._colLocTo.HeaderText = "To";
            this._colLocTo.Name = "_colLocTo";
            this._colLocTo.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // _colLocToKind
            // 
            this._colLocToKind.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this._colLocToKind.HeaderText = "ToKind";
            this._colLocToKind.Name = "_colLocToKind";
            this._colLocToKind.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // _colLocReason
            // 
            this._colLocReason.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this._colLocReason.HeaderText = "사유";
            this._colLocReason.Name = "_colLocReason";
            this._colLocReason.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // SubstrateDetailView
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.Controls.Add(this._tlpSubstrateDetailWrapper);
            this.Name = "SubstrateDetailView";
            this.Size = new System.Drawing.Size(1140, 384);
            this._tlpSubstrateDetailWrapper.ResumeLayout(false);
            this._pnlSubstratePageBar.ResumeLayout(false);
            this._pnlSubstratePagesHost.ResumeLayout(false);
            this._pnlSubstratePageExtra.ResumeLayout(false);
            this._pnlSubstrateExtraSection.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this._gvSubstrateExtra)).EndInit();
            this._pnlSubstratePageHistory.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this._gvSubstrateLotHistory)).EndInit();
            this._pnlSubstratePageProcessing.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this._gvProcessing)).EndInit();
            this._pnlSubstratePageLocation.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this._gvLocation)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel _tlpSubstrateDetailWrapper;
        private System.Windows.Forms.Panel _pnlSubstratePagesHost;
        private System.Windows.Forms.Panel _pnlSubstratePageExtra;
        private System.Windows.Forms.Panel _pnlSubstrateExtraSection;
        private Sys3Controls.Sys3DoubleBufferedDataGridView _gvSubstrateExtra;
        private System.Windows.Forms.DataGridViewTextBoxColumn _colSubstrateExtraKey;
        private System.Windows.Forms.DataGridViewTextBoxColumn _colSubstrateExtraValue;
        private Sys3Controls.Sys3Label _lblSubstrateExtraSectionHeader;
        private System.Windows.Forms.Panel _pnlSubstratePageHistory;
        private Sys3Controls.Sys3DoubleBufferedDataGridView _gvSubstrateLotHistory;
        private System.Windows.Forms.DataGridViewTextBoxColumn _colSubHistTime;
        private System.Windows.Forms.DataGridViewTextBoxColumn _colSubHistPortEvent;
        private System.Windows.Forms.DataGridViewTextBoxColumn _colSubHistWafer;
        private System.Windows.Forms.DataGridViewTextBoxColumn _colSubHistWaferEvent;
        private System.Windows.Forms.DataGridViewTextBoxColumn _colSubHistMessage;
        private System.Windows.Forms.Panel _pnlSubstratePageProcessing;
        private Sys3Controls.Sys3DoubleBufferedDataGridView _gvProcessing;
        private System.Windows.Forms.DataGridViewTextBoxColumn _colProcTime;
        private System.Windows.Forms.DataGridViewTextBoxColumn _colProcOld;
        private System.Windows.Forms.DataGridViewTextBoxColumn _colProcNew;
        private System.Windows.Forms.DataGridViewTextBoxColumn _colProcControlJob;
        private System.Windows.Forms.DataGridViewTextBoxColumn _colProcProcessJob;
        private System.Windows.Forms.DataGridViewTextBoxColumn _colProcLocation;
        private System.Windows.Forms.DataGridViewTextBoxColumn _colProcDesc;
        private System.Windows.Forms.Panel _pnlSubstratePageLocation;
        private Sys3Controls.Sys3DoubleBufferedDataGridView _gvLocation;
        private System.Windows.Forms.DataGridViewTextBoxColumn _colLocTime;
        private System.Windows.Forms.DataGridViewTextBoxColumn _colLocFrom;
        private System.Windows.Forms.DataGridViewTextBoxColumn _colLocFromKind;
        private System.Windows.Forms.DataGridViewTextBoxColumn _colLocTo;
        private System.Windows.Forms.DataGridViewTextBoxColumn _colLocToKind;
        private System.Windows.Forms.DataGridViewTextBoxColumn _colLocReason;
        private System.Windows.Forms.TableLayoutPanel _pnlSubstratePageBar;
        private Sys3Controls.Sys3button btnExtra;
        private Sys3Controls.Sys3button btnLotHistory;
        private Sys3Controls.Sys3button btnProcessingHistory;
        private Sys3Controls.Sys3button btnLocationHistory;
        private Sys3Controls.Sys3button btnReturnToCarrier;
    }
}
