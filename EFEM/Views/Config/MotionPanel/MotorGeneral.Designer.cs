namespace FrameOfSystem3.Views.Config.MotionPanel
{
    partial class MotorGeneral
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
            this.m_groupSpeedContents = new Sys3Controls.Sys3GroupBoxContainer();
            this._tableLayoutPanel_GantryStatus = new System.Windows.Forms.TableLayoutPanel();
            this.sys3Label8 = new Sys3Controls.Sys3Label();
            this.m_labelZphaseLogic = new Sys3Controls.Sys3Label();
            this.sys3Label7 = new Sys3Controls.Sys3Label();
            this.m_labelOutputScaleCPR = new Sys3Controls.Sys3Label();
            this.m_lblZphaseLogic = new Sys3Controls.Sys3Label();
            this.m_labelInputScaleCPR = new Sys3Controls.Sys3Label();
            this.m_labelMotorOutmode = new Sys3Controls.Sys3Label();
            this.m_lblMotorType = new Sys3Controls.Sys3Label();
            this.m_labelOutputScaleMPR = new Sys3Controls.Sys3Label();
            this.m_labelInputScaleMPR = new Sys3Controls.Sys3Label();
            this.m_lblMotorOutmode = new Sys3Controls.Sys3Label();
            this.m_labelMotorInmode = new Sys3Controls.Sys3Label();
            this.m_labelMotorType = new Sys3Controls.Sys3Label();
            this.m_labelAlarmStopMode = new Sys3Controls.Sys3Label();
            this.m_labelEncoderDirection = new Sys3Controls.Sys3Label();
            this.m_labelAlarmLogic = new Sys3Controls.Sys3Label();
            this.m_lblMotionType = new Sys3Controls.Sys3Label();
            this.m_lblMotorInmode = new Sys3Controls.Sys3Label();
            this.m_lblAlarmStopMode = new Sys3Controls.Sys3Label();
            this.m_labelCommandDirection = new Sys3Controls.Sys3Label();
            this.m_lblAlarmLogic = new Sys3Controls.Sys3Label();
            this.m_labelMotionType = new Sys3Controls.Sys3Label();
            this.m_lblOutputScale = new Sys3Controls.Sys3Label();
            this.m_lblCommandDirection = new Sys3Controls.Sys3Label();
            this.m_lblEncoderDirection = new Sys3Controls.Sys3Label();
            this.m_lblInputScale = new Sys3Controls.Sys3Label();
            this.m_groupSpeedContents.SuspendLayout();
            this._tableLayoutPanel_GantryStatus.SuspendLayout();
            this.SuspendLayout();
            // 
            // m_groupSpeedContents
            // 
            this.m_groupSpeedContents.BackGroundColor = System.Drawing.Color.WhiteSmoke;
            this.m_groupSpeedContents.Controls.Add(this._tableLayoutPanel_GantryStatus);
            this.m_groupSpeedContents.Dock = System.Windows.Forms.DockStyle.Fill;
            this.m_groupSpeedContents.EdgeBorderStroke = 2;
            this.m_groupSpeedContents.EdgeRadius = 2;
            this.m_groupSpeedContents.LabelFont = new System.Drawing.Font("맑은 고딕", 10F, System.Drawing.FontStyle.Bold);
            this.m_groupSpeedContents.LabelGradientColorFirst = System.Drawing.Color.FromArgb(((int)(((byte)(208)))), ((int)(((byte)(208)))), ((int)(((byte)(208)))));
            this.m_groupSpeedContents.LabelGradientColorSecond = System.Drawing.Color.FromArgb(((int)(((byte)(240)))), ((int)(((byte)(240)))), ((int)(((byte)(240)))));
            this.m_groupSpeedContents.LabelHeight = 30;
            this.m_groupSpeedContents.LabelTextColor = System.Drawing.Color.Black;
            this.m_groupSpeedContents.Location = new System.Drawing.Point(0, 0);
            this.m_groupSpeedContents.Name = "m_groupSpeedContents";
            this.m_groupSpeedContents.Padding = new System.Windows.Forms.Padding(3, 18, 3, 3);
            this.m_groupSpeedContents.Size = new System.Drawing.Size(1190, 411);
            this.m_groupSpeedContents.TabIndex = 1372;
            this.m_groupSpeedContents.TabStop = false;
            this.m_groupSpeedContents.Text = "CONFIG";
            this.m_groupSpeedContents.TextAlign = Sys3Controls.EN_TEXTALIGN.MIDDLE_CENTER;
            this.m_groupSpeedContents.ThemeIndex = 0;
            this.m_groupSpeedContents.UseLabelBorder = true;
            this.m_groupSpeedContents.UseTitle = true;
            // 
            // _tableLayoutPanel_GantryStatus
            // 
            this._tableLayoutPanel_GantryStatus.ColumnCount = 7;
            this._tableLayoutPanel_GantryStatus.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 15.51724F));
            this._tableLayoutPanel_GantryStatus.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 17.24138F));
            this._tableLayoutPanel_GantryStatus.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this._tableLayoutPanel_GantryStatus.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 15.51724F));
            this._tableLayoutPanel_GantryStatus.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 17.24138F));
            this._tableLayoutPanel_GantryStatus.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 17.24138F));
            this._tableLayoutPanel_GantryStatus.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 17.24138F));
            this._tableLayoutPanel_GantryStatus.Controls.Add(this.sys3Label8, 5, 0);
            this._tableLayoutPanel_GantryStatus.Controls.Add(this.m_labelZphaseLogic, 4, 8);
            this._tableLayoutPanel_GantryStatus.Controls.Add(this.sys3Label7, 4, 0);
            this._tableLayoutPanel_GantryStatus.Controls.Add(this.m_labelOutputScaleCPR, 5, 2);
            this._tableLayoutPanel_GantryStatus.Controls.Add(this.m_lblZphaseLogic, 3, 8);
            this._tableLayoutPanel_GantryStatus.Controls.Add(this.m_labelInputScaleCPR, 5, 1);
            this._tableLayoutPanel_GantryStatus.Controls.Add(this.m_labelMotorOutmode, 1, 5);
            this._tableLayoutPanel_GantryStatus.Controls.Add(this.m_lblMotorType, 0, 1);
            this._tableLayoutPanel_GantryStatus.Controls.Add(this.m_labelOutputScaleMPR, 4, 2);
            this._tableLayoutPanel_GantryStatus.Controls.Add(this.m_labelInputScaleMPR, 4, 1);
            this._tableLayoutPanel_GantryStatus.Controls.Add(this.m_lblMotorOutmode, 0, 5);
            this._tableLayoutPanel_GantryStatus.Controls.Add(this.m_labelMotorInmode, 1, 4);
            this._tableLayoutPanel_GantryStatus.Controls.Add(this.m_labelMotorType, 1, 1);
            this._tableLayoutPanel_GantryStatus.Controls.Add(this.m_labelAlarmStopMode, 4, 5);
            this._tableLayoutPanel_GantryStatus.Controls.Add(this.m_labelEncoderDirection, 1, 8);
            this._tableLayoutPanel_GantryStatus.Controls.Add(this.m_labelAlarmLogic, 4, 4);
            this._tableLayoutPanel_GantryStatus.Controls.Add(this.m_lblMotionType, 0, 2);
            this._tableLayoutPanel_GantryStatus.Controls.Add(this.m_lblMotorInmode, 0, 4);
            this._tableLayoutPanel_GantryStatus.Controls.Add(this.m_lblAlarmStopMode, 3, 5);
            this._tableLayoutPanel_GantryStatus.Controls.Add(this.m_labelCommandDirection, 1, 7);
            this._tableLayoutPanel_GantryStatus.Controls.Add(this.m_lblAlarmLogic, 3, 4);
            this._tableLayoutPanel_GantryStatus.Controls.Add(this.m_labelMotionType, 1, 2);
            this._tableLayoutPanel_GantryStatus.Controls.Add(this.m_lblOutputScale, 3, 2);
            this._tableLayoutPanel_GantryStatus.Controls.Add(this.m_lblCommandDirection, 0, 7);
            this._tableLayoutPanel_GantryStatus.Controls.Add(this.m_lblEncoderDirection, 0, 8);
            this._tableLayoutPanel_GantryStatus.Controls.Add(this.m_lblInputScale, 3, 1);
            this._tableLayoutPanel_GantryStatus.Dock = System.Windows.Forms.DockStyle.Fill;
            this._tableLayoutPanel_GantryStatus.Location = new System.Drawing.Point(3, 32);
            this._tableLayoutPanel_GantryStatus.Name = "_tableLayoutPanel_GantryStatus";
            this._tableLayoutPanel_GantryStatus.Padding = new System.Windows.Forms.Padding(5);
            this._tableLayoutPanel_GantryStatus.RowCount = 9;
            this._tableLayoutPanel_GantryStatus.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 14.28571F));
            this._tableLayoutPanel_GantryStatus.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 14.28572F));
            this._tableLayoutPanel_GantryStatus.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 14.28572F));
            this._tableLayoutPanel_GantryStatus.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 10F));
            this._tableLayoutPanel_GantryStatus.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 14.28572F));
            this._tableLayoutPanel_GantryStatus.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 14.28572F));
            this._tableLayoutPanel_GantryStatus.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 10F));
            this._tableLayoutPanel_GantryStatus.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 14.28572F));
            this._tableLayoutPanel_GantryStatus.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 14.28572F));
            this._tableLayoutPanel_GantryStatus.Size = new System.Drawing.Size(1184, 376);
            this._tableLayoutPanel_GantryStatus.TabIndex = 1;
            // 
            // sys3Label8
            // 
            this.sys3Label8.BackGroundColor = System.Drawing.Color.FromArgb(((int)(((byte)(200)))), ((int)(((byte)(200)))), ((int)(((byte)(220)))));
            this.sys3Label8.BorderStroke = 2;
            this.sys3Label8.BorderStyle = System.Windows.Forms.ButtonBorderStyle.Solid;
            this.sys3Label8.Description = "";
            this.sys3Label8.DisabledColor = System.Drawing.Color.DarkGray;
            this.sys3Label8.Dock = System.Windows.Forms.DockStyle.Fill;
            this.sys3Label8.EdgeRadius = 1;
            this.sys3Label8.ImagePosition = new System.Drawing.Point(0, 0);
            this.sys3Label8.ImageSize = new System.Drawing.Point(0, 0);
            this.sys3Label8.LoadImage = null;
            this.sys3Label8.Location = new System.Drawing.Point(781, 8);
            this.sys3Label8.MainFont = new System.Drawing.Font("맑은 고딕", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.sys3Label8.MainFontColor = System.Drawing.Color.DarkBlue;
            this.sys3Label8.Margin = new System.Windows.Forms.Padding(2, 3, 0, 0);
            this.sys3Label8.Name = "sys3Label8";
            this.sys3Label8.Size = new System.Drawing.Size(196, 46);
            this.sys3Label8.SubFont = new System.Drawing.Font("맑은 고딕", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.sys3Label8.SubFontColor = System.Drawing.Color.DarkBlue;
            this.sys3Label8.SubText = "MM PER REVOLUTION \\n\\n\\n COUNT PER REVOLUTION";
            this.sys3Label8.TabIndex = 1388;
            this.sys3Label8.Text = "COUNT PER REVOLUTION";
            this.sys3Label8.TextAlignMain = Sys3Controls.EN_TEXTALIGN.TOP_LEFT;
            this.sys3Label8.TextAlignSub = Sys3Controls.EN_TEXTALIGN.BOTTOM_RIGHT;
            this.sys3Label8.TextAlignUnit = Sys3Controls.EN_TEXTALIGN.MIDDLE_CENTER;
            this.sys3Label8.ThemeIndex = 0;
            this.sys3Label8.UnitAreaRate = 40;
            this.sys3Label8.UnitFont = new System.Drawing.Font("맑은 고딕", 10F, System.Drawing.FontStyle.Bold);
            this.sys3Label8.UnitFontColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.sys3Label8.UnitPositionVertical = false;
            this.sys3Label8.UnitText = "";
            this.sys3Label8.UseBorder = true;
            this.sys3Label8.UseEdgeRadius = false;
            this.sys3Label8.UseImage = false;
            this.sys3Label8.UseSubFont = false;
            this.sys3Label8.UseUnitFont = false;
            // 
            // m_labelZphaseLogic
            // 
            this.m_labelZphaseLogic.BackGroundColor = System.Drawing.Color.White;
            this.m_labelZphaseLogic.BorderStroke = 2;
            this.m_labelZphaseLogic.BorderStyle = System.Windows.Forms.ButtonBorderStyle.Solid;
            this.m_labelZphaseLogic.Description = "";
            this.m_labelZphaseLogic.DisabledColor = System.Drawing.Color.LightGray;
            this.m_labelZphaseLogic.Dock = System.Windows.Forms.DockStyle.Fill;
            this.m_labelZphaseLogic.EdgeRadius = 1;
            this.m_labelZphaseLogic.Enabled = false;
            this.m_labelZphaseLogic.ImagePosition = new System.Drawing.Point(0, 0);
            this.m_labelZphaseLogic.ImageSize = new System.Drawing.Point(0, 0);
            this.m_labelZphaseLogic.LoadImage = null;
            this.m_labelZphaseLogic.Location = new System.Drawing.Point(583, 322);
            this.m_labelZphaseLogic.MainFont = new System.Drawing.Font("맑은 고딕", 11F, System.Drawing.FontStyle.Bold);
            this.m_labelZphaseLogic.MainFontColor = System.Drawing.Color.Black;
            this.m_labelZphaseLogic.Margin = new System.Windows.Forms.Padding(2, 3, 0, 3);
            this.m_labelZphaseLogic.Name = "m_labelZphaseLogic";
            this.m_labelZphaseLogic.Size = new System.Drawing.Size(196, 46);
            this.m_labelZphaseLogic.SubFont = new System.Drawing.Font("맑은 고딕", 10F);
            this.m_labelZphaseLogic.SubFontColor = System.Drawing.Color.Black;
            this.m_labelZphaseLogic.SubText = "";
            this.m_labelZphaseLogic.TabIndex = 6;
            this.m_labelZphaseLogic.Text = "--";
            this.m_labelZphaseLogic.TextAlignMain = Sys3Controls.EN_TEXTALIGN.BOTTOM_RIGHT;
            this.m_labelZphaseLogic.TextAlignSub = Sys3Controls.EN_TEXTALIGN.MIDDLE_CENTER;
            this.m_labelZphaseLogic.TextAlignUnit = Sys3Controls.EN_TEXTALIGN.MIDDLE_CENTER;
            this.m_labelZphaseLogic.ThemeIndex = 0;
            this.m_labelZphaseLogic.UnitAreaRate = 40;
            this.m_labelZphaseLogic.UnitFont = new System.Drawing.Font("맑은 고딕", 10F, System.Drawing.FontStyle.Bold);
            this.m_labelZphaseLogic.UnitFontColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.m_labelZphaseLogic.UnitPositionVertical = false;
            this.m_labelZphaseLogic.UnitText = "";
            this.m_labelZphaseLogic.UseBorder = true;
            this.m_labelZphaseLogic.UseEdgeRadius = false;
            this.m_labelZphaseLogic.UseImage = false;
            this.m_labelZphaseLogic.UseSubFont = false;
            this.m_labelZphaseLogic.UseUnitFont = false;
            this.m_labelZphaseLogic.Click += new System.EventHandler(this.Click_ConfigLabel);
            // 
            // sys3Label7
            // 
            this.sys3Label7.BackGroundColor = System.Drawing.Color.FromArgb(((int)(((byte)(200)))), ((int)(((byte)(200)))), ((int)(((byte)(220)))));
            this.sys3Label7.BorderStroke = 2;
            this.sys3Label7.BorderStyle = System.Windows.Forms.ButtonBorderStyle.Solid;
            this.sys3Label7.Description = "";
            this.sys3Label7.DisabledColor = System.Drawing.Color.DarkGray;
            this.sys3Label7.Dock = System.Windows.Forms.DockStyle.Fill;
            this.sys3Label7.EdgeRadius = 1;
            this.sys3Label7.ImagePosition = new System.Drawing.Point(0, 0);
            this.sys3Label7.ImageSize = new System.Drawing.Point(0, 0);
            this.sys3Label7.LoadImage = null;
            this.sys3Label7.Location = new System.Drawing.Point(583, 8);
            this.sys3Label7.MainFont = new System.Drawing.Font("맑은 고딕", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.sys3Label7.MainFontColor = System.Drawing.Color.DarkBlue;
            this.sys3Label7.Margin = new System.Windows.Forms.Padding(2, 3, 0, 0);
            this.sys3Label7.Name = "sys3Label7";
            this.sys3Label7.Size = new System.Drawing.Size(196, 46);
            this.sys3Label7.SubFont = new System.Drawing.Font("맑은 고딕", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.sys3Label7.SubFontColor = System.Drawing.Color.DarkBlue;
            this.sys3Label7.SubText = "MM PER REVOLUTION \\n\\n\\n COUNT PER REVOLUTION";
            this.sys3Label7.TabIndex = 1388;
            this.sys3Label7.Text = "MM PER REVOLUTION";
            this.sys3Label7.TextAlignMain = Sys3Controls.EN_TEXTALIGN.TOP_LEFT;
            this.sys3Label7.TextAlignSub = Sys3Controls.EN_TEXTALIGN.BOTTOM_RIGHT;
            this.sys3Label7.TextAlignUnit = Sys3Controls.EN_TEXTALIGN.MIDDLE_CENTER;
            this.sys3Label7.ThemeIndex = 0;
            this.sys3Label7.UnitAreaRate = 40;
            this.sys3Label7.UnitFont = new System.Drawing.Font("맑은 고딕", 10F, System.Drawing.FontStyle.Bold);
            this.sys3Label7.UnitFontColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.sys3Label7.UnitPositionVertical = false;
            this.sys3Label7.UnitText = "";
            this.sys3Label7.UseBorder = true;
            this.sys3Label7.UseEdgeRadius = false;
            this.sys3Label7.UseImage = false;
            this.sys3Label7.UseSubFont = false;
            this.sys3Label7.UseUnitFont = false;
            // 
            // m_labelOutputScaleCPR
            // 
            this.m_labelOutputScaleCPR.BackGroundColor = System.Drawing.Color.White;
            this.m_labelOutputScaleCPR.BorderStroke = 2;
            this.m_labelOutputScaleCPR.BorderStyle = System.Windows.Forms.ButtonBorderStyle.Solid;
            this.m_labelOutputScaleCPR.Description = "";
            this.m_labelOutputScaleCPR.DisabledColor = System.Drawing.Color.LightGray;
            this.m_labelOutputScaleCPR.Dock = System.Windows.Forms.DockStyle.Fill;
            this.m_labelOutputScaleCPR.EdgeRadius = 1;
            this.m_labelOutputScaleCPR.Enabled = false;
            this.m_labelOutputScaleCPR.ImagePosition = new System.Drawing.Point(0, 0);
            this.m_labelOutputScaleCPR.ImageSize = new System.Drawing.Point(0, 0);
            this.m_labelOutputScaleCPR.LoadImage = null;
            this.m_labelOutputScaleCPR.Location = new System.Drawing.Point(781, 106);
            this.m_labelOutputScaleCPR.MainFont = new System.Drawing.Font("맑은 고딕", 11F, System.Drawing.FontStyle.Bold);
            this.m_labelOutputScaleCPR.MainFontColor = System.Drawing.Color.Black;
            this.m_labelOutputScaleCPR.Margin = new System.Windows.Forms.Padding(2, 3, 0, 3);
            this.m_labelOutputScaleCPR.Name = "m_labelOutputScaleCPR";
            this.m_labelOutputScaleCPR.Size = new System.Drawing.Size(196, 43);
            this.m_labelOutputScaleCPR.SubFont = new System.Drawing.Font("맑은 고딕", 10F);
            this.m_labelOutputScaleCPR.SubFontColor = System.Drawing.Color.Black;
            this.m_labelOutputScaleCPR.SubText = "";
            this.m_labelOutputScaleCPR.TabIndex = 3;
            this.m_labelOutputScaleCPR.Text = "--";
            this.m_labelOutputScaleCPR.TextAlignMain = Sys3Controls.EN_TEXTALIGN.BOTTOM_RIGHT;
            this.m_labelOutputScaleCPR.TextAlignSub = Sys3Controls.EN_TEXTALIGN.MIDDLE_CENTER;
            this.m_labelOutputScaleCPR.TextAlignUnit = Sys3Controls.EN_TEXTALIGN.BOTTOM_RIGHT;
            this.m_labelOutputScaleCPR.ThemeIndex = 0;
            this.m_labelOutputScaleCPR.UnitAreaRate = 40;
            this.m_labelOutputScaleCPR.UnitFont = new System.Drawing.Font("맑은 고딕", 10F, System.Drawing.FontStyle.Bold);
            this.m_labelOutputScaleCPR.UnitFontColor = System.Drawing.Color.Black;
            this.m_labelOutputScaleCPR.UnitPositionVertical = false;
            this.m_labelOutputScaleCPR.UnitText = "count";
            this.m_labelOutputScaleCPR.UseBorder = true;
            this.m_labelOutputScaleCPR.UseEdgeRadius = false;
            this.m_labelOutputScaleCPR.UseImage = false;
            this.m_labelOutputScaleCPR.UseSubFont = false;
            this.m_labelOutputScaleCPR.UseUnitFont = true;
            this.m_labelOutputScaleCPR.Click += new System.EventHandler(this.Click_ScaleConfig);
            // 
            // m_lblZphaseLogic
            // 
            this.m_lblZphaseLogic.BackGroundColor = System.Drawing.Color.FromArgb(((int)(((byte)(200)))), ((int)(((byte)(200)))), ((int)(((byte)(220)))));
            this.m_lblZphaseLogic.BorderStroke = 2;
            this.m_lblZphaseLogic.BorderStyle = System.Windows.Forms.ButtonBorderStyle.Solid;
            this.m_lblZphaseLogic.Description = "";
            this.m_lblZphaseLogic.DisabledColor = System.Drawing.Color.DarkGray;
            this.m_lblZphaseLogic.Dock = System.Windows.Forms.DockStyle.Fill;
            this.m_lblZphaseLogic.EdgeRadius = 1;
            this.m_lblZphaseLogic.ImagePosition = new System.Drawing.Point(0, 0);
            this.m_lblZphaseLogic.ImageSize = new System.Drawing.Point(0, 0);
            this.m_lblZphaseLogic.LoadImage = null;
            this.m_lblZphaseLogic.Location = new System.Drawing.Point(402, 322);
            this.m_lblZphaseLogic.MainFont = new System.Drawing.Font("맑은 고딕", 9F, System.Drawing.FontStyle.Bold);
            this.m_lblZphaseLogic.MainFontColor = System.Drawing.Color.Black;
            this.m_lblZphaseLogic.Margin = new System.Windows.Forms.Padding(0, 3, 0, 3);
            this.m_lblZphaseLogic.Name = "m_lblZphaseLogic";
            this.m_lblZphaseLogic.Size = new System.Drawing.Size(179, 46);
            this.m_lblZphaseLogic.SubFont = new System.Drawing.Font("맑은 고딕", 9F, System.Drawing.FontStyle.Bold);
            this.m_lblZphaseLogic.SubFontColor = System.Drawing.Color.DarkBlue;
            this.m_lblZphaseLogic.SubText = "ENCODER";
            this.m_lblZphaseLogic.TabIndex = 1388;
            this.m_lblZphaseLogic.Text = "ZPHASE LOGIC";
            this.m_lblZphaseLogic.TextAlignMain = Sys3Controls.EN_TEXTALIGN.TOP_LEFT;
            this.m_lblZphaseLogic.TextAlignSub = Sys3Controls.EN_TEXTALIGN.BOTTOM_RIGHT;
            this.m_lblZphaseLogic.TextAlignUnit = Sys3Controls.EN_TEXTALIGN.MIDDLE_CENTER;
            this.m_lblZphaseLogic.ThemeIndex = 0;
            this.m_lblZphaseLogic.UnitAreaRate = 40;
            this.m_lblZphaseLogic.UnitFont = new System.Drawing.Font("맑은 고딕", 10F, System.Drawing.FontStyle.Bold);
            this.m_lblZphaseLogic.UnitFontColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.m_lblZphaseLogic.UnitPositionVertical = false;
            this.m_lblZphaseLogic.UnitText = "";
            this.m_lblZphaseLogic.UseBorder = true;
            this.m_lblZphaseLogic.UseEdgeRadius = false;
            this.m_lblZphaseLogic.UseImage = false;
            this.m_lblZphaseLogic.UseSubFont = false;
            this.m_lblZphaseLogic.UseUnitFont = false;
            // 
            // m_labelInputScaleCPR
            // 
            this.m_labelInputScaleCPR.BackGroundColor = System.Drawing.Color.White;
            this.m_labelInputScaleCPR.BorderStroke = 2;
            this.m_labelInputScaleCPR.BorderStyle = System.Windows.Forms.ButtonBorderStyle.Solid;
            this.m_labelInputScaleCPR.Description = "";
            this.m_labelInputScaleCPR.DisabledColor = System.Drawing.Color.LightGray;
            this.m_labelInputScaleCPR.Dock = System.Windows.Forms.DockStyle.Fill;
            this.m_labelInputScaleCPR.EdgeRadius = 1;
            this.m_labelInputScaleCPR.Enabled = false;
            this.m_labelInputScaleCPR.ImagePosition = new System.Drawing.Point(0, 0);
            this.m_labelInputScaleCPR.ImageSize = new System.Drawing.Point(0, 0);
            this.m_labelInputScaleCPR.LoadImage = null;
            this.m_labelInputScaleCPR.Location = new System.Drawing.Point(781, 57);
            this.m_labelInputScaleCPR.MainFont = new System.Drawing.Font("맑은 고딕", 11F, System.Drawing.FontStyle.Bold);
            this.m_labelInputScaleCPR.MainFontColor = System.Drawing.Color.Black;
            this.m_labelInputScaleCPR.Margin = new System.Windows.Forms.Padding(2, 3, 0, 3);
            this.m_labelInputScaleCPR.Name = "m_labelInputScaleCPR";
            this.m_labelInputScaleCPR.Size = new System.Drawing.Size(196, 43);
            this.m_labelInputScaleCPR.SubFont = new System.Drawing.Font("맑은 고딕", 10F);
            this.m_labelInputScaleCPR.SubFontColor = System.Drawing.Color.Black;
            this.m_labelInputScaleCPR.SubText = "";
            this.m_labelInputScaleCPR.TabIndex = 1;
            this.m_labelInputScaleCPR.Text = "--";
            this.m_labelInputScaleCPR.TextAlignMain = Sys3Controls.EN_TEXTALIGN.BOTTOM_RIGHT;
            this.m_labelInputScaleCPR.TextAlignSub = Sys3Controls.EN_TEXTALIGN.MIDDLE_CENTER;
            this.m_labelInputScaleCPR.TextAlignUnit = Sys3Controls.EN_TEXTALIGN.BOTTOM_RIGHT;
            this.m_labelInputScaleCPR.ThemeIndex = 0;
            this.m_labelInputScaleCPR.UnitAreaRate = 40;
            this.m_labelInputScaleCPR.UnitFont = new System.Drawing.Font("맑은 고딕", 10F, System.Drawing.FontStyle.Bold);
            this.m_labelInputScaleCPR.UnitFontColor = System.Drawing.Color.Black;
            this.m_labelInputScaleCPR.UnitPositionVertical = false;
            this.m_labelInputScaleCPR.UnitText = "count";
            this.m_labelInputScaleCPR.UseBorder = true;
            this.m_labelInputScaleCPR.UseEdgeRadius = false;
            this.m_labelInputScaleCPR.UseImage = false;
            this.m_labelInputScaleCPR.UseSubFont = false;
            this.m_labelInputScaleCPR.UseUnitFont = true;
            this.m_labelInputScaleCPR.Click += new System.EventHandler(this.Click_ScaleConfig);
            // 
            // m_labelMotorOutmode
            // 
            this.m_labelMotorOutmode.BackGroundColor = System.Drawing.Color.White;
            this.m_labelMotorOutmode.BorderStroke = 2;
            this.m_labelMotorOutmode.BorderStyle = System.Windows.Forms.ButtonBorderStyle.Solid;
            this.m_labelMotorOutmode.Description = "";
            this.m_labelMotorOutmode.DisabledColor = System.Drawing.Color.LightGray;
            this.m_labelMotorOutmode.Dock = System.Windows.Forms.DockStyle.Fill;
            this.m_labelMotorOutmode.EdgeRadius = 1;
            this.m_labelMotorOutmode.Enabled = false;
            this.m_labelMotorOutmode.ImagePosition = new System.Drawing.Point(0, 0);
            this.m_labelMotorOutmode.ImageSize = new System.Drawing.Point(0, 0);
            this.m_labelMotorOutmode.LoadImage = null;
            this.m_labelMotorOutmode.Location = new System.Drawing.Point(186, 214);
            this.m_labelMotorOutmode.MainFont = new System.Drawing.Font("맑은 고딕", 11F, System.Drawing.FontStyle.Bold);
            this.m_labelMotorOutmode.MainFontColor = System.Drawing.Color.Black;
            this.m_labelMotorOutmode.Margin = new System.Windows.Forms.Padding(2, 3, 0, 3);
            this.m_labelMotorOutmode.Name = "m_labelMotorOutmode";
            this.m_labelMotorOutmode.Size = new System.Drawing.Size(196, 43);
            this.m_labelMotorOutmode.SubFont = new System.Drawing.Font("맑은 고딕", 10F);
            this.m_labelMotorOutmode.SubFontColor = System.Drawing.Color.Black;
            this.m_labelMotorOutmode.SubText = "";
            this.m_labelMotorOutmode.TabIndex = 8;
            this.m_labelMotorOutmode.Text = "--";
            this.m_labelMotorOutmode.TextAlignMain = Sys3Controls.EN_TEXTALIGN.BOTTOM_RIGHT;
            this.m_labelMotorOutmode.TextAlignSub = Sys3Controls.EN_TEXTALIGN.MIDDLE_CENTER;
            this.m_labelMotorOutmode.TextAlignUnit = Sys3Controls.EN_TEXTALIGN.MIDDLE_CENTER;
            this.m_labelMotorOutmode.ThemeIndex = 0;
            this.m_labelMotorOutmode.UnitAreaRate = 40;
            this.m_labelMotorOutmode.UnitFont = new System.Drawing.Font("맑은 고딕", 10F, System.Drawing.FontStyle.Bold);
            this.m_labelMotorOutmode.UnitFontColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.m_labelMotorOutmode.UnitPositionVertical = false;
            this.m_labelMotorOutmode.UnitText = "";
            this.m_labelMotorOutmode.UseBorder = true;
            this.m_labelMotorOutmode.UseEdgeRadius = false;
            this.m_labelMotorOutmode.UseImage = false;
            this.m_labelMotorOutmode.UseSubFont = false;
            this.m_labelMotorOutmode.UseUnitFont = false;
            this.m_labelMotorOutmode.Click += new System.EventHandler(this.Click_ConfigLabel);
            // 
            // m_lblMotorType
            // 
            this.m_lblMotorType.BackGroundColor = System.Drawing.Color.FromArgb(((int)(((byte)(200)))), ((int)(((byte)(200)))), ((int)(((byte)(220)))));
            this.m_lblMotorType.BorderStroke = 2;
            this.m_lblMotorType.BorderStyle = System.Windows.Forms.ButtonBorderStyle.Solid;
            this.m_lblMotorType.Description = "";
            this.m_lblMotorType.DisabledColor = System.Drawing.Color.DarkGray;
            this.m_lblMotorType.Dock = System.Windows.Forms.DockStyle.Fill;
            this.m_lblMotorType.EdgeRadius = 1;
            this.m_lblMotorType.ImagePosition = new System.Drawing.Point(0, 0);
            this.m_lblMotorType.ImageSize = new System.Drawing.Point(0, 0);
            this.m_lblMotorType.LoadImage = null;
            this.m_lblMotorType.Location = new System.Drawing.Point(5, 57);
            this.m_lblMotorType.MainFont = new System.Drawing.Font("맑은 고딕", 9F, System.Drawing.FontStyle.Bold);
            this.m_lblMotorType.MainFontColor = System.Drawing.Color.Black;
            this.m_lblMotorType.Margin = new System.Windows.Forms.Padding(0, 3, 0, 3);
            this.m_lblMotorType.Name = "m_lblMotorType";
            this.m_lblMotorType.Size = new System.Drawing.Size(179, 43);
            this.m_lblMotorType.SubFont = new System.Drawing.Font("맑은 고딕", 10F);
            this.m_lblMotorType.SubFontColor = System.Drawing.Color.Black;
            this.m_lblMotorType.SubText = "";
            this.m_lblMotorType.TabIndex = 1387;
            this.m_lblMotorType.Text = "MOTOR TYPE";
            this.m_lblMotorType.TextAlignMain = Sys3Controls.EN_TEXTALIGN.TOP_LEFT;
            this.m_lblMotorType.TextAlignSub = Sys3Controls.EN_TEXTALIGN.MIDDLE_CENTER;
            this.m_lblMotorType.TextAlignUnit = Sys3Controls.EN_TEXTALIGN.MIDDLE_CENTER;
            this.m_lblMotorType.ThemeIndex = 0;
            this.m_lblMotorType.UnitAreaRate = 40;
            this.m_lblMotorType.UnitFont = new System.Drawing.Font("맑은 고딕", 10F, System.Drawing.FontStyle.Bold);
            this.m_lblMotorType.UnitFontColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.m_lblMotorType.UnitPositionVertical = false;
            this.m_lblMotorType.UnitText = "";
            this.m_lblMotorType.UseBorder = true;
            this.m_lblMotorType.UseEdgeRadius = false;
            this.m_lblMotorType.UseImage = false;
            this.m_lblMotorType.UseSubFont = false;
            this.m_lblMotorType.UseUnitFont = false;
            // 
            // m_labelOutputScaleMPR
            // 
            this.m_labelOutputScaleMPR.BackGroundColor = System.Drawing.Color.White;
            this.m_labelOutputScaleMPR.BorderStroke = 2;
            this.m_labelOutputScaleMPR.BorderStyle = System.Windows.Forms.ButtonBorderStyle.Solid;
            this.m_labelOutputScaleMPR.Description = "";
            this.m_labelOutputScaleMPR.DisabledColor = System.Drawing.Color.LightGray;
            this.m_labelOutputScaleMPR.Dock = System.Windows.Forms.DockStyle.Fill;
            this.m_labelOutputScaleMPR.EdgeRadius = 1;
            this.m_labelOutputScaleMPR.Enabled = false;
            this.m_labelOutputScaleMPR.ImagePosition = new System.Drawing.Point(0, 0);
            this.m_labelOutputScaleMPR.ImageSize = new System.Drawing.Point(0, 0);
            this.m_labelOutputScaleMPR.LoadImage = null;
            this.m_labelOutputScaleMPR.Location = new System.Drawing.Point(583, 106);
            this.m_labelOutputScaleMPR.MainFont = new System.Drawing.Font("맑은 고딕", 11F, System.Drawing.FontStyle.Bold);
            this.m_labelOutputScaleMPR.MainFontColor = System.Drawing.Color.Black;
            this.m_labelOutputScaleMPR.Margin = new System.Windows.Forms.Padding(2, 3, 0, 3);
            this.m_labelOutputScaleMPR.Name = "m_labelOutputScaleMPR";
            this.m_labelOutputScaleMPR.Size = new System.Drawing.Size(196, 43);
            this.m_labelOutputScaleMPR.SubFont = new System.Drawing.Font("맑은 고딕", 10F);
            this.m_labelOutputScaleMPR.SubFontColor = System.Drawing.Color.Black;
            this.m_labelOutputScaleMPR.SubText = "";
            this.m_labelOutputScaleMPR.TabIndex = 2;
            this.m_labelOutputScaleMPR.Text = "--";
            this.m_labelOutputScaleMPR.TextAlignMain = Sys3Controls.EN_TEXTALIGN.BOTTOM_RIGHT;
            this.m_labelOutputScaleMPR.TextAlignSub = Sys3Controls.EN_TEXTALIGN.MIDDLE_CENTER;
            this.m_labelOutputScaleMPR.TextAlignUnit = Sys3Controls.EN_TEXTALIGN.BOTTOM_RIGHT;
            this.m_labelOutputScaleMPR.ThemeIndex = 0;
            this.m_labelOutputScaleMPR.UnitAreaRate = 40;
            this.m_labelOutputScaleMPR.UnitFont = new System.Drawing.Font("맑은 고딕", 10F, System.Drawing.FontStyle.Bold);
            this.m_labelOutputScaleMPR.UnitFontColor = System.Drawing.Color.Black;
            this.m_labelOutputScaleMPR.UnitPositionVertical = false;
            this.m_labelOutputScaleMPR.UnitText = "mm";
            this.m_labelOutputScaleMPR.UseBorder = true;
            this.m_labelOutputScaleMPR.UseEdgeRadius = false;
            this.m_labelOutputScaleMPR.UseImage = false;
            this.m_labelOutputScaleMPR.UseSubFont = false;
            this.m_labelOutputScaleMPR.UseUnitFont = true;
            this.m_labelOutputScaleMPR.Click += new System.EventHandler(this.Click_ScaleConfig);
            // 
            // m_labelInputScaleMPR
            // 
            this.m_labelInputScaleMPR.BackGroundColor = System.Drawing.Color.White;
            this.m_labelInputScaleMPR.BorderStroke = 2;
            this.m_labelInputScaleMPR.BorderStyle = System.Windows.Forms.ButtonBorderStyle.Solid;
            this.m_labelInputScaleMPR.Description = "";
            this.m_labelInputScaleMPR.DisabledColor = System.Drawing.Color.LightGray;
            this.m_labelInputScaleMPR.Dock = System.Windows.Forms.DockStyle.Fill;
            this.m_labelInputScaleMPR.EdgeRadius = 1;
            this.m_labelInputScaleMPR.Enabled = false;
            this.m_labelInputScaleMPR.ImagePosition = new System.Drawing.Point(0, 0);
            this.m_labelInputScaleMPR.ImageSize = new System.Drawing.Point(0, 0);
            this.m_labelInputScaleMPR.LoadImage = null;
            this.m_labelInputScaleMPR.Location = new System.Drawing.Point(583, 57);
            this.m_labelInputScaleMPR.MainFont = new System.Drawing.Font("맑은 고딕", 11F, System.Drawing.FontStyle.Bold);
            this.m_labelInputScaleMPR.MainFontColor = System.Drawing.Color.Black;
            this.m_labelInputScaleMPR.Margin = new System.Windows.Forms.Padding(2, 3, 0, 3);
            this.m_labelInputScaleMPR.Name = "m_labelInputScaleMPR";
            this.m_labelInputScaleMPR.Size = new System.Drawing.Size(196, 43);
            this.m_labelInputScaleMPR.SubFont = new System.Drawing.Font("맑은 고딕", 10F);
            this.m_labelInputScaleMPR.SubFontColor = System.Drawing.Color.Black;
            this.m_labelInputScaleMPR.SubText = "";
            this.m_labelInputScaleMPR.TabIndex = 0;
            this.m_labelInputScaleMPR.Text = "--";
            this.m_labelInputScaleMPR.TextAlignMain = Sys3Controls.EN_TEXTALIGN.BOTTOM_RIGHT;
            this.m_labelInputScaleMPR.TextAlignSub = Sys3Controls.EN_TEXTALIGN.MIDDLE_CENTER;
            this.m_labelInputScaleMPR.TextAlignUnit = Sys3Controls.EN_TEXTALIGN.BOTTOM_RIGHT;
            this.m_labelInputScaleMPR.ThemeIndex = 0;
            this.m_labelInputScaleMPR.UnitAreaRate = 40;
            this.m_labelInputScaleMPR.UnitFont = new System.Drawing.Font("맑은 고딕", 10F, System.Drawing.FontStyle.Bold);
            this.m_labelInputScaleMPR.UnitFontColor = System.Drawing.Color.Black;
            this.m_labelInputScaleMPR.UnitPositionVertical = false;
            this.m_labelInputScaleMPR.UnitText = "mm";
            this.m_labelInputScaleMPR.UseBorder = true;
            this.m_labelInputScaleMPR.UseEdgeRadius = false;
            this.m_labelInputScaleMPR.UseImage = false;
            this.m_labelInputScaleMPR.UseSubFont = false;
            this.m_labelInputScaleMPR.UseUnitFont = true;
            this.m_labelInputScaleMPR.Click += new System.EventHandler(this.Click_ScaleConfig);
            // 
            // m_lblMotorOutmode
            // 
            this.m_lblMotorOutmode.BackGroundColor = System.Drawing.Color.FromArgb(((int)(((byte)(200)))), ((int)(((byte)(200)))), ((int)(((byte)(220)))));
            this.m_lblMotorOutmode.BorderStroke = 2;
            this.m_lblMotorOutmode.BorderStyle = System.Windows.Forms.ButtonBorderStyle.Solid;
            this.m_lblMotorOutmode.Description = "";
            this.m_lblMotorOutmode.DisabledColor = System.Drawing.Color.DarkGray;
            this.m_lblMotorOutmode.Dock = System.Windows.Forms.DockStyle.Fill;
            this.m_lblMotorOutmode.EdgeRadius = 1;
            this.m_lblMotorOutmode.ImagePosition = new System.Drawing.Point(0, 0);
            this.m_lblMotorOutmode.ImageSize = new System.Drawing.Point(0, 0);
            this.m_lblMotorOutmode.LoadImage = null;
            this.m_lblMotorOutmode.Location = new System.Drawing.Point(5, 214);
            this.m_lblMotorOutmode.MainFont = new System.Drawing.Font("맑은 고딕", 9F, System.Drawing.FontStyle.Bold);
            this.m_lblMotorOutmode.MainFontColor = System.Drawing.Color.Black;
            this.m_lblMotorOutmode.Margin = new System.Windows.Forms.Padding(0, 3, 0, 3);
            this.m_lblMotorOutmode.Name = "m_lblMotorOutmode";
            this.m_lblMotorOutmode.Size = new System.Drawing.Size(179, 43);
            this.m_lblMotorOutmode.SubFont = new System.Drawing.Font("맑은 고딕", 10F);
            this.m_lblMotorOutmode.SubFontColor = System.Drawing.Color.Black;
            this.m_lblMotorOutmode.SubText = "";
            this.m_lblMotorOutmode.TabIndex = 1391;
            this.m_lblMotorOutmode.Text = "MOTOR OUTMODE";
            this.m_lblMotorOutmode.TextAlignMain = Sys3Controls.EN_TEXTALIGN.TOP_LEFT;
            this.m_lblMotorOutmode.TextAlignSub = Sys3Controls.EN_TEXTALIGN.MIDDLE_CENTER;
            this.m_lblMotorOutmode.TextAlignUnit = Sys3Controls.EN_TEXTALIGN.MIDDLE_CENTER;
            this.m_lblMotorOutmode.ThemeIndex = 0;
            this.m_lblMotorOutmode.UnitAreaRate = 40;
            this.m_lblMotorOutmode.UnitFont = new System.Drawing.Font("맑은 고딕", 10F, System.Drawing.FontStyle.Bold);
            this.m_lblMotorOutmode.UnitFontColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.m_lblMotorOutmode.UnitPositionVertical = false;
            this.m_lblMotorOutmode.UnitText = "";
            this.m_lblMotorOutmode.UseBorder = true;
            this.m_lblMotorOutmode.UseEdgeRadius = false;
            this.m_lblMotorOutmode.UseImage = false;
            this.m_lblMotorOutmode.UseSubFont = false;
            this.m_lblMotorOutmode.UseUnitFont = false;
            // 
            // m_labelMotorInmode
            // 
            this.m_labelMotorInmode.BackGroundColor = System.Drawing.Color.White;
            this.m_labelMotorInmode.BorderStroke = 2;
            this.m_labelMotorInmode.BorderStyle = System.Windows.Forms.ButtonBorderStyle.Solid;
            this.m_labelMotorInmode.Description = "";
            this.m_labelMotorInmode.DisabledColor = System.Drawing.Color.LightGray;
            this.m_labelMotorInmode.Dock = System.Windows.Forms.DockStyle.Fill;
            this.m_labelMotorInmode.EdgeRadius = 1;
            this.m_labelMotorInmode.Enabled = false;
            this.m_labelMotorInmode.ImagePosition = new System.Drawing.Point(0, 0);
            this.m_labelMotorInmode.ImageSize = new System.Drawing.Point(0, 0);
            this.m_labelMotorInmode.LoadImage = null;
            this.m_labelMotorInmode.Location = new System.Drawing.Point(186, 165);
            this.m_labelMotorInmode.MainFont = new System.Drawing.Font("맑은 고딕", 11F, System.Drawing.FontStyle.Bold);
            this.m_labelMotorInmode.MainFontColor = System.Drawing.Color.Black;
            this.m_labelMotorInmode.Margin = new System.Windows.Forms.Padding(2, 3, 0, 3);
            this.m_labelMotorInmode.Name = "m_labelMotorInmode";
            this.m_labelMotorInmode.Size = new System.Drawing.Size(196, 43);
            this.m_labelMotorInmode.SubFont = new System.Drawing.Font("맑은 고딕", 10F);
            this.m_labelMotorInmode.SubFontColor = System.Drawing.Color.Black;
            this.m_labelMotorInmode.SubText = "";
            this.m_labelMotorInmode.TabIndex = 7;
            this.m_labelMotorInmode.Text = "--";
            this.m_labelMotorInmode.TextAlignMain = Sys3Controls.EN_TEXTALIGN.BOTTOM_RIGHT;
            this.m_labelMotorInmode.TextAlignSub = Sys3Controls.EN_TEXTALIGN.MIDDLE_CENTER;
            this.m_labelMotorInmode.TextAlignUnit = Sys3Controls.EN_TEXTALIGN.MIDDLE_CENTER;
            this.m_labelMotorInmode.ThemeIndex = 0;
            this.m_labelMotorInmode.UnitAreaRate = 40;
            this.m_labelMotorInmode.UnitFont = new System.Drawing.Font("맑은 고딕", 10F, System.Drawing.FontStyle.Bold);
            this.m_labelMotorInmode.UnitFontColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.m_labelMotorInmode.UnitPositionVertical = false;
            this.m_labelMotorInmode.UnitText = "";
            this.m_labelMotorInmode.UseBorder = true;
            this.m_labelMotorInmode.UseEdgeRadius = false;
            this.m_labelMotorInmode.UseImage = false;
            this.m_labelMotorInmode.UseSubFont = false;
            this.m_labelMotorInmode.UseUnitFont = false;
            this.m_labelMotorInmode.Click += new System.EventHandler(this.Click_ConfigLabel);
            // 
            // m_labelMotorType
            // 
            this.m_labelMotorType.BackGroundColor = System.Drawing.Color.White;
            this.m_labelMotorType.BorderStroke = 2;
            this.m_labelMotorType.BorderStyle = System.Windows.Forms.ButtonBorderStyle.Solid;
            this.m_labelMotorType.Description = "";
            this.m_labelMotorType.DisabledColor = System.Drawing.Color.LightGray;
            this.m_labelMotorType.Dock = System.Windows.Forms.DockStyle.Fill;
            this.m_labelMotorType.EdgeRadius = 1;
            this.m_labelMotorType.Enabled = false;
            this.m_labelMotorType.ImagePosition = new System.Drawing.Point(0, 0);
            this.m_labelMotorType.ImageSize = new System.Drawing.Point(0, 0);
            this.m_labelMotorType.LoadImage = null;
            this.m_labelMotorType.Location = new System.Drawing.Point(186, 57);
            this.m_labelMotorType.MainFont = new System.Drawing.Font("맑은 고딕", 11F, System.Drawing.FontStyle.Bold);
            this.m_labelMotorType.MainFontColor = System.Drawing.Color.Black;
            this.m_labelMotorType.Margin = new System.Windows.Forms.Padding(2, 3, 0, 3);
            this.m_labelMotorType.Name = "m_labelMotorType";
            this.m_labelMotorType.Size = new System.Drawing.Size(196, 43);
            this.m_labelMotorType.SubFont = new System.Drawing.Font("맑은 고딕", 10F);
            this.m_labelMotorType.SubFontColor = System.Drawing.Color.Black;
            this.m_labelMotorType.SubText = "";
            this.m_labelMotorType.TabIndex = 0;
            this.m_labelMotorType.Text = "--";
            this.m_labelMotorType.TextAlignMain = Sys3Controls.EN_TEXTALIGN.BOTTOM_RIGHT;
            this.m_labelMotorType.TextAlignSub = Sys3Controls.EN_TEXTALIGN.MIDDLE_CENTER;
            this.m_labelMotorType.TextAlignUnit = Sys3Controls.EN_TEXTALIGN.MIDDLE_CENTER;
            this.m_labelMotorType.ThemeIndex = 0;
            this.m_labelMotorType.UnitAreaRate = 40;
            this.m_labelMotorType.UnitFont = new System.Drawing.Font("맑은 고딕", 10F, System.Drawing.FontStyle.Bold);
            this.m_labelMotorType.UnitFontColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.m_labelMotorType.UnitPositionVertical = false;
            this.m_labelMotorType.UnitText = "";
            this.m_labelMotorType.UseBorder = true;
            this.m_labelMotorType.UseEdgeRadius = false;
            this.m_labelMotorType.UseImage = false;
            this.m_labelMotorType.UseSubFont = false;
            this.m_labelMotorType.UseUnitFont = false;
            this.m_labelMotorType.Click += new System.EventHandler(this.Click_ConfigLabel);
            // 
            // m_labelAlarmStopMode
            // 
            this.m_labelAlarmStopMode.BackGroundColor = System.Drawing.Color.White;
            this.m_labelAlarmStopMode.BorderStroke = 2;
            this.m_labelAlarmStopMode.BorderStyle = System.Windows.Forms.ButtonBorderStyle.Solid;
            this.m_labelAlarmStopMode.Description = "";
            this.m_labelAlarmStopMode.DisabledColor = System.Drawing.Color.LightGray;
            this.m_labelAlarmStopMode.Dock = System.Windows.Forms.DockStyle.Fill;
            this.m_labelAlarmStopMode.EdgeRadius = 1;
            this.m_labelAlarmStopMode.Enabled = false;
            this.m_labelAlarmStopMode.ImagePosition = new System.Drawing.Point(0, 0);
            this.m_labelAlarmStopMode.ImageSize = new System.Drawing.Point(0, 0);
            this.m_labelAlarmStopMode.LoadImage = null;
            this.m_labelAlarmStopMode.Location = new System.Drawing.Point(583, 214);
            this.m_labelAlarmStopMode.MainFont = new System.Drawing.Font("맑은 고딕", 11F, System.Drawing.FontStyle.Bold);
            this.m_labelAlarmStopMode.MainFontColor = System.Drawing.Color.Black;
            this.m_labelAlarmStopMode.Margin = new System.Windows.Forms.Padding(2, 3, 0, 3);
            this.m_labelAlarmStopMode.Name = "m_labelAlarmStopMode";
            this.m_labelAlarmStopMode.Size = new System.Drawing.Size(196, 43);
            this.m_labelAlarmStopMode.SubFont = new System.Drawing.Font("맑은 고딕", 10F);
            this.m_labelAlarmStopMode.SubFontColor = System.Drawing.Color.Black;
            this.m_labelAlarmStopMode.SubText = "";
            this.m_labelAlarmStopMode.TabIndex = 5;
            this.m_labelAlarmStopMode.Text = "--";
            this.m_labelAlarmStopMode.TextAlignMain = Sys3Controls.EN_TEXTALIGN.BOTTOM_RIGHT;
            this.m_labelAlarmStopMode.TextAlignSub = Sys3Controls.EN_TEXTALIGN.MIDDLE_CENTER;
            this.m_labelAlarmStopMode.TextAlignUnit = Sys3Controls.EN_TEXTALIGN.MIDDLE_CENTER;
            this.m_labelAlarmStopMode.ThemeIndex = 0;
            this.m_labelAlarmStopMode.UnitAreaRate = 40;
            this.m_labelAlarmStopMode.UnitFont = new System.Drawing.Font("맑은 고딕", 10F, System.Drawing.FontStyle.Bold);
            this.m_labelAlarmStopMode.UnitFontColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.m_labelAlarmStopMode.UnitPositionVertical = false;
            this.m_labelAlarmStopMode.UnitText = "";
            this.m_labelAlarmStopMode.UseBorder = true;
            this.m_labelAlarmStopMode.UseEdgeRadius = false;
            this.m_labelAlarmStopMode.UseImage = false;
            this.m_labelAlarmStopMode.UseSubFont = false;
            this.m_labelAlarmStopMode.UseUnitFont = false;
            this.m_labelAlarmStopMode.Click += new System.EventHandler(this.Click_ConfigLabel);
            // 
            // m_labelEncoderDirection
            // 
            this.m_labelEncoderDirection.BackGroundColor = System.Drawing.Color.White;
            this.m_labelEncoderDirection.BorderStroke = 2;
            this.m_labelEncoderDirection.BorderStyle = System.Windows.Forms.ButtonBorderStyle.Solid;
            this.m_labelEncoderDirection.Description = "";
            this.m_labelEncoderDirection.DisabledColor = System.Drawing.Color.LightGray;
            this.m_labelEncoderDirection.Dock = System.Windows.Forms.DockStyle.Fill;
            this.m_labelEncoderDirection.EdgeRadius = 1;
            this.m_labelEncoderDirection.Enabled = false;
            this.m_labelEncoderDirection.ImagePosition = new System.Drawing.Point(0, 0);
            this.m_labelEncoderDirection.ImageSize = new System.Drawing.Point(0, 0);
            this.m_labelEncoderDirection.LoadImage = null;
            this.m_labelEncoderDirection.Location = new System.Drawing.Point(186, 322);
            this.m_labelEncoderDirection.MainFont = new System.Drawing.Font("맑은 고딕", 11F, System.Drawing.FontStyle.Bold);
            this.m_labelEncoderDirection.MainFontColor = System.Drawing.Color.Black;
            this.m_labelEncoderDirection.Margin = new System.Windows.Forms.Padding(2, 3, 0, 3);
            this.m_labelEncoderDirection.Name = "m_labelEncoderDirection";
            this.m_labelEncoderDirection.Size = new System.Drawing.Size(196, 46);
            this.m_labelEncoderDirection.SubFont = new System.Drawing.Font("맑은 고딕", 10F);
            this.m_labelEncoderDirection.SubFontColor = System.Drawing.Color.Black;
            this.m_labelEncoderDirection.SubText = "";
            this.m_labelEncoderDirection.TabIndex = 3;
            this.m_labelEncoderDirection.Text = "--";
            this.m_labelEncoderDirection.TextAlignMain = Sys3Controls.EN_TEXTALIGN.BOTTOM_RIGHT;
            this.m_labelEncoderDirection.TextAlignSub = Sys3Controls.EN_TEXTALIGN.MIDDLE_CENTER;
            this.m_labelEncoderDirection.TextAlignUnit = Sys3Controls.EN_TEXTALIGN.MIDDLE_CENTER;
            this.m_labelEncoderDirection.ThemeIndex = 0;
            this.m_labelEncoderDirection.UnitAreaRate = 40;
            this.m_labelEncoderDirection.UnitFont = new System.Drawing.Font("맑은 고딕", 10F, System.Drawing.FontStyle.Bold);
            this.m_labelEncoderDirection.UnitFontColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.m_labelEncoderDirection.UnitPositionVertical = false;
            this.m_labelEncoderDirection.UnitText = "";
            this.m_labelEncoderDirection.UseBorder = true;
            this.m_labelEncoderDirection.UseEdgeRadius = false;
            this.m_labelEncoderDirection.UseImage = false;
            this.m_labelEncoderDirection.UseSubFont = false;
            this.m_labelEncoderDirection.UseUnitFont = false;
            this.m_labelEncoderDirection.Click += new System.EventHandler(this.Click_ConfigLabel);
            // 
            // m_labelAlarmLogic
            // 
            this.m_labelAlarmLogic.BackGroundColor = System.Drawing.Color.White;
            this.m_labelAlarmLogic.BorderStroke = 2;
            this.m_labelAlarmLogic.BorderStyle = System.Windows.Forms.ButtonBorderStyle.Solid;
            this.m_labelAlarmLogic.Description = "";
            this.m_labelAlarmLogic.DisabledColor = System.Drawing.Color.LightGray;
            this.m_labelAlarmLogic.Dock = System.Windows.Forms.DockStyle.Fill;
            this.m_labelAlarmLogic.EdgeRadius = 1;
            this.m_labelAlarmLogic.Enabled = false;
            this.m_labelAlarmLogic.ImagePosition = new System.Drawing.Point(0, 0);
            this.m_labelAlarmLogic.ImageSize = new System.Drawing.Point(0, 0);
            this.m_labelAlarmLogic.LoadImage = null;
            this.m_labelAlarmLogic.Location = new System.Drawing.Point(583, 165);
            this.m_labelAlarmLogic.MainFont = new System.Drawing.Font("맑은 고딕", 11F, System.Drawing.FontStyle.Bold);
            this.m_labelAlarmLogic.MainFontColor = System.Drawing.Color.Black;
            this.m_labelAlarmLogic.Margin = new System.Windows.Forms.Padding(2, 3, 0, 3);
            this.m_labelAlarmLogic.Name = "m_labelAlarmLogic";
            this.m_labelAlarmLogic.Size = new System.Drawing.Size(196, 43);
            this.m_labelAlarmLogic.SubFont = new System.Drawing.Font("맑은 고딕", 10F);
            this.m_labelAlarmLogic.SubFontColor = System.Drawing.Color.Black;
            this.m_labelAlarmLogic.SubText = "";
            this.m_labelAlarmLogic.TabIndex = 4;
            this.m_labelAlarmLogic.Text = "--";
            this.m_labelAlarmLogic.TextAlignMain = Sys3Controls.EN_TEXTALIGN.BOTTOM_RIGHT;
            this.m_labelAlarmLogic.TextAlignSub = Sys3Controls.EN_TEXTALIGN.MIDDLE_CENTER;
            this.m_labelAlarmLogic.TextAlignUnit = Sys3Controls.EN_TEXTALIGN.MIDDLE_CENTER;
            this.m_labelAlarmLogic.ThemeIndex = 0;
            this.m_labelAlarmLogic.UnitAreaRate = 40;
            this.m_labelAlarmLogic.UnitFont = new System.Drawing.Font("맑은 고딕", 10F, System.Drawing.FontStyle.Bold);
            this.m_labelAlarmLogic.UnitFontColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.m_labelAlarmLogic.UnitPositionVertical = false;
            this.m_labelAlarmLogic.UnitText = "";
            this.m_labelAlarmLogic.UseBorder = true;
            this.m_labelAlarmLogic.UseEdgeRadius = false;
            this.m_labelAlarmLogic.UseImage = false;
            this.m_labelAlarmLogic.UseSubFont = false;
            this.m_labelAlarmLogic.UseUnitFont = false;
            this.m_labelAlarmLogic.Click += new System.EventHandler(this.Click_ConfigLabel);
            // 
            // m_lblMotionType
            // 
            this.m_lblMotionType.BackGroundColor = System.Drawing.Color.FromArgb(((int)(((byte)(200)))), ((int)(((byte)(200)))), ((int)(((byte)(220)))));
            this.m_lblMotionType.BorderStroke = 2;
            this.m_lblMotionType.BorderStyle = System.Windows.Forms.ButtonBorderStyle.Solid;
            this.m_lblMotionType.Description = "";
            this.m_lblMotionType.DisabledColor = System.Drawing.Color.DarkGray;
            this.m_lblMotionType.Dock = System.Windows.Forms.DockStyle.Fill;
            this.m_lblMotionType.EdgeRadius = 1;
            this.m_lblMotionType.ImagePosition = new System.Drawing.Point(0, 0);
            this.m_lblMotionType.ImageSize = new System.Drawing.Point(0, 0);
            this.m_lblMotionType.LoadImage = null;
            this.m_lblMotionType.Location = new System.Drawing.Point(5, 106);
            this.m_lblMotionType.MainFont = new System.Drawing.Font("맑은 고딕", 9F, System.Drawing.FontStyle.Bold);
            this.m_lblMotionType.MainFontColor = System.Drawing.Color.Black;
            this.m_lblMotionType.Margin = new System.Windows.Forms.Padding(0, 3, 0, 3);
            this.m_lblMotionType.Name = "m_lblMotionType";
            this.m_lblMotionType.Size = new System.Drawing.Size(179, 43);
            this.m_lblMotionType.SubFont = new System.Drawing.Font("맑은 고딕", 10F);
            this.m_lblMotionType.SubFontColor = System.Drawing.Color.Black;
            this.m_lblMotionType.SubText = "";
            this.m_lblMotionType.TabIndex = 1387;
            this.m_lblMotionType.Text = "MOTION TYPE";
            this.m_lblMotionType.TextAlignMain = Sys3Controls.EN_TEXTALIGN.TOP_LEFT;
            this.m_lblMotionType.TextAlignSub = Sys3Controls.EN_TEXTALIGN.MIDDLE_CENTER;
            this.m_lblMotionType.TextAlignUnit = Sys3Controls.EN_TEXTALIGN.MIDDLE_CENTER;
            this.m_lblMotionType.ThemeIndex = 0;
            this.m_lblMotionType.UnitAreaRate = 40;
            this.m_lblMotionType.UnitFont = new System.Drawing.Font("맑은 고딕", 10F, System.Drawing.FontStyle.Bold);
            this.m_lblMotionType.UnitFontColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.m_lblMotionType.UnitPositionVertical = false;
            this.m_lblMotionType.UnitText = "";
            this.m_lblMotionType.UseBorder = true;
            this.m_lblMotionType.UseEdgeRadius = false;
            this.m_lblMotionType.UseImage = false;
            this.m_lblMotionType.UseSubFont = false;
            this.m_lblMotionType.UseUnitFont = false;
            // 
            // m_lblMotorInmode
            // 
            this.m_lblMotorInmode.BackGroundColor = System.Drawing.Color.FromArgb(((int)(((byte)(200)))), ((int)(((byte)(200)))), ((int)(((byte)(220)))));
            this.m_lblMotorInmode.BorderStroke = 2;
            this.m_lblMotorInmode.BorderStyle = System.Windows.Forms.ButtonBorderStyle.Solid;
            this.m_lblMotorInmode.Description = "";
            this.m_lblMotorInmode.DisabledColor = System.Drawing.Color.DarkGray;
            this.m_lblMotorInmode.Dock = System.Windows.Forms.DockStyle.Fill;
            this.m_lblMotorInmode.EdgeRadius = 1;
            this.m_lblMotorInmode.ImagePosition = new System.Drawing.Point(0, 0);
            this.m_lblMotorInmode.ImageSize = new System.Drawing.Point(0, 0);
            this.m_lblMotorInmode.LoadImage = null;
            this.m_lblMotorInmode.Location = new System.Drawing.Point(5, 165);
            this.m_lblMotorInmode.MainFont = new System.Drawing.Font("맑은 고딕", 9F, System.Drawing.FontStyle.Bold);
            this.m_lblMotorInmode.MainFontColor = System.Drawing.Color.Black;
            this.m_lblMotorInmode.Margin = new System.Windows.Forms.Padding(0, 3, 0, 3);
            this.m_lblMotorInmode.Name = "m_lblMotorInmode";
            this.m_lblMotorInmode.Size = new System.Drawing.Size(179, 43);
            this.m_lblMotorInmode.SubFont = new System.Drawing.Font("맑은 고딕", 10F);
            this.m_lblMotorInmode.SubFontColor = System.Drawing.Color.Black;
            this.m_lblMotorInmode.SubText = "";
            this.m_lblMotorInmode.TabIndex = 1392;
            this.m_lblMotorInmode.Text = "MOTOR INMODE";
            this.m_lblMotorInmode.TextAlignMain = Sys3Controls.EN_TEXTALIGN.TOP_LEFT;
            this.m_lblMotorInmode.TextAlignSub = Sys3Controls.EN_TEXTALIGN.MIDDLE_CENTER;
            this.m_lblMotorInmode.TextAlignUnit = Sys3Controls.EN_TEXTALIGN.MIDDLE_CENTER;
            this.m_lblMotorInmode.ThemeIndex = 0;
            this.m_lblMotorInmode.UnitAreaRate = 40;
            this.m_lblMotorInmode.UnitFont = new System.Drawing.Font("맑은 고딕", 10F, System.Drawing.FontStyle.Bold);
            this.m_lblMotorInmode.UnitFontColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.m_lblMotorInmode.UnitPositionVertical = false;
            this.m_lblMotorInmode.UnitText = "";
            this.m_lblMotorInmode.UseBorder = true;
            this.m_lblMotorInmode.UseEdgeRadius = false;
            this.m_lblMotorInmode.UseImage = false;
            this.m_lblMotorInmode.UseSubFont = false;
            this.m_lblMotorInmode.UseUnitFont = false;
            // 
            // m_lblAlarmStopMode
            // 
            this.m_lblAlarmStopMode.BackGroundColor = System.Drawing.Color.FromArgb(((int)(((byte)(200)))), ((int)(((byte)(200)))), ((int)(((byte)(220)))));
            this.m_lblAlarmStopMode.BorderStroke = 2;
            this.m_lblAlarmStopMode.BorderStyle = System.Windows.Forms.ButtonBorderStyle.Solid;
            this.m_lblAlarmStopMode.Description = "";
            this.m_lblAlarmStopMode.DisabledColor = System.Drawing.Color.DarkGray;
            this.m_lblAlarmStopMode.Dock = System.Windows.Forms.DockStyle.Fill;
            this.m_lblAlarmStopMode.EdgeRadius = 1;
            this.m_lblAlarmStopMode.ImagePosition = new System.Drawing.Point(0, 0);
            this.m_lblAlarmStopMode.ImageSize = new System.Drawing.Point(0, 0);
            this.m_lblAlarmStopMode.LoadImage = null;
            this.m_lblAlarmStopMode.Location = new System.Drawing.Point(402, 214);
            this.m_lblAlarmStopMode.MainFont = new System.Drawing.Font("맑은 고딕", 9F, System.Drawing.FontStyle.Bold);
            this.m_lblAlarmStopMode.MainFontColor = System.Drawing.Color.Black;
            this.m_lblAlarmStopMode.Margin = new System.Windows.Forms.Padding(0, 3, 0, 3);
            this.m_lblAlarmStopMode.Name = "m_lblAlarmStopMode";
            this.m_lblAlarmStopMode.Size = new System.Drawing.Size(179, 43);
            this.m_lblAlarmStopMode.SubFont = new System.Drawing.Font("맑은 고딕", 9F, System.Drawing.FontStyle.Bold);
            this.m_lblAlarmStopMode.SubFontColor = System.Drawing.Color.DarkBlue;
            this.m_lblAlarmStopMode.SubText = "COMMAND";
            this.m_lblAlarmStopMode.TabIndex = 1388;
            this.m_lblAlarmStopMode.Text = "ALARM STOPMODE";
            this.m_lblAlarmStopMode.TextAlignMain = Sys3Controls.EN_TEXTALIGN.TOP_LEFT;
            this.m_lblAlarmStopMode.TextAlignSub = Sys3Controls.EN_TEXTALIGN.BOTTOM_RIGHT;
            this.m_lblAlarmStopMode.TextAlignUnit = Sys3Controls.EN_TEXTALIGN.MIDDLE_CENTER;
            this.m_lblAlarmStopMode.ThemeIndex = 0;
            this.m_lblAlarmStopMode.UnitAreaRate = 40;
            this.m_lblAlarmStopMode.UnitFont = new System.Drawing.Font("맑은 고딕", 10F, System.Drawing.FontStyle.Bold);
            this.m_lblAlarmStopMode.UnitFontColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.m_lblAlarmStopMode.UnitPositionVertical = false;
            this.m_lblAlarmStopMode.UnitText = "";
            this.m_lblAlarmStopMode.UseBorder = true;
            this.m_lblAlarmStopMode.UseEdgeRadius = false;
            this.m_lblAlarmStopMode.UseImage = false;
            this.m_lblAlarmStopMode.UseSubFont = false;
            this.m_lblAlarmStopMode.UseUnitFont = false;
            // 
            // m_labelCommandDirection
            // 
            this.m_labelCommandDirection.BackGroundColor = System.Drawing.Color.White;
            this.m_labelCommandDirection.BorderStroke = 2;
            this.m_labelCommandDirection.BorderStyle = System.Windows.Forms.ButtonBorderStyle.Solid;
            this.m_labelCommandDirection.Description = "";
            this.m_labelCommandDirection.DisabledColor = System.Drawing.Color.LightGray;
            this.m_labelCommandDirection.Dock = System.Windows.Forms.DockStyle.Fill;
            this.m_labelCommandDirection.EdgeRadius = 1;
            this.m_labelCommandDirection.Enabled = false;
            this.m_labelCommandDirection.ImagePosition = new System.Drawing.Point(0, 0);
            this.m_labelCommandDirection.ImageSize = new System.Drawing.Point(0, 0);
            this.m_labelCommandDirection.LoadImage = null;
            this.m_labelCommandDirection.Location = new System.Drawing.Point(186, 273);
            this.m_labelCommandDirection.MainFont = new System.Drawing.Font("맑은 고딕", 11F, System.Drawing.FontStyle.Bold);
            this.m_labelCommandDirection.MainFontColor = System.Drawing.Color.Black;
            this.m_labelCommandDirection.Margin = new System.Windows.Forms.Padding(2, 3, 0, 3);
            this.m_labelCommandDirection.Name = "m_labelCommandDirection";
            this.m_labelCommandDirection.Size = new System.Drawing.Size(196, 43);
            this.m_labelCommandDirection.SubFont = new System.Drawing.Font("맑은 고딕", 10F);
            this.m_labelCommandDirection.SubFontColor = System.Drawing.Color.Black;
            this.m_labelCommandDirection.SubText = "";
            this.m_labelCommandDirection.TabIndex = 2;
            this.m_labelCommandDirection.Text = "--";
            this.m_labelCommandDirection.TextAlignMain = Sys3Controls.EN_TEXTALIGN.BOTTOM_RIGHT;
            this.m_labelCommandDirection.TextAlignSub = Sys3Controls.EN_TEXTALIGN.MIDDLE_CENTER;
            this.m_labelCommandDirection.TextAlignUnit = Sys3Controls.EN_TEXTALIGN.MIDDLE_CENTER;
            this.m_labelCommandDirection.ThemeIndex = 0;
            this.m_labelCommandDirection.UnitAreaRate = 40;
            this.m_labelCommandDirection.UnitFont = new System.Drawing.Font("맑은 고딕", 10F, System.Drawing.FontStyle.Bold);
            this.m_labelCommandDirection.UnitFontColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.m_labelCommandDirection.UnitPositionVertical = false;
            this.m_labelCommandDirection.UnitText = "";
            this.m_labelCommandDirection.UseBorder = true;
            this.m_labelCommandDirection.UseEdgeRadius = false;
            this.m_labelCommandDirection.UseImage = false;
            this.m_labelCommandDirection.UseSubFont = false;
            this.m_labelCommandDirection.UseUnitFont = false;
            this.m_labelCommandDirection.Click += new System.EventHandler(this.Click_ConfigLabel);
            // 
            // m_lblAlarmLogic
            // 
            this.m_lblAlarmLogic.BackGroundColor = System.Drawing.Color.FromArgb(((int)(((byte)(200)))), ((int)(((byte)(200)))), ((int)(((byte)(220)))));
            this.m_lblAlarmLogic.BorderStroke = 2;
            this.m_lblAlarmLogic.BorderStyle = System.Windows.Forms.ButtonBorderStyle.Solid;
            this.m_lblAlarmLogic.Description = "";
            this.m_lblAlarmLogic.DisabledColor = System.Drawing.Color.DarkGray;
            this.m_lblAlarmLogic.Dock = System.Windows.Forms.DockStyle.Fill;
            this.m_lblAlarmLogic.EdgeRadius = 1;
            this.m_lblAlarmLogic.ImagePosition = new System.Drawing.Point(0, 0);
            this.m_lblAlarmLogic.ImageSize = new System.Drawing.Point(0, 0);
            this.m_lblAlarmLogic.LoadImage = null;
            this.m_lblAlarmLogic.Location = new System.Drawing.Point(402, 165);
            this.m_lblAlarmLogic.MainFont = new System.Drawing.Font("맑은 고딕", 9F, System.Drawing.FontStyle.Bold);
            this.m_lblAlarmLogic.MainFontColor = System.Drawing.Color.Black;
            this.m_lblAlarmLogic.Margin = new System.Windows.Forms.Padding(0, 3, 0, 3);
            this.m_lblAlarmLogic.Name = "m_lblAlarmLogic";
            this.m_lblAlarmLogic.Size = new System.Drawing.Size(179, 43);
            this.m_lblAlarmLogic.SubFont = new System.Drawing.Font("맑은 고딕", 10F);
            this.m_lblAlarmLogic.SubFontColor = System.Drawing.Color.Black;
            this.m_lblAlarmLogic.SubText = "";
            this.m_lblAlarmLogic.TabIndex = 1387;
            this.m_lblAlarmLogic.Text = "ALARM LOGIC";
            this.m_lblAlarmLogic.TextAlignMain = Sys3Controls.EN_TEXTALIGN.TOP_LEFT;
            this.m_lblAlarmLogic.TextAlignSub = Sys3Controls.EN_TEXTALIGN.MIDDLE_CENTER;
            this.m_lblAlarmLogic.TextAlignUnit = Sys3Controls.EN_TEXTALIGN.MIDDLE_CENTER;
            this.m_lblAlarmLogic.ThemeIndex = 0;
            this.m_lblAlarmLogic.UnitAreaRate = 40;
            this.m_lblAlarmLogic.UnitFont = new System.Drawing.Font("맑은 고딕", 10F, System.Drawing.FontStyle.Bold);
            this.m_lblAlarmLogic.UnitFontColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.m_lblAlarmLogic.UnitPositionVertical = false;
            this.m_lblAlarmLogic.UnitText = "";
            this.m_lblAlarmLogic.UseBorder = true;
            this.m_lblAlarmLogic.UseEdgeRadius = false;
            this.m_lblAlarmLogic.UseImage = false;
            this.m_lblAlarmLogic.UseSubFont = false;
            this.m_lblAlarmLogic.UseUnitFont = false;
            // 
            // m_labelMotionType
            // 
            this.m_labelMotionType.BackGroundColor = System.Drawing.Color.White;
            this.m_labelMotionType.BorderStroke = 2;
            this.m_labelMotionType.BorderStyle = System.Windows.Forms.ButtonBorderStyle.Solid;
            this.m_labelMotionType.Description = "";
            this.m_labelMotionType.DisabledColor = System.Drawing.Color.LightGray;
            this.m_labelMotionType.Dock = System.Windows.Forms.DockStyle.Fill;
            this.m_labelMotionType.EdgeRadius = 1;
            this.m_labelMotionType.Enabled = false;
            this.m_labelMotionType.ImagePosition = new System.Drawing.Point(0, 0);
            this.m_labelMotionType.ImageSize = new System.Drawing.Point(0, 0);
            this.m_labelMotionType.LoadImage = null;
            this.m_labelMotionType.Location = new System.Drawing.Point(186, 106);
            this.m_labelMotionType.MainFont = new System.Drawing.Font("맑은 고딕", 11F, System.Drawing.FontStyle.Bold);
            this.m_labelMotionType.MainFontColor = System.Drawing.Color.Black;
            this.m_labelMotionType.Margin = new System.Windows.Forms.Padding(2, 3, 0, 3);
            this.m_labelMotionType.Name = "m_labelMotionType";
            this.m_labelMotionType.Size = new System.Drawing.Size(196, 43);
            this.m_labelMotionType.SubFont = new System.Drawing.Font("맑은 고딕", 10F);
            this.m_labelMotionType.SubFontColor = System.Drawing.Color.Black;
            this.m_labelMotionType.SubText = "";
            this.m_labelMotionType.TabIndex = 1;
            this.m_labelMotionType.Text = "--";
            this.m_labelMotionType.TextAlignMain = Sys3Controls.EN_TEXTALIGN.BOTTOM_RIGHT;
            this.m_labelMotionType.TextAlignSub = Sys3Controls.EN_TEXTALIGN.MIDDLE_CENTER;
            this.m_labelMotionType.TextAlignUnit = Sys3Controls.EN_TEXTALIGN.MIDDLE_CENTER;
            this.m_labelMotionType.ThemeIndex = 0;
            this.m_labelMotionType.UnitAreaRate = 40;
            this.m_labelMotionType.UnitFont = new System.Drawing.Font("맑은 고딕", 10F, System.Drawing.FontStyle.Bold);
            this.m_labelMotionType.UnitFontColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.m_labelMotionType.UnitPositionVertical = false;
            this.m_labelMotionType.UnitText = "";
            this.m_labelMotionType.UseBorder = true;
            this.m_labelMotionType.UseEdgeRadius = false;
            this.m_labelMotionType.UseImage = false;
            this.m_labelMotionType.UseSubFont = false;
            this.m_labelMotionType.UseUnitFont = false;
            this.m_labelMotionType.Click += new System.EventHandler(this.Click_ConfigLabel);
            // 
            // m_lblOutputScale
            // 
            this.m_lblOutputScale.BackGroundColor = System.Drawing.Color.FromArgb(((int)(((byte)(200)))), ((int)(((byte)(200)))), ((int)(((byte)(220)))));
            this.m_lblOutputScale.BorderStroke = 2;
            this.m_lblOutputScale.BorderStyle = System.Windows.Forms.ButtonBorderStyle.Solid;
            this.m_lblOutputScale.Description = "";
            this.m_lblOutputScale.DisabledColor = System.Drawing.Color.DarkGray;
            this.m_lblOutputScale.Dock = System.Windows.Forms.DockStyle.Fill;
            this.m_lblOutputScale.EdgeRadius = 1;
            this.m_lblOutputScale.ImagePosition = new System.Drawing.Point(0, 0);
            this.m_lblOutputScale.ImageSize = new System.Drawing.Point(0, 0);
            this.m_lblOutputScale.LoadImage = null;
            this.m_lblOutputScale.Location = new System.Drawing.Point(402, 106);
            this.m_lblOutputScale.MainFont = new System.Drawing.Font("맑은 고딕", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.m_lblOutputScale.MainFontColor = System.Drawing.Color.Black;
            this.m_lblOutputScale.Margin = new System.Windows.Forms.Padding(0, 3, 0, 3);
            this.m_lblOutputScale.Name = "m_lblOutputScale";
            this.m_lblOutputScale.Size = new System.Drawing.Size(179, 43);
            this.m_lblOutputScale.SubFont = new System.Drawing.Font("맑은 고딕", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.m_lblOutputScale.SubFontColor = System.Drawing.Color.DarkBlue;
            this.m_lblOutputScale.SubText = "MM PER REVOLUTION \\n\\n\\n COUNT PER REVOLUTION";
            this.m_lblOutputScale.TabIndex = 1388;
            this.m_lblOutputScale.Text = "OUTPUT SCALE";
            this.m_lblOutputScale.TextAlignMain = Sys3Controls.EN_TEXTALIGN.TOP_LEFT;
            this.m_lblOutputScale.TextAlignSub = Sys3Controls.EN_TEXTALIGN.BOTTOM_RIGHT;
            this.m_lblOutputScale.TextAlignUnit = Sys3Controls.EN_TEXTALIGN.MIDDLE_CENTER;
            this.m_lblOutputScale.ThemeIndex = 0;
            this.m_lblOutputScale.UnitAreaRate = 40;
            this.m_lblOutputScale.UnitFont = new System.Drawing.Font("맑은 고딕", 10F, System.Drawing.FontStyle.Bold);
            this.m_lblOutputScale.UnitFontColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.m_lblOutputScale.UnitPositionVertical = false;
            this.m_lblOutputScale.UnitText = "";
            this.m_lblOutputScale.UseBorder = true;
            this.m_lblOutputScale.UseEdgeRadius = false;
            this.m_lblOutputScale.UseImage = false;
            this.m_lblOutputScale.UseSubFont = false;
            this.m_lblOutputScale.UseUnitFont = false;
            // 
            // m_lblCommandDirection
            // 
            this.m_lblCommandDirection.BackGroundColor = System.Drawing.Color.FromArgb(((int)(((byte)(200)))), ((int)(((byte)(200)))), ((int)(((byte)(220)))));
            this.m_lblCommandDirection.BorderStroke = 2;
            this.m_lblCommandDirection.BorderStyle = System.Windows.Forms.ButtonBorderStyle.Solid;
            this.m_lblCommandDirection.Description = "";
            this.m_lblCommandDirection.DisabledColor = System.Drawing.Color.DarkGray;
            this.m_lblCommandDirection.Dock = System.Windows.Forms.DockStyle.Fill;
            this.m_lblCommandDirection.EdgeRadius = 1;
            this.m_lblCommandDirection.ImagePosition = new System.Drawing.Point(0, 0);
            this.m_lblCommandDirection.ImageSize = new System.Drawing.Point(0, 0);
            this.m_lblCommandDirection.LoadImage = null;
            this.m_lblCommandDirection.Location = new System.Drawing.Point(5, 273);
            this.m_lblCommandDirection.MainFont = new System.Drawing.Font("맑은 고딕", 9F, System.Drawing.FontStyle.Bold);
            this.m_lblCommandDirection.MainFontColor = System.Drawing.Color.Black;
            this.m_lblCommandDirection.Margin = new System.Windows.Forms.Padding(0, 3, 0, 3);
            this.m_lblCommandDirection.Name = "m_lblCommandDirection";
            this.m_lblCommandDirection.Size = new System.Drawing.Size(179, 43);
            this.m_lblCommandDirection.SubFont = new System.Drawing.Font("맑은 고딕", 9F, System.Drawing.FontStyle.Bold);
            this.m_lblCommandDirection.SubFontColor = System.Drawing.Color.DarkBlue;
            this.m_lblCommandDirection.SubText = "COMMAND";
            this.m_lblCommandDirection.TabIndex = 1388;
            this.m_lblCommandDirection.Text = "MOTOR DIRECTION";
            this.m_lblCommandDirection.TextAlignMain = Sys3Controls.EN_TEXTALIGN.TOP_LEFT;
            this.m_lblCommandDirection.TextAlignSub = Sys3Controls.EN_TEXTALIGN.BOTTOM_RIGHT;
            this.m_lblCommandDirection.TextAlignUnit = Sys3Controls.EN_TEXTALIGN.MIDDLE_CENTER;
            this.m_lblCommandDirection.ThemeIndex = 0;
            this.m_lblCommandDirection.UnitAreaRate = 40;
            this.m_lblCommandDirection.UnitFont = new System.Drawing.Font("맑은 고딕", 10F, System.Drawing.FontStyle.Bold);
            this.m_lblCommandDirection.UnitFontColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.m_lblCommandDirection.UnitPositionVertical = false;
            this.m_lblCommandDirection.UnitText = "";
            this.m_lblCommandDirection.UseBorder = true;
            this.m_lblCommandDirection.UseEdgeRadius = false;
            this.m_lblCommandDirection.UseImage = false;
            this.m_lblCommandDirection.UseSubFont = true;
            this.m_lblCommandDirection.UseUnitFont = false;
            // 
            // m_lblEncoderDirection
            // 
            this.m_lblEncoderDirection.BackGroundColor = System.Drawing.Color.FromArgb(((int)(((byte)(200)))), ((int)(((byte)(200)))), ((int)(((byte)(220)))));
            this.m_lblEncoderDirection.BorderStroke = 2;
            this.m_lblEncoderDirection.BorderStyle = System.Windows.Forms.ButtonBorderStyle.Solid;
            this.m_lblEncoderDirection.Description = "";
            this.m_lblEncoderDirection.DisabledColor = System.Drawing.Color.DarkGray;
            this.m_lblEncoderDirection.Dock = System.Windows.Forms.DockStyle.Fill;
            this.m_lblEncoderDirection.EdgeRadius = 1;
            this.m_lblEncoderDirection.ImagePosition = new System.Drawing.Point(0, 0);
            this.m_lblEncoderDirection.ImageSize = new System.Drawing.Point(0, 0);
            this.m_lblEncoderDirection.LoadImage = null;
            this.m_lblEncoderDirection.Location = new System.Drawing.Point(5, 322);
            this.m_lblEncoderDirection.MainFont = new System.Drawing.Font("맑은 고딕", 9F, System.Drawing.FontStyle.Bold);
            this.m_lblEncoderDirection.MainFontColor = System.Drawing.Color.Black;
            this.m_lblEncoderDirection.Margin = new System.Windows.Forms.Padding(0, 3, 0, 3);
            this.m_lblEncoderDirection.Name = "m_lblEncoderDirection";
            this.m_lblEncoderDirection.Size = new System.Drawing.Size(179, 46);
            this.m_lblEncoderDirection.SubFont = new System.Drawing.Font("맑은 고딕", 9F, System.Drawing.FontStyle.Bold);
            this.m_lblEncoderDirection.SubFontColor = System.Drawing.Color.DarkBlue;
            this.m_lblEncoderDirection.SubText = "ENCODER";
            this.m_lblEncoderDirection.TabIndex = 1388;
            this.m_lblEncoderDirection.Text = "MOTOR DIRECTION";
            this.m_lblEncoderDirection.TextAlignMain = Sys3Controls.EN_TEXTALIGN.TOP_LEFT;
            this.m_lblEncoderDirection.TextAlignSub = Sys3Controls.EN_TEXTALIGN.BOTTOM_RIGHT;
            this.m_lblEncoderDirection.TextAlignUnit = Sys3Controls.EN_TEXTALIGN.MIDDLE_CENTER;
            this.m_lblEncoderDirection.ThemeIndex = 0;
            this.m_lblEncoderDirection.UnitAreaRate = 40;
            this.m_lblEncoderDirection.UnitFont = new System.Drawing.Font("맑은 고딕", 10F, System.Drawing.FontStyle.Bold);
            this.m_lblEncoderDirection.UnitFontColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.m_lblEncoderDirection.UnitPositionVertical = false;
            this.m_lblEncoderDirection.UnitText = "";
            this.m_lblEncoderDirection.UseBorder = true;
            this.m_lblEncoderDirection.UseEdgeRadius = false;
            this.m_lblEncoderDirection.UseImage = false;
            this.m_lblEncoderDirection.UseSubFont = true;
            this.m_lblEncoderDirection.UseUnitFont = false;
            // 
            // m_lblInputScale
            // 
            this.m_lblInputScale.BackGroundColor = System.Drawing.Color.FromArgb(((int)(((byte)(200)))), ((int)(((byte)(200)))), ((int)(((byte)(220)))));
            this.m_lblInputScale.BorderStroke = 2;
            this.m_lblInputScale.BorderStyle = System.Windows.Forms.ButtonBorderStyle.Solid;
            this.m_lblInputScale.Description = "";
            this.m_lblInputScale.DisabledColor = System.Drawing.Color.DarkGray;
            this.m_lblInputScale.Dock = System.Windows.Forms.DockStyle.Fill;
            this.m_lblInputScale.EdgeRadius = 1;
            this.m_lblInputScale.ImagePosition = new System.Drawing.Point(0, 0);
            this.m_lblInputScale.ImageSize = new System.Drawing.Point(0, 0);
            this.m_lblInputScale.LoadImage = null;
            this.m_lblInputScale.Location = new System.Drawing.Point(402, 57);
            this.m_lblInputScale.MainFont = new System.Drawing.Font("맑은 고딕", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.m_lblInputScale.MainFontColor = System.Drawing.Color.Black;
            this.m_lblInputScale.Margin = new System.Windows.Forms.Padding(0, 3, 0, 3);
            this.m_lblInputScale.Name = "m_lblInputScale";
            this.m_lblInputScale.Size = new System.Drawing.Size(179, 43);
            this.m_lblInputScale.SubFont = new System.Drawing.Font("맑은 고딕", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.m_lblInputScale.SubFontColor = System.Drawing.Color.DarkBlue;
            this.m_lblInputScale.SubText = "MM PER REVOLUTION \\n\\n\\n COUNT PER REVOLUTION";
            this.m_lblInputScale.TabIndex = 1388;
            this.m_lblInputScale.Text = "INPUT SCALE";
            this.m_lblInputScale.TextAlignMain = Sys3Controls.EN_TEXTALIGN.TOP_LEFT;
            this.m_lblInputScale.TextAlignSub = Sys3Controls.EN_TEXTALIGN.BOTTOM_RIGHT;
            this.m_lblInputScale.TextAlignUnit = Sys3Controls.EN_TEXTALIGN.MIDDLE_CENTER;
            this.m_lblInputScale.ThemeIndex = 0;
            this.m_lblInputScale.UnitAreaRate = 40;
            this.m_lblInputScale.UnitFont = new System.Drawing.Font("맑은 고딕", 10F, System.Drawing.FontStyle.Bold);
            this.m_lblInputScale.UnitFontColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.m_lblInputScale.UnitPositionVertical = false;
            this.m_lblInputScale.UnitText = "";
            this.m_lblInputScale.UseBorder = true;
            this.m_lblInputScale.UseEdgeRadius = false;
            this.m_lblInputScale.UseImage = false;
            this.m_lblInputScale.UseSubFont = false;
            this.m_lblInputScale.UseUnitFont = false;
            // 
            // MotorGeneral
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.Controls.Add(this.m_groupSpeedContents);
            this.Margin = new System.Windows.Forms.Padding(0);
            this.Name = "MotorGeneral";
            this.Size = new System.Drawing.Size(1190, 411);
            this.m_groupSpeedContents.ResumeLayout(false);
            this._tableLayoutPanel_GantryStatus.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

		private Sys3Controls.Sys3GroupBoxContainer m_groupSpeedContents;
		private Sys3Controls.Sys3Label m_lblMotorType;
		private Sys3Controls.Sys3Label m_lblCommandDirection;
		private Sys3Controls.Sys3Label m_lblEncoderDirection;
		private Sys3Controls.Sys3Label m_labelMotorType;
		private Sys3Controls.Sys3Label m_labelCommandDirection;
		private Sys3Controls.Sys3Label m_labelEncoderDirection;
		private Sys3Controls.Sys3Label m_labelOutputScaleMPR;
		private Sys3Controls.Sys3Label m_labelOutputScaleCPR;
		private Sys3Controls.Sys3Label m_lblMotionType;
		private Sys3Controls.Sys3Label m_labelMotionType;
		private Sys3Controls.Sys3Label m_lblInputScale;
		private Sys3Controls.Sys3Label m_labelInputScaleMPR;
		private Sys3Controls.Sys3Label m_labelInputScaleCPR;
        private Sys3Controls.Sys3Label m_lblOutputScale;
		private Sys3Controls.Sys3Label m_lblAlarmLogic;
		private Sys3Controls.Sys3Label m_lblAlarmStopMode;
		private Sys3Controls.Sys3Label m_lblZphaseLogic;
		private Sys3Controls.Sys3Label m_labelAlarmLogic;
		private Sys3Controls.Sys3Label m_labelAlarmStopMode;
		private Sys3Controls.Sys3Label m_labelZphaseLogic;
		private Sys3Controls.Sys3Label sys3Label7;
		private Sys3Controls.Sys3Label sys3Label8;
        private Sys3Controls.Sys3Label m_labelMotorOutmode;
        private Sys3Controls.Sys3Label m_labelMotorInmode;
        private Sys3Controls.Sys3Label m_lblMotorOutmode;
        private Sys3Controls.Sys3Label m_lblMotorInmode;
        private System.Windows.Forms.TableLayoutPanel _tableLayoutPanel_GantryStatus;
    }
}
