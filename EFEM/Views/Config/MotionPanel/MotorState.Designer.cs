namespace FrameOfSystem3.Views.Config.MotionPanel
{
    partial class MotorState
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
            this.m_ledSWLimitPositive = new Sys3Controls.Sys3LedLabel();
            this.m_ledSWLimitNegative = new Sys3Controls.Sys3LedLabel();
            this.m_ledRepeat = new Sys3Controls.Sys3LedLabel();
            this.m_ledHomeTimeout = new Sys3Controls.Sys3LedLabel();
            this.m_ledHomeEnd = new Sys3Controls.Sys3LedLabel();
            this.m_ledHomeSwitch = new Sys3Controls.Sys3LedLabel();
            this.m_ledMotionTimeout = new Sys3Controls.Sys3LedLabel();
            this.m_ledMotionDone = new Sys3Controls.Sys3LedLabel();
            this.m_ledLimitPositive = new Sys3Controls.Sys3LedLabel();
            this.m_ledLimitNegative = new Sys3Controls.Sys3LedLabel();
            this.m_ledAlarm = new Sys3Controls.Sys3LedLabel();
            this.m_ledMotorOn = new Sys3Controls.Sys3LedLabel();
            this.m_groupState = new Sys3Controls.Sys3GroupBoxContainer();
            this._tableLayoutPanel_GantryConfiguration = new System.Windows.Forms.TableLayoutPanel();
            this.m_lblMotorOn = new Sys3Controls.Sys3Label();
            this.m_lblAlarm = new Sys3Controls.Sys3Label();
            this.m_lblLimitNegative = new Sys3Controls.Sys3Label();
            this.m_lblLimitPositive = new Sys3Controls.Sys3Label();
            this.m_lblSWLimitNegative = new Sys3Controls.Sys3Label();
            this.m_lblSWLimitPositive = new Sys3Controls.Sys3Label();
            this.m_lblInterlock = new Sys3Controls.Sys3Label();
            this.m_lblHomeSwitch = new Sys3Controls.Sys3Label();
            this.m_lblHomeTimeout = new Sys3Controls.Sys3Label();
            this.m_lblMotionDone = new Sys3Controls.Sys3Label();
            this.m_lblHomeEnd = new Sys3Controls.Sys3Label();
            this.m_lblMotionTimeout = new Sys3Controls.Sys3Label();
            this.m_ledInterlock = new Sys3Controls.Sys3LedLabel();
            this.m_groupPosition = new Sys3Controls.Sys3GroupBoxContainer();
            this._tableLayoutPanel_Position = new System.Windows.Forms.TableLayoutPanel();
            this.m_lblCommand = new Sys3Controls.Sys3Label();
            this.m_labelCommand = new Sys3Controls.Sys3Label();
            this.m_lblActual = new Sys3Controls.Sys3Label();
            this.m_lblError = new Sys3Controls.Sys3Label();
            this.m_labelActual = new Sys3Controls.Sys3Label();
            this.m_labelError = new Sys3Controls.Sys3Label();
            this.m_btnClear = new Sys3Controls.Sys3button();
            this.m_groupRepeat = new Sys3Controls.Sys3GroupBoxContainer();
            this._tableLayoutPanel_Repeat = new System.Windows.Forms.TableLayoutPanel();
            this.m_ledFirstPosition = new Sys3Controls.Sys3LedLabel();
            this.m_ledSecondDelay = new Sys3Controls.Sys3LedLabel();
            this.m_lblPositionFirst = new Sys3Controls.Sys3Label();
            this.m_ledSecondPosition = new Sys3Controls.Sys3LedLabel();
            this.m_btnRepeat = new Sys3Controls.Sys3button();
            this.m_labelPositionFirst = new Sys3Controls.Sys3Label();
            this.m_btnMoveSecond = new Sys3Controls.Sys3button();
            this.m_ledFirstDelay = new Sys3Controls.Sys3LedLabel();
            this.m_btnMoveFirst = new Sys3Controls.Sys3button();
            this.m_lblDelayFirst = new Sys3Controls.Sys3Label();
            this.m_labelDelayFirst = new Sys3Controls.Sys3Label();
            this.m_lblDelaySecond = new Sys3Controls.Sys3Label();
            this.m_lblPositionSecond = new Sys3Controls.Sys3Label();
            this.m_labelPositionSecond = new Sys3Controls.Sys3Label();
            this.m_labelDelaySecond = new Sys3Controls.Sys3Label();
            this.m_groupJog = new Sys3Controls.Sys3GroupBoxContainer();
            this._tableLayoutPanel_Jog = new System.Windows.Forms.TableLayoutPanel();
            this.m_lblJogGroup = new Sys3Controls.Sys3Label();
            this.m_btnMovePositive = new Sys3Controls.Sys3button();
            this.m_labelJogGroup = new Sys3Controls.Sys3Label();
            this.m_btnMoveNegative = new Sys3Controls.Sys3button();
            this.panel1 = new System.Windows.Forms.Panel();
            this.m_progressbarJog = new Sys3Controls.Sys3ProgressBar();
            this._tableLayoutPanel_Main = new System.Windows.Forms.TableLayoutPanel();
            this.m_groupState.SuspendLayout();
            this._tableLayoutPanel_GantryConfiguration.SuspendLayout();
            this.m_groupPosition.SuspendLayout();
            this._tableLayoutPanel_Position.SuspendLayout();
            this.m_groupRepeat.SuspendLayout();
            this._tableLayoutPanel_Repeat.SuspendLayout();
            this.m_groupJog.SuspendLayout();
            this._tableLayoutPanel_Jog.SuspendLayout();
            this.panel1.SuspendLayout();
            this._tableLayoutPanel_Main.SuspendLayout();
            this.SuspendLayout();
            // 
            // m_ledSWLimitPositive
            // 
            this.m_ledSWLimitPositive.Active = false;
            this.m_ledSWLimitPositive.ColorActive = System.Drawing.Color.FromArgb(((int)(((byte)(75)))), ((int)(((byte)(216)))), ((int)(((byte)(101)))));
            this.m_ledSWLimitPositive.ColorNormal = System.Drawing.Color.DimGray;
            this.m_ledSWLimitPositive.Dock = System.Windows.Forms.DockStyle.Fill;
            this.m_ledSWLimitPositive.Location = new System.Drawing.Point(135, 117);
            this.m_ledSWLimitPositive.Margin = new System.Windows.Forms.Padding(0, 3, 0, 3);
            this.m_ledSWLimitPositive.Name = "m_ledSWLimitPositive";
            this.m_ledSWLimitPositive.Size = new System.Drawing.Size(25, 31);
            this.m_ledSWLimitPositive.TabIndex = 1171;
            // 
            // m_ledSWLimitNegative
            // 
            this.m_ledSWLimitNegative.Active = false;
            this.m_ledSWLimitNegative.ColorActive = System.Drawing.Color.FromArgb(((int)(((byte)(75)))), ((int)(((byte)(216)))), ((int)(((byte)(101)))));
            this.m_ledSWLimitNegative.ColorNormal = System.Drawing.Color.DimGray;
            this.m_ledSWLimitNegative.Dock = System.Windows.Forms.DockStyle.Fill;
            this.m_ledSWLimitNegative.Location = new System.Drawing.Point(3, 117);
            this.m_ledSWLimitNegative.Margin = new System.Windows.Forms.Padding(0, 3, 0, 3);
            this.m_ledSWLimitNegative.Name = "m_ledSWLimitNegative";
            this.m_ledSWLimitNegative.Size = new System.Drawing.Size(25, 31);
            this.m_ledSWLimitNegative.TabIndex = 1169;
            // 
            // m_ledRepeat
            // 
            this.m_ledRepeat.Active = false;
            this.m_ledRepeat.ColorActive = System.Drawing.Color.FromArgb(((int)(((byte)(75)))), ((int)(((byte)(216)))), ((int)(((byte)(101)))));
            this.m_ledRepeat.ColorNormal = System.Drawing.Color.DimGray;
            this.m_ledRepeat.Dock = System.Windows.Forms.DockStyle.Fill;
            this.m_ledRepeat.Location = new System.Drawing.Point(423, 49);
            this.m_ledRepeat.Margin = new System.Windows.Forms.Padding(0);
            this.m_ledRepeat.Name = "m_ledRepeat";
            this._tableLayoutPanel_Repeat.SetRowSpan(this.m_ledRepeat, 3);
            this.m_ledRepeat.Size = new System.Drawing.Size(20, 102);
            this.m_ledRepeat.TabIndex = 1163;
            // 
            // m_ledHomeTimeout
            // 
            this.m_ledHomeTimeout.Active = false;
            this.m_ledHomeTimeout.ColorActive = System.Drawing.Color.FromArgb(((int)(((byte)(75)))), ((int)(((byte)(216)))), ((int)(((byte)(101)))));
            this.m_ledHomeTimeout.ColorNormal = System.Drawing.Color.DimGray;
            this.m_ledHomeTimeout.Dock = System.Windows.Forms.DockStyle.Fill;
            this.m_ledHomeTimeout.Location = new System.Drawing.Point(3, 302);
            this.m_ledHomeTimeout.Margin = new System.Windows.Forms.Padding(0, 3, 0, 3);
            this.m_ledHomeTimeout.Name = "m_ledHomeTimeout";
            this.m_ledHomeTimeout.Size = new System.Drawing.Size(25, 31);
            this.m_ledHomeTimeout.TabIndex = 1151;
            // 
            // m_ledHomeEnd
            // 
            this.m_ledHomeEnd.Active = false;
            this.m_ledHomeEnd.ColorActive = System.Drawing.Color.FromArgb(((int)(((byte)(75)))), ((int)(((byte)(216)))), ((int)(((byte)(101)))));
            this.m_ledHomeEnd.ColorNormal = System.Drawing.Color.DimGray;
            this.m_ledHomeEnd.Dock = System.Windows.Forms.DockStyle.Fill;
            this.m_ledHomeEnd.Location = new System.Drawing.Point(3, 265);
            this.m_ledHomeEnd.Margin = new System.Windows.Forms.Padding(0, 3, 0, 3);
            this.m_ledHomeEnd.Name = "m_ledHomeEnd";
            this.m_ledHomeEnd.Size = new System.Drawing.Size(25, 31);
            this.m_ledHomeEnd.TabIndex = 1149;
            // 
            // m_ledHomeSwitch
            // 
            this.m_ledHomeSwitch.Active = false;
            this.m_ledHomeSwitch.ColorActive = System.Drawing.Color.FromArgb(((int)(((byte)(75)))), ((int)(((byte)(216)))), ((int)(((byte)(101)))));
            this.m_ledHomeSwitch.ColorNormal = System.Drawing.Color.DimGray;
            this.m_ledHomeSwitch.Dock = System.Windows.Forms.DockStyle.Fill;
            this.m_ledHomeSwitch.Location = new System.Drawing.Point(3, 228);
            this.m_ledHomeSwitch.Margin = new System.Windows.Forms.Padding(0, 3, 0, 3);
            this.m_ledHomeSwitch.Name = "m_ledHomeSwitch";
            this.m_ledHomeSwitch.Size = new System.Drawing.Size(25, 31);
            this.m_ledHomeSwitch.TabIndex = 1147;
            // 
            // m_ledMotionTimeout
            // 
            this.m_ledMotionTimeout.Active = false;
            this.m_ledMotionTimeout.ColorActive = System.Drawing.Color.FromArgb(((int)(((byte)(75)))), ((int)(((byte)(216)))), ((int)(((byte)(101)))));
            this.m_ledMotionTimeout.ColorNormal = System.Drawing.Color.DimGray;
            this.m_ledMotionTimeout.Dock = System.Windows.Forms.DockStyle.Fill;
            this.m_ledMotionTimeout.Location = new System.Drawing.Point(3, 191);
            this.m_ledMotionTimeout.Margin = new System.Windows.Forms.Padding(0, 3, 0, 3);
            this.m_ledMotionTimeout.Name = "m_ledMotionTimeout";
            this.m_ledMotionTimeout.Size = new System.Drawing.Size(25, 31);
            this.m_ledMotionTimeout.TabIndex = 1145;
            // 
            // m_ledMotionDone
            // 
            this.m_ledMotionDone.Active = false;
            this.m_ledMotionDone.ColorActive = System.Drawing.Color.FromArgb(((int)(((byte)(75)))), ((int)(((byte)(216)))), ((int)(((byte)(101)))));
            this.m_ledMotionDone.ColorNormal = System.Drawing.Color.DimGray;
            this.m_ledMotionDone.Dock = System.Windows.Forms.DockStyle.Fill;
            this.m_ledMotionDone.Location = new System.Drawing.Point(3, 154);
            this.m_ledMotionDone.Margin = new System.Windows.Forms.Padding(0, 3, 0, 3);
            this.m_ledMotionDone.Name = "m_ledMotionDone";
            this.m_ledMotionDone.Size = new System.Drawing.Size(25, 31);
            this.m_ledMotionDone.TabIndex = 1143;
            // 
            // m_ledLimitPositive
            // 
            this.m_ledLimitPositive.Active = false;
            this.m_ledLimitPositive.ColorActive = System.Drawing.Color.FromArgb(((int)(((byte)(75)))), ((int)(((byte)(216)))), ((int)(((byte)(101)))));
            this.m_ledLimitPositive.ColorNormal = System.Drawing.Color.DimGray;
            this.m_ledLimitPositive.Dock = System.Windows.Forms.DockStyle.Fill;
            this.m_ledLimitPositive.Location = new System.Drawing.Point(135, 80);
            this.m_ledLimitPositive.Margin = new System.Windows.Forms.Padding(0, 3, 0, 3);
            this.m_ledLimitPositive.Name = "m_ledLimitPositive";
            this.m_ledLimitPositive.Size = new System.Drawing.Size(25, 31);
            this.m_ledLimitPositive.TabIndex = 1141;
            // 
            // m_ledLimitNegative
            // 
            this.m_ledLimitNegative.Active = false;
            this.m_ledLimitNegative.ColorActive = System.Drawing.Color.FromArgb(((int)(((byte)(75)))), ((int)(((byte)(216)))), ((int)(((byte)(101)))));
            this.m_ledLimitNegative.ColorNormal = System.Drawing.Color.DimGray;
            this.m_ledLimitNegative.Dock = System.Windows.Forms.DockStyle.Fill;
            this.m_ledLimitNegative.Location = new System.Drawing.Point(3, 80);
            this.m_ledLimitNegative.Margin = new System.Windows.Forms.Padding(0, 3, 0, 3);
            this.m_ledLimitNegative.Name = "m_ledLimitNegative";
            this.m_ledLimitNegative.Size = new System.Drawing.Size(25, 31);
            this.m_ledLimitNegative.TabIndex = 1139;
            // 
            // m_ledAlarm
            // 
            this.m_ledAlarm.Active = false;
            this.m_ledAlarm.ColorActive = System.Drawing.Color.FromArgb(((int)(((byte)(75)))), ((int)(((byte)(216)))), ((int)(((byte)(101)))));
            this.m_ledAlarm.ColorNormal = System.Drawing.Color.DimGray;
            this.m_ledAlarm.Dock = System.Windows.Forms.DockStyle.Fill;
            this.m_ledAlarm.Location = new System.Drawing.Point(3, 43);
            this.m_ledAlarm.Margin = new System.Windows.Forms.Padding(0, 3, 0, 3);
            this.m_ledAlarm.Name = "m_ledAlarm";
            this.m_ledAlarm.Size = new System.Drawing.Size(25, 31);
            this.m_ledAlarm.TabIndex = 1137;
            // 
            // m_ledMotorOn
            // 
            this.m_ledMotorOn.Active = false;
            this.m_ledMotorOn.ColorActive = System.Drawing.Color.FromArgb(((int)(((byte)(75)))), ((int)(((byte)(216)))), ((int)(((byte)(101)))));
            this.m_ledMotorOn.ColorNormal = System.Drawing.Color.DimGray;
            this.m_ledMotorOn.Dock = System.Windows.Forms.DockStyle.Fill;
            this.m_ledMotorOn.Location = new System.Drawing.Point(3, 6);
            this.m_ledMotorOn.Margin = new System.Windows.Forms.Padding(0, 3, 0, 3);
            this.m_ledMotorOn.Name = "m_ledMotorOn";
            this.m_ledMotorOn.Size = new System.Drawing.Size(25, 31);
            this.m_ledMotorOn.TabIndex = 1135;
            // 
            // m_groupState
            // 
            this.m_groupState.BackGroundColor = System.Drawing.Color.WhiteSmoke;
            this.m_groupState.Controls.Add(this._tableLayoutPanel_GantryConfiguration);
            this.m_groupState.Dock = System.Windows.Forms.DockStyle.Fill;
            this.m_groupState.EdgeBorderStroke = 2;
            this.m_groupState.EdgeRadius = 2;
            this.m_groupState.LabelFont = new System.Drawing.Font("맑은 고딕", 10F, System.Drawing.FontStyle.Bold);
            this.m_groupState.LabelGradientColorFirst = System.Drawing.Color.FromArgb(((int)(((byte)(208)))), ((int)(((byte)(208)))), ((int)(((byte)(208)))));
            this.m_groupState.LabelGradientColorSecond = System.Drawing.Color.FromArgb(((int)(((byte)(240)))), ((int)(((byte)(240)))), ((int)(((byte)(240)))));
            this.m_groupState.LabelHeight = 30;
            this.m_groupState.LabelTextColor = System.Drawing.Color.Black;
            this.m_groupState.Location = new System.Drawing.Point(0, 0);
            this.m_groupState.Margin = new System.Windows.Forms.Padding(0);
            this.m_groupState.Name = "m_groupState";
            this.m_groupState.Padding = new System.Windows.Forms.Padding(3, 18, 3, 3);
            this._tableLayoutPanel_Main.SetRowSpan(this.m_groupState, 2);
            this.m_groupState.Size = new System.Drawing.Size(277, 411);
            this.m_groupState.TabIndex = 1372;
            this.m_groupState.TabStop = false;
            this.m_groupState.Text = "MOTOR STATE";
            this.m_groupState.TextAlign = Sys3Controls.EN_TEXTALIGN.MIDDLE_CENTER;
            this.m_groupState.ThemeIndex = 0;
            this.m_groupState.UseLabelBorder = true;
            this.m_groupState.UseTitle = true;
            // 
            // _tableLayoutPanel_GantryConfiguration
            // 
            this._tableLayoutPanel_GantryConfiguration.ColumnCount = 4;
            this._tableLayoutPanel_GantryConfiguration.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 25F));
            this._tableLayoutPanel_GantryConfiguration.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this._tableLayoutPanel_GantryConfiguration.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 25F));
            this._tableLayoutPanel_GantryConfiguration.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this._tableLayoutPanel_GantryConfiguration.Controls.Add(this.m_ledMotorOn, 0, 0);
            this._tableLayoutPanel_GantryConfiguration.Controls.Add(this.m_ledAlarm, 0, 1);
            this._tableLayoutPanel_GantryConfiguration.Controls.Add(this.m_lblMotorOn, 1, 0);
            this._tableLayoutPanel_GantryConfiguration.Controls.Add(this.m_lblAlarm, 1, 1);
            this._tableLayoutPanel_GantryConfiguration.Controls.Add(this.m_ledLimitNegative, 0, 2);
            this._tableLayoutPanel_GantryConfiguration.Controls.Add(this.m_lblLimitNegative, 1, 2);
            this._tableLayoutPanel_GantryConfiguration.Controls.Add(this.m_ledLimitPositive, 2, 2);
            this._tableLayoutPanel_GantryConfiguration.Controls.Add(this.m_lblLimitPositive, 3, 2);
            this._tableLayoutPanel_GantryConfiguration.Controls.Add(this.m_ledSWLimitNegative, 0, 3);
            this._tableLayoutPanel_GantryConfiguration.Controls.Add(this.m_lblSWLimitNegative, 1, 3);
            this._tableLayoutPanel_GantryConfiguration.Controls.Add(this.m_ledSWLimitPositive, 2, 3);
            this._tableLayoutPanel_GantryConfiguration.Controls.Add(this.m_lblSWLimitPositive, 3, 3);
            this._tableLayoutPanel_GantryConfiguration.Controls.Add(this.m_lblInterlock, 1, 9);
            this._tableLayoutPanel_GantryConfiguration.Controls.Add(this.m_lblHomeSwitch, 1, 6);
            this._tableLayoutPanel_GantryConfiguration.Controls.Add(this.m_lblHomeTimeout, 1, 8);
            this._tableLayoutPanel_GantryConfiguration.Controls.Add(this.m_lblMotionDone, 1, 4);
            this._tableLayoutPanel_GantryConfiguration.Controls.Add(this.m_lblHomeEnd, 1, 7);
            this._tableLayoutPanel_GantryConfiguration.Controls.Add(this.m_ledMotionDone, 0, 4);
            this._tableLayoutPanel_GantryConfiguration.Controls.Add(this.m_lblMotionTimeout, 1, 5);
            this._tableLayoutPanel_GantryConfiguration.Controls.Add(this.m_ledMotionTimeout, 0, 5);
            this._tableLayoutPanel_GantryConfiguration.Controls.Add(this.m_ledHomeSwitch, 0, 6);
            this._tableLayoutPanel_GantryConfiguration.Controls.Add(this.m_ledHomeEnd, 0, 7);
            this._tableLayoutPanel_GantryConfiguration.Controls.Add(this.m_ledHomeTimeout, 0, 8);
            this._tableLayoutPanel_GantryConfiguration.Controls.Add(this.m_ledInterlock, 0, 9);
            this._tableLayoutPanel_GantryConfiguration.Dock = System.Windows.Forms.DockStyle.Fill;
            this._tableLayoutPanel_GantryConfiguration.Location = new System.Drawing.Point(3, 32);
            this._tableLayoutPanel_GantryConfiguration.Margin = new System.Windows.Forms.Padding(0);
            this._tableLayoutPanel_GantryConfiguration.Name = "_tableLayoutPanel_GantryConfiguration";
            this._tableLayoutPanel_GantryConfiguration.Padding = new System.Windows.Forms.Padding(3);
            this._tableLayoutPanel_GantryConfiguration.RowCount = 10;
            this._tableLayoutPanel_GantryConfiguration.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 10F));
            this._tableLayoutPanel_GantryConfiguration.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 10F));
            this._tableLayoutPanel_GantryConfiguration.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 10F));
            this._tableLayoutPanel_GantryConfiguration.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 10F));
            this._tableLayoutPanel_GantryConfiguration.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 10F));
            this._tableLayoutPanel_GantryConfiguration.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 10F));
            this._tableLayoutPanel_GantryConfiguration.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 10F));
            this._tableLayoutPanel_GantryConfiguration.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 10F));
            this._tableLayoutPanel_GantryConfiguration.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 10F));
            this._tableLayoutPanel_GantryConfiguration.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 10F));
            this._tableLayoutPanel_GantryConfiguration.Size = new System.Drawing.Size(271, 376);
            this._tableLayoutPanel_GantryConfiguration.TabIndex = 1399;
            // 
            // m_lblMotorOn
            // 
            this.m_lblMotorOn.BackGroundColor = System.Drawing.Color.FromArgb(((int)(((byte)(200)))), ((int)(((byte)(200)))), ((int)(((byte)(220)))));
            this.m_lblMotorOn.BorderStroke = 2;
            this.m_lblMotorOn.BorderStyle = System.Windows.Forms.ButtonBorderStyle.Solid;
            this._tableLayoutPanel_GantryConfiguration.SetColumnSpan(this.m_lblMotorOn, 3);
            this.m_lblMotorOn.Description = "";
            this.m_lblMotorOn.DisabledColor = System.Drawing.Color.DarkGray;
            this.m_lblMotorOn.Dock = System.Windows.Forms.DockStyle.Fill;
            this.m_lblMotorOn.EdgeRadius = 1;
            this.m_lblMotorOn.ImagePosition = new System.Drawing.Point(0, 0);
            this.m_lblMotorOn.ImageSize = new System.Drawing.Point(0, 0);
            this.m_lblMotorOn.LoadImage = null;
            this.m_lblMotorOn.Location = new System.Drawing.Point(28, 6);
            this.m_lblMotorOn.MainFont = new System.Drawing.Font("맑은 고딕", 9F, System.Drawing.FontStyle.Bold);
            this.m_lblMotorOn.MainFontColor = System.Drawing.Color.Black;
            this.m_lblMotorOn.Margin = new System.Windows.Forms.Padding(0, 3, 0, 3);
            this.m_lblMotorOn.Name = "m_lblMotorOn";
            this.m_lblMotorOn.Size = new System.Drawing.Size(240, 31);
            this.m_lblMotorOn.SubFont = new System.Drawing.Font("맑은 고딕", 9F, System.Drawing.FontStyle.Bold);
            this.m_lblMotorOn.SubFontColor = System.Drawing.Color.DarkBlue;
            this.m_lblMotorOn.SubText = "ACTUAL";
            this.m_lblMotorOn.TabIndex = 13833;
            this.m_lblMotorOn.Text = "MOTOR ON";
            this.m_lblMotorOn.TextAlignMain = Sys3Controls.EN_TEXTALIGN.BOTTOM_RIGHT;
            this.m_lblMotorOn.TextAlignSub = Sys3Controls.EN_TEXTALIGN.BOTTOM_RIGHT;
            this.m_lblMotorOn.TextAlignUnit = Sys3Controls.EN_TEXTALIGN.MIDDLE_CENTER;
            this.m_lblMotorOn.ThemeIndex = 0;
            this.m_lblMotorOn.UnitAreaRate = 30;
            this.m_lblMotorOn.UnitFont = new System.Drawing.Font("맑은 고딕", 10F, System.Drawing.FontStyle.Bold);
            this.m_lblMotorOn.UnitFontColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.m_lblMotorOn.UnitPositionVertical = false;
            this.m_lblMotorOn.UnitText = "";
            this.m_lblMotorOn.UseBorder = true;
            this.m_lblMotorOn.UseEdgeRadius = false;
            this.m_lblMotorOn.UseImage = false;
            this.m_lblMotorOn.UseSubFont = false;
            this.m_lblMotorOn.UseUnitFont = false;
            // 
            // m_lblAlarm
            // 
            this.m_lblAlarm.BackGroundColor = System.Drawing.Color.FromArgb(((int)(((byte)(200)))), ((int)(((byte)(200)))), ((int)(((byte)(220)))));
            this.m_lblAlarm.BorderStroke = 2;
            this.m_lblAlarm.BorderStyle = System.Windows.Forms.ButtonBorderStyle.Solid;
            this._tableLayoutPanel_GantryConfiguration.SetColumnSpan(this.m_lblAlarm, 3);
            this.m_lblAlarm.Description = "";
            this.m_lblAlarm.DisabledColor = System.Drawing.Color.DarkGray;
            this.m_lblAlarm.Dock = System.Windows.Forms.DockStyle.Fill;
            this.m_lblAlarm.EdgeRadius = 1;
            this.m_lblAlarm.ImagePosition = new System.Drawing.Point(0, 0);
            this.m_lblAlarm.ImageSize = new System.Drawing.Point(0, 0);
            this.m_lblAlarm.LoadImage = null;
            this.m_lblAlarm.Location = new System.Drawing.Point(28, 43);
            this.m_lblAlarm.MainFont = new System.Drawing.Font("맑은 고딕", 9F, System.Drawing.FontStyle.Bold);
            this.m_lblAlarm.MainFontColor = System.Drawing.Color.Black;
            this.m_lblAlarm.Margin = new System.Windows.Forms.Padding(0, 3, 0, 3);
            this.m_lblAlarm.Name = "m_lblAlarm";
            this.m_lblAlarm.Size = new System.Drawing.Size(240, 31);
            this.m_lblAlarm.SubFont = new System.Drawing.Font("맑은 고딕", 9F, System.Drawing.FontStyle.Bold);
            this.m_lblAlarm.SubFontColor = System.Drawing.Color.DarkBlue;
            this.m_lblAlarm.SubText = "ACTUAL";
            this.m_lblAlarm.TabIndex = 13833;
            this.m_lblAlarm.Text = "ALARM";
            this.m_lblAlarm.TextAlignMain = Sys3Controls.EN_TEXTALIGN.BOTTOM_RIGHT;
            this.m_lblAlarm.TextAlignSub = Sys3Controls.EN_TEXTALIGN.BOTTOM_RIGHT;
            this.m_lblAlarm.TextAlignUnit = Sys3Controls.EN_TEXTALIGN.MIDDLE_CENTER;
            this.m_lblAlarm.ThemeIndex = 0;
            this.m_lblAlarm.UnitAreaRate = 30;
            this.m_lblAlarm.UnitFont = new System.Drawing.Font("맑은 고딕", 10F, System.Drawing.FontStyle.Bold);
            this.m_lblAlarm.UnitFontColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.m_lblAlarm.UnitPositionVertical = false;
            this.m_lblAlarm.UnitText = "";
            this.m_lblAlarm.UseBorder = true;
            this.m_lblAlarm.UseEdgeRadius = false;
            this.m_lblAlarm.UseImage = false;
            this.m_lblAlarm.UseSubFont = false;
            this.m_lblAlarm.UseUnitFont = false;
            // 
            // m_lblLimitNegative
            // 
            this.m_lblLimitNegative.BackGroundColor = System.Drawing.Color.FromArgb(((int)(((byte)(200)))), ((int)(((byte)(200)))), ((int)(((byte)(220)))));
            this.m_lblLimitNegative.BorderStroke = 2;
            this.m_lblLimitNegative.BorderStyle = System.Windows.Forms.ButtonBorderStyle.Solid;
            this.m_lblLimitNegative.Description = "";
            this.m_lblLimitNegative.DisabledColor = System.Drawing.Color.DarkGray;
            this.m_lblLimitNegative.Dock = System.Windows.Forms.DockStyle.Fill;
            this.m_lblLimitNegative.EdgeRadius = 1;
            this.m_lblLimitNegative.ImagePosition = new System.Drawing.Point(0, 0);
            this.m_lblLimitNegative.ImageSize = new System.Drawing.Point(0, 0);
            this.m_lblLimitNegative.LoadImage = null;
            this.m_lblLimitNegative.Location = new System.Drawing.Point(28, 80);
            this.m_lblLimitNegative.MainFont = new System.Drawing.Font("맑은 고딕", 9F, System.Drawing.FontStyle.Bold);
            this.m_lblLimitNegative.MainFontColor = System.Drawing.Color.Black;
            this.m_lblLimitNegative.Margin = new System.Windows.Forms.Padding(0, 3, 2, 3);
            this.m_lblLimitNegative.Name = "m_lblLimitNegative";
            this.m_lblLimitNegative.Size = new System.Drawing.Size(105, 31);
            this.m_lblLimitNegative.SubFont = new System.Drawing.Font("맑은 고딕", 9F, System.Drawing.FontStyle.Bold);
            this.m_lblLimitNegative.SubFontColor = System.Drawing.Color.DarkBlue;
            this.m_lblLimitNegative.SubText = "ACTUAL";
            this.m_lblLimitNegative.TabIndex = 13833;
            this.m_lblLimitNegative.Text = "LIMIT (-)";
            this.m_lblLimitNegative.TextAlignMain = Sys3Controls.EN_TEXTALIGN.BOTTOM_RIGHT;
            this.m_lblLimitNegative.TextAlignSub = Sys3Controls.EN_TEXTALIGN.BOTTOM_RIGHT;
            this.m_lblLimitNegative.TextAlignUnit = Sys3Controls.EN_TEXTALIGN.MIDDLE_CENTER;
            this.m_lblLimitNegative.ThemeIndex = 0;
            this.m_lblLimitNegative.UnitAreaRate = 30;
            this.m_lblLimitNegative.UnitFont = new System.Drawing.Font("맑은 고딕", 10F, System.Drawing.FontStyle.Bold);
            this.m_lblLimitNegative.UnitFontColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.m_lblLimitNegative.UnitPositionVertical = false;
            this.m_lblLimitNegative.UnitText = "";
            this.m_lblLimitNegative.UseBorder = true;
            this.m_lblLimitNegative.UseEdgeRadius = false;
            this.m_lblLimitNegative.UseImage = false;
            this.m_lblLimitNegative.UseSubFont = false;
            this.m_lblLimitNegative.UseUnitFont = false;
            // 
            // m_lblLimitPositive
            // 
            this.m_lblLimitPositive.BackGroundColor = System.Drawing.Color.FromArgb(((int)(((byte)(200)))), ((int)(((byte)(200)))), ((int)(((byte)(220)))));
            this.m_lblLimitPositive.BorderStroke = 2;
            this.m_lblLimitPositive.BorderStyle = System.Windows.Forms.ButtonBorderStyle.Solid;
            this.m_lblLimitPositive.Description = "";
            this.m_lblLimitPositive.DisabledColor = System.Drawing.Color.DarkGray;
            this.m_lblLimitPositive.Dock = System.Windows.Forms.DockStyle.Fill;
            this.m_lblLimitPositive.EdgeRadius = 1;
            this.m_lblLimitPositive.ImagePosition = new System.Drawing.Point(0, 0);
            this.m_lblLimitPositive.ImageSize = new System.Drawing.Point(0, 0);
            this.m_lblLimitPositive.LoadImage = null;
            this.m_lblLimitPositive.Location = new System.Drawing.Point(160, 80);
            this.m_lblLimitPositive.MainFont = new System.Drawing.Font("맑은 고딕", 9F, System.Drawing.FontStyle.Bold);
            this.m_lblLimitPositive.MainFontColor = System.Drawing.Color.Black;
            this.m_lblLimitPositive.Margin = new System.Windows.Forms.Padding(0, 3, 0, 3);
            this.m_lblLimitPositive.Name = "m_lblLimitPositive";
            this.m_lblLimitPositive.Size = new System.Drawing.Size(108, 31);
            this.m_lblLimitPositive.SubFont = new System.Drawing.Font("맑은 고딕", 9F, System.Drawing.FontStyle.Bold);
            this.m_lblLimitPositive.SubFontColor = System.Drawing.Color.DarkBlue;
            this.m_lblLimitPositive.SubText = "ACTUAL";
            this.m_lblLimitPositive.TabIndex = 13833;
            this.m_lblLimitPositive.Text = "LIMIT (+)";
            this.m_lblLimitPositive.TextAlignMain = Sys3Controls.EN_TEXTALIGN.BOTTOM_RIGHT;
            this.m_lblLimitPositive.TextAlignSub = Sys3Controls.EN_TEXTALIGN.BOTTOM_RIGHT;
            this.m_lblLimitPositive.TextAlignUnit = Sys3Controls.EN_TEXTALIGN.MIDDLE_CENTER;
            this.m_lblLimitPositive.ThemeIndex = 0;
            this.m_lblLimitPositive.UnitAreaRate = 30;
            this.m_lblLimitPositive.UnitFont = new System.Drawing.Font("맑은 고딕", 10F, System.Drawing.FontStyle.Bold);
            this.m_lblLimitPositive.UnitFontColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.m_lblLimitPositive.UnitPositionVertical = false;
            this.m_lblLimitPositive.UnitText = "";
            this.m_lblLimitPositive.UseBorder = true;
            this.m_lblLimitPositive.UseEdgeRadius = false;
            this.m_lblLimitPositive.UseImage = false;
            this.m_lblLimitPositive.UseSubFont = false;
            this.m_lblLimitPositive.UseUnitFont = false;
            // 
            // m_lblSWLimitNegative
            // 
            this.m_lblSWLimitNegative.BackGroundColor = System.Drawing.Color.FromArgb(((int)(((byte)(200)))), ((int)(((byte)(200)))), ((int)(((byte)(220)))));
            this.m_lblSWLimitNegative.BorderStroke = 2;
            this.m_lblSWLimitNegative.BorderStyle = System.Windows.Forms.ButtonBorderStyle.Solid;
            this.m_lblSWLimitNegative.Description = "";
            this.m_lblSWLimitNegative.DisabledColor = System.Drawing.Color.DarkGray;
            this.m_lblSWLimitNegative.Dock = System.Windows.Forms.DockStyle.Fill;
            this.m_lblSWLimitNegative.EdgeRadius = 1;
            this.m_lblSWLimitNegative.ImagePosition = new System.Drawing.Point(0, 0);
            this.m_lblSWLimitNegative.ImageSize = new System.Drawing.Point(0, 0);
            this.m_lblSWLimitNegative.LoadImage = null;
            this.m_lblSWLimitNegative.Location = new System.Drawing.Point(28, 117);
            this.m_lblSWLimitNegative.MainFont = new System.Drawing.Font("맑은 고딕", 9F, System.Drawing.FontStyle.Bold);
            this.m_lblSWLimitNegative.MainFontColor = System.Drawing.Color.Black;
            this.m_lblSWLimitNegative.Margin = new System.Windows.Forms.Padding(0, 3, 2, 3);
            this.m_lblSWLimitNegative.Name = "m_lblSWLimitNegative";
            this.m_lblSWLimitNegative.Size = new System.Drawing.Size(105, 31);
            this.m_lblSWLimitNegative.SubFont = new System.Drawing.Font("맑은 고딕", 9F, System.Drawing.FontStyle.Bold);
            this.m_lblSWLimitNegative.SubFontColor = System.Drawing.Color.DarkBlue;
            this.m_lblSWLimitNegative.SubText = "ACTUAL";
            this.m_lblSWLimitNegative.TabIndex = 13833;
            this.m_lblSWLimitNegative.Text = "SW LIMIT (-)";
            this.m_lblSWLimitNegative.TextAlignMain = Sys3Controls.EN_TEXTALIGN.BOTTOM_RIGHT;
            this.m_lblSWLimitNegative.TextAlignSub = Sys3Controls.EN_TEXTALIGN.BOTTOM_RIGHT;
            this.m_lblSWLimitNegative.TextAlignUnit = Sys3Controls.EN_TEXTALIGN.MIDDLE_CENTER;
            this.m_lblSWLimitNegative.ThemeIndex = 0;
            this.m_lblSWLimitNegative.UnitAreaRate = 30;
            this.m_lblSWLimitNegative.UnitFont = new System.Drawing.Font("맑은 고딕", 10F, System.Drawing.FontStyle.Bold);
            this.m_lblSWLimitNegative.UnitFontColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.m_lblSWLimitNegative.UnitPositionVertical = false;
            this.m_lblSWLimitNegative.UnitText = "";
            this.m_lblSWLimitNegative.UseBorder = true;
            this.m_lblSWLimitNegative.UseEdgeRadius = false;
            this.m_lblSWLimitNegative.UseImage = false;
            this.m_lblSWLimitNegative.UseSubFont = false;
            this.m_lblSWLimitNegative.UseUnitFont = false;
            // 
            // m_lblSWLimitPositive
            // 
            this.m_lblSWLimitPositive.BackGroundColor = System.Drawing.Color.FromArgb(((int)(((byte)(200)))), ((int)(((byte)(200)))), ((int)(((byte)(220)))));
            this.m_lblSWLimitPositive.BorderStroke = 2;
            this.m_lblSWLimitPositive.BorderStyle = System.Windows.Forms.ButtonBorderStyle.Solid;
            this.m_lblSWLimitPositive.Description = "";
            this.m_lblSWLimitPositive.DisabledColor = System.Drawing.Color.DarkGray;
            this.m_lblSWLimitPositive.Dock = System.Windows.Forms.DockStyle.Fill;
            this.m_lblSWLimitPositive.EdgeRadius = 1;
            this.m_lblSWLimitPositive.ImagePosition = new System.Drawing.Point(0, 0);
            this.m_lblSWLimitPositive.ImageSize = new System.Drawing.Point(0, 0);
            this.m_lblSWLimitPositive.LoadImage = null;
            this.m_lblSWLimitPositive.Location = new System.Drawing.Point(160, 117);
            this.m_lblSWLimitPositive.MainFont = new System.Drawing.Font("맑은 고딕", 9F, System.Drawing.FontStyle.Bold);
            this.m_lblSWLimitPositive.MainFontColor = System.Drawing.Color.Black;
            this.m_lblSWLimitPositive.Margin = new System.Windows.Forms.Padding(0, 3, 0, 3);
            this.m_lblSWLimitPositive.Name = "m_lblSWLimitPositive";
            this.m_lblSWLimitPositive.Size = new System.Drawing.Size(108, 31);
            this.m_lblSWLimitPositive.SubFont = new System.Drawing.Font("맑은 고딕", 9F, System.Drawing.FontStyle.Bold);
            this.m_lblSWLimitPositive.SubFontColor = System.Drawing.Color.DarkBlue;
            this.m_lblSWLimitPositive.SubText = "ACTUAL";
            this.m_lblSWLimitPositive.TabIndex = 13833;
            this.m_lblSWLimitPositive.Text = "SW LIMIT (+)";
            this.m_lblSWLimitPositive.TextAlignMain = Sys3Controls.EN_TEXTALIGN.BOTTOM_RIGHT;
            this.m_lblSWLimitPositive.TextAlignSub = Sys3Controls.EN_TEXTALIGN.BOTTOM_RIGHT;
            this.m_lblSWLimitPositive.TextAlignUnit = Sys3Controls.EN_TEXTALIGN.MIDDLE_CENTER;
            this.m_lblSWLimitPositive.ThemeIndex = 0;
            this.m_lblSWLimitPositive.UnitAreaRate = 30;
            this.m_lblSWLimitPositive.UnitFont = new System.Drawing.Font("맑은 고딕", 10F, System.Drawing.FontStyle.Bold);
            this.m_lblSWLimitPositive.UnitFontColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.m_lblSWLimitPositive.UnitPositionVertical = false;
            this.m_lblSWLimitPositive.UnitText = "";
            this.m_lblSWLimitPositive.UseBorder = true;
            this.m_lblSWLimitPositive.UseEdgeRadius = false;
            this.m_lblSWLimitPositive.UseImage = false;
            this.m_lblSWLimitPositive.UseSubFont = false;
            this.m_lblSWLimitPositive.UseUnitFont = false;
            // 
            // m_lblInterlock
            // 
            this.m_lblInterlock.BackGroundColor = System.Drawing.Color.FromArgb(((int)(((byte)(200)))), ((int)(((byte)(200)))), ((int)(((byte)(220)))));
            this.m_lblInterlock.BorderStroke = 2;
            this.m_lblInterlock.BorderStyle = System.Windows.Forms.ButtonBorderStyle.Solid;
            this._tableLayoutPanel_GantryConfiguration.SetColumnSpan(this.m_lblInterlock, 3);
            this.m_lblInterlock.Description = "";
            this.m_lblInterlock.DisabledColor = System.Drawing.Color.DarkGray;
            this.m_lblInterlock.Dock = System.Windows.Forms.DockStyle.Fill;
            this.m_lblInterlock.EdgeRadius = 1;
            this.m_lblInterlock.ImagePosition = new System.Drawing.Point(0, 0);
            this.m_lblInterlock.ImageSize = new System.Drawing.Point(0, 0);
            this.m_lblInterlock.LoadImage = null;
            this.m_lblInterlock.Location = new System.Drawing.Point(28, 339);
            this.m_lblInterlock.MainFont = new System.Drawing.Font("맑은 고딕", 9F, System.Drawing.FontStyle.Bold);
            this.m_lblInterlock.MainFontColor = System.Drawing.Color.Black;
            this.m_lblInterlock.Margin = new System.Windows.Forms.Padding(0, 3, 0, 3);
            this.m_lblInterlock.Name = "m_lblInterlock";
            this.m_lblInterlock.Size = new System.Drawing.Size(240, 31);
            this.m_lblInterlock.SubFont = new System.Drawing.Font("맑은 고딕", 9F, System.Drawing.FontStyle.Bold);
            this.m_lblInterlock.SubFontColor = System.Drawing.Color.DarkBlue;
            this.m_lblInterlock.SubText = "ACTUAL";
            this.m_lblInterlock.TabIndex = 13833;
            this.m_lblInterlock.Text = "INTERLOCK";
            this.m_lblInterlock.TextAlignMain = Sys3Controls.EN_TEXTALIGN.BOTTOM_RIGHT;
            this.m_lblInterlock.TextAlignSub = Sys3Controls.EN_TEXTALIGN.BOTTOM_RIGHT;
            this.m_lblInterlock.TextAlignUnit = Sys3Controls.EN_TEXTALIGN.MIDDLE_CENTER;
            this.m_lblInterlock.ThemeIndex = 0;
            this.m_lblInterlock.UnitAreaRate = 30;
            this.m_lblInterlock.UnitFont = new System.Drawing.Font("맑은 고딕", 10F, System.Drawing.FontStyle.Bold);
            this.m_lblInterlock.UnitFontColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.m_lblInterlock.UnitPositionVertical = false;
            this.m_lblInterlock.UnitText = "";
            this.m_lblInterlock.UseBorder = true;
            this.m_lblInterlock.UseEdgeRadius = false;
            this.m_lblInterlock.UseImage = false;
            this.m_lblInterlock.UseSubFont = false;
            this.m_lblInterlock.UseUnitFont = false;
            // 
            // m_lblHomeSwitch
            // 
            this.m_lblHomeSwitch.BackGroundColor = System.Drawing.Color.FromArgb(((int)(((byte)(200)))), ((int)(((byte)(200)))), ((int)(((byte)(220)))));
            this.m_lblHomeSwitch.BorderStroke = 2;
            this.m_lblHomeSwitch.BorderStyle = System.Windows.Forms.ButtonBorderStyle.Solid;
            this._tableLayoutPanel_GantryConfiguration.SetColumnSpan(this.m_lblHomeSwitch, 3);
            this.m_lblHomeSwitch.Description = "";
            this.m_lblHomeSwitch.DisabledColor = System.Drawing.Color.DarkGray;
            this.m_lblHomeSwitch.Dock = System.Windows.Forms.DockStyle.Fill;
            this.m_lblHomeSwitch.EdgeRadius = 1;
            this.m_lblHomeSwitch.ImagePosition = new System.Drawing.Point(0, 0);
            this.m_lblHomeSwitch.ImageSize = new System.Drawing.Point(0, 0);
            this.m_lblHomeSwitch.LoadImage = null;
            this.m_lblHomeSwitch.Location = new System.Drawing.Point(28, 228);
            this.m_lblHomeSwitch.MainFont = new System.Drawing.Font("맑은 고딕", 9F, System.Drawing.FontStyle.Bold);
            this.m_lblHomeSwitch.MainFontColor = System.Drawing.Color.Black;
            this.m_lblHomeSwitch.Margin = new System.Windows.Forms.Padding(0, 3, 0, 3);
            this.m_lblHomeSwitch.Name = "m_lblHomeSwitch";
            this.m_lblHomeSwitch.Size = new System.Drawing.Size(240, 31);
            this.m_lblHomeSwitch.SubFont = new System.Drawing.Font("맑은 고딕", 9F, System.Drawing.FontStyle.Bold);
            this.m_lblHomeSwitch.SubFontColor = System.Drawing.Color.DarkBlue;
            this.m_lblHomeSwitch.SubText = "ACTUAL";
            this.m_lblHomeSwitch.TabIndex = 13833;
            this.m_lblHomeSwitch.Text = "HOME SWITCH";
            this.m_lblHomeSwitch.TextAlignMain = Sys3Controls.EN_TEXTALIGN.BOTTOM_RIGHT;
            this.m_lblHomeSwitch.TextAlignSub = Sys3Controls.EN_TEXTALIGN.BOTTOM_RIGHT;
            this.m_lblHomeSwitch.TextAlignUnit = Sys3Controls.EN_TEXTALIGN.MIDDLE_CENTER;
            this.m_lblHomeSwitch.ThemeIndex = 0;
            this.m_lblHomeSwitch.UnitAreaRate = 30;
            this.m_lblHomeSwitch.UnitFont = new System.Drawing.Font("맑은 고딕", 10F, System.Drawing.FontStyle.Bold);
            this.m_lblHomeSwitch.UnitFontColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.m_lblHomeSwitch.UnitPositionVertical = false;
            this.m_lblHomeSwitch.UnitText = "";
            this.m_lblHomeSwitch.UseBorder = true;
            this.m_lblHomeSwitch.UseEdgeRadius = false;
            this.m_lblHomeSwitch.UseImage = false;
            this.m_lblHomeSwitch.UseSubFont = false;
            this.m_lblHomeSwitch.UseUnitFont = false;
            // 
            // m_lblHomeTimeout
            // 
            this.m_lblHomeTimeout.BackGroundColor = System.Drawing.Color.FromArgb(((int)(((byte)(200)))), ((int)(((byte)(200)))), ((int)(((byte)(220)))));
            this.m_lblHomeTimeout.BorderStroke = 2;
            this.m_lblHomeTimeout.BorderStyle = System.Windows.Forms.ButtonBorderStyle.Solid;
            this._tableLayoutPanel_GantryConfiguration.SetColumnSpan(this.m_lblHomeTimeout, 3);
            this.m_lblHomeTimeout.Description = "";
            this.m_lblHomeTimeout.DisabledColor = System.Drawing.Color.DarkGray;
            this.m_lblHomeTimeout.Dock = System.Windows.Forms.DockStyle.Fill;
            this.m_lblHomeTimeout.EdgeRadius = 1;
            this.m_lblHomeTimeout.ImagePosition = new System.Drawing.Point(0, 0);
            this.m_lblHomeTimeout.ImageSize = new System.Drawing.Point(0, 0);
            this.m_lblHomeTimeout.LoadImage = null;
            this.m_lblHomeTimeout.Location = new System.Drawing.Point(28, 302);
            this.m_lblHomeTimeout.MainFont = new System.Drawing.Font("맑은 고딕", 9F, System.Drawing.FontStyle.Bold);
            this.m_lblHomeTimeout.MainFontColor = System.Drawing.Color.Black;
            this.m_lblHomeTimeout.Margin = new System.Windows.Forms.Padding(0, 3, 0, 3);
            this.m_lblHomeTimeout.Name = "m_lblHomeTimeout";
            this.m_lblHomeTimeout.Size = new System.Drawing.Size(240, 31);
            this.m_lblHomeTimeout.SubFont = new System.Drawing.Font("맑은 고딕", 9F, System.Drawing.FontStyle.Bold);
            this.m_lblHomeTimeout.SubFontColor = System.Drawing.Color.DarkBlue;
            this.m_lblHomeTimeout.SubText = "ACTUAL";
            this.m_lblHomeTimeout.TabIndex = 13833;
            this.m_lblHomeTimeout.Text = "HOME TIMEOUT";
            this.m_lblHomeTimeout.TextAlignMain = Sys3Controls.EN_TEXTALIGN.BOTTOM_RIGHT;
            this.m_lblHomeTimeout.TextAlignSub = Sys3Controls.EN_TEXTALIGN.BOTTOM_RIGHT;
            this.m_lblHomeTimeout.TextAlignUnit = Sys3Controls.EN_TEXTALIGN.MIDDLE_CENTER;
            this.m_lblHomeTimeout.ThemeIndex = 0;
            this.m_lblHomeTimeout.UnitAreaRate = 30;
            this.m_lblHomeTimeout.UnitFont = new System.Drawing.Font("맑은 고딕", 10F, System.Drawing.FontStyle.Bold);
            this.m_lblHomeTimeout.UnitFontColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.m_lblHomeTimeout.UnitPositionVertical = false;
            this.m_lblHomeTimeout.UnitText = "";
            this.m_lblHomeTimeout.UseBorder = true;
            this.m_lblHomeTimeout.UseEdgeRadius = false;
            this.m_lblHomeTimeout.UseImage = false;
            this.m_lblHomeTimeout.UseSubFont = false;
            this.m_lblHomeTimeout.UseUnitFont = false;
            // 
            // m_lblMotionDone
            // 
            this.m_lblMotionDone.BackGroundColor = System.Drawing.Color.FromArgb(((int)(((byte)(200)))), ((int)(((byte)(200)))), ((int)(((byte)(220)))));
            this.m_lblMotionDone.BorderStroke = 2;
            this.m_lblMotionDone.BorderStyle = System.Windows.Forms.ButtonBorderStyle.Solid;
            this._tableLayoutPanel_GantryConfiguration.SetColumnSpan(this.m_lblMotionDone, 3);
            this.m_lblMotionDone.Description = "";
            this.m_lblMotionDone.DisabledColor = System.Drawing.Color.DarkGray;
            this.m_lblMotionDone.Dock = System.Windows.Forms.DockStyle.Fill;
            this.m_lblMotionDone.EdgeRadius = 1;
            this.m_lblMotionDone.ImagePosition = new System.Drawing.Point(0, 0);
            this.m_lblMotionDone.ImageSize = new System.Drawing.Point(0, 0);
            this.m_lblMotionDone.LoadImage = null;
            this.m_lblMotionDone.Location = new System.Drawing.Point(28, 154);
            this.m_lblMotionDone.MainFont = new System.Drawing.Font("맑은 고딕", 9F, System.Drawing.FontStyle.Bold);
            this.m_lblMotionDone.MainFontColor = System.Drawing.Color.Black;
            this.m_lblMotionDone.Margin = new System.Windows.Forms.Padding(0, 3, 0, 3);
            this.m_lblMotionDone.Name = "m_lblMotionDone";
            this.m_lblMotionDone.Size = new System.Drawing.Size(240, 31);
            this.m_lblMotionDone.SubFont = new System.Drawing.Font("맑은 고딕", 9F, System.Drawing.FontStyle.Bold);
            this.m_lblMotionDone.SubFontColor = System.Drawing.Color.DarkBlue;
            this.m_lblMotionDone.SubText = "ACTUAL";
            this.m_lblMotionDone.TabIndex = 13833;
            this.m_lblMotionDone.Text = "MOTION DONE";
            this.m_lblMotionDone.TextAlignMain = Sys3Controls.EN_TEXTALIGN.BOTTOM_RIGHT;
            this.m_lblMotionDone.TextAlignSub = Sys3Controls.EN_TEXTALIGN.BOTTOM_RIGHT;
            this.m_lblMotionDone.TextAlignUnit = Sys3Controls.EN_TEXTALIGN.MIDDLE_CENTER;
            this.m_lblMotionDone.ThemeIndex = 0;
            this.m_lblMotionDone.UnitAreaRate = 30;
            this.m_lblMotionDone.UnitFont = new System.Drawing.Font("맑은 고딕", 10F, System.Drawing.FontStyle.Bold);
            this.m_lblMotionDone.UnitFontColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.m_lblMotionDone.UnitPositionVertical = false;
            this.m_lblMotionDone.UnitText = "";
            this.m_lblMotionDone.UseBorder = true;
            this.m_lblMotionDone.UseEdgeRadius = false;
            this.m_lblMotionDone.UseImage = false;
            this.m_lblMotionDone.UseSubFont = false;
            this.m_lblMotionDone.UseUnitFont = false;
            // 
            // m_lblHomeEnd
            // 
            this.m_lblHomeEnd.BackGroundColor = System.Drawing.Color.FromArgb(((int)(((byte)(200)))), ((int)(((byte)(200)))), ((int)(((byte)(220)))));
            this.m_lblHomeEnd.BorderStroke = 2;
            this.m_lblHomeEnd.BorderStyle = System.Windows.Forms.ButtonBorderStyle.Solid;
            this._tableLayoutPanel_GantryConfiguration.SetColumnSpan(this.m_lblHomeEnd, 3);
            this.m_lblHomeEnd.Description = "";
            this.m_lblHomeEnd.DisabledColor = System.Drawing.Color.DarkGray;
            this.m_lblHomeEnd.Dock = System.Windows.Forms.DockStyle.Fill;
            this.m_lblHomeEnd.EdgeRadius = 1;
            this.m_lblHomeEnd.ImagePosition = new System.Drawing.Point(0, 0);
            this.m_lblHomeEnd.ImageSize = new System.Drawing.Point(0, 0);
            this.m_lblHomeEnd.LoadImage = null;
            this.m_lblHomeEnd.Location = new System.Drawing.Point(28, 265);
            this.m_lblHomeEnd.MainFont = new System.Drawing.Font("맑은 고딕", 9F, System.Drawing.FontStyle.Bold);
            this.m_lblHomeEnd.MainFontColor = System.Drawing.Color.Black;
            this.m_lblHomeEnd.Margin = new System.Windows.Forms.Padding(0, 3, 0, 3);
            this.m_lblHomeEnd.Name = "m_lblHomeEnd";
            this.m_lblHomeEnd.Size = new System.Drawing.Size(240, 31);
            this.m_lblHomeEnd.SubFont = new System.Drawing.Font("맑은 고딕", 9F, System.Drawing.FontStyle.Bold);
            this.m_lblHomeEnd.SubFontColor = System.Drawing.Color.DarkBlue;
            this.m_lblHomeEnd.SubText = "ACTUAL";
            this.m_lblHomeEnd.TabIndex = 13833;
            this.m_lblHomeEnd.Text = "HOME END";
            this.m_lblHomeEnd.TextAlignMain = Sys3Controls.EN_TEXTALIGN.BOTTOM_RIGHT;
            this.m_lblHomeEnd.TextAlignSub = Sys3Controls.EN_TEXTALIGN.BOTTOM_RIGHT;
            this.m_lblHomeEnd.TextAlignUnit = Sys3Controls.EN_TEXTALIGN.MIDDLE_CENTER;
            this.m_lblHomeEnd.ThemeIndex = 0;
            this.m_lblHomeEnd.UnitAreaRate = 30;
            this.m_lblHomeEnd.UnitFont = new System.Drawing.Font("맑은 고딕", 10F, System.Drawing.FontStyle.Bold);
            this.m_lblHomeEnd.UnitFontColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.m_lblHomeEnd.UnitPositionVertical = false;
            this.m_lblHomeEnd.UnitText = "";
            this.m_lblHomeEnd.UseBorder = true;
            this.m_lblHomeEnd.UseEdgeRadius = false;
            this.m_lblHomeEnd.UseImage = false;
            this.m_lblHomeEnd.UseSubFont = false;
            this.m_lblHomeEnd.UseUnitFont = false;
            // 
            // m_lblMotionTimeout
            // 
            this.m_lblMotionTimeout.BackGroundColor = System.Drawing.Color.FromArgb(((int)(((byte)(200)))), ((int)(((byte)(200)))), ((int)(((byte)(220)))));
            this.m_lblMotionTimeout.BorderStroke = 2;
            this.m_lblMotionTimeout.BorderStyle = System.Windows.Forms.ButtonBorderStyle.Solid;
            this._tableLayoutPanel_GantryConfiguration.SetColumnSpan(this.m_lblMotionTimeout, 3);
            this.m_lblMotionTimeout.Description = "";
            this.m_lblMotionTimeout.DisabledColor = System.Drawing.Color.DarkGray;
            this.m_lblMotionTimeout.Dock = System.Windows.Forms.DockStyle.Fill;
            this.m_lblMotionTimeout.EdgeRadius = 1;
            this.m_lblMotionTimeout.ImagePosition = new System.Drawing.Point(0, 0);
            this.m_lblMotionTimeout.ImageSize = new System.Drawing.Point(0, 0);
            this.m_lblMotionTimeout.LoadImage = null;
            this.m_lblMotionTimeout.Location = new System.Drawing.Point(28, 191);
            this.m_lblMotionTimeout.MainFont = new System.Drawing.Font("맑은 고딕", 9F, System.Drawing.FontStyle.Bold);
            this.m_lblMotionTimeout.MainFontColor = System.Drawing.Color.Black;
            this.m_lblMotionTimeout.Margin = new System.Windows.Forms.Padding(0, 3, 0, 3);
            this.m_lblMotionTimeout.Name = "m_lblMotionTimeout";
            this.m_lblMotionTimeout.Size = new System.Drawing.Size(240, 31);
            this.m_lblMotionTimeout.SubFont = new System.Drawing.Font("맑은 고딕", 9F, System.Drawing.FontStyle.Bold);
            this.m_lblMotionTimeout.SubFontColor = System.Drawing.Color.DarkBlue;
            this.m_lblMotionTimeout.SubText = "ACTUAL";
            this.m_lblMotionTimeout.TabIndex = 13833;
            this.m_lblMotionTimeout.Text = "MOTION TIMEOUT";
            this.m_lblMotionTimeout.TextAlignMain = Sys3Controls.EN_TEXTALIGN.BOTTOM_RIGHT;
            this.m_lblMotionTimeout.TextAlignSub = Sys3Controls.EN_TEXTALIGN.BOTTOM_RIGHT;
            this.m_lblMotionTimeout.TextAlignUnit = Sys3Controls.EN_TEXTALIGN.MIDDLE_CENTER;
            this.m_lblMotionTimeout.ThemeIndex = 0;
            this.m_lblMotionTimeout.UnitAreaRate = 30;
            this.m_lblMotionTimeout.UnitFont = new System.Drawing.Font("맑은 고딕", 10F, System.Drawing.FontStyle.Bold);
            this.m_lblMotionTimeout.UnitFontColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.m_lblMotionTimeout.UnitPositionVertical = false;
            this.m_lblMotionTimeout.UnitText = "";
            this.m_lblMotionTimeout.UseBorder = true;
            this.m_lblMotionTimeout.UseEdgeRadius = false;
            this.m_lblMotionTimeout.UseImage = false;
            this.m_lblMotionTimeout.UseSubFont = false;
            this.m_lblMotionTimeout.UseUnitFont = false;
            // 
            // m_ledInterlock
            // 
            this.m_ledInterlock.Active = false;
            this.m_ledInterlock.ColorActive = System.Drawing.Color.FromArgb(((int)(((byte)(75)))), ((int)(((byte)(216)))), ((int)(((byte)(101)))));
            this.m_ledInterlock.ColorNormal = System.Drawing.Color.DimGray;
            this.m_ledInterlock.Dock = System.Windows.Forms.DockStyle.Fill;
            this.m_ledInterlock.Location = new System.Drawing.Point(3, 339);
            this.m_ledInterlock.Margin = new System.Windows.Forms.Padding(0, 3, 0, 3);
            this.m_ledInterlock.Name = "m_ledInterlock";
            this.m_ledInterlock.Size = new System.Drawing.Size(25, 31);
            this.m_ledInterlock.TabIndex = 1151;
            // 
            // m_groupPosition
            // 
            this.m_groupPosition.BackGroundColor = System.Drawing.Color.WhiteSmoke;
            this.m_groupPosition.Controls.Add(this._tableLayoutPanel_Position);
            this.m_groupPosition.Dock = System.Windows.Forms.DockStyle.Fill;
            this.m_groupPosition.EdgeBorderStroke = 2;
            this.m_groupPosition.EdgeRadius = 2;
            this.m_groupPosition.LabelFont = new System.Drawing.Font("맑은 고딕", 10F, System.Drawing.FontStyle.Bold);
            this.m_groupPosition.LabelGradientColorFirst = System.Drawing.Color.FromArgb(((int)(((byte)(208)))), ((int)(((byte)(208)))), ((int)(((byte)(208)))));
            this.m_groupPosition.LabelGradientColorSecond = System.Drawing.Color.FromArgb(((int)(((byte)(240)))), ((int)(((byte)(240)))), ((int)(((byte)(240)))));
            this.m_groupPosition.LabelHeight = 30;
            this.m_groupPosition.LabelTextColor = System.Drawing.Color.Black;
            this.m_groupPosition.Location = new System.Drawing.Point(277, 0);
            this.m_groupPosition.Margin = new System.Windows.Forms.Padding(0);
            this.m_groupPosition.Name = "m_groupPosition";
            this.m_groupPosition.Padding = new System.Windows.Forms.Padding(3, 18, 3, 3);
            this.m_groupPosition.Size = new System.Drawing.Size(560, 175);
            this.m_groupPosition.TabIndex = 1373;
            this.m_groupPosition.TabStop = false;
            this.m_groupPosition.Text = "POSITION";
            this.m_groupPosition.TextAlign = Sys3Controls.EN_TEXTALIGN.MIDDLE_CENTER;
            this.m_groupPosition.ThemeIndex = 0;
            this.m_groupPosition.UseLabelBorder = true;
            this.m_groupPosition.UseTitle = true;
            // 
            // _tableLayoutPanel_Position
            // 
            this._tableLayoutPanel_Position.ColumnCount = 3;
            this._tableLayoutPanel_Position.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 27.70563F));
            this._tableLayoutPanel_Position.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 72.29437F));
            this._tableLayoutPanel_Position.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 100F));
            this._tableLayoutPanel_Position.Controls.Add(this.m_lblCommand, 0, 0);
            this._tableLayoutPanel_Position.Controls.Add(this.m_labelCommand, 1, 0);
            this._tableLayoutPanel_Position.Controls.Add(this.m_lblActual, 0, 1);
            this._tableLayoutPanel_Position.Controls.Add(this.m_lblError, 0, 2);
            this._tableLayoutPanel_Position.Controls.Add(this.m_labelActual, 1, 1);
            this._tableLayoutPanel_Position.Controls.Add(this.m_labelError, 1, 2);
            this._tableLayoutPanel_Position.Controls.Add(this.m_btnClear, 2, 0);
            this._tableLayoutPanel_Position.Dock = System.Windows.Forms.DockStyle.Fill;
            this._tableLayoutPanel_Position.Location = new System.Drawing.Point(3, 32);
            this._tableLayoutPanel_Position.Margin = new System.Windows.Forms.Padding(0);
            this._tableLayoutPanel_Position.Name = "_tableLayoutPanel_Position";
            this._tableLayoutPanel_Position.Padding = new System.Windows.Forms.Padding(5);
            this._tableLayoutPanel_Position.RowCount = 3;
            this._tableLayoutPanel_Position.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 33.33333F));
            this._tableLayoutPanel_Position.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 33.33333F));
            this._tableLayoutPanel_Position.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 33.33333F));
            this._tableLayoutPanel_Position.Size = new System.Drawing.Size(554, 140);
            this._tableLayoutPanel_Position.TabIndex = 0;
            // 
            // m_lblCommand
            // 
            this.m_lblCommand.BackGroundColor = System.Drawing.Color.FromArgb(((int)(((byte)(200)))), ((int)(((byte)(200)))), ((int)(((byte)(220)))));
            this.m_lblCommand.BorderStroke = 2;
            this.m_lblCommand.BorderStyle = System.Windows.Forms.ButtonBorderStyle.Solid;
            this.m_lblCommand.Description = "";
            this.m_lblCommand.DisabledColor = System.Drawing.Color.DarkGray;
            this.m_lblCommand.Dock = System.Windows.Forms.DockStyle.Fill;
            this.m_lblCommand.EdgeRadius = 1;
            this.m_lblCommand.ImagePosition = new System.Drawing.Point(0, 0);
            this.m_lblCommand.ImageSize = new System.Drawing.Point(0, 0);
            this.m_lblCommand.LoadImage = null;
            this.m_lblCommand.Location = new System.Drawing.Point(5, 8);
            this.m_lblCommand.MainFont = new System.Drawing.Font("맑은 고딕", 9F, System.Drawing.FontStyle.Bold);
            this.m_lblCommand.MainFontColor = System.Drawing.Color.Black;
            this.m_lblCommand.Margin = new System.Windows.Forms.Padding(0, 3, 0, 3);
            this.m_lblCommand.Name = "m_lblCommand";
            this.m_lblCommand.Size = new System.Drawing.Size(123, 37);
            this.m_lblCommand.SubFont = new System.Drawing.Font("맑은 고딕", 9F, System.Drawing.FontStyle.Bold);
            this.m_lblCommand.SubFontColor = System.Drawing.Color.DarkBlue;
            this.m_lblCommand.SubText = "COMMAND";
            this.m_lblCommand.TabIndex = 1377;
            this.m_lblCommand.Text = "POSITION";
            this.m_lblCommand.TextAlignMain = Sys3Controls.EN_TEXTALIGN.TOP_LEFT;
            this.m_lblCommand.TextAlignSub = Sys3Controls.EN_TEXTALIGN.BOTTOM_RIGHT;
            this.m_lblCommand.TextAlignUnit = Sys3Controls.EN_TEXTALIGN.MIDDLE_CENTER;
            this.m_lblCommand.ThemeIndex = 0;
            this.m_lblCommand.UnitAreaRate = 30;
            this.m_lblCommand.UnitFont = new System.Drawing.Font("맑은 고딕", 10F, System.Drawing.FontStyle.Bold);
            this.m_lblCommand.UnitFontColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.m_lblCommand.UnitPositionVertical = false;
            this.m_lblCommand.UnitText = "";
            this.m_lblCommand.UseBorder = true;
            this.m_lblCommand.UseEdgeRadius = false;
            this.m_lblCommand.UseImage = false;
            this.m_lblCommand.UseSubFont = true;
            this.m_lblCommand.UseUnitFont = false;
            // 
            // m_labelCommand
            // 
            this.m_labelCommand.BackGroundColor = System.Drawing.Color.White;
            this.m_labelCommand.BorderStroke = 2;
            this.m_labelCommand.BorderStyle = System.Windows.Forms.ButtonBorderStyle.Solid;
            this.m_labelCommand.Description = "";
            this.m_labelCommand.DisabledColor = System.Drawing.Color.LightGray;
            this.m_labelCommand.Dock = System.Windows.Forms.DockStyle.Fill;
            this.m_labelCommand.EdgeRadius = 1;
            this.m_labelCommand.Enabled = false;
            this.m_labelCommand.ImagePosition = new System.Drawing.Point(0, 0);
            this.m_labelCommand.ImageSize = new System.Drawing.Point(0, 0);
            this.m_labelCommand.LoadImage = null;
            this.m_labelCommand.Location = new System.Drawing.Point(130, 8);
            this.m_labelCommand.MainFont = new System.Drawing.Font("맑은 고딕", 11F, System.Drawing.FontStyle.Bold);
            this.m_labelCommand.MainFontColor = System.Drawing.Color.Black;
            this.m_labelCommand.Margin = new System.Windows.Forms.Padding(2, 3, 0, 3);
            this.m_labelCommand.Name = "m_labelCommand";
            this.m_labelCommand.Size = new System.Drawing.Size(318, 37);
            this.m_labelCommand.SubFont = new System.Drawing.Font("맑은 고딕", 10F);
            this.m_labelCommand.SubFontColor = System.Drawing.Color.Black;
            this.m_labelCommand.SubText = "";
            this.m_labelCommand.TabIndex = 1378;
            this.m_labelCommand.Text = "--";
            this.m_labelCommand.TextAlignMain = Sys3Controls.EN_TEXTALIGN.BOTTOM_RIGHT;
            this.m_labelCommand.TextAlignSub = Sys3Controls.EN_TEXTALIGN.MIDDLE_CENTER;
            this.m_labelCommand.TextAlignUnit = Sys3Controls.EN_TEXTALIGN.BOTTOM_RIGHT;
            this.m_labelCommand.ThemeIndex = 0;
            this.m_labelCommand.UnitAreaRate = 20;
            this.m_labelCommand.UnitFont = new System.Drawing.Font("맑은 고딕", 10F, System.Drawing.FontStyle.Bold);
            this.m_labelCommand.UnitFontColor = System.Drawing.Color.Black;
            this.m_labelCommand.UnitPositionVertical = false;
            this.m_labelCommand.UnitText = "mm";
            this.m_labelCommand.UseBorder = true;
            this.m_labelCommand.UseEdgeRadius = false;
            this.m_labelCommand.UseImage = false;
            this.m_labelCommand.UseSubFont = false;
            this.m_labelCommand.UseUnitFont = true;
            // 
            // m_lblActual
            // 
            this.m_lblActual.BackGroundColor = System.Drawing.Color.FromArgb(((int)(((byte)(200)))), ((int)(((byte)(200)))), ((int)(((byte)(220)))));
            this.m_lblActual.BorderStroke = 2;
            this.m_lblActual.BorderStyle = System.Windows.Forms.ButtonBorderStyle.Solid;
            this.m_lblActual.Description = "";
            this.m_lblActual.DisabledColor = System.Drawing.Color.DarkGray;
            this.m_lblActual.Dock = System.Windows.Forms.DockStyle.Fill;
            this.m_lblActual.EdgeRadius = 1;
            this.m_lblActual.ImagePosition = new System.Drawing.Point(0, 0);
            this.m_lblActual.ImageSize = new System.Drawing.Point(0, 0);
            this.m_lblActual.LoadImage = null;
            this.m_lblActual.Location = new System.Drawing.Point(5, 51);
            this.m_lblActual.MainFont = new System.Drawing.Font("맑은 고딕", 9F, System.Drawing.FontStyle.Bold);
            this.m_lblActual.MainFontColor = System.Drawing.Color.Black;
            this.m_lblActual.Margin = new System.Windows.Forms.Padding(0, 3, 0, 3);
            this.m_lblActual.Name = "m_lblActual";
            this.m_lblActual.Size = new System.Drawing.Size(123, 37);
            this.m_lblActual.SubFont = new System.Drawing.Font("맑은 고딕", 9F, System.Drawing.FontStyle.Bold);
            this.m_lblActual.SubFontColor = System.Drawing.Color.DarkBlue;
            this.m_lblActual.SubText = "ACTUAL";
            this.m_lblActual.TabIndex = 1377;
            this.m_lblActual.Text = "POSITION";
            this.m_lblActual.TextAlignMain = Sys3Controls.EN_TEXTALIGN.TOP_LEFT;
            this.m_lblActual.TextAlignSub = Sys3Controls.EN_TEXTALIGN.BOTTOM_RIGHT;
            this.m_lblActual.TextAlignUnit = Sys3Controls.EN_TEXTALIGN.MIDDLE_CENTER;
            this.m_lblActual.ThemeIndex = 0;
            this.m_lblActual.UnitAreaRate = 30;
            this.m_lblActual.UnitFont = new System.Drawing.Font("맑은 고딕", 10F, System.Drawing.FontStyle.Bold);
            this.m_lblActual.UnitFontColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.m_lblActual.UnitPositionVertical = false;
            this.m_lblActual.UnitText = "";
            this.m_lblActual.UseBorder = true;
            this.m_lblActual.UseEdgeRadius = false;
            this.m_lblActual.UseImage = false;
            this.m_lblActual.UseSubFont = true;
            this.m_lblActual.UseUnitFont = false;
            // 
            // m_lblError
            // 
            this.m_lblError.BackGroundColor = System.Drawing.Color.FromArgb(((int)(((byte)(200)))), ((int)(((byte)(200)))), ((int)(((byte)(220)))));
            this.m_lblError.BorderStroke = 2;
            this.m_lblError.BorderStyle = System.Windows.Forms.ButtonBorderStyle.Solid;
            this.m_lblError.Description = "";
            this.m_lblError.DisabledColor = System.Drawing.Color.DarkGray;
            this.m_lblError.Dock = System.Windows.Forms.DockStyle.Fill;
            this.m_lblError.EdgeRadius = 1;
            this.m_lblError.ImagePosition = new System.Drawing.Point(0, 0);
            this.m_lblError.ImageSize = new System.Drawing.Point(0, 0);
            this.m_lblError.LoadImage = null;
            this.m_lblError.Location = new System.Drawing.Point(5, 94);
            this.m_lblError.MainFont = new System.Drawing.Font("맑은 고딕", 9F, System.Drawing.FontStyle.Bold);
            this.m_lblError.MainFontColor = System.Drawing.Color.Black;
            this.m_lblError.Margin = new System.Windows.Forms.Padding(0, 3, 0, 3);
            this.m_lblError.Name = "m_lblError";
            this.m_lblError.Size = new System.Drawing.Size(123, 38);
            this.m_lblError.SubFont = new System.Drawing.Font("맑은 고딕", 9F, System.Drawing.FontStyle.Bold);
            this.m_lblError.SubFontColor = System.Drawing.Color.Red;
            this.m_lblError.SubText = "ERROR";
            this.m_lblError.TabIndex = 1377;
            this.m_lblError.Text = "POSITION";
            this.m_lblError.TextAlignMain = Sys3Controls.EN_TEXTALIGN.TOP_LEFT;
            this.m_lblError.TextAlignSub = Sys3Controls.EN_TEXTALIGN.BOTTOM_RIGHT;
            this.m_lblError.TextAlignUnit = Sys3Controls.EN_TEXTALIGN.MIDDLE_CENTER;
            this.m_lblError.ThemeIndex = 0;
            this.m_lblError.UnitAreaRate = 30;
            this.m_lblError.UnitFont = new System.Drawing.Font("맑은 고딕", 10F, System.Drawing.FontStyle.Bold);
            this.m_lblError.UnitFontColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.m_lblError.UnitPositionVertical = false;
            this.m_lblError.UnitText = "";
            this.m_lblError.UseBorder = true;
            this.m_lblError.UseEdgeRadius = false;
            this.m_lblError.UseImage = false;
            this.m_lblError.UseSubFont = true;
            this.m_lblError.UseUnitFont = false;
            // 
            // m_labelActual
            // 
            this.m_labelActual.BackGroundColor = System.Drawing.Color.White;
            this.m_labelActual.BorderStroke = 2;
            this.m_labelActual.BorderStyle = System.Windows.Forms.ButtonBorderStyle.Solid;
            this.m_labelActual.Description = "";
            this.m_labelActual.DisabledColor = System.Drawing.Color.LightGray;
            this.m_labelActual.Dock = System.Windows.Forms.DockStyle.Fill;
            this.m_labelActual.EdgeRadius = 1;
            this.m_labelActual.Enabled = false;
            this.m_labelActual.ImagePosition = new System.Drawing.Point(0, 0);
            this.m_labelActual.ImageSize = new System.Drawing.Point(0, 0);
            this.m_labelActual.LoadImage = null;
            this.m_labelActual.Location = new System.Drawing.Point(130, 51);
            this.m_labelActual.MainFont = new System.Drawing.Font("맑은 고딕", 11F, System.Drawing.FontStyle.Bold);
            this.m_labelActual.MainFontColor = System.Drawing.Color.Black;
            this.m_labelActual.Margin = new System.Windows.Forms.Padding(2, 3, 0, 3);
            this.m_labelActual.Name = "m_labelActual";
            this.m_labelActual.Size = new System.Drawing.Size(318, 37);
            this.m_labelActual.SubFont = new System.Drawing.Font("맑은 고딕", 10F);
            this.m_labelActual.SubFontColor = System.Drawing.Color.Black;
            this.m_labelActual.SubText = "";
            this.m_labelActual.TabIndex = 1378;
            this.m_labelActual.Text = "--";
            this.m_labelActual.TextAlignMain = Sys3Controls.EN_TEXTALIGN.BOTTOM_RIGHT;
            this.m_labelActual.TextAlignSub = Sys3Controls.EN_TEXTALIGN.MIDDLE_CENTER;
            this.m_labelActual.TextAlignUnit = Sys3Controls.EN_TEXTALIGN.BOTTOM_RIGHT;
            this.m_labelActual.ThemeIndex = 0;
            this.m_labelActual.UnitAreaRate = 20;
            this.m_labelActual.UnitFont = new System.Drawing.Font("맑은 고딕", 10F, System.Drawing.FontStyle.Bold);
            this.m_labelActual.UnitFontColor = System.Drawing.Color.Black;
            this.m_labelActual.UnitPositionVertical = false;
            this.m_labelActual.UnitText = "mm";
            this.m_labelActual.UseBorder = true;
            this.m_labelActual.UseEdgeRadius = false;
            this.m_labelActual.UseImage = false;
            this.m_labelActual.UseSubFont = false;
            this.m_labelActual.UseUnitFont = true;
            // 
            // m_labelError
            // 
            this.m_labelError.BackGroundColor = System.Drawing.Color.White;
            this.m_labelError.BorderStroke = 2;
            this.m_labelError.BorderStyle = System.Windows.Forms.ButtonBorderStyle.Solid;
            this.m_labelError.Description = "";
            this.m_labelError.DisabledColor = System.Drawing.Color.LightGray;
            this.m_labelError.Dock = System.Windows.Forms.DockStyle.Fill;
            this.m_labelError.EdgeRadius = 1;
            this.m_labelError.Enabled = false;
            this.m_labelError.ImagePosition = new System.Drawing.Point(0, 0);
            this.m_labelError.ImageSize = new System.Drawing.Point(0, 0);
            this.m_labelError.LoadImage = null;
            this.m_labelError.Location = new System.Drawing.Point(130, 94);
            this.m_labelError.MainFont = new System.Drawing.Font("맑은 고딕", 11F, System.Drawing.FontStyle.Bold);
            this.m_labelError.MainFontColor = System.Drawing.Color.Black;
            this.m_labelError.Margin = new System.Windows.Forms.Padding(2, 3, 0, 3);
            this.m_labelError.Name = "m_labelError";
            this.m_labelError.Size = new System.Drawing.Size(318, 38);
            this.m_labelError.SubFont = new System.Drawing.Font("맑은 고딕", 10F);
            this.m_labelError.SubFontColor = System.Drawing.Color.Black;
            this.m_labelError.SubText = "";
            this.m_labelError.TabIndex = 1378;
            this.m_labelError.Text = "--";
            this.m_labelError.TextAlignMain = Sys3Controls.EN_TEXTALIGN.BOTTOM_RIGHT;
            this.m_labelError.TextAlignSub = Sys3Controls.EN_TEXTALIGN.MIDDLE_CENTER;
            this.m_labelError.TextAlignUnit = Sys3Controls.EN_TEXTALIGN.BOTTOM_RIGHT;
            this.m_labelError.ThemeIndex = 0;
            this.m_labelError.UnitAreaRate = 20;
            this.m_labelError.UnitFont = new System.Drawing.Font("맑은 고딕", 10F, System.Drawing.FontStyle.Bold);
            this.m_labelError.UnitFontColor = System.Drawing.Color.Black;
            this.m_labelError.UnitPositionVertical = false;
            this.m_labelError.UnitText = "mm";
            this.m_labelError.UseBorder = true;
            this.m_labelError.UseEdgeRadius = false;
            this.m_labelError.UseImage = false;
            this.m_labelError.UseSubFont = false;
            this.m_labelError.UseUnitFont = true;
            // 
            // m_btnClear
            // 
            this.m_btnClear.BorderWidth = 3;
            this.m_btnClear.ButtonClicked = false;
            this.m_btnClear.ClickedEmphasizeTextColor = System.Drawing.Color.White;
            this.m_btnClear.CustomClickedGradientFirstColor = System.Drawing.Color.White;
            this.m_btnClear.CustomClickedGradientSecondColor = System.Drawing.Color.White;
            this.m_btnClear.Description = "";
            this.m_btnClear.DisabledColor = System.Drawing.Color.DarkGray;
            this.m_btnClear.EdgeRadius = 5;
            this.m_btnClear.Enabled = false;
            this.m_btnClear.GradientAngle = 70F;
            this.m_btnClear.GradientFirstColor = System.Drawing.Color.White;
            this.m_btnClear.GradientSecondColor = System.Drawing.Color.FromArgb(((int)(((byte)(170)))), ((int)(((byte)(176)))), ((int)(((byte)(183)))));
            this.m_btnClear.HoverEmphasizeCustomColor = System.Drawing.Color.White;
            this.m_btnClear.ImagePosition = new System.Drawing.Point(7, 7);
            this.m_btnClear.ImageSize = new System.Drawing.Point(30, 30);
            this.m_btnClear.LoadImage = global::FrameOfSystem3.Properties.Resources.CONNECT_BLACK;
            this.m_btnClear.Location = new System.Drawing.Point(451, 8);
            this.m_btnClear.MainFont = new System.Drawing.Font("맑은 고딕", 13F, System.Drawing.FontStyle.Bold);
            this.m_btnClear.MainFontColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(36)))), ((int)(((byte)(0)))));
            this.m_btnClear.Name = "m_btnClear";
            this._tableLayoutPanel_Position.SetRowSpan(this.m_btnClear, 3);
            this.m_btnClear.Size = new System.Drawing.Size(95, 124);
            this.m_btnClear.SubFont = new System.Drawing.Font("맑은 고딕", 8F, System.Drawing.FontStyle.Bold);
            this.m_btnClear.SubFontColor = System.Drawing.Color.DarkBlue;
            this.m_btnClear.SubText = "STATE";
            this.m_btnClear.TabIndex = 13834;
            this.m_btnClear.Text = "CLEAR";
            this.m_btnClear.TextAlignMain = Sys3Controls.EN_TEXTALIGN.MIDDLE_RIGHT;
            this.m_btnClear.TextAlignSub = Sys3Controls.EN_TEXTALIGN.TOP_RIGHT;
            this.m_btnClear.ThemeIndex = 0;
            this.m_btnClear.UseBorder = true;
            this.m_btnClear.UseClickedEmphasizeTextColor = false;
            this.m_btnClear.UseCustomizeClickedColor = false;
            this.m_btnClear.UseEdge = true;
            this.m_btnClear.UseHoverEmphasizeCustomColor = false;
            this.m_btnClear.UseImage = false;
            this.m_btnClear.UserHoverEmpahsize = false;
            this.m_btnClear.UseSubFont = true;
            this.m_btnClear.Click += new System.EventHandler(this.Click_StateClear);
            // 
            // m_groupRepeat
            // 
            this.m_groupRepeat.BackGroundColor = System.Drawing.Color.WhiteSmoke;
            this.m_groupRepeat.Controls.Add(this._tableLayoutPanel_Repeat);
            this.m_groupRepeat.Dock = System.Windows.Forms.DockStyle.Fill;
            this.m_groupRepeat.EdgeBorderStroke = 2;
            this.m_groupRepeat.EdgeRadius = 2;
            this.m_groupRepeat.LabelFont = new System.Drawing.Font("맑은 고딕", 10F, System.Drawing.FontStyle.Bold);
            this.m_groupRepeat.LabelGradientColorFirst = System.Drawing.Color.FromArgb(((int)(((byte)(208)))), ((int)(((byte)(208)))), ((int)(((byte)(208)))));
            this.m_groupRepeat.LabelGradientColorSecond = System.Drawing.Color.FromArgb(((int)(((byte)(240)))), ((int)(((byte)(240)))), ((int)(((byte)(240)))));
            this.m_groupRepeat.LabelHeight = 30;
            this.m_groupRepeat.LabelTextColor = System.Drawing.Color.Black;
            this.m_groupRepeat.Location = new System.Drawing.Point(277, 175);
            this.m_groupRepeat.Margin = new System.Windows.Forms.Padding(0);
            this.m_groupRepeat.Name = "m_groupRepeat";
            this.m_groupRepeat.Padding = new System.Windows.Forms.Padding(3, 18, 3, 3);
            this.m_groupRepeat.Size = new System.Drawing.Size(560, 236);
            this.m_groupRepeat.TabIndex = 1374;
            this.m_groupRepeat.TabStop = false;
            this.m_groupRepeat.Text = "REPEAT";
            this.m_groupRepeat.TextAlign = Sys3Controls.EN_TEXTALIGN.MIDDLE_CENTER;
            this.m_groupRepeat.ThemeIndex = 0;
            this.m_groupRepeat.UseLabelBorder = true;
            this.m_groupRepeat.UseTitle = true;
            // 
            // _tableLayoutPanel_Repeat
            // 
            this._tableLayoutPanel_Repeat.ColumnCount = 7;
            this._tableLayoutPanel_Repeat.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this._tableLayoutPanel_Repeat.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 21.23691F));
            this._tableLayoutPanel_Repeat.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 18.317F));
            this._tableLayoutPanel_Repeat.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 18.317F));
            this._tableLayoutPanel_Repeat.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 21.06455F));
            this._tableLayoutPanel_Repeat.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this._tableLayoutPanel_Repeat.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 21.06455F));
            this._tableLayoutPanel_Repeat.Controls.Add(this.m_ledFirstPosition, 0, 0);
            this._tableLayoutPanel_Repeat.Controls.Add(this.m_ledSecondDelay, 0, 4);
            this._tableLayoutPanel_Repeat.Controls.Add(this.m_lblPositionFirst, 1, 0);
            this._tableLayoutPanel_Repeat.Controls.Add(this.m_ledSecondPosition, 0, 3);
            this._tableLayoutPanel_Repeat.Controls.Add(this.m_btnRepeat, 6, 1);
            this._tableLayoutPanel_Repeat.Controls.Add(this.m_labelPositionFirst, 2, 0);
            this._tableLayoutPanel_Repeat.Controls.Add(this.m_btnMoveSecond, 4, 3);
            this._tableLayoutPanel_Repeat.Controls.Add(this.m_ledFirstDelay, 0, 1);
            this._tableLayoutPanel_Repeat.Controls.Add(this.m_btnMoveFirst, 4, 0);
            this._tableLayoutPanel_Repeat.Controls.Add(this.m_lblDelayFirst, 1, 1);
            this._tableLayoutPanel_Repeat.Controls.Add(this.m_labelDelayFirst, 2, 1);
            this._tableLayoutPanel_Repeat.Controls.Add(this.m_lblDelaySecond, 1, 4);
            this._tableLayoutPanel_Repeat.Controls.Add(this.m_lblPositionSecond, 1, 3);
            this._tableLayoutPanel_Repeat.Controls.Add(this.m_labelPositionSecond, 2, 3);
            this._tableLayoutPanel_Repeat.Controls.Add(this.m_labelDelaySecond, 2, 4);
            this._tableLayoutPanel_Repeat.Controls.Add(this.m_ledRepeat, 5, 1);
            this._tableLayoutPanel_Repeat.Dock = System.Windows.Forms.DockStyle.Fill;
            this._tableLayoutPanel_Repeat.Location = new System.Drawing.Point(3, 32);
            this._tableLayoutPanel_Repeat.Name = "_tableLayoutPanel_Repeat";
            this._tableLayoutPanel_Repeat.Padding = new System.Windows.Forms.Padding(3);
            this._tableLayoutPanel_Repeat.RowCount = 5;
            this._tableLayoutPanel_Repeat.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this._tableLayoutPanel_Repeat.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this._tableLayoutPanel_Repeat.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 10F));
            this._tableLayoutPanel_Repeat.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this._tableLayoutPanel_Repeat.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this._tableLayoutPanel_Repeat.Size = new System.Drawing.Size(554, 201);
            this._tableLayoutPanel_Repeat.TabIndex = 0;
            // 
            // m_ledFirstPosition
            // 
            this.m_ledFirstPosition.Active = false;
            this.m_ledFirstPosition.ColorActive = System.Drawing.Color.FromArgb(((int)(((byte)(75)))), ((int)(((byte)(216)))), ((int)(((byte)(101)))));
            this.m_ledFirstPosition.ColorNormal = System.Drawing.Color.DimGray;
            this.m_ledFirstPosition.Dock = System.Windows.Forms.DockStyle.Fill;
            this.m_ledFirstPosition.Location = new System.Drawing.Point(3, 6);
            this.m_ledFirstPosition.Margin = new System.Windows.Forms.Padding(0, 3, 0, 3);
            this.m_ledFirstPosition.Name = "m_ledFirstPosition";
            this.m_ledFirstPosition.Size = new System.Drawing.Size(20, 40);
            this.m_ledFirstPosition.TabIndex = 13838;
            // 
            // m_ledSecondDelay
            // 
            this.m_ledSecondDelay.Active = false;
            this.m_ledSecondDelay.ColorActive = System.Drawing.Color.FromArgb(((int)(((byte)(75)))), ((int)(((byte)(216)))), ((int)(((byte)(101)))));
            this.m_ledSecondDelay.ColorNormal = System.Drawing.Color.DimGray;
            this.m_ledSecondDelay.Dock = System.Windows.Forms.DockStyle.Fill;
            this.m_ledSecondDelay.Location = new System.Drawing.Point(3, 154);
            this.m_ledSecondDelay.Margin = new System.Windows.Forms.Padding(0, 3, 0, 3);
            this.m_ledSecondDelay.Name = "m_ledSecondDelay";
            this.m_ledSecondDelay.Size = new System.Drawing.Size(20, 41);
            this.m_ledSecondDelay.TabIndex = 13841;
            // 
            // m_lblPositionFirst
            // 
            this.m_lblPositionFirst.BackGroundColor = System.Drawing.Color.FromArgb(((int)(((byte)(200)))), ((int)(((byte)(200)))), ((int)(((byte)(220)))));
            this.m_lblPositionFirst.BorderStroke = 2;
            this.m_lblPositionFirst.BorderStyle = System.Windows.Forms.ButtonBorderStyle.Solid;
            this.m_lblPositionFirst.Description = "";
            this.m_lblPositionFirst.DisabledColor = System.Drawing.Color.DarkGray;
            this.m_lblPositionFirst.Dock = System.Windows.Forms.DockStyle.Fill;
            this.m_lblPositionFirst.EdgeRadius = 1;
            this.m_lblPositionFirst.ImagePosition = new System.Drawing.Point(0, 0);
            this.m_lblPositionFirst.ImageSize = new System.Drawing.Point(0, 0);
            this.m_lblPositionFirst.LoadImage = null;
            this.m_lblPositionFirst.Location = new System.Drawing.Point(23, 6);
            this.m_lblPositionFirst.MainFont = new System.Drawing.Font("맑은 고딕", 9F, System.Drawing.FontStyle.Bold);
            this.m_lblPositionFirst.MainFontColor = System.Drawing.Color.Black;
            this.m_lblPositionFirst.Margin = new System.Windows.Forms.Padding(0, 3, 0, 3);
            this.m_lblPositionFirst.Name = "m_lblPositionFirst";
            this.m_lblPositionFirst.Size = new System.Drawing.Size(107, 40);
            this.m_lblPositionFirst.SubFont = new System.Drawing.Font("맑은 고딕", 9F, System.Drawing.FontStyle.Bold);
            this.m_lblPositionFirst.SubFontColor = System.Drawing.Color.DarkBlue;
            this.m_lblPositionFirst.SubText = "FIRST";
            this.m_lblPositionFirst.TabIndex = 1379;
            this.m_lblPositionFirst.Text = "POSITION";
            this.m_lblPositionFirst.TextAlignMain = Sys3Controls.EN_TEXTALIGN.TOP_LEFT;
            this.m_lblPositionFirst.TextAlignSub = Sys3Controls.EN_TEXTALIGN.BOTTOM_RIGHT;
            this.m_lblPositionFirst.TextAlignUnit = Sys3Controls.EN_TEXTALIGN.MIDDLE_CENTER;
            this.m_lblPositionFirst.ThemeIndex = 0;
            this.m_lblPositionFirst.UnitAreaRate = 30;
            this.m_lblPositionFirst.UnitFont = new System.Drawing.Font("맑은 고딕", 10F, System.Drawing.FontStyle.Bold);
            this.m_lblPositionFirst.UnitFontColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.m_lblPositionFirst.UnitPositionVertical = false;
            this.m_lblPositionFirst.UnitText = "";
            this.m_lblPositionFirst.UseBorder = true;
            this.m_lblPositionFirst.UseEdgeRadius = false;
            this.m_lblPositionFirst.UseImage = false;
            this.m_lblPositionFirst.UseSubFont = true;
            this.m_lblPositionFirst.UseUnitFont = false;
            // 
            // m_ledSecondPosition
            // 
            this.m_ledSecondPosition.Active = false;
            this.m_ledSecondPosition.ColorActive = System.Drawing.Color.FromArgb(((int)(((byte)(75)))), ((int)(((byte)(216)))), ((int)(((byte)(101)))));
            this.m_ledSecondPosition.ColorNormal = System.Drawing.Color.DimGray;
            this.m_ledSecondPosition.Dock = System.Windows.Forms.DockStyle.Fill;
            this.m_ledSecondPosition.Location = new System.Drawing.Point(3, 108);
            this.m_ledSecondPosition.Margin = new System.Windows.Forms.Padding(0, 3, 0, 3);
            this.m_ledSecondPosition.Name = "m_ledSecondPosition";
            this.m_ledSecondPosition.Size = new System.Drawing.Size(20, 40);
            this.m_ledSecondPosition.TabIndex = 13840;
            // 
            // m_btnRepeat
            // 
            this.m_btnRepeat.BorderWidth = 2;
            this.m_btnRepeat.ButtonClicked = false;
            this.m_btnRepeat.ClickedEmphasizeTextColor = System.Drawing.Color.White;
            this.m_btnRepeat.CustomClickedGradientFirstColor = System.Drawing.Color.White;
            this.m_btnRepeat.CustomClickedGradientSecondColor = System.Drawing.Color.White;
            this.m_btnRepeat.Description = "";
            this.m_btnRepeat.DisabledColor = System.Drawing.Color.DarkGray;
            this.m_btnRepeat.Dock = System.Windows.Forms.DockStyle.Fill;
            this.m_btnRepeat.EdgeRadius = 5;
            this.m_btnRepeat.Enabled = false;
            this.m_btnRepeat.GradientAngle = 70F;
            this.m_btnRepeat.GradientFirstColor = System.Drawing.Color.White;
            this.m_btnRepeat.GradientSecondColor = System.Drawing.Color.FromArgb(((int)(((byte)(170)))), ((int)(((byte)(176)))), ((int)(((byte)(183)))));
            this.m_btnRepeat.HoverEmphasizeCustomColor = System.Drawing.Color.White;
            this.m_btnRepeat.ImagePosition = new System.Drawing.Point(7, 7);
            this.m_btnRepeat.ImageSize = new System.Drawing.Point(30, 30);
            this.m_btnRepeat.LoadImage = global::FrameOfSystem3.Properties.Resources.CONNECT_BLACK;
            this.m_btnRepeat.Location = new System.Drawing.Point(443, 49);
            this.m_btnRepeat.MainFont = new System.Drawing.Font("맑은 고딕", 11F, System.Drawing.FontStyle.Bold);
            this.m_btnRepeat.MainFontColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(36)))), ((int)(((byte)(0)))));
            this.m_btnRepeat.Margin = new System.Windows.Forms.Padding(0);
            this.m_btnRepeat.Name = "m_btnRepeat";
            this._tableLayoutPanel_Repeat.SetRowSpan(this.m_btnRepeat, 3);
            this.m_btnRepeat.Size = new System.Drawing.Size(108, 102);
            this.m_btnRepeat.SubFont = new System.Drawing.Font("맑은 고딕", 8F, System.Drawing.FontStyle.Bold);
            this.m_btnRepeat.SubFontColor = System.Drawing.Color.DarkBlue;
            this.m_btnRepeat.SubText = "MOVE";
            this.m_btnRepeat.TabIndex = 2;
            this.m_btnRepeat.Text = "REPEAT";
            this.m_btnRepeat.TextAlignMain = Sys3Controls.EN_TEXTALIGN.MIDDLE_RIGHT;
            this.m_btnRepeat.TextAlignSub = Sys3Controls.EN_TEXTALIGN.TOP_RIGHT;
            this.m_btnRepeat.ThemeIndex = 0;
            this.m_btnRepeat.UseBorder = true;
            this.m_btnRepeat.UseClickedEmphasizeTextColor = false;
            this.m_btnRepeat.UseCustomizeClickedColor = false;
            this.m_btnRepeat.UseEdge = false;
            this.m_btnRepeat.UseHoverEmphasizeCustomColor = false;
            this.m_btnRepeat.UseImage = false;
            this.m_btnRepeat.UserHoverEmpahsize = false;
            this.m_btnRepeat.UseSubFont = true;
            this.m_btnRepeat.Click += new System.EventHandler(this.Click_Move);
            // 
            // m_labelPositionFirst
            // 
            this.m_labelPositionFirst.BackGroundColor = System.Drawing.Color.White;
            this.m_labelPositionFirst.BorderStroke = 2;
            this.m_labelPositionFirst.BorderStyle = System.Windows.Forms.ButtonBorderStyle.Solid;
            this._tableLayoutPanel_Repeat.SetColumnSpan(this.m_labelPositionFirst, 2);
            this.m_labelPositionFirst.Description = "";
            this.m_labelPositionFirst.DisabledColor = System.Drawing.Color.LightGray;
            this.m_labelPositionFirst.Dock = System.Windows.Forms.DockStyle.Fill;
            this.m_labelPositionFirst.EdgeRadius = 1;
            this.m_labelPositionFirst.Enabled = false;
            this.m_labelPositionFirst.ImagePosition = new System.Drawing.Point(0, 0);
            this.m_labelPositionFirst.ImageSize = new System.Drawing.Point(0, 0);
            this.m_labelPositionFirst.LoadImage = null;
            this.m_labelPositionFirst.Location = new System.Drawing.Point(132, 6);
            this.m_labelPositionFirst.MainFont = new System.Drawing.Font("맑은 고딕", 11F, System.Drawing.FontStyle.Bold);
            this.m_labelPositionFirst.MainFontColor = System.Drawing.Color.Black;
            this.m_labelPositionFirst.Margin = new System.Windows.Forms.Padding(2, 3, 0, 3);
            this.m_labelPositionFirst.Name = "m_labelPositionFirst";
            this.m_labelPositionFirst.Size = new System.Drawing.Size(184, 40);
            this.m_labelPositionFirst.SubFont = new System.Drawing.Font("맑은 고딕", 10F);
            this.m_labelPositionFirst.SubFontColor = System.Drawing.Color.Black;
            this.m_labelPositionFirst.SubText = "";
            this.m_labelPositionFirst.TabIndex = 0;
            this.m_labelPositionFirst.Text = "--";
            this.m_labelPositionFirst.TextAlignMain = Sys3Controls.EN_TEXTALIGN.BOTTOM_RIGHT;
            this.m_labelPositionFirst.TextAlignSub = Sys3Controls.EN_TEXTALIGN.MIDDLE_CENTER;
            this.m_labelPositionFirst.TextAlignUnit = Sys3Controls.EN_TEXTALIGN.BOTTOM_RIGHT;
            this.m_labelPositionFirst.ThemeIndex = 0;
            this.m_labelPositionFirst.UnitAreaRate = 20;
            this.m_labelPositionFirst.UnitFont = new System.Drawing.Font("맑은 고딕", 10F, System.Drawing.FontStyle.Bold);
            this.m_labelPositionFirst.UnitFontColor = System.Drawing.Color.Black;
            this.m_labelPositionFirst.UnitPositionVertical = false;
            this.m_labelPositionFirst.UnitText = "mm";
            this.m_labelPositionFirst.UseBorder = true;
            this.m_labelPositionFirst.UseEdgeRadius = false;
            this.m_labelPositionFirst.UseImage = false;
            this.m_labelPositionFirst.UseSubFont = false;
            this.m_labelPositionFirst.UseUnitFont = true;
            this.m_labelPositionFirst.Click += new System.EventHandler(this.Click_Labels);
            // 
            // m_btnMoveSecond
            // 
            this.m_btnMoveSecond.BorderWidth = 3;
            this.m_btnMoveSecond.ButtonClicked = false;
            this.m_btnMoveSecond.ClickedEmphasizeTextColor = System.Drawing.Color.White;
            this.m_btnMoveSecond.CustomClickedGradientFirstColor = System.Drawing.Color.White;
            this.m_btnMoveSecond.CustomClickedGradientSecondColor = System.Drawing.Color.White;
            this.m_btnMoveSecond.Description = "";
            this.m_btnMoveSecond.DisabledColor = System.Drawing.Color.DarkGray;
            this.m_btnMoveSecond.Dock = System.Windows.Forms.DockStyle.Fill;
            this.m_btnMoveSecond.EdgeRadius = 5;
            this.m_btnMoveSecond.Enabled = false;
            this.m_btnMoveSecond.GradientAngle = 70F;
            this.m_btnMoveSecond.GradientFirstColor = System.Drawing.Color.White;
            this.m_btnMoveSecond.GradientSecondColor = System.Drawing.Color.FromArgb(((int)(((byte)(170)))), ((int)(((byte)(176)))), ((int)(((byte)(183)))));
            this.m_btnMoveSecond.HoverEmphasizeCustomColor = System.Drawing.Color.White;
            this.m_btnMoveSecond.ImagePosition = new System.Drawing.Point(7, 7);
            this.m_btnMoveSecond.ImageSize = new System.Drawing.Point(30, 30);
            this.m_btnMoveSecond.LoadImage = global::FrameOfSystem3.Properties.Resources.CONNECT_BLACK;
            this.m_btnMoveSecond.Location = new System.Drawing.Point(319, 108);
            this.m_btnMoveSecond.MainFont = new System.Drawing.Font("맑은 고딕", 11F, System.Drawing.FontStyle.Bold);
            this.m_btnMoveSecond.MainFontColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(36)))), ((int)(((byte)(0)))));
            this.m_btnMoveSecond.Name = "m_btnMoveSecond";
            this._tableLayoutPanel_Repeat.SetRowSpan(this.m_btnMoveSecond, 2);
            this.m_btnMoveSecond.Size = new System.Drawing.Size(101, 87);
            this.m_btnMoveSecond.SubFont = new System.Drawing.Font("맑은 고딕", 8F, System.Drawing.FontStyle.Bold);
            this.m_btnMoveSecond.SubFontColor = System.Drawing.Color.DarkBlue;
            this.m_btnMoveSecond.SubText = "MOVE";
            this.m_btnMoveSecond.TabIndex = 1;
            this.m_btnMoveSecond.Text = "SECOND";
            this.m_btnMoveSecond.TextAlignMain = Sys3Controls.EN_TEXTALIGN.MIDDLE_RIGHT;
            this.m_btnMoveSecond.TextAlignSub = Sys3Controls.EN_TEXTALIGN.TOP_RIGHT;
            this.m_btnMoveSecond.ThemeIndex = 0;
            this.m_btnMoveSecond.UseBorder = true;
            this.m_btnMoveSecond.UseClickedEmphasizeTextColor = false;
            this.m_btnMoveSecond.UseCustomizeClickedColor = false;
            this.m_btnMoveSecond.UseEdge = true;
            this.m_btnMoveSecond.UseHoverEmphasizeCustomColor = false;
            this.m_btnMoveSecond.UseImage = false;
            this.m_btnMoveSecond.UserHoverEmpahsize = false;
            this.m_btnMoveSecond.UseSubFont = true;
            this.m_btnMoveSecond.Click += new System.EventHandler(this.Click_Move);
            // 
            // m_ledFirstDelay
            // 
            this.m_ledFirstDelay.Active = false;
            this.m_ledFirstDelay.ColorActive = System.Drawing.Color.FromArgb(((int)(((byte)(75)))), ((int)(((byte)(216)))), ((int)(((byte)(101)))));
            this.m_ledFirstDelay.ColorNormal = System.Drawing.Color.DimGray;
            this.m_ledFirstDelay.Dock = System.Windows.Forms.DockStyle.Fill;
            this.m_ledFirstDelay.Location = new System.Drawing.Point(3, 52);
            this.m_ledFirstDelay.Margin = new System.Windows.Forms.Padding(0, 3, 0, 3);
            this.m_ledFirstDelay.Name = "m_ledFirstDelay";
            this.m_ledFirstDelay.Size = new System.Drawing.Size(20, 40);
            this.m_ledFirstDelay.TabIndex = 13839;
            // 
            // m_btnMoveFirst
            // 
            this.m_btnMoveFirst.BorderWidth = 3;
            this.m_btnMoveFirst.ButtonClicked = false;
            this.m_btnMoveFirst.ClickedEmphasizeTextColor = System.Drawing.Color.White;
            this.m_btnMoveFirst.CustomClickedGradientFirstColor = System.Drawing.Color.White;
            this.m_btnMoveFirst.CustomClickedGradientSecondColor = System.Drawing.Color.White;
            this.m_btnMoveFirst.Description = "";
            this.m_btnMoveFirst.DisabledColor = System.Drawing.Color.DarkGray;
            this.m_btnMoveFirst.Dock = System.Windows.Forms.DockStyle.Fill;
            this.m_btnMoveFirst.EdgeRadius = 5;
            this.m_btnMoveFirst.Enabled = false;
            this.m_btnMoveFirst.GradientAngle = 70F;
            this.m_btnMoveFirst.GradientFirstColor = System.Drawing.Color.White;
            this.m_btnMoveFirst.GradientSecondColor = System.Drawing.Color.FromArgb(((int)(((byte)(170)))), ((int)(((byte)(176)))), ((int)(((byte)(183)))));
            this.m_btnMoveFirst.HoverEmphasizeCustomColor = System.Drawing.Color.White;
            this.m_btnMoveFirst.ImagePosition = new System.Drawing.Point(7, 7);
            this.m_btnMoveFirst.ImageSize = new System.Drawing.Point(30, 30);
            this.m_btnMoveFirst.LoadImage = global::FrameOfSystem3.Properties.Resources.CONNECT_BLACK;
            this.m_btnMoveFirst.Location = new System.Drawing.Point(319, 6);
            this.m_btnMoveFirst.MainFont = new System.Drawing.Font("맑은 고딕", 11F, System.Drawing.FontStyle.Bold);
            this.m_btnMoveFirst.MainFontColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(36)))), ((int)(((byte)(0)))));
            this.m_btnMoveFirst.Name = "m_btnMoveFirst";
            this._tableLayoutPanel_Repeat.SetRowSpan(this.m_btnMoveFirst, 2);
            this.m_btnMoveFirst.Size = new System.Drawing.Size(101, 86);
            this.m_btnMoveFirst.SubFont = new System.Drawing.Font("맑은 고딕", 8F, System.Drawing.FontStyle.Bold);
            this.m_btnMoveFirst.SubFontColor = System.Drawing.Color.DarkBlue;
            this.m_btnMoveFirst.SubText = "MOVE";
            this.m_btnMoveFirst.TabIndex = 0;
            this.m_btnMoveFirst.Text = "FIRST";
            this.m_btnMoveFirst.TextAlignMain = Sys3Controls.EN_TEXTALIGN.MIDDLE_RIGHT;
            this.m_btnMoveFirst.TextAlignSub = Sys3Controls.EN_TEXTALIGN.TOP_RIGHT;
            this.m_btnMoveFirst.ThemeIndex = 0;
            this.m_btnMoveFirst.UseBorder = true;
            this.m_btnMoveFirst.UseClickedEmphasizeTextColor = false;
            this.m_btnMoveFirst.UseCustomizeClickedColor = false;
            this.m_btnMoveFirst.UseEdge = true;
            this.m_btnMoveFirst.UseHoverEmphasizeCustomColor = false;
            this.m_btnMoveFirst.UseImage = false;
            this.m_btnMoveFirst.UserHoverEmpahsize = false;
            this.m_btnMoveFirst.UseSubFont = true;
            this.m_btnMoveFirst.Click += new System.EventHandler(this.Click_Move);
            // 
            // m_lblDelayFirst
            // 
            this.m_lblDelayFirst.BackGroundColor = System.Drawing.Color.FromArgb(((int)(((byte)(200)))), ((int)(((byte)(200)))), ((int)(((byte)(220)))));
            this.m_lblDelayFirst.BorderStroke = 2;
            this.m_lblDelayFirst.BorderStyle = System.Windows.Forms.ButtonBorderStyle.Solid;
            this.m_lblDelayFirst.Description = "";
            this.m_lblDelayFirst.DisabledColor = System.Drawing.Color.DarkGray;
            this.m_lblDelayFirst.Dock = System.Windows.Forms.DockStyle.Fill;
            this.m_lblDelayFirst.EdgeRadius = 1;
            this.m_lblDelayFirst.ImagePosition = new System.Drawing.Point(0, 0);
            this.m_lblDelayFirst.ImageSize = new System.Drawing.Point(0, 0);
            this.m_lblDelayFirst.LoadImage = null;
            this.m_lblDelayFirst.Location = new System.Drawing.Point(23, 52);
            this.m_lblDelayFirst.MainFont = new System.Drawing.Font("맑은 고딕", 9F, System.Drawing.FontStyle.Bold);
            this.m_lblDelayFirst.MainFontColor = System.Drawing.Color.Black;
            this.m_lblDelayFirst.Margin = new System.Windows.Forms.Padding(0, 3, 0, 3);
            this.m_lblDelayFirst.Name = "m_lblDelayFirst";
            this.m_lblDelayFirst.Size = new System.Drawing.Size(107, 40);
            this.m_lblDelayFirst.SubFont = new System.Drawing.Font("맑은 고딕", 9F, System.Drawing.FontStyle.Bold);
            this.m_lblDelayFirst.SubFontColor = System.Drawing.Color.DarkBlue;
            this.m_lblDelayFirst.SubText = "FIRST";
            this.m_lblDelayFirst.TabIndex = 1381;
            this.m_lblDelayFirst.Text = "DELAY";
            this.m_lblDelayFirst.TextAlignMain = Sys3Controls.EN_TEXTALIGN.TOP_LEFT;
            this.m_lblDelayFirst.TextAlignSub = Sys3Controls.EN_TEXTALIGN.BOTTOM_RIGHT;
            this.m_lblDelayFirst.TextAlignUnit = Sys3Controls.EN_TEXTALIGN.MIDDLE_CENTER;
            this.m_lblDelayFirst.ThemeIndex = 0;
            this.m_lblDelayFirst.UnitAreaRate = 30;
            this.m_lblDelayFirst.UnitFont = new System.Drawing.Font("맑은 고딕", 10F, System.Drawing.FontStyle.Bold);
            this.m_lblDelayFirst.UnitFontColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.m_lblDelayFirst.UnitPositionVertical = false;
            this.m_lblDelayFirst.UnitText = "";
            this.m_lblDelayFirst.UseBorder = true;
            this.m_lblDelayFirst.UseEdgeRadius = false;
            this.m_lblDelayFirst.UseImage = false;
            this.m_lblDelayFirst.UseSubFont = true;
            this.m_lblDelayFirst.UseUnitFont = false;
            // 
            // m_labelDelayFirst
            // 
            this.m_labelDelayFirst.BackGroundColor = System.Drawing.Color.White;
            this.m_labelDelayFirst.BorderStroke = 2;
            this.m_labelDelayFirst.BorderStyle = System.Windows.Forms.ButtonBorderStyle.Solid;
            this._tableLayoutPanel_Repeat.SetColumnSpan(this.m_labelDelayFirst, 2);
            this.m_labelDelayFirst.Description = "";
            this.m_labelDelayFirst.DisabledColor = System.Drawing.Color.LightGray;
            this.m_labelDelayFirst.Dock = System.Windows.Forms.DockStyle.Fill;
            this.m_labelDelayFirst.EdgeRadius = 1;
            this.m_labelDelayFirst.Enabled = false;
            this.m_labelDelayFirst.ImagePosition = new System.Drawing.Point(0, 0);
            this.m_labelDelayFirst.ImageSize = new System.Drawing.Point(0, 0);
            this.m_labelDelayFirst.LoadImage = null;
            this.m_labelDelayFirst.Location = new System.Drawing.Point(132, 52);
            this.m_labelDelayFirst.MainFont = new System.Drawing.Font("맑은 고딕", 11F, System.Drawing.FontStyle.Bold);
            this.m_labelDelayFirst.MainFontColor = System.Drawing.Color.Black;
            this.m_labelDelayFirst.Margin = new System.Windows.Forms.Padding(2, 3, 0, 3);
            this.m_labelDelayFirst.Name = "m_labelDelayFirst";
            this.m_labelDelayFirst.Size = new System.Drawing.Size(184, 40);
            this.m_labelDelayFirst.SubFont = new System.Drawing.Font("맑은 고딕", 10F);
            this.m_labelDelayFirst.SubFontColor = System.Drawing.Color.Black;
            this.m_labelDelayFirst.SubText = "";
            this.m_labelDelayFirst.TabIndex = 1;
            this.m_labelDelayFirst.Text = "--";
            this.m_labelDelayFirst.TextAlignMain = Sys3Controls.EN_TEXTALIGN.BOTTOM_RIGHT;
            this.m_labelDelayFirst.TextAlignSub = Sys3Controls.EN_TEXTALIGN.MIDDLE_CENTER;
            this.m_labelDelayFirst.TextAlignUnit = Sys3Controls.EN_TEXTALIGN.BOTTOM_RIGHT;
            this.m_labelDelayFirst.ThemeIndex = 0;
            this.m_labelDelayFirst.UnitAreaRate = 20;
            this.m_labelDelayFirst.UnitFont = new System.Drawing.Font("맑은 고딕", 10F, System.Drawing.FontStyle.Bold);
            this.m_labelDelayFirst.UnitFontColor = System.Drawing.Color.Black;
            this.m_labelDelayFirst.UnitPositionVertical = false;
            this.m_labelDelayFirst.UnitText = "ms";
            this.m_labelDelayFirst.UseBorder = true;
            this.m_labelDelayFirst.UseEdgeRadius = false;
            this.m_labelDelayFirst.UseImage = false;
            this.m_labelDelayFirst.UseSubFont = false;
            this.m_labelDelayFirst.UseUnitFont = true;
            this.m_labelDelayFirst.Click += new System.EventHandler(this.Click_Labels);
            // 
            // m_lblDelaySecond
            // 
            this.m_lblDelaySecond.BackGroundColor = System.Drawing.Color.FromArgb(((int)(((byte)(200)))), ((int)(((byte)(200)))), ((int)(((byte)(220)))));
            this.m_lblDelaySecond.BorderStroke = 2;
            this.m_lblDelaySecond.BorderStyle = System.Windows.Forms.ButtonBorderStyle.Solid;
            this.m_lblDelaySecond.Description = "";
            this.m_lblDelaySecond.DisabledColor = System.Drawing.Color.DarkGray;
            this.m_lblDelaySecond.Dock = System.Windows.Forms.DockStyle.Fill;
            this.m_lblDelaySecond.EdgeRadius = 1;
            this.m_lblDelaySecond.ImagePosition = new System.Drawing.Point(0, 0);
            this.m_lblDelaySecond.ImageSize = new System.Drawing.Point(0, 0);
            this.m_lblDelaySecond.LoadImage = null;
            this.m_lblDelaySecond.Location = new System.Drawing.Point(23, 154);
            this.m_lblDelaySecond.MainFont = new System.Drawing.Font("맑은 고딕", 9F, System.Drawing.FontStyle.Bold);
            this.m_lblDelaySecond.MainFontColor = System.Drawing.Color.Black;
            this.m_lblDelaySecond.Margin = new System.Windows.Forms.Padding(0, 3, 0, 3);
            this.m_lblDelaySecond.Name = "m_lblDelaySecond";
            this.m_lblDelaySecond.Size = new System.Drawing.Size(107, 41);
            this.m_lblDelaySecond.SubFont = new System.Drawing.Font("맑은 고딕", 9F, System.Drawing.FontStyle.Bold);
            this.m_lblDelaySecond.SubFontColor = System.Drawing.Color.DarkBlue;
            this.m_lblDelaySecond.SubText = "SECOND";
            this.m_lblDelaySecond.TabIndex = 1382;
            this.m_lblDelaySecond.Text = "DELAY";
            this.m_lblDelaySecond.TextAlignMain = Sys3Controls.EN_TEXTALIGN.TOP_LEFT;
            this.m_lblDelaySecond.TextAlignSub = Sys3Controls.EN_TEXTALIGN.BOTTOM_RIGHT;
            this.m_lblDelaySecond.TextAlignUnit = Sys3Controls.EN_TEXTALIGN.MIDDLE_CENTER;
            this.m_lblDelaySecond.ThemeIndex = 0;
            this.m_lblDelaySecond.UnitAreaRate = 30;
            this.m_lblDelaySecond.UnitFont = new System.Drawing.Font("맑은 고딕", 10F, System.Drawing.FontStyle.Bold);
            this.m_lblDelaySecond.UnitFontColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.m_lblDelaySecond.UnitPositionVertical = false;
            this.m_lblDelaySecond.UnitText = "";
            this.m_lblDelaySecond.UseBorder = true;
            this.m_lblDelaySecond.UseEdgeRadius = false;
            this.m_lblDelaySecond.UseImage = false;
            this.m_lblDelaySecond.UseSubFont = true;
            this.m_lblDelaySecond.UseUnitFont = false;
            // 
            // m_lblPositionSecond
            // 
            this.m_lblPositionSecond.BackGroundColor = System.Drawing.Color.FromArgb(((int)(((byte)(200)))), ((int)(((byte)(200)))), ((int)(((byte)(220)))));
            this.m_lblPositionSecond.BorderStroke = 2;
            this.m_lblPositionSecond.BorderStyle = System.Windows.Forms.ButtonBorderStyle.Solid;
            this.m_lblPositionSecond.Description = "";
            this.m_lblPositionSecond.DisabledColor = System.Drawing.Color.DarkGray;
            this.m_lblPositionSecond.Dock = System.Windows.Forms.DockStyle.Fill;
            this.m_lblPositionSecond.EdgeRadius = 1;
            this.m_lblPositionSecond.ImagePosition = new System.Drawing.Point(0, 0);
            this.m_lblPositionSecond.ImageSize = new System.Drawing.Point(0, 0);
            this.m_lblPositionSecond.LoadImage = null;
            this.m_lblPositionSecond.Location = new System.Drawing.Point(23, 108);
            this.m_lblPositionSecond.MainFont = new System.Drawing.Font("맑은 고딕", 9F, System.Drawing.FontStyle.Bold);
            this.m_lblPositionSecond.MainFontColor = System.Drawing.Color.Black;
            this.m_lblPositionSecond.Margin = new System.Windows.Forms.Padding(0, 3, 0, 3);
            this.m_lblPositionSecond.Name = "m_lblPositionSecond";
            this.m_lblPositionSecond.Size = new System.Drawing.Size(107, 40);
            this.m_lblPositionSecond.SubFont = new System.Drawing.Font("맑은 고딕", 9F, System.Drawing.FontStyle.Bold);
            this.m_lblPositionSecond.SubFontColor = System.Drawing.Color.DarkBlue;
            this.m_lblPositionSecond.SubText = "SECOND";
            this.m_lblPositionSecond.TabIndex = 1380;
            this.m_lblPositionSecond.Text = "POSITION";
            this.m_lblPositionSecond.TextAlignMain = Sys3Controls.EN_TEXTALIGN.TOP_LEFT;
            this.m_lblPositionSecond.TextAlignSub = Sys3Controls.EN_TEXTALIGN.BOTTOM_RIGHT;
            this.m_lblPositionSecond.TextAlignUnit = Sys3Controls.EN_TEXTALIGN.MIDDLE_CENTER;
            this.m_lblPositionSecond.ThemeIndex = 0;
            this.m_lblPositionSecond.UnitAreaRate = 30;
            this.m_lblPositionSecond.UnitFont = new System.Drawing.Font("맑은 고딕", 10F, System.Drawing.FontStyle.Bold);
            this.m_lblPositionSecond.UnitFontColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.m_lblPositionSecond.UnitPositionVertical = false;
            this.m_lblPositionSecond.UnitText = "";
            this.m_lblPositionSecond.UseBorder = true;
            this.m_lblPositionSecond.UseEdgeRadius = false;
            this.m_lblPositionSecond.UseImage = false;
            this.m_lblPositionSecond.UseSubFont = true;
            this.m_lblPositionSecond.UseUnitFont = false;
            // 
            // m_labelPositionSecond
            // 
            this.m_labelPositionSecond.BackGroundColor = System.Drawing.Color.White;
            this.m_labelPositionSecond.BorderStroke = 2;
            this.m_labelPositionSecond.BorderStyle = System.Windows.Forms.ButtonBorderStyle.Solid;
            this._tableLayoutPanel_Repeat.SetColumnSpan(this.m_labelPositionSecond, 2);
            this.m_labelPositionSecond.Description = "";
            this.m_labelPositionSecond.DisabledColor = System.Drawing.Color.LightGray;
            this.m_labelPositionSecond.Dock = System.Windows.Forms.DockStyle.Fill;
            this.m_labelPositionSecond.EdgeRadius = 1;
            this.m_labelPositionSecond.Enabled = false;
            this.m_labelPositionSecond.ImagePosition = new System.Drawing.Point(0, 0);
            this.m_labelPositionSecond.ImageSize = new System.Drawing.Point(0, 0);
            this.m_labelPositionSecond.LoadImage = null;
            this.m_labelPositionSecond.Location = new System.Drawing.Point(132, 108);
            this.m_labelPositionSecond.MainFont = new System.Drawing.Font("맑은 고딕", 11F, System.Drawing.FontStyle.Bold);
            this.m_labelPositionSecond.MainFontColor = System.Drawing.Color.Black;
            this.m_labelPositionSecond.Margin = new System.Windows.Forms.Padding(2, 3, 0, 3);
            this.m_labelPositionSecond.Name = "m_labelPositionSecond";
            this.m_labelPositionSecond.Size = new System.Drawing.Size(184, 40);
            this.m_labelPositionSecond.SubFont = new System.Drawing.Font("맑은 고딕", 10F);
            this.m_labelPositionSecond.SubFontColor = System.Drawing.Color.Black;
            this.m_labelPositionSecond.SubText = "";
            this.m_labelPositionSecond.TabIndex = 2;
            this.m_labelPositionSecond.Text = "--";
            this.m_labelPositionSecond.TextAlignMain = Sys3Controls.EN_TEXTALIGN.BOTTOM_RIGHT;
            this.m_labelPositionSecond.TextAlignSub = Sys3Controls.EN_TEXTALIGN.MIDDLE_CENTER;
            this.m_labelPositionSecond.TextAlignUnit = Sys3Controls.EN_TEXTALIGN.BOTTOM_RIGHT;
            this.m_labelPositionSecond.ThemeIndex = 0;
            this.m_labelPositionSecond.UnitAreaRate = 20;
            this.m_labelPositionSecond.UnitFont = new System.Drawing.Font("맑은 고딕", 10F, System.Drawing.FontStyle.Bold);
            this.m_labelPositionSecond.UnitFontColor = System.Drawing.Color.Black;
            this.m_labelPositionSecond.UnitPositionVertical = false;
            this.m_labelPositionSecond.UnitText = "mm";
            this.m_labelPositionSecond.UseBorder = true;
            this.m_labelPositionSecond.UseEdgeRadius = false;
            this.m_labelPositionSecond.UseImage = false;
            this.m_labelPositionSecond.UseSubFont = false;
            this.m_labelPositionSecond.UseUnitFont = true;
            this.m_labelPositionSecond.Click += new System.EventHandler(this.Click_Labels);
            // 
            // m_labelDelaySecond
            // 
            this.m_labelDelaySecond.BackGroundColor = System.Drawing.Color.White;
            this.m_labelDelaySecond.BorderStroke = 2;
            this.m_labelDelaySecond.BorderStyle = System.Windows.Forms.ButtonBorderStyle.Solid;
            this._tableLayoutPanel_Repeat.SetColumnSpan(this.m_labelDelaySecond, 2);
            this.m_labelDelaySecond.Description = "";
            this.m_labelDelaySecond.DisabledColor = System.Drawing.Color.LightGray;
            this.m_labelDelaySecond.Dock = System.Windows.Forms.DockStyle.Fill;
            this.m_labelDelaySecond.EdgeRadius = 1;
            this.m_labelDelaySecond.Enabled = false;
            this.m_labelDelaySecond.ImagePosition = new System.Drawing.Point(0, 0);
            this.m_labelDelaySecond.ImageSize = new System.Drawing.Point(0, 0);
            this.m_labelDelaySecond.LoadImage = null;
            this.m_labelDelaySecond.Location = new System.Drawing.Point(132, 154);
            this.m_labelDelaySecond.MainFont = new System.Drawing.Font("맑은 고딕", 11F, System.Drawing.FontStyle.Bold);
            this.m_labelDelaySecond.MainFontColor = System.Drawing.Color.Black;
            this.m_labelDelaySecond.Margin = new System.Windows.Forms.Padding(2, 3, 0, 3);
            this.m_labelDelaySecond.Name = "m_labelDelaySecond";
            this.m_labelDelaySecond.Size = new System.Drawing.Size(184, 41);
            this.m_labelDelaySecond.SubFont = new System.Drawing.Font("맑은 고딕", 10F);
            this.m_labelDelaySecond.SubFontColor = System.Drawing.Color.Black;
            this.m_labelDelaySecond.SubText = "";
            this.m_labelDelaySecond.TabIndex = 3;
            this.m_labelDelaySecond.Text = "--";
            this.m_labelDelaySecond.TextAlignMain = Sys3Controls.EN_TEXTALIGN.BOTTOM_RIGHT;
            this.m_labelDelaySecond.TextAlignSub = Sys3Controls.EN_TEXTALIGN.MIDDLE_CENTER;
            this.m_labelDelaySecond.TextAlignUnit = Sys3Controls.EN_TEXTALIGN.BOTTOM_RIGHT;
            this.m_labelDelaySecond.ThemeIndex = 0;
            this.m_labelDelaySecond.UnitAreaRate = 20;
            this.m_labelDelaySecond.UnitFont = new System.Drawing.Font("맑은 고딕", 10F, System.Drawing.FontStyle.Bold);
            this.m_labelDelaySecond.UnitFontColor = System.Drawing.Color.Black;
            this.m_labelDelaySecond.UnitPositionVertical = false;
            this.m_labelDelaySecond.UnitText = "ms";
            this.m_labelDelaySecond.UseBorder = true;
            this.m_labelDelaySecond.UseEdgeRadius = false;
            this.m_labelDelaySecond.UseImage = false;
            this.m_labelDelaySecond.UseSubFont = false;
            this.m_labelDelaySecond.UseUnitFont = true;
            this.m_labelDelaySecond.Click += new System.EventHandler(this.Click_Labels);
            // 
            // m_groupJog
            // 
            this.m_groupJog.BackGroundColor = System.Drawing.Color.WhiteSmoke;
            this.m_groupJog.Controls.Add(this._tableLayoutPanel_Jog);
            this.m_groupJog.Dock = System.Windows.Forms.DockStyle.Fill;
            this.m_groupJog.EdgeBorderStroke = 2;
            this.m_groupJog.EdgeRadius = 2;
            this.m_groupJog.LabelFont = new System.Drawing.Font("맑은 고딕", 10F, System.Drawing.FontStyle.Bold);
            this.m_groupJog.LabelGradientColorFirst = System.Drawing.Color.FromArgb(((int)(((byte)(208)))), ((int)(((byte)(208)))), ((int)(((byte)(208)))));
            this.m_groupJog.LabelGradientColorSecond = System.Drawing.Color.FromArgb(((int)(((byte)(240)))), ((int)(((byte)(240)))), ((int)(((byte)(240)))));
            this.m_groupJog.LabelHeight = 30;
            this.m_groupJog.LabelTextColor = System.Drawing.Color.Black;
            this.m_groupJog.Location = new System.Drawing.Point(837, 0);
            this.m_groupJog.Margin = new System.Windows.Forms.Padding(0);
            this.m_groupJog.Name = "m_groupJog";
            this.m_groupJog.Padding = new System.Windows.Forms.Padding(3, 18, 3, 3);
            this._tableLayoutPanel_Main.SetRowSpan(this.m_groupJog, 2);
            this.m_groupJog.Size = new System.Drawing.Size(353, 411);
            this.m_groupJog.TabIndex = 1375;
            this.m_groupJog.TabStop = false;
            this.m_groupJog.Text = "JOG";
            this.m_groupJog.TextAlign = Sys3Controls.EN_TEXTALIGN.MIDDLE_CENTER;
            this.m_groupJog.ThemeIndex = 0;
            this.m_groupJog.UseLabelBorder = true;
            this.m_groupJog.UseTitle = true;
            // 
            // _tableLayoutPanel_Jog
            // 
            this._tableLayoutPanel_Jog.ColumnCount = 2;
            this._tableLayoutPanel_Jog.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this._tableLayoutPanel_Jog.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this._tableLayoutPanel_Jog.Controls.Add(this.m_lblJogGroup, 0, 1);
            this._tableLayoutPanel_Jog.Controls.Add(this.m_btnMovePositive, 1, 5);
            this._tableLayoutPanel_Jog.Controls.Add(this.m_labelJogGroup, 1, 1);
            this._tableLayoutPanel_Jog.Controls.Add(this.m_btnMoveNegative, 0, 5);
            this._tableLayoutPanel_Jog.Controls.Add(this.panel1, 0, 3);
            this._tableLayoutPanel_Jog.Dock = System.Windows.Forms.DockStyle.Fill;
            this._tableLayoutPanel_Jog.Location = new System.Drawing.Point(3, 32);
            this._tableLayoutPanel_Jog.Margin = new System.Windows.Forms.Padding(0);
            this._tableLayoutPanel_Jog.Name = "_tableLayoutPanel_Jog";
            this._tableLayoutPanel_Jog.Padding = new System.Windows.Forms.Padding(5);
            this._tableLayoutPanel_Jog.RowCount = 7;
            this._tableLayoutPanel_Jog.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 6.098495F));
            this._tableLayoutPanel_Jog.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 12.21324F));
            this._tableLayoutPanel_Jog.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 6.058274F));
            this._tableLayoutPanel_Jog.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 30.72355F));
            this._tableLayoutPanel_Jog.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 7.067987F));
            this._tableLayoutPanel_Jog.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 30.72355F));
            this._tableLayoutPanel_Jog.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 7.114911F));
            this._tableLayoutPanel_Jog.Size = new System.Drawing.Size(347, 376);
            this._tableLayoutPanel_Jog.TabIndex = 0;
            // 
            // m_lblJogGroup
            // 
            this.m_lblJogGroup.BackGroundColor = System.Drawing.Color.FromArgb(((int)(((byte)(200)))), ((int)(((byte)(200)))), ((int)(((byte)(220)))));
            this.m_lblJogGroup.BorderStroke = 2;
            this.m_lblJogGroup.BorderStyle = System.Windows.Forms.ButtonBorderStyle.Solid;
            this.m_lblJogGroup.Description = "";
            this.m_lblJogGroup.DisabledColor = System.Drawing.Color.DarkGray;
            this.m_lblJogGroup.Dock = System.Windows.Forms.DockStyle.Fill;
            this.m_lblJogGroup.EdgeRadius = 1;
            this.m_lblJogGroup.ImagePosition = new System.Drawing.Point(0, 0);
            this.m_lblJogGroup.ImageSize = new System.Drawing.Point(0, 0);
            this.m_lblJogGroup.LoadImage = null;
            this.m_lblJogGroup.Location = new System.Drawing.Point(5, 27);
            this.m_lblJogGroup.MainFont = new System.Drawing.Font("맑은 고딕", 9F, System.Drawing.FontStyle.Bold);
            this.m_lblJogGroup.MainFontColor = System.Drawing.Color.Black;
            this.m_lblJogGroup.Margin = new System.Windows.Forms.Padding(0);
            this.m_lblJogGroup.Name = "m_lblJogGroup";
            this.m_lblJogGroup.Size = new System.Drawing.Size(168, 44);
            this.m_lblJogGroup.SubFont = new System.Drawing.Font("맑은 고딕", 9F, System.Drawing.FontStyle.Bold);
            this.m_lblJogGroup.SubFontColor = System.Drawing.Color.DarkBlue;
            this.m_lblJogGroup.SubText = "ACTUAL";
            this.m_lblJogGroup.TabIndex = 13831;
            this.m_lblJogGroup.Text = "JOG GROUP";
            this.m_lblJogGroup.TextAlignMain = Sys3Controls.EN_TEXTALIGN.TOP_LEFT;
            this.m_lblJogGroup.TextAlignSub = Sys3Controls.EN_TEXTALIGN.BOTTOM_RIGHT;
            this.m_lblJogGroup.TextAlignUnit = Sys3Controls.EN_TEXTALIGN.MIDDLE_CENTER;
            this.m_lblJogGroup.ThemeIndex = 0;
            this.m_lblJogGroup.UnitAreaRate = 30;
            this.m_lblJogGroup.UnitFont = new System.Drawing.Font("맑은 고딕", 10F, System.Drawing.FontStyle.Bold);
            this.m_lblJogGroup.UnitFontColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.m_lblJogGroup.UnitPositionVertical = false;
            this.m_lblJogGroup.UnitText = "";
            this.m_lblJogGroup.UseBorder = true;
            this.m_lblJogGroup.UseEdgeRadius = false;
            this.m_lblJogGroup.UseImage = false;
            this.m_lblJogGroup.UseSubFont = false;
            this.m_lblJogGroup.UseUnitFont = false;
            // 
            // m_btnMovePositive
            // 
            this.m_btnMovePositive.BorderWidth = 3;
            this.m_btnMovePositive.ButtonClicked = false;
            this.m_btnMovePositive.ClickedEmphasizeTextColor = System.Drawing.Color.White;
            this.m_btnMovePositive.CustomClickedGradientFirstColor = System.Drawing.Color.White;
            this.m_btnMovePositive.CustomClickedGradientSecondColor = System.Drawing.Color.White;
            this.m_btnMovePositive.Description = "";
            this.m_btnMovePositive.DisabledColor = System.Drawing.Color.DarkGray;
            this.m_btnMovePositive.Dock = System.Windows.Forms.DockStyle.Fill;
            this.m_btnMovePositive.EdgeRadius = 5;
            this.m_btnMovePositive.Enabled = false;
            this.m_btnMovePositive.GradientAngle = 70F;
            this.m_btnMovePositive.GradientFirstColor = System.Drawing.Color.White;
            this.m_btnMovePositive.GradientSecondColor = System.Drawing.Color.FromArgb(((int)(((byte)(170)))), ((int)(((byte)(176)))), ((int)(((byte)(183)))));
            this.m_btnMovePositive.HoverEmphasizeCustomColor = System.Drawing.Color.White;
            this.m_btnMovePositive.ImagePosition = new System.Drawing.Point(7, 7);
            this.m_btnMovePositive.ImageSize = new System.Drawing.Point(30, 30);
            this.m_btnMovePositive.LoadImage = global::FrameOfSystem3.Properties.Resources.CONNECT_BLACK;
            this.m_btnMovePositive.Location = new System.Drawing.Point(183, 230);
            this.m_btnMovePositive.MainFont = new System.Drawing.Font("맑은 고딕", 22F, System.Drawing.FontStyle.Bold);
            this.m_btnMovePositive.MainFontColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(36)))), ((int)(((byte)(0)))));
            this.m_btnMovePositive.Margin = new System.Windows.Forms.Padding(10, 0, 10, 0);
            this.m_btnMovePositive.Name = "m_btnMovePositive";
            this.m_btnMovePositive.Size = new System.Drawing.Size(149, 112);
            this.m_btnMovePositive.SubFont = new System.Drawing.Font("맑은 고딕", 8F, System.Drawing.FontStyle.Bold);
            this.m_btnMovePositive.SubFontColor = System.Drawing.Color.DarkBlue;
            this.m_btnMovePositive.SubText = "MOVE";
            this.m_btnMovePositive.TabIndex = 1;
            this.m_btnMovePositive.Text = ">>";
            this.m_btnMovePositive.TextAlignMain = Sys3Controls.EN_TEXTALIGN.MIDDLE_CENTER;
            this.m_btnMovePositive.TextAlignSub = Sys3Controls.EN_TEXTALIGN.TOP_RIGHT;
            this.m_btnMovePositive.ThemeIndex = 0;
            this.m_btnMovePositive.UseBorder = true;
            this.m_btnMovePositive.UseClickedEmphasizeTextColor = false;
            this.m_btnMovePositive.UseCustomizeClickedColor = false;
            this.m_btnMovePositive.UseEdge = true;
            this.m_btnMovePositive.UseHoverEmphasizeCustomColor = false;
            this.m_btnMovePositive.UseImage = false;
            this.m_btnMovePositive.UserHoverEmpahsize = false;
            this.m_btnMovePositive.UseSubFont = false;
            this.m_btnMovePositive.MouseDown += new System.Windows.Forms.MouseEventHandler(this.Down_MoveAtSpeed);
            this.m_btnMovePositive.MouseUp += new System.Windows.Forms.MouseEventHandler(this.Up_MoveAtSpeed);
            // 
            // m_labelJogGroup
            // 
            this.m_labelJogGroup.BackGroundColor = System.Drawing.Color.White;
            this.m_labelJogGroup.BorderStroke = 2;
            this.m_labelJogGroup.BorderStyle = System.Windows.Forms.ButtonBorderStyle.Solid;
            this.m_labelJogGroup.Description = "";
            this.m_labelJogGroup.DisabledColor = System.Drawing.Color.LightGray;
            this.m_labelJogGroup.Dock = System.Windows.Forms.DockStyle.Fill;
            this.m_labelJogGroup.EdgeRadius = 1;
            this.m_labelJogGroup.Enabled = false;
            this.m_labelJogGroup.ImagePosition = new System.Drawing.Point(0, 0);
            this.m_labelJogGroup.ImageSize = new System.Drawing.Point(0, 0);
            this.m_labelJogGroup.LoadImage = null;
            this.m_labelJogGroup.Location = new System.Drawing.Point(175, 27);
            this.m_labelJogGroup.MainFont = new System.Drawing.Font("맑은 고딕", 11F, System.Drawing.FontStyle.Bold);
            this.m_labelJogGroup.MainFontColor = System.Drawing.Color.Black;
            this.m_labelJogGroup.Margin = new System.Windows.Forms.Padding(2, 0, 0, 0);
            this.m_labelJogGroup.Name = "m_labelJogGroup";
            this.m_labelJogGroup.Size = new System.Drawing.Size(167, 44);
            this.m_labelJogGroup.SubFont = new System.Drawing.Font("맑은 고딕", 10F);
            this.m_labelJogGroup.SubFontColor = System.Drawing.Color.Black;
            this.m_labelJogGroup.SubText = "";
            this.m_labelJogGroup.TabIndex = 13832;
            this.m_labelJogGroup.Text = "--";
            this.m_labelJogGroup.TextAlignMain = Sys3Controls.EN_TEXTALIGN.BOTTOM_RIGHT;
            this.m_labelJogGroup.TextAlignSub = Sys3Controls.EN_TEXTALIGN.MIDDLE_CENTER;
            this.m_labelJogGroup.TextAlignUnit = Sys3Controls.EN_TEXTALIGN.BOTTOM_RIGHT;
            this.m_labelJogGroup.ThemeIndex = 0;
            this.m_labelJogGroup.UnitAreaRate = 20;
            this.m_labelJogGroup.UnitFont = new System.Drawing.Font("맑은 고딕", 10F, System.Drawing.FontStyle.Bold);
            this.m_labelJogGroup.UnitFontColor = System.Drawing.Color.Black;
            this.m_labelJogGroup.UnitPositionVertical = false;
            this.m_labelJogGroup.UnitText = "mm";
            this.m_labelJogGroup.UseBorder = true;
            this.m_labelJogGroup.UseEdgeRadius = false;
            this.m_labelJogGroup.UseImage = false;
            this.m_labelJogGroup.UseSubFont = false;
            this.m_labelJogGroup.UseUnitFont = false;
            // 
            // m_btnMoveNegative
            // 
            this.m_btnMoveNegative.BorderWidth = 3;
            this.m_btnMoveNegative.ButtonClicked = false;
            this.m_btnMoveNegative.ClickedEmphasizeTextColor = System.Drawing.Color.White;
            this.m_btnMoveNegative.CustomClickedGradientFirstColor = System.Drawing.Color.White;
            this.m_btnMoveNegative.CustomClickedGradientSecondColor = System.Drawing.Color.White;
            this.m_btnMoveNegative.Description = "";
            this.m_btnMoveNegative.DisabledColor = System.Drawing.Color.DarkGray;
            this.m_btnMoveNegative.Dock = System.Windows.Forms.DockStyle.Fill;
            this.m_btnMoveNegative.EdgeRadius = 5;
            this.m_btnMoveNegative.Enabled = false;
            this.m_btnMoveNegative.GradientAngle = 70F;
            this.m_btnMoveNegative.GradientFirstColor = System.Drawing.Color.White;
            this.m_btnMoveNegative.GradientSecondColor = System.Drawing.Color.FromArgb(((int)(((byte)(170)))), ((int)(((byte)(176)))), ((int)(((byte)(183)))));
            this.m_btnMoveNegative.HoverEmphasizeCustomColor = System.Drawing.Color.White;
            this.m_btnMoveNegative.ImagePosition = new System.Drawing.Point(7, 7);
            this.m_btnMoveNegative.ImageSize = new System.Drawing.Point(30, 30);
            this.m_btnMoveNegative.LoadImage = global::FrameOfSystem3.Properties.Resources.CONNECT_BLACK;
            this.m_btnMoveNegative.Location = new System.Drawing.Point(15, 230);
            this.m_btnMoveNegative.MainFont = new System.Drawing.Font("맑은 고딕", 22F, System.Drawing.FontStyle.Bold);
            this.m_btnMoveNegative.MainFontColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(36)))), ((int)(((byte)(0)))));
            this.m_btnMoveNegative.Margin = new System.Windows.Forms.Padding(10, 0, 10, 0);
            this.m_btnMoveNegative.Name = "m_btnMoveNegative";
            this.m_btnMoveNegative.Size = new System.Drawing.Size(148, 112);
            this.m_btnMoveNegative.SubFont = new System.Drawing.Font("맑은 고딕", 8F, System.Drawing.FontStyle.Bold);
            this.m_btnMoveNegative.SubFontColor = System.Drawing.Color.DarkBlue;
            this.m_btnMoveNegative.SubText = "MOVE";
            this.m_btnMoveNegative.TabIndex = 0;
            this.m_btnMoveNegative.Text = "<<";
            this.m_btnMoveNegative.TextAlignMain = Sys3Controls.EN_TEXTALIGN.MIDDLE_CENTER;
            this.m_btnMoveNegative.TextAlignSub = Sys3Controls.EN_TEXTALIGN.TOP_RIGHT;
            this.m_btnMoveNegative.ThemeIndex = 0;
            this.m_btnMoveNegative.UseBorder = true;
            this.m_btnMoveNegative.UseClickedEmphasizeTextColor = false;
            this.m_btnMoveNegative.UseCustomizeClickedColor = false;
            this.m_btnMoveNegative.UseEdge = true;
            this.m_btnMoveNegative.UseHoverEmphasizeCustomColor = false;
            this.m_btnMoveNegative.UseImage = false;
            this.m_btnMoveNegative.UserHoverEmpahsize = false;
            this.m_btnMoveNegative.UseSubFont = false;
            this.m_btnMoveNegative.MouseDown += new System.Windows.Forms.MouseEventHandler(this.Down_MoveAtSpeed);
            this.m_btnMoveNegative.MouseUp += new System.Windows.Forms.MouseEventHandler(this.Up_MoveAtSpeed);
            // 
            // panel1
            // 
            this.panel1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)));
            this._tableLayoutPanel_Jog.SetColumnSpan(this.panel1, 2);
            this.panel1.Controls.Add(this.m_progressbarJog);
            this.panel1.Location = new System.Drawing.Point(8, 96);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(331, 106);
            this.panel1.TabIndex = 13833;
            // 
            // m_progressbarJog
            // 
            this.m_progressbarJog.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)));
            this.m_progressbarJog.BackGroundColor = System.Drawing.Color.White;
            this.m_progressbarJog.BorderStroke = 1;
            this.m_progressbarJog.Enabled = false;
            this.m_progressbarJog.FirstColor = System.Drawing.Color.Gainsboro;
            this.m_progressbarJog.GageCount = 10;
            this.m_progressbarJog.LabelWidth = 50;
            this.m_progressbarJog.Location = new System.Drawing.Point(14, 0);
            this.m_progressbarJog.Margin = new System.Windows.Forms.Padding(0);
            this.m_progressbarJog.MaxTickCount = ((uint)(100u));
            this.m_progressbarJog.MinTickCount = ((uint)(1u));
            this.m_progressbarJog.Name = "m_progressbarJog";
            this.m_progressbarJog.NormalGageColor = System.Drawing.Color.LightGray;
            this.m_progressbarJog.OffSet = 2;
            this.m_progressbarJog.Padding = new System.Windows.Forms.Padding(5);
            this.m_progressbarJog.SecondColor = System.Drawing.Color.DodgerBlue;
            this.m_progressbarJog.Size = new System.Drawing.Size(302, 106);
            this.m_progressbarJog.TabIndex = 13837;
            this.m_progressbarJog.Tick = ((uint)(50u));
            this.m_progressbarJog.TickFont = new System.Drawing.Font("맑은 고딕", 14F, System.Drawing.FontStyle.Bold);
            this.m_progressbarJog.TickFontColor = System.Drawing.Color.Black;
            this.m_progressbarJog.UseBorder = true;
            this.m_progressbarJog.UseControlBorder = true;
            this.m_progressbarJog.UseLinearGradientMode = true;
            // 
            // _tableLayoutPanel_Main
            // 
            this._tableLayoutPanel_Main.ColumnCount = 3;
            this._tableLayoutPanel_Main.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 23.28251F));
            this._tableLayoutPanel_Main.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 47.0852F));
            this._tableLayoutPanel_Main.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 29.63229F));
            this._tableLayoutPanel_Main.Controls.Add(this.m_groupState, 0, 0);
            this._tableLayoutPanel_Main.Controls.Add(this.m_groupPosition, 1, 0);
            this._tableLayoutPanel_Main.Controls.Add(this.m_groupRepeat, 1, 1);
            this._tableLayoutPanel_Main.Controls.Add(this.m_groupJog, 2, 0);
            this._tableLayoutPanel_Main.Dock = System.Windows.Forms.DockStyle.Fill;
            this._tableLayoutPanel_Main.Location = new System.Drawing.Point(0, 0);
            this._tableLayoutPanel_Main.Margin = new System.Windows.Forms.Padding(0);
            this._tableLayoutPanel_Main.Name = "_tableLayoutPanel_Main";
            this._tableLayoutPanel_Main.RowCount = 2;
            this._tableLayoutPanel_Main.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 42.6F));
            this._tableLayoutPanel_Main.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 57.4F));
            this._tableLayoutPanel_Main.Size = new System.Drawing.Size(1190, 411);
            this._tableLayoutPanel_Main.TabIndex = 13842;
            // 
            // MotorState
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.Controls.Add(this._tableLayoutPanel_Main);
            this.Margin = new System.Windows.Forms.Padding(0);
            this.Name = "MotorState";
            this.Size = new System.Drawing.Size(1190, 411);
            this.m_groupState.ResumeLayout(false);
            this._tableLayoutPanel_GantryConfiguration.ResumeLayout(false);
            this.m_groupPosition.ResumeLayout(false);
            this._tableLayoutPanel_Position.ResumeLayout(false);
            this.m_groupRepeat.ResumeLayout(false);
            this._tableLayoutPanel_Repeat.ResumeLayout(false);
            this.m_groupJog.ResumeLayout(false);
            this._tableLayoutPanel_Jog.ResumeLayout(false);
            this.panel1.ResumeLayout(false);
            this._tableLayoutPanel_Main.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

		private Sys3Controls.Sys3LedLabel m_ledSWLimitPositive;
		private Sys3Controls.Sys3LedLabel m_ledSWLimitNegative;
		private Sys3Controls.Sys3LedLabel m_ledRepeat;
		private Sys3Controls.Sys3LedLabel m_ledHomeTimeout;
		private Sys3Controls.Sys3LedLabel m_ledHomeEnd;
		private Sys3Controls.Sys3LedLabel m_ledHomeSwitch;
		private Sys3Controls.Sys3LedLabel m_ledMotionTimeout;
		private Sys3Controls.Sys3LedLabel m_ledMotionDone;
		private Sys3Controls.Sys3LedLabel m_ledLimitPositive;
		private Sys3Controls.Sys3LedLabel m_ledLimitNegative;
		private Sys3Controls.Sys3LedLabel m_ledAlarm;
		private Sys3Controls.Sys3LedLabel m_ledMotorOn;
		private Sys3Controls.Sys3GroupBoxContainer m_groupState;
        private Sys3Controls.Sys3GroupBoxContainer m_groupPosition;
        private Sys3Controls.Sys3GroupBoxContainer m_groupRepeat;
        private Sys3Controls.Sys3GroupBoxContainer m_groupJog;
		private Sys3Controls.Sys3Label m_lblCommand;
		private Sys3Controls.Sys3Label m_lblActual;
		private Sys3Controls.Sys3Label m_lblError;
		private Sys3Controls.Sys3Label m_labelCommand;
		private Sys3Controls.Sys3Label m_labelActual;
		private Sys3Controls.Sys3Label m_labelError;
		private Sys3Controls.Sys3Label m_lblPositionFirst;
		private Sys3Controls.Sys3Label m_lblPositionSecond;
		private Sys3Controls.Sys3Label m_lblDelayFirst;
		private Sys3Controls.Sys3Label m_lblDelaySecond;
		private Sys3Controls.Sys3Label m_labelPositionFirst;
		private Sys3Controls.Sys3Label m_labelDelayFirst;
		private Sys3Controls.Sys3Label m_labelPositionSecond;
		private Sys3Controls.Sys3Label m_labelDelaySecond;
		private Sys3Controls.Sys3Label m_lblJogGroup;
		private Sys3Controls.Sys3Label m_labelJogGroup;
		private Sys3Controls.Sys3Label m_lblMotorOn;
		private Sys3Controls.Sys3Label m_lblAlarm;
		private Sys3Controls.Sys3Label m_lblLimitNegative;
		private Sys3Controls.Sys3Label m_lblSWLimitNegative;
		private Sys3Controls.Sys3Label m_lblSWLimitPositive;
		private Sys3Controls.Sys3Label m_lblLimitPositive;
		private Sys3Controls.Sys3Label m_lblMotionTimeout;
		private Sys3Controls.Sys3Label m_lblMotionDone;
		private Sys3Controls.Sys3Label m_lblHomeEnd;
		private Sys3Controls.Sys3Label m_lblHomeSwitch;
		private Sys3Controls.Sys3Label m_lblHomeTimeout;
		private Sys3Controls.Sys3button m_btnClear;
		private Sys3Controls.Sys3button m_btnMoveFirst;
		private Sys3Controls.Sys3button m_btnMoveSecond;
		private Sys3Controls.Sys3button m_btnRepeat;
		private Sys3Controls.Sys3button m_btnMoveNegative;
		private Sys3Controls.Sys3button m_btnMovePositive;
		private Sys3Controls.Sys3ProgressBar m_progressbarJog;
		private Sys3Controls.Sys3LedLabel m_ledFirstPosition;
		private Sys3Controls.Sys3LedLabel m_ledFirstDelay;
		private Sys3Controls.Sys3LedLabel m_ledSecondPosition;
		private Sys3Controls.Sys3LedLabel m_ledSecondDelay;
        private Sys3Controls.Sys3LedLabel m_ledInterlock;
        private Sys3Controls.Sys3Label m_lblInterlock;
        private System.Windows.Forms.TableLayoutPanel _tableLayoutPanel_Main;
        private System.Windows.Forms.TableLayoutPanel _tableLayoutPanel_GantryConfiguration;
        private System.Windows.Forms.TableLayoutPanel _tableLayoutPanel_Position;
        private System.Windows.Forms.TableLayoutPanel _tableLayoutPanel_Repeat;
        private System.Windows.Forms.TableLayoutPanel _tableLayoutPanel_Jog;
        private System.Windows.Forms.Panel panel1;
    }
}
