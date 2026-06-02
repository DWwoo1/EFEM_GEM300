
namespace EFEM.CustomizedByProcessType.UserInterface.Setup.PWA500BIN
{
    partial class SetupPWA500BINLoadPortOptions
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
            this.tableLayoutPanel2 = new System.Windows.Forms.TableLayoutPanel();
            this.sys3Label5 = new Sys3Controls.Sys3Label();
            this.toggleUseLoadPort = new FrameOfSystem3.Component.CustomParameterToggleButton();
            this.lblWaferType = new FrameOfSystem3.Component.CustomParameterLabel();
            this.sys3Label1 = new Sys3Controls.Sys3Label();
            this.gbTitle = new Sys3Controls.Sys3GroupBoxContainer();
            this.tableLayoutPanel1.SuspendLayout();
            this.tableLayoutPanel2.SuspendLayout();
            this.gbTitle.SuspendLayout();
            this.SuspendLayout();
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 1;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.Controls.Add(this.tableLayoutPanel2, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.lblWaferType, 0, 2);
            this.tableLayoutPanel1.Controls.Add(this.sys3Label1, 0, 1);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(5, 32);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 3;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 15F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 15F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 70F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(160, 238);
            this.tableLayoutPanel1.TabIndex = 20669;
            // 
            // tableLayoutPanel2
            // 
            this.tableLayoutPanel2.ColumnCount = 2;
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel2.Controls.Add(this.sys3Label5, 0, 0);
            this.tableLayoutPanel2.Controls.Add(this.toggleUseLoadPort, 1, 0);
            this.tableLayoutPanel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel2.Location = new System.Drawing.Point(1, 1);
            this.tableLayoutPanel2.Margin = new System.Windows.Forms.Padding(1);
            this.tableLayoutPanel2.Name = "tableLayoutPanel2";
            this.tableLayoutPanel2.RowCount = 1;
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 33F));
            this.tableLayoutPanel2.Size = new System.Drawing.Size(158, 33);
            this.tableLayoutPanel2.TabIndex = 20670;
            // 
            // sys3Label5
            // 
            this.sys3Label5.BackGroundColor = System.Drawing.Color.DarkGray;
            this.sys3Label5.BorderStroke = 2;
            this.sys3Label5.BorderStyle = System.Windows.Forms.ButtonBorderStyle.Solid;
            this.sys3Label5.Description = "";
            this.sys3Label5.DisabledColor = System.Drawing.Color.DarkGray;
            this.sys3Label5.Dock = System.Windows.Forms.DockStyle.Fill;
            this.sys3Label5.EdgeRadius = 1;
            this.sys3Label5.ImagePosition = new System.Drawing.Point(0, 0);
            this.sys3Label5.ImageSize = new System.Drawing.Point(0, 0);
            this.sys3Label5.LoadImage = null;
            this.sys3Label5.Location = new System.Drawing.Point(0, 0);
            this.sys3Label5.MainFont = new System.Drawing.Font("맑은 고딕", 11F, System.Drawing.FontStyle.Bold);
            this.sys3Label5.MainFontColor = System.Drawing.Color.Black;
            this.sys3Label5.Margin = new System.Windows.Forms.Padding(0);
            this.sys3Label5.Name = "sys3Label5";
            this.sys3Label5.Size = new System.Drawing.Size(79, 33);
            this.sys3Label5.SubFont = new System.Drawing.Font("맑은 고딕", 10F, System.Drawing.FontStyle.Bold);
            this.sys3Label5.SubFontColor = System.Drawing.Color.Gray;
            this.sys3Label5.SubText = "";
            this.sys3Label5.TabIndex = 20637;
            this.sys3Label5.Text = "USE";
            this.sys3Label5.TextAlignMain = Sys3Controls.EN_TEXTALIGN.MIDDLE_CENTER;
            this.sys3Label5.TextAlignSub = Sys3Controls.EN_TEXTALIGN.TOP_LEFT;
            this.sys3Label5.TextAlignUnit = Sys3Controls.EN_TEXTALIGN.MIDDLE_CENTER;
            this.sys3Label5.ThemeIndex = 0;
            this.sys3Label5.UnitAreaRate = 30;
            this.sys3Label5.UnitFont = new System.Drawing.Font("맑은 고딕", 10F, System.Drawing.FontStyle.Bold);
            this.sys3Label5.UnitFontColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.sys3Label5.UnitPositionVertical = false;
            this.sys3Label5.UnitText = "";
            this.sys3Label5.UseBorder = true;
            this.sys3Label5.UseEdgeRadius = false;
            this.sys3Label5.UseImage = false;
            this.sys3Label5.UseSubFont = true;
            this.sys3Label5.UseUnitFont = false;
            // 
            // toggleUseLoadPort
            // 
            this.toggleUseLoadPort.Active = false;
            this.toggleUseLoadPort.ActiveColorFirst = System.Drawing.Color.RoyalBlue;
            this.toggleUseLoadPort.ActiveColorSecond = System.Drawing.Color.DodgerBlue;
            this.toggleUseLoadPort.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.toggleUseLoadPort.AssociatedMap = null;
            this.toggleUseLoadPort.Location = new System.Drawing.Point(89, 2);
            this.toggleUseLoadPort.Margin = new System.Windows.Forms.Padding(1, 1, 0, 0);
            this.toggleUseLoadPort.Name = "toggleUseLoadPort";
            this.toggleUseLoadPort.NeedRemakeMap = false;
            this.toggleUseLoadPort.NormalColorFirst = System.Drawing.Color.DarkGray;
            this.toggleUseLoadPort.NormalColorSecond = System.Drawing.Color.Silver;
            this.toggleUseLoadPort.ParameterChangeDefaultActiveColorFirst = System.Drawing.Color.RoyalBlue;
            this.toggleUseLoadPort.ParameterChangeDefaultActiveColorSecond = System.Drawing.Color.DodgerBlue;
            this.toggleUseLoadPort.ParameterChangeDefaultNormalColorFirst = System.Drawing.Color.DarkGray;
            this.toggleUseLoadPort.ParameterChangeDefaultNormalColorSecond = System.Drawing.Color.Silver;
            this.toggleUseLoadPort.ParameterChangeWaitColorFirst = System.Drawing.Color.DarkRed;
            this.toggleUseLoadPort.ParameterChangeWaitColorSecond = System.Drawing.Color.Firebrick;
            this.toggleUseLoadPort.ParameterIndex = 0;
            this.toggleUseLoadPort.ParameterName = "UseLoadPort";
            this.toggleUseLoadPort.ParameterStorage = "False";
            this.toggleUseLoadPort.ParameterStored = false;
            this.toggleUseLoadPort.ParameterType = FrameOfSystem3.Recipe.EN_RECIPE_TYPE.EQUIPMENT;
            this.toggleUseLoadPort.Size = new System.Drawing.Size(60, 30);
            this.toggleUseLoadPort.TabIndex = 20666;
            this.toggleUseLoadPort.TaskName = "Sample";
            this.toggleUseLoadPort.UseParameterChangeConfirm = true;
            // 
            // lblWaferType
            // 
            this.lblWaferType.AssociatedMap = null;
            this.lblWaferType.BackGroundColor = System.Drawing.Color.LightSkyBlue;
            this.lblWaferType.BorderStroke = 2;
            this.lblWaferType.BorderStyle = System.Windows.Forms.ButtonBorderStyle.Solid;
            this.lblWaferType.Description = "";
            this.lblWaferType.DisabledColor = System.Drawing.Color.DarkGray;
            this.lblWaferType.Dock = System.Windows.Forms.DockStyle.Top;
            this.lblWaferType.EdgeRadius = 1;
            this.lblWaferType.ImagePosition = new System.Drawing.Point(0, 0);
            this.lblWaferType.ImageSize = new System.Drawing.Point(0, 0);
            this.lblWaferType.LoadImage = null;
            this.lblWaferType.Location = new System.Drawing.Point(1, 71);
            this.lblWaferType.MainFont = new System.Drawing.Font("맑은 고딕", 11F, System.Drawing.FontStyle.Bold);
            this.lblWaferType.MainFontColor = System.Drawing.Color.Black;
            this.lblWaferType.Margin = new System.Windows.Forms.Padding(1, 1, 0, 0);
            this.lblWaferType.Modifiable = true;
            this.lblWaferType.Name = "lblWaferType";
            this.lblWaferType.NeedRemakeMap = false;
            this.lblWaferType.ParameterChangeDefaultColor = System.Drawing.Color.Black;
            this.lblWaferType.ParameterChangeWaitColor = System.Drawing.Color.Red;
            this.lblWaferType.ParameterDefaultValue = "";
            this.lblWaferType.ParameterIndex = 0;
            this.lblWaferType.ParameterMAX = "";
            this.lblWaferType.ParameterMIN = "";
            this.lblWaferType.ParameterName = "LoadPortType";
            this.lblWaferType.ParameterSettingType = FrameOfSystem3.Component.EN_LABEL_PARAMETER_TYPE.SELECT;
            this.lblWaferType.ParameterStorage = "";
            this.lblWaferType.ParameterStored = false;
            this.lblWaferType.ParameterType = FrameOfSystem3.Recipe.EN_RECIPE_TYPE.EQUIPMENT;
            this.lblWaferType.ParameterUNIT = "";
            this.lblWaferType.SelectionList = Define.DefineEnumProject.SelectionList.EN_SELECTIONLIST.WAFER_TYPE_BIN;
            this.lblWaferType.Size = new System.Drawing.Size(159, 35);
            this.lblWaferType.SubFont = new System.Drawing.Font("맑은 고딕", 8F, System.Drawing.FontStyle.Bold);
            this.lblWaferType.SubFontColor = System.Drawing.Color.Gray;
            this.lblWaferType.SubText = "";
            this.lblWaferType.TabIndex = 20668;
            this.lblWaferType.Tag = "";
            this.lblWaferType.TaskName = "WaferType";
            this.lblWaferType.Text = "--";
            this.lblWaferType.TextAlignMain = Sys3Controls.EN_TEXTALIGN.MIDDLE_CENTER;
            this.lblWaferType.TextAlignSub = Sys3Controls.EN_TEXTALIGN.TOP_LEFT;
            this.lblWaferType.TextAlignUnit = Sys3Controls.EN_TEXTALIGN.MIDDLE_RIGHT;
            this.lblWaferType.ThemeIndex = 0;
            this.lblWaferType.UnitAreaRate = 40;
            this.lblWaferType.UnitFont = new System.Drawing.Font("맑은 고딕", 10F, System.Drawing.FontStyle.Bold);
            this.lblWaferType.UnitFontColor = System.Drawing.Color.Red;
            this.lblWaferType.UnitPositionVertical = false;
            this.lblWaferType.UnitText = "--";
            this.lblWaferType.UseBorder = true;
            this.lblWaferType.UseEdgeRadius = false;
            this.lblWaferType.UseImage = false;
            this.lblWaferType.UseParameterChangeConfirm = true;
            this.lblWaferType.UseSubFont = true;
            this.lblWaferType.UseUnitFont = false;
            // 
            // sys3Label1
            // 
            this.sys3Label1.BackGroundColor = System.Drawing.Color.DarkGray;
            this.sys3Label1.BorderStroke = 2;
            this.sys3Label1.BorderStyle = System.Windows.Forms.ButtonBorderStyle.Solid;
            this.sys3Label1.Description = "";
            this.sys3Label1.DisabledColor = System.Drawing.Color.DarkGray;
            this.sys3Label1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.sys3Label1.EdgeRadius = 1;
            this.sys3Label1.ImagePosition = new System.Drawing.Point(0, 0);
            this.sys3Label1.ImageSize = new System.Drawing.Point(0, 0);
            this.sys3Label1.LoadImage = null;
            this.sys3Label1.Location = new System.Drawing.Point(1, 36);
            this.sys3Label1.MainFont = new System.Drawing.Font("맑은 고딕", 11F, System.Drawing.FontStyle.Bold);
            this.sys3Label1.MainFontColor = System.Drawing.Color.Black;
            this.sys3Label1.Margin = new System.Windows.Forms.Padding(1, 1, 0, 0);
            this.sys3Label1.Name = "sys3Label1";
            this.sys3Label1.Size = new System.Drawing.Size(159, 34);
            this.sys3Label1.SubFont = new System.Drawing.Font("맑은 고딕", 10F, System.Drawing.FontStyle.Bold);
            this.sys3Label1.SubFontColor = System.Drawing.Color.Gray;
            this.sys3Label1.SubText = "";
            this.sys3Label1.TabIndex = 20639;
            this.sys3Label1.Text = "WAFER TYPE";
            this.sys3Label1.TextAlignMain = Sys3Controls.EN_TEXTALIGN.MIDDLE_CENTER;
            this.sys3Label1.TextAlignSub = Sys3Controls.EN_TEXTALIGN.TOP_LEFT;
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
            this.sys3Label1.UseSubFont = true;
            this.sys3Label1.UseUnitFont = false;
            // 
            // gbTitle
            // 
            this.gbTitle.BackGroundColor = System.Drawing.Color.WhiteSmoke;
            this.gbTitle.Controls.Add(this.tableLayoutPanel1);
            this.gbTitle.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gbTitle.EdgeBorderStroke = 2;
            this.gbTitle.EdgeRadius = 2;
            this.gbTitle.LabelFont = new System.Drawing.Font("맑은 고딕", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.gbTitle.LabelGradientColorFirst = System.Drawing.Color.DarkGray;
            this.gbTitle.LabelGradientColorSecond = System.Drawing.Color.WhiteSmoke;
            this.gbTitle.LabelHeight = 30;
            this.gbTitle.LabelTextColor = System.Drawing.Color.Black;
            this.gbTitle.Location = new System.Drawing.Point(0, 0);
            this.gbTitle.Margin = new System.Windows.Forms.Padding(0);
            this.gbTitle.Name = "gbTitle";
            this.gbTitle.Padding = new System.Windows.Forms.Padding(5, 18, 5, 5);
            this.gbTitle.Size = new System.Drawing.Size(170, 275);
            this.gbTitle.TabIndex = 21134;
            this.gbTitle.TabStop = false;
            this.gbTitle.TextAlign = Sys3Controls.EN_TEXTALIGN.MIDDLE_CENTER;
            this.gbTitle.ThemeIndex = 0;
            this.gbTitle.UseLabelBorder = true;
            this.gbTitle.UseTitle = true;
            // 
            // SetupPWA500BINLoadPortOptions
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.Controls.Add(this.gbTitle);
            this.Margin = new System.Windows.Forms.Padding(0);
            this.Name = "SetupPWA500BINLoadPortOptions";
            this.Size = new System.Drawing.Size(170, 275);
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel2.ResumeLayout(false);
            this.gbTitle.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel2;
        private Sys3Controls.Sys3Label sys3Label5;
        private FrameOfSystem3.Component.CustomParameterToggleButton toggleUseLoadPort;
        private FrameOfSystem3.Component.CustomParameterLabel lblWaferType;
        private Sys3Controls.Sys3Label sys3Label1;
        private Sys3Controls.Sys3GroupBoxContainer gbTitle;
    }
}
