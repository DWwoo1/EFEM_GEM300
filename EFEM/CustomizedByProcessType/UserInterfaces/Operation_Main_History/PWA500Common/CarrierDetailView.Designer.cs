namespace EFEM.CustomizedByProcessType.UserInterface.OperationMainHistory.PWA500Common
{
    partial class CarrierDetailView
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(CarrierDetailView));
            this._tlpCarrierDetailWrapper = new System.Windows.Forms.TableLayoutPanel();
            this._pnlSubstratePageBar = new System.Windows.Forms.TableLayoutPanel();
            this.btnSearchSelectedSubstrate = new Sys3Controls.Sys3button();
            this.btnExtra = new Sys3Controls.Sys3button();
            this.btnLotHistory = new Sys3Controls.Sys3button();
            this.btnSubstratesInCarrier = new Sys3Controls.Sys3button();
            this._pnlCarrierPagesHost = new System.Windows.Forms.Panel();
            this._pnlCarrierPageExtra = new System.Windows.Forms.Panel();
            this._tlpCarrierExtraLayout = new System.Windows.Forms.TableLayoutPanel();
            this._pnlCarrierSlotMapSection = new System.Windows.Forms.Panel();
            this._gvCarrierSlotMap = new Sys3Controls.Sys3DoubleBufferedDataGridView();
            this._colCarrierSlotMapSlot = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this._colCarrierSlotMapMap = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this._lblCarrierSlotMapHeader = new Sys3Controls.Sys3Label();
            this._pnlCarrierExtraSection = new System.Windows.Forms.Panel();
            this._gvCarrierExtra = new Sys3Controls.Sys3DoubleBufferedDataGridView();
            this._colCarrierExtraKey = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this._colCarrierExtraValue = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this._lblCarrierExtraSectionHeader = new Sys3Controls.Sys3Label();
            this._pnlCarrierPageHistory = new System.Windows.Forms.Panel();
            this._gvCarrierLotHistory = new Sys3Controls.Sys3DoubleBufferedDataGridView();
            this._colCarrierHistTime = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this._colCarrierHistPortEvent = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this._colCarrierHistWafer = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this._colCarrierHistWaferEvent = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this._colCarrierHistMessage = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this._pnlCarrierPageSubstrates = new System.Windows.Forms.Panel();
            this._gvSubstratesInCarrier = new Sys3Controls.Sys3DoubleBufferedDataGridView();
            this._tlpCarrierDetailWrapper.SuspendLayout();
            this._pnlSubstratePageBar.SuspendLayout();
            this._pnlCarrierPagesHost.SuspendLayout();
            this._pnlCarrierPageExtra.SuspendLayout();
            this._tlpCarrierExtraLayout.SuspendLayout();
            this._pnlCarrierSlotMapSection.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this._gvCarrierSlotMap)).BeginInit();
            this._pnlCarrierExtraSection.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this._gvCarrierExtra)).BeginInit();
            this._pnlCarrierPageHistory.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this._gvCarrierLotHistory)).BeginInit();
            this._pnlCarrierPageSubstrates.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this._gvSubstratesInCarrier)).BeginInit();
            this.SuspendLayout();
            // 
            // _tlpCarrierDetailWrapper
            // 
            this._tlpCarrierDetailWrapper.ColumnCount = 1;
            this._tlpCarrierDetailWrapper.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this._tlpCarrierDetailWrapper.Controls.Add(this._pnlSubstratePageBar, 0, 0);
            this._tlpCarrierDetailWrapper.Controls.Add(this._pnlCarrierPagesHost, 0, 1);
            this._tlpCarrierDetailWrapper.Dock = System.Windows.Forms.DockStyle.Fill;
            this._tlpCarrierDetailWrapper.Location = new System.Drawing.Point(0, 0);
            this._tlpCarrierDetailWrapper.Name = "_tlpCarrierDetailWrapper";
            this._tlpCarrierDetailWrapper.RowCount = 2;
            this._tlpCarrierDetailWrapper.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 40F));
            this._tlpCarrierDetailWrapper.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this._tlpCarrierDetailWrapper.Size = new System.Drawing.Size(1140, 384);
            this._tlpCarrierDetailWrapper.TabIndex = 0;
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
            this._pnlSubstratePageBar.Controls.Add(this.btnSearchSelectedSubstrate, 4, 0);
            this._pnlSubstratePageBar.Controls.Add(this.btnExtra, 2, 0);
            this._pnlSubstratePageBar.Controls.Add(this.btnLotHistory, 1, 0);
            this._pnlSubstratePageBar.Controls.Add(this.btnSubstratesInCarrier, 0, 0);
            this._pnlSubstratePageBar.Dock = System.Windows.Forms.DockStyle.Fill;
            this._pnlSubstratePageBar.Location = new System.Drawing.Point(3, 3);
            this._pnlSubstratePageBar.Name = "_pnlSubstratePageBar";
            this._pnlSubstratePageBar.RowCount = 1;
            this._pnlSubstratePageBar.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this._pnlSubstratePageBar.Size = new System.Drawing.Size(1134, 34);
            this._pnlSubstratePageBar.TabIndex = 4;
            // 
            // btnSearchSelectedSubstrate
            // 
            this.btnSearchSelectedSubstrate.BorderWidth = 2;
            this.btnSearchSelectedSubstrate.ButtonClicked = false;
            this.btnSearchSelectedSubstrate.ClickedEmphasizeTextColor = System.Drawing.Color.White;
            this.btnSearchSelectedSubstrate.CustomClickedGradientFirstColor = System.Drawing.Color.BlanchedAlmond;
            this.btnSearchSelectedSubstrate.CustomClickedGradientSecondColor = System.Drawing.Color.Gold;
            this.btnSearchSelectedSubstrate.Description = "";
            this.btnSearchSelectedSubstrate.DisabledColor = System.Drawing.Color.Silver;
            this.btnSearchSelectedSubstrate.Dock = System.Windows.Forms.DockStyle.Right;
            this.btnSearchSelectedSubstrate.EdgeRadius = 5;
            this.btnSearchSelectedSubstrate.GradientAngle = 70F;
            this.btnSearchSelectedSubstrate.GradientFirstColor = System.Drawing.Color.PaleGreen;
            this.btnSearchSelectedSubstrate.GradientSecondColor = System.Drawing.Color.ForestGreen;
            this.btnSearchSelectedSubstrate.HoverEmphasizeCustomColor = System.Drawing.Color.Firebrick;
            this.btnSearchSelectedSubstrate.ImagePosition = new System.Drawing.Point(10, 10);
            this.btnSearchSelectedSubstrate.ImageSize = new System.Drawing.Point(15, 15);
            this.btnSearchSelectedSubstrate.LoadImage = ((System.Drawing.Image)(resources.GetObject("btnSearchSelectedSubstrate.LoadImage")));
            this.btnSearchSelectedSubstrate.Location = new System.Drawing.Point(922, 2);
            this.btnSearchSelectedSubstrate.MainFont = new System.Drawing.Font("맑은 고딕", 12F, System.Drawing.FontStyle.Bold);
            this.btnSearchSelectedSubstrate.MainFontColor = System.Drawing.Color.White;
            this.btnSearchSelectedSubstrate.Margin = new System.Windows.Forms.Padding(2);
            this.btnSearchSelectedSubstrate.Name = "btnSearchSelectedSubstrate";
            this.btnSearchSelectedSubstrate.Size = new System.Drawing.Size(210, 30);
            this.btnSearchSelectedSubstrate.SubFont = new System.Drawing.Font("맑은 고딕", 10F);
            this.btnSearchSelectedSubstrate.SubFontColor = System.Drawing.Color.SeaGreen;
            this.btnSearchSelectedSubstrate.SubText = "";
            this.btnSearchSelectedSubstrate.TabIndex = 7;
            this.btnSearchSelectedSubstrate.Tag = "";
            this.btnSearchSelectedSubstrate.Text = "Search This Substrate";
            this.btnSearchSelectedSubstrate.TextAlignMain = Sys3Controls.EN_TEXTALIGN.MIDDLE_CENTER;
            this.btnSearchSelectedSubstrate.TextAlignSub = Sys3Controls.EN_TEXTALIGN.MIDDLE_CENTER;
            this.btnSearchSelectedSubstrate.ThemeIndex = 0;
            this.btnSearchSelectedSubstrate.UseBorder = false;
            this.btnSearchSelectedSubstrate.UseClickedEmphasizeTextColor = false;
            this.btnSearchSelectedSubstrate.UseCustomizeClickedColor = true;
            this.btnSearchSelectedSubstrate.UseEdge = false;
            this.btnSearchSelectedSubstrate.UseHoverEmphasizeCustomColor = false;
            this.btnSearchSelectedSubstrate.UseImage = false;
            this.btnSearchSelectedSubstrate.UserHoverEmpahsize = false;
            this.btnSearchSelectedSubstrate.UseSubFont = false;
            this.btnSearchSelectedSubstrate.Click += new System.EventHandler(this.BtnSearchSelectedSubstrateClicked);
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
            this.btnExtra.Location = new System.Drawing.Point(422, 2);
            this.btnExtra.MainFont = new System.Drawing.Font("맑은 고딕", 12F, System.Drawing.FontStyle.Bold);
            this.btnExtra.MainFontColor = System.Drawing.Color.SeaGreen;
            this.btnExtra.Margin = new System.Windows.Forms.Padding(2);
            this.btnExtra.Name = "btnExtra";
            this.btnExtra.Size = new System.Drawing.Size(206, 30);
            this.btnExtra.SubFont = new System.Drawing.Font("맑은 고딕", 10F);
            this.btnExtra.SubFontColor = System.Drawing.Color.SeaGreen;
            this.btnExtra.SubText = "";
            this.btnExtra.TabIndex = 2;
            this.btnExtra.Tag = "";
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
            this.btnExtra.Click += new System.EventHandler(this.CarrierPageButtonClicked);
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
            this.btnLotHistory.Click += new System.EventHandler(this.CarrierPageButtonClicked);
            // 
            // btnSubstratesInCarrier
            // 
            this.btnSubstratesInCarrier.BorderWidth = 2;
            this.btnSubstratesInCarrier.ButtonClicked = false;
            this.btnSubstratesInCarrier.ClickedEmphasizeTextColor = System.Drawing.Color.White;
            this.btnSubstratesInCarrier.CustomClickedGradientFirstColor = System.Drawing.Color.SeaGreen;
            this.btnSubstratesInCarrier.CustomClickedGradientSecondColor = System.Drawing.Color.SeaGreen;
            this.btnSubstratesInCarrier.Description = "";
            this.btnSubstratesInCarrier.DisabledColor = System.Drawing.Color.DarkGray;
            this.btnSubstratesInCarrier.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnSubstratesInCarrier.EdgeRadius = 5;
            this.btnSubstratesInCarrier.GradientAngle = 70F;
            this.btnSubstratesInCarrier.GradientFirstColor = System.Drawing.Color.White;
            this.btnSubstratesInCarrier.GradientSecondColor = System.Drawing.Color.White;
            this.btnSubstratesInCarrier.HoverEmphasizeCustomColor = System.Drawing.Color.White;
            this.btnSubstratesInCarrier.ImagePosition = new System.Drawing.Point(10, 10);
            this.btnSubstratesInCarrier.ImageSize = new System.Drawing.Point(15, 15);
            this.btnSubstratesInCarrier.LoadImage = ((System.Drawing.Image)(resources.GetObject("btnSubstratesInCarrier.LoadImage")));
            this.btnSubstratesInCarrier.Location = new System.Drawing.Point(2, 2);
            this.btnSubstratesInCarrier.MainFont = new System.Drawing.Font("맑은 고딕", 12F, System.Drawing.FontStyle.Bold);
            this.btnSubstratesInCarrier.MainFontColor = System.Drawing.Color.SeaGreen;
            this.btnSubstratesInCarrier.Margin = new System.Windows.Forms.Padding(2);
            this.btnSubstratesInCarrier.Name = "btnSubstratesInCarrier";
            this.btnSubstratesInCarrier.Size = new System.Drawing.Size(206, 30);
            this.btnSubstratesInCarrier.SubFont = new System.Drawing.Font("맑은 고딕", 10F);
            this.btnSubstratesInCarrier.SubFontColor = System.Drawing.Color.SeaGreen;
            this.btnSubstratesInCarrier.SubText = "";
            this.btnSubstratesInCarrier.TabIndex = 0;
            this.btnSubstratesInCarrier.Tag = "";
            this.btnSubstratesInCarrier.Text = "Substrates In Carrier";
            this.btnSubstratesInCarrier.TextAlignMain = Sys3Controls.EN_TEXTALIGN.MIDDLE_CENTER;
            this.btnSubstratesInCarrier.TextAlignSub = Sys3Controls.EN_TEXTALIGN.MIDDLE_CENTER;
            this.btnSubstratesInCarrier.ThemeIndex = 0;
            this.btnSubstratesInCarrier.UseBorder = false;
            this.btnSubstratesInCarrier.UseClickedEmphasizeTextColor = false;
            this.btnSubstratesInCarrier.UseCustomizeClickedColor = true;
            this.btnSubstratesInCarrier.UseEdge = false;
            this.btnSubstratesInCarrier.UseHoverEmphasizeCustomColor = false;
            this.btnSubstratesInCarrier.UseImage = false;
            this.btnSubstratesInCarrier.UserHoverEmpahsize = false;
            this.btnSubstratesInCarrier.UseSubFont = false;
            this.btnSubstratesInCarrier.Click += new System.EventHandler(this.CarrierPageButtonClicked);
            // 
            // _pnlCarrierPagesHost
            // 
            this._pnlCarrierPagesHost.Controls.Add(this._pnlCarrierPageExtra);
            this._pnlCarrierPagesHost.Controls.Add(this._pnlCarrierPageHistory);
            this._pnlCarrierPagesHost.Controls.Add(this._pnlCarrierPageSubstrates);
            this._pnlCarrierPagesHost.Dock = System.Windows.Forms.DockStyle.Fill;
            this._pnlCarrierPagesHost.Location = new System.Drawing.Point(3, 43);
            this._pnlCarrierPagesHost.Name = "_pnlCarrierPagesHost";
            this._pnlCarrierPagesHost.Size = new System.Drawing.Size(1134, 338);
            this._pnlCarrierPagesHost.TabIndex = 1;
            // 
            // _pnlCarrierPageExtra
            // 
            this._pnlCarrierPageExtra.Controls.Add(this._tlpCarrierExtraLayout);
            this._pnlCarrierPageExtra.Dock = System.Windows.Forms.DockStyle.Fill;
            this._pnlCarrierPageExtra.Location = new System.Drawing.Point(0, 0);
            this._pnlCarrierPageExtra.Name = "_pnlCarrierPageExtra";
            this._pnlCarrierPageExtra.Size = new System.Drawing.Size(1134, 338);
            this._pnlCarrierPageExtra.TabIndex = 0;
            // 
            // _tlpCarrierExtraLayout
            // 
            this._tlpCarrierExtraLayout.ColumnCount = 1;
            this._tlpCarrierExtraLayout.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this._tlpCarrierExtraLayout.Controls.Add(this._pnlCarrierSlotMapSection, 0, 0);
            this._tlpCarrierExtraLayout.Controls.Add(this._pnlCarrierExtraSection, 0, 1);
            this._tlpCarrierExtraLayout.Dock = System.Windows.Forms.DockStyle.Fill;
            this._tlpCarrierExtraLayout.Location = new System.Drawing.Point(0, 0);
            this._tlpCarrierExtraLayout.Name = "_tlpCarrierExtraLayout";
            this._tlpCarrierExtraLayout.RowCount = 2;
            this._tlpCarrierExtraLayout.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 40F));
            this._tlpCarrierExtraLayout.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 60F));
            this._tlpCarrierExtraLayout.Size = new System.Drawing.Size(1134, 338);
            this._tlpCarrierExtraLayout.TabIndex = 0;
            // 
            // _pnlCarrierSlotMapSection
            // 
            this._pnlCarrierSlotMapSection.Controls.Add(this._gvCarrierSlotMap);
            this._pnlCarrierSlotMapSection.Controls.Add(this._lblCarrierSlotMapHeader);
            this._pnlCarrierSlotMapSection.Dock = System.Windows.Forms.DockStyle.Fill;
            this._pnlCarrierSlotMapSection.Location = new System.Drawing.Point(3, 3);
            this._pnlCarrierSlotMapSection.Name = "_pnlCarrierSlotMapSection";
            this._pnlCarrierSlotMapSection.Size = new System.Drawing.Size(1128, 129);
            this._pnlCarrierSlotMapSection.TabIndex = 0;
            // 
            // _gvCarrierSlotMap
            // 
            this._gvCarrierSlotMap.AllowUserToAddRows = false;
            this._gvCarrierSlotMap.AllowUserToDeleteRows = false;
            this._gvCarrierSlotMap.AllowUserToResizeRows = false;
            this._gvCarrierSlotMap.BackgroundColor = System.Drawing.Color.White;
            this._gvCarrierSlotMap.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this._gvCarrierSlotMap.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this._colCarrierSlotMapSlot,
            this._colCarrierSlotMapMap});
            this._gvCarrierSlotMap.Dock = System.Windows.Forms.DockStyle.Fill;
            this._gvCarrierSlotMap.EditMode = System.Windows.Forms.DataGridViewEditMode.EditProgrammatically;
            this._gvCarrierSlotMap.Location = new System.Drawing.Point(0, 24);
            this._gvCarrierSlotMap.MultiSelect = false;
            this._gvCarrierSlotMap.Name = "_gvCarrierSlotMap";
            this._gvCarrierSlotMap.RowHeadersVisible = false;
            this._gvCarrierSlotMap.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this._gvCarrierSlotMap.Size = new System.Drawing.Size(1128, 105);
            this._gvCarrierSlotMap.TabIndex = 1;
            // 
            // _colCarrierSlotMapSlot
            // 
            this._colCarrierSlotMapSlot.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this._colCarrierSlotMapSlot.HeaderText = "Slot";
            this._colCarrierSlotMapSlot.Name = "_colCarrierSlotMapSlot";
            this._colCarrierSlotMapSlot.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // _colCarrierSlotMapMap
            // 
            this._colCarrierSlotMapMap.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this._colCarrierSlotMapMap.HeaderText = "Map";
            this._colCarrierSlotMapMap.Name = "_colCarrierSlotMapMap";
            this._colCarrierSlotMapMap.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // _lblCarrierSlotMapHeader
            // 
            this._lblCarrierSlotMapHeader.BackGroundColor = System.Drawing.Color.LightSteelBlue;
            this._lblCarrierSlotMapHeader.BorderStroke = 1;
            this._lblCarrierSlotMapHeader.BorderStyle = System.Windows.Forms.ButtonBorderStyle.Solid;
            this._lblCarrierSlotMapHeader.Description = "";
            this._lblCarrierSlotMapHeader.DisabledColor = System.Drawing.Color.DarkGray;
            this._lblCarrierSlotMapHeader.Dock = System.Windows.Forms.DockStyle.Top;
            this._lblCarrierSlotMapHeader.EdgeRadius = 1;
            this._lblCarrierSlotMapHeader.ImagePosition = new System.Drawing.Point(0, 0);
            this._lblCarrierSlotMapHeader.ImageSize = new System.Drawing.Point(0, 0);
            this._lblCarrierSlotMapHeader.LoadImage = null;
            this._lblCarrierSlotMapHeader.Location = new System.Drawing.Point(0, 0);
            this._lblCarrierSlotMapHeader.MainFont = new System.Drawing.Font("맑은 고딕", 9F, System.Drawing.FontStyle.Bold);
            this._lblCarrierSlotMapHeader.MainFontColor = System.Drawing.Color.Black;
            this._lblCarrierSlotMapHeader.Name = "_lblCarrierSlotMapHeader";
            this._lblCarrierSlotMapHeader.Size = new System.Drawing.Size(1128, 24);
            this._lblCarrierSlotMapHeader.SubFont = new System.Drawing.Font("맑은 고딕", 10F);
            this._lblCarrierSlotMapHeader.SubFontColor = System.Drawing.Color.Black;
            this._lblCarrierSlotMapHeader.SubText = "";
            this._lblCarrierSlotMapHeader.TabIndex = 2;
            this._lblCarrierSlotMapHeader.Text = "Slot Map";
            this._lblCarrierSlotMapHeader.TextAlignMain = Sys3Controls.EN_TEXTALIGN.MIDDLE_LEFT;
            this._lblCarrierSlotMapHeader.TextAlignSub = Sys3Controls.EN_TEXTALIGN.MIDDLE_CENTER;
            this._lblCarrierSlotMapHeader.TextAlignUnit = Sys3Controls.EN_TEXTALIGN.MIDDLE_CENTER;
            this._lblCarrierSlotMapHeader.ThemeIndex = 0;
            this._lblCarrierSlotMapHeader.UnitAreaRate = 30;
            this._lblCarrierSlotMapHeader.UnitFont = new System.Drawing.Font("맑은 고딕", 10F, System.Drawing.FontStyle.Bold);
            this._lblCarrierSlotMapHeader.UnitFontColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this._lblCarrierSlotMapHeader.UnitPositionVertical = false;
            this._lblCarrierSlotMapHeader.UnitText = "";
            this._lblCarrierSlotMapHeader.UseBorder = true;
            this._lblCarrierSlotMapHeader.UseEdgeRadius = false;
            this._lblCarrierSlotMapHeader.UseImage = false;
            this._lblCarrierSlotMapHeader.UseSubFont = false;
            this._lblCarrierSlotMapHeader.UseUnitFont = false;
            // 
            // _pnlCarrierExtraSection
            // 
            this._pnlCarrierExtraSection.Controls.Add(this._gvCarrierExtra);
            this._pnlCarrierExtraSection.Controls.Add(this._lblCarrierExtraSectionHeader);
            this._pnlCarrierExtraSection.Dock = System.Windows.Forms.DockStyle.Fill;
            this._pnlCarrierExtraSection.Location = new System.Drawing.Point(3, 138);
            this._pnlCarrierExtraSection.Name = "_pnlCarrierExtraSection";
            this._pnlCarrierExtraSection.Size = new System.Drawing.Size(1128, 197);
            this._pnlCarrierExtraSection.TabIndex = 1;
            // 
            // _gvCarrierExtra
            // 
            this._gvCarrierExtra.AllowUserToAddRows = false;
            this._gvCarrierExtra.AllowUserToDeleteRows = false;
            this._gvCarrierExtra.AllowUserToResizeRows = false;
            this._gvCarrierExtra.BackgroundColor = System.Drawing.Color.White;
            this._gvCarrierExtra.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this._gvCarrierExtra.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this._colCarrierExtraKey,
            this._colCarrierExtraValue});
            this._gvCarrierExtra.Dock = System.Windows.Forms.DockStyle.Fill;
            this._gvCarrierExtra.EditMode = System.Windows.Forms.DataGridViewEditMode.EditProgrammatically;
            this._gvCarrierExtra.Location = new System.Drawing.Point(0, 24);
            this._gvCarrierExtra.MultiSelect = false;
            this._gvCarrierExtra.Name = "_gvCarrierExtra";
            this._gvCarrierExtra.RowHeadersVisible = false;
            this._gvCarrierExtra.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this._gvCarrierExtra.Size = new System.Drawing.Size(1128, 173);
            this._gvCarrierExtra.TabIndex = 1;
            // 
            // _colCarrierExtraKey
            // 
            this._colCarrierExtraKey.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this._colCarrierExtraKey.HeaderText = "Extra Key";
            this._colCarrierExtraKey.Name = "_colCarrierExtraKey";
            this._colCarrierExtraKey.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // _colCarrierExtraValue
            // 
            this._colCarrierExtraValue.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this._colCarrierExtraValue.HeaderText = "값";
            this._colCarrierExtraValue.Name = "_colCarrierExtraValue";
            this._colCarrierExtraValue.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // _lblCarrierExtraSectionHeader
            // 
            this._lblCarrierExtraSectionHeader.BackGroundColor = System.Drawing.Color.LightSteelBlue;
            this._lblCarrierExtraSectionHeader.BorderStroke = 1;
            this._lblCarrierExtraSectionHeader.BorderStyle = System.Windows.Forms.ButtonBorderStyle.Solid;
            this._lblCarrierExtraSectionHeader.Description = "";
            this._lblCarrierExtraSectionHeader.DisabledColor = System.Drawing.Color.DarkGray;
            this._lblCarrierExtraSectionHeader.Dock = System.Windows.Forms.DockStyle.Top;
            this._lblCarrierExtraSectionHeader.EdgeRadius = 1;
            this._lblCarrierExtraSectionHeader.ImagePosition = new System.Drawing.Point(0, 0);
            this._lblCarrierExtraSectionHeader.ImageSize = new System.Drawing.Point(0, 0);
            this._lblCarrierExtraSectionHeader.LoadImage = null;
            this._lblCarrierExtraSectionHeader.Location = new System.Drawing.Point(0, 0);
            this._lblCarrierExtraSectionHeader.MainFont = new System.Drawing.Font("맑은 고딕", 9F, System.Drawing.FontStyle.Bold);
            this._lblCarrierExtraSectionHeader.MainFontColor = System.Drawing.Color.Black;
            this._lblCarrierExtraSectionHeader.Name = "_lblCarrierExtraSectionHeader";
            this._lblCarrierExtraSectionHeader.Size = new System.Drawing.Size(1128, 24);
            this._lblCarrierExtraSectionHeader.SubFont = new System.Drawing.Font("맑은 고딕", 10F);
            this._lblCarrierExtraSectionHeader.SubFontColor = System.Drawing.Color.Black;
            this._lblCarrierExtraSectionHeader.SubText = "";
            this._lblCarrierExtraSectionHeader.TabIndex = 2;
            this._lblCarrierExtraSectionHeader.Text = "Extra";
            this._lblCarrierExtraSectionHeader.TextAlignMain = Sys3Controls.EN_TEXTALIGN.MIDDLE_LEFT;
            this._lblCarrierExtraSectionHeader.TextAlignSub = Sys3Controls.EN_TEXTALIGN.MIDDLE_CENTER;
            this._lblCarrierExtraSectionHeader.TextAlignUnit = Sys3Controls.EN_TEXTALIGN.MIDDLE_CENTER;
            this._lblCarrierExtraSectionHeader.ThemeIndex = 0;
            this._lblCarrierExtraSectionHeader.UnitAreaRate = 30;
            this._lblCarrierExtraSectionHeader.UnitFont = new System.Drawing.Font("맑은 고딕", 10F, System.Drawing.FontStyle.Bold);
            this._lblCarrierExtraSectionHeader.UnitFontColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this._lblCarrierExtraSectionHeader.UnitPositionVertical = false;
            this._lblCarrierExtraSectionHeader.UnitText = "";
            this._lblCarrierExtraSectionHeader.UseBorder = true;
            this._lblCarrierExtraSectionHeader.UseEdgeRadius = false;
            this._lblCarrierExtraSectionHeader.UseImage = false;
            this._lblCarrierExtraSectionHeader.UseSubFont = false;
            this._lblCarrierExtraSectionHeader.UseUnitFont = false;
            // 
            // _pnlCarrierPageHistory
            // 
            this._pnlCarrierPageHistory.Controls.Add(this._gvCarrierLotHistory);
            this._pnlCarrierPageHistory.Dock = System.Windows.Forms.DockStyle.Fill;
            this._pnlCarrierPageHistory.Location = new System.Drawing.Point(0, 0);
            this._pnlCarrierPageHistory.Name = "_pnlCarrierPageHistory";
            this._pnlCarrierPageHistory.Size = new System.Drawing.Size(1134, 338);
            this._pnlCarrierPageHistory.TabIndex = 1;
            this._pnlCarrierPageHistory.Visible = false;
            // 
            // _gvCarrierLotHistory
            // 
            this._gvCarrierLotHistory.AllowUserToAddRows = false;
            this._gvCarrierLotHistory.AllowUserToDeleteRows = false;
            this._gvCarrierLotHistory.AllowUserToResizeRows = false;
            this._gvCarrierLotHistory.BackgroundColor = System.Drawing.Color.White;
            this._gvCarrierLotHistory.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this._gvCarrierLotHistory.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this._colCarrierHistTime,
            this._colCarrierHistPortEvent,
            this._colCarrierHistWafer,
            this._colCarrierHistWaferEvent,
            this._colCarrierHistMessage});
            this._gvCarrierLotHistory.Dock = System.Windows.Forms.DockStyle.Fill;
            this._gvCarrierLotHistory.EditMode = System.Windows.Forms.DataGridViewEditMode.EditProgrammatically;
            this._gvCarrierLotHistory.Location = new System.Drawing.Point(0, 0);
            this._gvCarrierLotHistory.MultiSelect = false;
            this._gvCarrierLotHistory.Name = "_gvCarrierLotHistory";
            this._gvCarrierLotHistory.RowHeadersVisible = false;
            this._gvCarrierLotHistory.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this._gvCarrierLotHistory.Size = new System.Drawing.Size(1134, 338);
            this._gvCarrierLotHistory.TabIndex = 0;
            // 
            // _colCarrierHistTime
            // 
            this._colCarrierHistTime.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this._colCarrierHistTime.HeaderText = "TIME";
            this._colCarrierHistTime.Name = "_colCarrierHistTime";
            this._colCarrierHistTime.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // _colCarrierHistPortEvent
            // 
            this._colCarrierHistPortEvent.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this._colCarrierHistPortEvent.HeaderText = "PORT EVENT";
            this._colCarrierHistPortEvent.Name = "_colCarrierHistPortEvent";
            this._colCarrierHistPortEvent.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // _colCarrierHistWafer
            // 
            this._colCarrierHistWafer.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this._colCarrierHistWafer.HeaderText = "WAFER";
            this._colCarrierHistWafer.Name = "_colCarrierHistWafer";
            this._colCarrierHistWafer.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // _colCarrierHistWaferEvent
            // 
            this._colCarrierHistWaferEvent.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this._colCarrierHistWaferEvent.HeaderText = "WAFER EVENT";
            this._colCarrierHistWaferEvent.Name = "_colCarrierHistWaferEvent";
            this._colCarrierHistWaferEvent.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // _colCarrierHistMessage
            // 
            this._colCarrierHistMessage.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this._colCarrierHistMessage.HeaderText = "MESSAGE";
            this._colCarrierHistMessage.Name = "_colCarrierHistMessage";
            this._colCarrierHistMessage.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // _pnlCarrierPageSubstrates
            // 
            this._pnlCarrierPageSubstrates.Controls.Add(this._gvSubstratesInCarrier);
            this._pnlCarrierPageSubstrates.Dock = System.Windows.Forms.DockStyle.Fill;
            this._pnlCarrierPageSubstrates.Location = new System.Drawing.Point(0, 0);
            this._pnlCarrierPageSubstrates.Name = "_pnlCarrierPageSubstrates";
            this._pnlCarrierPageSubstrates.Size = new System.Drawing.Size(1134, 338);
            this._pnlCarrierPageSubstrates.TabIndex = 2;
            this._pnlCarrierPageSubstrates.Visible = false;
            // 
            // _gvSubstratesInCarrier
            // 
            this._gvSubstratesInCarrier.AllowUserToAddRows = false;
            this._gvSubstratesInCarrier.AllowUserToDeleteRows = false;
            this._gvSubstratesInCarrier.AllowUserToResizeRows = false;
            this._gvSubstratesInCarrier.BackgroundColor = System.Drawing.Color.White;
            this._gvSubstratesInCarrier.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this._gvSubstratesInCarrier.Dock = System.Windows.Forms.DockStyle.Fill;
            this._gvSubstratesInCarrier.EditMode = System.Windows.Forms.DataGridViewEditMode.EditProgrammatically;
            this._gvSubstratesInCarrier.Location = new System.Drawing.Point(0, 0);
            this._gvSubstratesInCarrier.MultiSelect = false;
            this._gvSubstratesInCarrier.Name = "_gvSubstratesInCarrier";
            this._gvSubstratesInCarrier.RowHeadersVisible = false;
            this._gvSubstratesInCarrier.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this._gvSubstratesInCarrier.Size = new System.Drawing.Size(1134, 338);
            this._gvSubstratesInCarrier.TabIndex = 0;
            this._gvSubstratesInCarrier.CellDoubleClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.GvSubstratesInCarrierCellDoubleClick);
            // 
            // CarrierDetailView
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.Controls.Add(this._tlpCarrierDetailWrapper);
            this.Name = "CarrierDetailView";
            this.Size = new System.Drawing.Size(1140, 384);
            this._tlpCarrierDetailWrapper.ResumeLayout(false);
            this._pnlSubstratePageBar.ResumeLayout(false);
            this._pnlCarrierPagesHost.ResumeLayout(false);
            this._pnlCarrierPageExtra.ResumeLayout(false);
            this._tlpCarrierExtraLayout.ResumeLayout(false);
            this._pnlCarrierSlotMapSection.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this._gvCarrierSlotMap)).EndInit();
            this._pnlCarrierExtraSection.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this._gvCarrierExtra)).EndInit();
            this._pnlCarrierPageHistory.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this._gvCarrierLotHistory)).EndInit();
            this._pnlCarrierPageSubstrates.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this._gvSubstratesInCarrier)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel _tlpCarrierDetailWrapper;
        private System.Windows.Forms.Panel _pnlCarrierPagesHost;
        private System.Windows.Forms.Panel _pnlCarrierPageExtra;
        private System.Windows.Forms.TableLayoutPanel _tlpCarrierExtraLayout;
        private System.Windows.Forms.Panel _pnlCarrierSlotMapSection;
        private Sys3Controls.Sys3DoubleBufferedDataGridView _gvCarrierSlotMap;
        private System.Windows.Forms.DataGridViewTextBoxColumn _colCarrierSlotMapSlot;
        private System.Windows.Forms.DataGridViewTextBoxColumn _colCarrierSlotMapMap;
        private Sys3Controls.Sys3Label _lblCarrierSlotMapHeader;
        private System.Windows.Forms.Panel _pnlCarrierExtraSection;
        private Sys3Controls.Sys3DoubleBufferedDataGridView _gvCarrierExtra;
        private System.Windows.Forms.DataGridViewTextBoxColumn _colCarrierExtraKey;
        private System.Windows.Forms.DataGridViewTextBoxColumn _colCarrierExtraValue;
        private Sys3Controls.Sys3Label _lblCarrierExtraSectionHeader;
        private System.Windows.Forms.Panel _pnlCarrierPageHistory;
        private Sys3Controls.Sys3DoubleBufferedDataGridView _gvCarrierLotHistory;
        private System.Windows.Forms.DataGridViewTextBoxColumn _colCarrierHistTime;
        private System.Windows.Forms.DataGridViewTextBoxColumn _colCarrierHistPortEvent;
        private System.Windows.Forms.DataGridViewTextBoxColumn _colCarrierHistWafer;
        private System.Windows.Forms.DataGridViewTextBoxColumn _colCarrierHistWaferEvent;
        private System.Windows.Forms.DataGridViewTextBoxColumn _colCarrierHistMessage;
        private System.Windows.Forms.Panel _pnlCarrierPageSubstrates;
        private Sys3Controls.Sys3DoubleBufferedDataGridView _gvSubstratesInCarrier;
        private System.Windows.Forms.TableLayoutPanel _pnlSubstratePageBar;
        private Sys3Controls.Sys3button btnSearchSelectedSubstrate;
        private Sys3Controls.Sys3button btnExtra;
        private Sys3Controls.Sys3button btnLotHistory;
        private Sys3Controls.Sys3button btnSubstratesInCarrier;
    }
}
