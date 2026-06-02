
namespace EFEM.CustomizedByProcessType.UserInterface.OperationMainMonitoring.PWA500W
{
    partial class MainMonitoringSubPanel500WDigitals
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
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.lblIonizer3 = new Sys3Controls.Sys3LedLabelWithText();
            this.lblDoorOpened = new Sys3Controls.Sys3LedLabelWithText();
            this.lblPowerBoxFan = new Sys3Controls.Sys3LedLabelWithText();
            this.lblIOBoxFan = new Sys3Controls.Sys3LedLabelWithText();
            this.lblFFUAlarm = new Sys3Controls.Sys3LedLabelWithText();
            this.lblIonizer1 = new Sys3Controls.Sys3LedLabelWithText();
            this.sys3LedLabelWithText1 = new Sys3Controls.Sys3LedLabelWithText();
            this.tableLayoutPanel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 4;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.tableLayoutPanel1.Controls.Add(this.lblIonizer3, 0, 1);
            this.tableLayoutPanel1.Controls.Add(this.lblDoorOpened, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.lblPowerBoxFan, 1, 0);
            this.tableLayoutPanel1.Controls.Add(this.lblIOBoxFan, 2, 0);
            this.tableLayoutPanel1.Controls.Add(this.lblFFUAlarm, 3, 0);
            this.tableLayoutPanel1.Controls.Add(this.lblIonizer1, 1, 1);
            this.tableLayoutPanel1.Controls.Add(this.sys3LedLabelWithText1, 0, 1);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel1.Margin = new System.Windows.Forms.Padding(0);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 2;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(624, 96);
            this.tableLayoutPanel1.TabIndex = 4;
            // 
            // lblIonizer3
            // 
            this.lblIonizer3.Active = false;
            this.lblIonizer3.ActiveMessage = "FOUP_8 AREA\\nIONIZER ALARM";
            this.lblIonizer3.BackGroundColor = System.Drawing.Color.Bisque;
            this.lblIonizer3.BorderStroke = 2;
            this.lblIonizer3.BorderStyle = System.Windows.Forms.ButtonBorderStyle.Solid;
            this.lblIonizer3.ColorActive = System.Drawing.Color.Red;
            this.lblIonizer3.ColorNormal = System.Drawing.Color.LimeGreen;
            this.lblIonizer3.Description = "";
            this.lblIonizer3.DisabledColor = System.Drawing.Color.DarkGray;
            this.lblIonizer3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblIonizer3.EdgeRadius = 1;
            this.lblIonizer3.ImagePosition = new System.Drawing.Point(0, 0);
            this.lblIonizer3.ImageSize = new System.Drawing.Point(0, 0);
            this.lblIonizer3.LedWidth = 15;
            this.lblIonizer3.LoadImage = null;
            this.lblIonizer3.Location = new System.Drawing.Point(157, 49);
            this.lblIonizer3.MainFont = new System.Drawing.Font("맑은 고딕", 8F, System.Drawing.FontStyle.Bold);
            this.lblIonizer3.MainFontColor = System.Drawing.Color.Black;
            this.lblIonizer3.Margin = new System.Windows.Forms.Padding(1);
            this.lblIonizer3.Name = "lblIonizer3";
            this.lblIonizer3.Padding = new System.Windows.Forms.Padding(5);
            this.lblIonizer3.Size = new System.Drawing.Size(154, 46);
            this.lblIonizer3.SubFont = new System.Drawing.Font("맑은 고딕", 8F, System.Drawing.FontStyle.Bold);
            this.lblIonizer3.SubFontColor = System.Drawing.Color.Red;
            this.lblIonizer3.SubText = "IONIZER";
            this.lblIonizer3.TabIndex = 30;
            this.lblIonizer3.Tag = "52";
            this.lblIonizer3.Text = "FOUP_8 AREA\\nIONIZER";
            this.lblIonizer3.TextAlignMain = Sys3Controls.EN_TEXTALIGN.MIDDLE_CENTER;
            this.lblIonizer3.TextAlignSub = Sys3Controls.EN_TEXTALIGN.TOP_LEFT;
            this.lblIonizer3.TextAlignUnit = Sys3Controls.EN_TEXTALIGN.BOTTOM_RIGHT;
            this.lblIonizer3.TextMargin = 5;
            this.lblIonizer3.ThemeIndex = 0;
            this.lblIonizer3.UnitAreaRate = 10;
            this.lblIonizer3.UnitFont = new System.Drawing.Font("맑은 고딕", 9F, System.Drawing.FontStyle.Bold);
            this.lblIonizer3.UnitFontColor = System.Drawing.Color.Red;
            this.lblIonizer3.UnitPositionVertical = false;
            this.lblIonizer3.UnitText = "MPa";
            this.lblIonizer3.UseActiveMessage = false;
            this.lblIonizer3.UseBorder = true;
            this.lblIonizer3.UseEdgeRadius = false;
            this.lblIonizer3.UseImage = false;
            this.lblIonizer3.UseSubFont = false;
            this.lblIonizer3.UseUnitFont = false;
            // 
            // lblDoorOpened
            // 
            this.lblDoorOpened.Active = false;
            this.lblDoorOpened.ActiveMessage = "DOOR LOCKED";
            this.lblDoorOpened.BackGroundColor = System.Drawing.Color.Bisque;
            this.lblDoorOpened.BorderStroke = 2;
            this.lblDoorOpened.BorderStyle = System.Windows.Forms.ButtonBorderStyle.Solid;
            this.lblDoorOpened.ColorActive = System.Drawing.Color.LimeGreen;
            this.lblDoorOpened.ColorNormal = System.Drawing.Color.Red;
            this.lblDoorOpened.Description = "";
            this.lblDoorOpened.DisabledColor = System.Drawing.Color.DarkGray;
            this.lblDoorOpened.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblDoorOpened.EdgeRadius = 1;
            this.lblDoorOpened.ImagePosition = new System.Drawing.Point(0, 0);
            this.lblDoorOpened.ImageSize = new System.Drawing.Point(0, 0);
            this.lblDoorOpened.LedWidth = 15;
            this.lblDoorOpened.LoadImage = null;
            this.lblDoorOpened.Location = new System.Drawing.Point(1, 1);
            this.lblDoorOpened.MainFont = new System.Drawing.Font("맑은 고딕", 8F, System.Drawing.FontStyle.Bold);
            this.lblDoorOpened.MainFontColor = System.Drawing.Color.Black;
            this.lblDoorOpened.Margin = new System.Windows.Forms.Padding(1);
            this.lblDoorOpened.Name = "lblDoorOpened";
            this.lblDoorOpened.Padding = new System.Windows.Forms.Padding(5);
            this.lblDoorOpened.Size = new System.Drawing.Size(154, 46);
            this.lblDoorOpened.SubFont = new System.Drawing.Font("맑은 고딕", 8F, System.Drawing.FontStyle.Bold);
            this.lblDoorOpened.SubFontColor = System.Drawing.Color.Red;
            this.lblDoorOpened.SubText = "IONIZER";
            this.lblDoorOpened.TabIndex = 25;
            this.lblDoorOpened.Tag = "86";
            this.lblDoorOpened.Text = "DOOR UNLOCKED";
            this.lblDoorOpened.TextAlignMain = Sys3Controls.EN_TEXTALIGN.MIDDLE_CENTER;
            this.lblDoorOpened.TextAlignSub = Sys3Controls.EN_TEXTALIGN.TOP_LEFT;
            this.lblDoorOpened.TextAlignUnit = Sys3Controls.EN_TEXTALIGN.BOTTOM_RIGHT;
            this.lblDoorOpened.TextMargin = 5;
            this.lblDoorOpened.ThemeIndex = 0;
            this.lblDoorOpened.UnitAreaRate = 10;
            this.lblDoorOpened.UnitFont = new System.Drawing.Font("맑은 고딕", 9F, System.Drawing.FontStyle.Bold);
            this.lblDoorOpened.UnitFontColor = System.Drawing.Color.Red;
            this.lblDoorOpened.UnitPositionVertical = false;
            this.lblDoorOpened.UnitText = "MPa";
            this.lblDoorOpened.UseActiveMessage = true;
            this.lblDoorOpened.UseBorder = true;
            this.lblDoorOpened.UseEdgeRadius = false;
            this.lblDoorOpened.UseImage = false;
            this.lblDoorOpened.UseSubFont = false;
            this.lblDoorOpened.UseUnitFont = false;
            // 
            // lblPowerBoxFan
            // 
            this.lblPowerBoxFan.Active = false;
            this.lblPowerBoxFan.ActiveMessage = "POWER BOX FAN ALARM";
            this.lblPowerBoxFan.BackGroundColor = System.Drawing.Color.Bisque;
            this.lblPowerBoxFan.BorderStroke = 2;
            this.lblPowerBoxFan.BorderStyle = System.Windows.Forms.ButtonBorderStyle.Solid;
            this.lblPowerBoxFan.ColorActive = System.Drawing.Color.Red;
            this.lblPowerBoxFan.ColorNormal = System.Drawing.Color.LimeGreen;
            this.lblPowerBoxFan.Description = "";
            this.lblPowerBoxFan.DisabledColor = System.Drawing.Color.DarkGray;
            this.lblPowerBoxFan.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblPowerBoxFan.EdgeRadius = 1;
            this.lblPowerBoxFan.ImagePosition = new System.Drawing.Point(0, 0);
            this.lblPowerBoxFan.ImageSize = new System.Drawing.Point(0, 0);
            this.lblPowerBoxFan.LedWidth = 15;
            this.lblPowerBoxFan.LoadImage = null;
            this.lblPowerBoxFan.Location = new System.Drawing.Point(157, 1);
            this.lblPowerBoxFan.MainFont = new System.Drawing.Font("맑은 고딕", 8F, System.Drawing.FontStyle.Bold);
            this.lblPowerBoxFan.MainFontColor = System.Drawing.Color.Black;
            this.lblPowerBoxFan.Margin = new System.Windows.Forms.Padding(1);
            this.lblPowerBoxFan.Name = "lblPowerBoxFan";
            this.lblPowerBoxFan.Padding = new System.Windows.Forms.Padding(5);
            this.lblPowerBoxFan.Size = new System.Drawing.Size(154, 46);
            this.lblPowerBoxFan.SubFont = new System.Drawing.Font("맑은 고딕", 8F, System.Drawing.FontStyle.Bold);
            this.lblPowerBoxFan.SubFontColor = System.Drawing.Color.Red;
            this.lblPowerBoxFan.SubText = "IONIZER";
            this.lblPowerBoxFan.TabIndex = 26;
            this.lblPowerBoxFan.Tag = "48";
            this.lblPowerBoxFan.Text = "POWER BOX FAN";
            this.lblPowerBoxFan.TextAlignMain = Sys3Controls.EN_TEXTALIGN.MIDDLE_CENTER;
            this.lblPowerBoxFan.TextAlignSub = Sys3Controls.EN_TEXTALIGN.TOP_LEFT;
            this.lblPowerBoxFan.TextAlignUnit = Sys3Controls.EN_TEXTALIGN.BOTTOM_RIGHT;
            this.lblPowerBoxFan.TextMargin = 5;
            this.lblPowerBoxFan.ThemeIndex = 0;
            this.lblPowerBoxFan.UnitAreaRate = 10;
            this.lblPowerBoxFan.UnitFont = new System.Drawing.Font("맑은 고딕", 9F, System.Drawing.FontStyle.Bold);
            this.lblPowerBoxFan.UnitFontColor = System.Drawing.Color.Red;
            this.lblPowerBoxFan.UnitPositionVertical = false;
            this.lblPowerBoxFan.UnitText = "MPa";
            this.lblPowerBoxFan.UseActiveMessage = false;
            this.lblPowerBoxFan.UseBorder = true;
            this.lblPowerBoxFan.UseEdgeRadius = false;
            this.lblPowerBoxFan.UseImage = false;
            this.lblPowerBoxFan.UseSubFont = false;
            this.lblPowerBoxFan.UseUnitFont = false;
            // 
            // lblIOBoxFan
            // 
            this.lblIOBoxFan.Active = false;
            this.lblIOBoxFan.ActiveMessage = "IO BOX FAN ALARM";
            this.lblIOBoxFan.BackGroundColor = System.Drawing.Color.Bisque;
            this.lblIOBoxFan.BorderStroke = 2;
            this.lblIOBoxFan.BorderStyle = System.Windows.Forms.ButtonBorderStyle.Solid;
            this.lblIOBoxFan.ColorActive = System.Drawing.Color.Red;
            this.lblIOBoxFan.ColorNormal = System.Drawing.Color.LimeGreen;
            this.lblIOBoxFan.Description = "";
            this.lblIOBoxFan.DisabledColor = System.Drawing.Color.DarkGray;
            this.lblIOBoxFan.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblIOBoxFan.EdgeRadius = 1;
            this.lblIOBoxFan.ImagePosition = new System.Drawing.Point(0, 0);
            this.lblIOBoxFan.ImageSize = new System.Drawing.Point(0, 0);
            this.lblIOBoxFan.LedWidth = 15;
            this.lblIOBoxFan.LoadImage = null;
            this.lblIOBoxFan.Location = new System.Drawing.Point(313, 1);
            this.lblIOBoxFan.MainFont = new System.Drawing.Font("맑은 고딕", 8F, System.Drawing.FontStyle.Bold);
            this.lblIOBoxFan.MainFontColor = System.Drawing.Color.Black;
            this.lblIOBoxFan.Margin = new System.Windows.Forms.Padding(1);
            this.lblIOBoxFan.Name = "lblIOBoxFan";
            this.lblIOBoxFan.Padding = new System.Windows.Forms.Padding(5);
            this.lblIOBoxFan.Size = new System.Drawing.Size(154, 46);
            this.lblIOBoxFan.SubFont = new System.Drawing.Font("맑은 고딕", 8F, System.Drawing.FontStyle.Bold);
            this.lblIOBoxFan.SubFontColor = System.Drawing.Color.Red;
            this.lblIOBoxFan.SubText = "IONIZER";
            this.lblIOBoxFan.TabIndex = 27;
            this.lblIOBoxFan.Tag = "49";
            this.lblIOBoxFan.Text = "IO BOX FAN";
            this.lblIOBoxFan.TextAlignMain = Sys3Controls.EN_TEXTALIGN.MIDDLE_CENTER;
            this.lblIOBoxFan.TextAlignSub = Sys3Controls.EN_TEXTALIGN.TOP_LEFT;
            this.lblIOBoxFan.TextAlignUnit = Sys3Controls.EN_TEXTALIGN.BOTTOM_RIGHT;
            this.lblIOBoxFan.TextMargin = 5;
            this.lblIOBoxFan.ThemeIndex = 0;
            this.lblIOBoxFan.UnitAreaRate = 10;
            this.lblIOBoxFan.UnitFont = new System.Drawing.Font("맑은 고딕", 9F, System.Drawing.FontStyle.Bold);
            this.lblIOBoxFan.UnitFontColor = System.Drawing.Color.Red;
            this.lblIOBoxFan.UnitPositionVertical = false;
            this.lblIOBoxFan.UnitText = "MPa";
            this.lblIOBoxFan.UseActiveMessage = false;
            this.lblIOBoxFan.UseBorder = true;
            this.lblIOBoxFan.UseEdgeRadius = false;
            this.lblIOBoxFan.UseImage = false;
            this.lblIOBoxFan.UseSubFont = false;
            this.lblIOBoxFan.UseUnitFont = false;
            // 
            // lblFFUAlarm
            // 
            this.lblFFUAlarm.Active = false;
            this.lblFFUAlarm.ActiveMessage = "FAN FILTER UNIT ALARM";
            this.lblFFUAlarm.BackGroundColor = System.Drawing.Color.Bisque;
            this.lblFFUAlarm.BorderStroke = 2;
            this.lblFFUAlarm.BorderStyle = System.Windows.Forms.ButtonBorderStyle.Solid;
            this.lblFFUAlarm.ColorActive = System.Drawing.Color.Red;
            this.lblFFUAlarm.ColorNormal = System.Drawing.Color.LimeGreen;
            this.lblFFUAlarm.Description = "";
            this.lblFFUAlarm.DisabledColor = System.Drawing.Color.DarkGray;
            this.lblFFUAlarm.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblFFUAlarm.EdgeRadius = 1;
            this.lblFFUAlarm.ImagePosition = new System.Drawing.Point(0, 0);
            this.lblFFUAlarm.ImageSize = new System.Drawing.Point(0, 0);
            this.lblFFUAlarm.LedWidth = 15;
            this.lblFFUAlarm.LoadImage = null;
            this.lblFFUAlarm.Location = new System.Drawing.Point(469, 1);
            this.lblFFUAlarm.MainFont = new System.Drawing.Font("맑은 고딕", 8F, System.Drawing.FontStyle.Bold);
            this.lblFFUAlarm.MainFontColor = System.Drawing.Color.Black;
            this.lblFFUAlarm.Margin = new System.Windows.Forms.Padding(1);
            this.lblFFUAlarm.Name = "lblFFUAlarm";
            this.lblFFUAlarm.Padding = new System.Windows.Forms.Padding(5);
            this.lblFFUAlarm.Size = new System.Drawing.Size(154, 46);
            this.lblFFUAlarm.SubFont = new System.Drawing.Font("맑은 고딕", 8F, System.Drawing.FontStyle.Bold);
            this.lblFFUAlarm.SubFontColor = System.Drawing.Color.Red;
            this.lblFFUAlarm.SubText = "IONIZER";
            this.lblFFUAlarm.TabIndex = 28;
            this.lblFFUAlarm.Tag = "50";
            this.lblFFUAlarm.Text = "FAN FILTER UNIT";
            this.lblFFUAlarm.TextAlignMain = Sys3Controls.EN_TEXTALIGN.MIDDLE_CENTER;
            this.lblFFUAlarm.TextAlignSub = Sys3Controls.EN_TEXTALIGN.TOP_LEFT;
            this.lblFFUAlarm.TextAlignUnit = Sys3Controls.EN_TEXTALIGN.BOTTOM_RIGHT;
            this.lblFFUAlarm.TextMargin = 5;
            this.lblFFUAlarm.ThemeIndex = 0;
            this.lblFFUAlarm.UnitAreaRate = 10;
            this.lblFFUAlarm.UnitFont = new System.Drawing.Font("맑은 고딕", 9F, System.Drawing.FontStyle.Bold);
            this.lblFFUAlarm.UnitFontColor = System.Drawing.Color.Red;
            this.lblFFUAlarm.UnitPositionVertical = false;
            this.lblFFUAlarm.UnitText = "MPa";
            this.lblFFUAlarm.UseActiveMessage = false;
            this.lblFFUAlarm.UseBorder = true;
            this.lblFFUAlarm.UseEdgeRadius = false;
            this.lblFFUAlarm.UseImage = false;
            this.lblFFUAlarm.UseSubFont = false;
            this.lblFFUAlarm.UseUnitFont = false;
            // 
            // lblIonizer1
            // 
            this.lblIonizer1.Active = false;
            this.lblIonizer1.ActiveMessage = "EQUIPMENT AREA\\nIONIZER ALARM";
            this.lblIonizer1.BackGroundColor = System.Drawing.Color.Bisque;
            this.lblIonizer1.BorderStroke = 2;
            this.lblIonizer1.BorderStyle = System.Windows.Forms.ButtonBorderStyle.Solid;
            this.lblIonizer1.ColorActive = System.Drawing.Color.Red;
            this.lblIonizer1.ColorNormal = System.Drawing.Color.LimeGreen;
            this.lblIonizer1.Description = "";
            this.lblIonizer1.DisabledColor = System.Drawing.Color.DarkGray;
            this.lblIonizer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblIonizer1.EdgeRadius = 1;
            this.lblIonizer1.ImagePosition = new System.Drawing.Point(0, 0);
            this.lblIonizer1.ImageSize = new System.Drawing.Point(0, 0);
            this.lblIonizer1.LedWidth = 15;
            this.lblIonizer1.LoadImage = null;
            this.lblIonizer1.Location = new System.Drawing.Point(313, 49);
            this.lblIonizer1.MainFont = new System.Drawing.Font("맑은 고딕", 8F, System.Drawing.FontStyle.Bold);
            this.lblIonizer1.MainFontColor = System.Drawing.Color.Black;
            this.lblIonizer1.Margin = new System.Windows.Forms.Padding(1);
            this.lblIonizer1.Name = "lblIonizer1";
            this.lblIonizer1.Padding = new System.Windows.Forms.Padding(5);
            this.lblIonizer1.Size = new System.Drawing.Size(154, 46);
            this.lblIonizer1.SubFont = new System.Drawing.Font("맑은 고딕", 8F, System.Drawing.FontStyle.Bold);
            this.lblIonizer1.SubFontColor = System.Drawing.Color.Red;
            this.lblIonizer1.SubText = "IONIZER";
            this.lblIonizer1.TabIndex = 31;
            this.lblIonizer1.Tag = "53";
            this.lblIonizer1.Text = "EQUIPMENT AREA\\nIONIZER";
            this.lblIonizer1.TextAlignMain = Sys3Controls.EN_TEXTALIGN.MIDDLE_CENTER;
            this.lblIonizer1.TextAlignSub = Sys3Controls.EN_TEXTALIGN.TOP_LEFT;
            this.lblIonizer1.TextAlignUnit = Sys3Controls.EN_TEXTALIGN.BOTTOM_RIGHT;
            this.lblIonizer1.TextMargin = 5;
            this.lblIonizer1.ThemeIndex = 0;
            this.lblIonizer1.UnitAreaRate = 10;
            this.lblIonizer1.UnitFont = new System.Drawing.Font("맑은 고딕", 9F, System.Drawing.FontStyle.Bold);
            this.lblIonizer1.UnitFontColor = System.Drawing.Color.Red;
            this.lblIonizer1.UnitPositionVertical = false;
            this.lblIonizer1.UnitText = "MPa";
            this.lblIonizer1.UseActiveMessage = false;
            this.lblIonizer1.UseBorder = true;
            this.lblIonizer1.UseEdgeRadius = false;
            this.lblIonizer1.UseImage = false;
            this.lblIonizer1.UseSubFont = false;
            this.lblIonizer1.UseUnitFont = false;
            // 
            // sys3LedLabelWithText1
            // 
            this.sys3LedLabelWithText1.Active = false;
            this.sys3LedLabelWithText1.ActiveMessage = "FOUP_12 AREA\\nIONIZER ALARM";
            this.sys3LedLabelWithText1.BackGroundColor = System.Drawing.Color.Bisque;
            this.sys3LedLabelWithText1.BorderStroke = 2;
            this.sys3LedLabelWithText1.BorderStyle = System.Windows.Forms.ButtonBorderStyle.Solid;
            this.sys3LedLabelWithText1.ColorActive = System.Drawing.Color.Red;
            this.sys3LedLabelWithText1.ColorNormal = System.Drawing.Color.LimeGreen;
            this.sys3LedLabelWithText1.Description = "";
            this.sys3LedLabelWithText1.DisabledColor = System.Drawing.Color.DarkGray;
            this.sys3LedLabelWithText1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.sys3LedLabelWithText1.EdgeRadius = 1;
            this.sys3LedLabelWithText1.ImagePosition = new System.Drawing.Point(0, 0);
            this.sys3LedLabelWithText1.ImageSize = new System.Drawing.Point(0, 0);
            this.sys3LedLabelWithText1.LedWidth = 15;
            this.sys3LedLabelWithText1.LoadImage = null;
            this.sys3LedLabelWithText1.Location = new System.Drawing.Point(1, 49);
            this.sys3LedLabelWithText1.MainFont = new System.Drawing.Font("맑은 고딕", 8F, System.Drawing.FontStyle.Bold);
            this.sys3LedLabelWithText1.MainFontColor = System.Drawing.Color.Black;
            this.sys3LedLabelWithText1.Margin = new System.Windows.Forms.Padding(1);
            this.sys3LedLabelWithText1.Name = "sys3LedLabelWithText1";
            this.sys3LedLabelWithText1.Padding = new System.Windows.Forms.Padding(5);
            this.sys3LedLabelWithText1.Size = new System.Drawing.Size(154, 46);
            this.sys3LedLabelWithText1.SubFont = new System.Drawing.Font("맑은 고딕", 8F, System.Drawing.FontStyle.Bold);
            this.sys3LedLabelWithText1.SubFontColor = System.Drawing.Color.Red;
            this.sys3LedLabelWithText1.SubText = "IONIZER";
            this.sys3LedLabelWithText1.TabIndex = 29;
            this.sys3LedLabelWithText1.Tag = "51";
            this.sys3LedLabelWithText1.Text = "FOUP_12 AREA\\nIONIZER";
            this.sys3LedLabelWithText1.TextAlignMain = Sys3Controls.EN_TEXTALIGN.MIDDLE_CENTER;
            this.sys3LedLabelWithText1.TextAlignSub = Sys3Controls.EN_TEXTALIGN.TOP_LEFT;
            this.sys3LedLabelWithText1.TextAlignUnit = Sys3Controls.EN_TEXTALIGN.BOTTOM_RIGHT;
            this.sys3LedLabelWithText1.TextMargin = 5;
            this.sys3LedLabelWithText1.ThemeIndex = 0;
            this.sys3LedLabelWithText1.UnitAreaRate = 10;
            this.sys3LedLabelWithText1.UnitFont = new System.Drawing.Font("맑은 고딕", 9F, System.Drawing.FontStyle.Bold);
            this.sys3LedLabelWithText1.UnitFontColor = System.Drawing.Color.Red;
            this.sys3LedLabelWithText1.UnitPositionVertical = false;
            this.sys3LedLabelWithText1.UnitText = "MPa";
            this.sys3LedLabelWithText1.UseActiveMessage = false;
            this.sys3LedLabelWithText1.UseBorder = true;
            this.sys3LedLabelWithText1.UseEdgeRadius = false;
            this.sys3LedLabelWithText1.UseImage = false;
            this.sys3LedLabelWithText1.UseSubFont = false;
            this.sys3LedLabelWithText1.UseUnitFont = false;
            // 
            // MainMonitoringSubPanel500WDigitals
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.Controls.Add(this.tableLayoutPanel1);
            this.Name = "MainMonitoringSubPanel500WDigitals";
            this.Size = new System.Drawing.Size(624, 96);
            this.tableLayoutPanel1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private Sys3Controls.Sys3LedLabelWithText lblIonizer3;
        private Sys3Controls.Sys3LedLabelWithText lblDoorOpened;
        private Sys3Controls.Sys3LedLabelWithText lblPowerBoxFan;
        private Sys3Controls.Sys3LedLabelWithText lblIOBoxFan;
        private Sys3Controls.Sys3LedLabelWithText lblFFUAlarm;
        private Sys3Controls.Sys3LedLabelWithText lblIonizer1;
        private Sys3Controls.Sys3LedLabelWithText sys3LedLabelWithText1;
    }
}
