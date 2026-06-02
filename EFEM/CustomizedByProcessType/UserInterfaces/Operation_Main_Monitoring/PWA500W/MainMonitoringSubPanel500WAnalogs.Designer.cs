
namespace EFEM.CustomizedByProcessType.UserInterface.OperationMainMonitoring.PWA500W
{
    partial class MainMonitoringSubPanel500WAnalogs
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
            this.lblIonizer1 = new Sys3Controls.Sys3LedLabelWithText();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.lblIonizer3 = new Sys3Controls.Sys3LedLabelWithText();
            this.lblIonizer2 = new Sys3Controls.Sys3LedLabelWithText();
            this.lblMainAir = new Sys3Controls.Sys3LedLabelWithText();
            this.lblMainVacuum = new Sys3Controls.Sys3LedLabelWithText();
            this.lblRobotCDA = new Sys3Controls.Sys3LedLabelWithText();
            this.lblIonizerPressure = new Sys3Controls.Sys3LedLabelWithText();
            this.tableLayoutPanel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // lblIonizer1
            // 
            this.lblIonizer1.Active = false;
            this.lblIonizer1.ActiveMessage = "";
            this.lblIonizer1.BackGroundColor = System.Drawing.Color.Bisque;
            this.lblIonizer1.BorderStroke = 2;
            this.lblIonizer1.BorderStyle = System.Windows.Forms.ButtonBorderStyle.Solid;
            this.lblIonizer1.ColorActive = System.Drawing.Color.LimeGreen;
            this.lblIonizer1.ColorNormal = System.Drawing.Color.Red;
            this.lblIonizer1.Description = "";
            this.lblIonizer1.DisabledColor = System.Drawing.Color.DarkGray;
            this.lblIonizer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblIonizer1.EdgeRadius = 1;
            this.lblIonizer1.ImagePosition = new System.Drawing.Point(0, 0);
            this.lblIonizer1.ImageSize = new System.Drawing.Point(0, 0);
            this.lblIonizer1.LedWidth = 15;
            this.lblIonizer1.LoadImage = null;
            this.lblIonizer1.Location = new System.Drawing.Point(314, 50);
            this.lblIonizer1.MainFont = new System.Drawing.Font("맑은 고딕", 12F, System.Drawing.FontStyle.Bold);
            this.lblIonizer1.MainFontColor = System.Drawing.Color.Black;
            this.lblIonizer1.Margin = new System.Windows.Forms.Padding(2);
            this.lblIonizer1.Name = "lblIonizer1";
            this.lblIonizer1.Padding = new System.Windows.Forms.Padding(5);
            this.lblIonizer1.Size = new System.Drawing.Size(152, 44);
            this.lblIonizer1.SubFont = new System.Drawing.Font("맑은 고딕", 7F, System.Drawing.FontStyle.Bold);
            this.lblIonizer1.SubFontColor = System.Drawing.Color.Red;
            this.lblIonizer1.SubText = "IONIZER FLOW #1";
            this.lblIonizer1.TabIndex = 10;
            this.lblIonizer1.Tag = "4";
            this.lblIonizer1.Text = "10.0000";
            this.lblIonizer1.TextAlignMain = Sys3Controls.EN_TEXTALIGN.BOTTOM_RIGHT;
            this.lblIonizer1.TextAlignSub = Sys3Controls.EN_TEXTALIGN.TOP_LEFT;
            this.lblIonizer1.TextAlignUnit = Sys3Controls.EN_TEXTALIGN.BOTTOM_RIGHT;
            this.lblIonizer1.TextMargin = 5;
            this.lblIonizer1.ThemeIndex = 0;
            this.lblIonizer1.UnitAreaRate = 30;
            this.lblIonizer1.UnitFont = new System.Drawing.Font("맑은 고딕", 9F, System.Drawing.FontStyle.Bold);
            this.lblIonizer1.UnitFontColor = System.Drawing.Color.Red;
            this.lblIonizer1.UnitPositionVertical = false;
            this.lblIonizer1.UnitText = "L/Min";
            this.lblIonizer1.UseActiveMessage = false;
            this.lblIonizer1.UseBorder = true;
            this.lblIonizer1.UseEdgeRadius = false;
            this.lblIonizer1.UseImage = false;
            this.lblIonizer1.UseSubFont = true;
            this.lblIonizer1.UseUnitFont = true;
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 4;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.tableLayoutPanel1.Controls.Add(this.lblIonizer3, 0, 1);
            this.tableLayoutPanel1.Controls.Add(this.lblIonizer2, 0, 1);
            this.tableLayoutPanel1.Controls.Add(this.lblIonizer1, 0, 1);
            this.tableLayoutPanel1.Controls.Add(this.lblMainAir, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.lblMainVacuum, 1, 0);
            this.tableLayoutPanel1.Controls.Add(this.lblRobotCDA, 2, 0);
            this.tableLayoutPanel1.Controls.Add(this.lblIonizerPressure, 3, 0);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel1.Margin = new System.Windows.Forms.Padding(0);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 2;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(627, 96);
            this.tableLayoutPanel1.TabIndex = 4;
            // 
            // lblIonizer3
            // 
            this.lblIonizer3.Active = false;
            this.lblIonizer3.ActiveMessage = "";
            this.lblIonizer3.BackGroundColor = System.Drawing.Color.Bisque;
            this.lblIonizer3.BorderStroke = 2;
            this.lblIonizer3.BorderStyle = System.Windows.Forms.ButtonBorderStyle.Solid;
            this.lblIonizer3.ColorActive = System.Drawing.Color.LimeGreen;
            this.lblIonizer3.ColorNormal = System.Drawing.Color.Red;
            this.lblIonizer3.Description = "";
            this.lblIonizer3.DisabledColor = System.Drawing.Color.DarkGray;
            this.lblIonizer3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblIonizer3.EdgeRadius = 1;
            this.lblIonizer3.ImagePosition = new System.Drawing.Point(0, 0);
            this.lblIonizer3.ImageSize = new System.Drawing.Point(0, 0);
            this.lblIonizer3.LedWidth = 15;
            this.lblIonizer3.LoadImage = null;
            this.lblIonizer3.Location = new System.Drawing.Point(2, 50);
            this.lblIonizer3.MainFont = new System.Drawing.Font("맑은 고딕", 12F, System.Drawing.FontStyle.Bold);
            this.lblIonizer3.MainFontColor = System.Drawing.Color.Black;
            this.lblIonizer3.Margin = new System.Windows.Forms.Padding(2);
            this.lblIonizer3.Name = "lblIonizer3";
            this.lblIonizer3.Padding = new System.Windows.Forms.Padding(5);
            this.lblIonizer3.Size = new System.Drawing.Size(152, 44);
            this.lblIonizer3.SubFont = new System.Drawing.Font("맑은 고딕", 7F, System.Drawing.FontStyle.Bold);
            this.lblIonizer3.SubFontColor = System.Drawing.Color.Red;
            this.lblIonizer3.SubText = "IONIZER FLOW #3";
            this.lblIonizer3.TabIndex = 13;
            this.lblIonizer3.Tag = "6";
            this.lblIonizer3.Text = "10.0000";
            this.lblIonizer3.TextAlignMain = Sys3Controls.EN_TEXTALIGN.BOTTOM_RIGHT;
            this.lblIonizer3.TextAlignSub = Sys3Controls.EN_TEXTALIGN.TOP_LEFT;
            this.lblIonizer3.TextAlignUnit = Sys3Controls.EN_TEXTALIGN.BOTTOM_RIGHT;
            this.lblIonizer3.TextMargin = 5;
            this.lblIonizer3.ThemeIndex = 0;
            this.lblIonizer3.UnitAreaRate = 30;
            this.lblIonizer3.UnitFont = new System.Drawing.Font("맑은 고딕", 9F, System.Drawing.FontStyle.Bold);
            this.lblIonizer3.UnitFontColor = System.Drawing.Color.Red;
            this.lblIonizer3.UnitPositionVertical = false;
            this.lblIonizer3.UnitText = "L/Min";
            this.lblIonizer3.UseActiveMessage = false;
            this.lblIonizer3.UseBorder = true;
            this.lblIonizer3.UseEdgeRadius = false;
            this.lblIonizer3.UseImage = false;
            this.lblIonizer3.UseSubFont = true;
            this.lblIonizer3.UseUnitFont = true;
            // 
            // lblIonizer2
            // 
            this.lblIonizer2.Active = false;
            this.lblIonizer2.ActiveMessage = "";
            this.lblIonizer2.BackGroundColor = System.Drawing.Color.Bisque;
            this.lblIonizer2.BorderStroke = 2;
            this.lblIonizer2.BorderStyle = System.Windows.Forms.ButtonBorderStyle.Solid;
            this.lblIonizer2.ColorActive = System.Drawing.Color.LimeGreen;
            this.lblIonizer2.ColorNormal = System.Drawing.Color.Red;
            this.lblIonizer2.Description = "";
            this.lblIonizer2.DisabledColor = System.Drawing.Color.DarkGray;
            this.lblIonizer2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblIonizer2.EdgeRadius = 1;
            this.lblIonizer2.ImagePosition = new System.Drawing.Point(0, 0);
            this.lblIonizer2.ImageSize = new System.Drawing.Point(0, 0);
            this.lblIonizer2.LedWidth = 15;
            this.lblIonizer2.LoadImage = null;
            this.lblIonizer2.Location = new System.Drawing.Point(158, 50);
            this.lblIonizer2.MainFont = new System.Drawing.Font("맑은 고딕", 12F, System.Drawing.FontStyle.Bold);
            this.lblIonizer2.MainFontColor = System.Drawing.Color.Black;
            this.lblIonizer2.Margin = new System.Windows.Forms.Padding(2);
            this.lblIonizer2.Name = "lblIonizer2";
            this.lblIonizer2.Padding = new System.Windows.Forms.Padding(5);
            this.lblIonizer2.Size = new System.Drawing.Size(152, 44);
            this.lblIonizer2.SubFont = new System.Drawing.Font("맑은 고딕", 7F, System.Drawing.FontStyle.Bold);
            this.lblIonizer2.SubFontColor = System.Drawing.Color.Red;
            this.lblIonizer2.SubText = "IONIZER FLOW #2";
            this.lblIonizer2.TabIndex = 11;
            this.lblIonizer2.Tag = "5";
            this.lblIonizer2.Text = "10.0000";
            this.lblIonizer2.TextAlignMain = Sys3Controls.EN_TEXTALIGN.BOTTOM_RIGHT;
            this.lblIonizer2.TextAlignSub = Sys3Controls.EN_TEXTALIGN.TOP_LEFT;
            this.lblIonizer2.TextAlignUnit = Sys3Controls.EN_TEXTALIGN.BOTTOM_RIGHT;
            this.lblIonizer2.TextMargin = 5;
            this.lblIonizer2.ThemeIndex = 0;
            this.lblIonizer2.UnitAreaRate = 30;
            this.lblIonizer2.UnitFont = new System.Drawing.Font("맑은 고딕", 9F, System.Drawing.FontStyle.Bold);
            this.lblIonizer2.UnitFontColor = System.Drawing.Color.Red;
            this.lblIonizer2.UnitPositionVertical = false;
            this.lblIonizer2.UnitText = "L/Min";
            this.lblIonizer2.UseActiveMessage = false;
            this.lblIonizer2.UseBorder = true;
            this.lblIonizer2.UseEdgeRadius = false;
            this.lblIonizer2.UseImage = false;
            this.lblIonizer2.UseSubFont = true;
            this.lblIonizer2.UseUnitFont = true;
            // 
            // lblMainAir
            // 
            this.lblMainAir.Active = false;
            this.lblMainAir.ActiveMessage = "";
            this.lblMainAir.BackGroundColor = System.Drawing.Color.Bisque;
            this.lblMainAir.BorderStroke = 2;
            this.lblMainAir.BorderStyle = System.Windows.Forms.ButtonBorderStyle.Solid;
            this.lblMainAir.ColorActive = System.Drawing.Color.LimeGreen;
            this.lblMainAir.ColorNormal = System.Drawing.Color.Red;
            this.lblMainAir.Description = "";
            this.lblMainAir.DisabledColor = System.Drawing.Color.DarkGray;
            this.lblMainAir.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblMainAir.EdgeRadius = 1;
            this.lblMainAir.ImagePosition = new System.Drawing.Point(0, 0);
            this.lblMainAir.ImageSize = new System.Drawing.Point(0, 0);
            this.lblMainAir.LedWidth = 15;
            this.lblMainAir.LoadImage = null;
            this.lblMainAir.Location = new System.Drawing.Point(2, 2);
            this.lblMainAir.MainFont = new System.Drawing.Font("맑은 고딕", 12F, System.Drawing.FontStyle.Bold);
            this.lblMainAir.MainFontColor = System.Drawing.Color.Black;
            this.lblMainAir.Margin = new System.Windows.Forms.Padding(2);
            this.lblMainAir.Name = "lblMainAir";
            this.lblMainAir.Padding = new System.Windows.Forms.Padding(5);
            this.lblMainAir.Size = new System.Drawing.Size(152, 44);
            this.lblMainAir.SubFont = new System.Drawing.Font("맑은 고딕", 8F, System.Drawing.FontStyle.Bold);
            this.lblMainAir.SubFontColor = System.Drawing.Color.Red;
            this.lblMainAir.SubText = "MAIN CDA";
            this.lblMainAir.TabIndex = 8;
            this.lblMainAir.Tag = "0";
            this.lblMainAir.Text = "10.0000";
            this.lblMainAir.TextAlignMain = Sys3Controls.EN_TEXTALIGN.BOTTOM_RIGHT;
            this.lblMainAir.TextAlignSub = Sys3Controls.EN_TEXTALIGN.TOP_LEFT;
            this.lblMainAir.TextAlignUnit = Sys3Controls.EN_TEXTALIGN.BOTTOM_RIGHT;
            this.lblMainAir.TextMargin = 5;
            this.lblMainAir.ThemeIndex = 0;
            this.lblMainAir.UnitAreaRate = 30;
            this.lblMainAir.UnitFont = new System.Drawing.Font("맑은 고딕", 9F, System.Drawing.FontStyle.Bold);
            this.lblMainAir.UnitFontColor = System.Drawing.Color.Red;
            this.lblMainAir.UnitPositionVertical = false;
            this.lblMainAir.UnitText = "MPa";
            this.lblMainAir.UseActiveMessage = false;
            this.lblMainAir.UseBorder = true;
            this.lblMainAir.UseEdgeRadius = false;
            this.lblMainAir.UseImage = false;
            this.lblMainAir.UseSubFont = true;
            this.lblMainAir.UseUnitFont = true;
            // 
            // lblMainVacuum
            // 
            this.lblMainVacuum.Active = false;
            this.lblMainVacuum.ActiveMessage = "";
            this.lblMainVacuum.BackGroundColor = System.Drawing.Color.Bisque;
            this.lblMainVacuum.BorderStroke = 2;
            this.lblMainVacuum.BorderStyle = System.Windows.Forms.ButtonBorderStyle.Solid;
            this.lblMainVacuum.ColorActive = System.Drawing.Color.LimeGreen;
            this.lblMainVacuum.ColorNormal = System.Drawing.Color.Red;
            this.lblMainVacuum.Description = "";
            this.lblMainVacuum.DisabledColor = System.Drawing.Color.DarkGray;
            this.lblMainVacuum.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblMainVacuum.EdgeRadius = 1;
            this.lblMainVacuum.ImagePosition = new System.Drawing.Point(0, 0);
            this.lblMainVacuum.ImageSize = new System.Drawing.Point(0, 0);
            this.lblMainVacuum.LedWidth = 15;
            this.lblMainVacuum.LoadImage = null;
            this.lblMainVacuum.Location = new System.Drawing.Point(158, 2);
            this.lblMainVacuum.MainFont = new System.Drawing.Font("맑은 고딕", 12F, System.Drawing.FontStyle.Bold);
            this.lblMainVacuum.MainFontColor = System.Drawing.Color.Black;
            this.lblMainVacuum.Margin = new System.Windows.Forms.Padding(2);
            this.lblMainVacuum.Name = "lblMainVacuum";
            this.lblMainVacuum.Padding = new System.Windows.Forms.Padding(5);
            this.lblMainVacuum.Size = new System.Drawing.Size(152, 44);
            this.lblMainVacuum.SubFont = new System.Drawing.Font("맑은 고딕", 8F, System.Drawing.FontStyle.Bold);
            this.lblMainVacuum.SubFontColor = System.Drawing.Color.Red;
            this.lblMainVacuum.SubText = "VACUUM";
            this.lblMainVacuum.TabIndex = 12;
            this.lblMainVacuum.Tag = "1";
            this.lblMainVacuum.Text = "10.0000";
            this.lblMainVacuum.TextAlignMain = Sys3Controls.EN_TEXTALIGN.BOTTOM_RIGHT;
            this.lblMainVacuum.TextAlignSub = Sys3Controls.EN_TEXTALIGN.TOP_LEFT;
            this.lblMainVacuum.TextAlignUnit = Sys3Controls.EN_TEXTALIGN.BOTTOM_RIGHT;
            this.lblMainVacuum.TextMargin = 5;
            this.lblMainVacuum.ThemeIndex = 0;
            this.lblMainVacuum.UnitAreaRate = 30;
            this.lblMainVacuum.UnitFont = new System.Drawing.Font("맑은 고딕", 9F, System.Drawing.FontStyle.Bold);
            this.lblMainVacuum.UnitFontColor = System.Drawing.Color.Red;
            this.lblMainVacuum.UnitPositionVertical = false;
            this.lblMainVacuum.UnitText = "kPa";
            this.lblMainVacuum.UseActiveMessage = false;
            this.lblMainVacuum.UseBorder = true;
            this.lblMainVacuum.UseEdgeRadius = false;
            this.lblMainVacuum.UseImage = false;
            this.lblMainVacuum.UseSubFont = true;
            this.lblMainVacuum.UseUnitFont = true;
            // 
            // lblRobotCDA
            // 
            this.lblRobotCDA.Active = false;
            this.lblRobotCDA.ActiveMessage = "";
            this.lblRobotCDA.BackGroundColor = System.Drawing.Color.Bisque;
            this.lblRobotCDA.BorderStroke = 2;
            this.lblRobotCDA.BorderStyle = System.Windows.Forms.ButtonBorderStyle.Solid;
            this.lblRobotCDA.ColorActive = System.Drawing.Color.LimeGreen;
            this.lblRobotCDA.ColorNormal = System.Drawing.Color.Red;
            this.lblRobotCDA.Description = "";
            this.lblRobotCDA.DisabledColor = System.Drawing.Color.DarkGray;
            this.lblRobotCDA.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblRobotCDA.EdgeRadius = 1;
            this.lblRobotCDA.ImagePosition = new System.Drawing.Point(0, 0);
            this.lblRobotCDA.ImageSize = new System.Drawing.Point(0, 0);
            this.lblRobotCDA.LedWidth = 15;
            this.lblRobotCDA.LoadImage = null;
            this.lblRobotCDA.Location = new System.Drawing.Point(314, 2);
            this.lblRobotCDA.MainFont = new System.Drawing.Font("맑은 고딕", 12F, System.Drawing.FontStyle.Bold);
            this.lblRobotCDA.MainFontColor = System.Drawing.Color.Black;
            this.lblRobotCDA.Margin = new System.Windows.Forms.Padding(2);
            this.lblRobotCDA.Name = "lblRobotCDA";
            this.lblRobotCDA.Padding = new System.Windows.Forms.Padding(5);
            this.lblRobotCDA.Size = new System.Drawing.Size(152, 44);
            this.lblRobotCDA.SubFont = new System.Drawing.Font("맑은 고딕", 8F, System.Drawing.FontStyle.Bold);
            this.lblRobotCDA.SubFontColor = System.Drawing.Color.Red;
            this.lblRobotCDA.SubText = "ROBOT CDA";
            this.lblRobotCDA.TabIndex = 15;
            this.lblRobotCDA.Tag = "2";
            this.lblRobotCDA.Text = "10.0000";
            this.lblRobotCDA.TextAlignMain = Sys3Controls.EN_TEXTALIGN.BOTTOM_RIGHT;
            this.lblRobotCDA.TextAlignSub = Sys3Controls.EN_TEXTALIGN.TOP_LEFT;
            this.lblRobotCDA.TextAlignUnit = Sys3Controls.EN_TEXTALIGN.BOTTOM_RIGHT;
            this.lblRobotCDA.TextMargin = 5;
            this.lblRobotCDA.ThemeIndex = 0;
            this.lblRobotCDA.UnitAreaRate = 30;
            this.lblRobotCDA.UnitFont = new System.Drawing.Font("맑은 고딕", 9F, System.Drawing.FontStyle.Bold);
            this.lblRobotCDA.UnitFontColor = System.Drawing.Color.Red;
            this.lblRobotCDA.UnitPositionVertical = false;
            this.lblRobotCDA.UnitText = "MPa";
            this.lblRobotCDA.UseActiveMessage = false;
            this.lblRobotCDA.UseBorder = true;
            this.lblRobotCDA.UseEdgeRadius = false;
            this.lblRobotCDA.UseImage = false;
            this.lblRobotCDA.UseSubFont = true;
            this.lblRobotCDA.UseUnitFont = true;
            // 
            // lblIonizerPressure
            // 
            this.lblIonizerPressure.Active = false;
            this.lblIonizerPressure.ActiveMessage = "";
            this.lblIonizerPressure.BackGroundColor = System.Drawing.Color.Bisque;
            this.lblIonizerPressure.BorderStroke = 2;
            this.lblIonizerPressure.BorderStyle = System.Windows.Forms.ButtonBorderStyle.Solid;
            this.lblIonizerPressure.ColorActive = System.Drawing.Color.LimeGreen;
            this.lblIonizerPressure.ColorNormal = System.Drawing.Color.Red;
            this.lblIonizerPressure.Description = "";
            this.lblIonizerPressure.DisabledColor = System.Drawing.Color.DarkGray;
            this.lblIonizerPressure.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblIonizerPressure.EdgeRadius = 1;
            this.lblIonizerPressure.ImagePosition = new System.Drawing.Point(0, 0);
            this.lblIonizerPressure.ImageSize = new System.Drawing.Point(0, 0);
            this.lblIonizerPressure.LedWidth = 15;
            this.lblIonizerPressure.LoadImage = null;
            this.lblIonizerPressure.Location = new System.Drawing.Point(470, 2);
            this.lblIonizerPressure.MainFont = new System.Drawing.Font("맑은 고딕", 12F, System.Drawing.FontStyle.Bold);
            this.lblIonizerPressure.MainFontColor = System.Drawing.Color.Black;
            this.lblIonizerPressure.Margin = new System.Windows.Forms.Padding(2);
            this.lblIonizerPressure.Name = "lblIonizerPressure";
            this.lblIonizerPressure.Padding = new System.Windows.Forms.Padding(5);
            this.lblIonizerPressure.Size = new System.Drawing.Size(155, 44);
            this.lblIonizerPressure.SubFont = new System.Drawing.Font("맑은 고딕", 8F, System.Drawing.FontStyle.Bold);
            this.lblIonizerPressure.SubFontColor = System.Drawing.Color.Red;
            this.lblIonizerPressure.SubText = "IONIZER";
            this.lblIonizerPressure.TabIndex = 14;
            this.lblIonizerPressure.Tag = "3";
            this.lblIonizerPressure.Text = "10.0000";
            this.lblIonizerPressure.TextAlignMain = Sys3Controls.EN_TEXTALIGN.BOTTOM_RIGHT;
            this.lblIonizerPressure.TextAlignSub = Sys3Controls.EN_TEXTALIGN.TOP_LEFT;
            this.lblIonizerPressure.TextAlignUnit = Sys3Controls.EN_TEXTALIGN.BOTTOM_RIGHT;
            this.lblIonizerPressure.TextMargin = 5;
            this.lblIonizerPressure.ThemeIndex = 0;
            this.lblIonizerPressure.UnitAreaRate = 30;
            this.lblIonizerPressure.UnitFont = new System.Drawing.Font("맑은 고딕", 9F, System.Drawing.FontStyle.Bold);
            this.lblIonizerPressure.UnitFontColor = System.Drawing.Color.Red;
            this.lblIonizerPressure.UnitPositionVertical = false;
            this.lblIonizerPressure.UnitText = "MPa";
            this.lblIonizerPressure.UseActiveMessage = false;
            this.lblIonizerPressure.UseBorder = true;
            this.lblIonizerPressure.UseEdgeRadius = false;
            this.lblIonizerPressure.UseImage = false;
            this.lblIonizerPressure.UseSubFont = true;
            this.lblIonizerPressure.UseUnitFont = true;
            // 
            // MainMonitoringSubPanel500WAnalogs
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.Controls.Add(this.tableLayoutPanel1);
            this.Name = "MainMonitoringSubPanel500WAnalogs";
            this.Size = new System.Drawing.Size(627, 96);
            this.tableLayoutPanel1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private Sys3Controls.Sys3LedLabelWithText lblIonizer1;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private Sys3Controls.Sys3LedLabelWithText lblIonizer3;
        private Sys3Controls.Sys3LedLabelWithText lblIonizer2;
        private Sys3Controls.Sys3LedLabelWithText lblMainAir;
        private Sys3Controls.Sys3LedLabelWithText lblMainVacuum;
        private Sys3Controls.Sys3LedLabelWithText lblRobotCDA;
        private Sys3Controls.Sys3LedLabelWithText lblIonizerPressure;
    }
}
