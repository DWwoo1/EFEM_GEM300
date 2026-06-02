namespace FrameOfSystem3.Views.Config.MotionPanel
{
    partial class MotorSpeed
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
            this.m_ledCustom2 = new Sys3Controls.Sys3LedLabel();
            this.m_ledCustom1 = new Sys3Controls.Sys3LedLabel();
            this.m_ledManual = new Sys3Controls.Sys3LedLabel();
            this.m_ledJogHigh = new Sys3Controls.Sys3LedLabel();
            this.m_ledJogLow = new Sys3Controls.Sys3LedLabel();
            this.m_ledRun = new Sys3Controls.Sys3LedLabel();
            this.m_groupSpeedContents = new Sys3Controls.Sys3GroupBoxContainer();
            this._tableLayoutPanel_GantryConfiguration = new System.Windows.Forms.TableLayoutPanel();
            this.m_labelRun = new Sys3Controls.Sys3Label();
            this.m_labelJogLow = new Sys3Controls.Sys3Label();
            this.m_labelJogHigh = new Sys3Controls.Sys3Label();
            this.m_labelManual = new Sys3Controls.Sys3Label();
            this.m_labelCustom1 = new Sys3Controls.Sys3Label();
            this.m_labelCustom2 = new Sys3Controls.Sys3Label();
            this.m_groupSelectedContents = new Sys3Controls.Sys3GroupBoxContainer();
            this.tableLayoutPanel2 = new System.Windows.Forms.TableLayoutPanel();
            this.m_lblName = new Sys3Controls.Sys3Label();
            this.m_labelName = new Sys3Controls.Sys3Label();
            this.m_labelShortDistanceAuto = new Sys3Controls.Sys3Label();
            this.m_lblShortDistanceAuto = new Sys3Controls.Sys3Label();
            this.m_labelShortDistance = new Sys3Controls.Sys3Label();
            this.m_lblShortDistance = new Sys3Controls.Sys3Label();
            this.m_groupSpeedParameters = new Sys3Controls.Sys3GroupBoxContainer();
            this.tableLayoutPanel3 = new System.Windows.Forms.TableLayoutPanel();
            this.m_lblSpeedPattern = new Sys3Controls.Sys3Label();
            this.m_btnCopy = new Sys3Controls.Sys3button();
            this.m_labelSpeedPattern = new Sys3Controls.Sys3Label();
            this.m_labelJerkDecel = new Sys3Controls.Sys3Label();
            this.m_lblJerkDecel = new Sys3Controls.Sys3Label();
            this.m_labelDecelTime = new Sys3Controls.Sys3Label();
            this.m_labelDecel = new Sys3Controls.Sys3Label();
            this.m_lblDecelTime = new Sys3Controls.Sys3Label();
            this.m_lblDecel = new Sys3Controls.Sys3Label();
            this.m_labelMaxVelocity = new Sys3Controls.Sys3Label();
            this.m_lblTimeout = new Sys3Controls.Sys3Label();
            this.m_labelTimeout = new Sys3Controls.Sys3Label();
            this.m_lblVelocity = new Sys3Controls.Sys3Label();
            this.m_labelVelocity = new Sys3Controls.Sys3Label();
            this.m_lblJerkAccel = new Sys3Controls.Sys3Label();
            this.m_lblAccel = new Sys3Controls.Sys3Label();
            this.m_lblAccelTime = new Sys3Controls.Sys3Label();
            this.m_labelAccel = new Sys3Controls.Sys3Label();
            this.m_labelJerkAccel = new Sys3Controls.Sys3Label();
            this.m_labelAccelTime = new Sys3Controls.Sys3Label();
            this.m_lblMaxVelocity = new Sys3Controls.Sys3Label();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.m_groupSpeedContents.SuspendLayout();
            this._tableLayoutPanel_GantryConfiguration.SuspendLayout();
            this.m_groupSelectedContents.SuspendLayout();
            this.tableLayoutPanel2.SuspendLayout();
            this.m_groupSpeedParameters.SuspendLayout();
            this.tableLayoutPanel3.SuspendLayout();
            this.tableLayoutPanel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // m_ledCustom2
            // 
            this.m_ledCustom2.Active = false;
            this.m_ledCustom2.ColorActive = System.Drawing.Color.FromArgb(((int)(((byte)(75)))), ((int)(((byte)(216)))), ((int)(((byte)(101)))));
            this.m_ledCustom2.ColorNormal = System.Drawing.Color.DimGray;
            this.m_ledCustom2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.m_ledCustom2.Location = new System.Drawing.Point(10, 308);
            this.m_ledCustom2.Margin = new System.Windows.Forms.Padding(0, 3, 0, 3);
            this.m_ledCustom2.Name = "m_ledCustom2";
            this.m_ledCustom2.Size = new System.Drawing.Size(28, 55);
            this.m_ledCustom2.TabIndex = 1143;
            // 
            // m_ledCustom1
            // 
            this.m_ledCustom1.Active = false;
            this.m_ledCustom1.ColorActive = System.Drawing.Color.FromArgb(((int)(((byte)(75)))), ((int)(((byte)(216)))), ((int)(((byte)(101)))));
            this.m_ledCustom1.ColorNormal = System.Drawing.Color.DimGray;
            this.m_ledCustom1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.m_ledCustom1.Location = new System.Drawing.Point(10, 249);
            this.m_ledCustom1.Margin = new System.Windows.Forms.Padding(0, 3, 0, 3);
            this.m_ledCustom1.Name = "m_ledCustom1";
            this.m_ledCustom1.Size = new System.Drawing.Size(28, 53);
            this.m_ledCustom1.TabIndex = 1142;
            // 
            // m_ledManual
            // 
            this.m_ledManual.Active = false;
            this.m_ledManual.ColorActive = System.Drawing.Color.FromArgb(((int)(((byte)(75)))), ((int)(((byte)(216)))), ((int)(((byte)(101)))));
            this.m_ledManual.ColorNormal = System.Drawing.Color.DimGray;
            this.m_ledManual.Dock = System.Windows.Forms.DockStyle.Fill;
            this.m_ledManual.Location = new System.Drawing.Point(10, 190);
            this.m_ledManual.Margin = new System.Windows.Forms.Padding(0, 3, 0, 3);
            this.m_ledManual.Name = "m_ledManual";
            this.m_ledManual.Size = new System.Drawing.Size(28, 53);
            this.m_ledManual.TabIndex = 1141;
            // 
            // m_ledJogHigh
            // 
            this.m_ledJogHigh.Active = false;
            this.m_ledJogHigh.ColorActive = System.Drawing.Color.FromArgb(((int)(((byte)(75)))), ((int)(((byte)(216)))), ((int)(((byte)(101)))));
            this.m_ledJogHigh.ColorNormal = System.Drawing.Color.DimGray;
            this.m_ledJogHigh.Dock = System.Windows.Forms.DockStyle.Fill;
            this.m_ledJogHigh.Location = new System.Drawing.Point(10, 131);
            this.m_ledJogHigh.Margin = new System.Windows.Forms.Padding(0, 3, 0, 3);
            this.m_ledJogHigh.Name = "m_ledJogHigh";
            this.m_ledJogHigh.Size = new System.Drawing.Size(28, 53);
            this.m_ledJogHigh.TabIndex = 1140;
            // 
            // m_ledJogLow
            // 
            this.m_ledJogLow.Active = false;
            this.m_ledJogLow.ColorActive = System.Drawing.Color.FromArgb(((int)(((byte)(75)))), ((int)(((byte)(216)))), ((int)(((byte)(101)))));
            this.m_ledJogLow.ColorNormal = System.Drawing.Color.DimGray;
            this.m_ledJogLow.Dock = System.Windows.Forms.DockStyle.Fill;
            this.m_ledJogLow.Location = new System.Drawing.Point(10, 72);
            this.m_ledJogLow.Margin = new System.Windows.Forms.Padding(0, 3, 0, 3);
            this.m_ledJogLow.Name = "m_ledJogLow";
            this.m_ledJogLow.Size = new System.Drawing.Size(28, 53);
            this.m_ledJogLow.TabIndex = 1138;
            // 
            // m_ledRun
            // 
            this.m_ledRun.Active = false;
            this.m_ledRun.ColorActive = System.Drawing.Color.FromArgb(((int)(((byte)(75)))), ((int)(((byte)(216)))), ((int)(((byte)(101)))));
            this.m_ledRun.ColorNormal = System.Drawing.Color.DimGray;
            this.m_ledRun.Dock = System.Windows.Forms.DockStyle.Fill;
            this.m_ledRun.Location = new System.Drawing.Point(10, 13);
            this.m_ledRun.Margin = new System.Windows.Forms.Padding(0, 3, 0, 3);
            this.m_ledRun.Name = "m_ledRun";
            this.m_ledRun.Size = new System.Drawing.Size(28, 53);
            this.m_ledRun.TabIndex = 1134;
            // 
            // m_groupSpeedContents
            // 
            this.m_groupSpeedContents.BackGroundColor = System.Drawing.Color.WhiteSmoke;
            this.m_groupSpeedContents.Controls.Add(this._tableLayoutPanel_GantryConfiguration);
            this.m_groupSpeedContents.Dock = System.Windows.Forms.DockStyle.Fill;
            this.m_groupSpeedContents.EdgeBorderStroke = 2;
            this.m_groupSpeedContents.EdgeRadius = 2;
            this.m_groupSpeedContents.LabelFont = new System.Drawing.Font("맑은 고딕", 10F, System.Drawing.FontStyle.Bold);
            this.m_groupSpeedContents.LabelGradientColorFirst = System.Drawing.Color.FromArgb(((int)(((byte)(208)))), ((int)(((byte)(208)))), ((int)(((byte)(208)))));
            this.m_groupSpeedContents.LabelGradientColorSecond = System.Drawing.Color.FromArgb(((int)(((byte)(240)))), ((int)(((byte)(240)))), ((int)(((byte)(240)))));
            this.m_groupSpeedContents.LabelHeight = 30;
            this.m_groupSpeedContents.LabelTextColor = System.Drawing.Color.Black;
            this.m_groupSpeedContents.Location = new System.Drawing.Point(0, 0);
            this.m_groupSpeedContents.Margin = new System.Windows.Forms.Padding(0);
            this.m_groupSpeedContents.Name = "m_groupSpeedContents";
            this.m_groupSpeedContents.Padding = new System.Windows.Forms.Padding(3, 18, 3, 3);
            this.tableLayoutPanel1.SetRowSpan(this.m_groupSpeedContents, 2);
            this.m_groupSpeedContents.Size = new System.Drawing.Size(293, 411);
            this.m_groupSpeedContents.TabIndex = 1371;
            this.m_groupSpeedContents.TabStop = false;
            this.m_groupSpeedContents.Text = "SPEED CONTENTS";
            this.m_groupSpeedContents.TextAlign = Sys3Controls.EN_TEXTALIGN.MIDDLE_CENTER;
            this.m_groupSpeedContents.ThemeIndex = 0;
            this.m_groupSpeedContents.UseLabelBorder = true;
            this.m_groupSpeedContents.UseTitle = true;
            // 
            // _tableLayoutPanel_GantryConfiguration
            // 
            this._tableLayoutPanel_GantryConfiguration.ColumnCount = 2;
            this._tableLayoutPanel_GantryConfiguration.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 28F));
            this._tableLayoutPanel_GantryConfiguration.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this._tableLayoutPanel_GantryConfiguration.Controls.Add(this.m_ledRun, 0, 0);
            this._tableLayoutPanel_GantryConfiguration.Controls.Add(this.m_ledJogLow, 0, 1);
            this._tableLayoutPanel_GantryConfiguration.Controls.Add(this.m_ledJogHigh, 0, 2);
            this._tableLayoutPanel_GantryConfiguration.Controls.Add(this.m_ledManual, 0, 3);
            this._tableLayoutPanel_GantryConfiguration.Controls.Add(this.m_ledCustom1, 0, 4);
            this._tableLayoutPanel_GantryConfiguration.Controls.Add(this.m_ledCustom2, 0, 5);
            this._tableLayoutPanel_GantryConfiguration.Controls.Add(this.m_labelRun, 1, 0);
            this._tableLayoutPanel_GantryConfiguration.Controls.Add(this.m_labelJogLow, 1, 1);
            this._tableLayoutPanel_GantryConfiguration.Controls.Add(this.m_labelJogHigh, 1, 2);
            this._tableLayoutPanel_GantryConfiguration.Controls.Add(this.m_labelManual, 1, 3);
            this._tableLayoutPanel_GantryConfiguration.Controls.Add(this.m_labelCustom1, 1, 4);
            this._tableLayoutPanel_GantryConfiguration.Controls.Add(this.m_labelCustom2, 1, 5);
            this._tableLayoutPanel_GantryConfiguration.Dock = System.Windows.Forms.DockStyle.Fill;
            this._tableLayoutPanel_GantryConfiguration.Location = new System.Drawing.Point(3, 32);
            this._tableLayoutPanel_GantryConfiguration.Margin = new System.Windows.Forms.Padding(0);
            this._tableLayoutPanel_GantryConfiguration.Name = "_tableLayoutPanel_GantryConfiguration";
            this._tableLayoutPanel_GantryConfiguration.Padding = new System.Windows.Forms.Padding(10);
            this._tableLayoutPanel_GantryConfiguration.RowCount = 6;
            this._tableLayoutPanel_GantryConfiguration.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 16.66667F));
            this._tableLayoutPanel_GantryConfiguration.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 16.66667F));
            this._tableLayoutPanel_GantryConfiguration.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 16.66667F));
            this._tableLayoutPanel_GantryConfiguration.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 16.66667F));
            this._tableLayoutPanel_GantryConfiguration.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 16.66667F));
            this._tableLayoutPanel_GantryConfiguration.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 16.66667F));
            this._tableLayoutPanel_GantryConfiguration.Size = new System.Drawing.Size(287, 376);
            this._tableLayoutPanel_GantryConfiguration.TabIndex = 1398;
            // 
            // m_labelRun
            // 
            this.m_labelRun.BackGroundColor = System.Drawing.Color.White;
            this.m_labelRun.BorderStroke = 2;
            this.m_labelRun.BorderStyle = System.Windows.Forms.ButtonBorderStyle.Solid;
            this.m_labelRun.Description = "";
            this.m_labelRun.DisabledColor = System.Drawing.Color.LightGray;
            this.m_labelRun.Dock = System.Windows.Forms.DockStyle.Fill;
            this.m_labelRun.EdgeRadius = 1;
            this.m_labelRun.Enabled = false;
            this.m_labelRun.ImagePosition = new System.Drawing.Point(0, 0);
            this.m_labelRun.ImageSize = new System.Drawing.Point(0, 0);
            this.m_labelRun.LoadImage = null;
            this.m_labelRun.Location = new System.Drawing.Point(38, 13);
            this.m_labelRun.MainFont = new System.Drawing.Font("맑은 고딕", 11F, System.Drawing.FontStyle.Bold);
            this.m_labelRun.MainFontColor = System.Drawing.Color.Black;
            this.m_labelRun.Margin = new System.Windows.Forms.Padding(0, 3, 0, 3);
            this.m_labelRun.Name = "m_labelRun";
            this.m_labelRun.Size = new System.Drawing.Size(239, 53);
            this.m_labelRun.SubFont = new System.Drawing.Font("맑은 고딕", 10F);
            this.m_labelRun.SubFontColor = System.Drawing.Color.Black;
            this.m_labelRun.SubText = "";
            this.m_labelRun.TabIndex = 0;
            this.m_labelRun.Text = "RUN";
            this.m_labelRun.TextAlignMain = Sys3Controls.EN_TEXTALIGN.BOTTOM_RIGHT;
            this.m_labelRun.TextAlignSub = Sys3Controls.EN_TEXTALIGN.MIDDLE_CENTER;
            this.m_labelRun.TextAlignUnit = Sys3Controls.EN_TEXTALIGN.MIDDLE_CENTER;
            this.m_labelRun.ThemeIndex = 0;
            this.m_labelRun.UnitAreaRate = 40;
            this.m_labelRun.UnitFont = new System.Drawing.Font("맑은 고딕", 10F, System.Drawing.FontStyle.Bold);
            this.m_labelRun.UnitFontColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.m_labelRun.UnitPositionVertical = false;
            this.m_labelRun.UnitText = "";
            this.m_labelRun.UseBorder = true;
            this.m_labelRun.UseEdgeRadius = false;
            this.m_labelRun.UseImage = false;
            this.m_labelRun.UseSubFont = false;
            this.m_labelRun.UseUnitFont = false;
            this.m_labelRun.Click += new System.EventHandler(this.Click_SpeedContents);
            // 
            // m_labelJogLow
            // 
            this.m_labelJogLow.BackGroundColor = System.Drawing.Color.White;
            this.m_labelJogLow.BorderStroke = 2;
            this.m_labelJogLow.BorderStyle = System.Windows.Forms.ButtonBorderStyle.Solid;
            this.m_labelJogLow.Description = "";
            this.m_labelJogLow.DisabledColor = System.Drawing.Color.LightGray;
            this.m_labelJogLow.Dock = System.Windows.Forms.DockStyle.Fill;
            this.m_labelJogLow.EdgeRadius = 1;
            this.m_labelJogLow.Enabled = false;
            this.m_labelJogLow.ImagePosition = new System.Drawing.Point(0, 0);
            this.m_labelJogLow.ImageSize = new System.Drawing.Point(0, 0);
            this.m_labelJogLow.LoadImage = null;
            this.m_labelJogLow.Location = new System.Drawing.Point(38, 72);
            this.m_labelJogLow.MainFont = new System.Drawing.Font("맑은 고딕", 11F, System.Drawing.FontStyle.Bold);
            this.m_labelJogLow.MainFontColor = System.Drawing.Color.Black;
            this.m_labelJogLow.Margin = new System.Windows.Forms.Padding(0, 3, 0, 3);
            this.m_labelJogLow.Name = "m_labelJogLow";
            this.m_labelJogLow.Size = new System.Drawing.Size(239, 53);
            this.m_labelJogLow.SubFont = new System.Drawing.Font("맑은 고딕", 10F);
            this.m_labelJogLow.SubFontColor = System.Drawing.Color.Black;
            this.m_labelJogLow.SubText = "";
            this.m_labelJogLow.TabIndex = 1;
            this.m_labelJogLow.Text = "JOG LOW";
            this.m_labelJogLow.TextAlignMain = Sys3Controls.EN_TEXTALIGN.BOTTOM_RIGHT;
            this.m_labelJogLow.TextAlignSub = Sys3Controls.EN_TEXTALIGN.MIDDLE_CENTER;
            this.m_labelJogLow.TextAlignUnit = Sys3Controls.EN_TEXTALIGN.MIDDLE_CENTER;
            this.m_labelJogLow.ThemeIndex = 0;
            this.m_labelJogLow.UnitAreaRate = 40;
            this.m_labelJogLow.UnitFont = new System.Drawing.Font("맑은 고딕", 10F, System.Drawing.FontStyle.Bold);
            this.m_labelJogLow.UnitFontColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.m_labelJogLow.UnitPositionVertical = false;
            this.m_labelJogLow.UnitText = "";
            this.m_labelJogLow.UseBorder = true;
            this.m_labelJogLow.UseEdgeRadius = false;
            this.m_labelJogLow.UseImage = false;
            this.m_labelJogLow.UseSubFont = false;
            this.m_labelJogLow.UseUnitFont = false;
            this.m_labelJogLow.Click += new System.EventHandler(this.Click_SpeedContents);
            // 
            // m_labelJogHigh
            // 
            this.m_labelJogHigh.BackGroundColor = System.Drawing.Color.White;
            this.m_labelJogHigh.BorderStroke = 2;
            this.m_labelJogHigh.BorderStyle = System.Windows.Forms.ButtonBorderStyle.Solid;
            this.m_labelJogHigh.Description = "";
            this.m_labelJogHigh.DisabledColor = System.Drawing.Color.LightGray;
            this.m_labelJogHigh.Dock = System.Windows.Forms.DockStyle.Fill;
            this.m_labelJogHigh.EdgeRadius = 1;
            this.m_labelJogHigh.Enabled = false;
            this.m_labelJogHigh.ImagePosition = new System.Drawing.Point(0, 0);
            this.m_labelJogHigh.ImageSize = new System.Drawing.Point(0, 0);
            this.m_labelJogHigh.LoadImage = null;
            this.m_labelJogHigh.Location = new System.Drawing.Point(38, 131);
            this.m_labelJogHigh.MainFont = new System.Drawing.Font("맑은 고딕", 11F, System.Drawing.FontStyle.Bold);
            this.m_labelJogHigh.MainFontColor = System.Drawing.Color.Black;
            this.m_labelJogHigh.Margin = new System.Windows.Forms.Padding(0, 3, 0, 3);
            this.m_labelJogHigh.Name = "m_labelJogHigh";
            this.m_labelJogHigh.Size = new System.Drawing.Size(239, 53);
            this.m_labelJogHigh.SubFont = new System.Drawing.Font("맑은 고딕", 10F);
            this.m_labelJogHigh.SubFontColor = System.Drawing.Color.Black;
            this.m_labelJogHigh.SubText = "";
            this.m_labelJogHigh.TabIndex = 2;
            this.m_labelJogHigh.Text = "JOG HIGH";
            this.m_labelJogHigh.TextAlignMain = Sys3Controls.EN_TEXTALIGN.BOTTOM_RIGHT;
            this.m_labelJogHigh.TextAlignSub = Sys3Controls.EN_TEXTALIGN.MIDDLE_CENTER;
            this.m_labelJogHigh.TextAlignUnit = Sys3Controls.EN_TEXTALIGN.MIDDLE_CENTER;
            this.m_labelJogHigh.ThemeIndex = 0;
            this.m_labelJogHigh.UnitAreaRate = 40;
            this.m_labelJogHigh.UnitFont = new System.Drawing.Font("맑은 고딕", 10F, System.Drawing.FontStyle.Bold);
            this.m_labelJogHigh.UnitFontColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.m_labelJogHigh.UnitPositionVertical = false;
            this.m_labelJogHigh.UnitText = "";
            this.m_labelJogHigh.UseBorder = true;
            this.m_labelJogHigh.UseEdgeRadius = false;
            this.m_labelJogHigh.UseImage = false;
            this.m_labelJogHigh.UseSubFont = false;
            this.m_labelJogHigh.UseUnitFont = false;
            this.m_labelJogHigh.Click += new System.EventHandler(this.Click_SpeedContents);
            // 
            // m_labelManual
            // 
            this.m_labelManual.BackGroundColor = System.Drawing.Color.White;
            this.m_labelManual.BorderStroke = 2;
            this.m_labelManual.BorderStyle = System.Windows.Forms.ButtonBorderStyle.Solid;
            this.m_labelManual.Description = "";
            this.m_labelManual.DisabledColor = System.Drawing.Color.LightGray;
            this.m_labelManual.Dock = System.Windows.Forms.DockStyle.Fill;
            this.m_labelManual.EdgeRadius = 1;
            this.m_labelManual.Enabled = false;
            this.m_labelManual.ImagePosition = new System.Drawing.Point(0, 0);
            this.m_labelManual.ImageSize = new System.Drawing.Point(0, 0);
            this.m_labelManual.LoadImage = null;
            this.m_labelManual.Location = new System.Drawing.Point(38, 190);
            this.m_labelManual.MainFont = new System.Drawing.Font("맑은 고딕", 11F, System.Drawing.FontStyle.Bold);
            this.m_labelManual.MainFontColor = System.Drawing.Color.Black;
            this.m_labelManual.Margin = new System.Windows.Forms.Padding(0, 3, 0, 3);
            this.m_labelManual.Name = "m_labelManual";
            this.m_labelManual.Size = new System.Drawing.Size(239, 53);
            this.m_labelManual.SubFont = new System.Drawing.Font("맑은 고딕", 10F);
            this.m_labelManual.SubFontColor = System.Drawing.Color.Black;
            this.m_labelManual.SubText = "";
            this.m_labelManual.TabIndex = 3;
            this.m_labelManual.Text = "MANUAL";
            this.m_labelManual.TextAlignMain = Sys3Controls.EN_TEXTALIGN.BOTTOM_RIGHT;
            this.m_labelManual.TextAlignSub = Sys3Controls.EN_TEXTALIGN.MIDDLE_CENTER;
            this.m_labelManual.TextAlignUnit = Sys3Controls.EN_TEXTALIGN.MIDDLE_CENTER;
            this.m_labelManual.ThemeIndex = 0;
            this.m_labelManual.UnitAreaRate = 40;
            this.m_labelManual.UnitFont = new System.Drawing.Font("맑은 고딕", 10F, System.Drawing.FontStyle.Bold);
            this.m_labelManual.UnitFontColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.m_labelManual.UnitPositionVertical = false;
            this.m_labelManual.UnitText = "";
            this.m_labelManual.UseBorder = true;
            this.m_labelManual.UseEdgeRadius = false;
            this.m_labelManual.UseImage = false;
            this.m_labelManual.UseSubFont = false;
            this.m_labelManual.UseUnitFont = false;
            this.m_labelManual.Click += new System.EventHandler(this.Click_SpeedContents);
            // 
            // m_labelCustom1
            // 
            this.m_labelCustom1.BackGroundColor = System.Drawing.Color.White;
            this.m_labelCustom1.BorderStroke = 2;
            this.m_labelCustom1.BorderStyle = System.Windows.Forms.ButtonBorderStyle.Solid;
            this.m_labelCustom1.Description = "";
            this.m_labelCustom1.DisabledColor = System.Drawing.Color.LightGray;
            this.m_labelCustom1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.m_labelCustom1.EdgeRadius = 1;
            this.m_labelCustom1.Enabled = false;
            this.m_labelCustom1.ImagePosition = new System.Drawing.Point(0, 0);
            this.m_labelCustom1.ImageSize = new System.Drawing.Point(0, 0);
            this.m_labelCustom1.LoadImage = null;
            this.m_labelCustom1.Location = new System.Drawing.Point(38, 249);
            this.m_labelCustom1.MainFont = new System.Drawing.Font("맑은 고딕", 11F, System.Drawing.FontStyle.Bold);
            this.m_labelCustom1.MainFontColor = System.Drawing.Color.Black;
            this.m_labelCustom1.Margin = new System.Windows.Forms.Padding(0, 3, 0, 3);
            this.m_labelCustom1.Name = "m_labelCustom1";
            this.m_labelCustom1.Size = new System.Drawing.Size(239, 53);
            this.m_labelCustom1.SubFont = new System.Drawing.Font("맑은 고딕", 10F);
            this.m_labelCustom1.SubFontColor = System.Drawing.Color.Black;
            this.m_labelCustom1.SubText = "";
            this.m_labelCustom1.TabIndex = 4;
            this.m_labelCustom1.Text = "CUSTOM 1";
            this.m_labelCustom1.TextAlignMain = Sys3Controls.EN_TEXTALIGN.BOTTOM_RIGHT;
            this.m_labelCustom1.TextAlignSub = Sys3Controls.EN_TEXTALIGN.MIDDLE_CENTER;
            this.m_labelCustom1.TextAlignUnit = Sys3Controls.EN_TEXTALIGN.MIDDLE_CENTER;
            this.m_labelCustom1.ThemeIndex = 0;
            this.m_labelCustom1.UnitAreaRate = 40;
            this.m_labelCustom1.UnitFont = new System.Drawing.Font("맑은 고딕", 10F, System.Drawing.FontStyle.Bold);
            this.m_labelCustom1.UnitFontColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.m_labelCustom1.UnitPositionVertical = false;
            this.m_labelCustom1.UnitText = "";
            this.m_labelCustom1.UseBorder = true;
            this.m_labelCustom1.UseEdgeRadius = false;
            this.m_labelCustom1.UseImage = false;
            this.m_labelCustom1.UseSubFont = false;
            this.m_labelCustom1.UseUnitFont = false;
            this.m_labelCustom1.Click += new System.EventHandler(this.Click_SpeedContents);
            // 
            // m_labelCustom2
            // 
            this.m_labelCustom2.BackGroundColor = System.Drawing.Color.White;
            this.m_labelCustom2.BorderStroke = 2;
            this.m_labelCustom2.BorderStyle = System.Windows.Forms.ButtonBorderStyle.Solid;
            this.m_labelCustom2.Description = "";
            this.m_labelCustom2.DisabledColor = System.Drawing.Color.LightGray;
            this.m_labelCustom2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.m_labelCustom2.EdgeRadius = 1;
            this.m_labelCustom2.Enabled = false;
            this.m_labelCustom2.ImagePosition = new System.Drawing.Point(0, 0);
            this.m_labelCustom2.ImageSize = new System.Drawing.Point(0, 0);
            this.m_labelCustom2.LoadImage = null;
            this.m_labelCustom2.Location = new System.Drawing.Point(38, 308);
            this.m_labelCustom2.MainFont = new System.Drawing.Font("맑은 고딕", 11F, System.Drawing.FontStyle.Bold);
            this.m_labelCustom2.MainFontColor = System.Drawing.Color.Black;
            this.m_labelCustom2.Margin = new System.Windows.Forms.Padding(0, 3, 0, 3);
            this.m_labelCustom2.Name = "m_labelCustom2";
            this.m_labelCustom2.Size = new System.Drawing.Size(239, 55);
            this.m_labelCustom2.SubFont = new System.Drawing.Font("맑은 고딕", 10F);
            this.m_labelCustom2.SubFontColor = System.Drawing.Color.Black;
            this.m_labelCustom2.SubText = "";
            this.m_labelCustom2.TabIndex = 5;
            this.m_labelCustom2.Text = "SHORT DISTANCE";
            this.m_labelCustom2.TextAlignMain = Sys3Controls.EN_TEXTALIGN.BOTTOM_RIGHT;
            this.m_labelCustom2.TextAlignSub = Sys3Controls.EN_TEXTALIGN.MIDDLE_CENTER;
            this.m_labelCustom2.TextAlignUnit = Sys3Controls.EN_TEXTALIGN.MIDDLE_CENTER;
            this.m_labelCustom2.ThemeIndex = 0;
            this.m_labelCustom2.UnitAreaRate = 40;
            this.m_labelCustom2.UnitFont = new System.Drawing.Font("맑은 고딕", 10F, System.Drawing.FontStyle.Bold);
            this.m_labelCustom2.UnitFontColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.m_labelCustom2.UnitPositionVertical = false;
            this.m_labelCustom2.UnitText = "";
            this.m_labelCustom2.UseBorder = true;
            this.m_labelCustom2.UseEdgeRadius = false;
            this.m_labelCustom2.UseImage = false;
            this.m_labelCustom2.UseSubFont = false;
            this.m_labelCustom2.UseUnitFont = false;
            this.m_labelCustom2.Click += new System.EventHandler(this.Click_SpeedContents);
            // 
            // m_groupSelectedContents
            // 
            this.m_groupSelectedContents.BackGroundColor = System.Drawing.Color.WhiteSmoke;
            this.m_groupSelectedContents.Controls.Add(this.tableLayoutPanel2);
            this.m_groupSelectedContents.Dock = System.Windows.Forms.DockStyle.Fill;
            this.m_groupSelectedContents.EdgeBorderStroke = 2;
            this.m_groupSelectedContents.EdgeRadius = 2;
            this.m_groupSelectedContents.LabelFont = new System.Drawing.Font("맑은 고딕", 10F, System.Drawing.FontStyle.Bold);
            this.m_groupSelectedContents.LabelGradientColorFirst = System.Drawing.Color.FromArgb(((int)(((byte)(208)))), ((int)(((byte)(208)))), ((int)(((byte)(208)))));
            this.m_groupSelectedContents.LabelGradientColorSecond = System.Drawing.Color.FromArgb(((int)(((byte)(240)))), ((int)(((byte)(240)))), ((int)(((byte)(240)))));
            this.m_groupSelectedContents.LabelHeight = 30;
            this.m_groupSelectedContents.LabelTextColor = System.Drawing.Color.Black;
            this.m_groupSelectedContents.Location = new System.Drawing.Point(293, 0);
            this.m_groupSelectedContents.Margin = new System.Windows.Forms.Padding(0);
            this.m_groupSelectedContents.Name = "m_groupSelectedContents";
            this.m_groupSelectedContents.Padding = new System.Windows.Forms.Padding(3, 18, 3, 3);
            this.m_groupSelectedContents.Size = new System.Drawing.Size(897, 127);
            this.m_groupSelectedContents.TabIndex = 1372;
            this.m_groupSelectedContents.TabStop = false;
            this.m_groupSelectedContents.Text = "SELECTED CONTENTS";
            this.m_groupSelectedContents.TextAlign = Sys3Controls.EN_TEXTALIGN.MIDDLE_CENTER;
            this.m_groupSelectedContents.ThemeIndex = 0;
            this.m_groupSelectedContents.UseLabelBorder = true;
            this.m_groupSelectedContents.UseTitle = true;
            // 
            // tableLayoutPanel2
            // 
            this.tableLayoutPanel2.ColumnCount = 4;
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 18.08511F));
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 31.91489F));
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 18.08511F));
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 31.91489F));
            this.tableLayoutPanel2.Controls.Add(this.m_lblName, 0, 0);
            this.tableLayoutPanel2.Controls.Add(this.m_labelName, 1, 0);
            this.tableLayoutPanel2.Controls.Add(this.m_labelShortDistanceAuto, 3, 1);
            this.tableLayoutPanel2.Controls.Add(this.m_lblShortDistanceAuto, 2, 1);
            this.tableLayoutPanel2.Controls.Add(this.m_labelShortDistance, 3, 0);
            this.tableLayoutPanel2.Controls.Add(this.m_lblShortDistance, 2, 0);
            this.tableLayoutPanel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel2.Location = new System.Drawing.Point(3, 32);
            this.tableLayoutPanel2.Margin = new System.Windows.Forms.Padding(0);
            this.tableLayoutPanel2.Name = "tableLayoutPanel2";
            this.tableLayoutPanel2.Padding = new System.Windows.Forms.Padding(5, 0, 5, 0);
            this.tableLayoutPanel2.RowCount = 2;
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel2.Size = new System.Drawing.Size(891, 92);
            this.tableLayoutPanel2.TabIndex = 0;
            // 
            // m_lblName
            // 
            this.m_lblName.BackGroundColor = System.Drawing.Color.FromArgb(((int)(((byte)(200)))), ((int)(((byte)(200)))), ((int)(((byte)(220)))));
            this.m_lblName.BorderStroke = 2;
            this.m_lblName.BorderStyle = System.Windows.Forms.ButtonBorderStyle.Solid;
            this.m_lblName.Description = "";
            this.m_lblName.DisabledColor = System.Drawing.Color.DarkGray;
            this.m_lblName.Dock = System.Windows.Forms.DockStyle.Fill;
            this.m_lblName.EdgeRadius = 1;
            this.m_lblName.ImagePosition = new System.Drawing.Point(0, 0);
            this.m_lblName.ImageSize = new System.Drawing.Point(0, 0);
            this.m_lblName.LoadImage = null;
            this.m_lblName.Location = new System.Drawing.Point(5, 3);
            this.m_lblName.MainFont = new System.Drawing.Font("맑은 고딕", 9F, System.Drawing.FontStyle.Bold);
            this.m_lblName.MainFontColor = System.Drawing.Color.Black;
            this.m_lblName.Margin = new System.Windows.Forms.Padding(0, 3, 0, 3);
            this.m_lblName.Name = "m_lblName";
            this.m_lblName.Size = new System.Drawing.Size(159, 40);
            this.m_lblName.SubFont = new System.Drawing.Font("맑은 고딕", 10F);
            this.m_lblName.SubFontColor = System.Drawing.Color.Black;
            this.m_lblName.SubText = "";
            this.m_lblName.TabIndex = 1386;
            this.m_lblName.Text = "NAME";
            this.m_lblName.TextAlignMain = Sys3Controls.EN_TEXTALIGN.TOP_LEFT;
            this.m_lblName.TextAlignSub = Sys3Controls.EN_TEXTALIGN.MIDDLE_CENTER;
            this.m_lblName.TextAlignUnit = Sys3Controls.EN_TEXTALIGN.MIDDLE_CENTER;
            this.m_lblName.ThemeIndex = 0;
            this.m_lblName.UnitAreaRate = 40;
            this.m_lblName.UnitFont = new System.Drawing.Font("맑은 고딕", 10F, System.Drawing.FontStyle.Bold);
            this.m_lblName.UnitFontColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.m_lblName.UnitPositionVertical = false;
            this.m_lblName.UnitText = "";
            this.m_lblName.UseBorder = true;
            this.m_lblName.UseEdgeRadius = false;
            this.m_lblName.UseImage = false;
            this.m_lblName.UseSubFont = false;
            this.m_lblName.UseUnitFont = false;
            // 
            // m_labelName
            // 
            this.m_labelName.BackGroundColor = System.Drawing.Color.White;
            this.m_labelName.BorderStroke = 2;
            this.m_labelName.BorderStyle = System.Windows.Forms.ButtonBorderStyle.Solid;
            this.m_labelName.Description = "";
            this.m_labelName.DisabledColor = System.Drawing.Color.LightGray;
            this.m_labelName.Dock = System.Windows.Forms.DockStyle.Fill;
            this.m_labelName.EdgeRadius = 1;
            this.m_labelName.Enabled = false;
            this.m_labelName.ImagePosition = new System.Drawing.Point(0, 0);
            this.m_labelName.ImageSize = new System.Drawing.Point(0, 0);
            this.m_labelName.LoadImage = null;
            this.m_labelName.Location = new System.Drawing.Point(166, 3);
            this.m_labelName.MainFont = new System.Drawing.Font("맑은 고딕", 11F, System.Drawing.FontStyle.Bold);
            this.m_labelName.MainFontColor = System.Drawing.Color.Black;
            this.m_labelName.Margin = new System.Windows.Forms.Padding(2, 3, 5, 3);
            this.m_labelName.Name = "m_labelName";
            this.m_labelName.Size = new System.Drawing.Size(274, 40);
            this.m_labelName.SubFont = new System.Drawing.Font("맑은 고딕", 10F);
            this.m_labelName.SubFontColor = System.Drawing.Color.Black;
            this.m_labelName.SubText = "";
            this.m_labelName.TabIndex = 1385;
            this.m_labelName.TextAlignMain = Sys3Controls.EN_TEXTALIGN.MIDDLE_CENTER;
            this.m_labelName.TextAlignSub = Sys3Controls.EN_TEXTALIGN.MIDDLE_CENTER;
            this.m_labelName.TextAlignUnit = Sys3Controls.EN_TEXTALIGN.MIDDLE_CENTER;
            this.m_labelName.ThemeIndex = 0;
            this.m_labelName.UnitAreaRate = 40;
            this.m_labelName.UnitFont = new System.Drawing.Font("맑은 고딕", 10F, System.Drawing.FontStyle.Bold);
            this.m_labelName.UnitFontColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.m_labelName.UnitPositionVertical = false;
            this.m_labelName.UnitText = "";
            this.m_labelName.UseBorder = true;
            this.m_labelName.UseEdgeRadius = false;
            this.m_labelName.UseImage = false;
            this.m_labelName.UseSubFont = false;
            this.m_labelName.UseUnitFont = false;
            // 
            // m_labelShortDistanceAuto
            // 
            this.m_labelShortDistanceAuto.BackGroundColor = System.Drawing.Color.White;
            this.m_labelShortDistanceAuto.BorderStroke = 2;
            this.m_labelShortDistanceAuto.BorderStyle = System.Windows.Forms.ButtonBorderStyle.Solid;
            this.m_labelShortDistanceAuto.Description = "";
            this.m_labelShortDistanceAuto.DisabledColor = System.Drawing.Color.LightGray;
            this.m_labelShortDistanceAuto.Dock = System.Windows.Forms.DockStyle.Fill;
            this.m_labelShortDistanceAuto.EdgeRadius = 1;
            this.m_labelShortDistanceAuto.Enabled = false;
            this.m_labelShortDistanceAuto.ImagePosition = new System.Drawing.Point(0, 0);
            this.m_labelShortDistanceAuto.ImageSize = new System.Drawing.Point(0, 0);
            this.m_labelShortDistanceAuto.LoadImage = null;
            this.m_labelShortDistanceAuto.Location = new System.Drawing.Point(606, 49);
            this.m_labelShortDistanceAuto.MainFont = new System.Drawing.Font("맑은 고딕", 11F, System.Drawing.FontStyle.Bold);
            this.m_labelShortDistanceAuto.MainFontColor = System.Drawing.Color.Black;
            this.m_labelShortDistanceAuto.Margin = new System.Windows.Forms.Padding(2, 3, 0, 3);
            this.m_labelShortDistanceAuto.Name = "m_labelShortDistanceAuto";
            this.m_labelShortDistanceAuto.Size = new System.Drawing.Size(280, 40);
            this.m_labelShortDistanceAuto.SubFont = new System.Drawing.Font("맑은 고딕", 10F);
            this.m_labelShortDistanceAuto.SubFontColor = System.Drawing.Color.Black;
            this.m_labelShortDistanceAuto.SubText = "";
            this.m_labelShortDistanceAuto.TabIndex = 9;
            this.m_labelShortDistanceAuto.Text = "--";
            this.m_labelShortDistanceAuto.TextAlignMain = Sys3Controls.EN_TEXTALIGN.BOTTOM_RIGHT;
            this.m_labelShortDistanceAuto.TextAlignSub = Sys3Controls.EN_TEXTALIGN.MIDDLE_CENTER;
            this.m_labelShortDistanceAuto.TextAlignUnit = Sys3Controls.EN_TEXTALIGN.MIDDLE_CENTER;
            this.m_labelShortDistanceAuto.ThemeIndex = 0;
            this.m_labelShortDistanceAuto.UnitAreaRate = 40;
            this.m_labelShortDistanceAuto.UnitFont = new System.Drawing.Font("맑은 고딕", 10F, System.Drawing.FontStyle.Bold);
            this.m_labelShortDistanceAuto.UnitFontColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.m_labelShortDistanceAuto.UnitPositionVertical = false;
            this.m_labelShortDistanceAuto.UnitText = "";
            this.m_labelShortDistanceAuto.UseBorder = true;
            this.m_labelShortDistanceAuto.UseEdgeRadius = false;
            this.m_labelShortDistanceAuto.UseImage = false;
            this.m_labelShortDistanceAuto.UseSubFont = false;
            this.m_labelShortDistanceAuto.UseUnitFont = false;
            this.m_labelShortDistanceAuto.Click += new System.EventHandler(this.Click_Parameters);
            // 
            // m_lblShortDistanceAuto
            // 
            this.m_lblShortDistanceAuto.BackGroundColor = System.Drawing.Color.FromArgb(((int)(((byte)(200)))), ((int)(((byte)(200)))), ((int)(((byte)(220)))));
            this.m_lblShortDistanceAuto.BorderStroke = 2;
            this.m_lblShortDistanceAuto.BorderStyle = System.Windows.Forms.ButtonBorderStyle.Solid;
            this.m_lblShortDistanceAuto.Description = "";
            this.m_lblShortDistanceAuto.DisabledColor = System.Drawing.Color.DarkGray;
            this.m_lblShortDistanceAuto.Dock = System.Windows.Forms.DockStyle.Fill;
            this.m_lblShortDistanceAuto.EdgeRadius = 1;
            this.m_lblShortDistanceAuto.ImagePosition = new System.Drawing.Point(0, 0);
            this.m_lblShortDistanceAuto.ImageSize = new System.Drawing.Point(0, 0);
            this.m_lblShortDistanceAuto.LoadImage = null;
            this.m_lblShortDistanceAuto.Location = new System.Drawing.Point(445, 49);
            this.m_lblShortDistanceAuto.MainFont = new System.Drawing.Font("맑은 고딕", 9F, System.Drawing.FontStyle.Bold);
            this.m_lblShortDistanceAuto.MainFontColor = System.Drawing.Color.Black;
            this.m_lblShortDistanceAuto.Margin = new System.Windows.Forms.Padding(0, 3, 0, 3);
            this.m_lblShortDistanceAuto.Name = "m_lblShortDistanceAuto";
            this.m_lblShortDistanceAuto.Size = new System.Drawing.Size(159, 40);
            this.m_lblShortDistanceAuto.SubFont = new System.Drawing.Font("맑은 고딕", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.m_lblShortDistanceAuto.SubFontColor = System.Drawing.Color.Purple;
            this.m_lblShortDistanceAuto.SubText = "AUTO";
            this.m_lblShortDistanceAuto.TabIndex = 1390;
            this.m_lblShortDistanceAuto.Text = "SHORT DISTANCE";
            this.m_lblShortDistanceAuto.TextAlignMain = Sys3Controls.EN_TEXTALIGN.TOP_LEFT;
            this.m_lblShortDistanceAuto.TextAlignSub = Sys3Controls.EN_TEXTALIGN.BOTTOM_RIGHT;
            this.m_lblShortDistanceAuto.TextAlignUnit = Sys3Controls.EN_TEXTALIGN.MIDDLE_CENTER;
            this.m_lblShortDistanceAuto.ThemeIndex = 0;
            this.m_lblShortDistanceAuto.UnitAreaRate = 40;
            this.m_lblShortDistanceAuto.UnitFont = new System.Drawing.Font("맑은 고딕", 10F, System.Drawing.FontStyle.Bold);
            this.m_lblShortDistanceAuto.UnitFontColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.m_lblShortDistanceAuto.UnitPositionVertical = false;
            this.m_lblShortDistanceAuto.UnitText = "";
            this.m_lblShortDistanceAuto.UseBorder = true;
            this.m_lblShortDistanceAuto.UseEdgeRadius = false;
            this.m_lblShortDistanceAuto.UseImage = false;
            this.m_lblShortDistanceAuto.UseSubFont = true;
            this.m_lblShortDistanceAuto.UseUnitFont = false;
            // 
            // m_labelShortDistance
            // 
            this.m_labelShortDistance.BackGroundColor = System.Drawing.Color.White;
            this.m_labelShortDistance.BorderStroke = 2;
            this.m_labelShortDistance.BorderStyle = System.Windows.Forms.ButtonBorderStyle.Solid;
            this.m_labelShortDistance.Description = "";
            this.m_labelShortDistance.DisabledColor = System.Drawing.Color.LightGray;
            this.m_labelShortDistance.Dock = System.Windows.Forms.DockStyle.Fill;
            this.m_labelShortDistance.EdgeRadius = 1;
            this.m_labelShortDistance.Enabled = false;
            this.m_labelShortDistance.ImagePosition = new System.Drawing.Point(0, 0);
            this.m_labelShortDistance.ImageSize = new System.Drawing.Point(0, 0);
            this.m_labelShortDistance.LoadImage = null;
            this.m_labelShortDistance.Location = new System.Drawing.Point(606, 3);
            this.m_labelShortDistance.MainFont = new System.Drawing.Font("맑은 고딕", 11F, System.Drawing.FontStyle.Bold);
            this.m_labelShortDistance.MainFontColor = System.Drawing.Color.Black;
            this.m_labelShortDistance.Margin = new System.Windows.Forms.Padding(2, 3, 0, 3);
            this.m_labelShortDistance.Name = "m_labelShortDistance";
            this.m_labelShortDistance.Size = new System.Drawing.Size(280, 40);
            this.m_labelShortDistance.SubFont = new System.Drawing.Font("맑은 고딕", 10F);
            this.m_labelShortDistance.SubFontColor = System.Drawing.Color.Black;
            this.m_labelShortDistance.SubText = "";
            this.m_labelShortDistance.TabIndex = 8;
            this.m_labelShortDistance.Text = "--";
            this.m_labelShortDistance.TextAlignMain = Sys3Controls.EN_TEXTALIGN.BOTTOM_RIGHT;
            this.m_labelShortDistance.TextAlignSub = Sys3Controls.EN_TEXTALIGN.MIDDLE_CENTER;
            this.m_labelShortDistance.TextAlignUnit = Sys3Controls.EN_TEXTALIGN.MIDDLE_CENTER;
            this.m_labelShortDistance.ThemeIndex = 0;
            this.m_labelShortDistance.UnitAreaRate = 40;
            this.m_labelShortDistance.UnitFont = new System.Drawing.Font("맑은 고딕", 10F, System.Drawing.FontStyle.Bold);
            this.m_labelShortDistance.UnitFontColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.m_labelShortDistance.UnitPositionVertical = false;
            this.m_labelShortDistance.UnitText = "";
            this.m_labelShortDistance.UseBorder = true;
            this.m_labelShortDistance.UseEdgeRadius = false;
            this.m_labelShortDistance.UseImage = false;
            this.m_labelShortDistance.UseSubFont = false;
            this.m_labelShortDistance.UseUnitFont = false;
            this.m_labelShortDistance.Click += new System.EventHandler(this.Click_Parameters);
            // 
            // m_lblShortDistance
            // 
            this.m_lblShortDistance.BackGroundColor = System.Drawing.Color.FromArgb(((int)(((byte)(200)))), ((int)(((byte)(200)))), ((int)(((byte)(220)))));
            this.m_lblShortDistance.BorderStroke = 2;
            this.m_lblShortDistance.BorderStyle = System.Windows.Forms.ButtonBorderStyle.Solid;
            this.m_lblShortDistance.Description = "";
            this.m_lblShortDistance.DisabledColor = System.Drawing.Color.DarkGray;
            this.m_lblShortDistance.Dock = System.Windows.Forms.DockStyle.Fill;
            this.m_lblShortDistance.EdgeRadius = 1;
            this.m_lblShortDistance.ImagePosition = new System.Drawing.Point(0, 0);
            this.m_lblShortDistance.ImageSize = new System.Drawing.Point(0, 0);
            this.m_lblShortDistance.LoadImage = null;
            this.m_lblShortDistance.Location = new System.Drawing.Point(445, 3);
            this.m_lblShortDistance.MainFont = new System.Drawing.Font("맑은 고딕", 9F, System.Drawing.FontStyle.Bold);
            this.m_lblShortDistance.MainFontColor = System.Drawing.Color.Black;
            this.m_lblShortDistance.Margin = new System.Windows.Forms.Padding(0, 3, 0, 3);
            this.m_lblShortDistance.Name = "m_lblShortDistance";
            this.m_lblShortDistance.Size = new System.Drawing.Size(159, 40);
            this.m_lblShortDistance.SubFont = new System.Drawing.Font("맑은 고딕", 10F);
            this.m_lblShortDistance.SubFontColor = System.Drawing.Color.Black;
            this.m_lblShortDistance.SubText = "";
            this.m_lblShortDistance.TabIndex = 1388;
            this.m_lblShortDistance.Text = "SHORT DISTANCE";
            this.m_lblShortDistance.TextAlignMain = Sys3Controls.EN_TEXTALIGN.TOP_LEFT;
            this.m_lblShortDistance.TextAlignSub = Sys3Controls.EN_TEXTALIGN.MIDDLE_CENTER;
            this.m_lblShortDistance.TextAlignUnit = Sys3Controls.EN_TEXTALIGN.MIDDLE_CENTER;
            this.m_lblShortDistance.ThemeIndex = 0;
            this.m_lblShortDistance.UnitAreaRate = 40;
            this.m_lblShortDistance.UnitFont = new System.Drawing.Font("맑은 고딕", 10F, System.Drawing.FontStyle.Bold);
            this.m_lblShortDistance.UnitFontColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.m_lblShortDistance.UnitPositionVertical = false;
            this.m_lblShortDistance.UnitText = "";
            this.m_lblShortDistance.UseBorder = true;
            this.m_lblShortDistance.UseEdgeRadius = false;
            this.m_lblShortDistance.UseImage = false;
            this.m_lblShortDistance.UseSubFont = false;
            this.m_lblShortDistance.UseUnitFont = false;
            // 
            // m_groupSpeedParameters
            // 
            this.m_groupSpeedParameters.BackGroundColor = System.Drawing.Color.WhiteSmoke;
            this.m_groupSpeedParameters.Controls.Add(this.tableLayoutPanel3);
            this.m_groupSpeedParameters.Dock = System.Windows.Forms.DockStyle.Fill;
            this.m_groupSpeedParameters.EdgeBorderStroke = 2;
            this.m_groupSpeedParameters.EdgeRadius = 2;
            this.m_groupSpeedParameters.LabelFont = new System.Drawing.Font("맑은 고딕", 10F, System.Drawing.FontStyle.Bold);
            this.m_groupSpeedParameters.LabelGradientColorFirst = System.Drawing.Color.FromArgb(((int)(((byte)(208)))), ((int)(((byte)(208)))), ((int)(((byte)(208)))));
            this.m_groupSpeedParameters.LabelGradientColorSecond = System.Drawing.Color.FromArgb(((int)(((byte)(240)))), ((int)(((byte)(240)))), ((int)(((byte)(240)))));
            this.m_groupSpeedParameters.LabelHeight = 30;
            this.m_groupSpeedParameters.LabelTextColor = System.Drawing.Color.Black;
            this.m_groupSpeedParameters.Location = new System.Drawing.Point(293, 127);
            this.m_groupSpeedParameters.Margin = new System.Windows.Forms.Padding(0);
            this.m_groupSpeedParameters.Name = "m_groupSpeedParameters";
            this.m_groupSpeedParameters.Padding = new System.Windows.Forms.Padding(3, 18, 3, 3);
            this.m_groupSpeedParameters.Size = new System.Drawing.Size(897, 284);
            this.m_groupSpeedParameters.TabIndex = 1373;
            this.m_groupSpeedParameters.TabStop = false;
            this.m_groupSpeedParameters.Text = "SPEED PARAMETERS";
            this.m_groupSpeedParameters.TextAlign = Sys3Controls.EN_TEXTALIGN.MIDDLE_CENTER;
            this.m_groupSpeedParameters.ThemeIndex = 0;
            this.m_groupSpeedParameters.UseLabelBorder = true;
            this.m_groupSpeedParameters.UseTitle = true;
            // 
            // tableLayoutPanel3
            // 
            this.tableLayoutPanel3.ColumnCount = 4;
            this.tableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 18.08511F));
            this.tableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 31.91489F));
            this.tableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 18.0851F));
            this.tableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 31.91489F));
            this.tableLayoutPanel3.Controls.Add(this.m_lblSpeedPattern, 0, 0);
            this.tableLayoutPanel3.Controls.Add(this.m_btnCopy, 2, 0);
            this.tableLayoutPanel3.Controls.Add(this.m_labelSpeedPattern, 1, 0);
            this.tableLayoutPanel3.Controls.Add(this.m_labelJerkDecel, 3, 5);
            this.tableLayoutPanel3.Controls.Add(this.m_lblJerkDecel, 2, 5);
            this.tableLayoutPanel3.Controls.Add(this.m_labelDecelTime, 3, 4);
            this.tableLayoutPanel3.Controls.Add(this.m_labelDecel, 3, 3);
            this.tableLayoutPanel3.Controls.Add(this.m_lblDecelTime, 2, 4);
            this.tableLayoutPanel3.Controls.Add(this.m_lblDecel, 2, 3);
            this.tableLayoutPanel3.Controls.Add(this.m_labelMaxVelocity, 3, 2);
            this.tableLayoutPanel3.Controls.Add(this.m_lblTimeout, 0, 1);
            this.tableLayoutPanel3.Controls.Add(this.m_labelTimeout, 1, 1);
            this.tableLayoutPanel3.Controls.Add(this.m_lblVelocity, 0, 2);
            this.tableLayoutPanel3.Controls.Add(this.m_labelVelocity, 1, 2);
            this.tableLayoutPanel3.Controls.Add(this.m_lblJerkAccel, 0, 5);
            this.tableLayoutPanel3.Controls.Add(this.m_lblAccel, 0, 3);
            this.tableLayoutPanel3.Controls.Add(this.m_lblAccelTime, 0, 4);
            this.tableLayoutPanel3.Controls.Add(this.m_labelAccel, 1, 3);
            this.tableLayoutPanel3.Controls.Add(this.m_labelJerkAccel, 1, 5);
            this.tableLayoutPanel3.Controls.Add(this.m_labelAccelTime, 1, 4);
            this.tableLayoutPanel3.Controls.Add(this.m_lblMaxVelocity, 2, 2);
            this.tableLayoutPanel3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel3.Location = new System.Drawing.Point(3, 32);
            this.tableLayoutPanel3.Margin = new System.Windows.Forms.Padding(0);
            this.tableLayoutPanel3.Name = "tableLayoutPanel3";
            this.tableLayoutPanel3.Padding = new System.Windows.Forms.Padding(5, 0, 5, 0);
            this.tableLayoutPanel3.RowCount = 6;
            this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 16.66667F));
            this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 16.66667F));
            this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 16.66667F));
            this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 16.66667F));
            this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 16.66667F));
            this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 16.66667F));
            this.tableLayoutPanel3.Size = new System.Drawing.Size(891, 249);
            this.tableLayoutPanel3.TabIndex = 1;
            // 
            // m_lblSpeedPattern
            // 
            this.m_lblSpeedPattern.BackGroundColor = System.Drawing.Color.FromArgb(((int)(((byte)(200)))), ((int)(((byte)(200)))), ((int)(((byte)(220)))));
            this.m_lblSpeedPattern.BorderStroke = 2;
            this.m_lblSpeedPattern.BorderStyle = System.Windows.Forms.ButtonBorderStyle.Solid;
            this.m_lblSpeedPattern.Description = "";
            this.m_lblSpeedPattern.DisabledColor = System.Drawing.Color.DarkGray;
            this.m_lblSpeedPattern.Dock = System.Windows.Forms.DockStyle.Fill;
            this.m_lblSpeedPattern.EdgeRadius = 1;
            this.m_lblSpeedPattern.ImagePosition = new System.Drawing.Point(0, 0);
            this.m_lblSpeedPattern.ImageSize = new System.Drawing.Point(0, 0);
            this.m_lblSpeedPattern.LoadImage = null;
            this.m_lblSpeedPattern.Location = new System.Drawing.Point(5, 3);
            this.m_lblSpeedPattern.MainFont = new System.Drawing.Font("맑은 고딕", 9F, System.Drawing.FontStyle.Bold);
            this.m_lblSpeedPattern.MainFontColor = System.Drawing.Color.Black;
            this.m_lblSpeedPattern.Margin = new System.Windows.Forms.Padding(0, 3, 0, 3);
            this.m_lblSpeedPattern.Name = "m_lblSpeedPattern";
            this.m_lblSpeedPattern.Size = new System.Drawing.Size(159, 35);
            this.m_lblSpeedPattern.SubFont = new System.Drawing.Font("맑은 고딕", 10F);
            this.m_lblSpeedPattern.SubFontColor = System.Drawing.Color.Black;
            this.m_lblSpeedPattern.SubText = "";
            this.m_lblSpeedPattern.TabIndex = 1386;
            this.m_lblSpeedPattern.Text = "SPEED PATTERN";
            this.m_lblSpeedPattern.TextAlignMain = Sys3Controls.EN_TEXTALIGN.TOP_LEFT;
            this.m_lblSpeedPattern.TextAlignSub = Sys3Controls.EN_TEXTALIGN.MIDDLE_CENTER;
            this.m_lblSpeedPattern.TextAlignUnit = Sys3Controls.EN_TEXTALIGN.MIDDLE_CENTER;
            this.m_lblSpeedPattern.ThemeIndex = 0;
            this.m_lblSpeedPattern.UnitAreaRate = 40;
            this.m_lblSpeedPattern.UnitFont = new System.Drawing.Font("맑은 고딕", 10F, System.Drawing.FontStyle.Bold);
            this.m_lblSpeedPattern.UnitFontColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.m_lblSpeedPattern.UnitPositionVertical = false;
            this.m_lblSpeedPattern.UnitText = "";
            this.m_lblSpeedPattern.UseBorder = true;
            this.m_lblSpeedPattern.UseEdgeRadius = false;
            this.m_lblSpeedPattern.UseImage = false;
            this.m_lblSpeedPattern.UseSubFont = false;
            this.m_lblSpeedPattern.UseUnitFont = false;
            // 
            // m_btnCopy
            // 
            this.m_btnCopy.BorderWidth = 3;
            this.m_btnCopy.ButtonClicked = false;
            this.m_btnCopy.ClickedEmphasizeTextColor = System.Drawing.Color.White;
            this.tableLayoutPanel3.SetColumnSpan(this.m_btnCopy, 2);
            this.m_btnCopy.CustomClickedGradientFirstColor = System.Drawing.Color.White;
            this.m_btnCopy.CustomClickedGradientSecondColor = System.Drawing.Color.White;
            this.m_btnCopy.Description = "";
            this.m_btnCopy.DisabledColor = System.Drawing.Color.DarkGray;
            this.m_btnCopy.Dock = System.Windows.Forms.DockStyle.Fill;
            this.m_btnCopy.EdgeRadius = 5;
            this.m_btnCopy.Enabled = false;
            this.m_btnCopy.GradientAngle = 80F;
            this.m_btnCopy.GradientFirstColor = System.Drawing.Color.White;
            this.m_btnCopy.GradientSecondColor = System.Drawing.Color.FromArgb(((int)(((byte)(170)))), ((int)(((byte)(176)))), ((int)(((byte)(183)))));
            this.m_btnCopy.HoverEmphasizeCustomColor = System.Drawing.Color.White;
            this.m_btnCopy.ImagePosition = new System.Drawing.Point(13, 13);
            this.m_btnCopy.ImageSize = new System.Drawing.Point(30, 30);
            this.m_btnCopy.LoadImage = global::FrameOfSystem3.Properties.Resources.COPY;
            this.m_btnCopy.Location = new System.Drawing.Point(485, 10);
            this.m_btnCopy.MainFont = new System.Drawing.Font("맑은 고딕", 15F, System.Drawing.FontStyle.Bold);
            this.m_btnCopy.MainFontColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(36)))), ((int)(((byte)(0)))));
            this.m_btnCopy.Margin = new System.Windows.Forms.Padding(40, 10, 40, 10);
            this.m_btnCopy.Name = "m_btnCopy";
            this.tableLayoutPanel3.SetRowSpan(this.m_btnCopy, 2);
            this.m_btnCopy.Size = new System.Drawing.Size(361, 62);
            this.m_btnCopy.SubFont = new System.Drawing.Font("맑은 고딕", 8F, System.Drawing.FontStyle.Bold);
            this.m_btnCopy.SubFontColor = System.Drawing.Color.DarkBlue;
            this.m_btnCopy.SubText = "STATUS";
            this.m_btnCopy.TabIndex = 1392;
            this.m_btnCopy.Text = "PARAMETER COPY";
            this.m_btnCopy.TextAlignMain = Sys3Controls.EN_TEXTALIGN.MIDDLE_RIGHT;
            this.m_btnCopy.TextAlignSub = Sys3Controls.EN_TEXTALIGN.TOP_LEFT;
            this.m_btnCopy.ThemeIndex = 0;
            this.m_btnCopy.UseBorder = true;
            this.m_btnCopy.UseClickedEmphasizeTextColor = false;
            this.m_btnCopy.UseCustomizeClickedColor = false;
            this.m_btnCopy.UseEdge = true;
            this.m_btnCopy.UseHoverEmphasizeCustomColor = false;
            this.m_btnCopy.UseImage = true;
            this.m_btnCopy.UserHoverEmpahsize = false;
            this.m_btnCopy.UseSubFont = false;
            this.m_btnCopy.Click += new System.EventHandler(this.Click_CopySpeedParameter);
            // 
            // m_labelSpeedPattern
            // 
            this.m_labelSpeedPattern.BackGroundColor = System.Drawing.Color.White;
            this.m_labelSpeedPattern.BorderStroke = 2;
            this.m_labelSpeedPattern.BorderStyle = System.Windows.Forms.ButtonBorderStyle.Solid;
            this.m_labelSpeedPattern.Description = "";
            this.m_labelSpeedPattern.DisabledColor = System.Drawing.Color.LightGray;
            this.m_labelSpeedPattern.Dock = System.Windows.Forms.DockStyle.Fill;
            this.m_labelSpeedPattern.EdgeRadius = 1;
            this.m_labelSpeedPattern.Enabled = false;
            this.m_labelSpeedPattern.ImagePosition = new System.Drawing.Point(0, 0);
            this.m_labelSpeedPattern.ImageSize = new System.Drawing.Point(0, 0);
            this.m_labelSpeedPattern.LoadImage = null;
            this.m_labelSpeedPattern.Location = new System.Drawing.Point(166, 3);
            this.m_labelSpeedPattern.MainFont = new System.Drawing.Font("맑은 고딕", 11F, System.Drawing.FontStyle.Bold);
            this.m_labelSpeedPattern.MainFontColor = System.Drawing.Color.Black;
            this.m_labelSpeedPattern.Margin = new System.Windows.Forms.Padding(2, 3, 5, 3);
            this.m_labelSpeedPattern.Name = "m_labelSpeedPattern";
            this.m_labelSpeedPattern.Size = new System.Drawing.Size(274, 35);
            this.m_labelSpeedPattern.SubFont = new System.Drawing.Font("맑은 고딕", 10F);
            this.m_labelSpeedPattern.SubFontColor = System.Drawing.Color.Black;
            this.m_labelSpeedPattern.SubText = "";
            this.m_labelSpeedPattern.TabIndex = 0;
            this.m_labelSpeedPattern.Text = "--";
            this.m_labelSpeedPattern.TextAlignMain = Sys3Controls.EN_TEXTALIGN.BOTTOM_RIGHT;
            this.m_labelSpeedPattern.TextAlignSub = Sys3Controls.EN_TEXTALIGN.MIDDLE_CENTER;
            this.m_labelSpeedPattern.TextAlignUnit = Sys3Controls.EN_TEXTALIGN.MIDDLE_CENTER;
            this.m_labelSpeedPattern.ThemeIndex = 0;
            this.m_labelSpeedPattern.UnitAreaRate = 40;
            this.m_labelSpeedPattern.UnitFont = new System.Drawing.Font("맑은 고딕", 10F, System.Drawing.FontStyle.Bold);
            this.m_labelSpeedPattern.UnitFontColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.m_labelSpeedPattern.UnitPositionVertical = false;
            this.m_labelSpeedPattern.UnitText = "";
            this.m_labelSpeedPattern.UseBorder = true;
            this.m_labelSpeedPattern.UseEdgeRadius = false;
            this.m_labelSpeedPattern.UseImage = false;
            this.m_labelSpeedPattern.UseSubFont = false;
            this.m_labelSpeedPattern.UseUnitFont = false;
            this.m_labelSpeedPattern.Click += new System.EventHandler(this.Click_Parameters);
            // 
            // m_labelJerkDecel
            // 
            this.m_labelJerkDecel.BackGroundColor = System.Drawing.Color.White;
            this.m_labelJerkDecel.BorderStroke = 2;
            this.m_labelJerkDecel.BorderStyle = System.Windows.Forms.ButtonBorderStyle.Solid;
            this.m_labelJerkDecel.Description = "";
            this.m_labelJerkDecel.DisabledColor = System.Drawing.Color.LightGray;
            this.m_labelJerkDecel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.m_labelJerkDecel.EdgeRadius = 1;
            this.m_labelJerkDecel.Enabled = false;
            this.m_labelJerkDecel.ImagePosition = new System.Drawing.Point(0, 0);
            this.m_labelJerkDecel.ImageSize = new System.Drawing.Point(0, 0);
            this.m_labelJerkDecel.LoadImage = null;
            this.m_labelJerkDecel.Location = new System.Drawing.Point(606, 208);
            this.m_labelJerkDecel.MainFont = new System.Drawing.Font("맑은 고딕", 11F, System.Drawing.FontStyle.Bold);
            this.m_labelJerkDecel.MainFontColor = System.Drawing.Color.Black;
            this.m_labelJerkDecel.Margin = new System.Windows.Forms.Padding(2, 3, 0, 3);
            this.m_labelJerkDecel.Name = "m_labelJerkDecel";
            this.m_labelJerkDecel.Size = new System.Drawing.Size(280, 38);
            this.m_labelJerkDecel.SubFont = new System.Drawing.Font("맑은 고딕", 10F);
            this.m_labelJerkDecel.SubFontColor = System.Drawing.Color.Black;
            this.m_labelJerkDecel.SubText = "";
            this.m_labelJerkDecel.TabIndex = 7;
            this.m_labelJerkDecel.Text = "--";
            this.m_labelJerkDecel.TextAlignMain = Sys3Controls.EN_TEXTALIGN.BOTTOM_RIGHT;
            this.m_labelJerkDecel.TextAlignSub = Sys3Controls.EN_TEXTALIGN.MIDDLE_CENTER;
            this.m_labelJerkDecel.TextAlignUnit = Sys3Controls.EN_TEXTALIGN.BOTTOM_RIGHT;
            this.m_labelJerkDecel.ThemeIndex = 0;
            this.m_labelJerkDecel.UnitAreaRate = 40;
            this.m_labelJerkDecel.UnitFont = new System.Drawing.Font("맑은 고딕", 10F, System.Drawing.FontStyle.Bold);
            this.m_labelJerkDecel.UnitFontColor = System.Drawing.Color.Black;
            this.m_labelJerkDecel.UnitPositionVertical = false;
            this.m_labelJerkDecel.UnitText = "mm/s³";
            this.m_labelJerkDecel.UseBorder = true;
            this.m_labelJerkDecel.UseEdgeRadius = false;
            this.m_labelJerkDecel.UseImage = false;
            this.m_labelJerkDecel.UseSubFont = false;
            this.m_labelJerkDecel.UseUnitFont = true;
            this.m_labelJerkDecel.Click += new System.EventHandler(this.Click_Parameters);
            // 
            // m_lblJerkDecel
            // 
            this.m_lblJerkDecel.BackGroundColor = System.Drawing.Color.FromArgb(((int)(((byte)(200)))), ((int)(((byte)(200)))), ((int)(((byte)(220)))));
            this.m_lblJerkDecel.BorderStroke = 2;
            this.m_lblJerkDecel.BorderStyle = System.Windows.Forms.ButtonBorderStyle.Solid;
            this.m_lblJerkDecel.Description = "";
            this.m_lblJerkDecel.DisabledColor = System.Drawing.Color.DarkGray;
            this.m_lblJerkDecel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.m_lblJerkDecel.EdgeRadius = 1;
            this.m_lblJerkDecel.ImagePosition = new System.Drawing.Point(0, 0);
            this.m_lblJerkDecel.ImageSize = new System.Drawing.Point(0, 0);
            this.m_lblJerkDecel.LoadImage = null;
            this.m_lblJerkDecel.Location = new System.Drawing.Point(445, 208);
            this.m_lblJerkDecel.MainFont = new System.Drawing.Font("맑은 고딕", 9F, System.Drawing.FontStyle.Bold);
            this.m_lblJerkDecel.MainFontColor = System.Drawing.Color.Black;
            this.m_lblJerkDecel.Margin = new System.Windows.Forms.Padding(0, 3, 0, 3);
            this.m_lblJerkDecel.Name = "m_lblJerkDecel";
            this.m_lblJerkDecel.Size = new System.Drawing.Size(159, 38);
            this.m_lblJerkDecel.SubFont = new System.Drawing.Font("맑은 고딕", 10F);
            this.m_lblJerkDecel.SubFontColor = System.Drawing.Color.Black;
            this.m_lblJerkDecel.SubText = "";
            this.m_lblJerkDecel.TabIndex = 1386;
            this.m_lblJerkDecel.Text = "DECELERATION JERK";
            this.m_lblJerkDecel.TextAlignMain = Sys3Controls.EN_TEXTALIGN.TOP_LEFT;
            this.m_lblJerkDecel.TextAlignSub = Sys3Controls.EN_TEXTALIGN.MIDDLE_CENTER;
            this.m_lblJerkDecel.TextAlignUnit = Sys3Controls.EN_TEXTALIGN.MIDDLE_CENTER;
            this.m_lblJerkDecel.ThemeIndex = 0;
            this.m_lblJerkDecel.UnitAreaRate = 40;
            this.m_lblJerkDecel.UnitFont = new System.Drawing.Font("맑은 고딕", 10F, System.Drawing.FontStyle.Bold);
            this.m_lblJerkDecel.UnitFontColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.m_lblJerkDecel.UnitPositionVertical = false;
            this.m_lblJerkDecel.UnitText = "";
            this.m_lblJerkDecel.UseBorder = true;
            this.m_lblJerkDecel.UseEdgeRadius = false;
            this.m_lblJerkDecel.UseImage = false;
            this.m_lblJerkDecel.UseSubFont = false;
            this.m_lblJerkDecel.UseUnitFont = false;
            // 
            // m_labelDecelTime
            // 
            this.m_labelDecelTime.BackGroundColor = System.Drawing.Color.White;
            this.m_labelDecelTime.BorderStroke = 2;
            this.m_labelDecelTime.BorderStyle = System.Windows.Forms.ButtonBorderStyle.Solid;
            this.m_labelDecelTime.Description = "";
            this.m_labelDecelTime.DisabledColor = System.Drawing.Color.LightGray;
            this.m_labelDecelTime.Dock = System.Windows.Forms.DockStyle.Fill;
            this.m_labelDecelTime.EdgeRadius = 1;
            this.m_labelDecelTime.Enabled = false;
            this.m_labelDecelTime.ImagePosition = new System.Drawing.Point(0, 0);
            this.m_labelDecelTime.ImageSize = new System.Drawing.Point(0, 0);
            this.m_labelDecelTime.LoadImage = null;
            this.m_labelDecelTime.Location = new System.Drawing.Point(606, 167);
            this.m_labelDecelTime.MainFont = new System.Drawing.Font("맑은 고딕", 11F, System.Drawing.FontStyle.Bold);
            this.m_labelDecelTime.MainFontColor = System.Drawing.Color.Black;
            this.m_labelDecelTime.Margin = new System.Windows.Forms.Padding(2, 3, 0, 3);
            this.m_labelDecelTime.Name = "m_labelDecelTime";
            this.m_labelDecelTime.Size = new System.Drawing.Size(280, 35);
            this.m_labelDecelTime.SubFont = new System.Drawing.Font("맑은 고딕", 10F);
            this.m_labelDecelTime.SubFontColor = System.Drawing.Color.Black;
            this.m_labelDecelTime.SubText = "";
            this.m_labelDecelTime.TabIndex = 6;
            this.m_labelDecelTime.Text = "--";
            this.m_labelDecelTime.TextAlignMain = Sys3Controls.EN_TEXTALIGN.BOTTOM_RIGHT;
            this.m_labelDecelTime.TextAlignSub = Sys3Controls.EN_TEXTALIGN.MIDDLE_CENTER;
            this.m_labelDecelTime.TextAlignUnit = Sys3Controls.EN_TEXTALIGN.BOTTOM_RIGHT;
            this.m_labelDecelTime.ThemeIndex = 0;
            this.m_labelDecelTime.UnitAreaRate = 40;
            this.m_labelDecelTime.UnitFont = new System.Drawing.Font("맑은 고딕", 10F, System.Drawing.FontStyle.Bold);
            this.m_labelDecelTime.UnitFontColor = System.Drawing.Color.Black;
            this.m_labelDecelTime.UnitPositionVertical = false;
            this.m_labelDecelTime.UnitText = "sec";
            this.m_labelDecelTime.UseBorder = true;
            this.m_labelDecelTime.UseEdgeRadius = false;
            this.m_labelDecelTime.UseImage = false;
            this.m_labelDecelTime.UseSubFont = false;
            this.m_labelDecelTime.UseUnitFont = true;
            this.m_labelDecelTime.Click += new System.EventHandler(this.Click_Parameters);
            // 
            // m_labelDecel
            // 
            this.m_labelDecel.BackGroundColor = System.Drawing.Color.White;
            this.m_labelDecel.BorderStroke = 2;
            this.m_labelDecel.BorderStyle = System.Windows.Forms.ButtonBorderStyle.Solid;
            this.m_labelDecel.Description = "";
            this.m_labelDecel.DisabledColor = System.Drawing.Color.LightGray;
            this.m_labelDecel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.m_labelDecel.EdgeRadius = 1;
            this.m_labelDecel.Enabled = false;
            this.m_labelDecel.ImagePosition = new System.Drawing.Point(0, 0);
            this.m_labelDecel.ImageSize = new System.Drawing.Point(0, 0);
            this.m_labelDecel.LoadImage = null;
            this.m_labelDecel.Location = new System.Drawing.Point(606, 126);
            this.m_labelDecel.MainFont = new System.Drawing.Font("맑은 고딕", 11F, System.Drawing.FontStyle.Bold);
            this.m_labelDecel.MainFontColor = System.Drawing.Color.Black;
            this.m_labelDecel.Margin = new System.Windows.Forms.Padding(2, 3, 0, 3);
            this.m_labelDecel.Name = "m_labelDecel";
            this.m_labelDecel.Size = new System.Drawing.Size(280, 35);
            this.m_labelDecel.SubFont = new System.Drawing.Font("맑은 고딕", 10F);
            this.m_labelDecel.SubFontColor = System.Drawing.Color.Black;
            this.m_labelDecel.SubText = "";
            this.m_labelDecel.TabIndex = 11;
            this.m_labelDecel.Text = "--";
            this.m_labelDecel.TextAlignMain = Sys3Controls.EN_TEXTALIGN.BOTTOM_RIGHT;
            this.m_labelDecel.TextAlignSub = Sys3Controls.EN_TEXTALIGN.MIDDLE_CENTER;
            this.m_labelDecel.TextAlignUnit = Sys3Controls.EN_TEXTALIGN.BOTTOM_RIGHT;
            this.m_labelDecel.ThemeIndex = 0;
            this.m_labelDecel.UnitAreaRate = 40;
            this.m_labelDecel.UnitFont = new System.Drawing.Font("맑은 고딕", 10F, System.Drawing.FontStyle.Bold);
            this.m_labelDecel.UnitFontColor = System.Drawing.Color.Black;
            this.m_labelDecel.UnitPositionVertical = false;
            this.m_labelDecel.UnitText = "mm/s²";
            this.m_labelDecel.UseBorder = true;
            this.m_labelDecel.UseEdgeRadius = false;
            this.m_labelDecel.UseImage = false;
            this.m_labelDecel.UseSubFont = false;
            this.m_labelDecel.UseUnitFont = true;
            this.m_labelDecel.Click += new System.EventHandler(this.Click_Parameters);
            // 
            // m_lblDecelTime
            // 
            this.m_lblDecelTime.BackGroundColor = System.Drawing.Color.FromArgb(((int)(((byte)(200)))), ((int)(((byte)(200)))), ((int)(((byte)(220)))));
            this.m_lblDecelTime.BorderStroke = 2;
            this.m_lblDecelTime.BorderStyle = System.Windows.Forms.ButtonBorderStyle.Solid;
            this.m_lblDecelTime.Description = "";
            this.m_lblDecelTime.DisabledColor = System.Drawing.Color.DarkGray;
            this.m_lblDecelTime.Dock = System.Windows.Forms.DockStyle.Fill;
            this.m_lblDecelTime.EdgeRadius = 1;
            this.m_lblDecelTime.ImagePosition = new System.Drawing.Point(0, 0);
            this.m_lblDecelTime.ImageSize = new System.Drawing.Point(0, 0);
            this.m_lblDecelTime.LoadImage = null;
            this.m_lblDecelTime.Location = new System.Drawing.Point(445, 167);
            this.m_lblDecelTime.MainFont = new System.Drawing.Font("맑은 고딕", 9F, System.Drawing.FontStyle.Bold);
            this.m_lblDecelTime.MainFontColor = System.Drawing.Color.Black;
            this.m_lblDecelTime.Margin = new System.Windows.Forms.Padding(0, 3, 0, 3);
            this.m_lblDecelTime.Name = "m_lblDecelTime";
            this.m_lblDecelTime.Size = new System.Drawing.Size(159, 35);
            this.m_lblDecelTime.SubFont = new System.Drawing.Font("맑은 고딕", 10F);
            this.m_lblDecelTime.SubFontColor = System.Drawing.Color.Black;
            this.m_lblDecelTime.SubText = "";
            this.m_lblDecelTime.TabIndex = 1386;
            this.m_lblDecelTime.Text = "DECELERATION TIME";
            this.m_lblDecelTime.TextAlignMain = Sys3Controls.EN_TEXTALIGN.TOP_LEFT;
            this.m_lblDecelTime.TextAlignSub = Sys3Controls.EN_TEXTALIGN.MIDDLE_CENTER;
            this.m_lblDecelTime.TextAlignUnit = Sys3Controls.EN_TEXTALIGN.MIDDLE_CENTER;
            this.m_lblDecelTime.ThemeIndex = 0;
            this.m_lblDecelTime.UnitAreaRate = 40;
            this.m_lblDecelTime.UnitFont = new System.Drawing.Font("맑은 고딕", 10F, System.Drawing.FontStyle.Bold);
            this.m_lblDecelTime.UnitFontColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.m_lblDecelTime.UnitPositionVertical = false;
            this.m_lblDecelTime.UnitText = "";
            this.m_lblDecelTime.UseBorder = true;
            this.m_lblDecelTime.UseEdgeRadius = false;
            this.m_lblDecelTime.UseImage = false;
            this.m_lblDecelTime.UseSubFont = false;
            this.m_lblDecelTime.UseUnitFont = false;
            // 
            // m_lblDecel
            // 
            this.m_lblDecel.BackGroundColor = System.Drawing.Color.FromArgb(((int)(((byte)(200)))), ((int)(((byte)(200)))), ((int)(((byte)(220)))));
            this.m_lblDecel.BorderStroke = 2;
            this.m_lblDecel.BorderStyle = System.Windows.Forms.ButtonBorderStyle.Solid;
            this.m_lblDecel.Description = "";
            this.m_lblDecel.DisabledColor = System.Drawing.Color.DarkGray;
            this.m_lblDecel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.m_lblDecel.EdgeRadius = 1;
            this.m_lblDecel.ImagePosition = new System.Drawing.Point(0, 0);
            this.m_lblDecel.ImageSize = new System.Drawing.Point(0, 0);
            this.m_lblDecel.LoadImage = null;
            this.m_lblDecel.Location = new System.Drawing.Point(445, 126);
            this.m_lblDecel.MainFont = new System.Drawing.Font("맑은 고딕", 9F, System.Drawing.FontStyle.Bold);
            this.m_lblDecel.MainFontColor = System.Drawing.Color.Black;
            this.m_lblDecel.Margin = new System.Windows.Forms.Padding(0, 3, 0, 3);
            this.m_lblDecel.Name = "m_lblDecel";
            this.m_lblDecel.Size = new System.Drawing.Size(159, 35);
            this.m_lblDecel.SubFont = new System.Drawing.Font("맑은 고딕", 10F);
            this.m_lblDecel.SubFontColor = System.Drawing.Color.Black;
            this.m_lblDecel.SubText = "";
            this.m_lblDecel.TabIndex = 1386;
            this.m_lblDecel.Text = "DECELERATION";
            this.m_lblDecel.TextAlignMain = Sys3Controls.EN_TEXTALIGN.TOP_LEFT;
            this.m_lblDecel.TextAlignSub = Sys3Controls.EN_TEXTALIGN.MIDDLE_CENTER;
            this.m_lblDecel.TextAlignUnit = Sys3Controls.EN_TEXTALIGN.MIDDLE_CENTER;
            this.m_lblDecel.ThemeIndex = 0;
            this.m_lblDecel.UnitAreaRate = 40;
            this.m_lblDecel.UnitFont = new System.Drawing.Font("맑은 고딕", 10F, System.Drawing.FontStyle.Bold);
            this.m_lblDecel.UnitFontColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.m_lblDecel.UnitPositionVertical = false;
            this.m_lblDecel.UnitText = "";
            this.m_lblDecel.UseBorder = true;
            this.m_lblDecel.UseEdgeRadius = false;
            this.m_lblDecel.UseImage = false;
            this.m_lblDecel.UseSubFont = false;
            this.m_lblDecel.UseUnitFont = false;
            // 
            // m_labelMaxVelocity
            // 
            this.m_labelMaxVelocity.BackGroundColor = System.Drawing.Color.White;
            this.m_labelMaxVelocity.BorderStroke = 2;
            this.m_labelMaxVelocity.BorderStyle = System.Windows.Forms.ButtonBorderStyle.Solid;
            this.m_labelMaxVelocity.Description = "";
            this.m_labelMaxVelocity.DisabledColor = System.Drawing.Color.LightGray;
            this.m_labelMaxVelocity.Dock = System.Windows.Forms.DockStyle.Fill;
            this.m_labelMaxVelocity.EdgeRadius = 1;
            this.m_labelMaxVelocity.Enabled = false;
            this.m_labelMaxVelocity.ImagePosition = new System.Drawing.Point(0, 0);
            this.m_labelMaxVelocity.ImageSize = new System.Drawing.Point(0, 0);
            this.m_labelMaxVelocity.LoadImage = null;
            this.m_labelMaxVelocity.Location = new System.Drawing.Point(606, 85);
            this.m_labelMaxVelocity.MainFont = new System.Drawing.Font("맑은 고딕", 11F, System.Drawing.FontStyle.Bold);
            this.m_labelMaxVelocity.MainFontColor = System.Drawing.Color.Black;
            this.m_labelMaxVelocity.Margin = new System.Windows.Forms.Padding(2, 3, 0, 3);
            this.m_labelMaxVelocity.Name = "m_labelMaxVelocity";
            this.m_labelMaxVelocity.Size = new System.Drawing.Size(280, 35);
            this.m_labelMaxVelocity.SubFont = new System.Drawing.Font("맑은 고딕", 10F);
            this.m_labelMaxVelocity.SubFontColor = System.Drawing.Color.Black;
            this.m_labelMaxVelocity.SubText = "";
            this.m_labelMaxVelocity.TabIndex = 5;
            this.m_labelMaxVelocity.Text = "--";
            this.m_labelMaxVelocity.TextAlignMain = Sys3Controls.EN_TEXTALIGN.BOTTOM_RIGHT;
            this.m_labelMaxVelocity.TextAlignSub = Sys3Controls.EN_TEXTALIGN.MIDDLE_CENTER;
            this.m_labelMaxVelocity.TextAlignUnit = Sys3Controls.EN_TEXTALIGN.BOTTOM_RIGHT;
            this.m_labelMaxVelocity.ThemeIndex = 0;
            this.m_labelMaxVelocity.UnitAreaRate = 40;
            this.m_labelMaxVelocity.UnitFont = new System.Drawing.Font("맑은 고딕", 10F, System.Drawing.FontStyle.Bold);
            this.m_labelMaxVelocity.UnitFontColor = System.Drawing.Color.Black;
            this.m_labelMaxVelocity.UnitPositionVertical = false;
            this.m_labelMaxVelocity.UnitText = "mm/s";
            this.m_labelMaxVelocity.UseBorder = true;
            this.m_labelMaxVelocity.UseEdgeRadius = false;
            this.m_labelMaxVelocity.UseImage = false;
            this.m_labelMaxVelocity.UseSubFont = false;
            this.m_labelMaxVelocity.UseUnitFont = true;
            this.m_labelMaxVelocity.Click += new System.EventHandler(this.Click_Parameters);
            // 
            // m_lblTimeout
            // 
            this.m_lblTimeout.BackGroundColor = System.Drawing.Color.FromArgb(((int)(((byte)(200)))), ((int)(((byte)(200)))), ((int)(((byte)(220)))));
            this.m_lblTimeout.BorderStroke = 2;
            this.m_lblTimeout.BorderStyle = System.Windows.Forms.ButtonBorderStyle.Solid;
            this.m_lblTimeout.Description = "";
            this.m_lblTimeout.DisabledColor = System.Drawing.Color.DarkGray;
            this.m_lblTimeout.Dock = System.Windows.Forms.DockStyle.Fill;
            this.m_lblTimeout.EdgeRadius = 1;
            this.m_lblTimeout.ImagePosition = new System.Drawing.Point(0, 0);
            this.m_lblTimeout.ImageSize = new System.Drawing.Point(0, 0);
            this.m_lblTimeout.LoadImage = null;
            this.m_lblTimeout.Location = new System.Drawing.Point(5, 44);
            this.m_lblTimeout.MainFont = new System.Drawing.Font("맑은 고딕", 9F, System.Drawing.FontStyle.Bold);
            this.m_lblTimeout.MainFontColor = System.Drawing.Color.Black;
            this.m_lblTimeout.Margin = new System.Windows.Forms.Padding(0, 3, 0, 3);
            this.m_lblTimeout.Name = "m_lblTimeout";
            this.m_lblTimeout.Size = new System.Drawing.Size(159, 35);
            this.m_lblTimeout.SubFont = new System.Drawing.Font("맑은 고딕", 10F);
            this.m_lblTimeout.SubFontColor = System.Drawing.Color.Black;
            this.m_lblTimeout.SubText = "";
            this.m_lblTimeout.TabIndex = 1386;
            this.m_lblTimeout.Text = "MOTION TIMEOUT";
            this.m_lblTimeout.TextAlignMain = Sys3Controls.EN_TEXTALIGN.TOP_LEFT;
            this.m_lblTimeout.TextAlignSub = Sys3Controls.EN_TEXTALIGN.MIDDLE_CENTER;
            this.m_lblTimeout.TextAlignUnit = Sys3Controls.EN_TEXTALIGN.MIDDLE_CENTER;
            this.m_lblTimeout.ThemeIndex = 0;
            this.m_lblTimeout.UnitAreaRate = 40;
            this.m_lblTimeout.UnitFont = new System.Drawing.Font("맑은 고딕", 10F, System.Drawing.FontStyle.Bold);
            this.m_lblTimeout.UnitFontColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.m_lblTimeout.UnitPositionVertical = false;
            this.m_lblTimeout.UnitText = "";
            this.m_lblTimeout.UseBorder = true;
            this.m_lblTimeout.UseEdgeRadius = false;
            this.m_lblTimeout.UseImage = false;
            this.m_lblTimeout.UseSubFont = false;
            this.m_lblTimeout.UseUnitFont = false;
            // 
            // m_labelTimeout
            // 
            this.m_labelTimeout.BackGroundColor = System.Drawing.Color.White;
            this.m_labelTimeout.BorderStroke = 2;
            this.m_labelTimeout.BorderStyle = System.Windows.Forms.ButtonBorderStyle.Solid;
            this.m_labelTimeout.Description = "";
            this.m_labelTimeout.DisabledColor = System.Drawing.Color.LightGray;
            this.m_labelTimeout.Dock = System.Windows.Forms.DockStyle.Fill;
            this.m_labelTimeout.EdgeRadius = 1;
            this.m_labelTimeout.Enabled = false;
            this.m_labelTimeout.ImagePosition = new System.Drawing.Point(0, 0);
            this.m_labelTimeout.ImageSize = new System.Drawing.Point(0, 0);
            this.m_labelTimeout.LoadImage = null;
            this.m_labelTimeout.Location = new System.Drawing.Point(166, 44);
            this.m_labelTimeout.MainFont = new System.Drawing.Font("맑은 고딕", 11F, System.Drawing.FontStyle.Bold);
            this.m_labelTimeout.MainFontColor = System.Drawing.Color.Black;
            this.m_labelTimeout.Margin = new System.Windows.Forms.Padding(2, 3, 5, 3);
            this.m_labelTimeout.Name = "m_labelTimeout";
            this.m_labelTimeout.Size = new System.Drawing.Size(274, 35);
            this.m_labelTimeout.SubFont = new System.Drawing.Font("맑은 고딕", 10F);
            this.m_labelTimeout.SubFontColor = System.Drawing.Color.Black;
            this.m_labelTimeout.SubText = "";
            this.m_labelTimeout.TabIndex = 4;
            this.m_labelTimeout.Text = "--";
            this.m_labelTimeout.TextAlignMain = Sys3Controls.EN_TEXTALIGN.BOTTOM_RIGHT;
            this.m_labelTimeout.TextAlignSub = Sys3Controls.EN_TEXTALIGN.MIDDLE_CENTER;
            this.m_labelTimeout.TextAlignUnit = Sys3Controls.EN_TEXTALIGN.BOTTOM_RIGHT;
            this.m_labelTimeout.ThemeIndex = 0;
            this.m_labelTimeout.UnitAreaRate = 40;
            this.m_labelTimeout.UnitFont = new System.Drawing.Font("맑은 고딕", 10F, System.Drawing.FontStyle.Bold);
            this.m_labelTimeout.UnitFontColor = System.Drawing.Color.Black;
            this.m_labelTimeout.UnitPositionVertical = false;
            this.m_labelTimeout.UnitText = "ms";
            this.m_labelTimeout.UseBorder = true;
            this.m_labelTimeout.UseEdgeRadius = false;
            this.m_labelTimeout.UseImage = false;
            this.m_labelTimeout.UseSubFont = false;
            this.m_labelTimeout.UseUnitFont = true;
            this.m_labelTimeout.Click += new System.EventHandler(this.Click_Parameters);
            // 
            // m_lblVelocity
            // 
            this.m_lblVelocity.BackGroundColor = System.Drawing.Color.FromArgb(((int)(((byte)(200)))), ((int)(((byte)(200)))), ((int)(((byte)(220)))));
            this.m_lblVelocity.BorderStroke = 2;
            this.m_lblVelocity.BorderStyle = System.Windows.Forms.ButtonBorderStyle.Solid;
            this.m_lblVelocity.Description = "";
            this.m_lblVelocity.DisabledColor = System.Drawing.Color.DarkGray;
            this.m_lblVelocity.Dock = System.Windows.Forms.DockStyle.Fill;
            this.m_lblVelocity.EdgeRadius = 1;
            this.m_lblVelocity.ImagePosition = new System.Drawing.Point(0, 0);
            this.m_lblVelocity.ImageSize = new System.Drawing.Point(0, 0);
            this.m_lblVelocity.LoadImage = null;
            this.m_lblVelocity.Location = new System.Drawing.Point(5, 85);
            this.m_lblVelocity.MainFont = new System.Drawing.Font("맑은 고딕", 9F, System.Drawing.FontStyle.Bold);
            this.m_lblVelocity.MainFontColor = System.Drawing.Color.Black;
            this.m_lblVelocity.Margin = new System.Windows.Forms.Padding(0, 3, 0, 3);
            this.m_lblVelocity.Name = "m_lblVelocity";
            this.m_lblVelocity.Size = new System.Drawing.Size(159, 35);
            this.m_lblVelocity.SubFont = new System.Drawing.Font("맑은 고딕", 10F);
            this.m_lblVelocity.SubFontColor = System.Drawing.Color.Black;
            this.m_lblVelocity.SubText = "";
            this.m_lblVelocity.TabIndex = 1386;
            this.m_lblVelocity.Text = "VELOCITY";
            this.m_lblVelocity.TextAlignMain = Sys3Controls.EN_TEXTALIGN.TOP_LEFT;
            this.m_lblVelocity.TextAlignSub = Sys3Controls.EN_TEXTALIGN.MIDDLE_CENTER;
            this.m_lblVelocity.TextAlignUnit = Sys3Controls.EN_TEXTALIGN.MIDDLE_CENTER;
            this.m_lblVelocity.ThemeIndex = 0;
            this.m_lblVelocity.UnitAreaRate = 40;
            this.m_lblVelocity.UnitFont = new System.Drawing.Font("맑은 고딕", 10F, System.Drawing.FontStyle.Bold);
            this.m_lblVelocity.UnitFontColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.m_lblVelocity.UnitPositionVertical = false;
            this.m_lblVelocity.UnitText = "";
            this.m_lblVelocity.UseBorder = true;
            this.m_lblVelocity.UseEdgeRadius = false;
            this.m_lblVelocity.UseImage = false;
            this.m_lblVelocity.UseSubFont = false;
            this.m_lblVelocity.UseUnitFont = false;
            // 
            // m_labelVelocity
            // 
            this.m_labelVelocity.BackGroundColor = System.Drawing.Color.White;
            this.m_labelVelocity.BorderStroke = 2;
            this.m_labelVelocity.BorderStyle = System.Windows.Forms.ButtonBorderStyle.Solid;
            this.m_labelVelocity.Description = "";
            this.m_labelVelocity.DisabledColor = System.Drawing.Color.LightGray;
            this.m_labelVelocity.Dock = System.Windows.Forms.DockStyle.Fill;
            this.m_labelVelocity.EdgeRadius = 1;
            this.m_labelVelocity.Enabled = false;
            this.m_labelVelocity.ImagePosition = new System.Drawing.Point(0, 0);
            this.m_labelVelocity.ImageSize = new System.Drawing.Point(0, 0);
            this.m_labelVelocity.LoadImage = null;
            this.m_labelVelocity.Location = new System.Drawing.Point(166, 85);
            this.m_labelVelocity.MainFont = new System.Drawing.Font("맑은 고딕", 11F, System.Drawing.FontStyle.Bold);
            this.m_labelVelocity.MainFontColor = System.Drawing.Color.Black;
            this.m_labelVelocity.Margin = new System.Windows.Forms.Padding(2, 3, 5, 3);
            this.m_labelVelocity.Name = "m_labelVelocity";
            this.m_labelVelocity.Size = new System.Drawing.Size(274, 35);
            this.m_labelVelocity.SubFont = new System.Drawing.Font("맑은 고딕", 10F);
            this.m_labelVelocity.SubFontColor = System.Drawing.Color.Black;
            this.m_labelVelocity.SubText = "";
            this.m_labelVelocity.TabIndex = 1;
            this.m_labelVelocity.Text = "--";
            this.m_labelVelocity.TextAlignMain = Sys3Controls.EN_TEXTALIGN.BOTTOM_RIGHT;
            this.m_labelVelocity.TextAlignSub = Sys3Controls.EN_TEXTALIGN.MIDDLE_CENTER;
            this.m_labelVelocity.TextAlignUnit = Sys3Controls.EN_TEXTALIGN.BOTTOM_RIGHT;
            this.m_labelVelocity.ThemeIndex = 0;
            this.m_labelVelocity.UnitAreaRate = 40;
            this.m_labelVelocity.UnitFont = new System.Drawing.Font("맑은 고딕", 10F, System.Drawing.FontStyle.Bold);
            this.m_labelVelocity.UnitFontColor = System.Drawing.Color.Black;
            this.m_labelVelocity.UnitPositionVertical = false;
            this.m_labelVelocity.UnitText = "mm/s";
            this.m_labelVelocity.UseBorder = true;
            this.m_labelVelocity.UseEdgeRadius = false;
            this.m_labelVelocity.UseImage = false;
            this.m_labelVelocity.UseSubFont = false;
            this.m_labelVelocity.UseUnitFont = true;
            this.m_labelVelocity.Click += new System.EventHandler(this.Click_Parameters);
            // 
            // m_lblJerkAccel
            // 
            this.m_lblJerkAccel.BackGroundColor = System.Drawing.Color.FromArgb(((int)(((byte)(200)))), ((int)(((byte)(200)))), ((int)(((byte)(220)))));
            this.m_lblJerkAccel.BorderStroke = 2;
            this.m_lblJerkAccel.BorderStyle = System.Windows.Forms.ButtonBorderStyle.Solid;
            this.m_lblJerkAccel.Description = "";
            this.m_lblJerkAccel.DisabledColor = System.Drawing.Color.DarkGray;
            this.m_lblJerkAccel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.m_lblJerkAccel.EdgeRadius = 1;
            this.m_lblJerkAccel.ImagePosition = new System.Drawing.Point(0, 0);
            this.m_lblJerkAccel.ImageSize = new System.Drawing.Point(0, 0);
            this.m_lblJerkAccel.LoadImage = null;
            this.m_lblJerkAccel.Location = new System.Drawing.Point(5, 208);
            this.m_lblJerkAccel.MainFont = new System.Drawing.Font("맑은 고딕", 9F, System.Drawing.FontStyle.Bold);
            this.m_lblJerkAccel.MainFontColor = System.Drawing.Color.Black;
            this.m_lblJerkAccel.Margin = new System.Windows.Forms.Padding(0, 3, 0, 3);
            this.m_lblJerkAccel.Name = "m_lblJerkAccel";
            this.m_lblJerkAccel.Size = new System.Drawing.Size(159, 38);
            this.m_lblJerkAccel.SubFont = new System.Drawing.Font("맑은 고딕", 10F);
            this.m_lblJerkAccel.SubFontColor = System.Drawing.Color.Black;
            this.m_lblJerkAccel.SubText = "";
            this.m_lblJerkAccel.TabIndex = 1386;
            this.m_lblJerkAccel.Text = "ACCELERATION JERK";
            this.m_lblJerkAccel.TextAlignMain = Sys3Controls.EN_TEXTALIGN.TOP_LEFT;
            this.m_lblJerkAccel.TextAlignSub = Sys3Controls.EN_TEXTALIGN.MIDDLE_CENTER;
            this.m_lblJerkAccel.TextAlignUnit = Sys3Controls.EN_TEXTALIGN.MIDDLE_CENTER;
            this.m_lblJerkAccel.ThemeIndex = 0;
            this.m_lblJerkAccel.UnitAreaRate = 40;
            this.m_lblJerkAccel.UnitFont = new System.Drawing.Font("맑은 고딕", 10F, System.Drawing.FontStyle.Bold);
            this.m_lblJerkAccel.UnitFontColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.m_lblJerkAccel.UnitPositionVertical = false;
            this.m_lblJerkAccel.UnitText = "";
            this.m_lblJerkAccel.UseBorder = true;
            this.m_lblJerkAccel.UseEdgeRadius = false;
            this.m_lblJerkAccel.UseImage = false;
            this.m_lblJerkAccel.UseSubFont = false;
            this.m_lblJerkAccel.UseUnitFont = false;
            // 
            // m_lblAccel
            // 
            this.m_lblAccel.BackGroundColor = System.Drawing.Color.FromArgb(((int)(((byte)(200)))), ((int)(((byte)(200)))), ((int)(((byte)(220)))));
            this.m_lblAccel.BorderStroke = 2;
            this.m_lblAccel.BorderStyle = System.Windows.Forms.ButtonBorderStyle.Solid;
            this.m_lblAccel.Description = "";
            this.m_lblAccel.DisabledColor = System.Drawing.Color.DarkGray;
            this.m_lblAccel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.m_lblAccel.EdgeRadius = 1;
            this.m_lblAccel.ImagePosition = new System.Drawing.Point(0, 0);
            this.m_lblAccel.ImageSize = new System.Drawing.Point(0, 0);
            this.m_lblAccel.LoadImage = null;
            this.m_lblAccel.Location = new System.Drawing.Point(5, 126);
            this.m_lblAccel.MainFont = new System.Drawing.Font("맑은 고딕", 9F, System.Drawing.FontStyle.Bold);
            this.m_lblAccel.MainFontColor = System.Drawing.Color.Black;
            this.m_lblAccel.Margin = new System.Windows.Forms.Padding(0, 3, 0, 3);
            this.m_lblAccel.Name = "m_lblAccel";
            this.m_lblAccel.Size = new System.Drawing.Size(159, 35);
            this.m_lblAccel.SubFont = new System.Drawing.Font("맑은 고딕", 10F);
            this.m_lblAccel.SubFontColor = System.Drawing.Color.Black;
            this.m_lblAccel.SubText = "";
            this.m_lblAccel.TabIndex = 1386;
            this.m_lblAccel.Text = "ACCELERATION";
            this.m_lblAccel.TextAlignMain = Sys3Controls.EN_TEXTALIGN.TOP_LEFT;
            this.m_lblAccel.TextAlignSub = Sys3Controls.EN_TEXTALIGN.MIDDLE_CENTER;
            this.m_lblAccel.TextAlignUnit = Sys3Controls.EN_TEXTALIGN.MIDDLE_CENTER;
            this.m_lblAccel.ThemeIndex = 0;
            this.m_lblAccel.UnitAreaRate = 40;
            this.m_lblAccel.UnitFont = new System.Drawing.Font("맑은 고딕", 10F, System.Drawing.FontStyle.Bold);
            this.m_lblAccel.UnitFontColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.m_lblAccel.UnitPositionVertical = false;
            this.m_lblAccel.UnitText = "";
            this.m_lblAccel.UseBorder = true;
            this.m_lblAccel.UseEdgeRadius = false;
            this.m_lblAccel.UseImage = false;
            this.m_lblAccel.UseSubFont = false;
            this.m_lblAccel.UseUnitFont = false;
            // 
            // m_lblAccelTime
            // 
            this.m_lblAccelTime.BackGroundColor = System.Drawing.Color.FromArgb(((int)(((byte)(200)))), ((int)(((byte)(200)))), ((int)(((byte)(220)))));
            this.m_lblAccelTime.BorderStroke = 2;
            this.m_lblAccelTime.BorderStyle = System.Windows.Forms.ButtonBorderStyle.Solid;
            this.m_lblAccelTime.Description = "";
            this.m_lblAccelTime.DisabledColor = System.Drawing.Color.DarkGray;
            this.m_lblAccelTime.Dock = System.Windows.Forms.DockStyle.Fill;
            this.m_lblAccelTime.EdgeRadius = 1;
            this.m_lblAccelTime.ImagePosition = new System.Drawing.Point(0, 0);
            this.m_lblAccelTime.ImageSize = new System.Drawing.Point(0, 0);
            this.m_lblAccelTime.LoadImage = null;
            this.m_lblAccelTime.Location = new System.Drawing.Point(5, 167);
            this.m_lblAccelTime.MainFont = new System.Drawing.Font("맑은 고딕", 9F, System.Drawing.FontStyle.Bold);
            this.m_lblAccelTime.MainFontColor = System.Drawing.Color.Black;
            this.m_lblAccelTime.Margin = new System.Windows.Forms.Padding(0, 3, 0, 3);
            this.m_lblAccelTime.Name = "m_lblAccelTime";
            this.m_lblAccelTime.Size = new System.Drawing.Size(159, 35);
            this.m_lblAccelTime.SubFont = new System.Drawing.Font("맑은 고딕", 10F);
            this.m_lblAccelTime.SubFontColor = System.Drawing.Color.Black;
            this.m_lblAccelTime.SubText = "";
            this.m_lblAccelTime.TabIndex = 1386;
            this.m_lblAccelTime.Text = "ACCELERATION TIME";
            this.m_lblAccelTime.TextAlignMain = Sys3Controls.EN_TEXTALIGN.TOP_LEFT;
            this.m_lblAccelTime.TextAlignSub = Sys3Controls.EN_TEXTALIGN.MIDDLE_CENTER;
            this.m_lblAccelTime.TextAlignUnit = Sys3Controls.EN_TEXTALIGN.MIDDLE_CENTER;
            this.m_lblAccelTime.ThemeIndex = 0;
            this.m_lblAccelTime.UnitAreaRate = 40;
            this.m_lblAccelTime.UnitFont = new System.Drawing.Font("맑은 고딕", 10F, System.Drawing.FontStyle.Bold);
            this.m_lblAccelTime.UnitFontColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.m_lblAccelTime.UnitPositionVertical = false;
            this.m_lblAccelTime.UnitText = "";
            this.m_lblAccelTime.UseBorder = true;
            this.m_lblAccelTime.UseEdgeRadius = false;
            this.m_lblAccelTime.UseImage = false;
            this.m_lblAccelTime.UseSubFont = false;
            this.m_lblAccelTime.UseUnitFont = false;
            // 
            // m_labelAccel
            // 
            this.m_labelAccel.BackGroundColor = System.Drawing.Color.White;
            this.m_labelAccel.BorderStroke = 2;
            this.m_labelAccel.BorderStyle = System.Windows.Forms.ButtonBorderStyle.Solid;
            this.m_labelAccel.Description = "";
            this.m_labelAccel.DisabledColor = System.Drawing.Color.LightGray;
            this.m_labelAccel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.m_labelAccel.EdgeRadius = 1;
            this.m_labelAccel.Enabled = false;
            this.m_labelAccel.ImagePosition = new System.Drawing.Point(0, 0);
            this.m_labelAccel.ImageSize = new System.Drawing.Point(0, 0);
            this.m_labelAccel.LoadImage = null;
            this.m_labelAccel.Location = new System.Drawing.Point(166, 126);
            this.m_labelAccel.MainFont = new System.Drawing.Font("맑은 고딕", 11F, System.Drawing.FontStyle.Bold);
            this.m_labelAccel.MainFontColor = System.Drawing.Color.Black;
            this.m_labelAccel.Margin = new System.Windows.Forms.Padding(2, 3, 5, 3);
            this.m_labelAccel.Name = "m_labelAccel";
            this.m_labelAccel.Size = new System.Drawing.Size(274, 35);
            this.m_labelAccel.SubFont = new System.Drawing.Font("맑은 고딕", 10F);
            this.m_labelAccel.SubFontColor = System.Drawing.Color.Black;
            this.m_labelAccel.SubText = "";
            this.m_labelAccel.TabIndex = 10;
            this.m_labelAccel.Text = "--";
            this.m_labelAccel.TextAlignMain = Sys3Controls.EN_TEXTALIGN.BOTTOM_RIGHT;
            this.m_labelAccel.TextAlignSub = Sys3Controls.EN_TEXTALIGN.MIDDLE_CENTER;
            this.m_labelAccel.TextAlignUnit = Sys3Controls.EN_TEXTALIGN.BOTTOM_RIGHT;
            this.m_labelAccel.ThemeIndex = 0;
            this.m_labelAccel.UnitAreaRate = 40;
            this.m_labelAccel.UnitFont = new System.Drawing.Font("맑은 고딕", 10F, System.Drawing.FontStyle.Bold);
            this.m_labelAccel.UnitFontColor = System.Drawing.Color.Black;
            this.m_labelAccel.UnitPositionVertical = false;
            this.m_labelAccel.UnitText = "mm/s²";
            this.m_labelAccel.UseBorder = true;
            this.m_labelAccel.UseEdgeRadius = false;
            this.m_labelAccel.UseImage = false;
            this.m_labelAccel.UseSubFont = false;
            this.m_labelAccel.UseUnitFont = true;
            this.m_labelAccel.Click += new System.EventHandler(this.Click_Parameters);
            // 
            // m_labelJerkAccel
            // 
            this.m_labelJerkAccel.BackGroundColor = System.Drawing.Color.White;
            this.m_labelJerkAccel.BorderStroke = 2;
            this.m_labelJerkAccel.BorderStyle = System.Windows.Forms.ButtonBorderStyle.Solid;
            this.m_labelJerkAccel.Description = "";
            this.m_labelJerkAccel.DisabledColor = System.Drawing.Color.LightGray;
            this.m_labelJerkAccel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.m_labelJerkAccel.EdgeRadius = 1;
            this.m_labelJerkAccel.Enabled = false;
            this.m_labelJerkAccel.ImagePosition = new System.Drawing.Point(0, 0);
            this.m_labelJerkAccel.ImageSize = new System.Drawing.Point(0, 0);
            this.m_labelJerkAccel.LoadImage = null;
            this.m_labelJerkAccel.Location = new System.Drawing.Point(166, 208);
            this.m_labelJerkAccel.MainFont = new System.Drawing.Font("맑은 고딕", 11F, System.Drawing.FontStyle.Bold);
            this.m_labelJerkAccel.MainFontColor = System.Drawing.Color.Black;
            this.m_labelJerkAccel.Margin = new System.Windows.Forms.Padding(2, 3, 5, 3);
            this.m_labelJerkAccel.Name = "m_labelJerkAccel";
            this.m_labelJerkAccel.Size = new System.Drawing.Size(274, 38);
            this.m_labelJerkAccel.SubFont = new System.Drawing.Font("맑은 고딕", 10F);
            this.m_labelJerkAccel.SubFontColor = System.Drawing.Color.Black;
            this.m_labelJerkAccel.SubText = "";
            this.m_labelJerkAccel.TabIndex = 3;
            this.m_labelJerkAccel.Text = "--";
            this.m_labelJerkAccel.TextAlignMain = Sys3Controls.EN_TEXTALIGN.BOTTOM_RIGHT;
            this.m_labelJerkAccel.TextAlignSub = Sys3Controls.EN_TEXTALIGN.MIDDLE_CENTER;
            this.m_labelJerkAccel.TextAlignUnit = Sys3Controls.EN_TEXTALIGN.BOTTOM_RIGHT;
            this.m_labelJerkAccel.ThemeIndex = 0;
            this.m_labelJerkAccel.UnitAreaRate = 40;
            this.m_labelJerkAccel.UnitFont = new System.Drawing.Font("맑은 고딕", 10F, System.Drawing.FontStyle.Bold);
            this.m_labelJerkAccel.UnitFontColor = System.Drawing.Color.Black;
            this.m_labelJerkAccel.UnitPositionVertical = false;
            this.m_labelJerkAccel.UnitText = "mm/s³";
            this.m_labelJerkAccel.UseBorder = true;
            this.m_labelJerkAccel.UseEdgeRadius = false;
            this.m_labelJerkAccel.UseImage = false;
            this.m_labelJerkAccel.UseSubFont = false;
            this.m_labelJerkAccel.UseUnitFont = true;
            this.m_labelJerkAccel.Click += new System.EventHandler(this.Click_Parameters);
            // 
            // m_labelAccelTime
            // 
            this.m_labelAccelTime.BackGroundColor = System.Drawing.Color.White;
            this.m_labelAccelTime.BorderStroke = 2;
            this.m_labelAccelTime.BorderStyle = System.Windows.Forms.ButtonBorderStyle.Solid;
            this.m_labelAccelTime.Description = "";
            this.m_labelAccelTime.DisabledColor = System.Drawing.Color.LightGray;
            this.m_labelAccelTime.Dock = System.Windows.Forms.DockStyle.Fill;
            this.m_labelAccelTime.EdgeRadius = 1;
            this.m_labelAccelTime.Enabled = false;
            this.m_labelAccelTime.ImagePosition = new System.Drawing.Point(0, 0);
            this.m_labelAccelTime.ImageSize = new System.Drawing.Point(0, 0);
            this.m_labelAccelTime.LoadImage = null;
            this.m_labelAccelTime.Location = new System.Drawing.Point(166, 167);
            this.m_labelAccelTime.MainFont = new System.Drawing.Font("맑은 고딕", 11F, System.Drawing.FontStyle.Bold);
            this.m_labelAccelTime.MainFontColor = System.Drawing.Color.Black;
            this.m_labelAccelTime.Margin = new System.Windows.Forms.Padding(2, 3, 5, 3);
            this.m_labelAccelTime.Name = "m_labelAccelTime";
            this.m_labelAccelTime.Size = new System.Drawing.Size(274, 35);
            this.m_labelAccelTime.SubFont = new System.Drawing.Font("맑은 고딕", 10F);
            this.m_labelAccelTime.SubFontColor = System.Drawing.Color.Black;
            this.m_labelAccelTime.SubText = "";
            this.m_labelAccelTime.TabIndex = 2;
            this.m_labelAccelTime.Text = "--";
            this.m_labelAccelTime.TextAlignMain = Sys3Controls.EN_TEXTALIGN.BOTTOM_RIGHT;
            this.m_labelAccelTime.TextAlignSub = Sys3Controls.EN_TEXTALIGN.MIDDLE_CENTER;
            this.m_labelAccelTime.TextAlignUnit = Sys3Controls.EN_TEXTALIGN.BOTTOM_RIGHT;
            this.m_labelAccelTime.ThemeIndex = 0;
            this.m_labelAccelTime.UnitAreaRate = 40;
            this.m_labelAccelTime.UnitFont = new System.Drawing.Font("맑은 고딕", 10F, System.Drawing.FontStyle.Bold);
            this.m_labelAccelTime.UnitFontColor = System.Drawing.Color.Black;
            this.m_labelAccelTime.UnitPositionVertical = false;
            this.m_labelAccelTime.UnitText = "sec";
            this.m_labelAccelTime.UseBorder = true;
            this.m_labelAccelTime.UseEdgeRadius = false;
            this.m_labelAccelTime.UseImage = false;
            this.m_labelAccelTime.UseSubFont = false;
            this.m_labelAccelTime.UseUnitFont = true;
            this.m_labelAccelTime.Click += new System.EventHandler(this.Click_Parameters);
            // 
            // m_lblMaxVelocity
            // 
            this.m_lblMaxVelocity.BackGroundColor = System.Drawing.Color.FromArgb(((int)(((byte)(200)))), ((int)(((byte)(200)))), ((int)(((byte)(220)))));
            this.m_lblMaxVelocity.BorderStroke = 2;
            this.m_lblMaxVelocity.BorderStyle = System.Windows.Forms.ButtonBorderStyle.Solid;
            this.m_lblMaxVelocity.Description = "";
            this.m_lblMaxVelocity.DisabledColor = System.Drawing.Color.DarkGray;
            this.m_lblMaxVelocity.Dock = System.Windows.Forms.DockStyle.Fill;
            this.m_lblMaxVelocity.EdgeRadius = 1;
            this.m_lblMaxVelocity.ImagePosition = new System.Drawing.Point(0, 0);
            this.m_lblMaxVelocity.ImageSize = new System.Drawing.Point(0, 0);
            this.m_lblMaxVelocity.LoadImage = null;
            this.m_lblMaxVelocity.Location = new System.Drawing.Point(445, 85);
            this.m_lblMaxVelocity.MainFont = new System.Drawing.Font("맑은 고딕", 9F, System.Drawing.FontStyle.Bold);
            this.m_lblMaxVelocity.MainFontColor = System.Drawing.Color.Black;
            this.m_lblMaxVelocity.Margin = new System.Windows.Forms.Padding(0, 3, 0, 3);
            this.m_lblMaxVelocity.Name = "m_lblMaxVelocity";
            this.m_lblMaxVelocity.Size = new System.Drawing.Size(159, 35);
            this.m_lblMaxVelocity.SubFont = new System.Drawing.Font("맑은 고딕", 10F);
            this.m_lblMaxVelocity.SubFontColor = System.Drawing.Color.Black;
            this.m_lblMaxVelocity.SubText = "";
            this.m_lblMaxVelocity.TabIndex = 1386;
            this.m_lblMaxVelocity.Text = "MAX VELOCITY";
            this.m_lblMaxVelocity.TextAlignMain = Sys3Controls.EN_TEXTALIGN.TOP_LEFT;
            this.m_lblMaxVelocity.TextAlignSub = Sys3Controls.EN_TEXTALIGN.MIDDLE_CENTER;
            this.m_lblMaxVelocity.TextAlignUnit = Sys3Controls.EN_TEXTALIGN.MIDDLE_CENTER;
            this.m_lblMaxVelocity.ThemeIndex = 0;
            this.m_lblMaxVelocity.UnitAreaRate = 40;
            this.m_lblMaxVelocity.UnitFont = new System.Drawing.Font("맑은 고딕", 10F, System.Drawing.FontStyle.Bold);
            this.m_lblMaxVelocity.UnitFontColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.m_lblMaxVelocity.UnitPositionVertical = false;
            this.m_lblMaxVelocity.UnitText = "";
            this.m_lblMaxVelocity.UseBorder = true;
            this.m_lblMaxVelocity.UseEdgeRadius = false;
            this.m_lblMaxVelocity.UseImage = false;
            this.m_lblMaxVelocity.UseSubFont = false;
            this.m_lblMaxVelocity.UseUnitFont = false;
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 2;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 24.70588F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 75.29412F));
            this.tableLayoutPanel1.Controls.Add(this.m_groupSpeedContents, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.m_groupSelectedContents, 1, 0);
            this.tableLayoutPanel1.Controls.Add(this.m_groupSpeedParameters, 1, 1);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel1.Margin = new System.Windows.Forms.Padding(0);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 2;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 284F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(1190, 411);
            this.tableLayoutPanel1.TabIndex = 1393;
            // 
            // MotorSpeed
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.Controls.Add(this.tableLayoutPanel1);
            this.Margin = new System.Windows.Forms.Padding(0);
            this.Name = "MotorSpeed";
            this.Size = new System.Drawing.Size(1190, 411);
            this.m_groupSpeedContents.ResumeLayout(false);
            this._tableLayoutPanel_GantryConfiguration.ResumeLayout(false);
            this.m_groupSelectedContents.ResumeLayout(false);
            this.tableLayoutPanel2.ResumeLayout(false);
            this.m_groupSpeedParameters.ResumeLayout(false);
            this.tableLayoutPanel3.ResumeLayout(false);
            this.tableLayoutPanel1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

		private Sys3Controls.Sys3LedLabel m_ledCustom2;
		private Sys3Controls.Sys3LedLabel m_ledCustom1;
		private Sys3Controls.Sys3LedLabel m_ledManual;
		private Sys3Controls.Sys3LedLabel m_ledJogHigh;
		private Sys3Controls.Sys3LedLabel m_ledJogLow;
		private Sys3Controls.Sys3LedLabel m_ledRun;
		private Sys3Controls.Sys3GroupBoxContainer m_groupSpeedContents;
        private Sys3Controls.Sys3GroupBoxContainer m_groupSelectedContents;
        private Sys3Controls.Sys3GroupBoxContainer m_groupSpeedParameters;
		private Sys3Controls.Sys3Label m_labelRun;
		private Sys3Controls.Sys3Label m_labelJogLow;
		private Sys3Controls.Sys3Label m_labelJogHigh;
		private Sys3Controls.Sys3Label m_labelManual;
		private Sys3Controls.Sys3Label m_labelCustom1;
		private Sys3Controls.Sys3Label m_labelCustom2;
		private Sys3Controls.Sys3Label m_labelName;
		private Sys3Controls.Sys3Label m_labelSpeedPattern;
		private Sys3Controls.Sys3Label m_labelVelocity;
		private Sys3Controls.Sys3Label m_labelAccelTime;
		private Sys3Controls.Sys3Label m_labelJerkAccel;
		private Sys3Controls.Sys3Label m_labelTimeout;
		private Sys3Controls.Sys3Label m_labelMaxVelocity;
		private Sys3Controls.Sys3Label m_labelDecelTime;
		private Sys3Controls.Sys3Label m_labelJerkDecel;
		private Sys3Controls.Sys3Label m_lblName;
		private Sys3Controls.Sys3Label m_lblSpeedPattern;
		private Sys3Controls.Sys3Label m_lblVelocity;
		private Sys3Controls.Sys3Label m_lblAccelTime;
		private Sys3Controls.Sys3Label m_lblJerkAccel;
		private Sys3Controls.Sys3Label m_lblTimeout;
		private Sys3Controls.Sys3Label m_lblJerkDecel;
		private Sys3Controls.Sys3Label m_lblDecelTime;
		private Sys3Controls.Sys3Label m_lblMaxVelocity;
        private Sys3Controls.Sys3Label m_lblShortDistance;
        private Sys3Controls.Sys3Label m_labelShortDistance;
        private Sys3Controls.Sys3Label m_lblShortDistanceAuto;
        private Sys3Controls.Sys3Label m_labelShortDistanceAuto;
		private Sys3Controls.Sys3Label m_labelAccel;
		private Sys3Controls.Sys3Label m_labelDecel;
		private Sys3Controls.Sys3Label m_lblAccel;
		private Sys3Controls.Sys3Label m_lblDecel;
		private Sys3Controls.Sys3button m_btnCopy;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.TableLayoutPanel _tableLayoutPanel_GantryConfiguration;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel2;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel3;
    }
}
